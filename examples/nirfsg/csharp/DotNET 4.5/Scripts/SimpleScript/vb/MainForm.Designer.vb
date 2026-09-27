Imports System	Partial Class MainForm
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
        Me.scriptLabel = New System.Windows.Forms.Label()
        Me.frequencyLabel = New System.Windows.Forms.Label()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.textmessageLabel = New System.Windows.Forms.Label()
        Me.actualFrequencyOffsetLabel = New System.Windows.Forms.Label()
        Me.actualIQRateLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.scriptTextBox = New System.Windows.Forms.TextBox()
        Me.actualFrequencyOffsetTextBox = New System.Windows.Forms.TextBox()
        Me.actualIQRateTextBox = New System.Windows.Forms.TextBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.configurationGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(20, 16)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'scriptLabel
        '
        Me.scriptLabel.AutoSize = True
        Me.scriptLabel.Location = New System.Drawing.Point(20, 71)
        Me.scriptLabel.Name = "scriptLabel"
        Me.scriptLabel.Size = New System.Drawing.Size(34, 13)
        Me.scriptLabel.TabIndex = 1
        Me.scriptLabel.Text = "Script"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.AutoSize = True
        Me.frequencyLabel.Location = New System.Drawing.Point(21, 26)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.frequencyLabel.TabIndex = 2
        Me.frequencyLabel.Text = "Center Frequency [Hz]"
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(168, 26)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.powerLevelLabel.TabIndex = 3
        Me.powerLevelLabel.Text = "Power Level [dBm]"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(21, 79)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
        Me.iqRateLabel.TabIndex = 4
        Me.iqRateLabel.Text = "IQ Rate [S/s]"
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
        'actualIQRateLabel
        '
        Me.actualIQRateLabel.AutoSize = True
        Me.actualIQRateLabel.Location = New System.Drawing.Point(21, 80)
        Me.actualIQRateLabel.Name = "actualIQRateLabel"
        Me.actualIQRateLabel.Size = New System.Drawing.Size(103, 13)
        Me.actualIQRateLabel.TabIndex = 7
        Me.actualIQRateLabel.Text = "Actual IQ Rate [S/s]"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(20, 351)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 8
        Me.errorLabel.Text = "Warning/Error Message"
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
        Me.frequencyNumeric.TabIndex = 2
        Me.frequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'powerLevelNumeric
        '
        Me.powerLevelNumeric.DecimalPlaces = 2
        Me.powerLevelNumeric.Location = New System.Drawing.Point(168, 47)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerLevelNumeric.TabIndex = 4
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 6
        Me.iqRateNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.iqRateNumeric.Location = New System.Drawing.Point(21, 100)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqRateNumeric.TabIndex = 3
        Me.iqRateNumeric.Value = New Decimal(New Integer() {5000000, 0, 0, 0})
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(20, 37)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'scriptTextBox
        '
        Me.scriptTextBox.Location = New System.Drawing.Point(20, 93)
        Me.scriptTextBox.Multiline = True
        Me.scriptTextBox.Name = "scriptTextBox"
        Me.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.scriptTextBox.Size = New System.Drawing.Size(309, 91)
        Me.scriptTextBox.TabIndex = 1
        Me.scriptTextBox.Text = "script simple" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    repeat forever" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       generate positiveOffset" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       generat" & _
            "e negativeOffset" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    end repeat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " end script"
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
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(20, 372)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(478, 34)
        Me.errorTextBox.TabIndex = 12
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(345, 35)
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
        Me.stopButton.Location = New System.Drawing.Point(423, 35)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 4
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
        Me.configurationGroupBox.Controls.Add(Me.frequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.frequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.iqRateLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(20, 204)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(309, 131)
        Me.configurationGroupBox.TabIndex = 2
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyOffsetTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyOffsetLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(345, 204)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(154, 131)
        Me.measurementGroupBox.TabIndex = 0
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
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
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Simple Script"
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
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
		Private scriptLabel As System.Windows.Forms.Label
		Private frequencyLabel As System.Windows.Forms.Label
		Private powerLevelLabel As System.Windows.Forms.Label
		Private iqRateLabel As System.Windows.Forms.Label
		Private textmessageLabel As System.Windows.Forms.Label
		Private actualFrequencyOffsetLabel As System.Windows.Forms.Label
		Private actualIQRateLabel As System.Windows.Forms.Label
		Private errorLabel As System.Windows.Forms.Label
    Private frequencyNumeric As System.Windows.Forms.NumericUpDown
		Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
		Private iqRateNumeric As System.Windows.Forms.NumericUpDown
		Private resourceNameComboBox As System.Windows.Forms.ComboBox
		Private scriptTextBox As System.Windows.Forms.TextBox
		Private actualFrequencyOffsetTextBox As System.Windows.Forms.TextBox
		Private actualIQRateTextBox As System.Windows.Forms.TextBox
		Private errorTextBox As System.Windows.Forms.TextBox
		Private WithEvents startButton As System.Windows.Forms.Button
		Private WithEvents stopButton As System.Windows.Forms.Button    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
		Private configurationGroupBox As System.Windows.Forms.GroupBox
		Private measurementGroupBox As System.Windows.Forms.GroupBox

	End Class
