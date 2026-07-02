"""
RFmx VNA External Fixtures De-embedding Example

Steps:
1. Open a new RFmx session.
2. Configure Frequency Reference.
3. Configure S-parameter External Attenuation Table (External Fixture's De-embedding Table) from S2P File.
4. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
5. Configure Averaging.
6. Select S-Parameter measurement.
7. Configure number of S-Parameters.
8. Configure each S-Parameter and format.
9. Configure Magnitude Units & Phase Trace Type.
11. Load Calset data from a file.
12. Enable Correction.
13. Initiate the Measurement after user confirmation.
14. Read Number of SParams.
15. Fetch S-Parameter X data.
16. Fetch S-Parameter Y data for each S-Parameter.
17. Close RFmx Session.

Note:
    The S2P files for external fixtures must exist in the same directory as this script.
    Place 1dB_Attenuation.s2p (or equivalent fixture files) in the Support/ subfolder and
    update s2p_file_paths accordingly.
"""

import argparse
import os
import sys

import nirfmxinstr
import nirfmxvna

_SUPPORT_DIR = os.path.join(os.path.dirname(os.path.abspath(__file__)), "Support")


def example(resource_name, option_string, calset_file_path=""):
    """VNA external fixtures de-embedding example."""
    frequency_start = 1e9  # Hz
    frequency_end = 26e9  # Hz
    number_of_frequency_points = 251
    port1_power_level = -10.0  # dBm
    port2_power_level = -10.0  # dBm
    port1_test_receiver_attenuation = 0.0  # dB
    port2_test_receiver_attenuation = 0.0  # dB
    if_bandwidth = 100e3  # Hz

    frequency_reference_source = "PXI_Clk"
    frequency_reference_frequency = 100e6  # Hz

    port_names = ["port1", "port2"]
    s2p_file_paths = [
        os.path.join(_SUPPORT_DIR, "1dB_Attenuation.s2p"),
        os.path.join(_SUPPORT_DIR, "1dB_Attenuation.s2p"),
    ]
    s_parameter_orientations = [
        nirfmxinstr.SParameterOrientation.PORT2_TOWARDS_DUT,
        nirfmxinstr.SParameterOrientation.PORT2_TOWARDS_DUT,
    ]

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

    averaging_enabled = nirfmxvna.AveragingEnabled.FALSE
    averaging_count = 10

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

        vna_signal.set_sweep_type("", nirfmxvna.SweepType.LINEAR)
        vna_signal.set_start_frequency("", frequency_start)
        vna_signal.set_stop_frequency("", frequency_end)
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

        vna_signal.select_measurements("", nirfmxvna.MeasurementTypes.SPARAMS, False)
        vna_signal.s_params.configuration.set_number_of_s_parameters("", number_of_sparams)

        for i in range(number_of_sparams):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)
            vna_signal.s_params.configuration.configure_s_parameter(sparam_selector_string, sparams_parameters[i])
            vna_signal.s_params.configuration.set_format(sparam_selector_string, sparams_formats[i])

        vna_signal.s_params.configuration.set_magnitude_units("", magnitude_units)
        vna_signal.s_params.configuration.set_phase_trace_type("", phase_trace_type)

        vna_signal.calset_load_from_file("", "", calset_file_path)
        vna_signal.set_correction_enabled("", nirfmxvna.CorrectionEnabled.TRUE)
        vna_signal.initiate("", "")

        number_of_sparams_result, _ = vna_signal.s_params.configuration.get_number_of_s_parameters("")
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
        description="VNA External Fixtures De-embedding Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-r", "--resource", type=str, default="VNA", help="Resource name of the VNA device")
    parser.add_argument("-o", "--option-string", type=str, default="", help="Option string")
    parser.add_argument("-cs", "--calset-file-path", type=str, default="", help="Path to calset file (empty = no pre-loaded calset)")
    return parser.parse_args()


def main():
    args = _parse_args()
    example(args.resource, args.option_string, args.calset_file_path)


if __name__ == "__main__":
    main()
