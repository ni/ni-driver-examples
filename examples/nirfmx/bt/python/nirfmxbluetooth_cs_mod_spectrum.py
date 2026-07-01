r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Packet Type.
6. Configure Data Rate.
7. Configure CS Packet Format, CS Sync Sequence, Payload Length Mode and Payload Length.
8. Select ModSpectrum measurement and enable Traces.
9. Configure ModSpectrum Burst Synchronization Type.
10. Configure Averaging Parameters for ModSpectrum measurement.
11. Initiate the Measurement.
12. Fetch ModSpectrum Measurements and Traces.
13. Close RFmx Session.
"""

import argparse
import sys

import nirfmxbluetooth
import nirfmxinstr
import numpy


def example(resource_name, option_string):
    """Run Bluetooth Channel Sounding ModSpectrum Example."""
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

    payload_length_mode = nirfmxbluetooth.PayloadLengthMode.AUTO
    payload_length = 10  # bytes

    averaging_enabled = nirfmxbluetooth.ModSpectrumAveragingEnabled.FALSE
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
        bt_signal.set_channel_sounding_packet_format("", nirfmxbluetooth.ChannelSoundingPacketFormat.SYNC)
        bt_signal.set_channel_sounding_sync_sequence(
            "", nirfmxbluetooth.ChannelSoundingSyncSequence.PAYLOAD_PATTERN
        )
        bt_signal.set_payload_length_mode("", payload_length_mode)
        bt_signal.set_payload_length("", payload_length)

        bt_signal.select_measurements("", nirfmxbluetooth.MeasurementTypes.MODSPECTRUM, True)

        bt_signal.modspectrum.configuration.configure_burst_synchronization_type(
            "", nirfmxbluetooth.ModSpectrumBurstSynchronizationType.PREAMBLE
        )
        bt_signal.modspectrum.configuration.configure_averaging(
            "", averaging_enabled, averaging_count
        )

        bt_signal.initiate("", "")

        # Retrieve results
        results_bandwidth, error_code = bt_signal.modspectrum.results.get_bandwidth("")
        results_high_frequency, error_code = bt_signal.modspectrum.results.get_high_frequency("")
        results_low_frequency, error_code = bt_signal.modspectrum.results.get_low_frequency("")

        # Fetch traces
        spectrum = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = bt_signal.modspectrum.results.fetch_spectrum(
            "", timeout, spectrum
        )

        # Print Results
        print("------------------------ModSpectrum--------------------------------")
        print(f"ModSpectrum Results Bandwidth (Hz)       : {results_bandwidth} \n")
        print(f"ModSpectrum Results High Freq (Hz)       : {results_high_frequency} \n")
        print(f"ModSpectrum Results Low Freq (Hz)        : {results_low_frequency} \n")

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
        print("Press any key to exit")
        input()


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for Bluetooth CS ModSpectrum Example",
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
