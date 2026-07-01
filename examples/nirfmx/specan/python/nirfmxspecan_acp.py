r"""Steps:
1. Open a new RFmx session.
2. Configure Selected Ports.
3. Configure Frequency Reference.
4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation).
5. Configure RF Attenuation.
6. Select ACP measurement and enable the traces.
7. Configure ACP Measurement Method, Power Units and Averaging Parameters.
8. Configure ACP RBW filter.
9. Configure ACP Sweep Time.
10. Configure ACP Noise Compensation.
11. Configure ACP Carrier Channel Settings (Integration BW, RRC Filter).
12. Configure ACP Number of Offset Channels.
13. Configure ACP Offset Channel Settings (Offset Frequencies, Integration BW, RRC Filter).
    Use "offset:all" selector string to set a parameter for all the Offset Channels.
14. Initiate Measurement.
15. Fetch ACP Measurements and Traces.
16. Close the RFmx Session.
"""

import argparse
import ctypes
import sys

import nirfmxspecan
import numpy

import nirfmxinstr

NUMBER_OF_OFFSETS = 2


class CarrierChannel(ctypes.Structure):
    """Carrier Channel Structure."""

    _fields_ = [
        ("integration_bandwidth", ctypes.c_double),
        ("rrc_filter_enabled", ctypes.c_int),
        ("rrc_filter_alpha", ctypes.c_double),
    ]


class OffsetChannel(ctypes.Structure):
    """Offset Channel Structure."""

    _fields_ = [
        ("integration_bandwidth", ctypes.c_double),
        ("frequency_offset", ctypes.c_double * NUMBER_OF_OFFSETS),
        ("rrc_filter_enabled", ctypes.c_int),
        ("rrc_filter_alpha", ctypes.c_double),
    ]


def example(resource_name, option_string):
    """Run Example."""
    # Initialize input variables
    selected_ports = ""
    center_frequency = 1e+9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.00  # dB

    frequency_source = "OnboardClock"
    frequency = 10.0e+6  # Hz

    measurement_interval = 10e-3  # seconds
    auto_level = True

    carrier_channel_input = CarrierChannel()
    carrier_channel_input.integration_bandwidth = 1.0e+6
    carrier_channel_input.rrc_filter_enabled = nirfmxspecan.AcpCarrierRrcFilterEnabled.FALSE.value
    carrier_channel_input.rrc_filter_alpha = 0.220

    power_units = nirfmxspecan.AcpPowerUnits.DBM
    noise_compensation_enabled = nirfmxspecan.AcpNoiseCompensationEnabled.FALSE

    sweep_time_auto = nirfmxspecan.AcpSweepTimeAuto.TRUE
    sweep_time_interval = 1.00e-3  # seconds

    rbw_filter_type = nirfmxspecan.AcpRbwFilterType.GAUSSIAN
    rbw_auto = nirfmxspecan.AcpRbwAutoBandwidth.TRUE
    rbw = 10.0e3  # Hz

    offset_channel_input = OffsetChannel()
    offset_channel_input.integration_bandwidth = 1.0e+6  # Hz
    offset_channel_input.rrc_filter_enabled = nirfmxspecan.AcpOffsetRrcFilterEnabled.FALSE.value
    offset_channel_input.rrc_filter_alpha = 0.220
    offset_channel_input.frequency_offset[0] = 1.0e+6  # Hz
    offset_channel_input.frequency_offset[1] = 2.0e+6  # Hz

    averaging_enabled = nirfmxspecan.AcpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.AcpAveragingType.RMS

    timeout = 10.0  # seconds
    
    instr_session = None
    specan = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get SpecAn signal
        specan = instr_session.get_specan_signal_configuration()

        # Configure measurement
        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_frequency("", center_frequency)
        specan.configure_external_attenuation("", external_attenuation)

        if auto_level:
            auto_set_reference_level, _ = specan.auto_level(
                "", carrier_channel_input.integration_bandwidth, measurement_interval
            )
            print(f"Reference Level(dBm)                  {auto_set_reference_level}\n")
        else:
            specan.configure_reference_level("", reference_level)

        specan.select_measurements("", nirfmxspecan.MeasurementTypes.ACP, True)

        specan.acp.configuration.configure_power_units("", power_units)
        specan.acp.configuration.configure_averaging(
            "", averaging_enabled, averaging_count, averaging_type
        )
        specan.acp.configuration.configure_rbw_filter("", rbw_auto, rbw, rbw_filter_type)
        specan.acp.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        specan.acp.configuration.configure_noise_compensation_enabled(
            "", noise_compensation_enabled
        )
        specan.acp.configuration.configure_carrier_integration_bandwidth(
            "", carrier_channel_input.integration_bandwidth
        )
        specan.acp.configuration.configure_carrier_rrc_filter(
            "", carrier_channel_input.rrc_filter_enabled, carrier_channel_input.rrc_filter_alpha
        )

        specan.acp.configuration.configure_number_of_offsets("", NUMBER_OF_OFFSETS)

        specan.acp.configuration.configure_offset_array(
            "", offset_channel_input.frequency_offset, None, None
        )

        specan.acp.configuration.configure_offset_integration_bandwidth(
            "offset::all", offset_channel_input.integration_bandwidth
        )

        specan.acp.configuration.configure_offset_rrc_filter(
            "offset::all",
            offset_channel_input.rrc_filter_enabled,
            offset_channel_input.rrc_filter_alpha,
        )

        specan.initiate("", "")

        # Retrieve results
        spectrum = numpy.empty(0, dtype=numpy.float32)

        (
            lower_relative_power,
            upper_relative_power,
            lower_absolute_power,
            upper_absolute_power,
            error_code,
        ) = specan.acp.results.fetch_offset_measurement_array("", timeout)

        absolute_power, total_relative_power, carrier_offset, integration_bandwidth, error_code = (
            specan.acp.results.fetch_carrier_measurement("", timeout)
        )

        spectrum = numpy.empty(0, dtype=numpy.float32)
        specan.acp.results.fetch_spectrum("", timeout, spectrum)

        # Print Results
        print("-----------------Carrier Measurements-----------------\n")
        print(f"Absolute Power (dBm or dBm/Hz)       {absolute_power}")

        print("\n--------------Offset Channel Measurements-------------\n")
        for i in range(NUMBER_OF_OFFSETS):
            print(f"----Offset {i}\n")
            print(f"Lower Relative Power (dB)            {lower_relative_power[i]}\n")
            print(f"Upper Relative Power (dB)            {upper_relative_power[i]}\n")
            print(f"Lower Absolute Power (dBm or dBm/Hz) {lower_absolute_power[i]}\n")
            print(f"Upper Absolute Power (dBm or dBm/Hz) {upper_absolute_power[i]}\n")
        print("-------------------------------------------------\n")

    except Exception as e:
        print("ERROR: " + str(e))

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
        description="Pass arguments for ACP Example",
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
