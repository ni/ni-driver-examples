"""
RFmx WLAN DSSS Power Ramp Example

Steps:
1. Open a new RFmx session.
2. Configure the frequency reference properties (Clock Source and Clock Frequency).
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure IQ Power Edge Trigger properties (Trigger Delay, IQ Power Edge Level, Minimum Quiet Time).
5. Configure Standard as 802.11b.
6. Select PowerRamp measurement and enable the traces.
7. Configure the Acquisition Length.
8. Configure Averaging parameters.
9. Initiate Measurement.
10. Fetch PowerRamp Traces and Measurements.
11. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxwlan
import numpy


def example(resource_name, option_string):
    """WLAN DSSS Power Ramp measurement example."""

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

    standard = nirfmxwlan.Standard.STANDARD_802_11_B

    acquisition_length = 1e-3  # s

    averaging_enabled = nirfmxwlan.PowerRampAveragingEnabled.FALSE
    averaging_count = 10

    timeout = 10.0  # s

    instr_session = None
    wlan_signal = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get WLAN signal configuration
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

        wlan_signal.select_measurements("", nirfmxwlan.MeasurementTypes.POWERRAMP, True)

        wlan_signal.powerramp.configuration.configure_acquisition_length("", acquisition_length)
        wlan_signal.powerramp.configuration.configure_averaging("", averaging_enabled, averaging_count)

        wlan_signal.initiate("", "")

        rise_time_mean, fall_time_mean, error_code = (
            wlan_signal.powerramp.results.fetch_measurement("", timeout)
        )

        rise_raw_waveform = numpy.empty(0, dtype=numpy.float32)
        rise_processed_waveform = numpy.empty(0, dtype=numpy.float32)
        rise_threshold = numpy.empty(0, dtype=numpy.float32)
        rise_power_reference = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = wlan_signal.powerramp.results.fetch_rise_trace(
            "", timeout, rise_raw_waveform, rise_processed_waveform, rise_threshold, rise_power_reference
        )

        fall_raw_waveform = numpy.empty(0, dtype=numpy.float32)
        fall_processed_waveform = numpy.empty(0, dtype=numpy.float32)
        fall_threshold = numpy.empty(0, dtype=numpy.float32)
        fall_power_reference = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = wlan_signal.powerramp.results.fetch_fall_trace(
            "", timeout, fall_raw_waveform, fall_processed_waveform, fall_threshold, fall_power_reference
        )

        # Print results
        print("\n---------------Measurement---------------\n")
        print(f"Rise Time (s)                     :{rise_time_mean}")
        print(f"Fall Time (s)                     :{fall_time_mean}")

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
        description="Pass arguments for WLAN DSSS Power Ramp Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instrument"
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
        "--resource-name",
        "RFSA",
        "--option-string",
        "",
    ]
    _main(cmd_line)


def test_example():
    """Call example function."""
    example("RFSA", "")


if __name__ == "__main__":
    main()
