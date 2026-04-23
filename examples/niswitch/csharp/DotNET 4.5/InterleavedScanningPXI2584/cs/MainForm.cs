//==============================================================================================
// Title        : Interleaved Scanning PXI 2584
// Description  : Use this example to use NI-Switch and Ni-Dmm together. The example,first gets a interleaved scan list and using this scan list configuration is done for the scanning operation for switch, then measurement configurations for Dmm is done, Finally, the Dmm device takes the reading after the switch scan is performed.
//==============================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDmm;
using NationalInstruments.ModularInstruments.NISwitch;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;


namespace NationalInstruments.Examples.InterleavedScanningPxi2584
{
    public partial class MainForm : Form
    {
        GenerateMultiDeviceInterleavedScanList generateMultiDeviceInterleavedScanList;
        private const string SwitchTopology = "2584/Independent";
        string interleavedScanList;
        NISwitch switchSession;
        NIDmm dmmSession;
        string dmmInstrumentModel;
        Thread workerThread;
        volatile bool threadStop;
        bool closeRequested;
        object lockobj = new object();
        string StartEndRelationError, ValidChannelValuesError, InputFieldBlank;

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
        int acquisitionBacklog;
        double[] measurement;

        public MainForm()
        {
            InitializeComponent();
            InitializeLookupTable();
            LoadSwitchDeviceNames();
            LoadSwitchTriggerInput();
            LoadSwitchTriggerOutput();
            LoadDmmDeviceNames();
            LoadDmmMeasurementModes();
            LoadDmmMeasurementCompleteDestination();
            LoadDmmTriggerSource();
        }
        #region UI Initial Value Config Section
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
        private void LoadSwitchTriggerInput()
        {
            var triggerList = new List<DictionaryEntry>();
            triggerList.Add(new DictionaryEntry("Immediate", SwitchScanTriggerInput.Immediate));
            triggerList.Add(new DictionaryEntry("External", SwitchScanTriggerInput.External));
            triggerList.Add(new DictionaryEntry("TTL0", SwitchScanTriggerInput.Ttl0));
            triggerList.Add(new DictionaryEntry("TTL1", SwitchScanTriggerInput.Ttl1));
            triggerList.Add(new DictionaryEntry("TTL2", SwitchScanTriggerInput.Ttl2));
            triggerList.Add(new DictionaryEntry("TTL3", SwitchScanTriggerInput.Ttl3));
            triggerList.Add(new DictionaryEntry("TTL4", SwitchScanTriggerInput.Ttl4));
            triggerList.Add(new DictionaryEntry("TTL5", SwitchScanTriggerInput.Ttl5));
            triggerList.Add(new DictionaryEntry("TTL6", SwitchScanTriggerInput.Ttl6));
            triggerList.Add(new DictionaryEntry("TTL7", SwitchScanTriggerInput.Ttl7));
            triggerList.Add(new DictionaryEntry("PXI Star", SwitchScanTriggerInput.PxiStar));
            triggerList.Add(new DictionaryEntry("Software Trigger Functiom", SwitchScanTriggerInput.SoftwareTrigger));
            triggerList.Add(new DictionaryEntry("Rear Connector", SwitchScanTriggerInput.RearConnector));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 1", SwitchScanTriggerInput.RearConnectorModule1));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 2", SwitchScanTriggerInput.RearConnectorModule2));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 3", SwitchScanTriggerInput.RearConnectorModule3));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 4", SwitchScanTriggerInput.RearConnectorModule4));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 5", SwitchScanTriggerInput.RearConnectorModule5));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 6", SwitchScanTriggerInput.RearConnectorModule6));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 7", SwitchScanTriggerInput.RearConnectorModule7));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 8", SwitchScanTriggerInput.RearConnectorModule8));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 9", SwitchScanTriggerInput.RearConnectorModule9));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 10", SwitchScanTriggerInput.RearConnectorModule10));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 11", SwitchScanTriggerInput.RearConnectorModule11));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 12", SwitchScanTriggerInput.RearConnectorModule12));
            triggerList.Add(new DictionaryEntry("Front Connector", SwitchScanTriggerInput.FrontConnector));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 1", SwitchScanTriggerInput.FrontConnectorModule1));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 2", SwitchScanTriggerInput.FrontConnectorModule2));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 3", SwitchScanTriggerInput.FrontConnectorModule3));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 4", SwitchScanTriggerInput.FrontConnectorModule4));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 5", SwitchScanTriggerInput.FrontConnectorModule5));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 6", SwitchScanTriggerInput.FrontConnectorModule6));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 7", SwitchScanTriggerInput.FrontConnectorModule7));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 8", SwitchScanTriggerInput.FrontConnectorModule8));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 9", SwitchScanTriggerInput.FrontConnectorModule9));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 10", SwitchScanTriggerInput.FrontConnectorModule10));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 11", SwitchScanTriggerInput.FrontConnectorModule11));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 12", SwitchScanTriggerInput.FrontConnectorModule12));
            switchTriggerInputComboBox.DataSource = triggerList;
            switchTriggerInputComboBox.DisplayMember = "Key";
            switchTriggerInputComboBox.ValueMember = "Value";
            switchTriggerInputComboBox.SelectedIndex = 2;
        }
        private void LoadSwitchTriggerOutput()
        {

            var triggerList = new List<DictionaryEntry>();

            triggerList.Add(new DictionaryEntry("None", SwitchScanAdvancedOutput.None));
            triggerList.Add(new DictionaryEntry("External", SwitchScanAdvancedOutput.External));
            triggerList.Add(new DictionaryEntry("TTL0", SwitchScanAdvancedOutput.Ttl0));
            triggerList.Add(new DictionaryEntry("TTL1", SwitchScanAdvancedOutput.Ttl1));
            triggerList.Add(new DictionaryEntry("TTL2", SwitchScanAdvancedOutput.Ttl2));
            triggerList.Add(new DictionaryEntry("TTL3", SwitchScanAdvancedOutput.Ttl3));
            triggerList.Add(new DictionaryEntry("TTL4", SwitchScanAdvancedOutput.Ttl4));
            triggerList.Add(new DictionaryEntry("TTL5", SwitchScanAdvancedOutput.Ttl5));
            triggerList.Add(new DictionaryEntry("TTL6", SwitchScanAdvancedOutput.Ttl6));
            triggerList.Add(new DictionaryEntry("TTL7", SwitchScanAdvancedOutput.Ttl7));
            triggerList.Add(new DictionaryEntry("Rear Connector", SwitchScanAdvancedOutput.RearConnector));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 1", SwitchScanAdvancedOutput.RearConnectorModule1));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 2", SwitchScanAdvancedOutput.RearConnectorModule2));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 3", SwitchScanAdvancedOutput.RearConnectorModule3));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 4", SwitchScanAdvancedOutput.RearConnectorModule4));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 5", SwitchScanAdvancedOutput.RearConnectorModule5));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 6", SwitchScanAdvancedOutput.RearConnectorModule6));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 7", SwitchScanAdvancedOutput.RearConnectorModule7));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 8", SwitchScanAdvancedOutput.RearConnectorModule8));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 9", SwitchScanAdvancedOutput.RearConnectorModule9));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 10", SwitchScanAdvancedOutput.RearConnectorModule10));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 11", SwitchScanAdvancedOutput.RearConnectorModule11));
            triggerList.Add(new DictionaryEntry("Rear Connector of Module 12", SwitchScanAdvancedOutput.RearConnectorModule12));
            triggerList.Add(new DictionaryEntry("Front Connector", SwitchScanAdvancedOutput.FrontConnector));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 1", SwitchScanAdvancedOutput.FrontConnectorModule1));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 2", SwitchScanAdvancedOutput.FrontConnectorModule2));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 3", SwitchScanAdvancedOutput.FrontConnectorModule3));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 4", SwitchScanAdvancedOutput.FrontConnectorModule4));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 5", SwitchScanAdvancedOutput.FrontConnectorModule5));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 6", SwitchScanAdvancedOutput.FrontConnectorModule6));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 7", SwitchScanAdvancedOutput.FrontConnectorModule7));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 8", SwitchScanAdvancedOutput.FrontConnectorModule8));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 9", SwitchScanAdvancedOutput.FrontConnectorModule9));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 10", SwitchScanAdvancedOutput.FrontConnectorModule10));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 11", SwitchScanAdvancedOutput.FrontConnectorModule11));
            triggerList.Add(new DictionaryEntry("Front Connector of Module 12", SwitchScanAdvancedOutput.FrontConnectorModule12));
            switchScanAdvancedOutputComboBox.DataSource = triggerList;
            switchScanAdvancedOutputComboBox.DisplayMember = "Key";
            switchScanAdvancedOutputComboBox.ValueMember = "Value";
            switchScanAdvancedOutputComboBox.SelectedIndex = 3;
        }
        private void LoadDmmDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-DMM");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                dmmResourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                dmmResourceNameComboBox.SelectedIndex = 0;
        }
        private void LoadDmmMeasurementModes()
        {
            dmmMeasurementTypeComboBox.Items.AddRange(Enum.GetNames(typeof(DmmMeasurementFunction)));
            dmmMeasurementTypeComboBox.Items.Remove(DmmMeasurementFunction.WaveformCurrent.ToString());
            dmmMeasurementTypeComboBox.Items.Remove(DmmMeasurementFunction.WaveformVoltage.ToString());
            dmmMeasurementTypeComboBox.SelectedIndex = 0;
        }
        private void LoadDmmMeasurementCompleteDestination()
        {

            var triggerList = new List<DictionaryEntry>();
            triggerList.Add(new DictionaryEntry("None", DmmMeasurementCompleteDestination.None));
            triggerList.Add(new DictionaryEntry("External", DmmMeasurementCompleteDestination.External));
            triggerList.Add(new DictionaryEntry("TTL0", DmmMeasurementCompleteDestination.Ttl0));
            triggerList.Add(new DictionaryEntry("TTL1", DmmMeasurementCompleteDestination.Ttl1));
            triggerList.Add(new DictionaryEntry("TTL2", DmmMeasurementCompleteDestination.Ttl2));
            triggerList.Add(new DictionaryEntry("TTL3", DmmMeasurementCompleteDestination.Ttl3));
            triggerList.Add(new DictionaryEntry("TTL4", DmmMeasurementCompleteDestination.Ttl4));
            triggerList.Add(new DictionaryEntry("TTL5", DmmMeasurementCompleteDestination.Ttl5));
            triggerList.Add(new DictionaryEntry("TTL6", DmmMeasurementCompleteDestination.Ttl6));
            triggerList.Add(new DictionaryEntry("TTL7", DmmMeasurementCompleteDestination.Ttl7));
            triggerList.Add(new DictionaryEntry("LBR Trig0", DmmMeasurementCompleteDestination.LbrTrig0));
            dmmMeasurementCompleteDestinationComboBox.DataSource = triggerList;
            dmmMeasurementCompleteDestinationComboBox.DisplayMember = "Key";
            dmmMeasurementCompleteDestinationComboBox.ValueMember = "Value";
            dmmMeasurementCompleteDestinationComboBox.SelectedIndex = 2;
        }
        private void LoadDmmTriggerSource()
        {

            var triggerList = new List<DictionaryEntry>();
            triggerList.Add(new DictionaryEntry("Immediate", DmmTriggerSource.Immediate));
            triggerList.Add(new DictionaryEntry("External", DmmTriggerSource.External));
            triggerList.Add(new DictionaryEntry("TTL0", DmmTriggerSource.Ttl0));
            triggerList.Add(new DictionaryEntry("TTL1", DmmTriggerSource.Ttl1));
            triggerList.Add(new DictionaryEntry("TTL2", DmmTriggerSource.Ttl2));
            triggerList.Add(new DictionaryEntry("TTL3", DmmTriggerSource.Ttl3));
            triggerList.Add(new DictionaryEntry("TTL4", DmmTriggerSource.Ttl4));
            triggerList.Add(new DictionaryEntry("TTL5", DmmTriggerSource.Ttl5));
            triggerList.Add(new DictionaryEntry("TTL6", DmmTriggerSource.Ttl6));
            triggerList.Add(new DictionaryEntry("TTL7", DmmTriggerSource.Ttl7));
            triggerList.Add(new DictionaryEntry("PXI Star", DmmTriggerSource.PxiStar));
            triggerList.Add(new DictionaryEntry("LBR Trig1", DmmTriggerSource.LbrTrig1));
            triggerList.Add(new DictionaryEntry("Aux Trig1", DmmTriggerSource.AuxTrig1));
            dmmTriggerSourceComboBox.DataSource = triggerList;
            dmmTriggerSourceComboBox.DisplayMember = "Key";
            dmmTriggerSourceComboBox.ValueMember = "Value";
            dmmTriggerSourceComboBox.SelectedIndex = 3;
        }
        #endregion UI Initial Value Config Section

        #region FormEvents
        private void startButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                ThreadStopMarker = false;
                
                CloseSwitchSession();
                if (InputNotBlank())
                {
                    interleavedScanList = generateMultiDeviceInterleavedScanList.Generate(InterleavedStartChannel, InterleavedEndChannel);
                    if (interleavedScanList.Equals(StartEndRelationError) || interleavedScanList.Equals(ValidChannelValuesError))
                    {
                        throw new System.ArgumentException(interleavedScanList);
                    }
                    //programming switch
                    InitializeSwitchSession();
                    ConfigureSwitchScan();
                    CommitSwitch();



                    //programming DMM
                    CloseDmmSession();
                    InitializeDmmSession();
                    dmmInstrumentModel = GetDmmInstrumentModel();
                    ConfigureDmmMeasurement();
                    //If the DMM model is an NI 4060, add a delay.
                    if (dmmInstrumentModel.Equals("PXI-4060") || dmmInstrumentModel.Equals("PCI-4060"))
                    {
                        Thread.Sleep(1000);
                    }
                    InitiateDmmMeasurement();
                    CreateDataGridColumns();
                    InitiateSwitchScan();
                    //start measuring
                    StartMeasurement();
                    ToggleButtonState(false);
                }
                else
                {
                    throw new System.ArgumentException(InputFieldBlank);
                }
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
        private void CreateDataGridColumns()
        {
            dataGridViewResults.Rows.Clear();
            dataGridViewResults.ColumnCount = 0;
            dataGridViewResults.Columns.Add("", "Sample Number");
            dataGridViewResults.Columns.Add("", "Measurement");
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
        #endregion FormEvents

        #region ProgramFunctions
        private bool InputNotBlank()
        {
            if ((string.IsNullOrEmpty(((Control)this.switchInterleavedEndChannelNumericUpDown).Text)) || (string.IsNullOrEmpty(((Control)this.switchInterleavedStartChannelNumericUpDown).Text)))
            {
                return false;
            }
            else
                return true;
        }
        private void ToggleButtonState(bool isEnabled)
        {
            this.startButton.Enabled = isEnabled;
            this.stopButton.Enabled = !isEnabled;
        }

        private void ChangeControlState(bool isEnabled)
        {
            this.startButton.Enabled = isEnabled;
            this.stopButton.Enabled = isEnabled;
            this.dmmResourceNameComboBox.Enabled = isEnabled;
            this.switchResourceNameComboBox.Enabled = isEnabled;
            this.switchInterleavedEndChannelNumericUpDown.Enabled = isEnabled;
            this.switchInterleavedStartChannelNumericUpDown.Enabled = isEnabled;
            this.switchTriggerInputComboBox.Enabled = isEnabled;
            this.switchScanAdvancedOutputComboBox.Enabled = isEnabled;
            this.dmmMeasurementTypeComboBox.Enabled = isEnabled;
            this.dmmRangeNumericUpDown.Enabled = isEnabled;
            this.dmmResolutionNumericUpDown.Enabled = isEnabled;
            this.dmmSamplesNumericUpDown.Enabled = isEnabled;
            this.dmmMeasurementCompleteDestinationComboBox.Enabled = isEnabled;
            this.dmmTriggerSourceComboBox.Enabled = isEnabled;
        }

        private void InitializeLookupTable()
        {
            generateMultiDeviceInterleavedScanList = new GenerateMultiDeviceInterleavedScanList();
            generateMultiDeviceInterleavedScanList.InitializeLookupTable();
            StartEndRelationError = "Interleaved start channel must be smaller or equal to interleaved end channel on all devices.";
            ValidChannelValuesError = "Valid channel numbers for the PXI-2584 interleaved scanning are between 0 and 10.";
            InputFieldBlank = "The input fields for Interleaved Start or End Channel should not be blank";
        }

        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }

        private void DriverOperationWarning(object sender, SwitchWarningEventArgs e)
        {
            MessageBox.Show(e.ToString(), "Warning");
        }

        private void InitializeSwitchSession()
        {
            //Open a session to the switch module and set the topology.
            switchSession = new NISwitch(SwitchResourceName, SwitchTopology, false, true);
            switchSession.DriverOperation.Warning += new System.EventHandler<SwitchWarningEventArgs>(DriverOperationWarning);
        }

        private void ConfigureSwitchScan()
        {
            //Configures the input trigger of the switch module. This should match the
            //output trigger of the DMM.
            switchSession.Scan.ConfigureTrigger(new PrecisionTimeSpan(0.0), TriggerInput, ScanAdvancedOutput);

            //Configures the switch to loop continuously through the scan list until
            //niSwitch_Abort is called.
            switchSession.Scan.Continuous = true;

            //Configures the switch module for scanning. 
            switchSession.Scan.ConfigureList(interleavedScanList, SwitchScanMode.None);
        }

        private void CommitSwitch()
        {
            switchSession.Scan.Commit();
        }

        private void InitiateSwitchScan()
        {
            switchSession.Scan.Initiate();
        }

        private void AbortSwitchScan()
        {
            if (switchSession != null)
            {
                switchSession.Scan.Abort();
            }
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

        private string GetDmmInstrumentModel()
        {
            //Queries an attribute value to determine the instrument module of the DMM.
            return (dmmSession.DriverIdentity.InstrumentModel);
        }

        private void ConfigureDmmMeasurement()
        {
            ConfigureMeasurement();
            ConfigureDmmTrigger();
            ConfigureDmmMultiPoint();
            ConfigureSampleTriggerSlope();
            ConfigureDmmMeasurementComplete();
        }

        private void ConfigureMeasurement()
        {

            //Configure the function, range, and resolution of the measurement.
            
            dmmSession.Configure((DmmMeasurementFunction)Enum.Parse(typeof(DmmMeasurementFunction), MeasurementMode), DmmRange, DmmResolution);
        }

        private void ConfigureDmmTrigger()
        {
            //Configure the input trigger for the DMM. 
            //This should match the output trigger (Scan Advanced Output) of the switch.

            dmmSession.Trigger.Configure(DmmTriggerSource, true);
            dmmSession.Trigger.Slope = DmmSlope.Negative;
        }

        private void ConfigureDmmMultiPoint()
        {
            //Configure a multipoint acquisition.
            dmmSession.Trigger.MultiPoint.Configure(1, 1, DmmSampleTrigger.External, PrecisionTimeSpan.MaxValue);
            dmmSession.Trigger.MultiPoint.SampleCount = 0;

        }

        private void ConfigureSampleTriggerSlope()
        {
            //Configures the slope of the secondary DMM input trigger (Sample Trigger).
            dmmSession.Trigger.MultiPoint.SampleTriggerSlope = DmmSlope.Negative;
        }

        private void ConfigureDmmMeasurementComplete()
        {
            //Configures the destination of the DMM output trigger (Measurement Complete). 
            //This should match the switch module input trigger.
            dmmSession.Trigger.MeasurementCompleteDestination = DmmMeasurementCompleteDestination;
            //Configures the slope of the DMM output trigger.
            dmmSession.Trigger.MeasurementCompleteDestinationSlope = DmmSlope.Negative;
        }

        private void InitiateDmmMeasurement()
        {
            dmmSession.Measurement.Initiate();
        }

        private void CloseDmm()
        {
            if (dmmSession != null)
            {
                try
                {
                    dmmSession.Measurement.Abort();
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
                    measurement = dmmSession.Measurement.FetchMultiPoint(new PrecisionTimeSpan(5), Math.Max(acquisitionBacklog, SamplesToFetch));
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
        #endregion ProgramFunctions

        #region ProgramProperties
        private string SwitchResourceName
        {
            get
            {
                return this.switchResourceNameComboBox.Text;
            }
        }
        private int InterleavedStartChannel
        {
            get
            {
                return (int)this.switchInterleavedStartChannelNumericUpDown.Value;
            }
        }

        private int InterleavedEndChannel
        {
            get
            {
                return (int)this.switchInterleavedEndChannelNumericUpDown.Value;
            }
        }
        private string TriggerInput
        {
            get
            {
                return (string)this.switchTriggerInputComboBox.SelectedValue;
            }
        }
        private int SamplesToFetch
        {
            get
            {
                return (int)this.dmmSamplesNumericUpDown.Value;
            }
        }
        private string ScanAdvancedOutput
        {
            get
            {
                return (string)this.switchScanAdvancedOutputComboBox.SelectedValue;
            }
        }


        private string DmmResourceName
        {
            get
            {
                return this.dmmResourceNameComboBox.Text;
            }
        }
        private string MeasurementMode
        {
            get
            {
                return this.dmmMeasurementTypeComboBox.Text;
            }
        }
        
      
        private double DmmResolution
        {
            get
            {
                return (double)this.dmmResolutionNumericUpDown.Value;
            }
        }
        private DmmMeasurementCompleteDestination DmmMeasurementCompleteDestination
        {
            get
            {
                return (DmmMeasurementCompleteDestination)this.dmmMeasurementCompleteDestinationComboBox.SelectedValue;
            }
        }
        private double DmmRange
        {
            get
            {
                return (double)this.dmmRangeNumericUpDown.Value;
            }
        }
       
        private DmmTriggerSource DmmTriggerSource
        {
            get
            {
                return (DmmTriggerSource)this.dmmTriggerSourceComboBox.SelectedValue;
            }
        }

        #endregion ProgramProperties




    }
}
