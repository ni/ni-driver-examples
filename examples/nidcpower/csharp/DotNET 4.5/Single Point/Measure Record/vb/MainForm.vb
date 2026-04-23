'=============================================================================================================
'
' Title:
'      NI-DCPower Measure Record
'
' Description:
'      This example demonstrates how to take multiple measurements in succesion
'      by using a measure record.
'
'      Note: In this example the Output Function is set to DC Voltage. If you change the
'      Output Function to DC Current, you must use Current Level and Voltage Limit instead
'      of Voltage Level and Current Limit.
'
'  Suggested Devices:
'      PXI-4132
'      PXIe-4112, PXIe-4113, PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4154, PXIe-4162, PXIe-4163
'
'=============================================================================================================

Imports System.Threading
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private dcPowerSession As NIDCPower
    Private stopFetch As Boolean

    Public Sub New()
        InitializeComponent()
        ConfigureAutoZeroComboBox()
        LoadDCPowerDeviceNames()
    End Sub

#Region "MainForm initial configuration"
    Private Sub ConfigureAutoZeroComboBox()
        For Each item As DCPowerMeasurementAutoZero In [Enum].GetValues(GetType(DCPowerMeasurementAutoZero))
            autoZeroComboBox.Items.Add(item)
        Next
        autoZeroComboBox.SelectedIndex = 0
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

#Region "MainForm initial values"
    Private ReadOnly Property ResourceName() As String
        Get
            Return GetResourceNameSafe()
        End Get
    End Property

    Private ReadOnly Property ChannelName() As String
        Get
            Return GetChannelNameSafe()
        End Get
    End Property

    Private ReadOnly Property FullyQualifiedChannelName() As String
        Get
            Return $"{ResourceName}/{ChannelName}"
        End Get
    End Property

    Private ReadOnly Property VoltageLevel() As Double
        Get
            Return GetVoltageLevelSafe()
        End Get
    End Property

    Private ReadOnly Property MeasureRecordLength() As Integer
        Get
            Return GetMeasureRecordLengthSafe()
        End Get
    End Property

    Private ReadOnly Property IsMeasureRecordFinite() As Boolean
        Get
            Return GetIsMeasureRecordFiniteSafe()
        End Get
    End Property

    Private ReadOnly Property AutoZero() As DCPowerMeasurementAutoZero
        Get
            Return GetAutoZeroSafe()
        End Get
    End Property
#End Region

#Region "Thread-safe calls to MainForm controls"
    Private Delegate Function GetResourceNameSafeCallBack() As String
    Private Function GetResourceNameSafe() As String
        If Me.resourceNameComboBox.InvokeRequired Then
            Return Me.Invoke(New GetResourceNameSafeCallBack(AddressOf GetResourceNameSafe), Nothing).ToString()
        Else
            Return Me.resourceNameComboBox.Text
        End If
    End Function

    Private Delegate Function GetChannelNameSafeCallBack() As String
    Private Function GetChannelNameSafe() As String
        If Me.channelNameTextBox.InvokeRequired Then
            Return Me.Invoke(New GetChannelNameSafeCallBack(AddressOf GetChannelNameSafe), Nothing).ToString()
        Else
            Return Me.channelNameTextBox.Text
        End If
    End Function

    Private Delegate Function GetVoltageLevelSafeCallBack() As Double
    Private Function GetVoltageLevelSafe() As Double
        If Me.voltageLevelNumeric.InvokeRequired Then
            Return CDbl(Me.Invoke(New GetVoltageLevelSafeCallBack(AddressOf GetVoltageLevelSafe), Nothing))
        Else
            Return Decimal.ToDouble(Me.voltageLevelNumeric.Value)
        End If
    End Function

    Private Delegate Function GetMeasureRecordLengthSafeCallBack() As Integer
    Private Function GetMeasureRecordLengthSafe() As Integer
        If Me.measureRecordLengthNumeric.InvokeRequired Then
            Return CInt(Me.Invoke(New GetMeasureRecordLengthSafeCallBack(AddressOf GetMeasureRecordLengthSafe), Nothing))
        Else
            Return Decimal.ToInt32(Me.measureRecordLengthNumeric.Value)
        End If
    End Function

    Private Delegate Function GetIsMeasureRecordFiniteCallBack() As Boolean
    Private Function GetIsMeasureRecordFiniteSafe() As Boolean
        If Me.isMeasureRecordFiniteCheckBox.InvokeRequired Then
            Return CBool(Me.Invoke(New GetIsMeasureRecordFiniteCallBack(AddressOf GetIsMeasureRecordFiniteSafe), Nothing))
        Else
            Return Me.isMeasureRecordFiniteCheckBox.Checked
        End If
    End Function

    Private Delegate Function GetAutoZeroSafeCallBack() As DCPowerMeasurementAutoZero
    Private Function GetAutoZeroSafe() As DCPowerMeasurementAutoZero
        If Me.autoZeroComboBox.InvokeRequired Then
            Return CType(Me.Invoke(New GetAutoZeroSafeCallBack(AddressOf GetAutoZeroSafe), Nothing), DCPowerMeasurementAutoZero)
        Else
            Return CType(Me.autoZeroComboBox.SelectedItem, DCPowerMeasurementAutoZero)
        End If
    End Function

    Private Delegate Sub ChangeControlStateSafeCallBack(ByVal isEnabled As Boolean)
    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        If resourceNameComboBox.InvokeRequired Then
            Me.Invoke(New ChangeControlStateSafeCallBack(AddressOf ChangeControlState), New Object() {isEnabled})
        Else
            resourceNameAndChannelNameGroupBox.Enabled = isEnabled
            configurationGroupBox.Enabled = isEnabled
            startButton.Enabled = isEnabled
            stopButton.Enabled = Not isEnabled
            resourceNameComboBox.[Select]()
            Me.Refresh()
        End If
    End Sub

    Private Delegate Sub UpdateMeasurementsSafeCallBack(ByVal voltageMeasurements As Double(), ByVal currentMeasurements As Double())
    Private Sub UpdateMeasurements(ByVal voltageMeasurements As Double(), ByVal currentMeasurements As Double())
        If measurementRateTextBox.InvokeRequired Then
            Me.Invoke(New UpdateMeasurementsSafeCallBack(AddressOf UpdateMeasurements), New Object() {voltageMeasurements, currentMeasurements})
        Else
            Dim i As Integer = 0
            While i < voltageMeasurements.Length AndAlso i < currentMeasurements.Length
                measurementsDataGridView.Rows.Add((measurementsDataGridView.Rows.Count + 1).ToString(), voltageMeasurements(i).ToString("E"), currentMeasurements(i).ToString("E"))
                i += 1
            End While
        End If
    End Sub

    Private Delegate Sub UpdateMeasurementRateCallBack(ByVal measurementRate As Double)
    Private Sub UpdateMeasurementRate(ByVal measurementRate As Double)
        If measurementRateTextBox.InvokeRequired Then
            Me.Invoke(New UpdateMeasurementRateCallBack(AddressOf UpdateMeasurementRate), New Object() {measurementRate})
        Else
            measurementRateTextBox.Text = measurementRate.ToString("E")
        End If
    End Sub
