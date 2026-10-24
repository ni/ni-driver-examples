'==================================================================================================
' Title        : Forward Frequency Sweep (5672In-bandRetuning)
' Description  : This example demonstrates how to generate a frequency sweep 
'			     at a specified power level as well as how to use the
'			     in-band retuning feature through the use of the
'			     upconverter center frequency attribute.
'
'			     Note: In order to run this example, you must have your 
'			     NI PXI-5610 configured to work together with a NI PXI-5442(AWG) 
'			     To do this, open the Measurements & Automation Explorer, 
'			     select the NI PXI-5610 and click on properties.
'==================================================================================================

Imports System
Imports System.Threading
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg
    Private _stopFrequency As Double, _currentFrequency As Double, _frequencyIncrement As Double
    Private _upconverterCenterFrequency As Double, _halfOfEffectiveSpan As Double, _dwellTime As Double
    Private _phaseDetectorFrequency As Double
    Const ErrorCantReachFrequency As String = "The current frequency cannot be reached from the Upconverter " & vbCr & vbLf & "                                                    Center Frequency with the current phase detector frequency, " & vbCr & vbLf & "                                                    in-band retuning span and # of steps."

    Public Sub New()
        InitializeComponent()

        LoadRfsgDeviceNames()

        ConfigurePhaseDetectorFrequencyComboBox()
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

    Private Sub ConfigurePhaseDetectorFrequencyComboBox()
        phaseDetectorFrequencyComboBox.Items.Add(1000000.0)
        phaseDetectorFrequencyComboBox.Items.Add(5000000.0)
        phaseDetectorFrequencyComboBox.SelectedIndex = 0
    End Sub

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim power As Double
        Dim startFrequency As Double
        Dim inbandSpan As Double

        Dim numberOfSteps As Integer
        Try
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            power = CDbl(powerLevelNumeric.Value)
            startFrequency = CDbl(startFrequencyNumeric.Value)
            _stopFrequency = CDbl(stopFrequencyNumeric.Value)
            numberOfSteps = CInt(numberStepsNumeric.Value)
            _dwellTime = CDbl(dwellTimeNumeric.Value)
            inbandSpan = CDbl(deviceBandwidthToUseNumeric.Value)
            _phaseDetectorFrequency = Double.Parse(phaseDetectorFrequencyComboBox.Text)

            _currentFrequency = startFrequency
            _frequencyIncrement = (_stopFrequency - startFrequency) / (numberOfSteps - 1)

            ' 100Hz is the default signal bandwidth for CW mode 
            _halfOfEffectiveSpan = (inbandSpan - 100) / 2

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the device 
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the generation mode 
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave

            ' Configure the starting frequency and power level 
            _rfsgSession.RF.Configure(startFrequency, power)

            ' Start the frequency sweep timer 
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

    Private Sub SetNextSweepFrequency()
        Try
            Dim nextPotentialUCF As Double

            If _currentFrequency > _stopFrequency Then
                StopGeneration()
            Else
                actualCurrentFrequencyTextBox.Text = _currentFrequency.ToString()

                ' Check to see if any part of the new frequency will move past the band allowed by the
                ' upconverter center frequency (UCF)
                If (_currentFrequency - _halfOfEffectiveSpan) > _upconverterCenterFrequency Then
                    ' If the frequency is not in range, pick the next UCF and check to make sure that the new
                    ' frequency can be reached from the new UCF.  Depending on the phase detector frequency,
                    ' in-band span and step size of the sweep, the new frequency might be out of reach of the UCF.
                    nextPotentialUCF = _currentFrequency + _halfOfEffectiveSpan
                    nextPotentialUCF = Math.Floor(nextPotentialUCF / _phaseDetectorFrequency) * _phaseDetectorFrequency

                    If _currentFrequency > (nextPotentialUCF + _halfOfEffectiveSpan) Then
                        Throw New ArgumentException(ErrorCantReachFrequency)
                    End If

                    _rfsgSession.RF.Upconverter.CenterFrequency = nextPotentialUCF
                    _rfsgSession.RF.Frequency = _currentFrequency
                    _upconverterCenterFrequency = _rfsgSession.RF.Upconverter.CenterFrequency
                Else
                    ' If the frequency is within range, only the frequency needs to be set 
                    _rfsgSession.RF.Frequency = _currentFrequency
                End If

                ' Configure the starting frequency and power level 
                _rfsgSession.Initiate()

                ' Dwell the specified time at the current frequency.  Add code in here to take a measurement of the signal. 
                Thread.Sleep(CInt(_dwellTime * 1000))

                ' Abort the generation 
                _rfsgSession.Abort()

                _currentFrequency += _frequencyIncrement

                Application.DoEvents()
            End If
        Catch ex As Exception
            ShowError("SetNextSweepFrequency()", ex)
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
        SetNextSweepFrequency()
    End Sub
#End Region

    Private Sub EnableControls(ByVal enabled As Boolean)
        startButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        resourceNameComboBox.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        startFrequencyNumeric.Enabled = enabled
        stopFrequencyNumeric.Enabled = enabled
        numberStepsNumeric.Enabled = enabled
        dwellTimeNumeric.Enabled = enabled
        deviceBandwidthToUseNumeric.Enabled = enabled
        phaseDetectorFrequencyComboBox.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
