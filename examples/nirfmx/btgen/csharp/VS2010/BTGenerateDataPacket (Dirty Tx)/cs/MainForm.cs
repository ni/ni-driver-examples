using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.RFToolkits.BT.Generation;
using System;

namespace NationalInstruments.Examples.BTGenerateDataPacket
{
   public partial class MainForm : Form
   {
      NIRfsg rfsgSession;
      niBTSG btsgSession;
      string resourceName, referenceClockSource, exportClock;

      string script = "script GenerateDataPkt\n repeat forever\n generate pkt\n end repeat\n end script";
      int[][] userDefinedBits = new int[][]{new int[] {0,0,0,0,0,0,0,0},
                             new int[]{1,1,1,1,1,1,1,1},
                               new int[]{0,0,0,0,1,1,1,1},
                             new int[]{1,1,1,1,0,0,0,0},
                               new int[]{1,0,1,0,1,0,1,0},
                             new int[]{0,1,0,1,0,1,0,1}
                           };

      double powerLevel, carrierFrequency, upConverterCenterFrequency, upConverterCenterFrequencyOffset, actualHeadroom,
             externalAttenuation, headroom, quadratureSkew, iDCOffset, qDCOffset, iQGainImbalance, carrierFrequencyOffset,
             carrierToNoiseRatio, iCommonModeOffset, qCommonModeOffset, iOffset, qOffset, hdtPhyInterval;

      int channelNumber, itemIndex, allIqImpairmentsEnabled, awgnEnabled, whiteningEnabled, autoHeadroomEnabled,
          packetHeaderArqn, packetHeaderSeqn, actualPayloadLength, dataType, bdAddressLap, bdAddressUap, payloadLengthMode,
          payloadLength, bdAddressNap, packetType, HeaderLtAddress, headerFlow, headerPayloadLlid, seed, headerSeqn, dataPNOrder,
          whiteningclock, uniqPkts, idleSlots, outputPort, terminalConfiguration, dataRate, LETPPayloadType, LETPCorruptAlternateCRC,
          zadoffChuIndex, physicalChannelAddress, hdtPacketFormat, dirtyTxSettingsMode, dirtyTxModulationIndexType;

        int[] parametersEnabledSetArr, carrierFrequencyOffsetSetArr, symbolTimingErrorSetArr;
      double[] modulationIndexSetArr;

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
         ConfigurePacketComboBox();
         ConfigurePayhdrPaylenModeComboBox();
         ConfigurePayhdrDatatypeComboBox();
         ConfigureWhiteEnComboBox();
         ConfigurestopButton();
         ConfigureUserDefinedBitsComboBox();
         ConfigureOutputPortComboBox();
         ConfigureTerminalConfigurationComboBox();
         ConfigureLETPPayloadTypeComboBox();
         ConfigureLETPCorruptAlternateCRCComboBox();
         ConfigurehdtPacketFormatComboBox();
         ConfigureDirtyTxSettingsModeComboBox();
         ConfigureParametersEnabledSetComboBox();
         ConfigureDirtyTxModulationIndexTypeComboBox();
        }

      #region UI Initial Value Config Section

      private void ConfigurestopButton()
      {
         //Deactivate the 'Stop' button
         stopButton.Enabled = false;
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

         HdtPhyIntervalNumeric.Minimum = decimal.MinValue;
         HdtPhyIntervalNumeric.Maximum = decimal.MaxValue;
         HdtPhyIntervalNumeric.Increment = 1;
         HdtPhyIntervalNumeric.Value = 0;
         HdtPhyIntervalNumeric.DecimalPlaces = 2;
      }

