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
    Private resourceName, clkOutTerm As String
    Private referenceClockSource, exportClock, waveformName As String
    Private model As String
    Private iDcOffset, qDcOffset, iQGainImbalance, carrierFrequencyOffset, carrierToNoiseRatio, powerLevel, quadratureSkew, headroom, externalAttenuation, carrierFrequency, actualHeadroom, upConverterCenterFrequency, upConverterCenterFrequencyOffset, hdtPhyInterval As Double
    Private channelNumber, allIqImpairmentsEnabled, awgnEnabled, autoHeadroomEnabled, payloadLengthMode, outputPort, terminalConfiguration, zadoffChuIndex, numberOfPayloads, format1PayloadZoneConfigurationMode, directionFindingMode, dataRate, payloadZoneLength, oversamplingFactor As Integer
    Private payloadLength, txLenSequenceNumber, numberOfBlocks, blockSize, lastBlockSize, txBlockMap As Integer()
    Private actualPayloadLengthBytes As Integer()
    Private physicalChannelAddress As Long
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
        ConfigureFormat1PayloadZoneConfigurationModeComboBox()
        ConfigurePayloadLengthModeComboBox()
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

        numberOfPayloadNumericUpDown.Minimum = 1
        numberOfPayloadNumericUpDown.Maximum = 100
        numberOfPayloadNumericUpDown.Increment = 1
        numberOfPayloadNumericUpDown.Value = 1
        numberOfPayloadNumericUpDown.DecimalPlaces = 0

        OversamplingFactorNumeric.Minimum = 1
        OversamplingFactorNumeric.Maximum = 100
        OversamplingFactorNumeric.Increment = 1
        OversamplingFactorNumeric.Value = 8
        OversamplingFactorNumeric.DecimalPlaces = 0
    End Sub

    Private Sub ConfigureAutoheadroomEnabComboBox()
        'var autoheadroomEnabValueList = new List<DictionaryEntry>();
        autoheadroomEnabComboBox.DataSource = GetTrueFalseValueList()
        autoheadroomEnabComboBox.DisplayMember = "Key"
        autoheadroomEnabComboBox.ValueMember = "Value"
        autoheadroomEnabComboBox.SelectedIndex = 1
    End Sub

    Private Function GetTrueFalseValueList() As List(Of DictionaryEntry)
        Dim trueFalseValueList As List(Of DictionaryEntry) = New List(Of DictionaryEntry)()
        trueFalseValueList.Add(New DictionaryEntry("False", niBTSGConstants.False))
        trueFalseValueList.Add(New DictionaryEntry("True", niBTSGConstants.True))
        Return trueFalseValueList
    End Function

    Private Sub ConfigureRefSourceComboBox()
        Dim refSourceValueList As List(Of DictionaryEntry) = New List(Of DictionaryEntry)()
        refSourceValueList.Add(New DictionaryEntry("OnBoardClock", RfsgFrequencyReferenceSource.OnboardClock))
        refSourceValueList.Add(New DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn))
        refSourceValueList.Add(New DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock))
        refSourceValueList.Add(New DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn))
        refSourceComboBox.DataSource = refSourceValueList
        refSourceComboBox.DisplayMember = "Key"
        refSourceComboBox.ValueMember = "Value"
        refSourceComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureClkOutTerminalComboBox()
        Dim clkOutTerminalValueList As List(Of DictionaryEntry) = New List(Of DictionaryEntry)()
        clkOutTerminalValueList.Add(New DictionaryEntry("Do not export clock", RfsgOutputTerminal.DoNotExport))
        clkOutTerminalValueList.Add(New DictionaryEntry("RefOut", RfsgOutputTerminal.ReferenceOut))
        clkOutTerminalValueList.Add(New DictionaryEntry("RefOut2", RfsgOutputTerminal.ReferenceOut2))
        clkOutTerminalValueList.Add(New DictionaryEntry("ClkOut", RfsgOutputTerminal.ClockOut))
        clkOutTerminalValueList.Add(New DictionaryEntry("PFI0", RfsgOutputTerminal.PFI0))
        clkOutTerminalValueList.Add(New DictionaryEntry("PFI1", RfsgOutputTerminal.PFI1))
        clkOutTerminalValueList.Add(New DictionaryEntry("PFI4", RfsgOutputTerminal.PFI4))
        clkOutTerminalValueList.Add(New DictionaryEntry("PFI5", RfsgOutputTerminal.PFI5))
        clkOutTerminalValueList.Add(New DictionaryEntry("PXI_Trig0", RfsgOutputTerminal.PxiTriggerLine0))
        clkOutTerminalValueList.Add(New DictionaryEntry("PXI_Trig1", RfsgOutputTerminal.PxiTriggerLine1))
        clkOutTerminalValueList.Add(New DictionaryEntry("PXI_Trig2", RfsgOutputTerminal.PxiTriggerLine2))
        clkOutTerminalValueList.Add(New DictionaryEntry("PXI_Trig3", RfsgOutputTerminal.PxiTriggerLine3))
        clkOutTerminalValueList.Add(New DictionaryEntry("PXI_Trig4", RfsgOutputTerminal.PxiTriggerLine4))
        clkOutTerminalValueList.Add(New DictionaryEntry("PXI_Trig5", RfsgOutputTerminal.PxiTriggerLine5))
        clkOutTerminalValueList.Add(New DictionaryEntry("PXI_Trig6", RfsgOutputTerminal.PxiTriggerLine6))
        clkOutTerminalComboBox.DataSource = clkOutTerminalValueList
        clkOutTerminalComboBox.DisplayMember = "Key"
        clkOutTerminalComboBox.ValueMember = "Value"
        clkOutTerminalComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureAllIqImpairEnComboBox()
        allIqImpairEnComboBox.DataSource = GetTrueFalseValueList()
        allIqImpairEnComboBox.DisplayMember = "Key"
        allIqImpairEnComboBox.ValueMember = "Value"
        allIqImpairEnComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureAwgnEnabledComboBox()
        awgnEnabledComboBox.DataSource = GetTrueFalseValueList()
        awgnEnabledComboBox.DisplayMember = "Key"
        awgnEnabledComboBox.ValueMember = "Value"
        awgnEnabledComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureOutputPortComboBox()
        Dim opList As List(Of DictionaryEntry) = New List(Of DictionaryEntry)()
        opList.Add(New DictionaryEntry("RF Out", RfsgOutputPort.RFOut))
        opList.Add(New DictionaryEntry("IQ Out", RfsgOutputPort.IQOut))
        outputPortComboBox.DataSource = opList
        outputPortComboBox.DisplayMember = "Key"
        outputPortComboBox.ValueMember = "Value"
        outputPortComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureTerminalConfigurationComboBox()
        Dim tcList As List(Of DictionaryEntry) = New List(Of DictionaryEntry)()
        tcList.Add(New DictionaryEntry("Differential", RfsgTerminalConfiguration.Differential))
        tcList.Add(New DictionaryEntry("SingleEnded", RfsgTerminalConfiguration.SingleEnded))
        terminalConfigurationComboBox.DataSource = tcList
        terminalConfigurationComboBox.DisplayMember = "Key"
        terminalConfigurationComboBox.ValueMember = "Value"
        terminalConfigurationComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureFormat1PayloadZoneConfigurationModeComboBox()
        Dim format1PayloadZoneConfigurationModeList As List(Of DictionaryEntry) = New List(Of DictionaryEntry)()
        format1PayloadZoneConfigurationModeList.Add(New DictionaryEntry("Auto", 0))
        format1PayloadZoneConfigurationModeList.Add(New DictionaryEntry("User Defined", 1))
        Format1PayloadZoneConfigurationModeComboBox.DataSource = format1PayloadZoneConfigurationModeList
        Format1PayloadZoneConfigurationModeComboBox.DisplayMember = "Key"
        Format1PayloadZoneConfigurationModeComboBox.ValueMember = "Value"
        Format1PayloadZoneConfigurationModeComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigurePayloadLengthModeComboBox()
        Dim payloadLengthModeList As List(Of DictionaryEntry) = New List(Of DictionaryEntry)()
        payloadLengthModeList.Add(New DictionaryEntry("Maximum Length", 0))
        payloadLengthModeList.Add(New DictionaryEntry("User Defined", 1))
        payloadLengthModeComboBox.DataSource = payloadLengthModeList
        payloadLengthModeComboBox.DisplayMember = "Key"
        payloadLengthModeComboBox.ValueMember = "Value"
        payloadLengthModeComboBox.SelectedIndex = 0
    End Sub

