/*******************************************************************************
*
* Example program:
*   Advanced Property Access
*
* Category:
*   NI-RFSA
*
* Description:
*   This example demonstrates how to use the Advanced Property Access Service
*   in NI-RFSA .NET API.
*	The example shows how to configure the following parameters
*	of I/Q acquisition: reference clock, reference level, carrier frequency, I/Q
*	rate, number of samples per record, and I/Q acquisition type.
*	This example also reads and displays data on a dataGrid.
*
* Instructions for running:
*   1. Configure RFSA device in the MAX for the program to run. 
*
*   2. Configure the Reference Clock in the UI.
*
*	3. Configure the Reference Level in the UI. 
*		The reference level represents the maximum expected power of an input RF signal.
*
*	4. Configure the Carrier Frequency in the UI.
*
*	5. Configure the Samples Per Record in the UI and the IQ Rate.
*
*   6. Select the Acquire Button in the UI to start the aqcuisition.
*   
*   7. The data is displayed in the DataGrid.
*  
* I/O Connections Overview:
*   Make sure your signal input terminals match the Physical Channel I/O
*   Controls.  If you have a PXI chassis, ensure that it has been properly
*   identified in MAX.  
*
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments;
using NationalInstruments.ModularInstruments.NIRfsa;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.AdvancedPropertyAccess
{
    public partial class MainForm : Form
    {
        // Attribute values obtained from the C header files: ivi.h, nirfsa.h
        internal static class CDriverAttributeId
        {
            // NIRFSA_ATTR_REF_CLOCK_SOURCE
            internal const long ReferenceClockSource = 1150019;

            // NIRFSA_ATTR_REF_CLOCK_RATE
            internal const long ReferenceClockRate = 1150020;

            // NIRFSA_ATTR_ACQUISITION_TYPE
            internal const long AcquisitionType = 1150001;

            // NIRFSA_ATTR_REFERENCE_LEVEL
            internal const long ReferenceLevel = 1150004;

            // NIRFSA_ATTR_IQ_CARRIER_FREQUENCY
            internal const long IQCarrierFrequency = 1150059;

            // NIRFSA_ATTR_NUMBER_OF_SAMPLES
            internal const long NumberOfSamples = 1150009;

            // NIRFSA_ATTR_NUMBER_OF_SAMPLES_IS_FINITE
            internal const long NumberOfSamplesIsFinite = 1150008;

            // NIRFSA_ATTR_IQ_RATE
            internal const long IQRate = 1150007;
        }

        NIRfsa rfsaSession;

        private string ResourceName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        private double ReferenceLevel
        {
            get
            {
                return decimal.ToDouble(this.referenceLevelNumeric.Value);
            }
        }

        private double CarrierFrequency
        {
            get
            {
                return decimal.ToDouble(this.carrierFrequencyNumeric.Value);
            }
        }

        private long NumberOfSamples
        {
            get
            {
                return decimal.ToInt64(this.samplesPerRecordNumeric.Value);
            }
        }

        private double IQRate
        {
            get
            {
                return decimal.ToDouble(this.iqRateNumeric.Value);
            }
        }

        private string ReferenceClock
        {
            get
            {
                return this.referenceClockComboBox.Text;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            ConfigureRefClockComboBox();
            LoadRfsaDeviceNames();
        }

        private void LoadRfsaDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-RFSA");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        #region UI Initial Value Config Section

        private void ConfigureRefClockComboBox()
        {
            List<KeyValuePair<string, RfsaReferenceClockSource>> refClockValueList = new List<KeyValuePair<string, RfsaReferenceClockSource>>();
            refClockValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("OnboardClock", RfsaReferenceClockSource.OnboardClock));
            refClockValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("RefIn", RfsaReferenceClockSource.ReferenceIn));
            refClockValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("PXI_Clk", RfsaReferenceClockSource.PxiClock));
            refClockValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("ClkIn", RfsaReferenceClockSource.ClockIn));
            referenceClockComboBox.DisplayMember = "Key";
            referenceClockComboBox.ValueMember = "Value";
            referenceClockComboBox.DataSource = refClockValueList;
            referenceClockComboBox.SelectedIndex = 0;
        }

        #endregion

        private void acquireButton_Click(object sender, System.EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                InitializeRfsaSession();
                ConfigureUsingAdvancedPropertyAccessService();
                ReadIQData();
                CloseSession();
            }
            catch (System.Exception ex)
            {
                ShowError(ex.Message);
                CloseSession();
            }
            finally
            {
                ChangeControlState(true);
            }

        }

        private void ChangeControlState(bool isEnabled)
        {
            this.acquireButton.Enabled = isEnabled;
            this.resourceNameComboBox.Enabled = isEnabled;
            this.referenceLevelNumeric.Enabled = isEnabled;
            this.carrierFrequencyNumeric.Enabled = isEnabled;
            this.iqRateNumeric.Enabled = isEnabled;
            this.referenceClockComboBox.Enabled = isEnabled;
            this.samplesPerRecordNumeric.Enabled = isEnabled;
        }

        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }

        private void InitializeRfsaSession()
        {
            CloseSession();
            rfsaSession = new NIRfsa(ResourceName, true, false);
            rfsaSession.DriverOperation.Warning += new System.EventHandler<RfsaWarningEventArgs>(DriverOperationWarning);
        }

        private void DriverOperationWarning(object sender, RfsaWarningEventArgs e)
        {
            MessageBox.Show(e.Warning.ToString(), "Warning");
        }

        private void ConfigureUsingAdvancedPropertyAccessService()
        {
            AdvancedPropertyAccessService rfsaAdvancedPropertyAccessService =
                (AdvancedPropertyAccessService)(rfsaSession as IServiceProvider).GetService(typeof(AdvancedPropertyAccessService));

            // Configure Reference Clock Using Advanced Property Access Service.
            rfsaAdvancedPropertyAccessService.SetAttributeString(CDriverAttributeId.ReferenceClockSource, ReferenceClock);
            rfsaAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.ReferenceClockRate, 10E6);

            // Configure IQ using Advanced Property Access Service.
            rfsaAdvancedPropertyAccessService.SetAttributeInt32(CDriverAttributeId.AcquisitionType, 100); // Value of NIRFSA_VAL_IQ is 100.
            rfsaAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.ReferenceLevel, ReferenceLevel);
            rfsaAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.IQCarrierFrequency, CarrierFrequency);
            rfsaAdvancedPropertyAccessService.SetAttributeInt64(CDriverAttributeId.NumberOfSamples, NumberOfSamples);
            rfsaAdvancedPropertyAccessService.SetAttributeBoolean(CDriverAttributeId.NumberOfSamplesIsFinite, true);
            rfsaAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.IQRate, IQRate);
        }

        private void ReadIQData()
        {
            RfsaWaveformInfo wfmInfo;
            ComplexDouble[] data;
            PrecisionTimeSpan timespan = new PrecisionTimeSpan(10.0);
            data = rfsaSession.Acquisition.IQ.ReadIQSingleRecordComplex(timespan, out wfmInfo);
            this.dataGridViewResults.DataSource = data;
        }

        private void CloseSession()
        {
            if (rfsaSession != null)
            {
                try
                {
                    rfsaSession.Close();
                    rfsaSession = null;
                }
                catch (System.Exception ex)
                {
                    ShowError("Unable to Close Session, Reset the device.\n" + "Error : " + ex.Message);
                    Application.Exit();
                }
            }
        }
    }
}