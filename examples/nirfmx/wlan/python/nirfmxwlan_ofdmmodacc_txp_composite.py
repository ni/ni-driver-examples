"""
RFmx WLAN OFDMModAcc and TXP Composite Example

Steps:
1. Open a new RFmx session.
2. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
5. Configure Standard and Channel Bandwidth Properties.
6. Select OFDMModAcc and TXP measurements.
7. Configure the Measurement Interval.
8. Configure Averaging parameters for OFDMModAcc.
9. Configure Averaging parameters for TXP.
10. Configure the Maximum Measurement Interval.
11. Initiate Measurement.
12. Fetch OFDMModAcc Measurement.
13. Fetch TXP Measurement.
14. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxwlan


def example(resource_name, option_string):
    """WLAN OFDMModAcc and TXP composite measurement example."""

    # Configuration parameters
    center_frequency = 2.412e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    iq_power_edge_enabled = True
    iq_power_edge_level = -20.0  # dB
    trigger_delay = 0.0  # s
    minimum_quiet_time_mode = nirfmxwlan.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 5.0e-6  # s

    standard = nirfmxwlan.Standard.STANDARD_802_11_AG
    channel_bandwidth = 20e6  # Hz

    measurement_offset = 0  # symbols
    maximum_measurement_length = 16  # symbols

    ofdm_modacc_averaging_enabled = nirfmxwlan.OfdmModAccAveragingEnabled.FALSE
    ofdm_modacc_averaging_count = 10

    txp_averaging_enabled = nirfmxwlan.TxpAveragingEnabled.FALSE
    txp_averaging_count = 10

    maximum_measurement_interval = 1e-3  # s

    timeout = 10.0  # s

    instr_session = None
    wlan_signal = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        wlan_signal = instr_session.get_wlan_signal_configuration()

        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )
        wlan_signal.configure_frequency("", center_frequency)
        wlan_signal.configure_reference_level("", reference_level)
        wlan_signal.configure_external_attenuation("", external_attenuation)

        wlan_signal.configure_iq_power_edge_trigger(
            "",
            "0",
            nirfmxwlan.IQPowerEdgeTriggerSlope.RISING_SLOPE,
            iq_power_edge_level,
            trigger_delay,
            minimum_quiet_time_mode,
            minimum_quiet_time,
            nirfmxwlan.IQPowerEdgeTriggerLevelType.RELATIVE,
            iq_power_edge_enabled,
        )

        wlan_signal.configure_standard("", standard)
        wlan_signal.configure_channel_bandwidth("", channel_bandwidth)

        wlan_signal.select_measurements(
            "",
            nirfmxwlan.MeasurementTypes.OFDMMODACC | nirfmxwlan.MeasurementTypes.TXP,
            True,
        )

        wlan_signal.ofdmmodacc.configuration.configure_measurement_length(
            "", measurement_offset, maximum_measurement_length
        )
        wlan_signal.ofdmmodacc.configuration.configure_averaging(
            "", ofdm_modacc_averaging_enabled, ofdm_modacc_averaging_count
        )

        wlan_signal.txp.configuration.configure_averaging(
            "", txp_averaging_enabled, txp_averaging_count
        )
        wlan_signal.txp.configuration.configure_maximum_measurement_interval(
            "", maximum_measurement_interval
        )

        wlan_signal.initiate("", "")

        (
            composite_rms_evm_mean,
            composite_data_rms_evm_mean,
            composite_pilot_rms_evm_mean,
            error_code,
        ) = wlan_signal.ofdmmodacc.results.fetch_composite_rms_evm("", timeout)

        average_power_mean, peak_power_maximum, error_code = (
            wlan_signal.txp.results.fetch_measurement("", timeout)
        )

        # Print results
        print("------------------OFDMModAcc Measurement------------------\n")
        print(f"RMS EVM Mean (dB)                       :{composite_rms_evm_mean}")
        print(f"Data RMS EVM Mean (dB)                  :{composite_data_rms_evm_mean}")
        print(f"Pilot RMS EVM Mean (dB)                 :{composite_pilot_rms_evm_mean}")
        print("\n----------TXP Measurement----------\n")
        print(f"Average Power Mean (dBm)                :{average_power_mean}")
        print(f"Peak Power Maximum (dBm)                :{peak_power_maximum}")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close Session
        if wlan_signal is not None:
            wlan_signal.dispose()
            wlan_signal = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for WLAN OFDMModAcc TXP Composite Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instrument"
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
        "--resource-name",
        "RFSA",
        "--option-string",
        "",
    ]
    _main(cmd_line)


def test_example():
    """Call example function."""
    example("RFSA", "")


if __name__ == "__main__":
    main()
