//=============================================================================================================
//
// Title:
//      NI-DCPower LCR Source AC Current Example
//
// Description:
//      Demonstrates how to use the LCR AC Current Output Function to force an output current. This example
//      initializes a session; configures the Instrument Mode, LCR Stimulus Function, LCR Frequency, LCR
//      Current Amplitude, LCR Impedance Autorange, LCR Impedance Range, LCR DC Bias Source,
//      LCR DC Bias Current Level, LCR Measurement Time, Cable Length, LCR Source Delay Mode and Source Delay;
//      initiates generation, waits for a specified delay; and measures the output.
//
//  Suggested Device(s):
//      PXIe-4190
//
//=============================================================================================================

using System;
using System.Drawing;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.LCRSourceACCurrent
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
            ConfigureLcrSourceDelayModeComboBox();
            ConfigureLcrImpedanceAutorangeComboBox();
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

        void ConfigureLcrSourceDelayModeComboBox()
        {
            foreach (DCPowerLCRSourceDelayMode item in Enum.GetValues(typeof(DCPowerLCRSourceDelayMode)))
            {
                lcrSourceDelayModeComboBox.Items.Add(item);
            }
            lcrSourceDelayModeComboBox.SelectedIndex = 0;
        }

        void ConfigureLcrImpedanceAutorangeComboBox()
        {
            foreach (DCPowerLCRImpedanceAutorange item in Enum.GetValues(typeof(DCPowerLCRImpedanceAutorange)))
            {
                lcrImpedanceAutorangeComboBox.Items.Add(item);
            }
            lcrImpedanceAutorangeComboBox.SelectedItem = DCPowerLCRImpedanceAutorange.On;
        }
        #endregion

        #region MainForm configuration values

        string FullyQualifiedChannelName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        double Frequency
        {
            get
            {
                return decimal.ToDouble(this.lcrFrequencyUpDown.Value);
            }
        }

        DCPowerCableLength CableLength
        {
            get
            {
                return (DCPowerCableLength)this.cableLengthComboBox.SelectedItem;
            }
        }

        DCPowerLCRImpedanceAutorange ImpedanceAutorange
        {
            get
            {
                return (DCPowerLCRImpedanceAutorange)this.lcrImpedanceAutorangeComboBox.SelectedItem;
            }
        }

        double ImpedanceRange
        {
            get
            {
                return decimal.ToDouble(this.lcrImpedanceRangeUpDown.Value);
            }
        }

        double AcCurrent
        {
            get
            {
                return decimal.ToDouble(this.lcrCurrentUpDown.Value);
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
                return decimal.ToDouble(this.lcrDcBiasVoltageUpDown.Value);
            }
        }

        double DcBiasCurrent
        {
            get
            {
                return decimal.ToDouble(this.lcrDcBiasCurrentUpDown.Value);
            }
        }

        DCPowerLCRMeasurementTime LcrMeasurementTime
        {
            get
            {
                return (DCPowerLCRMeasurementTime)this.lcrMeasurementTimeComboBox.SelectedItem;
            }
        }

        double LcrCustomMeasurementTime
        {
            get
            {
                return decimal.ToDouble(this.lcrCustomMeasurementTimeUpDown.Value);
            }
        }

        DCPowerLCRSourceDelayMode LcrSourceDelayMode
        {
            get
            {
                return (DCPowerLCRSourceDelayMode)this.lcrSourceDelayModeComboBox.SelectedItem;
            }
        }

        PrecisionTimeSpan SourceDelay
        {
            get
            {
                return new PrecisionTimeSpan(decimal.ToDouble(this.sourceDelayUpDown.Value));
            }
        }

        #endregion

        void startButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            Start();
            ChangeControlState(true);
        }

        void mainForm_Closing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
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

            try
            {
                InitializeDCPowerSession();

                dcPowerSession.Outputs[FullyQualifiedChannelName].InstrumentMode = DCPowerInstrumentMode.LCR;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.StimulusFunction = DCPowerLCRStimulusFunction.ACCurrent;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.Frequency = Frequency;
                dcPowerSession.Outputs[FullyQualifiedChannelName].DeviceSpecific.LCR.CableLength = CableLength;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.ImpedanceAutoRange = ImpedanceAutorange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.ImpedanceRange = ImpedanceRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.CurrentAmplitude = AcCurrent;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.DCBiasSource = LcrDcBiasSource;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.DCBiasVoltageLevel = DcBiasVoltage;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.DCBiasCurrentLevel = DcBiasCurrent;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.MeasurementTime = LcrMeasurementTime;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.CustomMeasurementTime = LcrCustomMeasurementTime;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.SourceDelayMode = LcrSourceDelayMode;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.SourceDelay = SourceDelay;

                dcPowerSession.Control.Initiate();

                // Wait for output to settle.
                dcPowerSession.Events.SourceCompleteEvent.WaitForEvent(new PrecisionTimeSpan(21.0));

                NILCRMeasurement[] results = dcPowerSession.Measurement.MeasureLCR(FullyQualifiedChannelName);

                // Though this example can return multiple measurements from multiple channels, it is designed
                // to display only the first measurement taken across any number of channels. You can adapt the
                // example to display multiple measurements from multiple channels.
                DisplayMeasurements(results[0]);

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

        void InitializeDCPowerSession()
        {
            dcPowerSession = new NIDCPower(FullyQualifiedChannelName, false, String.Empty);
            dcPowerSession.DriverOperation.Warning += new EventHandler<DCPowerWarningEventArgs>(DCPowerDriverOperationWarning);
        }

        void DCPowerDriverOperationWarning(object sender, DCPowerWarningEventArgs e)
        {
            MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        void DisplayMeasurements(NILCRMeasurement results)
        {
            this.vdcTextBox.Text = results.Vdc.ToString("E");
            this.idcTextBox.Text = results.Idc.ToString("E");
            this.stimulusFrequencyTextBox.Text = results.stimulusFrequency.ToString("E");
            this.acVoltageTextBox.Text = results.ACVoltage.ToString("E");
            this.acCurrentTextBox.Text = results.ACCurrent.ToString("E");
            this.zTextBox.Text = results.Z.ToString("E");
            this.zMagTextBox.Text = results.ZMagnitude.ToString("E");
            this.zThetaTextBox.Text = results.ZPhase.ToString("E");
            this.yTextBox.Text = results.Y.ToString("E");
            this.yMagTextBox.Text = results.YMagnitude.ToString("E");
            this.yThetaTextBox.Text = results.YPhase.ToString("E");
            this.lsTextBox.Text = results.Ls.ToString("E");
            this.csTextBox.Text = results.Cs.ToString("E");
            this.rsTextBox.Text = results.Rs.ToString("E");
            this.lpTextBox.Text = results.Lp.ToString("E");
            this.cpTextBox.Text = results.Cp.ToString("E");
            this.rpTextBox.Text = results.Rp.ToString("E");
            this.dTextBox.Text = results.D.ToString("E");
            this.qTextBox.Text = results.Q.ToString("E");
            this.measurementModeTextBox.Text = results.measurementMode.ToString();
            this.dcInComplianceButtonLed.BackColor = results.dcInCompliance ? Color.Red : SystemColors.Control;
            this.acInComplianceButtonLed.BackColor = results.acInCompliance ? Color.Red : SystemColors.Control;
            this.unbalancedButtonLed.BackColor = results.unbalanced ? Color.Red : SystemColors.Control;
        }

        static void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void ChangeControlState(bool isEnabled)
        {
            this.configurationGroupBox.Enabled = isEnabled;
            this.startButton.Enabled = isEnabled;
            this.resourceNameComboBox.Select();
            this.Refresh();
        }
    }
}