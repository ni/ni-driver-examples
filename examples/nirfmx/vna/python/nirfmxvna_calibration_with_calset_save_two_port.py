"""
RFmx VNA Calibration with Calset Save Two Port Example

Steps:
1. Open a new RFmx session.
2. Configure Frequency Reference.
3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, 
   IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
4. Select S-Parameter measurement.
5. Configure Calibration Ports and Calibration Method.
6. Configure Connector type & vCal Resource Name for each VNA port.
7. Detect vCal ports connected to the VNA ports.
8. Initiate Calibration.
9. Acquire Calibration data after user confirmation.
10. Save Calibration data.
11. Save Calset data to a file.
12. Get Calset Error Terms.
12a. Calculate Magnitude of Error Terms.
13. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxvna
import numpy


def example(resource_name, option_string):
    """VNA calibration with calset save two port example."""
    # Configuration parameters
    frequency_start = 1e9  # Hz
    frequency_end = 26e9  # Hz
    num_of_frequency_points = 251
    port1_power_level = -10.0  # dBm
    port2_power_level = -10.0  # dBm
    port1_test_receiver_attenuation = 0.0  # dB
    port2_test_receiver_attenuation = 0.0  # dB
    if_bandwidth = 100e3  # Hz

    frequency_reference_source = "PXI_Clk"
    frequency_reference_frequency = 100e6  # Hz

    vcal_resource_name = "vCal"
    connector_type = "3.5 mm female"
    calibration_timeout = 100.0  # seconds
    auto_detect_vcal_orientation = True
    vcal_orientation = "PortA:Port1,PortB:Port2"

    calset_file_path = ""
    timeout = 10.0  # seconds

    instr_session = None
    vna_signal = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get VNA signal configuration
        vna_signal = instr_session.get_vna_signal_configuration()

        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )

        vna_signal.set_sweep_type("", nirfmxvna.SweepType.LINEAR)
        vna_signal.set_start_frequency("", frequency_start)
        vna_signal.set_stop_frequency("", frequency_end)
        vna_signal.set_number_of_points("", num_of_frequency_points)
        vna_signal.set_if_bandwidth("", if_bandwidth)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port1")
        vna_signal.set_power_level(port_selector_string, port1_power_level)
        vna_signal.set_test_receiver_attenuation(
            port_selector_string, port1_test_receiver_attenuation
        )

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port2")
        vna_signal.set_power_level(port_selector_string, port2_power_level)
        vna_signal.set_test_receiver_attenuation(
            port_selector_string, port2_test_receiver_attenuation
        )

        vna_signal.select_measurements("", nirfmxvna.MeasurementTypes.SPARAMS, False)

        vna_signal.set_correction_calibration_ports("", ["port1", "port2"])
        vna_signal.set_correction_calibration_method(
            "", nirfmxvna.CorrectionCalibrationMethod.SOLT
        )

        vna_signal.set_correction_calibration_connector_type("port::all", connector_type)
        vna_signal.set_correction_calibration_calkit_electronic_resource_name(
            "port::all", vcal_resource_name
        )

        if auto_detect_vcal_orientation:
            vna_signal.auto_detect_vcal_orientation("")
            vcal_orientation, error_code = vna_signal.get_correction_calibration_calkit_electronic_orientation(
                ""
            )
            print(f"vCal Orientation         :{vcal_orientation}")
        else:
            vna_signal.set_correction_calibration_calkit_electronic_orientation(
                "", vcal_orientation
            )

        vna_signal.calibration_initiate("")
        vna_signal.calibration_acquire("", calibration_timeout)
        vna_signal.calibration_save("", "")
        vna_signal.calset_save_to_file("", "", calset_file_path)

        # Retrieve error terms
        error_term_identifiers = [
            nirfmxvna.CalErrorTerm.DIRECTIVITY,
            nirfmxvna.CalErrorTerm.SOURCE_MATCH,
            nirfmxvna.CalErrorTerm.REFLECTION_TRACKING,
            nirfmxvna.CalErrorTerm.TRANSMISSION_TRACKING,
            nirfmxvna.CalErrorTerm.LOAD_MATCH,
        ]

        frequency_grid = numpy.empty(0, dtype=numpy.float64)
        frequency_grid = vna_signal.calset_get_frequency_grid(
            "", "", nirfmxvna.CalFrequencyGrid.DIRECTIVITY
        )

        for error_term_id in error_term_identifiers:
            error_terms_port1 = numpy.empty(len(frequency_grid), dtype=numpy.complex64)
            error_terms_port2 = numpy.empty(len(frequency_grid), dtype=numpy.complex64)

            error_terms_port1 = vna_signal.calset_get_error_term(
                "", "", error_term_id, "port1", "port2", error_terms_port1
            )
            error_terms_port2 = vna_signal.calset_get_error_term(
                "", "", error_term_id, "port2", "port1", error_terms_port2
            )

            error_terms = numpy.concatenate((error_terms_port1, error_terms_port2))

            # Calculate magnitude of error terms
            error_terms_magnitude = 20 * numpy.log10(numpy.abs(error_terms))

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
        description="VNA Calibration with Calset Save Two Port",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-r",
        "--resource",
        type=str,
        default="VNA",
        help="Resource name of the VNA device",
    )
    parser.add_argument(
        "-o",
        "--option-string",
        type=str,
        default="",
        help="Option string",
    )
    return parser.parse_args()


def main():
    """Main function."""
    args = _parse_args()
    example(args.resource, args.option_string)


if __name__ == "__main__":
    main()
