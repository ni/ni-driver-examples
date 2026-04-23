
'==================================================================================================
' Title        : IVI Current Measurement
' Copyright    : National Instruments 2011. All Rights Reserved.
' Description  : This example demonstrates how to make measurements in an IVI application.
'                An IVI session is created,the session is configured for current measurement 
'                and a reading is acquired and displayed.
' *Note        : Before running this example, make sure that IVI driver session is properly 
'                configured in MAX and a corresponding Logical name (case sensitive) 
'                is assigned to the session.
'===================================================================================================

Imports System
Imports System.Windows.Forms
Imports Ivi.Dmm

Partial Public Class MainForm
    Inherits Form
    Friend modeACEnabled As [Boolean] = False
    Private iviDmmSession As Ivi.Dmm.IIviDmm

    '/ <summary>
    '/ The main entry point for the application.
    '/ </summary>
    <STAThread()> _
    Shared Sub Main()
        Application.EnableVisualStyles()
        Application.Run(New MainForm)
    End Sub 'Main

    Public Sub New()
        InitializeComponent()
        LoadPowerlineFrequencyValues()
        LoadMeasurementModes()
        LoadResolutionValues()
    End Sub

    Private Sub LoadPowerlineFrequencyValues()
        Dim powerlineFrequencyValues As Double() = {50, 60}
        Dim i As Integer = 0
        While i < powerlineFrequencyValues.Length
            powerlineFrequencyValueComboBox.Items.Add(powerlineFrequencyValues(i))
            i += 1
        End While
        powerlineFrequencyValueComboBox.SelectedIndex = 1

    End Sub

    Private Sub LoadMeasurementModes()
        measurementModeComboBox.Items.AddRange([Enum].GetNames(GetType(Ivi.Dmm.MeasurementFunction)))
        'DC Current is the default measurement mode
        measurementModeComboBox.SelectedIndex = 2
    End Sub

    Private Sub LoadResolutionValues()
        Dim resolutionValues As Double() = {3.5, 4.5, 5.5, 6.5, 7.5}
        Dim i As Integer = 0
        While i < resolutionValues.Length
            resolutionValueComboBox.Items.Add(resolutionValues(i))
            i += 1
        End While
        resolutionValueComboBox.SelectedIndex = 3
    End Sub

    Private Sub EnableControls(ByVal enabled As Boolean)
        resolutionValueComboBox.Enabled = enabled
        measurementModeComboBox.Enabled = enabled
        deviceNameTextBox.Enabled = enabled
        rangeTextBox.Enabled = enabled
        readButton.Enabled = enabled
        powerlineFrequencyValueComboBox.Enabled = enabled
        If modeACEnabled = True Then
            minACFrequencyTextBox.Enabled = enabled
            maxACFrequencyTextBox.Enabled = enabled
        End If
    End Sub

    Private Sub UpdateActualRange(ByVal sampleDmmSession As IIviDmm)
        Dim actualRange As Double = sampleDmmSession.Range
        actualRangeTextBox.Text = [String].Format("{0:G5}", actualRange)
    End Sub

    Private Sub Configure()
        Dim range As Double = Double.Parse(rangeTextBox.Text)
        Dim resolution As Double = Double.Parse(resolutionValueComboBox.Text)
        Dim powerlineFrequency As Double = Double.Parse(powerlineFrequencyValueComboBox.Text)

        'Get the Measurement Mode from the UI
        Dim measurementMode As Ivi.Dmm.MeasurementFunction = Ivi.Dmm.MeasurementFunction.DCCurrent
        measurementMode = DirectCast([Enum].Parse(GetType(Ivi.Dmm.MeasurementFunction), measurementModeComboBox.Text), Ivi.Dmm.MeasurementFunction)
        'Configure Dmm session Measurement parameters
        iviDmmSession.Configure(measurementMode, range, resolution)
        'Configure Powerline Frequency
        iviDmmSession.Advanced.PowerlineFrequency = powerlineFrequency
        'Configure minimum and maximum AC frequency
        If modeACEnabled = True Then
            Dim minACFrequency As Double = Double.Parse(minACFrequencyTextBox.Text)
            Dim maxACFrequency As Double = Double.Parse(maxACFrequencyTextBox.Text)
            iviDmmSession.AC.FrequencyMin = minACFrequency

            iviDmmSession.AC.FrequencyMax = maxACFrequency
        End If
    End Sub

    Private Sub readButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles readButton.Click
        EnableControls(False)
        messageTextBox.Clear()
        Application.DoEvents()
        Dim reading As Double
        Try
            'Create an Ivi Dmm Session
            iviDmmSession = Ivi.Driver.IviDriver.Create(Of IIviDmm)(deviceNameTextBox.Text, True, True)
            'Configure session parameters
            Configure()
            'Obtain the reading from the session
            reading = iviDmmSession.Measurement.Read(Ivi.Driver.PrecisionTimeSpan.FromSeconds(2))
            'Update the actual range
            UpdateActualRange(iviDmmSession)
            'Display the reading
            measurementTextBox.Text = [String].Format("{0:G8}", reading)
            messageTextBox.Text = "Operation completed successfully."
        Catch sessionNotFoundException As Ivi.Driver.SessionNotFoundException
            messageTextBox.Text = sessionNotFoundException.Message + " Make sure that Measurement & Automation Explorer " _
                     + "is configured correctly with Logical Name (case sensitive) and the corresponding Driver Session."
        Catch exception As Exception
            messageTextBox.Text = exception.Message
        Finally
            If iviDmmSession IsNot Nothing Then
                iviDmmSession.Close()
            End If
            Application.DoEvents()
            EnableControls(True)
        End Try
    End Sub

    Private Sub measurementModeComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles measurementModeComboBox.SelectedIndexChanged
        If measurementModeComboBox.Text.Contains("AC") Then
            acConfigurationGroupBox.Enabled = True
            modeACEnabled = True
        Else
            acConfigurationGroupBox.Enabled = False
            modeACEnabled = False
        End If
    End Sub

End Class