r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select IM measurement and enable the traces.
6. Configure IM Averaging.
7. Configure IM RBW Filter parameters.
8. Configure IM Sweep Time.
9. Configure IM FFT.
10. Configure Frequency Definition.
11. Configure Measurement Method.
12. Configure Fundamental Tones.
13. Configure Auto Intermods Setup Enabled and Maximum Intermod Order.
14. Configure intermods (Array).
15. Initiate Measurement.
16. Fetch IM Measurements and Traces.
17. Close RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr

NUMBER_OF_INTERMODS = 1


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9    # Hz
    reference_level = 0.0       # dBm
    external_attenuation = 0.0  # dB

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

    frequency_definition = nirfmxspecan.IMFrequencyDefinition.RELATIVE
    measurement_method = nirfmxspecan.IMMeasurementMethod.NORMAL

    lower_tone_frequency = -1.0e6  # Hz (offset from center, if relative)
    upper_tone_frequency = 1.0e6   # Hz

    auto_intermods_setup_enabled = nirfmxspecan.IMAutoIntermodsSetupEnabled.TRUE
    maximum_intermod_order = 3

    # Manual intermod configuration (used if auto_intermods_setup_enabled == FALSE)
    intermod_enabled = [nirfmxspecan.IMIntermodEnabled.TRUE] * NUMBER_OF_INTERMODS
    intermod_order = [3] * NUMBER_OF_INTERMODS
    intermod_side = [nirfmxspecan.IMIntermodSide.BOTH] * NUMBER_OF_INTERMODS
    lower_intermod_frequency = [-3.0e6] * NUMBER_OF_INTERMODS  # Hz
    upper_intermod_frequency = [3.0e6] * NUMBER_OF_INTERMODS   # Hz

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

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
        specan.im.configuration.configure_frequency_definition("", frequency_definition)
        specan.im.configuration.configure_measurement_method("", measurement_method)
        specan.im.configuration.configure_fundamental_tones("", lower_tone_frequency, upper_tone_frequency)
        specan.im.configuration.configure_auto_intermods_setup("", auto_intermods_setup_enabled, maximum_intermod_order)

        if auto_intermods_setup_enabled == nirfmxspecan.IMAutoIntermodsSetupEnabled.FALSE:
            specan.im.configuration.configure_number_of_intermods("", NUMBER_OF_INTERMODS)
            specan.im.configuration.configure_intermod_array(
                "",
                intermod_order,
                lower_intermod_frequency,
                upper_intermod_frequency,
                intermod_side,
                intermod_enabled,
            )

        specan.initiate("", "")

        actual_number_of_intermods, error_code = specan.im.configuration.get_number_of_intermods("")
        if measurement_method == nirfmxspecan.IMMeasurementMethod.NORMAL:
            number_of_spectrums = 1
        else:
            number_of_spectrums = 2 * actual_number_of_intermods + 2

        lower_tone_power, upper_tone_power, error_code = specan.im.results.fetch_fundamental_measurement("", timeout)
        print("Fundamental Tone Measurement\n")
        print(f"Lower Tone Power(dBm)              :{lower_tone_power}")
        print(f"Upper Tone Power(dBm)              :{upper_tone_power}")

        (
            intermod_order,
            lower_intermod_power,
            upper_intermod_power,
            error_code,
        ) = specan.im.results.fetch_intermod_measurement_array("", timeout)
        (
            intermod_order,
            worst_case_output_intercept,
            lower_output_intercept,
            upper_output_intercept,
            error_code,
        ) = specan.im.results.fetch_intercept_power_array("", timeout)
        for spectrum_index in range(number_of_spectrums):
            spectrum = numpy.empty(0, dtype=numpy.float32)
            specan.im.results.fetch_spectrum("", timeout, spectrum_index, spectrum)

        print("\nIntermod Measurements\n")

        for i in range(actual_number_of_intermods):
            print("")
            print(f"Intermod Measurement                  : {i}")
            print(f"Order                                 : {intermod_order[i]}")
            print(f"Lower Intermod Power(dBm)             :{lower_intermod_power[i]}")
            print(f"Upper Intermod Power(dBm)             :{upper_intermod_power[i]}")
            print(f"Lower Output Intercept Power(dBm)     :{lower_output_intercept[i]}")
            print(f"Upper Output Intercept Power(dBm)     :{upper_output_intercept[i]}")
            print(f"Worst Case Output Intercept Power(dBm):{worst_case_output_intercept[i]}")

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
        description="Pass arguments for IM Advanced Example",
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
