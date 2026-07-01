"""
RFmx WLAN OFDMModAcc Trigger Based PPDU Example

Steps:
1. Open a new RFmx session.
2. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
5. Configure Standard to 802.11ax and Channel Bandwidth properties.
6. Configure MCS Index, RU Size, RU Offset, Guard Interval Type, LTF Size and PE Disambiguity.
7. Select OFDMModAcc measurement and enable the traces.
8. Configure Measurement Interval.
9. Configure Unused Tone Error Mask Reference.
10. Configure Averaging parameters.
11. Initiate Measurement.
12. Fetch OFDMModAcc Measurements.
13. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxwlan
import numpy


def example(resource_name, option_string):
    """WLAN OFDM ModAcc Trigger Based PPDU measurement example."""

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

    standard = nirfmxwlan.Standard.STANDARD_802_11_AX
    channel_bandwidth = 20e6  # Hz

    mcs_index = 0
    ru_size = 26
    ru_offset_mru_index = 0
    ru_type = nirfmxwlan.OfdmRUType.RRU
    distribution_bandwidth = 20e6  # Hz
    guard_interval_type = nirfmxwlan.OfdmGuardIntervalType.ONE_BY_FOUR
    ltf_size = nirfmxwlan.OfdmLtfSize.OFDM_LTF_SIZE_4X
    pe_disambiguity = 0

    measurement_offset = 0  # symbols
    maximum_measurement_length = 16  # symbols

    unused_tone_error_mask_reference = nirfmxwlan.OfdmModAccUnusedToneErrorMaskReference.LIMIT1

    averaging_enabled = nirfmxwlan.OfdmModAccAveragingEnabled.FALSE
    averaging_count = 10

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

        # Configure OFDM parameters for 802.11ax
        wlan_signal.set_ofdm_mcs_index("", mcs_index)
        wlan_signal.set_ofdm_ru_size("", ru_size)
        wlan_signal.set_ofdm_ru_offset_mru_index("", ru_offset_mru_index)
        wlan_signal.set_ofdm_ru_type("", ru_type)
        wlan_signal.set_ofdm_distribution_bandwidth("", distribution_bandwidth)
        wlan_signal.set_ofdm_guard_interval_type("", guard_interval_type)
        wlan_signal.set_ofdm_ltf_size("", ltf_size)
        wlan_signal.set_ofdm_pe_disambiguity("", pe_disambiguity)

        wlan_signal.select_measurements("", nirfmxwlan.MeasurementTypes.OFDMMODACC, True)

        wlan_signal.ofdmmodacc.configuration.configure_measurement_length(
            "", measurement_offset, maximum_measurement_length
        )
        wlan_signal.ofdmmodacc.configuration.set_unused_tone_error_mask_reference(
            "", unused_tone_error_mask_reference
        )
        wlan_signal.ofdmmodacc.configuration.configure_averaging(
            "", averaging_enabled, averaging_count
        )

        wlan_signal.initiate("", "")

        # Retrieve results
        (
            composite_rms_evm_mean,
            composite_data_rms_evm_mean,
            composite_pilot_rms_evm_mean,
            error_code,
        ) = wlan_signal.ofdmmodacc.results.fetch_composite_rms_evm("", timeout)

        unused_tone_error_margin, unused_tone_error_margin_ru_index, error_code = (
            wlan_signal.ofdmmodacc.results.fetch_unused_tone_error("", timeout)
        )

        unused_tone_error_margin_per_ru = numpy.empty(0, dtype=numpy.float64)
        error_code = wlan_signal.ofdmmodacc.results.fetch_unused_tone_error_margin_per_ru(
            "", timeout, unused_tone_error_margin_per_ru
        )

        frequency_error_mean, error_code = (
            wlan_signal.ofdmmodacc.results.fetch_frequency_error_mean("", timeout)
        )
        frequency_error_ccdf_10_percent, error_code = (
            wlan_signal.ofdmmodacc.results.fetch_frequency_error_ccdf_10_percent("", timeout)
        )

        symbol_clock_error_mean, error_code = (
            wlan_signal.ofdmmodacc.results.fetch_symbol_clock_error_mean("", timeout)
        )

        ppdu_type, error_code = wlan_signal.ofdmmodacc.results.fetch_ppdu_type("", timeout)

        pilot_constellation = numpy.empty(0, dtype=numpy.complex64)
        error_code = wlan_signal.ofdmmodacc.results.fetch_pilot_constellation_trace(
            "", timeout, pilot_constellation
        )

        data_constellation = numpy.empty(0, dtype=numpy.complex64)
        error_code = wlan_signal.ofdmmodacc.results.fetch_data_constellation_trace(
            "", timeout, data_constellation
        )

        unused_tone_error = numpy.empty(0, dtype=numpy.float32)
        unused_tone_error_mask = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = wlan_signal.ofdmmodacc.results.fetch_unused_tone_error_mean_trace(
            "", timeout, unused_tone_error, unused_tone_error_mask
        )

        # Print results
        print("------------------EVM & Impairments------------------\n")
        print(f"RMS EVM Mean (dB)                       :{composite_rms_evm_mean}")
        print(f"Frequency Error Mean (Hz)               :{frequency_error_mean}")
        print(f"Frequency Error CCDF 10 % (Hz)          :{frequency_error_ccdf_10_percent}")
        print(f"Symbol Clock Error Mean (ppm)           :{symbol_clock_error_mean}")
        print(f"PPDU Type                               :{ppdu_type.name}\n")

        print("------------------Unused Tone Error------------------\n")
        print(f"Margin (dB)                             :{unused_tone_error_margin}")
        print(f"Margin RU Index                         :{unused_tone_error_margin_ru_index}")

        for margin in unused_tone_error_margin_per_ru:
            print(f"Unused Tone Error Margin per RU(dB)     :{margin}")

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
        description="Pass arguments for WLAN OFDM ModAcc Trigger Based PPDU Example",
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
