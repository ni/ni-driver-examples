r"""80211ac Generate Waveform (Multi-User) Example.

Steps:
1. Open multiple RFSG Sessions (numTx devices).
2. Open WLAN Generation Session.
3. Configure RFSG sessions (Power Level Type, External Gain).
4. Configure WLAN 802.11ac MU-MIMO standard and parameters.
5. Configure per-user MCS index and number of space-time streams.
6. Configure RF Blanking.
7. Configure Multiple Device Synchronization.
8. Configure Frequency using Single LO.
9. Create and Download MIMO Waveforms to RFSG.
10. Configure Script for each RFSG device and Initiate Generation.
11. Print Results (IQ Rate, Actual Headroom per user).
12. Stop Generation and Close Sessions.

Prerequisites:
- nirfsg package (`pip install nirfsg`)

"""

import argparse
import sys
import threading
import time

import nirfmxwlangen
import nirfsg


MAX_NUM_RFSG = 4


def example(resource_names, power_levels, external_attenuations, option_string):
    """Run WLAN 802.11ac Generate Waveform (Multi-User) Example."""
    # Input variables
    standard = nirfmxwlangen.Standard.STANDARD_80211AC_MIMO_OFDM
    channel_bandwidth = 20e6  # Hz
    carrier_frequency = 5.18e9  # Hz
    num_tx = 2
    number_of_users = 2
    mapping_matrix_type = nirfmxwlangen.MappingMatrixType.DIRECT

    # Per-user parameters (index by user)
    mcs_index = [0, 8]
    number_of_space_time_streams = [1, 1]

    master_reference_clock_source = "PXI_Clk"
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
    rfsg_sessions = [None] * MAX_NUM_RFSG

    try:
        # Create RFSG Sessions
        active_rfsg_sessions = []
        for i in range(num_tx):
            rfsg_sessions[i] = nirfsg.Session(resource_names[i], True, False)
            rfsg_sessions[i].power_level_type = nirfsg.PowerLevelType.PEAK
            rfsg_sessions[i].generation_mode = nirfsg.GenerationMode.ARB_WAVEFORM
            rfsg_sessions[i].external_gain = -external_attenuations[i]
            active_rfsg_sessions.append(rfsg_sessions[i])

        # Create WLAN Generation Session
        wlangen_session = nirfmxwlangen.Session(
            nirfmxwlangen.CompatibilityVersion.Version060000, "WlanGenSession"
        )

        # Configure WLAN 802.11ac MU-MIMO standard and parameters
        wlangen_session.set_standard("", standard)
        wlangen_session.set_number_of_transmit_channels("", num_tx)
        wlangen_session.set_channel_bandwidth("", channel_bandwidth)
        wlangen_session.set_mapping_matrix_type("", mapping_matrix_type)
        wlangen_session.set_ppdu_type("", nirfmxwlangen.PpduType.MU_PPDU)
        wlangen_session.set_number_of_users("", number_of_users)

        # Configure per-user MCS index and number of space-time streams
        for i in range(number_of_users):
            user_string = f"user{i}"
            wlangen_session.set_mcs_index(user_string, mcs_index[i])
            wlangen_session.set_number_of_space_time_streams(
                user_string, number_of_space_time_streams[i]
            )

        # Configure RF Blanking
        wlangen_session.set_rf_blanking_enabled("", nirfmxwlangen.RFBlankingEnabled.TRUE)

        # Configure Multiple Device Synchronization
        wlangen_session.rfsg_configure_multiple_device_synchronization(
            active_rfsg_sessions, master_reference_clock_source, trigger_lines
        )

        # Configure Frequency using Single LO (no external LO)
        wlangen_session.rfsg_configure_frequency_single_lo(
            active_rfsg_sessions,
            nirfmxwlangen.LOSource.ONBOARD,
            rfsg_sessions[0],
            carrier_frequency,
            False,
            False,
        )

        # Create and Download MIMO Waveforms
        wlangen_session.rfsg_create_and_download_mimo_waveforms(
            active_rfsg_sessions, None, waveform_name
        )

        # Configure Script for each RFSG device
        for i in range(num_tx):
            wlangen_session.rfsg_configure_script(
                rfsg_sessions[i], "", script, power_levels[i]
            )

        # Initiate Multiple Device Generation
        wlangen_session.rfsg_multiple_device_initiate(active_rfsg_sessions)

        # Fetch results
        iq_rate, _ = wlangen_session.get_iq_rate("")
        print(f"IQ Rate : {iq_rate}")

        for i in range(num_tx):
            channel_string = f"segment0/channel{i}"
            actual_headroom, _ = wlangen_session.get_actual_headroom(channel_string)
            print(f"Actual HeadRoom User: {actual_headroom}")
        print()

        # Monitor generation status in background thread
        generation_error = [None]
        stop_event = threading.Event()

        def check_generation():
            while not stop_event.is_set():
                try:
                    for i in range(num_tx):
                        rfsg_sessions[i].check_generation_status()
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
        for i in range(MAX_NUM_RFSG):
            if rfsg_sessions[i] is not None:
                try:
                    rfsg_sessions[i].abort()
                    rfsg_sessions[i].output_enabled = False
                    rfsg_sessions[i].commit()
                except Exception:
                    pass

        if wlangen_session is not None:
            for i in range(num_tx):
                if rfsg_sessions[i] is not None:
                    try:
                        wlangen_session.rfsg_clear_database(rfsg_sessions[i], "", waveform_name)
                    except Exception:
                        pass
            wlangen_session.close()
            wlangen_session = None

        for i in range(MAX_NUM_RFSG):
            if rfsg_sessions[i] is not None:
                try:
                    rfsg_sessions[i].close()
                except Exception:
                    pass
                rfsg_sessions[i] = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for WLAN 802.11ac Generate Waveform (Multi-User) Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n",
        "--resource-names",
        nargs="+",
        default=["RIO0", "RIO1", "RIO2", "RIO3"],
        help="Resource names of NI-RFSG devices (space-separated).",
    )
    parser.add_argument(
        "-p",
        "--power-levels",
        nargs="+",
        type=float,
        default=[-10.0, -10.0, -10.0, -10.0],
        help="Power levels in dBm for each RFSG device (space-separated).",
    )
    parser.add_argument(
        "-a",
        "--external-attenuations",
        nargs="+",
        type=float,
        default=[0.0, 0.0, 0.0, 0.0],
        help="External attenuations in dB for each RFSG device (space-separated).",
    )
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    args = parser.parse_args(argsv)

    num_devices = len(args.resource_names)
    power_levels = (args.power_levels + [-10.0] * num_devices)[:num_devices]
    external_attenuations = (args.external_attenuations + [0.0] * num_devices)[:num_devices]

    example(args.resource_names, power_levels, external_attenuations, args.option_string)


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
    example(["RIO0", "RIO1", "RIO2", "RIO3"], [-10.0, -10.0, -10.0, -10.0], [0.0, 0.0, 0.0, 0.0], "")


if __name__ == "__main__":
    main()
