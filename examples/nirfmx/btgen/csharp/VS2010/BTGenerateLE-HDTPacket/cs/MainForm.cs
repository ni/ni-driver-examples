using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.RFToolkits.BT.Generation;
using System.Text.RegularExpressions;


namespace NationalInstruments.Examples.BTGenerateLEHDTPacket
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
      int channelNumber, allIqImpairmentsEnabled, awgnEnabled, autoHeadroomEnabled, payloadLengthMode, payloadLength,
          dirtyTxEnabld, outputPort, terminalConfiguration, zadoffChuIndex, physicalChannelAddress, hdtPacketFormat;


        int numberOfUniquePackets, directionFindingMode, oversamplingFactor, dataRate;


      public MainForm()
      {
         InitializeComponent();
         ConfigureNumericUpDown();
         ConfigureAutoheadroomEnabComboBox();
         ConfigureRefSourceComboBox();
         ConfigureClkOutTerminalComboBox();
         ConfigureAllIqImpairEnComboBox();
         ConfigureAwgnEnabledComboBox();
         ConfigurePayhdrPaylenModeComboBox();
         ConfigureDirtyTxComboBox();
         ConfigureOutputPortComboBox();
         ConfigureTerminalConfigurationComboBox();
         ConfigurehdtPacketFormatComboBox();
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

         payHdrPaylenNumeric.Minimum = int.MinValue;
         payHdrPaylenNumeric.Maximum = int.MaxValue;
         payHdrPaylenNumeric.Increment = 1;
         payHdrPaylenNumeric.Value = 0;
         payHdrPaylenNumeric.DecimalPlaces = 0;
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


      private void ConfigurePayhdrPaylenModeComboBox()
      {
         var payHdrPaylenModeValueList = new List<DictionaryEntry>();
         payHdrPaylenModeValueList.Add(new DictionaryEntry("Maximum Length", niBTSGConstants.PayloadLengthModeMaximumLength));
         payHdrPaylenModeValueList.Add(new DictionaryEntry("User Defined", niBTSGConstants.PayloadLengthModeUserDefined));
         payHdrPaylenModeComboBox.DataSource = payHdrPaylenModeValueList;
         payHdrPaylenModeComboBox.DisplayMember = "Key";
         payHdrPaylenModeComboBox.ValueMember = "Value";
         payHdrPaylenModeComboBox.SelectedIndex = 0;
      }

      private void ConfigureDirtyTxComboBox()
      {
         //var dirtyTxValueList = new List<DictionaryEntry>();
         dirtyTxComboBox.DataSource = GetTrueFalseValueList();
         dirtyTxComboBox.DisplayMember = "Key";
         dirtyTxComboBox.ValueMember = "Value";
         dirtyTxComboBox.SelectedIndex = 0;
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

      private void ConfigurehdtPacketFormatComboBox()
      {
         var hdtPacketFormatValueList = new List<DictionaryEntry>();
         hdtPacketFormatValueList.Add(new DictionaryEntry("Short Format", niBTSGConstants.HdtPacketFormat_ShortFormat));
         hdtPacketFormatValueList.Add(new DictionaryEntry("Format0", niBTSGConstants.HdtPacketFormat_Format0));
         hdtPacketFormatValueList.Add(new DictionaryEntry("Format1", niBTSGConstants.HdtPacketFormat_Format1));
         HdtPacketFormatComboBox.DataSource = hdtPacketFormatValueList;
         HdtPacketFormatComboBox.DisplayMember = "Key";
         HdtPacketFormatComboBox.ValueMember = "Value";
         HdtPacketFormatComboBox.SelectedIndex = niBTSGConstants.HdtPacketFormat_Format0;
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

            payloadLengthMode = (int)payHdrPaylenModeComboBox.SelectedValue;
            payloadLength = (int)payHdrPaylenNumeric.Value;
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
            dirtyTxEnabld = (int)dirtyTxComboBox.SelectedValue;
            clkOutTerm = clkOutTerminalComboBox.SelectedValue.ToString();
            autoHeadroomEnabled = (int)autoheadroomEnabComboBox.SelectedValue;
            outputPort = (int)outputPortComboBox.SelectedValue;
            terminalConfiguration = (int)terminalConfigurationComboBox.SelectedValue;
            numberOfUniquePackets = (int)NumOfUniquePacketsNumeric.Value;
            directionFindingMode = (int)directionFindingModeComboBox.SelectedValue;
            dataRate = (int)dataRateNumeric.Value;
            zadoffChuIndex = (int)zadoffChuIndexNumeric.Value;
            physicalChannelAddress = (int)physicalChannelAddressNumeric.Value;
            hdtPacketFormat = (int)HdtPacketFormatComboBox.SelectedValue;
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

         btsgSession.SetPacketType(null, niBTSGConstants.PacketTypeLEHdt);

         //Set Data Rate
         btsgSession.SetDataRate(null, dataRate);


         /* Set Payload Header */
         btsgSession.SetPayloadLengthMode(null, payloadLengthMode);

         if (payloadLengthMode == niBTSGConstants.PayloadLengthModeUserDefined)
         {
            btsgSession.SetPayloadLength(null, payloadLength);
         }
         btsgSession.SetDirtyTxEnabled(null, dirtyTxEnabld);

         btsgSession.SetNumberOfUniquePackets(null, numberOfUniquePackets);
         btsgSession.SetOversamplingFactor(null, oversamplingFactor);

         /* High Data Throughput*/

         //Set Zadoff-Chu Index
         btsgSession.SetZadoffChuIndex(null, zadoffChuIndex);

         //Set Physical Channel Address
         btsgSession.SetPhysicalChannelAddress(null, physicalChannelAddress);

         //Set HDT Packet Format
         btsgSession.SetHdtPacketFormat(null, hdtPacketFormat);

         //Set HDT PHY Interval
         btsgSession.SetHdtPhyInterval(null, hdtPhyInterval);


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
         btsgSession.GetActualHeadroom(null, out actualHeadroom);
         actualHeadroomTextBox.Text = actualHeadroom.ToString();

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
   }
}
