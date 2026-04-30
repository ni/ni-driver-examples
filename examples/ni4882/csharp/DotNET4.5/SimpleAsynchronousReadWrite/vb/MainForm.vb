Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports NationalInstruments.NI4882


Public Class MainForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "
    Private WithEvents openButton As System.Windows.Forms.Button
    Private WithEvents closeButton As System.Windows.Forms.Button
    Private stringToWriteTextBox As System.Windows.Forms.TextBox
    Private WithEvents writeButton As System.Windows.Forms.Button
    Private stringToWriteLabel As System.Windows.Forms.Label
    Private WithEvents readButton As System.Windows.Forms.Button
    Private stringReadLabel As System.Windows.Forms.Label
    Private GpibDevice As Device
    Private boardIdNumericUpDown As System.Windows.Forms.NumericUpDown
    Private boardIdLabel As System.Windows.Forms.Label
    Private primaryAddressNumericUpDown As System.Windows.Forms.NumericUpDown
    Private primaryAddressLabel As System.Windows.Forms.Label
    Private secondaryAddressLabel As System.Windows.Forms.Label
    Private stringReadTextBox As System.Windows.Forms.TextBox
    Private WithEvents terminateButton As System.Windows.Forms.Button
    Private elementsTransferredTextBox As System.Windows.Forms.TextBox
    Private lastIOStatusTextBox As System.Windows.Forms.TextBox
    Private elementsTransferredLabel As System.Windows.Forms.Label
    Private lastIOStatusLabel As System.Windows.Forms.Label
    Private components As System.ComponentModel.Container = Nothing

    Public Sub New()
        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()
        secondaryAddressComboBox.Items.Add("None")

        Dim i As Integer
        For i = 96 To 126
            secondaryAddressComboBox.Items.Add(i)
        Next
        secondaryAddressComboBox.SelectedIndex = 0
    End Sub 'New

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (GpibDevice Is Nothing) Then
                GpibDevice.Dispose()
            End If
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub 'Dispose
    Friend WithEvents secondaryAddressComboBox As System.Windows.Forms.ComboBox

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.openButton = New System.Windows.Forms.Button
        Me.closeButton = New System.Windows.Forms.Button
        Me.stringToWriteTextBox = New System.Windows.Forms.TextBox
        Me.stringReadTextBox = New System.Windows.Forms.TextBox
        Me.writeButton = New System.Windows.Forms.Button
        Me.stringToWriteLabel = New System.Windows.Forms.Label
        Me.readButton = New System.Windows.Forms.Button
        Me.stringReadLabel = New System.Windows.Forms.Label
        Me.boardIdNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.primaryAddressNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.boardIdLabel = New System.Windows.Forms.Label
        Me.primaryAddressLabel = New System.Windows.Forms.Label
        Me.secondaryAddressLabel = New System.Windows.Forms.Label
        Me.terminateButton = New System.Windows.Forms.Button
        Me.elementsTransferredTextBox = New System.Windows.Forms.TextBox
        Me.lastIOStatusTextBox = New System.Windows.Forms.TextBox
        Me.elementsTransferredLabel = New System.Windows.Forms.Label
        Me.lastIOStatusLabel = New System.Windows.Forms.Label
        Me.secondaryAddressComboBox = New System.Windows.Forms.ComboBox
        CType(Me.boardIdNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.primaryAddressNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'openButton
        '
        Me.openButton.Location = New System.Drawing.Point(8, 96)
        Me.openButton.Name = "openButton"
        Me.openButton.TabIndex = 2
        Me.openButton.Text = "&Open"
        '
        'closeButton
        '
        Me.closeButton.Enabled = False
        Me.closeButton.Location = New System.Drawing.Point(88, 96)
        Me.closeButton.Name = "closeButton"
        Me.closeButton.TabIndex = 3
        Me.closeButton.Text = "&Close"
        '
        'stringToWriteTextBox
        '
        Me.stringToWriteTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.stringToWriteTextBox.Enabled = False
        Me.stringToWriteTextBox.Location = New System.Drawing.Point(8, 152)
        Me.stringToWriteTextBox.Name = "stringToWriteTextBox"
        Me.stringToWriteTextBox.Size = New System.Drawing.Size(288, 20)
        Me.stringToWriteTextBox.TabIndex = 4
        Me.stringToWriteTextBox.Text = "*idn?\n"
        '
        'stringReadTextBox
        '
        Me.stringReadTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.stringReadTextBox.Location = New System.Drawing.Point(8, 240)
        Me.stringReadTextBox.Multiline = True
        Me.stringReadTextBox.Name = "stringReadTextBox"
        Me.stringReadTextBox.ReadOnly = True
        Me.stringReadTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.stringReadTextBox.Size = New System.Drawing.Size(288, 136)
        Me.stringReadTextBox.TabIndex = 6
        Me.stringReadTextBox.Text = ""
        '
        'writeButton
        '
        Me.writeButton.Enabled = False
        Me.writeButton.Location = New System.Drawing.Point(8, 184)
        Me.writeButton.Name = "writeButton"
        Me.writeButton.TabIndex = 7
        Me.writeButton.Text = "&Write"
        '
        'stringToWriteLabel
        '
        Me.stringToWriteLabel.Location = New System.Drawing.Point(8, 128)
        Me.stringToWriteLabel.Name = "stringToWriteLabel"
        Me.stringToWriteLabel.TabIndex = 8
        Me.stringToWriteLabel.Text = "String to Write:"
        '
        'readButton
        '
        Me.readButton.Enabled = False
        Me.readButton.Location = New System.Drawing.Point(88, 184)
        Me.readButton.Name = "readButton"
        Me.readButton.TabIndex = 9
        Me.readButton.Text = "&Read"
        '
        'stringReadLabel
        '
        Me.stringReadLabel.Location = New System.Drawing.Point(8, 216)
        Me.stringReadLabel.Name = "stringReadLabel"
        Me.stringReadLabel.TabIndex = 10
        Me.stringReadLabel.Text = "String Read:"
        '
        'boardIdNumericUpDown
        '
        Me.boardIdNumericUpDown.Location = New System.Drawing.Point(128, 16)
        Me.boardIdNumericUpDown.Name = "boardIdNumericUpDown"
        Me.boardIdNumericUpDown.Size = New System.Drawing.Size(60, 20)
        Me.boardIdNumericUpDown.TabIndex = 11
        '
        'primaryAddressNumericUpDown
        '
        Me.primaryAddressNumericUpDown.Location = New System.Drawing.Point(128, 40)
        Me.primaryAddressNumericUpDown.Name = "primaryAddressNumericUpDown"
        Me.primaryAddressNumericUpDown.Size = New System.Drawing.Size(60, 20)
        Me.primaryAddressNumericUpDown.TabIndex = 12
        Me.primaryAddressNumericUpDown.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'boardIdLabel
        '
        Me.boardIdLabel.Location = New System.Drawing.Point(8, 16)
        Me.boardIdLabel.Name = "boardIdLabel"
        Me.boardIdLabel.Size = New System.Drawing.Size(72, 16)
        Me.boardIdLabel.TabIndex = 14
        Me.boardIdLabel.Text = "Board ID:"
        '
        'primaryAddressLabel
        '
        Me.primaryAddressLabel.Location = New System.Drawing.Point(8, 40)
        Me.primaryAddressLabel.Name = "primaryAddressLabel"
        Me.primaryAddressLabel.Size = New System.Drawing.Size(100, 16)
        Me.primaryAddressLabel.TabIndex = 15
        Me.primaryAddressLabel.Text = "Primary Address:"
        '
        'secondaryAddressLabel
        '
        Me.secondaryAddressLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.secondaryAddressLabel.Location = New System.Drawing.Point(8, 64)
        Me.secondaryAddressLabel.Name = "secondaryAddressLabel"
        Me.secondaryAddressLabel.Size = New System.Drawing.Size(112, 16)
        Me.secondaryAddressLabel.TabIndex = 16
        Me.secondaryAddressLabel.Text = "Secondary Address:"
        '
        'terminateButton
        '
        Me.terminateButton.Enabled = False
        Me.terminateButton.Location = New System.Drawing.Point(168, 184)
        Me.terminateButton.Name = "terminateButton"
        Me.terminateButton.TabIndex = 17
        Me.terminateButton.Text = "&Terminate"
        '
        'elementsTransferredTextBox
        '
        Me.elementsTransferredTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.elementsTransferredTextBox.Location = New System.Drawing.Point(8, 408)
        Me.elementsTransferredTextBox.Name = "elementsTransferredTextBox"
        Me.elementsTransferredTextBox.ReadOnly = True
        Me.elementsTransferredTextBox.Size = New System.Drawing.Size(120, 20)
        Me.elementsTransferredTextBox.TabIndex = 18
        Me.elementsTransferredTextBox.Text = ""
        '
        'lastIOStatusTextBox
        '
        Me.lastIOStatusTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lastIOStatusTextBox.Location = New System.Drawing.Point(136, 408)
        Me.lastIOStatusTextBox.Name = "lastIOStatusTextBox"
        Me.lastIOStatusTextBox.ReadOnly = True
        Me.lastIOStatusTextBox.Size = New System.Drawing.Size(160, 20)
        Me.lastIOStatusTextBox.TabIndex = 19
        Me.lastIOStatusTextBox.Text = ""
        '
        'elementsTransferredLabel
        '
        Me.elementsTransferredLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.elementsTransferredLabel.Location = New System.Drawing.Point(8, 384)
        Me.elementsTransferredLabel.Name = "elementsTransferredLabel"
        Me.elementsTransferredLabel.Size = New System.Drawing.Size(120, 23)
        Me.elementsTransferredLabel.TabIndex = 20
        Me.elementsTransferredLabel.Text = "Elements Transferred:"
        '
        'lastIOStatusLabel
        '
        Me.lastIOStatusLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lastIOStatusLabel.Location = New System.Drawing.Point(136, 384)
        Me.lastIOStatusLabel.Name = "lastIOStatusLabel"
        Me.lastIOStatusLabel.TabIndex = 21
        Me.lastIOStatusLabel.Text = "Last I/O Status:"
        '
        'secondaryAddressComboBox
        '
        Me.secondaryAddressComboBox.Location = New System.Drawing.Point(128, 64)
        Me.secondaryAddressComboBox.Name = "secondaryAddressComboBox"
        Me.secondaryAddressComboBox.Size = New System.Drawing.Size(60, 21)
        Me.secondaryAddressComboBox.TabIndex = 22
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(304, 437)
        Me.Controls.Add(Me.secondaryAddressComboBox)
        Me.Controls.Add(Me.lastIOStatusLabel)
        Me.Controls.Add(Me.elementsTransferredLabel)
        Me.Controls.Add(Me.lastIOStatusTextBox)
        Me.Controls.Add(Me.elementsTransferredTextBox)
        Me.Controls.Add(Me.terminateButton)
        Me.Controls.Add(Me.secondaryAddressLabel)
        Me.Controls.Add(Me.primaryAddressLabel)
        Me.Controls.Add(Me.boardIdLabel)
        Me.Controls.Add(Me.primaryAddressNumericUpDown)
        Me.Controls.Add(Me.boardIdNumericUpDown)
        Me.Controls.Add(Me.stringReadLabel)
        Me.Controls.Add(Me.readButton)
        Me.Controls.Add(Me.stringToWriteLabel)
        Me.Controls.Add(Me.writeButton)
        Me.Controls.Add(Me.stringReadTextBox)
        Me.Controls.Add(Me.stringToWriteTextBox)
        Me.Controls.Add(Me.closeButton)
        Me.Controls.Add(Me.openButton)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(256, 352)
        Me.Name = "MainForm"
        Me.Text = "NI-488.2 Simple Async Read/Write"
        CType(Me.boardIdNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.primaryAddressNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub 'InitializeComponent

#End Region

    <STAThread()> _
    Shared Sub Main()
        Application.Run(New MainForm)
    End Sub 'Main

    Private Sub SetupControlState(ByVal isSessionOpen As Boolean)
        boardIdNumericUpDown.Enabled = Not isSessionOpen
        primaryAddressNumericUpDown.Enabled = Not isSessionOpen
        secondaryAddressComboBox.Enabled = Not isSessionOpen
        openButton.Enabled = Not isSessionOpen
        closeButton.Enabled = isSessionOpen
        stringToWriteTextBox.Enabled = isSessionOpen
        writeButton.Enabled = isSessionOpen
        readButton.Enabled = isSessionOpen
        terminateButton.Enabled = isSessionOpen
        stringReadTextBox.Enabled = isSessionOpen
    End Sub 'SetupControlState

    Private Sub openButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles openButton.Click
        Try
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            Dim currentSecondaryAddress As Integer

            If secondaryAddressComboBox.SelectedIndex <> 0 Then
                currentSecondaryAddress = secondaryAddressComboBox.SelectedItem
            Else
                currentSecondaryAddress = 0
            End If

            GpibDevice = New Device(CInt(boardIdNumericUpDown.Value), CByte(primaryAddressNumericUpDown.Value), CByte(currentSecondaryAddress))

#If NETFX2_0 Then
            'For .NET Framework 2.0, use SynchronizeCallbacks to specify that the object 
            'marshals callbacks across threads appropriately.
            GpibDevice.SynchronizeCallbacks = True
#Else
                'For .NET Framework 1.1, set SynchronizingObject to the Windows Form to specify 
                'that the object marshals callbacks across threads appropriately.
                GpibDevice.SynchronizingObject = Me
#End If


            SetupControlState(True)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Windows.Forms.Cursor.Current = Cursors.Default
        End Try
    End Sub 'openButton_Click

    Private Sub closeButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles closeButton.Click
        Try
            GpibDevice.Dispose()
            SetupControlState(False)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub 'closeButton_Click

    Private Sub writeButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles writeButton.Click
        Try
            GpibDevice.BeginWrite(ReplaceCommonEscapeSequences(stringToWriteTextBox.Text), New AsyncCallback(AddressOf OnWriteComplete), Nothing)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub 'writeButton_Click

    Private Sub OnWriteComplete(ByVal result As IAsyncResult)
        Try
            GpibDevice.EndWrite(result)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        elementsTransferredTextBox.Text = GpibDevice.LastCount.ToString()
        lastIOStatusTextBox.Text = GpibDevice.LastStatus.ToString()
    End Sub 'OnWriteComplete

    Private Sub readButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles readButton.Click
        Try
            GpibDevice.BeginRead(New AsyncCallback(AddressOf OnReadComplete), Nothing)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub 'readButton_Click

    Private Sub OnReadComplete(ByVal result As IAsyncResult)
        Try
            stringReadTextBox.Text = InsertCommonEscapeSequences(GpibDevice.EndReadString(result))
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        elementsTransferredTextBox.Text = GpibDevice.LastCount.ToString()
        lastIOStatusTextBox.Text = GpibDevice.LastStatus.ToString()
    End Sub 'OnReadComplete

    Private Function ReplaceCommonEscapeSequences(ByVal s As String) As String
        Return s.Replace("\n", ControlChars.Lf).Replace("\r", ControlChars.Cr)
    End Function 'ReplaceCommonEscapeSequences

    Private Function InsertCommonEscapeSequences(ByVal s As String) As String
        Return s.Replace(ControlChars.Lf, "\n").Replace(ControlChars.Cr, "\r")
    End Function 'InsertCommonEscapeSequences

    Private Sub terminateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles terminateButton.Click
        Try
            GpibDevice.AbortAsynchronousIO()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub 'terminateButton_Click
End Class 'MainForm