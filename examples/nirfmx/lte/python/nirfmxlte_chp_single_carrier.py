"""
RFmx LTE CHP Single Carrier Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Carrier Bandwidth.
6. Select CHP measurement and enable Traces.
7. Configure Sweep Time Parameters.
8. Configure Averaging Parameters for CHP measurement.
9. Initiate the Measurement.
10. Fetch CHP Measurements and Traces.
11. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxlte
import numpy


def example(resource_name, option_string):
    """LTE CHP measurement example."""
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

    component_carrier_bandwidth = 200e3  # Hz
    component_carrier_frequency = 0.0  # Hz
    cell_id = 0

    sweep_time_auto = nirfmxlte.ChpSweepTimeAuto.TRUE
    sweep_time_interval = 0.001  # s

    averaging_enabled = nirfmxlte.ChpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxlte.ChpAveragingType.RMS

    timeout = 10.0  # seconds

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

        lte_signal.select_measurements("", nirfmxlte.MeasurementTypes.CHP, True)

        lte_signal.chp.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        lte_signal.chp.configuration.configure_averaging(
            "", averaging_enabled, averaging_count, averaging_type
        )

        lte_signal.initiate("", "")

        absolute_power, relative_power, error_code = (
            lte_signal.chp.results.component_carrier.fetch_measurement("", timeout)
        )

        spectrum = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = lte_signal.chp.results.fetch_spectrum("", timeout, spectrum)

        # Print results
        print(f"Carrier Absolute Power (dBm)         :{absolute_power}")

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
        description="Pass arguments for LTE CHP Example",
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
