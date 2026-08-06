//==================================================================================================
// Title        : Forward Frequency Sweep (5672In-bandRetuning)
// Description  : This example demonstrates how to generate a frequency sweep 
//			at a specified power level as well as how to use the
//			in-band retuning feature through the use of the
//			upconverter center frequency attribute.
//
//			Note: In order to run this example, you must have your 
//			NI PXI-5610 configured to work together with a NI PXI-5442(AWG) 
//			To do this, open the Measurements & Automation Explorer, 
//			select the NI PXI-5610 and click on properties.
//==================================================================================================

using System;
using System.Threading;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ForwardFrequencySweep5672InbandRetuning
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        double _stopFrequency, _currentFrequency, _frequencyIncrement;
        double _upconverterCenterFrequency, _halfOfEffectiveSpan, _dwellTime;
        double _phaseDetectorFrequency;

        const string ErrorCantReachFrequency = @"The current frequency cannot be reached from the Upconverter 
                                                    Center Frequency with the current phase detector frequency, 
                                                    in-band retuning span and # of steps.";

        public MainForm()
        {
            InitializeComponent();

            LoadRfsgDeviceNames();

            ConfigurePdfComboBox();
        }

        private void LoadRfsgDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-Rfsg");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        #region UI Initial Value Config Section

        private void ConfigurePdfComboBox()
        {
            phaseDetectorFrequencyComboBox.Items.Add(1.0e6);
            phaseDetectorFrequencyComboBox.Items.Add(5.0e6);
            phaseDetectorFrequencyComboBox.SelectedIndex = 0;
        }

        #endregion

        #region Program Functions

        void StartGeneration()
        {
            string resourceName;
            double power;
            double startFrequency;
            double inbandSpan;

            int numberOfSteps;
            try
            {
                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                power = (double)powerLevelNumeric.Value;
                startFrequency = (double)startFrequencyNumeric.Value;
                _stopFrequency = (double)stopFrequencyNumeric.Value;
                numberOfSteps = (int)numberStepsNumeric.Value;
                _dwellTime = (double)dwellTimeNumeric.Value;
                inbandSpan = (double)deviceBandwidthToUseNumeric.Value;
                _phaseDetectorFrequency = double.Parse(phaseDetectorFrequencyComboBox.Text);

                _currentFrequency = startFrequency;
                _frequencyIncrement = (_stopFrequency - startFrequency) / (numberOfSteps - 1);

                // 100Hz is the default signal bandwidth for CW mode 
                _halfOfEffectiveSpan = (inbandSpan - 100) / 2;

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the device 
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the generation mode 
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave;

                // Configure the starting frequency and power level 
                _rfsgSession.RF.Configure(startFrequency, power);

                // Start the frequency sweep timer 
                EnableControls(false);
                stopButton.Focus();
            }
            catch (Exception ex)
            {
                ShowError("StartGeneration()", ex);
            }
        }

        void DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = e.Message;
        }

        void StopGeneration()
        {
            // Stop the status checking timer 
            EnableControls(true);
            try
            {
                if (_rfsgSession != null)
                {
                    // Disable the output.  This sets the noise floor as low as possible.
                    _rfsgSession.RF.OutputEnabled = false;

                    // Unsubscribe from warning events
                    _rfsgSession.DriverOperation.Warning -= DriverOperation_Warning;

                    // Close the RFSG NIRfsg session
                    _rfsgSession.Close();
                }
                _rfsgSession = null;
            }
            catch (Exception ex)
            {
                errorTextBox.Text = "Error in StopGeneration(): " + ex.Message;
            }
        }

        void SetNextSweepFrequency()
        {
            try
            {
                double nextPotentialUCF;

                if (_currentFrequency > _stopFrequency)
                {
                    StopGeneration();
                }
                else
                {
                    actualCurrentFrequencyTextBox.Text = _currentFrequency.ToString();

                    // Check to see if any part of the new frequency will move past the band allowed by the
                    // upconverter center frequency (UCF)
                    if ((_currentFrequency - _halfOfEffectiveSpan) > _upconverterCenterFrequency)
                    {
                        // If the frequency is not in range, pick the next UCF and check to make sure that the new
                        // frequency can be reached from the new UCF.  Depending on the phase detector frequency,
                        // in-band span and step size of the sweep, the new frequency might be out of reach of the UCF.
                        nextPotentialUCF = _currentFrequency + _halfOfEffectiveSpan;
                        nextPotentialUCF = Math.Floor(nextPotentialUCF / _phaseDetectorFrequency) * _phaseDetectorFrequency;

                        if (_currentFrequency > (nextPotentialUCF + _halfOfEffectiveSpan))
                        {
                            throw new ArgumentException(ErrorCantReachFrequency);
                        }

                        _rfsgSession.RF.Upconverter.CenterFrequency = nextPotentialUCF;
                        _rfsgSession.RF.Frequency = _currentFrequency;
                        _upconverterCenterFrequency = _rfsgSession.RF.Upconverter.CenterFrequency;
                    }
                    else
                    {
                        // If the frequency is within range, only the frequency needs to be set 
                        _rfsgSession.RF.Frequency = _currentFrequency;
                    }

                    // Configure the starting frequency and power level 
                    _rfsgSession.Initiate();

                    // Dwell the specified time at the current frequency.  Add code in here to take a measurement of the signal. 
                    Thread.Sleep((int)(_dwellTime * 1000));

                    // Abort the generation 
                    _rfsgSession.Abort();

                    _currentFrequency += _frequencyIncrement;

                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                ShowError("SetNextSweepFrequency()", ex);
            }
        }

        void ShowError(string functionName, Exception exception)
        {
            StopGeneration();
            errorTextBox.Text = "Error in " + functionName + ": " + exception.Message;
        }

        #endregion

        #region Form Events
        private void startButton_Click(object sender, System.EventArgs e)
        {
            StartGeneration();
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            StopGeneration();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopGeneration();
        }

        private void rfsgStatusTimer_Tick(object sender, System.EventArgs e)
        {
            SetNextSweepFrequency();
        }
        #endregion

        private void EnableControls(bool enabled)
        {
            startButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            resourceNameComboBox.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            startFrequencyNumeric.Enabled = enabled;
            stopFrequencyNumeric.Enabled = enabled;
            numberStepsNumeric.Enabled = enabled;
            dwellTimeNumeric.Enabled = enabled;
            deviceBandwidthToUseNumeric.Enabled = enabled;
            phaseDetectorFrequencyComboBox.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
