r"""Steps:
1. Open a new RFmx session.
2. Create WCDMA Signal.
3. Configure Selected Ports for WCDMA Signal.
4. Configure the basic signal properties for WCDMA Signal (Center Frequency, Reference Level and External Attenuation).
5. Configure ACP Averaging for WCDMA Signal.
6. Configure ACP Integration BW, Number of Offset Channels, Channel Spacing for WCDMA Signal.
7. Create LTE Signal.
8. Configure Selected Ports for LTE Signal.
9. Configure the basic signal properties for LTE Signal (Center Frequency, Reference Level and External Attenuation).
10. Configure ACP Averaging for LTE Signal.
11. Configure ACP Number of Carriers = "1" for LTE Signal.
12. Configure ACP Carrier Integration BW for LTE Signal.
13. Configure ACP Number of Offsets for LTE Signal.
14. Configure ACP Offset Integration BW, Offset Frequency, Sidebands and RRC Filter for LTE Signal.
15. Read ACP Measurements for WCDMA Signal.
16. Read ACP Measurements for LTE Signal.
17. Delete WCDMA Signal.
18. Delete LTE Signal.
19. Close the RFmx session.
"""

import argparse
import sys

import nirfmxspecan

import nirfmxinstr

NUMBER_OF_OFFSET_CHANNELS = 2
NUMBER_OF_CARRIERS = 1


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    reference_level = 0.0        # dBm
    external_attenuation = 0.0   # dB
    timeout = 10.0               # seconds

    # WCDMA signal settings
    wcdma_center_frequency = 468.0e6      # Hz
    wcdma_integration_bandwidth = 3.84e6  # Hz
    wcdma_channel_spacing = 5.0e6         # Hz
    wcdma_averaging_enabled = nirfmxspecan.AcpAveragingEnabled.FALSE
    wcdma_averaging_count = 10
    wcdma_averaging_type = nirfmxspecan.AcpAveragingType.RMS

    # LTE signal settings
    lte_center_frequency = 2.1e9       # Hz
    lte_integration_bandwidth = 9.0e6  # Hz
    lte_averaging_enabled = nirfmxspecan.AcpAveragingEnabled.FALSE
    lte_averaging_count = 10
    lte_averaging_type = nirfmxspecan.AcpAveragingType.RMS

    offset_channel_enabled = [nirfmxspecan.AcpOffsetEnabled.TRUE] * NUMBER_OF_OFFSET_CHANNELS
    offset_channel_sideband = [nirfmxspecan.AcpOffsetSideband.BOTH] * NUMBER_OF_OFFSET_CHANNELS
    offset_channel_offset = [10.0e6, 7.5e6]         # Hz
    offset_channel_integration_bw = [9.0e6, 3.84e6]  # Hz
    offset_channel_rrc_enabled = [
        nirfmxspecan.AcpOffsetRrcFilterEnabled.FALSE,
        nirfmxspecan.AcpOffsetRrcFilterEnabled.TRUE,
    ]
    offset_channel_rrc_alpha = [0.22, 0.22]

    instr_session = None
    wcdma_signal = None
    lte_signal = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # WCDMA signal
        wcdma_signal = instr_session.get_specan_signal_configuration("WCDMA")
        wcdma_signal.set_selected_ports("", selected_ports)
        wcdma_signal.configure_rf("", wcdma_center_frequency, reference_level, external_attenuation)
        wcdma_signal.acp.configuration.configure_averaging("", wcdma_averaging_enabled, wcdma_averaging_count, wcdma_averaging_type)
        wcdma_signal.acp.configuration.configure_carrier_and_offsets("", wcdma_integration_bandwidth, NUMBER_OF_OFFSET_CHANNELS, wcdma_channel_spacing)

        # LTE signal
        lte_signal = instr_session.get_specan_signal_configuration("LTE")
        lte_signal.set_selected_ports("", selected_ports)
        lte_signal.configure_rf("", lte_center_frequency, reference_level, external_attenuation)
        lte_signal.acp.configuration.configure_averaging("", lte_averaging_enabled, lte_averaging_count, lte_averaging_type)
        lte_signal.acp.configuration.configure_number_of_carriers("", NUMBER_OF_CARRIERS)
        lte_signal.acp.configuration.configure_carrier_integration_bandwidth("", lte_integration_bandwidth)
        lte_signal.acp.configuration.configure_number_of_offsets("", NUMBER_OF_OFFSET_CHANNELS)
        lte_signal.acp.configuration.configure_offset_integration_bandwidth_array("", offset_channel_integration_bw)
        lte_signal.acp.configuration.configure_offset_array("", offset_channel_offset, offset_channel_sideband, offset_channel_enabled)
        lte_signal.acp.configuration.configure_offset_rrc_filter_array("", offset_channel_rrc_enabled, offset_channel_rrc_alpha)

        # Read WCDMA ACP
        (
            wcdma_carrier_abs_power,
            wcdma_off0_lower_rel,
            wcdma_off0_upper_rel,
            wcdma_off1_lower_rel,
            wcdma_off1_upper_rel,
            error_code,
        ) = wcdma_signal.acp.results.read("", timeout)

        # Read LTE ACP
        (
            lte_carrier_abs_power,
            lte_off0_lower_rel,
            lte_off0_upper_rel,
            lte_off1_lower_rel,
            lte_off1_upper_rel,
            error_code,
        ) = lte_signal.acp.results.read("", timeout)

        print("---------------WCDMA Result--------------------\n")
        print(f"Carrier Abs Power (dBm or dBm/Hz)    : {wcdma_carrier_abs_power}")
        print(f"Off ch0 Lower Relative Power (dB)    : {wcdma_off0_lower_rel}")
        print(f"Off ch0 Upper Relative Power(dB)     : {wcdma_off0_upper_rel}")
        print(f"Off ch1 Lower Relative Power(dB)     : {wcdma_off1_lower_rel}")
        print(f"Off ch1 Upper Relative Power(dB)     : {wcdma_off1_upper_rel}")

        print("\n---------------LTE Result----------------------\n")
        print(f"Carrier Abs Power (dBm or dBm/Hz)    : {lte_carrier_abs_power}")
        print(f"Off ch0 Lower Relative Power (dB)    : {lte_off0_lower_rel}")
        print(f"Off ch0 Upper Relative Power(dB)     : {lte_off0_upper_rel}")
        print(f"Off ch1 Lower Relative Power(dB)     : {lte_off1_lower_rel}")
        print(f"Off ch1 Upper Relative Power(dB)     : {lte_off1_upper_rel}")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        if wcdma_signal is not None:
            wcdma_signal.dispose()
        if lte_signal is not None:
            lte_signal.dispose()
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    parser = argparse.ArgumentParser(
        description="Pass arguments for Multiple Signals ACP Example",
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
