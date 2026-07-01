r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure IQ Power Edge Trigger properties.
6. Configure IQ measurement.
7. Configure Acquisition parameters (Sample Rate, Records, Acquisition Time).
8. Initiate Measurement.
9. Fetch IQ Data.
10. Calculate Mean Power: Power (dBm) = 10 * log10(((I^2 + Q^2) / (2*R)) / 1mW), R=50 Ohms.
11. Close the RFmx Session.
"""

import argparse
import sys
import math

import nirfmxspecan
import numpy

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1e9   # Hz
    reference_level = 0.00   # dBm
    external_attenuation = 0.00  # dB

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    iq_power_edge_enabled = False
    iq_power_edge_level = -20.0  # dBm
    trigger_delay = 0.0  # seconds
    minimum_quiet_time = 0.0  # seconds

    sample_rate = 10.0e6  # samples/s
    acquisition_time = 1.0e-3  # seconds

    record_to_fetch = 0
    samples_to_read = -1
    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.configure_iq_power_edge_trigger(
            "", "0", iq_power_edge_level,
            nirfmxspecan.IQPowerEdgeTriggerSlope.RISING_SLOPE,
            trigger_delay,
            nirfmxspecan.TriggerMinimumQuietTimeMode.MANUAL,
            minimum_quiet_time,
            iq_power_edge_enabled,
        )
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.IQ, False)
        specan.iq.configuration.configure_acquisition("", sample_rate, 1, acquisition_time, 0)
        specan.initiate("", "")

        # Fetch IQ data
        data = numpy.array([], dtype=numpy.complex64)
        t0, dt, error_code = specan.iq.results.fetch_data(
            "", timeout, record_to_fetch, samples_to_read, data
        )

        # Calculate mean power
        real_array = numpy.real(data)
        imag_array = numpy.imag(data)
        mean_power = numpy.mean(real_array**2 + imag_array**2)
        mean_power_dbm = 10.0 * math.log10(mean_power / (2 * 50) / 0.001)

        print(f"Mean Power (dBm)   {mean_power_dbm}")

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
        description="Pass arguments for IQ Example",
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
