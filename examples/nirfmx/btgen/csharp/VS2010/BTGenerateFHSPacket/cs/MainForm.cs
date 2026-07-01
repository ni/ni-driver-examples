using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.RFToolkits.BT.Generation;


namespace NationalInstruments.Examples.BTGenerateFHSPacket
{
   public partial class MainForm : Form
   {
      NIRfsg rfsgSession;
      niBTSG btsgSession;

      string resourceName, referenceClockSource, exportClock, waveformName, script;

      double powerLevel, carrierFrequency, upConverterCenterFrequency, upConverterCenterFrequencyOffset, actualHeadroom, externalAttenuation, headroom, quadratureSkew, iDcOffset, qDcOffset, iQGainImbalance, carrierFrequencyOffset, carrierToNoiseRatio, iCommonModeOffset, qCommonModeOffset, iOffset, qOffset;

      int channelNumber, allIqImpairmentsEnabled, awgnEnabled, whiteningEnabled, autoHeadroomEnabled, packetHeaderArqn, packetHeaderSeqn, bdAddressLap, bdAddressUap, bdAddressNap, packetType = niBTSGConstants.PacketTypeIdle, HeaderLtAddress, headerFlow, headerSeqn, whiteningclock, outputPort, terminalConfiguration, standard;

      int fhsBdAddressLap, fhsBdAddressUap, fhsBdAddressNap, fhsPayloadLTAddress, fhsPayloadDeviceClass, fhsPayloadScanRepetition, fhsPayloadPageScanMode, fhsPayloadDeviceClock;
      string model;

      public MainForm()
      {
         InitializeComponent();
         ConfigureNumericUpDown();
         ConfigureAutoheadroomEnabComboBox();
         ConfigureRefSourceComboBox();
         ConfigureClkOutTerminalComboBox();
         ConfigureAllIqImpairEnComboBox();
         ConfigureAwgnEnabledComboBox();
         ConfigurePkthdrArqnComboBox();
         ConfigureWhiteEnComboBox();
         ConfigurestopButton();
         ConfigureOutputPortComboBox();
         ConfigureTerminalConfigurationComboBox();
      }

      #region UI Initial Value Config Section

      private void ConfigurestopButton()
      {
         //Deactivate the 'Stop' button
         stopButton.Enabled = false;
      }

