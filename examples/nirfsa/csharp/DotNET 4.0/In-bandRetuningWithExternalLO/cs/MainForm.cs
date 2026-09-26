/*******************************************************************************
*
* Example program:
*    Inband Retuning With External LO
*
* Category:
*   NI-RFSA
*
* Description:
*   Use this example to learn the basics of in-band retuning with an external
	LO using the NI PXIe-5601 RF downconverter, NI PXIe-5622 digitizer, and
	example shows how to enable in-band retuning and configure the spectrum
	acquisition parameters in a way that does not change the LO tuned frequency.

* Instructions for running:
*   1. Configure RFSA device in the MAX for the program to run. 
*
*   2. Configure the LO Source in the MAX and Select the resource in the LO Source Name.
*
*	3. Configure the Reference Level in the UI. 
*		The reference level represents the maximum expected power of an input RF signal.
*
*	4. Configure the Center Frequency and Span in the UI.
*
*	5. Configure the Resolution BandWidth and DownConvertor Frequency.
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
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsa;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.InbandRetuningWithExternalLO
{
    public partial class MainForm : Form
    {
        NIRfsa rfsaSession;
        NIRfsg rfsgSession;

        public MainForm()
        {
			InitializeComponent();
            LoadRfsaDeviceNames();
            LoadRfsgDeviceNames();
        }
		
		#region UI Initial Value Config Section
        private void LoadRfsaDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-RFSA");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        private void LoadRfsgDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-RFSG");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                loResourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                loResourceNameComboBox.SelectedIndex = 0;
        }
        #endregion

        private string RFSAResourceName
        {
            get
            {
                return this.resourceNameComboBox.Text;
            }
        }

        private string LOResourceName
        {
            get
            {
                return this.loResourceNameComboBox.Text;
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

        private double CenterFrequency
        {
            get
            {
                return decimal.ToDouble(this.centerFrequencyNumeric.Value);
            }
        }

        private double Span
        {
            get
            {
                return decimal.ToDouble(this.spanNumeric.Value);
            }
        }

        private double DownConverterCenterFrequency
        {
            get
            {
                return decimal.ToDouble(this.downconverterCenterFrequencyNumeric.Value);
            }
        }

        private void InitializeRfsaSession()
        {
            CloseRfsaSession();
            rfsaSession = new NIRfsa(RFSAResourceName, true, false, "DriverSetup = LO: <external>");
            rfsaSession.DriverOperation.Warning += new System.EventHandler<RfsaWarningEventArgs>(DriverOperationWarning);
        }

        private void DriverOperationWarning(object sender, RfsaWarningEventArgs e)
        {
            MessageBox.Show(e.Warning.ToString(), "Warning");
        }

        private void InitializeExternalLOSession()
        {
            // Initiate Rfsg Session   
            CloseExternalLOSession();
            rfsgSession = new NIRfsg(LOResourceName, true, false);
        }
        
        private void ConfigureRefClock()
        {
            rfsaSession.Configuration.ReferenceClock.Source = RfsaReferenceClockSource.OnboardClock;
        }

        private double ConfigureForSpectrumAndEnableInBandRetuning()
        {
            rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel;
            rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.Spectrum;
            rfsaSession.Acquisition.Advanced.DownconverterCenterFrequency = DownConverterCenterFrequency;
            rfsaSession.Configuration.Spectrum.ConfigureSpectrumFrequencyCenterSpan(CenterFrequency, Span);
            rfsaSession.Configuration.Spectrum.ResolutionBandwidth = ResolutionBandwidth;
            System.Reflection.PropertyInfo profInfo = rfsaSession.Configuration.SignalPath.LocalOscillator.GetType().GetProperty("LOFrequency");
            rfsaSession.Utility.ResetAttribute(profInfo);
            return rfsaSession.Configuration.SignalPath.LocalOscillator.LOFrequency;
        }

        private double ConfigureExternalLO(double loFrequency)
        {
            string InstrumentModel = rfsgSession.Identity.InstrumentModel;
            if (string.Compare(InstrumentModel, "NI PXIe-5653", StringComparison.OrdinalIgnoreCase) == 0)
            {
                rfsgSession.RF.Frequency = loFrequency;
            }
            else
            {
                rfsgSession.RF.Configure(loFrequency, 0.0);
            }

            return rfsgSession.RF.Frequency;
        }

        private void InitiateExternalLO()
        {
            rfsgSession.Initiate();
        }

        private void ConfigureActualLOFrequency(double actualLOFrequency)
        {
            rfsaSession.Configuration.SignalPath.LocalOscillator.LOFrequency = actualLOFrequency;
        }

        private void ReadPowerSpectrum()
        {
            RfsaSpectrumInfo spectrumInfo;
            double[] data;
            PrecisionTimeSpan timespan = new PrecisionTimeSpan(10.0);
            data = rfsaSession.Acquisition.Spectrum.ReadPowerSpectrum(timespan, out spectrumInfo);
            DoubleData[] val = Array.ConvertAll<double, DoubleData>(data, (x) => new DoubleData(x));
            dataGridViewResults.DataSource = val;
            
        }

        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }


        private void CloseRfsaSession()
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

        private void CloseExternalLOSession()
        {
            if (rfsgSession != null)
            {
                try
                {
                    rfsgSession.Close();
                    rfsgSession = null;
                }
                catch (Exception ex)
                {
                    ShowError("Unable to Close the external LO Session, Reset the device.\n" + "Error : " + ex.Message);
                    Application.Exit();
                }
            }
        }

        private void readPowerSpectrumButton_Click(object sender, System.EventArgs e)
        {
            ChangeControlState(false);
            try
            {
                InitializeRfsaSession();
                InitializeExternalLOSession();
                ConfigureRefClock();
                double loFrequency = ConfigureForSpectrumAndEnableInBandRetuning();
                double actualLOFrequency = ConfigureExternalLO(loFrequency);
                InitiateExternalLO();
                this.loFrequencyTextBox.Text = actualLOFrequency.ToString();
                ConfigureActualLOFrequency(actualLOFrequency);
                ReadPowerSpectrum();
                CloseRfsaSession();
                CloseExternalLOSession();
            }
            catch (System.Exception ex)
            {
                CloseRfsaSession();
                CloseExternalLOSession();
                ShowError(ex.Message);
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
            this.loResourceNameComboBox.Enabled = isEnabled;
            this.referenceLevelNumeric.Enabled = isEnabled;
            this.centerFrequencyNumeric.Enabled = isEnabled;
            this.spanNumeric.Enabled = isEnabled;
            this.downconverterCenterFrequencyNumeric.Enabled = isEnabled;
            this.resolutionBandwidthNumeric.Enabled = isEnabled;
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