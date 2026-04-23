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
        Me.actualIQRateTextBox = New System.Windows.Forms.TextBox()
        Me.actualFrequencyOffsetTextBox = New System.Windows.Forms.TextBox()
        Me.actualIQRateLabel = New System.Windows.Forms.Label()
        Me.actualFrequencyOffsetLabel = New System.Windows.Forms.Label()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.referenceClockComboBox = New System.Windows.Forms.ComboBox()
        Me.referenceClockLabel = New System.Windows.Forms.Label()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqOutPortLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqPortFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqPortFrequencyLabel = New System.Windows.Forms.Label()
        Me.iqOutPortLevelLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.scriptLabel = New System.Windows.Forms.Label()
        Me.textmessageLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.scriptTextBox = New System.Windows.Forms.TextBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.measurementGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqOutPortLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqPortFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyOffsetTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyOffsetLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(337, 197)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(154, 131)
        Me.measurementGroupBox.TabIndex = 15
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'actualIQRateTextBox
        '
        Me.actualIQRateTextBox.Location = New System.Drawing.Point(21, 101)
        Me.actualIQRateTextBox.Name = "actualIQRateTextBox"
        Me.actualIQRateTextBox.ReadOnly = True
        Me.actualIQRateTextBox.Size = New System.Drawing.Size(109, 20)
        Me.actualIQRateTextBox.TabIndex = 11
        Me.actualIQRateTextBox.TabStop = False
        Me.actualIQRateTextBox.Text = "0.000000"
        '
        'actualFrequencyOffsetTextBox
        '
        Me.actualFrequencyOffsetTextBox.Location = New System.Drawing.Point(21, 46)
        Me.actualFrequencyOffsetTextBox.Name = "actualFrequencyOffsetTextBox"
        Me.actualFrequencyOffsetTextBox.ReadOnly = True
        Me.actualFrequencyOffsetTextBox.Size = New System.Drawing.Size(109, 20)
        Me.actualFrequencyOffsetTextBox.TabIndex = 10
        Me.actualFrequencyOffsetTextBox.TabStop = False
        Me.actualFrequencyOffsetTextBox.Text = "0.000000"
        '
        'actualIQRateLabel
        '
        Me.actualIQRateLabel.AutoSize = True
        Me.actualIQRateLabel.Location = New System.Drawing.Point(21, 80)
        Me.actualIQRateLabel.Name = "actualIQRateLabel"
        Me.actualIQRateLabel.Size = New System.Drawing.Size(103, 13)
        Me.actualIQRateLabel.TabIndex = 7
        Me.actualIQRateLabel.Text = "Actual IQ Rate [S/s]"
        '
        'actualFrequencyOffsetLabel
        '
        Me.actualFrequencyOffsetLabel.AutoSize = True
        Me.actualFrequencyOffsetLabel.Location = New System.Drawing.Point(21, 25)
        Me.actualFrequencyOffsetLabel.Name = "actualFrequencyOffsetLabel"
        Me.actualFrequencyOffsetLabel.Size = New System.Drawing.Size(110, 13)
        Me.actualFrequencyOffsetLabel.TabIndex = 6
        Me.actualFrequencyOffsetLabel.Text = "Frequency Offset [Hz]"
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.referenceClockComboBox)
        Me.configurationGroupBox.Controls.Add(Me.referenceClockLabel)
        Me.configurationGroupBox.Controls.Add(Me.iqRateNumeric)
        Me.configurationGroupBox.Controls.Add(Me.iqOutPortLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.iqPortFrequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.iqPortFrequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.iqOutPortLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.iqRateLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 197)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(309, 131)
        Me.configurationGroupBox.TabIndex = 2
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'referenceClockComboBox
        '
        Me.referenceClockComboBox.Location = New System.Drawing.Point(21, 47)
        Me.referenceClockComboBox.Name = "referenceClockComboBox"
        Me.referenceClockComboBox.Size = New System.Drawing.Size(110, 21)
        Me.referenceClockComboBox.TabIndex = 1
        '
        'referenceClockLabel
        '
        Me.referenceClockLabel.AutoSize = True
        Me.referenceClockLabel.Location = New System.Drawing.Point(21, 26)
        Me.referenceClockLabel.Name = "referenceClockLabel"
        Me.referenceClockLabel.Size = New System.Drawing.Size(71, 13)
        Me.referenceClockLabel.TabIndex = 30
        Me.referenceClockLabel.Text = "Clock Source"
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 6
        Me.iqRateNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.iqRateNumeric.Location = New System.Drawing.Point(168, 100)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqRateNumeric.TabIndex = 4
        Me.iqRateNumeric.Value = New Decimal(New Integer() {5000000, 0, 0, 0})
        '
        'iqOutPortLevelNumeric
        '
        Me.iqOutPortLevelNumeric.DecimalPlaces = 3
        Me.iqOutPortLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.iqOutPortLevelNumeric.Location = New System.Drawing.Point(168, 47)
        Me.iqOutPortLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqOutPortLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqOutPortLevelNumeric.Name = "iqOutPortLevelNumeric"
        Me.iqOutPortLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqOutPortLevelNumeric.TabIndex = 3
        Me.iqOutPortLevelNumeric.Value = New Decimal(New Integer() {5, 0, 0, 65536})
        '
        'iqPortFrequencyNumeric
        '
        Me.iqPortFrequencyNumeric.DecimalPlaces = 6
        Me.iqPortFrequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.iqPortFrequencyNumeric.Location = New System.Drawing.Point(21, 100)
        Me.iqPortFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqPortFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqPortFrequencyNumeric.Name = "iqPortFrequencyNumeric"
        Me.iqPortFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqPortFrequencyNumeric.TabIndex = 2
        '
        'iqPortFrequencyLabel
        '
        Me.iqPortFrequencyLabel.AutoSize = True
        Me.iqPortFrequencyLabel.Location = New System.Drawing.Point(21, 79)
        Me.iqPortFrequencyLabel.Name = "iqPortFrequencyLabel"
        Me.iqPortFrequencyLabel.Size = New System.Drawing.Size(115, 13)
        Me.iqPortFrequencyLabel.TabIndex = 2
        Me.iqPortFrequencyLabel.Text = "IQ Port Frequency (Hz)"
        '
        'iqOutPortLevelLabel
        '
        Me.iqOutPortLevelLabel.AutoSize = True
        Me.iqOutPortLevelLabel.Location = New System.Drawing.Point(168, 26)
        Me.iqOutPortLevelLabel.Name = "iqOutPortLevelLabel"
        Me.iqOutPortLevelLabel.Size = New System.Drawing.Size(117, 13)
        Me.iqOutPortLevelLabel.TabIndex = 3
        Me.iqOutPortLevelLabel.Text = "IQ Out Port Level (Vpp)"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(168, 79)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
        Me.iqRateLabel.TabIndex = 4
        Me.iqRateLabel.Text = "IQ Rate [S/s]"
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
        'scriptLabel
        '
        Me.scriptLabel.AutoSize = True
        Me.scriptLabel.Location = New System.Drawing.Point(12, 64)
        Me.scriptLabel.Name = "scriptLabel"
        Me.scriptLabel.Size = New System.Drawing.Size(34, 13)
        Me.scriptLabel.TabIndex = 17
        Me.scriptLabel.Text = "Script"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(12, 344)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 22
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 30)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'scriptTextBox
        '
        Me.scriptTextBox.Location = New System.Drawing.Point(12, 86)
        Me.scriptTextBox.Multiline = True
        Me.scriptTextBox.Name = "scriptTextBox"
        Me.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.scriptTextBox.Size = New System.Drawing.Size(309, 91)
        Me.scriptTextBox.TabIndex = 1
        Me.scriptTextBox.Text = "script simple" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    repeat forever" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       generate positiveOffset" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       generat" & _
    "e negativeOffset" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    end repeat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " end script"
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(12, 365)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(478, 34)
        Me.errorTextBox.TabIndex = 23
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(337, 28)
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
        Me.stopButton.Location = New System.Drawing.Point(415, 28)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 4
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'rfsgStatusTimer
        '
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(520, 449)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.scriptLabel)
        Me.Controls.Add(Me.textmessageLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.scriptTextBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.Text = "Simple Script IQ Device"
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqOutPortLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqPortFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

	#End Region

	Private measurementGroupBox As System.Windows.Forms.GroupBox
	Private actualIQRateTextBox As System.Windows.Forms.TextBox
	Private actualFrequencyOffsetTextBox As System.Windows.Forms.TextBox
	Private actualIQRateLabel As System.Windows.Forms.Label
	Private actualFrequencyOffsetLabel As System.Windows.Forms.Label
	Private configurationGroupBox As System.Windows.Forms.GroupBox
	Private iqRateNumeric As System.Windows.Forms.NumericUpDown
	Private iqOutPortLevelNumeric As System.Windows.Forms.NumericUpDown
	Private iqPortFrequencyNumeric As System.Windows.Forms.NumericUpDown
	Private iqPortFrequencyLabel As System.Windows.Forms.Label
	Private iqOutPortLevelLabel As System.Windows.Forms.Label
	Private iqRateLabel As System.Windows.Forms.Label
	Private resourceNameLabel As System.Windows.Forms.Label
	Private scriptLabel As System.Windows.Forms.Label
	Private textmessageLabel As System.Windows.Forms.Label
	Private errorLabel As System.Windows.Forms.Label
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private scriptTextBox As System.Windows.Forms.TextBox
	Private errorTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
	Private referenceClockComboBox As System.Windows.Forms.ComboBox
	Private referenceClockLabel As System.Windows.Forms.Label

End Class

