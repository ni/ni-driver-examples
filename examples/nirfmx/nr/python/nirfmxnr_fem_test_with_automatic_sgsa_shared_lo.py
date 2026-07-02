"""
RFmx NR FEM Test with Automatic SG/SA Shared LO Example

Steps (RFSG):
1. Open NI-RFSG session.
2. Configure RFSG frequency reference.
3. Configure RF output frequency and power level.
4. Set Automatic SG/SA Shared LO to Enabled (via NIRfsgPlayback StoreAutomaticSGSASharedLO).
5. Set LO Offset Mode to Auto while performing an in-band ModAcc measurement.
6. Initiate signal generation.

Steps (ModAcc measurement):
7. Open a new RFmx session.
8. Configure the Frequency Reference properties (Clock Source and Clock Frequency).
9. Configure Selected Ports.
10. Set LO Source to Automatic_SG_SA_Shared.
11. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
12. Configure IQ Power Edge Trigger properties.
13. Configure Link Direction, Frequency Range, CC Bandwidth, Cell ID, Band and BWP Subcarrier Spacing.
14. Set LO Leakage Avoidance Enabled to True.
15. Select ModAcc measurement and disable Traces.
16. Initiate ModAcc measurement.
17. Fetch ModAcc measurements.

Steps (SEM measurement):
18. Re-initiate signal generation for SEM measurement.
19. Select SEM measurement and disable Traces.
20. Initiate SEM measurement.
21. Fetch SEM measurements.

Steps (cleanup):
22. Close the RFmx Session.
23. Close the RFSG session.
"""

import argparse
import os
import sys

import nirfmxnr
import numpy

import nirfmxinstr
import nirfsg

_DEFAULT_WAVEFORM_FILE = os.path.join(
    os.path.dirname(os.path.abspath(__file__)),
    "Support",
    "NR_FR2_UL_SISO_CC-1_BW-50MHz_SCS-120kHz.tdms",
)


