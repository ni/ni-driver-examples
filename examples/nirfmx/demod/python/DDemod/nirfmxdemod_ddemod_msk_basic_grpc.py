r"""Getting Started:

To run this example, install "RFmx Demod" on the server machine:
  https://www.ni.com/en-us/support/downloads/software-products/download.rfmx-demod.html

Download and run the NI gRPC Device Server (ni_grpc_device_server.exe) on the server machine:
  https://github.com/ni/grpc-device/releases


Running from command line:

Server machine's IP address, port number, resource name and options can be passed as separate
command line arguments.

  > python nirfmxdemod_ddemod_msk_basic_grpc.py <server_address> <port_number> <resource_name> <options>

If they are not passed in as command line arguments, then by default the server address will be
"localhost:31763", with "RFSA" as the resource name and empty option string.
"""

r"""Example Steps:
1. Open a new RFmx gRPC Session.
2. Configure Selected Ports.
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Select MSK Modulation and Differential Enabled.
5. Configure DDemod Symbol Rate, Number of Symbols and Pulse Shaping Filter.
6. Configure DDemod Measurement Filter Type as Auto.
7. Configure DDemod Averaging.
8. Read DDemod Measurement Results.
9. Close the RFmx Session.
"""

import argparse
import sys

import grpc
import numpy
import nirfmxdemod
import nirfmxinstr


def example(server_name, port, resource_name, option_string):
    """Run Example."""
    # Initialize input variables
    selector_string = ""
    selected_ports = ""
    center_frequency = 1e9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.00  # dB

    timeout = 10.0  # seconds

    symbol_rate = 100.000e3  # Hz
    num_of_symbols = 1000

    # Pulse shaping filter
    pulse_shaping_filter_parameter = 0.50
    pulse_shaping_filter_custom_coefficients = numpy.empty(0, dtype=numpy.float32)
    measurement_filter_custom_coefficients = numpy.empty(0, dtype=numpy.float32)

    # Averaging
    averaging_count = 10

    instr_session = None
    demod = None

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

        # Get Demod signal
        demod = instr_session.get_demod_signal_configuration()

        # Configure DDemod parameters
        demod.set_selected_ports("", selected_ports)
        demod.configure_rf(selector_string, center_frequency, reference_level, external_attenuation)
        demod.digital_demod.configuration.configure_modulation_type(
            selector_string,
            nirfmxdemod.DDemodModulationType.MSK,
            nirfmxdemod.DDemodM.M4,
            nirfmxdemod.DDemodDifferentialEnabled.FALSE,
        )
        demod.digital_demod.configuration.configure_symbol_rate(selector_string, symbol_rate)
        demod.digital_demod.configuration.configure_number_of_symbols(selector_string, num_of_symbols)
        demod.digital_demod.configuration.configure_pulse_shaping_filter(
            selector_string,
            nirfmxdemod.DDemodPulseShapingFilterType.GAUSSIAN,
            pulse_shaping_filter_parameter,
            0,
            1,
            pulse_shaping_filter_custom_coefficients,
        )
        demod.digital_demod.configuration.configure_measurement_filter(
            selector_string, nirfmxdemod.DDemodMeasurementFilterType.AUTO, 0, 1, measurement_filter_custom_coefficients
        )
        demod.digital_demod.configuration.configure_averaging(
            selector_string, nirfmxdemod.DDemodAveragingEnabled.FALSE, averaging_count
        )

        # Retrieve results
        mean_frequency_offset, mean_rms_evm, maximum_peak_evm, mean_modulation_error_ratio, error_code = (
            demod.digital_demod.results.read(selector_string, timeout)
        )

        # Print Results
        print(f"Mean Frequency Offset (Hz)         : {mean_frequency_offset}")
        print(f"Mean RMS EVM(%)                    : {mean_rms_evm}")
        print(f"Maximum Peak EVM(%)                : {maximum_peak_evm}")
        print(f"Mean Modulation Error Ratio(dB)    : {mean_modulation_error_ratio}")

    except nirfmxinstr.RFmxError as e:
        print("ERROR: " + str(e.description))

    finally:
        # Close Session
        if demod is not None:
            demod.dispose()
            demod = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for DDemod MSK Basic gRPC Example",
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
