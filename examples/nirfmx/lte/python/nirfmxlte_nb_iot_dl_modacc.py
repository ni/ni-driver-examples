"""
RFmx LTE NB-IoT DL ModAcc Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Carrier Bandwidth.
6. Select Downlink as Link Direction.
7. Configure Downlink Test Model.
8. Select ModAcc measurement and enable Traces.
9. Configure Averaging Parameters for ModAcc measurement.
10. Select Frame as Synchronization Mode and configure Measurement Interval.
11. Configure EVM Unit.
12. Initiate the Measurement.
13. Fetch ModAcc Measurements and Traces.
14. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxlte
import numpy


def example(resource_name, option_string):
    """LTE NB-IoT downlink ModAcc measurement example."""
    # Configuration parameters
    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    center_frequency = 2.14e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    enable_trigger = False
    digital_edge_source = "PFI0"
    digital_edge = nirfmxlte.DigitalEdgeTriggerEdge.RISING_EDGE
    trigger_delay = 0.0  # s

    component_carrier_bandwidth = 200e3  # Hz (NB-IoT: 200 kHz)
    component_carrier_frequency = 0.0  # Hz
    cell_id = 0

    n_cell_id = 0
    nb_iot_downlink_channel_configuration_mode = (
        nirfmxlte.NBIoTDownlinkChannelConfigurationMode.TEST_MODEL
    )

    averaging_enabled = nirfmxlte.ModAccAveragingEnabled.FALSE
    averaging_count = 10

    measurement_offset = 0  # slots
    measurement_length = 20  # slots

    timeout = 10.0  # s

    instr_session = None
    lte_signal = None

    try:
        # Create a new RFmx Session
        instr_session = nirfmxinstr.Session(resource_name, option_string)

        # Get LTE signal configuration
        lte_signal = instr_session.get_lte_signal_configuration()

        instr_session.configure_frequency_reference(
            "", frequency_reference_source, frequency_reference_frequency
        )

        lte_signal.configure_rf("", center_frequency, reference_level, external_attenuation)

        lte_signal.configure_digital_edge_trigger(
            "", digital_edge_source, digital_edge, trigger_delay, enable_trigger
        )

        lte_signal.component_carrier.configure(
            "", component_carrier_bandwidth, component_carrier_frequency, cell_id
        )

        lte_signal.configure_link_direction("", nirfmxlte.LinkDirection.DOWNLINK)

        lte_signal.select_measurements("", nirfmxlte.MeasurementTypes.MODACC, True)

        lte_signal.modacc.configuration.configure_averaging("", averaging_enabled, averaging_count)

        lte_signal.modacc.configuration.configure_synchronization_mode_and_interval(
            "",
            nirfmxlte.ModAccSynchronizationMode.FRAME,
            measurement_offset,
            measurement_length,
        )

        lte_signal.modacc.configuration.configure_evm_unit(
            "", nirfmxlte.ModAccEvmUnit.PERCENTAGE
        )

        lte_signal.component_carrier.set_nb_iot_downlink_channel_configuration_mode(
            "", nb_iot_downlink_channel_configuration_mode
        )

        lte_signal.initiate("", "")

        (
            mean_rms_composite_evm,
            max_peak_composite_evm,
            mean_frequency_error,
            peak_composite_evm_symbol_index,
            peak_composite_evm_subcarrier_index,
            peak_composite_evm_slot_index,
            error_code,
        ) = lte_signal.modacc.results.fetch_composite_evm("", timeout)

        mean_iq_origin_offset, mean_iq_gain_imbalance, mean_iq_quadrature_error, error_code = (
            lte_signal.modacc.results.fetch_iq_impairments("", timeout)
        )

        mean_rms_npdsch_evm, error_code = lte_signal.modacc.results.get_npdsch_mean_rms_evm("")
        mean_rms_npdsch_qpsk_evm, error_code = (
            lte_signal.modacc.results.get_npdsch_mean_rms_qpsk_evm("")
        )
        mean_rms_npss_evm, error_code = lte_signal.modacc.results.get_mean_rms_npss_evm("")
        mean_rms_nsss_evm, error_code = lte_signal.modacc.results.get_mean_rms_nsss_evm("")
        mean_rms_nrs_evm, error_code = lte_signal.modacc.results.get_mean_rms_nrs_evm("")

        qpsk_constellation = numpy.empty(0, dtype=numpy.complex64)
        error_code = lte_signal.modacc.results.fetch_npdsch_qpsk_constellation(
            "", timeout, qpsk_constellation
        )

        mean_rms_evm_per_subcarrier = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = lte_signal.modacc.results.fetch_evm_per_subcarrier_trace(
            "", timeout, mean_rms_evm_per_subcarrier
        )

        # Print results
        print("------------------Measurement---------------")
        print(f"Mean RMS Composite EVM  (% or dB)    : {mean_rms_composite_evm}")
        print(f"Max Peak Composite EVM  (% or dB)    : {max_peak_composite_evm}")
        print(f"NPDSCH Mean RMS  EVM  (% or dB)      : {mean_rms_npdsch_evm}")
        print(f"NPDSCH Mean RMS QPSK EVM  (% or dB)  : {mean_rms_npdsch_qpsk_evm}")
        print(f"Mean RMS NPSS EVM  (% or dB)         : {mean_rms_npss_evm}")
        print(f"Mean RMS NSSS EVM  (% or dB)         : {mean_rms_nsss_evm}")
        print(f"Mean RMS NRS EVM (% or dB)           : {mean_rms_nrs_evm}")
        print(f"Mean Frequency Error  (Hz)           : {mean_frequency_error}")
        print(f"Mean IQ Origin Offset  (dBc)         : {mean_iq_origin_offset}")

    except Exception as e:
        print("ERROR: " + str(e))

    finally:
        # Close Session
        if lte_signal is not None:
            lte_signal.dispose()
            lte_signal = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def main():
    """Main entry point."""
    parser = argparse.ArgumentParser(
        description="RFmx LTE NB-IoT DL ModAcc Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "--resource-name",
        default="RFSA",
        help="RFSA resource name.",
    )
    parser.add_argument(
        "--option-string",
        default="",
        help="RFSA option string.",
    )
    args = parser.parse_args()
    example(args.resource_name, args.option_string)
    return 0


if __name__ == "__main__":
    sys.exit(main())
