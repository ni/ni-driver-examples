r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select Harmonics measurement and enable the traces.
6. Configure RBW Filter parameters for the Fundamental signal.
7. Configure Measurement Interval of the Fundamental signal.
8. Configure Number of Harmonics and Auto Harmonics Setup.
9. Configure Harmonic Array (Order, Bandwidth, Enabled, Measurement Interval).
10. Configure Averaging parameters.
11. Initiate Measurement.
12. Fetch Harm Measurements and Traces.
13. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr

NUMBER_OF_HARMONICS = 3


def example(resource_name, option_string):
    """Run Example."""
    # Initialize input variables
    selected_ports = ""
    center_frequency = 1.0e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB
    timeout = 10.0  # seconds

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    rbw_filter_type = nirfmxspecan.HarmRbwFilterType.GAUSSIAN
    rbw = 100.0e3  # Hz
    rrc_alpha = 0.010
    measurement_interval = 1.0e-3  # seconds

    auto_harmonics_setup = nirfmxspecan.HarmAutoHarmonicsSetupEnabled.TRUE

    harmonics_order = numpy.array(list(range(1, NUMBER_OF_HARMONICS + 1)), dtype=numpy.int32)
    harmonics_bandwidth = numpy.array([100.0e3] * NUMBER_OF_HARMONICS)  # Hz
    harmonics_enabled = numpy.array(
        [nirfmxspecan.HarmHarmonicEnabled.TRUE.value] * NUMBER_OF_HARMONICS, dtype=numpy.int32
    )
    harmonics_measurement_interval = numpy.array([1.0e-3] * NUMBER_OF_HARMONICS)  # seconds

    averaging_enabled = nirfmxspecan.HarmAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.HarmAveragingType.RMS

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
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.HARMONICS, True)
        specan.harm.configuration.configure_fundamental_rbw("", rbw, rbw_filter_type, rrc_alpha)
        specan.harm.configuration.configure_fundamental_measurement_interval("", measurement_interval)
        specan.harm.configuration.configure_auto_harmonics("", auto_harmonics_setup)
        specan.harm.configuration.configure_number_of_harmonics("", NUMBER_OF_HARMONICS)
        if auto_harmonics_setup != nirfmxspecan.HarmAutoHarmonicsSetupEnabled.TRUE:
            specan.harm.configuration.configure_harmonic_array(
                "",
                harmonics_order,
                harmonics_bandwidth,
                harmonics_enabled,
                harmonics_measurement_interval,
            )
        specan.harm.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.initiate("", "")

        # Retrieve results
        total_harmonic_distortion, average_fundamental_power, fundamental_frequency, error_code = (
            specan.harm.results.fetch_total_harmonic_distortion("", timeout)
        )

        average_relative_power, average_absolute_power, harmonics_rbw, harmonics_frequency, error_code = (
            specan.harm.results.fetch_harmonic_measurement_array("", timeout)
        )

        for i in range(NUMBER_OF_HARMONICS):
            harmonic_string = nirfmxspecan.SpecAn.build_harmonic_string("", i)
            power = numpy.empty(0, dtype=numpy.float32)
            specan.harm.results.fetch_harmonic_power_trace(harmonic_string, timeout, power)

        # Print results
        print(f"Total Harmonic Distortion (%)    : {total_harmonic_distortion}")
        print(f"Average Fundamental Power (dBm)  : {average_fundamental_power}")
        print(f"Fundamental Frequency (Hz)       : {fundamental_frequency}\n")

        print("-----------------Harmonics----------------------\n")
        for i in range(NUMBER_OF_HARMONICS):
            print(f"Harmonic {i + 1}")
            print(f"  Harmonics Frequency (Hz)       : {harmonics_frequency[i]}")
            print(f"  Harmonics RBW (Hz)             : {harmonics_rbw[i]}")
            print(f"  Average Absolute Power (dBm)   : {average_absolute_power[i]}")
            print(f"  Average Relative Power (dB)    : {average_relative_power[i]}")
            print("----------------------------------------")

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
        description="Pass arguments for Harmonics Example",
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
