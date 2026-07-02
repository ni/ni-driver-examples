r"""Steps:
1. Open a new RFmx session.
2. Configure the instrument properties Clock Source and Clock Frequency.
3. Configure Selected Ports.
4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation).
5. Select CHP measurement and enable the traces.
6. Configure CHP Integration BW, Span and Sweep Time.
7. Configure CHP Averaging.
8. Configure CHP RBW filter.
9. Configure CHP FFT.
10. Configure CHP RRC Filter.
11. Initiate Measurement.
12. Fetch CHP Measurements and Traces.
13. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    # Initialize the input variables
    selected_ports = ""
    center_frequency = 1e9  # Hz
    reference_level = 0.00  # dBm
    external_attenuation = 0.00  # dB
    timeout = 10.0  # seconds

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    integration_bandwidth = 1.0e6  # Hz
    span = 1.0e6

    rbw_filter_type = nirfmxspecan.ChpRbwFilterType.GAUSSIAN
    rbw_auto = nirfmxspecan.ChpRbwAutoBandwidth.TRUE
    rbw = 10.0e3

    sweep_time_auto = nirfmxspecan.ChpSweepTimeAuto.TRUE
    sweep_time_interval = 1.00e-3  # seconds

    averaging_enabled = nirfmxspecan.ChpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.ChpAveragingType.RMS

    rrc_filter_enabled = nirfmxspecan.ChpCarrierRrcFilterEnabled.FALSE
    rrc_alpha = 0.220

    fft_window = nirfmxspecan.ChpFftWindow.FLAT_TOP
    fft_padding = -1.0

    enable_all_traces = True

    instr_session = None
    specan = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Configure SpecAn
        specan = instr_session.get_specan_signal_configuration()

        # Configure measurement
        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_frequency("", center_frequency)
        specan.configure_reference_level("", reference_level)
        specan.configure_external_attenuation("", external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.CHP, enable_all_traces)
        specan.chp.configuration.configure_integration_bandwidth("", integration_bandwidth)
        specan.chp.configuration.configure_span("", span)
        specan.chp.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        specan.chp.configuration.configure_averaging(
            "", averaging_enabled, averaging_count, averaging_type
        )
        specan.chp.configuration.configure_rbw_filter("", rbw_auto, rbw, rbw_filter_type)
        specan.chp.configuration.configure_fft("", fft_window, fft_padding)
        specan.chp.configuration.configure_rrc_filter("", rrc_filter_enabled, rrc_alpha)
        specan.initiate("", "")

        # Retrive results
        spectrum = numpy.empty(0, dtype=numpy.float32)
        specan.chp.results.fetch_spectrum("", timeout, spectrum)
        absolute_power, psd, relative_power, error_code = (
            specan.chp.results.fetch_carrier_measurement("", timeout)
        )

        # Print results
        print(f"Average Channel Power (dBm)  {absolute_power}")
        print(f"Average Channel PSD (dBm/Hz) {psd}")

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
        description="Pass arguments for CHP Example",
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
