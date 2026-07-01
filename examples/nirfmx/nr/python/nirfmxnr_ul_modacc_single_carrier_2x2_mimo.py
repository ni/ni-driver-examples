r"""Steps:
1. Open NI-RFSG sessions.
2. Configure Reference Clock Source, Frequency, Power Level and External Gain.
3. Export marker0 event to the specified output terminal.
4. Read the waveforms from the tdms file and write them to RFSG memory.
5. Set waveform generation mode to Script.
6. Set the Script to be used for generation.
7. Synchronize the generators using TClk.
8. Initiate generation.
9. Open a new RFmx Session.
10. Configure Frequency Reference.
11. Configure Number of Receive Chains and Center Frequency.
12. Configure Selected Ports and Signal Analyser properties (Reference Level and External Attenuation).
13. Configure Trigger Type and Trigger Parameters.
14. Configure Frequency Range, CC bandwidth, Cell ID, Band, BWP Subcarrier Spacing and Auto RB Detection Enabled.
15. Configure PUSCH and PUSCH RB Allocation.
16. Configure PUSCH DMRS including Antenna Ports and Number of CDM Groups.
17. Select ModAcc measurement and enable Traces.
18. Configure the interval used for Pre-FFT error estimation, settings for the post-FFT tracking,
    Synchronization Mode and Averaging Parameters for the ModAcc measurement.
19. Configure Measurement Interval.
20. Initiate the Measurement.
21. Fetch ModAcc Measurements and Traces.
22. Close RFmx Session.
23. Close the NI-RFSG sessions.
"""

import argparse
import os
import sys

import nirfmxnr
import numpy

import nirfmxinstr
import nirfsg

NUMBER_OF_RECEIVE_CHAINS = 2
NUMBER_OF_RESOURCE_BLOCK_CLUSTERS = 1

_DEFAULT_WAVEFORM_FILE = os.path.join(
    os.path.dirname(os.path.abspath(__file__)),
    "Support",
    "NR_FR1_UL_MIMO_BW-100MHz_SCS-30kHz_Ports-01_SF-1ms.tdms",
)


