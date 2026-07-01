"""
RFmx VNA Waves Example

Steps:
1. Open a new RFmx session.
2. Configure Frequency Reference.
3. Configure Sweep settings: Frequency List, IF Bandwidth, and Power Level and Test Rx Attenuation 
   with different port names.
4. Configure Averaging.
5. Select Wave measurement.
6. Configure Number of Waves.
7. Configure each Wave and Format.
8. Configure Magnitude Units & Phase Trace Type.
9. Initiate the Measurement.
10. Read the Num Waves.
11. Fetch Waves X data.
12. Fetch Waves Y data for each Wave.
13. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxvna
import numpy


def example(resource_name, option_string):
    """VNA waves example."""
    # Configuration parameters
    sweep_type = nirfmxvna.SweepType.LINEAR
    start_frequency = 1e9  # Hz
    stop_frequency = 10e9  # Hz
    frequency_points = 10
    port1_power_level = -10.0  # dBm
    port2_power_level = -10.0  # dBm
    port1_test_receiver_attenuation = 0.0  # dB
    port2_test_receiver_attenuation = 0.0  # dB
    if_bandwidth = 100.0e3  # Hz

    frequency_reference_source = "PXI_Clk"
    frequency_reference_frequency = 100e6  # Hz

    number_of_waves = 4
    waves = ["a1_1", "b1_1", "a1_2", "b1_2"]
    waves_formats = [
        nirfmxvna.WavesFormat.MAGNITUDE,
        nirfmxvna.WavesFormat.PHASE,
        nirfmxvna.WavesFormat.MAGNITUDE,
        nirfmxvna.WavesFormat.PHASE,
    ]

    magnitude_units = nirfmxvna.WavesMagnitudeUnits.DBM
    phase_trace_type = nirfmxvna.WavesPhaseTraceType.WRAPPED

    averaging_enabled = nirfmxvna.AveragingEnabled.FALSE
    averaging_count = 10

    measurement = nirfmxvna.MeasurementTypes.WAVES
    enable_all_traces = False

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

        vna_signal.set_sweep_type("", sweep_type)
        vna_signal.set_start_frequency("", start_frequency)
        vna_signal.set_stop_frequency("", stop_frequency)
        vna_signal.set_number_of_points("", frequency_points)
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

        vna_signal.set_averaging_enabled("", averaging_enabled)
        vna_signal.set_averaging_count("", averaging_count)

        vna_signal.select_measurements("", measurement, enable_all_traces)

        vna_signal.waves.configuration.set_number_of_waves("", number_of_waves)

        for i in range(number_of_waves):
            wave_selector_string = nirfmxvna.Vna.build_wave_string("", i)
            vna_signal.waves.configuration.configure_wave(wave_selector_string, waves[i])
            vna_signal.waves.configuration.set_format(wave_selector_string, waves_formats[i])

        vna_signal.waves.configuration.set_magnitude_units("", magnitude_units)
        vna_signal.waves.configuration.set_phase_trace_type("", phase_trace_type)

        vna_signal.initiate("", "")

        # Retrieve results
        number_of_waves_result, error_code = vna_signal.waves.configuration.get_number_of_waves("")

        waves_x_data_result, _ = vna_signal.waves.results.fetch_x_data("", timeout)

        waves_y1_data_result = []
        waves_y2_data_result = []

        for i in range(number_of_waves_result):
            wave_selector_string = nirfmxvna.Vna.build_wave_string("", i)

            y1_data, y2_data, error_code = vna_signal.waves.results.fetch_y_data(
                wave_selector_string, timeout
            )

            waves_y1_data_result.append(y1_data)
            waves_y2_data_result.append(y2_data)

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
        description="VNA Waves Example",
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
