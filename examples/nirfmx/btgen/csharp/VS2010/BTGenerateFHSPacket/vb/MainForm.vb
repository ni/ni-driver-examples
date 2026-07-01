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
      packetHeaderSeqn As Integer, bdAddressLap As Integer, bdAddressUap As Integer, bdAddressNap As Integer, packetType As Integer = niBTSGConstants.PacketTypeIdle, HeaderLtAddress As Integer,
      headerFlow As Integer, headerSeqn As Integer, whiteningclock As Integer, outputPort As Integer, terminalConfiguration As Integer, standard As Integer

   Private fhsBdAddressLap As Integer, fhsBdAddressUap As Integer, fhsBdAddressNap As Integer, fhsPayloadLTAddress As Integer, fhsPayloadDeviceClass As Integer, fhsPayloadScanRepetition As Integer,
      fhsPayloadPageScanMode As Integer, fhsPayloadDeviceClock As Integer
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

      fhsBdaddrLapNumeric.Minimum = Integer.MinValue
      fhsBdaddrLapNumeric.Maximum = Integer.MaxValue
      fhsBdaddrLapNumeric.Increment = 1
      fhsBdaddrLapNumeric.Value = 0
      fhsBdaddrLapNumeric.DecimalPlaces = 0

      fhsBdaddrUapNumeric.Minimum = Integer.MinValue
      fhsBdaddrUapNumeric.Maximum = Integer.MaxValue
      fhsBdaddrUapNumeric.Increment = 1
      fhsBdaddrUapNumeric.Value = 0
      fhsBdaddrUapNumeric.DecimalPlaces = 0

      fhsBdaddrNapNumeric.Minimum = 0
      fhsBdaddrNapNumeric.Maximum = 30
      fhsBdaddrNapNumeric.Increment = 0
      fhsBdaddrNapNumeric.Value = 0
      fhsBdaddrNapNumeric.DecimalPlaces = 0

      fhsLtAddrNumeric.Minimum = Integer.MinValue
      fhsLtAddrNumeric.Maximum = Integer.MaxValue
      fhsLtAddrNumeric.Increment = 1
      fhsLtAddrNumeric.Value = 0
      fhsLtAddrNumeric.DecimalPlaces = 0

      fhsDevClassNumeric.Minimum = Integer.MinValue
      fhsDevClassNumeric.Maximum = Integer.MaxValue
      fhsDevClassNumeric.Increment = 1
      fhsDevClassNumeric.Value = 0
      fhsDevClassNumeric.DecimalPlaces = 0

      fhsScanRepNumeric.Minimum = Integer.MinValue
      fhsScanRepNumeric.Maximum = Integer.MaxValue
      fhsScanRepNumeric.Increment = 1
      fhsScanRepNumeric.Value = 0
      fhsScanRepNumeric.DecimalPlaces = 0

      fhsPgScanNumeric.Minimum = Integer.MinValue
      fhsPgScanNumeric.Maximum = Integer.MaxValue
      fhsPgScanNumeric.Increment = 1
      fhsPgScanNumeric.Value = 0
      fhsPgScanNumeric.DecimalPlaces = 0

      fhsDevClockNumeric.Minimum = Integer.MinValue
      fhsDevClockNumeric.Maximum = Integer.MaxValue
      fhsDevClockNumeric.Increment = 1
      fhsDevClockNumeric.Value = 0
      fhsDevClockNumeric.DecimalPlaces = 0

      whiteClkNumeric.Minimum = Integer.MinValue
      whiteClkNumeric.Maximum = Integer.MaxValue
      whiteClkNumeric.Increment = 1
      whiteClkNumeric.Value = 0
      whiteClkNumeric.DecimalPlaces = 0
   End Sub

   Private Sub ConfigureAutoheadroomEnabComboBox()
      Dim autoheadroomEnabValueList As New List(Of DictionaryEntry)()
      autoheadroomEnabComboBox.DataSource = GetTrueFalseValueList()
      autoheadroomEnabComboBox.DisplayMember = "Key"
      autoheadroomEnabComboBox.ValueMember = "Value"
      autoheadroomEnabComboBox.SelectedIndex = 1
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

   Private Sub ConfigureWhiteEnComboBox()
      Dim whiteEnValueList As New List(Of DictionaryEntry)()
      whiteEnComboBox.DataSource = GetTrueFalseValueList()
      whiteEnComboBox.DisplayMember = "Key"
      whiteEnComboBox.ValueMember = "Value"
      whiteEnComboBox.SelectedIndex = 0
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

   Private Function GetTrueFalseValueList() As List(Of DictionaryEntry)
      Dim trueFalseValueList As New List(Of DictionaryEntry)()
      trueFalseValueList.Add(New DictionaryEntry("False", niBTSGConstants.[False]))
      trueFalseValueList.Add(New DictionaryEntry("True", niBTSGConstants.[True]))
      Return trueFalseValueList
   End Function
