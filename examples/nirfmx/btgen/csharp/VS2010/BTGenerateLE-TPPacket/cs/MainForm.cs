using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.RFToolkits.BT.Generation;
using System.Text.RegularExpressions;


namespace NationalInstruments.Examples.BTGenerateLETPPacket
{
   public partial class MainForm : Form
   {
      NIRfsg rfsgSession;
      niBTSG btsgSession;

      string script;
      string resourceName, clkOutTerm, userDefinedBits;
      string referenceClockSource, exportClock, waveformName;
      string model, antennaSwitchingPattern;
      double iDcOffset, qDcOffset, iQGainImbalance, carrierFrequencyOffset, carrierToNoiseRatio, powerLevel, quadratureSkew, headroom, externalAttenuation, carrierFrequency, actualHeadroom, upConverterCenterFrequency, upConverterCenterFrequencyOffset;
      double cteLength, cteSlotDuration, antennaSwitchingDuration, antennaSwitchingDurationUsed;
      int[] userDefBitsArray;
      double[] relativePhase, relativeAmplitude;
      int channelNumber, allIqImpairmentsEnabled, awgnEnabled, autoHeadroomEnabled, payloadLengthMode, payloadLength, payloadType, dirtyTxEnabld, outputPort, terminalConfiguration, packetType, letpCorruptAlternateCrc;
      int numberOfUniquePackets, directionFindingMode, antennaSwitchingEnabled, numberOfAntennas, oversamplingFactor;


