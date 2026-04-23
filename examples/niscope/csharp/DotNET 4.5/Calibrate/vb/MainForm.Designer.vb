Partial Class MainForm
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso (components IsNot Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Windows Form Designer generated code"

    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.calibrateButton = New System.Windows.Forms.Button()
        Me.optionComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.RichTextBox()
        Me.resourceNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.optoinGroupBox = New System.Windows.Forms.GroupBox()
        Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageGroupBox.SuspendLayout()
        Me.resourceNameGroupBox.SuspendLayout()
        Me.optoinGroupBox.SuspendLayout()
        Me.buttonsGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'calibrateButton
        '
        Me.calibrateButton.Location = New System.Drawing.Point(65, 19)
        Me.calibrateButton.Name = "calibrateButton"
        Me.calibrateButton.Size = New System.Drawing.Size(75, 23)
        Me.calibrateButton.TabIndex = 0
        Me.calibrateButton.Text = "&Calibrate"
        Me.calibrateButton.UseVisualStyleBackColor = True
        '
        'optionComboBox
        '
        Me.optionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.optionComboBox.Location = New System.Drawing.Point(6, 19)
        Me.optionComboBox.Name = "optionComboBox"
        Me.optionComboBox.Size = New System.Drawing.Size(193, 21)
        Me.optionComboBox.TabIndex = 0
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.Location = New System.Drawing.Point(9, 19)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(104, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(12, 132)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(205, 84)
        Me.messageGroupBox.TabIndex = 2
        Me.messageGroupBox.TabStop = False
        Me.messageGroupBox.Text = "Message"
        '
        'messageTextBox
        '
        Me.messageTextBox.Location = New System.Drawing.Point(9, 19)
        Me.messageTextBox.Name = "messageTextBox"
        Me.messageTextBox.ReadOnly = True
        Me.messageTextBox.Size = New System.Drawing.Size(190, 59)
        Me.messageTextBox.TabIndex = 0
        Me.messageTextBox.Text = "Note : Calibration may take several minutes to complete."
        '
        'resourceNameGroupBox
        '
        Me.resourceNameGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceNameGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.resourceNameGroupBox.Name = "resourceNameGroupBox"
        Me.resourceNameGroupBox.Size = New System.Drawing.Size(205, 49)
        Me.resourceNameGroupBox.TabIndex = 0
        Me.resourceNameGroupBox.TabStop = False
        Me.resourceNameGroupBox.Text = "Resource Name"
        '
        'optoinGroupBox
        '
        Me.optoinGroupBox.Controls.Add(Me.optionComboBox)
        Me.optoinGroupBox.Location = New System.Drawing.Point(12, 72)
        Me.optoinGroupBox.Name = "optoinGroupBox"
        Me.optoinGroupBox.Size = New System.Drawing.Size(205, 49)
        Me.optoinGroupBox.TabIndex = 1
        Me.optoinGroupBox.TabStop = False
        Me.optoinGroupBox.Text = "Self Calibration Option"
        '
        'buttonsGroupBox
        '
        Me.buttonsGroupBox.Controls.Add(Me.calibrateButton)
        Me.buttonsGroupBox.Location = New System.Drawing.Point(12, 222)
        Me.buttonsGroupBox.Name = "buttonsGroupBox"
        Me.buttonsGroupBox.Size = New System.Drawing.Size(205, 53)
        Me.buttonsGroupBox.TabIndex = 3
        Me.buttonsGroupBox.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(231, 288)
        Me.Controls.Add(Me.buttonsGroupBox)
        Me.Controls.Add(Me.optoinGroupBox)
        Me.Controls.Add(Me.resourceNameGroupBox)
        Me.Controls.Add(Me.messageGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Calibrate"
        Me.messageGroupBox.ResumeLayout(False)
        Me.resourceNameGroupBox.ResumeLayout(False)
        Me.optoinGroupBox.ResumeLayout(False)
        Me.buttonsGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private WithEvents calibrateButton As System.Windows.Forms.Button
    Private optionComboBox As System.Windows.Forms.ComboBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private messageGroupBox As System.Windows.Forms.GroupBox
    Private messageTextBox As System.Windows.Forms.RichTextBox
    Private resourceNameGroupBox As System.Windows.Forms.GroupBox
    Private optoinGroupBox As System.Windows.Forms.GroupBox
    Private buttonsGroupBox As System.Windows.Forms.GroupBox


End Class
