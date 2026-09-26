'==================================================================================================
' Title        : Forward Frequency Sweep (5673 In-band Retuning)
' Description  : This example demonstrates how to generate a frequency sweep 
'			     at a specified power level.
'
'			     Note: In order to run this example, you must have your 
'			     NI PXIe-5673 configured to work together with a NI PXIe-5450(AWG) 
'			     and a NI PXI-5652 (LO).
'			     To do this, open the Measurements & Automation Explorer, 
'			     select the NI PXIe-5611 and click on properties.
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
    Private _stopFrequency As Double, _currentFrequency As Double, _frequencyIncrement As Double, _inBandSpan As Double
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
        loopBandwidthComboBox.SelectedIndex = 2
    End Sub

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim power As Double
        Dim startFrequency As Double
        Dim dwellTime As Integer
        Dim upconverterCenterFrequency As Double
        Dim upconverterLoopBandwidth As RfsgLoopBandwidth
        Dim clockSource As String

        Dim numberOfSteps As Integer
        Try
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            clockSource = DirectCast(clockSourceComboBox.Text, String)
            power = CDbl(powerLevelNumeric.Value)
            startFrequency = CDbl(startFrequencyNumeric.Value)
            _stopFrequency = CDbl(stopFrequencyNumeric.Value)
            _inBandSpan = CDbl(deviceBandwidthToUseNumeric.Value)
            upconverterLoopBandwidth = DirectCast([Enum].Parse(GetType(RfsgLoopBandwidth), DirectCast(loopBandwidthComboBox.SelectedItem, String)), RfsgLoopBandwidth)
            numberOfSteps = CInt(numberStepsNumeric.Value)
            dwellTime = CInt(dwellTimeNumeric.Value * 1000)

            _currentFrequency = startFrequency
            _frequencyIncrement = (_stopFrequency - startFrequency) / (numberOfSteps - 1)

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the instrument for continuous wave generation
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave

            ' Configure the reference clock for the instrument
            _rfsgSession.FrequencyReference.Configure(clockSource, FrequencyReferenceRate)

            ' Configure the instrument for power setting and start frequency
            _rfsgSession.RF.Configure(startFrequency, power)


            ' Default signal bandwidth in Continuous Wave mode is 100
            upconverterCenterFrequency = ((_inBandSpan - 100) / 2) + startFrequency

            ' Set the upconverter center frequency and upconverter loop bandwidth-
            ' By setting the upconverter center frequency property, the in-band
            ' retuning feature is enabled.
            _rfsgSession.RF.Upconverter.CenterFrequency = upconverterCenterFrequency
            _rfsgSession.RF.LocalOscillator.LoopBandwidth = upconverterLoopBandwidth

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Output Current Frequency 
            actualCurrentFrequencyTextBox.Text = startFrequency.ToString()

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
                ' checkWarn is omitted to ensure that the session is closed even in
                ' the case of an error occurring. 

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
            Dim upconverterCenterFrequency As Double

            ' Obtain Current Upconverter Center Frequency 

            upconverterCenterFrequency = _rfsgSession.RF.Upconverter.CenterFrequency

            _currentFrequency += _frequencyIncrement

            ' If we are beyond the last frequency, it is time to stop. 
            If _currentFrequency > _stopFrequency Then
                StopGeneration()
            Else
                If (_currentFrequency - ((_inBandSpan - 100) / 2)) > upconverterCenterFrequency Then
                    ' Update Upconverter Center Frequency 
                    ' Setting the upconverter center frequency will not change the LO immediately.
                    ' This will happen when the frequency property changes.  Thus the upconverter
                    ' center frequency should be set first.
                    upconverterCenterFrequency = _currentFrequency + ((_inBandSpan - 100) / 2)

                    _rfsgSession.RF.Upconverter.CenterFrequency = upconverterCenterFrequency

                    ' Set the frequency on the fly 
                    _rfsgSession.RF.Frequency = _currentFrequency

                    _rfsgSession.Utility.WaitUntilSettled(10000)
                Else
                    ' Set the frequency on the fly 
                    _rfsgSession.RF.Frequency = _currentFrequency
                End If

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
    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        StopGeneration()
    End Sub

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        StartGeneration()
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
        clockSourceComboBox.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        startFrequencyNumeric.Enabled = enabled
        stopFrequencyNumeric.Enabled = enabled
        deviceBandwidthToUseNumeric.Enabled = enabled
        loopBandwidthComboBox.Enabled = enabled
        numberStepsNumeric.Enabled = enabled
        dwellTimeNumeric.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
