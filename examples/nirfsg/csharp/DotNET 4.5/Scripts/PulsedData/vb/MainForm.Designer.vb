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
		''' the contents of Me method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.actualTimeSlotPeriodLabel = New System.Windows.Forms.Label()
        Me.actualFramePeriodLabel = New System.Windows.Forms.Label()
        Me.framePeriodActualLabel = New System.Windows.Forms.Label()
        Me.frequencyLabel = New System.Windows.Forms.Label()
        Me.timeSlotsLabel = New System.Windows.Forms.Label()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.actualIQRateLabel = New System.Windows.Forms.Label()
        Me.framePeriodNumeric = New System.Windows.Forms.NumericUpDown()
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.timeSlotsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.actualTimeSlotPeriodTextBox = New System.Windows.Forms.TextBox()
        Me.actualFramePeriodTextBox = New System.Windows.Forms.TextBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.actualIQRateTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.framePeriodNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.timeSlotsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.configurationGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(18, 16)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'actualTimeSlotPeriodLabel
        '
        Me.actualTimeSlotPeriodLabel.AutoSize = True
        Me.actualTimeSlotPeriodLabel.Location = New System.Drawing.Point(24, 84)
        Me.actualTimeSlotPeriodLabel.Name = "actualTimeSlotPeriodLabel"
        Me.actualTimeSlotPeriodLabel.Size = New System.Drawing.Size(131, 13)
        Me.actualTimeSlotPeriodLabel.TabIndex = 1
        Me.actualTimeSlotPeriodLabel.Text = "Actual Time Slot Period [s]"
        '
        'actualFramePeriodLabel
        '
        Me.actualFramePeriodLabel.AutoSize = True
        Me.actualFramePeriodLabel.Location = New System.Drawing.Point(175, 25)
        Me.actualFramePeriodLabel.Name = "actualFramePeriodLabel"
        Me.actualFramePeriodLabel.Size = New System.Drawing.Size(83, 13)
        Me.actualFramePeriodLabel.TabIndex = 2
        Me.actualFramePeriodLabel.Text = "Frame Period [s]"
        '
        'framePeriodActualLabel
        '
        Me.framePeriodActualLabel.AutoSize = True
        Me.framePeriodActualLabel.Location = New System.Drawing.Point(24, 31)
        Me.framePeriodActualLabel.Name = "framePeriodActualLabel"
        Me.framePeriodActualLabel.Size = New System.Drawing.Size(116, 13)
        Me.framePeriodActualLabel.TabIndex = 3
        Me.framePeriodActualLabel.Text = "Actual Frame Period [s]"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.AutoSize = True
        Me.frequencyLabel.Location = New System.Drawing.Point(21, 26)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.frequencyLabel.TabIndex = 4
        Me.frequencyLabel.Text = "Center Frequency [Hz]"
        '
        'timeSlotsLabel
        '
        Me.timeSlotsLabel.AutoSize = True
        Me.timeSlotsLabel.Location = New System.Drawing.Point(175, 78)
        Me.timeSlotsLabel.Name = "timeSlotsLabel"
        Me.timeSlotsLabel.Size = New System.Drawing.Size(56, 13)
        Me.timeSlotsLabel.TabIndex = 5
        Me.timeSlotsLabel.Text = "Time Slots"
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(21, 79)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.powerLevelLabel.TabIndex = 6
        Me.powerLevelLabel.Text = "Power Level [dBm]"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(21, 135)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
        Me.iqRateLabel.TabIndex = 7
        Me.iqRateLabel.Text = "IQ Rate [S/s]"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(18, 304)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 8
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'actualIQRateLabel
        '
        Me.actualIQRateLabel.AutoSize = True
        Me.actualIQRateLabel.Location = New System.Drawing.Point(24, 137)
        Me.actualIQRateLabel.Name = "actualIQRateLabel"
        Me.actualIQRateLabel.Size = New System.Drawing.Size(103, 13)
        Me.actualIQRateLabel.TabIndex = 10
        Me.actualIQRateLabel.Text = "Actual IQ Rate [S/s]"
        '
        'framePeriodNumeric
        '
        Me.framePeriodNumeric.DecimalPlaces = 5
        Me.framePeriodNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.framePeriodNumeric.Location = New System.Drawing.Point(175, 46)
        Me.framePeriodNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.framePeriodNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.framePeriodNumeric.Name = "framePeriodNumeric"
        Me.framePeriodNumeric.Size = New System.Drawing.Size(120, 20)
        Me.framePeriodNumeric.TabIndex = 3
        Me.framePeriodNumeric.Value = New Decimal(New Integer() {461536, 0, 0, 524288})
        '
        'frequencyNumeric
        '
        Me.frequencyNumeric.DecimalPlaces = 6
        Me.frequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.frequencyNumeric.Location = New System.Drawing.Point(21, 47)
        Me.frequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.frequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.frequencyNumeric.Name = "frequencyNumeric"
        Me.frequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.frequencyNumeric.TabIndex = 0
        Me.frequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'timeSlotsNumeric
        '
        Me.timeSlotsNumeric.Location = New System.Drawing.Point(175, 99)
        Me.timeSlotsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.timeSlotsNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.timeSlotsNumeric.Name = "timeSlotsNumeric"
        Me.timeSlotsNumeric.Size = New System.Drawing.Size(120, 20)
        Me.timeSlotsNumeric.TabIndex = 4
        Me.timeSlotsNumeric.Value = New Decimal(New Integer() {8, 0, 0, 0})
        '
        'powerLevelNumeric
        '
        Me.powerLevelNumeric.DecimalPlaces = 2
        Me.powerLevelNumeric.Location = New System.Drawing.Point(21, 100)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerLevelNumeric.TabIndex = 1
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 6
        Me.iqRateNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.iqRateNumeric.Location = New System.Drawing.Point(21, 156)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqRateNumeric.TabIndex = 5
        Me.iqRateNumeric.Value = New Decimal(New Integer() {100000000, 0, 0, 0})
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(18, 37)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'actualTimeSlotPeriodTextBox
        '
        Me.actualTimeSlotPeriodTextBox.Location = New System.Drawing.Point(24, 105)
        Me.actualTimeSlotPeriodTextBox.Name = "actualTimeSlotPeriodTextBox"
        Me.actualTimeSlotPeriodTextBox.ReadOnly = True
        Me.actualTimeSlotPeriodTextBox.Size = New System.Drawing.Size(109, 20)
        Me.actualTimeSlotPeriodTextBox.TabIndex = 1
        Me.actualTimeSlotPeriodTextBox.TabStop = False
        Me.actualTimeSlotPeriodTextBox.Text = "0.00000"
        '
        'actualFramePeriodTextBox
        '
        Me.actualFramePeriodTextBox.Location = New System.Drawing.Point(24, 52)
        Me.actualFramePeriodTextBox.Name = "actualFramePeriodTextBox"
        Me.actualFramePeriodTextBox.ReadOnly = True
        Me.actualFramePeriodTextBox.Size = New System.Drawing.Size(109, 20)
        Me.actualFramePeriodTextBox.TabIndex = 2
        Me.actualFramePeriodTextBox.TabStop = False
        Me.actualFramePeriodTextBox.Text = "0.00000"
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(19, 320)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(499, 34)
        Me.errorTextBox.TabIndex = 12
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'actualIQRateTextBox
        '
        Me.actualIQRateTextBox.Location = New System.Drawing.Point(24, 158)
        Me.actualIQRateTextBox.Name = "actualIQRateTextBox"
        Me.actualIQRateTextBox.ReadOnly = True
        Me.actualIQRateTextBox.Size = New System.Drawing.Size(109, 20)
        Me.actualIQRateTextBox.TabIndex = 14
        Me.actualIQRateTextBox.TabStop = False
        Me.actualIQRateTextBox.Text = "0.000000"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(365, 37)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 3
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(443, 37)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 3
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'rfsgStatusTimer
        '
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.iqRateNumeric)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.actualFramePeriodLabel)
        Me.configurationGroupBox.Controls.Add(Me.timeSlotsNumeric)
        Me.configurationGroupBox.Controls.Add(Me.frequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.frequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.timeSlotsLabel)
        Me.configurationGroupBox.Controls.Add(Me.framePeriodNumeric)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.iqRateLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(18, 79)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(317, 204)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.actualFramePeriodTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualTimeSlotPeriodTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualTimeSlotPeriodLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateLabel)
        Me.measurementGroupBox.Controls.Add(Me.framePeriodActualLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(352, 76)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(166, 207)
        Me.measurementGroupBox.TabIndex = 0
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(540, 400)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Pulsed Data"
        CType(Me.framePeriodNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.timeSlotsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
		#End Region

		Private resourceNameLabel As System.Windows.Forms.Label
		Private actualTimeSlotPeriodLabel As System.Windows.Forms.Label
		Private actualFramePeriodLabel As System.Windows.Forms.Label
		Private framePeriodActualLabel As System.Windows.Forms.Label
		Private frequencyLabel As System.Windows.Forms.Label
		Private timeSlotsLabel As System.Windows.Forms.Label
		Private powerLevelLabel As System.Windows.Forms.Label
		Private iqRateLabel As System.Windows.Forms.Label
		Private errorLabel As System.Windows.Forms.Label
    Private actualIQRateLabel As System.Windows.Forms.Label
		Private framePeriodNumeric As System.Windows.Forms.NumericUpDown
		Private frequencyNumeric As System.Windows.Forms.NumericUpDown
		Private timeSlotsNumeric As System.Windows.Forms.NumericUpDown
		Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
		Private iqRateNumeric As System.Windows.Forms.NumericUpDown
		Private resourceNameComboBox As System.Windows.Forms.ComboBox
		Private actualTimeSlotPeriodTextBox As System.Windows.Forms.TextBox
		Private actualFramePeriodTextBox As System.Windows.Forms.TextBox
		Private errorTextBox As System.Windows.Forms.TextBox
		Private actualIQRateTextBox As System.Windows.Forms.TextBox
		Private WithEvents startButton As System.Windows.Forms.Button
		Private WithEvents stopButton As System.Windows.Forms.Button    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
		Private configurationGroupBox As System.Windows.Forms.GroupBox
		Private measurementGroupBox As System.Windows.Forms.GroupBox

	End Class
