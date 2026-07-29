r"""Steps:
1. Open RFSG session.
2. Configure RFSG frequency reference and Generation mode to Script.
3. Configure marker0 to be generated from RFSG on the specified output terminal.
4. Configure frequency and power level of RF output signal.
5. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at the input of the DUT
   matches the user configured DUT Average Input Power.
   Configure RFSG Power Level Type and Upconverter Frequency Offset Mode.
6. Read waveform from file and download Waveform from file to RFSG.
   Configure RFSG IQ Rate and Pre-filter Gain.
   Read waveform sample rate, multiply by 0.8 and set the result to the signal bandwidth.
   Write script to generate the waveform specified in the script. This script is programmed
   to generate waveform continuously, with marker0 aligned to sample index 0.
7. Initiate generation.
8. Open RFmx session.
9. Configure frequency reference of the analyser.
10. Configure Selected Ports.
11. Configure trigger to use as reference for signal acquisition.
12. Configure center frequency and external attenuation.
13. Select IDPD measurement, configure the reference waveform.
14. Configure the equalizer coefficients for 'Hold' mode of Equalizer.
15. Set power of configured reference signal at the input of the DUT.
    Set the measurement sample rate and the measurement interval to use for analysis.
    Set equalizer mode.
    Configure averaging and EVM Enabled with EVM unit.
    Set start and stop for impairment estimation and synchronization estimation.
    Configure gain expansion (dB) and power linearity tradeoff (%).
16. Perform Auto Level to compute an approximate reference level to use by the analyzer and adjust
    the reference level to account for PAPR changes after applying DPD.
17. Configure predistorted waveform obtained from previous iteration.
18. Initiates IDPD measurement.
19. Fetch predistorted waveform and scale from -1 to 1 as RFSG power level type is peak power and
    fetch RMS EVM.
20. Abort RFSG generation and write a new Predistorted Waveform.
    Set Waveform Runtime Scaling to the desired Pre-filter Gain.
    Set the sample rate computed from Predistorted Waveform.
    Set the final PAPR to the sum of the actual PAPR and the Power Offset as computed from
    Predistorted Waveform.
    Set the Signal Bandwidth.
    Initiate RFSG generation using the script that was selected earlier.
21. Configure appropriate trigger delay for AMPM measurement based on burst location in the Waveform.
22. Perform Auto Level to compute an approximate reference level while generating the predistorted waveform.
23. Select and configure AMPM measurement in RFmx after IDPD measurement is complete.
    AMPM measurement is used to measure the AM-AM and AM-PM response of the DUT.
24. Initiate and fetch AMPM results.
25. Close RFmx session.
26. Close RFSG session.
    It is recommended to clear the waveform before closing RFSG session.

Prerequisites:
- nptdms package (`pip install nptdms`)
"""

import argparse
import os
import sys

