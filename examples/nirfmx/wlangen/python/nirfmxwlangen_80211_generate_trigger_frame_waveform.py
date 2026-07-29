r"""80211 Generate Trigger Frame Waveform Example.

Steps:
1. Open RFSG Session.
2. Open Trigger Frame Generation Session (802.11ax, TB PPDU).
3. Configure Trigger Frame Session parameters (RU, MCS, user settings).
4. Create Trigger Frame MSDU bits.
5. Open WLAN Generation Session (802.11ag, trigger frame payload type).
6. Configure WLAN Session with trigger frame payload bits.
7. Configure RF Blanking.
8. Configure Frequency using Single LO.
9. Create and Download Waveform to RFSG.
10. Fetch IQ Rate, Waveform Duration, Actual Headroom.
11. Configure Script and Initiate Generation.
12. Print Results.
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


def example(resource_name, option_string):
    """Run WLAN Generate Trigger Frame Waveform Example."""
    # Trigger frame session parameters (802.11ax TB PPDU)
    trigger_frame_channel_bandwidth = 20e6  # Hz
    trigger_frame_guard_interval_type = nirfmxwlangen.GuardIntervalType.ONE_BY_FOUR
    trigger_frame_ap_tx_power = 0
    trigger_frame_stbc_all_streams_enabled = False
    trigger_frame_number_of_ltf_symbols = -1
    trigger_frame_ru_size = nirfmxwlangen.RUSize.RU_SIZE_26
    trigger_frame_ru_offset = 0
    trigger_frame_mcs_index = 0
    trigger_frame_number_of_space_time_streams = 1
    trigger_frame_dcm_enabled = False
    trigger_frame_fec_coding_type = nirfmxwlangen.FecCodingType.LDPC
    trigger_frame_payload_data_length = 100
    trigger_frame_target_rssi = 78
    trigger_frame_sta_id = 0
    trigger_frame_number_of_users = 1
    midamble_periodicity = nirfmxwlangen.MidamblePeriodicity.NONE
    cs_required = 0
    ltf_size = nirfmxwlangen.LtfSize.AUTO
    ampdu_enabled = False

    # L-SIG advanced parameters (-1 means auto)
    l_sig_length = -1
    pre_fec_padding_factor = -1
    pe_disambiguity = -1
    ldpc_extra_symbol_segment = -1

    # Main wlangen session parameters (802.11ag, trigger frame payload)
    standard = nirfmxwlangen.Standard.STANDARD_80211AG_OFDM
    channel_bandwidth = 20e6  # Hz
    mcs_index = 0
    ofdm_data_rate = nirfmxwlangen.OfdmDataRate.OFDM_DATA_RATE_6
    mac_padding_duration = nirfmxwlangen.TriggerFrameMacPaddingDuration.PADDING_DURATION_0US
    mac_frame_control = 0x0024
    mac_duration_or_id = 0x0000
    mac_address_ra = 0x000000000000
    mac_address_ta = 0x000000000000

    carrier_frequency = 5.18e9  # Hz
    power_level = -10.0  # dBm
    external_attenuation = 0.0  # dB

    waveform_name = "Wlan"
    script = (
        "script GenerateWlan\n"
        "    repeat forever\n"
        "        generate Wlan\n"
        "    end repeat\n"
        "end script"
    )

    trigger_frame_session = None
    wlangen_session = None
    rfsg_session = None

    try:
        # Create RFSG Session
        rfsg_session = nirfsg.Session(resource_name, True, False)
        rfsg_session.power_level_type = nirfsg.PowerLevelType.PEAK
        rfsg_session.generation_mode = nirfsg.GenerationMode.SCRIPT
        rfsg_session.configure_ref_clock("OnboardClock", 10.0e6)
        rfsg_session.external_gain = -external_attenuation

        # Create Trigger Frame Generation Session (802.11ax, TB PPDU)
        trigger_frame_session = nirfmxwlangen.Session(
            nirfmxwlangen.CompatibilityVersion.Version060000, "WlanGenTriggerFrameSession"
        )

        # Configure trigger frame session
        trigger_frame_session.set_standard("", nirfmxwlangen.Standard.STANDARD_80211AX_MIMO_OFDM)
        trigger_frame_session.set_channel_bandwidth("", trigger_frame_channel_bandwidth)
        trigger_frame_session.set_guard_interval_type("", trigger_frame_guard_interval_type)
        trigger_frame_session.set_ppdu_type("", nirfmxwlangen.PpduType.TRIGGER_BASED_PPDU)
        trigger_frame_session.set_trigger_frame_ap_tx_power("", trigger_frame_ap_tx_power)
        trigger_frame_session.set_ampdu_enabled("", ampdu_enabled)
        trigger_frame_session.set_stbc_all_streams_enabled("", trigger_frame_stbc_all_streams_enabled)
        trigger_frame_session.set_number_of_ltf_symbols("", trigger_frame_number_of_ltf_symbols)
        trigger_frame_session.set_number_of_users("", trigger_frame_number_of_users)
        trigger_frame_session.set_trigger_frame_cs_required("", cs_required)
        trigger_frame_session.set_ltf_size("", ltf_size)
        trigger_frame_session.set_midamble_periodicity("", midamble_periodicity)

        # Configure user0 parameters
        trigger_frame_session.set_ru_size("user0", trigger_frame_ru_size)
        trigger_frame_session.set_ru_offset_mru_index("user0", trigger_frame_ru_offset)
        trigger_frame_session.set_mcs_index("user0", trigger_frame_mcs_index)
        trigger_frame_session.set_number_of_space_time_streams(
            "user0", trigger_frame_number_of_space_time_streams
        )
        trigger_frame_session.set_dual_carrier_modulation_enabled("user0", trigger_frame_dcm_enabled)
        trigger_frame_session.set_fec_coding_type("user0", trigger_frame_fec_coding_type)
        trigger_frame_session.set_payload_data_length("user0", trigger_frame_payload_data_length)
        trigger_frame_session.set_trigger_frame_target_rssi("user0", trigger_frame_target_rssi)
        trigger_frame_session.set_sta_id("user0", trigger_frame_sta_id)

        if l_sig_length == -1:
            # Auto mode: disable MAC FCS and header, set payload length
            trigger_frame_session.set_mac_fcs_enabled("user0", False)
            trigger_frame_session.set_mac_header_enabled("user0", False)
            trigger_frame_session.set_payload_data_length("user0", trigger_frame_payload_data_length)
        else:
            # Manual L-SIG configuration
            trigger_frame_session.set_l_sig_length("", l_sig_length)
            trigger_frame_session.set_pre_fec_padding_factor("", pre_fec_padding_factor)
            trigger_frame_session.set_pe_disambiguity("", pe_disambiguity)
            trigger_frame_session.set_ldpc_extra_symbol_segment("", ldpc_extra_symbol_segment)
            trigger_frame_session.set_payload_auto_number_of_mpdus("", True)
            trigger_frame_session.set_auto_payload_data_length_mode("", True)

        # Create trigger frame MSDU bits
        generation_done, trigger_frame_msdu_bits, _ = trigger_frame_session.create_trigger_frame_msdu("")

        # Create main WLAN Generation Session (802.11ag with trigger frame payload)
        wlangen_session = nirfmxwlangen.Session(
            nirfmxwlangen.CompatibilityVersion.Version050000, "WlanGenSession"
        )

        # Configure main session standard
        wlangen_session.set_standard("", standard)
        wlangen_session.set_channel_bandwidth("", channel_bandwidth)
        wlangen_session.set_ofdm_data_rate("", ofdm_data_rate)
        wlangen_session.set_mcs_index("", mcs_index)

        # Configure RF Blanking
        wlangen_session.set_rf_blanking_enabled("", nirfmxwlangen.RFBlankingEnabled.TRUE)

        # Configure trigger frame payload
        wlangen_session.set_payload_mac_frame_type("", nirfmxwlangen.PayloadMacFrameType.TRIGGER_FRAME)
        wlangen_session.set_mac_frame_control("", mac_frame_control)
        wlangen_session.set_mac_duration_or_id("", mac_duration_or_id)
        wlangen_session.set_mac_address1("", mac_address_ra)
        wlangen_session.set_mac_address2("", mac_address_ta)
        wlangen_session.set_trigger_frame_mac_padding_duration("", mac_padding_duration)

        # Set payload to user-defined bits from the trigger frame session
        wlangen_session.set_payload_data_type("", nirfmxwlangen.PayloadDataType.USER_DEFINED_PATTERN)
        if trigger_frame_msdu_bits is not None:
            wlangen_session.set_payload_user_defined_bits("", trigger_frame_msdu_bits)
            msdu_bit_size = len(trigger_frame_msdu_bits)
            payload_data_length = msdu_bit_size // 8 if msdu_bit_size > 0 else trigger_frame_payload_data_length
            wlangen_session.set_payload_data_length("", payload_data_length)

        # Configure Frequency using Single LO
        wlangen_session.rfsg_configure_frequency_single_lo(
            [rfsg_session],
            nirfmxwlangen.LOSource.ONBOARD,
            rfsg_session,
            carrier_frequency,
            False,
            False,
        )

        # Create and Download Waveform
        wlangen_session.rfsg_create_and_download_mimo_waveforms([rfsg_session], "", waveform_name)

        # Fetch results from trigger frame session
        iq_rate, _ = trigger_frame_session.get_iq_rate("")
        iq_waveform_size, _ = trigger_frame_session.get_iq_waveform_size("")
        waveform_duration = iq_waveform_size / iq_rate if iq_rate != 0 else 0
        actual_headroom, _ = trigger_frame_session.get_actual_headroom("channel0")

        print(f"Waveform Duration : {waveform_duration}")
        print(f"Actual HeadRoom : {actual_headroom}")
        print()

        # Configure Script and Initiate Generation
        wlangen_session.rfsg_configure_script(rfsg_session, "", script, power_level)
        rfsg_session.output_enabled = True
        rfsg_session.commit()
        rfsg_session.initiate()

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
        if trigger_frame_session is not None:
            trigger_frame_session.close()
            trigger_frame_session = None
        if rfsg_session is not None:
            rfsg_session.close()
            rfsg_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for WLAN Generate Trigger Frame Waveform Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="RIO0", help="Resource name of NI-RFSG."
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
    example("RIO0", "")


if __name__ == "__main__":
    main()
