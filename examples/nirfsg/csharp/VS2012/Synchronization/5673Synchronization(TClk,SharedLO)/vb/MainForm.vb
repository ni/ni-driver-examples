'==================================================================================================
' Title        : 5673 Synchronization (TClk,SharedLO)
' Description  : This example demonstrates how to synchronize multiple NI 5673 device that share a LO using NI-TClk. 
'                The master 5673 has a LO and AWG while the slaves should be configured to have an external LO through MAX.
'==================================================================================================

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.TimingServices
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgMasterSession As NIRfsg
    Private _rfsgSlaveSessions As NIRfsg()
    Private _numberOfSlaveResources As Integer
    Private _tClockSession As TClock
    Const FrequencyReferenceRate As Double = 10000000.0

    Public Sub New()
        InitializeComponent()

        LoadMasterRfsgDeviceNames()
        ConfigureSlaveFrequencyReferenceOutputComboBox()
        ConfigureMasterFrequencyReferenceOutputComboBox()
        ConfigureSlaveFrequencyReferenceSourceComboBox()
        ConfigureMasterFrequencyReferenceSourceComboBox()
    End Sub

#Region "UI Initial Value Config Section"

    Private Sub LoadMasterRfsgDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-Rfsg")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            masterRfsgResourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            masterRfsgResourceNameComboBox.SelectedIndex = 0
            slaveRfsgResourceNamesTextBox.Text = masterRfsgResourceNameComboBox.Text
        End If
    End Sub

    Private Sub ConfigureSlaveFrequencyReferenceOutputComboBox()
        Dim freqSourceValueList = New List(Of DictionaryEntry)()
        freqSourceValueList.Add(New DictionaryEntry("ClkOut", RfsgFrequencyReferenceExportedOutputTerminal.ClockOut))
        freqSourceValueList.Add(New DictionaryEntry("Do Not Export", RfsgFrequencyReferenceExportedOutputTerminal.DoNotExport))
        freqSourceValueList.Add(New DictionaryEntry("RefOut", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut))
        freqSourceValueList.Add(New DictionaryEntry("RefOut2", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut2))

        slaveFrequencyReferenceOutputComboBox.DataSource = freqSourceValueList
        slaveFrequencyReferenceOutputComboBox.DisplayMember = "Key"
        slaveFrequencyReferenceOutputComboBox.ValueMember = "Value"
        slaveFrequencyReferenceOutputComboBox.SelectedValue = RfsgFrequencyReferenceExportedOutputTerminal.ClockOut
    End Sub

    Private Sub ConfigureMasterFrequencyReferenceOutputComboBox()
        Dim freqSourceValueList = New List(Of DictionaryEntry)()
        freqSourceValueList.Add(New DictionaryEntry("ClkOut", RfsgFrequencyReferenceExportedOutputTerminal.ClockOut))
        freqSourceValueList.Add(New DictionaryEntry("Do Not Export", RfsgFrequencyReferenceExportedOutputTerminal.DoNotExport))
        freqSourceValueList.Add(New DictionaryEntry("RefOut", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut))
        freqSourceValueList.Add(New DictionaryEntry("RefOut2", RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut2))

        masterFrequencyReferenceOutputComboBox.DataSource = freqSourceValueList
        masterFrequencyReferenceOutputComboBox.DisplayMember = "Key"
        masterFrequencyReferenceOutputComboBox.ValueMember = "Value"
        masterFrequencyReferenceOutputComboBox.SelectedValue = RfsgFrequencyReferenceExportedOutputTerminal.ClockOut
    End Sub

    Private Sub ConfigureSlaveFrequencyReferenceSourceComboBox()
        Dim refSourceValueList = New List(Of DictionaryEntry)()
        refSourceValueList.Add(New DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn))
        refSourceValueList.Add(New DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock))
        refSourceValueList.Add(New DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock))
        refSourceValueList.Add(New DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn))

        slaveFrequencyReferenceSourceComboBox.DataSource = refSourceValueList
        slaveFrequencyReferenceSourceComboBox.DisplayMember = "Key"
        slaveFrequencyReferenceSourceComboBox.ValueMember = "Value"
        slaveFrequencyReferenceSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.ClockIn
    End Sub

    Private Sub ConfigureMasterFrequencyReferenceSourceComboBox()
        Dim refSourceValueList = New List(Of DictionaryEntry)()
        refSourceValueList.Add(New DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn))
        refSourceValueList.Add(New DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock))
        refSourceValueList.Add(New DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock))
        refSourceValueList.Add(New DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn))

        masterFrequencyReferenceSourceComboBox.DataSource = refSourceValueList
        masterFrequencyReferenceSourceComboBox.DisplayMember = "Key"
        masterFrequencyReferenceSourceComboBox.ValueMember = "Value"
        masterFrequencyReferenceSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.OnboardClock
    End Sub

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim masterResourceName As String
        Dim slaveResourceNamesList As String()
        Dim frequency As Double, power As Double, loOutPower As Double = 0.0
        Dim index As Integer
        Dim masterFrequencyReferenceSource As RfsgFrequencyReferenceSource
        Dim masterFrequencyReferenceOutput As RfsgFrequencyReferenceExportedOutputTerminal
        Dim slaveFrequencyReferenceSource As RfsgFrequencyReferenceSource
        Dim slaveFrequencyReferenceOutput As RfsgFrequencyReferenceExportedOutputTerminal
        Dim rfsgSynchronizableDevices As ITClockSynchronizableDevice()
        Try
            ' Read in all the control values 
            masterResourceName = masterRfsgResourceNameComboBox.Text
            slaveResourceNamesList = slaveRfsgResourceNamesTextBox.Text.Split(",")
            frequency = CDbl(frequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)
            masterFrequencyReferenceSource = If(TryCast(masterFrequencyReferenceSourceComboBox.SelectedValue, RfsgFrequencyReferenceSource), RfsgFrequencyReferenceSource.FromString(masterFrequencyReferenceSourceComboBox.Text))
            masterFrequencyReferenceOutput = If(TryCast(masterFrequencyReferenceOutputComboBox.SelectedValue, RfsgFrequencyReferenceExportedOutputTerminal), RfsgFrequencyReferenceExportedOutputTerminal.FromString(masterFrequencyReferenceOutputComboBox.Text))
            slaveFrequencyReferenceSource = If(TryCast(slaveFrequencyReferenceSourceComboBox.SelectedValue, RfsgFrequencyReferenceSource), RfsgFrequencyReferenceSource.FromString(slaveFrequencyReferenceSourceComboBox.Text))
            slaveFrequencyReferenceOutput = If(TryCast(slaveFrequencyReferenceOutputComboBox.SelectedValue, RfsgFrequencyReferenceExportedOutputTerminal), RfsgFrequencyReferenceExportedOutputTerminal.FromString(slaveFrequencyReferenceOutputComboBox.Text))

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Configure the master RFSG
            ' Open a NI-RFSG session 
            _rfsgMasterSession = New NIRfsg(masterResourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgMasterSession.DriverOperation.Warning, AddressOf Me.MasterRfsgDriverOperation_Warning

            ' Configure the frequency and output power level 
            _rfsgMasterSession.RF.Configure(frequency, power)

            ' Configure the generation mode 
            _rfsgMasterSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave

            ' Master Rfsg is the device that has the LO.
            ' All other devices have an external LO.  The LO from the master
            ' device is daisy-chained through each 5611.
            ' Configure the reference clock source 
            _rfsgMasterSession.FrequencyReference.Configure(masterFrequencyReferenceSource, FrequencyReferenceRate)

            ' Output the LO 
            _rfsgMasterSession.RF.LocalOscillator.LOOutEnabled = True

            ' Export the reference clock for other devices to use 
            _rfsgMasterSession.FrequencyReference.ExportedOutputTerminal = masterFrequencyReferenceOutput

            ' Read the LO out power for the next device in the chain 
            loOutPower = _rfsgMasterSession.RF.LocalOscillator.LOOutPower

            ' Configure all the slave devices now
            _numberOfSlaveResources = slaveResourceNamesList.Length
            _rfsgSlaveSessions = New NIRfsg(_numberOfSlaveResources - 1) {}

            rfsgSynchronizableDevices = New ITClockSynchronizableDevice(_numberOfSlaveResources) {}
            rfsgSynchronizableDevices(0) = DirectCast(_rfsgMasterSession, ITClockSynchronizableDevice)

            For index = 0 To _numberOfSlaveResources - 1
                ' Open a NI-RFSG session 
                _rfsgSlaveSessions(index) = New NIRfsg(slaveResourceNamesList(index), True, False)

                ' Subscribe to Rfsg warnings
                AddHandler _rfsgSlaveSessions(index).DriverOperation.Warning, AddressOf Me.SlaveRfsgDriverOperation_Warning

                ' Configure the frequency and output power level 
                _rfsgSlaveSessions(index).RF.Configure(frequency, power)

                ' Configure the generation mode 
                _rfsgSlaveSessions(index).Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave


                ' Configure the reference clock source 
                _rfsgSlaveSessions(index).FrequencyReference.Configure(slaveFrequencyReferenceSource, 10000000.0)

                ' Output the LO 
                _rfsgSlaveSessions(index).RF.LocalOscillator.LOOutEnabled = (index <> slaveResourceNamesList.Length - 1)

                ' Set the LO in power as the LO out power from the previous device in the daisy-chain -
                _rfsgSlaveSessions(index).RF.LocalOscillator.LOInPower = loOutPower

                ' Export the reference clock for other devices to use 
                _rfsgSlaveSessions(index).FrequencyReference.ExportedOutputTerminal = slaveFrequencyReferenceOutput

                ' Read the LO out power for the next device in the chain 
                loOutPower = _rfsgSlaveSessions(index).RF.LocalOscillator.LOOutPower

                ' Populate the synchronizable devices array to use for TClock
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
                        RemoveHandler _rfsgSlaveSessions(index).DriverOperation.Warning, AddressOf Me.SlaveRfsgDriverOperation_Warning

                        ' Close the NI-RFSG session 
                        _rfsgSlaveSessions(index).Close()
                        _rfsgSlaveSessions(index) = Nothing
                    End If
                Next
            End If
            _rfsgSlaveSessions = Nothing

            ' Close the Master session
            If _rfsgMasterSession IsNot Nothing Then
                ' Disable the output 
                _rfsgMasterSession.RF.OutputEnabled = False

                ' Unsubscribe from warning events
                RemoveHandler _rfsgMasterSession.DriverOperation.Warning, AddressOf Me.MasterRfsgDriverOperation_Warning

                ' Close the NI-RFSG Master session 
                _rfsgMasterSession.Close()
                _rfsgMasterSession = Nothing
            End If
        Catch ex As Exception
            errorTextBox.Text = "Error in StopGeneration(): " + ex.Message
        End Try

        ' Dispose the TClock session
        _tClockSession = Nothing
    End Sub

    Private Sub ShowError(ByVal functionName As String, ByVal exception As Exception)
        StopGeneration()
        errorTextBox.Text = "Error in " + functionName + ": " + exception.Message
    End Sub

#End Region

#Region "Form Events"

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        StartGeneration()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        StopGeneration()
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        StopGeneration()
    End Sub

    Private Sub rfsgStatusTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles rfsgStatusTimer.Tick
        CheckGeneration()
    End Sub

    Private Sub SlaveRfsgDriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = "Slave Device:" + e.Message
    End Sub

    Private Sub MasterRfsgDriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = "Master Device:" + e.Message
    End Sub

#End Region

    Private Sub EnableControls(ByVal enabled As Boolean)
        startButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        masterRfsgResourceNameComboBox.Enabled = enabled
        slaveRfsgResourceNamesTextBox.Enabled = enabled
        frequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        masterFrequencyReferenceSourceComboBox.Enabled = enabled
        masterFrequencyReferenceOutputComboBox.Enabled = enabled
        slaveFrequencyReferenceSourceComboBox.Enabled = enabled
        slaveFrequencyReferenceOutputComboBox.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
