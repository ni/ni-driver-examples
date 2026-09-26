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
        Me.currentLimitLabel = New System.Windows.Forms.Label()
        Me.currentLimitRangeLabel = New System.Windows.Forms.Label()
        Me.voltageLevelLabel = New System.Windows.Forms.Label()
        Me.voltageLevelRangeLabel = New System.Windows.Forms.Label()
        Me.sourceDelayLabel = New System.Windows.Forms.Label()
        Me.voltageMeasurementLabel = New System.Windows.Forms.Label()
        Me.currentMeasurementLabel = New System.Windows.Forms.Label()
        Me.inComplianceLabel = New System.Windows.Forms.Label()
        Me.currentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.currentLimitRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.voltageLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.voltageLevelRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.outputResistanceLabel = New System.Windows.Forms.Label()
        Me.outputResistanceNumeric = New System.Windows.Forms.NumericUpDown()
        Me.sourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startButton = New System.Windows.Forms.Button()
        Me.resourceNameAndChannelNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.measurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.voltageMeasurementTextBox = New System.Windows.Forms.TextBox()
        Me.currentMeasurementTextBox = New System.Windows.Forms.TextBox()
        Me.inComplianceButtonLed = New System.Windows.Forms.Button()
        Me.outputShortedLabel = New System.Windows.Forms.Label()
        Me.outputShortedCheckBox = New System.Windows.Forms.CheckBox()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.currentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.currentLimitRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageLevelRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.outputResistanceNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resourceNameAndChannelNameGroupBox.SuspendLayout()
        Me.measurementsGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'currentLimitLabel
        '
        Me.currentLimitLabel.AutoSize = True
        Me.currentLimitLabel.Location = New System.Drawing.Point(6, 76)
        Me.currentLimitLabel.Name = "currentLimitLabel"
        Me.currentLimitLabel.Size = New System.Drawing.Size(95, 15)
        Me.currentLimitLabel.TabIndex = 4
        Me.currentLimitLabel.Text = "Current Limit (A)"
        '
        'currentLimitRangeLabel
        '
        Me.currentLimitRangeLabel.AutoSize = True
        Me.currentLimitRangeLabel.Location = New System.Drawing.Point(6, 102)
        Me.currentLimitRangeLabel.Name = "currentLimitRangeLabel"
        Me.currentLimitRangeLabel.Size = New System.Drawing.Size(135, 15)
        Me.currentLimitRangeLabel.TabIndex = 6
        Me.currentLimitRangeLabel.Text = "Current Limit Range (A)"
        '
        'voltageLevelLabel
        '
        Me.voltageLevelLabel.AutoSize = True
        Me.voltageLevelLabel.Location = New System.Drawing.Point(6, 23)
        Me.voltageLevelLabel.Name = "voltageLevelLabel"
        Me.voltageLevelLabel.Size = New System.Drawing.Size(98, 15)
        Me.voltageLevelLabel.TabIndex = 0
        Me.voltageLevelLabel.Text = "Voltage Level (V)"
        '
        'voltageLevelRangeLabel
        '
        Me.voltageLevelRangeLabel.AutoSize = True
        Me.voltageLevelRangeLabel.Location = New System.Drawing.Point(6, 49)
        Me.voltageLevelRangeLabel.Name = "voltageLevelRangeLabel"
        Me.voltageLevelRangeLabel.Size = New System.Drawing.Size(138, 15)
        Me.voltageLevelRangeLabel.TabIndex = 2
        Me.voltageLevelRangeLabel.Text = "Voltage Level Range (V)"
        '
        'sourceDelayLabel
        '
        Me.sourceDelayLabel.AutoSize = True
        Me.sourceDelayLabel.Location = New System.Drawing.Point(6, 156)
        Me.sourceDelayLabel.Name = "sourceDelayLabel"
        Me.sourceDelayLabel.Size = New System.Drawing.Size(97, 15)
        Me.sourceDelayLabel.TabIndex = 10
        Me.sourceDelayLabel.Text = "Source Delay (s)"
        '
        'voltageMeasurementLabel
        '
        Me.voltageMeasurementLabel.AutoSize = True
        Me.voltageMeasurementLabel.Location = New System.Drawing.Point(6, 23)
        Me.voltageMeasurementLabel.Name = "voltageMeasurementLabel"
        Me.voltageMeasurementLabel.Size = New System.Drawing.Size(66, 15)
        Me.voltageMeasurementLabel.TabIndex = 0
        Me.voltageMeasurementLabel.Text = "Voltage (V)"
        '
        'currentMeasurementLabel
        '
        Me.currentMeasurementLabel.AutoSize = True
        Me.currentMeasurementLabel.Location = New System.Drawing.Point(6, 49)
        Me.currentMeasurementLabel.Name = "currentMeasurementLabel"
        Me.currentMeasurementLabel.Size = New System.Drawing.Size(65, 15)
        Me.currentMeasurementLabel.TabIndex = 2
        Me.currentMeasurementLabel.Text = "Current (A)"
        '
        'inComplianceLabel
        '
        Me.inComplianceLabel.AutoSize = True
        Me.inComplianceLabel.Location = New System.Drawing.Point(30, 85)
        Me.inComplianceLabel.Name = "inComplianceLabel"
        Me.inComplianceLabel.Size = New System.Drawing.Size(156, 15)
        Me.inComplianceLabel.TabIndex = 4
        Me.inComplianceLabel.Text = "Compliance/Limit Reached"
        '
        'currentLimitNumeric
        '
        Me.currentLimitNumeric.DecimalPlaces = 6
        Me.currentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.currentLimitNumeric.Location = New System.Drawing.Point(160, 72)
        Me.currentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.currentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.currentLimitNumeric.Name = "currentLimitNumeric"
        Me.currentLimitNumeric.Size = New System.Drawing.Size(91, 20)
        Me.currentLimitNumeric.TabIndex = 5
        Me.currentLimitNumeric.Value = New Decimal(New Integer() {4, 0, 0, 0})
        '
        'currentLimitRangeNumeric
        '
        Me.currentLimitRangeNumeric.DecimalPlaces = 6
        Me.currentLimitRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.currentLimitRangeNumeric.Location = New System.Drawing.Point(160, 98)
        Me.currentLimitRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.currentLimitRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.currentLimitRangeNumeric.Name = "currentLimitRangeNumeric"
        Me.currentLimitRangeNumeric.Size = New System.Drawing.Size(91, 20)
        Me.currentLimitRangeNumeric.TabIndex = 7
        Me.currentLimitRangeNumeric.Value = New Decimal(New Integer() {4, 0, 0, 0})
        '
        'voltageLevelNumeric
        '
        Me.voltageLevelNumeric.DecimalPlaces = 6
        Me.voltageLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLevelNumeric.Location = New System.Drawing.Point(160, 19)
        Me.voltageLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLevelNumeric.Name = "voltageLevelNumeric"
        Me.voltageLevelNumeric.Size = New System.Drawing.Size(91, 20)
        Me.voltageLevelNumeric.TabIndex = 1
        Me.voltageLevelNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'voltageLevelRangeNumeric
        '
        Me.voltageLevelRangeNumeric.DecimalPlaces = 6
        Me.voltageLevelRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLevelRangeNumeric.Location = New System.Drawing.Point(160, 45)
        Me.voltageLevelRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLevelRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLevelRangeNumeric.Name = "voltageLevelRangeNumeric"
        Me.voltageLevelRangeNumeric.Size = New System.Drawing.Size(91, 20)
        Me.voltageLevelRangeNumeric.TabIndex = 3
        Me.voltageLevelRangeNumeric.Value = New Decimal(New Integer() {6, 0, 0, 0})
        '
        'outputResistanceLabel
        '
        Me.outputResistanceLabel.AutoSize = True
        Me.outputResistanceLabel.Location = New System.Drawing.Point(6, 130)
        Me.outputResistanceLabel.Name = "outputResistanceLabel"
        Me.outputResistanceLabel.Size = New System.Drawing.Size(151, 15)
        Me.outputResistanceLabel.TabIndex = 8
        Me.outputResistanceLabel.Text = "Output Resistance (Ohms)"
        '
        'outputResistanceNumeric
        '
        Me.outputResistanceNumeric.DecimalPlaces = 6
        Me.outputResistanceNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.outputResistanceNumeric.Location = New System.Drawing.Point(160, 128)
        Me.outputResistanceNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.outputResistanceNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.outputResistanceNumeric.Name = "outputResistanceNumeric"
        Me.outputResistanceNumeric.Size = New System.Drawing.Size(91, 20)
        Me.outputResistanceNumeric.TabIndex = 9
        Me.outputResistanceNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'sourceDelayNumeric
        '
        Me.sourceDelayNumeric.DecimalPlaces = 6
        Me.sourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.sourceDelayNumeric.Location = New System.Drawing.Point(160, 154)
        Me.sourceDelayNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.sourceDelayNumeric.Name = "sourceDelayNumeric"
        Me.sourceDelayNumeric.Size = New System.Drawing.Size(91, 20)
        Me.sourceDelayNumeric.TabIndex = 11
        Me.sourceDelayNumeric.Value = New Decimal(New Integer() {5, 0, 0, 65536})
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(345, 278)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 3
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'resourceNameAndChannelNameGroupBox
        '
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.resourceNameAndChannelNameGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.resourceNameAndChannelNameGroupBox.Name = "resourceNameAndChannelNameGroupBox"
        Me.resourceNameAndChannelNameGroupBox.Size = New System.Drawing.Size(257, 73)
        Me.resourceNameAndChannelNameGroupBox.TabIndex = 0
        Me.resourceNameAndChannelNameGroupBox.TabStop = False
        Me.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(8, 39)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(243, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 22)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(97, 15)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'measurementsGroupBox
        '
        Me.measurementsGroupBox.Controls.Add(Me.voltageMeasurementTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.currentMeasurementTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.currentMeasurementLabel)
        Me.measurementsGroupBox.Controls.Add(Me.inComplianceButtonLed)
        Me.measurementsGroupBox.Controls.Add(Me.voltageMeasurementLabel)
        Me.measurementsGroupBox.Controls.Add(Me.inComplianceLabel)
        Me.measurementsGroupBox.Location = New System.Drawing.Point(285, 12)
        Me.measurementsGroupBox.Name = "measurementsGroupBox"
        Me.measurementsGroupBox.Size = New System.Drawing.Size(194, 132)
        Me.measurementsGroupBox.TabIndex = 2
        Me.measurementsGroupBox.TabStop = False
        Me.measurementsGroupBox.Text = "Measurements"
        '
        'voltageMeasurementTextBox
        '
        Me.voltageMeasurementTextBox.Location = New System.Drawing.Point(88, 19)
        Me.voltageMeasurementTextBox.Name = "voltageMeasurementTextBox"
        Me.voltageMeasurementTextBox.ReadOnly = True
        Me.voltageMeasurementTextBox.Size = New System.Drawing.Size(100, 20)
        Me.voltageMeasurementTextBox.TabIndex = 1
        Me.voltageMeasurementTextBox.Text = "0.000000E+000"
        '
        'currentMeasurementTextBox
        '
        Me.currentMeasurementTextBox.Location = New System.Drawing.Point(88, 45)
        Me.currentMeasurementTextBox.Name = "currentMeasurementTextBox"
        Me.currentMeasurementTextBox.ReadOnly = True
        Me.currentMeasurementTextBox.Size = New System.Drawing.Size(100, 20)
        Me.currentMeasurementTextBox.TabIndex = 3
        Me.currentMeasurementTextBox.Text = "0.000000E+000"
        '
        'inComplianceButtonLed
        '
        Me.inComplianceButtonLed.Enabled = False
        Me.inComplianceButtonLed.Location = New System.Drawing.Point(87, 101)
        Me.inComplianceButtonLed.Name = "inComplianceButtonLed"
        Me.inComplianceButtonLed.Size = New System.Drawing.Size(21, 21)
        Me.inComplianceButtonLed.TabIndex = 5
        Me.inComplianceButtonLed.UseVisualStyleBackColor = True
        '
        'outputShortedLabel
        '
        Me.outputShortedLabel.AutoSize = True
        Me.outputShortedLabel.Location = New System.Drawing.Point(6, 182)
        Me.outputShortedLabel.Name = "outputShortedLabel"
        Me.outputShortedLabel.Size = New System.Drawing.Size(89, 15)
        Me.outputShortedLabel.TabIndex = 12
        Me.outputShortedLabel.Text = "Output Shorted"
        '
        'outputShortedCheckBox
        '
        Me.outputShortedCheckBox.Location = New System.Drawing.Point(160, 180)
        Me.outputShortedCheckBox.Name = "outputShortedCheckBox"
        Me.outputShortedCheckBox.Size = New System.Drawing.Size(91, 21)
        Me.outputShortedCheckBox.TabIndex = 13
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.outputResistanceNumeric)
        Me.configurationGroupBox.Controls.Add(Me.outputResistanceLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitNumeric)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayLabel)
        Me.configurationGroupBox.Controls.Add(Me.outputShortedLabel)
        Me.configurationGroupBox.Controls.Add(Me.outputShortedCheckBox)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 101)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(257, 214)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(489, 328)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.measurementsGroupBox)
        Me.Controls.Add(Me.resourceNameAndChannelNameGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Sink DC Voltage with Output Resistance"
        CType(Me.currentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.currentLimitRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageLevelRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.outputResistanceNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.resourceNameAndChannelNameGroupBox.ResumeLayout(False)
        Me.resourceNameAndChannelNameGroupBox.PerformLayout()
        Me.measurementsGroupBox.ResumeLayout(False)
        Me.measurementsGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private currentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private currentLimitRangeNumeric As System.Windows.Forms.NumericUpDown
    Private voltageLevelNumeric As System.Windows.Forms.NumericUpDown
    Private voltageLevelRangeNumeric As System.Windows.Forms.NumericUpDown
    Private sourceDelayNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents startButton As System.Windows.Forms.Button
    Private resourceNameAndChannelNameGroupBox As System.Windows.Forms.GroupBox
    Private measurementsGroupBox As System.Windows.Forms.GroupBox
    Private inComplianceButtonLed As System.Windows.Forms.Button
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private voltageMeasurementTextBox As System.Windows.Forms.TextBox
    Private currentMeasurementTextBox As System.Windows.Forms.TextBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox

	Private currentLimitLabel As System.Windows.Forms.Label
	Private currentLimitRangeLabel As System.Windows.Forms.Label
	Private voltageLevelLabel As System.Windows.Forms.Label
	Private voltageLevelRangeLabel As System.Windows.Forms.Label
    Private outputResistanceLabel As System.Windows.Forms.Label
    Private sourceDelayLabel As System.Windows.Forms.Label
	Private voltageMeasurementLabel As System.Windows.Forms.Label
	Private currentMeasurementLabel As System.Windows.Forms.Label
	Private inComplianceLabel As System.Windows.Forms.Label
	Private resourceNameLabel As System.Windows.Forms.Label
    Private outputResistanceNumeric As System.Windows.Forms.NumericUpDown
    Private outputShortedLabel As System.Windows.Forms.Label
    Private outputShortedCheckBox As System.Windows.Forms.CheckBox

End Class
