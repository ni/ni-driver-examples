r"""Getting Started:

To run this example, install "RFmx WLAN" on the server machine:
  https://www.ni.com/en-us/support/downloads/software-products/download.rfmx-wlan.html

Download and run the NI gRPC Device Server (ni_grpc_device_server.exe) on the server machine:
  https://github.com/ni/grpc-device/releases


Running from command line:

Server machine's IP address, port number, resource name and options can be passed as separate
command line arguments.

  > python nirfmxwlangen_generate_waveform_multi_user_ofdma_grpc.py <server_address> <port_number> <resource_name> <options>

If they are not passed in as command line arguments, then by default the server address will be
"localhost:31763", with "RIO0" as the resource name and empty option string.
"""

r"""80211 Generate Waveform (Multi-User OFDMA) gRPC Example.

Steps:
1. Open a gRPC channel to the NI gRPC Device Server.
2. Open RFSG Session and WLAN Generation Session (over gRPC).
3. Configure WLAN 802.11ax MIMO OFDM standard and parameters.
4. Configure PPDU Type (MU-PPDU) and Number of Users.
5. Configure Guard Interval Type.
6. Configure per-user RU Size, RU Offset/MRU Index, MCS Index,
   Number of Space-Time Streams, and Payload Data Length.
7. Configure RF Blanking.
8. Configure RFSG session (Power Level Type, External Gain).
9. Configure Frequency using Single LO.
10. Create and Download MIMO Waveforms to RFSG.
11. Configure Script and Initiate Generation.
12. Print results (Waveform Duration, Actual Headroom per channel).
13. Stop Generation and Close Sessions.

Prerequisites:
- nirfsg package (`pip install nirfsg`)

"""

import argparse
import grpc
import sys
import threading
import time

import nirfmxwlangen
import nirfsg


