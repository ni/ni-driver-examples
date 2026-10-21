/*******************************************************************************
*
* Example program:
*    Acquire IQ in Blocks
*
* Category:
*   NI-RFSA
*
* Description:
*   Use this example to learn how to acquire I/Q data using the RF vector
*	signal analyzer. The example shows how to configure NI-RFSA for finite I/Q
*	acquisition, how to set the carrier frequency and the I/Q rate, and how to
*	fetch I/Q data in blocks. The quadrature data is displayed on the I and Q datagrid.
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
*   5. Configure the Maximum Samples Per Block in the UI.
*
*	6. Select Acquire Button for RFSA to start acquiring the data.
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
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsa;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.AcquireIQinBlocks
{
    public partial class MainForm : Form
    {
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

        private int SamplePerBlock
        {
            get
            {
                return decimal.ToInt32(this.samplesPerBlockNumeric.Value);
            }
        }

        private long SampleRead
        {
            get
            {
                return long.Parse(this.samplesReadTextBox.Text);
            }
        }

        public MainForm()
        {
            InitializeComponent();
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

        private void InitializeRfsaSession()
        {
            CloseSession();
            rfsaSession = new NIRfsa(ResourceName, true, false);
            rfsaSession.DriverOperation.Warning += new System.EventHandler<RfsaWarningEventArgs>(DriverOperationWarning);
        }

        private void DriverOperationWarning(object sender, RfsaWarningEventArgs e)
        {
            MessageBox.Show(e.Warning.ToString(),"Warning");
        }

        private void ConfigureIQ()
        {
            rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ;
            rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel;
            rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFrequency;
            rfsaSession.Configuration.IQ.NumberOfSamples = NumberOfSamples;
            rfsaSession.Configuration.IQ.IQRate = IQRate;
        }

        private void InitiateAcquisition()
        {
            rfsaSession.Acquisition.IQ.Initiate();
        }

        public void FetchIQDataInBlocks()
        {
            long counter = 0;
            while (rfsaSession != null && counter < NumberOfSamples)
            {
                long sampleFetched = FetchIQData(SamplePerBlock);
                counter += sampleFetched;
                samplesReadTextBox.Text = counter.ToString();
                this.Refresh();
            }
        }

        private long FetchIQData(int samplesPerBlock)
        {
            RfsaWaveformInfo wfmInfo;
            ComplexDouble[] newdata;
            PrecisionTimeSpan timespan = new PrecisionTimeSpan(10.0);
            newdata = rfsaSession.Acquisition.IQ.FetchIQSingleRecordComplex<ComplexDouble>(0, samplesPerBlock, timespan, out wfmInfo);
            this.dataGridView1.DataSource = newdata;
            return wfmInfo.ActualSamples;
        }

        private void acquireButton_Click(object sender, System.EventArgs e)
        {
            ChangeControlState(false);            
            try
            {
                InitializeRfsaSession();
                ConfigureIQ();
                InitiateAcquisition();
                FetchIQDataInBlocks();
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

        private void ChangeControlState(bool state)
        {
            acquireButton.Enabled = state;
            resourceNameComboBox.Enabled = state;
            referenceLevelNumeric.Enabled = state;
            carrierFrequencyNumeric.Enabled = state;
            iqRateNumeric.Enabled = state;
            samplesPerBlockNumeric.Enabled = state;
            samplesPerRecordNumeric.Enabled = state;            
        }

        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;           
        }

        public void CloseSession()
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
