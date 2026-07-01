r"""Bluetooth Generate LE-TP Packet Example.

Steps:
1. Open a new Bluetooth Generation Session and RFSG Session.
2. Convert Channel Number to Carrier Frequency.
3. Configure Packet Type (LE-TP, LE-TP-EXT, LE-Enhanced, LE-LR-125k, LE-LR-500k).
4. Configure LE-TP Payload Type.
5. Configure Payload Length Mode and Length.
6. Configure LE-TP Corrupt Alternate CRC.
7. Configure Dirty Tx.
8. Configure Number of Unique Packets and Oversampling Factor.
9. Configure Direction Finding (Mode, CTE Length, Slot Duration, Antenna Switching).
10. Configure Waveform Properties (Headroom, Carrier Frequency Offset).
11. Configure IQ Impairments.
12. Configure AWGN.
13. Configure RFSG session (Frequency, Power Level, External Attenuation).
14. Create and Download Waveform to RFSG.
15. Configure Script and Initiate Generation.
16. Close Sessions.

Prerequisites:
- nirfsg package (`pip install nirfsg`)

"""

import argparse
import sys
import threading
import time

import nirfmxbluetoothgen
import nirfsg


def example(resource_name, option_string):
    """Run Bluetooth Generate LE-TP Packet Example."""
    # Initialize input variables
    channel_number = 3
    standard = 1  # LE
    frequency_band = 0  # 2.4 GHz

    packet_type = nirfmxbluetoothgen.PacketType.LE_TP

    # LE-TP Payload
    le_tp_payload_type = nirfmxbluetoothgen.LETPPayloadType.PRBS9
    user_defined_bits = "10101010"

    # Payload Length
    payload_length_mode = nirfmxbluetoothgen.PayloadLengthMode.MAXIMUM_LENGTH
    payload_length = 0  # bytes (used when payload_length_mode is USER_DEFINED)

    le_tp_corrupt_alternate_crc = nirfmxbluetoothgen.LETPCorruptAlternateCrc.FALSE
    dirty_tx_enabled = nirfmxbluetoothgen.DirtyTxEnabled.FALSE

    number_of_unique_packets = 1
    oversampling_factor = 4

    # Direction Finding
    direction_finding_mode = nirfmxbluetoothgen.DirectionFindingMode.DISABLED
    cte_length = 20.0  # us
    cte_slot_duration = 1e-6  # seconds (1 us)
    antenna_switching_enabled = nirfmxbluetoothgen.AntennaSwitchingEnabled.FALSE
    number_of_antennas = 2
    antenna_switching_pattern = "01"
    antenna_switching_duration = 1e-6  # seconds
    relative_phase = [0.0]  # degrees
    relative_amplitude = [0.0]  # dB

    # Waveform Properties
    auto_headroom_enabled = nirfmxbluetoothgen.AutoHeadroomEnabled.TRUE
    headroom = 0.0  # dB (used when auto_headroom_enabled is FALSE)
    carrier_frequency_offset = 0.0  # Hz

    # IQ Impairments
    all_iq_impairments_enabled = nirfmxbluetoothgen.AllIQImpairmentsEnabled.FALSE
    quadrature_skew = 0.0
    i_dc_offset = 0.0
    q_dc_offset = 0.0
    iq_gain_imbalance = 0.0

    # AWGN
    awgn_enabled = nirfmxbluetoothgen.AwgnEnabled.FALSE
    carrier_to_noise_ratio = 50.0  # dB

    # RFSG Properties
    power_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    waveform_name = "BTWaveform"
    script = (
        "script GenerateLEPkt\n"
        "    repeat forever\n"
        "        generate BTWaveform\n"
        "    end repeat\n"
        "end script"
    )

    btsg_session = None
    rfsg_session = None

    try:
        # Create Bluetooth Generation Session
        btsg_session = nirfmxbluetoothgen.Session(
            nirfmxbluetoothgen.CompatibilityVersion.Version020000, resource_name
        )

        # Convert Channel Number to Carrier Frequency
        carrier_frequency, error_code = btsg_session.channel_number_to_carrier_frequency_v2(
            channel_number, standard, frequency_band
        )

        # Configure Packet Type
        btsg_session.set_packet_type("", packet_type.value)

        # Configure LE-TP Payload Type
        btsg_session.set_le_tp_payload_type("", le_tp_payload_type.value)

        # Configure Payload Length Mode
        btsg_session.set_payload_length_mode("", payload_length_mode.value)

        if payload_length_mode == nirfmxbluetoothgen.PayloadLengthMode.USER_DEFINED:
            btsg_session.set_payload_length("", payload_length)

        # Configure LE-TP Corrupt Alternate CRC
        btsg_session.set_le_tp_corrupt_alternate_crc("", le_tp_corrupt_alternate_crc.value)

        # Configure Dirty Tx
        btsg_session.set_dirty_tx_enabled("", dirty_tx_enabled.value)

        # Configure User Defined Bits (if applicable)
        if le_tp_payload_type == nirfmxbluetoothgen.LETPPayloadType.USER_DEFINED_BITS:
            user_def_bits_array = [int(bit) for bit in user_defined_bits]
            btsg_session.set_payload_user_defined_bits("", user_def_bits_array)

        # Configure Number of Unique Packets and Oversampling Factor
        btsg_session.set_number_of_unique_packets("", number_of_unique_packets)
        btsg_session.set_oversampling_factor("", oversampling_factor)

        # Configure Direction Finding
        btsg_session.set_direction_finding_mode("", direction_finding_mode.value)

        if direction_finding_mode != nirfmxbluetoothgen.DirectionFindingMode.DISABLED:
            btsg_session.set_direction_finding_constant_tone_extension_length("", cte_length)
            btsg_session.set_direction_finding_constant_tone_extension_slot_duration(
                "", cte_slot_duration
            )
            btsg_session.set_direction_finding_antenna_switching_enabled(
                "", antenna_switching_enabled.value
            )
            if antenna_switching_enabled == nirfmxbluetoothgen.AntennaSwitchingEnabled.TRUE:
                btsg_session.set_direction_finding_number_of_antennas("", number_of_antennas)
                btsg_session.set_direction_finding_antenna_switching_pattern(
                    "", antenna_switching_pattern
                )
                btsg_session.set_direction_finding_antenna_switching_duration(
                    "", antenna_switching_duration
                )
                btsg_session.set_antenna_relative_phase_and_amplitude(
                    "", relative_amplitude, relative_phase
                )

        # Configure Waveform Properties - Headroom
        btsg_session.set_auto_headroom_enabled("", auto_headroom_enabled.value)
        if auto_headroom_enabled == nirfmxbluetoothgen.AutoHeadroomEnabled.FALSE:
            btsg_session.set_headroom("", headroom)

        btsg_session.set_carrier_frequency_offset("", carrier_frequency_offset)

        # Configure IQ Impairments
        btsg_session.set_all_iq_impairments_enabled("", all_iq_impairments_enabled.value)

        if all_iq_impairments_enabled == nirfmxbluetoothgen.AllIQImpairmentsEnabled.TRUE:
            btsg_session.set_quadrature_skew("", quadrature_skew)
            btsg_session.set_i_dc_offset("", i_dc_offset)
            btsg_session.set_q_dc_offset("", q_dc_offset)
            btsg_session.set_iq_gain_imbalance("", iq_gain_imbalance)

        # Configure AWGN
        btsg_session.set_awgn_enabled("", awgn_enabled.value)
        btsg_session.set_carrier_to_noise_ratio("", carrier_to_noise_ratio)

        # Create RFSG Session
        rfsg_session = nirfsg.Session(resource_name, False, True)

        # Configure RFSG Session
        rfsg_session.frequency = carrier_frequency
        rfsg_session.external_gain = -external_attenuation
        rfsg_session.power_level_type = nirfsg.PowerLevelType.PEAK
        rfsg_session.generation_mode = nirfsg.GenerationMode.SCRIPT

        # Create and Download Waveform
        btsg_session.rfsg_create_and_download_waveform(rfsg_session, "", waveform_name)

        actual_headroom, error_code = btsg_session.get_actual_headroom("")

        # Get Direction Finding Antenna Switching Duration Used (if applicable)
        antenna_switching_duration_used = None
        if direction_finding_mode != nirfmxbluetoothgen.DirectionFindingMode.DISABLED:
            antenna_switching_duration_used, error_code = (
                btsg_session.get_direction_finding_antenna_switching_duration_used("")
            )

        # Configure Script and Initiate Generation
        btsg_session.rfsg_configure_script(rfsg_session, "", script, power_level)
        rfsg_session.output_enabled = True
        rfsg_session.initiate()

        # Print Results
        print("------------------BT Generate LE-TP Packet------------------")
        print(f"Carrier Frequency (Hz)             : {carrier_frequency}")
        print(f"Actual Headroom (dB)               : {actual_headroom}")
        if antenna_switching_duration_used is not None:
            print(f"Antenna Switching Duration Used (s) : {antenna_switching_duration_used}")
        print()

        # Monitor generation status in background thread
        generation_error = [None]
        stop_event = threading.Event()

        def check_generation():
            while not stop_event.is_set():
                try:
                    rfsg_session.check_generation_status()
                except Exception as ex:
                    generation_error[0] = ex
                    break
                time.sleep(1)

        status_thread = threading.Thread(target=check_generation, daemon=True)
        status_thread.start()

        # Keep generating until user stops
        input("Press Enter to stop generation...")
        stop_event.set()
        status_thread.join()

        if generation_error[0] is not None:
            raise generation_error[0]

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close Sessions
        if btsg_session is not None:
            if rfsg_session is not None:
                try:
                    rfsg_session.abort()
                    rfsg_session.output_enabled = False
                    rfsg_session.commit()
                    btsg_session.rfsg_clear_database(rfsg_session, "", "")
                except Exception:
                    pass
            btsg_session.close()
            btsg_session = None
        if rfsg_session is not None:
            rfsg_session.close()
            rfsg_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for Bluetooth Generate LE-TP Packet Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="RFSG", help="Resource name of NI-RFSG."
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
    example("RFSG", "")


if __name__ == "__main__":
    main()
