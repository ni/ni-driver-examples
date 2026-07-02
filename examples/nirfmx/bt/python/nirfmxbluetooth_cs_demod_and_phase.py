r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Packet Type.
6. Configure Data Rate.
7. Configure Channel Sounding properties (CS Packet Format, CS Sync Sequence,
   CS Phase Measurement Period, CS Tone Extension Slot).
8. Select ModAcc measurement and enable Traces.
9. Configure ModAcc Burst Synchronization Type.
10. Configure Averaging Parameters for ModAcc measurement.
11. Initiate the Measurement.
12. Fetch ModAcc Measurements and Trace.
13. Close RFmx Session.
"""

import argparse
import sys

import nirfmxbluetooth
import nirfmxinstr
import numpy


def example(resource_name, option_string):
    """Run Bluetooth Channel Sounding Demod and Phase Example."""
    # Initialize input variables
    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    center_frequency = 2.402e9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.0  # dB

    enable_trigger = True
    iq_power_edge_trigger_level = -20.0  # dB
    minimum_quiet_time_mode = nirfmxbluetooth.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 100e-6  # seconds
    trigger_delay = 0.0  # seconds

    packet_type = nirfmxbluetooth.PacketType.PACKET_TYPE_LE_CS
    data_rate = 1000000  # bps

    channel_sounding_packet_format = nirfmxbluetooth.ChannelSoundingPacketFormat.SYNC
    channel_sounding_sync_sequence = nirfmxbluetooth.ChannelSoundingSyncSequence.NONE
    channel_sounding_phase_measurement_period = 10e-6  # seconds
    channel_sounding_tone_extension_slot = nirfmxbluetooth.ChannelSoundingToneExtensionSlot.DISABLED

    burst_synchronization_type = nirfmxbluetooth.ModAccBurstSynchronizationType.PREAMBLE

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
            nirfmxbluetooth.IQPowerEdgeTriggerSlope.RISING,
            iq_power_edge_trigger_level,
            trigger_delay,
            minimum_quiet_time_mode,
            minimum_quiet_time,
            nirfmxbluetooth.IQPowerEdgeTriggerLevelType.RELATIVE,
            enable_trigger,
        )

        bt_signal.configure_packet_type("", packet_type)
        bt_signal.configure_data_rate("", data_rate)
        bt_signal.set_channel_sounding_packet_format("", channel_sounding_packet_format)
        bt_signal.set_channel_sounding_sync_sequence("", channel_sounding_sync_sequence)
        bt_signal.set_channel_sounding_phase_measurement_period(
            "", channel_sounding_phase_measurement_period
        )
        bt_signal.set_channel_sounding_tone_extension_slot(
            "", channel_sounding_tone_extension_slot
        )

        bt_signal.select_measurements("", nirfmxbluetooth.MeasurementTypes.MODACC, True)

        bt_signal.modacc.configuration.configure_burst_synchronization_type(
            "", burst_synchronization_type
        )
        bt_signal.modacc.configuration.configure_averaging("", averaging_enabled, averaging_count)

        bt_signal.initiate("", "")

        # Retrieve results
        clock_drift_mean, error_code = bt_signal.modacc.results.get_clock_drift_mean("")

        preamble_start_time_mean, error_code = (
            bt_signal.modacc.results.get_preamble_start_time_mean("")
        )

        (
            peak_frequency_error_maximum,
            initial_frequency_drift_maximum,
            peak_frequency_drift_maximum,
            peak_frequency_drift_rate_maximum,
            error_code,
        ) = bt_signal.modacc.results.fetch_frequency_error_le("", timeout)

        # Fetch traces
        cs_tone_amplitude = numpy.empty(0, dtype=numpy.float32)
        cs_tone_phase = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = bt_signal.modacc.results.fetch_cs_tone_trace(
            "", timeout, cs_tone_amplitude, cs_tone_phase
        )

        cs_detrended_phase = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = bt_signal.modacc.results.fetch_cs_detrended_phase_trace(
            "", timeout, cs_detrended_phase
        )

        # Print Results
        print("------------------ Channel Sounding Measurement ------------------")
        print(f"Peak Frequency Error Maximum (Hz)               : {peak_frequency_error_maximum}")
        print(f"Clock Drift Mean  (ppm)                         : {clock_drift_mean}")
        print(f"Preamble Start Time Mean (seconds)              : {preamble_start_time_mean}")

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
        description="Pass arguments for Bluetooth CS Demod and Phase Example",
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
