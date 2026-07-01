using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.RFToolkits.BT.Generation;

namespace NationalInstruments.Examples.BTGenerateDataPacket
{
   public partial class MainForm : Form
   {
      NIRfsg rfsgSession;
      niBTSG btsgSession;

      string resourceName, referenceClockSource, exportClock, waveformName, script;

      double powerLevel, carrierFrequency, upConverterCenterFrequency, upConverterCenterFrequencyOffset, actualHeadroom, externalAttenuation, headroom, quadratureSkew, iDCOffset, qDCOffset, iQGainImbalance, carrierFrequencyOffset, carrierToNoiseRatio, iCommonModeOffset, qCommonModeOffset, iOffset, qOffset;
      int itemIndex = 0;
      int channelNumber, allIqImpairmentsEnabled, awgnEnabled, whiteningEnabled, autoHeadroomEnabled, packetHeaderArqn, packetHeaderSeqn, actualPayloadLength, dataType, bdAddressLap, bdAddressUap, payloadLengthMode, payloadLength, bdAddressNap, packetType, HeaderLtAddress, headerFlow, headerPayloadLlid, seed, headerSeqn, dataPNOrder, whiteningclock, uniqPkts, idleSlots, outputPort, terminalConfiguration;
      string model;
      int[][] userDefinedBits = new int[][]{new int[] {0,0,0,0,0,0,0,0},
                             new int[]{1,1,1,1,1,1,1,1},
                               new int[]{0,0,0,0,1,1,1,1},
                             new int[]{1,1,1,1,0,0,0,0},
                               new int[]{1,0,1,0,1,0,1,0},
                             new int[]{0,1,0,1,0,1,0,1}
                           };
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
         ConfigurePacketComboBox();
         ConfigurePayhdrPaylenModeComboBox();
         ConfigurePayhdrDatatypeComboBox();
         ConfigureUserDefinedBitsComboBox();
         ConfigureOutputPortComboBox();
         ConfigureTerminalConfigurationComboBox();
         ConfigureWhiteEnComboBox();
         ConfigurestopButton();
      }

      #region UI Initial Value Config Section
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

         iDCOffsetNumeric.Minimum = decimal.MinValue;
         iDCOffsetNumeric.Maximum = decimal.MaxValue;
         iDCOffsetNumeric.Increment = 0.5M;
         iDCOffsetNumeric.Value = 0;
         iDCOffsetNumeric.DecimalPlaces = 2;

         qDCOffsetNumeric.Minimum = decimal.MinValue;
         qDCOffsetNumeric.Maximum = decimal.MaxValue;
         qDCOffsetNumeric.Increment = 0.5M;
         qDCOffsetNumeric.Value = 0;
         qDCOffsetNumeric.DecimalPlaces = 2;

         qCommonModeOffsetNumeric.Minimum = decimal.MinValue;
         qCommonModeOffsetNumeric.Maximum = decimal.MaxValue;
         qCommonModeOffsetNumeric.Increment = 1;
         qCommonModeOffsetNumeric.Value = 0;
         qCommonModeOffsetNumeric.DecimalPlaces = 0;

         iCommonModeOffsetNumeric.Minimum = decimal.MinValue;
         iCommonModeOffsetNumeric.Maximum = decimal.MaxValue;
         iCommonModeOffsetNumeric.Increment = 1;
         iCommonModeOffsetNumeric.Value = 0;
         iCommonModeOffsetNumeric.DecimalPlaces = 0;

         qOffsetNumeric.Minimum = decimal.MinValue;
         qOffsetNumeric.Maximum = decimal.MaxValue;
         qOffsetNumeric.Increment = 1;
         qOffsetNumeric.Value = 0;
         qOffsetNumeric.DecimalPlaces = 0;

         iOffsetNumeric.Minimum = decimal.MinValue;
         iOffsetNumeric.Maximum = decimal.MaxValue;
         iOffsetNumeric.Increment = 1;
         iOffsetNumeric.Value = 0;
         iOffsetNumeric.DecimalPlaces = 0;

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
         bdaddrNapNumeric.Maximum = 3;
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

