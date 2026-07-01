r"""Steps:
1. Open a new RFmx session.
2. Configure Selected Ports.
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure FCnt Measurement Interval.
5. Configure FCnt Averaging.
6. Configure FCnt RBW.
7. Read FCnt Measurement Results.
8. Close the RFmx Session.
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
    measurement_interval = 1.0e-3  # seconds
    timeout = 10.0  # seconds

    averaging_enabled = nirfmxspecan.FcntAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.FcntAveragingType.MEAN

    rbw = 100.0e3  # Hz
    rrc_alpha = 0.1
    rbw_filter_type = nirfmxspecan.FcntRbwFilterType.NONE

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.fcnt.configuration.configure_measurement_interval("", measurement_interval)
        specan.fcnt.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.fcnt.configuration.configure_rbw_filter("", rbw, rbw_filter_type, rrc_alpha)

        average_relative_frequency, average_absolute_frequency, mean_phase, error_code = (
            specan.fcnt.results.read("", timeout)
        )

        print(f"Average Relative Frequency (Hz)  {average_relative_frequency}")
        print(f"Average Absolute Frequency (Hz)  {average_absolute_frequency}")
        print(f"Mean Phase (deg)                 {mean_phase}")

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
        description="Pass arguments for FCnt Basic Example",
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
