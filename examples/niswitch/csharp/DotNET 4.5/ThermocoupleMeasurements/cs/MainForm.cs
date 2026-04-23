//==================================================================================================
// Title        : Thermocouple Measurements
// Description  :This example helps us learn how to use Switch and Dmm to make thermocouple            measurements.The example first configures scan operation for switch, then dmm measurement configuration performed. The dmm fetches data once the switch scan operation is completed, for each fetch operation done by the Dmm the thermocouple conversion is done and diplayed in the UI.                
//==================================================================================================
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDmm;
using NationalInstruments.ModularInstruments.NISwitch;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ThermocoupleMeasurements
{
    public partial class MainForm : Form
    {
        NIDmm dmmSession;
        NISwitch switchSession;
        int dmmSampleCount = 1;
        double[] dmmSamplesArray;
        string dmmInstrumentModel;
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
            LoadDmmDeviceNames();
            LoadMeasurementModes();
            LoadTriggerInput();
            LoadScanAdvancedOutput();
            LoadMeasCompleteDest();
            LoadTriggerSource();
            LoadThermocoupleType();

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
        private void LoadTriggerInput()
        {
            Type myType = typeof(SwitchScanTriggerInput);
            PropertyInfo[] properties = myType.GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                triggerInputComboBox.Items.Add(prop.GetValue(myType, null).ToString());
            }
            triggerInputComboBox.SelectedIndex = 3;
        }
        private void LoadScanAdvancedOutput()
        {
            Type myType = typeof(SwitchScanAdvancedOutput);
            PropertyInfo[] properties = myType.GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                scanAdvancedOutputComboBox.Items.Add(prop.GetValue(myType, null).ToString());
            }
            scanAdvancedOutputComboBox.SelectedIndex = 3;
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
        private void LoadMeasCompleteDest()
        {
            Type myType = typeof(DmmMeasurementCompleteDestination);
            PropertyInfo[] properties = myType.GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                measCompleteDestComboBox.Items.Add(prop.GetValue(myType, null).ToString());
            }
            measCompleteDestComboBox.SelectedIndex = 2;
        }
        private void LoadTriggerSource()
        {
            Type myType = typeof(DmmTriggerSource);
            PropertyInfo[] properties = myType.GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                triggerSourceComboBox.Items.Add(prop.GetValue(myType, null).ToString());
            }
            triggerSourceComboBox.SelectedIndex = 4;
        }
        private void LoadThermocoupleType()
        {
            List<KeyValuePair<char, UInt16>> thermocoupleValue = new List<KeyValuePair<char, UInt16>>();
            thermocoupleValue.Add(new KeyValuePair<char, UInt16>('B', 0));
            thermocoupleValue.Add(new KeyValuePair<char, UInt16>('E', 1));
            thermocoupleValue.Add(new KeyValuePair<char, UInt16>('J', 2));
            thermocoupleValue.Add(new KeyValuePair<char, UInt16>('K', 3));
            thermocoupleValue.Add(new KeyValuePair<char, UInt16>('R', 4));
            thermocoupleValue.Add(new KeyValuePair<char, UInt16>('S', 5));
            thermocoupleValue.Add(new KeyValuePair<char, UInt16>('T', 6));
            thermocoupleValue.Add(new KeyValuePair<char, UInt16>('N', 7));

            thermocoupleTypeComboBox.DisplayMember = "Key";
            thermocoupleTypeComboBox.ValueMember = "Value";
            thermocoupleTypeComboBox.DataSource = thermocoupleValue;
            thermocoupleTypeComboBox.SelectedIndex = 2;
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
        private string ScanAdvancedOutput
        {
            get
            {
                return this.scanAdvancedOutputComboBox.Text;
            }
        }
        private string TriggerInput
        {
            get
            {
                return this.triggerInputComboBox.Text;
            }
        }
        private int NumberOfChannels
        {
            get
            {
                return (int)this.numOfChannelsNumericUpDown.Value;
            }
        }
        private string MeasurementCompleteDestination
        {
            get
            {
                return this.measCompleteDestComboBox.Text;
            }
        }
        private string TriggerSource
        {
            get
            {
                return this.triggerSourceComboBox.Text;
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
        private UInt16 ThermocoupleType
        {
            get
            {
                return (UInt16)this.thermocoupleTypeComboBox.SelectedValue;
            }
        }

        #endregion Program Properties
        #region ProgramFunctions
        #region SwitchDMMConfig
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
            this.triggerInputComboBox.Enabled = isEnabled;
            this.scanAdvancedOutputComboBox.Enabled = isEnabled;
            this.numOfChannelsNumericUpDown.Enabled = isEnabled;
            this.thermocoupleTypeComboBox.Enabled = isEnabled;
            this.triggerSourceComboBox.Enabled = isEnabled;
            this.measCompleteDestComboBox.Enabled = isEnabled;

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

        private void ConfigureDmmMeasurementComplete()
        {
            //Configures the destination of the DMM output trigger (Measurement Complete). 
            //This should match the switch module input trigger.
            dmmSession.Trigger.MeasurementCompleteDestination = MeasurementCompleteDestination;

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

            dmmSession.Trigger.MultiPoint.Configure(1, dmmSampleCount, TriggerSource, PrecisionTimeSpan.MaxValue);
            dmmSession.Trigger.MultiPoint.SampleCount = 0;
        }

        private void ConfigureDmmTrigger()
        {
            // Configure Trigger Source
            dmmSession.Trigger.Source = TriggerSource;

            dmmSession.Trigger.Slope = DmmSlope.Negative;

        }

        private void ConfigureMeasurement()
        {
            //Configure the function, range, and resolution of the measurement.
            dmmSession.Configure((DmmMeasurementFunction)Enum.Parse(typeof(DmmMeasurementFunction), MeasurementMode), 1.0, 0.000001);

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
        private string GetDmmInstrumentModel()
        {
            //Queries an attribute value to determine the instrument module of the DMM.
            return (dmmSession.DriverIdentity.InstrumentModel);
        }
        private void ConfigureSwitchScan()
        {
            string ScanListInput = "cjtemp->com0;" + ScanList;
            //Configures the input trigger of the switch module. This should match the
            //output trigger of the DMM.
            switchSession.Scan.ConfigureTrigger(new PrecisionTimeSpan(0.0), TriggerInput, ScanAdvancedOutput);

            //Configures the switch to loop continuously through the scan list until
            //niSwitch_Abort is called.
            switchSession.Scan.Continuous = true;

            //Configures the switch module for scanning. 
            switchSession.Scan.ConfigureList(ScanListInput, SwitchScanMode.BreakBeforeMake);

        }
        private void CommitScan()
        {
            switchSession.Scan.Commit();
        }
        #endregion SwitchDMMConfig
        private void ThreadTerminate()
        {

            //Waits for the thread To Terminate.
            workerThread.Join();
            workerThread = null;
            if (closeRequested == true)
                this.Close();
        }
        private void ToggleButtonState(bool isEnabled)
        {
            this.startButton.Enabled = isEnabled;
            this.stopButton.Enabled = !isEnabled;
        }
        private void StartMeasurement()
        {
            // Starts a Thread for Performing Scan, when NextButton is clicked, so that Main UI thread doesn't hangs.
            workerThread = new Thread(StartDmmMeasurement);
            workerThread.Start();
        }

        private void StartDmmMeasurement()
        {
            //Send software trigger to switch module when Next Connection button pressed on front panel.
            int j = 0;
            try
            {

                while (ThreadStopMarker == false)
                {
                    //Download data from the DMM.
                    dmmSamplesArray = dmmSession.Measurement.FetchMultiPoint(PrecisionTimeSpan.MaxValue, NumberOfChannels + 1);
                    double[] temp = new double[NumberOfChannels + 1];
                    Invoke(new Action(() => temp = ConvertThermocoupleReading(dmmSamplesArray)));
                    dataGridViewResults.Invoke(new Action(() => UpdateData(temp, j)));
                    j++;

                }
                this.BeginInvoke(new Action(ThreadTerminate));
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                ThreadStopMarker = true;
                this.BeginInvoke(new Action(ChangingControlState));
                this.BeginInvoke(new Action(ThreadTerminate));
            }
        }

        private void UpdateData(double[] temp, int j)
        {
            this.dataGridViewResults.Rows.Add();
            for (int i = 0; i < temp.Length; i++)
            {
                this.dataGridViewResults.Rows[j].Cells[i].Value = temp[i];
            }

        }
        #region ThermoCalculations
        private double[] ConvertThermocoupleReading(double[] dmmSamplesArray)
        {
            double[] temperature = new double[NumberOfChannels];
            double[] thermocoupleVoltages = new double[NumberOfChannels];
            Array.Copy(dmmSamplesArray, 1, thermocoupleVoltages, 0, NumberOfChannels);
            double thermistorReading = ConvertThermistorReading(dmmSamplesArray[0]);
            double thermocoupleVoltage = ConvertTempToVolts(thermistorReading);

            for (int i = 0; i < thermocoupleVoltages.Length; i++)
            {
                temperature[i] = ConvertVoltToTemp((thermocoupleVoltages[i] + thermocoupleVoltage));
            }

            return temperature;
        }

        private  double ConvertVoltToTemp(double V)
        {
            double temp = 0;


            switch (ThermocoupleType)
            {

                case 0:
                    temp = CalculateBThermocoupleVoltsToTemp(V * 1000000.00);
                    break;
                case 1:
                    temp = CalculateEThermocoupleVoltsToTemp(V * 1000000.00);
                    break;
                case 2:
                    temp = CalculateJThermocoupleVoltsToTemp(V * 1000000.00);
                    break;
                case 3:
                    temp = CalculateKThermocoupleTempToVolts(V * 1000000.00);
                    break;
                case 4:
                    temp = CalculateRThermocoupleTempToVolts(V * 1000000.00);
                    break;
                case 5:
                    temp = CalculateSThermocoupleTempToVolts(V * 1000000.00);
                    break;
                case 6:
                    temp = CalculateTThermocoupleTempToVolts(V * 1000000.00);
                    break;
                case 7:
                    temp = CalculateNThermocoupleTempToVolts(V * 1000000.00);
                    break;
            }

            return (temp);
        }
        private static double CalculateNThermocoupleVoltsToTemp(double V)
        {
            double T;
            double v = V;
            int condition = ((V >= 20613.0) ? 1 : 0) + ((V >= 0.0) ? 1 : 0);
            if (condition == 0)
            {
                T = v * ((3.8436847E-2) + v * ((1.1010485E-6) + v * ((5.2229312E-9) + v * ((7.2060525E-12) + v * ((5.8488586E-15) + v * ((2.7754916E-18) + v * ((7.7075166E-22) + v * ((1.1582665E-25) + v * (7.3138868E-30)))))))));
            }
            else if (condition == 1)
            {
                T = v * ((3.86896E-2) + v * ((-1.08267E-6) + v * ((4.70205E-11) + v * ((-2.12169E-18) + v * ((-1.17272E-19) + v * ((5.39280E-24) + v * (-7.98156E-29)))))));
            }
            else
            {
                T = (1.972485E1) + v * ((3.300943E-2) + v * ((-3.915159E-7) + v * ((9.855391E-12) + v * ((-1.274371E-16) + v * (7.767022E-22)))));
            }
            return T;
        }
        private static double CalculateTThermocoupleVoltsToTemp(double V)
        {
            double T;
            double v = V;
            if (V >= 0.0)
            {
                T = v * ((2.592800E-2) + v * ((-7.602961E-7) + v * ((4.637791E-11) + v * ((-2.165394E-15) + v * ((6.048144E-20) + v * (-7.293422E-25))))));
            }
            else
            {
                T = v * ((2.5949192E-2) + v * ((-2.1316967E-7) + v * ((7.9018692E-10) + v * ((4.2527777E-13) + v * ((1.3304473E-16) + v * ((2.0241446E-20) + v * (1.2668171E-24)))))));
            }
            return T;
        }
        private static double CalculateSThermocoupleVoltsToTemp(double V)
        {
            int condition = ((V >= 17536.0) ? 1 : 0) + ((V >= 10332.0) ? 1 : 0) + ((V >= 1874.0) ? 1 : 0);
            double T;
            double v = V;
            if (condition == 0)
            {
                T = v * ((1.84949460E-1) + v * ((-8.00504062E-5) + v * ((1.02237430E-7) + v * ((-1.52248592E-10) + v * ((1.88821343E-13) + v * ((-1.59085941E-16) + v * ((8.23027880E-20) + v * ((-2.34181944E-23) + v * (2.79786260E-27)))))))));
            }
            else if (condition == 1)
            {
                T = (1.291507177E1) + v * ((1.466298863E-1) + v * ((-1.534713402E-5) + v * ((3.145945973E-9) + v * ((-4.163257839E-13) + v * ((3.187963771E-17) + v * ((-1.29163750E-21) + v * ((2.183475087E-26) + v * ((-1.447379511E-31) + v * (8.211272125E-36)))))))));
            }
            else if (condition == 2)
            {
                T = (-8.087801117E1) + v * ((1.621573104E-1) + v * ((-8.536869453E-6) + v * ((4.719686976E-10) + v * ((-1.441693666E-14) + v * (2.081618890E-19)))));
            }
            else
            {
                T = (5.333875126E4) + v * ((-1.235892298E1) + v * ((1.092657613E-3) + v * ((-4.265693686E-8) +
v * (6.247205420E-13))));
            }
            return T;
        }
        private static double CalculateRThermocoupleVoltsToTemp(double V)
        {
            int condition = ((V >= 19739.0) ? 1 : 0) + ((V >= 11361.0) ? 1 : 0) + ((V >= 1923.0) ? 1 : 0);
            double T;
            double v = V;
            if (condition == 0)
            {
                T = v * ((1.8891380E-1) + v * ((-9.3835290E-5) + v * ((1.3068619E-7) + v * ((-2.2703580E-10) + v * ((3.5145659E-13) + v * ((-3.8953900E-16) + v * ((2.8239471E-19) + v * ((-1.2607281E-22) + v * ((3.1353611E-26) + v * (-3.3187769E-30))))))))));
            }
            else if (condition == 1)
            {
                T = (1.334584505E1) + v * ((1.472644573E-1) + v * ((-1.844024844E-5) + v * ((4.031129726E-9) + v * ((-6.249428360E-13) + v * ((6.468412046E-17) + v * ((-4.458750426E-21) + v * ((1.994710149E-25) + v * ((-5.313401790E-30) + v * (6.481976217E-35)))))))));
            }
            else if (condition == 2)
            {
                T = (-8.199599416E1) + v * ((1.553962042E-1) + v * ((-8.342197663E-6) + v * ((4.279433549E-10) + v * ((-1.191577910E-14) + v * (1.492290091E-19)))));
            }
            else
            {
                T = (3.406177836E4) + v * ((-7.023729171) + v * ((5.582903813E-4) + v * ((-1.952394635E-8) + v * (2.560740231E-13))));
            }
            return T;
        }

        private static double CalculateKThermocoupleVoltsToTemp(double V)
        {
            double T;
            double v = V;
            int condition = (V >= 20644.0 ? 1 : 0) + (V >= 0.0 ? 1 : 0);
            if (condition == 0)
            {
                T = v * ((2.5173462E-2) + v * ((-1.1662878E-6) + v * ((-1.0833638E-9) + v * ((-8.9773540E-13) + v * ((-3.7342377E-16) + v * ((-8.6632643E-20) + v * ((-1.0450598E-23) + v * (-5.1920577E-28))))))));
            }
            else if (condition == 1)
            {
                T = v * ((2.508355E-2) + v * ((7.860106E-8) + v * ((-2.503131E-10) + v * ((8.315270E-14) + v * ((-1.228034E-17) + v * ((9.804036E-22) + v * ((-4.413030E-26) + v * ((1.057734E-30) + v * (-1.052755E-35)))))))));
            }
            else
            {
                T = (-1.318058E+2) + v * ((4.830222E-2) + v * ((-1.646031E-6) + v * ((5.464731E-11) + v * ((-9.650715E-16) + v * ((8.802193E-21) + v * (-3.110810E-26))))));

            }
            return T;
        }
        private static  double CalculateJThermocoupleVoltsToTemp(double V)
        {
            double T;
            double v = V;
            int condition = (V >= 42919.0 ? 1 : 0) + (V >= 0.0 ? 1 : 0);
            if (condition == 0)
            {
                T = v * ((1.9528268E-2) + v * ((-1.2286185E-6) + v * ((-1.0752178E-9) + v * ((-5.9086933E-13) + v * ((-1.7256713E-16) + v * ((-2.8131513E-20) + v * ((-2.3963370E-24) + v * (-8.3823321E-29))))))));
            }
            else if (condition == 1)
            {
                T = v * ((1.978425E-2) + v * ((-2.001204E-7) + v * ((1.036969E-11) + v * ((-2.549687E-16) + v * ((3.585153E-21) + v * ((-5.344285E-26) + v * (5.099890E-31)))))));
            }
            else
            {
                T = (-3.11358187E3) + v * ((3.00543684E-1) + v * ((-9.94773230E-6) + v * ((1.70276630E-10) + v * ((-1.43033468E-15) + v * (4.73886084E-21)))));
            }
            return T;
        }
        private static double CalculateEThermocoupleVoltsToTemp(double V)
        {
            double T;
            double v = V;
            if (V >= 0.0)
            {
                T = v * ((1.7057035E-2) + v * ((-2.3301759E-7) + v * ((6.5435585E-12) + v * ((-7.3562749E-17) + v * ((-1.7896001E-21) + v * ((8.4036165E-26) + v * ((-1.3735879E-30) + v * ((1.0629823E-35) + v * (-3.2447087E-41)))))))));
            }
            else
            {
                T = v * ((1.6977288E-2) + v * ((-4.3514970E-7) + v * ((-1.5859697E-10) + v * ((-9.2502871E-14) + v * ((-2.6084314E-17) + v * ((-4.1360199E-21) + v * ((-3.4034030E-25) + v * (-1.1564890E-29))))))));
            }
            return T;
        }
        private static double CalculateBThermocoupleVoltsToTemp(double V)
        {
            double T;
            double v = V;
            if (V >= 2431.0)
            {
                T = (2.1315071E2) + v * ((2.8510504E-1) + v * ((-5.2742887E-5) + v * ((9.9160804E-9) + v * ((-1.2965303E-12) + v * ((1.1195870E-16) + v * ((-6.0625199E-21) + v * ((1.8661696E-25) + v * (-2.4878585E-30))))))));
            }
            else
            {
                T = (9.842332E1) + v * ((6.9971500E-1) + v * ((-8.4765304E-4) + v * ((1.0052644E-6) + v * ((-8.3345952E-10) + v * ((4.5508542E-13) + v * ((-1.5523037E-16) + v * ((2.9886750E-20) + v * (-2.4742860E-24))))))));
            }
            return T;
        }

        private  double ConvertTempToVolts(double T)
        {
            double V = 0;

            switch (ThermocoupleType)
            {

                case 0:
                    V = CalculateBThermocoupleTempToVolts(T);
                    break;
                case 1:
                    V = CalculateEThermocoupleTempToVolts(T);
                    break;
                case 2:
                    V = CalculateJThermocoupleTempToVolts(T);
                    break;
                case 3:
                    V = CalculateKThermocoupleTempToVolts(T);
                    break;
                case 4:
                    V = CalculateRThermocoupleTempToVolts(T);
                    break;
                case 5:
                    V = CalculateSThermocoupleTempToVolts(T);
                    break;
                case 6:
                    V = CalculateTThermocoupleTempToVolts(T);
                    break;
                case 7:
                    V = CalculateNThermocoupleTempToVolts(T);
                    break;
            }

            return (V / 1000000.0);
        }
        private static double CalculateNThermocoupleTempToVolts(double T)
        {
            double V;
            double c = T;
            if (T >= 0.0)
            {
                V = (2.5929394601E1) * c + (1.5710141880E-2) * c * c + (4.3825627237E-5) * c * c * c + (-2.5261169794E-7) * c * c * c * c + (6.4311819339E-10) * c * c * c * c * c + (-1.0063471519E-12) * c * c * c * c * c * c + (9.9745338992E-16) * c * c * c * c * c * c * c + (-6.0863245607E-19) * c * c * c * c * c * c * c * c + (2.0849229339E-22) * c * c * c * c * c * c * c * c * c + (-3.0682196151E-26) * c * c * c * c * c * c * c * c * c * c;
            }
            else
            {
                V = (2.6159105962E1) * c + (1.0957484228E-2) * c * c + (-9.3841111554E-5) * c * c * c + (-4.6412039759E-8) * c * c * c * c + (-2.6303357716E-9) * c * c * c * c * c + (-2.2653438003E-11) * c * c * c * c * c * c + (-7.6089300791E-14) * c * c * c * c * c * c * c + (-9.3419667835E-17) * c * c * c * c * c * c * c * c;
            }
            return V;
        }
        private static double CalculateTThermocoupleTempToVolts(double T)
        {
            double V;
            double c = T;
            if (T >= 0.0)
            {
                V = (3.8748106364E1) * c + (3.3292227880E-2) * c * c + (2.0618243404E-4) * c * c * c + (-2.1882256846E-6) * c * c * c * c + (1.0996880928E-8) * c * c * c * c * c + (-3.0815758772E-11) * c * c * c * c * c * c + (4.5479135290E-14) * c * c * c * c * c * c * c + (-2.7512901673E-17) * c * c * c * c * c * c * c * c;
            }
            else
            {
                V = (3.8748106364E1) * c + (4.4194434347E-2) * c * c + (1.1844323105E-4) * c * c * c + (2.0032973554E-5) * c * c * c * c + (9.0138019559E-7) * c * c * c * c * c + (2.2651156593E-8) * c * c * c * c * c * c + (3.6071154205E-10) * c * c * c * c * c * c * c + (3.8493939883E-12) * c * c * c * c * c * c * c * c + (2.8213521925E-14) * c * c * c * c * c * c * c * c * c + (1.4251594779E-16) * c * c * c * c * c * c * c * c * c * c + (4.8768662286E-19) * c * c * c * c * c * c * c * c * c * c * c + (1.0795539270E-21) * c * c * c * c * c * c * c * c * c * c * c * c + (1.3945027062E-24) * c * c * c * c * c * c * c * c * c * c * c * c * c + (7.9795153927E-28) * c * c * c * c * c * c * c * c * c * c * c * c * c * c;
            }
            return V;
        }
        private static double CalculateSThermocoupleTempToVolts(double T)
        {
            int condition = ((T >= 1664.5) ? 1 : 0) + ((T >= 1064.18) ? 1 : 0);
            double V;
            double c = T;
            if (condition == 0)
            {
                V = (5.40313308631) * c + (1.25934289740E-2) * c * c + (-2.32477968689E-5) * c * c * c + (3.22028823036E-8) * c * c * c * c + (-3.31465196389E-11) * c * c * c * c * c + (2.55744251786E-14) * c * c * c * c * c * c + (-1.25068871393E-17) * c * c * c * c * c * c * c + (2.71443176145E-21) * c * c * c * c * c * c * c * c;
            }
            else if (condition == 1)
            {
                V = (2.95157925316E3) + (-2.52061251332) * c + (1.59564501865E-2) * c * c + (-7.64085947576E-6) * c * c * c + (2.05305291024E-9) * c * c * c * c + (-2.93359668173E-13) * c * c * c * c * c;
            }
            else
            {
                V = (1.52232118209E5) + (-2.68819888545E2) * c + (1.71280280471E-1) * c * c + (-3.45895706453E-5) * c * c * c + (-9.34633971046E-12) * c * c * c * c;
            }
            return V;
        }
        private static double CalculateRThermocoupleTempToVolts(double T)
        {
            int condition = ((T >= 1664.5) ? 1 : 0) + ((T >= 1064.18) ? 1 : 0);
            double V;
            double c = T;
            if (condition == 0)
            {
                V = (5.28961729765) * c + (1.39166589782E-2) * c * c + (-2.38855693017E-5) * c * c * c + (3.56916001063E-8) * c * c * c * c + (-4.62347666298E-11) * c * c * c * c * c + (5.00777441034E-14) * c * c * c * c * c * c + (-3.73105886191E-17) * c * c * c * c * c * c * c + (1.57716482367E-20) * c * c * c * c * c * c * c * c + (-2.81038625251E-24) * c * c * c * c * c * c * c * c * c;
            }
            else if (condition == 1)
            {
                V = (2.95157925316E3) + (-2.52061251332) * c + (1.59564501865E-2) * c * c + (-7.64085947576E-6) * c * c * c + (2.05305291024E-9) * c * c * c * c + (-2.93359668173E-13) * c * c * c * c * c;
            }
            else
            {
                V = (1.52232118209E5) + (-2.68819888545E2) * c + (1.71280280471E-1) * c * c + (-3.45895706453E-5) * c * c * c + (-9.34633971046E-12) * c * c * c * c;
            }
            return V;
        }

        private static double CalculateKThermocoupleTempToVolts(double T)
        {
            double V;
            double c = T;
            if (T >= 760.0)
            {
                V = (-1.7600413686E1) + (3.8921204975E1) * c + (1.8558770032E-2) * c * c + (-9.9457592874E-5) * c * c * c + (3.1840945719E-7) * c * c * c * c + (-5.6072844889E-10) * c * c * c * c * c + (5.6075059059E-13) * c * c * c * c * c * c + (-3.2020720003E-16) * c * c * c * c * c * c * c + (9.7151147152E-20) * c * c * c * c * c * c * c * c + (-1.2104721275E-23) * c * c * c * c * c * c * c * c * c + (1.185976E2) * Math.Exp((-1.183432E-4) * (c - 126.9686) * (c - 126.9686));
            }
            else
            {
                V = (3.9450128025E1) * c + (2.3622373598E-2) * c * c + (-3.2858906784E-4) * c * c * c + (-4.9904828777E-6) * c * c * c * c + (-6.7509059173E-8) * c * c * c * c * c + (-5.7410327428E-10) * c * c * c * c * c * c + (-3.1088872894E-12) * c * c * c * c * c * c * c + (-1.0451609365E-14) * c * c * c * c * c * c * c * c + (-1.9889266878E-17) * c * c * c * c * c * c * c * c * c + (-1.6322697486E-20) * c * c * c * c * c * c * c * c * c * c;
            }
            return V;
        }
        private static double CalculateJThermocoupleTempToVolts(double T)
        {
            double V;
            double c = T;
            if (T >= 760.0)
            {
                V = (2.9645625681E5) + (-1.4976127786E3) * c + (3.1787103924) * c * c + (-3.1847686701E-3) * c * c * c + (1.5720819004E-6) * c * c * c * c + (-3.0691369056E-10) * c * c * c * c * c;
            }
            else
            {
                V = V = (5.0381187815E1) * c + (3.0475836930E-2) * c * c + (-8.5681065720E-5) * c * c * c + (1.3228195295E-7) * c * c * c * c + (-1.7052958337E-10) * c * c * c * c * c + (2.0948090697E-13) * c * c * c * c * c * c + (-1.2538395336E-16) * c * c * c * c * c * c * c + (1.5631725697E-20) * c * c * c * c * c * c * c * c;
            }
            return V;
        }
        private static double CalculateEThermocoupleTempToVolts(double T)
        {
            double V;
            double c = T;
            if (T >= 0.0)
            {
                V = V = (5.8665508710E1) * c + (4.5032275585E-2) * c * c + (2.8908407212E-5) * c * c * c + (-3.3056896652E-7) * c * c * c * c + (6.5024403270E-10) * c * c * c * c * c + (-1.9197495504E-13) * c * c * c * c * c * c + (-1.2536600497E-15) * c * c * c * c * c * c * c + (2.1489217569E-18) * c * c * c * c * c * c * c * c + (-1.4388041782E-21) * c * c * c * c * c * c * c * c * c + (3.5960899481E-25) * c * c * c * c * c * c * c * c * c * c;
            }
            else
            {
                V = V = (5.8665508708E1) * c + (4.5410977124E-2) * c * c + (-7.7998048686E-4) * c * c * c + (-2.5800160843E-5) * c * c * c * c + (-5.9452583057E-7) * c * c * c * c * c + (-9.3214058667E-9) * c * c * c * c * c * c + (-1.0287605534E-10) * c * c * c * c * c * c * c + (-8.0370123621E-13) * c * c * c * c * c * c * c * c + (-4.3979497391E-15) * c * c * c * c * c * c * c * c * c + (-1.6414776355E-17) * c * c * c * c * c * c * c * c * c * c + (-3.9673619516E-20) * c * c * c * c * c * c * c * c * c * c * c + (-5.5827328721E-23) * c * c * c * c * c * c * c * c * c * c * c * c + (-3.4657842013E-26) * c * c * c * c * c * c * c * c * c * c * c * c * c;
            }
            return V;
        }
        private static double CalculateBThermocoupleTempToVolts(double T)
        {
            double V;
            double c = T;
            if (T >= 630.615)
            {
                V = (-3.8938168621E3) + (2.8571747470E1) * c + (-8.4885104785E-2) * c * c + (1.5785280164E-4) * c * c * c + (-1.6835344864E-7) * c * c * c * c + (1.1109794013E-10) * c * c * c * c * c + (-4.4515431033E-14) * c * c * c * c * c * c + (9.8975640821E-18) * c * c * c * c * c * c * c + (-9.3791330289E-22) * c * c * c * c * c * c * c * c;
            }
            else
            {
                V = (-2.4650818346E-1) * c + (5.9040421171E-3) * c * c + (-1.3257931636E-6) * c * c * c + (1.5668291901E-9) * c * c * c * c + (-1.6944529240E-12) * c * c * c * c * c + (6.2990347094E-16) * c * c * c * c * c * c;
            }
            return V;
        }
        private static  double ConvertThermistorReading(double CJCVoltage)
        {
            double voltageRef = 2.50;
            double R1 = 189000.00;
            double Irt = (voltageRef - CJCVoltage) / R1;
            double a = (double)1.295361E-3;
            double b = (double)2.343159E-4;
            double c = (double)1.018703E-7;
            double lnRt = Math.Log(CJCVoltage / Irt);
            double T = 1 / (a + lnRt * (b + c * (Math.Pow(lnRt, 2))));
            return (T - 273.15);

        }
        #endregion ThermoCalculations
        #endregion ProgramFunctions
        #region FormEvents
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeRequested = true;
            if (workerThread != null)
            {
                ThreadStopMarker = true;
                e.Cancel = true;
            }
            if (dmmSession != null)
            {

                if (!dmmSession.IsDisposed)
                {
                    dmmSession.Measurement.Abort();
                    dmmSession.Close();
                }
            }
            //Abort scanning.
            if (switchSession != null)
            {
                if (!switchSession.IsDisposed)
                {
                    switchSession.Scan.Abort();
                    switchSession.Close();
                }
            }


        }
        private void startButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                ThreadStopMarker = false;
                ToggleButtonState(false);
                CloseSwitchSession();
                InitializeSwitchSession();
                ConfigureSwitchScan();
                CommitScan();

                //Programming the DMM
                CloseDmmSession();
                InitializeDmmSession();
                ConfigureDmmMeasurement();
                dmmInstrumentModel = GetDmmInstrumentModel();
                //If the DMM model is an NI 4060, add a delay.
                if (dmmInstrumentModel.Equals("PXI-4060") || dmmInstrumentModel.Equals("PCI-4060"))
                {
                    Thread.Sleep(1000);
                }

                InitiateDmmMeasurement();
                CreateDataGridColumns();
                InitiateSwitchScan();

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

        private void CreateDataGridColumns()
        {
            dataGridViewResults.Rows.Clear();
            dataGridViewResults.ColumnCount = 0;
            for (int i = 0; i < NumberOfChannels; i++)
            {
                dataGridViewResults.Columns.Add("", "Temperature Reading " + (i + 1));
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

        private void ChangingControlState()
        {
            ChangeControlState(true);
            ToggleButtonState(true);
        }

      
    }
}
