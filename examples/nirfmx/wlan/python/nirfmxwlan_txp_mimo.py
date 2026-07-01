"""
RFmx WLAN TXP MIMO Example

Steps:
1. Open a new RFmx session.
2. Configure the frequency reference properties (Clock Source and Clock Frequency).
3. Configure Number of Frequency Segment and Receive Chain.
4. Configure the Center Frequency for each Segment.
5. Configure Selected Port.
6. Configure Standard and Channel Bandwidth Properties.
7. Configure Reference Level.
8. Configure the External Attenuation.
9. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
10. Select TXP measurement and enable the traces.
11. Configure the Measurement Interval.
12. Configure Averaging parameters.
13. Initiate Measurement.
14. Fetch TXP Traces and Measurements.
15. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxwlan
import numpy


def example(resource_name, option_string):
    """WLAN TXP MIMO measurement example."""

    number_of_devices = len(resource_name)

    selected_ports = ["", ""]

    frequency_reference_source = "PXI_Clk"
    frequency_reference_frequency = 10e6  # Hz

    number_of_frequency_segments = 1
    number_of_receive_chains = 2

    center_frequency = [5.180000e9, 5.260000e9]  # Hz
    reference_level = [0.0, 0.0]  # dBm
    external_attenuation = [0.0, 0.0]  # dB

    iq_power_edge_enabled = True
    iq_power_edge_level = -20.0  # dB
    trigger_delay = 0.0  # s
    minimum_quiet_time_mode = nirfmxwlan.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 5.0e-6  # s

    standard = nirfmxwlan.Standard.STANDARD_802_11_N

    channel_bandwidth = 20e6  # Hz

    auto_level = True
    measurement_interval = 10e-3  # s
    maximum_measurement_interval = 1e-3  # s

    averaging_enabled = nirfmxwlan.TxpAveragingEnabled.FALSE
    averaging_count = 10

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

        wlan_signal.configure_number_of_frequency_segments_and_receive_chains(
            "", number_of_frequency_segments, number_of_receive_chains
        )

        for i in range(number_of_frequency_segments):
            segment_string = nirfmxwlan.Wlan.build_segment_string("", i)
            wlan_signal.configure_frequency(segment_string, center_frequency[i])

        selected_ports_string = []
        port_string = []

        for i in range(number_of_devices):
            port_str = nirfmxinstr.Session.build_port_string("", "", resource_name[i], 0)
            selected_ports_string.append(port_str)
            port_string.append(port_str)

        wlan_signal.configure_selected_ports_multiple("", ",".join(selected_ports_string))

        wlan_signal.configure_standard("", standard)
        wlan_signal.configure_channel_bandwidth("", channel_bandwidth)

        if auto_level:
            wlan_signal.auto_level("", measurement_interval)
        else:
            for i in range(number_of_devices):
                wlan_signal.configure_reference_level(port_string[i], reference_level[i])

        for i in range(number_of_devices):
            wlan_signal.configure_external_attenuation(port_string[i], external_attenuation[i])

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

        wlan_signal.select_measurements("", nirfmxwlan.MeasurementTypes.TXP, True)

        wlan_signal.txp.configuration.configure_maximum_measurement_interval(
            "", maximum_measurement_interval
        )
        wlan_signal.txp.configuration.configure_averaging("", averaging_enabled, averaging_count)

        wlan_signal.initiate("", "")

        # Retrieve and print results per segment and chain
        for i in range(number_of_frequency_segments):
            segment_string = nirfmxwlan.Wlan.build_segment_string("", i)
            for j in range(number_of_receive_chains):
                chain_string = nirfmxwlan.Wlan.build_chain_string(segment_string, j)

                average_power_mean, peak_power_maximum, error_code = (
                    wlan_signal.txp.results.fetch_measurement(chain_string, timeout)
                )

                power = numpy.empty(0, dtype=numpy.float32)
                x0, dx, error_code = wlan_signal.txp.results.fetch_power_trace(
                    chain_string, timeout, power
                )

                print(f"\n----------Measurement for {chain_string}----------\n")
                print(f"Average Power Mean (dBm)         :{average_power_mean}")
                print(f"Peak Power Maximum (dBm)         :{peak_power_maximum}")
                print()

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
        description="Pass arguments for WLAN TXP MIMO Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n",
        "--resource-name",
        default=["RFSA1", "RFSA2"],
        nargs="+",
        help="Resource names of NI-RFmx Instruments (space-separated for MIMO)",
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
        "RFSA1",
        "RFSA2",
        "--option-string",
        "",
    ]
    _main(cmd_line)


def test_example():
    """Call example function."""
    example(["RFSA1", "RFSA2"], "")


if __name__ == "__main__":
    main()
