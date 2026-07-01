r"""Steps:
1. Read waveform from TDMS file.
2. Open RFSG session and configure frequency reference.
3. Configure RFSG marker0 export to PXI trigger line.
4. Configure RFSG center frequency, power, power type, and external gain.
5. Open RFmx session.
6. Configure frequency reference.
7. Configure Selected Ports and digital edge trigger.
8. Configure center frequency and external attenuation.
9. Select IDPD measurement.
10. Configure reference waveform, DUT input power, and Equalizer Mode = Train.
11. Configure Measurement Sample Rate and synchronization estimation interval.
12. Commit IDPD measurement to get equalizer training waveform.
13. Fetch equalizer training waveform, normalize, and write to RFSG.
14. Configure RFSG waveform metadata and script, then initiate RFSG generation.
15. Auto Level.
16. Initiate IDPD measurement.
17. Fetch trained equalizer coefficients.
18. Close sessions.

Note: Full deployment requires NIRfsg + NIRfsgPlayback and a TDMS waveform file.
"""

import argparse
import os
import sys

import numpy
from nptdms import TdmsFile
import nirfsg

import nirfmxspecan

import nirfmxinstr


_DEFAULT_WAVEFORM_FILE = os.path.join(
    os.path.dirname(os.path.abspath(__file__)),
    "Support",
    "LTE20MHz Waveform (Two Subframes).tdms",
)


def _normalize_complex_waveform(waveform):
    peak = numpy.max(numpy.abs(waveform)) if waveform.size > 0 else 0.0
    if peak <= 0.0:
        return waveform
    return (waveform / peak).astype(numpy.complex64)


