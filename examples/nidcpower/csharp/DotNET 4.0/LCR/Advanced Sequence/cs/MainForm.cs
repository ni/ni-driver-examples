//=============================================================================================================
//
// Title:
//      NI-DCPower LCR Advanced Sequence Frequency Sweep
//
// Description:
//      Demonstrates how to make sweep times faster by using LCR Advanced Sequence
//      functions to change the frequency in a sequence. During the sequence, the
//      channel steps through the predetermined set configurations (frequency in
//      this example) without any interaction with the host system. The advantage
//      of sequencing is that changes from one step in the sequence to the next
//      are deterministic. The program will perform an AC Voltage frequency sweep
//      from Start Frequency to End Frequency. The sweep frequencies are an
//      exponential sequence and the length of each of the sweeps is equal to the
//      value in the Number Of Steps control.
//
//  Suggested Device(s):
//      PXIe-4190
//
//============================================================================================================
using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.LCRAdvancedSequenceFrequencySweep
{
    public partial class MainForm : Form
    {
        NIDCPower dcPowerSession;

        public MainForm()
        {
            InitializeComponent();
            ConfigureMeasurementTimeComboBox();
            ConfigureCableLengthComboBox();
            ConfigureDcBiasSourceComboBox();
            LoadDCPowerDeviceNames();
        }

        #region MainForm initial configuration

        void LoadDCPowerDeviceNames()
        {
            using (ModularInstrumentsSystem dcPowerDevices = new ModularInstrumentsSystem("NI-DCPower"))
            {
                foreach (DeviceInfo device in dcPowerDevices.DeviceCollection)
                {
                    resourceNameComboBox.Items.Add(device.Name + "/0");
                }
            }
            if (resourceNameComboBox.Items.Count > 0)
            {
                resourceNameComboBox.SelectedIndex = 0;
            }
        }

        void ConfigureMeasurementTimeComboBox()
        {
            foreach (DCPowerLCRMeasurementTime item in Enum.GetValues(typeof(DCPowerLCRMeasurementTime)))
            {
                lcrMeasurementTimeComboBox.Items.Add(item);
            }
            lcrMeasurementTimeComboBox.SelectedIndex = 1;
        }

        void ConfigureCableLengthComboBox()
        {
            foreach (DCPowerCableLength item in Enum.GetValues(typeof(DCPowerCableLength)))
            {
                cableLengthComboBox.Items.Add(item);
            }
            cableLengthComboBox.SelectedIndex = 1;  // NIStandard1M
        }

        void ConfigureDcBiasSourceComboBox()
        {
            foreach (DCPowerLCRDCBiasSource item in Enum.GetValues(typeof(DCPowerLCRDCBiasSource)))
            {
                lcrDcBiasSourceComboBox.Items.Add(item);
            }
            lcrDcBiasSourceComboBox.SelectedIndex = 0;
        }
        #endregion

        #region Mainform configuration values

        string ResourceName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        string FullyQualifiedChannelName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        double StartFrequency
        {
            get
            {
                return decimal.ToDouble(this.startFrequencyNumeric.Value);
            }
        }

        double EndFrequency
        {
            get
            {
                return decimal.ToDouble(this.endFrequencyNumeric.Value);
            }
        }

        UInt32 NumberOfSteps
        {
            get
            {
                return decimal.ToUInt32(this.numberOfStepsNumeric.Value);
            }
        }

        DCPowerCableLength CableLength
        {
            get
            {
                return (DCPowerCableLength)this.cableLengthComboBox.SelectedItem;
            }
        }

        double ImpedanceRange
        {
            get
            {
                return decimal.ToDouble(this.lcrImpedanceRangeNumeric.Value);
            }
        }

        double AcVoltage
        {
            get
            {
                return decimal.ToDouble(this.VoltageAmplitudeRmsNumeric.Value);
            }
        }

        DCPowerLCRDCBiasSource LcrDcBiasSource
        {
            get
            {
                return (DCPowerLCRDCBiasSource)this.lcrDcBiasSourceComboBox.SelectedItem;
            }
        }

        double DcBiasVoltage
        {
            get
            {
                return decimal.ToDouble(this.lcrDcBiasVoltageLevelNumeric.Value);
            }
        }

        double DcBiasCurrent
        {
            get
            {
                return decimal.ToDouble(this.lcrDcBiasCurrentLevelNumeric.Value);
            }
        }

        DCPowerLCRMeasurementTime LcrMeasurementTime
        {
            get
            {
                return (DCPowerLCRMeasurementTime)this.lcrMeasurementTimeComboBox.SelectedItem;
            }
        }

        double LcrCustomMeasurementTimeSec
        {
            get
            {
                return decimal.ToDouble(this.lcrCustomMeasurementTimeSecNumeric.Value);
            }
        }

        bool LcrOpenCompensationEnabled
        {
            get
            {
                return lcrOpenCompensationEnabledCheckBox.Checked;
            }
        }

        bool LcrShortCompensationEnabled
        {
            get
            {
                return lcrShortCompensationEnabledCheckBox.Checked;
            }
        }
        #endregion

        void startButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            ClearMeasurementsDataGridView();
            Start();
            ChangeControlState(true);
        }

        void Start()
        {
            if (CableLength == DCPowerCableLength.CustomAsConfigured)
            {
                MessageBox.Show
                (
                    "Cable Length - (Custom) As Configured is selected. This cable length option is not supported in this example. Search ni.com for information about NI power supplies/SMUs and the NI-DCPower API.",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            /* Specify the Advanced Sequence Attributes which can change per step. */
            DCPowerAdvancedSequenceProperty[] advancedSequenceProperties = {DCPowerAdvancedSequenceProperty.LcrFrequency};

            try
            {

                InitializeDCPowerSession();

                double[] frequenciesSequence = new double[NumberOfSteps];

                dcPowerSession.Source.Mode = DCPowerSourceMode.Sequence;

                dcPowerSession.Outputs[FullyQualifiedChannelName].InstrumentMode = DCPowerInstrumentMode.LCR;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.StimulusFunction = DCPowerLCRStimulusFunction.ACVoltage;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.VoltageAmplitude = AcVoltage;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.ImpedanceRange = ImpedanceRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.DCBiasSource = LcrDcBiasSource;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.DCBiasVoltageLevel = DcBiasVoltage;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.DCBiasCurrentLevel = DcBiasCurrent;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.MeasurementTime = LcrMeasurementTime;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.CustomMeasurementTime = LcrCustomMeasurementTimeSec;
                dcPowerSession.Outputs[FullyQualifiedChannelName].DeviceSpecific.LCR.CableLength = CableLength;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.Compensation.OpenCompensationEnabled = LcrOpenCompensationEnabled;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.Compensation.ShortCompensationEnabled = LcrShortCompensationEnabled;

                string advancedSequenceName = "MySequence";
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.AdvancedSequencing.CreateAdvancedSequence(advancedSequenceName, advancedSequenceProperties, true);

                double minFrequency = (StartFrequency < EndFrequency) ? StartFrequency : EndFrequency;
                double maxFrequency = (StartFrequency > EndFrequency) ? StartFrequency : EndFrequency;
                double nthRootFrequencyRatio = Math.Pow(maxFrequency / minFrequency, 1.0 / (NumberOfSteps - 1));
                double frequenciesSequenceValue;
                for (int pointIndex = 0; pointIndex < NumberOfSteps; ++pointIndex)
                {
                    /* Create new Advanced Sequence Step for every frequency. */
                    dcPowerSession.Outputs[FullyQualifiedChannelName].Source.AdvancedSequencing.CreateAdvancedSequenceStep(true);
                    /* Calculate the Frequency for this step. */
                    frequenciesSequenceValue = Math.Pow(nthRootFrequencyRatio, pointIndex) * minFrequency;

                    dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.Frequency = frequenciesSequenceValue;
                }


                dcPowerSession.Control.Initiate();

                PrecisionTimeSpan timeout = new PrecisionTimeSpan(300.0);
                /* Wait for the sequence engine to be done. */
                dcPowerSession.Outputs[FullyQualifiedChannelName].Events.SequenceEngineDoneEvent.WaitForEvent(timeout);

                NILCRMeasurement[] results = dcPowerSession.Measurement.FetchLCR(FullyQualifiedChannelName, timeout, Convert.ToInt32(NumberOfSteps));
                DisplayMeasurements(results);

                dcPowerSession.Outputs[FullyQualifiedChannelName].Control.Abort();
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.AdvancedSequencing.DeleteAdvancedSequence(advancedSequenceName);

                dcPowerSession.Utility.Reset(FullyQualifiedChannelName);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            finally
            {
                CloseSession();
            }
        }

        void DisplayMeasurements(NILCRMeasurement[] results)
        {
            for (int i = 0; i < results.Length; i++)
            {
                measurementsDataGridView.Rows.Add(
                    (i + 1).ToString(),
                    results[i].stimulusFrequency.ToString("E"),
                    results[i].ZMagnitude.ToString("E"));
            }
        }

        void InitializeDCPowerSession()
        {
            dcPowerSession = new NIDCPower(FullyQualifiedChannelName, false, String.Empty);
            dcPowerSession.DriverOperation.Warning += new EventHandler<DCPowerWarningEventArgs>(DCPowerDriverOperationWarning);
        }

        void DCPowerDriverOperationWarning(object sender, DCPowerWarningEventArgs e)
        {
            MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void mainForm_Closing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        void ClearMeasurementsDataGridView()
        {
            measurementsDataGridView.Rows.Clear();
        }

        void CloseSession()
        {
            if (dcPowerSession != null)
            {
                try
                {
                    dcPowerSession.Close();
                    dcPowerSession = null;
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                    Application.Exit();
                }
            }
        }

        static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void ChangeControlState(bool isEnabled)
        {
            configurationGroupBox.Enabled = isEnabled;
            startButton.Enabled = isEnabled;
            this.Refresh();
        }
    }
}