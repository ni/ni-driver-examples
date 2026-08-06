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
        Me.centerFrequencyLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.actualIQRateLabel = New System.Windows.Forms.Label()
        Me.numberOfTonesLabel = New System.Windows.Forms.Label()
        Me.initialPhaseLabel = New System.Windows.Forms.Label()
        Me.powerPerToneLabel = New System.Windows.Forms.Label()
        Me.maximumNumberOfSamplesLabel = New System.Windows.Forms.Label()
        Me.frequencyBetweenTonesLabel = New System.Windows.Forms.Label()
        Me.actualPeakPowerLabel = New System.Windows.Forms.Label()
        Me.actualFrequencyBetweenTonesLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.centerFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.numberOfTonesNumeric = New System.Windows.Forms.NumericUpDown()
        Me.initialPhaseNumeric = New System.Windows.Forms.NumericUpDown()
        Me.powerPerToneNumeric = New System.Windows.Forms.NumericUpDown()
        Me.maximumNumberOfSamplesNumeric = New System.Windows.Forms.NumericUpDown()
        Me.frequencyBetweenTonesNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.actualIQRateTextBox = New System.Windows.Forms.TextBox()
        Me.actualPeakPowerTextBox = New System.Windows.Forms.TextBox()
        Me.actualFrequencyBetweenTonesTextBox = New System.Windows.Forms.TextBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()        Me.startButton = New System.Windows.Forms.Button()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.stopButton = New System.Windows.Forms.Button()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageMultitoneParamsGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.centerFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberOfTonesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.initialPhaseNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerPerToneNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maximumNumberOfSamplesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.frequencyBetweenTonesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementGroupBox.SuspendLayout()
        Me.messageMultitoneParamsGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(21, 39)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'centerFrequencyLabel
        '
        Me.centerFrequencyLabel.AutoSize = True
        Me.centerFrequencyLabel.Location = New System.Drawing.Point(21, 88)
        Me.centerFrequencyLabel.Name = "centerFrequencyLabel"
        Me.centerFrequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.centerFrequencyLabel.TabIndex = 1
        Me.centerFrequencyLabel.Text = "Center Frequency (Hz)"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(20, 138)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
        Me.iqRateLabel.TabIndex = 2
        Me.iqRateLabel.Text = "IQ Rate (S/s)"
        '
        'actualIQRateLabel
        '
        Me.actualIQRateLabel.AutoSize = True
        Me.actualIQRateLabel.Location = New System.Drawing.Point(21, 24)
        Me.actualIQRateLabel.Name = "actualIQRateLabel"
        Me.actualIQRateLabel.Size = New System.Drawing.Size(103, 13)
        Me.actualIQRateLabel.TabIndex = 3
        Me.actualIQRateLabel.Text = "Actual IQ Rate (S/s)"
        '
        'numberOfTonesLabel
        '
        Me.numberOfTonesLabel.AutoSize = True
        Me.numberOfTonesLabel.Location = New System.Drawing.Point(14, 22)
        Me.numberOfTonesLabel.Name = "numberOfTonesLabel"
        Me.numberOfTonesLabel.Size = New System.Drawing.Size(89, 13)
        Me.numberOfTonesLabel.TabIndex = 4
        Me.numberOfTonesLabel.Text = "Number of Tones"
        '
        'initialPhaseLabel
        '
        Me.initialPhaseLabel.AutoSize = True
        Me.initialPhaseLabel.Location = New System.Drawing.Point(184, 23)
        Me.initialPhaseLabel.Name = "initialPhaseLabel"
        Me.initialPhaseLabel.Size = New System.Drawing.Size(111, 13)
        Me.initialPhaseLabel.TabIndex = 5
        Me.initialPhaseLabel.Text = "Initial Phase (degrees)"
        '
        'powerPerToneLabel
        '
        Me.powerPerToneLabel.AutoSize = True
        Me.powerPerToneLabel.Location = New System.Drawing.Point(14, 70)
        Me.powerPerToneLabel.Name = "powerPerToneLabel"
        Me.powerPerToneLabel.Size = New System.Drawing.Size(142, 13)
        Me.powerPerToneLabel.TabIndex = 6
        Me.powerPerToneLabel.Text = "Power Level per Tone (dBm)"
        '
        'maximumNumberOfSamplesLabel
        '
        Me.maximumNumberOfSamplesLabel.AutoSize = True
        Me.maximumNumberOfSamplesLabel.Location = New System.Drawing.Point(185, 74)
        Me.maximumNumberOfSamplesLabel.Name = "maximumNumberOfSamplesLabel"
        Me.maximumNumberOfSamplesLabel.Size = New System.Drawing.Size(122, 13)
        Me.maximumNumberOfSamplesLabel.TabIndex = 7
        Me.maximumNumberOfSamplesLabel.Text = "Max Number of Samples"
        '
        'frequencyBetweenTonesLabel
        '
        Me.frequencyBetweenTonesLabel.AutoSize = True
        Me.frequencyBetweenTonesLabel.Location = New System.Drawing.Point(13, 120)
        Me.frequencyBetweenTonesLabel.Name = "frequencyBetweenTonesLabel"
        Me.frequencyBetweenTonesLabel.Size = New System.Drawing.Size(157, 13)
        Me.frequencyBetweenTonesLabel.TabIndex = 8
        Me.frequencyBetweenTonesLabel.Text = "Frequency Between Tones (Hz)"
        '
        'actualPeakPowerLabel
        '
        Me.actualPeakPowerLabel.AutoSize = True
        Me.actualPeakPowerLabel.Location = New System.Drawing.Point(152, 26)
        Me.actualPeakPowerLabel.Name = "actualPeakPowerLabel"
        Me.actualPeakPowerLabel.Size = New System.Drawing.Size(143, 13)
        Me.actualPeakPowerLabel.TabIndex = 10
        Me.actualPeakPowerLabel.Text = "Peak Envelope Power (dBm)"
        '
        'actualFrequencyBetweenTonesLabel
        '
        Me.actualFrequencyBetweenTonesLabel.AutoSize = True
        Me.actualFrequencyBetweenTonesLabel.Location = New System.Drawing.Point(334, 22)
        Me.actualFrequencyBetweenTonesLabel.Name = "actualFrequencyBetweenTonesLabel"
        Me.actualFrequencyBetweenTonesLabel.Size = New System.Drawing.Size(190, 13)
        Me.actualFrequencyBetweenTonesLabel.TabIndex = 11
        Me.actualFrequencyBetweenTonesLabel.Text = "Actual Frequency Between Tones (Hz)"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(22, 323)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 12
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'centerFrequencyNumeric
        '
        Me.centerFrequencyNumeric.DecimalPlaces = 2
        Me.centerFrequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.centerFrequencyNumeric.Location = New System.Drawing.Point(21, 109)
        Me.centerFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.centerFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.centerFrequencyNumeric.Name = "centerFrequencyNumeric"
        Me.centerFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.centerFrequencyNumeric.TabIndex = 1
        Me.centerFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 2
        Me.iqRateNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.iqRateNumeric.Location = New System.Drawing.Point(21, 158)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqRateNumeric.TabIndex = 2
        Me.iqRateNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'numberOfTonesNumeric
        '
        Me.numberOfTonesNumeric.Location = New System.Drawing.Point(14, 43)
        Me.numberOfTonesNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numberOfTonesNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numberOfTonesNumeric.Name = "numberOfTonesNumeric"
        Me.numberOfTonesNumeric.Size = New System.Drawing.Size(120, 20)
        Me.numberOfTonesNumeric.TabIndex = 4
        Me.numberOfTonesNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'initialPhaseNumeric
        '
        Me.initialPhaseNumeric.DecimalPlaces = 2
        Me.initialPhaseNumeric.Location = New System.Drawing.Point(184, 43)
        Me.initialPhaseNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.initialPhaseNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.initialPhaseNumeric.Name = "initialPhaseNumeric"
        Me.initialPhaseNumeric.Size = New System.Drawing.Size(120, 20)
        Me.initialPhaseNumeric.TabIndex = 5
        '
        'powerPerToneNumeric
        '
        Me.powerPerToneNumeric.DecimalPlaces = 2
        Me.powerPerToneNumeric.Location = New System.Drawing.Point(14, 91)
        Me.powerPerToneNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerPerToneNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerPerToneNumeric.Name = "powerPerToneNumeric"
        Me.powerPerToneNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerPerToneNumeric.TabIndex = 6
        Me.powerPerToneNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'maximumNumberOfSamplesNumeric
        '
        Me.maximumNumberOfSamplesNumeric.DecimalPlaces = 2
        Me.maximumNumberOfSamplesNumeric.Location = New System.Drawing.Point(184, 91)
        Me.maximumNumberOfSamplesNumeric.Maximum = New Decimal(New Integer() {-1, 0, 0, 0})
        Me.maximumNumberOfSamplesNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.maximumNumberOfSamplesNumeric.Name = "maximumNumberOfSamplesNumeric"
        Me.maximumNumberOfSamplesNumeric.Size = New System.Drawing.Size(120, 20)
        Me.maximumNumberOfSamplesNumeric.TabIndex = 7
        Me.maximumNumberOfSamplesNumeric.Value = New Decimal(New Integer() {500000, 0, 0, 0})
        '
        'frequencyBetweenTonesNumeric
        '
        Me.frequencyBetweenTonesNumeric.DecimalPlaces = 2
        Me.frequencyBetweenTonesNumeric.Location = New System.Drawing.Point(13, 141)
        Me.frequencyBetweenTonesNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.frequencyBetweenTonesNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.frequencyBetweenTonesNumeric.Name = "frequencyBetweenTonesNumeric"
        Me.frequencyBetweenTonesNumeric.Size = New System.Drawing.Size(120, 20)
        Me.frequencyBetweenTonesNumeric.TabIndex = 8
        Me.frequencyBetweenTonesNumeric.Value = New Decimal(New Integer() {100000, 0, 0, 0})
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(21, 60)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'actualIQRateTextBox
        '
        Me.actualIQRateTextBox.Location = New System.Drawing.Point(24, 43)
        Me.actualIQRateTextBox.Name = "actualIQRateTextBox"
        Me.actualIQRateTextBox.ReadOnly = True
        Me.actualIQRateTextBox.Size = New System.Drawing.Size(76, 20)
        Me.actualIQRateTextBox.TabIndex = 3
        Me.actualIQRateTextBox.TabStop = False
        Me.actualIQRateTextBox.Text = "1000000.00"
        '
        'actualPeakPowerTextBox
        '
        Me.actualPeakPowerTextBox.Location = New System.Drawing.Point(155, 43)
        Me.actualPeakPowerTextBox.Name = "actualPeakPowerTextBox"
        Me.actualPeakPowerTextBox.ReadOnly = True
        Me.actualPeakPowerTextBox.Size = New System.Drawing.Size(132, 20)
        Me.actualPeakPowerTextBox.TabIndex = 10
        Me.actualPeakPowerTextBox.TabStop = False
        Me.actualPeakPowerTextBox.Text = "0.00"
        '
        'actualFrequencyBetweenTonesTextBox
        '
        Me.actualFrequencyBetweenTonesTextBox.Location = New System.Drawing.Point(337, 43)
        Me.actualFrequencyBetweenTonesTextBox.Name = "actualFrequencyBetweenTonesTextBox"
        Me.actualFrequencyBetweenTonesTextBox.ReadOnly = True
        Me.actualFrequencyBetweenTonesTextBox.Size = New System.Drawing.Size(131, 20)
        Me.actualFrequencyBetweenTonesTextBox.TabIndex = 11
        Me.actualFrequencyBetweenTonesTextBox.TabStop = False
        Me.actualFrequencyBetweenTonesTextBox.Text = "0.00000000000000E+0"
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(20, 344)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(538, 47)
        Me.errorTextBox.TabIndex = 14
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No Error"
        '        '        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(321, 397)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 13
        Me.startButton.Text = "Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(402, 397)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 15
        Me.stopButton.Text = "Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyBetweenTonesTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualPeakPowerTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyBetweenTonesLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualPeakPowerLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(21, 209)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(537, 88)
        Me.measurementGroupBox.TabIndex = 16
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'messageMultitoneParamsGroupBox
        '
        Me.messageMultitoneParamsGroupBox.Controls.Add(Me.frequencyBetweenTonesNumeric)
        Me.messageMultitoneParamsGroupBox.Controls.Add(Me.maximumNumberOfSamplesNumeric)
        Me.messageMultitoneParamsGroupBox.Controls.Add(Me.powerPerToneNumeric)
        Me.messageMultitoneParamsGroupBox.Controls.Add(Me.initialPhaseNumeric)
        Me.messageMultitoneParamsGroupBox.Controls.Add(Me.numberOfTonesNumeric)
        Me.messageMultitoneParamsGroupBox.Controls.Add(Me.frequencyBetweenTonesLabel)
        Me.messageMultitoneParamsGroupBox.Controls.Add(Me.numberOfTonesLabel)
        Me.messageMultitoneParamsGroupBox.Controls.Add(Me.maximumNumberOfSamplesLabel)
        Me.messageMultitoneParamsGroupBox.Controls.Add(Me.initialPhaseLabel)
        Me.messageMultitoneParamsGroupBox.Controls.Add(Me.powerPerToneLabel)
        Me.messageMultitoneParamsGroupBox.Location = New System.Drawing.Point(176, 12)
        Me.messageMultitoneParamsGroupBox.Name = "messageMultitoneParamsGroupBox"
        Me.messageMultitoneParamsGroupBox.Size = New System.Drawing.Size(323, 174)
        Me.messageMultitoneParamsGroupBox.TabIndex = 3
        Me.messageMultitoneParamsGroupBox.TabStop = False
        Me.messageMultitoneParamsGroupBox.Text = "Multitone Parameters"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True        Me.ClientSize = New System.Drawing.Size(586, 432)
        Me.Controls.Add(Me.messageMultitoneParamsGroupBox)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.centerFrequencyLabel)
        Me.Controls.Add(Me.iqRateLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.centerFrequencyNumeric)
        Me.Controls.Add(Me.iqRateNumeric)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.errorTextBox)        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Multitone Uniform Spacing"
        CType(Me.centerFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberOfTonesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.initialPhaseNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerPerToneNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maximumNumberOfSamplesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.frequencyBetweenTonesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.messageMultitoneParamsGroupBox.ResumeLayout(False)
        Me.messageMultitoneParamsGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private resourceNameLabel As System.Windows.Forms.Label
    Private centerFrequencyLabel As System.Windows.Forms.Label
    Private iqRateLabel As System.Windows.Forms.Label
    Private actualIQRateLabel As System.Windows.Forms.Label
    Private numberOfTonesLabel As System.Windows.Forms.Label
    Private initialPhaseLabel As System.Windows.Forms.Label
    Private powerPerToneLabel As System.Windows.Forms.Label
    Private maximumNumberOfSamplesLabel As System.Windows.Forms.Label
    Private frequencyBetweenTonesLabel As System.Windows.Forms.Label
    Private actualPeakPowerLabel As System.Windows.Forms.Label
    Private actualFrequencyBetweenTonesLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private centerFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private numberOfTonesNumeric As System.Windows.Forms.NumericUpDown
    Private initialPhaseNumeric As System.Windows.Forms.NumericUpDown
    Private powerPerToneNumeric As System.Windows.Forms.NumericUpDown
    Private maximumNumberOfSamplesNumeric As System.Windows.Forms.NumericUpDown
    Private frequencyBetweenTonesNumeric As System.Windows.Forms.NumericUpDown
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private actualIQRateTextBox As System.Windows.Forms.TextBox
    Private actualPeakPowerTextBox As System.Windows.Forms.TextBox
    Private actualFrequencyBetweenTonesTextBox As System.Windows.Forms.TextBox
    Private errorTextBox As System.Windows.Forms.TextBox    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private measurementGroupBox As System.Windows.Forms.GroupBox
    Private messageMultitoneParamsGroupBox As System.Windows.Forms.GroupBox

End Class