def example(server_name, port, resource_name, option_string):
    """Run WLAN Generate Waveform (Multi-User OFDMA) gRPC Example."""
    # Input variables
    standard = nirfmxwlangen.Standard.STANDARD_80211AX_MIMO_OFDM
    channel_bandwidth = 20e6  # Hz
    carrier_frequency = 5.18e9  # Hz
    num_tx = 1
    mapping_matrix_type = nirfmxwlangen.MappingMatrixType.DIRECT
    ppdu_type = nirfmxwlangen.PpduType.MU_PPDU
    number_of_users = 5
    guard_interval_type = nirfmxwlangen.GuardIntervalType.ONE_BY_FOUR

    # Per-user configuration arrays
    ru_size = [
        nirfmxwlangen.RUSize.RU_SIZE_26,
        nirfmxwlangen.RUSize.RU_SIZE_26,
        nirfmxwlangen.RUSize.RU_SIZE_52,
        nirfmxwlangen.RUSize.RU_SIZE_26,
        nirfmxwlangen.RUSize.RU_SIZE_106,
    ]
    ru_offset_mru_index = [0, 1, 2, 4, 5]
    mcs_index = [0, 0, 0, 0, 0]
    number_of_space_time_streams = [1, 1, 1, 1, 1]
    payload_length = [100, 100, 100, 100, 100]

    power_level = -10.0  # dBm
    external_attenuation = 0.0  # dB

    master_reference_clock_source = "PXI_CLK"
    trigger_lines = [0, 1]

    waveform_name = "Wlan"
    script = (
        "script GenerateWlan\n"
        "    repeat forever\n"
        "        generate Wlan\n"
        "    end repeat\n"
        "end script"
    )

    wlangen_session = None
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
        rfsg_session = nirfsg.Session(resource_name, False, True, grpc_options=grpc_options_for_rfsg)
        active_rfsg_sessions = [rfsg_session]

        grpc_options = nirfmxwlangen.GrpcSessionOptions(
            channel, "Remote_WlanGen_Session"
        )

        # Configure RFSG session
        rfsg_session.power_level_type = nirfsg.PowerLevelType.PEAK
        rfsg_session.generation_mode = nirfsg.GenerationMode.SCRIPT
        rfsg_session.external_gain = -external_attenuation

        # Create WLAN Generation Session (over gRPC)
        wlangen_session = nirfmxwlangen.Session(
            nirfmxwlangen.CompatibilityVersion.Version060000,
            "WlanGenSession",
            grpc_options=grpc_options,
        )

        # Configure WLAN Standard and Parameters
        wlangen_session.set_standard("", standard)
        wlangen_session.set_channel_bandwidth("", channel_bandwidth)
        wlangen_session.set_number_of_transmit_channels("", num_tx)
        wlangen_session.set_mapping_matrix_type("", mapping_matrix_type)
        wlangen_session.set_ppdu_type("", ppdu_type)
        wlangen_session.set_number_of_users("", number_of_users)
        wlangen_session.set_guard_interval_type("", guard_interval_type)

        # Configure per-user parameters
        for i in range(number_of_users):
            user_string = f"user{i}"
            wlangen_session.set_ru_size(user_string, ru_size[i])
            wlangen_session.set_ru_offset_mru_index(user_string, ru_offset_mru_index[i])
            wlangen_session.set_mcs_index(user_string, mcs_index[i])
            wlangen_session.set_number_of_space_time_streams(
                user_string, number_of_space_time_streams[i]
            )
            mpdu_string = f"{user_string}/mpdu0"
            wlangen_session.set_payload_data_length(mpdu_string, payload_length[i])

        # Configure RF Blanking
        wlangen_session.set_rf_blanking_enabled("", nirfmxwlangen.RFBlankingEnabled.TRUE)

        # Configure Multiple Device Synchronization (required for multi-device scenarios)
        if num_tx > 1:
            wlangen_session.rfsg_configure_multiple_device_synchronization(
                active_rfsg_sessions, master_reference_clock_source, trigger_lines
            )

        # Configure Frequency using Single LO (no external LO)
        wlangen_session.rfsg_configure_frequency_single_lo(
            active_rfsg_sessions,
            nirfmxwlangen.LOSource.ONBOARD,
            rfsg_session,
            carrier_frequency,
            False,
            False,
        )

        # Create and Download MIMO Waveforms
        wlangen_session.rfsg_create_and_download_mimo_waveforms(
            active_rfsg_sessions, None, waveform_name
        )

        # Configure Script and Initiate Generation
        wlangen_session.rfsg_configure_script(rfsg_session, "", script, power_level)
        wlangen_session.rfsg_multiple_device_initiate(active_rfsg_sessions)

        # Fetch results
        iq_rate, _ = wlangen_session.get_iq_rate("")
        iq_waveform_size, _ = wlangen_session.get_iq_waveform_size("")
        waveform_duration = iq_waveform_size / iq_rate if iq_rate != 0 else 0

        # Print Results
        print(
            "------------------WLAN Generate Waveform (Multi-User OFDMA, gRPC)------------------"
        )
        print(f"Waveform Duration (s)              : {waveform_duration}")
        print("Actual Headroom (dB)")
        for i in range(num_tx):
            actual_headroom, _ = wlangen_session.get_actual_headroom(f"channel{i}")
            print(f"\t{actual_headroom}")
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

        input("Press Enter to stop generation...")
        stop_event.set()
        status_thread.join()

        if generation_error[0] is not None:
            raise generation_error[0]

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Stop Generation and Close Sessions
        if rfsg_session is not None:
            try:
                rfsg_session.abort()
                rfsg_session.output_enabled = False
                rfsg_session.commit()
            except Exception:
                pass
        if wlangen_session is not None:
            try:
                wlangen_session.rfsg_clear_database(rfsg_session, "", waveform_name)
            except Exception:
                pass
            wlangen_session.close()
            wlangen_session = None
        if rfsg_session is not None:
            rfsg_session.close()
            rfsg_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for WLAN Generate Waveform (Multi-User OFDMA) gRPC Example",
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
        "-n", "--resource-name", default="RIO0", help="Resource name of NI-RFSG."
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
    example("localhost", "31763", "RIO0", "")


if __name__ == "__main__":
    main()
