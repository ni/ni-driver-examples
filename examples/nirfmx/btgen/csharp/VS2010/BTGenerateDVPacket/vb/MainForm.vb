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

   Private resourceName As String, referenceClockSource As String, exportClock As String, waveformName As String, script As String

   Private powerLevel As Double, carrierFrequency As Double, upConverterCenterFrequency As Double, upConverterCenterFrequencyOffset As Double, actualHeadroom As Double, externalAttenuation As Double,
      headroom As Double, quadratureSkew As Double, iDcOffset As Double, qDcOffset As Double, iQGainImbalance As Double, carrierFrequencyOffset As Double,
      carrierToNoiseRatio As Double, iCommonModeOffset As Double, qCommonModeOffset As Double, iOffset As Double, qOffset As Double

   Private channelNumber As Integer, allIqImpairmentsEnabled As Integer, awgnEnabled As Integer, whiteningEnabled As Integer, autoHeadroomEnabled As Integer, packetHeaderArqn As Integer,
      packetHeaderSeqn As Integer, actualPayloadLength As Integer, dataType As Integer, bdAddressLap As Integer, bdAddressUap As Integer, payloadLengthMode As Integer,
      payloadLength As Integer, bdAddressNap As Integer, HeaderLtAddress As Integer, headerFlow As Integer, headerPayloadLlid As Integer, dataseed As Integer,
      headerSeqn As Integer, dataPNOrder As Integer, whiteningclock As Integer, outputPort As Integer, terminalConfiguration As Integer
   Private dvVoicePayloadDataType As Integer, dvVoicePayloadPnOrder As Integer, dvVoicePayloadDataPnSeed As Integer
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
      ConfigurePayhdrPaylenModeComboBox()
      ConfigurePayhdrDatatypeComboBox()
      ConfigureDvPayhdrDatatypeComboBox()
      ConfigurePkthdrArqnComboBox()
      ConfigureWhiteEnComboBox()
      ConfigurestopButton()
      ConfigureOutputPortComboBox()
      ConfigureTerminalConfigurationComboBox()
   End Sub

#Region "UI Initial Value Config Section"
   Private Sub ConfigurestopButton()
      'Deactivate the 'Stop' button
      stopButton.Enabled = False
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
      bdaddrNapNumeric.Maximum = 3
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

      dvPaydatPnorderNumeric.Minimum = Integer.MinValue
      dvPaydatPnorderNumeric.Maximum = Integer.MaxValue
      dvPaydatPnorderNumeric.Increment = 1
      dvPaydatPnorderNumeric.Value = 9
      dvPaydatPnorderNumeric.DecimalPlaces = 0

      dvPaydatSeedNumeric.Minimum = Integer.MinValue
      dvPaydatSeedNumeric.Maximum = Integer.MaxValue
      dvPaydatSeedNumeric.Increment = 1
      dvPaydatSeedNumeric.Value = 497
      dvPaydatSeedNumeric.DecimalPlaces = 0

      whiteClkNumeric.Minimum = Integer.MinValue
      whiteClkNumeric.Maximum = Integer.MaxValue
      whiteClkNumeric.Increment = 1
      whiteClkNumeric.Value = 0
      whiteClkNumeric.DecimalPlaces = 0
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

   Private Sub ConfigurePayhdrPaylenModeComboBox()
      Dim payHdrPaylenModeValueList As New List(Of DictionaryEntry)()
      payHdrPaylenModeValueList.Add(New DictionaryEntry("Maximum Length", niBTSGConstants.PayloadLengthModeMaximumLength))
      payHdrPaylenModeValueList.Add(New DictionaryEntry("User Defined", niBTSGConstants.PayloadLengthModeUserDefined))
      payHdrPaylenModeComboBox.DataSource = payHdrPaylenModeValueList
      payHdrPaylenModeComboBox.DisplayMember = "Key"
      payHdrPaylenModeComboBox.ValueMember = "Value"
      payHdrPaylenModeComboBox.SelectedIndex = 0
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

   Private Sub ConfigureDvPayhdrDatatypeComboBox()
      Dim dvPayhdrDatatypeValueList As New List(Of DictionaryEntry)()
      dvPayhdrDatatypeValueList.Add(New DictionaryEntry("PN Sequence", niBTSGConstants.PayloadDataTypePnSequence))
      dvPayhdrDatatypeValueList.Add(New DictionaryEntry("User Defined", niBTSGConstants.PayloadDataTypeUserDefinedBits))
      dvPayhdrDatatypeComboBox.DataSource = dvPayhdrDatatypeValueList
      dvPayhdrDatatypeComboBox.DisplayMember = "Key"
      dvPayhdrDatatypeComboBox.ValueMember = "Value"
      dvPayhdrDatatypeComboBox.SelectedIndex = 0
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

   Private Sub ConfigureWhiteEnComboBox()
      Dim whiteEnValueList As New List(Of DictionaryEntry)()
      whiteEnComboBox.DataSource = GetTrueFalseValueList()
      whiteEnComboBox.DisplayMember = "Key"
      whiteEnComboBox.ValueMember = "Value"
      whiteEnComboBox.SelectedIndex = 0
   End Sub

   Private Function GetTrueFalseValueList() As List(Of DictionaryEntry)
      Dim trueFalseValueList As New List(Of DictionaryEntry)()
      trueFalseValueList.Add(New DictionaryEntry("False", niBTSGConstants.[False]))
      trueFalseValueList.Add(New DictionaryEntry("True", niBTSGConstants.[True]))
      Return trueFalseValueList
   End Function
