"""
RFmx WLAN OFDMModAcc FEM Test Speed Optimized Example

Steps:
1. Open a new RFmx session.
2. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
5. Configure Standard and Channel Bandwidth Properties.
6. Disable auto PPDU type detection and header decoding.
   Configure PPDU Type and properties that are otherwise decoded from the header.
7. Select OFDMModAcc measurement and disable traces.
8. Configure the Measurement Interval. Make sure that the input signal has a number of symbols at
   least equal to the specified Maximum Measurement Length.
9. Configure Frequency Error Estimation Method. You may set Frequency Error Estimation Method to
   Disabled to optimize speed when there is no frequency error between transmitter and receiver.
10. Configure Amplitude Tracking Enabled.
11. Configure Symbol Clock Error Correction Enabled. You may set Symbol Clock Correction Enabled to
    False to optimize speed when there is no symbol clock error between transmitter and receiver.
12. Configure Averaging parameters.
13. Disable burst start detection and I/Q impairments estimation.
14. Initiate Measurement.
15. Fetch OFDMModAcc Measurements.
16. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxwlan


def example(resource_name, option_string):
    """WLAN OFDMModAcc FEM Test speed-optimized measurement example."""

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

    # PPDU overrides — disable auto-detection for faster measurement
    auto_ppdu_type_detection_enabled = nirfmxwlan.OfdmAutoPpduTypeDetectionEnabled.FALSE
    ppdu_type = nirfmxwlan.OfdmPpduType.NON_HT
    header_decoding_enabled = nirfmxwlan.OfdmHeaderDecodingEnabled.FALSE
    mcs_index = 0
    guard_interval_type = nirfmxwlan.OfdmGuardIntervalType.ONE_BY_FOUR
    ltf_size = nirfmxwlan.OfdmLtfSize.OFDM_LTF_SIZE_4X
    ru_size = 26
    number_of_sig_symbols = 1

    # Speed optimizations: disable optional processing steps
    burst_start_detection_enabled = nirfmxwlan.OfdmModAccBurstStartDetectionEnabled.FALSE
    iq_impairments_estimation_enabled = nirfmxwlan.OfdmModAccIQImpairmentsEstimationEnabled.FALSE
    amplitude_tracking_enabled = nirfmxwlan.OfdmModAccAmplitudeTrackingEnabled.FALSE
    symbol_clock_error_correction_enabled = (
        nirfmxwlan.OfdmModAccSymbolClockErrorCorrectionEnabled.FALSE
    )
    frequency_error_estimation_method = (
        nirfmxwlan.OfdmModAccFrequencyErrorEstimationMethod.DISABLED
    )

    averaging_enabled = nirfmxwlan.OfdmModAccAveragingEnabled.FALSE
    averaging_count = 10

    measurement_offset = 0  # symbols
    maximum_measurement_length = 16  # symbols

    timeout = 10.0  # s

    instr_session = None
    wlan_signal = None

    try:
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

        # Disable PPDU auto-detection and header decoding for speed
        wlan_signal.set_ofdm_auto_ppdu_type_detection_enabled("", auto_ppdu_type_detection_enabled)
        wlan_signal.set_ofdm_ppdu_type("", ppdu_type)
        wlan_signal.set_ofdm_header_decoding_enabled("", header_decoding_enabled)
        wlan_signal.set_ofdm_mcs_index("", mcs_index)
        wlan_signal.set_ofdm_guard_interval_type("", guard_interval_type)
        wlan_signal.set_ofdm_ltf_size("", ltf_size)
        wlan_signal.set_ofdm_ru_size("", ru_size)
        wlan_signal.set_ofdm_number_of_sig_symbols("", number_of_sig_symbols)

        # Select OFDMModAcc and disable traces (False = no traces)
        wlan_signal.select_measurements("", nirfmxwlan.MeasurementTypes.OFDMMODACC, False)

        wlan_signal.ofdmmodacc.configuration.configure_measurement_length(
            "", measurement_offset, maximum_measurement_length
        )
        wlan_signal.ofdmmodacc.configuration.configure_frequency_error_estimation_method(
            "", frequency_error_estimation_method
        )
        wlan_signal.ofdmmodacc.configuration.configure_amplitude_tracking_enabled(
            "", amplitude_tracking_enabled
        )
        wlan_signal.ofdmmodacc.configuration.configure_symbol_clock_error_correction_enabled(
            "", symbol_clock_error_correction_enabled
        )
        wlan_signal.ofdmmodacc.configuration.configure_averaging(
            "", averaging_enabled, averaging_count
        )

        # Disable burst start detection and IQ impairments estimation for speed
        wlan_signal.ofdmmodacc.configuration.set_burst_start_detection_enabled(
            "", burst_start_detection_enabled
        )
        wlan_signal.ofdmmodacc.configuration.set_iq_impairments_estimation_enabled(
            "", iq_impairments_estimation_enabled
        )

        wlan_signal.initiate("", "")

        (
            composite_rms_evm_mean,
            composite_data_rms_evm_mean,
            composite_pilot_rms_evm_mean,
            error_code,
        ) = wlan_signal.ofdmmodacc.results.fetch_composite_rms_evm("", timeout)

        number_of_symbols_used, error_code = (
            wlan_signal.ofdmmodacc.results.fetch_number_of_symbols_used("", timeout)
        )

        frequency_error_mean, error_code = (
            wlan_signal.ofdmmodacc.results.fetch_frequency_error_mean("", timeout)
        )

        symbol_clock_error_mean, error_code = (
            wlan_signal.ofdmmodacc.results.fetch_symbol_clock_error_mean("", timeout)
        )

        print("------------------Composite EVM------------------")
        print(f"RMS EVM Mean (dB)                       :{composite_rms_evm_mean}")
        print(f"Data RMS EVM Mean (dB)                  :{composite_data_rms_evm_mean}")
        print(f"Pilot RMS EVM Mean (dB)                 :{composite_pilot_rms_evm_mean}\n")
        print(f"Number of Symbols Used                  :{number_of_symbols_used}")
        print(f"Frequency Error Mean(Hz)                :{frequency_error_mean}")
        print(f"Symbol Clock Error Mean(ppm)            :{symbol_clock_error_mean}")

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
        description="Pass arguments for WLAN OFDMModAcc FEM Test Speed Optimized Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-n", "--resource-name", default="RFSA", help="Resource name of RFSA")
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
