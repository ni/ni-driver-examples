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
        Me.scriptLabel = New System.Windows.Forms.Label()
        Me.scriptTriggerParametersGroupBox = New System.Windows.Forms.GroupBox()
        Me.trigger2TypeLabel = New System.Windows.Forms.Label()
        Me.trigger1TypeLabel = New System.Windows.Forms.Label()
        Me.triggerSource2Label = New System.Windows.Forms.Label()
        Me.triggerSource1Label = New System.Windows.Forms.Label()
        Me.trigger2TypeComboBox = New System.Windows.Forms.ComboBox()
        Me.trigger1TypeComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerSource2ComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerSource1ComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.frequencyLabel = New System.Windows.Forms.Label()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.textmessageLabel = New System.Windows.Forms.Label()
        Me.actualFrequencyOffsetLabel = New System.Windows.Forms.Label()
        Me.actualIQRateLabel = New System.Windows.Forms.Label()
        Me.scriptIndexLabel = New System.Windows.Forms.Label()
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.scriptIndexNumeric = New System.Windows.Forms.NumericUpDown()
        Me.scriptTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.actualFrequencyOffsetTextBox = New System.Windows.Forms.TextBox()
        Me.actualIQRateTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.scriptTriggerParametersGroupBox.SuspendLayout()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.scriptIndexNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.configurationGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'scriptLabel
        '
        Me.scriptLabel.AutoSize = True
        Me.scriptLabel.Location = New System.Drawing.Point(19, 203)
        Me.scriptLabel.Name = "scriptLabel"
        Me.scriptLabel.Size = New System.Drawing.Size(34, 13)
        Me.scriptLabel.TabIndex = 0
        Me.scriptLabel.Text = "Script"
        '
        'scriptTriggerParametersGroupBox
        '
        Me.scriptTriggerParametersGroupBox.Controls.Add(Me.trigger2TypeLabel)
        Me.scriptTriggerParametersGroupBox.Controls.Add(Me.trigger1TypeLabel)
        Me.scriptTriggerParametersGroupBox.Controls.Add(Me.triggerSource2Label)
        Me.scriptTriggerParametersGroupBox.Controls.Add(Me.triggerSource1Label)
        Me.scriptTriggerParametersGroupBox.Controls.Add(Me.trigger2TypeComboBox)
        Me.scriptTriggerParametersGroupBox.Controls.Add(Me.trigger1TypeComboBox)
        Me.scriptTriggerParametersGroupBox.Controls.Add(Me.triggerSource2ComboBox)
        Me.scriptTriggerParametersGroupBox.Controls.Add(Me.triggerSource1ComboBox)
        Me.scriptTriggerParametersGroupBox.Location = New System.Drawing.Point(19, 69)
        Me.scriptTriggerParametersGroupBox.Name = "scriptTriggerParametersGroupBox"
        Me.scriptTriggerParametersGroupBox.Size = New System.Drawing.Size(290, 119)
        Me.scriptTriggerParametersGroupBox.TabIndex = 2
        Me.scriptTriggerParametersGroupBox.TabStop = False
        Me.scriptTriggerParametersGroupBox.Text = "Script Trigger Parameters"
        '
        'trigger2TypeLabel
        '
        Me.trigger2TypeLabel.AutoSize = True
        Me.trigger2TypeLabel.Location = New System.Drawing.Point(10, 71)
        Me.trigger2TypeLabel.Name = "trigger2TypeLabel"
        Me.trigger2TypeLabel.Size = New System.Drawing.Size(106, 13)
        Me.trigger2TypeLabel.TabIndex = 2
        Me.trigger2TypeLabel.Text = "Script Trigger 2 Type"
        '
        'trigger1TypeLabel
        '
        Me.trigger1TypeLabel.AutoSize = True
        Me.trigger1TypeLabel.Location = New System.Drawing.Point(10, 21)
        Me.trigger1TypeLabel.Name = "trigger1TypeLabel"
        Me.trigger1TypeLabel.Size = New System.Drawing.Size(103, 13)
        Me.trigger1TypeLabel.TabIndex = 3
        Me.trigger1TypeLabel.Text = "Script Trigger 1Type"
        '
        'triggerSource2Label
        '
        Me.triggerSource2Label.AutoSize = True
        Me.triggerSource2Label.Location = New System.Drawing.Point(154, 71)
        Me.triggerSource2Label.Name = "triggerSource2Label"
        Me.triggerSource2Label.Size = New System.Drawing.Size(116, 13)
        Me.triggerSource2Label.TabIndex = 4
        Me.triggerSource2Label.Text = "Script Trigger Source 2"
        '
        'triggerSource1Label
        '
        Me.triggerSource1Label.AutoSize = True
        Me.triggerSource1Label.Location = New System.Drawing.Point(154, 21)
        Me.triggerSource1Label.Name = "triggerSource1Label"
        Me.triggerSource1Label.Size = New System.Drawing.Size(116, 13)
        Me.triggerSource1Label.TabIndex = 5
        Me.triggerSource1Label.Text = "Script Trigger Source 1"
        '
        'trigger2TypeComboBox
        '
        Me.trigger2TypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.trigger2TypeComboBox.Location = New System.Drawing.Point(10, 91)
        Me.trigger2TypeComboBox.Name = "trigger2TypeComboBox"
        Me.trigger2TypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.trigger2TypeComboBox.TabIndex = 1
        '
        'trigger1TypeComboBox
        '
        Me.trigger1TypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.trigger1TypeComboBox.Location = New System.Drawing.Point(10, 41)
        Me.trigger1TypeComboBox.Name = "trigger1TypeComboBox"
        Me.trigger1TypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.trigger1TypeComboBox.TabIndex = 0
        '
        'triggerSource2ComboBox
        '
        Me.triggerSource2ComboBox.Location = New System.Drawing.Point(154, 91)
        Me.triggerSource2ComboBox.Name = "triggerSource2ComboBox"
        Me.triggerSource2ComboBox.Size = New System.Drawing.Size(120, 21)
        Me.triggerSource2ComboBox.TabIndex = 3
        '
        'triggerSource1ComboBox
        '
        Me.triggerSource1ComboBox.Location = New System.Drawing.Point(154, 41)
        Me.triggerSource1ComboBox.Name = "triggerSource1ComboBox"
        Me.triggerSource1ComboBox.Size = New System.Drawing.Size(120, 21)
        Me.triggerSource1ComboBox.TabIndex = 2
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(18, 6)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 1
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.AutoSize = True
        Me.frequencyLabel.Location = New System.Drawing.Point(18, 21)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.frequencyLabel.TabIndex = 6
        Me.frequencyLabel.Text = "Center Frequency [Hz]"
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(18, 71)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.powerLevelLabel.TabIndex = 7
        Me.powerLevelLabel.Text = "Power Level [dBm]"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(162, 21)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
        Me.iqRateLabel.TabIndex = 8
        Me.iqRateLabel.Text = "IQ Rate [S/s]"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(21, 427)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 9
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'actualFrequencyOffsetLabel
        '
        Me.actualFrequencyOffsetLabel.AutoSize = True
        Me.actualFrequencyOffsetLabel.Location = New System.Drawing.Point(19, 28)
        Me.actualFrequencyOffsetLabel.Name = "actualFrequencyOffsetLabel"
        Me.actualFrequencyOffsetLabel.Size = New System.Drawing.Size(110, 13)
        Me.actualFrequencyOffsetLabel.TabIndex = 11
        Me.actualFrequencyOffsetLabel.Text = "Frequency Offset [Hz]"
        '
        'actualIQRateLabel
        '
        Me.actualIQRateLabel.AutoSize = True
        Me.actualIQRateLabel.Location = New System.Drawing.Point(162, 28)
        Me.actualIQRateLabel.Name = "actualIQRateLabel"
        Me.actualIQRateLabel.Size = New System.Drawing.Size(103, 13)
        Me.actualIQRateLabel.TabIndex = 12
        Me.actualIQRateLabel.Text = "Actual IQ Rate [S/s]"
        '
        'scriptIndexLabel
        '
        Me.scriptIndexLabel.AutoSize = True
        Me.scriptIndexLabel.Location = New System.Drawing.Point(16, 204)
        Me.scriptIndexLabel.Name = "scriptIndexLabel"
        Me.scriptIndexLabel.Size = New System.Drawing.Size(0, 13)
        Me.scriptIndexLabel.TabIndex = 14
        '
        'frequencyNumeric
        '
        Me.frequencyNumeric.DecimalPlaces = 6
        Me.frequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.frequencyNumeric.Location = New System.Drawing.Point(18, 42)
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
        Me.powerLevelNumeric.Location = New System.Drawing.Point(18, 92)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerLevelNumeric.TabIndex = 1
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 6
        Me.iqRateNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.iqRateNumeric.Location = New System.Drawing.Point(162, 42)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqRateNumeric.TabIndex = 2
        Me.iqRateNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'scriptIndexNumeric
        '
        Me.scriptIndexNumeric.Location = New System.Drawing.Point(19, 224)
        Me.scriptIndexNumeric.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.scriptIndexNumeric.Name = "scriptIndexNumeric"
        Me.scriptIndexNumeric.Size = New System.Drawing.Size(34, 20)
        Me.scriptIndexNumeric.TabIndex = 3
        '
        'scriptTextBox
        '
        Me.scriptTextBox.Location = New System.Drawing.Point(59, 224)
        Me.scriptTextBox.Multiline = True
        Me.scriptTextBox.Name = "scriptTextBox"
        Me.scriptTextBox.ReadOnly = True
        Me.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.scriptTextBox.Size = New System.Drawing.Size(250, 190)
        Me.scriptTextBox.TabIndex = 4
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(18, 27)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(21, 448)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(606, 34)
        Me.errorTextBox.TabIndex = 14
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'actualFrequencyOffsetTextBox
        '
        Me.actualFrequencyOffsetTextBox.Location = New System.Drawing.Point(19, 49)
        Me.actualFrequencyOffsetTextBox.Name = "actualFrequencyOffsetTextBox"
        Me.actualFrequencyOffsetTextBox.ReadOnly = True
        Me.actualFrequencyOffsetTextBox.Size = New System.Drawing.Size(109, 20)
        Me.actualFrequencyOffsetTextBox.TabIndex = 16
        Me.actualFrequencyOffsetTextBox.TabStop = False
        Me.actualFrequencyOffsetTextBox.Text = "0.000000"
        '
        'actualIQRateTextBox
        '
        Me.actualIQRateTextBox.Location = New System.Drawing.Point(162, 49)
        Me.actualIQRateTextBox.Name = "actualIQRateTextBox"
        Me.actualIQRateTextBox.ReadOnly = True
        Me.actualIQRateTextBox.Size = New System.Drawing.Size(109, 20)
        Me.actualIQRateTextBox.TabIndex = 17
        Me.actualIQRateTextBox.TabStop = False
        Me.actualIQRateTextBox.Text = "0.000000"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(466, 24)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 6
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(552, 25)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 7
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'rfsgStatusTimer
        '
        Me.rfsgStatusTimer.Interval = 1
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.iqRateNumeric)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.frequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.iqRateLabel)
        Me.configurationGroupBox.Controls.Add(Me.frequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(330, 69)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(297, 119)
        Me.configurationGroupBox.TabIndex = 5
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyOffsetTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyOffsetLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(330, 216)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(297, 86)
        Me.measurementGroupBox.TabIndex = 22
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(646, 517)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.scriptTriggerParametersGroupBox)
        Me.Controls.Add(Me.scriptLabel)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.textmessageLabel)
        Me.Controls.Add(Me.scriptIndexLabel)
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
        Me.Text = "Script Trigger - Hardware Source"
        Me.scriptTriggerParametersGroupBox.ResumeLayout(False)
        Me.scriptTriggerParametersGroupBox.PerformLayout()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.scriptIndexNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private scriptTriggerParametersGroupBox As System.Windows.Forms.GroupBox
    Private scriptLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private trigger2TypeLabel As System.Windows.Forms.Label
    Private trigger1TypeLabel As System.Windows.Forms.Label
    Private triggerSource2Label As System.Windows.Forms.Label
    Private triggerSource1Label As System.Windows.Forms.Label
    Private frequencyLabel As System.Windows.Forms.Label
    Private powerLevelLabel As System.Windows.Forms.Label
    Private iqRateLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private textmessageLabel As System.Windows.Forms.Label
    Private actualFrequencyOffsetLabel As System.Windows.Forms.Label
    Private actualIQRateLabel As System.Windows.Forms.Label
    Private scriptIndexLabel As System.Windows.Forms.Label
    Private frequencyNumeric As System.Windows.Forms.NumericUpDown
    Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents scriptIndexNumeric As System.Windows.Forms.NumericUpDown
    Private scriptTextBox As System.Windows.Forms.TextBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private errorTextBox As System.Windows.Forms.TextBox
    Private actualFrequencyOffsetTextBox As System.Windows.Forms.TextBox
    Private actualIQRateTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button    Private trigger2TypeComboBox As System.Windows.Forms.ComboBox
    Private trigger1TypeComboBox As System.Windows.Forms.ComboBox
    Private triggerSource2ComboBox As System.Windows.Forms.ComboBox
    Private triggerSource1ComboBox As System.Windows.Forms.ComboBox
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private measurementGroupBox As System.Windows.Forms.GroupBox

End Class
