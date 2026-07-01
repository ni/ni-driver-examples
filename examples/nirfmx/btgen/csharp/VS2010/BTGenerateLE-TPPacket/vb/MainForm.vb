Imports System
Imports System.Collections.Generic
Imports System.Collections
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.RFToolkits.BT.Generation
Imports System.Text.RegularExpressions

Partial Public Class MainForm
   Inherits Form
   Private rfsgSession As NIRfsg
   Private btsgSession As niBTSG

   Private script As String
   Private resourceName As String, clkOutTerm As String, userDefinedBits As String
   Private referenceClockSource As String, exportClock As String, waveformName As String
   Private model As String, antennaSwitchingPattern As String
   Private iDcOffset As Double, qDcOffset As Double, iQGainImbalance As Double, carrierFrequencyOffset As Double, carrierToNoiseRatio As Double, powerLevel As Double,
    quadratureSkew As Double, headroom As Double, externalAttenuation As Double, carrierFrequency As Double, actualHeadroom As Double, upConverterCenterFrequency As Double,
    upConverterCenterFrequencyOffset As Double
   Private cteLength As Double, cteSlotDuration As Double, antennaSwitchingDuration As Double, antennaSwitchingDurationUsed As Double
   Private userDefBitsArray As Integer()
   Private relativePhaseArray As Double(), relativeAmplitudeArray As Double()
   Private channelNumber As Integer, allIqImpairmentsEnabled As Integer, awgnEnabled As Integer, autoHeadroomEnabled As Integer, payloadLengthMode As Integer, payloadLength As Integer,
    payloadType As Integer, dirtyTxEnabld As Integer, outputPort As Integer, terminalConfiguration As Integer, packetType As Integer, letpCorruptAlternateCrc As Integer
   Private numberOfUniquePackets As Integer, directionFindingMode As Integer, antennaSwitchingEnabled As Integer, numberOfAntennas As Integer, oversamplingFactor As Integer
   Private generationStatus As RfsgGenerationStatus


   Public Sub New()
      InitializeComponent()
      ConfigureNumericUpDown()
      ConfigureAutoheadroomEnabComboBox()
      ConfigureRefSourceComboBox()
      ConfigureClkOutTerminalComboBox()
      ConfigureAllIqImpairEnComboBox()
      ConfigureAwgnEnabledComboBox()
      ConfigureLETPPayloadType()
      ConfigurePayhdrPaylenModeComboBox()
      ConfigureDirtyTxComboBox()
      ConfigureOutputPortComboBox()
      ConfigureTerminalConfigurationComboBox()
      ConfigurePacketTypeComboBox()
      ConfigureLETPCorruptAlternateCRCComboBox()
      ConfigureDirectionFindingModeComboBox()
      ConfigureCTESlotDurationComboBox()
      ConfigureAntennaSwitchingEnabledComboBox()
   End Sub

