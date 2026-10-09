'==================================================================================================
' Title        : Arbitrary Waveform Generation
' Description  : This example demonstrates how to generate an arbitrary waveform. The example 
'			     lets you choose which waveform to create and download among three options: 
'
'			      + Double Side Band --> two tones around the center frequency. 
'			      + Lower Side Band  --> one tone to the left of the center frequency. 
'			      + Upper Side Band  --> one tone to the right of the center frequency. 
'
'			     All waveforms have 4,000 samples, which are sampled at 100 MS/s by the device. 
'			     Therefore, the frequency of waveforms generated is 25 kHz; if seen in a 
'			     spectrum analyzer the power will show 25 kHz away from the specified center 
'			     frequency. 
'
'			     Note: In order to run this example, the upconverter must be configured with 
'			     an Arbitrary Waveform Generator. 
'			     To do this, open Measurement & Automation Explorer, select the upconverter 
'			     and click on properties.
'==================================================================================================

Imports System
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg

    Public Sub New()
        InitializeComponent()

        LoadRfsgDeviceNames()

        ConfigureWaveformComboBox()
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

    Private Sub ConfigureWaveformComboBox()
        waveformComboBox.Items.AddRange(New String() {"DoubleSideBand", "LowerSideBand", "UpperSideBand"})
        waveformComboBox.SelectedIndex = 0
    End Sub

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim kNumberOfSamples As Integer = 100
        Dim frequency As Double
        Dim frequencyOffset As Double
        Dim power As Double
        Dim waveform As Integer
        Dim iqRate As Double
        Dim actualIQRate As Double
        Dim iData As Double()
        Dim qData As Double()
        Try
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)
            waveform = waveformComboBox.SelectedIndex
            iqRate = CDbl(iqRateNumeric.Value)

            iData = New Double(kNumberOfSamples - 1) {}
            qData = New Double(kNumberOfSamples - 1) {}

            Select Case waveform
                Case 0
                    ' Double Side Band 
                    iData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0)
                    qData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0)
                    Exit Select

                Case 1
                    ' Lower Side Band 
                    iData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0)
                    qData = SinePattern(kNumberOfSamples, 1.0, 90.0, 1.0)
                    Exit Select

                Case 2
                    ' Upper Side Band 
                    iData = SinePattern(kNumberOfSamples, 1.0, 90.0, 1.0)
                    qData = SinePattern(kNumberOfSamples, 1.0, 0.0, 1.0)
                    Exit Select
            End Select

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the instrument 
            _rfsgSession.RF.Configure(frequency, power)
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform
            _rfsgSession.Arb.IQRate = iqRate

            ' Get the actual IQ rate, and populate the GUI output 
            actualIQRate = _rfsgSession.Arb.IQRate
            frequencyOffset = actualIQRate / kNumberOfSamples

            actualIQRateTextBox.Text = actualIQRate.ToString()
            actualFrequencyOffsetTextBox.Text = frequencyOffset.ToString()

            ' Configure the signal bandwidth to twice the baseband signal's maximum frequency deviation from 0 Hz. 
            _rfsgSession.Arb.SignalBandwidth = 2 * frequencyOffset

            ' Write the arb waveform 
            _rfsgSession.Arb.WriteWaveform(String.Empty, iData, qData)

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Activate stop button 
            stopButton.Focus()
            EnableControls(False)
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
        End Try
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = e.Message
    End Sub

    Private Shared Function SinePattern(ByVal kNumberOfSamples As Integer, ByVal amplitude As Double, ByVal phaseDegrees As Double, ByVal numberOfCycles As Double) As Double()
        Dim sineArray As Double() = New Double(kNumberOfSamples - 1) {}
        For i As Integer = 0 To kNumberOfSamples - 1
            sineArray(i) = amplitude * Math.Sin(2 * Math.PI * i * numberOfCycles / kNumberOfSamples + Math.PI * phaseDegrees / 180)
        Next
        Return sineArray
    End Function

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
            If _rfsgSession IsNot Nothing Then
                ' Disable the output.  This sets the noise floor as low as possible.
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

        ' Start the status checking timer
        rfsgStatusTimer.Enabled = Not enabled

        resourceNameComboBox.Enabled = enabled
        frequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        iqRateNumeric.Enabled = enabled
        waveformComboBox.Enabled = enabled

        Application.DoEvents()
    End Sub
End Class
