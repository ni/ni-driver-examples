//Steps:
//1. Open a new RFmxInstrMX session and create a Demod Signal
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Select QAM Modulation and M
//5. Configure DDemod Symbol Rate, Number of Symbols and Pulse Shaping Filter
//6. Configure DDemod Measurement Filter Type as Auto
//7. Configure DDemod Averaging
//8. Read DDemod Measurement Results
//9. Dispose Demod Signal and Close the RFmxInstrMX Session


using System;
using NationalInstruments.RFmx.DemodMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxDemodDDemodQamBasic
{
    public class RFmxDemodDDemodQamBasic
    {
        RFmxDemodMX demod;
        RFmxInstrMX instrSession;


        public void CloseSession()
        {
            if (demod != null)
            {
                demod.Dispose();
                demod = null;
            }
            if (instrSession != null)
            {
                instrSession.Close();
                instrSession = null;
            }
        }

        public void Run()
        {
            try
            {
                //declaring the variables
                string selectedPorts;
                double centerFrequency, referenceLevel, externalAttenuation;
                double symbolRate, pulseShapingFilterParameter;
                int numOfSymbols;
                int averagingCount;
                double meanFrequencyOffset, meanRmsEvm, maximumPeakEvm, meanModulationErrorRatio, timeout = 10.0;
                selectedPorts = "";
                centerFrequency = 1e+9;
                referenceLevel = 0.00;
                externalAttenuation = 0.00;
                RFmxDemodMXDDemodM m = RFmxDemodMXDDemodM.M16;
                symbolRate = 100.000e+3;
                numOfSymbols = 1000;
                pulseShapingFilterParameter = 0.50;
                averagingCount = 10;

                //initialising the new session
                string resourceName = "RFSA";
                instrSession = new RFmxInstrMX(resourceName, "");
                demod = instrSession.GetDemodSignalConfiguration();
                //configuring the session
                demod.SetSelectedPorts("", selectedPorts);
                demod.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
                demod.DDemod.Configuration.ConfigureModulationType("", RFmxDemodMXDDemodModulationType.Qam,
                    m, RFmxDemodMXDDemodDifferentialEnabled.False);
                demod.DDemod.Configuration.ConfigureSymbolRate("", symbolRate);
                demod.DDemod.Configuration.ConfigureNumberOfSymbols("", numOfSymbols);

                demod.DDemod.Configuration.ConfigurePulseShapingFilter("", RFmxDemodMXDDemodPulseShapingFilterType.RootRaisedCosine, pulseShapingFilterParameter, 0, 1, null);
                demod.DDemod.Configuration.ConfigureMeasurementFilter("", RFmxDemodMXDDemodMeasurementFilterType.Auto, 0, 1, null);

                demod.DDemod.Configuration.ConfigureAveraging("", RFmxDemodMXDDemodAveragingEnabled.False, averagingCount);
                //retriving the results
                demod.DDemod.Results.Read("", timeout, out meanFrequencyOffset, out meanRmsEvm, out maximumPeakEvm,
                    out meanModulationErrorRatio);
                Console.WriteLine("Mean Frequency Offset(Hz)               : " + meanFrequencyOffset + "\n");
                Console.WriteLine("Mean RMS EVM (%)                        : " + meanRmsEvm + "\n");
                Console.WriteLine("Maximum Peak EVM (%)                    : " + maximumPeakEvm + "\n");
                Console.WriteLine("Mean Modulation Error Ration (dB)       : " + meanModulationErrorRatio + "\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                CloseSession();
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}
