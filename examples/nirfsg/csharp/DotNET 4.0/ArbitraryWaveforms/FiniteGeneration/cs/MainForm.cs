//==================================================================================================
// Title        : Finite Generation
// Description  : This example demonstrates how to generate a finite waveform. 
//==================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.FiniteGeneration
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;

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

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            double frequency;
            double power;
            int waveformQuantum;
            double waveformRepeatCount;
            double actualIQRate;
            int numberOfSamples;
            float[] iData;
            float[] qData;
            try
            {
                EnableControls(false);

                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                waveformRepeatCount = (double)waveformRepeatCountRateNumeric.Value;

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the instrument 
                _rfsgSession.RF.Configure(frequency, power);
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform;
                _rfsgSession.Arb.IQRate = 50e6;

                // Enable finite generation and set number of times to repeat the waveform.
                _rfsgSession.Arb.IsWaveformRepeatCountFinite = true;
                _rfsgSession.Arb.WaveformRepeatCount = Convert.ToInt32(waveformRepeatCount);

                // Configure the signal bandwidth
                // Set the signal bandwidth to 1Hz since we are outputting a sine tone.
                _rfsgSession.Arb.SignalBandwidth = 1.0;

                // Get the actual (coerced) IQ rate.
                actualIQRate = _rfsgSession.Arb.IQRate;

                // Calculate the number of samples it would take to last 500ms and coerce to
                // the minimum waveform quantum
                waveformQuantum = _rfsgSession.Arb.WaveformCapabilities.WaveformQuantum;
                numberOfSamples = CoerceToQuantum(actualIQRate * 0.5, waveformQuantum);

                // Populate the GUI output for Actual Waveform Duration.
                double waveformDuration = numberOfSamples / actualIQRate;
                waveformDurationTextBox.Text = String.Format("{0:#,0.000}", waveformDuration);

                // Generate a DC signal to be upconverted
                iData = new float[numberOfSamples];
                qData = new float[numberOfSamples];

                for (int i = 0; i < numberOfSamples; i++)
                {
                    iData[i] = 1.0F;
                    qData[i] = 0.0F;
                }

                // Write the waveform, since we aren't using multiple waveforms or selecting
                // it in a script it doesn't need a name.
                _rfsgSession.Arb.WriteWaveform("", iData, qData);

                // Initiate Generation 
                _rfsgSession.Initiate();

                // Start the status checking timer
                rfsgStatusTimer.Enabled = true;

                // Activate stop button 
                stopButton.Focus();
            }
            catch (Exception ex)
            {
                ShowError("StartGeneration()", ex);
            }
        }

        static int CoerceToQuantum(double numberOfSamples, int waveformQuantum)
        {
            double smallestNumberOfSamples;

            if (waveformQuantum <= 0)
                return -1;

            if (numberOfSamples >= waveformQuantum)
                smallestNumberOfSamples = numberOfSamples;
            else
                smallestNumberOfSamples = waveformQuantum;

            return Convert.ToInt32(smallestNumberOfSamples / waveformQuantum) * waveformQuantum;
        }

        void DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = e.Message;
        }

        static double[] SinePattern(int kNumberOfSamples, double amplitude, double phaseDegrees, double numberOfCycles)
        {
            double[] sineArray = new double[kNumberOfSamples];
            for (int i = 0; i < kNumberOfSamples; i++)
            {
                sineArray[i] = amplitude * Math.Sin(2 * Math.PI * i * numberOfCycles / kNumberOfSamples + Math.PI * phaseDegrees / 180);
            }
            return sineArray;
        }

        void CheckGeneration()
        {
            try
            {
                RfsgGenerationStatus generationStatus = _rfsgSession.CheckGenerationStatus();
                if (generationStatus == RfsgGenerationStatus.Complete)
                {
                    StopGeneration();
                }
            }
            catch (Exception ex)
            {
                ShowError("CheckGeneration()", ex);
            }
        }

        void StopGeneration()
        {
            // Stop the status checking timer
            rfsgStatusTimer.Enabled = false;

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
            waveformRepeatCountRateNumeric.Enabled = enabled;

            Application.DoEvents();
        }
    }
}
