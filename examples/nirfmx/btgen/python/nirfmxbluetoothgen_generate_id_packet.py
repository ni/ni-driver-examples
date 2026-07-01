r"""Bluetooth Generate ID Packet Example.

Steps:
1. Open a new Bluetooth Generation Session and RFSG Session.
2. Convert Channel Number to Carrier Frequency.
3. Configure BD Address.
4. Configure Packet Type (ID).
5. Configure Packet Header (LT Address, Flow, ARQN, SEQN).
6. Configure Waveform Properties (Headroom).
7. Configure Whitening.
8. Configure RFSG session (Frequency, Power Level, External Attenuation).
9. Create and Download ID Packet Waveform and IDLE Waveform to RFSG.
10. Configure Script and Initiate Generation.
11. Close Sessions.

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
    """Run Bluetooth Generate ID Packet Example."""
    # Initialize input variables
    channel_number = 3
    standard = 0  # Basic/EDR
    frequency_band = 0  # 2.4 GHz

    bd_address_lap = 0
    bd_address_uap = 0
    bd_address_nap = 0

    # Packet Header
    packet_header_lt_address = 0
    packet_header_flow = 0
    packet_header_arqn = nirfmxbluetoothgen.PacketHeaderArqn.NAK
    packet_header_seqn = 0

    # Waveform Properties
    auto_headroom_enabled = nirfmxbluetoothgen.AutoHeadroomEnabled.TRUE
    headroom = 0.0  # dB (used when auto_headroom_enabled is FALSE)

    # Whitening
    whitening_enabled = nirfmxbluetoothgen.WhiteningEnabled.FALSE
    whitening_clock = 0

    # RFSG Properties
    power_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    waveform_name = "ID"
    script = (
        "script GenerateIDPkt\n"
        "    repeat forever\n"
        "        generate ID\n"
        "        generate idle\n"
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

        # Configure BD Address
        btsg_session.set_bd_address_lap("", bd_address_lap)
        btsg_session.set_bd_address_uap("", bd_address_uap)
        btsg_session.set_bd_address_nap("", bd_address_nap)

        # Configure Packet Type
        btsg_session.set_packet_type("", nirfmxbluetoothgen.PacketType.ID.value)

        # Configure Packet Header
        btsg_session.set_packet_header_lt_address("", packet_header_lt_address)
        btsg_session.set_packet_header_flow("", packet_header_flow)
        btsg_session.set_packet_header_arqn("", packet_header_arqn.value)
        btsg_session.set_packet_header_seqn("", packet_header_seqn)

        # Configure Waveform Properties - Headroom
        btsg_session.set_auto_headroom_enabled("", auto_headroom_enabled.value)
        if auto_headroom_enabled == nirfmxbluetoothgen.AutoHeadroomEnabled.FALSE:
            btsg_session.set_headroom("", headroom)

        # Configure Whitening
        btsg_session.set_whitening_enabled("", whitening_enabled.value)
        btsg_session.set_whitening_clock("", whitening_clock)

        # Create RFSG Session
        rfsg_session = nirfsg.Session(resource_name, False, True)

        # Configure RFSG Session
        rfsg_session.frequency = carrier_frequency
        rfsg_session.external_gain = -external_attenuation
        rfsg_session.power_level_type = nirfsg.PowerLevelType.PEAK
        rfsg_session.generation_mode = nirfsg.GenerationMode.SCRIPT

        # Create and Download ID Packet Waveform
        btsg_session.rfsg_create_and_download_waveform(rfsg_session, "", waveform_name)
        actual_headroom, error_code = btsg_session.get_actual_headroom("")

        # Create and Download IDLE Waveform
        btsg_session.set_packet_type("", nirfmxbluetoothgen.PacketType.IDLE.value)
        btsg_session.rfsg_create_and_download_waveform(rfsg_session, "", "idle")

        # Configure Script
        btsg_session.rfsg_configure_script(rfsg_session, "", script, power_level)

        rfsg_session.output_enabled = True
        rfsg_session.initiate()

        # Print Results
        print("------------------BT Generate ID Packet------------------")
        print(f"Carrier Frequency (Hz)             : {carrier_frequency}")
        print(f"Actual Headroom (dB)               : {actual_headroom}")
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
        description="Pass arguments for Bluetooth Generate ID Packet Example",
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
