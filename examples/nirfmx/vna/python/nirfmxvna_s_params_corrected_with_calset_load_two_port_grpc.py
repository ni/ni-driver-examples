r"""Getting Started:

To run this example, install "RFmx VNA" on the server machine:
  https://www.ni.com/en-us/support/downloads/software-products/download.rfmx-vna.html

Download and run the NI gRPC Device Server (ni_grpc_device_server.exe) on the server machine:
  https://github.com/ni/grpc-device/releases

  
Running from command line:

Server machine's IP address, port number, resource name and options can be passed as separate
command line arguments.

  > python nirfmxvna_s_params_corrected_with_calset_load_two_port_grpc.py <server_address> <port_number> <resource_name> <options>

If they are not passed in as command line arguments, then by default the server address will be
"localhost:31763", with "VNA" as the resource name and empty option string.
"""

r"""RFmx VNA S-Parameters Corrected with Calset Load Two Port gRPC Example

Steps:
1. Open a new RFmx session.
2. Configure Frequency Reference.
3. Configure sweep settings: Frequency List, IF Bandwidth, and Power Level and Test Rx Attenuation 
   with different port names.
4. Select S-Parameter measurement.
5. Configure number of S-Parameters.
6. Configure each S-Parameter and format.
7. Configure Magnitude Units & Phase Trace Type.
8. Load Calset data from a file.
9. Enable Correction.
10. Initiate the Measurement after user confirmation.
11. Read Number of SParams.
12. Fetch S-Parameter Correction State.
13. Fetch S-Parameter X data.
14. Fetch S-Parameter Y data for each S-Parameter.
15. Set SnP Export attributes (can be accessed and written before or after measurement initiate) 
    and save S-Parameter data to file.
16. Close RFmx Session.
"""

import argparse
import sys

import grpc
import nirfmxinstr
import nirfmxvna
import numpy


def example(server_name, port, resource_name, option_string):
    """Run VNA S-Parameters Corrected with Calset Load Two Port gRPC Example."""
    # Configuration parameters
    sweep_type = nirfmxvna.SweepType.LINEAR
    frequency_list_size = 251
    frequency_start = 1e9  # Hz
    frequency_stop = 26e9  # Hz

    port1_power_level = -10.0  # dBm
    port2_power_level = -10.0  # dBm
    port1_test_receiver_attenuation = 0.0  # dB
    port2_test_receiver_attenuation = 0.0  # dB
    if_bandwidth = 100e3  # Hz

    frequency_reference_source = "PXI_Clk"
    frequency_reference_frequency = 100e6  # Hz

    number_of_sparams = 4
    sparams_parameters = ["S11", "S12", "S21", "S22"]
    sparams_formats = [
        nirfmxvna.SParamsFormat.MAGNITUDE,
        nirfmxvna.SParamsFormat.MAGNITUDE,
        nirfmxvna.SParamsFormat.MAGNITUDE,
        nirfmxvna.SParamsFormat.MAGNITUDE,
    ]

    magnitude_units = nirfmxvna.SParamsMagnitudeUnits.DB
    phase_trace_type = nirfmxvna.SParamsPhaseTraceType.WRAPPED

    calset_file_path = ""
    snp_file_path = ""

    timeout = 10.0  # seconds

    instr_session = None
    vna_signal = None

    try:
        # Create a new RFmx gRPC Session
        channel = grpc.insecure_channel(
            f"{server_name}:{port}",
            options=[
                ("grpc.max_receive_message_length", -1),
                ("grpc.max_send_message_length", -1),
            ],
        )
        grpc_options = nirfmxinstr.GrpcSessionOptions(channel, "Remote_VNA_Session")

        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(
            resource_name, option_string, grpc_options=grpc_options
        )

        # Get VNA signal configuration
        vna_signal = instr_session.get_vna_signal_configuration()

        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )

        vna_signal.set_sweep_type("", sweep_type)
        vna_signal.set_start_frequency("", frequency_start)
        vna_signal.set_stop_frequency("", frequency_stop)
        vna_signal.set_number_of_points("", frequency_list_size)
        vna_signal.set_if_bandwidth("", if_bandwidth)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port1")
        vna_signal.set_power_level(port_selector_string, port1_power_level)
        vna_signal.set_test_receiver_attenuation(
            port_selector_string, port1_test_receiver_attenuation
        )

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port2")
        vna_signal.set_power_level(port_selector_string, port2_power_level)
        vna_signal.set_test_receiver_attenuation(
            port_selector_string, port2_test_receiver_attenuation
        )

        vna_signal.select_measurements("", nirfmxvna.MeasurementTypes.SPARAMS, False)

        vna_signal.s_params.configuration.set_number_of_s_parameters("", number_of_sparams)

        for i in range(number_of_sparams):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)
            vna_signal.s_params.configuration.configure_s_parameter(
                sparam_selector_string, sparams_parameters[i]
            )
            vna_signal.s_params.configuration.set_format(
                sparam_selector_string, sparams_formats[i]
            )

        vna_signal.s_params.configuration.set_magnitude_units("", magnitude_units)
        vna_signal.s_params.configuration.set_phase_trace_type("", phase_trace_type)

        vna_signal.calset_load_from_file("", "", calset_file_path)

        vna_signal.set_correction_enabled("", nirfmxvna.CorrectionEnabled.TRUE)
        vna_signal.initiate("", "")

        # Retrieve results
        number_of_sparams_result, error_code = vna_signal.s_params.configuration.get_number_of_s_parameters("")

        correction_state_result, error_code = vna_signal.s_params.results.get_correction_state("")

        sparams_x_data_result, _ = vna_signal.s_params.results.fetch_x_data("", timeout)

        sparams_y1_data_result = []
        sparams_y2_data_result = []

        for i in range(number_of_sparams_result):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)

            y1_data, y2_data, error_code = vna_signal.s_params.results.fetch_y_data(
                sparam_selector_string, timeout
            )

            sparams_y1_data_result.append(y1_data)
            sparams_y2_data_result.append(y2_data)

        vna_signal.s_params.configuration.set_snp_data_format(
            "", nirfmxvna.SParamsSnPDataFormat.AUTO
        )
        vna_signal.s_params.configuration.set_snp_ports("", "port1,port2")
        vna_signal.s_params.configuration.export_to_snp_file("", snp_file_path)

    except Exception as e:
        print(f"ERROR: {e}")
        sys.exit(1)

    finally:
        if vna_signal is not None:
            vna_signal.dispose()
            vna_signal = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _parse_args():
    parser = argparse.ArgumentParser(
        description="VNA S-Parameters Corrected with Calset Load Two Port gRPC Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-s",
        "--server-address",
        type=str,
        default="localhost",
        help="Server address",
    )
    parser.add_argument(
        "-p",
        "--port",
        type=str,
        default="31763",
        help="Server port",
    )
    parser.add_argument(
        "-r",
        "--resource",
        type=str,
        default="VNA",
        help="Resource name of the VNA device",
    )
    parser.add_argument(
        "-o",
        "--option-string",
        type=str,
        default="",
        help="Option string",
    )
    return parser.parse_args()


def main():
    """Main function."""
    args = _parse_args()
    example(args.server_address, args.port, args.resource, args.option_string)


if __name__ == "__main__":
    main()
