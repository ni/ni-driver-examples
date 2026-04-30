//Steps:
//1. Open a new RFmx session and create a Demod Signal
//2. Configure the basic instrument properties (Clock Source, Clock Frequency) 
//3. Configure Selected Ports
//4. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//5. Select DDemod Measurement
//6. Configure FSK Modulation and M
//7. Configure DDemod FSK Deviation 
//8. Configure DDemod Symbol Rate, Number of Symbols and Pulse Shaping Filter
//9. Configure DDemod Measurement Filter Type as Auto
//10. Configure DDemod Averaging
//11. Initiate Measurement
//12. Read FSK Measurement Results
//13. Dispose Demod Signal and Close the RFmx Session


using System;
using NationalInstruments.RFmx.DemodMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxDemodDDemodFskBasic
{
    public class RFmxDemodDDemodFskBasic
    {
        RFmxDemodMX demod;
        RFmxInstrMX instrSession;

        double meanFskDeviation, meanRmsFskError, maxPeakFskError, timeout = 10.0;



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
                //declare the variables 
                string frequencySource;
                string selectedPorts;
                double frequency, centerFrequency, referenceLevel, externalAttenuation;
                double symbolRate, fskDeviation, pulseShapingFilterParameter;
                int numOfSymbols;
                int averagingCount;

                selectedPorts = "";
                centerFrequency = 1e+9;
                referenceLevel = 0.00;
                externalAttenuation = 0.00;

                frequency = 10.0e+6;
                frequencySource = RFmxInstrMXConstants.OnboardClock;

                fskDeviation = 15.000e+3;
                symbolRate = 100.000e+3;
                numOfSymbols = 1000;

                pulseShapingFilterParameter = 0.50;
                averagingCount = 10;
                //initialise the demod session
                string resourceName = "RFSA";
                instrSession = new RFmxInstrMX(resourceName, "");
                demod = instrSession.GetDemodSignalConfiguration();

                //Configure the session
                instrSession.ConfigureFrequencyReference("", frequencySource, frequency);
                demod.SetSelectedPorts("", selectedPorts);
                demod.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
                demod.SelectMeasurements("", RFmxDemodMXMeasurementTypes.DDemod, true);
                demod.DDemod.Configuration.ConfigureModulationType("", RFmxDemodMXDDemodModulationType.Fsk,
                    RFmxDemodMXDDemodM.M2, RFmxDemodMXDDemodDifferentialEnabled.False);
                demod.DDemod.Configuration.ConfigureFskDeviation("", fskDeviation, RFmxDemodMXDDemodFskReferenceCompensationEnabled.True);
                demod.DDemod.Configuration.ConfigureSymbolRate("", symbolRate);
                demod.DDemod.Configuration.ConfigureNumberOfSymbols("", numOfSymbols);

                demod.DDemod.Configuration.ConfigurePulseShapingFilter("", RFmxDemodMXDDemodPulseShapingFilterType.Gaussian, pulseShapingFilterParameter, 0, 1, null);

                demod.DDemod.Configuration.ConfigureMeasurementFilter("", RFmxDemodMXDDemodMeasurementFilterType.Auto, 0, 1, null);
                demod.DDemod.Configuration.ConfigureAveraging("", RFmxDemodMXDDemodAveragingEnabled.False, averagingCount);
                demod.Initiate("", "");

                //retrieve the results
                demod.DDemod.Results.FetchFskResults("", timeout, out meanFskDeviation, out meanRmsFskError,
                    out maxPeakFskError);
                Console.WriteLine("Mean FSK Deviation(Hz)                : " + meanFskDeviation);
                Console.WriteLine("Mean RMS FSK Error(Hz)                : " + meanRmsFskError);
                Console.WriteLine("Maximum Peak FSK Error (%)            : " + maxPeakFskError);
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
