'=============================================================================================================
'
' Title:
'      NI-DCPower LCR Source AC Voltage Example
'
' Description:
'      Demonstrates how to use the LCR AC Voltage Output Function to force an output voltage. This example
'      initializes a session; configures the Instrument Mode, LCR Stimulus Function, LCR Frequency,
'      LCR Voltage Amplitude, LCR Impedance Autorange, LCR Impedance Range, LCR DC Bias Source, 
'      LCR DC Bias Voltage Level, LCR Measurement Time, Cable Length, LCR Source Delay Mode and Source Delay; 
'      initiates generation, waits for a specified delay; and measures the output.
'
'  Suggested Device(s):
'      PXIe-4190
'
'=============================================================================================================

Imports System.Drawing
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private dcPowerSession As NIDCPower

    Public Sub New()
        InitializeComponent()
        ConfigureMeasurementTimeComboBox()
        ConfigureCableLengthComboBox()
        ConfigureDcBiasSourceComboBox()
        ConfigureLcrSourceDelayModeComboBox()
        ConfigureLcrImpedanceAutoRangeComboBox()
        LoadDCPowerDeviceNames()
    End Sub

#Region "MainForm initial configuration"
    Private Sub LoadDCPowerDeviceNames()
        Using dcPowerDevices As New ModularInstrumentsSystem("NI-DCPower")
            For Each device As DeviceInfo In dcPowerDevices.DeviceCollection
                resourceNameComboBox.Items.Add(device.Name + "/0")
            Next
        End Using
        If resourceNameComboBox.Items.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub

    Private Sub ConfigureMeasurementTimeComboBox()
        For Each item As DCPowerLCRMeasurementTime In [Enum].GetValues(GetType(DCPowerLCRMeasurementTime))
            lcrMeasurementTimeComboBox.Items.Add(item)
        Next
        lcrMeasurementTimeComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureCableLengthComboBox()
        For Each item As DCPowerCableLength In [Enum].GetValues(GetType(DCPowerCableLength))
            cableLengthComboBox.Items.Add(item)
        Next
        cableLengthComboBox.SelectedIndex = 2
    End Sub

    Private Sub ConfigureDcBiasSourceComboBox()
        For Each item As DCPowerLCRDCBiasSource In [Enum].GetValues(GetType(DCPowerLCRDCBiasSource))
            lcrDcBiasSourceComboBox.Items.Add(item)
        Next
        lcrDcBiasSourceComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureLcrSourceDelayModeComboBox()
        For Each item As DCPowerLCRSourceDelayMode In [Enum].GetValues(GetType(DCPowerLCRSourceDelayMode))
            lcrSourceDelayModeComboBox.Items.Add(item)
        Next
        lcrSourceDelayModeComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureLcrImpedanceAutorangeComboBox()
        For Each item As DCPowerLCRImpedanceAutorange In [Enum].GetValues(GetType(DCPowerLCRImpedanceAutorange))
            lcrImpedanceAutorangeComboBox.Items.Add(item)
        Next
        lcrImpedanceAutorangeComboBox.SelectedItem = DCPowerLCRImpedanceAutorange.On
    End Sub
#End Region

