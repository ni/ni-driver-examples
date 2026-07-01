r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Packet Type.
6. Configure Payload Length.
7. Select ModAcc measurement and enable Traces.
8. Configure ModAcc Burst Synchronization Mode.
9. Configure Averaging Parameters for ModAcc measurement.
10. Initiate the Measurement.
11. Fetch ModAcc Measurements and Trace.
12. Close RFmx Session.
"""

import argparse
import sys

import nirfmxbluetooth
import nirfmxinstr
import numpy


def example(resource_name, option_string):
    """Run Bluetooth HDT ModAcc Example."""
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

    packet_type = nirfmxbluetooth.PacketType.PACKET_TYPE_LE_HDT
    data_rate = 2000000  # bps

    zadoff_chu_index = 7
    high_data_throughput_packet_format = nirfmxbluetooth.HighDataThroughputPacketFormat.FORMAT0

    payload_length_mode = nirfmxbluetooth.PayloadLengthMode.AUTO
    payload_length = 10  # bytes

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

        bt_signal.set_zadoff_chu_index("", zadoff_chu_index)
        bt_signal.set_high_data_throughput_packet_format("", high_data_throughput_packet_format)

        bt_signal.configure_payload_length("", payload_length_mode, payload_length)

        bt_signal.select_measurements("", measurement, enable_all_traces)

        bt_signal.modacc.configuration.configure_burst_synchronization_type(
            "", burst_synchronization_type
        )
        bt_signal.modacc.configuration.configure_averaging("", averaging_enabled, averaging_count)

        bt_signal.initiate("", "")

        # Retrieve results
        preamble_rms_evm_mean, error_code = bt_signal.modacc.results.get_preamble_rms_evm_mean("")

        control_header_rms_evm_mean, error_code = (
            bt_signal.modacc.results.get_control_header_rms_evm_mean("")
        )

        payload_rms_evm_mean, error_code = bt_signal.modacc.results.get_payload_rms_evm_mean("")

        # Fetch traces
        evm_per_symbol, error_code = bt_signal.modacc.results.fetch_evm_per_symbol_trace(
            "", timeout
        )

        constellation = numpy.empty(0, dtype=numpy.complex64)
        error_code = bt_signal.modacc.results.fetch_constellation_trace("", timeout, constellation)

        # Print Results
        print("------------------EVM------------------")
        print(f"Preamble RMS EVM Mean (dB)                : {preamble_rms_evm_mean}")
        print(f"Control Header RMS EVM Mean (dB)          : {control_header_rms_evm_mean}")
        print(f"Payload RMS EVM Mean (dB)                 : {payload_rms_evm_mean}")

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
        description="Pass arguments for Bluetooth HDT ModAcc Example",
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
