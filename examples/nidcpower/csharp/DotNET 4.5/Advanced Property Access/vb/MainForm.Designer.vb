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
        Me.senseLabel = New System.Windows.Forms.Label()
        Me.sourceDelayLabel = New System.Windows.Forms.Label()
        Me.measuredVoltageLabel = New System.Windows.Forms.Label()
        Me.measuredCurrentLabel = New System.Windows.Forms.Label()
        Me.limitReachedLabel = New System.Windows.Forms.Label()
        Me.currentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.currentLimitRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.voltageLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.voltageLevelRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.sourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startButton = New System.Windows.Forms.Button()
        Me.senseComboBox = New System.Windows.Forms.ComboBox()
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
        CType(Me.currentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.currentLimitRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageLevelRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resourceNameAndChannelNameGroupBox.SuspendLayout()
        Me.measurementsGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'currentLimitLabel
        '
        Me.currentLimitLabel.AutoSize = True
        Me.currentLimitLabel.Location = New System.Drawing.Point(6, 75)
        Me.currentLimitLabel.Name = "currentLimitLabel"
        Me.currentLimitLabel.Size = New System.Drawing.Size(81, 13)
        Me.currentLimitLabel.TabIndex = 4
        Me.currentLimitLabel.Text = "Current Limit (A)"
        '
        'currentLimitRangeLabel
        '
        Me.currentLimitRangeLabel.AutoSize = True
        Me.currentLimitRangeLabel.Location = New System.Drawing.Point(6, 101)
        Me.currentLimitRangeLabel.Name = "currentLimitRangeLabel"
        Me.currentLimitRangeLabel.Size = New System.Drawing.Size(118, 13)
        Me.currentLimitRangeLabel.TabIndex = 6
        Me.currentLimitRangeLabel.Text = "Current Limitl Range (A)"
        '
        'voltageLevelLabel
        '
        Me.voltageLevelLabel.AutoSize = True
        Me.voltageLevelLabel.Location = New System.Drawing.Point(6, 23)
        Me.voltageLevelLabel.Name = "voltageLevelLabel"
        Me.voltageLevelLabel.Size = New System.Drawing.Size(88, 13)
        Me.voltageLevelLabel.TabIndex = 0
        Me.voltageLevelLabel.Text = "Voltage Level (V)"
        '
        'voltageLevelRangeLabel
        '
        Me.voltageLevelRangeLabel.AutoSize = True
        Me.voltageLevelRangeLabel.Location = New System.Drawing.Point(6, 49)
        Me.voltageLevelRangeLabel.Name = "voltageLevelRangeLabel"
        Me.voltageLevelRangeLabel.Size = New System.Drawing.Size(123, 13)
        Me.voltageLevelRangeLabel.TabIndex = 2
        Me.voltageLevelRangeLabel.Text = "Voltage Level Range (V)"
        '
        'senseLabel
        '
        Me.senseLabel.AutoSize = True
        Me.senseLabel.Location = New System.Drawing.Point(6, 127)
        Me.senseLabel.Name = "senseLabel"
        Me.senseLabel.Size = New System.Drawing.Size(37, 13)
        Me.senseLabel.TabIndex = 8
        Me.senseLabel.Text = "Sense"
        '
        'sourceDelayLabel
        '
        Me.sourceDelayLabel.AutoSize = True
        Me.sourceDelayLabel.Location = New System.Drawing.Point(6, 154)
        Me.sourceDelayLabel.Name = "sourceDelayLabel"
        Me.sourceDelayLabel.Size = New System.Drawing.Size(85, 13)
        Me.sourceDelayLabel.TabIndex = 10
        Me.sourceDelayLabel.Text = "Source Delay (s)"
        '
        'measuredVoltageLabel
        '
        Me.measuredVoltageLabel.AutoSize = True
        Me.measuredVoltageLabel.Location = New System.Drawing.Point(6, 23)
        Me.measuredVoltageLabel.Name = "measuredVoltageLabel"
        Me.measuredVoltageLabel.Size = New System.Drawing.Size(59, 13)
        Me.measuredVoltageLabel.TabIndex = 0
        Me.measuredVoltageLabel.Text = "Voltage (V)"
        '
        'measuredCurrentLabel
        '
        Me.measuredCurrentLabel.AutoSize = True
        Me.measuredCurrentLabel.Location = New System.Drawing.Point(6, 49)
        Me.measuredCurrentLabel.Name = "measuredCurrentLabel"
        Me.measuredCurrentLabel.Size = New System.Drawing.Size(57, 13)
        Me.measuredCurrentLabel.TabIndex = 2
        Me.measuredCurrentLabel.Text = "Current (A)"
        '
        'limitReachedLabel
        '
        Me.limitReachedLabel.AutoSize = True
        Me.limitReachedLabel.Location = New System.Drawing.Point(30, 85)
        Me.limitReachedLabel.Name = "limitReachedLabel"
        Me.limitReachedLabel.Size = New System.Drawing.Size(135, 13)
        Me.limitReachedLabel.TabIndex = 4
        Me.limitReachedLabel.Text = "Compliance/Limit Reached"
        '
        'currentLimitNumeric
        '
        Me.currentLimitNumeric.DecimalPlaces = 6
        Me.currentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.currentLimitNumeric.Location = New System.Drawing.Point(140, 71)
        Me.currentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.currentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.currentLimitNumeric.Name = "currentLimitNumeric"
        Me.currentLimitNumeric.Size = New System.Drawing.Size(91, 20)
        Me.currentLimitNumeric.TabIndex = 5
        Me.currentLimitNumeric.Value = New Decimal(New Integer() {2, 0, 0, 131072})
        '
        'currentLimitRangeNumeric
        '
        Me.currentLimitRangeNumeric.DecimalPlaces = 6
        Me.currentLimitRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.currentLimitRangeNumeric.Location = New System.Drawing.Point(140, 97)
        Me.currentLimitRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.currentLimitRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.currentLimitRangeNumeric.Name = "currentLimitRangeNumeric"
        Me.currentLimitRangeNumeric.Size = New System.Drawing.Size(91, 20)
        Me.currentLimitRangeNumeric.TabIndex = 7
        Me.currentLimitRangeNumeric.Value = New Decimal(New Integer() {2, 0, 0, 131072})
        '
        'voltageLevelNumeric
        '
        Me.voltageLevelNumeric.DecimalPlaces = 6
        Me.voltageLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLevelNumeric.Location = New System.Drawing.Point(140, 19)
        Me.voltageLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLevelNumeric.Name = "voltageLevelNumeric"
        Me.voltageLevelNumeric.Size = New System.Drawing.Size(91, 20)
        Me.voltageLevelNumeric.TabIndex = 1
        Me.voltageLevelNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'voltageLevelRangeNumeric
        '
        Me.voltageLevelRangeNumeric.DecimalPlaces = 6
        Me.voltageLevelRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLevelRangeNumeric.Location = New System.Drawing.Point(140, 45)
        Me.voltageLevelRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLevelRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLevelRangeNumeric.Name = "voltageLevelRangeNumeric"
        Me.voltageLevelRangeNumeric.Size = New System.Drawing.Size(91, 20)
        Me.voltageLevelRangeNumeric.TabIndex = 3
        Me.voltageLevelRangeNumeric.Value = New Decimal(New Integer() {6, 0, 0, 0})
        '
        'sourceDelayNumeric
        '
        Me.sourceDelayNumeric.DecimalPlaces = 6
        Me.sourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.sourceDelayNumeric.Location = New System.Drawing.Point(140, 150)
        Me.sourceDelayNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.sourceDelayNumeric.Name = "sourceDelayNumeric"
        Me.sourceDelayNumeric.Size = New System.Drawing.Size(91, 20)
        Me.sourceDelayNumeric.TabIndex = 11
        Me.sourceDelayNumeric.Value = New Decimal(New Integer() {5, 0, 0, 131072})
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(325, 226)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 3
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'senseComboBox
        '
        Me.senseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.senseComboBox.Location = New System.Drawing.Point(140, 123)
        Me.senseComboBox.Name = "senseComboBox"
        Me.senseComboBox.Size = New System.Drawing.Size(91, 21)
        Me.senseComboBox.TabIndex = 9
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
        Me.channelNameTextBox.TabIndex = 3
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
        'measurementsGroupBox
        '
        Me.measurementsGroupBox.Controls.Add(Me.voltageMeasurementsTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.currentMeasurementsTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.measuredCurrentLabel)
        Me.measurementsGroupBox.Controls.Add(Me.inComplianceButtonLed)
        Me.measurementsGroupBox.Controls.Add(Me.measuredVoltageLabel)
        Me.measurementsGroupBox.Controls.Add(Me.limitReachedLabel)
        Me.measurementsGroupBox.Location = New System.Drawing.Point(265, 12)
        Me.measurementsGroupBox.Name = "measurementsGroupBox"
        Me.measurementsGroupBox.Size = New System.Drawing.Size(194, 132)
        Me.measurementsGroupBox.TabIndex = 2
        Me.measurementsGroupBox.TabStop = False
        Me.measurementsGroupBox.Text = "Measurements"
        '
        'voltageMeasurementsTextBox
        '
        Me.voltageMeasurementsTextBox.Location = New System.Drawing.Point(88, 19)
        Me.voltageMeasurementsTextBox.Name = "voltageMeasurementsTextBox"
        Me.voltageMeasurementsTextBox.ReadOnly = True
        Me.voltageMeasurementsTextBox.Size = New System.Drawing.Size(100, 20)
        Me.voltageMeasurementsTextBox.TabIndex = 1
        Me.voltageMeasurementsTextBox.Text = "0.000000E+000"
        '
        'currentMeasurementsTextBox
        '
        Me.currentMeasurementsTextBox.Location = New System.Drawing.Point(88, 45)
        Me.currentMeasurementsTextBox.Name = "currentMeasurementsTextBox"
        Me.currentMeasurementsTextBox.ReadOnly = True
        Me.currentMeasurementsTextBox.Size = New System.Drawing.Size(100, 20)
        Me.currentMeasurementsTextBox.TabIndex = 3
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
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.senseComboBox)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitLabel)
        Me.configurationGroupBox.Controls.Add(Me.senseLabel)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitNumeric)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 101)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(237, 180)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(469, 292)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.measurementsGroupBox)
        Me.Controls.Add(Me.resourceNameAndChannelNameGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Source DC Voltage using Advanced Property Access"
        CType(Me.currentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.currentLimitRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageLevelRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
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

    Private currentLimitLabel As System.Windows.Forms.Label
    Private currentLimitRangeLabel As System.Windows.Forms.Label
    Private voltageLevelLabel As System.Windows.Forms.Label
    Private voltageLevelRangeLabel As System.Windows.Forms.Label
    Private senseLabel As System.Windows.Forms.Label
    Private sourceDelayLabel As System.Windows.Forms.Label
    Private measuredVoltageLabel As System.Windows.Forms.Label
    Private measuredCurrentLabel As System.Windows.Forms.Label
    Private limitReachedLabel As System.Windows.Forms.Label
    Private currentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private currentLimitRangeNumeric As System.Windows.Forms.NumericUpDown
    Private voltageLevelNumeric As System.Windows.Forms.NumericUpDown
    Private voltageLevelRangeNumeric As System.Windows.Forms.NumericUpDown
    Private sourceDelayNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents startButton As System.Windows.Forms.Button
    Private senseComboBox As System.Windows.Forms.ComboBox
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
