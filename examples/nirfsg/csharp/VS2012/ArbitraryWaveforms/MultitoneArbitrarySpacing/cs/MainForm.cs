//==================================================================================================
// Title        : Multitone Arbitrary Spacing
// Description  : This example demonstrates how to generate arbitrarily separated tones.
//			
//			The example lets you create a list of tones with a defined offset,a power level
//			and an initial phase.
//			
//			To add a tone:
//			   -Set the Tone Name with a name to identify the tone.
//			   -Set the Offset with the required frequency for the tone.
//			   -Set the Power Level with the specific level for the tone.
//			   -Set the Initial Phase for the tone.
//			   -Click the Add button.
//			   -The Tone Name is added to the Tone List.
//
//			To display the properties of a tone from the Tone List:
//			   -Select the tone from the Tone List by clicking it.
//			   -Click the Load button.
//			   -The properties of the selected tone are displayed.
//	
//			To delete a tone frome the Tone List:
//			   -Select the tone from the Tone List by clicking it.
//			   -Click the Del (Delete) button.
//			   -The tone is removed from the Tone List.
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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.MultitoneArbitrarySpacing
{
    public partial class MainForm : Form
    {
        NIRfsg _rfsgSession;
        List<ComplexMultitoneStruct> _toneList = new List<ComplexMultitoneStruct>();

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
            bool mirrorImage;
            int maximumNumberOfSamples = 0;
            double IQRate = 0;
            double centerfrequency = 0;
            double actualIQRate = 0;
            double peakEnvelopePower = 0;

            try
            {
                // Clear previous errors
                errorTextBox.Text = "No error.";

                // Read controls from GUI
                resourceName = resourceNameComboBox.Text;
                maximumNumberOfSamples = (int)numberOfSamplesNumeric.Value;
                IQRate = (double)iqRateNumeric.Value;
                centerfrequency = (double)centerFrequencyNumeric.Value;
                mirrorImage = mirrorImageCheckBox.Checked;

                // Initialize the NIRfsg session
                _rfsgSession = new NIRfsg(resourceName, true, false);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the NIRfsg session
                // Configure generation mode
                // Set generation mode to Arb Waveform           
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform;
                // Set to change the IQ Rate
                _rfsgSession.Arb.IQRate = IQRate;
                // Set to change the Pre-filter gain
                _rfsgSession.Arb.PreFilterGain = -2;
                // Read NIRfsg sessions configuration 
                // Set to read the IQ Rate
                actualIQRate = _rfsgSession.Arb.IQRate;
                // Generate waveform
                ComplexEquidistantMultitoneStruct complexEquidistantMultitone = GetComplexMultitone(_toneList, mirrorImage, maximumNumberOfSamples, IQRate);

                // Configure NIRfsg session
                _rfsgSession.RF.Configure(centerfrequency, complexEquidistantMultitone.signalPowerLevel);
                _rfsgSession.Arb.SignalBandwidth = complexEquidistantMultitone.signalBandwidth;

                // Write generated waveform
                _rfsgSession.Arb.WriteWaveform(string.Empty, complexEquidistantMultitone.IDataBuffer, complexEquidistantMultitone.QDataBuffer);

                // Display calculated data
                actualIQRateTextBox.Text = actualIQRate.ToString();
                actualPowerLevelTextBox.Text = complexEquidistantMultitone.signalPowerLevel.ToString();

                // Initiate Generation
                _rfsgSession.Initiate();

                // Read NIRfsg sessions attribute 
                // Set to read the Peak Env. power
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

        static ComplexEquidistantMultitoneStruct GetComplexMultitone(List<ComplexMultitoneStruct> complexMultitoneList,
            bool mirrorImage, int samples, double sampleRate)
        {
            ComplexEquidistantMultitoneStruct complexEquidistantMultitone =
                new ComplexEquidistantMultitoneStruct(0, null, null, 0, -100);
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
                throw new ArgumentException("The number of samples must be greater than 0.");
            }
            if (complexMultitoneList.Count <= 0)
            {
                throw new ArgumentException("You must enter at least one tone.");
            }
            // Allocate local resources
            IDataBuffer = new double[samples];
            QDataBuffer = new double[samples];

            // Allocate buffers
            frequency = new double[complexMultitoneList.Count];
            power = new double[complexMultitoneList.Count];
            initialPhase = new double[complexMultitoneList.Count];

            // Build data arrays
            for (i = 0; i < complexMultitoneList.Count; i++)
            {
                frequency[i] = complexMultitoneList[i].frequency;
                power[i] = complexMultitoneList[i].power;
                initialPhase[i] = complexMultitoneList[i].initialPhase;
            }

            // Initialize arrays to 0
            IDataBuffer = Enumerable.Repeat(0.0, samples).ToArray();
            QDataBuffer = Enumerable.Repeat(0.0, samples).ToArray();

            referenceFrequency = sampleRate * .4;
            minimumFrequencyStep = sampleRate / samples;
            gradesToRadians = Math.PI * 180;
            for (i = 0; i < complexMultitoneList.Count; i++)
            {
                if (frequency[i] >= referenceFrequency)
                {
                    throw new ArgumentException("Tone frequencies must be less than 0.4 * Sample rate.");
                }
                if ((frequency[i] - (minimumFrequencyStep * Math.Floor(frequency[i] / minimumFrequencyStep))) != 0)
                {
                    throw new ArgumentException("Frequencies must be multiple of sample rate.");
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

        void DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = e.Message;
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

                    // Unsubscribe from Rfsg warnings
                    _rfsgSession.DriverOperation.Warning -= DriverOperation_Warning;

                    // Close the RFSG NIRfsg session
                    _rfsgSession.Close();
                }
                _rfsgSession = null;                     // Clear handle value
            }
            catch (Exception ex)
            {
                errorTextBox.Text = "Error in StopGeneration(): " + ex.Message;
            }
        }

        #endregion

        #region Form Events
        private void loadButton_Click(object sender, System.EventArgs e)
        {
            int index;

            // Get the selected item to load settings from
            index = toneListListBox.SelectedIndex;
            if (index >= 0 && index < _toneList.Count)
            {
                // Read the items label
                // Read item from tone list
                toneNameTextBox.Text = toneListListBox.SelectedItem.ToString();
                offsetNumeric.Value = (decimal)_toneList[index].frequency;
                powerLevelNumeric.Value = (decimal)_toneList[index].power;
                initialPhaseNumeric.Value = (decimal)_toneList[index].initialPhase;
            }
        }

        private void deleteButton_Click(object sender, System.EventArgs e)
        {
            int index;
            // Get the selected item to delete
            index = toneListListBox.SelectedIndex;
            if (index >= 0)
            {
                // Delete item from list
                _toneList.RemoveAt(index);
                toneListListBox.Items.RemoveAt(index);
            }
        }

        private void addButton_Click(object sender, System.EventArgs e)
        {
            string name;
            double frequency;
            double power;
            double initialPhase;
            name = toneNameTextBox.Text;
            frequency = (double)offsetNumeric.Value;
            power = (double)powerLevelNumeric.Value;
            initialPhase = (double)initialPhaseNumeric.Value;

            // Add tones to the list
            _toneList.Add(new ComplexMultitoneStruct(frequency, power, initialPhase));
            // insert into list box
            toneListListBox.Items.Add(name);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop generation on exit
            StopGeneration();
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            // Stop
            StopGeneration();
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            // Start
            StartGeneration();
        }

        private void rfsgStatusTimer_Tick(object sender, System.EventArgs e)
        {
            // Check RFSG status every timer event (100ms)
            CheckGeneration();
        }
        #endregion

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

            public ComplexEquidistantMultitoneStruct(int bufferSize, double[] IDataBuffer, double[] QDataBuffer,
                double signalBandwidth, double signalPowerLevel)
            {
                this.bufferSize = bufferSize;
                this.IDataBuffer = IDataBuffer;
                this.QDataBuffer = QDataBuffer;
                this.signalBandwidth = signalBandwidth;
                this.signalPowerLevel = signalPowerLevel;
            }
        }

        private void EnableControls(bool enabled)
        {
            startButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            resourceNameComboBox.Enabled = enabled;
            mirrorImageCheckBox.Enabled = enabled;
            toneNameTextBox.Enabled = enabled;
            numberOfSamplesNumeric.Enabled = enabled;
            iqRateNumeric.Enabled = enabled;
            centerFrequencyNumeric.Enabled = enabled;
            offsetNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            initialPhaseNumeric.Enabled = enabled;
            // Start the status checking timer
            rfsgStatusTimer.Enabled = !enabled;

            addButton.Enabled = enabled;
            deleteButton.Enabled = enabled;
            loadButton.Enabled = enabled;

            Application.DoEvents();
        }
    }
}
