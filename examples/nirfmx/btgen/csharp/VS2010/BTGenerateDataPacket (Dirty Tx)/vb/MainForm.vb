Imports System
Imports System.Collections.Generic
Imports System.Collections
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.RFToolkits.BT.Generation

Partial Public Class MainForm
   Inherits Form

   Private rfsgSession As NIRfsg
   Private btsgSession As niBTSG
   Private resourceName As String, referenceClockSource As String, exportClock As String

   Private script As String = "script GenerateDataPkt" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    repeat forever" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       generate pkt" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    end repeat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " end script"

   Private userDefinedBits As Integer()() = New Integer()() {New Integer() {0, 0, 0, 0, 0, 0,
      0, 0}, New Integer() {1, 1, 1, 1, 1, 1,
      1, 1}, New Integer() {0, 0, 0, 0, 1, 1,
      1, 1}, New Integer() {1, 1, 1, 1, 0, 0,
      0, 0}, New Integer() {1, 0, 1, 0, 1, 0,
      1, 0}, New Integer() {0, 1, 0, 1, 0, 1,
      0, 1}}

   Private powerLevel As Double, carrierFrequency As Double, upConverterCenterFrequency As Double, upConverterCenterFrequencyOffset As Double, actualHeadroom As Double, externalAttenuation As Double,
      headroom As Double, quadratureSkew As Double, iDCOffset As Double, qDCOffset As Double, iQGainImbalance As Double, carrierFrequencyOffset As Double,
      carrierToNoiseRatio As Double, iCommonModeOffset As Double, qCommonModeOffset As Double, iOffset As Double, qOffset As Double, hdtPhyInterval As Double

   Private channelNumber As Integer, itemIndex As Integer, allIqImpairmentsEnabled As Integer, awgnEnabled As Integer, whiteningEnabled As Integer, autoHeadroomEnabled As Integer,
      packetHeaderArqn As Integer, packetHeaderSeqn As Integer, actualPayloadLength As Integer, dataType As Integer, bdAddressLap As Integer, bdAddressUap As Integer,
      payloadLengthMode As Integer, payloadLength As Integer, bdAddressNap As Integer, packetType As Integer, HeaderLtAddress As Integer, headerFlow As Integer,
      headerPayloadLlid As Integer, seed As Integer, headerSeqn As Integer, dataPNOrder As Integer, whiteningclock As Integer, uniqPkts As Integer,
      idleSlots As Integer, outputPort As Integer, terminalConfiguration As Integer, dataRate As Integer, LETPPayloadType As Integer, LETPCorruptAlternateCRC As Integer,
      zadoffChuIndex As Integer, physicalChannelAddress As Integer, hdtPacketFormat As Integer, dirtyTxSettingsMode As Integer, dirtyTxModulationIndexType As Integer

   Private parametersEnabledSetArr As Integer(), carrierFrequencyOffsetSetArr As Integer(), symbolTimingErrorSetArr As Integer()
   Private modulationIndexSetArr As Double()
   Private model As String
   Private generationStatus As RfsgGenerationStatus

   Public Sub New()
      InitializeComponent()
      ConfigureNumericUpDown()
      ConfigureAutoheadroomEnabComboBox()
      ConfigureRefSourceComboBox()
      ConfigureClkOutTerminalComboBox()
      ConfigureAllIqImpairEnComboBox()
      ConfigureAwgnEnabledComboBox()
      ConfigurePkthdrArqnComboBox()
      ConfigurePacketComboBox()
      ConfigurePayhdrPaylenModeComboBox()
      ConfigurePayhdrDatatypeComboBox()
      ConfigureWhiteEnComboBox()
      ConfigurestopButton()
      ConfigureUserDefinedBitsComboBox()
      ConfigureOutputPortComboBox()
      ConfigureTerminalConfigurationComboBox()
      ConfigureLETPPayloadTypeComboBox()
      ConfigureLETPCorruptAlternateCRCComboBox()
      ConfigurehdtPacketFormatComboBox()
      ConfigureDirtyTxSettingsModeComboBox()
      ConfigureParametersEnabledSetComboBox()
      ConfigureDirtyTxModulationIndexTypeComboBox()
   End Sub

