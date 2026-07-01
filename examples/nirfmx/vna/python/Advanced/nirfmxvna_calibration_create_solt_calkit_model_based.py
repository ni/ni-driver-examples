"""
RFmx VNA Create SOLT Calkit (Model Based) Example

Steps:
1. Open a new RFmx session.
2. Create a new Calkit.
3. Define Male and Female Connectors for the Calkit.
4. Create Calibration Elements for Short calibration standard with Male and Female Connector.
5. Create Calibration Elements for Open calibration standard with Male and Female Connector.
6. Create Calibration Elements for Load calibration standard with Male and Female Connector.
7. Create Calibration Element for Thru calibration standard with Male-Male, Female-Female, and Male-Female Connectors.
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
    """VNA create SOLT calkit (model based) example."""
    calkit_id = "Example SOLT CalKit (Model Based)"
    if not calkit_file_path:
        calkit_file_path = os.path.join(_SUPPORT_DIR, "Example SOLT CalKit (Model Based).nckt")

    calibration_element_id_short = ["Short-M", "Short-F"]
    calibration_element_id_open = ["Open-M", "Open-F"]
    calibration_element_id_load = ["Load-M", "Load-F"]
    calibration_element_id_thru = ["Thru-M-M", "Thru-F-F", "Thru-M-F"]
    connector_id = ["3.5mm-Male", "3.5mm-Female"]
    connector_id_combined = ["3.5mm-Male,3.5mm-Male", "3.5mm-Female,3.5mm-Female", "3.5mm-Male,3.5mm-Female"]
    connector_genders = [
        nirfmxvna.CalkitManagerCalkitConnectorGender.MALE,
        nirfmxvna.CalkitManagerCalkitConnectorGender.FEMALE,
    ]

    instr_session = None
    vna_signal = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        vna_signal = instr_session.get_vna_signal_configuration()

        vna_signal.calkit_manager_create_calkit("", calkit_id)
        calkit_selector_string = nirfmxvna.Vna.build_calkit_string("", calkit_id)

        vna_signal.calkit_manager_calkit_set_description(calkit_selector_string, "Example SOLT Calkit")
        vna_signal.calkit_manager_calkit_set_version(calkit_selector_string, "1.0.0")

        # Define connectors
        for i, cid in enumerate(connector_id):
            vna_signal.calkit_manager_calkit_add_connector(calkit_selector_string, cid)
            connector_selector_string = nirfmxvna.Vna.build_connector_string(calkit_selector_string, cid)
            vna_signal.calkit_manager_calkit_connector_set_gender(connector_selector_string, connector_genders[i])
            vna_signal.calkit_manager_calkit_connector_set_type(connector_selector_string, "3.5mm")
            vna_signal.calkit_manager_calkit_connector_set_description(connector_selector_string, "Connector Description")
            vna_signal.calkit_manager_calkit_connector_set_minimum_frequency(connector_selector_string, 0.0)
            vna_signal.calkit_manager_calkit_connector_set_maximum_frequency(connector_selector_string, 100.0e9)
            vna_signal.calkit_manager_calkit_connector_set_impedance(connector_selector_string, 50.0)

        # Short elements (one per connector gender)
        for i, cid in enumerate(connector_id):
            elem_id = calibration_element_id_short[i]
            vna_signal.calkit_manager_calkit_add_calibration_element(calkit_selector_string, elem_id)
            cal_element_selector_string = nirfmxvna.Vna.build_calibration_element_string(calkit_selector_string, elem_id)
            vna_signal.calkit_manager_calkit_calibration_element_set_types(
                cal_element_selector_string,
                [nirfmxvna.CalkitManagerCalkitCalibrationElementType.SHORT],
            )
            vna_signal.calkit_manager_calkit_calibration_element_set_port_connectors(cal_element_selector_string, [cid])
            vna_signal.calkit_manager_calkit_calibration_element_set_description(cal_element_selector_string, "Example Short")
            vna_signal.calkit_manager_calkit_calibration_element_set_minimum_frequency(cal_element_selector_string, 1.0e9)
            vna_signal.calkit_manager_calkit_calibration_element_set_maximum_frequency(cal_element_selector_string, 99.0e9)
            vna_signal.calkit_manager_calkit_calibration_element_set_s_parameter_definition(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementSParameterDefinition.REFLECT_MODEL,
            )
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_model_type(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementReflectModelType.REFLECT_SHORT,
            )
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_c0(cal_element_selector_string, 2.0e-12)
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_c1(cal_element_selector_string, 1.0e-22)
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_c2(cal_element_selector_string, 2.0e-33)
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_c3(cal_element_selector_string, 1.0e-44)
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_offset_delay(cal_element_selector_string, 3.0e-11)
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_offset_loss(cal_element_selector_string, 2.0e-9)
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_offset_z0(cal_element_selector_string, 50.0)
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_s_param_availability(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.KNOWN,
            )

        # Open elements
        for i, cid in enumerate(connector_id):
            elem_id = calibration_element_id_open[i]
            vna_signal.calkit_manager_calkit_add_calibration_element(calkit_selector_string, elem_id)
            cal_element_selector_string = nirfmxvna.Vna.build_calibration_element_string(calkit_selector_string, elem_id)
            vna_signal.calkit_manager_calkit_calibration_element_set_types(
                cal_element_selector_string,
                [nirfmxvna.CalkitManagerCalkitCalibrationElementType.OPEN],
            )
            vna_signal.calkit_manager_calkit_calibration_element_set_port_connectors(cal_element_selector_string, [cid])
            vna_signal.calkit_manager_calkit_calibration_element_set_description(cal_element_selector_string, "Example Open")
            vna_signal.calkit_manager_calkit_calibration_element_set_minimum_frequency(cal_element_selector_string, 1.0e9)
            vna_signal.calkit_manager_calkit_calibration_element_set_maximum_frequency(cal_element_selector_string, 99.0e9)
            vna_signal.calkit_manager_calkit_calibration_element_set_s_parameter_definition(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementSParameterDefinition.REFLECT_MODEL,
            )
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_model_type(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementReflectModelType.REFLECT_OPEN,
            )
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_c0(cal_element_selector_string, 5.0e-14)
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_c1(cal_element_selector_string, 3.0e-25)
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_c2(cal_element_selector_string, 2.0e-35)
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_c3(cal_element_selector_string, 1.0e-46)
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_offset_delay(cal_element_selector_string, 3.0e-11)
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_offset_loss(cal_element_selector_string, 2.0e-9)
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_offset_z0(cal_element_selector_string, 50.0)
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_s_param_availability(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.KNOWN,
            )

        # Load elements
        for i, cid in enumerate(connector_id):
            elem_id = calibration_element_id_load[i]
            vna_signal.calkit_manager_calkit_add_calibration_element(calkit_selector_string, elem_id)
            cal_element_selector_string = nirfmxvna.Vna.build_calibration_element_string(calkit_selector_string, elem_id)
            vna_signal.calkit_manager_calkit_calibration_element_set_types(
                cal_element_selector_string,
                [nirfmxvna.CalkitManagerCalkitCalibrationElementType.LOAD],
            )
            vna_signal.calkit_manager_calkit_calibration_element_set_port_connectors(cal_element_selector_string, [cid])
            vna_signal.calkit_manager_calkit_calibration_element_set_description(cal_element_selector_string, "Example Load")
            vna_signal.calkit_manager_calkit_calibration_element_set_minimum_frequency(cal_element_selector_string, 1.0e9)
            vna_signal.calkit_manager_calkit_calibration_element_set_maximum_frequency(cal_element_selector_string, 99.0e9)
            vna_signal.calkit_manager_calkit_calibration_element_set_s_parameter_definition(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementSParameterDefinition.REFLECT_MODEL,
            )
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_model_type(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementReflectModelType.LOAD,
            )
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_s_param_availability(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.KNOWN,
            )

        # Thru elements (delay model)
        for i, combined_cid in enumerate(connector_id_combined):
            elem_id = calibration_element_id_thru[i]
            vna_signal.calkit_manager_calkit_add_calibration_element(calkit_selector_string, elem_id)
            cal_element_selector_string = nirfmxvna.Vna.build_calibration_element_string(calkit_selector_string, elem_id)
            vna_signal.calkit_manager_calkit_calibration_element_set_types(
                cal_element_selector_string,
                [nirfmxvna.CalkitManagerCalkitCalibrationElementType.THRU],
            )
            vna_signal.calkit_manager_calkit_calibration_element_set_port_connectors(cal_element_selector_string, [combined_cid])
            vna_signal.calkit_manager_calkit_calibration_element_set_description(cal_element_selector_string, "Example Thru")
            vna_signal.calkit_manager_calkit_calibration_element_set_minimum_frequency(cal_element_selector_string, 1.0e9)
            vna_signal.calkit_manager_calkit_calibration_element_set_maximum_frequency(cal_element_selector_string, 99.0e9)
            vna_signal.calkit_manager_calkit_calibration_element_set_s_parameter_definition(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementSParameterDefinition.DELAY_MODEL,
            )
            vna_signal.calkit_manager_calkit_calibration_element_delay_model_set_delay(cal_element_selector_string, 1.0e-11)

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
        description="VNA Create SOLT Calkit (Model Based) Example",
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
