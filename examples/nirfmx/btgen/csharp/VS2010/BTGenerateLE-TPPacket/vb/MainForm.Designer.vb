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
        Me.payHdrPaylenModeLabel = New System.Windows.Forms.Label
        Me.payHdrPaylenLabel = New System.Windows.Forms.Label
        Me.waveNameLabel = New System.Windows.Forms.Label
        Me.scriptLabel = New System.Windows.Forms.Label
        Me.errorLabel = New System.Windows.Forms.Label
        Me.payHdrPaylenNumeric = New System.Windows.Forms.NumericUpDown
        Me.waveNameTextBox = New System.Windows.Forms.TextBox
        Me.scriptTextBox = New System.Windows.Forms.TextBox
        Me.errorTextBox = New System.Windows.Forms.TextBox
        Me.payHdrPaylenModeComboBox = New System.Windows.Forms.ComboBox
        Me.letpPayloadTypeLabel = New System.Windows.Forms.Label
        Me.letpPayloadTypeComboBox = New System.Windows.Forms.ComboBox
        Me.generateButton = New System.Windows.Forms.Button
        Me.stopButton = New System.Windows.Forms.Button
        Me.dirtyTxComboBox = New System.Windows.Forms.ComboBox
        Me.dirtyTxLabel = New System.Windows.Forms.Label
        Me.label2 = New System.Windows.Forms.Label
        Me.terminalConfigurationComboBox = New System.Windows.Forms.ComboBox
        Me.label1 = New System.Windows.Forms.Label
        Me.outputPortComboBox = New System.Windows.Forms.ComboBox
        Me.clkTerminalLabel = New System.Windows.Forms.Label
        Me.packetTypeLabel = New System.Windows.Forms.Label
        Me.packetTypeComboBox = New System.Windows.Forms.ComboBox
        Me.letpCorruptAlternateCRCLabel = New System.Windows.Forms.Label
        Me.letpCorruptAlternateCRCComboBox = New System.Windows.Forms.ComboBox
        Me.NumOfUniquePacketsLabel = New System.Windows.Forms.Label
        Me.NumOfUniquePacketsNumeric = New System.Windows.Forms.NumericUpDown
        Me.UserDefinedBitsLabel = New System.Windows.Forms.Label
        Me.antennaSwitchingEnabledLabel = New System.Windows.Forms.Label
        Me.cteSlotDurationLabel = New System.Windows.Forms.Label
        Me.antennaSwitchingEnabledComboBox = New System.Windows.Forms.ComboBox
        Me.cteSlotDurationComboBox = New System.Windows.Forms.ComboBox
        Me.directionFindingModeComboBox = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.directionFindingModeLabel = New System.Windows.Forms.Label
        Me.antennaSwitchingPatternLabel = New System.Windows.Forms.Label
        Me.numberOfAntennasLabel = New System.Windows.Forms.Label
        Me.cteLengthLabel = New System.Windows.Forms.Label
        Me.numberOfAntennasNumeric = New System.Windows.Forms.NumericUpDown
        Me.cteLengthNumeric = New System.Windows.Forms.NumericUpDown
        Me.antennaSwitchingPatternTextBox = New System.Windows.Forms.TextBox
        Me.insertRowButton = New System.Windows.Forms.Button
        Me.deleteRowButton = New System.Windows.Forms.Button
        Me.dataGridView1 = New System.Windows.Forms.DataGridView
        Me.Index = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.RelativePhase = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.RelativeAmplitude = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.userDefinedBitsTextBox = New System.Windows.Forms.TextBox
        Me.OversamplingFactorLabel = New System.Windows.Forms.Label
        Me.OversamplingFactorNumeric = New System.Windows.Forms.NumericUpDown
        Me.AntennaSwitchingDurationLabel = New System.Windows.Forms.Label
        Me.AntennaSwitchingDurationNumeric = New System.Windows.Forms.NumericUpDown
        Me.AntennaSwitchingDurationUsedTextBox = New System.Windows.Forms.TextBox
        Me.AntennaSwitchingDurationUsedLabel = New System.Windows.Forms.Label
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
        CType(Me.payHdrPaylenNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumOfUniquePacketsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberOfAntennasNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cteLengthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OversamplingFactorNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AntennaSwitchingDurationNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'payHdrPaylenModeLabel
        '
        Me.payHdrPaylenModeLabel.AutoSize = True
        Me.payHdrPaylenModeLabel.Location = New System.Drawing.Point(283, 329)
        Me.payHdrPaylenModeLabel.Name = "payHdrPaylenModeLabel"
        Me.payHdrPaylenModeLabel.Size = New System.Drawing.Size(111, 13)
        Me.payHdrPaylenModeLabel.TabIndex = 82
        Me.payHdrPaylenModeLabel.Text = "Payload Length Mode"
        '
        'payHdrPaylenLabel
        '
        Me.payHdrPaylenLabel.AutoSize = True
        Me.payHdrPaylenLabel.Location = New System.Drawing.Point(283, 372)
        Me.payHdrPaylenLabel.Name = "payHdrPaylenLabel"
        Me.payHdrPaylenLabel.Size = New System.Drawing.Size(115, 13)
        Me.payHdrPaylenLabel.TabIndex = 84
        Me.payHdrPaylenLabel.Text = "Payload Length (bytes)"
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
        'payHdrPaylenNumeric
        '
        Me.payHdrPaylenNumeric.Location = New System.Drawing.Point(286, 388)
        Me.payHdrPaylenNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.payHdrPaylenNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.payHdrPaylenNumeric.Name = "payHdrPaylenNumeric"
        Me.payHdrPaylenNumeric.Size = New System.Drawing.Size(120, 20)
        Me.payHdrPaylenNumeric.TabIndex = 83
        '
        'waveNameTextBox
        '
        Me.waveNameTextBox.Location = New System.Drawing.Point(286, 119)
        Me.waveNameTextBox.Name = "waveNameTextBox"
        Me.waveNameTextBox.Size = New System.Drawing.Size(120, 20)
        Me.waveNameTextBox.TabIndex = 87
        Me.waveNameTextBox.Text = "LETP"
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
        Me.scriptTextBox.Text = "script GenerateLEPkt" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  repeat forever" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    generate LETP" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  end repeat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "end scri" & _
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
        'payHdrPaylenModeComboBox
        '
        Me.payHdrPaylenModeComboBox.Location = New System.Drawing.Point(286, 345)
        Me.payHdrPaylenModeComboBox.Name = "payHdrPaylenModeComboBox"
        Me.payHdrPaylenModeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.payHdrPaylenModeComboBox.TabIndex = 81
        '
        'letpPayloadTypeLabel
        '
        Me.letpPayloadTypeLabel.AutoSize = True
        Me.letpPayloadTypeLabel.Location = New System.Drawing.Point(283, 241)
        Me.letpPayloadTypeLabel.Name = "letpPayloadTypeLabel"
        Me.letpPayloadTypeLabel.Size = New System.Drawing.Size(105, 13)
        Me.letpPayloadTypeLabel.TabIndex = 91
        Me.letpPayloadTypeLabel.Text = "LE-TP Payload Type"
        '
        'letpPayloadTypeComboBox
        '
        Me.letpPayloadTypeComboBox.FormattingEnabled = True
        Me.letpPayloadTypeComboBox.Location = New System.Drawing.Point(286, 257)
        Me.letpPayloadTypeComboBox.Name = "letpPayloadTypeComboBox"
        Me.letpPayloadTypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.letpPayloadTypeComboBox.TabIndex = 92
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
        'dirtyTxComboBox
        '
        Me.dirtyTxComboBox.FormattingEnabled = True
        Me.dirtyTxComboBox.Location = New System.Drawing.Point(286, 513)
        Me.dirtyTxComboBox.Name = "dirtyTxComboBox"
        Me.dirtyTxComboBox.Size = New System.Drawing.Size(120, 21)
        Me.dirtyTxComboBox.TabIndex = 95
        '
        'dirtyTxLabel
        '
        Me.dirtyTxLabel.AutoSize = True
        Me.dirtyTxLabel.Location = New System.Drawing.Point(283, 497)
        Me.dirtyTxLabel.Name = "dirtyTxLabel"
        Me.dirtyTxLabel.Size = New System.Drawing.Size(85, 13)
        Me.dirtyTxLabel.TabIndex = 96
        Me.dirtyTxLabel.Text = "Dirty Tx Enabled"
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
        Me.packetTypeLabel.Location = New System.Drawing.Point(283, 194)
        Me.packetTypeLabel.Name = "packetTypeLabel"
        Me.packetTypeLabel.Size = New System.Drawing.Size(68, 13)
        Me.packetTypeLabel.TabIndex = 88
        Me.packetTypeLabel.Text = "Packet Type"
        '
        'packetTypeComboBox
        '
        Me.packetTypeComboBox.FormattingEnabled = True
        Me.packetTypeComboBox.Location = New System.Drawing.Point(286, 215)
        Me.packetTypeComboBox.Name = "packetTypeComboBox"
        Me.packetTypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.packetTypeComboBox.TabIndex = 92
        '
        'letpCorruptAlternateCRCLabel
        '
        Me.letpCorruptAlternateCRCLabel.AutoSize = True
        Me.letpCorruptAlternateCRCLabel.Location = New System.Drawing.Point(283, 417)
        Me.letpCorruptAlternateCRCLabel.Name = "letpCorruptAlternateCRCLabel"
        Me.letpCorruptAlternateCRCLabel.Size = New System.Drawing.Size(144, 13)
        Me.letpCorruptAlternateCRCLabel.TabIndex = 88
        Me.letpCorruptAlternateCRCLabel.Text = "LE-TP Corrupt Alternate CRC"
        '
        'letpCorruptAlternateCRCComboBox
        '
        Me.letpCorruptAlternateCRCComboBox.FormattingEnabled = True
        Me.letpCorruptAlternateCRCComboBox.Location = New System.Drawing.Point(286, 432)
        Me.letpCorruptAlternateCRCComboBox.Name = "letpCorruptAlternateCRCComboBox"
        Me.letpCorruptAlternateCRCComboBox.Size = New System.Drawing.Size(120, 21)
        Me.letpCorruptAlternateCRCComboBox.TabIndex = 92
        '
        'NumOfUniquePacketsLabel
        '
        Me.NumOfUniquePacketsLabel.AutoSize = True
        Me.NumOfUniquePacketsLabel.Location = New System.Drawing.Point(283, 454)
        Me.NumOfUniquePacketsLabel.Name = "NumOfUniquePacketsLabel"
        Me.NumOfUniquePacketsLabel.Size = New System.Drawing.Size(135, 13)
        Me.NumOfUniquePacketsLabel.TabIndex = 88
        Me.NumOfUniquePacketsLabel.Text = "Number of Unique Packets"
        '
        'NumOfUniquePacketsNumeric
        '
        Me.NumOfUniquePacketsNumeric.Location = New System.Drawing.Point(286, 470)
        Me.NumOfUniquePacketsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.NumOfUniquePacketsNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.NumOfUniquePacketsNumeric.Name = "NumOfUniquePacketsNumeric"
        Me.NumOfUniquePacketsNumeric.Size = New System.Drawing.Size(120, 20)
        Me.NumOfUniquePacketsNumeric.TabIndex = 83
        Me.NumOfUniquePacketsNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'UserDefinedBitsLabel
        '
        Me.UserDefinedBitsLabel.AutoSize = True
        Me.UserDefinedBitsLabel.Location = New System.Drawing.Point(283, 283)
        Me.UserDefinedBitsLabel.Name = "UserDefinedBitsLabel"
        Me.UserDefinedBitsLabel.Size = New System.Drawing.Size(89, 13)
        Me.UserDefinedBitsLabel.TabIndex = 88
        Me.UserDefinedBitsLabel.Text = "User Defined Bits"
        '
        'antennaSwitchingEnabledLabel
        '
        Me.antennaSwitchingEnabledLabel.AutoSize = True
        Me.antennaSwitchingEnabledLabel.Location = New System.Drawing.Point(453, 219)
        Me.antennaSwitchingEnabledLabel.Name = "antennaSwitchingEnabledLabel"
        Me.antennaSwitchingEnabledLabel.Size = New System.Drawing.Size(138, 13)
        Me.antennaSwitchingEnabledLabel.TabIndex = 112
        Me.antennaSwitchingEnabledLabel.Text = "Antenna Switching Enabled"
        '
        'cteSlotDurationLabel
        '
        Me.cteSlotDurationLabel.AutoSize = True
        Me.cteSlotDurationLabel.Location = New System.Drawing.Point(453, 196)
        Me.cteSlotDurationLabel.Name = "cteSlotDurationLabel"
        Me.cteSlotDurationLabel.Size = New System.Drawing.Size(92, 13)
        Me.cteSlotDurationLabel.TabIndex = 113
        Me.cteSlotDurationLabel.Text = "CTE Slot Duration"
        '
        'antennaSwitchingEnabledComboBox
        '
        Me.antennaSwitchingEnabledComboBox.FormattingEnabled = True
        Me.antennaSwitchingEnabledComboBox.Location = New System.Drawing.Point(610, 214)
        Me.antennaSwitchingEnabledComboBox.Name = "antennaSwitchingEnabledComboBox"
        Me.antennaSwitchingEnabledComboBox.Size = New System.Drawing.Size(120, 21)
        Me.antennaSwitchingEnabledComboBox.TabIndex = 110
        '
        'cteSlotDurationComboBox
        '
        Me.cteSlotDurationComboBox.FormattingEnabled = True
        Me.cteSlotDurationComboBox.Location = New System.Drawing.Point(610, 193)
        Me.cteSlotDurationComboBox.Name = "cteSlotDurationComboBox"
        Me.cteSlotDurationComboBox.Size = New System.Drawing.Size(120, 21)
        Me.cteSlotDurationComboBox.TabIndex = 111
        '
        'directionFindingModeComboBox
        '
        Me.directionFindingModeComboBox.FormattingEnabled = True
        Me.directionFindingModeComboBox.Location = New System.Drawing.Point(610, 149)
        Me.directionFindingModeComboBox.Name = "directionFindingModeComboBox"
        Me.directionFindingModeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.directionFindingModeComboBox.TabIndex = 109
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(508, 133)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(103, 13)
        Me.Label3.TabIndex = 103
        Me.Label3.Text = "Direction Finding"
        '
        'directionFindingModeLabel
        '
        Me.directionFindingModeLabel.AutoSize = True
        Me.directionFindingModeLabel.Location = New System.Drawing.Point(455, 154)
        Me.directionFindingModeLabel.Name = "directionFindingModeLabel"
        Me.directionFindingModeLabel.Size = New System.Drawing.Size(116, 13)
        Me.directionFindingModeLabel.TabIndex = 108
        Me.directionFindingModeLabel.Text = "Direction Finding Mode"
        '
        'antennaSwitchingPatternLabel
        '
        Me.antennaSwitchingPatternLabel.AutoSize = True
        Me.antennaSwitchingPatternLabel.Location = New System.Drawing.Point(453, 260)
        Me.antennaSwitchingPatternLabel.Name = "antennaSwitchingPatternLabel"
        Me.antennaSwitchingPatternLabel.Size = New System.Drawing.Size(133, 13)
        Me.antennaSwitchingPatternLabel.TabIndex = 107
        Me.antennaSwitchingPatternLabel.Text = "Antenna Switching Pattern"
        '
        'numberOfAntennasLabel
        '
        Me.numberOfAntennasLabel.AutoSize = True
        Me.numberOfAntennasLabel.Location = New System.Drawing.Point(453, 238)
        Me.numberOfAntennasLabel.Name = "numberOfAntennasLabel"
        Me.numberOfAntennasLabel.Size = New System.Drawing.Size(104, 13)
        Me.numberOfAntennasLabel.TabIndex = 105
        Me.numberOfAntennasLabel.Text = "Number of Antennas"
        '
        'cteLengthLabel
        '
        Me.cteLengthLabel.AutoSize = True
        Me.cteLengthLabel.Location = New System.Drawing.Point(453, 177)
        Me.cteLengthLabel.Name = "cteLengthLabel"
        Me.cteLengthLabel.Size = New System.Drawing.Size(78, 13)
        Me.cteLengthLabel.TabIndex = 106
        Me.cteLengthLabel.Text = "CTE Length (s)"
        '
        'numberOfAntennasNumeric
        '
        Me.numberOfAntennasNumeric.Location = New System.Drawing.Point(610, 235)
        Me.numberOfAntennasNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numberOfAntennasNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.numberOfAntennasNumeric.Name = "numberOfAntennasNumeric"
        Me.numberOfAntennasNumeric.Size = New System.Drawing.Size(120, 20)
        Me.numberOfAntennasNumeric.TabIndex = 104
        Me.numberOfAntennasNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'cteLengthNumeric
        '
        Me.cteLengthNumeric.DecimalPlaces = 6
        Me.cteLengthNumeric.Location = New System.Drawing.Point(612, 172)
        Me.cteLengthNumeric.Name = "cteLengthNumeric"
        Me.cteLengthNumeric.Size = New System.Drawing.Size(118, 20)
        Me.cteLengthNumeric.TabIndex = 102
        Me.cteLengthNumeric.Value = New Decimal(New Integer() {16, 0, 0, 327680})
        '
        'antennaSwitchingPatternTextBox
        '
        Me.antennaSwitchingPatternTextBox.Location = New System.Drawing.Point(610, 255)
        Me.antennaSwitchingPatternTextBox.Name = "antennaSwitchingPatternTextBox"
        Me.antennaSwitchingPatternTextBox.Size = New System.Drawing.Size(120, 20)
        Me.antennaSwitchingPatternTextBox.TabIndex = 101
        Me.antennaSwitchingPatternTextBox.Text = "A0"
        '
        'insertRowButton
        '
        Me.insertRowButton.Location = New System.Drawing.Point(632, 457)
        Me.insertRowButton.Name = "insertRowButton"
        Me.insertRowButton.Size = New System.Drawing.Size(50, 22)
        Me.insertRowButton.TabIndex = 116
        Me.insertRowButton.Text = "Insert"
        Me.insertRowButton.UseVisualStyleBackColor = True
        '
        'deleteRowButton
        '
        Me.deleteRowButton.Location = New System.Drawing.Point(691, 457)
        Me.deleteRowButton.Name = "deleteRowButton"
        Me.deleteRowButton.Size = New System.Drawing.Size(50, 22)
        Me.deleteRowButton.TabIndex = 117
        Me.deleteRowButton.Text = "Delete"
        Me.deleteRowButton.UseVisualStyleBackColor = True
        '
        'dataGridView1
        '
        Me.dataGridView1.AllowUserToAddRows = False
        Me.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Index, Me.RelativePhase, Me.RelativeAmplitude})
        Me.dataGridView1.Location = New System.Drawing.Point(460, 350)
        Me.dataGridView1.Name = "dataGridView1"
        Me.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dataGridView1.Size = New System.Drawing.Size(286, 102)
        Me.dataGridView1.TabIndex = 118
        '
        'Index
        '
        Me.Index.Frozen = True
        Me.Index.HeaderText = "Index"
        Me.Index.Name = "Index"
        Me.Index.ReadOnly = True
        Me.Index.Width = 40
        '
        'RelativePhase
        '
        Me.RelativePhase.HeaderText = "Relative Phase (deg)"
        Me.RelativePhase.Name = "RelativePhase"
        '
        'RelativeAmplitude
        '
        Me.RelativeAmplitude.HeaderText = "Relative Amplitude (dB)"
        Me.RelativeAmplitude.Name = "RelativeAmplitude"
        '
        'userDefinedBitsTextBox
        '
        Me.userDefinedBitsTextBox.Location = New System.Drawing.Point(286, 302)
        Me.userDefinedBitsTextBox.Name = "userDefinedBitsTextBox"
        Me.userDefinedBitsTextBox.Size = New System.Drawing.Size(120, 20)
        Me.userDefinedBitsTextBox.TabIndex = 119
        Me.userDefinedBitsTextBox.Text = "0000"
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
        'AntennaSwitchingDurationLabel
        '
        Me.AntennaSwitchingDurationLabel.AutoSize = True
        Me.AntennaSwitchingDurationLabel.Location = New System.Drawing.Point(451, 283)
        Me.AntennaSwitchingDurationLabel.Name = "AntennaSwitchingDurationLabel"
        Me.AntennaSwitchingDurationLabel.Size = New System.Drawing.Size(139, 13)
        Me.AntennaSwitchingDurationLabel.TabIndex = 123
        Me.AntennaSwitchingDurationLabel.Text = "Antenna Switching Duration"
        '
        'AntennaSwitchingDurationNumeric
        '
        Me.AntennaSwitchingDurationNumeric.DecimalPlaces = 8
        Me.AntennaSwitchingDurationNumeric.Location = New System.Drawing.Point(610, 278)
        Me.AntennaSwitchingDurationNumeric.Name = "AntennaSwitchingDurationNumeric"
        Me.AntennaSwitchingDurationNumeric.Size = New System.Drawing.Size(118, 20)
        Me.AntennaSwitchingDurationNumeric.TabIndex = 122
        Me.AntennaSwitchingDurationNumeric.Value = New Decimal(New Integer() {5, 0, 0, 458752})
        '
        'AntennaSwitchingDurationUsedTextBox
        '
        Me.AntennaSwitchingDurationUsedTextBox.Enabled = False
        Me.AntennaSwitchingDurationUsedTextBox.Location = New System.Drawing.Point(610, 313)
        Me.AntennaSwitchingDurationUsedTextBox.Name = "AntennaSwitchingDurationUsedTextBox"
        Me.AntennaSwitchingDurationUsedTextBox.Size = New System.Drawing.Size(118, 20)
        Me.AntennaSwitchingDurationUsedTextBox.TabIndex = 125
        Me.AntennaSwitchingDurationUsedTextBox.Text = "0.00"
        '
        'AntennaSwitchingDurationUsedLabel
        '
        Me.AntennaSwitchingDurationUsedLabel.AutoSize = True
        Me.AntennaSwitchingDurationUsedLabel.Location = New System.Drawing.Point(442, 316)
        Me.AntennaSwitchingDurationUsedLabel.Name = "AntennaSwitchingDurationUsedLabel"
        Me.AntennaSwitchingDurationUsedLabel.Size = New System.Drawing.Size(167, 13)
        Me.AntennaSwitchingDurationUsedLabel.TabIndex = 124
        Me.AntennaSwitchingDurationUsedLabel.Text = "Antenna Switching Duration Used"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(753, 625)
        Me.Controls.Add(Me.AntennaSwitchingDurationUsedTextBox)
        Me.Controls.Add(Me.AntennaSwitchingDurationUsedLabel)
        Me.Controls.Add(Me.AntennaSwitchingDurationLabel)
        Me.Controls.Add(Me.AntennaSwitchingDurationNumeric)
        Me.Controls.Add(Me.OversamplingFactorLabel)
        Me.Controls.Add(Me.OversamplingFactorNumeric)
        Me.Controls.Add(Me.userDefinedBitsTextBox)
        Me.Controls.Add(Me.dataGridView1)
        Me.Controls.Add(Me.insertRowButton)
        Me.Controls.Add(Me.deleteRowButton)
        Me.Controls.Add(Me.antennaSwitchingEnabledLabel)
        Me.Controls.Add(Me.cteSlotDurationLabel)
        Me.Controls.Add(Me.antennaSwitchingEnabledComboBox)
        Me.Controls.Add(Me.cteSlotDurationComboBox)
        Me.Controls.Add(Me.directionFindingModeComboBox)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.directionFindingModeLabel)
        Me.Controls.Add(Me.antennaSwitchingPatternLabel)
        Me.Controls.Add(Me.numberOfAntennasLabel)
        Me.Controls.Add(Me.cteLengthLabel)
        Me.Controls.Add(Me.numberOfAntennasNumeric)
        Me.Controls.Add(Me.cteLengthNumeric)
        Me.Controls.Add(Me.antennaSwitchingPatternTextBox)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.terminalConfigurationComboBox)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.outputPortComboBox)
        Me.Controls.Add(Me.dirtyTxLabel)
        Me.Controls.Add(Me.dirtyTxComboBox)
        Me.Controls.Add(Me.generateButton)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.letpCorruptAlternateCRCComboBox)
        Me.Controls.Add(Me.packetTypeComboBox)
        Me.Controls.Add(Me.letpPayloadTypeComboBox)
        Me.Controls.Add(Me.letpPayloadTypeLabel)
        Me.Controls.Add(Me.payHdrPaylenModeLabel)
        Me.Controls.Add(Me.payHdrPaylenLabel)
        Me.Controls.Add(Me.letpCorruptAlternateCRCLabel)
        Me.Controls.Add(Me.NumOfUniquePacketsLabel)
        Me.Controls.Add(Me.UserDefinedBitsLabel)
        Me.Controls.Add(Me.packetTypeLabel)
        Me.Controls.Add(Me.waveNameLabel)
        Me.Controls.Add(Me.scriptLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.NumOfUniquePacketsNumeric)
        Me.Controls.Add(Me.payHdrPaylenNumeric)
        Me.Controls.Add(Me.waveNameTextBox)
        Me.Controls.Add(Me.scriptTextBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.payHdrPaylenModeComboBox)
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
        CType(Me.payHdrPaylenNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumOfUniquePacketsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberOfAntennasNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cteLengthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OversamplingFactorNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AntennaSwitchingDurationNumeric, System.ComponentModel.ISupportInitialize).EndInit()
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
    Private payHdrPaylenModeLabel As System.Windows.Forms.Label
    Private payHdrPaylenLabel As System.Windows.Forms.Label
    Private waveNameLabel As System.Windows.Forms.Label
    Private scriptLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private payHdrPaylenNumeric As System.Windows.Forms.NumericUpDown
    Private waveNameTextBox As System.Windows.Forms.TextBox
    Private scriptTextBox As System.Windows.Forms.TextBox
    Private errorTextBox As System.Windows.Forms.TextBox
    Private payHdrPaylenModeComboBox As System.Windows.Forms.ComboBox
    Private letpPayloadTypeLabel As System.Windows.Forms.Label
    Private letpPayloadTypeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents generateButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private dirtyTxComboBox As System.Windows.Forms.ComboBox
    Private dirtyTxLabel As System.Windows.Forms.Label
    Private label2 As System.Windows.Forms.Label
    Private terminalConfigurationComboBox As System.Windows.Forms.ComboBox
    Private label1 As System.Windows.Forms.Label
    Private outputPortComboBox As System.Windows.Forms.ComboBox
    Private WithEvents clkTerminalLabel As System.Windows.Forms.Label
    Private WithEvents packetTypeLabel As System.Windows.Forms.Label
    Private WithEvents packetTypeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents letpCorruptAlternateCRCLabel As System.Windows.Forms.Label
    Private WithEvents letpCorruptAlternateCRCComboBox As System.Windows.Forms.ComboBox
    Private WithEvents NumOfUniquePacketsLabel As System.Windows.Forms.Label
    Private WithEvents NumOfUniquePacketsNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents UserDefinedBitsLabel As System.Windows.Forms.Label
    Private WithEvents antennaSwitchingEnabledLabel As System.Windows.Forms.Label
    Private WithEvents cteSlotDurationLabel As System.Windows.Forms.Label
    Private WithEvents antennaSwitchingEnabledComboBox As System.Windows.Forms.ComboBox
    Private WithEvents cteSlotDurationComboBox As System.Windows.Forms.ComboBox
    Private WithEvents directionFindingModeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents Label3 As System.Windows.Forms.Label
    Private WithEvents directionFindingModeLabel As System.Windows.Forms.Label
    Private WithEvents antennaSwitchingPatternLabel As System.Windows.Forms.Label
    Private WithEvents numberOfAntennasLabel As System.Windows.Forms.Label
    Private WithEvents cteLengthLabel As System.Windows.Forms.Label
    Private WithEvents numberOfAntennasNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents cteLengthNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents antennaSwitchingPatternTextBox As System.Windows.Forms.TextBox
    Private WithEvents insertRowButton As System.Windows.Forms.Button
    Private WithEvents deleteRowButton As System.Windows.Forms.Button
    Private WithEvents dataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Index As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents RelativePhase As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents RelativeAmplitude As System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents userDefinedBitsTextBox As System.Windows.Forms.TextBox
    Private WithEvents OversamplingFactorLabel As System.Windows.Forms.Label
    Private WithEvents OversamplingFactorNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents AntennaSwitchingDurationLabel As System.Windows.Forms.Label
    Private WithEvents AntennaSwitchingDurationNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents AntennaSwitchingDurationUsedTextBox As System.Windows.Forms.TextBox
    Private WithEvents AntennaSwitchingDurationUsedLabel As System.Windows.Forms.Label
End Class

