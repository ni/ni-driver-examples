Public Class MainForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        sadComboBox.Items.Add("None")
        Dim i As Integer
        For i = 96 To 126
            sadComboBox.Items.Add(i)
        Next
        sadComboBox.SelectedIndex = 0

        If reenableMaskComboBox.Items.Count > 0 Then
            reenableMaskComboBox.SelectedIndex = 0
        End If

        SetupControlState(False)

        notifyUpdateStatusHandler = New NotifyUpdateStatusDelegate(AddressOf NotifyUpdateStatus)
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (testDevice Is Nothing) Then
                testDevice.Dispose()
            End If
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents notifyControlGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents notifyOnDSRButton As System.Windows.Forms.Button
    Friend WithEvents reenableMaskComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents reenableMaskLabel As System.Windows.Forms.Label
    Friend WithEvents communicationGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents clearOutputButton As System.Windows.Forms.Button
    Friend WithEvents stringReadLabel As System.Windows.Forms.Label
    Friend WithEvents stringReadTextBox As System.Windows.Forms.TextBox
    Friend WithEvents stringToWriteLabel As System.Windows.Forms.Label
    Friend WithEvents writeButton As System.Windows.Forms.Button
    Friend WithEvents stringToWriteTextBox As System.Windows.Forms.TextBox
    Friend WithEvents notifyDataGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents notifyCountLabel As System.Windows.Forms.Label
    Friend WithEvents notifyStatusTextBox As System.Windows.Forms.TextBox
    Friend WithEvents notifyErrorTextBox As System.Windows.Forms.TextBox
    Friend WithEvents notifyStatusLabel As System.Windows.Forms.Label
    Friend WithEvents notifyCountTextBox As System.Windows.Forms.TextBox
    Friend WithEvents deviceGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents closeButton As System.Windows.Forms.Button
    Friend WithEvents openButton As System.Windows.Forms.Button
    Friend WithEvents padNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents boardIDNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents sadLabel As System.Windows.Forms.Label
    Friend WithEvents padLabel As System.Windows.Forms.Label
    Friend WithEvents boardIDLabel As System.Windows.Forms.Label
    Friend WithEvents sadComboBox As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
        Me.notifyControlGroupBox = New System.Windows.Forms.GroupBox
        Me.notifyOnDSRButton = New System.Windows.Forms.Button
        Me.reenableMaskComboBox = New System.Windows.Forms.ComboBox
        Me.reenableMaskLabel = New System.Windows.Forms.Label
        Me.communicationGroupBox = New System.Windows.Forms.GroupBox
        Me.clearOutputButton = New System.Windows.Forms.Button
        Me.stringReadLabel = New System.Windows.Forms.Label
        Me.stringReadTextBox = New System.Windows.Forms.TextBox
        Me.stringToWriteLabel = New System.Windows.Forms.Label
        Me.writeButton = New System.Windows.Forms.Button
        Me.stringToWriteTextBox = New System.Windows.Forms.TextBox
        Me.notifyDataGroupBox = New System.Windows.Forms.GroupBox
        Me.notifyCountLabel = New System.Windows.Forms.Label
        Me.notifyStatusTextBox = New System.Windows.Forms.TextBox
        Me.notifyStatusLabel = New System.Windows.Forms.Label
        Me.notifyCountTextBox = New System.Windows.Forms.TextBox
        Me.deviceGroupBox = New System.Windows.Forms.GroupBox
        Me.closeButton = New System.Windows.Forms.Button
        Me.openButton = New System.Windows.Forms.Button
        Me.padNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.boardIDNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.sadLabel = New System.Windows.Forms.Label
        Me.padLabel = New System.Windows.Forms.Label
        Me.boardIDLabel = New System.Windows.Forms.Label
        Me.sadComboBox = New System.Windows.Forms.ComboBox
        Me.notifyControlGroupBox.SuspendLayout()
        Me.communicationGroupBox.SuspendLayout()
        Me.notifyDataGroupBox.SuspendLayout()
        Me.deviceGroupBox.SuspendLayout()
        CType(Me.padNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.boardIDNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'notifyControlGroupBox
        '
        Me.notifyControlGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.notifyControlGroupBox.Controls.Add(Me.notifyOnDSRButton)
        Me.notifyControlGroupBox.Controls.Add(Me.reenableMaskComboBox)
        Me.notifyControlGroupBox.Controls.Add(Me.reenableMaskLabel)
        Me.notifyControlGroupBox.Location = New System.Drawing.Point(210, 8)
        Me.notifyControlGroupBox.Name = "notifyControlGroupBox"
        Me.notifyControlGroupBox.Size = New System.Drawing.Size(286, 144)
        Me.notifyControlGroupBox.TabIndex = 30
        Me.notifyControlGroupBox.TabStop = False
        Me.notifyControlGroupBox.Text = "Notify Control"
        '
        'notifyOnDSRButton
        '
        Me.notifyOnDSRButton.Location = New System.Drawing.Point(16, 24)
        Me.notifyOnDSRButton.Name = "notifyOnDSRButton"
        Me.notifyOnDSRButton.Size = New System.Drawing.Size(184, 24)
        Me.notifyOnDSRButton.TabIndex = 13
        Me.notifyOnDSRButton.Text = "Notify on DeviceServiceRequest"
        '
        'reenableMaskComboBox
        '
        Me.reenableMaskComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.reenableMaskComboBox.Items.AddRange(New Object() {"DeviceServiceRequest", "None"})
        Me.reenableMaskComboBox.Location = New System.Drawing.Point(16, 72)
        Me.reenableMaskComboBox.Name = "reenableMaskComboBox"
        Me.reenableMaskComboBox.Size = New System.Drawing.Size(184, 21)
        Me.reenableMaskComboBox.TabIndex = 14
        '
        'reenableMaskLabel
        '
        Me.reenableMaskLabel.Location = New System.Drawing.Point(16, 56)
        Me.reenableMaskLabel.Name = "reenableMaskLabel"
        Me.reenableMaskLabel.Size = New System.Drawing.Size(136, 16)
        Me.reenableMaskLabel.TabIndex = 15
        Me.reenableMaskLabel.Text = "Reenable Notify Mask:"
        '
        'communicationGroupBox
        '
        Me.communicationGroupBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.communicationGroupBox.Controls.Add(Me.clearOutputButton)
        Me.communicationGroupBox.Controls.Add(Me.stringReadLabel)
        Me.communicationGroupBox.Controls.Add(Me.stringReadTextBox)
        Me.communicationGroupBox.Controls.Add(Me.stringToWriteLabel)
        Me.communicationGroupBox.Controls.Add(Me.writeButton)
        Me.communicationGroupBox.Controls.Add(Me.stringToWriteTextBox)
        Me.communicationGroupBox.Location = New System.Drawing.Point(8, 160)
        Me.communicationGroupBox.Name = "communicationGroupBox"
        Me.communicationGroupBox.Size = New System.Drawing.Size(328, 216)
        Me.communicationGroupBox.TabIndex = 29
        Me.communicationGroupBox.TabStop = False
        Me.communicationGroupBox.Text = "Communication"
        '
        'clearOutputButton
        '
        Me.clearOutputButton.Location = New System.Drawing.Point(96, 64)
        Me.clearOutputButton.Name = "clearOutputButton"
        Me.clearOutputButton.Size = New System.Drawing.Size(80, 24)
        Me.clearOutputButton.TabIndex = 13
        Me.clearOutputButton.Text = "Clear Output"
        '
        'stringReadLabel
        '
        Me.stringReadLabel.Location = New System.Drawing.Point(16, 104)
        Me.stringReadLabel.Name = "stringReadLabel"
        Me.stringReadLabel.Size = New System.Drawing.Size(216, 16)
        Me.stringReadLabel.TabIndex = 12
        Me.stringReadLabel.Text = "String Read After DeviceServiceRequest:"
        '
        'stringReadTextBox
        '
        Me.stringReadTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.stringReadTextBox.Location = New System.Drawing.Point(16, 120)
        Me.stringReadTextBox.Multiline = True
        Me.stringReadTextBox.Name = "stringReadTextBox"
        Me.stringReadTextBox.ReadOnly = True
        Me.stringReadTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.stringReadTextBox.Size = New System.Drawing.Size(296, 80)
        Me.stringReadTextBox.TabIndex = 11
        Me.stringReadTextBox.Text = ""
        '
        'stringToWriteLabel
        '
        Me.stringToWriteLabel.Location = New System.Drawing.Point(16, 24)
        Me.stringToWriteLabel.Name = "stringToWriteLabel"
        Me.stringToWriteLabel.Size = New System.Drawing.Size(88, 16)
        Me.stringToWriteLabel.TabIndex = 9
        Me.stringToWriteLabel.Text = "String To Write:"
        '
        'writeButton
        '
        Me.writeButton.Location = New System.Drawing.Point(16, 64)
        Me.writeButton.Name = "writeButton"
        Me.writeButton.Size = New System.Drawing.Size(72, 24)
        Me.writeButton.TabIndex = 10
        Me.writeButton.Text = "Write"
        '
        'stringToWriteTextBox
        '
        Me.stringToWriteTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.stringToWriteTextBox.Location = New System.Drawing.Point(16, 40)
        Me.stringToWriteTextBox.Name = "stringToWriteTextBox"
        Me.stringToWriteTextBox.Size = New System.Drawing.Size(296, 20)
        Me.stringToWriteTextBox.TabIndex = 8
        Me.stringToWriteTextBox.Text = "*IDN?\n"
        '
        'notifyDataGroupBox
        '
        Me.notifyDataGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.notifyDataGroupBox.Controls.Add(Me.notifyCountLabel)
        Me.notifyDataGroupBox.Controls.Add(Me.notifyStatusTextBox)
        Me.notifyDataGroupBox.Controls.Add(Me.notifyStatusLabel)
        Me.notifyDataGroupBox.Controls.Add(Me.notifyCountTextBox)
        Me.notifyDataGroupBox.Location = New System.Drawing.Point(344, 160)
        Me.notifyDataGroupBox.Name = "notifyDataGroupBox"
        Me.notifyDataGroupBox.Size = New System.Drawing.Size(152, 136)
        Me.notifyDataGroupBox.TabIndex = 28
        Me.notifyDataGroupBox.TabStop = False
        Me.notifyDataGroupBox.Text = "Notify Data"
        '
        'notifyCountLabel
        '
        Me.notifyCountLabel.Location = New System.Drawing.Point(16, 72)
        Me.notifyCountLabel.Name = "notifyCountLabel"
        Me.notifyCountLabel.Size = New System.Drawing.Size(72, 16)
        Me.notifyCountLabel.TabIndex = 22
        Me.notifyCountLabel.Text = "Count:"
        '
        'notifyStatusTextBox
        '
        Me.notifyStatusTextBox.Location = New System.Drawing.Point(16, 40)
        Me.notifyStatusTextBox.Name = "notifyStatusTextBox"
        Me.notifyStatusTextBox.ReadOnly = True
        Me.notifyStatusTextBox.Size = New System.Drawing.Size(120, 20)
        Me.notifyStatusTextBox.TabIndex = 16
        Me.notifyStatusTextBox.Text = ""
        '
        'notifyStatusLabel
        '
        Me.notifyStatusLabel.Location = New System.Drawing.Point(16, 24)
        Me.notifyStatusLabel.Name = "notifyStatusLabel"
        Me.notifyStatusLabel.Size = New System.Drawing.Size(80, 16)
        Me.notifyStatusLabel.TabIndex = 20
        Me.notifyStatusLabel.Text = "Status:"
        '
        'notifyCountTextBox
        '
        Me.notifyCountTextBox.Location = New System.Drawing.Point(16, 88)
        Me.notifyCountTextBox.Name = "notifyCountTextBox"
        Me.notifyCountTextBox.ReadOnly = True
        Me.notifyCountTextBox.Size = New System.Drawing.Size(120, 20)
        Me.notifyCountTextBox.TabIndex = 18
        Me.notifyCountTextBox.Text = ""
        '
        'deviceGroupBox
        '
        Me.deviceGroupBox.Controls.Add(Me.sadComboBox)
        Me.deviceGroupBox.Controls.Add(Me.closeButton)
        Me.deviceGroupBox.Controls.Add(Me.openButton)
        Me.deviceGroupBox.Controls.Add(Me.padNumericUpDown)
        Me.deviceGroupBox.Controls.Add(Me.boardIDNumericUpDown)
        Me.deviceGroupBox.Controls.Add(Me.sadLabel)
        Me.deviceGroupBox.Controls.Add(Me.padLabel)
        Me.deviceGroupBox.Controls.Add(Me.boardIDLabel)
        Me.deviceGroupBox.Location = New System.Drawing.Point(8, 8)
        Me.deviceGroupBox.Name = "deviceGroupBox"
        Me.deviceGroupBox.Size = New System.Drawing.Size(194, 144)
        Me.deviceGroupBox.TabIndex = 27
        Me.deviceGroupBox.TabStop = False
        Me.deviceGroupBox.Text = "Device"
        '
        'closeButton
        '
        Me.closeButton.Location = New System.Drawing.Point(96, 104)
        Me.closeButton.Name = "closeButton"
        Me.closeButton.Size = New System.Drawing.Size(72, 24)
        Me.closeButton.TabIndex = 15
        Me.closeButton.Text = "Close"
        '
        'openButton
        '
        Me.openButton.Location = New System.Drawing.Point(16, 104)
        Me.openButton.Name = "openButton"
        Me.openButton.Size = New System.Drawing.Size(72, 24)
        Me.openButton.TabIndex = 14
        Me.openButton.Text = "Open"
        '
        'padNumericUpDown
        '
        Me.padNumericUpDown.Location = New System.Drawing.Point(128, 48)
        Me.padNumericUpDown.Maximum = New Decimal(New Integer() {31, 0, 0, 0})
        Me.padNumericUpDown.Name = "padNumericUpDown"
        Me.padNumericUpDown.Size = New System.Drawing.Size(60, 20)
        Me.padNumericUpDown.TabIndex = 12
        Me.padNumericUpDown.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'boardIDNumericUpDown
        '
        Me.boardIDNumericUpDown.Location = New System.Drawing.Point(128, 24)
        Me.boardIDNumericUpDown.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.boardIDNumericUpDown.Name = "boardIDNumericUpDown"
        Me.boardIDNumericUpDown.Size = New System.Drawing.Size(60, 20)
        Me.boardIDNumericUpDown.TabIndex = 11
        '
        'sadLabel
        '
        Me.sadLabel.Location = New System.Drawing.Point(16, 72)
        Me.sadLabel.Name = "sadLabel"
        Me.sadLabel.Size = New System.Drawing.Size(110, 16)
        Me.sadLabel.TabIndex = 10
        Me.sadLabel.Text = "Secondary Address:"
        '
        'padLabel
        '
        Me.padLabel.Location = New System.Drawing.Point(16, 48)
        Me.padLabel.Name = "padLabel"
        Me.padLabel.Size = New System.Drawing.Size(112, 16)
        Me.padLabel.TabIndex = 9
        Me.padLabel.Text = "Primary Address:"
        '
        'boardIDLabel
        '
        Me.boardIDLabel.Location = New System.Drawing.Point(16, 24)
        Me.boardIDLabel.Name = "boardIDLabel"
        Me.boardIDLabel.Size = New System.Drawing.Size(112, 16)
        Me.boardIDLabel.TabIndex = 8
        Me.boardIDLabel.Text = "Board ID:"
        '
        'sadComboBox
        '
        Me.sadComboBox.Location = New System.Drawing.Point(128, 72)
        Me.sadComboBox.Name = "sadComboBox"
        Me.sadComboBox.Size = New System.Drawing.Size(60, 21)
        Me.sadComboBox.TabIndex = 16
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(504, 397)
        Me.Controls.Add(Me.notifyControlGroupBox)
        Me.Controls.Add(Me.communicationGroupBox)
        Me.Controls.Add(Me.notifyDataGroupBox)
        Me.Controls.Add(Me.deviceGroupBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(512, 424)
        Me.Name = "MainForm"
        Me.Text = "Using Notify"
        Me.notifyControlGroupBox.ResumeLayout(False)
        Me.communicationGroupBox.ResumeLayout(False)
        Me.notifyDataGroupBox.ResumeLayout(False)
        Me.deviceGroupBox.ResumeLayout(False)
        CType(Me.padNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.boardIDNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private testDevice As Device
    Public Delegate Function NotifyUpdateStatusDelegate(ByVal readText As String, ByVal status As String, ByVal count As String) As String
    Dim notifyUpdateStatusHandler As NotifyUpdateStatusDelegate

    Private Function ReplaceCommonEscapeSequences(ByVal s As String) As String
        Return s.Replace("\n", ControlChars.Lf).Replace("\r", ControlChars.Cr)
    End Function


    Private Function InsertCommonEscapeSequences(ByVal s As String) As String
        Return s.Replace(ControlChars.Lf, "\n").Replace(ControlChars.Cr, "\r")
    End Function


    Private Sub SetupControlState(ByVal deviceOpen As Boolean)
        boardIDNumericUpDown.Enabled = Not deviceOpen
        padNumericUpDown.Enabled = Not deviceOpen
        sadComboBox.Enabled = Not deviceOpen
        openButton.Enabled = Not deviceOpen
        closeButton.Enabled = deviceOpen
        notifyControlGroupBox.Enabled = deviceOpen
        communicationGroupBox.Enabled = deviceOpen
        notifyDataGroupBox.Enabled = deviceOpen
    End Sub

    Private Sub ClearOutput()
        stringReadTextBox.Text = String.Empty
        notifyStatusTextBox.Text = String.Empty        
        notifyCountTextBox.Text = String.Empty
    End Sub 'ClearOutput


    Private Sub openButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles openButton.Click
        Try
            Dim currentSecondaryAddress As Integer
            If sadComboBox.SelectedIndex <> 0 Then
                currentSecondaryAddress = sadComboBox.SelectedItem
            Else
                currentSecondaryAddress = 0
            End If

            testDevice = New Device(CInt(boardIDNumericUpDown.Value), CByte(padNumericUpDown.Value), CByte(currentSecondaryAddress))
            SetupControlState(True)
            ClearOutput()
        Catch exp As Exception
            MessageBox.Show(exp.Message)
        End Try
    End Sub

    Private Sub closeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles closeButton.Click
        Try
            testDevice.Dispose()
            SetupControlState(False)
        Catch exp As Exception
            MessageBox.Show(exp.Message)
        End Try
    End Sub

    Private Sub writeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles writeButton.Click
        Try
            testDevice.Write(ReplaceCommonEscapeSequences(stringToWriteTextBox.Text))
        Catch exp As Exception
            MessageBox.Show(exp.Message)
        End Try
    End Sub

    Private Sub notifyOnDSRButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles notifyOnDSRButton.Click
        Try
            testDevice.Notify(GpibStatusFlags.DeviceServiceRequest, New NotifyCallback(AddressOf testDevice_Notify), "Sample user data")
        Catch exp As Exception
            MessageBox.Show(exp.Message)
        End Try
    End Sub

    Private Sub testDevice_Notify(ByVal sender As Object, ByVal e As NotifyData)
        Try
            testDevice.SerialPoll()
            Dim reenableMask As String = CType(Invoke(notifyUpdateStatusHandler, New Object() {InsertCommonEscapeSequences(testDevice.ReadString), e.Status.ToString, e.Count.ToString}), String)
            e.SetReenableMask(CType([Enum].Parse(GetType(GpibStatusFlags), reenableMask), GpibStatusFlags))
        Catch exp As Exception
            MessageBox.Show(exp.Message)
        End Try
    End Sub

    Private Function NotifyUpdateStatus(ByVal readText As String, ByVal status As String, ByVal count As String) As String
        stringReadTextBox.Text = readText
        notifyStatusTextBox.Text = status
        notifyCountTextBox.Text = count

        Return reenableMaskComboBox.Text
    End Function

    Private Sub clearOutputButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clearOutputButton.Click
        ClearOutput()
    End Sub
End Class
