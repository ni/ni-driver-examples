r"""Getting Started:

To run this example, install "RFmx SpecAn" on the server machine:
  https://www.ni.com/en-us/support/downloads/software-products/download.rfmx-specan.html

Download and run the NI gRPC Device Server (ni_grpc_device_server.exe) on the server machine:
  https://github.com/ni/grpc-device/releases


Running from command line:

Server machine's IP address, port number, resource name and options can be passed as separate
command line arguments.

  > python nirfmxspecan_txp_grpc.py <server_address> <port_number> <resource_name> <options>

If they are not passed in as command line arguments, then by default the server address will be
"localhost:31763", with "RFSA" as the resource name and empty option string.
"""

r"""Example Steps:
1. Open a new RFmx session
2. Configure the basic instrument properties (Clock Source and Clock Frequency)
3. Configure Selected Ports
4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
5. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time)
6. Configure TXP measurement and enable the traces
7. Configure TXP Measurement Interval
8. Configure TXP RBW Filter
9. Configure TXP Threshold
10. Configure TXP Averaging
11. Initiate Measurement
12. Fetch TXP Traces and Measurements
13. Close the RFmx Session
"""

import argparse
import sys

import grpc
import nirfmxinstr
import nirfmxspecan
import numpy


def example(server_name, port, resource_name, option_string):
    """Run Example."""
    # Initialize input variables
    selected_ports = ""
    center_frequency = 1e9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.00  # dB

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    iq_power_edge_enabled = False
    iq_power_edge_level = -20.0  # dBm
    trigger_delay = 0.0  # seconds
    min_quiet_time = 0.0  # seconds
    enable_trigger = True

    measurement_interval = 1e-3  # seconds
    rbw = 100e3  # Hz

    rbw_filter_type = nirfmxspecan.TxpRbwFilterType.GAUSSIAN
    rrc_alpha = 0.010

    averaging_enabled = nirfmxspecan.TxpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.TxpAveragingType.RMS

    vbw_auto = nirfmxspecan.TxpVbwFilterAutoBandwidth.TRUE
    vbw = 30.0e3  # Hz
    vbw_to_rbw_ratio = 3

    threshold_enabled = nirfmxspecan.TxpThresholdEnabled.FALSE
    threshold_type = nirfmxspecan.TxpThresholdType.RELATIVE
    threshold_level = -20.0  # dBm or dBm / Hz

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        # Create a new RFmx gRPC Session
        channel = grpc.insecure_channel(
            f"{server_name}:{port}",
            options=[
                ("grpc.max_receive_message_length", -1),
                ("grpc.max_send_message_length", -1),
            ],
        )
        grpc_options = nirfmxinstr.GrpcSessionOptions(channel, "Remote_RFSA_Session")
        instr_session = nirfmxinstr.Session(resource_name, option_string, grpc_options=grpc_options)

        # Get SpecAn signal
        specan = instr_session.get_specan_signal_configuration()

        # Configure measurement
        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_frequency("", center_frequency)
        specan.configure_reference_level("", reference_level)
        specan.configure_external_attenuation("", external_attenuation)

        if iq_power_edge_enabled:
            specan.configure_iq_power_edge_trigger(
                "",
                "0",
                iq_power_edge_level,
                nirfmxspecan.IQPowerEdgeTriggerSlope.RISING_SLOPE,
                trigger_delay,
                nirfmxspecan.TriggerMinimumQuietTimeMode.MANUAL,
                min_quiet_time,
                enable_trigger,
            )
        else:
            specan.disable_trigger("")

        specan.select_measurements("", nirfmxspecan.MeasurementTypes.TXP, True)

        specan.txp.configuration.configure_measurement_interval("", measurement_interval)
        specan.txp.configuration.configure_rbw_filter("", rbw, rbw_filter_type, rrc_alpha)
        specan.txp.configuration.configure_averaging(
            "", averaging_enabled, averaging_count, averaging_type
        )
        specan.txp.configuration.configure_vbw_filter("", vbw_auto, vbw, vbw_to_rbw_ratio)
        specan.txp.configuration.configure_threshold(
            "", threshold_enabled, threshold_level, threshold_type
        )
        specan.initiate("", "")

        # Retrieve results
        power = numpy.empty(0, dtype=numpy.float32)
        error_code = specan.txp.results.fetch_power_trace("", timeout, power)
        average_mean_power, peak_to_average_ratio, maximum_power, minimum_power, error_code = (
            specan.txp.results.fetch_measurement("", timeout)
        )

        # Print Results
        print(f"Average Mean Power  (dBm)      : {average_mean_power}")
        print(f"Peak to Average Ratio (dB)     : {peak_to_average_ratio}")
        print(f"Maximum Power (dBm)            : {maximum_power}")
        print(f"Minimum Power (dBm)            : {minimum_power}")

    except nirfmxinstr.RFmxError as e:
        print("ERROR: " + str(e.description))

    finally:
        # Close Session
        if specan is not None:
            specan.dispose()
            specan = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for TXP gRPC Example",
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