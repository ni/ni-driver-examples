//==================================================================================================
// Title        : DMM Switch Synchronous Scanning
// Description  : Use this example to learn how to work with NI-Switch and NI-DMM together,  The example, first configures scanning operation for switch, then measurement configurations for Dmm is done, Finally, the Dmm device does a fetch operation after the switch scan is performed.
//==================================================================================================
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDmm;
using NationalInstruments.ModularInstruments.NISwitch;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.DmmSwitchSynchronousScanning
{
    public partial class MainForm : Form
    {
        NIDmm dmmSession;
        NISwitch switchSession;
        double[] dmmSamplesArray;
        const long MillisecondsToWait = 5000;

        public MainForm()
        {
            InitializeComponent();
            LoadSwitchDeviceNames();
            LoadTopology();
            LoadDmmDeviceNames();
            LoadMeasurementModes();
            LoadSwitchTriggerInput();
            LoadDmmMeasCompleteDest();
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
        private string SwitchTriggerInput
        {
            get
            {
                return this.switchTriggerInputComboBox.Text;
            }
        }
        private double SampleInterval
        {
            get
            {
                return (double)this.sampleIntervalNumericUpDown.Value;
            }
        }
        #endregion Program Properties
        #region Program Functions
        private void InitializeDmmSession()
        {
            //Open a session to the DMM.
            dmmSession = new NIDmm(DmmResourceName, true, true);
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
            this.switchTriggerInputComboBox.Enabled = isEnabled;
            this.rangeNumericUpDown.Enabled = isEnabled;
            this.resolutionNumericUpDown.Enabled = isEnabled;
            this.samplesToFetchNumericUpDown.Enabled = isEnabled;
            this.measCompleteDestComboBox.Enabled = isEnabled;
        }
        private void ConfigureDmmMeasurement()
        {
            ConfigureMeasurement();
            ConfigureDmmMultiPoint();
            ConfigureDmmMeasurementComplete();
        }
        private void ConfigureDmmMeasurementComplete()
        {
            //Configures the destination of the DMM output trigger (Measurement Complete).
            //This should match the switch module input trigger.
            dmmSession.Trigger.MeasurementCompleteDestination = MeasCompleteDestination;

            //Configures the slope of the DMM output trigger.
            dmmSession.Trigger.MeasurementCompleteDestinationSlope = DmmSlope.Negative;
        }
        private void ConfigureDmmMultiPoint()
        {
            //Configure a multipoint acquisition.
            dmmSession.Trigger.MultiPoint.Configure(1,SamplesToFetch , DmmSampleTrigger.Interval, new PrecisionTimeSpan(SampleInterval));
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
        private void InitiateDmmMeasurement()
        {
            //Initiate the DMM measurement.
            dmmSession.Measurement.Initiate();
        }
        private void InitiateSwitchScan()
        {
            //Initiates the scan usings the configured scan list and triggers.
            switchSession.Scan.Initiate();
        }

        private void ConfigureSwitchScan()
        {
            

            //Configures the input trigger of the switch module. This should match the
            //output trigger of the DMM.
            switchSession.Scan.ConfigureTrigger(new PrecisionTimeSpan(0.0), SwitchTriggerInput, SwitchScanAdvancedOutput.None);

            //Configures the switch to loop continuously through the scan list until
            //niSwitch_Abort is called.
            switchSession.Scan.Continuous = true;

            //Configures the switch module for scanning. 
            switchSession.Scan.ConfigureList(ScanList, SwitchScanMode.BreakBeforeMake);


        }
       
      
        #endregion Program Functions
        #region FormEvents
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseDmmSession();
            CloseSwitchSession();
        }
      

        private void startButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                //Programming the Switch
                CloseSwitchSession();
                InitializeSwitchSession();
                ConfigureSwitchScan();


                //Programming the DMM
                CloseDmmSession();

                InitializeDmmSession();
                ConfigureDmmMeasurement();

                InitiateSwitchScan();
               
                bool isWaitingForTrigger = switchSession.Scan.IsWaitingForTrigger;
                
                var sw = new Stopwatch();
                sw.Start();

                while (isWaitingForTrigger)
                {
                    if (sw.ElapsedMilliseconds > MillisecondsToWait)
                    {
                        break;
                    }
                    isWaitingForTrigger = switchSession.Scan.IsWaitingForTrigger;
                }

                InitiateDmmMeasurement();

                //Download data from the DMM.
                dmmSamplesArray = dmmSession.Measurement.FetchMultiPoint(SamplesToFetch);

                AbortSwitchScan();
                this.dataGridViewResults.Rows.Clear();
                this.dataGridViewResults.Rows.Add(dmmSamplesArray.Length-1);
                for (int i = 0; i < dmmSamplesArray.Length; i++)
                {
                   
                    this.dataGridViewResults.Rows[i].Cells[0].Value = i + 1;
                    this.dataGridViewResults.Rows[i].Cells[1].Value = dmmSamplesArray[i];
                }

            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                ChangeControlState(true);
                CloseDmmSession();
                CloseSwitchSession();
            }

        }
        #endregion FormEvents

    }
}