import nirfsg
import numpy
from nptdms import TdmsFile

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
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9  # Hz
    reference_level = 0.0  # dBm
    rfsa_external_attenuation = 0.0  # dB
    rfsg_external_attenuation = 0.0  # dB
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

    sample_rate_mode = nirfmxspecan.IdpdMeasurementSampleRateMode.REFERENCE_WAVEFORM
    sample_rate = 120.0e6  # samples/s
    number_of_iterations = 10

    equalizer_mode = nirfmxspecan.IdpdEqualizerMode.OFF

    gain_expansion = 3.0  # dB
    power_linearity_tradeoff = 50.0  # %
    start_time = 0.0  # seconds
    stop_time = 100.0e-6  # seconds

    idpd_evm_enabled = nirfmxspecan.IdpdEvmEnabled.TRUE
    evm_unit = nirfmxspecan.IdpdEvmUnit.DB
    target_gain = 20.0           # dB

    auto_level_bandwidth = 20.0e6     # Hz
    auto_level_interval = 100.0e-6   # seconds

    ampm_idle_duration_present = nirfmxspecan.AmpmReferenceWaveformIdleDurationPresent.FALSE
    ampm_signal_type = nirfmxspecan.AmpmSignalType.MODULATED
    ampm_reference_power_type = nirfmxspecan.AmpmReferencePowerType.INPUT
    ampm_threshold_enabled = nirfmxspecan.AmpmThresholdEnabled.TRUE

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
        rfsg_session.write_arb_waveform(waveform_name, reference_waveform_iq)
        rfsg_session.waveforms[waveform_name].waveform_runtime_scaling = pre_filter_gain
        rfsg_session.waveforms[waveform_name].waveform_iq_rate = 1.0 / ref_dx
        rfsg_session.waveforms[waveform_name].waveform_signal_bandwidth = 0.8 / ref_dx
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
        specan.idpd.configuration.configure_equalizer_coefficients(
            "", 0.0, 0.0, numpy.array([], dtype=numpy.complex64)
        )
        specan.idpd.configuration.set_dut_average_input_power("", dut_average_input_power)
        specan.idpd.configuration.set_measurement_sample_rate_mode("", sample_rate_mode)
        specan.idpd.configuration.set_measurement_sample_rate("", sample_rate)
        specan.idpd.configuration.set_equalizer_mode("", equalizer_mode)
        specan.idpd.configuration.set_evm_enabled("", idpd_evm_enabled)
        specan.idpd.configuration.set_evm_unit("", evm_unit)
        specan.idpd.configuration.set_gain_expansion("", gain_expansion)
        specan.idpd.configuration.set_power_linearity_tradeoff("", power_linearity_tradeoff)
        specan.idpd.configuration.set_impairment_estimation_start("", start_time)
        specan.idpd.configuration.set_synchronization_estimation_start("", start_time)
        specan.idpd.configuration.set_impairment_estimation_stop("", stop_time)
        specan.idpd.configuration.set_synchronization_estimation_stop("", stop_time)

        reference_level, error_code = specan.auto_level("", auto_level_bandwidth, auto_level_interval)

        idpd_mean_rms_evm = []
        predistorted_waveform = numpy.array([], dtype=numpy.complex64)
        for _ in range(number_of_iterations):
            specan.idpd.configuration.configure_predistorted_waveform(
                "", 0.0, 0.0, predistorted_waveform, target_gain
            )
            specan.initiate("", "")
            pred_x0, pred_dx, papr, power_offset, gain, error_code = (
                specan.idpd.results.fetch_predistorted_waveform("", timeout, predistorted_waveform)
            )
            mean_rms_evm, error_code = specan.idpd.results.get_mean_rms_evm("")
            idpd_mean_rms_evm.append(mean_rms_evm)

            normalized_pred = _normalize_complex_waveform(predistorted_waveform)
            target_gain = gain

            rfsg_session.abort()
            rfsg_session.clear_arb_waveform(waveform_name)
            rfsg_iq_rate = 1.0 / pred_dx
            rfsg_session.write_arb_waveform(waveform_name, normalized_pred)
            rfsg_session.waveforms[waveform_name].waveform_runtime_scaling = pre_filter_gain
            rfsg_session.waveforms[waveform_name].waveform_iq_rate = rfsg_iq_rate
            rfsg_session.waveforms[waveform_name].waveform_papr = papr + power_offset
            rfsg_session.waveforms[waveform_name].waveform_signal_bandwidth = 0.8 * rfsg_iq_rate
            rfsg_session.write_script(waveform_script)
            rfsg_session.selected_script = script_name
            rfsg_session.initiate()

        specan.set_trigger_delay("", trigger_delay)
        reference_level, error_code = specan.auto_level("", auto_level_bandwidth, auto_level_interval)

        specan.select_measurements("", nirfmxspecan.MeasurementTypes.AMPM, True)
        specan.ampm.configuration.configure_reference_waveform(
            "", ref_x0, ref_dx, reference_waveform_iq, ampm_idle_duration_present, ampm_signal_type
        )
        specan.ampm.configuration.set_dut_average_input_power("", dut_average_input_power)
        specan.ampm.configuration.set_measurement_sample_rate_mode(
            "", nirfmxspecan.AmpmMeasurementSampleRateMode.REFERENCE_WAVEFORM
        )
        specan.ampm.configuration.set_measurement_sample_rate("", sample_rate)
        specan.ampm.configuration.set_measurement_interval("", stop_time - start_time)
        specan.ampm.configuration.set_reference_power_type("", ampm_reference_power_type)
        specan.ampm.configuration.set_threshold_enabled("", ampm_threshold_enabled)

        specan.initiate("", "")

        mean_linear_gain, one_db_compression_point, mean_rms_evm, error_code = (
            specan.ampm.results.fetch_dut_characteristics("", timeout)
        )
        gain_error_range, phase_error_range, mean_phase_error, error_code = (
            specan.ampm.results.fetch_error("", timeout)
        )
        am_to_am_residual, am_to_pm_residual, error_code = (
            specan.ampm.results.fetch_curve_fit_residual("", timeout)
        )
        reference_powers_am_to_am, measured_am_to_am, curve_fit_am_to_am, error_code = (
            specan.ampm.results.fetch_am_to_am_trace("", timeout)
        )
        reference_powers_am_to_pm, measured_am_to_pm, curve_fit_am_to_pm, error_code = (
            specan.ampm.results.fetch_am_to_pm_trace("", timeout)
        )

        print("-----------------AMPM Measurement-----------------\n")
        print(f"Mean Linear Gain (dB)            {mean_linear_gain}")
        print(f"Mean Phase Error (deg)           {mean_phase_error}")
        print(f"Mean RMS EVM (%)                 {mean_rms_evm}")
        print(f"AM to AM Residual (dB)           {am_to_am_residual}")
        print(f"AM to PM Residual (deg)          {am_to_pm_residual}")
        print(f"Gain Error Range (dB)            {gain_error_range}")
        print(f"Phase Error Range (deg)          {phase_error_range}")
        print(f"1 dB Compression Point (dBm)     {one_db_compression_point}")

        print("\n-----------------IDPD Measurement-----------------\n")
        print("IDPD RMS EVM Mean (% or dB) per iteration:")
        for evm in idpd_mean_rms_evm:
            print(evm)

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
        description="Pass arguments for IDPD Example",
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
