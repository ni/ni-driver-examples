"""
RFmx VNA S-Parameters Advanced Example

Steps:
1. Open a new RFmx session.
2. Configure Frequency Reference.
3. Configure S-parameter External Attenuation Table (External Fixture's De-embedding Table) from S2P File.
4. Configure Port Extension.
5 & 6. Configure sweep settings: Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
7. Configure Averaging.
8. Configure Trigger.
9. Select S-Parameter measurement.
10. Configure number of S-Parameters.
11. Configure S-Parameter and format.
12. Configure Magnitude Units, Phase Trace Type & Group Delay Aperture Settings.
13. Load Calset data from a file.
14. Enable Correction, Configure Interpolation Enabled and Configure correction port subset settings.
15. Initiate the Measurement after user confirmation.
16. Read Number of SParams.
17. Fetch S-Parameter X data.
18. Fetch S-Parameter Y data for each S-Parameter.
19. Fetch S-Parameter Correction Level.
20. Fetch S-Parameter Correction State.
21. Set SnP Export attributes (can be accessed and written before or after measurement initiate) and save S-Parameter data to file.
22. Close RFmx Session.
"""

import argparse
import os
import sys

import nirfmxinstr
import nirfmxvna
import numpy

_SUPPORT_DIR = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Support")


def example(resource_name, option_string, calset_file_path="", snp_file_path=""):
    """VNA S-parameters advanced example."""
    frequency_list_size = 251
    frequency_step = 100e6  # Hz
    sweep_type = nirfmxvna.SweepType.LINEAR
    frequency_start = 1e9  # Hz
    frequency_stop = 26e9  # Hz
    number_of_frequency_points = 251
    port1_power_level = -10.0  # dBm
    port2_power_level = -10.0  # dBm
    port1_test_receiver_attenuation = 0.0  # dB
    port2_test_receiver_attenuation = 0.0  # dB
    if_bandwidth = 100.0e3  # Hz

    frequency_reference_source = "PXI_Clk"
    frequency_reference_frequency = 100e6  # Hz

    # External fixture de-embedding
    port_names = ["port1", "port2"]
    s2p_file_paths = [
        os.path.join(_SUPPORT_DIR, "1dB_Attenuation.s2p"),
        os.path.join(_SUPPORT_DIR, "1dB_Attenuation.s2p"),
    ]
    s_parameter_orientations = [
        nirfmxinstr.SParameterOrientation.PORT2_TOWARDS_DUT,
        nirfmxinstr.SParameterOrientation.PORT2_TOWARDS_DUT,
    ]

    # Port extension
    port_extension_enabled = nirfmxvna.CorrectionPortExtensionEnabled.FALSE
    port_extension_delay_domain = nirfmxvna.CorrectionPortExtensionDelayDomain.DELAY
    port_extension_delay = 100.0e-12  # s
    port_extension_distance = 29.9792e-3
    port_extension_distance_unit = nirfmxvna.CorrectionPortExtensionDistanceUnit.METERS
    port_extension_velocity_factor = 1.0
    port_extension_dc_loss_enabled = nirfmxvna.CorrectionPortExtensionDCLossEnabled.FALSE
    port_extension_dc_loss = 0.0  # dB
    port_extension_loss1_enabled = nirfmxvna.CorrectionPortExtensionLoss1Enabled.FALSE
    port_extension_loss2_enabled = nirfmxvna.CorrectionPortExtensionLoss2Enabled.FALSE
    port_extension_loss1_frequency = 0.0  # Hz
    port_extension_loss2_frequency = 0.0  # Hz
    port_extension_loss1 = 0.0  # dB
    port_extension_loss2 = 0.0  # dB

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
    group_delay_aperture_mode = nirfmxvna.SParamsGroupDelayApertureMode.POINTS
    group_delay_aperture_points = 11.0
    group_delay_aperture_percentage = 4.0   # %
    group_delay_aperture_frequency_span = 1.0e9  # Hz

    interpolation_enabled = nirfmxvna.CorrectionInterpolationEnabled.TRUE
    port_subset_enabled = nirfmxvna.CorrectionPortSubsetEnabled.FALSE

    averaging_enabled = nirfmxvna.AveragingEnabled.FALSE
    averaging_count = 10

    trigger_type = nirfmxvna.TriggerType.NONE
    trigger_mode = nirfmxvna.TriggerMode.SIGNAL
    trigger_delay = 0.0  # s

    timeout = 10.0  # s

    instr_session = None
    vna_signal = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        vna_signal = instr_session.get_vna_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)

        # Load external fixture S-parameter de-embedding tables
        for i, port_name in enumerate(port_names):
            port_selector_string = nirfmxvna.Vna.build_port_string("", port_name)
            instr_session.load_s_parameter_external_attenuation_table_from_s2p_file(
                port_selector_string, "", s2p_file_paths[i], s_parameter_orientations[i]
            )

        # Configure port extension for each port
        for i, port_name in enumerate(port_names):
            port_selector_string = nirfmxvna.Vna.build_port_string("", port_name)
            vna_signal.set_correction_port_extension_enabled(port_selector_string, port_extension_enabled)
            vna_signal.set_correction_port_extension_delay_domain(port_selector_string, port_extension_delay_domain)
            vna_signal.set_correction_port_extension_delay(port_selector_string, port_extension_delay)
            vna_signal.set_correction_port_extension_distance(port_selector_string, port_extension_distance)
            vna_signal.set_correction_port_extension_distance_unit(port_selector_string, port_extension_distance_unit)
            vna_signal.set_correction_port_extension_velocity_factor(port_selector_string, port_extension_velocity_factor)
            vna_signal.set_correction_port_extension_dc_loss_enabled(port_selector_string, port_extension_dc_loss_enabled)
            vna_signal.set_correction_port_extension_loss_dc_loss(port_selector_string, port_extension_dc_loss)
            vna_signal.set_correction_port_extension_loss1_enabled(port_selector_string, port_extension_loss1_enabled)
            vna_signal.set_correction_port_extension_loss2_enabled(port_selector_string, port_extension_loss2_enabled)
            vna_signal.set_correction_port_extension_loss1_frequency(port_selector_string, port_extension_loss1_frequency)
            vna_signal.set_correction_port_extension_loss2_frequency(port_selector_string, port_extension_loss2_frequency)
            vna_signal.set_correction_port_extension_loss1(port_selector_string, port_extension_loss1)
            vna_signal.set_correction_port_extension_loss2(port_selector_string, port_extension_loss2)

        # Configure sweep
        if sweep_type == nirfmxvna.SweepType.LIST:
            vna_signal.set_sweep_type("", sweep_type)
            frequency_list = numpy.array(
                [frequency_start + i * frequency_step for i in range(frequency_list_size)], dtype=numpy.float64
            )
            vna_signal.set_frequency_list("", frequency_list)
        else:
            vna_signal.set_sweep_type("", sweep_type)
            vna_signal.set_start_frequency("", frequency_start)
            vna_signal.set_stop_frequency("", frequency_stop)
            vna_signal.set_number_of_points("", number_of_frequency_points)

        vna_signal.set_if_bandwidth("", if_bandwidth)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port1")
        vna_signal.set_power_level(port_selector_string, port1_power_level)
        vna_signal.set_test_receiver_attenuation(port_selector_string, port1_test_receiver_attenuation)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port2")
        vna_signal.set_power_level(port_selector_string, port2_power_level)
        vna_signal.set_test_receiver_attenuation(port_selector_string, port2_test_receiver_attenuation)

        vna_signal.set_averaging_enabled("", averaging_enabled)
        vna_signal.set_averaging_count("", averaging_count)
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
        vna_signal.s_params.configuration.set_group_delay_aperture_mode("", group_delay_aperture_mode)
        vna_signal.s_params.configuration.set_group_delay_aperture_points("", group_delay_aperture_points)
        vna_signal.s_params.configuration.set_group_delay_aperture_percentage("", group_delay_aperture_percentage)
        vna_signal.s_params.configuration.set_group_delay_aperture_frequency_span("", group_delay_aperture_frequency_span)

        vna_signal.calset_load_from_file("", "", calset_file_path)
        vna_signal.set_correction_enabled("", nirfmxvna.CorrectionEnabled.TRUE)
        vna_signal.set_correction_interpolation_enabled("", interpolation_enabled)
        vna_signal.set_correction_port_subset_enabled("", port_subset_enabled)
        vna_signal.set_correction_port_subset_full_ports("", "port1,port2")
        vna_signal.set_correction_port_subset_response_ports("", "")

        vna_signal.initiate("", "")

        number_of_sparams_result, _ = vna_signal.s_params.configuration.get_number_of_s_parameters("")
        vna_signal.s_params.results.fetch_x_data("", timeout)

        for i in range(number_of_sparams_result):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)
            vna_signal.s_params.results.fetch_y_data(sparam_selector_string, timeout)
            correction_level_result, _ = vna_signal.s_params.results.get_correction_level(sparam_selector_string)

        correction_state_result, _ = vna_signal.s_params.results.get_correction_state("")

        vna_signal.s_params.configuration.set_snp_data_format("", nirfmxvna.SParamsSnPDataFormat.AUTO)
        vna_signal.s_params.configuration.set_snp_ports("", "port1,port2")
        vna_signal.s_params.configuration.export_to_snp_file("", snp_file_path)

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
        description="VNA S-Parameters Advanced Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-r", "--resource", type=str, default="VNA", help="Resource name of the VNA device")
    parser.add_argument("-o", "--option-string", type=str, default="", help="Option string")
    parser.add_argument("-cs", "--calset-file-path", type=str, default="", help="Path to calset file")
    return parser.parse_args()


def main():
    args = _parse_args()
    example(args.resource, args.option_string, args.calset_file_path)


if __name__ == "__main__":
    main()
