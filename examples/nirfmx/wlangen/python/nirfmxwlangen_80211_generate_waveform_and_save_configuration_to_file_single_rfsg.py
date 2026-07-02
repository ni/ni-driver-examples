r"""80211 Generate Waveform and Save Configuration to File (Single RFSG) Example.

Steps:
1. Open RFSG Session.
2. Open WLAN Generation Session.
3. Configure RFSG session (Power Level Type, External Gain).
4. Configure WLAN standard and parameters.
5. Configure RF Blanking.
6. Configure Frequency using Single LO.
7. Create and Download Waveform to RFSG.
8. Fetch IQ Rate, Actual Headroom, and Standard.
9. Print Results (Standard, IQ Rate, Actual Headroom).
10. Save WLAN configuration to file.
11. Configure Script and Initiate Generation.
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


def example(resource_name, file_path, option_string):
    """Run WLAN Generate Waveform and Save Configuration to File (Single RFSG) Example."""
    # Input variables
    carrier_frequency = 5.18e9  # Hz
    channel_bandwidth = 20e6  # Hz
    ofdm_data_rate = nirfmxwlangen.OfdmDataRate.OFDM_DATA_RATE_6
    dsss_data_rate = nirfmxwlangen.DsssDataRate.DSSS_DATA_RATE_1
    mcs_index = 0
    power_level = -10.0  # dBm
    external_attenuation = 0.0  # dB
    standard = nirfmxwlangen.Standard.STANDARD_80211AG_OFDM

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
        # Create RFSG Session
        rfsg_session = nirfsg.Session(resource_name, True, False)
        rfsg_session.power_level_type = nirfsg.PowerLevelType.PEAK
        rfsg_session.generation_mode = nirfsg.GenerationMode.SCRIPT
        rfsg_session.configure_ref_clock("OnboardClock", 10.0e6)
        rfsg_session.external_gain = -external_attenuation

        # Create WLAN Generation Session
        wlangen_session = nirfmxwlangen.Session(
            nirfmxwlangen.CompatibilityVersion.Version060000, "WlanGenSession"
        )

        # Configure WLAN Standard and parameters
        mimo_standards = {
            nirfmxwlangen.Standard.STANDARD_80211AC_MIMO_OFDM,
            nirfmxwlangen.Standard.STANDARD_80211N_MIMO_OFDM,
            nirfmxwlangen.Standard.STANDARD_80211AH_MIMO_OFDM,
            nirfmxwlangen.Standard.STANDARD_80211AF_MIMO_OFDM,
            nirfmxwlangen.Standard.STANDARD_80211AX_MIMO_OFDM,
            nirfmxwlangen.Standard.STANDARD_80211BE_MIMO_OFDM,
            nirfmxwlangen.Standard.STANDARD_80211BN_MIMO_OFDM,
        }

        if standard in mimo_standards:
            wlangen_session.set_standard("", standard)
            wlangen_session.set_channel_bandwidth("", channel_bandwidth)
            wlangen_session.set_mcs_index("", mcs_index)
            channel_string = "channel0"
        elif standard == nirfmxwlangen.Standard.STANDARD_80211BG_DSSS:
            wlangen_session.set_standard("", standard)
            wlangen_session.set_dsss_data_rate("", dsss_data_rate)
            channel_string = ""
        else:
            wlangen_session.set_standard("", standard)
            wlangen_session.set_channel_bandwidth("", channel_bandwidth)
            wlangen_session.set_ofdm_data_rate("", ofdm_data_rate)
            channel_string = ""

        # Configure RF Blanking
        wlangen_session.set_rf_blanking_enabled("", nirfmxwlangen.RFBlankingEnabled.TRUE)

        # Configure Frequency using Single LO
        wlangen_session.rfsg_configure_frequency_single_lo(
            [rfsg_session],
            nirfmxwlangen.LOSource.ONBOARD,
            rfsg_session,
            carrier_frequency,
            False,
            False,
        )

        # Create and Download Waveform (MIMO or non-MIMO based on standard)
        if standard in mimo_standards:
            wlangen_session.rfsg_create_and_download_mimo_waveforms(
                [rfsg_session], None, waveform_name
            )
        else:
            wlangen_session.rfsg_create_and_download_waveform(rfsg_session, "", waveform_name)

        # Fetch results
        iq_rate, _ = wlangen_session.get_iq_rate("")
        actual_headroom, _ = wlangen_session.get_actual_headroom(channel_string)
        standard_val, _ = wlangen_session.get_standard("")

        print(f"Standard : {standard_val}")
        print(f"IQ Rate : {iq_rate}")
        print(f"Actual HeadRoom : {actual_headroom}")
        print()

        # Save WLAN configuration to file
        wlangen_session.save_configuration_to_file(
            file_path, nirfmxwlangen.FileOperationMode.CREATE_OR_REPLACE
        )

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
        if rfsg_session is not None:
            rfsg_session.close()
            rfsg_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for WLAN Generate Waveform and Save Configuration to File Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="RIO0", help="Resource name of NI-RFSG."
    )
    parser.add_argument(
        "-f",
        "--file-path",
        default="configurationFilePathToBeAdded",
        type=str,
        help="File path to save the WLAN configuration.",
    )
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    args = parser.parse_args(argsv)
    example(args.resource_name, args.file_path, args.option_string)


def main():
    """Call _main function."""
    _main(sys.argv[1:])


def test_main():
    """Call _main function with empty option string."""
    cmd_line = [
        "--file-path",
        "configurationFilePathToBeAdded",
        "--option-string",
        "",
    ]
    _main(cmd_line)


def test_example():
    """Call example function."""
    example("RIO0", "configurationFilePathToBeAdded", "")


if __name__ == "__main__":
    main()
