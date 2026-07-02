"""
RFmx LTE ACP Contiguous Multi-Carrier Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, RF Attenuation and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Component Carrier Spacing.
6. Configure Component Carriers.
7. Configure Reference Level.
8. Configure Duplex Mode.
9. Configure Link Direction.
10. Select ACP measurement and enable Traces.
11. Configure Measurement Method.
12. Configure Averaging Parameters for ACP measurement.
13. Configure Sweep Time Parameters.
14. Configure Noise Compensation Parameter.
15. Initiate the Measurement.
16. Fetch ACP Measurements and Traces.
17. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxlte
import numpy


def example(resource_name, option_string):
    """LTE ACP contiguous multi-carrier measurement example."""
    # Configuration parameters
    center_frequency = 1.95e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    auto_level = True
    measurement_interval = 0.01  # seconds

    rf_attenuation_auto = nirfmxinstr.RFAttenuationAuto.TRUE
    rf_attenuation = 10.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    enable_trigger = False
    digital_edge_source = "PFI0"
    digital_edge = nirfmxlte.DigitalEdgeTriggerEdge.RISING_EDGE
    trigger_delay = 0.0  # seconds

    uplink_downlink_configuration = nirfmxlte.UplinkDownlinkConfiguration.CONFIGURATION_0
    duplex_scheme = nirfmxlte.DuplexScheme.FDD
    link_direction = nirfmxlte.LinkDirection.UPLINK

    number_of_component_carriers = 2
    component_carrier_spacing_type = nirfmxlte.ComponentCarrierSpacingType.NOMINAL
    component_carrier_at_center_frequency = -1
    component_carrier_bandwidth = [20e6, 20e6]  # Hz
    component_carrier_frequency = [-9.9e6, 9.9e6]  # Hz

    measurement_method = nirfmxlte.AcpMeasurementMethod.NORMAL
    noise_compensation_enabled = nirfmxlte.AcpNoiseCompensationEnabled.FALSE
    sweep_time_auto = nirfmxlte.AcpSweepTimeAuto.TRUE
    sweep_time_interval = 0.001  # seconds

    averaging_enabled = nirfmxlte.AcpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxlte.AcpAveragingType.RMS

    number_of_offsets = 3
    timeout = 10.0  # seconds

    instr_session = None
    lte_signal = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get LTE signal configuration
        lte_signal = instr_session.get_lte_signal_configuration()

        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )

        lte_signal.configure_frequency("", center_frequency)
        lte_signal.configure_external_attenuation("", external_attenuation)
        instr_session.configure_rf_attenuation("", rf_attenuation_auto, rf_attenuation)

        lte_signal.configure_digital_edge_trigger(
            "", digital_edge_source, digital_edge, trigger_delay, enable_trigger
        )

        lte_signal.component_carrier.configure_spacing(
            "", component_carrier_spacing_type, component_carrier_at_center_frequency
        )

        lte_signal.configure_number_of_component_carriers("", number_of_component_carriers)

        lte_signal.component_carrier.configure_array(
            "", component_carrier_bandwidth, component_carrier_frequency, None
        )

        if auto_level:
            auto_set_reference_level, error_code = lte_signal.auto_level("", measurement_interval)
            print(f"Reference level (dBm)  : {auto_set_reference_level}\n")
        else:
            lte_signal.configure_reference_level("", reference_level)

        lte_signal.configure_duplex_scheme("", duplex_scheme, uplink_downlink_configuration)
        lte_signal.configure_link_direction("", link_direction)

        lte_signal.select_measurements("", nirfmxlte.MeasurementTypes.ACP, True)

        lte_signal.acp.configuration.configure_measurement_method("", measurement_method)
        lte_signal.acp.configuration.configure_averaging(
            "", averaging_enabled, averaging_count, averaging_type
        )
        lte_signal.acp.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        lte_signal.acp.configuration.configure_noise_compensation_enabled(
            "", noise_compensation_enabled
        )

        lte_signal.initiate("", "")

        (
            lower_relative_power,
            upper_relative_power,
            lower_absolute_power,
            upper_absolute_power,
            error_code,
        ) = lte_signal.acp.results.fetch_offset_measurement_array("", timeout)

        total_aggregated_power, error_code = lte_signal.acp.results.fetch_total_aggregated_power(
            "", timeout
        )

        for i in range(number_of_offsets):
            absolute_powers_trace = numpy.empty(0, dtype=numpy.float32)
            x0, dx, error_code = lte_signal.acp.results.fetch_absolute_powers_trace(
                "", timeout, i, absolute_powers_trace
            )

        for i in range(number_of_offsets):
            relative_powers_trace = numpy.empty(0, dtype=numpy.float32)
            x0, dx, error_code = lte_signal.acp.results.fetch_relative_powers_trace(
                "", timeout, i, relative_powers_trace
            )

        spectrum = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = lte_signal.acp.results.fetch_spectrum("", timeout, spectrum)

        # Print results
        print(f"Total Aggregated Power  (dBm)  : {total_aggregated_power}")
        print("\n----------Offset Channel Measurements----------")
        for i in range(len(lower_relative_power)):
            print(f"\nOffset  : {i}")
            print(f"Lower Relative Power (dB)  : {lower_relative_power[i]}")
            print(f"Upper Relative Power (dB)  : {upper_relative_power[i]}")
            print(f"Lower Absolute Power (dBm) : {lower_absolute_power[i]}")
            print(f"Upper Absolute Power (dBm) : {upper_absolute_power[i]}")
            print("---------------------------------------------")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close Session
        if lte_signal is not None:
            lte_signal.dispose()
            lte_signal = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for LTE ACP Contiguous Multi-Carrier Example",
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
