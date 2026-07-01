using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.RFToolkits.BT.Generation;
using System.Text.RegularExpressions;
using System.Runtime.ConstrainedExecution;


namespace NationalInstruments.Examples.BTGenerateLEHDTPacketFormat1
{
    public partial class MainForm : Form
    {
        NIRfsg rfsgSession;
        niBTSG btsgSession;

        string script;
        string resourceName, clkOutTerm;
        string referenceClockSource, exportClock, waveformName;
        string model;
        double iDcOffset, qDcOffset, iQGainImbalance, carrierFrequencyOffset, carrierToNoiseRatio, powerLevel, quadratureSkew,
               headroom, externalAttenuation, carrierFrequency, actualHeadroom, upConverterCenterFrequency, upConverterCenterFrequencyOffset,
               hdtPhyInterval;
        int channelNumber, allIqImpairmentsEnabled, awgnEnabled, autoHeadroomEnabled, payloadLengthMode,
            outputPort, terminalConfiguration, zadoffChuIndex, numberOfPayloads, format1PayloadZoneConfigurationMode,
            oversamplingFactor, dataRate, payloadZoneLength;
        long physicalChannelAddress;
        int[] payloadLength, txLenSequenceNumber, numberOfBlocks, blockSize, lastBlockSize, txBlockMap, actualPayloadLengthBytes;



        public MainForm()
        {
            InitializeComponent();
            ConfigureNumericUpDown();
            ConfigureAutoheadroomEnabComboBox();
            ConfigureRefSourceComboBox();
            ConfigureClkOutTerminalComboBox();
            ConfigureAllIqImpairEnComboBox();
            ConfigureAwgnEnabledComboBox();
            ConfigureOutputPortComboBox();
            ConfigureTerminalConfigurationComboBox();
            ConfigureFormat1PayloadZoneConfigurationModeComboBox();
            ConfigurePayloadLengthModeComboBox();
        }

        #region UI Initial Value Config Section
        private void ConfigureNumericUpDown()
        {
            chnNumberNumeric.Minimum = 0;
            chnNumberNumeric.Maximum = 79;
            chnNumberNumeric.Increment = 1;
            chnNumberNumeric.Value = 3;
            chnNumberNumeric.DecimalPlaces = 0;

            powerLevelNumeric.Minimum = -179;
            powerLevelNumeric.Maximum = 25;
            powerLevelNumeric.Increment = 1;
            powerLevelNumeric.Value = 0;
            powerLevelNumeric.DecimalPlaces = 2;

            externalAttnNumeric.Minimum = decimal.MinValue;
            externalAttnNumeric.Maximum = decimal.MaxValue;
            externalAttnNumeric.Increment = 0.1M;
            externalAttnNumeric.Value = 0;
            externalAttnNumeric.DecimalPlaces = 2;

            headroomNumeric.Minimum = decimal.MinValue;
            headroomNumeric.Maximum = decimal.MaxValue;
            headroomNumeric.Increment = 1;
            headroomNumeric.Value = 0;
            headroomNumeric.DecimalPlaces = 2;

            quadratureSkewNumeric.Minimum = decimal.MinValue;
            quadratureSkewNumeric.Maximum = decimal.MaxValue;
            quadratureSkewNumeric.Increment = 0.5M;
            quadratureSkewNumeric.Value = 0;
            quadratureSkewNumeric.DecimalPlaces = 2;

            iDcOffsetNumeric.Minimum = decimal.MinValue;
            iDcOffsetNumeric.Maximum = decimal.MaxValue;
            iDcOffsetNumeric.Increment = 0.5M;
            iDcOffsetNumeric.Value = 0;
            iDcOffsetNumeric.DecimalPlaces = 2;

            qDcOffsetNumeric.Minimum = decimal.MinValue;
            qDcOffsetNumeric.Maximum = decimal.MaxValue;
            qDcOffsetNumeric.Increment = 0.5M;
            qDcOffsetNumeric.Value = 0;
            qDcOffsetNumeric.DecimalPlaces = 2;

            iqGaimbalanceNumeric.Minimum = decimal.MinValue;
            iqGaimbalanceNumeric.Maximum = decimal.MaxValue;
            iqGaimbalanceNumeric.Increment = 0.5M;
            iqGaimbalanceNumeric.Value = 0;
            iqGaimbalanceNumeric.DecimalPlaces = 2;

            carrierFreqOffNumeric.Minimum = decimal.MinValue;
            carrierFreqOffNumeric.Maximum = decimal.MaxValue;
            carrierFreqOffNumeric.Increment = 1.000E+0M;
            carrierFreqOffNumeric.Value = 0;
            carrierFreqOffNumeric.DecimalPlaces = 3;

            cnrNumeric.Minimum = decimal.MinValue;
            cnrNumeric.Maximum = decimal.MaxValue;
            cnrNumeric.Increment = 0.5M;
            cnrNumeric.Value = 50;
            cnrNumeric.DecimalPlaces = 2;

            numberOfPayloadNumericUpDown.Minimum = 1;
            numberOfPayloadNumericUpDown.Maximum = 100;
            numberOfPayloadNumericUpDown.Increment = 1;
            numberOfPayloadNumericUpDown.Value = 1;
            numberOfPayloadNumericUpDown.DecimalPlaces = 0;

            OversamplingFactorNumeric.Minimum = 1;
            OversamplingFactorNumeric.Maximum = 100;
            OversamplingFactorNumeric.Increment = 1;
            OversamplingFactorNumeric.Value = 8;
            OversamplingFactorNumeric.DecimalPlaces = 0;
        }

