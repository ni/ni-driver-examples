'==================================================================================================
' Title        : Analog Bus Sharing Enable NI2815
' Description  : Use this example to learn how to enable sharing and connect of analog bus channels of two different devices.
'                The example shows how to configure NI-Switch to facilitate enabling of analog bus channels, Reserve specific channels for routing ,
'                Connect and Disconnect the channels and Reset the two Devices.
'==================================================================================================
Imports System
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NISwitch
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private switchSession1 As NISwitch
    Private switchSession2 As NISwitch
    Private analogBusChannels As New List(Of String)() From { _
     "ab0", _
     "ab1", _
     "ab2", _
     "ab3" _
    }
    Private rowChannels As New List(Of String)() From { _
     "card1r0", _
     "card1r1", _
     "card1r2", _
     "card1r3" _
    }
    Private maximumTime As New PrecisionTimeSpan(5)

    Public Sub New()
        InitializeComponent()
        LoadSwitchDeviceNames()
        LoadTopology()
        LoadAnalogBusChannels()
        LoadColumnChannels()
        ToggleConnectDisconnectButton(True)
    End Sub

#Region "UI Initial Value Config Section"

    Private Sub ToggleConnectDisconnectButton(ByVal state As Boolean)
        connectButton.Enabled = state
        disconnectButton.Enabled = Not state
    End Sub

    Private Sub LoadAnalogBusChannels()
        For Each channel As String In analogBusChannels
            analogBusChannelsComboBox.Items.Add(channel)
        Next
        analogBusChannelsComboBox.SelectedIndex = 0
    End Sub

    Private Sub LoadColumnChannels()
        Const columnChannelPrefix As Char = "c"c
        Dim channelPrefix As String
        For i As Integer = 0 To 85
            channelPrefix = columnChannelPrefix & "" & i
            columnChannel1ComboBox.Items.Add(channelPrefix)
            columnChannel2ComboBox.Items.Add(channelPrefix)
        Next
        columnChannel1ComboBox.SelectedIndex = 0
        columnChannel2ComboBox.SelectedIndex = 0
    End Sub

    Private Sub LoadTopology()
        Dim myType As Type = GetType(SwitchDeviceTopology)
        Dim properties As PropertyInfo() = myType.GetProperties()

        For Each prop As PropertyInfo In properties
            topologyName1ComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
            topologyName2ComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
        Next
        topologyName1ComboBox.SelectedIndex = 0
        topologyName2ComboBox.SelectedIndex = 0
    End Sub

    Private Sub LoadSwitchDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-SWITCH")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            resourceName1ComboBox.Items.Add(device.Name)
            resourceName2ComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            resourceName1ComboBox.SelectedIndex = 0
            resourceName2ComboBox.SelectedIndex = 0
        End If
    End Sub

#End Region

#Region "Program Properties"

    Private ReadOnly Property ResourceName1() As String
        Get
            Return Me.resourceName1ComboBox.Text
        End Get
    End Property

    Private ReadOnly Property ResourceName2() As String
        Get
            Return Me.resourceName2ComboBox.Text
        End Get
    End Property

    Private ReadOnly Property TopologyName1() As String
        Get
            Return Me.topologyName1ComboBox.SelectedItem.ToString()
        End Get
    End Property

    Private ReadOnly Property TopologyName2() As String
        Get
            Return Me.topologyName2ComboBox.SelectedItem.ToString()
        End Get
    End Property

    Private ReadOnly Property ColumnChannel1() As String
        Get
            Return Me.columnChannel1ComboBox.Text
        End Get
    End Property

    Private ReadOnly Property ColumnChannel2() As String
        Get
            Return Me.columnChannel2ComboBox.Text
        End Get
    End Property

    Private ReadOnly Property AnalogBusChannel() As String
        Get
            Return Me.analogBusChannelsComboBox.SelectedItem.ToString()
        End Get
    End Property

#End Region

