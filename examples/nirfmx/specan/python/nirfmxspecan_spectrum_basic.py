r"""Steps:
1. Open a new RFmx session.
2. Configure Selected Ports.
3. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Spectrum Span.
5. Configure Spectrum RBW Filter.
6. Configure Spectrum Averaging.
7. Read Spectrum Measurement Results.
8. Close the RFmx Session.
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
    timeout = 10.0  # seconds
    span = 1.0e6  # Hz

    rbw_auto = nirfmxspecan.SpectrumRbwAutoBandwidth.TRUE
    rbw = 100.0e3  # Hz
    rbw_filter_type = nirfmxspecan.SpectrumRbwFilterType.GAUSSIAN

    averaging_enabled = nirfmxspecan.SpectrumAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.SpectrumAveragingType.RMS

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.spectrum.configuration.configure_span("", span)
        specan.spectrum.configuration.configure_rbw_filter("", rbw_auto, rbw, rbw_filter_type)
        specan.spectrum.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)

        spectrum = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = specan.spectrum.results.read("", timeout, spectrum)

        print(f"Start Frequency (Hz)       {x0}")
        print(f"Frequency Increment (Hz)   {dx}")
        print(f"Sample Count               {len(spectrum)}")

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
        description="Pass arguments for Spectrum Basic Example",
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
