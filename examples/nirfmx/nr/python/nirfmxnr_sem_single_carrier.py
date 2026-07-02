r"""Steps:
1. Open a new RFmx session.
2. Configure Frequency Reference.
3. Configure Selected Ports.
4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure Trigger Parameters for IQ Power Edge Trigger.
6. Configure Link Direction, Frequency Range, Carrier Bandwidth and BWP Subcarrier Spacing.
7. Select SEM measurement and enable Traces.
8. Configure Uplink Mask Type, or Downlink Mask, Band, gNodeB Category, Delta F_Max(Hz) and
   Component Carrier Rated Output Power based on Link Direction.
9. Configure Sweep Time Parameters.
10. Configure Averaging Parameters for SEM measurement.
11. Initiate the Measurement.
12. Fetch SEM Measurements and Traces.
13. Close RFmx Session.
"""

import argparse
import sys

import nirfmxnr
import numpy

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    # Initialize input variables
    selected_ports = ""
    center_frequency = 3.5e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    iq_power_edge_enabled = False
    iq_power_edge_level = -20.0  # dB or dBm
    trigger_delay = 0.0  # s
    minimum_quiet_time_mode = nirfmxnr.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 8.0e-6  # s

    link_direction = nirfmxnr.LinkDirection.UPLINK

    frequency_range = nirfmxnr.FrequencyRange.RANGE1

    uplink_mask_type = nirfmxnr.SemUplinkMaskType.GENERAL

    gnodeb_category = nirfmxnr.gNodeBCategory.WIDE_AREA_BASE_STATION_CATEGORY_A
    downlink_mask_type = nirfmxnr.SemDownlinkMaskType.STANDARD
    delta_f_maximum = 15.0e6  # Hz
    component_carrier_rated_output_power = 0.0  # dBm
    band = 78

    carrier_bandwidth = 100e6  # Hz
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
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get NR signal
        nr = instr_session.get_nr_signal_configuration()

        # Configure measurement
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
            iq_power_edge_enabled,
        )

        nr.set_link_direction("", link_direction)
        nr.set_frequency_range("", frequency_range)
        nr.component_carrier.set_bandwidth("", carrier_bandwidth)
        nr.component_carrier.set_bandwidth_part_subcarrier_spacing("", subcarrier_spacing)

        nr.select_measurements("", nirfmxnr.MeasurementTypes.SEM, True)

        if link_direction == nirfmxnr.LinkDirection.UPLINK:
            nr.sem.configuration.configure_uplink_mask_type("", uplink_mask_type)
        else:
            nr.configure_gnodeb_category("", gnodeb_category)
            nr.set_band("", band)
            nr.sem.configuration.set_downlink_mask_type("", downlink_mask_type)
            nr.sem.configuration.set_delta_f_maximum("", delta_f_maximum)
            nr.sem.configuration.component_carrier.configure_rated_output_power("", component_carrier_rated_output_power)

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

        absolute_power, peak_absolute_power, peak_frequency, relative_power, error_code = (
            nr.sem.results.component_carrier.fetch_measurement("", timeout)
        )

        measurement_status, error_code = nr.sem.results.fetch_measurement_status("", timeout)

        spectrum = numpy.empty(0, dtype=numpy.float32)
        composite_mask = numpy.empty(0, dtype=numpy.float32)
        nr.sem.results.fetch_spectrum("", timeout, spectrum, composite_mask)

        # Print results
        print(f"Measurement Status                       : {measurement_status.name}")
        print(f"Carrier Absolute Integrated Power (dBm)  : {absolute_power}")

        print("\n----------Lower Offset Segment Measurements----------\n")
        for i in range(len(lower_offset_margin)):
            print(f"Offset  {i}")
            print(f"Measurement Status                       : {lower_offset_measurement_status[i].name}")
            print(f"Margin (dB)                              : {lower_offset_margin[i]}")
            print(f"Margin Frequency (Hz)                    : {lower_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power (dBm)              : {lower_offset_margin_absolute_power[i]}\n")

        print("\n----------Upper Offset Segment Measurements----------\n")
        for i in range(len(upper_offset_margin)):
            print(f"Offset  {i}")
            print(f"Measurement Status                       : {upper_offset_measurement_status[i].name}")
            print(f"Margin (dB)                              : {upper_offset_margin[i]}")
            print(f"Margin Frequency (Hz)                    : {upper_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power (dBm)              : {upper_offset_margin_absolute_power[i]}\n")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close Session
        if nr is not None:
            nr.dispose()
            nr = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for SEM Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr."
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
        "--option-string",
        "",
    ]
    _main(cmd_line)


def test_example():
    """Call example function."""
    options = {}
    example("RFSA", options)


if __name__ == "__main__":
    main()