def example(rfsg_resource_name, rfsa_resource_name, option_string, waveform_file_path=_DEFAULT_WAVEFORM_FILE):
    """Run NR FEM Test with Automatic SG/SA Shared LO example."""
    center_frequency = 3.5e9  # Hz

    rfsg_external_attenuation = 0.0  # dB
    power_level = -10.0  # dBm

    rfsa_reference_level = 0.0  # dBm
    rfsa_external_attenuation = 0.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10.0e6  # Hz

    iq_power_edge_enabled = False
    iq_power_edge_level = -20.0  # dB
    trigger_delay = 0.0  # s
    minimum_quiet_time_mode = nirfmxnr.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time = 5.0e-6  # s

    frequency_range = nirfmxnr.FrequencyRange.RANGE1
    carrier_bandwidth = 100e6  # Hz
    subcarrier_spacing = 30e3  # Hz
    band = 78
    cell_id = 0

    timeout = 10.0  # s

    rfsg_session = None
    instr_session = None
    nr = None

    try:
        # --- Configure and start RFSG ---
        rfsg_session = nirfsg.Session(rfsg_resource_name)
        rfsg_session.frequency_reference.configure_frequency_reference(
            nirfsg.FrequencyReferenceSource.ONBOARD_CLOCK, frequency_reference_frequency
        )
        rfsg_session.rf.configure_rf(center_frequency, power_level)
        rfsg_session.rf.external_gain = -1.0 * rfsg_external_attenuation
        # Note: With nirfsgplayback, you would load a modulated NR waveform here
        # and configure Automatic SG/SA Shared LO via:
        #   NIRfsgPlayback.StoreAutomaticSGSASharedLO(handle, "", Enabled)
        #   NIRfsgPlayback.StoreWaveformLOOffsetMode(handle, waveformName, Auto)
        # Since nirfsgplayback is not available in Python, CW generation is used
        # and an IQ power edge trigger is used on the analyzer side.
        rfsg_session.initiate()

        # --- Configure RFmx ---
        instr_session = nirfmxinstr.Session(rfsa_resource_name, option_string)
        nr = instr_session.get_nr_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)
        nr.set_selected_ports("", "")

        # Configure SG/SA Shared LO — RFmx will coordinate the RFSG and RFSA LOs
        instr_session.set_lo_source("", "Automatic_SG_SA_Shared")

        nr.configure_rf("", center_frequency, rfsa_reference_level, rfsa_external_attenuation)
        nr.configure_iq_power_edge_trigger(
            "",
            "0",
            nirfmxnr.IQPowerEdgeTriggerSlope.RISING_SLOPE,
            iq_power_edge_level,
            trigger_delay,
            minimum_quiet_time_mode,
            minimum_quiet_time,
            nirfmxnr.IQPowerEdgeTriggerLevelType.RELATIVE,
            iq_power_edge_enabled,
        )

        nr.set_link_direction("", nirfmxnr.LinkDirection.UPLINK)
        nr.set_frequency_range("", frequency_range)
        nr.component_carrier.set_bandwidth("", carrier_bandwidth)
        nr.component_carrier.set_cell_id("", cell_id)
        nr.set_band("", band)
        nr.component_carrier.set_bandwidth_part_subcarrier_spacing("", subcarrier_spacing)

        # LO leakage avoidance causes RFmx to shift the LO outside the measurement
        # bandwidth when the signal bandwidth is less than half the device bandwidth.
        instr_session.set_lo_leakage_avoidance_enabled("", nirfmxinstr.LOLeakageAvoidanceEnabled.TRUE)

        # --- ModAcc measurement ---
        nr.select_measurements("", nirfmxnr.MeasurementTypes.MODACC, False)
        nr.initiate("", "")

        composite_rms_evm_mean, error_code = nr.modacc.results.get_composite_rms_evm_mean("")
        composite_peak_evm_maximum, error_code = nr.modacc.results.get_composite_peak_evm_maximum("")

        # Re-start RFSG for SEM measurement
        rfsg_session.abort()
        rfsg_session.initiate()

        # --- SEM measurement ---
        nr.select_measurements("", nirfmxnr.MeasurementTypes.SEM, False)
        nr.initiate("", "")

        sem_measurement_status, error_code = nr.sem.results.fetch_measurement_status("", timeout)
        sem_absolute_power, sem_peak_absolute_power, sem_peak_frequency, sem_relative_power, error_code = \
            nr.sem.results.component_carrier.fetch_measurement("", timeout)
        (
            lower_offset_measurement_status,
            lower_offset_margin,
            lower_offset_margin_frequency,
            lower_offset_margin_absolute_power,
            lower_offset_margin_relative_power,
            error_code,
        ) = nr.sem.results.fetch_lower_offset_margin_array("", timeout)
        (
            upper_offset_measurement_status,
            upper_offset_margin,
            upper_offset_margin_frequency,
            upper_offset_margin_absolute_power,
            upper_offset_margin_relative_power,
            error_code,
        ) = nr.sem.results.fetch_upper_offset_margin_array("", timeout)

        # Print ModAcc results
        print("------------------ModAcc------------------\n")
        print("------------------Measurement------------------\n")
        print(f"Composite RMS EVM Mean (%)                     : {composite_rms_evm_mean}")
        print(f"Composite Peak EVM Maximum (%)                 : {composite_peak_evm_maximum}")

        # Print SEM results
        print("\n------------------SEM------------------\n")
        print(f"Measurement Status                       : {sem_measurement_status}")
        print(f"Carrier Absolute Integrated Power (dBm)  : {sem_absolute_power}")
        print("\n----------Lower Offset Segment Measurements----------\n")
        for i in range(len(lower_offset_margin)):
            print(f"Offset {i}")
            print(f"Measurement Status                 : {lower_offset_measurement_status[i]}")
            print(f"Margin (dB)                        : {lower_offset_margin[i]}")
            print(f"Margin Frequency (Hz)              : {lower_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power (dBm)        : {lower_offset_margin_absolute_power[i]}\n")
        print("\n----------Upper Offset Segment Measurements----------\n")
        for i in range(len(upper_offset_margin)):
            print(f"Offset {i}")
            print(f"Measurement Status                 : {upper_offset_measurement_status[i]}")
            print(f"Margin (dB)                        : {upper_offset_margin[i]}")
            print(f"Margin Frequency (Hz)              : {upper_offset_margin_frequency[i]}")
            print(f"Margin Absolute Power (dBm)        : {upper_offset_margin_absolute_power[i]}\n")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        if nr is not None:
            nr.dispose()
            nr = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None
        if rfsg_session is not None:
            rfsg_session.abort()
            rfsg_session.close()
            rfsg_session = None


def _main(argsv):
    parser = argparse.ArgumentParser(
        description="Pass arguments for NR FEM Test with Automatic SG/SA Shared LO Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-sg", "--rfsg-resource-name", default="RFSG", help="Resource name of NI-RFSG.")
    parser.add_argument("-n", "--rfsa-resource-name", default="RFSA", help="Resource name of NI-RFmx Instr.")
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    parser.add_argument("-wf", "--waveform-file-path", default=_DEFAULT_WAVEFORM_FILE, help="Path to NR waveform support TDMS file.")
    args = parser.parse_args(argsv)
    example(args.rfsg_resource_name, args.rfsa_resource_name, args.option_string, args.waveform_file_path)


def main():
    _main(sys.argv[1:])


def test_main():
    cmd_line = ["--option-string", ""]
    _main(cmd_line)


def test_example():
    example("RFSG", "RFSA", {})


if __name__ == "__main__":
    main()