#End Region

   Private Sub StartGeneration()
      'BTSG Session

      If btsgSession Is Nothing Then
         btsgSession = New niBTSG(niBTSGConstants.ToolkitCompatibilityVersion020000)
      End If
      'niBTSG.ChannelNumberToCarrierFrequency(channelNumber, niBTSGConstants.StandardBasicEdr, out carrierFrequency);

      'Set BD Address
      btsgSession.SetBDAddress(Nothing, bdAddressLap, bdAddressUap, bdAddressNap)
      btsgSession.SetPacketType(Nothing, niBTSGConstants.PacketTypeFhs)

      'Set Carrier Mode
      btsgSession.SetCarrierMode(Nothing, niBTSGConstants.CarrierModeBurst)

      'Set Packet Header
      btsgSession.SetPacketHeaderLTAddress(Nothing, HeaderLtAddress)
      btsgSession.SetPacketHeaderFlow(Nothing, headerFlow)
      btsgSession.SetPacketHeaderArqn(Nothing, packetHeaderArqn)
      btsgSession.SetPacketHeaderSeqn(Nothing, packetHeaderSeqn)

      'Set FHS Payload
      btsgSession.SetFhsPayloadBDAddress(Nothing, fhsBdAddressLap, fhsBdAddressUap, fhsBdAddressNap)
      btsgSession.SetFhsPayloadLTAddress(Nothing, fhsPayloadLTAddress)
      btsgSession.SetFhsPayloadDeviceClass(Nothing, fhsPayloadDeviceClass)
      btsgSession.SetFhsPayloadScanRepetition(Nothing, fhsPayloadScanRepetition)
      btsgSession.SetFhsPayloadPageScanMode(Nothing, fhsPayloadPageScanMode)
      btsgSession.SetFhsPayloadDeviceClock(Nothing, fhsPayloadDeviceClock)

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
      btsgSession.SetCarrierFrequencyOffset(Nothing, 0)
      btsgSession.SetAwgnEnabled(Nothing, awgnEnabled)
      btsgSession.SetCarrierToNoiseRatio(Nothing, carrierToNoiseRatio)

      'Whitening Properties
      btsgSession.SetWhiteningEnabled(Nothing, whiteningEnabled)
      btsgSession.SetWhiteningClock(Nothing, whiteningclock)
      niBTSG.ChannelNumberToCarrierFrequency(channelNumber, standard, carrierFrequency)
      carrierFreqTextBox.Text = carrierFrequency.ToString()
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
      btsgSession.GetDouble(Nothing, niBTSGProperties.ActualHeadroom, actualHeadroom)
      actualHeadroomTextBox.Text = actualHeadroom.ToString()

      'Create and Download Waveform (IDLE)
      btsgSession.SetPacketType("", packetType)
      btsgSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", "idle")

      'Execute Script
      niBTSG.RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, script, powerLevel)
      rfsgSession.RF.OutputEnabled = True
      rfsgSession.Initiate()

      'Start the status checking timer 
      timer.Enabled = True
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

   Private Sub MainFormClosing(sender As Object, e As FormClosingEventArgs)
      StopGeneration()
      CloseSession()
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
         fhsBdAddressLap = CInt(Math.Truncate(fhsBdaddrLapNumeric.Value))
         fhsBdAddressUap = CInt(Math.Truncate(fhsBdaddrUapNumeric.Value))
         fhsBdAddressNap = CInt(Math.Truncate(fhsBdaddrNapNumeric.Value))
         fhsPayloadLTAddress = CInt(Math.Truncate(fhsLtAddrNumeric.Value))
         fhsPayloadDeviceClass = CInt(Math.Truncate(fhsDevClassNumeric.Value))
         fhsPayloadScanRepetition = CInt(Math.Truncate(fhsScanRepNumeric.Value))
         fhsPayloadPageScanMode = CInt(Math.Truncate(fhsPgScanNumeric.Value))
         fhsPayloadDeviceClock = CInt(Math.Truncate(fhsDevClockNumeric.Value))
         channelNumber = CInt(Math.Truncate(chnNumberNumeric.Value))
         powerLevel = CDbl(powerLevelNumeric.Value)
         bdAddressLap = CInt(Math.Truncate(bdaddrLapNumeric.Value))
         bdAddressUap = CInt(Math.Truncate(bdaddrUapNumeric.Value))
         bdAddressNap = CInt(Math.Truncate(bdaddrNapNumeric.Value))
         HeaderLtAddress = CInt(Math.Truncate(pktLtAddrNumeric.Value))
         headerFlow = CInt(Math.Truncate(pktHdrFlowNumeric.Value))
         headerSeqn = CInt(Math.Truncate(pktHdrSeqnNumeric.Value))
         autoHeadroomEnabled = CInt(autoheadroomEnabComboBox.SelectedValue)
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
         standard = niBTSGConstants.StandardBasicEDR
         'BT Generation 
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