      private void ConfigureAutoheadroomEnabComboBox()
      {
         var autoheadroomEnabValueList = new List<DictionaryEntry>();
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

      private void ConfigureLETPPayloadTypeComboBox()
      {
         var LETPPayloadTypeValueList = new List<DictionaryEntry>();
         LETPPayloadTypeValueList.Add(new DictionaryEntry("PRBS9", niBTSGConstants.LETPPayloadTypePRBS9));
         LETPPayloadTypeValueList.Add(new DictionaryEntry("11110000", niBTSGConstants.LETPPayloadType11110000));
         LETPPayloadTypeValueList.Add(new DictionaryEntry("10101010", niBTSGConstants.LETPPayloadType10101010));
         LETPPayloadTypeValueList.Add(new DictionaryEntry("PRBS15", niBTSGConstants.LETPPayloadTypePRBS15));
         LETPPayloadTypeValueList.Add(new DictionaryEntry("1111111", niBTSGConstants.LETPPayloadType11111111));
         LETPPayloadTypeValueList.Add(new DictionaryEntry("00000000", niBTSGConstants.LETPPayloadType00000000));
         LETPPayloadTypeValueList.Add(new DictionaryEntry("00001111", niBTSGConstants.LETPPayloadType00001111));
         LETPPayloadTypeValueList.Add(new DictionaryEntry("01010101", niBTSGConstants.LETPPayloadType01010101));
         LETPPayloadTypeValueList.Add(new DictionaryEntry("User Defined Bits", niBTSGConstants.LETPPayloadTypeUserDefinedBits));
         LETPPayloadTypeComboBox.DataSource = LETPPayloadTypeValueList ;
         LETPPayloadTypeComboBox.DisplayMember = "Key";
         LETPPayloadTypeComboBox.ValueMember = "Value";
         LETPPayloadTypeComboBox.SelectedIndex = niBTSGConstants.LETPPayloadTypePRBS9;
      }

      private void ConfigureLETPCorruptAlternateCRCComboBox()
      {
         var LETPCorruptAlternateCRCValueList = new List<DictionaryEntry>();
         LETPCorruptAlternateCRCValueList.Add(new DictionaryEntry("False", niBTSGConstants.False));
         LETPCorruptAlternateCRCValueList.Add(new DictionaryEntry("True", niBTSGConstants.True));
         LETPCorruptAlternateCRCComboBox.DataSource = LETPCorruptAlternateCRCValueList;
         LETPCorruptAlternateCRCComboBox.DisplayMember = "Key";
         LETPCorruptAlternateCRCComboBox.ValueMember = "Value";
         LETPCorruptAlternateCRCComboBox.SelectedIndex = niBTSGConstants.False;
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

      private void ConfigureDirtyTxSettingsModeComboBox()
      {
         var dirtyTxSettingsValueList = new List<DictionaryEntry>();
         dirtyTxSettingsValueList.Add(new DictionaryEntry("Standard", niBTSGConstants.DirtyTxModeStandard));
         dirtyTxSettingsValueList.Add(new DictionaryEntry("User Defined", niBTSGConstants.DirtyTxModeUserDefined));
         HdtPacketFormatComboBox.DataSource = dirtyTxSettingsValueList;
         HdtPacketFormatComboBox.DisplayMember = "Key";
         HdtPacketFormatComboBox.ValueMember = "Value";
         HdtPacketFormatComboBox.SelectedIndex = niBTSGConstants.DirtyTxModeStandard;
      }

      private void ConfigureParametersEnabledSetComboBox()
      {
         var parametersEnabledSetValueList = new List<DictionaryEntry>();
         parametersEnabledSetValueList.Add(new DictionaryEntry("False", niBTSGConstants.False));
         parametersEnabledSetValueList.Add(new DictionaryEntry("True", niBTSGConstants.True));
         HdtPacketFormatComboBox.DataSource = parametersEnabledSetValueList;
         HdtPacketFormatComboBox.DisplayMember = "Key";
         HdtPacketFormatComboBox.ValueMember = "Value";
         HdtPacketFormatComboBox.SelectedIndex = niBTSGConstants.False;
      }

      private void ConfigureDirtyTxModulationIndexTypeComboBox()
      {
         var dirtyTxModulationIndexTypeValueList = new List<DictionaryEntry>();
         dirtyTxModulationIndexTypeValueList.Add(new DictionaryEntry("Standard", niBTSGConstants.DirtyTxModulationIndexTypeStandard));
         dirtyTxModulationIndexTypeValueList.Add(new DictionaryEntry("Stable", niBTSGConstants.DirtyTxModulationIndexTypeStable));
         dirtyTxModulationIndexTypeComboBox.DataSource = dirtyTxModulationIndexTypeValueList;
         dirtyTxModulationIndexTypeComboBox.DisplayMember = "Key";
         dirtyTxModulationIndexTypeComboBox.ValueMember = "Value";
         dirtyTxModulationIndexTypeComboBox.SelectedIndex = niBTSGConstants.DirtyTxModulationIndexTypeStandard;
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

      private void ConfigureWhiteEnComboBox()
      {
         var whiteEnValueList = new List<DictionaryEntry>();
         whiteEnComboBox.DataSource = GetTrueFalseValueList();
         whiteEnComboBox.DisplayMember = "Key";
         whiteEnComboBox.ValueMember = "Value";
         whiteEnComboBox.SelectedIndex = 0;
      }

      private void ConfigurePayhdrDatatypeComboBox()
      {
         var payHdrDatatypeValueList = new List<DictionaryEntry>();

         payHdrDatatypeValueList.Add(new DictionaryEntry("PN Sequence", niBTSGConstants.PayloadDataTypePnSequence));

         payHdrDatatypeValueList.Add(new DictionaryEntry("User Defined", niBTSGConstants.PayloadDataTypeUserDefinedBits));
         payHdrDatatypeComboBox.DataSource = payHdrDatatypeValueList;
         payHdrDatatypeComboBox.DisplayMember = "Key";
         payHdrDatatypeComboBox.ValueMember = "Value";
         payHdrDatatypeComboBox.SelectedIndex = 0;
      }

      private List<DictionaryEntry> GetTrueFalseValueList()
      {
         var trueFalseValueList = new List<DictionaryEntry>();
         trueFalseValueList.Add(new DictionaryEntry("False", niBTSGConstants.False));
         trueFalseValueList.Add(new DictionaryEntry("True", niBTSGConstants.True));
         return trueFalseValueList;
      }
      #endregion

      void ProcessTimerEvent(object sender, System.EventArgs e)
      {
         CheckGeneration();
      }

      void StartGeneration()
      {
         /*BTSG session*/
         if (btsgSession == null)
         {
            btsgSession = new niBTSG(niBTSGConstants.ToolkitCompatibilityVersion020000);
         }
         //Set BD Address
         niBTSG.ChannelNumberToCarrierFrequency(channelNumber, niBTSGConstants.StandardBasicEDR, out carrierFrequency);
         carrierFreqTextBox.Text = carrierFrequency.ToString();
         btsgSession.SetBDAddress(null, bdAddressLap, bdAddressUap, bdAddressNap);

         //Set Packet Type
         btsgSession.SetPacketType(null, packetType);

         //Set Data Rate
         btsgSession.SetDataRate(null, dataRate);

         /* Set Number of Unique Packets and Idle Slots */
         btsgSession.SetNumberOfUniquePackets(null, uniqPkts);
         btsgSession.SetNumberOfIdleSlots(null, idleSlots);

         //Set LE-TP Payload Type
         btsgSession.SetLETPPayloadType(null, LETPPayloadType);

         //Set LE-TP Corrupt Alternate CRC
         btsgSession.SetLETPCorruptAlternateCrc(null, LETPCorruptAlternateCRC);

         //Set Zadoff-Chu Index
         btsgSession.SetZadoffChuIndex(null, zadoffChuIndex);

         //Set Physical Channel Address
         btsgSession.SetPhysicalChannelAddress(null, physicalChannelAddress);

         //Set HDT Packet Format
         btsgSession.SetHdtPacketFormat(null, hdtPacketFormat);

         //Set HDT PHY Interval
         btsgSession.SetHdtPhyInterval(null, hdtPhyInterval);

         //Set Dirty Tx Settings Mode
         btsgSession.SetDirtyTxMode(null, dirtyTxSettingsMode);

         //Set Parameters Enabled Set
         int parametersEnabledSetArraySize = paraEnabledDataGridView.Rows.Count;
         parametersEnabledSetArr = new int[parametersEnabledSetArraySize];

         for (int rows = 0; rows < parametersEnabledSetArraySize; rows++)
         {

            {
               parametersEnabledSetArr[rows] = Convert.ToInt32(paraEnabledDataGridView.Rows[rows].Cells[1].Value.ToString());
            }
         }

         btsgSession.SetDirtyTxParametersEnabledSet(null, parametersEnabledSetArr, parametersEnabledSetArraySize);

         //Set Carrier Frequency Offset Set
         int carrierFrequencyOffsetArraySize = carrFreqOffsetDataGridView.Rows.Count;
         carrierFrequencyOffsetSetArr = new int[carrierFrequencyOffsetArraySize];

         for (int rows = 0; rows < carrierFrequencyOffsetArraySize; rows++)
         {

            {
               carrierFrequencyOffsetSetArr[rows] = Convert.ToInt32(carrFreqOffsetDataGridView.Rows[rows].Cells[1].Value.ToString());
            }
         }

         btsgSession.SetDirtyTxCarrierFrequencyOffsetSet(null, carrierFrequencyOffsetSetArr, carrierFrequencyOffsetArraySize);

         //Set Modulation Index Set
         int modulationIndexSetArraySize = modulationIndexDataGridView.Rows.Count;
         modulationIndexSetArr = new double[modulationIndexSetArraySize];

         for (int rows = 0; rows < modulationIndexSetArraySize; rows++)
         {

            {
               modulationIndexSetArr[rows] = Convert.ToDouble(modulationIndexDataGridView.Rows[rows].Cells[1].Value.ToString());
            }
         }

         btsgSession.SetDirtyTxModulationIndexSet(null, modulationIndexSetArr, modulationIndexSetArraySize);

         //Set Symbol Timing Error Set
         int symbolTimingErrorSetArraySize = symbolTimingErrorDataGridView.Rows.Count;
         symbolTimingErrorSetArr = new int[symbolTimingErrorSetArraySize];

         for (int rows = 0; rows < symbolTimingErrorSetArraySize; rows++)
         {

            {
               symbolTimingErrorSetArr[rows] = Convert.ToInt32(symbolTimingErrorDataGridView.Rows[rows].Cells[1].Value.ToString());
            }
         }

         btsgSession.SetDirtyTxSymbolTimingErrorSet(null, symbolTimingErrorSetArr, symbolTimingErrorSetArraySize);

         //Set Dirty Tx Modulation Index Type
         btsgSession.SetDirtyTxModulationIndexType(null, dirtyTxModulationIndexType);

         //Set Packet Header
         btsgSession.SetPacketHeaderLTAddress(null, HeaderLtAddress);
         btsgSession.SetPacketHeaderFlow(null, headerFlow);
         btsgSession.SetPacketHeaderArqn(null, packetHeaderArqn);
         btsgSession.SetPacketHeaderSeqn(null, packetHeaderSeqn);

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
         btsgSession.SetDirtyTxEnabled(null, niBTSGConstants.True);


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
         /*Create and Download Waveform*/
         btsgSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", "pkt");
         btsgSession.GetActualHeadroom(null, out actualHeadroom);
         actualHeadroomTextBox.Text = actualHeadroom.ToString();

         niBTSG.RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, script, powerLevel);

         rfsgSession.RF.OutputEnabled = true;
         rfsgSession.Initiate();
         /*Start the status checking timer*/
         timer.Enabled = true;
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
               niBTSG.RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, "");
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

      void ShowError(string functionName, System.Exception exception)
      {
         StopGeneration();
         CloseSession();

         // Display error to the user
         string exceptionMessage = string.IsNullOrEmpty(exception.Message) ? "Undefined Error." : exception.Message;
         errorTextBox.Text = "Error in " + functionName + System.Environment.NewLine + exceptionMessage;
         MessageBox.Show(exceptionMessage, functionName, MessageBoxButtons.OK, MessageBoxIcon.Error); ;
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
            itemIndex = (int)userDefinedBitsComboBox.SelectedValue;
            exportClock = clkOutTerminalComboBox.SelectedValue.ToString();
            packetType = (int)packetComboBox.SelectedValue;
            dataRate = (int)dataRateNumeric.Value;
            packetHeaderArqn = (int)pktHdrArqnComboBox.SelectedValue;
            packetHeaderSeqn = (int)pktHdrSeqnNumeric.Value;
            payloadLengthMode = (int)payHdrPaylenModeComboBox.SelectedValue;
            payloadLength = (int)payHdrPaylenNumeric.Value;
            dataType = (int)payHdrDatatypeComboBox.SelectedValue;
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
            allIqImpairmentsEnabled = (int)allIqImpairEnComboBox.SelectedValue;
            actualHeadroom = System.Convert.ToDouble(actualHeadroomTextBox.Text);
            uniqPkts = (int)numOfUniqNumeric.Value;
            idleSlots = (int)numOfIdleSlotsNumeric.Value;
            whiteningEnabled = (int)whiteEnComboBox.SelectedValue;
            whiteningclock = (int)whiteClkNumeric.Value;
            LETPPayloadType = (int)LETPPayloadTypeComboBox.SelectedValue;
            LETPCorruptAlternateCRC = (int)LETPCorruptAlternateCRCComboBox.SelectedValue;
            zadoffChuIndex = (int)zadoffChuIndexNumeric.Value;
            physicalChannelAddress = (int)physicalChannelAddressNumeric.Value;
            hdtPacketFormat = (int)HdtPacketFormatComboBox.SelectedValue;
            hdtPhyInterval = (double)HdtPhyIntervalNumeric.Value;
            dirtyTxSettingsMode = (int)dirtyTxModeComboBox.SelectedValue;
            dirtyTxModulationIndexType = (int)dirtyTxModulationIndexTypeComboBox.SelectedValue;

            iOffset = (double)iOffsetNumeric.Value;
            qOffset = (double)qOffsetNumeric.Value;
            iCommonModeOffset = (double)iCommonModeOffsetNumeric.Value;
            qCommonModeOffset = (double)qCommonModeOffsetNumeric.Value;
            outputPort = (int)outputPortComboBox.SelectedValue;
            terminalConfiguration = (int)terminalConfigurationComboBox.SelectedValue;

            // Open the BT Generation Session
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

      private void paraEnabledInsert_Click(object sender, EventArgs e)
      {
         int IndexNum = paraEnabledDataGridView.Rows.Add();
         paraEnabledDataGridView.Rows[IndexNum].Cells["parametersEnabledSet"].Value = "0";
         paraEnabledDataGridView.Rows[IndexNum].Cells["Index"].Value = IndexNum.ToString();
      }

      private void paraEnabledDelete_Click(object sender, EventArgs e)
      {
         if (paraEnabledDataGridView.Rows.Count > 0)
         {
            int IndexNum = paraEnabledDataGridView.CurrentCell.RowIndex;
                paraEnabledDataGridView.Rows.RemoveAt(IndexNum);
            for (int i = IndexNum; i < paraEnabledDataGridView.Rows.Count; i++)
            {
                    paraEnabledDataGridView.Rows[i].Cells["Index"].Value = (i).ToString();
            }
         }
      }

        private void carrFreqOffInsert_Click(object sender, EventArgs e)
        {
            int IndexNum = carrFreqOffsetDataGridView.Rows.Add();
            carrFreqOffsetDataGridView.Rows[IndexNum].Cells["carrFreqOffsetSet"].Value = "0";
            carrFreqOffsetDataGridView.Rows[IndexNum].Cells["Index"].Value = IndexNum.ToString();
        }

        private void carrFreqOffDelete_Click(object sender, EventArgs e)
        {
            if (carrFreqOffsetDataGridView.Rows.Count > 0)
            {
                int IndexNum = carrFreqOffsetDataGridView.CurrentCell.RowIndex;
                carrFreqOffsetDataGridView.Rows.RemoveAt(IndexNum);
                for (int i = IndexNum; i < carrFreqOffsetDataGridView.Rows.Count; i++)
                {
                    carrFreqOffsetDataGridView.Rows[i].Cells["Index"].Value = (i).ToString();
                }
            }
        }

        private void modulationIndexInsert_Click(object sender, EventArgs e)
        {
            int IndexNum = modulationIndexDataGridView.Rows.Add();
            modulationIndexDataGridView.Rows[IndexNum].Cells["modulationIndexSet"].Value = "0";
            modulationIndexDataGridView.Rows[IndexNum].Cells["Index"].Value = IndexNum.ToString();
        }

        private void modulationIndexDelete_Click(object sender, EventArgs e)
        {
            if (modulationIndexDataGridView.Rows.Count > 0)
            {
                int IndexNum = modulationIndexDataGridView.CurrentCell.RowIndex;
                modulationIndexDataGridView.Rows.RemoveAt(IndexNum);
                for (int i = IndexNum; i < modulationIndexDataGridView.Rows.Count; i++)
                {
                    modulationIndexDataGridView.Rows[i].Cells["Index"].Value = (i).ToString();
                }
            }
        }
        private void symbolTimingErrorInsert_Click(object sender, EventArgs e)
        {
            int IndexNum = symbolTimingErrorDataGridView.Rows.Add();
            symbolTimingErrorDataGridView.Rows[IndexNum].Cells["symbolTimingErrorSet"].Value = "0";
            symbolTimingErrorDataGridView.Rows[IndexNum].Cells["Index"].Value = IndexNum.ToString();
        }

        private void symbolTimingErrorDelete_Click(object sender, EventArgs e)
        {
            if (symbolTimingErrorDataGridView.Rows.Count > 0)
            {
                int IndexNum = symbolTimingErrorDataGridView.CurrentCell.RowIndex;
                symbolTimingErrorDataGridView.Rows.RemoveAt(IndexNum);
                for (int i = IndexNum; i < symbolTimingErrorDataGridView.Rows.Count; i++)
                {
                    symbolTimingErrorDataGridView.Rows[i].Cells["Index"].Value = (i).ToString();
                }
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
   }
}
