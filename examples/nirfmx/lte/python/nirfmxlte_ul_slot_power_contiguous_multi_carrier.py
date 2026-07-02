"""
RFmx LTE UL Slot Power Contiguous Multi-Carrier Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Component Carrier Spacing.
6. Configure Component Carriers.
7. Configure Duplex Scheme.
8. Select SlotPower measurement and enable Traces.
9. Configure Measurement Interval.
10. Initiate the Measurement.
11. Fetch SlotPower Measurements and Traces.
12. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxlte
import numpy


def example(resource_name, option_string):
    """LTE UL slot power contiguous multi-carrier measurement example."""
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

    number_of_component_carriers = 2
    component_carrier_spacing_type = nirfmxlte.ComponentCarrierSpacingType.NOMINAL
    component_carrier_at_center_frequency = -1
    component_carrier_bandwidth = [20e6, 20e6]  # Hz
    component_carrier_frequency = [-9.9e6, 9.9e6]  # Hz
    cell_id = [0, 1]

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

        lte_signal.component_carrier.configure_spacing(
            "", component_carrier_spacing_type, component_carrier_at_center_frequency
        )

        lte_signal.configure_number_of_component_carriers("", number_of_component_carriers)

        lte_signal.component_carrier.configure_array(
            "", component_carrier_bandwidth, component_carrier_frequency, cell_id
        )

        lte_signal.configure_duplex_scheme("", duplex_scheme, uplink_downlink_configuration)

        lte_signal.select_measurements("", nirfmxlte.MeasurementTypes.SLOTPOWER, True)

        lte_signal.slotpower.configuration.configure_measurement_interval(
            "", measurement_offset, measurement_length
        )

        lte_signal.initiate("", "")

        for i in range(number_of_component_carriers):
            carrier_string = nirfmxlte.Lte.build_carrier_string("", i)
            subframe_power, subframe_power_delta, error_code = (
                lte_signal.slotpower.results.fetch_powers(carrier_string, timeout)
            )
            print(f"\n\nCC {i} Trace")
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
        description="Pass arguments for LTE UL Slot Power Contiguous Multi-Carrier Example",
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
