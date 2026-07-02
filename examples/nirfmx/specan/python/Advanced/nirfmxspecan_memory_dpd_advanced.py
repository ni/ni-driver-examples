r"""Steps:
1. Open RFSG session.
2. Configure RFSG frequency reference, generation mode to Script.
3. Configure marker0 to be generated from RFSG on the specified output terminal.
4. Configure frequency and power level of RF output signal.
5. Configure RFSG power level type.
6. Set RFSG External Gain. #4 and #5 ensure that the average power of the signal at the input of the DUT
   matches the user configured DUT Average Input Power.
7. a. Read waveform from file.
   b. Write input waveform on RFSG device.
      Configure RFSG IQ rate, Pre-filter Gain and PAPR.
      Read waveform sample rate, multiply by 0.8 and set the result to the RFSG signal bandwidth.
      Write script to generate the waveform specified in the script. This script is programmed
      to generate waveform continuously, with marker0 aligned to sample index 0.
8. Initiate generation.
9. Open RFmx session.
10. Configure frequency reference of the analyser.
11. Configure Selected Ports.
12. Configure trigger to use as reference for signal acquisition.
13. Configure center frequency and external attenuation.
14. Select DPD measurement.
15. Configure pre-DPD CFR.
16. Configure waveform settings for pre-DPD CFR with filtering.
17. Apply pre-DPD CFR.
18. Retrieve the waveform PAPR.
19. Configure the reference waveform.
20. Configure power of the signal at the input of the DUT. Select and configure the Memory
    polynomial or Generalized memory polynomial model and its parameters to estimate the predistortor.
21. Set the measurement sample rate and the measurement interval to use for analysis.
22. Enable iterative DPD.
23. Configure DPD NMSE Enabled.
24. Configure the Memory models Correction type.
25. Configure apply DPD CFR settings before calling RFmx initiate.
    This is because these settings are used by measurement when performing iterative DPD.
26. Perform Auto Level to compute an approximate reference level to use by the analyser.
27. Set the previous iteration polynomial, in case DPD is measured iteratively.
28. Initiates DPD measurement and then configure Apply Digital Predistortion to remove the
    effects of memory and nonlinearity introduced by the DUT.
29. a. Fetch DPD Polynomial.
    b. Fetch NMSE (dB).
30. Abort RFSG generation and write a new waveform that is predistorted by applying memory polynomial coefficients.
    Set RFSG Pre-filter Gain and sample rate computed from Apply Digital Predistortion.
    Set the final PAPR to the sum of the actual PAPR and the Power Offset as computed by Apply Digital Predistortion.
    Set the RFSG signal bandwidth by reading the waveform sample rate and multiplying it by 0.8.
    Initiate RFSG generation using the script that was selected earlier.
31. Perform Auto Level to compute an approximate reference level while generating the predistorted waveform.
32. Select and configure AMPM measurement in RFmx after DPD measurement is complete.
    AMPM measurement is used to inspect the measure the AM-AM and AM-PM response of the DUT.
33. Initiate and fetch AMPM results.
34. Close RFmx session.
35. Close RFSG session.
    It is recommended to clear the waveform before closing RFSG session.

Prerequisites:
- `nirfsg`
- `numpy`
- `nptdms` (`pip install nptdms`)
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
    os.path.dirname(os.path.dirname(os.path.abspath(__file__))),
    "Support",
    "LTE20MHz Waveform (Two Subframes).tdms",
)

def _compute_papr_db(waveform):
    """Compute PAPR in dB for complex waveform."""
    power = numpy.abs(waveform) ** 2
    peak = float(numpy.max(power))
    avg = float(numpy.mean(power))
    if avg <= 0.0:
        return 0.0
    return 10.0 * numpy.log10(peak / avg)


def example(resource_name, rfsg_resource_name, option_string, reference_waveform_file):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9  # Hz
    reference_level = 0.0  # dBm
    rfsa_external_attenuation = 0.0  # dB
    rfsg_external_attenuation = 0.0  # dB
    pre_filter_gain = -4.0  # dB

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
    signal_bandwidth = 20.0e6  # Hz
    auto_level_interval = 100.0e-6  # seconds

    idle_duration_present = nirfmxspecan.DpdReferenceWaveformIdleDurationPresent.FALSE
    signal_type = nirfmxspecan.DpdSignalType.MODULATED

    dpd_model = nirfmxspecan.DpdModel.MEMORY_POLYNOMIAL
    memory_polynomial_order = 3
    memory_polynomial_depth = 2
    cross_terms_lead_order = 2
    cross_terms_lag_order = 2
    cross_terms_lead_memory_depth = 2
    cross_terms_lag_memory_depth = 2
    cross_terms_maximum_lead = 2
    cross_terms_maximum_lag = 2

    sample_rate_mode = nirfmxspecan.DpdMeasurementSampleRateMode.REFERENCE_WAVEFORM
    sample_rate = 120.0e6  # samples/s
    measurement_interval = 100.0e-6  # seconds

    iterative_dpd_enabled = nirfmxspecan.DpdIterativeDpdEnabled.FALSE
    number_of_iterations = 3
    nmse_enabled = nirfmxspecan.DpdNmseEnabled.FALSE
    memory_model_correction_type = (
        nirfmxspecan.DpdApplyDpdMemoryModelCorrectionType.MAGNITUDE_AND_PHASE
    )

    # Pre-DPD CFR
    pre_dpd_cfr_enabled = nirfmxspecan.DpdPreDpdCfrEnabled.FALSE
    pre_dpd_cfr_method = nirfmxspecan.DpdPreDpdCfrMethod.CLIPPING
    pre_dpd_cfr_max_iterations = 10
    pre_dpd_cfr_target_papr = 8.0  # dB
    pre_dpd_cfr_window_type = nirfmxspecan.DpdPreDpdCfrWindowType.KAISER_BESSEL
    pre_dpd_cfr_window_length = 10
    pre_dpd_cfr_shaping_factor = 5.0
    pre_dpd_cfr_shaping_threshold = -5.0  # dB
    pre_dpd_cfr_filter_enabled = nirfmxspecan.DpdPreDpdCfrFilterEnabled.FALSE
    pre_dpd_carrier_offsets = [0.0]  # Hz
    pre_dpd_carrier_bandwidths = [20.0e6]  # Hz

    # Apply DPD CFR
    apply_dpd_cfr_enabled = nirfmxspecan.DpdApplyDpdCfrEnabled.FALSE
    apply_dpd_cfr_method = nirfmxspecan.DpdApplyDpdCfrMethod.CLIPPING
    apply_dpd_cfr_max_iterations = 10
    apply_dpd_cfr_target_papr_type = nirfmxspecan.DpdApplyDpdCfrTargetPaprType.INPUT_PAPR
    apply_dpd_cfr_target_papr = 8.0  # dB
    apply_dpd_cfr_window_type = nirfmxspecan.DpdApplyDpdCfrWindowType.KAISER_BESSEL
    apply_dpd_cfr_window_length = 10
    apply_dpd_cfr_shaping_factor = 5.0
    apply_dpd_cfr_shaping_threshold = -5.0  # dB

    # AMPM
    threshold_level = -20.0
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
        rfsg_session.generation_mode = nirfsg.GenerationMode.SCRIPT
        rfsg_session.configure_ref_clock("OnboardClock", 10.0e6)
        rfsg_session.markers[marker_index].exported_marker_event_output_terminal = "PXI_Trig0"
        rfsg_session.configure_rf(center_frequency, dut_average_input_power)
        rfsg_session.power_level_type = nirfsg.PowerLevelType.PEAK
        rfsg_session.external_gain = -rfsg_external_attenuation

        waveform_script = (
            f"script {script_name}\n"
            "repeat forever\n"
            f"generate {waveform_name} marker{marker_index}(0)\n"
            "end repeat\n"
            "end script"
        )

        waveform_for_dpd = reference_waveform_iq
        waveform_x0 = ref_x0
        waveform_dx = ref_dx
        waveform_papr = _compute_papr_db(reference_waveform_iq)

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

        specan.select_measurements("", nirfmxspecan.MeasurementTypes.DPD, True)

        # Configure pre-DPD CFR.
        if pre_dpd_cfr_enabled == nirfmxspecan.DpdPreDpdCfrEnabled.TRUE:
            specan.dpd.pre_dpd.set_cfr_enabled("", pre_dpd_cfr_enabled)
            specan.dpd.pre_dpd.set_cfr_method("", pre_dpd_cfr_method)
            specan.dpd.pre_dpd.set_cfr_maximum_iterations("", pre_dpd_cfr_max_iterations)
            specan.dpd.pre_dpd.set_cfr_target_papr("", pre_dpd_cfr_target_papr)
            specan.dpd.pre_dpd.set_cfr_window_type("", pre_dpd_cfr_window_type)
            specan.dpd.pre_dpd.set_cfr_window_length("", pre_dpd_cfr_window_length)
            specan.dpd.pre_dpd.set_cfr_shaping_factor("", pre_dpd_cfr_shaping_factor)
            specan.dpd.pre_dpd.set_cfr_shaping_threshold("", pre_dpd_cfr_shaping_threshold)
            specan.dpd.pre_dpd.set_cfr_filter_enabled("", pre_dpd_cfr_filter_enabled)
            specan.dpd.pre_dpd.set_cfr_number_of_carriers("", len(pre_dpd_carrier_offsets))
            for i in range(len(pre_dpd_carrier_offsets)):
                carrier_string = nirfmxspecan.SpecAn.build_carrier_string("", i)
                specan.dpd.pre_dpd.set_carrier_offset(carrier_string, pre_dpd_carrier_offsets[i])
                specan.dpd.pre_dpd.set_carrier_bandwidth(carrier_string, pre_dpd_carrier_bandwidths[i])

            pre_dpd_waveform = numpy.zeros(len(reference_waveform_iq), dtype=numpy.complex64)
            x0_out, dx_out, papr_out, error_code = specan.dpd.pre_dpd.apply_pre_dpd_signal_conditioning(
                "",
                ref_x0,
                ref_dx,
                reference_waveform_iq,
                nirfmxspecan.DpdApplyDpdIdleDurationPresent.FALSE,
                pre_dpd_waveform,
            )
            waveform_for_dpd = pre_dpd_waveform
            waveform_x0 = x0_out
            waveform_dx = dx_out
            waveform_papr = papr_out

        if pre_dpd_cfr_enabled == nirfmxspecan.DpdPreDpdCfrEnabled.TRUE:
            rfsg_iq_rate = 1.0 / waveform_dx
            rfsg_session.write_arb_waveform(waveform_name, waveform_for_dpd)
            rfsg_session.waveforms[waveform_name].waveform_iq_rate = rfsg_iq_rate
            rfsg_session.waveforms[waveform_name].waveform_papr = waveform_papr
        else:
            rfsg_iq_rate = 1.0 / ref_dx
            rfsg_session.write_arb_waveform(waveform_name, reference_waveform_iq)
            rfsg_session.waveforms[waveform_name].waveform_iq_rate = rfsg_iq_rate
            rfsg_session.waveforms[waveform_name].waveform_papr = waveform_papr
        rfsg_session.waveforms[waveform_name].waveform_runtime_scaling = pre_filter_gain
        rfsg_session.waveforms[waveform_name].waveform_signal_bandwidth = 0.8 * rfsg_iq_rate
        rfsg_session.write_script(waveform_script)
        rfsg_session.selected_script = script_name
        rfsg_session.initiate()

        if pre_dpd_cfr_enabled == nirfmxspecan.DpdPreDpdCfrEnabled.TRUE:
            specan.dpd.configuration.configure_reference_waveform(
                "", waveform_x0, waveform_dx, waveform_for_dpd, idle_duration_present, signal_type
            )
        else:
            specan.dpd.configuration.configure_reference_waveform(
                "", ref_x0, ref_dx, reference_waveform_iq, idle_duration_present, signal_type
            )
        specan.dpd.configuration.configure_dut_average_input_power("", dut_average_input_power)
        specan.dpd.configuration.configure_dpd_model("", dpd_model)
        specan.dpd.configuration.configure_memory_polynomial(
            "", memory_polynomial_order, memory_polynomial_depth
        )
        specan.dpd.configuration.configure_generalized_memory_polynomial_cross_terms(
            "",
            cross_terms_lead_order,
            cross_terms_lag_order,
            cross_terms_lead_memory_depth,
            cross_terms_lag_memory_depth,
            cross_terms_maximum_lead,
            cross_terms_maximum_lag,
        )
        specan.dpd.configuration.configure_measurement_sample_rate("", sample_rate_mode, sample_rate)
        specan.dpd.configuration.configure_measurement_interval("", measurement_interval)
        specan.dpd.configuration.configure_iterative_dpd_enabled("", iterative_dpd_enabled)
        if iterative_dpd_enabled == nirfmxspecan.DpdIterativeDpdEnabled.FALSE:
            number_of_iterations = 1
        specan.dpd.configuration.set_nmse_enabled("", nmse_enabled)

        specan.dpd.apply_dpd.configure_memory_model_correction_type("", memory_model_correction_type)
        specan.dpd.apply_dpd.set_cfr_enabled("", apply_dpd_cfr_enabled)
        specan.dpd.apply_dpd.set_cfr_method("", apply_dpd_cfr_method)
        specan.dpd.apply_dpd.set_cfr_maximum_iterations("", apply_dpd_cfr_max_iterations)
        specan.dpd.apply_dpd.set_cfr_target_papr_type("", apply_dpd_cfr_target_papr_type)
        specan.dpd.apply_dpd.set_cfr_target_papr("", apply_dpd_cfr_target_papr)
        specan.dpd.apply_dpd.set_cfr_window_type("", apply_dpd_cfr_window_type)
        specan.dpd.apply_dpd.set_cfr_window_length("", apply_dpd_cfr_window_length)
        specan.dpd.apply_dpd.set_cfr_shaping_factor("", apply_dpd_cfr_shaping_factor)
        specan.dpd.apply_dpd.set_cfr_shaping_threshold("", apply_dpd_cfr_shaping_threshold)

        reference_level, error_code = specan.auto_level("", signal_bandwidth, auto_level_interval)

        dpd_polynomial = numpy.zeros(0, dtype=numpy.complex64)
        waveform_out = numpy.zeros(len(waveform_for_dpd), dtype=numpy.complex64)

        for _ in range(number_of_iterations):
            specan.dpd.configuration.configure_previous_dpd_polynomial("", dpd_polynomial)
            specan.initiate("", "")

            x0_out, dx_out, papr_out, power_offset, error_code = (
                specan.dpd.apply_dpd.apply_digital_predistortion(
                    "",
                    waveform_x0,
                    waveform_dx,
                    waveform_for_dpd,
                    nirfmxspecan.DpdApplyDpdIdleDurationPresent.FALSE,
                    timeout,
                    waveform_out,
                )
            )

            error_code = specan.dpd.results.fetch_dpd_polynomial("", timeout, dpd_polynomial)
            nmse, error_code = specan.dpd.results.fetch_nmse("", timeout)
            print(f"NMSE            {nmse}")

            rfsg_session.abort()
            rfsg_session.clear_arb_waveform(waveform_name)
            rfsg_iq_rate = 1.0 / dx_out
            rfsg_session.write_arb_waveform(waveform_name, waveform_out)
            rfsg_session.waveforms[waveform_name].waveform_runtime_scaling = pre_filter_gain
            rfsg_session.waveforms[waveform_name].waveform_iq_rate = rfsg_iq_rate
            rfsg_session.waveforms[waveform_name].waveform_papr = papr_out + power_offset
            rfsg_session.waveforms[waveform_name].waveform_signal_bandwidth = 0.8 * rfsg_iq_rate
            rfsg_session.write_script(waveform_script)
            rfsg_session.selected_script = script_name
            rfsg_session.initiate()

        reference_level, error_code = specan.auto_level("", signal_bandwidth, auto_level_interval)

        specan.select_measurements("", nirfmxspecan.MeasurementTypes.AMPM, True)
        specan.ampm.configuration.configure_measurement_sample_rate(
            "", nirfmxspecan.AmpmMeasurementSampleRateMode.REFERENCE_WAVEFORM, sample_rate
        )
        specan.ampm.configuration.configure_measurement_interval("", measurement_interval)
        specan.ampm.configuration.configure_reference_waveform(
            "",
            waveform_x0,
            waveform_dx,
            waveform_for_dpd,
            nirfmxspecan.AmpmReferenceWaveformIdleDurationPresent.FALSE,
            nirfmxspecan.AmpmSignalType.MODULATED,
        )
        specan.ampm.configuration.configure_dut_average_input_power("", dut_average_input_power)
        specan.ampm.configuration.configure_threshold(
            "", nirfmxspecan.AmpmThresholdEnabled.TRUE, -20.0, nirfmxspecan.AmpmThresholdType.RELATIVE
        )
        specan.ampm.configuration.configure_reference_power_type("", ampm_reference_power_type)

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

        print("-----------------AMPM Measurement-----------------\n")
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
        description="Pass arguments for Memory DPD Advanced Example",
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
