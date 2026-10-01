/*******************************************************************************
*
* Example program:
*    Getting Started Multi Record IQ 
*
* Category:
*   NI-RFSA
*
* Description:
*   Use this example to learn the basics of Multi Record I/Q acquisition using
    the RF vector signal analyzer. The example shows how to configure the following parameters
	of I/Q acquisition: reference clock, reference level, carrier frequency, I/Q
	rate, number of samples per record, number of records, I/Q acquisition type, and reference triggers.
	Multi Record acquisition is non-continuous, so the example includes a triggering setup
	to reinforce this property. This example also reads and display each record's I/Q data on a dataGrid.
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
*   6. Configure the Reference Trigger Type in the UI.
*
*   7. Configure the PreTrigger Samples and the trigger Level in the UI.
*
*   8. Select the Acquire Button in the UI to start the aqcuisition. 
*      If the Reference Trigger is set to Software, press the send Software Trigger Button "Number Of Records" times to perform Fetch operation
*   
*   9. The data is displayed in the DataGrid.
*  
* I/O Connections Overview:
*   Make sure your signal input terminals match the Physical Channel I/O
*   Controls.  If you have a PXI chassis, ensure that it has been properly
*   identified in MAX.  
*
* Known Issues
*   This example may give Design-Time error and warnings if the Designer is opened in Visual Studio before building the project.
*   To view the MainForm Designer after the error has occured, one should close the file, build the project and then open the Designer. 
*
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;
using NationalInstruments.ModularInstruments.NIRfsa;
using NationalInstruments;

namespace NationalInstruments.Examples.GettingStartedMultiRecordIQ
{
    public partial class MainForm : Form
    {
        NIRfsa rfsaSession;        
        int swTriggerCount = 0;

        public MainForm()
        {
            InitializeComponent();
            ConfigureTriggerTypeComboBox();
            ConfigureRefClockComboBox();
            ConfigureTriggerSourceComboBox();

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

        private void ConfigureTriggerTypeComboBox()
        {
            var triggerTypeValueList = new List<KeyValuePair<string, RfsaReferenceTriggerType>>();
            triggerTypeValueList.Add(new KeyValuePair<string, RfsaReferenceTriggerType>("None", RfsaReferenceTriggerType.None));
            triggerTypeValueList.Add(new KeyValuePair<string, RfsaReferenceTriggerType>("Digital Edge", RfsaReferenceTriggerType.DigitalEdge));
            triggerTypeValueList.Add(new KeyValuePair<string, RfsaReferenceTriggerType>("IQ Power Edge", RfsaReferenceTriggerType.IQPowerEdge));
            triggerTypeValueList.Add(new KeyValuePair<string, RfsaReferenceTriggerType>("Software", RfsaReferenceTriggerType.SoftwareEdge));
            referenceTriggerTypeComboBox.DisplayMember = "Key";
            referenceTriggerTypeComboBox.ValueMember = "Value";
            referenceTriggerTypeComboBox.DataSource = triggerTypeValueList;
            referenceTriggerTypeComboBox.SelectedIndex = 0;
        }

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

        private void ConfigureTriggerSourceComboBox()
        {
            var triggerSourceValueList = new List<KeyValuePair<string, RfsaDigitalEdgeReferenceTriggerSource>>();
            triggerSourceValueList.Add(new KeyValuePair<string, RfsaDigitalEdgeReferenceTriggerSource>("PFI0", RfsaDigitalEdgeReferenceTriggerSource.Pfi0));
            triggerSourceValueList.Add(new KeyValuePair<string, RfsaDigitalEdgeReferenceTriggerSource>("PFI1", RfsaDigitalEdgeReferenceTriggerSource.Pfi1));
            triggerSourceValueList.Add(new KeyValuePair<string, RfsaDigitalEdgeReferenceTriggerSource>("PXI_Trig0", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine0));
            triggerSourceValueList.Add(new KeyValuePair<string, RfsaDigitalEdgeReferenceTriggerSource>("PXI_Trig1", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine1));
            triggerSourceValueList.Add(new KeyValuePair<string, RfsaDigitalEdgeReferenceTriggerSource>("PXI_Trig2", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine2));
            triggerSourceValueList.Add(new KeyValuePair<string, RfsaDigitalEdgeReferenceTriggerSource>("PXI_Trig3", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine3));
            triggerSourceValueList.Add(new KeyValuePair<string, RfsaDigitalEdgeReferenceTriggerSource>("PXI_Trig4", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine4));
            triggerSourceValueList.Add(new KeyValuePair<string, RfsaDigitalEdgeReferenceTriggerSource>("PXI_Trig5", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine5));
            triggerSourceValueList.Add(new KeyValuePair<string, RfsaDigitalEdgeReferenceTriggerSource>("PXI_Trig6", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine6));
            triggerSourceValueList.Add(new KeyValuePair<string, RfsaDigitalEdgeReferenceTriggerSource>("PXI_Trig7", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine7));
            triggerSourceValueList.Add(new KeyValuePair<string, RfsaDigitalEdgeReferenceTriggerSource>("PXI_STAR", RfsaDigitalEdgeReferenceTriggerSource.PxiStarLine));
            triggerSourceValueList.Add(new KeyValuePair<string, RfsaDigitalEdgeReferenceTriggerSource>("TimerEvent", RfsaDigitalEdgeReferenceTriggerSource.TimerEvent));
            triggerSourceComboBox.DisplayMember = "Key";
            triggerSourceComboBox.ValueMember = "Value";
            triggerSourceComboBox.DataSource = triggerSourceValueList;
            triggerSourceComboBox.SelectedIndex = 0;
        }

        #endregion

        #region UI Values

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

        private int NumberOfSamples
        {
            get
            {
                return decimal.ToInt32(this.samplesPerRecordNumeric.Value);
            }
        }

        private int NumberOfRecords
        {
            get
            {
                return decimal.ToInt32(this.numberOfRecordsNumeric.Value);
            }
        }

        private double IQRate
        {
            get
            {
                return decimal.ToDouble(this.iqRateNumeric.Value);
            }
        }

        private long PreTriggerSamples
        {
            get
            {
                return decimal.ToInt64(this.pretriggerSamplesNumeric.Value);
            }
        }

        private string ReferenceClock
        {
            get
            {
                return this.referenceClockComboBox.SelectedValue as RfsaReferenceClockSource ?? RfsaReferenceClockSource.FromString(this.referenceClockComboBox.Text);
            }
        }

        private RfsaReferenceTriggerType ReferenceTriggerType
        {
            get
            {
                return (RfsaReferenceTriggerType)this.referenceTriggerTypeComboBox.SelectedValue;
            }
        }

        public RfsaDigitalEdgeReferenceTriggerSource TriggerSource
        {
            get
            {
                return this.triggerSourceComboBox.SelectedValue as RfsaDigitalEdgeReferenceTriggerSource ?? RfsaDigitalEdgeReferenceTriggerSource.FromString(triggerSourceComboBox.Text);
            }
        }

        private double TriggerLevel
        {
            get
            {
                return decimal.ToDouble(this.triggerLevelNumeric.Value);
            }
        }

        #endregion

        private void acquireButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                InitializeRfsaSession();
                ConfigureRefClock();
                ConfigureIQ();
                ConfigureReferenceTrigger();
                InitiateRfsa();

                if (ReferenceTriggerType != RfsaReferenceTriggerType.SoftwareEdge)
                {
                    FetchIQData();
                    CloseSession();
                }
                else
                {
                    swTriggerCount = 0;
                    sendSoftwareTriggerbutton.Enabled = true;
                    sendSoftwareTriggerbutton.Focus();
                }
              
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


        private void FetchIQData()
        {
            RfsaWaveformInfo[] wfmInfo = new RfsaWaveformInfo[NumberOfRecords];
            ComplexDouble[,] data;
            PrecisionTimeSpan timespan = new PrecisionTimeSpan(10.0);
            data = rfsaSession.Acquisition.IQ.FetchIQMultiRecordComplex<ComplexDouble>(0, NumberOfRecords, NumberOfSamples, timespan, out wfmInfo);            
            this.multiDataGridViewResults.SetData(data);
            
        }

        private void InitiateRfsa() 
        {
            rfsaSession.Acquisition.IQ.Initiate();
        }

        private void ConfigureReferenceTrigger()
        {
            RfsaReferenceTriggerType triggerType = ReferenceTriggerType;
            switch (triggerType)
            {
                case RfsaReferenceTriggerType.None:
                    rfsaSession.Configuration.Triggers.ReferenceTrigger.Type = RfsaReferenceTriggerType.None;
                    rfsaSession.Configuration.Triggers.ReferenceTrigger.Disable();
                    break;
                case RfsaReferenceTriggerType.DigitalEdge:
                    rfsaSession.Configuration.Triggers.ReferenceTrigger.Type = RfsaReferenceTriggerType.DigitalEdge;
                    rfsaSession.Configuration.Triggers.ReferenceTrigger.DigitalEdge.Edge = RfsaTriggerEdge.Rising;
                    rfsaSession.Configuration.Triggers.ReferenceTrigger.DigitalEdge.Source = TriggerSource;
                    rfsaSession.Configuration.Triggers.ReferenceTrigger.PreTriggerSamples = PreTriggerSamples;
                    break;
                case RfsaReferenceTriggerType.SoftwareEdge:
                    rfsaSession.Configuration.Triggers.ReferenceTrigger.Type = RfsaReferenceTriggerType.SoftwareEdge;
                    rfsaSession.Configuration.Triggers.ReferenceTrigger.PreTriggerSamples = PreTriggerSamples;
                    break;
                case RfsaReferenceTriggerType.IQPowerEdge:
                    rfsaSession.Configuration.Triggers.ReferenceTrigger.Type = RfsaReferenceTriggerType.IQPowerEdge;
                    rfsaSession.Configuration.Triggers.ReferenceTrigger.IQPowerEdge.Level = TriggerLevel;
                    rfsaSession.Configuration.Triggers.ReferenceTrigger.IQPowerEdge.Source = RfsaIQPowerEdgeReferenceTriggerSource.Zero;
                    rfsaSession.Configuration.Triggers.ReferenceTrigger.IQPowerEdge.Slope = RfsaIQPowerEdgeReferenceTriggerSlope.Rising;
                    rfsaSession.Configuration.Triggers.ReferenceTrigger.PreTriggerSamples = PreTriggerSamples;
                    break;
                default:
                    rfsaSession.Configuration.Triggers.ReferenceTrigger.Disable();
                    break;
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

        private void ConfigureRefClock()
        {
            rfsaSession.Configuration.ReferenceClock.Source = ReferenceClock;
            rfsaSession.Configuration.ReferenceClock.Rate = 10E6;
        }

        private void ConfigureIQ()
        {
            rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ;
            rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel;
            rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFrequency;
            rfsaSession.Configuration.IQ.NumberOfSamples = NumberOfSamples;
            rfsaSession.Configuration.IQ.NumberOfSamplesIsFinite = true;
            rfsaSession.Configuration.IQ.NumberOfRecords = NumberOfRecords;
            rfsaSession.Configuration.IQ.NumberOfRecordsIsFinite = true;
            rfsaSession.Configuration.IQ.IQRate = IQRate;
        }

        private void DriverOperationWarning(object sender, RfsaWarningEventArgs e)
        {
            MessageBox.Show(e.Warning.ToString(), "Warning");
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

        private void sendSoftwareTriggerbutton_Click(object sender, EventArgs e)
        {
            rfsaSession.Configuration.Triggers.ReferenceTrigger.SendSoftwareEdgeTrigger();
            swTriggerCount += 1;
            if (swTriggerCount == NumberOfRecords)
            {
                FetchIQData();
                CloseSession();
                sendSoftwareTriggerbutton.Enabled = false;
            }

        }

        private void referenceTriggerTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.referenceTriggerTypeComboBox.SelectedValue != null)
            {
                RfsaReferenceTriggerType triggerType = (RfsaReferenceTriggerType)this.referenceTriggerTypeComboBox.SelectedValue;

                if (triggerType == RfsaReferenceTriggerType.None)
                {
                    this.triggerSourceLabel.Visible = false;
                    this.triggerSourceComboBox.Visible = false;
                    this.pretriggerSamplesLabel.Visible = false;
                    this.pretriggerSamplesNumeric.Visible = false;
                    this.sendSoftwareTriggerbutton.Visible = false;
                    this.triggerLevelLabel.Visible = false;
                    this.triggerLevelNumeric.Visible = false;
                }
                else if (triggerType == RfsaReferenceTriggerType.SoftwareEdge)
                {
                    this.triggerSourceLabel.Visible = false;
                    this.triggerSourceComboBox.Visible = false;
                    this.pretriggerSamplesLabel.Visible = true;
                    this.pretriggerSamplesNumeric.Visible = true;
                    this.sendSoftwareTriggerbutton.Visible = true;
                    this.sendSoftwareTriggerbutton.Enabled = false;
                    this.triggerLevelLabel.Visible = false;
                    this.triggerLevelNumeric.Visible = false;
                }
                else if (triggerType == RfsaReferenceTriggerType.IQPowerEdge)
                {
                    this.triggerSourceLabel.Visible = false;
                    this.triggerSourceComboBox.Visible = false;
                    this.pretriggerSamplesLabel.Visible = true;
                    this.pretriggerSamplesNumeric.Visible = true;
                    this.sendSoftwareTriggerbutton.Visible = false;
                    this.triggerLevelLabel.Visible = true;
                    this.triggerLevelNumeric.Visible = true;

                }
                else
                {
                    this.triggerSourceLabel.Visible = true;
                    this.triggerSourceComboBox.Visible = true;
                    this.pretriggerSamplesLabel.Visible = true;
                    this.pretriggerSamplesNumeric.Visible = true;
                    this.sendSoftwareTriggerbutton.Visible = false;
                    this.triggerLevelLabel.Visible = false;
                    this.triggerLevelNumeric.Visible = false;
                }
            }
        }


    }

}
