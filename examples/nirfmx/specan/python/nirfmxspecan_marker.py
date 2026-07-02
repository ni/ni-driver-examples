r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select Spectrum measurement and enable the traces.
6. Configure Spectrum RBW.
7. Configure Spectrum Span.
8. Configure Spectrum Averaging.
9. Initiate Measurement.
10. Fetch Spectrum Trace.
11. Configure Marker Peak Threshold and Excursion.
12. Configure Marker Type and Trace.
13. Use Marker Peak Search to detect peaks in the Spectrum.
14. Fetch XY Location of the Marker.
15. Display interactive menu: l/L - Next Left, r/R - Next Right, h/H - Next Highest, s/S - Stop.
16. Move Marker to the selected next peak and fetch XY Location.
17. Close the RFmx Session.
"""

import argparse
import msvcrt
import sys

import nirfmxspecan
import numpy

import nirfmxinstr


def _display_marker_data(number_of_peaks, next_peak_found, marker_x_location, marker_y_location):
    print("---------------------------------------------------\n")
    print(f"Number of peaks          {number_of_peaks}")
    print(f"Next Peak found?         {next_peak_found}")
    print(f"Marker X Location (Hz)   {marker_x_location}")
    print(f"Marker Y Location (dBm)  {marker_y_location}")
    print("----------------------------------------------------\n")


def _display_marker_menu():
    print("To read different peaks, use the following keys\n")
    print("l/L - Next Left\n")
    print("r/R - Next Right\n")
    print("h/H - Next Highest\n")
    print("s/S - Stop and Exit menu\n")


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1e9   # Hz
    reference_level = 0.00   # dBm
    external_attenuation = 0.00  # dB
    timeout = 10.0  # seconds

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz
    span = 1.0e6  # Hz

    rbw_filter_type = nirfmxspecan.SpectrumRbwFilterType.GAUSSIAN
    rbw_auto = nirfmxspecan.SpectrumRbwAutoBandwidth.FALSE
    rbw = 10.0e3  # Hz

    averaging_enabled = nirfmxspecan.SpectrumAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.SpectrumAveragingType.RMS

    marker_type = nirfmxspecan.MarkerType.NORMAL
    marker_trace = nirfmxspecan.MarkerTrace.SPECTRUM

    threshold_enabled = nirfmxspecan.MarkerThresholdEnabled.FALSE
    threshold = -90.0  # dBm

    excursion_enabled = nirfmxspecan.MarkerPeakExcursionEnabled.FALSE
    excursion = 6.0  # dB (relative)

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.SPECTRUM, True)
        specan.spectrum.configuration.configure_rbw_filter("", rbw_auto, rbw, rbw_filter_type)
        specan.spectrum.configuration.configure_span("", span)
        specan.spectrum.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.initiate("", "")

        # Fetch spectrum
        spectrum = numpy.empty(0, dtype=numpy.float32)
        specan.spectrum.results.fetch_spectrum("", timeout, spectrum)

        # Configure and use marker
        specan.marker.configuration.configure_threshold("", threshold_enabled, threshold)
        specan.marker.configuration.configure_peak_excursion("", excursion_enabled, excursion)
        specan.marker.configuration.configure_type("marker0", marker_type)
        specan.marker.configuration.configure_trace("marker0", marker_trace)

        number_of_peaks, error_code = specan.marker.results.peak_search("marker0")
        marker_x_location, marker_y_location, error_code = specan.marker.results.fetch_xy("marker0")

        next_peak_found = False
        _display_marker_data(number_of_peaks, next_peak_found, marker_x_location, marker_y_location)
        _display_marker_menu()

        peak_to_fetch = nirfmxspecan.MarkerNextPeak.NEXT_HIGHEST
        while True:
            ch = msvcrt.getwch().lower()
            do_not_fetch = False
            if ch == "l":
                peak_to_fetch = nirfmxspecan.MarkerNextPeak.NEXT_LEFT
            elif ch == "r":
                peak_to_fetch = nirfmxspecan.MarkerNextPeak.NEXT_RIGHT
            elif ch == "h":
                peak_to_fetch = nirfmxspecan.MarkerNextPeak.NEXT_HIGHEST
            elif ch == "s":
                break
            else:
                do_not_fetch = True
                _display_marker_menu()

            if not do_not_fetch:
                next_peak_found, error_code = specan.marker.results.next_peak("marker0", peak_to_fetch)
                marker_x_location, marker_y_location, error_code = specan.marker.results.fetch_xy("marker0")
                _display_marker_data(number_of_peaks, next_peak_found, marker_x_location, marker_y_location)

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        if specan is not None:
            specan.dispose()
            specan = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    parser = argparse.ArgumentParser(
        description="Pass arguments for Marker Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr.")
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string)


def main():
    _main(sys.argv[1:])


def test_main():
    _main(["--option-string", ""])


def test_example():
    example("RFSA", "")


if __name__ == "__main__":
    main()
