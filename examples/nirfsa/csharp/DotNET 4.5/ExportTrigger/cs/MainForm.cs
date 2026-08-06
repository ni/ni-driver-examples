/*******************************************************************************
*
* Example program:
*    Rfsa Export Trigger
*
* Category:
*   NI-RFSA
*
* Description:
*   Use this example to learn how to acquire I/Q data using the RF vector
*   signal analyzer. The example shows how to configure NI-RFSA for finite I/Q
*   acquisition, how to set the carrier frequency and the I/Q rate, and how to
*	fetch I/Q after exporting the Output terminal for the start trigger. 
*	The quadrature data is displayed on the datagrid.
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
*   5. Configure the Output Terminal in the UI.
*
*	6. Select start acquisition button to start the acquisition.
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
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;
using NationalInstruments.ModularInstruments.NIRfsa;

namespace NationalInstruments.Examples.RfsaExportTrigger
{
    public partial class MainForm : Form
    {
        private NIRfsa rfsaSession;

        public MainForm()
        {
            InitializeComponent();
            ConfigureOutputTerminalComboBox();
            LoadRfsaDeviceNames();
        }

        #region InitialCOnfiguration

        private void LoadRfsaDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-RFSA");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        private void ConfigureOutputTerminalComboBox()
        {
            var outputTerminalValueList = new List<KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>>();
            outputTerminalValueList.Add(new KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>("RefOut", RfsaExportStartTriggerExportedOutputTerminal.ReferenceOut));
            outputTerminalValueList.Add(new KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>("RefOut2", RfsaExportStartTriggerExportedOutputTerminal.ReferenceOut2));
            outputTerminalValueList.Add(new KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>("ClkOut", RfsaExportStartTriggerExportedOutputTerminal.ClockOut));
            outputTerminalValueList.Add(new KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>("PFI0", RfsaExportStartTriggerExportedOutputTerminal.Pfi0));
            outputTerminalValueList.Add(new KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>("PFI1", RfsaExportStartTriggerExportedOutputTerminal.Pfi1));
            outputTerminalValueList.Add(new KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>("PXI_Trig0", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine0));
            outputTerminalValueList.Add(new KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>("PXI_Trig1", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine1));
            outputTerminalValueList.Add(new KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>("PXI_Trig2", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine2));
            outputTerminalValueList.Add(new KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>("PXI_Trig3", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine3));
            outputTerminalValueList.Add(new KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>("PXI_Trig4", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine4));
            outputTerminalValueList.Add(new KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>("PXI_Trig5", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine5));
            outputTerminalValueList.Add(new KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>("PXI_Trig6", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine6));
            outputTerminalValueList.Add(new KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>("PXI_Trig7", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine7));
            outputTerminalValueList.Add(new KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>("PXI_STAR", RfsaExportStartTriggerExportedOutputTerminal.PxiStarLine));
            outputTerminalValueList.Add(new KeyValuePair<string, RfsaExportStartTriggerExportedOutputTerminal>("Do_Not_Export", RfsaExportStartTriggerExportedOutputTerminal.DoNotExport));
            outputTerminalComboBox.DisplayMember = "Key";
            outputTerminalComboBox.ValueMember = "Value";
            outputTerminalComboBox.DataSource = outputTerminalValueList;
            outputTerminalComboBox.SelectedIndex = 0;
        }
        #endregion

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

        private RfsaExportStartTriggerExportedOutputTerminal OutputTerminal
        {
            get
            {
                return this.outputTerminalComboBox.SelectedValue as RfsaExportStartTriggerExportedOutputTerminal ?? RfsaExportStartTriggerExportedOutputTerminal.FromString(outputTerminalComboBox.Text);
            }
        }

        #endregion

        private void ChangeControlState(bool isEnabled)
        {
            this.resourceNameComboBox.Enabled = isEnabled;
            this.referenceLevelNumeric.Enabled = isEnabled;
            this.carrierFrequencyNumeric.Enabled = isEnabled;
            this.iqRateNumeric.Enabled = isEnabled;
            this.samplesPerRecordNumeric.Enabled = isEnabled;
            this.startAcquisitionButton.Enabled = isEnabled;
            this.outputTerminalComboBox.Enabled = isEnabled;
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
                catch (Exception ex)
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
            rfsaSession.Configuration.IQ.NumberOfSamplesIsFinite = true;
        }

        private void ConfigureExportSignal()
        {
            rfsaSession.Configuration.Triggers.StartTrigger.Export.OutputTerminal = OutputTerminal;
        }

        private void InitiateAcquisition()
        {
            rfsaSession.Acquisition.IQ.Initiate();
        }

        private void FetchIQdata()
        {
            RfsaWaveformInfo wfmInfo;
            ComplexDouble[] dataPtr;
            PrecisionTimeSpan timespan = new PrecisionTimeSpan(10.0);
            dataPtr = rfsaSession.Acquisition.IQ.FetchIQSingleRecordComplex<ComplexDouble>(0, NumberOfSamples, timespan, out wfmInfo);
            this.dataGridViewResults.DataSource = dataPtr;
        }

        private void startAcquisitionButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                // Steps:
                //1. Open a new NI-RFSA session.
                //2. Configure the acquisition type to I/Q.
                //3. Configure the reference level.
                //4. Configure the carrier frequency.
                //5. Configure the I/Q rate.
                //6. Configure the number of samples per record.
                //7. Export the trigger to the output terminal.
                //8. Read the I/Q data.
                //9. Get I/Q components and plot the data.
                //10. Close the NI-RFSA session.
                InitializeRfsaSession();
                ConfigureIQ();
                ConfigureExportSignal();
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
    }
}