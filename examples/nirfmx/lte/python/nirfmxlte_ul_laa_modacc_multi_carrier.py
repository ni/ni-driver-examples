"""
RFmx LTE UL LAA ModAcc Multi-Carrier Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Component Carrier Spacing.
6. Configure operating Band to 46.
7. Configure Duplex Mode to LAA.
8. Configure Component Carriers settings.
9. Configure Auto DMRS Detection Enabled.
10. Select ModAcc measurement and enable Traces.
11. Configure Synchronization Mode and Measurement Interval.
12. Configure EVM Unit.
13. Configure Averaging Parameters for ModAcc measurement.
14. Initiate the Measurement.
15. Fetch ModAcc Measurements and Traces.
16. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxlte
import numpy


def example(resource_name, option_string):
    """LTE UL LAA ModAcc multi-carrier measurement example."""
    # Configuration parameters
    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    center_frequency = 1.95e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    iq_power_edge_trigger_source = "0"
    enable_trigger = True
    iq_power_edge_trigger_level = -20.0  # dB (relative)
    iq_power_edge_trigger_slope = nirfmxlte.IQPowerEdgeTriggerSlope.RISING_SLOPE
    iq_power_edge_trigger_level_type = nirfmxlte.IQPowerEdgeTriggerLevelType.RELATIVE
    trigger_delay = 0.0  # s
    minimum_quiet_time_mode = nirfmxlte.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time_duration = 50.0e-6  # s

    number_of_component_carriers = 2
    component_carrier_spacing_type = nirfmxlte.ComponentCarrierSpacingType.NOMINAL
    component_carrier_at_center_frequency = -1

    component_carrier_bandwidth = [20e6, 20e6]  # Hz
    component_carrier_frequency = [-9.9e6, 9.9e6]  # Hz
    component_carrier_cell_id = [0, 0]

    laa_starting_subframe = [0, 0]
    laa_number_of_subframes = [1, 1]
    laa_uplink_start_position = [
        nirfmxlte.LaaUplinkStartPosition.START_POSITION_00,
        nirfmxlte.LaaUplinkStartPosition.START_POSITION_00,
    ]
    laa_uplink_ending_symbol = [
        nirfmxlte.LaaUplinkEndingSymbol.ENDING_SYMBOL_13,
        nirfmxlte.LaaUplinkEndingSymbol.ENDING_SYMBOL_13,
    ]

    duplex_scheme = nirfmxlte.DuplexScheme.LAA
    uplink_downlink_configuration = nirfmxlte.UplinkDownlinkConfiguration.CONFIGURATION_0
    band = 46

    auto_dmrs_detection_enabled = nirfmxlte.AutoDmrsDetectionEnabled.TRUE

    synchronization_mode = nirfmxlte.ModAccSynchronizationMode.SLOT
    measurement_offset = 0  # slots
    measurement_length = 1  # slots

    evm_unit = nirfmxlte.ModAccEvmUnit.PERCENTAGE

    averaging_enabled = nirfmxlte.ModAccAveragingEnabled.FALSE
    averaging_count = 10

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

        lte_signal.component_carrier.configure_spacing(
            "", component_carrier_spacing_type, component_carrier_at_center_frequency
        )

        lte_signal.configure_band("", band)

        lte_signal.configure_duplex_scheme("", duplex_scheme, uplink_downlink_configuration)

        lte_signal.configure_number_of_component_carriers("", number_of_component_carriers)

        for i in range(number_of_component_carriers):
            carrier_string = nirfmxlte.Lte.build_carrier_string("", i)
            lte_signal.component_carrier.configure(
                carrier_string,
                component_carrier_bandwidth[i],
                component_carrier_frequency[i],
                component_carrier_cell_id[i],
            )
            lte_signal.component_carrier.set_laa_starting_subframe(
                carrier_string, laa_starting_subframe[i]
            )
            lte_signal.component_carrier.set_laa_number_of_subframes(
                carrier_string, laa_number_of_subframes[i]
            )
            lte_signal.component_carrier.set_laa_uplink_start_position(
                carrier_string, laa_uplink_start_position[i]
            )
            lte_signal.component_carrier.set_laa_uplink_ending_symbol(
                carrier_string, laa_uplink_ending_symbol[i]
            )

        lte_signal.configure_auto_dmrs_detection_enabled("", auto_dmrs_detection_enabled)

        lte_signal.select_measurements("", nirfmxlte.MeasurementTypes.MODACC, True)

        lte_signal.modacc.configuration.configure_synchronization_mode_and_interval(
            "", synchronization_mode, measurement_offset, measurement_length
        )

        lte_signal.modacc.configuration.configure_evm_unit("", evm_unit)

        lte_signal.modacc.configuration.configure_averaging("", averaging_enabled, averaging_count)

        lte_signal.initiate("", "")

        (
            mean_rms_composite_evm,
            maximum_peak_composite_evm,
            mean_frequency_error,
            peak_composite_evm_symbol_index,
            peak_composite_evm_subcarrier_index,
            peak_composite_evm_slot_index,
            error_code,
        ) = lte_signal.modacc.results.fetch_composite_evm_array("", timeout)

        (
            mean_iq_origin_offset,
            mean_iq_gain_imbalance,
            mean_iq_quadrature_error,
            error_code,
        ) = lte_signal.modacc.results.fetch_iq_impairments_array("", timeout)

        in_band_emission_margin, error_code = (
            lte_signal.modacc.results.fetch_in_band_emission_margin_array("", timeout)
        )

        for i in range(number_of_component_carriers):
            carrier_string = nirfmxlte.Lte.build_carrier_string("", i)
            data_constellation = numpy.empty(0, dtype=numpy.complex64)
            dmrs_data_constellation = numpy.empty(0, dtype=numpy.complex64)
            error_code = lte_signal.modacc.results.fetch_pusch_constellation_trace(
                carrier_string, timeout, data_constellation, dmrs_data_constellation
            )
            evm_per_subcarrier = numpy.empty(0, dtype=numpy.float32)
            x0, dx, error_code = lte_signal.modacc.results.fetch_evm_per_subcarrier_trace(
                carrier_string, timeout, evm_per_subcarrier
            )

        print("------------------------Measurements------------------------\n")
        for i in range(number_of_component_carriers):
            print(f"Carrier  : {i}\n")
            print(f"Mean RMS Composite EVM  (% or dB)    : {mean_rms_composite_evm[i]}")
            print(f"Maximum Peak Composite EVM  (% or dB): {maximum_peak_composite_evm[i]}")
            print(f"Peak Composite EVM Slot Index        : {peak_composite_evm_slot_index[i]}")
            print(f"Peak Composite EVM Symbol Index      : {peak_composite_evm_symbol_index[i]}")
            print(f"Peak Composite EVM Subcarrier Index  : {peak_composite_evm_subcarrier_index[i]}")
            print(f"Mean Frequency Error  (Hz)           : {mean_frequency_error[i]}")
            print(f"Mean IQ Origin Offset  (dBc)         : {mean_iq_origin_offset[i]}")
            print(f"Mean IQ Gain Imbalance  (dB)         : {mean_iq_gain_imbalance[i]}")
            print(f"Mean IQ Quadrature Error  (deg)      : {mean_iq_quadrature_error[i]}")
            print(f"In Band Emission Margin  (dB)        : {in_band_emission_margin[i]}")
            print("-------------------------------------------------\n")

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
        description="Pass arguments for LTE UL LAA ModAcc Multi-Carrier Example",
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