#Region "MainForm configuration values"

    Private ReadOnly Property FullyQualifiedChannelName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property


    Private ReadOnly Property Frequency() As Double
        Get
            Return Decimal.ToDouble(Me.lcrFrequencyUpDown.Value)
        End Get
    End Property

    Private ReadOnly Property CableLength() As DCPowerCableLength
        Get
            Return CType(Me.cableLengthComboBox.SelectedItem, DCPowerCableLength)
        End Get
    End Property

    Private ReadOnly Property ImpedanceAutorange() As DCPowerLCRImpedanceAutorange
        Get
            Return CType(Me.lcrImpedanceAutorangeComboBox.SelectedItem, DCPowerLCRImpedanceAutorange)
        End Get
    End Property

    Private ReadOnly Property ImpedanceRange() As Double
        Get
            Return Decimal.ToDouble(Me.lcrImpedanceRangeUpDown.Value)
        End Get
    End Property

    Private ReadOnly Property AcVoltage() As Double
        Get
            Return Decimal.ToDouble(Me.lcrVoltageUpDown.Value)
        End Get
    End Property

    Private ReadOnly Property LcrDcBiasSource() As DCPowerLCRDCBiasSource
        Get
            Return CType(Me.lcrDcBiasSourceComboBox.SelectedItem, DCPowerLCRDCBiasSource)
        End Get
    End Property

    Private ReadOnly Property DcBiasVoltage() As Double
        Get
            Return Decimal.ToDouble(Me.lcrDcBiasVoltageUpDown.Value)
        End Get
    End Property

    Private ReadOnly Property DcBiasCurrent() As Double
        Get
            Return Decimal.ToDouble(Me.lcrDcBiasCurrentUpDown.Value)
        End Get
    End Property

    Private ReadOnly Property LcrMeasurementTime() As DCPowerLCRMeasurementTime
        Get
            Return CType(Me.lcrMeasurementTimeComboBox.SelectedItem, DCPowerLCRMeasurementTime)
        End Get
    End Property

    Private ReadOnly Property LcrCustomMeasurementTime() As Double
        Get
            Return Decimal.ToDouble(Me.lcrCustomMeasurementTimeUpDown.Value)
        End Get
    End Property

    Private ReadOnly Property LcrSourceDelayMode() As DCPowerLCRSourceDelayMode
        Get
            Return CType(Me.lcrSourceDelayModeComboBox.SelectedItem, DCPowerLCRSourceDelayMode)
        End Get
    End Property

    Private ReadOnly Property SourceDelay() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Decimal.ToDouble(Me.sourceDelayUpDown.Value))
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
        If CableLength = DCPowerCableLength.CustomAsConfigured Then
            MessageBox.Show("Cable Length - (Custom) As Configured is selected. This cable length option is not supported in this example. Search ni.com for information about NI power supplies/SMUs and the NI-DCPower API.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Try
            InitializeDCPowerSession()

            dcPowerSession.Outputs(FullyQualifiedChannelName).InstrumentMode = DCPowerInstrumentMode.LCR
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.StimulusFunction = DCPowerLCRStimulusFunction.ACVoltage
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.Frequency = Frequency
            dcPowerSession.Outputs(FullyQualifiedChannelName).DeviceSpecific.LCR.CableLength = CableLength
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.ImpedanceAutoRange = ImpedanceAutorange
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.ImpedanceRange = ImpedanceRange
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.VoltageAmplitude = AcVoltage
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.DCBiasSource = LcrDcBiasSource
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.DCBiasVoltageLevel = DcBiasVoltage
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.DCBiasCurrentLevel = DcBiasCurrent
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.MeasurementTime = LcrMeasurementTime
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.CustomMeasurementTime = LcrCustomMeasurementTime
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.SourceDelayMode = LcrSourceDelayMode
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.SourceDelay = SourceDelay

            dcPowerSession.Control.Initiate()

            ' Wait for output to settle.
            dcPowerSession.Events.SourceCompleteEvent.WaitForEvent(New PrecisionTimeSpan(21.0))

            Dim results As NILCRMeasurement() = dcPowerSession.Measurement.MeasureLCR(FullyQualifiedChannelName)

            ' Though this example can return multiple measurements from multiple channels, it is designed
            ' to display only the first measurement taken across any number of channels. You can adapt the
            ' example to display multiple measurements from multiple channels.
            DisplayMeasurements(results(0))

            dcPowerSession.Utility.Reset(FullyQualifiedChannelName)
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
        End Try
    End Sub

    Private Sub InitializeDCPowerSession()
        dcPowerSession = New NIDCPower(FullyQualifiedChannelName, False, String.Empty)
        AddHandler dcPowerSession.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)
    End Sub

    Private Sub DCPowerDriverOperationWarning(ByVal sender As Object, ByVal e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

    Private Sub DisplayMeasurements(results As NILCRMeasurement)
        Me.vdcTextBox.Text = results.Vdc.ToString("E")
        Me.idcTextBox.Text = results.Idc.ToString("E")
        Me.stimulusFrequencyTextBox.Text = results.stimulusFrequency.ToString("E")
        Me.acVoltageTextBox.Text = results.ACVoltage.ToString("E")
        Me.acCurrentTextBox.Text = results.ACCurrent.ToString("E")
        Me.zTextBox.Text = results.Z.ToString("E")
        Me.zMagTextBox.Text = results.ZMagnitude.ToString("E")
        Me.zThetaTextBox.Text = results.ZPhase.ToString("E")
        Me.yTextBox.Text = results.Y.ToString("E")
        Me.yMagTextBox.Text = results.YMagnitude.ToString("E")
        Me.yThetaTextBox.Text = results.YPhase.ToString("E")
        Me.lsTextBox.Text = results.Ls.ToString("E")
        Me.csTextBox.Text = results.Cs.ToString("E")
        Me.rsTextBox.Text = results.Rs.ToString("E")
        Me.lpTextBox.Text = results.Lp.ToString("E")
        Me.cpTextBox.Text = results.Cp.ToString("E")
        Me.rpTextBox.Text = results.Rp.ToString("E")
        Me.dTextBox.Text = results.D.ToString("E")
        Me.qTextBox.Text = results.Q.ToString("E")
        Me.measurementModeTextBox.Text = results.measurementMode.ToString()
        Me.dcInComplianceButtonLed.BackColor = If(results.dcInCompliance, Color.Red, SystemColors.Control)
        Me.acInComplianceButtonLed.BackColor = If(results.acInCompliance, Color.Red, SystemColors.Control)
        Me.unbalancedButtonLed.BackColor = If(results.unbalanced, Color.Red, SystemColors.Control)
    End Sub

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub ChangeControlState(isEnabled As Boolean)
        Me.configurationGroupBox.Enabled = isEnabled
        Me.startButton.Enabled = isEnabled
        Me.resourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub
End Class