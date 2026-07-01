"""
RFmx WLAN OFDMModAcc with EVM-Based Auto Level Example

Steps:
1. Open a new RFmx session.
2. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
   Reference Level should be set to the peak power in the signal.
4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time).
5. Configure Standard and Channel Bandwidth Properties.
6. Select OFDMModAcc measurement and enable the traces.
7. Configure the Measurement Interval.
8. Configure Frequency Error Estimation Method.
9. Configure Amplitude Tracking Enabled.
10. Configure Phase Tracking Enabled.
11. Configure Symbol Clock Error Correction Enabled.
12. Configure Channel Estimation Type.
13. Configure Averaging parameters.
14. Call OFDMModAcc Auto Level to search for the reference level that gives the best EVM.
15. Initiate Measurement.
16. Fetch OFDMModAcc Measurements.
17. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxwlan
import numpy


def example(resource_name, option_string):
    """WLAN OFDMModAcc with EVM-based Auto Level measurement example."""

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

    frequency_error_estimation_method = (
        nirfmxwlan.OfdmModAccFrequencyErrorEstimationMethod.PREAMBLE_AND_PILOTS
    )
    channel_estimation_type = nirfmxwlan.OfdmModAccChannelEstimationType.REFERENCE
    phase_tracking_enabled = nirfmxwlan.OfdmModAccPhaseTrackingEnabled.TRUE
    amplitude_tracking_enabled = nirfmxwlan.OfdmModAccAmplitudeTrackingEnabled.FALSE
    symbol_clock_error_correction_enabled = (
        nirfmxwlan.OfdmModAccSymbolClockErrorCorrectionEnabled.TRUE
    )

    averaging_enabled = nirfmxwlan.OfdmModAccAveragingEnabled.FALSE
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

        wlan_signal.select_measurements("", nirfmxwlan.MeasurementTypes.OFDMMODACC, True)

        wlan_signal.ofdmmodacc.configuration.configure_measurement_length(
            "", measurement_offset, maximum_measurement_length
        )
        wlan_signal.ofdmmodacc.configuration.configure_frequency_error_estimation_method(
            "", frequency_error_estimation_method
        )
        wlan_signal.ofdmmodacc.configuration.configure_amplitude_tracking_enabled(
            "", amplitude_tracking_enabled
        )
        wlan_signal.ofdmmodacc.configuration.configure_phase_tracking_enabled(
            "", phase_tracking_enabled
        )
        wlan_signal.ofdmmodacc.configuration.configure_symbol_clock_error_correction_enabled(
            "", symbol_clock_error_correction_enabled
        )
        wlan_signal.ofdmmodacc.configuration.configure_channel_estimation_type(
            "", channel_estimation_type
        )
        wlan_signal.ofdmmodacc.configuration.configure_averaging(
            "", averaging_enabled, averaging_count
        )

        # Find the reference level that gives the best EVM
        wlan_signal.ofdmmodacc.configuration.auto_level("", timeout)

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

        (
            relative_iq_origin_offset_mean,
            iq_gain_imbalance_mean,
            iq_quadrature_error_mean,
            absolute_iq_origin_offset_mean,
            iq_timing_skew_mean,
            error_code,
        ) = wlan_signal.ofdmmodacc.results.fetch_iq_impairments("", timeout)

        ppdu_type, error_code = wlan_signal.ofdmmodacc.results.fetch_ppdu_type("", timeout)
        mcs_index, error_code = wlan_signal.ofdmmodacc.results.fetch_mcs_index("", timeout)
        guard_interval_type, error_code = wlan_signal.ofdmmodacc.results.fetch_guard_interval_type(
            "", timeout
        )
        l_sig_parity_check_status, error_code = (
            wlan_signal.ofdmmodacc.results.fetch_l_sig_parity_check_status("", timeout)
        )
        sig_crc_status, error_code = wlan_signal.ofdmmodacc.results.fetch_sig_crc_status(
            "", timeout
        )
        sig_b_crc_status, error_code = wlan_signal.ofdmmodacc.results.fetch_sig_b_crc_status(
            "", timeout
        )

        pilot_constellation = numpy.empty(0, dtype=numpy.complex64)
        error_code = wlan_signal.ofdmmodacc.results.fetch_pilot_constellation_trace(
            "", timeout, pilot_constellation
        )

        data_constellation = numpy.empty(0, dtype=numpy.complex64)
        error_code = wlan_signal.ofdmmodacc.results.fetch_data_constellation_trace(
            "", timeout, data_constellation
        )

        chain_rms_evm_per_subcarrier_mean = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = (
            wlan_signal.ofdmmodacc.results.fetch_chain_rms_evm_per_subcarrier_mean_trace(
                "", timeout, chain_rms_evm_per_subcarrier_mean
            )
        )

        # Print results
        print("------------------EVM------------------\n")
        print("------------------Composite EVM------------------")
        print(f"RMS EVM Mean (dB)                       : {composite_rms_evm_mean}")
        print(f"Data RMS EVM Mean (dB)                  : {composite_data_rms_evm_mean}")
        print(f"Pilot RMS EVM Mean (dB)                 : {composite_pilot_rms_evm_mean}\n")
        print(f"Number of Symbols Used                  : {number_of_symbols_used}")
        print("\n------------------Impairments & PPDU Info------------------\n")
        print(f"Frequency Error Mean (Hz)               : {frequency_error_mean}")
        print(f"Symbol Clock Error Mean (ppm)           : {symbol_clock_error_mean}")
        print("\n------------------IQ Impairments------------------")
        print(f"Relative I/Q Origin Offset Mean (dB)    : {relative_iq_origin_offset_mean}")
        print(f"Absolute I/Q Origin Offset Mean (dBm)   : {absolute_iq_origin_offset_mean}")
        print(f"I/Q Gain Imbalance Mean (dB)            : {iq_gain_imbalance_mean}")
        print(f"I/Q Quadrature Error Mean (deg)         : {iq_quadrature_error_mean}")
        print(f"I/Q Timing Skew Mean (s)                : {iq_timing_skew_mean}")
        print("\n------------------PPDU Info------------------")
        print(f"PPDU Type                               : {ppdu_type.name}")
        print(f"MCS Index                               : {mcs_index}")
        print(f"Guard Interval Type                     : {guard_interval_type.name}")
        print(f"L-SIG Parity Check Status               : {l_sig_parity_check_status.name}")
        print(f"SIG CRC Status                          : {sig_crc_status.name}")
        print(f"SIG-B CRC Status                        : {sig_b_crc_status.name}\n")

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
        description="Pass arguments for WLAN OFDMModAcc with EVM-Based Auto Level Example",
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
