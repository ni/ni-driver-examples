r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select SEM measurement and enable the traces.
6. Configure SEM Power Units and Reference Type.
7. Configure SEM Averaging.
8. Configure SEM Carrier Integration BW and RBW Filter.
9. Configure SEM Carrier RRC Filter.
10. Configure SEM Number of Offsets.
11. Configure SEM Offset Frequency Array (Start, Stop, Enabled, Sideband).
12. Configure SEM Offset Absolute and Relative Limit Arrays.
13. Configure SEM Offset RBW Filter Array.
14. Configure Offset Limit Fail Mask.
15. Initiate Measurement.
16. Fetch SEM Measurements and Traces.
17. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr

NUMBER_OF_OFFSETS = 2


def example(resource_name, option_string):
    """Run Example."""
    # Initialize input variables
    selected_ports = ""
    center_frequency = 1e9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.00  # dB
    timeout = 10.0  # seconds

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    integration_bandwidth = 2.0e6  # Hz
    rbw_auto = nirfmxspecan.SemCarrierRbwAutoBandwidth.FALSE
    rbw_filter_type = nirfmxspecan.SemCarrierRbwFilterType.GAUSSIAN
    rbw = 10.0e3  # Hz

    rrc_filter_enabled = nirfmxspecan.SemCarrierRrcFilterEnabled.FALSE
    rrc_filter_alpha = 0.220

    reference_type = nirfmxspecan.SemReferenceType.INTEGRATION
    power_units = nirfmxspecan.SemPowerUnits.DBM

    averaging_enabled = nirfmxspecan.SemAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.SemAveragingType.RMS

    limit_fail_mask = nirfmxspecan.SemOffsetLimitFailMask.ABSOLUTE

    # Offset arrays
    offset_start_frequency = numpy.array([1.0e6, 2.0e6])  # Hz
    offset_stop_frequency = numpy.array([2.0e6, 3.0e6])   # Hz
    offset_enabled = numpy.array(
        [nirfmxspecan.SemOffsetEnabled.TRUE.value, nirfmxspecan.SemOffsetEnabled.TRUE.value],
        dtype=numpy.int32,
    )
    offset_sideband = numpy.array(
        [nirfmxspecan.SemOffsetSideband.BOTH.value, nirfmxspecan.SemOffsetSideband.BOTH.value],
        dtype=numpy.int32,
    )

    offset_rbw_auto = numpy.array(
        [nirfmxspecan.SemOffsetRbwAutoBandwidth.TRUE.value] * NUMBER_OF_OFFSETS,
        dtype=numpy.int32,
    )
    offset_rbw = numpy.array([10.0e3] * NUMBER_OF_OFFSETS)
    offset_rbw_filter_type = numpy.array(
        [nirfmxspecan.SemOffsetRbwFilterType.GAUSSIAN.value] * NUMBER_OF_OFFSETS,
        dtype=numpy.int32,
    )

    offset_absolute_limit_mode = numpy.array(
        [nirfmxspecan.SemOffsetAbsoluteLimitMode.COUPLE.value] * NUMBER_OF_OFFSETS,
        dtype=numpy.int32,
    )
    offset_absolute_limit_start = numpy.array([-10.0] * NUMBER_OF_OFFSETS)
    offset_absolute_limit_stop = numpy.array([-10.0] * NUMBER_OF_OFFSETS)

    offset_relative_limit_mode = numpy.array(
        [
            nirfmxspecan.SemOffsetRelativeLimitMode.MANUAL.value,
            nirfmxspecan.SemOffsetRelativeLimitMode.COUPLE.value,
        ],
        dtype=numpy.int32,
    )
    offset_relative_limit_start = numpy.array([-10.0, -30.0])
    offset_relative_limit_stop = numpy.array([-30.0, -30.0])

    instr_session = None
    specan = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get SpecAn signal
        specan = instr_session.get_specan_signal_configuration()

        # Configure measurement
        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.SEM, True)
        specan.sem.configuration.configure_power_units("", power_units)
        specan.sem.configuration.configure_reference_type("", reference_type)
        specan.sem.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.sem.configuration.configure_carrier_integration_bandwidth("", integration_bandwidth)
        specan.sem.configuration.configure_carrier_rbw_filter("", rbw_auto, rbw, rbw_filter_type)
        specan.sem.configuration.configure_carrier_rrc_filter("", rrc_filter_enabled, rrc_filter_alpha)
        specan.sem.configuration.configure_number_of_offsets("", NUMBER_OF_OFFSETS)
        specan.sem.configuration.configure_offset_frequency_array(
            "", offset_start_frequency, offset_stop_frequency, offset_enabled, offset_sideband
        )
        specan.sem.configuration.configure_offset_absolute_limit_array(
            "", offset_absolute_limit_mode, offset_absolute_limit_start, offset_absolute_limit_stop
        )
        specan.sem.configuration.configure_offset_relative_limit_array(
            "", offset_relative_limit_mode, offset_relative_limit_start, offset_relative_limit_stop
        )
        specan.sem.configuration.configure_offset_rbw_filter_array(
            "", offset_rbw_auto, offset_rbw, offset_rbw_filter_type
        )
        specan.sem.configuration.configure_offset_limit_fail_mask("offset::all", limit_fail_mask)
        specan.initiate("", "")

        # Retrieve results
        (
            lower_offset_total_absolute_power,
            lower_offset_total_relative_power,
            lower_offset_peak_absolute_power,
            lower_offset_peak_frequency,
            lower_offset_peak_relative_power,
            error_code,
        ) = specan.sem.results.fetch_lower_offset_power_array("", timeout)

        (
            lower_offset_measurement_status,
            lower_offset_margin,
            lower_offset_margin_frequency,
            lower_offset_margin_absolute_power,
            lower_offset_margin_relative_power,
            error_code,
        ) = specan.sem.results.fetch_lower_offset_margin_array("", timeout)

        (
            upper_offset_total_absolute_power,
            upper_offset_total_relative_power,
            upper_offset_peak_absolute_power,
            upper_offset_peak_frequency,
            upper_offset_peak_relative_power,
            error_code,
        ) = specan.sem.results.fetch_upper_offset_power_array("", timeout)

        (
            upper_offset_measurement_status,
            upper_offset_margin,
            upper_offset_margin_frequency,
            upper_offset_margin_absolute_power,
            upper_offset_margin_relative_power,
            error_code,
        ) = specan.sem.results.fetch_upper_offset_margin_array("", timeout)

        absolute_power, peak_absolute_power, peak_frequency, total_relative_power, error_code = (
            specan.sem.results.fetch_carrier_measurement("", timeout)
        )

        absolute_mask = numpy.empty(0, dtype=numpy.float32)
        specan.sem.results.fetch_absolute_mask_trace("", timeout, absolute_mask)

        relative_mask = numpy.empty(0, dtype=numpy.float32)
        specan.sem.results.fetch_relative_mask_trace("", timeout, relative_mask)

        spectrum = numpy.empty(0, dtype=numpy.float32)
        specan.sem.results.fetch_spectrum("", timeout, spectrum)

        composite_measurement_status, error_code = (
            specan.sem.results.fetch_composite_measurement_status("", timeout)
        )

        # Print results in the same style as the .NET example.
        composite_pass = (
            composite_measurement_status == nirfmxspecan.SemCompositeMeasurementStatus.PASS
            or composite_measurement_status == nirfmxspecan.SemCompositeMeasurementStatus.PASS.value
        )
        status = "Pass" if composite_pass else "Fail"
        print(f"Composite measurement status         : {status}\n")

        print("\n--------------Carrier Measurements----------------------------\n")
        print(f"Absolute Power (dBm or dBm/Hz)       : {absolute_power}")
        print(f"Peak Absolute Power (dBm or dbm/Hz)  : {peak_absolute_power}")
        print(f"Peak Frequency                       : {peak_frequency}")

        print("\n--------------Offset segment measurements ---------------------------\n")
        for i in range(NUMBER_OF_OFFSETS):
            print(f"Offset {i}\n")

            print(f"Lower offset : Total Absolute Power (dBm or dBm/Hz):  {lower_offset_total_absolute_power[i]}")
            print(f"Lower offset : Total Relative Power (dB):             {lower_offset_total_relative_power[i]}")
            print(f"Lower Offset : Peak Absolute Power (dBm or dBm/Hz):   {lower_offset_peak_absolute_power[i]}")
            print(f"Lower offset : Peak Frequency (Hz):                   {lower_offset_peak_frequency[i]}")
            print(f"Lower offset : Peak Relative Power (dB):              {lower_offset_peak_relative_power[i]}")
            print(f"Lower Offset : Margin (dB):                           {lower_offset_margin[i]}")
            print(f"Lower offset : Margin Absolute Power (dBm or dBm/Hz): {lower_offset_margin_absolute_power[i]}")
            print(f"Lower offset : Margin Relative Power (dB):            {lower_offset_margin_relative_power[i]}")
            print(f"Lower offset : Margin Frequency (Hz):                 {lower_offset_margin_frequency[i]}")

            lower_pass = (
                lower_offset_measurement_status[i] == nirfmxspecan.SemLowerOffsetMeasurementStatus.PASS
                or lower_offset_measurement_status[i] == nirfmxspecan.SemLowerOffsetMeasurementStatus.PASS.value
            )
            status = "Pass" if lower_pass else "Fail"
            print(f"Lower offset : Measurement Status : {status}\n")

            print(f"\nUpper offset : Total Absolute Power (dBm or dBm/Hz)  : {upper_offset_total_absolute_power[i]}")
            print(f"Upper offset : Total Relative Power (dB):              {upper_offset_total_relative_power[i]}")
            print(f"Upper Offset : Peak Absolute Power (dBm or dBm/Hz):    {upper_offset_peak_absolute_power[i]}")
            print(f"Upper offset : Peak Frequency (Hz):                    {upper_offset_peak_frequency[i]}")
            print(f"Upper offset : Peak Relative Power (dB):               {upper_offset_peak_relative_power[i]}")
            print(f"Upper Offset : Margin (dB):                            {upper_offset_margin[i]}")
            print(f"Upper offset : Margin Absolute Power (dBm or dBm/Hz):  {upper_offset_margin_absolute_power[i]}")
            print(f"Upper offset : Margin Relative Power (dB):             {upper_offset_margin_relative_power[i]}")
            print(f"Upper offset : Margin Frequency (Hz):                  {upper_offset_margin_frequency[i]}")

            upper_pass = (
                upper_offset_measurement_status[i] == nirfmxspecan.SemUpperOffsetMeasurementStatus.PASS
                or upper_offset_measurement_status[i] == nirfmxspecan.SemUpperOffsetMeasurementStatus.PASS.value
            )
            status = "Pass" if upper_pass else "Fail"
            print(f"Upper offset : Measurement Status : {status}\n")
            print("-----------------------------------------------------------------------\n")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close Session
        if specan is not None:
            specan.dispose()
            specan = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for SEM Example",
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
    example("RFSA", "")


if __name__ == "__main__":
    main()
