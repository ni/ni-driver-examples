"""
RFmx LTE UL PVT Non-Contiguous Multi-Carrier Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Subblock Configurations:
   5A. Configure Number of Subblocks.
   5B. Configure subblock Frequency.
   5C. Configure Component Carrier Spacing.
   5D. Configure Number of Component Carriers.
   5E. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
6. Select PvT measurement and enable Traces.
7. Configure Duplex Scheme.
8. Configure Measurements.
9. Configure Averaging Parameters for PvT measurement.
10. Initiate the Measurement.
11. Fetch PvT Measurements and Traces.
12. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxlte
import numpy


def example(resource_name, option_string):
    """LTE UL PVT non-contiguous multi-carrier measurement example."""
    # Configuration parameters
    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    center_frequency = 1.95e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    iq_power_edge_trigger_source = "0"
    iq_power_edge_trigger_level = -20.0  # dB (relative)
    iq_power_edge_trigger_slope = nirfmxlte.IQPowerEdgeTriggerSlope.RISING_SLOPE
    iq_power_edge_trigger_level_type = nirfmxlte.IQPowerEdgeTriggerLevelType.RELATIVE
    trigger_delay = 0.0  # s
    minimum_quiet_time_mode = nirfmxlte.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time_duration = 5.0e-6  # s
    enable_trigger = True

    number_of_subblocks = 2
    number_of_component_carriers = 1

    # Per-subblock configuration
    subblock_frequency = [0.0, 30e6]  # Hz
    subblock_component_carrier_spacing_type = [
        nirfmxlte.ComponentCarrierSpacingType.NOMINAL,
        nirfmxlte.ComponentCarrierSpacingType.NOMINAL,
    ]
    subblock_component_carrier_at_center_frequency = [-1, -1]
    subblock_component_carrier_bandwidth = [[20e6], [20e6]]  # Hz
    subblock_component_carrier_frequency = [[0.0], [0.0]]  # Hz

    duplex_scheme = nirfmxlte.DuplexScheme.TDD
    uplink_downlink_configuration = nirfmxlte.UplinkDownlinkConfiguration.CONFIGURATION_0

    measurement_method = nirfmxlte.PvtMeasurementMethod.NORMAL
    off_power_exclusion_before = 0.0  # s
    off_power_exclusion_after = 0.0  # s
    averaging_enabled = nirfmxlte.PvtAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxlte.PvtAveragingType.RMS

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
            iq_power_edge_trigger_source,
            iq_power_edge_trigger_slope,
            iq_power_edge_trigger_level,
            trigger_delay,
            minimum_quiet_time_mode,
            minimum_quiet_time_duration,
            iq_power_edge_trigger_level_type,
            enable_trigger,
        )

        lte_signal.configure_number_of_subblocks("", number_of_subblocks)

        for i in range(number_of_subblocks):
            subblock_string = nirfmxlte.Lte.build_subblock_string("", i)
            lte_signal.set_subblock_frequency(subblock_string, subblock_frequency[i])
            lte_signal.component_carrier.configure_spacing(
                subblock_string,
                subblock_component_carrier_spacing_type[i],
                subblock_component_carrier_at_center_frequency[i],
            )
            lte_signal.configure_number_of_component_carriers(
                subblock_string, number_of_component_carriers
            )
            lte_signal.component_carrier.configure_array(
                subblock_string,
                subblock_component_carrier_bandwidth[i],
                subblock_component_carrier_frequency[i],
                None,
            )

        lte_signal.select_measurements("", nirfmxlte.MeasurementTypes.PVT, True)

        lte_signal.configure_duplex_scheme("", duplex_scheme, uplink_downlink_configuration)

        lte_signal.pvt.configuration.configure_measurement_method("", measurement_method)

        lte_signal.pvt.configuration.configure_off_power_exclusion_periods(
            "", off_power_exclusion_before, off_power_exclusion_after
        )

        lte_signal.pvt.configuration.configure_averaging(
            "", averaging_enabled, averaging_count, averaging_type
        )

        lte_signal.initiate("", "")

        print("\n********** Measurements ********** ")
        for i in range(number_of_subblocks):
            subblock_string = nirfmxlte.Lte.build_subblock_string("", i)
            (
                measurement_status,
                mean_absolute_off_power_before,
                mean_absolute_off_power_after,
                mean_absolute_on_power,
                burst_width,
                error_code,
            ) = lte_signal.pvt.results.fetch_measurement_array(subblock_string, timeout)

            carrier_string = nirfmxlte.Lte.build_carrier_string(subblock_string, 0)
            signal_power = numpy.empty(0, dtype=numpy.float32)
            absolute_limit = numpy.empty(0, dtype=numpy.float32)
            x0, dx, error_code = lte_signal.pvt.results.fetch_signal_power_trace(
                carrier_string, timeout, signal_power, absolute_limit
            )

            print(f"Subblock                             : {i}")
            for j in range(number_of_component_carriers):
                print(f"Carrier                              : {j}")
                print(f"Status                               : {measurement_status[j].name}\n")
                print(f"Mean Absolute OFF Power Before (dBm) : {mean_absolute_off_power_before[j]}\n")
                print(f"Mean Absolute OFF Power After (dBm)  : {mean_absolute_off_power_after[j]}\n")
                print(f"Mean Absolute ON Power (dBm)         : {mean_absolute_on_power[j]}\n")
                print(f"Burst Width (s)                      : {burst_width[j]}\n")
                print("---------------------------------------------\n")

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


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for LTE UL PVT Non-Contiguous Multi-Carrier Example",
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
