"""
RFmx VNA S-Parameters and Waves Composite Example

Steps:
1. Open a new RFmx session.
2. Configure Frequency Reference.
3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
4. Configure Averaging.
5. Select S-Parameter and Waves measurements.
6. Configure number of S-Parameters.
7. Configure each S-Parameter and Format.
8. Configure Magnitude Units & Phase Trace Type.
9. Configure Number of Waves.
10. Configure each Wave and Format.
11. Configure Magnitude Units & Phase Trace Type.
12. Initiate the Measurement.
13. Read Number of SParams and X-Axis Values (aggregated frequency list).
14. Fetch S-Parameter X data.
15. Fetch S-Parameter Y data for each S-Parameter.
16. Read the Num Waves.
17. Fetch Waves X data.
18. Fetch Waves Y Data for each Wave.
19. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxvna


def example(resource_name, option_string):
    """VNA S-parameters and waves composite example."""
    start_frequency = 1e9  # Hz
    stop_frequency = 26e9  # Hz
    frequency_points = 251
    port1_power_level = -10.0  # dBm
    port2_power_level = -10.0  # dBm
    port1_test_receiver_attenuation = 0.0  # dB
    port2_test_receiver_attenuation = 0.0  # dB
    if_bandwidth = 100.0e3  # Hz

    frequency_reference_source = "PXI_Clk"
    frequency_reference_frequency = 100e6  # Hz

    averaging_enabled = nirfmxvna.AveragingEnabled.FALSE
    averaging_count = 10

    number_of_sparams = 4
    sparams_parameters = ["S11", "S12", "S21", "S22"]
    sparams_formats = [
        nirfmxvna.SParamsFormat.MAGNITUDE,
        nirfmxvna.SParamsFormat.MAGNITUDE,
        nirfmxvna.SParamsFormat.MAGNITUDE,
        nirfmxvna.SParamsFormat.MAGNITUDE,
    ]
    sparams_magnitude_units = nirfmxvna.SParamsMagnitudeUnits.DB
    sparams_phase_trace_type = nirfmxvna.SParamsPhaseTraceType.WRAPPED

    number_of_waves = 4
    waves = ["a1_1", "b1_1", "a1_2", "b1_2"]
    waves_formats = [
        nirfmxvna.WavesFormat.MAGNITUDE,
        nirfmxvna.WavesFormat.PHASE,
        nirfmxvna.WavesFormat.MAGNITUDE,
        nirfmxvna.WavesFormat.PHASE,
    ]
    waves_magnitude_units = nirfmxvna.WavesMagnitudeUnits.DBM
    waves_phase_trace_type = nirfmxvna.WavesPhaseTraceType.WRAPPED

    timeout = 10.0  # s

    instr_session = None
    vna_signal = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        vna_signal = instr_session.get_vna_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)

        vna_signal.set_sweep_type("", nirfmxvna.SweepType.LINEAR)
        vna_signal.set_start_frequency("", start_frequency)
        vna_signal.set_stop_frequency("", stop_frequency)
        vna_signal.set_number_of_points("", frequency_points)
        vna_signal.set_if_bandwidth("", if_bandwidth)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port1")
        vna_signal.set_power_level(port_selector_string, port1_power_level)
        vna_signal.set_test_receiver_attenuation(port_selector_string, port1_test_receiver_attenuation)

        port_selector_string = nirfmxvna.Vna.build_port_string("", "port2")
        vna_signal.set_power_level(port_selector_string, port2_power_level)
        vna_signal.set_test_receiver_attenuation(port_selector_string, port2_test_receiver_attenuation)

        vna_signal.set_averaging_enabled("", averaging_enabled)
        vna_signal.set_averaging_count("", averaging_count)

        # Select both SParams and Waves in one call using bitwise OR
        vna_signal.select_measurements(
            "", nirfmxvna.MeasurementTypes.SPARAMS | nirfmxvna.MeasurementTypes.WAVES, False
        )

        # Configure S-Parameters
        vna_signal.s_params.configuration.set_number_of_s_parameters("", number_of_sparams)
        for i in range(number_of_sparams):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)
            vna_signal.s_params.configuration.configure_s_parameter(sparam_selector_string, sparams_parameters[i])
            vna_signal.s_params.configuration.set_format(sparam_selector_string, sparams_formats[i])
        vna_signal.s_params.configuration.set_magnitude_units("", sparams_magnitude_units)
        vna_signal.s_params.configuration.set_phase_trace_type("", sparams_phase_trace_type)

        # Configure Waves
        vna_signal.waves.configuration.set_number_of_waves("", number_of_waves)
        for i in range(number_of_waves):
            wave_selector_string = nirfmxvna.Vna.build_wave_string("", i)
            vna_signal.waves.configuration.configure_wave(wave_selector_string, waves[i])
            vna_signal.waves.configuration.set_format(wave_selector_string, waves_formats[i])
        vna_signal.waves.configuration.set_magnitude_units("", waves_magnitude_units)
        vna_signal.waves.configuration.set_phase_trace_type("", waves_phase_trace_type)

        vna_signal.initiate("", "")

        # Fetch S-Parameter results
        number_of_sparams_result, _ = vna_signal.s_params.configuration.get_number_of_s_parameters("")
        vna_signal.s_params.results.fetch_x_data("", timeout)
        for i in range(number_of_sparams_result):
            sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)
            vna_signal.s_params.results.fetch_y_data(sparam_selector_string, timeout)

        # Fetch Waves results
        number_of_waves_result, _ = vna_signal.waves.configuration.get_number_of_waves("")
        vna_signal.waves.results.fetch_x_data("", timeout)
        for i in range(number_of_waves_result):
            wave_selector_string = nirfmxvna.Vna.build_wave_string("", i)
            vna_signal.waves.results.fetch_y_data(wave_selector_string, timeout)

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
        description="VNA S-Parameters and Waves Composite Example",
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
