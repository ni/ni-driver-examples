r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Packet Type.
6. Configure Payload Length.
7. Select ModAcc, ACP and TXP measurements.
8. Configure Averaging Parameters for ModAcc measurement.
9. Configure Averaging Parameters for ACP measurement.
10. Configure Averaging Parameters for TXP measurement.
11. Configure ACP Number of Offsets.
12. Configure ACP Offset Channel Mode.
13. Initiate the Measurement.
14. Fetch ModAcc measurements.
15. Fetch ACP measurements.
16. Fetch TXP measurements.
17. Close RFmx Session.
"""

import argparse
import sys

import nirfmxbluetooth
import nirfmxinstr


def example(resource_name, option_string):
    """Run Bluetooth ModAcc ACP TXP EDR Composite Example."""
    # Initialize input variables
    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    center_frequency = 2.402e9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.0  # dB

    enable_trigger = True
    iq_power_edge_trigger_slope = nirfmxbluetooth.IQPowerEdgeTriggerSlope.RISING
    iq_power_edge_trigger_level = -20.0  # dB
    minimum_quiet_time_mode = nirfmxbluetooth.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 100e-6  # seconds
    iq_power_edge_trigger_level_type = nirfmxbluetooth.IQPowerEdgeTriggerLevelType.RELATIVE
    trigger_delay = 0.0  # seconds

    packet_type = nirfmxbluetooth.PacketType.PACKET_TYPE_2_DH1

    payload_length_mode = nirfmxbluetooth.PayloadLengthMode.AUTO
    payload_length = 10  # bytes

    measurements = nirfmxbluetooth.MeasurementTypes.MODACC | nirfmxbluetooth.MeasurementTypes.ACP | nirfmxbluetooth.MeasurementTypes.TXP
    enable_all_traces = False

    averaging_count = 10

    offset_channel_mode = nirfmxbluetooth.AcpOffsetChannelMode.SYMMETRIC
    number_of_offsets = 5

    timeout = 10.0  # seconds

    instr_session = None
    bt_signal = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get Bluetooth signal configuration
        bt_signal = instr_session.get_bluetooth_signal_configuration()

        # Configure frequency reference
        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )

        bt_signal.configure_rf("", center_frequency, reference_level, external_attenuation)

        bt_signal.configure_iq_power_edge_trigger(
            "",
            "0",
            iq_power_edge_trigger_slope,
            iq_power_edge_trigger_level,
            trigger_delay,
            minimum_quiet_time_mode,
            minimum_quiet_time,
            iq_power_edge_trigger_level_type,
            enable_trigger,
        )

        bt_signal.configure_packet_type("", packet_type)
        bt_signal.configure_payload_length("", payload_length_mode, payload_length)

        bt_signal.select_measurements("", measurements, enable_all_traces)

        bt_signal.modacc.configuration.configure_averaging(
            "", nirfmxbluetooth.ModAccAveragingEnabled.FALSE, averaging_count
        )
        bt_signal.acp.configuration.configure_averaging(
            "", nirfmxbluetooth.AcpAveragingEnabled.FALSE, averaging_count
        )
        bt_signal.txp.configuration.configure_averaging(
            "", nirfmxbluetooth.TxpAveragingEnabled.FALSE, averaging_count
        )
        bt_signal.acp.configuration.configure_number_of_offsets("", number_of_offsets)
        bt_signal.acp.configuration.configure_offset_channel_mode("", offset_channel_mode)

        bt_signal.initiate("", "")

        # Retrieve ModAcc results
        (
            peak_rms_devm_maximum,
            peak_devm_maximum,
            ninetynine_percent_devm,
            error_code,
        ) = bt_signal.modacc.results.fetch_devm("", timeout)

        (
            header_frequency_error_wi_maximum,
            peak_frequency_error_wi_plus_w0_maximum,
            peak_frequency_error_w0_maximum,
            error_code,
        ) = bt_signal.modacc.results.fetch_frequency_error_edr("", timeout)

        # Retrieve ACP results
        reference_channel_power, error_code = bt_signal.acp.results.fetch_reference_channel_power(
            "", timeout
        )

        (
            lower_absolute_power,
            upper_absolute_power,
            lower_relative_power,
            upper_relative_power,
            lower_margin,
            upper_margin,
            error_code,
        ) = bt_signal.acp.results.fetch_offset_measurement_array("", timeout)

        # Retrieve TXP results
        (
            average_power_mean,
            average_power_maximum,
            average_power_minimum,
            peak_to_average_power_ratio_maximum,
            error_code,
        ) = bt_signal.txp.results.fetch_powers("", timeout)

        (
            edr_gfsk_average_power_mean,
            edr_dpsk_average_power_mean,
            edr_dpsk_gfsk_average_power_ratio_mean,
            error_code,
        ) = bt_signal.txp.results.fetch_edr_powers("", timeout)

        # Print Results
        print("------------------ModAcc------------------")
        print("------------------DEVM------------------")
        print(f"Peak Rms Devm Maximum (%)                     : {peak_rms_devm_maximum}")
        print(f"Peak Devm Maximum (%)                         : {peak_devm_maximum}")
        print(f"99% Devm (%)                                  : {ninetynine_percent_devm}")
        print("------------------EDR Frequency Error------------------")
        print(
            f"Header Frequency Error wi Maximum (Hz)        : {header_frequency_error_wi_maximum}"
        )
        print(
            f"Peak Frequency Error wi+w0 Maximum (Hz)       : {peak_frequency_error_wi_plus_w0_maximum}"
        )
        print(
            f"Peak Frequency Error w0 Maximum (Hz)          : {peak_frequency_error_w0_maximum}\n"
        )

        print("------------------ACP------------------")
        print(f"Reference Channel Power (dBm)                 : {reference_channel_power} \n")

        print("------------------Offset Measurements------------------")
        for i in range(len(lower_absolute_power)):
            print(f"Offset {i}")
            print(
                f"Lower Absolute Powers (dBm)                   : {lower_absolute_power[i]} "
            )
            print(
                f"Upper Absolute Powers (dBm)                   : {upper_absolute_power[i]} "
            )
            print(
                f"Lower Relative Powers (dB)                    : {lower_relative_power[i]} "
            )
            print(
                f"Upper Relative Powers (dB)                    : {upper_relative_power[i]} "
            )
            print(f"Lower Margin (dB)                             : {lower_margin[i]} ")
            print(f"Upper Margin (dB)                             : {upper_margin[i]} \n")

        print("------------------TXP------------------")
        print(f"Average Power Mean (dBm)                      : {average_power_mean}")
        print(f"Average Power Maximum (dBm)                   : {average_power_maximum}")
        print(f"Average Power Minimum (dBm)                   : {average_power_minimum}")
        print(
            f"Peak to Average Power Ratio Maximum (dB)      : {peak_to_average_power_ratio_maximum}"
        )
        print(f"EDR GFSK Average Power Mean (dBm)             : {edr_gfsk_average_power_mean}")
        print(f"EDR DPSK Average Power Mean (dBm)             : {edr_dpsk_average_power_mean}")
        print(
            f"EDR DPSK GFSK Average Power Ratio Mean (dB)   : {edr_dpsk_gfsk_average_power_ratio_mean}"
        )

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close Session
        if bt_signal is not None:
            bt_signal.dispose()
            bt_signal = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for Bluetooth ModAcc ACP TXP EDR Composite Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr."
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
        "--option-string",
        "",
    ]
    _main(cmd_line)


def test_example():
    """Call example function."""
    options = {}
    example("RFSA", options)


if __name__ == "__main__":
    main()
