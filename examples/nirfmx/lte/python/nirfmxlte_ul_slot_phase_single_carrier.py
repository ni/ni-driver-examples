"""
RFmx LTE UL Slot Phase Single Carrier Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Carrier Bandwidth.
6. Configure Duplex Scheme.
7. Select SlotPhase measurement and enable Traces.
8. Configure Synchronization Mode and Interval.
9. Initiate the Measurement.
10. Fetch SlotPhase Traces and Measurements.
11. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxlte
import numpy


def example(resource_name, option_string):
    """LTE UL slot phase single carrier measurement example."""
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
    component_carrier_bandwidth = 10e6  # Hz
    component_carrier_frequency = 0.0  # Hz
    cell_id = 0

    number_of_slots = 20
    synchronization_mode = nirfmxlte.SlotPhaseSynchronizationMode.SLOT
    measurement_offset = 0  # slots
    measurement_length = number_of_slots  # slots

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

        lte_signal.select_measurements("", nirfmxlte.MeasurementTypes.SLOTPHASE, True)

        lte_signal.configure_duplex_scheme("", duplex_scheme, uplink_downlink_configuration)

        lte_signal.slotphase.configuration.configure_synchronization_mode_and_interval(
            "", synchronization_mode, measurement_offset, measurement_length
        )

        lte_signal.initiate("", "")

        slot_phase_discontinuity, error_code = lte_signal.slotphase.results.fetch_phase_discontinuities(
            "", timeout
        )

        maximum_phase_discontinuity, error_code = (
            lte_signal.slotphase.results.fetch_maximum_phase_discontinuity("", timeout)
        )

        sample_phase_error = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = lte_signal.slotphase.results.fetch_sample_phase_error(
            "", timeout, sample_phase_error
        )

        sample_phase_error_linear_fit = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = lte_signal.slotphase.results.fetch_sample_phase_error_linear_fit_trace(
            "", timeout, sample_phase_error_linear_fit
        )

        # Print results
        print(f"Maximum  Phase Discontinuity (deg) : {maximum_phase_discontinuity}")
        for i in range(measurement_length):
            print(f"\nSlot Number {i}")
            print(f"Slot Phase Discontinuities (deg)   : {slot_phase_discontinuity[i]}")

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
        description="Pass arguments for LTE UL Slot Phase Single Carrier Example",
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
