'==================================================================================================
' Title        : Finite Generation
' Description  : This example demonstrates how to generate a finite waveform. 
'==================================================================================================

Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Public Partial Class MainForm
	Inherits Form
	Private _rfsgSession As NIRfsg

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
		Dim frequency As Double
		Dim power As Double
		Dim waveformQuantum As Integer
		Dim waveformRepeatCount As Double
		Dim actualIQRate As Double
		Dim numberOfSamples As Integer
		Dim iData As Single()
		Dim qData As Single()
        Try
            EnableControls(False)
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)
            waveformRepeatCount = CDbl(waveformRepeatCountRateNumeric.Value)

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, New EventHandler(Of RfsgWarningEventArgs)(AddressOf DriverOperation_Warning)

            ' Configure the instrument 
            _rfsgSession.RF.Configure(frequency, power)
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform
            _rfsgSession.Arb.IQRate = 50000000.0

            ' Enable finite generation and set number of times to repeat the waveform.
            _rfsgSession.Arb.IsWaveformRepeatCountFinite = True
            _rfsgSession.Arb.WaveformRepeatCount = Convert.ToInt32(waveformRepeatCount)

            ' Configure the signal bandwidth
            ' Set the signal bandwidth to 1Hz since we are outputting a sine tone.
            _rfsgSession.Arb.SignalBandwidth = 1.0

            ' Get the actual (coerced) IQ rate.
            actualIQRate = _rfsgSession.Arb.IQRate

            ' Calculate the number of samples it would take to last 500ms and coerce to
            ' the minimum waveform quantum
            waveformQuantum = _rfsgSession.Arb.WaveformCapabilities.WaveformQuantum
            numberOfSamples = CoerceToQuantum(actualIQRate * 0.5, waveformQuantum)

            ' Populate the GUI output for Actual Waveform Duration.
            Dim waveformDuration As Double = numberOfSamples / actualIQRate
            waveformDurationTextBox.Text = [String].Format("{0:#,0.000}", waveformDuration)

            ' Generate a DC signal to be upconverted
            iData = New Single(numberOfSamples - 1) {}
            qData = New Single(numberOfSamples - 1) {}

            For i As Integer = 0 To numberOfSamples - 1
                iData(i) = 1.0
                qData(i) = 0.0
            Next

            ' Write the waveform, since we aren't using multiple waveforms or selecting
            ' it in a script it doesn't need a name.
            _rfsgSession.Arb.WriteWaveform("", iData, qData)

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Start the status checking timer 
            rfsgStatusTimer.Enabled = True

            ' Activate stop button 
            stopButton.Focus()
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
        End Try
	End Sub

	Private Shared Function CoerceToQuantum(numberOfSamples As Double, waveformQuantum As Integer) As Integer
		Dim smallestNumberOfSamples As Double

		If waveformQuantum <= 0 Then
			Return -1
		End If

		If numberOfSamples >= waveformQuantum Then
			smallestNumberOfSamples = numberOfSamples
		Else
			smallestNumberOfSamples = waveformQuantum
		End If

        Return Convert.ToInt32(smallestNumberOfSamples / waveformQuantum) * waveformQuantum
	End Function

	Private Sub DriverOperation_Warning(sender As Object, e As RfsgWarningEventArgs)
		' Display the rfsg warning
		errorTextBox.Text = e.Message
	End Sub

	Private Shared Function SinePattern(kNumberOfSamples As Integer, amplitude As Double, phaseDegrees As Double, numberOfCycles As Double) As Double()
		Dim sineArray As Double() = New Double(kNumberOfSamples - 1) {}
		For i As Integer = 0 To kNumberOfSamples - 1
			sineArray(i) = amplitude * Math.Sin(2 * Math.PI * i * numberOfCycles / kNumberOfSamples + Math.PI * phaseDegrees / 180)
		Next
		Return sineArray
	End Function

	Private Sub CheckGeneration()
		Try
			' Check the status of the RFSG 
            Dim generationStatus As RfsgGenerationStatus = _rfsgSession.CheckGenerationStatus()
            If generationStatus = RfsgGenerationStatus.Complete Then
                StopGeneration()
            End If

		Catch ex As Exception
			ShowError("CheckGeneration()", ex)
		End Try
	End Sub

	Private Sub StopGeneration()
		' Stop the status checking timer 
        rfsgStatusTimer.Enabled = False
        EnableControls(True)

		Try
			If _rfsgSession IsNot Nothing Then
				' Disable the output.  This sets the noise floor as low as possible.
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

	Private Sub ShowError(functionName As String, exception As Exception)
		StopGeneration()
		errorTextBox.Text = "Error in " & functionName & ": " & exception.Message
	End Sub
	#End Region

	#Region "Form Events"

    Private Sub startButton_Click(sender As Object, e As System.EventArgs) Handles startButton.Click
        StartGeneration()
    End Sub

    Private Sub stopButton_Click(sender As Object, e As System.EventArgs) Handles stopButton.Click
        StopGeneration()
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
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
		frequencyNumeric.Enabled = enabled
		powerLevelNumeric.Enabled = enabled
		waveformRepeatCountRateNumeric.Enabled = enabled

		Application.DoEvents()
	End Sub
End Class
