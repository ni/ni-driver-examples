r"""Steps:
1. Open a new RFmx Session.
2. Configure Selected Ports.
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure ADemod RBW, Measurement Interval, FM DeEmphasis and Averaging.
5. Read ADemod FM Measurement Results.
6. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxdemod
import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    # Initialize input variables
    selected_ports = ""
    center_frequency = 1e9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.00  # dB

    measurement_interval = 10.00e-3  # seconds
    de_emphasis = 0.0
    rbw = 100.00e3  # Hz
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
        demod.analog_demod.configuration.configure_rbw_filter(
            "", rbw, nirfmxdemod.ADemodRbwFilterType.FLAT, 0.1
        )
        demod.analog_demod.configuration.configure_measurement_interval("", measurement_interval)
        demod.analog_demod.configuration.configure_fm_de_emphasis("", de_emphasis)
        demod.analog_demod.configuration.configure_averaging(
            "", nirfmxdemod.ADemodAveragingEnabled.FALSE, averaging_count, nirfmxdemod.ADemodAveragingType.LINEAR
        )

        # Retrieve results
        mean_deviation, mean_carrier_frequency_error, error_code = demod.analog_demod.results.read_fm(
            "", timeout
        )

        # Print Results
        print(f"Mean Deviation(deg)               : {mean_deviation}")
        print(f"Mean Carrier Frequency Error(Hz)  : {mean_carrier_frequency_error}")

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
        description="Pass arguments for ADemod FM Basic Example",
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
