//==================================================================================================
// Title        : Pulsed Data
// Description  : This example demonstrates how to use scripts to generate pulsed data.  Pulsed
//			data is typically used in time division multiplexing (TDM) systems.  TDM is a 
//			type of multiplexing where two or more channels of information are transmitted 
//			over the same link by allocating a different time interval ("slot") for the 
//			transmission of each channel.  The set of all slots makes up a frame.  This 
//			example generates a CW inside one of the time slots. 
//
//			Note: In order to run this example, the upconverter must be configured with 
//			an Arbitrary Waveform Generator. 
//			To do this, open Measurement & Automation Explorer, select the upconverter 
//			and click on properties.
//==================================================================================================

using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.PulsedData
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        const double ArbSignalBandwidth = 1;
        const double ArbPreFilterGain = -2;
        public MainForm()
        {
            InitializeComponent();

            LoadRfsgDeviceNames();

        }

        private void LoadRfsgDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-Rfsg");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        #region UI Initial Value Config Section

        
        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            double frequency;
            double power;
            double framePeriod;
            int timeSlots;
            double iqRate;
            string script;
            double actualIQRate;
            int waveformQuantum;
            RfsgRFPowerLevelType powerLevelType;
            int waveformSize;
            double[] offData;
            double[] iData;
            double[] qData;
            double actualTimeSlotPeriod;
            int waveformItr;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                framePeriod = (double)framePeriodNumeric.Value;
                timeSlots = (int)timeSlotsNumeric.Value;
                iqRate = (double)iqRateNumeric.Value;
                powerLevelType = RfsgRFPowerLevelType.PeakPower;

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);
                
                // Configure the instrument 
                _rfsgSession.RF.Configure(frequency, power);

                // Configure the power level type 
                _rfsgSession.RF.PowerLevelType = powerLevelType;

                // Configure the generation mode to Script 
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;

                // Configure the IQ rate of the waveforms 
                _rfsgSession.Arb.IQRate = iqRate;

                // Configure the signal bandwidth 
                _rfsgSession.Arb.SignalBandwidth = ArbSignalBandwidth;

                // Configure Pre-filter Gain to avoid overflow due to phase-
                //  discontinuous signals 
                _rfsgSession.Arb.PreFilterGain = ArbPreFilterGain;

                //  Disable phase continuity since we do not expect to maintain a
                //   phase continuous signal in a pulsed data scenario 
                _rfsgSession.Arb.PhaseContinuityEnabled = RfsgPhaseContinuityEnabled.Disabled;


                // Get the actual IQ rate, and populate the GUI output 
                actualIQRate = _rfsgSession.Arb.IQRate;
                actualIQRateTextBox.Text = actualIQRate.ToString();

                // Get the waveform quantum in order to generate an aligned waveform 
                waveformQuantum = _rfsgSession.Arb.WaveformCapabilities.WaveformQuantum;

                // Configure the script with the timeSlots -
                script =
                @"script generationWithPulsedData 
                    repeat forever                
                        repeat " + (timeSlots - 1).ToString() + @"
                            generate offTime      
                        end repeat                
                        generate carrier          
                    end repeat                    
                end script";

                waveformSize = RfsgCreatePulsedData(
                    actualIQRate,
                    framePeriod,
                    timeSlots,
                    waveformQuantum);

                // Initialize waveforms -
                offData = new double[waveformSize];
                iData = new double[waveformSize];
                qData = new double[waveformSize];
                for (waveformItr = 0; waveformItr < waveformSize; waveformItr++)
                {
                    offData[waveformItr] = 0.0;
                    iData[waveformItr] = 1.0;
                    qData[waveformItr] = 0.0;
                }

                // Populate the display -
                actualTimeSlotPeriod = waveformSize / iqRate;
                actualTimeSlotPeriodTextBox.Text = actualTimeSlotPeriod.ToString();
                actualFramePeriodTextBox.Text = (actualTimeSlotPeriod * timeSlots).ToString();


                // Write the two waveforms 
                _rfsgSession.Arb.WriteWaveform("offTime", offData, offData);
                _rfsgSession.Arb.WriteWaveform("carrier", iData, qData);

                // Write the script 
                _rfsgSession.Arb.Scripting.WriteScript(script);

                // Initiate Generation 
                _rfsgSession.Initiate();

                // Start the status checking timer 
                EnableControls(false);
                stopButton.Focus();
            }
            catch (Exception ex)
            {
                ShowError("StartGeneration()", ex);
            }
        }

        void DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = e.Message;
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
            // Stop the status checking timer 
            EnableControls(true);
            try
            {
                if (_rfsgSession != null)
                {
                    // Disable the output.  This sets the noise floor as low as possible.
                    _rfsgSession.RF.OutputEnabled = false;

                    // Unsubscribe from warning events
                    _rfsgSession.DriverOperation.Warning -= DriverOperation_Warning;

                    // Close the RFSG NIRfsg session
                    _rfsgSession.Close();
                }
                _rfsgSession = null;
            }
            catch (Exception ex)
            {
                errorTextBox.Text = "Error in StopGeneration(): " + ex.Message;
            }
        }

        void ShowError(string functionName, Exception exception)
        {
            StopGeneration();
            errorTextBox.Text = "Error in " + functionName + ": " + exception.Message;
        }

        static int RfsgCoerceToQuantum(int numberOfSamples, int quantum)
        {
            double smallestNumberOfSamples;

            if (quantum <= 0)
            {
                return -1;
            }

            if (numberOfSamples >= quantum)
                smallestNumberOfSamples = numberOfSamples;
            else
                smallestNumberOfSamples = quantum;

            return (int)Math.Round(smallestNumberOfSamples / quantum) * quantum;
        }

        static int RfsgCreatePulsedData(double iqRate, double framePeriod, int timeSlots, int waveformQuantum)
        {
            int kMinWaveformSize = 16;
            int theoreticalWaveformSize;

            try
            {
                // Determine waveform size
                theoreticalWaveformSize = (int)(framePeriod * iqRate / timeSlots);

                if (theoreticalWaveformSize < kMinWaveformSize)
                {
                    theoreticalWaveformSize = kMinWaveformSize;
                }

                return RfsgCoerceToQuantum(theoreticalWaveformSize, waveformQuantum);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Form Events
        private void startButton_Click(object sender, System.EventArgs e)
        {
            StartGeneration();
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            StopGeneration();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopGeneration();
        }

        private void rfsgStatusTimer_Tick(object sender, System.EventArgs e)
        {
            CheckGeneration();
        }
        #endregion

        private void EnableControls(bool enabled)
        {
            startButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            resourceNameComboBox.Enabled = enabled;
            frequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            framePeriodNumeric.Enabled = enabled;
            timeSlotsNumeric.Enabled = enabled;
            iqRateNumeric.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
