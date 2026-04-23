'==================================================================================================
' Title        : Configure Marker At Sample 0
' Description  : This example demonstrates how to generate marker at the sample 0 of an 
'                arbitrary waveform. The example generates a double side band waveform.
'==================================================================================================
Imports NationalInstruments.ModularInstruments.NIRfsg

Public Class ConfigureMarkerAtSample0
	'declare variables
	Private _rfsgSession As NIRfsg
	Private resourceName As String
	Private frequency As Double, power As Double
    Private numberOfSamples As Integer = 100
	Private waveformName As String = "wfm"
	Private iData As Double(), qData As Double()
	'marker locations expressed in samples
	Private locations As Double() = New Double() {0}

	Public Sub New()
		Try

			InitializeVariables()

			StartGeneration()

			Dim generationStatus As RfsgGenerationStatus = RfsgGenerationStatus.InProgress

			Console.WriteLine("Press any key to stop generation.")

			Do
				generationStatus = _rfsgSession.CheckGenerationStatus()
			Loop While (generationStatus <> RfsgGenerationStatus.Complete) AndAlso (Not Console.KeyAvailable)
		Catch ex As Exception
			ShowError("ConfigureMarkerAtSample0()", ex)
		Finally
			StopGeneration()
			Console.WriteLine("Press any key to exit the application.")
			Console.ReadKey()

		End Try
	End Sub

	#Region "Initialize Variables Section"

	Private Sub InitializeVariables()
        resourceName = "RFSG"
		frequency = 1000000000.0
		'Hz
		power = -20
		'dBm
        iData = New Double(numberOfSamples - 1) {}
        qData = New Double(numberOfSamples - 1) {}
        'Generate IQ Data for Double Side Band Waveform
        iData = SinePattern(numberOfSamples, 1.0, 0.0, 1.0)
        qData = SinePattern(numberOfSamples, 1.0, 0.0, 1.0)

	End Sub

	#End Region

	#Region "Program Functions"

	Private Sub StartGeneration()

		Try
			' Initialize the NIRfsg session
			_rfsgSession = New NIRfsg(resourceName, True, False)

			' Subscribe to Rfsg warnings
			AddHandler _rfsgSession.DriverOperation.Warning, New EventHandler(Of RfsgWarningEventArgs)(AddressOf DriverOperation_Warning)

			' Configure the instrument 
			_rfsgSession.RF.Configure(frequency, power)
			_rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform

			' Write the arb waveform 
			_rfsgSession.Arb.WriteWaveform(waveformName, iData, qData)

			' Set Marker Locations
			_rfsgSession.Arb.Waveforms(waveformName).Markers(0).SetMarkerEventLocations(locations)

			' Initiate Generation 
			_rfsgSession.Initiate()

			'Write to Console

			Console.WriteLine("Started Signal Generation.")
		Catch ex As Exception
			ShowError("StartGeneration()", ex)
		End Try
	End Sub

	Private Sub DriverOperation_Warning(sender As Object, e As RfsgWarningEventArgs)
		' Display the rfsg warning
		Console.WriteLine(e.Message)
	End Sub

    Private Shared Function SinePattern(numberOfSamples As Integer, amplitude As Double, phaseDegrees As Double, numberOfCycles As Double) As Double()
        Dim sineArray As Double() = New Double(numberOfSamples - 1) {}
        For i As Integer = 0 To numberOfSamples - 1
            sineArray(i) = amplitude * Math.Sin(2 * Math.PI * i * numberOfCycles / numberOfSamples + Math.PI * phaseDegrees / 180)
        Next
        Return sineArray
    End Function

	Private Sub CheckGeneration()
		Try
			' Check the status of the RFSG 
			_rfsgSession.CheckGenerationStatus()
		Catch ex As Exception
			ShowError("CheckGeneration()", ex)
		End Try
	End Sub

	Private Sub StopGeneration()
		Try
			If _rfsgSession IsNot Nothing Then
				' Disable the output.  This sets the noise floor as low as possible.
				_rfsgSession.RF.OutputEnabled = False

				' Unsubscribe from warning events
				RemoveHandler _rfsgSession.DriverOperation.Warning, AddressOf DriverOperation_Warning

                ' Close the NIRfsg session
				_rfsgSession.Close()
			End If
			_rfsgSession = Nothing
		Catch ex As Exception
			ShowError("StopGeneration()", ex)
		End Try
	End Sub

	Private Sub ShowError(functionName As String, exception As Exception)
		StopGeneration()
		Console.WriteLine("Error in " & functionName & ": " & exception.Message)
	End Sub

	#End Region

End Class
