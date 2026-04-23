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
        Me.pulseVoltageLevelLabel = New System.Windows.Forms.Label()
        Me.pulseCurrentLimitLabel = New System.Windows.Forms.Label()
        Me.pulseOnTimeLabel = New System.Windows.Forms.Label()
        Me.sourceDelayLabel = New System.Windows.Forms.Label()
        Me.pulseVoltageLevelRangeLabel = New System.Windows.Forms.Label()
        Me.pulseCurrentLimitRangeLabel = New System.Windows.Forms.Label()
        Me.pulseOffTimeLabel = New System.Windows.Forms.Label()
        Me.apertureTimeLabel = New System.Windows.Forms.Label()
        Me.biasVoltageLevelLabel = New System.Windows.Forms.Label()
        Me.biasCurrentLimitLabel = New System.Windows.Forms.Label()
        Me.pulseBiasDelayLabel = New System.Windows.Forms.Label()
        Me.voltageMeasurementLabel = New System.Windows.Forms.Label()
        Me.currentMeasurementLabel = New System.Windows.Forms.Label()
        Me.pulseVoltageLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseCurrentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseOnTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.sourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseVoltLevelRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseCurrentLimitRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseOffTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.apertureTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.biasVoltageLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.biasCurrentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseBiasDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceAndChannelNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.inputConfigurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.currentMeasurementTextBox = New System.Windows.Forms.TextBox()
        Me.voltageMeasurementTextBox = New System.Windows.Forms.TextBox()
        CType(Me.pulseVoltageLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseOnTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseVoltLevelRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseCurrentLimitRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseOffTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.apertureTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.biasVoltageLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.biasCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseBiasDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resourceAndChannelNameGroupBox.SuspendLayout()
        Me.inputConfigurationGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 29)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.Location = New System.Drawing.Point(8, 60)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.channelNameLabel.TabIndex = 1
        Me.channelNameLabel.Text = "Channel Name"
        '
        'pulseVoltageLevelLabel
        '
        Me.pulseVoltageLevelLabel.AutoSize = True
        Me.pulseVoltageLevelLabel.Location = New System.Drawing.Point(8, 27)
        Me.pulseVoltageLevelLabel.Name = "pulseVoltageLevelLabel"
        Me.pulseVoltageLevelLabel.Size = New System.Drawing.Size(133, 13)
        Me.pulseVoltageLevelLabel.TabIndex = 2
        Me.pulseVoltageLevelLabel.Text = "Pulse Voltage Level (Volts)"
        '
        'pulseCurrentLimitLabel
        '
        Me.pulseCurrentLimitLabel.AutoSize = True
        Me.pulseCurrentLimitLabel.Location = New System.Drawing.Point(8, 52)
        Me.pulseCurrentLimitLabel.Name = "pulseCurrentLimitLabel"
        Me.pulseCurrentLimitLabel.Size = New System.Drawing.Size(129, 13)
        Me.pulseCurrentLimitLabel.TabIndex = 3
        Me.pulseCurrentLimitLabel.Text = "Pulse Current Limit (Amps)"
        '
        'pulseOnTimeLabel
        '
        Me.pulseOnTimeLabel.AutoSize = True
        Me.pulseOnTimeLabel.Location = New System.Drawing.Point(8, 75)
        Me.pulseOnTimeLabel.Name = "pulseOnTimeLabel"
        Me.pulseOnTimeLabel.Size = New System.Drawing.Size(104, 13)
        Me.pulseOnTimeLabel.TabIndex = 4
        Me.pulseOnTimeLabel.Text = "Pulse On Time (Sec)"
        '
        'sourceDelayLabel
        '
        Me.sourceDelayLabel.AutoSize = True
        Me.sourceDelayLabel.Location = New System.Drawing.Point(288, 126)
        Me.sourceDelayLabel.Name = "sourceDelayLabel"
        Me.sourceDelayLabel.Size = New System.Drawing.Size(122, 13)
        Me.sourceDelayLabel.TabIndex = 5
        Me.sourceDelayLabel.Text = "Source Delay (Seconds)"
        '
        'pulseVoltageLevelRangeLabel
        '
        Me.pulseVoltageLevelRangeLabel.AutoSize = True
        Me.pulseVoltageLevelRangeLabel.Location = New System.Drawing.Point(288, 30)
        Me.pulseVoltageLevelRangeLabel.Name = "pulseVoltageLevelRangeLabel"
        Me.pulseVoltageLevelRangeLabel.Size = New System.Drawing.Size(168, 13)
        Me.pulseVoltageLevelRangeLabel.TabIndex = 6
        Me.pulseVoltageLevelRangeLabel.Text = "Pulse Voltage Level Range (Volts)"
        '
        'pulseCurrentLimitRangeLabel
        '
        Me.pulseCurrentLimitRangeLabel.AutoSize = True
        Me.pulseCurrentLimitRangeLabel.Location = New System.Drawing.Point(288, 55)
        Me.pulseCurrentLimitRangeLabel.Name = "pulseCurrentLimitRangeLabel"
        Me.pulseCurrentLimitRangeLabel.Size = New System.Drawing.Size(164, 13)
        Me.pulseCurrentLimitRangeLabel.TabIndex = 7
        Me.pulseCurrentLimitRangeLabel.Text = "Pulse Current Limit Range (Amps)"
        '
        'pulseOffTimeLabel
        '
        Me.pulseOffTimeLabel.AutoSize = True
        Me.pulseOffTimeLabel.Location = New System.Drawing.Point(288, 78)
        Me.pulseOffTimeLabel.Name = "pulseOffTimeLabel"
        Me.pulseOffTimeLabel.Size = New System.Drawing.Size(104, 13)
        Me.pulseOffTimeLabel.TabIndex = 8
        Me.pulseOffTimeLabel.Text = "Pulse Off Time (Sec)"
        '
        'apertureTimeLabel
        '
        Me.apertureTimeLabel.AutoSize = True
        Me.apertureTimeLabel.Location = New System.Drawing.Point(288, 102)
        Me.apertureTimeLabel.Name = "apertureTimeLabel"
        Me.apertureTimeLabel.Size = New System.Drawing.Size(101, 13)
        Me.apertureTimeLabel.TabIndex = 9
        Me.apertureTimeLabel.Text = "Aperture Time (Sec)"
        '
        'biasVoltageLevelLabel
        '
        Me.biasVoltageLevelLabel.AutoSize = True
        Me.biasVoltageLevelLabel.Location = New System.Drawing.Point(8, 99)
        Me.biasVoltageLevelLabel.Name = "biasVoltageLevelLabel"
        Me.biasVoltageLevelLabel.Size = New System.Drawing.Size(156, 13)
        Me.biasVoltageLevelLabel.TabIndex = 10
        Me.biasVoltageLevelLabel.Text = "Pulse Bias Voltage Level (Volts)"
        '
        'biasCurrentLimitLabel
        '
        Me.biasCurrentLimitLabel.AutoSize = True
        Me.biasCurrentLimitLabel.Location = New System.Drawing.Point(8, 121)
        Me.biasCurrentLimitLabel.Name = "biasCurrentLimitLabel"
        Me.biasCurrentLimitLabel.Size = New System.Drawing.Size(152, 13)
        Me.biasCurrentLimitLabel.TabIndex = 11
        Me.biasCurrentLimitLabel.Text = "Pulse Bias Current Limit (Amps)"
        '
        'pulseBiasDelayLabel
        '
        Me.pulseBiasDelayLabel.AutoSize = True
        Me.pulseBiasDelayLabel.Location = New System.Drawing.Point(8, 144)
        Me.pulseBiasDelayLabel.Name = "pulseBiasDelayLabel"
        Me.pulseBiasDelayLabel.Size = New System.Drawing.Size(114, 13)
        Me.pulseBiasDelayLabel.TabIndex = 12
        Me.pulseBiasDelayLabel.Text = "Pulse Bias Delay (Sec)"
        '
        'voltageMeasurementLabel
        '
        Me.voltageMeasurementLabel.AutoSize = True
        Me.voltageMeasurementLabel.Location = New System.Drawing.Point(6, 29)
        Me.voltageMeasurementLabel.Name = "voltageMeasurementLabel"
        Me.voltageMeasurementLabel.Size = New System.Drawing.Size(142, 13)
        Me.voltageMeasurementLabel.TabIndex = 15
        Me.voltageMeasurementLabel.Text = "Voltage Measurement (Volts)"
        '
        'currentMeasurementLabel
        '
        Me.currentMeasurementLabel.AutoSize = True
        Me.currentMeasurementLabel.Location = New System.Drawing.Point(6, 55)
        Me.currentMeasurementLabel.Name = "currentMeasurementLabel"
        Me.currentMeasurementLabel.Size = New System.Drawing.Size(143, 13)
        Me.currentMeasurementLabel.TabIndex = 16
        Me.currentMeasurementLabel.Text = "Current Measurement (Amps)"
        '
        'pulseVoltageLevelNumeric
        '
        Me.pulseVoltageLevelNumeric.DecimalPlaces = 5
        Me.pulseVoltageLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.pulseVoltageLevelNumeric.Location = New System.Drawing.Point(171, 27)
        Me.pulseVoltageLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseVoltageLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseVoltageLevelNumeric.Name = "pulseVoltageLevelNumeric"
        Me.pulseVoltageLevelNumeric.Size = New System.Drawing.Size(89, 20)
        Me.pulseVoltageLevelNumeric.TabIndex = 3
        Me.pulseVoltageLevelNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'pulseCurrentLimitNumeric
        '
        Me.pulseCurrentLimitNumeric.DecimalPlaces = 5
        Me.pulseCurrentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.pulseCurrentLimitNumeric.Location = New System.Drawing.Point(171, 50)
        Me.pulseCurrentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseCurrentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseCurrentLimitNumeric.Name = "pulseCurrentLimitNumeric"
        Me.pulseCurrentLimitNumeric.Size = New System.Drawing.Size(89, 20)
        Me.pulseCurrentLimitNumeric.TabIndex = 4
        Me.pulseCurrentLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 131072})
        '
        'pulseOnTimeNumeric
        '
        Me.pulseOnTimeNumeric.DecimalPlaces = 7
        Me.pulseOnTimeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.pulseOnTimeNumeric.Location = New System.Drawing.Point(171, 73)
        Me.pulseOnTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseOnTimeNumeric.Name = "pulseOnTimeNumeric"
        Me.pulseOnTimeNumeric.Size = New System.Drawing.Size(89, 20)
        Me.pulseOnTimeNumeric.TabIndex = 5
        Me.pulseOnTimeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        'sourceDelayNumeric
        '
        Me.sourceDelayNumeric.DecimalPlaces = 7
        Me.sourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.sourceDelayNumeric.Location = New System.Drawing.Point(462, 124)
        Me.sourceDelayNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.sourceDelayNumeric.Name = "sourceDelayNumeric"
        Me.sourceDelayNumeric.Size = New System.Drawing.Size(72, 20)
        Me.sourceDelayNumeric.TabIndex = 13
        Me.sourceDelayNumeric.Value = New Decimal(New Integer() {5, 0, 0, 327680})
        '
        'pulseVoltLevelRangeNumeric
        '
        Me.pulseVoltLevelRangeNumeric.DecimalPlaces = 5
        Me.pulseVoltLevelRangeNumeric.Location = New System.Drawing.Point(462, 28)
        Me.pulseVoltLevelRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseVoltLevelRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseVoltLevelRangeNumeric.Name = "pulseVoltLevelRangeNumeric"
        Me.pulseVoltLevelRangeNumeric.Size = New System.Drawing.Size(72, 20)
        Me.pulseVoltLevelRangeNumeric.TabIndex = 9
        Me.pulseVoltLevelRangeNumeric.Value = New Decimal(New Integer() {6, 0, 0, 0})
        '
        'pulseCurrentLimitRangeNumeric
        '
        Me.pulseCurrentLimitRangeNumeric.DecimalPlaces = 5
        Me.pulseCurrentLimitRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.pulseCurrentLimitRangeNumeric.Location = New System.Drawing.Point(462, 53)
        Me.pulseCurrentLimitRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseCurrentLimitRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseCurrentLimitRangeNumeric.Name = "pulseCurrentLimitRangeNumeric"
        Me.pulseCurrentLimitRangeNumeric.Size = New System.Drawing.Size(72, 20)
        Me.pulseCurrentLimitRangeNumeric.TabIndex = 10
        Me.pulseCurrentLimitRangeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'pulseOffTimeNumeric
        '
        Me.pulseOffTimeNumeric.DecimalPlaces = 7
        Me.pulseOffTimeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.pulseOffTimeNumeric.Location = New System.Drawing.Point(462, 78)
        Me.pulseOffTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseOffTimeNumeric.Name = "pulseOffTimeNumeric"
        Me.pulseOffTimeNumeric.Size = New System.Drawing.Size(72, 20)
        Me.pulseOffTimeNumeric.TabIndex = 11
        Me.pulseOffTimeNumeric.Value = New Decimal(New Integer() {5, 0, 0, 196608})
        '
        'apertureTimeNumeric
        '
        Me.apertureTimeNumeric.DecimalPlaces = 7
        Me.apertureTimeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.apertureTimeNumeric.Location = New System.Drawing.Point(462, 100)
        Me.apertureTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.apertureTimeNumeric.Name = "apertureTimeNumeric"
        Me.apertureTimeNumeric.Size = New System.Drawing.Size(72, 20)
        Me.apertureTimeNumeric.TabIndex = 12
        Me.apertureTimeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 262144})
        '
        'biasVoltageLevelNumeric
        '
        Me.biasVoltageLevelNumeric.DecimalPlaces = 5
        Me.biasVoltageLevelNumeric.Location = New System.Drawing.Point(171, 97)
        Me.biasVoltageLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.biasVoltageLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.biasVoltageLevelNumeric.Name = "biasVoltageLevelNumeric"
        Me.biasVoltageLevelNumeric.Size = New System.Drawing.Size(89, 20)
        Me.biasVoltageLevelNumeric.TabIndex = 6
        '
        'biasCurrentLimitNumeric
        '
        Me.biasCurrentLimitNumeric.DecimalPlaces = 5
        Me.biasCurrentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.biasCurrentLimitNumeric.Location = New System.Drawing.Point(171, 120)
        Me.biasCurrentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.biasCurrentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.biasCurrentLimitNumeric.Name = "biasCurrentLimitNumeric"
        Me.biasCurrentLimitNumeric.Size = New System.Drawing.Size(89, 20)
        Me.biasCurrentLimitNumeric.TabIndex = 7
        Me.biasCurrentLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 131072})
        '
        'pulseBiasDelayNumeric
        '
        Me.pulseBiasDelayNumeric.DecimalPlaces = 7
        Me.pulseBiasDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.pulseBiasDelayNumeric.Location = New System.Drawing.Point(171, 142)
        Me.pulseBiasDelayNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseBiasDelayNumeric.Name = "pulseBiasDelayNumeric"
        Me.pulseBiasDelayNumeric.Size = New System.Drawing.Size(89, 20)
        Me.pulseBiasDelayNumeric.TabIndex = 8
        Me.pulseBiasDelayNumeric.Value = New Decimal(New Integer() {1, 0, 0, 393216})
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(139, 58)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(120, 20)
        Me.channelNameTextBox.TabIndex = 2
        Me.channelNameTextBox.Text = "0"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(246, 303)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 14
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(138, 27)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.resourceNameComboBox.TabIndex = 11
        '
        'resourceAndChannelNameGroupBox
        '
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.channelNameLabel)
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.resourceAndChannelNameGroupBox.Location = New System.Drawing.Point(13, 13)
        Me.resourceAndChannelNameGroupBox.Name = "resourceAndChannelNameGroupBox"
        Me.resourceAndChannelNameGroupBox.Size = New System.Drawing.Size(278, 100)
        Me.resourceAndChannelNameGroupBox.TabIndex = 18
        Me.resourceAndChannelNameGroupBox.TabStop = False
        Me.resourceAndChannelNameGroupBox.Text = "Resource Name and Channel Name"
        '
        'inputConfigurationGroupBox
        '
        Me.inputConfigurationGroupBox.Controls.Add(Me.pulseVoltageLevelLabel)
        Me.inputConfigurationGroupBox.Controls.Add(Me.pulseVoltageLevelNumeric)
        Me.inputConfigurationGroupBox.Controls.Add(Me.sourceDelayLabel)
        Me.inputConfigurationGroupBox.Controls.Add(Me.pulseOnTimeLabel)
        Me.inputConfigurationGroupBox.Controls.Add(Me.pulseOffTimeNumeric)
        Me.inputConfigurationGroupBox.Controls.Add(Me.pulseCurrentLimitRangeNumeric)
        Me.inputConfigurationGroupBox.Controls.Add(Me.sourceDelayNumeric)
        Me.inputConfigurationGroupBox.Controls.Add(Me.apertureTimeLabel)
        Me.inputConfigurationGroupBox.Controls.Add(Me.pulseOffTimeLabel)
        Me.inputConfigurationGroupBox.Controls.Add(Me.pulseCurrentLimitRangeLabel)
        Me.inputConfigurationGroupBox.Controls.Add(Me.pulseVoltageLevelRangeLabel)
        Me.inputConfigurationGroupBox.Controls.Add(Me.pulseCurrentLimitLabel)
        Me.inputConfigurationGroupBox.Controls.Add(Me.apertureTimeNumeric)
        Me.inputConfigurationGroupBox.Controls.Add(Me.pulseCurrentLimitNumeric)
        Me.inputConfigurationGroupBox.Controls.Add(Me.pulseOnTimeNumeric)
        Me.inputConfigurationGroupBox.Controls.Add(Me.biasVoltageLevelLabel)
        Me.inputConfigurationGroupBox.Controls.Add(Me.pulseBiasDelayLabel)
        Me.inputConfigurationGroupBox.Controls.Add(Me.pulseVoltLevelRangeNumeric)
        Me.inputConfigurationGroupBox.Controls.Add(Me.biasCurrentLimitLabel)
        Me.inputConfigurationGroupBox.Controls.Add(Me.biasVoltageLevelNumeric)
        Me.inputConfigurationGroupBox.Controls.Add(Me.biasCurrentLimitNumeric)
        Me.inputConfigurationGroupBox.Controls.Add(Me.pulseBiasDelayNumeric)
        Me.inputConfigurationGroupBox.Location = New System.Drawing.Point(13, 119)
        Me.inputConfigurationGroupBox.Name = "inputConfigurationGroupBox"
        Me.inputConfigurationGroupBox.Size = New System.Drawing.Size(560, 169)
        Me.inputConfigurationGroupBox.TabIndex = 19
        Me.inputConfigurationGroupBox.TabStop = False
        Me.inputConfigurationGroupBox.Text = "Input Configuration"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.currentMeasurementTextBox)
        Me.measurementGroupBox.Controls.Add(Me.voltageMeasurementTextBox)
        Me.measurementGroupBox.Controls.Add(Me.voltageMeasurementLabel)
        Me.measurementGroupBox.Controls.Add(Me.currentMeasurementLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(304, 13)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(269, 100)
        Me.measurementGroupBox.TabIndex = 20
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurements"
        '
        'currentMeasurementTextBox
        '
        Me.currentMeasurementTextBox.Location = New System.Drawing.Point(155, 53)
        Me.currentMeasurementTextBox.Name = "currentMeasurementTextBox"
        Me.currentMeasurementTextBox.ReadOnly = True
        Me.currentMeasurementTextBox.Size = New System.Drawing.Size(100, 20)
        Me.currentMeasurementTextBox.TabIndex = 18
        '
        'voltageMeasurementTextBox
        '
        Me.voltageMeasurementTextBox.Location = New System.Drawing.Point(155, 29)
        Me.voltageMeasurementTextBox.Name = "voltageMeasurementTextBox"
        Me.voltageMeasurementTextBox.ReadOnly = True
        Me.voltageMeasurementTextBox.Size = New System.Drawing.Size(100, 20)
        Me.voltageMeasurementTextBox.TabIndex = 17
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(583, 340)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.inputConfigurationGroupBox)
        Me.Controls.Add(Me.resourceAndChannelNameGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Pulse Voltage"
        CType(Me.pulseVoltageLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseOnTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseVoltLevelRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseCurrentLimitRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseOffTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.apertureTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.biasVoltageLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.biasCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseBiasDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.resourceAndChannelNameGroupBox.ResumeLayout(False)
        Me.resourceAndChannelNameGroupBox.PerformLayout()
        Me.inputConfigurationGroupBox.ResumeLayout(False)
        Me.inputConfigurationGroupBox.PerformLayout()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub



#End Region
    Private resourceNameLabel As System.Windows.Forms.Label
    Private channelNameLabel As System.Windows.Forms.Label
    Private pulseVoltageLevelLabel As System.Windows.Forms.Label
    Private pulseCurrentLimitLabel As System.Windows.Forms.Label
    Private pulseOnTimeLabel As System.Windows.Forms.Label
    Private sourceDelayLabel As System.Windows.Forms.Label
    Private pulseVoltageLevelRangeLabel As System.Windows.Forms.Label
    Private pulseCurrentLimitRangeLabel As System.Windows.Forms.Label
    Private pulseOffTimeLabel As System.Windows.Forms.Label
    Private apertureTimeLabel As System.Windows.Forms.Label
    Private biasVoltageLevelLabel As System.Windows.Forms.Label
    Private biasCurrentLimitLabel As System.Windows.Forms.Label
    Private pulseBiasDelayLabel As System.Windows.Forms.Label
    Private voltageMeasurementLabel As System.Windows.Forms.Label
    Private currentMeasurementLabel As System.Windows.Forms.Label
    Private pulseVoltageLevelNumeric As System.Windows.Forms.NumericUpDown
    Private pulseCurrentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private pulseOnTimeNumeric As System.Windows.Forms.NumericUpDown
    Private sourceDelayNumeric As System.Windows.Forms.NumericUpDown
    Private pulseVoltLevelRangeNumeric As System.Windows.Forms.NumericUpDown
    Private pulseCurrentLimitRangeNumeric As System.Windows.Forms.NumericUpDown
    Private pulseOffTimeNumeric As System.Windows.Forms.NumericUpDown
    Private apertureTimeNumeric As System.Windows.Forms.NumericUpDown
    Private biasVoltageLevelNumeric As System.Windows.Forms.NumericUpDown
    Private biasCurrentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private pulseBiasDelayNumeric As System.Windows.Forms.NumericUpDown
    Private channelNameTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private resourceAndChannelNameGroupBox As System.Windows.Forms.GroupBox
    Private inputConfigurationGroupBox As System.Windows.Forms.GroupBox
    Private measurementGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents currentMeasurementTextBox As System.Windows.Forms.TextBox
    Friend WithEvents voltageMeasurementTextBox As System.Windows.Forms.TextBox
End Class

