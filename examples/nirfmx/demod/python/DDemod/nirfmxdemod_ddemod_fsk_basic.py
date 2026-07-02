r"""Steps:
1. Open a new RFmx Session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select DDemod Measurement.
6. Configure FSK Modulation and M.
7. Configure DDemod FSK Deviation.
8. Configure DDemod Symbol Rate, Number of Symbols and Pulse Shaping Filter.
9. Configure DDemod Measurement Filter Type as Auto.
10. Configure DDemod Averaging.
11. Initiate Measurement.
12. Fetch FSK Measurement Results.
13. Close the RFmx Session.
"""

import argparse
import sys

import numpy

import nirfmxdemod
import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    # Initialize input variables
    selected_ports = ""
    center_frequency = 1e9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.00  # dB

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    fsk_deviation = 15.000e3  # Hz
    symbol_rate = 100.000e3  # Hz
    num_of_symbols = 1000
    pulse_shaping_filter_parameter = 0.50

    # Averaging
    averaging_count = 10
    timeout = 10.0  # seconds

    instr_session = None
    demod = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get Demod signal
        demod = instr_session.get_demod_signal_configuration()

        # Configure measurement
        instr_session.configure_frequency_reference("", frequency_source, frequency)
        demod.set_selected_ports("", selected_ports)
        demod.configure_rf("", center_frequency, reference_level, external_attenuation)
        demod.select_measurements("", nirfmxdemod.MeasurementTypes.DDEMOD, True)
        demod.digital_demod.configuration.configure_modulation_type(
            "",
            nirfmxdemod.DDemodModulationType.FSK,
            nirfmxdemod.DDemodM.M2,
            nirfmxdemod.DDemodDifferentialEnabled.FALSE,
        )
        demod.digital_demod.configuration.configure_fsk_deviation(
            "", fsk_deviation, nirfmxdemod.DDemodFskReferenceCompensationEnabled.TRUE
        )
        demod.digital_demod.configuration.configure_symbol_rate("", symbol_rate)
        demod.digital_demod.configuration.configure_number_of_symbols("", num_of_symbols)
        demod.digital_demod.configuration.configure_pulse_shaping_filter(
            "",
            nirfmxdemod.DDemodPulseShapingFilterType.GAUSSIAN,
            pulse_shaping_filter_parameter,
            0,
            1,
            numpy.empty(0, dtype=numpy.float32),
        )
        demod.digital_demod.configuration.configure_measurement_filter(
            "",
            nirfmxdemod.DDemodMeasurementFilterType.AUTO,
            0,
            1,
            numpy.empty(0, dtype=numpy.float32),
        )
        demod.digital_demod.configuration.configure_averaging(
            "", nirfmxdemod.DDemodAveragingEnabled.FALSE, averaging_count
        )
        demod.initiate("", "")

        # Retrieve results
        mean_fsk_deviation, mean_rms_fsk_error, max_peak_fsk_error, error_code = (
            demod.digital_demod.results.fetch_fsk_results("", timeout)
        )

        # Print Results
        print(f"Mean FSK Deviation(Hz)                : {mean_fsk_deviation}")
        print(f"Mean RMS FSK Error(Hz)                : {mean_rms_fsk_error}")
        print(f"Maximum Peak FSK Error (%)            : {max_peak_fsk_error}")

    except Exception as e:
        print("ERROR: " + str(e))

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
        description="Pass arguments for DDemod FSK Basic Example",
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
