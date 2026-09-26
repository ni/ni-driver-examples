//==================================================================================================
// Title        : Large Arbitrary Waveform Generation
// Description  : This example demonstrates how to write a large waveform to the NI-RFSG. 
//			It shows how large waveforms can be written in pieces, therefore using less 
//			memory.  This example generates a chirp waveform. 
//
//			Note: In order to run this example, the upconverter must be configured with 
//			an Arbitrary Waveform Generator. 
//			To do this, open Measurement & Automation Explorer, select the upconverter 
//			and click on properties.
//==================================================================================================

using System;
using System.Drawing;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.LargeArbitraryWaveformGeneration
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        double _phase = -90;
        const string WaveformName = "waveform";
        const int WriteAtOnce = 200000;

        public MainForm()
        {
            InitializeComponent();

            LoadRfsgDeviceNames();

            ConfigurePowerLevelTypeComboBox();
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

        private void ConfigurePowerLevelTypeComboBox()
        {
            powerLevelTypeComboBox.Items.AddRange(Enum.GetNames(typeof(RfsgRFPowerLevelType)));
            powerLevelTypeComboBox.SelectedIndex = 1;
        }

        #endregion

        #region Program Functions

        struct ChirpConfiguration
        {
            public double triangleWaveformFrequency;
            public double triangleWaveformAmplitude;
            public double actualIQRate;
            public double actualChirpDuration;
            public double carrierFrequency;
            public double sign;
            public int totalNumberOfSamples;
        }

        ChirpConfiguration ConfigureChirpForLargeWaveform(double iqRate,
                                                double startFrequency,
                                                double stopFrequency,
                                                double chirpDuration)
        {
            try
            {
                double bandwidth;
                int maxWaveformSize, minWaveformSize, waveformQuantum;
                ChirpConfiguration chirpConfiguration;
                string modelType;

                // Set RFSG frequency (Carrier Freqency) to average of start and stop 
                // frequency
                chirpConfiguration.carrierFrequency = (startFrequency + stopFrequency) / 2.0;

                // Find out if we should sweep the frequency range up (+1) or down (-1)
                chirpConfiguration.sign = (startFrequency < stopFrequency) ? 1 : -1;

                // Get the signal bandwidth
                bandwidth = (stopFrequency - startFrequency) * chirpConfiguration.sign;

                _rfsgSession.Arb.SignalBandwidth = bandwidth;

                // Set the IQ Rate 
                _rfsgSession.Arb.IQRate = iqRate;

                // Disable phase continuity. We don't need the driver to maintain
                // phase continuity because the chirp waveform we're using is not 
                // phase continuous (big discontinuity as we jump from stop back
                // to start frequency).
                _rfsgSession.Arb.PhaseContinuityEnabled = RfsgPhaseContinuityEnabled.Disabled;

                modelType = _rfsgSession.Identity.InstrumentModel;

                if (modelType.Equals("NI PXI-5670", StringComparison.OrdinalIgnoreCase) || modelType.Equals("NI PXI-5671", StringComparison.OrdinalIgnoreCase))
                {
                    // Disable digital equalization. For bandwidths greater than ~2MHz,
                    // you may want to turn this on to make the output power flatter across
                    // the signal bandwidth. _rfsgSession.Initiate function will execute faster
                    // with disabled equalization on 5670/71 devices.
                    _rfsgSession.Arb.DigitalEqualizationEnabled = false;
                }

                // Get the actual (coerced) IQ Rate 
                chirpConfiguration.actualIQRate = _rfsgSession.Arb.IQRate;

                // Get number of samples, coerce it to RFSG max/min waveform size
                maxWaveformSize = _rfsgSession.Arb.WaveformCapabilities.WaveformSizeMaximum;
                minWaveformSize = _rfsgSession.Arb.WaveformCapabilities.WaveformSizeMinimum;
                waveformQuantum = _rfsgSession.Arb.WaveformCapabilities.WaveformQuantum;
                chirpConfiguration.totalNumberOfSamples = (int)(chirpConfiguration.actualIQRate * chirpDuration);
                if (chirpConfiguration.totalNumberOfSamples > maxWaveformSize)
                {
                    chirpConfiguration.totalNumberOfSamples = maxWaveformSize;
                }
                else if (chirpConfiguration.totalNumberOfSamples < minWaveformSize)
                {
                    chirpConfiguration.totalNumberOfSamples = minWaveformSize;
                }
                // Round the number of samples to the nearest multiple of RFSG quantum
                chirpConfiguration.totalNumberOfSamples = waveformQuantum *
                   (int)(Math.Round((double)chirpConfiguration.totalNumberOfSamples / waveformQuantum, 0));
                chirpConfiguration.actualChirpDuration =
                   chirpConfiguration.totalNumberOfSamples / chirpConfiguration.actualIQRate;


                // Get Parameters for creating chirp waveform.  We want a complex waveform 
                // with the following characteristics:
                //       i)   constant power (so R = sqrt(I^2 + Q^2) is a constant)
                //       ii)  single tone, with frequency increasing (or decreasing) linearly 
                //            with time, so dq/dt = 2m*t, and q(t) = m*(t^2) for some m
                //       iii) f(t0) = Start Frequency Offset, f(t1) = Stop Frequency Offset, for 
                //            t0 = (-1) * Actual Chirp Duration / 2, 
                //            t1 = Actual Chirp Duration / 2.
                //            since f(t) = dq/dt (t) / 2pi, we can use ii) to rewrite f as 
                //            f(t) = (2m*t)/2pi
                //
                // Calculate m (see text in 8 iii).  Stop Frequency Offset = f(t1) --> Stop 
                // Frequency Offset = (2m*Actual Chirp Duration/2)/2pi
                // --> m = (Stop Frequency Offset * 2pi)/Actual Chirp Duration.  Note we're actually 
                // using Max Frequency Offset instead of Stop Frequency Offset.  Using Max Frequency Offset 
                // prevents us from taking the square root of a negative number.  This assumes
                // that we are sweeping the frequency up, so we may get sign of m wrong if 
                // we're actually sweeping down.  We'll correct for this in step 11.
                //
                // Eventually we'll generate q(t) = m*(t^2) by simply squaring a linear 
                // waveform.  So rewrite as q(t) = (sqrt(m) * t)^2.  Inside the loop we 
                // generate the (sqrt(m) * t) waveform, then square it.
                //
                // Calculate the maximum value in the ramp in the waveform 
                //    y(t) = (sqrt(m) * t).  
                // Half the ramp will be below zero, and the other half above, 
                // and num samples / Sample Clock Rate is time, so 
                //    Ramp Max Val = sqrt(m) * ( (#samples/2) / Sample Clock Rate) ).
                //
                // Calculate frequency (cycles/s) for the triangle waveform.  
                // Sample Clock Rate / Samples in Waveform = Waveforms (cycles) per second.
                // We don't actually want a triangle waveform.  We're using it to create 
                // a ramp.

                chirpConfiguration.triangleWaveformFrequency = chirpConfiguration.actualIQRate /
                                                       (2.0 * chirpConfiguration.totalNumberOfSamples);
                chirpConfiguration.triangleWaveformAmplitude = Math.Sqrt((bandwidth * Math.PI) /
                                                       chirpConfiguration.actualChirpDuration) *
                                                       (chirpConfiguration.totalNumberOfSamples /
                                                       (2.0 * chirpConfiguration.actualIQRate));
                return chirpConfiguration;
            }
            catch
            {
                throw;
            }
        }

        static double[] TriangleWave(int numberToWrite, double amplitude, double frequency, ref double phase)
        {
            double[] waveform = new double[numberToWrite];
            if (phase < 0)
                phase += 360;
            for (int i = 0; i < numberToWrite; i++)
            {
                double p = (phase + frequency * 360 * i) % 360;
                if (p >= 0 && p < 90)
                {
                    waveform[i] = 2 * amplitude * p / 180;
                }
                else if (p >= 90 && p < 270)
                {
                    waveform[i] = 2 * amplitude * (1 - p / 180);
                }
                else
                {
                    waveform[i] = 2 * amplitude * (p / 180 - 2);
                }
            }
            phase = (phase + frequency * 360 * numberToWrite) % 360;
            return waveform;
        }

        void StartGeneration()
        {
            try
            {
                string resourceName;
                double startFrequency, stopFrequency;
                double chirpDuration;
                double power;
                double iqRate;
                RfsgRFPowerLevelType powerLevelType;
                bool directDownload;
                int samplesRemaining;
                double[] iData, qData;
                bool moreToCome = true;
                ChirpConfiguration chirpConfiguration;

                // Read in all of the control values 
                resourceName = resourceNameComboBox.Text;
                startFrequency = (double)startFrequencyNumeric.Value;
                stopFrequency = (double)stopFrequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                powerLevelType = (RfsgRFPowerLevelType)Enum.Parse(typeof(RfsgRFPowerLevelType), (string)powerLevelTypeComboBox.Text);
                directDownload = directDownloadCheckBox.Checked;
                iqRate = (double)iqRateNumeric.Value;
                chirpDuration = (double)iqChirpDurationNumeric.Value;

                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform;

                // Configure the instrument 
                chirpConfiguration = ConfigureChirpForLargeWaveform(iqRate, startFrequency, stopFrequency, chirpDuration);

                _rfsgSession.RF.Configure(chirpConfiguration.carrierFrequency, power);

                actualIQRateTextBox.Text = chirpConfiguration.actualIQRate.ToString();
                actualIQNumberOfSamplesTextBox.Text = chirpConfiguration.totalNumberOfSamples.ToString();
                actualIQChirpDurationTextBox.Text = chirpConfiguration.actualChirpDuration.ToString();

                _rfsgSession.RF.PowerLevelType = powerLevelType;

                _rfsgSession.Arb.PreFilterGain = -2.0; // -2.0 dB 
                _rfsgSession.Arb.DataTransfer.DirectDownloadEnabled = directDownload;

                _rfsgSession.Arb.AllocateWaveform(WaveformName, chirpConfiguration.totalNumberOfSamples);


                // Activate stop button (so that the user can stop the waveform upload) 
                stopButton.Enabled = true;
                stopButton.Focus();
                samplesRemaining = chirpConfiguration.totalNumberOfSamples;

                iData = new double[WriteAtOnce];
                qData = new double[WriteAtOnce];

                double[] waveform;
                do
                {
                    int size = WriteAtOnce;

                    if (size > samplesRemaining)
                    {
                        size = samplesRemaining;
                        moreToCome = false;
                        Array.Resize<double>(ref iData, size);
                        Array.Resize<double>(ref qData, size);
                    }

                    waveform = TriangleWave(size,
                                 chirpConfiguration.triangleWaveformAmplitude,
                                 chirpConfiguration.triangleWaveformFrequency / chirpConfiguration.actualIQRate,
                                 ref _phase);

                    for (int i = 0; i < size; ++i)
                    {
                        double element = chirpConfiguration.sign * waveform[i] * waveform[i];
                        iData[i] = Math.Cos(element);
                        qData[i] = Math.Sin(element);
                    }

                    samplesRemaining -= size;

                    // If the stop button has been pressed, the NIRfsg session will be null. In 
                    // that case, we must exit this function;
                    if (_rfsgSession == null)
                    {
                        return;
                    }

                    _rfsgSession.Arb.WriteWaveform(WaveformName, iData, qData);
                    // Update the Progress bar 
                    generationStatusProgressBar.Value = (int)(100 - samplesRemaining / (chirpConfiguration.totalNumberOfSamples * 0.01));
                } while (moreToCome);

                // Initiate Generation 
                _rfsgSession.Initiate();

                // Turn on the light
                generatingLed.BackColor = Color.Lime;

                // Start the status checking timer 
                EnableControls(false);
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

        void CheckGeneration()
        {
            try
            {
                // Check the status of the RFSG 
                _rfsgSession.CheckGenerationStatus();
            }
            catch (Exception ex)
            {
                generatingLed.BackColor = SystemColors.Control; // generation failed
                ShowError("CheckGeneration()", ex);
            }
        }

        void StopGeneration()
        {
            // Reset the phase in case another waveform is going to be written
            _phase = -90;
            
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

                    // Close the NIRfsg session
                    _rfsgSession.Close();
                }
                _rfsgSession = null;
            }
            catch (Exception ex)
            {
                errorTextBox.Text = "Error in StopGeneration(): " + ex.Message;
            }

            // Activate all stopped controls 
            generatingLed.BackColor = SystemColors.Control; // generation stopped
            generationStatusProgressBar.Value = 0;
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
            CheckGeneration();
        }
        #endregion

        private void EnableControls(bool enabled)
        {
            startButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            resourceNameComboBox.Enabled = enabled;
            startFrequencyNumeric.Enabled = enabled;
            stopFrequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            powerLevelTypeComboBox.Enabled = enabled;
            directDownloadCheckBox.Enabled = enabled;
            iqRateNumeric.Enabled = enabled;
            iqChirpDurationNumeric.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
