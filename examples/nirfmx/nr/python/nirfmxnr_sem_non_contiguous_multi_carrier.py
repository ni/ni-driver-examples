r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure Selected Ports.
4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure Trigger Parameters for Digital Edge Trigger.
6. Configure Number of Subblocks and Link Direction.
7. Configure Frequency Range, Component Carrier Spacing, Channel Raster,
   Component Carrier Center Frequency, Subblock Frequency and Number of Component Carriers.
8. Configure Component Carriers.
9. Configure Bandwidth Part Subcarrier Spacing.
10. Select SEM measurement and enable Traces.
11. Configure Uplink Mask Type, or Downlink Mask, Band, gNodeB Category and Delta F_Max(Hz) based on Link Direction.
12. Configure Component Carrier Rated Output Power based on Link Direction.
13. Configure Offsets.
14. Configure Sweep Time Parameters.
15. Configure Averaging Parameters for SEM measurement.
16. Initiate the Measurement.
17. Fetch SEM Measurements and Traces.
18. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxnr
import numpy

import nirfmxinstr

NUMBER_OF_SUBBLOCKS = 2
NUMBER_OF_COMPONENT_CARRIERS = 2
NUMBER_OF_OFFSETS = 4


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

    uplink_mask_type = nirfmxnr.SemUplinkMaskType.GENERAL
    gnodeb_category = nirfmxnr.gNodeBCategory.WIDE_AREA_BASE_STATION_CATEGORY_A
    downlink_mask_type = nirfmxnr.SemDownlinkMaskType.STANDARD
    delta_f_maximum = 15.0e6  # Hz
    band = 78

    subcarrier_spacing = 30e3  # Hz

    # Per-subblock inputs
    subblock_frequency = [0.0, 200e6]  # Hz
    component_carrier_spacing_type = [
        nirfmxnr.ComponentCarrierSpacingType.NOMINAL,
        nirfmxnr.ComponentCarrierSpacingType.NOMINAL,
    ]
    channel_raster = [15e3, 15e3]  # Hz
    component_carrier_at_center_frequency = [-1, -1]

    component_carrier_bandwidth = [
        [100e6, 100e6],
        [100e6, 100e6],
    ]  # Hz
    component_carrier_frequency = [
        [-49.98e6, 50.01e6],
        [-49.98e6, 50.01e6],
    ]  # Hz
    component_carrier_rated_output_power = [
        [0.0, 0.0],
        [0.0, 0.0],
    ]  # dBm

    offset_start_frequency = [
        [15.0e3, 1.5e6, 5.5e6, 20.5e6],
        [15.0e3, 1.5e6, 5.5e6, 20.5e6],
    ]  # Hz
    offset_stop_frequency = [
        [985.0e3, 4.5e6, 19.5e6, 24.5e6],
        [985.0e3, 4.5e6, 19.5e6, 24.5e6],
    ]  # Hz
    offset_sideband = [
        [nirfmxnr.SemOffsetSideband.BOTH] * NUMBER_OF_OFFSETS,
        [nirfmxnr.SemOffsetSideband.BOTH] * NUMBER_OF_OFFSETS,
    ]
    offset_rbw = [
        [10.0e3, 250.0e3, 250.0e3, 250.0e3],
        [10.0e3, 250.0e3, 250.0e3, 250.0e3],
    ]  # Hz
    offset_rbw_filter_type = [
        [nirfmxnr.SemOffsetRbwFilterType.GAUSSIAN] * NUMBER_OF_OFFSETS,
        [nirfmxnr.SemOffsetRbwFilterType.GAUSSIAN] * NUMBER_OF_OFFSETS,
    ]
    bandwidth_integral = [
        [3, 4, 4, 4],
        [3, 4, 4, 4],
    ]
    limit_fail_mask = [
        [nirfmxnr.SemOffsetLimitFailMask.ABSOLUTE] * NUMBER_OF_OFFSETS,
        [nirfmxnr.SemOffsetLimitFailMask.ABSOLUTE] * NUMBER_OF_OFFSETS,
    ]
    absolute_limit_start = [
        [-22.50, -8.5, -11.5, -23.5],
        [-22.50, -8.5, -11.5, -23.5],
    ]  # dBm
    absolute_limit_stop = [
        [-22.50, -8.5, -11.5, -23.5],
        [-22.50, -8.5, -11.5, -23.5],
    ]  # dBm
    relative_limit_start = [
        [-53.0, -53.0, -53.0, -53.0],
        [-53.0, -53.0, -53.0, -51.5],
    ]  # dB
    relative_limit_stop = [
        [-60.0, -60.0, -60.0, -60.0],
        [-60.0, -60.0, -60.0, -58.5],
    ]  # dB

    sweep_time_auto = nirfmxnr.SemSweepTimeAuto.TRUE
    sweep_time_interval = 1.0e-3  # s

    averaging_enabled = nirfmxnr.SemAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxnr.SemAveragingType.RMS

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

        nr.set_number_of_subblocks("", NUMBER_OF_SUBBLOCKS)
        nr.set_link_direction("", link_direction)

        for i in range(NUMBER_OF_SUBBLOCKS):
            subblock_string = nirfmxnr.NR.build_subblock_string("", i)
            nr.set_frequency_range(subblock_string, frequency_range)
            nr.set_component_carrier_spacing_type(subblock_string, component_carrier_spacing_type[i])
            nr.set_channel_raster(subblock_string, channel_raster[i])
            nr.set_component_carrier_at_center_frequency(subblock_string, component_carrier_at_center_frequency[i])
            nr.set_subblock_frequency(subblock_string, subblock_frequency[i])
            nr.component_carrier.set_number_of_component_carriers(subblock_string, NUMBER_OF_COMPONENT_CARRIERS)

            for j in range(NUMBER_OF_COMPONENT_CARRIERS):
                carrier_string = nirfmxnr.NR.build_carrier_string(subblock_string, j)
                nr.component_carrier.set_bandwidth(carrier_string, component_carrier_bandwidth[i][j])
                nr.component_carrier.set_frequency(carrier_string, component_carrier_frequency[i][j])

            carrier_string = nirfmxnr.NR.build_carrier_string(subblock_string, -1)
            nr.component_carrier.set_bandwidth_part_subcarrier_spacing(carrier_string, subcarrier_spacing)

        nr.select_measurements("", nirfmxnr.MeasurementTypes.SEM, True)

        if link_direction == nirfmxnr.LinkDirection.UPLINK:
            nr.sem.configuration.configure_uplink_mask_type("", uplink_mask_type)
        else:
            nr.configure_gnodeb_category("", gnodeb_category)
            nr.sem.configuration.set_downlink_mask_type("", downlink_mask_type)
            nr.sem.configuration.set_delta_f_maximum("", delta_f_maximum)
            all_subblocks_string = nirfmxnr.NR.build_subblock_string("", -1)
            nr.set_band(all_subblocks_string, band)

        for i in range(NUMBER_OF_SUBBLOCKS):
            subblock_string = nirfmxnr.NR.build_subblock_string("", i)
            if link_direction == nirfmxnr.LinkDirection.DOWNLINK:
                nr.sem.configuration.component_carrier.configure_rated_output_power_array(
                    subblock_string, component_carrier_rated_output_power[i]
                )
            nr.sem.configuration.configure_number_of_offsets(subblock_string, NUMBER_OF_OFFSETS)
            nr.sem.configuration.configure_offset_frequency_array(
                subblock_string, offset_start_frequency[i], offset_stop_frequency[i], offset_sideband[i]
            )
            nr.sem.configuration.configure_offset_rbw_filter_array(
                subblock_string, offset_rbw[i], offset_rbw_filter_type[i]
            )
            nr.sem.configuration.configure_offset_bandwidth_integral_array(subblock_string, bandwidth_integral[i])
            nr.sem.configuration.configure_offset_limit_fail_mask_array(subblock_string, limit_fail_mask[i])
            nr.sem.configuration.configure_offset_absolute_limit_array(
                subblock_string, absolute_limit_start[i], absolute_limit_stop[i]
            )
            nr.sem.configuration.configure_offset_relative_limit_array(
                subblock_string, relative_limit_start[i], relative_limit_stop[i]
            )

        nr.sem.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        nr.sem.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        nr.initiate("", "")

        # Retrieve results - per-subblock fetches first
        subblock_results = []
        for i in range(NUMBER_OF_SUBBLOCKS):
            subblock_string = nirfmxnr.NR.build_subblock_string("", i)

            (
                upper_status,
                upper_margin,
                upper_margin_frequency,
                upper_margin_abs_power,
                upper_margin_rel_power,
                error_code,
            ) = nr.sem.results.fetch_upper_offset_margin_array(subblock_string, timeout)

            (
                lower_status,
                lower_margin,
                lower_margin_frequency,
                lower_margin_abs_power,
                lower_margin_rel_power,
                error_code,
            ) = nr.sem.results.fetch_lower_offset_margin_array(subblock_string, timeout)

            subblock_power, integration_bandwidth, freq, error_code = (
                nr.sem.results.fetch_subblock_measurement(subblock_string, timeout)
            )

            subblock_results.append((
                upper_status, upper_margin, upper_margin_frequency, upper_margin_abs_power,
                lower_status, lower_margin, lower_margin_frequency, lower_margin_abs_power,
                subblock_power, integration_bandwidth, freq,
            ))

        total_aggregated_power, error_code = nr.sem.results.fetch_total_aggregated_power("", timeout)
        measurement_status, error_code = nr.sem.results.fetch_measurement_status("", timeout)

        spectrum = numpy.empty(0, dtype=numpy.float32)
        composite_mask = numpy.empty(0, dtype=numpy.float32)
        nr.sem.results.fetch_spectrum("", timeout, spectrum, composite_mask)

        print(f"Total Aggregated Power (dBm)    :{total_aggregated_power}")
        print(f"Measurement Status              :{measurement_status.name}")
        print("\n--------------------Subblock Measurements--------------------")

        for i, (
            upper_status, upper_margin, upper_margin_frequency, upper_margin_abs_power,
            lower_status, lower_margin, lower_margin_frequency, lower_margin_abs_power,
            subblock_power, integration_bandwidth, freq,
        ) in enumerate(subblock_results):
            print(f"\nSubblock {i}\n")
            print(f"Subblock Power (dBm)            :{subblock_power}")
            print(f"Integration Bandwidth (Hz)      :{integration_bandwidth}")
            print(f"Frequency (Hz)                  :{freq}")
            print("\nOffset Segment Measurements")
            for j in range(len(lower_margin)):
                print(f"\nLower Offset Segment Measurement {j}")
                print(f"Measurement Status              :{lower_status[j].name}")
                print(f"Margin (dB)                     :{lower_margin[j]}")
                print(f"Margin Frequency (Hz)           :{lower_margin_frequency[j]}")
                print(f"Margin Absolute Power (dBm)     :{lower_margin_abs_power[j]}")
                print(f"\nUpper Offset Segment Measurement {j}")
                print(f"Measurement Status              :{upper_status[j].name}")
                print(f"Margin (dB)                     :{upper_margin[j]}")
                print(f"Margin Frequency (Hz)           :{upper_margin_frequency[j]}")
                print(f"Margin Absolute Power (dBm)     :{upper_margin_abs_power[j]}")
            print("\n-----------------------------------------------------")

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
        description="Pass arguments for SEM Non-Contiguous Multi-Carrier Example",
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
