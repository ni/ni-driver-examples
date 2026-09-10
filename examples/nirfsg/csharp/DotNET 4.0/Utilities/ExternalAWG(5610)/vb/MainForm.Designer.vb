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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.frequencyLabel = New System.Windows.Forms.Label()
        Me.gainLabel = New System.Windows.Forms.Label()
        Me.bandwidthLabel = New System.Windows.Forms.Label()
        Me.arbCarrierLabel = New System.Windows.Forms.Label()
        Me.actualFrequencyLabel = New System.Windows.Forms.Label()
        Me.actualGainLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.gainNumeric = New System.Windows.Forms.NumericUpDown()
        Me.bandwidthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.arbCarrierNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.actualFrequencyTextBox = New System.Windows.Forms.TextBox()
        Me.actualGainTextBox = New System.Windows.Forms.TextBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gainNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bandwidthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.arbCarrierNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.configurationGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(28, 16)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.AutoSize = True
        Me.frequencyLabel.Location = New System.Drawing.Point(21, 22)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.frequencyLabel.TabIndex = 1
        Me.frequencyLabel.Text = "Center Frequency [Hz]"
        '
        'gainLabel
        '
        Me.gainLabel.AutoSize = True
        Me.gainLabel.Location = New System.Drawing.Point(21, 70)
        Me.gainLabel.Name = "gainLabel"
        Me.gainLabel.Size = New System.Drawing.Size(51, 13)
        Me.gainLabel.TabIndex = 2
        Me.gainLabel.Text = "Gain [dB]"
        '
        'bandwidthLabel
        '
        Me.bandwidthLabel.AutoSize = True
        Me.bandwidthLabel.Location = New System.Drawing.Point(21, 119)
        Me.bandwidthLabel.Name = "bandwidthLabel"
        Me.bandwidthLabel.Size = New System.Drawing.Size(79, 13)
        Me.bandwidthLabel.TabIndex = 3
        Me.bandwidthLabel.Text = "Bandwidth [Hz]"
        '
        'arbCarrierLabel
        '
        Me.arbCarrierLabel.AutoSize = True
        Me.arbCarrierLabel.Location = New System.Drawing.Point(21, 172)
        Me.arbCarrierLabel.Name = "arbCarrierLabel"
        Me.arbCarrierLabel.Size = New System.Drawing.Size(131, 13)
        Me.arbCarrierLabel.TabIndex = 4
        Me.arbCarrierLabel.Text = "Arb Carrier Frequency [Hz]"
        '
        'actualFrequencyLabel
        '
        Me.actualFrequencyLabel.AutoSize = True
        Me.actualFrequencyLabel.Location = New System.Drawing.Point(19, 25)
        Me.actualFrequencyLabel.Name = "actualFrequencyLabel"
        Me.actualFrequencyLabel.Size = New System.Drawing.Size(146, 13)
        Me.actualFrequencyLabel.TabIndex = 5
        Me.actualFrequencyLabel.Text = "Actual Center Frequency [Hz]"
        '
        'actualGainLabel
        '
        Me.actualGainLabel.AutoSize = True
        Me.actualGainLabel.Location = New System.Drawing.Point(19, 73)
        Me.actualGainLabel.Name = "actualGainLabel"
        Me.actualGainLabel.Size = New System.Drawing.Size(84, 13)
        Me.actualGainLabel.TabIndex = 6
        Me.actualGainLabel.Text = "Actual Gain [dB]"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(28, 334)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 7
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'frequencyNumeric
        '
        Me.frequencyNumeric.DecimalPlaces = 6
        Me.frequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.frequencyNumeric.Location = New System.Drawing.Point(21, 43)
        Me.frequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.frequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.frequencyNumeric.Name = "frequencyNumeric"
        Me.frequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.frequencyNumeric.TabIndex = 1
        Me.frequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'gainNumeric
        '
        Me.gainNumeric.DecimalPlaces = 2
        Me.gainNumeric.Location = New System.Drawing.Point(21, 91)
        Me.gainNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.gainNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.gainNumeric.Name = "gainNumeric"
        Me.gainNumeric.Size = New System.Drawing.Size(120, 20)
        Me.gainNumeric.TabIndex = 2
        Me.gainNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'bandwidthNumeric
        '
        Me.bandwidthNumeric.DecimalPlaces = 6
        Me.bandwidthNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.bandwidthNumeric.Location = New System.Drawing.Point(21, 140)
        Me.bandwidthNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.bandwidthNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.bandwidthNumeric.Name = "bandwidthNumeric"
        Me.bandwidthNumeric.Size = New System.Drawing.Size(120, 20)
        Me.bandwidthNumeric.TabIndex = 3
        Me.bandwidthNumeric.Value = New Decimal(New Integer() {5000000, 0, 0, 0})
        '
        'arbCarrierNumeric
        '
        Me.arbCarrierNumeric.DecimalPlaces = 6
        Me.arbCarrierNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.arbCarrierNumeric.Location = New System.Drawing.Point(21, 193)
        Me.arbCarrierNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.arbCarrierNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.arbCarrierNumeric.Name = "arbCarrierNumeric"
        Me.arbCarrierNumeric.Size = New System.Drawing.Size(120, 20)
        Me.arbCarrierNumeric.TabIndex = 4
        Me.arbCarrierNumeric.Value = New Decimal(New Integer() {25000000, 0, 0, 0})
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(28, 37)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'actualFrequencyTextBox
        '
        Me.actualFrequencyTextBox.Location = New System.Drawing.Point(19, 43)
        Me.actualFrequencyTextBox.Name = "actualFrequencyTextBox"
        Me.actualFrequencyTextBox.ReadOnly = True
        Me.actualFrequencyTextBox.Size = New System.Drawing.Size(109, 20)
        Me.actualFrequencyTextBox.TabIndex = 8
        Me.actualFrequencyTextBox.TabStop = False
        Me.actualFrequencyTextBox.Text = "0.000000"
        '
        'actualGainTextBox
        '
        Me.actualGainTextBox.Location = New System.Drawing.Point(19, 91)
        Me.actualGainTextBox.Name = "actualGainTextBox"
        Me.actualGainTextBox.ReadOnly = True
        Me.actualGainTextBox.Size = New System.Drawing.Size(109, 20)
        Me.actualGainTextBox.TabIndex = 9
        Me.actualGainTextBox.TabStop = False
        Me.actualGainTextBox.Text = "0.00"
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(31, 352)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(365, 34)
        Me.errorTextBox.TabIndex = 10
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(321, 37)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 3
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.frequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.arbCarrierNumeric)
        Me.configurationGroupBox.Controls.Add(Me.bandwidthNumeric)
        Me.configurationGroupBox.Controls.Add(Me.frequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.gainNumeric)
        Me.configurationGroupBox.Controls.Add(Me.gainLabel)
        Me.configurationGroupBox.Controls.Add(Me.arbCarrierLabel)
        Me.configurationGroupBox.Controls.Add(Me.bandwidthLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(28, 81)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(165, 229)
        Me.configurationGroupBox.TabIndex = 2
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.actualGainTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualGainLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(214, 81)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(182, 121)
        Me.measurementGroupBox.TabIndex = 0
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(425, 424)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "External AWG (5610)"
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gainNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bandwidthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.arbCarrierNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private resourceNameLabel As System.Windows.Forms.Label
    Private frequencyLabel As System.Windows.Forms.Label
    Private gainLabel As System.Windows.Forms.Label
    Private bandwidthLabel As System.Windows.Forms.Label
    Private arbCarrierLabel As System.Windows.Forms.Label
    Private actualFrequencyLabel As System.Windows.Forms.Label
    Private actualGainLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private frequencyNumeric As System.Windows.Forms.NumericUpDown
    Private gainNumeric As System.Windows.Forms.NumericUpDown
    Private bandwidthNumeric As System.Windows.Forms.NumericUpDown
    Private arbCarrierNumeric As System.Windows.Forms.NumericUpDown
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private actualFrequencyTextBox As System.Windows.Forms.TextBox
    Private actualGainTextBox As System.Windows.Forms.TextBox
    Private errorTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button    Private configurationGroupBox As System.Windows.Forms.GroupBox
		Private measurementGroupBox As System.Windows.Forms.GroupBox

	End Class
