r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure Selected Ports.
4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure Trigger Parameters for IQ Power Edge Trigger.
6. Configure Link Direction, Frequency Range, Carrier Bandwidth and Subcarrier Spacing.
7. Select OBW measurement and enable Traces.
8. Configure Sweep Time Parameters.
9. Configure Span Parameters for OBW measurement.
10. Configure Averaging Parameters for OBW measurement.
11. Initiate the Measurement.
12. Fetch OBW Measurements and Traces.
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

    iq_power_edge_enabled = False
    iq_power_edge_level = -20.0  # dB or dBm
    trigger_delay = 0.0  # s
    minimum_quiet_time_mode = nirfmxnr.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 8.0e-6  # s

    link_direction = nirfmxnr.LinkDirection.UPLINK
    frequency_range = nirfmxnr.FrequencyRange.RANGE1
    carrier_bandwidth = 100e6  # Hz
    subcarrier_spacing = 30e3  # Hz

    sweep_time_auto = nirfmxnr.ObwSweepTimeAuto.TRUE
    sweep_time_interval = 1.0e-3  # s

    averaging_enabled = nirfmxnr.ObwAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxnr.ObwAveragingType.RMS

    span_auto = nirfmxnr.ObwSpanAuto.TRUE
    span = 200e6  # Hz
    power_integration_method = nirfmxnr.ObwPowerIntegrationMethod.NORMAL

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

        nr.set_link_direction("", link_direction)
        nr.set_frequency_range("", frequency_range)
        nr.component_carrier.set_bandwidth("", carrier_bandwidth)
        nr.component_carrier.set_bandwidth_part_subcarrier_spacing("", subcarrier_spacing)

        nr.select_measurements("", nirfmxnr.MeasurementTypes.OBW, True)

        nr.obw.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        nr.obw.configuration.set_span_auto("", span_auto)
        nr.obw.configuration.set_power_integration_method("", power_integration_method)
        nr.obw.configuration.set_span("subblock0", span)
        nr.obw.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        nr.initiate("", "")

        # Retrieve results
        spectrum = numpy.empty(0, dtype=numpy.float32)
        nr.obw.results.fetch_spectrum("", timeout, spectrum)

        occupied_bandwidth, absolute_power, start_frequency, stop_frequency, error_code = (
            nr.obw.results.fetch_measurement("", timeout)
        )

        # Print Results
        print("----------------- Measurement -----------------\n")
        print(f"Occupied Bandwidth (Hz)   : {occupied_bandwidth}")
        print(f"Absolute Power (dBm)      : {absolute_power}")
        print(f"Start Frequency (Hz)      : {start_frequency}")
        print(f"Stop Frequency (Hz)       : {stop_frequency}\n")

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
        description="Pass arguments for OBW Single Carrier Example",
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
