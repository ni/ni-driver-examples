"""
RFmx LTE DL ModAcc Non-Contiguous Multi-Carrier Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Duplex Mode.
6. Configure Link Direction as Downlink.
7. Configure Subblock Parameters:
   7A. Configure Number of Subblocks.
   7B. Configure subblock Frequency.
   7C. Configure Component Carrier Spacing.
   7D. Configure Band.
   7E. Configure Number of Component Carriers.
   7F. Configure Component Carriers (Component Carrier Frequency and Component Carrier Bandwidth).
8. Configure Downlink Test Model.
9. Select ModAcc measurement and enable Traces.
10. Configure Averaging Parameters for ModAcc measurement.
11. Select Frame as Synchronization Mode and configure Measurement Interval.
12. Configure EVM Unit.
13. Initiate the Measurement.
14. Fetch ModAcc Measurements and Traces.
15. Close RFmx Session.
"""

import argparse
import sys

import nirfmxinstr
import nirfmxlte
import numpy


def example(resource_name, option_string):
    """LTE DL ModAcc non-contiguous multi-carrier measurement example."""
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

    duplex_scheme = nirfmxlte.DuplexScheme.FDD
    uplink_downlink_configuration = nirfmxlte.UplinkDownlinkConfiguration.CONFIGURATION_0
    link_direction = nirfmxlte.LinkDirection.DOWNLINK

    number_of_subblocks = 2
    number_of_component_carriers = 1

    # Per-subblock configuration
    subblock_frequency = [0.0, 30e6]  # Hz
    subblock_component_carrier_spacing_type = [
        nirfmxlte.ComponentCarrierSpacingType.NOMINAL,
        nirfmxlte.ComponentCarrierSpacingType.NOMINAL,
    ]
    subblock_component_carrier_at_center_frequency = [-1, -1]
    subblock_band = [1, 1]
    subblock_component_carrier_bandwidth = [[20e6], [20e6]]  # Hz
    subblock_component_carrier_frequency = [[0.0], [0.0]]  # Hz
    subblock_cell_id = [[0], [0]]
    subblock_downlink_test_model = [
        [nirfmxlte.DownlinkTestModel.TM_1_1],
        [nirfmxlte.DownlinkTestModel.TM_1_1],
    ]

    averaging_enabled = nirfmxlte.ModAccAveragingEnabled.FALSE
    averaging_count = 10

    synchronization_mode = nirfmxlte.ModAccSynchronizationMode.FRAME
    measurement_offset = 0  # frames
    measurement_length = 1  # frames

    evm_unit = nirfmxlte.ModAccEvmUnit.PERCENTAGE

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

        lte_signal.configure_duplex_scheme("", duplex_scheme, uplink_downlink_configuration)

        lte_signal.configure_link_direction("", link_direction)

        lte_signal.configure_number_of_subblocks("", number_of_subblocks)

        for i in range(number_of_subblocks):
            subblock_string = nirfmxlte.Lte.build_subblock_string("", i)
            lte_signal.set_subblock_frequency(subblock_string, subblock_frequency[i])
            lte_signal.component_carrier.configure_spacing(
                subblock_string,
                subblock_component_carrier_spacing_type[i],
                subblock_component_carrier_at_center_frequency[i],
            )
            lte_signal.configure_band(subblock_string, subblock_band[i])
            lte_signal.configure_number_of_component_carriers(
                subblock_string, number_of_component_carriers
            )
            lte_signal.component_carrier.configure_array(
                subblock_string,
                subblock_component_carrier_bandwidth[i],
                subblock_component_carrier_frequency[i],
                subblock_cell_id[i],
            )
            lte_signal.component_carrier.configure_downlink_test_model_array(
                subblock_string, subblock_downlink_test_model[i]
            )

        lte_signal.select_measurements("", nirfmxlte.MeasurementTypes.MODACC, True)

        lte_signal.modacc.configuration.configure_averaging("", averaging_enabled, averaging_count)

        lte_signal.modacc.configuration.configure_synchronization_mode_and_interval(
            "", synchronization_mode, measurement_offset, measurement_length
        )

        lte_signal.modacc.configuration.configure_evm_unit("", evm_unit)

        lte_signal.initiate("", "")

        print("----------------------Measurements--------------------")
        for i in range(number_of_subblocks):
            subblock_string = nirfmxlte.Lte.build_subblock_string("", i)
            (
                mean_rms_composite_evm,
                maximum_peak_composite_evm,
                mean_frequency_error,
                peak_composite_evm_symbol_index,
                peak_composite_evm_subcarrier_index,
                peak_composite_evm_slot_index,
                error_code,
            ) = lte_signal.modacc.results.fetch_composite_evm_array(subblock_string, timeout)

            (
                mean_iq_origin_offset,
                mean_iq_gain_imbalance,
                mean_iq_quadrature_error,
                error_code,
            ) = lte_signal.modacc.results.fetch_iq_impairments_array(subblock_string, timeout)

            (
                mean_rms_evm,
                mean_rms_qpsk_evm,
                mean_rms_16qam_evm,
                mean_rms_64qam_evm,
                mean_rms_256qam_evm,
                error_code,
            ) = lte_signal.modacc.results.fetch_pdsch_evm_array(subblock_string, timeout)

            mean_rms_1024qam_evm, error_code = (
                lte_signal.modacc.results.fetch_pdsch_1024_qam_evm_array(subblock_string, timeout)
            )

            for j in range(number_of_component_carriers):
                carrier_string = nirfmxlte.Lte.build_carrier_string(subblock_string, j)
                qpsk_constellation = numpy.empty(0, dtype=numpy.complex64)
                error_code = lte_signal.modacc.results.fetch_pdsch_qpsk_constellation(
                    carrier_string, timeout, qpsk_constellation
                )
                evm_per_subcarrier = numpy.empty(0, dtype=numpy.float32)
                x0, dx, error_code = lte_signal.modacc.results.fetch_evm_per_subcarrier_trace(
                    carrier_string, timeout, evm_per_subcarrier
                )

            print(f"\nSubblock Number {i}")
            print("-----------Component Carrier Measurements----------------")
            for j in range(number_of_component_carriers):
                print(f"Carrier {j}")
                print(f"Mean Rms Composite Evm  (%  or dB)      : {mean_rms_composite_evm[j]}")
                print(f"Mean Rms Evm  (%  or dB)                : {mean_rms_evm[j]}")
                print(f"Mean Rms Qpsk Evm  (%  or dB)           : {mean_rms_qpsk_evm[j]}")
                print(f"Mean Rms 16Qam Evm  (%  or dB)          : {mean_rms_16qam_evm[j]}")
                print(f"Mean Rms 64Qam Evm  (%  or dB)          : {mean_rms_64qam_evm[j]}")
                print(f"Mean Rms 256Qam Evm  (%  or dB)         : {mean_rms_256qam_evm[j]}")
                print(f"Mean Rms 1024Qam Evm  (%  or dB)        : {mean_rms_1024qam_evm[j]}")
                print(f"Mean Frequency Error  (Hz)              : {mean_frequency_error[j]}")
                print(f"Mean IQ Origin Offset  (dBc)            : {mean_iq_origin_offset[j]}")
                print(f"Mean IQ Gain Imbalance  (dB)            : {mean_iq_gain_imbalance[j]}")
                print(f"Mean IQ Quadrature Error  (deg)         : {mean_iq_quadrature_error[j]}")

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


def _main(argsv):
    """Parse the arguments and call example function."""
    parser = argparse.ArgumentParser(
        description="Pass arguments for LTE DL ModAcc Non-Contiguous Multi-Carrier Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument(
        "-n", "--resource-name", default="RFSA", help="Resource name of NI-RFmx Instrument"
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
        "--resource-name",
        "RFSA",
        "--option-string",
        "",
    ]
    _main(cmd_line)


def test_example():
    """Call example function."""
    example("RFSA", "")


if __name__ == "__main__":
    main()