#End Region

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startButton.Click
        ChangeControlState(False)
        ClearMeasurementsDataGridView()
        stopFetch = False

        ' Create a new thread to perform acquisition.
        ThreadPool.QueueUserWorkItem(AddressOf AcquisitionThreadFunction)
    End Sub

    Private Sub mainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles stopButton.Click
        stopFetch = True
    End Sub

    Private Sub AcquisitionThreadFunction(userState As Object)
        Try
            InitializeDCPowerSession()

            dcPowerSession.Source.Mode = DCPowerSourceMode.SinglePoint
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.[Function] = DCPowerSourceOutputFunction.DCVoltage
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevel = VoltageLevel
            dcPowerSession.Outputs(FullyQualifiedChannelName).Measurement.RecordLength = MeasureRecordLength
            dcPowerSession.Outputs(FullyQualifiedChannelName).Measurement.IsRecordLengthFinite = IsMeasureRecordFinite
            dcPowerSession.Outputs(FullyQualifiedChannelName).Measurement.MeasureWhen = DCPowerMeasurementWhen.AutomaticallyAfterSourceComplete

            dcPowerSession.Outputs(FullyQualifiedChannelName).Measurement.AutoZero = AutoZero
            dcPowerSession.Control.Commit()

            ' Display measurement rate in Mainform.
            Dim measureRecordDeltaTime As Double = dcPowerSession.Outputs(FullyQualifiedChannelName).Measurement.RecordDeltaTime
            UpdateMeasurementRate(1.0 / measureRecordDeltaTime)

            dcPowerSession.Control.Initiate()

            Dim backlog As Integer = 0
            Dim totalMeasurementsFetched As Integer = 0
            While Not stopFetch AndAlso (Not IsMeasureRecordFinite OrElse totalMeasurementsFetched < MeasureRecordLength)
                backlog = dcPowerSession.Measurement.FetchBacklog
                If backlog > 0 Then
                    Dim result As DCPowerFetchResult = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, New PrecisionTimeSpan(1.0), backlog)
                    totalMeasurementsFetched += result.VoltageMeasurements.Length
                    UpdateMeasurements(result.VoltageMeasurements, result.CurrentMeasurements)
                End If
            End While
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub InitializeDCPowerSession()
        dcPowerSession = New NIDCPower(FullyQualifiedChannelName, False, String.Empty)
        AddHandler dcPowerSession.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)
    End Sub

    Private Sub DCPowerDriverOperationWarning(ByVal sender As Object, ByVal e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub ClearMeasurementsDataGridView()
        measurementsDataGridView.Rows.Clear()
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

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

End Class