"""
RFmx VNA S-Parameters Trace Math Example

Steps:
1. Open a new RFmx session.
2. Configure Frequency Reference.
3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
4. Configure Averaging.
5. Select S-Parameter measurement.
6. Configure S-Parameter and format for selected S-Parameters.
7. Configure Magnitude Units & Phase Trace Type.
8. Initiate the Measurement.
9. Copy Measurement Data to Memory and Get Memory Data.
10. Configure Math Function.
11. Initiate the Measurement for Applying Math Function.
12. Read X-Axis Values (aggregated frequency list).
13. Fetch Math Applied S-Parameter Y data.
14. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxvna


def example(resource_name, option_string):
    """VNA S-parameters trace math example."""
    start_frequency = 1e9  # Hz
    stop_frequency = 26e9  # Hz
    number_of_points = 251
    port1_power_level = -10.0  # dBm
    port2_power_level = -10.0  # dBm
    port1_test_receiver_attenuation = 0.0  # dB
    port2_test_receiver_attenuation = 0.0  # dB
    if_bandwidth = 100.0e3  # Hz

    sparams_parameter = "S11"
    sparams_format = nirfmxvna.SParamsFormat.MAGNITUDE

    memory_name = "Memory0"
    math_function = nirfmxvna.SParamsMathFunction.DIVIDE

    averaging_enabled = nirfmxvna.AveragingEnabled.FALSE
    averaging_count = 10

    timeout = 10.0  # s

    instr_session = None
    vna_signal = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        vna_signal = instr_session.get_vna_signal_configuration()

        instr_session.configure_frequency_reference("", "PXI_Clk", 100e6)

        vna_signal.set_sweep_type("", nirfmxvna.SweepType.LINEAR)
        vna_signal.set_start_frequency("", start_frequency)
        vna_signal.set_stop_frequency("", stop_frequency)
        vna_signal.set_number_of_points("", number_of_points)
        vna_signal.set_if_bandwidth("", if_bandwidth)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port1")
        vna_signal.set_power_level(port_selector_string, port1_power_level)
        vna_signal.set_test_receiver_attenuation(port_selector_string, port1_test_receiver_attenuation)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port2")
        vna_signal.set_power_level(port_selector_string, port2_power_level)
        vna_signal.set_test_receiver_attenuation(port_selector_string, port2_test_receiver_attenuation)

        vna_signal.set_averaging_enabled("", averaging_enabled)
        vna_signal.set_averaging_count("", averaging_count)

        vna_signal.select_measurements("", nirfmxvna.MeasurementTypes.SPARAMS, False)

        sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", 0)
        vna_signal.s_params.configuration.configure_s_parameter(sparam_selector_string, sparams_parameter)
        vna_signal.s_params.configuration.set_format(sparam_selector_string, sparams_format)
        vna_signal.s_params.configuration.set_magnitude_units("", nirfmxvna.SParamsMagnitudeUnits.DB)
        vna_signal.s_params.configuration.set_phase_trace_type("", nirfmxvna.SParamsPhaseTraceType.WRAPPED)

        # First measurement — capture baseline into memory
        vna_signal.initiate("", "")

        vna_signal.copy_data_to_measurement_memory(sparam_selector_string, memory_name)
        memory_selector_string = nirfmxvna.Vna.build_measurement_memory_string(sparam_selector_string, memory_name)
        vna_signal.get_measurement_memory_x_data(memory_selector_string)
        vna_signal.get_measurement_memory_y_data(memory_selector_string)

        # Apply math function and re-initiate
        vna_signal.s_params.configuration.set_math_function(sparam_selector_string, math_function)
        vna_signal.initiate("", "")

        vna_signal.s_params.results.fetch_x_data("", timeout)
        vna_signal.s_params.results.fetch_y_data(sparam_selector_string, timeout)

    except Exception as e:
        print(f"ERROR: {e}")
        sys.exit(1)

    finally:
        if vna_signal is not None:
            vna_signal.dispose()
            vna_signal = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _parse_args():
    parser = argparse.ArgumentParser(
        description="VNA S-Parameters Trace Math Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-r", "--resource", type=str, default="VNA", help="Resource name of the VNA device")
    parser.add_argument("-o", "--option-string", type=str, default="", help="Option string")
    return parser.parse_args()


def main():
    args = _parse_args()
    example(args.resource, args.option_string)


if __name__ == "__main__":
    main()
