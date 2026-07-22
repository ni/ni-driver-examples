r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source and Clock Frequency).
3. Configure S-parameter External Attenuation Table.
4. Configure External Attenuation Interpolation.
5. Configure S-parameter External Attenuation Type.
6. Configure Selected Ports.
7. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
8. Configure IQ Power Edge Trigger properties.
9. Configure TXP measurement and enable the traces.
10. Configure the Measurement Interval.
11. Configure RBW filter parameters.
12. Configure Thresholding.
13. Configure Averaging parameters.
14. Configure VBW filter parameters.
15. Initiate Measurement.
16. Fetch TXP Traces and Measurements.
17. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr

FREQUENCY_ARRAY_SIZE = 3


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9    # Hz
    reference_level = 0.0       # dBm
    external_attenuation = 0.0  # dB

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    iq_power_edge_level = -20.0   # dBm
    trigger_delay = 0.0           # seconds
    minimum_quiet_time = 0.0      # seconds
    enable_trigger = False

    # S-parameter external attenuation table
    table_name = ""
    format_ = nirfmxinstr.LinearInterpolationFormat.MAGNITUDE_AND_PHASE
    s_parameter_orientation = nirfmxinstr.SParameterOrientation.PORT1_TOWARDS_DUT
    s_parameter_type = nirfmxinstr.SParameterType.SCALAR

    frequency_array = numpy.array([997.0e6, 1.0e9, 1.003e9], dtype=numpy.float64)  # Hz

    s_parameters = numpy.array([
        [
            [complex(1.00, 0.00), complex(1.00, 0.10)],
            [complex(1.00, 0.10), complex(1.00, 0.00)]
        ],
        [
            [complex(0.80, 0.10), complex(0.80, 0.25)],
            [complex(0.80, 0.25), complex(0.80, 0.10)]
        ],
        [
            [complex(1.00, 0.25), complex(1.00, 0.50)],
            [complex(1.00, 0.50), complex(1.00, 0.25)]
        ]
    ], dtype=numpy.complex128)

    measurement_interval = 1.0e-3  # seconds

    rbw_filter_type = nirfmxspecan.TxpRbwFilterType.GAUSSIAN
    rbw = 100.0e3   # Hz
    rrc_alpha = 0.010

    vbw_auto = nirfmxspecan.TxpVbwFilterAutoBandwidth.TRUE
    vbw = 30.0e3       # Hz
    vbw_to_rbw_ratio = 3

    averaging_enabled = nirfmxspecan.TxpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.TxpAveragingType.RMS

    threshold_enabled = nirfmxspecan.TxpThresholdEnabled.FALSE
    threshold_type = nirfmxspecan.TxpThresholdType.RELATIVE
    threshold_level = -20.0  # dB or dBm

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_source, frequency)

        port_string = nirfmxinstr.Session.build_port_string("", selected_ports, "", 0)
        instr_session.configure_s_parameter_external_attenuation_table(
            port_string, table_name, frequency_array, s_parameters, s_parameter_orientation
        )
        instr_session.configure_external_attenuation_interpolation_linear(port_string, table_name, format_)
        instr_session.configure_s_parameter_external_attenuation_type(port_string, s_parameter_type)

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
            enable_trigger,
        )
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.TXP, True)
        specan.txp.configuration.configure_measurement_interval("", measurement_interval)
        specan.txp.configuration.configure_rbw_filter("", rbw, rbw_filter_type, rrc_alpha)
        specan.txp.configuration.configure_threshold("", threshold_enabled, threshold_level, threshold_type)
        specan.txp.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.txp.configuration.configure_vbw_filter("", vbw_auto, vbw, vbw_to_rbw_ratio)
        specan.initiate("", "")

        power = numpy.empty(0, dtype=numpy.float32)
        error_code = specan.txp.results.fetch_power_trace("", timeout, power)

        average_mean_power, peak_to_average_ratio, maximum_power, minimum_power, error_code = (
            specan.txp.results.fetch_measurement("", timeout)
        )

        print("---------------Measurement---------------")
        print(f"Average Mean Power  (dBm)      : {average_mean_power}")
        print(f"Peak to Average Ratio(dB)      : {peak_to_average_ratio}")
        print(f"Maximum Power (dBm)            : {maximum_power}")
        print(f"Minimum Power (dBm)            : {minimum_power}")

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
        description="Pass arguments for S-Parameter External Attenuation Table Example",
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
