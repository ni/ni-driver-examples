
//==================================================================================================
// Title        : Advanced Property Access
// Copyright    : National Instruments 2011. All Rights Reserved.
// Description  : This application demonstrates how to use the Advanced Property Access Service.
//                This application configures the DMM for Capacitance/Inductance measurement,
//                acquires a reading and displays the aquired reading to the user.
//===================================================================================================

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments;
using NationalInstruments.ModularInstruments.NIDmm;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;


namespace NationalInstruments.Examples.AdvancedPropertyAccess
{
    public partial class MainForm : Form
    {
        NIDmm sampleDmmSession;

        //Attribute values obtained from the C header files: ivi.h, ividmm.h, nidmm.h
        internal enum DmmCAttributeIdentifier : long
        {
            NIDMM_ATTR_RANGE = 1250002,
            NIDMM_ATTR_FUNCTION = 1250001,
            NIDMM_ATTR_RESOLUTION_DIGITS = 1250003,
            NIDMM_ATTR_NUMBER_OF_AVERAGES = 1150055,
            NIDMM_VAL_CAPACITANCE = 1005,
            NIDMM_VAL_INDUCTANCE = 1006,
            NIDMM_ATTR_OPERATION_MODE = 1150014,
            NIDMM_VAL_IVIDMM_MODE = 0,
        };

        public MainForm()
        {
            InitializeComponent();
            LoadDmmDeviceNames();
            LoadMeasurementModes();
        }

        private void LoadDmmDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-DMM");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        private void LoadMeasurementModes()
        {
            measurementModeComboBox.Items.Add("Capacitance");
            measurementModeComboBox.Items.Add("Inductance");
            measurementModeComboBox.SelectedIndex = 0;
        }

        private void EnableControls(bool enabled)
        {
            measurementModeComboBox.Enabled = enabled;
            resourceNameComboBox.Enabled = enabled;
            rangeTextBox.Enabled = enabled;
            measurementsToAverageNumericUpDown.Enabled = enabled;
            readButton.Enabled = enabled;
        }

        private void UpdateActualRange(NIDmm sampleDmmSession)
        {
            double actualRange = sampleDmmSession.Range;
            actualRangeTextBox.Text = String.Format("{0:G5}", actualRange);
        }

        private void Configure()
        {
            double range = double.Parse(rangeTextBox.Text);
            double resolution = 6.5;
            long measurementMode;
            int measurementsToAverage = (int)measurementsToAverageNumericUpDown.Value;

            //Create a Dmm Session
            sampleDmmSession = new NIDmm(resourceNameComboBox.Text, true, true);

            if (measurementModeComboBox.Text.CompareTo("Capacitance") == 0)
                measurementMode = (long)DmmCAttributeIdentifier.NIDMM_VAL_CAPACITANCE;
            else
                measurementMode = (long)DmmCAttributeIdentifier.NIDMM_VAL_INDUCTANCE;

            //Use advanced property access to get the measurements to average value
            IServiceProvider iServiceProviderInterface = sampleDmmSession as IServiceProvider;
            AdvancedPropertyAccessService advancedPropertyAccessService = (AdvancedPropertyAccessService)iServiceProviderInterface.GetService(typeof(AdvancedPropertyAccessService));
            //set the Measurement Mode
            advancedPropertyAccessService.SetAttributeInteger((long)DmmCAttributeIdentifier.NIDMM_ATTR_FUNCTION, measurementMode);
            //Set the Range
            advancedPropertyAccessService.SetAttributeDouble((long)DmmCAttributeIdentifier.NIDMM_ATTR_RANGE, range);
            //Set the operation mode
            advancedPropertyAccessService.SetAttributeInteger((long)DmmCAttributeIdentifier.NIDMM_ATTR_OPERATION_MODE, (long)DmmCAttributeIdentifier.NIDMM_VAL_IVIDMM_MODE);
            //Set the number of Averages
            advancedPropertyAccessService.SetAttributeInteger((long)DmmCAttributeIdentifier.NIDMM_ATTR_NUMBER_OF_AVERAGES, measurementsToAverage);
            //Set the Resolution
            advancedPropertyAccessService.SetAttributeDouble((long)DmmCAttributeIdentifier.NIDMM_ATTR_RESOLUTION_DIGITS, resolution);
        }

        private void readButton_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            messageTextBox.Clear();
            Application.DoEvents();
            try
            {
                double reading;
                int measurementsToAverage = (int)measurementsToAverageNumericUpDown.Value;
                Configure();
                // Obtain the reading from the session
                reading = sampleDmmSession.Measurement.Read();
                // Update the actual range
                UpdateActualRange(sampleDmmSession);
                // Display the reading
                measurementTextBox.Text = String.Format("{0:G8}", reading);
                messageTextBox.Text = "Operation completed successfully.";
            }
            catch (Exception exception)
            {
                messageTextBox.Text = exception.Message;
            }
            finally
            {
                if (sampleDmmSession != null)
                    sampleDmmSession.Close();
                Application.DoEvents();
                EnableControls(true);
            }
        }
    }
}