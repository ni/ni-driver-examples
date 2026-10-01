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
        Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.centerFrequencyLabel = New System.Windows.Forms.Label()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.loGroupBox = New System.Windows.Forms.GroupBox()
        Me.outputTerminalLabel = New System.Windows.Forms.Label()
        Me.loSwitchLabel = New System.Windows.Forms.Label()
        Me.outputTerminalComboBox = New System.Windows.Forms.ComboBox()
        Me.frequencyReferenceSourceLabel = New System.Windows.Forms.Label()
        Me.frequencyReferenceSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.loSwitchComboBox = New System.Windows.Forms.ComboBox()
        Me.actualGainLabel = New System.Windows.Forms.Label()
        Me.actualSkewLabel = New System.Windows.Forms.Label()
        Me.actualGainImbalanceLabel = New System.Windows.Forms.Label()
        Me.actualQOffsetLabel = New System.Windows.Forms.Label()
        Me.actualIOffsetLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.centerFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.actualGainTextBox = New System.Windows.Forms.TextBox()
        Me.actualSkewTextBox = New System.Windows.Forms.TextBox()
        Me.actualGainImbalanceTextBox = New System.Windows.Forms.TextBox()
        Me.actualQOffsetTextBox = New System.Windows.Forms.TextBox()
        Me.actualIOffsetTextBox = New System.Windows.Forms.TextBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()        Me.stopButton = New System.Windows.Forms.Button()
        Me.startButton = New System.Windows.Forms.Button()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.loGroupBox.SuspendLayout()
        CType(Me.centerFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' centerFrequencyLabel
        ' 
        Me.centerFrequencyLabel.AutoSize = True
        Me.centerFrequencyLabel.Location = New System.Drawing.Point(16, 67)
        Me.centerFrequencyLabel.Name = "centerFrequencyLabel"
        Me.centerFrequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.centerFrequencyLabel.TabIndex = 0
        Me.centerFrequencyLabel.Text = "Center Frequency (Hz)"
        ' 
        ' powerLevelLabel
        ' 
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(16, 120)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(51, 13)
        Me.powerLevelLabel.TabIndex = 1
        Me.powerLevelLabel.Text = "Gain (dB)"
        ' 
        ' loGroupBox
        ' 
        Me.loGroupBox.Controls.Add(Me.outputTerminalLabel)
        Me.loGroupBox.Controls.Add(Me.loSwitchLabel)
        Me.loGroupBox.Controls.Add(Me.outputTerminalComboBox)
        Me.loGroupBox.Controls.Add(Me.frequencyReferenceSourceLabel)
        Me.loGroupBox.Controls.Add(Me.frequencyReferenceSourceComboBox)
        Me.loGroupBox.Controls.Add(Me.loSwitchComboBox)
        Me.loGroupBox.Location = New System.Drawing.Point(17, 173)
        Me.loGroupBox.Name = "loGroupBox"
        Me.loGroupBox.Size = New System.Drawing.Size(215, 172)
        Me.loGroupBox.TabIndex = 3
        Me.loGroupBox.TabStop = False
        Me.loGroupBox.Text = "LO Parameters"
        ' 
        ' outputTerminalLabel
        ' 
        Me.outputTerminalLabel.AutoSize = True
        Me.outputTerminalLabel.Location = New System.Drawing.Point(17, 117)
        Me.outputTerminalLabel.Name = "outputTerminalLabel"
        Me.outputTerminalLabel.Size = New System.Drawing.Size(188, 13)
        Me.outputTerminalLabel.TabIndex = 18
        Me.outputTerminalLabel.Text = "Frequency Reference Output Terminal"
        ' 
        ' loSwitchLabel
        ' 
        Me.loSwitchLabel.AutoSize = True
        Me.loSwitchLabel.Location = New System.Drawing.Point(19, 16)
        Me.loSwitchLabel.Name = "loSwitchLabel"
        Me.loSwitchLabel.Size = New System.Drawing.Size(21, 13)
        Me.loSwitchLabel.TabIndex = 14
        Me.loSwitchLabel.Text = "LO"
        ' 
        ' outputTerminalComboBox
        ' 
        Me.outputTerminalComboBox.FormattingEnabled = True
        Me.outputTerminalComboBox.Location = New System.Drawing.Point(17, 138)
        Me.outputTerminalComboBox.Name = "outputTerminalComboBox"
        Me.outputTerminalComboBox.Size = New System.Drawing.Size(120, 21)
        Me.outputTerminalComboBox.TabIndex = 15
        ' 
        ' frequencyReferenceSourceLabel
        ' 
        Me.frequencyReferenceSourceLabel.AutoSize = True
        Me.frequencyReferenceSourceLabel.Location = New System.Drawing.Point(17, 66)
        Me.frequencyReferenceSourceLabel.Name = "frequencyReferenceSourceLabel"
        Me.frequencyReferenceSourceLabel.Size = New System.Drawing.Size(147, 13)
        Me.frequencyReferenceSourceLabel.TabIndex = 16
        Me.frequencyReferenceSourceLabel.Text = "Frequency Reference Source"
        ' 
        ' frequencyReferenceSourceComboBox
        ' 
        Me.frequencyReferenceSourceComboBox.FormattingEnabled = True
        Me.frequencyReferenceSourceComboBox.Location = New System.Drawing.Point(17, 87)
        Me.frequencyReferenceSourceComboBox.Name = "frequencyReferenceSourceComboBox"
        Me.frequencyReferenceSourceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.frequencyReferenceSourceComboBox.TabIndex = 14
        ' 
        ' loSwitchComboBox
        ' 
        Me.loSwitchComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.loSwitchComboBox.FormattingEnabled = True
        Me.loSwitchComboBox.Location = New System.Drawing.Point(17, 37)
        Me.loSwitchComboBox.Name = "loSwitchComboBox"
        Me.loSwitchComboBox.Size = New System.Drawing.Size(120, 21)
        Me.loSwitchComboBox.TabIndex = 13

        ' 
        ' actualGainLabel
        ' 
        Me.actualGainLabel.AutoSize = True
        Me.actualGainLabel.Location = New System.Drawing.Point(16, 25)
        Me.actualGainLabel.Name = "actualGainLabel"
        Me.actualGainLabel.Size = New System.Drawing.Size(84, 13)
        Me.actualGainLabel.TabIndex = 2
        Me.actualGainLabel.Text = "Actual Gain (dB)"
        ' 
        ' actualSkewLabel
        ' 
        Me.actualSkewLabel.AutoSize = True
        Me.actualSkewLabel.Location = New System.Drawing.Point(116, 144)
        Me.actualSkewLabel.Name = "actualSkewLabel"
        Me.actualSkewLabel.Size = New System.Drawing.Size(61, 13)
        Me.actualSkewLabel.TabIndex = 3
        Me.actualSkewLabel.Text = "Skew (deg)"
        ' 
        ' actualGainImbalanceLabel
        ' 
        Me.actualGainImbalanceLabel.AutoSize = True
        Me.actualGainImbalanceLabel.Location = New System.Drawing.Point(16, 144)
        Me.actualGainImbalanceLabel.Name = "actualGainImbalanceLabel"
        Me.actualGainImbalanceLabel.Size = New System.Drawing.Size(71, 13)
        Me.actualGainImbalanceLabel.TabIndex = 4
        Me.actualGainImbalanceLabel.Text = "Gain Imb (dB)"
        ' 
        ' actualQOffsetLabel
        ' 
        Me.actualQOffsetLabel.AutoSize = True
        Me.actualQOffsetLabel.Location = New System.Drawing.Point(116, 94)
        Me.actualQOffsetLabel.Name = "actualQOffsetLabel"
        Me.actualQOffsetLabel.Size = New System.Drawing.Size(62, 13)
        Me.actualQOffsetLabel.TabIndex = 5
        Me.actualQOffsetLabel.Text = "Q Offset (V)"
        ' 
        ' actualIOffsetLabel
        ' 
        Me.actualIOffsetLabel.AutoSize = True
        Me.actualIOffsetLabel.Location = New System.Drawing.Point(16, 94)
        Me.actualIOffsetLabel.Name = "actualIOffsetLabel"
        Me.actualIOffsetLabel.Size = New System.Drawing.Size(57, 13)
        Me.actualIOffsetLabel.TabIndex = 6
        Me.actualIOffsetLabel.Text = "I Offset (V)"
        ' 
        ' errorLabel
        ' 
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(16, 367)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 7
        Me.errorLabel.Text = "Warning/Error Message"
        ' 
        ' resourceNameLabel
        ' 
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(16, 22)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 8
        Me.resourceNameLabel.Text = "Resource Name:"
        ' 
        ' centerFrequencyNumeric
        ' 
        Me.centerFrequencyNumeric.DecimalPlaces = 6
        Me.centerFrequencyNumeric.Location = New System.Drawing.Point(17, 87)
        Me.centerFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.centerFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.centerFrequencyNumeric.Name = "centerFrequencyNumeric"
        Me.centerFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.centerFrequencyNumeric.TabIndex = 1
        Me.centerFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})

        ' 
        ' powerLevelNumeric
        ' 
        Me.powerLevelNumeric.DecimalPlaces = 2
        Me.powerLevelNumeric.Location = New System.Drawing.Point(17, 137)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerLevelNumeric.TabIndex = 2
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})

        ' 
        ' actualGainTextBox
        ' 
        Me.actualGainTextBox.Location = New System.Drawing.Point(15, 42)
        Me.actualGainTextBox.Name = "actualGainTextBox"
        Me.actualGainTextBox.[ReadOnly] = True
        Me.actualGainTextBox.Size = New System.Drawing.Size(176, 20)
        Me.actualGainTextBox.TabIndex = 2
        Me.actualGainTextBox.TabStop = False
        Me.actualGainTextBox.Text = "0.00"
        ' 
        ' actualSkewTextBox
        ' 
        Me.actualSkewTextBox.Location = New System.Drawing.Point(116, 165)
        Me.actualSkewTextBox.Name = "actualSkewTextBox"
        Me.actualSkewTextBox.[ReadOnly] = True
        Me.actualSkewTextBox.Size = New System.Drawing.Size(75, 20)
        Me.actualSkewTextBox.TabIndex = 3
        Me.actualSkewTextBox.TabStop = False
        Me.actualSkewTextBox.Text = "0.00"
        ' 
        ' actualGainImbalanceTextBox
        ' 
        Me.actualGainImbalanceTextBox.Location = New System.Drawing.Point(16, 165)
        Me.actualGainImbalanceTextBox.Name = "actualGainImbalanceTextBox"
        Me.actualGainImbalanceTextBox.[ReadOnly] = True
        Me.actualGainImbalanceTextBox.Size = New System.Drawing.Size(75, 20)
        Me.actualGainImbalanceTextBox.TabIndex = 4
        Me.actualGainImbalanceTextBox.TabStop = False
        Me.actualGainImbalanceTextBox.Text = "0.000"
        ' 
        ' actualQOffsetTextBox
        ' 
        Me.actualQOffsetTextBox.Location = New System.Drawing.Point(116, 115)
        Me.actualQOffsetTextBox.Name = "actualQOffsetTextBox"
        Me.actualQOffsetTextBox.[ReadOnly] = True
        Me.actualQOffsetTextBox.Size = New System.Drawing.Size(75, 20)
        Me.actualQOffsetTextBox.TabIndex = 5
        Me.actualQOffsetTextBox.TabStop = False
        Me.actualQOffsetTextBox.Text = "0.00"
        ' 
        ' actualIOffsetTextBox
        ' 
        Me.actualIOffsetTextBox.Location = New System.Drawing.Point(16, 115)
        Me.actualIOffsetTextBox.Name = "actualIOffsetTextBox"
        Me.actualIOffsetTextBox.[ReadOnly] = True
        Me.actualIOffsetTextBox.Size = New System.Drawing.Size(75, 20)
        Me.actualIOffsetTextBox.TabIndex = 6
        Me.actualIOffsetTextBox.TabStop = False
        Me.actualIOffsetTextBox.Text = "0.00"
        ' 
        ' errorTextBox
        ' 
        Me.errorTextBox.Location = New System.Drawing.Point(17, 383)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.[ReadOnly] = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(449, 34)
        Me.errorTextBox.TabIndex = 7
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        ' 
        ' resourceNameComboBox
        ' 
        Me.resourceNameComboBox.Location = New System.Drawing.Point(17, 38)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '         ' 
        ' 
        ' stopButton
        ' 
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(391, 36)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 5
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True

        ' 
        ' startButton
        ' 
        Me.startButton.Location = New System.Drawing.Point(310, 36)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 4
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True

        ' 
        ' rfsgStatusTimer
        ' 
        Me.rfsgStatusTimer.Interval = 50

        ' 
        ' measurementGroupBox
        ' 
        Me.measurementGroupBox.Controls.Add(Me.actualGainTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualIOffsetTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualQOffsetTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualGainImbalanceTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualGainLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualSkewTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualSkewLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualIOffsetLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualGainImbalanceLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualQOffsetLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(252, 95)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(214, 208)
        Me.measurementGroupBox.TabIndex = 18
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        ' 
        ' MainForm
        ' 
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True        Me.ClientSize = New System.Drawing.Size(488, 451)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.loGroupBox)
        Me.Controls.Add(Me.centerFrequencyLabel)
        Me.Controls.Add(Me.powerLevelLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.centerFrequencyNumeric)
        Me.Controls.Add(Me.powerLevelNumeric)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.resourceNameComboBox)        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "External AWG (5611)"
        Me.loGroupBox.ResumeLayout(False)
        Me.loGroupBox.PerformLayout()
        CType(Me.centerFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private loGroupBox As System.Windows.Forms.GroupBox
    Private centerFrequencyLabel As System.Windows.Forms.Label
    Private powerLevelLabel As System.Windows.Forms.Label
    Private actualGainLabel As System.Windows.Forms.Label
    Private actualSkewLabel As System.Windows.Forms.Label
    Private actualGainImbalanceLabel As System.Windows.Forms.Label
    Private actualQOffsetLabel As System.Windows.Forms.Label
    Private actualIOffsetLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private WithEvents centerFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private actualGainTextBox As System.Windows.Forms.TextBox
    Private actualSkewTextBox As System.Windows.Forms.TextBox
    Private actualGainImbalanceTextBox As System.Windows.Forms.TextBox
    Private actualQOffsetTextBox As System.Windows.Forms.TextBox
    Private actualIOffsetTextBox As System.Windows.Forms.TextBox
    Private errorTextBox As System.Windows.Forms.TextBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
    Private WithEvents loSwitchComboBox As System.Windows.Forms.ComboBox
    Private loSwitchLabel As System.Windows.Forms.Label
    Private frequencyReferenceSourceLabel As System.Windows.Forms.Label
    Private frequencyReferenceSourceComboBox As System.Windows.Forms.ComboBox
    Private outputTerminalLabel As System.Windows.Forms.Label
    Private outputTerminalComboBox As System.Windows.Forms.ComboBox
    Private measurementGroupBox As System.Windows.Forms.GroupBox

End Class
