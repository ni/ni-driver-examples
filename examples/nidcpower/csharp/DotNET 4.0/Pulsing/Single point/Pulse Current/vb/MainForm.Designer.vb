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
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.pulseCurrentLevelLabel = New System.Windows.Forms.Label()
        Me.pulseVoltageLimitLabel = New System.Windows.Forms.Label()
        Me.pulseOnTimeLabel = New System.Windows.Forms.Label()
        Me.sourceDelayLabel = New System.Windows.Forms.Label()
        Me.pulseCurrentLevelRangeLabel = New System.Windows.Forms.Label()
        Me.pulseVoltageLimitRangeLabel = New System.Windows.Forms.Label()
        Me.pulseOffTimeLabel = New System.Windows.Forms.Label()
        Me.apertureTimeLabel = New System.Windows.Forms.Label()
        Me.biasCurrentLevelLabel = New System.Windows.Forms.Label()
        Me.biasVoltageLimitLabel = New System.Windows.Forms.Label()
        Me.pulseBiasDelayLabel = New System.Windows.Forms.Label()
        Me.currentMeasurementLabel = New System.Windows.Forms.Label()
        Me.voltageMeasurementLabel = New System.Windows.Forms.Label()
        Me.pulseCurrentLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseVoltageLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseOnTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.sourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseCurrentLevelRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseVoltLimitRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseOffTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.apertureTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.biasCurrentLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.biasVoltageLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseBiasDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceAndChannelNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.currentMeasurementTextBox = New System.Windows.Forms.TextBox()
        Me.voltageMeasurementTextBox = New System.Windows.Forms.TextBox()
        CType(Me.pulseCurrentLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseOnTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseCurrentLevelRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseVoltLimitRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseOffTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.apertureTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.biasCurrentLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.biasVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseBiasDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resourceAndChannelNameGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 28)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.Location = New System.Drawing.Point(6, 54)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.channelNameLabel.TabIndex = 1
        Me.channelNameLabel.Text = "Channel Name"
        '
        'pulseCurrentLevelLabel
        '
        Me.pulseCurrentLevelLabel.AutoSize = True
        Me.pulseCurrentLevelLabel.Location = New System.Drawing.Point(6, 21)
        Me.pulseCurrentLevelLabel.Name = "pulseCurrentLevelLabel"
        Me.pulseCurrentLevelLabel.Size = New System.Drawing.Size(134, 13)
        Me.pulseCurrentLevelLabel.TabIndex = 2
        Me.pulseCurrentLevelLabel.Text = "Pulse Current Level (Amps)"
        '
        'pulseVoltageLimitLabel
        '
        Me.pulseVoltageLimitLabel.AutoSize = True
        Me.pulseVoltageLimitLabel.Location = New System.Drawing.Point(6, 43)
        Me.pulseVoltageLimitLabel.Name = "pulseVoltageLimitLabel"
        Me.pulseVoltageLimitLabel.Size = New System.Drawing.Size(128, 13)
        Me.pulseVoltageLimitLabel.TabIndex = 3
        Me.pulseVoltageLimitLabel.Text = "Pulse Voltage Limit (Volts)"
        '
        'pulseOnTimeLabel
        '
        Me.pulseOnTimeLabel.AutoSize = True
        Me.pulseOnTimeLabel.Location = New System.Drawing.Point(6, 65)
        Me.pulseOnTimeLabel.Name = "pulseOnTimeLabel"
        Me.pulseOnTimeLabel.Size = New System.Drawing.Size(104, 13)
        Me.pulseOnTimeLabel.TabIndex = 4
        Me.pulseOnTimeLabel.Text = "Pulse On Time (Sec)"
        '
        'sourceDelayLabel
        '
        Me.sourceDelayLabel.AutoSize = True
        Me.sourceDelayLabel.Location = New System.Drawing.Point(6, 112)
        Me.sourceDelayLabel.Name = "sourceDelayLabel"
        Me.sourceDelayLabel.Size = New System.Drawing.Size(99, 13)
        Me.sourceDelayLabel.TabIndex = 5
        Me.sourceDelayLabel.Text = "Source Delay (Sec)"
        '
        'pulseCurrentLevelRangeLabel
        '
        Me.pulseCurrentLevelRangeLabel.AutoSize = True
        Me.pulseCurrentLevelRangeLabel.Location = New System.Drawing.Point(296, 20)
        Me.pulseCurrentLevelRangeLabel.Name = "pulseCurrentLevelRangeLabel"
        Me.pulseCurrentLevelRangeLabel.Size = New System.Drawing.Size(169, 13)
        Me.pulseCurrentLevelRangeLabel.TabIndex = 6
        Me.pulseCurrentLevelRangeLabel.Text = "Pulse Current Level Range (Amps)"
        '
        'pulseVoltageLimitRangeLabel
        '
        Me.pulseVoltageLimitRangeLabel.AutoSize = True
        Me.pulseVoltageLimitRangeLabel.Location = New System.Drawing.Point(296, 43)
        Me.pulseVoltageLimitRangeLabel.Name = "pulseVoltageLimitRangeLabel"
        Me.pulseVoltageLimitRangeLabel.Size = New System.Drawing.Size(163, 13)
        Me.pulseVoltageLimitRangeLabel.TabIndex = 7
        Me.pulseVoltageLimitRangeLabel.Text = "Pulse Voltage Limit Range (Volts)"
        '
        'pulseOffTimeLabel
        '
        Me.pulseOffTimeLabel.AutoSize = True
        Me.pulseOffTimeLabel.Location = New System.Drawing.Point(6, 90)
        Me.pulseOffTimeLabel.Name = "pulseOffTimeLabel"
        Me.pulseOffTimeLabel.Size = New System.Drawing.Size(104, 13)
        Me.pulseOffTimeLabel.TabIndex = 8
        Me.pulseOffTimeLabel.Text = "Pulse Off Time (Sec)"
        '
        'apertureTimeLabel
        '
        Me.apertureTimeLabel.AutoSize = True
        Me.apertureTimeLabel.Location = New System.Drawing.Point(296, 64)
        Me.apertureTimeLabel.Name = "apertureTimeLabel"
        Me.apertureTimeLabel.Size = New System.Drawing.Size(101, 13)
        Me.apertureTimeLabel.TabIndex = 9
        Me.apertureTimeLabel.Text = "Aperture Time (Sec)"
        '
        'biasCurrentLevelLabel
        '
        Me.biasCurrentLevelLabel.AutoSize = True
        Me.biasCurrentLevelLabel.Location = New System.Drawing.Point(296, 89)
        Me.biasCurrentLevelLabel.Name = "biasCurrentLevelLabel"
        Me.biasCurrentLevelLabel.Size = New System.Drawing.Size(157, 13)
        Me.biasCurrentLevelLabel.TabIndex = 10
        Me.biasCurrentLevelLabel.Text = "Pulse Bias Current Level (Amps)"
        '
        'biasVoltageLimitLabel
        '
        Me.biasVoltageLimitLabel.AutoSize = True
        Me.biasVoltageLimitLabel.Location = New System.Drawing.Point(296, 114)
        Me.biasVoltageLimitLabel.Name = "biasVoltageLimitLabel"
        Me.biasVoltageLimitLabel.Size = New System.Drawing.Size(151, 13)
        Me.biasVoltageLimitLabel.TabIndex = 11
        Me.biasVoltageLimitLabel.Text = "Pulse Bias Voltage Limit (Volts)"
        '
        'pulseBiasDelayLabel
        '
        Me.pulseBiasDelayLabel.AutoSize = True
        Me.pulseBiasDelayLabel.Location = New System.Drawing.Point(7, 137)
        Me.pulseBiasDelayLabel.Name = "pulseBiasDelayLabel"
        Me.pulseBiasDelayLabel.Size = New System.Drawing.Size(114, 13)
        Me.pulseBiasDelayLabel.TabIndex = 12
        Me.pulseBiasDelayLabel.Text = "Pulse Bias Delay (Sec)"
        '
        'currentMeasurementLabel
        '
        Me.currentMeasurementLabel.AutoSize = True
        Me.currentMeasurementLabel.Location = New System.Drawing.Point(6, 28)
        Me.currentMeasurementLabel.Name = "currentMeasurementLabel"
        Me.currentMeasurementLabel.Size = New System.Drawing.Size(143, 13)
        Me.currentMeasurementLabel.TabIndex = 15
        Me.currentMeasurementLabel.Text = "Current Measurement (Amps)"
        '
        'voltageMeasurementLabel
        '
        Me.voltageMeasurementLabel.AutoSize = True
        Me.voltageMeasurementLabel.Location = New System.Drawing.Point(6, 54)
        Me.voltageMeasurementLabel.Name = "voltageMeasurementLabel"
        Me.voltageMeasurementLabel.Size = New System.Drawing.Size(142, 13)
        Me.voltageMeasurementLabel.TabIndex = 16
        Me.voltageMeasurementLabel.Text = "Voltage Measurement (Volts)"
        '
        'pulseCurrentLevelNumeric
        '
        Me.pulseCurrentLevelNumeric.DecimalPlaces = 5
        Me.pulseCurrentLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.pulseCurrentLevelNumeric.Location = New System.Drawing.Point(174, 18)
        Me.pulseCurrentLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseCurrentLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseCurrentLevelNumeric.Name = "pulseCurrentLevelNumeric"
        Me.pulseCurrentLevelNumeric.Size = New System.Drawing.Size(78, 20)
        Me.pulseCurrentLevelNumeric.TabIndex = 3
        Me.pulseCurrentLevelNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'pulseVoltageLimitNumeric
        '
        Me.pulseVoltageLimitNumeric.DecimalPlaces = 5
        Me.pulseVoltageLimitNumeric.Location = New System.Drawing.Point(174, 40)
        Me.pulseVoltageLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseVoltageLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseVoltageLimitNumeric.Name = "pulseVoltageLimitNumeric"
        Me.pulseVoltageLimitNumeric.Size = New System.Drawing.Size(78, 20)
        Me.pulseVoltageLimitNumeric.TabIndex = 4
        Me.pulseVoltageLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'pulseOnTimeNumeric
        '
        Me.pulseOnTimeNumeric.DecimalPlaces = 7
        Me.pulseOnTimeNumeric.Location = New System.Drawing.Point(174, 62)
        Me.pulseOnTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseOnTimeNumeric.Name = "pulseOnTimeNumeric"
        Me.pulseOnTimeNumeric.Size = New System.Drawing.Size(78, 20)
        Me.pulseOnTimeNumeric.TabIndex = 5
        Me.pulseOnTimeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        'sourceDelayNumeric
        '
        Me.sourceDelayNumeric.DecimalPlaces = 7
        Me.sourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.sourceDelayNumeric.Location = New System.Drawing.Point(174, 109)
        Me.sourceDelayNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.sourceDelayNumeric.Name = "sourceDelayNumeric"
        Me.sourceDelayNumeric.Size = New System.Drawing.Size(78, 20)
        Me.sourceDelayNumeric.TabIndex = 7
        Me.sourceDelayNumeric.Value = New Decimal(New Integer() {5, 0, 0, 393216})
        '
        'pulseCurrentLevelRangeNumeric
        '
        Me.pulseCurrentLevelRangeNumeric.DecimalPlaces = 5
        Me.pulseCurrentLevelRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.pulseCurrentLevelRangeNumeric.Location = New System.Drawing.Point(480, 18)
        Me.pulseCurrentLevelRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseCurrentLevelRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseCurrentLevelRangeNumeric.Name = "pulseCurrentLevelRangeNumeric"
        Me.pulseCurrentLevelRangeNumeric.Size = New System.Drawing.Size(78, 20)
        Me.pulseCurrentLevelRangeNumeric.TabIndex = 11
        Me.pulseCurrentLevelRangeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'pulseVoltLimitRangeNumeric
        '
        Me.pulseVoltLimitRangeNumeric.DecimalPlaces = 5
        Me.pulseVoltLimitRangeNumeric.Location = New System.Drawing.Point(480, 41)
        Me.pulseVoltLimitRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseVoltLimitRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseVoltLimitRangeNumeric.Name = "pulseVoltLimitRangeNumeric"
        Me.pulseVoltLimitRangeNumeric.Size = New System.Drawing.Size(78, 20)
        Me.pulseVoltLimitRangeNumeric.TabIndex = 12
        Me.pulseVoltLimitRangeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'pulseOffTimeNumeric
        '
        Me.pulseOffTimeNumeric.DecimalPlaces = 7
        Me.pulseOffTimeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.pulseOffTimeNumeric.Location = New System.Drawing.Point(174, 86)
        Me.pulseOffTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseOffTimeNumeric.Name = "pulseOffTimeNumeric"
        Me.pulseOffTimeNumeric.Size = New System.Drawing.Size(78, 20)
        Me.pulseOffTimeNumeric.TabIndex = 6
        Me.pulseOffTimeNumeric.Value = New Decimal(New Integer() {5, 0, 0, 196608})
        '
        'apertureTimeNumeric
        '
        Me.apertureTimeNumeric.DecimalPlaces = 7
        Me.apertureTimeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.apertureTimeNumeric.Location = New System.Drawing.Point(480, 65)
        Me.apertureTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.apertureTimeNumeric.Name = "apertureTimeNumeric"
        Me.apertureTimeNumeric.Size = New System.Drawing.Size(78, 20)
        Me.apertureTimeNumeric.TabIndex = 13
        Me.apertureTimeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 262144})
        '
        'biasCurrentLevelNumeric
        '
        Me.biasCurrentLevelNumeric.DecimalPlaces = 5
        Me.biasCurrentLevelNumeric.Location = New System.Drawing.Point(480, 89)
        Me.biasCurrentLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.biasCurrentLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.biasCurrentLevelNumeric.Name = "biasCurrentLevelNumeric"
        Me.biasCurrentLevelNumeric.Size = New System.Drawing.Size(78, 20)
        Me.biasCurrentLevelNumeric.TabIndex = 14
        '
        'biasVoltageLimitNumeric
        '
        Me.biasVoltageLimitNumeric.DecimalPlaces = 5
        Me.biasVoltageLimitNumeric.Location = New System.Drawing.Point(480, 114)
        Me.biasVoltageLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.biasVoltageLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.biasVoltageLimitNumeric.Name = "biasVoltageLimitNumeric"
        Me.biasVoltageLimitNumeric.Size = New System.Drawing.Size(75, 20)
        Me.biasVoltageLimitNumeric.TabIndex = 15
        Me.biasVoltageLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'pulseBiasDelayNumeric
        '
        Me.pulseBiasDelayNumeric.DecimalPlaces = 7
        Me.pulseBiasDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.pulseBiasDelayNumeric.Location = New System.Drawing.Point(175, 134)
        Me.pulseBiasDelayNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseBiasDelayNumeric.Name = "pulseBiasDelayNumeric"
        Me.pulseBiasDelayNumeric.Size = New System.Drawing.Size(75, 20)
        Me.pulseBiasDelayNumeric.TabIndex = 8
        Me.pulseBiasDelayNumeric.Value = New Decimal(New Integer() {1, 0, 0, 393216})
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(173, 54)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(94, 20)
        Me.channelNameTextBox.TabIndex = 2
        Me.channelNameTextBox.Text = "0"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(248, 281)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(72, 26)
        Me.startButton.TabIndex = 16
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(173, 28)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(94, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'resourceAndChannelNameGroupBox
        '
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.channelNameLabel)
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.resourceAndChannelNameGroupBox.Location = New System.Drawing.Point(13, 13)
        Me.resourceAndChannelNameGroupBox.Name = "resourceAndChannelNameGroupBox"
        Me.resourceAndChannelNameGroupBox.Size = New System.Drawing.Size(277, 83)
        Me.resourceAndChannelNameGroupBox.TabIndex = 18
        Me.resourceAndChannelNameGroupBox.TabStop = False
        Me.resourceAndChannelNameGroupBox.Text = "Resource Name and Channel Name"
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.pulseCurrentLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseCurrentLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseBiasDelayLabel)
        Me.configurationGroupBox.Controls.Add(Me.biasVoltageLimitLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseBiasDelayNumeric)
        Me.configurationGroupBox.Controls.Add(Me.biasCurrentLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.apertureTimeLabel)
        Me.configurationGroupBox.Controls.Add(Me.biasVoltageLimitNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseVoltageLimitRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseCurrentLevelRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.biasCurrentLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseOnTimeLabel)
        Me.configurationGroupBox.Controls.Add(Me.apertureTimeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseVoltageLimitLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseVoltageLimitNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseVoltLimitRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseOffTimeLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseOnTimeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseCurrentLevelRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseOffTimeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayNumeric)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 102)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(568, 173)
        Me.configurationGroupBox.TabIndex = 19
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Input Configuration"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.voltageMeasurementTextBox)
        Me.measurementGroupBox.Controls.Add(Me.currentMeasurementTextBox)
        Me.measurementGroupBox.Controls.Add(Me.currentMeasurementLabel)
        Me.measurementGroupBox.Controls.Add(Me.voltageMeasurementLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(301, 12)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(279, 91)
        Me.measurementGroupBox.TabIndex = 20
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurements"
        '
        'currentMeasurementTextBox
        '
        Me.currentMeasurementTextBox.Location = New System.Drawing.Point(169, 25)
        Me.currentMeasurementTextBox.Name = "currentMeasurementTextBox"
        Me.currentMeasurementTextBox.ReadOnly = True
        Me.currentMeasurementTextBox.Size = New System.Drawing.Size(100, 20)
        Me.currentMeasurementTextBox.TabIndex = 17
        '
        'voltageMeasurementTextBox
        '
        Me.voltageMeasurementTextBox.Location = New System.Drawing.Point(169, 55)
        Me.voltageMeasurementTextBox.Name = "voltageMeasurementTextBox"
        Me.voltageMeasurementTextBox.ReadOnly = True
        Me.voltageMeasurementTextBox.Size = New System.Drawing.Size(100, 20)
        Me.voltageMeasurementTextBox.TabIndex = 18
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(589, 313)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceAndChannelNameGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Pulse Current"
        CType(Me.pulseCurrentLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseOnTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseCurrentLevelRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseVoltLimitRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseOffTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.apertureTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.biasCurrentLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.biasVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseBiasDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.resourceAndChannelNameGroupBox.ResumeLayout(False)
        Me.resourceAndChannelNameGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private resourceNameLabel As System.Windows.Forms.Label
    Private channelNameLabel As System.Windows.Forms.Label
    Private pulseCurrentLevelLabel As System.Windows.Forms.Label
    Private pulseVoltageLimitLabel As System.Windows.Forms.Label
    Private pulseOnTimeLabel As System.Windows.Forms.Label
    Private sourceDelayLabel As System.Windows.Forms.Label
    Private pulseCurrentLevelRangeLabel As System.Windows.Forms.Label
    Private pulseVoltageLimitRangeLabel As System.Windows.Forms.Label
    Private pulseOffTimeLabel As System.Windows.Forms.Label
    Private apertureTimeLabel As System.Windows.Forms.Label
    Private biasCurrentLevelLabel As System.Windows.Forms.Label
    Private biasVoltageLimitLabel As System.Windows.Forms.Label
    Private pulseBiasDelayLabel As System.Windows.Forms.Label
    Private currentMeasurementLabel As System.Windows.Forms.Label
    Private voltageMeasurementLabel As System.Windows.Forms.Label
    Private pulseCurrentLevelNumeric As System.Windows.Forms.NumericUpDown
    Private pulseVoltageLimitNumeric As System.Windows.Forms.NumericUpDown
    Private pulseOnTimeNumeric As System.Windows.Forms.NumericUpDown
    Private sourceDelayNumeric As System.Windows.Forms.NumericUpDown
    Private pulseCurrentLevelRangeNumeric As System.Windows.Forms.NumericUpDown
    Private pulseVoltLimitRangeNumeric As System.Windows.Forms.NumericUpDown
    Private pulseOffTimeNumeric As System.Windows.Forms.NumericUpDown
    Private apertureTimeNumeric As System.Windows.Forms.NumericUpDown
    Private biasCurrentLevelNumeric As System.Windows.Forms.NumericUpDown
    Private biasVoltageLimitNumeric As System.Windows.Forms.NumericUpDown
    Private pulseBiasDelayNumeric As System.Windows.Forms.NumericUpDown
    Private channelNameTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private resourceAndChannelNameGroupBox As System.Windows.Forms.GroupBox
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private measurementGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents voltageMeasurementTextBox As System.Windows.Forms.TextBox
    Friend WithEvents currentMeasurementTextBox As System.Windows.Forms.TextBox
End Class

