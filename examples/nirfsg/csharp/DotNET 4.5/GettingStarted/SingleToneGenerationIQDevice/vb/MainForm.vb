Imports System.Collections.Generic
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Public Partial Class MainForm
	Inherits Form
	Private _rfsgSession As NIRfsg
	Const frequencyReferenceRate As Double = 10000000.0

	Public Sub New()
		InitializeComponent()

		' Enable controls on startup
		EnableControls(True)
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

	#Region "Program Functions"

	Private Sub StartGeneration()
		Dim resourceName As String
		Dim iqPortFrequency As Double, iqOutPortLevel As Double
		Dim referenceClockSource As RfsgFrequencyReferenceSource
		Dim powerLevelType As RfsgRFPowerLevelType
		Dim waveformGenerationMode As RfsgWaveformGenerationMode
		Dim outputPort As RfsgOutputPort
		Try
			' Read in all the control values 
			resourceName = resourceNameComboBox.Text
			referenceClockSource = referenceClockComboBox.Text
			powerLevelType = RfsgRFPowerLevelType.PeakPower
			waveformGenerationMode = RfsgWaveformGenerationMode.ContinuousWave
			outputPort = RfsgOutputPort.IQOut
			iqPortFrequency = CDbl(iqPortFrequencyNumeric.Value)
			iqOutPortLevel = CDbl(iqOutPortLevelNumeric.Value)

			errorTextBox.Text = "No error."
			Application.DoEvents()

			' Initialize the NIRfsg session
			_rfsgSession = New NIRfsg(resourceName, True, False)
			_rfsgSession.FrequencyReference.Configure(referenceClockSource, frequencyReferenceRate)
			_rfsgSession.RF.PowerLevelType = powerLevelType
			_rfsgSession.Arb.GenerationMode = waveformGenerationMode

			_rfsgSession.Arb.OutputPort = outputPort
			' Configure IQOutPort CarrierFrequency, OutputPort and Level (Vpp)
			_rfsgSession.IQOutPort.CarrierFrequency = iqPortFrequency
			' Set channel name as I or Q for 5645R, empty string for 5820
			_rfsgSession.IQOutPort("").Level = iqOutPortLevel

			' Initiate Generation 
			_rfsgSession.Initiate()

			' Disable all controls
			EnableControls(False)
		Catch ex As Exception
			ShowError("StartGeneration()", ex)
		End Try
	End Sub

	Private Sub StopGeneration()
		EnableControls(True)
		updateButton.Enabled = False

		Try
			If _rfsgSession IsNot Nothing Then
				' Disable the output.  This sets the noise floor as low as possible.
				_rfsgSession.RF.OutputEnabled = False

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
			_rfsgSession.CheckGenerationStatus()
		Catch ex As Exception
			ShowError("CheckGeneration()", ex)
		End Try
	End Sub

	Private Sub UpdateGeneration()
		Dim iqPortFrequency As Double, iqOutPortLevel As Double
		Try
			' Stop the status checking timer 
			EnableControls(True)

			' Read in all the control values 
			iqPortFrequency = CDbl(iqPortFrequencyNumeric.Value)
			iqOutPortLevel = CDbl(iqOutPortLevelNumeric.Value)

			' Abort generation 
			_rfsgSession.Abort()

			_rfsgSession.Arb.OutputPort = RfsgOutputPort.IQOut
			' Configure IQOutPort CarrierFrequency, OutputPort and Level (Vpp)
			_rfsgSession.IQOutPort.CarrierFrequency = iqPortFrequency
			' Set channel name as I or Q for 5645R, empty string for 5820
			_rfsgSession.IQOutPort("").Level = iqOutPortLevel

			' Initiate Generation 
			_rfsgSession.Initiate()

			' Start the status checking timer 

			EnableControls(False)
		Catch ex As Exception
			ShowError("UpdateGeneration()", ex)
		End Try
	End Sub

	Private Sub ShowError(functionName As String, exception As Exception)
		StopGeneration()
		errorTextBox.Text = "Error in " & functionName & ": " & exception.Message
	End Sub

	#End Region

	#Region "Form Events"
    Private Sub updateButton_Click(sender As Object, e As System.EventArgs) Handles updateButton.Click
        UpdateGeneration()
    End Sub

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
		updateButton.Enabled = Not enabled
		stopButton.Enabled = Not enabled
		resourceNameComboBox.Enabled = enabled
		' Start the status checking timer
		rfsgStatusTimer.Enabled = Not enabled
		referenceClockComboBox.Enabled = enabled

		Application.DoEvents()
	End Sub
End Class