      public MainForm()
      {
         InitializeComponent();
         ConfigureNumericUpDown();
         ConfigureAutoheadroomEnabComboBox();
         ConfigureRefSourceComboBox();
         ConfigureClkOutTerminalComboBox();
         ConfigureAllIqImpairEnComboBox();
         ConfigureAwgnEnabledComboBox();
         ConfigureLETPPayloadType();
         ConfigurePayhdrPaylenModeComboBox();
         ConfigureDirtyTxComboBox();
         ConfigureOutputPortComboBox();
         ConfigureTerminalConfigurationComboBox();
         ConfigurePacketTypeComboBox();
         ConfigureLETPCorruptAlternateCRCComboBox();
         ConfigureDirectionFindingModeComboBox();
         ConfigureCTESlotDurationComboBox();
         ConfigureAntennaSwitchingEnabledComboBox();
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

      private void ConfigureLETPPayloadType()
      {
         var letpPayloadTypeValueList = new List<DictionaryEntry>();
         letpPayloadTypeValueList.Add(new DictionaryEntry("PRBS9", niBTSGConstants.LETPPayloadTypePRBS9));
         letpPayloadTypeValueList.Add(new DictionaryEntry("11110000", niBTSGConstants.LETPPayloadType11110000));
         letpPayloadTypeValueList.Add(new DictionaryEntry("10101010", niBTSGConstants.LETPPayloadType10101010));
         letpPayloadTypeValueList.Add(new DictionaryEntry("PRBS15", niBTSGConstants.LETPPayloadTypePRBS15));
         letpPayloadTypeValueList.Add(new DictionaryEntry("11111111", niBTSGConstants.LETPPayloadType11111111));
         letpPayloadTypeValueList.Add(new DictionaryEntry("00000000", niBTSGConstants.LETPPayloadType00000000));
         letpPayloadTypeValueList.Add(new DictionaryEntry("00001111", niBTSGConstants.LETPPayloadType00001111));
         letpPayloadTypeValueList.Add(new DictionaryEntry("01010101", niBTSGConstants.LETPPayloadType01010101));
         letpPayloadTypeValueList.Add(new DictionaryEntry("User Defined Bits", niBTSGConstants.LETPPayloadTypeUserDefinedBits));
         letpPayloadTypeComboBox.DataSource = letpPayloadTypeValueList;
         letpPayloadTypeComboBox.DisplayMember = "Key";
         letpPayloadTypeComboBox.ValueMember = "Value";
         letpPayloadTypeComboBox.SelectedIndex = 0;
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

      private void ConfigurePacketTypeComboBox()
      {
         var ptList = new List<DictionaryEntry>();
         ptList.Add(new DictionaryEntry("LE-TP", niBTSGConstants.PacketTypeLETP));
         ptList.Add(new DictionaryEntry("LE-TP-EXT", niBTSGConstants.PacketTypeLETPExt));
         ptList.Add(new DictionaryEntry("LE-Enhanced", niBTSGConstants.PacketTypeLEEnhanced));
         ptList.Add(new DictionaryEntry("LE-LR-125k", niBTSGConstants.PacketTypeLeLr125k));
         ptList.Add(new DictionaryEntry("LE-LR-500k", niBTSGConstants.PacketTypeLeLr500k));
         packetTypeComboBox.DataSource = ptList;
         packetTypeComboBox.DisplayMember = "Key";
         packetTypeComboBox.ValueMember = "Value";
         packetTypeComboBox.SelectedIndex = 0;
      }

      private void ConfigureLETPCorruptAlternateCRCComboBox()
      {
         //var LETPCorruptAltCRC = new List<DictionaryEntry>();
         letpCorruptAlternateCRCComboBox.DataSource = GetTrueFalseValueList();
         letpCorruptAlternateCRCComboBox.DisplayMember = "Key";
         letpCorruptAlternateCRCComboBox.ValueMember = "Value";
         letpCorruptAlternateCRCComboBox.SelectedIndex = 0;
      }

      private void ConfigureDirectionFindingModeComboBox()
      {
         var dfmList = new List<DictionaryEntry>();
         dfmList.Add(new DictionaryEntry("Disabled", niBTSGConstants.DirectionFindingModeDisabled));
         dfmList.Add(new DictionaryEntry("Angle of Arrival", niBTSGConstants.DirectionFindingModeAngleOfArrival));
         dfmList.Add(new DictionaryEntry("Angle of Departure", niBTSGConstants.DirectionFindingModeAngleOfDeparture));
         directionFindingModeComboBox.DataSource = dfmList;
         directionFindingModeComboBox.DisplayMember = "Key";
         directionFindingModeComboBox.ValueMember = "Value";
         directionFindingModeComboBox.SelectedIndex = 0;
      }

      private void ConfigureCTESlotDurationComboBox()
      {
         var cteSltDur = new List<DictionaryEntry>();
         cteSltDur.Add(new DictionaryEntry("1 us", niBTSGConstants.CteSlotDuration1us));
         cteSltDur.Add(new DictionaryEntry("2 us", niBTSGConstants.CteSlotDuration2us));
         cteSlotDurationComboBox.DataSource = cteSltDur;
         cteSlotDurationComboBox.DisplayMember = "Key";
         cteSlotDurationComboBox.ValueMember = "Value";
         cteSlotDurationComboBox.SelectedIndex = 0;
      }

      private void ConfigureAntennaSwitchingEnabledComboBox()
      {
         //var LETPCorruptAltCRC = new List<DictionaryEntry>();
         antennaSwitchingEnabledComboBox.DataSource = GetTrueFalseValueList();
         antennaSwitchingEnabledComboBox.DisplayMember = "Key";
         antennaSwitchingEnabledComboBox.ValueMember = "Value";
         antennaSwitchingEnabledComboBox.SelectedIndex = 0;
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

            packetType = (int)packetTypeComboBox.SelectedValue;
            payloadLengthMode = (int)payHdrPaylenModeComboBox.SelectedValue;
            payloadLength = (int)payHdrPaylenNumeric.Value;
            letpCorruptAlternateCrc = (int)letpCorruptAlternateCRCComboBox.SelectedValue;
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
            payloadType = (int)letpPayloadTypeComboBox.SelectedValue;
            dirtyTxEnabld = (int)dirtyTxComboBox.SelectedValue;
            clkOutTerm = clkOutTerminalComboBox.SelectedValue.ToString();
            autoHeadroomEnabled = (int)autoheadroomEnabComboBox.SelectedValue;
            outputPort = (int)outputPortComboBox.SelectedValue;
            terminalConfiguration = (int)terminalConfigurationComboBox.SelectedValue;
            userDefinedBits = userDefinedBitsTextBox.Text.ToString();
            numberOfUniquePackets = (int)NumOfUniquePacketsNumeric.Value;
            directionFindingMode = (int)directionFindingModeComboBox.SelectedValue;
            cteLength = (double)cteLengthNumeric.Value;
            cteSlotDuration = (double)cteSlotDurationComboBox.SelectedValue;
            antennaSwitchingEnabled = (int)antennaSwitchingEnabledComboBox.SelectedValue;
            numberOfAntennas = (int)numberOfAntennasNumeric.Value;
            antennaSwitchingPattern = (string)antennaSwitchingPatternTextBox.Text;
            antennaSwitchingDuration = (double)AntennaSwitchingDurationNumeric.Value;
            oversamplingFactor = (int)OversamplingFactorNumeric.Value;
            antennaSwitchingDurationUsed = System.Convert.ToDouble(AntennaSwitchingDurationUsedTextBox.Text);

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

         btsgSession.SetPacketType(null, packetType);

         /* Set LE-TP Payload */
         btsgSession.SetLETPPayloadType(null, payloadType);

         /* Set Payload Header */
         btsgSession.SetPayloadLengthMode(null, payloadLengthMode);

         if (payloadLengthMode == niBTSGConstants.PayloadLengthModeUserDefined)
         {
            btsgSession.SetPayloadLength(null, payloadLength);
         }
         btsgSession.SetLETPCorruptAlternateCrc(null, letpCorruptAlternateCrc);
         btsgSession.SetDirtyTxEnabled(null, dirtyTxEnabld);

         if (payloadType == niBTSGConstants.LETPPayloadTypeUserDefinedBits)
         {
            userDefBitsArray = new int[userDefinedBits.Length];
            for (int i = 0; i < userDefinedBits.Length; i++)
            {
               userDefBitsArray[i] = (int)Char.GetNumericValue(userDefinedBits[i]);
            }
            btsgSession.SetPayloadUserDefinedBits(null, userDefBitsArray, userDefinedBits.Length);
         }

         btsgSession.SetNumberOfUniquePackets(null, numberOfUniquePackets);
         btsgSession.SetOversamplingFactor(null, oversamplingFactor);
         /* Direction Finding */
         btsgSession.SetDirectionFindingMode(null, directionFindingMode);
         if (directionFindingMode != niBTSGConstants.DirectionFindingModeDisabled)
         {
            btsgSession.SetDirectionFindingConstantToneExtensionLength(null, cteLength);
            btsgSession.SetDirectionFindingConstantToneExtensionSlotDuration(null, cteSlotDuration);
            btsgSession.SetDirectionFindingAntennaSwitchingEnabled(null, antennaSwitchingEnabled);
            if (antennaSwitchingEnabled == niBTSGConstants.True)
            {
               btsgSession.SetDirectionFindingNumberOfAntennas(null, numberOfAntennas);
               btsgSession.SetDirectionFindingAntennaSwitchingPattern(null, antennaSwitchingPattern);
               btsgSession.SetDirectionFindingAntennaSwitchingDuration(null, antennaSwitchingDuration);
               int RelPhaseAmpArraySize = dataGridView1.Rows.Count;
               relativePhase = new double[RelPhaseAmpArraySize];
               relativeAmplitude = new double[RelPhaseAmpArraySize];
               for (int rows = 0; rows < RelPhaseAmpArraySize; rows++)
               {
                  {
                     relativePhase[rows] = Convert.ToDouble(dataGridView1.Rows[rows].Cells[1].Value.ToString());
                     relativeAmplitude[rows] = Convert.ToDouble(dataGridView1.Rows[rows].Cells[2].Value.ToString());
                  }
               }
               btsgSession.SetAntennaRelativePhaseAndAmplitude(null, relativeAmplitude, relativePhase, RelPhaseAmpArraySize);
            }
         }

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
         btsgSession.GetDirectionFindingAntennaSwitchingDurationUsed(null, out antennaSwitchingDurationUsed);
         AntennaSwitchingDurationUsedTextBox.Text = antennaSwitchingDurationUsed.ToString();

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

      private void insertRowButton_Click(object sender, EventArgs e)
      {
         int IndexNum = dataGridView1.Rows.Add();
         dataGridView1.Rows[IndexNum].Cells["RelativePhase"].Value = "0.00";
         dataGridView1.Rows[IndexNum].Cells["RelativeAmplitude"].Value = "0.00";
         dataGridView1.Rows[IndexNum].Cells["Index"].Value = IndexNum.ToString();
      }

      private void deleteRowButton_Click(object sender, EventArgs e)
      {
         if (dataGridView1.Rows.Count > 0)
         {
            int IndexNum = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows.RemoveAt(IndexNum);
            for (int i = IndexNum; i < dataGridView1.Rows.Count; i++)
            {
               dataGridView1.Rows[i].Cells["Index"].Value = (i).ToString();
            }
         }
      }

      private void userDefinedBitsTextBox_KeyPress(object sender, KeyPressEventArgs e)
      {
         var regex = new Regex(@"[^0-1\b]");
         if (regex.IsMatch(e.KeyChar.ToString()))
         {
            e.Handled = true;
         }
      }
   }
}
