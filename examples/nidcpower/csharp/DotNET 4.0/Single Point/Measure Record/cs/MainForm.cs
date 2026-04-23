//============================================================================================================
//
// Title:
//      NI-DCPower Measure Record
//
// Description:
//      This example demonstrates how to take multiple measurements in succesion
//      by using a measure record.
//
//      Note: In this example the Output Function is set to DC Voltage. If you change the
//      Output Function to DC Current, you must use Current Level and Voltage Limit instead
//      of Voltage Level and Current Limit.
//
//  Suggested Devices:
//      PXI-4132
//      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4154
//
//============================================================================================================

using System;
using System.Threading;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.MeasureRecord
{
    public partial class MainForm : Form
    {
        NIDCPower dcPowerSession;
        bool stopFetch;

        public MainForm()
        {
            InitializeComponent();
            ConfigureAutoZeroComboBox();
            LoadDCPowerDeviceNames();
        }

        #region MainForm initial configuration
        void ConfigureAutoZeroComboBox()
        {
            foreach (DCPowerMeasurementAutoZero item in Enum.GetValues(typeof(DCPowerMeasurementAutoZero)))
            {
                autoZeroComboBox.Items.Add(item);
            }
            autoZeroComboBox.SelectedIndex = 0;
        }

        void LoadDCPowerDeviceNames()
        {
            using (ModularInstrumentsSystem dcPowerDevices = new ModularInstrumentsSystem("NI-DCPower"))
            {
                foreach (DeviceInfo device in dcPowerDevices.DeviceCollection)
                {
                    resourceNameComboBox.Items.Add(device.Name);
                }
            }
            if (resourceNameComboBox.Items.Count > 0)
            {
                resourceNameComboBox.SelectedIndex = 0;
            }
        }
        #endregion

        #region MainForm initial values
        string ResourceName
        {
            get
            {
                return GetResourceNameSafe();
            }
        }

        string ChannelName
        {
            get
            {
                return GetChannelNameSafe();
            }
        }

        string FullyQualifiedChannelName
        {
            get
            {
                return $"{ResourceName}/{ChannelName}";
            }
        }

        double VoltageLevel
        {
            get
            {
                return GetVoltageLevelSafe();
            }
        }

        int MeasureRecordLength
        {
            get
            {
                return GetMeasureRecordLengthSafe();
            }
        }

        bool IsMeasureRecordFinite
        {
            get
            {
                return GetIsMeasureRecordFiniteSafe();
            }
        }

        DCPowerMeasurementAutoZero AutoZero
        {
            get
            {
                return GetAutoZeroSafe();
            }
        }
        #endregion

        #region Thread-safe calls to MainForm controls
        delegate string GetResourceNameSafeCallBack();
        string GetResourceNameSafe()
        {
            if (this.resourceNameComboBox.InvokeRequired)
            {
                return this.Invoke(new GetResourceNameSafeCallBack(GetResourceNameSafe), null).ToString();
            }
            else
            {
                return this.resourceNameComboBox.Text;
            }
        }

        delegate string GetChannelNameSafeCallBack();
        string GetChannelNameSafe()
        {
            if (this.channelNameTextBox.InvokeRequired)
            {
                return this.Invoke(new GetChannelNameSafeCallBack(GetChannelNameSafe), null).ToString();
            }
            else
            {
                return this.channelNameTextBox.Text;
            }
        }

        delegate double GetVoltageLevelSafeCallBack();
        double GetVoltageLevelSafe()
        {
            if (this.voltageLevelNumeric.InvokeRequired)
            {
                return (double)this.Invoke(new GetVoltageLevelSafeCallBack(GetVoltageLevelSafe), null);
            }
            else
            {
                return decimal.ToDouble(this.voltageLevelNumeric.Value);
            }
        }

        delegate int GetMeasureRecordLengthSafeCallBack();
        int GetMeasureRecordLengthSafe()
        {
            if (this.measureRecordLengthNumeric.InvokeRequired)
            {
                return (int)this.Invoke(new GetMeasureRecordLengthSafeCallBack(GetMeasureRecordLengthSafe), null);
            }
            else
            {
                return decimal.ToInt32(this.measureRecordLengthNumeric.Value);
            }
        }

        delegate bool GetIsMeasureRecordFiniteCallBack();
        bool GetIsMeasureRecordFiniteSafe()
        {
            if (this.isMeasureRecordFiniteCheckBox.InvokeRequired)
            {
                return (bool)this.Invoke(new GetIsMeasureRecordFiniteCallBack(GetIsMeasureRecordFiniteSafe), null);
            }
            else
            {
                return this.isMeasureRecordFiniteCheckBox.Checked;
            }
        }

        delegate DCPowerMeasurementAutoZero GetAutoZeroSafeCallBack();
        DCPowerMeasurementAutoZero GetAutoZeroSafe()
        {
            if (this.autoZeroComboBox.InvokeRequired)
            {
                return (DCPowerMeasurementAutoZero)this.Invoke(new GetAutoZeroSafeCallBack(GetAutoZeroSafe), null);
            }
            else
            {
                return (DCPowerMeasurementAutoZero)this.autoZeroComboBox.SelectedItem;
            }
        }

        delegate void ChangeControlStateSafeCallBack(bool isEnabled);
        void ChangeControlState(bool isEnabled)
        {
            if (resourceNameComboBox.InvokeRequired)
            {
                this.Invoke(new ChangeControlStateSafeCallBack(ChangeControlState), new object[] { isEnabled });
            }
            else
            {
                resourceNameAndChannelNameGroupBox.Enabled = isEnabled;
                configurationGroupBox.Enabled = isEnabled;
                startButton.Enabled = isEnabled;
                stopButton.Enabled = !isEnabled;
                resourceNameComboBox.Select();
                this.Refresh();
            }
        }

        delegate void UpdateMeasurementsSafeCallBack(double[] voltageMeasurements, double[] currentMeasurements);
        void UpdateMeasurements(double[] voltageMeasurements, double[] currentMeasurements)
        {
            if (measurementRateTextBox.InvokeRequired)
            {
                this.Invoke(new UpdateMeasurementsSafeCallBack(UpdateMeasurements), new object[] { voltageMeasurements, currentMeasurements });
            }
            else
            {
                for (int i = 0; i < voltageMeasurements.Length && i < currentMeasurements.Length; i++)
                {
                    measurementsDataGridView.Rows.Add(
                        (measurementsDataGridView.Rows.Count + 1).ToString(),
                        voltageMeasurements[i].ToString("E"),
                        currentMeasurements[i].ToString("E"));
                }
            }
        }

        delegate void UpdateMeasurementRateCallBack(double measurementRate);
        void UpdateMeasurementRate(double measurementRate)
        {
            if (measurementRateTextBox.InvokeRequired)
            {
                this.Invoke(new UpdateMeasurementRateCallBack(UpdateMeasurementRate), new object[] { measurementRate });
            }
            else
            {
                measurementRateTextBox.Text = measurementRate.ToString("E");
            }
        }
        #endregion

        void startButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            ClearMeasurementsDataGridView();
            stopFetch = false;

            // Create a new thread to perform acquisition.
            ThreadPool.QueueUserWorkItem(AcquisitionThreadFunction);
        }

        void mainForm_Closing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        void stopButton_Click(object sender, EventArgs e)
        {
            stopFetch = true;
        }

        void AcquisitionThreadFunction(object userState)
        {
            try
            {
                InitializeDCPowerSession();

                dcPowerSession.Source.Mode = DCPowerSourceMode.SinglePoint;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Source.Voltage.VoltageLevel = VoltageLevel;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Measurement.RecordLength = MeasureRecordLength;

                dcPowerSession.Outputs[FullyQualifiedChannelName].Measurement.IsRecordLengthFinite = IsMeasureRecordFinite;
                dcPowerSession.Outputs[FullyQualifiedChannelName].Measurement.MeasureWhen = DCPowerMeasurementWhen.AutomaticallyAfterSourceComplete;

                dcPowerSession.Outputs[FullyQualifiedChannelName].Measurement.AutoZero = AutoZero;
                dcPowerSession.Control.Commit();

                // Display measurement rate in Mainform.
                double measureRecordDeltaTime = dcPowerSession.Outputs[FullyQualifiedChannelName].Measurement.RecordDeltaTime;
                UpdateMeasurementRate(1.0 / measureRecordDeltaTime);

                dcPowerSession.Control.Initiate();

                int backlog = 0;
                int totalMeasurementsFetched = 0;
                while (!stopFetch && (!IsMeasureRecordFinite || totalMeasurementsFetched < MeasureRecordLength))
                {
                    backlog = dcPowerSession.Measurement.FetchBacklog;
                    if (backlog > 0)
                    {
                        DCPowerFetchResult result = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, new PrecisionTimeSpan(1.0), backlog);
                        totalMeasurementsFetched += result.VoltageMeasurements.Length;
                        UpdateMeasurements(result.VoltageMeasurements, result.CurrentMeasurements);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            finally
            {
                CloseSession();
                ChangeControlState(true);
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
    }
}