Partial Class MainForm
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(disposing As Boolean)
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
        Me.powerSweepGroupBox = New System.Windows.Forms.GroupBox()
        Me.holdAttenuatorsLabel = New System.Windows.Forms.Label()
        Me.holdAttenuatorsCheckBox = New System.Windows.Forms.CheckBox()
        Me.startPowerLabel = New System.Windows.Forms.Label()
        Me.stopPowerLabel = New System.Windows.Forms.Label()
        Me.numberStepsLabel = New System.Windows.Forms.Label()
        Me.dwellTimeLabel = New System.Windows.Forms.Label()
        Me.startPowerNumeric = New System.Windows.Forms.NumericUpDown()
        Me.stopPowerNumeric = New System.Windows.Forms.NumericUpDown()
        Me.numberStepsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.dwellTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.frequencyLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.actualCurrentPowerLabel = New System.Windows.Forms.Label()
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.actualCurrentPowerTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.powerSweepGroupBox.SuspendLayout()
        CType(Me.startPowerNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.stopPowerNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberStepsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dwellTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'powerSweepGroupBox
        '
        Me.powerSweepGroupBox.Controls.Add(Me.holdAttenuatorsLabel)
        Me.powerSweepGroupBox.Controls.Add(Me.holdAttenuatorsCheckBox)
        Me.powerSweepGroupBox.Controls.Add(Me.startPowerLabel)
        Me.powerSweepGroupBox.Controls.Add(Me.stopPowerLabel)
        Me.powerSweepGroupBox.Controls.Add(Me.numberStepsLabel)
        Me.powerSweepGroupBox.Controls.Add(Me.dwellTimeLabel)
        Me.powerSweepGroupBox.Controls.Add(Me.startPowerNumeric)
        Me.powerSweepGroupBox.Controls.Add(Me.stopPowerNumeric)
        Me.powerSweepGroupBox.Controls.Add(Me.numberStepsNumeric)
        Me.powerSweepGroupBox.Controls.Add(Me.dwellTimeNumeric)
        Me.powerSweepGroupBox.Location = New System.Drawing.Point(163, 12)
        Me.powerSweepGroupBox.Name = "powerSweepGroupBox"
        Me.powerSweepGroupBox.Size = New System.Drawing.Size(169, 264)
        Me.powerSweepGroupBox.TabIndex = 3
        Me.powerSweepGroupBox.TabStop = False
        Me.powerSweepGroupBox.Text = "Power Sweep Parameters"
        '
        'holdAttenuatorsLabel
        '
        Me.holdAttenuatorsLabel.AutoSize = True
        Me.holdAttenuatorsLabel.Location = New System.Drawing.Point(24, 218)
        Me.holdAttenuatorsLabel.Name = "holdAttenuatorsLabel"
        Me.holdAttenuatorsLabel.Size = New System.Drawing.Size(86, 13)
        Me.holdAttenuatorsLabel.TabIndex = 15
        Me.holdAttenuatorsLabel.Text = "Hold Attenuators"
        '
        'holdAttenuatorsCheckBox
        '
        Me.holdAttenuatorsCheckBox.AutoSize = True
        Me.holdAttenuatorsCheckBox.Location = New System.Drawing.Point(27, 239)
        Me.holdAttenuatorsCheckBox.Name = "holdAttenuatorsCheckBox"
        Me.holdAttenuatorsCheckBox.Size = New System.Drawing.Size(40, 17)
        Me.holdAttenuatorsCheckBox.TabIndex = 6
        Me.holdAttenuatorsCheckBox.Text = "On"
        Me.holdAttenuatorsCheckBox.UseVisualStyleBackColor = True
        '
        'startPowerLabel
        '
        Me.startPowerLabel.AutoSize = True
        Me.startPowerLabel.Location = New System.Drawing.Point(24, 25)
        Me.startPowerLabel.Name = "startPowerLabel"
        Me.startPowerLabel.Size = New System.Drawing.Size(92, 13)
        Me.startPowerLabel.TabIndex = 2
        Me.startPowerLabel.Text = "Start Power [dBm]"
        '
        'stopPowerLabel
        '
        Me.stopPowerLabel.AutoSize = True
        Me.stopPowerLabel.Location = New System.Drawing.Point(24, 72)
        Me.stopPowerLabel.Name = "stopPowerLabel"
        Me.stopPowerLabel.Size = New System.Drawing.Size(92, 13)
        Me.stopPowerLabel.TabIndex = 3
        Me.stopPowerLabel.Text = "Stop Power [dBm]"
        '
        'numberStepsLabel
        '
        Me.numberStepsLabel.AutoSize = True
        Me.numberStepsLabel.Location = New System.Drawing.Point(24, 119)
        Me.numberStepsLabel.Name = "numberStepsLabel"
        Me.numberStepsLabel.Size = New System.Drawing.Size(86, 13)
        Me.numberStepsLabel.TabIndex = 4
        Me.numberStepsLabel.Text = "Number of Steps"
        '
        'dwellTimeLabel
        '
        Me.dwellTimeLabel.AutoSize = True
        Me.dwellTimeLabel.Location = New System.Drawing.Point(24, 166)
        Me.dwellTimeLabel.Name = "dwellTimeLabel"
        Me.dwellTimeLabel.Size = New System.Drawing.Size(111, 13)
        Me.dwellTimeLabel.TabIndex = 5
        Me.dwellTimeLabel.Text = "Dwell in Each Step [s]"
        '
        'startPowerNumeric
        '
        Me.startPowerNumeric.DecimalPlaces = 2
        Me.startPowerNumeric.Location = New System.Drawing.Point(24, 46)
        Me.startPowerNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.startPowerNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.startPowerNumeric.Name = "startPowerNumeric"
        Me.startPowerNumeric.Size = New System.Drawing.Size(120, 20)
        Me.startPowerNumeric.TabIndex = 2
        Me.startPowerNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'stopPowerNumeric
        '
        Me.stopPowerNumeric.DecimalPlaces = 2
        Me.stopPowerNumeric.Location = New System.Drawing.Point(24, 93)
        Me.stopPowerNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.stopPowerNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.stopPowerNumeric.Name = "stopPowerNumeric"
        Me.stopPowerNumeric.Size = New System.Drawing.Size(120, 20)
        Me.stopPowerNumeric.TabIndex = 3
        '
        'numberStepsNumeric
        '
        Me.numberStepsNumeric.Location = New System.Drawing.Point(24, 140)
        Me.numberStepsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numberStepsNumeric.Name = "numberStepsNumeric"
        Me.numberStepsNumeric.Size = New System.Drawing.Size(120, 20)
        Me.numberStepsNumeric.TabIndex = 4
        Me.numberStepsNumeric.Value = New Decimal(New Integer() {21, 0, 0, 0})
        '
        'dwellTimeNumeric
        '
        Me.dwellTimeNumeric.DecimalPlaces = 2
        Me.dwellTimeNumeric.Location = New System.Drawing.Point(24, 187)
        Me.dwellTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.dwellTimeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.dwellTimeNumeric.Name = "dwellTimeNumeric"
        Me.dwellTimeNumeric.Size = New System.Drawing.Size(120, 20)
        Me.dwellTimeNumeric.TabIndex = 5
        Me.dwellTimeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(20, 36)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.AutoSize = True
        Me.frequencyLabel.Location = New System.Drawing.Point(20, 84)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.frequencyLabel.TabIndex = 1
        Me.frequencyLabel.Text = "Center Frequency [Hz]"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(20, 282)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 6
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'actualCurrentPowerLabel
        '
        Me.actualCurrentPowerLabel.AutoSize = True
        Me.actualCurrentPowerLabel.Location = New System.Drawing.Point(366, 83)
        Me.actualCurrentPowerLabel.Name = "actualCurrentPowerLabel"
        Me.actualCurrentPowerLabel.Size = New System.Drawing.Size(104, 13)
        Me.actualCurrentPowerLabel.TabIndex = 7
        Me.actualCurrentPowerLabel.Text = "Current Power [dBm]"
        '
        'frequencyNumeric
        '
        Me.frequencyNumeric.DecimalPlaces = 6
        Me.frequencyNumeric.Location = New System.Drawing.Point(20, 105)
        Me.frequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.frequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.frequencyNumeric.Name = "frequencyNumeric"
        Me.frequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.frequencyNumeric.TabIndex = 1
        Me.frequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(20, 57)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(20, 303)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(486, 34)
        Me.errorTextBox.TabIndex = 12
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'actualCurrentPowerTextBox
        '
        Me.actualCurrentPowerTextBox.Location = New System.Drawing.Point(366, 104)
        Me.actualCurrentPowerTextBox.Name = "actualCurrentPowerTextBox"
        Me.actualCurrentPowerTextBox.ReadOnly = True
        Me.actualCurrentPowerTextBox.Size = New System.Drawing.Size(75, 20)
        Me.actualCurrentPowerTextBox.TabIndex = 13
        Me.actualCurrentPowerTextBox.TabStop = False
        Me.actualCurrentPowerTextBox.Text = "0.00"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(350, 18)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 4
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
                Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(431, 18)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 5
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '        '        '
        'rfsgStatusTimer
        '
        Me.rfsgStatusTimer.Interval = 250
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True        Me.ClientSize = New System.Drawing.Size(520, 378)
        Me.Controls.Add(Me.powerSweepGroupBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.frequencyLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.actualCurrentPowerLabel)
        Me.Controls.Add(Me.frequencyNumeric)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.actualCurrentPowerTextBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Power Sweep"
        Me.powerSweepGroupBox.ResumeLayout(False)
        Me.powerSweepGroupBox.PerformLayout()
        CType(Me.startPowerNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.stopPowerNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberStepsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dwellTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private powerSweepGroupBox As System.Windows.Forms.GroupBox
    Private resourceNameLabel As System.Windows.Forms.Label
    Private frequencyLabel As System.Windows.Forms.Label
    Private startPowerLabel As System.Windows.Forms.Label
    Private stopPowerLabel As System.Windows.Forms.Label
    Private numberStepsLabel As System.Windows.Forms.Label
    Private dwellTimeLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private actualCurrentPowerLabel As System.Windows.Forms.Label
    Private frequencyNumeric As System.Windows.Forms.NumericUpDown
    Private startPowerNumeric As System.Windows.Forms.NumericUpDown
    Private stopPowerNumeric As System.Windows.Forms.NumericUpDown
    Private numberStepsNumeric As System.Windows.Forms.NumericUpDown
		Private dwellTimeNumeric As System.Windows.Forms.NumericUpDown
		Private resourceNameComboBox As System.Windows.Forms.ComboBox
		Private errorTextBox As System.Windows.Forms.TextBox
		Private actualCurrentPowerTextBox As System.Windows.Forms.TextBox
		Private WithEvents startButton As System.Windows.Forms.Button
		Private WithEvents stopButton As System.Windows.Forms.Button		Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
		Private holdAttenuatorsCheckBox As System.Windows.Forms.CheckBox
		Private holdAttenuatorsLabel As System.Windows.Forms.Label

	End Class
