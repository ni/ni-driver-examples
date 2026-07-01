"""
RFmx LTE NB-IoT ModAcc (UL) Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Carrier Bandwidth.
6. Configure NB-IoT Component Carrier.
7. Configure NPUSCH Format.
8. Configure Auto NPUSCH Channel Detection Enabled.
9. Configure NPUSCH Starting Slot.
10. Configure NPUSCH DMRS.
11. Select ModAcc measurement and enable Traces.
12. Configure Measurement Interval.
13. Configure EVM Unit.
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
    """LTE NB-IoT uplink ModAcc measurement example."""
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

    n_pusch_format = 1
    auto_npusch_channel_detection_enabled = (
        nirfmxlte.AutoNPuschChannelDetectionEnabled.TRUE
    )
    n_pusch_starting_slot = 0
    n_pusch_dmrs_base_sequence_mode = nirfmxlte.NPuschDmrsBaseSequenceMode.AUTO
    n_pusch_dmrs_base_sequence_index = 0
    n_pusch_dmrs_cyclic_shift = 0
    n_pusch_dmrs_group_hopping_enabled = nirfmxlte.NPuschDmrsGroupHoppingEnabled.FALSE
    n_pusch_dmrs_delta_ss = 0

    evm_unit = nirfmxlte.ModAccEvmUnit.PERCENTAGE

    measurement_offset = 0  # slots
    measurement_length = 1  # slots

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

        lte_signal.component_carrier.configure_npusch_format("", n_pusch_format)

        lte_signal.component_carrier.configure_auto_npusch_channel_detection_enabled(
            "", auto_npusch_channel_detection_enabled
        )

        lte_signal.component_carrier.configure_npusch_starting_slot("", n_pusch_starting_slot)

        lte_signal.component_carrier.configure_npusch_dmrs(
            "",
            n_pusch_dmrs_base_sequence_mode,
            n_pusch_dmrs_base_sequence_index,
            n_pusch_dmrs_cyclic_shift,
            n_pusch_dmrs_group_hopping_enabled,
            n_pusch_dmrs_delta_ss,
        )

        lte_signal.select_measurements("", nirfmxlte.MeasurementTypes.MODACC, True)

        lte_signal.modacc.configuration.configure_synchronization_mode_and_interval(
            "",
            nirfmxlte.ModAccSynchronizationMode.SLOT,
            measurement_offset,
            measurement_length,
        )

        lte_signal.modacc.configuration.configure_evm_unit("", evm_unit)

        lte_signal.initiate("", "")

        (
            mean_rms_composite_evm,
            max_peak_composite_evm,
            mean_frequency_error,
            peak_composite_evm_symbol_index,
            peak_composite_evm_subcarrier_index,
            peak_composite_evm_slot_index,
            error_code,
        ) = lte_signal.modacc.results.fetch_composite_evm("", timeout)

        mean_iq_origin_offset, mean_iq_gain_imbalance, mean_iq_quadrature_error, error_code = (
            lte_signal.modacc.results.fetch_iq_impairments("", timeout)
        )

        in_band_emission_margin, error_code = (
            lte_signal.modacc.results.fetch_in_band_emission_margin("", timeout)
        )

        npusch_data_constellation = numpy.empty(0, dtype=numpy.complex64)
        npusch_dmrs_constellation = numpy.empty(0, dtype=numpy.complex64)
        error_code = lte_signal.modacc.results.fetch_npusch_constellation_trace(
            "", timeout, npusch_data_constellation, npusch_dmrs_constellation
        )

        rms_evm_per_symbol = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = lte_signal.modacc.results.fetch_evm_per_symbol_trace(
            "", timeout, rms_evm_per_symbol
        )

        # Print results
        print("------------------Measurements------------------")
        print(f"Mean RMS Composite EVM  (% or dB)       : {mean_rms_composite_evm}")
        print(f"Max Peak Composite EVM  (% or dB)       : {max_peak_composite_evm}")
        print(f"Peak Composite EVM Slot Index           : {peak_composite_evm_slot_index}")
        print(f"Peak Composite EVM Symbol Index         : {peak_composite_evm_symbol_index}")
        print(f"Peak Composite EVM Subcarrier Index     : {peak_composite_evm_subcarrier_index}")
        print(f"Mean Frequency Error  (Hz)              : {mean_frequency_error}")
        print(f"Mean IQ Origin Offset  (dBc)            : {mean_iq_origin_offset}")
        print(f"Mean IQ Gain Imbalance  (dB)            : {mean_iq_gain_imbalance}")
        print(f"Mean IQ Quadrature Error  (deg)         : {mean_iq_quadrature_error}")
        print(f"In-Band Emission Margin  (dB)           : {in_band_emission_margin}")

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
        description="RFmx LTE NB-IoT ModAcc (UL) Example",
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
