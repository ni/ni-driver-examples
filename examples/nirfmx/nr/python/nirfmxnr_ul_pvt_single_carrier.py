r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure Selected Ports.
4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure Trigger Parameters for IQ Power Edge Trigger.
6. Configure Frequency Range, Carrier Bandwidth, Cell ID and Subcarrier Spacing.
7. Configure PUSCH and PUSCH RB Allocation.
8. Configure PUSCH DMRS.
9. Select PVT measurement and enable Traces.
10. Configure Measurement Methods.
11. Configure OFF Power Exclusion Periods.
12. Configure Averaging Parameters for PVT measurement.
13. Initiate the Measurement.
14. Fetch PVT Traces and Measurements.
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

    iq_power_edge_level = -20.0  # dB
    trigger_delay = 0.0  # s
    minimum_quiet_time_mode = nirfmxnr.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 8.0e-6  # s

    frequency_range = nirfmxnr.FrequencyRange.RANGE1
    cell_id = 0
    carrier_bandwidth = 100e6  # Hz
    subcarrier_spacing = 30e3  # Hz

    pusch_transform_precoding_enabled = nirfmxnr.PuschTransformPrecodingEnabled.FALSE
    pusch_modulation_type = nirfmxnr.PuschModulationType.QPSK
    pusch_resource_block_offset = [0]
    pusch_number_of_resource_blocks = [-1]
    pusch_slot_allocation = "1"
    pusch_symbol_allocation = "0-Last"

    pusch_dmrs_power_mode = nirfmxnr.PuschDmrsPowerMode.CDM_GROUPS
    pusch_dmrs_power = 0.0  # dB
    pusch_dmrs_configuration_type = nirfmxnr.PuschDmrsConfigurationType.TYPE1
    pusch_mapping_type = nirfmxnr.PuschMappingType.TYPE_A
    pusch_dmrs_type_a_position = 2
    pusch_dmrs_duration = nirfmxnr.PuschDmrsDuration.SINGLE_SYMBOL
    pusch_dmrs_additional_positions = 0

    measurement_method = nirfmxnr.PvtMeasurementMethod.NORMAL
    off_power_exclusion_before = 0.0  # s
    off_power_exclusion_after = 0.0  # s

    averaging_enabled = nirfmxnr.PvtAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxnr.PvtAveragingType.RMS

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
            True,
        )

        nr.set_frequency_range("", frequency_range)
        nr.component_carrier.set_bandwidth("", carrier_bandwidth)
        nr.component_carrier.set_cell_id("", cell_id)
        nr.component_carrier.set_bandwidth_part_subcarrier_spacing("", subcarrier_spacing)

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

        nr.select_measurements("", nirfmxnr.MeasurementTypes.PVT, True)
        nr.pvt.configuration.configure_measurement_method("", measurement_method)
        nr.pvt.configuration.configure_off_power_exclusion_periods("", off_power_exclusion_before, off_power_exclusion_after)
        nr.pvt.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        nr.initiate("", "")

        # Retrieve results
        (
            measurement_status,
            absolute_off_power_before,
            absolute_off_power_after,
            absolute_on_power,
            burst_width,
            error_code,
        ) = nr.pvt.results.fetch_measurement("", timeout)

        signal_power = numpy.empty(0, dtype=numpy.float32)
        absolute_limit = numpy.empty(0, dtype=numpy.float32)
        nr.pvt.results.fetch_signal_power_trace("", timeout, signal_power, absolute_limit)

        # Print Results
        print("------------------Measurement------------------\n")
        print(f"Measurement Status                   : {measurement_status.name}")
        print(f"Mean Absolute OFF Power Before (dBm) : {absolute_off_power_before}")
        print(f"Mean Absolute OFF Power After (dBm)  : {absolute_off_power_after}")
        print(f"Mean Absolute ON Power (dBm)         : {absolute_on_power}")
        print(f"Burst Width (s)                      : {burst_width}\n")

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
        description="Pass arguments for UL PVT Single Carrier Example",
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
