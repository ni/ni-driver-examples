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
        Me.currentLevelLabel = New System.Windows.Forms.Label()
        Me.currentLevelRangeLabel = New System.Windows.Forms.Label()
        Me.voltageLimitLabel = New System.Windows.Forms.Label()
        Me.voltageLimitRangeLabel = New System.Windows.Forms.Label()
        Me.sourceDelayLabel = New System.Windows.Forms.Label()
        Me.voltageMeasurementLabel = New System.Windows.Forms.Label()
        Me.currentMeasurementLabel = New System.Windows.Forms.Label()
        Me.inComplianceLabel = New System.Windows.Forms.Label()
        Me.currentLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.currentLevelRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.voltageLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.voltageLimitRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.sourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startButton = New System.Windows.Forms.Button()
        Me.resourceNameAndChannelNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.measurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.voltageMeasurementsTextBox = New System.Windows.Forms.TextBox()
        Me.currentMeasurementsTextBox = New System.Windows.Forms.TextBox()
        Me.inComplianceButtonLed = New System.Windows.Forms.Button()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.currentLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.currentLevelRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageLimitRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resourceNameAndChannelNameGroupBox.SuspendLayout()
        Me.measurementsGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'currentLevelLabel
        '
        Me.currentLevelLabel.AutoSize = True
        Me.currentLevelLabel.Location = New System.Drawing.Point(6, 23)
        Me.currentLevelLabel.Name = "currentLevelLabel"
        Me.currentLevelLabel.Size = New System.Drawing.Size(86, 13)
        Me.currentLevelLabel.TabIndex = 0
        Me.currentLevelLabel.Text = "Current Level (A)"
        '
        'currentLevelRangeLabel
        '
        Me.currentLevelRangeLabel.AutoSize = True
        Me.currentLevelRangeLabel.Location = New System.Drawing.Point(6, 49)
        Me.currentLevelRangeLabel.Name = "currentLevelRangeLabel"
        Me.currentLevelRangeLabel.Size = New System.Drawing.Size(121, 13)
        Me.currentLevelRangeLabel.TabIndex = 2
        Me.currentLevelRangeLabel.Text = "Current Level Range (A)"
        '
        'voltageLimitLabel
        '
        Me.voltageLimitLabel.AutoSize = True
        Me.voltageLimitLabel.Location = New System.Drawing.Point(6, 75)
        Me.voltageLimitLabel.Name = "voltageLimitLabel"
        Me.voltageLimitLabel.Size = New System.Drawing.Size(83, 13)
        Me.voltageLimitLabel.TabIndex = 4
        Me.voltageLimitLabel.Text = "Voltage Limit (V)"
        '
        'voltageLimitRangeLabel
        '
        Me.voltageLimitRangeLabel.AutoSize = True
        Me.voltageLimitRangeLabel.Location = New System.Drawing.Point(6, 101)
        Me.voltageLimitRangeLabel.Name = "voltageLimitRangeLabel"
        Me.voltageLimitRangeLabel.Size = New System.Drawing.Size(118, 13)
        Me.voltageLimitRangeLabel.TabIndex = 6
        Me.voltageLimitRangeLabel.Text = "Voltage Limit Range (V)"
        '
        'sourceDelayLabel
        '
        Me.sourceDelayLabel.AutoSize = True
        Me.sourceDelayLabel.Location = New System.Drawing.Point(6, 130)
        Me.sourceDelayLabel.Name = "sourceDelayLabel"
        Me.sourceDelayLabel.Size = New System.Drawing.Size(85, 13)
        Me.sourceDelayLabel.TabIndex = 10
        Me.sourceDelayLabel.Text = "Source Delay (s)"
        '
        'voltageMeasurementLabel
        '
        Me.voltageMeasurementLabel.AutoSize = True
        Me.voltageMeasurementLabel.Location = New System.Drawing.Point(6, 49)
        Me.voltageMeasurementLabel.Name = "voltageMeasurementLabel"
        Me.voltageMeasurementLabel.Size = New System.Drawing.Size(59, 13)
        Me.voltageMeasurementLabel.TabIndex = 2
        Me.voltageMeasurementLabel.Text = "Voltage (V)"
        '
        'currentMeasurementLabel
        '
        Me.currentMeasurementLabel.AutoSize = True
        Me.currentMeasurementLabel.Location = New System.Drawing.Point(6, 23)
        Me.currentMeasurementLabel.Name = "currentMeasurementLabel"
        Me.currentMeasurementLabel.Size = New System.Drawing.Size(57, 13)
        Me.currentMeasurementLabel.TabIndex = 0
        Me.currentMeasurementLabel.Text = "Current (A)"
        '
        'inComplianceLabel
        '
        Me.inComplianceLabel.AutoSize = True
        Me.inComplianceLabel.Location = New System.Drawing.Point(30, 85)
        Me.inComplianceLabel.Name = "inComplianceLabel"
        Me.inComplianceLabel.Size = New System.Drawing.Size(135, 13)
        Me.inComplianceLabel.TabIndex = 4
        Me.inComplianceLabel.Text = "Compliance/Limit Reached"
        '
        'currentLevelNumeric
        '
        Me.currentLevelNumeric.DecimalPlaces = 6
        Me.currentLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.currentLevelNumeric.Location = New System.Drawing.Point(160, 19)
        Me.currentLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.currentLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.currentLevelNumeric.Name = "currentLevelNumeric"
        Me.currentLevelNumeric.Size = New System.Drawing.Size(91, 20)
        Me.currentLevelNumeric.TabIndex = 1
        Me.currentLevelNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'currentLevelRangeNumeric
        '
        Me.currentLevelRangeNumeric.DecimalPlaces = 6
        Me.currentLevelRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.currentLevelRangeNumeric.Location = New System.Drawing.Point(160, 45)
        Me.currentLevelRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.currentLevelRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.currentLevelRangeNumeric.Name = "currentLevelRangeNumeric"
        Me.currentLevelRangeNumeric.Size = New System.Drawing.Size(91, 20)
        Me.currentLevelRangeNumeric.TabIndex = 3
        Me.currentLevelRangeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'voltageLimitNumeric
        '
        Me.voltageLimitNumeric.DecimalPlaces = 6
        Me.voltageLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLimitNumeric.Location = New System.Drawing.Point(160, 71)
        Me.voltageLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLimitNumeric.Name = "voltageLimitNumeric"
        Me.voltageLimitNumeric.Size = New System.Drawing.Size(91, 20)
        Me.voltageLimitNumeric.TabIndex = 5
        Me.voltageLimitNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'voltageLimitRangeNumeric
        '
        Me.voltageLimitRangeNumeric.DecimalPlaces = 6
        Me.voltageLimitRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLimitRangeNumeric.Location = New System.Drawing.Point(160, 97)
        Me.voltageLimitRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLimitRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLimitRangeNumeric.Name = "voltageLimitRangeNumeric"
        Me.voltageLimitRangeNumeric.Size = New System.Drawing.Size(91, 20)
        Me.voltageLimitRangeNumeric.TabIndex = 7
        Me.voltageLimitRangeNumeric.Value = New Decimal(New Integer() {6, 0, 0, 0})
        '
        'sourceDelayNumeric
        '
        Me.sourceDelayNumeric.DecimalPlaces = 6
        Me.sourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.sourceDelayNumeric.Location = New System.Drawing.Point(160, 128)
        Me.sourceDelayNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.sourceDelayNumeric.Name = "sourceDelayNumeric"
        Me.sourceDelayNumeric.Size = New System.Drawing.Size(91, 20)
        Me.sourceDelayNumeric.TabIndex = 11
        Me.sourceDelayNumeric.Value = New Decimal(New Integer() {5, 0, 0, 131072})
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(345, 226)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 3
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
        Me.resourceNameAndChannelNameGroupBox.Size = New System.Drawing.Size(257, 73)
        Me.resourceNameAndChannelNameGroupBox.TabIndex = 0
        Me.resourceNameAndChannelNameGroupBox.TabStop = False
        Me.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name"
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(160, 45)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(91, 20)
        Me.channelNameTextBox.TabIndex = 3
        Me.channelNameTextBox.Text = "0"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(126, 18)
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
        'measurementsGroupBox
        '
        Me.measurementsGroupBox.Controls.Add(Me.voltageMeasurementsTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.currentMeasurementsTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.inComplianceButtonLed)
        Me.measurementsGroupBox.Controls.Add(Me.currentMeasurementLabel)
        Me.measurementsGroupBox.Controls.Add(Me.voltageMeasurementLabel)
        Me.measurementsGroupBox.Controls.Add(Me.inComplianceLabel)
        Me.measurementsGroupBox.Location = New System.Drawing.Point(285, 12)
        Me.measurementsGroupBox.Name = "measurementsGroupBox"
        Me.measurementsGroupBox.Size = New System.Drawing.Size(194, 132)
        Me.measurementsGroupBox.TabIndex = 2
        Me.measurementsGroupBox.TabStop = False
        Me.measurementsGroupBox.Text = "Measurements"
        '
        'voltageMeasurementsTextBox
        '
        Me.voltageMeasurementsTextBox.Location = New System.Drawing.Point(88, 45)
        Me.voltageMeasurementsTextBox.Name = "voltageMeasurementsTextBox"
        Me.voltageMeasurementsTextBox.ReadOnly = True
        Me.voltageMeasurementsTextBox.Size = New System.Drawing.Size(100, 20)
        Me.voltageMeasurementsTextBox.TabIndex = 3
        Me.voltageMeasurementsTextBox.Text = "0.000000E+000"
        '
        'currentMeasurementsTextBox
        '
        Me.currentMeasurementsTextBox.Location = New System.Drawing.Point(88, 19)
        Me.currentMeasurementsTextBox.Name = "currentMeasurementsTextBox"
        Me.currentMeasurementsTextBox.ReadOnly = True
        Me.currentMeasurementsTextBox.Size = New System.Drawing.Size(100, 20)
        Me.currentMeasurementsTextBox.TabIndex = 1
        Me.currentMeasurementsTextBox.Text = "0.000000E+000"
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
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayNumeric)
        Me.configurationGroupBox.Controls.Add(Me.voltageLimitLabel)
        Me.configurationGroupBox.Controls.Add(Me.currentLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltageLimitRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLevelRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.currentLevelRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.voltageLimitRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltageLimitNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 101)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(257, 162)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(489, 282)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.measurementsGroupBox)
        Me.Controls.Add(Me.resourceNameAndChannelNameGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Source DC Current"
        CType(Me.currentLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.currentLevelRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageLimitRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
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

    Private currentLevelLabel As System.Windows.Forms.Label
    Private currentLevelRangeLabel As System.Windows.Forms.Label
    Private voltageLimitLabel As System.Windows.Forms.Label
    Private voltageLimitRangeLabel As System.Windows.Forms.Label
    Private sourceDelayLabel As System.Windows.Forms.Label
    Private voltageMeasurementLabel As System.Windows.Forms.Label
    Private currentMeasurementLabel As System.Windows.Forms.Label
    Private inComplianceLabel As System.Windows.Forms.Label
    Private currentLevelNumeric As System.Windows.Forms.NumericUpDown
    Private currentLevelRangeNumeric As System.Windows.Forms.NumericUpDown
    Private voltageLimitNumeric As System.Windows.Forms.NumericUpDown
    Private voltageLimitRangeNumeric As System.Windows.Forms.NumericUpDown
    Private sourceDelayNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents startButton As System.Windows.Forms.Button
    Private resourceNameAndChannelNameGroupBox As System.Windows.Forms.GroupBox
    Private channelNameLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private measurementsGroupBox As System.Windows.Forms.GroupBox
    Private inComplianceButtonLed As System.Windows.Forms.Button
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private voltageMeasurementsTextBox As System.Windows.Forms.TextBox
    Private currentMeasurementsTextBox As System.Windows.Forms.TextBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private channelNameTextBox As System.Windows.Forms.TextBox

End Class
