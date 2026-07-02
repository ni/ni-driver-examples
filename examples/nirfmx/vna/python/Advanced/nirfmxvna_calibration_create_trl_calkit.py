"""
RFmx VNA Create TRL Calkit Example

Steps:
1. Open a new RFmx session.
2. Create a new Calkit.
3. Define Male and Female Connectors for the Calkit.
4. Create Calibration Elements for Reflect calibration standard with Male and Female Connector.
5. Create Calibration Element for Thru calibration standard with Male-Female Connectors.
6. Create Calibration Elements for Line calibration standards with Male-Female Connectors.
7. Configure TRL Options.
8. Export the Calkit to file.
9. Close RFmx Session.
"""

import argparse
import os
import sys

import nirfmxinstr
import nirfmxvna

_SUPPORT_DIR = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Support")


def example(resource_name, option_string, calkit_file_path=""):
    """VNA create TRL calkit example."""
    calkit_id = "Example TRL Calkit"
    if not calkit_file_path:
        calkit_file_path = os.path.join(_SUPPORT_DIR, "Example TRL Calkit.nckt")

    connector_id = ["3.5 mm Male", "3.5 mm Female"]
    connector_genders = [
        nirfmxvna.CalkitManagerCalkitConnectorGender.MALE,
        nirfmxvna.CalkitManagerCalkitConnectorGender.FEMALE,
    ]
    calibration_element_id_reflect = ["Reflect-M", "Reflect-F"]
    connector_id_two_port = [connector_id[0], connector_id[1]]
    calibration_element_id_thru = "Insertable Thru"
    calibration_element_id_line = ["Line 1", "Line 2"]
    calibration_element_line_delay_sec = [55e-12, 15e-12]
    calibration_element_line_minimum_frequency_hz = [2e9, 7e9]
    calibration_element_line_maximum_frequency_hz = [7e9, 26.5e9]

    instr_session = None
    vna_signal = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        vna_signal = instr_session.get_vna_signal_configuration()

        vna_signal.calkit_manager_create_calkit("", calkit_id)
        calkit_selector_string = nirfmxvna.Vna.build_calkit_string("", calkit_id)

        vna_signal.calkit_manager_calkit_set_description(calkit_selector_string, "Example TRL Calkit")
        vna_signal.calkit_manager_calkit_set_version(calkit_selector_string, "1.0.0")

        # Define connectors
        for i, cid in enumerate(connector_id):
            vna_signal.calkit_manager_calkit_add_connector(calkit_selector_string, cid)
            connector_selector_string = nirfmxvna.Vna.build_connector_string(calkit_selector_string, cid)
            vna_signal.calkit_manager_calkit_connector_set_gender(connector_selector_string, connector_genders[i])
            vna_signal.calkit_manager_calkit_connector_set_type(connector_selector_string, "3.5 mm")
            vna_signal.calkit_manager_calkit_connector_set_description(connector_selector_string, "Connector Description")
            vna_signal.calkit_manager_calkit_connector_set_minimum_frequency(connector_selector_string, 0.0)
            vna_signal.calkit_manager_calkit_connector_set_maximum_frequency(connector_selector_string, 100.0e9)
            vna_signal.calkit_manager_calkit_connector_set_impedance(connector_selector_string, 50.0)

        # Reflect elements (one per connector)
        for i, cid in enumerate(connector_id):
            elem_id = calibration_element_id_reflect[i]
            vna_signal.calkit_manager_calkit_add_calibration_element(calkit_selector_string, elem_id)
            cal_element_selector_string = nirfmxvna.Vna.build_calibration_element_string(calkit_selector_string, elem_id)
            vna_signal.calkit_manager_calkit_calibration_element_set_types(
                cal_element_selector_string,
                [nirfmxvna.CalkitManagerCalkitCalibrationElementType.REFLECT],
            )
            vna_signal.calkit_manager_calkit_calibration_element_set_port_connectors(cal_element_selector_string, [cid])
            vna_signal.calkit_manager_calkit_calibration_element_set_description(cal_element_selector_string, "Reflect (Short)")
            vna_signal.calkit_manager_calkit_calibration_element_set_minimum_frequency(cal_element_selector_string, 0.0)
            vna_signal.calkit_manager_calkit_calibration_element_set_maximum_frequency(cal_element_selector_string, 100.0e9)
            vna_signal.calkit_manager_calkit_calibration_element_set_s_parameter_definition(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementSParameterDefinition.REFLECT_MODEL,
            )
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_model_type(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementReflectModelType.REFLECT_SHORT,
            )
            for c_setter, val in [
                (vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_c0, 0.0),
                (vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_c1, 0.0),
                (vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_c2, 0.0),
                (vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_c3, 0.0),
                (vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_offset_delay, 0.0),
                (vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_offset_loss, 0.0),
                (vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_offset_z0, 50.0),
            ]:
                c_setter(cal_element_selector_string, val)
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_s_param_availability(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.ESTIMATE,
            )

        # Thru element (insertable, zero delay)
        vna_signal.calkit_manager_calkit_add_calibration_element(calkit_selector_string, calibration_element_id_thru)
        cal_element_selector_string = nirfmxvna.Vna.build_calibration_element_string(calkit_selector_string, calibration_element_id_thru)
        vna_signal.calkit_manager_calkit_calibration_element_set_types(
            cal_element_selector_string,
            [nirfmxvna.CalkitManagerCalkitCalibrationElementType.THRU],
        )
        vna_signal.calkit_manager_calkit_calibration_element_set_port_connectors(cal_element_selector_string, connector_id_two_port)
        vna_signal.calkit_manager_calkit_calibration_element_set_description(cal_element_selector_string, "Insertable Thru")
        vna_signal.calkit_manager_calkit_calibration_element_set_minimum_frequency(cal_element_selector_string, 0.0)
        vna_signal.calkit_manager_calkit_calibration_element_set_maximum_frequency(cal_element_selector_string, 100.0e9)
        vna_signal.calkit_manager_calkit_calibration_element_set_s_parameter_definition(
            cal_element_selector_string,
            nirfmxvna.CalkitManagerCalkitCalibrationElementSParameterDefinition.DELAY_MODEL,
        )
        vna_signal.calkit_manager_calkit_calibration_element_delay_model_set_delay(cal_element_selector_string, 0.0)

        # Line elements
        for i, elem_id in enumerate(calibration_element_id_line):
            vna_signal.calkit_manager_calkit_add_calibration_element(calkit_selector_string, elem_id)
            cal_element_selector_string = nirfmxvna.Vna.build_calibration_element_string(calkit_selector_string, elem_id)
            vna_signal.calkit_manager_calkit_calibration_element_set_types(
                cal_element_selector_string,
                [nirfmxvna.CalkitManagerCalkitCalibrationElementType.LINE],
            )
            vna_signal.calkit_manager_calkit_calibration_element_set_port_connectors(cal_element_selector_string, connector_id_two_port)
            vna_signal.calkit_manager_calkit_calibration_element_set_description(cal_element_selector_string, elem_id)
            vna_signal.calkit_manager_calkit_calibration_element_set_minimum_frequency(
                cal_element_selector_string, calibration_element_line_minimum_frequency_hz[i]
            )
            vna_signal.calkit_manager_calkit_calibration_element_set_maximum_frequency(
                cal_element_selector_string, calibration_element_line_maximum_frequency_hz[i]
            )
            vna_signal.calkit_manager_calkit_calibration_element_set_s_parameter_definition(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementSParameterDefinition.DELAY_MODEL,
            )
            vna_signal.calkit_manager_calkit_calibration_element_delay_model_set_delay(
                cal_element_selector_string, calibration_element_line_delay_sec[i]
            )

        # TRL options
        vna_signal.calkit_manager_calkit_set_trl_reference_plane(
            calkit_selector_string, nirfmxvna.CalkitManagerCalkitTrlReferencePlane.THRU
        )
        vna_signal.calkit_manager_calkit_set_lrl_line_auto_char(calkit_selector_string, False)

        vna_signal.calkit_manager_export_calkit("", calkit_id, calkit_file_path)

    except Exception as e:
        print(f"ERROR: {e}")
        sys.exit(1)

    finally:
        if vna_signal is not None:
            vna_signal.dispose()
            vna_signal = None
        if instr_session is not None:
            instr_session.close()
            instr_session = None


def _parse_args():
    parser = argparse.ArgumentParser(
        description="VNA Create TRL Calkit Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-r", "--resource", type=str, default="VNA", help="Resource name of the VNA device")
    parser.add_argument("-o", "--option-string", type=str, default="", help="Option string")
    parser.add_argument("-ck", "--calkit-file-path", type=str, default="", help="Output calkit file path (.nckt)")
    return parser.parse_args()


def main():
    args = _parse_args()
    example(args.resource, args.option_string, args.calkit_file_path)


if __name__ == "__main__":
    main()
