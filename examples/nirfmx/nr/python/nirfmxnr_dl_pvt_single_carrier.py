r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure Selected Ports.
4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure Trigger Parameters for IQ Power Edge Trigger.
6. Configure Link Direction as Downlink, Frequency Range, Carrier Bandwidth and BWP Subcarrier Spacing.
7. Configure DL Test Model, DL Test Model Duplex Scheme, and gNodeB Type.
8. Select PVT measurement and enable Traces.
9. Configure Measurement Method, Off Power Exclusion Periods, and Averaging Parameters.
10. Configure Measurement Interval Parameters.
11. Initiate the Measurement.
12. Fetch PVT Measurements and Traces.
13. Close RFmx Session.
"""

import argparse
import sys

import nirfmxnr
import numpy

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 3.5e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10.0e6  # Hz

    iq_power_edge_enabled = True
    iq_power_edge_level = -20.0  # dB or dBm
    trigger_delay = 0.0  # s
    minimum_quiet_time_mode = nirfmxnr.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 8.0e-6  # s

    frequency_range = nirfmxnr.FrequencyRange.RANGE1
    carrier_bandwidth = 100e6  # Hz
    subcarrier_spacing = 30e3  # Hz

    downlink_test_model = nirfmxnr.DownlinkTestModel.TM1_1
    downlink_test_model_duplex_scheme = nirfmxnr.DownlinkTestModelDuplexScheme.TDD
    gnodeb_type = nirfmxnr.gNodeBType.TYPE_1_C
    rated_trp = 0.0  # dBm
    rated_eirp = 0.0  # dBm

    measurement_method = nirfmxnr.PvtMeasurementMethod.NORMAL
    off_power_exclusion_before = 0.0  # s
    off_power_exclusion_after = 0.0  # s

    averaging_enabled = nirfmxnr.PvtAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxnr.PvtAveragingType.RMS

    measurement_interval_auto = nirfmxnr.PvtMeasurementIntervalAuto.TRUE
    measurement_interval = 1.0e-3  # s

    timeout = 10.0  # s

    instr_session = None
    nr = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        nr = instr_session.get_nr_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)
        nr.set_selected_ports("", selected_ports)
        nr.configure_rf("", center_frequency, reference_level, external_attenuation)
        nr.configure_iq_power_edge_trigger(
            "",
            "0",
            nirfmxnr.IQPowerEdgeTriggerSlope.RISING_SLOPE,
            iq_power_edge_level,
            trigger_delay,
            minimum_quiet_time_mode,
            minimum_quiet_time,
            nirfmxnr.IQPowerEdgeTriggerLevelType.RELATIVE,
            iq_power_edge_enabled,
        )

        nr.set_link_direction("", nirfmxnr.LinkDirection.DOWNLINK)
        nr.set_frequency_range("", frequency_range)
        nr.component_carrier.set_bandwidth("", carrier_bandwidth)
        nr.component_carrier.set_bandwidth_part_subcarrier_spacing("", subcarrier_spacing)

        nr.component_carrier.set_downlink_test_model("", downlink_test_model)
        nr.component_carrier.set_downlink_test_model_duplex_scheme("", downlink_test_model_duplex_scheme)
        nr.set_gnodeb_type("", gnodeb_type)
        nr.component_carrier.set_rated_trp("", rated_trp)
        nr.component_carrier.set_rated_eirp("", rated_eirp)

        nr.select_measurements("", nirfmxnr.MeasurementTypes.PVT, True)
        nr.pvt.configuration.configure_measurement_method("", measurement_method)
        nr.pvt.configuration.configure_off_power_exclusion_periods("", off_power_exclusion_before, off_power_exclusion_after)
        nr.pvt.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        nr.pvt.configuration.set_measurement_interval_auto("", measurement_interval_auto)
        nr.pvt.configuration.set_measurement_interval("", measurement_interval)
        nr.initiate("", "")

        # Retrieve results
        measurement_status, error_code = nr.pvt.results.get_measurement_status("")
        peak_windowed_off_power, error_code = nr.pvt.results.get_peak_windowed_off_power("")
        peak_windowed_off_power_margin, error_code = nr.pvt.results.get_peak_windowed_off_power_margin("")
        peak_windowed_off_power_time, error_code = nr.pvt.results.get_peak_windowed_off_power_time("")
        absolute_on_power, error_code = nr.pvt.results.get_absolute_on_power("")

        power_trace = numpy.empty(0, dtype=numpy.float32)
        absolute_limit = numpy.empty(0, dtype=numpy.float32)
        nr.pvt.results.fetch_signal_power_trace("", timeout, power_trace, absolute_limit)

        windowed_signal_power = numpy.empty(0, dtype=numpy.float32)
        nr.pvt.results.fetch_windowed_signal_power_trace("", timeout, windowed_signal_power)

        # Print Results
        print("------------------Measurement------------------\n")
        print(f"Measurement Status                             : {measurement_status.name}")
        print(f"PVT Results Pk Windowed OFF Pwr (dBm/MHz)      : {peak_windowed_off_power}")
        print(f"PVT Results Pk Windowed OFF Pwr Margin (dB)    : {peak_windowed_off_power_margin}")
        print(f"PVT Results Pk Windowed OFF Pwr Time (s)       : {peak_windowed_off_power_time}")
        print(f"Absolute ON Power (dBm)                        : {absolute_on_power}\n")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        if nr is not None:
            nr.dispose()
            nr = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    parser = argparse.ArgumentParser(
        description="Pass arguments for DL PVT Single Carrier Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr.")
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string)


def main():
    _main(sys.argv[1:])


def test_main():
    cmd_line = ["--option-string", ""]
    _main(cmd_line)


def test_example():
    example("RFSA", {})


if __name__ == "__main__":
    main()
