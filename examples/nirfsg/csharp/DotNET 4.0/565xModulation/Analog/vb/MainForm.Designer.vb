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
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.frequencyLabel = New System.Windows.Forms.Label()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.messageFrequencyLabel = New System.Windows.Forms.Label()
        Me.frequencyReferenceSourceLabel = New System.Windows.Forms.Label()
        Me.messageWaveformTypeLabel = New System.Windows.Forms.Label()
        Me.modulationTypeLabel = New System.Windows.Forms.Label()
        Me.pmDeviationLabel = New System.Windows.Forms.Label()
        Me.fmDeviationLabel = New System.Windows.Forms.Label()
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.messageFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pmDeviationNumeric = New System.Windows.Forms.NumericUpDown()
        Me.fmDeviationNumeric = New System.Windows.Forms.NumericUpDown()
        Me.frequencyReferenceSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.messageWaveformTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.modulationTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.generateGroupBox = New System.Windows.Forms.GroupBox()
        Me.statusLabel = New System.Windows.Forms.Label()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.generateButton = New System.Windows.Forms.Button()
        Me.statusLed = New System.Windows.Forms.Button()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.messageFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pmDeviationNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.fmDeviationNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.generateGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.frequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.messageFrequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.frequencyReferenceSourceLabel)
        Me.configurationGroupBox.Controls.Add(Me.messageWaveformTypeLabel)
        Me.configurationGroupBox.Controls.Add(Me.modulationTypeLabel)
        Me.configurationGroupBox.Controls.Add(Me.pmDeviationLabel)
        Me.configurationGroupBox.Controls.Add(Me.fmDeviationLabel)
        Me.configurationGroupBox.Controls.Add(Me.frequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.messageFrequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pmDeviationNumeric)
        Me.configurationGroupBox.Controls.Add(Me.fmDeviationNumeric)
        Me.configurationGroupBox.Controls.Add(Me.frequencyReferenceSourceComboBox)
        Me.configurationGroupBox.Controls.Add(Me.messageWaveformTypeComboBox)
        Me.configurationGroupBox.Controls.Add(Me.modulationTypeComboBox)
        Me.configurationGroupBox.Location = New System.Drawing.Point(31, 74)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(488, 207)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.AutoSize = True
        Me.frequencyLabel.Location = New System.Drawing.Point(13, 24)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.frequencyLabel.TabIndex = 0
        Me.frequencyLabel.Text = "Center Frequency [Hz]"
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(13, 91)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.powerLevelLabel.TabIndex = 1
        Me.powerLevelLabel.Text = "Power Level [dBm]"
        '
        'messageFrequencyLabel
        '
        Me.messageFrequencyLabel.AutoSize = True
        Me.messageFrequencyLabel.Location = New System.Drawing.Point(325, 91)
        Me.messageFrequencyLabel.Name = "messageFrequencyLabel"
        Me.messageFrequencyLabel.Size = New System.Drawing.Size(155, 13)
        Me.messageFrequencyLabel.TabIndex = 3
        Me.messageFrequencyLabel.Text = "Message Waveform Frequency"
        '
        'frequencyReferenceSourceLabel
        '
        Me.frequencyReferenceSourceLabel.AutoSize = True
        Me.frequencyReferenceSourceLabel.Location = New System.Drawing.Point(13, 152)
        Me.frequencyReferenceSourceLabel.Name = "frequencyReferenceSourceLabel"
        Me.frequencyReferenceSourceLabel.Size = New System.Drawing.Size(147, 13)
        Me.frequencyReferenceSourceLabel.TabIndex = 4
        Me.frequencyReferenceSourceLabel.Text = "Frequency Reference Source"
        '
        'messageWaveformTypeLabel
        '
        Me.messageWaveformTypeLabel.AutoSize = True
        Me.messageWaveformTypeLabel.Location = New System.Drawing.Point(325, 24)
        Me.messageWaveformTypeLabel.Name = "messageWaveformTypeLabel"
        Me.messageWaveformTypeLabel.Size = New System.Drawing.Size(129, 13)
        Me.messageWaveformTypeLabel.TabIndex = 5
        Me.messageWaveformTypeLabel.Text = "Message Waveform Type"
        '
        'modulationTypeLabel
        '
        Me.modulationTypeLabel.AutoSize = True
        Me.modulationTypeLabel.Location = New System.Drawing.Point(171, 24)
        Me.modulationTypeLabel.Name = "modulationTypeLabel"
        Me.modulationTypeLabel.Size = New System.Drawing.Size(86, 13)
        Me.modulationTypeLabel.TabIndex = 6
        Me.modulationTypeLabel.Text = "Modulation Type"
        '
        'pmDeviationLabel
        '
        Me.pmDeviationLabel.AutoSize = True
        Me.pmDeviationLabel.Location = New System.Drawing.Point(171, 91)
        Me.pmDeviationLabel.Name = "pmDeviationLabel"
        Me.pmDeviationLabel.Size = New System.Drawing.Size(71, 13)
        Me.pmDeviationLabel.TabIndex = 8
        Me.pmDeviationLabel.Text = "PM Deviation"
        '
        'fmDeviationLabel
        '
        Me.fmDeviationLabel.AutoSize = True
        Me.fmDeviationLabel.Location = New System.Drawing.Point(172, 91)
        Me.fmDeviationLabel.Name = "fmDeviationLabel"
        Me.fmDeviationLabel.Size = New System.Drawing.Size(70, 13)
        Me.fmDeviationLabel.TabIndex = 9
        Me.fmDeviationLabel.Text = "FM Deviation"
        '
        'frequencyNumeric
        '
        Me.frequencyNumeric.DecimalPlaces = 6
        Me.frequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.frequencyNumeric.Location = New System.Drawing.Point(13, 45)
        Me.frequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.frequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.frequencyNumeric.Name = "frequencyNumeric"
        Me.frequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.frequencyNumeric.TabIndex = 0
        Me.frequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'powerLevelNumeric
        '
        Me.powerLevelNumeric.DecimalPlaces = 2
        Me.powerLevelNumeric.Location = New System.Drawing.Point(13, 112)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerLevelNumeric.TabIndex = 1
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'messageFrequencyNumeric
        '
        Me.messageFrequencyNumeric.DecimalPlaces = 2
        Me.messageFrequencyNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.messageFrequencyNumeric.Location = New System.Drawing.Point(325, 112)
        Me.messageFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.messageFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.messageFrequencyNumeric.Name = "messageFrequencyNumeric"
        Me.messageFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.messageFrequencyNumeric.TabIndex = 6
        Me.messageFrequencyNumeric.Value = New Decimal(New Integer() {10000, 0, 0, 0})
        '
        'pmDeviationNumeric
        '
        Me.pmDeviationNumeric.DecimalPlaces = 2
        Me.pmDeviationNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.pmDeviationNumeric.Location = New System.Drawing.Point(171, 112)
        Me.pmDeviationNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pmDeviationNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pmDeviationNumeric.Name = "pmDeviationNumeric"
        Me.pmDeviationNumeric.Size = New System.Drawing.Size(120, 20)
        Me.pmDeviationNumeric.TabIndex = 4
        Me.pmDeviationNumeric.Value = New Decimal(New Integer() {90, 0, 0, 0})
        '
        'fmDeviationNumeric
        '
        Me.fmDeviationNumeric.DecimalPlaces = 2
        Me.fmDeviationNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.fmDeviationNumeric.Location = New System.Drawing.Point(172, 112)
        Me.fmDeviationNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.fmDeviationNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.fmDeviationNumeric.Name = "fmDeviationNumeric"
        Me.fmDeviationNumeric.Size = New System.Drawing.Size(120, 20)
        Me.fmDeviationNumeric.TabIndex = 15
        Me.fmDeviationNumeric.Value = New Decimal(New Integer() {10000, 0, 0, 0})
        '
        'frequencyReferenceSourceComboBox
        '
        Me.frequencyReferenceSourceComboBox.Location = New System.Drawing.Point(13, 173)
        Me.frequencyReferenceSourceComboBox.Name = "frequencyReferenceSourceComboBox"
        Me.frequencyReferenceSourceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.frequencyReferenceSourceComboBox.TabIndex = 2
        '
        'messageWaveformTypeComboBox
        '
        Me.messageWaveformTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.messageWaveformTypeComboBox.Location = New System.Drawing.Point(325, 45)
        Me.messageWaveformTypeComboBox.Name = "messageWaveformTypeComboBox"
        Me.messageWaveformTypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.messageWaveformTypeComboBox.TabIndex = 5
        '
        'modulationTypeComboBox
        '
        Me.modulationTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.modulationTypeComboBox.Location = New System.Drawing.Point(171, 45)
        Me.modulationTypeComboBox.Name = "modulationTypeComboBox"
        Me.modulationTypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.modulationTypeComboBox.TabIndex = 3
        '
        'generateGroupBox
        '
        Me.generateGroupBox.Controls.Add(Me.statusLabel)
        Me.generateGroupBox.Controls.Add(Me.stopButton)
        Me.generateGroupBox.Controls.Add(Me.generateButton)
        Me.generateGroupBox.Controls.Add(Me.statusLed)
        Me.generateGroupBox.Location = New System.Drawing.Point(31, 291)
        Me.generateGroupBox.Name = "generateGroupBox"
        Me.generateGroupBox.Size = New System.Drawing.Size(488, 50)
        Me.generateGroupBox.TabIndex = 2
        Me.generateGroupBox.TabStop = False
        '
        'statusLabel
        '
        Me.statusLabel.AutoSize = True
        Me.statusLabel.Location = New System.Drawing.Point(51, 21)
        Me.statusLabel.Name = "statusLabel"
        Me.statusLabel.Size = New System.Drawing.Size(92, 13)
        Me.statusLabel.TabIndex = 19
        Me.statusLabel.Text = "Generation Status"
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(394, 16)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'generateButton
        '
        Me.generateButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.generateButton.Location = New System.Drawing.Point(293, 16)
        Me.generateButton.Name = "generateButton"
        Me.generateButton.Size = New System.Drawing.Size(75, 23)
        Me.generateButton.TabIndex = 0
        Me.generateButton.Text = "&Generate"
        Me.generateButton.UseVisualStyleBackColor = True
        '
        'statusLed
        '
        Me.statusLed.BackColor = System.Drawing.SystemColors.Control
        Me.statusLed.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.statusLed.Enabled = False
        Me.statusLed.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.statusLed.Location = New System.Drawing.Point(13, 13)
        Me.statusLed.Name = "statusLed"
        Me.statusLed.Size = New System.Drawing.Size(29, 29)
        Me.statusLed.TabIndex = 17
        Me.statusLed.TabStop = False
        Me.statusLed.UseVisualStyleBackColor = False
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(31, 354)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 2
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(31, 16)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 7
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(33, 370)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(486, 60)
        Me.errorTextBox.TabIndex = 7
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(31, 37)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'rfsgStatusTimer
        '
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.CancelButton = Me.stopButton
        Me.ClientSize = New System.Drawing.Size(549, 478)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.generateGroupBox)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "565x Analog Modulation"
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.messageFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pmDeviationNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.fmDeviationNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.generateGroupBox.ResumeLayout(False)
        Me.generateGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private generateGroupBox As System.Windows.Forms.GroupBox
    Private frequencyLabel As System.Windows.Forms.Label
    Private powerLevelLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private messageFrequencyLabel As System.Windows.Forms.Label
    Private frequencyReferenceSourceLabel As System.Windows.Forms.Label
    Private messageWaveformTypeLabel As System.Windows.Forms.Label
    Private modulationTypeLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private pmDeviationLabel As System.Windows.Forms.Label
    Private fmDeviationLabel As System.Windows.Forms.Label
    Private frequencyNumeric As System.Windows.Forms.NumericUpDown
    Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private messageFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private pmDeviationNumeric As System.Windows.Forms.NumericUpDown
    Private fmDeviationNumeric As System.Windows.Forms.NumericUpDown
    Private errorTextBox As System.Windows.Forms.TextBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents generateButton As System.Windows.Forms.Button
    Private frequencyReferenceSourceComboBox As System.Windows.Forms.ComboBox
    Private messageWaveformTypeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents modulationTypeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
    Private statusLed As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private statusLabel As System.Windows.Forms.Label

End Class