      private void ConfigureNumericUpDown()
      {
         chnNumberNumeric.Minimum = 0;
         chnNumberNumeric.Maximum = 79;
         chnNumberNumeric.Increment = 1;
         chnNumberNumeric.Value = 3;
         chnNumberNumeric.DecimalPlaces = 0;

         powerLevelNumeric.Minimum = decimal.MinValue;
         powerLevelNumeric.Maximum = decimal.MaxValue;
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
         carrierFreqOffNumeric.Increment = 1;
         carrierFreqOffNumeric.Value = 0;
         carrierFreqOffNumeric.DecimalPlaces = 3;

         cnrNumeric.Minimum = decimal.MinValue;
         cnrNumeric.Maximum = decimal.MaxValue;
         cnrNumeric.Increment = 0.5M;
         cnrNumeric.Value = 50;
         cnrNumeric.DecimalPlaces = 2;

         bdaddrLapNumeric.Minimum = int.MinValue;
         bdaddrLapNumeric.Maximum = int.MaxValue;
         bdaddrLapNumeric.Increment = 1;
         bdaddrLapNumeric.Value = 0;
         bdaddrLapNumeric.DecimalPlaces = 0;

         bdaddrUapNumeric.Minimum = int.MinValue;
         bdaddrUapNumeric.Maximum = int.MaxValue;
         bdaddrUapNumeric.Increment = 1;
         bdaddrUapNumeric.Value = 0;
         bdaddrUapNumeric.DecimalPlaces = 0;

         bdaddrNapNumeric.Minimum = 0;
         bdaddrNapNumeric.Maximum = 30;
         bdaddrNapNumeric.Increment = 0;
         bdaddrNapNumeric.Value = 0;
         bdaddrNapNumeric.DecimalPlaces = 0;

         pktLtAddrNumeric.Minimum = int.MinValue;
         pktLtAddrNumeric.Maximum = int.MaxValue;
         pktLtAddrNumeric.Increment = 1;
         pktLtAddrNumeric.Value = 0;
         pktLtAddrNumeric.DecimalPlaces = 0;

         pktHdrFlowNumeric.Minimum = int.MinValue;
         pktHdrFlowNumeric.Maximum = int.MaxValue;
         pktHdrFlowNumeric.Increment = 1;
         pktHdrFlowNumeric.Value = 0;
         pktHdrFlowNumeric.DecimalPlaces = 0;

         pktHdrSeqnNumeric.Minimum = int.MinValue;
         pktHdrSeqnNumeric.Maximum = int.MaxValue;
         pktHdrSeqnNumeric.Increment = 1;
         pktHdrSeqnNumeric.Value = 0;
         pktHdrSeqnNumeric.DecimalPlaces = 0;

         fhsBdaddrLapNumeric.Minimum = int.MinValue;
         fhsBdaddrLapNumeric.Maximum = int.MaxValue;
         fhsBdaddrLapNumeric.Increment = 1;
         fhsBdaddrLapNumeric.Value = 0;
         fhsBdaddrLapNumeric.DecimalPlaces = 0;

         fhsBdaddrUapNumeric.Minimum = int.MinValue;
         fhsBdaddrUapNumeric.Maximum = int.MaxValue;
         fhsBdaddrUapNumeric.Increment = 1;
         fhsBdaddrUapNumeric.Value = 0;
         fhsBdaddrUapNumeric.DecimalPlaces = 0;

         fhsBdaddrNapNumeric.Minimum = 0;
         fhsBdaddrNapNumeric.Maximum = 30;
         fhsBdaddrNapNumeric.Increment = 0;
         fhsBdaddrNapNumeric.Value = 0;
         fhsBdaddrNapNumeric.DecimalPlaces = 0;

         fhsLtAddrNumeric.Minimum = int.MinValue;
         fhsLtAddrNumeric.Maximum = int.MaxValue;
         fhsLtAddrNumeric.Increment = 1;
         fhsLtAddrNumeric.Value = 0;
         fhsLtAddrNumeric.DecimalPlaces = 0;

         fhsDevClassNumeric.Minimum = int.MinValue;
         fhsDevClassNumeric.Maximum = int.MaxValue;
         fhsDevClassNumeric.Increment = 1;
         fhsDevClassNumeric.Value = 0;
         fhsDevClassNumeric.DecimalPlaces = 0;

         fhsScanRepNumeric.Minimum = int.MinValue;
         fhsScanRepNumeric.Maximum = int.MaxValue;
         fhsScanRepNumeric.Increment = 1;
         fhsScanRepNumeric.Value = 0;
         fhsScanRepNumeric.DecimalPlaces = 0;

         fhsPgScanNumeric.Minimum = int.MinValue;
         fhsPgScanNumeric.Maximum = int.MaxValue;
         fhsPgScanNumeric.Increment = 1;
         fhsPgScanNumeric.Value = 0;
         fhsPgScanNumeric.DecimalPlaces = 0;

         fhsDevClockNumeric.Minimum = int.MinValue;
         fhsDevClockNumeric.Maximum = int.MaxValue;
         fhsDevClockNumeric.Increment = 1;
         fhsDevClockNumeric.Value = 0;
         fhsDevClockNumeric.DecimalPlaces = 0;

         whiteClkNumeric.Minimum = int.MinValue;
         whiteClkNumeric.Maximum = int.MaxValue;
         whiteClkNumeric.Increment = 1;
         whiteClkNumeric.Value = 0;
         whiteClkNumeric.DecimalPlaces = 0;
      }

      private void ConfigureAutoheadroomEnabComboBox()
      {
         var autoheadroomEnabValueList = new List<DictionaryEntry>();
         autoheadroomEnabComboBox.DataSource = GetTrueFalseValueList();
         autoheadroomEnabComboBox.DisplayMember = "Key";
         autoheadroomEnabComboBox.ValueMember = "Value";
         autoheadroomEnabComboBox.SelectedIndex = 1;
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
         refSourceComboBox.SelectedIndex = 2;
      }