#Region "UI Initial Value Config Section"

   Private Sub ConfigurestopButton()
      'Deactivate the 'Stop' button
      stopButton.Enabled = False
   End Sub

   Private Sub ConfigureUserDefinedBitsComboBox()
      Dim userDefinedBitsValueList As New List(Of DictionaryEntry)()
      userDefinedBitsValueList.Add(New DictionaryEntry("0 0 0 0 0 0 0 0", 0))
      userDefinedBitsValueList.Add(New DictionaryEntry("1 1 1 1 1 1 1 1", 1))
      userDefinedBitsValueList.Add(New DictionaryEntry("0 0 0 0 1 1 1 1", 2))
      userDefinedBitsValueList.Add(New DictionaryEntry("1 1 1 1 0 0 0 0", 3))
      userDefinedBitsValueList.Add(New DictionaryEntry("1 0 1 0 1 0 1 0", 4))
      userDefinedBitsValueList.Add(New DictionaryEntry("0 1 0 1 0 1 0 1", 5))
      userDefinedBitsComboBox.DataSource = userDefinedBitsValueList
      userDefinedBitsComboBox.DisplayMember = "Key"
      userDefinedBitsComboBox.ValueMember = "Value"
      userDefinedBitsComboBox.SelectedIndex = 4
   End Sub

   Private Sub ConfigureNumericUpDown()
      chnNumberNumeric.Minimum = 0
      chnNumberNumeric.Maximum = 79
      chnNumberNumeric.Increment = 1
      chnNumberNumeric.Value = 3
      chnNumberNumeric.DecimalPlaces = 0

      powerLevelNumeric.Minimum = Decimal.MinValue
      powerLevelNumeric.Maximum = Decimal.MaxValue
      powerLevelNumeric.Increment = 1
      powerLevelNumeric.Value = 0
      powerLevelNumeric.DecimalPlaces = 2

      externalAttnNumeric.Minimum = Decimal.MinValue
      externalAttnNumeric.Maximum = Decimal.MaxValue
      externalAttnNumeric.Increment = 0.1D
      externalAttnNumeric.Value = 0
      externalAttnNumeric.DecimalPlaces = 2

      headroomNumeric.Minimum = Decimal.MinValue
      headroomNumeric.Maximum = Decimal.MaxValue
      headroomNumeric.Increment = 1
      headroomNumeric.Value = 0
      headroomNumeric.DecimalPlaces = 2

      quadratureSkewNumeric.Minimum = Decimal.MinValue
      quadratureSkewNumeric.Maximum = Decimal.MaxValue
      quadratureSkewNumeric.Increment = 0.5D
      quadratureSkewNumeric.Value = 0
      quadratureSkewNumeric.DecimalPlaces = 2

      iDCOffsetNumeric.Minimum = Decimal.MinValue
      iDCOffsetNumeric.Maximum = Decimal.MaxValue
      iDCOffsetNumeric.Increment = 0.5D
      iDCOffsetNumeric.Value = 0
      iDCOffsetNumeric.DecimalPlaces = 2

      qDCOffsetNumeric.Minimum = Decimal.MinValue
      qDCOffsetNumeric.Maximum = Decimal.MaxValue
      qDCOffsetNumeric.Increment = 0.5D
      qDCOffsetNumeric.Value = 0
      qDCOffsetNumeric.DecimalPlaces = 2

      qCommonModeOffsetNumeric.Minimum = Decimal.MinValue
      qCommonModeOffsetNumeric.Maximum = Decimal.MaxValue
      qCommonModeOffsetNumeric.Increment = 1
      qCommonModeOffsetNumeric.Value = 0
      qCommonModeOffsetNumeric.DecimalPlaces = 0

      iCommonModeOffsetNumeric.Minimum = Decimal.MinValue
      iCommonModeOffsetNumeric.Maximum = Decimal.MaxValue
      iCommonModeOffsetNumeric.Increment = 1
      iCommonModeOffsetNumeric.Value = 0
      iCommonModeOffsetNumeric.DecimalPlaces = 0

      qOffsetNumeric.Minimum = Decimal.MinValue
      qOffsetNumeric.Maximum = Decimal.MaxValue
      qOffsetNumeric.Increment = 1
      qOffsetNumeric.Value = 0
      qOffsetNumeric.DecimalPlaces = 0

      iOffsetNumeric.Minimum = Decimal.MinValue
      iOffsetNumeric.Maximum = Decimal.MaxValue
      iOffsetNumeric.Increment = 1
      iOffsetNumeric.Value = 0
      iOffsetNumeric.DecimalPlaces = 0

      iqGaimbalanceNumeric.Minimum = Decimal.MinValue
      iqGaimbalanceNumeric.Maximum = Decimal.MaxValue
      iqGaimbalanceNumeric.Increment = 0.5D
      iqGaimbalanceNumeric.Value = 0
      iqGaimbalanceNumeric.DecimalPlaces = 2

      carrierFreqOffNumeric.Minimum = Decimal.MinValue
      carrierFreqOffNumeric.Maximum = Decimal.MaxValue
      carrierFreqOffNumeric.Increment = 1
      carrierFreqOffNumeric.Value = 0
      carrierFreqOffNumeric.DecimalPlaces = 3

      cnrNumeric.Minimum = Decimal.MinValue
      cnrNumeric.Maximum = Decimal.MaxValue
      cnrNumeric.Increment = 0.5D
      cnrNumeric.Value = 50
      cnrNumeric.DecimalPlaces = 2

      bdaddrLapNumeric.Minimum = Integer.MinValue
      bdaddrLapNumeric.Maximum = Integer.MaxValue
      bdaddrLapNumeric.Increment = 1
      bdaddrLapNumeric.Value = 0
      bdaddrLapNumeric.DecimalPlaces = 0

      bdaddrUapNumeric.Minimum = Integer.MinValue
      bdaddrUapNumeric.Maximum = Integer.MaxValue
      bdaddrUapNumeric.Increment = 1
      bdaddrUapNumeric.Value = 0
      bdaddrUapNumeric.DecimalPlaces = 0

      bdaddrNapNumeric.Minimum = 0
      bdaddrNapNumeric.Maximum = 30
      bdaddrNapNumeric.Increment = 0
      bdaddrNapNumeric.Value = 0
      bdaddrNapNumeric.DecimalPlaces = 0

      pktLtAddrNumeric.Minimum = Integer.MinValue
      pktLtAddrNumeric.Maximum = Integer.MaxValue
      pktLtAddrNumeric.Increment = 1
      pktLtAddrNumeric.Value = 0
      pktLtAddrNumeric.DecimalPlaces = 0

      pktHdrFlowNumeric.Minimum = Integer.MinValue
      pktHdrFlowNumeric.Maximum = Integer.MaxValue
      pktHdrFlowNumeric.Increment = 1
      pktHdrFlowNumeric.Value = 0
      pktHdrFlowNumeric.DecimalPlaces = 0

      pktHdrSeqnNumeric.Minimum = Integer.MinValue
      pktHdrSeqnNumeric.Maximum = Integer.MaxValue
      pktHdrSeqnNumeric.Increment = 1
      pktHdrSeqnNumeric.Value = 0
      pktHdrSeqnNumeric.DecimalPlaces = 0

      payHdrLlidNumeric.Minimum = Integer.MinValue
      payHdrLlidNumeric.Maximum = Integer.MaxValue
      payHdrLlidNumeric.Increment = 1
      payHdrLlidNumeric.Value = 0
      payHdrLlidNumeric.DecimalPlaces = 0

      payHdrFlowNumeric.Minimum = Integer.MinValue
      payHdrFlowNumeric.Maximum = Integer.MaxValue
      payHdrFlowNumeric.Increment = 1
      payHdrFlowNumeric.Value = 0
      payHdrFlowNumeric.DecimalPlaces = 0

      payHdrPaylenNumeric.Minimum = Integer.MinValue
      payHdrPaylenNumeric.Maximum = Integer.MaxValue
      payHdrPaylenNumeric.Increment = 1
      payHdrPaylenNumeric.Value = 1
      payHdrPaylenNumeric.DecimalPlaces = 0

      paydatPnorderNumeric.Minimum = Integer.MinValue
      paydatPnorderNumeric.Maximum = Integer.MaxValue
      paydatPnorderNumeric.Increment = 1
      paydatPnorderNumeric.Value = 9
      paydatPnorderNumeric.DecimalPlaces = 0

      paydatSeedNumeric.Minimum = Integer.MinValue
      paydatSeedNumeric.Maximum = Integer.MaxValue
      paydatSeedNumeric.Increment = 1
      paydatSeedNumeric.Value = 497
      paydatSeedNumeric.DecimalPlaces = 0

      HdtPhyIntervalNumeric.Minimum = decimal.MinValue
      HdtPhyIntervalNumeric.Maximum = decimal.MaxValue
      HdtPhyIntervalNumeric.Increment = 1
      HdtPhyIntervalNumeric.Value = 0
      HdtPhyIntervalNumeric.DecimalPlaces = 2
   End Sub

   Private Sub ConfigureAutoheadroomEnabComboBox()
      Dim autoheadroomEnabValueList As New List(Of DictionaryEntry)()
      autoHeadroomEnabComboBox.DataSource = GetTrueFalseValueList()
      autoHeadroomEnabComboBox.DisplayMember = "Key"
      autoHeadroomEnabComboBox.ValueMember = "Value"
      autoHeadroomEnabComboBox.SelectedIndex = 1
   End Sub

   Private Sub ConfigureOutputPortComboBox()
      Dim opList As New List(Of DictionaryEntry)()
      opList.Add(New DictionaryEntry("RF Out", RfsgOutputPort.RFOut))
      opList.Add(New DictionaryEntry("IQ Out", RfsgOutputPort.IQOut))
      outputPortComboBox.DataSource = opList
      outputPortComboBox.DisplayMember = "Key"
      outputPortComboBox.ValueMember = "Value"
      outputPortComboBox.SelectedIndex = 0
   End Sub

   Private Sub ConfigureTerminalConfigurationComboBox()
      Dim tcList As New List(Of DictionaryEntry)()
      tcList.Add(New DictionaryEntry("Differential", RfsgTerminalConfiguration.Differential))
      tcList.Add(New DictionaryEntry("SingleEnded", RfsgTerminalConfiguration.SingleEnded))
      terminalConfigurationComboBox.DataSource = tcList
      terminalConfigurationComboBox.DisplayMember = "Key"
      terminalConfigurationComboBox.ValueMember = "Value"
      terminalConfigurationComboBox.SelectedIndex = 0
   End Sub

   Private Sub ConfigureRefSourceComboBox()
      Dim refSourceValueList As New List(Of DictionaryEntry)()
      refSourceValueList.Add(New DictionaryEntry("OnBoardClock", RfsgFrequencyReferenceSource.OnboardClock))
      refSourceValueList.Add(New DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn))
      refSourceValueList.Add(New DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock))
      refSourceValueList.Add(New DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn))
      refSourceComboBox.DataSource = refSourceValueList
      refSourceComboBox.DisplayMember = "Key"
      refSourceComboBox.ValueMember = "Value"
      refSourceComboBox.SelectedIndex = 2
   End Sub

   Private Sub ConfigureClkOutTerminalComboBox()
      Dim clkOutTerminalValueList As New List(Of DictionaryEntry)()
      clkOutTerminalValueList.Add(New DictionaryEntry("Do not export clock", RfsgOutputTerminal.DoNotExport))
      clkOutTerminalValueList.Add(New DictionaryEntry("RefOut", RfsgOutputTerminal.ReferenceOut))
      clkOutTerminalValueList.Add(New DictionaryEntry("RefOut2", RfsgOutputTerminal.ReferenceOut2))
      clkOutTerminalValueList.Add(New DictionaryEntry("ClkOut", RfsgOutputTerminal.ClockOut))
      clkOutTerminalComboBox.DataSource = clkOutTerminalValueList
      clkOutTerminalComboBox.DisplayMember = "Key"
      clkOutTerminalComboBox.ValueMember = "Value"
      clkOutTerminalComboBox.SelectedIndex = 0
   End Sub

   Private Sub ConfigureAllIqImpairEnComboBox()
      Dim allIqImpairEnValueList As New List(Of DictionaryEntry)()
      allIqImpairEnComboBox.DataSource = GetTrueFalseValueList()
      allIqImpairEnComboBox.DisplayMember = "Key"
      allIqImpairEnComboBox.ValueMember = "Value"
      allIqImpairEnComboBox.SelectedIndex = 0
   End Sub

   Private Sub ConfigureAwgnEnabledComboBox()
      Dim awgnEnabledValueList As New List(Of DictionaryEntry)()
      awgnEnabledComboBox.DataSource = GetTrueFalseValueList()
      awgnEnabledComboBox.DisplayMember = "Key"
      awgnEnabledComboBox.ValueMember = "Value"
      awgnEnabledComboBox.SelectedIndex = 0
   End Sub

   Private Sub ConfigurePkthdrArqnComboBox()
      Dim pktHdrArqnValueList As New List(Of DictionaryEntry)()
      pktHdrArqnValueList.Add(New DictionaryEntry("ACK", niBTSGConstants.PacketHeaderArqnAck))
      pktHdrArqnValueList.Add(New DictionaryEntry("NAK", niBTSGConstants.PacketHeaderArqnNak))
      pktHdrArqnComboBox.DataSource = pktHdrArqnValueList
      pktHdrArqnComboBox.DisplayMember = "Key"
      pktHdrArqnComboBox.ValueMember = "Value"
      pktHdrArqnComboBox.SelectedValue = niBTSGConstants.PacketHeaderArqnNak
   End Sub

   Private Sub ConfigurePacketComboBox()
      Dim packetValueList As New List(Of DictionaryEntry)()
      packetValueList.Add(New DictionaryEntry("Null", niBTSGConstants.PacketTypeNull))
      packetValueList.Add(New DictionaryEntry("Poll", niBTSGConstants.PacketTypePoll))
      packetValueList.Add(New DictionaryEntry("Fhs", niBTSGConstants.PacketTypeFhs))
      packetValueList.Add(New DictionaryEntry("DM1", niBTSGConstants.PacketTypeDm1))
      packetValueList.Add(New DictionaryEntry("DH1", niBTSGConstants.PacketTypeDh1))
      packetValueList.Add(New DictionaryEntry("2DH1", niBTSGConstants.PacketType2Dh1))
      packetValueList.Add(New DictionaryEntry("HV1", niBTSGConstants.PacketTypeHv1))
      packetValueList.Add(New DictionaryEntry("HV2", niBTSGConstants.PacketTypeHv2))
      packetValueList.Add(New DictionaryEntry("2EV3", niBTSGConstants.PacketType2Ev3))
      packetValueList.Add(New DictionaryEntry("HV2", niBTSGConstants.PacketTypeHv3))
      packetValueList.Add(New DictionaryEntry("EV3", niBTSGConstants.PacketTypeEv3))
      packetValueList.Add(New DictionaryEntry("3EV3", niBTSGConstants.PacketType3Ev3))
      packetValueList.Add(New DictionaryEntry("DV", niBTSGConstants.PacketTypeDv))
      packetValueList.Add(New DictionaryEntry("3DH1", niBTSGConstants.PacketType3Dh1))
      packetValueList.Add(New DictionaryEntry("Aux1", niBTSGConstants.PacketTypeAux1))
      packetValueList.Add(New DictionaryEntry("Dm3", niBTSGConstants.PacketTypeDm3))
      packetValueList.Add(New DictionaryEntry("2Dh3", niBTSGConstants.PacketType2Dh3))
      packetValueList.Add(New DictionaryEntry("Dh3", niBTSGConstants.PacketTypeDh3))
      packetValueList.Add(New DictionaryEntry("3Dh3", niBTSGConstants.PacketType3Dh3))
      packetValueList.Add(New DictionaryEntry("EV4", niBTSGConstants.PacketTypeEv4))
      packetValueList.Add(New DictionaryEntry("2EV5", niBTSGConstants.PacketType2Ev5))
      packetValueList.Add(New DictionaryEntry("EV5", niBTSGConstants.PacketTypeEv5))
      packetValueList.Add(New DictionaryEntry("3EV5", niBTSGConstants.PacketType3Ev5))
      packetValueList.Add(New DictionaryEntry("Dm5", niBTSGConstants.PacketTypeDm5))
      packetValueList.Add(New DictionaryEntry("2Dh5", niBTSGConstants.PacketType2Dh5))
      packetValueList.Add(New DictionaryEntry("Dh5", niBTSGConstants.PacketTypeDh5))
      packetValueList.Add(New DictionaryEntry("Dm3", niBTSGConstants.PacketTypeDm3))
      packetValueList.Add(New DictionaryEntry("Id", niBTSGConstants.PacketTypeId))
      packetComboBox.DataSource = packetValueList
      packetComboBox.DisplayMember = "Key"
      packetComboBox.ValueMember = "Value"
      packetComboBox.SelectedIndex = niBTSGConstants.PacketTypeDh1
   End Sub

   Private Sub ConfigurePayhdrPaylenModeComboBox()
      Dim payHdrPaylenModeValueList As New List(Of DictionaryEntry)()
      payHdrPaylenModeValueList.Add(New DictionaryEntry("Maximum Length", niBTSGConstants.PayloadLengthModeMaximumLength))
      payHdrPaylenModeValueList.Add(New DictionaryEntry("User Defined", niBTSGConstants.PayloadLengthModeUserDefined))
      payHdrPaylenModeComboBox.DataSource = payHdrPaylenModeValueList
      payHdrPaylenModeComboBox.DisplayMember = "Key"
      payHdrPaylenModeComboBox.ValueMember = "Value"
      payHdrPaylenModeComboBox.SelectedIndex = 0
   End Sub

   Private Sub ConfigureWhiteEnComboBox()
      Dim whiteEnValueList As New List(Of DictionaryEntry)()
      whiteEnComboBox.DataSource = GetTrueFalseValueList()
      whiteEnComboBox.DisplayMember = "Key"
      whiteEnComboBox.ValueMember = "Value"
      whiteEnComboBox.SelectedIndex = 0
   End Sub

   Private Sub ConfigurePayhdrDatatypeComboBox()
      Dim payHdrDatatypeValueList As New List(Of DictionaryEntry)()
      payHdrDatatypeValueList.Add(New DictionaryEntry("PN Sequence", niBTSGConstants.PayloadDataTypePnSequence))
      payHdrDatatypeValueList.Add(New DictionaryEntry("User Defined", niBTSGConstants.PayloadDataTypeUserDefinedBits))
      payHdrDatatypeComboBox.DataSource = payHdrDatatypeValueList
      payHdrDatatypeComboBox.DisplayMember = "Key"
      payHdrDatatypeComboBox.ValueMember = "Value"
      payHdrDatatypeComboBox.SelectedIndex = 0
   End Sub

   Private Sub ConfigureLETPPayloadTypeComboBox()
      Dim LETPPayloadTypeValueList As New List(Of DictionaryEntry)()
      LETPPayloadTypeValueList.Add(new DictionaryEntry("PRBS9", niBTSGConstants.LETPPayloadTypePRBS9))
      LETPPayloadTypeValueList.Add(new DictionaryEntry("11110000", niBTSGConstants.LETPPayloadType11110000))
      LETPPayloadTypeValueList.Add(new DictionaryEntry("10101010", niBTSGConstants.LETPPayloadType10101010))
      LETPPayloadTypeValueList.Add(new DictionaryEntry("PRBS15", niBTSGConstants.LETPPayloadTypePRBS15))
      LETPPayloadTypeValueList.Add(new DictionaryEntry("1111111", niBTSGConstants.LETPPayloadType11111111))
      LETPPayloadTypeValueList.Add(new DictionaryEntry("00000000", niBTSGConstants.LETPPayloadType00000000))
      LETPPayloadTypeValueList.Add(new DictionaryEntry("00001111", niBTSGConstants.LETPPayloadType00001111))
      LETPPayloadTypeValueList.Add(new DictionaryEntry("01010101", niBTSGConstants.LETPPayloadType01010101))
      LETPPayloadTypeValueList.Add(new DictionaryEntry("User Defined Bits", niBTSGConstants.LETPPayloadTypeUserDefinedBits))
      LETPPayloadTypeComboBox.DataSource = LETPPayloadTypeValueList
      LETPPayloadTypeComboBox.DisplayMember = "Key"
      LETPPayloadTypeComboBox.ValueMember = "Value"
      LETPPayloadTypeComboBox.SelectedIndex = niBTSGConstants.LETPPayloadTypePRBS9
   End Sub

   Private Sub ConfigureLETPCorruptAlternateCRCComboBox()
      Dim LETPCorruptAlternateCRCValueList As new List(Of DictionaryEntry)()
      LETPCorruptAlternateCRCValueList.Add(new DictionaryEntry("False", niBTSGConstants.False))
      LETPCorruptAlternateCRCValueList.Add(new DictionaryEntry("True", niBTSGConstants.True))
      LETPCorruptAlternateCRCComboBox.DataSource = LETPCorruptAlternateCRCValueList
      LETPCorruptAlternateCRCComboBox.DisplayMember = "Key"
      LETPCorruptAlternateCRCComboBox.ValueMember = "Value"
      LETPCorruptAlternateCRCComboBox.SelectedIndex = niBTSGConstants.False
   End Sub   

   Private Sub ConfigurehdtPacketFormatComboBox()
      Dim hdtPacketFormatValueList As new List(Of DictionaryEntry)()
      hdtPacketFormatValueList.Add(new DictionaryEntry("Short Format", niBTSGConstants.HdtPacketFormat_ShortFormat))
      hdtPacketFormatValueList.Add(new DictionaryEntry("Format0", niBTSGConstants.HdtPacketFormat_Format0))
      hdtPacketFormatValueList.Add(new DictionaryEntry("Format1", niBTSGConstants.HdtPacketFormat_Format1))
      HdtPacketFormatComboBox.DataSource = hdtPacketFormatValueList
      HdtPacketFormatComboBox.DisplayMember = "Key"
      HdtPacketFormatComboBox.ValueMember = "Value"
      HdtPacketFormatComboBox.SelectedIndex = niBTSGConstants.HdtPacketFormat_Format0
   End Sub

   Private Sub ConfigureDirtyTxSettingsModeComboBox()
      Dim dirtyTxSettingsValueList As new List(Of DictionaryEntry)()
      dirtyTxSettingsValueList.Add(new DictionaryEntry("Standard", niBTSGConstants.DirtyTxModeStandard))
      dirtyTxSettingsValueList.Add(new DictionaryEntry("User Defined", niBTSGConstants.DirtyTxModeUserDefined))
      dirtyTxModeComboBox.DataSource = dirtyTxSettingsValueList
      dirtyTxModeComboBox.DisplayMember = "Key"
      dirtyTxModeComboBox.ValueMember = "Value"
      dirtyTxModeComboBox.SelectedIndex = niBTSGConstants.DirtyTxModeStandard
   End Sub

   Private Sub ConfigureParametersEnabledSetComboBox()
      Dim parametersEnabledSetValueList As new List(Of DictionaryEntry)()
      parametersEnabledSetValueList.Add(new DictionaryEntry("False", niBTSGConstants.False))
      parametersEnabledSetValueList.Add(new DictionaryEntry("True", niBTSGConstants.True))
      parametersEnabledSet.DataSource = parametersEnabledSetValueList
      parametersEnabledSet.DisplayMember = "Key"
      parametersEnabledSet.ValueMember = "Value"
   End Sub

   Private Sub ConfigureDirtyTxModulationIndexTypeComboBox()
      Dim dirtyTxModulationIndexTypeValueList As new List(Of DictionaryEntry)()
      dirtyTxModulationIndexTypeValueList.Add(new DictionaryEntry("Standard", niBTSGConstants.DirtyTxModulationIndexTypeStandard))
      dirtyTxModulationIndexTypeValueList.Add(new DictionaryEntry("Stable", niBTSGConstants.DirtyTxModulationIndexTypeStable))
      dirtyTxModulationIndexTypeComboBox.DataSource = dirtyTxModulationIndexTypeValueList
      dirtyTxModulationIndexTypeComboBox.DisplayMember = "Key"
      dirtyTxModulationIndexTypeComboBox.ValueMember = "Value"
      dirtyTxModulationIndexTypeComboBox.SelectedIndex = niBTSGConstants.DirtyTxModulationIndexTypeStandard
   End Sub


   Private Function GetTrueFalseValueList() As List(Of DictionaryEntry)
      Dim trueFalseValueList As New List(Of DictionaryEntry)()
      trueFalseValueList.Add(New DictionaryEntry("False", niBTSGConstants.[False]))
      trueFalseValueList.Add(New DictionaryEntry("True", niBTSGConstants.[True]))
      Return trueFalseValueList
   End Function
