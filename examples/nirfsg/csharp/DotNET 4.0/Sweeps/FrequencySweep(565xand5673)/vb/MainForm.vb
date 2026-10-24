'==================================================================================================
' Title        : FrequencySweep(565xand5673)
' Description  : This program demonstrates the use of niRFSG to generate a sine wave with a 
'			     frequency sweep and specified output power for the 565x and 5673. 
'
'			     Note: 
'			     The 565x and 5673 frequency sweep is different from the standard example because
'			     the 565x and 5673 has the ability to make changes on-the-fly.  The example no longer
'			     uses abort and initiate when the frequency changes and instead, it now waits until
'			     the output is settled. This ensures that the signal is constantly outputted.
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
    Private _stopFrequency As Double, _currentFrequency As Double, _frequencyIncrement As Double
    Const FrequencyReferenceRate As Double = 10000000.0

    Public Sub New()
        InitializeComponent()

        LoadRfsgDeviceNames()

        ConfigureClockSourceComboBox()
        ConfigureLoopBandwidthComboBox()
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

    Private Sub ConfigureLoopBandwidthComboBox()
        loopBandwidthComboBox.Items.AddRange([Enum].GetNames(GetType(RfsgLoopBandwidth)))
        loopBandwidthComboBox.SelectedIndex = 0
    End Sub

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim power As Double, startFrequency As Double
        Dim numberOfSteps As Integer, dwellTime As Integer
        Dim clockSource As RfsgFrequencyReferenceSource
        Dim loopBandwidth As RfsgLoopBandwidth
        Try
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            power = CDbl(powerLevelNumeric.Value)
            startFrequency = CDbl(startFrequencyNumeric.Value)
            _stopFrequency = CDbl(stopFrequencyNumeric.Value)
            numberOfSteps = CInt(numberStepsNumeric.Value)
            dwellTime = CInt(dwellTimeNumeric.Value * 1000)
            clockSource = If(TryCast(clockSourceComboBox.SelectedValue, RfsgFrequencyReferenceSource), RfsgFrequencyReferenceSource.FromString(clockSourceComboBox.Text))
            loopBandwidth = DirectCast([Enum].Parse(GetType(RfsgLoopBandwidth), DirectCast(loopBandwidthComboBox.SelectedItem, String)), RfsgLoopBandwidth)

            _currentFrequency = startFrequency
            _frequencyIncrement = (_stopFrequency - startFrequency) / (numberOfSteps - 1)

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the power level and the starting frequency 
            _rfsgSession.RF.Configure(_currentFrequency, power)
            actualCurrentFrequencyTextBox.Text = _currentFrequency.ToString()

            ' Configure generation mode (continuous wave) 
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave

            ' Configure the clock source 
            _rfsgSession.FrequencyReference.Configure(clockSource, FrequencyReferenceRate)

            ' Configure the loop bandwidth 
            _rfsgSession.RF.LocalOscillator.LoopBandwidth = loopBandwidth

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

    Private Sub SetNextSweepFrequency()
        Try
            _currentFrequency += _frequencyIncrement

            ' If we are beyond the last frequency, it is time to stop. 
            If (_frequencyIncrement >= 0 AndAlso _currentFrequency > _stopFrequency) OrElse (_frequencyIncrement < 0 AndAlso _currentFrequency < _stopFrequency) Then
                StopGeneration()
            Else
                ' Set the frequency on the fly 
                _rfsgSession.RF.Frequency = _currentFrequency

                ' Wait for the output to settle after making the change 
                _rfsgSession.Utility.WaitUntilSettled(10000)

                actualCurrentFrequencyTextBox.Text = _currentFrequency.ToString()
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
        clockSourceComboBox.Enabled = enabled
        loopBandwidthComboBox.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
