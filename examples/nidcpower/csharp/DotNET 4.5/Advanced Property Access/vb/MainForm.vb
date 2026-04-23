'=============================================================================================================
'
' Title:
'      NI-DCPower Source DC Current Using Advanced Property Access 
'      
' Description:
'      This example demonstrates how to use the Advanced Property Access Service
'      in the NI-DCPower .NET API. This example configures the Output Function, Sense,
'      Autorange, Current Level, and Voltage Limit using Advanced Property Access Service.
'
'      Note: In this example the Output Function is set to  DC Voltage. If you change the 
'      Output Function to DC Current, you must use Current Level and Voltage Limit instead
'      of Voltage Level and Current Limit.
'
'============================================================================================================

Imports System.Drawing
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form

    ' Attribute values obtained from the C header files: ivi.h, nidcpower.h
    Friend NotInheritable Class CDriverAttributeId
        Private Sub New()
        End Sub
        ' NIDCPOWER_ATTR_SOURCE_MODE
        Friend Const SourceMode As Long = 1150054

        ' NIDCPOWER_ATTR_OUTPUT_FUNCTION
        Friend Const OutputFunction As Long = 1150008

        ' NIDCPOWER_ATTR_SENSE
        Friend Const Sense As Long = 1150013

        ' NIDCPOWER_ATTR_VOLTAGE_LEVEL
        Friend Const VoltageLevel As Long = 1250001

        ' NIDCPOWER_ATTR_VOLTAGE_LEVEL_RANGE
        Friend Const VoltageLevelRange As Long = 1150005

        ' NIDCPOWER_ATTR_CURRENT_LIMIT
        Friend Const CurrentLimit As Long = 1250005

        ' NIDCPOWER_ATTR_CURRENT_LIMIT_RANGE
        Friend Const CurrentLimitRange As Long = 1150004
    End Class

    Private dcPowerSession As NIDCPower

    Public Sub New()
        InitializeComponent()
        ConfigureSenseComboBox()
        LoadDCPowerDeviceNames()
    End Sub

#Region "MainForm initial configuration"
    Private Sub ConfigureSenseComboBox()
        For Each item As DCPowerMeasurementSense In [Enum].GetValues(GetType(DCPowerMeasurementSense))
            senseComboBox.Items.Add(item)
        Next
        senseComboBox.SelectedIndex = 0
    End Sub

    Private Sub LoadDCPowerDeviceNames()
        Using dcPowerDevices As New ModularInstrumentsSystem("NI-DCPower")
            For Each device As DeviceInfo In dcPowerDevices.DeviceCollection
                resourceNameComboBox.Items.Add(device.Name)
            Next
        End Using
        If resourceNameComboBox.Items.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub
#End Region

#Region "Mainform configuration values"
    Private ReadOnly Property ResourceName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property ChannelName() As String
        Get
            Return Me.channelNameTextBox.Text
        End Get
    End Property

    Private ReadOnly Property VoltageLevel() As Double
        Get
            Return Decimal.ToDouble(Me.voltageLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property VoltageLevelRange() As Double
        Get
            Return Decimal.ToDouble(Me.voltageLevelRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CurrentLimit() As Double
        Get
            Return Decimal.ToDouble(Me.currentLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CurrentLimitRange() As Double
        Get
            Return Decimal.ToDouble(Me.currentLimitRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Sense() As DCPowerMeasurementSense
        Get
            Return CType(Me.senseComboBox.SelectedItem, DCPowerMeasurementSense)
        End Get
    End Property

    Private ReadOnly Property SourceDelay() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.sourceDelayNumeric.Value))
        End Get
    End Property
#End Region

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startButton.Click
        ChangeControlState(False)
        Start()
        ChangeControlState(True)
    End Sub

    Private Sub mainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub Start()
        Try
            InitializeDCPowerSession()
            ConfigureSessionUsingAdvancedPropertyAccessService()

            dcPowerSession.Control.Initiate()
            dcPowerSession.Events.SourceCompleteEvent.WaitForEvent(New PrecisionTimeSpan(5.0))

            Dim result As DCPowerMeasureResult = dcPowerSession.Measurement.Measure(ChannelName)
            Dim inCompliance As Boolean = dcPowerSession.Measurement.QueryInCompliance(ChannelName)
            DisplayMeasurements(result.VoltageMeasurements(0), result.CurrentMeasurements(0), inCompliance)

            dcPowerSession.Utility.Reset()
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
        End Try
    End Sub

    Private Sub InitializeDCPowerSession()
        dcPowerSession = New NIDCPower(ResourceName, ChannelName, False)
        AddHandler dcPowerSession.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)
    End Sub

    Private Sub DCPowerDriverOperationWarning(ByVal sender As Object, ByVal e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub ConfigureSessionUsingAdvancedPropertyAccessService()
        Dim dcPowerAdvancedPropertyAccessService As AdvancedPropertyAccessService = DirectCast(TryCast(dcPowerSession, IServiceProvider).GetService(GetType(AdvancedPropertyAccessService)), AdvancedPropertyAccessService)

        dcPowerAdvancedPropertyAccessService.SetAttributeInt32(CDriverAttributeId.SourceMode, CInt(DCPowerSourceMode.SinglePoint))
        dcPowerAdvancedPropertyAccessService.SetAttributeInt32(CDriverAttributeId.OutputFunction, ChannelName, CInt(DCPowerSourceOutputFunction.DCVoltage))
        dcPowerAdvancedPropertyAccessService.SetAttributeInt32(CDriverAttributeId.Sense, ChannelName, CInt(Sense))
        dcPowerAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.VoltageLevel, ChannelName, VoltageLevel)
        dcPowerAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.CurrentLimit, ChannelName, CurrentLimit)
        dcPowerAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.VoltageLevelRange, ChannelName, VoltageLevelRange)
        dcPowerAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.CurrentLimitRange, ChannelName, CurrentLimitRange)
    End Sub

    Private Sub CloseSession()
        If dcPowerSession IsNot Nothing Then
            Try
                dcPowerSession.Close()
                dcPowerSession = Nothing
            Catch ex As Exception
                ShowError(ex)
                Application.[Exit]()
            End Try
        End If
    End Sub

    Private Sub DisplayMeasurements(voltage As Double, current As Double, inCompliance As Boolean)
        Me.currentMeasurementsTextBox.Text = current.ToString("E")
        Me.voltageMeasurementsTextBox.Text = voltage.ToString("E")
        Me.inComplianceButtonLed.BackColor = If(inCompliance, Color.Red, SystemColors.Control)
    End Sub

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub ChangeControlState(isEnabled As Boolean)
        Me.resourceNameAndChannelNameGroupBox.Enabled = isEnabled
        Me.configurationGroupBox.Enabled = isEnabled
        Me.startButton.Enabled = isEnabled
        Me.resourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub

End Class
