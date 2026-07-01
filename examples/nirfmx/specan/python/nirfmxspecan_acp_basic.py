r"""Steps:
1. Open a new RFmx session.
2. Configure Selected Ports.
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure ACP Averaging Parameters.
5. Configure ACP Integration BW, Number of Offset Channels and Channel Spacing.
6. Read ACP Measurement Results.
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

    integration_bandwidth = 1.0e6   # Hz
    number_of_offset_channels = 2
    channel_spacing = 1.0e6  # Hz

    averaging_enabled = nirfmxspecan.AcpAveragingEnabled.FALSE
    averaging_type = nirfmxspecan.AcpAveragingType.RMS
    averaging_count = 10
    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.acp.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.acp.configuration.configure_carrier_and_offsets(
            "", integration_bandwidth, number_of_offset_channels, channel_spacing
        )

        (
            carrier_absolute_power,
            off_ch0_lower_relative_power,
            off_ch0_upper_relative_power,
            off_ch1_lower_relative_power,
            off_ch1_upper_relative_power,
            error_code,
        ) = specan.acp.results.read("", timeout)

        print(f"Carrier Absolute Power (dBm or dBm/Hz)  {carrier_absolute_power}")
        print(f"Offset ch0 Lower Relative Power (dB)    {off_ch0_lower_relative_power}")
        print(f"Offset ch0 Upper Relative Power (dB)    {off_ch0_upper_relative_power}")
        print(f"Offset ch1 Lower Relative Power (dB)    {off_ch1_lower_relative_power}")
        print(f"Offset ch1 Upper Relative Power (dB)    {off_ch1_upper_relative_power}")

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
        description="Pass arguments for ACP Basic Example",
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
