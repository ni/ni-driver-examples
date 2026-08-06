'==================================================================================================
' Title        : 565x Digital Modulation
' Description  : This example demonstrates the use of 565x family of NI RF Signal Generators.
'			     The example shows how to generate a continuous wave signal, and optionally apply
'			     digital modulation.
'==================================================================================================

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg
    Private _userDefinedWaveformString As String()
    Private _userDefinedWaveformBytes As Byte()
    Const FrequencyReferenceRate As Double = 10000000.0

    <STAThread()>
    Shared Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New MainForm())
    End Sub

    Public Sub New()
        InitializeComponent()

        LoadRfsgDeviceNames()

        EnableControls(True)

        ConfigureMessageWaveformTypeComboBox()
        ConfigureModulationTypeComboBox()
        ConfigureFrequencyReferenceSourceComboBox()
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

    Private Sub ConfigureMessageWaveformTypeComboBox()
        messageWaveformTypeComboBox.Items.AddRange([Enum].GetNames(GetType(RfsgDigitalModulationWaveformType)))
        messageWaveformTypeComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureModulationTypeComboBox()
        RemoveHandler modulationTypeComboBox.SelectedIndexChanged, AddressOf Me.modulationTypeComboBox_SelectedIndexChanged
        modulationTypeComboBox.Items.AddRange([Enum].GetNames(GetType(RfsgDigitalModulationType)))
        AddHandler modulationTypeComboBox.SelectedIndexChanged, AddressOf Me.modulationTypeComboBox_SelectedIndexChanged
        modulationTypeComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureFrequencyReferenceSourceComboBox()
        Dim refSourceValueList = New List(Of DictionaryEntry)()
        refSourceValueList.Add(New DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn))
        refSourceValueList.Add(New DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock))
        refSourceValueList.Add(New DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock))
        refSourceValueList.Add(New DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn))

        frequencyReferenceSourceComboBox.DataSource = refSourceValueList
        frequencyReferenceSourceComboBox.DisplayMember = "Key"
        frequencyReferenceSourceComboBox.ValueMember = "Value"
        frequencyReferenceSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.OnboardClock
    End Sub

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim frequencyReferenceSource As String = String.Empty
        Dim centerFrequency As Double
        Dim powerLevel As Double
        Dim modulationType As RfsgDigitalModulationType
        Dim messageWaveformType As RfsgDigitalModulationWaveformType
        Dim messageSymbolRate As Double
        Dim prbsOrder As Integer
        Dim fskDeviation As Double
        Try

            ' Read in all the control values
            resourceName = resourceNameComboBox.Text
            frequencyReferenceSource = DirectCast(frequencyReferenceSourceComboBox.Text, String)
            centerFrequency = CDbl(frequencyNumeric.Value)
            powerLevel = CDbl(powerLevelNumeric.Value)
            modulationType = DirectCast([Enum].Parse(GetType(RfsgDigitalModulationType), DirectCast(modulationTypeComboBox.SelectedItem, String)), RfsgDigitalModulationType)
            messageWaveformType = DirectCast([Enum].Parse(GetType(RfsgDigitalModulationWaveformType), DirectCast(messageWaveformTypeComboBox.SelectedItem, String)), RfsgDigitalModulationWaveformType)
            messageSymbolRate = CDbl(messageSymbolRateNumeric.Value)
            prbsOrder = CInt(messagePrbsOrderNumeric.Value)
            fskDeviation = CDbl(fskDeviationNumeric.Value)

            ' Convert the user-defined waveform string into an array of integers.
            _userDefinedWaveformString = messageUserDefinedWaveformTextBox.Text.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
            _userDefinedWaveformBytes = New Byte(_userDefinedWaveformString.Length - 1) {}
            For i As Integer = 0 To _userDefinedWaveformString.Length - 1
                _userDefinedWaveformBytes(i) = Byte.Parse(_userDefinedWaveformString(i))
            Next

            ' Update status box
            errorTextBox.Text = "Initializing..."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            If _rfsgSession Is Nothing Then
                _rfsgSession = New NIRfsg(resourceName, True, False)

                ' Subscribe to Rfsg warnings
                AddHandler _rfsgSession.DriverOperation.Warning, AddressOf DriverOperation_Warning
            End If

            ' Configure the instrument
            _rfsgSession.RF.Configure(centerFrequency, powerLevel)
            _rfsgSession.FrequencyReference.Configure(frequencyReferenceSource, FrequencyReferenceRate)

            ' Set the modulation parameters
            ' We must set all the parameters regardless of modulation type so that
            ' when we change the modulation type on-the-fly, we do not have to also
            ' set the other modulation parameters.
            _rfsgSession.Modulation.Digital.ModulationType = modulationType
            _rfsgSession.Modulation.Digital.WaveformType = messageWaveformType
            _rfsgSession.Modulation.Digital.SymbolRate = messageSymbolRate
            _rfsgSession.Modulation.Digital.PrbsOrder = prbsOrder
            _rfsgSession.Modulation.Digital.PrbsSeed = 1
            _rfsgSession.Modulation.Digital.ConfigureUserDefinedWaveform(_userDefinedWaveformBytes)
            _rfsgSession.Modulation.Digital.FskDeviation = fskDeviation

            ' Initiate Generation
            _rfsgSession.RF.OutputEnabled = True
            _rfsgSession.Initiate()

            ' Successful initialization, turn LED green
            errorTextBox.Text = "Generating."
            statusLed.BackColor = Color.Lime

            EnableControls(False)
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
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

    Private Sub AbortGeneration()
        Try
            ' Disable the output.  This sets the noise floor as low as possible.
            _rfsgSession.RF.OutputEnabled = False
            _rfsgSession.Abort()
            errorTextBox.Text = "Stopped."
            statusLed.BackColor = System.Drawing.SystemColors.Control

            EnableControls(True)
        Catch ex As Exception
            ShowError("AbortGeneration()", ex)
        End Try
    End Sub

    Private Sub StopGeneration()
        Try
            If _rfsgSession IsNot Nothing Then
                ' Disable the output.  This sets the noise floor as low as possible.
                _rfsgSession.RF.OutputEnabled = False

                ' Unsubscribe from warning events
                RemoveHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

                ' Close the RFSG session
                _rfsgSession.Close()
            End If
            _rfsgSession = Nothing
            errorTextBox.Clear()
            statusLed.BackColor = System.Drawing.SystemColors.Control

            EnableControls(True)
        Catch ex As Exception
            errorTextBox.Text = "Error in StopGeneration(): " + ex.Message
            statusLed.BackColor = System.Drawing.Color.Red
        End Try
    End Sub

