"""
RFmx VNA Segment Sweep Example

Steps:
1. Open a new RFmx session.
2. Configure Frequency Reference.
3. Configure sweep settings: IF Bandwidth, Power Level and Test Rx Attenuation with different port names.
4. Configure Sweep Type as Segment.Configure Number of Segments and Independent Settings enabled per segment.
5. Configure per segment settings like Segment Enabled, Start and Stop Frequencies, Number of Frequency points, Segment IF Bandwidth, Segment Dwell Time, Segment Power Level and Segment Test Receiver Attenuation.
   For the properties where the Segment <property> Enabled was set to True in Step 4, values configured in Step 5 are used. If they were set to False, values configured in Step 3 are used.
6. Configure Trigger settings.
7. Select S-Parameter measurement.
8. Configure number of S-Parameters.
9. Configure each S-Parameter and format.
10. Configure Magnitude Units & Phase Trace Type.
11. Configure Calibration Ports and Calibration Method
12. Configure Connector type & vCal Resource Name for each VNA port
13. Initiate Calibration
14. Acquire Calibration data after user confirmation
15. Save Calibration data
16. Enable Correction
17. Initiate the Measurement after user confirmation
18. Read Number of SParams.
19. Fetch S-Parameter Correction State.
20. Fetch S-Parameter X data.
21. Fetch S-Parameter Y data for each S-Parameter.
22. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxvna


def example(resource_name, option_string):
    """VNA segment sweep example."""
    port1_power_level = -10.0  # dBm
    port2_power_level = -10.0  # dBm
    port1_test_receiver_attenuation = 0.0  # dB
    port2_test_receiver_attenuation = 0.0  # dB
    if_bandwidth = 100.0e3  # Hz

    segment_power_level_enabled = nirfmxvna.SegmentPowerLevelEnabled.TRUE
    segment_if_bandwidth_enabled = nirfmxvna.SegmentIFBandwidthEnabled.TRUE
    segment_test_receiver_attenuation_enabled = nirfmxvna.SegmentTestReceiverAttenuationEnabled.TRUE
    segment_dwell_time_enabled = nirfmxvna.SegmentDwellTimeEnabled.TRUE

    number_of_segments = 5
    segment_enabled = [
        nirfmxvna.SegmentEnabled.TRUE,
        nirfmxvna.SegmentEnabled.TRUE,
        nirfmxvna.SegmentEnabled.TRUE,
        nirfmxvna.SegmentEnabled.TRUE,
        nirfmxvna.SegmentEnabled.TRUE,
    ]
    segment_start_frequency = [1.0e9, 5.1e9, 10.1e9, 15.1e9, 20.1e9]  # Hz
    segment_stop_frequency = [5.0e9, 10.0e9, 15.0e9, 20.0e9, 26.5e9]  # Hz
    segment_number_of_frequency_points = [41, 50, 50, 50, 65]
    segment_if_bandwidth = [100.0e3, 1.0e6, 100.0e3, 10.0e3, 100.0e3]  # Hz
    segment_dwell_time = [0.0, 0.0, 0.0, 0.0, 0.0]  # s
    port1_segment_power_level = [-10.0, -10.0, -10.0, -10.0, -10.0]  # dBm
    port2_segment_power_level = [-10.0, -10.0, -10.0, -10.0, -10.0]  # dBm
    port1_segment_test_receiver_attenuation = [0.0, 0.0, 0.0, 0.0, 0.0]  # dB
    port2_segment_test_receiver_attenuation = [0.0, 0.0, 0.0, 0.0, 0.0]  # dB

    trigger_type = nirfmxvna.TriggerType.NONE
    trigger_mode = nirfmxvna.TriggerMode.SEGMENT
    trigger_delay = 0.0  # s

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

    vcal_resource_name = "vCal"
    connector_type = "3.5 mm female"
    calibration_timeout = 100.0  # s

    timeout = 10.0  # s

    instr_session = None
    vna_signal = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        vna_signal = instr_session.get_vna_signal_configuration()

        instr_session.configure_frequency_reference("", "PXI_Clk", 100e6)

        vna_signal.set_if_bandwidth("", if_bandwidth)
        port_selector_string = nirfmxvna.Vna.build_port_string("", "port1")
        vna_signal.set_power_level(port_selector_string, port1_power_level)
        vna_signal.set_test_receiver_attenuation(port_selector_string, port1_test_receiver_attenuation)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port2")
        vna_signal.set_power_level(port_selector_string, port2_power_level)
        vna_signal.set_test_receiver_attenuation(port_selector_string, port2_test_receiver_attenuation)

        vna_signal.set_sweep_type("", nirfmxvna.SweepType.SEGMENT)
        vna_signal.set_number_of_segments("", number_of_segments)
        vna_signal.set_segment_power_level_enabled("", segment_power_level_enabled)
        vna_signal.set_segment_if_bandwidth_enabled("", segment_if_bandwidth_enabled)
        vna_signal.set_segment_test_receiver_attenuation_enabled("", segment_test_receiver_attenuation_enabled)
        vna_signal.set_segment_dwell_time_enabled("", segment_dwell_time_enabled)

        for i in range(number_of_segments):
            segment_selector_string = nirfmxvna.Vna.build_segment_string("", i)
            vna_signal.set_segment_enabled(segment_selector_string, segment_enabled[i])
            vna_signal.set_segment_start_frequency(segment_selector_string, segment_start_frequency[i])
            vna_signal.set_segment_stop_frequency(segment_selector_string, segment_stop_frequency[i])
            vna_signal.set_segment_number_of_frequency_points(segment_selector_string, segment_number_of_frequency_points[i])
            vna_signal.set_segment_if_bandwidth(segment_selector_string, segment_if_bandwidth[i])
            vna_signal.set_segment_dwell_time(segment_selector_string, segment_dwell_time[i])

            port_selector_string = nirfmxvna.Vna.build_port_string(segment_selector_string, "port1")
            vna_signal.set_segment_power_level(port_selector_string, port1_segment_power_level[i])
            vna_signal.set_segment_test_receiver_attenuation(port_selector_string, port1_segment_test_receiver_attenuation[i])

            port_selector_string = nirfmxvna.Vna.build_port_string(segment_selector_string, "port2")
            vna_signal.set_segment_power_level(port_selector_string, port2_segment_power_level[i])
            vna_signal.set_segment_test_receiver_attenuation(port_selector_string, port2_segment_test_receiver_attenuation[i])

        vna_signal.set_trigger_type("", trigger_type)
        vna_signal.set_trigger_mode("", trigger_mode)
        vna_signal.set_trigger_delay("", trigger_delay)

        vna_signal.select_measurements("", nirfmxvna.MeasurementTypes.SPARAMS, False)
        vna_signal.s_params.configuration.set_number_of_s_parameters("", number_of_sparams)
        for i in range(number_of_sparams):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)
            vna_signal.s_params.configuration.configure_s_parameter(sparam_selector_string, sparams_parameters[i])
            vna_signal.s_params.configuration.set_format(sparam_selector_string, sparams_formats[i])

        vna_signal.s_params.configuration.set_magnitude_units("", magnitude_units)
        vna_signal.s_params.configuration.set_phase_trace_type("", phase_trace_type)

        vna_signal.set_correction_calibration_ports("", ["port1", "port2"])
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
        correction_state_result, _ = vna_signal.s_params.results.get_correction_state("")
        vna_signal.s_params.results.fetch_x_data("", timeout)
        for i in range(number_of_sparams_result):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)
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
        description="VNA Segment Sweep Example",
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
