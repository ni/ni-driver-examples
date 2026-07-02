r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure Selected Ports.
4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure Trigger Parameters for Digital Edge Trigger.
6. Configure Link Direction, Frequency Range, Carrier Bandwidth and Subcarrier Spacing.
7. Select TXP measurement and enable Traces.
8. Configure Measurement Offset & Measurement Length Parameters and Averaging Parameters for TXP measurement.
9. Initiate the Measurement.
10. Fetch TXP Measurements and Traces.
11. Close RFmx Session.
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

    measurement_offset = 0.0  # s
    measurement_length = 1.0e-3  # s

    averaging_enabled = nirfmxnr.TxpAveragingEnabled.TRUE
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
        nr.component_carrier.set_bandwidth("", carrier_bandwidth)
        nr.component_carrier.set_bandwidth_part_subcarrier_spacing("", subcarrier_spacing)
        nr.select_measurements("", nirfmxnr.MeasurementTypes.TXP, True)
        nr.txp.configuration.set_measurement_interval("", measurement_length)
        nr.txp.configuration.set_measurement_offset("", measurement_offset)
        nr.txp.configuration.set_averaging_enabled("", averaging_enabled)
        nr.txp.configuration.set_averaging_count("", averaging_count)
        nr.initiate("", "")

        # Retrieve results
        average_power_mean, peak_power_maximum, error_code = nr.txp.results.fetch_measurement("", timeout)

        power = numpy.empty(0, dtype=numpy.float32)
        nr.txp.results.fetch_power_trace("", timeout, power)

        # Print Results
        print("\n-------------Measurement------------\n")
        print(f"Average Power Mean (dBm)      : {average_power_mean}")
        print(f"Peak Power Maximum (dBm)      : {peak_power_maximum}\n")

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
        description="Pass arguments for TXP Single Carrier Example",
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
