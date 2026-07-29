"""
RFmx NR FEM Test with Automatic SG/SA Shared LO Example

Steps (RFSG):
1. Open NI-RFSG session.
2. Set generation mode to Script.
3. Configure RFSG frequency reference.
4. Configure RF output frequency and power level.
5. Set RFSG LO Source to Automatic_SG_SA_Shared.
6. Read waveform from file and download it to RFSG memory.
7. Write generation script and initiate signal generation.

Steps (ModAcc measurement):
8.  Open a new RFmx session.
9.  Configure the Frequency Reference properties (Clock Source and Clock Frequency).
10. Configure Selected Ports.
11. Set LO Source to Automatic_SG_SA_Shared.
12. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
13. Configure Digital Edge Trigger properties.
14. Configure Link Direction, Frequency Range, CC Bandwidth, Cell ID, Band and BWP Subcarrier Spacing.
15. Set LO Leakage Avoidance Enabled to True.
16. Select ModAcc measurement and disable Traces.
17. Initiate ModAcc measurement.
18. Fetch ModAcc measurements.

Steps (SEM measurement):
19. Abort RFSG, write script, and re-initiate for SEM measurement.
20. Select SEM measurement and disable Traces.
21. Initiate SEM measurement.
22. Fetch SEM measurements.

Steps (cleanup):
23. Close the RFmx Session.
24. Close the RFSG session.
"""

import argparse
import os
import sys

import nirfmxnr

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

    enable_trigger = False
    digital_edge_source = "PXI_Trig0"
    digital_edge = nirfmxnr.DigitalEdgeTriggerEdge.RISING_EDGE
    trigger_delay = 0.0  # s

    frequency_range = nirfmxnr.FrequencyRange.RANGE2_1
    carrier_bandwidth = 50e6  # Hz
    subcarrier_spacing = 120e3  # Hz
    band = 257
    cell_id = 0

    waveform_name = "Wfm"
    script_name = "GenerateWaveform"

    timeout = 10.0  # s

    rfsg_session = None
    instr_session = None
    nr = None

    try:
        # --- Configure and start RFSG ---
        rfsg_session = nirfsg.Session(rfsg_resource_name)
        rfsg_session.generation_mode = nirfsg.GenerationMode.SCRIPT
        rfsg_session.configure_ref_clock(frequency_reference_source, frequency_reference_frequency)
        rfsg_session.configure_rf(center_frequency, power_level)
        rfsg_session.external_gain = -1.0 * rfsg_external_attenuation
        rfsg_session.read_and_download_waveform_from_file_tdms(waveform_name, waveform_file_path, 0)
        rfsg_session.lo_source = nirfsg.LoSource.AUTOMATIC_SG_SA_SHARED
        waveform_script = (
            f"script {script_name}\n"
            "repeat forever\n"
            f"generate {waveform_name}\n"
            "end repeat\n"
            "end script"
        )
        rfsg_session.write_script(waveform_script)
        rfsg_session.selected_script = script_name
        rfsg_session.initiate()

        # --- Configure RFmx ---
        instr_session = nirfmxinstr.Session(rfsa_resource_name, option_string)
        nr = instr_session.get_nr_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)
        nr.set_selected_ports("", "")

        # Configure SG/SA Shared LO — RFmx will coordinate the RFSG and RFSA LOs
        instr_session.set_lo_source("", "Automatic_SG_SA_Shared")

        nr.configure_rf("", center_frequency, rfsa_reference_level, rfsa_external_attenuation)
        nr.configure_digital_edge_trigger("", digital_edge_source, digital_edge, trigger_delay, enable_trigger)

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
        rfsg_session.write_script(waveform_script)
        rfsg_session.selected_script = script_name
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
            try:
                rfsg_session.clear_arb_waveform(waveform_name)
            except Exception:
                pass
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
    example("RFSG", "RFSA", "", _DEFAULT_WAVEFORM_FILE)


if __name__ == "__main__":
    main()
