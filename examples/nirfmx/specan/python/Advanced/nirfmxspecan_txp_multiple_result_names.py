r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source and Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select TXP measurement and enable the traces.
6. Configure TXP Measurement Interval and RBW.
7. Configure TXP Averaging.
8. Producer loop: for each frequency step, configure frequency, initiate with a unique result name,
   enqueue that result name, and wait for acquisition to complete.
9. Consumer loop: dequeue result names and fetch TXP measurements.
10. Close the RFmx Session.
"""

import argparse
import sys
import queue
import threading

import nirfmxspecan

import nirfmxinstr

NUMBER_OF_MEASUREMENTS = 2


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9    # Hz
    reference_level = 0.0       # dBm
    external_attenuation = 0.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference = 10.0e6  # Hz

    measurement_interval = 1.0e-3  # seconds

    rbw_filter_type = nirfmxspecan.TxpRbwFilterType.GAUSSIAN
    rbw = 100.0e3  # Hz
    rrc_alpha = 0.010

    averaging_enabled = nirfmxspecan.TxpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.TxpAveragingType.RMS

    timeout = 10.0  # seconds

    average_mean_power = [None] * NUMBER_OF_MEASUREMENTS
    peak_to_average_ratio = [None] * NUMBER_OF_MEASUREMENTS
    maximum_power = [None] * NUMBER_OF_MEASUREMENTS
    minimum_power = [None] * NUMBER_OF_MEASUREMENTS

    task_queue = queue.Queue()
    error_flag = [False]

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference)
        specan.set_selected_ports("", selected_ports)
        specan.configure_reference_level("", reference_level)
        specan.configure_external_attenuation("", external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.TXP, True)
        specan.txp.configuration.configure_measurement_interval("", measurement_interval)
        specan.txp.configuration.configure_rbw_filter("", rbw, rbw_filter_type, rrc_alpha)
        specan.txp.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)

        def fetch_results_thread():
            try:
                for i in range(NUMBER_OF_MEASUREMENTS):
                    result_string = task_queue.get(timeout=10.0)
                    avg, par, mx, mn, error_code = specan.txp.results.fetch_measurement(result_string, timeout)
                    average_mean_power[i] = avg
                    peak_to_average_ratio[i] = par
                    maximum_power[i] = mx
                    minimum_power[i] = mn
            except Exception as ex:
                error_flag[0] = True
                print(f"ERROR in fetch thread: {ex}")

        fetch_thread = threading.Thread(target=fetch_results_thread)
        fetch_thread.start()

        # Producer loop
        for i in range(NUMBER_OF_MEASUREMENTS):
            offset = i * 1.0e6  # step frequency by 1 MHz per iteration
            specan.configure_frequency("", center_frequency + offset)
            result_string = nirfmxspecan.SpecAn.build_result_string(f"TXP_Result{i}")
            specan.initiate("", result_string)
            task_queue.put(result_string)
            instr_session.wait_for_acquisition_complete(timeout)

        fetch_thread.join()

        if not error_flag[0]:
            for i in range(NUMBER_OF_MEASUREMENTS):
                print(f"--------------------Measurement{i + 1}--------------------\n")
                print(f"Average Mean Frequency (Hz)    {average_mean_power[i]}\n")
                print(f"Peak to Average Ratio (dB)     {peak_to_average_ratio[i]}\n")
                print(f"Maximum Power (dBm)            {maximum_power[i]}\n")
                print(f"Minimum Power (dBm)            {minimum_power[i]}\n")

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
        description="Pass arguments for TXP Multiple Result Names Example",
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
