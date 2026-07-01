r"""Getting Started:

To run this example, install "RFmx LTE" on the server machine:
  https://www.ni.com/en-us/support/downloads/software-products/download.rfmx-lte.html

Download and run the NI gRPC Device Server (ni_grpc_device_server.exe) on the server machine:
  https://github.com/ni/grpc-device/releases

  
Running from command line:

Server machine's IP address, port number, resource name and options can be passed as separate
command line arguments.

  > python nirfmxlte_ul_pvt_single_carrier_grpc.py <server_address> <port_number> <resource_name> <options>

If they are not passed in as command line arguments, then by default the server address will be
"localhost:31763", with "RFSA" as the resource name and empty option string.
"""

r"""RFmx LTE PvT gRPC Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Carrier Bandwidth.
6. Select PvT measurement and enable Traces.
7. Configure Duplex Scheme.
8. Configure Measurement Methods.
9. Configure Averaging Parameters for PvT measurement.
10. Initiate the Measurement.
11. Fetch PvT Traces and Measurements.
12. Close RFmx Session.
"""

import argparse
import sys

import grpc
import nirfmxinstr
import nirfmxlte
import numpy


def example(server_name, port, resource_name, option_string):
    """Run LTE PvT gRPC Example."""
    # Configuration parameters
    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    center_frequency = 1.95e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    iq_power_edge_trigger_source = "0"
    iq_power_edge_trigger_level = -20.0  # dB
    trigger_delay = 0.0  # seconds
    minimum_quiet_time_duration = 50.0e-6  # seconds
    enable_trigger = True

    iq_power_edge_trigger_slope = nirfmxlte.IQPowerEdgeTriggerSlope.RISING_SLOPE
    minimum_quiet_time_mode = nirfmxlte.TriggerMinimumQuietTimeMode.AUTO
    iq_power_edge_trigger_level_type = nirfmxlte.IQPowerEdgeTriggerLevelType.RELATIVE

    component_carrier_bandwidth = 10e6  # Hz
    component_carrier_frequency = 0.0  # Hz
    cell_id = 0

    uplink_downlink_configuration = nirfmxlte.UplinkDownlinkConfiguration.CONFIGURATION_0
    duplex_scheme = nirfmxlte.DuplexScheme.TDD

    measurement_method = nirfmxlte.PvtMeasurementMethod.NORMAL

    off_power_exclusion_before = 0.0
    off_power_exclusion_after = 0.0

    averaging_enabled = nirfmxlte.PvtAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxlte.PvtAveragingType.RMS

    timeout = 10.0  # seconds

    instr_session = None
    lte_signal = None

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

        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(
            resource_name, option_string, grpc_options=grpc_options
        )

        # Get LTE signal configuration
        lte_signal = instr_session.get_lte_signal_configuration()

        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )

        lte_signal.configure_rf("", center_frequency, reference_level, external_attenuation)

        lte_signal.configure_iq_power_edge_trigger(
            "",
            iq_power_edge_trigger_source,
            iq_power_edge_trigger_slope,
            iq_power_edge_trigger_level,
            trigger_delay,
            minimum_quiet_time_mode,
            minimum_quiet_time_duration,
            iq_power_edge_trigger_level_type,
            enable_trigger,
        )

        lte_signal.component_carrier.configure(
            "", component_carrier_bandwidth, component_carrier_frequency, cell_id
        )

        lte_signal.select_measurements("", nirfmxlte.MeasurementTypes.PVT, True)

        lte_signal.configure_duplex_scheme("", duplex_scheme, uplink_downlink_configuration)

        lte_signal.pvt.configuration.configure_measurement_method("", measurement_method)

        lte_signal.pvt.configuration.configure_off_power_exclusion_periods(
            "", off_power_exclusion_before, off_power_exclusion_after
        )

        lte_signal.pvt.configuration.configure_averaging(
            "", averaging_enabled, averaging_count, averaging_type
        )

        lte_signal.initiate("", "")

        # Retrieve results
        signal_power = numpy.empty(0, dtype=numpy.float32)
        absolute_limit = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = lte_signal.pvt.results.fetch_signal_power_trace(
            "", timeout, signal_power, absolute_limit
        )

        (
            measurement_status,
            mean_absolute_off_power_before,
            mean_absolute_off_power_after,
            mean_absolute_on_power,
            burst_width,
            error_code,
        ) = lte_signal.pvt.results.fetch_measurement("", timeout)

        # Print results
        print("\n********** Measurement **********")
        print(f"Status                               : {measurement_status.name}")
        print(f"Mean Absolute OFF Power Before (dBm) : {mean_absolute_off_power_before}")
        print(f"Mean Absolute OFF Power After (dBm)  : {mean_absolute_off_power_after}")
        print(f"Mean Absolute ON Power (dBm)         : {mean_absolute_on_power}")
        print(f"Burst Width (s)                      : {burst_width}")

    except nirfmxinstr.RFmxError as e:
        print("ERROR: " + str(e.description))

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
        description="Pass arguments for LTE PvT gRPC Example",
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
