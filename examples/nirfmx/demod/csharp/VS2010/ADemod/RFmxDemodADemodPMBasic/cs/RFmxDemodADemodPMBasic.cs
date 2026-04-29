//Steps:
//1. Open a new RFmx session and create a Demod Signal
//2. Configure Selected Ports
//3. Configure the basic signal properties  (Center Frequency, Reference Level and External Attenuation)
//4. Configure ADemod RBW, RBW Filter Type, Measurement Interval 
//5. Configure ADemod Carrier Frequency and Carrier Phase Correction as True
//6. Configure ADemod Averaging 
//7. Read ADemod PM Measurement Results
//8. Dispose Demod Signal and Close the RFmx Session

using System;
using NationalInstruments.RFmx.DemodMX;
using NationalInstruments.RFmx.InstrMX;

namespace NationalInstruments.Examples.RFmxDemodADemodPMBasic
{
    public class RFmxDemodADemodPMBasic
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
                //declaring all variables
                string selectedPorts;
                double centerFrequency, referenceLevel, externalAttenuation;
                double measurementInterval, rbw;
                int averagingCount;

                selectedPorts = "";
                centerFrequency = 1e+9;
                referenceLevel = 0.00;
                externalAttenuation = 0.00;
                measurementInterval = 10.00e-3;
                rbw = 100.00e+3;
                averagingCount = 10;
                double meanDeviation, meanCarrierFrequencyError, timeout = 10.0;
                //initialising demod session
                string resourceName = "RFSA";
                instrSession = new RFmxInstrMX(resourceName, "");
                demod = instrSession.GetDemodSignalConfiguration();
                //configuring the session
                demod.SetSelectedPorts("", selectedPorts);
                demod.ConfigureRF("", centerFrequency, referenceLevel, externalAttenuation);
                demod.ADemod.Configuration.ConfigureRbwFilter("", rbw, RFmxDemodMXADemodRbwFilterType.Flat, 0.1);
                demod.ADemod.Configuration.ConfigureMeasurementInterval("", measurementInterval);
                demod.ADemod.Configuration.ConfigureCarrierCorrection("", RFmxDemodMXADemodCarrierFrequencyCorrectionEnabled.True,
                                        RFmxDemodMXADemodCarrierPhaseCorrectionEnabled.True);
                demod.ADemod.Configuration.ConfigureAveraging("", RFmxDemodMXADemodAveragingEnabled.False,
                    averagingCount, RFmxDemodMXADemodAveragingType.Linear);

                
                //retriving the results
                demod.ADemod.Results.ReadPM("", timeout, out meanDeviation, out meanCarrierFrequencyError);
                Console.WriteLine("Mean Deviation(deg)               : " + meanDeviation + "\n");
                Console.WriteLine("Mean Carrier Frequency Error(Hz)  : " + meanCarrierFrequencyError + "\n");
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
