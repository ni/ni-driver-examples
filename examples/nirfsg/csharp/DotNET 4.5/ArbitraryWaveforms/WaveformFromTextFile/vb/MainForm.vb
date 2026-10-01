'==================================================================================================
' Title        : Waveform From Text File
' Description  : This example demonstrates how to generate a sine signal\n\
'                using the Arbitrary Waveform capabilities of NI-RFSG.
'			     Note: In order to run this example, the upconverter must be configured with 
'			     an Arbitrary Waveform Generator. 
'			     To do this, open Measurement & Automation Explorer, select the upconverter 
'			     and click on properties.
'==================================================================================================

Imports System
Imports System.IO
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg
    Const WaveformName As String = "waveform"

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

    Private Sub StartGeneration()
        Try
            Dim resourceName As String
            Dim fileName As String
            Dim frequency As Double
            Dim frequencyOffset As Double
            Dim power As Double
            Dim iqRate As Double
            Dim actualIQRate As Double
            Dim preFilterGain As Double
            Dim signalBandwidth As Double
            Dim directDownload As Boolean
            Dim powerLevelType As RfsgRFPowerLevelType
            Dim iData As Double()
            Dim qData As Double()

            ' We are starting - set GUI ctrls. 
            generatingLed.BackColor = System.Drawing.SystemColors.Control
            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)
            iqRate = CDbl(iqRateNumeric.Value)
            directDownload = directDownloadCheckBox.Checked
            preFilterGain = CDbl(preGainNumeric.Value)
            powerLevelType = DirectCast([Enum].Parse(GetType(RfsgRFPowerLevelType), DirectCast(powerLevelTypeComboBox.SelectedItem, String)), RfsgRFPowerLevelType)
            fileName = pathTextBox.Text
            signalBandwidth = CDbl(signalBandwidthNumeric.Value)



            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the instrument 
            _rfsgSession.Arb.IQRate = iqRate

            _rfsgSession.RF.PowerLevelType = powerLevelType

            _rfsgSession.Arb.PreFilterGain = preFilterGain

            _rfsgSession.Arb.DataTransfer.DirectDownloadEnabled = directDownload

            _rfsgSession.RF.Configure(frequency, power)

            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform

            ' Get the actual IQ rate, and populate the GUI output 
            actualIQRate = _rfsgSession.Arb.IQRate

            ' Configure the signal bandwidth 
            _rfsgSession.Arb.SignalBandwidth = signalBandwidth

            ' Read the content of a text file into I and Q arrays
            Dim data As String = File.ReadAllText(fileName)
            Dim iqData As String() = data.Split(TryCast(Nothing, String()), StringSplitOptions.RemoveEmptyEntries)

            Dim numberOfSamples As Integer = iqData.Length / 2
            iData = New Double(numberOfSamples - 1) {}
            qData = New Double(numberOfSamples - 1) {}
            For i As Integer = 0 To numberOfSamples - 1
                iData(i) = Double.Parse(iqData(i * 2))
                qData(i) = Double.Parse(iqData(i * 2 + 1))
            Next

            _rfsgSession.Arb.WriteWaveform(WaveformName, iData, qData)

            frequencyOffset = actualIQRate / numberOfSamples

            actualIQRateTextBox.Text = actualIQRate.ToString()
            actualFrequencyOffsetTextBox.Text = frequencyOffset.ToString()

            ' Initiate Generation 
            _rfsgSession.Initiate()

            generatingLed.BackColor = Color.Lime

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

    Private Sub CheckGeneration()
        Try
            ' Check the status of the RFSG 
            _rfsgSession.CheckGenerationStatus()
        Catch ex As Exception
            ShowError("CheckGeneration()", ex)
        End Try
    End Sub

    Private Sub StopGeneration()
        ' Activate all stopped controls 
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

        generatingLed.BackColor = System.Drawing.SystemColors.Control
    End Sub

    Private Sub ShowError(ByVal functionName As String, ByVal exception As Exception)
        StopGeneration()
        errorTextBox.Text = "Error in " + functionName + ": " + exception.Message
    End Sub

#End Region

#Region "Form Events"

    Private Sub browseButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles browseButton.Click
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "Text Files (*.txt)|*.txt"
        openFileDialog.Title = "Select a waveform file..."
        openFileDialog.InitialDirectory = Path.GetDirectoryName(Path.GetFullPath(pathTextBox.Text))
        openFileDialog.FileName = Path.GetFileName(pathTextBox.Text)
        If openFileDialog.ShowDialog() = DialogResult.OK Then
            pathTextBox.Text = openFileDialog.FileName
        End If
        openFileDialog.Dispose()
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
        directDownloadCheckBox.Enabled = enabled
        pathTextBox.Enabled = enabled
        frequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        iqRateNumeric.Enabled = enabled
        preGainNumeric.Enabled = enabled
        powerLevelTypeComboBox.Enabled = enabled
        signalBandwidthNumeric.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