#Region "UI Initial Value Config Section"
   Private Sub ConfigureNumericUpDown()
      chnNumberNumeric.Minimum = 0
      chnNumberNumeric.Maximum = 79
      chnNumberNumeric.Increment = 1
      chnNumberNumeric.Value = 3
      chnNumberNumeric.DecimalPlaces = 0

      powerLevelNumeric.Minimum = -179
      powerLevelNumeric.Maximum = 25
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

      iDcOffsetNumeric.Minimum = Decimal.MinValue
      iDcOffsetNumeric.Maximum = Decimal.MaxValue
      iDcOffsetNumeric.Increment = 0.5D
      iDcOffsetNumeric.Value = 0
      iDcOffsetNumeric.DecimalPlaces = 2

      qDcOffsetNumeric.Minimum = Decimal.MinValue
      qDcOffsetNumeric.Maximum = Decimal.MaxValue
      qDcOffsetNumeric.Increment = 0.5D
      qDcOffsetNumeric.Value = 0
      qDcOffsetNumeric.DecimalPlaces = 2

      iqGaimbalanceNumeric.Minimum = Decimal.MinValue
      iqGaimbalanceNumeric.Maximum = Decimal.MaxValue
      iqGaimbalanceNumeric.Increment = 0.5D
      iqGaimbalanceNumeric.Value = 0
      iqGaimbalanceNumeric.DecimalPlaces = 2

      carrierFreqOffNumeric.Minimum = Decimal.MinValue
      carrierFreqOffNumeric.Maximum = Decimal.MaxValue
      carrierFreqOffNumeric.Increment = 1D
      carrierFreqOffNumeric.Value = 0
      carrierFreqOffNumeric.DecimalPlaces = 3

      cnrNumeric.Minimum = Decimal.MinValue
      cnrNumeric.Maximum = Decimal.MaxValue
      cnrNumeric.Increment = 0.5D
      cnrNumeric.Value = 50
      cnrNumeric.DecimalPlaces = 2

      payHdrPaylenNumeric.Minimum = Integer.MinValue
      payHdrPaylenNumeric.Maximum = Integer.MaxValue
      payHdrPaylenNumeric.Increment = 1
      payHdrPaylenNumeric.Value = 0
      payHdrPaylenNumeric.DecimalPlaces = 0
   End Sub

   Private Sub ConfigureAutoheadroomEnabComboBox()
      Dim autoheadroomEnabValueList As New List(Of DictionaryEntry)()
      autoheadroomEnabComboBox.DataSource = GetTrueFalseValueList()
      autoheadroomEnabComboBox.DisplayMember = "Key"
      autoheadroomEnabComboBox.ValueMember = "Value"
      autoheadroomEnabComboBox.SelectedIndex = 1
   End Sub

   Private Function GetTrueFalseValueList() As List(Of DictionaryEntry)
      Dim trueFalseValueList As New List(Of DictionaryEntry)()
      trueFalseValueList.Add(New DictionaryEntry("False", niBTSGConstants.[False]))
      trueFalseValueList.Add(New DictionaryEntry("True", niBTSGConstants.[True]))
      Return trueFalseValueList
   End Function

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

   Private Sub ConfigureLETPPayloadType()
      Dim letpPayloadTypeValueList As New List(Of DictionaryEntry)()
      letpPayloadTypeValueList.Add(New DictionaryEntry("PRBS9", niBTSGConstants.LETPPayloadTypePRBS9))
      letpPayloadTypeValueList.Add(New DictionaryEntry("11110000", niBTSGConstants.LETPPayloadType11110000))
      letpPayloadTypeValueList.Add(New DictionaryEntry("10101010", niBTSGConstants.LETPPayloadType10101010))
      letpPayloadTypeValueList.Add(New DictionaryEntry("PRBS15", niBTSGConstants.LETPPayloadTypePRBS15))
      letpPayloadTypeValueList.Add(New DictionaryEntry("11111111", niBTSGConstants.LETPPayloadType11111111))
      letpPayloadTypeValueList.Add(New DictionaryEntry("00000000", niBTSGConstants.LETPPayloadType00000000))
      letpPayloadTypeValueList.Add(New DictionaryEntry("00001111", niBTSGConstants.LETPPayloadType00001111))
      letpPayloadTypeValueList.Add(New DictionaryEntry("01010101", niBTSGConstants.LETPPayloadType01010101))
      letpPayloadTypeValueList.Add(New DictionaryEntry("User Defined Bits", niBTSGConstants.LETPPayloadTypeUserDefinedBits))
      letpPayloadTypeComboBox.DataSource = letpPayloadTypeValueList
      letpPayloadTypeComboBox.DisplayMember = "Key"
      letpPayloadTypeComboBox.ValueMember = "Value"
      letpPayloadTypeComboBox.SelectedIndex = 0
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

   Private Sub ConfigureDirtyTxComboBox()
      Dim dirtyTxValueList As New List(Of DictionaryEntry)()
      dirtyTxComboBox.DataSource = GetTrueFalseValueList()
      dirtyTxComboBox.DisplayMember = "Key"
      dirtyTxComboBox.ValueMember = "Value"
      dirtyTxComboBox.SelectedIndex = 0
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

   Private Sub ConfigurePacketTypeComboBox()
      Dim ptList As New List(Of DictionaryEntry)()
      ptList.Add(New DictionaryEntry("LE-TP", niBTSGConstants.PacketTypeLETP))
      ptList.Add(New DictionaryEntry("LE-TP-EXT", niBTSGConstants.PacketTypeLETPExt))
      ptList.Add(New DictionaryEntry("LE-Enhanced", niBTSGConstants.PacketTypeLEEnhanced))
      ptList.Add(New DictionaryEntry("LE-LR-125k", niBTSGConstants.PacketTypeLeLr125k))
      ptList.Add(New DictionaryEntry("LE-LR-500k", niBTSGConstants.PacketTypeLeLr500k))
      packetTypeComboBox.DataSource = ptList
      packetTypeComboBox.DisplayMember = "Key"
      packetTypeComboBox.ValueMember = "Value"
      packetTypeComboBox.SelectedIndex = 0
   End Sub

   Private Sub ConfigureLETPCorruptAlternateCRCComboBox()
      letpCorruptAlternateCRCComboBox.DataSource = GetTrueFalseValueList()
      letpCorruptAlternateCRCComboBox.DisplayMember = "Key"
      letpCorruptAlternateCRCComboBox.ValueMember = "Value"
      letpCorruptAlternateCRCComboBox.SelectedIndex = 0
   End Sub

   Private Sub ConfigureDirectionFindingModeComboBox()
      Dim dfmList As New List(Of DictionaryEntry)()
      dfmList.Add(New DictionaryEntry("Disabled", niBTSGConstants.DirectionFindingModeDisabled))
      dfmList.Add(New DictionaryEntry("Angle of Arrival", niBTSGConstants.DirectionFindingModeAngleOfArrival))
      dfmList.Add(New DictionaryEntry("Angle of Departure", niBTSGConstants.DirectionFindingModeAngleOfDeparture))
      directionFindingModeComboBox.DataSource = dfmList
      directionFindingModeComboBox.DisplayMember = "Key"
      directionFindingModeComboBox.ValueMember = "Value"
      directionFindingModeComboBox.SelectedIndex = 0
   End Sub

   Private Sub ConfigureCTESlotDurationComboBox()
      Dim cteSltDur As New List(Of DictionaryEntry)()
      cteSltDur.Add(New DictionaryEntry("1 us", niBTSGConstants.CteSlotDuration1us))
      cteSltDur.Add(New DictionaryEntry("2 us", niBTSGConstants.CteSlotDuration2us))
      cteSlotDurationComboBox.DataSource = cteSltDur
      cteSlotDurationComboBox.DisplayMember = "Key"
      cteSlotDurationComboBox.ValueMember = "Value"
      cteSlotDurationComboBox.SelectedIndex = 0
   End Sub

   Private Sub ConfigureAntennaSwitchingEnabledComboBox()
      antennaSwitchingEnabledComboBox.DataSource = GetTrueFalseValueList()
      antennaSwitchingEnabledComboBox.DisplayMember = "Key"
      antennaSwitchingEnabledComboBox.ValueMember = "Value"
      antennaSwitchingEnabledComboBox.SelectedIndex = 0
   End Sub
