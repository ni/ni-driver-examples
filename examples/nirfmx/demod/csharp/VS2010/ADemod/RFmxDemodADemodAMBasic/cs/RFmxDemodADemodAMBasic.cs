//Steps:
//1. Open a new RFmxInstrMX session and create a Demod Signal
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure ADemod RBW, Measurement Interval, AM Carrier Suppressed and Averaging
//5. Read ADemod AM Measurement Results
//6. Dispose Demod Signal and Close the RFmxInstrMX Session


using System;
using NationalInstruments.RFmx.DemodMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxDemodADemodAMBasic
{
    public class RFmxDemodADemodAMBasic
    {
        RFmxDemodMX demod;
        RFmxInstrMX instrSession;


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
                double measurementInterval, rbw;

                selectedPorts = "";
                centerFrequency = 1e+9;
                referenceLevel = 0.00;
                externalAttenuation = 0.00;
                measurementInterval = 10.00e-3;
                rbw = 100.00e+3;
                int averagingCount = 10;
                double meanModulationDepth, meanCarrierPower, timeout = 10.0;

                //initialising session
                string resourceName = "RFSA";
                instrSession = new RFmxInstrMX(resourceName, "");
                demod = instrSession.GetDemodSignalConfiguration();
                //Configuring the session
                demod.SetSelectedPorts("", selectedPorts);
                demod.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
                demod.ADemod.Configuration.ConfigureRbwFilter("", rbw, RFmxDemodMXADemodRbwFilterType.Flat, 0.1);
                demod.ADemod.Configuration.ConfigureMeasurementInterval("", measurementInterval);
                demod.ADemod.Configuration.ConfigureAMCarrierSuppressed("", RFmxDemodMXADemodAMCarrierSuppressedEnabled.False);
                demod.ADemod.Configuration.ConfigureAveraging("", RFmxDemodMXADemodAveragingEnabled.False,
                            averagingCount, RFmxDemodMXADemodAveragingType.Linear);

                //retriving results
                demod.ADemod.Results.ReadAM("", timeout, out meanModulationDepth, out meanCarrierPower);
                Console.WriteLine("Mean Modulation Depth (%): " + meanModulationDepth + "\n");
                Console.WriteLine("Mean Carrier Power   (dBm): " + meanCarrierPower + "\n");
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