#Region "FormEvents"

    Private Sub connectButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles connectButton.Click
        ' Disable all controls till connect is performed
        ChangeControlState(False)
        Try
            CloseSession(switchSession1)
            CloseSession(switchSession2)

            'Open session to two switch modules and set topology
            switchSession1 = InitializeSwitchSession(ResourceName1, TopologyName1)
            switchSession2 = InitializeSwitchSession(ResourceName2, TopologyName2)

            'Mark analog bus channels as Analog Bus Sharing Enable
            For Each channel As String In analogBusChannels
                switchSession1.Channels(channel).AnalogBusSharingEnable = True
                switchSession2.Channels(channel).AnalogBusSharingEnable = True
            Next
            'Mark rows as Reserved For Routing.
            For Each channel As String In rowChannels
                switchSession1.Channels(channel).IsConfigurationChannel = True
                switchSession2.Channels(channel).IsConfigurationChannel = True
            Next

            'Connect specified channels in Device 1.
            switchSession1.Path.Connect(ColumnChannel1, AnalogBusChannel)
            'Wait for relay(s) to activate and debounce.
            switchSession1.Path.WaitForDebounce(maximumTime)

            'Connect specified channels in Device 2.
            switchSession2.Path.Connect(ColumnChannel2, AnalogBusChannel)
            'Wait for relay(s) to activate and debounce.
            switchSession2.Path.WaitForDebounce(maximumTime)

            'Enable Disconnect when Connect is performed.
            ToggleConnectDisconnectButton(False)
        Catch ex As System.Exception
            ShowError(ex.Message)
            CloseSession(switchSession1)
            CloseSession(switchSession2)
            ChangeControlState(True)
            ToggleConnectDisconnectButton(True)
        End Try

    End Sub

    Private Sub disconnectButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles disconnectButton.Click

        Try
            'Disconnect specified channels.
            switchSession1.Path.Disconnect(ColumnChannel1, AnalogBusChannel)
            switchSession2.Path.Disconnect(ColumnChannel2, AnalogBusChannel)

            'Reset the switch modules to disable analog bus sharing.
            switchSession1.Utility.Reset()
            switchSession2.Utility.Reset()



            ToggleConnectDisconnectButton(True)
        Catch ex As System.Exception
            ShowError(ex.Message)
        Finally
            'Close session to switch module.
            CloseSession(switchSession1)
            CloseSession(switchSession2)
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession(switchSession1)
        CloseSession(switchSession2)
    End Sub

#End Region

#Region "Program Functions"

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Dim connectButtonState As Boolean = Me.connectButton.Enabled
        Me.connectButton.Enabled = isEnabled
        Me.disconnectButton.Enabled = isEnabled
        Me.resourceName1ComboBox.Enabled = isEnabled
        Me.resourceName2ComboBox.Enabled = isEnabled
        Me.topologyName1ComboBox.Enabled = isEnabled
        Me.topologyName2ComboBox.Enabled = isEnabled
        Me.columnChannel1ComboBox.Enabled = isEnabled
        Me.columnChannel2ComboBox.Enabled = isEnabled
        Me.analogBusChannelsComboBox.Enabled = isEnabled
        If isEnabled Then
            Me.connectButton.Enabled = connectButtonState
            Me.disconnectButton.Enabled = Not connectButtonState
        End If
    End Sub

    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")
    End Sub

    Private Function InitializeSwitchSession(ByVal resourceName As String, ByVal topologyName As String) As NISwitch
        Dim switchSession As NISwitch
        switchSession = New NISwitch(resourceName, topologyName, False, True)
        AddHandler switchSession.DriverOperation.Warning, New System.EventHandler(Of SwitchWarningEventArgs)(AddressOf Me.DriverOperationWarning)
        Return switchSession
    End Function

    Private Sub DriverOperationWarning(ByVal sender As Object, ByVal e As SwitchWarningEventArgs)
        MessageBox.Show(e.ToString(), "Warning")
    End Sub

    Private Shared Sub CloseSession(ByVal switchSession As NISwitch)
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

