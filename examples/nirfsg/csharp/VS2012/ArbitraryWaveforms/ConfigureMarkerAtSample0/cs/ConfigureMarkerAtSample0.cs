//==================================================================================================
// Title        : Configure Marker At Sample 0
// Description  : This example demonstrates how to generate marker at the sample 0 of an 
//                arbitrary waveform. The example generates a double side band waveform.
//==================================================================================================
using System;
using NationalInstruments.ModularInstruments.NIRfsg;

namespace NationalInstruments.Examples.ConfigureMarkerAtSample0
{
    public class ConfigureMarkerAtSample0
    {
        //declare variables
        NIRfsg _rfsgSession;
        string resourceName;
        double frequency, power;
        int numberOfSamples = 100;
        string waveformName = "wfm";
        double[] iData, qData;
        //marker locations expressed in samples
        double[] locations = new double[] { 0 };

        public ConfigureMarkerAtSample0()
        {
            try
            {

                InitializeVariables();

                StartGeneration();

                RfsgGenerationStatus generationStatus = RfsgGenerationStatus.InProgress;

                Console.WriteLine("Press any key to stop generation.");

                do
                {
                    generationStatus = _rfsgSession.CheckGenerationStatus();
                } while ((generationStatus != RfsgGenerationStatus.Complete) && (!Console.KeyAvailable));
            }
            catch (Exception ex)
            {
                ShowError("ConfigureMarkerAtSample0()", ex);
            }
            finally
            {
                StopGeneration();
                Console.WriteLine("Press any key to exit the application.");
                Console.ReadKey();
            }

        }

        #region Initialize Variables Section

        private void InitializeVariables()
        {
            resourceName = "RFSG";
            frequency = 1e9;                    //Hz
            power = -20;                        //dBm
            iData = new double[numberOfSamples];
            qData = new double[numberOfSamples];
            //Generate IQ Data for Double Side Band Waveform
            iData = SinePattern(numberOfSamples, 1.0, 0.0, 1.0);
            qData = SinePattern(numberOfSamples, 1.0, 0.0, 1.0);

        }

        #endregion

        #region Program Functions

        void StartGeneration()
        {

            try
            {
                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the instrument 
                _rfsgSession.RF.Configure(frequency, power);
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform;

                // Write the arb waveform 
                _rfsgSession.Arb.WriteWaveform(waveformName, iData, qData);

                // Set Marker Locations
                _rfsgSession.Arb.Waveforms[waveformName].Markers[0].SetMarkerEventLocations(locations);

                // Initiate Generation 
                _rfsgSession.Initiate();

                //Write to Console
                Console.WriteLine("Started Signal Generation.");

            }
            catch (Exception ex)
            {
                ShowError("StartGeneration()", ex);
            }
        }

        void DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            Console.WriteLine(e.Message);
        }

        static double[] SinePattern(int numberOfSamples, double amplitude, double phaseDegrees, double numberOfCycles)
        {
            double[] sineArray = new double[numberOfSamples];
            for (int i = 0; i < numberOfSamples; i++)
            {
                sineArray[i] = amplitude * Math.Sin(2 * Math.PI * i * numberOfCycles / numberOfSamples + Math.PI * phaseDegrees / 180);
            }
            return sineArray;
        }

        void CheckGeneration()
        {
            try
            {
                // Check the status of the RFSG 
                _rfsgSession.CheckGenerationStatus();
            }
            catch (Exception ex)
            {
                ShowError("CheckGeneration()", ex);
            }
        }

        void StopGeneration()
        {
            try
            {
                if (_rfsgSession != null)
                {
                    // Disable the output.  This sets the noise floor as low as possible.
                    _rfsgSession.RF.OutputEnabled = false;

                    // Unsubscribe from warning events
                    _rfsgSession.DriverOperation.Warning -= DriverOperation_Warning;

                    // Close the NIRfsg session
                    _rfsgSession.Close();
                }
                _rfsgSession = null;
            }
            catch (Exception ex)
            {
                ShowError("StopGeneration()", ex);
            }
        }

        void ShowError(string functionName, Exception exception)
        {
            StopGeneration();
            Console.WriteLine("Error in " + functionName + ": " + exception.Message);
        }

        #endregion

    }
}
