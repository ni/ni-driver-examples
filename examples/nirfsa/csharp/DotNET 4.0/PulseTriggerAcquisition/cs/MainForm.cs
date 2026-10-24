/*******************************************************************************
*
* Example program:
*    Pulse Trigger Acquisition
*
* Category:
*   NI-RFSA
*
* Description:
*   Use this example to learn how to acquire I/Q data using the RF vector signal analyzer.
    The example shows how to configure NI-RFSA for finite I/Q acquisition, how to set the carrier frequency and the I/Q rate,
    and how to fetch I/Q data on a IQ Power Edge Reference Trigger. The quadrature data is displayed on the datagrid.
                         

* Instructions for running:
*   1. Configure RFSA device in the MAX for the program to run. 
*
*	2. Configure the Reference Level, Carrier Frequency and IQ Rate in the UI.
*   
*   3. Configure the Trigger Setting in Trigger Slope and Trigger Level.
*
*	4. Configure the Burst Length, Reference Position and Minimum Quiet Time.
*
*   5. Select the Start Button in the UI to start the acquisition.
*   
*   6. The data is displayed in the DataGrid.
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
using NationalInstruments.ModularInstruments.NIRfsa;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.PulseTriggerAcquisition
{
    public partial class MainForm : Form
    {
        private NIRfsa rfsaSession;
        private double actualCoercedIQRate;
        private int numberOfSamples;

        public MainForm()
        {
            InitializeComponent();
            LoadRfsaDeviceNames();
            ConfigureTriggerSlopeComboBox();
        }

        private void LoadRfsaDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-RFSA");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        private void ConfigureTriggerSlopeComboBox()
        {
            var triggerSlopeValueList = new List<KeyValuePair<string, RfsaIQPowerEdgeReferenceTriggerSlope>>();
            triggerSlopeValueList.Add(new KeyValuePair<string, RfsaIQPowerEdgeReferenceTriggerSlope>("Falling", RfsaIQPowerEdgeReferenceTriggerSlope.Falling));
            triggerSlopeValueList.Add(new KeyValuePair<string, RfsaIQPowerEdgeReferenceTriggerSlope>("Rising", RfsaIQPowerEdgeReferenceTriggerSlope.Rising));
            triggerSlopeComboBox.DisplayMember = "Key";
            triggerSlopeComboBox.ValueMember = "Value";
            triggerSlopeComboBox.DataSource = triggerSlopeValueList;
            triggerSlopeComboBox.SelectedIndex = 0;
        }

        #region UI Gets

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

        private double IQRate
        {
            get
            {
                return decimal.ToDouble(this.iqRateNumeric.Value);
            }
        }

        private double TriggerLevel
        {
            get
            {
                return decimal.ToDouble(this.triggerLevelNumeric.Value);
            }
        }

        private double BurstLength
        {
            get
            {
                return decimal.ToDouble(this.burstLengthNumeric.Value);
            }
        }

        private double ReferencePosition
        {
            get
            {
                return decimal.ToDouble(this.referencePositionNumeric.Value);
            }
        }

        private double MinimumQuiteTime
        {
            get
            {
                return decimal.ToDouble(this.minimumQuiteTimeNumeric.Value);
            }
        }

        private RfsaIQPowerEdgeReferenceTriggerSlope TriggerSlope
        {
            get
            {
                return (RfsaIQPowerEdgeReferenceTriggerSlope)this.triggerSlopeComboBox.SelectedValue;
            }
        }

        #endregion

        private void InitializeRfsaSession()
        {

            //Open a new NI-RFSA session.
            CloseSession();
            rfsaSession = new NIRfsa(ResourceName, true, false);
            rfsaSession.DriverOperation.Warning += new System.EventHandler<RfsaWarningEventArgs>(DriverOperationWarning);
        }

        private void DriverOperationWarning(object sender, RfsaWarningEventArgs e)
        {
            MessageBox.Show(e.Warning.ToString(),"Warning");
        }

        private void CloseSession()
        {
            // Close the NI-RFSA session.
            if (rfsaSession != null)
            {
                try
                {
                    rfsaSession.Close();
                    rfsaSession = null;
                }
                catch (Exception ex)
                {
                    ShowError("Unable to Close Session, Reset the device.\n" + "Error : " + ex.Message);
                    Application.Exit();
                }
            }
        }

        private void ConfigureIQ()
        {
            // Set the acquisition type to I/Q.
            // Configure the reference level.
            // Configure the carrier frequency.
            // Configure the I/Q rate.
            rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ;
            rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel;
            rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFrequency;
            rfsaSession.Configuration.IQ.IQRate = IQRate;
        }

        private void ConfigureNumberOfSamples()
        {
            //  Read the actual coerced I/Q rate for the calculations.
            //  Configure the samples per record (set to the burst length * IQ rate).
            actualCoercedIQRate = rfsaSession.Configuration.IQ.IQRate;
            numberOfSamples = (int)(actualCoercedIQRate * BurstLength);
            rfsaSession.Configuration.IQ.NumberOfSamples = numberOfSamples;
            rfsaSession.Configuration.IQ.NumberOfSamplesIsFinite = true;
        }

        private void ConfigureIQPowerEdgeTrigger()
        {
            // Configure the I/Q power edge Reference trigger.
            // Configure the minimum quiet time.
            int pretriggerSamples = (int)(numberOfSamples * (ReferencePosition / 100));

            rfsaSession.Configuration.Triggers.ReferenceTrigger.IQPowerEdge.Level = TriggerLevel;
            rfsaSession.Configuration.Triggers.ReferenceTrigger.IQPowerEdge.Slope = TriggerSlope;
            rfsaSession.Configuration.Triggers.ReferenceTrigger.IQPowerEdge.Source = RfsaIQPowerEdgeReferenceTriggerSource.Zero;
            rfsaSession.Configuration.Triggers.ReferenceTrigger.Type = RfsaReferenceTriggerType.IQPowerEdge;
            rfsaSession.Configuration.Triggers.ReferenceTrigger.PreTriggerSamples = pretriggerSamples;
            rfsaSession.Configuration.Triggers.ReferenceTrigger.MinimumQuietTime = MinimumQuiteTime;

        }

        private void InitiateAcquisition()
        {
            // Initiate the acquisition.
            rfsaSession.Acquisition.IQ.Initiate();
        }
        
        private void FetchIQdata()
        {
            // Fetch the I/Q Data.
            // Get the I/Q components and plot the data.
            ComplexDouble[] dataPtr;
            PrecisionTimeSpan timespan = new PrecisionTimeSpan(10.0);
            dataPtr = rfsaSession.Acquisition.IQ.FetchIQSingleRecordComplex<ComplexDouble>(0, numberOfSamples, timespan);
            this.dataGridViewResults.DataSource = dataPtr;
            this.Refresh();
        }

        private void ChangeControlState(bool isEnabled)
        {
            this.resourceNameComboBox.Enabled = isEnabled;
            this.referenceLevelNumeric.Enabled = isEnabled;
            this.carrierFrequencyNumeric.Enabled = isEnabled;
            this.iqRateNumeric.Enabled = isEnabled;
            this.triggerLevelNumeric.Enabled = isEnabled;
            this.burstLengthNumeric.Enabled = isEnabled;
            this.referencePositionNumeric.Enabled = isEnabled;
            this.minimumQuiteTimeNumeric.Enabled = isEnabled;
            this.triggerSlopeComboBox.Enabled = isEnabled;
            this.startButton.Enabled = isEnabled;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeControlState(false);
                InitializeRfsaSession();
                ConfigureIQ();
                ConfigureNumberOfSamples();
                ConfigureIQPowerEdgeTrigger();
                InitiateAcquisition();
                FetchIQdata();
                CloseSession();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                CloseSession();
            }
            finally
            {
                ChangeControlState(true);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }
    }
}