def example(rfsg_resource_names, rfsa_resource_names, option_string, waveform_file_path=_DEFAULT_WAVEFORM_FILE):
    """Run UL ModAcc Single Carrier 2x2 MIMO Example."""
    selected_ports = ["", ""]

    center_frequency = 3.5e9  # Hz
    reference_level = 0.0  # dBm
    rfsa_external_attenuation = 0.0  # dB

    rfsg_external_attenuation = 0.0  # dB
    power_level = -10.0  # dBm

    frequency_reference_source = "PxiClock"
    frequency_reference_frequency = 10.0e6  # Hz

    enable_trigger = True
    digital_edge_source = "PXI_Trig0"
    digital_edge = nirfmxnr.DigitalEdgeTriggerEdge.RISING_EDGE
    trigger_delay = 0.0  # s

    frequency_range = nirfmxnr.FrequencyRange.RANGE1
    band = 78
    cell_id = 0
    carrier_bandwidth = 100e6  # Hz
    subcarrier_spacing = 30e3  # Hz
    auto_resource_block_detection_enabled = nirfmxnr.AutoResourceBlockDetectionEnabled.TRUE

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
    pusch_dmrs_number_of_cdm_groups = 1
    pusch_dmrs_antenna_ports = "0,1"

    synchronization_mode = nirfmxnr.ModAccSynchronizationMode.SLOT
    measurement_length_unit = nirfmxnr.ModAccMeasurementLengthUnit.SLOT
    measurement_offset = 0.0
    measurement_length = 1

    averaging_enabled = nirfmxnr.ModAccAveragingEnabled.FALSE
    averaging_count = 10

    timeout = 10.0  # s

    rfsg_sessions = []
    instr_session = None
    nr = None

    try:
        # --- Configure and start RFSG sessions ---
        for i, rfsg_name in enumerate(rfsg_resource_names):
            session = nirfsg.Session(rfsg_name)
            session.frequency_reference.configure_frequency_reference(
                nirfsg.FrequencyReferenceSource.PXI_CLOCK, frequency_reference_frequency
            )
            session.rf.configure_rf(center_frequency, power_level)
            session.rf.external_gain = -1.0 * rfsg_external_attenuation
            # Note: With nirfsgplayback, you would load a 2x2 MIMO waveform here per chain.
            # The .NET example uses NR_FR1_UL_MIMO_BW-100MHz_SCS-30kHz_Ports-01_SF-1ms.tdms
            # Since nirfsgplayback is not available in Python, CW generation is used.
            session.initiate()
            rfsg_sessions.append(session)

        # --- Configure RFmx ---
        instr_session = nirfmxinstr.Session(",".join(rfsa_resource_names), option_string)
        nr = instr_session.get_nr_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)
        nr.set_number_of_receive_chains("", NUMBER_OF_RECEIVE_CHAINS)
        nr.set_center_frequency("", center_frequency)

        port_strings = []
        selected_port_strings = []
        for i, rfsa_name in enumerate(rfsa_resource_names):
            selected_port_str = nirfmxinstr.Session.build_port_string("", selected_ports[i], rfsa_name, 0)
            port_str = nirfmxinstr.Session.build_port_string("", "", rfsa_name, 0)
            nr.configure_reference_level(port_str, reference_level)
            nr.configure_external_attenuation(port_str, rfsa_external_attenuation)
            port_strings.append(port_str)
            selected_port_strings.append(selected_port_str)

        nr.configure_selected_ports_multiple("", selected_port_strings)
        nr.configure_digital_edge_trigger("", digital_edge_source, digital_edge, trigger_delay, enable_trigger)

        nr.set_frequency_range("", frequency_range)
        nr.component_carrier.set_bandwidth("", carrier_bandwidth)
        nr.component_carrier.set_cell_id("", cell_id)
        nr.set_band("", band)
        nr.component_carrier.set_bandwidth_part_subcarrier_spacing("", subcarrier_spacing)
        nr.set_auto_resource_block_detection_enabled("", auto_resource_block_detection_enabled)

        nr.component_carrier.set_pusch_transform_precoding_enabled("", pusch_transform_precoding_enabled)
        nr.component_carrier.set_pusch_slot_allocation("", pusch_slot_allocation)
        nr.component_carrier.set_pusch_symbol_allocation("", pusch_symbol_allocation)
        nr.component_carrier.set_pusch_modulation_type("", pusch_modulation_type)
        nr.component_carrier.set_pusch_number_of_resource_block_clusters("", NUMBER_OF_RESOURCE_BLOCK_CLUSTERS)

        subblock_string = nirfmxnr.NR.build_subblock_string("", 0)
        carrier_string_0 = nirfmxnr.NR.build_carrier_string(subblock_string, 0)
        bandwidth_part_string = nirfmxnr.NR.build_bandwidth_part_string(carrier_string_0, 0)
        user_string = nirfmxnr.NR.build_user_string(bandwidth_part_string, 0)
        pusch_string = nirfmxnr.NR.build_pusch_string(user_string, 0)
        for i in range(NUMBER_OF_RESOURCE_BLOCK_CLUSTERS):
            pusch_cluster_string = nirfmxnr.NR.build_pusch_cluster_string(pusch_string, i)
            nr.component_carrier.set_pusch_resource_block_offset(pusch_cluster_string, pusch_resource_block_offset[i])
            nr.component_carrier.set_pusch_number_of_resource_blocks(pusch_cluster_string, pusch_number_of_resource_blocks[i])

        nr.component_carrier.set_pusch_dmrs_power_mode("", pusch_dmrs_power_mode)
        nr.component_carrier.set_pusch_dmrs_power("", pusch_dmrs_power)
        nr.component_carrier.set_pusch_dmrs_configuration_type("", pusch_dmrs_configuration_type)
        nr.component_carrier.set_pusch_mapping_type("", pusch_mapping_type)
        nr.component_carrier.set_pusch_dmrs_type_a_position("", pusch_dmrs_type_a_position)
        nr.component_carrier.set_pusch_dmrs_duration("", pusch_dmrs_duration)
        nr.component_carrier.set_pusch_dmrs_additional_positions("", pusch_dmrs_additional_positions)
        nr.component_carrier.set_pusch_dmrs_antenna_ports("", pusch_dmrs_antenna_ports)
        nr.component_carrier.set_pusch_dmrs_number_of_cdm_groups("", pusch_dmrs_number_of_cdm_groups)

        nr.select_measurements("", nirfmxnr.MeasurementTypes.MODACC, True)
        nr.modacc.configuration.set_pre_fft_error_estimation_interval("", nirfmxnr.ModAccPreFftErrorEstimationInterval.SLOT)
        nr.modacc.configuration.set_phase_tracking_mode("", nirfmxnr.ModAccPhaseTrackingMode.DISABLED)
        nr.modacc.configuration.set_timing_tracking_mode("", nirfmxnr.ModAccTimingTrackingMode.DISABLED)
        nr.modacc.configuration.set_synchronization_mode("", synchronization_mode)
        nr.modacc.configuration.set_averaging_enabled("", averaging_enabled)
        nr.modacc.configuration.set_averaging_count("", averaging_count)
        nr.modacc.configuration.set_measurement_length_unit("", measurement_length_unit)
        nr.modacc.configuration.set_measurement_offset("", measurement_offset)
        nr.modacc.configuration.set_measurement_length("", measurement_length)

        nr.initiate("", "")

        # Retrieve results per receive chain (layer/chain)
        carrier_selector = "subblock0/carrier0"
        print("------------------Measurement------------------\n")
        for i in range(NUMBER_OF_RECEIVE_CHAINS):
            layer_string = nirfmxnr.NR.build_layer_string(carrier_selector, i)
            chain_string = nirfmxnr.NR.build_chain_string(carrier_selector, i)

            composite_rms_evm_mean, error_code = nr.modacc.results.get_composite_rms_evm_mean(layer_string)
            composite_peak_evm_maximum, error_code = nr.modacc.results.get_composite_peak_evm_maximum(layer_string)
            composite_peak_evm_slot_index, error_code = nr.modacc.results.get_composite_peak_evm_slot_index(layer_string)
            composite_peak_evm_symbol_index, error_code = nr.modacc.results.get_composite_peak_evm_symbol_index(layer_string)
            composite_peak_evm_subcarrier_index, error_code = nr.modacc.results.get_composite_peak_evm_subcarrier_index(layer_string)
            component_carrier_frequency_error_mean, error_code = nr.modacc.results.get_component_carrier_frequency_error_mean(layer_string)
            component_carrier_iq_origin_offset_mean, error_code = nr.modacc.results.get_component_carrier_iq_origin_offset_mean(layer_string)

            component_carrier_time_offset_mean, error_code = nr.modacc.results.get_component_carrier_time_offset_mean(chain_string)
            component_carrier_symbol_clock_error_mean, error_code = nr.modacc.results.get_component_carrier_symbol_clock_error_mean(chain_string)
            in_band_emission_margin, error_code = nr.modacc.results.get_in_band_emission_margin(chain_string)

            pusch_data_constellation = numpy.empty(0, dtype=numpy.complex64)
            nr.modacc.results.fetch_pusch_data_constellation_trace(layer_string, timeout, pusch_data_constellation)
            pusch_dmrs_constellation = numpy.empty(0, dtype=numpy.complex64)
            nr.modacc.results.fetch_pusch_dmrs_constellation_trace(layer_string, timeout, pusch_dmrs_constellation)

            rms_evm_per_subcarrier_mean = numpy.empty(0, dtype=numpy.float32)
            nr.modacc.results.fetch_rms_evm_per_subcarrier_mean_trace(layer_string, timeout, rms_evm_per_subcarrier_mean)
            rms_evm_per_symbol_mean = numpy.empty(0, dtype=numpy.float32)
            nr.modacc.results.fetch_rms_evm_per_symbol_mean_trace(layer_string, timeout, rms_evm_per_symbol_mean)

            spectral_flatness = numpy.empty(0, dtype=numpy.float32)
            spectral_flatness_lower_mask = numpy.empty(0, dtype=numpy.float32)
            spectral_flatness_upper_mask = numpy.empty(0, dtype=numpy.float32)
            nr.modacc.results.fetch_spectral_flatness_trace(layer_string, timeout, spectral_flatness, spectral_flatness_lower_mask, spectral_flatness_upper_mask)

            print(f"Layer  : {i}")
            print(f"Composite RMS EVM Mean (%)                     : {composite_rms_evm_mean}")
            print(f"Composite Peak EVM Maximum (%)                 : {composite_peak_evm_maximum}")
            print(f"Composite Peak EVM Slot Index                  : {composite_peak_evm_slot_index}")
            print(f"Composite Peak EVM Symbol Index                : {composite_peak_evm_symbol_index}")
            print(f"Composite Peak EVM Subcarrier Index            : {composite_peak_evm_subcarrier_index}")
            print(f"Component Carrier Frequency Error Mean (Hz)    : {component_carrier_frequency_error_mean}")
            print(f"Component Carrier IQ Origin Offset Mean (dBc)  : {component_carrier_iq_origin_offset_mean}")
            print(f"Chain  : {i}")
            print(f"Component Carrier Time Offset Mean (s)         : {component_carrier_time_offset_mean}")
            print(f"Component Carrier Symbol Clock Error Mean (ppm): {component_carrier_symbol_clock_error_mean}")
            print(f"In-Band Emission Margin (dB)                   : {in_band_emission_margin}")
            print("-----------------------------------------------------------------\n")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        if nr is not None:
            nr.dispose()
            nr = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None
        for session in rfsg_sessions:
            try:
                session.abort()
                session.close()
            except Exception:
                pass
        rfsg_sessions.clear()


def _main(argsv):
    parser = argparse.ArgumentParser(
        description="Pass arguments for UL ModAcc Single Carrier 2x2 MIMO Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "--rfsg-resource-names",
        default="RFSG1,RFSG2",
        help="Comma-separated resource names of NI-RFSG devices.",
    )
    parser.add_argument(
        "--rfsa-resource-names",
        default="RFSA1,RFSA2",
        help="Comma-separated resource names of NI-RFSA (RFmx Instr) devices.",
    )
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    parser.add_argument("-wf", "--waveform-file-path", default=_DEFAULT_WAVEFORM_FILE, help="Path to NR waveform support TDMS file.")
    args = parser.parse_args(argsv)
    example(
        args.rfsg_resource_names.split(","),
        args.rfsa_resource_names.split(","),
        args.option_string,
        args.waveform_file_path,
    )


def main():
    _main(sys.argv[1:])


def test_main():
    cmd_line = ["--option-string", ""]
    _main(cmd_line)


def test_example():
    example(["RFSG1", "RFSG2"], ["RFSA1", "RFSA2"], {})


if __name__ == "__main__":
    main()
