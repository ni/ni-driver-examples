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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.referenceClockComboBox = New System.Windows.Forms.ComboBox()
        Me.referenceClockLabel = New System.Windows.Forms.Label()
        Me.updateButton = New System.Windows.Forms.Button()
        Me.iqPortFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqPortFrequencyLabel = New System.Windows.Forms.Label()
        Me.iqOutPortLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqOutPortLevelLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.startButton = New System.Windows.Forms.Button()
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.iqPortFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqOutPortLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.referenceClockComboBox)
        Me.configurationGroupBox.Controls.Add(Me.referenceClockLabel)
        Me.configurationGroupBox.Controls.Add(Me.updateButton)
        Me.configurationGroupBox.Controls.Add(Me.iqPortFrequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.iqPortFrequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.iqOutPortLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.iqOutPortLevelLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 66)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(360, 196)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'referenceClockComboBox
        '
        Me.referenceClockComboBox.Location = New System.Drawing.Point(13, 43)
        Me.referenceClockComboBox.Name = "referenceClockComboBox"
        Me.referenceClockComboBox.Size = New System.Drawing.Size(116, 21)
        Me.referenceClockComboBox.TabIndex = 1
        '
        'referenceClockLabel
        '
        Me.referenceClockLabel.AutoSize = True
        Me.referenceClockLabel.Location = New System.Drawing.Point(13, 23)
        Me.referenceClockLabel.Name = "referenceClockLabel"
        Me.referenceClockLabel.Size = New System.Drawing.Size(71, 13)
        Me.referenceClockLabel.TabIndex = 28
        Me.referenceClockLabel.Text = "Clock Source"
        '
        'updateButton
        '
        Me.updateButton.Enabled = False
        Me.updateButton.Location = New System.Drawing.Point(246, 41)
        Me.updateButton.Name = "updateButton"
        Me.updateButton.Size = New System.Drawing.Size(75, 23)
        Me.updateButton.TabIndex = 4
        Me.updateButton.Text = "&Update"
        Me.updateButton.UseVisualStyleBackColor = True
        '
        'iqPortFrequencyNumeric
        '
        Me.iqPortFrequencyNumeric.DecimalPlaces = 6
        Me.iqPortFrequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.iqPortFrequencyNumeric.Location = New System.Drawing.Point(13, 98)
        Me.iqPortFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqPortFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqPortFrequencyNumeric.Name = "iqPortFrequencyNumeric"
        Me.iqPortFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqPortFrequencyNumeric.TabIndex = 2
        '
        'iqPortFrequencyLabel
        '
        Me.iqPortFrequencyLabel.AutoSize = True
        Me.iqPortFrequencyLabel.Location = New System.Drawing.Point(13, 78)
        Me.iqPortFrequencyLabel.Name = "iqPortFrequencyLabel"
        Me.iqPortFrequencyLabel.Size = New System.Drawing.Size(115, 13)
        Me.iqPortFrequencyLabel.TabIndex = 1
        Me.iqPortFrequencyLabel.Text = "IQ Port Frequency (Hz)"
        '
        'iqOutPortLevelNumeric
        '
        Me.iqOutPortLevelNumeric.DecimalPlaces = 3
        Me.iqOutPortLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.iqOutPortLevelNumeric.Location = New System.Drawing.Point(13, 153)
        Me.iqOutPortLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iqOutPortLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iqOutPortLevelNumeric.Name = "iqOutPortLevelNumeric"
        Me.iqOutPortLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iqOutPortLevelNumeric.TabIndex = 3
        Me.iqOutPortLevelNumeric.Value = New Decimal(New Integer() {500, 0, 0, 196608})
        '
        'iqOutPortLevelLabel
        '
        Me.iqOutPortLevelLabel.AutoSize = True
        Me.iqOutPortLevelLabel.Location = New System.Drawing.Point(13, 133)
        Me.iqOutPortLevelLabel.Name = "iqOutPortLevelLabel"
        Me.iqOutPortLevelLabel.Size = New System.Drawing.Size(117, 13)
        Me.iqOutPortLevelLabel.TabIndex = 2
        Me.iqOutPortLevelLabel.Text = "IQ Out Port Level (Vpp)"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(12, 9)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 11
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(12, 270)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 16
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 30)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(12, 291)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(360, 60)
        Me.errorTextBox.TabIndex = 17
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(297, 28)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 3
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'rfsgStatusTimer
        '
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(216, 28)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 2
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(409, 380)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.Text = "Single Tone Generation IQ Device"
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.iqPortFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqOutPortLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

	#End Region

	Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents updateButton As System.Windows.Forms.Button
	Private iqPortFrequencyNumeric As System.Windows.Forms.NumericUpDown
	Private iqPortFrequencyLabel As System.Windows.Forms.Label
	Private iqOutPortLevelNumeric As System.Windows.Forms.NumericUpDown
	Private iqOutPortLevelLabel As System.Windows.Forms.Label
	Private resourceNameLabel As System.Windows.Forms.Label
	Private errorLabel As System.Windows.Forms.Label
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private errorTextBox As System.Windows.Forms.TextBox
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
	Private referenceClockComboBox As System.Windows.Forms.ComboBox
    Private referenceClockLabel As System.Windows.Forms.Label
    Private WithEvents startButton As System.Windows.Forms.Button
End Class

