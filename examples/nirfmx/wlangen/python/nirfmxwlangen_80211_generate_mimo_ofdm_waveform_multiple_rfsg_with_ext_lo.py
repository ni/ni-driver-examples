r"""80211 Generate MIMO OFDM Waveform (Multiple RFSG with External LO) Example.

Steps:
1. Open multiple RFSG Sessions (up to MAX_NUM_RFSG).
2. Open WLAN Generation Session.
3. Configure WLAN MIMO OFDM standard and parameters.
4. Configure RF Blanking and LO Frequency Offset Mode.
5. Configure RFSG sessions (Power Level Type, External Gain).
6. Open optional External LO Session.
7. Configure Multiple Device Synchronization.
8. Configure Frequency using Single LO (with optional external LO).
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


def example(resource_names, power_levels, external_attenuations, option_string):
    """Run WLAN Generate MIMO OFDM Waveform (Multiple RFSG with External LO) Example."""
    # Input variables
    standard = nirfmxwlangen.Standard.STANDARD_80211AX_MIMO_OFDM
    channel_bandwidth = 80e6  # Hz
    carrier_frequency = 5.18e9  # Hz
    num_tx = 1
    mcs_index = 0
    mapping_matrix_type = nirfmxwlangen.MappingMatrixType.DIRECT
    num_space_time_streams = 1
    frame_format_80211n = nirfmxwlangen.PlcpFrameFormat80211n.MIXED
    preamble_type_80211ah = nirfmxwlangen.PreambleType80211ah.SHORT

    lo_source = nirfmxwlangen.LOSource.EXTERNAL
    rfsg_lo_daisy_chain_enabled = False
    lo_export_to_external_devices_enabled = False

    # External LO configuration (set to non-empty string to use external LO)
    external_lo_resource_name = None  # e.g. "RIO_EXT_LO"
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
    external_lo_session = None

    try:
        # Create RFSG Sessions
        active_rfsg_sessions = []
        for i in range(num_tx):
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
        wlangen_session.set_mcs_index("", mcs_index)
        wlangen_session.set_mapping_matrix_type("", mapping_matrix_type)
        wlangen_session.set_80211n_plcp_frame_format("", frame_format_80211n)
        wlangen_session.set_number_of_space_time_streams("", num_space_time_streams)
        wlangen_session.set_80211ah_preamble_type("", preamble_type_80211ah)
        wlangen_session.set_lo_frequency_offset_mode("", nirfmxwlangen.LOFrequencyOffsetMode.AUTO)
        wlangen_session.set_rf_blanking_enabled("", nirfmxwlangen.RFBlankingEnabled.TRUE)

        # Open External LO session (if configured)
        if external_lo_resource_name and lo_source == nirfmxwlangen.LOSource.EXTERNAL and carrier_frequency > 3.2e9:
            external_lo_session = nirfsg.Session(external_lo_resource_name, True, False)
            external_lo_session.generation_mode = nirfsg.GenerationMode.CW
            external_lo_session.configure_ref_clock(external_lo_ref_clock_source, 10.0e6)

        # Configure Multiple Device Synchronization
        wlangen_session.rfsg_configure_multiple_device_synchronization(
            active_rfsg_sessions, master_reference_clock_source, trigger_lines
        )

        # Configure Frequency using Single LO
        # When no external LO is configured, pass the first RFSG session as a dummy
        # ext_lo handle (ignored by the C function when LOSource.ONBOARD is used)
        ext_lo_handle = external_lo_session if external_lo_session is not None else rfsg_sessions[0]
        wlangen_session.rfsg_configure_frequency_single_lo(
            active_rfsg_sessions,
            lo_source,
            ext_lo_handle,
            carrier_frequency,
            rfsg_lo_daisy_chain_enabled,
            lo_export_to_external_devices_enabled,
        )

        # Initiate External LO if present
        if external_lo_session is not None:
            external_lo_session.initiate()

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
        iq_waveform_size, _ = wlangen_session.get_iq_waveform_size("")
        waveform_duration = iq_waveform_size / iq_rate if iq_rate != 0 else 0

        # Print Results
        print("------------------WLAN Generate MIMO OFDM Waveform (Multiple RFSG)------------------")
        print(f"Waveform Duration {{s}}{waveform_duration}")
        print("Actual Headroom (dB)")
        for i in range(num_tx):
            channel_string = f"channel{i}"
            actual_headroom, _ = wlangen_session.get_actual_headroom(channel_string)
            print(f"\t{actual_headroom}")
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

        input("Press Enter to exit...")
        stop_event.set()
        status_thread.join()

        if generation_error[0] is not None:
            raise generation_error[0]

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Stop Generation and close External LO
        if external_lo_session is not None:
            try:
                external_lo_session.abort()
                external_lo_session.output_enabled = False
                external_lo_session.commit()
                external_lo_session.close()
            except Exception:
                pass
            external_lo_session = None

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
        description="Pass arguments for WLAN Generate MIMO OFDM Waveform (Multiple RFSG with Ext LO) Example",
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
    # Pad power levels and attenuations if fewer values provided than devices
    power_levels = (args.power_levels + [-10.0] * num_devices)[:num_devices]
    external_attenuations = (args.external_attenuations + [0.0] * num_devices)[:num_devices]

    example(args.resource_names, power_levels, external_attenuations, args.option_string)


def main():
    """Call _main function."""
    _main(sys.argv[1:])


def test_main():
    """Call _main function with default options."""
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
