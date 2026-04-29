// Steps:
// 1. Open a new RFmx session.
// 2. Load Calset data from a file.
// 3. Embed reverse of the input Fixture network from s2p file into new Calset.
// 4. Save new Calset with Embedding applied.
// 5. Read Frequency Grid for each calset.
// 6. Fetch Error Terms for each calset - Directivity, Source Match, Reflection Tracking
// 7. Close RFmx Session.

using System;
using System.IO;
using System.Reflection;
using NationalInstruments.RFmx.InstrMX;
using NationalInstruments.RFmx.VnaMX;

namespace NationalInstruments.Examples.RFmxVNACalsetEmbedFixture
{
    public class RFmxVNACalsetEmbedFixture
    {
        RFmxInstrMX instrSession;
        RFmxVnaMX vna;
        string resourceName;

        string vnaPort;

        string s2pFixtureFilePath;
        RFmxVnaMXSParameterOrientation sParameterOrientation;

        string calsetFilePath;
        string outputCalsetFilePath;
        string[] calsetNames;

        double[] frequencyGrid;

        string currentDirectoryPath;

        public void Run()
        {
            try
            {
                InitializeVariables();
                InitializeInstr();
                ConfigureVna();
                RetrieveResults();
            }
            catch (Exception ex)
            {
                DisplayError(ex);
            }
            finally
            {
                /* Close session */
                CloseSession();
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        void InitializeVariables()
        {
            resourceName = "VNA";
            vnaPort = "port1";

            s2pFixtureFilePath = "1dB_Attenuation.s2p";
            sParameterOrientation = RFmxVnaMXSParameterOrientation.Port1TowardsVna;

            calsetFilePath = "Calset_Embed_Fixture.ncst";
            outputCalsetFilePath = "Calset_Embed_Fixture_(Embedding_Applied).ncst";
            calsetNames = new string[] { "Original Calset", "New Calset (Embedding Applied)" };
        }

        void InitializeInstr()
        {
            instrSession = new RFmxInstrMX(resourceName, "");
        }

        void ConfigureVna()
        {
            vna = instrSession.GetVnaSignalConfiguration();                                      /* Create a new RFmx Session */
            
            currentDirectoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\";

            vna.CalsetLoadFromFile("", calsetNames[0], currentDirectoryPath + calsetFilePath);
            vna.CalsetEmbedFixtureS2p("", calsetNames[0], currentDirectoryPath + s2pFixtureFilePath, vnaPort, sParameterOrientation, calsetNames[1]);
            vna.CalsetSaveToFile("", calsetNames[1], currentDirectoryPath + outputCalsetFilePath);
        }

        void RetrieveResults()
        {
            var errorTermIdentifier = new RFmxVnaMXCalErrorTerm[] { RFmxVnaMXCalErrorTerm.Directivity, RFmxVnaMXCalErrorTerm.SourceMatch, RFmxVnaMXCalErrorTerm.ReflectionTracking };
            foreach (string calsetName in calsetNames)
            {
                vna.CalsetGetFrequencyGrid("", calsetName, RFmxVnaMXCalFrequencyGrid.Directivity, ref frequencyGrid);
                var errorTerms = new ComplexSingle[frequencyGrid.Length];
                foreach (RFmxVnaMXCalErrorTerm errorTermType in errorTermIdentifier)
                {
                    vna.CalsetGetErrorTerm("", calsetName, errorTermType, vnaPort, vnaPort, ref errorTerms);
                    var errorTermsMagnitude = CalculateMagnitude(ref errorTerms);
                }
            }
        }

        private float[] CalculateMagnitude(ref ComplexSingle[] complexArray)
        {
            var magnitude = new float[complexArray.Length];
            for (int i = 0; i < complexArray.Length; i++)
            {
                double absValue = Math.Sqrt(complexArray[i].Real * complexArray[i].Real + complexArray[i].Imaginary * complexArray[i].Imaginary);
                magnitude[i] = (float)(20 * Math.Log10(absValue));
            }
            return magnitude;
        }

        void CloseSession()
        {
            if (vna != null)
            {
                vna.Dispose();
                vna = null;
            }
            if (instrSession != null)
            {
                instrSession.Close();
                instrSession = null;
            }
        }

        static void DisplayError(Exception ex)
        {
            Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
        }
    }
}
