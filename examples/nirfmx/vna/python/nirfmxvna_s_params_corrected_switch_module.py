"""
RFmx VNA S-Parameters Corrected (Switch Module) Example

Steps:
1. Open a new RFmx session.
2. Configure Frequency Reference.
3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points,
   IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
4. Configure Averaging.
5. Select S-Parameter measurement.
6. Configure number of S-Parameters.
7. Configure each S-Parameter and format.
8. Configure Magnitude Units & Phase Trace Type.
9. Configure Calibration Ports and Calibration Method
10. Configure Connector type & vCal Resource Name for each VNA port
11. Initiate Calibration
12. Read the Calstep Description for connection information.
13. Acquire Calibration data after user confirmation
14. Save Calibration data
15. Enable Correction
16. Initiate the Measurement after user confirmation
17. Read Number of SParams.
18. Fetch S-Parameter X data.
19. Fetch S-Parameter Y data for each S-Parameter.
20. Fetch S-Parameter Correction State.
21. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxvna


def example(resource_name, option_string):
    """VNA S-parameters corrected with switch module example."""
    frequency_start = 1e9  # Hz 
    frequency_end = 26e9  # Hz
    number_of_frequency_points = 251
    port_names = ["rmm0/port0", "rmm0/port1"]
    power_level = [-10.0, -10.0]  # dBm
    test_receiver_attenuation = [0.0, 0.0]  # dB
    if_bandwidth = 100e3  # Hz

    frequency_reference_source = "PXI_Clk"
    frequency_reference_frequency = 100e6  # Hz

    number_of_sparams = 4
    sparams_receiver_ports = ["rmm0/port0", "rmm0/port0", "rmm0/port1", "rmm0/port1"]
    sparams_source_ports = ["rmm0/port0", "rmm0/port1", "rmm0/port0", "rmm0/port1"]
    sparams_formats = [
        nirfmxvna.SParamsFormat.MAGNITUDE,
        nirfmxvna.SParamsFormat.MAGNITUDE,
        nirfmxvna.SParamsFormat.MAGNITUDE,
        nirfmxvna.SParamsFormat.MAGNITUDE,
    ]

    magnitude_units = nirfmxvna.SParamsMagnitudeUnits.DB
    phase_trace_type = nirfmxvna.SParamsPhaseTraceType.WRAPPED

    averaging_enabled = nirfmxvna.AveragingEnabled.FALSE
    averaging_count = 10

    calibration_ports = ["rmm0/port0", "rmm0/port1"]
    vcal_orientation = "portA:rmm0/port0,portB:rmm0/port1"
    vcal_resource_name = "vCal"
    connector_type = "3.5 mm female"
    calibration_timeout = 100.0  # seconds

    timeout = 10.0  # seconds

    instr_session = None
    vna_signal = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        vna_signal = instr_session.get_vna_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)

        vna_signal.set_sweep_type("", nirfmxvna.SweepType.LINEAR)
        vna_signal.set_start_frequency("", frequency_start)
        vna_signal.set_stop_frequency("", frequency_end)
        vna_signal.set_number_of_points("", number_of_frequency_points)
        vna_signal.set_if_bandwidth("", if_bandwidth)

        for i, port_name in enumerate(port_names):
            port_selector_string = nirfmxvna.Vna.build_port_string("", port_name)
            vna_signal.set_power_level(port_selector_string, power_level[i])
            vna_signal.set_test_receiver_attenuation(port_selector_string, test_receiver_attenuation[i])

        vna_signal.set_averaging_enabled("", averaging_enabled)
        vna_signal.set_averaging_count("", averaging_count)

        vna_signal.select_measurements("", nirfmxvna.MeasurementTypes.SPARAMS, False)

        vna_signal.s_params.configuration.set_number_of_s_parameters("", number_of_sparams)

        for i in range(number_of_sparams):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)
            vna_signal.s_params.configuration.set_receiver_port(sparam_selector_string, sparams_receiver_ports[i])
            vna_signal.s_params.configuration.set_source_port(sparam_selector_string, sparams_source_ports[i])
            vna_signal.s_params.configuration.set_format(sparam_selector_string, sparams_formats[i])

        vna_signal.s_params.configuration.set_magnitude_units("", magnitude_units)
        vna_signal.s_params.configuration.set_phase_trace_type("", phase_trace_type)

        vna_signal.set_correction_calibration_ports("", calibration_ports)
        vna_signal.set_correction_calibration_method("", nirfmxvna.CorrectionCalibrationMethod.SOLT)
        vna_signal.set_correction_calibration_calkit_electronic_orientation("", vcal_orientation)

        vna_signal.set_correction_calibration_connector_type("port::all", connector_type)
        vna_signal.set_correction_calibration_calkit_electronic_resource_name("port::all", vcal_resource_name)

        vna_signal.calibration_initiate("")
        connection_instruction, _ = vna_signal.get_correction_calibration_step_description("")
        print(connection_instruction)
        input("Press any key to continue.")
        vna_signal.calibration_acquire("", calibration_timeout)
        vna_signal.calibration_save("", "")
        print("Connect DUT across the specified measurement ports")
        input("Press any key to continue.")

        vna_signal.set_correction_enabled("", nirfmxvna.CorrectionEnabled.TRUE)
        vna_signal.initiate("", "")

        number_of_sparams_result, _ = vna_signal.s_params.configuration.get_number_of_s_parameters("")
        vna_signal.s_params.results.fetch_x_data("", timeout)

        for i in range(number_of_sparams_result):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)
            vna_signal.s_params.results.fetch_y_data(sparam_selector_string, timeout)

        correction_state_result, _ = vna_signal.s_params.results.get_correction_state("")
        print(f"Correction State: {correction_state_result}")

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
        description="VNA S-Parameters Corrected (Switch Module) Example",
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
