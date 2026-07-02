"""
RFmx VNA Guided Two-Port Calibration (Mechanical Calkit) Example

Steps:
1. Open a new RFmx session.
2. Configure Frequency Reference.
3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
4. Select S-Parameter measurement.
5. Configure Calibration Ports, Calibration Method and Thru.
6. Import Calkit File
7. Configure Connector type & Mechanical Calkit Name for each VNA port.
8. Initiate Calibration.
9. Acquire Calibration data after user confirmation.
10. Save Calibration data.
11. Save Calset data to a file.
12. Get Calset Frequency Grid.
13. Get Calset Error Terms.
13a. Calculate Magnitude of Error Terms
14. Close RFmx Session.
"""

import argparse
import math
import sys

import nirfmxinstr
import nirfmxvna


def example(resource_name, option_string, calkit_file_paths=None, calset_file_path=""):
    """VNA guided two-port calibration with mechanical calkit."""
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

    correction_calibration_method = nirfmxvna.CorrectionCalibrationMethod.SOLT
    correction_calibration_thru_method = nirfmxvna.CorrectionCalibrationThruMethod.AUTO
    calibration_timeout = 100.0  # seconds
    thru_coax_delay = math.nan  # seconds — NaN means don't set it

    calibration_ports = ["port1", "port2"]
    mechanical_calkit_name = ""
    connector_type = "3.5mm female"

    if calkit_file_paths is None:
        calkit_file_paths = [""]

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

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port1")
        vna_signal.set_power_level(port_selector_string, port1_power_level)
        vna_signal.set_test_receiver_attenuation(port_selector_string, port1_test_receiver_attenuation)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port2")
        vna_signal.set_power_level(port_selector_string, port2_power_level)
        vna_signal.set_test_receiver_attenuation(port_selector_string, port2_test_receiver_attenuation)

        vna_signal.select_measurements("", nirfmxvna.MeasurementTypes.SPARAMS, False)

        vna_signal.set_correction_calibration_ports("", calibration_ports)
        vna_signal.set_correction_calibration_method("", correction_calibration_method)
        vna_signal.set_correction_calibration_thru_method("", correction_calibration_thru_method)

        if not math.isnan(thru_coax_delay):
            vna_signal.set_correction_calibration_thru_coax_delay("", thru_coax_delay)

        for path in calkit_file_paths:
            vna_signal.calkit_manager_import_calkit("", path)

        for port_name in calibration_ports:
            port_selector_string = nirfmxvna.Vna.build_port_string("", port_name)
            vna_signal.set_correction_calibration_calkit_type(port_selector_string, nirfmxvna.CorrectionCalibrationCalkitType.MECHANICAL)
            vna_signal.set_correction_calibration_connector_type(port_selector_string, connector_type)
            vna_signal.set_correction_calibration_calkit_mechanical_name(port_selector_string, mechanical_calkit_name)

        vna_signal.calibration_initiate("")

        cal_step_count, _ = vna_signal.get_correction_calibration_step_count("")
        for i in range(cal_step_count):
            calstep_selector_string = nirfmxvna.Vna.build_calstep_string("", i)
            cal_step_description, _ = vna_signal.get_correction_calibration_step_description(calstep_selector_string)
            print(f"CalStep {i + 1} Description: {cal_step_description}")
            input("Press Enter for Next Cal Step...")
            vna_signal.calibration_acquire(calstep_selector_string, calibration_timeout)

        vna_signal.calibration_save("", "")
        vna_signal.calset_save_to_file("", "", calset_file_path)

        estimated_thru_delay, _ = vna_signal.get_correction_calibration_estimated_thru_delay("")
        print(f"Calibration Estimated Thru Delay: {estimated_thru_delay}")

        # Retrieve calset error terms — 10 error terms (2 ports × 5 types)
        frequency_grid = vna_signal.calset_get_frequency_grid("", "", nirfmxvna.CalFrequencyGrid.DIRECTIVITY)

        error_term_types = [
            nirfmxvna.CalErrorTerm.DIRECTIVITY,
            nirfmxvna.CalErrorTerm.DIRECTIVITY,
            nirfmxvna.CalErrorTerm.SOURCE_MATCH,
            nirfmxvna.CalErrorTerm.SOURCE_MATCH,
            nirfmxvna.CalErrorTerm.REFLECTION_TRACKING,
            nirfmxvna.CalErrorTerm.REFLECTION_TRACKING,
            nirfmxvna.CalErrorTerm.TRANSMISSION_TRACKING,
            nirfmxvna.CalErrorTerm.TRANSMISSION_TRACKING,
            nirfmxvna.CalErrorTerm.LOAD_MATCH,
            nirfmxvna.CalErrorTerm.LOAD_MATCH,
        ]
        port1_list = ["port1", "port2", "port1", "port2", "port1", "port2", "port1", "port2", "port1", "port2"]
        port2_list = ["port2", "port1", "port2", "port1", "port2", "port1", "port2", "port1", "port2", "port1"]

        for i in range(10):
            error_term_data = vna_signal.calset_get_error_term(
                "", "", error_term_types[i], port1_list[i], port2_list[i]
            )
            # Calculate magnitude (dB) for first element as a spot check
            if len(error_term_data) > 0:
                val = error_term_data[0]
                magnitude_db = 20.0 * math.log10(math.sqrt(val.real ** 2 + val.imag ** 2))

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
        description="VNA Guided Two-Port Calibration (Mechanical Calkit) Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-r", "--resource", type=str, default="VNA", help="Resource name of the VNA device")
    parser.add_argument("-o", "--option-string", type=str, default="", help="Option string")
    parser.add_argument("-cs", "--calset-file-path", type=str, default="", help="Path to save calset file")
    parser.add_argument("-ck", "--calkit-file-paths", type=str, nargs="*", default=[""], help="Calkit file path(s) to import")
    return parser.parse_args()


def main():
    args = _parse_args()
    example(args.resource, args.option_string, args.calkit_file_paths, args.calset_file_path)


if __name__ == "__main__":
    main()
