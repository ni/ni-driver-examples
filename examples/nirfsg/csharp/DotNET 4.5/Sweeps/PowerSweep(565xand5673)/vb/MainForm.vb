'==================================================================================================
' Title        : PowerSweep(565xand5673)
' Description  : This program demonstrates the use of niRFSG to generate a sine wave with a 
'			     power sweep and specified output frequency for the 565x and 5673. 
'
'			     Note: 
'			     The 565x and 5673 power sweep is different from the standard example because the
'			     565x and 5673 has the ability to make changes on-the-fly.  The example no longer uses
'			     abort and initiate when the power changes and instead, it now waits until 
'			     the output is settled.  This ensures that the signal is constantly outputted.
'==================================================================================================

Imports System
Imports System.Windows.Forms
Imports System.Collections
Imports System.Collections.Generic
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg
    Private _stopPower As Double, _currentPower As Double, _powerIncrement As Double
    Const FrequencyReferenceRate As Double = 10000000.0

    Public Sub New()
        InitializeComponent()

        LoadRfsgDeviceNames()

        ConfigureClockSourceComboBox()
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

    Private Sub ConfigureClockSourceComboBox()
        Dim refSourceValueList = New List(Of DictionaryEntry)()
        refSourceValueList.Add(New DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn))
        refSourceValueList.Add(New DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock))
        refSourceValueList.Add(New DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock))
        refSourceValueList.Add(New DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn))

        clockSourceComboBox.DataSource = refSourceValueList
        clockSourceComboBox.DisplayMember = "Key"
        clockSourceComboBox.ValueMember = "Value"
        clockSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.OnboardClock
    End Sub

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim frequency As Double
        Dim startPower As Double
        Dim dwellTime As Integer
        Dim numberOfSteps As Integer
        Dim clockSource As RfsgFrequencyReferenceSource
        Try
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            startPower = CDbl(startPowerNumeric.Value)
            _stopPower = CDbl(stopPowerNumeric.Value)
            numberOfSteps = CInt(numberStepsNumeric.Value)
            dwellTime = CInt(dwellTimeNumeric.Value * 1000)
            clockSource = If(TryCast(clockSourceComboBox.SelectedValue, RfsgFrequencyReferenceSource), RfsgFrequencyReferenceSource.FromString(clockSourceComboBox.Text))

            _currentPower = startPower
            _powerIncrement = (_stopPower - startPower) / (numberOfSteps - 1)

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' subscribe to the warning events
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure center frequency and power level 
            _rfsgSession.RF.Configure(frequency, _currentPower)

            actualCurrentPowerTextBox.Text = _currentPower.ToString()

            ' Configure the generation mode (continuous wave) 
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave

            ' Configure the clock source 
            _rfsgSession.FrequencyReference.Configure(clockSource, FrequencyReferenceRate)

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

    Private Sub SetNextSweepPower()
        Try
            _currentPower += _powerIncrement

            ' If we are beyond the last power, it is time to stop. 
            If (_powerIncrement >= 0 AndAlso _currentPower > _stopPower) OrElse (_powerIncrement < 0 AndAlso _currentPower < _stopPower) Then
                StopGeneration()
            Else
                ' Set the power on the fly 
                _rfsgSession.RF.PowerLevel = _currentPower
                actualCurrentPowerTextBox.Text = _currentPower.ToString()

                ' Wait for the output to settle 
                _rfsgSession.Utility.WaitUntilSettled(10000)
            End If
        Catch ex As Exception
            ShowError("SetNextSweepPower()", ex)
        End Try
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display any Rfsg warnings
        errorTextBox.Text = e.Message
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
        SetNextSweepPower()
    End Sub
#End Region

    Private Sub EnableControls(ByVal enabled As Boolean)
        startButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        resourceNameComboBox.Enabled = enabled
        frequencyNumeric.Enabled = enabled
        startPowerNumeric.Enabled = enabled
        stopPowerNumeric.Enabled = enabled
        numberStepsNumeric.Enabled = enabled
        dwellTimeNumeric.Enabled = enabled
        clockSourceComboBox.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
