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
        Me.initializeDCPowerGroupBox = New System.Windows.Forms.GroupBox()
        Me.resetDeviceCheckBox = New System.Windows.Forms.CheckBox()
        Me.idQueryCheckBox = New System.Windows.Forms.CheckBox()
        Me.initializeButton = New System.Windows.Forms.Button()
        Me.logicalNameTextBox = New System.Windows.Forms.TextBox()
        Me.logicalNameLabel = New System.Windows.Forms.Label()
        Me.messageRichTextBox = New System.Windows.Forms.RichTextBox()
        Me.configureDCPowerGroupBox = New System.Windows.Forms.GroupBox()
        Me.configureAndOutputButton = New System.Windows.Forms.Button()
        Me.ovpEnabledCheckBox = New System.Windows.Forms.CheckBox()
        Me.outputEnabledCheckBox = New System.Windows.Forms.CheckBox()
        Me.channelNameComboBox = New System.Windows.Forms.ComboBox()
        Me.currentLimitLabel = New System.Windows.Forms.Label()
        Me.currentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.ovpLimitLabel = New System.Windows.Forms.Label()
        Me.ovpLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.voltageLevelLabel = New System.Windows.Forms.Label()
        Me.voltageLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.currentLimitBehaviorComboBox = New System.Windows.Forms.ComboBox()
        Me.currentLimitBehaviorLabel = New System.Windows.Forms.Label()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.initializeDCPowerGroupBox.SuspendLayout()
        Me.configureDCPowerGroupBox.SuspendLayout()
        CType(Me.currentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ovpLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'initializeDCPowerGroupBox
        '
        Me.initializeDCPowerGroupBox.Controls.Add(Me.resetDeviceCheckBox)
        Me.initializeDCPowerGroupBox.Controls.Add(Me.idQueryCheckBox)
        Me.initializeDCPowerGroupBox.Controls.Add(Me.initializeButton)
        Me.initializeDCPowerGroupBox.Controls.Add(Me.logicalNameTextBox)
        Me.initializeDCPowerGroupBox.Controls.Add(Me.logicalNameLabel)
        Me.initializeDCPowerGroupBox.Controls.Add(Me.messageRichTextBox)
        Me.initializeDCPowerGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.initializeDCPowerGroupBox.Name = "initializeDCPowerGroupBox"
        Me.initializeDCPowerGroupBox.Size = New System.Drawing.Size(454, 146)
        Me.initializeDCPowerGroupBox.TabIndex = 0
        Me.initializeDCPowerGroupBox.TabStop = False
        Me.initializeDCPowerGroupBox.Text = "Initialize DC Power"
        '
        'resetDeviceCheckBox
        '
        Me.resetDeviceCheckBox.AutoSize = True
        Me.resetDeviceCheckBox.Checked = True
        Me.resetDeviceCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.resetDeviceCheckBox.Location = New System.Drawing.Point(357, 24)
        Me.resetDeviceCheckBox.Name = "resetDeviceCheckBox"
        Me.resetDeviceCheckBox.Size = New System.Drawing.Size(91, 17)
        Me.resetDeviceCheckBox.TabIndex = 3
        Me.resetDeviceCheckBox.Text = "Reset Device"
        Me.resetDeviceCheckBox.UseVisualStyleBackColor = True
        '
        'idQueryCheckBox
        '
        Me.idQueryCheckBox.AutoSize = True
        Me.idQueryCheckBox.Checked = True
        Me.idQueryCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.idQueryCheckBox.Location = New System.Drawing.Point(248, 24)
        Me.idQueryCheckBox.Name = "idQueryCheckBox"
        Me.idQueryCheckBox.Size = New System.Drawing.Size(68, 17)
        Me.idQueryCheckBox.TabIndex = 2
        Me.idQueryCheckBox.Text = "ID Query"
        Me.idQueryCheckBox.UseVisualStyleBackColor = True
        '
        'initializeButton
        '
        Me.initializeButton.Location = New System.Drawing.Point(173, 115)
        Me.initializeButton.Name = "initializeButton"
        Me.initializeButton.Size = New System.Drawing.Size(73, 23)
        Me.initializeButton.TabIndex = 5
        Me.initializeButton.Text = "&Initialize"
        Me.initializeButton.UseVisualStyleBackColor = True
        '
        'logicalNameTextBox
        '
        Me.logicalNameTextBox.Location = New System.Drawing.Point(84, 22)
        Me.logicalNameTextBox.Name = "logicalNameTextBox"
        Me.logicalNameTextBox.Size = New System.Drawing.Size(108, 20)
        Me.logicalNameTextBox.TabIndex = 1
        '
        'logicalNameLabel
        '
        Me.logicalNameLabel.AutoSize = True
        Me.logicalNameLabel.Location = New System.Drawing.Point(6, 26)
        Me.logicalNameLabel.Name = "logicalNameLabel"
        Me.logicalNameLabel.Size = New System.Drawing.Size(72, 13)
        Me.logicalNameLabel.TabIndex = 0
        Me.logicalNameLabel.Text = "Logical Name"
        '
        'messageRichTextBox
        '
        Me.messageRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.messageRichTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.messageRichTextBox.Location = New System.Drawing.Point(6, 58)
        Me.messageRichTextBox.Name = "messageRichTextBox"
        Me.messageRichTextBox.ReadOnly = True
        Me.messageRichTextBox.Size = New System.Drawing.Size(442, 41)
        Me.messageRichTextBox.TabIndex = 4
        Me.messageRichTextBox.TabStop = False
        Me.messageRichTextBox.Text = resources.GetString("messageRichTextBox.Text")
        '
        'configureDCPowerGroupBox
        '
        Me.configureDCPowerGroupBox.Controls.Add(Me.configureAndOutputButton)
        Me.configureDCPowerGroupBox.Controls.Add(Me.ovpEnabledCheckBox)
        Me.configureDCPowerGroupBox.Controls.Add(Me.outputEnabledCheckBox)
        Me.configureDCPowerGroupBox.Controls.Add(Me.channelNameComboBox)
        Me.configureDCPowerGroupBox.Controls.Add(Me.currentLimitLabel)
        Me.configureDCPowerGroupBox.Controls.Add(Me.currentLimitNumeric)
        Me.configureDCPowerGroupBox.Controls.Add(Me.ovpLimitLabel)
        Me.configureDCPowerGroupBox.Controls.Add(Me.ovpLimitNumeric)
        Me.configureDCPowerGroupBox.Controls.Add(Me.voltageLevelLabel)
        Me.configureDCPowerGroupBox.Controls.Add(Me.voltageLevelNumeric)
        Me.configureDCPowerGroupBox.Controls.Add(Me.currentLimitBehaviorComboBox)
        Me.configureDCPowerGroupBox.Controls.Add(Me.currentLimitBehaviorLabel)
        Me.configureDCPowerGroupBox.Controls.Add(Me.channelNameLabel)
        Me.configureDCPowerGroupBox.Location = New System.Drawing.Point(12, 174)
        Me.configureDCPowerGroupBox.Name = "configureDCPowerGroupBox"
        Me.configureDCPowerGroupBox.Size = New System.Drawing.Size(454, 182)
        Me.configureDCPowerGroupBox.TabIndex = 1
        Me.configureDCPowerGroupBox.TabStop = False
        Me.configureDCPowerGroupBox.Text = "Configure DC Power"
        '
        'configureAndOutputButton
        '
        Me.configureAndOutputButton.Location = New System.Drawing.Point(149, 150)
        Me.configureAndOutputButton.Name = "configureAndOutputButton"
        Me.configureAndOutputButton.Size = New System.Drawing.Size(120, 23)
        Me.configureAndOutputButton.TabIndex = 12
        Me.configureAndOutputButton.Text = "&Configure and Output"
        Me.configureAndOutputButton.UseVisualStyleBackColor = True
        '
        'ovpEnabledCheckBox
        '
        Me.ovpEnabledCheckBox.AutoSize = True
        Me.ovpEnabledCheckBox.Checked = True
        Me.ovpEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ovpEnabledCheckBox.Location = New System.Drawing.Point(52, 84)
        Me.ovpEnabledCheckBox.Name = "ovpEnabledCheckBox"
        Me.ovpEnabledCheckBox.Size = New System.Drawing.Size(90, 17)
        Me.ovpEnabledCheckBox.TabIndex = 11
        Me.ovpEnabledCheckBox.Text = "OVP Enabled"
        Me.ovpEnabledCheckBox.UseVisualStyleBackColor = True
        '
        'outputEnabledCheckBox
        '
        Me.outputEnabledCheckBox.AutoSize = True
        Me.outputEnabledCheckBox.Location = New System.Drawing.Point(52, 58)
        Me.outputEnabledCheckBox.Name = "outputEnabledCheckBox"
        Me.outputEnabledCheckBox.Size = New System.Drawing.Size(100, 17)
        Me.outputEnabledCheckBox.TabIndex = 10
        Me.outputEnabledCheckBox.Text = "Output Enabled"
        Me.outputEnabledCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.outputEnabledCheckBox.UseVisualStyleBackColor = True
        '
        'channelNameComboBox
        '
        Me.channelNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.channelNameComboBox.Location = New System.Drawing.Point(202, 19)
        Me.channelNameComboBox.Name = "channelNameComboBox"
        Me.channelNameComboBox.Size = New System.Drawing.Size(91, 21)
        Me.channelNameComboBox.TabIndex = 1
        '
        'currentLimitLabel
        '
        Me.currentLimitLabel.AutoSize = True
        Me.currentLimitLabel.Location = New System.Drawing.Point(245, 112)
        Me.currentLimitLabel.Name = "currentLimitLabel"
        Me.currentLimitLabel.Size = New System.Drawing.Size(81, 13)
        Me.currentLimitLabel.TabIndex = 6
        Me.currentLimitLabel.Text = "Current Limit (A)"
        '
        'currentLimitNumeric
        '
        Me.currentLimitNumeric.DecimalPlaces = 6
        Me.currentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.currentLimitNumeric.Location = New System.Drawing.Point(357, 108)
        Me.currentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.currentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.currentLimitNumeric.Name = "currentLimitNumeric"
        Me.currentLimitNumeric.Size = New System.Drawing.Size(91, 20)
        Me.currentLimitNumeric.TabIndex = 7
        Me.currentLimitNumeric.Value = New Decimal(New Integer() {2, 0, 0, 131072})
        '
        'ovpLimitLabel
        '
        Me.ovpLimitLabel.AutoSize = True
        Me.ovpLimitLabel.Location = New System.Drawing.Point(245, 86)
        Me.ovpLimitLabel.Name = "ovpLimitLabel"
        Me.ovpLimitLabel.Size = New System.Drawing.Size(69, 13)
        Me.ovpLimitLabel.TabIndex = 4
        Me.ovpLimitLabel.Text = "OVP Limit (V)"
        '
        'ovpLimitNumeric
        '
        Me.ovpLimitNumeric.DecimalPlaces = 6
        Me.ovpLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.ovpLimitNumeric.Location = New System.Drawing.Point(357, 82)
        Me.ovpLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.ovpLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.ovpLimitNumeric.Name = "ovpLimitNumeric"
        Me.ovpLimitNumeric.Size = New System.Drawing.Size(91, 20)
        Me.ovpLimitNumeric.TabIndex = 5
        Me.ovpLimitNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'voltageLevelLabel
        '
        Me.voltageLevelLabel.AutoSize = True
        Me.voltageLevelLabel.Location = New System.Drawing.Point(245, 60)
        Me.voltageLevelLabel.Name = "voltageLevelLabel"
        Me.voltageLevelLabel.Size = New System.Drawing.Size(106, 13)
        Me.voltageLevelLabel.TabIndex = 2
        Me.voltageLevelLabel.Text = "DC Voltage Level (V)"
        '
        'voltageLevelNumeric
        '
        Me.voltageLevelNumeric.DecimalPlaces = 6
        Me.voltageLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLevelNumeric.Location = New System.Drawing.Point(357, 56)
        Me.voltageLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLevelNumeric.Name = "voltageLevelNumeric"
        Me.voltageLevelNumeric.Size = New System.Drawing.Size(91, 20)
        Me.voltageLevelNumeric.TabIndex = 3
        Me.voltageLevelNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'currentLimitBehaviorComboBox
        '
        Me.currentLimitBehaviorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.currentLimitBehaviorComboBox.Location = New System.Drawing.Point(122, 107)
        Me.currentLimitBehaviorComboBox.Name = "currentLimitBehaviorComboBox"
        Me.currentLimitBehaviorComboBox.Size = New System.Drawing.Size(91, 21)
        Me.currentLimitBehaviorComboBox.TabIndex = 9
        '
        'currentLimitBehaviorLabel
        '
        Me.currentLimitBehaviorLabel.AutoSize = True
        Me.currentLimitBehaviorLabel.Location = New System.Drawing.Point(6, 112)
        Me.currentLimitBehaviorLabel.Name = "currentLimitBehaviorLabel"
        Me.currentLimitBehaviorLabel.Size = New System.Drawing.Size(110, 13)
        Me.currentLimitBehaviorLabel.TabIndex = 8
        Me.currentLimitBehaviorLabel.Text = "Current Limit Behavior"
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.Location = New System.Drawing.Point(119, 23)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.channelNameLabel.TabIndex = 0
        Me.channelNameLabel.Text = "Channel Name"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(478, 367)
        Me.Controls.Add(Me.configureDCPowerGroupBox)
        Me.Controls.Add(Me.initializeDCPowerGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "IVI DCPower .NET application"
        Me.initializeDCPowerGroupBox.ResumeLayout(False)
        Me.initializeDCPowerGroupBox.PerformLayout()
        Me.configureDCPowerGroupBox.ResumeLayout(False)
        Me.configureDCPowerGroupBox.PerformLayout()
        CType(Me.currentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ovpLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private initializeDCPowerGroupBox As System.Windows.Forms.GroupBox
    Private messageRichTextBox As System.Windows.Forms.RichTextBox
	Private logicalNameTextBox As System.Windows.Forms.TextBox
	Private logicalNameLabel As System.Windows.Forms.Label
	Private configureDCPowerGroupBox As System.Windows.Forms.GroupBox
	Private channelNameLabel As System.Windows.Forms.Label
	Private voltageLevelLabel As System.Windows.Forms.Label
	Private voltageLevelNumeric As System.Windows.Forms.NumericUpDown
	Private currentLimitBehaviorComboBox As System.Windows.Forms.ComboBox
	Private currentLimitBehaviorLabel As System.Windows.Forms.Label
	Private currentLimitLabel As System.Windows.Forms.Label
	Private currentLimitNumeric As System.Windows.Forms.NumericUpDown
	Private ovpLimitLabel As System.Windows.Forms.Label
	Private ovpLimitNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents configureAndOutputButton As System.Windows.Forms.Button
    Private WithEvents initializeButton As System.Windows.Forms.Button
	Private channelNameComboBox As System.Windows.Forms.ComboBox
	Private resetDeviceCheckBox As System.Windows.Forms.CheckBox
	Private idQueryCheckBox As System.Windows.Forms.CheckBox
	Private ovpEnabledCheckBox As System.Windows.Forms.CheckBox
	Private outputEnabledCheckBox As System.Windows.Forms.CheckBox
End Class

