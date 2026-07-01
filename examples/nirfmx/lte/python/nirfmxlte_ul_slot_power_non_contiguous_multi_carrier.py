"""
RFmx LTE UL Slot Power Non-Contiguous Multi-Carrier Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Subblock Configurations:
   5A. Configure Number of Subblocks.
   5B. Configure subblock Frequency.
   5C. Configure Component Carrier Spacing.
   5D. Configure Number of Component Carriers.
   5E. Configure Component Carriers.
6. Configure Duplex Scheme.
7. Select SlotPower measurement and enable Traces.
8. Configure Measurement Interval.
9. Initiate the Measurement.
10. Fetch SlotPower Measurements and Traces.
11. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxlte
import numpy


def example(resource_name, option_string):
    """LTE UL slot power non-contiguous multi-carrier measurement example."""
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

    number_of_subblocks = 2
    number_of_component_carriers = 1

    # Per-subblock configuration
    subblock_frequency = [0.0, 30e6]  # Hz
    subblock_component_carrier_spacing_type = [
        nirfmxlte.ComponentCarrierSpacingType.NOMINAL,
        nirfmxlte.ComponentCarrierSpacingType.NOMINAL,
    ]
    subblock_component_carrier_at_center_frequency = [-1, -1]
    subblock_component_carrier_bandwidth = [[20e6], [20e6]]  # Hz
    subblock_component_carrier_frequency = [[0.0], [0.0]]  # Hz
    subblock_cell_id = [[0], [1]]

    duplex_scheme = nirfmxlte.DuplexScheme.FDD
    uplink_downlink_configuration = nirfmxlte.UplinkDownlinkConfiguration.CONFIGURATION_0

    measurement_offset = 0  # subframes
    measurement_length = 10  # subframes

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
                subblock_cell_id[i],
            )

        lte_signal.configure_duplex_scheme("", duplex_scheme, uplink_downlink_configuration)

        lte_signal.select_measurements("", nirfmxlte.MeasurementTypes.SLOTPOWER, True)

        lte_signal.slotpower.configuration.configure_measurement_interval(
            "", measurement_offset, measurement_length
        )

        lte_signal.initiate("", "")

        print("********** Subblock Measurements **********")
        for i in range(number_of_subblocks):
            subblock_string = nirfmxlte.Lte.build_subblock_string("", i)
            carrier_string = nirfmxlte.Lte.build_carrier_string(subblock_string, 0)
            subframe_power, subframe_power_delta, error_code = (
                lte_signal.slotpower.results.fetch_powers(carrier_string, timeout)
            )
            print(f"\n\nSubblock {i} CC0 Trace")
            print("Subframe Power(dBm)")
            print(",".join(str(v) for v in subframe_power))
            print("\nSubframe Power Delta(dB)")
            print(",".join(str(v) for v in subframe_power_delta))

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
        description="Pass arguments for LTE UL Slot Power Non-Contiguous Multi-Carrier Example",
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
