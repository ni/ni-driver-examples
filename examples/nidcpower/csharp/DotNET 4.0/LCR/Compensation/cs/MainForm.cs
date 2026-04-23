//=============================================================================================================
//
// Title:
//      NI-DCPower LCR Compensation Example
//
// Description:
//      Demonstrates how to perform and apply cable compensation and LCR compensation to your LCR measurements.
//      This example does the following:
//      - Initializes a session
//      - If the Generate LCR Custom Cable Compensation Data checkbox is checked:
//          - Prompts a message to the user to set up an open connection to perform open custom cable compensation.
//          - Prompts a message to the user to set up a short connection to perform short custom cable compensation.
//      - If the Generate LCR Short Compensation Data checkbox is checked:
//          - Sets the cable length configuration to apply cable compensation data when performing LCR compensation.
//          - Prompts the user to set up a short connection to perform short LCR compensation.
//      - If the Generate LCR Open Compensation Data checkbox is checked:
//          - Sets the cable length configuration to apply cable compensation data when performing LCR compensation.
//          - Prompts the user to set up an open connection to perform open LCR compensation.
//      - If the Generate LCR Load Compensation Data checkbox is checked:
//          - Sets configuration to apply cable compensation data when performing LCR compensation.
//          - Prompts the user to set up a load connection to perform load LCR compensation.
//      - Sets the LCR Open/Short/Load Compensation Data Source and applies the LCR compensation data to LCR
//         measurements.
//      - Sets the cable length configuration to apply cable compensation data to LCR sourcing.
//      - Sets the sourcing configurations, initiates generation, waits for the sourcing to complete,
//         and measures the output.
//      - Closes the session.
//
//  Suggested Device(s):
//      PXIe-4190
//
//=============================================================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.LCRCompensation
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
            ConfigureLcrReferenceValueTypeComboBoxColumn();
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

        void ConfigureLcrReferenceValueTypeComboBoxColumn()
        {
            loadCompensationSpotsDataGridView.Columns["columnSpotReferenceValueType"].ValueType = typeof(DCPowerLCRReferenceValueType);
            foreach (DCPowerLCRReferenceValueType item in Enum.GetValues(typeof(DCPowerLCRReferenceValueType)))
            {
                ((DataGridViewComboBoxColumn)loadCompensationSpotsDataGridView.Columns["columnSpotReferenceValueType"]).Items.Add(item);
            }
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

        double ImpedanceRange
        {
            get
            {
                return decimal.ToDouble(this.lcrImpedanceRangeUpDown.Value);
            }
        }

        DCPowerCableLength CableLength
        {
            get
            {
                return (DCPowerCableLength)this.cableLengthComboBox.SelectedItem;
            }
        }

        double AcVoltage
        {
            get
            {
                return decimal.ToDouble(this.lcrVoltageUpDown.Value);
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

        bool GenerateLcrCustomCableCompensationData
        {
            get
            {
                return generateLcrCustomCableCompensationDataCheckBox.Checked;
            }
        }

        bool GenerateLcrOpenCompensationData
        {
            get
            {
                return generateLcrOpenCompensationDataCheckBox.Checked;
            }
        }

        bool GenerateLcrShortCompensationData
        {
            get
            {
                return generateLcrShortCompensationDataCheckBox.Checked;
            }
        }

        bool GenerateLcrLoadCompensationData
        {
            get
            {
                return generateLcrLoadCompensationDataCheckBox.Checked;
            }
        }

        double[] AdditionalFrequencies
        {
            get
            {
                List<double> additionalFrequenciesList = new List<double>();
                foreach (DataGridViewRow row in additionalCompensationFrequenciesDataGridView.Rows)
                {
                    if (!string.IsNullOrEmpty((string)row.Cells[0].Value))
                    {
                        additionalFrequenciesList.Add(Convert.ToDouble(row.Cells[0].Value));
                    }
                }
                return additionalFrequenciesList.ToArray();
            }
        }

        DCPowerLCRLoadCompensationSpot[] LoadCompensationSpots
        {
            get
            {
                List<DCPowerLCRLoadCompensationSpot> loadCompensationSpotsList = new List<DCPowerLCRLoadCompensationSpot>();
                foreach (DataGridViewRow row in loadCompensationSpotsDataGridView.Rows)
                {
                    if
                    (
                        !string.IsNullOrEmpty((string)row.Cells["columnSpotFrequency"].Value)
                            && row.Cells["columnSpotReferenceValueType"].Value != null
                            && !string.IsNullOrEmpty((string)row.Cells["columnSpotReferenceValueA"].Value)
                            && !string.IsNullOrEmpty((string)row.Cells["columnSpotReferenceValueB"].Value)
                    )
                    {
                        loadCompensationSpotsList.Add
                        (
                            new DCPowerLCRLoadCompensationSpot
                            (
                                Convert.ToDouble(row.Cells["columnSpotFrequency"].Value),
                                (DCPowerLCRReferenceValueType)row.Cells["columnSpotReferenceValueType"].Value,
                                Convert.ToDouble(row.Cells["columnSpotReferenceValueA"].Value),
                                Convert.ToDouble(row.Cells["columnSpotReferenceValueB"].Value)
                            )
                        );
                    }
                }
                return loadCompensationSpotsList.ToArray();
            }
        }

        bool EnableLcrOpenCompensation
        {
            get
            {
                return enableLcrOpenCompensationCheckBox.Checked;
            }
        }

        bool EnableLcrShortCompensation
        {
            get
            {
                return enableLcrShortCompensationCheckBox.Checked;
            }
        }

        bool EnableLcrLoadCompensation
        {
            get
            {
                return enableLcrLoadCompensationCheckBox.Checked;
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

                bool isOpenConnectionSetUp = false;
                bool isShortConnectionSetUp = false;

                if (GenerateLcrCustomCableCompensationData)
                {
                    // Call PerformOpenCustomCableCompensation if the user clicks OK. Calling this function resets most session attributes to their default values.
                    if
                    (
                        MessageBox.Show
                        (
                            "Set up an open connection between the LCR Meter's LO CUR, LO POT and HI CUR, HI POT terminals, then click Yes to generate open custom cable compensation data. Click No to skip",
                            "Action is required!",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information
                        )
                            == DialogResult.Yes
                    )
                    {
                        isOpenConnectionSetUp = true;
                        isShortConnectionSetUp = false;
                        dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.Compensation.PerformOpenCustomCableCompensation();
                    }
                    // Call PerformShortCustomCableCompensation if the user clicks OK. Calling this function resets most session properties to their default values.
                    if
                    (
                        MessageBox.Show
                        (
                            "Set up a short connection between the LCR Meter's LO CUR, LO POT and HI CUR, HI POT terminals, then click Yes to generate short custom cable compensation data. Click No to skip",
                            "Action is required!",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information
                        )
                            == DialogResult.Yes
                    )
                    {
                        isOpenConnectionSetUp = false;
                        isShortConnectionSetUp = true;
                        dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.Compensation.PerformShortCustomCableCompensation();
                    }
                }

                if (GenerateLcrShortCompensationData)
                {
                    // Skip the short LCR compensation message if we already have a short connection
                    if (!isShortConnectionSetUp)
                    {
                        MessageBox.Show
                        (
                            "Set up a short connection between the LCR Meter's LO CUR, LO POT and HI CUR, HI POT terminals, then click OK to perform short LCR compensation.",
                            "Action is required!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                        isOpenConnectionSetUp = false;
                        isShortConnectionSetUp = true;
                    }
                    // Set Cable Length before performing short LCR compensation.
                    dcPowerSession.Outputs[FullyQualifiedChannelName].DeviceSpecific.LCR.CableLength = CableLength;
                    // Call PerformShortCompensation. Calling this function resets most session properties to their default values.
                    dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.Compensation.PerformShortCompensation(AdditionalFrequencies);
                }

                if (GenerateLcrOpenCompensationData)
                {
                    // Skip the open LCR compensation message if we already have an open connection
                    if (!isOpenConnectionSetUp)
                    {
                        MessageBox.Show
                        (
                            "Set up an open connection between the LCR Meter's LO CUR, LO POT and HI CUR, HI POT terminals, then click OK to perform open LCR compensation.",
                            "Action is required!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                        isOpenConnectionSetUp = true;
                        isShortConnectionSetUp = false;
                    }
                    // Set Cable Length before performing open LCR compensation.
                    dcPowerSession.Outputs[FullyQualifiedChannelName].DeviceSpecific.LCR.CableLength = CableLength;
                    // Call PerformOpenCompensation. Calling this function resets most session properties to their default values.
                    dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.Compensation.PerformOpenCompensation(AdditionalFrequencies);
                }

                if (GenerateLcrLoadCompensationData)
                {
                    MessageBox.Show
                    (
                        "Connect the reference load between the LCR Meter's LO CUR, LO POT and HI CUR, HI POT terminals, then click OK to perform load LCR compensation.",
                        "Action is required!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    // Set channel properties to the runtime settings before performing load LCR compensation.
                    // This acquires the load compensation data with the same configuration as used to test the DUT.
                    dcPowerSession.Outputs[FullyQualifiedChannelName].DeviceSpecific.LCR.CableLength = CableLength;
                    dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.VoltageAmplitude = AcVoltage;
                    dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.ImpedanceRange = ImpedanceRange;
                    dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.DCBiasSource = LcrDcBiasSource;
                    dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.DCBiasVoltageLevel = DcBiasVoltage;
                    dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.DCBiasCurrentLevel = DcBiasCurrent;
                    dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.MeasurementTime = LcrMeasurementTime;
                    dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.CustomMeasurementTime = LcrCustomMeasurementTime;
                    // Call PerformLoadCompensation. Calling this function resets most session properties to their default values.
                    dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.Compensation.PerformLoadCompensation(LoadCompensationSpots);
                }

                if
                (
                    GenerateLcrCustomCableCompensationData
                        || GenerateLcrOpenCompensationData
                        || GenerateLcrShortCompensationData
                        || GenerateLcrLoadCompensationData
                )
                {
                    MessageBox.Show
                    (
                        "Set up a connection between the LCR meter and your DUT. Then click OK to proceed to take LCR measurements.",
                        "Action is required!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                dcPowerSession.Outputs[FullyQualifiedChannelName].InstrumentMode = DCPowerInstrumentMode.LCR;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.StimulusFunction = DCPowerLCRStimulusFunction.ACVoltage;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.Frequency = Frequency;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.ImpedanceRange = ImpedanceRange;
                dcPowerSession.Outputs[FullyQualifiedChannelName].DeviceSpecific.LCR.CableLength = CableLength;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.VoltageAmplitude = AcVoltage;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.DCBiasSource = LcrDcBiasSource;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.DCBiasVoltageLevel = DcBiasVoltage;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.DCBiasCurrentLevel = DcBiasCurrent;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.MeasurementTime = LcrMeasurementTime;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.CustomMeasurementTime = LcrCustomMeasurementTime;

                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.Compensation.OpenShortLoadCompensationDataSource = DCPowerLCROpenShortLoadCompensationDataSource.OnboardStorage;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.Compensation.OpenCompensationEnabled = EnableLcrOpenCompensation;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.Compensation.ShortCompensationEnabled = EnableLcrShortCompensation;
                dcPowerSession.Outputs[FullyQualifiedChannelName].LCR.Compensation.LoadCompensationEnabled = EnableLcrLoadCompensation;

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
            this.generateLcrCompensationDataGroupBox.Enabled = isEnabled;
            this.applyCompensationDataGroupBox.Enabled = isEnabled;
            this.startButton.Enabled = isEnabled;
            this.resourceNameComboBox.Select();
            this.Refresh();
        }
    }
}