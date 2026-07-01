r"""Steps:
1. Open a new RFmx session.
2. Configure Selected Ports.
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure CCDF Measurement Interval.
5. Configure CCDF RBW.
6. Read CCDF Measurement Results.
7. Close the RFmx Session.
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
    rbw = 100.0e3  # Hz
    enable_all_traces = True
    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.CCDF, enable_all_traces)
        specan.ccdf.configuration.configure_measurement_interval("", measurement_interval)
        specan.ccdf.configuration.set_rbw_filter_bandwidth("", rbw)

        mean_power, mean_power_percentile, peak_power, measured_samples_count, error_code = (
            specan.ccdf.results.read("", timeout)
        )

        print(f"Mean Power (dBm)              {mean_power}")
        print(f"Mean Power Percentile (%)     {mean_power_percentile}")
        print(f"Peak Power (dB)               {peak_power}")
        print(f"Measured Samples Count        {measured_samples_count}")

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
        description="Pass arguments for CCDF Basic Example",
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
