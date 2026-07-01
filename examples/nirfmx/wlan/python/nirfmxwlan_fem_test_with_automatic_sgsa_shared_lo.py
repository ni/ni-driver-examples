"""
RFmx WLAN FEM Test with Automatic SG/SA Shared LO Example

Steps (RFSG):
1.  Open NI-RFSG session.
2.  Configure RFSG frequency reference and set waveform generation mode to Script.
3.  Configure RF output frequency and power level.
4.  Set RFSG External Gain, Power Level Type, and Pre-filter Gain.
5.  Export the Marker Event marker0 to PFI0 (used as the source for the
    digital edge trigger on the RFSA side).
6.  Configure RFSG LO Source to Automatic SG/SA Shared.
7.  Set Upconverter Frequency Offset Mode to Auto.
8.  Read waveform from file and download it to RFSG.
9.  Retrieve waveform PAPR, Signal Bandwidth, and IQ Rate from waveform metadata.
    Configure RFSG Signal Bandwidth, IQ Rate, and Peak Power Adjustment (PAPR).
    With the signal bandwidth configured and the Upconverter Frequency Offset Mode
    set to Auto, the RFSG LO is placed outside the signal if the signal bandwidth is
    less than half of the device instantaneous bandwidth; otherwise the LO is placed
    at the center of the signal.
10. Write script to generate the waveform continuously with marker0 at sample 0.
11. Initiate signal generation.

Steps (OFDMModAcc measurement):
12. Open a new RFmx session.
13. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
14. Configure the basic signal properties (Center Frequency, Reference Level,
    External Attenuation).
15. Configure Digital Edge Trigger properties (Digital Edge Source, Digital Edge,
    Trigger Delay). The source is PFI0, driven by the RFSG marker.
16. Configure Standard and Channel Bandwidth properties.
17. Set LO Source to Automatic_SG_SA_Shared.
18. Set LO Leakage Avoidance Enabled to True.
19. Select OFDMModAcc measurement and disable traces.
20. Configure OFDMModAcc Averaging properties.
21. Initiate OFDMModAcc measurement.
22. Fetch OFDMModAcc measurements.

Steps (SEM measurement):
23. Abort and re-initiate signal generation (LO offset mode may change).
24. Select SEM measurement and disable traces.
25. Configure SEM Averaging properties.
26. Initiate SEM measurement.
27. Fetch SEM measurements.

Steps (cleanup):
28. Close the RFmx Session.
29. Clear the waveform from RFSG memory and close the RFSG session.
"""

import argparse
import os
import sys

import nirfmxinstr
import nirfmxwlan
import nirfsg
import numpy

_SUPPORT_DIR = os.path.join(os.path.dirname(os.path.abspath(__file__)), "support")