#End Region

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = e.Message
    End Sub

#Region "Form Events"

    Private Sub modulationTypeComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim val As RfsgDigitalModulationType
        val = DirectCast([Enum].Parse(GetType(RfsgDigitalModulationType), DirectCast(modulationTypeComboBox.SelectedItem, String)), RfsgDigitalModulationType)
        Select Case val
            Case RfsgDigitalModulationType.None
                messageWaveformTypeLabel.Visible = False
                messageWaveformTypeComboBox.Visible = False
                messageSymbolRateLabel.Visible = False
                messageSymbolRateNumeric.Visible = False
                messagePrbsOrderLabel.Visible = False
                messagePrbsOrderNumeric.Visible = False
                messageUserDefinedWaveformLabel.Visible = False
                messageUserDefinedWaveformTextBox.Visible = False
                fskDeviationLabel.Visible = False
                fskDeviationNumeric.Visible = False
                Exit Select
            Case RfsgDigitalModulationType.Fsk
                messageWaveformTypeLabel.Visible = True
                messageWaveformTypeComboBox.Visible = True
                messageSymbolRateLabel.Visible = True
                messageSymbolRateNumeric.Visible = True
                messagePrbsOrderLabel.Visible = True
                messagePrbsOrderNumeric.Visible = True
                messageUserDefinedWaveformLabel.Visible = True
                messageUserDefinedWaveformTextBox.Visible = True
                fskDeviationLabel.Visible = True
                fskDeviationNumeric.Visible = True
                Exit Select
            Case RfsgDigitalModulationType.Ook, RfsgDigitalModulationType.Psk
                messageWaveformTypeLabel.Visible = True
                messageWaveformTypeComboBox.Visible = True
                messageSymbolRateLabel.Visible = True
                messageSymbolRateNumeric.Visible = True
                messagePrbsOrderLabel.Visible = True
                messagePrbsOrderNumeric.Visible = True
                messageUserDefinedWaveformLabel.Visible = True
                messageUserDefinedWaveformTextBox.Visible = True
                fskDeviationLabel.Visible = False
                fskDeviationNumeric.Visible = False
                Exit Select
        End Select
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        StopGeneration()
    End Sub

    Private Sub generateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles generateButton.Click
        StartGeneration()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles stopButton.Click
        StopGeneration()
    End Sub

    Private Sub rfsgStatusTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles rfsgStatusTimer.Tick
        CheckGeneration()
    End Sub

    Private Sub messageUserDefinedWaveformTextBox_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles messageUserDefinedWaveformTextBox.TextChanged

        'Clear off any previous errors.
        errorTextBox.Clear()
        generateButton.Enabled = True
        statusLed.BackColor = SystemColors.Control

        ' Check if the user is entering valid bytes data.
        ' Convert the user-defined waveform string into an array of integers.
        _userDefinedWaveformString = messageUserDefinedWaveformTextBox.Text.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
        Try
            _userDefinedWaveformBytes = New Byte(_userDefinedWaveformString.Length - 1) {}
            For i As Integer = 0 To _userDefinedWaveformString.Length - 1
                If Not Byte.TryParse(_userDefinedWaveformString(i), _userDefinedWaveformBytes(i)) Then
                    ShowError("Entering Waveform values", New ArgumentException("Input string not in correct format"))
                    generateButton.Enabled = False
                    Exit For
                End If
            Next
        Catch
            ShowError("Entering Waveform values", New ArgumentException("Input string not in correct format"))
            generateButton.Enabled = False
        End Try
    End Sub

#End Region

    Private Sub ShowError(ByVal functionName As String, ByVal exception As Exception)
        StopGeneration()
        errorTextBox.Text = "Error in " + functionName + ": " + exception.Message
        statusLed.BackColor = System.Drawing.Color.Red
    End Sub

    Private Sub EnableControls(ByVal enabled As Boolean)
        generateButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        rfsgStatusTimer.Enabled = Not enabled
        resourceNameComboBox.Enabled = enabled
        messageUserDefinedWaveformTextBox.Enabled = enabled
        frequencyReferenceSourceComboBox.Enabled = enabled
        frequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        modulationTypeComboBox.Enabled = enabled
        messageWaveformTypeComboBox.Enabled = enabled
        messageSymbolRateNumeric.Enabled = enabled
        messagePrbsOrderNumeric.Enabled = enabled
        fskDeviationNumeric.Enabled = enabled

        Application.DoEvents()
    End Sub

End Class
