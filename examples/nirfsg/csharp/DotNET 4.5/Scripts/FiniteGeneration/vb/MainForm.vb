''==================================================================================================
'' Title        : Finite Generation
'' Description  : This example demonstrates how to generate a finite waveform.
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

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim frequency As Double
        Dim power As Double
        Dim iqRate As Double
        Dim waveformDuration As Double
        Dim numberOfSamples As Integer
        Dim waveformItr As Integer
        Dim actualIQRate As Double
        Dim quantum As Integer
        Dim powerLevelType As RfsgRFPowerLevelType
        Dim iData As Double(), qData As Double()
        Try
            Dim script As String = "script finiteGeneration" & vbCr & vbLf & "                                    generate waveform" & vbCr & vbLf & "                                end script"

            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)
            iqRate = CDbl(iqRateNumeric.Value)
            waveformDuration = CDbl(durationNumeric.Value)
            powerLevelType = RfsgRFPowerLevelType.PeakPower

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)
            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning


            ' Configure the instrument 
            _rfsgSession.RF.Configure(frequency, power)

            ' Configure the power level type 
            _rfsgSession.RF.PowerLevelType = powerLevelType

            ' Configure the generation mode to Script 
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script

            ' Configure the IQ rate of the waveforms 
            _rfsgSession.Arb.IQRate = iqRate

            ' Configure the signal bandwidth 
            _rfsgSession.Arb.SignalBandwidth = ArbSignalBandwidth

            ' Get the actual IQ rate, and populate the GUI output 
            actualIQRate = _rfsgSession.Arb.IQRate
            actualIQRateTextBox.Text = actualIQRate.ToString()

            ' Generate and Write a DC signal to be upconverted 
            quantum = _rfsgSession.Arb.WaveformCapabilities.WaveformQuantum
            numberOfSamples = RfsgCoerceToQuantum(CInt(actualIQRate * waveformDuration), quantum)

            ' Populate the GUI output for Actual Waveform Duration 
            actualDurationTextBox.Text = (numberOfSamples / actualIQRate).ToString()

            iData = New Double(numberOfSamples - 1) {}
            qData = New Double(numberOfSamples - 1) {}
            For waveformItr = 0 To numberOfSamples - 1
                iData(waveformItr) = 1.0
                qData(waveformItr) = 0.0
            Next

            _rfsgSession.Arb.WriteWaveform("waveform", iData, qData)

            ' Write the script 
            _rfsgSession.Arb.Scripting.WriteScript(script)

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Start the status checking timer 
            EnableControls(False)

            ' Activate stop button 
            stopButton.Focus()
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
        End Try
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = e.Message
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

    Private Sub CheckGeneration()
        Try
            ' Check the status of the RFSG 
            If _rfsgSession.CheckGenerationStatus() = RfsgGenerationStatus.Complete Then
                StopGeneration()
            End If
        Catch ex As Exception
            ShowError("CheckGeneration()", ex)
        End Try
    End Sub

    Private Sub ShowError(ByVal functionName As String, ByVal exception As Exception)
        StopGeneration()
        errorTextBox.Text = "Error in " + functionName + ": " + exception.Message
    End Sub

    Private Shared Function RfsgCoerceToQuantum(ByVal numberOfSamples As Integer, ByVal quantum As Integer) As Integer
        Dim smallestNumberOfSamples As Double

        If quantum <= 0 Then
            Return -1
        End If

        If numberOfSamples >= quantum Then
            smallestNumberOfSamples = numberOfSamples
        Else
            smallestNumberOfSamples = quantum
        End If

        Return CInt(Math.Round(smallestNumberOfSamples / quantum)) * quantum
    End Function

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
        frequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        iqRateNumeric.Enabled = enabled
        durationNumeric.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
