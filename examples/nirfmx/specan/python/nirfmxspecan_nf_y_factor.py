r"""Steps:
1. Open a new RFmx session.
2. Configure RF Attenuation.
3. Configure Selected Ports.
4. Select NF measurement.
5. Configure Measurement Method (Y-Factor).
6. Configure Noise Source type.
7. Configure measurement frequencies (Start, Stop, Step or Points or List).
8. Configure Measurement Bandwidth.
9. Configure Measurement Interval.
10. Configure Averaging.
11. Configure Calibration Loss.
12. Configure DUT Input Loss.
13. Configure DUT Output Loss.
14. Configure RF preamplifier.
15. Configure Y-Factor Mode.
16. Configure Y-Factor Noise Source ENR.
17. Configure Y-Factor Noise Source Settling Time.
18. Configure Y-Factor Noise Source Loss.
19. Configure Reference Level (auto-recommend or manual).
20. Initiate the measurement.
21. Fetch NF Measurements.
22. Close RFmx Session.
"""

import argparse
import sys

import nirfmxspecan

import nirfmxinstr


FREQUENCY_LIST_CONFIGURATION_STEP = 0
FREQUENCY_LIST_CONFIGURATION_POINTS = 1
FREQUENCY_LIST_CONFIGURATION_FREQUENCY = 2


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""

    rf_attenuation_auto = nirfmxinstr.RFAttenuationAuto.TRUE
    rf_attenuation = 10.0  # dB

    start_frequency = 1.0e9     # Hz
    stop_frequency = 2.0e9      # Hz
    step_size = 100.0e6         # Hz
    number_of_points = 10

    dut_max_gain = 0.0          # dB
    dut_max_noise_figure = 0.0  # dB
    reference_level = -55.0     # dBm

    frequency_list_configuration = FREQUENCY_LIST_CONFIGURATION_STEP
    manual_reference_level = True

    measurement_method = nirfmxspecan.NFMeasurementMethod.Y_FACTOR
    y_factor_mode = nirfmxspecan.NFYFactorMode.MEASURE
    measurement_bandwidth = 100.0e3  # Hz
    measurement_interval = 1.0e-3   # seconds

    averaging_enabled = nirfmxspecan.NFAveragingEnabled.FALSE
    averaging_count = 10

    noise_source_type = nirfmxspecan.NFYFactorNoiseSourceType.EXTERNAL_NOISE_SOURCE
    noise_source_rfsg_port = ""
    settling_time = 0.0       # seconds
    cold_temperature = 302.8  # K
    off_temperature = 297.0   # K

    dut_input_loss_comp_enabled = nirfmxspecan.NFDutInputLossCompensationEnabled.FALSE
    dut_input_loss_temperature = 297.0  # K

    dut_output_loss_comp_enabled = nirfmxspecan.NFDutOutputLossCompensationEnabled.FALSE
    dut_output_loss_temperature = 297.0  # K

    calibration_loss_comp_enabled = nirfmxspecan.NFCalibrationLossCompensationEnabled.FALSE
    calibration_loss_temperature = 297.0  # K

    noise_source_loss_comp_enabled = nirfmxspecan.NFYFactorNoiseSourceLossCompensationEnabled.FALSE
    noise_source_loss_temperature = 297.0  # K

    preamp = nirfmxinstr.PreampEnabled.ENABLED
    preselector_enabled = nirfmxinstr.DownconverterPreselectorEnabled.ENABLED

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_rf_attenuation("", rf_attenuation_auto, rf_attenuation)
        specan.set_selected_ports("", selected_ports)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.NF, False)
        specan.nf.configuration.configure_measurement_method("", measurement_method)
        specan.nf.configuration.set_y_factor_noise_source_type("", noise_source_type)
        specan.nf.configuration.set_y_factor_noise_source_rf_signal_generator_port("", noise_source_rfsg_port)

        if frequency_list_configuration == FREQUENCY_LIST_CONFIGURATION_STEP:
            specan.nf.configuration.configure_frequency_list_start_stop_step("", start_frequency, stop_frequency, step_size)
        elif frequency_list_configuration == FREQUENCY_LIST_CONFIGURATION_POINTS:
            specan.nf.configuration.configure_frequency_list_start_stop_points("", start_frequency, stop_frequency, number_of_points)
        else:
            specan.nf.configuration.configure_frequency_list("", [start_frequency])

        specan.nf.configuration.configure_measurement_bandwidth("", measurement_bandwidth)
        specan.nf.configuration.configure_measurement_interval("", measurement_interval)
        specan.nf.configuration.configure_averaging("", averaging_enabled, averaging_count)
        specan.nf.configuration.configure_calibration_loss(
            "", calibration_loss_comp_enabled, None, None, calibration_loss_temperature
        )
        specan.nf.configuration.configure_dut_input_loss(
            "", dut_input_loss_comp_enabled, None, None, dut_input_loss_temperature
        )
        specan.nf.configuration.configure_dut_output_loss(
            "", dut_output_loss_comp_enabled, None, None, dut_output_loss_temperature
        )
        instr_session.set_preamp_enabled("", preamp)
        instr_session.set_downconverter_preselector_enabled("", preselector_enabled)
        specan.nf.configuration.configure_y_factor_mode("", y_factor_mode)
        specan.nf.configuration.configure_y_factor_noise_source_enr(
            "", None, None, cold_temperature, off_temperature
        )
        specan.nf.configuration.configure_y_factor_noise_source_settling_time("", settling_time)
        specan.nf.configuration.configure_y_factor_noise_source_loss(
            "", noise_source_loss_comp_enabled, None, None, noise_source_loss_temperature
        )

        if not manual_reference_level:
            reference_level, error_code = specan.nf.configuration.recommend_reference_level(
                "", dut_max_gain, dut_max_noise_figure
            )
            print(f"Reference Level (dBm)      {reference_level}")
        else:
            specan.configure_reference_level("", reference_level)

        specan.initiate("", "")

        measurement_y_factor, calibration_y_factor, error_code = (
            specan.nf.results.fetch_y_factors("", timeout)
        )
        hot_power, cold_power, error_code = specan.nf.results.fetch_y_factor_powers("", timeout)
        analyser_noise_figure, error_code = specan.nf.results.fetch_analyzer_noise_figure("", timeout)
        dut_noise_figure, dut_noise_temperature, dut_gain, error_code = (
            specan.nf.results.fetch_dut_noise_figure_and_gain("", timeout)
        )
        frequency_list, error_code = specan.nf.configuration.get_frequency_list("")

        print("\nResults\n")
        for i in range(len(frequency_list)):
            print(f"\nResult {i}:\n")
            print(f"Frequency (Hz)              :      {frequency_list[i]}")
            print(f"DUT Noise Figure (dB)       :      {dut_noise_figure[i]}")
            print(f"DUT Noise Temperature (K)   :      {dut_noise_temperature[i]}")
            print(f"DUT Gain (dB)               :      {dut_gain[i]}")
            print(f"Analyser Noise Figure (dB)  :      {analyser_noise_figure[i]}")
            print(f"Hot Power (dBm)             :      {hot_power[i]}")
            print(f"Cold Power (dBm)            :      {cold_power[i]}")
            print(f"Measurement Y-Factor (dB)   :      {measurement_y_factor[i]}")
            print(f"Calibration Y-Factor (dB)   :      {calibration_y_factor[i]}")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        if specan is not None:
            specan.dispose()
            specan = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    parser = argparse.ArgumentParser(
        description="Pass arguments for NF Y-Factor Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr.")
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string)


def main():
    _main(sys.argv[1:])


def test_main():
    _main(["--option-string", ""])


def test_example():
    example("RFSA", "")


if __name__ == "__main__":
    main()
