"""
RFmx LTE UL ModAcc ACP CHP OBW SEM Composite Single Carrier Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Component Carrier.
6. Configure operating Band.
7. Configure Duplex Mode.
8. Configure Auto DMRS Detection Enabled.
9. Select ACP,CHP,ModAcc,OBW and SEM measurements and enable Traces.
10. Configure Averaging Parameters for ModAcc.
11. Configure Averaging Parameters for ACP.
12. Configure Averaging Parameters for CHP.
13. Configure Averaging Parameters for OBW.
14. Configure Averaging Parameters for SEM.
15. Configure ACP Sweep Time.
16. Configure CHP Sweep Time.
17. Configure OBW Sweep Time.
18. Configure SEM Sweep Time.
19. Configure Standard Mask Type for SEM.
20. Configure Synchronization Mode and Measurement Interval.
21. Initiate the Measurement.
22. Fetch SEM Measurements.
23. Fetch OBW Measurements.
24. Fetch CHP Measurements.
25. Fetch ACP Measurements.
26. Fetch ModAcc Measurements.
27. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxlte
import numpy


def example(resource_name, option_string):
    """LTE UL ModAcc+ACP+CHP+OBW+SEM composite single carrier measurement example."""
    # Configuration parameters
    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    center_frequency = 1.95e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    enable_trigger = False
    digital_edge_source = "PFI0"
    digital_edge = nirfmxlte.DigitalEdgeTriggerEdge.RISING_EDGE
    trigger_delay = 0.0  # s

    duplex_scheme = nirfmxlte.DuplexScheme.FDD
    uplink_downlink_configuration = nirfmxlte.UplinkDownlinkConfiguration.CONFIGURATION_0

    band = 1
    component_carrier_bandwidth = 10e6  # Hz
    component_carrier_frequency = 0.0  # Hz
    cell_id = 0

    auto_dmrs_detection_enabled = nirfmxlte.AutoDmrsDetectionEnabled.TRUE

    synchronization_mode = nirfmxlte.ModAccSynchronizationMode.SLOT
    measurement_offset = 0  # slots
    measurement_length = 1  # slots

    uplink_mask_type = nirfmxlte.SemUplinkMaskType.GENERAL_NS_01

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

        lte_signal.configure_digital_edge_trigger(
            "", digital_edge_source, digital_edge, trigger_delay, enable_trigger
        )

        lte_signal.component_carrier.configure(
            "", component_carrier_bandwidth, component_carrier_frequency, cell_id
        )

        lte_signal.configure_band("", band)

        lte_signal.configure_duplex_scheme("", duplex_scheme, uplink_downlink_configuration)

        lte_signal.configure_auto_dmrs_detection_enabled("", auto_dmrs_detection_enabled)

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

        lte_signal.sem.configuration.configure_uplink_mask_type("", uplink_mask_type)

        lte_signal.modacc.configuration.configure_synchronization_mode_and_interval(
            "", synchronization_mode, measurement_offset, measurement_length
        )

        lte_signal.initiate("", "")

        # --- Fetch SEM results ---
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

        sem_measurement_status, error_code = lte_signal.sem.results.fetch_measurement_status(
            "", timeout
        )

        # --- Fetch OBW results ---
        obw_occupied_bandwidth, obw_absolute_power, obw_start_frequency, obw_stop_frequency, error_code = (
            lte_signal.obw.results.fetch_measurement("", timeout)
        )

        # --- Fetch CHP results ---
        chp_absolute_power, chp_relative_power, error_code = (
            lte_signal.chp.results.component_carrier.fetch_measurement("", timeout)
        )

        # --- Fetch ACP results ---
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

        # --- Fetch ModAcc results ---
        (
            modacc_mean_rms_composite_evm,
            modacc_max_peak_composite_evm,
            modacc_mean_frequency_error,
            modacc_peak_composite_evm_symbol_index,
            modacc_peak_composite_evm_subcarrier_index,
            modacc_peak_composite_evm_slot_index,
            error_code,
        ) = lte_signal.modacc.results.fetch_composite_evm("", timeout)

        (
            modacc_mean_iq_origin_offset,
            modacc_mean_iq_gain_imbalance,
            modacc_mean_iq_quadrature_error,
            error_code,
        ) = lte_signal.modacc.results.fetch_iq_impairments("", timeout)

        # --- Print results ---
        print("************************* ModAcc *************************\n")
        print(f"Mean RMS Composite EVM  (%)               : {modacc_mean_rms_composite_evm}")
        print(f"Max Peak Composite EVM  (% )              : {modacc_max_peak_composite_evm}")
        print(f"Mean FrequencyError     (Hz)              : {modacc_mean_frequency_error}")
        print(f"Mean IQ Origin Offset   (dBc)             : {modacc_mean_iq_origin_offset}")

        print("\n************************* CHP *************************\n")
        print(f"Carrier Absolute Power  (dBm)             : {chp_absolute_power}")

        print("\n************************* ACP *************************\n")
        print(f"Carrier Absolute Power  (dBm)             : {acp_absolute_power}")
        print("\n------- Offset Channel Measurements -------")
        for i in range(len(acp_lower_relative_power)):
            print(f"\nOffset  : {i}")
            print(f"Lower Relative Power (dB)                 : {acp_lower_relative_power[i]}")
            print(f"Upper Relative Power (dB)                 : {acp_upper_relative_power[i]}")
            print(f"Lower Absolute Power (dBm)                : {acp_lower_absolute_power[i]}")
            print(f"Upper Absolute Power (dBm)                : {acp_upper_absolute_power[i]}")

        print("\n************************* OBW *************************\n")
        print(f"Occupied Bandwidth  (Hz)                  : {obw_occupied_bandwidth}")
        print(f"Absolute Power      (dBm)                 : {obw_absolute_power}")
        print(f"Start Frequency     (Hz)                  : {obw_start_frequency}")
        print(f"Stop Frequency      (Hz)                  : {obw_stop_frequency}")

        print("\n************************* SEM *************************\n")
        print(f"Measurement Status                        : {sem_measurement_status.name}")
        print(f"Carrier Absolute Integrated Power  (dBm)  : {sem_absolute_integrated_power}")
        print("\n----- Lower Offset Segment Measurements -----")
        for i in range(len(sem_lower_offset_margin)):
            print(f"\nOffset  : {i}")
            print(f"Measurement Status                        : {sem_lower_offset_measurement_status[i].name}")
            print(f"Margin                 (dB)               : {sem_lower_offset_margin[i]}")
            print(f"Margin Frequency       (Hz)               : {sem_lower_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power  (dBm)              : {sem_lower_offset_margin_absolute_power[i]}")
        print("\n----- Upper Offset Segment Measurements -----")
        for i in range(len(sem_upper_offset_margin)):
            print(f"\nOffset  : {i}")
            print(f"Measurement Status                        : {sem_upper_offset_measurement_status[i].name}")
            print(f"Margin                 (dB)               : {sem_upper_offset_margin[i]}")
            print(f"Margin Frequency       (Hz)               : {sem_upper_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power  (dBm)              : {sem_upper_offset_margin_absolute_power[i]}")

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
        description="Pass arguments for LTE UL ModAcc ACP CHP OBW SEM Composite Single Carrier Example",
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
