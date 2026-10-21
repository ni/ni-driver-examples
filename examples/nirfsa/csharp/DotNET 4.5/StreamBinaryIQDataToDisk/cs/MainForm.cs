/*******************************************************************************
*
* Example program:
*   Stream Binary IQ Data To Disk
*
* Category:
*   NI-RFSA
*
* Description:
*   Use this example to learn how to acquire I/Q data using the RF vector
	signal analyzer and then stream the data to a file. This example shows how
	to configure NI-RFSA for a continuous I/Q acquisition, how to set the
	carrier frequency and the I/Q rate, and how to fetch I/Q data in blocks.
    The data is then stored in the file that you select. This example also
	demonstrates how to read previously acquired data from the file.
                         

* Instructions for running:
*   1. Configure both RFSA device in the MAX for the program to run. 
*
*	2. Configure the Reference Level, Carrier Frequency and the IQ Rate in the UI.
*   
*   3. Configure the Number of Samples Per Block and Total Number of Samples in UI.
*
*   4. Select the Stream to Disk Button in the UI to start the acquisition and put all the data in the file mentioned in the UI.
*   
*   5. The data is displayed in the DataGrid.
*
*   6. Select the Stream From Disk Button in the UI to read the data from the file on the disk. 
*
* I/O Connections Overview:
*   Make sure your signal input terminals match the Physical Channel I/O
*   Controls.  If you have a PXI chassis, ensure that it has been properly
*   identified in MAX.  
*
*******************************************************************************/
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsa;
using NationalInstruments.ModularInstruments.SystemServices.DeviceServices;

namespace NationalInstruments.Examples.StreamBinaryIQDataToDisk
{
    public partial class MainForm : Form
    {
        NIRfsa rfsaSession;
        const string headerFileName = "header.bin";

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

        private string RFSAResourceName
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

        private double CarrierFrequency
        {
            get
            {
                return decimal.ToDouble(this.carrierFrequencyNumeric.Value);
            }
        }

        private double IQRate
        {
            get
            {
                return decimal.ToDouble(this.iqRateNumeric.Value);
            }
        }

        private int NumberOfSamples
        {
            get
            {
                return decimal.ToInt32(this.samplesPerBlockNumeric.Value);
            }
        }

        private string FileName
        {
            get
            {
                return fileNameTextBox.Text;
            }
        }

