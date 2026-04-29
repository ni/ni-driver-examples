//Steps:
//1. Open a new RFmxInstrMX session and create a Demod Signal
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure ADemod RBW, Measurement Interval, FM DeEmphasis and Averaging
//5. Read ADemod FM Measurement Results
//6. Dispose Demod Signal and Close the RFmxInstrMX Session


using System;
using NationalInstruments.RFmx.DemodMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxDemodADemodFMBasic
{
    public class RFmxDemodADemodFMBasic
    {
        RFmxInstrMX instrSession;
        RFmxDemodMX demod;

        void CloseSession()
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
                double deEmphasis, rbw;
                int averagingCount;

                selectedPorts = "";
                centerFrequency = 1e+9;
                referenceLevel = 0.00;
                externalAttenuation = 0.00;
                deEmphasis = 0.0;
                rbw = 100.00e+3;
                averagingCount = 10;
                double meanDeviation, meanCarrierFrequencyError, timeout = 10.0, measurementInterval = 10e-3;

                //initialising a new session
                string resourceName = "RFSA";
                instrSession = new RFmxInstrMX(resourceName, "");
                demod = instrSession.GetDemodSignalConfiguration();
                //Configuring the session
                demod.SetSelectedPorts("", selectedPorts);
                demod.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
                demod.ADemod.Configuration.ConfigureRbwFilter("", rbw, RFmxDemodMXADemodRbwFilterType.Flat, 0.1);
                demod.ADemod.Configuration.ConfigureMeasurementInterval("", measurementInterval);
                demod.ADemod.Configuration.ConfigureFMDeEmphasis("", deEmphasis);

                demod.ADemod.Configuration.ConfigureAveraging("", RFmxDemodMXADemodAveragingEnabled.False,
                    averagingCount, RFmxDemodMXADemodAveragingType.Linear);
                //retriving the results
                demod.ADemod.Results.ReadFM("", timeout, out meanDeviation, out meanCarrierFrequencyError);
                Console.WriteLine("Mean Deviation(deg)               : " + meanDeviation);
                Console.WriteLine("Mean Carrier Frequency Error(Hz)  : " + meanCarrierFrequencyError);
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
