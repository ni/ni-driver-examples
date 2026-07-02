r"""Getting Started:

To run this example, install "RFmx Bluetooth" on the server machine:
  https://www.ni.com/en-us/support/downloads/software-products/download.rfmx-bluetooth.html

Download and run the NI gRPC Device Server (ni_grpc_device_server.exe) on the server machine:
  https://github.com/ni/grpc-device/releases

  
Running from command line:

Server machine's IP address, port number, resource name and options can be passed as separate
command line arguments.

  > python nirfmxbluetooth_devm.py <server_address> <port_number> <resource_name> <options>

If they are not passed in as command line arguments, then by default the server address will be
"localhost:31763", with "RFSA" as the resource name and empty option string.
"""

r"""RFmx Bluetooth DEVM Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Packet Type.
6. Configure Payload Length.
7. Select ModAcc measurement and enable Traces.
8. Configure ModAcc Burst Synchronization Mode.
9. Configure Averaging Parameters for ModAcc measurement.
10. Initiate the Measurement.
11. Fetch ModAcc Measurements and Trace.
12. Close RFmx Session.
"""

import argparse
import grpc
import sys

import nirfmxbluetooth
import nirfmxinstr
import numpy


def example(server_name, port, resource_name, option_string):
    """Run Bluetooth DEVM Example."""
    # Initialize input variables
    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    center_frequency = 2.402e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    enable_trigger = True
    iq_power_edge_trigger_slope = nirfmxbluetooth.IQPowerEdgeTriggerSlope.RISING
    iq_power_edge_trigger_level = -20.0  # dB
    minimum_quiet_time_mode = nirfmxbluetooth.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 100e-6  # seconds
    iq_power_edge_trigger_level_type = nirfmxbluetooth.IQPowerEdgeTriggerLevelType.RELATIVE
    trigger_delay = 0.0  # seconds

    # Configure packet type
    packet_type = nirfmxbluetooth.PacketType.PACKET_TYPE_2_DH1

    payload_length_mode = nirfmxbluetooth.PayloadLengthMode.AUTO
    payload_length = 10  # bytes

    burst_synchronization_type = nirfmxbluetooth.ModAccBurstSynchronizationType.PREAMBLE

    averaging_enabled = nirfmxbluetooth.ModAccAveragingEnabled.FALSE
    averaging_count = 10

    timeout = 10.0  # seconds

    instr_session = None
    bt_signal = None

    try:
        # Create a new RFmx gRPC Session
        channel = grpc.insecure_channel(f"{server_name}:{port}", options = [
            ("grpc.max_receive_message_length", -1),
            ("grpc.max_send_message_length", -1),
        ])
        grpc_options = nirfmxinstr.GrpcSessionOptions(channel, "Remote_RFSA_Session")

        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string, grpc_options = grpc_options)

        # Get Bluetooth signal configuration
        bt_signal = instr_session.get_bluetooth_signal_configuration()

        # Configure Frequency Reference
        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )

        bt_signal.configure_rf("", center_frequency, reference_level, external_attenuation)

        bt_signal.configure_iq_power_edge_trigger(
            "",
            "0",
            iq_power_edge_trigger_slope,
            iq_power_edge_trigger_level,
            trigger_delay,
            minimum_quiet_time_mode,
            minimum_quiet_time,
            iq_power_edge_trigger_level_type,
            enable_trigger,
        )

        bt_signal.configure_packet_type("", packet_type)
        bt_signal.configure_payload_length("", payload_length_mode, payload_length)

        bt_signal.select_measurements("", nirfmxbluetooth.MeasurementTypes.MODACC, True)

        bt_signal.modacc.configuration.configure_burst_synchronization_type(
            "", burst_synchronization_type
        )
        bt_signal.modacc.configuration.configure_averaging("", averaging_enabled, averaging_count)

        bt_signal.initiate("", "")

        peak_rms_devm_maximum, peak_devm_maximum, ninetynine_percent_devm, error_code = (
            bt_signal.modacc.results.fetch_devm("", timeout)
        )

        (
            header_frequency_error_wi_maximum,
            peak_frequency_error_wi_plus_w0_maximum,
            peak_frequency_error_w0_maximum,
            error_code,
        ) = bt_signal.modacc.results.fetch_frequency_error_edr("", timeout)

        devm_per_symbol, error_code = bt_signal.modacc.results.fetch_devm_per_symbol_trace(
            "", timeout
        )

        (
            time,
            frequency_error_wi_plus_w0,
            error_code,
        ) = bt_signal.modacc.results.fetch_frequency_error_wi_plus_w0_trace_edr("", timeout)

        constellation = numpy.empty(0, dtype=numpy.complex64)
        error_code = bt_signal.modacc.results.fetch_constellation_trace("", timeout, constellation)

        # Print Results
        print("------------------DEVM------------------")
        print(f"Peak Rms Devm Maximum (%)                : {peak_rms_devm_maximum}")
        print(f"Peak Devm Maximum (%)                    : {peak_devm_maximum}")
        print(f"99% Devm (%)                             : {ninetynine_percent_devm}")
        print("\n------------------EDR Frequency Error------------------\n")
        print(f"Header Frequency Error wi Maximum (Hz)   : {header_frequency_error_wi_maximum}")
        print(
            f"Peak Frequency Error wi+w0 Maximum (Hz)  : {peak_frequency_error_wi_plus_w0_maximum}"
        )
        print(f"Peak Frequency Error w0 Maximum (Hz)     : {peak_frequency_error_w0_maximum}")

    except nirfmxinstr.RFmxError as e:
        print("ERROR: " + str(e.description))

    finally:
        # Close Session
        if bt_signal is not None:
            bt_signal.dispose()
            bt_signal = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for Bluetooth DEVM gRPC Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-s", "--server-name", default="localhost", help="Server name or IP address of the gRPC server machine."
    )
    parser.add_argument(
        "-p", "--port", default="31763", help="Port number of the gRPC server."
    )
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
