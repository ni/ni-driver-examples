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
        Me.constantResistanceCurrentLimitLabel = New System.Windows.Forms.Label()
        Me.constantResistanceLevelLabel = New System.Windows.Forms.Label()
        Me.constantResistanceLevelRangeLabel = New System.Windows.Forms.Label()
        Me.constantPowerCurrentLimitLabel = New System.Windows.Forms.Label()
        Me.constantPowerLevelLabel = New System.Windows.Forms.Label()
        Me.constantPowerLevelRangeLabel = New System.Windows.Forms.Label()
        Me.sourceDelayLabel = New System.Windows.Forms.Label()
        Me.voltageMeasurementLabel = New System.Windows.Forms.Label()
        Me.currentMeasurementLabel = New System.Windows.Forms.Label()
        Me.resistanceMeasurementLabel = New System.Windows.Forms.Label()
        Me.powerMeasurementLabel = New System.Windows.Forms.Label()
        Me.inComplianceLabel = New System.Windows.Forms.Label()
        Me.constantResistanceCurrentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.constantResistanceLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.constantResistanceLevelRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.constantPowerCurrentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.constantPowerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.constantPowerLevelRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.sourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startButton = New System.Windows.Forms.Button()
        Me.resourceNameAndChannelNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.measurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.voltageMeasurementTextBox = New System.Windows.Forms.TextBox()
        Me.currentMeasurementTextBox = New System.Windows.Forms.TextBox()
        Me.resistanceMeasurementTextBox = New System.Windows.Forms.TextBox()
        Me.powerMeasurementTextBox = New System.Windows.Forms.TextBox()
        Me.inComplianceButtonLed = New System.Windows.Forms.Button()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.constantResistanceConstantPowerTabControl = New System.Windows.Forms.TabControl()
        Me.constantResistanceTab = New System.Windows.Forms.TabPage()
        Me.constantPowerTab = New System.Windows.Forms.TabPage()
        CType(Me.constantResistanceCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.constantResistanceLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.constantResistanceLevelRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.constantPowerCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.constantPowerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.constantPowerLevelRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resourceNameAndChannelNameGroupBox.SuspendLayout()
        Me.measurementsGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        Me.constantResistanceConstantPowerTabControl.SuspendLayout()
        Me.constantResistanceTab.SuspendLayout()
        Me.constantPowerTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'constantResistanceCurrentLimitLabel
        '
        Me.constantResistanceCurrentLimitLabel.AutoSize = True
        Me.constantResistanceCurrentLimitLabel.Location = New System.Drawing.Point(5, 63)
        Me.constantResistanceCurrentLimitLabel.Name = "constantResistanceCurrentLimitLabel"
        Me.constantResistanceCurrentLimitLabel.Size = New System.Drawing.Size(166, 13)
        Me.constantResistanceCurrentLimitLabel.TabIndex = 4
        Me.constantResistanceCurrentLimitLabel.Text = "Constant Resistance Current Limit"
        '
        'constantResistanceLevelLabel
        '
        Me.constantResistanceLevelLabel.AutoSize = True
        Me.constantResistanceLevelLabel.Location = New System.Drawing.Point(5, 10)
        Me.constantResistanceLevelLabel.Name = "constantResistanceLevelLabel"
        Me.constantResistanceLevelLabel.Size = New System.Drawing.Size(134, 13)
        Me.constantResistanceLevelLabel.TabIndex = 0
        Me.constantResistanceLevelLabel.Text = "Constant Resistance Level"
        '
        'constantResistanceLevelRangeLabel
        '
        Me.constantResistanceLevelRangeLabel.AutoSize = True
        Me.constantResistanceLevelRangeLabel.Location = New System.Drawing.Point(5, 36)
        Me.constantResistanceLevelRangeLabel.Name = "constantResistanceLevelRangeLabel"
        Me.constantResistanceLevelRangeLabel.Size = New System.Drawing.Size(169, 13)
        Me.constantResistanceLevelRangeLabel.TabIndex = 2
        Me.constantResistanceLevelRangeLabel.Text = "Constant Resistance Level Range"
        '
        'constantPowerCurrentLimitLabel
        '
        Me.constantPowerCurrentLimitLabel.AutoSize = True
        Me.constantPowerCurrentLimitLabel.Location = New System.Drawing.Point(5, 63)
        Me.constantPowerCurrentLimitLabel.Name = "constantPowerCurrentLimitLabel"
        Me.constantPowerCurrentLimitLabel.Size = New System.Drawing.Size(143, 13)
        Me.constantPowerCurrentLimitLabel.TabIndex = 4
        Me.constantPowerCurrentLimitLabel.Text = "Constant Power Current Limit"
        '
        'constantPowerLevelLabel
        '
        Me.constantPowerLevelLabel.AutoSize = True
        Me.constantPowerLevelLabel.Location = New System.Drawing.Point(5, 10)
        Me.constantPowerLevelLabel.Name = "constantPowerLevelLabel"
        Me.constantPowerLevelLabel.Size = New System.Drawing.Size(111, 13)
        Me.constantPowerLevelLabel.TabIndex = 0
        Me.constantPowerLevelLabel.Text = "Constant Power Level"
        '
        'constantPowerLevelRangeLabel
        '
        Me.constantPowerLevelRangeLabel.AutoSize = True
        Me.constantPowerLevelRangeLabel.Location = New System.Drawing.Point(5, 36)
        Me.constantPowerLevelRangeLabel.Name = "constantPowerLevelRangeLabel"
        Me.constantPowerLevelRangeLabel.Size = New System.Drawing.Size(146, 13)
        Me.constantPowerLevelRangeLabel.TabIndex = 2
        Me.constantPowerLevelRangeLabel.Text = "Constant Power Level Range"
        '
        'sourceDelayLabel
        '
        Me.sourceDelayLabel.AutoSize = True
        Me.sourceDelayLabel.Location = New System.Drawing.Point(15, 140)
        Me.sourceDelayLabel.Name = "sourceDelayLabel"
        Me.sourceDelayLabel.Size = New System.Drawing.Size(71, 13)
        Me.sourceDelayLabel.TabIndex = 1
        Me.sourceDelayLabel.Text = "Source Delay"
        '
        'voltageMeasurementLabel
        '
        Me.voltageMeasurementLabel.AutoSize = True
        Me.voltageMeasurementLabel.Location = New System.Drawing.Point(6, 23)
        Me.voltageMeasurementLabel.Name = "voltageMeasurementLabel"
        Me.voltageMeasurementLabel.Size = New System.Drawing.Size(43, 13)
        Me.voltageMeasurementLabel.TabIndex = 0
        Me.voltageMeasurementLabel.Text = "Voltage"
        '
        'currentMeasurementLabel
        '
        Me.currentMeasurementLabel.AutoSize = True
        Me.currentMeasurementLabel.Location = New System.Drawing.Point(6, 49)
        Me.currentMeasurementLabel.Name = "currentMeasurementLabel"
        Me.currentMeasurementLabel.Size = New System.Drawing.Size(41, 13)
        Me.currentMeasurementLabel.TabIndex = 2
        Me.currentMeasurementLabel.Text = "Current"
        '
        'resistanceMeasurementLabel
        '
        Me.resistanceMeasurementLabel.AutoSize = True
        Me.resistanceMeasurementLabel.Location = New System.Drawing.Point(6, 141)
        Me.resistanceMeasurementLabel.Name = "resistanceMeasurementLabel"
        Me.resistanceMeasurementLabel.Size = New System.Drawing.Size(60, 13)
        Me.resistanceMeasurementLabel.TabIndex = 6
        Me.resistanceMeasurementLabel.Text = "Resistance"
        '
        'powerMeasurementLabel
        '
        Me.powerMeasurementLabel.AutoSize = True
        Me.powerMeasurementLabel.Location = New System.Drawing.Point(6, 167)
        Me.powerMeasurementLabel.Name = "powerMeasurementLabel"
        Me.powerMeasurementLabel.Size = New System.Drawing.Size(37, 13)
        Me.powerMeasurementLabel.TabIndex = 8
        Me.powerMeasurementLabel.Text = "Power"
        '
        'inComplianceLabel
        '
        Me.inComplianceLabel.AutoSize = True
        Me.inComplianceLabel.Location = New System.Drawing.Point(41, 85)
        Me.inComplianceLabel.Name = "inComplianceLabel"
        Me.inComplianceLabel.Size = New System.Drawing.Size(135, 13)
        Me.inComplianceLabel.TabIndex = 4
        Me.inComplianceLabel.Text = "Compliance/Limit Reached"
        '
        'constantResistanceCurrentLimitNumeric
        '
        Me.constantResistanceCurrentLimitNumeric.DecimalPlaces = 6
        Me.constantResistanceCurrentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.constantResistanceCurrentLimitNumeric.Location = New System.Drawing.Point(193, 59)
        Me.constantResistanceCurrentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.constantResistanceCurrentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.constantResistanceCurrentLimitNumeric.Name = "constantResistanceCurrentLimitNumeric"
        Me.constantResistanceCurrentLimitNumeric.Size = New System.Drawing.Size(91, 20)
        Me.constantResistanceCurrentLimitNumeric.TabIndex = 5
        Me.constantResistanceCurrentLimitNumeric.Value = New Decimal(New Integer() {8, 0, 0, 65536})
        '
        'constantResistanceLevelNumeric
        '
        Me.constantResistanceLevelNumeric.DecimalPlaces = 6
        Me.constantResistanceLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.constantResistanceLevelNumeric.Location = New System.Drawing.Point(193, 6)
        Me.constantResistanceLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.constantResistanceLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.constantResistanceLevelNumeric.Name = "constantResistanceLevelNumeric"
        Me.constantResistanceLevelNumeric.Size = New System.Drawing.Size(91, 20)
        Me.constantResistanceLevelNumeric.TabIndex = 1
        Me.constantResistanceLevelNumeric.Value = New Decimal(New Integer() {15, 0, 0, 0})
        '
        'constantResistanceLevelRangeNumeric
        '
        Me.constantResistanceLevelRangeNumeric.DecimalPlaces = 6
        Me.constantResistanceLevelRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.constantResistanceLevelRangeNumeric.Location = New System.Drawing.Point(193, 32)
        Me.constantResistanceLevelRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.constantResistanceLevelRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.constantResistanceLevelRangeNumeric.Name = "constantResistanceLevelRangeNumeric"
        Me.constantResistanceLevelRangeNumeric.Size = New System.Drawing.Size(91, 20)
        Me.constantResistanceLevelRangeNumeric.TabIndex = 3
        Me.constantResistanceLevelRangeNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'constantPowerCurrentLimitNumeric
        '
        Me.constantPowerCurrentLimitNumeric.DecimalPlaces = 6
        Me.constantPowerCurrentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.constantPowerCurrentLimitNumeric.Location = New System.Drawing.Point(193, 59)
        Me.constantPowerCurrentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.constantPowerCurrentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.constantPowerCurrentLimitNumeric.Name = "constantPowerCurrentLimitNumeric"
        Me.constantPowerCurrentLimitNumeric.Size = New System.Drawing.Size(91, 20)
        Me.constantPowerCurrentLimitNumeric.TabIndex = 5
        Me.constantPowerCurrentLimitNumeric.Value = New Decimal(New Integer() {8, 0, 0, 65536})
        '
        'constantPowerLevelNumeric
        '
        Me.constantPowerLevelNumeric.DecimalPlaces = 6
        Me.constantPowerLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.constantPowerLevelNumeric.Location = New System.Drawing.Point(193, 6)
        Me.constantPowerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.constantPowerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.constantPowerLevelNumeric.Name = "constantPowerLevelNumeric"
        Me.constantPowerLevelNumeric.Size = New System.Drawing.Size(91, 20)
        Me.constantPowerLevelNumeric.TabIndex = 1
        Me.constantPowerLevelNumeric.Value = New Decimal(New Integer() {7, 0, 0, 0})
        '
        'constantPowerLevelRangeNumeric
        '
        Me.constantPowerLevelRangeNumeric.DecimalPlaces = 6
        Me.constantPowerLevelRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.constantPowerLevelRangeNumeric.Location = New System.Drawing.Point(193, 32)
        Me.constantPowerLevelRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.constantPowerLevelRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.constantPowerLevelRangeNumeric.Name = "constantPowerLevelRangeNumeric"
        Me.constantPowerLevelRangeNumeric.Size = New System.Drawing.Size(91, 20)
        Me.constantPowerLevelRangeNumeric.TabIndex = 3
        Me.constantPowerLevelRangeNumeric.Value = New Decimal(New Integer() {300, 0, 0, 0})
        '
        'sourceDelayNumeric
        '
        Me.sourceDelayNumeric.DecimalPlaces = 6
        Me.sourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.sourceDelayNumeric.Location = New System.Drawing.Point(203, 137)
        Me.sourceDelayNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.sourceDelayNumeric.Name = "sourceDelayNumeric"
        Me.sourceDelayNumeric.Size = New System.Drawing.Size(91, 20)
        Me.sourceDelayNumeric.TabIndex = 2
        Me.sourceDelayNumeric.Value = New Decimal(New Integer() {100, 0, 0, 131072})
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(398, 221)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 3
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        AddHandler Me.startButton.Click, AddressOf Me.startButton_Click
        '
        'resourceNameAndChannelNameGroupBox
        '
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.resourceNameAndChannelNameGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.resourceNameAndChannelNameGroupBox.Name = "resourceNameAndChannelNameGroupBox"
        Me.resourceNameAndChannelNameGroupBox.Size = New System.Drawing.Size(309, 48)
        Me.resourceNameAndChannelNameGroupBox.TabIndex = 0
        Me.resourceNameAndChannelNameGroupBox.TabStop = False
        Me.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(169, 18)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(125, 21)
        Me.resourceNameComboBox.TabIndex = 1
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
        'measurementsGroupBox
        '
        Me.measurementsGroupBox.Controls.Add(Me.voltageMeasurementTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.currentMeasurementTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.resistanceMeasurementTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.powerMeasurementTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.currentMeasurementLabel)
        Me.measurementsGroupBox.Controls.Add(Me.inComplianceButtonLed)
        Me.measurementsGroupBox.Controls.Add(Me.voltageMeasurementLabel)
        Me.measurementsGroupBox.Controls.Add(Me.resistanceMeasurementLabel)
        Me.measurementsGroupBox.Controls.Add(Me.powerMeasurementLabel)
        Me.measurementsGroupBox.Controls.Add(Me.inComplianceLabel)
        Me.measurementsGroupBox.Location = New System.Drawing.Point(329, 12)
        Me.measurementsGroupBox.Name = "measurementsGroupBox"
        Me.measurementsGroupBox.Size = New System.Drawing.Size(222, 193)
        Me.measurementsGroupBox.TabIndex = 2
        Me.measurementsGroupBox.TabStop = False
        Me.measurementsGroupBox.Text = "Measurements"
        '
        'voltageMeasurementTextBox
        '
        Me.voltageMeasurementTextBox.Location = New System.Drawing.Point(102, 19)
        Me.voltageMeasurementTextBox.Name = "voltageMeasurementTextBox"
        Me.voltageMeasurementTextBox.ReadOnly = True
        Me.voltageMeasurementTextBox.Size = New System.Drawing.Size(100, 20)
        Me.voltageMeasurementTextBox.TabIndex = 1
        Me.voltageMeasurementTextBox.Text = "0.000000E+000"
        '
        'currentMeasurementTextBox
        '
        Me.currentMeasurementTextBox.Location = New System.Drawing.Point(102, 45)
        Me.currentMeasurementTextBox.Name = "currentMeasurementTextBox"
        Me.currentMeasurementTextBox.ReadOnly = True
        Me.currentMeasurementTextBox.Size = New System.Drawing.Size(100, 20)
        Me.currentMeasurementTextBox.TabIndex = 3
        Me.currentMeasurementTextBox.Text = "0.000000E+000"
        '
        'resistanceMeasurementTextBox
        '
        Me.resistanceMeasurementTextBox.Location = New System.Drawing.Point(102, 137)
        Me.resistanceMeasurementTextBox.Name = "resistanceMeasurementTextBox"
        Me.resistanceMeasurementTextBox.ReadOnly = True
        Me.resistanceMeasurementTextBox.Size = New System.Drawing.Size(100, 20)
        Me.resistanceMeasurementTextBox.TabIndex = 7
        Me.resistanceMeasurementTextBox.Text = "0.000000E+000"
        '
        'powerMeasurementTextBox
        '
        Me.powerMeasurementTextBox.Location = New System.Drawing.Point(102, 163)
        Me.powerMeasurementTextBox.Name = "powerMeasurementTextBox"
        Me.powerMeasurementTextBox.ReadOnly = True
        Me.powerMeasurementTextBox.Size = New System.Drawing.Size(100, 20)
        Me.powerMeasurementTextBox.TabIndex = 9
        Me.powerMeasurementTextBox.Text = "0.000000E+000"
        '
        'inComplianceButtonLed
        '
        Me.inComplianceButtonLed.Enabled = False
        Me.inComplianceButtonLed.Location = New System.Drawing.Point(92, 101)
        Me.inComplianceButtonLed.Name = "inComplianceButtonLed"
        Me.inComplianceButtonLed.Size = New System.Drawing.Size(21, 21)
        Me.inComplianceButtonLed.TabIndex = 5
        Me.inComplianceButtonLed.UseVisualStyleBackColor = True
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.constantResistanceConstantPowerTabControl)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayNumeric)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 76)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(309, 168)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'constantResistanceConstantPowerTabControl
        '
        Me.constantResistanceConstantPowerTabControl.Controls.Add(Me.constantResistanceTab)
        Me.constantResistanceConstantPowerTabControl.Controls.Add(Me.constantPowerTab)
        Me.constantResistanceConstantPowerTabControl.Location = New System.Drawing.Point(6, 19)
        Me.constantResistanceConstantPowerTabControl.Name = "constantResistanceConstantPowerTabControl"
        Me.constantResistanceConstantPowerTabControl.SelectedIndex = 0
        Me.constantResistanceConstantPowerTabControl.Size = New System.Drawing.Size(298, 112)
        Me.constantResistanceConstantPowerTabControl.TabIndex = 0
        '
        'constantResistanceTab
        '
        Me.constantResistanceTab.Controls.Add(Me.constantResistanceCurrentLimitLabel)
        Me.constantResistanceTab.Controls.Add(Me.constantResistanceLevelLabel)
        Me.constantResistanceTab.Controls.Add(Me.constantResistanceLevelRangeLabel)
        Me.constantResistanceTab.Controls.Add(Me.constantResistanceCurrentLimitNumeric)
        Me.constantResistanceTab.Controls.Add(Me.constantResistanceLevelNumeric)
        Me.constantResistanceTab.Controls.Add(Me.constantResistanceLevelRangeNumeric)
        Me.constantResistanceTab.Location = New System.Drawing.Point(4, 22)
        Me.constantResistanceTab.Name = "constantResistanceTab"
        Me.constantResistanceTab.Padding = New System.Windows.Forms.Padding(3)
        Me.constantResistanceTab.Size = New System.Drawing.Size(290, 86)
        Me.constantResistanceTab.TabIndex = 0
        Me.constantResistanceTab.Text = "Constant Resistance"
        Me.constantResistanceTab.UseVisualStyleBackColor = True
        '
        'constantPowerTab
        '
        Me.constantPowerTab.Controls.Add(Me.constantPowerCurrentLimitLabel)
        Me.constantPowerTab.Controls.Add(Me.constantPowerLevelLabel)
        Me.constantPowerTab.Controls.Add(Me.constantPowerLevelRangeLabel)
        Me.constantPowerTab.Controls.Add(Me.constantPowerCurrentLimitNumeric)
        Me.constantPowerTab.Controls.Add(Me.constantPowerLevelNumeric)
        Me.constantPowerTab.Controls.Add(Me.constantPowerLevelRangeNumeric)
        Me.constantPowerTab.Location = New System.Drawing.Point(4, 22)
        Me.constantPowerTab.Name = "constantPowerTab"
        Me.constantPowerTab.Padding = New System.Windows.Forms.Padding(3)
        Me.constantPowerTab.Size = New System.Drawing.Size(290, 86)
        Me.constantPowerTab.TabIndex = 1
        Me.constantPowerTab.Text = "Constant Power"
        Me.constantPowerTab.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(558, 250)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.measurementsGroupBox)
        Me.Controls.Add(Me.resourceNameAndChannelNameGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Constant Resistance and Constant Power"
        CType(Me.constantResistanceCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.constantResistanceLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.constantResistanceLevelRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.constantPowerCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.constantPowerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.constantPowerLevelRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.resourceNameAndChannelNameGroupBox.ResumeLayout(False)
        Me.resourceNameAndChannelNameGroupBox.PerformLayout()
        Me.measurementsGroupBox.ResumeLayout(False)
        Me.measurementsGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.constantResistanceConstantPowerTabControl.ResumeLayout(False)
        Me.constantResistanceTab.ResumeLayout(False)
        Me.constantResistanceTab.PerformLayout()
        Me.constantPowerTab.ResumeLayout(False)
        Me.constantPowerTab.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private constantResistanceCurrentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private constantResistanceLevelNumeric As System.Windows.Forms.NumericUpDown
    Private constantResistanceLevelRangeNumeric As System.Windows.Forms.NumericUpDown
    Private constantPowerCurrentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private constantPowerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private constantPowerLevelRangeNumeric As System.Windows.Forms.NumericUpDown
    Private sourceDelayNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents startButton As System.Windows.Forms.Button
    Private resourceNameAndChannelNameGroupBox As System.Windows.Forms.GroupBox
    Private measurementsGroupBox As System.Windows.Forms.GroupBox
    Private inComplianceButtonLed As System.Windows.Forms.Button
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private voltageMeasurementTextBox As System.Windows.Forms.TextBox
    Private currentMeasurementTextBox As System.Windows.Forms.TextBox
    Private resistanceMeasurementTextBox As System.Windows.Forms.TextBox
    Private powerMeasurementTextBox As System.Windows.Forms.TextBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private constantResistanceCurrentLimitLabel As System.Windows.Forms.Label
    Private constantResistanceLevelLabel As System.Windows.Forms.Label
    Private constantResistanceLevelRangeLabel As System.Windows.Forms.Label
    Private constantPowerCurrentLimitLabel As System.Windows.Forms.Label
    Private constantPowerLevelLabel As System.Windows.Forms.Label
    Private constantPowerLevelRangeLabel As System.Windows.Forms.Label
    Private sourceDelayLabel As System.Windows.Forms.Label
    Private voltageMeasurementLabel As System.Windows.Forms.Label
    Private currentMeasurementLabel As System.Windows.Forms.Label
    Private resistanceMeasurementLabel As System.Windows.Forms.Label
    Private powerMeasurementLabel As System.Windows.Forms.Label
    Private inComplianceLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private constantResistanceConstantPowerTabControl As System.Windows.Forms.TabControl
    Private constantResistanceTab As System.Windows.Forms.TabPage
    Private constantPowerTab As System.Windows.Forms.TabPage

End Class
