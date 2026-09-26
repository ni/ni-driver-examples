'==================================================================================================
' Title        : 5673 Synchronization (TClk,SharedLO,Script)
' Description  : This example demonstrates how to use NI-TClk to synchronize the start and script triggers of multiple NI 5673 devices      
'                that share a local oscillator (LO). The master NI 5673 has an LO and arbitrary waveform generator (AWG). Slave NI   
'                5673 devices should be configured in Measurement and Automation Explorer (MAX) to use an external LO.
'==================================================================================================

Imports System.Collections
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices
Imports NationalInstruments.ModularInstruments.SystemServices.TimingServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgMasterSession As NIRfsg
    Private _rfsgSlaveSessions As NIRfsg()
    Private _numberOfSlaveResources As Integer
    Private _tClockSession As TClock
    Const FrequencyReferenceRate As Double = 10000000.0
    Const NumberOfSamples As Integer = 50000
    Const SamplesPerCycle As Integer = 5000
    Private _frequency As Double, _power As Double, _iqRate As Double
    Private _offsetIData As Double(), _offsetQData As Double(), _iData As Double(), _qData As Double()
    Private _script As String

    Public Sub New()
        InitializeComponent()
        LoadMasterRfsgDeviceNames()
        ConfigureSlaveReferenceClockOutputTerminalComboBox()
        ConfigureMasterReferenceClockOutputTerminalComboBox()
        ConfigureSlaveReferenceClockSourceComboBox()
        ConfigureMasterReferenceClockSourceComboBox()
    End Sub

