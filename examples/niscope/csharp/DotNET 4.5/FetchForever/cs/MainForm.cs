//=====================================================================================================
//
// Title:
//        Fetch Forever
//
// Description:
//      The application demonstrates how to fetch data records continuously from a NI-SCOPE device.
//      It uses a Memory Optimized asynchronous version of Fetch. The program continues fetching 
//      until the stop button is pressed or an exception is thrown.
//
//==================================================================================================

using System;
using System.Windows.Forms;
using System.Data;
using NationalInstruments.ModularInstruments.NIScope;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;
using System.Text.RegularExpressions;

namespace NationalInstruments.Examples.FetchForever
{
    public partial class MainForm : Form
    {
        NIScope scopeSession;
        bool stop;

        public MainForm()
        {
            InitializeComponent();
            LoadScopeDeviceNames();
            ChangeControlState(true);
        }

        #region Mainform initial configuration
        void LoadScopeDeviceNames()
        {
            using (ModularInstrumentsSystem scopeDevices = new ModularInstrumentsSystem("NI-Scope"))
            {
                foreach (DeviceInfo device in scopeDevices.DeviceCollection)
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

        #region Mainform configuration values
        string ResourceName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        string ChannelName
        {
            get
            {
                int val;
                //Strip off the device name from the fully qualified channel name (i.e. convert Dev1/0 to 0)
                string unqualifiedChannelName = Regex.Replace(this.channelNameTextBox.Text, "[^,]+/", "");
                //Check the unqualified channel name to verify it is a single channel with only digits
                if (!Int32.TryParse(unqualifiedChannelName, out val))
                {
                    throw new ArgumentException("The channel name specified is either invalid or multiple channels are specified.\n\r\n\rThis example supports only 1 channel.");
                }
                return this.channelNameTextBox.Text;
            }
        }

        double SampleRateMin
        {
            get
            {
                return decimal.ToDouble(this.minSampleRateNumeric.Value);
            }
        }

        double Range
        {
            get
            {
                return decimal.ToDouble(this.verticalRangeNumeric.Value);
            }
        }

        long PointsToFetchMax
        {
            get
            {
                return decimal.ToInt64(this.maxPointsFetchedNumeric.Value);
            }
        }

        string TotalPointsFetched
        {
            set
            {
                this.totalPointsFetchedTextBox.Text = value;
            }
        }

        string LastFetchPoints
        {
            set
            {
                this.lastFetchedPointsTextBox.Text = value;
            }
        }
        #endregion

        void acquireButton_Click(object sender, System.EventArgs e)
        {
            FetchForever();
        }

        void FetchForever()
        {
            stop = false;
            ChangeControlState(false);
            DisplayMessage("Acquisition is in progress...");

            try
            {
                InitializeSession();

                // Configure the vertical parameters.
                double offset = 0.0;
                ScopeVerticalCoupling coupling = ScopeVerticalCoupling.DC;
                double probeAttenuation = 1.0;
                scopeSession.Channels[ChannelName].Configure(Range, offset, coupling, probeAttenuation, true);

                // Configure the horizontal parameters.
                int recordLengthMin = 1;
                double referencePosition = 0.0;
                int numberOfRecords = 1;
                bool enforceRealtime = true;
                scopeSession.Timing.ConfigureTiming(SampleRateMin, recordLengthMin, referencePosition, numberOfRecords, enforceRealtime);

                // Configure software trigger, but never send the trigger.
                // This starts an infinite acquisition, until you call niScope_Abort or niScope_close.
                scopeSession.Trigger.ConfigureTriggerSoftware(PrecisionTimeSpan.Zero, PrecisionTimeSpan.Zero);
                scopeSession.Measurement.Initiate();

                long totalPointsFetched = 0;
                scopeSession.Measurement.FetchRelativeTo = ScopeFetchRelativeTo.ReadPointer;
                while (!stop)
                {
                    AnalogWaveformCollection<double> waveforms = null;
                    ScopeWaveformInfo[] waveformInfo = null;
                    waveforms = scopeSession.Channels[ChannelName].Measurement.FetchDouble(PrecisionTimeSpan.Zero, PointsToFetchMax, waveforms, out waveformInfo);
                    totalPointsFetched += waveformInfo[0].ActualNumberOfSamples;
                    UpdateOutputResults(totalPointsFetched, waveformInfo[0].ActualNumberOfSamples);
                    PlotWaveforms(waveformDataGridView, waveforms[0], waveformInfo[0].ActualNumberOfSamples);
                    Application.DoEvents();
                }
                DisplayMessage("Acquisition successful!!!");
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            finally
            {
                ChangeControlState(true);
                CloseSession();
            }
        }

        void ClearWaveforms()
        {
            waveformDataGridView.Columns.Clear();
        }

        static void PlotWaveforms(DataGridView dgv, AnalogWaveform<double> waveform, long actualNumberOfSamples)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Index", typeof(int));
            table.Columns.Add("Waveform", typeof(double));

            for (int idx = 0; idx < actualNumberOfSamples; idx++)
            {
                table.Rows.Add(idx + 1, waveform.Samples[idx].Value);
            }

            dgv.DataSource = table;
        }

        void UpdateOutputResults(long totalPointsFetched, long actualNumberOfSamples)
        {
            TotalPointsFetched = totalPointsFetched.ToString();
            LastFetchPoints = actualNumberOfSamples.ToString();
        }

        void mainForm_Closing(object sender, FormClosingEventArgs e)
        {
            CloseSession();
        }

        void stopButton_Click(object sender, System.EventArgs e)
        {
            if (!stop)
            {
                stop = true;
                DisplayMessage("Stop in progress...Fetched points are being plotted...");
            }
        }

        void DisplayMessage(string message)
        {
            messageTextBox.Text = message;
            this.Refresh();
        }

        void InitializeSession()
        {
            scopeSession = new NIScope(ResourceName, false, false);
            scopeSession.DriverOperation.Warning += new EventHandler<ScopeWarningEventArgs>(DriverOperation_Warning);
        }

        void DriverOperation_Warning(object sender, ScopeWarningEventArgs e)
        {
            messageTextBox.Clear();
            MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        void ChangeControlState(bool isEnabled)
        {
            generalGroupBox.Enabled = isEnabled;
            acquireButton.Enabled = isEnabled;
            stopButton.Enabled = !isEnabled;
            if (!isEnabled)
            {
                ClearWaveforms();
            }
            this.Refresh();
        }

        void CloseSession()
        {
            if (scopeSession != null)
            {
                try
                {
                    scopeSession.Close();
                    scopeSession = null;
                }
                catch (Exception ex)
                {
                    ShowError(ex);
                    Application.Exit();
                }
            }
        }

        void ShowError(Exception ex)
        {
            messageTextBox.Clear();
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
