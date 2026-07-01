r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select Spur measurement and enable the traces.
6. Configure Spur Averaging.
7. Configure Spur FFT Window.
8. Configure Spur Trace Range Index.
9. Configure Spur Number of Ranges.
10. Configure Spur Range properties using Array APIs:
    - Frequency (start/stop), Enabled, Relative Attenuation, RBW Filter,
      Absolute Limit, Number of Spurs to Report, Peak Criteria, Detector, VBW Filter.
11. Initiate Measurement.
12. Fetch Range Status Array.
13. Fetch All Spurs.
14. Fetch Range Absolute Limit Trace and Range Spectrum Trace.
15. Fetch Measurement Status.
16. Close the RFmx Session.
"""

import argparse
import sys

import numpy
import nirfmxspecan

import nirfmxinstr

NUMBER_OF_RANGES = 1
NUMBER_OF_SPURS_TO_REPORT = 10


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9    # Hz
    reference_level = 0.0       # dBm
    external_attenuation = 0.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference = 10.0e6  # Hz

    averaging_enabled = nirfmxspecan.SpurAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.SpurAveragingType.RMS

    fft_window = nirfmxspecan.SpurFftWindow.FLAT_TOP

    trace_range_index = 0

    # Range list
    range_enabled = [nirfmxspecan.SpurRangeEnabled.TRUE] * NUMBER_OF_RANGES
    range_start_frequency = [1.0e9]   # Hz
    range_stop_frequency = [1.5e9]    # Hz
    range_relative_attenuation = [0.0] * NUMBER_OF_RANGES  # dB
    range_rbw_filter_auto = [nirfmxspecan.SpurRbwAutoBandwidth.TRUE] * NUMBER_OF_RANGES
    range_rbw_filter_bandwidth = [30.0e3] * NUMBER_OF_RANGES  # Hz
    range_rbw_filter_type = [nirfmxspecan.SpurRbwFilterType.GAUSSIAN] * NUMBER_OF_RANGES
    range_absolute_limit_mode = [nirfmxspecan.SpurAbsoluteLimitMode.COUPLE] * NUMBER_OF_RANGES
    range_absolute_start_limit = [-10.0] * NUMBER_OF_RANGES  # dBm
    range_absolute_stop_limit = [-10.0] * NUMBER_OF_RANGES   # dBm
    range_peak_threshold = [-200.0] * NUMBER_OF_RANGES       # dBm
    range_peak_excursion = [0.0] * NUMBER_OF_RANGES           # dB
    range_number_of_spurs_to_report = [NUMBER_OF_SPURS_TO_REPORT] * NUMBER_OF_RANGES
    range_vbw_auto = [nirfmxspecan.SpurRangeVbwFilterAutoBandwidth.TRUE] * NUMBER_OF_RANGES
    range_vbw = [30.0e3] * NUMBER_OF_RANGES        # Hz
    range_vbw_to_rbw_ratio = [3.0] * NUMBER_OF_RANGES
    range_detector_type = [nirfmxspecan.SpurRangeDetectorType.NONE] * NUMBER_OF_RANGES
    range_detector_points = [1001] * NUMBER_OF_RANGES

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference)
        specan.set_selected_ports("", selected_ports)
        specan.configure_frequency("", center_frequency)
        specan.configure_reference_level("", reference_level)
        specan.configure_external_attenuation("", external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.SPUR, True)
        specan.spur.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.spur.configuration.configure_fft_window_type("", fft_window)
        specan.spur.configuration.configure_trace_range_index("", trace_range_index)
        specan.spur.configuration.configure_number_of_ranges("", NUMBER_OF_RANGES)

        specan.spur.configuration.configure_range_frequency_array("", range_start_frequency, range_stop_frequency, range_enabled)
        specan.spur.configuration.configure_range_relative_attenuation_array("", range_relative_attenuation)
        specan.spur.configuration.configure_range_rbw_array("", range_rbw_filter_auto, range_rbw_filter_bandwidth, range_rbw_filter_type)
        specan.spur.configuration.configure_range_absolute_limit_array("", range_absolute_limit_mode, range_absolute_start_limit, range_absolute_stop_limit)
        specan.spur.configuration.configure_range_number_of_spurs_to_report_array("", range_number_of_spurs_to_report)
        specan.spur.configuration.configure_range_peak_criteria_array("", range_peak_threshold, range_peak_excursion)
        specan.spur.configuration.configure_range_detector_array("", range_detector_type, range_detector_points)
        specan.spur.configuration.configure_range_vbw_filter_array("", range_vbw_auto, range_vbw, range_vbw_to_rbw_ratio)
        specan.initiate("", "")

        range_status, detected_spurs, error_code = specan.spur.results.fetch_range_status_array("", timeout)
        total_spurs = sum(detected_spurs)

        spur_frequency, spur_amplitude, spur_margin, spur_absolute_limit, spur_range_index, error_code = (
            specan.spur.results.fetch_all_spurs("", timeout)
        )

        trace_range_index_for_trace = trace_range_index if trace_range_index != -1 else 0
        range_str = nirfmxspecan.SpecAn.build_range_string("", trace_range_index_for_trace)
        absolute_limit_trace = numpy.empty(0, dtype=numpy.float32)
        range_spectrum = numpy.empty(0, dtype=numpy.float32)
        specan.spur.results.fetch_range_absolute_limit_trace(range_str, timeout, absolute_limit_trace)
        specan.spur.results.fetch_range_spectrum_trace(range_str, timeout, range_spectrum)

        measurement_status, error_code = specan.spur.results.fetch_measurement_status("", timeout)

        print("----------------Measurement-------------------\n")
        status = "Pass" if measurement_status == nirfmxspecan.SpurMeasurementStatus.PASS else "Fail"
        print(f"Measurement Status: {status}\n")

        print("----------- Spur List-------------------\n")
        for i in range(total_spurs):
            range_status_str = "Pass" if range_status[spur_range_index[i]] == nirfmxspecan.SpurRangeStatus.PASS else "Fail"
            print(f"Spur                      {i + 1}")
            print(f"Range Measurement Status  {range_status_str}")
            print(f"Range Index               {spur_range_index[i]}")
            print(f"Frequency (Hz)            {spur_frequency[i]}")
            print(f"Amplitude (dBm)           {spur_amplitude[i]}")
            print(f"Absolute Limit (dBm)      {spur_absolute_limit[i]}")
            print(f"Margin (dB)               {spur_margin[i]}")
            print("---------------------------------------")

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
        description="Pass arguments for Spur Advanced Example",
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
