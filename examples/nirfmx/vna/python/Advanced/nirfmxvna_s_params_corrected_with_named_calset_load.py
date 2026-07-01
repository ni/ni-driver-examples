"""
RFmx VNA S-Parameters Corrected with Named Calset Load Example

Steps:
1. Open a new RFmx session.
2. Configure Frequency Reference.
3. Create a named signal instance.
4. Configure sweep settings: Frequency List, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
5. Select S-Parameter measurement.
6. Configure number of S-Parameters.
7. Configure each S-Parameter and format.
8. Configure Magnitude Units & Phase Trace Type.
9. Load one or more Calset data from files(s) to create a global pool of named calsets.
10. Select a named calset from the global pool to set as active calset for the specified signal.
11. Enable Correction.
12. Initiate the Measurement after user confirmation.
13. Read Number of SParams and X-Axis Values (aggregated frequency list).
14. Fetch S-Parameter X data.
15. Fetch S-Parameter Y data for each S-Parameter.
16. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxvna


def example(resource_name, option_string, calset_file_paths=None):
    """VNA S-parameters corrected with named calset load example."""
    frequency_list_size = 251
    frequency_start = 1e9  # Hz
    frequency_stop = 26e9  # Hz
    port1_power_level = -10.0  # dBm
    port2_power_level = -10.0  # dBm
    port1_test_receiver_attenuation = 0.0  # dB
    port2_test_receiver_attenuation = 0.0  # dB
    if_bandwidth = 100e3  # Hz

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

    # Three calsets in the global pool; Signal1 will use Calset1
    calset_names = ["Calset1", "Calset2", "Calset3"]
    if calset_file_paths is None:
        calset_file_paths = ["", "", ""]

    timeout = 10.0  # s

    instr_session = None
    vna_signal = None
    vna_signal1 = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        vna_signal = instr_session.get_vna_signal_configuration()

        # Create a named signal instance
        vna_signal1, _ = vna_signal.clone_signal_configuration("Signal1")

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)

        vna_signal1.set_sweep_type("", nirfmxvna.SweepType.LINEAR)
        vna_signal1.set_start_frequency("", frequency_start)
        vna_signal1.set_stop_frequency("", frequency_stop)
        vna_signal1.set_number_of_points("", frequency_list_size)
        vna_signal1.set_if_bandwidth("", if_bandwidth)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port1")
        vna_signal1.set_power_level(port_selector_string, port1_power_level)
        vna_signal1.set_test_receiver_attenuation(port_selector_string, port1_test_receiver_attenuation)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port2")
        vna_signal1.set_power_level(port_selector_string, port2_power_level)
        vna_signal1.set_test_receiver_attenuation(port_selector_string, port2_test_receiver_attenuation)

        vna_signal1.select_measurements("", nirfmxvna.MeasurementTypes.SPARAMS, False)
        vna_signal1.s_params.configuration.set_number_of_s_parameters("", number_of_sparams)

        for i in range(number_of_sparams):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)
            vna_signal1.s_params.configuration.configure_s_parameter(sparam_selector_string, sparams_parameters[i])
            vna_signal1.s_params.configuration.set_format(sparam_selector_string, sparams_formats[i])

        vna_signal1.s_params.configuration.set_magnitude_units("", magnitude_units)
        vna_signal1.s_params.configuration.set_phase_trace_type("", phase_trace_type)

        # Load each calset into the global pool with its name
        for i, calset_name in enumerate(calset_names):
            vna_signal.calset_load_from_file("", calset_name, calset_file_paths[i])

        # Select the active calset for Signal1
        vna_signal1.select_active_calset("", "Calset1", nirfmxvna.RestoreConfiguration.NONE)

        vna_signal1.set_correction_enabled("", nirfmxvna.CorrectionEnabled.TRUE)
        vna_signal1.initiate("", "")

        number_of_sparams_result, _ = vna_signal1.s_params.configuration.get_number_of_s_parameters("")
        vna_signal1.s_params.results.fetch_x_data("", timeout)

        for i in range(number_of_sparams_result):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)
            vna_signal1.s_params.results.fetch_y_data(sparam_selector_string, timeout)

    except Exception as e:
        print(f"ERROR: {e}")
        sys.exit(1)

    finally:
        if vna_signal1 is not None:
            vna_signal1.dispose()
            vna_signal1 = None
        if vna_signal is not None:
            vna_signal.dispose()
            vna_signal = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _parse_args():
    parser = argparse.ArgumentParser(
        description="VNA S-Parameters Corrected with Named Calset Load Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-r", "--resource", type=str, default="VNA", help="Resource name of the VNA device")
    parser.add_argument("-o", "--option-string", type=str, default="", help="Option string")
    parser.add_argument("-cs", "--calset-file-paths", type=str, nargs=3, default=["", "", ""],
                        metavar=("CALSET1", "CALSET2", "CALSET3"), help="Three calset file paths")
    return parser.parse_args()


def main():
    args = _parse_args()
    example(args.resource, args.option_string, args.calset_file_paths)


if __name__ == "__main__":
    main()