        private void ConfigureAutoheadroomEnabComboBox()
        {
            //var autoheadroomEnabValueList = new List<DictionaryEntry>();
            autoheadroomEnabComboBox.DataSource = GetTrueFalseValueList();
            autoheadroomEnabComboBox.DisplayMember = "Key";
            autoheadroomEnabComboBox.ValueMember = "Value";
            autoheadroomEnabComboBox.SelectedIndex = 1;
        }

        private List<DictionaryEntry> GetTrueFalseValueList()
        {
            var trueFalseValueList = new List<DictionaryEntry>();
            trueFalseValueList.Add(new DictionaryEntry("False", niBTSGConstants.False));
            trueFalseValueList.Add(new DictionaryEntry("True", niBTSGConstants.True));
            return trueFalseValueList;
        }

        private void ConfigureRefSourceComboBox()
        {
            var refSourceValueList = new List<DictionaryEntry>();
            refSourceValueList.Add(new DictionaryEntry("OnBoardClock", RfsgFrequencyReferenceSource.OnboardClock));
            refSourceValueList.Add(new DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn));
            refSourceValueList.Add(new DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock));
            refSourceValueList.Add(new DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn));
            refSourceComboBox.DataSource = refSourceValueList;
            refSourceComboBox.DisplayMember = "Key";
            refSourceComboBox.ValueMember = "Value";
            refSourceComboBox.SelectedIndex = 0;
        }

        private void ConfigureClkOutTerminalComboBox()
        {
            var clkOutTerminalValueList = new List<DictionaryEntry>();
            clkOutTerminalValueList.Add(new DictionaryEntry("Do not export clock", RfsgOutputTerminal.DoNotExport));
            clkOutTerminalValueList.Add(new DictionaryEntry("RefOut", RfsgOutputTerminal.ReferenceOut));
            clkOutTerminalValueList.Add(new DictionaryEntry("RefOut2", RfsgOutputTerminal.ReferenceOut2));
            clkOutTerminalValueList.Add(new DictionaryEntry("ClkOut", RfsgOutputTerminal.ClockOut));
            clkOutTerminalValueList.Add(new DictionaryEntry("PFI0", RfsgOutputTerminal.PFI0));
            clkOutTerminalValueList.Add(new DictionaryEntry("PFI1", RfsgOutputTerminal.PFI1));
            clkOutTerminalValueList.Add(new DictionaryEntry("PFI4", RfsgOutputTerminal.PFI4));
            clkOutTerminalValueList.Add(new DictionaryEntry("PFI5", RfsgOutputTerminal.PFI5));
            clkOutTerminalValueList.Add(new DictionaryEntry("PXI_Trig0", RfsgOutputTerminal.PxiTriggerLine0));
            clkOutTerminalValueList.Add(new DictionaryEntry("PXI_Trig1", RfsgOutputTerminal.PxiTriggerLine1));
            clkOutTerminalValueList.Add(new DictionaryEntry("PXI_Trig2", RfsgOutputTerminal.PxiTriggerLine2));
            clkOutTerminalValueList.Add(new DictionaryEntry("PXI_Trig3", RfsgOutputTerminal.PxiTriggerLine3));
            clkOutTerminalValueList.Add(new DictionaryEntry("PXI_Trig4", RfsgOutputTerminal.PxiTriggerLine4));
            clkOutTerminalValueList.Add(new DictionaryEntry("PXI_Trig5", RfsgOutputTerminal.PxiTriggerLine5));
            clkOutTerminalValueList.Add(new DictionaryEntry("PXI_Trig6", RfsgOutputTerminal.PxiTriggerLine6));
            clkOutTerminalComboBox.DataSource = clkOutTerminalValueList;
            clkOutTerminalComboBox.DisplayMember = "Key";
            clkOutTerminalComboBox.ValueMember = "Value";
            clkOutTerminalComboBox.SelectedIndex = 0;
        }

        private void ConfigureAllIqImpairEnComboBox()
        {
            //var allIqImpairEnValueList = new List<DictionaryEntry>();
            allIqImpairEnComboBox.DataSource = GetTrueFalseValueList();
            allIqImpairEnComboBox.DisplayMember = "Key";
            allIqImpairEnComboBox.ValueMember = "Value";
            allIqImpairEnComboBox.SelectedIndex = 0;
        }

        private void ConfigureAwgnEnabledComboBox()
        {
            //var awgnEnabledValueList = new List<DictionaryEntry>();
            awgnEnabledComboBox.DataSource = GetTrueFalseValueList();
            awgnEnabledComboBox.DisplayMember = "Key";
            awgnEnabledComboBox.ValueMember = "Value";
            awgnEnabledComboBox.SelectedIndex = 0;
        }

        private void ConfigureOutputPortComboBox()
        {
            var opList = new List<DictionaryEntry>();
            opList.Add(new DictionaryEntry("RF Out", RfsgOutputPort.RFOut));
            opList.Add(new DictionaryEntry("IQ Out", RfsgOutputPort.IQOut));
            outputPortComboBox.DataSource = opList;
            outputPortComboBox.DisplayMember = "Key";
            outputPortComboBox.ValueMember = "Value";
            outputPortComboBox.SelectedIndex = 0;
        }

        private void ConfigureTerminalConfigurationComboBox()
        {
            var tcList = new List<DictionaryEntry>();
            tcList.Add(new DictionaryEntry("Differential", RfsgTerminalConfiguration.Differential));
            tcList.Add(new DictionaryEntry("SingleEnded", RfsgTerminalConfiguration.SingleEnded));
            terminalConfigurationComboBox.DataSource = tcList;
            terminalConfigurationComboBox.DisplayMember = "Key";
            terminalConfigurationComboBox.ValueMember = "Value";
            terminalConfigurationComboBox.SelectedIndex = 0;
        }

        private void ConfigureFormat1PayloadZoneConfigurationModeComboBox()
        {
            var format1PayloadZoneConfigurationModeList = new List<DictionaryEntry>();
            format1PayloadZoneConfigurationModeList.Add(new DictionaryEntry("Auto", 0));
            format1PayloadZoneConfigurationModeList.Add(new DictionaryEntry("User Defined", 1));
            Format1PayloadZoneConfigurationModeComboBox.DataSource = format1PayloadZoneConfigurationModeList;
            Format1PayloadZoneConfigurationModeComboBox.DisplayMember = "Key";
            Format1PayloadZoneConfigurationModeComboBox.ValueMember = "Value";
            Format1PayloadZoneConfigurationModeComboBox.SelectedIndex = 0;
        }

        private void ConfigurePayloadLengthModeComboBox()
        {
            var payloadLengthModeList = new List<DictionaryEntry>();
            payloadLengthModeList.Add(new DictionaryEntry("Maximum Length", 0));
            payloadLengthModeList.Add(new DictionaryEntry("User Defined", 1));
            payloadLengthModeComboBox.DataSource = payloadLengthModeList;
            payloadLengthModeComboBox.DisplayMember = "Key";
            payloadLengthModeComboBox.ValueMember = "Value";
            payloadLengthModeComboBox.SelectedIndex = 0;
        }

        #endregion UI Initial Value Config Section

        private void generateButton_Click(object sender, EventArgs e)
        {
            try
            {
                //Deactivate the 'Start' button
                generateButton.Enabled = false;
                //Activate the 'Stop' button
                stopButton.Enabled = true;

                //Reset the error message text box
                errorTextBox.Text = "No Error";

                //Read the Panel control values
                resourceName = (string)rfsgResourceTextBox.Text;
                referenceClockSource = refSourceComboBox.SelectedValue.ToString();
                exportClock = clkOutTerminalComboBox.SelectedValue.ToString();
                numberOfPayloads = (int)numberOfPayloadNumericUpDown.Value;
                format1PayloadZoneConfigurationMode = (int)Format1PayloadZoneConfigurationModeComboBox.SelectedValue;
                payloadLengthMode = (int)payloadLengthModeComboBox.SelectedValue;
                oversamplingFactor = (int)OversamplingFactorNumeric.Value;
                waveformName = (string)waveNameTextBox.Text;
                script = (string)scriptTextBox.Text;
                carrierFrequency = double.Parse(carrierFreqTextBox.Text);
                externalAttenuation = (double)externalAttnNumeric.Value;
                headroom = (double)headroomNumeric.Value;
                quadratureSkew = (double)quadratureSkewNumeric.Value;
                iDcOffset = (double)iDcOffsetNumeric.Value;
                qDcOffset = (double)qDcOffsetNumeric.Value;
                iQGainImbalance = (double)iqGaimbalanceNumeric.Value;
                carrierFrequencyOffset = (double)carrierFreqOffNumeric.Value;
                carrierToNoiseRatio = (double)cnrNumeric.Value;
                channelNumber = (int)chnNumberNumeric.Value;
                powerLevel = (double)powerLevelNumeric.Value;
                awgnEnabled = (int)awgnEnabledComboBox.SelectedValue;
                allIqImpairmentsEnabled = (int)allIqImpairEnComboBox.SelectedValue;
                actualHeadroom = System.Convert.ToDouble(actualHeadroomTextBox.Text);
                clkOutTerm = clkOutTerminalComboBox.SelectedValue.ToString();
                autoHeadroomEnabled = (int)autoheadroomEnabComboBox.SelectedValue;
                outputPort = (int)outputPortComboBox.SelectedValue;
                terminalConfiguration = (int)terminalConfigurationComboBox.SelectedValue;
                dataRate = (int)dataRateNumeric.Value;
                zadoffChuIndex = (int)zadoffChuIndexNumeric.Value;
                physicalChannelAddress = (long)physicalChannelAddressNumeric.Value;
                hdtPhyInterval = (double)HdtPhyIntervalNumeric.Value;

                //  BT Generation 
                StartGeneration();

                // Start the status checking timer
                timer.Enabled = true;
            }
            catch (System.Exception exception)
            {
                ShowError("StartGeneration()", exception);
            }
        }


        private void stopButton_Click(object sender, System.EventArgs e)
        {
            if (rfsgSession != null)
            {
                rfsgSession.Abort();
            }
            StopGeneration();
        }

        private void StartGeneration()
        {
            /*Bluetooth Session*/
            if (btsgSession == null)
            {
                btsgSession = new niBTSG(niBTSGConstants.ToolkitCompatibilityVersion020000);
            }
            niBTSG.ChannelNumberToCarrierFrequency(channelNumber, niBTSGConstants.StandardLE, out carrierFrequency);
            carrierFreqTextBox.Text = carrierFrequency.ToString();

            btsgSession.SetCarrierMode(null, niBTSGConstants.CarrierModeBurst);
            btsgSession.SetPacketType(null, niBTSGConstants.PacketTypeLEHdt);

            //Set Data Rate
            btsgSession.SetDataRate(null, dataRate);

            /* Set Payload Header */
            btsgSession.SetPayloadLengthMode(null, payloadLengthMode);

            /* Set Oversampling Factor */
            btsgSession.SetOversamplingFactor(null, oversamplingFactor);

            /* High Data Throughput*/
            //Set Zadoff-Chu Index
            btsgSession.SetZadoffChuIndex(null, zadoffChuIndex);

            //Set Physical Channel Address
            btsgSession.SetPhysicalChannelAddress(null, physicalChannelAddress);

            //Set HDT Packet Format
            btsgSession.SetHdtPacketFormat(null, niBTSGConstants.HdtPacketFormat_Format1);

            //Set HDT PHY Interval
            btsgSession.SetHdtPhyInterval(null, hdtPhyInterval);

            btsgSession.SetNumberOfPayloads(null, numberOfPayloads);

            /* User Defined Payload Zone Properties Settings */
            // PayloadLengthBytes
            int PayloadLengthBytesArraySize = PayloadLengthBytesGrid.Rows.Count;
            payloadLength = new int[PayloadLengthBytesArraySize];
            for (int rows = 0; rows < PayloadLengthBytesArraySize; rows++)
            {
                payloadLength[rows] = Convert.ToInt32(PayloadLengthBytesGrid.Rows[rows].Cells[1].Value.ToString());
            }

            // txLenSequenceNumber
            int txLenSequenceNumberArraySize = TxLenSequenceNumberGrid.Rows.Count;
            txLenSequenceNumber = new int[txLenSequenceNumberArraySize];
            for (int rows = 0; rows < txLenSequenceNumberArraySize; rows++)
            {
                int value = Convert.ToInt32(TxLenSequenceNumberGrid.Rows[rows].Cells[1].Value.ToString());
                switch (value)
                {
                    case 0: //00
                        txLenSequenceNumber[rows] = 0;
                        break;
                    case 1: //01
                        txLenSequenceNumber[rows] = 1;
                        break;
                    case 10:
                        txLenSequenceNumber[rows] = 2;
                        break;
                    case 11:
                        txLenSequenceNumber[rows] = 3;
                        break;
                    default:
                        // Optionally handle unexpected values
                        txLenSequenceNumber[rows] = value;
                        break;
                }
            }

            // numberOfBlocks
            int numberOfBlocksArraySize = NumberOfBlocksGrid.Rows.Count;
            numberOfBlocks = new int[numberOfBlocksArraySize];
            for (int rows = 0; rows < numberOfBlocksArraySize; rows++)
            {
                numberOfBlocks[rows] = Convert.ToInt32(NumberOfBlocksGrid.Rows[rows].Cells[1].Value.ToString());
            }

            // blockSize
            int blockSizeArraySize = BlockSizeGrid.Rows.Count;
            blockSize = new int[blockSizeArraySize];
            for (int rows = 0; rows < blockSizeArraySize; rows++)
            {
                blockSize[rows] = Convert.ToInt32(BlockSizeGrid.Rows[rows].Cells[1].Value.ToString());
            }

            // lastBlockSize
            int lastBlockSizeArraySize = LastBlockSizeGrid.Rows.Count;
            lastBlockSize = new int[lastBlockSizeArraySize];
            for (int rows = 0; rows < lastBlockSizeArraySize; rows++)
            {
                lastBlockSize[rows] = Convert.ToInt32(LastBlockSizeGrid.Rows[rows].Cells[1].Value.ToString());
            }

            // txBlockMap
            int txBlockMapArraySize = TxBlockMapGrid.Rows.Count;
            txBlockMap = new int[txBlockMapArraySize];
            for (int rows = 0; rows < txBlockMapArraySize; rows++)
            {
                txBlockMap[rows] = Convert.ToInt32(TxBlockMapGrid.Rows[rows].Cells[1].Value.ToString(), 16);
            }

            // actualPayloadLengthBytes
            int actualPayloadLengthBytesArraySize = ActualPayloadLengthGrid.Rows.Count;
            actualPayloadLengthBytes = new int[actualPayloadLengthBytesArraySize];
            for (int rows = 0; rows < actualPayloadLengthBytesArraySize; rows++)
            {
                actualPayloadLengthBytes[rows] = Convert.ToInt32(ActualPayloadLengthGrid.Rows[rows].Cells[1].Value.ToString());
            }

            for (int i = 0; i < numberOfPayloads; i++)
            {
                string activeChannel = "payload" + i;
                btsgSession.SetFormat1PayloadZoneConfigurationMode(null, format1PayloadZoneConfigurationMode);
                if (format1PayloadZoneConfigurationMode == 0)
                {
                    if (payloadLengthMode == niBTSGConstants.PayloadLengthModeUserDefined)
                    {
                        btsgSession.SetPayloadLength(activeChannel, payloadLength[i]);
                    }

                }
                else
                {
                    btsgSession.SetTxLenSequenceNumber(activeChannel, txLenSequenceNumber[i]);
                    btsgSession.SetNumberOfBlocks(activeChannel, numberOfBlocks[i]);
                    btsgSession.SetBlockSize(activeChannel, blockSize[i]);
                    btsgSession.SetLastBlockSize(activeChannel, lastBlockSize[i]);
                    btsgSession.SetTxBlockMap(activeChannel, txBlockMap[i]);
                }
            }

            /*Waveform Properties*/
            //Set Headroom Properties
            btsgSession.SetAutoHeadroomEnabled(null, autoHeadroomEnabled);
            if (autoHeadroomEnabled == niBTSGConstants.False)
            {
                btsgSession.SetHeadroom(null, headroom);
            }

            //Set Impairments
            btsgSession.SetAllIqImpairmentsEnabled(null, allIqImpairmentsEnabled);

            if (allIqImpairmentsEnabled == niBTSGConstants.True)
            {
                btsgSession.SetQuadratureSkew(null, quadratureSkew);
                btsgSession.SetIDCOffset(null, iDcOffset);
                btsgSession.SetQDCOffset(null, qDcOffset);
                btsgSession.SetIQGainImbalance(null, iQGainImbalance);
            }
            btsgSession.SetCarrierFrequencyOffset(null, carrierFrequencyOffset);
            btsgSession.SetAwgnEnabled(null, awgnEnabled);
            btsgSession.SetCarrierToNoiseRatio(null, carrierToNoiseRatio);

            /*RFSG Session*/
            if (rfsgSession == null)
            {
                rfsgSession = new NIRfsg(resourceName, false, true);
            }

            //get model
            model = rfsgSession.Identity.InstrumentModel;

            /*RFSG Properties*/
            rfsgSession.FrequencyReference.Configure((RfsgFrequencyReferenceSource)referenceClockSource, 10.0e6);
            rfsgSession.Utility.ExportSignal(RfsgSignalType.ReferenceClock, "", exportClock);

            rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
            rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;

            if (model.Equals("NI PXI-5670") || model.Equals("NI PXI-5671") || model.Equals("NI PXI-5672") || model.Equals("NI PXI-5673") || model.Equals("NI PXIe-5673E"))
            {
                //Do nothing
            }
            else
            {
                rfsgSession.Arb.OutputPort = (RfsgOutputPort)outputPort;
            }

            if (outputPort == (int)RfsgOutputPort.IQOut)
            {
                rfsgSession.IQOutPort["I"].TerminalConfiguration = (RfsgTerminalConfiguration)terminalConfiguration;
            }
            else
            {
                rfsgSession.RF.Frequency = carrierFrequency;
                upConverterCenterFrequencyOffset = 0;
                upConverterCenterFrequency = carrierFrequency + upConverterCenterFrequencyOffset;
                rfsgSession.RF.Upconverter.CenterFrequency = upConverterCenterFrequency;
            }
            rfsgSession.RF.ExternalGain = -(externalAttenuation);

            /*Create & Download Waveform*/
            btsgSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", waveformName);
            btsgSession.GetPayloadZoneLength(null, out payloadZoneLength);
            PayloadZoneLengthValue.Text = payloadZoneLength.ToString();

            actualPayloadLengthBytes = new int[numberOfPayloads];
            ActualPayloadLengthGrid.Rows.Clear();

            for (int i = 0; i < numberOfPayloads; i++)
            {
                string activeChannel = "payload" + i;
                btsgSession.GetActualPayloadLength(activeChannel, out actualPayloadLengthBytes[i]);
                ActualPayloadLengthGrid.Rows.Add(i, actualPayloadLengthBytes[i]);
            }

            /*Execute Script*/
            niBTSG.RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, script, powerLevel);
            rfsgSession.RF.OutputEnabled = true;
            rfsgSession.Initiate();

            /*Start the status checking timer */
            timer.Enabled = true;
        }
        private void StopGeneration()
        {
            //Stop the status checking timer
            timer.Enabled = false;
            if (rfsgSession != null)
            {
                rfsgSession.Abort();
                rfsgSession.RF.OutputEnabled = false;
                rfsgSession.Utility.Commit();
                try
                {
                    niBTSG.RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, waveformName);
                }
                catch
                {
                    // ignoring the exception here - the RFSGClearDatabase API throws an exception
                    // if called more than once on the same waveformName
                }
            }

            // Deactivate 'Stop' button
            stopButton.Enabled = false;

            // Activate 'Start' button
            generateButton.Enabled = true;
        }

        // --------------------------------------------------------------------------
        // Callback function when the MainForm is closed
        // --------------------------------------------------------------------------
        void MainFormClosing(object sender, FormClosingEventArgs e)
        {
            StopGeneration();
            CloseSession();
        }

        void ProcessTimerEvent(object sender, System.EventArgs e)
        {
            CheckGeneration();
        }

        void CheckGeneration()
        {
            try
            {
                RfsgGenerationStatus status = RfsgGenerationStatus.InProgress;
                if (rfsgSession != null)
                    status = rfsgSession.CheckGenerationStatus();
            }
            catch (System.Exception exception)
            {
                ShowError("CheckGeneration()", exception);
            }
        }

        void ShowError(string functionName, System.Exception exception)
        {
            StopGeneration();
            CloseSession();

            // Display error to the user
            string exceptionMessage = string.IsNullOrEmpty(exception.Message) ? "Undefined Error." : exception.Message;
            errorTextBox.Text = "Error in " + functionName + System.Environment.NewLine + exceptionMessage;
            MessageBox.Show(exceptionMessage, functionName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void CloseSession()
        {
            // Close the RFSG session
            if (rfsgSession != null)
            {
                rfsgSession.Abort();
                niBTSG.RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, "");
                rfsgSession.Dispose();
                rfsgSession = null;
            }

            // Close the BT Session
            if (btsgSession != null)
            {
                btsgSession.CloseSession();
                btsgSession = null;
            }
        }

        private void payloadLengthInsertButton_Click(object sender, EventArgs e)
        {
            int IndexNum = PayloadLengthBytesGrid.Rows.Add();
            PayloadLengthBytesGrid.Rows[IndexNum].Cells["payloadLengthBytesValues"].Value = "0";
            PayloadLengthBytesGrid.Rows[IndexNum].Cells["payloadLengthIndex"].Value = IndexNum.ToString();
        }

        private void payloadLengthDeleteButton_Click(object sender, EventArgs e)
        {
            if (PayloadLengthBytesGrid.Rows.Count > 0)
            {
                int IndexNum = PayloadLengthBytesGrid.CurrentCell.RowIndex;
                PayloadLengthBytesGrid.Rows.RemoveAt(IndexNum);
                for (int i = IndexNum; i < PayloadLengthBytesGrid.Rows.Count; i++)
                {
                    PayloadLengthBytesGrid.Rows[i].Cells["payloadLengthIndex"].Value = (i).ToString();
                }
            }
        }

        private void txLenSequenceNumberInsertButton_Click(object sender, EventArgs e)
        {
            int indexNum = TxLenSequenceNumberGrid.Rows.Add();
            TxLenSequenceNumberGrid.Rows[indexNum].Cells["TxLenSequenceNumberValues"].Value = "11";
            TxLenSequenceNumberGrid.Rows[indexNum].Cells["TxLenSequenceNumberIndex"].Value = indexNum.ToString();
        }

        private void txLenSequenceNumberDeleteButton_Click(object sender, EventArgs e)
        {
            if (TxLenSequenceNumberGrid.Rows.Count > 0)
            {
                int indexNum = TxLenSequenceNumberGrid.CurrentCell.RowIndex;
                TxLenSequenceNumberGrid.Rows.RemoveAt(indexNum);
                for (int i = indexNum; i < TxLenSequenceNumberGrid.Rows.Count; i++)
                {
                    TxLenSequenceNumberGrid.Rows[i].Cells["TxLenSequenceNumberIndex"].Value = i.ToString();
                }
            }
        }

        private void numberOfBlocksInsertButton_Click(object sender, EventArgs e)
        {
            int indexNum = NumberOfBlocksGrid.Rows.Add();
            NumberOfBlocksGrid.Rows[indexNum].Cells["NumberOfBlocksValues"].Value = "15";
            NumberOfBlocksGrid.Rows[indexNum].Cells["NumberOfBlocksIndex"].Value = indexNum.ToString();
        }

        private void numberOfBlocksDeleteButton_Click(object sender, EventArgs e)
        {
            if (NumberOfBlocksGrid.Rows.Count > 0)
            {
                int indexNum = NumberOfBlocksGrid.CurrentCell.RowIndex;
                NumberOfBlocksGrid.Rows.RemoveAt(indexNum);
                for (int i = indexNum; i < NumberOfBlocksGrid.Rows.Count; i++)
                {
                    NumberOfBlocksGrid.Rows[i].Cells["NumberOfBlocksValues"].Value = i.ToString();
                }
            }
        }

        private void blockSizeInsertButton_Click(object sender, EventArgs e)
        {
            int indexNum = BlockSizeGrid.Rows.Add();
            BlockSizeGrid.Rows[indexNum].Cells["blockSizeValues"].Value = "511";
            BlockSizeGrid.Rows[indexNum].Cells["blockSizeIndex"].Value = indexNum.ToString();
        }

        private void blockSizeDeleteButton_Click(object sender, EventArgs e)
        {
            if (BlockSizeGrid.Rows.Count > 0)
            {
                int indexNum = BlockSizeGrid.CurrentCell.RowIndex;
                BlockSizeGrid.Rows.RemoveAt(indexNum);
                for (int i = indexNum; i < BlockSizeGrid.Rows.Count; i++)
                {
                    BlockSizeGrid.Rows[i].Cells["blockSizeIndex"].Value = i.ToString();
                }
            }
        }

        private void lastBlockSizeInsertButton_Click(object sender, EventArgs e)
        {
            int indexNum = LastBlockSizeGrid.Rows.Add();
            LastBlockSizeGrid.Rows[indexNum].Cells["LastBlockSizeValues"].Value = "526";
            LastBlockSizeGrid.Rows[indexNum].Cells["LastBlockSizeIndex"].Value = indexNum.ToString();
        }

        private void lastBlockSizeDeleteButton_Click(object sender, EventArgs e)
        {
            if (LastBlockSizeGrid.Rows.Count > 0)
            {
                int indexNum = LastBlockSizeGrid.CurrentCell.RowIndex;
                LastBlockSizeGrid.Rows.RemoveAt(indexNum);
                for (int i = indexNum; i < LastBlockSizeGrid.Rows.Count; i++)
                {
                    LastBlockSizeGrid.Rows[i].Cells["LastBlockSizeIndex"].Value = i.ToString();
                }
            }
        }

        private void txBlockMapInsertButton_Click(object sender, EventArgs e)
        {
            int indexNum = TxBlockMapGrid.Rows.Add();
            TxBlockMapGrid.Rows[indexNum].Cells["txBlockMapValues"].Value = "0xFFFF";
            TxBlockMapGrid.Rows[indexNum].Cells["txBlockMapIndex"].Value = indexNum.ToString();
        }

        private void txBlockMapDeleteButton_Click(object sender, EventArgs e)
        {
            if (TxBlockMapGrid.Rows.Count > 0)
            {
                int indexNum = TxBlockMapGrid.CurrentCell.RowIndex;
                TxBlockMapGrid.Rows.RemoveAt(indexNum);
                for (int i = indexNum; i < TxBlockMapGrid.Rows.Count; i++)
                {
                    TxBlockMapGrid.Rows[i].Cells["txBlockMapIndex"].Value = i.ToString();
                }
            }
        }

        private void actualPayloadLengthInsertButton_Click(object sender, EventArgs e)
        {
            int indexNum = ActualPayloadLengthGrid.Rows.Add();
            ActualPayloadLengthGrid.Rows[indexNum].Cells["ActualPayloadLengthValues"].Value = "0";
            ActualPayloadLengthGrid.Rows[indexNum].Cells["ActualPayloadLengthIndex"].Value = indexNum.ToString();
        }

        private void actualPayloadLengthDeleteButton_Click(object sender, EventArgs e)
        {
            if (ActualPayloadLengthGrid.Rows.Count > 0)
            {
                int indexNum = ActualPayloadLengthGrid.CurrentCell.RowIndex;
                ActualPayloadLengthGrid.Rows.RemoveAt(indexNum);
                for (int i = indexNum; i < ActualPayloadLengthGrid.Rows.Count; i++)
                {
                    ActualPayloadLengthGrid.Rows[i].Cells["ActualPayloadLengthIndex"].Value = i.ToString();
                }
            }
        }

    }
}
