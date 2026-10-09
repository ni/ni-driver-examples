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
	''' the contents of this method with the code editor.
	''' </summary>
	Private Sub InitializeComponent()
		Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
		Me.resourceNameLabel = New System.Windows.Forms.Label()
		Me.streamingSizeLabel = New System.Windows.Forms.Label()
		Me.blockSizeLabel = New System.Windows.Forms.Label()
		Me.frequencyLabel = New System.Windows.Forms.Label()
		Me.powerLevelLabel = New System.Windows.Forms.Label()
		Me.preGainLabel = New System.Windows.Forms.Label()
		Me.iqRateLabel = New System.Windows.Forms.Label()
		Me.errorLabel = New System.Windows.Forms.Label()
		Me.actualIQRateLabel = New System.Windows.Forms.Label()
		Me.generatingLabel = New System.Windows.Forms.Label()
		Me.streamingSizeNumeric = New System.Windows.Forms.NumericUpDown()
		Me.blockSizeNumeric = New System.Windows.Forms.NumericUpDown()
		Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
		Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
		Me.preGainNumeric = New System.Windows.Forms.NumericUpDown()
		Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
		Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
		Me.errorTextBox = New System.Windows.Forms.TextBox()
		Me.actualIQRateTextBox = New System.Windows.Forms.TextBox()
		Me.browseButton = New System.Windows.Forms.Button()
		Me.startButton = New System.Windows.Forms.Button()
		Me.stopButton = New System.Windows.Forms.Button()
		Me.generatingLed = New System.Windows.Forms.Button()
		Me.pathLabel = New System.Windows.Forms.Label()
		Me.pathTextBox = New System.Windows.Forms.TextBox()
		DirectCast(Me.streamingSizeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.blockSizeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.preGainNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		' 
		' resourceNameLabel
		' 
		Me.resourceNameLabel.AutoSize = True
		Me.resourceNameLabel.Location = New System.Drawing.Point(12, 16)
		Me.resourceNameLabel.Name = "resourceNameLabel"
		Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
		Me.resourceNameLabel.TabIndex = 0
		Me.resourceNameLabel.Text = "Resource Name"
		' 
		' streamingSizeLabel
		' 
		Me.streamingSizeLabel.AutoSize = True
		Me.streamingSizeLabel.Location = New System.Drawing.Point(161, 185)
		Me.streamingSizeLabel.Name = "streamingSizeLabel"
		Me.streamingSizeLabel.Size = New System.Drawing.Size(145, 13)
		Me.streamingSizeLabel.TabIndex = 1
		Me.streamingSizeLabel.Text = "Streaming Waveform Size (S)"
		' 
		' blockSizeLabel
		' 
		Me.blockSizeLabel.AutoSize = True
		Me.blockSizeLabel.Location = New System.Drawing.Point(161, 134)
		Me.blockSizeLabel.Name = "blockSizeLabel"
		Me.blockSizeLabel.Size = New System.Drawing.Size(101, 13)
		Me.blockSizeLabel.TabIndex = 2
		Me.blockSizeLabel.Text = "Write Block Size (S)"
		' 
		' frequencyLabel
		' 
		Me.frequencyLabel.AutoSize = True
		Me.frequencyLabel.Location = New System.Drawing.Point(12, 79)
		Me.frequencyLabel.Name = "frequencyLabel"
		Me.frequencyLabel.Size = New System.Drawing.Size(113, 13)
		Me.frequencyLabel.TabIndex = 2
		Me.frequencyLabel.Text = "Center Frequency [Hz]"
		' 
		' powerLevelLabel
		' 
		Me.powerLevelLabel.AutoSize = True
		Me.powerLevelLabel.Location = New System.Drawing.Point(12, 134)
		Me.powerLevelLabel.Name = "powerLevelLabel"
		Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
		Me.powerLevelLabel.TabIndex = 4
		Me.powerLevelLabel.Text = "Power Level [dBm]"
		' 
		' preGainLabel
		' 
		Me.preGainLabel.AutoSize = True
		Me.preGainLabel.Location = New System.Drawing.Point(12, 185)
		Me.preGainLabel.Name = "preGainLabel"
		Me.preGainLabel.Size = New System.Drawing.Size(92, 13)
		Me.preGainLabel.TabIndex = 5
		Me.preGainLabel.Text = "Pre-filter Gain [dB]"
		' 
		' iqRateLabel
		' 
		Me.iqRateLabel.AutoSize = True
		Me.iqRateLabel.Location = New System.Drawing.Point(12, 236)
		Me.iqRateLabel.Name = "iqRateLabel"
		Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
		Me.iqRateLabel.TabIndex = 6
		Me.iqRateLabel.Text = "IQ Rate [S/s]"
		' 
		' errorLabel
		' 
		Me.errorLabel.AutoSize = True
		Me.errorLabel.Location = New System.Drawing.Point(12, 301)
		Me.errorLabel.Name = "errorLabel"
		Me.errorLabel.Size = New System.Drawing.Size(120, 13)
		Me.errorLabel.TabIndex = 7
		Me.errorLabel.Text = "Warning/Error Message"
		' 
		' actualIQRateLabel
		' 
		Me.actualIQRateLabel.AutoSize = True
		Me.actualIQRateLabel.Location = New System.Drawing.Point(332, 133)
		Me.actualIQRateLabel.Name = "actualIQRateLabel"
		Me.actualIQRateLabel.Size = New System.Drawing.Size(103, 13)
		Me.actualIQRateLabel.TabIndex = 8
		Me.actualIQRateLabel.Text = "Actual IQ Rate [S/s]"
		' 
		' generatingLabel
		' 
		Me.generatingLabel.AutoSize = True
		Me.generatingLabel.Location = New System.Drawing.Point(195, 40)
		Me.generatingLabel.Name = "generatingLabel"
		Me.generatingLabel.Size = New System.Drawing.Size(59, 13)
		Me.generatingLabel.TabIndex = 9
		Me.generatingLabel.Text = "Generating"
		' 
		' streamingSizeNumeric
		' 
		Me.streamingSizeNumeric.Location = New System.Drawing.Point(161, 206)
		Me.streamingSizeNumeric.Maximum = New Decimal(New Integer() {2147483646, 0, 0, 0})
		Me.streamingSizeNumeric.Name = "streamingSizeNumeric"
		Me.streamingSizeNumeric.Size = New System.Drawing.Size(120, 20)
		Me.streamingSizeNumeric.TabIndex = 8
		Me.streamingSizeNumeric.Value = New Decimal(New Integer() {4000000, 0, 0, 0})
		' 
		' blockSizeNumeric
		' 
		Me.blockSizeNumeric.Location = New System.Drawing.Point(161, 155)
		Me.blockSizeNumeric.Maximum = New Decimal(New Integer() {2147483646, 0, 0, 0})
		Me.blockSizeNumeric.Name = "blockSizeNumeric"
		Me.blockSizeNumeric.Size = New System.Drawing.Size(120, 20)
		Me.blockSizeNumeric.TabIndex = 7
		Me.blockSizeNumeric.Value = New Decimal(New Integer() {250000, 0, 0, 0})
		' 
		' frequencyNumeric
		' 
		Me.frequencyNumeric.DecimalPlaces = 6
		Me.frequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
		Me.frequencyNumeric.Location = New System.Drawing.Point(12, 100)
		Me.frequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.frequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.frequencyNumeric.Name = "frequencyNumeric"
		Me.frequencyNumeric.Size = New System.Drawing.Size(120, 20)
		Me.frequencyNumeric.TabIndex = 1
		Me.frequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
		' 
		' powerLevelNumeric
		' 
		Me.powerLevelNumeric.DecimalPlaces = 1
		Me.powerLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.powerLevelNumeric.Location = New System.Drawing.Point(12, 155)
		Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.powerLevelNumeric.Name = "powerLevelNumeric"
		Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
		Me.powerLevelNumeric.TabIndex = 2
		Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
		' 
		' preGainNumeric
		' 
		Me.preGainNumeric.DecimalPlaces = 1
		Me.preGainNumeric.Location = New System.Drawing.Point(12, 206)
		Me.preGainNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.preGainNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.preGainNumeric.Name = "preGainNumeric"
		Me.preGainNumeric.Size = New System.Drawing.Size(120, 20)
		Me.preGainNumeric.TabIndex = 3
		Me.preGainNumeric.Value = New Decimal(New Integer() {2, 0, 0, -2147483648})
		' 
		' iqRateNumeric
		' 
		Me.iqRateNumeric.DecimalPlaces = 6
		Me.iqRateNumeric.Increment = New Decimal(New Integer() {10000, 0, 0, 0})
		Me.iqRateNumeric.Location = New System.Drawing.Point(12, 257)
		Me.iqRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.iqRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.iqRateNumeric.Name = "iqRateNumeric"
		Me.iqRateNumeric.Size = New System.Drawing.Size(120, 20)
		Me.iqRateNumeric.TabIndex = 4
		Me.iqRateNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
		' 
		' resourceNameComboBox
		' 
		Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 37)
		Me.resourceNameComboBox.Name = "resourceNameComboBox"
		Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
		Me.resourceNameComboBox.TabIndex = 0
		' 
		' errorTextBox
		' 
		Me.errorTextBox.Location = New System.Drawing.Point(12, 317)
		Me.errorTextBox.Multiline = True
		Me.errorTextBox.Name = "errorTextBox"
		Me.errorTextBox.[ReadOnly] = True
		Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.errorTextBox.Size = New System.Drawing.Size(466, 77)
		Me.errorTextBox.TabIndex = 10
		Me.errorTextBox.TabStop = False
		Me.errorTextBox.Text = "No error."
		' 
		' actualIQRateTextBox
		' 
		Me.actualIQRateTextBox.Location = New System.Drawing.Point(332, 154)
		Me.actualIQRateTextBox.Name = "actualIQRateTextBox"
		Me.actualIQRateTextBox.[ReadOnly] = True
		Me.actualIQRateTextBox.Size = New System.Drawing.Size(113, 20)
		Me.actualIQRateTextBox.TabIndex = 14
		Me.actualIQRateTextBox.TabStop = False
		Me.actualIQRateTextBox.Text = "8333333.000000"
		' 
		' browseButton
		' 
		Me.browseButton.Location = New System.Drawing.Point(453, 97)
		Me.browseButton.Name = "browseButton"
		Me.browseButton.Size = New System.Drawing.Size(25, 23)
		Me.browseButton.TabIndex = 6
		Me.browseButton.Text = "..."
		Me.browseButton.UseVisualStyleBackColor = True
		AddHandler Me.browseButton.Click, New System.EventHandler(AddressOf Me.browseButton_Click)
		' 
		' startButton
		' 
		Me.startButton.Location = New System.Drawing.Point(321, 37)
		Me.startButton.Name = "startButton"
		Me.startButton.Size = New System.Drawing.Size(75, 23)
		Me.startButton.TabIndex = 9
		Me.startButton.Text = "St&art"
		Me.startButton.UseVisualStyleBackColor = True
		AddHandler Me.startButton.Click, New System.EventHandler(AddressOf Me.startButton_Click)
		' 
		' stopButton
		' 
		Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.stopButton.Enabled = False
		Me.stopButton.Location = New System.Drawing.Point(403, 37)
		Me.stopButton.Name = "stopButton"
		Me.stopButton.Size = New System.Drawing.Size(75, 23)
		Me.stopButton.TabIndex = 10
		Me.stopButton.Text = "St&op"
		Me.stopButton.UseVisualStyleBackColor = True
		AddHandler Me.stopButton.Click, New System.EventHandler(AddressOf Me.stopButton_Click)
		' 
		' generatingLed
		' 
		Me.generatingLed.BackColor = System.Drawing.SystemColors.Control
		Me.generatingLed.Enabled = False
		Me.generatingLed.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.generatingLed.Location = New System.Drawing.Point(171, 37)
		Me.generatingLed.Name = "generatingLed"
		Me.generatingLed.Size = New System.Drawing.Size(20, 20)
		Me.generatingLed.TabIndex = 15
		Me.generatingLed.TabStop = False
		Me.generatingLed.UseVisualStyleBackColor = False
		' 
		' pathLabel
		' 
		Me.pathLabel.AutoSize = True
		Me.pathLabel.Location = New System.Drawing.Point(160, 78)
		Me.pathLabel.Name = "pathLabel"
		Me.pathLabel.Size = New System.Drawing.Size(112, 13)
		Me.pathLabel.TabIndex = 17
		Me.pathLabel.Text = "Path to Waveform File"
		' 
		' pathTextBox
		' 
		Me.pathTextBox.Location = New System.Drawing.Point(161, 99)
		Me.pathTextBox.Name = "pathTextBox"
		Me.pathTextBox.Size = New System.Drawing.Size(284, 20)
		Me.pathTextBox.TabIndex = 5
		Me.pathTextBox.Text = "..\..\ChirpWaveform.bin"
		' 
		' MainForm
		' 
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.AutoSize = True
		Me.ClientSize = New System.Drawing.Size(488, 444)
		Me.Controls.Add(Me.pathLabel)
		Me.Controls.Add(Me.pathTextBox)
		Me.Controls.Add(Me.resourceNameLabel)
		Me.Controls.Add(Me.streamingSizeLabel)
		Me.Controls.Add(Me.blockSizeLabel)
		Me.Controls.Add(Me.frequencyLabel)
		Me.Controls.Add(Me.powerLevelLabel)
		Me.Controls.Add(Me.preGainLabel)
		Me.Controls.Add(Me.iqRateLabel)
		Me.Controls.Add(Me.errorLabel)
		Me.Controls.Add(Me.actualIQRateLabel)
		Me.Controls.Add(Me.generatingLabel)
		Me.Controls.Add(Me.streamingSizeNumeric)
		Me.Controls.Add(Me.blockSizeNumeric)
		Me.Controls.Add(Me.frequencyNumeric)
		Me.Controls.Add(Me.powerLevelNumeric)
		Me.Controls.Add(Me.preGainNumeric)
		Me.Controls.Add(Me.iqRateNumeric)
		Me.Controls.Add(Me.resourceNameComboBox)
		Me.Controls.Add(Me.errorTextBox)
		Me.Controls.Add(Me.actualIQRateTextBox)
		Me.Controls.Add(Me.browseButton)
		Me.Controls.Add(Me.startButton)
		Me.Controls.Add(Me.stopButton)
		Me.Controls.Add(Me.generatingLed)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Icon = DirectCast(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.Name = "MainForm"
		Me.Text = "Arbitrary Waveform Streaming"
		AddHandler Me.FormClosing, New System.Windows.Forms.FormClosingEventHandler(AddressOf Me.MainForm_FormClosing)
		DirectCast(Me.streamingSizeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.blockSizeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.preGainNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	#End Region

	Private resourceNameLabel As System.Windows.Forms.Label
	Private streamingSizeLabel As System.Windows.Forms.Label
	Private blockSizeLabel As System.Windows.Forms.Label
	Private frequencyLabel As System.Windows.Forms.Label
	Private powerLevelLabel As System.Windows.Forms.Label
	Private preGainLabel As System.Windows.Forms.Label
	Private iqRateLabel As System.Windows.Forms.Label
	Private errorLabel As System.Windows.Forms.Label
	Private actualIQRateLabel As System.Windows.Forms.Label
	Private generatingLabel As System.Windows.Forms.Label
	Private streamingSizeNumeric As System.Windows.Forms.NumericUpDown
	Private blockSizeNumeric As System.Windows.Forms.NumericUpDown
	Private frequencyNumeric As System.Windows.Forms.NumericUpDown
	Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
	Private preGainNumeric As System.Windows.Forms.NumericUpDown
	Private iqRateNumeric As System.Windows.Forms.NumericUpDown
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private errorTextBox As System.Windows.Forms.TextBox
	Private actualIQRateTextBox As System.Windows.Forms.TextBox
	Private browseButton As System.Windows.Forms.Button
	Private startButton As System.Windows.Forms.Button
	Private stopButton As System.Windows.Forms.Button
	Private generatingLed As System.Windows.Forms.Button
	Private pathLabel As System.Windows.Forms.Label
	Private pathTextBox As System.Windows.Forms.TextBox

End Class
