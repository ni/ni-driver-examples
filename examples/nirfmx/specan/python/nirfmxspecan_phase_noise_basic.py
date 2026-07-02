r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source and Clock Frequency).
3. Configure Selected Ports.
4. Configure instrument RF Attenuation.
5. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
6. Configure PhaseNoise measurement and enable the traces.
7. Configure Auto Range.
8. Configure Averaging Multiplier.
9. Configure Smoothing.
10. Initiate Measurement.
11. Fetch PhaseNoise Measurements and Traces.
12. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9   # Hz
    reference_level = 0.0      # dBm
    external_attenuation = 0.0  # dB
    timeout = 10.0             # seconds

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10.0e6  # Hz

    enable_all_traces = True

    autolevel = True
    bandwidth = 200.0e3     # Hz
    measurement_interval = 10.0e-3  # seconds

    rf_attenuation_auto = nirfmxinstr.RFAttenuationAuto.TRUE
    rf_attenuation = 10.0   # dB

    # Phase noise span
    start_frequency = 1.0e3   # Hz offset
    stop_frequency = 1.0e6    # Hz offset
    rbw_percentage = 10.0     # %

    averaging_multiplier = 1

    smoothing_type = nirfmxspecan.PhaseNoiseSmoothingType.LOGARITHMIC
    smoothing_percentage = 2.0  # %

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)
        specan.set_selected_ports("", selected_ports)
        instr_session.configure_rf_attenuation("", rf_attenuation_auto, rf_attenuation)
        specan.configure_frequency("", center_frequency)
        specan.configure_external_attenuation("", external_attenuation)

        if autolevel:
            reference_level, error_code = specan.auto_level("", bandwidth, measurement_interval)
            print(f"Reference level(dBm)         : {reference_level}")
        else:
            specan.configure_reference_level("", reference_level)

        specan.select_measurements("", nirfmxspecan.MeasurementTypes.PHASENOISE, enable_all_traces)
        specan.phase_noise.configuration.configure_auto_range("", start_frequency, stop_frequency, rbw_percentage)
        specan.phase_noise.configuration.configure_averaging_multiplier("", averaging_multiplier)
        specan.phase_noise.configuration.configure_smoothing("", smoothing_type, smoothing_percentage)
        specan.initiate("", "")

        carrier_frequency, carrier_power, error_code = (
            specan.phase_noise.results.fetch_carrier_measurement("", timeout)
        )
        (
            integrated_phase_noise,
            residual_pm_in_radian,
            residual_pm_in_degree,
            residual_fm,
            jitter,
            error_code,
        ) = specan.phase_noise.results.fetch_integrated_noise("", timeout)
        measured_frequency, measured_phase_noise, error_code = (
            specan.phase_noise.results.fetch_measured_log_plot_trace("", timeout)
        )
        smoothed_frequency, smoothed_phase_noise, error_code = (
            specan.phase_noise.results.fetch_smoothed_log_plot_trace("", timeout)
        )

        print("\nCarrier Measurement\n")
        print(f"Carrier Frequency(Hz)        : {carrier_frequency}")
        print(f"Carrier Power(dBm)           : {carrier_power}")

        print("\nIntegrated Noise\n")
        print(f"Integrated Phase Noise(dBc)  : {integrated_phase_noise[0]}")
        print(f"Residual PM(rad)             : {residual_pm_in_radian[0]}")
        print(f"Residual PM(deg)             : {residual_pm_in_degree[0]}")
        print(f"Residual FM(Hz)              : {residual_fm[0]}")
        print(f"Jitter(s)                    : {jitter[0]}")

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
        description="Pass arguments for Phase Noise Basic Example",
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
