r"""Steps:
1. Open a new RFmx session.
2. Configure Selected Ports.
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure OBW Span.
5. Configure OBW Averaging.
6. Read OBW Measurement Results.
7. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1e9   # Hz
    reference_level = 0.00   # dBm
    external_attenuation = 0.00  # dB
    span = 1.0e6  # Hz
    timeout = 10.0  # seconds
    averaging_enabled = nirfmxspecan.ObwAveragingEnabled.FALSE
    averaging_type = nirfmxspecan.ObwAveragingType.RMS
    averaging_count = 10

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.obw.configuration.configure_span("", span)
        specan.obw.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)

        occupied_bandwidth, average_power, frequency_resolution, start_frequency, stop_frequency, error_code = (
            specan.obw.results.read("", timeout)
        )

        print(f"Occupied Bandwidth (Hz)       {occupied_bandwidth}")
        print(f"Average Total Power (dBm)     {average_power}")
        print(f"Frequency Resolution (Hz)     {frequency_resolution}")
        print(f"Start Frequency (Hz)          {start_frequency}")
        print(f"Stop Frequency (Hz)           {stop_frequency}")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        if specan is not None:
            specan.dispose()
            specan = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    parser = argparse.ArgumentParser(
        description="Pass arguments for OBW Basic Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr.")
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string)


def main():
    _main(sys.argv[1:])


def test_main():
    _main(["--option-string", ""])


def test_example():
    example("RFSA", "")


if __name__ == "__main__":
    main()
