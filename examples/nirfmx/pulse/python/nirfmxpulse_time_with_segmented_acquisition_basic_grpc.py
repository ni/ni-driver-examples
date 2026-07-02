"""
RFmxPulse Time with Segmented Acquisition Basic gRPC Example

Getting Started:

To run this example, install "RFmx Pulse" on the server machine:
  https://www.ni.com/en-us/support/downloads/software-products/download.rfmx-pulse.html

Download and run the NI gRPC Device Server (ni_grpc_device_server.exe) on the server machine:
  https://github.com/ni/grpc-device/releases


Running from command line:

Server machine's IP address, port number, resource name and options can be passed as separate
command line arguments.

  > python nirfmxpulse_time_with_segmented_acquisition_basic_grpc.py <server_address> <port_number> <resource_name> <options>

If they are not passed in as command line arguments, then by default the server address will be
"localhost:31763", with "RFSA" as the resource name and empty option string.
"""

"""Example Steps:
1. Open a new RFmx gRPC Session.
2. Configure Frequency Reference.
3. Configure basic RF signal properties (Center Frequency, RF Attenuation and External Attenuation).
4. Enable Pulse measurement; disable Traces Result.
5. Configure Trigger Type and Trigger Parameters.
6. Configure Bandwidth and Filter Type.
7. Configure Segmented Acquisition Settings.
8. Configure Pulse Detection Settings.
9. Configure State and Threshold Level Settings.
10. Enable Pulse Metrics, disable Pulse Stability.
11. Initiate the Measurement.
12. Wait for Measurement to complete.
13. Fetch Pulse Count and Timing Results.
14. Close RFmx Session.
"""

import argparse
import sys

import grpc
import nirfmxinstr
import nirfmxpulse


def example(server_name, port, resource_name, option_string):
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
    segment_acquisition_length = 15.0e-6  # s
    number_of_segments = 100
    segmented_acquisition_enabled = nirfmxpulse.SegmentedAcquisitionEnabled.TRUE

    pulse_detection_reference = nirfmxpulse.PulseDetectionReference.REFERENCE_LEVEL
    pulse_detection_threshold = -20.0    # dB
    pulse_detection_hysteresis = 1.0     # dB
    pulse_detection_minimum_off_duration = 50.0e-9  # s

    pulse_level_computation_method = nirfmxpulse.PulseLevelComputationMethod.MEDIAN
    pulse_amplitude_level_domain = nirfmxpulse.PulseAmplitudeLevelDomain.VOLTS
    pulse_upper_threshold_level = 90.0   # %
    pulse_width_threshold_level = 50.0   # %
    pulse_lower_threshold_level = 10.0   # %

    timeout = 10.0                       # s

    instr_session = None
    pulse = None

    try:
        # Create a new RFmx gRPC Session
        channel = grpc.insecure_channel(
            f"{server_name}:{port}",
            options=[
                ("grpc.max_receive_message_length", -1),
                ("grpc.max_send_message_length", -1),
            ],
        )
        grpc_options = nirfmxpulse.GrpcSessionOptions(channel, "Remote_RFSA_Session")
        instr_session = nirfmxinstr.Session(resource_name, option_string, grpc_options=grpc_options)

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

        pulse.set_segmented_acquisition_enabled("", segmented_acquisition_enabled)
        pulse.set_number_of_segments("", number_of_segments)
        pulse.set_acquisition_length("", segment_acquisition_length)

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
        pulse.pulse_measurement.configuration.set_amplitude_level_domain(
            "", pulse_amplitude_level_domain
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

        pulse.pulse_measurement.configuration.set_metrics_enabled(
            "", nirfmxpulse.PulseMetricsEnabled.TRUE
        )
        pulse.pulse_measurement.configuration.set_stability_enabled(
            "", nirfmxpulse.PulseStabilityEnabled.FALSE
        )

        pulse.initiate("", "")
        pulse.wait_for_measurement_complete("", timeout)

        pulse_count, _ = pulse.pulse_measurement.results.get_pulse_count("")

        rise_time, _ = pulse.pulse_measurement.results.get_rise_time("")                                     # s
        fall_time, _ = pulse.pulse_measurement.results.get_fall_time("")                                     # s
        pulse_width, _ = pulse.pulse_measurement.results.get_pulse_width("")                                 # s
        pulse_off_duration, _ = pulse.pulse_measurement.results.get_pulse_off_duration("")                   # s
        duty_cycle, _ = pulse.pulse_measurement.results.get_duty_cycle("")                                   # %
        pulse_repetition_interval, _ = pulse.pulse_measurement.results.get_pulse_repetition_interval("")    # s

        # Print Results
        print(f"\nPulse Count                            : {pulse_count}\n")
        print("\n------------------- Timing Results ----------------------\n")
        for i in range(len(rise_time)):
            print(f"Index                                  : {i}")
            print(f"Rise Time (s)                          : {rise_time[i]:.11f}")
            print(f"Fall Time (s)                          : {fall_time[i]:.11f}")
            print(f"Pulse Width (s)                        : {pulse_width[i]:.9f}")
            print(f"Pulse Off Duration (s)                 : {pulse_off_duration[i]:.9f}")
            print(f"Duty Cycle (%)                         : {duty_cycle[i]:.9f}")
            print(f"Pulse Repetition Interval (s)          : {pulse_repetition_interval[i]:.9f}")
            print("-------------------------------------------------------\n")

    except nirfmxinstr.RFmxError as e:
        print("ERROR: " + str(e.description))

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
        description="Pass arguments for RFmxPulse Time with Segmented Acquisition Basic gRPC Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-s",
        "--server-name",
        default="localhost",
        help="Server name or IP address of the gRPC server machine.",
    )
    parser.add_argument("-p", "--port", default="31763", help="Port number of the gRPC server.")
    parser.add_argument(
        "-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr."
    )
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    args = parser.parse_args(argsv)
    example(args.server_name, args.port, args.resource_name, args.option_string)


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
    example("localhost", "31763", "RFSA", "")


if __name__ == "__main__":
    main()
