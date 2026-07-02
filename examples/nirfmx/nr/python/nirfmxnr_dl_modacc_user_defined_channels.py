r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure Selected Ports.
4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure Trigger Type and Trigger Parameters.
6. Configure Link Direction as Downlink, Frequency Range, Carrier Bandwidth, Cell ID, BWP Subcarrier Spacing,
   Auto RB Detection Enabled, DL Channel Configuration Mode and Auto Increment Cell ID Enabled.
7. Configure PDSCH and PDSCH RB Allocation.
8. Configure PDSCH DMRS.
9. Configure SSB.
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
    cell_id = 0
    carrier_bandwidth = 100e6  # Hz
    subcarrier_spacing = 30e3  # Hz
    auto_resource_block_detection_enabled = nirfmxnr.AutoResourceBlockDetectionEnabled.TRUE
    auto_increment_cell_id_enabled = nirfmxnr.AutoIncrementCellIDEnabled.TRUE

    pdsch_modulation_type = nirfmxnr.PdschModulationType.QPSK
    pdsch_resource_block_offset = [0]
    pdsch_number_of_resource_blocks = [-1]
    pdsch_slot_allocation = "0-Last"
    pdsch_symbol_allocation = "0-Last"

    pdsch_dmrs_power_mode = nirfmxnr.PdschDmrsPowerMode.CDM_GROUPS
    pdsch_dmrs_power = 0.0  # dB
    pdsch_dmrs_configuration_type = nirfmxnr.PdschDmrsConfigurationType.TYPE1
    pdsch_mapping_type = nirfmxnr.PdschMappingType.TYPE_A
    pdsch_dmrs_type_a_position = 2
    pdsch_dmrs_duration = nirfmxnr.PdschDmrsDuration.SINGLE_SYMBOL
    pdsch_dmrs_additional_positions = 0

    ssb_enabled = nirfmxnr.SsbEnabled.FALSE
    ssb_crb_offset = 0
    ssb_subcarrier_offset = 0
    ssb_pattern = nirfmxnr.SsbPattern.CASE_B_3GHZ_TO_6GHZ

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

        nr.set_link_direction("", nirfmxnr.LinkDirection.DOWNLINK)
        nr.set_frequency_range("", frequency_range)
        nr.component_carrier.set_bandwidth("", carrier_bandwidth)
        nr.component_carrier.set_cell_id("", cell_id)
        nr.component_carrier.set_bandwidth_part_subcarrier_spacing("", subcarrier_spacing)
        nr.set_auto_resource_block_detection_enabled("", auto_resource_block_detection_enabled)
        nr.set_downlink_channel_configuration_mode("", nirfmxnr.DownlinkChannelConfigurationMode.USER_DEFINED)
        nr.set_auto_increment_cell_id_enabled("", auto_increment_cell_id_enabled)

        nr.component_carrier.set_pdsch_modulation_type("", pdsch_modulation_type)
        nr.component_carrier.set_pdsch_slot_allocation("", pdsch_slot_allocation)
        nr.component_carrier.set_pdsch_symbol_allocation("", pdsch_symbol_allocation)

        nr.component_carrier.set_pdsch_number_of_resource_block_clusters("", NUMBER_OF_RESOURCE_BLOCK_CLUSTERS)

        subblock_string = nirfmxnr.NR.build_subblock_string("", 0)
        carrier_string = nirfmxnr.NR.build_carrier_string(subblock_string, 0)
        bandwidth_part_string = nirfmxnr.NR.build_bandwidth_part_string(carrier_string, 0)
        user_string = nirfmxnr.NR.build_user_string(bandwidth_part_string, 0)
        pdsch_string = nirfmxnr.NR.build_pdsch_string(user_string, 0)
        for i in range(NUMBER_OF_RESOURCE_BLOCK_CLUSTERS):
            pdsch_cluster_string = nirfmxnr.NR.build_pdsch_cluster_string(pdsch_string, i)
            nr.component_carrier.set_pdsch_resource_block_offset(pdsch_cluster_string, pdsch_resource_block_offset[i])
            nr.component_carrier.set_pdsch_number_of_resource_blocks(pdsch_cluster_string, pdsch_number_of_resource_blocks[i])

        nr.component_carrier.set_pdsch_dmrs_power_mode("", pdsch_dmrs_power_mode)
        nr.component_carrier.set_pdsch_dmrs_power("", pdsch_dmrs_power)
        nr.component_carrier.set_pdsch_dmrs_configuration_type("", pdsch_dmrs_configuration_type)
        nr.component_carrier.set_pdsch_mapping_type("", pdsch_mapping_type)
        nr.component_carrier.set_pdsch_dmrs_type_a_position("", pdsch_dmrs_type_a_position)
        nr.component_carrier.set_pdsch_dmrs_duration("", pdsch_dmrs_duration)
        nr.component_carrier.set_pdsch_dmrs_additional_positions("", pdsch_dmrs_additional_positions)

        nr.component_carrier.set_ssb_enabled("", ssb_enabled)
        nr.component_carrier.set_ssb_crb_offset("", ssb_crb_offset)
        nr.component_carrier.set_ssb_subcarrier_offset("", ssb_subcarrier_offset)
        nr.component_carrier.set_ssb_pattern("", ssb_pattern)

        nr.select_measurements("", nirfmxnr.MeasurementTypes.MODACC, True)
        nr.modacc.configuration.set_synchronization_mode("", synchronization_mode)
        nr.modacc.configuration.set_averaging_enabled("", averaging_enabled)
        nr.modacc.configuration.set_averaging_count("", averaging_count)
        nr.modacc.configuration.set_measurement_length_unit("", measurement_length_unit)
        nr.modacc.configuration.set_measurement_offset("", measurement_offset)
        nr.modacc.configuration.set_measurement_length("", measurement_length)

        nr.initiate("", "")

        # Retrieve results
        composite_rms_evm_mean, error_code = nr.modacc.results.get_composite_rms_evm_mean("")
        composite_peak_evm_maximum, error_code = nr.modacc.results.get_composite_peak_evm_maximum("")
        composite_peak_evm_slot_index, error_code = nr.modacc.results.get_composite_peak_evm_slot_index("")
        composite_peak_evm_symbol_index, error_code = nr.modacc.results.get_composite_peak_evm_symbol_index("")
        composite_peak_evm_subcarrier_index, error_code = nr.modacc.results.get_composite_peak_evm_subcarrier_index("")
        component_carrier_frequency_error_mean, error_code = nr.modacc.results.get_component_carrier_frequency_error_mean("")
        component_carrier_iq_origin_offset_mean, error_code = nr.modacc.results.get_component_carrier_iq_origin_offset_mean("")
        component_carrier_iq_gain_imbalance_mean, error_code = nr.modacc.results.get_component_carrier_iq_gain_imbalance_mean("")
        component_carrier_quadrature_error_mean, error_code = nr.modacc.results.get_component_carrier_quadrature_error_mean("")

        pdsch_qpsk_rms_evm_mean, error_code = nr.modacc.results.get_pdsch_qpsk_rms_evm_mean("")
        pdsch_16qam_rms_evm_mean, error_code = nr.modacc.results.get_pdsch_16qam_rms_evm_mean("")
        pdsch_64qam_rms_evm_mean, error_code = nr.modacc.results.get_pdsch_64qam_rms_evm_mean("")
        pdsch_256qam_rms_evm_mean, error_code = nr.modacc.results.get_pdsch_256qam_rms_evm_mean("")

        qpsk_constellation = numpy.empty(0, dtype=numpy.complex64)
        nr.modacc.results.fetch_pdsch_qpsk_constellation_trace("", timeout, qpsk_constellation)
        qam16_constellation = numpy.empty(0, dtype=numpy.complex64)
        nr.modacc.results.fetch_pdsch16_qam_constellation_trace("", timeout, qam16_constellation)
        qam64_constellation = numpy.empty(0, dtype=numpy.complex64)
        nr.modacc.results.fetch_pdsch64_qam_constellation_trace("", timeout, qam64_constellation)
        qam256_constellation = numpy.empty(0, dtype=numpy.complex64)
        nr.modacc.results.fetch_pdsch256_qam_constellation_trace("", timeout, qam256_constellation)

        rms_evm_per_subcarrier_mean = numpy.empty(0, dtype=numpy.float32)
        nr.modacc.results.fetch_rms_evm_per_subcarrier_mean_trace("", timeout, rms_evm_per_subcarrier_mean)
        rms_evm_per_symbol_mean = numpy.empty(0, dtype=numpy.float32)
        nr.modacc.results.fetch_rms_evm_per_symbol_mean_trace("", timeout, rms_evm_per_symbol_mean)

        # Print Results
        print("------------------Measurement------------------\n")
        print(f"Composite RMS EVM Mean (%)                     : {composite_rms_evm_mean}")
        print(f"Composite Peak EVM Maximum (%)                 : {composite_peak_evm_maximum}")
        print(f"Composite Peak EVM Slot Index                  : {composite_peak_evm_slot_index}")
        print(f"Composite Peak EVM Symbol Index                : {composite_peak_evm_symbol_index}")
        print(f"Composite Peak EVM Subcarrier Index            : {composite_peak_evm_subcarrier_index}")
        print(f"PDSCH QPSK RMS EVM Mean (%)                    : {pdsch_qpsk_rms_evm_mean}")
        print(f"PDSCH 16QAM RMS EVM Mean (%)                   : {pdsch_16qam_rms_evm_mean}")
        print(f"PDSCH 64QAM RMS EVM Mean (%)                   : {pdsch_64qam_rms_evm_mean}")
        print(f"PDSCH 256QAM RMS EVM Mean (%)                  : {pdsch_256qam_rms_evm_mean}")
        print(f"Component Carrier Frequency Error Mean (Hz)    : {component_carrier_frequency_error_mean}")
        print(f"Component Carrier IQ Origin Offset Mean (dBc)  : {component_carrier_iq_origin_offset_mean}")
        print(f"Component Carrier IQ Gain Imbalance Mean (dB)  : {component_carrier_iq_gain_imbalance_mean}")
        print(f"Component Carrier Quadrature Error Mean (deg)  : {component_carrier_quadrature_error_mean}\n")

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
        description="Pass arguments for DL ModAcc User Defined Channels Example",
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
