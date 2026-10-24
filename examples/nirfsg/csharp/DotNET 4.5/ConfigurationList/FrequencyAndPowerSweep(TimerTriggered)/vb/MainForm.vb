'==================================================================================================
' Title        : Frequency And PowerSweep (TimerTriggered)
' Description  : This example demonstrates how to create a Configuration List and how to trigger 
'			     steps using a timer. 
'			     Set the Start and End to the same value to keep the Frequency or Power Level 
'			     constant in the Configuration List. 
'
'			     Note: In order to run this example, the upconverter must be configured with 
'			     an Arbitrary Waveform Generator. 
'			     To do this, open Measurement & Automation Explorer, select the upconverter 
'			     and click on properties.
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
    Const FrequencyReferenceRate As Double = 10000000.0
    Const ArbPreFilterGain As Double = -2
    Const AdvancedFrequencySettlingTime As Integer = 0


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
        Dim iqRate As Double
        Dim actualIQRate As Double
        Dim startFrequency As Double
        Dim endFrequency As Double
        Dim startPowerLevel As Double
        Dim endPowerLevel As Double
        Dim dwellTime As Double
        Dim numberOfSteps As Integer
        Dim loopBandwidth As RfsgLoopBandwidth
        Dim clockSource As String
        Dim script As String
        Dim frequencyForStep As Double
        Dim powerLevelForStep As Double
        Dim frequencyIncrement As Double = 0
        Dim powerLevelIncrement As Double = 0
        Dim waveformSize As Integer = 100000
        Dim waveformIterator As Integer, numberOfStepsIterator As Integer
        Dim iData As Double()
        Dim qData As Double()
        Dim configurationListAttributes As RfsgConfigurationListProperties() = {RfsgConfigurationListProperties.Frequency, RfsgConfigurationListProperties.PowerLevel}
        Try
            ' Read in all of the control values 
            iqRate = CDbl(iqRateNumeric.Value)
            resourceName = resourceNameComboBox.Text
            startFrequency = CDbl(startFrequencyNumeric.Value)
            endFrequency = CDbl(stopFrequencyNumeric.Value)
            startPowerLevel = CDbl(startPowerNumeric.Value)
            endPowerLevel = CDbl(stopPowerNumeric.Value)
            dwellTime = CDbl(dwellTimeNumeric.Value)
            numberOfSteps = CInt(numberStepsNumeric.Value)
            loopBandwidth = DirectCast([Enum].Parse(GetType(RfsgLoopBandwidth), DirectCast(loopBandwidthComboBox.SelectedItem, String)), RfsgLoopBandwidth)
            clockSource = DirectCast(clockSourceComboBox.Text, String)
            script = scriptTextBox.Text

            ' Generate I and Q data
            iData = New Double(waveformSize - 1) {}
            qData = New Double(waveformSize - 1) {}
            For waveformIterator = 0 To waveformSize - 1
                iData(waveformIterator) = 1.0
                qData(waveformIterator) = 0.0
            Next

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the generation mode to Script 
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script

            ' Configure the power level type 
            _rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower

            ' Configure the reference clock source 
            _rfsgSession.FrequencyReference.Configure(clockSource, FrequencyReferenceRate)

            ' Configure Pre-filter Gain to avoid overflow due to phase-
            '  discontinuous signals 
            _rfsgSession.Arb.PreFilterGain = ArbPreFilterGain

            ' Configure the loop bandwidth 
            _rfsgSession.RF.LocalOscillator.LoopBandwidth = loopBandwidth

            ' Configure the IQ rate of the waveforms 
            _rfsgSession.Arb.IQRate = iqRate

            ' Get the actual IQ rate, and populate the GUI output 
            actualIQRate = _rfsgSession.Arb.IQRate

            actualIQRateTextBox.Text = actualIQRate.ToString()

            '  Configure the Frequency Settling Units to 'Seconds After I/O'. 
            '   RFSG currrently only supports  'Seconds After I/O' when using the 
            '   Configuration List feature. 
            _rfsgSession.RF.Advanced.FrequencySettlingUnits = RfsgRFFrequencySettlingUnits.TimeAfterIO

            ' Configure the Frequency Settling to 0 seconds to indicate that we 
            '  don't want to wait for the frequency to settle in each step. If the 
            '  step is waiting for the frequency to settle, the device will not 
            '  advance to the next configuration when a trigger is received. 
            _rfsgSession.RF.Advanced.FrequencySettlingTime = AdvancedFrequencySettlingTime

            ' Configure the trigger type to advance steps in the list 
            _rfsgSession.Triggers.ConfigurationListStepTrigger.TriggerType = RfsgConfigurationListStepTriggerType.DigitalEdge

            ' Configure the trigger source to advance steps in the list 
            _rfsgSession.Triggers.ConfigurationListStepTrigger.DigitalEdge.Source = RfsgDigitalEdgeConfigurationListStepTriggerSource.TimerEvent

            ' Configure the timer event interval 
            _rfsgSession.DeviceEvents.Timer.Interval = dwellTime

            '  Create a Configuration List. Pass Frequency and Power Level in the 
            '   Configuration List Attributes parameter to be able to configure 
            '   Frequency and Power Level in each step that we create. Pass true to
            '   the Set As Active List parameter, this will set the Active Configuration 
            '   List attribute to the name of the created Configuration List. Once the 
            '   Active Configuration List is set, the Frequency or Power Level will 
            '   be modified for this configuration list. 
            _rfsgSession.BasicConfigurationList.CreateConfigurationList("frequencyAndPowerLevelList", configurationListAttributes, True)

            frequencyForStep = startFrequency
            powerLevelForStep = startPowerLevel

            If numberOfSteps > 1 Then
                frequencyIncrement = (endFrequency - startFrequency) / (numberOfSteps - 1)
                powerLevelIncrement = (endPowerLevel - startPowerLevel) / (numberOfSteps - 1)
            End If

            ' Build the Configuration List 
            For numberOfStepsIterator = 0 To numberOfSteps - 1
                '  Create a Configuration List Step. Pass true to the Set As Active 
                ' Step parameter, this will set the Active Configuration List Step 
                ' attribute to the created Configuration List Step index. Once the 
                ' Active Configuration List Step is set, the Frequency or Power Level 
                ' will be modified for this Configuration List Step in the Configuration 
                ' List indicated by the Active Configuration List attribute. 
                _rfsgSession.BasicConfigurationList.CreateStep(True)

                ' Configure the Frequency for this step
                _rfsgSession.RF.Frequency = frequencyForStep

                ' Configure the Power Level for this step
                _rfsgSession.RF.PowerLevel = powerLevelForStep

                frequencyForStep += frequencyIncrement
                powerLevelForStep += powerLevelIncrement
            Next

            ' Write the waveform 
            _rfsgSession.Arb.WriteWaveform("continuousWaveformTone", iData, qData)

            ' Write the script 
            _rfsgSession.Arb.Scripting.WriteScript(script)

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Start the status checking timer 
            EnableControls(False)
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
            If _rfsgSession IsNot Nothing Then
                ' Abort the generation to delete the Configuration List 
                _rfsgSession.Abort()

                ' Delete the Configuration List 
                _rfsgSession.BasicConfigurationList.DeleteConfigurationList("frequencyAndPowerLevelList")

                ' Set the Active Configuration List to string.Empty 
                _rfsgSession.BasicConfigurationList.ActiveList = String.Empty

                ' Disable the output 
                _rfsgSession.RF.OutputEnabled = False

                ' When a Configuration List is stopped, NI-RFSG transitions to the Configuration State.  
                '  The new value of the Output Enabled attribute is committed to apply the change to the hardware.
                _rfsgSession.Utility.Commit()

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
        iqRateNumeric.Enabled = enabled
        resourceNameComboBox.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled
        scriptTextBox.Enabled = enabled
        startFrequencyNumeric.Enabled = enabled
        stopFrequencyNumeric.Enabled = enabled
        startPowerNumeric.Enabled = enabled
        stopPowerNumeric.Enabled = enabled
        dwellTimeNumeric.Enabled = enabled
        numberStepsNumeric.Enabled = enabled
        loopBandwidthComboBox.Enabled = enabled
        clockSourceComboBox.Enabled = enabled

        Application.DoEvents()
    End Sub
End Class
