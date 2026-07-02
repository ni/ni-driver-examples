"""
RFmxPulse Time and Power Basic Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic RF signal properties (Center Frequency, RF Attenuation and External Attenuation).
4. Enable Pulse measurement and Traces Result.
5. Configure Trigger Type and Trigger Parameters.
6. Configure Acquisition Settings.
7. Configure Pulse Detection Settings.
8. Configure State and Threshold Level Settings.
9. Configure Selected Traces Settings; enable Pulse Metrics, disable Pulse Stability.
10. Initiate the Measurement.
11. Wait for Measurement to complete.
12. Fetch Pulse Count, Timing, Amplitude Measurements and Amplitude Trace.
13. Close RFmx Session.
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

    pulse_detection_reference = nirfmxpulse.PulseDetectionReference.REFERENCE_LEVEL
    pulse_detection_threshold = -20.0    # dB
    pulse_detection_hysteresis = 1.0     # dB
    pulse_detection_minimum_off_duration = 50.0e-9  # s

    pulse_level_computation_method = nirfmxpulse.PulseLevelComputationMethod.MEDIAN
    pulse_droop_compensation_enabled = nirfmxpulse.PulseDroopCompensationEnabled.TRUE

    pulse_selected_pulse_trace = 0
    pulse_amplitude_trace_unit = nirfmxpulse.PulseAmplitudeTraceUnit.DBM

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

        # Configure Pulse Detection Settings
        pulse.pulse_measurement.configuration.set_detection_reference(
            "", pulse_detection_reference
        )
        pulse.pulse_measurement.configuration.set_detection_threshold(
            "", pulse_detection_threshold
        )
        pulse.pulse_measurement.configuration.set_detection_hysteresis(
            "", pulse_detection_hysteresis
        )
        pulse.pulse_measurement.configuration.set_detection_minimum_off_duration(
            "", pulse_detection_minimum_off_duration
        )

        # Configure Level Computation and Droop
        pulse.pulse_measurement.configuration.set_level_computation_method(
            "", pulse_level_computation_method
        )
        pulse.pulse_measurement.configuration.set_droop_compensation_enabled(
            "", pulse_droop_compensation_enabled
        )

        # Enable Metrics, disable Stability; configure trace settings
        pulse.pulse_measurement.configuration.set_metrics_enabled(
            "", nirfmxpulse.PulseMetricsEnabled.TRUE
        )
        pulse.pulse_measurement.configuration.set_stability_enabled(
            "", nirfmxpulse.PulseStabilityEnabled.FALSE
        )
        pulse.pulse_measurement.configuration.set_selected_pulse_trace(
            "", pulse_selected_pulse_trace
        )
        pulse.pulse_measurement.configuration.set_amplitude_trace_unit(
            "", pulse_amplitude_trace_unit
        )

        pulse.initiate("", "")
        pulse.wait_for_measurement_complete("", timeout)

        pulse_count, _ = pulse.pulse_measurement.results.get_pulse_count("")

        rise_time, _ = pulse.pulse_measurement.results.get_rise_time("")                                      # s
        fall_time, _ = pulse.pulse_measurement.results.get_fall_time("")                                      # s
        pulse_width, _ = pulse.pulse_measurement.results.get_pulse_width("")                                  # s
        pulse_repetition_interval, _ = pulse.pulse_measurement.results.get_pulse_repetition_interval("")     # s

        top_level, _ = pulse.pulse_measurement.results.get_top_level("")                                     # dBm
        base_level, _ = pulse.pulse_measurement.results.get_base_level("")                                   # dBm
        average_on_level, _ = pulse.pulse_measurement.results.get_average_on_level("")                       # dBm
        overshoot, _ = pulse.pulse_measurement.results.get_overshoot("")                                     # %
        droop, _ = pulse.pulse_measurement.results.get_droop("")                                             # %
        ripple, _ = pulse.pulse_measurement.results.get_ripple("")                                           # %

        amplitude = numpy.empty(0, dtype=numpy.float32)
        _ = pulse.pulse_measurement.results.fetch_amplitude_trace("", timeout, amplitude)

        # Print Results
        print(f"\nPulse Count                            : {pulse_count}\n")
        print("\n------------------- Timing Results ----------------------\n")
        for i in range(len(rise_time)):
            print(f"Index                                  : {i}")
            print(f"Rise Time (dB)                         : {rise_time[i]:.11f}")
            print(f"Fall Time (dB)                         : {fall_time[i]:.11f}")
            print(f"Pulse Width (dB)                       : {pulse_width[i]:.9f}")
            print(f"Pulse Repetition Interval (dB)         : {pulse_repetition_interval[i]:.9f}")
            print("-------------------------------------------------------\n")

        print("\n\n------------------ Level Results ------------------------\n")
        for i in range(len(top_level)):
            print(f"Index                                  : {i}")
            print(f"Top Level (dBm)                        : {top_level[i]}")
            print(f"Base Level (dBm)                       : {base_level[i]}")
            print(f"Average On Level (dBm)                 : {average_on_level[i]}")
            print(f"Overshoot (%)                          : {overshoot[i]}")
            print(f"Droop (%)                              : {droop[i]}")
            print(f"Ripple (%)                             : {ripple[i]}")
            print("---------------------------------------------------------\n")

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
        description="Pass arguments for RFmxPulse Time and Power Basic Example",
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
