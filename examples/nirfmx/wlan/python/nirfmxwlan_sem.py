"""
RFmx WLAN SEM Example

Steps:
1. Open a new RFmx session.
2. Configure the frequency reference properties(Clock Source and Clock Frequency).
3. Configure the basic signal properties(Center Frequency, Reference Level and External Attenuation).
4. Configure IQ Power Edge Trigger properties(Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
5. Configure Standard and Channel Bandwidth Properties.
6. Select SEM measurement and enable the traces.
7. Configure Averaging parameters.
8. Configure Sweep Time and Span parameters.
9. Initiate Measurement.
10. Fetch SEM Traces and Measurements.
11. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxwlan
import numpy


def example(resource_name: str, option_string: str):
    """WLAN SEM measurement example (single device)."""

    # Configuration parameters
    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    center_frequency = 2.412e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    iq_power_edge_enabled = True
    iq_power_edge_level = -20.0  # dB
    trigger_delay = 0.0  # s
    minimum_quiet_time_mode = nirfmxwlan.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 5.0e-6  # s

    standard = nirfmxwlan.Standard.STANDARD_802_11_AG
    channel_bandwidth = 20e6  # Hz

    averaging_enabled = nirfmxwlan.SemAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxwlan.SemAveragingType.RMS

    span_auto = nirfmxwlan.SemSpanAuto.TRUE
    span = 66.0e6  # Hz

    sweep_time_auto = nirfmxwlan.SemSweepTimeAuto.TRUE
    sweep_time = 1.0e-3  # s

    mask_type = nirfmxwlan.SemMaskType.STANDARD

    timeout = 10.0  # s

    instr_session = None
    wlan_signal = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get WLAN Signal
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
        wlan_signal.select_measurements("", nirfmxwlan.MeasurementTypes.SEM, True)
        wlan_signal.sem.configuration.configure_averaging(
            "", averaging_enabled, averaging_count, averaging_type
        )
        wlan_signal.sem.configuration.configure_mask_type("", mask_type)
        wlan_signal.sem.configuration.configure_sweep_time("", sweep_time_auto, sweep_time)
        wlan_signal.sem.configuration.configure_span("", span_auto, span)

        wlan_signal.initiate("", "")

        # Retrieve results
        measurement_status, error_code = wlan_signal.sem.results.fetch_measurement_status(
            "", timeout
        )

        absolute_power, relative_power, error_code = (
            wlan_signal.sem.results.fetch_carrier_measurement("", timeout)
        )

        (
            lower_offset_measurement_status,
            lower_offset_margin,
            lower_offset_margin_frequency,
            lower_offset_margin_absolute_power,
            lower_offset_margin_relative_power,
            error_code,
        ) = wlan_signal.sem.results.fetch_lower_offset_margin_array("", timeout)

        (
            upper_offset_measurement_status,
            upper_offset_margin,
            upper_offset_margin_frequency,
            upper_offset_margin_absolute_power,
            upper_offset_margin_relative_power,
            error_code,
        ) = wlan_signal.sem.results.fetch_upper_offset_margin_array("", timeout)

        spectrum = numpy.empty(0, dtype=numpy.float32)
        composite_mask = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = wlan_signal.sem.results.fetch_spectrum(
            "", timeout, spectrum, composite_mask
        )

        # Print Results
        print(f"Measurement Status                          :{measurement_status.name}")
        print(f"Carrier Absolute Power (dBm)                :{absolute_power}")

        print("----------Lower Offset Measurements----------\n")
        for i in range(len(lower_offset_margin)):
            print(f"Offset {i}")
            print(f"Measurement Status              :{lower_offset_measurement_status[i].name}")
            print(f"Margin (dB)                     :{lower_offset_margin[i]}")
            print(f"Margin Frequency (Hz)           :{lower_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power (dBm)     :{lower_offset_margin_absolute_power[i]}\n")

        print("\n----------Upper Offset Measurements----------\n")
        for i in range(len(upper_offset_margin)):
            print(f"Offset {i}")
            print(f"Measurement Status              :{upper_offset_measurement_status[i].name}")
            print(f"Margin (dB)                     :{upper_offset_margin[i]}")
            print(f"Margin Frequency (Hz)           :{upper_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power (dBm)     :{upper_offset_margin_absolute_power[i]}\n")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close session
        if wlan_signal is not None:
            wlan_signal.dispose()
            wlan_signal = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for WLAN SEM Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="RFSA", type=str, help="Resource name of NI-RFmx Instr."
    )
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string)


def main():
    """Call _main function."""
    _main(sys.argv[1:])


def test_main():
    """Call _main function with defaults."""
    cmd_line = ["--resource-name", "RFSA", "--option-string", ""]
    _main(cmd_line)


def test_example():
    """Call example function."""
    example("RFSA", "")


if __name__ == "__main__":
    main()
