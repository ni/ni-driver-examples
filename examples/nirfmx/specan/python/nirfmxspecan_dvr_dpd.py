r"""Steps:
1. Read waveform from TDMS file.
2. Open RFSG session and configure frequency reference.
3. Configure marker0 to be generated from RFSG on the specified output terminal.
4. Configure RFSG center frequency, power level, and external gain.
5. Write the reference waveform to RFSG, set waveform properties, write script, and initiate generation.
6. Open RFmx session.
7. Configure frequency reference, selected ports, trigger, and RF.
8. Select IDPD measurement, configure the reference waveform.
9. Configure the equalizer coefficients for 'Hold' mode of Equalizer.
10. Set power of configured reference signal at the input of the DUT.
    Set the measurement sample rate and the measurement interval to use for analysis.
    Set equalizer mode.
    Configure EVM Enabled with EVM unit.
    Set start and stop for impairment estimation and synchronization estimation.
    Configure gain expansion (dB) and power linearity tradeoff (%).
11. Perform Auto Level to compute an approximate reference level.
12. Iterate IDPD measurement: configure predistorted waveform, initiate, fetch predistorted waveform,
    normalize, abort RFSG, write predistorted waveform to RFSG, and re-initiate generation.
13. Select DPD measurement. Configure the reference waveform.
    Set power of configured reference signal at the input of the DUT.
    Select and configure the Decomposed Vector Rotation model.
    Configure DPD measurement mode to 'Extract Only'.
    Configure IDPD predistorted waveform as target waveform.
    Initiate measurement and apply digital predistortion.
14. Abort RFSG, write DPD-predistorted waveform to RFSG, and re-initiate generation.
15. Select and configure AMPM measurement.
16. Initiate and fetch AMPM results.
17. Abort RFSG and clear waveform.
18. Close sessions.

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
    center_frequency = 1.0e9       # Hz
    rfsa_external_attenuation = 0.0  # dB
    rfsg_external_attenuation = 0.0  # dB
    pre_filter_gain = -1.5           # dB

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
    target_gain = 20.0               # dB

    # IDPD settings
    idpd_idle_duration_present = nirfmxspecan.IdpdReferenceWaveformIdleDurationPresent.FALSE
    idpd_signal_type = nirfmxspecan.IdpdSignalType.MODULATED
    idpd_equalizer_mode = nirfmxspecan.IdpdEqualizerMode.OFF
    idpd_sample_rate_mode = nirfmxspecan.IdpdMeasurementSampleRateMode.REFERENCE_WAVEFORM
    idpd_sample_rate = 120.0e6                    # samples/s
    idpd_evm_enabled = nirfmxspecan.IdpdEvmEnabled.TRUE
    idpd_evm_unit = nirfmxspecan.IdpdEvmUnit.DB
    idpd_gain_expansion = 3.0                     # dB
    idpd_power_linearity_tradeoff = 50.0          # %
    idpd_start_time = 0.0                         # seconds
    idpd_stop_time = 100.0e-6                     # seconds
    idpd_number_of_iterations = 10

    # DPD DVR settings
    dpd_idle_duration_present = nirfmxspecan.DpdReferenceWaveformIdleDurationPresent.FALSE
    dpd_signal_type = nirfmxspecan.DpdSignalType.MODULATED
    dpd_measurement_mode = nirfmxspecan.DpdMeasurementMode.EXTRACT_ONLY
    dpd_model = nirfmxspecan.DpdModel.DECOMPOSED_VECTOR_ROTATION
    dvr_number_of_segments = 4
    dvr_linear_memory_depth = 21
    dvr_nonlinear_memory_depth = 2
    dvr_ddr_enabled = nirfmxspecan.DpdDvrDdrEnabled.TRUE
    nmse_enabled = nirfmxspecan.DpdNmseEnabled.TRUE
    dpd_sample_rate_mode = nirfmxspecan.DpdMeasurementSampleRateMode.REFERENCE_WAVEFORM
    dpd_sample_rate = 120.0e6                     # samples/s
    dpd_measurement_interval = 100.0e-6           # seconds

    # AMPM settings
    ampm_idle_duration_present = nirfmxspecan.AmpmReferenceWaveformIdleDurationPresent.FALSE
    ampm_signal_type = nirfmxspecan.AmpmSignalType.MODULATED
    ampm_sample_rate_mode = nirfmxspecan.AmpmMeasurementSampleRateMode.REFERENCE_WAVEFORM
    ampm_sample_rate = 120.0e6                    # samples/s
    ampm_measurement_interval = 100.0e-6          # seconds
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
        rfsg_session.configure_ref_clock("OnboardClock", frequency_reference_frequency)
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

        # --- IDPD configuration ---
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.IDPD, True)
        specan.idpd.configuration.configure_reference_waveform(
            "", ref_x0, ref_dx, reference_waveform_iq, idpd_idle_duration_present, idpd_signal_type
        )
        specan.idpd.configuration.configure_equalizer_coefficients(
            "", 0.0, 0.0, numpy.array([], dtype=numpy.complex64)
        )
        specan.idpd.configuration.set_dut_average_input_power("", dut_average_input_power)
        specan.idpd.configuration.set_measurement_sample_rate_mode("", idpd_sample_rate_mode)
        specan.idpd.configuration.set_measurement_sample_rate("", idpd_sample_rate)
        specan.idpd.configuration.set_equalizer_mode("", idpd_equalizer_mode)
        specan.idpd.configuration.set_evm_enabled("", idpd_evm_enabled)
        specan.idpd.configuration.set_evm_unit("", idpd_evm_unit)
        specan.idpd.configuration.set_gain_expansion("", idpd_gain_expansion)
        specan.idpd.configuration.set_power_linearity_tradeoff("", idpd_power_linearity_tradeoff)
        specan.idpd.configuration.set_impairment_estimation_start("", idpd_start_time)
        specan.idpd.configuration.set_impairment_estimation_stop("", idpd_stop_time)
        specan.idpd.configuration.set_synchronization_estimation_start("", idpd_start_time)
        specan.idpd.configuration.set_synchronization_estimation_stop("", idpd_stop_time)

        signal_bandwidth = 0.8 * rfsg_iq_rate
        auto_level_measurement_interval = ref_dx * len(reference_waveform_iq)
        reference_level_auto, error_code = specan.auto_level("", signal_bandwidth, auto_level_measurement_interval)
        specan.configure_reference_level("", reference_level_auto + idpd_gain_expansion)

        # --- IDPD iteration loop ---
        idpd_mean_rms_evm = []
        predistorted_waveform = numpy.empty(0, dtype=numpy.complex64)
        pred_x0 = ref_x0
        pred_dx = ref_dx

        for _ in range(idpd_number_of_iterations):
            specan.idpd.configuration.configure_predistorted_waveform(
                "", pred_x0, pred_dx, predistorted_waveform, target_gain
            )
            specan.initiate("", "")
            waveform_out = numpy.zeros(len(reference_waveform_iq), dtype=numpy.complex64)
            pred_x0, pred_dx, papr, power_offset, gain, error_code = (
                specan.idpd.results.fetch_predistorted_waveform("", timeout, waveform_out)
            )
            evm, error_code = specan.idpd.results.get_mean_rms_evm("")
            idpd_mean_rms_evm.append(evm)
            target_gain = gain
            predistorted_waveform = _normalize_complex_waveform(waveform_out)

            rfsg_session.abort()
            rfsg_session.clear_arb_waveform(waveform_name)
            rfsg_iq_rate = 1.0 / pred_dx
            rfsg_session.write_arb_waveform(waveform_name, predistorted_waveform)
            rfsg_session.waveforms[waveform_name].waveform_runtime_scaling = pre_filter_gain
            rfsg_session.waveforms[waveform_name].waveform_iq_rate = rfsg_iq_rate
            rfsg_session.waveforms[waveform_name].waveform_papr = papr + power_offset
            rfsg_session.waveforms[waveform_name].waveform_signal_bandwidth = 0.8 * rfsg_iq_rate
            rfsg_session.write_script(waveform_script)
            rfsg_session.selected_script = script_name
            rfsg_session.initiate()

        # --- DPD DVR configuration ---
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.DPD, True)
        specan.dpd.configuration.configure_reference_waveform(
            "", ref_x0, ref_dx, reference_waveform_iq, dpd_idle_duration_present, dpd_signal_type
        )
        specan.dpd.configuration.set_dut_average_input_power("", dut_average_input_power)
        specan.dpd.configuration.set_measurement_mode("", dpd_measurement_mode)
        specan.dpd.configuration.set_model("", dpd_model)
        specan.dpd.configuration.set_dvr_number_of_segments("", dvr_number_of_segments)
        specan.dpd.configuration.set_dvr_linear_memory_depth("", dvr_linear_memory_depth)
        specan.dpd.configuration.set_dvr_nonlinear_memory_depth("", dvr_nonlinear_memory_depth)
        specan.dpd.configuration.set_dvr_ddr_enabled("", dvr_ddr_enabled)
        specan.dpd.configuration.set_nmse_enabled("", nmse_enabled)
        specan.dpd.configuration.set_measurement_sample_rate_mode("", dpd_sample_rate_mode)
        specan.dpd.configuration.set_measurement_sample_rate("", dpd_sample_rate)
        specan.dpd.configuration.set_measurement_interval("", dpd_measurement_interval)
        specan.dpd.configuration.configure_extract_model_target_waveform(
            "", pred_x0, pred_dx, predistorted_waveform
        )

        specan.initiate("", "")

        waveform_out = numpy.zeros(len(reference_waveform_iq), dtype=numpy.complex64)
        x0_out, dx_out, dpd_papr, dpd_power_offset, error_code = (
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

        dvr_nmse, error_code = specan.dpd.results.fetch_nmse("", timeout)
        dvr_model = numpy.zeros(0, dtype=numpy.complex64)
        specan.dpd.results.fetch_dvr_model("", timeout, dvr_model)

        dpd_waveform = _normalize_complex_waveform(waveform_out)
        rfsg_session.abort()
        rfsg_session.clear_arb_waveform(waveform_name)
        rfsg_iq_rate = 1.0 / dx_out
        rfsg_session.write_arb_waveform(waveform_name, dpd_waveform)
        rfsg_session.waveforms[waveform_name].waveform_runtime_scaling = pre_filter_gain
        rfsg_session.waveforms[waveform_name].waveform_iq_rate = rfsg_iq_rate
        rfsg_session.waveforms[waveform_name].waveform_papr = dpd_papr + dpd_power_offset
        rfsg_session.waveforms[waveform_name].waveform_signal_bandwidth = 0.8 * rfsg_iq_rate
        rfsg_session.write_script(waveform_script)
        rfsg_session.selected_script = script_name
        rfsg_session.initiate()

        # --- AMPM configuration ---
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.AMPM, True)
        specan.ampm.configuration.configure_reference_waveform(
            "", ref_x0, ref_dx, reference_waveform_iq, ampm_idle_duration_present, ampm_signal_type
        )
        specan.ampm.configuration.set_dut_average_input_power("", dut_average_input_power)
        specan.ampm.configuration.set_measurement_sample_rate_mode("", ampm_sample_rate_mode)
        specan.ampm.configuration.set_measurement_sample_rate("", ampm_sample_rate)
        specan.ampm.configuration.set_measurement_interval("", ampm_measurement_interval)
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

        print("\n-----------------IDPD Measurement-----------------\n")
        print("IDPD RMS EVM Mean (% or dB) per iteration:")
        for evm in idpd_mean_rms_evm:
            print(evm)

        print("\n-----------------DPD Measurement------------------\n")
        print(f"DVR NMSE (dB)                    {dvr_nmse}")
        print("\nDVR Model Coefficients")
        for coeff in dvr_model:
            print(coeff)

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
        description="Pass arguments for DVR DPD Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr.")
    parser.add_argument("-sn", "--rfsg-resource-name", default="RFSG", help="Resource name of NI-RFSG.")
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    parser.add_argument(
        "-w",
        "--reference-waveform-file",
        default=_DEFAULT_WAVEFORM_FILE,
        help="Path to the reference waveform TDMS file.",
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
