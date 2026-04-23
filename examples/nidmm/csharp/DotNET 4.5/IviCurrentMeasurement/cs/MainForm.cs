
//==================================================================================================
// Title        : IVI Current Measurement
// Copyright    : National Instruments 2011. All Rights Reserved.
// Description  : This example demonstrates how to make measurements in an IVI application.
//                An IVI session is created,the session is configured for current measurement 
//                and a reading is acquired and displayed.
// *Note        : Before running this example, make sure that IVI driver session is properly 
//                configured in MAX and a corresponding Logical name (case sensitive) 
//                is assigned to the session.
//===================================================================================================

using System;
using System.Windows.Forms;
using Ivi.Dmm;


namespace NationalInstruments.Examples.IviCurrentMeasurement
{
    public partial class MainForm : Form
    {
        internal Boolean modeACEnabled = false;
        Ivi.Dmm.IIviDmm iviDmmSession;

        public MainForm()
        {
            InitializeComponent();
            LoadPowerlineFrequencyValues();
            LoadMeasurementModes();
            LoadResolutionValues();
        }

        private void LoadPowerlineFrequencyValues()
        {
            double[] powerlineFrequencyValues = { 50, 60 };
            for (int i = 0; i < powerlineFrequencyValues.Length; i++)
                powerlineFrequencyValueComboBox.Items.Add(powerlineFrequencyValues[i]);
            powerlineFrequencyValueComboBox.SelectedIndex = 1;
        }

        private void LoadMeasurementModes()
        {
            measurementModeComboBox.Items.AddRange(Enum.GetNames(typeof(Ivi.Dmm.MeasurementFunction)));
            // DC Current is the default measurement mode
            measurementModeComboBox.SelectedIndex = 2;
        }

        private void LoadResolutionValues()
        {
            double[] resolutionValues = { 3.5, 4.5, 5.5, 6.5, 7.5 };
            for (int i = 0; i < resolutionValues.Length; i++)
                resolutionValueComboBox.Items.Add(resolutionValues[i]);
            resolutionValueComboBox.SelectedIndex = 3;
        }

        private void EnableControls(bool enabled)
        {
            resolutionValueComboBox.Enabled = enabled;
            measurementModeComboBox.Enabled = enabled;
            deviceNameTextBox.Enabled = enabled;
            rangeTextBox.Enabled = enabled;
            readButton.Enabled = enabled;
            powerlineFrequencyValueComboBox.Enabled = enabled;
            if (modeACEnabled == true)
            {
                minACFrequencyTextBox.Enabled = enabled;
                maxACFrequencyTextBox.Enabled = enabled;
            }
        }

        private void UpdateActualRange(IIviDmm sampleDmmSession)
        {
            double actualRange = sampleDmmSession.Range;
            actualRangeTextBox.Text = String.Format("{0:G5}", actualRange);
        }

        private void Configure()
        {
            double range = double.Parse(rangeTextBox.Text);
            double resolution = double.Parse(resolutionValueComboBox.Text);
            double powerlineFrequency = double.Parse(powerlineFrequencyValueComboBox.Text);

            // Get the Measurement Mode from the UI
            Ivi.Dmm.MeasurementFunction measurementMode = Ivi.Dmm.MeasurementFunction.DCCurrent;
            measurementMode = (Ivi.Dmm.MeasurementFunction)Enum.Parse(typeof(Ivi.Dmm.MeasurementFunction), measurementModeComboBox.Text);
            // Configure Dmm session Measurement parameters
            iviDmmSession.Configure(measurementMode, range, resolution);
            // Configure Powerline Frequency
            iviDmmSession.Advanced.PowerlineFrequency = powerlineFrequency;
            // Configure minimum and maximum AC frequency
            if (modeACEnabled == true)
            {
                double minACFrequency = double.Parse(minACFrequencyTextBox.Text);
                double maxACFrequency = double.Parse(maxACFrequencyTextBox.Text);
                iviDmmSession.AC.FrequencyMin = minACFrequency;
                iviDmmSession.AC.FrequencyMax = maxACFrequency;
            }
        }

        private void readButton_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            messageTextBox.Clear();
            Application.DoEvents();
            double reading;
            try
            {
                //Create an Ivi Dmm Session
                iviDmmSession = Ivi.Driver.IviDriver.Create<IIviDmm>(deviceNameTextBox.Text, true, true);
                //Configure session parameters
                Configure();
                //Obtain the reading from the session
                reading = iviDmmSession.Measurement.Read(Ivi.Driver.PrecisionTimeSpan.FromSeconds(2));
                //Update the actual range
                UpdateActualRange(iviDmmSession);
                //Display the reading
                measurementTextBox.Text = String.Format("{0:G8}", reading);
                messageTextBox.Text = "Operation completed successfully.";
            }
            catch (Ivi.Driver.SessionNotFoundException sessionNotFoundException)
            {
                messageTextBox.Text = sessionNotFoundException.Message + " Make sure that Measurement & Automation Explorer "
                    + "is configured correctly with Logical Name (case sensitive) and the corresponding Driver Session.";
            }
            catch (Exception exception)
            {
                messageTextBox.Text = exception.Message;
            }
            finally
            {
                if (iviDmmSession != null)
                    iviDmmSession.Close();
                Application.DoEvents();
                EnableControls(true);
            }
        }

        private void measurementModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (measurementModeComboBox.Text.Contains("AC"))
            {
                acConfigurationGroupBox.Enabled = true;
                modeACEnabled = true;
            }
            else
            {
                acConfigurationGroupBox.Enabled = false;
                modeACEnabled = false;
            }
        }
    }
}
