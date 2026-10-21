//==================================================================================================
// Title        : 565x Digital Modulation
// Description  : This example demonstrates the use of 565x family of NI RF Signal Generators.
//			The example shows how to generate a continuous wave signal, and optionally apply
//			digital modulation.
//==================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.DigitalModulation565x
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        string[] _userDefinedWaveformString;
        byte[] _userDefinedWaveformBytes;
        const double FrequencyReferenceRate = 10E6;

        public MainForm()
        {
            InitializeComponent();

            LoadRfsgDeviceNames();

            EnableControls(true);

            ConfigureMessageWaveformTypeComboBox();
            ConfigureModulationTypeComboBox();
            ConfigureFrequencyReferenceSourceComboBox();
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


        private void ConfigureMessageWaveformTypeComboBox()
        {

            messageWaveformTypeComboBox.Items.AddRange(Enum.GetNames(typeof(RfsgDigitalModulationWaveformType)));
            messageWaveformTypeComboBox.SelectedIndex = 0;
        }

        private void ConfigureModulationTypeComboBox()
        {
            modulationTypeComboBox.SelectedIndexChanged -= modulationTypeComboBox_SelectedIndexChanged;
            modulationTypeComboBox.Items.AddRange(Enum.GetNames(typeof(RfsgDigitalModulationType)));
            modulationTypeComboBox.SelectedIndexChanged += new EventHandler(modulationTypeComboBox_SelectedIndexChanged);
            modulationTypeComboBox.SelectedIndex = 1;
        }

        private void ConfigureFrequencyReferenceSourceComboBox()
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

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            string frequencyReferenceSource = string.Empty;
            double centerFrequency;
            double powerLevel;
            RfsgDigitalModulationType modulationType;
            RfsgDigitalModulationWaveformType messageWaveformType;
            double messageSymbolRate;
            int prbsOrder;
            double fskDeviation;
            try
            {

                // Read in all the control values
                resourceName = resourceNameComboBox.Text;
                frequencyReferenceSource = (string)frequencyReferenceSourceComboBox.Text;
                centerFrequency = (double)frequencyNumeric.Value;
                powerLevel = (double)powerLevelNumeric.Value;
                modulationType = (RfsgDigitalModulationType)Enum.Parse(typeof(RfsgDigitalModulationType), (string)modulationTypeComboBox.SelectedItem);
                messageWaveformType = (RfsgDigitalModulationWaveformType)Enum.Parse(typeof(RfsgDigitalModulationWaveformType), (string)messageWaveformTypeComboBox.SelectedItem);
                messageSymbolRate = (double)messageSymbolRateNumeric.Value;
                prbsOrder = (int)messagePrbsOrderNumeric.Value;
                fskDeviation = (double)fskDeviationNumeric.Value;

                // Convert the user-defined waveform string into an array of integers.
                _userDefinedWaveformString = messageUserDefinedWaveformTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                _userDefinedWaveformBytes = new byte[_userDefinedWaveformString.Length];
                for (int i = 0; i < _userDefinedWaveformString.Length; i++)
                {
                    _userDefinedWaveformBytes[i] = byte.Parse(_userDefinedWaveformString[i]);
                }

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
                _rfsgSession.Modulation.Digital.ModulationType = modulationType;
                _rfsgSession.Modulation.Digital.WaveformType = messageWaveformType;
                _rfsgSession.Modulation.Digital.SymbolRate = messageSymbolRate;
                _rfsgSession.Modulation.Digital.PrbsOrder = prbsOrder;
                _rfsgSession.Modulation.Digital.PrbsSeed = 1;
                _rfsgSession.Modulation.Digital.ConfigureUserDefinedWaveform(_userDefinedWaveformBytes);
                _rfsgSession.Modulation.Digital.FskDeviation = fskDeviation;

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
                statusLed.BackColor = SystemColors.Control;

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

        void DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = e.Message;
        }

        #region Form Events

        private void modulationTypeComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            RfsgDigitalModulationType val;
            val = (RfsgDigitalModulationType)Enum.Parse(typeof(RfsgDigitalModulationType), (string)modulationTypeComboBox.SelectedItem);
            switch (val)
            {
                case RfsgDigitalModulationType.None:
                    messageWaveformTypeLabel.Visible = false;
                    messageWaveformTypeComboBox.Visible = false;
                    messageSymbolRateLabel.Visible = false;
                    messageSymbolRateNumeric.Visible = false;
                    messagePrbsOrderLabel.Visible = false;
                    messagePrbsOrderNumeric.Visible = false;
                    messageUserDefinedWaveformLabel.Visible = false;
                    messageUserDefinedWaveformTextBox.Visible = false;
                    fskDeviationLabel.Visible = false;
                    fskDeviationNumeric.Visible = false;
                    break;
                case RfsgDigitalModulationType.Fsk:
                    messageWaveformTypeLabel.Visible = true;
                    messageWaveformTypeComboBox.Visible = true;
                    messageSymbolRateLabel.Visible = true;
                    messageSymbolRateNumeric.Visible = true;
                    messagePrbsOrderLabel.Visible = true;
                    messagePrbsOrderNumeric.Visible = true;
                    messageUserDefinedWaveformLabel.Visible = true;
                    messageUserDefinedWaveformTextBox.Visible = true;
                    fskDeviationLabel.Visible = true;
                    fskDeviationNumeric.Visible = true;
                    break;
                case RfsgDigitalModulationType.Ook:
                case RfsgDigitalModulationType.Psk:
                    messageWaveformTypeLabel.Visible = true;
                    messageWaveformTypeComboBox.Visible = true;
                    messageSymbolRateLabel.Visible = true;
                    messageSymbolRateNumeric.Visible = true;
                    messagePrbsOrderLabel.Visible = true;
                    messagePrbsOrderNumeric.Visible = true;
                    messageUserDefinedWaveformLabel.Visible = true;
                    messageUserDefinedWaveformTextBox.Visible = true;
                    fskDeviationLabel.Visible = false;
                    fskDeviationNumeric.Visible = false;
                    break;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopGeneration();
        }

        private void generateButton_Click(object sender, System.EventArgs e)
        {
            StartGeneration();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            StopGeneration();
        }

        private void rfsgStatusTimer_Tick(object sender, System.EventArgs e)
        {
            CheckGeneration();
        }

        private void messageUserDefinedWaveformTextBox_TextChanged(object sender, EventArgs e)
        {
            // Clear off any previous errors.
            errorTextBox.Clear();
            generateButton.Enabled = true;
            statusLed.BackColor = SystemColors.Control;

            // Check if the user is entering valid bytes data.
            // Convert the user-defined waveform string into an array of integers.
            _userDefinedWaveformString = messageUserDefinedWaveformTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                _userDefinedWaveformBytes = new byte[_userDefinedWaveformString.Length];
                for (int i = 0; i < _userDefinedWaveformString.Length; i++)
                {
                    if (!byte.TryParse(_userDefinedWaveformString[i], out _userDefinedWaveformBytes[i]))
                    {
                        ShowError("Entering Waveform values", new ArgumentException("Input string not in correct format"));
                        generateButton.Enabled = false;
                        break;
                    }
                }
            }
            catch
            {
                ShowError("Entering Waveform values", new ArgumentException("Input string not in correct format"));
                generateButton.Enabled = false;
            }
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
            rfsgStatusTimer.Enabled = !enabled;
            resourceNameComboBox.Enabled = enabled;
            messageUserDefinedWaveformTextBox.Enabled = enabled;
            frequencyReferenceSourceComboBox.Enabled = enabled;
            frequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            modulationTypeComboBox.Enabled = enabled;
            messageWaveformTypeComboBox.Enabled = enabled;
            messageSymbolRateNumeric.Enabled = enabled;
            messagePrbsOrderNumeric.Enabled = enabled;
            fskDeviationNumeric.Enabled = enabled;

            Application.DoEvents();
        }
    }
}
