r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Packet Type.
6. Configure Data Rate.
7. Configure Payload Length.
8. Select ACP measurement and enable Traces.
9. Configure ACP Burst Sync Type.
10. Configure Averaging Parameters for ACP measurement.
11. Configure Number of Offsets Or Channel Number depending on Offset Channel Mode.
12. Initiate the Measurement.
13. Fetch ACP Measurements and Trace.
14. Close RFmx Session.
"""

import argparse
import sys


import nirfmxbluetooth
import nirfmxinstr
import numpy


def example(resource_name, option_string):
    """Run Bluetooth ACP Example."""
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

    payload_length_mode = nirfmxbluetooth.PayloadLengthMode.AUTO
    payload_length = 10  # bytes

    burst_synchronization_type = nirfmxbluetooth.AcpBurstSynchronizationType.PREAMBLE

    measurement = nirfmxbluetooth.MeasurementTypes.ACP
    enable_all_traces = True

    averaging_enabled = nirfmxbluetooth.AcpAveragingEnabled.FALSE
    averaging_count = 10

    number_of_offsets = 5
    offset_channel_mode = nirfmxbluetooth.AcpOffsetChannelMode.SYMMETRIC
    channel_number = 0
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
        bt_signal.configure_payload_length("", payload_length_mode, payload_length)

        bt_signal.select_measurements("", measurement, enable_all_traces)

        bt_signal.acp.configuration.configure_burst_synchronization_type(
            "", burst_synchronization_type
        )
        bt_signal.acp.configuration.configure_averaging("", averaging_enabled, averaging_count)
        bt_signal.acp.configuration.configure_offset_channel_mode("", offset_channel_mode)

        if offset_channel_mode == nirfmxbluetooth.AcpOffsetChannelMode.SYMMETRIC:
            bt_signal.acp.configuration.configure_number_of_offsets("", number_of_offsets)
        elif offset_channel_mode == nirfmxbluetooth.AcpOffsetChannelMode.INBAND:
            bt_signal.configure_channel_number("", channel_number)

        bt_signal.initiate("", "")

        # Retrieve results
        measurement_status, error_code = bt_signal.acp.results.fetch_measurement_status("", timeout)

        reference_channel_power, error_code = bt_signal.acp.results.fetch_reference_channel_power(
            "", timeout
        )

        (
            lower_absolute_power,
            upper_absolute_power,
            lower_relative_power,
            upper_relative_power,
            lower_margin,
            upper_margin,
            error_code,
        ) = bt_signal.acp.results.fetch_offset_measurement_array("", timeout)

        # Fetch traces
        limit_with_exception_mask = numpy.empty(0, dtype=numpy.float32)
        limit_without_exception_mask = numpy.empty(0, dtype=numpy.float32)
        x0_mask, dx_mask, error_code = bt_signal.acp.results.fetch_mask_trace(
            "", timeout, limit_with_exception_mask, limit_without_exception_mask
        )

        absolute_power_trace = numpy.empty(0, dtype=numpy.float32)
        x0_abs, dx_abs, error_code = bt_signal.acp.results.fetch_absolute_power_trace(
            "", timeout, absolute_power_trace
        )

        spectrum = numpy.empty(0, dtype=numpy.float32)
        x0_spec, dx_spec, error_code = bt_signal.acp.results.fetch_spectrum("", timeout, spectrum)

        # Print Results
        print("------------------ACP------------------")
        print(f"Measurement Status                 : {measurement_status.name}")
        print(f"Reference Channel Power (dBm)      : {reference_channel_power}")
        print()

        print("------------------Offset Measurements------------------")
        for i in range(len(lower_absolute_power)):
            print(f"Offset {i}")
            print(f"Lower Absolute Powers (dBm)        : {lower_absolute_power[i]}")
            print(f"Upper Absolute Powers (dBm)        : {upper_absolute_power[i]}")
            print(f"Lower Relative Powers (dB)         : {lower_relative_power[i]}")
            print(f"Upper Relative Powers (dB)         : {upper_relative_power[i]}")
            print(f"Lower Margin (dB)                  : {lower_margin[i]}")
            print(f"Upper Margin (dB)                  : {upper_margin[i]}")
            print()

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
        description="Pass arguments for Bluetooth ACP Example",
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
