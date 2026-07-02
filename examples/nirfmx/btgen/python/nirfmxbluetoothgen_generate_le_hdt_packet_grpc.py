r"""Getting Started:

To run this example, install "RFmx Bluetooth" on the server machine:
  https://www.ni.com/en-us/support/downloads/software-products/download.rfmx-bluetooth.html

Download and run the NI gRPC Device Server (ni_grpc_device_server.exe) on the server machine:
  https://github.com/ni/grpc-device/releases


Running from command line:

Server machine's IP address, port number, resource name and options can be passed as separate
command line arguments.

  > python nirfmxbluetoothgen_generate_le_hdt_packet_grpc.py <server_address> <port_number> <resource_name> <options>

If they are not passed in as command line arguments, then by default the server address will be
"localhost:31763", with "RFSG" as the resource name and empty option string.
"""

r"""Bluetooth Generate LE-HDT Packet gRPC Example.

Steps:
1. Open a gRPC channel to the NI gRPC Device Server.
2. Open a new Bluetooth Generation Session (over gRPC) and RFSG Session.
3. Convert Channel Number to Carrier Frequency.
4. Configure Packet Type (LE-HDT).
5. Configure Data Rate.
6. Configure Payload Length Mode and Length.
7. Configure Dirty Tx.
8. Configure Number of Unique Packets and Oversampling Factor.
9. Configure High Data Throughput (Zadoff-Chu Index, Physical Channel Address,
   HDT Packet Format, HDT PHY Interval).
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
import grpc
import sys
import threading
import time

import nirfmxbluetoothgen
import nirfsg


def example(server_name, port, resource_name, option_string):
    """Run Bluetooth Generate LE-HDT Packet gRPC Example."""
    # Initialize input variables
    channel_number = 3
    standard = 1  # LE
    frequency_band = 0  # 2.4 GHz

    packet_type = nirfmxbluetoothgen.PacketType.LE_HDT

    # Data Rate
    data_rate = nirfmxbluetoothgen.DataRate.DATA_RATE_2M

    # Payload Length
    payload_length_mode = nirfmxbluetoothgen.PayloadLengthMode.MAXIMUM_LENGTH
    payload_length = 0  # bytes (used when payload_length_mode is USER_DEFINED)

    dirty_tx_enabled = nirfmxbluetoothgen.DirtyTxEnabled.FALSE

    number_of_unique_packets = 1
    oversampling_factor = 4

    # High Data Throughput
    zadoff_chu_index = 7
    physical_channel_address = 0
    hdt_packet_format = nirfmxbluetoothgen.HdtPacketFormat.FORMAT0
    hdt_phy_interval = 64e-6  # seconds (must be 64us, 128us, 192us, or 256us)

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
        "script GenerateLEHdtPkt\n"
        "    repeat forever\n"
        "        generate BTWaveform\n"
        "    end repeat\n"
        "end script"
    )

    btsg_session = None
    rfsg_session = None

    try:
        # Create a gRPC channel to the NI gRPC Device Server
        channel = grpc.insecure_channel(
            f"{server_name}:{port}",
            options=[
                ("grpc.max_receive_message_length", -1),
                ("grpc.max_send_message_length", -1),
            ],
        )
        # Create RFSG Session
        grpc_options_for_rfsg = nirfsg.GrpcSessionOptions(channel, "Remote_RFSG_Session")
        rfsg_session = nirfsg.Session(resource_name, grpc_options=grpc_options_for_rfsg)

        grpc_options = nirfmxbluetoothgen.GrpcSessionOptions(
            channel, "Remote_BluetoothGen_Session"
        )
        # Create Bluetooth Generation Session (over gRPC)
        btsg_session = nirfmxbluetoothgen.Session(
            nirfmxbluetoothgen.CompatibilityVersion.Version020000,
            "BluetoothGenSession",
            grpc_options=grpc_options,
        )

        # Convert Channel Number to Carrier Frequency
        carrier_frequency, error_code = btsg_session.channel_number_to_carrier_frequency_v2(
            channel_number, standard, frequency_band
        )

        # Configure Packet Type
        btsg_session.set_packet_type("", packet_type)

        # Configure Data Rate
        btsg_session.set_data_rate("", data_rate)

        # Configure Payload Length Mode
        btsg_session.set_payload_length_mode("", payload_length_mode)

        if payload_length_mode == nirfmxbluetoothgen.PayloadLengthMode.USER_DEFINED:
            btsg_session.set_payload_length("", payload_length)

        # Configure Dirty Tx
        btsg_session.set_dirty_tx_enabled("", dirty_tx_enabled)

        # Configure Number of Unique Packets and Oversampling Factor
        btsg_session.set_number_of_unique_packets("", number_of_unique_packets)
        btsg_session.set_oversampling_factor("", oversampling_factor)

        # Configure High Data Throughput

        # Set Zadoff-Chu Index
        btsg_session.set_zadoff_chu_index("", zadoff_chu_index)

        # Set Physical Channel Address
        btsg_session.set_physical_channel_address("", physical_channel_address)

        # Set HDT Packet Format
        btsg_session.set_hdt_packet_format("", hdt_packet_format)

        # Set HDT PHY Interval
        btsg_session.set_hdt_phy_interval("", hdt_phy_interval)

        # Configure Waveform Properties - Headroom
        btsg_session.set_auto_headroom_enabled("", auto_headroom_enabled)
        if auto_headroom_enabled == nirfmxbluetoothgen.AutoHeadroomEnabled.FALSE:
            btsg_session.set_headroom("", headroom)

        btsg_session.set_carrier_frequency_offset("", carrier_frequency_offset)

        # Configure IQ Impairments
        btsg_session.set_all_iq_impairments_enabled("", all_iq_impairments_enabled)

        if all_iq_impairments_enabled == nirfmxbluetoothgen.AllIQImpairmentsEnabled.TRUE:
            btsg_session.set_quadrature_skew("", quadrature_skew)
            btsg_session.set_i_dc_offset("", i_dc_offset)
            btsg_session.set_q_dc_offset("", q_dc_offset)
            btsg_session.set_iq_gain_imbalance("", iq_gain_imbalance)

        # Configure AWGN
        btsg_session.set_awgn_enabled("", awgn_enabled)
        btsg_session.set_carrier_to_noise_ratio("", carrier_to_noise_ratio)

        # Configure RFSG Session
        rfsg_session.frequency = carrier_frequency
        rfsg_session.external_gain = -external_attenuation
        rfsg_session.power_level_type = nirfsg.PowerLevelType.PEAK
        rfsg_session.generation_mode = nirfsg.GenerationMode.SCRIPT

        # Create and Download Waveform
        btsg_session.rfsg_create_and_download_waveform(rfsg_session, "", waveform_name)

        actual_headroom, error_code = btsg_session.get_actual_headroom("")

        # Configure Script and Initiate Generation
        btsg_session.rfsg_configure_script(rfsg_session, "", script, power_level)
        rfsg_session.output_enabled = True
        rfsg_session.initiate()

        # Print Results
        print("------------------BT Generate LE-HDT Packet (gRPC)------------------")
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
        description="Pass arguments for Bluetooth Generate LE-HDT Packet gRPC Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-s",
        "--server-name",
        default="localhost",
        help="Server name or IP address of the gRPC server machine.",
    )
    parser.add_argument(
        "-p", "--port", default="31763", help="Port number of the gRPC server."
    )
    parser.add_argument(
        "-n", "--resource-name", default="RFSG", help="Resource name of NI-RFSG."
    )
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    args = parser.parse_args(argsv)
    example(args.server_name, args.port, args.resource_name, args.option_string)


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
    example("localhost", "31763", "RFSG", "")


if __name__ == "__main__":
    main()