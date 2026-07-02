"""
RFmx VNA Calset Embed Fixture Example

Steps:
1. Open a new RFmx session.
2. Load Calset data from a file.
3. Embed reverse of the input Fixture network from s2p file into new Calset.
4. Save new Calset with Embedding applied.
5. Read Frequency Grid for each calset.
6. Fetch Error Terms for each calset - Directivity, Source Match, Reflection Tracking
7. Close RFmx Session.

Note:
    Place the required files in the Support/ folder next to this script:
    - 1dB_Attenuation.s2p  (fixture S2P file)
    - Calset_Embed_Fixture.ncst  (input calset file)
"""

import argparse
import math
import os
import sys

import nirfmxinstr
import nirfmxvna

_SUPPORT_DIR = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Support")


def example(resource_name, option_string, calset_file_path="", output_calset_file_path=""):
    """VNA calset embed fixture example."""
    vna_port = "port1"
    s2p_fixture_file_path = os.path.join(_SUPPORT_DIR, "1dB_Attenuation.s2p")
    s_parameter_orientation = nirfmxvna.SParameterOrientation.PORT1_TOWARDS_VNA

    if not calset_file_path:
        calset_file_path = os.path.join(_SUPPORT_DIR, "Calset_Embed_Fixture.ncst")
    if not output_calset_file_path:
        output_calset_file_path = os.path.join(_SUPPORT_DIR, "Calset_Embed_Fixture_(Embedding_Applied).ncst")

    calset_names = ["Original Calset", "New Calset (Embedding Applied)"]

    instr_session = None
    vna_signal = None

    try:
        instr_session = nirfmxinstr.Session(resource_name, option_string)
        vna_signal = instr_session.get_vna_signal_configuration()

        vna_signal.calset_load_from_file("", calset_names[0], calset_file_path)
        vna_signal.calset_embed_fixture_s2p(
            "", calset_names[0], s2p_fixture_file_path, vna_port, s_parameter_orientation, calset_names[1]
        )
        vna_signal.calset_save_to_file("", calset_names[1], output_calset_file_path)

        error_term_types = [
            nirfmxvna.CalErrorTerm.DIRECTIVITY,
            nirfmxvna.CalErrorTerm.SOURCE_MATCH,
            nirfmxvna.CalErrorTerm.REFLECTION_TRACKING,
        ]

        for calset_name in calset_names:
            frequency_grid = vna_signal.calset_get_frequency_grid("", calset_name, nirfmxvna.CalFrequencyGrid.DIRECTIVITY)

            for error_term_type in error_term_types:
                error_terms = vna_signal.calset_get_error_term("", calset_name, error_term_type, vna_port, vna_port)
                if len(error_terms) > 0:
                    val = error_terms[0]
                    magnitude_db = 20.0 * math.log10(max(math.sqrt(val.real ** 2 + val.imag ** 2), 1e-300))

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
        description="VNA Calset Embed Fixture Example",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter,
    )
    parser.add_argument("-r", "--resource", type=str, default="VNA", help="Resource name of the VNA device")
    parser.add_argument("-o", "--option-string", type=str, default="", help="Option string")
    parser.add_argument("-cs", "--calset-file-path", type=str, default="", help="Input calset file path (.ncst)")
    parser.add_argument("-ocs", "--output-calset-file-path", type=str, default="", help="Output calset file path (.ncst)")
    return parser.parse_args()


def main():
    args = _parse_args()
    example(args.resource, args.option_string, args.calset_file_path, args.output_calset_file_path)


if __name__ == "__main__":
    main()
