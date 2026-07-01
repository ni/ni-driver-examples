r"""80211 Generate MIMO OFDM Waveform (Multiple RFSG with Multiple External LO) Example.

Steps:
1. Open multiple RFSG Sessions (numTx * numSegments devices).
2. Open WLAN Generation Session.
3. Configure RFSG sessions (Power Level Type, External Gain).
4. Open optional External LO Sessions (one per segment).
5. Configure WLAN MIMO OFDM standard and parameters.
6. Configure RF Blanking and LO Frequency Offset Mode.
7. Configure Multiple Device Synchronization.
8. Configure Frequency using Multiple LO (with optional external LOs).
9. Initiate External LO generation (if applicable).
10. Create and Download MIMO Waveforms to RFSG.
11. Configure Script for each RFSG device.
12. Initiate Multiple Device Generation.
13. Print results (Waveform Duration, Actual Headroom per channel).
14. Stop Generation and Close Sessions.

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
NUMBER_OF_EXTERNAL_LO = 1
NUMBER_OF_SEGMENTS = 1


def example(resource_names, power_levels, external_attenuations, option_string):
    """Run WLAN Generate MIMO OFDM Waveform (Multiple RFSG with Multiple External LO) Example."""
    # Input variables
    standard = nirfmxwlangen.Standard.STANDARD_80211AX_MIMO_OFDM
    channel_bandwidth = 80e6  # Hz
    num_tx = 1
    mcs_index = 0
    mapping_matrix_type = nirfmxwlangen.MappingMatrixType.DIRECT
    num_space_time_streams = 1
    frame_format_80211n = nirfmxwlangen.PlcpFrameFormat80211n.MIXED
    preamble_type_80211ah = nirfmxwlangen.PreambleType80211ah.SHORT

    lo_source = nirfmxwlangen.LOSource.EXTERNAL
    rfsg_lo_daisy_chain_enabled = False
    lo_export_to_external_devices_enabled = False

    # Segment carrier frequencies (one per segment)
    segment0_carrier_frequency = 5.21e9  # Hz
    segment1_carrier_frequency = 5.53e9  # Hz
    if NUMBER_OF_SEGMENTS == 1:
        carrier_frequencies = [segment0_carrier_frequency]
    else:
        carrier_frequencies = [segment0_carrier_frequency, segment1_carrier_frequency]

    # External LO configuration: set resource names to non-None to use external LOs
    external_lo_resource_names = [None] * NUMBER_OF_EXTERNAL_LO  # e.g. ["RIO_EXT_LO_0"]
    external_lo_ref_clock_source = "OnboardClock"

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
    external_lo_sessions = [None] * NUMBER_OF_EXTERNAL_LO

    try:
        # Create RFSG Sessions
        rfsg_array_size = num_tx * NUMBER_OF_SEGMENTS
        active_rfsg_sessions = []
        for i in range(rfsg_array_size):
            rfsg_sessions[i] = nirfsg.Session(resource_names[i], True, False)
            rfsg_sessions[i].power_level_type = nirfsg.PowerLevelType.PEAK
            rfsg_sessions[i].generation_mode = nirfsg.GenerationMode.SCRIPT
            rfsg_sessions[i].external_gain = -external_attenuations[i]
            active_rfsg_sessions.append(rfsg_sessions[i])

        # Create WLAN Generation Session
        wlangen_session = nirfmxwlangen.Session(
            nirfmxwlangen.CompatibilityVersion.Version060000, "WlanGenSession"
        )

        # Configure WLAN MIMO OFDM standard and parameters
        wlangen_session.set_standard("", standard)
        wlangen_session.set_channel_bandwidth("", channel_bandwidth)
        wlangen_session.set_number_of_transmit_channels("", num_tx)
        wlangen_session.set_number_of_segments("", NUMBER_OF_SEGMENTS)
        wlangen_session.set_mcs_index("", mcs_index)
        wlangen_session.set_mapping_matrix_type("", mapping_matrix_type)
        wlangen_session.set_80211n_plcp_frame_format("", frame_format_80211n)
        wlangen_session.set_number_of_space_time_streams("", num_space_time_streams)
        wlangen_session.set_80211ah_preamble_type("", preamble_type_80211ah)
        wlangen_session.set_lo_frequency_offset_mode("", nirfmxwlangen.LOFrequencyOffsetMode.AUTO)
        wlangen_session.set_rf_blanking_enabled("", nirfmxwlangen.RFBlankingEnabled.TRUE)

        # Open External LO Sessions (if configured)
        active_ext_lo_sessions = []
        for i in range(NUMBER_OF_EXTERNAL_LO):
            if (
                external_lo_resource_names[i] is not None
                and lo_source == nirfmxwlangen.LOSource.EXTERNAL
                and carrier_frequencies[0] > 3.2e9
            ):
                external_lo_sessions[i] = nirfsg.Session(external_lo_resource_names[i], True, False)
                external_lo_sessions[i].generation_mode = nirfsg.GenerationMode.CW
                external_lo_sessions[i].configure_ref_clock(external_lo_ref_clock_source, 10.0e6)
                active_ext_lo_sessions.append(external_lo_sessions[i])

        # Configure Multiple Device Synchronization
        wlangen_session.rfsg_configure_multiple_device_synchronization(
            active_rfsg_sessions, master_reference_clock_source, trigger_lines
        )

        # Configure Frequency using Multiple LO (one carrier frequency per segment)
        # Pass external_lo_sessions list (may be empty if no external LOs used)
        wlangen_session.rfsg_configure_frequency_multiple_lo(
            active_rfsg_sessions,
            lo_source,
            active_ext_lo_sessions,
            carrier_frequencies,
            rfsg_lo_daisy_chain_enabled,
            lo_export_to_external_devices_enabled,
        )

        # Initiate External LO generation (if applicable)
        for ext_lo in active_ext_lo_sessions:
            if ext_lo is not None:
                ext_lo.initiate()

        # Create and Download MIMO Waveforms
        wlangen_session.rfsg_create_and_download_mimo_waveforms(
            active_rfsg_sessions, None, waveform_name
        )

        # Configure Script for each RFSG device
        for i in range(rfsg_array_size):
            wlangen_session.rfsg_configure_script(
                rfsg_sessions[i], "", script, power_levels[i]
            )

        # Initiate Multiple Device Generation
        wlangen_session.rfsg_multiple_device_initiate(active_rfsg_sessions)

        # Fetch results
        iq_rate, _ = wlangen_session.get_iq_rate("")
        iq_waveform_size, _ = wlangen_session.get_iq_waveform_size("")
        waveform_duration = iq_waveform_size / iq_rate if iq_rate != 0 else 0

        print("------------------WLAN Generate MIMO OFDM Waveform (Multiple RFSG + Multiple Ext LO)------------------")
        print(f"Waveform Duration {{s}}{waveform_duration}")
        print("Actual Headroom (dB)")

        # Determine channel strings per standard
        mimo_standards_use_channel = {
            nirfmxwlangen.Standard.STANDARD_80211N_MIMO_OFDM,
            nirfmxwlangen.Standard.STANDARD_80211AH_MIMO_OFDM,
            nirfmxwlangen.Standard.STANDARD_80211BE_MIMO_OFDM,
            nirfmxwlangen.Standard.STANDARD_80211BN_MIMO_OFDM,
        }

        if standard in mimo_standards_use_channel:
            channel_strings = [f"channel{i}" for i in range(rfsg_array_size)]
        else:
            channel_strings = []
            for seg in range(NUMBER_OF_SEGMENTS):
                for ch in range(num_tx):
                    channel_strings.append(f"segment{seg}/channel{ch}")

        for i in range(rfsg_array_size):
            actual_headroom, _ = wlangen_session.get_actual_headroom(channel_strings[i])
            print(f"\t{actual_headroom}")
        print()

        # Monitor generation status in background thread
        generation_error = [None]
        stop_event = threading.Event()

        def check_generation():
            while not stop_event.is_set():
                try:
                    for i in range(rfsg_array_size):
                        rfsg_sessions[i].check_generation_status()
                except Exception as ex:
                    generation_error[0] = ex
                    break
                time.sleep(1)

        status_thread = threading.Thread(target=check_generation, daemon=True)
        status_thread.start()

        input("Press Enter to exit...")
        stop_event.set()
        status_thread.join()

        if generation_error[0] is not None:
            raise generation_error[0]

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Stop Generation and close External LO Sessions
        for i in range(NUMBER_OF_EXTERNAL_LO):
            if external_lo_sessions[i] is not None:
                try:
                    external_lo_sessions[i].abort()
                    external_lo_sessions[i].output_enabled = False
                    external_lo_sessions[i].commit()
                    external_lo_sessions[i].close()
                except Exception:
                    pass
                external_lo_sessions[i] = None

        for i in range(MAX_NUM_RFSG):
            if rfsg_sessions[i] is not None:
                try:
                    rfsg_sessions[i].abort()
                    rfsg_sessions[i].output_enabled = False
                    rfsg_sessions[i].commit()
                except Exception:
                    pass

        if wlangen_session is not None:
            for i in range(num_tx * NUMBER_OF_SEGMENTS):
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
        description="Pass arguments for WLAN Generate MIMO OFDM Waveform (Multiple RFSG with Multiple Ext LO) Example",
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
