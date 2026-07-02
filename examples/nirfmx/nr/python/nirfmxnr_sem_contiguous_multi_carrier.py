r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure Selected Ports.
4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure Trigger Parameters for Digital Edge Trigger.
6. Configure Link Direction, Frequency Range, Channel Raster and Component Carrier Spacing.
7. Configure Bandwidth Part Subcarrier Spacing.
8. Configure Component Carriers.
9. Select SEM measurement and enable Traces.
10. Configure Offsets.
11. Configure Uplink Mask Type.
12. Configure Sweep Time Parameters.
13. Configure Averaging Parameters for SEM measurement.
14. Initiate the Measurement.
15. Fetch SEM Measurements and Traces.
16. Close RFmx Session.
"""

import argparse
import sys

import nirfmxnr
import numpy

import nirfmxinstr

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
    uplink_mask_type = nirfmxnr.SemUplinkMaskType.GENERAL

    gnodeb_category = nirfmxnr.gNodeBCategory.WIDE_AREA_BASE_STATION_CATEGORY_A
    downlink_mask_type = nirfmxnr.SemDownlinkMaskType.STANDARD
    delta_f_maximum = 15.0e6  # Hz
    band = 78

    component_carrier_bandwidth = [100e6, 100e6]  # Hz
    component_carrier_frequency = [-49.98e6, 50.01e6]  # Hz
    component_carrier_rated_output_power = [0.0, 0.0]  # dBm

    offset_start_frequency = [15.0e3, 1.5e6, 5.5e6, 40.3e6]  # Hz
    offset_stop_frequency = [985.0e3, 5.5e6, 39.3e6, 44.3e6]  # Hz
    offset_sideband = [
        nirfmxnr.SemOffsetSideband.BOTH,
        nirfmxnr.SemOffsetSideband.BOTH,
        nirfmxnr.SemOffsetSideband.BOTH,
        nirfmxnr.SemOffsetSideband.BOTH,
    ]
    offset_rbw = [10.0e3, 250.0e3, 1.0e6, 1.0e6]  # Hz
    offset_rbw_filter_type = [
        nirfmxnr.SemOffsetRbwFilterType.GAUSSIAN,
        nirfmxnr.SemOffsetRbwFilterType.GAUSSIAN,
        nirfmxnr.SemOffsetRbwFilterType.GAUSSIAN,
        nirfmxnr.SemOffsetRbwFilterType.GAUSSIAN,
    ]
    bandwidth_integral = [3, 4, 1, 1]
    limit_fail_mask = [
        nirfmxnr.SemOffsetLimitFailMask.ABSOLUTE,
        nirfmxnr.SemOffsetLimitFailMask.ABSOLUTE,
        nirfmxnr.SemOffsetLimitFailMask.ABSOLUTE,
        nirfmxnr.SemOffsetLimitFailMask.ABSOLUTE,
    ]
    absolute_limit_start = [-22.5, -8.5, -11.5, -23.5]  # dBm
    absolute_limit_stop = [-22.5, -8.5, -11.5, -23.5]  # dBm
    relative_limit_start = [-53.0, -53.0, -53.0, -53.0]  # dB
    relative_limit_stop = [-60.0, -60.0, -60.0, -60.0]  # dB

    frequency_range = nirfmxnr.FrequencyRange.RANGE1
    component_carrier_spacing_type = nirfmxnr.ComponentCarrierSpacingType.NOMINAL
    channel_raster = 15e3  # Hz
    component_carrier_at_center_frequency = -1
    subcarrier_spacing = 30e3  # Hz

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

        nr.set_link_direction("", link_direction)
        nr.set_frequency_range("", frequency_range)
        nr.set_channel_raster("", channel_raster)
        nr.set_component_carrier_spacing_type("", component_carrier_spacing_type)
        nr.set_component_carrier_at_center_frequency("", component_carrier_at_center_frequency)

        nr.component_carrier.set_bandwidth_part_subcarrier_spacing("carrier::all", subcarrier_spacing)
        nr.component_carrier.set_number_of_component_carriers("", NUMBER_OF_COMPONENT_CARRIERS)

        subblock_string = nirfmxnr.NR.build_subblock_string("", 0)
        for i in range(NUMBER_OF_COMPONENT_CARRIERS):
            carrier_string = nirfmxnr.NR.build_carrier_string(subblock_string, i)
            nr.component_carrier.set_bandwidth(carrier_string, component_carrier_bandwidth[i])
            nr.component_carrier.set_frequency(carrier_string, component_carrier_frequency[i])

        nr.select_measurements("", nirfmxnr.MeasurementTypes.SEM, True)

        nr.sem.configuration.configure_number_of_offsets("", NUMBER_OF_OFFSETS)
        nr.sem.configuration.configure_offset_frequency_array("", offset_start_frequency, offset_stop_frequency, offset_sideband)
        nr.sem.configuration.configure_offset_rbw_filter_array("", offset_rbw, offset_rbw_filter_type)
        nr.sem.configuration.configure_offset_bandwidth_integral_array("", bandwidth_integral)
        nr.sem.configuration.configure_offset_limit_fail_mask_array("", limit_fail_mask)
        nr.sem.configuration.configure_offset_absolute_limit_array("", absolute_limit_start, absolute_limit_stop)
        nr.sem.configuration.configure_offset_relative_limit_array("", relative_limit_start, relative_limit_stop)

        if link_direction == nirfmxnr.LinkDirection.UPLINK:
            nr.sem.configuration.configure_uplink_mask_type("", uplink_mask_type)
        else:
            nr.configure_gnodeb_category("", gnodeb_category)
            nr.set_band("", band)
            nr.sem.configuration.set_downlink_mask_type("", downlink_mask_type)
            nr.sem.configuration.set_delta_f_maximum("", delta_f_maximum)
            nr.sem.configuration.component_carrier.configure_rated_output_power_array("", component_carrier_rated_output_power)

        nr.sem.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        nr.sem.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        nr.initiate("", "")

        # Retrieve results
        (
            upper_offset_measurement_status,
            upper_offset_margin,
            upper_offset_margin_frequency,
            upper_offset_margin_absolute_power,
            upper_offset_margin_relative_power,
            error_code,
        ) = nr.sem.results.fetch_upper_offset_margin_array("", timeout)

        (
            lower_offset_measurement_status,
            lower_offset_margin,
            lower_offset_margin_frequency,
            lower_offset_margin_absolute_power,
            lower_offset_margin_relative_power,
            error_code,
        ) = nr.sem.results.fetch_lower_offset_margin_array("", timeout)

        total_aggregated_power, error_code = nr.sem.results.fetch_total_aggregated_power("", timeout)
        measurement_status, error_code = nr.sem.results.fetch_measurement_status("", timeout)

        spectrum = numpy.empty(0, dtype=numpy.float32)
        composite_mask = numpy.empty(0, dtype=numpy.float32)
        nr.sem.results.fetch_spectrum("", timeout, spectrum, composite_mask)

        # Print Results
        print(f"Total Aggregated Power (dBm)    : {total_aggregated_power}")
        print(f"Measurement Status              : {measurement_status.name}")

        print("\n--------  Lower Offset Segement Measurements --------\n")
        for i in range(len(lower_offset_margin)):
            print(f"Offset  {i}")
            print(f"Measurement Status              : {lower_offset_measurement_status[i].name}")
            print(f"Margin (dB)                     : {lower_offset_margin[i]}")
            print(f"Margin Frequency (Hz)           : {lower_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power (dBm)     : {lower_offset_margin_absolute_power[i]}\n")

        print("\n--------  Upper  Offset Segement Measurements --------\n")
        for i in range(len(upper_offset_margin)):
            print(f"Offset  {i}")
            print(f"Measurement Status              : {upper_offset_measurement_status[i].name}")
            print(f"Margin (dB)                     : {upper_offset_margin[i]}")
            print(f"Margin Frequency (Hz)           : {upper_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power (dBm)     : {upper_offset_margin_absolute_power[i]}\n")

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
        description="Pass arguments for SEM Contiguous Multi-Carrier Example",
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
