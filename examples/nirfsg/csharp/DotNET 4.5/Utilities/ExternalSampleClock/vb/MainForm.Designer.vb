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
        Me.externalClockSourceLabel = New System.Windows.Forms.Label()
        Me.vectorSignalGeneratorLabel = New System.Windows.Forms.Label()
        Me.frequencyLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.actualFrequencyOffsetLabel = New System.Windows.Forms.Label()
        Me.actualArbSampleRateLabel = New System.Windows.Forms.Label()
        Me.actualIQRateLabel = New System.Windows.Forms.Label()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.waveformLabel = New System.Windows.Forms.Label()
        Me.arbSampleClockSourceLabel = New System.Windows.Forms.Label()
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.externalClockSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.vectorSignalGeneratorComboBox = New System.Windows.Forms.ComboBox()
        Me.actualIQRateTextBox = New System.Windows.Forms.TextBox()
        Me.actualFrequencyOffsetTextBox = New System.Windows.Forms.TextBox()
        Me.actualArbSampleRateTextBox = New System.Windows.Forms.TextBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()        Me.waveformComboBox = New System.Windows.Forms.ComboBox()
        Me.arbSampleClockSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.configurationParametersGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementGroupBox.SuspendLayout()
        Me.configurationParametersGroupBox.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' externalClockSourceLabel
        ' 
        Me.externalClockSourceLabel.AutoSize = True
        Me.externalClockSourceLabel.Location = New System.Drawing.Point(187, 16)
        Me.externalClockSourceLabel.Name = "externalClockSourceLabel"
        Me.externalClockSourceLabel.Size = New System.Drawing.Size(112, 13)
        Me.externalClockSourceLabel.TabIndex = 0
        Me.externalClockSourceLabel.Text = "External Clock Source"
        ' 
        ' vectorSignalGeneratorLabel
        ' 
        Me.vectorSignalGeneratorLabel.AutoSize = True
        Me.vectorSignalGeneratorLabel.Location = New System.Drawing.Point(25, 16)
        Me.vectorSignalGeneratorLabel.Name = "vectorSignalGeneratorLabel"
        Me.vectorSignalGeneratorLabel.Size = New System.Drawing.Size(120, 13)
        Me.vectorSignalGeneratorLabel.TabIndex = 1
        Me.vectorSignalGeneratorLabel.Text = "Vector Signal Generator"
        ' 
        ' frequencyLabel
        ' 
        Me.frequencyLabel.AutoSize = True
        Me.frequencyLabel.Location = New System.Drawing.Point(21, 23)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.frequencyLabel.TabIndex = 2
        Me.frequencyLabel.Text = "Center Frequency [Hz]"
        ' 
        ' iqRateLabel
        ' 
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(169, 80)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
        Me.iqRateLabel.TabIndex = 3
        Me.iqRateLabel.Text = "IQ Rate (S/s)"
        ' 
        ' actualFrequencyOffsetLabel
        ' 
        Me.actualFrequencyOffsetLabel.AutoSize = True
        Me.actualFrequencyOffsetLabel.Location = New System.Drawing.Point(21, 80)
        Me.actualFrequencyOffsetLabel.Name = "actualFrequencyOffsetLabel"
        Me.actualFrequencyOffsetLabel.Size = New System.Drawing.Size(88, 13)
        Me.actualFrequencyOffsetLabel.TabIndex = 4
        Me.actualFrequencyOffsetLabel.Text = "Frequency Offset"
        ' 
        ' actualArbSampleRateLabel
        ' 
        Me.actualArbSampleRateLabel.AutoSize = True
        Me.actualArbSampleRateLabel.Location = New System.Drawing.Point(21, 23)
        Me.actualArbSampleRateLabel.Name = "actualArbSampleRateLabel"
        Me.actualArbSampleRateLabel.Size = New System.Drawing.Size(113, 13)
        Me.actualArbSampleRateLabel.TabIndex = 5
        Me.actualArbSampleRateLabel.Text = "Arb Sample Rate (S/s)"
        ' 
        ' actualIQRateLabel
        ' 
        Me.actualIQRateLabel.AutoSize = True
        Me.actualIQRateLabel.Location = New System.Drawing.Point(21, 143)
        Me.actualIQRateLabel.Name = "actualIQRateLabel"
        Me.actualIQRateLabel.Size = New System.Drawing.Size(103, 13)
        Me.actualIQRateLabel.TabIndex = 0
        Me.actualIQRateLabel.Text = "Actual IQ Rate (S/s)"
        ' 
        ' powerLevelLabel
        ' 
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(21, 80)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.powerLevelLabel.TabIndex = 6
        Me.powerLevelLabel.Text = "Power Level [dBm]"
        ' 
        ' errorLabel
        ' 
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(25, 289)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 7
        Me.errorLabel.Text = "Warning/Error Message"
        ' 
        ' waveformLabel
        ' 
        Me.waveformLabel.AutoSize = True
        Me.waveformLabel.Location = New System.Drawing.Point(25, 226)
        Me.waveformLabel.Name = "waveformLabel"
        Me.waveformLabel.Size = New System.Drawing.Size(101, 13)
        Me.waveformLabel.TabIndex = 8
        Me.waveformLabel.Text = "Selected Waveform"
        ' 
        ' arbSampleClockSourceLabel
        ' 
        Me.arbSampleClockSourceLabel.AutoSize = True
        Me.arbSampleClockSourceLabel.Location = New System.Drawing.Point(169, 23)
        Me.arbSampleClockSourceLabel.Name = "arbSampleClockSourceLabel"
        Me.arbSampleClockSourceLabel.Size = New System.Drawing.Size(128, 13)
        Me.arbSampleClockSourceLabel.TabIndex = 9
        Me.arbSampleClockSourceLabel.Text = "Arb Sample Clock Source"
        ' 
        ' frequencyNumeric
        ' 
        Me.frequencyNumeric.DecimalPlaces = 6
        Me.frequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.frequencyNumeric.Location = New System.Drawing.Point(19, 44)
        Me.frequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.frequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.frequencyNumeric.Name = "frequencyNumeric"
        Me.frequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.frequencyNumeric.TabIndex = 0
        Me.frequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        ' 
        ' iqRateNumeric
        ' 
        Me.iqRateNumeric.DecimalPlaces = 4
        Me.iqRateNumeric.Location = New System.Drawing.Point(172, 101)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqRateNumeric.TabIndex = 4
        Me.iqRateNumeric.Value = New Decimal(New Integer() {833333333, 0, 0, 131072})
        ' 
        ' powerLevelNumeric
        ' 
        Me.powerLevelNumeric.DecimalPlaces = 2
        Me.powerLevelNumeric.Location = New System.Drawing.Point(20, 101)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerLevelNumeric.TabIndex = 1
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        ' 
        ' externalClockSourceComboBox
        ' 
        Me.externalClockSourceComboBox.Location = New System.Drawing.Point(187, 37)
        Me.externalClockSourceComboBox.Name = "externalClockSourceComboBox"
        Me.externalClockSourceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.externalClockSourceComboBox.TabIndex = 1
        ' 
        ' vectorSignalGeneratorComboBox
        ' 
        Me.vectorSignalGeneratorComboBox.Location = New System.Drawing.Point(22, 37)
        Me.vectorSignalGeneratorComboBox.Name = "vectorSignalGeneratorComboBox"
        Me.vectorSignalGeneratorComboBox.Size = New System.Drawing.Size(120, 21)
        Me.vectorSignalGeneratorComboBox.TabIndex = 0
        ' 
        ' actualIQRateTextBox
        ' 
        Me.actualIQRateTextBox.Location = New System.Drawing.Point(21, 159)
        Me.actualIQRateTextBox.Name = "actualIQRateTextBox"
        Me.actualIQRateTextBox.[ReadOnly] = True
        Me.actualIQRateTextBox.Size = New System.Drawing.Size(109, 20)
        Me.actualIQRateTextBox.TabIndex = 3
        Me.actualIQRateTextBox.TabStop = False
        Me.actualIQRateTextBox.Text = "1000000.0000"
        ' 
        ' actualFrequencyOffsetTextBox
        ' 
        Me.actualFrequencyOffsetTextBox.Location = New System.Drawing.Point(21, 100)
        Me.actualFrequencyOffsetTextBox.Name = "actualFrequencyOffsetTextBox"
        Me.actualFrequencyOffsetTextBox.[ReadOnly] = True
        Me.actualFrequencyOffsetTextBox.Size = New System.Drawing.Size(109, 20)
        Me.actualFrequencyOffsetTextBox.TabIndex = 4
        Me.actualFrequencyOffsetTextBox.TabStop = False
        Me.actualFrequencyOffsetTextBox.Text = "1000000.0000"
        ' 
        ' actualArbSampleRateTextBox
        ' 
        Me.actualArbSampleRateTextBox.Location = New System.Drawing.Point(21, 43)
        Me.actualArbSampleRateTextBox.Name = "actualArbSampleRateTextBox"
        Me.actualArbSampleRateTextBox.[ReadOnly] = True
        Me.actualArbSampleRateTextBox.Size = New System.Drawing.Size(109, 20)
        Me.actualArbSampleRateTextBox.TabIndex = 5
        Me.actualArbSampleRateTextBox.TabStop = False
        Me.actualArbSampleRateTextBox.Text = "1000000.0000"
        ' 
        ' errorTextBox
        ' 
        Me.errorTextBox.Location = New System.Drawing.Point(22, 305)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.[ReadOnly] = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(499, 59)
        Me.errorTextBox.TabIndex = 13
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        ' 
        ' startButton
        ' 
        Me.startButton.Location = New System.Drawing.Point(365, 35)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 5
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True

        ' 
        ' stopButton
        ' 
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(446, 35)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 6
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True

        '         ' 
        ' 
        ' waveformComboBox
        ' 
        Me.waveformComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.waveformComboBox.Location = New System.Drawing.Point(22, 246)
        Me.waveformComboBox.Name = "waveformComboBox"
        Me.waveformComboBox.Size = New System.Drawing.Size(120, 21)
        Me.waveformComboBox.TabIndex = 4
        ' 
        ' arbSampleClockSourceComboBox
        ' 
        Me.arbSampleClockSourceComboBox.Location = New System.Drawing.Point(170, 42)
        Me.arbSampleClockSourceComboBox.Name = "arbSampleClockSourceComboBox"
        Me.arbSampleClockSourceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.arbSampleClockSourceComboBox.TabIndex = 3
        ' 
        ' rfsgStatusTimer
        ' 

        ' 
        ' measurementGroupBox
        ' 
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualArbSampleRateTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyOffsetTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualArbSampleRateLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyOffsetLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(365, 72)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(156, 190)
        Me.measurementGroupBox.TabIndex = 16
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        ' 
        ' configurationParametersGroupBox
        ' 
        Me.configurationParametersGroupBox.Controls.Add(Me.arbSampleClockSourceComboBox)
        Me.configurationParametersGroupBox.Controls.Add(Me.powerLevelNumeric)
        Me.configurationParametersGroupBox.Controls.Add(Me.iqRateNumeric)
        Me.configurationParametersGroupBox.Controls.Add(Me.frequencyNumeric)
        Me.configurationParametersGroupBox.Controls.Add(Me.frequencyLabel)
        Me.configurationParametersGroupBox.Controls.Add(Me.arbSampleClockSourceLabel)
        Me.configurationParametersGroupBox.Controls.Add(Me.iqRateLabel)
        Me.configurationParametersGroupBox.Controls.Add(Me.powerLevelLabel)
        Me.configurationParametersGroupBox.Location = New System.Drawing.Point(22, 72)
        Me.configurationParametersGroupBox.Name = "configurationParametersGroupBox"
        Me.configurationParametersGroupBox.Size = New System.Drawing.Size(318, 140)
        Me.configurationParametersGroupBox.TabIndex = 3
        Me.configurationParametersGroupBox.TabStop = False
        Me.configurationParametersGroupBox.Text = "Configuration Parameters"
        ' 
        ' MainForm
        ' 
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True        Me.ClientSize = New System.Drawing.Size(543, 404)
        Me.Controls.Add(Me.configurationParametersGroupBox)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.externalClockSourceLabel)
        Me.Controls.Add(Me.vectorSignalGeneratorLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.waveformLabel)
        Me.Controls.Add(Me.externalClockSourceComboBox)
        Me.Controls.Add(Me.vectorSignalGeneratorComboBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)        Me.Controls.Add(Me.waveformComboBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "External Sample Clock"
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.configurationParametersGroupBox.ResumeLayout(False)
        Me.configurationParametersGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
#End Region
    Private externalClockSourceLabel As System.Windows.Forms.Label
    Private vectorSignalGeneratorLabel As System.Windows.Forms.Label
    Private frequencyLabel As System.Windows.Forms.Label
    Private iqRateLabel As System.Windows.Forms.Label
    Private actualFrequencyOffsetLabel As System.Windows.Forms.Label
    Private actualArbSampleRateLabel As System.Windows.Forms.Label
    Private actualIQRateLabel As System.Windows.Forms.Label
    Private powerLevelLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private waveformLabel As System.Windows.Forms.Label
    Private arbSampleClockSourceLabel As System.Windows.Forms.Label
    Private frequencyNumeric As System.Windows.Forms.NumericUpDown
    Private iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private externalClockSourceComboBox As System.Windows.Forms.ComboBox
    Private vectorSignalGeneratorComboBox As System.Windows.Forms.ComboBox
    Private actualIQRateTextBox As System.Windows.Forms.TextBox
    Private actualFrequencyOffsetTextBox As System.Windows.Forms.TextBox
    Private actualArbSampleRateTextBox As System.Windows.Forms.TextBox
    Private errorTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button    Private waveformComboBox As System.Windows.Forms.ComboBox
    Private arbSampleClockSourceComboBox As System.Windows.Forms.ComboBox
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
    Private measurementGroupBox As System.Windows.Forms.GroupBox
    Private configurationParametersGroupBox As System.Windows.Forms.GroupBox
End Class
