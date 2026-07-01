"""
RFmxPulse Stability Basic Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure LO Leakage Avoidance Enabled, LO source and Downconverter Frequency Offset (Hz).
4. Configure basic RF signal properties (Center Frequency, RF Attenuation and External Attenuation).
5. Enable Pulse measurement and Traces Result.
6. Configure Trigger Type and Trigger Parameters.
7. Configure Acquisition Settings.
8. Configure Measurement Point Settings.
9. Configure Stability Settings.
10. Configure Selected Traces setting and enable Pulse Stability, disable Pulse Metrics.
11. Initiate the Measurement.
12. Wait for Measurement to complete.
13. Fetch Average Stability, Per Pulse Stability Measurements, Stability and Pulse to Pulse Stability Traces.
14. Close RFmx Session.

"""

import argparse
import sys

import numpy
import nirfmxinstr
import nirfmxpulse


def example(resource_name, option_string):
    """Run Example."""
    # Initialize input variables
    center_frequency = 1.0e9              # Hz
    reference_level = -10.0              # dBm
    external_attenuation = 0.0           # dB

    down_converter_frequency_offset = 0.0  # Hz

    lo_leakage_avoidance_enabled = nirfmxinstr.LOLeakageAvoidanceEnabled.TRUE
    lo_source = "Onboard"

    frequency_settling_units = nirfmxinstr.FrequencySettlingUnits.PPM
    frequency_settling = 1.0e-1          # ppm

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10.0e6  # Hz

    iq_power_edge_enabled = True
    iq_power_edge_level = -20.0          # dBm
    trigger_delay = 0.0                  # s
    minimum_quiet_time_mode = nirfmxpulse.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 5.0e-6          # s

    measurement_bandwidth = 80.0e6       # Hz
    measurement_filter_type = nirfmxpulse.MeasurementFilterType.GAUSSIAN
    acquisition_length = 1.0e-3          # s
    maximum_pulse_count_enabled = nirfmxpulse.MaximumPulseCountEnabled.FALSE
    maximum_pulse_count = 100

    pulse_measurement_point_reference = nirfmxpulse.PulseMeasurementPointReference.CENTER
    pulse_measurement_point_offset = 0.0            # s
    pulse_measurement_point_averaging_duration = 0.0  # s

    pulse_stability_measurement_offset = 0
    pulse_stability_reference_offset = 0
    pulse_stability_pulse_to_pulse_offset = 1
    pulse_stability_frequency_error_compensation = nirfmxpulse.PulseStabilityFrequencyErrorCompensation.ON

    pulse_selected_pulse_trace = 0

    timeout = 10.0                       # s

    instr_session = None
    pulse = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get Pulse signal configuration
        pulse = instr_session.get_pulse_signal_configuration()

        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )

        instr_session.set_lo_leakage_avoidance_enabled("", lo_leakage_avoidance_enabled)
        instr_session.set_lo_source("", lo_source)
        instr_session.set_downconverter_frequency_offset("", down_converter_frequency_offset)
        instr_session.set_frequency_settling_units("", frequency_settling_units)
        instr_session.set_frequency_settling("", frequency_settling)

        pulse.configure_rf("", center_frequency, reference_level, external_attenuation)

        pulse.select_measurements("", nirfmxpulse.MeasurementTypes.PULSE, True)

        pulse.configure_iq_power_edge_trigger(
            "",
            "0",
            nirfmxpulse.IQPowerEdgeTriggerSlope.RISING,
            iq_power_edge_level,
            trigger_delay,
            minimum_quiet_time_mode,
            minimum_quiet_time,
            nirfmxpulse.IQPowerEdgeTriggerLevelType.RELATIVE,
            iq_power_edge_enabled,
        )

        pulse.set_measurement_bandwidth("", measurement_bandwidth)
        pulse.set_measurement_filter_type("", measurement_filter_type)
        pulse.set_acquisition_length("", acquisition_length)
        pulse.set_maximum_pulse_count_enabled("", maximum_pulse_count_enabled)
        pulse.set_maximum_pulse_count("", maximum_pulse_count)

        pulse.pulse_measurement.configuration.set_measurement_point_reference(
            "", pulse_measurement_point_reference
        )
        pulse.pulse_measurement.configuration.set_measurement_point_offset(
            "", pulse_measurement_point_offset
        )
        pulse.pulse_measurement.configuration.set_measurement_point_averaging_duration(
            "", pulse_measurement_point_averaging_duration
        )

        # Configure Stability Settings
        pulse.pulse_measurement.configuration.set_stability_measurement_offset(
            "", pulse_stability_measurement_offset
        )
        pulse.pulse_measurement.configuration.set_stability_reference_offset(
            "", pulse_stability_reference_offset
        )
        pulse.pulse_measurement.configuration.set_stability_pulse_to_pulse_offset(
            "", pulse_stability_pulse_to_pulse_offset
        )
        pulse.pulse_measurement.configuration.set_stability_frequency_error_compensation(
            "", pulse_stability_frequency_error_compensation
        )
        pulse.pulse_measurement.configuration.set_selected_pulse_trace(
            "", pulse_selected_pulse_trace
        )

        # Enable Stability, disable Metrics
        pulse.pulse_measurement.configuration.set_metrics_enabled(
            "", nirfmxpulse.PulseMetricsEnabled.FALSE
        )
        pulse.pulse_measurement.configuration.set_stability_enabled(
            "", nirfmxpulse.PulseStabilityEnabled.TRUE
        )

        pulse.initiate("", "")
        pulse.wait_for_measurement_complete("", timeout)

        average_amplitude_stability, _ = pulse.pulse_measurement.results.get_average_amplitude_stability("")  # dB
        average_phase_stability, _ = pulse.pulse_measurement.results.get_average_phase_stability("")        # dB
        average_total_stability, _ = pulse.pulse_measurement.results.get_average_total_stability("")        # dB

        amplitude_stability, _ = pulse.pulse_measurement.results.get_amplitude_stability("")  # dB
        phase_stability, _ = pulse.pulse_measurement.results.get_phase_stability("")          # dB
        total_stability, _ = pulse.pulse_measurement.results.get_total_stability("")          # dB

        pulse_amplitude_stability = numpy.empty(0, dtype=numpy.float32)
        pulse_phase_stability = numpy.empty(0, dtype=numpy.float32)
        pulse_total_stability = numpy.empty(0, dtype=numpy.float32)
        _ = pulse.pulse_measurement.results.fetch_stability_trace(
            "", timeout,
            pulse_amplitude_stability,
            pulse_phase_stability,
            pulse_total_stability,
        )

        (
            pulse_index,
            pulse_to_pulse_amplitude_stability,
            pulse_to_pulse_phase_stability,
            pulse_to_pulse_total_stability,
            _,
        ) = pulse.pulse_measurement.results.fetch_pulse_to_pulse_stability_trace("", timeout)

        # Print Results
        print("\n----------------- Average Stability Results ---------------\n")
        print(f"Average Amplitude Stability (dB)     : {average_amplitude_stability}")
        print(f"Average Phase Stability (dB)         : {average_phase_stability}")
        print(f"Average Total Stability (dB)         : {average_total_stability}")

        print("\n\n----------------- Stability Results ----------------------\n")
        for i in range(len(amplitude_stability)):
            print(f"Index                                : {i}")
            print(f"Amplitude Stability (dB)             : {amplitude_stability[i]}")
            print(f"Phase Stability (dB)                 : {phase_stability[i]}")
            print(f"Total Stability (dB)                 : {total_stability[i]}")
            print("-----------------------------------------------------------\n")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close Session
        if pulse is not None:
            pulse.dispose()
            pulse = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for RFmxPulse Stability Basic Example",
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
    example("RFSA", "")


if __name__ == "__main__":
    main()
