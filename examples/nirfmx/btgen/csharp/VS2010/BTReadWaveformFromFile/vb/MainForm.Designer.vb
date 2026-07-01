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
		Me.rfsgResourceLabel = New System.Windows.Forms.Label()
		Me.chnNumberLabel = New System.Windows.Forms.Label()
		Me.carrierFreqLabel = New System.Windows.Forms.Label()
		Me.powerLevelLabel = New System.Windows.Forms.Label()
		Me.externalAttnLabel = New System.Windows.Forms.Label()
		Me.actualHeadroomLabel = New System.Windows.Forms.Label()
		Me.waveformNameLabel = New System.Windows.Forms.Label()
		Me.clkOutTerminalLabel = New System.Windows.Forms.Label()
		Me.refSourceLabel = New System.Windows.Forms.Label()
		Me.scriptLabel = New System.Windows.Forms.Label()
		Me.errorLabel = New System.Windows.Forms.Label()
		Me.hardwareLabel = New System.Windows.Forms.Label()
		Me.idleFilePathLabel = New System.Windows.Forms.Label()
		Me.filePathLabel = New System.Windows.Forms.Label()
		Me.chnNumberNumeric = New System.Windows.Forms.NumericUpDown()
		Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
		Me.externalAttnNumeric = New System.Windows.Forms.NumericUpDown()
		Me.rfsgResourceTextBox = New System.Windows.Forms.TextBox()
		Me.carrierFreqTextBox = New System.Windows.Forms.TextBox()
		Me.actualHeadroomTextBox = New System.Windows.Forms.TextBox()
		Me.waveNameTextBox = New System.Windows.Forms.TextBox()
		Me.scriptTextBox = New System.Windows.Forms.TextBox()
		Me.errorTextBox = New System.Windows.Forms.TextBox()
		Me.filePathTextBox = New System.Windows.Forms.TextBox()
		Me.stopButton = New System.Windows.Forms.Button()
		Me.generateButton = New System.Windows.Forms.Button()
		Me.clkOutTerminalComboBox = New System.Windows.Forms.ComboBox()
		Me.refSourceComboBox = New System.Windows.Forms.ComboBox()
		Me.dataWaveformFileBrowseButton = New System.Windows.Forms.Button()
		Me.timer = New System.Windows.Forms.Timer(Me.components)
		Me.iOffsetLabel = New System.Windows.Forms.Label()
		Me.qOffsetLabel = New System.Windows.Forms.Label()
		Me.iOffsetNumeric = New System.Windows.Forms.NumericUpDown()
		Me.qOffsetNumeric = New System.Windows.Forms.NumericUpDown()
		Me.iCommonModeOffsetLabel = New System.Windows.Forms.Label()
		Me.qCommonModeOffsetLabel = New System.Windows.Forms.Label()
		Me.iCommonModeOffsetNumeric = New System.Windows.Forms.NumericUpDown()
		Me.qCommonModeOffsetNumeric = New System.Windows.Forms.NumericUpDown()
		Me.label2 = New System.Windows.Forms.Label()
		Me.terminalConfigurationComboBox = New System.Windows.Forms.ComboBox()
		Me.label1 = New System.Windows.Forms.Label()
		Me.outputPortComboBox = New System.Windows.Forms.ComboBox()
		Me.standardLabel = New System.Windows.Forms.Label()
		Me.standardComboBox = New System.Windows.Forms.ComboBox()
		DirectCast(Me.chnNumberNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.externalAttnNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.iOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.qOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.iCommonModeOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.qCommonModeOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		' 
		' rfsgResourceLabel
		' 
		Me.rfsgResourceLabel.AutoSize = True
		Me.rfsgResourceLabel.Location = New System.Drawing.Point(5, 80)
		Me.rfsgResourceLabel.Name = "rfsgResourceLabel"
		Me.rfsgResourceLabel.Size = New System.Drawing.Size(84, 13)
		Me.rfsgResourceLabel.TabIndex = 0
		Me.rfsgResourceLabel.Text = "Resource Name"
		' 
		' chnNumberLabel
		' 
		Me.chnNumberLabel.AutoSize = True
		Me.chnNumberLabel.Location = New System.Drawing.Point(4, 101)
		Me.chnNumberLabel.Name = "chnNumberLabel"
		Me.chnNumberLabel.Size = New System.Drawing.Size(86, 13)
		Me.chnNumberLabel.TabIndex = 1
		Me.chnNumberLabel.Text = "Channel Number"
		' 
		' carrierFreqLabel
		' 
		Me.carrierFreqLabel.AutoSize = True
		Me.carrierFreqLabel.Location = New System.Drawing.Point(6, 122)
		Me.carrierFreqLabel.Name = "carrierFreqLabel"
		Me.carrierFreqLabel.Size = New System.Drawing.Size(112, 13)
		Me.carrierFreqLabel.TabIndex = 2
		Me.carrierFreqLabel.Text = "Carrier Frequency (Hz)"
		' 
		' powerLevelLabel
		' 
		Me.powerLevelLabel.AutoSize = True
		Me.powerLevelLabel.Location = New System.Drawing.Point(4, 142)
		Me.powerLevelLabel.Name = "powerLevelLabel"
		Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
		Me.powerLevelLabel.TabIndex = 3
		Me.powerLevelLabel.Text = "Power Level (dBm)"
		' 
		' externalAttnLabel
		' 
		Me.externalAttnLabel.AutoSize = True
		Me.externalAttnLabel.Location = New System.Drawing.Point(4, 164)
		Me.externalAttnLabel.Name = "externalAttnLabel"
		Me.externalAttnLabel.Size = New System.Drawing.Size(124, 13)
		Me.externalAttnLabel.TabIndex = 4
		Me.externalAttnLabel.Text = "External Attenuation (dB)"
		' 
		' actualHeadroomLabel
		' 
		Me.actualHeadroomLabel.AutoSize = True
		Me.actualHeadroomLabel.Location = New System.Drawing.Point(4, 186)
		Me.actualHeadroomLabel.Name = "actualHeadroomLabel"
		Me.actualHeadroomLabel.Size = New System.Drawing.Size(111, 13)
		Me.actualHeadroomLabel.TabIndex = 5
		Me.actualHeadroomLabel.Text = "Actual Headroom (dB)"
		' 
		' waveformNameLabel
		' 
		Me.waveformNameLabel.AutoSize = True
		Me.waveformNameLabel.Location = New System.Drawing.Point(262, 95)
		Me.waveformNameLabel.Name = "waveformNameLabel"
		Me.waveformNameLabel.Size = New System.Drawing.Size(87, 13)
		Me.waveformNameLabel.TabIndex = 7
		Me.waveformNameLabel.Text = "Waveform Name"
		' 
		' clkOutTerminalLabel
		' 
		Me.clkOutTerminalLabel.AutoSize = True
		Me.clkOutTerminalLabel.Location = New System.Drawing.Point(7, 268)
		Me.clkOutTerminalLabel.Name = "clkOutTerminalLabel"
		Me.clkOutTerminalLabel.Size = New System.Drawing.Size(67, 13)
		Me.clkOutTerminalLabel.TabIndex = 8
		Me.clkOutTerminalLabel.Text = "Export Clock"
		' 
		' refSourceLabel
		' 
		Me.refSourceLabel.AutoSize = True
		Me.refSourceLabel.Location = New System.Drawing.Point(7, 249)
		Me.refSourceLabel.Name = "refSourceLabel"
		Me.refSourceLabel.Size = New System.Drawing.Size(94, 13)
		Me.refSourceLabel.TabIndex = 9
		Me.refSourceLabel.Text = "Reference Source"
		' 
		' scriptLabel
		' 
		Me.scriptLabel.AutoSize = True
		Me.scriptLabel.Location = New System.Drawing.Point(269, 158)
		Me.scriptLabel.Name = "scriptLabel"
		Me.scriptLabel.Size = New System.Drawing.Size(34, 13)
		Me.scriptLabel.TabIndex = 10
		Me.scriptLabel.Text = "Script"
		' 
		' errorLabel
		' 
		Me.errorLabel.AutoSize = True
		Me.errorLabel.Location = New System.Drawing.Point(11, 292)
		Me.errorLabel.Name = "errorLabel"
		Me.errorLabel.Size = New System.Drawing.Size(120, 13)
		Me.errorLabel.TabIndex = 11
		Me.errorLabel.Text = "Error/Warning Message"
		' 
		' hardwareLabel
		' 
		Me.hardwareLabel.AutoSize = True
		Me.hardwareLabel.Location = New System.Drawing.Point(21, 54)
		Me.hardwareLabel.Name = "hardwareLabel"
		Me.hardwareLabel.Size = New System.Drawing.Size(53, 13)
		Me.hardwareLabel.TabIndex = 12
		Me.hardwareLabel.Text = "Hardware"
		' 
		' idleFilePathLabel
		' 
		Me.idleFilePathLabel.AutoSize = True
		Me.idleFilePathLabel.Location = New System.Drawing.Point(10, 22)
		Me.idleFilePathLabel.Name = "idleFilePathLabel"
		Me.idleFilePathLabel.Size = New System.Drawing.Size(0, 13)
		Me.idleFilePathLabel.TabIndex = 13
		' 
		' filePathLabel
		' 
		Me.filePathLabel.AutoSize = True
		Me.filePathLabel.Location = New System.Drawing.Point(10, -9)
		Me.filePathLabel.Name = "filePathLabel"
		Me.filePathLabel.Size = New System.Drawing.Size(0, 13)
		Me.filePathLabel.TabIndex = 14
		' 
		' chnNumberNumeric
		' 
		Me.chnNumberNumeric.Location = New System.Drawing.Point(136, 97)
		Me.chnNumberNumeric.Name = "chnNumberNumeric"
		Me.chnNumberNumeric.Size = New System.Drawing.Size(103, 20)
		Me.chnNumberNumeric.TabIndex = 1
		' 
		' powerLevelNumeric
		' 
		Me.powerLevelNumeric.Location = New System.Drawing.Point(136, 139)
		Me.powerLevelNumeric.Name = "powerLevelNumeric"
		Me.powerLevelNumeric.Size = New System.Drawing.Size(103, 20)
		Me.powerLevelNumeric.TabIndex = 3
		' 
		' externalAttnNumeric
		' 
		Me.externalAttnNumeric.Location = New System.Drawing.Point(136, 161)
		Me.externalAttnNumeric.Name = "externalAttnNumeric"
		Me.externalAttnNumeric.Size = New System.Drawing.Size(103, 20)
		Me.externalAttnNumeric.TabIndex = 4
		' 
		' rfsgResourceTextBox
		' 
		Me.rfsgResourceTextBox.Location = New System.Drawing.Point(136, 76)
		Me.rfsgResourceTextBox.Name = "rfsgResourceTextBox"
		Me.rfsgResourceTextBox.Size = New System.Drawing.Size(103, 20)
		Me.rfsgResourceTextBox.TabIndex = 0
		Me.rfsgResourceTextBox.Text = "RFSG"
		' 
		' carrierFreqTextBox
		' 
		Me.carrierFreqTextBox.Enabled = False
		Me.carrierFreqTextBox.Location = New System.Drawing.Point(136, 117)
		Me.carrierFreqTextBox.Name = "carrierFreqTextBox"
		Me.carrierFreqTextBox.Size = New System.Drawing.Size(103, 20)
		Me.carrierFreqTextBox.TabIndex = 2
		Me.carrierFreqTextBox.Text = "2.405E+9"
		' 
		' actualHeadroomTextBox
		' 
		Me.actualHeadroomTextBox.Enabled = False
		Me.actualHeadroomTextBox.Location = New System.Drawing.Point(136, 183)
		Me.actualHeadroomTextBox.Name = "actualHeadroomTextBox"
		Me.actualHeadroomTextBox.Size = New System.Drawing.Size(103, 20)
		Me.actualHeadroomTextBox.TabIndex = 5
		Me.actualHeadroomTextBox.Text = "0.00"
		' 
		' waveNameTextBox
		' 
		Me.waveNameTextBox.Location = New System.Drawing.Point(352, 92)
		Me.waveNameTextBox.Name = "waveNameTextBox"
		Me.waveNameTextBox.Size = New System.Drawing.Size(87, 20)
		Me.waveNameTextBox.TabIndex = 7
		Me.waveNameTextBox.Text = "Data"
		' 
		' scriptTextBox
		' 
		Me.scriptTextBox.Location = New System.Drawing.Point(272, 188)
		Me.scriptTextBox.Multiline = True
		Me.scriptTextBox.Name = "scriptTextBox"
		Me.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.scriptTextBox.Size = New System.Drawing.Size(162, 82)
		Me.scriptTextBox.TabIndex = 15
		Me.scriptTextBox.TabStop = False
		Me.scriptTextBox.Text = "script GenerateDataPkt" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    repeat forever" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       generate Data" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    end repeat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " end script"
		' 
		' errorTextBox
		' 
		Me.errorTextBox.Location = New System.Drawing.Point(11, 313)
		Me.errorTextBox.Multiline = True
		Me.errorTextBox.Name = "errorTextBox"
		Me.errorTextBox.[ReadOnly] = True
		Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.errorTextBox.Size = New System.Drawing.Size(603, 73)
		Me.errorTextBox.TabIndex = 11
		Me.errorTextBox.TabStop = False
		Me.errorTextBox.Text = "No Error"
		' 
		' filePathTextBox
		' 
		Me.filePathTextBox.Location = New System.Drawing.Point(10, 12)
		Me.filePathTextBox.Name = "filePathTextBox"
		Me.filePathTextBox.Size = New System.Drawing.Size(303, 20)
		Me.filePathTextBox.TabIndex = 20
		Me.filePathTextBox.Text = "C:\Users\Public\Documents\saveConfig.tdms"
		' 
		' stopButton
		' 
		Me.stopButton.Enabled = False
		Me.stopButton.Location = New System.Drawing.Point(493, 405)
		Me.stopButton.Name = "stopButton"
		Me.stopButton.Size = New System.Drawing.Size(75, 23)
		Me.stopButton.TabIndex = 12
		Me.stopButton.Text = "S&top"
		Me.stopButton.UseVisualStyleBackColor = True
		AddHandler Me.stopButton.Click, New System.EventHandler(AddressOf Me.stopButton_Click)
		' 
		' generateButton
		' 
		Me.generateButton.Location = New System.Drawing.Point(16, 405)
		Me.generateButton.Name = "generateButton"
		Me.generateButton.Size = New System.Drawing.Size(75, 23)
		Me.generateButton.TabIndex = 16
		Me.generateButton.Text = "&Start"
		Me.generateButton.UseVisualStyleBackColor = True
		AddHandler Me.generateButton.Click, New System.EventHandler(AddressOf Me.generateButton_Click)
		' 
		' clkOutTerminalComboBox
		' 
		Me.clkOutTerminalComboBox.Location = New System.Drawing.Point(152, 268)
		Me.clkOutTerminalComboBox.Name = "clkOutTerminalComboBox"
		Me.clkOutTerminalComboBox.Size = New System.Drawing.Size(90, 21)
		Me.clkOutTerminalComboBox.TabIndex = 9
		' 
		' refSourceComboBox
		' 
		Me.refSourceComboBox.Location = New System.Drawing.Point(152, 249)
		Me.refSourceComboBox.Name = "refSourceComboBox"
		Me.refSourceComboBox.Size = New System.Drawing.Size(90, 21)
		Me.refSourceComboBox.TabIndex = 10
		' 
		' dataWaveformFileBrowseButton
		' 
		Me.dataWaveformFileBrowseButton.Location = New System.Drawing.Point(322, 12)
		Me.dataWaveformFileBrowseButton.Name = "dataWaveformFileBrowseButton"
		Me.dataWaveformFileBrowseButton.Size = New System.Drawing.Size(112, 23)
		Me.dataWaveformFileBrowseButton.TabIndex = 21
		Me.dataWaveformFileBrowseButton.Text = "&Data Waveform File"
		Me.dataWaveformFileBrowseButton.UseVisualStyleBackColor = True
		AddHandler Me.dataWaveformFileBrowseButton.Click, New System.EventHandler(AddressOf Me.dataWaveformFileBrowseButton_Click)
		' 
		' timer
		' 
		AddHandler Me.timer.Tick, New System.EventHandler(AddressOf Me.ProcessTimerEvent)
		' 
		' iOffsetLabel
		' 
		Me.iOffsetLabel.AutoSize = True
		Me.iOffsetLabel.Location = New System.Drawing.Point(458, 108)
		Me.iOffsetLabel.Name = "iOffsetLabel"
		Me.iOffsetLabel.Size = New System.Drawing.Size(54, 13)
		Me.iOffsetLabel.TabIndex = 76
		Me.iOffsetLabel.Text = "I Offset(V)"
		' 
		' qOffsetLabel
		' 
		Me.qOffsetLabel.AutoSize = True
		Me.qOffsetLabel.Location = New System.Drawing.Point(458, 130)
		Me.qOffsetLabel.Name = "qOffsetLabel"
		Me.qOffsetLabel.Size = New System.Drawing.Size(59, 13)
		Me.qOffsetLabel.TabIndex = 78
		Me.qOffsetLabel.Text = "Q Offset(V)"
		' 
		' iOffsetNumeric
		' 
		Me.iOffsetNumeric.Location = New System.Drawing.Point(600, 106)
		Me.iOffsetNumeric.Name = "iOffsetNumeric"
		Me.iOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.iOffsetNumeric.TabIndex = 75
		' 
		' qOffsetNumeric
		' 
		Me.qOffsetNumeric.Location = New System.Drawing.Point(598, 132)
		Me.qOffsetNumeric.Name = "qOffsetNumeric"
		Me.qOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.qOffsetNumeric.TabIndex = 77
		' 
		' iCommonModeOffsetLabel
		' 
		Me.iCommonModeOffsetLabel.AutoSize = True
		Me.iCommonModeOffsetLabel.Location = New System.Drawing.Point(458, 158)
		Me.iCommonModeOffsetLabel.Name = "iCommonModeOffsetLabel"
		Me.iCommonModeOffsetLabel.Size = New System.Drawing.Size(128, 13)
		Me.iCommonModeOffsetLabel.TabIndex = 72
		Me.iCommonModeOffsetLabel.Text = "I Common Mode Offset(V)"
		' 
		' qCommonModeOffsetLabel
		' 
		Me.qCommonModeOffsetLabel.AutoSize = True
		Me.qCommonModeOffsetLabel.Location = New System.Drawing.Point(458, 180)
		Me.qCommonModeOffsetLabel.Name = "qCommonModeOffsetLabel"
		Me.qCommonModeOffsetLabel.Size = New System.Drawing.Size(133, 13)
		Me.qCommonModeOffsetLabel.TabIndex = 74
		Me.qCommonModeOffsetLabel.Text = "Q Common Mode Offset(V)"
		' 
		' iCommonModeOffsetNumeric
		' 
		Me.iCommonModeOffsetNumeric.Location = New System.Drawing.Point(600, 156)
		Me.iCommonModeOffsetNumeric.Name = "iCommonModeOffsetNumeric"
		Me.iCommonModeOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.iCommonModeOffsetNumeric.TabIndex = 71
		' 
		' qCommonModeOffsetNumeric
		' 
		Me.qCommonModeOffsetNumeric.Location = New System.Drawing.Point(600, 178)
		Me.qCommonModeOffsetNumeric.Name = "qCommonModeOffsetNumeric"
		Me.qCommonModeOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.qCommonModeOffsetNumeric.TabIndex = 73
		' 
		' label2
		' 
		Me.label2.AutoSize = True
		Me.label2.Location = New System.Drawing.Point(456, 82)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(112, 13)
		Me.label2.TabIndex = 70
		Me.label2.Text = "Terminal Configuration"
		' 
		' terminalConfigurationComboBox
		' 
		Me.terminalConfigurationComboBox.Location = New System.Drawing.Point(598, 79)
		Me.terminalConfigurationComboBox.Name = "terminalConfigurationComboBox"
		Me.terminalConfigurationComboBox.Size = New System.Drawing.Size(90, 21)
		Me.terminalConfigurationComboBox.TabIndex = 69
		' 
		' label1
		' 
		Me.label1.AutoSize = True
		Me.label1.Location = New System.Drawing.Point(459, 54)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(61, 13)
		Me.label1.TabIndex = 68
		Me.label1.Text = "Output Port"
		' 
		' outputPortComboBox
		' 
		Me.outputPortComboBox.Location = New System.Drawing.Point(598, 54)
		Me.outputPortComboBox.Name = "outputPortComboBox"
		Me.outputPortComboBox.Size = New System.Drawing.Size(90, 21)
		Me.outputPortComboBox.TabIndex = 67
		' 
		' standardLabel
		' 
		Me.standardLabel.AutoSize = True
		Me.standardLabel.Location = New System.Drawing.Point(6, 210)
		Me.standardLabel.Name = "standardLabel"
		Me.standardLabel.Size = New System.Drawing.Size(50, 13)
		Me.standardLabel.TabIndex = 79
		Me.standardLabel.Text = "Standard"
		' 
		' standardComboBox
		' 
		Me.standardComboBox.Location = New System.Drawing.Point(136, 210)
		Me.standardComboBox.Name = "standardComboBox"
		Me.standardComboBox.Size = New System.Drawing.Size(103, 21)
		Me.standardComboBox.TabIndex = 80
		' 
		' MainForm
		' 
		Me.AcceptButton = Me.generateButton
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(701, 444)
		Me.Controls.Add(Me.standardComboBox)
		Me.Controls.Add(Me.standardLabel)
		Me.Controls.Add(Me.iOffsetLabel)
		Me.Controls.Add(Me.qOffsetLabel)
		Me.Controls.Add(Me.iOffsetNumeric)
		Me.Controls.Add(Me.qOffsetNumeric)
		Me.Controls.Add(Me.iCommonModeOffsetLabel)
		Me.Controls.Add(Me.qCommonModeOffsetLabel)
		Me.Controls.Add(Me.iCommonModeOffsetNumeric)
		Me.Controls.Add(Me.qCommonModeOffsetNumeric)
		Me.Controls.Add(Me.label2)
		Me.Controls.Add(Me.terminalConfigurationComboBox)
		Me.Controls.Add(Me.label1)
		Me.Controls.Add(Me.outputPortComboBox)
		Me.Controls.Add(Me.dataWaveformFileBrowseButton)
		Me.Controls.Add(Me.rfsgResourceLabel)
		Me.Controls.Add(Me.chnNumberLabel)
		Me.Controls.Add(Me.carrierFreqLabel)
		Me.Controls.Add(Me.powerLevelLabel)
		Me.Controls.Add(Me.externalAttnLabel)
		Me.Controls.Add(Me.actualHeadroomLabel)
		Me.Controls.Add(Me.waveformNameLabel)
		Me.Controls.Add(Me.clkOutTerminalLabel)
		Me.Controls.Add(Me.refSourceLabel)
		Me.Controls.Add(Me.scriptLabel)
		Me.Controls.Add(Me.errorLabel)
		Me.Controls.Add(Me.hardwareLabel)
		Me.Controls.Add(Me.idleFilePathLabel)
		Me.Controls.Add(Me.filePathLabel)
		Me.Controls.Add(Me.chnNumberNumeric)
		Me.Controls.Add(Me.powerLevelNumeric)
		Me.Controls.Add(Me.externalAttnNumeric)
		Me.Controls.Add(Me.rfsgResourceTextBox)
		Me.Controls.Add(Me.carrierFreqTextBox)
		Me.Controls.Add(Me.actualHeadroomTextBox)
		Me.Controls.Add(Me.waveNameTextBox)
		Me.Controls.Add(Me.scriptTextBox)
		Me.Controls.Add(Me.errorTextBox)
		Me.Controls.Add(Me.filePathTextBox)
		Me.Controls.Add(Me.stopButton)
		Me.Controls.Add(Me.generateButton)
		Me.Controls.Add(Me.clkOutTerminalComboBox)
		Me.Controls.Add(Me.refSourceComboBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.MaximizeBox = False
		Me.Name = "MainForm"
		Me.ShowIcon = False
		Me.Text = "Bluetooth Generate Waveform from File Example"
		AddHandler Me.FormClosing, New System.Windows.Forms.FormClosingEventHandler(AddressOf Me.MainFormClosing)
		DirectCast(Me.chnNumberNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.externalAttnNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.iOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.qOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.iCommonModeOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.qCommonModeOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	#End Region

	Private rfsgResourceLabel As System.Windows.Forms.Label
	Private chnNumberLabel As System.Windows.Forms.Label
	Private carrierFreqLabel As System.Windows.Forms.Label
	Private powerLevelLabel As System.Windows.Forms.Label
	Private externalAttnLabel As System.Windows.Forms.Label
	Private actualHeadroomLabel As System.Windows.Forms.Label
	Private waveformNameLabel As System.Windows.Forms.Label
	Private clkOutTerminalLabel As System.Windows.Forms.Label
	Private refSourceLabel As System.Windows.Forms.Label
	Private scriptLabel As System.Windows.Forms.Label
	Private errorLabel As System.Windows.Forms.Label
	Private hardwareLabel As System.Windows.Forms.Label
	Private idleFilePathLabel As System.Windows.Forms.Label
	Private filePathLabel As System.Windows.Forms.Label
	Private chnNumberNumeric As System.Windows.Forms.NumericUpDown
	Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
	Private externalAttnNumeric As System.Windows.Forms.NumericUpDown
	Private rfsgResourceTextBox As System.Windows.Forms.TextBox
	Private carrierFreqTextBox As System.Windows.Forms.TextBox
	Private actualHeadroomTextBox As System.Windows.Forms.TextBox
	Private waveNameTextBox As System.Windows.Forms.TextBox
	Private scriptTextBox As System.Windows.Forms.TextBox
	Private errorTextBox As System.Windows.Forms.TextBox
	Private filePathTextBox As System.Windows.Forms.TextBox
	Private stopButton As System.Windows.Forms.Button
	Private generateButton As System.Windows.Forms.Button
	Private clkOutTerminalComboBox As System.Windows.Forms.ComboBox
	Private refSourceComboBox As System.Windows.Forms.ComboBox
	Private dataWaveformFileBrowseButton As System.Windows.Forms.Button
	Private timer As System.Windows.Forms.Timer
	Private iOffsetLabel As System.Windows.Forms.Label
	Private qOffsetLabel As System.Windows.Forms.Label
	Private iOffsetNumeric As System.Windows.Forms.NumericUpDown
	Private qOffsetNumeric As System.Windows.Forms.NumericUpDown
	Private iCommonModeOffsetLabel As System.Windows.Forms.Label
	Private qCommonModeOffsetLabel As System.Windows.Forms.Label
	Private iCommonModeOffsetNumeric As System.Windows.Forms.NumericUpDown
	Private qCommonModeOffsetNumeric As System.Windows.Forms.NumericUpDown
	Private label2 As System.Windows.Forms.Label
	Private terminalConfigurationComboBox As System.Windows.Forms.ComboBox
	Private label1 As System.Windows.Forms.Label
	Private outputPortComboBox As System.Windows.Forms.ComboBox
	Private standardLabel As System.Windows.Forms.Label
	Private standardComboBox As System.Windows.Forms.ComboBox

End Class
