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
		Me.autoheadroomEnabLabel = New System.Windows.Forms.Label()
		Me.headroomLabel = New System.Windows.Forms.Label()
		Me.actualHeadroomLabel = New System.Windows.Forms.Label()
		Me.refSourceLabel = New System.Windows.Forms.Label()
		Me.clkOutputTerminalLabel = New System.Windows.Forms.Label()
		Me.allIqImpairEnLabel = New System.Windows.Forms.Label()
		Me.quadratureSkewLabel = New System.Windows.Forms.Label()
		Me.iDcOffsetLabel = New System.Windows.Forms.Label()
		Me.qDcOffsetLabel = New System.Windows.Forms.Label()
		Me.iqGaimbalanceLabel = New System.Windows.Forms.Label()
		Me.carrierFreqOffLabel = New System.Windows.Forms.Label()
		Me.awgnEnabledLabel = New System.Windows.Forms.Label()
		Me.cnrLabel = New System.Windows.Forms.Label()
		Me.hardwareLabel = New System.Windows.Forms.Label()
		Me.frequencySettingsLabel = New System.Windows.Forms.Label()
		Me.impairmentsLabel = New System.Windows.Forms.Label()
		Me.chnNumberNumeric = New System.Windows.Forms.NumericUpDown()
		Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
		Me.externalAttnNumeric = New System.Windows.Forms.NumericUpDown()
		Me.headroomNumeric = New System.Windows.Forms.NumericUpDown()
		Me.quadratureSkewNumeric = New System.Windows.Forms.NumericUpDown()
		Me.iDcOffsetNumeric = New System.Windows.Forms.NumericUpDown()
		Me.qDcOffsetNumeric = New System.Windows.Forms.NumericUpDown()
		Me.iqGaimbalanceNumeric = New System.Windows.Forms.NumericUpDown()
		Me.cnrNumeric = New System.Windows.Forms.NumericUpDown()
		Me.rfsgResourceTextBox = New System.Windows.Forms.TextBox()
		Me.carrierFreqTextBox = New System.Windows.Forms.TextBox()
		Me.actualHeadroomTextBox = New System.Windows.Forms.TextBox()
		Me.autoheadroomEnabComboBox = New System.Windows.Forms.ComboBox()
		Me.refSourceComboBox = New System.Windows.Forms.ComboBox()
		Me.clkOutTerminalComboBox = New System.Windows.Forms.ComboBox()
		Me.allIqImpairEnComboBox = New System.Windows.Forms.ComboBox()
		Me.awgnEnabledComboBox = New System.Windows.Forms.ComboBox()
		Me.timer = New System.Windows.Forms.Timer(Me.components)
		Me.carrierFreqOffNumeric = New System.Windows.Forms.NumericUpDown()
		Me.payHdrPaylenModeLabel = New System.Windows.Forms.Label()
		Me.payHdrPaylenLabel = New System.Windows.Forms.Label()
		Me.waveNameLabel = New System.Windows.Forms.Label()
		Me.scriptLabel = New System.Windows.Forms.Label()
		Me.errorLabel = New System.Windows.Forms.Label()
		Me.payHdrPaylenNumeric = New System.Windows.Forms.NumericUpDown()
		Me.waveNameTextBox = New System.Windows.Forms.TextBox()
		Me.scriptTextBox = New System.Windows.Forms.TextBox()
		Me.errorTextBox = New System.Windows.Forms.TextBox()
		Me.payHdrPaylenModeComboBox = New System.Windows.Forms.ComboBox()
		Me.generateButton = New System.Windows.Forms.Button()
		Me.stopButton = New System.Windows.Forms.Button()
		Me.dirtyTxComboBox = New System.Windows.Forms.ComboBox()
		Me.dirtyTxLabel = New System.Windows.Forms.Label()
		Me.label2 = New System.Windows.Forms.Label()
		Me.terminalConfigurationComboBox = New System.Windows.Forms.ComboBox()
		Me.label1 = New System.Windows.Forms.Label()
		Me.outputPortComboBox = New System.Windows.Forms.ComboBox()
		Me.clkTerminalLabel = New System.Windows.Forms.Label()
		Me.NumOfUniquePacketsLabel = New System.Windows.Forms.Label()
		Me.NumOfUniquePacketsNumeric = New System.Windows.Forms.NumericUpDown()
		Me.OversamplingFactorLabel = New System.Windows.Forms.Label()
		Me.OversamplingFactorNumeric = New System.Windows.Forms.NumericUpDown()
		Me.dataRateLabel = New System.Windows.Forms.Label()
		Me.dataRateNumeric = New System.Windows.Forms.NumericUpDown()
		Me.zadoffChuIndexLabel = New System.Windows.Forms.Label()
		Me.zadoffChuIndexNumeric = New System.Windows.Forms.NumericUpDown()
		Me.physicalChannelAddressLabel = New System.Windows.Forms.Label()
		Me.physicalChannelAddressNumeric = New System.Windows.Forms.NumericUpDown()
		Me.HdtPacketFormatLabel = New System.Windows.Forms.Label()
		Me.HdtPacketFormatComboBox = New System.Windows.Forms.ComboBox()
		Me.HdtPhyIntervalLabel = New System.Windows.Forms.Label()
		Me.HdtPhyIntervalNumeric = New System.Windows.Forms.NumericUpDown()
		Me.Label3 = New System.Windows.Forms.Label()
		CType(Me.chnNumberNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.powerLevelNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.externalAttnNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.headroomNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.quadratureSkewNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.iDcOffsetNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.qDcOffsetNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.iqGaimbalanceNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.cnrNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.carrierFreqOffNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.payHdrPaylenNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.NumOfUniquePacketsNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.OversamplingFactorNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.dataRateNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.zadoffChuIndexNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.physicalChannelAddressNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.HdtPhyIntervalNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'rfsgResourceLabel
		'
		Me.rfsgResourceLabel.AutoSize = true
		Me.rfsgResourceLabel.Location = New System.Drawing.Point(21, 76)
		Me.rfsgResourceLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.rfsgResourceLabel.Name = "rfsgResourceLabel"
		Me.rfsgResourceLabel.Size = New System.Drawing.Size(220, 32)
		Me.rfsgResourceLabel.TabIndex = 43
		Me.rfsgResourceLabel.Text = "RFSG Resource"
		'
		'chnNumberLabel
		'
		Me.chnNumberLabel.AutoSize = true
		Me.chnNumberLabel.Location = New System.Drawing.Point(21, 207)
		Me.chnNumberLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.chnNumberLabel.Name = "chnNumberLabel"
		Me.chnNumberLabel.Size = New System.Drawing.Size(228, 32)
		Me.chnNumberLabel.TabIndex = 44
		Me.chnNumberLabel.Text = "Channel Number"
		'
		'carrierFreqLabel
		'
		Me.carrierFreqLabel.AutoSize = true
		Me.carrierFreqLabel.Location = New System.Drawing.Point(21, 255)
		Me.carrierFreqLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.carrierFreqLabel.Name = "carrierFreqLabel"
		Me.carrierFreqLabel.Size = New System.Drawing.Size(300, 32)
		Me.carrierFreqLabel.TabIndex = 47
		Me.carrierFreqLabel.Text = "Carrier Frequency (Hz)"
		'
		'powerLevelLabel
		'
		Me.powerLevelLabel.AutoSize = true
		Me.powerLevelLabel.Location = New System.Drawing.Point(21, 401)
		Me.powerLevelLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.powerLevelLabel.Name = "powerLevelLabel"
		Me.powerLevelLabel.Size = New System.Drawing.Size(253, 32)
		Me.powerLevelLabel.TabIndex = 48
		Me.powerLevelLabel.Text = "Power Level (dBm)"
		'
		'externalAttnLabel
		'
		Me.externalAttnLabel.AutoSize = true
		Me.externalAttnLabel.Location = New System.Drawing.Point(21, 453)
		Me.externalAttnLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.externalAttnLabel.Name = "externalAttnLabel"
		Me.externalAttnLabel.Size = New System.Drawing.Size(332, 32)
		Me.externalAttnLabel.TabIndex = 51
		Me.externalAttnLabel.Text = "External Attenuation (dB)"
		'
		'autoheadroomEnabLabel
		'
		Me.autoheadroomEnabLabel.AutoSize = true
		Me.autoheadroomEnabLabel.Location = New System.Drawing.Point(21, 506)
		Me.autoheadroomEnabLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.autoheadroomEnabLabel.Name = "autoheadroomEnabLabel"
		Me.autoheadroomEnabLabel.Size = New System.Drawing.Size(325, 32)
		Me.autoheadroomEnabLabel.TabIndex = 52
		Me.autoheadroomEnabLabel.Text = "Auto Headroom Enabled"
		'
		'headroomLabel
		'
		Me.headroomLabel.AutoSize = true
		Me.headroomLabel.Location = New System.Drawing.Point(21, 556)
		Me.headroomLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.headroomLabel.Name = "headroomLabel"
		Me.headroomLabel.Size = New System.Drawing.Size(206, 32)
		Me.headroomLabel.TabIndex = 54
		Me.headroomLabel.Text = "Headroom (dB)"
		'
		'actualHeadroomLabel
		'
		Me.actualHeadroomLabel.AutoSize = true
		Me.actualHeadroomLabel.Location = New System.Drawing.Point(21, 601)
		Me.actualHeadroomLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.actualHeadroomLabel.Name = "actualHeadroomLabel"
		Me.actualHeadroomLabel.Size = New System.Drawing.Size(293, 32)
		Me.actualHeadroomLabel.TabIndex = 56
		Me.actualHeadroomLabel.Text = "Actual Headroom (dB)"
		'
		'refSourceLabel
		'
		Me.refSourceLabel.AutoSize = true
		Me.refSourceLabel.Location = New System.Drawing.Point(21, 744)
		Me.refSourceLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.refSourceLabel.Name = "refSourceLabel"
		Me.refSourceLabel.Size = New System.Drawing.Size(242, 32)
		Me.refSourceLabel.TabIndex = 59
		Me.refSourceLabel.Text = "Reference Source"
		'
		'clkOutputTerminalLabel
		'
		Me.clkOutputTerminalLabel.AutoSize = true
		Me.clkOutputTerminalLabel.Location = New System.Drawing.Point(21, 873)
		Me.clkOutputTerminalLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.clkOutputTerminalLabel.Name = "clkOutputTerminalLabel"
		Me.clkOutputTerminalLabel.Size = New System.Drawing.Size(266, 32)
		Me.clkOutputTerminalLabel.TabIndex = 60
		Me.clkOutputTerminalLabel.Text = "Clk Output Terminal"
		'
		'allIqImpairEnLabel
		'
		Me.allIqImpairEnLabel.AutoSize = true
		Me.allIqImpairEnLabel.Location = New System.Drawing.Point(21, 1030)
		Me.allIqImpairEnLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.allIqImpairEnLabel.Name = "allIqImpairEnLabel"
		Me.allIqImpairEnLabel.Size = New System.Drawing.Size(358, 32)
		Me.allIqImpairEnLabel.TabIndex = 62
		Me.allIqImpairEnLabel.Text = "All IQ Impairments Enabled"
		'
		'quadratureSkewLabel
		'
		Me.quadratureSkewLabel.AutoSize = true
		Me.quadratureSkewLabel.Location = New System.Drawing.Point(21, 1083)
		Me.quadratureSkewLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.quadratureSkewLabel.Name = "quadratureSkewLabel"
		Me.quadratureSkewLabel.Size = New System.Drawing.Size(307, 32)
		Me.quadratureSkewLabel.TabIndex = 64
		Me.quadratureSkewLabel.Text = "Quadrature Skew (deg)"
		'
		'iDcOffsetLabel
		'
		Me.iDcOffsetLabel.AutoSize = true
		Me.iDcOffsetLabel.Location = New System.Drawing.Point(21, 1135)
		Me.iDcOffsetLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.iDcOffsetLabel.Name = "iDcOffsetLabel"
		Me.iDcOffsetLabel.Size = New System.Drawing.Size(201, 32)
		Me.iDcOffsetLabel.TabIndex = 67
		Me.iDcOffsetLabel.Text = "I DC Offset (%)"
		'
		'qDcOffsetLabel
		'
		Me.qDcOffsetLabel.AutoSize = true
		Me.qDcOffsetLabel.Location = New System.Drawing.Point(21, 1188)
		Me.qDcOffsetLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.qDcOffsetLabel.Name = "qDcOffsetLabel"
		Me.qDcOffsetLabel.Size = New System.Drawing.Size(216, 32)
		Me.qDcOffsetLabel.TabIndex = 69
		Me.qDcOffsetLabel.Text = "Q DC Offset (%)"
		'
		'iqGaimbalanceLabel
		'
		Me.iqGaimbalanceLabel.AutoSize = true
		Me.iqGaimbalanceLabel.Location = New System.Drawing.Point(21, 1240)
		Me.iqGaimbalanceLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.iqGaimbalanceLabel.Name = "iqGaimbalanceLabel"
		Me.iqGaimbalanceLabel.Size = New System.Drawing.Size(309, 32)
		Me.iqGaimbalanceLabel.TabIndex = 70
		Me.iqGaimbalanceLabel.Text = "IQ Gain Imbalance (dB)"
		'
		'carrierFreqOffLabel
		'
		Me.carrierFreqOffLabel.AutoSize = true
		Me.carrierFreqOffLabel.Location = New System.Drawing.Point(21, 1288)
		Me.carrierFreqOffLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.carrierFreqOffLabel.Name = "carrierFreqOffLabel"
		Me.carrierFreqOffLabel.Size = New System.Drawing.Size(383, 32)
		Me.carrierFreqOffLabel.TabIndex = 72
		Me.carrierFreqOffLabel.Text = "Carrier Frequency Offset (Hz)"
		'
		'awgnEnabledLabel
		'
		Me.awgnEnabledLabel.AutoSize = true
		Me.awgnEnabledLabel.Location = New System.Drawing.Point(21, 1338)
		Me.awgnEnabledLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.awgnEnabledLabel.Name = "awgnEnabledLabel"
		Me.awgnEnabledLabel.Size = New System.Drawing.Size(214, 32)
		Me.awgnEnabledLabel.TabIndex = 73
		Me.awgnEnabledLabel.Text = "AWGN Enabled"
		'
		'cnrLabel
		'
		Me.cnrLabel.AutoSize = true
		Me.cnrLabel.Location = New System.Drawing.Point(21, 1385)
		Me.cnrLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.cnrLabel.Name = "cnrLabel"
		Me.cnrLabel.Size = New System.Drawing.Size(345, 32)
		Me.cnrLabel.TabIndex = 75
		Me.cnrLabel.Text = "Carrier to Noise Ratio (dB)"
		'
		'hardwareLabel
		'
		Me.hardwareLabel.AutoSize = true
		Me.hardwareLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.hardwareLabel.Location = New System.Drawing.Point(56, 21)
		Me.hardwareLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.hardwareLabel.Name = "hardwareLabel"
		Me.hardwareLabel.Size = New System.Drawing.Size(144, 32)
		Me.hardwareLabel.TabIndex = 77
		Me.hardwareLabel.Text = "Hardware"
		'
		'frequencySettingsLabel
		'
		Me.frequencySettingsLabel.AutoSize = true
		Me.frequencySettingsLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.frequencySettingsLabel.Location = New System.Drawing.Point(56, 701)
		Me.frequencySettingsLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.frequencySettingsLabel.Name = "frequencySettingsLabel"
		Me.frequencySettingsLabel.Size = New System.Drawing.Size(277, 32)
		Me.frequencySettingsLabel.TabIndex = 78
		Me.frequencySettingsLabel.Text = "Frequency Settings"
		'
		'impairmentsLabel
		'
		Me.impairmentsLabel.AutoSize = true
		Me.impairmentsLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.impairmentsLabel.Location = New System.Drawing.Point(56, 980)
		Me.impairmentsLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.impairmentsLabel.Name = "impairmentsLabel"
		Me.impairmentsLabel.Size = New System.Drawing.Size(180, 32)
		Me.impairmentsLabel.TabIndex = 79
		Me.impairmentsLabel.Text = "Impairments"
		'
		'chnNumberNumeric
		'
		Me.chnNumberNumeric.Location = New System.Drawing.Point(408, 196)
		Me.chnNumberNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.chnNumberNumeric.Maximum = New Decimal(New Integer() {39, 0, 0, 0})
		Me.chnNumberNumeric.Name = "chnNumberNumeric"
		Me.chnNumberNumeric.Size = New System.Drawing.Size(240, 38)
		Me.chnNumberNumeric.TabIndex = 45
		Me.chnNumberNumeric.Value = New Decimal(New Integer() {3, 0, 0, 0})
		'
		'powerLevelNumeric
		'
		Me.powerLevelNumeric.Location = New System.Drawing.Point(408, 393)
		Me.powerLevelNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {25, 0, 0, 0})
		Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {179, 0, 0, -2147483648})
		Me.powerLevelNumeric.Name = "powerLevelNumeric"
		Me.powerLevelNumeric.Size = New System.Drawing.Size(240, 38)
		Me.powerLevelNumeric.TabIndex = 49
		'
		'externalAttnNumeric
		'
		Me.externalAttnNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.externalAttnNumeric.Location = New System.Drawing.Point(408, 446)
		Me.externalAttnNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.externalAttnNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.externalAttnNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.externalAttnNumeric.Name = "externalAttnNumeric"
		Me.externalAttnNumeric.Size = New System.Drawing.Size(240, 38)
		Me.externalAttnNumeric.TabIndex = 50
		'
		'headroomNumeric
		'
		Me.headroomNumeric.Location = New System.Drawing.Point(408, 548)
		Me.headroomNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.headroomNumeric.Name = "headroomNumeric"
		Me.headroomNumeric.Size = New System.Drawing.Size(240, 38)
		Me.headroomNumeric.TabIndex = 55
		'
		'quadratureSkewNumeric
		'
		Me.quadratureSkewNumeric.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
		Me.quadratureSkewNumeric.Location = New System.Drawing.Point(408, 1080)
		Me.quadratureSkewNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.quadratureSkewNumeric.Name = "quadratureSkewNumeric"
		Me.quadratureSkewNumeric.Size = New System.Drawing.Size(240, 38)
		Me.quadratureSkewNumeric.TabIndex = 65
		'
		'iDcOffsetNumeric
		'
		Me.iDcOffsetNumeric.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
		Me.iDcOffsetNumeric.Location = New System.Drawing.Point(408, 1133)
		Me.iDcOffsetNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.iDcOffsetNumeric.Name = "iDcOffsetNumeric"
		Me.iDcOffsetNumeric.Size = New System.Drawing.Size(240, 38)
		Me.iDcOffsetNumeric.TabIndex = 66
		'
		'qDcOffsetNumeric
		'
		Me.qDcOffsetNumeric.Increment = New Decimal(New Integer() {50, 0, 0, 131072})
		Me.qDcOffsetNumeric.Location = New System.Drawing.Point(408, 1185)
		Me.qDcOffsetNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.qDcOffsetNumeric.Name = "qDcOffsetNumeric"
		Me.qDcOffsetNumeric.Size = New System.Drawing.Size(240, 38)
		Me.qDcOffsetNumeric.TabIndex = 68
		'
		'iqGaimbalanceNumeric
		'
		Me.iqGaimbalanceNumeric.Increment = New Decimal(New Integer() {50, 0, 0, 131072})
		Me.iqGaimbalanceNumeric.Location = New System.Drawing.Point(408, 1238)
		Me.iqGaimbalanceNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.iqGaimbalanceNumeric.Name = "iqGaimbalanceNumeric"
		Me.iqGaimbalanceNumeric.Size = New System.Drawing.Size(240, 38)
		Me.iqGaimbalanceNumeric.TabIndex = 71
		'
		'cnrNumeric
		'
		Me.cnrNumeric.Increment = New Decimal(New Integer() {50, 0, 0, 131072})
		Me.cnrNumeric.Location = New System.Drawing.Point(408, 1383)
		Me.cnrNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.cnrNumeric.Name = "cnrNumeric"
		Me.cnrNumeric.Size = New System.Drawing.Size(240, 38)
		Me.cnrNumeric.TabIndex = 76
		Me.cnrNumeric.Value = New Decimal(New Integer() {50, 0, 0, 0})
		'
		'rfsgResourceTextBox
		'
		Me.rfsgResourceTextBox.Location = New System.Drawing.Point(408, 60)
		Me.rfsgResourceTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.rfsgResourceTextBox.Name = "rfsgResourceTextBox"
		Me.rfsgResourceTextBox.Size = New System.Drawing.Size(233, 38)
		Me.rfsgResourceTextBox.TabIndex = 42
		Me.rfsgResourceTextBox.Text = "RFSG"
		'
		'carrierFreqTextBox
		'
		Me.carrierFreqTextBox.Enabled = false
		Me.carrierFreqTextBox.Location = New System.Drawing.Point(408, 243)
		Me.carrierFreqTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.carrierFreqTextBox.Name = "carrierFreqTextBox"
		Me.carrierFreqTextBox.Size = New System.Drawing.Size(233, 38)
		Me.carrierFreqTextBox.TabIndex = 46
		Me.carrierFreqTextBox.Text = "2.408E+9"
		'
		'actualHeadroomTextBox
		'
		Me.actualHeadroomTextBox.Enabled = false
		Me.actualHeadroomTextBox.Location = New System.Drawing.Point(408, 594)
		Me.actualHeadroomTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.actualHeadroomTextBox.Name = "actualHeadroomTextBox"
		Me.actualHeadroomTextBox.Size = New System.Drawing.Size(233, 38)
		Me.actualHeadroomTextBox.TabIndex = 57
		Me.actualHeadroomTextBox.Text = "0.00"
		'
		'autoheadroomEnabComboBox
		'
		Me.autoheadroomEnabComboBox.Location = New System.Drawing.Point(408, 498)
		Me.autoheadroomEnabComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.autoheadroomEnabComboBox.Name = "autoheadroomEnabComboBox"
		Me.autoheadroomEnabComboBox.Size = New System.Drawing.Size(233, 39)
		Me.autoheadroomEnabComboBox.TabIndex = 53
		'
		'refSourceComboBox
		'
		Me.refSourceComboBox.Location = New System.Drawing.Point(408, 744)
		Me.refSourceComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.refSourceComboBox.Name = "refSourceComboBox"
		Me.refSourceComboBox.Size = New System.Drawing.Size(233, 39)
		Me.refSourceComboBox.TabIndex = 58
		'
		'clkOutTerminalComboBox
		'
		Me.clkOutTerminalComboBox.Location = New System.Drawing.Point(408, 873)
		Me.clkOutTerminalComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.clkOutTerminalComboBox.Name = "clkOutTerminalComboBox"
		Me.clkOutTerminalComboBox.Size = New System.Drawing.Size(233, 39)
		Me.clkOutTerminalComboBox.TabIndex = 61
		'
		'allIqImpairEnComboBox
		'
		Me.allIqImpairEnComboBox.Location = New System.Drawing.Point(408, 1028)
		Me.allIqImpairEnComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.allIqImpairEnComboBox.Name = "allIqImpairEnComboBox"
		Me.allIqImpairEnComboBox.Size = New System.Drawing.Size(233, 39)
		Me.allIqImpairEnComboBox.TabIndex = 63
		'
		'awgnEnabledComboBox
		'
		Me.awgnEnabledComboBox.Location = New System.Drawing.Point(408, 1335)
		Me.awgnEnabledComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.awgnEnabledComboBox.Name = "awgnEnabledComboBox"
		Me.awgnEnabledComboBox.Size = New System.Drawing.Size(233, 39)
		Me.awgnEnabledComboBox.TabIndex = 74
		'
		'timer
		'
		'
		'carrierFreqOffNumeric
		'
		Me.carrierFreqOffNumeric.Location = New System.Drawing.Point(408, 1283)
		Me.carrierFreqOffNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.carrierFreqOffNumeric.Name = "carrierFreqOffNumeric"
		Me.carrierFreqOffNumeric.Size = New System.Drawing.Size(240, 38)
		Me.carrierFreqOffNumeric.TabIndex = 80
		'
		'payHdrPaylenModeLabel
		'
		Me.payHdrPaylenModeLabel.AutoSize = true
		Me.payHdrPaylenModeLabel.Location = New System.Drawing.Point(755, 596)
		Me.payHdrPaylenModeLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.payHdrPaylenModeLabel.Name = "payHdrPaylenModeLabel"
		Me.payHdrPaylenModeLabel.Size = New System.Drawing.Size(291, 32)
		Me.payHdrPaylenModeLabel.TabIndex = 82
		Me.payHdrPaylenModeLabel.Text = "Payload Length Mode"
		'
		'payHdrPaylenLabel
		'
		Me.payHdrPaylenLabel.AutoSize = true
		Me.payHdrPaylenLabel.Location = New System.Drawing.Point(755, 698)
		Me.payHdrPaylenLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.payHdrPaylenLabel.Name = "payHdrPaylenLabel"
		Me.payHdrPaylenLabel.Size = New System.Drawing.Size(306, 32)
		Me.payHdrPaylenLabel.TabIndex = 84
		Me.payHdrPaylenLabel.Text = "Payload Length (bytes)"
		'
		'waveNameLabel
		'
		Me.waveNameLabel.AutoSize = true
		Me.waveNameLabel.Location = New System.Drawing.Point(755, 241)
		Me.waveNameLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.waveNameLabel.Name = "waveNameLabel"
		Me.waveNameLabel.Size = New System.Drawing.Size(224, 32)
		Me.waveNameLabel.TabIndex = 88
		Me.waveNameLabel.Text = "Waveform Name"
		'
		'scriptLabel
		'
		Me.scriptLabel.AutoSize = true
		Me.scriptLabel.Location = New System.Drawing.Point(1213, 29)
		Me.scriptLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.scriptLabel.Name = "scriptLabel"
		Me.scriptLabel.Size = New System.Drawing.Size(87, 32)
		Me.scriptLabel.TabIndex = 85
		Me.scriptLabel.Text = "Script"
		'
		'errorLabel
		'
		Me.errorLabel.AutoSize = true
		Me.errorLabel.Location = New System.Drawing.Point(1265, 1108)
		Me.errorLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.errorLabel.Name = "errorLabel"
		Me.errorLabel.Size = New System.Drawing.Size(76, 32)
		Me.errorLabel.TabIndex = 89
		Me.errorLabel.Text = "Error"
		'
		'payHdrPaylenNumeric
		'
		Me.payHdrPaylenNumeric.Location = New System.Drawing.Point(763, 736)
		Me.payHdrPaylenNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.payHdrPaylenNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.payHdrPaylenNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.payHdrPaylenNumeric.Name = "payHdrPaylenNumeric"
		Me.payHdrPaylenNumeric.Size = New System.Drawing.Size(320, 38)
		Me.payHdrPaylenNumeric.TabIndex = 83
		'
		'waveNameTextBox
		'
		Me.waveNameTextBox.Location = New System.Drawing.Point(763, 284)
		Me.waveNameTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.waveNameTextBox.Name = "waveNameTextBox"
		Me.waveNameTextBox.Size = New System.Drawing.Size(313, 38)
		Me.waveNameTextBox.TabIndex = 87
		Me.waveNameTextBox.Text = "LETP"
		'
		'scriptTextBox
		'
		Me.scriptTextBox.Location = New System.Drawing.Point(1227, 69)
		Me.scriptTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.scriptTextBox.Multiline = true
		Me.scriptTextBox.Name = "scriptTextBox"
		Me.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.scriptTextBox.Size = New System.Drawing.Size(399, 197)
		Me.scriptTextBox.TabIndex = 86
		Me.scriptTextBox.TabStop = false
		Me.scriptTextBox.Text = "script GenerateLEPkt"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"  repeat forever"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"    generate LETP"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"  end repeat"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"end scri"& _ 
    "pt"
		'
		'errorTextBox
		'
		Me.errorTextBox.Location = New System.Drawing.Point(1265, 1158)
		Me.errorTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.errorTextBox.Multiline = true
		Me.errorTextBox.Name = "errorTextBox"
		Me.errorTextBox.ReadOnly = true
		Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.errorTextBox.Size = New System.Drawing.Size(505, 271)
		Me.errorTextBox.TabIndex = 90
		Me.errorTextBox.TabStop = false
		Me.errorTextBox.Text = "No Error"
		'
		'payHdrPaylenModeComboBox
		'
		Me.payHdrPaylenModeComboBox.Location = New System.Drawing.Point(763, 634)
		Me.payHdrPaylenModeComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.payHdrPaylenModeComboBox.Name = "payHdrPaylenModeComboBox"
		Me.payHdrPaylenModeComboBox.Size = New System.Drawing.Size(313, 39)
		Me.payHdrPaylenModeComboBox.TabIndex = 81
		'
		'generateButton
		'
		Me.generateButton.Location = New System.Drawing.Point(763, 1374)
		Me.generateButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.generateButton.Name = "generateButton"
		Me.generateButton.Size = New System.Drawing.Size(200, 55)
		Me.generateButton.TabIndex = 93
		Me.generateButton.Text = "&Generate"
		Me.generateButton.UseVisualStyleBackColor = true
		'
		'stopButton
		'
		Me.stopButton.Location = New System.Drawing.Point(997, 1374)
		Me.stopButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.stopButton.Name = "stopButton"
		Me.stopButton.Size = New System.Drawing.Size(200, 55)
		Me.stopButton.TabIndex = 94
		Me.stopButton.Text = "&Stop"
		Me.stopButton.UseVisualStyleBackColor = true
		'
		'dirtyTxComboBox
		'
		Me.dirtyTxComboBox.FormattingEnabled = true
		Me.dirtyTxComboBox.Location = New System.Drawing.Point(763, 952)
		Me.dirtyTxComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.dirtyTxComboBox.Name = "dirtyTxComboBox"
		Me.dirtyTxComboBox.Size = New System.Drawing.Size(313, 39)
		Me.dirtyTxComboBox.TabIndex = 95
		'
		'dirtyTxLabel
		'
		Me.dirtyTxLabel.AutoSize = true
		Me.dirtyTxLabel.Location = New System.Drawing.Point(755, 914)
		Me.dirtyTxLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.dirtyTxLabel.Name = "dirtyTxLabel"
		Me.dirtyTxLabel.Size = New System.Drawing.Size(223, 32)
		Me.dirtyTxLabel.TabIndex = 96
		Me.dirtyTxLabel.Text = "Dirty Tx Enabled"
		'
		'label2
		'
		Me.label2.AutoSize = true
		Me.label2.Location = New System.Drawing.Point(755, 141)
		Me.label2.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(303, 32)
		Me.label2.TabIndex = 100
		Me.label2.Text = "Terminal Configuration"
		'
		'terminalConfigurationComboBox
		'
		Me.terminalConfigurationComboBox.Location = New System.Drawing.Point(763, 179)
		Me.terminalConfigurationComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.terminalConfigurationComboBox.Name = "terminalConfigurationComboBox"
		Me.terminalConfigurationComboBox.Size = New System.Drawing.Size(313, 39)
		Me.terminalConfigurationComboBox.TabIndex = 99
		'
		'label1
		'
		Me.label1.AutoSize = true
		Me.label1.Location = New System.Drawing.Point(755, 29)
		Me.label1.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(159, 32)
		Me.label1.TabIndex = 98
		Me.label1.Text = "Output Port"
		'
		'outputPortComboBox
		'
		Me.outputPortComboBox.Location = New System.Drawing.Point(763, 69)
		Me.outputPortComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.outputPortComboBox.Name = "outputPortComboBox"
		Me.outputPortComboBox.Size = New System.Drawing.Size(313, 39)
		Me.outputPortComboBox.TabIndex = 97
		'
		'clkTerminalLabel
		'
		Me.clkTerminalLabel.AutoSize = true
		Me.clkTerminalLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.clkTerminalLabel.Location = New System.Drawing.Point(56, 835)
		Me.clkTerminalLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.clkTerminalLabel.Name = "clkTerminalLabel"
		Me.clkTerminalLabel.Size = New System.Drawing.Size(306, 32)
		Me.clkTerminalLabel.TabIndex = 78
		Me.clkTerminalLabel.Text = "Export Clock Settings"
		'
		'NumOfUniquePacketsLabel
		'
		Me.NumOfUniquePacketsLabel.AutoSize = true
		Me.NumOfUniquePacketsLabel.Location = New System.Drawing.Point(755, 812)
		Me.NumOfUniquePacketsLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.NumOfUniquePacketsLabel.Name = "NumOfUniquePacketsLabel"
		Me.NumOfUniquePacketsLabel.Size = New System.Drawing.Size(351, 32)
		Me.NumOfUniquePacketsLabel.TabIndex = 88
		Me.NumOfUniquePacketsLabel.Text = "Number of Unique Packets"
		'
		'NumOfUniquePacketsNumeric
		'
		Me.NumOfUniquePacketsNumeric.Location = New System.Drawing.Point(763, 850)
		Me.NumOfUniquePacketsNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.NumOfUniquePacketsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.NumOfUniquePacketsNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.NumOfUniquePacketsNumeric.Name = "NumOfUniquePacketsNumeric"
		Me.NumOfUniquePacketsNumeric.Size = New System.Drawing.Size(320, 38)
		Me.NumOfUniquePacketsNumeric.TabIndex = 83
		Me.NumOfUniquePacketsNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'OversamplingFactorLabel
		'
		Me.OversamplingFactorLabel.AutoSize = true
		Me.OversamplingFactorLabel.Location = New System.Drawing.Point(755, 353)
		Me.OversamplingFactorLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.OversamplingFactorLabel.Name = "OversamplingFactorLabel"
		Me.OversamplingFactorLabel.Size = New System.Drawing.Size(277, 32)
		Me.OversamplingFactorLabel.TabIndex = 121
		Me.OversamplingFactorLabel.Text = "Oversampling Factor"
		'
		'OversamplingFactorNumeric
		'
		Me.OversamplingFactorNumeric.Location = New System.Drawing.Point(763, 391)
		Me.OversamplingFactorNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.OversamplingFactorNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.OversamplingFactorNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.OversamplingFactorNumeric.Name = "OversamplingFactorNumeric"
		Me.OversamplingFactorNumeric.Size = New System.Drawing.Size(320, 38)
		Me.OversamplingFactorNumeric.TabIndex = 120
		Me.OversamplingFactorNumeric.Value = New Decimal(New Integer() {8, 0, 0, 0})
		'
		'dataRateLabel
		'
		Me.dataRateLabel.AutoSize = true
		Me.dataRateLabel.Location = New System.Drawing.Point(766, 472)
		Me.dataRateLabel.Margin = New System.Windows.Forms.Padding(56, 0, 56, 0)
		Me.dataRateLabel.Name = "dataRateLabel"
		Me.dataRateLabel.Size = New System.Drawing.Size(212, 32)
		Me.dataRateLabel.TabIndex = 45
		Me.dataRateLabel.Text = "Data Rate (bps)"
		'
		'dataRateNumeric
		'
		Me.dataRateNumeric.Location = New System.Drawing.Point(761, 519)
		Me.dataRateNumeric.Margin = New System.Windows.Forms.Padding(56, 41, 56, 41)
		Me.dataRateNumeric.Maximum = New Decimal(New Integer() {7500000, 0, 0, 0})
		Me.dataRateNumeric.Name = "dataRateNumeric"
		Me.dataRateNumeric.Size = New System.Drawing.Size(322, 38)
		Me.dataRateNumeric.TabIndex = 43
		Me.dataRateNumeric.Value = New Decimal(New Integer() {2000000, 0, 0, 0})
		'
		'zadoffChuIndexLabel
		'
		Me.zadoffChuIndexLabel.AutoSize = true
		Me.zadoffChuIndexLabel.Location = New System.Drawing.Point(1237, 384)
		Me.zadoffChuIndexLabel.Margin = New System.Windows.Forms.Padding(56, 0, 56, 0)
		Me.zadoffChuIndexLabel.Name = "zadoffChuIndexLabel"
		Me.zadoffChuIndexLabel.Size = New System.Drawing.Size(232, 32)
		Me.zadoffChuIndexLabel.TabIndex = 45
		Me.zadoffChuIndexLabel.Text = "Zadoff-Chu Index"
		'
		'zadoffChuIndexNumeric
		'
		Me.zadoffChuIndexNumeric.Location = New System.Drawing.Point(1530, 378)
		Me.zadoffChuIndexNumeric.Margin = New System.Windows.Forms.Padding(56, 41, 56, 41)
		Me.zadoffChuIndexNumeric.Name = "zadoffChuIndexNumeric"
		Me.zadoffChuIndexNumeric.Size = New System.Drawing.Size(240, 38)
		Me.zadoffChuIndexNumeric.TabIndex = 43
		Me.zadoffChuIndexNumeric.Value = New Decimal(New Integer() {7, 0, 0, 0})
		'
		'physicalChannelAddressLabel
		'
		Me.physicalChannelAddressLabel.AutoSize = true
		Me.physicalChannelAddressLabel.Location = New System.Drawing.Point(1169, 446)
		Me.physicalChannelAddressLabel.Margin = New System.Windows.Forms.Padding(56, 0, 56, 0)
		Me.physicalChannelAddressLabel.Name = "physicalChannelAddressLabel"
		Me.physicalChannelAddressLabel.Size = New System.Drawing.Size(346, 32)
		Me.physicalChannelAddressLabel.TabIndex = 45
		Me.physicalChannelAddressLabel.Text = "Physical Channel Address"
		'
		'physicalChannelAddressNumeric
		'
		Me.physicalChannelAddressNumeric.Hexadecimal = true
		Me.physicalChannelAddressNumeric.Location = New System.Drawing.Point(1530, 442)
		Me.physicalChannelAddressNumeric.Margin = New System.Windows.Forms.Padding(21, 17, 21, 17)
		Me.physicalChannelAddressNumeric.Maximum = New Decimal(New Integer() {-727379968, 232, 0, 0})
		Me.physicalChannelAddressNumeric.Name = "physicalChannelAddressNumeric"
		Me.physicalChannelAddressNumeric.Size = New System.Drawing.Size(240, 38)
		Me.physicalChannelAddressNumeric.TabIndex = 43
		Me.physicalChannelAddressNumeric.Value = New Decimal(New Integer() {357913941, 159, 0, 0})
		'
		'HdtPacketFormatLabel
		'
		Me.HdtPacketFormatLabel.AutoSize = true
		Me.HdtPacketFormatLabel.Location = New System.Drawing.Point(1208, 510)
		Me.HdtPacketFormatLabel.Margin = New System.Windows.Forms.Padding(56, 0, 56, 0)
		Me.HdtPacketFormatLabel.Name = "HdtPacketFormatLabel"
		Me.HdtPacketFormatLabel.Size = New System.Drawing.Size(261, 32)
		Me.HdtPacketFormatLabel.TabIndex = 41
		Me.HdtPacketFormatLabel.Text = "HDT Packet Format"
		'
		'HdtPacketFormatComboBox
		'
		Me.HdtPacketFormatComboBox.Location = New System.Drawing.Point(1530, 504)
		Me.HdtPacketFormatComboBox.Margin = New System.Windows.Forms.Padding(56, 41, 56, 41)
		Me.HdtPacketFormatComboBox.Name = "HdtPacketFormatComboBox"
		Me.HdtPacketFormatComboBox.Size = New System.Drawing.Size(240, 39)
		Me.HdtPacketFormatComboBox.TabIndex = 28
		Me.HdtPacketFormatComboBox.Text = "Format0"
		'
		'HdtPhyIntervalLabel
		'
		Me.HdtPhyIntervalLabel.AutoSize = true
		Me.HdtPhyIntervalLabel.Location = New System.Drawing.Point(1208, 567)
		Me.HdtPhyIntervalLabel.Margin = New System.Windows.Forms.Padding(56, 0, 56, 0)
		Me.HdtPhyIntervalLabel.Name = "HdtPhyIntervalLabel"
		Me.HdtPhyIntervalLabel.Size = New System.Drawing.Size(275, 32)
		Me.HdtPhyIntervalLabel.TabIndex = 45
		Me.HdtPhyIntervalLabel.Text = "HDT PHY Interval (s)"
		'
		'HdtPhyIntervalNumeric
		'
		Me.HdtPhyIntervalNumeric.DecimalPlaces = 10
		Me.HdtPhyIntervalNumeric.Location = New System.Drawing.Point(1530, 567)
		Me.HdtPhyIntervalNumeric.Margin = New System.Windows.Forms.Padding(56, 41, 56, 41)
		Me.HdtPhyIntervalNumeric.Name = "HdtPhyIntervalNumeric"
		Me.HdtPhyIntervalNumeric.Size = New System.Drawing.Size(240, 38)
		Me.HdtPhyIntervalNumeric.TabIndex = 43
		Me.HdtPhyIntervalNumeric.Value = New Decimal(New Integer() {64, 0, 0, 393216})
		'
		'Label3
		'
		Me.Label3.AutoSize = true
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.Label3.Location = New System.Drawing.Point(1221, 321)
		Me.Label3.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(433, 32)
		Me.Label3.TabIndex = 122
		Me.Label3.Text = "High Data Throughput Settings"
		'
		'MainForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(16!, 31!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.AutoSize = true
		Me.ClientSize = New System.Drawing.Size(1839, 1490)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.OversamplingFactorLabel)
		Me.Controls.Add(Me.OversamplingFactorNumeric)
		Me.Controls.Add(Me.label2)
		Me.Controls.Add(Me.terminalConfigurationComboBox)
		Me.Controls.Add(Me.label1)
		Me.Controls.Add(Me.outputPortComboBox)
		Me.Controls.Add(Me.dirtyTxLabel)
		Me.Controls.Add(Me.dirtyTxComboBox)
		Me.Controls.Add(Me.generateButton)
		Me.Controls.Add(Me.stopButton)
		Me.Controls.Add(Me.payHdrPaylenModeLabel)
		Me.Controls.Add(Me.payHdrPaylenLabel)
		Me.Controls.Add(Me.NumOfUniquePacketsLabel)
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
		Me.Controls.Add(Me.dataRateLabel)
		Me.Controls.Add(Me.dataRateNumeric)
		Me.Controls.Add(Me.zadoffChuIndexLabel)
		Me.Controls.Add(Me.zadoffChuIndexNumeric)
		Me.Controls.Add(Me.physicalChannelAddressLabel)
		Me.Controls.Add(Me.physicalChannelAddressNumeric)
		Me.Controls.Add(Me.HdtPacketFormatLabel)
		Me.Controls.Add(Me.HdtPacketFormatComboBox)
		Me.Controls.Add(Me.HdtPhyIntervalLabel)
		Me.Controls.Add(Me.HdtPhyIntervalNumeric)
		Me.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.MaximumSize = New System.Drawing.Size(2040, 1578)
		Me.Name = "MainForm"
		Me.Text = "LE-HDT Example"
		CType(Me.chnNumberNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.powerLevelNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.externalAttnNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.headroomNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.quadratureSkewNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.iDcOffsetNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.qDcOffsetNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.iqGaimbalanceNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.cnrNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.carrierFreqOffNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.payHdrPaylenNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.NumOfUniquePacketsNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.OversamplingFactorNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.dataRateNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.zadoffChuIndexNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.physicalChannelAddressNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.HdtPhyIntervalNumeric,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)
		Me.PerformLayout

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
    Private WithEvents generateButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private dirtyTxComboBox As System.Windows.Forms.ComboBox
    Private dirtyTxLabel As System.Windows.Forms.Label
    Private label2 As System.Windows.Forms.Label
    Private terminalConfigurationComboBox As System.Windows.Forms.ComboBox
    Private label1 As System.Windows.Forms.Label
    Private outputPortComboBox As System.Windows.Forms.ComboBox
    Private WithEvents clkTerminalLabel As System.Windows.Forms.Label
    Private WithEvents NumOfUniquePacketsLabel As System.Windows.Forms.Label
    Private WithEvents NumOfUniquePacketsNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents OversamplingFactorLabel As System.Windows.Forms.Label
    Private WithEvents OversamplingFactorNumeric As System.Windows.Forms.NumericUpDown
    Private dataRateLabel As System.Windows.Forms.Label 
    Private dataRateNumeric As System.Windows.Forms.NumericUpDown
    Private zadoffChuIndexLabel As System.Windows.Forms.Label 
    Private zadoffChuIndexNumeric As System.Windows.Forms.NumericUpDown 
    Private physicalChannelAddressLabel As System.Windows.Forms.Label 
    Private physicalChannelAddressNumeric As System.Windows.Forms.NumericUpDown
    Private HdtPacketFormatLabel As System.Windows.Forms.Label 
    Private HdtPacketFormatComboBox As System.Windows.Forms.ComboBox 
    Private HdtPhyIntervalLabel As System.Windows.Forms.Label 
    Private HdtPhyIntervalNumeric As System.Windows.Forms.NumericUpDown
   Private WithEvents Label3 As System.Windows.Forms.Label
End Class

