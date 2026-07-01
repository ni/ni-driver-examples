r"""Steps:
1. Open a new RFmx Session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Min Quiet Time).
6. Select DDemod Measurement and enable the traces.
7. Configure Modulation Type.
8. Configure DDemod Symbol Rate, Sample Per Symbol, Number of Symbols.
9. Configure DDemod PSK Format and EVM Norm Reference.
10. Configure DDemod FSK Deviation.
11. Configure DDemod Pulse Shaping Filter and Measurement Filter.
12. Configure DDemod Equalizer, Synchronization, Signal Structure and Burst Exclusion.
13. Configure DDemod Averaging.
14. Initiate Measurement.
15. Fetch DDemod Measurement Results.
16. Close the RFmx Session.
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

    frequency = 10.0e6  # Hz
    frequency_source = "OnboardClock"

    iq_power_edge_enabled = False
    iq_power_edge_level = -20.00  # dBm
    trigger_delay = 0.00  # seconds
    min_quiet_time = 0.00  # seconds

    samples_per_symbol = -1
    fsk_deviation = 15.000e3  # Hz
    apsk_r2_to_r1_ratio = 2.84
    apsk_r3_to_r1_ratio = 5.27
    symbol_rate = 100.000e3  # Hz
    num_of_symbols = 1000

    measurement_offset = 0

    # Averaging
    averaging_count = 10

    # Signal Structure
    signal_structure = nirfmxdemod.DDemodSignalStructure.CONTINUOUS

    # Burst Start Exclusion Symbols
    burst_start_exclusion_symbols = 0
    # Burst End Exclusion Symbols
    burst_end_exclusion_symbols = 0

    pulse_shaping_filter_alpha_or_bt = 0.50
    pulse_shaping_filter_custom_coefficient_x0 = 0
    pulse_shaping_filter_custom_coefficient_dx = 1.00e0
    pulse_shaping_filter_custom_coefficients = numpy.empty(0, dtype=numpy.float32)

    measurement_filter_custom_coefficient_x0 = 0.00e0
    measurement_filter_custom_coefficient_dx = 1.00e0
    measurement_filter_custom_coefficients = numpy.empty(0, dtype=numpy.float32)

    equalizer_length = 20
    equalizer_training_count = 10
    equalizer_convergence_factor = 0.01e0
    equalizer_initial_coefficients = numpy.empty(0, dtype=numpy.complex64)

    sync_bits = numpy.empty(0, dtype=numpy.int8)

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

        demod.configure_iq_power_edge_trigger(
            "",
            "0",
            iq_power_edge_level,
            nirfmxdemod.IQPowerEdgeTriggerSlope.RISING_SLOPE,
            trigger_delay,
            nirfmxdemod.TriggerMinimumQuietTimeMode.MANUAL,
            min_quiet_time,
            iq_power_edge_enabled,
        )

        demod.select_measurements("", nirfmxdemod.MeasurementTypes.DDEMOD, True)
        demod.digital_demod.configuration.configure_modulation_type(
            "",
            nirfmxdemod.DDemodModulationType.PSK,
            nirfmxdemod.DDemodM.M4,
            nirfmxdemod.DDemodDifferentialEnabled.FALSE,
        )
        demod.digital_demod.configuration.configure_symbol_rate("", symbol_rate)
        demod.digital_demod.configuration.configure_samples_per_symbol("", samples_per_symbol)
        demod.digital_demod.configuration.configure_number_of_symbols("", num_of_symbols)

        demod.digital_demod.configuration.configure_psk_format("", nirfmxdemod.DDemodPskFormat.NORMAL)
        demod.digital_demod.configuration.configure_evm_normalization_reference(
            "", nirfmxdemod.DDemodEvmNormalizationReference.PEAK
        )
        demod.digital_demod.configuration.configure_fsk_deviation(
            "", fsk_deviation, nirfmxdemod.DDemodFskReferenceCompensationEnabled.FALSE
        )
        demod.digital_demod.configuration.set_apsk_r2_to_r1_ratio("", apsk_r2_to_r1_ratio)
        demod.digital_demod.configuration.set_apsk_r3_to_r1_ratio("", apsk_r3_to_r1_ratio)
        demod.digital_demod.configuration.configure_pulse_shaping_filter(
            "",
            nirfmxdemod.DDemodPulseShapingFilterType.ROOT_RAISED_COSINE,
            pulse_shaping_filter_alpha_or_bt,
            pulse_shaping_filter_custom_coefficient_x0,
            pulse_shaping_filter_custom_coefficient_dx,
            pulse_shaping_filter_custom_coefficients,
        )
        demod.digital_demod.configuration.configure_measurement_filter(
            "",
            nirfmxdemod.DDemodMeasurementFilterType.AUTO,
            measurement_filter_custom_coefficient_x0,
            measurement_filter_custom_coefficient_dx,
            measurement_filter_custom_coefficients,
        )
        demod.digital_demod.configuration.configure_equalizer(
            "",
            nirfmxdemod.DDemodEqualizerMode.OFF,
            equalizer_length,
            0.0,
            1.0,
            equalizer_initial_coefficients,
            equalizer_training_count,
            equalizer_convergence_factor,
        )
        demod.digital_demod.configuration.configure_synchronization(
            "", nirfmxdemod.DDemodSynchronizationEnabled.FALSE, sync_bits, measurement_offset
        )
        demod.digital_demod.configuration.configure_averaging(
            "", nirfmxdemod.DDemodAveragingEnabled.FALSE, averaging_count
        )
        demod.digital_demod.configuration.configure_signal_structure("", signal_structure)
        demod.digital_demod.configuration.set_burst_start_exclusion_symbols(
            "", burst_start_exclusion_symbols
        )
        demod.digital_demod.configuration.set_burst_end_exclusion_symbols("", burst_end_exclusion_symbols)
        demod.initiate("", "")

        # Retrieve results
        mean_carrier_frequency_error, mean_frequency_drift, mean_carrier_phase_error, error_code = (
            demod.digital_demod.results.fetch_carrier_measurement("", timeout)
        )
        mean_rms_evm, max_rms_evm, mean_mer, max_peak_evm, mean_peak_evm, error_code = (
            demod.digital_demod.results.fetch_evm("", timeout)
        )
        (
            mean_rms_offset_evm,
            max_rms_offset_evm,
            max_peak_offset_evm,
            mean_peak_offset_evm,
            error_code,
        ) = demod.digital_demod.results.fetch_offset_evm("", timeout)
        mean_magnitude_error, max_magnitude_error, error_code = (
            demod.digital_demod.results.fetch_magnitude_error("", timeout)
        )
        mean_phase_error, max_phase_error, error_code = demod.digital_demod.results.fetch_phase_error(
            "", timeout
        )
        mean_fsk_deviation, mean_rms_fsk_error, max_peak_fsk_error, error_code = (
            demod.digital_demod.results.fetch_fsk_results("", timeout)
        )
        mean_iq_gain_imbalance, mean_quadrature_skew, mean_iq_origin_offset, error_code = (
            demod.digital_demod.results.fetch_iq_impairments("", timeout)
        )
        sync_found, error_code = demod.digital_demod.results.fetch_sync_found("", timeout)
        mean_rho_factor, error_code = demod.digital_demod.results.fetch_mean_rho_factor("", timeout)
        mean_amplitude_droop, error_code = demod.digital_demod.results.fetch_mean_amplitude_droop(
            "", timeout
        )
        constellation_trace = numpy.empty(0, dtype=numpy.complex64)
        error_code = demod.digital_demod.results.fetch_constellation_trace(
            "", timeout, constellation_trace
        )
        evm_trace = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = demod.digital_demod.results.fetch_evm_trace("", timeout, evm_trace)
        offset_evm_trace = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = demod.digital_demod.results.fetch_offset_evm_trace(
            "", timeout, offset_evm_trace
        )

        # Print Results
        print("-------------------Carrier measurements----------")
        print(f"Mean Carrier Frequency Error(Hz)        : {mean_carrier_frequency_error}")
        print(f"Mean Frequency Drift (Hz)               : {mean_frequency_drift}")
        print(f"Mean Phase Error (deg)                  : {mean_carrier_phase_error}")

        print("\n---------------------------EVM-----------------")
        print(f"Mean MER (dB)                           : {mean_mer}")
        print(f"Mean RMS EVM (%)                        : {mean_rms_evm}")
        print(f"Maximum RMS EVM (%)                     : {max_rms_evm}")
        print(f"Mean Peak EVM (%)                       : {mean_peak_evm}")
        print(f"Maximum Peak EVM (%)                    : {max_peak_evm}")
        print(f"Mean RMS Offset EVM (%)                 : {mean_rms_offset_evm}")
        print(f"Maximum RMS Offset EVM (%)              : {max_rms_offset_evm}")
        print(f"Mean Peak Offset EVM (%)                : {mean_peak_offset_evm}")
        print(f"Maximum Peak Offset EVM (%)             : {max_peak_offset_evm}")

        print("\n--------------------------FSK Results--------------")
        print(f"Mean Deviation (Hz)                     : {mean_fsk_deviation}")
        print(f"Mean RMS FSK Error (Hz)                 : {mean_rms_fsk_error}")
        print(f"Maximum Peak FSK Error (%)              : {max_peak_fsk_error}")

        print("\n--------------------------Measurements------------")
        if sync_found:
            print("Sync Found is True\n")
        else:
            print("Sync Found is False\n")

        print(f"Mean Magnitude Error (%)                : {mean_magnitude_error}")
        print(f"Maximum Magnitude Error (%)             : {max_magnitude_error}")
        print(f"Mean Phase Error (deg)                  : {mean_phase_error}")
        print(f"Maximum Phase Error (deg)               : {max_phase_error}")
        print(f"Mean IQ Origin Offset (dB)              : {mean_iq_origin_offset}")
        print(f"Mean IQ Gain Imbalance (dB)             : {mean_iq_gain_imbalance}")
        print(f"Mean Quadrature Skew (deg)              : {mean_quadrature_skew}")
        print(f"Mean Rho Factor                         : {mean_rho_factor}")
        print(f"Mean Amplitude Droop (dB/Symbol)        : {mean_amplitude_droop}")

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
        description="Pass arguments for DDemod Advanced Example",
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
    options = {}
    example("RFSA", options)


if __name__ == "__main__":
    main()
