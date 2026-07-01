r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select OBW measurement and enable the traces.
6. Configure OBW Bandwidth Percentage, Span and Sweep Time.
7. Configure OBW Averaging.
8. Configure OBW RBW Filter.
9. Configure OBW FFT.
10. Configure OBW Power Units.
11. Initiate Measurement.
12. Fetch OBW Measurement and Traces.
13. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    # Initialize input variables
    selected_ports = ""
    center_frequency = 1e9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.00  # dB
    timeout = 10.0  # seconds

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    span = 1.0e6  # Hz
    bandwidth_percentage = 99.00
    power_units = nirfmxspecan.ObwPowerUnits.DBM

    rbw_filter_type = nirfmxspecan.ObwRbwFilterType.GAUSSIAN
    rbw_auto = nirfmxspecan.ObwRbwAutoBandwidth.TRUE
    rbw = 10.0e3  # Hz

    sweep_time_auto = nirfmxspecan.ObwSweepTimeAuto.TRUE
    sweep_time_interval = 1.0e-3  # seconds

    averaging_enabled = nirfmxspecan.ObwAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.ObwAveragingType.RMS

    fft_window = nirfmxspecan.ObwFftWindow.FLAT_TOP
    fft_padding = -1.0

    instr_session = None
    specan = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get SpecAn signal
        specan = instr_session.get_specan_signal_configuration()

        # Configure measurement
        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_frequency("", center_frequency)
        specan.configure_reference_level("", reference_level)
        specan.configure_external_attenuation("", external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.OBW, True)
        specan.obw.configuration.configure_bandwidth_percentage("", bandwidth_percentage)
        specan.obw.configuration.configure_span("", span)
        specan.obw.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        specan.obw.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.obw.configuration.configure_rbw_filter("", rbw_auto, rbw, rbw_filter_type)
        specan.obw.configuration.configure_fft("", fft_window, fft_padding)
        specan.obw.configuration.configure_power_units("", power_units)
        specan.initiate("", "")

        # Retrieve results
        spectrum = numpy.empty(0, dtype=numpy.float32)
        specan.obw.results.fetch_spectrum_trace("", timeout, spectrum)
        occupied_bandwidth, average_power, frequency_resolution, start_frequency, stop_frequency, error_code = (
            specan.obw.results.fetch_measurement("", timeout)
        )

        # Print results
        print(f"Occupied Bandwidth (Hz)               {occupied_bandwidth}")
        print(f"Average Power (dBm or dBm/Hz)         {average_power}")
        print(f"Frequency Resolution (Hz)             {frequency_resolution}")
        print(f"Start Frequency (Hz)                  {start_frequency}")
        print(f"Stop Frequency (Hz)                   {stop_frequency}")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close Session
        if specan is not None:
            specan.dispose()
            specan = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for OBW Example",
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
    example("RFSA", "")


if __name__ == "__main__":
    main()
