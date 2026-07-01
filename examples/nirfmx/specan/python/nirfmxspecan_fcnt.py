r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select FCnt measurement and enable the traces.
6. Configure FCnt Measurement Interval.
7. Configure FCnt RBW Filter.
8. Configure FCnt Averaging.
9. Configure FCnt Threshold.
10. Initiate Measurement.
11. Fetch FCnt Measurements and Traces.
12. Close the RFmx Session.
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

    measurement_interval = 1.0e-3  # seconds

    rbw = 100.0e3  # Hz
    rbw_filter_type = nirfmxspecan.FcntRbwFilterType.NONE
    rrc_alpha = 0.10

    averaging_type = nirfmxspecan.FcntAveragingType.MEAN
    averaging_enabled = nirfmxspecan.FcntAveragingEnabled.FALSE
    averaging_count = 10

    threshold_enabled = nirfmxspecan.FcntThresholdEnabled.FALSE
    threshold_type = nirfmxspecan.FcntThresholdType.RELATIVE
    threshold_level = -20.0  # dB or dBm

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
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.FCNT, True)
        specan.fcnt.configuration.configure_rbw_filter("", rbw, rbw_filter_type, rrc_alpha)
        specan.fcnt.configuration.configure_measurement_interval("", measurement_interval)
        specan.fcnt.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.fcnt.configuration.configure_threshold("", threshold_enabled, threshold_level, threshold_type)
        specan.initiate("", "")

        # Retrieve results
        frequency_trace = numpy.empty(0, dtype=numpy.float32)
        specan.fcnt.results.fetch_frequency_trace("", timeout, frequency_trace)

        average_relative_frequency, average_absolute_frequency, mean_phase, error_code = (
            specan.fcnt.results.fetch_measurement("", timeout)
        )

        # Print results
        print(f"Average Relative Frequency (Hz)  {average_relative_frequency}")
        print(f"Average Absolute Frequency (Hz)  {average_absolute_frequency}")
        print(f"Mean Phase (deg)                 {mean_phase}")

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
        description="Pass arguments for FCnt Example",
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