#Region "UI Initial Value Config Section"

    Private Sub LoadMasterRfsgDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-Rfsg")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            masterRfsgResourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            masterRfsgResourceNameComboBox.SelectedIndex = 0
            slaveRfsgResourceNamesTextBox.Text = modularInstrumentsSystem.DeviceCollection(0).Name
            If modularInstrumentsSystem.DeviceCollection.Count > 1 Then
                slaveRfsgResourceNamesTextBox.Text = modularInstrumentsSystem.DeviceCollection(1).Name
            End If
        End If
    End Sub

    Private Sub ConfigureSlaveReferenceClockOutputTerminalComboBox()
        Dim referenceClockOutputTerminalValueList As New List(Of DictionaryEntry)()
        referenceClockOutputTerminalValueList.Add(New DictionaryEntry("Do Not Export", RfsgFrequencyReferenceExportedOutputTerminal.DoNotExport))
        referenceClockOutputTerminalValueList.Add(New DictionaryEntry("RefOut", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut))
        referenceClockOutputTerminalValueList.Add(New DictionaryEntry("RefOut2", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut2))
        referenceClockOutputTerminalValueList.Add(New DictionaryEntry("ClkOut", RfsgFrequencyReferenceExportedOutputTerminal.ClockOut))
        slaveReferenceClockOutputTerminalComboBox.DataSource = referenceClockOutputTerminalValueList
        slaveReferenceClockOutputTerminalComboBox.DisplayMember = "Key"
        slaveReferenceClockOutputTerminalComboBox.ValueMember = "Value"
        slaveReferenceClockOutputTerminalComboBox.SelectedValue = RfsgFrequencyReferenceExportedOutputTerminal.ClockOut
    End Sub

    Private Sub ConfigureMasterReferenceClockOutputTerminalComboBox()
        Dim referenceClockOutputTerminalValueList As New List(Of DictionaryEntry)()
        referenceClockOutputTerminalValueList.Add(New DictionaryEntry("Do Not Export", RfsgFrequencyReferenceExportedOutputTerminal.DoNotExport))
        referenceClockOutputTerminalValueList.Add(New DictionaryEntry("RefOut", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut))
        referenceClockOutputTerminalValueList.Add(New DictionaryEntry("RefOut2", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut2))
        referenceClockOutputTerminalValueList.Add(New DictionaryEntry("ClkOut", RfsgFrequencyReferenceExportedOutputTerminal.ClockOut))
        masterReferenceClockOutputTerminalComboBox.DataSource = referenceClockOutputTerminalValueList
        masterReferenceClockOutputTerminalComboBox.DisplayMember = "Key"
        masterReferenceClockOutputTerminalComboBox.ValueMember = "Value"
        masterReferenceClockOutputTerminalComboBox.SelectedValue = RfsgFrequencyReferenceExportedOutputTerminal.ClockOut
    End Sub

    Private Sub ConfigureSlaveReferenceClockSourceComboBox()
        Dim referenceSourceValueList As New List(Of DictionaryEntry)()
        referenceSourceValueList.Add(New DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock))
        referenceSourceValueList.Add(New DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn))
        referenceSourceValueList.Add(New DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock))
        referenceSourceValueList.Add(New DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn))
        slaveReferenceClockSourceComboBox.DataSource = referenceSourceValueList
        slaveReferenceClockSourceComboBox.DisplayMember = "Key"
        slaveReferenceClockSourceComboBox.ValueMember = "Value"
        slaveReferenceClockSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.ClockIn
    End Sub

    Private Sub ConfigureMasterReferenceClockSourceComboBox()
        Dim referenceSourceValueList As New List(Of DictionaryEntry)()
        referenceSourceValueList.Add(New DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock))
        referenceSourceValueList.Add(New DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn))
        referenceSourceValueList.Add(New DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock))
        referenceSourceValueList.Add(New DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn))
        masterReferenceClockSourceComboBox.DataSource = referenceSourceValueList
        masterReferenceClockSourceComboBox.DisplayMember = "Key"
        masterReferenceClockSourceComboBox.ValueMember = "Value"
        masterReferenceClockSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.OnboardClock
    End Sub

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim slaveResourceNamesList As String()
        Dim loOutPower As Double
        Dim index As Integer
        Dim masterReferenceClockSource As RfsgFrequencyReferenceSource
        Dim masterReferenceClockOutput As RfsgFrequencyReferenceExportedOutputTerminal
        Dim slaveReferenceClockSource As RfsgFrequencyReferenceSource
        Dim slaveReferenceClockOutput As RfsgFrequencyReferenceExportedOutputTerminal
        Dim rfsgSynchronizableDevices As ITClockSynchronizableDevice()
        Try
            ' Read in all the control values 
            _frequency = CDbl(frequencyNumeric.Value)
            _power = CDbl(powerLevelNumeric.Value)
            _iqRate = CDbl(iqRateNumeric.Value)
            masterReferenceClockSource = If(TryCast(masterReferenceClockSourceComboBox.SelectedValue, RfsgFrequencyReferenceSource), RfsgFrequencyReferenceSource.FromString(masterReferenceClockSourceComboBox.Text))
            masterReferenceClockOutput = If(TryCast(masterReferenceClockOutputTerminalComboBox.SelectedValue, RfsgFrequencyReferenceExportedOutputTerminal), RfsgFrequencyReferenceExportedOutputTerminal.FromString(masterReferenceClockOutputTerminalComboBox.Text))
            slaveReferenceClockSource = If(TryCast(slaveReferenceClockSourceComboBox.SelectedValue, RfsgFrequencyReferenceSource), RfsgFrequencyReferenceSource.FromString(slaveReferenceClockSourceComboBox.Text))
            slaveReferenceClockOutput = If(TryCast(slaveReferenceClockOutputTerminalComboBox.SelectedValue, RfsgFrequencyReferenceExportedOutputTerminal), RfsgFrequencyReferenceExportedOutputTerminal.FromString(slaveReferenceClockOutputTerminalComboBox.Text))
            slaveResourceNamesList = slaveRfsgResourceNamesTextBox.Text.Split(Convert.ToChar(","))
            _script = scriptRichTextBox.Text

            _offsetIData = New Double(NumberOfSamples - 1) {}
            _offsetQData = New Double(NumberOfSamples - 1) {}
            _iData = New Double(NumberOfSamples - 1) {}
            _qData = New Double(NumberOfSamples - 1) {}
            For index = 0 To NumberOfSamples - 1
                _iData(index) = 1.0
                _qData(index) = 0.0
            Next
            _offsetIData = SinePattern(NumberOfSamples, 1.0, 0.0, SamplesPerCycle)
            _offsetQData = SinePattern(NumberOfSamples, 1.0, 90.0, SamplesPerCycle)

            errorTextBox.Text = "No error."
            Application.DoEvents()

            Dim currentResource As String = DirectCast(masterRfsgResourceNameComboBox.Text, String)
            _rfsgMasterSession = New NIRfsg(currentResource, True, False)
            AddHandler _rfsgMasterSession.DriverOperation.Warning, AddressOf MasterRfsgDriverOperation_Warning
            ConfigureNIRfsgSession(_rfsgMasterSession)

            ' Configure the referenceerence clock source
            _rfsgMasterSession.FrequencyReference.Configure(masterReferenceClockSource, 10000000.0)

            ' Output the LO 
            _rfsgMasterSession.RF.LocalOscillator.LOOutEnabled = True

            ' Export the referenceerence clock for other devices to use
            _rfsgMasterSession.FrequencyReference.ExportedOutputTerminal = masterReferenceClockOutput

            ' Read the LO out power for the next device in the chain
            loOutPower = _rfsgMasterSession.RF.LocalOscillator.LOOutPower

            ' Configure the master device to receive the script trigger for all 
            ' TClk will automatically synchronize and propegate the trigger to the other devices
            _rfsgMasterSession.Triggers.ScriptTriggers(0).ConfigureSoftwareTrigger()

            ' Configure all slave devices
            _numberOfSlaveResources = slaveResourceNamesList.Length
            _rfsgSlaveSessions = New NIRfsg(_numberOfSlaveResources - 1) {}
            rfsgSynchronizableDevices = New ITClockSynchronizableDevice(_numberOfSlaveResources) {}
            rfsgSynchronizableDevices(0) = DirectCast(_rfsgMasterSession, ITClockSynchronizableDevice)
            For index = 0 To _numberOfSlaveResources - 1
                currentResource = slaveResourceNamesList(index)
                _rfsgSlaveSessions(index) = New NIRfsg(currentResource, True, False)
                AddHandler _rfsgSlaveSessions(index).DriverOperation.Warning, AddressOf SlaveRfsgDriverOperation_Warning
                ConfigureNIRfsgSession(_rfsgSlaveSessions(index))

                ' Configure the referenceerence clock source
                _rfsgSlaveSessions(index).FrequencyReference.Configure(slaveReferenceClockSource, 10000000.0)

                ' Output the LO 
                _rfsgSlaveSessions(index).RF.LocalOscillator.LOOutEnabled = (index <> _numberOfSlaveResources - 1)

                ' Set the LO in power as the LO out power from the previous device in the daisy-chain
                _rfsgSlaveSessions(index).RF.LocalOscillator.LOInPower = loOutPower

                ' Export the referenceerence clock for other devices to use
                _rfsgSlaveSessions(index).FrequencyReference.ExportedOutputTerminal = slaveReferenceClockOutput

                ' Read the LO out power for the next device in the chain
                loOutPower = _rfsgSlaveSessions(index).RF.LocalOscillator.LOOutPower

                rfsgSynchronizableDevices(index + 1) = DirectCast(_rfsgSlaveSessions(index), ITClockSynchronizableDevice)
            Next

            ' Configure the devices for homogeneous triggers 
            _tClockSession = New TClock(rfsgSynchronizableDevices)
            _tClockSession.ConfigureForHomogeneousTriggers()

            ' Synchronize the generators 
            _tClockSession.Synchronize()

            ' Initiate generation 
            _tClockSession.Initiate()

            ' Start the status checking timer 
            EnableControls(False)
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
        End Try
    End Sub

    Private Sub ConfigureNIRfsgSession(ByRef _rfsgSession As NIRfsg)
        _rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
        _rfsgSession.RF.Configure(_frequency, _power)
        _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
        _rfsgSession.Arb.IQRate = _iqRate
        _rfsgSession.Arb.PreFilterGain = -2
        _rfsgSession.Arb.WriteWaveform("NegativeOffset", _offsetIData, _offsetQData)
        _rfsgSession.Arb.WriteWaveform("PositiveOffset", _offsetQData, _offsetIData)
        _rfsgSession.Arb.WriteWaveform("NoOffset", _iData, _qData)
        _rfsgSession.Arb.Scripting.WriteScript(_script)
    End Sub

    Private Sub CheckGeneration()
        Try
            Dim isDone As Boolean
            ' Continue generation until the Stop button is pressed or there is a hardware error.
            ' tClock.IsDone is used for status checking and is equivalent to _rfsgSession.CheckGenerationStatus
            isDone = _tClockSession.IsDone
        Catch ex As Exception
            ShowError("CheckGeneration()", ex)
        End Try
    End Sub

    Private Sub StopGeneration()
        EnableControls(True)
        Try
            If _rfsgSlaveSessions IsNot Nothing Then
                For index As Integer = 0 To _numberOfSlaveResources - 1
                    If _rfsgSlaveSessions(index) IsNot Nothing Then
                        ' Disable the output 
                        _rfsgSlaveSessions(index).RF.OutputEnabled = False

                        ' Unsubscribe from warning events
                        RemoveHandler _rfsgSlaveSessions(index).DriverOperation.Warning, AddressOf SlaveRfsgDriverOperation_Warning

                        ' Close the NI-RFSG session 
                        _rfsgSlaveSessions(index).Close()
                        _rfsgSlaveSessions(index) = Nothing
                    End If
                Next
                _rfsgSlaveSessions = Nothing
            End If
            If _rfsgMasterSession IsNot Nothing Then
                ' Close the Master session
                _rfsgMasterSession.RF.OutputEnabled = False
                RemoveHandler _rfsgMasterSession.DriverOperation.Warning, AddressOf MasterRfsgDriverOperation_Warning

                ' Close the NI-RFSG session 
                _rfsgMasterSession.Close()
                _rfsgMasterSession = Nothing
            End If
        Catch ex As Exception
            errorTextBox.Text = "Error in StopGeneration(): " & ex.Message
        End Try

        ' Dispose the TClock session
        _tClockSession = Nothing
    End Sub

    Private Shared Function SinePattern(ByVal NumberOfSamples As Integer, ByVal amplitude As Double, ByVal phaseDegrees As Double, ByVal numberOfCycles As Double) As Double()
        Dim sineArray As Double() = New Double(NumberOfSamples - 1) {}
        For i As Integer = 0 To NumberOfSamples - 1
            sineArray(i) = amplitude * Math.Sin(2 * Math.PI * i * numberOfCycles / NumberOfSamples + Math.PI * phaseDegrees / 180)
        Next
        Return sineArray
    End Function

    Private Sub EnableControls(ByVal enabled As Boolean)
        masterRfsgResourceNameComboBox.Enabled = enabled
        slaveRfsgResourceNamesTextBox.Enabled = enabled
        configurationGroupBox.Enabled = enabled
        frequencyReferenceGroupBox.Enabled = enabled
        scriptRichTextBox.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled
        startButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        softwareTriggerButton.Enabled = Not enabled
        Application.DoEvents()
    End Sub

    Private Sub ShowError(ByVal functionName As String, ByVal exception As Exception)
        StopGeneration()
        errorTextBox.Text = "Error in " & functionName & ": " & exception.Message
    End Sub

#End Region

#Region "Form Events"

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        StartGeneration()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        StopGeneration()
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        StopGeneration()
    End Sub

    Private Sub rfsgStatusTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles rfsgStatusTimer.Tick
        CheckGeneration()
    End Sub

    Private Sub SlaveRfsgDriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = "Slave Device:" & Convert.ToString(e.Message)
    End Sub

    Private Sub MasterRfsgDriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = "Master Device:" & Convert.ToString(e.Message)
    End Sub

    Private Sub softwareTriggerButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        _rfsgMasterSession.Triggers.ScriptTriggers(0).SendSoftwareEdgeTrigger()
    End Sub
#End Region
End Class