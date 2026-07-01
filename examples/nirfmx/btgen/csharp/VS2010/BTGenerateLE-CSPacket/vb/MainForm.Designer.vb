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
        Me.components = New System.ComponentModel.Container
        Me.rfsgResourceLabel = New System.Windows.Forms.Label
        Me.chnNumberLabel = New System.Windows.Forms.Label
        Me.carrierFreqLabel = New System.Windows.Forms.Label
        Me.powerLevelLabel = New System.Windows.Forms.Label
        Me.externalAttnLabel = New System.Windows.Forms.Label
        Me.autoheadroomEnabLabel = New System.Windows.Forms.Label
        Me.headroomLabel = New System.Windows.Forms.Label
        Me.actualHeadroomLabel = New System.Windows.Forms.Label
        Me.refSourceLabel = New System.Windows.Forms.Label
        Me.clkOutputTerminalLabel = New System.Windows.Forms.Label
        Me.allIqImpairEnLabel = New System.Windows.Forms.Label
        Me.quadratureSkewLabel = New System.Windows.Forms.Label
        Me.iDcOffsetLabel = New System.Windows.Forms.Label
        Me.qDcOffsetLabel = New System.Windows.Forms.Label
        Me.iqGaimbalanceLabel = New System.Windows.Forms.Label
        Me.carrierFreqOffLabel = New System.Windows.Forms.Label
        Me.awgnEnabledLabel = New System.Windows.Forms.Label
        Me.cnrLabel = New System.Windows.Forms.Label
        Me.hardwareLabel = New System.Windows.Forms.Label
        Me.frequencySettingsLabel = New System.Windows.Forms.Label
        Me.impairmentsLabel = New System.Windows.Forms.Label
        Me.chnNumberNumeric = New System.Windows.Forms.NumericUpDown
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown
        Me.externalAttnNumeric = New System.Windows.Forms.NumericUpDown
        Me.headroomNumeric = New System.Windows.Forms.NumericUpDown
        Me.quadratureSkewNumeric = New System.Windows.Forms.NumericUpDown
        Me.iDcOffsetNumeric = New System.Windows.Forms.NumericUpDown
        Me.qDcOffsetNumeric = New System.Windows.Forms.NumericUpDown
        Me.iqGaimbalanceNumeric = New System.Windows.Forms.NumericUpDown
        Me.cnrNumeric = New System.Windows.Forms.NumericUpDown
        Me.rfsgResourceTextBox = New System.Windows.Forms.TextBox
        Me.carrierFreqTextBox = New System.Windows.Forms.TextBox
        Me.actualHeadroomTextBox = New System.Windows.Forms.TextBox
        Me.autoheadroomEnabComboBox = New System.Windows.Forms.ComboBox
        Me.refSourceComboBox = New System.Windows.Forms.ComboBox
        Me.clkOutTerminalComboBox = New System.Windows.Forms.ComboBox
        Me.allIqImpairEnComboBox = New System.Windows.Forms.ComboBox
        Me.awgnEnabledComboBox = New System.Windows.Forms.ComboBox
        Me.timer = New System.Windows.Forms.Timer(Me.components)
        Me.carrierFreqOffNumeric = New System.Windows.Forms.NumericUpDown
        Me.waveNameLabel = New System.Windows.Forms.Label
        Me.scriptLabel = New System.Windows.Forms.Label
        Me.errorLabel = New System.Windows.Forms.Label
        Me.waveNameTextBox = New System.Windows.Forms.TextBox
        Me.scriptTextBox = New System.Windows.Forms.TextBox
        Me.errorTextBox = New System.Windows.Forms.TextBox
        Me.generateButton = New System.Windows.Forms.Button
        Me.stopButton = New System.Windows.Forms.Button
        Me.label2 = New System.Windows.Forms.Label
        Me.terminalConfigurationComboBox = New System.Windows.Forms.ComboBox
        Me.label1 = New System.Windows.Forms.Label
        Me.outputPortComboBox = New System.Windows.Forms.ComboBox
        Me.clkTerminalLabel = New System.Windows.Forms.Label
        Me.packetTypeLabel = New System.Windows.Forms.Label
        Me.packetTypeComboBox = New System.Windows.Forms.ComboBox
        Me.NumOfUniquePacketsLabel = New System.Windows.Forms.Label
        Me.NumOfUniquePacketsNumeric = New System.Windows.Forms.NumericUpDown
        Me.Label3 = New System.Windows.Forms.Label
        Me.insertRowButton = New System.Windows.Forms.Button
        Me.deleteRowButton = New System.Windows.Forms.Button
        Me.insertRowButton2 = New System.Windows.Forms.Button
        Me.deleteRowButton2 = New System.Windows.Forms.Button
        Me.dataGridView1Label = New System.Windows.Forms.Label
        Me.dataGridView1 = New System.Windows.Forms.DataGridView
        Me.dataGridView2Label = New System.Windows.Forms.Label
        Me.dataGridView2 = New System.Windows.Forms.DataGridView
        Me.Index = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.SoundingSequenceMarkerPositions = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Index2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.SoundingSequenceMarkerSignals = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.OversamplingFactorLabel = New System.Windows.Forms.Label
        Me.OversamplingFactorNumeric = New System.Windows.Forms.NumericUpDown
        Me.csPacketFormatLabel = New System.Windows.Forms.Label
        Me.csPacketFormatComboBox = New System.Windows.Forms.ComboBox
        Me.csSyncSequenceLabel = New System.Windows.Forms.Label
        Me.csSyncSequenceComboBox = New System.Windows.Forms.ComboBox
        Me.csPhaseMeasurementPeriodLabel = New System.Windows.Forms.Label
        Me.csPhaseMeasurementPeriodNumeric = New System.Windows.Forms.NumericUpDown
        Me.soundingSequenceLengthLabel = New System.Windows.Forms.Label
        Me.soundingSequenceLengthNumeric = New System.Windows.Forms.NumericUpDown
        Me.csToneExtSlotEnabledLabel = New System.Windows.Forms.Label
        Me.csToneExtSlotEnabledComboBox = New System.Windows.Forms.ComboBox
        CType(Me.chnNumberNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.externalAttnNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.headroomNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.quadratureSkewNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iDcOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.qDcOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqGaimbalanceNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cnrNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.carrierFreqOffNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumOfUniquePacketsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OversamplingFactorNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.csPhaseMeasurementPeriodNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.soundingSequenceLengthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'rfsgResourceLabel
        '
        Me.rfsgResourceLabel.AutoSize = True
        Me.rfsgResourceLabel.Location = New System.Drawing.Point(8, 32)
        Me.rfsgResourceLabel.Name = "rfsgResourceLabel"
        Me.rfsgResourceLabel.Size = New System.Drawing.Size(85, 13)
        Me.rfsgResourceLabel.TabIndex = 43
        Me.rfsgResourceLabel.Text = "RFSG Resource"
        '
        'chnNumberLabel
        '
        Me.chnNumberLabel.AutoSize = True
        Me.chnNumberLabel.Location = New System.Drawing.Point(8, 87)
        Me.chnNumberLabel.Name = "chnNumberLabel"
        Me.chnNumberLabel.Size = New System.Drawing.Size(86, 13)
        Me.chnNumberLabel.TabIndex = 44
        Me.chnNumberLabel.Text = "Channel Number"
        '
        'carrierFreqLabel
        '
        Me.carrierFreqLabel.AutoSize = True
        Me.carrierFreqLabel.Location = New System.Drawing.Point(8, 107)
        Me.carrierFreqLabel.Name = "carrierFreqLabel"
        Me.carrierFreqLabel.Size = New System.Drawing.Size(112, 13)
        Me.carrierFreqLabel.TabIndex = 47
        Me.carrierFreqLabel.Text = "Carrier Frequency (Hz)"
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(8, 168)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.powerLevelLabel.TabIndex = 48
        Me.powerLevelLabel.Text = "Power Level (dBm)"
        '
        'externalAttnLabel
        '
        Me.externalAttnLabel.AutoSize = True
        Me.externalAttnLabel.Location = New System.Drawing.Point(8, 190)
        Me.externalAttnLabel.Name = "externalAttnLabel"
        Me.externalAttnLabel.Size = New System.Drawing.Size(124, 13)
        Me.externalAttnLabel.TabIndex = 51
        Me.externalAttnLabel.Text = "External Attenuation (dB)"
        '
        'autoheadroomEnabLabel
        '
        Me.autoheadroomEnabLabel.AutoSize = True
        Me.autoheadroomEnabLabel.Location = New System.Drawing.Point(8, 212)
        Me.autoheadroomEnabLabel.Name = "autoheadroomEnabLabel"
        Me.autoheadroomEnabLabel.Size = New System.Drawing.Size(123, 13)
        Me.autoheadroomEnabLabel.TabIndex = 52
        Me.autoheadroomEnabLabel.Text = "Auto Headroom Enabled"
        '
        'headroomLabel
        '
        Me.headroomLabel.AutoSize = True
        Me.headroomLabel.Location = New System.Drawing.Point(8, 233)
        Me.headroomLabel.Name = "headroomLabel"
        Me.headroomLabel.Size = New System.Drawing.Size(78, 13)
        Me.headroomLabel.TabIndex = 54
        Me.headroomLabel.Text = "Headroom (dB)"
        '
        'actualHeadroomLabel
        '
        Me.actualHeadroomLabel.AutoSize = True
        Me.actualHeadroomLabel.Location = New System.Drawing.Point(8, 252)
        Me.actualHeadroomLabel.Name = "actualHeadroomLabel"
        Me.actualHeadroomLabel.Size = New System.Drawing.Size(111, 13)
        Me.actualHeadroomLabel.TabIndex = 56
        Me.actualHeadroomLabel.Text = "Actual Headroom (dB)"
        '
        'refSourceLabel
        '
        Me.refSourceLabel.AutoSize = True
        Me.refSourceLabel.Location = New System.Drawing.Point(8, 312)
        Me.refSourceLabel.Name = "refSourceLabel"
        Me.refSourceLabel.Size = New System.Drawing.Size(94, 13)
        Me.refSourceLabel.TabIndex = 59
        Me.refSourceLabel.Text = "Reference Source"
        '
        'clkOutputTerminalLabel
        '
        Me.clkOutputTerminalLabel.AutoSize = True
        Me.clkOutputTerminalLabel.Location = New System.Drawing.Point(8, 366)
        Me.clkOutputTerminalLabel.Name = "clkOutputTerminalLabel"
        Me.clkOutputTerminalLabel.Size = New System.Drawing.Size(100, 13)
        Me.clkOutputTerminalLabel.TabIndex = 60
        Me.clkOutputTerminalLabel.Text = "Clk Output Terminal"
        '
        'allIqImpairEnLabel
        '
        Me.allIqImpairEnLabel.AutoSize = True
        Me.allIqImpairEnLabel.Location = New System.Drawing.Point(8, 432)
        Me.allIqImpairEnLabel.Name = "allIqImpairEnLabel"
        Me.allIqImpairEnLabel.Size = New System.Drawing.Size(133, 13)
        Me.allIqImpairEnLabel.TabIndex = 62
        Me.allIqImpairEnLabel.Text = "All IQ Impairments Enabled"
        '
        'quadratureSkewLabel
        '
        Me.quadratureSkewLabel.AutoSize = True
        Me.quadratureSkewLabel.Location = New System.Drawing.Point(8, 454)
        Me.quadratureSkewLabel.Name = "quadratureSkewLabel"
        Me.quadratureSkewLabel.Size = New System.Drawing.Size(117, 13)
        Me.quadratureSkewLabel.TabIndex = 64
        Me.quadratureSkewLabel.Text = "Quadrature Skew (deg)"
        '
        'iDcOffsetLabel
        '
        Me.iDcOffsetLabel.AutoSize = True
        Me.iDcOffsetLabel.Location = New System.Drawing.Point(8, 476)
        Me.iDcOffsetLabel.Name = "iDcOffsetLabel"
        Me.iDcOffsetLabel.Size = New System.Drawing.Size(76, 13)
        Me.iDcOffsetLabel.TabIndex = 67
        Me.iDcOffsetLabel.Text = "I DC Offset (%)"
        '
        'qDcOffsetLabel
        '
        Me.qDcOffsetLabel.AutoSize = True
        Me.qDcOffsetLabel.Location = New System.Drawing.Point(8, 498)
        Me.qDcOffsetLabel.Name = "qDcOffsetLabel"
        Me.qDcOffsetLabel.Size = New System.Drawing.Size(81, 13)
        Me.qDcOffsetLabel.TabIndex = 69
        Me.qDcOffsetLabel.Text = "Q DC Offset (%)"
        '
        'iqGaimbalanceLabel
        '
        Me.iqGaimbalanceLabel.AutoSize = True
        Me.iqGaimbalanceLabel.Location = New System.Drawing.Point(8, 520)
        Me.iqGaimbalanceLabel.Name = "iqGaimbalanceLabel"
        Me.iqGaimbalanceLabel.Size = New System.Drawing.Size(117, 13)
        Me.iqGaimbalanceLabel.TabIndex = 70
        Me.iqGaimbalanceLabel.Text = "IQ Gain Imbalance (dB)"
        '
        'carrierFreqOffLabel
        '
        Me.carrierFreqOffLabel.AutoSize = True
        Me.carrierFreqOffLabel.Location = New System.Drawing.Point(8, 540)
        Me.carrierFreqOffLabel.Name = "carrierFreqOffLabel"
        Me.carrierFreqOffLabel.Size = New System.Drawing.Size(143, 13)
        Me.carrierFreqOffLabel.TabIndex = 72
        Me.carrierFreqOffLabel.Text = "Carrier Frequency Offset (Hz)"
        '
        'awgnEnabledLabel
        '
        Me.awgnEnabledLabel.AutoSize = True
        Me.awgnEnabledLabel.Location = New System.Drawing.Point(8, 561)
        Me.awgnEnabledLabel.Name = "awgnEnabledLabel"
        Me.awgnEnabledLabel.Size = New System.Drawing.Size(83, 13)
        Me.awgnEnabledLabel.TabIndex = 73
        Me.awgnEnabledLabel.Text = "AWGN Enabled"
        '
        'cnrLabel
        '
        Me.cnrLabel.AutoSize = True
        Me.cnrLabel.Location = New System.Drawing.Point(8, 581)
        Me.cnrLabel.Name = "cnrLabel"
        Me.cnrLabel.Size = New System.Drawing.Size(129, 13)
        Me.cnrLabel.TabIndex = 75
        Me.cnrLabel.Text = "Carrier to Noise Ratio (dB)"
        '
        'hardwareLabel
        '
        Me.hardwareLabel.AutoSize = True
        Me.hardwareLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.hardwareLabel.Location = New System.Drawing.Point(21, 9)
        Me.hardwareLabel.Name = "hardwareLabel"
        Me.hardwareLabel.Size = New System.Drawing.Size(61, 13)
        Me.hardwareLabel.TabIndex = 77
        Me.hardwareLabel.Text = "Hardware"
        '
        'frequencySettingsLabel
        '
        Me.frequencySettingsLabel.AutoSize = True
        Me.frequencySettingsLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frequencySettingsLabel.Location = New System.Drawing.Point(21, 294)
        Me.frequencySettingsLabel.Name = "frequencySettingsLabel"
        Me.frequencySettingsLabel.Size = New System.Drawing.Size(116, 13)
        Me.frequencySettingsLabel.TabIndex = 78
        Me.frequencySettingsLabel.Text = "Frequency Settings"
        '
        'impairmentsLabel
        '
        Me.impairmentsLabel.AutoSize = True
        Me.impairmentsLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.impairmentsLabel.Location = New System.Drawing.Point(21, 411)
        Me.impairmentsLabel.Name = "impairmentsLabel"
        Me.impairmentsLabel.Size = New System.Drawing.Size(74, 13)
        Me.impairmentsLabel.TabIndex = 79
        Me.impairmentsLabel.Text = "Impairments"
        '
        'chnNumberNumeric
        '
        Me.chnNumberNumeric.Location = New System.Drawing.Point(153, 82)
        Me.chnNumberNumeric.Maximum = New Decimal(New Integer() {39, 0, 0, 0})
        Me.chnNumberNumeric.Name = "chnNumberNumeric"
        Me.chnNumberNumeric.Size = New System.Drawing.Size(90, 20)
        Me.chnNumberNumeric.TabIndex = 45
        Me.chnNumberNumeric.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'powerLevelNumeric
        '
        Me.powerLevelNumeric.Location = New System.Drawing.Point(153, 165)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {25, 0, 0, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {179, 0, 0, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(90, 20)
        Me.powerLevelNumeric.TabIndex = 49
        '
        'externalAttnNumeric
        '
        Me.externalAttnNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.externalAttnNumeric.Location = New System.Drawing.Point(153, 187)
        Me.externalAttnNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.externalAttnNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.externalAttnNumeric.Name = "externalAttnNumeric"
        Me.externalAttnNumeric.Size = New System.Drawing.Size(90, 20)
        Me.externalAttnNumeric.TabIndex = 50
        '
        'headroomNumeric
        '
        Me.headroomNumeric.Location = New System.Drawing.Point(153, 230)
        Me.headroomNumeric.Name = "headroomNumeric"
        Me.headroomNumeric.Size = New System.Drawing.Size(90, 20)
        Me.headroomNumeric.TabIndex = 55
        '
        'quadratureSkewNumeric
        '
        Me.quadratureSkewNumeric.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
        Me.quadratureSkewNumeric.Location = New System.Drawing.Point(153, 453)
        Me.quadratureSkewNumeric.Name = "quadratureSkewNumeric"
        Me.quadratureSkewNumeric.Size = New System.Drawing.Size(90, 20)
        Me.quadratureSkewNumeric.TabIndex = 65
        '
        'iDcOffsetNumeric
        '
        Me.iDcOffsetNumeric.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
        Me.iDcOffsetNumeric.Location = New System.Drawing.Point(153, 475)
        Me.iDcOffsetNumeric.Name = "iDcOffsetNumeric"
        Me.iDcOffsetNumeric.Size = New System.Drawing.Size(90, 20)
        Me.iDcOffsetNumeric.TabIndex = 66
        '
        'qDcOffsetNumeric
        '
        Me.qDcOffsetNumeric.Increment = New Decimal(New Integer() {50, 0, 0, 131072})
        Me.qDcOffsetNumeric.Location = New System.Drawing.Point(153, 497)
        Me.qDcOffsetNumeric.Name = "qDcOffsetNumeric"
        Me.qDcOffsetNumeric.Size = New System.Drawing.Size(90, 20)
        Me.qDcOffsetNumeric.TabIndex = 68
        '
        'iqGaimbalanceNumeric
        '
        Me.iqGaimbalanceNumeric.Increment = New Decimal(New Integer() {50, 0, 0, 131072})
        Me.iqGaimbalanceNumeric.Location = New System.Drawing.Point(153, 519)
        Me.iqGaimbalanceNumeric.Name = "iqGaimbalanceNumeric"
        Me.iqGaimbalanceNumeric.Size = New System.Drawing.Size(90, 20)
        Me.iqGaimbalanceNumeric.TabIndex = 71
        '
        'cnrNumeric
        '
        Me.cnrNumeric.Increment = New Decimal(New Integer() {50, 0, 0, 131072})
        Me.cnrNumeric.Location = New System.Drawing.Point(153, 580)
        Me.cnrNumeric.Name = "cnrNumeric"
        Me.cnrNumeric.Size = New System.Drawing.Size(90, 20)
        Me.cnrNumeric.TabIndex = 76
        Me.cnrNumeric.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'rfsgResourceTextBox
        '
        Me.rfsgResourceTextBox.Location = New System.Drawing.Point(153, 25)
        Me.rfsgResourceTextBox.Name = "rfsgResourceTextBox"
        Me.rfsgResourceTextBox.Size = New System.Drawing.Size(90, 20)
        Me.rfsgResourceTextBox.TabIndex = 42
        Me.rfsgResourceTextBox.Text = "RFSG"
        '
        'carrierFreqTextBox
        '
        Me.carrierFreqTextBox.Enabled = False
        Me.carrierFreqTextBox.Location = New System.Drawing.Point(153, 102)
        Me.carrierFreqTextBox.Name = "carrierFreqTextBox"
        Me.carrierFreqTextBox.Size = New System.Drawing.Size(90, 20)
        Me.carrierFreqTextBox.TabIndex = 46
        Me.carrierFreqTextBox.Text = "2.408E+9"
        '
        'actualHeadroomTextBox
        '
        Me.actualHeadroomTextBox.Enabled = False
        Me.actualHeadroomTextBox.Location = New System.Drawing.Point(153, 249)
        Me.actualHeadroomTextBox.Name = "actualHeadroomTextBox"
        Me.actualHeadroomTextBox.Size = New System.Drawing.Size(90, 20)
        Me.actualHeadroomTextBox.TabIndex = 57
        Me.actualHeadroomTextBox.Text = "0.00"
        '
        'autoheadroomEnabComboBox
        '
        Me.autoheadroomEnabComboBox.Location = New System.Drawing.Point(153, 209)
        Me.autoheadroomEnabComboBox.Name = "autoheadroomEnabComboBox"
        Me.autoheadroomEnabComboBox.Size = New System.Drawing.Size(90, 21)
        Me.autoheadroomEnabComboBox.TabIndex = 53
        '
        'refSourceComboBox
        '
        Me.refSourceComboBox.Location = New System.Drawing.Point(153, 312)
        Me.refSourceComboBox.Name = "refSourceComboBox"
        Me.refSourceComboBox.Size = New System.Drawing.Size(90, 21)
        Me.refSourceComboBox.TabIndex = 58
        '
        'clkOutTerminalComboBox
        '
        Me.clkOutTerminalComboBox.Location = New System.Drawing.Point(153, 366)
        Me.clkOutTerminalComboBox.Name = "clkOutTerminalComboBox"
        Me.clkOutTerminalComboBox.Size = New System.Drawing.Size(90, 21)
        Me.clkOutTerminalComboBox.TabIndex = 61
        '
        'allIqImpairEnComboBox
        '
        Me.allIqImpairEnComboBox.Location = New System.Drawing.Point(153, 431)
        Me.allIqImpairEnComboBox.Name = "allIqImpairEnComboBox"
        Me.allIqImpairEnComboBox.Size = New System.Drawing.Size(90, 21)
        Me.allIqImpairEnComboBox.TabIndex = 63
        '
        'awgnEnabledComboBox
        '
        Me.awgnEnabledComboBox.Location = New System.Drawing.Point(153, 560)
        Me.awgnEnabledComboBox.Name = "awgnEnabledComboBox"
        Me.awgnEnabledComboBox.Size = New System.Drawing.Size(90, 21)
        Me.awgnEnabledComboBox.TabIndex = 74
        '
        'timer
        '
        '
        'carrierFreqOffNumeric
        '
        Me.carrierFreqOffNumeric.Location = New System.Drawing.Point(153, 538)
        Me.carrierFreqOffNumeric.Name = "carrierFreqOffNumeric"
        Me.carrierFreqOffNumeric.Size = New System.Drawing.Size(90, 20)
        Me.carrierFreqOffNumeric.TabIndex = 80
        '
        'waveNameLabel
        '
        Me.waveNameLabel.AutoSize = True
        Me.waveNameLabel.Location = New System.Drawing.Point(283, 101)
        Me.waveNameLabel.Name = "waveNameLabel"
        Me.waveNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.waveNameLabel.TabIndex = 88
        Me.waveNameLabel.Text = "Waveform Name"
        '
        'scriptLabel
        '
        Me.scriptLabel.AutoSize = True
        Me.scriptLabel.Location = New System.Drawing.Point(455, 12)
        Me.scriptLabel.Name = "scriptLabel"
        Me.scriptLabel.Size = New System.Drawing.Size(34, 13)
        Me.scriptLabel.TabIndex = 85
        Me.scriptLabel.Text = "Script"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(516, 468)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(29, 13)
        Me.errorLabel.TabIndex = 89
        Me.errorLabel.Text = "Error"
        '
        'waveNameTextBox
        '
        Me.waveNameTextBox.Location = New System.Drawing.Point(286, 119)
        Me.waveNameTextBox.Name = "waveNameTextBox"
        Me.waveNameTextBox.Size = New System.Drawing.Size(120, 20)
        Me.waveNameTextBox.TabIndex = 87
        Me.waveNameTextBox.Text = "LECS"
        '
        'scriptTextBox
        '
        Me.scriptTextBox.Location = New System.Drawing.Point(460, 29)
        Me.scriptTextBox.Multiline = True
        Me.scriptTextBox.Name = "scriptTextBox"
        Me.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.scriptTextBox.Size = New System.Drawing.Size(152, 85)
        Me.scriptTextBox.TabIndex = 86
        Me.scriptTextBox.TabStop = False
        Me.scriptTextBox.Text = "script GenerateLEPkt" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  repeat forever" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    generate LECS" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  end repeat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "end scri" &
            "pt"
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(516, 489)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(192, 116)
        Me.errorTextBox.TabIndex = 90
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No Error"
        '
        'generateButton
        '
        Me.generateButton.Location = New System.Drawing.Point(286, 576)
        Me.generateButton.Name = "generateButton"
        Me.generateButton.Size = New System.Drawing.Size(75, 23)
        Me.generateButton.TabIndex = 93
        Me.generateButton.Text = "&Generate"
        Me.generateButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.Location = New System.Drawing.Point(374, 576)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 94
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(283, 59)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(112, 13)
        Me.label2.TabIndex = 100
        Me.label2.Text = "Terminal Configuration"
        '
        'terminalConfigurationComboBox
        '
        Me.terminalConfigurationComboBox.Location = New System.Drawing.Point(286, 75)
        Me.terminalConfigurationComboBox.Name = "terminalConfigurationComboBox"
        Me.terminalConfigurationComboBox.Size = New System.Drawing.Size(120, 21)
        Me.terminalConfigurationComboBox.TabIndex = 99
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(283, 12)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(61, 13)
        Me.label1.TabIndex = 98
        Me.label1.Text = "Output Port"
        '
        'outputPortComboBox
        '
        Me.outputPortComboBox.Location = New System.Drawing.Point(286, 29)
        Me.outputPortComboBox.Name = "outputPortComboBox"
        Me.outputPortComboBox.Size = New System.Drawing.Size(120, 21)
        Me.outputPortComboBox.TabIndex = 97
        '
        'clkTerminalLabel
        '
        Me.clkTerminalLabel.AutoSize = True
        Me.clkTerminalLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clkTerminalLabel.Location = New System.Drawing.Point(21, 350)
        Me.clkTerminalLabel.Name = "clkTerminalLabel"
        Me.clkTerminalLabel.Size = New System.Drawing.Size(129, 13)
        Me.clkTerminalLabel.TabIndex = 78
        Me.clkTerminalLabel.Text = "Export Clock Settings"
        '
        'packetTypeLabel
        '
        Me.packetTypeLabel.AutoSize = True
        Me.packetTypeLabel.Location = New System.Drawing.Point(283, 190)
        Me.packetTypeLabel.Name = "packetTypeLabel"
        Me.packetTypeLabel.Size = New System.Drawing.Size(68, 13)
        Me.packetTypeLabel.TabIndex = 88
        Me.packetTypeLabel.Text = "Packet Type"
        '
        'packetTypeComboBox
        '
        Me.packetTypeComboBox.FormattingEnabled = True
        Me.packetTypeComboBox.Location = New System.Drawing.Point(286, 206)
        Me.packetTypeComboBox.Name = "packetTypeComboBox"
        Me.packetTypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.packetTypeComboBox.TabIndex = 92
        '
        'NumOfUniquePacketsLabel
        '
        Me.NumOfUniquePacketsLabel.AutoSize = True
        Me.NumOfUniquePacketsLabel.Location = New System.Drawing.Point(283, 237)
        Me.NumOfUniquePacketsLabel.Name = "NumOfUniquePacketsLabel"
        Me.NumOfUniquePacketsLabel.Size = New System.Drawing.Size(135, 13)
        Me.NumOfUniquePacketsLabel.TabIndex = 88
        Me.NumOfUniquePacketsLabel.Text = "Number of Unique Packets"
        '
        'NumOfUniquePacketsNumeric
        '
        Me.NumOfUniquePacketsNumeric.Location = New System.Drawing.Point(286, 253)
        Me.NumOfUniquePacketsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.NumOfUniquePacketsNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.NumOfUniquePacketsNumeric.Name = "NumOfUniquePacketsNumeric"
        Me.NumOfUniquePacketsNumeric.Size = New System.Drawing.Size(120, 20)
        Me.NumOfUniquePacketsNumeric.TabIndex = 83
        Me.NumOfUniquePacketsNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(290, 294)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(103, 13)
        Me.Label3.TabIndex = 103
        Me.Label3.Text = "Channel Sounding"
        '
        'csPacketFormatLabel
        '
        Me.csPacketFormatLabel.AutoSize = True
        Me.csPacketFormatLabel.Location = New System.Drawing.Point(286, 312)
        Me.csPacketFormatLabel.Name = "csPacketFormatLabel"
        Me.csPacketFormatLabel.Size = New System.Drawing.Size(68, 13)
        Me.csPacketFormatLabel.TabIndex = 95
        Me.csPacketFormatLabel.Text = "CS Packet Format"
        '
        'csPacketFormatComboBox
        '
        Me.csPacketFormatComboBox.FormattingEnabled = True
        Me.csPacketFormatComboBox.Location = New System.Drawing.Point(286, 328)
        Me.csPacketFormatComboBox.Name = "csPacketFormatComboBox"
        Me.csPacketFormatComboBox.Size = New System.Drawing.Size(120, 21)
        Me.csPacketFormatComboBox.TabIndex = 96
        '
        'csSyncSequenceLabel
        '
        Me.csSyncSequenceLabel.AutoSize = True
        Me.csSyncSequenceLabel.Location = New System.Drawing.Point(286, 358)
        Me.csSyncSequenceLabel.Name = "csSyncSequenceLabel"
        Me.csSyncSequenceLabel.Size = New System.Drawing.Size(68, 13)
        Me.csSyncSequenceLabel.TabIndex = 97
        Me.csSyncSequenceLabel.Text = "CS Sync Sequence"
        '
        'csSyncSequenceComboBox
        '
        Me.csSyncSequenceComboBox.FormattingEnabled = True
        Me.csSyncSequenceComboBox.Location = New System.Drawing.Point(286, 374)
        Me.csSyncSequenceComboBox.Name = "csSyncSequenceComboBox"
        Me.csSyncSequenceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.csSyncSequenceComboBox.TabIndex = 98
        '
        'csPhaseMeasurementsPeriodLabel
        '
        Me.csPhaseMeasurementPeriodLabel.AutoSize = True
        Me.csPhaseMeasurementPeriodLabel.Location = New System.Drawing.Point(286, 404)
        Me.csPhaseMeasurementPeriodLabel.Name = "csPhaseMeasurementsPeriodLabel"
        Me.csPhaseMeasurementPeriodLabel.Size = New System.Drawing.Size(135, 13)
        Me.csPhaseMeasurementPeriodLabel.TabIndex = 99
        Me.csPhaseMeasurementPeriodLabel.Text = "CS Phase Measurements Period (s)"
        '
        'csPhaseMeasurementsPeriodNumeric
        '
        Me.csPhaseMeasurementPeriodNumeric.Location = New System.Drawing.Point(286, 420)
        Me.csPhaseMeasurementPeriodNumeric.DecimalPlaces = 6
        Me.csPhaseMeasurementPeriodNumeric.Maximum = New Decimal(New Integer() {4, 0, 0, 327680})
        Me.csPhaseMeasurementPeriodNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 327680})
        Me.csPhaseMeasurementPeriodNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 327680})
        Me.csPhaseMeasurementPeriodNumeric.Name = "csPhaseMeasurementsPeriodNumeric"
        Me.csPhaseMeasurementPeriodNumeric.Size = New System.Drawing.Size(120, 20)
        Me.csPhaseMeasurementPeriodNumeric.TabIndex = 100
        Me.csPhaseMeasurementPeriodNumeric.Value = New Decimal(New Integer() {1, 0, 0, 327680})
        '
        'soundingSequenceLengthLabel
        '
        Me.soundingSequenceLengthLabel.AutoSize = True
        Me.soundingSequenceLengthLabel.Location = New System.Drawing.Point(286, 450)
        Me.soundingSequenceLengthLabel.Name = "soundingSequenceLengthLabel"
        Me.soundingSequenceLengthLabel.Size = New System.Drawing.Size(135, 13)
        Me.soundingSequenceLengthLabel.TabIndex = 101
        Me.soundingSequenceLengthLabel.Text = "Sounding Sequence Length"
        '
        'soundingSequenceLengthNumeric
        '
        Me.soundingSequenceLengthNumeric.Location = New System.Drawing.Point(286, 466)
        Me.soundingSequenceLengthNumeric.Maximum = New Decimal(New Integer() {96, 0, 0, 0})
        Me.soundingSequenceLengthNumeric.Minimum = New Decimal(New Integer() {32, 0, 0, 0})
        Me.soundingSequenceLengthNumeric.Increment = New Decimal(New Integer() {32, 0, 0, 0})
        Me.soundingSequenceLengthNumeric.Name = "soundingSequenceLengthNumeric"
        Me.soundingSequenceLengthNumeric.Size = New System.Drawing.Size(120, 20)
        Me.soundingSequenceLengthNumeric.TabIndex = 102
        Me.soundingSequenceLengthNumeric.Value = New Decimal(New Integer() {32, 0, 0, 0})
        '
        'csToneExtSlotEnabledLabel
        '
        Me.csToneExtSlotEnabledLabel.AutoSize = True
        Me.csToneExtSlotEnabledLabel.Location = New System.Drawing.Point(286, 496)
        Me.csToneExtSlotEnabledLabel.Name = "csToneExtSlotEnabledLabel"
        Me.csToneExtSlotEnabledLabel.Size = New System.Drawing.Size(68, 13)
        Me.csToneExtSlotEnabledLabel.TabIndex = 104
        Me.csToneExtSlotEnabledLabel.Text = "CS Tone Extension Slot Enabled"
        '
        'csToneExtSlotEnabledComboBox
        '
        Me.csToneExtSlotEnabledComboBox.FormattingEnabled = True
        Me.csToneExtSlotEnabledComboBox.Location = New System.Drawing.Point(286, 512)
        Me.csToneExtSlotEnabledComboBox.Name = "csToneExtSlotEnabledComboBox"
        Me.csToneExtSlotEnabledComboBox.Size = New System.Drawing.Size(120, 21)
        Me.csToneExtSlotEnabledComboBox.TabIndex = 105
        '
        'insertRowButton
        '
        Me.insertRowButton.Location = New System.Drawing.Point(510, 260)
        Me.insertRowButton.Name = "insertRowButton"
        Me.insertRowButton.Size = New System.Drawing.Size(50, 22)
        Me.insertRowButton.TabIndex = 116
        Me.insertRowButton.Text = "Insert"
        Me.insertRowButton.UseVisualStyleBackColor = True
        '
        'deleteRowButton
        '
        Me.deleteRowButton.Location = New System.Drawing.Point(565, 260)
        Me.deleteRowButton.Name = "deleteRowButton"
        Me.deleteRowButton.Size = New System.Drawing.Size(50, 22)
        Me.deleteRowButton.TabIndex = 117
        Me.deleteRowButton.Text = "Delete"
        Me.deleteRowButton.UseVisualStyleBackColor = True
        '
        'insertRowButton2
        '
        Me.insertRowButton2.Location = New System.Drawing.Point(510, 420)
        Me.insertRowButton2.Name = "insertRowButton"
        Me.insertRowButton2.Size = New System.Drawing.Size(50, 22)
        Me.insertRowButton2.TabIndex = 116
        Me.insertRowButton2.Text = "Insert"
        Me.insertRowButton2.UseVisualStyleBackColor = True
        '
        'deleteRowButton2
        '
        Me.deleteRowButton2.Location = New System.Drawing.Point(565, 420)
        Me.deleteRowButton2.Name = "deleteRowButton"
        Me.deleteRowButton2.Size = New System.Drawing.Size(50, 22)
        Me.deleteRowButton2.TabIndex = 117
        Me.deleteRowButton2.Text = "Delete"
        Me.deleteRowButton2.UseVisualStyleBackColor = True
        '
        'dataGridView1Label
        '
        Me.dataGridView1Label.AutoSize = True
        Me.dataGridView1Label.Location = New System.Drawing.Point(455, 130)
        Me.dataGridView1Label.Name = "dataGridView1Label"
        Me.dataGridView1Label.Size = New System.Drawing.Size(34, 13)
        Me.dataGridView1Label.TabIndex = 119
        Me.dataGridView1Label.Text = "Sounding Sequence Marker Positions"
        '
        'dataGridView1
        '
        Me.dataGridView1.AllowUserToAddRows = False
        Me.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Index, Me.SoundingSequenceMarkerPositions})
        Me.dataGridView1.Location = New System.Drawing.Point(460, 150)
        Me.dataGridView1.Name = "dataGridView1"
        Me.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dataGridView1.Size = New System.Drawing.Size(155, 102)
        Me.dataGridView1.TabIndex = 118
        '
        'dataGridView2Label
        '
        Me.dataGridView2Label.AutoSize = True
        Me.dataGridView2Label.Location = New System.Drawing.Point(455, 290)
        Me.dataGridView2Label.Name = "dataGridView2Label"
        Me.dataGridView2Label.Size = New System.Drawing.Size(34, 13)
        Me.dataGridView2Label.TabIndex = 120
        Me.dataGridView2Label.Text = "Sounding Sequence Marker Signals"
        '
        'dataGridView2
        '
        Me.dataGridView2.AllowUserToAddRows = False
        Me.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Index2, Me.SoundingSequenceMarkerSignals})
        Me.dataGridView2.Location = New System.Drawing.Point(460, 310)
        Me.dataGridView2.Name = "dataGridView2"
        Me.dataGridView2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dataGridView2.Size = New System.Drawing.Size(155, 102)
        Me.dataGridView2.TabIndex = 121
        '
        'Index
        '
        Me.Index.Frozen = True
        Me.Index.HeaderText = "Index"
        Me.Index.Name = "Index"
        Me.Index.ReadOnly = True
        Me.Index.Width = 40
        '
        'Index2
        '
        Me.Index2.Frozen = True
        Me.Index2.HeaderText = "Index"
        Me.Index2.Name = "Index"
        Me.Index2.ReadOnly = True
        Me.Index2.Width = 40
        '
        'SoundingSequenceMarkerPositions
        '
        Me.SoundingSequenceMarkerPositions.HeaderText = "Sounding Sequence Marker Positions"
        Me.SoundingSequenceMarkerPositions.Name = "SoundingSequenceMarkerPositions"
        '
        'SoundingSequenceMarkerSignals
        '
        Me.SoundingSequenceMarkerSignals.HeaderText = "Sounding Sequence         Marker        Signals"
        Me.SoundingSequenceMarkerSignals.Name = "SoundingSequenceMarkerSignals"
        '
        'OversamplingFactorLabel
        '
        Me.OversamplingFactorLabel.AutoSize = True
        Me.OversamplingFactorLabel.Location = New System.Drawing.Point(283, 148)
        Me.OversamplingFactorLabel.Name = "OversamplingFactorLabel"
        Me.OversamplingFactorLabel.Size = New System.Drawing.Size(104, 13)
        Me.OversamplingFactorLabel.TabIndex = 121
        Me.OversamplingFactorLabel.Text = "Oversampling Factor"
        '
        'OversamplingFactorNumeric
        '
        Me.OversamplingFactorNumeric.Location = New System.Drawing.Point(286, 164)
        Me.OversamplingFactorNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.OversamplingFactorNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.OversamplingFactorNumeric.Name = "OversamplingFactorNumeric"
        Me.OversamplingFactorNumeric.Size = New System.Drawing.Size(120, 20)
        Me.OversamplingFactorNumeric.TabIndex = 120
        Me.OversamplingFactorNumeric.Value = New Decimal(New Integer() {8, 0, 0, 0})
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(753, 625)
        Me.Controls.Add(Me.OversamplingFactorLabel)
        Me.Controls.Add(Me.OversamplingFactorNumeric)
        Me.Controls.Add(Me.dataGridView1Label)
        Me.Controls.Add(Me.dataGridView1)
        Me.Controls.Add(Me.dataGridView2Label)
        Me.Controls.Add(Me.dataGridView2)
        Me.Controls.Add(Me.insertRowButton)
        Me.Controls.Add(Me.deleteRowButton)
        Me.Controls.Add(Me.insertRowButton2)
        Me.Controls.Add(Me.deleteRowButton2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.terminalConfigurationComboBox)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.outputPortComboBox)
        Me.Controls.Add(Me.generateButton)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.packetTypeComboBox)
        Me.Controls.Add(Me.NumOfUniquePacketsLabel)
        Me.Controls.Add(Me.packetTypeLabel)
        Me.Controls.Add(Me.waveNameLabel)
        Me.Controls.Add(Me.scriptLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.NumOfUniquePacketsNumeric)
        Me.Controls.Add(Me.waveNameTextBox)
        Me.Controls.Add(Me.scriptTextBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.carrierFreqOffNumeric)
        Me.Controls.Add(Me.rfsgResourceLabel)
        Me.Controls.Add(Me.chnNumberLabel)
        Me.Controls.Add(Me.carrierFreqLabel)
        Me.Controls.Add(Me.powerLevelLabel)
        Me.Controls.Add(Me.externalAttnLabel)
        Me.Controls.Add(Me.autoheadroomEnabLabel)
        Me.Controls.Add(Me.headroomLabel)
        Me.Controls.Add(Me.actualHeadroomLabel)
        Me.Controls.Add(Me.refSourceLabel)
        Me.Controls.Add(Me.clkOutputTerminalLabel)
        Me.Controls.Add(Me.allIqImpairEnLabel)
        Me.Controls.Add(Me.quadratureSkewLabel)
        Me.Controls.Add(Me.iDcOffsetLabel)
        Me.Controls.Add(Me.qDcOffsetLabel)
        Me.Controls.Add(Me.iqGaimbalanceLabel)
        Me.Controls.Add(Me.carrierFreqOffLabel)
        Me.Controls.Add(Me.awgnEnabledLabel)
        Me.Controls.Add(Me.cnrLabel)
        Me.Controls.Add(Me.hardwareLabel)
        Me.Controls.Add(Me.clkTerminalLabel)
        Me.Controls.Add(Me.frequencySettingsLabel)
        Me.Controls.Add(Me.impairmentsLabel)
        Me.Controls.Add(Me.chnNumberNumeric)
        Me.Controls.Add(Me.powerLevelNumeric)
        Me.Controls.Add(Me.externalAttnNumeric)
        Me.Controls.Add(Me.headroomNumeric)
        Me.Controls.Add(Me.quadratureSkewNumeric)
        Me.Controls.Add(Me.iDcOffsetNumeric)
        Me.Controls.Add(Me.qDcOffsetNumeric)
        Me.Controls.Add(Me.iqGaimbalanceNumeric)
        Me.Controls.Add(Me.cnrNumeric)
        Me.Controls.Add(Me.rfsgResourceTextBox)
        Me.Controls.Add(Me.carrierFreqTextBox)
        Me.Controls.Add(Me.actualHeadroomTextBox)
        Me.Controls.Add(Me.autoheadroomEnabComboBox)
        Me.Controls.Add(Me.refSourceComboBox)
        Me.Controls.Add(Me.clkOutTerminalComboBox)
        Me.Controls.Add(Me.allIqImpairEnComboBox)
        Me.Controls.Add(Me.awgnEnabledComboBox)
        Me.Controls.Add(Me.csPacketFormatLabel)
        Me.Controls.Add(Me.csPacketFormatComboBox)
        Me.Controls.Add(Me.csSyncSequenceLabel)
        Me.Controls.Add(Me.csSyncSequenceComboBox)
        Me.Controls.Add(Me.csPhaseMeasurementPeriodLabel)
        Me.Controls.Add(Me.csPhaseMeasurementPeriodNumeric)
        Me.Controls.Add(Me.soundingSequenceLengthLabel)
        Me.Controls.Add(Me.soundingSequenceLengthNumeric)
        Me.Controls.Add(Me.csToneExtSlotEnabledLabel)
        Me.Controls.Add(Me.csToneExtSlotEnabledComboBox)
        Me.Name = "MainForm"
        Me.Text = "Form1"
        CType(Me.chnNumberNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.externalAttnNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.headroomNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.quadratureSkewNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iDcOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.qDcOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqGaimbalanceNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cnrNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.carrierFreqOffNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumOfUniquePacketsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OversamplingFactorNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.csPhaseMeasurementPeriodNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.soundingSequenceLengthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private rfsgResourceLabel As System.Windows.Forms.Label
    Private chnNumberLabel As System.Windows.Forms.Label
    Private carrierFreqLabel As System.Windows.Forms.Label
    Private powerLevelLabel As System.Windows.Forms.Label
    Private externalAttnLabel As System.Windows.Forms.Label
    Private autoheadroomEnabLabel As System.Windows.Forms.Label
    Private headroomLabel As System.Windows.Forms.Label
    Private actualHeadroomLabel As System.Windows.Forms.Label
    Private refSourceLabel As System.Windows.Forms.Label
    Private clkOutputTerminalLabel As System.Windows.Forms.Label
    Private allIqImpairEnLabel As System.Windows.Forms.Label
    Private quadratureSkewLabel As System.Windows.Forms.Label
    Private iDcOffsetLabel As System.Windows.Forms.Label
    Private qDcOffsetLabel As System.Windows.Forms.Label
    Private iqGaimbalanceLabel As System.Windows.Forms.Label
    Private carrierFreqOffLabel As System.Windows.Forms.Label
    Private awgnEnabledLabel As System.Windows.Forms.Label
    Private cnrLabel As System.Windows.Forms.Label
    Private hardwareLabel As System.Windows.Forms.Label
    Private frequencySettingsLabel As System.Windows.Forms.Label
    Private impairmentsLabel As System.Windows.Forms.Label
    Private chnNumberNumeric As System.Windows.Forms.NumericUpDown
    Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private externalAttnNumeric As System.Windows.Forms.NumericUpDown
    Private headroomNumeric As System.Windows.Forms.NumericUpDown
    Private quadratureSkewNumeric As System.Windows.Forms.NumericUpDown
    Private iDcOffsetNumeric As System.Windows.Forms.NumericUpDown
    Private qDcOffsetNumeric As System.Windows.Forms.NumericUpDown
    Private iqGaimbalanceNumeric As System.Windows.Forms.NumericUpDown
    Private cnrNumeric As System.Windows.Forms.NumericUpDown
    Private rfsgResourceTextBox As System.Windows.Forms.TextBox
    Private carrierFreqTextBox As System.Windows.Forms.TextBox
    Private actualHeadroomTextBox As System.Windows.Forms.TextBox
    Private autoheadroomEnabComboBox As System.Windows.Forms.ComboBox
    Private refSourceComboBox As System.Windows.Forms.ComboBox
    Private clkOutTerminalComboBox As System.Windows.Forms.ComboBox
    Private allIqImpairEnComboBox As System.Windows.Forms.ComboBox
    Private awgnEnabledComboBox As System.Windows.Forms.ComboBox
    Private WithEvents timer As System.Windows.Forms.Timer
    Private carrierFreqOffNumeric As System.Windows.Forms.NumericUpDown
    Private waveNameLabel As System.Windows.Forms.Label
    Private scriptLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private waveNameTextBox As System.Windows.Forms.TextBox
    Private scriptTextBox As System.Windows.Forms.TextBox
    Private errorTextBox As System.Windows.Forms.TextBox
    Private WithEvents generateButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private label2 As System.Windows.Forms.Label
    Private terminalConfigurationComboBox As System.Windows.Forms.ComboBox
    Private label1 As System.Windows.Forms.Label
    Private outputPortComboBox As System.Windows.Forms.ComboBox
    Private WithEvents clkTerminalLabel As System.Windows.Forms.Label
    Private WithEvents packetTypeLabel As System.Windows.Forms.Label
    Private WithEvents packetTypeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents NumOfUniquePacketsLabel As System.Windows.Forms.Label
    Private WithEvents NumOfUniquePacketsNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents Label3 As System.Windows.Forms.Label
    Private WithEvents insertRowButton As System.Windows.Forms.Button
    Private WithEvents deleteRowButton As System.Windows.Forms.Button
    Private WithEvents insertRowButton2 As System.Windows.Forms.Button
    Private WithEvents deleteRowButton2 As System.Windows.Forms.Button
    Private dataGridView1Label As System.Windows.Forms.Label
    Private WithEvents dataGridView1 As System.Windows.Forms.DataGridView
    Private dataGridView2Label As System.Windows.Forms.Label
    Private WithEvents dataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents Index As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Index2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SoundingSequenceMarkerPositions As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SoundingSequenceMarkerSignals As System.Windows.Forms.DataGridViewComboBoxColumn
    Private WithEvents OversamplingFactorLabel As System.Windows.Forms.Label
    Private WithEvents OversamplingFactorNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents csPacketFormatLabel As System.Windows.Forms.Label
    Private WithEvents csPacketFormatComboBox As System.Windows.Forms.ComboBox
    Private WithEvents csSyncSequenceLabel As System.Windows.Forms.Label
    Private WithEvents csSyncSequenceComboBox As System.Windows.Forms.ComboBox
    Private WithEvents csPhaseMeasurementPeriodLabel As System.Windows.Forms.Label
    Private WithEvents csPhaseMeasurementPeriodNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents soundingSequenceLengthLabel As System.Windows.Forms.Label
    Private WithEvents soundingSequenceLengthNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents csToneExtSlotEnabledLabel As System.Windows.Forms.Label
    Private WithEvents csToneExtSlotEnabledComboBox As System.Windows.Forms.ComboBox
End Class

