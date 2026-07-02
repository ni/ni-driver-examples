r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure IQ Power Edge Trigger properties.
6. Configure TXP measurement and enable the traces.
7. Configure Measurement Interval.
8. Configure RBW Filter parameters.
9. Configure Thresholding.
10. Configure Averaging parameters.
11. Configure VBW Filter parameters.
12. Initiate Measurement.
13. Fetch TXP Traces and Measurements.
14. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1e9   # Hz
    reference_level = 0.00   # dBm
    external_attenuation = 0.00  # dB

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    iq_power_edge_enabled = False
    iq_power_edge_level = -20.0  # dBm
    trigger_delay = 0.0  # seconds
    minimum_quiet_time = 0.0  # seconds

    measurement_interval = 1.0e-3  # seconds

    rbw_filter_type = nirfmxspecan.TxpRbwFilterType.GAUSSIAN
    rbw = 100.0e3  # Hz
    rrc_alpha = 0.010

    vbw_auto = nirfmxspecan.TxpVbwFilterAutoBandwidth.TRUE
    vbw = 30.0e3  # Hz
    vbw_to_rbw_ratio = 3

    threshold_enabled = nirfmxspecan.TxpThresholdEnabled.FALSE
    threshold_type = nirfmxspecan.TxpThresholdType.RELATIVE
    threshold_level = -20.0  # dB or dBm

    averaging_enabled = nirfmxspecan.TxpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.TxpAveragingType.RMS

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_frequency("", center_frequency)
        specan.configure_reference_level("", reference_level)
        specan.configure_external_attenuation("", external_attenuation)
        specan.configure_iq_power_edge_trigger(
            "", "0", iq_power_edge_level,
            nirfmxspecan.IQPowerEdgeTriggerSlope.RISING_SLOPE,
            trigger_delay,
            nirfmxspecan.TriggerMinimumQuietTimeMode.MANUAL,
            minimum_quiet_time,
            iq_power_edge_enabled,
        )
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.TXP, True)
        specan.txp.configuration.configure_measurement_interval("", measurement_interval)
        specan.txp.configuration.configure_rbw_filter("", rbw, rbw_filter_type, rrc_alpha)
        specan.txp.configuration.configure_threshold("", threshold_enabled, threshold_level, threshold_type)
        specan.txp.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.txp.configuration.configure_vbw_filter("", vbw_auto, vbw, vbw_to_rbw_ratio)
        specan.initiate("", "")

        power = numpy.empty(0, dtype=numpy.float32)
        specan.txp.results.fetch_power_trace("", timeout, power)

        average_mean_power, peak_to_average_ratio, maximum_power, minimum_power, error_code = (
            specan.txp.results.fetch_measurement("", timeout)
        )

        print(f"Average Mean Power (dBm)      : {average_mean_power}")
        print(f"Peak to Average Ratio(dB)     : {peak_to_average_ratio}")
        print(f"Maximum Power (dBm)           : {maximum_power}")
        print(f"Minimum Power (dBm)           : {minimum_power}")

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
        description="Pass arguments for TXP IQ Device Example",
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
