/*******************************************************************************
*
* Example program:
*   List Mode Timer Triggered
*
* Category:
*   NI-RFSA
*
* Description:
*   Use this example to learn the basics of RF list mode using the RF vector
	signal analyzer. The example shows how to configure the device for RF list
	mode and how to create a configuration list and configuration list steps.
	It uses RF list mode to vary the I/Q carrier frequency and reference level
	across different steps that advance on a hardware timer edge.
                         

* Instructions for running:
*   1. Configure RFSA device in the MAX for the program to run. 
*
*	2. Configure the IQ Rate, Samples Per Record and Number of Steps in the UI.
        The number of steps created in the List are taken from Number of Steps.
*   
*   3. Configure the IQ Carrier Frequency Start ans Stop Values in the UI.
*
*	4. Configure the Reference Level Start ans Stop Values in the UI.
*
*	5. Configure the Timer Event Interval.
*   	
*   6. Select the Acquire Button in the UI to start the acquisition.
*   
*   7. The data is displayed in the DataGrid.
*
* Note: Each Start and Stop Values are partitioned by the number of Steps for each step in Configuration List.  
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
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;
using NationalInstruments.ModularInstruments.NIRfsa;

namespace NationalInstruments.Examples.ListModeTimerTriggered
{
    public partial class MainForm : Form
    {
        NIRfsa rfsaSession;

        public MainForm()
        {
            InitializeComponent();
            LoadRfsaDeviceNames();
        }

        #region UI Initial Value Config Section

        private void LoadRfsaDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-RFSA");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        #endregion

        private void CloseRfsaSession()
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

        private string ResourceName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        private double IQRate
        {
            get
            {
                return decimal.ToDouble(this.iqRateNumeric.Value);
            }
        }

        private double CarrierFreqRmpStart
        {
            get
            {
                return decimal.ToDouble(this.carrierFrequencyRampStrtNumeric.Value);
            }
        }

        private double CarrierFreqRmpStop
        {
            get
            {
                return decimal.ToDouble(this.carrierFrequencyRampStopNumeric.Value);
            }
        }

        private double RefLevelRmpStart
        {
            get
            {
                return decimal.ToDouble(this.referenceLevelRampStrtNumeric.Value);
            }
        }

        private double RefLevelRmpStop
        {
            get
            {
                return decimal.ToDouble(this.referenceLevelRampStopNumeric.Value);
            }
        }

        private double TimerEventInterval
        {
            get
            {
                return decimal.ToDouble(this.timerEventIntervalNumeric.Value);
            }
        }

        private int NumberOfSamples
        {
            get
            {
                return decimal.ToInt32(this.samplesPerRecordNumeric.Value);
            }
        }

        private int NumberOfSteps
        {
            get
            {
                return decimal.ToInt32(this.numberOfStepsNumeric.Value);
            }
        }

        private void InitializeRfsaSession()
        {
            CloseRfsaSession();
            rfsaSession = new NIRfsa(ResourceName, true, false);
            rfsaSession.DriverOperation.Warning += new System.EventHandler<RfsaWarningEventArgs>(DriverOperationWarning);
        }

        private void DriverOperationWarning(object sender, RfsaWarningEventArgs e)
        {
            MessageBox.Show(e.Warning.ToString(), "Warning");
        }

        private void ConfigureForIQ()
        {
            rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ;
            rfsaSession.Configuration.IQ.IQRate = IQRate;
        }

        private void ConfigureForMultiRecord()
        {
            rfsaSession.Configuration.IQ.NumberOfSamples = NumberOfSamples;
            rfsaSession.Configuration.IQ.NumberOfSamplesIsFinite = true;
            rfsaSession.Configuration.IQ.NumberOfRecords = NumberOfSteps;
            rfsaSession.Configuration.IQ.NumberOfRecordsIsFinite = true;
        }

        private void FetchIQData()
        {
            PrecisionTimeSpan timeout = new PrecisionTimeSpan(5.0);
            RfsaWaveformInfo[] wfmInfo = new RfsaWaveformInfo[NumberOfSteps];
            ComplexDouble[,] data;
            data = rfsaSession.Acquisition.IQ.FetchIQMultiRecordComplex<ComplexDouble>(0, NumberOfSteps, NumberOfSamples, timeout, out wfmInfo);
            multiDataGridViewResults.SetData(data);
        }

        private void InitiateAcquisition()
        {
            rfsaSession.Acquisition.IQ.Initiate();
        }

        private void ConfigureListMode()
        {
            RfsaConfigurationListProperties[] propertyList = new RfsaConfigurationListProperties[] { RfsaConfigurationListProperties.IQCarrierFrequency, 
                                                                                                      RfsaConfigurationListProperties.ReferenceLevel};

            double carrierFreqRampIncrement = (CarrierFreqRmpStop - CarrierFreqRmpStart) / NumberOfSteps;
            double refLevelRampIncrement = (RefLevelRmpStop - RefLevelRmpStart) / NumberOfSteps;

            rfsaSession.Configuration.SignalPath.LocalOscillator.FrequencySettlingUnits = RfsaFrequencySettlingUnits.SecondsAfterIO;
            rfsaSession.Configuration.SignalPath.LocalOscillator.FrequencySettlingTime = 0.005;

            string instrumentModel = rfsaSession.Identity.InstrumentModel;

            if (String.Equals(instrumentModel, "NI PXIe-5665 (3.6GHz)", StringComparison.Ordinal) || String.Equals(instrumentModel, "NI PXIe-5665 (14GHz)", StringComparison.Ordinal) ||
                String.Equals(instrumentModel, "NI PXIe-5667 (3.6GHz)", StringComparison.Ordinal) || String.Equals(instrumentModel, "NI PXIe-5667 (7GHz)", StringComparison.Ordinal))
            {
                rfsaSession.Configuration.SignalPath.LocalOscillator.LOYigMainCoilDrive = RfsaLOYigMainCoilDrive.Fast;
            }
            else /* Otherwise, set the Downconverter Loop Bandwidth to Wide */
            {
                rfsaSession.Configuration.SignalPath.LocalOscillator.DownconverterLoopBandwidth = RfsaDownconverterLoopBandwidth.Wide;
            }

            rfsaSession.Configuration.BasicConfigurationList.CreateConfigurationList("exampleList", propertyList, true);

            for (int ii = 0; ii < NumberOfSteps; ii++)
            {
                rfsaSession.Configuration.BasicConfigurationList.CreateStep(true);
                rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFreqRmpStart + (ii * carrierFreqRampIncrement);
                rfsaSession.Configuration.Vertical.ReferenceLevel = RefLevelRmpStart + (ii * refLevelRampIncrement);

            }

        }

        private void ConfigureTimer()
        {
            rfsaSession.Configuration.BasicConfigurationList.TimerEventInterval = TimerEventInterval;
            rfsaSession.Configuration.Triggers.AdvanceTrigger.DigitalEdge.Source = RfsaDigitalEdgeAdvanceTriggerSource.TimerEvent;
        }

        private void acquireButton_Click(object sender, System.EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                InitializeRfsaSession();
                ConfigureForIQ();
                ConfigureForMultiRecord();
                ConfigureListMode();
                ConfigureTimer();
                InitiateAcquisition();
                FetchIQData();
                CloseRfsaSession();
            }
            catch (System.Exception ex)
            {
                ShowError(ex.Message);
                CloseRfsaSession();
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
            this.iqRateNumeric.Enabled = isEnabled;
            this.samplesPerRecordNumeric.Enabled = isEnabled;
            this.numberOfStepsNumeric.Enabled = isEnabled;
            this.carrierFrequencyRampStrtNumeric.Enabled = isEnabled;
            this.carrierFrequencyRampStopNumeric.Enabled = isEnabled;
            this.referenceLevelRampStrtNumeric.Enabled = isEnabled;
            this.referenceLevelRampStopNumeric.Enabled = isEnabled;
            this.timerEventIntervalNumeric.Enabled = isEnabled;
        }

        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }
    }
}