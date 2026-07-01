r"""Steps:
1. Open a new RFmx session.
2. Configure Selected Ports.
3. Configure the basic signal properties (Reference Level and External Attenuation).
4. Select Spur measurement.
5. Configure Spur Number of Ranges.
6. Configure Spur Start Frequency, Stop Frequency, RBW Filter and Absolute Limit Start for all ranges.
7. Initiate Measurement.
8. Read Spur Measurement Status.
9. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 0.0   # Hz
    reference_level = 0.00   # dBm
    external_attenuation = 0.00  # dB

    range_list_size = 1
    start_frequency = numpy.array([1.0e9])   # Hz
    stop_frequency = numpy.array([1.5e9])    # Hz
    range_enabled = numpy.array(
        [nirfmxspecan.SpurRangeEnabled.TRUE.value], dtype=numpy.int32
    )

    rbw_filter_auto = numpy.array(
        [nirfmxspecan.SpurRbwAutoBandwidth.FALSE.value], dtype=numpy.int32
    )
    rbw_filter_bandwidth = numpy.array([30.0e3])  # Hz
    rbw_filter_type = numpy.array(
        [nirfmxspecan.SpurRbwFilterType.GAUSSIAN.value], dtype=numpy.int32
    )

    limit = numpy.array([-10.0])  # dBm
    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.SPUR, True)
        specan.spur.configuration.configure_number_of_ranges("", range_list_size)
        specan.spur.configuration.configure_range_frequency_array(
            "", start_frequency, stop_frequency, range_enabled
        )
        specan.spur.configuration.configure_range_rbw_array(
            "", rbw_filter_auto, rbw_filter_bandwidth, rbw_filter_type
        )
        specan.spur.configuration.configure_range_absolute_limit_array("", None, limit, None)
        specan.initiate("", "")

        measurement_status, error_code = specan.spur.results.fetch_measurement_status("", timeout)

        status = "Pass" if measurement_status == nirfmxspecan.SpurMeasurementStatus.PASS else "Fail"
        print(f"Measurement Status: {status}")

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
        description="Pass arguments for Spur Basic Example",
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
