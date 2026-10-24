/*******************************************************************************
*
* Example program:
*    Acquire IQ with Reference Trigger
*
* Category:
*   NI-RFSA
*
* Description:
*   Use this example to learn how to acquire I/Q data using the RF vector
*   signal analyzer and a Reference trigger. This example shows how to
*   configure NI-RFSA for triggered I/Q acquisition, how to set the carrier
*   frequency and I/Q rate, and how to fetch I/Q data in blocks. The quadrature
*   data is displayed on the data grid. This example demonstrates the use
*   of three types of triggering: software, digital edge, and I/Q power edge
*   triggering. The triggerring is always done for the Rising edge.
*   This example also demonstrates how to configure a triggering
*   subsystem to set the number of pretrigger points as well as relevant
*   Reference trigger parameters such as minimum quiet time and trigger level.
*
* Instructions for running:
*   1. Configure RFSA device in the MAX for the program to run. 
*
*	2. Configure the Reference Level in the UI. 
*		The reference level represents the maximum expected power of an input RF signal.
*
*	3. Configure the Carrier Frequency in the UI.
*
*	4. Configure the IQ Rate and the Samples to Read Per Block in the UI.
*
*   5. Configure the Trigger Type for the Acquisition. The triggering happens on the rising edge.
*
*	6. Select start acquisition button to start the acquisition. If the trigger type is Software,
*      Send Software Trigger Button will send the trigger needed for the device.	
*	
*   7. The data is displayed in the DataGrid.
*  
* I/O Connections Overview:
*   Make sure your signal input terminals match the Physical Channel I/O
*   Controls.  If you have a PXI chassis, ensure that it has been properly
*   identified in MAX.  
*
*******************************************************************************/

using System.Collections.Generic;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsa;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.AcquireIQWithReferenceTrigger
{
    public partial class MainForm : Form
    {
        NIRfsa rfsaSession;
        
        public MainForm()
        {
            InitializeComponent();
            ConfigureTriggerTypeComboBox();
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
            var triggerTypeValueList = new List<KeyValuePair<string,RfsaReferenceTriggerType>>();
            triggerTypeValueList.Add(new KeyValuePair<string, RfsaReferenceTriggerType>("None", RfsaReferenceTriggerType.None));
            triggerTypeValueList.Add(new KeyValuePair<string, RfsaReferenceTriggerType>("Digital Edge", RfsaReferenceTriggerType.DigitalEdge));
            triggerTypeValueList.Add(new KeyValuePair<string, RfsaReferenceTriggerType>("IQ Power Edge", RfsaReferenceTriggerType.IQPowerEdge));
            triggerTypeValueList.Add(new KeyValuePair<string, RfsaReferenceTriggerType>("Software", RfsaReferenceTriggerType.SoftwareEdge));
            triggerTypeComboBox.DisplayMember = "Key";
            triggerTypeComboBox.ValueMember = "Value";
            triggerTypeComboBox.DataSource = triggerTypeValueList;            
            triggerTypeComboBox.SelectedIndex = 0;
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

        public RfsaReferenceTriggerType TriggerType
        {
            get
            {
                return (RfsaReferenceTriggerType)this.triggerTypeComboBox.SelectedValue;
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

        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }

        private void ConfigureIQ()
        {
            rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ;
            rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel;
            rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFrequency;
            rfsaSession.Configuration.IQ.NumberOfSamples = NumberOfSamples;
            rfsaSession.Configuration.IQ.IQRate = IQRate;
        }

        private void ConfigureRefTrigger()
        {
            RfsaReferenceTriggerType triggerType = TriggerType;
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

        private void FetchIQdata()
        {
            RfsaWaveformInfo wfmInfo;
            ComplexDouble[] dataPtr;
            PrecisionTimeSpan timespan = new PrecisionTimeSpan(10.0);
            dataPtr = rfsaSession.Acquisition.IQ.FetchIQSingleRecordComplex<ComplexDouble>(0, NumberOfSamples, timespan, out wfmInfo);
            this.dataGridViewResults.DataSource = dataPtr;
        }

        private void InitiateAcquisition()
        {
            rfsaSession.Acquisition.IQ.Initiate();
        }

        private void SendSoftwareTrigger()
        {
            rfsaSession.Configuration.Triggers.ReferenceTrigger.SendSoftwareEdgeTrigger();
        }

        private void triggerTypeComboBox_SelectedValueChanged(object sender, System.EventArgs e)
        {
            if (this.triggerTypeComboBox.SelectedValue!=null)
            {
                RfsaReferenceTriggerType triggerType = (RfsaReferenceTriggerType)this.triggerTypeComboBox.SelectedValue;
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

        private void ChangeControlState(bool isEnabled)
        {
            this.resourceNameComboBox.Enabled = isEnabled;
            this.referenceLevelNumeric.Enabled = isEnabled;
            this.carrierFrequencyNumeric.Enabled= isEnabled;
            this.iqRateNumeric.Enabled= isEnabled;
            this.samplesPerRecordNumeric.Enabled= isEnabled;
            this.triggerTypeComboBox.Enabled = isEnabled;
            this.pretriggerSamplesNumeric.Enabled= isEnabled;
            this.triggerSourceComboBox.Enabled= isEnabled;
            this.triggerLevelNumeric.Enabled= isEnabled;
            this.startAcquisitionButton.Enabled = isEnabled;
        }

        private void startAcquisitionButton_Click(object sender, System.EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                InitializeRfsaSession();
                ConfigureIQ();
                ConfigureRefTrigger();
                InitiateAcquisition();

                if (TriggerType != RfsaReferenceTriggerType.SoftwareEdge)
                {
                    FetchIQdata();
                    CloseSession();
                }
                else
                {
                    this.sendSoftwareTriggerbutton.Enabled = true;
                }
            }
            catch (System.Exception ex)
            {
                ShowError(ex.Message);
                CloseSession();
                ChangeControlState(true);
            }
            finally
            {
                if (TriggerType != RfsaReferenceTriggerType.SoftwareEdge)
                    ChangeControlState(true);
            }

        }

        private void sendSoftwareTriggerbutton_Click(object sender, System.EventArgs e)
        {
            this.sendSoftwareTriggerbutton.Enabled = false;
            SendSoftwareTrigger();
            FetchIQdata();
            CloseSession();
            ChangeControlState(true);
        }        
    }
}