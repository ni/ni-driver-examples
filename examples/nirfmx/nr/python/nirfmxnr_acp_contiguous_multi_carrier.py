r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure Selected Ports.
4. Configure basic signal properties (Center Frequency, RF Attenuation and External Attenuation).
5. Configure Trigger Parameters for Digital Edge Trigger.
6. Configure Link Direction, Frequency Range, Channel Raster, Component Carrier Spacing and Number of Component Carriers.
7. Configure Subcarrier Spacing.
8. Configure Component Carriers.
9. Configure Reference Level.
10. Select ACP measurement and enable Traces.
11. Configure Measurement Method.
12. Configure Noise Compensation Parameter.
13. Configure Sweep Time Parameters.
14. Configure Averaging Parameters for ACP measurement.
15. Initiate the Measurement.
16. Fetch ACP Measurements and Traces.
17. Close RFmx Session.
"""

import argparse
import sys

import nirfmxnr
import numpy

import nirfmxinstr

NUMBER_OF_COMPONENT_CARRIERS = 2


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 3.5e9  # Hz
    external_attenuation = 0.0  # dB

    auto_level = True
    reference_level = 0.0  # dBm
    measurement_interval = 10.0e-3  # s

    rf_attenuation_auto = nirfmxinstr.RFAttenuationAuto.TRUE
    rf_attenuation = 10.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10.0e6  # Hz

    enable_trigger = False
    digital_edge_source = "PXI_Trig0"
    digital_edge = nirfmxnr.DigitalEdgeTriggerEdge.RISING_EDGE
    trigger_delay = 0.0  # s

    link_direction = nirfmxnr.LinkDirection.UPLINK
    frequency_range = nirfmxnr.FrequencyRange.RANGE1

    component_carrier_spacing_type = nirfmxnr.ComponentCarrierSpacingType.NOMINAL
    channel_raster = 15e3  # Hz
    component_carrier_at_center_frequency = -1
    subcarrier_spacing = 30e3  # Hz

    component_carrier_bandwidth = [100e6, 100e6]  # Hz
    component_carrier_frequency = [-49.98e6, 50.01e6]  # Hz

    measurement_method = nirfmxnr.AcpMeasurementMethod.NORMAL
    noise_compensation_enabled = nirfmxnr.AcpNoiseCompensationEnabled.FALSE

    sweep_time_auto = nirfmxnr.AcpSweepTimeAuto.TRUE
    sweep_time_interval = 1.0e-3  # s

    averaging_enabled = nirfmxnr.AcpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxnr.AcpAveragingType.RMS

    timeout = 10.0  # s

    instr_session = None
    nr = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        nr = instr_session.get_nr_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)
        nr.set_selected_ports("", selected_ports)
        nr.configure_frequency("", center_frequency)
        nr.configure_external_attenuation("", external_attenuation)
        instr_session.configure_rf_attenuation("", rf_attenuation_auto, rf_attenuation)
        nr.configure_digital_edge_trigger("", digital_edge_source, digital_edge, trigger_delay, enable_trigger)

        nr.set_link_direction("", link_direction)
        nr.set_frequency_range("", frequency_range)
        nr.set_channel_raster("", channel_raster)
        nr.set_component_carrier_spacing_type("", component_carrier_spacing_type)
        nr.set_component_carrier_at_center_frequency("", component_carrier_at_center_frequency)
        nr.component_carrier.set_number_of_component_carriers("", NUMBER_OF_COMPONENT_CARRIERS)

        nr.component_carrier.set_bandwidth_part_subcarrier_spacing("carrier::all", subcarrier_spacing)

        subblock_string = nirfmxnr.NR.build_subblock_string("", 0)
        for i in range(NUMBER_OF_COMPONENT_CARRIERS):
            carrier_string = nirfmxnr.NR.build_carrier_string(subblock_string, i)
            nr.component_carrier.set_bandwidth(carrier_string, component_carrier_bandwidth[i])
            nr.component_carrier.set_frequency(carrier_string, component_carrier_frequency[i])

        if auto_level:
            reference_level, error_code = nr.auto_level("", measurement_interval)
            print(f"Reference level (dBm)           : {reference_level}\n")
        else:
            nr.configure_reference_level("", reference_level)

        nr.select_measurements("", nirfmxnr.MeasurementTypes.ACP, True)
        nr.acp.configuration.configure_measurement_method("", measurement_method)
        nr.acp.configuration.configure_noise_compensation_enabled("", noise_compensation_enabled)
        nr.acp.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        nr.acp.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        nr.initiate("", "")

        # Retrieve results
        (
            lower_relative_power,
            upper_relative_power,
            lower_absolute_power,
            upper_absolute_power,
            error_code,
        ) = nr.acp.results.fetch_offset_measurement_array("", timeout)

        total_aggregated_power, error_code = nr.acp.results.fetch_total_aggregated_power("", timeout)

        for i in range(len(lower_relative_power)):
            relative_powers_trace = numpy.empty(0, dtype=numpy.float32)
            nr.acp.results.fetch_relative_powers_trace("", timeout, i, relative_powers_trace)

        spectrum = numpy.empty(0, dtype=numpy.float32)
        nr.acp.results.fetch_spectrum("", timeout, spectrum)

        # Print Results
        print(f"Total Aggregated Power (dBm or dBm/Hz)    : {total_aggregated_power}")
        print("\n-----------Offset Channel Measurements------------ \n")
        for i in range(len(lower_relative_power)):
            print(f"Offset  : {i}")
            print(f"Lower Relative Power (dB)                 : {lower_relative_power[i]}")
            print(f"Upper Relative Power (dB)                 : {upper_relative_power[i]}")
            print(f"Lower Absolute Power (dBm or dBm/Hz)      : {lower_absolute_power[i]}")
            print(f"Upper Absolute Power (dBm or dBm/Hz)      : {upper_absolute_power[i]}")
            print("---------------------------------------------------\n")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        if nr is not None:
            nr.dispose()
            nr = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    parser = argparse.ArgumentParser(
        description="Pass arguments for ACP Contiguous Multi-Carrier Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr.")
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string)


def main():
    _main(sys.argv[1:])


def test_main():
    cmd_line = ["--option-string", ""]
    _main(cmd_line)


def test_example():
    example("RFSA", {})


if __name__ == "__main__":
    main()