#End Region

   Private Sub ShowError(functionName As String, exception As System.Exception)
      StopGeneration()
      CloseSession()

      ' Display error to the user
      Dim exceptionMessage As String = If(String.IsNullOrEmpty(exception.Message), "Undefined Error.", exception.Message)
      errorTextBox.Text = "Error in " & functionName & System.Environment.NewLine & exceptionMessage
      MessageBox.Show(exceptionMessage, functionName, MessageBoxButtons.OK, MessageBoxIcon.[Error])

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
            niBTSG.RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, "idle")
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
   ' --------------------------------------------------------------------------
   ' Callback function when the MainForm is closed
   ' --------------------------------------------------------------------------
   Private Sub MainFormClosing(sender As Object, e As FormClosingEventArgs)
      StopGeneration()
      CloseSession()
   End Sub
   Private Sub ProcessTimerEvent(sender As Object, e As System.EventArgs)
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

   Private Sub StartGeneration()
      'Bluetooth Session

      If btsgSession Is Nothing Then
         btsgSession = New niBTSG(niBTSGConstants.ToolkitCompatibilityVersion020000)
      End If
      niBTSG.ChannelNumberToCarrierFrequency(channelNumber, niBTSGConstants.StandardBasicEDR, carrierFrequency)
      carrierFreqTextBox.Text = carrierFrequency.ToString()

      'Set BD Address
      btsgSession.SetBDAddress(Nothing, bdAddressLap, bdAddressUap, bdAddressNap)
      btsgSession.SetPacketType(Nothing, niBTSGConstants.PacketTypeDv)

      'Set Packet Header
      btsgSession.SetPacketHeaderLTAddress(Nothing, HeaderLtAddress)
      btsgSession.SetPacketHeaderFlow(Nothing, headerFlow)
      btsgSession.SetPacketHeaderArqn(Nothing, packetHeaderArqn)
      btsgSession.SetPacketHeaderSeqn(Nothing, packetHeaderSeqn)

      'Set Payload Header
      btsgSession.SetPayloadHeaderLlid(Nothing, headerPayloadLlid)
      btsgSession.SetPayloadHeaderFlow(Nothing, headerFlow)
      btsgSession.SetPayloadLengthMode(Nothing, payloadLengthMode)
      If payloadLengthMode <> 0 Then
         btsgSession.SetPayloadLength(Nothing, payloadLength)
      End If
      btsgSession.GetActualPayloadLength(Nothing, actualPayloadLength)
      payHdrActPaylenTextBox.Text = actualPayloadLength.ToString()

      'Set Payload Data
      btsgSession.SetPayloadDataType(Nothing, dataType)
      btsgSession.SetPayloadPNOrder(Nothing, dataPNOrder)
      btsgSession.SetPayloadPNSeed(Nothing, dataseed)

      'Set Voice Data
      btsgSession.SetDVVoicePayloadDataType(Nothing, dvVoicePayloadDataType)
      btsgSession.SetDVVoicePayloadPNOrder(Nothing, dvVoicePayloadPnOrder)
      btsgSession.SetDVVoicePayloadPNSeed(Nothing, dvVoicePayloadDataPnSeed)

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

      'Whitening Properties
      btsgSession.SetWhiteningEnabled(Nothing, whiteningEnabled)
      btsgSession.SetWhiteningClock(Nothing, whiteningclock)

      'RFSG Session
      If rfsgSession Is Nothing Then
         rfsgSession = New NIRfsg(resourceName, False, True)
      End If

      'RFSG Properties

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
      'Create & Download Waveform

      btsgSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", waveformName)
      btsgSession.GetActualHeadroom(Nothing, actualHeadroom)
      actualHeadroomTextBox.Text = actualHeadroom.ToString()
      'Create and Download Waveform (IDLE)

      btsgSession.SetPacketType(Nothing, niBTSGConstants.PacketTypeIdle)
      btsgSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", "idle")
      'Execute Script

      niBTSG.RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, script, powerLevel)
      rfsgSession.RF.OutputEnabled = True

      rfsgSession.Initiate()

      'Start the status checking timer 
      timer.Enabled = True
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
         exportClock = CType(clkOutTerminalComboBox.SelectedValue, RfsgOutputTerminal)
         packetHeaderArqn = CInt(pktHdrArqnComboBox.SelectedValue)
         packetHeaderSeqn = CInt(Math.Truncate(pktHdrSeqnNumeric.Value))
         payloadLengthMode = CInt(payHdrPaylenModeComboBox.SelectedValue)
         payloadLength = CInt(Math.Truncate(payHdrPaylenNumeric.Value))
         dataType = CInt(payHdrDatatypeComboBox.SelectedValue)
         dvVoicePayloadDataType = CInt(dvPayhdrDatatypeComboBox.SelectedValue)
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
         dataseed = CInt(Math.Truncate(paydatSeedNumeric.Value))
         dvVoicePayloadDataPnSeed = CInt(Math.Truncate(dvPaydatSeedNumeric.Value))
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
         dvVoicePayloadPnOrder = CInt(Math.Truncate(dvPaydatPnorderNumeric.Value))
         autoHeadroomEnabled = CInt(autoHeadroomEnabComboBox.SelectedValue)
         awgnEnabled = CInt(awgnEnabledComboBox.SelectedValue)
         whiteningEnabled = CInt(whiteEnComboBox.SelectedValue)
         allIqImpairmentsEnabled = CInt(allIqImpairEnComboBox.SelectedValue)
         whiteningclock = CInt(Math.Truncate(whiteClkNumeric.Value))
         actualHeadroom = System.Convert.ToDouble(actualHeadroomTextBox.Text)
         iOffset = CDbl(iOffsetNumeric.Value)
         qOffset = CDbl(qOffsetNumeric.Value)
         iCommonModeOffset = CDbl(iCommonModeOffsetNumeric.Value)
         qCommonModeOffset = CDbl(qCommonModeOffsetNumeric.Value)
         outputPort = CInt(outputPortComboBox.SelectedValue)
         terminalConfiguration = CInt(terminalConfigurationComboBox.SelectedValue)
         '  BT Generation 
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
End Class
