r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Packet Type.
6. Configure Data Rate.
7. Configure Payload Bit Pattern.
8. Configure Payload Length.
9. Configure Direction Finding.
10. Select ModAcc measurement and enable Traces.
11. Configure ModAcc Burst Synchronization Type.
12. Configure Averaging Parameters for ModAcc measurement.
13. Initiate the Measurement.
14. Fetch ModAcc Measurements and Trace.
15. Close RFmx Session.
"""

import argparse
import sys

import nirfmxbluetooth
import nirfmxinstr


def example(resource_name, option_string):
    """Run Bluetooth df2 Example."""
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

    packet_type = nirfmxbluetooth.PacketType.PACKET_TYPE_DH1
    data_rate = 1000000  # bps

    payload_bit_pattern = nirfmxbluetooth.PayloadBitPattern.PATTERN_10101010
    payload_length_mode = nirfmxbluetooth.PayloadLengthMode.AUTO
    payload_length = 10  # bytes

    direction_finding_mode = nirfmxbluetooth.DirectionFindingMode.DISABLED
    cte_length = 160e-6  # seconds
    cte_slot_duration = 1e-6  # seconds

    burst_synchronization_type = nirfmxbluetooth.ModAccBurstSynchronizationType.PREAMBLE

    measurement = nirfmxbluetooth.MeasurementTypes.MODACC
    enable_all_traces = True

    averaging_enabled = nirfmxbluetooth.ModAccAveragingEnabled.FALSE
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

        bt_signal.configure_packet_type("", packet_type)
        bt_signal.configure_data_rate("", data_rate)
        bt_signal.configure_payload_bit_pattern("", payload_bit_pattern)
        bt_signal.configure_payload_length("", payload_length_mode, payload_length)
        bt_signal.configure_le_direction_finding(
            "", direction_finding_mode, cte_length, cte_slot_duration
        )

        bt_signal.select_measurements("", measurement, enable_all_traces)

        bt_signal.modacc.configuration.configure_burst_synchronization_type(
            "", burst_synchronization_type
        )
        bt_signal.modacc.configuration.configure_averaging("", averaging_enabled, averaging_count)

        bt_signal.initiate("", "")

        # Retrieve results
        (
            df2avg_minimum,
            percentage_of_symbols_above_df2max_threshold,
            error_code,
        ) = bt_signal.modacc.results.fetch_df2("", timeout)

        (
            initial_frequency_error_maximum,
            br_peak_frequency_drift_maximum,
            br_peak_frequency_drift_rate_maximum,
            error_code,
        ) = bt_signal.modacc.results.fetch_frequency_error_br("", timeout)

        (
            peak_frequency_error_maximum,
            initial_frequency_drift_maximum,
            le_peak_frequency_drift_maximum,
            le_peak_frequency_drift_rate_maximum,
            error_code,
        ) = bt_signal.modacc.results.fetch_frequency_error_le("", timeout)

        # Fetch traces
        time, df2max, error_code = bt_signal.modacc.results.fetch_df2max_trace("", timeout)

        time_br, frequency_error_br, error_code = (
            bt_signal.modacc.results.fetch_frequency_error_trace_br("", timeout)
        )

        time_le, frequency_error_le, error_code = (
            bt_signal.modacc.results.fetch_frequency_error_trace_le("", timeout)
        )

        # Print Results
        print("------------------df2 Measurement------------------")
        print(f"df2avg Minimum (Hz)                             : {df2avg_minimum}")
        print(
            f"Percentage of Symbols above df2max Threshold (%) : {percentage_of_symbols_above_df2max_threshold}"
        )

        print("\n------------------BR Frequency Error------------------\n")
        print(
            f"Initial Frequency Error Maximum (Hz)            : {initial_frequency_error_maximum}"
        )
        print(
            f"Peak Frequency Drift Maximum (Hz)               : {br_peak_frequency_drift_maximum}"
        )
        print(
            f"Peak Frequency Drift Rate Maximum (Hz)          : {br_peak_frequency_drift_rate_maximum}"
        )

        print("\n------------------LE Frequency Error------------------\n")
        print(f"Peak Frequency Error Maximum (Hz)               : {peak_frequency_error_maximum}")
        print(
            f"Initial Frequency Drift Maximum (Hz)            : {initial_frequency_drift_maximum}"
        )
        print(
            f"Peak Frequency Drift Maximum (Hz)               : {le_peak_frequency_drift_maximum}"
        )
        print(
            f"Peak Frequency Drift Rate Maximum (Hz)          : {le_peak_frequency_drift_rate_maximum}"
        )

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
        description="Pass arguments for Bluetooth df2 Example",
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
