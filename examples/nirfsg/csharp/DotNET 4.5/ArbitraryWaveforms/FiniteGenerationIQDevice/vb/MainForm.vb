Imports System.Windows.Forms
Imports System.Collections
Imports System.Collections.Generic
Imports NationalInstruments.ModularInstruments
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Public Partial Class MainForm
	Inherits Form
	Private _rfsgSession As NIRfsg
	Const frequencyReferenceRate As Double = 10000000.0

	Public Sub New()
		InitializeComponent()
		ConfigureRefClockComboBox()
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

	Private Sub ConfigureRefClockComboBox()
		Dim refClockValueList As New List(Of KeyValuePair(Of String, RfsgFrequencyReferenceSource))()
		refClockValueList.Add(New KeyValuePair(Of String, RfsgFrequencyReferenceSource)("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock))
		refClockValueList.Add(New KeyValuePair(Of String, RfsgFrequencyReferenceSource)("RefIn", RfsgFrequencyReferenceSource.ReferenceIn))
		refClockValueList.Add(New KeyValuePair(Of String, RfsgFrequencyReferenceSource)("PXI_Clk", RfsgFrequencyReferenceSource.PxiClock))
		refClockValueList.Add(New KeyValuePair(Of String, RfsgFrequencyReferenceSource)("ClkIn", RfsgFrequencyReferenceSource.ClockIn))
		referenceClockComboBox.DisplayMember = "Key"
		referenceClockComboBox.ValueMember = "Value"
		referenceClockComboBox.DataSource = refClockValueList
		referenceClockComboBox.SelectedIndex = 0
	End Sub

	#Region "UI Initial Value Config Section"


	#End Region

	#Region "Program Functions"

	Private Sub StartGeneration()
		Dim resourceName As String
		Dim referenceClockSource As RfsgFrequencyReferenceSource
		Dim iqPortFrequency As Double
		Dim iqOutPortLevel As Double
		Dim iqRate As Double
		Dim isWaveformRepeatCountFinite As Boolean
		Dim waveformRepeatCount As Integer
		Dim signalBandwidth As Double
		Dim numberOfSamples As Integer
		Dim waveformItr As Integer
		Dim actualIQRate As Double
		Dim quantum As Integer
		Dim powerLevelType As RfsgRFPowerLevelType
		Dim iData As Single(), qData As Single()
		Try
			' Read in all of the control values 
			resourceName = resourceNameComboBox.Text
			referenceClockSource = referenceClockComboBox.Text
			iqPortFrequency = CDbl(iqPortFrequencyNumeric.Value)
			iqOutPortLevel = CDbl(iqOutPortLevelNumeric.Value)
			iqRate = 50000000.0
			waveformRepeatCount = CInt(Math.Truncate(waveformRepeatCountNumeric.Value))
			isWaveformRepeatCountFinite = True
			powerLevelType = RfsgRFPowerLevelType.PeakPower

			errorTextBox.Text = "No error."
			Application.DoEvents()

			' Initialize the NIRfsg session
			_rfsgSession = New NIRfsg(resourceName, True, False)

			' Subscribe to Rfsg warnings
			AddHandler _rfsgSession.DriverOperation.Warning, New EventHandler(Of RfsgWarningEventArgs)(AddressOf DriverOperation_Warning)

			' Configure Clock Source
			_rfsgSession.FrequencyReference.Configure(referenceClockSource, frequencyReferenceRate)

			' Configure IQOutPort CarrierFrequency, OutputPort and Level (Vpp)
			_rfsgSession.IQOutPort.CarrierFrequency = iqPortFrequency
			_rfsgSession.Arb.OutputPort = RfsgOutputPort.IQOut
			_rfsgSession.IQOutPort("").Level = iqOutPortLevel

            ' Configure the generation mode
			_rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform

			' Configure the power level type 
			_rfsgSession.RF.PowerLevelType = powerLevelType

			' Configure the IQ rate of the waveforms 
			_rfsgSession.Arb.IQRate = iqRate
			_rfsgSession.Arb.IsWaveformRepeatCountFinite = isWaveformRepeatCountFinite
			_rfsgSession.Arb.WaveformRepeatCount = waveformRepeatCount

			signalBandwidth = _rfsgSession.Arb.IQRate * 0.8
			' Configure the signal bandwidth 
			_rfsgSession.Arb.SignalBandwidth = signalBandwidth

            ' Get the actual IQ rate
			actualIQRate = _rfsgSession.Arb.IQRate

			' Generate and Write a DC signal to be upconverted 
			quantum = _rfsgSession.Arb.WaveformCapabilities.WaveformQuantum
			numberOfSamples = RfsgCoerceToQuantum(CInt(Math.Truncate(actualIQRate / 2)), quantum)

			' Populate the GUI output for Actual Waveform Duration 
			waveformDurationTextBox.Text = (numberOfSamples / actualIQRate).ToString()

			iData = New Single(numberOfSamples - 1) {}
			qData = New Single(numberOfSamples - 1) {}
			For waveformItr = 0 To numberOfSamples - 1
				iData(waveformItr) = 1.0
				qData(waveformItr) = 0.0
			Next

			_rfsgSession.Arb.WriteWaveform("waveform", iData, qData)

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

	Private Sub DriverOperation_Warning(sender As Object, e As RfsgWarningEventArgs)
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
				RemoveHandler _rfsgSession.DriverOperation.Warning, AddressOf DriverOperation_Warning

				' Close the RFSG NIRfsg session
				_rfsgSession.Close()
			End If
			_rfsgSession = Nothing
		Catch ex As Exception
			errorTextBox.Text = "Error in StopGeneration(): " & ex.Message
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

	Private Sub ShowError(functionName As String, exception As Exception)
		StopGeneration()
		errorTextBox.Text = "Error in " & functionName & ": " & exception.Message
	End Sub

	Private Shared Function RfsgCoerceToQuantum(numberOfSamples As Integer, quantum As Integer) As Integer
		Dim smallestNumberOfSamples As Double

		If quantum <= 0 Then
			Return -1
		End If

		If numberOfSamples >= quantum Then
			smallestNumberOfSamples = numberOfSamples
		Else
			smallestNumberOfSamples = quantum
		End If

		Return CInt(Math.Truncate(Math.Round(smallestNumberOfSamples / quantum))) * quantum
	End Function

	#End Region

	#Region "Form Events"
    Private Sub startButton_Click(sender As Object, e As System.EventArgs) Handles startButton.Click
        StartGeneration()
    End Sub

    Private Sub stopButton_Click(sender As Object, e As System.EventArgs) Handles stopButton.Click
        StopGeneration()
    End Sub

	Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs)
		StopGeneration()
	End Sub

    Private Sub rfsgStatusTimer_Tick(sender As Object, e As System.EventArgs) Handles rfsgStatusTimer.Tick
        CheckGeneration()
    End Sub

	#End Region

	Private Sub EnableControls(enabled As Boolean)
		startButton.Enabled = enabled
		stopButton.Enabled = Not enabled
		resourceNameComboBox.Enabled = enabled
		iqPortFrequencyNumeric.Enabled = enabled
		iqOutPortLevelNumeric.Enabled = enabled
		waveformRepeatCountNumeric.Enabled = enabled
		rfsgStatusTimer.Enabled = Not enabled
		referenceClockComboBox.Enabled = enabled

		Application.DoEvents()
	End Sub
End Class
