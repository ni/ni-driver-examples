r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select Harmonics measurement and enable the traces.
6. Configure RBW Filter parameters.
7. Configure Measurement Interval of the Fundamental.
8. Configure Auto Harmonics Setup.
9. Configure Number of Harmonics.
10. Configure Measurement Method (DynamicRange) and Noise Compensation Enabled.
11. Configure Averaging parameters.
12. Initiate Measurement.
13. Fetch Total Harmonic Distortion.
14. Fetch Harmonic Measurement results.
15. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr

NUMBER_OF_HARMONICS = 3


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9  # Hz
    reference_level = 0.0   # dBm
    external_attenuation = 0.0  # dB
    timeout = 10.0  # seconds

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    rbw_filter_type = nirfmxspecan.HarmRbwFilterType.GAUSSIAN
    rbw = 100.0e3  # Hz
    rrc_alpha = 0.010
    measurement_interval = 1.0e-3  # seconds

    auto_harmonics_setup = nirfmxspecan.HarmAutoHarmonicsSetupEnabled.TRUE

    harmonics_order = numpy.array(list(range(1, NUMBER_OF_HARMONICS + 1)), dtype=numpy.int32)
    harmonics_bandwidth = numpy.array([100.0e3] * NUMBER_OF_HARMONICS)
    harmonics_enabled = numpy.array(
        [nirfmxspecan.HarmHarmonicEnabled.TRUE.value] * NUMBER_OF_HARMONICS, dtype=numpy.int32
    )
    harmonics_measurement_interval = numpy.array([1.0e-3] * NUMBER_OF_HARMONICS)

    measurement_method = nirfmxspecan.HarmMeasurementMethod.DYNAMIC_RANGE
    noise_compensation_enabled = nirfmxspecan.HarmNoiseCompensationEnabled.TRUE

    averaging_enabled = nirfmxspecan.HarmAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.HarmAveragingType.RMS

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_frequency("", center_frequency)
        specan.configure_reference_level("", reference_level)
        specan.configure_external_attenuation("", external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.HARMONICS, True)
        specan.harm.configuration.configure_fundamental_rbw("", rbw, rbw_filter_type, rrc_alpha)
        specan.harm.configuration.configure_fundamental_measurement_interval("", measurement_interval)
        specan.harm.configuration.configure_auto_harmonics("", auto_harmonics_setup)
        specan.harm.configuration.configure_number_of_harmonics("", NUMBER_OF_HARMONICS)
        if auto_harmonics_setup != nirfmxspecan.HarmAutoHarmonicsSetupEnabled.TRUE:
            specan.harm.configuration.configure_harmonic_array(
                "", harmonics_order, harmonics_bandwidth, harmonics_enabled, harmonics_measurement_interval
            )
        specan.harm.configuration.set_measurement_method("", measurement_method)
        specan.harm.configuration.set_noise_compensation_enabled("", noise_compensation_enabled)
        specan.harm.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.initiate("", "")

        total_harmonic_distortion, average_fundamental_power, fundamental_frequency, error_code = (
            specan.harm.results.fetch_total_harmonic_distortion("", timeout)
        )

        for i in range(NUMBER_OF_HARMONICS):
            harmonic_string = nirfmxspecan.SpecAn.build_harmonic_string("", i)
            power = numpy.empty(0, dtype=numpy.float32)
            specan.harm.results.fetch_harmonic_power_trace(harmonic_string, timeout, power)

        average_relative_power, average_absolute_power, harmonics_rbw, harmonics_frequency, error_code = (
            specan.harm.results.fetch_harmonic_measurement_array("", timeout)
        )

        print("Measurement\n")
        print(f"Total Harmonic Distortion (%)    {total_harmonic_distortion}")
        print(f"Average Fundamental Power (dBm)  {average_fundamental_power}")
        print(f"Fundamental Frequency (Hz)       {fundamental_frequency}")

        print("\n----------------Harmonics----------------------\n")
        for i in range(NUMBER_OF_HARMONICS):
            print(f"Harmonic {i + 1}:")
            print(f"Harmonics Frequency    (Hz)       : {harmonics_frequency[i]}")
            print(f"Harmonics RBW          (Hz)       : {harmonics_rbw[i]}")
            print(f"Average Absolute Power (dBm)      : {average_absolute_power[i]}")
            print(f"Average Relative Power (dB)       : {average_relative_power[i]}")
            print("---------------------------------------------\n")

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
        description="Pass arguments for Harmonics Dynamic Range Example",
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
