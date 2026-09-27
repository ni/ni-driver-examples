Imports System

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
        Me.configurationListGroupBox = New System.Windows.Forms.GroupBox()
        Me.startFrequencyLabel = New System.Windows.Forms.Label()
        Me.startPowerLabel = New System.Windows.Forms.Label()
        Me.stopPowerLabel = New System.Windows.Forms.Label()
        Me.stopFrequencyLabel = New System.Windows.Forms.Label()
        Me.numberStepsLabel = New System.Windows.Forms.Label()
        Me.startFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startPowerNumeric = New System.Windows.Forms.NumericUpDown()
        Me.stopPowerNumeric = New System.Windows.Forms.NumericUpDown()
        Me.stopFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.numberStepsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.actualContinuousWaveformToneDurationLabel = New System.Windows.Forms.Label()
        Me.actualIdleWaveformDurationLabel = New System.Windows.Forms.Label()
        Me.actualIQRateLabel = New System.Windows.Forms.Label()
        Me.clockSourceLabel = New System.Windows.Forms.Label()
        Me.loopBandwidthLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.scriptLabel = New System.Windows.Forms.Label()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.actualContinuousWaveformToneDurationTextBox = New System.Windows.Forms.TextBox()
        Me.actualIdleWaveformDurationTextBox = New System.Windows.Forms.TextBox()
        Me.actualIQRateTextBox = New System.Windows.Forms.TextBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.scriptTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.clockSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.loopBandwidthComboBox = New System.Windows.Forms.ComboBox()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.configurationListGroupBox.SuspendLayout()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.startPowerNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.stopPowerNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.stopFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberStepsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.configurationGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'configurationListGroupBox
        '
        Me.configurationListGroupBox.Controls.Add(Me.startFrequencyLabel)
        Me.configurationListGroupBox.Controls.Add(Me.startPowerLabel)
        Me.configurationListGroupBox.Controls.Add(Me.stopPowerLabel)
        Me.configurationListGroupBox.Controls.Add(Me.stopFrequencyLabel)
        Me.configurationListGroupBox.Controls.Add(Me.numberStepsLabel)
        Me.configurationListGroupBox.Controls.Add(Me.startFrequencyNumeric)
        Me.configurationListGroupBox.Controls.Add(Me.startPowerNumeric)
        Me.configurationListGroupBox.Controls.Add(Me.stopPowerNumeric)
        Me.configurationListGroupBox.Controls.Add(Me.stopFrequencyNumeric)
        Me.configurationListGroupBox.Controls.Add(Me.numberStepsNumeric)
        Me.configurationListGroupBox.Location = New System.Drawing.Point(453, 93)
        Me.configurationListGroupBox.Name = "configurationListGroupBox"
        Me.configurationListGroupBox.Size = New System.Drawing.Size(177, 335)
        Me.configurationListGroupBox.TabIndex = 6
        Me.configurationListGroupBox.TabStop = False
        Me.configurationListGroupBox.Text = "Configuration List Parameters"
        '
        'startFrequencyLabel
        '
        Me.startFrequencyLabel.AutoSize = True
        Me.startFrequencyLabel.Location = New System.Drawing.Point(28, 28)
        Me.startFrequencyLabel.Name = "startFrequencyLabel"
        Me.startFrequencyLabel.Size = New System.Drawing.Size(104, 13)
        Me.startFrequencyLabel.TabIndex = 0
        Me.startFrequencyLabel.Text = "Start Frequency [Hz]"
        '
        'startPowerLabel
        '
        Me.startPowerLabel.AutoSize = True
        Me.startPowerLabel.Location = New System.Drawing.Point(25, 164)
        Me.startPowerLabel.Name = "startPowerLabel"
        Me.startPowerLabel.Size = New System.Drawing.Size(92, 13)
        Me.startPowerLabel.TabIndex = 4
        Me.startPowerLabel.Text = "Start Power [dBm]"
        '
        'stopPowerLabel
        '
        Me.stopPowerLabel.AutoSize = True
        Me.stopPowerLabel.Location = New System.Drawing.Point(25, 215)
        Me.stopPowerLabel.Name = "stopPowerLabel"
        Me.stopPowerLabel.Size = New System.Drawing.Size(92, 13)
        Me.stopPowerLabel.TabIndex = 6
        Me.stopPowerLabel.Text = "Stop Power [dBm]"
        '
        'stopFrequencyLabel
        '
        Me.stopFrequencyLabel.AutoSize = True
        Me.stopFrequencyLabel.Location = New System.Drawing.Point(29, 79)
        Me.stopFrequencyLabel.Name = "stopFrequencyLabel"
        Me.stopFrequencyLabel.Size = New System.Drawing.Size(101, 13)
        Me.stopFrequencyLabel.TabIndex = 2
        Me.stopFrequencyLabel.Text = "End Frequency [Hz]"
        '
        'numberStepsLabel
        '
        Me.numberStepsLabel.AutoSize = True
        Me.numberStepsLabel.Location = New System.Drawing.Point(25, 282)
        Me.numberStepsLabel.Name = "numberStepsLabel"
        Me.numberStepsLabel.Size = New System.Drawing.Size(86, 13)
        Me.numberStepsLabel.TabIndex = 8
        Me.numberStepsLabel.Text = "Number of Steps"
        '
        'startFrequencyNumeric
        '
        Me.startFrequencyNumeric.DecimalPlaces = 6
        Me.startFrequencyNumeric.Location = New System.Drawing.Point(25, 48)
        Me.startFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.startFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.startFrequencyNumeric.Name = "startFrequencyNumeric"
        Me.startFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.startFrequencyNumeric.TabIndex = 1
        Me.startFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'startPowerNumeric
        '
        Me.startPowerNumeric.DecimalPlaces = 2
        Me.startPowerNumeric.Location = New System.Drawing.Point(25, 180)
        Me.startPowerNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.startPowerNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.startPowerNumeric.Name = "startPowerNumeric"
        Me.startPowerNumeric.Size = New System.Drawing.Size(120, 20)
        Me.startPowerNumeric.TabIndex = 5
        Me.startPowerNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'stopPowerNumeric
        '
        Me.stopPowerNumeric.DecimalPlaces = 2
        Me.stopPowerNumeric.Location = New System.Drawing.Point(25, 231)
        Me.stopPowerNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.stopPowerNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.stopPowerNumeric.Name = "stopPowerNumeric"
        Me.stopPowerNumeric.Size = New System.Drawing.Size(120, 20)
        Me.stopPowerNumeric.TabIndex = 7
        '
        'stopFrequencyNumeric
        '
        Me.stopFrequencyNumeric.DecimalPlaces = 6
        Me.stopFrequencyNumeric.Location = New System.Drawing.Point(25, 100)
        Me.stopFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.stopFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.stopFrequencyNumeric.Name = "stopFrequencyNumeric"
        Me.stopFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.stopFrequencyNumeric.TabIndex = 3
        Me.stopFrequencyNumeric.Value = New Decimal(New Integer() {1020000000, 0, 0, 0})
        '
        'numberStepsNumeric
        '
        Me.numberStepsNumeric.Location = New System.Drawing.Point(25, 300)
        Me.numberStepsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numberStepsNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numberStepsNumeric.Name = "numberStepsNumeric"
        Me.numberStepsNumeric.Size = New System.Drawing.Size(120, 20)
        Me.numberStepsNumeric.TabIndex = 9
        Me.numberStepsNumeric.Value = New Decimal(New Integer() {21, 0, 0, 0})
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(22, 21)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(17, 24)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
        Me.iqRateLabel.TabIndex = 0
        Me.iqRateLabel.Text = "IQ Rate (S/s)"
        '
        'actualContinuousWaveformToneDurationLabel
        '
        Me.actualContinuousWaveformToneDurationLabel.AutoSize = True
        Me.actualContinuousWaveformToneDurationLabel.Location = New System.Drawing.Point(16, 80)
        Me.actualContinuousWaveformToneDurationLabel.Name = "actualContinuousWaveformToneDurationLabel"
        Me.actualContinuousWaveformToneDurationLabel.Size = New System.Drawing.Size(190, 13)
        Me.actualContinuousWaveformToneDurationLabel.TabIndex = 2
        Me.actualContinuousWaveformToneDurationLabel.Text = "continuousWaveformTone Duration (s)"
        '
        'actualIdleWaveformDurationLabel
        '
        Me.actualIdleWaveformDurationLabel.AutoSize = True
        Me.actualIdleWaveformDurationLabel.Location = New System.Drawing.Point(16, 132)
        Me.actualIdleWaveformDurationLabel.Name = "actualIdleWaveformDurationLabel"
        Me.actualIdleWaveformDurationLabel.Size = New System.Drawing.Size(129, 13)
        Me.actualIdleWaveformDurationLabel.TabIndex = 4
        Me.actualIdleWaveformDurationLabel.Text = "idleWaveform Duration (s)"
        '
        'actualIQRateLabel
        '
        Me.actualIQRateLabel.AutoSize = True
        Me.actualIQRateLabel.Location = New System.Drawing.Point(17, 28)
        Me.actualIQRateLabel.Name = "actualIQRateLabel"
        Me.actualIQRateLabel.Size = New System.Drawing.Size(103, 13)
        Me.actualIQRateLabel.TabIndex = 0
        Me.actualIQRateLabel.Text = "Actual IQ Rate (S/s)"
        '
        'clockSourceLabel
        '
        Me.clockSourceLabel.AutoSize = True
        Me.clockSourceLabel.Location = New System.Drawing.Point(17, 79)
        Me.clockSourceLabel.Name = "clockSourceLabel"
        Me.clockSourceLabel.Size = New System.Drawing.Size(147, 13)
        Me.clockSourceLabel.TabIndex = 2
        Me.clockSourceLabel.Text = "Frequency Reference Source"
        '
        'loopBandwidthLabel
        '
        Me.loopBandwidthLabel.AutoSize = True
        Me.loopBandwidthLabel.Location = New System.Drawing.Point(17, 131)
        Me.loopBandwidthLabel.Name = "loopBandwidthLabel"
        Me.loopBandwidthLabel.Size = New System.Drawing.Size(84, 13)
        Me.loopBandwidthLabel.TabIndex = 4
        Me.loopBandwidthLabel.Text = "Loop Bandwidth"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(23, 450)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 8
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'scriptLabel
        '
        Me.scriptLabel.AutoSize = True
        Me.scriptLabel.Location = New System.Drawing.Point(23, 74)
        Me.scriptLabel.Name = "scriptLabel"
        Me.scriptLabel.Size = New System.Drawing.Size(34, 13)
        Me.scriptLabel.TabIndex = 2
        Me.scriptLabel.Text = "Script"
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 2
        Me.iqRateNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.iqRateNumeric.Location = New System.Drawing.Point(17, 45)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqRateNumeric.TabIndex = 1
        Me.iqRateNumeric.Value = New Decimal(New Integer() {5000000, 0, 0, 0})
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(22, 42)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'actualContinuousWaveformToneDurationTextBox
        '
        Me.actualContinuousWaveformToneDurationTextBox.Location = New System.Drawing.Point(17, 98)
        Me.actualContinuousWaveformToneDurationTextBox.Name = "actualContinuousWaveformToneDurationTextBox"
        Me.actualContinuousWaveformToneDurationTextBox.ReadOnly = True
        Me.actualContinuousWaveformToneDurationTextBox.Size = New System.Drawing.Size(126, 20)
        Me.actualContinuousWaveformToneDurationTextBox.TabIndex = 3
        Me.actualContinuousWaveformToneDurationTextBox.TabStop = False
        Me.actualContinuousWaveformToneDurationTextBox.Text = "0.00"
        '
        'actualIdleWaveformDurationTextBox
        '
        Me.actualIdleWaveformDurationTextBox.Location = New System.Drawing.Point(17, 150)
        Me.actualIdleWaveformDurationTextBox.Name = "actualIdleWaveformDurationTextBox"
        Me.actualIdleWaveformDurationTextBox.ReadOnly = True
        Me.actualIdleWaveformDurationTextBox.Size = New System.Drawing.Size(126, 20)
        Me.actualIdleWaveformDurationTextBox.TabIndex = 5
        Me.actualIdleWaveformDurationTextBox.TabStop = False
        Me.actualIdleWaveformDurationTextBox.Text = "0.00000000000000E+0"
        '
        'actualIQRateTextBox
        '
        Me.actualIQRateTextBox.Location = New System.Drawing.Point(18, 46)
        Me.actualIQRateTextBox.Name = "actualIQRateTextBox"
        Me.actualIQRateTextBox.ReadOnly = True
        Me.actualIQRateTextBox.Size = New System.Drawing.Size(126, 20)
        Me.actualIQRateTextBox.TabIndex = 1
        Me.actualIQRateTextBox.TabStop = False
        Me.actualIQRateTextBox.Text = "0.00"
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(22, 471)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(608, 47)
        Me.errorTextBox.TabIndex = 9
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No Error"
        '
        'scriptTextBox
        '
        Me.scriptTextBox.AcceptsReturn = True
        Me.scriptTextBox.Location = New System.Drawing.Point(22, 93)
        Me.scriptTextBox.Multiline = True
        Me.scriptTextBox.Name = "scriptTextBox"
        Me.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.scriptTextBox.Size = New System.Drawing.Size(414, 138)
        Me.scriptTextBox.TabIndex = 3
        Me.scriptTextBox.Text = "script configurationListControlScript" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    repeat forever" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       generate contin" & _
            "uousWaveformTone" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       generate idleWaveform marker0(0)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    end repeat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "end s" & _
            "cript"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(454, 42)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 7
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(536, 42)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 10
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'clockSourceComboBox
        '
        Me.clockSourceComboBox.Location = New System.Drawing.Point(17, 95)
        Me.clockSourceComboBox.Name = "clockSourceComboBox"
        Me.clockSourceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.clockSourceComboBox.TabIndex = 3
        '
        'loopBandwidthComboBox
        '
        Me.loopBandwidthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.loopBandwidthComboBox.Location = New System.Drawing.Point(17, 149)
        Me.loopBandwidthComboBox.Name = "loopBandwidthComboBox"
        Me.loopBandwidthComboBox.Size = New System.Drawing.Size(120, 21)
        Me.loopBandwidthComboBox.TabIndex = 5
        '
        'rfsgStatusTimer
        '
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.iqRateNumeric)
        Me.configurationGroupBox.Controls.Add(Me.loopBandwidthComboBox)
        Me.configurationGroupBox.Controls.Add(Me.clockSourceComboBox)
        Me.configurationGroupBox.Controls.Add(Me.loopBandwidthLabel)
        Me.configurationGroupBox.Controls.Add(Me.iqRateLabel)
        Me.configurationGroupBox.Controls.Add(Me.clockSourceLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(22, 244)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(177, 186)
        Me.configurationGroupBox.TabIndex = 4
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualIQRateTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualIdleWaveformDurationTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualContinuousWaveformToneDurationTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualIdleWaveformDurationLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualContinuousWaveformToneDurationLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(218, 244)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(218, 184)
        Me.measurementGroupBox.TabIndex = 5
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(661, 563)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.configurationListGroupBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.scriptLabel)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.scriptTextBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Frequency And Power Sweep (Script Triggered)"
        Me.configurationListGroupBox.ResumeLayout(False)
        Me.configurationListGroupBox.PerformLayout()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.startPowerNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.stopPowerNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.stopFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberStepsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private configurationListGroupBox As System.Windows.Forms.GroupBox
    Private resourceNameLabel As System.Windows.Forms.Label
    Private startFrequencyLabel As System.Windows.Forms.Label
    Private startPowerLabel As System.Windows.Forms.Label
    Private stopPowerLabel As System.Windows.Forms.Label
    Private stopFrequencyLabel As System.Windows.Forms.Label
    Private numberStepsLabel As System.Windows.Forms.Label
    Private iqRateLabel As System.Windows.Forms.Label
    Private actualContinuousWaveformToneDurationLabel As System.Windows.Forms.Label
    Private actualIdleWaveformDurationLabel As System.Windows.Forms.Label
    Private actualIQRateLabel As System.Windows.Forms.Label
    Private clockSourceLabel As System.Windows.Forms.Label
    Private loopBandwidthLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private scriptLabel As System.Windows.Forms.Label
    Private startFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private startPowerNumeric As System.Windows.Forms.NumericUpDown
    Private stopPowerNumeric As System.Windows.Forms.NumericUpDown
    Private stopFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private numberStepsNumeric As System.Windows.Forms.NumericUpDown
    Private iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private actualContinuousWaveformToneDurationTextBox As System.Windows.Forms.TextBox
    Private actualIdleWaveformDurationTextBox As System.Windows.Forms.TextBox
    Private actualIQRateTextBox As System.Windows.Forms.TextBox
    Private errorTextBox As System.Windows.Forms.TextBox
    Private scriptTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button    Private clockSourceComboBox As System.Windows.Forms.ComboBox
    Private loopBandwidthComboBox As System.Windows.Forms.ComboBox
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private measurementGroupBox As System.Windows.Forms.GroupBox

End Class