        private int MaxSamples
        {
            get
            {
                return decimal.ToInt32(this.maxSamplesNumeric.Value);
            }
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

        private void ConfigureForIQ()
        {
            rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel;
            rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ;
            rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFrequency;
            rfsaSession.Configuration.IQ.NumberOfSamples = 1000;
            rfsaSession.Configuration.IQ.NumberOfSamplesIsFinite = false;
            rfsaSession.Configuration.IQ.IQRate = IQRate;
        }

        private void StreamToDisk()
        {
            int samplesToRead = NumberOfSamples;
            int maxSamplesToSave = MaxSamples;
            int sampleSaved = 0;
            using (FileStream writestream = new FileStream(FileName, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(writestream))
                {
                    RfsaWaveformInfo wfInfo;
                    ComplexInt16[] rawData;
                    do
                    {
                        if (sampleSaved + samplesToRead > maxSamplesToSave)
                        {
                            samplesToRead = maxSamplesToSave - sampleSaved;
                        }

                        ComplexWaveform<ComplexInt16> IQData = FetchBinaryIQdata(samplesToRead, out wfInfo);
                        if (sampleSaved == 0)
                            StreamHeaderToDisk(wfInfo);
                        SaveToFile(bw, ref sampleSaved, IQData, samplesToRead, wfInfo);

                        rawData = IQData.GetRawData();
                        this.dataGridViewResults.DataSource = rawData;
                        this.samplesSoFarTextBox.Text = sampleSaved.ToString();
                        this.Refresh();

                    } while (sampleSaved < maxSamplesToSave);
                }
            }
        }

        private void StreamHeaderToDisk(RfsaWaveformInfo wfInfo)
        {
            using (FileStream writestream = new FileStream(headerFileName, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(writestream))
                {
                    bw.Write(wfInfo.XIncrement);
                    bw.Write(wfInfo.Gain);
                    bw.Write(wfInfo.Offset);

                    this.dtTextBox.Text = wfInfo.XIncrement.ToString();
                    this.gainTextBox.Text = wfInfo.Gain.ToString();
                    this.offsetTextBox.Text = wfInfo.Offset.ToString();
                }
            }
        }

        private void InitiateAcquisition()
        {
            rfsaSession.Acquisition.IQ.Initiate();

        }

        private ComplexWaveform<ComplexInt16> FetchBinaryIQdata(int samplesToRead, out RfsaWaveformInfo wfInfo)
        {
            PrecisionTimeSpan timespan = new PrecisionTimeSpan(10.0);
            return rfsaSession.Acquisition.IQ.FetchIQSingleRecordComplexWaveform<ComplexInt16>(0, samplesToRead, timespan, out wfInfo);
        }

        private void SaveToFile(BinaryWriter bw, ref int sampleSaved, ComplexWaveform<ComplexInt16> IQData, int sampleToRead, RfsaWaveformInfo wfInfo)
        {
            ComplexInt16 [] data =  IQData.GetRawData();

            foreach (ComplexInt16 d in data)
            {
                bw.Write(d.Real);
                bw.Write(d.Imaginary);
            }

            sampleSaved += sampleToRead;
        }

        private void streamFromDisk()
        {
            int samplesPerBlock = NumberOfSamples;
            FileStream readstream = null;
            BinaryReader rb = null;
            try
            {
                readstream = new FileStream(FileName, FileMode.Open);
                rb = new BinaryReader(readstream);
                ReadWaveformInfo();
                ComplexInt16[] data;
                int totalRead = 0;
                int samplesToRead = 0;

                long totalLength = readstream.Length;
                long totalSamples = totalLength / 4;
                do
                {
                    if (totalSamples - totalRead > samplesPerBlock)
                        samplesToRead = samplesPerBlock;
                    else
                        samplesToRead = (int)(totalSamples - totalRead);

                    if (totalRead >= totalSamples)
                        break;

                    data = new ComplexInt16[samplesToRead];

                    for (int i = 0; i < samplesToRead; i++)
                    {
                        data[i].Real = rb.ReadInt16();
                        data[i].Imaginary = rb.ReadInt16();
                    }

                    totalRead += samplesToRead;
                    this.dataGridViewResults.DataSource = data;
                    this.Refresh();
                    this.samplesSoFarTextBox.Text = totalRead.ToString();
                } while (samplesToRead == samplesPerBlock);
            }
            catch (Exception e)
            {
                ShowError(e.Message);
                return;
            }
            finally
            {
                if (rb != null)
                    rb.Close();
                if (readstream != null)
                    readstream.Close();
            }
        }

        private void ReadWaveformInfo()
        {
            using (FileStream readstream = new FileStream(headerFileName, FileMode.Open))
            {
                using (BinaryReader rb = new BinaryReader(readstream))
                {
                    this.dtTextBox.Text = rb.ReadDouble().ToString();
                    this.gainTextBox.Text = rb.ReadDouble().ToString();
                    this.offsetTextBox.Text = rb.ReadDouble().ToString();
                }
            }
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

        private void streamToDiskButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                ChangeControlState(false);
                InitializeRfsaSession();
                ConfigureForIQ();
                InitiateAcquisition();
                StreamToDisk();
                CloseSession();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                CloseSession();
            }
            finally
            {
                ChangeControlState(true);
            }
        }

        private void readFromDiskButton_Click(object sender, EventArgs e)
        {
            ChangeControlState(false);
            streamFromDisk();
            ChangeControlState(true);
        }

        private void ChangeControlState(bool isEnabled)
        {
            this.readFromDiskButton.Enabled = isEnabled;
            this.streamToDiskButton.Enabled = isEnabled;
            this.resourceNameComboBox.Enabled = isEnabled;
            this.referenceLevelNumeric.Enabled = isEnabled;
            this.carrierFrequencyNumeric.Enabled = isEnabled;
            this.iqRateNumeric.Enabled = isEnabled;
            this.maxSamplesNumeric.Enabled = isEnabled;
            this.samplesPerBlockNumeric.Enabled = isEnabled;
            this.fileNameTextBox.Enabled = isEnabled;
        }

        private static void ShowError(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "Unexpected Error";
            MessageBox.Show(message, "Error"); ;
        }
    }
}