r"""Steps:
1. Open a new RFmx session.
2. Configure Selected Ports.
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Select ACP and TXP measurements in composite mode.
5. Configure ACP Averaging.
6. Configure ACP Integration BW, Number of Offset Channels and Channel Spacing.
7. Configure TXP Measurement Interval and RBW.
8. Configure TXP Averaging.
9. Initiate Measurement.
10. Fetch ACP Carrier and Offset Measurements.
11. Fetch TXP Measurement.
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
    center_frequency = 1.0e9  # Hz
    reference_level = 0.0     # dBm
    external_attenuation = 0.0  # dB

    enable_all_traces = True

    integration_bandwidth = 1.0e6  # Hz
    channel_spacing = 1.0e6        # Hz

    acp_averaging_enabled = nirfmxspecan.AcpAveragingEnabled.FALSE
    acp_averaging_count = 10
    acp_averaging_type = nirfmxspecan.AcpAveragingType.RMS

    measurement_interval = 1.0e-3  # seconds
    rbw = 100.0e3                  # Hz
    rbw_filter_type = nirfmxspecan.TxpRbwFilterType.GAUSSIAN
    rrc_alpha = 0.1

    txp_averaging_enabled = nirfmxspecan.TxpAveragingEnabled.FALSE
    txp_averaging_count = 10
    txp_averaging_type = nirfmxspecan.TxpAveragingType.RMS

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.select_measurements(
            "",
            nirfmxspecan.MeasurementTypes.ACP | nirfmxspecan.MeasurementTypes.TXP,
            enable_all_traces,
        )

        # ACP
        specan.acp.configuration.configure_averaging("", acp_averaging_enabled, acp_averaging_count, acp_averaging_type)
        specan.acp.configuration.configure_carrier_and_offsets("", integration_bandwidth, NUMBER_OF_OFFSET_CHANNELS, channel_spacing)

        # TXP
        specan.txp.configuration.configure_measurement_interval("", measurement_interval)
        specan.txp.configuration.configure_rbw_filter("", rbw, rbw_filter_type, rrc_alpha)
        specan.txp.configuration.configure_averaging("", txp_averaging_enabled, txp_averaging_count, txp_averaging_type)

        specan.initiate("", "")

        # Fetch ACP
        carrier_selector = nirfmxspecan.SpecAn.build_carrier_string("", 0)
        carrier_abs_power, relative_power, carrier_frequency, res_integration_bw, error_code = (
            specan.acp.results.fetch_carrier_measurement(carrier_selector, timeout)
        )
        offset_selector = nirfmxspecan.SpecAn.build_offset_string("", 0)
        off_ch0_lower_rel, off_ch0_upper_rel, off_ch0_lower_abs, off_ch0_upper_abs, error_code = (
            specan.acp.results.fetch_offset_measurement(offset_selector, timeout)
        )

        offset_selector = nirfmxspecan.SpecAn.build_offset_string("", 1)
        off_ch1_lower_rel, off_ch1_upper_rel, off_ch1_lower_abs, off_ch1_upper_abs, error_code = (
            specan.acp.results.fetch_offset_measurement(offset_selector, timeout)
        )

        # Fetch TXP
        average_mean_power, peak_to_average_ratio, max_power, min_power, error_code = (
            specan.txp.results.fetch_measurement("", timeout)
        )

        print("------------------------ACP-----------------------------\n")
        print(f"{'Carrier Abs Power (dBm or dBm/Hz)':<44}{carrier_abs_power}")
        print(f"{'Offset ch0 Lower Relative Power (dB)':<44}{off_ch0_lower_rel}")
        print(f"{'Offset ch0 Upper Relative Power (dB)':<44}{off_ch0_upper_rel}")
        print(f"{'Offset ch1 Lower Relative Power (dB)':<44}{off_ch1_lower_rel}")
        print(f"{'Offset ch1 Upper Relative Power (dB)':<44}{off_ch1_upper_rel}")

        print("\n------------------------TXP-----------------------------\n")
        print(f"{'Average Mean Power    (dBm)':<44}{average_mean_power}")
        print(f"{'Peak to Average Ratio (dB)':<44}{peak_to_average_ratio}")
        print(f"{'Maximum Power         (dBm)':<44}{max_power}")
        print(f"{'Minimum Power         (dBm)':<44}{min_power}")

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
        description="Pass arguments for ACP TXP Composite Example",
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
