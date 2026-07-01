r"""Getting Started:
To run this example, install "RFmx WLAN" on the server machine:
  https://www.ni.com/en-us/support/downloads/software-products/download.rfmx-wlan.html
Download and run the NI gRPC Device Server (ni_grpc_device_server.exe) on the server machine:
  https://github.com/ni/grpc-device/releases
Running from command line:
Server machine's IP address, port number, resource name and options can be passed as separate
command line arguments.
  > python nirfmxwlan_dsssmodacc.py <server_address> <port_number> <resource_name> <options>
If they are not passed in as command line arguments, then by default the server address will be
"localhost:31763", with "RFSA" as the resource name and empty option string.
"""

r"""RFmx WLAN DSSS ModAcc Example
Steps:
1. Open a new RFmx session.
2. Configure the frequency reference properties (Clock Source and Clock Frequency).
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
5. Configure Standard to 802.11b.
6. Select DSSSModAcc measurement and enable the traces.
7. Configure Measurement Length.
8. Configure Pulse Shaping Filter Type and Parameter.
9. Configure EVM unit.
10. Configure Averaging parameters.
11. Initiate Measurement.
12. Fetch DSSSModAcc Traces and Measurements.
13. Close the RFmx Session.
"""

import argparse
import sys

import grpc
import nirfmxinstr
import nirfmxwlan
import numpy


def example(server_name, port, resource_name, option_string):
    """Run WLAN DSSS ModAcc Example."""
    # Initialize input variables
    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    center_frequency = 2.412e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    iq_power_edge_enabled = True
    iq_power_edge_level = -20.0  # dB
    trigger_delay = 0.0  # seconds
    minimum_quiet_time_mode = nirfmxwlan.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 5.0e-6  # seconds

    standard = nirfmxwlan.Standard.STANDARD_802_11_B

    measurement_offset = 0  # chips
    maximum_measurement_length = 1000  # chips

    pulse_shaping_filter_type = nirfmxwlan.DsssModAccPulseShapingFilterType.RECTANGULAR
    pulse_shaping_filter_parameter = 0.50

    evm_unit = nirfmxwlan.DsssModAccEvmUnit.PERCENTAGE

    averaging_enabled = nirfmxwlan.DsssModAccAveragingEnabled.FALSE
    averaging_count = 10

    timeout = 10.0  # seconds

    instr_session = None
    wlan_signal = None

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
        instr_session = nirfmxinstr.Session(resource_name, option_string, grpc_options=grpc_options)

        # Get WLAN signal configuration
        wlan_signal = instr_session.get_wlan_signal_configuration()

        # Configure frequency reference
        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )

        wlan_signal.configure_frequency("", center_frequency)
        wlan_signal.configure_reference_level("", reference_level)
        wlan_signal.configure_external_attenuation("", external_attenuation)

        wlan_signal.configure_iq_power_edge_trigger(
            "",
            "0",
            nirfmxwlan.IQPowerEdgeTriggerSlope.RISING_SLOPE,
            iq_power_edge_level,
            trigger_delay,
            minimum_quiet_time_mode,
            minimum_quiet_time,
            nirfmxwlan.IQPowerEdgeTriggerLevelType.RELATIVE,
            iq_power_edge_enabled,
        )

        wlan_signal.configure_standard("", standard)

        wlan_signal.select_measurements("", nirfmxwlan.MeasurementTypes.DSSSMODACC, True)

        wlan_signal.dsssmodacc.configuration.configure_measurement_length(
            "", measurement_offset, maximum_measurement_length
        )
        wlan_signal.dsssmodacc.configuration.set_pulse_shaping_filter_type(
            "", pulse_shaping_filter_type
        )
        wlan_signal.dsssmodacc.configuration.set_pulse_shaping_filter_parameter(
            "", pulse_shaping_filter_parameter
        )
        wlan_signal.dsssmodacc.configuration.configure_evm_unit("", evm_unit)
        wlan_signal.dsssmodacc.configuration.configure_averaging(
            "", averaging_enabled, averaging_count
        )

        wlan_signal.initiate("", "")

        # Retrieve results
        (
            rms_evm_mean,
            peak_evm_2016_maximum,
            peak_evm_2007_maximum,
            peak_evm_1999_maximum,
            frequency_error_mean,
            chip_clock_error_mean,
            number_of_chips_used,
            error_code,
        ) = wlan_signal.dsssmodacc.results.fetch_evm("", timeout)

        (
            data_modulation_format,
            payload_length,
            preamble_type,
            locked_clocks_bit,
            header_crc_status,
            psdu_crc_status,
            error_code,
        ) = wlan_signal.dsssmodacc.results.fetch_ppdu_information("", timeout)

        (
            iq_origin_offset_mean,
            iq_gain_imbalance_mean,
            iq_quadrature_error_mean,
            error_code,
        ) = wlan_signal.dsssmodacc.results.fetch_iq_impairments("", timeout)

        # Fetch traces
        evm_per_chip_mean_trace = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = wlan_signal.dsssmodacc.results.fetch_evm_per_chip_mean_trace(
            "", timeout, evm_per_chip_mean_trace
        )

        constellation_trace = numpy.empty(0, dtype=numpy.complex64)
        error_code = wlan_signal.dsssmodacc.results.fetch_constellation_trace(
            "", timeout, constellation_trace
        )

        # Print Results
        print("\n---------------EVM---------------\n")
        print(f"RMS EVM Mean (% or dB)                     :{rms_evm_mean}")
        print(f"Peak EVM (802.11-2016) Maximum (% or dB)   :{peak_evm_2016_maximum}")
        print(f"Peak EVM (802.11-2007) Maximum (% or dB)   :{peak_evm_2007_maximum}")
        print(f"Peak EVM (802.11-1999) Maximum (% or dB)   :{peak_evm_1999_maximum}")
        print(f"Number of Chips Used                       :{number_of_chips_used}")
        print("\n---------------Impairments & PPDU Info---------------\n")
        print(f"Frequency Error Mean (Hz)                  :{frequency_error_mean}")
        print(f"Chip Clock Error Mean (ppm)                :{chip_clock_error_mean}")

        print("\n---------------IQ Impairments---------------\n")
        print(f"I/Q Origin Offset Mean (dB)                :{iq_origin_offset_mean}")
        print(f"I/Q Gain Imbalance Mean (dB)               :{iq_gain_imbalance_mean}")
        print(f"I/Q Quadrature Error Mean (deg)            :{iq_quadrature_error_mean}")

        print("\n---------------PPDU Information---------------\n")
        print(f"Data Modulation Format                     :{data_modulation_format.name}")
        print(f"Payload Length (bytes)                     :{payload_length}")
        print(f"Preamble Type                              :{preamble_type.name}")
        print(f"Locked Clock Bit                           :{locked_clocks_bit}")

    except nirfmxinstr.RFmxError as e:
        print("ERROR: " + str(e.description))

    finally:
        # Close Session
        if wlan_signal is not None:
            wlan_signal.dispose()
            wlan_signal = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for WLAN DSSS ModAcc gRPC Example",
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