      private void ConfigureClkOutTerminalComboBox()
      {
         var clkOutTerminalValueList = new List<DictionaryEntry>();
         clkOutTerminalValueList.Add(new DictionaryEntry("Do not export clock", RfsgOutputTerminal.DoNotExport));
         clkOutTerminalValueList.Add(new DictionaryEntry("RefOut", RfsgOutputTerminal.ReferenceOut));
         clkOutTerminalValueList.Add(new DictionaryEntry("RefOut2", RfsgOutputTerminal.ReferenceOut2));
         clkOutTerminalValueList.Add(new DictionaryEntry("ClkOut", RfsgOutputTerminal.ClockOut));
         clkOutTerminalComboBox.DataSource = clkOutTerminalValueList;
         clkOutTerminalComboBox.DisplayMember = "Key";
         clkOutTerminalComboBox.ValueMember = "Value";
         clkOutTerminalComboBox.SelectedIndex = 0;
      }

      private void ConfigureAllIqImpairEnComboBox()
      {
         var allIqImpairEnValueList = new List<DictionaryEntry>();
         allIqImpairEnComboBox.DataSource = GetTrueFalseValueList();
         allIqImpairEnComboBox.DisplayMember = "Key";
         allIqImpairEnComboBox.ValueMember = "Value";
         allIqImpairEnComboBox.SelectedIndex = 0;
      }

      private void ConfigureAwgnEnabledComboBox()
      {
         var awgnEnabledValueList = new List<DictionaryEntry>();
         awgnEnabledComboBox.DataSource = GetTrueFalseValueList();
         awgnEnabledComboBox.DisplayMember = "Key";
         awgnEnabledComboBox.ValueMember = "Value";
         awgnEnabledComboBox.SelectedIndex = 0;
      }

      private void ConfigurePkthdrArqnComboBox()
      {
         var pktHdrArqnValueList = new List<DictionaryEntry>();
         pktHdrArqnValueList.Add(new DictionaryEntry("ACK", niBTSGConstants.PacketHeaderArqnAck));
         pktHdrArqnValueList.Add(new DictionaryEntry("NAK", niBTSGConstants.PacketHeaderArqnNak));
         pktHdrArqnComboBox.DataSource = pktHdrArqnValueList;
         pktHdrArqnComboBox.DisplayMember = "Key";
         pktHdrArqnComboBox.ValueMember = "Value";
         pktHdrArqnComboBox.SelectedValue = niBTSGConstants.PacketHeaderArqnNak;
      }

