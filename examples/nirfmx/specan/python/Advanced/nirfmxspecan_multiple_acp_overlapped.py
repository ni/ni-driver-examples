r"""Steps:
1. Open a new RFmx session.
2. Configure Selected Ports.
3. Configure Center Frequency, External Attenuation and Reference Level.
4. Select ACP measurement.
5. Configure ACP Averaging.
6. Configure ACP Integration BW, Number of Offset Channels, Channel Spacing.
7. Initiate first ACP measurement with result name "ACP_Results_1". Wait for completion.
8. Reconfigure Reference Level.
9. Initiate second ACP measurement with result name "ACP_Results_2".
10. Fetch ACP_1 Measurement Results.
11. Fetch ACP_2 Measurement Results.
12. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan

import nirfmxinstr

NUMBER_OF_OFFSET_CHANNELS = 2


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9    # Hz
    reference_level1 = 0.0      # dBm
    reference_level2 = -10.0    # dBm
    external_attenuation = 0.0  # dB

    integration_bandwidth = 1.0e6  # Hz
    channel_spacing = 1.0e6        # Hz

    averaging_enabled = nirfmxspecan.AcpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.AcpAveragingType.RMS

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        specan.set_selected_ports("", selected_ports)
        specan.configure_frequency("", center_frequency)
        specan.configure_external_attenuation("", external_attenuation)
        specan.configure_reference_level("", reference_level1)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.ACP, False)
        specan.acp.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.acp.configuration.configure_carrier_and_offsets("", integration_bandwidth, NUMBER_OF_OFFSET_CHANNELS, channel_spacing)

        result1_name = nirfmxspecan.SpecAn.build_result_string("ACP_Results_1")
        specan.initiate("", result1_name)
        instr_session.wait_for_acquisition_complete(timeout)

        specan.configure_reference_level("", reference_level2)
        result2_name = nirfmxspecan.SpecAn.build_result_string("ACP_Results_2")
        specan.initiate("", result2_name)

        # Fetch result 1
        carrier_abs_power1, relative_power1, carrier_freq1, bw1, error_code = (
            specan.acp.results.fetch_carrier_measurement(result1_name, timeout)
        )
        lower_rel1, upper_rel1, lower_abs1, upper_abs1, error_code = (
            specan.acp.results.fetch_offset_measurement_array(result1_name, timeout)
        )

        # Fetch result 2
        carrier_abs_power2, relative_power2, carrier_freq2, bw2, error_code = (
            specan.acp.results.fetch_carrier_measurement(result2_name, timeout)
        )
        lower_rel2, upper_rel2, lower_abs2, upper_abs2, error_code = (
            specan.acp.results.fetch_offset_measurement_array(result2_name, timeout)
        )

        print("------------------------ACP Measurement1-----------------------------\n")
        print(f"Carrier Abs Power (dBm or dBm/Hz)  {carrier_abs_power1}")
        for i in range(NUMBER_OF_OFFSET_CHANNELS):
            print(f"\nOffset Channel : {i}")
            print(f"Lower Relative Power (dB)         {lower_rel1[i]}")
            print(f"Upper Relative Power (dB)         {upper_rel1[i]}")

        print("\n------------------------ACP Measurement2-----------------------------\n")
        print(f"Carrier Abs Power (dBm or dBm/Hz)  {carrier_abs_power2}")
        for i in range(NUMBER_OF_OFFSET_CHANNELS):
            print(f"\nOffset Channel : {i}")
            print(f"Lower Relative Power (dB)         {lower_rel2[i]}")
            print(f"Upper Relative Power (dB)         {upper_rel2[i]}")

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
        description="Pass arguments for Multiple ACP Overlapped Example",
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
