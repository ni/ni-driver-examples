r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Packet Type.
6. Configure Data Rate.
7. Configure Payload Length.
8. Configure Direction Finding.
9. Configure Reference Level.
10. Select Txp measurement and enable Traces.
11. Configure Txp Burst Synchronization Type.
12. Configure Averaging Parameters for Txp measurement.
13. Initiate the Measurement.
14. Fetch Txp Measurements and Trace.
15. Close RFmx Session.
"""

import argparse
import sys

import nirfmxbluetooth
import nirfmxinstr
import numpy


def example(resource_name, option_string):
    """Run Bluetooth TXP Example."""
    # Initialize input variables
    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    center_frequency = 2.402e9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.0  # dB
    auto_level = True
    measurement_interval = 10e-3  # seconds

    enable_trigger = True
    iq_power_edge_trigger_slope = nirfmxbluetooth.IQPowerEdgeTriggerSlope.RISING
    iq_power_edge_level = -20.0  # dB
    minimum_quiet_time_mode = nirfmxbluetooth.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 100e-6  # seconds
    iq_power_edge_trigger_level_type = nirfmxbluetooth.IQPowerEdgeTriggerLevelType.RELATIVE
    trigger_delay = 0.0  # seconds

    packet_type = nirfmxbluetooth.PacketType.PACKET_TYPE_DH1
    data_rate = 1000000  # bps

    payload_length_mode = nirfmxbluetooth.PayloadLengthMode.AUTO
    payload_length = 10  # bytes

    direction_finding_mode = nirfmxbluetooth.DirectionFindingMode.DISABLED
    cte_length = 160e-6  # seconds
    cte_slot_duration = 1e-6  # seconds

    packet_format = nirfmxbluetooth.ChannelSoundingPacketFormat.SYNC
    sync_sequence = nirfmxbluetooth.ChannelSoundingSyncSequence.NONE
    phase_measurement_period = 10e-6  # seconds
    tone_extension_slot = nirfmxbluetooth.ChannelSoundingToneExtensionSlot.DISABLED

    zadoff_chu_index = 7
    high_data_throughput_packet_format = nirfmxbluetooth.HighDataThroughputPacketFormat.FORMAT0

    burst_synchronization_type = nirfmxbluetooth.TxpBurstSynchronizationType.PREAMBLE

    measurement = nirfmxbluetooth.MeasurementTypes.TXP
    enable_all_traces = True

    averaging_enabled = nirfmxbluetooth.TxpAveragingEnabled.FALSE
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

        bt_signal.configure_frequency("", center_frequency)
        bt_signal.configure_external_attenuation("", external_attenuation)

        bt_signal.configure_iq_power_edge_trigger(
            "",
            "0",
            iq_power_edge_trigger_slope,
            iq_power_edge_level,
            trigger_delay,
            minimum_quiet_time_mode,
            minimum_quiet_time,
            iq_power_edge_trigger_level_type,
            enable_trigger,
        )

        bt_signal.configure_packet_type("", packet_type)
        bt_signal.configure_data_rate("", data_rate)
        bt_signal.configure_payload_length("", payload_length_mode, payload_length)

        bt_signal.configure_le_direction_finding(
            "", direction_finding_mode, cte_length, cte_slot_duration
        )

        bt_signal.set_channel_sounding_packet_format("", packet_format)
        bt_signal.set_channel_sounding_sync_sequence("", sync_sequence)
        bt_signal.set_channel_sounding_phase_measurement_period("", phase_measurement_period)
        bt_signal.set_channel_sounding_tone_extension_slot("", tone_extension_slot)
        bt_signal.set_zadoff_chu_index("", zadoff_chu_index)
        bt_signal.set_high_data_throughput_packet_format("", high_data_throughput_packet_format)

        if auto_level:
            auto_set_reference_level, error_code = bt_signal.auto_level("", measurement_interval)
            reference_level = auto_set_reference_level
        else:
            bt_signal.configure_reference_level("", reference_level)

        bt_signal.select_measurements("", measurement, enable_all_traces)

        bt_signal.txp.configuration.configure_burst_synchronization_type(
            "", burst_synchronization_type
        )
        bt_signal.txp.configuration.configure_averaging("", averaging_enabled, averaging_count)

        bt_signal.initiate("", "")

        # Retrieve results
        (
            average_power_mean,
            average_power_maximum,
            average_power_minimum,
            peak_to_average_power_ratio_maximum,
            error_code,
        ) = bt_signal.txp.results.fetch_powers("", timeout)

        (
            edr_gfsk_average_power_mean,
            edr_dpsk_average_power_mean,
            edr_dpsk_gfsk_average_power_ratio_mean,
            error_code,
        ) = bt_signal.txp.results.fetch_edr_powers("", timeout)

        (
            reference_period_average_power_mean,
            reference_period_peak_absolute_power_deviation_maximum,
            error_code,
        ) = bt_signal.txp.results.fetch_le_cte_reference_period_powers("", timeout)

        (
            transmit_slot_average_power_mean,
            transmit_slot_peak_absolute_power_deviation_maximum,
            error_code,
        ) = bt_signal.txp.results.fetch_le_cte_transmit_slot_powers_array("", timeout)

        # Fetch power trace
        power_trace = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = bt_signal.txp.results.fetch_power_trace("", timeout, power_trace)

        # Print Results
        print("------------------Measurement------------------")
        print(f"Average Power Mean (dBm)                        : {average_power_mean}")
        print(f"Average Power Maximum (dBm)                     : {average_power_maximum}")
        print(f"Average Power Minimum (dBm)                     : {average_power_minimum}")
        print(
            f"Peak to Average Power Ratio Maximum (dB)        : {peak_to_average_power_ratio_maximum}"
        )
        print(f"EDR GFSK Average Power Mean (dBm)               : {edr_gfsk_average_power_mean}")
        print(f"EDR DPSK Average Power Mean (dBm)               : {edr_dpsk_average_power_mean}")
        print(
            f"EDR DPSK GFSK Average Power Ratio Mean (dB)     : {edr_dpsk_gfsk_average_power_ratio_mean}"
        )

        print("------------------LE CTE Reference Period Measurement------------------")
        print(
            f"Average Power Mean (dBm)                                        : {reference_period_average_power_mean}"
        )
        print(
            f"Peak Absolute Power Deviation Maximum (%)                       : {reference_period_peak_absolute_power_deviation_maximum}"
        )

        print("------------------LE CTE Transmit Slot Power Measurement------------------")
        for i in range(len(transmit_slot_average_power_mean)):
            print(
                f"Average Power Mean (dBm)[{i}]                      : {transmit_slot_average_power_mean[i]}"
            )
            print(
                f"Peak Absolute Power Deviation Maximum (%)[{i}]     : {transmit_slot_peak_absolute_power_deviation_maximum[i]}"
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
        description="Pass arguments for Bluetooth TXP Example",
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
