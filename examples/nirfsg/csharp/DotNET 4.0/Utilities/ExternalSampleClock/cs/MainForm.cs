//==================================================================================================
// Title        : External Sample Clock
// Description  : This example demonstrates how to use an external sample clock
//			to generate an arbitrary waveform. The example lets you choose
//			one of three waveforms to create and write:
//			  + Double Side Band --> two tones around the center frequency.
//			  + Lower Side Band  --> one tone to the left of the center frequency.
//			  + Upper Side Band  --> one tone to the right of the center frequency.
//			Each waveform is 100 samples long. The waveforms are generated at a
//			frequency of IQ Rate / 100 samples. The user sets the IQ rate on the
//			RF Signal Generator and reads out the Arb Sample Clock Rate, which
//			is the rate at which the external clock is generated. The use of
//			the external clock gives the user the flexibility of not having to resample
//			his data as the IQ rate is not coerced.
//==================================================================================================

using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ExternalSampleClock
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSignalGenerator, _rfsgExternalClock;
        const double PowerLevel = 0.0;

        public MainForm()
        {
            InitializeComponent();

            LoadRfsgVectorSignalGeneratorDeviceNames();
            LoadRfsgExternalClockSourceDeviceNames();

            ConfigureWaveformComboBox();
            ConfigureArbSampleClockSourceComboBox();
        }

        private void LoadRfsgVectorSignalGeneratorDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-Rfsg");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                vectorSignalGeneratorComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                vectorSignalGeneratorComboBox.SelectedIndex = 0;
        }

        private void LoadRfsgExternalClockSourceDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-Rfsg");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                externalClockSourceComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                externalClockSourceComboBox.SelectedIndex = 0;
        }

        #region UI Initial Value Config Section

        private void ConfigureWaveformComboBox()
        {
            waveformComboBox.Items.AddRange(new string[] { "DoubleSideBand", "LowerSideBand", "UpperSideBand" });
            waveformComboBox.SelectedIndex = 0;
        }

        private void ConfigureArbSampleClockSourceComboBox()
        {
            var arbSampleClockSourceValueList = new List<DictionaryEntry>();
            arbSampleClockSourceValueList.Add(new DictionaryEntry("ClkIn", RfsgArbSampleClockSource.ClockIn));
            arbSampleClockSourceValueList.Add(new DictionaryEntry("OnboardClock", RfsgArbSampleClockSource.OnboardClock));

            arbSampleClockSourceComboBox.DataSource = arbSampleClockSourceValueList;
            arbSampleClockSourceComboBox.DisplayMember = "Key";
            arbSampleClockSourceComboBox.ValueMember = "Value";
            arbSampleClockSourceComboBox.SelectedValue = RfsgArbSampleClockSource.ClockIn;
        }
        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string vectorSigGenName;
            string externalClockName;
            int kNumberOfSamples = 100;
            double frequency, power, iqRate, arbSampleClockRate;
            int waveform;
            RfsgArbSampleClockSource arbSampleClockSource;
            string sameDeviceError = "The signal generator and external sample clock cannot be the same device.";
            try
            {
                double[] iData = null;
                double[] qData = null;

                // Read in all the control values
                vectorSigGenName = vectorSignalGeneratorComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                iqRate = (double)iqRateNumeric.Value;
                arbSampleClockSource = arbSampleClockSourceComboBox.SelectedValue as RfsgArbSampleClockSource ?? RfsgArbSampleClockSource.FromString(arbSampleClockSourceComboBox.Text);
                waveform = waveformComboBox.SelectedIndex;
                externalClockName = externalClockSourceComboBox.Text;

                if (vectorSigGenName.Equals(externalClockName))
                {
                    errorTextBox.Text = "Error: " + sameDeviceError;
                    return;
                }

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                switch (waveform)
                {
                    case 0:  // Double Side Band
                        iData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0);
                        qData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0);
                        break;

                    case 1:  // Lower Side Band
                        iData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0);
                        qData = SinePattern(kNumberOfSamples, 1.0, 90.0, 1.0);
                        break;

                    case 2:  // Upper Side Band
                        iData = SinePattern(kNumberOfSamples, 1.0, 90.0, 1.0);
                        qData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0);
                        break;
                }

                // Open a session to NI-RFSG.
                _rfsgSignalGenerator = new NIRfsg(vectorSigGenName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSignalGenerator.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(RfsgSigGen_DriverOperation_Warning);

                // Configure the center frequency and output power level.
                _rfsgSignalGenerator.RF.Configure(frequency, power);

                // Configure the generation mode to Arb Waveform.
                _rfsgSignalGenerator.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform;

                // Configure the desired IQ Rate. Set the Arb Sample Clock Source to the desired source.
                _rfsgSignalGenerator.Arb.IQRate = iqRate;
                _rfsgSignalGenerator.ArbSampleClock.Source = arbSampleClockSource;

                // Get the Arb Sample Clock Rate to configure the external clock source. Get the actual IQ Rate.
                arbSampleClockRate = _rfsgSignalGenerator.ArbSampleClock.Rate;
                iqRate = _rfsgSignalGenerator.Arb.IQRate;

                actualArbSampleRateTextBox.Text = arbSampleClockRate.ToString();
                actualIQRateTextBox.Text = iqRate.ToString();
                actualFrequencyOffsetTextBox.Text = (iqRate / kNumberOfSamples).ToString();

                // Open a session to the external sample clock source.
                _rfsgExternalClock = new NIRfsg(externalClockName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgExternalClock.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(RfsgExtClk_DriverOperation_Warning);

                // Configure the power level and the center frequency with the arb sample clock rate acquired earlier.
                _rfsgExternalClock.RF.Configure(arbSampleClockRate, PowerLevel);

                // Initiate generation on the external sample clock source according to programmed settings.
                _rfsgExternalClock.Initiate();

                // Set the signal bandwidth.  Signal bandwidth is 2 * baseband signal's
                //   maximum frequency deviation from 0 Hz.
                _rfsgSignalGenerator.Arb.SignalBandwidth = (iqRate / kNumberOfSamples) * 2;

                // Write the arbitrary waveform.
                _rfsgSignalGenerator.Arb.WriteWaveform(string.Empty, iData, qData);

                // Initiate Generation
                _rfsgSignalGenerator.Initiate();

                // Start the status checking timer
                EnableControls(false);
            }
            catch (Exception ex)
            {
                ShowError("StartGeneration()", ex);
            }
        }

        void RfsgSigGen_DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = "Vector Signal Generator Warning: " + e.Message;
        }

        void RfsgExtClk_DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = "Extrenal Clock Source Warning: " + e.Message;
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
                // Check the status of the RFSG
                _rfsgSignalGenerator.CheckGenerationStatus();
                _rfsgExternalClock.CheckGenerationStatus();
            }
            catch (Exception ex)
            {
                ShowError("CheckGeneration()", ex);
            }
        }

        void StopGeneration()
        {
            EnableControls(true);
            try
            {
                if (_rfsgSignalGenerator != null)
                {
                    _rfsgSignalGenerator.RF.OutputEnabled = false;

                    // Unsubscribe from rfsg warnings
                    _rfsgSignalGenerator.DriverOperation.Warning -= RfsgSigGen_DriverOperation_Warning;

                    _rfsgSignalGenerator.Close();
                }
                _rfsgSignalGenerator = null;

                if (_rfsgExternalClock != null)
                {
                    _rfsgExternalClock.RF.OutputEnabled = false;

                    // Unsubscribe from rfsg warnings
                    _rfsgExternalClock.DriverOperation.Warning -= RfsgExtClk_DriverOperation_Warning;

                    _rfsgExternalClock.Close();
                }
                _rfsgExternalClock = null;
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
            vectorSignalGeneratorComboBox.Enabled = enabled;
            frequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            iqRateNumeric.Enabled = enabled;
            arbSampleClockSourceComboBox.Enabled = enabled;
            externalClockSourceComboBox.Enabled = enabled;
            waveformComboBox.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
