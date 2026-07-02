r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select CCDF measurement and enable the traces.
6. Configure CCDF Number of Records and Measurement Interval.
7. Configure CCDF RBW Filter.
8. Configure CCDF Threshold.
9. Initiate Measurement.
10. Fetch CCDF Measurements and Traces.
11. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    # Initialize input variables
    selected_ports = ""
    center_frequency = 1e9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.00  # dB
    timeout = 10.0  # seconds

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    num_of_records = 1
    measurement_interval = 1.0e-3  # seconds

    rbw_filter_type = nirfmxspecan.CcdfRbwFilterType.NONE
    rbw = 100.0e3  # Hz
    rrc_alpha = 0.010

    threshold_enabled = nirfmxspecan.CcdfThresholdEnabled.FALSE
    threshold_type = nirfmxspecan.CcdfThresholdType.RELATIVE
    threshold_level = -20.00  # dB or dBm

    enable_all_traces = True

    instr_session = None
    specan = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get SpecAn signal
        specan = instr_session.get_specan_signal_configuration()

        # Configure measurement
        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_frequency("", center_frequency)
        specan.configure_reference_level("", reference_level)
        specan.configure_external_attenuation("", external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.CCDF, enable_all_traces)
        specan.ccdf.configuration.configure_number_of_records("", num_of_records)
        specan.ccdf.configuration.configure_measurement_interval("", measurement_interval)
        specan.ccdf.configuration.configure_rbw_filter("", rbw, rbw_filter_type, rrc_alpha)
        specan.ccdf.configuration.configure_threshold("", threshold_enabled, threshold_level, threshold_type)
        specan.initiate("", "")

        # Retrieve results
        mean_power, mean_power_percentile, peak_power, measured_samples_count, error_code = (
            specan.ccdf.results.fetch_power("", timeout)
        )

        (
            ten_percent_power,
            one_percent_power,
            one_tenth_percent_power,
            one_hundredth_percent_power,
            one_thousandth_percent_power,
            one_ten_thousandth_percent_power,
            error_code,
        ) = specan.ccdf.results.fetch_basic_power_probabilities("", timeout)

        gaussian_probabilities = numpy.empty(0, dtype=numpy.float32)
        specan.ccdf.results.fetch_gaussian_probabilities_trace("", timeout, gaussian_probabilities)

        probabilities = numpy.empty(0, dtype=numpy.float32)
        specan.ccdf.results.fetch_probabilities_trace("", timeout, probabilities)

        # Print results
        print("--------------Power----------------------\n")
        print(f"Mean Power (dBm)              {mean_power}")
        print(f"Mean Power Percentile (%)     {mean_power_percentile}")
        print(f"Peak Power (dB)               {peak_power}")
        print(f"Measured Samples Count        {measured_samples_count}")

        print("\n--------------Power Probabilities-------------\n")
        print(f"10 % Power (dB)               {ten_percent_power}")
        print(f"1 % Power (dB)                {one_percent_power}")
        print(f"0.1 % Power (dB)              {one_tenth_percent_power}")
        print(f"0.01 % Power (dB)             {one_hundredth_percent_power}")
        print(f"0.001 % Power (dB)            {one_thousandth_percent_power}")
        print(f"0.0001 % Power (dB)           {one_ten_thousandth_percent_power}")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close Session
        if specan is not None:
            specan.dispose()
            specan = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for CCDF Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr."
    )
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string)


def main():
    """Call _main function."""
    _main(sys.argv[1:])


def test_main():
    """Call _main function with empty option string."""
    cmd_line = [
        "--option-string",
        "",
    ]
    _main(cmd_line)


def test_example():
    """Call example function."""
    example("RFSA", "")


if __name__ == "__main__":
    main()
