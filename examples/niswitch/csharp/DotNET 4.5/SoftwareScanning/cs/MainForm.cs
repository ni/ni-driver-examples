//==================================================================================================
// Title        : Software Scanning
// Description  : Use this example to learn how to perform scanning operation with a software trigger.The scanning operation is set to a continous mode.Once the start button is clicked the configuration required for scan operation is done. When the next button is clicked a software trigger is sent, the next scan operation in the list is performed.Click the stop button to  abort the scan and to close the switch session.
//==================================================================================================
using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NISwitch;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;


namespace NationalInstruments.Examples.SoftwareScanning
{
    public partial class MainForm : Form
    {
        NISwitch switchSession;
        PrecisionTimeSpan scanDelay = new PrecisionTimeSpan(0);
        bool isNextConnection;
        Thread workerThread;
        volatile bool threadStop;
        bool closeRequested;
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
            LoadSwitchDeviceNames();
            LoadTopology();
            ToggleButtonState(true);
        }
        #region UI Initial Value Config Section
        private void LoadTopology()
        {
            Type myType = typeof(SwitchDeviceTopology);
            PropertyInfo[] properties = myType.GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                topologyNameComboBox.Items.Add(prop.GetValue(myType, null).ToString());
            }
            topologyNameComboBox.SelectedIndex = 0;
        }
        private void LoadSwitchDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-SWITCH");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
            {
                resourceNameComboBox.Items.Add(device.Name);
            }
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
            {
                resourceNameComboBox.SelectedIndex = 0;
            }
        }
        #endregion UI Initial Value Config Section
        #region Program Properties
        private string ResourceName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        private string TopologyName
        {
            get
            {
                return this.topologyNameComboBox.SelectedItem.ToString();
            }
        }
        private string ScanList
        {
            get
            {
                return this.scanListTextBox.Text;
            }
        }

        #endregion Program Properties
        #region Form Events
        private void startScanningButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                CloseSession();

                InitializeSwitchSession();
                ConfigureScanList();
                InitiateScan();
                ToggleButtonState(false);
                StartScan();
            }
            catch (System.Exception ex)
            {
                ShowError(ex.Message);
                CloseSession();
                ChangeControlState(true);
                ToggleButtonState(true);
            }
        }

        private void StartScan()
        {
            // Starts a Thread for Performing Scan, when NextButton is clicked, so that Main UI thread doesn't hangs.
            workerThread = new Thread(SendSoftwareTrigger);
            ThreadStopMarker = false;
            workerThread.Start();
        }

        private void InitiateScan()
        {
            //Initiate scanning.
            switchSession.Scan.Initiate();
        }

        private void ConfigureScanList()
        {
            //Configures the switch module for scanning. 
            switchSession.Scan.ConfigureList(ScanList, SwitchScanMode.BreakBeforeMake);
            //Configures the trigger to be software trigger.
            switchSession.Scan.ConfigureTrigger(scanDelay, SwitchScanTriggerInput.SoftwareTrigger, SwitchScanAdvancedOutput.None);

            //Loop through scan list continuously.
            switchSession.Scan.Continuous = true;
        }
        private void ThreadTerminate()
        {

            //Waits for the thread To Terminate.
            workerThread.Join();
            workerThread = null;
            if (closeRequested == true)
                this.Close();
        }

        private void SendSoftwareTrigger()
        {
            //Send software trigger to switch module when Next Connection button pressed on front panel.
            while (ThreadStopMarker == false)
            {

                if (isNextConnection)
                {
                    try
                    {
                        switchSession.Scan.SendSoftwareTrigger();
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex.Message);
                        this.BeginInvoke(new Action(StopScanning)); 
                        break;
                    }
                    isNextConnection = false;
                }
            }
            this.BeginInvoke(new Action(ThreadTerminate));
        }

        private void nextConnectionButton_Click(object sender, EventArgs e)
        {
            isNextConnection = true;
        }

        private void stopScanningButton_Click(object sender, EventArgs e)
        {

            StopScanning();

        }

        private void StopScanning()
        {
            // Stops the Acquisition.
            if (workerThread != null)
            {
                ThreadStopMarker = true;
            }
            //Abort scanning.
            if (switchSession.Scan.IsScanning)
            {
                switchSession.Scan.Abort();
            }
            ChangeControlState(true);
            ToggleButtonState(true);
            CloseSession();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeRequested = true;
            // Stops the Acquisition.
            if (workerThread != null)
            {
                ThreadStopMarker = true;
            }
            //Abort scanning.
            if (switchSession != null)
            {
                if (switchSession.Scan.IsScanning)
                {
                    switchSession.Scan.Abort();
                }
                CloseSession();
            }
            if (workerThread != null)
                e.Cancel = true;
        }
        #endregion Form Events
        #region Program Functions
        private void ChangeControlState(bool isEnabled)
        {
            this.startScanningButton.Enabled = isEnabled;
            this.nextConnectionButton.Enabled = isEnabled;
            this.stopScanningButton.Enabled = isEnabled;
            this.resourceNameComboBox.Enabled = isEnabled;
            this.topologyNameComboBox.Enabled = isEnabled;
            this.scanListTextBox.Enabled = isEnabled;
        }

        private void ToggleButtonState(bool isEnabled)
        {
            this.startScanningButton.Enabled = isEnabled;
            this.stopScanningButton.Enabled = !isEnabled;
            this.nextConnectionButton.Enabled = !isEnabled;

        }
        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }
        private void InitializeSwitchSession()
        {
            switchSession = new NISwitch(ResourceName, TopologyName, false, true);
            switchSession.DriverOperation.Warning += new System.EventHandler<SwitchWarningEventArgs>(DriverOperationWarning);
        }
        private void DriverOperationWarning(object sender, SwitchWarningEventArgs e)
        {
            MessageBox.Show(e.ToString(), "Warning");
        }
        private void CloseSession()
        {
            if (switchSession != null)
            {
                try
                {
                    switchSession.Close();
                    switchSession = null;
                }
                catch (System.Exception ex)
                {
                    ShowError("Unable to Close Session, Reset the device.\n" + "Error : " + ex.Message);
                    Application.Exit();
                }
            }
        }
        #endregion Program Functions
    }
}
