r"""Steps:
1. Open a new RFmx session.
2. Create a Named signal configuration ("TxP_Signal").
3. Configure the basic instrument properties (Clock Source and Clock Frequency).
4. Configure Selected Ports.
5. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
6. Configure TXP measurement and enable the traces.
7. Configure the Measurement Interval.
8. Configure RBW filter parameters.
9. Configure Averaging parameters.
10. Initiate Measurement with a named result ("TxP_Result").
11. Fetch TXP Traces and Measurements using the result name.
12. Close the RFmx Session.
"""

import argparse
import sys

import numpy
import nirfmxspecan

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9    # Hz
    reference_level = 0.0       # dBm
    external_attenuation = 0.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference = 10.0e6  # Hz

    measurement_interval = 1.0e-3  # seconds

    rbw_filter_type = nirfmxspecan.TxpRbwFilterType.GAUSSIAN
    rbw = 100.0e3  # Hz
    rrc_alpha = 0.010

    averaging_enabled = nirfmxspecan.TxpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.TxpAveragingType.RMS

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration("TxP_Signal")

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference)
        specan.set_selected_ports("", selected_ports)
        specan.configure_external_attenuation("", external_attenuation)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.TXP, True)
        specan.txp.configuration.configure_measurement_interval("", measurement_interval)
        specan.txp.configuration.configure_rbw_filter("", rbw, rbw_filter_type, rrc_alpha)
        specan.txp.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)

        result_name = nirfmxspecan.SpecAn.build_result_string("TxP_Result")
        specan.initiate("", result_name)

        power_trace = numpy.empty(0, dtype=numpy.float32)
        specan.txp.results.fetch_power_trace(result_name, timeout, power_trace)

        average_mean_power, peak_to_average_ratio, max_power, min_power, error_code = (
            specan.txp.results.fetch_measurement(result_name, timeout)
        )

        print(f"Average Mean Frequency (Hz)    {average_mean_power}")
        print(f"Mean Phase (deg)               {peak_to_average_ratio}")
        print(f"Maximum Power (dBm)            {max_power}")
        print(f"Minimum Power (dBm)            {min_power}")

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
        description="Pass arguments for TXP With Signal And Result Name Example",
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
