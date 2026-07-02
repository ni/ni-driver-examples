r"""Steps:
1. Open a new RFmx Session.
2. Configure Selected Ports.
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Select ASK Modulation and M.
5. Configure DDemod Symbol Rate, Number of Symbols and Pulse Shaping Filter.
6. Configure DDemod Measurement Filter Type as Auto.
7. Configure DDemod Averaging.
8. Read DDemod Measurement Results.
9. Close the RFmx Session.
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
        demod.set_selected_ports("", selected_ports)
        demod.configure_rf("", center_frequency, reference_level, external_attenuation)
        demod.digital_demod.configuration.configure_modulation_type(
            "",
            nirfmxdemod.DDemodModulationType.ASK,
            nirfmxdemod.DDemodM.M4,
            nirfmxdemod.DDemodDifferentialEnabled.FALSE,
        )
        demod.digital_demod.configuration.configure_symbol_rate("", symbol_rate)
        demod.digital_demod.configuration.configure_number_of_symbols("", num_of_symbols)
        demod.digital_demod.configuration.configure_pulse_shaping_filter(
            "",
            nirfmxdemod.DDemodPulseShapingFilterType.ROOT_RAISED_COSINE,
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

        # Retrieve results
        mean_frequency_error, mean_rms_evm, max_peak_evm, mean_modulation_error_ratio, error_code = (
            demod.digital_demod.results.read("", timeout)
        )

        # Print Results
        print(f"Mean Carrier Frequency Error(Hz)       : {mean_frequency_error}")
        print(f"Mean Rms Evm(%)                        : {mean_rms_evm}")
        print(f"Maximum peak Evm(%)                    : {max_peak_evm}")
        print(f"Mean Modulation Error Ratio(dB)        : {mean_modulation_error_ratio}")

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
        description="Pass arguments for DDemod ASK Basic Example",
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
