''==================================================================================================
'' Title        : Pulsed Data
'' Description  : This example demonstrates how to use scripts to generate pulsed data.  Pulsed
''			     data is typically used in time division multiplexing (TDM) systems.  TDM is a 
''			     type of multiplexing where two or more channels of information are transmitted 
''			     over the same link by allocating a different time interval ("slot") for the 
''			     transmission of each channel.  The set of all slots makes up a frame.  This 
''			     example generates a CW inside one of the time slots. 
''
''			     Note: In order to run this example, the upconverter must be configured with 
''			     an Arbitrary Waveform Generator. 
''			     To do this, open Measurement & Automation Explorer, select the upconverter 
''			     and click on properties.
''==================================================================================================

Imports System
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg
    Const ArbPreFilterGain As Double = -2
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
        Dim framePeriod As Double
        Dim timeSlots As Integer
        Dim iqRate As Double
        Dim script As String
        Dim actualIQRate As Double
        Dim waveformQuantum As Integer
        Dim powerLevelType As RfsgRFPowerLevelType
        Dim waveformSize As Integer
        Dim offData As Double()
        Dim iData As Double()
        Dim qData As Double()
        Dim actualTimeSlotPeriod As Double
        Dim waveformItr As Integer
        Try
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)
            framePeriod = CDbl(framePeriodNumeric.Value)
            timeSlots = CInt(timeSlotsNumeric.Value)
            iqRate = CDbl(iqRateNumeric.Value)
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


            ' Configure Pre-filter Gain to avoid overflow due to phase-
            '  discontinuous signals 
            _rfsgSession.Arb.PreFilterGain = ArbPreFilterGain


            '  Disable phase continuity since we do not expect to maintain a
            '   phase continuous signal in a pulsed data scenario 
            _rfsgSession.Arb.PhaseContinuityEnabled = RfsgPhaseContinuityEnabled.Disabled


            ' Get the actual IQ rate, and populate the GUI output 
            actualIQRate = _rfsgSession.Arb.IQRate
            actualIQRateTextBox.Text = actualIQRate.ToString()

            ' Get the waveform quantum in order to generate an aligned waveform 
            waveformQuantum = _rfsgSession.Arb.WaveformCapabilities.WaveformQuantum

            ' Configure the script with the timeSlots -
            script = "script generationWithPulsedData" & vbCr & vbLf &
                "   repeat forever" & vbCr & vbLf &
                "       repeat " + (timeSlots - 1).ToString() + vbCr & vbLf &
                "           generate offTime" & vbCr & vbLf &
                "       end repeat" & vbCr & vbLf &
                "       generate carrier" & vbCr & vbLf &
                "   end repeat" & vbCr & vbLf &
                "end script"

            waveformSize = RfsgCreatePulsedData(actualIQRate, framePeriod, timeSlots, waveformQuantum)

            ' Initialize waveforms -
            offData = New Double(waveformSize - 1) {}
            iData = New Double(waveformSize - 1) {}
            qData = New Double(waveformSize - 1) {}
            For waveformItr = 0 To waveformSize - 1
                offData(waveformItr) = 0.0
                iData(waveformItr) = 1.0
                qData(waveformItr) = 0.0
            Next

            ' Populate the display -
            actualTimeSlotPeriod = waveformSize / iqRate
            actualTimeSlotPeriodTextBox.Text = actualTimeSlotPeriod.ToString()
            actualFramePeriodTextBox.Text = (actualTimeSlotPeriod * timeSlots).ToString()


            ' Write the two waveforms 
            _rfsgSession.Arb.WriteWaveform("offTime", offData, offData)
            _rfsgSession.Arb.WriteWaveform("carrier", iData, qData)

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

    Private Shared Function RfsgCreatePulsedData(ByVal iqRate As Double, ByVal framePeriod As Double, ByVal timeSlots As Integer, ByVal waveformQuantum As Integer) As Integer
        Dim kMinWaveformSize As Integer = 16
        Dim theoreticalWaveformSize As Integer

        Try
            ' Determine waveform size
            theoreticalWaveformSize = CInt(framePeriod * iqRate / timeSlots)

            If theoreticalWaveformSize < kMinWaveformSize Then
                theoreticalWaveformSize = kMinWaveformSize
            End If

            Return RfsgCoerceToQuantum(theoreticalWaveformSize, waveformQuantum)
        Catch
            Throw
        End Try
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
        framePeriodNumeric.Enabled = enabled
        timeSlotsNumeric.Enabled = enabled
        iqRateNumeric.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
