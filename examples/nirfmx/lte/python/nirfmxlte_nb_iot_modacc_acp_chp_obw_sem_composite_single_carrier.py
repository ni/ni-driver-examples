"""
RFmx LTE NB-IoT ModAcc ACP CHP OBW SEM Composite Single Carrier Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Component Carrier to 200k.
6. Configure NB-IoT Component Carrier.
7. Configure NPUSCH Format.
8. Configure Auto NPUSCH Channel Detection Enabled.
9. Configure NPUSCH Starting Slot.
10. Configure NPUSCH DMRS.
11. Select ACP,ModAcc,OBW,CHP and SEM measurements and enable Traces.
12. Configure Averaging Parameters for ModAcc.
13. Configure Averaging Parameters for ACP.
14. Configure Averaging Parameters for CHP.
15. Configure Averaging Parameters for OBW.
16. Configure Averaging Parameters for SEM.
17. Configure ACP Sweep Time.
18. Configure CHP Sweep Time.
19. Configure OBW Sweep Time.
20. Configure SEM Sweep Time.
21. Configure ModAcc Synchronization Mode and Measurement Interval.
22. Initiate the Measurement.
23. Fetch ModAcc Measurements.
24. Fetch ACP Measurements.
25. Fetch SEM Measurements.
26. Fetch OBW Measurement.
27. Fetch CHP Measurement.
28. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxlte


def example(resource_name, option_string):
    """LTE NB-IoT composite ModAcc/ACP/CHP/OBW/SEM single carrier measurement example."""
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

    synchronization_mode = nirfmxlte.ModAccSynchronizationMode.SLOT
    measurement_offset = 0  # slots
    measurement_length = 1  # slots

    sweep_time_interval = 0.001  # s
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

        lte_signal.select_measurements(
            "",
            nirfmxlte.MeasurementTypes.ACP
            | nirfmxlte.MeasurementTypes.CHP
            | nirfmxlte.MeasurementTypes.MODACC
            | nirfmxlte.MeasurementTypes.OBW
            | nirfmxlte.MeasurementTypes.SEM,
            True,
        )

        lte_signal.modacc.configuration.configure_averaging(
            "", nirfmxlte.ModAccAveragingEnabled.FALSE, averaging_count
        )

        lte_signal.acp.configuration.configure_averaging(
            "", nirfmxlte.AcpAveragingEnabled.FALSE, averaging_count, nirfmxlte.AcpAveragingType.RMS
        )

        lte_signal.chp.configuration.configure_averaging(
            "", nirfmxlte.ChpAveragingEnabled.FALSE, averaging_count, nirfmxlte.ChpAveragingType.RMS
        )

        lte_signal.obw.configuration.configure_averaging(
            "", nirfmxlte.ObwAveragingEnabled.FALSE, averaging_count, nirfmxlte.ObwAveragingType.RMS
        )

        lte_signal.sem.configuration.configure_averaging(
            "", nirfmxlte.SemAveragingEnabled.FALSE, averaging_count, nirfmxlte.SemAveragingType.RMS
        )

        lte_signal.acp.configuration.configure_sweep_time(
            "", nirfmxlte.AcpSweepTimeAuto.TRUE, sweep_time_interval
        )

        lte_signal.chp.configuration.configure_sweep_time(
            "", nirfmxlte.ChpSweepTimeAuto.TRUE, sweep_time_interval
        )

        lte_signal.obw.configuration.configure_sweep_time(
            "", nirfmxlte.ObwSweepTimeAuto.TRUE, sweep_time_interval
        )

        lte_signal.sem.configuration.configure_sweep_time(
            "", nirfmxlte.SemSweepTimeAuto.TRUE, sweep_time_interval
        )

        lte_signal.modacc.configuration.configure_synchronization_mode_and_interval(
            "", synchronization_mode, measurement_offset, measurement_length
        )

        lte_signal.initiate("", "")

        # Fetch ModAcc results
        (
            modacc_mean_rms_composite_evm,
            modacc_max_peak_composite_evm,
            modacc_mean_frequency_error,
            modacc_peak_composite_evm_symbol_index,
            modacc_peak_composite_evm_subcarrier_index,
            modacc_peak_composite_evm_slot_index,
            error_code,
        ) = lte_signal.modacc.results.fetch_composite_evm("", timeout)

        modacc_mean_iq_origin_offset, modacc_mean_iq_gain_imbalance, modacc_mean_iq_quadrature_error, error_code = (
            lte_signal.modacc.results.fetch_iq_impairments("", timeout)
        )

        modacc_in_band_emission_margin, error_code = (
            lte_signal.modacc.results.fetch_in_band_emission_margin("", timeout)
        )

        # Fetch ACP results
        (
            acp_lower_relative_power,
            acp_upper_relative_power,
            acp_lower_absolute_power,
            acp_upper_absolute_power,
            error_code,
        ) = lte_signal.acp.results.fetch_offset_measurement_array("", timeout)

        acp_absolute_power, acp_relative_power, error_code = (
            lte_signal.acp.results.component_carrier.fetch_measurement("", timeout)
        )

        # Fetch SEM results
        (
            sem_lower_offset_measurement_status,
            sem_lower_offset_margin,
            sem_lower_offset_margin_frequency,
            sem_lower_offset_margin_absolute_power,
            sem_lower_offset_margin_relative_power,
            error_code,
        ) = lte_signal.sem.results.fetch_lower_offset_margin_array("", timeout)

        (
            sem_upper_offset_measurement_status,
            sem_upper_offset_margin,
            sem_upper_offset_margin_frequency,
            sem_upper_offset_margin_absolute_power,
            sem_upper_offset_margin_relative_power,
            error_code,
        ) = lte_signal.sem.results.fetch_upper_offset_margin_array("", timeout)

        sem_absolute_integrated_power, sem_relative_integrated_power, error_code = (
            lte_signal.sem.results.component_carrier.fetch_measurement("", timeout)
        )

        sem_measurement_status, error_code = (
            lte_signal.sem.results.fetch_measurement_status("", timeout)
        )

        # Fetch OBW results
        obw_occupied_bandwidth, obw_absolute_power, obw_start_frequency, obw_stop_frequency, error_code = (
            lte_signal.obw.results.fetch_measurement("", timeout)
        )

        # Fetch CHP results
        chp_absolute_power, chp_relative_power, error_code = (
            lte_signal.chp.results.component_carrier.fetch_measurement("", timeout)
        )

        # Print results
        print("************************* ModAcc *************************\n")
        print("---------------- Measurements ----------------")
        print(f"Mean RMS Composite EVM (% or dB)         : {modacc_mean_rms_composite_evm}")
        print(f"Maximum Peak Composite EVM (% or dB)     : {modacc_max_peak_composite_evm}")
        print(f"Peak Composite EVM Slot Index            : {modacc_peak_composite_evm_slot_index}")
        print(f"Peak Composite EVM Symbol Index          : {modacc_peak_composite_evm_symbol_index}")
        print(f"Peak Composite EVM Subcarrier Index      : {modacc_peak_composite_evm_subcarrier_index}")
        print(f"Mean Frequency Error    (Hz)             : {modacc_mean_frequency_error}")
        print(f"Mean IQ Origin Offset   (dBc)            : {modacc_mean_iq_origin_offset}")
        print(f"Mean IQ Gain Imbalance (dB)              : {modacc_mean_iq_gain_imbalance}")
        print(f"Mean IQ Quadrature Error (deg)           : {modacc_mean_iq_quadrature_error}")
        print(f"In-Band Emission Margin (dB)             : {modacc_in_band_emission_margin}")

        print("\n************************* ACP *************************\n")
        print(f"Carrier Absolute Power (dBm)    : {acp_absolute_power}")
        print("\n------ Offset Channel Measurements -------")
        for i in range(len(acp_lower_relative_power)):
            print(f"\nOffset  :{i}")
            print(f"Lower Relative Power (dB)  : {acp_lower_relative_power[i]}")
            print(f"Upper Relative Power (dB)  : {acp_upper_relative_power[i]}")
            print(f"Lower Absolute Power (dBm) : {acp_lower_absolute_power[i]}")
            print(f"Upper Absolute Power (dBm) : {acp_upper_absolute_power[i]}")

        print("\n************************* SEM *************************\n")
        print(f"Measurement Status                      : {sem_measurement_status.name}")
        print(f"Carrier Absolute Integrated Power (dBm) : {sem_absolute_integrated_power}")
        print("\n---- Lower Offset Segment Measurements ---- ")
        for i in range(len(sem_lower_offset_margin)):
            print(f"\nOffset  : {i}")
            print(f"Measurement Status            : {sem_lower_offset_measurement_status[i].name}")
            print(f"Margin                 (dB)   : {sem_lower_offset_margin[i]}")
            print(f"Margin Frequency       (Hz)   : {sem_lower_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power  (dBm)  : {sem_lower_offset_margin_absolute_power[i]}")
        print("\n---- Upper Offset Segment Measurements ---- ")
        for i in range(len(sem_upper_offset_margin)):
            print(f"\nOffset  :{i}")
            print(f"Measurement Status            : {sem_upper_offset_measurement_status[i].name}")
            print(f"Margin                 (dB)   : {sem_upper_offset_margin[i]}")
            print(f"Margin Frequency       (Hz)   : {sem_upper_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power  (dBm)  : {sem_upper_offset_margin_absolute_power[i]}")

        print("\n************************* OBW *************************\n")
        print("---------------- Measurement ----------------")
        print(f"Occupied Bandwidth  (Hz)  : {obw_occupied_bandwidth}")
        print(f"Absolute Power      (dBm) : {obw_absolute_power}")
        print(f"Start Frequency     (Hz)  : {obw_start_frequency}")
        print(f"Stop Frequency      (Hz)  : {obw_stop_frequency}")

        print("\n************************* CHP *************************\n")
        print(f"Carrier Absolute Power (dBm)   : {chp_absolute_power}")

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
        description="RFmx LTE NB-IoT ModAcc ACP CHP OBW SEM Composite Single Carrier Example",
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
