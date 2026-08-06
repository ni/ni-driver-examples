Partial Class MainForm
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Me.actualFrequencyOffsetTextBox = New System.Windows.Forms.TextBox()
        Me.triggerButton = New System.Windows.Forms.Button()
        Me.triggerControlGroupBox = New System.Windows.Forms.GroupBox()
        Me.trigger2Button = New System.Windows.Forms.Button()
        Me.readyLed = New System.Windows.Forms.Button()
        Me.readyLabel = New System.Windows.Forms.Label()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.actualIQRateTextBox = New System.Windows.Forms.TextBox()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.actualIQRateLabel = New System.Windows.Forms.Label()
        Me.actualFrequencyOffsetLabel = New System.Windows.Forms.Label()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.frequencyLabel = New System.Windows.Forms.Label()
        Me.scriptLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.textmessageLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.scriptIndexNumeric = New System.Windows.Forms.NumericUpDown()
        Me.scriptTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.triggerControlGroupBox.SuspendLayout()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementGroupBox.SuspendLayout()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.scriptIndexNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'actualFrequencyOffsetTextBox
        '
        Me.actualFrequencyOffsetTextBox.Location = New System.Drawing.Point(19, 46)
        Me.actualFrequencyOffsetTextBox.Name = "actualFrequencyOffsetTextBox"
        Me.actualFrequencyOffsetTextBox.ReadOnly = True
        Me.actualFrequencyOffsetTextBox.Size = New System.Drawing.Size(151, 20)
        Me.actualFrequencyOffsetTextBox.TabIndex = 12
        Me.actualFrequencyOffsetTextBox.TabStop = False
        Me.actualFrequencyOffsetTextBox.Text = "100000000.000000"
        '
        'triggerButton
        '
        Me.triggerButton.Location = New System.Drawing.Point(12, 57)
        Me.triggerButton.Name = "triggerButton"
        Me.triggerButton.Size = New System.Drawing.Size(165, 23)
        Me.triggerButton.TabIndex = 0
        Me.triggerButton.Text = "&Send Software scriptTrigger0"
        Me.triggerButton.UseVisualStyleBackColor = True
        '
        'triggerControlGroupBox
        '
        Me.triggerControlGroupBox.Controls.Add(Me.triggerButton)
        Me.triggerControlGroupBox.Controls.Add(Me.trigger2Button)
        Me.triggerControlGroupBox.Controls.Add(Me.readyLed)
        Me.triggerControlGroupBox.Controls.Add(Me.readyLabel)
        Me.triggerControlGroupBox.Location = New System.Drawing.Point(337, 82)
        Me.triggerControlGroupBox.Name = "triggerControlGroupBox"
        Me.triggerControlGroupBox.Size = New System.Drawing.Size(191, 120)
        Me.triggerControlGroupBox.TabIndex = 20
        Me.triggerControlGroupBox.TabStop = False
        Me.triggerControlGroupBox.Text = "Trigger Control"
        '
        'trigger2Button
        '
        Me.trigger2Button.Location = New System.Drawing.Point(12, 88)
        Me.trigger2Button.Name = "trigger2Button"
        Me.trigger2Button.Size = New System.Drawing.Size(165, 23)
        Me.trigger2Button.TabIndex = 1
        Me.trigger2Button.Text = "&Send Software scriptTrigger1"
        Me.trigger2Button.UseVisualStyleBackColor = True
        '
        'readyLed
        '
        Me.readyLed.BackColor = System.Drawing.SystemColors.Control
        Me.readyLed.Enabled = False
        Me.readyLed.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.readyLed.Location = New System.Drawing.Point(33, 19)
        Me.readyLed.Name = "readyLed"
        Me.readyLed.Size = New System.Drawing.Size(25, 23)
        Me.readyLed.TabIndex = 20
        Me.readyLed.TabStop = False
        Me.readyLed.UseVisualStyleBackColor = False
        '
        'readyLabel
        '
        Me.readyLabel.AutoSize = True
        Me.readyLabel.Location = New System.Drawing.Point(64, 24)
        Me.readyLabel.Name = "readyLabel"
        Me.readyLabel.Size = New System.Drawing.Size(89, 13)
        Me.readyLabel.TabIndex = 19
        Me.readyLabel.Text = "Ready for Trigger"
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 6
        Me.iqRateNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.iqRateNumeric.Location = New System.Drawing.Point(17, 96)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqRateNumeric.TabIndex = 3
        Me.iqRateNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'actualIQRateTextBox
        '
        Me.actualIQRateTextBox.Location = New System.Drawing.Point(19, 97)
        Me.actualIQRateTextBox.Name = "actualIQRateTextBox"
        Me.actualIQRateTextBox.ReadOnly = True
        Me.actualIQRateTextBox.Size = New System.Drawing.Size(151, 20)
        Me.actualIQRateTextBox.TabIndex = 14
        Me.actualIQRateTextBox.TabStop = False
        Me.actualIQRateTextBox.Text = "100000.000000"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyOffsetTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyOffsetLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(337, 252)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(191, 126)
        Me.measurementGroupBox.TabIndex = 22
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'actualIQRateLabel
        '
        Me.actualIQRateLabel.AutoSize = True
        Me.actualIQRateLabel.Location = New System.Drawing.Point(19, 76)
        Me.actualIQRateLabel.Name = "actualIQRateLabel"
        Me.actualIQRateLabel.Size = New System.Drawing.Size(103, 13)
        Me.actualIQRateLabel.TabIndex = 8
        Me.actualIQRateLabel.Text = "Actual IQ Rate [S/s]"
        '
        'actualFrequencyOffsetLabel
        '
        Me.actualFrequencyOffsetLabel.AutoSize = True
        Me.actualFrequencyOffsetLabel.Location = New System.Drawing.Point(19, 25)
        Me.actualFrequencyOffsetLabel.Name = "actualFrequencyOffsetLabel"
        Me.actualFrequencyOffsetLabel.Size = New System.Drawing.Size(110, 13)
        Me.actualFrequencyOffsetLabel.TabIndex = 6
        Me.actualFrequencyOffsetLabel.Text = "Frequency Offset [Hz]"
        '
        'rfsgStatusTimer
        '
        Me.rfsgStatusTimer.Interval = 1
        '
        'powerLevelNumeric
        '
        Me.powerLevelNumeric.DecimalPlaces = 2
        Me.powerLevelNumeric.Location = New System.Drawing.Point(166, 45)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerLevelNumeric.TabIndex = 4
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.powerLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.iqRateNumeric)
        Me.configurationGroupBox.Controls.Add(Me.frequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.iqRateLabel)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.frequencyLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(20, 252)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(302, 126)
        Me.configurationGroupBox.TabIndex = 19
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'frequencyNumeric
        '
        Me.frequencyNumeric.DecimalPlaces = 6
        Me.frequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.frequencyNumeric.Location = New System.Drawing.Point(18, 45)
        Me.frequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.frequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.frequencyNumeric.Name = "frequencyNumeric"
        Me.frequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.frequencyNumeric.TabIndex = 2
        Me.frequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(15, 76)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
        Me.iqRateLabel.TabIndex = 4
        Me.iqRateLabel.Text = "IQ Rate [S/s]"
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(166, 24)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.powerLevelLabel.TabIndex = 3
        Me.powerLevelLabel.Text = "Power Level [dBm]"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.AutoSize = True
        Me.frequencyLabel.Location = New System.Drawing.Point(15, 25)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.frequencyLabel.TabIndex = 2
        Me.frequencyLabel.Text = "Center Frequency [Hz]"
        '
        'scriptLabel
        '
        Me.scriptLabel.AutoSize = True
        Me.scriptLabel.Location = New System.Drawing.Point(20, 66)
        Me.scriptLabel.Name = "scriptLabel"
        Me.scriptLabel.Size = New System.Drawing.Size(34, 13)
        Me.scriptLabel.TabIndex = 14
        Me.scriptLabel.Text = "Script"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(20, 11)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 15
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(22, 401)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 23
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'scriptIndexNumeric
        '
        Me.scriptIndexNumeric.Location = New System.Drawing.Point(20, 83)
        Me.scriptIndexNumeric.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.scriptIndexNumeric.Name = "scriptIndexNumeric"
        Me.scriptIndexNumeric.Size = New System.Drawing.Size(30, 20)
        Me.scriptIndexNumeric.TabIndex = 17
        '
        'scriptTextBox
        '
        Me.scriptTextBox.Location = New System.Drawing.Point(54, 82)
        Me.scriptTextBox.Multiline = True
        Me.scriptTextBox.Name = "scriptTextBox"
        Me.scriptTextBox.ReadOnly = True
        Me.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.scriptTextBox.Size = New System.Drawing.Size(268, 120)
        Me.scriptTextBox.TabIndex = 18
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(20, 32)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 16
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(20, 420)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(508, 34)
        Me.errorTextBox.TabIndex = 27
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(372, 30)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 24
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(453, 30)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 25
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(545, 495)
        Me.Controls.Add(Me.triggerControlGroupBox)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.scriptLabel)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.textmessageLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.scriptIndexNumeric)
        Me.Controls.Add(Me.scriptTextBox)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Script Trigger - Software Source"
        Me.triggerControlGroupBox.ResumeLayout(False)
        Me.triggerControlGroupBox.PerformLayout()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.scriptIndexNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region
    Private WithEvents actualFrequencyOffsetTextBox As System.Windows.Forms.TextBox
    Private WithEvents triggerButton As System.Windows.Forms.Button
    Private WithEvents triggerControlGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents trigger2Button As System.Windows.Forms.Button
    Private WithEvents readyLed As System.Windows.Forms.Button
    Private WithEvents readyLabel As System.Windows.Forms.Label
    Private WithEvents iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents actualIQRateTextBox As System.Windows.Forms.TextBox
    Private WithEvents measurementGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents actualIQRateLabel As System.Windows.Forms.Label
    Private WithEvents actualFrequencyOffsetLabel As System.Windows.Forms.Label
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
    Private WithEvents powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents configurationGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents frequencyNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents iqRateLabel As System.Windows.Forms.Label
    Private WithEvents powerLevelLabel As System.Windows.Forms.Label
    Private WithEvents frequencyLabel As System.Windows.Forms.Label
    Private WithEvents scriptLabel As System.Windows.Forms.Label
    Private WithEvents resourceNameLabel As System.Windows.Forms.Label
    Private WithEvents textmessageLabel As System.Windows.Forms.Label
    Private WithEvents errorLabel As System.Windows.Forms.Label
    Private WithEvents scriptIndexNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents scriptTextBox As System.Windows.Forms.TextBox
    Private WithEvents resourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents errorTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.ButtonEnd Class
