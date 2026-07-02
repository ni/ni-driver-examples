r"""Bluetooth Generate LE-HDT Packet Format1 Example.

Steps:
1. Open a new Bluetooth Generation Session and RFSG Session.
2. Convert Channel Number to Carrier Frequency.
3. Configure Carrier Mode and Packet Type (LE-HDT).
4. Configure Data Rate, Payload Length Mode and Oversampling Factor.
5. Configure High Data Throughput Properties (Zadoff-Chu Index, Physical Channel
   Address, HDT Packet Format Format1, HDT PHY Interval, Number of Payloads).
6. Configure per-payload zone properties (Format1 Payload Zone Configuration Mode and,
   depending on the mode, Payload Length or block-based settings).
7. Configure Waveform Properties (Headroom).
8. Configure IQ Impairments, Carrier Frequency Offset and AWGN.
9. Configure RFSG session (Frequency, Power Level, External Attenuation).
10. Create and Download Waveform to RFSG.
11. Configure Script and Initiate Generation.
12. Close Sessions.

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
    """Run Bluetooth Generate LE-HDT Packet Format1 Example."""
    # Initialize input variables
    channel_number = 3
    standard = 1  # LE
    frequency_band = 0  # 2.4 GHz

    data_rate = nirfmxbluetoothgen.DataRate.DATA_RATE_2M
    payload_length_mode = nirfmxbluetoothgen.PayloadLengthMode.MAXIMUM_LENGTH
    oversampling_factor = 8

    # High Data Throughput Properties
    zadoff_chu_index = 7
    physical_channel_address = 0x9F15555555
    hdt_phy_interval = 64e-6  # s
    number_of_payloads = 1

    # Payload Zone Properties
    format1_payload_zone_configuration_mode = (
        nirfmxbluetoothgen.Format1PayloadZoneConfigurationMode.AUTO
    )
    payload_length_array = [1] * number_of_payloads  # bytes (AUTO + USER_DEFINED mode)
    txlen_sequence_number_array = [0] * number_of_payloads  # USER_DEFINED mode
    number_of_blocks_array = [1] * number_of_payloads  # USER_DEFINED mode
    block_size_array = [1] * number_of_payloads  # USER_DEFINED mode
    last_block_size_array = [1] * number_of_payloads  # USER_DEFINED mode
    txblock_map_array = [0] * number_of_payloads  # USER_DEFINED mode

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

    waveform_name = "LEHDT"
    script = (
        "script GenerateLEPkt\n"
        "    repeat forever\n"
        "        generate LEHDT\n"
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

        # Configure Carrier Mode and Packet Type
        btsg_session.set_carrier_mode("", nirfmxbluetoothgen.CarrierMode.BURST.value)
        btsg_session.set_packet_type("", nirfmxbluetoothgen.PacketType.LE_HDT.value)

        # Configure Data Rate
        btsg_session.set_data_rate("", data_rate.value)

        # Configure Payload Header
        btsg_session.set_payload_length_mode("", payload_length_mode.value)

        # Configure Oversampling Factor
        btsg_session.set_oversampling_factor("", oversampling_factor)

        # Configure High Data Throughput Properties
        btsg_session.set_zadoff_chu_index("", zadoff_chu_index)
        btsg_session.set_physical_channel_address("", physical_channel_address)
        btsg_session.set_hdt_packet_format("", nirfmxbluetoothgen.HdtPacketFormat.FORMAT1.value)
        btsg_session.set_hdt_phy_interval("", hdt_phy_interval)
        btsg_session.set_number_of_payloads("", number_of_payloads)

        # Configure per-payload zone properties
        for i in range(number_of_payloads):
            active_channel = "payload" + str(i)
            btsg_session.set_format1_payload_zone_configuration_mode(
                "", format1_payload_zone_configuration_mode.value
            )
            if (
                format1_payload_zone_configuration_mode
                == nirfmxbluetoothgen.Format1PayloadZoneConfigurationMode.AUTO
            ):
                if payload_length_mode == nirfmxbluetoothgen.PayloadLengthMode.USER_DEFINED:
                    btsg_session.set_payload_length(active_channel, payload_length_array[i])
            else:
                btsg_session.set_txlen_sequence_number(
                    active_channel, txlen_sequence_number_array[i]
                )
                btsg_session.set_number_of_blocks(active_channel, number_of_blocks_array[i])
                btsg_session.set_block_size(active_channel, block_size_array[i])
                btsg_session.set_last_block_size(active_channel, last_block_size_array[i])
                btsg_session.set_txblock_map(active_channel, txblock_map_array[i])

        # Configure Waveform Properties - Headroom
        btsg_session.set_auto_headroom_enabled("", auto_headroom_enabled.value)
        if auto_headroom_enabled == nirfmxbluetoothgen.AutoHeadroomEnabled.FALSE:
            btsg_session.set_headroom("", headroom)

        # Configure IQ Impairments
        btsg_session.set_all_iq_impairments_enabled("", all_iq_impairments_enabled.value)

        if all_iq_impairments_enabled == nirfmxbluetoothgen.AllIQImpairmentsEnabled.TRUE:
            btsg_session.set_quadrature_skew("", quadrature_skew)
            btsg_session.set_i_dc_offset("", i_dc_offset)
            btsg_session.set_q_dc_offset("", q_dc_offset)
            btsg_session.set_iq_gain_imbalance("", iq_gain_imbalance)

        btsg_session.set_carrier_frequency_offset("", carrier_frequency_offset)

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
        payload_zone_length, error_code = btsg_session.get_payload_zone_length("")

        # Configure Script
        btsg_session.rfsg_configure_script(rfsg_session, "", script, power_level)

        rfsg_session.output_enabled = True
        rfsg_session.initiate()

        # Print Results
        print("------------------BT Generate LE-HDT Packet Format1------------------")
        print(f"Carrier Frequency (Hz)             : {carrier_frequency}")
        print(f"Payload Zone Length (bytes)         : {payload_zone_length}")
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
        description="Pass arguments for Bluetooth Generate LE-HDT Packet Format1 Example",
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
