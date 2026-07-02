"""
RFmx LTE NB-IoT ACP Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Carrier Bandwidth.
6. Configure Uplink Subcarrier Spacing.
7. Select ACP measurement and enable Traces.
8. Configure Averaging Parameters for ACP measurement.
9. Configure Sweep Time Parameters.
10. Initiate the Measurement.
11. Fetch ACP Measurements and Traces.
12. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxlte
import numpy


def example(resource_name, option_string):
    """LTE NB-IoT ACP measurement example."""
    # Configuration parameters
    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    center_frequency = 1.95e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    iq_power_edge_source = "0"
    iq_power_edge_slope = nirfmxlte.IQPowerEdgeTriggerSlope.RISING_SLOPE
    iq_power_edge_level = -20.0  # dB
    trigger_delay = 0.0  # s
    minimum_quiet_time_mode = nirfmxlte.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time_duration = 100e-6  # s
    iq_power_edge_level_type = nirfmxlte.IQPowerEdgeTriggerLevelType.RELATIVE
    enable_trigger = True

    component_carrier_bandwidth = 200e3  # Hz (NB-IoT: 200 kHz)
    component_carrier_frequency = 0.0  # Hz
    cell_id = 0

    n_cell_id = 0
    uplink_subcarrier_spacing = nirfmxlte.NBIoTUplinkSubcarrierSpacing.SUBCARRIER_SPACING_15_KHZ

    sweep_time_auto = nirfmxlte.AcpSweepTimeAuto.TRUE
    sweep_time_interval = 0.001  # s

    averaging_enabled = nirfmxlte.AcpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxlte.AcpAveragingType.RMS

    number_of_offsets = 2

    timeout = 10.0  # s

    instr_session = None
    lte_signal = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get LTE signal configuration
        lte_signal = instr_session.get_lte_signal_configuration()

        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )

        lte_signal.configure_rf("", center_frequency, reference_level, external_attenuation)

        lte_signal.configure_iq_power_edge_trigger(
            "",
            iq_power_edge_source,
            iq_power_edge_slope,
            iq_power_edge_level,
            trigger_delay,
            minimum_quiet_time_mode,
            minimum_quiet_time_duration,
            iq_power_edge_level_type,
            enable_trigger,
        )

        lte_signal.component_carrier.configure(
            "", component_carrier_bandwidth, component_carrier_frequency, cell_id
        )

        lte_signal.component_carrier.configure_nb_iot_component_carrier(
            "", n_cell_id, uplink_subcarrier_spacing
        )

        lte_signal.configure_link_direction("", nirfmxlte.LinkDirection.UPLINK)

        lte_signal.select_measurements("", nirfmxlte.MeasurementTypes.ACP, True)

        lte_signal.acp.configuration.configure_averaging(
            "", averaging_enabled, averaging_count, averaging_type
        )

        lte_signal.acp.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)

        lte_signal.initiate("", "")

        (
            lower_relative_power,
            upper_relative_power,
            lower_absolute_power,
            upper_absolute_power,
            error_code,
        ) = lte_signal.acp.results.fetch_offset_measurement_array("", timeout)

        absolute_power, relative_power, error_code = (
            lte_signal.acp.results.component_carrier.fetch_measurement("", timeout)
        )

        for i in range(number_of_offsets):
            absolute_powers_trace = numpy.empty(0, dtype=numpy.float32)
            x0, dx, error_code = lte_signal.acp.results.fetch_absolute_powers_trace(
                "", timeout, i, absolute_powers_trace
            )

        for i in range(number_of_offsets):
            relative_powers_trace = numpy.empty(0, dtype=numpy.float32)
            x0, dx, error_code = lte_signal.acp.results.fetch_relative_powers_trace(
                "", timeout, i, relative_powers_trace
            )

        spectrum = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = lte_signal.acp.results.fetch_spectrum("", timeout, spectrum)

        # Print results
        print(f"Carrier Absolute Power  (dBm)   : {absolute_power}")
        print("\n-----------Offset Channel Measurements-----------")
        for i in range(len(lower_relative_power)):
            print(f"\nOffset  {i}")
            print(f"Lower Relative Power (dB)  : {lower_relative_power[i]}")
            print(f"Upper Relative Power (dB)  : {upper_relative_power[i]}")
            print(f"Lower Absolute Power (dBm) : {lower_absolute_power[i]}")
            print(f"Upper Absolute Power (dBm) : {upper_absolute_power[i]}")
            print("------------------------------------------")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close Session
        if lte_signal is not None:
            lte_signal.dispose()
            lte_signal = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def main():
    """Main entry point."""
    parser = argparse.ArgumentParser(
        description="RFmx LTE NB-IoT ACP Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "--resource-name",
        default="RFSA",
        help="RFSA resource name.",
    )
    parser.add_argument(
        "--option-string",
        default="",
        help="RFSA option string.",
    )
    args = parser.parse_args()
    example(args.resource_name, args.option_string)
    return 0


if __name__ == "__main__":
    sys.exit(main())
