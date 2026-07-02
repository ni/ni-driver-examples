r"""Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure Selected Ports.
4. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
5. Configure Trigger Type and Trigger Parameters.
6. Configure Link Direction as Downlink, Frequency Range, Carrier Bandwidth and BWP Subcarrier Spacing.
7. Configure DL Test Model and DL Test Model Duplex Scheme.
8. Select ModAcc measurement and enable Traces.
9. Configure Synchronization Mode and Averaging Parameters for ModAcc measurement.
10. Configure Measurement Interval.
11. Initiate the Measurement.
12. Fetch ModAcc Measurements and Traces.
13. Close RFmx Session.
"""

import argparse
import sys

import nirfmxnr
import numpy

import nirfmxinstr


def example(resource_name, option_string):
    """Run Example."""
    selected_ports = ""
    center_frequency = 3.5e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10.0e6  # Hz

    enable_trigger = False
    digital_edge_source = "PXI_Trig0"
    digital_edge = nirfmxnr.DigitalEdgeTriggerEdge.RISING_EDGE
    trigger_delay = 0.0  # s

    frequency_range = nirfmxnr.FrequencyRange.RANGE1
    carrier_bandwidth = 100e6  # Hz
    subcarrier_spacing = 30e3  # Hz

    downlink_test_model = nirfmxnr.DownlinkTestModel.TM1_1
    downlink_test_model_duplex_scheme = nirfmxnr.DownlinkTestModelDuplexScheme.FDD

    synchronization_mode = nirfmxnr.ModAccSynchronizationMode.SLOT

    averaging_enabled = nirfmxnr.ModAccAveragingEnabled.FALSE
    averaging_count = 10

    measurement_length_unit = nirfmxnr.ModAccMeasurementLengthUnit.SLOT
    measurement_offset = 0.0
    measurement_length = 1

    timeout = 10.0  # s

    instr_session = None
    nr = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        nr = instr_session.get_nr_signal_configuration()

        instr_session.configure_frequency_reference("", frequency_reference_source, frequency_reference_frequency)
        nr.set_selected_ports("", selected_ports)
        nr.configure_rf("", center_frequency, reference_level, external_attenuation)
        nr.configure_digital_edge_trigger("", digital_edge_source, digital_edge, trigger_delay, enable_trigger)

        nr.set_link_direction("", nirfmxnr.LinkDirection.DOWNLINK)
        nr.set_frequency_range("", frequency_range)
        nr.component_carrier.set_bandwidth("", carrier_bandwidth)
        nr.component_carrier.set_bandwidth_part_subcarrier_spacing("", subcarrier_spacing)

        nr.component_carrier.set_downlink_test_model("", downlink_test_model)
        nr.component_carrier.set_downlink_test_model_duplex_scheme("", downlink_test_model_duplex_scheme)

        nr.select_measurements("", nirfmxnr.MeasurementTypes.MODACC, True)

        nr.modacc.configuration.set_synchronization_mode("", synchronization_mode)
        nr.modacc.configuration.set_averaging_enabled("", averaging_enabled)
        nr.modacc.configuration.set_averaging_count("", averaging_count)
        nr.modacc.configuration.set_measurement_length_unit("", measurement_length_unit)
        nr.modacc.configuration.set_measurement_offset("", measurement_offset)
        nr.modacc.configuration.set_measurement_length("", measurement_length)

        nr.initiate("", "")

        # Retrieve results
        composite_rms_evm_mean, error_code = nr.modacc.results.get_composite_rms_evm_mean("")
        composite_peak_evm_maximum, error_code = nr.modacc.results.get_composite_peak_evm_maximum("")
        composite_peak_evm_slot_index, error_code = nr.modacc.results.get_composite_peak_evm_slot_index("")
        composite_peak_evm_symbol_index, error_code = nr.modacc.results.get_composite_peak_evm_symbol_index("")
        composite_peak_evm_subcarrier_index, error_code = nr.modacc.results.get_composite_peak_evm_subcarrier_index("")

        component_carrier_frequency_error_mean, error_code = nr.modacc.results.get_component_carrier_frequency_error_mean("")
        component_carrier_iq_origin_offset_mean, error_code = nr.modacc.results.get_component_carrier_iq_origin_offset_mean("")
        component_carrier_iq_gain_imbalance_mean, error_code = nr.modacc.results.get_component_carrier_iq_gain_imbalance_mean("")
        component_carrier_quadrature_error_mean, error_code = nr.modacc.results.get_component_carrier_quadrature_error_mean("")

        pdsch_constellation = numpy.empty(0, dtype=numpy.complex64)
        if downlink_test_model in (
            nirfmxnr.DownlinkTestModel.TM1_1,
            nirfmxnr.DownlinkTestModel.TM1_2,
            nirfmxnr.DownlinkTestModel.TM3_3,
        ):
            pdsch_rms_evm_mean, error_code = nr.modacc.results.get_pdsch_qpsk_rms_evm_mean("")
            nr.modacc.results.fetch_pdsch_qpsk_constellation_trace("", timeout, pdsch_constellation)
        elif downlink_test_model in (
            nirfmxnr.DownlinkTestModel.TM2,
            nirfmxnr.DownlinkTestModel.TM3_1,
        ):
            pdsch_rms_evm_mean, error_code = nr.modacc.results.get_pdsch_64qam_rms_evm_mean("")
            nr.modacc.results.fetch_pdsch_64qam_constellation_trace("", timeout, pdsch_constellation)
        elif downlink_test_model in (
            nirfmxnr.DownlinkTestModel.TM2A,
            nirfmxnr.DownlinkTestModel.TM3_1A,
        ):
            pdsch_rms_evm_mean, error_code = nr.modacc.results.get_pdsch_256qam_rms_evm_mean("")
            nr.modacc.results.fetch_pdsch_256qam_constellation_trace("", timeout, pdsch_constellation)
        elif downlink_test_model == nirfmxnr.DownlinkTestModel.TM3_2:
            pdsch_rms_evm_mean, error_code = nr.modacc.results.get_pdsch_16qam_rms_evm_mean("")
            nr.modacc.results.fetch_pdsch_16qam_constellation_trace("", timeout, pdsch_constellation)
        else:
            pdsch_rms_evm_mean = 0.0

        rms_evm_per_subcarrier_mean = numpy.empty(0, dtype=numpy.float32)
        nr.modacc.results.fetch_rms_evm_per_subcarrier_mean_trace("", timeout, rms_evm_per_subcarrier_mean)

        rms_evm_per_symbol_mean = numpy.empty(0, dtype=numpy.float32)
        nr.modacc.results.fetch_rms_evm_per_symbol_mean_trace("", timeout, rms_evm_per_symbol_mean)

        # Print Results
        print("------------------Measurement------------------\n")
        print(f"Composite RMS EVM Mean (%)                     : {composite_rms_evm_mean}")
        print(f"Composite Peak EVM Maximum (%)                 : {composite_peak_evm_maximum}")
        print(f"Composite Peak EVM Slot Index                  : {composite_peak_evm_slot_index}")
        print(f"Composite Peak EVM Symbol Index                : {composite_peak_evm_symbol_index}")
        print(f"Composite Peak EVM Subcarrier Index            : {composite_peak_evm_subcarrier_index}")
        print(f"PDSCH RMS EVM Mean (%)                         : {pdsch_rms_evm_mean}")
        print(f"Component Carrier Frequency Error Mean (Hz)    : {component_carrier_frequency_error_mean}")
        print(f"Component Carrier IQ Origin Offset Mean (dBc)  : {component_carrier_iq_origin_offset_mean}")
        print(f"Component Carrier IQ Gain Imbalance Mean (dB)  : {component_carrier_iq_gain_imbalance_mean}")
        print(f"Component Carrier Quadrature Error Mean (deg)  : {component_carrier_quadrature_error_mean}\n")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        if nr is not None:
            nr.dispose()
            nr = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _main(argsv):
    parser = argparse.ArgumentParser(
        description="Pass arguments for DL ModAcc Single Carrier Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instr.")
    parser.add_argument("-op", "--option-string", default="", type=str, help="Option string")
    args = parser.parse_args(argsv)
    example(args.resource_name, args.option_string)


def main():
    _main(sys.argv[1:])


def test_main():
    cmd_line = ["--option-string", ""]
    _main(cmd_line)


def test_example():
    example("RFSA", {})


if __name__ == "__main__":
    main()
