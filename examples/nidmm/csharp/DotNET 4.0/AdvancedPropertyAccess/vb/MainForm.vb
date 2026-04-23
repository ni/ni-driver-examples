
'==================================================================================================
' Title        : Advanced Property Access
' Copyright    : National Instruments 2011. All Rights Reserved.
' Description  : This application demonstrates how to use the Advanced Property Access Service.
'                This application configures the DMM for Capacitance/Inductance measurement,
'                acquires a reading and displays the aquired reading to the user.
'===================================================================================================

Imports System
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments
Imports NationalInstruments.ModularInstruments.NIDmm
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private sampleDmmSession As NIDmm
    'Attribute values obtained from the C header files:ivi.h,ividmm.h,nidmm.h
    Friend Enum DmmCAttributeIdentifier As Long
        NIDMM_ATTR_RANGE = 1250002
        NIDMM_ATTR_FUNCTION = 1250001
        NIDMM_ATTR_RESOLUTION_DIGITS = 1250003
        NIDMM_ATTR_NUMBER_OF_AVERAGES = 1150055
        NIDMM_VAL_CAPACITANCE = 1005
        NIDMM_VAL_INDUCTANCE = 1006
        NIDMM_ATTR_OPERATION_MODE = 1150014
        NIDMM_VAL_IVIDMM_MODE = 0
    End Enum

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
        LoadDmmDeviceNames()
        LoadMeasurementModes()
    End Sub
    Private Sub LoadDmmDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-DMM")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            resourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub
    Private Sub LoadMeasurementModes()
        measurementModeComboBox.Items.Add("Capacitance")
        measurementModeComboBox.Items.Add("Inductance")
        measurementModeComboBox.SelectedIndex = 0
    End Sub
    Private Sub EnableControls(ByVal enabled As Boolean)
        measurementModeComboBox.Enabled = enabled
        resourceNameComboBox.Enabled = enabled
        rangeTextBox.Enabled = enabled
        measurementsToAverageNumericUpDown.Enabled = enabled
        readButton.Enabled = enabled
    End Sub
    Private Sub UpdateActualRange(ByVal sampleDmmSession As NIDmm)
        Dim actualRange As Double = sampleDmmSession.Range
        actualRangeTextBox.Text = [String].Format("{0:G5}", actualRange)
    End Sub
    Private Sub Configure()

        Dim range As Double = Double.Parse(rangeTextBox.Text)
        Dim resolution As Double = 6.5
        Dim measurementMode As Long
        Dim measurementsToAverage As Integer = measurementsToAverageNumericUpDown.Value

        'Create a Dmm Session
        sampleDmmSession = New NIDmm(resourceNameComboBox.Text, True, True)

        If measurementModeComboBox.Text.CompareTo("Capacitance") = 0 Then
            measurementMode = DirectCast(DmmCAttributeIdentifier.NIDMM_VAL_CAPACITANCE, Long)
        Else
            measurementMode = DirectCast(DmmCAttributeIdentifier.NIDMM_VAL_INDUCTANCE, Long)
        End If
        ' Use advanced property access to get the measurements to average value
        Dim iServiceProviderInterface As IServiceProvider = TryCast(sampleDmmSession, IServiceProvider)
        Dim advancedPropertyAccessService As AdvancedPropertyAccessService = DirectCast(iServiceProviderInterface.GetService(GetType(AdvancedPropertyAccessService)), AdvancedPropertyAccessService)
        ' Set the Measurement Mode
        advancedPropertyAccessService.SetAttributeInteger(DirectCast(DmmCAttributeIdentifier.NIDMM_ATTR_FUNCTION, Long), measurementMode)
        ' Set the Range
        advancedPropertyAccessService.SetAttributeDouble(DirectCast(DmmCAttributeIdentifier.NIDMM_ATTR_RANGE, Long), range)
        ' Set the operation mode
        advancedPropertyAccessService.SetAttributeInteger(DirectCast(DmmCAttributeIdentifier.NIDMM_ATTR_OPERATION_MODE, Long), DirectCast(DmmCAttributeIdentifier.NIDMM_VAL_IVIDMM_MODE, Long))
        ' Set the number of Averages
        advancedPropertyAccessService.SetAttributeInteger(DirectCast(DmmCAttributeIdentifier.NIDMM_ATTR_NUMBER_OF_AVERAGES, Long), measurementsToAverage)
        ' Set the Resolution
        advancedPropertyAccessService.SetAttributeDouble(DirectCast(DmmCAttributeIdentifier.NIDMM_ATTR_RESOLUTION_DIGITS, Long), resolution)

    End Sub


    Private Sub readButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles readButton.Click
        EnableControls(False)
        messageTextBox.Clear()
        Application.DoEvents()
        Try
            Dim reading As Double
            Dim measurementsToAverage As Integer = measurementsToAverageNumericUpDown.Value
            Configure()
            ' Obtain the reading from the session
            reading = sampleDmmSession.Measurement.Read()
            ' Update the actual range
            UpdateActualRange(sampleDmmSession)
            ' Display the reading
            measurementTextBox.Text = [String].Format("{0:G8}", reading)
            messageTextBox.Text = "Operation completed successfully."
        Catch exception As Exception
            messageTextBox.Text = exception.Message
        Finally
            If sampleDmmSession IsNot Nothing Then
                sampleDmmSession.Close()
            End If
            Application.DoEvents()
            EnableControls(True)
        End Try
    End Sub
End Class


