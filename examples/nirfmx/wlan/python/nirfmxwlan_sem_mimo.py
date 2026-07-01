"""
RFmx WLAN SEM MIMO Example

Steps:
1. Open a new RFmx session.
2. Configure the Frequency Reference properties(Clock Source and Clock Frequency).
3. Configure Number of Frequency Segment and Receive Chain.
4. Configure Center Frequency for each Segment.
5. Configure the basic signal port specific properties(Reference Level and External Attenuation).
6. Configure Selected Port.
7. Configure IQ Power Edge Trigger properties(Trigger Delay, IQ Power Edge Level, Min Quiet Time).
8. Configure Standard and Channel Bandwidth Properties.
9. Select SEM measurement and enable the traces.
10. Configure SEM Mask Type.
11. Configure Averaging parameters.
12. Configure Sweep Time and Span parameters.
13. Initiate Measurement.
14. Fetch SEM Traces and Measurements.
15. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxwlan
import numpy


def example(resource_name, option_string):
    """WLAN SEM MIMO measurement example."""
    # Configuration parameters
    number_of_devices = len(resource_name)
    selected_ports = ["", ""]

    frequency_reference_source = "PXI_CLK"
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

    averaging_enabled = nirfmxwlan.SemAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxwlan.SemAveragingType.RMS

    sweep_time_auto = nirfmxwlan.SemSweepTimeAuto.TRUE
    sweep_time = 1.0e-3  # s

    span_auto = nirfmxwlan.SemSpanAuto.TRUE
    span = 66.0e6  # Hz

    timeout = 10.0  # seconds

    instr_session = None
    wlan_signal = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get WLAN signal configuration
        wlan_signal = instr_session.get_wlan_signal_configuration()

        # Configure frequency reference
        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )

        # Configure Number of Frequency Segments and Receive Chains
        wlan_signal.configure_number_of_frequency_segments_and_receive_chains(
            "", number_of_frequency_segments, number_of_receive_chains
        )

        # Configure Center Frequency for each Segment
        for i in range(number_of_frequency_segments):
            segment_string = nirfmxwlan.Wlan.build_segment_string("", i)
            wlan_signal.configure_frequency(segment_string, center_frequency[i])

        # Configure Selected Port and basic signal properties for each device
        selected_ports_string = []
        port_string = []

        for i in range(number_of_devices):
            selected_ports_string.append(
                nirfmxinstr.Session.build_port_string("", selected_ports[i], resource_name[i], 0)
            )
            port_string.append(nirfmxinstr.Session.build_port_string("", "", resource_name[i], 0))
            wlan_signal.configure_reference_level(port_string[i], reference_level[i])
            wlan_signal.configure_external_attenuation(port_string[i], external_attenuation[i])

        # Configure Selected Ports for MIMO
        selected_ports_string_formatted = ",".join(selected_ports_string)
        wlan_signal.configure_selected_ports_multiple("", selected_ports_string_formatted)

        # Configure IQ Power Edge Trigger
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

        # Configure Standard and Channel Bandwidth
        wlan_signal.configure_standard("", standard)
        wlan_signal.configure_channel_bandwidth("", channel_bandwidth)

        # Select SEM measurement and enable traces
        wlan_signal.select_measurements("", nirfmxwlan.MeasurementTypes.SEM, True)

        # Configure SEM measurement parameters
        wlan_signal.sem.configuration.configure_mask_type("", nirfmxwlan.SemMaskType.STANDARD)
        wlan_signal.sem.configuration.configure_averaging(
            "", averaging_enabled, averaging_count, averaging_type
        )
        wlan_signal.sem.configuration.configure_sweep_time("", sweep_time_auto, sweep_time)
        wlan_signal.sem.configuration.configure_span("", span_auto, span)

        # Initiate measurement
        wlan_signal.initiate("", "")

        # Retrieve results

        # Initialize result arrays with proper sizes
        absolute_power = [
            [0.0 for _ in range(number_of_receive_chains)]
            for _ in range(number_of_frequency_segments)
        ]
        relative_power = [
            [0.0 for _ in range(number_of_receive_chains)]
            for _ in range(number_of_frequency_segments)
        ]

        upper_offset_measurement_status = [
            [[] for _ in range(number_of_receive_chains)]
            for _ in range(number_of_frequency_segments)
        ]
        upper_offset_margin = [
            [[] for _ in range(number_of_receive_chains)]
            for _ in range(number_of_frequency_segments)
        ]
        upper_offset_margin_frequency = [
            [[] for _ in range(number_of_receive_chains)]
            for _ in range(number_of_frequency_segments)
        ]
        upper_offset_margin_absolute_power = [
            [[] for _ in range(number_of_receive_chains)]
            for _ in range(number_of_frequency_segments)
        ]
        upper_offset_margin_relative_power = [
            [[] for _ in range(number_of_receive_chains)]
            for _ in range(number_of_frequency_segments)
        ]

        lower_offset_measurement_status = [
            [[] for _ in range(number_of_receive_chains)]
            for _ in range(number_of_frequency_segments)
        ]
        lower_offset_margin = [
            [[] for _ in range(number_of_receive_chains)]
            for _ in range(number_of_frequency_segments)
        ]
        lower_offset_margin_frequency = [
            [[] for _ in range(number_of_receive_chains)]
            for _ in range(number_of_frequency_segments)
        ]
        lower_offset_margin_absolute_power = [
            [[] for _ in range(number_of_receive_chains)]
            for _ in range(number_of_frequency_segments)
        ]
        lower_offset_margin_relative_power = [
            [[] for _ in range(number_of_receive_chains)]
            for _ in range(number_of_frequency_segments)
        ]

        spectrum = [
            [numpy.empty(0, dtype=numpy.float32) for _ in range(number_of_receive_chains)]
            for _ in range(number_of_frequency_segments)
        ]
        composite_mask = [
            [numpy.empty(0, dtype=numpy.float32) for _ in range(number_of_receive_chains)]
            for _ in range(number_of_frequency_segments)
        ]

        # Fetch results for each segment and chain
        for i in range(number_of_frequency_segments):
            segment_string = nirfmxwlan.Wlan.build_segment_string("", i)
            for j in range(number_of_receive_chains):
                chain_string = nirfmxwlan.Wlan.build_chain_string(segment_string, j)

                # Fetch carrier measurement
                absolute_power[i][j], relative_power[i][j], error_code = (
                    wlan_signal.sem.results.fetch_carrier_measurement(chain_string, timeout)
                )

                # Fetch lower offset margin array
                (
                    lower_offset_measurement_status[i][j],
                    lower_offset_margin[i][j],
                    lower_offset_margin_frequency[i][j],
                    lower_offset_margin_absolute_power[i][j],
                    lower_offset_margin_relative_power[i][j],
                    error_code,
                ) = wlan_signal.sem.results.fetch_lower_offset_margin_array(chain_string, timeout)

                # Fetch upper offset margin array
                (
                    upper_offset_measurement_status[i][j],
                    upper_offset_margin[i][j],
                    upper_offset_margin_frequency[i][j],
                    upper_offset_margin_absolute_power[i][j],
                    upper_offset_margin_relative_power[i][j],
                    error_code,
                ) = wlan_signal.sem.results.fetch_upper_offset_margin_array(chain_string, timeout)

                # Fetch spectrum traces
                x0, dx, error_code = wlan_signal.sem.results.fetch_spectrum(
                    chain_string, timeout, spectrum[i][j], composite_mask[i][j]
                )

        # Fetch measurement status
        measurement_status, error_code = wlan_signal.sem.results.fetch_measurement_status(
            "", timeout
        )

        print(f"Measurement Status                          :{measurement_status.name}\n")

        for i in range(number_of_frequency_segments):
            segment_string = nirfmxwlan.Wlan.build_segment_string("", i)
            for j in range(number_of_receive_chains):
                chain_string = nirfmxwlan.Wlan.build_chain_string(segment_string, j)

                print(f"-------Measurement for {chain_string}-------\n\n")
                print(f"Carrier Absolute Power (dBm)                :{absolute_power[i][j]}\n\n")

                print("----------Lower Offset Measurements----------\n")
                if lower_offset_margin[i][j]:
                    for k in range(len(lower_offset_margin[i][j])):
                        print(f"Offset {k}")
                        print(
                            f"Measurement Status              :{lower_offset_measurement_status[i][j][k].name}"
                        )
                        print(f"Margin (dB)                     :{lower_offset_margin[i][j][k]}")
                        print(
                            f"Margin Frequency (Hz)           :{lower_offset_margin_frequency[i][j][k]}"
                        )
                        print(
                            f"Margin Absolute Power (dBm)     :{lower_offset_margin_absolute_power[i][j][k]}\n"
                        )

                print("\n----------Upper Offset Measurements----------\n")
                if upper_offset_margin[i][j]:
                    for k in range(len(upper_offset_margin[i][j])):
                        print(f"Offset {k}")
                        print(
                            f"Measurement Status              :{upper_offset_measurement_status[i][j][k].name}"
                        )
                        print(f"Margin (dB)                     :{upper_offset_margin[i][j][k]}")
                        print(
                            f"Margin Frequency (Hz)           :{upper_offset_margin_frequency[i][j][k]}"
                        )
                        print(
                            f"Margin Absolute Power (dBm)     :{upper_offset_margin_absolute_power[i][j][k]}\n"
                        )

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
        description="Pass arguments for WLAN SEM MIMO Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n",
        "--resource-name",
        default=["RFSA1", "RFSA2"],
        nargs="+",
        help="Resource names of NI-RFmx Instruments for MIMO",
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
