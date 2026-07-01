r"""Steps:
1. Open a new RFmx session.
2. Configure Selected Ports.
3. Select NF measurement.
4. Configure Measurement Method (Cold Source).
5. Configure measurement frequencies (Start, Stop, Step or Points or List).
6. Configure Measurement Bandwidth.
7. Configure Measurement Interval.
8. Configure Averaging.
9. Configure Calibration Loss.
10. Configure DUT Input Loss.
11. Configure DUT Output Loss.
12. Configure Cold Source Mode.
13. Configure Cold Source DUT S-Parameters.
14. Configure Reference Level (auto-recommend or manual).
15. Initiate the measurement.
16. Fetch NF Measurements.
17. Close RFmx Session.
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

    start_frequency = 1.0e9    # Hz
    stop_frequency = 2.0e9     # Hz
    step_size = 100.0e6        # Hz
    number_of_points = 10

    dut_max_gain = 0.0         # dB
    dut_max_noise_figure = 0.0 # dB
    reference_level = -55.0    # dBm

    frequency_list_configuration = FREQUENCY_LIST_CONFIGURATION_STEP
    manual_reference_level = True

    measurement_method = nirfmxspecan.NFMeasurementMethod.COLD_SOURCE
    cold_source_mode = nirfmxspecan.NFColdSourceMode.MEASURE
    measurement_bandwidth = 100.0e3   # Hz
    measurement_interval = 1.0e-3     # seconds

    averaging_enabled = nirfmxspecan.NFAveragingEnabled.FALSE
    averaging_count = 10

    # Loss compensation — disabled by default (pass None for freq/value arrays)
    dut_input_loss_comp_enabled = nirfmxspecan.NFDutInputLossCompensationEnabled.FALSE
    dut_input_loss_temperature = 297.0   # K

    dut_output_loss_comp_enabled = nirfmxspecan.NFDutOutputLossCompensationEnabled.FALSE
    dut_output_loss_temperature = 297.0  # K

    calibration_loss_comp_enabled = nirfmxspecan.NFCalibrationLossCompensationEnabled.FALSE
    calibration_loss_temperature = 297.0  # K

    # S-parameters for Cold Source DUT input impedance (None = not used)
    s_param_frequency = None
    s21 = None
    s12 = None
    s11 = None
    s22 = None

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        specan.set_selected_ports("", selected_ports)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.NF, False)
        specan.nf.configuration.configure_measurement_method("", measurement_method)

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
        specan.nf.configuration.configure_cold_source_mode("", cold_source_mode)
        specan.nf.configuration.configure_cold_source_dut_s_parameters("", s_param_frequency, s21, s12, s11, s22)

        if not manual_reference_level:
            reference_level, error_code = specan.nf.configuration.recommend_reference_level(
                "", dut_max_gain, dut_max_noise_figure
            )
            print(f"Reference Level (dBm)      {reference_level}")
        else:
            specan.configure_reference_level("", reference_level)

        specan.initiate("", "")

        cold_source_power, error_code = specan.nf.results.fetch_cold_source_power("", timeout)
        analyser_noise_figure, error_code = specan.nf.results.fetch_analyzer_noise_figure("", timeout)
        dut_noise_figure, dut_noise_temperature, dut_gain, error_code = (
            specan.nf.results.fetch_dut_noise_figure_and_gain("", timeout)
        )
        frequency_list_out, error_code = specan.nf.configuration.get_frequency_list("")

        print("\nResults\n")
        for i in range(len(frequency_list_out)):
            print(f"\nResult {i}:\n")
            print(f"Frequency (Hz)             :      {frequency_list_out[i]}")
            print(f"DUT Noise Figure (dB)      :      {dut_noise_figure[i]}")
            print(f"DUT Noise Temperature (K)  :      {dut_noise_temperature[i]}")
            print(f"DUT Gain (dB)              :      {dut_gain[i]}")
            print(f"Analyser Noise Figure(dB)  :      {analyser_noise_figure[i]}")
            print(f"Measured Power (dBm)       :      {cold_source_power[i]}")

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
        description="Pass arguments for NF Cold Source Example",
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
