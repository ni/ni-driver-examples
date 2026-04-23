'=========================================================================================================
'
' Title:
'      Calibrate
'
' Description:
'      This example performs a self-calibration of your digitizer.  Disconnect or disable 
'      any AC input signals before starting self-calibration.  AC or varying signals can,
'      in some cases, cause self-calibration to fail or compromise the accuracy of the
'      calibration.
'
'=========================================================================================================

Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIScope
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private scopeSession As NIScope

    Public Sub New()
        InitializeComponent()
        ConfigureOptionComboBox()
        LoadScopeDeviceNames()
        ChangeControlState(True)
    End Sub

#Region "Mainform Initial Configuration"
    Private Sub ConfigureOptionComboBox()
        For Each value As ScopeSelfCalibrationOption In [Enum].GetValues(GetType(ScopeSelfCalibrationOption))
            optionComboBox.Items.Add(value)
        Next
        optionComboBox.SelectedIndex = 0
    End Sub

    Private Sub LoadScopeDeviceNames()
        Using scopeDevices As New ModularInstrumentsSystem("NI-Scope")
            For Each device As DeviceInfo In scopeDevices.DeviceCollection
                resourceNameComboBox.Items.Add(device.Name)
            Next
        End Using
        If resourceNameComboBox.Items.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub
#End Region

#Region "Mainform Configuration values"
    Private ReadOnly Property ResourceName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property [Option]() As ScopeSelfCalibrationOption
        Get
            Return CType(Me.optionComboBox.SelectedItem, ScopeSelfCalibrationOption)
        End Get
    End Property
#End Region

    Private Sub calibrateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles calibrateButton.Click
        SelfCalibrate()
    End Sub

    Private Sub mainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As ScopeWarningEventArgs)
        MessageBox.Show(e.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub InitializeSession()
        scopeSession = New NIScope(ResourceName, False, False)
        AddHandler scopeSession.DriverOperation.Warning, New EventHandler(Of ScopeWarningEventArgs)(AddressOf DriverOperation_Warning)
    End Sub

    Private Sub SelfCalibrate()
        ChangeControlState(False)
        DisplayMessage("Calibration in Progress (May take a couple of mintues)...")

        Try
            InitializeSession()
            scopeSession.Calibration.Self.SelfCalibrate([Option])
            DisplayMessage("Calibration successful!")
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub DisplayMessage(ByVal message As String)
        messageTextBox.Text = message
        Me.Refresh()
    End Sub

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        resourceNameComboBox.Enabled = isEnabled
        optoinGroupBox.Enabled = isEnabled
        Me.Refresh()
    End Sub

    Private Sub CloseSession()
        If scopeSession IsNot Nothing Then
            Try
                scopeSession.Close()
                scopeSession = Nothing
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
