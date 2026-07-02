r"""Steps:
1. Open a new RFmx session.
2. Configure Selected Ports.
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Harmonics RBW.
5. Configure Harmonics Measurement Interval.
6. Configure Number of Harmonics.
7. Configure Harmonics Averaging.
8. Read Harmonics Measurement Results.
9. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1e9   # Hz
    reference_level = 0.00   # dBm
    external_attenuation = 0.00  # dB

    rbw = 100.0e3  # Hz
    measurement_interval = 1.0e-3  # seconds
    rbw_filter_type = nirfmxspecan.HarmRbwFilterType.GAUSSIAN
    rrc_alpha = 0.1

    number_of_harmonics = 3

    averaging_enabled = nirfmxspecan.HarmAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.HarmAveragingType.RMS

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.harm.configuration.configure_fundamental_rbw("", rbw, rbw_filter_type, rrc_alpha)
        specan.harm.configuration.configure_fundamental_measurement_interval("", measurement_interval)
        specan.harm.configuration.configure_number_of_harmonics("", number_of_harmonics)
        specan.harm.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)

        total_harmonic_distortion, average_fundamental_power, error_code = (
            specan.harm.results.read("", timeout)
        )

        print(f"Total Harmonic Distortion (%)    {total_harmonic_distortion}")
        print(f"Average Fundamental Power (dBm)  {average_fundamental_power}")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        if specan is not None:
            specan.dispose()
            specan = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    parser = argparse.ArgumentParser(
        description="Pass arguments for Harmonics Basic Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr.")
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string)


def main():
    _main(sys.argv[1:])


def test_main():
    _main(["--option-string", ""])


def test_example():
    example("RFSA", "")


if __name__ == "__main__":
    main()
