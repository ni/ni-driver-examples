/*******************************************************************************
*
* Example program:
*    Inband Retuning With Internal LO
*
* Category:
*   NI-RFSA
*
* Description:
*   Use this example to learn the basics of in-band retuning  using the
	NIPXIe-5663/5663E RF vector signal analyzer.  The example shows how to
	enable in-band retuning and configure a spectrum acquisition that does not
	reconfigure the LO frequency of the downconverter with every change in
	spectrum center frequency.

* Instructions for running:
*   1. Configure RFSA device in the MAX for the program to run. 
*
*	2. Configure the Reference Level in the UI. 
*		The reference level represents the maximum expected power of an input RF signal.
*
*	3. Configure the Center Frequency and Span in the UI.
*
*	4. Configure the Resolution BandWidth and DownConvertor Frequency.
*	
*   5. Select the Read Power Spectrum Button in the UI to start the spectrum acquisition.
*   
*   6. The data is displayed in the DataGrid.
*  
* I/O Connections Overview:
*   Make sure your signal input terminals match the Physical Channel I/O
*   Controls.  If you have a PXI chassis, ensure that it has been properly
*   identified in MAX.  
*
*******************************************************************************/
using System.Collections.Generic;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsa;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;
using System;

namespace NationalInstruments.Examples.InbandRetuningWithInternalLO
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

        #region UI Initial Value Config Section
        private void ConfigureRefClockComboBox()
        {
            List<KeyValuePair<string, RfsaReferenceClockSource>> refClockValueList = new List<KeyValuePair<string, RfsaReferenceClockSource>>();
            refClockValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("OnboardClock", RfsaReferenceClockSource.OnboardClock));
            refClockValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("RefIn", RfsaReferenceClockSource.ReferenceIn));
            refClockValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("PXI_Clk", RfsaReferenceClockSource.PxiClock));
            refClockValueList.Add(new KeyValuePair<string, RfsaReferenceClockSource>("ClkIn", RfsaReferenceClockSource.ClockIn));
            referenceClockComboBox.DataSource = refClockValueList;
            referenceClockComboBox.DisplayMember = "Key";
            referenceClockComboBox.ValueMember = "Value";
            referenceClockComboBox.SelectedIndex = 0;
        }

        private void LoadRfsaDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-RFSA");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                rfsaResourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                rfsaResourceNameComboBox.SelectedIndex = 0;
        }

        #endregion
        private string RFSAResourceName
        {
            get
            {
                return this.rfsaResourceNameComboBox.Text;
            }
        }

        private RfsaReferenceClockSource ReferenceClockSource
        {
            get
            {
                return this.referenceClockComboBox.SelectedValue as RfsaReferenceClockSource ?? RfsaReferenceClockSource.FromString(this.referenceClockComboBox.Text);
            }
        }

        private double ReferenceLevel
        {
            get
            {
                return decimal.ToDouble(this.referenceLevelNumeric.Value);
            }
        }

        private double CenterFrequency
        {
            get
            {
                return decimal.ToDouble(this.centerFrequencyNumeric.Value);
            }
        }

        private double ResolutionBandwidth
        {
            get
            {
                return decimal.ToDouble(this.resolutionBandwidthNumeric.Value);
            }
        }

        private double DownConverterCenterFrequency
        {
            get
            {
                return decimal.ToDouble(this.downconverterCenterFrequencyNumeric.Value);
            }
        }

        private double Span
        {
            get
            {
                return decimal.ToDouble(this.spanNumeric.Value);
            }
        }

        private void readPowerSpectrumButton_Click(object sender, System.EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                InitializeRfsaSession();
                ConfigureRefClock();
                configureForSpectrumAndEnableInBandRetuning();
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
            this.rfsaResourceNameComboBox.Enabled = isEnabled;
            this.referenceClockComboBox.Enabled = isEnabled;
            this.referenceLevelNumeric.Enabled = isEnabled;
            this.centerFrequencyNumeric.Enabled = isEnabled;
            this.spanNumeric.Enabled = isEnabled;
            this.downconverterCenterFrequencyNumeric.Enabled = isEnabled;
            this.resolutionBandwidthNumeric.Enabled = isEnabled;
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
            rfsaSession = new NIRfsa(RFSAResourceName, true, false);
            rfsaSession.DriverOperation.Warning += new System.EventHandler<RfsaWarningEventArgs>(DriverOperationWarning);
        }

        private void DriverOperationWarning(object sender, RfsaWarningEventArgs e)
        {
            MessageBox.Show(e.Warning.ToString(), "Warning");
        }

        private void ConfigureRefClock()
        {
            rfsaSession.Configuration.ReferenceClock.Source = ReferenceClockSource;
            rfsaSession.Configuration.ReferenceClock.Rate = 10E6;
        }

        private void configureForSpectrumAndEnableInBandRetuning()
        {
            rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel;
            rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.Spectrum;
            rfsaSession.Acquisition.Advanced.DownconverterCenterFrequency = DownConverterCenterFrequency;
            rfsaSession.Configuration.Spectrum.ConfigureSpectrumFrequencyCenterSpan(CenterFrequency, Span);
            rfsaSession.Configuration.Spectrum.ResolutionBandwidth = ResolutionBandwidth;
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
            DoubleData[] dData = System.Array.ConvertAll<double, DoubleData>(data, (x) => new DoubleData(x));
            this.dataGridViewResults.DataSource = dData;
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