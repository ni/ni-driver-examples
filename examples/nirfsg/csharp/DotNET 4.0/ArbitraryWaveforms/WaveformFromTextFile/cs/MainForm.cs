//==================================================================================================
// Title        : Waveform From File
// Description  : This example demonstrates how to generate a phase-continuous Chirp signal
//			using the Arbitrary Waveform capabilities of NI-RFSG.
//			Note: In order to run this example, the upconverter must be configured with 
//			an Arbitrary Waveform Generator. 
//			To do this, open Measurement & Automation Explorer, select the upconverter 
//			and click on properties.
//==================================================================================================

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.WaveformFromTextFile
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        const string WaveformName = "waveform";

        public MainForm()
        {
            InitializeComponent();

            LoadRfsgDeviceNames();

            ConfigurePowerLevelTypeComboBox();
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

        private void ConfigurePowerLevelTypeComboBox()
        {
            powerLevelTypeComboBox.Items.AddRange(Enum.GetNames(typeof(RfsgRFPowerLevelType)));
            powerLevelTypeComboBox.SelectedIndex = 1;
        }

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            try
            {
                string resourceName;
                string fileName;
                double frequency;
                double frequencyOffset;
                double power;
                double iqRate;
                double actualIQRate;
                double preFilterGain;
                double signalBandwidth;
                bool directDownload;
                RfsgRFPowerLevelType powerLevelType;
                double[] iData;
                double[] qData;


                // We are starting - set GUI ctrls. 
                generatingLed.BackColor = SystemColors.Control;
                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                iqRate = (double)iqRateNumeric.Value;
                directDownload = directDownloadCheckBox.Checked;
                preFilterGain = (double)preGainNumeric.Value;
                powerLevelType = (RfsgRFPowerLevelType)Enum.Parse(typeof(RfsgRFPowerLevelType), (string)powerLevelTypeComboBox.SelectedItem);
                fileName = pathTextBox.Text;
                signalBandwidth = (double)signalBandwidthNumeric.Value;

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the instrument 
                _rfsgSession.Arb.IQRate = iqRate;

                _rfsgSession.RF.PowerLevelType = powerLevelType;

                _rfsgSession.Arb.PreFilterGain = preFilterGain;

                _rfsgSession.Arb.DataTransfer.DirectDownloadEnabled = directDownload;

                _rfsgSession.RF.Configure(frequency, power);

                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform;

                // Get the actual IQ rate, and populate the GUI output 
                actualIQRate = _rfsgSession.Arb.IQRate;

                // Configure the signal bandwidth 
                _rfsgSession.Arb.SignalBandwidth = signalBandwidth;

                // Read the content of a text file into I and Q arrays
                string file = File.ReadAllText(fileName);
                string[] iqData = file.Split(null as string[], StringSplitOptions.RemoveEmptyEntries);

                int numberOfSamples = iqData.Length / 2;
                iData = new double[numberOfSamples];
                qData = new double[numberOfSamples];
                for (int i = 0; i < numberOfSamples; i++)
                {
                    iData[i] = double.Parse(iqData[i * 2]);
                    qData[i] = double.Parse(iqData[i * 2 + 1]);
                }

                _rfsgSession.Arb.WriteWaveform(WaveformName, iData, qData);

                actualTotalSamplesTextBox.Text = numberOfSamples.ToString();

                frequencyOffset = actualIQRate / numberOfSamples;

                actualIQRateTextBox.Text = actualIQRate.ToString();
                actualFrequencyOffsetTextBox.Text = frequencyOffset.ToString();

                // Initiate Generation 
                _rfsgSession.Initiate();

                generatingLed.BackColor = Color.Lime;

                // Activate stop button 
                stopButton.Focus();

                // Start the status checking timer 
                EnableControls(false);
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
            // Activate all stopped controls 
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

            generatingLed.BackColor = SystemColors.Control;
        }

        void ShowError(string functionName, Exception exception)
        {
            StopGeneration();
            errorTextBox.Text = "Error in " + functionName + ": " + exception.Message;
        }

        #endregion

        #region Form Events
        private void browseButton_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";
            openFileDialog.Title = "Select a waveform file...";
            openFileDialog.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(pathTextBox.Text));
            openFileDialog.FileName = Path.GetFileName(pathTextBox.Text);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pathTextBox.Text = openFileDialog.FileName;
            }
            openFileDialog.Dispose();
        }

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
            directDownloadCheckBox.Enabled = enabled;
            pathTextBox.Enabled = enabled;
            frequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            iqRateNumeric.Enabled = enabled;
            preGainNumeric.Enabled = enabled;
            powerLevelTypeComboBox.Enabled = enabled;
            signalBandwidthNumeric.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
