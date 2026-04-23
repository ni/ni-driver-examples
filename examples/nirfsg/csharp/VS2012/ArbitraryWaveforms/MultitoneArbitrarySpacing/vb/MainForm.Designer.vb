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
        Me.toneListGroupBox = New System.Windows.Forms.GroupBox()
        Me.offsetLabel = New System.Windows.Forms.Label()
        Me.addButton = New System.Windows.Forms.Button()
        Me.deleteButton = New System.Windows.Forms.Button()
        Me.loadButton = New System.Windows.Forms.Button()
        Me.toneNameTextBox = New System.Windows.Forms.TextBox()
        Me.initialPhaseNumeric = New System.Windows.Forms.NumericUpDown()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.offsetNumeric = New System.Windows.Forms.NumericUpDown()
        Me.toneListListBox = New System.Windows.Forms.ListBox()
        Me.initialPhaseLabel = New System.Windows.Forms.Label()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.toneNameLabel = New System.Windows.Forms.Label()
        Me.actualPeakPowerLabel = New System.Windows.Forms.Label()
        Me.actualPowerLevelLabel = New System.Windows.Forms.Label()
        Me.actualPeakPowerTextBox = New System.Windows.Forms.TextBox()
        Me.actualPowerLevelTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.centerFrequencyLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.numberOfSamplesLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.actualIQRateLabel = New System.Windows.Forms.Label()
        Me.centerFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.numberOfSamplesNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.actualIQRateTextBox = New System.Windows.Forms.TextBox()        Me.stopButton = New System.Windows.Forms.Button()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.startButton = New System.Windows.Forms.Button()
        Me.mirrorImageCheckBox = New System.Windows.Forms.CheckBox()
        Me.mirrorImageLabel = New System.Windows.Forms.Label()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.toneListGroupBox.SuspendLayout()
        CType(Me.initialPhaseNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.offsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.centerFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberOfSamplesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.configurationGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'toneListGroupBox
        '
        Me.toneListGroupBox.Controls.Add(Me.offsetLabel)
        Me.toneListGroupBox.Controls.Add(Me.addButton)
        Me.toneListGroupBox.Controls.Add(Me.deleteButton)
        Me.toneListGroupBox.Controls.Add(Me.loadButton)
        Me.toneListGroupBox.Controls.Add(Me.toneNameTextBox)
        Me.toneListGroupBox.Controls.Add(Me.initialPhaseNumeric)
        Me.toneListGroupBox.Controls.Add(Me.powerLevelNumeric)
        Me.toneListGroupBox.Controls.Add(Me.offsetNumeric)
        Me.toneListGroupBox.Controls.Add(Me.toneListListBox)
        Me.toneListGroupBox.Controls.Add(Me.initialPhaseLabel)
        Me.toneListGroupBox.Controls.Add(Me.powerLevelLabel)
        Me.toneListGroupBox.Controls.Add(Me.toneNameLabel)
        Me.toneListGroupBox.Location = New System.Drawing.Point(212, 83)
        Me.toneListGroupBox.Name = "toneListGroupBox"
        Me.toneListGroupBox.Size = New System.Drawing.Size(345, 248)
        Me.toneListGroupBox.TabIndex = 2
        Me.toneListGroupBox.TabStop = False
        Me.toneListGroupBox.Text = "Tone List"
        '
        'offsetLabel
        '
        Me.offsetLabel.AutoSize = True
        Me.offsetLabel.Location = New System.Drawing.Point(139, 86)
        Me.offsetLabel.Name = "offsetLabel"
        Me.offsetLabel.Size = New System.Drawing.Size(57, 13)
        Me.offsetLabel.TabIndex = 5
        Me.offsetLabel.Text = "Offset (Hz)"
        '
        'addButton
        '
        Me.addButton.Location = New System.Drawing.Point(25, 213)
        Me.addButton.Name = "addButton"
        Me.addButton.Size = New System.Drawing.Size(75, 23)
        Me.addButton.TabIndex = 5
        Me.addButton.Text = "&Add"
        Me.addButton.UseVisualStyleBackColor = True
        '
        'deleteButton
        '
        Me.deleteButton.Location = New System.Drawing.Point(116, 213)
        Me.deleteButton.Name = "deleteButton"
        Me.deleteButton.Size = New System.Drawing.Size(75, 23)
        Me.deleteButton.TabIndex = 6
        Me.deleteButton.Text = "&Delete"
        Me.deleteButton.UseVisualStyleBackColor = True
        '
        'loadButton
        '
        Me.loadButton.Location = New System.Drawing.Point(208, 213)
        Me.loadButton.Name = "loadButton"
        Me.loadButton.Size = New System.Drawing.Size(75, 23)
        Me.loadButton.TabIndex = 7
        Me.loadButton.Text = "&Load"
        Me.loadButton.UseVisualStyleBackColor = True
        '
        'toneNameTextBox
        '
        Me.toneNameTextBox.Location = New System.Drawing.Point(213, 51)
        Me.toneNameTextBox.Name = "toneNameTextBox"
        Me.toneNameTextBox.Size = New System.Drawing.Size(120, 20)
        Me.toneNameTextBox.TabIndex = 1
        Me.toneNameTextBox.Text = "100k tone"
        '
        'initialPhaseNumeric
        '
        Me.initialPhaseNumeric.DecimalPlaces = 2
        Me.initialPhaseNumeric.Location = New System.Drawing.Point(213, 147)
        Me.initialPhaseNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.initialPhaseNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.initialPhaseNumeric.Name = "initialPhaseNumeric"
        Me.initialPhaseNumeric.Size = New System.Drawing.Size(120, 20)
        Me.initialPhaseNumeric.TabIndex = 4
        '
        'powerLevelNumeric
        '
        Me.powerLevelNumeric.DecimalPlaces = 2
        Me.powerLevelNumeric.Location = New System.Drawing.Point(213, 115)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerLevelNumeric.TabIndex = 3
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'offsetNumeric
        '
        Me.offsetNumeric.DecimalPlaces = 2
        Me.offsetNumeric.Location = New System.Drawing.Point(213, 83)
        Me.offsetNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.offsetNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.offsetNumeric.Name = "offsetNumeric"
        Me.offsetNumeric.Size = New System.Drawing.Size(120, 20)
        Me.offsetNumeric.TabIndex = 2
        Me.offsetNumeric.Value = New Decimal(New Integer() {100000, 0, 0, 0})
        '
        'toneListListBox
        '
        Me.toneListListBox.FormattingEnabled = True
        Me.toneListListBox.Location = New System.Drawing.Point(22, 29)
        Me.toneListListBox.Name = "toneListListBox"
        Me.toneListListBox.Size = New System.Drawing.Size(100, 160)
        Me.toneListListBox.TabIndex = 0
        '
        'initialPhaseLabel
        '
        Me.initialPhaseLabel.AutoSize = True
        Me.initialPhaseLabel.Location = New System.Drawing.Point(139, 150)
        Me.initialPhaseLabel.Name = "initialPhaseLabel"
        Me.initialPhaseLabel.Size = New System.Drawing.Size(67, 13)
        Me.initialPhaseLabel.TabIndex = 7
        Me.initialPhaseLabel.Text = "Initial Phase "
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(139, 118)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(66, 13)
        Me.powerLevelLabel.TabIndex = 6
        Me.powerLevelLabel.Text = "Power Level"
        '
        'toneNameLabel
        '
        Me.toneNameLabel.AutoSize = True
        Me.toneNameLabel.Location = New System.Drawing.Point(139, 54)
        Me.toneNameLabel.Name = "toneNameLabel"
        Me.toneNameLabel.Size = New System.Drawing.Size(63, 13)
        Me.toneNameLabel.TabIndex = 4
        Me.toneNameLabel.Text = "Tone Name"
        '
        'actualPeakPowerLabel
        '
        Me.actualPeakPowerLabel.AutoSize = True
        Me.actualPeakPowerLabel.Location = New System.Drawing.Point(385, 27)
        Me.actualPeakPowerLabel.Name = "actualPeakPowerLabel"
        Me.actualPeakPowerLabel.Size = New System.Drawing.Size(143, 13)
        Me.actualPeakPowerLabel.TabIndex = 9
        Me.actualPeakPowerLabel.Text = "Peak Envelope Power (dBm)"
        '
        'actualPowerLevelLabel
        '
        Me.actualPowerLevelLabel.AutoSize = True
        Me.actualPowerLevelLabel.Location = New System.Drawing.Point(203, 27)
        Me.actualPowerLevelLabel.Name = "actualPowerLevelLabel"
        Me.actualPowerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.actualPowerLevelLabel.TabIndex = 10
        Me.actualPowerLevelLabel.Text = "Power Level (dBm)"
        '
        'actualPeakPowerTextBox
        '
        Me.actualPeakPowerTextBox.Location = New System.Drawing.Point(382, 48)
        Me.actualPeakPowerTextBox.Name = "actualPeakPowerTextBox"
        Me.actualPeakPowerTextBox.ReadOnly = True
        Me.actualPeakPowerTextBox.Size = New System.Drawing.Size(120, 20)
        Me.actualPeakPowerTextBox.TabIndex = 17
        Me.actualPeakPowerTextBox.TabStop = False
        Me.actualPeakPowerTextBox.Text = "0.00"
        '
        'actualPowerLevelTextBox
        '
        Me.actualPowerLevelTextBox.Location = New System.Drawing.Point(200, 48)
        Me.actualPowerLevelTextBox.Name = "actualPowerLevelTextBox"
        Me.actualPowerLevelTextBox.ReadOnly = True
        Me.actualPowerLevelTextBox.Size = New System.Drawing.Size(120, 20)
        Me.actualPowerLevelTextBox.TabIndex = 18
        Me.actualPowerLevelTextBox.TabStop = False
        Me.actualPowerLevelTextBox.Text = "0.00"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(21, 27)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'centerFrequencyLabel
        '
        Me.centerFrequencyLabel.AutoSize = True
        Me.centerFrequencyLabel.Location = New System.Drawing.Point(27, 29)
        Me.centerFrequencyLabel.Name = "centerFrequencyLabel"
        Me.centerFrequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.centerFrequencyLabel.TabIndex = 1
        Me.centerFrequencyLabel.Text = "Center Frequency (Hz)"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(27, 83)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
        Me.iqRateLabel.TabIndex = 2
        Me.iqRateLabel.Text = "IQ Rate (S/s)"
        '
        'numberOfSamplesLabel
        '
        Me.numberOfSamplesLabel.AutoSize = True
        Me.numberOfSamplesLabel.Location = New System.Drawing.Point(27, 138)
        Me.numberOfSamplesLabel.Name = "numberOfSamplesLabel"
        Me.numberOfSamplesLabel.Size = New System.Drawing.Size(99, 13)
        Me.numberOfSamplesLabel.TabIndex = 3
        Me.numberOfSamplesLabel.Text = "Number of Samples"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(24, 462)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 8
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'actualIQRateLabel
        '
        Me.actualIQRateLabel.AutoSize = True
        Me.actualIQRateLabel.Location = New System.Drawing.Point(29, 27)
        Me.actualIQRateLabel.Name = "actualIQRateLabel"
        Me.actualIQRateLabel.Size = New System.Drawing.Size(103, 13)
        Me.actualIQRateLabel.TabIndex = 11
        Me.actualIQRateLabel.Text = "Actual IQ Rate (S/s)"
        '
        'centerFrequencyNumeric
        '
        Me.centerFrequencyNumeric.DecimalPlaces = 2
        Me.centerFrequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.centerFrequencyNumeric.Location = New System.Drawing.Point(24, 49)
        Me.centerFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.centerFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.centerFrequencyNumeric.Name = "centerFrequencyNumeric"
        Me.centerFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.centerFrequencyNumeric.TabIndex = 0
        Me.centerFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 2
        Me.iqRateNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.iqRateNumeric.Location = New System.Drawing.Point(24, 104)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqRateNumeric.TabIndex = 1
        Me.iqRateNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'numberOfSamplesNumeric
        '
        Me.numberOfSamplesNumeric.DecimalPlaces = 2
        Me.numberOfSamplesNumeric.Increment = New Decimal(New Integer() {100, 0, 0, 0})
        Me.numberOfSamplesNumeric.Location = New System.Drawing.Point(24, 159)
        Me.numberOfSamplesNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numberOfSamplesNumeric.Name = "numberOfSamplesNumeric"
        Me.numberOfSamplesNumeric.Size = New System.Drawing.Size(120, 20)
        Me.numberOfSamplesNumeric.TabIndex = 2
        Me.numberOfSamplesNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(18, 44)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(21, 485)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(529, 47)
        Me.errorTextBox.TabIndex = 13
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No Error"
        '
        'actualIQRateTextBox
        '
        Me.actualIQRateTextBox.Location = New System.Drawing.Point(26, 48)
        Me.actualIQRateTextBox.Name = "actualIQRateTextBox"
        Me.actualIQRateTextBox.ReadOnly = True
        Me.actualIQRateTextBox.Size = New System.Drawing.Size(120, 20)
        Me.actualIQRateTextBox.TabIndex = 19
        Me.actualIQRateTextBox.TabStop = False
        Me.actualIQRateTextBox.Text = "0.00"
        '        '        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(394, 538)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 4
        Me.stopButton.Text = "Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(313, 538)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 3
        Me.startButton.Text = "Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'mirrorImageCheckBox
        '
        Me.mirrorImageCheckBox.AutoSize = True
        Me.mirrorImageCheckBox.Location = New System.Drawing.Point(30, 219)
        Me.mirrorImageCheckBox.Name = "mirrorImageCheckBox"
        Me.mirrorImageCheckBox.Size = New System.Drawing.Size(40, 17)
        Me.mirrorImageCheckBox.TabIndex = 3
        Me.mirrorImageCheckBox.Text = "On"
        Me.mirrorImageCheckBox.UseVisualStyleBackColor = True
        '
        'mirrorImageLabel
        '
        Me.mirrorImageLabel.AutoSize = True
        Me.mirrorImageLabel.Location = New System.Drawing.Point(27, 198)
        Me.mirrorImageLabel.Name = "mirrorImageLabel"
        Me.mirrorImageLabel.Size = New System.Drawing.Size(65, 13)
        Me.mirrorImageLabel.TabIndex = 26
        Me.mirrorImageLabel.Text = "Mirror Image"
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.centerFrequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.numberOfSamplesNumeric)
        Me.configurationGroupBox.Controls.Add(Me.iqRateNumeric)
        Me.configurationGroupBox.Controls.Add(Me.mirrorImageLabel)
        Me.configurationGroupBox.Controls.Add(Me.numberOfSamplesLabel)
        Me.configurationGroupBox.Controls.Add(Me.mirrorImageCheckBox)
        Me.configurationGroupBox.Controls.Add(Me.iqRateLabel)
        Me.configurationGroupBox.Controls.Add(Me.centerFrequencyLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(18, 83)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(167, 248)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.actualPowerLevelTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualPeakPowerLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualPeakPowerTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualPowerLevelLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(18, 351)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(539, 85)
        Me.measurementGroupBox.TabIndex = 0
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True        Me.ClientSize = New System.Drawing.Size(573, 573)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.toneListGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.errorTextBox)        Me.Controls.Add(Me.stopButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Multitone Arbitrary Spacing"
        Me.toneListGroupBox.ResumeLayout(False)
        Me.toneListGroupBox.PerformLayout()
        CType(Me.initialPhaseNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.offsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.centerFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberOfSamplesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private toneListGroupBox As System.Windows.Forms.GroupBox
    Private resourceNameLabel As System.Windows.Forms.Label
    Private centerFrequencyLabel As System.Windows.Forms.Label
    Private iqRateLabel As System.Windows.Forms.Label
    Private numberOfSamplesLabel As System.Windows.Forms.Label
    Private toneNameLabel As System.Windows.Forms.Label
    Private offsetLabel As System.Windows.Forms.Label
    Private powerLevelLabel As System.Windows.Forms.Label
    Private initialPhaseLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private actualPeakPowerLabel As System.Windows.Forms.Label
    Private actualPowerLevelLabel As System.Windows.Forms.Label
    Private actualIQRateLabel As System.Windows.Forms.Label
    Private centerFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private numberOfSamplesNumeric As System.Windows.Forms.NumericUpDown
    Private offsetNumeric As System.Windows.Forms.NumericUpDown
    Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private initialPhaseNumeric As System.Windows.Forms.NumericUpDown
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private toneNameTextBox As System.Windows.Forms.TextBox
    Private errorTextBox As System.Windows.Forms.TextBox
    Private actualPeakPowerTextBox As System.Windows.Forms.TextBox
    Private actualPowerLevelTextBox As System.Windows.Forms.TextBox
    Private actualIQRateTextBox As System.Windows.Forms.TextBox
    Private WithEvents loadButton As System.Windows.Forms.Button
    Private WithEvents deleteButton As System.Windows.Forms.Button
    Private WithEvents addButton As System.Windows.Forms.Button    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
    Private WithEvents startButton As System.Windows.Forms.Button
    Private toneListListBox As System.Windows.Forms.ListBox
    Private mirrorImageCheckBox As System.Windows.Forms.CheckBox
    Private mirrorImageLabel As System.Windows.Forms.Label
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private measurementGroupBox As System.Windows.Forms.GroupBox

End Class
