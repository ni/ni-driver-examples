'==================================================================================================
' Title        : Script Trigger-Hardware Source
' Description  : This example demonstrates how to a generate signals based on a hardware script
'			     trigger. The behavior of the signals is dictated by a script. This example has 
'			     two scripts.  The first alternates between two signals when the specified software 
'			     script trigger is received. The second script shows nested triggering.  It waits 
'			     for a trigger for it to start generating a waveform.  A separate trigger will 
'			     generate a second waveform. 
'
'			     Note: In order to run this example, the upconverter must be configured with 
'			     an Arbitrary Waveform Generator. 
'			     To do this, open Measurement & Automation Explorer, select the upconverter 
'			     and click on properties.
'==================================================================================================

Imports System
Imports System.Linq
Imports System.Windows.Forms
Imports System.Collections
Imports System.Collections.Generic
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg
    Const IDataphaseDegrees As Double = 0.0
    Const QDataPhaseDegrees As Double = 90.0
    Const NumberOfCycles As Double = 1.0
    Const Amplitude As Double = 1.0
    Const ArbPreFilterGain As Double = -2
    Private _scripts As String() = {
                                    "script triggersToggleWaveforms" & vbCr & vbLf &
                                    "   repeat forever" & vbCr & vbLf &
                                    "       repeat until scriptTrigger0" & vbCr & vbLf &
                                    "          generate negativeOffset" & vbCr & vbLf &
                                    "       end repeat" & vbCr & vbLf &
                                    "        repeat until scriptTrigger0" & vbCr & vbLf &
                                    "          generate positiveOffset" & vbCr & vbLf &
                                    "      end repeat" & vbCr & vbLf &
                                    "  end repeat" & vbCr & vbLf &
                                    "end script",
                                   "script myScript" & vbCr & vbLf &
                                   "    repeat forever" & vbCr & vbLf &
                                   "       generate allZeros" & vbCr & vbLf &
                                   "        clear scriptTrigger0" & vbCr & vbLf &
                                   "        wait until scriptTrigger0" & vbCr & vbLf &
                                   "        clear scriptTrigger1" & vbCr & vbLf &
                                   "        repeat until scriptTrigger1" & vbCr & vbLf &
                                   "           generate positiveOffset" & vbCr & vbLf &
                                   "        end repeat" & vbCr & vbLf &
                                   "        clear scriptTrigger1" & vbCr & vbLf &
                                   "        repeat until scriptTrigger1" & vbCr & vbLf &
                                   "            generate negativeOffset" & vbCr & vbLf &
                                   "        end repeat" & vbCr & vbLf &
                                   "    end repeat" & vbCr & vbLf &
                                   "end script"}

    Public Sub New()
        InitializeComponent()

        LoadRfsgDeviceNames()

        ConfigureTrigger2TypeComboBox()
        ConfigureTrigger1TypeComboBox()
        ConfigureTriggerSource2ComboBox()
        ConfigureTriggerSource1ComboBox()

        ' force set the script
        scriptIndexNumeric.Value = 1
        scriptIndexNumeric.Value = 0
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

    Private Sub ConfigureTrigger2TypeComboBox()
        trigger2TypeComboBox.Items.Add(RfsgScriptTriggerType.None)
        trigger2TypeComboBox.Items.Add(RfsgScriptTriggerType.DigitalEdge)
        trigger2TypeComboBox.Items.Add(RfsgScriptTriggerType.DigitalLevel)
        trigger2TypeComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureTrigger1TypeComboBox()
        trigger1TypeComboBox.Items.Add(RfsgScriptTriggerType.None)
        trigger1TypeComboBox.Items.Add(RfsgScriptTriggerType.DigitalEdge)
        trigger1TypeComboBox.Items.Add(RfsgScriptTriggerType.DigitalLevel)
        trigger1TypeComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureTriggerSource2ComboBox()
        Dim triggerSource2ValueList = New List(Of DictionaryEntry)()
        triggerSource2ValueList.Add(New DictionaryEntry("PFI0", RfsgDigitalEdgeScriptTriggerSource.Pfi0))
        triggerSource2ValueList.Add(New DictionaryEntry("PFI1", RfsgDigitalEdgeScriptTriggerSource.Pfi1))
        triggerSource2ValueList.Add(New DictionaryEntry("PFI2", RfsgDigitalEdgeScriptTriggerSource.Pfi2))
        triggerSource2ValueList.Add(New DictionaryEntry("PFI3", RfsgDigitalEdgeScriptTriggerSource.Pfi3))
        triggerSource2ValueList.Add(New DictionaryEntry("PXI_Trig0", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine0))
        triggerSource2ValueList.Add(New DictionaryEntry("PXI_Trig1", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine1))
        triggerSource2ValueList.Add(New DictionaryEntry("PXI_Trig2", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine2))
        triggerSource2ValueList.Add(New DictionaryEntry("PXI_Trig3", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine3))
        triggerSource2ValueList.Add(New DictionaryEntry("PXI_Trig4", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine4))
        triggerSource2ValueList.Add(New DictionaryEntry("PXI_Trig5", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine5))
        triggerSource2ValueList.Add(New DictionaryEntry("PXI_Trig6", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine6))
        triggerSource2ValueList.Add(New DictionaryEntry("PXI_Trig7", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine7))
        triggerSource2ValueList.Add(New DictionaryEntry("PXI_STAR", RfsgDigitalEdgeScriptTriggerSource.PxiStarLine))

        triggerSource2ComboBox.DataSource = triggerSource2ValueList
        triggerSource2ComboBox.DisplayMember = "Key"
        triggerSource2ComboBox.ValueMember = "Value"
        triggerSource2ComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureTriggerSource1ComboBox()
        Dim triggerSource1ValueList = New List(Of DictionaryEntry)()
        triggerSource1ValueList.Add(New DictionaryEntry("PFI0", RfsgDigitalEdgeScriptTriggerSource.Pfi0))
        triggerSource1ValueList.Add(New DictionaryEntry("PFI1", RfsgDigitalEdgeScriptTriggerSource.Pfi1))
        triggerSource1ValueList.Add(New DictionaryEntry("PFI2", RfsgDigitalEdgeScriptTriggerSource.Pfi2))
        triggerSource1ValueList.Add(New DictionaryEntry("PFI3", RfsgDigitalEdgeScriptTriggerSource.Pfi3))
        triggerSource1ValueList.Add(New DictionaryEntry("PXI_Trig0", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine0))
        triggerSource1ValueList.Add(New DictionaryEntry("PXI_Trig1", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine1))
        triggerSource1ValueList.Add(New DictionaryEntry("PXI_Trig2", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine2))
        triggerSource1ValueList.Add(New DictionaryEntry("PXI_Trig3", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine3))
        triggerSource1ValueList.Add(New DictionaryEntry("PXI_Trig4", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine4))
        triggerSource1ValueList.Add(New DictionaryEntry("PXI_Trig5", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine5))
        triggerSource1ValueList.Add(New DictionaryEntry("PXI_Trig6", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine6))
        triggerSource1ValueList.Add(New DictionaryEntry("PXI_Trig7", RfsgDigitalEdgeScriptTriggerSource.PxiTriggerLine7))
        triggerSource1ValueList.Add(New DictionaryEntry("PXI_STAR", RfsgDigitalEdgeScriptTriggerSource.PxiStarLine))

        triggerSource1ComboBox.DataSource = triggerSource1ValueList
        triggerSource1ComboBox.DisplayMember = "Key"
        triggerSource1ComboBox.ValueMember = "Value"
        triggerSource1ComboBox.SelectedIndex = 0
    End Sub

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim kNumberOfSamples As Integer = 100
        Dim kNumberOfBlankSamples As Integer = 500
        Dim triggerType1 As RfsgScriptTriggerType
        Dim triggerType2 As RfsgScriptTriggerType
        Dim scriptNum As Integer
        Dim frequency As Double
        Dim power As Double
        Dim iqRate As Double
        Dim actualIQRate As Double
        Dim frequencyOffset As Double
        Dim triggerSource1 As String
        Dim triggerSource2 As String
        Dim iData As Double()
        Dim qData As Double()
        Dim blankData As Double()
        Try
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)
            iqRate = CDbl(iqRateNumeric.Value)
            triggerType1 = DirectCast(trigger1TypeComboBox.SelectedItem, RfsgScriptTriggerType)
            triggerType2 = DirectCast(trigger2TypeComboBox.SelectedItem, RfsgScriptTriggerType)
            triggerSource1 = triggerSource1ComboBox.Text
            triggerSource2 = triggerSource2ComboBox.Text

            iData = SinePattern(kNumberOfSamples, Amplitude, IDataphaseDegrees, NumberOfCycles)
            qData = SinePattern(kNumberOfSamples, Amplitude, QDataPhaseDegrees, NumberOfCycles)
            blankData = Enumerable.Repeat(0.0, kNumberOfBlankSamples).ToArray()

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the instrument 
            _rfsgSession.RF.Configure(frequency, power)

            ' Configure the generation mode to Script 
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script

            ' Configure the IQ rate of the waveforms 
            _rfsgSession.Arb.IQRate = iqRate

            ' Configure Pre-filter Gain to avoid overflow due to phase-
            '  discontinuous signals 
            _rfsgSession.Arb.PreFilterGain = ArbPreFilterGain


            ' Configure the power level type 
            _rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower

            ' Configure scriptTrigger0 (refer to the script) 
            If triggerType1 = RfsgScriptTriggerType.DigitalEdge Then
                _rfsgSession.Triggers.ScriptTriggers(0).DigitalEdge.Configure(triggerSource1, RfsgTriggerEdge.RisingEdge)
            ElseIf triggerType1 = RfsgScriptTriggerType.DigitalLevel Then
                _rfsgSession.Triggers.ScriptTriggers(0).DigitalLevel.Configure(triggerSource1, RfsgTriggerLevel.ActiveHigh)
            Else
                _rfsgSession.Triggers.ScriptTriggers(0).Disable()
            End If

            ' Configure scriptTrigger1 (refer to the script) 
            If triggerType2 = RfsgScriptTriggerType.DigitalEdge Then
                _rfsgSession.Triggers.ScriptTriggers(1).DigitalEdge.Configure(triggerSource2, RfsgTriggerEdge.RisingEdge)
            ElseIf triggerType2 = RfsgScriptTriggerType.DigitalLevel Then
                _rfsgSession.Triggers.ScriptTriggers(1).DigitalLevel.Configure(triggerSource2, RfsgTriggerLevel.ActiveHigh)
            Else
                _rfsgSession.Triggers.ScriptTriggers(1).Disable()
            End If

            ' Get the actual IQ rate, and populate the GUI output 
            actualIQRate = _rfsgSession.Arb.IQRate
            actualIQRateTextBox.Text = actualIQRate.ToString()
            frequencyOffset = actualIQRate / kNumberOfSamples
            actualFrequencyOffsetTextBox.Text = frequencyOffset.ToString()

            ' Configure the signal bandwidth 
            _rfsgSession.Arb.SignalBandwidth = frequencyOffset * 2

            ' Write the arb waveforms 
            _rfsgSession.Arb.WriteWaveform("negativeOffset", iData, qData)
            _rfsgSession.Arb.WriteWaveform("positiveOffset", qData, iData)
            _rfsgSession.Arb.WriteWaveform("allZeros", blankData, blankData)

            ' Write the script 
            scriptNum = CInt(scriptIndexNumeric.Value)
            _rfsgSession.Arb.Scripting.WriteScript(_scripts(scriptNum))

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Activate stop button 
            stopButton.Focus()

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

    Private Shared Function SinePattern(ByVal kNumberOfSamples As Integer, ByVal amplitude As Double, ByVal phaseDegrees As Double, ByVal numberOfCycles As Double) As Double()
        Dim sineArray As Double() = New Double(kNumberOfSamples - 1) {}
        For i As Integer = 0 To kNumberOfSamples - 1
            sineArray(i) = amplitude * Math.Sin(2 * Math.PI * i * numberOfCycles / kNumberOfSamples + Math.PI * phaseDegrees / 180)
        Next
        Return sineArray
    End Function

    Private Sub StopGeneration()
        ' Stop the status checking timer, and turn off the LED 
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

#End Region

#Region "Form Events"
    Private Sub scriptIndexNumeric_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles scriptIndexNumeric.ValueChanged
        Dim scriptNum As Integer
        scriptNum = CInt(scriptIndexNumeric.Value)
        If scriptNum > 1 Then
            scriptNum = 1
        End If
        scriptTextBox.Text = _scripts(scriptNum)
    End Sub

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
        trigger1TypeComboBox.Enabled = enabled
        triggerSource1ComboBox.Enabled = enabled
        trigger2TypeComboBox.Enabled = enabled
        triggerSource2ComboBox.Enabled = enabled
        scriptIndexNumeric.Enabled = enabled

        ' Start the status checking timer
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
