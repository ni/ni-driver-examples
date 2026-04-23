Imports System.Drawing
Imports System.Windows.Forms
Imports Ivi.Swtch
Imports System
Imports Microsoft.VisualBasic

Namespace NationalInstruments.Examples.IviSwitch
    Partial Public Class MainForm
        Inherits Form
        Private switchSession As IIviSwtch

        Public Sub New()
            InitializeComponent()
        End Sub
#Region "Program Properties"
        Private ReadOnly Property ResourceName() As String
            Get
                Return Me.resourceNameTextBox.Text
            End Get
        End Property

        Private ReadOnly Property IDQuery() As Boolean
            Get
                Return Me.idQueryCheckBox.Checked
            End Get
        End Property
        Private ReadOnly Property ResetDevice() As Boolean
            Get
                Return Me.resetDeviceCheckBox.Checked
            End Get
        End Property
#End Region

#Region "FormEvents"


        Private Sub initializeButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles initializeButton.Click
            Try

                switchSession = IviSwtch.Create(ResourceName, IDQuery, ResetDevice)

                Dim X As New ConfigureIviSwitch(switchSession)
                switchSession.Path.CanConnect("ab0", "ch0")
                X.Show()
            Catch ex As Exception
                ShowError(ex.Message)
                CloseSession()
            End Try



        End Sub
        Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
            CloseSession()
        End Sub
#End Region
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
        Private Shared Sub ShowError(ByVal message As String)
            If String.IsNullOrEmpty(message) Then
                message = "Unexpected Error"
            End If
            MessageBox.Show(message, "Error")


        End Sub


    End Class
End Namespace