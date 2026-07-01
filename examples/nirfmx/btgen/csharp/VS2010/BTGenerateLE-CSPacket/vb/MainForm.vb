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
   Private resourceName As String, clkOutTerm As String
   Private referenceClockSource As String, exportClock As String, waveformName As String
   Private model As String
   Private iDcOffset As Double, qDcOffset As Double, iQGainImbalance As Double, carrierFrequencyOffset As Double, carrierToNoiseRatio As Double, powerLevel As Double,
    quadratureSkew As Double, headroom As Double, externalAttenuation As Double, carrierFrequency As Double, actualHeadroom As Double, upConverterCenterFrequency As Double,
    upConverterCenterFrequencyOffset As Double
   Private userDefBitsArray As Integer()
   Private soundingSequenceMarkerPos As Integer(), soundingSequenceMarkerSig As Integer()
   Private channelNumber As Integer, allIqImpairmentsEnabled As Integer, awgnEnabled As Integer, autoHeadroomEnabled As Integer,
    outputPort As Integer, terminalConfiguration As Integer, packetType As Integer
   Private csPacketFormat As Integer, csSyncSequence As Integer, soundingSequenceLength As Integer, csToneExtSlotEnabled As Integer
   Private numberOfUniquePackets As Integer, oversamplingFactor As Integer
   Private csPhaseMeasPeriod As Double
   Private generationStatus As RfsgGenerationStatus


    Public Sub New()
      InitializeComponent()
      ConfigureNumericUpDown()
      ConfigureAutoheadroomEnabComboBox()
      ConfigureRefSourceComboBox()
      ConfigureClkOutTerminalComboBox()
      ConfigureAllIqImpairEnComboBox()
      ConfigureAwgnEnabledComboBox()
      ConfigureOutputPortComboBox()
      ConfigureTerminalConfigurationComboBox()
      ConfigurePacketTypeComboBox()
      ConfigurecsPacketFormatComboBox()
      ConfigurecsSyncSequenceComboBox()
      ConfigurecsToneExtSlotEnabledComboBox()
      ConfigureSoundingSequenceMarkerSignalsComboBox()
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
        ptList.Add(New DictionaryEntry("LE-CS-1M", niBTSGConstants.PacketTypeLECS1M))
        ptList.Add(New DictionaryEntry("LE-CS-2M", niBTSGConstants.PacketTypeLECS2M))
        packetTypeComboBox.DataSource = ptList
        packetTypeComboBox.DisplayMember = "Key"
        packetTypeComboBox.ValueMember = "Value"
        packetTypeComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigurecsPacketFormatComboBox()
        Dim cspfList As New List(Of DictionaryEntry)()
        cspfList.Add(New DictionaryEntry("SYNC", niBTSGConstants.CSPacketFormatSync))
        cspfList.Add(New DictionaryEntry("CS Tone", niBTSGConstants.CSPacketFormatCSTone))
        cspfList.Add(New DictionaryEntry("CS Tone after SYNC", niBTSGConstants.CSPacketFormatCSToneAfterSync))
        cspfList.Add(New DictionaryEntry("CS Tone before SYNC", niBTSGConstants.CSPacketFormatCSToneBeforeSync))
        csPacketFormatComboBox.DataSource = cspfList
        csPacketFormatComboBox.DisplayMember = "Key"
        csPacketFormatComboBox.ValueMember = "Value"
        csPacketFormatComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigurecsSyncSequenceComboBox()
        Dim csssList As New List(Of DictionaryEntry)()
        csssList.Add(New DictionaryEntry("None", niBTSGConstants.CSSyncSequenceNone))
        csssList.Add(New DictionaryEntry("Sounding Sequence", niBTSGConstants.CSSyncSequenceSoundingSequence))
        csSyncSequenceComboBox.DataSource = csssList
        csSyncSequenceComboBox.DisplayMember = "Key"
        csSyncSequenceComboBox.ValueMember = "Value"
        csSyncSequenceComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigurecsToneExtSlotEnabledComboBox()
        Dim csteseList As New List(Of DictionaryEntry)()
        csteseList.Add(New DictionaryEntry("False", niBTSGConstants.False))
        csteseList.Add(New DictionaryEntry("True", niBTSGConstants.True))
        csToneExtSlotEnabledComboBox.DataSource = csteseList
        csToneExtSlotEnabledComboBox.DisplayMember = "Key"
        csToneExtSlotEnabledComboBox.ValueMember = "Value"
        csToneExtSlotEnabledComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureSoundingSequenceMarkerSignalsComboBox()
        Dim ssmsList As New List(Of DictionaryEntry)()
        ssmsList.Add(New DictionaryEntry("1100", niBTSGConstants.SoundingSequenceMarkerSignals_1100))
        ssmsList.Add(New DictionaryEntry("0011", niBTSGConstants.SoundingSequenceMarkerSignals_0011))
        SoundingSequenceMarkerSignals.DataSource = ssmsList
        SoundingSequenceMarkerSignals.DisplayMember = "Key"
        SoundingSequenceMarkerSignals.ValueMember = "Value"
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
         clkOutTerm = CType(clkOutTerminalComboBox.SelectedValue, RfsgOutputTerminal)
         autoHeadroomEnabled = CInt(autoheadroomEnabComboBox.SelectedValue)
         outputPort = CInt(outputPortComboBox.SelectedValue)
         terminalConfiguration = CInt(terminalConfigurationComboBox.SelectedValue)
         numberOfUniquePackets = CInt(NumOfUniquePacketsNumeric.Value)
         oversamplingFactor = CInt(OversamplingFactorNumeric.Value)
         csPacketFormat = CInt(csPacketFormatComboBox.SelectedValue)
         csSyncSequence = CInt(csSyncSequenceComboBox.SelectedValue)
         csPhaseMeasPeriod = CDbl(csPhaseMeasurementPeriodNumeric.Value)
         soundingSequenceLength = CInt(soundingSequenceLengthNumeric.Value)
         csToneExtSlotEnabled = CInt(csToneExtSlotEnabledComboBox.SelectedValue)

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
        niBTSG.ChannelNumberToCarrierFrequency(channelNumber, niBTSGConstants.StandardLECS, carrierFrequency)
        carrierFreqTextBox.Text = carrierFrequency.ToString()

      btsgSession.SetPacketType(Nothing, packetType)

      btsgSession.SetNumberOfUniquePackets(Nothing, numberOfUniquePackets)
      btsgSession.SetOversamplingFactor(Nothing, oversamplingFactor)

      btsgSession.SetCSPacketFormat(Nothing, csPacketFormat)
      btsgSession.SetCSSyncSequence(Nothing, csSyncSequence)
      btsgSession.SetCSPhaseMeasurementPeriod(Nothing, csPhaseMeasPeriod)
      btsgSession.SetSoundingSequenceLength(Nothing, soundingSequenceLength)
      btsgSession.SetCSToneExtensionSlotEnabled(Nothing, csToneExtSlotEnabled)


        Dim soundingSequenceMarkerPosArraySize As Integer = dataGridView1.Rows.Count
      soundingSequenceMarkerPos = New Integer(soundingSequenceMarkerPosArraySize - 1) {}
      For rows As Integer = 0 To soundingSequenceMarkerPosArraySize - 1
          If True Then
              soundingSequenceMarkerPos(rows) = Convert.ToInt32(dataGridView1.Rows(rows).Cells(1).Value.ToString())
          End If
      Next
      btsgSession.SetSoundingSequenceMarkerPositions(Nothing, soundingSequenceMarkerPos, soundingSequenceMarkerPosArraySize)

      Dim soundingSequenceMarkerSigArraySize As Integer = dataGridView2.Rows.Count
      soundingSequenceMarkerSig = New Integer(soundingSequenceMarkerSigArraySize - 1) {}
      For rows As Integer = 0 To soundingSequenceMarkerSigArraySize - 1
          If True Then
              soundingSequenceMarkerSig(rows) = Convert.ToInt32(dataGridView2.Rows(rows).Cells(1).Value.ToString())
          End If
      Next
      btsgSession.SetSoundingSequenceMarkerSignals(Nothing, soundingSequenceMarkerSig, soundingSequenceMarkerSigArraySize)

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
      dataGridView1.Rows(IndexNum).Cells("SoundingSequenceMarkerPositions").Value = "0"
      dataGridView1.Rows(IndexNum).Cells("Index").Value = IndexNum.ToString()
   End Sub

    Private Sub insertRowButton2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles insertRowButton2.Click
        Dim IndexNum As Integer = dataGridView2.Rows.Add()
        dataGridView2.Rows(IndexNum).Cells("SoundingSequenceMarkerSignals").Value = 0
        dataGridView2.Rows(IndexNum).Cells("Index").Value = IndexNum.ToString()
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

    Private Sub deleteRowButton2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles deleteRowButton2.Click
        If dataGridView2.Rows.Count > 0 Then
            Dim IndexNum As Integer = dataGridView2.CurrentCell.RowIndex
            dataGridView2.Rows.RemoveAt(IndexNum)

            For i As Integer = IndexNum To dataGridView1.Rows.Count - 1
                dataGridView2.Rows(i).Cells("Index").Value = (i).ToString()
            Next
        End If
    End Sub

End Class
