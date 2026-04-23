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
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.currentLevelLabel = New System.Windows.Forms.Label()
        Me.currentLevelRangeLabel = New System.Windows.Forms.Label()
        Me.voltageLimitRangeLabel = New System.Windows.Forms.Label()
        Me.sourceDelayLabel = New System.Windows.Forms.Label()
        Me.outputShortedLabel = New System.Windows.Forms.Label()
        Me.conductionVoltageModeLabel = New System.Windows.Forms.Label()
        Me.conductionVoltageOnThresholdLabel = New System.Windows.Forms.Label()
        Me.conductionVoltageOffThresholdLabel = New System.Windows.Forms.Label()
        Me.currentLevelRisingSlewRateLabel = New System.Windows.Forms.Label()
        Me.currentLevelFallingSlewRateLabel = New System.Windows.Forms.Label()
        Me.currentLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.currentLevelRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.voltageLimitRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.sourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.outputShortedCheckBox = New System.Windows.Forms.CheckBox()
        Me.conductionVoltageModeComboBox = New System.Windows.Forms.ComboBox()
        Me.conductionVoltageOnThresholdNumeric = New System.Windows.Forms.NumericUpDown()
        Me.conductionVoltageOffThresholdNumeric = New System.Windows.Forms.NumericUpDown()
        Me.currentLevelRisingSlewRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.currentLevelFallingSlewRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.voltageMeasurementLabel = New System.Windows.Forms.Label()
        Me.currentMeasurementLabel = New System.Windows.Forms.Label()
        Me.inComplianceLabel = New System.Windows.Forms.Label()
        Me.measurementResultGroupBox = New System.Windows.Forms.GroupBox()
        Me.currentMeasurementTextBox = New System.Windows.Forms.TextBox()
        Me.voltageMeasurementTextBox = New System.Windows.Forms.TextBox()
        Me.inComplianceButtonLed = New System.Windows.Forms.Button()
        Me.startButton = New System.Windows.Forms.Button()
        Me.resourceNameAndChannelNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.currentLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.currentLevelRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageLimitRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.conductionVoltageOnThresholdNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.conductionVoltageOffThresholdNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.currentLevelRisingSlewRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.currentLevelFallingSlewRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementResultGroupBox.SuspendLayout()
        Me.resourceNameAndChannelNameGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.currentLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.currentLevelRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltageLimitRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayLabel)
        Me.configurationGroupBox.Controls.Add(Me.outputShortedLabel)
        Me.configurationGroupBox.Controls.Add(Me.conductionVoltageModeLabel)
        Me.configurationGroupBox.Controls.Add(Me.conductionVoltageOnThresholdLabel)
        Me.configurationGroupBox.Controls.Add(Me.conductionVoltageOffThresholdLabel)
        Me.configurationGroupBox.Controls.Add(Me.currentLevelRisingSlewRateLabel)
        Me.configurationGroupBox.Controls.Add(Me.currentLevelFallingSlewRateLabel)
        Me.configurationGroupBox.Controls.Add(Me.currentLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLevelRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.voltageLimitRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayNumeric)
        Me.configurationGroupBox.Controls.Add(Me.outputShortedCheckBox)
        Me.configurationGroupBox.Controls.Add(Me.conductionVoltageModeComboBox)
        Me.configurationGroupBox.Controls.Add(Me.conductionVoltageOnThresholdNumeric)
        Me.configurationGroupBox.Controls.Add(Me.conductionVoltageOffThresholdNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLevelRisingSlewRateNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLevelFallingSlewRateNumeric)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 97)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(360, 282)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'currentLevelLabel
        '
        Me.currentLevelLabel.AutoSize = True
        Me.currentLevelLabel.Location = New System.Drawing.Point(8, 23)
        Me.currentLevelLabel.Name = "currentLevelLabel"
        Me.currentLevelLabel.Size = New System.Drawing.Size(70, 13)
        Me.currentLevelLabel.TabIndex = 0
        Me.currentLevelLabel.Text = "Current Level"
        '
        'currentLevelRangeLabel
        '
        Me.currentLevelRangeLabel.AutoSize = True
        Me.currentLevelRangeLabel.Location = New System.Drawing.Point(8, 77)
        Me.currentLevelRangeLabel.Name = "currentLevelRangeLabel"
        Me.currentLevelRangeLabel.Size = New System.Drawing.Size(105, 13)
        Me.currentLevelRangeLabel.TabIndex = 2
        Me.currentLevelRangeLabel.Text = "Current Level Range"
        '
        'voltageLimitRangeLabel
        '
        Me.voltageLimitRangeLabel.AutoSize = True
        Me.voltageLimitRangeLabel.Location = New System.Drawing.Point(8, 130)
        Me.voltageLimitRangeLabel.Name = "voltageLimitRangeLabel"
        Me.voltageLimitRangeLabel.Size = New System.Drawing.Size(102, 13)
        Me.voltageLimitRangeLabel.TabIndex = 4
        Me.voltageLimitRangeLabel.Text = "Voltage Limit Range"
        '
        'sourceDelayLabel
        '
        Me.sourceDelayLabel.AutoSize = True
        Me.sourceDelayLabel.Location = New System.Drawing.Point(8, 183)
        Me.sourceDelayLabel.Name = "sourceDelayLabel"
        Me.sourceDelayLabel.Size = New System.Drawing.Size(71, 13)
        Me.sourceDelayLabel.TabIndex = 6
        Me.sourceDelayLabel.Text = "Source Delay"
        '
        'outputShortedLabel
        '
        Me.outputShortedLabel.AutoSize = True
        Me.outputShortedLabel.Location = New System.Drawing.Point(8, 237)
        Me.outputShortedLabel.Name = "outputShortedLabel"
        Me.outputShortedLabel.Size = New System.Drawing.Size(79, 13)
        Me.outputShortedLabel.TabIndex = 8
        Me.outputShortedLabel.Text = "Output Shorted"
        '
        'conductionVoltageModeLabel
        '
        Me.conductionVoltageModeLabel.AutoSize = True
        Me.conductionVoltageModeLabel.Location = New System.Drawing.Point(162, 22)
        Me.conductionVoltageModeLabel.Name = "conductionVoltageModeLabel"
        Me.conductionVoltageModeLabel.Size = New System.Drawing.Size(130, 13)
        Me.conductionVoltageModeLabel.TabIndex = 10
        Me.conductionVoltageModeLabel.Text = "Conduction Voltage Mode"
        '
        'conductionVoltageOnThresholdLabel
        '
        Me.conductionVoltageOnThresholdLabel.AutoSize = True
        Me.conductionVoltageOnThresholdLabel.Location = New System.Drawing.Point(162, 76)
        Me.conductionVoltageOnThresholdLabel.Name = "conductionVoltageOnThresholdLabel"
        Me.conductionVoltageOnThresholdLabel.Size = New System.Drawing.Size(167, 13)
        Me.conductionVoltageOnThresholdLabel.TabIndex = 12
        Me.conductionVoltageOnThresholdLabel.Text = "Conduction Voltage On Threshold"
        '
        'conductionVoltageOffThresholdLabel
        '
        Me.conductionVoltageOffThresholdLabel.AutoSize = True
        Me.conductionVoltageOffThresholdLabel.Location = New System.Drawing.Point(162, 130)
        Me.conductionVoltageOffThresholdLabel.Name = "conductionVoltageOffThresholdLabel"
        Me.conductionVoltageOffThresholdLabel.Size = New System.Drawing.Size(167, 13)
        Me.conductionVoltageOffThresholdLabel.TabIndex = 14
        Me.conductionVoltageOffThresholdLabel.Text = "Conduction Voltage Off Threshold"
        '
        'currentLevelRisingSlewRateLabel
        '
        Me.currentLevelRisingSlewRateLabel.AutoSize = True
        Me.currentLevelRisingSlewRateLabel.Location = New System.Drawing.Point(162, 183)
        Me.currentLevelRisingSlewRateLabel.Name = "currentLevelRisingSlewRateLabel"
        Me.currentLevelRisingSlewRateLabel.Size = New System.Drawing.Size(154, 13)
        Me.currentLevelRisingSlewRateLabel.TabIndex = 16
        Me.currentLevelRisingSlewRateLabel.Text = "Current Level Rising Slew Rate"
        '
        'currentLevelFallingSlewRateLabel
        '
        Me.currentLevelFallingSlewRateLabel.AutoSize = True
        Me.currentLevelFallingSlewRateLabel.Location = New System.Drawing.Point(162, 237)
        Me.currentLevelFallingSlewRateLabel.Name = "currentLevelFallingSlewRateLabel"
        Me.currentLevelFallingSlewRateLabel.Size = New System.Drawing.Size(155, 13)
        Me.currentLevelFallingSlewRateLabel.TabIndex = 18
        Me.currentLevelFallingSlewRateLabel.Text = "Current Level Falling Slew Rate"
        '
        'currentLevelNumeric
        '
        Me.currentLevelNumeric.DecimalPlaces = 6
        Me.currentLevelNumeric.Location = New System.Drawing.Point(8, 40)
        Me.currentLevelNumeric.Maximum = New Decimal(New Integer() {40, 0, 0, 0})
        Me.currentLevelNumeric.Name = "currentLevelNumeric"
        Me.currentLevelNumeric.Size = New System.Drawing.Size(125, 20)
        Me.currentLevelNumeric.TabIndex = 1
        Me.currentLevelNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 196608})
        '
        'currentLevelRangeNumeric
        '
        Me.currentLevelRangeNumeric.DecimalPlaces = 6
        Me.currentLevelRangeNumeric.Location = New System.Drawing.Point(8, 94)
        Me.currentLevelRangeNumeric.Maximum = New Decimal(New Integer() {40, 0, 0, 0})
        Me.currentLevelRangeNumeric.Name = "currentLevelRangeNumeric"
        Me.currentLevelRangeNumeric.Size = New System.Drawing.Size(125, 20)
        Me.currentLevelRangeNumeric.TabIndex = 3
        Me.currentLevelRangeNumeric.Value = New Decimal(New Integer() {40, 0, 0, 0})
        '
        'voltageLimitRangeNumeric
        '
        Me.voltageLimitRangeNumeric.DecimalPlaces = 6
        Me.voltageLimitRangeNumeric.Location = New System.Drawing.Point(8, 147)
        Me.voltageLimitRangeNumeric.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
        Me.voltageLimitRangeNumeric.Name = "voltageLimitRangeNumeric"
        Me.voltageLimitRangeNumeric.Size = New System.Drawing.Size(125, 20)
        Me.voltageLimitRangeNumeric.TabIndex = 5
        Me.voltageLimitRangeNumeric.Value = New Decimal(New Integer() {60, 0, 0, 0})
        '
        'sourceDelayNumeric
        '
        Me.sourceDelayNumeric.DecimalPlaces = 6
        Me.sourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.sourceDelayNumeric.Location = New System.Drawing.Point(8, 200)
        Me.sourceDelayNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.sourceDelayNumeric.Name = "sourceDelayNumeric"
        Me.sourceDelayNumeric.Size = New System.Drawing.Size(125, 20)
        Me.sourceDelayNumeric.TabIndex = 7
        Me.sourceDelayNumeric.Value = New Decimal(New Integer() {50000, 0, 0, 327680})
        '
        'outputShortedCheckBox
        '
        Me.outputShortedCheckBox.Location = New System.Drawing.Point(8, 254)
        Me.outputShortedCheckBox.Name = "outputShortedCheckBox"
        Me.outputShortedCheckBox.Size = New System.Drawing.Size(125, 21)
        Me.outputShortedCheckBox.TabIndex = 9
        '
        'conductionVoltageModeComboBox
        '
        Me.conductionVoltageModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.conductionVoltageModeComboBox.FormattingEnabled = True
        Me.conductionVoltageModeComboBox.Location = New System.Drawing.Point(162, 39)
        Me.conductionVoltageModeComboBox.Name = "conductionVoltageModeComboBox"
        Me.conductionVoltageModeComboBox.Size = New System.Drawing.Size(189, 21)
        Me.conductionVoltageModeComboBox.TabIndex = 11
        '
        'conductionVoltageOnThresholdNumeric
        '
        Me.conductionVoltageOnThresholdNumeric.DecimalPlaces = 6
        Me.conductionVoltageOnThresholdNumeric.Location = New System.Drawing.Point(162, 94)
        Me.conductionVoltageOnThresholdNumeric.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
        Me.conductionVoltageOnThresholdNumeric.Name = "conductionVoltageOnThresholdNumeric"
        Me.conductionVoltageOnThresholdNumeric.Size = New System.Drawing.Size(189, 20)
        Me.conductionVoltageOnThresholdNumeric.TabIndex = 13
        Me.conductionVoltageOnThresholdNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'conductionVoltageOffThresholdNumeric
        '
        Me.conductionVoltageOffThresholdNumeric.DecimalPlaces = 6
        Me.conductionVoltageOffThresholdNumeric.Location = New System.Drawing.Point(162, 147)
        Me.conductionVoltageOffThresholdNumeric.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
        Me.conductionVoltageOffThresholdNumeric.Name = "conductionVoltageOffThresholdNumeric"
        Me.conductionVoltageOffThresholdNumeric.Size = New System.Drawing.Size(189, 20)
        Me.conductionVoltageOffThresholdNumeric.TabIndex = 15
        '
        'currentLevelRisingSlewRateNumeric
        '
        Me.currentLevelRisingSlewRateNumeric.DecimalPlaces = 6
        Me.currentLevelRisingSlewRateNumeric.Location = New System.Drawing.Point(162, 200)
        Me.currentLevelRisingSlewRateNumeric.Maximum = New Decimal(New Integer() {24, 0, 0, 0})
        Me.currentLevelRisingSlewRateNumeric.Name = "currentLevelRisingSlewRateNumeric"
        Me.currentLevelRisingSlewRateNumeric.Size = New System.Drawing.Size(189, 20)
        Me.currentLevelRisingSlewRateNumeric.TabIndex = 17
        Me.currentLevelRisingSlewRateNumeric.Value = New Decimal(New Integer() {2400, 0, 0, 131072})
        '
        'currentLevelFallingSlewRateNumeric
        '
        Me.currentLevelFallingSlewRateNumeric.DecimalPlaces = 6
        Me.currentLevelFallingSlewRateNumeric.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.currentLevelFallingSlewRateNumeric.Location = New System.Drawing.Point(162, 254)
        Me.currentLevelFallingSlewRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.currentLevelFallingSlewRateNumeric.Name = "currentLevelFallingSlewRateNumeric"
        Me.currentLevelFallingSlewRateNumeric.Size = New System.Drawing.Size(189, 20)
        Me.currentLevelFallingSlewRateNumeric.TabIndex = 19
        Me.currentLevelFallingSlewRateNumeric.Value = New Decimal(New Integer() {24000, 0, 0, 196608})
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(8, 22)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'voltageMeasurementLabel
        '
        Me.voltageMeasurementLabel.AutoSize = True
        Me.voltageMeasurementLabel.Location = New System.Drawing.Point(9, 77)
        Me.voltageMeasurementLabel.Name = "voltageMeasurementLabel"
        Me.voltageMeasurementLabel.Size = New System.Drawing.Size(110, 13)
        Me.voltageMeasurementLabel.TabIndex = 2
        Me.voltageMeasurementLabel.Text = "Voltage"
        '
        'currentMeasurementLabel
        '
        Me.currentMeasurementLabel.AutoSize = True
        Me.currentMeasurementLabel.Location = New System.Drawing.Point(9, 22)
        Me.currentMeasurementLabel.Name = "currentMeasurementLabel"
        Me.currentMeasurementLabel.Size = New System.Drawing.Size(108, 13)
        Me.currentMeasurementLabel.TabIndex = 0
        Me.currentMeasurementLabel.Text = "Current"
        '
        'inComplianceLabel
        '
        Me.inComplianceLabel.AutoSize = True
        Me.inComplianceLabel.Location = New System.Drawing.Point(9, 131)
        Me.inComplianceLabel.Name = "inComplianceLabel"
        Me.inComplianceLabel.Size = New System.Drawing.Size(135, 13)
        Me.inComplianceLabel.TabIndex = 4
        Me.inComplianceLabel.Text = "Compliance/Limit Reached"
        '
        'measurementResultGroupBox
        '
        Me.measurementResultGroupBox.Controls.Add(Me.voltageMeasurementLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.currentMeasurementLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.inComplianceLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.currentMeasurementTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.voltageMeasurementTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.inComplianceButtonLed)
        Me.measurementResultGroupBox.Location = New System.Drawing.Point(388, 12)
        Me.measurementResultGroupBox.Name = "measurementResultGroupBox"
        Me.measurementResultGroupBox.Size = New System.Drawing.Size(210, 184)
        Me.measurementResultGroupBox.TabIndex = 2
        Me.measurementResultGroupBox.TabStop = False
        Me.measurementResultGroupBox.Text = "Measurements"
        '
        'currentMeasurementTextBox
        '
        Me.currentMeasurementTextBox.Location = New System.Drawing.Point(9, 40)
        Me.currentMeasurementTextBox.Name = "currentMeasurementTextBox"
        Me.currentMeasurementTextBox.ReadOnly = True
        Me.currentMeasurementTextBox.Size = New System.Drawing.Size(187, 20)
        Me.currentMeasurementTextBox.TabIndex = 1
        Me.currentMeasurementTextBox.Text = "0.000000E+000"
        '
        'voltageMeasurementTextBox
        '
        Me.voltageMeasurementTextBox.Location = New System.Drawing.Point(9, 94)
        Me.voltageMeasurementTextBox.Name = "voltageMeasurementTextBox"
        Me.voltageMeasurementTextBox.ReadOnly = True
        Me.voltageMeasurementTextBox.Size = New System.Drawing.Size(187, 20)
        Me.voltageMeasurementTextBox.TabIndex = 3
        Me.voltageMeasurementTextBox.Text = "0.000000E+000"
        '
        'inComplianceButtonLed
        '
        Me.inComplianceButtonLed.Enabled = False
        Me.inComplianceButtonLed.Location = New System.Drawing.Point(9, 149)
        Me.inComplianceButtonLed.Name = "inComplianceButtonLed"
        Me.inComplianceButtonLed.Size = New System.Drawing.Size(21, 22)
        Me.inComplianceButtonLed.TabIndex = 5
        Me.inComplianceButtonLed.UseVisualStyleBackColor = True
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(457, 356)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 3
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'resourceNameAndChannelNameGroupBox
        '
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceNameAndChannelNameGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.resourceNameAndChannelNameGroupBox.Name = "resourceNameAndChannelNameGroupBox"
        Me.resourceNameAndChannelNameGroupBox.Size = New System.Drawing.Size(360, 71)
        Me.resourceNameAndChannelNameGroupBox.TabIndex = 0
        Me.resourceNameAndChannelNameGroupBox.TabStop = False
        Me.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(8, 39)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(343, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(610, 386)
        Me.Controls.Add(Me.resourceNameAndChannelNameGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.measurementResultGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Sink DC Current into Electronic Load"
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.currentLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.currentLevelRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageLimitRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.conductionVoltageOnThresholdNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.conductionVoltageOffThresholdNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.currentLevelRisingSlewRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.currentLevelFallingSlewRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementResultGroupBox.ResumeLayout(False)
        Me.measurementResultGroupBox.PerformLayout()
        Me.resourceNameAndChannelNameGroupBox.ResumeLayout(False)
        Me.resourceNameAndChannelNameGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private resourceNameLabel As System.Windows.Forms.Label
    Private currentLevelLabel As System.Windows.Forms.Label
    Private currentLevelRangeLabel As System.Windows.Forms.Label
    Private voltageLimitRangeLabel As System.Windows.Forms.Label
    Private sourceDelayLabel As System.Windows.Forms.Label
    Private outputShortedLabel As System.Windows.Forms.Label
    Private conductionVoltageModeLabel As System.Windows.Forms.Label
    Private conductionVoltageOnThresholdLabel As System.Windows.Forms.Label
    Private conductionVoltageOffThresholdLabel As System.Windows.Forms.Label
    Private currentLevelRisingSlewRateLabel As System.Windows.Forms.Label
    Private currentLevelFallingSlewRateLabel As System.Windows.Forms.Label
    Private voltageMeasurementLabel As System.Windows.Forms.Label
    Private currentMeasurementLabel As System.Windows.Forms.Label
    Private inComplianceLabel As System.Windows.Forms.Label
    Private currentLevelNumeric As System.Windows.Forms.NumericUpDown
    Private currentLevelRangeNumeric As System.Windows.Forms.NumericUpDown
    Private voltageLimitRangeNumeric As System.Windows.Forms.NumericUpDown
    Private sourceDelayNumeric As System.Windows.Forms.NumericUpDown
    Private conductionVoltageModeComboBox As System.Windows.Forms.ComboBox
    Private conductionVoltageOnThresholdNumeric As System.Windows.Forms.NumericUpDown
    Private conductionVoltageOffThresholdNumeric As System.Windows.Forms.NumericUpDown
    Private currentLevelRisingSlewRateNumeric As System.Windows.Forms.NumericUpDown
    Private currentLevelFallingSlewRateNumeric As System.Windows.Forms.NumericUpDown
    Private measurementResultGroupBox As System.Windows.Forms.GroupBox
    Private currentMeasurementTextBox As System.Windows.Forms.TextBox
    Private voltageMeasurementTextBox As System.Windows.Forms.TextBox
    Private inComplianceButtonLed As System.Windows.Forms.Button
    Private WithEvents startButton As System.Windows.Forms.Button
    Private outputShortedCheckBox As System.Windows.Forms.CheckBox
    Private WithEvents resourceNameAndChannelNameGroupBox As Windows.Forms.GroupBox
    Private WithEvents resourceNameComboBox As Windows.Forms.ComboBox
End Class
