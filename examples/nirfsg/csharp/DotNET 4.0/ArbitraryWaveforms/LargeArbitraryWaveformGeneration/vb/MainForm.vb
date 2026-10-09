'==================================================================================================
' Title        : Large Arbitrary Waveform Generation
' Description  : This example demonstrates how to write a large waveform to the NI-RFSG. 
'			     It shows how large waveforms can be written in pieces, therefore using less 
'			     memory.  This example generates a chirp waveform. 
'
'			     Note: In order to run this example, the upconverter must be configured with 
'			     an Arbitrary Waveform Generator. 
'			     To do this, open Measurement & Automation Explorer, select the upconverter 
'			     and click on properties.
'==================================================================================================

Imports System
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg
    Private _phase As Double = -90
    Const WaveformName As String = "waveform"
    Const WriteAtOnce As Integer = 200000

    Public Sub New()
        InitializeComponent()

        LoadRfsgDeviceNames()

        ConfigurePowerLevelTypeComboBox()
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

    Private Sub ConfigurePowerLevelTypeComboBox()
        powerLevelTypeComboBox.Items.AddRange([Enum].GetNames(GetType(RfsgRFPowerLevelType)))
        powerLevelTypeComboBox.SelectedIndex = 1
    End Sub

#End Region

#Region "Program Functions"

    Private Structure ChirpConfiguration
        Public triangleWaveformFrequency As Double
        Public triangleWaveformAmplitude As Double
        Public actualIQRate As Double
        Public actualChirpDuration As Double
        Public carrierFrequency As Double
        Public sign As Double
        Public totalNumberOfSamples As Integer
    End Structure

    Private Function ConfigureChirpForLargeWaveform(ByVal iqRate As Double, ByVal startFrequency As Double, ByVal stopFrequency As Double, ByVal chirpDuration As Double) As ChirpConfiguration
        Try
            Dim bandwidth As Double
            Dim maxWaveformSize As Integer, minWaveformSize As Integer, waveformQuantum As Integer
            Dim chirpConfiguration As ChirpConfiguration
            Dim modelType As String

            ' Set RFSG frequency (Carrier Freqency) to average of start and stop 
            ' frequency
            chirpConfiguration.carrierFrequency = (startFrequency + stopFrequency) / 2.0

            ' Find out if we should sweep the frequency range up (+1) or down (-1)
            chirpConfiguration.sign = If((startFrequency < stopFrequency), 1, -1)

            ' Get the signal bandwidth
            bandwidth = (stopFrequency - startFrequency) * chirpConfiguration.sign

            _rfsgSession.Arb.SignalBandwidth = bandwidth

            ' Set the IQ Rate 
            _rfsgSession.Arb.IQRate = iqRate

            ' Disable phase continuity. We don't need the driver to maintain
            ' phase continuity because the chirp waveform we're using is not 
            ' phase continuous (big discontinuity as we jump from stop back
            ' to start frequency).
            _rfsgSession.Arb.PhaseContinuityEnabled = RfsgPhaseContinuityEnabled.Disabled

            modelType = _rfsgSession.Identity.InstrumentModel

            If modelType.Equals("NI PXI-5670", StringComparison.OrdinalIgnoreCase) OrElse modelType.Equals("NI PXI-5671", StringComparison.OrdinalIgnoreCase) Then
                ' Disable digital equalization. For bandwidths greater than ~2MHz,
                ' you may want to turn this on to make the output power flatter across
                ' the signal bandwidth. _rfsgSession.Initiate function will execute faster
                ' with disabled equalization on 5670/71 devices.
                _rfsgSession.Arb.DigitalEqualizationEnabled = False
            End If

            ' Get the actual (coerced) IQ Rate 
            chirpConfiguration.actualIQRate = _rfsgSession.Arb.IQRate

            ' Get number of samples, coerce it to RFSG max/min waveform size
            maxWaveformSize = _rfsgSession.Arb.WaveformCapabilities.WaveformSizeMaximum
            minWaveformSize = _rfsgSession.Arb.WaveformCapabilities.WaveformSizeMinimum
            waveformQuantum = _rfsgSession.Arb.WaveformCapabilities.WaveformQuantum
            chirpConfiguration.totalNumberOfSamples = CInt(chirpConfiguration.actualIQRate * chirpDuration)
            If chirpConfiguration.totalNumberOfSamples > maxWaveformSize Then
                chirpConfiguration.totalNumberOfSamples = maxWaveformSize
            ElseIf chirpConfiguration.totalNumberOfSamples < minWaveformSize Then
                chirpConfiguration.totalNumberOfSamples = minWaveformSize
            End If
            ' Round the number of samples to the nearest multiple of RFSG quantum
            chirpConfiguration.totalNumberOfSamples = waveformQuantum * CInt(Math.Round(CDbl(chirpConfiguration.totalNumberOfSamples) / waveformQuantum, 0))
            chirpConfiguration.actualChirpDuration = chirpConfiguration.totalNumberOfSamples / chirpConfiguration.actualIQRate


            ' Get Parameters for creating chirp waveform.  We want a complex waveform 
            ' with the following characteristics:
            '       i)   constant power (so R = sqrt(I^2 + Q^2) is a constant)
            '       ii)  single tone, with frequency increasing (or decreasing) linearly 
            '            with time, so dq/dt = 2m*t, and q(t) = m*(t^2) for some m
            '       iii) f(t0) = Start Frequency Offset, f(t1) = Stop Frequency Offset, for 
            '            t0 = (-1) * Actual Chirp Duration / 2, 
            '            t1 = Actual Chirp Duration / 2.
            '            since f(t) = dq/dt (t) / 2pi, we can use ii) to rewrite f as 
            '            f(t) = (2m*t)/2pi
            '
            ' Calculate m (see text in 8 iii).  Stop Frequency Offset = f(t1) --> Stop 
            ' Frequency Offset = (2m*Actual Chirp Duration/2)/2pi
            ' --> m = (Stop Frequency Offset * 2pi)/Actual Chirp Duration.  Note we're actually 
            ' using Max Frequency Offset instead of Stop Frequency Offset.  Using Max Frequency Offset 
            ' prevents us from taking the square root of a negative number.  This assumes
            ' that we are sweeping the frequency up, so we may get sign of m wrong if 
            ' we're actually sweeping down.  We'll correct for this in step 11.
            '
            ' Eventually we'll generate q(t) = m*(t^2) by simply squaring a linear 
            ' waveform.  So rewrite as q(t) = (sqrt(m) * t)^2.  Inside the loop we 
            ' generate the (sqrt(m) * t) waveform, then square it.
            '
            ' Calculate the maximum value in the ramp in the waveform 
            '    y(t) = (sqrt(m) * t).  
            ' Half the ramp will be below zero, and the other half above, 
            ' and num samples / Sample Clock Rate is time, so 
            '    Ramp Max Val = sqrt(m) * ( (#samples/2) / Sample Clock Rate) ).
            '
            ' Calculate frequency (cycles/s) for the triangle waveform.  
            ' Sample Clock Rate / Samples in Waveform = Waveforms (cycles) per second.
            ' We don't actually want a triangle waveform.  We're using it to create 
            ' a ramp.

            chirpConfiguration.triangleWaveformFrequency = chirpConfiguration.actualIQRate / (2.0 * chirpConfiguration.totalNumberOfSamples)
            chirpConfiguration.triangleWaveformAmplitude = Math.Sqrt((bandwidth * Math.PI) / chirpConfiguration.actualChirpDuration) * (chirpConfiguration.totalNumberOfSamples / (2.0 * chirpConfiguration.actualIQRate))
            Return chirpConfiguration
        Catch
            Throw
        End Try
    End Function

    Private Shared Function TriangleWave(ByVal numberToWrite As Integer, ByVal amplitude As Double, ByVal frequency As Double, ByRef phase As Double) As Double()
        Dim waveform As Double() = New Double(numberToWrite - 1) {}
        If (phase < 0) Then
            phase += 360
        End If
        For i As Integer = 0 To numberToWrite - 1
            Dim p As Double = (phase + frequency * 360 * i) Mod 360
            If p >= 0 AndAlso p < 90 Then
                waveform(i) = 2 * amplitude * p / 180
            ElseIf p >= 90 AndAlso p < 270 Then
                waveform(i) = 2 * amplitude * (1 - p / 180)
            Else
                waveform(i) = 2 * amplitude * (p / 180 - 2)
            End If
        Next
        phase = (phase + frequency * 360 * numberToWrite) Mod 360
        Return waveform
    End Function

    Private Sub StartGeneration()
        Try
            Dim resourceName As String
            Dim startFrequency As Double, stopFrequency As Double
            Dim chirpDuration As Double
            Dim power As Double
            Dim iqRate As Double
            Dim powerLevelType As RfsgRFPowerLevelType
            Dim directDownload As Boolean
            Dim samplesRemaining As Integer
            Dim iData As Double(), qData As Double()
            Dim moreToCome As Boolean = True
            Dim chirpConfiguration As ChirpConfiguration

            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            startFrequency = CDbl(startFrequencyNumeric.Value)
            stopFrequency = CDbl(stopFrequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)
            powerLevelType = DirectCast([Enum].Parse(GetType(RfsgRFPowerLevelType), DirectCast(powerLevelTypeComboBox.Text, String)), RfsgRFPowerLevelType)
            directDownload = directDownloadCheckBox.Checked
            iqRate = CDbl(iqRateNumeric.Value)
            chirpDuration = CDbl(iqChirpDurationNumeric.Value)

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform

            ' Configure the instrument 
            chirpConfiguration = ConfigureChirpForLargeWaveform(iqRate, startFrequency, stopFrequency, chirpDuration)

            _rfsgSession.RF.Configure(chirpConfiguration.carrierFrequency, power)

            actualIQRateTextBox.Text = chirpConfiguration.actualIQRate.ToString()
            actualIQNumberOfSamplesTextBox.Text = chirpConfiguration.totalNumberOfSamples.ToString()
            actualIQChirpDurationTextBox.Text = chirpConfiguration.actualChirpDuration.ToString()

            _rfsgSession.RF.PowerLevelType = powerLevelType

            _rfsgSession.Arb.PreFilterGain = -2.0 ' -2.0 dB 
            _rfsgSession.Arb.DataTransfer.DirectDownloadEnabled = directDownload

            _rfsgSession.Arb.AllocateWaveform(WaveformName, chirpConfiguration.totalNumberOfSamples)


            ' Activate stop button (so that the user can stop the waveform upload) 
            stopButton.Enabled = True
            stopButton.Focus()
            samplesRemaining = chirpConfiguration.totalNumberOfSamples

            iData = New Double(WriteAtOnce - 1) {}
            qData = New Double(WriteAtOnce - 1) {}

            Dim waveform As Double()
            Do
                Dim size As Integer = WriteAtOnce

                If size > samplesRemaining Then
                    size = samplesRemaining
                    moreToCome = False
                    ReDim Preserve iData(size - 1)
                    ReDim Preserve qData(size - 1)
                End If

                waveform = TriangleWave(size, chirpConfiguration.triangleWaveformAmplitude, chirpConfiguration.triangleWaveformFrequency / chirpConfiguration.actualIQRate, _phase)

                For i As Integer = 0 To size - 1
                    Dim element As Double = chirpConfiguration.sign * waveform(i) * waveform(i)
                    iData(i) = Math.Cos(element)
                    qData(i) = Math.Sin(element)
                Next

                samplesRemaining -= size

                ' If the stop button has been pressed, the NIRfsg session will be null. In 
                ' that case, we must exit this function;
                If _rfsgSession Is Nothing Then
                    Return
                End If

                _rfsgSession.Arb.WriteWaveform(WaveformName, iData, qData)
                ' Update the Progress bar 
                generationStatusProgressBar.Value = CInt(100 - samplesRemaining / (chirpConfiguration.totalNumberOfSamples * 0.01))
            Loop While moreToCome

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Turn on the light
            generatingLed.BackColor = Color.Lime

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
            generatingLed.BackColor = System.Drawing.SystemColors.Control
            ' generation failed
            ShowError("CheckGeneration()", ex)
        End Try
    End Sub

    Private Sub StopGeneration()
        ' Reset the phase in case another waveform is going to be written
        _phase = -90

        ' Stop the status checking timer 
        EnableControls(True)

        Try
            If _rfsgSession IsNot Nothing Then
                ' Disable the output.  This sets the noise floor as low as possible.
                _rfsgSession.RF.OutputEnabled = False

                ' Unsubscribe from warning events
                RemoveHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

                ' Close the NIRfsg session
                _rfsgSession.Close()
            End If
            _rfsgSession = Nothing
        Catch ex As Exception
            errorTextBox.Text = "Error in StopGeneration(): " + ex.Message
        End Try

        ' Activate all stopped controls 
        generatingLed.BackColor = System.Drawing.SystemColors.Control
        ' generation stopped
        generationStatusProgressBar.Value = 0
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
        resourceNameComboBox.Enabled = enabled
        startFrequencyNumeric.Enabled = enabled
        stopFrequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        powerLevelTypeComboBox.Enabled = enabled
        directDownloadCheckBox.Enabled = enabled
        iqRateNumeric.Enabled = enabled
        iqChirpDurationNumeric.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
