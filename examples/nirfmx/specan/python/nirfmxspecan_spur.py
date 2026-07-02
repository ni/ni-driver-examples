r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Reference Level and External Attenuation).
5. Select Spur measurement and enable the traces.
6. Configure Spur Averaging.
7. Configure Spur Number of Ranges.
8. Configure Spur Range Start and Stop Frequency Array.
9. Configure Spur Range RBW Filter Array.
10. Configure Spur Range Absolute Limit Array.
11. Configure Spur Range Number of Spurs to Report Array.
12. Configure Spur Range Peak Criteria Array.
13. Configure Spur Range Detector Array.
14. Configure Spur Range VBW Filter Array.
15. Configure Spur Trace Range Index.
16. Initiate Measurement.
17. Fetch Spur Measurements, Traces and Status.
18. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr

NUMBER_OF_RANGES = 1
NUMBER_OF_SPURS_TO_REPORT = 10


def example(resource_name, option_string):
    """Run Example."""
    # Initialize input variables
    selected_ports = ""
    reference_level = 0.00  # dBm
    external_attenuation = 0.00  # dB
    timeout = 10.0  # seconds

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    averaging_enabled = nirfmxspecan.SpurAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.SpurAveragingType.RMS

    trace_range_index = 0

    # Range arrays
    range_enabled = numpy.array(
        [nirfmxspecan.SpurRangeEnabled.TRUE.value] * NUMBER_OF_RANGES, dtype=numpy.int32
    )
    start_frequency = numpy.array([1.0e9])   # Hz
    stop_frequency = numpy.array([1.5e9])    # Hz

    rbw_filter_auto = numpy.array(
        [nirfmxspecan.SpurRbwAutoBandwidth.TRUE.value] * NUMBER_OF_RANGES, dtype=numpy.int32
    )
    rbw_filter_bandwidth = numpy.array([30.0e3])  # Hz
    rbw_filter_type = numpy.array(
        [nirfmxspecan.SpurRbwFilterType.GAUSSIAN.value] * NUMBER_OF_RANGES, dtype=numpy.int32
    )

    vbw_auto = numpy.array(
        [nirfmxspecan.SpurRangeVbwFilterAutoBandwidth.TRUE.value] * NUMBER_OF_RANGES,
        dtype=numpy.int32,
    )
    vbw = numpy.array([30.0e3])  # Hz
    vbw_to_rbw_ratio = numpy.array([3.0])

    detector_type = numpy.array(
        [nirfmxspecan.SpurRangeDetectorType.NONE.value] * NUMBER_OF_RANGES, dtype=numpy.int32
    )
    detector_points = numpy.array([1001], dtype=numpy.int32)

    absolute_limit_mode = numpy.array(
        [nirfmxspecan.SpurAbsoluteLimitMode.COUPLE.value] * NUMBER_OF_RANGES, dtype=numpy.int32
    )
    absolute_limit_start = numpy.array([-10.0])
    absolute_limit_stop = numpy.array([-10.0])

    peak_threshold = numpy.array([-200.0])
    peak_excursion = numpy.array([0.0])

    number_of_spurs_to_report = numpy.array([NUMBER_OF_SPURS_TO_REPORT], dtype=numpy.int32)

    instr_session = None
    specan = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get SpecAn signal
        specan = instr_session.get_specan_signal_configuration()

        # Configure measurement
        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_reference_level("", reference_level)
        specan.configure_external_attenuation("", external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.SPUR, True)
        specan.spur.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.spur.configuration.configure_number_of_ranges("", NUMBER_OF_RANGES)
        specan.spur.configuration.configure_range_frequency_array(
            "", start_frequency, stop_frequency, range_enabled
        )
        specan.spur.configuration.configure_range_rbw_array(
            "", rbw_filter_auto, rbw_filter_bandwidth, rbw_filter_type
        )
        specan.spur.configuration.configure_range_absolute_limit_array(
            "", absolute_limit_mode, absolute_limit_start, absolute_limit_stop
        )
        specan.spur.configuration.configure_range_number_of_spurs_to_report_array(
            "", number_of_spurs_to_report
        )
        specan.spur.configuration.configure_range_peak_criteria_array("", peak_threshold, peak_excursion)
        specan.spur.configuration.configure_range_detector_array("", detector_type, detector_points)
        specan.spur.configuration.configure_range_vbw_filter_array("", vbw_auto, vbw, vbw_to_rbw_ratio)
        specan.spur.configuration.configure_trace_range_index("", trace_range_index)
        specan.initiate("", "")

        # Retrieve results
        range_status, number_of_detected_spurs, error_code = (
            specan.spur.results.fetch_range_status_array("", timeout)
        )

        spur_frequency, spur_amplitude, spur_margin, spur_absolute_limit, spur_range_index, error_code = (
            specan.spur.results.fetch_all_spurs("", timeout)
        )

        range_number = 0 if trace_range_index == -1 else trace_range_index
        range_string = nirfmxspecan.SpecAn.build_range_string("", range_number)

        absolute_limit_trace = numpy.empty(0, dtype=numpy.float32)
        specan.spur.results.fetch_range_absolute_limit_trace(range_string, timeout, absolute_limit_trace)

        spectrum_trace = numpy.empty(0, dtype=numpy.float32)
        specan.spur.results.fetch_range_spectrum_trace(range_string, timeout, spectrum_trace)

        measurement_status, error_code = specan.spur.results.fetch_measurement_status("", timeout)

        # Print results in a structured console format.
        status = "Pass" if measurement_status == nirfmxspecan.SpurMeasurementStatus.PASS else "Fail"
        separator = "-" * 70
        print("\n-------------------------Measurement-------------------------")
        print(f"\nMeasurement Status   : {status}\n")

        total_spurs = int(numpy.sum(number_of_detected_spurs))
        print("Spur List:\n")
        for i in range(total_spurs):
            print(f"Spur {i}")
            print(f"Range Index          : {spur_range_index[i]}")
            print(f"Frequency (Hz)       : {spur_frequency[i]}")
            print(f"Amplitude (dBm)      : {spur_amplitude[i]}")
            print(f"Absolute Limit (dBm) : {spur_absolute_limit[i]}")
            print(f"Margin (dB)          : {spur_margin[i]}")
            print(separator)
            print("")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close Session
        if specan is not None:
            specan.dispose()
            specan = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for Spur Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr."
    )
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string)


def main():
    """Call _main function."""
    _main(sys.argv[1:])


def test_main():
    """Call _main function with empty option string."""
    cmd_line = [
        "--option-string",
        "",
    ]
    _main(cmd_line)


def test_example():
    """Call example function."""
    example("RFSA", "")


if __name__ == "__main__":
    main()