def example(rfsg_resource_name, rfsa_resource_name, option_string, waveform_file_path=None):
    """WLAN FEM Test with Automatic SG/SA Shared LO measurement example."""

    if waveform_file_path is None:
        waveform_file_path = os.path.join(_SUPPORT_DIR, "WLAN_80211ac_BW-80MHz_SISO.tdms")

    center_frequency = 2.412e9  # Hz

    rfsg_external_attenuation = 0.0  # dB
    power_level = -10.0  # dBm

    rfsa_reference_level = 0.0  # dBm
    rfsa_external_attenuation = 0.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    digital_trigger_enabled = True
    digital_edge_source = "PFI0"
    trigger_delay = 0.0  # s

    standard = nirfmxwlan.Standard.STANDARD_802_11_AC
    channel_bandwidth = 80e6  # Hz

    waveform_name = "Wfm"
    script = (
        f"script GenerateWaveform\n"
        f"  repeat forever\n"
        f"    generate {waveform_name} marker0(0)\n"
        f"   end repeat\n"
        f"  end script"
    )
    marker_number = 0

    ofdmmodacc_averaging_enabled = nirfmxwlan.OfdmModAccAveragingEnabled.FALSE
    ofdmmodacc_averaging_count = 10
    ofdmmodacc_averaging_type = nirfmxwlan.OfdmModAccAveragingType.RMS
    vector_averaging_time_alignment_enabled = (
        nirfmxwlan.OfdmModAccVectorAveragingTimeAlignmentEnabled.TRUE
    )
    vector_averaging_phase_alignment_enabled = (
        nirfmxwlan.OfdmModAccVectorAveragingPhaseAlignmentEnabled.TRUE
    )

    sem_averaging_enabled = nirfmxwlan.SemAveragingEnabled.FALSE
    sem_averaging_count = 10
    sem_averaging_type = nirfmxwlan.SemAveragingType.RMS

    timeout = 10.0  # s

    rfsg_session = None
    instr_session = None
    wlan_signal = None

    try:
        # --- Configure and start RFSG ---
        rfsg_session = nirfsg.Session(rfsg_resource_name)
        rfsg_session.frequency_reference.configure_frequency_reference(
            nirfsg.FrequencyReferenceSource.ONBOARD_CLOCK, frequency_reference_frequency
        )
        rfsg_session.generation_mode = nirfsg.GenerationMode.SCRIPT
        rfsg_session.rf.configure_rf(center_frequency, power_level)
        rfsg_session.power_level_type = nirfsg.PowerLevelType.PEAK
        rfsg_session.arb_pre_filter_gain = -1.5
        rfsg_session.external_gain = -1.0 * rfsg_external_attenuation
        rfsg_session.markers[marker_number].exported_marker_event_output_terminal = "PFI0"
        rfsg_session.lo_source = nirfsg.LoSource.AUTOMATIC_SG_SA_SHARED
        rfsg_session.upconverter_frequency_offset_mode = nirfsg.UpconverterFrequencyOffsetMode.AUTO
        rfsg_session.read_and_download_waveform_from_file_tdms(
            waveform_name, waveform_file_path, 0
        )
        waveform_papr = rfsg_session.waveforms[waveform_name].waveform_papr
        waveform_signal_bandwidth = rfsg_session.waveforms[waveform_name].waveform_signal_bandwidth
        waveform_iq_rate = rfsg_session.waveforms[waveform_name].waveform_iq_rate
        rfsg_session.signal_bandwidth = waveform_signal_bandwidth
        rfsg_session.iq_rate = waveform_iq_rate
        rfsg_session.peak_power_adjustment = waveform_papr
        rfsg_session.write_script(script)
        rfsg_session.initiate()

        # --- Configure RFmx ---
        instr_session = nirfmxinstr.Session(rfsa_resource_name, option_string)
        wlan_signal = instr_session.get_wlan_signal_configuration()

        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )

        wlan_signal.configure_frequency("", center_frequency)
        wlan_signal.configure_reference_level("", rfsa_reference_level)
        wlan_signal.configure_external_attenuation("", rfsa_external_attenuation)

        wlan_signal.configure_digital_edge_trigger(
            "",
            digital_edge_source,
            nirfmxwlan.DigitalEdgeTriggerEdge.RISING_EDGE,
            trigger_delay,
            digital_trigger_enabled,
        )

        wlan_signal.configure_standard("", standard)
        wlan_signal.configure_channel_bandwidth("", channel_bandwidth)

        instr_session.set_lo_source("", "Automatic_SG_SA_Shared")
        instr_session.set_lo_leakage_avoidance_enabled(
            "", nirfmxinstr.LOLeakageAvoidanceEnabled.TRUE
        )

        # --- OFDMModAcc measurement ---
        wlan_signal.select_measurements("", nirfmxwlan.MeasurementTypes.OFDMMODACC, False)
        wlan_signal.ofdmmodacc.configuration.configure_averaging(
            "", ofdmmodacc_averaging_enabled, ofdmmodacc_averaging_count
        )
        wlan_signal.ofdmmodacc.configuration.set_averaging_type("", ofdmmodacc_averaging_type)
        wlan_signal.ofdmmodacc.configuration.set_vector_averaging_time_alignment_enabled(
            "", vector_averaging_time_alignment_enabled
        )
        wlan_signal.ofdmmodacc.configuration.set_vector_averaging_phase_alignment_enabled(
            "", vector_averaging_phase_alignment_enabled
        )

        wlan_signal.initiate("", "")

        (
            composite_rms_evm_mean,
            composite_data_rms_evm_mean,
            composite_pilot_rms_evm_mean,
            error_code,
        ) = wlan_signal.ofdmmodacc.results.fetch_composite_rms_evm("", timeout)

        # Re-start RFSG for SEM measurement (LO offset mode may change)
        rfsg_session.abort()
        rfsg_session.initiate()

        # --- SEM measurement ---
        wlan_signal.select_measurements("", nirfmxwlan.MeasurementTypes.SEM, False)
        wlan_signal.sem.configuration.configure_averaging(
            "", sem_averaging_enabled, sem_averaging_count, sem_averaging_type
        )

        wlan_signal.initiate("", "")

        measurement_status, error_code = wlan_signal.sem.results.fetch_measurement_status(
            "", timeout
        )
        absolute_power, relative_power, error_code = (
            wlan_signal.sem.results.fetch_carrier_measurement("", timeout)
        )

        (
            lower_offset_measurement_status,
            lower_offset_margin,
            lower_offset_margin_frequency,
            lower_offset_margin_absolute_power,
            lower_offset_margin_relative_power,
            error_code,
        ) = wlan_signal.sem.results.fetch_lower_offset_margin_array("", timeout)

        (
            upper_offset_measurement_status,
            upper_offset_margin,
            upper_offset_margin_frequency,
            upper_offset_margin_absolute_power,
            upper_offset_margin_relative_power,
            error_code,
        ) = wlan_signal.sem.results.fetch_upper_offset_margin_array("", timeout)

        # Print OFDMModAcc results
        print("------------------OFDMModAcc------------------\n")
        print("------------------Composite EVM------------------")
        print(f"RMS EVM Mean (dB)                  : {composite_rms_evm_mean}")
        print(f"Data RMS EVM Mean (dB)             : {composite_data_rms_evm_mean}")
        print(f"Pilot RMS EVM Mean (dB)            : {composite_pilot_rms_evm_mean}\n")

        # Print SEM results
        print("\n------------------SEM------------------\n")
        print(f"Measurement Status                 :{measurement_status}")
        print(f"Carrier Absolute Power (dBm)       :{absolute_power}")

        print("\n----------Lower Offset Measurements----------\n")
        for i in range(len(lower_offset_margin)):
            print(f"Offset {i}")
            print(f"Measurement Status                 :{lower_offset_measurement_status[i]}")
            print(f"Margin (dB)                        :{lower_offset_margin[i]}")
            print(f"Margin Frequency (Hz)              :{lower_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power (dBm)        :{lower_offset_margin_absolute_power[i]}\n")

        print("\n----------Upper Offset Measurements----------\n")
        for i in range(len(upper_offset_margin)):
            print(f"Offset {i}")
            print(f"Measurement Status                 :{upper_offset_measurement_status[i]}")
            print(f"Margin (dB)                        :{upper_offset_margin[i]}")
            print(f"Margin Frequency (Hz)              :{upper_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power (dBm)        :{upper_offset_margin_absolute_power[i]}\n")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close RFmx Session
        if wlan_signal is not None:
            wlan_signal.dispose()
            wlan_signal = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None

        # Stop, clear waveform, and close RFSG session
        if rfsg_session is not None:
            rfsg_session.abort()
            rfsg_session.clear_arb_waveform(waveform_name)
            rfsg_session.close()
            rfsg_session = None


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for WLAN FEM Test with Automatic SG/SA Shared LO Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-g", "--rfsg-resource-name", default="RFSG", help="Resource name of RFSG"
    )
    parser.add_argument(
        "-n", "--rfsa-resource-name", default="RFSA", help="Resource name of RFSA"
    )
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    parser.add_argument(
        "-f",
        "--waveform-file-path",
        default=None,
        help="Path to the WLAN TDMS waveform file (default: support/WLAN_80211ac_BW-80MHz_SISO.tdms)",
    )
    args = parser.parse_args(argsv)
    example(args.rfsg_resource_name, args.rfsa_resource_name, args.option_string, args.waveform_file_path)


def main():
    """Call _main function."""
    _main(sys.argv[1:])


def test_main():
    """Call _main function with empty option string."""
    cmd_line = [
        "--rfsg-resource-name",
        "RFSG",
        "--rfsa-resource-name",
        "RFSA",
        "--option-string",
        "",
    ]
    _main(cmd_line)


def test_example():
    """Call example function."""
    example("RFSG", "RFSA", "")


if __name__ == "__main__":
    main()
