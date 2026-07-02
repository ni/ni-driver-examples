"""
RFmxPulse Phase and Frequency Advanced Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure LO Leakage Avoidance Enabled, LO source and Downconverter Frequency Offset (Hz).
4. Configure basic RF signal properties (Center Frequency, RF Attenuation and External Attenuation).
5. Enable Pulse measurement and Traces Result.
6. Configure Trigger Type and Trigger Parameters.
7. Configure Acquisition Settings.
8. Configure Pulse Detection Settings.
9. Configure Level Computation Settings.
10. Configure Measurement Point Settings.
11. Configure Frequency and Phase Deviation Range Settings.
12. Configure Frequency and Phase Modulation Settings.
13. Configure Selected Traces setting, disable Pulse Stability and enable Pulse Metrics.
14. Initiate the Measurement.
15. Wait for Measurement to complete.
16. Fetch Per Pulse Phase and Frequency Results.
17. Fetch Statistical Phase Results, Statistical Frequency Results and Statistical FM Chirp Results.
18. Fetch Phase Wrapped Trace and Frequency Trace.
19. Close RFmx Session.

Prerequisites:
- None

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
    pulse_upper_threshold_level = 90.0   # %
    pulse_width_threshold_level = 50.0   # %
    pulse_lower_threshold_level = 10.0   # %

    pulse_measurement_point_reference = nirfmxpulse.PulseMeasurementPointReference.CENTER
    pulse_measurement_point_offset = 0.0            # s
    pulse_measurement_point_averaging_duration = 0.0  # s

    pulse_freq_phase_deviation_range_reference = nirfmxpulse.PulseFrequencyAndPhaseDeviationRangeReference.CENTER
    pulse_freq_phase_deviation_range_length = 75.0   # %
    pulse_freq_phase_deviation_range_edge_start = 0.0  # %
    pulse_freq_phase_deviation_range_edge_stop = 0.0   # %

    pulse_modulation_type = nirfmxpulse.PulseModulationType.CW
    pulse_cw_frequency_offset_auto = nirfmxpulse.PulseCWFrequencyOffsetAuto.TRUE
    pulse_cw_frequency_offset = 0.0      # Hz

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

        pulse.configure_rf("", center_frequency, reference_level, external_attenuation)

        pulse.select_measurements("", nirfmxpulse.MeasurementTypes.PULSE, False)

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

        # Configure Level Computation Settings
        pulse.pulse_measurement.configuration.set_level_computation_method(
            "", pulse_level_computation_method
        )
        pulse.pulse_measurement.configuration.set_upper_threshold_level(
            "", pulse_upper_threshold_level
        )
        pulse.pulse_measurement.configuration.set_width_threshold_level(
            "", pulse_width_threshold_level
        )
        pulse.pulse_measurement.configuration.set_lower_threshold_level(
            "", pulse_lower_threshold_level
        )

        # Configure Measurement Point Settings
        pulse.pulse_measurement.configuration.set_measurement_point_reference(
            "", pulse_measurement_point_reference
        )
        pulse.pulse_measurement.configuration.set_measurement_point_offset(
            "", pulse_measurement_point_offset
        )
        pulse.pulse_measurement.configuration.set_measurement_point_averaging_duration(
            "", pulse_measurement_point_averaging_duration
        )

        # Configure Frequency and Phase Deviation Range Settings
        pulse.pulse_measurement.configuration.set_frequency_and_phase_deviation_range_reference(
            "", pulse_freq_phase_deviation_range_reference
        )
        pulse.pulse_measurement.configuration.set_frequency_and_phase_deviation_range_length(
            "", pulse_freq_phase_deviation_range_length
        )
        pulse.pulse_measurement.configuration.set_frequency_and_phase_deviation_range_edge_start(
            "", pulse_freq_phase_deviation_range_edge_start
        )
        pulse.pulse_measurement.configuration.set_frequency_and_phase_deviation_range_edge_stop(
            "", pulse_freq_phase_deviation_range_edge_stop
        )

        # Configure Frequency and Phase Modulation Settings
        pulse.pulse_measurement.configuration.set_frequency_and_phase_modulation_type(
            "", pulse_modulation_type
        )
        pulse.pulse_measurement.configuration.set_frequency_and_phase_cw_frequency_offset_auto(
            "", pulse_cw_frequency_offset_auto
        )
        pulse.pulse_measurement.configuration.set_frequency_and_phase_cw_frequency_offset(
            "", pulse_cw_frequency_offset
        )

        # Enable Metrics, disable Stability, enable All Traces
        pulse.pulse_measurement.configuration.set_metrics_enabled(
            "", nirfmxpulse.PulseMetricsEnabled.TRUE
        )
        pulse.pulse_measurement.configuration.set_stability_enabled(
            "", nirfmxpulse.PulseStabilityEnabled.FALSE
        )
        pulse.pulse_measurement.configuration.set_all_traces_enabled("", True)

        pulse.initiate("", "")
        pulse.wait_for_measurement_complete("", timeout)

        # Per-pulse Phase Results
        average_phase, _ = pulse.pulse_measurement.results.get_average_phase("")          # deg
        phase_deviation, _ = pulse.pulse_measurement.results.get_phase_deviation("")      # deg
        phase_error_rms, _ = pulse.pulse_measurement.results.get_phase_error_rms("")      # deg

        # Per-pulse Frequency Results
        average_frequency, _ = pulse.pulse_measurement.results.get_average_frequency("")  # Hz
        frequency_deviation, _ = pulse.pulse_measurement.results.get_frequency_deviation("")  # Hz
        frequency_error_rms, _ = pulse.pulse_measurement.results.get_frequency_error_rms("")  # Hz

        # Per-pulse FM Chirp Results
        fm_chirp_rate, _ = pulse.pulse_measurement.results.get_fm_chirp_rate("")          # Hz/us
        fm_chirp_rate2, _ = pulse.pulse_measurement.results.get_fm_chirp_rate2("")        # Hz/us

        # Statistical Phase Results
        average_phase_mean, _ = pulse.pulse_measurement.results.get_average_phase_mean("")
        average_phase_maximum, _ = pulse.pulse_measurement.results.get_average_phase_maximum("")
        average_phase_minimum, _ = pulse.pulse_measurement.results.get_average_phase_minimum("")
        average_phase_sd, _ = pulse.pulse_measurement.results.get_average_phase_standard_deviation("")
        phase_deviation_mean, _ = pulse.pulse_measurement.results.get_phase_deviation_mean("")
        phase_deviation_maximum, _ = pulse.pulse_measurement.results.get_phase_deviation_maximum("")
        phase_deviation_minimum, _ = pulse.pulse_measurement.results.get_phase_deviation_minimum("")
        phase_deviation_sd, _ = pulse.pulse_measurement.results.get_phase_deviation_standard_deviation("")
        phase_error_rms_mean, _ = pulse.pulse_measurement.results.get_phase_error_rms_mean("")
        phase_error_rms_maximum, _ = pulse.pulse_measurement.results.get_phase_error_rms_maximum("")
        phase_error_rms_minimum, _ = pulse.pulse_measurement.results.get_phase_error_rms_minimum("")
        phase_error_rms_sd, _ = pulse.pulse_measurement.results.get_phase_error_rms_standard_deviation("")

        # Statistical Frequency Results
        average_frequency_mean, _ = pulse.pulse_measurement.results.get_average_frequency_mean("")
        average_frequency_maximum, _ = pulse.pulse_measurement.results.get_average_frequency_maximum("")
        average_frequency_minimum, _ = pulse.pulse_measurement.results.get_average_frequency_minimum("")
        average_frequency_sd, _ = pulse.pulse_measurement.results.get_average_frequency_standard_deviation("")
        frequency_deviation_mean, _ = pulse.pulse_measurement.results.get_frequency_deviation_mean("")
        frequency_deviation_maximum, _ = pulse.pulse_measurement.results.get_frequency_deviation_maximum("")
        frequency_deviation_minimum, _ = pulse.pulse_measurement.results.get_frequency_deviation_minimum("")
        frequency_deviation_sd, _ = pulse.pulse_measurement.results.get_frequency_deviation_standard_deviation("")
        frequency_error_rms_mean, _ = pulse.pulse_measurement.results.get_frequency_error_rms_mean("")
        frequency_error_rms_maximum, _ = pulse.pulse_measurement.results.get_frequency_error_rms_maximum("")
        frequency_error_rms_minimum, _ = pulse.pulse_measurement.results.get_frequency_error_rms_minimum("")
        frequency_error_rms_sd, _ = pulse.pulse_measurement.results.get_frequency_error_rms_standard_deviation("")

        # Statistical FM Chirp Results
        fm_chirp_rate_mean, _ = pulse.pulse_measurement.results.get_fm_chirp_rate_mean("")
        fm_chirp_rate_maximum, _ = pulse.pulse_measurement.results.get_fm_chirp_rate_maximum("")
        fm_chirp_rate_minimum, _ = pulse.pulse_measurement.results.get_fm_chirp_rate_minimum("")
        fm_chirp_rate_sd, _ = pulse.pulse_measurement.results.get_fm_chirp_rate_standard_deviation("")
        fm_chirp_rate2_mean, _ = pulse.pulse_measurement.results.get_fm_chirp_rate2_mean("")
        fm_chirp_rate2_maximum, _ = pulse.pulse_measurement.results.get_fm_chirp_rate2_maximum("")
        fm_chirp_rate2_minimum, _ = pulse.pulse_measurement.results.get_fm_chirp_rate2_minimum("")
        fm_chirp_rate2_sd, _ = pulse.pulse_measurement.results.get_fm_chirp_rate2_standard_deviation("")

        # Fetch Traces
        wrapped_phase = numpy.empty(0, dtype=numpy.float32)
        pulse.pulse_measurement.results.fetch_phase_wrapped_trace("", timeout, wrapped_phase)

        frequency_trace = numpy.empty(0, dtype=numpy.float32)
        pulse.pulse_measurement.results.fetch_frequency_trace("", timeout, frequency_trace)

        # Print Per-Pulse Phase Results
        print("\n----------------- Phase Results ---------------\n")
        for i in range(len(average_phase)):
            print(f"Index                                : {i}")
            print(f"Average Phase (deg)                  : {average_phase[i]}")
            print(f"Phase Deviation (deg)                : {phase_deviation[i]}")
            print(f"Phase Error RMS (deg)                : {phase_error_rms[i]}")
            print("-----------------------------------------------------------\n")

        # Print Per-Pulse Frequency Results
        print("\n----------------- Frequency Results ---------------\n")
        for i in range(len(average_frequency)):
            print(f"Index                                : {i}")
            print(f"Average Frequency (Hz)               : {average_frequency[i]}")
            print(f"Frequency Deviation (Hz)             : {frequency_deviation[i]}")
            print(f"Frequency Error RMS (Hz)             : {frequency_error_rms[i]}")
            print("-----------------------------------------------------------\n")

        # Print Per-Pulse FM Chirp Results
        print("\n----------------- FM Chirp Results ---------------\n")
        for i in range(len(fm_chirp_rate)):
            print(f"Index                                : {i}")
            print(f"Chirp Rate (Hz/us)                   : {fm_chirp_rate[i]}")
            print(f"Chirp Rate2 (Hz/us)                  : {fm_chirp_rate2[i]}")
            print("-----------------------------------------------------------\n")

        # Print Statistical Phase Results
        print("\n\n--------------Statistical Phase Results--------------------\n")
        print(f"Average Phase Mean (deg)                             : {average_phase_mean}")
        print(f"Average Phase Maximum (deg)                          : {average_phase_maximum}")
        print(f"Average Phase Minimum (deg)                          : {average_phase_minimum}")
        print(f"Average Phase Standard Deviation (deg)               : {average_phase_sd}")
        print(f"Phase Deviation Mean (deg)                           : {phase_deviation_mean}")
        print(f"Phase Deviation Maximum (deg)                        : {phase_deviation_maximum}")
        print(f"Phase Deviation Minimum (deg)                        : {phase_deviation_minimum}")
        print(f"Phase Deviation Standard Deviation (deg)             : {phase_deviation_sd}")
        print(f"Phase Error RMS Mean (deg)                           : {phase_error_rms_mean}")
        print(f"Phase Error RMS Maximum (deg)                        : {phase_error_rms_maximum}")
        print(f"Phase Error RMS Minimum (deg)                        : {phase_error_rms_minimum}")
        print(f"Phase Error RMS Standard Deviation (deg)             : {phase_error_rms_sd}")
        print("--------------------------------------------------------------\n")

        # Print Statistical Frequency Results
        print("\n\n--------------Statistical Frequency Results--------------------\n")
        print(f"Average Frequency Mean (Hz)                          : {average_frequency_mean}")
        print(f"Average Frequency Maximum (Hz)                       : {average_frequency_maximum}")
        print(f"Average Frequency Minimum (Hz)                       : {average_frequency_minimum}")
        print(f"Average Frequency Standard Deviation (Hz)            : {average_frequency_sd}")
        print(f"Frequency Deviation Mean (Hz)                        : {frequency_deviation_mean}")
        print(f"Frequency Deviation Maximum (Hz)                     : {frequency_deviation_maximum}")
        print(f"Frequency Deviation Minimum (Hz)                     : {frequency_deviation_minimum}")
        print(f"Frequency Deviation Standard Deviation (Hz)          : {frequency_deviation_sd}")
        print(f"Frequency Error RMS Mean (Hz)                        : {frequency_error_rms_mean}")
        print(f"Frequency Error RMS Maximum (Hz)                     : {frequency_error_rms_maximum}")
        print(f"Frequency Error RMS Minimum (Hz)                     : {frequency_error_rms_minimum}")
        print(f"Frequency Error RMS Standard Deviation (Hz)          : {frequency_error_rms_sd}")
        print("--------------------------------------------------------------\n")

        # Print Statistical FM Chirp Results
        print("\n\n--------------Statistical FM Chirp Results--------------------\n")
        print(f"Chirp Rate Mean (Hz/us)                              : {fm_chirp_rate_mean}")
        print(f"Chirp Rate Maximum (Hz/us)                           : {fm_chirp_rate_maximum}")
        print(f"Chirp Rate Minimum (Hz/us)                           : {fm_chirp_rate_minimum}")
        print(f"Chirp Rate Standard Deviation (Hz/us)                : {fm_chirp_rate_sd}")
        print(f"Chirp Rate 2 Mean (Hz/us)                            : {fm_chirp_rate2_mean}")
        print(f"Chirp Rate 2 Maximum (Hz/us)                         : {fm_chirp_rate2_maximum}")
        print(f"Chirp Rate 2 Minimum (Hz/us)                         : {fm_chirp_rate2_minimum}")
        print(f"Chirp Rate 2 Standard Deviation (Hz/us)              : {fm_chirp_rate2_sd}")
        print("--------------------------------------------------------------\n")

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
        description="Pass arguments for RFmxPulse Phase and Frequency Advanced Example",
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
