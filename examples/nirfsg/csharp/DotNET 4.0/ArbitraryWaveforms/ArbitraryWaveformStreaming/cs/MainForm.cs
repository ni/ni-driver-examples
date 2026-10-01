//==================================================================================================
// Title        : Arbitrary Waveform Streaming
// Description  : This example demonstrates how to generate an arbitrary streaming waveform.
//==================================================================================================

using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.ArbitraryWaveformStreaming
{
    public partial class MainForm : Form
    {
        // Maximum elements in queue
        const int MaximumQueueSize = 100;

        // Time in milliseconds that the ProducerLoop waits for ConsumerLoop to catch up.
        // Used when queue is full (MaximumQueueSize items).
        // Note: If consumerLoop is faster than the producerLoop then streaming cannot be sustained and will result in an RFSG error.
        // Hence we need to handle only the situation where ProducerLoop is faster.
        const int WaitForConsumerInterval = 100;

        const string StreamingWaveformName = "streamingWaveform";
        const bool RfsgIDQuery = true;
        const bool RfsgReset = false;

        NIRfsg _rfsgSession;
        bool _readFromStart;
        int _elementsRemaining, _blockSize, _elementsPerBlock, _elementsInFile;

        // Thread safe variables available in .Net 4.0
        ConcurrentQueue<ComplexInt16[]> _queueHandle;
        volatile bool _running, _producerDone, _consumerDone;

        ComplexInt16[] _producerBuffer, _consumerBuffer;
        MemoryMappedFile _memoryMappedFileObject;
        MemoryMappedViewAccessor _memoryMappedAccessor;
        int _fileCurrentIndex;

        // ThreadSafe method wrapper for errorTextBox control
        delegate void SetErrorText(string functionName, Exception exception);

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

        void StartGeneration()
        {
            string resourceName;
            int streamingSize, fileSize;
            double frequency, power, iqRate, preFilterGain;
            int i, numberOfBlocksInWaveform, elementsToWrite;
            string fileName;
            FileStream fStream = null;

            try
            {
                // We are starting - set GUI ctrls
                generatingLed.BackColor = System.Drawing.SystemColors.Control;
                errorTextBox.Text = "No error.";
                Application.DoEvents();

                // Read in all of the control values
                resourceName = resourceNameComboBox.Text;
                frequency = (double)frequencyNumeric.Value;
                power = (double)powerLevelNumeric.Value;
                iqRate = (double)iqRateNumeric.Value;
                preFilterGain = (double)preGainNumeric.Value;
                streamingSize = (int)streamingSizeNumeric.Value;
                fileName = pathTextBox.Text;
                _blockSize = (int)blockSizeNumeric.Value;

                // Open a session to the device
                _rfsgSession = new NIRfsg(resourceName, RfsgIDQuery, RfsgReset);

                // Subscribe to Rfsg warnings
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                // Configure the generation mode to Arb Waveform
                _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform;

                // Configure the frequency and output power level
                _rfsgSession.RF.Configure(frequency, power);

                // Set the power level type to peak power
                _rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;

                // Enable streaming and specify which waveform will be the streaming waveform
                _rfsgSession.Arb.DataTransfer.Streaming.StreamingWaveformName = StreamingWaveformName;
                _rfsgSession.Arb.DataTransfer.Streaming.StreamingEnabled = true;

                // Set the IQ rate equal to the IQ rate for the waveform data
                _rfsgSession.Arb.IQRate = iqRate;

                // Set the pre-filter gain in the arb
                _rfsgSession.Arb.PreFilterGain = preFilterGain;

                // Configure the signal bandwidth
                iqRate = _rfsgSession.Arb.IQRate;
                actualIQRateTextBox.Text = iqRate.ToString();
                _rfsgSession.Arb.SignalBandwidth = iqRate * 0.8;

                // Open the file that contains the waveform.  
                // The file contains signed binary interleaved IQ Data.x
                // Each IQ sample takes 4 bytes; the first two for the I and the second two for the Q.
                fStream = File.OpenRead(fileName);
                fileSize = (int)fStream.Length;
                fStream.Close();

                _memoryMappedFileObject = MemoryMappedFile.CreateFromFile(fileName);
                _memoryMappedAccessor = _memoryMappedFileObject.CreateViewAccessor();

                // Allocate a buffer on the device with the specificed size
                _rfsgSession.Arb.AllocateWaveform(StreamingWaveformName, streamingSize);

                numberOfBlocksInWaveform = (int)Math.Floor((double)(streamingSize / _blockSize));
                _elementsPerBlock = _blockSize * 2;
                _elementsInFile = fileSize / 2;
                _elementsRemaining = _elementsInFile;
                _readFromStart = true;
                _producerBuffer = new ComplexInt16[_blockSize];

                // Fill the initial buffer allocated buffer with data
                for (i = 0; i < numberOfBlocksInWaveform; i++)
                {
                    if (_readFromStart)
                    {
                        _fileCurrentIndex = 0;
                        _readFromStart = false;
                    }

                    elementsToWrite = Math.Min(_elementsPerBlock, _elementsRemaining);

                    // If we have reached the end of the file, start from the beginning
                    if ((_elementsRemaining - elementsToWrite) == 0)
                    {
                        _elementsRemaining = _elementsInFile;
                        _readFromStart = true;
                    }
                    else
                        _elementsRemaining -= elementsToWrite;

                    _memoryMappedAccessor.ReadArray<ComplexInt16>(_fileCurrentIndex, _producerBuffer, 0, elementsToWrite / 2);
                    _fileCurrentIndex += elementsToWrite * 2;

                    // Write to the Rfsg Arb memory
                    _rfsgSession.Arb.WriteWaveform<ComplexInt16>(StreamingWaveformName, _producerBuffer);
                }

                // Create the queue used by the producer and consumer
                _queueHandle = new ConcurrentQueue<ComplexInt16[]>();

                _consumerBuffer = new ComplexInt16[_blockSize];

                _running = true;
                _producerDone = false;
                _consumerDone = false;
                generatingLed.BackColor = Color.Lime;

                // Setup the thread that will handle the file I/O
                object sync = new object();
                ThreadPool.QueueUserWorkItem(ProducerLoop, sync);

                // Wait till the producer populates the queue before invoking consumer
                while (_queueHandle.Count < 90)
                    Thread.Sleep(50);

                _rfsgSession.Initiate();

                // Setup the thread that will hand the data from the queue
                ThreadPool.QueueUserWorkItem(ConsumerLoop, sync);

                // Activate stop button
                EnableControls(false);
                stopButton.Focus();
            }
            catch (Exception ex)
            {
                ShowError("StartGeneration()", ex);
            }
            finally
            {
                if (fStream != null)
                    fStream.Close();
            }
        }

        void ConsumerLoop(object sync)
        {
            try
            {

                // Unsubscribe from warning events before going into the loop. This is to make it work similar to CVI example.
                // Otherwise the example will keep giving warning for a wrong configuration.
                _rfsgSession.DriverOperation.Warning -= DriverOperation_Warning;

                while ((_running) && (!_producerDone))
                {
                    lock (sync)
                    {
                        // If there are no items in the queue, wait till the producer populates the queue
                        while (_queueHandle.IsEmpty)
                        {
                            Monitor.Wait(sync);
                        }
                        // Write new data there is enough data. The Write function will block until there is enough space available on the AWG.
                        if (_queueHandle.Count > 0)
                        {
                            if (_queueHandle.TryDequeue(out _consumerBuffer))
                            {
                                // Now that the queue isnt full, let the producer run
                                if (_queueHandle.Count < 100)
                                {
                                    Monitor.Pulse(sync);
                                }

                                // Write the data retrieved from the queue
                                _rfsgSession.Arb.WriteWaveform<ComplexInt16>(StreamingWaveformName, _consumerBuffer);
                            }
                        }
                        _rfsgSession.CheckGenerationStatus();
                    }
                }

                // Re-Subscribe to Rfsg warnings.
                _rfsgSession.DriverOperation.Warning += new EventHandler<RfsgWarningEventArgs>(DriverOperation_Warning);

                _consumerDone = true;
            }
            catch (Exception ex)
            {
                _consumerDone = true;
                _running = false;
                ShowErrorSafe("ConsumerLoop()", ex);
            }
        }

        void ProducerLoop(object sync)
        {
            int elementsToWrite;

            while ((_running) && (!_consumerDone))
            {
                lock (sync)
                {
                    // If the queue is getting full, wait for the consumer to dequeue some elements
                    while (_queueHandle.Count > MaximumQueueSize && _running && !_consumerDone)
                    {
                        Monitor.Wait(sync, WaitForConsumerInterval);
                    }
                }

                if (_readFromStart)
                {
                    _fileCurrentIndex = 0;
                    _readFromStart = false;
                }

                elementsToWrite = Math.Min(_elementsPerBlock, _elementsRemaining);

                if ((_elementsRemaining - elementsToWrite) == 0)
                {
                    _elementsRemaining = _elementsInFile;
                    _readFromStart = true;
                }
                else
                    _elementsRemaining -= elementsToWrite;

                lock (sync)
                {
                    _memoryMappedAccessor.ReadArray<ComplexInt16>(_fileCurrentIndex, _producerBuffer, 0, elementsToWrite / 2);
                    _fileCurrentIndex += elementsToWrite * 2;
                    _queueHandle.Enqueue(_producerBuffer);

                    // Now that queue has some elements, let the consumer read from it
                    Monitor.Pulse(sync);
                }
            }
            _producerDone = true;
        }

        void StopGeneration()
        {
            // Tell the producer to stop
            _running = false;

            // Wait for the threads to finish before closing things out
            if (_queueHandle != null)
            {
                while ((!_producerDone) || (!_consumerDone))
                {
                    Thread.Sleep(50);
                }
            }

            try
            {
                if (_rfsgSession != null)
                {
                    // Abort generation
                    _rfsgSession.Abort();

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
                ShowErrorSafe("StopGeneration", ex);
            }

            // Close the queue
            if (_queueHandle != null)
            {
                _queueHandle = null;
            }

            // Close the file handle
            if (_memoryMappedAccessor != null)
            {
                _memoryMappedAccessor.Dispose();
            }
            if (_memoryMappedFileObject != null)
            {
                _memoryMappedFileObject.Dispose();
            }

            // Activate all stopped controls
            EnableControls(true);
            generatingLed.BackColor = System.Drawing.SystemColors.Control;
        }

        void ShowErrorSafe(string functionName, Exception exception)
        {
            if (errorTextBox.InvokeRequired)
                errorTextBox.Invoke(new SetErrorText(ShowError), functionName, exception);
            else
                ShowError(functionName, exception);
        }

        void ShowError(string functionName, Exception exception)
        {
            StopGeneration();

            errorTextBox.Text = "Error in " + functionName + ": " + exception.Message;
            EnableControls(true);
        }

        #endregion

        void DriverOperation_Warning(object sender, RfsgWarningEventArgs e)
        {
            // Display the rfsg warning
            errorTextBox.Text = e.Message;
        }

        #region Form Events

        private void browseButton_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(pathTextBox.Text));
            openFileDialog.FileName = Path.GetFileName(pathTextBox.Text);
            openFileDialog.Filter = "Binary Files (*.bin)|*.bin";
            openFileDialog.Title = "Select a waveform file...";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pathTextBox.Text = openFileDialog.FileName;
            }
            openFileDialog.Dispose();
        }

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

        #endregion

        private void EnableControls(bool enabled)
        {
            startButton.Enabled = enabled;
            stopButton.Enabled = !enabled;
            resourceNameComboBox.Enabled = enabled;
            pathTextBox.Enabled = enabled;
            frequencyNumeric.Enabled = enabled;
            powerLevelNumeric.Enabled = enabled;
            iqRateNumeric.Enabled = enabled;
            preGainNumeric.Enabled = enabled;
            streamingSizeNumeric.Enabled = enabled;
            blockSizeNumeric.Enabled = enabled;

            Application.DoEvents();
        }
    }
}
