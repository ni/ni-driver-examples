//Steps:
//1. Open a new RFmxInstrMX session and create a Demod Signal
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)//
//4. Select MSK Modulation and Differential Enabled
//5. Configure DDemod Symbol Rate, Number of Symbols and Pulse Shaping Filter
//6. Configure DDemod Measurement Filter Type as Auto
//7. Configure DDemod Averaging
//8. Read DDemod Measurement Results
//9. Dispose Demod Signal and Close the RFmxInstrMX Session


using System;
using NationalInstruments.RFmx.DemodMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxDemodDDemodMskBasic
{
    public class RFmxDemodDDemodMskBasic
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
                string resourceName = "RFSA", selectorString = "";
                string selectedPorts = "";
                double centerFrequency = 1e+9;			/* Hz */
                double referenceLevel = 0.00;			/* dBm */
                double externalAttenuation = 0.00;		/* dB */

                double timeout = 10.0;					/* seconds */

                double symbolRate = 100.000e+3;
                int numOfSymbols = 1000;

                /* Pulse shaping filter */
                double pulseShapingFilterParameter = 0.50;

                /* Averaging */

                int averagingCount = 10;

                /* Variables to store the results */
                double meanFrequencyOffet = 0;
                double meanRmsEvm = 0;
                double maxPeakEvm = 0;
                double meanModulationErrorRatio = 0;

                /* Create a new RFmx Session */
                instrSession = new RFmxInstrMX(resourceName, "");
                demod = instrSession.GetDemodSignalConfiguration();
                /* Configure DDemod parameters */
                demod.SetSelectedPorts("", selectedPorts);
                demod.ConfigureRF(selectorString, centerFrequency, referenceLevel, externalAttenuation);
                demod.DDemod.Configuration.ConfigureModulationType(selectorString, RFmxDemodMXDDemodModulationType.Msk,
                    RFmxDemodMXDDemodM.M4, RFmxDemodMXDDemodDifferentialEnabled.False);
                demod.DDemod.Configuration.ConfigureSymbolRate(selectorString, symbolRate);
                demod.DDemod.Configuration.ConfigureNumberOfSymbols(selectorString, numOfSymbols);
                demod.DDemod.Configuration.ConfigurePulseShapingFilter(selectorString, RFmxDemodMXDDemodPulseShapingFilterType.Gaussian,
                    pulseShapingFilterParameter, 0, 1, null);
                demod.DDemod.Configuration.ConfigureMeasurementFilter(selectorString, RFmxDemodMXDDemodMeasurementFilterType.Auto,
                    0, 1, null);
                demod.DDemod.Configuration.ConfigureAveraging(selectorString, RFmxDemodMXDDemodAveragingEnabled.False, averagingCount);


                /* Retrieve results */
                demod.DDemod.Results.Read(selectorString, timeout, out meanFrequencyOffet, out meanRmsEvm,
                    out maxPeakEvm, out meanModulationErrorRatio);

                /* Display results */
                Console.WriteLine("Mean Frequency Offset (Hz)         : " + meanFrequencyOffet);
                Console.WriteLine("Mean RMS EVM(%)                    : " + meanRmsEvm);
                Console.WriteLine("Maximum Peak EVM(%)                : " + maxPeakEvm);
                Console.WriteLine("Mean Modulation Error Ratio(dB)    : " + meanModulationErrorRatio);

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
