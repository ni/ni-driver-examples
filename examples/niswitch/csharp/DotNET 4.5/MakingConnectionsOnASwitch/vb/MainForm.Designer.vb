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
        Me.topologyNameLabel = New System.Windows.Forms.Label()
        Me.topologyNameComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.firstConnectionGroupBox = New System.Windows.Forms.GroupBox()
        Me.channel2TextBox = New System.Windows.Forms.TextBox()
        Me.channel1TextBox = New System.Windows.Forms.TextBox()
        Me.channel2Label = New System.Windows.Forms.Label()
        Me.channel1Label = New System.Windows.Forms.Label()
        Me.secConnectionGroupBox = New System.Windows.Forms.GroupBox()
        Me.channel4TextBox = New System.Windows.Forms.TextBox()
        Me.channel3TextBox = New System.Windows.Forms.TextBox()
        Me.channel4Label = New System.Windows.Forms.Label()
        Me.channel3Label = New System.Windows.Forms.Label()
        Me.connectButton = New System.Windows.Forms.Button()
        Me.firstConnectionGroupBox.SuspendLayout()
        Me.secConnectionGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'topologyNameLabel
        '
        Me.topologyNameLabel.AutoSize = True
        Me.topologyNameLabel.Location = New System.Drawing.Point(158, 20)
        Me.topologyNameLabel.Name = "topologyNameLabel"
        Me.topologyNameLabel.Size = New System.Drawing.Size(82, 13)
        Me.topologyNameLabel.TabIndex = 6
        Me.topologyNameLabel.Text = "Topology Name"
        '
        'topologyNameComboBox
        '
        Me.topologyNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.topologyNameComboBox.FormattingEnabled = True
        Me.topologyNameComboBox.Location = New System.Drawing.Point(161, 36)
        Me.topologyNameComboBox.Name = "topologyNameComboBox"
        Me.topologyNameComboBox.Size = New System.Drawing.Size(143, 21)
        Me.topologyNameComboBox.TabIndex = 1
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(12, 20)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 5
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 36)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(143, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'firstConnectionGroupBox
        '
        Me.firstConnectionGroupBox.Controls.Add(Me.channel2TextBox)
        Me.firstConnectionGroupBox.Controls.Add(Me.channel1TextBox)
        Me.firstConnectionGroupBox.Controls.Add(Me.channel2Label)
        Me.firstConnectionGroupBox.Controls.Add(Me.channel1Label)
        Me.firstConnectionGroupBox.Location = New System.Drawing.Point(12, 82)
        Me.firstConnectionGroupBox.Name = "firstConnectionGroupBox"
        Me.firstConnectionGroupBox.Size = New System.Drawing.Size(143, 100)
        Me.firstConnectionGroupBox.TabIndex = 2
        Me.firstConnectionGroupBox.TabStop = False
        Me.firstConnectionGroupBox.Text = "First Connection"
        '
        'channel2TextBox
        '
        Me.channel2TextBox.Location = New System.Drawing.Point(9, 74)
        Me.channel2TextBox.Name = "channel2TextBox"
        Me.channel2TextBox.Size = New System.Drawing.Size(100, 20)
        Me.channel2TextBox.TabIndex = 1
        '
        'channel1TextBox
        '
        Me.channel1TextBox.Location = New System.Drawing.Point(9, 32)
        Me.channel1TextBox.Name = "channel1TextBox"
        Me.channel1TextBox.Size = New System.Drawing.Size(100, 20)
        Me.channel1TextBox.TabIndex = 0
        '
        'channel2Label
        '
        Me.channel2Label.AutoSize = True
        Me.channel2Label.Location = New System.Drawing.Point(6, 55)
        Me.channel2Label.Name = "channel2Label"
        Me.channel2Label.Size = New System.Drawing.Size(55, 13)
        Me.channel2Label.TabIndex = 3
        Me.channel2Label.Text = "Channel 2"
        '
        'channel1Label
        '
        Me.channel1Label.AutoSize = True
        Me.channel1Label.Location = New System.Drawing.Point(6, 16)
        Me.channel1Label.Name = "channel1Label"
        Me.channel1Label.Size = New System.Drawing.Size(55, 13)
        Me.channel1Label.TabIndex = 2
        Me.channel1Label.Text = "Channel 1"
        '
        'secConnectionGroupBox
        '
        Me.secConnectionGroupBox.Controls.Add(Me.channel4TextBox)
        Me.secConnectionGroupBox.Controls.Add(Me.channel3TextBox)
        Me.secConnectionGroupBox.Controls.Add(Me.channel4Label)
        Me.secConnectionGroupBox.Controls.Add(Me.channel3Label)
        Me.secConnectionGroupBox.Location = New System.Drawing.Point(161, 82)
        Me.secConnectionGroupBox.Name = "secConnectionGroupBox"
        Me.secConnectionGroupBox.Size = New System.Drawing.Size(143, 100)
        Me.secConnectionGroupBox.TabIndex = 3
        Me.secConnectionGroupBox.TabStop = False
        Me.secConnectionGroupBox.Text = "Second Connection"
        '
        'channel4TextBox
        '
        Me.channel4TextBox.Location = New System.Drawing.Point(9, 71)
        Me.channel4TextBox.Name = "channel4TextBox"
        Me.channel4TextBox.Size = New System.Drawing.Size(100, 20)
        Me.channel4TextBox.TabIndex = 1
        '
        'channel3TextBox
        '
        Me.channel3TextBox.Location = New System.Drawing.Point(9, 32)
        Me.channel3TextBox.Name = "channel3TextBox"
        Me.channel3TextBox.Size = New System.Drawing.Size(100, 20)
        Me.channel3TextBox.TabIndex = 0
        '
        'channel4Label
        '
        Me.channel4Label.AutoSize = True
        Me.channel4Label.Location = New System.Drawing.Point(6, 55)
        Me.channel4Label.Name = "channel4Label"
        Me.channel4Label.Size = New System.Drawing.Size(55, 13)
        Me.channel4Label.TabIndex = 3
        Me.channel4Label.Text = "Channel 4"
        '
        'channel3Label
        '
        Me.channel3Label.AutoSize = True
        Me.channel3Label.Location = New System.Drawing.Point(6, 16)
        Me.channel3Label.Name = "channel3Label"
        Me.channel3Label.Size = New System.Drawing.Size(55, 13)
        Me.channel3Label.TabIndex = 2
        Me.channel3Label.Text = "Channel 3"
        '
        'connectButton
        '
        Me.connectButton.Location = New System.Drawing.Point(124, 198)
        Me.connectButton.Name = "connectButton"
        Me.connectButton.Size = New System.Drawing.Size(75, 23)
        Me.connectButton.TabIndex = 4
        Me.connectButton.Text = "Connect"
        Me.connectButton.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AcceptButton = Me.connectButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(326, 233)
        Me.Controls.Add(Me.connectButton)
        Me.Controls.Add(Me.secConnectionGroupBox)
        Me.Controls.Add(Me.firstConnectionGroupBox)
        Me.Controls.Add(Me.topologyNameLabel)
        Me.Controls.Add(Me.topologyNameComboBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Making Connections On A Switch"
        Me.firstConnectionGroupBox.ResumeLayout(False)
        Me.firstConnectionGroupBox.PerformLayout()
        Me.secConnectionGroupBox.ResumeLayout(False)
        Me.secConnectionGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private topologyNameLabel As System.Windows.Forms.Label
    Private topologyNameComboBox As System.Windows.Forms.ComboBox
    Private resourceNameLabel As System.Windows.Forms.Label
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private firstConnectionGroupBox As System.Windows.Forms.GroupBox
    Private secConnectionGroupBox As System.Windows.Forms.GroupBox
    Private channel2Label As System.Windows.Forms.Label
    Private channel1Label As System.Windows.Forms.Label
    Private channel4Label As System.Windows.Forms.Label
    Private channel3Label As System.Windows.Forms.Label
    Private channel2TextBox As System.Windows.Forms.TextBox
    Private channel1TextBox As System.Windows.Forms.TextBox
    Private channel4TextBox As System.Windows.Forms.TextBox
    Private channel3TextBox As System.Windows.Forms.TextBox
    Private WithEvents connectButton As System.Windows.Forms.Button
End Class


