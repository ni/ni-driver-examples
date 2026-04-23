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
        Me.sourceDelayLabel = New System.Windows.Forms.Label()
        Me.voltageLevelRangeLabel = New System.Windows.Forms.Label()
        Me.fetchTimeoutLabel = New System.Windows.Forms.Label()
        Me.currentLimitRangeLabel = New System.Windows.Forms.Label()
        Me.currentLimitLabel = New System.Windows.Forms.Label()
        Me.sourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.voltageLevelRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.fetchTimeoutNumeric = New System.Windows.Forms.NumericUpDown()
        Me.voltageLevel1Numeric = New System.Windows.Forms.NumericUpDown()
        Me.currentLimitRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.currentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startButton = New System.Windows.Forms.Button()
        Me.resourceNameAndChannelNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.voltageLevel1GroupBox = New System.Windows.Forms.GroupBox()
        Me.voltageLevel2Numeric = New System.Windows.Forms.NumericUpDown()
        Me.measurements1GroupBox = New System.Windows.Forms.GroupBox()
        Me.voltage1MeasurementsTextBox = New System.Windows.Forms.TextBox()
        Me.current1MeasurementsTextBox = New System.Windows.Forms.TextBox()
        Me.measuredCurrent1Label = New System.Windows.Forms.Label()
        Me.inCompliance1ButtonLed = New System.Windows.Forms.Button()
        Me.measuredVoltage1Label = New System.Windows.Forms.Label()
        Me.inCompliance1Label = New System.Windows.Forms.Label()
        Me.voltageLevel2GroupBox = New System.Windows.Forms.GroupBox()
        Me.measurements2GroupBox = New System.Windows.Forms.GroupBox()
        Me.voltage2MeasurementsTextBox = New System.Windows.Forms.TextBox()
        Me.current2MeasurementsTextBox = New System.Windows.Forms.TextBox()
        Me.measuredCurrent2Label = New System.Windows.Forms.Label()
        Me.inCompliance2ButtonLed = New System.Windows.Forms.Button()
        Me.measuredVoltage2Label = New System.Windows.Forms.Label()
        Me.inCompliance2Label = New System.Windows.Forms.Label()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageLevelRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.fetchTimeoutNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageLevel1Numeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.currentLimitRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.currentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resourceNameAndChannelNameGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        Me.voltageLevel1GroupBox.SuspendLayout()
        CType(Me.voltageLevel2Numeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurements1GroupBox.SuspendLayout()
        Me.voltageLevel2GroupBox.SuspendLayout()
        Me.measurements2GroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'sourceDelayLabel
        '
        Me.sourceDelayLabel.AutoSize = True
        Me.sourceDelayLabel.Location = New System.Drawing.Point(6, 119)
        Me.sourceDelayLabel.Name = "sourceDelayLabel"
        Me.sourceDelayLabel.Size = New System.Drawing.Size(85, 13)
        Me.sourceDelayLabel.TabIndex = 8
        Me.sourceDelayLabel.Text = "Source Delay (s)"
        '
        'voltageLevelRangeLabel
        '
        Me.voltageLevelRangeLabel.AutoSize = True
        Me.voltageLevelRangeLabel.Location = New System.Drawing.Point(6, 52)
        Me.voltageLevelRangeLabel.Name = "voltageLevelRangeLabel"
        Me.voltageLevelRangeLabel.Size = New System.Drawing.Size(123, 13)
        Me.voltageLevelRangeLabel.TabIndex = 4
        Me.voltageLevelRangeLabel.Text = "Voltage Level Range (V)"
        '
        'fetchTimeoutLabel
        '
        Me.fetchTimeoutLabel.AutoSize = True
        Me.fetchTimeoutLabel.Location = New System.Drawing.Point(6, 153)
        Me.fetchTimeoutLabel.Name = "fetchTimeoutLabel"
        Me.fetchTimeoutLabel.Size = New System.Drawing.Size(89, 13)
        Me.fetchTimeoutLabel.TabIndex = 10
        Me.fetchTimeoutLabel.Text = "Fetch Timeout (s)"
        '
        'currentLimitRangeLabel
        '
        Me.currentLimitRangeLabel.AutoSize = True
        Me.currentLimitRangeLabel.Location = New System.Drawing.Point(6, 85)
        Me.currentLimitRangeLabel.Name = "currentLimitRangeLabel"
        Me.currentLimitRangeLabel.Size = New System.Drawing.Size(116, 13)
        Me.currentLimitRangeLabel.TabIndex = 6
        Me.currentLimitRangeLabel.Text = "Current Limit Range (A)"
        '
        'currentLimitLabel
        '
        Me.currentLimitLabel.AutoSize = True
        Me.currentLimitLabel.Location = New System.Drawing.Point(6, 25)
        Me.currentLimitLabel.Name = "currentLimitLabel"
        Me.currentLimitLabel.Size = New System.Drawing.Size(81, 13)
        Me.currentLimitLabel.TabIndex = 2
        Me.currentLimitLabel.Text = "Current Limit (A)"
        '
        'sourceDelayNumeric
        '
        Me.sourceDelayNumeric.DecimalPlaces = 6
        Me.sourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.sourceDelayNumeric.Location = New System.Drawing.Point(141, 116)
        Me.sourceDelayNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.sourceDelayNumeric.Name = "sourceDelayNumeric"
        Me.sourceDelayNumeric.Size = New System.Drawing.Size(90, 20)
        Me.sourceDelayNumeric.TabIndex = 9
        Me.sourceDelayNumeric.Value = New Decimal(New Integer() {5, 0, 0, 131072})
        '
        'voltageLevelRangeNumeric
        '
        Me.voltageLevelRangeNumeric.DecimalPlaces = 6
        Me.voltageLevelRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLevelRangeNumeric.Location = New System.Drawing.Point(141, 47)
        Me.voltageLevelRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLevelRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLevelRangeNumeric.Name = "voltageLevelRangeNumeric"
        Me.voltageLevelRangeNumeric.Size = New System.Drawing.Size(90, 20)
        Me.voltageLevelRangeNumeric.TabIndex = 5
        Me.voltageLevelRangeNumeric.Value = New Decimal(New Integer() {6, 0, 0, 0})
        '
        'fetchTimeoutNumeric
        '
        Me.fetchTimeoutNumeric.DecimalPlaces = 6
        Me.fetchTimeoutNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.fetchTimeoutNumeric.Location = New System.Drawing.Point(140, 149)
        Me.fetchTimeoutNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.fetchTimeoutNumeric.Name = "fetchTimeoutNumeric"
        Me.fetchTimeoutNumeric.Size = New System.Drawing.Size(90, 20)
        Me.fetchTimeoutNumeric.TabIndex = 11
        Me.fetchTimeoutNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'voltageLevel1Numeric
        '
        Me.voltageLevel1Numeric.DecimalPlaces = 6
        Me.voltageLevel1Numeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLevel1Numeric.Location = New System.Drawing.Point(9, 18)
        Me.voltageLevel1Numeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLevel1Numeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLevel1Numeric.Name = "voltageLevel1Numeric"
        Me.voltageLevel1Numeric.Size = New System.Drawing.Size(90, 20)
        Me.voltageLevel1Numeric.TabIndex = 0
        Me.voltageLevel1Numeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'currentLimitRangeNumeric
        '
        Me.currentLimitRangeNumeric.DecimalPlaces = 6
        Me.currentLimitRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.currentLimitRangeNumeric.Location = New System.Drawing.Point(141, 83)
        Me.currentLimitRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.currentLimitRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.currentLimitRangeNumeric.Name = "currentLimitRangeNumeric"
        Me.currentLimitRangeNumeric.Size = New System.Drawing.Size(90, 20)
        Me.currentLimitRangeNumeric.TabIndex = 7
        Me.currentLimitRangeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'currentLimitNumeric
        '
        Me.currentLimitNumeric.DecimalPlaces = 6
        Me.currentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.currentLimitNumeric.Location = New System.Drawing.Point(140, 20)
        Me.currentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.currentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.currentLimitNumeric.Name = "currentLimitNumeric"
        Me.currentLimitNumeric.Size = New System.Drawing.Size(90, 20)
        Me.currentLimitNumeric.TabIndex = 3
        Me.currentLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(414, 255)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 6
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'resourceNameAndChannelNameGroupBox
        '
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.channelNameLabel)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.resourceNameAndChannelNameGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.resourceNameAndChannelNameGroupBox.Name = "resourceNameAndChannelNameGroupBox"
        Me.resourceNameAndChannelNameGroupBox.Size = New System.Drawing.Size(237, 73)
        Me.resourceNameAndChannelNameGroupBox.TabIndex = 0
        Me.resourceNameAndChannelNameGroupBox.TabStop = False
        Me.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name"
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(140, 45)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(91, 20)
        Me.channelNameTextBox.TabIndex = 15
        Me.channelNameTextBox.Text = "0"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(106, 18)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(125, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.Location = New System.Drawing.Point(6, 49)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.channelNameLabel.TabIndex = 2
        Me.channelNameLabel.Text = "Channel Name"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 22)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.currentLimitLabel)
        Me.configurationGroupBox.Controls.Add(Me.fetchTimeoutLabel)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayLabel)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitNumeric)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayNumeric)
        Me.configurationGroupBox.Controls.Add(Me.fetchTimeoutNumeric)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 101)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(237, 177)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'voltageLevel1GroupBox
        '
        Me.voltageLevel1GroupBox.Controls.Add(Me.voltageLevel1Numeric)
        Me.voltageLevel1GroupBox.Location = New System.Drawing.Point(265, 12)
        Me.voltageLevel1GroupBox.Name = "voltageLevel1GroupBox"
        Me.voltageLevel1GroupBox.Size = New System.Drawing.Size(179, 49)
        Me.voltageLevel1GroupBox.TabIndex = 2
        Me.voltageLevel1GroupBox.TabStop = False
        Me.voltageLevel1GroupBox.Text = "Voltage Level 1 (V)"
        '
        'voltageLevel2Numeric
        '
        Me.voltageLevel2Numeric.DecimalPlaces = 6
        Me.voltageLevel2Numeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLevel2Numeric.Location = New System.Drawing.Point(6, 18)
        Me.voltageLevel2Numeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLevel2Numeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLevel2Numeric.Name = "voltageLevel2Numeric"
        Me.voltageLevel2Numeric.Size = New System.Drawing.Size(90, 20)
        Me.voltageLevel2Numeric.TabIndex = 0
        Me.voltageLevel2Numeric.Value = New Decimal(New Integer() {4, 0, 0, 0})
        '
        'measurements1GroupBox
        '
        Me.measurements1GroupBox.Controls.Add(Me.voltage1MeasurementsTextBox)
        Me.measurements1GroupBox.Controls.Add(Me.current1MeasurementsTextBox)
        Me.measurements1GroupBox.Controls.Add(Me.measuredCurrent1Label)
        Me.measurements1GroupBox.Controls.Add(Me.inCompliance1ButtonLed)
        Me.measurements1GroupBox.Controls.Add(Me.measuredVoltage1Label)
        Me.measurements1GroupBox.Controls.Add(Me.inCompliance1Label)
        Me.measurements1GroupBox.Location = New System.Drawing.Point(265, 101)
        Me.measurements1GroupBox.Name = "measurements1GroupBox"
        Me.measurements1GroupBox.Size = New System.Drawing.Size(179, 140)
        Me.measurements1GroupBox.TabIndex = 4
        Me.measurements1GroupBox.TabStop = False
        Me.measurements1GroupBox.Text = "Measurements 1"
        '
        'voltage1MeasurementsTextBox
        '
        Me.voltage1MeasurementsTextBox.Location = New System.Drawing.Point(80, 18)
        Me.voltage1MeasurementsTextBox.Name = "voltage1MeasurementsTextBox"
        Me.voltage1MeasurementsTextBox.ReadOnly = True
        Me.voltage1MeasurementsTextBox.Size = New System.Drawing.Size(91, 20)
        Me.voltage1MeasurementsTextBox.TabIndex = 1
        Me.voltage1MeasurementsTextBox.Text = "0.000000E+000"
        '
        'current1MeasurementsTextBox
        '
        Me.current1MeasurementsTextBox.Location = New System.Drawing.Point(80, 45)
        Me.current1MeasurementsTextBox.Name = "current1MeasurementsTextBox"
        Me.current1MeasurementsTextBox.ReadOnly = True
        Me.current1MeasurementsTextBox.Size = New System.Drawing.Size(91, 20)
        Me.current1MeasurementsTextBox.TabIndex = 3
        Me.current1MeasurementsTextBox.Text = "0.000000E+000"
        '
        'measuredCurrent1Label
        '
        Me.measuredCurrent1Label.AutoSize = True
        Me.measuredCurrent1Label.Location = New System.Drawing.Point(6, 49)
        Me.measuredCurrent1Label.Name = "measuredCurrent1Label"
        Me.measuredCurrent1Label.Size = New System.Drawing.Size(66, 13)
        Me.measuredCurrent1Label.TabIndex = 2
        Me.measuredCurrent1Label.Text = "Current 1 (A)"
        '
        'inCompliance1ButtonLed
        '
        Me.inCompliance1ButtonLed.Enabled = False
        Me.inCompliance1ButtonLed.Location = New System.Drawing.Point(71, 101)
        Me.inCompliance1ButtonLed.Name = "inCompliance1ButtonLed"
        Me.inCompliance1ButtonLed.Size = New System.Drawing.Size(21, 21)
        Me.inCompliance1ButtonLed.TabIndex = 5
        Me.inCompliance1ButtonLed.UseVisualStyleBackColor = True
        '
        'measuredVoltage1Label
        '
        Me.measuredVoltage1Label.AutoSize = True
        Me.measuredVoltage1Label.Location = New System.Drawing.Point(6, 22)
        Me.measuredVoltage1Label.Name = "measuredVoltage1Label"
        Me.measuredVoltage1Label.Size = New System.Drawing.Size(68, 13)
        Me.measuredVoltage1Label.TabIndex = 0
        Me.measuredVoltage1Label.Text = "Voltage 1 (V)"
        '
        'inCompliance1Label
        '
        Me.inCompliance1Label.AutoSize = True
        Me.inCompliance1Label.Location = New System.Drawing.Point(40, 85)
        Me.inCompliance1Label.Name = "inCompliance1Label"
        Me.inCompliance1Label.Size = New System.Drawing.Size(83, 13)
        Me.inCompliance1Label.TabIndex = 4
        Me.inCompliance1Label.Text = "In Compliance 1"
        '
        'voltageLevel2GroupBox
        '
        Me.voltageLevel2GroupBox.Controls.Add(Me.voltageLevel2Numeric)
        Me.voltageLevel2GroupBox.Location = New System.Drawing.Point(460, 12)
        Me.voltageLevel2GroupBox.Name = "voltageLevel2GroupBox"
        Me.voltageLevel2GroupBox.Size = New System.Drawing.Size(179, 49)
        Me.voltageLevel2GroupBox.TabIndex = 3
        Me.voltageLevel2GroupBox.TabStop = False
        Me.voltageLevel2GroupBox.Text = "Voltage Level 2 (V)"
        '
        'measurements2GroupBox
        '
        Me.measurements2GroupBox.Controls.Add(Me.voltage2MeasurementsTextBox)
        Me.measurements2GroupBox.Controls.Add(Me.current2MeasurementsTextBox)
        Me.measurements2GroupBox.Controls.Add(Me.measuredCurrent2Label)
        Me.measurements2GroupBox.Controls.Add(Me.inCompliance2ButtonLed)
        Me.measurements2GroupBox.Controls.Add(Me.measuredVoltage2Label)
        Me.measurements2GroupBox.Controls.Add(Me.inCompliance2Label)
        Me.measurements2GroupBox.Location = New System.Drawing.Point(460, 101)
        Me.measurements2GroupBox.Name = "measurements2GroupBox"
        Me.measurements2GroupBox.Size = New System.Drawing.Size(179, 140)
        Me.measurements2GroupBox.TabIndex = 5
        Me.measurements2GroupBox.TabStop = False
        Me.measurements2GroupBox.Text = "Measurements 2"
        '
        'voltage2MeasurementsTextBox
        '
        Me.voltage2MeasurementsTextBox.Location = New System.Drawing.Point(80, 18)
        Me.voltage2MeasurementsTextBox.Name = "voltage2MeasurementsTextBox"
        Me.voltage2MeasurementsTextBox.ReadOnly = True
        Me.voltage2MeasurementsTextBox.Size = New System.Drawing.Size(91, 20)
        Me.voltage2MeasurementsTextBox.TabIndex = 1
        Me.voltage2MeasurementsTextBox.Text = "0.000000E+000"
        '
        'current2MeasurementsTextBox
        '
        Me.current2MeasurementsTextBox.Location = New System.Drawing.Point(80, 45)
        Me.current2MeasurementsTextBox.Name = "current2MeasurementsTextBox"
        Me.current2MeasurementsTextBox.ReadOnly = True
        Me.current2MeasurementsTextBox.Size = New System.Drawing.Size(91, 20)
        Me.current2MeasurementsTextBox.TabIndex = 3
        Me.current2MeasurementsTextBox.Text = "0.000000E+000"
        '
        'measuredCurrent2Label
        '
        Me.measuredCurrent2Label.AutoSize = True
        Me.measuredCurrent2Label.Location = New System.Drawing.Point(6, 49)
        Me.measuredCurrent2Label.Name = "measuredCurrent2Label"
        Me.measuredCurrent2Label.Size = New System.Drawing.Size(66, 13)
        Me.measuredCurrent2Label.TabIndex = 2
        Me.measuredCurrent2Label.Text = "Current 2 (A)"
        '
        'inCompliance2ButtonLed
        '
        Me.inCompliance2ButtonLed.Enabled = False
        Me.inCompliance2ButtonLed.Location = New System.Drawing.Point(71, 101)
        Me.inCompliance2ButtonLed.Name = "inCompliance2ButtonLed"
        Me.inCompliance2ButtonLed.Size = New System.Drawing.Size(21, 21)
        Me.inCompliance2ButtonLed.TabIndex = 5
        Me.inCompliance2ButtonLed.UseVisualStyleBackColor = True
        '
        'measuredVoltage2Label
        '
        Me.measuredVoltage2Label.AutoSize = True
        Me.measuredVoltage2Label.Location = New System.Drawing.Point(6, 22)
        Me.measuredVoltage2Label.Name = "measuredVoltage2Label"
        Me.measuredVoltage2Label.Size = New System.Drawing.Size(68, 13)
        Me.measuredVoltage2Label.TabIndex = 0
        Me.measuredVoltage2Label.Text = "Voltage 2 (V)"
        '
        'inCompliance2Label
        '
        Me.inCompliance2Label.AutoSize = True
        Me.inCompliance2Label.Location = New System.Drawing.Point(40, 85)
        Me.inCompliance2Label.Name = "inCompliance2Label"
        Me.inCompliance2Label.Size = New System.Drawing.Size(83, 13)
        Me.inCompliance2Label.TabIndex = 4
        Me.inCompliance2Label.Text = "In Compliance 2"
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(651, 288)
        Me.Controls.Add(Me.measurements2GroupBox)
        Me.Controls.Add(Me.voltageLevel2GroupBox)
        Me.Controls.Add(Me.measurements1GroupBox)
        Me.Controls.Add(Me.voltageLevel1GroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceNameAndChannelNameGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Hardware-Timed Single-Point"
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageLevelRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.fetchTimeoutNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageLevel1Numeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.currentLimitRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.currentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.resourceNameAndChannelNameGroupBox.ResumeLayout(False)
        Me.resourceNameAndChannelNameGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.voltageLevel1GroupBox.ResumeLayout(False)
        CType(Me.voltageLevel2Numeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurements1GroupBox.ResumeLayout(False)
        Me.measurements1GroupBox.PerformLayout()
        Me.voltageLevel2GroupBox.ResumeLayout(False)
        Me.measurements2GroupBox.ResumeLayout(False)
        Me.measurements2GroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private sourceDelayLabel As System.Windows.Forms.Label
    Private voltageLevelRangeLabel As System.Windows.Forms.Label
    Private fetchTimeoutLabel As System.Windows.Forms.Label
    Private currentLimitRangeLabel As System.Windows.Forms.Label
    Private currentLimitLabel As System.Windows.Forms.Label
    Private sourceDelayNumeric As System.Windows.Forms.NumericUpDown
    Private voltageLevelRangeNumeric As System.Windows.Forms.NumericUpDown
    Private fetchTimeoutNumeric As System.Windows.Forms.NumericUpDown
    Private voltageLevel1Numeric As System.Windows.Forms.NumericUpDown
    Private currentLimitRangeNumeric As System.Windows.Forms.NumericUpDown
    Private currentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents startButton As System.Windows.Forms.Button
    Private resourceNameAndChannelNameGroupBox As System.Windows.Forms.GroupBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private channelNameLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private voltageLevel1GroupBox As System.Windows.Forms.GroupBox
    Private voltageLevel2Numeric As System.Windows.Forms.NumericUpDown
    Private measurements1GroupBox As System.Windows.Forms.GroupBox
    Private voltage1MeasurementsTextBox As System.Windows.Forms.TextBox
    Private current1MeasurementsTextBox As System.Windows.Forms.TextBox
    Private measuredCurrent1Label As System.Windows.Forms.Label
    Private inCompliance1ButtonLed As System.Windows.Forms.Button
    Private measuredVoltage1Label As System.Windows.Forms.Label
    Private inCompliance1Label As System.Windows.Forms.Label
    Private voltageLevel2GroupBox As System.Windows.Forms.GroupBox
    Private measurements2GroupBox As System.Windows.Forms.GroupBox
    Private voltage2MeasurementsTextBox As System.Windows.Forms.TextBox
    Private current2MeasurementsTextBox As System.Windows.Forms.TextBox
    Private measuredCurrent2Label As System.Windows.Forms.Label
    Private inCompliance2ButtonLed As System.Windows.Forms.Button
    Private measuredVoltage2Label As System.Windows.Forms.Label
    Private inCompliance2Label As System.Windows.Forms.Label
    Private channelNameTextBox As System.Windows.Forms.TextBox

End Class
