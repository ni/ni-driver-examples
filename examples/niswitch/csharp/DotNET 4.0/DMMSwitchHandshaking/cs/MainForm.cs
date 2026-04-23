//==================================================================================================
// Title        : DMM Switch Handshaking
// Description  : Use this example to use NI-Switch and Ni-Dmm together. The example, first configures scanning operation for switch, then measurement configurations for Dmm is done, Finally, the Dmm device takes the reading after the switch scan is performed.
//================================================================================================== 


using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using NationalInstruments.ModularInstruments.NIDmm;
using NationalInstruments.ModularInstruments.NISwitch;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;


namespace NationalInstruments.Examples.DmmSwitchHandshaking
{
    public partial class MainForm : Form
    {
        NIDmm dmmSession;
        NISwitch switchSession;

        string dmmInstrumentModel;
        Thread workerThread;
        volatile bool threadStop;
        bool closeRequested;
        object lockobj = new object();
        int acquisitionBacklog;
        double[] measurement;
        public MainForm()
        {
            InitializeComponent();
            LoadSwitchDeviceNames();
            LoadTopology();
            LoadDmmDeviceNames();
            LoadMeasurementModes();
            LoadSwitchTriggerInput();
            LoadScanAdvancedOutput();
            LoadDmmTriggerSource();
            LoadDmmMeasCompleteDest();
        }