def example(resource_name, rfsg_resource_name, option_string, reference_waveform_file):
    """Run Example.

    Note: Full deployment requires an NIRfsg session + TDMS waveform file.
    """
    selected_ports = ""
    center_frequency = 1.0e9     # Hz
    reference_level = 0.0        # dBm
    rfsa_external_attenuation = 0.0   # dB
    rfsg_external_attenuation = 0.0   # dB
    pre_filter_gain = -1.5  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10.0e6  # Hz

    enable_trigger = True
    digital_edge_source = "PXI_Trig0"
    trigger_delay = 0.0  # seconds
    trigger_type = nirfmxspecan.TriggerType.DIGITAL_EDGE
    iq_power_edge_trigger_source = "0"
    iq_power_edge_trigger_level = -20.0  # dB or dBm
    minimum_quiet_time_duration = 0.0  # seconds

    dut_average_input_power = -20.0  # dBm

    idle_duration_present = nirfmxspecan.IdpdReferenceWaveformIdleDurationPresent.FALSE
    signal_type = nirfmxspecan.IdpdSignalType.MODULATED

    # Equalizer mode = Train (fixed for equalizer training workflow)
    equalizer_mode = nirfmxspecan.IdpdEqualizerMode.TRAIN

    sample_rate_mode = nirfmxspecan.IdpdMeasurementSampleRateMode.REFERENCE_WAVEFORM
    sample_rate = 120.0e6  # samples/s

    start_time = 0.0      # seconds
    stop_time = 100.0e-6  # seconds

    auto_level_bandwidth = 20.0e6    # Hz
    auto_level_interval = 100.0e-6  # seconds

    timeout = 10.0  # seconds
    waveform_name = "Wfm"
    script_name = "IDPDScript"
    marker_index = "0"

    instr_session = None
    specan = None
    rfsg_session = None

    try:
        with TdmsFile.open(reference_waveform_file) as tdms_file:
            group = tdms_file.groups()[0]
            channel = group.channels()[0]
            raw = channel[:].astype(numpy.float32)
            i_data = raw[0::2]
            q_data = raw[1::2]
            reference_waveform_iq = (i_data + 1j * q_data).astype(numpy.complex64)
            ref_x0 = float(channel.properties.get("wf_start_offset", 0.0))
            ref_dx = float(channel.properties.get("wf_increment", 1.0 / 30.72e6))

        rfsg_session = nirfsg.Session(rfsg_resource_name, options=option_string)
        rfsg_session.configure_ref_clock("OnboardClock", 10.0e6)
        rfsg_session.markers[marker_index].exported_marker_event_output_terminal = "PXI_Trig0"
        rfsg_session.configure_rf(center_frequency, dut_average_input_power)
        rfsg_session.power_level_type = nirfsg.PowerLevelType.PEAK
        rfsg_session.external_gain = -rfsg_external_attenuation

        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)
        specan.set_selected_ports("", selected_ports)
        if trigger_type == nirfmxspecan.TriggerType.DIGITAL_EDGE:
            specan.configure_digital_edge_trigger(
                "", digital_edge_source, nirfmxspecan.DigitalEdgeTriggerEdge.RISING_EDGE, trigger_delay, enable_trigger
            )
        elif trigger_type == nirfmxspecan.TriggerType.IQ_POWER_EDGE:
            specan.configure_iq_power_edge_trigger(
                "",
                iq_power_edge_trigger_source,
                iq_power_edge_trigger_level,
                nirfmxspecan.IQPowerEdgeTriggerSlope.RISING_SLOPE,
                trigger_delay,
                nirfmxspecan.TriggerMinimumQuietTimeMode.MANUAL,
                minimum_quiet_time_duration,
                enable_trigger,
            )
        specan.configure_rf("", center_frequency, reference_level, rfsa_external_attenuation)

        specan.select_measurements("", nirfmxspecan.MeasurementTypes.IDPD, True)
        specan.idpd.configuration.configure_reference_waveform(
            "", ref_x0, ref_dx, reference_waveform_iq, idle_duration_present, signal_type
        )
        specan.idpd.configuration.set_equalizer_mode("", equalizer_mode)
        specan.idpd.configuration.set_dut_average_input_power("", dut_average_input_power)
        specan.idpd.configuration.set_measurement_sample_rate_mode("", sample_rate_mode)
        specan.idpd.configuration.set_measurement_sample_rate("", sample_rate)

        # Commit to get equalizer training waveform
        specan.commit("")
        eq_ref_waveform = numpy.array([], dtype=numpy.complex64)
        eq_x0, eq_dx, eq_papr, error_code = specan.idpd.results.get_equalizer_reference_waveform(
            "", eq_ref_waveform
        )
        normalized_eq_waveform = _normalize_complex_waveform(eq_ref_waveform)
        rfsg_iq_rate = 1.0 / eq_dx

        rfsg_session.write_arb_waveform(waveform_name, normalized_eq_waveform)
        rfsg_session.waveforms[waveform_name].waveform_runtime_scaling = pre_filter_gain
        rfsg_session.waveforms[waveform_name].waveform_iq_rate = rfsg_iq_rate
        rfsg_session.waveforms[waveform_name].waveform_papr = eq_papr
        rfsg_session.waveforms[waveform_name].waveform_signal_bandwidth = 0.8 * rfsg_iq_rate

        rfsg_session.generation_mode = nirfsg.GenerationMode.SCRIPT
        waveform_script = (
            f"script {script_name}\n"
            f"repeat forever\n"
            f"generate {waveform_name} marker{marker_index}(0)\n"
            "end repeat\n"
            "end script"
        )
        rfsg_session.write_script(waveform_script)
        rfsg_session.selected_script = script_name
        rfsg_session.initiate()

        reference_level, error_code = specan.auto_level("", auto_level_bandwidth, auto_level_interval)

        specan.initiate("", "")
        equalizer_coefficients = numpy.array([], dtype=numpy.complex64)
        coeff_x0, coeff_dx, error_code = specan.idpd.results.fetch_equalizer_coefficients(
            "", timeout, equalizer_coefficients
        )
        print(f"x0 : {coeff_x0}")
        print(f"dx : {coeff_dx}")
        print("Equalizer Coefficients: ")
        for value in equalizer_coefficients:
            print(value)

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        if rfsg_session is not None:
            try:
                rfsg_session.abort()
            except Exception:
                pass
            try:
                rfsg_session.clear_arb_waveform(waveform_name)
            except Exception:
                pass
            rfsg_session.close()
            rfsg_session = None
        if specan is not None:
            specan.dispose()
            specan = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    parser = argparse.ArgumentParser(
        description="Pass arguments for IDPD Equalizer Train Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr.")
    parser.add_argument("-sn", "--rfsg-resource-name", default="RFSG", help="Resource name of NI-RFSG.")
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    parser.add_argument(
        "-wf",
        "--reference-waveform-file",
        default=_DEFAULT_WAVEFORM_FILE,
        help="Path to TDMS reference waveform file",
    )
    args = parser.parse_args(argsv)
    example(args.resource_name, args.rfsg_resource_name, args.option_string, args.reference_waveform_file)


def main():
    _main(sys.argv[1:])


def test_main():
    _main(["--option-string", ""])


def test_example():
    example("RFSA", "RFSG", "", _DEFAULT_WAVEFORM_FILE)


if __name__ == "__main__":
    main()