#End Region

   Private Sub ProcessTimerEvent(sender As Object, e As System.EventArgs)
      CheckGeneration()
   End Sub

   Private Sub StartGeneration()

      'BTSG session

      If btsgSession Is Nothing Then
         btsgSession = New niBTSG(niBTSGConstants.ToolkitCompatibilityVersion020000)
      End If
      'Set BD Address
      niBTSG.ChannelNumberToCarrierFrequency(channelNumber, niBTSGConstants.StandardBasicEDR, carrierFrequency)
      carrierFreqTextBox.Text = carrierFrequency.ToString()
      btsgSession.SetBDAddress(Nothing, bdAddressLap, bdAddressUap, bdAddressNap)

      'Set Packet Type
      btsgSession.SetPacketType(Nothing, packetType)

      'Set Data Rate
      btsgSession.SetDataRate(Nothing, dataRate)

      ' Set Number of Unique Packets and Idle Slots 

      btsgSession.SetNumberOfUniquePackets(Nothing, uniqPkts)
      btsgSession.SetNumberOfIdleSlots(Nothing, idleSlots)

      ' Set LE-TP Payload Type
      btsgSession.SetLETPPayloadType(Nothing, LETPPayloadType)

      ' Set LE-TP Corrupt Alternate CRC
      btsgSession.SetLETPCorruptAlternateCrc(Nothing, LETPCorruptAlternateCRC)

      ' Set Zadoff-Chu Index
      btsgSession.SetZadoffChuIndex(Nothing, zadoffChuIndex)

      ' Set Physical Channel Address
      btsgSession.SetPhysicalChannelAddress(Nothing, physicalChannelAddress)

      ' Set HDT Packet Format
      btsgSession.SetHdtPacketFormat(Nothing, hdtPacketFormat)

      ' Set HDT PHY Interval
      btsgSession.SetHdtPhyInterval(Nothing, hdtPhyInterval)

      ' Set Dirty Tx Settings Mode
      btsgSession.SetDirtyTxMode(Nothing, dirtyTxSettingsMode)

      ' Set Parameters Enabled Set
      Dim parametersEnabledSetArraySize As Integer =  paraEnabledDataGridView.Rows.Count
      parametersEnabledSetArr = new Integer (parametersEnabledSetArraySize - 1) {}
         For rows As Integer = 0 To parametersEnabledSetArraySize - 1
            If True Then
               parametersEnabledSetArr(rows) = Convert.ToInt32(paraEnabledDataGridView.Rows(rows).Cells(1).Value.ToString())
            End If 
         Next
         btsgSession.SetDirtyTxParametersEnabledSet(Nothing, parametersEnabledSetArr, parametersEnabledSetArraySize)

      ' Set Carrier Frequency Offset Set
      Dim carrierFrequencyOffsetArraySize As Integer = carrFreqOffsetDataGridView.Rows.Count
      carrierFrequencyOffsetSetArr = new Integer (carrierFrequencyOffsetArraySize - 1) {}
         For rows As Integer = 0 To carrierFrequencyOffsetArraySize - 1
            If True Then
               carrierFrequencyOffsetSetArr(rows) = Convert.ToInt32(carrFreqOffsetDataGridView.Rows(rows).Cells(1).Value.ToString())
            End If
         Next
         btsgSession.SetDirtyTxCarrierFrequencyOffsetSet(Nothing, carrierFrequencyOffsetSetArr, carrierFrequencyOffsetArraySize)

      ' Set Modulation Index Set
      Dim modulationIndexSetArraySize As Integer = modulationIndexDataGridView.Rows.Count
      modulationIndexSetArr = new double (modulationIndexSetArraySize - 1) {}
         For rows As Integer = 0 To modulationIndexSetArraySize - 1
            If True Then
               modulationIndexSetArr(rows) = Convert.ToDouble(modulationIndexDataGridView.Rows(rows).Cells(1).Value.ToString())
            End If
         Next
         btsgSession.SetDirtyTxModulationIndexSet(Nothing, modulationIndexSetArr, modulationIndexSetArraySize)

      ' Set Symbol Timing Error Set
      Dim symbolTimingErrorSetArraySize As Integer = symbolTimingErrorDataGridView.Rows.Count
      symbolTimingErrorSetArr = new Integer (symbolTimingErrorSetArraySize - 1) {}
         For rows As Integer = 0 To symbolTimingErrorSetArraySize - 1
            If True Then
               symbolTimingErrorSetArr(rows) = Convert.ToInt32(symbolTimingErrorDataGridView.Rows(rows).Cells(1).Value.ToString())
            End If
         Next
         btsgSession.SetDirtyTxSymbolTimingErrorSet(Nothing, symbolTimingErrorSetArr, symbolTimingErrorSetArraySize)

       ' Set Dirty Tx Modulation Index Type
         btsgSession.SetDirtyTxModulationIndexType(Nothing, dirtyTxModulationIndexType)

      'Set Packet Header
      btsgSession.SetPacketHeaderLTAddress(Nothing, HeaderLtAddress)
      btsgSession.SetPacketHeaderFlow(Nothing, headerFlow)
      btsgSession.SetPacketHeaderArqn(Nothing, packetHeaderArqn)
      btsgSession.SetPacketHeaderSeqn(Nothing, packetHeaderSeqn)

      'Set Payload Header
      btsgSession.SetPayloadHeaderLlid(Nothing, headerPayloadLlid)
      btsgSession.SetPayloadHeaderFlow(Nothing, headerFlow)
      btsgSession.SetPayloadLengthMode(Nothing, payloadLengthMode)

      If payloadLengthMode = niBTSGConstants.PayloadLengthModeUserDefined Then

         btsgSession.SetPayloadLength(Nothing, payloadLength)
      End If
      btsgSession.GetActualPayloadLength(Nothing, actualPayloadLength)

      payHdrActPaylenTextBox.Text = actualPayloadLength.ToString()

      'Set Payload Data
      btsgSession.SetPayloadDataType(Nothing, dataType)

      If dataType = niBTSGConstants.PayloadDataTypePnSequence Then
         btsgSession.SetPayloadPNOrder(Nothing, dataPNOrder)
         btsgSession.SetPayloadPNSeed(Nothing, seed)
      Else

         btsgSession.SetPayloadUserDefinedBits(Nothing, DirectCast(userDefinedBits(itemIndex), Integer()), 8)
      End If

      'Waveform Properties

      'Set Headroom Properties
      btsgSession.SetAutoHeadroomEnabled(Nothing, autoHeadroomEnabled)
      If autoHeadroomEnabled = niBTSGConstants.[False] Then
         btsgSession.SetHeadroom(Nothing, headroom)
      End If

      btsgSession.SetCarrierFrequencyOffset(Nothing, carrierFrequencyOffset)

      'Set Impairments

      btsgSession.SetAllIqImpairmentsEnabled(Nothing, allIqImpairmentsEnabled)

      If allIqImpairmentsEnabled = niBTSGConstants.[True] Then
         btsgSession.SetQuadratureSkew(Nothing, quadratureSkew)
         btsgSession.SetIDCOffset(Nothing, iDCOffset)
         btsgSession.SetQDCOffset(Nothing, qDCOffset)
         btsgSession.SetIQGainImbalance(Nothing, iQGainImbalance)
      End If

      btsgSession.SetAwgnEnabled(Nothing, awgnEnabled)
      btsgSession.SetCarrierToNoiseRatio(Nothing, carrierToNoiseRatio)

      'Whitening Properties
      btsgSession.SetWhiteningEnabled(Nothing, whiteningEnabled)
      btsgSession.SetWhiteningClock(Nothing, whiteningclock)
      btsgSession.SetDirtyTxEnabled(Nothing, niBTSGConstants.[True])

      'RFSG Session
      If rfsgSession Is Nothing Then

         rfsgSession = New NIRfsg(resourceName, False, True)
      End If
      'get model
      model = rfsgSession.Identity.InstrumentModel

      'RFSG Properties
      rfsgSession.FrequencyReference.Configure(referenceClockSource, 10000000.0)
      rfsgSession.Utility.ExportSignal(RfsgSignalType.ReferenceClock, "", exportClock)
      If model.Equals("NI PXIe-5644R") OrElse model.Equals("NI PXIe-5645R") OrElse model.Equals("NI PXIe-5646R") Then
         If outputPort = RfsgOutputPort.IQOut Then
            rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
            rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
            rfsgSession.IQOutPort("I").TerminalConfiguration = CType(terminalConfiguration, RfsgTerminalConfiguration)
            rfsgSession.IQOutPort("I").CommonModeOffset = iCommonModeOffset
            rfsgSession.IQOutPort("I").Offset = iOffset
            rfsgSession.IQOutPort("Q").CommonModeOffset = qCommonModeOffset
            rfsgSession.IQOutPort("Q").Offset = qOffset
            carrierFrequency = 0
         Else
            rfsgSession.RF.Frequency = carrierFrequency
            rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
            rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
            upConverterCenterFrequencyOffset = 0
            upConverterCenterFrequency = carrierFrequency + upConverterCenterFrequencyOffset
            rfsgSession.RF.Upconverter.CenterFrequency = upConverterCenterFrequency
         End If
      Else
         rfsgSession.RF.Frequency = carrierFrequency
         rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
         rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
         upConverterCenterFrequencyOffset = 0
         upConverterCenterFrequency = carrierFrequency + upConverterCenterFrequencyOffset
         rfsgSession.RF.Upconverter.CenterFrequency = upConverterCenterFrequency
      End If
      rfsgSession.RF.ExternalGain = -(externalAttenuation)
      'Create and Download Waveform

      btsgSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", "pkt")
      btsgSession.GetActualHeadroom(Nothing, actualHeadroom)
      actualHeadroomTextBox.Text = actualHeadroom.ToString()

      niBTSG.RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, script, powerLevel)

      rfsgSession.RF.OutputEnabled = True
      rfsgSession.Initiate()
      'Start the status checking timer

      timer.Enabled = True
   End Sub

   Private Sub CheckGeneration()
      Try
         generationStatus = RfsgGenerationStatus.InProgress
         ' Check Generation Status
         If rfsgSession IsNot Nothing Then
            generationStatus = rfsgSession.CheckGenerationStatus()
         End If
      Catch exception As System.Exception
         ShowError("CheckGeneration()", exception)
      End Try
   End Sub

   Private Sub StopGeneration()
      'Stop the status checking timer
      timer.Enabled = False
      If rfsgSession IsNot Nothing Then
         rfsgSession.Abort()
         rfsgSession.RF.OutputEnabled = False
         rfsgSession.Utility.Commit()

         Try
            niBTSG.RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, "")
            ' ignoring the exception here - the RFSGClearDatabase API throws an exception
            ' if called more than once on the same waveformName
         Catch
         End Try
      End If

      ' Deactivate 'Stop' button
      stopButton.Enabled = False

      ' Activate 'Start' button
      generateButton.Enabled = True
   End Sub

   Private Sub CloseSession()
      ' Close the RFSG session
      If rfsgSession IsNot Nothing Then
         rfsgSession.Abort()
         niBTSG.RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, "")
         rfsgSession.Dispose()
         rfsgSession = Nothing
      End If

      ' Close the BT Session
      If btsgSession IsNot Nothing Then
         btsgSession.CloseSession()
         btsgSession = Nothing
      End If
   End Sub

   Private Sub ShowError(functionName As String, exception As System.Exception)
      StopGeneration()
      CloseSession()

      ' Display error to the user
      Dim exceptionMessage As String = If(String.IsNullOrEmpty(exception.Message), "Undefined Error.", exception.Message)
      errorTextBox.Text = "Error in " & functionName & System.Environment.NewLine & exceptionMessage
      MessageBox.Show(exceptionMessage, functionName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
   End Sub

   Private Sub generateButton_Click(sender As Object, e As System.EventArgs)
      Try
         'Deactivate the 'Start' button
         generateButton.Enabled = False
         'Activate the 'Stop' button
         stopButton.Enabled = True
         'Reset the error message text box
         errorTextBox.Text = "No Error"

         'Read the Panel control values
         resourceName = DirectCast(rfsgResourceTextBox.Text, String)
         referenceClockSource = CType(refSourceComboBox.SelectedValue, RfsgFrequencyReferenceSource)
         itemIndex = CInt(userDefinedBitsComboBox.SelectedValue)
         exportClock = CType(clkOutTerminalComboBox.SelectedValue, RfsgOutputTerminal)
         packetType = CInt(packetComboBox.SelectedValue)
         packetHeaderArqn = CInt(pktHdrArqnComboBox.SelectedValue)
         packetHeaderSeqn = CInt(Math.Truncate(pktHdrSeqnNumeric.Value))
         payloadLengthMode = CInt(payHdrPaylenModeComboBox.SelectedValue)
         payloadLength = CInt(Math.Truncate(payHdrPaylenNumeric.Value))
         dataType = CInt(payHdrDatatypeComboBox.SelectedValue)
         carrierFrequency = Double.Parse(carrierFreqTextBox.Text)
         externalAttenuation = CDbl(externalAttnNumeric.Value)
         headroom = CDbl(headroomNumeric.Value)
         quadratureSkew = CDbl(quadratureSkewNumeric.Value)
         iDCOffset = CDbl(iDCOffsetNumeric.Value)
         qDCOffset = CDbl(qDCOffsetNumeric.Value)
         iQGainImbalance = CDbl(iqGaimbalanceNumeric.Value)
         carrierFrequencyOffset = CDbl(carrierFreqOffNumeric.Value)
         carrierToNoiseRatio = CDbl(cnrNumeric.Value)
         seed = CInt(Math.Truncate(paydatSeedNumeric.Value))
         channelNumber = CInt(Math.Truncate(chnNumberNumeric.Value))
         powerLevel = CDbl(powerLevelNumeric.Value)
         bdAddressLap = CInt(Math.Truncate(bdaddrLapNumeric.Value))
         bdAddressUap = CInt(Math.Truncate(bdaddrUapNumeric.Value))
         bdAddressNap = CInt(Math.Truncate(bdaddrNapNumeric.Value))
         HeaderLtAddress = CInt(Math.Truncate(pktLtAddrNumeric.Value))
         headerPayloadLlid = CInt(Math.Truncate(payHdrLlidNumeric.Value))
         headerFlow = CInt(Math.Truncate(pktHdrFlowNumeric.Value))
         headerSeqn = CInt(Math.Truncate(pktHdrSeqnNumeric.Value))
         dataPNOrder = CInt(Math.Truncate(paydatPnorderNumeric.Value))
         autoHeadroomEnabled = CInt(autoHeadroomEnabComboBox.SelectedValue)
         awgnEnabled = CInt(awgnEnabledComboBox.SelectedValue)
         allIqImpairmentsEnabled = CInt(allIqImpairEnComboBox.SelectedValue)
         actualHeadroom = System.Convert.ToDouble(actualHeadroomTextBox.Text)
         uniqPkts = CInt(Math.Truncate(numOfUniqNumeric.Value))
         idleSlots = CInt(Math.Truncate(numOfIdleSlotsNumeric.Value))
         whiteningEnabled = CInt(whiteEnComboBox.SelectedValue)
         whiteningclock = CInt(Math.Truncate(whiteClkNumeric.Value))
         dataRate = CInt(Math.Truncate(dataRateNumeric.Value))
         LETPPayloadType = CInt(LETPPayloadTypeComboBox.SelectedValue)
         LETPCorruptAlternateCRC = CInt(LETPCorruptAlternateCRCComboBox.SelectedValue)
         zadoffChuIndex = CInt(Math.Truncate(zadoffChuIndexNumeric.Value))
         physicalChannelAddress = CInt(Math.Truncate(physicalChannelAddressNumeric.Value))
         hdtPacketFormat = CInt(HdtPacketFormatComboBox.SelectedValue)
         hdtPhyInterval = CDbl(Math.Truncate(HdtPhyIntervalNumeric.Value))
         dirtyTxSettingsMode = CInt(dirtyTxModeComboBox.SelectedValue)
         dirtyTxModulationIndexType = CInt(dirtyTxModulationIndexTypeComboBox.SelectedValue)

         iOffset = CDbl(iOffsetNumeric.Value)
         qOffset = CDbl(qOffsetNumeric.Value)
         iCommonModeOffset = CDbl(iCommonModeOffsetNumeric.Value)
         qCommonModeOffset = CDbl(qCommonModeOffsetNumeric.Value)
         outputPort = CInt(outputPortComboBox.SelectedValue)
         terminalConfiguration = CInt(terminalConfigurationComboBox.SelectedValue)
         ' Open the BT Generation Session
         StartGeneration()

         ' Start the status checking timer
         timer.Enabled = True
      Catch exception As System.Exception
         ShowError("StartGeneration()", exception)
      End Try
   End Sub

   Private Sub stopButton_Click(sender As Object, e As System.EventArgs)
      If rfsgSession IsNot Nothing Then
         rfsgSession.Abort()
      End If
      StopGeneration()
   End Sub

   Private Sub paraEnabledInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles paraEnabledInsert.Click
      Dim IndexNum As Integer = paraEnabledDataGridView.Rows.Add()
      paraEnabledDataGridView.Rows(IndexNum).Cells("parametersEnabledSet").Value = "0"
      paraEnabledDataGridView.Rows(IndexNum).Cells("parametersEnabledSetIndex").Value = IndexNum.ToString()
   End Sub

    Private Sub paraEnabledDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles paraEnabledDelete.Click
        If paraEnabledDataGridView.Rows.Count > 0 Then
            Dim IndexNum As Integer = paraEnabledDataGridView.CurrentCell.RowIndex
            paraEnabledDataGridView.Rows.RemoveAt(IndexNum)

            For i As Integer = IndexNum To paraEnabledDataGridView.Rows.Count - 1
                paraEnabledDataGridView.Rows(i).Cells("parametersEnabledSetIndex").Value = (i).ToString()
            Next
        End If
    End Sub

   Private Sub carrFreqOffInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles carrFreqOffInsert.Click
      Dim IndexNum As Integer = carrFreqOffsetDataGridView.Rows.Add()
      carrFreqOffsetDataGridView.Rows(IndexNum).Cells("carrFreqOffsetSet").Value = "0"
      carrFreqOffsetDataGridView.Rows(IndexNum).Cells("carrFreqOffsetIndex").Value = IndexNum.ToString()
   End Sub

    Private Sub CarrFreqOffDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles carrFreqOffDelete.Click
        If carrFreqOffsetDataGridView.Rows.Count > 0 Then
            Dim IndexNum As Integer = carrFreqOffsetDataGridView.CurrentCell.RowIndex
            carrFreqOffsetDataGridView.Rows.RemoveAt(IndexNum)

            For i As Integer = IndexNum To carrFreqOffsetDataGridView.Rows.Count - 1
                carrFreqOffsetDataGridView.Rows(i).Cells("carrFreqOffsetIndex").Value = (i).ToString()
            Next
        End If
    End Sub

   Private Sub modulationIndexInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles modulationIndexInsert.Click
      Dim IndexNum As Integer = modulationIndexDataGridView.Rows.Add()
      modulationIndexDataGridView.Rows(IndexNum).Cells("modulationIndexSet").Value = "0"
      modulationIndexDataGridView.Rows(IndexNum).Cells("modulationIndex").Value = IndexNum.ToString()
   End Sub

    Private Sub modulationIndexDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles modulationIndexDelete.Click
        If modulationIndexDataGridView.Rows.Count > 0 Then
            Dim IndexNum As Integer = modulationIndexDataGridView.CurrentCell.RowIndex
            modulationIndexDataGridView.Rows.RemoveAt(IndexNum)

            For i As Integer = IndexNum To modulationIndexDataGridView.Rows.Count - 1
                modulationIndexDataGridView.Rows(i).Cells("modulationIndex").Value = (i).ToString()
            Next
        End If
    End Sub

   Private Sub symbolTimingErrorInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles symbolTimingErrorInsert.Click
      Dim IndexNum As Integer = symbolTimingErrorDataGridView.Rows.Add()
      symbolTimingErrorDataGridView.Rows(IndexNum).Cells("symbolTimingErrorSet").Value = "0"
      symbolTimingErrorDataGridView.Rows(IndexNum).Cells("symbolTimingErrorIndex").Value = IndexNum.ToString()
   End Sub

    Private Sub symbolTimingErrorDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles symbolTimingErrorDelete.Click
        If symbolTimingErrorDataGridView.Rows.Count > 0 Then
            Dim IndexNum As Integer = symbolTimingErrorDataGridView.CurrentCell.RowIndex
            symbolTimingErrorDataGridView.Rows.RemoveAt(IndexNum)

            For i As Integer = IndexNum To symbolTimingErrorDataGridView.Rows.Count - 1
                symbolTimingErrorDataGridView.Rows(i).Cells("modulationIndex").Value = (i).ToString()
            Next
        End If
    End Sub


   ' --------------------------------------------------------------------------
   ' Callback function when the MainForm is closed
   ' --------------------------------------------------------------------------
   Private Sub MainFormClosing(sender As Object, e As FormClosingEventArgs)
      StopGeneration()
      CloseSession()
   End Sub
End Class
