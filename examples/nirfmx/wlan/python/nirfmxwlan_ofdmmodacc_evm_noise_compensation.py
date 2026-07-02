"""
RFmx WLAN OFDMModAcc EVM Noise Compensation Example

Instructions:
1. This example demonstrates the use of RFmxWLAN OFDM ModAcc measurement to compute EVM after
   compensating for the noise attributed to the VSA.
2. The example uses an enum control, "Calibrate Noise Floor" with two values:
   Disabled (0): select this to skip calibrating the VSA noise floor, and perform the
   OFDMModAcc measurement directly. You may want to do this when the VSA noise floor has
   already been calibrated or when Noise Compensation is disabled.
   Enabled (1): select this to first calibrate the VSA noise floor, and then perform the
   OFDMModAcc measurement.

Follow these steps to calibrate VSA noise, and then perform ModAcc measurement:
1. Set Calibrate Noise Floor to "Enabled".
2. Run the example.
3. When "Turn OFF Generation" dialog box appears, ensure that signal generation is turned OFF
   and then click "OK". Wait for calibration to complete.
4. When "Turn ON Generation" dialog box appears, ensure that signal generation is turned ON
   and then click "OK".

Follow these steps to skip (re)calibrating and directly perform ModAcc measurement:
1. Set Calibrate Noise Floor to "Disabled".
2. Run the example.

Steps:
1.  Open a new RFmx session.
2.  Configure the frequency reference properties (Clock Source and Clock Frequency).
3.  Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4.  Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
5.  Configure Standard and Channel Bandwidth.
6.  Select OFDMModAcc measurement and enable the traces.
7.  Configure Optimize Dynamic Range for EVM.
8.  Configure Measurement Mode as Calibrate Noise Floor.
9.  Initiate Measurement.
10. Wait for Measurement Complete.
11. Configure Measurement Mode as Measure.
12. Configure Measurement Interval.
13. Configure Averaging parameters.
14. Configure Noise Compensation Enabled.
15. Initiate Measurement.
16. Fetch OFDMModAcc Measurements.
17. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxwlan
import numpy


def example(resource_name, option_string, calibrate_noise_floor=True):
    """WLAN OFDMModAcc EVM Noise Compensation measurement example."""

    # Configuration parameters
    center_frequency = 2.412e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    iq_power_edge_enabled = True
    iq_power_edge_level = -20.0  # dB
    trigger_delay = 0.0  # s
    minimum_quiet_time_mode = nirfmxwlan.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 5.0e-6  # s

    standard = nirfmxwlan.Standard.STANDARD_802_11_AG
    channel_bandwidth = 20e6  # Hz

    averaging_enabled = nirfmxwlan.OfdmModAccAveragingEnabled.FALSE
    averaging_count = 10

    noise_compensation_enabled = nirfmxwlan.OfdmModAccNoiseCompensationEnabled.TRUE
    optimize_dynamic_range_for_evm_enabled = (
        nirfmxwlan.OfdmModAccOptimizeDynamicRangeForEvmEnabled.TRUE
    )
    optimize_dynamic_range_for_evm_margin = 0.0  # dB

    measurement_offset = 0  # symbols
    maximum_measurement_length = 16  # symbols

    timeout = 10.0  # s

    instr_session = None
    wlan_signal = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        wlan_signal = instr_session.get_wlan_signal_configuration()

        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )
        wlan_signal.configure_frequency("", center_frequency)
        wlan_signal.configure_reference_level("", reference_level)
        wlan_signal.configure_external_attenuation("", external_attenuation)

        wlan_signal.configure_iq_power_edge_trigger(
            "",
            "0",
            nirfmxwlan.IQPowerEdgeTriggerSlope.RISING_SLOPE,
            iq_power_edge_level,
            trigger_delay,
            minimum_quiet_time_mode,
            minimum_quiet_time,
            nirfmxwlan.IQPowerEdgeTriggerLevelType.RELATIVE,
            iq_power_edge_enabled,
        )

        wlan_signal.configure_standard("", standard)
        wlan_signal.configure_channel_bandwidth("", channel_bandwidth)

        wlan_signal.select_measurements("", nirfmxwlan.MeasurementTypes.OFDMMODACC, True)

        wlan_signal.ofdmmodacc.configuration.configure_optimize_dynamic_range_for_evm(
            "",
            optimize_dynamic_range_for_evm_enabled,
            optimize_dynamic_range_for_evm_margin,
        )

        if calibrate_noise_floor:
            # Calibrate the VSA noise floor
            # Turn OFF signal generation before initiating calibration
            wlan_signal.ofdmmodacc.configuration.configure_measurement_mode(
                "", nirfmxwlan.OfdmModAccMeasurementMode.CALIBRATE_NOISE_FLOOR
            )
            print(
                "Turn OFF signal generation and press Enter to start noise floor calibration..."
            )
            input()
            wlan_signal.initiate("", "")
            wlan_signal.wait_for_measurement_complete("", timeout)
            print("Noise floor calibration complete. Turn ON signal generation and press Enter...")
            input()

        # Perform the ModAcc measurement
        wlan_signal.ofdmmodacc.configuration.configure_measurement_mode(
            "", nirfmxwlan.OfdmModAccMeasurementMode.MEASURE
        )
        wlan_signal.ofdmmodacc.configuration.configure_measurement_length(
            "", measurement_offset, maximum_measurement_length
        )
        wlan_signal.ofdmmodacc.configuration.configure_averaging(
            "", averaging_enabled, averaging_count
        )
        wlan_signal.ofdmmodacc.configuration.configure_noise_compensation_enabled(
            "", noise_compensation_enabled
        )

        wlan_signal.initiate("", "")

        (
            composite_rms_evm_mean,
            composite_data_rms_evm_mean,
            composite_pilot_rms_evm_mean,
            error_code,
        ) = wlan_signal.ofdmmodacc.results.fetch_composite_rms_evm("", timeout)

        pilot_constellation = numpy.empty(0, dtype=numpy.complex64)
        error_code = wlan_signal.ofdmmodacc.results.fetch_pilot_constellation_trace(
            "", timeout, pilot_constellation
        )

        data_constellation = numpy.empty(0, dtype=numpy.complex64)
        error_code = wlan_signal.ofdmmodacc.results.fetch_data_constellation_trace(
            "", timeout, data_constellation
        )

        chain_rms_evm_per_subcarrier_mean = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = (
            wlan_signal.ofdmmodacc.results.fetch_chain_rms_evm_per_subcarrier_mean_trace(
                "", timeout, chain_rms_evm_per_subcarrier_mean
            )
        )

        # Print results
        print("------------------Composite EVM------------------")
        print(f"RMS EVM Mean (dB)                       :{composite_rms_evm_mean}")
        print(f"Data RMS EVM Mean (dB)                  :{composite_data_rms_evm_mean}")
        print(f"Pilot RMS EVM Mean (dB)                 :{composite_pilot_rms_evm_mean}")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close Session
        if wlan_signal is not None:
            wlan_signal.dispose()
            wlan_signal = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for WLAN OFDMModAcc EVM Noise Compensation Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instrument"
    )
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    parser.add_argument(
        "--calibrate",
        action="store_true",
        default=False,
        help="Calibrate noise floor before measurement",
    )
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string, args.calibrate)


def main():
    """Call _main function."""
    _main(sys.argv[1:])


def test_main():
    """Call _main function with empty option string."""
    cmd_line = [
        "--resource-name",
        "RFSA",
        "--option-string",
        "",
    ]
    _main(cmd_line)


def test_example():
    """Call example function."""
    example("RFSA", "", calibrate_noise_floor=False)


if __name__ == "__main__":
    main()