      private void ConfigureWhiteEnComboBox()
      {
         var whiteEnValueList = new List<DictionaryEntry>();
         whiteEnComboBox.DataSource = GetTrueFalseValueList();
         whiteEnComboBox.DisplayMember = "Key";
         whiteEnComboBox.ValueMember = "Value";
         whiteEnComboBox.SelectedIndex = 0;
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

      private List<DictionaryEntry> GetTrueFalseValueList()
      {
         var trueFalseValueList = new List<DictionaryEntry>();
         trueFalseValueList.Add(new DictionaryEntry("False", niBTSGConstants.False));
         trueFalseValueList.Add(new DictionaryEntry("True", niBTSGConstants.True));
         return trueFalseValueList;
      }
      #endregion

      void StartGeneration()
      {
         /*BTSG Session*/
         if (btsgSession == null)
         {
            btsgSession = new niBTSG(niBTSGConstants.ToolkitCompatibilityVersion020000);
         }
         //niBTSG.ChannelNumberToCarrierFrequency(channelNumber, niBTSGConstants.StandardBasicEdr, out carrierFrequency);

         //Set BD Address
         btsgSession.SetBDAddress(null, bdAddressLap, bdAddressUap, bdAddressNap);
         btsgSession.SetPacketType(null, niBTSGConstants.PacketTypeFhs);

         //Set Carrier Mode
         btsgSession.SetCarrierMode(null, niBTSGConstants.CarrierModeBurst);

         //Set Packet Header
         btsgSession.SetPacketHeaderLTAddress(null, HeaderLtAddress);
         btsgSession.SetPacketHeaderFlow(null, headerFlow);
         btsgSession.SetPacketHeaderArqn(null, packetHeaderArqn);
         btsgSession.SetPacketHeaderSeqn(null, packetHeaderSeqn);

         //Set FHS Payload
         btsgSession.SetFhsPayloadBDAddress(null, fhsBdAddressLap, fhsBdAddressUap, fhsBdAddressNap);
         btsgSession.SetFhsPayloadLTAddress(null, fhsPayloadLTAddress);
         btsgSession.SetFhsPayloadDeviceClass(null, fhsPayloadDeviceClass);
         btsgSession.SetFhsPayloadScanRepetition(null, fhsPayloadScanRepetition);
         btsgSession.SetFhsPayloadPageScanMode(null, fhsPayloadPageScanMode);
         btsgSession.SetFhsPayloadDeviceClock(null, fhsPayloadDeviceClock);

         /*Waveform Properties*/
         //Set Headroom Properties
         btsgSession.SetAutoHeadroomEnabled(null, autoHeadroomEnabled);
         if (autoHeadroomEnabled == niBTSGConstants.False)
         {
            btsgSession.SetHeadroom(null, headroom);
         }

         btsgSession.SetCarrierFrequencyOffset(null, carrierFrequencyOffset);

         //Set Impairments
         btsgSession.SetAllIqImpairmentsEnabled(null, allIqImpairmentsEnabled);
         if (allIqImpairmentsEnabled == niBTSGConstants.True)
         {
            btsgSession.SetQuadratureSkew(null, quadratureSkew);
            btsgSession.SetIDCOffset(null, iDcOffset);
            btsgSession.SetQDCOffset(null, qDcOffset);
            btsgSession.SetIQGainImbalance(null, iQGainImbalance);
         }
         btsgSession.SetCarrierFrequencyOffset(null, 0);
         btsgSession.SetAwgnEnabled(null, awgnEnabled);
         btsgSession.SetCarrierToNoiseRatio(null, carrierToNoiseRatio);

         //Whitening Properties
         btsgSession.SetWhiteningEnabled(null, whiteningEnabled);
         btsgSession.SetWhiteningClock(null, whiteningclock);
         niBTSG.ChannelNumberToCarrierFrequency(channelNumber, standard, out carrierFrequency);
         carrierFreqTextBox.Text = carrierFrequency.ToString();
         /*RFSG Session*/
         if (rfsgSession == null)
         {
            rfsgSession = new NIRfsg(resourceName, false, true);
         }

         /*RFSG Properties*/
         //get model
         model = rfsgSession.Identity.InstrumentModel;

         /*RFSG Properties*/
         rfsgSession.FrequencyReference.Configure((RfsgFrequencyReferenceSource)referenceClockSource, 10.0e6);
         rfsgSession.Utility.ExportSignal(RfsgSignalType.ReferenceClock, "", exportClock);
         if (model.Equals("NI PXIe-5644R") || model.Equals("NI PXIe-5645R") || model.Equals("NI PXIe-5646R"))
         {
            if (outputPort == (int)RfsgOutputPort.IQOut)
            {
               rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
               rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
               rfsgSession.IQOutPort["I"].TerminalConfiguration = (RfsgTerminalConfiguration)terminalConfiguration;
               rfsgSession.IQOutPort["I"].CommonModeOffset = iCommonModeOffset;
               rfsgSession.IQOutPort["I"].Offset = iOffset;
               rfsgSession.IQOutPort["Q"].CommonModeOffset = qCommonModeOffset;
               rfsgSession.IQOutPort["Q"].Offset = qOffset;
               carrierFrequency = 0;
            }
            else
            {
               rfsgSession.RF.Frequency = carrierFrequency;
               rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
               rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
               upConverterCenterFrequencyOffset = 0;
               upConverterCenterFrequency = carrierFrequency + upConverterCenterFrequencyOffset;
               rfsgSession.RF.Upconverter.CenterFrequency = upConverterCenterFrequency;
            }
         }
         else
         {
            rfsgSession.RF.Frequency = carrierFrequency;
            rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
            rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
            upConverterCenterFrequencyOffset = 0;
            upConverterCenterFrequency = carrierFrequency + upConverterCenterFrequencyOffset;
            rfsgSession.RF.Upconverter.CenterFrequency = upConverterCenterFrequency;
         }
         rfsgSession.RF.ExternalGain = -(externalAttenuation);

         /*Create & Download Waveform*/
         btsgSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", waveformName);
         btsgSession.GetDouble(null, niBTSGProperties.ActualHeadroom, out actualHeadroom);
         actualHeadroomTextBox.Text = actualHeadroom.ToString();

         /*Create and Download Waveform (IDLE)*/
         btsgSession.SetPacketType("", packetType);
         btsgSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", "idle");

         /*Execute Script*/
         niBTSG.RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, script, powerLevel);
         rfsgSession.RF.OutputEnabled = true;
         rfsgSession.Initiate();

         /*Start the status checking timer */
         timer.Enabled = true;
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
         MessageBox.Show(exceptionMessage, functionName, MessageBoxButtons.OK, MessageBoxIcon.Error); ;
      }