#End Region

   Private Sub generateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles generateButton.Click
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
         exportClock = CType(clkOutTerminalComboBox.SelectedValue, RfsgOutputTerminal)

         packetType = CInt(packetTypeComboBox.SelectedValue)
         payloadLengthMode = CInt(payHdrPaylenModeComboBox.SelectedValue)
         letpCorruptAlternateCrc = CInt(letpCorruptAlternateCRCComboBox.SelectedValue)
         payloadLength = CInt(Math.Truncate(payHdrPaylenNumeric.Value))
         waveformName = DirectCast(waveNameTextBox.Text, String)
         script = DirectCast(scriptTextBox.Text, String)
         carrierFrequency = Double.Parse(carrierFreqTextBox.Text)
         externalAttenuation = CDbl(externalAttnNumeric.Value)
         headroom = CDbl(headroomNumeric.Value)
         quadratureSkew = CDbl(quadratureSkewNumeric.Value)
         iDcOffset = CDbl(iDcOffsetNumeric.Value)
         qDcOffset = CDbl(qDcOffsetNumeric.Value)
         iQGainImbalance = CDbl(iqGaimbalanceNumeric.Value)
         carrierFrequencyOffset = CDbl(carrierFreqOffNumeric.Value)
         carrierToNoiseRatio = CDbl(cnrNumeric.Value)
         channelNumber = CInt(Math.Truncate(chnNumberNumeric.Value))
         powerLevel = CDbl(powerLevelNumeric.Value)
         awgnEnabled = CInt(awgnEnabledComboBox.SelectedValue)
         allIqImpairmentsEnabled = CInt(allIqImpairEnComboBox.SelectedValue)
         actualHeadroom = System.Convert.ToDouble(actualHeadroomTextBox.Text)
         payloadType = CInt(letpPayloadTypeComboBox.SelectedValue)
         dirtyTxEnabld = CInt(dirtyTxComboBox.SelectedValue)
         clkOutTerm = CType(clkOutTerminalComboBox.SelectedValue, RfsgOutputTerminal)
         autoHeadroomEnabled = CInt(autoheadroomEnabComboBox.SelectedValue)
         outputPort = CInt(outputPortComboBox.SelectedValue)
         terminalConfiguration = CInt(terminalConfigurationComboBox.SelectedValue)
         userDefinedBits = CStr(userDefinedBitsTextBox.Text)
         numberOfUniquePackets = CInt(NumOfUniquePacketsNumeric.Value)
         directionFindingMode = CInt(directionFindingModeComboBox.SelectedValue)
         cteLength = CDbl(cteLengthNumeric.Value)
         cteSlotDuration = CDbl(cteSlotDurationComboBox.SelectedValue)
         antennaSwitchingEnabled = CInt(antennaSwitchingEnabledComboBox.SelectedValue)
         numberOfAntennas = CInt(numberOfAntennasNumeric.Value)
         antennaSwitchingPattern = CStr(antennaSwitchingPatternTextBox.Text)
         antennaSwitchingDuration = CDbl(AntennaSwitchingDurationNumeric.Value)
         oversamplingFactor = CInt(OversamplingFactorNumeric.Value)
         antennaSwitchingDurationUsed = System.Convert.ToDouble(AntennaSwitchingDurationUsedTextBox.Text)

         '  BT Generation 
         StartGeneration()

         ' Start the status checking timer
         timer.Enabled = True
      Catch exception As System.Exception
         ShowError("StartGeneration()", exception)
      End Try
   End Sub


   Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click

      If rfsgSession IsNot Nothing Then
         rfsgSession.Abort()
      End If
      StopGeneration()
   End Sub

   Private Sub StartGeneration()
      'Bluetooth Session

      If btsgSession Is Nothing Then
         btsgSession = New niBTSG(niBTSGConstants.ToolkitCompatibilityVersion020000)
      End If
      niBTSG.ChannelNumberToCarrierFrequency(channelNumber, niBTSGConstants.StandardLE, carrierFrequency)
      carrierFreqTextBox.Text = carrierFrequency.ToString()

      btsgSession.SetPacketType(Nothing, packetType)

      ' Set LE-TP Payload 
      btsgSession.SetLETPPayloadType(Nothing, payloadType)

      ' Set Payload Header 
      btsgSession.SetPayloadLengthMode(Nothing, payloadLengthMode)

      If payloadLengthMode = niBTSGConstants.PayloadLengthModeUserDefined Then
         btsgSession.SetPayloadLength(Nothing, payloadLength)
      End If

      btsgSession.SetLETPCorruptAlternateCrc(Nothing, letpCorruptAlternateCrc)
      btsgSession.SetDirtyTxEnabled(Nothing, dirtyTxEnabld)

      If payloadType = niBTSGConstants.LETPPayloadTypeUserDefinedBits Then
         userDefBitsArray = New Integer(userDefinedBits.Length - 1) {}

         For i As Integer = 0 To userDefinedBits.Length - 1
            userDefBitsArray(i) = CInt(Char.GetNumericValue(userDefinedBits(i)))
         Next

         btsgSession.SetPayloadUserDefinedBits(Nothing, userDefBitsArray, userDefinedBits.Length)
      End If

      btsgSession.SetNumberOfUniquePackets(Nothing, numberOfUniquePackets)
      btsgSession.SetOversamplingFactor(Nothing, oversamplingFactor)

      btsgSession.SetDirectionFindingMode(Nothing, directionFindingMode)

      If directionFindingMode <> niBTSGConstants.DirectionFindingModeDisabled Then
         btsgSession.SetDirectionFindingConstantToneExtensionLength(Nothing, cteLength)
         btsgSession.SetDirectionFindingConstantToneExtensionSlotDuration(Nothing, cteSlotDuration)
         btsgSession.SetDirectionFindingAntennaSwitchingEnabled(Nothing, antennaSwitchingEnabled)
         If antennaSwitchingEnabled = niBTSGConstants.[True] Then
            btsgSession.SetDirectionFindingNumberOfAntennas(Nothing, numberOfAntennas)
            btsgSession.SetDirectionFindingAntennaSwitchingPattern(Nothing, antennaSwitchingPattern)
            btsgSession.SetDirectionFindingAntennaSwitchingDuration(Nothing, antennaSwitchingDuration)
            Dim RelPhaseAmpArraySize As Integer = dataGridView1.Rows.Count
            relativePhaseArray = New Double(RelPhaseAmpArraySize - 1) {}
            relativeAmplitudeArray = New Double(RelPhaseAmpArraySize - 1) {}
            For rows As Integer = 0 To RelPhaseAmpArraySize - 1
               If True Then
                  relativePhaseArray(rows) = Convert.ToDouble(dataGridView1.Rows(rows).Cells(1).Value.ToString())
                  relativeAmplitudeArray(rows) = Convert.ToDouble(dataGridView1.Rows(rows).Cells(2).Value.ToString())
               End If
            Next
            btsgSession.SetAntennaRelativePhaseAndAmplitude(Nothing, relativeAmplitudeArray, relativePhaseArray, RelPhaseAmpArraySize)
         End If
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
         btsgSession.SetIDCOffset(Nothing, iDcOffset)
         btsgSession.SetQDCOffset(Nothing, qDcOffset)
         btsgSession.SetIQGainImbalance(Nothing, iQGainImbalance)
      End If
      btsgSession.SetAwgnEnabled(Nothing, awgnEnabled)
      btsgSession.SetCarrierToNoiseRatio(Nothing, carrierToNoiseRatio)

      'RFSG Session
      If rfsgSession Is Nothing Then
         rfsgSession = New NIRfsg(resourceName, False, True)
      End If


      'get model
      model = rfsgSession.Identity.InstrumentModel

      'RFSG Properties

      rfsgSession.FrequencyReference.Configure(referenceClockSource, 10000000.0)
      rfsgSession.Utility.ExportSignal(RfsgSignalType.ReferenceClock, "", exportClock)

      rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
      rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script

      If model.Equals("NI PXI-5670") OrElse model.Equals("NI PXI-5671") OrElse model.Equals("NI PXI-5672") OrElse model.Equals("NI PXI-5673") OrElse model.Equals("NI PXIe-5673E") Then
      Else
         rfsgSession.Arb.OutputPort = CType(outputPort, RfsgOutputPort)
      End If

      If outputPort = RfsgOutputPort.IQOut Then
         rfsgSession.IQOutPort("I").TerminalConfiguration = CType(terminalConfiguration, RfsgTerminalConfiguration)
      Else
         rfsgSession.RF.Frequency = carrierFrequency
         upConverterCenterFrequencyOffset = 0
         upConverterCenterFrequency = carrierFrequency + upConverterCenterFrequencyOffset
         rfsgSession.RF.Upconverter.CenterFrequency = upConverterCenterFrequency
      End If

      rfsgSession.RF.ExternalGain = -(externalAttenuation)

      'Create & Download Waveform
      btsgSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", waveformName)
      btsgSession.GetActualHeadroom(Nothing, actualHeadroom)
      actualHeadroomTextBox.Text = actualHeadroom.ToString()
      btsgSession.GetDirectionFindingAntennaSwitchingDurationUsed(Nothing, antennaSwitchingDurationUsed)
      AntennaSwitchingDurationUsedTextBox.Text = antennaSwitchingDurationUsed.ToString()

      'Execute Script
      niBTSG.RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, script, powerLevel)
      rfsgSession.RF.OutputEnabled = True
      rfsgSession.Initiate()

      'Start the status checking timer 
      timer.Enabled = True
   End Sub

   Private Sub StopGeneration()
      'Stop the status checking timer
      timer.Enabled = False
      If rfsgSession IsNot Nothing Then
         rfsgSession.Abort()
         rfsgSession.RF.OutputEnabled = False
         rfsgSession.Utility.Commit()
         Try
            niBTSG.RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, waveformName)
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

   ' --------------------------------------------------------------------------
   ' Callback function when the MainForm is closed
   ' --------------------------------------------------------------------------
   Private Sub MainFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
      StopGeneration()
      CloseSession()
   End Sub

   Private Sub ProcessTimerEvent(ByVal sender As Object, ByVal e As System.EventArgs) Handles timer.Tick
      CheckGeneration()
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

   Private Sub ShowError(functionName As String, exception As System.Exception)
      StopGeneration()
      CloseSession()

      ' Display error to the user
      Dim exceptionMessage As String = If(String.IsNullOrEmpty(exception.Message), "Undefined Error.", exception.Message)
      errorTextBox.Text = "Error in " & functionName & System.Environment.NewLine & exceptionMessage
      MessageBox.Show(exceptionMessage, functionName, MessageBoxButtons.OK, MessageBoxIcon.[Error])
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

   Private Sub insertRowButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles insertRowButton.Click
      Dim IndexNum As Integer = dataGridView1.Rows.Add()
      dataGridView1.Rows(IndexNum).Cells("RelativePhase").Value = "0.00"
      dataGridView1.Rows(IndexNum).Cells("RelativeAmplitude").Value = "0.00"
      dataGridView1.Rows(IndexNum).Cells("Index").Value = IndexNum.ToString()
   End Sub

   Private Sub deleteRowButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles deleteRowButton.Click
      If dataGridView1.Rows.Count > 0 Then
         Dim IndexNum As Integer = dataGridView1.CurrentCell.RowIndex
         dataGridView1.Rows.RemoveAt(IndexNum)

         For i As Integer = IndexNum To dataGridView1.Rows.Count - 1
            dataGridView1.Rows(i).Cells("Index").Value = (i).ToString()
         Next
      End If
   End Sub

   Private Sub userDefinedBitsTextBox_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles userDefinedBitsTextBox.KeyPress 'Handles userDefinedBitsTextBox.TextChanged
      Dim regex As New Regex("[^0-1\b]")
      If regex.IsMatch(e.KeyChar) Then
         e.Handled = True
      End If
   End Sub

End Class
