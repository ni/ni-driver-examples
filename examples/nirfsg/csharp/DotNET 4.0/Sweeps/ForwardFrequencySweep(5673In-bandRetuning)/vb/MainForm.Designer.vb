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
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.clockSourceLabel = New System.Windows.Forms.Label()
        Me.dwellTimeLabel = New System.Windows.Forms.Label()
        Me.numberStepsLabel = New System.Windows.Forms.Label()
        Me.deviceBandwidthToUseLabel = New System.Windows.Forms.Label()
        Me.loopBandwidthLabel = New System.Windows.Forms.Label()
        Me.stopFrequencyLabel = New System.Windows.Forms.Label()
        Me.actualCurrentFrequencyLabel = New System.Windows.Forms.Label()
        Me.startFrequencyLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.dwellTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.numberStepsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.deviceBandwidthToUseNumeric = New System.Windows.Forms.NumericUpDown()
        Me.stopFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.actualCurrentFrequencyTextBox = New System.Windows.Forms.TextBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.startButton = New System.Windows.Forms.Button()        Me.clockSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.loopBandwidthComboBox = New System.Windows.Forms.ComboBox()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.configurationListParametersGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dwellTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberStepsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.deviceBandwidthToUseNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.stopFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.configurationGroupBox.SuspendLayout()
        Me.configurationListParametersGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(22, 26)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.powerLevelLabel.TabIndex = 0
        Me.powerLevelLabel.Text = "Power Level (dBm)"
        '
        'clockSourceLabel
        '
        Me.clockSourceLabel.AutoSize = True
        Me.clockSourceLabel.Location = New System.Drawing.Point(22, 76)
        Me.clockSourceLabel.Name = "clockSourceLabel"
        Me.clockSourceLabel.Size = New System.Drawing.Size(147, 13)
        Me.clockSourceLabel.TabIndex = 1
        Me.clockSourceLabel.Text = "Frequency Reference Source"
        '
        'dwellTimeLabel
        '
        Me.dwellTimeLabel.AutoSize = True
        Me.dwellTimeLabel.Location = New System.Drawing.Point(18, 177)
        Me.dwellTimeLabel.Name = "dwellTimeLabel"
        Me.dwellTimeLabel.Size = New System.Drawing.Size(117, 13)
        Me.dwellTimeLabel.TabIndex = 2
        Me.dwellTimeLabel.Text = "Dwell Time Per Step (s)"
        '
        'numberStepsLabel
        '
        Me.numberStepsLabel.AutoSize = True
        Me.numberStepsLabel.Location = New System.Drawing.Point(18, 127)
        Me.numberStepsLabel.Name = "numberStepsLabel"
        Me.numberStepsLabel.Size = New System.Drawing.Size(86, 13)
        Me.numberStepsLabel.TabIndex = 3
        Me.numberStepsLabel.Text = "Number of Steps"
        '
        'deviceBandwidthToUseLabel
        '
        Me.deviceBandwidthToUseLabel.AutoSize = True
        Me.deviceBandwidthToUseLabel.Location = New System.Drawing.Point(22, 126)
        Me.deviceBandwidthToUseLabel.Name = "deviceBandwidthToUseLabel"
        Me.deviceBandwidthToUseLabel.Size = New System.Drawing.Size(150, 13)
        Me.deviceBandwidthToUseLabel.TabIndex = 4
        Me.deviceBandwidthToUseLabel.Text = "Device Bandwidth to Use (Hz)"
        '
        'loopBandwidthLabel
        '
        Me.loopBandwidthLabel.AutoSize = True
        Me.loopBandwidthLabel.Location = New System.Drawing.Point(22, 176)
        Me.loopBandwidthLabel.Name = "loopBandwidthLabel"
        Me.loopBandwidthLabel.Size = New System.Drawing.Size(84, 13)
        Me.loopBandwidthLabel.TabIndex = 5
        Me.loopBandwidthLabel.Text = "Loop Bandwidth"
        '
        'stopFrequencyLabel
        '
        Me.stopFrequencyLabel.AutoSize = True
        Me.stopFrequencyLabel.Location = New System.Drawing.Point(18, 77)
        Me.stopFrequencyLabel.Name = "stopFrequencyLabel"
        Me.stopFrequencyLabel.Size = New System.Drawing.Size(104, 13)
        Me.stopFrequencyLabel.TabIndex = 6
        Me.stopFrequencyLabel.Text = "Stop Frequency (Hz)"
        '
        'actualCurrentFrequencyLabel
        '
        Me.actualCurrentFrequencyLabel.AutoSize = True
        Me.actualCurrentFrequencyLabel.Location = New System.Drawing.Point(429, 101)
        Me.actualCurrentFrequencyLabel.Name = "actualCurrentFrequencyLabel"
        Me.actualCurrentFrequencyLabel.Size = New System.Drawing.Size(116, 13)
        Me.actualCurrentFrequencyLabel.TabIndex = 7
        Me.actualCurrentFrequencyLabel.Text = "Current Frequency (Hz)"
        '
        'startFrequencyLabel
        '
        Me.startFrequencyLabel.AutoSize = True
        Me.startFrequencyLabel.Location = New System.Drawing.Point(18, 27)
        Me.startFrequencyLabel.Name = "startFrequencyLabel"
        Me.startFrequencyLabel.Size = New System.Drawing.Size(104, 13)
        Me.startFrequencyLabel.TabIndex = 8
        Me.startFrequencyLabel.Text = "Start Frequency (Hz)"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(13, 319)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 9
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(19, 20)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 11
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'powerLevelNumeric
        '
        Me.powerLevelNumeric.DecimalPlaces = 2
        Me.powerLevelNumeric.Location = New System.Drawing.Point(20, 44)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerLevelNumeric.TabIndex = 0
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'dwellTimeNumeric
        '
        Me.dwellTimeNumeric.DecimalPlaces = 6
        Me.dwellTimeNumeric.Location = New System.Drawing.Point(21, 195)
        Me.dwellTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.dwellTimeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.dwellTimeNumeric.Name = "dwellTimeNumeric"
        Me.dwellTimeNumeric.Size = New System.Drawing.Size(120, 20)
        Me.dwellTimeNumeric.TabIndex = 3
        Me.dwellTimeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        'numberStepsNumeric
        '
        Me.numberStepsNumeric.Location = New System.Drawing.Point(21, 145)
        Me.numberStepsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numberStepsNumeric.Name = "numberStepsNumeric"
        Me.numberStepsNumeric.Size = New System.Drawing.Size(120, 20)
        Me.numberStepsNumeric.TabIndex = 2
        Me.numberStepsNumeric.Value = New Decimal(New Integer() {501, 0, 0, 0})
        '
        'deviceBandwidthToUseNumeric
        '
        Me.deviceBandwidthToUseNumeric.Location = New System.Drawing.Point(20, 144)
        Me.deviceBandwidthToUseNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.deviceBandwidthToUseNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.deviceBandwidthToUseNumeric.Name = "deviceBandwidthToUseNumeric"
        Me.deviceBandwidthToUseNumeric.Size = New System.Drawing.Size(120, 20)
        Me.deviceBandwidthToUseNumeric.TabIndex = 2
        Me.deviceBandwidthToUseNumeric.Value = New Decimal(New Integer() {100000000, 0, 0, 0})
        '
        'stopFrequencyNumeric
        '
        Me.stopFrequencyNumeric.DecimalPlaces = 6
        Me.stopFrequencyNumeric.Location = New System.Drawing.Point(21, 95)
        Me.stopFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.stopFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.stopFrequencyNumeric.Name = "stopFrequencyNumeric"
        Me.stopFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.stopFrequencyNumeric.TabIndex = 1
        Me.stopFrequencyNumeric.Value = New Decimal(New Integer() {2000000000, 0, 0, 0})
        '
        'startFrequencyNumeric
        '
        Me.startFrequencyNumeric.DecimalPlaces = 6
        Me.startFrequencyNumeric.Location = New System.Drawing.Point(21, 45)
        Me.startFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.startFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.startFrequencyNumeric.Name = "startFrequencyNumeric"
        Me.startFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.startFrequencyNumeric.TabIndex = 0
        Me.startFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'actualCurrentFrequencyTextBox
        '
        Me.actualCurrentFrequencyTextBox.Location = New System.Drawing.Point(429, 119)
        Me.actualCurrentFrequencyTextBox.Name = "actualCurrentFrequencyTextBox"
        Me.actualCurrentFrequencyTextBox.ReadOnly = True
        Me.actualCurrentFrequencyTextBox.Size = New System.Drawing.Size(108, 20)
        Me.actualCurrentFrequencyTextBox.TabIndex = 7
        Me.actualCurrentFrequencyTextBox.TabStop = False
        Me.actualCurrentFrequencyTextBox.Text = "0.0000"
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(12, 337)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(525, 34)
        Me.errorTextBox.TabIndex = 11
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(16, 37)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'stopButton
        '
                Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(462, 35)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 4
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(381, 35)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 3
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True
        '        '        '
        'clockSourceComboBox
        '
        Me.clockSourceComboBox.Location = New System.Drawing.Point(20, 94)
        Me.clockSourceComboBox.Name = "clockSourceComboBox"
        Me.clockSourceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.clockSourceComboBox.TabIndex = 1
        '
        'loopBandwidthComboBox
        '
        Me.loopBandwidthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.loopBandwidthComboBox.Location = New System.Drawing.Point(20, 194)
        Me.loopBandwidthComboBox.Name = "loopBandwidthComboBox"
        Me.loopBandwidthComboBox.Size = New System.Drawing.Size(120, 21)
        Me.loopBandwidthComboBox.TabIndex = 3
        '
        'rfsgStatusTimer
        '
        Me.rfsgStatusTimer.Interval = 1000
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.loopBandwidthComboBox)
        Me.configurationGroupBox.Controls.Add(Me.clockSourceComboBox)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.deviceBandwidthToUseNumeric)
        Me.configurationGroupBox.Controls.Add(Me.clockSourceLabel)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.deviceBandwidthToUseLabel)
        Me.configurationGroupBox.Controls.Add(Me.loopBandwidthLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(16, 74)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(189, 224)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'configurationListParametersGroupBox
        '
        Me.configurationListParametersGroupBox.Controls.Add(Me.dwellTimeNumeric)
        Me.configurationListParametersGroupBox.Controls.Add(Me.startFrequencyNumeric)
        Me.configurationListParametersGroupBox.Controls.Add(Me.stopFrequencyNumeric)
        Me.configurationListParametersGroupBox.Controls.Add(Me.numberStepsNumeric)
        Me.configurationListParametersGroupBox.Controls.Add(Me.dwellTimeLabel)
        Me.configurationListParametersGroupBox.Controls.Add(Me.startFrequencyLabel)
        Me.configurationListParametersGroupBox.Controls.Add(Me.numberStepsLabel)
        Me.configurationListParametersGroupBox.Controls.Add(Me.stopFrequencyLabel)
        Me.configurationListParametersGroupBox.Location = New System.Drawing.Point(229, 74)
        Me.configurationListParametersGroupBox.Name = "configurationListParametersGroupBox"
        Me.configurationListParametersGroupBox.Size = New System.Drawing.Size(168, 224)
        Me.configurationListParametersGroupBox.TabIndex = 2
        Me.configurationListParametersGroupBox.TabStop = False
        Me.configurationListParametersGroupBox.Text = "Configuration List Parameters"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True        Me.ClientSize = New System.Drawing.Size(559, 411)
        Me.Controls.Add(Me.configurationListParametersGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.actualCurrentFrequencyLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.actualCurrentFrequencyTextBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Forward Frequency Sweep (5673 In-band Retuning)"
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dwellTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberStepsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.deviceBandwidthToUseNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.stopFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.configurationListParametersGroupBox.ResumeLayout(False)
        Me.configurationListParametersGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private powerLevelLabel As System.Windows.Forms.Label
    Private clockSourceLabel As System.Windows.Forms.Label
    Private dwellTimeLabel As System.Windows.Forms.Label
    Private numberStepsLabel As System.Windows.Forms.Label
    Private deviceBandwidthToUseLabel As System.Windows.Forms.Label
    Private loopBandwidthLabel As System.Windows.Forms.Label
    Private stopFrequencyLabel As System.Windows.Forms.Label
    Private actualCurrentFrequencyLabel As System.Windows.Forms.Label
    Private startFrequencyLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private dwellTimeNumeric As System.Windows.Forms.NumericUpDown
    Private numberStepsNumeric As System.Windows.Forms.NumericUpDown
    Private deviceBandwidthToUseNumeric As System.Windows.Forms.NumericUpDown
    Private stopFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private startFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private actualCurrentFrequencyTextBox As System.Windows.Forms.TextBox
    Private errorTextBox As System.Windows.Forms.TextBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents startButton As System.Windows.Forms.Button    Private clockSourceComboBox As System.Windows.Forms.ComboBox
    Private loopBandwidthComboBox As System.Windows.Forms.ComboBox
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents configurationListParametersGroupBox As System.Windows.Forms.GroupBox

End Class
