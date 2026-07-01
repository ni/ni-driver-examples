"""
RFmx VNA Marker Example

Steps:
1. Open a new RFmx session.
2. Configure Frequency Reference.
3. Configure sweep settings: Frequency List, IF Bandwidth, and Power Level and Test Rx Attenuation with different port names.
4. Configure Averaging.
5. Select S-Parameter measurement.
6. Configure S-Parameter and Format.
7. Configure Magnitude Units & Phase Trace Type.
8. Initiate the Measurement.
9. Read X-Axis Values (aggregated frequency list).
10. Fetch S-Parameter Y data.
11. Configure Marker Type as Normal.
12. Configure Marker Data Source as 'sparam0' to be used by the Marker.
13. Configure Marker Peak Threshold.
14. Configure Marker Peak Excursion.
15. Perform Peak Search on the configured data source.
16. Fetch X value of the Marker.
17. Fetch Y value of the Marker.
18. Based on the user selection move the Marker to Next Peak, Next Left Peak and Next Right Peak  position and Fetch X and Y value of the Marker after moving the Marker to new position. Stop the Loop on Error or if the user has pressed the Stop button.
19. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxvna


def _display_marker_data(marker_x, marker_y1, marker_y2):
    print("---------------------------------------------------")
    print(f"Marker X : {marker_x}")
    print(f"Marker Y1: {marker_y1}")
    print(f"Marker Y2: {marker_y2}")
    print("---------------------------------------------------\n")


def example(resource_name, option_string):
    """VNA marker example."""
    start_frequency = 1e9  # Hz
    stop_frequency = 26e9  # Hz
    number_of_points = 251
    port1_power_level = -10.0  # dBm
    port2_power_level = -10.0  # dBm
    port1_test_receiver_attenuation = 0.0  # dB
    port2_test_receiver_attenuation = 0.0  # dB
    if_bandwidth = 100.0e3  # Hz

    sparams_parameter = "S11"
    sparams_format = nirfmxvna.SParamsFormat.MAGNITUDE

    averaging_enabled = nirfmxvna.AveragingEnabled.FALSE
    averaging_count = 10

    threshold_enabled = nirfmxvna.MarkerPeakSearchThresholdEnabled.FALSE
    threshold = -100.0

    excursion_enabled = nirfmxvna.MarkerPeakSearchExcursionEnabled.FALSE
    excursion = 3.0

    timeout = 10.0  # seconds

    instr_session = None
    vna_signal = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        vna_signal = instr_session.get_vna_signal_configuration()

        instr_session.configure_frequency_reference("", "PXI_Clk", 100e6)

        vna_signal.set_sweep_type("", nirfmxvna.SweepType.LINEAR)
        vna_signal.set_start_frequency("", start_frequency)
        vna_signal.set_stop_frequency("", stop_frequency)
        vna_signal.set_number_of_points("", number_of_points)
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
        vna_signal.s_params.configuration.configure_s_parameter("", sparams_parameter)
        vna_signal.s_params.configuration.set_format("", sparams_format)
        vna_signal.s_params.configuration.set_magnitude_units("", nirfmxvna.SParamsMagnitudeUnits.DB)
        vna_signal.s_params.configuration.set_phase_trace_type("", nirfmxvna.SParamsPhaseTraceType.WRAPPED)

        vna_signal.initiate("", "")

        vna_signal.s_params.results.fetch_x_data("", timeout)
        vna_signal.s_params.results.fetch_y_data("", timeout)

        vna_signal.marker.configuration.configure_type("", nirfmxvna.MarkerType.NORMAL)
        vna_signal.marker.configuration.configure_data_source("", "sparam0")
        vna_signal.marker.configuration.configure_peak_search_threshold("", threshold_enabled, threshold)
        vna_signal.marker.configuration.configure_peak_search_excursion("", excursion_enabled, excursion)

        search_modes = [
            nirfmxvna.MarkerSearchMode.PEAK,
            nirfmxvna.MarkerSearchMode.NEXT_PEAK,
            nirfmxvna.MarkerSearchMode.NEXT_LEFT_PEAK,
            nirfmxvna.MarkerSearchMode.NEXT_RIGHT_PEAK,
        ]

        for mode in search_modes:
            vna_signal.marker.results.marker_search("", mode)
            marker_x, _ = vna_signal.marker.results.fetch_x("")
            marker_y1, marker_y2, _ = vna_signal.marker.results.fetch_y("")
            _display_marker_data(marker_x, marker_y1, marker_y2)

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
        description="VNA Marker Example",
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
