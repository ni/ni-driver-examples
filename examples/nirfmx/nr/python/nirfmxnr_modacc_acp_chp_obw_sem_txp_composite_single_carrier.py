r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure Selected Ports.
4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure Trigger Type and Trigger Parameters.
6. Configure Link Direction as Uplink, Frequency Range, Band, Component Carrier and Subcarrier Spacing.
7. Select ModAcc, ACP, CHP, OBW, SEM and TXP measurements and enable Traces.
8. Configure ACP Sweep Time.
9. Configure CHP Sweep Time.
10. Configure OBW Sweep Time.
11. Configure SEM Sweep Time.
12. Configure Averaging Parameters for ModAcc.
13. Configure Averaging Parameters for ACP.
14. Configure Averaging Parameters for CHP.
15. Configure Averaging Parameters for OBW.
16. Configure Averaging Parameters for SEM.
17. Configure Averaging Parameters for TXP.
18. Configure Measurement Interval for ModAcc.
19. Configure Measurement Interval for TXP.
20. Configure SEM Uplink Mask Type, or Downlink Mask, gNodeB Category, Delta F_Max (Hz) and Component Carrier Rated
    Output Power depending on Link Direction.
21. Initiate the Measurement.
22. Fetch ModAcc Measurements.
23. Fetch ACP Measurements.
24. Fetch CHP Measurements.
25. Fetch OBW Measurements.
26. Fetch SEM Measurements.
27. Fetch TXP Measurements.
28. Close RFmx Session.
"""

import argparse
import sys

import nirfmxnr
import numpy

import nirfmxinstr


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

    link_direction = nirfmxnr.LinkDirection.UPLINK
    frequency_range = nirfmxnr.FrequencyRange.RANGE1
    carrier_bandwidth = 100e6  # Hz
    subcarrier_spacing = 30e3  # Hz
    modacc_band = 78

    measurement_length_unit = nirfmxnr.ModAccMeasurementLengthUnit.SLOT
    measurement_offset = 0.0
    measurement_length = 1

    txp_measurement_offset = 0.0  # s
    txp_measurement_length = 1.0e-3  # s

    uplink_mask_type = nirfmxnr.SemUplinkMaskType.GENERAL

    gnodeb_category = nirfmxnr.gNodeBCategory.WIDE_AREA_BASE_STATION_CATEGORY_A
    downlink_mask_type = nirfmxnr.SemDownlinkMaskType.STANDARD
    delta_f_maximum = 15.0e6  # Hz
    component_carrier_rated_output_power = 0.0  # dBm

    sweep_time_interval = 1.0e-3  # s
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

        nr.set_link_direction("", link_direction)
        nr.set_frequency_range("", frequency_range)
        nr.set_band("", modacc_band)
        nr.component_carrier.set_bandwidth("", carrier_bandwidth)
        nr.component_carrier.set_bandwidth_part_subcarrier_spacing("", subcarrier_spacing)

        nr.select_measurements(
            "",
            nirfmxnr.MeasurementTypes.MODACC
            | nirfmxnr.MeasurementTypes.ACP
            | nirfmxnr.MeasurementTypes.CHP
            | nirfmxnr.MeasurementTypes.OBW
            | nirfmxnr.MeasurementTypes.SEM
            | nirfmxnr.MeasurementTypes.TXP,
            True,
        )

        nr.acp.configuration.configure_sweep_time("", nirfmxnr.AcpSweepTimeAuto.TRUE, sweep_time_interval)
        nr.chp.configuration.configure_sweep_time("", nirfmxnr.ChpSweepTimeAuto.TRUE, sweep_time_interval)
        nr.obw.configuration.configure_sweep_time("", nirfmxnr.ObwSweepTimeAuto.TRUE, sweep_time_interval)
        nr.sem.configuration.configure_sweep_time("", nirfmxnr.SemSweepTimeAuto.TRUE, sweep_time_interval)

        nr.modacc.configuration.set_averaging_enabled("", nirfmxnr.ModAccAveragingEnabled.FALSE)
        nr.modacc.configuration.set_averaging_count("", averaging_count)

        nr.acp.configuration.configure_averaging("", nirfmxnr.AcpAveragingEnabled.FALSE, averaging_count, nirfmxnr.AcpAveragingType.RMS)
        nr.chp.configuration.configure_averaging("", nirfmxnr.ChpAveragingEnabled.FALSE, averaging_count, nirfmxnr.ChpAveragingType.RMS)
        nr.obw.configuration.configure_averaging("", nirfmxnr.ObwAveragingEnabled.FALSE, averaging_count, nirfmxnr.ObwAveragingType.RMS)
        nr.sem.configuration.configure_averaging("", nirfmxnr.SemAveragingEnabled.FALSE, averaging_count, nirfmxnr.SemAveragingType.RMS)

        nr.txp.configuration.set_averaging_enabled("", nirfmxnr.TxpAveragingEnabled.FALSE)
        nr.txp.configuration.set_averaging_count("", averaging_count)

        nr.modacc.configuration.set_measurement_length_unit("", measurement_length_unit)
        nr.modacc.configuration.set_measurement_offset("", measurement_offset)
        nr.modacc.configuration.set_measurement_length("", measurement_length)

        nr.txp.configuration.set_measurement_offset("", txp_measurement_offset)
        nr.txp.configuration.set_measurement_interval("", txp_measurement_length)

        if link_direction == nirfmxnr.LinkDirection.UPLINK:
            nr.sem.configuration.configure_uplink_mask_type("", uplink_mask_type)
        else:
            nr.configure_gnodeb_category("", gnodeb_category)
            nr.sem.configuration.set_downlink_mask_type("", downlink_mask_type)
            nr.sem.configuration.set_delta_f_maximum("", delta_f_maximum)
            nr.sem.configuration.component_carrier.configure_rated_output_power("", component_carrier_rated_output_power)

        nr.initiate("", "")

        # Fetch ModAcc results
        composite_rms_evm_mean, error_code = nr.modacc.results.get_composite_rms_evm_mean("")
        composite_peak_evm_maximum, error_code = nr.modacc.results.get_composite_peak_evm_maximum("")
        component_carrier_frequency_error_mean, error_code = nr.modacc.results.get_component_carrier_frequency_error_mean("")
        component_carrier_iq_origin_offset_mean, error_code = nr.modacc.results.get_component_carrier_iq_origin_offset_mean("")

        # Fetch ACP results
        (
            acp_lower_relative_power,
            acp_upper_relative_power,
            acp_lower_absolute_power,
            acp_upper_absolute_power,
            error_code,
        ) = nr.acp.results.fetch_offset_measurement_array("", timeout)
        acp_absolute_power, acp_relative_power, error_code = nr.acp.results.component_carrier.fetch_measurement("", timeout)

        # Fetch CHP results
        chp_absolute_power, chp_relative_power, error_code = nr.chp.results.component_carrier.fetch_measurement("", timeout)

        # Fetch OBW results
        obw_occupied_bandwidth, obw_absolute_power, obw_start_frequency, obw_stop_frequency, error_code = \
            nr.obw.results.fetch_measurement("", timeout)

        # Fetch SEM results
        (
            sem_lower_offset_measurement_status,
            sem_lower_offset_margin,
            sem_lower_offset_margin_frequency,
            sem_lower_offset_margin_absolute_power,
            sem_lower_offset_margin_relative_power,
            error_code,
        ) = nr.sem.results.fetch_lower_offset_margin_array("", timeout)
        (
            sem_upper_offset_measurement_status,
            sem_upper_offset_margin,
            sem_upper_offset_margin_frequency,
            sem_upper_offset_margin_absolute_power,
            sem_upper_offset_margin_relative_power,
            error_code,
        ) = nr.sem.results.fetch_upper_offset_margin_array("", timeout)
        sem_absolute_integrated_power, sem_peak_absolute_power, sem_peak_frequency, sem_relative_integrated_power, error_code = \
            nr.sem.results.component_carrier.fetch_measurement("", timeout)
        sem_measurement_status, error_code = nr.sem.results.fetch_measurement_status("", timeout)

        # Fetch TXP results
        average_power_mean, peak_power_maximum, error_code = nr.txp.results.fetch_measurement("", timeout)

        # Print ModAcc Results
        print("************************* ModAcc *************************\n")
        print(f"Composite RMS EVM Mean (%)                     : {composite_rms_evm_mean}")
        print(f"Composite Peak EVM Maximum (%)                 : {composite_peak_evm_maximum}")
        print(f"Component Carrier Frequency Error Mean (Hz)    : {component_carrier_frequency_error_mean}")
        print(f"Component Carrier IQ Origin Offset Mean (dBc)  : {component_carrier_iq_origin_offset_mean}\n")

        # Print CHP Results
        print("************************* CHP *************************\n")
        print(f"Carrier Absolute Power (dBm)                   : {chp_absolute_power}\n")

        # Print ACP Results
        print("************************* ACP *************************\n")
        print(f"Carrier Absolute Power (dBm)                   : {acp_absolute_power}")
        print("\n------- Offset Channel Measurements -------")
        for i in range(len(acp_lower_relative_power)):
            print(f"\nOffset  {i}")
            print(f"Lower Relative Power (dB)                      : {acp_lower_relative_power[i]}")
            print(f"Upper Relative Power (dB)                      : {acp_upper_relative_power[i]}")
            print(f"Lower Absolute Power (dBm)                     : {acp_lower_absolute_power[i]}")
            print(f"Upper Absolute Power (dBm)                     : {acp_upper_absolute_power[i]}")

        # Print OBW Results
        print("\n\n************************* OBW *************************\n")
        print(f"Occupied Bandwidth (Hz)                        : {obw_occupied_bandwidth}")
        print(f"Absolute Power (dBm)                           : {obw_absolute_power}")
        print(f"Start Frequency (Hz)                           : {obw_start_frequency}")
        print(f"Stop Frequency (Hz)                            : {obw_stop_frequency}\n")

        # Print SEM Results
        print("************************* SEM *************************\n")
        print(f"Measurement Status                             : {sem_measurement_status.name}")
        print(f"Carrier Absolute Integrated Power (dBm)        : {sem_absolute_integrated_power}")
        print("\n----- Lower Offset Segment Measurements -----")
        for i in range(len(sem_lower_offset_margin)):
            print(f"\nOffset  {i}")
            print(f"Measurement Status                             : {sem_lower_offset_measurement_status[i].name}")
            print(f"Margin (dB)                                    : {sem_lower_offset_margin[i]}")
            print(f"Margin Frequency (Hz)                          : {sem_lower_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power (dBm)                    : {sem_lower_offset_margin_absolute_power[i]}")
        print("\n----- Upper Offset Segment Measurements -----")
        for i in range(len(sem_upper_offset_margin)):
            print(f"\nOffset  {i}")
            print(f"Measurement Status                             : {sem_upper_offset_measurement_status[i].name}")
            print(f"Margin (dB)                                    : {sem_upper_offset_margin[i]}")
            print(f"Margin Frequency (Hz)                          : {sem_upper_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power (dBm)                    : {sem_upper_offset_margin_absolute_power[i]}")

        # Print TXP Results
        print("\n\n************************* TXP *************************\n")
        print(f"Average Power Mean (dBm)                       : {average_power_mean}")
        print(f"Peak Power Maximum (dBm)                       : {peak_power_maximum}\n")

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
        description="Pass arguments for ModAcc ACP CHP OBW SEM TXP Composite Single Carrier Example",
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