         payHdrLlidNumeric.Minimum = int.MinValue;
         payHdrLlidNumeric.Maximum = int.MaxValue;
         payHdrLlidNumeric.Increment = 1;
         payHdrLlidNumeric.Value = 0;
         payHdrLlidNumeric.DecimalPlaces = 0;

         payHdrFlowNumeric.Minimum = int.MinValue;
         payHdrFlowNumeric.Maximum = int.MaxValue;
         payHdrFlowNumeric.Increment = 1;
         payHdrFlowNumeric.Value = 0;
         payHdrFlowNumeric.DecimalPlaces = 0;

         payHdrPaylenNumeric.Minimum = int.MinValue;
         payHdrPaylenNumeric.Maximum = int.MaxValue;
         payHdrPaylenNumeric.Increment = 1;
         payHdrPaylenNumeric.Value = 1;
         payHdrPaylenNumeric.DecimalPlaces = 0;

         paydatPnorderNumeric.Minimum = int.MinValue;
         paydatPnorderNumeric.Maximum = int.MaxValue;
         paydatPnorderNumeric.Increment = 1;
         paydatPnorderNumeric.Value = 9;
         paydatPnorderNumeric.DecimalPlaces = 0;

         paydatSeedNumeric.Minimum = int.MinValue;
         paydatSeedNumeric.Maximum = int.MaxValue;
         paydatSeedNumeric.Increment = 1;
         paydatSeedNumeric.Value = 497;
         paydatSeedNumeric.DecimalPlaces = 0;

         whiteClkNumeric.Minimum = int.MinValue;
         whiteClkNumeric.Maximum = int.MaxValue;
         whiteClkNumeric.Increment = 1;
         whiteClkNumeric.Value = 0;
         whiteClkNumeric.DecimalPlaces = 0;

         numOfIdleSlotsNumeric.Minimum = int.MinValue;
         numOfIdleSlotsNumeric.Maximum = int.MaxValue;
         numOfIdleSlotsNumeric.Increment = 1;
         numOfIdleSlotsNumeric.Value = 1;
         numOfIdleSlotsNumeric.DecimalPlaces = 0;

