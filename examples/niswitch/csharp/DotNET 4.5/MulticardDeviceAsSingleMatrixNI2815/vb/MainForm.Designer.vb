
Partial Class MainForm
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.channel2TextBox = New System.Windows.Forms.TextBox()
        Me.channel1TextBox = New System.Windows.Forms.TextBox()
        Me.channel2Label = New System.Windows.Forms.Label()
        Me.channel1Label = New System.Windows.Forms.Label()
        Me.connectButton = New System.Windows.Forms.Button()
        Me.topologyNameLabel = New System.Windows.Forms.Label()
        Me.topologyNameComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.noteTextBox = New System.Windows.Forms.RichTextBox()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(12, 28)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 4
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'channel2TextBox
        '
        Me.channel2TextBox.Location = New System.Drawing.Point(67, 61)
        Me.channel2TextBox.Name = "channel2TextBox"
        Me.channel2TextBox.Size = New System.Drawing.Size(100, 20)
        Me.channel2TextBox.TabIndex = 1
        Me.channel2TextBox.Text = "c0"
        '
        'channel1TextBox
        '
        Me.channel1TextBox.Location = New System.Drawing.Point(67, 23)
        Me.channel1TextBox.Name = "channel1TextBox"
        Me.channel1TextBox.Size = New System.Drawing.Size(100, 20)
        Me.channel1TextBox.TabIndex = 0
        Me.channel1TextBox.Text = "ab0"
        '
        'channel2Label
        '
        Me.channel2Label.AutoSize = True
        Me.channel2Label.Location = New System.Drawing.Point(6, 64)
        Me.channel2Label.Name = "channel2Label"
        Me.channel2Label.Size = New System.Drawing.Size(55, 13)
        Me.channel2Label.TabIndex = 3
        Me.channel2Label.Text = "Channel 2"
        '
        'channel1Label
        '
        Me.channel1Label.AutoSize = True
        Me.channel1Label.Location = New System.Drawing.Point(6, 26)
        Me.channel1Label.Name = "channel1Label"
        Me.channel1Label.Size = New System.Drawing.Size(55, 13)
        Me.channel1Label.TabIndex = 2
        Me.channel1Label.Text = "Channel 1"
        '
        'connectButton
        '
        Me.connectButton.Location = New System.Drawing.Point(94, 227)
        Me.connectButton.Name = "connectButton"
        Me.connectButton.Size = New System.Drawing.Size(75, 23)
        Me.connectButton.TabIndex = 3
        Me.connectButton.Text = "Connect"
        Me.connectButton.UseVisualStyleBackColor = True
        '
        'topologyNameLabel
        '
        Me.topologyNameLabel.AutoSize = True
        Me.topologyNameLabel.Location = New System.Drawing.Point(12, 71)
        Me.topologyNameLabel.Name = "topologyNameLabel"
        Me.topologyNameLabel.Size = New System.Drawing.Size(82, 13)
        Me.topologyNameLabel.TabIndex = 5
        Me.topologyNameLabel.Text = "Topology Name"
        '
        'topologyNameComboBox
        '
        Me.topologyNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.topologyNameComboBox.FormattingEnabled = True
        Me.topologyNameComboBox.Location = New System.Drawing.Point(12, 87)
        Me.topologyNameComboBox.Name = "topologyNameComboBox"
        Me.topologyNameComboBox.Size = New System.Drawing.Size(143, 21)
        Me.topologyNameComboBox.TabIndex = 1
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 44)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(143, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.channel1Label)
        Me.groupBox1.Controls.Add(Me.channel1TextBox)
        Me.groupBox1.Controls.Add(Me.channel2TextBox)
        Me.groupBox1.Controls.Add(Me.channel2Label)
        Me.groupBox1.Location = New System.Drawing.Point(15, 121)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(174, 100)
        Me.groupBox1.TabIndex = 2
        Me.groupBox1.TabStop = False
        '
        'noteTextBox
        '
        Me.noteTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.noteTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.noteTextBox.Location = New System.Drawing.Point(12, 280)
        Me.noteTextBox.Name = "noteTextBox"
        Me.noteTextBox.ReadOnly = True
        Me.noteTextBox.Size = New System.Drawing.Size(197, 15)
        Me.noteTextBox.TabIndex = 8
        Me.noteTextBox.TabStop = False
        Me.noteTextBox.Text = "This example works with NI 2815."
        '
        'MainForm
        '
        Me.AcceptButton = Me.connectButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(245, 329)
        Me.Controls.Add(Me.noteTextBox)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.connectButton)
        Me.Controls.Add(Me.topologyNameLabel)
        Me.Controls.Add(Me.topologyNameComboBox)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Multicard Device As Single Matrix NI2815"
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private resourceNameLabel As System.Windows.Forms.Label
    Private channel2TextBox As System.Windows.Forms.TextBox
    Private channel1TextBox As System.Windows.Forms.TextBox
    Private channel2Label As System.Windows.Forms.Label
    Private channel1Label As System.Windows.Forms.Label
    Private WithEvents connectButton As System.Windows.Forms.Button
    Private topologyNameLabel As System.Windows.Forms.Label
    Private topologyNameComboBox As System.Windows.Forms.ComboBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents noteTextBox As System.Windows.Forms.RichTextBox
End Class

