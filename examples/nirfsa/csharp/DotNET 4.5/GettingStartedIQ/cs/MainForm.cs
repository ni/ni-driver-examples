/*******************************************************************************
*
* Example program:
*   Getting Started IQ
*
* Category:
*   NI-RFSA
*
* Description:
*   Use this example to learn the basics of I/Q acquisition using the RF vector signal analyzer. 
	The example shows how to configure the following parameters
	of I/Q acquisition: reference clock, reference level, carrier frequency, I/Q
	rate, number of samples per record, and I/Q acquisition type. This example
	also reads and displays data on a dataGrid.
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
using System.Collections.Generic;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsa;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.GettingStartedIQ
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

        private RfsaReferenceClockSource ReferenceCLock
        {
            get
            {
                return this.referenceClockComboBox.SelectedValue as RfsaReferenceClockSource ?? RfsaReferenceClockSource.FromString(this.referenceClockComboBox.Text);
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
                ConfigureRefClock();
                ConfigureIQ();
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

        private void ConfigureRefClock()
        {
            rfsaSession.Configuration.ReferenceClock.Source = ReferenceCLock;
            rfsaSession.Configuration.ReferenceClock.Rate = 10E6;
        }

        private void ConfigureIQ()
        {
            rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ;
            rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel;
            rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFrequency;
            rfsaSession.Configuration.IQ.NumberOfSamples = NumberOfSamples;
            rfsaSession.Configuration.IQ.NumberOfSamplesIsFinite = true;
            rfsaSession.Configuration.IQ.IQRate = IQRate;
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