         numOfUniqNumeric.Minimum = int.MinValue;
         numOfUniqNumeric.Maximum = int.MaxValue;
         numOfUniqNumeric.Increment = 1;
         numOfUniqNumeric.Value = 1;
         numOfUniqNumeric.DecimalPlaces = 0;
      }

      private void ConfigurestopButton()
      {
         //Deactivate the 'Stop' button
         stopButton.Enabled = false;
      }

      private void ConfigureAutoheadroomEnabComboBox()
      {
         var autoHeadroomEnabValueList = new List<DictionaryEntry>();
         autoHeadroomEnabComboBox.DataSource = GetTrueFalseValueList();
         autoHeadroomEnabComboBox.DisplayMember = "Key";
         autoHeadroomEnabComboBox.ValueMember = "Value";
         autoHeadroomEnabComboBox.SelectedIndex = 1;
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
         refSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.PxiClock;
      }

      private void ConfigureUserDefinedBitsComboBox()
      {
         var userDefinedBitsValueList = new List<DictionaryEntry>();
         userDefinedBitsValueList.Add(new DictionaryEntry("0 0 0 0 0 0 0 0", 0));
         userDefinedBitsValueList.Add(new DictionaryEntry("1 1 1 1 1 1 1 1", 1));
         userDefinedBitsValueList.Add(new DictionaryEntry("0 0 0 0 1 1 1 1", 2));
         userDefinedBitsValueList.Add(new DictionaryEntry("1 1 1 1 0 0 0 0", 3));
         userDefinedBitsValueList.Add(new DictionaryEntry("1 0 1 0 1 0 1 0", 4));
         userDefinedBitsValueList.Add(new DictionaryEntry("0 1 0 1 0 1 0 1", 5));
         userDefinedBitsComboBox.DataSource = userDefinedBitsValueList;
         userDefinedBitsComboBox.DisplayMember = "Key";
         userDefinedBitsComboBox.ValueMember = "Value";
         userDefinedBitsComboBox.SelectedIndex = 4;
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
         clkOutTerminalComboBox.SelectedValue = RfsgOutputTerminal.DoNotExport;
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

      private void ConfigurePacketComboBox()
      {
         var packetValueList = new List<DictionaryEntry>();
         packetValueList.Add(new DictionaryEntry("Null", niBTSGConstants.PacketTypeNull));
         packetValueList.Add(new DictionaryEntry("Poll", niBTSGConstants.PacketTypePoll));
         packetValueList.Add(new DictionaryEntry("Fhs", niBTSGConstants.PacketTypeFhs));
         packetValueList.Add(new DictionaryEntry("DM1", niBTSGConstants.PacketTypeDm1));
         packetValueList.Add(new DictionaryEntry("DH1", niBTSGConstants.PacketTypeDh1));
         packetValueList.Add(new DictionaryEntry("2DH1", niBTSGConstants.PacketType2Dh1));
         packetValueList.Add(new DictionaryEntry("HV1", niBTSGConstants.PacketTypeHv1));
         packetValueList.Add(new DictionaryEntry("HV2", niBTSGConstants.PacketTypeHv2));
         packetValueList.Add(new DictionaryEntry("2EV3", niBTSGConstants.PacketType2Ev3));
         packetValueList.Add(new DictionaryEntry("HV2", niBTSGConstants.PacketTypeHv3));
         packetValueList.Add(new DictionaryEntry("EV3", niBTSGConstants.PacketTypeEv3));
         packetValueList.Add(new DictionaryEntry("3EV3", niBTSGConstants.PacketType3Ev3));
         packetValueList.Add(new DictionaryEntry("DV", niBTSGConstants.PacketTypeDv));
         packetValueList.Add(new DictionaryEntry("3DH1", niBTSGConstants.PacketType3Dh1));
         packetValueList.Add(new DictionaryEntry("Aux1", niBTSGConstants.PacketTypeAux1));
         packetValueList.Add(new DictionaryEntry("Dm3", niBTSGConstants.PacketTypeDm3));
         packetValueList.Add(new DictionaryEntry("2Dh3", niBTSGConstants.PacketType2Dh3));
         packetValueList.Add(new DictionaryEntry("Dh3", niBTSGConstants.PacketTypeDh3));
         packetValueList.Add(new DictionaryEntry("3Dh3", niBTSGConstants.PacketType3Dh3));
         packetValueList.Add(new DictionaryEntry("EV4", niBTSGConstants.PacketTypeEv4));
         packetValueList.Add(new DictionaryEntry("2EV5", niBTSGConstants.PacketType2Ev5));
         packetValueList.Add(new DictionaryEntry("EV5", niBTSGConstants.PacketTypeEv5));
         packetValueList.Add(new DictionaryEntry("3EV5", niBTSGConstants.PacketType3Ev5));
         packetValueList.Add(new DictionaryEntry("Dm5", niBTSGConstants.PacketTypeDm5));
         packetValueList.Add(new DictionaryEntry("2Dh5", niBTSGConstants.PacketType2Dh5));
         packetValueList.Add(new DictionaryEntry("Dh5", niBTSGConstants.PacketTypeDh5));
         packetValueList.Add(new DictionaryEntry("Dm3", niBTSGConstants.PacketTypeDm3));
         packetValueList.Add(new DictionaryEntry("Id", niBTSGConstants.PacketTypeId));
         packetComboBox.DataSource = packetValueList;
         packetComboBox.DisplayMember = "Key";
         packetComboBox.ValueMember = "Value";
         packetComboBox.SelectedIndex = niBTSGConstants.PacketTypeDh1;
      }

      private void ConfigurePayhdrPaylenModeComboBox()
      {
         var payHdrPaylenModeValueList = new List<DictionaryEntry>();
         payHdrPaylenModeValueList.Add(new DictionaryEntry("Maximum Length", niBTSGConstants.PayloadLengthModeMaximumLength));
         payHdrPaylenModeValueList.Add(new DictionaryEntry("User Defined", niBTSGConstants.PayloadLengthModeUserDefined));
         payHdrPaylenModeComboBox.DataSource = payHdrPaylenModeValueList;
         payHdrPaylenModeComboBox.DisplayMember = "Key";
         payHdrPaylenModeComboBox.ValueMember = "Value";
         payHdrPaylenModeComboBox.SelectedIndex = niBTSGConstants.PayloadLengthModeMaximumLength;
      }

      private void ConfigurePayhdrDatatypeComboBox()
      {
         var payHdrDatatypeValueList = new List<DictionaryEntry>();
         payHdrDatatypeValueList.Add(new DictionaryEntry("PN Sequence", niBTSGConstants.PayloadDataTypePnSequence));
         payHdrDatatypeValueList.Add(new DictionaryEntry("User Defined", niBTSGConstants.PayloadDataTypeUserDefinedBits));
         payHdrDatatypeComboBox.DataSource = payHdrDatatypeValueList;
         payHdrDatatypeComboBox.DisplayMember = "Key";
         payHdrDatatypeComboBox.ValueMember = "Value";
         payHdrDatatypeComboBox.SelectedIndex = niBTSGConstants.PayloadDataTypePnSequence;
      }

      private void ConfigureWhiteEnComboBox()
      {
         var whiteEnValueList = new List<DictionaryEntry>();
         whiteEnComboBox.DataSource = GetTrueFalseValueList();
         whiteEnComboBox.DisplayMember = "Key";
         whiteEnComboBox.ValueMember = "Value";
         whiteEnComboBox.SelectedIndex = 0;
      }

      private List<DictionaryEntry> GetTrueFalseValueList()
      {
         var trueFalseValueList = new List<DictionaryEntry>();
         trueFalseValueList.Add(new DictionaryEntry("False", niBTSGConstants.False));
         trueFalseValueList.Add(new DictionaryEntry("True", niBTSGConstants.True));
         return trueFalseValueList;
      }
      #endregion

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
            itemIndex = (int)userDefinedBitsComboBox.SelectedValue;
            exportClock = clkOutTerminalComboBox.SelectedValue.ToString();
            packetType = (int)packetComboBox.SelectedValue;
            packetHeaderArqn = (int)pktHdrArqnComboBox.SelectedValue;
            packetHeaderSeqn = (int)pktHdrSeqnNumeric.Value;
            payloadLengthMode = (int)payHdrPaylenModeComboBox.SelectedValue;
            payloadLength = (int)payHdrPaylenNumeric.Value;
            dataType = (int)payHdrDatatypeComboBox.SelectedValue;
            waveformName = (string)waveNameTextBox.Text;
            script = (string)scriptTextBox.Text;

            carrierFrequency = double.Parse(carrierFreqTextBox.Text);
            externalAttenuation = (double)externalAttnNumeric.Value;
            headroom = (double)headroomNumeric.Value;
            quadratureSkew = (double)quadratureSkewNumeric.Value;
            iDCOffset = (double)iDCOffsetNumeric.Value;
            qDCOffset = (double)qDCOffsetNumeric.Value;
            iQGainImbalance = (double)iqGaimbalanceNumeric.Value;
            carrierFrequencyOffset = (double)carrierFreqOffNumeric.Value;
            carrierToNoiseRatio = (double)cnrNumeric.Value;
            seed = (int)paydatSeedNumeric.Value;
            channelNumber = (int)chnNumberNumeric.Value;
            powerLevel = (double)powerLevelNumeric.Value;
            bdAddressLap = (int)bdaddrLapNumeric.Value;
            bdAddressUap = (int)bdaddrUapNumeric.Value;
            bdAddressNap = (int)bdaddrNapNumeric.Value;
            HeaderLtAddress = (int)pktLtAddrNumeric.Value;
            headerPayloadLlid = (int)payHdrLlidNumeric.Value;
            headerFlow = (int)pktHdrFlowNumeric.Value;
            headerSeqn = (int)pktHdrSeqnNumeric.Value;
            dataPNOrder = (int)paydatPnorderNumeric.Value;
            autoHeadroomEnabled = (int)autoHeadroomEnabComboBox.SelectedValue;
            awgnEnabled = (int)awgnEnabledComboBox.SelectedValue;
            whiteningEnabled = (int)whiteEnComboBox.SelectedValue;
            allIqImpairmentsEnabled = (int)allIqImpairEnComboBox.SelectedValue;
            whiteningclock = (int)whiteClkNumeric.Value;
            actualHeadroom = System.Convert.ToDouble(actualHeadroomTextBox.Text);
            uniqPkts = (int)numOfUniqNumeric.Value;
            idleSlots = (int)numOfIdleSlotsNumeric.Value;

            iOffset = (double)iOffsetNumeric.Value;
            qOffset = (double)qOffsetNumeric.Value;
            iCommonModeOffset = (double)iCommonModeOffsetNumeric.Value;
            qCommonModeOffset = (double)qCommonModeOffsetNumeric.Value;
            outputPort = (int)outputPortComboBox.SelectedValue;
            terminalConfiguration = (int)terminalConfigurationComboBox.SelectedValue;
            // Start the BT Generation 
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

      // --------------------------------------------------------------------------
      // Open and configure BT and RFSA sessions	
      // --------------------------------------------------------------------------
      void StartGeneration()
      {
         /*BTSG Session*/
         if (btsgSession == null)
         {
            btsgSession = new niBTSG(niBTSGConstants.ToolkitCompatibilityVersion020000);
         }
         niBTSG.ChannelNumberToCarrierFrequency(channelNumber, niBTSGConstants.StandardBasicEDR, out carrierFrequency);
         carrierFreqTextBox.Text = carrierFrequency.ToString();

         //Set BD Address
         btsgSession.SetBDAddress(null, bdAddressLap, bdAddressUap, bdAddressNap);

         //Set Packet Type
         btsgSession.SetPacketType(null, packetType);

         /* Set Number of Unique Packets and Idle Slots */
         btsgSession.SetNumberOfUniquePackets(null, uniqPkts);
         btsgSession.SetNumberOfIdleSlots(null, idleSlots);

         //Set Payload Header
         btsgSession.SetPayloadHeaderLlid(null, headerPayloadLlid);
         btsgSession.SetPayloadHeaderFlow(null, headerFlow);
         btsgSession.SetPayloadLengthMode(null, payloadLengthMode);

         if (payloadLengthMode == niBTSGConstants.PayloadLengthModeUserDefined)
         {
            btsgSession.SetPayloadLength(null, payloadLength);

         }
         btsgSession.GetActualPayloadLength(null, out actualPayloadLength);

         payHdrActPaylenTextBox.Text = actualPayloadLength.ToString();

         //Set Payload Data
         btsgSession.SetPayloadDataType(null, dataType);

         if (dataType == niBTSGConstants.PayloadDataTypePnSequence)
         {
            btsgSession.SetPayloadPNOrder(null, dataPNOrder);
            btsgSession.SetPayloadPNSeed(null, seed);
         }
         else
         {
            btsgSession.SetPayloadUserDefinedBits(null, (int[])userDefinedBits[itemIndex], 8);
         }

         //Set Packet Header
         btsgSession.SetPacketHeaderLTAddress(null, HeaderLtAddress);
         btsgSession.SetPacketHeaderFlow(null, headerFlow);
         btsgSession.SetPacketHeaderArqn(null, packetHeaderArqn);
         btsgSession.SetPacketHeaderSeqn(null, packetHeaderSeqn);

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
            btsgSession.SetIDCOffset(null, iDCOffset);
            btsgSession.SetQDCOffset(null, qDCOffset);
            btsgSession.SetIQGainImbalance(null, iQGainImbalance);
         }

         btsgSession.SetAwgnEnabled(null, awgnEnabled);
         btsgSession.SetCarrierToNoiseRatio(null, carrierToNoiseRatio);

         //Whitening Properties
         btsgSession.SetWhiteningEnabled(null, whiteningEnabled);
         btsgSession.SetWhiteningClock(null, whiteningclock);

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
         btsgSession.GetActualHeadroom(null, out actualHeadroom);
         actualHeadroomTextBox.Text = actualHeadroom.ToString();

         /*Execute Script*/
         niBTSG.RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, script, powerLevel);
         rfsgSession.RF.OutputEnabled = true;
         rfsgSession.Initiate();

         /*Start the status checking timer */

         timer.Enabled = true;
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
            //btsgSession.RFSGClearDatabase(rfsgSession.Handle, null, "");
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
      // --------------------------------------------------------------------------
      // Callback function when the MainForm is closed
      // --------------------------------------------------------------------------
      void MainFormClosing(object sender, FormClosingEventArgs e)
      {
         StopGeneration();
         CloseSession();
      }
      // --------------------------------------------------------------------------
      // CallBack function on MainForm timer tick
      // --------------------------------------------------------------------------
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
   }
}