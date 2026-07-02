r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source and Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure External Attenuation Table (frequency vs attenuation).
6. Configure Amplitude Correction Type.
7. Select SEM measurement and enable the traces.
8. Configure SEM Power Units and Reference Type.
9. Configure SEM Averaging.
10. Configure SEM Carrier Integration BW and RBW Filter.
11. Configure SEM Carrier RRC Filter.
12. Configure SEM Number of Offsets.
13. Configure SEM Offset Ranges, RBW Filter, Absolute and Relative Limits.
14. Set Offset Limit Fail Mask.
15. Initiate Measurement.
16. Fetch SEM Measurements.
17. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr

TABLE_SIZE = 3
NUMBER_OF_OFFSETS = 4


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9    # Hz
    reference_level = 0.0       # dBm
    external_attenuation = 0.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10.0e6  # Hz

    # External attenuation table
    frequency_table = numpy.array([997.0e6, 1.0e9, 1.003e9], dtype=numpy.float64)  # Hz
    attenuation_table = numpy.array([0.20, 0.50, 0.30], dtype=numpy.float64)  # dB

    amplitude_correction_type = nirfmxspecan.SemAmplitudeCorrectionType.RF_CENTER_FREQUENCY

    averaging_enabled = nirfmxspecan.SemAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.SemAveragingType.RMS

    integration_bandwidth = 3.84e6  # Hz

    rbw_auto = nirfmxspecan.SemCarrierRbwAutoBandwidth.FALSE
    rbw_filter_type = nirfmxspecan.SemCarrierRbwFilterType.GAUSSIAN
    rbw = 30.0e3  # Hz

    rrc_filter_enabled = nirfmxspecan.SemCarrierRrcFilterEnabled.TRUE
    rrc_alpha = 0.22

    reference_type = nirfmxspecan.SemReferenceType.INTEGRATION
    power_units = nirfmxspecan.SemPowerUnits.DBM

    # Offset settings (4 offsets)
    offset_enabled = [nirfmxspecan.SemOffsetEnabled.TRUE] * NUMBER_OF_OFFSETS
    offset_sideband = [nirfmxspecan.SemOffsetSideband.BOTH] * NUMBER_OF_OFFSETS
    offset_start_frequency = [2.515e6, 4.0e6, 7.5e6, 8.5e6]   # Hz
    offset_stop_frequency = [3.485e6, 7.5e6, 8.5e6, 12.0e6]   # Hz
    offset_rbw_filter_type = [nirfmxspecan.SemOffsetRbwFilterType.GAUSSIAN] * NUMBER_OF_OFFSETS
    offset_rbw_auto = [nirfmxspecan.SemOffsetRbwAutoBandwidth.FALSE] * NUMBER_OF_OFFSETS
    offset_rbw = [30.0e3, 500.0e3, 1.0e6, 1.0e6]  # Hz
    offset_absolute_limit_mode = [nirfmxspecan.SemOffsetAbsoluteLimitMode.COUPLE] * NUMBER_OF_OFFSETS
    offset_absolute_start_limit = [-69.60, -54.30, -54.30, -54.30]  # dBm
    offset_absolute_stop_limit = [-69.60, -54.30, -54.30, -54.30]   # dBm
    offset_relative_limit_mode = [
        nirfmxspecan.SemOffsetRelativeLimitMode.MANUAL,
        nirfmxspecan.SemOffsetRelativeLimitMode.MANUAL,
        nirfmxspecan.SemOffsetRelativeLimitMode.MANUAL,
        nirfmxspecan.SemOffsetRelativeLimitMode.COUPLE,
    ]
    offset_relative_start_limit = [-33.73, -34.00, -37.50, -47.50]  # dB
    offset_relative_stop_limit = [-48.27, -37.50, -47.50, -47.50]   # dB
    offset_limit_fail_mask = nirfmxspecan.SemOffsetLimitFailMask.ABSOLUTE_AND_RELATIVE

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)

        port_string = nirfmxinstr.Session.build_port_string("", selected_ports, "", 0)
        instr_session.configure_external_attenuation_table(port_string, "", frequency_table, attenuation_table)

        specan.select_measurements("", nirfmxspecan.MeasurementTypes.SEM, True)
        specan.sem.configuration.configure_power_units("", power_units)
        specan.sem.configuration.configure_reference_type("", reference_type)
        specan.sem.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.sem.configuration.configure_carrier_integration_bandwidth("", integration_bandwidth)
        specan.sem.configuration.configure_carrier_rbw_filter("", rbw_auto, rbw, rbw_filter_type)
        specan.sem.configuration.configure_carrier_rrc_filter("", rrc_filter_enabled, rrc_alpha)
        specan.sem.configuration.configure_number_of_offsets("", NUMBER_OF_OFFSETS)
        specan.sem.configuration.configure_offset_frequency_array(
            "", offset_start_frequency, offset_stop_frequency, offset_enabled, offset_sideband
        )
        specan.sem.configuration.configure_offset_absolute_limit_array(
            "", offset_absolute_limit_mode, offset_absolute_start_limit, offset_absolute_stop_limit
        )
        specan.sem.configuration.configure_offset_relative_limit_array(
            "", offset_relative_limit_mode, offset_relative_start_limit, offset_relative_stop_limit
        )
        specan.sem.configuration.configure_offset_rbw_filter_array(
            "", offset_rbw_auto, offset_rbw, offset_rbw_filter_type
        )
        offset_string = nirfmxspecan.SpecAn.build_offset_string("", -1)
        specan.sem.configuration.configure_offset_limit_fail_mask(offset_string, offset_limit_fail_mask)
        specan.sem.configuration.set_amplitude_correction_type("", amplitude_correction_type)

        specan.initiate("", "")

        (
            lower_status,
            lower_margin,
            lower_margin_frequency,
            lower_margin_abs_power,
            lower_margin_rel_power,
            error_code,
        ) = specan.sem.results.fetch_lower_offset_margin_array("", timeout)
        (
            upper_status,
            upper_margin,
            upper_margin_frequency,
            upper_margin_abs_power,
            upper_margin_rel_power,
            error_code,
        ) = specan.sem.results.fetch_upper_offset_margin_array("", timeout)

        carrier_abs_power, peak_abs_power, peak_freq, total_rel_power, error_code = (
            specan.sem.results.fetch_carrier_measurement("", timeout)
        )

        absolute_mask = numpy.empty(0, dtype=numpy.float32)
        relative_mask = numpy.empty(0, dtype=numpy.float32)
        spectrum = numpy.empty(0, dtype=numpy.float32)
        specan.sem.results.fetch_absolute_mask_trace("", timeout, absolute_mask)
        specan.sem.results.fetch_relative_mask_trace("", timeout, relative_mask)
        specan.sem.results.fetch_spectrum("", timeout, spectrum)
        composite_status, error_code = specan.sem.results.fetch_composite_measurement_status("", timeout)

        print(f"Measurement status                     :  {composite_status.name}")
        print(f"Carrier Absolute Power (dBm or dBm/Hz) :  {carrier_abs_power}\n")

        print("---------------Lower Offset---------------\n")
        print("Lower Offset Segment Measurements\n")
        for i in range(NUMBER_OF_OFFSETS):
            print(f"Offset : {i}\n")
            print(f"Margin (dB)                            :  {lower_margin[i]}")
            print(f"Margin Absolute Power (dBm)            :  {lower_margin_abs_power[i]}")
            print(f"Margin Relative Power (dB)             :  {lower_margin_rel_power[i]}")
            print(f"Margin Frequency (Hz)                  :  {lower_margin_frequency[i]}")
            print(f"Measurement Status                     :  {lower_status[i].name}")

        print("---------------Upper Offset---------------\n")
        print("Upper Offset Segment Measurements\n")
        for i in range(NUMBER_OF_OFFSETS):
            print(f"Offset : {i}\n")
            print(f"Margin (dB)                            :  {upper_margin[i]}")
            print(f"Margin Absolute Power (dBm)            :  {upper_margin_abs_power[i]}")
            print(f"Margin Relative Power (dB)             :  {upper_margin_rel_power[i]}")
            print(f"Margin Frequency (Hz)                  :  {upper_margin_frequency[i]}")
            print(f"Measurement Status                     :  {upper_status[i].name}")

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
        description="Pass arguments for External Attenuation Table Example",
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
