r"""80211be/bn Generate Waveform (Multi-User) Example.

Steps:
1. Open RFSG Sessions (numTx * numSegments devices).
2. Open WLAN Generation Session.
3. Configure RFSG sessions (Power Level Type, External Gain).
4. Configure WLAN 802.11be/bn MU standard and advanced parameters.
5. Configure per-channel headroom.
6. Configure per-user RU and payload parameters.
7. Configure RF Blanking.
8. Configure Multiple Device Synchronization.
9. Configure Frequency using Multiple LO.
10. Create and Download MIMO Waveforms to RFSG.
11. Configure Script for each RFSG device and Initiate Generation.
12. Print Results (Waveform Duration, Packet Extension Duration).
13. Stop Generation and Close Sessions.

Prerequisites:
- nirfsg package (`pip install nirfsg`)

"""

import argparse
import sys
import threading
import time

import nirfmxwlangen
import nirfsg


MAX_NUM_RFSG = 8


def example(resource_names, power_levels, external_attenuations, option_string):
    """Run WLAN 802.11be/bn Generate Waveform (Multi-User) Example."""
    # Input variables
    standard = nirfmxwlangen.Standard.STANDARD_80211BE_MIMO_OFDM
    channel_bandwidth = 20e6  # Hz
    carrier_frequency = 5.18e9  # Hz
    num_tx = 1
    num_segments = 1
    number_of_users = 5
    mapping_matrix_type = nirfmxwlangen.MappingMatrixType.DIRECT
    ppdu_type = nirfmxwlangen.PpduType.MU_PPDU
    guard_interval_type = nirfmxwlangen.GuardIntervalType.ONE_BY_FOUR
    ru_allocation_mode = nirfmxwlangen.RUAllocationMode.INDIVIDUAL
    ru_allocation = [80]
    transmission_mode = nirfmxwlangen.TransmissionMode.DOWNLINK

    # Advanced waveform parameters
    oversampling_factor = 4
    idle_interval = 100e-6  # s
    number_of_frames = 1
    auto_headroom_enabled = nirfmxwlangen.AutoHeadroomEnabled.TRUE
    ltf_size = nirfmxwlangen.LtfSize.AUTO
    sig_compression_enabled = nirfmxwlangen.SigCompressionEnabled.TRUE
    nominal_packet_padding = nirfmxwlangen.NominalPacketPadding.AUTO
    preamble_puncturing_enabled = False
    primary_20mhz_channel_index = 0
    preamble_puncturing_mask = 0xFFFF
    pulse_shaping_filter_enabled = False
    filter_type = nirfmxwlangen.PulseShapingFilterType.RECTANGULAR
    filter_length = 8
    filter_parameter = 0.5
    ofdm_window_length = 2
    swap_i_and_q_enabled = False
    sample_clock_rate_factor = 1.0

    # Per-channel headroom (numTx * numSegments)
    headroom = [12.0, 12.0, 12.0, 12.0, 12.0, 12.0, 12.0, 12.0]

    # Per-user parameters
    ru_size = [
        nirfmxwlangen.RUSize.RU_SIZE_26,
        nirfmxwlangen.RUSize.RU_SIZE_26,
        nirfmxwlangen.RUSize.RU_SIZE_52,
        nirfmxwlangen.RUSize.RU_SIZE_26,
        nirfmxwlangen.RUSize.RU_SIZE_106,
    ]
    ru_offset = [0, 1, 2, 4, 5]
    mcs_index = [0, 0, 0, 0, 0]
    number_of_space_time_streams = [1, 1, 1, 1, 1]
    payload_length = [100, 100, 100, 100, 100]
    sta_id = [0, 1, 2, 3, 4]
    dcm_enabled = [False, False, False, False, False]
    fec_coding_type = [
        nirfmxwlangen.FecCodingType.LDPC,
        nirfmxwlangen.FecCodingType.LDPC,
        nirfmxwlangen.FecCodingType.LDPC,
        nirfmxwlangen.FecCodingType.LDPC,
        nirfmxwlangen.FecCodingType.LDPC,
    ]
    power_boost_factor = [1, 1, 1, 1, 1]
    user_enabled = [True, True, True, True, True]

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
        # Create RFSG Sessions (numTx * numSegments)
        num_rfsg = num_tx * num_segments
        active_rfsg_sessions = []
        for i in range(min(num_rfsg, len(resource_names))):
            rfsg_sessions[i] = nirfsg.Session(resource_names[i], False, True)
            rfsg_sessions[i].power_level_type = nirfsg.PowerLevelType.PEAK
            rfsg_sessions[i].generation_mode = nirfsg.GenerationMode.SCRIPT
            rfsg_sessions[i].external_gain = -external_attenuations[i]
            active_rfsg_sessions.append(rfsg_sessions[i])

        # Create WLAN Generation Session
        wlangen_session = nirfmxwlangen.Session(
            nirfmxwlangen.CompatibilityVersion.Version060000, "WlanGenSession"
        )

        # Configure WLAN 802.11be/bn standard and advanced parameters
        wlangen_session.set_standard("", standard)
        wlangen_session.set_channel_bandwidth("", channel_bandwidth)
        wlangen_session.set_oversampling_factor("", oversampling_factor)
        wlangen_session.set_idle_interval("", idle_interval)
        wlangen_session.set_number_of_frames("", number_of_frames)
        wlangen_session.set_auto_headroom_enabled("", auto_headroom_enabled)
        wlangen_session.set_ltf_size("", ltf_size)
        wlangen_session.set_sig_compression_enabled("", sig_compression_enabled)
        wlangen_session.set_guard_interval_type("", guard_interval_type)

        # Configure per-channel headroom
        for i in range(num_tx * num_segments):
            channel_string = f"channel{i}"
            wlangen_session.set_headroom(channel_string, headroom[i])

        wlangen_session.set_number_of_transmit_channels("", num_tx)
        wlangen_session.set_mapping_matrix_type("", mapping_matrix_type)
        wlangen_session.set_number_of_segments("", num_segments)
        wlangen_session.set_number_of_users("", number_of_users)
        wlangen_session.set_transmission_mode("", transmission_mode)
        wlangen_session.set_preamble_puncturing_enabled("", preamble_puncturing_enabled)
        wlangen_session.set_primary_20mhz_channel_index("", primary_20mhz_channel_index)
        wlangen_session.set_preamble_puncturing_mask("", preamble_puncturing_mask)
        wlangen_session.set_nominal_packet_padding("", nominal_packet_padding)

        # Configure MU PPDU with per-user RU allocation (INDIVIDUAL mode)
        wlangen_session.set_ppdu_type("", ppdu_type)
        wlangen_session.set_ru_allocation_mode("", ru_allocation_mode)
        wlangen_session.set_ru_allocation("", ru_allocation)

        if ru_allocation_mode == nirfmxwlangen.RUAllocationMode.INDIVIDUAL:
            for i in range(number_of_users):
                user_string = f"user{i}"
                wlangen_session.set_ru_size(user_string, ru_size[i])
                wlangen_session.set_ru_offset_mru_index(user_string, ru_offset[i])

        for i in range(number_of_users):
            user_string = f"user{i}"
            wlangen_session.set_mcs_index(user_string, mcs_index[i])
            wlangen_session.set_number_of_space_time_streams(
                user_string, number_of_space_time_streams[i]
            )
            wlangen_session.set_sta_id(user_string, sta_id[i])
            wlangen_session.set_dual_carrier_modulation_enabled(user_string, dcm_enabled[i])
            wlangen_session.set_fec_coding_type(user_string, fec_coding_type[i])
            wlangen_session.set_power_boost_factor(user_string, power_boost_factor[i])
            wlangen_session.set_user_enabled(user_string, user_enabled[i])
            mpdu_string = f"{user_string}/mpdu0"
            wlangen_session.set_payload_data_length(mpdu_string, payload_length[i])

        # Configure pulse shaping and windowing
        wlangen_session.set_pulse_shaping_filter_enabled("", pulse_shaping_filter_enabled)
        wlangen_session.set_pulse_shaping_filter_type("", filter_type)
        wlangen_session.set_pulse_shaping_filter_parameter("", filter_parameter)
        wlangen_session.set_ofdm_window_length("", ofdm_window_length)
        wlangen_session.set_windowing_method("", nirfmxwlangen.WindowingMethod.CENTERED_AT_SYMBOL_BOUNDARY)
        wlangen_session.set_pulse_shaping_filter_length("", filter_length)
        wlangen_session.set_swap_i_and_q_enabled("", swap_i_and_q_enabled)
        wlangen_session.set_sample_clock_rate_factor("", sample_clock_rate_factor)

        # Configure RF Blanking
        wlangen_session.set_rf_blanking_enabled("", nirfmxwlangen.RFBlankingEnabled.TRUE)

        # Configure Multiple Device Synchronization
        wlangen_session.rfsg_configure_multiple_device_synchronization(
            active_rfsg_sessions, master_reference_clock_source, trigger_lines
        )

        # Configure Frequency using Multiple LO (one carrier frequency per segment)
        carrier_frequencies = [carrier_frequency] * num_segments
        wlangen_session.rfsg_configure_frequency_multiple_lo(
            active_rfsg_sessions,
            nirfmxwlangen.LOSource.ONBOARD,
            [],
            carrier_frequencies,
            False,
            False,
        )

        # Create and Download MIMO Waveforms
        wlangen_session.rfsg_create_and_download_mimo_waveforms(
            active_rfsg_sessions, None, waveform_name
        )

        # Configure Script for each RFSG device
        for i in range(num_rfsg):
            wlangen_session.rfsg_configure_script(
                rfsg_sessions[i], "", script, power_levels[i]
            )

        # Initiate Multiple Device Generation
        wlangen_session.rfsg_multiple_device_initiate(active_rfsg_sessions)

        # Fetch results
        iq_rate, _ = wlangen_session.get_iq_rate("")
        iq_waveform_size, _ = wlangen_session.get_iq_waveform_size("")
        waveform_duration = iq_waveform_size / iq_rate if iq_rate != 0 else 0
        packet_extension_duration, _ = wlangen_session.get_packet_extension_duration("")

        print(f"Waveform Duration{{s}} : {waveform_duration}")
        print(f"Packet Extension Duration{{s}} : {packet_extension_duration}")

        # Fetch actual headroom per channel
        actual_headroom = [None] * num_tx
        for i in range(num_tx):
            channel_string = f"channel{i}"
            actual_headroom[i], _ = wlangen_session.get_actual_headroom(channel_string)
        print()

        # Monitor generation status in background thread
        generation_error = [None]
        stop_event = threading.Event()

        def check_generation():
            while not stop_event.is_set():
                try:
                    for i in range(num_rfsg):
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
            for i in range(num_tx * num_segments):
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
        description="Pass arguments for WLAN 802.11be/bn Generate Waveform (Multi-User) Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n",
        "--resource-names",
        nargs="+",
        default=["RIO0", "RIO1", "RIO2", "RIO3", "RIO4", "RIO5", "RIO6", "RIO7"],
        help="Resource names of NI-RFSG devices (space-separated).",
    )
    parser.add_argument(
        "-p",
        "--power-levels",
        nargs="+",
        type=float,
        default=[-10.0, -10.0, -10.0, -10.0, -10.0, -10.0, -10.0, -10.0],
        help="Power levels in dBm for each RFSG device (space-separated).",
    )
    parser.add_argument(
        "-a",
        "--external-attenuations",
        nargs="+",
        type=float,
        default=[0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0],
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
    example(["RIO0", "RIO1", "RIO2", "RIO3", "RIO4", "RIO5", "RIO6", "RIO7"], [-10.0, -10.0, -10.0, -10.0, -10.0, -10.0, -10.0, -10.0], [0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0], "")


if __name__ == "__main__":
    main()
