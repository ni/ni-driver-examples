"""
RFmxPulse Time Sidelobe Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic RF signal properties (Center Frequency, RF Attenuation and External Attenuation).
4. Enable Pulse measurement and Traces Result.
5. Configure Trigger Type and Trigger Parameters.
6. Configure Reference Waveform.
7. Configure Acquisition Settings.
8. Configure Pulse Detection Settings.
9. Configure State and Threshold Level Settings.
10. Configure Time Sidelobe Settings.
11. Configure Selected Traces Settings; disable Pulse Metrics and Pulse Stability.
12. Initiate the Measurement.
13. Wait for Measurement to complete.
14. Fetch Pulse Count, Time Sidelobe, Statistical Time Sidelobe results and Time Sidelobe Trace.
15. Close RFmx Session.

Prerequisites:
- nptdms package (`pip install nptdms`)

"""

import argparse
import os
import sys

import numpy
from nptdms import TdmsFile
import nirfmxinstr
import nirfmxpulse

_DEFAULT_WAVEFORM_FILE = os.path.join(
    os.path.dirname(os.path.abspath(__file__)),
    "Support",
    "Pulse_FMChirpUp-10MHz_BW-80MHz_Rect-filter.tdms",
)


def example(resource_name, option_string, reference_waveform_file):
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
    measurement_filter_type = nirfmxpulse.MeasurementFilterType.RECTANGULAR
    acquisition_length = 1.0e-3          # s
    maximum_pulse_count_enabled = nirfmxpulse.MaximumPulseCountEnabled.FALSE
    maximum_pulse_count = 100

    pulse_detection_reference = nirfmxpulse.PulseDetectionReference.REFERENCE_LEVEL
    pulse_detection_threshold = -20.0    # dB
    pulse_detection_hysteresis = 1.0     # dB
    pulse_detection_minimum_off_duration = 50.0e-9  # s

    pulse_level_computation_method = nirfmxpulse.PulseLevelComputationMethod.MEDIAN
    pulse_droop_compensation_enabled = nirfmxpulse.PulseDroopCompensationEnabled.TRUE

    time_sidelobe_reference_window_type = nirfmxpulse.PulseTimeSidelobeReferenceWindowType.NONE
    time_sidelobe_keep_out_time_auto = nirfmxpulse.PulseTimeSidelobeKeepOutTimeAuto.TRUE
    time_sidelobe_keep_out_time = 1.0e-6  # s
    time_sidelobe_minimum_correlation = 0.5

    pulse_selected_pulse_trace = 0

    timeout = 10.0                       # s

    instr_session = None
    pulse = None

    try:
        # Read reference waveform from TDMS file
        with TdmsFile.open(reference_waveform_file) as tdms_file:
            group = tdms_file.groups()[0]
            channel = group.channels()[0]
            raw = channel[:].astype(numpy.float32)
            i_data = raw[0::2]
            q_data = raw[1::2]
            reference_waveform = (i_data + 1j * q_data).astype(numpy.complex64)
            ref_x0 = float(channel.properties.get("wf_start_offset", 0.0))
            ref_dx = float(channel.properties.get("wf_increment", 1.0 / 200.0e6))

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

        pulse.pulse_measurement.configuration.configure_1_reference_waveform(
            "", ref_x0, ref_dx, reference_waveform
        )

        pulse.set_measurement_bandwidth("", measurement_bandwidth)
        pulse.set_measurement_filter_type("", measurement_filter_type)
        pulse.set_acquisition_length("", acquisition_length)
        pulse.set_maximum_pulse_count_enabled("", maximum_pulse_count_enabled)
        pulse.set_maximum_pulse_count("", maximum_pulse_count)

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

        pulse.pulse_measurement.configuration.set_level_computation_method(
            "", pulse_level_computation_method
        )
        pulse.pulse_measurement.configuration.set_droop_compensation_enabled(
            "", pulse_droop_compensation_enabled
        )

        pulse.pulse_measurement.configuration.set_time_sidelobe_enabled(
            "", nirfmxpulse.PulseTimeSidelobeEnabled.TRUE
        )
        pulse.pulse_measurement.configuration.set_time_sidelobe_reference_window_type(
            "", time_sidelobe_reference_window_type
        )
        pulse.pulse_measurement.configuration.set_time_sidelobe_keep_out_time_auto(
            "", time_sidelobe_keep_out_time_auto
        )
        pulse.pulse_measurement.configuration.set_time_sidelobe_keep_out_time(
            "", time_sidelobe_keep_out_time
        )
        pulse.pulse_measurement.configuration.set_time_sidelobe_minimum_correlation(
            "", time_sidelobe_minimum_correlation
        )

        pulse.pulse_measurement.configuration.set_metrics_enabled(
            "", nirfmxpulse.PulseMetricsEnabled.FALSE
        )
        pulse.pulse_measurement.configuration.set_stability_enabled(
            "", nirfmxpulse.PulseStabilityEnabled.FALSE
        )
        pulse.pulse_measurement.configuration.set_selected_pulse_trace(
            "", pulse_selected_pulse_trace
        )

        pulse.initiate("", "")
        pulse.wait_for_measurement_complete("", timeout)

        pulse_count, _ = pulse.pulse_measurement.results.get_pulse_count("")

        mainlobe_width, _ = pulse.pulse_measurement.results.get_time_sidelobe_mainlobe_width("")
        sidelobe_delay, _ = pulse.pulse_measurement.results.get_time_sidelobe_delay("")
        peak_sidelobe_level, _ = pulse.pulse_measurement.results.get_time_sidelobe_peak_sidelobe_level("")
        compression_ratio, _ = pulse.pulse_measurement.results.get_time_sidelobe_compression_ratio("")
        peak_correlation, _ = pulse.pulse_measurement.results.get_time_sidelobe_peak_correlation("")

        mainlobe_width_mean, _ = pulse.pulse_measurement.results.get_time_sidelobe_mainlobe_width_mean("")
        mainlobe_width_max, _ = pulse.pulse_measurement.results.get_time_sidelobe_mainlobe_width_maximum("")
        mainlobe_width_min, _ = pulse.pulse_measurement.results.get_time_sidelobe_mainlobe_width_minimum("")
        mainlobe_width_sd, _ = pulse.pulse_measurement.results.get_time_sidelobe_mainlobe_width_standard_deviation("")
        sidelobe_delay_mean, _ = pulse.pulse_measurement.results.get_time_sidelobe_delay_mean("")
        sidelobe_delay_max, _ = pulse.pulse_measurement.results.get_time_sidelobe_delay_maximum("")
        sidelobe_delay_min, _ = pulse.pulse_measurement.results.get_time_sidelobe_delay_minimum("")
        sidelobe_delay_sd, _ = pulse.pulse_measurement.results.get_time_sidelobe_delay_standard_deviation("")
        peak_sl_level_mean, _ = pulse.pulse_measurement.results.get_time_sidelobe_peak_sidelobe_level_mean("")
        peak_sl_level_max, _ = pulse.pulse_measurement.results.get_time_sidelobe_peak_sidelobe_level_maximum("")
        peak_sl_level_min, _ = pulse.pulse_measurement.results.get_time_sidelobe_peak_sidelobe_level_minimum("")
        peak_sl_level_sd, _ = pulse.pulse_measurement.results.get_time_sidelobe_peak_sidelobe_level_standard_deviation("")
        compression_ratio_mean, _ = pulse.pulse_measurement.results.get_time_sidelobe_compression_ratio_mean("")
        compression_ratio_max, _ = pulse.pulse_measurement.results.get_time_sidelobe_compression_ratio_maximum("")
        compression_ratio_min, _ = pulse.pulse_measurement.results.get_time_sidelobe_compression_ratio_minimum("")
        compression_ratio_sd, _ = pulse.pulse_measurement.results.get_time_sidelobe_compression_ratio_standard_deviation("")
        peak_correlation_mean, _ = pulse.pulse_measurement.results.get_time_sidelobe_peak_correlation_mean("")
        peak_correlation_max, _ = pulse.pulse_measurement.results.get_time_sidelobe_peak_correlation_maximum("")
        peak_correlation_min, _ = pulse.pulse_measurement.results.get_time_sidelobe_peak_correlation_minimum("")
        peak_correlation_sd, _ = pulse.pulse_measurement.results.get_time_sidelobe_peak_correlation_standard_deviation("")

        time_sidelobe_trace = numpy.empty(0, dtype=numpy.float32)
        _ = pulse.pulse_measurement.results.fetch_time_sidelobe_trace(
            "", timeout, time_sidelobe_trace
        )

        # Print Results
        print(f"\nPulse Count                            : {pulse_count}\n")
        print("---------------------Time Sidelobe Results------------------------------------------")
        for i in range(pulse_count):
            print(f"Index                                      : {i}")
            print(f"Mainlobe Width (s)                         : {mainlobe_width[i]:.11f}")
            print(f"Sidelobe Delay (s)                         : {sidelobe_delay[i]:.11f}")
            print(f"Peak Sidelobe Level (dB)                   : {peak_sidelobe_level[i]:.9f}")
            print(f"Compression Ratio (%)                      : {compression_ratio[i]:.9f}")
            print(f"Peak Correlation                           : {peak_correlation[i]:.9f}")
            print("-----------------------------------------------------------------------------------\n")

        print("--------------------Statistical Time Sidelobe Results-------------------------------")
        print(f"Mainlobe Width Mean (s)                        : {mainlobe_width_mean}")
        print(f"Mainlobe Width Max (s)                         : {mainlobe_width_max}")
        print(f"Mainlobe Width Min (s)                         : {mainlobe_width_min}")
        print(f"Mainlobe Width SD (s)                          : {mainlobe_width_sd}")
        print(f"Sidelobe Delay Mean (s)                        : {sidelobe_delay_mean}")
        print(f"Sidelobe Delay Max (s)                         : {sidelobe_delay_max}")
        print(f"Sidelobe Delay Min (s)                         : {sidelobe_delay_min}")
        print(f"Sidelobe Delay SD (s)                          : {sidelobe_delay_sd}")
        print(f"Peak Sidelobe Level Mean (dB)                  : {peak_sl_level_mean}")
        print(f"Peak Sidelobe Level Max (dB)                   : {peak_sl_level_max}")
        print(f"Peak Sidelobe Level Min (dB)                   : {peak_sl_level_min}")
        print(f"Peak Sidelobe Level SD (dB)                    : {peak_sl_level_sd}")
        print(f"Compression Ratio Mean (%)                     : {compression_ratio_mean}")
        print(f"Compression Ratio Max (%)                      : {compression_ratio_max}")
        print(f"Compression Ratio Min (%)                      : {compression_ratio_min}")
        print(f"Compression Ratio SD (%)                       : {compression_ratio_sd}")
        print(f"Peak Correlation Mean                          : {peak_correlation_mean}")
        print(f"Peak Correlation Max                           : {peak_correlation_max}")
        print(f"Peak Correlation Min                           : {peak_correlation_min}")
        print(f"Peak Correlation SD                            : {peak_correlation_sd}")
        print("------------------------------------------------------------------------------------\n")

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
        description="Pass arguments for RFmxPulse Time Sidelobe Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr."
    )
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    parser.add_argument(
        "-w",
        "--reference-waveform-file",
        default=_DEFAULT_WAVEFORM_FILE,
        help="Path to the reference waveform TDMS file.",
    )
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string, args.reference_waveform_file)


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
    example("RFSA", "", _DEFAULT_WAVEFORM_FILE)


if __name__ == "__main__":
    main()
