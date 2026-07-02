r"""Steps:
1. Open a new RFmx session.
2. Configure the basic instrument properties (Clock Source, Clock Frequency).
3. Configure Selected Ports.
4. Configure the basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Select CHP measurement and enable the traces.
6. Configure CHP Integration BW, Span and Sweep Time.
7. Configure CHP Averaging.
8. Configure CHP RBW Filter.
9. Configure CHP FFT.
10. Configure CHP Number of Carriers.
11. For each carrier: Configure Carrier Offset, Integration BW and RRC Filter using Selector String.
12. Initiate Measurement.
13. Fetch CHP Measurements (Total Carrier Power, per-carrier Power, PSD, Spectrum).
14. Close the RFmx Session.
"""

import argparse
import sys

import nirfmxspecan
import numpy

import nirfmxinstr

NUMBER_OF_CARRIERS = 1


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9    # Hz
    reference_level = 0.0       # dBm
    external_attenuation = 0.0  # dB

    frequency_source = "OnboardClock"
    frequency = 10.0e6  # Hz

    span = 1.0e6  # Hz

    sweep_time_auto = nirfmxspecan.ChpSweepTimeAuto.TRUE
    sweep_time_interval = 1.0e-3  # seconds

    averaging_enabled = nirfmxspecan.ChpAveragingEnabled.FALSE
    averaging_count = 10
    averaging_type = nirfmxspecan.ChpAveragingType.RMS

    rbw_filter_type = nirfmxspecan.ChpRbwFilterType.GAUSSIAN
    rbw_auto = nirfmxspecan.ChpRbwAutoBandwidth.TRUE
    rbw = 10.0e3  # Hz

    fft_window = nirfmxspecan.ChpFftWindow.FLAT_TOP
    fft_padding = -1.0

    enable_all_traces = True

    # Carrier channel settings
    carrier_frequencies = [0.0]          # Hz offset from carrier
    carrier_integration_bws = [1.0e6]    # Hz
    carrier_rrc_enabled = [nirfmxspecan.ChpCarrierRrcFilterEnabled.FALSE]
    carrier_rrc_alpha = [0.22]

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_source, frequency)
        specan.set_selected_ports("", selected_ports)
        specan.configure_frequency("", center_frequency)
        specan.configure_reference_level("", reference_level)
        specan.configure_external_attenuation("", external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.CHP, enable_all_traces)
        specan.chp.configuration.configure_span("", span)
        specan.chp.configuration.configure_sweep_time("", sweep_time_auto, sweep_time_interval)
        specan.chp.configuration.configure_averaging("", averaging_enabled, averaging_count, averaging_type)
        specan.chp.configuration.configure_rbw_filter("", rbw_auto, rbw, rbw_filter_type)
        specan.chp.configuration.configure_fft("", fft_window, fft_padding)
        specan.chp.configuration.configure_number_of_carriers("", NUMBER_OF_CARRIERS)

        for i in range(NUMBER_OF_CARRIERS):
            carrier_str = nirfmxspecan.SpecAn.build_carrier_string("", i)
            specan.chp.configuration.configure_carrier_offset(carrier_str, carrier_frequencies[i])
            specan.chp.configuration.configure_integration_bandwidth(carrier_str, carrier_integration_bws[i])
            specan.chp.configuration.configure_rrc_filter(carrier_str, carrier_rrc_enabled[i], carrier_rrc_alpha[i])

        specan.initiate("", "")

        spectrum = numpy.empty(0, dtype=numpy.float32)
        specan.chp.results.fetch_spectrum("", timeout, spectrum)

        total_carrier_power, error_code = specan.chp.results.fetch_total_carrier_power("", timeout)
        print(f"Total Carrier Power (dBm) {total_carrier_power}")
        print("\nCarrier Measurements")

        for i in range(NUMBER_OF_CARRIERS):
            carrier_str = nirfmxspecan.SpecAn.build_carrier_string("", i)
            abs_power, psd, relative_power, error_code = (
                specan.chp.results.fetch_carrier_measurement(carrier_str, timeout)
            )
            print(f"\nCarrier : {i}")
            print(f"Absolute power (dBm)     {abs_power}")
            print(f"PSD (dBm/Hz)             {psd}")
            print(f"Relative Power (dB)      {relative_power}")
            print("-----------------------------------------------------------")

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
        description="Pass arguments for CHP Advanced Example",
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
