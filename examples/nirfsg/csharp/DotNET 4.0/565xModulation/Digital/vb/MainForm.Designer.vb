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
        Me.messageUserDefinedWaveformLabel = New System.Windows.Forms.Label()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.messageSymbolRateLabel = New System.Windows.Forms.Label()
        Me.frequencyLabel = New System.Windows.Forms.Label()
        Me.messagePrbsOrderLabel = New System.Windows.Forms.Label()
        Me.frequencyReferenceSourceLabel = New System.Windows.Forms.Label()
        Me.messageWaveformTypeLabel = New System.Windows.Forms.Label()
        Me.modulationTypeLabel = New System.Windows.Forms.Label()
        Me.fskDeviationLabel = New System.Windows.Forms.Label()
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.messageSymbolRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.messagePrbsOrderNumeric = New System.Windows.Forms.NumericUpDown()
        Me.fskDeviationNumeric = New System.Windows.Forms.NumericUpDown()
        Me.messageUserDefinedWaveformTextBox = New System.Windows.Forms.TextBox()
        Me.messageWaveformTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.modulationTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.frequencyReferenceSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.generateGroupBox = New System.Windows.Forms.GroupBox()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.statusLabel = New System.Windows.Forms.Label()
        Me.generateButton = New System.Windows.Forms.Button()
        Me.statusLed = New System.Windows.Forms.Button()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.messageSymbolRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.messagePrbsOrderNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.fskDeviationNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.generateGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.messageUserDefinedWaveformLabel)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.messageSymbolRateLabel)
        Me.configurationGroupBox.Controls.Add(Me.frequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.messagePrbsOrderLabel)
        Me.configurationGroupBox.Controls.Add(Me.frequencyReferenceSourceLabel)
        Me.configurationGroupBox.Controls.Add(Me.messageWaveformTypeLabel)
        Me.configurationGroupBox.Controls.Add(Me.modulationTypeLabel)
        Me.configurationGroupBox.Controls.Add(Me.fskDeviationLabel)
        Me.configurationGroupBox.Controls.Add(Me.frequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.messageSymbolRateNumeric)
        Me.configurationGroupBox.Controls.Add(Me.messagePrbsOrderNumeric)
        Me.configurationGroupBox.Controls.Add(Me.fskDeviationNumeric)
        Me.configurationGroupBox.Controls.Add(Me.messageUserDefinedWaveformTextBox)
        Me.configurationGroupBox.Controls.Add(Me.messageWaveformTypeComboBox)
        Me.configurationGroupBox.Controls.Add(Me.modulationTypeComboBox)
        Me.configurationGroupBox.Controls.Add(Me.frequencyReferenceSourceComboBox)
        Me.configurationGroupBox.Location = New System.Drawing.Point(26, 60)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(484, 303)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'messageUserDefinedWaveformLabel
        '
        Me.messageUserDefinedWaveformLabel.AutoSize = True
        Me.messageUserDefinedWaveformLabel.Location = New System.Drawing.Point(22, 196)
        Me.messageUserDefinedWaveformLabel.Name = "messageUserDefinedWaveformLabel"
        Me.messageUserDefinedWaveformLabel.Size = New System.Drawing.Size(262, 13)
        Me.messageUserDefinedWaveformLabel.TabIndex = 10
        Me.messageUserDefinedWaveformLabel.Text = "User Defined Waveform: Enter the bytes (one per line)"
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(22, 77)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.powerLevelLabel.TabIndex = 1
        Me.powerLevelLabel.Text = "Power Level [dBm]"
        '
        'messageSymbolRateLabel
        '
        Me.messageSymbolRateLabel.AutoSize = True
        Me.messageSymbolRateLabel.Location = New System.Drawing.Point(339, 77)
        Me.messageSymbolRateLabel.Name = "messageSymbolRateLabel"
        Me.messageSymbolRateLabel.Size = New System.Drawing.Size(113, 13)
        Me.messageSymbolRateLabel.TabIndex = 3
        Me.messageSymbolRateLabel.Text = "Message Symbol Rate"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.AutoSize = True
        Me.frequencyLabel.Location = New System.Drawing.Point(24, 19)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.frequencyLabel.TabIndex = 0
        Me.frequencyLabel.Text = "Center Frequency [Hz]"
        '
        'messagePrbsOrderLabel
        '
        Me.messagePrbsOrderLabel.AutoSize = True
        Me.messagePrbsOrderLabel.Location = New System.Drawing.Point(339, 135)
        Me.messagePrbsOrderLabel.Name = "messagePrbsOrderLabel"
        Me.messagePrbsOrderLabel.Size = New System.Drawing.Size(65, 13)
        Me.messagePrbsOrderLabel.TabIndex = 4
        Me.messagePrbsOrderLabel.Text = "PRBS Order"
        '
        'frequencyReferenceSourceLabel
        '
        Me.frequencyReferenceSourceLabel.AutoSize = True
        Me.frequencyReferenceSourceLabel.Location = New System.Drawing.Point(22, 135)
        Me.frequencyReferenceSourceLabel.Name = "frequencyReferenceSourceLabel"
        Me.frequencyReferenceSourceLabel.Size = New System.Drawing.Size(147, 13)
        Me.frequencyReferenceSourceLabel.TabIndex = 7
        Me.frequencyReferenceSourceLabel.Text = "Frequency Reference Source"
        '
        'messageWaveformTypeLabel
        '
        Me.messageWaveformTypeLabel.AutoSize = True
        Me.messageWaveformTypeLabel.Location = New System.Drawing.Point(341, 18)
        Me.messageWaveformTypeLabel.Name = "messageWaveformTypeLabel"
        Me.messageWaveformTypeLabel.Size = New System.Drawing.Size(129, 13)
        Me.messageWaveformTypeLabel.TabIndex = 5
        Me.messageWaveformTypeLabel.Text = "Message Waveform Type"
        '
        'modulationTypeLabel
        '
        Me.modulationTypeLabel.AutoSize = True
        Me.modulationTypeLabel.Location = New System.Drawing.Point(182, 19)
        Me.modulationTypeLabel.Name = "modulationTypeLabel"
        Me.modulationTypeLabel.Size = New System.Drawing.Size(86, 13)
        Me.modulationTypeLabel.TabIndex = 6
        Me.modulationTypeLabel.Text = "Modulation Type"
        '
        'fskDeviationLabel
        '
        Me.fskDeviationLabel.AutoSize = True
        Me.fskDeviationLabel.Location = New System.Drawing.Point(180, 77)
        Me.fskDeviationLabel.Name = "fskDeviationLabel"
        Me.fskDeviationLabel.Size = New System.Drawing.Size(75, 13)
        Me.fskDeviationLabel.TabIndex = 8
        Me.fskDeviationLabel.Text = "FSK Deviation"
        '
        'frequencyNumeric
        '
        Me.frequencyNumeric.DecimalPlaces = 6
        Me.frequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.frequencyNumeric.Location = New System.Drawing.Point(23, 35)
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
        Me.powerLevelNumeric.Location = New System.Drawing.Point(23, 93)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerLevelNumeric.TabIndex = 1
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'messageSymbolRateNumeric
        '
        Me.messageSymbolRateNumeric.DecimalPlaces = 2
        Me.messageSymbolRateNumeric.Location = New System.Drawing.Point(340, 93)
        Me.messageSymbolRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.messageSymbolRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.messageSymbolRateNumeric.Name = "messageSymbolRateNumeric"
        Me.messageSymbolRateNumeric.Size = New System.Drawing.Size(120, 20)
        Me.messageSymbolRateNumeric.TabIndex = 6
        Me.messageSymbolRateNumeric.Value = New Decimal(New Integer() {10000, 0, 0, 0})
        '
        'messagePrbsOrderNumeric
        '
        Me.messagePrbsOrderNumeric.Location = New System.Drawing.Point(340, 152)
        Me.messagePrbsOrderNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.messagePrbsOrderNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.messagePrbsOrderNumeric.Name = "messagePrbsOrderNumeric"
        Me.messagePrbsOrderNumeric.Size = New System.Drawing.Size(120, 20)
        Me.messagePrbsOrderNumeric.TabIndex = 7
        Me.messagePrbsOrderNumeric.Value = New Decimal(New Integer() {9, 0, 0, 0})
        '
        'fskDeviationNumeric
        '
        Me.fskDeviationNumeric.DecimalPlaces = 2
        Me.fskDeviationNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.fskDeviationNumeric.Location = New System.Drawing.Point(181, 93)
        Me.fskDeviationNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.fskDeviationNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.fskDeviationNumeric.Name = "fskDeviationNumeric"
        Me.fskDeviationNumeric.Size = New System.Drawing.Size(120, 20)
        Me.fskDeviationNumeric.TabIndex = 4
        Me.fskDeviationNumeric.Value = New Decimal(New Integer() {50000, 0, 0, 0})
        '
        'messageUserDefinedWaveformTextBox
        '
        Me.messageUserDefinedWaveformTextBox.Location = New System.Drawing.Point(23, 214)
        Me.messageUserDefinedWaveformTextBox.Multiline = True
        Me.messageUserDefinedWaveformTextBox.Name = "messageUserDefinedWaveformTextBox"
        Me.messageUserDefinedWaveformTextBox.Size = New System.Drawing.Size(437, 71)
        Me.messageUserDefinedWaveformTextBox.TabIndex = 8
        Me.messageUserDefinedWaveformTextBox.Text = "01"
        '
        'messageWaveformTypeComboBox
        '
        Me.messageWaveformTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.messageWaveformTypeComboBox.Location = New System.Drawing.Point(340, 34)
        Me.messageWaveformTypeComboBox.Name = "messageWaveformTypeComboBox"
        Me.messageWaveformTypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.messageWaveformTypeComboBox.TabIndex = 5
        '
        'modulationTypeComboBox
        '
        Me.modulationTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.modulationTypeComboBox.Location = New System.Drawing.Point(181, 35)
        Me.modulationTypeComboBox.Name = "modulationTypeComboBox"
        Me.modulationTypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.modulationTypeComboBox.TabIndex = 3
        '
        'frequencyReferenceSourceComboBox
        '
        Me.frequencyReferenceSourceComboBox.Location = New System.Drawing.Point(23, 151)
        Me.frequencyReferenceSourceComboBox.Name = "frequencyReferenceSourceComboBox"
        Me.frequencyReferenceSourceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.frequencyReferenceSourceComboBox.TabIndex = 2
        '
        'generateGroupBox
        '
        Me.generateGroupBox.Controls.Add(Me.stopButton)
        Me.generateGroupBox.Controls.Add(Me.statusLabel)
        Me.generateGroupBox.Controls.Add(Me.generateButton)
        Me.generateGroupBox.Controls.Add(Me.statusLed)
        Me.generateGroupBox.Location = New System.Drawing.Point(26, 369)
        Me.generateGroupBox.Name = "generateGroupBox"
        Me.generateGroupBox.Size = New System.Drawing.Size(484, 51)
        Me.generateGroupBox.TabIndex = 2
        Me.generateGroupBox.TabStop = False
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(381, 18)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'statusLabel
        '
        Me.statusLabel.AutoSize = True
        Me.statusLabel.Location = New System.Drawing.Point(61, 23)
        Me.statusLabel.Name = "statusLabel"
        Me.statusLabel.Size = New System.Drawing.Size(92, 13)
        Me.statusLabel.TabIndex = 11
        Me.statusLabel.Text = "Generation Status"
        '
        'generateButton
        '
        Me.generateButton.Location = New System.Drawing.Point(275, 18)
        Me.generateButton.Name = "generateButton"
        Me.generateButton.Size = New System.Drawing.Size(75, 23)
        Me.generateButton.TabIndex = 0
        Me.generateButton.Text = "&Generate"
        Me.generateButton.UseVisualStyleBackColor = True
        '
        'statusLed
        '
        Me.statusLed.BackColor = System.Drawing.SystemColors.Control
        Me.statusLed.Enabled = False
        Me.statusLed.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.statusLed.Location = New System.Drawing.Point(23, 15)
        Me.statusLed.Name = "statusLed"
        Me.statusLed.Size = New System.Drawing.Size(29, 29)
        Me.statusLed.TabIndex = 18
        Me.statusLed.TabStop = False
        Me.statusLed.UseVisualStyleBackColor = False
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(26, 441)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 2
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(26, 17)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 9
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(25, 461)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(485, 66)
        Me.errorTextBox.TabIndex = 7
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(26, 33)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '        '        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True        Me.ClientSize = New System.Drawing.Size(533, 570)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.generateGroupBox)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.resourceNameComboBox)        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "565x Digital Modulation"
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.messageSymbolRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.messagePrbsOrderNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.fskDeviationNumeric, System.ComponentModel.ISupportInitialize).EndInit()
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
    Private messageSymbolRateLabel As System.Windows.Forms.Label
    Private messagePrbsOrderLabel As System.Windows.Forms.Label
    Private messageWaveformTypeLabel As System.Windows.Forms.Label
    Private modulationTypeLabel As System.Windows.Forms.Label
    Private frequencyReferenceSourceLabel As System.Windows.Forms.Label
    Private fskDeviationLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private messageUserDefinedWaveformLabel As System.Windows.Forms.Label
    Private statusLabel As System.Windows.Forms.Label
    Private WithEvents frequencyNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents messageSymbolRateNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents messagePrbsOrderNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents fskDeviationNumeric As System.Windows.Forms.NumericUpDown
    Private errorTextBox As System.Windows.Forms.TextBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents messageUserDefinedWaveformTextBox As System.Windows.Forms.TextBox    Private WithEvents generateButton As System.Windows.Forms.Button
    Private WithEvents messageWaveformTypeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents modulationTypeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents frequencyReferenceSourceComboBox As System.Windows.Forms.ComboBox
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
    Private statusLed As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button

End Class
