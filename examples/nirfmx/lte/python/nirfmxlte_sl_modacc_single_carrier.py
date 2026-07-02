"""
RFmx LTE SL ModAcc Single Carrier Example

Steps:
1. Open a new RFmx Session.
2. Configure Frequency Reference.
3. Configure basic signal properties (Center Frequency, Reference Level and External Attenuation).
4. Configure Trigger Type and Trigger Parameters.
5. Configure Carrier Bandwidth.
6. Configure Link Direction to Sidelink.
7. Configure operating Band to 47.
8. Configure Auto Resource Block Detection Enabled to True.
9. Configure Auto DMRS Detection Enabled to True.
10. Select ModAcc measurement and enable Traces.
11. Configure Synchronization Mode and Measurement Interval.
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
    """LTE SL ModAcc single carrier measurement example."""
    # Configuration parameters
    frequency_reference_source = "OnboardClock"
    frequency_reference_frequency = 10e6  # Hz

    center_frequency = 5.89e9  # Hz
    reference_level = 0.0  # dBm
    external_attenuation = 0.0  # dB

    iq_power_edge_trigger_source = "0"
    enable_trigger = True
    iq_power_edge_trigger_level = -20.0  # dB (relative)
    iq_power_edge_trigger_slope = nirfmxlte.IQPowerEdgeTriggerSlope.RISING_SLOPE
    iq_power_edge_trigger_level_type = nirfmxlte.IQPowerEdgeTriggerLevelType.RELATIVE
    trigger_delay = 0.0  # s
    minimum_quiet_time_mode = nirfmxlte.TriggerMinimumQuietTimeMode.AUTO
    minimum_quiet_time_duration = 50.0e-6  # s

    carrier_bandwidth = 10e6  # Hz
    band = 47
    link_direction = nirfmxlte.LinkDirection.SIDELINK

    auto_resource_block_detection_enabled = (
        nirfmxlte.AutoResourceBlockDetectionEnabled.TRUE
    )
    auto_dmrs_detection_enabled = nirfmxlte.AutoDmrsDetectionEnabled.TRUE

    synchronization_mode = nirfmxlte.ModAccSynchronizationMode.SLOT
    measurement_offset = 0  # slots
    measurement_length = 1  # slots

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

        lte_signal.configure_iq_power_edge_trigger(
            "",
            iq_power_edge_trigger_source,
            iq_power_edge_trigger_slope,
            iq_power_edge_trigger_level,
            trigger_delay,
            minimum_quiet_time_mode,
            minimum_quiet_time_duration,
            iq_power_edge_trigger_level_type,
            enable_trigger,
        )

        lte_signal.component_carrier.configure("", carrier_bandwidth, 0.0, 0)

        lte_signal.configure_link_direction("", link_direction)

        lte_signal.configure_band("", band)

        lte_signal.component_carrier.configure_auto_resource_block_detection_enabled(
            "", auto_resource_block_detection_enabled
        )

        lte_signal.configure_auto_dmrs_detection_enabled("", auto_dmrs_detection_enabled)

        lte_signal.select_measurements("", nirfmxlte.MeasurementTypes.MODACC, True)

        lte_signal.modacc.configuration.configure_synchronization_mode_and_interval(
            "", synchronization_mode, measurement_offset, measurement_length
        )

        lte_signal.modacc.configuration.configure_evm_unit("", evm_unit)

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

        (
            mean_iq_origin_offset,
            mean_iq_gain_imbalance,
            mean_iq_quadrature_error,
            error_code,
        ) = lte_signal.modacc.results.fetch_iq_impairments("", timeout)

        in_band_emission_margin, error_code = (
            lte_signal.modacc.results.fetch_in_band_emission_margin("", timeout)
        )

        data_constellation = numpy.empty(0, dtype=numpy.complex64)
        dmrs_constellation = numpy.empty(0, dtype=numpy.complex64)
        error_code = lte_signal.modacc.results.fetch_pssch_constellation_trace(
            "", timeout, data_constellation, dmrs_constellation
        )

        evm_per_subcarrier = numpy.empty(0, dtype=numpy.float32)
        x0, dx, error_code = lte_signal.modacc.results.fetch_evm_per_subcarrier_trace(
            "", timeout, evm_per_subcarrier
        )

        print("------------------Measurement------------------")
        print(f"Mean RMS Composite EVM  (% or dB)       : {mean_rms_composite_evm}")
        print(f"Max Peak Composite EVM  (% or dB)       : {max_peak_composite_evm}")
        print(f"Peak Composite EVM Slot Index           : {peak_composite_evm_slot_index}")
        print(f"Peak Composite EVM Symbol Index         : {peak_composite_evm_symbol_index}")
        print(f"Peak Composite EVM Subcarrier Index     : {peak_composite_evm_subcarrier_index}")
        print(f"Mean Frequency Error  (Hz)              : {mean_frequency_error}")
        print(f"Mean IQ Origin Offset  (dBc)            : {mean_iq_origin_offset}")
        print(f"Mean IQ Gain Imbalance  (dB)            : {mean_iq_gain_imbalance}")
        print(f"Mean IQ Quadrature Error  (deg)         : {mean_iq_quadrature_error}")
        print(f"In Band Emission Margin  (dB)           : {in_band_emission_margin}")

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
        description="Pass arguments for LTE SL ModAcc Single Carrier Example",
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
