'==================================================================================================
' Title        : Making Connections On A Switch
' Description  : Use this example to learn how to connect and disconnect channels on a NI-Switch device.        
'==================================================================================================
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Reflection
Imports NationalInstruments
Imports NationalInstruments.ModularInstruments.NISwitch
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices


Partial Public Class MainForm
    Inherits Form
    Private switchSession As NISwitch
    Private maxTime As New PrecisionTimeSpan(5)
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
    Private ReadOnly Property Channel1() As String
        Get
            Return Me.channel1TextBox.Text
        End Get
    End Property
    Private ReadOnly Property Channel2() As String
        Get
            Return Me.channel2TextBox.Text
        End Get
    End Property
    Private ReadOnly Property Channel3() As String
        Get
            Return Me.channel3TextBox.Text
        End Get
    End Property
    Private ReadOnly Property Channel4() As String
        Get
            Return Me.channel4TextBox.Text
        End Get
    End Property
#End Region

#Region "FormEvents"
    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub
    Private Sub connectButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles connectButton.Click
        ChangeControlState(False)
        Try
            CloseSession()

            'Open session to the switch module and sets topology
            InitializeSwitchSession()
            'Connect channel1 and channel2.    
            switchSession.Path.Connect(Channel1, Channel2)
            ' Wait for any relay to activate and debounce.
            switchSession.Path.WaitForDebounce(maxTime)

            'Connect channel3 and channel4  
            switchSession.Path.Connect(Channel3, Channel4)
            ' Wait for any relay to activate and debounce.
            switchSession.Path.WaitForDebounce(maxTime)
        Catch ex As System.Exception
            ShowError(ex.Message)
        Finally
            ChangeControlState(True)
            'Close session to switch module.
            CloseSession()
        End Try
    End Sub
#End Region
#Region "Program Functions"
    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.connectButton.Enabled = isEnabled
        Me.resourceNameComboBox.Enabled = isEnabled
        Me.topologyNameComboBox.Enabled = isEnabled
        Me.channel1TextBox.Enabled = isEnabled
        Me.channel2TextBox.Enabled = isEnabled
        Me.channel3TextBox.Enabled = isEnabled
        Me.channel4TextBox.Enabled = isEnabled
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
