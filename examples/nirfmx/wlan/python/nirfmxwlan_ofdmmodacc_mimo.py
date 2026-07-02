"""
RFmx WLAN OFDMModAcc MIMO Example

Steps:
1. Open a New RFmx session.
2. Configure the Frequency Reference properties (Clock Source And Clock Frequency).
3. Configure Number of Frequency Segment And Receive Chain.
4. Configure Center Frequency for each Segment.
5. Configure Selected Port.
6. Configure the basic signal port specific properties (Reference Level And External Attenuation).
7. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time).
8. Configure Standard And Channel Bandwidth Properties.
9. Select OFDMModAcc measurement And enable the traces.
10. Configure the Measurement Interval.
11. Configure Frequency Error Estimation Method.
12. Configure Amplitude Tracking Enabled.
13. Configure Phase Tracking Enabled.
14. Configure Symbol Clock Error Correction Enabled.
15. Configure Channel Estimation Type.
16. Configure Averaging parameters.
17. Configure Channel Matrix Power Enabled.
18. Initiate Measurement.
19. Fetch OFDMModAcc Measurements.
20. Fetch User Specific Results based on the PPDU Type.
21. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxwlan
import numpy


def example(resource_name, option_string):
    """Run WLAN OFDM ModAcc MIMO Example."""

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
    trigger_delay = 0.0  # seconds
    minimum_quiet_time_mode = nirfmxwlan.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 5.0e-6  # seconds

    standard = nirfmxwlan.Standard.STANDARD_802_11_N

    channel_bandwidth = 20e6  # Hz

    measurement_offset = 0  # symbols
    maximum_measurement_length = 16  # symbols

    frequency_error_estimation_method = (
        nirfmxwlan.OfdmModAccFrequencyErrorEstimationMethod.PREAMBLE_AND_PILOTS
    )
    amplitude_tracking_enabled = nirfmxwlan.OfdmModAccAmplitudeTrackingEnabled.FALSE
    phase_tracking_enabled = nirfmxwlan.OfdmModAccPhaseTrackingEnabled.TRUE
    symbol_clock_error_correction_enabled = (
        nirfmxwlan.OfdmModAccSymbolClockErrorCorrectionEnabled.TRUE
    )
    channel_estimation_type = nirfmxwlan.OfdmModAccChannelEstimationType.REFERENCE
    channel_matrix_power_enabled = nirfmxwlan.OfdmModAccChannelMatrixPowerEnabled.TRUE

    averaging_enabled = nirfmxwlan.OfdmModAccAveragingEnabled.FALSE
    averaging_count = 10

    timeout = 10.0  # seconds

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
            wlan_signal.configure_reference_level(port_string[i], reference_level[i])
            wlan_signal.configure_external_attenuation(port_string[i], external_attenuation[i])

        selected_ports_string_formatted = ",".join(selected_ports_string)
        wlan_signal.configure_selected_ports_multiple("", selected_ports_string_formatted)

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
        wlan_signal.ofdmmodacc.configuration.set_channel_matrix_power_enabled(
            "", channel_matrix_power_enabled
        )

        wlan_signal.initiate("", "")

        # Retrieve results
        (
            composite_rms_evm_mean,
            composite_data_rms_evm_mean,
            composite_pilot_rms_evm_mean,
            error_code,
        ) = wlan_signal.ofdmmodacc.results.fetch_composite_rms_evm("", timeout)

        number_of_symbols_used, error_code = (
            wlan_signal.ofdmmodacc.results.fetch_number_of_symbols_used("", timeout)
        )

        ppdu_type, error_code = wlan_signal.ofdmmodacc.results.fetch_ppdu_type("", timeout)
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

        number_of_stream_results = 0
        mcs_index = []
        number_of_space_time_streams = []

        if ppdu_type == nirfmxwlan.OfdmPpduType.MU:
            number_of_users, error_code = wlan_signal.ofdmmodacc.results.fetch_number_of_users(
                "", timeout
            )
            mcs_index = [0] * number_of_users
            number_of_space_time_streams = [0] * number_of_users

            for i in range(number_of_users):
                user_string = nirfmxwlan.Wlan.build_user_string("", i)
                mcs_index[i], error_code = wlan_signal.ofdmmodacc.results.fetch_mcs_index(
                    user_string, timeout
                )
                number_of_space_time_streams[i], error_code = (
                    wlan_signal.ofdmmodacc.results.fetch_number_of_space_time_streams(
                        user_string, timeout
                    )
                )

                temp_offset, error_code = (
                    wlan_signal.ofdmmodacc.results.get_space_time_stream_offset(user_string)
                )
                temp_offset += number_of_space_time_streams[i]
                if temp_offset > number_of_stream_results:
                    number_of_stream_results = temp_offset
        else:
            mcs_index = [0]
            number_of_space_time_streams = [0]
            mcs_index[0], error_code = wlan_signal.ofdmmodacc.results.fetch_mcs_index("", timeout)
            number_of_space_time_streams[0], error_code = (
                wlan_signal.ofdmmodacc.results.fetch_number_of_space_time_streams("", timeout)
            )
            number_of_stream_results = number_of_space_time_streams[0]

        frequency_error_mean = numpy.zeros(number_of_frequency_segments, dtype=numpy.float64)
        symbol_clock_error_mean = numpy.zeros(number_of_frequency_segments, dtype=numpy.float64)

        stream_rms_evm_mean = numpy.zeros(
            (number_of_frequency_segments, number_of_stream_results), dtype=numpy.float64
        )
        stream_data_rms_evm_mean = numpy.zeros(
            (number_of_frequency_segments, number_of_stream_results), dtype=numpy.float64
        )
        stream_pilot_rms_evm_mean = numpy.zeros(
            (number_of_frequency_segments, number_of_stream_results), dtype=numpy.float64
        )

        cross_power_mean = numpy.zeros(
            (number_of_frequency_segments, number_of_receive_chains), dtype=numpy.float64
        )

        relative_iq_origin_offset_mean = numpy.zeros(
            (number_of_frequency_segments, number_of_receive_chains), dtype=numpy.float64
        )
        iq_gain_imbalance_mean = numpy.zeros(
            (number_of_frequency_segments, number_of_receive_chains), dtype=numpy.float64
        )
        iq_quadrature_error_mean = numpy.zeros(
            (number_of_frequency_segments, number_of_receive_chains), dtype=numpy.float64
        )
        absolute_iq_origin_offset_mean = numpy.zeros(
            (number_of_frequency_segments, number_of_receive_chains), dtype=numpy.float64
        )
        iq_timing_skew_mean = numpy.zeros(
            (number_of_frequency_segments, number_of_receive_chains), dtype=numpy.float64
        )

        for i in range(number_of_frequency_segments):
            segment_string = nirfmxwlan.Wlan.build_segment_string("", i)

            frequency_error_mean[i], error_code = (
                wlan_signal.ofdmmodacc.results.fetch_frequency_error_mean("", timeout)
            )
            symbol_clock_error_mean[i], error_code = (
                wlan_signal.ofdmmodacc.results.fetch_symbol_clock_error_mean("", timeout)
            )

            for j in range(number_of_stream_results):
                stream_string = nirfmxwlan.Wlan.build_stream_string(segment_string, j)

                (
                    stream_rms_evm_mean[i, j],
                    stream_data_rms_evm_mean[i, j],
                    stream_pilot_rms_evm_mean[i, j],
                    error_code,
                ) = wlan_signal.ofdmmodacc.results.fetch_stream_rms_evm(stream_string, timeout)

                stream_rms_evm_per_subcarrier_mean_trace = numpy.empty(0, dtype=numpy.float32)
                x0, dx, error_code = (
                    wlan_signal.ofdmmodacc.results.fetch_stream_rms_evm_per_subcarrier_mean_trace(
                        stream_string, timeout, stream_rms_evm_per_subcarrier_mean_trace
                    )
                )

                pilot_constellation_trace = numpy.empty(0, dtype=numpy.complex64)
                error_code = wlan_signal.ofdmmodacc.results.fetch_pilot_constellation_trace(
                    stream_string, timeout, pilot_constellation_trace
                )

                data_constellation_trace = numpy.empty(0, dtype=numpy.complex64)
                error_code = wlan_signal.ofdmmodacc.results.fetch_data_constellation_trace(
                    stream_string, timeout, data_constellation_trace
                )

            for j in range(number_of_receive_chains):
                chain_string = nirfmxwlan.Wlan.build_chain_string(segment_string, j)

                cross_power_mean[i, j], error_code = (
                    wlan_signal.ofdmmodacc.results.fetch_cross_power(segment_string, timeout)
                )

                (
                    relative_iq_origin_offset_mean[i, j],
                    iq_gain_imbalance_mean[i, j],
                    iq_quadrature_error_mean[i, j],
                    absolute_iq_origin_offset_mean[i, j],
                    iq_timing_skew_mean[i, j],
                    error_code,
                ) = wlan_signal.ofdmmodacc.results.fetch_iq_impairments(segment_string, timeout)

        # Print Results
        print("-----------------------EVM-----------------------\n")
        print("------------------Composite EVM------------------")
        print(f"RMS EVM Mean (dB)                       : {composite_rms_evm_mean}")
        print(f"Data RMS EVM Mean (dB)                  : {composite_data_rms_evm_mean}")
        print(f"Pilot RMS EVM Mean (dB)                 : {composite_pilot_rms_evm_mean}\n")
        print(f"Number of Symbols Used                  : {number_of_symbols_used}\n\n")
        print("--------------------------------------------------\n\n")

        print("---------------------PPDU Info--------------------")
        print(f"PPDU Type                               : {ppdu_type.name}")

        if ppdu_type == nirfmxwlan.OfdmPpduType.MU:
            for i in range(len(number_of_space_time_streams)):
                print(
                    f"NSTS {i}                                  : {number_of_space_time_streams[i]}"
                )
                print(f"MCS Index {i}                             : {mcs_index[i]}")
        else:
            print(f"NSTS                                    : {number_of_space_time_streams[0]}")
            print(f"MCS Index                               : {mcs_index[0]}")

        print(f"Guard Interval Type                     : {guard_interval_type.name}")
        print(f"L-SIG Parity Check Status               : {l_sig_parity_check_status.name}")
        print(f"SIG CRC Status                          : {sig_crc_status.name}")
        print(f"SIG-B CRC Status                        : {sig_b_crc_status.name}\n")
        print("--------------------------------------------------\n\n")

        # Print segment-specific results
        for i in range(number_of_frequency_segments):
            segment_string = nirfmxwlan.Wlan.build_segment_string("", i)
            print(f"------------Measurements for {segment_string}-------------\n")
            print(f"Frequency Error Mean (Hz)               : {frequency_error_mean[i]}")
            print(f"Symbol Clock Error Mean (ppm)           : {symbol_clock_error_mean[i]}\n\n")

            for j in range(number_of_stream_results):
                stream_string = nirfmxwlan.Wlan.build_stream_string(segment_string, j)
                print(f"---------Measurements for {stream_string}--------")
                print(f"Stream RMS EVM Mean (dB)                 : {stream_rms_evm_mean[i, j]}")
                print(
                    f"Stream Pilot RMS EVM Mean (dB)           : {stream_pilot_rms_evm_mean[i, j]}"
                )
                print(
                    f"Stream Data RMS EVM Mean (dB)            : {stream_data_rms_evm_mean[i, j]}\n\n"
                )

            for j in range(number_of_receive_chains):
                chain_string = nirfmxwlan.Wlan.build_chain_string(segment_string, j)
                print(f"---------Measurements for {chain_string}---------")
                print(f"Cross Power Mean (dB)                   : {cross_power_mean[i, j]}\n")
                print("------------------IQ Impairments------------------")
                print(
                    f"Relative I/Q Origin Offset Mean (dB)    : {relative_iq_origin_offset_mean[i, j]}"
                )
                print(
                    f"Absolute I/Q Origin Offset Mean (dBm)   : {absolute_iq_origin_offset_mean[i, j]}"
                )
                print(f"I/Q Gain Imbalance Mean (dB)            : {iq_gain_imbalance_mean[i, j]}")
                print(f"I/Q Quadrature Error Mean (deg)         : {iq_quadrature_error_mean[i, j]}")
                print(f"I/Q Timing Skew Mean (s)                : {iq_timing_skew_mean[i, j]}\n\n")

            print("--------------------------------------------------")

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
        description="Pass arguments for WLAN OFDM ModAcc MIMO Example",
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
