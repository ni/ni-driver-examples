"""
RFmx VNA CW Time Sweep Example

Steps:
1. Open a new RFmx session.
2. Configure Frequency Reference.
3. Configure sweep settings: Sweep Type = CW Time, Number of Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
4. Select S-Parameter measurement.
5. Configure number of S-Parameters.
6. Configure each S-Parameter and format.
7. Configure Magnitude Units & Phase Trace Type.
8. Configure Calibration Ports and Calibration Method
9. Configure Connector type & vCal Resource Name for each VNA port
10. Initiate Calibration
11. Acquire Calibration data after user confirmation
12. Save Calibration data
13. Enable Correction
14. Initiate the Measurement after user confirmation
15. Read Number of SParams.
16. Fetch S-Parameter X data.
17. Fetch S-Parameter Y data for each S-Parameter.
18. Fetch S-Parameter Correction State.
19. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxvna


def example(resource_name, option_string):
    """VNA CW time sweep example."""
    cw_frequency = 1e9  # Hz
    number_of_points = 100
    if_bandwidth = 100e3  # Hz
    port1_power_level = -10.0  # dBm
    port2_power_level = -10.0  # dBm
    port1_test_receiver_attenuation = 0.0  # dB
    port2_test_receiver_attenuation = 0.0  # dB

    frequency_reference_source = "PXI_Clk"
    frequency_reference_frequency = 100e6  # Hz

    number_of_sparams = 4
    sparams_parameters = ["S11", "S12", "S21", "S22"]
    sparams_formats = [
        nirfmxvna.SParamsFormat.MAGNITUDE,
        nirfmxvna.SParamsFormat.MAGNITUDE,
        nirfmxvna.SParamsFormat.MAGNITUDE,
        nirfmxvna.SParamsFormat.MAGNITUDE,
    ]

    magnitude_units = nirfmxvna.SParamsMagnitudeUnits.DB
    phase_trace_type = nirfmxvna.SParamsPhaseTraceType.WRAPPED

    calibration_ports = ["port1", "port2"]
    vcal_resource_name = "vCal"
    connector_type = "3.5 mm female"
    calibration_timeout = 100.0  # s

    timeout = 10.0  # s

    instr_session = None
    vna_signal = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        vna_signal = instr_session.get_vna_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)

        vna_signal.set_sweep_type("", nirfmxvna.SweepType.CW_TIME)
        vna_signal.set_cw_frequency("", cw_frequency)
        vna_signal.set_number_of_points("", number_of_points)
        vna_signal.set_if_bandwidth("", if_bandwidth)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port1")
        vna_signal.set_power_level(port_selector_string, port1_power_level)
        vna_signal.set_test_receiver_attenuation(port_selector_string, port1_test_receiver_attenuation)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port2")
        vna_signal.set_power_level(port_selector_string, port2_power_level)
        vna_signal.set_test_receiver_attenuation(port_selector_string, port2_test_receiver_attenuation)

        vna_signal.select_measurements("", nirfmxvna.MeasurementTypes.SPARAMS, False)
        vna_signal.s_params.configuration.set_number_of_s_parameters("", number_of_sparams)

        for i in range(number_of_sparams):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)
            vna_signal.s_params.configuration.configure_s_parameter(sparam_selector_string, sparams_parameters[i])
            vna_signal.s_params.configuration.set_format(sparam_selector_string, sparams_formats[i])

        vna_signal.s_params.configuration.set_magnitude_units("", magnitude_units)
        vna_signal.s_params.configuration.set_phase_trace_type("", phase_trace_type)

        vna_signal.set_correction_calibration_ports("", calibration_ports)
        vna_signal.set_correction_calibration_method("", nirfmxvna.CorrectionCalibrationMethod.SOLT)
        vna_signal.set_correction_calibration_connector_type("port::all", connector_type)
        vna_signal.set_correction_calibration_calkit_electronic_resource_name("port::all", vcal_resource_name)

        vna_signal.calibration_initiate("")
        print("Connect Port A of NI CAL-5501 to Port 1 of NI PXIe-5633,  and Port B of NI CAL-5501 to Port 2 of NI PXIe-5633.")
        input("Press any key to continue.")
        vna_signal.calibration_acquire("", calibration_timeout)
        vna_signal.calibration_save("", "")
        print("Connect DUT across port1 and port2 of NI PXIe-5633.")
        input("Press any key to continue.")

        vna_signal.set_correction_enabled("", nirfmxvna.CorrectionEnabled.TRUE)
        vna_signal.initiate("", "")

        number_of_sparams_result, _ = vna_signal.s_params.configuration.get_number_of_s_parameters("")
        vna_signal.s_params.results.fetch_x_data("", timeout)

        for i in range(number_of_sparams_result):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)
            vna_signal.s_params.results.fetch_y_data(sparam_selector_string, timeout)

        correction_state_result, _ = vna_signal.s_params.results.get_correction_state("")
        print(f"Correction State             : {correction_state_result}")

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
        description="VNA CW Time Sweep Example",
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
