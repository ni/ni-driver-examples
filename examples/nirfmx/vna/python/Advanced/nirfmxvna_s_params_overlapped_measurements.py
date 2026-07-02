"""
RFmx VNA S-Parameters Overlapped Measurements Example

Steps:
1. Open a new RFmx session.
2. Create two named Signals called 'Signal1' and 'Signal2'. Signals here can be considered
   equivalent to the concept of Channels in third party software.
   For each Signal, configure sweep and measurement settings. Note that, Power Level varies
   for each Signal in this example.
   2A. Create Signal configuration.
   2B. Configure the sweep properties - Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, Test Receiver Attn, Power Level.
   2C. Select S-Parameter Measurement.
   2D. Configure the number of S-Parameters.
   2E. Configure the S-Parameter and Format.
3. For each Signal, initiate the measurement and wait for the acquisition to complete.
   Here note that, between the two signals, only acquisition is sequential and measurements are overlapped.
4. Fetch the measurement results of each Signal.
5. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxvna


def example(resource_name, option_string):
    """VNA S-parameters overlapped measurements example."""
    frequency_start = 1e9  # Hz
    frequency_end = 26e9  # Hz
    number_of_frequency_points = 251
    power_level = -10.0  # dBm (base; Signal2 will be -20 dBm)
    port1_test_receiver_attenuation = 0.0  # dB
    port2_test_receiver_attenuation = 0.0  # dB
    if_bandwidth = 100.0e3  # Hz
    frequency_reference_source = "PXI_Clk"
    frequency_reference_frequency = 100e6  # Hz

    number_of_sparams = 4
    sparams_parameters = ["S11", "S12", "S21", "S22"]
    sparams_formats = [
        nirfmxvna.SParamsFormat.MAGNITUDE,
        nirfmxvna.SParamsFormat.MAGNITUDE,
        nirfmxvna.SParamsFormat.MAGNITUDE,
        nirfmxvna.SParamsFormat.MAGNITUDE,
    ]

    named_signals = ["Signal1", "Signal2"]
    acquisition_timeout = 10.0  # s
    fetch_timeout = 10.0  # s

    instr_session = None
    vna_signal1 = None
    vna_signal2 = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)

        vna_signal1 = instr_session.get_vna_signal_configuration("Signal1")
        vna_signal2 = instr_session.get_vna_signal_configuration("Signal2")

        signals = [vna_signal1, vna_signal2]

        for i, signal in enumerate(signals):
            signal_power_level = power_level - (i * 10)

            signal.set_sweep_type("", nirfmxvna.SweepType.LINEAR)
            signal.set_start_frequency("", frequency_start)
            signal.set_stop_frequency("", frequency_end)
            signal.set_number_of_points("", number_of_frequency_points)
            signal.set_if_bandwidth("", if_bandwidth)

            port_selector_string = nirfmxvna.Vna.build_port_string("", "port1")
            signal.set_power_level(port_selector_string, signal_power_level)
            signal.set_test_receiver_attenuation(port_selector_string, port1_test_receiver_attenuation)

            port_selector_string = nirfmxvna.Vna.build_port_string("", "port2")
            signal.set_power_level(port_selector_string, signal_power_level)
            signal.set_test_receiver_attenuation(port_selector_string, port2_test_receiver_attenuation)

            signal.select_measurements("", nirfmxvna.MeasurementTypes.SPARAMS, False)
            signal.s_params.configuration.set_number_of_s_parameters("", number_of_sparams)

            for k in range(number_of_sparams):
                sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", k)
                signal.s_params.configuration.configure_s_parameter(sparam_selector_string, sparams_parameters[k])
                signal.s_params.configuration.set_format(sparam_selector_string, sparams_formats[k])

        # Sequential acquisition, overlapped measurement processing
        for signal in signals:
            signal.initiate("", "")
            instr_session.wait_for_acquisition_complete(acquisition_timeout)

        # Fetch results for each signal
        for j, signal in enumerate(signals):
            number_of_sparams_result, _ = signal.s_params.configuration.get_number_of_s_parameters("")
            signal.s_params.results.fetch_x_data("", fetch_timeout)

            for i in range(number_of_sparams_result):
                sparam_selector_string = nirfmxvna.Vna.build_s_parameter_string("", i)
                signal.s_params.results.fetch_y_data(sparam_selector_string, fetch_timeout)

            correction_state_result, _ = signal.s_params.results.get_correction_state("")

    except Exception as e:
        print(f"ERROR: {e}")
        sys.exit(1)

    finally:
        if vna_signal1 is not None:
            vna_signal1.dispose()
            vna_signal1 = None
        if vna_signal2 is not None:
            vna_signal2.dispose()
            vna_signal2 = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _parse_args():
    parser = argparse.ArgumentParser(
        description="VNA S-Parameters Overlapped Measurements Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-r", "--resource", type=str, default="VNA", help="Resource name of the VNA device")
    parser.add_argument("-o", "--option-string", type=str, default="", help="Option string")
    return parser.parse_args()


def main():
    args = _parse_args()
    example(args.resource, args.option_string)


if __name__ == "__main__":
    main()
