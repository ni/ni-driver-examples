r"""Steps:
1. Read waveform from TDMS file.
2. Open RFSG session and configure frequency reference.
3. Configure RFSG marker0 export to PXI trigger line.
4. Configure RFSG center frequency, power level, and external gain.
5. Write the reference waveform to RFSG, set waveform properties, write script, and initiate generation.
6. Open RFmx session.
7. Configure frequency reference, selected ports, trigger, and RF.
8. Select DPD measurement, configure reference waveform and DUT Average Input Power.
9. Configure DPD Model (Memory Polynomial or Generalized Memory Polynomial).
10. Configure Measurement Sample Rate and Interval.
11. Enable Iterative DPD.
12. Auto Level.
13. Configure Memory Model Correction Type.
14. Iterate: configure previous DPD polynomial, initiate, apply DPD polynomial,
    update RFSG waveform, fetch DPD polynomial.
15. Auto Level on predistorted waveform.
16. Select and configure AMPM measurement. Initiate and fetch results.
17. Close sessions.

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


def example(resource_name, rfsg_resource_name, option_string, reference_waveform_file):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9      # Hz
    rfsa_external_attenuation = 0.0   # dB
    rfsg_external_attenuation = 0.0   # dB
    pre_filter_gain = -4.0        # dB

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
    idle_duration_present = nirfmxspecan.DpdReferenceWaveformIdleDurationPresent.FALSE
    signal_type = nirfmxspecan.DpdSignalType.MODULATED

    dpd_model = nirfmxspecan.DpdModel.MEMORY_POLYNOMIAL
    memory_polynomial_order = 3
    memory_polynomial_depth = 2
    memory_polynomial_lead_order = 2
    memory_polynomial_lag_order = 2
    memory_polynomial_lead_memory_depth = 2
    memory_polynomial_lag_memory_depth = 2
    memory_polynomial_max_lead = 2
    memory_polynomial_max_lag = 2

    iterative_dpd_enabled = nirfmxspecan.DpdIterativeDpdEnabled.FALSE
    number_of_iterations = 3
    memory_model_correction_type = nirfmxspecan.DpdApplyDpdMemoryModelCorrectionType.MAGNITUDE_AND_PHASE

    sample_rate_mode = nirfmxspecan.DpdMeasurementSampleRateMode.REFERENCE_WAVEFORM
    sample_rate = 120.0e6      # samples/s
    measurement_interval = 100.0e-6  # seconds
    signal_bandwidth = 20.0e6  # Hz
    threshold_level = -20.0    # dB
    auto_level_interval = 100.0e-6  # seconds

    ampm_reference_power_type = nirfmxspecan.AmpmReferencePowerType.INPUT

    timeout = 10.0  # seconds
    waveform_name = "Wfm"
    script_name = "DPDScript"
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
        rfsg_session.external_gain = -rfsg_external_attenuation

        rfsg_iq_rate = 1.0 / ref_dx
        rfsg_session.write_arb_waveform(waveform_name, reference_waveform_iq)
        rfsg_session.waveforms[waveform_name].waveform_runtime_scaling = pre_filter_gain
        rfsg_session.waveforms[waveform_name].waveform_iq_rate = rfsg_iq_rate
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
        specan.configure_rf("", center_frequency, 0.0, rfsa_external_attenuation)

        specan.select_measurements("", nirfmxspecan.MeasurementTypes.DPD, True)
        specan.dpd.configuration.configure_reference_waveform(
            "", ref_x0, ref_dx, reference_waveform_iq, idle_duration_present, signal_type
        )
        specan.dpd.configuration.configure_dut_average_input_power("", dut_average_input_power)
        specan.dpd.configuration.configure_dpd_model("", dpd_model)
        specan.dpd.configuration.configure_memory_polynomial("", memory_polynomial_order, memory_polynomial_depth)
        specan.dpd.configuration.configure_generalized_memory_polynomial_cross_terms(
            "", memory_polynomial_lead_order, memory_polynomial_lag_order,
            memory_polynomial_lead_memory_depth, memory_polynomial_lag_memory_depth,
            memory_polynomial_max_lead, memory_polynomial_max_lag,
        )
        specan.dpd.configuration.configure_measurement_sample_rate("", sample_rate_mode, sample_rate)
        specan.dpd.configuration.configure_measurement_interval("", measurement_interval)
        specan.dpd.configuration.configure_iterative_dpd_enabled("", iterative_dpd_enabled)

        specan.auto_level("", signal_bandwidth, auto_level_interval)

        specan.dpd.apply_dpd.configure_memory_model_correction_type("", memory_model_correction_type)

        if iterative_dpd_enabled == nirfmxspecan.DpdIterativeDpdEnabled.FALSE:
            number_of_iterations = 1

        dpd_polynomial = numpy.zeros(0, dtype=numpy.complex64)
        waveform_out = numpy.zeros(len(reference_waveform_iq), dtype=numpy.complex64)

        for _ in range(number_of_iterations):
            specan.dpd.configuration.configure_previous_dpd_polynomial("", dpd_polynomial)
            specan.initiate("", "")
            x0_out, dx_out, papr, power_offset, error_code = (
                specan.dpd.apply_dpd.apply_digital_predistortion(
                    "",
                    ref_x0,
                    ref_dx,
                    reference_waveform_iq,
                    nirfmxspecan.DpdApplyDpdIdleDurationPresent.FALSE,
                    timeout,
                    waveform_out,
                )
            )
            rfsg_session.abort()
            rfsg_session.clear_arb_waveform(waveform_name)
            rfsg_iq_rate = 1.0 / dx_out
            rfsg_session.write_arb_waveform(waveform_name, waveform_out)
            rfsg_session.waveforms[waveform_name].waveform_runtime_scaling = pre_filter_gain
            rfsg_session.waveforms[waveform_name].waveform_iq_rate = rfsg_iq_rate
            rfsg_session.waveforms[waveform_name].waveform_papr = papr + power_offset
            rfsg_session.waveforms[waveform_name].waveform_signal_bandwidth = 0.8 * rfsg_iq_rate
            rfsg_session.write_script(waveform_script)
            rfsg_session.selected_script = script_name
            rfsg_session.initiate()

            error_code = specan.dpd.results.fetch_dpd_polynomial("", timeout, dpd_polynomial)

        specan.auto_level("", signal_bandwidth, auto_level_interval)

        specan.select_measurements("", nirfmxspecan.MeasurementTypes.AMPM, True)
        specan.ampm.configuration.configure_measurement_sample_rate(
            "", nirfmxspecan.AmpmMeasurementSampleRateMode.REFERENCE_WAVEFORM, sample_rate
        )
        specan.ampm.configuration.configure_measurement_interval("", measurement_interval)
        specan.ampm.configuration.configure_reference_waveform(
            "", ref_x0, ref_dx, reference_waveform_iq,
            nirfmxspecan.AmpmReferenceWaveformIdleDurationPresent.FALSE,
            nirfmxspecan.AmpmSignalType.MODULATED,
        )
        specan.ampm.configuration.set_dut_average_input_power("", dut_average_input_power)
        specan.ampm.configuration.configure_threshold(
            "", nirfmxspecan.AmpmThresholdEnabled.TRUE, threshold_level, nirfmxspecan.AmpmThresholdType.RELATIVE
        )
        specan.ampm.configuration.set_reference_power_type("", ampm_reference_power_type)

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

        rfsg_session.abort()
        rfsg_session.clear_arb_waveform(waveform_name)

        print("\n-----------------Measurement-----------------\n")
        print(f"Mean Linear Gain (dB)            {mean_linear_gain}")
        print(f"Mean Phase Error (deg)           {mean_phase_error}")
        print(f"Mean RMS EVM (%)                 {mean_rms_evm}")
        print(f"AM to AM Residual (dB)           {am_to_am_residual}")
        print(f"AM to PM Residual (deg)          {am_to_pm_residual}")
        print(f"Gain Error Range (dB)            {gain_error_range}")
        print(f"Phase Error Range (deg)          {phase_error_range}")
        print(f"1 dB Compression Point (dBm)     {one_db_compression_point}")

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
        description="Pass arguments for Memory DPD Example",
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