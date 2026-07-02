r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Select FrequencyRange measurement and enable Traces.
6. Configure Averaging Parameters for FrequencyRange measurement.
7. Configure Span.
8. Initiate the Measurement.
9. Fetch FrequencyRange Measurements and Trace.
10. Close RFmx Session.
"""

import argparse
import sys

import nirfmxbluetooth
import nirfmxinstr
import numpy


def example(resource_name, option_string):
    """Run Bluetooth Frequency Range Example."""
    # Initialize input variables
    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    center_frequency = 2.402e9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.0  # dB

    enable_trigger = True
    iq_power_edge_trigger_slope = nirfmxbluetooth.IQPowerEdgeTriggerSlope.RISING
    iq_power_edge_trigger_level = -20.0  # dB
    minimum_quiet_time_mode = nirfmxbluetooth.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 100e-6  # seconds
    iq_power_edge_trigger_level_type = nirfmxbluetooth.IQPowerEdgeTriggerLevelType.RELATIVE
    trigger_delay = 0.0  # seconds

    span = 10e6  # Hz

    measurement = nirfmxbluetooth.MeasurementTypes.FREQUENCY_RANGE
    enable_all_traces = True

    averaging_enabled = nirfmxbluetooth.FrequencyRangeAveragingEnabled.FALSE
    averaging_count = 10

    timeout = 10.0  # seconds

    instr_session = None
    bt_signal = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get Bluetooth signal configuration
        bt_signal = instr_session.get_bluetooth_signal_configuration()

        # Configure frequency reference
        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )

        bt_signal.configure_rf("", center_frequency, reference_level, external_attenuation)

        bt_signal.configure_iq_power_edge_trigger(
            "",
            "0",
            iq_power_edge_trigger_slope,
            iq_power_edge_trigger_level,
            trigger_delay,
            minimum_quiet_time_mode,
            minimum_quiet_time,
            iq_power_edge_trigger_level_type,
            enable_trigger,
        )

        bt_signal.select_measurements("", measurement, enable_all_traces)

        bt_signal.frequency_range.configuration.configure_averaging(
            "", averaging_enabled, averaging_count
        )
        bt_signal.frequency_range.configuration.configure_span("", span)

        bt_signal.initiate("", "")

        # Retrieve results
        high_frequency, low_frequency, error_code = (
            bt_signal.frequency_range.results.fetch_measurement("", timeout)
        )

        # Fetch traces
        spectrum = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = bt_signal.frequency_range.results.fetch_spectrum(
            "", timeout, spectrum
        )

        # Print Results
        print("------------------Measurement------------------")
        print(f"High Frequency (Hz)                      : {high_frequency}")
        print(f"Low Frequency (Hz)                       : {low_frequency}")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close Session
        if bt_signal is not None:
            bt_signal.dispose()
            bt_signal = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for Bluetooth Frequency Range Example",
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
    options = {}
    example("RFSA", options)


if __name__ == "__main__":
    main()
