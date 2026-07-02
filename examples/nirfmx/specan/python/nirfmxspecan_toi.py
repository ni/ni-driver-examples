r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure RF Attenuation.
6. Select IM (TOI) measurement and enable the traces.
7. Configure Averaging.
8. Configure RBW Filter parameters.
9. Configure Sweep Time.
10. Configure FFT.
11. Configure Measurement Method.
12. Configure Fundamental Tones (Lower and Upper Tone Frequency).
13. Configure Auto Intermods Setup.
14. Initiate Measurement.
15. Fetch IM Measurements and Traces.
16. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    # Initialize input variables
    selected_ports = ""
    center_frequency = 1e9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.00  # dB
    timeout = 10.0  # seconds

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    rf_attenuation_auto = nirfmxinstr.RFAttenuationAuto.TRUE
    rf_attenuation = 10.0  # dB

    enable_all_traces = True

    averaging_enabled = nirfmxspecan.IMAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.IMAveragingType.RMS

    rbw_filter_type = nirfmxspecan.IMRbwFilterType.GAUSSIAN
    rbw_auto = nirfmxspecan.IMRbwFilterAutoBandwidth.TRUE
    rbw = 10.0e3  # Hz

    sweep_time_auto = nirfmxspecan.IMSweepTimeAuto.TRUE
    sweep_time_interval = 1.0e-3  # seconds

    fft_window = nirfmxspecan.IMFftWindow.FLAT_TOP
    fft_padding = -1.0

    measurement_method = nirfmxspecan.IMMeasurementMethod.NORMAL

    lower_tone_frequency = -1.0e6  # Hz (relative offset from center)
    upper_tone_frequency = 1.0e6   # Hz (relative offset from center)

    auto_intermods_setup_enabled = nirfmxspecan.IMAutoIntermodsSetupEnabled.TRUE
    maximum_intermod_order = 3

    instr_session = None
    specan = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get SpecAn signal
        specan = instr_session.get_specan_signal_configuration()

        # Configure measurement
        specan.configure_frequency("", center_frequency)
        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_reference_level("", reference_level)
        specan.configure_external_attenuation("", external_attenuation)
        instr_session.configure_rf_attenuation("", rf_attenuation_auto, rf_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.IM, enable_all_traces)
        specan.im.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.im.configuration.configure_rbw_filter("", rbw_auto, rbw, rbw_filter_type)
        specan.im.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        specan.im.configuration.configure_fft("", fft_window, fft_padding)
        specan.im.configuration.configure_measurement_method("", measurement_method)
        specan.im.configuration.configure_fundamental_tones("", lower_tone_frequency, upper_tone_frequency)
        specan.im.configuration.configure_auto_intermods_setup(
            "", auto_intermods_setup_enabled, maximum_intermod_order
        )
        specan.initiate("", "")

        # Retrieve results
        lower_tone_power, upper_tone_power, error_code = (
            specan.im.results.fetch_fundamental_measurement("", timeout)
        )

        intermod_order, lower_intermod_power, upper_intermod_power, error_code = (
            specan.im.results.fetch_intermod_measurement("", timeout)
        )

        intermod_order, worst_case_output_intercept_power, lower_output_intercept_power, upper_output_intercept_power, error_code = (
            specan.im.results.fetch_intercept_power("", timeout)
        )

        number_of_spectrums = 1 if measurement_method == nirfmxspecan.IMMeasurementMethod.NORMAL else 4
        for spectrum_index in range(number_of_spectrums):
            spectrum = numpy.empty(0, dtype=numpy.float32)
            specan.im.results.fetch_spectrum("", timeout, spectrum_index, spectrum)

        # Print results in aligned console format.
        print("Fundamental Tone Measurement\n")
        print(f"Lower Tone Power(dBm)      :{lower_tone_power}")
        print(f"Upper Tone Power(dBm)      :{upper_tone_power}")

        print("\nIntermod Measurement\n")
        print(f"Lower Intermod Power(dBm)  :{lower_intermod_power}")
        print(f"Upper Intermod Power(dBm)  :{upper_intermod_power}")
        print(f"Lower TOI(dBm)             :{lower_output_intercept_power}")
        print(f"Upper TOI(dBm)             :{upper_output_intercept_power}")
        print(f"Worst Case TOI(dBm)        :{worst_case_output_intercept_power}")

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
        description="Pass arguments for TOI (IM) Example",
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
