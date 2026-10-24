//==================================================================================================
// Title        : 565x Analog Modulation
// Description  : This example demonstrates the use of 565x family of NI RF Signal Generators.
//			The example shows how to generate a continuous wave signal, and optionally apply
//			analog modulation.
//==================================================================================================

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.AnalogModulation565x
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        const double FrequencyReferenceRate = 10E6;

        public MainForm()
        {
            InitializeComponent();

            LoadRfsgDeviceNames();

            EnableControls(true);

            ConfigureFrequencyReferenceSourceComboBox();
            ConfigureMessageWaveformTypeComboBox();
            ConfigureModulationTypeComboBox();
        }

        void LoadRfsgDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-Rfsg");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        #region UI Initial Value Config Section

        void ConfigureFrequencyReferenceSourceComboBox()
        {
            var refSourceValueList = new List<DictionaryEntry>();
            refSourceValueList.Add(new DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn));
            refSourceValueList.Add(new DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock));
            refSourceValueList.Add(new DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock));
            refSourceValueList.Add(new DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn));

            frequencyReferenceSourceComboBox.DataSource = refSourceValueList;
            frequencyReferenceSourceComboBox.DisplayMember = "Key";
            frequencyReferenceSourceComboBox.ValueMember = "Value";
            frequencyReferenceSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.OnboardClock;
        }

        void ConfigureMessageWaveformTypeComboBox()
        {
            messageWaveformTypeComboBox.Items.AddRange(Enum.GetNames(typeof(RfsgAnalogModulationWaveformType)));
            messageWaveformTypeComboBox.SelectedIndex = 0;
        }

        void ConfigureModulationTypeComboBox()
        {
            modulationTypeComboBox.Items.AddRange(Enum.GetNames(typeof(RfsgAnalogModulationType)));
            modulationTypeComboBox.SelectedIndex = 1;
        }

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            RfsgFrequencyReferenceSource frequencyReferenceSource;
            double centerFrequency;
            double powerLevel;
            RfsgAnalogModulationType modulationType;
            RfsgAnalogModulationWaveformType messageWaveformType;
            double messageWaveformFrequency;
            double fmDeviation;
            double pmDeviation;
            try
            {
                // Read in all the control values
                resourceName = resourceNameComboBox.Text;
                frequencyReferenceSource = frequencyReferenceSourceComboBox.SelectedValue as RfsgFrequencyReferenceSource ?? RfsgFrequencyReferenceSource.FromString(frequencyReferenceSourceComboBox.Text);
                centerFrequency = (double)frequencyNumeric.Value;
                powerLevel = (double)powerLevelNumeric.Value;
                modulationType = (RfsgAnalogModulationType)Enum.Parse(typeof(RfsgAnalogModulationType), (string)modulationTypeComboBox.SelectedItem);
                messageWaveformType = (RfsgAnalogModulationWaveformType)Enum.Parse(typeof(RfsgAnalogModulationWaveformType), (string)messageWaveformTypeComboBox.SelectedItem);
                messageWaveformFrequency = (double)messageFrequencyNumeric.Value;
                fmDeviation = (double)fmDeviationNumeric.Value;
                pmDeviation = (double)pmDeviationNumeric.Value;

                // Update status box
                errorTextBox.Text = "Initializing...";
                Application.DoEvents();
                // Initialize the NIRfsg session
                if (_rfsgSession == null)
                {
                    _rfsgSession = new NIRfsg(resourceName, true, false);

                    // Subscribe to Rfsg warnings
                    _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);
                }

                // Configure the instrument
                _rfsgSession.RF.Configure(centerFrequency, powerLevel);
                _rfsgSession.FrequencyReference.Configure(frequencyReferenceSource, FrequencyReferenceRate);

                // Set the modulation parameters
                // We must set all the parameters regardless of modulation type so that
                // when we change the modulation type on-the-fly, we do not have to also
                // set the other modulation parameters.
                _rfsgSession.Modulation.Analog.ModulationType = modulationType;
                _rfsgSession.Modulation.Analog.WaveformType = messageWaveformType;
                _rfsgSession.Modulation.Analog.WaveformFrequency = messageWaveformFrequency;
                _rfsgSession.Modulation.Analog.FMDeviation = fmDeviation;
                _rfsgSession.Modulation.Analog.PMDeviation = pmDeviation;

                // Initiate Generation
                _rfsgSession.RF.OutputEnabled = true;
                _rfsgSession.Initiate();

                // Successful initialization, turn LED green
                errorTextBox.Text = "Generating.";
                statusLed.BackColor = Color.Lime;

                EnableControls(false);
            }
            catch (Exception ex)
            {
                ShowError("StartGeneration()", ex);
            }
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

        void AbortGeneration()
        {
            try
            {
                // Disable the output.  This sets the noise floor as low as possible.
                _rfsgSession.RF.OutputEnabled = false;
                _rfsgSession.Abort();
                errorTextBox.Text = "Stopped.";
                statusLed.BackColor = System.Drawing.SystemColors.Control;
                EnableControls(true);
            }
            catch (Exception ex)
            {
                ShowError("AbortGeneration()", ex);
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

                    // Close the RFSG session
                    _rfsgSession.Close();
                }
                _rfsgSession = null;

                errorTextBox.Clear();
                statusLed.BackColor = SystemColors.Control;

                EnableControls(true);
            }
            catch (Exception ex)
            {
                errorTextBox.Text = "Error in StopGeneration(): " + ex.Message;
                statusLed.BackColor = Color.Red;
            }
        }

        #endregion

        #region Form Events

        void modulationTypeComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {

                RfsgAnalogModulationType val;

                // Change the visibility of controls based on modulation type
                val = (RfsgAnalogModulationType)Enum.Parse(typeof(RfsgAnalogModulationType), (string)modulationTypeComboBox.SelectedItem);
                switch (val)
                {
                    case RfsgAnalogModulationType.None:
                        messageWaveformTypeLabel.Visible = false;
                        messageWaveformTypeComboBox.Visible = false;
                        messageFrequencyLabel.Visible = false;
                        messageFrequencyNumeric.Visible = false;
                        fmDeviationLabel.Visible = false;
                        fmDeviationNumeric.Visible = false;
                        pmDeviationLabel.Visible = false;
                        pmDeviationNumeric.Visible = false;
                        break;
                    case RfsgAnalogModulationType.FM:
                        messageWaveformTypeLabel.Visible = true;
                        messageWaveformTypeComboBox.Visible = true;
                        messageFrequencyLabel.Visible = true;
                        messageFrequencyNumeric.Visible = true;
                        fmDeviationLabel.Visible = true;
                        fmDeviationNumeric.Visible = true;
                        pmDeviationLabel.Visible = false;
                        pmDeviationNumeric.Visible = false;
                        break;
                    case RfsgAnalogModulationType.PM:
                        messageWaveformTypeLabel.Visible = true;
                        messageWaveformTypeComboBox.Visible = true;
                        messageFrequencyLabel.Visible = true;
                        messageFrequencyNumeric.Visible = true;
                        fmDeviationLabel.Visible = false;
                        fmDeviationNumeric.Visible = false;
                        pmDeviationLabel.Visible = true;
                        pmDeviationNumeric.Visible = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowError("Set Analog Modulation Type", ex);
            }
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopGeneration();
        }

        void generateButton_Click(object sender, System.EventArgs e)
        {
            StartGeneration();
        }

        void stopButton_Click(object sender, EventArgs e)
        {
            StopGeneration();
        }

        void rfsgStatusTimer_Tick(object sender, System.EventArgs e)
        {
            CheckGeneration();
        }

        void DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = e.Message;
        }

        #endregion

        void ShowError(string functionName, Exception exception)
        {
            StopGeneration();
            errorTextBox.Text = "Error in " + functionName + ": " + exception.Message;
            statusLed.BackColor = Color.Red;
        }

        private void EnableControls(bool enabled)
        {
            generateButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            resourceNameComboBox.Enabled = enabled;
            frequencyReferenceSourceComboBox.Enabled = enabled;
            frequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            modulationTypeComboBox.Enabled = enabled;
            messageWaveformTypeComboBox.Enabled = enabled;
            messageFrequencyNumeric.Enabled = enabled;
            fmDeviationNumeric.Enabled = enabled;
            pmDeviationNumeric.Enabled = enabled;

            // Start the status checking timer
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
