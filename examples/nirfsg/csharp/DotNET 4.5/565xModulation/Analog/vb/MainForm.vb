'==================================================================================================
' Title        : 565x Analog Modulation
' Description  : This example demonstrates the use of 565x family of NI RF Signal Generators.
'			     The example shows how to generate a continuous wave signal, and optionally apply
'			     analog modulation.
'==================================================================================================

Imports System
Imports System.Windows.Forms
Imports System.Collections
Imports System.Collections.Generic
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg
    Const FrequencyReferenceRate As Double = 10000000.0

    Public Sub New()
        InitializeComponent()

        LoadRfsgDeviceNames()

        EnableControls(True)

        ConfigureFrequencyReferenceSourceComboBox()
        ConfigureMessageWaveformTypeComboBox()
        ConfigureModulationTypeComboBox()
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

    Private Sub ConfigureMessageWaveformTypeComboBox()
        messageWaveformTypeComboBox.Items.AddRange([Enum].GetNames(GetType(RfsgAnalogModulationWaveformType)))
        messageWaveformTypeComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureModulationTypeComboBox()
        modulationTypeComboBox.Items.AddRange([Enum].GetNames(GetType(RfsgAnalogModulationType)))
        modulationTypeComboBox.SelectedIndex = 1
    End Sub

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim frequencyReferenceSource As RfsgFrequencyReferenceSource
        Dim centerFrequency As Double
        Dim powerLevel As Double
        Dim modulationType As RfsgAnalogModulationType
        Dim messageWaveformType As RfsgAnalogModulationWaveformType
        Dim messageWaveformFrequency As Double
        Dim fmDeviation As Double
        Dim pmDeviation As Double
        Try
            ' Read in all the control values
            resourceName = resourceNameComboBox.Text
            frequencyReferenceSource = If(TryCast(frequencyReferenceSourceComboBox.SelectedValue, RfsgFrequencyReferenceSource), RfsgFrequencyReferenceSource.FromString(frequencyReferenceSourceComboBox.Text))
            centerFrequency = CDbl(frequencyNumeric.Value)
            powerLevel = CDbl(powerLevelNumeric.Value)
            modulationType = DirectCast([Enum].Parse(GetType(RfsgAnalogModulationType), DirectCast(modulationTypeComboBox.SelectedItem, String)), RfsgAnalogModulationType)
            messageWaveformType = DirectCast([Enum].Parse(GetType(RfsgAnalogModulationWaveformType), DirectCast(messageWaveformTypeComboBox.SelectedItem, String)), RfsgAnalogModulationWaveformType)
            messageWaveformFrequency = CDbl(messageFrequencyNumeric.Value)
            fmDeviation = CDbl(fmDeviationNumeric.Value)
            pmDeviation = CDbl(pmDeviationNumeric.Value)

            ' Update status box
            errorTextBox.Text = "Initializing..."
            Application.DoEvents()
            ' Initialize the NIRfsg session
            If _rfsgSession Is Nothing Then
                _rfsgSession = New NIRfsg(resourceName, True, False)

                ' Subscribe to Rfsg warnings
                AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning
            End If

            ' Configure the instrument
            _rfsgSession.RF.Configure(centerFrequency, powerLevel)
            _rfsgSession.FrequencyReference.Configure(frequencyReferenceSource, FrequencyReferenceRate)

            ' Set the modulation parameters
            ' We must set all the parameters regardless of modulation type so that
            ' when we change the modulation type on-the-fly, we do not have to also
            ' set the other modulation parameters.
            _rfsgSession.Modulation.Analog.ModulationType = modulationType
            _rfsgSession.Modulation.Analog.WaveformType = messageWaveformType
            _rfsgSession.Modulation.Analog.WaveformFrequency = messageWaveformFrequency
            _rfsgSession.Modulation.Analog.FMDeviation = fmDeviation
            _rfsgSession.Modulation.Analog.PMDeviation = pmDeviation

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
            statusLed.BackColor = SystemColors.Control

            EnableControls(True)
        Catch ex As Exception
            errorTextBox.Text = "Error in StopGeneration(): " + ex.Message
            statusLed.BackColor = Color.Red
        End Try
    End Sub

#End Region

#Region "Form Events"

    Private Sub modulationTypeComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles modulationTypeComboBox.SelectedIndexChanged
        Try
            Dim val As RfsgAnalogModulationType

            ' Change the visibility of controls based on modulation type
            val = DirectCast([Enum].Parse(GetType(RfsgAnalogModulationType), DirectCast(modulationTypeComboBox.SelectedItem, String)), RfsgAnalogModulationType)
            Select Case val
                Case RfsgAnalogModulationType.None
                    messageWaveformTypeLabel.Visible = False
                    messageWaveformTypeComboBox.Visible = False
                    messageFrequencyLabel.Visible = False
                    messageFrequencyNumeric.Visible = False
                    fmDeviationLabel.Visible = False
                    fmDeviationNumeric.Visible = False
                    pmDeviationLabel.Visible = False
                    pmDeviationNumeric.Visible = False
                    Exit Select
                Case RfsgAnalogModulationType.FM
                    messageWaveformTypeLabel.Visible = True
                    messageWaveformTypeComboBox.Visible = True
                    messageFrequencyLabel.Visible = True
                    messageFrequencyNumeric.Visible = True
                    fmDeviationLabel.Visible = True
                    fmDeviationNumeric.Visible = True
                    pmDeviationLabel.Visible = False
                    pmDeviationNumeric.Visible = False
                    Exit Select
                Case RfsgAnalogModulationType.PM
                    messageWaveformTypeLabel.Visible = True
                    messageWaveformTypeComboBox.Visible = True
                    messageFrequencyLabel.Visible = True
                    messageFrequencyNumeric.Visible = True
                    fmDeviationLabel.Visible = False
                    fmDeviationNumeric.Visible = False
                    pmDeviationLabel.Visible = True
                    pmDeviationNumeric.Visible = True
                    Exit Select
            End Select
        Catch ex As Exception
            ShowError("Set Analog Modulation Type", ex)
        End Try
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

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = e.Message
    End Sub

#End Region

    Private Sub ShowError(ByVal functionName As String, ByVal exception As Exception)
        StopGeneration()
        errorTextBox.Text = "Error in " + functionName + ": " + exception.Message
        statusLed.BackColor = Color.Red
    End Sub

    Private Sub EnableControls(ByVal enabled As Boolean)
        generateButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        resourceNameComboBox.Enabled = enabled
        frequencyReferenceSourceComboBox.Enabled = enabled
        frequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        modulationTypeComboBox.Enabled = enabled
        messageWaveformTypeComboBox.Enabled = enabled
        messageFrequencyNumeric.Enabled = enabled
        fmDeviationNumeric.Enabled = enabled
        pmDeviationNumeric.Enabled = enabled

        ' Start the status checking timer
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub

End Class
