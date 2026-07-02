r"""Steps:
1. Open a new RFmx Session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select ADemod Measurement and enable the traces.
6. Configure FM Modulation.
7. Configure ADemod RBW Filter, Measurement Interval, and Carrier Correction.
8. Configure ADemod FM DeEmphasis, Audio Filter and Averaging.
9. Initiate Measurement.
10. Fetch ADemod FM Measurement Results.
11. Close the RFmx Session.
"""

import argparse
import sys

import numpy

import nirfmxdemod
import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    # Initialize input variables
    selected_ports = ""
    center_frequency = 1e9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.00  # dB

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    measurement_interval = 10.00e-3  # seconds
    de_emphasis = 0.0
    rbw = 100.00e3  # Hz
    rbw_rrc_alpha = 0.100

    audio_filter_lower_cutoff = 100.000  # Hz
    audio_filter_upper_cutoff = 10.000e3  # Hz

    averaging_count = 10
    timeout = 10.0  # seconds

    instr_session = None
    demod = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get Demod signal
        demod = instr_session.get_demod_signal_configuration()

        # Configure measurement
        instr_session.configure_frequency_reference("", frequency_source, frequency)
        demod.set_selected_ports("", selected_ports)
        demod.configure_frequency("", center_frequency)
        demod.configure_reference_level("", reference_level)
        demod.configure_external_attenuation("", external_attenuation)
        demod.select_measurements("", nirfmxdemod.MeasurementTypes.ADEMOD, True)
        demod.analog_demod.configuration.set_audio_measurement_enabled(
            "", nirfmxdemod.ADemodAudioMeasurementEnabled.TRUE
        )
        demod.analog_demod.configuration.configure_modulation_type(
            "", nirfmxdemod.ADemodModulationType.FM
        )
        demod.analog_demod.configuration.configure_rbw_filter(
            "", rbw, nirfmxdemod.ADemodRbwFilterType.FLAT, rbw_rrc_alpha
        )
        demod.analog_demod.configuration.configure_measurement_interval("", measurement_interval)
        demod.analog_demod.configuration.configure_carrier_correction(
            "",
            nirfmxdemod.ADemodCarrierFrequencyCorrectionEnabled.TRUE,
            nirfmxdemod.ADemodCarrierPhaseCorrectionEnabled.TRUE,
        )
        demod.analog_demod.configuration.configure_fm_de_emphasis("", de_emphasis)
        demod.analog_demod.configuration.configure_audio_filter(
            "",
            nirfmxdemod.ADemodAudioFilterType.NONE,
            audio_filter_lower_cutoff,
            audio_filter_upper_cutoff,
        )
        demod.analog_demod.configuration.configure_averaging(
            "", nirfmxdemod.ADemodAveragingEnabled.FALSE, averaging_count, nirfmxdemod.ADemodAveragingType.LINEAR
        )

        demod.initiate("", "")

        # Retrieve results
        average_sinad, average_snr, average_thd, average_thd_with_noise, error_code = (
            demod.analog_demod.results.fetch_distortions("", timeout)
        )
        mean_modulation_frequency, error_code = (
            demod.analog_demod.results.fetch_mean_modulation_frequency("", timeout)
        )
        mean_carrier_frequency_error, mean_carrier_power, error_code = (
            demod.analog_demod.results.fetch_carrier_measurement("", timeout)
        )
        (
            mean_deviation,
            mean_half_peak_to_peak,
            mean_rms,
            mean_positive_peak,
            mean_negative_peak,
            error_code,
        ) = demod.analog_demod.results.fetch_fm_mean_deviation("", timeout)
        (
            max_deviation,
            max_half_peak_to_peak,
            max_rms,
            max_positive_peak,
            max_negative_peak,
            error_code,
        ) = demod.analog_demod.results.fetch_fm_maximum_deviation("", timeout)
        demod_spectrum_trace = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = demod.analog_demod.results.fetch_demod_spectrum_trace(
            "", timeout, demod_spectrum_trace
        )
        demod_signal_trace = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = demod.analog_demod.results.fetch_demod_signal_trace(
            "", timeout, demod_signal_trace
        )

        # Print Results
        print("------------------------------------------------------")
        print(f"Mean Carrier Power (dBm)         : {mean_carrier_power}")
        print(f"Average SINAD (dB)               : {average_sinad}")
        print(f"Average THD with Noise (%)       : {average_thd_with_noise}")
        print(f"Mean Carrier Frequency Error(Hz) : {mean_carrier_frequency_error}")
        print(f"Average SNR (dB)                 : {average_snr}")
        print(f"Average THD (%)                  : {average_thd}")
        print(f"Mean Modulation Frequency (Hz)   : {mean_modulation_frequency}")

        print("\n-------------------FM Deviations---------------------\n")
        print(f"Mean Deviation (Hz)            : {mean_deviation}")
        print(f"Maximum Deviation (Hz)         : {max_deviation}")
        print(f"Mean Peak to Peak/2 (Hz)       : {mean_half_peak_to_peak}")
        print(f"Maximum Peak to Peak/2 (Hz)    : {max_half_peak_to_peak}")
        print(f"Mean Positive Peak (Hz)        : {mean_positive_peak}")
        print(f"Maximum Positive Peak (Hz)     : {max_positive_peak}")
        print(f"Mean Negative peak (Hz)        : {mean_negative_peak}")
        print(f"Maximum Negative peak (Hz)     : {max_negative_peak}")
        print(f"Mean RMS (Hz)                  : {mean_rms}")
        print(f"Maximum RMS (Hz)               : {max_rms}")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close Session
        if demod is not None:
            demod.dispose()
            demod = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for ADemod FM Advanced Example",
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
