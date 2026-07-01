r"""Steps:
1. Read reference waveform from TDMS file.
2. Open a new RFmx session.
3. Configure frequency reference.
4. Configure Selected Ports.
5. Configure center frequency, reference level and external attenuation.
6. Select AMPM measurement.
7. Configure DUT Average Input Power.
8. Configure Reference Waveform.
9. Configure Measurement Sample Rate and Interval.
10. Configure Threshold.
11. Configure Reference Power Type.
12. Initiate Measurement.
13. Fetch AM/PM Results.
14. Close the RFmx Session.

Prerequisites:
- nptdms package (`pip install nptdms`)
"""

import argparse
import os
import sys

import numpy
from nptdms import TdmsFile

import nirfmxspecan
import nirfmxinstr

_DEFAULT_WAVEFORM_FILE = os.path.join(
    os.path.dirname(os.path.abspath(__file__)),
    "Support",
    "LTE20MHz Waveform (Two Subframes).tdms",
)


def example(resource_name, option_string, reference_waveform_file):
    """Run Example."""
    selected_ports = ""
    center_frequency = 1.0e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference = 10.0e6  # Hz

    dut_average_input_power = -20.0  # dBm
    idle_duration_present = nirfmxspecan.AmpmReferenceWaveformIdleDurationPresent.FALSE
    signal_type = nirfmxspecan.AmpmSignalType.MODULATED

    sample_rate_mode = nirfmxspecan.AmpmMeasurementSampleRateMode.REFERENCE_WAVEFORM
    sample_rate = 120.0e6  # samples/s
    measurement_interval = 100.0e-6  # seconds

    threshold_enabled = nirfmxspecan.AmpmThresholdEnabled.TRUE
    threshold_level = -20.0  # dB
    threshold_type = nirfmxspecan.AmpmThresholdType.RELATIVE

    reference_power_type = nirfmxspecan.AmpmReferencePowerType.INPUT

    timeout = 10.0  # seconds

    instr_session = None
    specan = None

    try:
        with TdmsFile.open(reference_waveform_file) as tdms_file:
            group = tdms_file.groups()[0]
            channel = group.channels()[0]
            raw = channel[:].astype(numpy.float32)
            i_data = raw[0::2]
            q_data = raw[1::2]
            reference_waveform_iq = (i_data + 1j * q_data).astype(numpy.complex64)
            ref_x0 = float(channel.properties.get("wf_start_offset", 0.0))
            ref_dx = float(channel.properties.get("wf_increment", 1.0 / 30.72e6))

        instr_session = nirfmxinstr.Session(resource_name, option_string)
        specan = instr_session.get_specan_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference)
        specan.set_selected_ports("", selected_ports)
        specan.configure_rf("", center_frequency, reference_level, external_attenuation)
        specan.select_measurements("", nirfmxspecan.MeasurementTypes.AMPM, True)
        specan.ampm.configuration.configure_dut_average_input_power("", dut_average_input_power)
        specan.ampm.configuration.configure_reference_waveform(
            "",
            ref_x0,
            ref_dx,
            reference_waveform_iq,
            idle_duration_present,
            signal_type,
        )
        specan.ampm.configuration.configure_measurement_sample_rate("", sample_rate_mode, sample_rate)
        specan.ampm.configuration.configure_measurement_interval("", measurement_interval)
        specan.ampm.configuration.configure_threshold("", threshold_enabled, threshold_level, threshold_type)
        specan.ampm.configuration.configure_reference_power_type("", reference_power_type)
        specan.initiate("", "")

        mean_linear_gain, one_db_compression_point, mean_rms_evm, error_code = (
            specan.ampm.results.fetch_dut_characteristics("", timeout)
        )
        gain_error_range, phase_error_range, mean_phase_error, error_code = (
            specan.ampm.results.fetch_error("", timeout)
        )
        am_to_am_residual, am_to_pm_residual, error_code = (
            specan.ampm.results.fetch_curve_fit_residual("", timeout)
        )

        specan.ampm.results.fetch_am_to_am_trace("", timeout)
        specan.ampm.results.fetch_am_to_pm_trace("", timeout)

        print("-----------------Measurement-----------------\n")
        print(f"Mean Linear Gain (dB)            {mean_linear_gain}")
        print(f"Mean Phase Error (deg)           {mean_phase_error}")
        print(f"Mean RMS EVM (%)                 {mean_rms_evm}")
        print(f"AM to AM Residual (dB)           {am_to_am_residual}")
        print(f"AM to PM Residual (deg)          {am_to_pm_residual}")
        print(f"Gain Error Range (dB)            {gain_error_range}")
        print(f"Phase Error Range (deg)          {phase_error_range}")
        print(f"1 dB Compression Point (dBm)     {one_db_compression_point}")

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
        description="Pass arguments for AMPM Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr.")
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    parser.add_argument(
        "-w",
        "--reference-waveform-file",
        default=_DEFAULT_WAVEFORM_FILE,
        help="Path to the reference waveform TDMS file.",
    )
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string, args.reference_waveform_file)


def main():
    _main(sys.argv[1:])


def test_main():
    _main(["--option-string", ""])


def test_example():
    example("RFSA", {}, _DEFAULT_WAVEFORM_FILE)


if __name__ == "__main__":
    main()
