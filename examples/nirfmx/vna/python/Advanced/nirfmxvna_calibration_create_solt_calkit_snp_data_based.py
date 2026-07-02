"""
RFmx VNA Create SOLT Calkit (SnP Data Based) Example

Steps:
1. Open a new RFmx session.
2. Create a new Calkit.
3. Define Male and Female Connectors for the Calkit.
4. Create Calibration Elements for One-port Standards.
5. Create Calibration Elements for Two-Port Thru's.
6. Export the Calkit to file.
7. Close RFmx Session.
"""

import argparse
import os
import sys

import nirfmxinstr
import nirfmxvna

_SUPPORT_DIR = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Support")


def example(resource_name, option_string, calkit_file_path=""):
    """VNA create SOLT calkit (SnP data based) example."""
    calkit_id = "Example SOLT CalKit (SnP Data Based)"
    if not calkit_file_path:
        calkit_file_path = os.path.join(_SUPPORT_DIR, "Example SOLT CalKit (SnP Data Based).nckt")

    connector_id = ["3.5mm-Male", "3.5mm-Female"]
    connector_id_combined = [
        "3.5mm-Male,3.5mm-Male",
        "3.5mm-Female,3.5mm-Female",
        "3.5mm-Male,3.5mm-Female",
    ]
    connector_genders = [
        nirfmxvna.CalkitManagerCalkitConnectorGender.MALE,
        nirfmxvna.CalkitManagerCalkitConnectorGender.FEMALE,
    ]

    one_port_files = [
        "Short(m).s1p", "Open(m).s1p", "Load(m).s1p", "OffsetShort(m).s1p",
        "Short(f).s1p", "Open(f).s1p", "Load(f).s1p", "OffsetShort(f).s1p",
    ]
    two_port_thru_files = ["Thru(m-m).s2p", "Thru(f-f).s2p", "Thru(m-f).s2p"]

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

        # One-port elements: first 4 are Male, last 4 are Female
        for j, cid in enumerate(connector_id):
            start_index = j * 4
            end_index = start_index + 4
            for i in range(start_index, end_index):
                fname = one_port_files[i]
                elem_name = os.path.splitext(fname)[0]
                vna_signal.calkit_manager_calkit_add_calibration_element(calkit_selector_string, elem_name)
                cal_element_selector_string = nirfmxvna.Vna.build_calibration_element_string(calkit_selector_string, elem_name)
                vna_signal.calkit_manager_calkit_calibration_element_set_types(
                    cal_element_selector_string,
                    [nirfmxvna.CalkitManagerCalkitCalibrationElementType.TERMINATION],
                )
                vna_signal.calkit_manager_calkit_calibration_element_set_description(cal_element_selector_string, fname)
                vna_signal.calkit_manager_calkit_calibration_element_set_port_connectors(cal_element_selector_string, [cid])
                vna_signal.calkit_manager_calkit_calibration_element_set_minimum_frequency(cal_element_selector_string, 1.0e9)
                vna_signal.calkit_manager_calkit_calibration_element_set_maximum_frequency(cal_element_selector_string, 99.0e9)
                vna_signal.calkit_manager_calkit_calibration_element_set_s_parameter_definition(
                    cal_element_selector_string,
                    nirfmxvna.CalkitManagerCalkitCalibrationElementSParameterDefinition.SPARAMETER,
                )
                vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_s_param_availability(
                    cal_element_selector_string,
                    nirfmxvna.CalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.KNOWN,
                )
                vna_signal.calkit_manager_calkit_calibration_element_s_parameter_set_from_file(
                    cal_element_selector_string,
                    os.path.join(_SUPPORT_DIR, fname),
                )

        # Two-port Thru elements
        for i, fname in enumerate(two_port_thru_files):
            elem_name = os.path.splitext(fname)[0]
            vna_signal.calkit_manager_calkit_add_calibration_element(calkit_selector_string, elem_name)
            cal_element_selector_string = nirfmxvna.Vna.build_calibration_element_string(calkit_selector_string, elem_name)
            vna_signal.calkit_manager_calkit_calibration_element_set_types(
                cal_element_selector_string,
                [nirfmxvna.CalkitManagerCalkitCalibrationElementType.THRU],
            )
            vna_signal.calkit_manager_calkit_calibration_element_set_description(cal_element_selector_string, fname)
            vna_signal.calkit_manager_calkit_calibration_element_set_port_connectors(
                cal_element_selector_string, [connector_id_combined[i]]
            )
            vna_signal.calkit_manager_calkit_calibration_element_set_minimum_frequency(cal_element_selector_string, 1.0e9)
            vna_signal.calkit_manager_calkit_calibration_element_set_maximum_frequency(cal_element_selector_string, 99.0e9)
            vna_signal.calkit_manager_calkit_calibration_element_set_s_parameter_definition(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementSParameterDefinition.SPARAMETER,
            )
            vna_signal.calkit_manager_calkit_calibration_element_reflect_model_set_s_param_availability(
                cal_element_selector_string,
                nirfmxvna.CalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.KNOWN,
            )
            vna_signal.calkit_manager_calkit_calibration_element_s_parameter_set_from_file(
                cal_element_selector_string,
                os.path.join(_SUPPORT_DIR, fname),
            )

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
        description="VNA Create SOLT Calkit (SnP Data Based) Example",
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
