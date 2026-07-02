"""
RFmx VNA Pulsed S-Parameters Example

Steps:
1. Open a new RFmx session.
2. Configure Frequency Reference.
3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, 
   IF Bandwidth, and Power Level with different port names.
4. Configure Pulse Settings.
5. Configure Averaging.
6. Select S-Parameter measurement.
7. Configure number of S-Parameters.
8. Configure each S-Parameter and format.
9. Initiate the Measurement.
10. Read Number of SParams.
11. Fetch S-Parameter X data.
12. Fetch S-Parameter Y data for each S-Parameter.
13. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxvna


def example(resource_name, option_string):
    """VNA pulsed S-parameters example."""
    # Configuration parameters
    frequency_start = 1e9  # Hz
    frequency_end = 26e9  # Hz
    number_of_frequency_points = 251
    port1_power_level = -10.0  # dBm
    port2_power_level = -10.0  # dBm

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

    pulse_mode_enabled = nirfmxvna.PulseModeEnabled.TRUE
    pulse_period = 1.0e-3  # seconds
    pulse_modulator_delay = 0.0  # seconds
    pulse_modulator_width = 100.0e-6  # seconds
    pulse_acquisition_auto = nirfmxvna.PulseAcquisitionAuto.TRUE
    pulse_acquisition_delay = 20.0e-6  # seconds
    pulse_acquisition_width = 46.65e-6  # seconds

    averaging_enabled = nirfmxvna.AveragingEnabled.FALSE
    averaging_count = 10

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
        vna_signal.set_number_of_points("", number_of_frequency_points)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port1")
        vna_signal.set_power_level(port_selector_string, port1_power_level)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port2")
        vna_signal.set_power_level(port_selector_string, port2_power_level)

        vna_signal.set_pulse_mode_enabled("", pulse_mode_enabled)
        vna_signal.set_pulse_period("", pulse_period)
        vna_signal.set_pulse_modulator_delay("", pulse_modulator_delay)
        vna_signal.set_pulse_modulator_width("", pulse_modulator_width)
        vna_signal.set_pulse_acquisition_auto("", pulse_acquisition_auto)
        vna_signal.set_pulse_acquisition_delay("", pulse_acquisition_delay)
        vna_signal.set_pulse_acquisition_width("", pulse_acquisition_width)

        vna_signal.set_averaging_enabled("", averaging_enabled)
        vna_signal.set_averaging_count("", averaging_count)

        vna_signal.select_measurements("", nirfmxvna.MeasurementTypes.SPARAMS, False)

        vna_signal.s_params.configuration.set_number_of_s_parameters("", number_of_sparams)

        for i in range(number_of_sparams):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)
            vna_signal.s_params.configuration.configure_s_parameter(
                sparam_selector_string, sparams_parameters[i]
            )
            vna_signal.s_params.configuration.set_format(
                sparam_selector_string, sparams_formats[i]
            )

        vna_signal.initiate("", "")

        # Retrieve results
        number_of_sparams_result, error_code = vna_signal.s_params.configuration.get_number_of_s_parameters("")

        sparams_x_data_result, _ = vna_signal.s_params.results.fetch_x_data("", timeout)

        sparams_y1_data_result = []
        sparams_y2_data_result = []

        for i in range(number_of_sparams_result):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)

            y1_data, y2_data, error_code = vna_signal.s_params.results.fetch_y_data(
                sparam_selector_string, timeout
            )

            sparams_y1_data_result.append(y1_data)
            sparams_y2_data_result.append(y2_data)

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
        description="VNA Pulsed S-Parameters Example",
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
