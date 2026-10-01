//==================================================================================================
// Title        : Multitone Uniform Spacing
// Description  : This example demonstrates how to generate uniformly separated tones.
//			
//			The example lets you choose the number of tones and the frequency between
//			them, as well as the power and initial phase for all the tones.
//			
//			The plot shows the baseband spectrum of the generated signal. The baseband
//			spectrum is calculated in this example.
//	
//			Note: In order to run this example, the upconverter must be configured with 
//			an Arbitrary Waveform Generator. 
//			To do this, open Measurement & Automation Explorer, select the upconverter 
//			and click on properties.
//==================================================================================================

using System;
using System.Linq;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.MultitoneUniformSpacing
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;

        public MainForm()
        {
            InitializeComponent();

            LoadRfsgDeviceNames();
        }

        private void LoadRfsgDeviceNames()
        {
            ModularInstrumentsSystem modularInstrumentsSystem = new ModularInstrumentsSystem("NI-Rfsg");
            foreach (DeviceInfo device in modularInstrumentsSystem.DeviceCollection)
                resourceNameComboBox.Items.Add(device.Name);
            if (modularInstrumentsSystem.DeviceCollection.Count > 0)
                resourceNameComboBox.SelectedIndex = 0;
        }

        #region Program Functions

        void ShowError(string functionName, Exception exception)
        {
            StopGeneration();
            errorTextBox.Text = "Error in " + functionName + ": " + exception.Message;
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
                ShowError("CheckGeneration()", ex);
            }
        }

        void StartGeneration()
        {
            string resourceName;
            int quantum;
            int numberOfTones;
            int maximumNumberOfSamples;
            double iqRate;
            double powerLevel;
            double centerFrequency;
            double initialPhase;
            double actualIQRate;
            double frequencyBetweenTones;
            double peakEnvelopePower;
            bool queryID = true;
            bool reset = false;

            try
            {
                // Clear previous errors
                errorTextBox.Text = "No Error";

                // Read controls from GUI
                resourceName = resourceNameComboBox.Text;
                numberOfTones = (int)numberOfTonesNumeric.Value;
                maximumNumberOfSamples = (int)maximumNumberOfSamplesNumeric.Value;
                iqRate = (double)iqRateNumeric.Value;
                powerLevel = (double)powerPerToneNumeric.Value;
                centerFrequency = (double)centerFrequencyNumeric.Value;
                initialPhase = (double)initialPhaseNumeric.Value;
                frequencyBetweenTones = (double)frequencyBetweenTonesNumeric.Value;

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, queryID, reset);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the NIRfsg session
                // Configure generation mode
                // Set generation mode to Arb Waveform           
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform;

                _rfsgSession.Arb.IQRate = iqRate;

                _rfsgSession.Arb.PreFilterGain = -2;

                // Read NIRfsg sessions configuration 
                actualIQRate = _rfsgSession.Arb.IQRate;

                quantum = _rfsgSession.Arb.WaveformCapabilities.WaveformQuantum;

                // Generate waveform
                ComplexEquidistantMultitoneStruct complexEquidistantMultitone =
                GetComplexEquidistantMultitone(
                   quantum,
                   numberOfTones,
                   maximumNumberOfSamples,
                   iqRate,
                   initialPhase,
                   powerLevel,
                   frequencyBetweenTones
                );

                // Configure NIRfsg session
                _rfsgSession.RF.Configure(centerFrequency, complexEquidistantMultitone.signalPowerLevel);      // Power Level calculated at rfsg_ComplexEquidistantMultitone


                _rfsgSession.Arb.SignalBandwidth = complexEquidistantMultitone.signalBandwidth;                  // Signal Bandwidth calculated by rfsg_ComplexEquidistantMultitone

                // Write generated waveform
                _rfsgSession.Arb.WriteWaveform(string.Empty, complexEquidistantMultitone.IDataBuffer, complexEquidistantMultitone.QDataBuffer);

                // Display calculated data
                actualIQRateTextBox.Text = actualIQRate.ToString();
                actualFrequencyBetweenTonesTextBox.Text = complexEquidistantMultitone.actualFrequencyBetweenTones.ToString();

                // Initiate Generation
                _rfsgSession.Initiate();

                // Read NIRfsg sessions attribute 
                peakEnvelopePower = _rfsgSession.RF.Advanced.PeakEnvelopePower;
                // Display Peak Env. Power on GUI indicator
                actualPeakPowerTextBox.Text = peakEnvelopePower.ToString();

                // Enable timer
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

        static ComplexEquidistantMultitoneStruct GetComplexEquidistantMultitone(int quantum, int numberOfTones,
            int maximumNumberOfSamples, double iqRate, double initialPhase, double powerLevelPerTone, double frequencyBetweenTones)
        {
            ComplexEquidistantMultitoneStruct complexEquidistantMultitone;
            double actualFrequencyBetweenTones;
            bool evenNumberOfGaps;
            int i;
            int numberOfSamples;
            double actualFrequency;
            double desiredFrequency;
            double fractionalIndex;
            double frequencyTolerance = 10.0;
            ComplexMultitoneStruct[] toneList;

            fractionalIndex = (numberOfTones - 1) * 0.5;

            // Even number of tones?
            if (numberOfTones % 2 == 1)
            {
                desiredFrequency = frequencyBetweenTones;
                evenNumberOfGaps = true;
            }
            else
            {
                desiredFrequency = frequencyBetweenTones / 2;
                evenNumberOfGaps = false;
            }

            FindWaveformParamsForFrequency(
               quantum,
               100,
               maximumNumberOfSamples,
               iqRate,
               desiredFrequency,
               frequencyTolerance,
                // Outputs
               out numberOfSamples,
               out actualFrequency
               );

            if (evenNumberOfGaps)
                actualFrequencyBetweenTones = actualFrequency;
            else
                actualFrequencyBetweenTones = actualFrequency * 2;

            toneList = new ComplexMultitoneStruct[numberOfTones];
            for (i = 0; i < numberOfTones; i++)
            {
                // Add tones to the list
                toneList[i] = new ComplexMultitoneStruct(
                    (i - fractionalIndex) * actualFrequencyBetweenTones,
                    powerLevelPerTone,
                    initialPhase
                );
            }

            // Build IQ Data arrays
            complexEquidistantMultitone = GetComplexMultitone(toneList, false, numberOfTones, numberOfSamples, iqRate);
            complexEquidistantMultitone.actualFrequencyBetweenTones = actualFrequencyBetweenTones;

            return complexEquidistantMultitone;
        }

        static ComplexEquidistantMultitoneStruct GetComplexMultitone(ComplexMultitoneStruct[] complexMultitone,
            bool mirrorImage, int numberOfTones, int samples, double sampleRate)
        {
            ComplexEquidistantMultitoneStruct complexEquidistantMultitone =
                new ComplexEquidistantMultitoneStruct(0, null, null, 0, -100, 0);
            double[] IDataBuffer;
            double[] QDataBuffer;
            int i;
            double maximumFrequency = 0;
            double maximumFrequencyTemp = 0;
            double referenceFrequency;
            double powerDB;
            double totalPower = 0;
            double minimumFrequencyStep;
            double numberOfCycles;
            double phaseInRadians;
            double gradesToRadians;
            double minimumPhaseIncrement;
            double[] frequency;
            double[] power;
            double[] initialPhase;

            if (samples <= 0)
            {
                throw new ArgumentOutOfRangeException("samples", "The number of samples must be greater than 0");
            }
            if (numberOfTones <= 0)
            {
                throw new ArgumentOutOfRangeException("numberOfTones", "You must enter at least one tone");
            }
            // Allocate local resources
            IDataBuffer = new double[samples];
            QDataBuffer = new double[samples];

            // Allocate buffers
            frequency = new double[numberOfTones];
            power = new double[numberOfTones];
            initialPhase = new double[numberOfTones];

            // Build data arrays
            for (i = 0; i < numberOfTones; i++)
            {
                frequency[i] = complexMultitone[i].frequency;
                power[i] = complexMultitone[i].power;
                initialPhase[i] = complexMultitone[i].initialPhase;
            }

            // Initialize arrays to 0
            IDataBuffer = Enumerable.Repeat(0.0, samples).ToArray();
            QDataBuffer = Enumerable.Repeat(0.0, samples).ToArray();

            referenceFrequency = sampleRate * .4;
            minimumFrequencyStep = sampleRate / samples;
            gradesToRadians = Math.PI * 180;
            for (i = 0; i < numberOfTones; i++)
            {
                if (frequency[i] >= referenceFrequency)
                {
                    throw new ArgumentOutOfRangeException("complexMultitone", "Tone frequencies must be less than .4 * Sample rate");
                }
                if ((frequency[i] - (minimumFrequencyStep * Math.Floor(frequency[i] / minimumFrequencyStep))) != 0)
                {
                    throw new ArgumentException("Frequencies must be multiple of sample rate");
                }
                // Keep track of the maximum Frequency to determine the bandwidth
                maximumFrequencyTemp = frequency[i];
                if (maximumFrequencyTemp < 0)
                {
                    maximumFrequencyTemp *= -1;
                }
                if (maximumFrequencyTemp > maximumFrequency)
                    maximumFrequency = maximumFrequencyTemp;


                phaseInRadians = initialPhase[i] * gradesToRadians;
                powerDB = power[i] / 10;
                powerDB = Math.Pow(10, powerDB);
                totalPower += powerDB;
                powerDB = Math.Sqrt(powerDB);

                numberOfCycles = frequency[i] / minimumFrequencyStep;
                minimumPhaseIncrement = (2 * Math.PI) / (samples / numberOfCycles);
                minimumPhaseIncrement = (2 * Math.PI * numberOfCycles) - minimumPhaseIncrement;

                for (int j = 0; j < samples; j++)
                {
                    double phase = phaseInRadians + j * (minimumPhaseIncrement / (samples - 1));
                    IDataBuffer[j] += powerDB * Math.Cos(phase);
                    QDataBuffer[j] += powerDB * Math.Sin(phase);
                }
            }
            if (mirrorImage)      // Mirror the image. Set Q Data buffer to 0
            {
                QDataBuffer = Enumerable.Repeat(0.0, samples).ToArray();
            }

            complexEquidistantMultitone.bufferSize = samples;
            complexEquidistantMultitone.IDataBuffer = IDataBuffer;
            complexEquidistantMultitone.QDataBuffer = QDataBuffer;
            complexEquidistantMultitone.signalBandwidth = maximumFrequency * 2;
            complexEquidistantMultitone.signalPowerLevel = Math.Log10(totalPower) * 10;
            return complexEquidistantMultitone;
        }

        static int FindWaveformParamsForFrequency(
           int quantum,
           int minimumNumberOfSamples,
           int maximumNumberOfSamples,
           double IQRate,
           double desiredFrequency,
           double frequencyTolerance,
           out int numberOfSamples,
           out double actualFrequency
        )
        {
            // Local Var.
            bool updateFrequency;
            bool positiveFrequency;
            bool restrictionsMet;
            bool lessThanMaxNumberOfSamples;
            int i = 0;
            int coercedQuantum;
            double newFrequency = 0;
            double oldError = 0;
            double newError = 0;

            // Var. used for feedback
            bool firstFoundYet = false;
            int numberOfSamplesTemp = 0;
            double frequencyErrorTemp = 0;
            double actualFrequencyTemp = double.NegativeInfinity; // Inf. (Max negative real 64)

            ValidateInputs(      // Check if inputs are valid
               quantum,
               minimumNumberOfSamples,
               maximumNumberOfSamples,
               IQRate,
               desiredFrequency
            );

            // Initial values
            do
            {
                i++;
                coercedQuantum = CoerceToQuantum(((int)(IQRate / desiredFrequency * i)), quantum);

                lessThanMaxNumberOfSamples = coercedQuantum <= maximumNumberOfSamples;   // Stop condition 2
                positiveFrequency = desiredFrequency > 0;                          // Stop condition 3

                updateFrequency = (coercedQuantum >= minimumNumberOfSamples) && lessThanMaxNumberOfSamples;

                newFrequency = (IQRate / coercedQuantum);
                newFrequency *= i;
                newError = desiredFrequency - newFrequency;
                if (newError < 0) // Get the Absolute Value
                    newError *= -1;
                oldError = actualFrequencyTemp - desiredFrequency;
                if (oldError < 0) // Get the Absolute Value
                    oldError *= -1;
                if (!((oldError > newError) && updateFrequency))  // Update the Frequency if the error is reduced
                {
                    // Keep values for: Num. of cycles, Num. of samples, fist found yet and act. Frequency 
                    frequencyErrorTemp = oldError;
                }
                else
                {
                    numberOfSamplesTemp = coercedQuantum;
                    firstFoundYet = true;
                    actualFrequencyTemp = newFrequency;
                    frequencyErrorTemp = newError;
                }
                restrictionsMet = (frequencyErrorTemp <= frequencyTolerance) && firstFoundYet;  // Stop condition 1
            } while (!restrictionsMet && positiveFrequency && lessThanMaxNumberOfSamples);

            numberOfSamples = numberOfSamplesTemp;
            actualFrequency = actualFrequencyTemp;

            if (!restrictionsMet)
            {
                throw new ArgumentException("The restrictions were not met");
            }
            return 0;
        }

        static int CoerceToQuantum(int samples, int quantum)
        {
            if (quantum == 0)
                return 0;
            if (samples >= quantum)
            {
                return quantum * (int)Math.Round((double)(samples / quantum), 0);
            }
            else
            {
                return quantum;
            }
        }

        static void ValidateInputs(int quantum, int minimumNumberOfSamples, int maximumNumberOfSamples, double IQRate,
          double desiredFrequency)
        {
            if ((IQRate < 1) || (IQRate < (desiredFrequency * 2)) || (desiredFrequency < 1) || (quantum < 1)
                || (minimumNumberOfSamples > maximumNumberOfSamples))
                throw new ArgumentException("Invalid settings");
        }

        struct ComplexMultitoneStruct
        {
            public double frequency;
            public double power;
            public double initialPhase;

            public ComplexMultitoneStruct(double frequency, double power, double initialPhase)
            {
                this.frequency = frequency;
                this.power = power;
                this.initialPhase = initialPhase;
            }
        }

        struct ComplexEquidistantMultitoneStruct
        {
            public int bufferSize;
            public double[] IDataBuffer;
            public double[] QDataBuffer;
            public double signalBandwidth;
            public double signalPowerLevel;
            public double actualFrequencyBetweenTones;

            public ComplexEquidistantMultitoneStruct(int bufferSize, double[] IDataBuffer, double[] QDataBuffer,
                double signalBandwidth, double signalPowerLevel, double actualFrequencyBetweenTones)
            {
                this.bufferSize = bufferSize;
                this.IDataBuffer = IDataBuffer;
                this.QDataBuffer = QDataBuffer;
                this.signalBandwidth = signalBandwidth;
                this.signalPowerLevel = signalPowerLevel;
                this.actualFrequencyBetweenTones = actualFrequencyBetweenTones;
            }
        }

        void StopGeneration()
        {
            // Stop the timer
            EnableControls(true);

            try
            {
                if (_rfsgSession != null)
                {
                    // Disable the output. This sets the noise floor as low as possible
                    _rfsgSession.RF.OutputEnabled = false;

                    // Unsubscribe from warning events
                    _rfsgSession.DriverOperation.Warning -= DriverOperation_Warning;

                    // Close the RFSG NIRfsg session
                    _rfsgSession.Close();
                }
                _rfsgSession = null;                           // Clear handle value
            }
            catch (Exception ex)
            {
                errorTextBox.Text = "Error in StopGeneration(): " + ex.Message;
            }
        }

        #endregion

        #region Form Events
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop generation on exit
            StopGeneration();
            // Quit user interface on exit
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            // Start
            StartGeneration();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            // Stop
            StopGeneration();
        }

        private void rfsgStatusTimer_Tick(object sender, System.EventArgs e)
        {
            // Check RFSG status every timer event (100ms)
            CheckGeneration();
        }
        #endregion

        private void EnableControls(bool enabled)
        {
            startButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            resourceNameComboBox.Enabled = enabled;
            numberOfTonesNumeric.Enabled = enabled;
            maximumNumberOfSamplesNumeric.Enabled = enabled;
            iqRateNumeric.Enabled = enabled;
            powerPerToneNumeric.Enabled = enabled;
            centerFrequencyNumeric.Enabled = enabled;
            initialPhaseNumeric.Enabled = enabled;
            frequencyBetweenTonesNumeric.Enabled = enabled;
            rfsgStatusTimer.Enabled = !enabled;

            Application.DoEvents();
        }
    }
}
