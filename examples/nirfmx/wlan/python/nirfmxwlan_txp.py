"""
RFmx WLAN TXP Example

Steps:
1. Open a new RFmx session.
2. Configure the frequency reference properties (Clock Source and Clock Frequency).
3. Configure the basic signal properties (Center Frequency and External Attenuation).
4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
5. Configure Standard and Channel Bandwidth Properties.
6. Configure Reference Level.
7. Select TXP measurement and enable the traces.
8. Configure the Measurement Interval.
9. Configure Averaging parameters.
10. Initiate Measurement.
11. Fetch TXP Traces and Measurements.
12. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxwlan
import numpy


def example(resource_name, option_string):
    """WLAN TXP measurement example."""

    # Configuration parameters
    center_frequency = 2.412e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB
    auto_level = True
    measurement_interval = 10e-3  # s

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    iq_power_edge_enabled = True
    iq_power_edge_level = -20.0  # dB
    trigger_delay = 0.0  # s
    minimum_quiet_time_mode = nirfmxwlan.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 5.0e-6  # s

    standard = nirfmxwlan.Standard.STANDARD_802_11_AG

    channel_bandwidth = 20e6  # Hz

    averaging_enabled = nirfmxwlan.TxpAveragingEnabled.FALSE
    averaging_count = 10

    maximum_measurement_interval = 1e-3  # s

    timeout = 10.0  # seconds

    instr_session = None
    wlan_signal = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get WLAN signal configuration
        wlan_signal = instr_session.get_wlan_signal_configuration()

        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )

        wlan_signal.configure_frequency("", center_frequency)
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

        if auto_level:
            wlan_signal.auto_level("", measurement_interval)
        else:
            wlan_signal.configure_reference_level("", reference_level)

        wlan_signal.select_measurements("", nirfmxwlan.MeasurementTypes.TXP, True)

        wlan_signal.txp.configuration.configure_maximum_measurement_interval(
            "", maximum_measurement_interval
        )
        wlan_signal.txp.configuration.configure_averaging("", averaging_enabled, averaging_count)

        wlan_signal.initiate("", "")

        power = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = wlan_signal.txp.results.fetch_power_trace("", timeout, power)

        average_power_mean, peak_power_maximum, error_code = (
            wlan_signal.txp.results.fetch_measurement("", timeout)
        )

        # Print results
        print("\n----------Measurement----------\n")
        print(f"Average Power Mean (dBm)         :{average_power_mean}")
        print(f"Peak Power Maximum (dBm)         :{peak_power_maximum}")

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
        description="Pass arguments for WLAN TXP Example",
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
