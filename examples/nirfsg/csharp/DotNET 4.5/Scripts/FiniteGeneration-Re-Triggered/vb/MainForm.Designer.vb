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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.components = New System.ComponentModel.Container()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.frequencyLabel = New System.Windows.Forms.Label()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.durationLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.actualDurationLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.actualIQRateLabel = New System.Windows.Forms.Label()
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.durationNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.actualDurationTextBox = New System.Windows.Forms.TextBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.actualIQRateTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.triggerButton = New System.Windows.Forms.Button()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.readyLabel = New System.Windows.Forms.Label()
        Me.readyLed = New System.Windows.Forms.Button()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.durationNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.configurationGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(19, 18)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.AutoSize = True
        Me.frequencyLabel.Location = New System.Drawing.Point(20, 26)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.frequencyLabel.TabIndex = 1
        Me.frequencyLabel.Text = "Center Frequency [Hz]"
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(20, 77)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.powerLevelLabel.TabIndex = 2
        Me.powerLevelLabel.Text = "Power Level [dBm]"
        '
        'durationLabel
        '
        Me.durationLabel.AutoSize = True
        Me.durationLabel.Location = New System.Drawing.Point(20, 130)
        Me.durationLabel.Name = "durationLabel"
        Me.durationLabel.Size = New System.Drawing.Size(113, 13)
        Me.durationLabel.TabIndex = 3
        Me.durationLabel.Text = "Waveform Duration [s]"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(20, 180)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
        Me.iqRateLabel.TabIndex = 4
        Me.iqRateLabel.Text = "IQ Rate [S/s]"
        '
        'actualDurationLabel
        '
        Me.actualDurationLabel.AutoSize = True
        Me.actualDurationLabel.Location = New System.Drawing.Point(23, 28)
        Me.actualDurationLabel.Name = "actualDurationLabel"
        Me.actualDurationLabel.Size = New System.Drawing.Size(146, 13)
        Me.actualDurationLabel.TabIndex = 5
        Me.actualDurationLabel.Text = "Actual Waveform Duration [s]"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(19, 323)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 6
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'actualIQRateLabel
        '
        Me.actualIQRateLabel.AutoSize = True
        Me.actualIQRateLabel.Location = New System.Drawing.Point(23, 78)
        Me.actualIQRateLabel.Name = "actualIQRateLabel"
        Me.actualIQRateLabel.Size = New System.Drawing.Size(103, 13)
        Me.actualIQRateLabel.TabIndex = 7
        Me.actualIQRateLabel.Text = "Actual IQ Rate [S/s]"
        '
        'frequencyNumeric
        '
        Me.frequencyNumeric.DecimalPlaces = 6
        Me.frequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.frequencyNumeric.Location = New System.Drawing.Point(20, 47)
        Me.frequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.frequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.frequencyNumeric.Name = "frequencyNumeric"
        Me.frequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.frequencyNumeric.TabIndex = 1
        Me.frequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'powerLevelNumeric
        '
        Me.powerLevelNumeric.DecimalPlaces = 2
        Me.powerLevelNumeric.Location = New System.Drawing.Point(20, 98)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerLevelNumeric.TabIndex = 2
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'durationNumeric
        '
        Me.durationNumeric.DecimalPlaces = 6
        Me.durationNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.durationNumeric.Location = New System.Drawing.Point(20, 151)
        Me.durationNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.durationNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.durationNumeric.Name = "durationNumeric"
        Me.durationNumeric.Size = New System.Drawing.Size(120, 20)
        Me.durationNumeric.TabIndex = 3
        Me.durationNumeric.Value = New Decimal(New Integer() {5, 0, 0, 65536})
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 6
        Me.iqRateNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.iqRateNumeric.Location = New System.Drawing.Point(20, 201)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqRateNumeric.TabIndex = 4
        Me.iqRateNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(19, 39)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'actualDurationTextBox
        '
        Me.actualDurationTextBox.Location = New System.Drawing.Point(23, 49)
        Me.actualDurationTextBox.Name = "actualDurationTextBox"
        Me.actualDurationTextBox.ReadOnly = True
        Me.actualDurationTextBox.Size = New System.Drawing.Size(151, 20)
        Me.actualDurationTextBox.TabIndex = 11
        Me.actualDurationTextBox.TabStop = False
        Me.actualDurationTextBox.Text = "0.000000"
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(19, 339)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(377, 48)
        Me.errorTextBox.TabIndex = 12
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'actualIQRateTextBox
        '
        Me.actualIQRateTextBox.Location = New System.Drawing.Point(23, 99)
        Me.actualIQRateTextBox.Name = "actualIQRateTextBox"
        Me.actualIQRateTextBox.ReadOnly = True
        Me.actualIQRateTextBox.Size = New System.Drawing.Size(151, 20)
        Me.actualIQRateTextBox.TabIndex = 13
        Me.actualIQRateTextBox.TabStop = False
        Me.actualIQRateTextBox.Text = "0.000000"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(241, 37)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 4
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(322, 37)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 5
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'triggerButton
        '
        Me.triggerButton.Enabled = False
        Me.triggerButton.Location = New System.Drawing.Point(241, 79)
        Me.triggerButton.Name = "triggerButton"
        Me.triggerButton.Size = New System.Drawing.Size(156, 23)
        Me.triggerButton.TabIndex = 3
        Me.triggerButton.Text = "&Send Software scriptTrigger0"
        Me.triggerButton.UseVisualStyleBackColor = True
        '
        'rfsgStatusTimer
        '
        Me.rfsgStatusTimer.Interval = 1
        '
        'readyLabel
        '
        Me.readyLabel.AutoSize = True
        Me.readyLabel.Location = New System.Drawing.Point(267, 128)
        Me.readyLabel.Name = "readyLabel"
        Me.readyLabel.Size = New System.Drawing.Size(89, 13)
        Me.readyLabel.TabIndex = 16
        Me.readyLabel.Text = "Ready for Trigger"
        '
        'readyLed
        '
        Me.readyLed.BackColor = System.Drawing.SystemColors.Control
        Me.readyLed.Enabled = False
        Me.readyLed.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.readyLed.Location = New System.Drawing.Point(241, 124)
        Me.readyLed.Name = "readyLed"
        Me.readyLed.Size = New System.Drawing.Size(20, 20)
        Me.readyLed.TabIndex = 17
        Me.readyLed.TabStop = False
        Me.readyLed.UseVisualStyleBackColor = False
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.iqRateNumeric)
        Me.configurationGroupBox.Controls.Add(Me.durationNumeric)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.frequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.iqRateLabel)
        Me.configurationGroupBox.Controls.Add(Me.frequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.durationLabel)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(19, 79)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(166, 229)
        Me.configurationGroupBox.TabIndex = 2
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualDurationTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualDurationLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(200, 180)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(197, 128)
        Me.measurementGroupBox.TabIndex = 0
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(419, 426)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.readyLabel)
        Me.Controls.Add(Me.readyLed)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.triggerButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Finite Generation - Re-Triggered"
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.durationNumeric, System.ComponentModel.ISupportInitialize).EndInit()
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
    Private frequencyLabel As System.Windows.Forms.Label
    Private powerLevelLabel As System.Windows.Forms.Label
    Private durationLabel As System.Windows.Forms.Label
    Private iqRateLabel As System.Windows.Forms.Label
    Private actualDurationLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private actualIQRateLabel As System.Windows.Forms.Label
    Private frequencyNumeric As System.Windows.Forms.NumericUpDown
    Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private durationNumeric As System.Windows.Forms.NumericUpDown
    Private iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private actualDurationTextBox As System.Windows.Forms.TextBox
    Private errorTextBox As System.Windows.Forms.TextBox
    Private actualIQRateTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents triggerButton As System.Windows.Forms.Button    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
    Private readyLabel As System.Windows.Forms.Label
    Private readyLed As System.Windows.Forms.Button
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private measurementGroupBox As System.Windows.Forms.GroupBox

End Class
