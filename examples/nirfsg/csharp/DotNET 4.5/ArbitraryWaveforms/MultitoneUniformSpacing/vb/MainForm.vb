'==================================================================================================
' Title        : Multitone Uniform Spacing
' Description  : This example demonstrates how to generate uniformly separated tones.
'			     
'			     The example lets you choose the number of tones and the frequency between
'			     them, as well as the power and initial phase for all the tones.
'			     
'			     The plot shows the baseband spectrum of the generated signal. The baseband
'			     spectrum is calculated in this example.
'	
'			     Note: In order to run this example, the upconverter must be configured with 
'			     an Arbitrary Waveform Generator. 
'			     To do this, open Measurement & Automation Explorer, select the upconverter 
'			     and click on properties.
'==================================================================================================

Imports System
Imports System.Linq
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
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

    Private Sub ShowError(ByVal functionName As String, ByVal exception As Exception)
        StopGeneration()
        errorTextBox.Text = "Error in " + functionName + ": " + exception.Message
    End Sub

    Private Sub CheckGeneration()
        Try
            ' Check the status of the RFSG
            _rfsgSession.CheckGenerationStatus()
        Catch ex As Exception
            ShowError("CheckGeneration()", ex)
        End Try
    End Sub

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim quantum As Integer
        Dim numberOfTones As Integer
        Dim maximumNumberOfSamples As Integer
        Dim iqRate As Double
        Dim powerLevel As Double
        Dim centerFrequency As Double
        Dim initialPhase As Double
        Dim actualIQRate As Double
        Dim frequencyBetweenTones As Double
        Dim peakEnvelopePower As Double
        Dim queryID As Boolean = True
        Dim reset As Boolean = False

        Try
            ' Clear previous errors
            errorTextBox.Text = "No error."

            ' Read controls from GUI
            resourceName = resourceNameComboBox.Text
            numberOfTones = CInt(numberOfTonesNumeric.Value)
            maximumNumberOfSamples = CInt(maximumNumberOfSamplesNumeric.Value)
            iqRate = CDbl(iqRateNumeric.Value)
            powerLevel = CDbl(powerPerToneNumeric.Value)
            centerFrequency = CDbl(centerFrequencyNumeric.Value)
            initialPhase = CDbl(initialPhaseNumeric.Value)
            frequencyBetweenTones = CDbl(frequencyBetweenTonesNumeric.Value)

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, queryID, reset)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the NIRfsg session
            ' Configure generation mode
            ' Set generation mode to Arb Waveform           
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform

            _rfsgSession.Arb.IQRate = iqRate

            _rfsgSession.Arb.PreFilterGain = -2

            ' Read NIRfsg sessions configuration 
            actualIQRate = _rfsgSession.Arb.IQRate

            quantum = _rfsgSession.Arb.WaveformCapabilities.WaveformQuantum

            ' Generate waveform
            Dim complexEquidistantMultitone As ComplexEquidistantMultitoneStruct = GetComplexEquidistantMultitone(quantum, numberOfTones, maximumNumberOfSamples, iqRate, initialPhase, powerLevel, _
             frequencyBetweenTones)

            ' Confirue NIRfsg session
            _rfsgSession.RF.Configure(centerFrequency, complexEquidistantMultitone.signalPowerLevel)
            ' Power Level calculated at rfsg_ComplexEquidistantMultitone

            _rfsgSession.Arb.SignalBandwidth = complexEquidistantMultitone.signalBandwidth
            ' Signal Bandwidth calculated by rfsg_ComplexEquidistantMultitone
            ' Write generated waveform
            _rfsgSession.Arb.WriteWaveform(String.Empty, complexEquidistantMultitone.IDataBuffer, complexEquidistantMultitone.QDataBuffer)

            ' Display calculated data
            actualIQRateTextBox.Text = actualIQRate.ToString()
            actualFrequencyBetweenTonesTextBox.Text = complexEquidistantMultitone.actualFrequencyBetweenTones.ToString()

            ' Initiate Generation
            _rfsgSession.Initiate()

            ' Read NIRfsg sessions attribute 
            peakEnvelopePower = _rfsgSession.RF.Advanced.PeakEnvelopePower
            ' Display Peak Env. Power on GUI indicator
            actualPeakPowerTextBox.Text = peakEnvelopePower.ToString()

            ' Enable timer
            EnableControls(False)
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
        End Try
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = e.Message
    End Sub

    Private Shared Function GetComplexEquidistantMultitone(ByVal quantum As Integer, ByVal numberOfTones As Integer, ByVal maximumNumberOfSamples As Integer, _
     ByVal iqRate As Double, ByVal initialPhase As Double, ByVal powerLevelPerTone As Double, ByVal frequencyBetweenTones As Double) As ComplexEquidistantMultitoneStruct
        Dim actualFrequencyBetweenTones As Double
        Dim evenNumberOfGaps As Boolean
        Dim i As Integer
        Dim numberOfSamples As Integer
        Dim actualFrequency As Double
        Dim desiredFrequency As Double
        Dim fractionalIndex As Double
        Dim frequencyTolerance As Double = 10.0
        Dim toneList As ComplexMultitoneStruct()

        fractionalIndex = (numberOfTones - 1) * 0.5

        ' Even number of tones?
        If numberOfTones Mod 2 = 1 Then
            ' & 0x01)
            desiredFrequency = frequencyBetweenTones
            evenNumberOfGaps = True
        Else
            desiredFrequency = frequencyBetweenTones / 2
            evenNumberOfGaps = False
        End If

        FindWaveformParamsForFrequency(quantum, 100, maximumNumberOfSamples, iqRate, desiredFrequency, frequencyTolerance, numberOfSamples, actualFrequency)

        If evenNumberOfGaps Then
            actualFrequencyBetweenTones = actualFrequency
        Else
            actualFrequencyBetweenTones = actualFrequency * 2
        End If

        toneList = New ComplexMultitoneStruct(numberOfTones - 1) {}
        For i = 0 To numberOfTones - 1
            ' Add tones to the list
            toneList(i) = New ComplexMultitoneStruct((i - fractionalIndex) * actualFrequencyBetweenTones, powerLevelPerTone, initialPhase)
        Next

        ' Build IQ Data arrays
        GetComplexEquidistantMultitone = GetComplexMultitone(toneList, False, numberOfTones, numberOfSamples, iqRate)
        GetComplexEquidistantMultitone.actualFrequencyBetweenTones = actualFrequencyBetweenTones

        Return GetComplexEquidistantMultitone
    End Function

    Private Shared Function GetComplexMultitone(ByVal complexMultitone As ComplexMultitoneStruct(), ByVal mirrorImage As Boolean,
                                         ByVal numberOfTones As Integer, ByVal samples As Integer, ByVal sampleRate As Double) As ComplexEquidistantMultitoneStruct
        Dim complexEquidistantMultitone As New ComplexEquidistantMultitoneStruct(0, Nothing, Nothing, 0, -100, 0)
        Dim IDataBuffer As Double()
        Dim QDataBuffer As Double()
        Dim i As Integer
        Dim maximumFrequency As Double = 0
        Dim maximumFrequencyTemp As Double = 0
        Dim referenceFrequency As Double
        Dim powerDB As Double
        Dim totalPower As Double = 0
        Dim minimumFrequencyStep As Double
        Dim numberOfCycles As Double
        Dim phaseInRadians As Double
        Dim gradesToRadians As Double
        Dim minimumPhaseIncrement As Double
        Dim frequency As Double()
        Dim power As Double()
        Dim initialPhase As Double()

        If samples <= 0 Then
            Throw New ArgumentOutOfRangeException("samples", "The number of samples must be greater than 0")
        End If
        If numberOfTones <= 0 Then
            Throw New ArgumentOutOfRangeException("numberOfTones", "You must enter at least one tone")
        End If
        ' Allocate local resources
        IDataBuffer = New Double(samples - 1) {}
        QDataBuffer = New Double(samples - 1) {}

        ' Allocate buffers
        frequency = New Double(numberOfTones - 1) {}
        power = New Double(numberOfTones - 1) {}
        initialPhase = New Double(numberOfTones - 1) {}

        ' Build data arrays
        For i = 0 To numberOfTones - 1
            frequency(i) = complexMultitone(i).frequency
            power(i) = complexMultitone(i).power
            initialPhase(i) = complexMultitone(i).initialPhase
        Next

        ' Initialize arrays to 0
        IDataBuffer = Enumerable.Repeat(0.0, samples).ToArray()
        QDataBuffer = Enumerable.Repeat(0.0, samples).ToArray()

        referenceFrequency = sampleRate * 0.4
        minimumFrequencyStep = sampleRate / samples
        gradesToRadians = Math.PI * 180
        For i = 0 To numberOfTones - 1
            If frequency(i) >= referenceFrequency Then
                Throw New ArgumentOutOfRangeException("complexMultitone", "Tone frequencies must be less than .4 * Sample rate")
            End If
            If (frequency(i) - (minimumFrequencyStep * Math.Floor(frequency(i) / minimumFrequencyStep))) <> 0 Then
                Throw New ArgumentException("Frequencies must be multiple of sample rate")
            End If
            ' Keep track of the maximum Frequency to determine the bandwidth
            maximumFrequencyTemp = frequency(i)
            If maximumFrequencyTemp < 0 Then
                maximumFrequencyTemp *= -1
            End If
            If maximumFrequencyTemp > maximumFrequency Then
                maximumFrequency = maximumFrequencyTemp
            End If


            phaseInRadians = initialPhase(i) * gradesToRadians
            powerDB = power(i) / 10
            powerDB = Math.Pow(10, powerDB)
            totalPower += powerDB
            powerDB = Math.Sqrt(powerDB)

            numberOfCycles = frequency(i) / minimumFrequencyStep
            minimumPhaseIncrement = (2 * Math.PI) / (samples / numberOfCycles)
            minimumPhaseIncrement = (2 * Math.PI * numberOfCycles) - minimumPhaseIncrement

            For j As Integer = 0 To samples - 1
                Dim phase As Double = phaseInRadians + j * (minimumPhaseIncrement / (samples - 1))
                IDataBuffer(j) += powerDB * Math.Cos(phase)
                QDataBuffer(j) += powerDB * Math.Sin(phase)
            Next
        Next
        If mirrorImage Then
            ' Mirror the image. Set Q Data buffer to 0
            QDataBuffer = Enumerable.Repeat(0.0, samples).ToArray()
        End If

        complexEquidistantMultitone.bufferSize = samples
        complexEquidistantMultitone.IDataBuffer = IDataBuffer
        complexEquidistantMultitone.QDataBuffer = QDataBuffer
        complexEquidistantMultitone.signalBandwidth = maximumFrequency * 2
        complexEquidistantMultitone.signalPowerLevel = Math.Log10(totalPower) * 10
        Return complexEquidistantMultitone
    End Function

    Private Shared Function FindWaveformParamsForFrequency(ByVal quantum As Integer, ByVal minimumNumberOfSamples As Integer, ByVal maximumNumberOfSamples As Integer, ByVal IQRate As Double, ByVal desiredFrequency As Double, ByVal frequencyTolerance As Double, _
     ByRef numberOfSamples As Integer, ByRef actualFrequency As Double) As Integer
        ' Local Var.
        Dim updateFrequency As Boolean
        Dim positiveFrequency As Boolean
        Dim restrictionsMet As Boolean
        Dim lessThanMaxNumberOfSamples As Boolean
        Dim i As Integer = 0
        Dim coercedQuantum As Integer
        Dim newFrequency As Double = 0
        Dim oldError As Double = 0
        Dim newError As Double = 0

        ' Var. used for feedback
        Dim firstFoundYet As Boolean = False
        Dim numberOfSamplesTemp As Integer = 0
        Dim frequencyErrorTemp As Double = 0
        Dim actualFrequencyTemp As Double = Double.NegativeInfinity
        ' Inf. (Max negative real 64)
        ' Check if inputs are valid
        ValidateInputs(quantum, minimumNumberOfSamples, maximumNumberOfSamples, IQRate, desiredFrequency)

        ' Initial values
        Do
            i += 1
            coercedQuantum = CoerceToQuantum(CInt(IQRate / desiredFrequency * i), quantum)

            lessThanMaxNumberOfSamples = coercedQuantum <= maximumNumberOfSamples
            ' Stop condition 2
            positiveFrequency = desiredFrequency > 0
            ' Stop condition 3
            updateFrequency = (coercedQuantum >= minimumNumberOfSamples) AndAlso lessThanMaxNumberOfSamples

            newFrequency = (IQRate / coercedQuantum)
            newFrequency *= i
            newError = desiredFrequency - newFrequency
            If newError < 0 Then
                ' Get the Absolute Value
                newError *= -1
            End If
            oldError = actualFrequencyTemp - desiredFrequency
            If oldError < 0 Then
                ' Get the Absolute Value
                oldError *= -1
            End If
            If Not ((oldError > newError) AndAlso updateFrequency) Then
                ' Update the Frequency if the error is reduced
                ' Keep values for: Num. of cycles, Num. of samples, fist found yet and act. Frequency 
                frequencyErrorTemp = oldError
            Else
                numberOfSamplesTemp = coercedQuantum
                firstFoundYet = True
                actualFrequencyTemp = newFrequency
                frequencyErrorTemp = newError
            End If
            ' Stop condition 1
            restrictionsMet = (frequencyErrorTemp <= frequencyTolerance) AndAlso firstFoundYet
        Loop While Not restrictionsMet AndAlso positiveFrequency AndAlso lessThanMaxNumberOfSamples

        numberOfSamples = numberOfSamplesTemp
        actualFrequency = actualFrequencyTemp

        If Not restrictionsMet Then
            Throw New ArgumentException("The restrictions were not met")
        End If
        Return 0
    End Function

    Private Shared Function CoerceToQuantum(ByVal samples As Integer, ByVal quantum As Integer) As Integer
        If quantum = 0 Then
            Return 0
        End If
        If samples >= quantum Then
            Return quantum * CInt(Math.Round(CDbl(samples / quantum), 0))
        Else
            Return quantum
        End If
    End Function

    Private Shared Sub ValidateInputs(ByVal quantum As Integer, ByVal minimumNumberOfSamples As Integer, ByVal maximumNumberOfSamples As Integer, ByVal IQRate As Double, ByVal desiredFrequency As Double)
        If (IQRate < 1) OrElse (IQRate < (desiredFrequency * 2)) OrElse (desiredFrequency < 1) OrElse (quantum < 1) OrElse (minimumNumberOfSamples > maximumNumberOfSamples) Then
            Throw New ArgumentException("Invalid settings")
        End If
    End Sub

    Private Structure ComplexMultitoneStruct
        Public frequency As Double
        Public power As Double
        Public initialPhase As Double

        Public Sub New(ByVal frequency As Double, ByVal power As Double, ByVal initialPhase As Double)
            Me.frequency = frequency
            Me.power = power
            Me.initialPhase = initialPhase
        End Sub
    End Structure

    Private Structure ComplexEquidistantMultitoneStruct
        Public bufferSize As Integer
        Public IDataBuffer As Double()
        Public QDataBuffer As Double()
        Public signalBandwidth As Double
        Public signalPowerLevel As Double
        Public actualFrequencyBetweenTones As Double

        Public Sub New(ByVal bufferSize As Integer, ByVal IDataBuffer As Double(), ByVal QDataBuffer As Double(), ByVal signalBandwidth As Double, ByVal signalPowerLevel As Double, ByVal actualFrequencyBetweenTones As Double)
            Me.bufferSize = bufferSize
            Me.IDataBuffer = IDataBuffer
            Me.QDataBuffer = QDataBuffer
            Me.signalBandwidth = signalBandwidth
            Me.signalPowerLevel = signalPowerLevel
            Me.actualFrequencyBetweenTones = actualFrequencyBetweenTones
        End Sub
    End Structure

    Private Sub StopGeneration()
        ' Stop the timer
        EnableControls(True)

        Try
            If _rfsgSession IsNot Nothing Then
                ' Disable the output. This sets the noise floor as low as possible
                _rfsgSession.RF.OutputEnabled = False

                ' Unsubscribe from warning events
                RemoveHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

                ' Close the RFSG NIRfsg session
                _rfsgSession.Close()
            End If
            ' Clear handle value
            _rfsgSession = Nothing
        Catch ex As Exception
            errorTextBox.Text = "Error in StopGeneration(): " + ex.Message
        End Try
    End Sub

#End Region

#Region "Form Events"
    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        ' Stop generation on exit
        StopGeneration()
        ' Quit user interface on exit
    End Sub

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        ' Start
        StartGeneration()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles stopButton.Click
        ' Stop
        StopGeneration()
    End Sub

    Private Sub rfsgStatusTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles rfsgStatusTimer.Tick
        ' Check RFSG status every timer event (100ms)
        CheckGeneration()
    End Sub
#End Region

    Private Sub EnableControls(ByVal enabled As Boolean)
        startButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        resourceNameComboBox.Enabled = enabled
        numberOfTonesNumeric.Enabled = enabled
        maximumNumberOfSamplesNumeric.Enabled = enabled
        iqRateNumeric.Enabled = enabled
        powerPerToneNumeric.Enabled = enabled
        centerFrequencyNumeric.Enabled = enabled
        initialPhaseNumeric.Enabled = enabled
        frequencyBetweenTonesNumeric.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
