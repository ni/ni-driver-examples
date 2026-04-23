Imports System
Imports System.Windows.Forms
Imports Ivi.Swtch
Imports System.Collections.Generic
Imports System.Linq
Imports System.Resources
Imports System.Reflection
Imports Microsoft.VisualBasic

Namespace NationalInstruments.Examples.IviSwitch
    Partial Public Class ConfigureIviSwitch
        Inherits Form
        Private switchSession As Ivi.Swtch.IIviSwtch
        Private numberOfChannels As Integer


        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(ByVal switchSession As Ivi.Swtch.IIviSwtch)
            Me.switchSession = switchSession
            InitializeComponent()
            LoadActions()
            LoadChannels()
        End Sub

        Private Sub LoadChannels()
            Dim x As List(Of IIviSwtchChannel) = switchSession.Channels.ToList()
            Dim channelList1 As New List(Of String)()
            Dim channelList2 As New List(Of String)()
            numberOfChannels = switchSession.Channels.Count


            For i As Integer = 0 To numberOfChannels - 1
                channelList1.Add(x(i).Name)
                channelList2.Add(x(i).Name)
            Next
            ' channel1ComboBox.DisplayMember = "Key";
            ' channel1ComboBox.ValueMember = "Value";
            channel1ComboBox.DataSource = channelList1
            channel1ComboBox.SelectedIndex = 10
            'channel2ComboBox.DisplayMember = "Key";
            'channel2ComboBox.ValueMember = "Value";
            channel2ComboBox.DataSource = channelList2
            channel2ComboBox.SelectedIndex = 0
            ' throw new NotImplementedException();
        End Sub

        Private Sub LoadActions()
            Dim actionsList As New List(Of KeyValuePair(Of String, Integer))()
            actionsList.Add(New KeyValuePair(Of String, Integer)("Connect Channels", 0))
            actionsList.Add(New KeyValuePair(Of String, Integer)("Disconnect Specific Channels", 1))
            actionsList.Add(New KeyValuePair(Of String, Integer)("Disconnect All Channels", 2))
            actionsList.Add(New KeyValuePair(Of String, Integer)("Get Switch Path", 3))

            selectActionComboBox.DisplayMember = "Key"
            selectActionComboBox.ValueMember = "Value"
            selectActionComboBox.DataSource = actionsList
            selectActionComboBox.SelectedIndex = 0

        End Sub

        Private Sub runPanelButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles runPanelButton.Click
            Dim canConnect As PathCapability
            Dim switchPath As String()
            Dim selectedEvent As Integer = Action
            Dim resourceMnger As New ResourceManager("NationalInstruments.Examples.IviSwitch.Resources", Assembly.GetExecutingAssembly())

            Try

                Select Case selectedEvent
                    Case 0
                        canConnect = switchSession.Path.CanConnect(Channel1, Channel2)

                        If canConnect = PathCapability.Available Then
                            switchSession.Path.Connect(Channel1, Channel2)
                            switchPath = switchSession.Path.GetPath(Channel1, Channel2)
                            switchPathTextBox.Text = String.Join(",", switchPath)

                            canConnectRichTextBox.Text = resourceMnger.GetString("Available")
                        Else
                            switchPathTextBox.Text = String.Empty
                        End If
                        If canConnect = PathCapability.Exists Then

                            canConnectRichTextBox.Text = resourceMnger.GetString("Exists")
                        End If
                        If canConnect = PathCapability.ResourceInUse Then

                            canConnectRichTextBox.Text = resourceMnger.GetString("InUse")
                        End If
                        If canConnect = PathCapability.Unsupported Then

                            canConnectRichTextBox.Text = resourceMnger.GetString("Unsupported")
                        End If
                        If canConnect = PathCapability.SourceConflict Then

                            canConnectRichTextBox.Text = resourceMnger.GetString("Conflict")
                        End If
                        Exit Select

                    Case 1
                        switchSession.Path.Disconnect(Channel1, Channel2)
                        switchPathTextBox.Text = String.Empty
                        canConnectRichTextBox.Text = String.Empty

                        Exit Select

                    Case 2
                        switchSession.Path.DisconnectAll()
                        switchPathTextBox.Text = String.Empty
                        canConnectRichTextBox.Text = String.Empty

                        Exit Select
                    Case 3
                        switchPath = switchSession.Path.GetPath(Channel1, Channel2)
                        switchPathTextBox.Text = String.Join(",", switchPath)
                        canConnectRichTextBox.Text = String.Empty

                        Exit Select
                End Select

                resultActionPanel.BackColor = System.Drawing.Color.LimeGreen
            Catch ex As Exception
                resultActionPanel.BackColor = System.Drawing.Color.DarkOliveGreen
                ShowError(ex.Message)
                CloseSession()
            End Try
        End Sub

        Public ReadOnly Property Action() As Integer
            Get
                Return CInt(selectActionComboBox.SelectedValue)
            End Get
        End Property

        Public ReadOnly Property Channel1() As String
            Get
                Return DirectCast(channel1ComboBox.SelectedItem, String)
            End Get
        End Property
        Public ReadOnly Property Channel2() As String
            Get
                Return DirectCast(channel2ComboBox.SelectedItem, String)
            End Get
        End Property

        Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
            CloseSession()
        End Sub

        Private Sub closeButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles closeButton.Click
            CloseSession()
        End Sub

        Private Sub CloseSession()
            If switchSession IsNot Nothing Then
                Try
                    switchSession.Close()

                    switchSession = Nothing
                Catch ex As System.Exception
                    ShowError("Unable to Close Session, Reset the device." & vbLf & "Error : " & ex.Message)
                    Application.[Exit]()
                Finally
                    Me.Close()
                End Try
            End If
        End Sub

        Private Shared Sub ShowError(ByVal message As String)
            If String.IsNullOrEmpty(message) Then
                message = "Unexpected Error"
            End If
            MessageBox.Show(message, "Error")


        End Sub






    End Class
End Namespace