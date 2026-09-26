/*******************************************************************************
*
* Example program:
*   Frequency Sweep
*
* Category:
*   NI-RFSA
*
* Description:
*   Use this example to learn how to acquire a spectrum using the RF vector
	signal analyzer. This example shows how to configure NI-RFSA for spectrum
	acquisition, how to set the resolution bandwidth and reference level, and
	how to read power spectra while changing the center frequency in each
	iteration of the sweep while keeping the span constant. The spectrum is
	displayed on the datagrid. Note: This example illustrates how to perform
	multiple spectrum acquisitions with different center frequency values. If
	you need to acquire a spectrum with a span wider than the instantaneous
	bandwidth of the device, use the Getting Started Spectrum example.
*
* Instructions for running:
*   1. Configure RFSA device in the MAX for the program to run. 
*
*	2. Configure the Reference Level in the UI. 
*		The reference level represents the maximum expected power of an input RF signal.
*
*	3. Configure the Start Center Frequency in the UI.
*
*	4. Configure the Stop Center Frequency in the UI.
*
*   5. Configure the Resolution Bandwidth in the UI.
*
*	6. Configure the Number of Steps in the UI.
*	
*   7. Select the start button to start the spectrum acquisition.
*	
*   8. The data is displayed in the DataGrid.
*  
* I/O Connections Overview:
*   Make sure your signal input terminals match the Physical Channel I/O
*   Controls.  If you have a PXI chassis, ensure that it has been properly
*   identified in MAX.  
*
*******************************************************************************/

using System;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsa;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.FrequencySweep
{
    public partial class MainForm : Form
    {
        NIRfsa rfsaSession;

        private string ResourceName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        private double ReferenceLevel
        {
            get
            {
                return decimal.ToDouble(this.referenceLevelNumeric.Value);
            }
        }

        private double ResolutionBandwidth
        {
            get
            {
                return decimal.ToDouble(this.resolutionBandwidthNumeric.Value);
            }
        }

        private double StartFrequency
        {
            get
            {
                return decimal.ToDouble(this.startFrequencyNumeric.Value);
            }
        }

        private double StopFrequency
        {
            get
            {
                return decimal.ToDouble(this.stopFrequencyNumeric.Value);
            }
        }

        private int Steps
        {
            get
            {
                return decimal.ToInt32(this.numStepsNumeric.Value);
            }
        }

        private double CenterFrequency
        {
            set
            {
                this.currentCenterFrequencyTextBox.Text = value.ToString();
            }
        }

        public MainForm()
        {
            InitializeComponent();
            LoadRfsaDeviceNames();
        }

        private void LoadRfsaDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-RFSA");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                InitializeRfsaSession();
                ConfigureForSpectrum();
                Sweep();
                CloseSession();
            }
            catch (System.Exception ex)
            {
                ShowError(ex.Message);
                CloseSession();
            }
            finally
            {
                ChangeControlState(true);
            }
        }

        private void ChangeControlState(bool isEnabled)
        {
            this.startButton.Enabled = isEnabled;
            this.resourceNameComboBox.Enabled = isEnabled;
            this.referenceLevelNumeric.Enabled = isEnabled;
            this.startFrequencyNumeric.Enabled = isEnabled;
            this.stopFrequencyNumeric.Enabled = isEnabled;
            this.resolutionBandwidthNumeric.Enabled = isEnabled;
            this.numStepsNumeric.Enabled = isEnabled;
        }

        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }

        private void InitializeRfsaSession()
        {
            CloseSession();
            rfsaSession = new NIRfsa(ResourceName, true, false);
            rfsaSession.DriverOperation.Warning += new System.EventHandler<RfsaWarningEventArgs>(DriverOperationWarning);
        }

        private void DriverOperationWarning(object sender, RfsaWarningEventArgs e)
        {
            MessageBox.Show(e.Warning.ToString(), "Warning");
        }

        private void ConfigureForSpectrum()
        {
            rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel;
            rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.Spectrum;
            rfsaSession.Configuration.Spectrum.ResolutionBandwidth = ResolutionBandwidth;
        }

        private void Sweep()
        {
            double startFreq = StartFrequency;
            double stopFreq = StopFrequency;
            double span = 20E6;

            if (Steps < 2)
            {
                ShowError("Invalid Input: Steps are less than 2");
                return;
            }

            if (stopFreq < startFreq)
            {
                ShowError("Invalid Input, Stop frequency is less than start frequency");
                return;
            }

            double increment = (stopFreq - startFreq) / (Steps - 1);
            while (startFreq <= stopFreq)
            {
                CenterFrequency = startFreq;
                rfsaSession.Configuration.Spectrum.ConfigureSpectrumFrequencyCenterSpan(startFreq, span);
                ReadPowerSpectrum();
                startFreq += increment;
            }
        }

        private void ReadPowerSpectrum()
        {
            double[] data;
            RfsaSpectrumInfo spectrumInfo;
            PrecisionTimeSpan timespan = new PrecisionTimeSpan(10.0);
            data = rfsaSession.Acquisition.Spectrum.ReadPowerSpectrum(timespan, out spectrumInfo);
            SetData(data);
        }

        private void SetData(double[] data)
        {
            DoubleData[] val = Array.ConvertAll<double, DoubleData>(data, (x) => new DoubleData(x));
            this.dataGridViewResults.DataSource = val;
            this.Refresh();
        }

        private void CloseSession()
        {
            if (rfsaSession != null)
            {
                try
                {
                    rfsaSession.Close();
                    rfsaSession = null;
                }
                catch (Exception ex)
                {
                    ShowError("Unable to Close Session, Reset the device.\n" + "Error : " + ex.Message);
                    Application.Exit();
                }
            }
        }
    }

    struct DoubleData
    {
        private double a;

        public DoubleData(double d)
        {
            a = d;
        }
        public double Value
        {
            get
            {
                return a;
            }
        }
    }
}