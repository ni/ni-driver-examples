r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure Selected Ports.
4. Configure basic signal properties (Center Frequency, RF Attenuation and External Attenuation).
5. Configure Trigger Parameters for Digital Edge Trigger.
6. Configure Link Direction and Number of Subblocks.
7. Configure Frequency Range, Center Frequency, Subblock Frequency Definition, Component Carrier Spacing Type,
   Channel Raster, Component Carrier Center Frequency and Number of Component Carriers.
8. Configure Subcarrier Spacing.
9. Configure Component Carriers.
10. Configure Reference Level.
11. Select ACP measurement and enable Traces.
12. Configure Measurement Method.
13. Configure Noise Compensation Parameter.
14. Configure Sweep Time Parameters.
15. Configure Averaging Parameters for ACP measurement.
16. Initiate the Measurement.
17. Fetch ACP Measurements and Traces.
18. Close RFmx Session.
"""

import argparse
import sys

import nirfmxnr
import numpy

import nirfmxinstr

NUMBER_OF_SUBBLOCKS = 2
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

    subblock_frequency = [0.0, 200e6]  # Hz
    component_carrier_spacing_type = [
        nirfmxnr.ComponentCarrierSpacingType.NOMINAL,
        nirfmxnr.ComponentCarrierSpacingType.NOMINAL,
    ]
    channel_raster = [15e3, 15e3]  # Hz
    component_carrier_at_center_frequency = [-1, -1]

    component_carrier_bandwidth = [
        [100e6, 100e6],
        [100e6, 100e6],
    ]  # Hz
    component_carrier_frequency = [
        [-49.98e6, 50.01e6],
        [-49.98e6, 50.01e6],
    ]  # Hz

    subcarrier_spacing = 30e3  # Hz
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
        nr.set_number_of_subblocks("", NUMBER_OF_SUBBLOCKS)

        for i in range(NUMBER_OF_SUBBLOCKS):
            subblock_string = nirfmxnr.NR.build_subblock_string("", i)
            nr.set_frequency_range(subblock_string, frequency_range)
            nr.set_subblock_frequency(subblock_string, subblock_frequency[i])
            nr.set_channel_raster(subblock_string, channel_raster[i])
            nr.set_component_carrier_spacing_type(subblock_string, component_carrier_spacing_type[i])
            nr.set_component_carrier_at_center_frequency(subblock_string, component_carrier_at_center_frequency[i])
            nr.component_carrier.set_number_of_component_carriers(subblock_string, NUMBER_OF_COMPONENT_CARRIERS)

            carrier_string = nirfmxnr.NR.build_carrier_string(subblock_string, -1)
            nr.component_carrier.set_bandwidth_part_subcarrier_spacing(carrier_string, subcarrier_spacing)

            for j in range(NUMBER_OF_COMPONENT_CARRIERS):
                carrier_string = nirfmxnr.NR.build_carrier_string(subblock_string, j)
                nr.component_carrier.set_bandwidth(carrier_string, component_carrier_bandwidth[i][j])
                nr.component_carrier.set_frequency(carrier_string, component_carrier_frequency[i][j])

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

        subblock_power = []
        integration_bandwidth = []
        subblock_freq = []
        lower_relative_power_list = []
        upper_relative_power_list = []
        lower_absolute_power_list = []
        upper_absolute_power_list = []

        for i in range(NUMBER_OF_SUBBLOCKS):
            subblock_string = nirfmxnr.NR.build_subblock_string("", i)
            sb_pwr, integ_bw, freq, error_code = nr.acp.results.fetch_subblock_measurement(subblock_string, timeout)
            subblock_power.append(sb_pwr)
            integration_bandwidth.append(integ_bw)
            subblock_freq.append(freq)

            (
                lower_rel,
                upper_rel,
                lower_abs,
                upper_abs,
                error_code,
            ) = nr.acp.results.fetch_offset_measurement_array(subblock_string, timeout)
            lower_relative_power_list.append(lower_rel)
            upper_relative_power_list.append(upper_rel)
            lower_absolute_power_list.append(lower_abs)
            upper_absolute_power_list.append(upper_abs)

        total_aggregated_power, error_code = nr.acp.results.fetch_total_aggregated_power("", timeout)

        for i in range(NUMBER_OF_SUBBLOCKS):
            relative_powers_trace = numpy.empty(0, dtype=numpy.float32)
            nr.acp.results.fetch_relative_powers_trace("", timeout, i, relative_powers_trace)

        spectrum = numpy.empty(0, dtype=numpy.float32)
        nr.acp.results.fetch_spectrum("", timeout, spectrum)

        # Print Results
        print(f"Total Aggregated Power (dBm or dBm/Hz)    : {total_aggregated_power}")
        print("\n-----------Subblock Measurements------------ \n")
        for i in range(NUMBER_OF_SUBBLOCKS):
            print(f"Subblock  : {i}")
            print(f"Subblock Power (dBm or dBm/Hz)     : {subblock_power[i]}")
            print(f"Integration Bandwidth (Hz)         : {integration_bandwidth[i]}")
            print(f"Frequency (Hz)                     : {subblock_freq[i]}")
            print("\n-----------Offset Channel Measurements------------ \n")
            for j in range(len(lower_relative_power_list[i])):
                print(f"Offset  : {j}")
                print(f"Lower Relative Power (dB)       : {lower_relative_power_list[i][j]}")
                print(f"Upper Relative Power (dB)       : {upper_relative_power_list[i][j]}")
                print(f"Lower Absolute Power (dBm)      : {lower_absolute_power_list[i][j]}")
                print(f"Upper Absolute Power (dBm)      : {upper_absolute_power_list[i][j]}")
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
        description="Pass arguments for ACP Non-Contiguous Multi-Carrier Example",
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
