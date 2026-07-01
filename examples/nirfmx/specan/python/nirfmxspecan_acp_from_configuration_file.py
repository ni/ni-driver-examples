r"""Steps:
1. Open a new RFmx session.
2. Configure Selected Ports.
3. Load all configurations from an rfmxconfig file.
4. Configure Frequency Reference.
5. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
6. Initiate Measurement.
7. Fetch ACP Measurements and Traces.
8. Close the RFmx Session.
"""

import argparse
import os
import sys

import nirfmxspecan
import numpy

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1e9   # Hz
    reference_level = 0.00   # dBm
    external_attenuation = 0.00  # dB
    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz
    configuration_file_name = os.path.join(os.path.dirname(__file__), "Support", "SpecAn_Configurations.rfmxconfig")
    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.load_configurations(configuration_file_name)
        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.initiate("", "")

        (
            lower_relative_power,
            upper_relative_power,
            lower_absolute_power,
            upper_absolute_power,
            error_code,
        ) = specan.acp.results.fetch_offset_measurement_array("", timeout)

        absolute_power, total_relative_power, carrier_frequency, integration_bandwidth, error_code = (
            specan.acp.results.fetch_carrier_measurement("", timeout)
        )

        spectrum = numpy.empty(0, dtype=numpy.float32)
        specan.acp.results.fetch_spectrum("", timeout, spectrum)

        print("-----------------Carrier Measurements-----------------\n")
        print(f"Absolute Power (dBm or dBm/Hz)         {absolute_power}")

        print("\n--------------Offset Channel Measurements-------------\n")
        for i in range(len(lower_relative_power)):
            print(f"----Offset {i}")
            print(f"Lower Relative Power (dB)              {lower_relative_power[i]}")
            print(f"Upper Relative Power (dB)              {upper_relative_power[i]}")
            print(f"Lower Absolute Power (dBm or dBm/Hz)   {lower_absolute_power[i]}")
            print(f"Upper Absolute Power (dBm or dBm/Hz)   {upper_absolute_power[i]}\n")

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
        description="Pass arguments for ACP From Configuration File Example",
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
