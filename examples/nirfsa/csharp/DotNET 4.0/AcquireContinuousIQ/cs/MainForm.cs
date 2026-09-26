/*******************************************************************************
*
* Example program:
*   Acquire Continuous IQ 
*
* Category:
*   NI-RFSA
*
* Description:
*    Use this example to learn how to acquire I/Q data using the RF vector
*    signal analyzer. The example shows how to configure NI-RFSA for infinite I/Q
*	 acquisition, how to set the carrier frequency and the I/Q rate, and how to
*	 fetch I/Q data continuously.
*
* Instructions for running:
*   1. Configure RFSA device in the MAX for the program to run. 
*
*	2. Configure the Reference Level in the UI. 
*		The reference level represents the maximum expected power of an input RF signal.
*
*	3. Configure the Carrier Frequency in the UI.
*
*	4. COnfigure the IQ Rate and the Samples to Read Per Block in the UI.
*
*   5. Configure the IQ Rate and Samples Per Record in the UI.
*
*	6. Select Start Acquisition Button for RFSA to start acquiring the data.
*	
*   7. This is an example of continuous acquisition and user has to use the Stop Button to stop the
*      the acquisition.
*		
*  Note:This is a multithreaded example in .NET.
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
using System.Threading;

namespace NationalInstruments.Examples.AcquireContinuousIQ
{
    public partial class MainForm : Form
    {
        NIRfsa rfsaSession;
        Thread workerThread;
        long totalNumberOfSamples = 0;
        volatile bool threadStop = false;
        bool closeRequested = false;
        object lockobj = new object();

        private bool ThreadStopMarker
        {
            get
            {
                lock (lockobj)
                {
                    return threadStop;
                }
            }
            set
            {
                lock (lockobj)
                {
                    threadStop = value;
                }
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

        #region Values From the UI

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

        #endregion

        private void InitializeRfsaSession()
        {
            // Close and Initiate a New Rfsa Session.
            CloseSession();
            rfsaSession = new NIRfsa(ResourceName, true, false);
            rfsaSession.DriverOperation.Warning += new EventHandler<RfsaWarningEventArgs>(DriverOperationWarning);
        }

        private void CloseSession()
        {
            //Closes the NiRfsa Session.
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
            MessageBox.Show(message, "Error");
        }

        private void ConfigureIQ()
        {
            // Configure various IQ Property for Acquisition.
            // Number Of Samples is kept Infinite for Continuous Acquisition.
            rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ;
            rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel;
            rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFrequency;
            rfsaSession.Configuration.IQ.NumberOfSamples = NumberOfSamples;
            rfsaSession.Configuration.IQ.NumberOfSamplesIsFinite = false;
            rfsaSession.Configuration.IQ.IQRate = IQRate;
        }

        private void InitiateAcquisition()
        {
            rfsaSession.Acquisition.IQ.Initiate();
        }

        private void ChangeControlState(bool state)
        {
            this.resourceNameComboBox.Enabled = state;
            this.referenceLevelNumeric.Enabled = state;
            this.carrierFrequencyNumeric.Enabled = state;
            this.iqRateNumeric.Enabled = state;
            this.startAcquisitionButton.Enabled = state;
            this.samplesPerRecordNumeric.Enabled = state;
            this.stopButton.Enabled = !state;
        }

        private void DriverOperationWarning(object o, RfsaWarningEventArgs e)
        {
            MessageBox.Show(e.Warning.ToString(), "Warning");
        }

        private void FetchIQ(int samplesPerBlock)
        {
            // Starts a Thread for Performing Fetch so that Main UI thread doesn't hangs.
            ParameterizedThreadStart _parameter1 = new ParameterizedThreadStart(FetchIQData);
            workerThread = new Thread(_parameter1);
            workerThread.Start(samplesPerBlock);
        }

        private void ResetDefaults()
        {
            numberOfSamplesFetchedTextBox.Clear();
            totalNumberOfSamples = 0;
            ThreadStopMarker = false;
        }

        private void startAcquisitionButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Steps:
                //1. Open a new NI-RFSA session.
                //2. Configure the acquisition type to I/Q.
                //3. Configure the reference level.
                //4. Configure the carrier frequency.
                //5. Configure the I/Q rate.
                //6. Configure the NI-RFSA device for a continuous acquisition.
                //7. Initiate the acquisition.
                //8. Fetch I/Q Data.
                //9. Get I/Q components and plot the data.
                //10. Close the NI-RFSA session.
                ResetDefaults();
                ChangeControlState(false);
                InitializeRfsaSession();
                ConfigureIQ();
                InitiateAcquisition();
                FetchIQ(NumberOfSamples);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                CloseSession();
                ChangeControlState(true);
            }

        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            // Stops the Acquisition.
            if (workerThread != null)
            {
                ThreadStopMarker = true;
            }

            ChangeControlState(true);
            CloseSession();
        }

        private void ThreadTerminate()
        {

            //Waits for the thread To Terminate.
            workerThread.Join();
            workerThread = null;
            if (closeRequested == true)
                this.Close();
        }

        private void FetchIQData(object numberOfSamples)
        {
            PrecisionTimeSpan timespan = new PrecisionTimeSpan(10.0);
            while (ThreadStopMarker == false)
            {
                RfsaWaveformInfo wfmInfo;
                ComplexDouble[] newdata = null;
                try
                {
                    rfsaSession.Acquisition.IQ.MemoryOptimizedFetchIQSingleRecordComplex<ComplexDouble>(0, (int)numberOfSamples, timespan, ref newdata, out wfmInfo);
                    numberOfSamplesFetchedTextBox.Invoke(new Action<long>(UpdateData), wfmInfo.ActualSamples);
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                    this.Invoke(new EventHandler(stopButton_Click));
                    break;
                }
            }
            this.BeginInvoke(new Action(ThreadTerminate));
        }

        private void UpdateData(long numberOfSamples)
        {
            totalNumberOfSamples += numberOfSamples;
            numberOfSamplesFetchedTextBox.Text = Convert.ToString(totalNumberOfSamples);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeRequested = true;
            stopButton_Click(null, null);
            if (workerThread != null)
                e.Cancel = true;
        }
    }
}
