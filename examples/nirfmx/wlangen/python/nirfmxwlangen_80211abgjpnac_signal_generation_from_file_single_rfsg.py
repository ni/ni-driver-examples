r"""80211abgjpnac Signal Generation from File (Single RFSG) Example.

Steps:
1. Open RFSG Session.
2. Open WLAN Generation Session.
3. Configure RFSG session (Frequency, Power Level Type, Generation Mode,
   Frequency Reference, External Gain).
4. Read the waveform from a TDMS file and download it to the RFSG.
5. Configure Script and enable RFSG output.
6. Initiate Generation.
7. Monitor generation status.
8. Stop Generation and Close Sessions.

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
    """Run WLAN 80211abgjpnac Signal Generation from File (Single RFSG) Example."""
    # Input variables
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

    wlangen_session = None
    rfsg_session = None

    try:
        # Create RFSG Session
        rfsg_session = nirfsg.Session(resource_name, True, False)

        # Configure RFSG session
        rfsg_session.frequency = carrier_frequency
        rfsg_session.power_level_type = nirfsg.PowerLevelType.PEAK
        rfsg_session.generation_mode = nirfsg.GenerationMode.SCRIPT
        rfsg_session.configure_ref_clock("OnboardClock", 10.0e6)
        rfsg_session.external_gain = -external_attenuation

        # Create WLAN Generation Session
        wlangen_session = nirfmxwlangen.Session(
            nirfmxwlangen.CompatibilityVersion.Version060000, "WlanGenSession"
        )

        # Read the waveform from the file and download it to the RFSG
        wlangen_session.rfsg_read_and_download_waveforms_from_file(
            [rfsg_session], waveform_name, file_path
        )

        # Configure Script and enable RFSG output
        wlangen_session.rfsg_configure_script(rfsg_session, "", script, power_level)
        rfsg_session.output_enabled = True

        # Initiate Generation
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
        description="Pass arguments for WLAN 80211abgjpnac Signal Generation from File Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="RIO0", help="Resource name of NI-RFSG."
    )
    parser.add_argument(
        "-f",
        "--file-path",
        default="",
        type=str,
        help="Path to the WLAN waveform TDMS file.",
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
        "",
        "--option-string",
        "",
    ]
    _main(cmd_line)


def test_example():
    """Call example function."""
    example("RIO0", "", "")


if __name__ == "__main__":
    main()
