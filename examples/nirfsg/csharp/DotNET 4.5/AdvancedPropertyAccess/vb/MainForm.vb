'==================================================================================================
' Title        : Advanced Property Access
' Description  : The application demonstrates how to use the Advanced Property
'			      Access Service.The application configures the reference clock 
'			      and initiates generation
'==================================================================================================

Imports System
Imports System.Linq
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _sampleRfsgSession As NIRfsg

    'Attribute values obtained from the C header files:ivi.h,ivirfsg.h,nirfsg.h
    Public Enum RfsgCAttributeIdentifier As Long
        NIRfsg_Attr_Frequency = 1250001
        NIRfsg_Attr_PowerLevel = 1250002
        NIRfsg_Attr_GenerationMode = 1150018
        NIRfsg_Attr_PowerLevelType = 1150043
        NIRfsg_Attr_FrequencyReferenceRate = 1250322
        NIRfsg_Attr_FrequencyReferenceSource = 1150001
        NIRfsg_Val_ContinuousWave = 1000
        NIRfsg_Val_ArbitraryWaveform = 1001
        NIRfsg_Val_Script = 1002
        NIRfsg_Val_PeakPower = 7001
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
        LoadRfsgDeviceNames()
        ConfigureGenerationModeComboBox()
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

    Private Sub ConfigureGenerationModeComboBox()
        generationModeComboBox.Items.Add("Continuous")
        generationModeComboBox.Items.Add("Arb")
        generationModeComboBox.Items.Add("Script")
        generationModeComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureFrequencyReferenceSourceComboBox()
        frequencyReferenceSourceComboBox.Items.Add(RfsgFrequencyReferenceSource.ClockIn)
        frequencyReferenceSourceComboBox.Items.Add(RfsgFrequencyReferenceSource.OnboardClock)
        frequencyReferenceSourceComboBox.Items.Add(RfsgFrequencyReferenceSource.PxiClock)
        frequencyReferenceSourceComboBox.Items.Add(RfsgFrequencyReferenceSource.ReferenceIn)
        frequencyReferenceSourceComboBox.SelectedIndex = 1
    End Sub

    Private Sub EnableControls(ByVal enabled As Boolean)
        generationModeComboBox.Enabled = enabled
        resourceNameComboBox.Enabled = enabled
        frequencyReferenceSourceComboBox.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        frequencyNumeric.Enabled = enabled
        startButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        Application.DoEvents()
    End Sub

    Private Sub Configure()
        Dim frequency As Double = frequencyNumeric.Value
        Dim frequencyReferenceRate As Double = 10000000.0
        Dim generationMode As Integer
        Dim powerLevel As Double = powerLevelNumeric.Value
        Dim frequencyReferenceSource As String = frequencyReferenceSourceComboBox.Text
        Dim powerLevelType As Integer = RfsgCAttributeIdentifier.NIRfsg_Val_PeakPower

        'Create a Rfsg Session
        _sampleRfsgSession = New NIRfsg(resourceNameComboBox.Text, True, True)

        ' Subscribe to Rfsg warnings
        AddHandler _sampleRfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

        If generationModeComboBox.SelectedIndex = 0 Then
            generationMode = RfsgCAttributeIdentifier.NIRfsg_Val_ContinuousWave
        ElseIf generationModeComboBox.SelectedIndex = 1 Then
            generationMode = RfsgCAttributeIdentifier.NIRfsg_Val_ArbitraryWaveform
        Else
            generationMode = RfsgCAttributeIdentifier.NIRfsg_Val_Script
        End If

        ' Use advanced property access to set the values into the driver
        Dim iServiceProviderInterface As IServiceProvider = TryCast(_sampleRfsgSession, IServiceProvider)
        Dim advancedPropertyAccessService As AdvancedPropertyAccessService = DirectCast(iServiceProviderInterface.GetService(GetType(AdvancedPropertyAccessService)), AdvancedPropertyAccessService)
        ' Set the Generation Mode
        advancedPropertyAccessService.SetAttributeInt32(DirectCast(RfsgCAttributeIdentifier.NIRfsg_Attr_GenerationMode, Long), generationMode)
        ' Set the Power Level Type
        advancedPropertyAccessService.SetAttributeInt32(DirectCast(RfsgCAttributeIdentifier.NIRfsg_Attr_PowerLevelType, Long), powerLevelType)
        ' Set the Frequency
        advancedPropertyAccessService.SetAttributeDouble(DirectCast(RfsgCAttributeIdentifier.NIRfsg_Attr_Frequency, Long), frequency)
        ' Set the operation mode
        advancedPropertyAccessService.SetAttributeString(DirectCast(RfsgCAttributeIdentifier.NIRfsg_Attr_FrequencyReferenceSource, Long), frequencyReferenceSource)
        ' Set the number of Averages
        advancedPropertyAccessService.SetAttributeDouble(DirectCast(RfsgCAttributeIdentifier.NIRfsg_Attr_PowerLevel, Long), powerLevel)
        ' Set the Resolution
        advancedPropertyAccessService.SetAttributeDouble(DirectCast(RfsgCAttributeIdentifier.NIRfsg_Attr_FrequencyReferenceRate, Long), frequencyReferenceRate)
    End Sub

    Private Sub StartGeneration()
        EnableControls(False)
        Application.DoEvents()
        Try
            errorTextBox.Text = "No error"
            Configure()

            ' Write waveform - this will be generated only in Arb\Script mode
            Dim iData As Double() = Enumerable.Repeat(Of Double)(1.0, 32).ToArray()
            Dim qData As Double() = Enumerable.Repeat(Of Double)(0.0, 32).ToArray()
            _sampleRfsgSession.Arb.WriteWaveform("waveform1", iData, qData)

            ' Write Script for the Script mode
            Dim script As String =
                    "script simpleScript" + Environment.NewLine +
                    "   repeat forever" + Environment.NewLine +
                    "       generate waveform1" + Environment.NewLine +
                    "   end repeat" + Environment.NewLine +
                    "end script"

            _sampleRfsgSession.Arb.Scripting.WriteScript(script)

            ' Start generation
            _sampleRfsgSession.Initiate()
            ' Update the actual values
            actualFrequencyTextBox.Text = [String].Format("{0:G8}", _sampleRfsgSession.RF.Frequency)
            actualPowerLevelTextBox.Text = [String].Format("{0:G8}", _sampleRfsgSession.RF.PowerLevel)
        Catch exception As Exception
            ShowError("StartGeneration()", exception)
        End Try
    End Sub

    Private Sub StopGeneration()
        Try
            If _sampleRfsgSession IsNot Nothing Then
                ' Unsubscribe from warning events
                RemoveHandler _sampleRfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning
                ' Close NIRfsg session
                _sampleRfsgSession.Close()
            End If
            _sampleRfsgSession = Nothing
            EnableControls(True)
        Catch exception As Exception
            errorTextBox.Text = "Error in StopGeneration(): " + exception.Message
        End Try
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = e.Message
    End Sub

    Private Sub ShowError(ByVal functionName As String, ByVal exception As Exception)
        StopGeneration()
        errorTextBox.Text = "Error in " + functionName + ": " + exception.Message
    End Sub

#Region "Form Events"

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        StartGeneration()
    End Sub

    Private Sub stopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stopButton.Click
        StopGeneration()
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        StopGeneration()
    End Sub

#End Region

End Class
