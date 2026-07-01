r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure Selected Ports.
4. Configure basic signal properties (Center Frequency, RF Attenuation and External Attenuation).
5. Configure Trigger Parameters for IQ Power Edge Trigger.
6. Configure Link Direction, Frequency Range and Carrier Bandwidth and Subcarrier Spacing.
7. Configure Reference Level.
8. Select ACP measurement and enable Traces.
9. Configure Measurement Method.
10. Configure Noise Compensation Parameter.
11. Configure Sweep Time Parameters.
12. Configure Averaging Parameters for ACP measurement.
13. Initiate the Measurement.
14. Fetch ACP Measurements and Traces.
15. Close RFmx Session.
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
    external_attenuation = 0.0  # dB

    auto_level = True
    reference_level = 0.0  # dBm
    measurement_interval = 10.0e-3  # s

    rf_attenuation_auto = nirfmxinstr.RFAttenuationAuto.TRUE
    rf_attenuation = 10.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10.0e6  # Hz

    iq_power_edge_enabled = False
    iq_power_edge_level = -20.0  # dB or dBm
    trigger_delay = 0.0  # s
    minimum_quiet_time_mode = nirfmxnr.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 8.0e-6  # s

    link_direction = nirfmxnr.LinkDirection.UPLINK
    frequency_range = nirfmxnr.FrequencyRange.RANGE1
    carrier_bandwidth = 100e6  # Hz
    subcarrier_spacing = 30e3  # Hz
    measurement_method = nirfmxnr.AcpMeasurementMethod.NORMAL
    noise_compensation_enabled = nirfmxnr.AcpNoiseCompensationEnabled.FALSE

    sweep_time_auto = nirfmxnr.AcpSweepTimeAuto.TRUE
    sweep_time_interval = 1.0e-3  # s

    averaging_enabled = nirfmxnr.AcpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxnr.AcpAveragingType.RMS

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
        nr.configure_frequency("", center_frequency)
        nr.configure_external_attenuation("", external_attenuation)
        instr_session.configure_rf_attenuation("", rf_attenuation_auto, rf_attenuation)
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
        if auto_level:
            reference_level, error_code = nr.auto_level("", measurement_interval)
            print(f"Reference level  (dBm)       : {reference_level}")
        else:
            nr.configure_reference_level("", reference_level)
        nr.select_measurements("", nirfmxnr.MeasurementTypes.ACP, True)
        nr.acp.configuration.configure_measurement_method("", measurement_method)
        nr.acp.configuration.configure_noise_compensation_enabled("", noise_compensation_enabled)
        nr.acp.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        nr.acp.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        nr.initiate("", "")

        # Retrieve results
        (
            lower_relative_power,
            upper_relative_power,
            lower_absolute_power,
            upper_absolute_power,
            error_code,
        ) = nr.acp.results.fetch_offset_measurement_array("", timeout)

        absolute_power, relative_power, error_code = (
            nr.acp.results.component_carrier.fetch_measurement("", timeout)
        )

        for i in range(len(lower_relative_power)):
            relative_powers_trace = numpy.empty(0, dtype=numpy.float32)
            nr.acp.results.fetch_relative_powers_trace("", timeout, i, relative_powers_trace)

        spectrum = numpy.empty(0, dtype=numpy.float32)
        nr.acp.results.fetch_spectrum("", timeout, spectrum)

        # Print Results
        print(f"\nCarrier Absolute Power (dBm or dBm/Hz) : {absolute_power}")

        print("\n-----------Offset Channel Measurements----------- \n")
        for i in range(len(lower_relative_power)):
            print(f"Offset  {i}")
            print(f"Lower Relative Power (dB)              : {lower_relative_power[i]}")
            print(f"Upper Relative Power (dB)              : {upper_relative_power[i]}")
            print(f"Lower Absolute Power (dBm or dBm/Hz)   : {lower_absolute_power[i]}")
            print(f"Upper Absolute Power (dBm or dBm/Hz)   : {upper_absolute_power[i]}")
            print("-------------------------------------------------\n")

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
        description="Pass arguments for ACP Example",
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
