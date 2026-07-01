r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties(Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the Center Frequency, Span or Start, Stop Frequency based on the Tab Selection.
5. Configure the basic signal properties(Reference Level, External Attenuation).
6. Select Spectrum measurement and enable the traces.
7. Configure Measurement Method.
8. Configure RBW Filter Parameters.
9. Configure Power Units.
10. Configure Sweep Time.
11. Configure Spectrum Averaging.
12. Configure FFT parameters.
13. Configure Noise Compensation Enabled.
14. Configure Detectors.
15. Configure VBW Filter Parameters.
16. If Measurement Method is Sequential FFT, Configure Sequential FFT Parameters.
17. Configure Cleaner Spectrum.
18. Initiate Measurement.
19. Fetch Spectrum Traces and Measurements.
20. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    # Initialize the variables
    selected_ports = ""
    center_frequency = 1e9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.00  # dB

    timeout = 10.0  # seconds

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    is_start_stop_freq = False
    start_frequency = 9.95e8  # Hz
    end_frequency = 1.005e9  # Hz
    span = 1.0e6  # Hz

    measurement_method = nirfmxspecan.SpectrumMeasurementMethod.NORMAL
    power_units = nirfmxspecan.SpectrumPowerUnits.DBM

    noise_compensation_enabled = nirfmxspecan.SpectrumNoiseCompensationEnabled.FALSE
    cleaner_spectrum = nirfmxinstr.CleanerSpectrum.DISABLED

    rbw_filter_type = nirfmxspecan.SpectrumRbwFilterType.GAUSSIAN
    rbw_auto = nirfmxspecan.SpectrumRbwAutoBandwidth.TRUE
    rbw = 10.0e3  # Hz

    vbw_auto = nirfmxspecan.SpectrumVbwFilterAutoBandwidth.TRUE
    vbw = 30.0e3  # Hz
    vbw_to_rbw_ratio = 3

    detector_type = nirfmxspecan.SpectrumDetectorType.NONE
    detector_points = 1001

    sweep_time_auto = nirfmxspecan.SpectrumSweepTimeAuto.TRUE
    sweep_time_interval = 1.00e-3  # seconds

    averaging_enabled = nirfmxspecan.SpectrumAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.SpectrumAveragingType.RMS

    fft_window = nirfmxspecan.SpectrumFftWindow.FLAT_TOP
    fft_padding = -1.0
    fft_overlap_mode = nirfmxspecan.SpectrumFftOverlapMode.DISABLED
    fft_overlap_percent = 0
    fft_overlap_type = nirfmxspecan.SpectrumFftOverlapType.RMS
    sequential_fft_size = 512

    instr_session = None
    specan = None

    try:
        # Initialize Instr
        instr_session = nirfmxinstr.Session(
            resource_name, option_string
        )  # Creates a new RFmx Session

        # Configure SpecAn
        specan = instr_session.get_specan_signal_configuration()

        # Configure measurement
        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)

        if is_start_stop_freq:
            specan.spectrum.configuration.configure_frequency_start_stop(
                "", start_frequency, end_frequency
            )
        else:
            specan.configure_frequency("", center_frequency)
            specan.spectrum.configuration.configure_span("", span)
        
        specan.configure_reference_level("", reference_level)
        specan.configure_external_attenuation("", external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.SPECTRUM, True)
        specan.spectrum.configuration.configure_measurement_method("", measurement_method)
        specan.spectrum.configuration.configure_rbw_filter("", rbw_auto, rbw, rbw_filter_type)
        specan.spectrum.configuration.configure_power_units("", power_units)
        specan.spectrum.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        specan.spectrum.configuration.configure_averaging(
            "", averaging_enabled, averaging_count, averaging_type
        )
        specan.spectrum.configuration.configure_fft("", fft_window, fft_padding)
        specan.spectrum.configuration.configure_noise_compensation_enabled(
            "", noise_compensation_enabled
        )
        specan.spectrum.configuration.configure_detector("", detector_type, detector_points)
        specan.spectrum.configuration.configure_vbw_filter("", vbw_auto, vbw, vbw_to_rbw_ratio)
        specan.spectrum.configuration.set_fft_overlap_mode("", fft_overlap_mode)
        specan.spectrum.configuration.set_fft_overlap("", fft_overlap_percent)
        specan.spectrum.configuration.set_fft_overlap_type("", fft_overlap_type)
        specan.spectrum.configuration.set_sequential_fft_size("", sequential_fft_size)
        instr_session.set_cleaner_spectrum("", cleaner_spectrum)
        specan.initiate("", "")

        # Retrieve results
        spectrum = numpy.empty(0, dtype=numpy.float32)
        x0, dx, _ = specan.spectrum.results.fetch_spectrum("", timeout, spectrum)

        peak_amplitude, peak_frequency, frequency_resolution, error_code = (
            specan.spectrum.results.fetch_measurement("", timeout)
        )

        # Print Results
        print(f"Peak Amplitude (dBm)             {peak_amplitude}")
        print(f"Peak Frequency (Hz)              {peak_frequency}")

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
        description="Pass arguments for Spectrum Example",
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