#End Region

    Private Sub generateButton_Click(sender As Object, e As EventArgs) Handles generateButton.Click
        Try
            'Deactivate the 'Start' button
            generateButton.Enabled = False
            'Activate the 'Stop' button
            stopButton.Enabled = True

            'Reset the error message text box
            errorTextBox.Text = "No Error"

            'Read the Panel control values
            resourceName = rfsgResourceTextBox.Text
            referenceClockSource = refSourceComboBox.SelectedValue.ToString()
            exportClock = clkOutTerminalComboBox.SelectedValue.ToString()
            numberOfPayloads = CInt(numberOfPayloadNumericUpDown.Value)
            format1PayloadZoneConfigurationMode = CInt(Format1PayloadZoneConfigurationModeComboBox.SelectedValue)
            payloadLengthMode = CInt(payloadLengthModeComboBox.SelectedValue)
            oversamplingFactor = CInt(OversamplingFactorNumeric.Value)
            waveformName = waveNameTextBox.Text
            script = scriptTextBox.Text
            carrierFrequency = Double.Parse(carrierFreqTextBox.Text)
            externalAttenuation = externalAttnNumeric.Value
            headroom = headroomNumeric.Value
            quadratureSkew = quadratureSkewNumeric.Value
            iDcOffset = iDcOffsetNumeric.Value
            qDcOffset = qDcOffsetNumeric.Value
            iQGainImbalance = iqGaimbalanceNumeric.Value
            carrierFrequencyOffset = carrierFreqOffNumeric.Value
            carrierToNoiseRatio = cnrNumeric.Value
            channelNumber = CInt(chnNumberNumeric.Value)
            powerLevel = powerLevelNumeric.Value
            awgnEnabled = CInt(awgnEnabledComboBox.SelectedValue)
            allIqImpairmentsEnabled = CInt(allIqImpairEnComboBox.SelectedValue)
            actualHeadroom = Convert.ToDouble(actualHeadroomTextBox.Text)
            clkOutTerm = clkOutTerminalComboBox.SelectedValue.ToString()
            autoHeadroomEnabled = CInt(autoheadroomEnabComboBox.SelectedValue)
            outputPort = CInt(outputPortComboBox.SelectedValue)
            terminalConfiguration = CInt(terminalConfigurationComboBox.SelectedValue)
            dataRate = CInt(dataRateNumeric.Value)
            zadoffChuIndex = CInt(zadoffChuIndexNumeric.Value)
            physicalChannelAddress = CLng(physicalChannelAddressNumeric.Value)
            hdtPhyInterval = HdtPhyIntervalNumeric.Value

            '  BT Generation 
            StartGeneration()

            ' Start the status checking timer
            timer.Enabled = True
        Catch exception As Exception
            ShowError("StartGeneration()", exception)
        End Try
    End Sub


    Private Sub stopButton_Click(sender As Object, e As EventArgs) Handles stopButton.Click
        If rfsgSession IsNot Nothing Then
            rfsgSession.Abort()
        End If
        StopGeneration()
    End Sub

    Private Sub StartGeneration()
        ' Bluetooth Session
        If btsgSession Is Nothing Then
            btsgSession = New niBTSG(niBTSGConstants.ToolkitCompatibilityVersion020000)
        End If
        niBTSG.ChannelNumberToCarrierFrequency(channelNumber, niBTSGConstants.StandardLE, carrierFrequency)
        carrierFreqTextBox.Text = carrierFrequency.ToString()

        btsgSession.SetCarrierMode(Nothing, niBTSGConstants.CarrierModeBurst)
        btsgSession.SetPacketType(Nothing, niBTSGConstants.PacketTypeLEHdt)

        'Set Data Rate
        btsgSession.SetDataRate(Nothing, dataRate)

        ' Set Payload Header 
        btsgSession.SetPayloadLengthMode(Nothing, payloadLengthMode)

        ' Set Oversampling Factor 
        btsgSession.SetOversamplingFactor(Nothing, oversamplingFactor)

        ' High Data Throughput
        'Set HDT Packet Format
        btsgSession.SetHdtPacketFormat(Nothing, niBTSGConstants.HdtPacketFormat_Format1)

        'Set Zadoff-Chu Index
        btsgSession.SetZadoffChuIndex(Nothing, zadoffChuIndex)

        'Set Physical Channel Address
        btsgSession.SetPhysicalChannelAddress(Nothing, physicalChannelAddress)

        'Set HDT PHY Interval
        btsgSession.SetHdtPhyInterval(Nothing, hdtPhyInterval)

        btsgSession.SetNumberOfPayloads(Nothing, numberOfPayloads)

        ' User Defined Payload Zone Properties Settings 
        ' PayloadLengthBytes
        Dim PayloadLengthBytesArraySize As Integer = PayloadLengthBytesGrid.Rows.Count
        payloadLength = New Integer(PayloadLengthBytesArraySize - 1) {}
        For rows As Integer = 0 To PayloadLengthBytesArraySize - 1
            If payloadLengthMode = niBTSGConstants.PayloadLengthModeUserDefined Then
                payloadLength(rows) = Convert.ToInt32(PayloadLengthBytesGrid.Rows(rows).Cells(1).Value.ToString())
            End If
        Next

        ' txLenSequenceNumber
        Dim txLenSequenceNumberArraySize As Integer = TxLenSequenceNumberGrid.Rows.Count
        txLenSequenceNumber = New Integer(txLenSequenceNumberArraySize - 1) {}
        For rows As Integer = 0 To txLenSequenceNumberArraySize - 1
            Dim value As Integer = Convert.ToInt32(TxLenSequenceNumberGrid.Rows(rows).Cells(1).Value.ToString())
            Select Case value
                Case 0 '00
                    txLenSequenceNumber(rows) = 0
                Case 1 '01
                    txLenSequenceNumber(rows) = 1
                Case 10
                    txLenSequenceNumber(rows) = 2
                Case 11
                    txLenSequenceNumber(rows) = 3
                Case Else
                    ' Optionally handle unexpected values
                    txLenSequenceNumber(rows) = value
            End Select
        Next

        ' numberOfBlocks
        Dim numberOfBlocksArraySize As Integer = NumberOfBlocksGrid.Rows.Count
        numberOfBlocks = New Integer(numberOfBlocksArraySize - 1) {}
        For rows As Integer = 0 To numberOfBlocksArraySize - 1
            numberOfBlocks(rows) = Convert.ToInt32(NumberOfBlocksGrid.Rows(rows).Cells(1).Value.ToString())
        Next

        ' blockSize
        Dim blockSizeArraySize As Integer = BlockSizeGrid.Rows.Count
        blockSize = New Integer(blockSizeArraySize - 1) {}
        For rows As Integer = 0 To blockSizeArraySize - 1
            blockSize(rows) = Convert.ToInt32(BlockSizeGrid.Rows(rows).Cells(1).Value.ToString())
        Next

        ' lastBlockSize
        Dim lastBlockSizeArraySize As Integer = LastBlockSizeGrid.Rows.Count
        lastBlockSize = New Integer(lastBlockSizeArraySize - 1) {}
        For rows As Integer = 0 To lastBlockSizeArraySize - 1
            lastBlockSize(rows) = Convert.ToInt32(LastBlockSizeGrid.Rows(rows).Cells(1).Value.ToString())
        Next

        ' txBlockMap
        Dim txBlockMapArraySize As Integer = TxBlockMapGrid.Rows.Count
        txBlockMap = New Integer(txBlockMapArraySize - 1) {}
        For rows As Integer = 0 To txBlockMapArraySize - 1
            txBlockMap(rows) = Convert.ToInt32(TxBlockMapGrid.Rows(rows).Cells(1).Value.ToString(), 16)
        Next

        ' actualPayloadLengthBytes
        Dim actualPayloadLengthBytesArraySize As Integer = ActualPayloadLengthGrid.Rows.Count
        actualPayloadLengthBytes = New Integer(actualPayloadLengthBytesArraySize - 1) {}
        For rows As Integer = 0 To actualPayloadLengthBytesArraySize - 1
            actualPayloadLengthBytes(rows) = Convert.ToInt32(ActualPayloadLengthGrid.Rows(rows).Cells(1).Value.ToString())
        Next

        For i As Integer = 0 To numberOfPayloads - 1
            Dim activeChannel As String = "payload" & i.ToString()
            btsgSession.SetFormat1PayloadZoneConfigurationMode(Nothing, format1PayloadZoneConfigurationMode)
            If format1PayloadZoneConfigurationMode = 0 Then
                If payloadLengthMode = niBTSGConstants.PayloadLengthModeUserDefined Then
                    btsgSession.SetPayloadLength(activeChannel, payloadLength(i))
                End If
            Else
                btsgSession.SetTxLenSequenceNumber(activeChannel, txLenSequenceNumber(i))
                btsgSession.SetNumberOfBlocks(activeChannel, numberOfBlocks(i))
                btsgSession.SetLastBlockSize(activeChannel, lastBlockSize(i))
                btsgSession.SetLastBlockSize(activeChannel, lastBlockSize(i))
                btsgSession.SetTxBlockMap(activeChannel, txBlockMap(i))
            End If
        Next

        ' Waveform Properties
        'Set Headroom Properties
        btsgSession.SetAutoHeadroomEnabled(Nothing, autoHeadroomEnabled)
        If autoHeadroomEnabled = niBTSGConstants.False Then
            btsgSession.SetHeadroom(Nothing, headroom)
        End If

        'Set Impairments
        btsgSession.SetAllIqImpairmentsEnabled(Nothing, allIqImpairmentsEnabled)

        If allIqImpairmentsEnabled = niBTSGConstants.True Then
            btsgSession.SetQuadratureSkew(Nothing, quadratureSkew)
            btsgSession.SetIDCOffset(Nothing, iDcOffset)
            btsgSession.SetQDCOffset(Nothing, qDcOffset)
            btsgSession.SetIQGainImbalance(Nothing, iQGainImbalance)
        End If
        btsgSession.SetCarrierFrequencyOffset(Nothing, carrierFrequencyOffset)
        btsgSession.SetAwgnEnabled(Nothing, awgnEnabled)
        btsgSession.SetCarrierToNoiseRatio(Nothing, carrierToNoiseRatio)

        ' RFSG Session
        If rfsgSession Is Nothing Then
            rfsgSession = New NIRfsg(resourceName, False, True)
        End If

        'get model
        model = rfsgSession.Identity.InstrumentModel

        ' RFSG Properties
        rfsgSession.FrequencyReference.Configure(referenceClockSource, 10000000.0)
        rfsgSession.Utility.ExportSignal(RfsgSignalType.ReferenceClock, "", exportClock)

        rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
        rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script

        'Do nothing
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
        rfsgSession.RF.ExternalGain = -externalAttenuation

        actualPayloadLengthBytes = New Integer(numberOfPayloads) {}
        ActualPayloadLengthGrid.Rows.Clear()

        ' Create & Download Waveform
        btsgSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", waveformName)
        btsgSession.GetPayloadZoneLength(Nothing, payloadZoneLength)
        PayloadZoneLengthValue.Text = payloadZoneLength.ToString()

        For i As Integer = 0 To numberOfPayloads - 1
            Dim activeChannel As String = "payload" & i.ToString()
            btsgSession.GetActualPayloadLength(activeChannel, actualPayloadLengthBytes(i))

            Dim found As Boolean = False
            For Each row As DataGridViewRow In ActualPayloadLengthGrid.Rows
                If Not row.IsNewRow AndAlso row.Cells(0).Value IsNot Nothing AndAlso CInt(row.Cells(0).Value).Equals(i) Then
                    row.Cells(1).Value = actualPayloadLengthBytes(i)
                    found = True
                    Exit For
                End If
            Next

            If Not found Then
                ActualPayloadLengthGrid.Rows.Add(i, actualPayloadLengthBytes(i))
            End If
        Next

        ' Execute Script
        Call niBTSG.RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, script, powerLevel)
        rfsgSession.RF.OutputEnabled = True
        rfsgSession.Initiate()

        ' Start the status checking timer 
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
                Call niBTSG.RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, waveformName)
            Catch
                ' ignoring the exception here - the RFSGClearDatabase API throws an exception
                ' if called more than once on the same waveformName
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
    Private Sub MainFormClosing(sender As Object, e As FormClosingEventArgs)
        StopGeneration()
        CloseSession()
    End Sub

    Private Sub ProcessTimerEvent(sender As Object, e As EventArgs)
        CheckGeneration()
    End Sub

    Private Sub CheckGeneration()
        Try
            Dim status As Integer = RfsgGenerationStatus.InProgress
            If rfsgSession IsNot Nothing Then status = rfsgSession.CheckGenerationStatus()
        Catch exception As Exception
            ShowError("CheckGeneration()", exception)
        End Try
    End Sub

    Private Sub ShowError(functionName As String, exception As Exception)
        StopGeneration()
        CloseSession()

        ' Display error to the user
        Dim exceptionMessage As String = If(String.IsNullOrEmpty(exception.Message), "Undefined Error.", exception.Message)
        errorTextBox.Text = "Error in " & functionName & Environment.NewLine & exceptionMessage
        MessageBox.Show(exceptionMessage, functionName, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub CloseSession()
        ' Close the RFSG session
        If rfsgSession IsNot Nothing Then
            rfsgSession.Abort()
            Call niBTSG.RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), Nothing, "")
            rfsgSession.Dispose()
            rfsgSession = Nothing
        End If

        ' Close the BT Session
        If btsgSession IsNot Nothing Then
            btsgSession.CloseSession()
            btsgSession = Nothing
        End If
    End Sub

    Private Sub payloadLengthInsertButton_Click(sender As Object, e As EventArgs) Handles payloadLengthInsertButton.Click
        Dim IndexNum As Integer = PayloadLengthBytesGrid.Rows.Add()
        PayloadLengthBytesGrid.Rows(IndexNum).Cells("payloadLengthBytesValues").Value = "0"
        PayloadLengthBytesGrid.Rows(IndexNum).Cells("payloadLengthIndex").Value = IndexNum.ToString()
    End Sub

    Private Sub payloadLengthDeleteButton_Click(sender As Object, e As EventArgs) Handles payloadLengthDeleteButton.Click

        If PayloadLengthBytesGrid.Rows.Count > 0 Then
            Dim IndexNum As Integer = PayloadLengthBytesGrid.CurrentCell.RowIndex
            PayloadLengthBytesGrid.Rows.RemoveAt(IndexNum)
            For i As Integer = IndexNum To PayloadLengthBytesGrid.Rows.Count - 1
                PayloadLengthBytesGrid.Rows(i).Cells("payloadLengthIndex").Value = i.ToString()
            Next
        End If
    End Sub

    Private Sub txLenSequenceNumberInsertButton_Click(sender As Object, e As EventArgs) Handles TxLenSequenceNumberInsertButton.Click
        Dim indexNum As Integer = TxLenSequenceNumberGrid.Rows.Add()
        TxLenSequenceNumberGrid.Rows(indexNum).Cells("TxLenSequenceNumberValues").Value = "11"
        TxLenSequenceNumberGrid.Rows(indexNum).Cells("TxLenSequenceNumberIndex").Value = indexNum.ToString()
    End Sub

    Private Sub txLenSequenceNumberDeleteButton_Click(sender As Object, e As EventArgs) Handles TxLenSequenceNumberDeleteButton.Click
        If TxLenSequenceNumberGrid.Rows.Count > 0 Then
            Dim indexNum As Integer = TxLenSequenceNumberGrid.CurrentCell.RowIndex
            TxLenSequenceNumberGrid.Rows.RemoveAt(indexNum)
            For i As Integer = indexNum To TxLenSequenceNumberGrid.Rows.Count - 1
                TxLenSequenceNumberGrid.Rows(i).Cells("TxLenSequenceNumberIndex").Value = i.ToString()
            Next
        End If
    End Sub

    Private Sub numberOfBlocksInsertButton_Click(sender As Object, e As EventArgs) Handles NumberOfBlocksInsertButton.Click
        Dim indexNum As Integer = NumberOfBlocksGrid.Rows.Add()
        NumberOfBlocksGrid.Rows(indexNum).Cells("NumberOfBlocksValues").Value = "15"
        NumberOfBlocksGrid.Rows(indexNum).Cells("NumberOfBlocksIndex").Value = indexNum.ToString()
    End Sub

    Private Sub numberOfBlocksDeleteButton_Click(sender As Object, e As EventArgs) Handles NumberOfBlocksDeleteButton.Click
        If NumberOfBlocksGrid.Rows.Count > 0 Then
            Dim indexNum As Integer = NumberOfBlocksGrid.CurrentCell.RowIndex
            NumberOfBlocksGrid.Rows.RemoveAt(indexNum)
            For i As Integer = indexNum To NumberOfBlocksGrid.Rows.Count - 1
                NumberOfBlocksGrid.Rows(i).Cells("NumberOfBlocksValues").Value = i.ToString()
            Next
        End If
    End Sub

    Private Sub blockSizeInsertButton_Click(sender As Object, e As EventArgs) Handles BlockSizeInsertButton.Click
        Dim indexNum As Integer = BlockSizeGrid.Rows.Add()
        BlockSizeGrid.Rows(indexNum).Cells("blockSizeValues").Value = "511"
        BlockSizeGrid.Rows(indexNum).Cells("blockSizeIndex").Value = indexNum.ToString()
    End Sub

    Private Sub blockSizeDeleteButton_Click(sender As Object, e As EventArgs) Handles BlockSizeDeleteButton.Click
        If BlockSizeGrid.Rows.Count > 0 Then
            Dim indexNum As Integer = BlockSizeGrid.CurrentCell.RowIndex
            BlockSizeGrid.Rows.RemoveAt(indexNum)
            For i As Integer = indexNum To BlockSizeGrid.Rows.Count - 1
                BlockSizeGrid.Rows(i).Cells("blockSizeIndex").Value = i.ToString()
            Next
        End If
    End Sub

    Private Sub lastBlockSizeInsertButton_Click(sender As Object, e As EventArgs) Handles LastBlockSizeInsertButton.Click
        Dim indexNum As Integer = LastBlockSizeGrid.Rows.Add()
        LastBlockSizeGrid.Rows(indexNum).Cells("LastBlockSizeValues").Value = "526"
        LastBlockSizeGrid.Rows(indexNum).Cells("LastBlockSizeIndex").Value = indexNum.ToString()
    End Sub

    Private Sub lastBlockSizeDeleteButton_Click(sender As Object, e As EventArgs) Handles LastBlockSizeDeleteButton.Click
        If LastBlockSizeGrid.Rows.Count > 0 Then
            Dim indexNum As Integer = LastBlockSizeGrid.CurrentCell.RowIndex
            LastBlockSizeGrid.Rows.RemoveAt(indexNum)
            For i As Integer = indexNum To LastBlockSizeGrid.Rows.Count - 1
                LastBlockSizeGrid.Rows(i).Cells("LastBlockSizeIndex").Value = i.ToString()
            Next
        End If
    End Sub

    Private Sub txBlockMapInsertButton_Click(sender As Object, e As EventArgs) Handles TxBlockMapInsertButton.Click
        Dim indexNum As Integer = TxBlockMapGrid.Rows.Add()
        TxBlockMapGrid.Rows(indexNum).Cells("txBlockMapValues").Value = "0xFFFF"
        TxBlockMapGrid.Rows(indexNum).Cells("txBlockMapIndex").Value = indexNum.ToString()
    End Sub

    Private Sub txBlockMapDeleteButton_Click(sender As Object, e As EventArgs) Handles TxBlockMapDeleteButton.Click
        If TxBlockMapGrid.Rows.Count > 0 Then
            Dim indexNum As Integer = TxBlockMapGrid.CurrentCell.RowIndex
            TxBlockMapGrid.Rows.RemoveAt(indexNum)
            For i As Integer = indexNum To TxBlockMapGrid.Rows.Count - 1
                TxBlockMapGrid.Rows(i).Cells("txBlockMapIndex").Value = i.ToString()
            Next
        End If
    End Sub

    Private Sub actualPayloadLengthInsertButton_Click(sender As Object, e As EventArgs) Handles ActualPayloadLengthInsertButton.Click
        Dim indexNum As Integer = ActualPayloadLengthGrid.Rows.Add()
        ActualPayloadLengthGrid.Rows(indexNum).Cells("ActualPayloadLengthValues").Value = "0"
        ActualPayloadLengthGrid.Rows(indexNum).Cells("ActualPayloadLengthIndex").Value = indexNum.ToString()
    End Sub

    Private Sub actualPayloadLengthDeleteButton_Click(sender As Object, e As EventArgs) Handles ActualPayloadLengthDeleteButton.Click
        If ActualPayloadLengthGrid.Rows.Count > 0 Then
            Dim indexNum As Integer = ActualPayloadLengthGrid.CurrentCell.RowIndex
            ActualPayloadLengthGrid.Rows.RemoveAt(indexNum)
            For i As Integer = indexNum To ActualPayloadLengthGrid.Rows.Count - 1
                ActualPayloadLengthGrid.Rows(i).Cells("ActualPayloadLengthIndex").Value = i.ToString()
            Next
        End If
    End Sub

End Class
