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
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.centerFrequencyLabel = New System.Windows.Forms.Label()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqrateLabel = New System.Windows.Forms.Label()
        Me.softwareTriggerButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.frequencyReferenceGroupBox = New System.Windows.Forms.GroupBox()
        Me.slaveReferenceClockOutputTerminalComboBox = New System.Windows.Forms.ComboBox()
        Me.masterReferenceClockSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.slaveReferenceClockSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.masterReferenceClockOutputTerminalComboBox = New System.Windows.Forms.ComboBox()
        Me.masterReferenceClockSourceLabel = New System.Windows.Forms.Label()
        Me.slaveReferenceClockSourceLabel = New System.Windows.Forms.Label()
        Me.masterReferenceClockOutputTerminalLabel = New System.Windows.Forms.Label()
        Me.slaveReferenceClockOutputTerminalLabel = New System.Windows.Forms.Label()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.rfsgMasterResourceNameLabel = New System.Windows.Forms.Label()
        Me.rfsgSlaveResourceNamesLabel = New System.Windows.Forms.Label()
        Me.slaveRfsgResourceNamesTextBox = New System.Windows.Forms.TextBox()
        Me.scriptRichTextBox = New System.Windows.Forms.RichTextBox()
        Me.scriptLabel = New System.Windows.Forms.Label()
        Me.startButton = New System.Windows.Forms.Button()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.textmessageLabel = New System.Windows.Forms.Label()
        Me.masterRfsgResourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.frequencyReferenceGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.powerLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.frequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.centerFrequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.iqRateNumeric)
        Me.configurationGroupBox.Controls.Add(Me.iqrateLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 111)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(176, 173)
        Me.configurationGroupBox.TabIndex = 2
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'powerLevelNumeric
        '
        Me.powerLevelNumeric.DecimalPlaces = 2
        Me.powerLevelNumeric.Location = New System.Drawing.Point(10, 87)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(121, 20)
        Me.powerLevelNumeric.TabIndex = 1
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'frequencyNumeric
        '
        Me.frequencyNumeric.DecimalPlaces = 6
        Me.frequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.frequencyNumeric.Location = New System.Drawing.Point(10, 40)
        Me.frequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.frequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.frequencyNumeric.Name = "frequencyNumeric"
        Me.frequencyNumeric.Size = New System.Drawing.Size(121, 20)
        Me.frequencyNumeric.TabIndex = 0
        Me.frequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(10, 70)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.powerLevelLabel.TabIndex = 4
        Me.powerLevelLabel.Text = "Power Level [dBm]"
        '
        'centerFrequencyLabel
        '
        Me.centerFrequencyLabel.AutoSize = True
        Me.centerFrequencyLabel.Location = New System.Drawing.Point(10, 23)
        Me.centerFrequencyLabel.Name = "centerFrequencyLabel"
        Me.centerFrequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.centerFrequencyLabel.TabIndex = 3
        Me.centerFrequencyLabel.Text = "Center Frequency [Hz]"
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 2
        Me.iqRateNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.iqRateNumeric.Location = New System.Drawing.Point(10, 134)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(121, 20)
        Me.iqRateNumeric.TabIndex = 2
        Me.iqRateNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'iqrateLabel
        '
        Me.iqrateLabel.AutoSize = True
        Me.iqrateLabel.Location = New System.Drawing.Point(10, 117)
        Me.iqrateLabel.Name = "iqrateLabel"
        Me.iqrateLabel.Size = New System.Drawing.Size(44, 13)
        Me.iqrateLabel.TabIndex = 5
        Me.iqrateLabel.Text = "IQ Rate"
        '
        'softwareTriggerButton
        '
        Me.softwareTriggerButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.softwareTriggerButton.Enabled = False
        Me.softwareTriggerButton.Location = New System.Drawing.Point(302, 559)
        Me.softwareTriggerButton.Name = "softwareTriggerButton"
        Me.softwareTriggerButton.Size = New System.Drawing.Size(131, 23)
        Me.softwareTriggerButton.TabIndex = 9
        Me.softwareTriggerButton.Text = "Send Software Trigger"
        Me.softwareTriggerButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(439, 559)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 10
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'frequencyReferenceGroupBox
        '
        Me.frequencyReferenceGroupBox.Controls.Add(Me.slaveReferenceClockOutputTerminalComboBox)
        Me.frequencyReferenceGroupBox.Controls.Add(Me.masterReferenceClockSourceComboBox)
        Me.frequencyReferenceGroupBox.Controls.Add(Me.slaveReferenceClockSourceComboBox)
        Me.frequencyReferenceGroupBox.Controls.Add(Me.masterReferenceClockOutputTerminalComboBox)
        Me.frequencyReferenceGroupBox.Controls.Add(Me.masterReferenceClockSourceLabel)
        Me.frequencyReferenceGroupBox.Controls.Add(Me.slaveReferenceClockSourceLabel)
        Me.frequencyReferenceGroupBox.Controls.Add(Me.masterReferenceClockOutputTerminalLabel)
        Me.frequencyReferenceGroupBox.Controls.Add(Me.slaveReferenceClockOutputTerminalLabel)
        Me.frequencyReferenceGroupBox.Location = New System.Drawing.Point(258, 70)
        Me.frequencyReferenceGroupBox.Name = "frequencyReferenceGroupBox"
        Me.frequencyReferenceGroupBox.Size = New System.Drawing.Size(256, 214)
        Me.frequencyReferenceGroupBox.TabIndex = 3
        Me.frequencyReferenceGroupBox.TabStop = False
        Me.frequencyReferenceGroupBox.Text = "Frequency Reference Parameters"
        '
        'slaveReferenceClockOutputTerminalComboBox
        '
        Me.slaveReferenceClockOutputTerminalComboBox.Location = New System.Drawing.Point(9, 183)
        Me.slaveReferenceClockOutputTerminalComboBox.Name = "slaveReferenceClockOutputTerminalComboBox"
        Me.slaveReferenceClockOutputTerminalComboBox.Size = New System.Drawing.Size(119, 21)
        Me.slaveReferenceClockOutputTerminalComboBox.TabIndex = 7
        '
        'masterReferenceClockSourceComboBox
        '
        Me.masterReferenceClockSourceComboBox.Location = New System.Drawing.Point(9, 39)
        Me.masterReferenceClockSourceComboBox.Name = "masterReferenceClockSourceComboBox"
        Me.masterReferenceClockSourceComboBox.Size = New System.Drawing.Size(119, 21)
        Me.masterReferenceClockSourceComboBox.TabIndex = 1
        '
        'slaveReferenceClockSourceComboBox
        '
        Me.slaveReferenceClockSourceComboBox.Location = New System.Drawing.Point(9, 135)
        Me.slaveReferenceClockSourceComboBox.Name = "slaveReferenceClockSourceComboBox"
        Me.slaveReferenceClockSourceComboBox.Size = New System.Drawing.Size(119, 21)
        Me.slaveReferenceClockSourceComboBox.TabIndex = 5
        '
        'masterReferenceClockOutputTerminalComboBox
        '
        Me.masterReferenceClockOutputTerminalComboBox.Location = New System.Drawing.Point(9, 87)
        Me.masterReferenceClockOutputTerminalComboBox.Name = "masterReferenceClockOutputTerminalComboBox"
        Me.masterReferenceClockOutputTerminalComboBox.Size = New System.Drawing.Size(119, 21)
        Me.masterReferenceClockOutputTerminalComboBox.TabIndex = 3
        '
        'masterReferenceClockSourceLabel
        '
        Me.masterReferenceClockSourceLabel.AutoSize = True
        Me.masterReferenceClockSourceLabel.Location = New System.Drawing.Point(6, 21)
        Me.masterReferenceClockSourceLabel.Name = "masterReferenceClockSourceLabel"
        Me.masterReferenceClockSourceLabel.Size = New System.Drawing.Size(159, 13)
        Me.masterReferenceClockSourceLabel.TabIndex = 0
        Me.masterReferenceClockSourceLabel.Text = "Master Reference Clock Source"
        '
        'slaveReferenceClockSourceLabel
        '
        Me.slaveReferenceClockSourceLabel.AutoSize = True
        Me.slaveReferenceClockSourceLabel.Location = New System.Drawing.Point(9, 118)
        Me.slaveReferenceClockSourceLabel.Name = "slaveReferenceClockSourceLabel"
        Me.slaveReferenceClockSourceLabel.Size = New System.Drawing.Size(154, 13)
        Me.slaveReferenceClockSourceLabel.TabIndex = 4
        Me.slaveReferenceClockSourceLabel.Text = "Slave Reference Clock Source"
        '
        'masterReferenceClockOutputTerminalLabel
        '
        Me.masterReferenceClockOutputTerminalLabel.AutoSize = True
        Me.masterReferenceClockOutputTerminalLabel.Location = New System.Drawing.Point(9, 70)
        Me.masterReferenceClockOutputTerminalLabel.Name = "masterReferenceClockOutputTerminalLabel"
        Me.masterReferenceClockOutputTerminalLabel.Size = New System.Drawing.Size(200, 13)
        Me.masterReferenceClockOutputTerminalLabel.TabIndex = 2
        Me.masterReferenceClockOutputTerminalLabel.Text = "Master Reference Clock Output Terminal"
        '
        'slaveReferenceClockOutputTerminalLabel
        '
        Me.slaveReferenceClockOutputTerminalLabel.AutoSize = True
        Me.slaveReferenceClockOutputTerminalLabel.Location = New System.Drawing.Point(9, 166)
        Me.slaveReferenceClockOutputTerminalLabel.Name = "slaveReferenceClockOutputTerminalLabel"
        Me.slaveReferenceClockOutputTerminalLabel.Size = New System.Drawing.Size(195, 13)
        Me.slaveReferenceClockOutputTerminalLabel.TabIndex = 6
        Me.slaveReferenceClockOutputTerminalLabel.Text = "Slave Reference Clock Output Terminal"
        '
        'rfsgStatusTimer
        '
        '
        'rfsgMasterResourceNameLabel
        '
        Me.rfsgMasterResourceNameLabel.AutoSize = True
        Me.rfsgMasterResourceNameLabel.Location = New System.Drawing.Point(12, 9)
        Me.rfsgMasterResourceNameLabel.Name = "rfsgMasterResourceNameLabel"
        Me.rfsgMasterResourceNameLabel.Size = New System.Drawing.Size(119, 13)
        Me.rfsgMasterResourceNameLabel.TabIndex = 31
        Me.rfsgMasterResourceNameLabel.Text = "Master Resource Name"
        '
        'rfsgSlaveResourceNamesLabel
        '
        Me.rfsgSlaveResourceNamesLabel.AutoSize = True
        Me.rfsgSlaveResourceNamesLabel.Location = New System.Drawing.Point(12, 58)
        Me.rfsgSlaveResourceNamesLabel.Name = "rfsgSlaveResourceNamesLabel"
        Me.rfsgSlaveResourceNamesLabel.Size = New System.Drawing.Size(237, 13)
        Me.rfsgSlaveResourceNamesLabel.TabIndex = 38
        Me.rfsgSlaveResourceNamesLabel.Text = "Rfsg Slave Resource Names (comma separated)"
        '
        'slaveRfsgResourceNamesTextBox
        '
        Me.slaveRfsgResourceNamesTextBox.Location = New System.Drawing.Point(12, 76)
        Me.slaveRfsgResourceNamesTextBox.Name = "slaveRfsgResourceNamesTextBox"
        Me.slaveRfsgResourceNamesTextBox.Size = New System.Drawing.Size(121, 20)
        Me.slaveRfsgResourceNamesTextBox.TabIndex = 1
        Me.slaveRfsgResourceNamesTextBox.Text = "Slave"
        '
        'scriptRichTextBox
        '
        Me.scriptRichTextBox.Location = New System.Drawing.Point(12, 310)
        Me.scriptRichTextBox.Name = "scriptRichTextBox"
        Me.scriptRichTextBox.Size = New System.Drawing.Size(502, 153)
        Me.scriptRichTextBox.TabIndex = 5
        Me.scriptRichTextBox.Text = resources.GetString("scriptRichTextBox.Text")
        '
        'scriptLabel
        '
        Me.scriptLabel.AutoSize = True
        Me.scriptLabel.Location = New System.Drawing.Point(12, 294)
        Me.scriptLabel.Name = "scriptLabel"
        Me.scriptLabel.Size = New System.Drawing.Size(34, 13)
        Me.scriptLabel.TabIndex = 4
        Me.scriptLabel.Text = "Script"
        '
        'startButton
        '
        Me.startButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.startButton.Location = New System.Drawing.Point(221, 559)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 8
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(12, 470)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 6
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'textmessageLabel
        '
        Me.textmessageLabel.Location = New System.Drawing.Point(255, 9)
        Me.textmessageLabel.Name = "textmessageLabel"
        Me.textmessageLabel.Size = New System.Drawing.Size(269, 58)
        Me.textmessageLabel.TabIndex = 29
        Me.textmessageLabel.Text = "The master NI 5673 should have a LO and AWG associated with it in Measurement and" & _
            " Automation Explorer (MAX). Slave devices should have an AWG, but an External LO" & _
            "."
        '
        'masterRfsgResourceNameComboBox
        '
        Me.masterRfsgResourceNameComboBox.FormattingEnabled = True
        Me.masterRfsgResourceNameComboBox.Location = New System.Drawing.Point(12, 27)
        Me.masterRfsgResourceNameComboBox.Name = "masterRfsgResourceNameComboBox"
        Me.masterRfsgResourceNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.masterRfsgResourceNameComboBox.TabIndex = 0
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(12, 486)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(502, 67)
        Me.errorTextBox.TabIndex = 7
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(537, 589)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.softwareTriggerButton)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.frequencyReferenceGroupBox)
        Me.Controls.Add(Me.rfsgMasterResourceNameLabel)
        Me.Controls.Add(Me.rfsgSlaveResourceNamesLabel)
        Me.Controls.Add(Me.slaveRfsgResourceNamesTextBox)
        Me.Controls.Add(Me.scriptRichTextBox)
        Me.Controls.Add(Me.scriptLabel)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.textmessageLabel)
        Me.Controls.Add(Me.masterRfsgResourceNameComboBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "5673 Synchronization (TClk, Shared LO, Script)"
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.frequencyReferenceGroupBox.ResumeLayout(False)
        Me.frequencyReferenceGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents configurationGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents frequencyNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents powerLevelLabel As System.Windows.Forms.Label
    Private WithEvents centerFrequencyLabel As System.Windows.Forms.Label
    Private WithEvents iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents iqrateLabel As System.Windows.Forms.Label
    Private WithEvents softwareTriggerButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents frequencyReferenceGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents slaveReferenceClockOutputTerminalComboBox As System.Windows.Forms.ComboBox
    Private WithEvents masterReferenceClockSourceComboBox As System.Windows.Forms.ComboBox
    Private WithEvents slaveReferenceClockSourceComboBox As System.Windows.Forms.ComboBox
    Private WithEvents masterReferenceClockOutputTerminalComboBox As System.Windows.Forms.ComboBox
    Private WithEvents masterReferenceClockSourceLabel As System.Windows.Forms.Label
    Private WithEvents slaveReferenceClockSourceLabel As System.Windows.Forms.Label
    Private WithEvents masterReferenceClockOutputTerminalLabel As System.Windows.Forms.Label
    Private WithEvents slaveReferenceClockOutputTerminalLabel As System.Windows.Forms.Label
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
    Private WithEvents rfsgMasterResourceNameLabel As System.Windows.Forms.Label
    Private WithEvents rfsgSlaveResourceNamesLabel As System.Windows.Forms.Label
    Private WithEvents slaveRfsgResourceNamesTextBox As System.Windows.Forms.TextBox
    Private WithEvents scriptRichTextBox As System.Windows.Forms.RichTextBox
    Private WithEvents scriptLabel As System.Windows.Forms.Label
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents errorLabel As System.Windows.Forms.Label
    Private WithEvents textmessageLabel As System.Windows.Forms.Label
    Private WithEvents masterRfsgResourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents errorTextBox As System.Windows.Forms.TextBox
	#End Region


End Class
