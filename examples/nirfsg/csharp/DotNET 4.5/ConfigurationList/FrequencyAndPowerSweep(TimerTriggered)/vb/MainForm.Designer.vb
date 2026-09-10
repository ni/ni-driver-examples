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
        Me.dwellTimeLabel = New System.Windows.Forms.Label()
        Me.startFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startPowerNumeric = New System.Windows.Forms.NumericUpDown()
        Me.stopPowerNumeric = New System.Windows.Forms.NumericUpDown()
        Me.stopFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.numberStepsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.dwellTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.actualIQRateLabel = New System.Windows.Forms.Label()
        Me.clockSourceLabel = New System.Windows.Forms.Label()
        Me.loopBandwidthLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.scriptLabel = New System.Windows.Forms.Label()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.actualIQRateTextBox = New System.Windows.Forms.TextBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.scriptTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.clockSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.loopBandwidthComboBox = New System.Windows.Forms.ComboBox()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.configurationListGroupBox.SuspendLayout()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.startPowerNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.stopPowerNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.stopFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberStepsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dwellTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'configurationListGroupBox
        '
        Me.configurationListGroupBox.Controls.Add(Me.startFrequencyLabel)
        Me.configurationListGroupBox.Controls.Add(Me.startPowerLabel)
        Me.configurationListGroupBox.Controls.Add(Me.stopPowerLabel)
        Me.configurationListGroupBox.Controls.Add(Me.stopFrequencyLabel)
        Me.configurationListGroupBox.Controls.Add(Me.numberStepsLabel)
        Me.configurationListGroupBox.Controls.Add(Me.dwellTimeLabel)
        Me.configurationListGroupBox.Controls.Add(Me.startFrequencyNumeric)
        Me.configurationListGroupBox.Controls.Add(Me.startPowerNumeric)
        Me.configurationListGroupBox.Controls.Add(Me.stopPowerNumeric)
        Me.configurationListGroupBox.Controls.Add(Me.stopFrequencyNumeric)
        Me.configurationListGroupBox.Controls.Add(Me.numberStepsNumeric)
        Me.configurationListGroupBox.Controls.Add(Me.dwellTimeNumeric)
        Me.configurationListGroupBox.Location = New System.Drawing.Point(348, 86)
        Me.configurationListGroupBox.Name = "configurationListGroupBox"
        Me.configurationListGroupBox.Size = New System.Drawing.Size(175, 335)
        Me.configurationListGroupBox.TabIndex = 11
        Me.configurationListGroupBox.TabStop = False
        Me.configurationListGroupBox.Text = "Configuration List Parameters"
        '
        'startFrequencyLabel
        '
        Me.startFrequencyLabel.AutoSize = True
        Me.startFrequencyLabel.Location = New System.Drawing.Point(23, 26)
        Me.startFrequencyLabel.Name = "startFrequencyLabel"
        Me.startFrequencyLabel.Size = New System.Drawing.Size(104, 13)
        Me.startFrequencyLabel.TabIndex = 1
        Me.startFrequencyLabel.Text = "Start Frequency [Hz]"
        '
        'startPowerLabel
        '
        Me.startPowerLabel.AutoSize = True
        Me.startPowerLabel.Location = New System.Drawing.Point(19, 136)
        Me.startPowerLabel.Name = "startPowerLabel"
        Me.startPowerLabel.Size = New System.Drawing.Size(92, 13)
        Me.startPowerLabel.TabIndex = 2
        Me.startPowerLabel.Text = "Start Power [dBm]"
        '
        'stopPowerLabel
        '
        Me.stopPowerLabel.AutoSize = True
        Me.stopPowerLabel.Location = New System.Drawing.Point(19, 183)
        Me.stopPowerLabel.Name = "stopPowerLabel"
        Me.stopPowerLabel.Size = New System.Drawing.Size(92, 13)
        Me.stopPowerLabel.TabIndex = 3
        Me.stopPowerLabel.Text = "Stop Power [dBm]"
        '
        'stopFrequencyLabel
        '
        Me.stopFrequencyLabel.AutoSize = True
        Me.stopFrequencyLabel.Location = New System.Drawing.Point(19, 78)
        Me.stopFrequencyLabel.Name = "stopFrequencyLabel"
        Me.stopFrequencyLabel.Size = New System.Drawing.Size(101, 13)
        Me.stopFrequencyLabel.TabIndex = 4
        Me.stopFrequencyLabel.Text = "End Frequency [Hz]"
        '
        'numberStepsLabel
        '
        Me.numberStepsLabel.AutoSize = True
        Me.numberStepsLabel.Location = New System.Drawing.Point(19, 239)
        Me.numberStepsLabel.Name = "numberStepsLabel"
        Me.numberStepsLabel.Size = New System.Drawing.Size(86, 13)
        Me.numberStepsLabel.TabIndex = 5
        Me.numberStepsLabel.Text = "Number of Steps"
        '
        'dwellTimeLabel
        '
        Me.dwellTimeLabel.AutoSize = True
        Me.dwellTimeLabel.Location = New System.Drawing.Point(19, 289)
        Me.dwellTimeLabel.Name = "dwellTimeLabel"
        Me.dwellTimeLabel.Size = New System.Drawing.Size(137, 13)
        Me.dwellTimeLabel.TabIndex = 6
        Me.dwellTimeLabel.Text = "Dwell Time in Each Step [s]"
        '
        'startFrequencyNumeric
        '
        Me.startFrequencyNumeric.DecimalPlaces = 6
        Me.startFrequencyNumeric.Location = New System.Drawing.Point(19, 42)
        Me.startFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.startFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.startFrequencyNumeric.Name = "startFrequencyNumeric"
        Me.startFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.startFrequencyNumeric.TabIndex = 5
        Me.startFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'startPowerNumeric
        '
        Me.startPowerNumeric.DecimalPlaces = 2
        Me.startPowerNumeric.Location = New System.Drawing.Point(19, 152)
        Me.startPowerNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.startPowerNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.startPowerNumeric.Name = "startPowerNumeric"
        Me.startPowerNumeric.Size = New System.Drawing.Size(120, 20)
        Me.startPowerNumeric.TabIndex = 7
        Me.startPowerNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'stopPowerNumeric
        '
        Me.stopPowerNumeric.DecimalPlaces = 2
        Me.stopPowerNumeric.Location = New System.Drawing.Point(19, 199)
        Me.stopPowerNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.stopPowerNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.stopPowerNumeric.Name = "stopPowerNumeric"
        Me.stopPowerNumeric.Size = New System.Drawing.Size(120, 20)
        Me.stopPowerNumeric.TabIndex = 8
        '
        'stopFrequencyNumeric
        '
        Me.stopFrequencyNumeric.DecimalPlaces = 6
        Me.stopFrequencyNumeric.Location = New System.Drawing.Point(19, 94)
        Me.stopFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.stopFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.stopFrequencyNumeric.Name = "stopFrequencyNumeric"
        Me.stopFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.stopFrequencyNumeric.TabIndex = 6
        Me.stopFrequencyNumeric.Value = New Decimal(New Integer() {1020000000, 0, 0, 0})
        '
        'numberStepsNumeric
        '
        Me.numberStepsNumeric.Location = New System.Drawing.Point(19, 255)
        Me.numberStepsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numberStepsNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numberStepsNumeric.Name = "numberStepsNumeric"
        Me.numberStepsNumeric.Size = New System.Drawing.Size(120, 20)
        Me.numberStepsNumeric.TabIndex = 9
        Me.numberStepsNumeric.Value = New Decimal(New Integer() {21, 0, 0, 0})
        '
        'dwellTimeNumeric
        '
        Me.dwellTimeNumeric.DecimalPlaces = 3
        Me.dwellTimeNumeric.Location = New System.Drawing.Point(19, 305)
        Me.dwellTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.dwellTimeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.dwellTimeNumeric.Name = "dwellTimeNumeric"
        Me.dwellTimeNumeric.Size = New System.Drawing.Size(120, 20)
        Me.dwellTimeNumeric.TabIndex = 10
        Me.dwellTimeNumeric.Value = New Decimal(New Integer() {2, 0, 0, 131072})
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
        Me.iqRateLabel.Location = New System.Drawing.Point(23, 254)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
        Me.iqRateLabel.TabIndex = 7
        Me.iqRateLabel.Text = "IQ Rate (S/s)"
        '
        'actualIQRateLabel
        '
        Me.actualIQRateLabel.AutoSize = True
        Me.actualIQRateLabel.Location = New System.Drawing.Point(196, 254)
        Me.actualIQRateLabel.Name = "actualIQRateLabel"
        Me.actualIQRateLabel.Size = New System.Drawing.Size(103, 13)
        Me.actualIQRateLabel.TabIndex = 8
        Me.actualIQRateLabel.Text = "Actual IQ Rate (S/s)"
        '
        'clockSourceLabel
        '
        Me.clockSourceLabel.AutoSize = True
        Me.clockSourceLabel.Location = New System.Drawing.Point(22, 308)
        Me.clockSourceLabel.Name = "clockSourceLabel"
        Me.clockSourceLabel.Size = New System.Drawing.Size(147, 13)
        Me.clockSourceLabel.TabIndex = 9
        Me.clockSourceLabel.Text = "Frequency Reference Source"
        '
        'loopBandwidthLabel
        '
        Me.loopBandwidthLabel.AutoSize = True
        Me.loopBandwidthLabel.Location = New System.Drawing.Point(22, 364)
        Me.loopBandwidthLabel.Name = "loopBandwidthLabel"
        Me.loopBandwidthLabel.Size = New System.Drawing.Size(84, 13)
        Me.loopBandwidthLabel.TabIndex = 10
        Me.loopBandwidthLabel.Text = "Loop Bandwidth"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(23, 442)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 11
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'scriptLabel
        '
        Me.scriptLabel.AutoSize = True
        Me.scriptLabel.Location = New System.Drawing.Point(22, 86)
        Me.scriptLabel.Name = "scriptLabel"
        Me.scriptLabel.Size = New System.Drawing.Size(34, 13)
        Me.scriptLabel.TabIndex = 12
        Me.scriptLabel.Text = "Script"
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 2
        Me.iqRateNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.iqRateNumeric.Location = New System.Drawing.Point(22, 272)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqRateNumeric.TabIndex = 2
        Me.iqRateNumeric.Value = New Decimal(New Integer() {5000000, 0, 0, 0})
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(22, 42)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'actualIQRateTextBox
        '
        Me.actualIQRateTextBox.Location = New System.Drawing.Point(197, 272)
        Me.actualIQRateTextBox.Name = "actualIQRateTextBox"
        Me.actualIQRateTextBox.ReadOnly = True
        Me.actualIQRateTextBox.Size = New System.Drawing.Size(126, 20)
        Me.actualIQRateTextBox.TabIndex = 10
        Me.actualIQRateTextBox.TabStop = False
        Me.actualIQRateTextBox.Text = "0.00"
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(22, 463)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(501, 47)
        Me.errorTextBox.TabIndex = 15
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No Error"
        '
        'scriptTextBox
        '
        Me.scriptTextBox.Location = New System.Drawing.Point(23, 107)
        Me.scriptTextBox.Multiline = True
        Me.scriptTextBox.Name = "scriptTextBox"
        Me.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.scriptTextBox.Size = New System.Drawing.Size(300, 118)
        Me.scriptTextBox.TabIndex = 1
        Me.scriptTextBox.Text = "script continuousWaveformToneScript" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    repeat forever" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       generate continuo" & _
            "usWaveformTone" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    end repeat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "end script"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(367, 42)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 11
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(448, 42)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 12
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'clockSourceComboBox
        '
        Me.clockSourceComboBox.Location = New System.Drawing.Point(22, 329)
        Me.clockSourceComboBox.Name = "clockSourceComboBox"
        Me.clockSourceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.clockSourceComboBox.TabIndex = 3
        '
        'loopBandwidthComboBox
        '
        Me.loopBandwidthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.loopBandwidthComboBox.Location = New System.Drawing.Point(22, 385)
        Me.loopBandwidthComboBox.Name = "loopBandwidthComboBox"
        Me.loopBandwidthComboBox.Size = New System.Drawing.Size(120, 21)
        Me.loopBandwidthComboBox.TabIndex = 4
        '
        'rfsgStatusTimer
        '
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(549, 556)
        Me.Controls.Add(Me.configurationListGroupBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.iqRateLabel)
        Me.Controls.Add(Me.actualIQRateLabel)
        Me.Controls.Add(Me.clockSourceLabel)
        Me.Controls.Add(Me.loopBandwidthLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.scriptLabel)
        Me.Controls.Add(Me.iqRateNumeric)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.actualIQRateTextBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.scriptTextBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.clockSourceComboBox)
        Me.Controls.Add(Me.loopBandwidthComboBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Frequency and Power Sweep (Timer Triggered)"
        Me.configurationListGroupBox.ResumeLayout(False)
        Me.configurationListGroupBox.PerformLayout()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.startPowerNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.stopPowerNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.stopFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberStepsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dwellTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
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
    Private dwellTimeLabel As System.Windows.Forms.Label
    Private iqRateLabel As System.Windows.Forms.Label
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
    Private dwellTimeNumeric As System.Windows.Forms.NumericUpDown
    Private iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private actualIQRateTextBox As System.Windows.Forms.TextBox
    Private errorTextBox As System.Windows.Forms.TextBox
    Private scriptTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button    Private clockSourceComboBox As System.Windows.Forms.ComboBox
    Private loopBandwidthComboBox As System.Windows.Forms.ComboBox
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer

End Class
