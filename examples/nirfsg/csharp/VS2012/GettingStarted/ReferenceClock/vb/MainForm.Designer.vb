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
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
			Me.resourceNameLabel = New System.Windows.Forms.Label()
			Me.frequencyLabel = New System.Windows.Forms.Label()
			Me.powerLevelLabel = New System.Windows.Forms.Label()
			Me.clockSourceLabel = New System.Windows.Forms.Label()
			Me.errorLabel = New System.Windows.Forms.Label()
			Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
			Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
			Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
			Me.errorTextBox = New System.Windows.Forms.TextBox()
			Me.startButton = New System.Windows.Forms.Button()
			Me.stopButton = New System.Windows.Forms.Button()			Me.clockSourceComboBox = New System.Windows.Forms.ComboBox()
			Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
			Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
			CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.configurationGroupBox.SuspendLayout()
			Me.SuspendLayout()
			' 
			' resourceNameLabel
			' 
			Me.resourceNameLabel.AutoSize = True
			Me.resourceNameLabel.Location = New System.Drawing.Point(18, 16)
			Me.resourceNameLabel.Name = "resourceNameLabel"
			Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
			Me.resourceNameLabel.TabIndex = 0
			Me.resourceNameLabel.Text = "Resource Name"
			' 
			' frequencyLabel
			' 
			Me.frequencyLabel.AutoSize = True
			Me.frequencyLabel.Location = New System.Drawing.Point(23, 27)
			Me.frequencyLabel.Name = "frequencyLabel"
			Me.frequencyLabel.Size = New System.Drawing.Size(113, 13)
			Me.frequencyLabel.TabIndex = 1
			Me.frequencyLabel.Text = "Center Frequency [Hz]"
			' 
			' powerLevelLabel
			' 
			Me.powerLevelLabel.AutoSize = True
			Me.powerLevelLabel.Location = New System.Drawing.Point(180, 27)
			Me.powerLevelLabel.Name = "powerLevelLabel"
			Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
			Me.powerLevelLabel.TabIndex = 2
			Me.powerLevelLabel.Text = "Power Level [dBm]"
			' 
			' clockSourceLabel
			' 
			Me.clockSourceLabel.AutoSize = True
			Me.clockSourceLabel.Location = New System.Drawing.Point(23, 88)
			Me.clockSourceLabel.Name = "clockSourceLabel"
			Me.clockSourceLabel.Size = New System.Drawing.Size(71, 13)
			Me.clockSourceLabel.TabIndex = 3
			Me.clockSourceLabel.Text = "Clock Source"
			' 
			' errorLabel
			' 
			Me.errorLabel.AutoSize = True
			Me.errorLabel.Location = New System.Drawing.Point(19, 235)
			Me.errorLabel.Name = "errorLabel"
			Me.errorLabel.Size = New System.Drawing.Size(120, 13)
			Me.errorLabel.TabIndex = 4
			Me.errorLabel.Text = "Warning/Error Message"
			' 
			' frequencyNumeric
			' 
			Me.frequencyNumeric.DecimalPlaces = 6
			Me.frequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
			Me.frequencyNumeric.Location = New System.Drawing.Point(23, 48)
			Me.frequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
			Me.frequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
			Me.frequencyNumeric.Name = "frequencyNumeric"
			Me.frequencyNumeric.Size = New System.Drawing.Size(120, 20)
			Me.frequencyNumeric.TabIndex = 1
			Me.frequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
			' 
			' powerLevelNumeric
			' 
			Me.powerLevelNumeric.DecimalPlaces = 2
			Me.powerLevelNumeric.Location = New System.Drawing.Point(183, 48)
			Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
			Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
			Me.powerLevelNumeric.Name = "powerLevelNumeric"
			Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
			Me.powerLevelNumeric.TabIndex = 3
			Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
			' 
			' resourceNameComboBox
			' 
			Me.resourceNameComboBox.Location = New System.Drawing.Point(18, 37)
			Me.resourceNameComboBox.Name = "resourceNameComboBox"
			Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
			Me.resourceNameComboBox.TabIndex = 0
			' 
			' errorTextBox
			' 
			Me.errorTextBox.Location = New System.Drawing.Point(19, 256)
			Me.errorTextBox.Multiline = True
			Me.errorTextBox.Name = "errorTextBox"
			Me.errorTextBox.[ReadOnly] = True
			Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
			Me.errorTextBox.Size = New System.Drawing.Size(335, 78)
			Me.errorTextBox.TabIndex = 9
			Me.errorTextBox.TabStop = False
			Me.errorTextBox.Text = "No error."
			' 
			' startButton
			' 
			Me.startButton.Location = New System.Drawing.Point(198, 35)
			Me.startButton.Name = "startButton"
			Me.startButton.Size = New System.Drawing.Size(75, 23)
			Me.startButton.TabIndex = 2
			Me.startButton.Text = "St&art"
			Me.startButton.UseVisualStyleBackColor = True
			
			' 
			' stopButton
			' 
			        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.stopButton.Enabled = False
			Me.stopButton.Location = New System.Drawing.Point(279, 35)
			Me.stopButton.Name = "stopButton"
			Me.stopButton.Size = New System.Drawing.Size(75, 23)
			Me.stopButton.TabIndex = 3
			Me.stopButton.Text = "St&op"
			Me.stopButton.UseVisualStyleBackColor = True
			
			' 			' 			
			' 
			' clockSourceComboBox
			' 
			Me.clockSourceComboBox.Location = New System.Drawing.Point(23, 104)
			Me.clockSourceComboBox.Name = "clockSourceComboBox"
			Me.clockSourceComboBox.Size = New System.Drawing.Size(120, 21)
			Me.clockSourceComboBox.TabIndex = 2
			' 
			' rfsgStatusTimer
			' 
			
			' 
			' configurationGroupBox
			' 
			Me.configurationGroupBox.Controls.Add(Me.clockSourceComboBox)
			Me.configurationGroupBox.Controls.Add(Me.powerLevelNumeric)
			Me.configurationGroupBox.Controls.Add(Me.frequencyLabel)
			Me.configurationGroupBox.Controls.Add(Me.frequencyNumeric)
			Me.configurationGroupBox.Controls.Add(Me.powerLevelLabel)
			Me.configurationGroupBox.Controls.Add(Me.clockSourceLabel)
			Me.configurationGroupBox.Location = New System.Drawing.Point(19, 74)
			Me.configurationGroupBox.Name = "configurationGroupBox"
			Me.configurationGroupBox.Size = New System.Drawing.Size(335, 141)
			Me.configurationGroupBox.TabIndex = 1
			Me.configurationGroupBox.TabStop = False
			Me.configurationGroupBox.Text = "Configuration"
			' 
			' MainForm
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.AutoSize = True			Me.ClientSize = New System.Drawing.Size(374, 375)
			Me.Controls.Add(Me.configurationGroupBox)
			Me.Controls.Add(Me.resourceNameLabel)
			Me.Controls.Add(Me.errorLabel)
			Me.Controls.Add(Me.resourceNameComboBox)
			Me.Controls.Add(Me.errorTextBox)
			Me.Controls.Add(Me.startButton)
			Me.Controls.Add(Me.stopButton)			Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.MaximizeBox = False
			Me.Name = "MainForm"
			Me.Text = "Frequency Reference"
			CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
			Me.configurationGroupBox.ResumeLayout(False)
			Me.configurationGroupBox.PerformLayout()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub
		#End Region

		Private resourceNameLabel As System.Windows.Forms.Label
		Private frequencyLabel As System.Windows.Forms.Label
		Private powerLevelLabel As System.Windows.Forms.Label
		Private clockSourceLabel As System.Windows.Forms.Label
		Private errorLabel As System.Windows.Forms.Label
		Private frequencyNumeric As System.Windows.Forms.NumericUpDown
		Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
		Private resourceNameComboBox As System.Windows.Forms.ComboBox
		Private errorTextBox As System.Windows.Forms.TextBox
		Private WithEvents startButton As System.Windows.Forms.Button
		Private WithEvents stopButton As System.Windows.Forms.Button		Private clockSourceComboBox As System.Windows.Forms.ComboBox
		Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
		Private configurationGroupBox As System.Windows.Forms.GroupBox

	End Class