        #region UI Initial Value Config Section
        private void LoadDmmMeasCompleteDest()
        {
            var dmmMeasCompleteDestList = new List<KeyValuePair<string, DmmMeasurementCompleteDestination>>();
            dmmMeasCompleteDestList.Add(new KeyValuePair<string, DmmMeasurementCompleteDestination>("None", DmmMeasurementCompleteDestination.None));
            dmmMeasCompleteDestList.Add(new KeyValuePair<string, DmmMeasurementCompleteDestination>("External", DmmMeasurementCompleteDestination.External));
            dmmMeasCompleteDestList.Add(new KeyValuePair<string, DmmMeasurementCompleteDestination>("TTL 0", DmmMeasurementCompleteDestination.Ttl0));
            dmmMeasCompleteDestList.Add(new KeyValuePair<string, DmmMeasurementCompleteDestination>("TTL 1", DmmMeasurementCompleteDestination.Ttl1));
            dmmMeasCompleteDestList.Add(new KeyValuePair<string, DmmMeasurementCompleteDestination>("TTL 2", DmmMeasurementCompleteDestination.Ttl2));
            dmmMeasCompleteDestList.Add(new KeyValuePair<string, DmmMeasurementCompleteDestination>("TTL 3", DmmMeasurementCompleteDestination.Ttl3));
            dmmMeasCompleteDestList.Add(new KeyValuePair<string, DmmMeasurementCompleteDestination>("TTL 4", DmmMeasurementCompleteDestination.Ttl4));
            dmmMeasCompleteDestList.Add(new KeyValuePair<string, DmmMeasurementCompleteDestination>("TTL 5", DmmMeasurementCompleteDestination.Ttl5));
            dmmMeasCompleteDestList.Add(new KeyValuePair<string, DmmMeasurementCompleteDestination>("TTL 6", DmmMeasurementCompleteDestination.Ttl6));
            dmmMeasCompleteDestList.Add(new KeyValuePair<string, DmmMeasurementCompleteDestination>("TTL 7", DmmMeasurementCompleteDestination.Ttl7));
            dmmMeasCompleteDestList.Add(new KeyValuePair<string, DmmMeasurementCompleteDestination>("LBR Trig 0 7", DmmMeasurementCompleteDestination.LbrTrig0));

            measCompleteDestComboBox.DisplayMember = "Key";
            measCompleteDestComboBox.ValueMember = "Value";
            measCompleteDestComboBox.DataSource = dmmMeasCompleteDestList;
            measCompleteDestComboBox.SelectedIndex = 1;
        }
        private void LoadDmmTriggerSource()
        {
            var triggerSourceValueList = new List<KeyValuePair<string, DmmTriggerSource>>();
            triggerSourceValueList.Add(new KeyValuePair<string, DmmTriggerSource>("Immediate", DmmTriggerSource.Immediate));
            triggerSourceValueList.Add(new KeyValuePair<string, DmmTriggerSource>("External", DmmTriggerSource.External));
            triggerSourceValueList.Add(new KeyValuePair<string, DmmTriggerSource>("TTL 0", DmmTriggerSource.Ttl0));
            triggerSourceValueList.Add(new KeyValuePair<string, DmmTriggerSource>("TTL 1", DmmTriggerSource.Ttl1));
            triggerSourceValueList.Add(new KeyValuePair<string, DmmTriggerSource>("TTL 2", DmmTriggerSource.Ttl2));
            triggerSourceValueList.Add(new KeyValuePair<string, DmmTriggerSource>("TTL 3", DmmTriggerSource.Ttl3));
            triggerSourceValueList.Add(new KeyValuePair<string, DmmTriggerSource>("TTL 4", DmmTriggerSource.Ttl4));
            triggerSourceValueList.Add(new KeyValuePair<string, DmmTriggerSource>("TTL 5", DmmTriggerSource.Ttl5));
            triggerSourceValueList.Add(new KeyValuePair<string, DmmTriggerSource>("TTL 6", DmmTriggerSource.Ttl6));
            triggerSourceValueList.Add(new KeyValuePair<string, DmmTriggerSource>("Software", DmmTriggerSource.SoftwareTrigger));
            triggerSourceValueList.Add(new KeyValuePair<string, DmmTriggerSource>("TTL 7", DmmTriggerSource.Ttl7));
            triggerSourceValueList.Add(new KeyValuePair<string, DmmTriggerSource>("PXI Star", DmmTriggerSource.PxiStar));
            triggerSourceValueList.Add(new KeyValuePair<string, DmmTriggerSource>("LBR Trig 1", DmmTriggerSource.LbrTrig1));
            triggerSourceValueList.Add(new KeyValuePair<string, DmmTriggerSource>("Aux Trig 1", DmmTriggerSource.LbrTrig1));

            triggerSourceComboBox.DisplayMember = "Key";
            triggerSourceComboBox.ValueMember = "Value";
            triggerSourceComboBox.DataSource = triggerSourceValueList;
            triggerSourceComboBox.SelectedIndex = 1;
        }
        private void LoadSwitchTriggerInput()
        {
            Type myType = typeof(SwitchScanTriggerInput);
            PropertyInfo[] properties = myType.GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                switchTriggerInputComboBox.Items.Add(prop.GetValue(myType, null).ToString());
            }
            switchTriggerInputComboBox.SelectedIndex = 12;
        }
        private void LoadScanAdvancedOutput()
        {

            Type myType = typeof(SwitchScanAdvancedOutput);
            PropertyInfo[] properties = myType.GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                scanAdvancedOutputComboBox.Items.Add(prop.GetValue(myType, null).ToString());
            }
            scanAdvancedOutputComboBox.SelectedIndex = 1;
        }
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
                switchResourceNameComboBox.Items.Add(device.Name);
            }
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
            {
                switchResourceNameComboBox.SelectedIndex = 0;
            }
        }
        private void LoadDmmDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-DMM");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                dmmResourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                dmmResourceNameComboBox.SelectedIndex = 0;
        }
        private void LoadMeasurementModes()
        {
            measurementModeComboBox.Items.AddRange(Enum.GetNames(typeof(DmmMeasurementFunction)));
            measurementModeComboBox.Items.Remove(DmmMeasurementFunction.WaveformCurrent.ToString());
            measurementModeComboBox.Items.Remove(DmmMeasurementFunction.WaveformVoltage.ToString());
            measurementModeComboBox.SelectedIndex = 0;
        }
        #endregion UI Initial Value Config Section

        #region Program Properties
        private string SwitchResourceName
        {
            get
            {
                return this.switchResourceNameComboBox.Text;
            }
        }
        private string DmmResourceName
        {
            get
            {
                return this.dmmResourceNameComboBox.Text;
            }
        }
        private string TopologyName
        {
            get
            {
                return this.topologyNameComboBox.SelectedItem.ToString();
            }
        }
        private string MeasurementMode
        {
            get
            {
                return this.measurementModeComboBox.Text;
            }
        }
        private string ScanList
        {
            get
            {
                return this.scanListTextBox.Text;
            }
        }
        private string SwitchTriggerInput
        {
            get
            {
                return this.switchTriggerInputComboBox.Text;
            }
        }
        private string ScanAdvancedOutput
        {
            get
            {
                return this.scanAdvancedOutputComboBox.Text;
            }
        }
        private DmmTriggerSource TriggerSource
        {
            get
            {
                return (DmmTriggerSource)this.triggerSourceComboBox.SelectedValue;
            }
        }
        private DmmMeasurementCompleteDestination MeasCompleteDestination
        {
            get
            {
                return (DmmMeasurementCompleteDestination)this.measCompleteDestComboBox.SelectedValue;
            }
        }
        private double MeasurementRange
        {
            get
            {
                return (double)this.rangeNumericUpDown.Value;
            }
        }
        private double MeasurementResolution
        {
            get
            {
                return (double)this.resolutionNumericUpDown.Value;
            }
        }
        private int SamplesToFetch
        {
            get
            {
                return (int)this.samplesToFetchNumericUpDown.Value;
            }
        }
        #endregion Program Properties
        #region Program Functions
        private void ThreadTerminate()
        {

            //Waits for the thread To Terminate.
            workerThread.Join();
            workerThread = null;
            if (closeRequested == true)
                this.Close();
        }

        private void UpdateData(double[] temp)
        {

            int rowsAlreadyAdded = dataGridViewResults.Rows.Count;
            int totalRowsAfterEntry = temp.Length + rowsAlreadyAdded - 1;
            this.dataGridViewResults.Rows.Add(totalRowsAfterEntry - rowsAlreadyAdded);
            for (int i = rowsAlreadyAdded - 1; i < totalRowsAfterEntry; i++)
            {
                this.dataGridViewResults.Rows[i].Cells[0].Value = i + 1;
                this.dataGridViewResults.Rows[i].Cells[1].Value = temp[i - rowsAlreadyAdded + 1];
            }

        }
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
        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }
        private void ChangeControlState(bool isEnabled)
        {
            this.startButton.Enabled = isEnabled;
            this.dmmResourceNameComboBox.Enabled = isEnabled;
            this.switchResourceNameComboBox.Enabled = isEnabled;
            this.topologyNameComboBox.Enabled = isEnabled;
            this.measurementModeComboBox.Enabled = isEnabled;
            this.scanListTextBox.Enabled = isEnabled;
            this.triggerSourceComboBox.Enabled = isEnabled;
            this.scanAdvancedOutputComboBox.Enabled = isEnabled;
            this.switchTriggerInputComboBox.Enabled = isEnabled;
            this.rangeNumericUpDown.Enabled = isEnabled;
            this.resolutionNumericUpDown.Enabled = isEnabled;
            this.samplesToFetchNumericUpDown.Enabled = isEnabled;
            this.measCompleteDestComboBox.Enabled = isEnabled;
        }

        private void InitializeSwitchSession()
        {
            //Open a session to the switch module and set the topology.
            switchSession = new NISwitch(SwitchResourceName, TopologyName, false, true);
            switchSession.DriverOperation.Warning += new System.EventHandler<SwitchWarningEventArgs>(DriverOperationWarning);
        }
        private void DriverOperationWarning(object sender, SwitchWarningEventArgs e)
        {
            MessageBox.Show(e.ToString(), "Warning");
        }
        private void CloseSwitchSession()
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
        private void CloseDmmSession()
        {
            if (dmmSession != null)
            {
                try
                {
                    dmmSession.Close();
                    dmmSession = null;
                }
                catch (System.Exception ex)
                {
                    ShowError("Unable to Close Session, Reset the device.\n" + "Error : " + ex.Message);
                    Application.Exit();
                }
            }


        }

        private void InitializeDmmSession()
        {

            // Create a Dmm Session
            dmmSession = new NIDmm(DmmResourceName, true, true);

        }
        private void ConfigureDmmMeasurement()
        {
            ConfigureMeasurement();
            ConfigureDmmTrigger();
            ConfigureDmmMultiPoint();
            ConfigureSampleTriggerSlope();
            ConfigureDmmMeasurementComplete();
        }
        private void InitiateDmmMeasurement()
        {
            //Initiate the DMM measurement.
            dmmSession.Measurement.Initiate();
        }

        private string GetDmmInstrumentModel()
        {
            //Queries an attribute value to determine the instrument module of the DMM.
            return (dmmSession.DriverIdentity.InstrumentModel);
        }

        private void ConfigureDmmMeasurementComplete()
        {
            //Configures the destination of the DMM output trigger (Measurement Complete). 
            //This should match the switch module input trigger.
            dmmSession.Trigger.MeasurementCompleteDestination = MeasCompleteDestination;

            //Configures the slope of the DMM output trigger.
            dmmSession.Trigger.MeasurementCompleteDestinationSlope = DmmSlope.Negative;
        }

        private void ConfigureSampleTriggerSlope()
        {
            //Configures the slope of the secondary DMM input trigger (Sample Trigger).
            dmmSession.Trigger.MultiPoint.SampleTriggerSlope = DmmSlope.Negative;
        }

        private void ConfigureDmmMultiPoint()
        {
            //Configure a multipoint acquisition.
            DmmSampleTrigger SampleTrigger = GetSampleTrigger();
            dmmSession.Trigger.MultiPoint.Configure(1, 1, SampleTrigger, PrecisionTimeSpan.MaxValue);
            dmmSession.Trigger.MultiPoint.SampleCount = 0;
        }

        DmmSampleTrigger GetSampleTrigger()
        {
            if (TriggerSource == DmmTriggerSource.SoftwareTrigger)
                return DmmSampleTrigger.SoftwareTrigger;
            else if (TriggerSource == DmmTriggerSource.Ttl7)
                return DmmSampleTrigger.Ttl7;
            else if (TriggerSource == DmmTriggerSource.LbrTrig1)
                return DmmSampleTrigger.LbrTrig1;
            else if (TriggerSource == DmmTriggerSource.PxiStar)
                return DmmSampleTrigger.PxiStar;
            else if (TriggerSource == DmmTriggerSource.AuxTrig1)
                return DmmSampleTrigger.AuxTrig1;
            else if (TriggerSource == DmmTriggerSource.Ttl0)
                return DmmSampleTrigger.Ttl0;
            else if (TriggerSource == DmmTriggerSource.Ttl1)
                return DmmSampleTrigger.Ttl1;
            else if (TriggerSource == DmmTriggerSource.Ttl2)
                return DmmSampleTrigger.Ttl2;
            else if (TriggerSource == DmmTriggerSource.Ttl3)
                return DmmSampleTrigger.Ttl3;
            else if (TriggerSource == DmmTriggerSource.Ttl4)
                return DmmSampleTrigger.Ttl4;
            else if (TriggerSource == DmmTriggerSource.Ttl5)
                return DmmSampleTrigger.Ttl5;
            else if (TriggerSource == DmmTriggerSource.Ttl6)
                return DmmSampleTrigger.Ttl6;
            else if (TriggerSource == DmmTriggerSource.External)
                return DmmSampleTrigger.External;
            else
                return DmmSampleTrigger.Immediate;
        }

        private void ConfigureDmmTrigger()
        {
            //Configure the input trigger for the DMM. 
            //This should match the output trigger (Scan Advanced Output) of the switch.
            dmmSession.Trigger.Configure(TriggerSource, true);
            dmmSession.Trigger.Slope = DmmSlope.Negative;
        }

        private void ConfigureMeasurement()
        {
            //Configure the function, range, and resolution of the measurement.
            dmmSession.Configure((DmmMeasurementFunction)Enum.Parse(typeof(DmmMeasurementFunction), MeasurementMode), MeasurementRange, MeasurementResolution);

        }
        private void AbortSwitchScan()
        {
            ////Halts the scan.
            switchSession.Scan.Abort();

        }

        private void InitiateSwitchScan()
        {
            //Initiates the scan usings the configured scan list and triggers.
            switchSession.Scan.Initiate();
        }

        private void ConfigureSwitchScan()
        {
            //Configures the switch module for scanning. 
            switchSession.Scan.ConfigureList(ScanList, SwitchScanMode.BreakBeforeMake);

            //Configures the input trigger of the switch module. This should match the
            //output trigger of the DMM.
            switchSession.Scan.ConfigureTrigger(new PrecisionTimeSpan(0.0), SwitchTriggerInput, ScanAdvancedOutput);

            //Configures the switch to loop continuously through the scan list until
            //niSwitch_Abort is called.
            switchSession.Scan.Continuous = true;

            //Sets the polarity and edge of the switch input trigger.
            switchSession.Scan.TriggerInputPolarity = SwitchTriggerInputPolarity.FallingEdge;

            //Sets the polarity and edge of the switch output trigger.
            switchSession.Scan.AdvancedPolarity = SwitchAdvancedPolarity.FallingEdge;
        }
        private void StartMeasurement()
        {
            // Starts a Thread for Performing Scan, when NextButton is clicked, so that Main UI thread doesn't hangs.
            workerThread = new Thread(StartDmmMeasurement);
            workerThread.Start();

        }
        private void StartDmmMeasurement()
        {
            try
            {

                while (ThreadStopMarker == false)
                {
                    dmmSession.Measurement.ReadStatus(out acquisitionBacklog);
                    measurement = dmmSession.Measurement.FetchMultiPoint(PrecisionTimeSpan.MaxValue, Math.Max(acquisitionBacklog, SamplesToFetch));
                    dataGridViewResults.Invoke(new Action(() => UpdateData(measurement)));
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);

            }
            finally
            {
                this.BeginInvoke(new Action(ThreadTerminate));
            }
        }

        private void CreateDataGridColumns()
        {
            dataGridViewResults.Rows.Clear();
            dataGridViewResults.ColumnCount = 0;
            dataGridViewResults.Columns.Add("", "Sample Number");
            dataGridViewResults.Columns.Add("", "Measurement");
        }
        private void ToggleButtonState(bool isEnabled)
        {
            this.startButton.Enabled = isEnabled;
            this.stopButton.Enabled = !isEnabled;
        }
        #endregion Program Functions
        #region FormEvents
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeRequested = true;

            // Stops the Acquisition.
            if (workerThread != null)
            {
                ThreadStopMarker = true;
            }

            if (dmmSession != null)
            {
                dmmSession.Measurement.Abort();
                CloseDmmSession();
            }
            //Abort scanning.
            if (switchSession != null)
            {
                switchSession.Scan.Abort();
                CloseSwitchSession();
            }

            if (workerThread != null)
                e.Cancel = true;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                ThreadStopMarker = false;
                ToggleButtonState(false);

                //Programming the Switch       
                CloseSwitchSession();
                InitializeSwitchSession();
                ConfigureSwitchScan();

                //Programming the DMM         
                CloseDmmSession();

                InitializeDmmSession();
                ConfigureDmmMeasurement();
                dmmInstrumentModel = GetDmmInstrumentModel();
                InitiateDmmMeasurement();

                //If the DMM model is an NI 4060, add a delay.
                if (dmmInstrumentModel.Equals("PXI-4060") || dmmInstrumentModel.Equals("PCI-4060"))
                {
                    Thread.Sleep(1000);
                }
                CreateDataGridColumns();
                InitiateSwitchScan();

                //start measuring
                StartMeasurement();


            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                ChangeControlState(true);
                ToggleButtonState(true);
                CloseDmmSession();
                CloseSwitchSession();
            }

        }

        private void stopButton_Click(object sender, EventArgs e)
        {

            // Stops the Acquisition.
            if (workerThread != null)
            {
                ThreadStopMarker = true;
            }
            //Abort scanning.
            if (dmmSession != null)
            {
                dmmSession.Measurement.Abort();
                CloseDmmSession();
            }
            if (switchSession != null)
            {
                switchSession.Scan.Abort();
                CloseSwitchSession();
            }
            ChangeControlState(true);
            ToggleButtonState(true);
        }

        #endregion FormEvents



    }
}
