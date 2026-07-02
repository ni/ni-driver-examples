"""
RFmx WLAN OFDMModAcc MIMO Analysis Only Example

Steps:
1. Open a new RFmx session in Analysis-Only mode.
   Note: To configure for more than 4 waveforms, set MaxNumWfms in the option string.
2. Configure Number of Frequency Segment and Receive Chain.
3. Configure Center Frequency for each Segment.
4. Configure Standard and Channel Bandwidth Properties.
5. Select OFDMModAcc measurement and enable all the traces.
6. Configure the Measurement Interval.
7. Configure Frequency Error Estimation Method.
8. Configure Amplitude Tracking Enabled.
9. Configure Phase Tracking Enabled.
10. Configure Symbol Clock Error Correction Enabled.
11. Configure Channel Estimation Type.
12. Read the MIMO Waveform from the .tdms file using nptdms.
13. Call AnalyzeNWaveformsIQ to perform the measurement.
14. Fetch OFDMModAcc Measurements.
15. Fetch the User specific results based on PPDU Type.
16. Close the RFmx Session.

Prerequisites:
- nptdms package: pip install nptdms
- TDMS file: support/WLAN_80211n_20MHz_1Seg_2Chain_MIMO.tdms (located in the support
  subfolder next to this example file)
"""

import argparse
import os
import sys

import nirfmxinstr

_SUPPORT_DIR = os.path.join(os.path.dirname(os.path.abspath(__file__)), "support")
import nirfmxwlan
import numpy


def _read_iq_waveforms_from_tdms(file_path, number_of_waveforms):
    """Read N complex IQ waveforms from a TDMS file using nptdms.

    Returns a tuple (x0_list, dx_list, iq_list) where:
      x0_list: list of start times per waveform (seconds)
      dx_list: list of sample intervals per waveform (seconds)
      iq_list: list of numpy.complex64 arrays, one per waveform
    """
    try:
        from nptdms import TdmsFile
    except ImportError as e:
        raise ImportError(
            "nptdms package is required for analysis-only examples. "
            "Install it with: pip install nptdms"
        ) from e

    tdms_file = TdmsFile.read(file_path)
    iq_list = []
    x0_list = []
    dx_list = []

    # one channel per receive chain, with interleaved IQ data [I0, Q0, I1, Q1, ...]
    waveforms_group = tdms_file["waveforms"]
    channels = waveforms_group.channels()
    for i in range(number_of_waveforms):
        channel = channels[i]
        raw = channel[:].astype(numpy.float64)
        # Data is interleaved: I samples at even indices, Q samples at odd indices
        i_data = raw[0::2].astype(numpy.float32)
        q_data = raw[1::2].astype(numpy.float32)
        iq_waveform = (i_data + 1j * q_data).astype(numpy.complex64)
        iq_list.append(iq_waveform)

        props = channel.properties
        x0_list.append(float(props.get("t0", 0.0)))
        dx_list.append(float(props.get("dt", 1.0 / 20e6)))

    return x0_list, dx_list, iq_list