      void StopGeneration()
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
      void MainFormClosing(object sender, FormClosingEventArgs e)
      {
         StopGeneration();
         CloseSession();
      }

      private void generateButton_Click(object sender, System.EventArgs e)
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
            packetHeaderArqn = (int)pktHdrArqnComboBox.SelectedValue;
            packetHeaderSeqn = (int)pktHdrSeqnNumeric.Value;
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
            fhsBdAddressLap = (int)fhsBdaddrLapNumeric.Value;
            fhsBdAddressUap = (int)fhsBdaddrUapNumeric.Value;
            fhsBdAddressNap = (int)fhsBdaddrNapNumeric.Value;
            fhsPayloadLTAddress = (int)fhsLtAddrNumeric.Value;
            fhsPayloadDeviceClass = (int)fhsDevClassNumeric.Value;
            fhsPayloadScanRepetition = (int)fhsScanRepNumeric.Value;
            fhsPayloadPageScanMode = (int)fhsPgScanNumeric.Value;
            fhsPayloadDeviceClock = (int)fhsDevClockNumeric.Value;
            channelNumber = (int)chnNumberNumeric.Value;
            powerLevel = (double)powerLevelNumeric.Value;
            bdAddressLap = (int)bdaddrLapNumeric.Value;
            bdAddressUap = (int)bdaddrUapNumeric.Value;
            bdAddressNap = (int)bdaddrNapNumeric.Value;
            HeaderLtAddress = (int)pktLtAddrNumeric.Value;
            headerFlow = (int)pktHdrFlowNumeric.Value;
            headerSeqn = (int)pktHdrSeqnNumeric.Value;
            autoHeadroomEnabled = (int)autoheadroomEnabComboBox.SelectedValue;
            awgnEnabled = (int)awgnEnabledComboBox.SelectedValue;
            whiteningEnabled = (int)whiteEnComboBox.SelectedValue;
            allIqImpairmentsEnabled = (int)allIqImpairEnComboBox.SelectedValue;
            whiteningclock = (int)whiteClkNumeric.Value;
            actualHeadroom = System.Convert.ToDouble(actualHeadroomTextBox.Text);
            iOffset = (double)iOffsetNumeric.Value;
            qOffset = (double)qOffsetNumeric.Value;
            iCommonModeOffset = (double)iCommonModeOffsetNumeric.Value;
            qCommonModeOffset = (double)qCommonModeOffsetNumeric.Value;
            outputPort = (int)outputPortComboBox.SelectedValue;
            terminalConfiguration = (int)terminalConfigurationComboBox.SelectedValue;
            standard = niBTSGConstants.StandardBasicEDR;
            //BT Generation 
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
   }
}