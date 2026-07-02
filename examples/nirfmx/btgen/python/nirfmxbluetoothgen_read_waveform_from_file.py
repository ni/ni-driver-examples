r"""Bluetooth Read Waveform From File Example.

Steps:
1. Open a new Bluetooth Generation Session and RFSG Session.
2. Convert Channel Number to Carrier Frequency.
3. Read Waveform from TDMS File (first call to get waveform size, second to read data).
4. Store IQ Rate and Headroom in RFSG Database.
5. Configure RFSG session (Frequency, Power Level, External Attenuation).
6. Configure Script and Initiate Generation.
7. Close Sessions.

Prerequisites:
- nirfsg package (`pip install nirfsg`)

"""

import argparse
import sys
import threading
import time

import numpy
import nirfmxbluetoothgen
import nirfsg


def example(resource_name, file_path, option_string):
    """Run Bluetooth Read Waveform From File Example."""
    # Initialize input variables
    channel_number = 3
    standard = 0  # Basic/EDR (0) or LE (1)
    frequency_band = 0  # 2.4 GHz

    # RFSG Properties
    power_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    waveform_name = "BTWaveform"
    script = (
        "script GenerateDataPkt\n"
        "    repeat forever\n"
        "        generate BTWaveform\n"
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

        # Create RFSG Session
        rfsg_session = nirfsg.Session(resource_name, True, True)

        # Configure RFSG Session
        rfsg_session.frequency = carrier_frequency
        rfsg_session.external_gain = -external_attenuation

        # Read Waveform from TDMS File
        # First call: get waveform size by passing an empty array
        empty_waveform = numpy.empty(0, dtype=numpy.complex128)
        t0, dt, iq_rate, headroom, eof, error_code = btsg_session.read_waveform_from_file(
            file_path, "", 0, -1, empty_waveform
        )

        # Second call: read the actual waveform data
        waveform = numpy.empty(0, dtype=numpy.complex128)
        t0, dt, iq_rate, headroom, eof, error_code = btsg_session.read_waveform_from_file(
            file_path, "", 0, -1, waveform
        )

        # Store IQ Rate and Headroom in RFSG Database
        btsg_session.rfsg_store_iq_rate(rfsg_session, "", waveform_name, iq_rate)
        btsg_session.rfsg_store_headroom(rfsg_session, "", waveform_name, headroom)

        # Configure Script and Initiate Generation
        btsg_session.rfsg_configure_script(rfsg_session, "", script, power_level)
        rfsg_session.output_enabled = True
        rfsg_session.initiate()

        # Print Results
        print("------------------BT Read Waveform From File------------------")
        print(f"Carrier Frequency (Hz)             : {carrier_frequency}")
        print(f"IQ Rate (S/s)                      : {iq_rate}")
        print(f"Headroom (dB)                      : {headroom}")
        print(f"Waveform t0 (s)                    : {t0}")
        print(f"Waveform dt (s)                    : {dt}")
        print(f"End of File                        : {eof}")
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
        description="Pass arguments for Bluetooth Read Waveform From File Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="RFSG", help="Resource name of NI-RFSG."
    )
    parser.add_argument(
        "-f",
        "--file-path",
        required=True,
        type=str,
        help="Path to the TDMS waveform file.",
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
        "waveform.tdms",
        "--option-string",
        "",
    ]
    _main(cmd_line)


def test_example():
    """Call example function."""
    example("RFSG", "waveform.tdms", "")


if __name__ == "__main__":
    main()