def example(resource_name, option_string, waveform_file_path=None):
    if waveform_file_path is None:
        waveform_file_path = os.path.join(_SUPPORT_DIR, "WLAN_80211n_20MHz_1Seg_2Chain_MIMO.tdms")
    """WLAN OFDMModAcc MIMO Analysis Only measurement example."""

    number_of_waveforms = 2
    number_of_frequency_segments = 1
    number_of_receive_chains = 2

    center_frequency = [5.18e9, 5.26e9]  # Hz

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

    timeout = 10.0  # s

    instr_session = None
    wlan_signal = None

    try:
        # Create a new RFmx session in Analysis-Only mode
        analysis_only_option = "AnalysisOnly=1;MaxNumWfms:8"
        if option_string:
            analysis_only_option = f"{option_string};{analysis_only_option}"

        instr_session = nirfmxinstr.Session("", analysis_only_option)

        wlan_signal = instr_session.get_wlan_signal_configuration()

        wlan_signal.configure_number_of_frequency_segments_and_receive_chains(
            "", number_of_frequency_segments, number_of_receive_chains
        )

        for i in range(number_of_frequency_segments):
            segment_string = nirfmxwlan.Wlan.build_segment_string("", i)
            wlan_signal.configure_frequency(segment_string, center_frequency[i])

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

        # Read waveforms from TDMS file and submit for analysis
        x0_list, dx_list, iq_list = _read_iq_waveforms_from_tdms(waveform_file_path, number_of_waveforms)
        wlan_signal.analyze_n_waveforms_iq("", "", x0_list, dx_list, iq_list, True)

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

        # Print results
        print("-----------------------EVM-----------------------\n")
        print("------------------Composite EVM------------------")
        print(f"RMS EVM Mean (dB)                       : {composite_rms_evm_mean}")
        print(f"Data RMS EVM Mean (dB)                  : {composite_data_rms_evm_mean}")
        print(f"Pilot RMS EVM Mean (dB)                 : {composite_pilot_rms_evm_mean}\n")
        print(f"Number of Symbols Used                  : {number_of_symbols_used}\n")
        print("\n--------------------------------------------------\n\n")

        for i in range(number_of_frequency_segments):
            segment_string = nirfmxwlan.Wlan.build_segment_string("", i)
            print(f"------------Measurements for {segment_string}-------------\n")

            for j in range(number_of_stream_results):
                stream_string = nirfmxwlan.Wlan.build_stream_string(segment_string, j)
                (
                    stream_rms_evm_mean,
                    stream_data_rms_evm_mean,
                    stream_pilot_rms_evm_mean,
                    error_code,
                ) = wlan_signal.ofdmmodacc.results.fetch_stream_rms_evm(stream_string, timeout)

                print(f"\n---------Measurements for {stream_string}--------")
                print(f"Stream RMS EVM Mean (dB)                 : {stream_data_rms_evm_mean}")
                print(f"Stream Pilot RMS EVM Mean (dB)           : {stream_pilot_rms_evm_mean}")
                print(f"Stream Data RMS EVM Mean (dB)            : {stream_data_rms_evm_mean}\n")

            for j in range(number_of_receive_chains):
                chain_string = nirfmxwlan.Wlan.build_chain_string(segment_string, j)
                cross_power_mean, error_code = (
                    wlan_signal.ofdmmodacc.results.fetch_cross_power(chain_string, timeout)
                )
                print(f"\n---------Measurements for {chain_string}---------")
                print(f"Cross Power Mean (dB)                   : {cross_power_mean}")

        print("\n--------------------------------------------------\n\n")
        print("---------------------Impairments & PPDU Info--------------------")

        for i in range(number_of_frequency_segments):
            segment_string = nirfmxwlan.Wlan.build_segment_string("", i)
            frequency_error_mean, error_code = (
                wlan_signal.ofdmmodacc.results.fetch_frequency_error_mean(segment_string, timeout)
            )
            symbol_clock_error_mean, error_code = (
                wlan_signal.ofdmmodacc.results.fetch_symbol_clock_error_mean(
                    segment_string, timeout
                )
            )

            print(f"\n---------Measurements for {segment_string}---------\n")
            print(f"Frequency Error Mean (Hz)               : {frequency_error_mean}")
            print(f"Symbol Clock Error Mean (ppm)           : {symbol_clock_error_mean}\n")

            for j in range(number_of_receive_chains):
                chain_string = nirfmxwlan.Wlan.build_chain_string(segment_string, j)
                (
                    relative_iq_origin_offset_mean,
                    iq_gain_imbalance_mean,
                    iq_quadrature_error_mean,
                    absolute_iq_origin_offset_mean,
                    iq_timing_skew_mean,
                    error_code,
                ) = wlan_signal.ofdmmodacc.results.fetch_iq_impairments(chain_string, timeout)

                print(f"\n------------------IQ Impairments for {chain_string}----------")
                print(
                    f"Relative I/Q Origin Offset Mean (dB)    : {relative_iq_origin_offset_mean}"
                )
                print(
                    f"Absolute I/Q Origin Offset Mean (dBm)   : {absolute_iq_origin_offset_mean}"
                )
                print(f"I/Q Gain Imbalance Mean (dB)            : {iq_gain_imbalance_mean}")
                print(
                    f"I/Q Quadrature Error Mean (deg)         : {iq_quadrature_error_mean}"
                )
                print(f"I/Q Timing Skew Mean (s)                : {iq_timing_skew_mean}\n")

        print("\n--------------------------------------------------\n\n")
        print("---------------------PPDU Info--------------------")
        print(f"PPDU Type                               : {ppdu_type.name}")

        if ppdu_type == nirfmxwlan.OfdmPpduType.MU:
            for i in range(len(number_of_space_time_streams)):
                print(f"\nNSTS {i}                                  : {number_of_space_time_streams[i]}")
                print(f"MCS Index {i}                             : {mcs_index[i]}\n")
        else:
            print(f"NSTS                                    : {number_of_space_time_streams[0]}")
            print(f"MCS Index                               : {mcs_index[0]}")

        print(f"Guard Interval Type                     : {guard_interval_type.name}")
        print(f"L-SIG Parity Check Status               : {l_sig_parity_check_status.name}")
        print(f"SIG CRC Status                          : {sig_crc_status.name}")
        print(f"SIG-B CRC Status                        : {sig_b_crc_status.name}")
        print("\n--------------------------------------------------\n\n")

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
        description="Pass arguments for WLAN OFDMModAcc MIMO Analysis Only Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="", help="Resource name (empty for Analysis-Only mode)"
    )
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    parser.add_argument(
        "-f",
        "--waveform-file-path",
        default=None,
        help="Path to the MIMO TDMS waveform file (default: support/WLAN_80211n_20MHz_1Seg_2Chain_MIMO.tdms)",
    )
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string, args.waveform_file_path)


def main():
    """Call _main function."""
    _main(sys.argv[1:])


def test_main():
    """Call _main function with empty option string."""
    cmd_line = [
        "--resource-name",
        "",
        "--option-string",
        "",
    ]
    _main(cmd_line)


def test_example():
    """Call example function."""
    example("", "")


if __name__ == "__main__":
    main()
