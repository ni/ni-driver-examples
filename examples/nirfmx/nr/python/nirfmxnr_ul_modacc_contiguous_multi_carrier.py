r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure Selected Ports.
4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure Trigger Type and Trigger Parameters.
6. Configure Frequency Range, Band, Channel Raster, Component Carrier Spacing, CC at Center Frequency,
   Auto RB Detection Enabled and Auto Increment Cell ID Enabled.
7. Configure Carriers.
8. Configure PUSCH and PUSCH RB Allocation.
9. Configure PUSCH DMRS.
10. Select ModAcc measurement and enable Traces.
11. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
12. Configure Measurement Interval.
13. Initiate the Measurement.
14. Fetch ModAcc Measurements and Traces.
15. Close RFmx Session.
"""

import argparse
import sys

import nirfmxnr
import numpy

import nirfmxinstr

NUMBER_OF_COMPONENT_CARRIERS = 2
NUMBER_OF_RESOURCE_BLOCK_CLUSTERS = 1


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 3.5e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10.0e6  # Hz

    enable_trigger = False
    digital_edge_source = "PXI_Trig0"
    digital_edge = nirfmxnr.DigitalEdgeTriggerEdge.RISING_EDGE
    trigger_delay = 0.0  # s

    frequency_range = nirfmxnr.FrequencyRange.RANGE1
    band = 78
    subcarrier_spacing = 30e3  # Hz
    auto_resource_block_detection_enabled = nirfmxnr.AutoResourceBlockDetectionEnabled.TRUE
    auto_increment_cell_id_enabled = nirfmxnr.AutoIncrementCellIDEnabled.TRUE

    component_carrier_spacing_type = nirfmxnr.ComponentCarrierSpacingType.NOMINAL
    channel_raster = 15e3  # Hz
    component_carrier_at_center_frequency = -1

    component_carrier_bandwidth = [100e6, 100e6]  # Hz
    component_carrier_frequency = [-49.98e6, 50.01e6]  # Hz
    cell_id = [0, 1]

    pusch_transform_precoding_enabled = nirfmxnr.PuschTransformPrecodingEnabled.FALSE
    pusch_modulation_type = nirfmxnr.PuschModulationType.QPSK
    pusch_resource_block_offset = [0]
    pusch_number_of_resource_blocks = [-1]
    pusch_slot_allocation = "0-Last"
    pusch_symbol_allocation = "0-Last"

    pusch_dmrs_power_mode = nirfmxnr.PuschDmrsPowerMode.CDM_GROUPS
    pusch_dmrs_power = 0.0  # dB
    pusch_dmrs_configuration_type = nirfmxnr.PuschDmrsConfigurationType.TYPE1
    pusch_mapping_type = nirfmxnr.PuschMappingType.TYPE_A
    pusch_dmrs_type_a_position = 2
    pusch_dmrs_duration = nirfmxnr.PuschDmrsDuration.SINGLE_SYMBOL
    pusch_dmrs_additional_positions = 0

    synchronization_mode = nirfmxnr.ModAccSynchronizationMode.SLOT
    measurement_length_unit = nirfmxnr.ModAccMeasurementLengthUnit.SLOT
    measurement_offset = 0.0
    measurement_length = 1

    averaging_enabled = nirfmxnr.ModAccAveragingEnabled.FALSE
    averaging_count = 10

    timeout = 10.0  # s

    instr_session = None
    nr = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        nr = instr_session.get_nr_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)
        nr.set_selected_ports("", selected_ports)
        nr.configure_rf("", center_frequency, reference_level, external_attenuation)
        nr.configure_digital_edge_trigger("", digital_edge_source, digital_edge, trigger_delay, enable_trigger)

        nr.set_frequency_range("", frequency_range)
        nr.set_band("", band)
        nr.set_channel_raster("", channel_raster)
        nr.set_component_carrier_spacing_type("", component_carrier_spacing_type)
        nr.set_component_carrier_at_center_frequency("", component_carrier_at_center_frequency)
        nr.set_auto_resource_block_detection_enabled("", auto_resource_block_detection_enabled)
        nr.set_auto_increment_cell_id_enabled("", auto_increment_cell_id_enabled)

        nr.component_carrier.set_number_of_component_carriers("", NUMBER_OF_COMPONENT_CARRIERS)

        subblock_string = nirfmxnr.NR.build_subblock_string("", 0)
        for i in range(NUMBER_OF_COMPONENT_CARRIERS):
            carrier_string = nirfmxnr.NR.build_carrier_string(subblock_string, i)
            nr.component_carrier.set_bandwidth(carrier_string, component_carrier_bandwidth[i])
            nr.component_carrier.set_cell_id(carrier_string, cell_id[i])
            nr.component_carrier.set_frequency(carrier_string, component_carrier_frequency[i])

        nr.component_carrier.set_pusch_transform_precoding_enabled("carrier::all", pusch_transform_precoding_enabled)
        nr.component_carrier.set_pusch_modulation_type("carrier::all", pusch_modulation_type)
        nr.component_carrier.set_pusch_slot_allocation("carrier::all", pusch_slot_allocation)
        nr.component_carrier.set_pusch_symbol_allocation("carrier::all", pusch_symbol_allocation)

        nr.component_carrier.set_bandwidth_part_subcarrier_spacing("carrier::all", subcarrier_spacing)
        nr.component_carrier.set_pusch_number_of_resource_block_clusters("carrier::all", NUMBER_OF_RESOURCE_BLOCK_CLUSTERS)

        for i in range(NUMBER_OF_RESOURCE_BLOCK_CLUSTERS):
            pusch_cluster_string = nirfmxnr.NR.build_pusch_cluster_string("carrier::all", i)
            nr.component_carrier.set_pusch_resource_block_offset(pusch_cluster_string, pusch_resource_block_offset[i])
            nr.component_carrier.set_pusch_number_of_resource_blocks(pusch_cluster_string, pusch_number_of_resource_blocks[i])

        nr.component_carrier.set_pusch_dmrs_power_mode("carrier::all", pusch_dmrs_power_mode)
        nr.component_carrier.set_pusch_dmrs_power("carrier::all", pusch_dmrs_power)
        nr.component_carrier.set_pusch_dmrs_configuration_type("carrier::all", pusch_dmrs_configuration_type)
        nr.component_carrier.set_pusch_mapping_type("carrier::all", pusch_mapping_type)
        nr.component_carrier.set_pusch_dmrs_type_a_position("carrier::all", pusch_dmrs_type_a_position)
        nr.component_carrier.set_pusch_dmrs_duration("carrier::all", pusch_dmrs_duration)
        nr.component_carrier.set_pusch_dmrs_additional_positions("carrier::all", pusch_dmrs_additional_positions)

        nr.select_measurements("", nirfmxnr.MeasurementTypes.MODACC, True)
        nr.modacc.configuration.set_synchronization_mode("", synchronization_mode)
        nr.modacc.configuration.set_averaging_enabled("", averaging_enabled)
        nr.modacc.configuration.set_averaging_count("", averaging_count)
        nr.modacc.configuration.set_measurement_length_unit("", measurement_length_unit)
        nr.modacc.configuration.set_measurement_offset("", measurement_offset)
        nr.modacc.configuration.set_measurement_length("", measurement_length)
        nr.initiate("", "")

        # Retrieve results per carrier
        print("------------------------Measurements------------------------\n")
        for i in range(NUMBER_OF_COMPONENT_CARRIERS):
            carrier_string = nirfmxnr.NR.build_carrier_string(subblock_string, i)

            composite_rms_evm_mean, error_code = nr.modacc.results.get_composite_rms_evm_mean(carrier_string)
            composite_peak_evm_maximum, error_code = nr.modacc.results.get_composite_peak_evm_maximum(carrier_string)
            composite_peak_evm_slot_index, error_code = nr.modacc.results.get_composite_peak_evm_slot_index(carrier_string)
            composite_peak_evm_symbol_index, error_code = nr.modacc.results.get_composite_peak_evm_symbol_index(carrier_string)
            composite_peak_evm_subcarrier_index, error_code = nr.modacc.results.get_composite_peak_evm_subcarrier_index(carrier_string)
            component_carrier_frequency_error_mean, error_code = nr.modacc.results.get_component_carrier_frequency_error_mean(carrier_string)
            component_carrier_iq_origin_offset_mean, error_code = nr.modacc.results.get_component_carrier_iq_origin_offset_mean(carrier_string)
            component_carrier_iq_gain_imbalance_mean, error_code = nr.modacc.results.get_component_carrier_iq_gain_imbalance_mean(carrier_string)
            component_carrier_quadrature_error_mean, error_code = nr.modacc.results.get_component_carrier_quadrature_error_mean(carrier_string)
            in_band_emission_margin, error_code = nr.modacc.results.get_in_band_emission_margin(carrier_string)

            rms_evm_per_subcarrier_mean = numpy.empty(0, dtype=numpy.float32)
            nr.modacc.results.fetch_rms_evm_per_subcarrier_mean_trace(carrier_string, timeout, rms_evm_per_subcarrier_mean)

            rms_evm_per_symbol_mean = numpy.empty(0, dtype=numpy.float32)
            nr.modacc.results.fetch_rms_evm_per_symbol_mean_trace(carrier_string, timeout, rms_evm_per_symbol_mean)

            print(f"Carrier  : {i}")
            print(f"Composite RMS EVM Mean (%)                     : {composite_rms_evm_mean}")
            print(f"Composite Peak EVM Maximum (%)                 : {composite_peak_evm_maximum}")
            print(f"Composite Peak EVM Slot Index                  : {composite_peak_evm_slot_index}")
            print(f"Composite Peak EVM Symbol Index                : {composite_peak_evm_symbol_index}")
            print(f"Composite Peak EVM Subcarrier Index            : {composite_peak_evm_subcarrier_index}")
            print(f"Component Carrier Frequency Error Mean (Hz)    : {component_carrier_frequency_error_mean}")
            print(f"Component Carrier IQ Origin Offset Mean (dBc)  : {component_carrier_iq_origin_offset_mean}")
            print(f"Component Carrier IQ Gain Imbalance Mean (dB)  : {component_carrier_iq_gain_imbalance_mean}")
            print(f"Component Carrier Quadrature Error Mean (deg)  : {component_carrier_quadrature_error_mean}")
            print(f"In-Band Emission Margin (dB)                   : {in_band_emission_margin}")
            print("-----------------------------------------------------------------\n")

        for i in range(NUMBER_OF_COMPONENT_CARRIERS):
            carrier_string = nirfmxnr.NR.build_carrier_string("", i)
            pusch_data_constellation = numpy.empty(0, dtype=numpy.complex64)
            nr.modacc.results.fetch_pusch_data_constellation_trace(carrier_string, timeout, pusch_data_constellation)

            pusch_dmrs_constellation = numpy.empty(0, dtype=numpy.complex64)
            nr.modacc.results.fetch_pusch_dmrs_constellation_trace(carrier_string, timeout, pusch_dmrs_constellation)

        spectral_flatness = numpy.empty(0, dtype=numpy.float32)
        spectral_flatness_lower_mask = numpy.empty(0, dtype=numpy.float32)
        spectral_flatness_upper_mask = numpy.empty(0, dtype=numpy.float32)
        nr.modacc.results.fetch_spectral_flatness_trace(
            "", timeout, spectral_flatness, spectral_flatness_lower_mask, spectral_flatness_upper_mask
        )

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
        description="Pass arguments for UL ModAcc Contiguous Multi-Carrier Example",
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
