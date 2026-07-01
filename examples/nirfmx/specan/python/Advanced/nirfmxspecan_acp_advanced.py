r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select ACP measurement and enable the traces.
6. Configure ACP Measurement Method, Power Units and Averaging Parameters.
7. Configure ACP FFT.
8. Configure ACP RBW Filter.
9. Configure ACP Sweep Time.
10. Configure ACP Noise Compensation.
11. Configure ACP Number of Carrier Channels.
12. Configure ACP Carrier Channel Settings (Integration BW, Carrier Mode, RRC Filter, Carrier Offset).
13. Configure ACP Number of Offset Channels.
14. Configure ACP Offset Channel Settings (Integration BW, Offset, Power Reference, RRC Filter).
15. Initiate Measurement.
16. Fetch ACP Measurements and Traces.
17. Close the RFmx Session.
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

    rf_attenuation_auto = nirfmxinstr.RFAttenuationAuto.TRUE
    rf_attenuation = 10.0  # dB

    power_units = nirfmxspecan.AcpPowerUnits.DBM
    measurement_method = nirfmxspecan.AcpMeasurementMethod.NORMAL
    noise_compensation_enabled = nirfmxspecan.AcpNoiseCompensationEnabled.FALSE

    sweep_time_auto = nirfmxspecan.AcpSweepTimeAuto.TRUE
    sweep_time_interval = 1.0e-3  # seconds

    rbw_filter_type = nirfmxspecan.AcpRbwFilterType.GAUSSIAN
    rbw_auto = nirfmxspecan.AcpRbwAutoBandwidth.TRUE
    rbw = 10.0e3  # Hz

    fft_window = nirfmxspecan.AcpFftWindow.FLAT_TOP
    fft_padding = -1.0

    averaging_enabled = nirfmxspecan.AcpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.AcpAveragingType.RMS

    enable_all_traces = True

    # Carrier channel settings
    carrier_modes = [nirfmxspecan.AcpCarrierMode.ACTIVE]
    carrier_frequencies = [0.0]      # Hz (offset from carrier)
    carrier_integration_bws = [1.0e6]  # Hz
    carrier_rrc_enabled = [nirfmxspecan.AcpCarrierRrcFilterEnabled.FALSE]
    carrier_rrc_alpha = [0.22]

    # Offset channel settings
    offset_enabled = [nirfmxspecan.AcpOffsetEnabled.TRUE] * NUMBER_OF_OFFSETS
    offset_sideband = [nirfmxspecan.AcpOffsetSideband.BOTH] * NUMBER_OF_OFFSETS
    offset_frequencies = [1.0e6, 2.0e6]   # Hz
    offset_integration_bws = [1.0e6, 1.0e6]  # Hz
    offset_power_reference_carrier = [nirfmxspecan.AcpOffsetPowerReferenceCarrier.CLOSEST] * NUMBER_OF_OFFSETS
    offset_reference_specific = [0] * NUMBER_OF_OFFSETS
    offset_rrc_enabled = [nirfmxspecan.AcpOffsetRrcFilterEnabled.FALSE] * NUMBER_OF_OFFSETS
    offset_rrc_alpha = [0.22] * NUMBER_OF_OFFSETS
    offset_relative_attenuation = [0.0] * NUMBER_OF_OFFSETS
    offset_frequency_definition = [nirfmxspecan.AcpOffsetFrequencyDefinition.CENTER] * NUMBER_OF_OFFSETS

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
        instr_session.configure_rf_attenuation("", rf_attenuation_auto, rf_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.ACP, enable_all_traces)

        specan.acp.configuration.configure_measurement_method("", measurement_method)
        specan.acp.configuration.configure_power_units("", power_units)
        specan.acp.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.acp.configuration.configure_fft("", fft_window, fft_padding)
        specan.acp.configuration.configure_rbw_filter("", rbw_auto, rbw, rbw_filter_type)
        specan.acp.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        specan.acp.configuration.configure_noise_compensation_enabled("", noise_compensation_enabled)
        specan.acp.configuration.configure_number_of_carriers("", NUMBER_OF_CARRIERS)

        for i in range(NUMBER_OF_CARRIERS):
            carrier_str = nirfmxspecan.SpecAn.build_carrier_string("", i)
            specan.acp.configuration.configure_carrier_mode(carrier_str, carrier_modes[i])
            specan.acp.configuration.configure_carrier_frequency(carrier_str, carrier_frequencies[i])
            specan.acp.configuration.configure_carrier_integration_bandwidth(carrier_str, carrier_integration_bws[i])
            specan.acp.configuration.configure_carrier_rrc_filter(carrier_str, carrier_rrc_enabled[i], carrier_rrc_alpha[i])

        specan.acp.configuration.configure_number_of_offsets("", NUMBER_OF_OFFSETS)
        for i in range(NUMBER_OF_OFFSETS):
            offset_str = nirfmxspecan.SpecAn.build_offset_string("", i)
            specan.acp.configuration.configure_offset(
                offset_str, offset_frequencies[i], offset_sideband[i], offset_enabled[i]
            )
            specan.acp.configuration.configure_offset_integration_bandwidth(offset_str, offset_integration_bws[i])
            specan.acp.configuration.configure_offset_power_reference(
                offset_str, offset_power_reference_carrier[i], offset_reference_specific[i]
            )
            specan.acp.configuration.configure_offset_rrc_filter(
                offset_str, offset_rrc_enabled[i], offset_rrc_alpha[i]
            )
            specan.acp.configuration.configure_offset_relative_attenuation(
                offset_str, offset_relative_attenuation[i]
            )
            specan.acp.configuration.configure_offset_frequency_definition(
                offset_str, offset_frequency_definition[i]
            )

        specan.initiate("", "")

        lower_rel_power = numpy.empty(0, dtype=numpy.float64)
        upper_rel_power = numpy.empty(0, dtype=numpy.float64)
        lower_abs_power = numpy.empty(0, dtype=numpy.float64)
        upper_abs_power = numpy.empty(0, dtype=numpy.float64)
        lower_rel_power, upper_rel_power, lower_abs_power, upper_abs_power, error_code = (
            specan.acp.results.fetch_offset_measurement_array("", timeout)
        )

        carrier_abs_power = [0.0] * NUMBER_OF_CARRIERS
        carrier_total_rel_power = [0.0] * NUMBER_OF_CARRIERS
        carrier_res_freq = [0.0] * NUMBER_OF_CARRIERS
        carrier_res_bw = [0.0] * NUMBER_OF_CARRIERS
        for i in range(NUMBER_OF_CARRIERS):
            carrier_str = nirfmxspecan.SpecAn.build_carrier_string("", i)
            carrier_abs_power[i], carrier_total_rel_power[i], carrier_res_freq[i], carrier_res_bw[i], error_code = (
                specan.acp.results.fetch_carrier_measurement(carrier_str, timeout)
            )

        total_carrier_power, error_code = specan.acp.results.fetch_total_carrier_power("", timeout)

        spectrum = numpy.empty(0, dtype=numpy.float32)
        specan.acp.results.fetch_spectrum("", timeout, spectrum)

        print(f"Total Carrier Power (dBm or dBm/Hz)  {total_carrier_power}\n")
        print("Carrier Measurements: \n")
        for i in range(NUMBER_OF_CARRIERS):
            print(f"Carrier {i}:")
            print(f"Absolute Power (dBm or dBm/Hz)       {carrier_abs_power[i]}")
            print(f"Total Relative Power (dB)            {carrier_total_rel_power[i]}")
            print(f"Carrier Offset (Hz)                  {carrier_res_freq[i]}")
            print(f"Integration Bandwidth (Hz)           {carrier_res_bw[i]}")
            print("---------------------------------------------------\n")

        print("Offset Channel Measurements: \n")
        for i in range(NUMBER_OF_OFFSETS):
            print(f"Offset {i}:")
            print(f"Lower Relative Power (dB)            {lower_rel_power[i]}")
            print(f"Upper Relative Power (dB)            {upper_rel_power[i]}")
            print(f"Lower Absolute Power (dBm or dBm/Hz) {lower_abs_power[i]}")
            print(f"Upper Absolute Power (dBm or dBm/Hz) {upper_abs_power[i]}")
            print("-------------------------------------------------\n")

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
        description="Pass arguments for ACP Advanced Example",
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
