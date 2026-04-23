'==================================================================================================
' Title        : Controlling An Individual Relay
' Description  : Use this example to learn how to control individual relay of a switch device .
'The example shows how to open and close a relay of a NI-Switch device.          
'==================================================================================================
 
Imports System.Reflection
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NISwitch
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices


Partial Public Class MainForm
    Inherits Form
    Private switchSession As NISwitch
    Private maximumTime As New PrecisionTimeSpan(5)

    Public Sub New()
        InitializeComponent()
        LoadSwitchDeviceNames()
        LoadTopology()
    End Sub

#Region "UI Initial Value Config Section"

    Private Sub LoadTopology()
        Dim myType As Type = GetType(SwitchDeviceTopology)
        Dim properties As PropertyInfo() = myType.GetProperties()

        For Each prop As PropertyInfo In properties
            topologyNameComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
        Next
        topologyNameComboBox.SelectedIndex = 0
    End Sub

    Private Sub LoadSwitchDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-SWITCH")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            resourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub

#End Region

#Region "Program Properties"

    Private ReadOnly Property ResourceName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property TopologyName() As String
        Get
            Return Me.topologyNameComboBox.SelectedItem.ToString()
        End Get
    End Property

    Private ReadOnly Property RelayName() As String
        Get
            Return Me.relayNameTextBox.Text
        End Get
    End Property

#End Region

#Region "FormEvents"

    Private Sub openRelayButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles openRelayButton.Click
        ChangeControlState(False)
        Try
            ChangeRelayPosition(SwitchRelayAction.OpenRelay)
        Catch ex As System.Exception
            ShowError(ex.Message)
        Finally
            'Close session to switch module.
            CloseSession()
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub ChangeRelayPosition(ByVal position As SwitchRelayAction)
        CloseSession()
        'Open session to the switch module and sets topology
        InitializeSwitchSession()
        ' Open the relay.
        switchSession.RelayOperations.RelayControl(RelayName, position)
        ' Wait for the relay to activate and debounce.
        switchSession.Path.WaitForDebounce(maximumTime)
    End Sub

    Private Sub closeRelayButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles closeRelayButton.Click
        ChangeControlState(False)
        Try
            ChangeRelayPosition(SwitchRelayAction.CloseRelay)
        Catch ex As System.Exception
            ShowError(ex.Message)
        Finally
            'Close session to switch module.
            CloseSession()
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

#End Region

#Region "Program Functions"

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.openRelayButton.Enabled = isEnabled
        Me.closeRelayButton.Enabled = isEnabled
        Me.resourceNameComboBox.Enabled = isEnabled
        Me.topologyNameComboBox.Enabled = isEnabled
        Me.relayNameTextBox.Enabled = isEnabled
    End Sub

    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")
    End Sub

    Private Sub InitializeSwitchSession()
        switchSession = New NISwitch(ResourceName, TopologyName, False, True)
        AddHandler switchSession.DriverOperation.Warning, New System.EventHandler(Of SwitchWarningEventArgs)(AddressOf DriverOperationWarning)
    End Sub

    Private Sub DriverOperationWarning(ByVal sender As Object, ByVal e As SwitchWarningEventArgs)
        MessageBox.Show(e.ToString(), "Warning")
    End Sub

    Private Sub CloseSession()
        If switchSession IsNot Nothing Then
            Try
                switchSession.Close()
                switchSession = Nothing
            Catch ex As System.Exception
                ShowError("Unable to Close Session, Reset the device." & vbLf & "Error : " & ex.Message)
                Application.[Exit]()
            End Try
        End If
    End Sub

#End Region

End Class

