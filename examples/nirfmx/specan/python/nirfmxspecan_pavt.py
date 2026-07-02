r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure Digital Edge Trigger.
6. Select PAVT measurement and enable the traces.
7. Configure Measurement Location Type.
8. Configure Segment Start Time (Step or List).
9. Configure Measurement Bandwidth.
10. Configure Measurement Interval (Offset and Length).
11. Initiate Measurement.
12. Fetch PAVT Phase and Amplitude Arrays.
13. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr

NUMBER_OF_SEGMENTS = 1


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference = 10.0e6  # Hz

    enable_trigger = True
    digital_edge_source = "PXI_Trig0"
    digital_edge = nirfmxspecan.DigitalEdgeTriggerEdge.RISING_EDGE
    trigger_delay = 0.0  # seconds

    measurement_location_type = nirfmxspecan.PavtMeasurementLocationType.TIME

    # Segment Step parameters
    segment0_start_time = 0.0  # seconds
    segment_interval = 1.0e-3  # seconds

    measurement_bandwidth = 10.0e6  # Hz
    measurement_offset = 0.0  # seconds
    measurement_length = 1.0e-3  # seconds

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference)
        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.configure_digital_edge_trigger("", digital_edge_source, digital_edge, trigger_delay, enable_trigger)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.PAVT, True)
        specan.pavt.configuration.configure_measurement_location_type("", measurement_location_type)
        if measurement_location_type == nirfmxspecan.PavtMeasurementLocationType.TIME:
            specan.pavt.configuration.configure_segment_start_time_step(
                "", NUMBER_OF_SEGMENTS, segment0_start_time, segment_interval
            )
        else:
            specan.pavt.configuration.configure_number_of_segments("", NUMBER_OF_SEGMENTS)
        specan.pavt.configuration.configure_measurement_bandwidth("", measurement_bandwidth)
        specan.pavt.configuration.configure_measurement_interval("", measurement_offset, measurement_length)
        specan.initiate("", "")

        (
            mean_relative_phase,
            mean_relative_amplitude,
            mean_absolute_phase,
            mean_absolute_amplitude,
            error_code,
        ) = specan.pavt.results.fetch_phase_and_amplitude_array("", timeout)

        for i in range(NUMBER_OF_SEGMENTS):
            phase = numpy.empty(0, dtype=numpy.float32)
            amplitude = numpy.empty(0, dtype=numpy.float32)
            specan.pavt.results.fetch_phase_trace("", timeout, i, phase)
            specan.pavt.results.fetch_amplitude_trace("", timeout, i, amplitude)

        print(f"Segment0 Mean Absolute Phase (deg)       : {mean_absolute_phase[0]}")
        print(f"Segment0 Mean Absolute Amplitude (dBm)   : {mean_absolute_amplitude[0]}\n")
        print("Segment Measurements")
        for i in range(NUMBER_OF_SEGMENTS):
            print(f"Segment  :  {i}")
            print(f"Mean Relative Phase (deg)                : {mean_relative_phase[i]}")
            print(f"Mean Relative Amplitude (dB)             : {mean_relative_amplitude[i]}")
            print("-------------------------------------------------\n")

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
        description="Pass arguments for PAVT Example",
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
