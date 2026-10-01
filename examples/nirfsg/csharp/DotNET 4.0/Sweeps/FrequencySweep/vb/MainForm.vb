'==================================================================================================
' Title        : Frequency Sweep
' Description  : This program demonstrates the use of niRFSG to generate a sine wave with a 
'			     frequency sweep and specified output power. 
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
    Shared _stopFrequency As Double, currentFrequency As Double, frequencyIncrement As Double

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

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim power As Double, startFrequency As Double
        Dim dwellTime As Integer
        Dim numberOfSteps As Integer
        Try
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            power = CDbl(powerLevelNumeric.Value)
            startFrequency = CDbl(startFrequencyNumeric.Value)
            _stopFrequency = CDbl(stopFrequencyNumeric.Value)
            numberOfSteps = CInt(numberStepsNumeric.Value)
            dwellTime = CInt(dwellTimeNumeric.Value * 1000)

            currentFrequency = startFrequency
            frequencyIncrement = (_stopFrequency - startFrequency) / (numberOfSteps - 1)

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the instrument for power setting
            _rfsgSession.RF.PowerLevel = power

            ' Set the start frequency 
            _rfsgSession.RF.Frequency = currentFrequency
            actualCurrentFrequencyTextBox.Text = currentFrequency.ToString()

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Start the frequency sweep timer 
            rfsgStatusTimer.Interval = dwellTime
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

    Private Sub SetNextSweepFrequency()
        Try
            currentFrequency += frequencyIncrement

            ' If we are beyond the last frequency, it is time to stop. 
            If (frequencyIncrement >= 0 AndAlso currentFrequency > _stopFrequency) OrElse (frequencyIncrement < 0 AndAlso currentFrequency < _stopFrequency) Then
                StopGeneration()
            Else
                ' Abort current generation 
                _rfsgSession.Abort()

                ' Set the frequency on the fly 
                _rfsgSession.RF.Frequency = currentFrequency

                ' Restart generation 
                _rfsgSession.Initiate()

                actualCurrentFrequencyTextBox.Text = currentFrequency.ToString()
                Application.DoEvents()
            End If
        Catch ex As Exception
            ShowError("SetNextSweepFrequency()", ex)
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
        SetNextSweepFrequency()
    End Sub
#End Region

    Private Sub EnableControls(ByVal enabled As Boolean)
        startButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        resourceNameComboBox.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        startFrequencyNumeric.Enabled = enabled
        stopFrequencyNumeric.Enabled = enabled
        numberStepsNumeric.Enabled = enabled
        dwellTimeNumeric.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
