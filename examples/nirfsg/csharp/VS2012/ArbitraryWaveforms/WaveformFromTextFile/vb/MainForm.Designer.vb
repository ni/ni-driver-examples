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
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.signalBandwidthLabel = New System.Windows.Forms.Label()
        Me.frequencyLabel = New System.Windows.Forms.Label()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.preGainLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.directDownloadLabel = New System.Windows.Forms.Label()
        Me.pathLabel = New System.Windows.Forms.Label()
        Me.powerLevelTypeLabel = New System.Windows.Forms.Label()
        Me.actualFrequencyOffsetLabel = New System.Windows.Forms.Label()
        Me.actualIQRateLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.generatingLabel = New System.Windows.Forms.Label()
        Me.actualTotalSamplesLabel = New System.Windows.Forms.Label()
        Me.signalBandwidthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.preGainNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.pathTextBox = New System.Windows.Forms.TextBox()
        Me.actualFrequencyOffsetTextBox = New System.Windows.Forms.TextBox()
        Me.actualIQRateTextBox = New System.Windows.Forms.TextBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.actualTotalSamplesTextBox = New System.Windows.Forms.TextBox()
        Me.browseButton = New System.Windows.Forms.Button()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.powerLevelTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.generatingLed = New System.Windows.Forms.Button()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.directDownloadCheckBox = New System.Windows.Forms.CheckBox()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.signalBandwidthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.preGainNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.configurationGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(17, 16)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'signalBandwidthLabel
        '
        Me.signalBandwidthLabel.AutoSize = True
        Me.signalBandwidthLabel.Location = New System.Drawing.Point(14, 177)
        Me.signalBandwidthLabel.Name = "signalBandwidthLabel"
        Me.signalBandwidthLabel.Size = New System.Drawing.Size(111, 13)
        Me.signalBandwidthLabel.TabIndex = 1
        Me.signalBandwidthLabel.Text = "Signal Bandwidth [Hz]"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.AutoSize = True
        Me.frequencyLabel.Location = New System.Drawing.Point(14, 24)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.frequencyLabel.TabIndex = 2
        Me.frequencyLabel.Text = "Center Frequency [Hz]"
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(14, 78)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.powerLevelLabel.TabIndex = 3
        Me.powerLevelLabel.Text = "Power Level [dBm]"
        '
        'preGainLabel
        '
        Me.preGainLabel.AutoSize = True
        Me.preGainLabel.Location = New System.Drawing.Point(14, 129)
        Me.preGainLabel.Name = "preGainLabel"
        Me.preGainLabel.Size = New System.Drawing.Size(92, 13)
        Me.preGainLabel.TabIndex = 4
        Me.preGainLabel.Text = "Pre-filter Gain [dB]"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(164, 129)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
        Me.iqRateLabel.TabIndex = 5
        Me.iqRateLabel.Text = "IQ Rate [S/s]"
        '
        'directDownloadLabel
        '
        Me.directDownloadLabel.AutoSize = True
        Me.directDownloadLabel.Location = New System.Drawing.Point(164, 23)
        Me.directDownloadLabel.Name = "directDownloadLabel"
        Me.directDownloadLabel.Size = New System.Drawing.Size(86, 13)
        Me.directDownloadLabel.TabIndex = 6
        Me.directDownloadLabel.Text = "Direct Download"
        '
        'pathLabel
        '
        Me.pathLabel.AutoSize = True
        Me.pathLabel.Location = New System.Drawing.Point(331, 97)
        Me.pathLabel.Name = "pathLabel"
        Me.pathLabel.Size = New System.Drawing.Size(112, 13)
        Me.pathLabel.TabIndex = 7
        Me.pathLabel.Text = "Path to Waveform File"
        '
        'powerLevelTypeLabel
        '
        Me.powerLevelTypeLabel.AutoSize = True
        Me.powerLevelTypeLabel.Location = New System.Drawing.Point(164, 78)
        Me.powerLevelTypeLabel.Name = "powerLevelTypeLabel"
        Me.powerLevelTypeLabel.Size = New System.Drawing.Size(93, 13)
        Me.powerLevelTypeLabel.TabIndex = 8
        Me.powerLevelTypeLabel.Text = "Power Level Type"
        '
        'actualFrequencyOffsetLabel
        '
        Me.actualFrequencyOffsetLabel.AutoSize = True
        Me.actualFrequencyOffsetLabel.Location = New System.Drawing.Point(141, 29)
        Me.actualFrequencyOffsetLabel.Name = "actualFrequencyOffsetLabel"
        Me.actualFrequencyOffsetLabel.Size = New System.Drawing.Size(110, 13)
        Me.actualFrequencyOffsetLabel.TabIndex = 9
        Me.actualFrequencyOffsetLabel.Text = "Frequency Offset [Hz]"
        '
        'actualIQRateLabel
        '
        Me.actualIQRateLabel.AutoSize = True
        Me.actualIQRateLabel.Location = New System.Drawing.Point(141, 79)
        Me.actualIQRateLabel.Name = "actualIQRateLabel"
        Me.actualIQRateLabel.Size = New System.Drawing.Size(103, 13)
        Me.actualIQRateLabel.TabIndex = 10
        Me.actualIQRateLabel.Text = "Actual IQ Rate [S/s]"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(17, 312)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 11
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'generatingLabel
        '
        Me.generatingLabel.AutoSize = True
        Me.generatingLabel.Location = New System.Drawing.Point(375, 39)
        Me.generatingLabel.Name = "generatingLabel"
        Me.generatingLabel.Size = New System.Drawing.Size(59, 13)
        Me.generatingLabel.TabIndex = 12
        Me.generatingLabel.Text = "Generating"
        '
        'actualTotalSamplesLabel
        '
        Me.actualTotalSamplesLabel.AutoSize = True
        Me.actualTotalSamplesLabel.Location = New System.Drawing.Point(19, 29)
        Me.actualTotalSamplesLabel.Name = "actualTotalSamplesLabel"
        Me.actualTotalSamplesLabel.Size = New System.Drawing.Size(84, 13)
        Me.actualTotalSamplesLabel.TabIndex = 13
        Me.actualTotalSamplesLabel.Text = "Total # Samples"
        '
        'signalBandwidthNumeric
        '
        Me.signalBandwidthNumeric.DecimalPlaces = 6
        Me.signalBandwidthNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.signalBandwidthNumeric.Location = New System.Drawing.Point(14, 196)
        Me.signalBandwidthNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.signalBandwidthNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.signalBandwidthNumeric.Name = "signalBandwidthNumeric"
        Me.signalBandwidthNumeric.Size = New System.Drawing.Size(120, 20)
        Me.signalBandwidthNumeric.TabIndex = 5
        Me.signalBandwidthNumeric.Value = New Decimal(New Integer() {5000000, 0, 0, 0})
        '
        'frequencyNumeric
        '
        Me.frequencyNumeric.DecimalPlaces = 6
        Me.frequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.frequencyNumeric.Location = New System.Drawing.Point(14, 43)
        Me.frequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.frequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.frequencyNumeric.Name = "frequencyNumeric"
        Me.frequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.frequencyNumeric.TabIndex = 2
        Me.frequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'powerLevelNumeric
        '
        Me.powerLevelNumeric.DecimalPlaces = 1
        Me.powerLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.powerLevelNumeric.Location = New System.Drawing.Point(14, 97)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerLevelNumeric.TabIndex = 3
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'preGainNumeric
        '
        Me.preGainNumeric.DecimalPlaces = 1
        Me.preGainNumeric.Location = New System.Drawing.Point(14, 148)
        Me.preGainNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.preGainNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.preGainNumeric.Name = "preGainNumeric"
        Me.preGainNumeric.Size = New System.Drawing.Size(120, 20)
        Me.preGainNumeric.TabIndex = 4
        Me.preGainNumeric.Value = New Decimal(New Integer() {2, 0, 0, -2147483648})
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 6
        Me.iqRateNumeric.Increment = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.iqRateNumeric.Location = New System.Drawing.Point(163, 148)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqRateNumeric.TabIndex = 8
        Me.iqRateNumeric.Value = New Decimal(New Integer() {833333333, 0, 0, 131072})
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(17, 37)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'pathTextBox
        '
        Me.pathTextBox.Location = New System.Drawing.Point(332, 118)
        Me.pathTextBox.Name = "pathTextBox"
        Me.pathTextBox.Size = New System.Drawing.Size(246, 20)
        Me.pathTextBox.TabIndex = 3
        Me.pathTextBox.Text = "..\..\TwoTone_IQ100MHz_2D.txt"
        '
        'actualFrequencyOffsetTextBox
        '
        Me.actualFrequencyOffsetTextBox.Location = New System.Drawing.Point(144, 47)
        Me.actualFrequencyOffsetTextBox.Name = "actualFrequencyOffsetTextBox"
        Me.actualFrequencyOffsetTextBox.ReadOnly = True
        Me.actualFrequencyOffsetTextBox.Size = New System.Drawing.Size(111, 20)
        Me.actualFrequencyOffsetTextBox.TabIndex = 14
        Me.actualFrequencyOffsetTextBox.TabStop = False
        Me.actualFrequencyOffsetTextBox.Text = "100000000.000000"
        '
        'actualIQRateTextBox
        '
        Me.actualIQRateTextBox.Location = New System.Drawing.Point(144, 95)
        Me.actualIQRateTextBox.Name = "actualIQRateTextBox"
        Me.actualIQRateTextBox.ReadOnly = True
        Me.actualIQRateTextBox.Size = New System.Drawing.Size(111, 20)
        Me.actualIQRateTextBox.TabIndex = 15
        Me.actualIQRateTextBox.TabStop = False
        Me.actualIQRateTextBox.Text = "8333333.330000"
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(17, 328)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(600, 47)
        Me.errorTextBox.TabIndex = 17
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'actualTotalSamplesTextBox
        '
        Me.actualTotalSamplesTextBox.Location = New System.Drawing.Point(19, 46)
        Me.actualTotalSamplesTextBox.Name = "actualTotalSamplesTextBox"
        Me.actualTotalSamplesTextBox.ReadOnly = True
        Me.actualTotalSamplesTextBox.Size = New System.Drawing.Size(99, 20)
        Me.actualTotalSamplesTextBox.TabIndex = 19
        Me.actualTotalSamplesTextBox.TabStop = False
        Me.actualTotalSamplesTextBox.Text = "10000"
        '
        'browseButton
        '
        Me.browseButton.Location = New System.Drawing.Point(584, 115)
        Me.browseButton.Name = "browseButton"
        Me.browseButton.Size = New System.Drawing.Size(32, 23)
        Me.browseButton.TabIndex = 4
        Me.browseButton.Text = "..."
        Me.browseButton.UseVisualStyleBackColor = True
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(461, 35)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 5
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(542, 34)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 6
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'powerLevelTypeComboBox
        '
        Me.powerLevelTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.powerLevelTypeComboBox.Location = New System.Drawing.Point(163, 97)
        Me.powerLevelTypeComboBox.Name = "powerLevelTypeComboBox"
        Me.powerLevelTypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.powerLevelTypeComboBox.TabIndex = 7
        '
        'generatingLed
        '
        Me.generatingLed.BackColor = System.Drawing.SystemColors.Control
        Me.generatingLed.Enabled = False
        Me.generatingLed.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.generatingLed.Location = New System.Drawing.Point(349, 36)
        Me.generatingLed.Name = "generatingLed"
        Me.generatingLed.Size = New System.Drawing.Size(20, 20)
        Me.generatingLed.TabIndex = 20
        Me.generatingLed.TabStop = False
        Me.generatingLed.UseVisualStyleBackColor = False
        '
        'rfsgStatusTimer
        '
        '
        'directDownloadCheckBox
        '
        Me.directDownloadCheckBox.AutoSize = True
        Me.directDownloadCheckBox.Checked = True
        Me.directDownloadCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.directDownloadCheckBox.Location = New System.Drawing.Point(163, 44)
        Me.directDownloadCheckBox.Name = "directDownloadCheckBox"
        Me.directDownloadCheckBox.Size = New System.Drawing.Size(59, 17)
        Me.directDownloadCheckBox.TabIndex = 6
        Me.directDownloadCheckBox.Text = "Enable"
        Me.directDownloadCheckBox.UseVisualStyleBackColor = True
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.powerLevelTypeComboBox)
        Me.configurationGroupBox.Controls.Add(Me.iqRateNumeric)
        Me.configurationGroupBox.Controls.Add(Me.directDownloadCheckBox)
        Me.configurationGroupBox.Controls.Add(Me.preGainNumeric)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.signalBandwidthLabel)
        Me.configurationGroupBox.Controls.Add(Me.frequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.frequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.signalBandwidthNumeric)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelTypeLabel)
        Me.configurationGroupBox.Controls.Add(Me.preGainLabel)
        Me.configurationGroupBox.Controls.Add(Me.directDownloadLabel)
        Me.configurationGroupBox.Controls.Add(Me.iqRateLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(17, 73)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(298, 226)
        Me.configurationGroupBox.TabIndex = 2
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyOffsetTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualTotalSamplesTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualTotalSamplesLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyOffsetLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(334, 173)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(283, 126)
        Me.measurementGroupBox.TabIndex = 3
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(631, 412)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.pathLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.generatingLabel)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.pathTextBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.browseButton)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.generatingLed)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Waveform From File"
        CType(Me.signalBandwidthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.preGainNumeric, System.ComponentModel.ISupportInitialize).EndInit()
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
    Private signalBandwidthLabel As System.Windows.Forms.Label
    Private frequencyLabel As System.Windows.Forms.Label
    Private powerLevelLabel As System.Windows.Forms.Label
    Private preGainLabel As System.Windows.Forms.Label
    Private iqRateLabel As System.Windows.Forms.Label
    Private directDownloadLabel As System.Windows.Forms.Label
    Private pathLabel As System.Windows.Forms.Label
    Private powerLevelTypeLabel As System.Windows.Forms.Label
    Private actualFrequencyOffsetLabel As System.Windows.Forms.Label
    Private actualIQRateLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private generatingLabel As System.Windows.Forms.Label
    Private actualTotalSamplesLabel As System.Windows.Forms.Label
    Private signalBandwidthNumeric As System.Windows.Forms.NumericUpDown
    Private frequencyNumeric As System.Windows.Forms.NumericUpDown
    Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private preGainNumeric As System.Windows.Forms.NumericUpDown
    Private iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private pathTextBox As System.Windows.Forms.TextBox
    Private actualFrequencyOffsetTextBox As System.Windows.Forms.TextBox
    Private actualIQRateTextBox As System.Windows.Forms.TextBox
    Private errorTextBox As System.Windows.Forms.TextBox
    Private actualTotalSamplesTextBox As System.Windows.Forms.TextBox
    Private WithEvents browseButton As System.Windows.Forms.Button
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button    Private powerLevelTypeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
    Private generatingLed As System.Windows.Forms.Button
    Private directDownloadCheckBox As System.Windows.Forms.CheckBox
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private measurementGroupBox As System.Windows.Forms.GroupBox

End Class
