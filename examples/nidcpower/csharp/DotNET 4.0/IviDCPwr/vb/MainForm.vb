'=============================================================================================================
'
' Title:
'     IVI DCPower .NET application
'      
' Description:
'      This example is a IVI DCPower .NET application. This example uses only those
'      interfaces from the IVI class for DC Power Supplies to configure the DCPower device.
'
'============================================================================================================

Imports System.Windows.Forms
Imports Ivi.DCPwr

Partial Public Class MainForm
    Inherits Form
    Private iviDCPwrSession As IIviDCPwr

    Public Sub New()
        InitializeComponent()
        configureDCPowerGroupBox.Enabled = False
    End Sub

#Region "MainForm initial configuration"
    Private Sub ConfigureCurrentLimitComboBox()
        For Each item As CurrentLimitBehavior In [Enum].GetValues(GetType(CurrentLimitBehavior))
            currentLimitBehaviorComboBox.Items.Add(item)
        Next
        currentLimitBehaviorComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureChannelNameComboBox()
        For Each channel As IIviDCPwrOutput In iviDCPwrSession.Outputs
            channelNameComboBox.Items.Add(channel.Name)
        Next
        If channelNameComboBox.Items.Count > 0 Then
            channelNameComboBox.SelectedIndex = 0
        End If
    End Sub

    Private Sub ChangeControlToConfiguration()
        ConfigureChannelNameComboBox()
        ConfigureCurrentLimitComboBox()
        initializeDCPowerGroupBox.Enabled = False
        configureDCPowerGroupBox.Enabled = True
        channelNameComboBox.[Select]()
        Me.Refresh()
    End Sub
#End Region

#Region "MainForm configuration values"
    Private ReadOnly Property LogicalName() As String
        Get
            Return Me.logicalNameTextBox.Text
        End Get
    End Property

    Private ReadOnly Property ChannelName() As String
        Get
            Return Me.channelNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property IdQuery() As Boolean
        Get
            Return Me.idQueryCheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property ResetDevice() As Boolean
        Get
            Return Me.resetDeviceCheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property OvpEnabled() As Boolean
        Get
            Return Me.outputEnabledCheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property OutputEnabled() As Boolean
        Get
            Return Me.outputEnabledCheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property VoltageLevel() As Double
        Get
            Return Decimal.ToDouble(Me.voltageLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property OvpLimit() As Double
        Get
            Return Decimal.ToDouble(Me.ovpLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CurrentLimit() As Double
        Get
            Return Decimal.ToDouble(Me.currentLimitNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CurrentLimitBehavior() As CurrentLimitBehavior
        Get
            Return CType(Me.currentLimitBehaviorComboBox.SelectedItem, CurrentLimitBehavior)
        End Get
    End Property
#End Region

    Private Sub configureAndOutputButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles configureAndOutputButton.Click
        ConfigureAndOutput()
    End Sub

    Private Sub initializeButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles initializeButton.Click
        Initialize()
    End Sub

    Private Sub mainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub Initialize()
        Try
            iviDCPwrSession = IviDCPwr.Create(LogicalName, IdQuery, ResetDevice)
            ChangeControlToConfiguration()
            ShowOperationSuccessful("Initialize()")
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub ConfigureAndOutput()
        Try
            ' Disable the output to prevent accidental trip conditions.
            iviDCPwrSession.Outputs(ChannelName).Enabled = False

            If OutputEnabled Then
                ' Set voltage to 0.0 to prevent accidental trip conditions.
                iviDCPwrSession.Outputs(ChannelName).VoltageLevel = 0.0

                iviDCPwrSession.Outputs(ChannelName).ConfigureCurrentLimit(CurrentLimitBehavior, CurrentLimit)
                iviDCPwrSession.Outputs(ChannelName).ConfigureOvp(OvpEnabled, OvpLimit)
                iviDCPwrSession.Outputs(ChannelName).VoltageLevel = VoltageLevel
                iviDCPwrSession.Outputs(ChannelName).Enabled = True
            End If

            ShowOperationSuccessful("ConfigureAndOutput()")
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub CloseSession()
        If iviDCPwrSession IsNot Nothing Then
            Try
                iviDCPwrSession.Close()
                iviDCPwrSession = Nothing
            Catch ex As Exception
                ShowError(ex)
                Application.[Exit]()
            End Try
        End If
    End Sub

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Shared Sub ShowOperationSuccessful(ByVal operation As String)
        MessageBox.Show(operation & " completed successfully.")
    End Sub

End Class
