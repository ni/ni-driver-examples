r"""Steps:
1. Open a new RFmx session.
2. Configure Selected Ports.
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure RF Attenuation.
5. Select ACP measurement and enable the traces.
6. Configure ACP Measurement Method (DynamicRange), Power Units and Averaging.
7. Configure ACP RBW Filter and Sweep Time.
8. Configure ACP Noise Compensation Enabled.
9. Configure ACP Carrier Channel Settings (Integration BW, RRC Filter).
10. Configure ACP Number of Offset Channels and Offset Channel Settings.
11. Initiate Measurement.
12. Fetch ACP Measurements.
13. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr

NUMBER_OF_OFFSETS = 2


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1e9   # Hz
    reference_level = 0.00   # dBm
    external_attenuation = 0.00  # dB
    timeout = 10.0  # seconds

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    rf_attenuation_auto = nirfmxinstr.RFAttenuationAuto.TRUE
    rf_attenuation = 10.0  # dB

    measurement_interval = 10e-3  # seconds
    auto_level = True

    carrier_integration_bandwidth = 1.0e6  # Hz
    carrier_rrc_filter_enabled = nirfmxspecan.AcpCarrierRrcFilterEnabled.FALSE
    carrier_rrc_filter_alpha = 0.220

    power_units = nirfmxspecan.AcpPowerUnits.DBM
    measurement_method = nirfmxspecan.AcpMeasurementMethod.DYNAMIC_RANGE
    noise_compensation_enabled = nirfmxspecan.AcpNoiseCompensationEnabled.TRUE

    sweep_time_auto = nirfmxspecan.AcpSweepTimeAuto.TRUE
    sweep_time_interval = 1.0e-3  # seconds

    rbw_filter_type = nirfmxspecan.AcpRbwFilterType.GAUSSIAN
    rbw_auto = nirfmxspecan.AcpRbwAutoBandwidth.TRUE
    rbw = 10.0e3  # Hz

    offset_frequency = numpy.array([1.0e6, 2.0e6])  # Hz
    offset_integration_bandwidth = 1.0e6  # Hz
    offset_rrc_filter_enabled = nirfmxspecan.AcpOffsetRrcFilterEnabled.FALSE
    offset_rrc_filter_alpha = 0.220

    averaging_enabled = nirfmxspecan.AcpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.AcpAveragingType.RMS

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_frequency("", center_frequency)
        specan.configure_external_attenuation("", external_attenuation)
        instr_session.configure_rf_attenuation("", rf_attenuation_auto, rf_attenuation)

        if auto_level:
            auto_set_reference_level, _ = specan.auto_level("", carrier_integration_bandwidth, measurement_interval)
            print(f"Reference Level (dBm)    {auto_set_reference_level}\n")
        else:
            specan.configure_reference_level("", reference_level)

        specan.select_measurements("", nirfmxspecan.MeasurementTypes.ACP, True)
        specan.acp.configuration.configure_measurement_method("", measurement_method)
        specan.acp.configuration.configure_power_units("", power_units)
        specan.acp.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.acp.configuration.configure_rbw_filter("", rbw_auto, rbw, rbw_filter_type)
        specan.acp.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        specan.acp.configuration.configure_noise_compensation_enabled("", noise_compensation_enabled)
        specan.acp.configuration.configure_carrier_integration_bandwidth("", carrier_integration_bandwidth)
        specan.acp.configuration.configure_carrier_rrc_filter("", carrier_rrc_filter_enabled, carrier_rrc_filter_alpha)
        specan.acp.configuration.configure_number_of_offsets("", NUMBER_OF_OFFSETS)
        specan.acp.configuration.configure_offset_array("", offset_frequency, None, None)
        specan.acp.configuration.configure_offset_integration_bandwidth("offset::all", offset_integration_bandwidth)
        specan.acp.configuration.configure_offset_rrc_filter("offset::all", offset_rrc_filter_enabled, offset_rrc_filter_alpha)
        specan.initiate("", "")

        (
            lower_relative_power,
            upper_relative_power,
            lower_absolute_power,
            upper_absolute_power,
            error_code,
        ) = specan.acp.results.fetch_offset_measurement_array("", timeout)

        absolute_power, total_relative_power, carrier_frequency, integration_bandwidth, error_code = (
            specan.acp.results.fetch_carrier_measurement("", timeout)
        )

        spectrum = numpy.empty(0, dtype=numpy.float32)
        specan.acp.results.fetch_spectrum("", timeout, spectrum)

        print("-----------------Carrier Measurements-----------------\n")
        print(f"Absolute Power (dBm or dBm/Hz)         {absolute_power}")

        print("\n--------------Offset Channel Measurements-------------\n")
        for i in range(NUMBER_OF_OFFSETS):
            print(f"----Offset {i}")
            print(f"Lower Relative Power (dB)              {lower_relative_power[i]}")
            print(f"Upper Relative Power (dB)              {upper_relative_power[i]}")
            print(f"Lower Absolute Power (dBm or dBm/Hz)   {lower_absolute_power[i]}")
            print(f"Upper Absolute Power (dBm or dBm/Hz)   {upper_absolute_power[i]}\n")

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
        description="Pass arguments for ACP Dynamic Range Example",
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
