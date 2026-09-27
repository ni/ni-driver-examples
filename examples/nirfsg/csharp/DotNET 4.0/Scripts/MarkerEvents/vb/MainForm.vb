''==================================================================================================
'' Title        : Marker Events
'' Description  : This example demonstrates how to generate marker events. Marker events are 
''			     digital pulses that are generated on specific samples in the waveform. 
''
''			     Note: In order to run this example, the upconverter must be configured with 
''			     an Arbitrary Waveform Generator. 
''			     To do this, open Measurement & Automation Explorer, select the upconverter 
''			     and click on properties.
''==================================================================================================

Imports System
Imports System.Windows.Forms
Imports System.Collections
Imports System.Collections.Generic
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg
    Const ArbSignalBandwidth As Double = 1

    Public Sub New()
        InitializeComponent()

        LoadRfsgDeviceNames()

        ConfigureExportTerminalComboBox()

    End Sub

    Private Sub LoadRfsgDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-Rfsg")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            resourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub

#Region "UI Initial Value Config Section"

    Private Sub ConfigureExportTerminalComboBox()
        Dim exportTerminalValueList = New List(Of DictionaryEntry)()
        exportTerminalValueList.Add(New DictionaryEntry("Do Not Export", RfsgMarkerEventExportedOutputTerminal.DoNotExport))
        exportTerminalValueList.Add(New DictionaryEntry("PFI0", RfsgMarkerEventExportedOutputTerminal.Pfi0))
        exportTerminalValueList.Add(New DictionaryEntry("PFI1", RfsgMarkerEventExportedOutputTerminal.Pfi1))
        exportTerminalValueList.Add(New DictionaryEntry("PFI4", RfsgMarkerEventExportedOutputTerminal.Pfi4))
        exportTerminalValueList.Add(New DictionaryEntry("PFI5", RfsgMarkerEventExportedOutputTerminal.Pfi5))
        exportTerminalValueList.Add(New DictionaryEntry("PXI_Trig0", RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine0))
        exportTerminalValueList.Add(New DictionaryEntry("PXI_Trig1", RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine1))
        exportTerminalValueList.Add(New DictionaryEntry("PXI_Trig2", RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine2))
        exportTerminalValueList.Add(New DictionaryEntry("PXI_Trig3", RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine3))
        exportTerminalValueList.Add(New DictionaryEntry("PXI_Trig4", RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine4))
        exportTerminalValueList.Add(New DictionaryEntry("PXI_Trig5", RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine5))
        exportTerminalValueList.Add(New DictionaryEntry("PXI_Trig6", RfsgMarkerEventExportedOutputTerminal.PxiTriggerLine6))

        exportTerminalComboBox.DataSource = exportTerminalValueList
        exportTerminalComboBox.DisplayMember = "Key"
        exportTerminalComboBox.ValueMember = "Value"
        exportTerminalComboBox.SelectedValue = RfsgMarkerEventExportedOutputTerminal.DoNotExport
    End Sub

    

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim kNumberOfSamples As Integer = 64
        Dim frequency As Double
        Dim power As Double
        Dim iqRate As Double
        Dim powerLevelType As RfsgRFPowerLevelType
        Dim script As String
        Dim outputTerminal As RfsgMarkerEventExportedOutputTerminal
        Dim actualIQRate As Double
        Dim iData As Double()
        Dim qData As Double()
        Dim waveformItr As Integer
        Try
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)
            iqRate = CDbl(iqRateNumeric.Value)
            script = scriptTextBox.Text
            outputTerminal = If(TryCast(exportTerminalComboBox.SelectedValue, RfsgMarkerEventExportedOutputTerminal), RfsgMarkerEventExportedOutputTerminal.FromString(exportTerminalComboBox.Text))
            powerLevelType = RfsgRFPowerLevelType.PeakPower

            ' Generate I and Q data 
            iData = New Double(kNumberOfSamples - 1) {}
            qData = New Double(kNumberOfSamples - 1) {}
            For waveformItr = 0 To kNumberOfSamples - 1
                iData(waveformItr) = 1.0
                qData(waveformItr) = 0.0
            Next

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the session 
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the instrument 
            _rfsgSession.RF.Configure(frequency, power)

            ' Configure the power level type 
            _rfsgSession.RF.PowerLevelType = powerLevelType

            ' Configure the generation mode to Script 
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script

            ' Export the marker event to the desired output terminal 
            _rfsgSession.DeviceEvents.MarkerEvents(0).ExportedOutputTerminal = outputTerminal

            ' Configure the IQ rate of the waveforms 
            _rfsgSession.Arb.IQRate = iqRate

            ' Configure the signal bandwidth 
            _rfsgSession.Arb.SignalBandwidth = ArbSignalBandwidth


            ' Get the actual IQ rate, and populate the GUI output 
            actualIQRate = _rfsgSession.Arb.IQRate
            actualIQRateTextBox.Text = actualIQRate.ToString()


            ' Write the DC arb waveform 
            _rfsgSession.Arb.WriteWaveform("waveformWithMarkers", iData, qData)

            ' Write the script 
            _rfsgSession.Arb.Scripting.WriteScript(script)

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Start the status checking timer 
            EnableControls(False)
            stopButton.Focus()
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
        End Try
    End Sub


    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = e.Message
    End Sub
    Private Sub CheckGeneration()
        Try
            ' Check the status of the RFSG 
            _rfsgSession.CheckGenerationStatus()
        Catch ex As Exception
            ShowError("CheckGeneration()", ex)
        End Try
    End Sub

    Private Sub StopGeneration()
        ' Stop the status checking timer 
        EnableControls(True)
        Try
            ' Disable the output.  This sets the noise floor as low as possible.
            If _rfsgSession IsNot Nothing Then
                _rfsgSession.RF.OutputEnabled = False

                ' Unsubscribe from warning events
                RemoveHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

                ' Close the RFSG NIRfsg session
                _rfsgSession.Close()
            End If
            _rfsgSession = Nothing
        Catch ex As Exception
            errorTextBox.Text = "Error in StopGeneration(): " + ex.Message
        End Try
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

#End Region

    Private Sub EnableControls(ByVal enabled As Boolean)
        startButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        resourceNameComboBox.Enabled = enabled
        scriptTextBox.Enabled = enabled
        frequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        iqRateNumeric.Enabled = enabled
        exportTerminalComboBox.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
