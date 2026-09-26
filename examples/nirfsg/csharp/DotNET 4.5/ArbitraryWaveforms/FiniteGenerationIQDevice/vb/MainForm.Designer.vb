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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.waveformDurationTextBox = New System.Windows.Forms.TextBox()
        Me.waveformDurationLabel = New System.Windows.Forms.Label()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.referenceClockComboBox = New System.Windows.Forms.ComboBox()
        Me.iqOutPortLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.referenceClockLabel = New System.Windows.Forms.Label()
        Me.waveformRepeatCountNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqPortFrequencyLabel = New System.Windows.Forms.Label()
        Me.iqOutPortLevelLabel = New System.Windows.Forms.Label()
        Me.iqPortFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.measurementGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.iqOutPortLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.waveformRepeatCountNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqPortFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.waveformDurationTextBox)
        Me.measurementGroupBox.Controls.Add(Me.waveformDurationLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(340, 67)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(188, 95)
        Me.measurementGroupBox.TabIndex = 15
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'waveformDurationTextBox
        '
        Me.waveformDurationTextBox.Location = New System.Drawing.Point(22, 49)
        Me.waveformDurationTextBox.Name = "waveformDurationTextBox"
        Me.waveformDurationTextBox.ReadOnly = True
        Me.waveformDurationTextBox.Size = New System.Drawing.Size(109, 20)
        Me.waveformDurationTextBox.TabIndex = 9
        Me.waveformDurationTextBox.TabStop = False
        Me.waveformDurationTextBox.Text = "0.001000"
        '
        'waveformDurationLabel
        '
        Me.waveformDurationLabel.AutoSize = True
        Me.waveformDurationLabel.Location = New System.Drawing.Point(22, 28)
        Me.waveformDurationLabel.Name = "waveformDurationLabel"
        Me.waveformDurationLabel.Size = New System.Drawing.Size(113, 13)
        Me.waveformDurationLabel.TabIndex = 5
        Me.waveformDurationLabel.Text = "Waveform Duration [s]"
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.referenceClockComboBox)
        Me.configurationGroupBox.Controls.Add(Me.iqOutPortLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.referenceClockLabel)
        Me.configurationGroupBox.Controls.Add(Me.waveformRepeatCountNumeric)
        Me.configurationGroupBox.Controls.Add(Me.iqPortFrequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.iqOutPortLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.iqPortFrequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.iqRateLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 67)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(316, 153)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'referenceClockComboBox
        '
        Me.referenceClockComboBox.Location = New System.Drawing.Point(22, 49)
        Me.referenceClockComboBox.Name = "referenceClockComboBox"
        Me.referenceClockComboBox.Size = New System.Drawing.Size(110, 21)
        Me.referenceClockComboBox.TabIndex = 1
        '
        'iqOutPortLevelNumeric
        '
        Me.iqOutPortLevelNumeric.DecimalPlaces = 3
        Me.iqOutPortLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.iqOutPortLevelNumeric.Location = New System.Drawing.Point(177, 103)
        Me.iqOutPortLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqOutPortLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqOutPortLevelNumeric.Name = "iqOutPortLevelNumeric"
        Me.iqOutPortLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqOutPortLevelNumeric.TabIndex = 4
        Me.iqOutPortLevelNumeric.Value = New Decimal(New Integer() {5, 0, 0, 65536})
        '
        'referenceClockLabel
        '
        Me.referenceClockLabel.AutoSize = True
        Me.referenceClockLabel.Location = New System.Drawing.Point(22, 28)
        Me.referenceClockLabel.Name = "referenceClockLabel"
        Me.referenceClockLabel.Size = New System.Drawing.Size(71, 13)
        Me.referenceClockLabel.TabIndex = 28
        Me.referenceClockLabel.Text = "Clock Source"
        '
        'waveformRepeatCountNumeric
        '
        Me.waveformRepeatCountNumeric.Location = New System.Drawing.Point(22, 103)
        Me.waveformRepeatCountNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.waveformRepeatCountNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.waveformRepeatCountNumeric.Name = "waveformRepeatCountNumeric"
        Me.waveformRepeatCountNumeric.Size = New System.Drawing.Size(120, 20)
        Me.waveformRepeatCountNumeric.TabIndex = 2
        Me.waveformRepeatCountNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'iqPortFrequencyLabel
        '
        Me.iqPortFrequencyLabel.AutoSize = True
        Me.iqPortFrequencyLabel.Location = New System.Drawing.Point(177, 28)
        Me.iqPortFrequencyLabel.Name = "iqPortFrequencyLabel"
        Me.iqPortFrequencyLabel.Size = New System.Drawing.Size(115, 13)
        Me.iqPortFrequencyLabel.TabIndex = 1
        Me.iqPortFrequencyLabel.Text = "IQ Port Frequency (Hz)"
        '
        'iqOutPortLevelLabel
        '
        Me.iqOutPortLevelLabel.AutoSize = True
        Me.iqOutPortLevelLabel.Location = New System.Drawing.Point(177, 82)
        Me.iqOutPortLevelLabel.Name = "iqOutPortLevelLabel"
        Me.iqOutPortLevelLabel.Size = New System.Drawing.Size(117, 13)
        Me.iqOutPortLevelLabel.TabIndex = 2
        Me.iqOutPortLevelLabel.Text = "IQ Out Port Level (Vpp)"
        '
        'iqPortFrequencyNumeric
        '
        Me.iqPortFrequencyNumeric.DecimalPlaces = 6
        Me.iqPortFrequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.iqPortFrequencyNumeric.Location = New System.Drawing.Point(177, 49)
        Me.iqPortFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqPortFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqPortFrequencyNumeric.Name = "iqPortFrequencyNumeric"
        Me.iqPortFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqPortFrequencyNumeric.TabIndex = 3
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(22, 82)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(125, 13)
        Me.iqRateLabel.TabIndex = 4
        Me.iqRateLabel.Text = "Waveform Repeat Count"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(12, 9)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 13
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(12, 238)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 19
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 30)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(12, 268)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(516, 34)
        Me.errorTextBox.TabIndex = 20
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(361, 28)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 2
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(453, 28)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 3
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'rfsgStatusTimer
        '
        Me.rfsgStatusTimer.Interval = 1
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(540, 335)
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
        Me.Name = "MainForm"
        Me.Text = "Finite Generation IQ Device"
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.iqOutPortLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.waveformRepeatCountNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqPortFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

	#End Region

	Private measurementGroupBox As System.Windows.Forms.GroupBox
	Private waveformDurationTextBox As System.Windows.Forms.TextBox
	Private waveformDurationLabel As System.Windows.Forms.Label
	Private configurationGroupBox As System.Windows.Forms.GroupBox
	Private iqOutPortLevelNumeric As System.Windows.Forms.NumericUpDown
	Private waveformRepeatCountNumeric As System.Windows.Forms.NumericUpDown
	Private iqPortFrequencyLabel As System.Windows.Forms.Label
	Private iqOutPortLevelLabel As System.Windows.Forms.Label
	Private iqPortFrequencyNumeric As System.Windows.Forms.NumericUpDown
	Private iqRateLabel As System.Windows.Forms.Label
	Private resourceNameLabel As System.Windows.Forms.Label
	Private errorLabel As System.Windows.Forms.Label
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private errorTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
	Private referenceClockComboBox As System.Windows.Forms.ComboBox
	Private referenceClockLabel As System.Windows.Forms.Label
End Class

