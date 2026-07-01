r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Packet Type.
6. Configure Data Rate.
7. Configure Payload Length.
8. Configure CS Packet Format, CS Sync Sequence, CS Phase Measurement Period and CS Tone Extension Slot.
9. Configure High Data Throughput Packet Format.
10. Select PowerRamp measurement and enable Traces.
11. Configure PowerRamp Burst Synchronization Type.
12. Configure Averaging Parameters for PowerRamp measurement.
13. Initiate the Measurement.
14. Fetch PowerRamp Measurements.
15. Close RFmx Session.
"""

import argparse
import sys

import nirfmxbluetooth
import nirfmxinstr


def example(resource_name, option_string):
    """Run Bluetooth Channel Sounding PowerRamp Example."""
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

    packet_type = nirfmxbluetooth.PacketType.PACKET_TYPE_LE_CS
    data_rate = 1000000  # bps

    payload_length_mode = nirfmxbluetooth.PayloadLengthMode.AUTO
    payload_length = 10  # bytes

    channel_sounding_packet_format = nirfmxbluetooth.ChannelSoundingPacketFormat.SYNC
    channel_sounding_sync_sequence = nirfmxbluetooth.ChannelSoundingSyncSequence.NONE
    channel_sounding_phase_measurement_period = 10e-6  # seconds
    channel_sounding_tone_extension_slot = nirfmxbluetooth.ChannelSoundingToneExtensionSlot.DISABLED

    high_data_throughput_packet_format = nirfmxbluetooth.HighDataThroughputPacketFormat.FORMAT0

    averaging_enabled = nirfmxbluetooth.PowerRampAveragingEnabled.FALSE
    averaging_count = 10

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
        bt_signal.set_channel_sounding_packet_format("", channel_sounding_packet_format)
        bt_signal.set_channel_sounding_sync_sequence("", channel_sounding_sync_sequence)
        bt_signal.set_channel_sounding_phase_measurement_period(
            "", channel_sounding_phase_measurement_period
        )
        bt_signal.set_channel_sounding_tone_extension_slot(
            "", channel_sounding_tone_extension_slot
        )
        bt_signal.set_high_data_throughput_packet_format("", high_data_throughput_packet_format)

        bt_signal.select_measurements("", nirfmxbluetooth.MeasurementTypes.POWERRAMP, True)

        bt_signal.powerramp.configuration.configure_burst_synchronization_type(
            "", nirfmxbluetooth.PowerRampBurstSynchronizationType.PREAMBLE
        )
        bt_signal.powerramp.configuration.configure_averaging(
            "", averaging_enabled, averaging_count
        )

        bt_signal.initiate("", "")

        # Retrieve results
        rise_time_mean, error_code = bt_signal.powerramp.results.get_rise_time_mean("")
        fall_time_mean, error_code = bt_signal.powerramp.results.get_fall_time_mean("")
        forty_db_fall_time_mean, error_code = bt_signal.powerramp.results.get_40db_fall_time_mean(
            ""
        )
        forty_db_rise_time_mean, error_code = bt_signal.powerramp.results.get_40db_rise_time_mean(
            ""
        )

        # Print Results
        print("------------------PowerRamp------------------")
        print(f"Rise Time Mean (s)      : {rise_time_mean} \n")
        print(f"Fall Time Mean (s)      : {fall_time_mean} \n")
        print(f"40dB Fall Time Mean (s) : {forty_db_fall_time_mean} \n")
        print(f"40dB Rise Time Mean (s) : {forty_db_rise_time_mean} \n")

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
        description="Pass arguments for Bluetooth CS PowerRamp Example",
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
