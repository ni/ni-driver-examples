r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select Spectrum measurement and enable the traces.
6. Configure Spectrum RBW Filter.
7. Configure Spectrum Span to Zero (Zero Span mode).
8. Configure Spectrum Sweep Time Interval.
9. Configure Spectrum Averaging.
10. Initiate Measurement.
11. Fetch Spectrum Power Trace.
12. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1e9   # Hz
    reference_level = 0.00   # dBm
    external_attenuation = 0.00  # dB
    frequency = 10.0e6  # Hz
    timeout = 10.0  # seconds
    frequency_source = "OnboardClock"

    rbw_filter_type = nirfmxspecan.SpectrumRbwFilterType.GAUSSIAN
    rbw = 10.0e3  # Hz

    averaging_enabled = nirfmxspecan.SpectrumAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.SpectrumAveragingType.RMS

    sweep_time_interval = 1.0e-3  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.SPECTRUM, True)
        specan.spectrum.configuration.configure_rbw_filter(
            "", nirfmxspecan.SpectrumRbwAutoBandwidth.FALSE, rbw, rbw_filter_type
        )
        specan.spectrum.configuration.configure_span("", 0.0)  # Zero Span
        specan.spectrum.configuration.configure_sweep_time(
            "", nirfmxspecan.SpectrumSweepTimeAuto.FALSE, sweep_time_interval
        )
        specan.spectrum.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.initiate("", "")

        power_trace = numpy.empty(0, dtype=numpy.float32)
        specan.spectrum.results.fetch_power_trace("", timeout, power_trace)

        print("Measurement Complete.")

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
        description="Pass arguments for Zero Span Example",
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
