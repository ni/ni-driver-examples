/*******************************************************************************
*
* Example program:
*    Getting Started Spectrum 
*
* Category:
*   NI-RFSA
*
* Description:
*   Use this example to learn the basics of spectrum acquisition using the RF
	vector signal analyzer. The example shows how to configure the following
	parameters of the spectrum acquisition: reference clock source, reference
	level, start and stop frequencies, resolution bandwidth, and the spectrum
	acquisition type. If you configure the spectrum span (Stop Frequency - Start
	Frequency) to a value larger than the instantaneous bandwidth of the device,
	NI-RFSA performs multiple acquisitions and combines them into one spectrum
	of the size you requested. 

* Instructions for running:
*   1. Configure RFSA device in the MAX for the program to run. 
*
*   2. Configure the Reference Clock in the UI.
*
*	3. Configure the Reference Level in the UI. 
*		The reference level represents the maximum expected power of an input RF signal.
*
*	4. Configure the Start and Stop Center Frequency.
*
*	5. Configure the Resolution BandWidth.
*	
*   6. Select the Read Power Spectrum Button in the UI to start the spectrum acquisition.
*   
*   7. The data is displayed in the DataGrid.
*  
* I/O Connections Overview:
*   Make sure your signal input terminals match the Physical Channel I/O
*   Controls.  If you have a PXI chassis, ensure that it has been properly
*   identified in MAX.  
*
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsa;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.GettingStartedSpectrum
{
    public partial class MainForm : Form
    {
        NIRfsa rfsaSession;

        public MainForm()
        {
            InitializeComponent();
            ConfigureRefClockComboBox();
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

        #region UI Value Config Section

        private RfsaReferenceClockSource ReferenceClockSource
        {
            get
            {
                return this.referenceClockComboBox.SelectedValue as RfsaReferenceClockSource ?? RfsaReferenceClockSource.FromString(this.referenceClockComboBox.Text);
            }
        }

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

        private void ConfigureRefClockComboBox()
        {
            List<KeyValuePair<string, RfsaReferenceClockSource>> refClockValueList = new List<KeyValuePair<string, RfsaReferenceClockSource>>();
            refClockValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("OnboardClock", RfsaReferenceClockSource.OnboardClock));
            refClockValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("RefIn", RfsaReferenceClockSource.ReferenceIn));
            refClockValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("PXI_Clk", RfsaReferenceClockSource.PxiClock));
            refClockValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("ClkIn", RfsaReferenceClockSource.ClockIn));
            referenceClockComboBox.DisplayMember = "Key";
            referenceClockComboBox.ValueMember = "Value";
            referenceClockComboBox.DataSource = refClockValueList;
            referenceClockComboBox.SelectedIndex = 0;
        }


        #endregion

        private void readPowerSpectrumButton_Click(object sender, System.EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                InitializeRfsaSession(ResourceName);
                ConfigureRefClock(ReferenceClockSource);
                ConfigureForSpectrum(ReferenceLevel, StartFrequency, StopFrequency, ResolutionBandwidth);
                ReadPowerSpectrum();
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
            this.readPowerSpectrumButton.Enabled = isEnabled;
            this.resourceNameComboBox.Enabled = isEnabled;
            this.referenceLevelNumeric.Enabled = isEnabled;
            this.startFrequencyNumeric.Enabled = isEnabled;
            this.stopFrequencyNumeric.Enabled = isEnabled;
            this.referenceClockComboBox.Enabled = isEnabled;
            this.resolutionBandwidthNumeric.Enabled = isEnabled;
        }

        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }

        private void InitializeRfsaSession(string ResourceName)
        {
            CloseSession();
            rfsaSession = new NIRfsa(ResourceName, true, false);
            rfsaSession.DriverOperation.Warning += new System.EventHandler<RfsaWarningEventArgs>(DriverOperationWarning);
        }

        private void DriverOperationWarning(object sender, RfsaWarningEventArgs e)
        {
            MessageBox.Show(e.Warning.ToString(), "Warning");
        }

        private void ConfigureRefClock(RfsaReferenceClockSource refClockSource)
        {
            rfsaSession.Configuration.ReferenceClock.Source = refClockSource;
        }

        private void ConfigureForSpectrum(double ReferenceLevel, double StartFrequency, double StopFrequency, double ResolutionBandwidth)
        {
            rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel;
            rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.Spectrum;
            rfsaSession.Configuration.Spectrum.ResolutionBandwidth = ResolutionBandwidth;
            rfsaSession.Configuration.Spectrum.ConfigureSpectrumFrequencyStartStop(StartFrequency, StopFrequency);
        }

        private void ReadPowerSpectrum()
        {
            RfsaSpectrumInfo spectrumInfo;
            double[] data;
            PrecisionTimeSpan timespan = new PrecisionTimeSpan(10.0);
            data = rfsaSession.Acquisition.Spectrum.ReadPowerSpectrum(timespan, out spectrumInfo);
            DisplayData(data);
        }

        private void DisplayData(double[] data)
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