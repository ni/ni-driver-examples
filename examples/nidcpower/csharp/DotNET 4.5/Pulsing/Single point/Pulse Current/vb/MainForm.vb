'=============================================================================================================
'
' Title:
'      NI-DCPower Pulse Current
'
' Demonstrates how to use the DCPower Pulse API to generate a single Current
' pulse.  This example initializes a session, configures the Source Mode and
' the Output Function, configures the pulse parameters, initiates the pulse
' output, and takes a measurement.
'
' Suggested Devices
'      NI PXIe-4135, NI PXIe-4136, NI PXIe-4137, NI PXIe-4138, NI PXIe-4139
'
'=============================================================================================================


Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Public Partial Class MainForm
    Inherits Form
    Private dcPowerSession As NIDCPower
    Public Sub New()
        InitializeComponent()
        LoadDCPowerDeviceNames()
    End Sub

#Region "UI Initial Value Config Section"
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

    Private ReadOnly Property FullyQualifiedChannelName() As String
        Get
            Return $"{ResourceName}/{ChannelName}"
        End Get
    End Property

    Private ReadOnly Property PulseCurrentLevel() As Double
        Get
            Return Decimal.ToDouble(Me.pulseCurrentLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PulseVoltageLimit() As Double
        Get
            Return Decimal.ToDouble(Me.pulseVoltageLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PulseOnTime() As Double
        Get
            Return Decimal.ToDouble(Me.pulseOnTimeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property SourceDelay() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.sourceDelayNumeric.Value))
        End Get
    End Property

    Private ReadOnly Property PulseCurrentLevelRange() As Double
        Get
            Return Decimal.ToDouble(Me.pulseCurrentLevelRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PulseVoltageLimitRange() As Double
        Get
            Return Decimal.ToDouble(Me.pulseVoltLimitRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PulseOffTime() As Double
        Get
            Return Decimal.ToDouble(Me.pulseOffTimeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property ApertureTime() As Double
        Get
            Return Decimal.ToDouble(Me.apertureTimeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property BiasCurrentLevel() As Double
        Get
            Return Decimal.ToDouble(Me.biasCurrentLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property BiasVoltageLimit() As Double
        Get
            Return Decimal.ToDouble(Me.biasVoltageLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PulseBiasDelay() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.pulseBiasDelayNumeric.Value))
        End Get
    End Property

    Private WriteOnly Property CurrentMeasurement() As Double
        Set
            currentMeasurementTextBox.Text = Value.ToString()
        End Set
    End Property

    Private WriteOnly Property VoltageMeasurement() As Double
        Set
            voltageMeasurementTextBox.Text = Value.ToString()
        End Set
    End Property
#End Region

    Private Sub startButton_Click(sender As Object, e As EventArgs) Handles startButton.Click
        ChangeControlState(False)
        Start()
        ChangeControlState(True)
    End Sub

    Private Sub mainForm_Closing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub Start()
        Try
            InitializeDCPowerSession(False)
            'Configure Session Parametres from UI
            dcPowerSession.Source.Mode = DCPowerSourceMode.SinglePoint
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.[Function] = DCPowerSourceOutputFunction.PulseCurrent
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseCurrent.CurrentLevel = PulseCurrentLevel
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseCurrent.CurrentLevelRange = PulseCurrentLevelRange
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseCurrent.BiasCurrentLevel = BiasCurrentLevel
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseCurrent.VoltageLimit = PulseVoltageLimit
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseCurrent.VoltageLimitRange = PulseVoltageLimitRange
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseCurrent.BiasVoltageLimit = BiasVoltageLimit
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseOnTime = PulseOnTime
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseOffTime = PulseOffTime
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.PulseBiasDelay = PulseBiasDelay
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.SourceDelay = SourceDelay
            dcPowerSession.Outputs(FullyQualifiedChannelName).Measurement.ConfigureApertureTime(ApertureTime, DCPowerMeasureApertureTimeUnits.Seconds)
            'Initiate the device to start generation and acquisition
            dcPowerSession.Control.Initiate()
            'Fetch voltage and current
            Dim result As DCPowerFetchResult = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, New PrecisionTimeSpan(10), 1)
            DisplayResult(result)
            dcPowerSession.Utility.Reset()
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
        End Try
    End Sub

    Private Sub DisplayResult(result As DCPowerFetchResult)
        VoltageMeasurement = result.VoltageMeasurements(0)
        CurrentMeasurement = result.CurrentMeasurements(0)
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

    Private Shared Sub ShowError(ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub InitializeDCPowerSession(reset As Boolean)
        dcPowerSession = New NIDCPower(FullyQualifiedChannelName, reset, String.Empty)
        AddHandler dcPowerSession.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)
    End Sub

    Private Sub ChangeControlState(isEnabled As Boolean)
        resourceAndChannelNameGroupBox.Enabled = isEnabled
        configurationGroupBox.Enabled = isEnabled
        startButton.Enabled = isEnabled
        resourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub

    Private Sub DCPowerDriverOperationWarning(sender As Object, e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

End Class