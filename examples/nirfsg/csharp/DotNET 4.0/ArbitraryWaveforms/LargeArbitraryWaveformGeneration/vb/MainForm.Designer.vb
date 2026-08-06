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
        Me.startFrequencyLabel = New System.Windows.Forms.Label()
        Me.stopFrequencyLabel = New System.Windows.Forms.Label()
        Me.directDownloadLabel = New System.Windows.Forms.Label()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.iqChirpDurationLabel = New System.Windows.Forms.Label()
        Me.powerLevelTypeLabel = New System.Windows.Forms.Label()
        Me.actualIQNumberOfSamplesLabel = New System.Windows.Forms.Label()
        Me.actualIQRateLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.generatingLabel = New System.Windows.Forms.Label()
        Me.generationStatusLabel = New System.Windows.Forms.Label()
        Me.startFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.stopFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqChirpDurationNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.actualIQNumberOfSamplesTextBox = New System.Windows.Forms.TextBox()
        Me.actualIQChirpDurationTextBox = New System.Windows.Forms.TextBox()
        Me.actualIQRateTextBox = New System.Windows.Forms.TextBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.powerLevelTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.generatingLed = New System.Windows.Forms.Button()
        Me.generationStatusProgressBar = New System.Windows.Forms.ProgressBar()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.directDownloadCheckBox = New System.Windows.Forms.CheckBox()
        Me.actualIQChirpDurationLabel = New System.Windows.Forms.Label()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.stopFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqChirpDurationNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.configurationGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(28, 19)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'startFrequencyLabel
        '
        Me.startFrequencyLabel.AutoSize = True
        Me.startFrequencyLabel.Location = New System.Drawing.Point(13, 22)
        Me.startFrequencyLabel.Name = "startFrequencyLabel"
        Me.startFrequencyLabel.Size = New System.Drawing.Size(104, 13)
        Me.startFrequencyLabel.TabIndex = 1
        Me.startFrequencyLabel.Text = "Start Frequency [Hz]"
        '
        'stopFrequencyLabel
        '
        Me.stopFrequencyLabel.AutoSize = True
        Me.stopFrequencyLabel.Location = New System.Drawing.Point(12, 89)
        Me.stopFrequencyLabel.Name = "stopFrequencyLabel"
        Me.stopFrequencyLabel.Size = New System.Drawing.Size(104, 13)
        Me.stopFrequencyLabel.TabIndex = 2
        Me.stopFrequencyLabel.Text = "Stop Frequency [Hz]"
        '
        'directDownloadLabel
        '
        Me.directDownloadLabel.AutoSize = True
        Me.directDownloadLabel.Location = New System.Drawing.Point(162, 90)
        Me.directDownloadLabel.Name = "directDownloadLabel"
        Me.directDownloadLabel.Size = New System.Drawing.Size(86, 13)
        Me.directDownloadLabel.TabIndex = 3
        Me.directDownloadLabel.Text = "Direct Download"
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(162, 23)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.powerLevelLabel.TabIndex = 4
        Me.powerLevelLabel.Text = "Power Level [dBm]"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(13, 211)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
        Me.iqRateLabel.TabIndex = 5
        Me.iqRateLabel.Text = "IQ Rate [S/s]"
        '
        'iqChirpDurationLabel
        '
        Me.iqChirpDurationLabel.AutoSize = True
        Me.iqChirpDurationLabel.Location = New System.Drawing.Point(12, 152)
        Me.iqChirpDurationLabel.Name = "iqChirpDurationLabel"
        Me.iqChirpDurationLabel.Size = New System.Drawing.Size(88, 13)
        Me.iqChirpDurationLabel.TabIndex = 6
        Me.iqChirpDurationLabel.Text = "Chirp Duration (s)"
        '
        'powerLevelTypeLabel
        '
        Me.powerLevelTypeLabel.AutoSize = True
        Me.powerLevelTypeLabel.Location = New System.Drawing.Point(162, 151)
        Me.powerLevelTypeLabel.Name = "powerLevelTypeLabel"
        Me.powerLevelTypeLabel.Size = New System.Drawing.Size(93, 13)
        Me.powerLevelTypeLabel.TabIndex = 7
        Me.powerLevelTypeLabel.Text = "Power Level Type"
        '
        'actualIQNumberOfSamplesLabel
        '
        Me.actualIQNumberOfSamplesLabel.AutoSize = True
        Me.actualIQNumberOfSamplesLabel.Location = New System.Drawing.Point(25, 154)
        Me.actualIQNumberOfSamplesLabel.Name = "actualIQNumberOfSamplesLabel"
        Me.actualIQNumberOfSamplesLabel.Size = New System.Drawing.Size(126, 13)
        Me.actualIQNumberOfSamplesLabel.TabIndex = 8
        Me.actualIQNumberOfSamplesLabel.Text = "Total Number of Samples"
        '
        'actualIQRateLabel
        '
        Me.actualIQRateLabel.Location = New System.Drawing.Point(25, 84)
        Me.actualIQRateLabel.Name = "actualIQRateLabel"
        Me.actualIQRateLabel.Size = New System.Drawing.Size(112, 18)
        Me.actualIQRateLabel.TabIndex = 9
        Me.actualIQRateLabel.Text = "Actual IQ Rate [S/s]"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(29, 354)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 10
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'generatingLabel
        '
        Me.generatingLabel.AutoSize = True
        Me.generatingLabel.Location = New System.Drawing.Point(299, 43)
        Me.generatingLabel.Name = "generatingLabel"
        Me.generatingLabel.Size = New System.Drawing.Size(59, 13)
        Me.generatingLabel.TabIndex = 11
        Me.generatingLabel.Text = "Generating"
        '
        'generationStatusLabel
        '
        Me.generationStatusLabel.AutoSize = True
        Me.generationStatusLabel.Location = New System.Drawing.Point(24, 213)
        Me.generationStatusLabel.Name = "generationStatusLabel"
        Me.generationStatusLabel.Size = New System.Drawing.Size(37, 13)
        Me.generationStatusLabel.TabIndex = 12
        Me.generationStatusLabel.Text = "Status"
        '
        'startFrequencyNumeric
        '
        Me.startFrequencyNumeric.DecimalPlaces = 6
        Me.startFrequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.startFrequencyNumeric.Location = New System.Drawing.Point(12, 43)
        Me.startFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.startFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.startFrequencyNumeric.Name = "startFrequencyNumeric"
        Me.startFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.startFrequencyNumeric.TabIndex = 0
        Me.startFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'stopFrequencyNumeric
        '
        Me.stopFrequencyNumeric.DecimalPlaces = 6
        Me.stopFrequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.stopFrequencyNumeric.Location = New System.Drawing.Point(12, 110)
        Me.stopFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.stopFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.stopFrequencyNumeric.Name = "stopFrequencyNumeric"
        Me.stopFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.stopFrequencyNumeric.TabIndex = 1
        Me.stopFrequencyNumeric.Value = New Decimal(New Integer() {1005000000, 0, 0, 0})
        '
        'powerLevelNumeric
        '
        Me.powerLevelNumeric.DecimalPlaces = 2
        Me.powerLevelNumeric.Location = New System.Drawing.Point(162, 43)
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
        Me.iqRateNumeric.Location = New System.Drawing.Point(13, 232)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqRateNumeric.TabIndex = 3
        Me.iqRateNumeric.Value = New Decimal(New Integer() {8333333, 0, 0, 0})
        '
        'iqChirpDurationNumeric
        '
        Me.iqChirpDurationNumeric.DecimalPlaces = 3
        Me.iqChirpDurationNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.iqChirpDurationNumeric.Location = New System.Drawing.Point(12, 173)
        Me.iqChirpDurationNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqChirpDurationNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqChirpDurationNumeric.Name = "iqChirpDurationNumeric"
        Me.iqChirpDurationNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqChirpDurationNumeric.TabIndex = 2
        Me.iqChirpDurationNumeric.Value = New Decimal(New Integer() {5, 0, 0, 65536})
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(28, 40)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'actualIQNumberOfSamplesTextBox
        '
        Me.actualIQNumberOfSamplesTextBox.Location = New System.Drawing.Point(25, 175)
        Me.actualIQNumberOfSamplesTextBox.Name = "actualIQNumberOfSamplesTextBox"
        Me.actualIQNumberOfSamplesTextBox.ReadOnly = True
        Me.actualIQNumberOfSamplesTextBox.Size = New System.Drawing.Size(120, 20)
        Me.actualIQNumberOfSamplesTextBox.TabIndex = 12
        Me.actualIQNumberOfSamplesTextBox.TabStop = False
        Me.actualIQNumberOfSamplesTextBox.Text = "40000000"
        '
        'actualIQChirpDurationTextBox
        '
        Me.actualIQChirpDurationTextBox.Location = New System.Drawing.Point(25, 46)
        Me.actualIQChirpDurationTextBox.Name = "actualIQChirpDurationTextBox"
        Me.actualIQChirpDurationTextBox.ReadOnly = True
        Me.actualIQChirpDurationTextBox.Size = New System.Drawing.Size(120, 20)
        Me.actualIQChirpDurationTextBox.TabIndex = 13
        Me.actualIQChirpDurationTextBox.TabStop = False
        Me.actualIQChirpDurationTextBox.Text = "0.500"
        '
        'actualIQRateTextBox
        '
        Me.actualIQRateTextBox.Location = New System.Drawing.Point(25, 105)
        Me.actualIQRateTextBox.Name = "actualIQRateTextBox"
        Me.actualIQRateTextBox.ReadOnly = True
        Me.actualIQRateTextBox.Size = New System.Drawing.Size(120, 20)
        Me.actualIQRateTextBox.TabIndex = 14
        Me.actualIQRateTextBox.TabStop = False
        Me.actualIQRateTextBox.Text = "0.000000"
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(30, 373)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(521, 43)
        Me.errorTextBox.TabIndex = 16
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(388, 38)
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
        Me.stopButton.Location = New System.Drawing.Point(476, 38)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 3
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'powerLevelTypeComboBox
        '
        Me.powerLevelTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.powerLevelTypeComboBox.Location = New System.Drawing.Point(162, 172)
        Me.powerLevelTypeComboBox.Name = "powerLevelTypeComboBox"
        Me.powerLevelTypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.powerLevelTypeComboBox.TabIndex = 5
        '
        'generatingLed
        '
        Me.generatingLed.BackColor = System.Drawing.SystemColors.Control
        Me.generatingLed.Enabled = False
        Me.generatingLed.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.generatingLed.Location = New System.Drawing.Point(274, 39)
        Me.generatingLed.Name = "generatingLed"
        Me.generatingLed.Size = New System.Drawing.Size(20, 20)
        Me.generatingLed.TabIndex = 17
        Me.generatingLed.TabStop = False
        Me.generatingLed.UseVisualStyleBackColor = False
        '
        'generationStatusProgressBar
        '
        Me.generationStatusProgressBar.ForeColor = System.Drawing.Color.Lime
        Me.generationStatusProgressBar.Location = New System.Drawing.Point(25, 234)
        Me.generationStatusProgressBar.Name = "generationStatusProgressBar"
        Me.generationStatusProgressBar.Size = New System.Drawing.Size(121, 21)
        Me.generationStatusProgressBar.TabIndex = 18
        '
        'rfsgStatusTimer
        '
        '
        'directDownloadCheckBox
        '
        Me.directDownloadCheckBox.AutoSize = True
        Me.directDownloadCheckBox.Checked = True
        Me.directDownloadCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.directDownloadCheckBox.Location = New System.Drawing.Point(165, 113)
        Me.directDownloadCheckBox.Name = "directDownloadCheckBox"
        Me.directDownloadCheckBox.Size = New System.Drawing.Size(59, 17)
        Me.directDownloadCheckBox.TabIndex = 5
        Me.directDownloadCheckBox.Text = "Enable"
        Me.directDownloadCheckBox.UseVisualStyleBackColor = True
        '
        'actualIQChirpDurationLabel
        '
        Me.actualIQChirpDurationLabel.AutoSize = True
        Me.actualIQChirpDurationLabel.Location = New System.Drawing.Point(25, 25)
        Me.actualIQChirpDurationLabel.Name = "actualIQChirpDurationLabel"
        Me.actualIQChirpDurationLabel.Size = New System.Drawing.Size(121, 13)
        Me.actualIQChirpDurationLabel.TabIndex = 20
        Me.actualIQChirpDurationLabel.Text = "Actual Chirp Duration (s)"
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.powerLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.iqChirpDurationLabel)
        Me.configurationGroupBox.Controls.Add(Me.directDownloadCheckBox)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelTypeComboBox)
        Me.configurationGroupBox.Controls.Add(Me.iqChirpDurationNumeric)
        Me.configurationGroupBox.Controls.Add(Me.startFrequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.iqRateNumeric)
        Me.configurationGroupBox.Controls.Add(Me.stopFrequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.stopFrequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.directDownloadLabel)
        Me.configurationGroupBox.Controls.Add(Me.startFrequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelTypeLabel)
        Me.configurationGroupBox.Controls.Add(Me.iqRateLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(28, 74)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(297, 270)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.actualIQChirpDurationTextBox)
        Me.measurementGroupBox.Controls.Add(Me.generationStatusProgressBar)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualIQNumberOfSamplesTextBox)
        Me.measurementGroupBox.Controls.Add(Me.generationStatusLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualIQChirpDurationLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualIQNumberOfSamplesLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(376, 74)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(175, 270)
        Me.measurementGroupBox.TabIndex = 0
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(576, 460)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.generatingLabel)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.generatingLed)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Large Arbitrary Waveform Generation"
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.stopFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqChirpDurationNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private resourceNameLabel As System.Windows.Forms.Label
    Private startFrequencyLabel As System.Windows.Forms.Label
    Private stopFrequencyLabel As System.Windows.Forms.Label
    Private directDownloadLabel As System.Windows.Forms.Label
    Private powerLevelLabel As System.Windows.Forms.Label
    Private iqRateLabel As System.Windows.Forms.Label
    Private iqChirpDurationLabel As System.Windows.Forms.Label
    Private powerLevelTypeLabel As System.Windows.Forms.Label
    Private actualIQNumberOfSamplesLabel As System.Windows.Forms.Label
    Private actualIQRateLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private generatingLabel As System.Windows.Forms.Label
    Private generationStatusLabel As System.Windows.Forms.Label
    Private startFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private stopFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private iqChirpDurationNumeric As System.Windows.Forms.NumericUpDown
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private actualIQNumberOfSamplesTextBox As System.Windows.Forms.TextBox
    Private actualIQChirpDurationTextBox As System.Windows.Forms.TextBox
    Private actualIQRateTextBox As System.Windows.Forms.TextBox
    Private errorTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button    Private powerLevelTypeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
    Private generatingLed As System.Windows.Forms.Button
    Private generationStatusProgressBar As System.Windows.Forms.ProgressBar
    Private directDownloadCheckBox As System.Windows.Forms.CheckBox
    Private actualIQChirpDurationLabel As System.Windows.Forms.Label
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private measurementGroupBox As System.Windows.Forms.GroupBox

End Class
