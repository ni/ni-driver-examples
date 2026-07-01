"""
RFmx LTE SEM Non-Contiguous Multi-Carrier Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5[A-F]. Configure Subblock Parameters.
   5A. Configure Number of Subblocks.
   5B. Configure subblock Frequency.
   5C. Configure Component Carrier Spacing.
   5D. Configure Number of Component Carriers.
   5E. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
   5F. Configure Component Carrier Maximum Output Power for Downlink Link Direction.
6. Configure Link Direction.
7. Select SEM measurement and enable Traces.
8. Configure Sweep Time Parameters.
9. Configure Averaging Parameters for SEM measurement.
10. Configure Uplink Mask Type, or Downlink Mask, eNodeB Category.
11. Initiate the Measurement.
12. Fetch SEM Measurements and Traces.
13. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxlte
import numpy


def example(resource_name, option_string):
    """LTE SEM non-contiguous multi-carrier measurement example."""
    # Configuration parameters
    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10.0e6  # Hz

    center_frequency = 1.95e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    enable_trigger = False
    digital_edge_source = "PFI0"
    digital_edge = nirfmxlte.DigitalEdgeTriggerEdge.RISING_EDGE
    trigger_delay = 0.0  # s

    link_direction = nirfmxlte.LinkDirection.UPLINK

    number_of_subblocks = 2
    number_of_component_carriers = 1

    subblock_frequency = [0.0, 30e6]  # Hz
    subblock_component_carrier_spacing_type = [
        nirfmxlte.ComponentCarrierSpacingType.NOMINAL,
        nirfmxlte.ComponentCarrierSpacingType.NOMINAL,
    ]
    subblock_component_carrier_at_center_frequency = [-1, -1]
    subblock_component_carrier_bandwidth = [[20e6], [20e6]]  # Hz
    subblock_component_carrier_frequency = [[0.0], [0.0]]  # Hz

    uplink_mask_type = nirfmxlte.SemUplinkMaskType.GENERAL_NS_01

    enodeb_category = nirfmxlte.eNodeBCategory.WIDE_AREA_BASE_STATION_CATEGORY_A
    downlink_mask_type = nirfmxlte.SemDownlinkMaskType.ENODEB_CATEGORY_BASED
    delta_f_maximum = 15.0e6  # Hz
    aggregated_maximum_power = 0.0  # dBm
    subblock_component_carrier_maximum_output_power = [[0.0], [0.0]]  # dBm

    sidelink_mask_type = nirfmxlte.SemSidelinkMaskType.GENERAL_NS_01

    sweep_time_auto = nirfmxlte.SemSweepTimeAuto.TRUE
    sweep_time_interval = 0.001  # s

    averaging_enabled = nirfmxlte.SemAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxlte.SemAveragingType.RMS

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

        lte_signal.configure_number_of_subblocks("", number_of_subblocks)

        for i in range(number_of_subblocks):
            subblock_string = nirfmxlte.Lte.build_subblock_string("", i)
            lte_signal.set_subblock_frequency(subblock_string, subblock_frequency[i])
            lte_signal.component_carrier.configure_spacing(
                subblock_string,
                subblock_component_carrier_spacing_type[i],
                subblock_component_carrier_at_center_frequency[i],
            )
            lte_signal.configure_number_of_component_carriers(
                subblock_string, number_of_component_carriers
            )
            lte_signal.component_carrier.configure_array(
                subblock_string,
                subblock_component_carrier_bandwidth[i],
                subblock_component_carrier_frequency[i],
                None,
            )
            if link_direction == nirfmxlte.LinkDirection.DOWNLINK:
                lte_signal.sem.configuration.component_carrier.configure_maximum_output_power_array(
                    subblock_string, subblock_component_carrier_maximum_output_power[i]
                )

        lte_signal.configure_link_direction("", link_direction)

        lte_signal.select_measurements("", nirfmxlte.MeasurementTypes.SEM, True)

        lte_signal.sem.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        lte_signal.sem.configuration.configure_averaging(
            "", averaging_enabled, averaging_count, averaging_type
        )

        if link_direction == nirfmxlte.LinkDirection.UPLINK:
            lte_signal.sem.configuration.configure_uplink_mask_type("", uplink_mask_type)
        elif link_direction == nirfmxlte.LinkDirection.DOWNLINK:
            lte_signal.configure_enodeb_category("", enodeb_category)
            lte_signal.sem.configuration.configure_downlink_mask(
                "", downlink_mask_type, delta_f_maximum, aggregated_maximum_power
            )
        else:
            lte_signal.sem.configuration.set_sidelink_mask_type("", sidelink_mask_type)

        lte_signal.initiate("", "")

        # Per-subblock results
        subblock_results = []
        for i in range(number_of_subblocks):
            subblock_string = nirfmxlte.Lte.build_subblock_string("", i)
            subblock_power, integration_bandwidth, frequency, error_code = (
                lte_signal.sem.results.fetch_subblock_measurement(subblock_string, timeout)
            )
            (
                upper_offset_measurement_status,
                upper_offset_margin,
                upper_offset_margin_frequency,
                upper_offset_margin_absolute_power,
                upper_offset_margin_relative_power,
                error_code,
            ) = lte_signal.sem.results.fetch_upper_offset_margin_array(subblock_string, timeout)
            (
                lower_offset_measurement_status,
                lower_offset_margin,
                lower_offset_margin_frequency,
                lower_offset_margin_absolute_power,
                lower_offset_margin_relative_power,
                error_code,
            ) = lte_signal.sem.results.fetch_lower_offset_margin_array(subblock_string, timeout)
            subblock_results.append(
                {
                    "subblock_power": subblock_power,
                    "integration_bandwidth": integration_bandwidth,
                    "frequency": frequency,
                    "upper_offset_measurement_status": upper_offset_measurement_status,
                    "upper_offset_margin": upper_offset_margin,
                    "upper_offset_margin_frequency": upper_offset_margin_frequency,
                    "upper_offset_margin_absolute_power": upper_offset_margin_absolute_power,
                    "lower_offset_measurement_status": lower_offset_measurement_status,
                    "lower_offset_margin": lower_offset_margin,
                    "lower_offset_margin_frequency": lower_offset_margin_frequency,
                    "lower_offset_margin_absolute_power": lower_offset_margin_absolute_power,
                }
            )

        total_aggregated_power, error_code = lte_signal.sem.results.fetch_total_aggregated_power(
            "", timeout
        )

        measurement_status, error_code = lte_signal.sem.results.fetch_measurement_status(
            "", timeout
        )

        spectrum = numpy.empty(0, dtype=numpy.float32)
        absolute_mask = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = lte_signal.sem.results.fetch_spectrum("", timeout, spectrum, absolute_mask)

        print(f"Total Aggregated Power (dBm)    :{total_aggregated_power}")
        print(f"Measurement Status              :{measurement_status.name}")
        print("--------------------Subblock Measurements--------------------")

        for i, res in enumerate(subblock_results):
            print(f"\nSubblock {i}\n")
            print(f"Subblock Power (dBm)            :{res['subblock_power']}")
            print(f"Integration Bandwidth (Hz)      :{res['integration_bandwidth']}")
            print(f"Frequency (Hz)                  :{res['frequency']}")

            print("\nOffset Segment Measurements \n")
            for j in range(len(res["lower_offset_margin"])):
                print(f"\nLower Offset Segement Measurement {j}")
                print(
                    f"Measurement Status              :{res['lower_offset_measurement_status'][j].name}"
                )
                print(f"Margin (dB)                     :{res['lower_offset_margin'][j]}")
                print(f"Margin Frequency (Hz)           :{res['lower_offset_margin_frequency'][j]}")
                print(
                    f"Margin Absolute Power (dBm)     :{res['lower_offset_margin_absolute_power'][j]}"
                )

                print(f"\nUpper Offset Segement Measurement {j}")
                print(
                    f"Measurement Status              :{res['upper_offset_measurement_status'][j].name}"
                )
                print(f"Margin (dB)                     :{res['upper_offset_margin'][j]}")
                print(f"Margin Frequency (Hz)           :{res['upper_offset_margin_frequency'][j]}")
                print(
                    f"Margin Absolute Power (dBm)     :{res['upper_offset_margin_absolute_power'][j]}"
                )


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
        description="Pass arguments for LTE SEM Non-Contiguous Multi-Carrier Example",
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
