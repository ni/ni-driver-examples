//==================================================================================================
// Title        : Advanced Property Access
// Description  : The application demonstrates how to use the Advanced Property
//			 Access Service.The application configures the reference clock 
//			 and initiates generation.
//==================================================================================================

using System;
using System.Linq;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.AdvancedPropertyAccess
{
    public partial class MainForm : Form
    {
        NIRfsg _sampleRfsgSession;

        // Attribute values obtained from the C header files:ivi.h,ivirfsg.h,_rfsgSession.h
        public enum RfsgCAttributeIdentifier : long
        {
            NIRfsg_Attr_Frequency = 1250001,
            NIRfsg_Attr_PowerLevel = 1250002,
            NIRfsg_Attr_GenerationMode = 1150018,
            NIRfsg_Attr_PowerLevelType = 1150043,
            NIRfsg_Attr_FrequencyReferenceRate = 1250322,
            NIRfsg_Attr_FrequencyReferenceSource = 1150001,
            NIRfsg_Val_ContinuousWave = 1000,
            NIRfsg_Val_ArbitraryWaveform = 1001,
            NIRfsg_Val_Script = 1002,
            NIRfsg_Val_PeakPower = 7001
        };

        public MainForm()
        {
            InitializeComponent();
            LoadRfsgDeviceNames();
            ConfigureGenerationModeComboBox();
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

        private void ConfigureGenerationModeComboBox()
        {
            generationModeComboBox.Items.Add("Continuous");
            generationModeComboBox.Items.Add("Arb");
            generationModeComboBox.Items.Add("Script");
            generationModeComboBox.SelectedIndex = 0;
        }

        private void ConfigureFrequencyReferenceSourceComboBox()
        {
            frequencyReferenceSourceComboBox.Items.Add(RfsgFrequencyReferenceSource.ClockIn);
            frequencyReferenceSourceComboBox.Items.Add(RfsgFrequencyReferenceSource.OnboardClock);
            frequencyReferenceSourceComboBox.Items.Add(RfsgFrequencyReferenceSource.PxiClock);
            frequencyReferenceSourceComboBox.Items.Add(RfsgFrequencyReferenceSource.ReferenceIn);
            frequencyReferenceSourceComboBox.SelectedIndex = 1;
        }

        private void EnableControls(bool enabled)
        {
            generationModeComboBox.Enabled = enabled;
            resourceNameComboBox.Enabled = enabled;
            frequencyReferenceSourceComboBox.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            frequencyNumeric.Enabled = enabled;
            startButton.Enabled = enabled;
            stopButton.Enabled = !enabled;

            Application.DoEvents();
        }

        private void Configure()
        {
            double frequency = (double)frequencyNumeric.Value;
            double frequencyReferenceRate = 10e6;
            int generationMode;
            double powerLevel = (double)powerLevelNumeric.Value;
            string frequencyReferenceSource = frequencyReferenceSourceComboBox.Text;
            int powerLevelType = (int)RfsgCAttributeIdentifier.NIRfsg_Val_PeakPower;

            // Create a Rfsg Session
            _sampleRfsgSession = new NIRfsg(resourceNameComboBox.Text, true, true);

            // Subscribe to Rfsg warnings
            _sampleRfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

            if (generationModeComboBox.SelectedIndex == 0)
                generationMode = (int)RfsgCAttributeIdentifier.NIRfsg_Val_ContinuousWave;
            else if (generationModeComboBox.SelectedIndex == 1)
                generationMode = (int)RfsgCAttributeIdentifier.NIRfsg_Val_ArbitraryWaveform;
            else
                generationMode = (int)RfsgCAttributeIdentifier.NIRfsg_Val_Script;

            // Use advanced property access to set the values into the driver
            IServiceProvider iServiceProviderInterface = _sampleRfsgSession as IServiceProvider;
            AdvancedPropertyAccessService advancedPropertyAccessService = (AdvancedPropertyAccessService)iServiceProviderInterface.GetService(typeof(AdvancedPropertyAccessService));
            // set the Generation Mode
            advancedPropertyAccessService.SetAttributeInt32((long)RfsgCAttributeIdentifier.NIRfsg_Attr_GenerationMode, generationMode);
            // set the Power Level Type
            advancedPropertyAccessService.SetAttributeInt32((long)RfsgCAttributeIdentifier.NIRfsg_Attr_PowerLevelType, powerLevelType);
            // Set the RF Frequency
            advancedPropertyAccessService.SetAttributeDouble((long)RfsgCAttributeIdentifier.NIRfsg_Attr_Frequency, frequency);
            // Set the operation mode
            advancedPropertyAccessService.SetAttributeString((long)RfsgCAttributeIdentifier.NIRfsg_Attr_FrequencyReferenceSource, frequencyReferenceSource);
            // Set the RF Power level
            advancedPropertyAccessService.SetAttributeDouble((long)RfsgCAttributeIdentifier.NIRfsg_Attr_PowerLevel, powerLevel);
            // Set the Reference Clock rate 
            advancedPropertyAccessService.SetAttributeDouble((long)RfsgCAttributeIdentifier.NIRfsg_Attr_FrequencyReferenceRate, frequencyReferenceRate);
        }

        private void StartGeneration()
        {
            EnableControls(false);
            Application.DoEvents();
            try
            {
                errorTextBox.Text = "No Error";
                Configure();

                // Write waveform - this will be generated only in Arb\Script mode
                double[] iData = Enumerable.Repeat<double>(1.0, 32).ToArray();
                double[] qData = Enumerable.Repeat<double>(0.0, 32).ToArray();
                _sampleRfsgSession.Arb.WriteWaveform("waveform1", iData, qData);

                // Write Script for the Script mode
                string script =
                    "script simpleScript" + Environment.NewLine +
                    "   repeat forever" + Environment.NewLine +
                    "       generate waveform1" + Environment.NewLine +
                    "   end repeat" + Environment.NewLine +
                    "end script";

                _sampleRfsgSession.Arb.Scripting.WriteScript(script);

                // Start generating
                _sampleRfsgSession.Initiate();
                // Update the actual values
                actualFrequencyTextBox.Text = String.Format("{0:G8}", _sampleRfsgSession.RF.Frequency);
                actualPowerLevelTextBox.Text = String.Format("{0:G8}", _sampleRfsgSession.RF.PowerLevel);
            }
            catch (Exception exception)
            {
                ShowError("StartGeneration()", exception);
            }
        }

        private void StopGeneration()
        {
            try
            {
                if (_sampleRfsgSession != null)
                {
                    // Unsubscribe from warning events
                    _sampleRfsgSession.DriverOperation.Warning -= DriverOperation_Warning;

                    // Close the NIRfsg session
                    _sampleRfsgSession.Close();
                }
                _sampleRfsgSession = null;
                EnableControls(true);
            }
            catch (Exception ex)
            {
                errorTextBox.Text = "Error in StopGeneration(): " + ex.Message;
            }
        }

        void DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = e.Message;
        }

        private void ShowError(string functionName, Exception exception)
        {
            StopGeneration();
            errorTextBox.Text = "Error in " + functionName + ": " + exception.Message;
        }

        #region Form Events

        private void startButton_Click(object sender, EventArgs e)
        {
            StartGeneration();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            StopGeneration();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopGeneration();
        }

        #endregion
    }
}
