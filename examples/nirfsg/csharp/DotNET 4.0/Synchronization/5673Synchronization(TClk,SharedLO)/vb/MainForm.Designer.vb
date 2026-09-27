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
			Me.masterRfsgResourceNameLabel = New System.Windows.Forms.Label()
			Me.frequencyLabel = New System.Windows.Forms.Label()
			Me.powerLevelLabel = New System.Windows.Forms.Label()
			Me.errorLabel = New System.Windows.Forms.Label()
			Me.slaveFrequencyReferenceOutputLabel = New System.Windows.Forms.Label()
			Me.masterFrequencyReferenceOutputLabel = New System.Windows.Forms.Label()
			Me.slaveFrequencyReferenceSourceLabel = New System.Windows.Forms.Label()
			Me.masterFrequencyReferenceSourceLabel = New System.Windows.Forms.Label()
			Me.textmessageLabel = New System.Windows.Forms.Label()
			Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
			Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
			Me.errorTextBox = New System.Windows.Forms.TextBox()
			Me.startButton = New System.Windows.Forms.Button()
			Me.stopButton = New System.Windows.Forms.Button()			Me.slaveFrequencyReferenceOutputComboBox = New System.Windows.Forms.ComboBox()
			Me.masterFrequencyReferenceOutputComboBox = New System.Windows.Forms.ComboBox()
			Me.slaveFrequencyReferenceSourceComboBox = New System.Windows.Forms.ComboBox()
			Me.masterFrequencyReferenceSourceComboBox = New System.Windows.Forms.ComboBox()
			Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
			Me.masterRfsgResourceNameComboBox = New System.Windows.Forms.ComboBox()
			Me.slaveRfsgResourceNamesLabel = New System.Windows.Forms.Label()
			Me.frequencyReferenceGroupBox = New System.Windows.Forms.GroupBox()
			Me.slaveRfsgResourceNamesTextBox = New System.Windows.Forms.TextBox()
			CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.frequencyReferenceGroupBox.SuspendLayout()
			Me.SuspendLayout()
			' 
			' masterRfsgResourceNameLabel
			' 
			Me.masterRfsgResourceNameLabel.AutoSize = True
			Me.masterRfsgResourceNameLabel.Location = New System.Drawing.Point(27, 19)
			Me.masterRfsgResourceNameLabel.Name = "masterRfsgResourceNameLabel"
			Me.masterRfsgResourceNameLabel.Size = New System.Drawing.Size(144, 13)
			Me.masterRfsgResourceNameLabel.TabIndex = 0
			Me.masterRfsgResourceNameLabel.Text = "Master Rfsg Resource Name"
			' 
			' frequencyLabel
			' 
			Me.frequencyLabel.AutoSize = True
			Me.frequencyLabel.Location = New System.Drawing.Point(27, 145)
			Me.frequencyLabel.Name = "frequencyLabel"
			Me.frequencyLabel.Size = New System.Drawing.Size(79, 13)
			Me.frequencyLabel.TabIndex = 1
			Me.frequencyLabel.Text = "Frequency [Hz]"
			' 
			' powerLevelLabel
			' 
			Me.powerLevelLabel.AutoSize = True
			Me.powerLevelLabel.Location = New System.Drawing.Point(27, 199)
			Me.powerLevelLabel.Name = "powerLevelLabel"
			Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
			Me.powerLevelLabel.TabIndex = 2
			Me.powerLevelLabel.Text = "Power Level [dBm]"
			' 
			' errorLabel
			' 
			Me.errorLabel.AutoSize = True
			Me.errorLabel.Location = New System.Drawing.Point(26, 416)
			Me.errorLabel.Name = "errorLabel"
			Me.errorLabel.Size = New System.Drawing.Size(120, 13)
			Me.errorLabel.TabIndex = 3
			Me.errorLabel.Text = "Warning/Error Message"
			' 
			' slaveFrequencyReferenceOutputLabel
			' 
			Me.slaveFrequencyReferenceOutputLabel.AutoSize = True
			Me.slaveFrequencyReferenceOutputLabel.Location = New System.Drawing.Point(302, 78)
			Me.slaveFrequencyReferenceOutputLabel.Name = "slaveFrequencyReferenceOutputLabel"
			Me.slaveFrequencyReferenceOutputLabel.Size = New System.Drawing.Size(218, 13)
			Me.slaveFrequencyReferenceOutputLabel.TabIndex = 4
			Me.slaveFrequencyReferenceOutputLabel.Text = "Slave Frequency Reference Output Terminal"
			' 
			' masterFrequencyReferenceOutputLabel
			' 
			Me.masterFrequencyReferenceOutputLabel.AutoSize = True
			Me.masterFrequencyReferenceOutputLabel.Location = New System.Drawing.Point(20, 78)
			Me.masterFrequencyReferenceOutputLabel.Name = "masterFrequencyReferenceOutputLabel"
			Me.masterFrequencyReferenceOutputLabel.Size = New System.Drawing.Size(223, 13)
			Me.masterFrequencyReferenceOutputLabel.TabIndex = 5
			Me.masterFrequencyReferenceOutputLabel.Text = "Master Frequency Reference Output Terminal"
			' 
			' slaveFrequencyReferenceSourceLabel
			' 
			Me.slaveFrequencyReferenceSourceLabel.AutoSize = True
			Me.slaveFrequencyReferenceSourceLabel.Location = New System.Drawing.Point(304, 28)
			Me.slaveFrequencyReferenceSourceLabel.Name = "slaveFrequencyReferenceSourceLabel"
			Me.slaveFrequencyReferenceSourceLabel.Size = New System.Drawing.Size(177, 13)
			Me.slaveFrequencyReferenceSourceLabel.TabIndex = 6
			Me.slaveFrequencyReferenceSourceLabel.Text = "Slave Frequency Reference Source"
			' 
			' masterFrequencyReferenceSourceLabel
			' 
			Me.masterFrequencyReferenceSourceLabel.AutoSize = True
			Me.masterFrequencyReferenceSourceLabel.Location = New System.Drawing.Point(21, 28)
			Me.masterFrequencyReferenceSourceLabel.Name = "masterFrequencyReferenceSourceLabel"
			Me.masterFrequencyReferenceSourceLabel.Size = New System.Drawing.Size(182, 13)
			Me.masterFrequencyReferenceSourceLabel.TabIndex = 7
			Me.masterFrequencyReferenceSourceLabel.Text = "Master Frequency Reference Source"
			' 
			' textmessageLabel
			' 
			Me.textmessageLabel.Location = New System.Drawing.Point(302, 66)
			Me.textmessageLabel.Name = "textmessageLabel"
			Me.textmessageLabel.Size = New System.Drawing.Size(269, 58)
			Me.textmessageLabel.TabIndex = 5
			Me.textmessageLabel.Text = "The master NI 5673 should have a LO and AWG associated with it in Measurement and" + " Automation Explorer (MAX). Slave devices should have an AWG, but an External LO" + "."
			' 
			' frequencyNumeric
			' 
			Me.frequencyNumeric.DecimalPlaces = 6
			Me.frequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
			Me.frequencyNumeric.Location = New System.Drawing.Point(27, 166)
			Me.frequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
			Me.frequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
			Me.frequencyNumeric.Name = "frequencyNumeric"
			Me.frequencyNumeric.Size = New System.Drawing.Size(121, 20)
			Me.frequencyNumeric.TabIndex = 2
			Me.frequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
			' 
			' powerLevelNumeric
			' 
			Me.powerLevelNumeric.DecimalPlaces = 2
			Me.powerLevelNumeric.Location = New System.Drawing.Point(27, 220)
			Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
			Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
			Me.powerLevelNumeric.Name = "powerLevelNumeric"
			Me.powerLevelNumeric.Size = New System.Drawing.Size(121, 20)
			Me.powerLevelNumeric.TabIndex = 3
			Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
			' 
			' errorTextBox
			' 
			Me.errorTextBox.Location = New System.Drawing.Point(27, 435)
			Me.errorTextBox.Multiline = True
			Me.errorTextBox.Name = "errorTextBox"
			Me.errorTextBox.[ReadOnly] = True
			Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
			Me.errorTextBox.Size = New System.Drawing.Size(544, 56)
			Me.errorTextBox.TabIndex = 8
			Me.errorTextBox.TabStop = False
			Me.errorTextBox.Text = "No error."
			' 
			' startButton
			' 
			Me.startButton.Location = New System.Drawing.Point(385, 19)
			Me.startButton.Name = "startButton"
			Me.startButton.Size = New System.Drawing.Size(75, 23)
			Me.startButton.TabIndex = 6
			Me.startButton.Text = "St&art"
			Me.startButton.UseVisualStyleBackColor = True
			
			' 
			' stopButton
			' 
			        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.stopButton.Enabled = False
			Me.stopButton.Location = New System.Drawing.Point(466, 19)
			Me.stopButton.Name = "stopButton"
			Me.stopButton.Size = New System.Drawing.Size(75, 23)
			Me.stopButton.TabIndex = 7
			Me.stopButton.Text = "St&op"
			Me.stopButton.UseVisualStyleBackColor = True
			
			' 			' 			
			' 
			' slaveFrequencyReferenceOutputComboBox
			' 
			Me.slaveFrequencyReferenceOutputComboBox.Location = New System.Drawing.Point(302, 99)
			Me.slaveFrequencyReferenceOutputComboBox.Name = "slaveFrequencyReferenceOutputComboBox"
			Me.slaveFrequencyReferenceOutputComboBox.Size = New System.Drawing.Size(124, 21)
			Me.slaveFrequencyReferenceOutputComboBox.TabIndex = 3
			' 
			' masterFrequencyReferenceOutputComboBox
			' 
			Me.masterFrequencyReferenceOutputComboBox.Location = New System.Drawing.Point(20, 99)
			Me.masterFrequencyReferenceOutputComboBox.Name = "masterFrequencyReferenceOutputComboBox"
			Me.masterFrequencyReferenceOutputComboBox.Size = New System.Drawing.Size(123, 21)
			Me.masterFrequencyReferenceOutputComboBox.TabIndex = 1
			' 
			' slaveFrequencyReferenceSourceComboBox
			' 
			Me.slaveFrequencyReferenceSourceComboBox.Location = New System.Drawing.Point(304, 49)
			Me.slaveFrequencyReferenceSourceComboBox.Name = "slaveFrequencyReferenceSourceComboBox"
			Me.slaveFrequencyReferenceSourceComboBox.Size = New System.Drawing.Size(122, 21)
			Me.slaveFrequencyReferenceSourceComboBox.TabIndex = 2
			' 
			' masterFrequencyReferenceSourceComboBox
			' 
			Me.masterFrequencyReferenceSourceComboBox.Location = New System.Drawing.Point(21, 49)
			Me.masterFrequencyReferenceSourceComboBox.Name = "masterFrequencyReferenceSourceComboBox"
			Me.masterFrequencyReferenceSourceComboBox.Size = New System.Drawing.Size(122, 21)
			Me.masterFrequencyReferenceSourceComboBox.TabIndex = 0
			' 
			' rfsgStatusTimer
			' 
			
			' 
			' masterRfsgResourceNameComboBox
			' 
			Me.masterRfsgResourceNameComboBox.FormattingEnabled = True
			Me.masterRfsgResourceNameComboBox.Location = New System.Drawing.Point(27, 43)
			Me.masterRfsgResourceNameComboBox.Name = "masterRfsgResourceNameComboBox"
			Me.masterRfsgResourceNameComboBox.Size = New System.Drawing.Size(121, 21)
			Me.masterRfsgResourceNameComboBox.TabIndex = 0
			' 
			' slaveRfsgResourceNamesLabel
			' 
			Me.slaveRfsgResourceNamesLabel.AutoSize = True
			Me.slaveRfsgResourceNamesLabel.Location = New System.Drawing.Point(26, 82)
			Me.slaveRfsgResourceNamesLabel.Name = "slaveRfsgResourceNamesLabel"
			Me.slaveRfsgResourceNamesLabel.Size = New System.Drawing.Size(237, 13)
			Me.slaveRfsgResourceNamesLabel.TabIndex = 14
			Me.slaveRfsgResourceNamesLabel.Text = "Slave Rfsg Resource Names (comma separated)"
			' 
			' frequencyReferenceGroupBox
			' 
			Me.frequencyReferenceGroupBox.Controls.Add(Me.slaveFrequencyReferenceOutputComboBox)
			Me.frequencyReferenceGroupBox.Controls.Add(Me.masterFrequencyReferenceSourceComboBox)
			Me.frequencyReferenceGroupBox.Controls.Add(Me.slaveFrequencyReferenceSourceComboBox)
			Me.frequencyReferenceGroupBox.Controls.Add(Me.masterFrequencyReferenceOutputComboBox)
			Me.frequencyReferenceGroupBox.Controls.Add(Me.masterFrequencyReferenceSourceLabel)
			Me.frequencyReferenceGroupBox.Controls.Add(Me.slaveFrequencyReferenceSourceLabel)
			Me.frequencyReferenceGroupBox.Controls.Add(Me.masterFrequencyReferenceOutputLabel)
			Me.frequencyReferenceGroupBox.Controls.Add(Me.slaveFrequencyReferenceOutputLabel)
			Me.frequencyReferenceGroupBox.Location = New System.Drawing.Point(27, 257)
			Me.frequencyReferenceGroupBox.Name = "frequencyReferenceGroupBox"
			Me.frequencyReferenceGroupBox.Size = New System.Drawing.Size(544, 141)
			Me.frequencyReferenceGroupBox.TabIndex = 4
			Me.frequencyReferenceGroupBox.TabStop = False
			Me.frequencyReferenceGroupBox.Text = "Frequency Reference Parameters"
			' 
			' slaveRfsgResourceNamesTextBox
			' 
			Me.slaveRfsgResourceNamesTextBox.Location = New System.Drawing.Point(27, 104)
			Me.slaveRfsgResourceNamesTextBox.Name = "slaveRfsgResourceNamesTextBox"
			Me.slaveRfsgResourceNamesTextBox.Size = New System.Drawing.Size(121, 20)
			Me.slaveRfsgResourceNamesTextBox.TabIndex = 1
			Me.slaveRfsgResourceNamesTextBox.Text = "Slave"
			' 
			' MainForm
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.AutoSize = True			Me.ClientSize = New System.Drawing.Size(583, 532)
			Me.Controls.Add(Me.slaveRfsgResourceNamesTextBox)
			Me.Controls.Add(Me.frequencyReferenceGroupBox)
			Me.Controls.Add(Me.slaveRfsgResourceNamesLabel)
			Me.Controls.Add(Me.masterRfsgResourceNameComboBox)
			Me.Controls.Add(Me.masterRfsgResourceNameLabel)
			Me.Controls.Add(Me.frequencyLabel)
			Me.Controls.Add(Me.powerLevelLabel)
			Me.Controls.Add(Me.errorLabel)
			Me.Controls.Add(Me.textmessageLabel)
			Me.Controls.Add(Me.frequencyNumeric)
			Me.Controls.Add(Me.powerLevelNumeric)
			Me.Controls.Add(Me.errorTextBox)
			Me.Controls.Add(Me.startButton)
			Me.Controls.Add(Me.stopButton)			Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.MaximizeBox = False
			Me.Name = "MainForm"
			Me.Text = "5673 Synchronization (TClk, Shared LO)"
			CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
			Me.frequencyReferenceGroupBox.ResumeLayout(False)
			Me.frequencyReferenceGroupBox.PerformLayout()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub
		#End Region

		Private masterRfsgResourceNameLabel As System.Windows.Forms.Label
		Private frequencyLabel As System.Windows.Forms.Label
		Private powerLevelLabel As System.Windows.Forms.Label
		Private errorLabel As System.Windows.Forms.Label
		Private slaveFrequencyReferenceOutputLabel As System.Windows.Forms.Label
		Private masterFrequencyReferenceOutputLabel As System.Windows.Forms.Label
		Private slaveFrequencyReferenceSourceLabel As System.Windows.Forms.Label
		Private masterFrequencyReferenceSourceLabel As System.Windows.Forms.Label
		Private textmessageLabel As System.Windows.Forms.Label
		Private frequencyNumeric As System.Windows.Forms.NumericUpDown
		Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
		Private errorTextBox As System.Windows.Forms.TextBox
		Private WithEvents startButton As System.Windows.Forms.Button
		Private WithEvents stopButton As System.Windows.Forms.Button		Private slaveFrequencyReferenceOutputComboBox As System.Windows.Forms.ComboBox
		Private masterFrequencyReferenceOutputComboBox As System.Windows.Forms.ComboBox
		Private slaveFrequencyReferenceSourceComboBox As System.Windows.Forms.ComboBox
		Private masterFrequencyReferenceSourceComboBox As System.Windows.Forms.ComboBox
		Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
		Private masterRfsgResourceNameComboBox As System.Windows.Forms.ComboBox
		Private slaveRfsgResourceNamesLabel As System.Windows.Forms.Label
		Private frequencyReferenceGroupBox As System.Windows.Forms.GroupBox
		Private slaveRfsgResourceNamesTextBox As System.Windows.Forms.TextBox

	End Class
