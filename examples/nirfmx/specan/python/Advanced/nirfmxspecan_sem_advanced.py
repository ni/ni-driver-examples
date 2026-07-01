r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select SEM measurement and enable the traces.
6. Configure SEM Sweep Time.
7. Configure SEM Power Units and Reference Type.
8. Configure SEM Averaging.
9. Configure SEM FFT.
10. Configure SEM Number of Carrier Channels.
11. For each carrier: Configure Carrier Offset, Integration BW, RBW Filter using Selector String.
12. Configure SEM Number of Offsets.
13. For each offset: Configure Offset Frequency, RBW Filter, Limit Fail Mask, Absolute Limit using Selector String.
14. Initiate Measurement.
15. Fetch SEM Lower/Upper Offset Power and Margin arrays for all offsets.
16. Fetch SEM Carrier Measurements for all carriers.
17. Fetch Total Carrier Power and Composite Measurement Status.
18. Print results.
19. Close the RFmx session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr

NUMBER_OF_CARRIERS = 1
NUMBER_OF_OFFSETS = 2


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9    # Hz
    reference_level = 0.0       # dBm
    external_attenuation = 0.0  # dB

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    reference_type = nirfmxspecan.SemReferenceType.INTEGRATION
    power_units = nirfmxspecan.SemPowerUnits.DBM

    sweep_time_auto = nirfmxspecan.SemSweepTimeAuto.TRUE
    sweep_time_interval = 1.0e-3  # seconds

    averaging_enabled = nirfmxspecan.SemAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.SemAveragingType.RMS

    fft_window = nirfmxspecan.SemFftWindow.FLAT_TOP
    fft_padding = -1.0

    enable_all_traces = True

    # Carrier channel settings
    carrier_frequencies = [0.0]           # Hz offset
    carrier_integration_bws = [2.0e6]     # Hz
    carrier_channel_bws = [2.0e6]         # Hz
    carrier_rbw_auto = [nirfmxspecan.SemCarrierRbwAutoBandwidth.TRUE]
    carrier_rbw_filter_type = [nirfmxspecan.SemCarrierRbwFilterType.GAUSSIAN]
    carrier_rbw = [10.0e3]                # Hz
    carrier_rrc_enabled = [nirfmxspecan.SemCarrierRrcFilterEnabled.FALSE]
    carrier_rrc_alpha = [0.22]

    # Offset settings
    offset_enabled = [nirfmxspecan.SemOffsetEnabled.TRUE] * NUMBER_OF_OFFSETS
    offset_sideband = [nirfmxspecan.SemOffsetSideband.BOTH] * NUMBER_OF_OFFSETS
    offset_start_frequency = [1.0e6, 2.0e6]     # Hz
    offset_stop_frequency = [2.0e6, 3.0e6]      # Hz
    offset_rbw_auto = [nirfmxspecan.SemOffsetRbwAutoBandwidth.TRUE] * NUMBER_OF_OFFSETS
    offset_rbw_filter_type = [nirfmxspecan.SemOffsetRbwFilterType.GAUSSIAN] * NUMBER_OF_OFFSETS
    offset_rbw = [10.0e3, 10.0e3]  # Hz
    offset_limit_fail_mask = [nirfmxspecan.SemOffsetLimitFailMask.ABSOLUTE] * NUMBER_OF_OFFSETS
    offset_absolute_limit_mode = [nirfmxspecan.SemOffsetAbsoluteLimitMode.COUPLE] * NUMBER_OF_OFFSETS
    offset_absolute_start_limit = [-10.0, -10.0]  # dBm
    offset_absolute_stop_limit = [-10.0, -10.0]   # dBm
    offset_relative_limit_mode = [
        nirfmxspecan.SemOffsetRelativeLimitMode.MANUAL,
        nirfmxspecan.SemOffsetRelativeLimitMode.COUPLE,
    ]
    offset_relative_start_limit = [-10.0, -30.0]  # dB
    offset_relative_stop_limit = [-30.0, -30.0]   # dB
    offset_frequency_definition = [nirfmxspecan.SemOffsetFrequencyDefinition.CENTER_TO_MEASUREMENT_BANDWIDTH_CENTER] * NUMBER_OF_OFFSETS

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_frequency("", center_frequency)
        specan.configure_reference_level("", reference_level)
        specan.configure_external_attenuation("", external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.SEM, enable_all_traces)
        specan.sem.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        specan.sem.configuration.configure_power_units("", power_units)
        specan.sem.configuration.configure_reference_type("", reference_type)
        specan.sem.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.sem.configuration.configure_fft("", fft_window, fft_padding)
        specan.sem.configuration.configure_number_of_carriers("", NUMBER_OF_CARRIERS)

        for i in range(NUMBER_OF_CARRIERS):
            carrier_str = nirfmxspecan.SpecAn.build_carrier_string("", i)
            specan.sem.configuration.configure_carrier_frequency(carrier_str, carrier_frequencies[i])
            specan.sem.configuration.configure_carrier_integration_bandwidth(carrier_str, carrier_integration_bws[i])
            specan.sem.configuration.configure_carrier_rbw_filter(
                carrier_str, carrier_rbw_auto[i], carrier_rbw[i], carrier_rbw_filter_type[i]
            )
            specan.sem.configuration.configure_carrier_rrc_filter(carrier_str, carrier_rrc_enabled[i], carrier_rrc_alpha[i])
            specan.sem.configuration.configure_carrier_channel_bandwidth(carrier_str, carrier_channel_bws[i])

        specan.sem.configuration.configure_number_of_offsets("", NUMBER_OF_OFFSETS)
        for i in range(NUMBER_OF_OFFSETS):
            offset_str = nirfmxspecan.SpecAn.build_offset_string("", i)
            specan.sem.configuration.configure_offset_frequency_definition(offset_str, offset_frequency_definition[i])
            specan.sem.configuration.configure_offset_limit_fail_mask(offset_str, offset_limit_fail_mask[i])

        specan.sem.configuration.configure_offset_frequency_array(
            "", offset_start_frequency, offset_stop_frequency, offset_enabled, offset_sideband
        )
        specan.sem.configuration.configure_offset_rbw_filter_array(
            "", offset_rbw_auto, offset_rbw, offset_rbw_filter_type
        )
        specan.sem.configuration.configure_offset_absolute_limit_array(
            "", offset_absolute_limit_mode, offset_absolute_start_limit, offset_absolute_stop_limit
        )
        specan.sem.configuration.configure_offset_relative_limit_array(
            "", offset_relative_limit_mode, offset_relative_start_limit, offset_relative_stop_limit
        )

        specan.initiate("", "")

        # Fetch lower offset arrays
        (
            lower_abs_power, lower_rel_power, lower_peak_abs_power,
            lower_peak_freq, lower_peak_rel_power, _
        ) = specan.sem.results.fetch_lower_offset_power_array("", timeout)
        (
            lower_status, lower_margin, lower_margin_freq,
            lower_margin_abs_power, lower_margin_rel_power, _
        ) = specan.sem.results.fetch_lower_offset_margin_array("", timeout)

        # Fetch upper offset arrays
        (
            upper_abs_power, upper_rel_power, upper_peak_abs_power,
            upper_peak_freq, upper_peak_rel_power, _
        ) = specan.sem.results.fetch_upper_offset_power_array("", timeout)
        (
            upper_status, upper_margin, upper_margin_freq,
            upper_margin_abs_power, upper_margin_rel_power, _
        ) = specan.sem.results.fetch_upper_offset_margin_array("", timeout)

        # Fetch per-carrier measurements
        carrier_measurements = []
        for i in range(NUMBER_OF_CARRIERS):
            carrier_str = nirfmxspecan.SpecAn.build_carrier_string("", i)
            abs_power, peak_abs_power, peak_freq, total_rel_power, _ = (
                specan.sem.results.fetch_carrier_measurement(carrier_str, timeout)
            )
            carrier_measurements.append((abs_power, peak_abs_power, peak_freq, total_rel_power))

        absolute_mask = numpy.empty(0, dtype=numpy.float32)
        specan.sem.results.fetch_absolute_mask_trace("", timeout, absolute_mask)

        relative_mask = numpy.empty(0, dtype=numpy.float32)
        specan.sem.results.fetch_relative_mask_trace("", timeout, relative_mask)

        spectrum = numpy.empty(0, dtype=numpy.float32)
        specan.sem.results.fetch_spectrum("", timeout, spectrum)

        total_carrier_power, _ = specan.sem.results.fetch_total_carrier_power("", timeout)
        composite_status, _ = specan.sem.results.fetch_composite_measurement_status("", timeout)

        # Print results
        composite_pass = (
            composite_status == nirfmxspecan.SemCompositeMeasurementStatus.PASS
            or composite_status == nirfmxspecan.SemCompositeMeasurementStatus.PASS.value
        )
        status = "Pass" if composite_pass else "Fail"
        print(f"Composite measurement status:         : {status}")
        print(f"Total Carrier Power (dBm or dBm/Hz)   : {total_carrier_power}\n")

        print("--------------Carrier Measurements-----------------------------\n")
        for i in range(NUMBER_OF_CARRIERS):
            abs_power, peak_abs_power, peak_freq, total_rel_power = carrier_measurements[i]
            print(f"*** Carrier {i} ***\n")
            print(f"Absolute Power (dBm or dBm/Hz)      : {abs_power}")
            print(f"Total Relative Power(dB)            : {total_rel_power}")
            print(f"Peak Absolute Power (dBm or dbm/Hz) : {peak_abs_power}")
            print(f"Peak Frequency                      : {peak_freq}")
            print("-----------------------------------------------------------\n")

        print("--------------Offset segment measurements ---------------------\n")
        for i in range(NUMBER_OF_OFFSETS):
            print(f"*** Offset {i} ***\n")
            print(f"Lower offset : Total Absolute Power (dBm or dBm/Hz)  : {lower_abs_power[i]}")
            print(f"Lower offset : Total Relative Power (dB)             : {lower_rel_power[i]}")
            print(f"Lower Offset : Peak Absolute Power (dBm or dBm/Hz)   : {lower_peak_abs_power[i]}")
            print(f"Lower offset : Peak Frequency (Hz)                   : {lower_peak_freq[i]}")
            print(f"Lower offset : Peak Relative Power (dB)              : {lower_peak_rel_power[i]}")
            print(f"Lower Offset : Margin (dB)                           : {lower_margin[i]}")
            print(f"Lower offset : Margin Absolute Power (dBm or dBm/Hz) : {lower_margin_abs_power[i]}")
            print(f"Lower offset : Margin Relative Power (dB)            : {lower_margin_rel_power[i]}")
            print(f"Lower offset : Margin Frequency (Hz)                 : {lower_margin_freq[i]}")
            lower_pass = (
                lower_status[i] == nirfmxspecan.SemLowerOffsetMeasurementStatus.PASS
                or lower_status[i] == nirfmxspecan.SemLowerOffsetMeasurementStatus.PASS.value
            )
            status = "Pass" if lower_pass else "Fail"
            print(f"Lower offset : Measurement Status                    : {status}\n")

            print(f"\nUpper offset : Total Absolute Power (dBm or dBm/Hz)  : {upper_abs_power[i]}")
            print(f"Upper offset : Total Relative Power (dB)             : {upper_rel_power[i]}")
            print(f"Upper Offset : Peak Absolute Power (dBm or dBm/Hz)   : {upper_peak_abs_power[i]}")
            print(f"Upper offset : Peak Frequency (Hz)                   : {upper_peak_freq[i]}")
            print(f"Upper offset : Peak Relative Power (dB)              : {upper_peak_rel_power[i]}")
            print(f"Upper Offset : Margin (dB)                           : {upper_margin[i]}")
            print(f"Upper offset : Margin Absolute Power (dBm or dBm/Hz) : {upper_margin_abs_power[i]}")
            print(f"Upper offset : Margin Relative Power (dB)            : {upper_margin_rel_power[i]}")
            print(f"Upper offset : Margin Frequency (Hz)                 : {upper_margin_freq[i]}")
            upper_pass = (
                upper_status[i] == nirfmxspecan.SemUpperOffsetMeasurementStatus.PASS
                or upper_status[i] == nirfmxspecan.SemUpperOffsetMeasurementStatus.PASS.value
            )
            status = "Pass" if upper_pass else "Fail"
            print(f"Upper offset : Measurement Status                    : {status}")
            print("-----------------------------------------------------------\n")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        if specan is not None:
            specan.dispose()
            specan = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    parser = argparse.ArgumentParser(
        description="Pass arguments for SEM Advanced Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr.")
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string)


def main():
    _main(sys.argv[1:])


def test_main():
    _main(["--option-string", ""])


def test_example():
    example("RFSA", "")


if __name__ == "__main__":
    main()
