'==================================================================================================
' Title        : Multitone Arbitrary Spacing
' Description  : This example demonstrates how to generate arbitrarily separated tones.
'			     
'			     The example lets you create a list of tones with a defined offset,a power level
'			     and an initial phase.
'			     
'			     To add a tone:
'			        -Set the Tone Name with a name to identify the tone.
'			        -Set the Offset with the required frequency for the tone.
'			        -Set the Power Level with the specific level for the tone.
'			        -Set the Initial Phase for the tone.
'			        -Click the Add button.
'			        -The Tone Name is added to the Tone List.
'
'			     To display the properties of a tone from the Tone List:
'			        -Select the tone from the Tone List by clicking it.
'			        -Click the Load button.
'			        -The properties of the selected tone are displayed.
'	
'			     To delete a tone frome the Tone List:
'			        -Select the tone from the Tone List by clicking it.
'			        -Click the Del (Delete) button.
'			        -The tone is removed from the Tone List.
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
Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg
    Private _toneList As New List(Of ComplexMultitoneStruct)()

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
        Dim mirrorImage As Boolean
        Dim maximumNumberOfSamples As Integer = 0
        Dim IQRate As Double = 0
        Dim centerfrequency As Double = 0
        Dim actualIQRate As Double = 0
        Dim peakEnvelopePower As Double = 0

        Try
            ' Clear previous errors
            errorTextBox.Text = "No error."

            ' Read controls from GUI
            resourceName = resourceNameComboBox.Text
            maximumNumberOfSamples = CInt(numberOfSamplesNumeric.Value)
            IQRate = CDbl(iqRateNumeric.Value)
            centerfrequency = CDbl(centerFrequencyNumeric.Value)
            mirrorImage = mirrorImageCheckBox.Checked

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the NIRfsg session
            ' Configure generation mode
            ' Set generation mode to Arb Waveform           
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform
            ' Set to change the IQ Rate
            _rfsgSession.Arb.IQRate = IQRate
            ' Set to change the Pre-filter gain
            _rfsgSession.Arb.PreFilterGain = -2
            ' Read NIRfsg sessions configuration 
            ' Set to read the IQ Rate
            actualIQRate = _rfsgSession.Arb.IQRate
            ' Generate waveform
            Dim complexEquidistantMultitone As ComplexEquidistantMultitoneStruct = GetComplexMultitone(_toneList, mirrorImage, maximumNumberOfSamples, IQRate)

            ' Configure NIRfsg session
            _rfsgSession.RF.Configure(centerfrequency, complexEquidistantMultitone.signalPowerLevel)
            _rfsgSession.Arb.SignalBandwidth = complexEquidistantMultitone.signalBandwidth

            ' Write generated waveform
            _rfsgSession.Arb.WriteWaveform(String.Empty, complexEquidistantMultitone.IDataBuffer, complexEquidistantMultitone.QDataBuffer)

            ' Display calculated data
            actualIQRateTextBox.Text = actualIQRate.ToString()
            actualPowerLevelTextBox.Text = complexEquidistantMultitone.signalPowerLevel.ToString()

            ' Initiate Generation
            _rfsgSession.Initiate()

            ' Read NIRfsg sessions attribute 
            ' Set to read the Peak Env. power
            peakEnvelopePower = _rfsgSession.RF.Advanced.PeakEnvelopePower
            ' Display Peak Env. Power on GUI indicator
            actualPeakPowerTextBox.Text = peakEnvelopePower.ToString()

            ' Enable timer
            EnableControls(False)
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
        End Try
    End Sub

    Private Shared Function GetComplexMultitone(ByVal complexMultitoneList As List(Of ComplexMultitoneStruct), ByVal mirrorImage As Boolean, ByVal samples As Integer, ByVal sampleRate As Double) As ComplexEquidistantMultitoneStruct
        Dim complexEquidistantMultitone As New ComplexEquidistantMultitoneStruct(0, Nothing, Nothing, 0, -100)
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
            Throw New ArgumentException("The number of samples must be greater than 0.")
        End If
        If complexMultitoneList.Count <= 0 Then
            Throw New ArgumentException("You must enter at least one tone.")
        End If
        ' Allocate local resources
        IDataBuffer = New Double(samples - 1) {}
        QDataBuffer = New Double(samples - 1) {}

        ' Allocate buffers
        frequency = New Double(complexMultitoneList.Count - 1) {}
        power = New Double(complexMultitoneList.Count - 1) {}
        initialPhase = New Double(complexMultitoneList.Count - 1) {}

        ' Build data arrays
        For i = 0 To complexMultitoneList.Count - 1
            frequency(i) = complexMultitoneList(i).frequency
            power(i) = complexMultitoneList(i).power
            initialPhase(i) = complexMultitoneList(i).initialPhase
        Next

        ' Initialize arrays to 0
        IDataBuffer = Enumerable.Repeat(0.0, samples).ToArray()
        QDataBuffer = Enumerable.Repeat(0.0, samples).ToArray()

        referenceFrequency = sampleRate * 0.4
        minimumFrequencyStep = sampleRate / samples
        gradesToRadians = Math.PI * 180
        For i = 0 To complexMultitoneList.Count - 1
            If frequency(i) >= referenceFrequency Then
                Throw New ArgumentException("Tone frequencies must be less than 0.4 * Sample rate.")
            End If
            If (frequency(i) - (minimumFrequencyStep * Math.Floor(frequency(i) / minimumFrequencyStep))) <> 0 Then
                Throw New ArgumentException("Frequencies must be multiple of sample rate.")
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

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = e.Message
    End Sub

    Private Sub StopGeneration()
        ' Stop the timer
        EnableControls(True)

        Try
            If _rfsgSession IsNot Nothing Then
                ' Disable the output. This sets the noise floor as low as possible
                _rfsgSession.RF.OutputEnabled = False

                ' Unsubscribe from Rfsg warnings
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
    Private Sub loadButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles loadButton.Click
        Dim index As Integer

        ' Get the selected item to load settings from
        index = toneListListBox.SelectedIndex
        If index >= 0 AndAlso index < _toneList.Count Then
            ' Read the items label
            ' Read item from tone list
            toneNameTextBox.Text = toneListListBox.SelectedItem.ToString()
            offsetNumeric.Value = CDec(_toneList(index).frequency)
            powerLevelNumeric.Value = CDec(_toneList(index).power)
            initialPhaseNumeric.Value = CDec(_toneList(index).initialPhase)
        End If
    End Sub

    Private Sub deleteButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles deleteButton.Click
        Dim index As Integer
        ' Get the selected item to delete
        index = toneListListBox.SelectedIndex
        If index >= 0 Then
            ' Delete item from list
            _toneList.RemoveAt(index)
            toneListListBox.Items.RemoveAt(index)
        End If
    End Sub

    Private Sub addButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles addButton.Click
        Dim name As String
        Dim frequency As Double
        Dim power As Double
        Dim initialPhase As Double
        name = toneNameTextBox.Text
        frequency = CDbl(offsetNumeric.Value)
        power = CDbl(powerLevelNumeric.Value)
        initialPhase = CDbl(initialPhaseNumeric.Value)

        ' Add tones to the list
        _toneList.Add(New ComplexMultitoneStruct(frequency, power, initialPhase))
        ' insert into list box
        toneListListBox.Items.Add(name)
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        ' Stop generation on exit
        StopGeneration()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        ' Stop
        StopGeneration()
    End Sub

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        ' Start
        StartGeneration()
    End Sub

    Private Sub rfsgStatusTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles rfsgStatusTimer.Tick
        ' Check RFSG status every timer event (100ms)
        CheckGeneration()
    End Sub
#End Region

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

        Public Sub New(ByVal bufferSize As Integer, ByVal IDataBuffer As Double(), ByVal QDataBuffer As Double(), ByVal signalBandwidth As Double, ByVal signalPowerLevel As Double)
            Me.bufferSize = bufferSize
            Me.IDataBuffer = IDataBuffer
            Me.QDataBuffer = QDataBuffer
            Me.signalBandwidth = signalBandwidth
            Me.signalPowerLevel = signalPowerLevel
        End Sub
    End Structure

    Private Sub EnableControls(ByVal enabled As Boolean)
        startButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        resourceNameComboBox.Enabled = enabled
        mirrorImageCheckBox.Enabled = enabled
        toneNameTextBox.Enabled = enabled
        numberOfSamplesNumeric.Enabled = enabled
        iqRateNumeric.Enabled = enabled
        centerFrequencyNumeric.Enabled = enabled
        offsetNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        initialPhaseNumeric.Enabled = enabled
        ' Start the status checking timer
        rfsgStatusTimer.Enabled = Not enabled

        addButton.Enabled = enabled
        deleteButton.Enabled = enabled
        loadButton.Enabled = enabled

        Application.DoEvents()
    End Sub
End Class
