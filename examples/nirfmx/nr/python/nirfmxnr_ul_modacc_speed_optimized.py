r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure LO Source to Automatic SG SA Shared.
4. Configure Selected Ports.
5. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
6. Configure Trigger Type and Trigger Parameters.
7. Configure Frequency Range, CC bandwidth, Cell ID, Band, BWP Subcarrier Spacing and Auto RB Detection Enabled.
   Setting Auto RB Detection Enabled to False reduces the measurement time.
8. Configure PUSCH and PUSCH RB Allocation.
9. Configure PUSCH DMRS.
10. Select ModAcc measurement and disable Traces.
11. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
12. Configure Measurement Interval.
13. Set ModAcc Magnitude and Phase Error Enabled and IQ Mismatch Estimation Enabled to False.
    This disables computation of the corresponding results. Configure ModAcc Frequency Error Estimation,
    Symbol Clock Error Estimation Enabled, Phase Tracking Mode, Timing Tracking Mode, and IQ Origin Offset
    Estimation Enabled. Set these attributes to False/Disabled to reduce the measurement time.
14. Configure EVM Reference Data Symbol Mode. If the mode is set to ReferenceWaveform, read the
    reference waveform from a .tdms file using nptdms and configure it on the ModAcc measurement.
15. Initiate the Measurement.
16. Fetch ModAcc Measurements.
17. Close RFmx Session.
"""

import argparse
import sys

import numpy

import nirfmxinstr
import nirfmxnr


def _read_reference_waveform_from_tdms(file_path):
    """Read a complex IQ waveform from a TDMS file for use as a ModAcc reference waveform."""
    try:
        from nptdms import TdmsFile
    except ImportError as e:
        raise ImportError(
            "nptdms package is required to use ReferenceWaveform mode. "
            "Install it with: pip install nptdms"
        ) from e

    tdms_file = TdmsFile.read(file_path)
    waveforms_group = tdms_file["waveforms"]
    channel = waveforms_group.channels()[0]
    raw = channel[:].astype(numpy.float64)
    i_data = raw[0::2].astype(numpy.float32)
    q_data = raw[1::2].astype(numpy.float32)
    iq = (i_data + 1j * q_data).astype(numpy.complex64)
    props = channel.properties
    x0 = float(props.get("t0", 0.0))
    dx = float(props.get("dt", 1.0 / 20e6))
    return x0, dx, iq

NUMBER_OF_RESOURCE_BLOCK_CLUSTERS = 1


def example(resource_name, option_string, waveform_file_name=""):
    """Run Example."""
    selected_ports = ""
    center_frequency = 3.5e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    frequency_reference_source = "OnboardClock"
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
    auto_resource_block_detection_enabled = nirfmxnr.AutoResourceBlockDetectionEnabled.FALSE

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

    synchronization_mode = nirfmxnr.ModAccSynchronizationMode.FRAME
    measurement_length_unit = nirfmxnr.ModAccMeasurementLengthUnit.SLOT
    measurement_offset = 0.0
    measurement_length = 1

    averaging_enabled = nirfmxnr.ModAccAveragingEnabled.FALSE
    averaging_count = 10

    frequency_error_estimation = nirfmxnr.ModAccFrequencyErrorEstimation.DISABLED
    symbol_clock_error_estimation_enabled = nirfmxnr.ModAccSymbolClockErrorEstimationEnabled.FALSE
    phase_tracking_mode = nirfmxnr.ModAccPhaseTrackingMode.DISABLED
    timing_tracking_mode = nirfmxnr.ModAccTimingTrackingMode.DISABLED
    iq_origin_offset_estimation_enabled = nirfmxnr.ModAccIQOriginOffsetEstimationEnabled.FALSE

    evm_reference_data_symbols_mode = nirfmxnr.ModAccEvmReferenceDataSymbolsMode.ACQUIRED_WAVEFORM

    instr_session = None
    nr = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        nr = instr_session.get_nr_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)
        instr_session.set_lo_source("", "Automatic_SG_SA_Shared")
        nr.set_selected_ports("", selected_ports)
        nr.configure_rf("", center_frequency, reference_level, external_attenuation)
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
        carrier_string = nirfmxnr.NR.build_carrier_string(subblock_string, 0)
        bandwidth_part_string = nirfmxnr.NR.build_bandwidth_part_string(carrier_string, 0)
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

        nr.select_measurements("", nirfmxnr.MeasurementTypes.MODACC, False)
        nr.modacc.configuration.set_synchronization_mode("", synchronization_mode)
        nr.modacc.configuration.set_averaging_enabled("", averaging_enabled)
        nr.modacc.configuration.set_averaging_count("", averaging_count)
        nr.modacc.configuration.set_measurement_length_unit("", measurement_length_unit)
        nr.modacc.configuration.set_measurement_offset("", measurement_offset)
        nr.modacc.configuration.set_measurement_length("", measurement_length)

        nr.modacc.configuration.set_magnitude_and_phase_error_enabled("", nirfmxnr.ModAccMagnitudeAndPhaseErrorEnabled.FALSE)
        nr.modacc.configuration.set_iq_mismatch_estimation_enabled("", nirfmxnr.ModAccIQMismatchEstimationEnabled.FALSE)
        nr.modacc.configuration.set_frequency_error_estimation("", frequency_error_estimation)
        nr.modacc.configuration.set_symbol_clock_error_estimation_enabled("", symbol_clock_error_estimation_enabled)
        nr.modacc.configuration.set_phase_tracking_mode("", phase_tracking_mode)
        nr.modacc.configuration.set_timing_tracking_mode("", timing_tracking_mode)
        nr.modacc.configuration.set_iq_origin_offset_estimation_enabled("", iq_origin_offset_estimation_enabled)
        nr.modacc.configuration.set_evm_reference_data_symbols_mode("", evm_reference_data_symbols_mode)
        if evm_reference_data_symbols_mode == nirfmxnr.ModAccEvmReferenceDataSymbolsMode.REFERENCE_WAVEFORM:
            x0, dx, reference_waveform = _read_reference_waveform_from_tdms(waveform_file_name)
            nr.modacc.configuration.configure_reference_waveform("", x0, dx, reference_waveform)

        nr.initiate("", "")

        # Retrieve results
        composite_rms_evm_mean, error_code = nr.modacc.results.get_composite_rms_evm_mean("")
        in_band_emission_margin, error_code = nr.modacc.results.get_in_band_emission_margin("")

        # Print Results
        print("------------------Measurement------------------\n")
        print(f"Composite RMS EVM Mean (%)                     : {composite_rms_evm_mean}")
        print(f"In-Band Emission Margin (dB)                   : {in_band_emission_margin}\n")

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
        description="Pass arguments for UL ModAcc Speed Optimized Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr.")
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    parser.add_argument(
        "-wfn",
        "--waveform-file-name",
        default="",
        type=str,
        help="Path to the TDMS file containing the reference waveform (used when EVM reference mode is ReferenceWaveform).",
    )
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string, args.waveform_file_name)


def main():
    _main(sys.argv[1:])


def test_main():
    cmd_line = ["--option-string", ""]
    _main(cmd_line)


def test_example():
    example("RFSA", {})


if __name__ == "__main__":
    main()
