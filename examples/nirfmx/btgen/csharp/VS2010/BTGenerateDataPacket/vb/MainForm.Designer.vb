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
		Me.autoHeadroomEnabLabel = New System.Windows.Forms.Label()
		Me.headroomLabel = New System.Windows.Forms.Label()
		Me.actualHeadroomLabel = New System.Windows.Forms.Label()
		Me.refSourceLabel = New System.Windows.Forms.Label()
		Me.clkOutTerminalLabel = New System.Windows.Forms.Label()
		Me.allIqImpairEnLabel = New System.Windows.Forms.Label()
		Me.quadratureSkewLabel = New System.Windows.Forms.Label()
		Me.iDCOffsetLabel = New System.Windows.Forms.Label()
		Me.qDCOffsetLabel = New System.Windows.Forms.Label()
		Me.iqGaimbalanceLabel = New System.Windows.Forms.Label()
		Me.carrierFreqOffLabel = New System.Windows.Forms.Label()
		Me.awgnEnabledLabel = New System.Windows.Forms.Label()
		Me.cnrLabel = New System.Windows.Forms.Label()
		Me.bdaddrLapLabel = New System.Windows.Forms.Label()
		Me.bdaddrUapLabel = New System.Windows.Forms.Label()
		Me.bdaddrNapLabel = New System.Windows.Forms.Label()
		Me.pktLtAddrLabel = New System.Windows.Forms.Label()
		Me.pktHdrFlowLabel = New System.Windows.Forms.Label()
		Me.pktHdrArqnLabel = New System.Windows.Forms.Label()
		Me.pktHdrSeqnLabel = New System.Windows.Forms.Label()
		Me.payHdrLlidLabel = New System.Windows.Forms.Label()
		Me.payHdrFlowLabel = New System.Windows.Forms.Label()
		Me.userDefinedBitLabel = New System.Windows.Forms.Label()
		Me.payHdrPaylenModeLabel = New System.Windows.Forms.Label()
		Me.payHdrPaylenLabel = New System.Windows.Forms.Label()
		Me.payHdrActPaylenLabel = New System.Windows.Forms.Label()
		Me.payHdrDatatypeLabel = New System.Windows.Forms.Label()
		Me.paydatPnorderLabel = New System.Windows.Forms.Label()
		Me.paydatSeedLabel = New System.Windows.Forms.Label()
		Me.whiteEnLabel = New System.Windows.Forms.Label()
		Me.whiteClkLabel = New System.Windows.Forms.Label()
		Me.waveNameLabel = New System.Windows.Forms.Label()
		Me.scriptLabel = New System.Windows.Forms.Label()
		Me.errorLabel = New System.Windows.Forms.Label()
		Me.extmsgPacket22Label = New System.Windows.Forms.Label()
		Me.textmsgPayloadHdrLabel = New System.Windows.Forms.Label()
		Me.payloadDataLabel = New System.Windows.Forms.Label()
		Me.hardwareLabel = New System.Windows.Forms.Label()
		Me.frequencySettingLabel = New System.Windows.Forms.Label()
		Me.impairmentsLabel = New System.Windows.Forms.Label()
		Me.whiteningSettingLabel = New System.Windows.Forms.Label()
		Me.bdAddressLabel = New System.Windows.Forms.Label()
		Me.packetLabel = New System.Windows.Forms.Label()
		Me.payloadLabel = New System.Windows.Forms.Label()
		Me.chnNumberNumeric = New System.Windows.Forms.NumericUpDown()
		Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
		Me.externalAttnNumeric = New System.Windows.Forms.NumericUpDown()
		Me.headroomNumeric = New System.Windows.Forms.NumericUpDown()
		Me.quadratureSkewNumeric = New System.Windows.Forms.NumericUpDown()
		Me.iDCOffsetNumeric = New System.Windows.Forms.NumericUpDown()
		Me.qDCOffsetNumeric = New System.Windows.Forms.NumericUpDown()
		Me.iqGaimbalanceNumeric = New System.Windows.Forms.NumericUpDown()
		Me.carrierFreqOffNumeric = New System.Windows.Forms.NumericUpDown()
		Me.cnrNumeric = New System.Windows.Forms.NumericUpDown()
		Me.bdaddrLapNumeric = New System.Windows.Forms.NumericUpDown()
		Me.bdaddrUapNumeric = New System.Windows.Forms.NumericUpDown()
		Me.bdaddrNapNumeric = New System.Windows.Forms.NumericUpDown()
		Me.pktLtAddrNumeric = New System.Windows.Forms.NumericUpDown()
		Me.pktHdrFlowNumeric = New System.Windows.Forms.NumericUpDown()
		Me.pktHdrSeqnNumeric = New System.Windows.Forms.NumericUpDown()
		Me.payHdrLlidNumeric = New System.Windows.Forms.NumericUpDown()
		Me.payHdrFlowNumeric = New System.Windows.Forms.NumericUpDown()
		Me.payHdrPaylenNumeric = New System.Windows.Forms.NumericUpDown()
		Me.paydatPnorderNumeric = New System.Windows.Forms.NumericUpDown()
		Me.paydatSeedNumeric = New System.Windows.Forms.NumericUpDown()
		Me.whiteClkNumeric = New System.Windows.Forms.NumericUpDown()
		Me.rfsgResourceTextBox = New System.Windows.Forms.TextBox()
		Me.carrierFreqTextBox = New System.Windows.Forms.TextBox()
		Me.actualHeadroomTextBox = New System.Windows.Forms.TextBox()
		Me.payHdrActPaylenTextBox = New System.Windows.Forms.TextBox()
		Me.waveNameTextBox = New System.Windows.Forms.TextBox()
		Me.scriptTextBox = New System.Windows.Forms.TextBox()
		Me.errorTextBox = New System.Windows.Forms.TextBox()
		Me.generateButton = New System.Windows.Forms.Button()
		Me.stopButton = New System.Windows.Forms.Button()
		Me.autoHeadroomEnabComboBox = New System.Windows.Forms.ComboBox()
		Me.refSourceComboBox = New System.Windows.Forms.ComboBox()
		Me.userDefinedBitsComboBox = New System.Windows.Forms.ComboBox()
		Me.clkOutTerminalComboBox = New System.Windows.Forms.ComboBox()
		Me.allIqImpairEnComboBox = New System.Windows.Forms.ComboBox()
		Me.awgnEnabledComboBox = New System.Windows.Forms.ComboBox()
		Me.pktHdrArqnComboBox = New System.Windows.Forms.ComboBox()
		Me.packetComboBox = New System.Windows.Forms.ComboBox()
		Me.payHdrPaylenModeComboBox = New System.Windows.Forms.ComboBox()
		Me.payHdrDatatypeComboBox = New System.Windows.Forms.ComboBox()
		Me.whiteEnComboBox = New System.Windows.Forms.ComboBox()
		Me.timer = New System.Windows.Forms.Timer(Me.components)
		Me.numOfUniqNumeric = New System.Windows.Forms.NumericUpDown()
		Me.numOfIdleSlotsNumeric = New System.Windows.Forms.NumericUpDown()
		Me.numOfUniqLabel = New System.Windows.Forms.Label()
		Me.numOfIdleLabel = New System.Windows.Forms.Label()
		Me.label1 = New System.Windows.Forms.Label()
		Me.outputPortComboBox = New System.Windows.Forms.ComboBox()
		Me.label2 = New System.Windows.Forms.Label()
		Me.terminalConfigurationComboBox = New System.Windows.Forms.ComboBox()
		Me.iCommonModeOffsetLabel = New System.Windows.Forms.Label()
		Me.qCommonModeOffsetLabel = New System.Windows.Forms.Label()
		Me.iCommonModeOffsetNumeric = New System.Windows.Forms.NumericUpDown()
		Me.qCommonModeOffsetNumeric = New System.Windows.Forms.NumericUpDown()
		Me.iOffsetLabel = New System.Windows.Forms.Label()
		Me.qOffsetLabel = New System.Windows.Forms.Label()
		Me.iOffsetNumeric = New System.Windows.Forms.NumericUpDown()
		Me.qOffsetNumeric = New System.Windows.Forms.NumericUpDown()
		DirectCast(Me.chnNumberNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.externalAttnNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.headroomNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.quadratureSkewNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.iDCOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.qDCOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.iqGaimbalanceNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.carrierFreqOffNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.cnrNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.bdaddrLapNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.bdaddrUapNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.bdaddrNapNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.pktLtAddrNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.pktHdrFlowNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.pktHdrSeqnNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.payHdrLlidNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.payHdrFlowNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.payHdrPaylenNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.paydatPnorderNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.paydatSeedNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.whiteClkNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.numOfUniqNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.numOfIdleSlotsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.iCommonModeOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.qCommonModeOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.iOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.qOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		' 
		' rfsgResourceLabel
		' 
		Me.rfsgResourceLabel.AutoSize = True
		Me.rfsgResourceLabel.Location = New System.Drawing.Point(17, 29)
		Me.rfsgResourceLabel.Name = "rfsgResourceLabel"
		Me.rfsgResourceLabel.Size = New System.Drawing.Size(85, 13)
		Me.rfsgResourceLabel.TabIndex = 0
		Me.rfsgResourceLabel.Text = "RFSG Resource"
		' 
		' chnNumberLabel
		' 
		Me.chnNumberLabel.AutoSize = True
		Me.chnNumberLabel.Location = New System.Drawing.Point(17, 47)
		Me.chnNumberLabel.Name = "chnNumberLabel"
		Me.chnNumberLabel.Size = New System.Drawing.Size(86, 13)
		Me.chnNumberLabel.TabIndex = 1
		Me.chnNumberLabel.Text = "Channel Number"
		' 
		' carrierFreqLabel
		' 
		Me.carrierFreqLabel.AutoSize = True
		Me.carrierFreqLabel.Location = New System.Drawing.Point(19, 68)
		Me.carrierFreqLabel.Name = "carrierFreqLabel"
		Me.carrierFreqLabel.Size = New System.Drawing.Size(112, 13)
		Me.carrierFreqLabel.TabIndex = 2
		Me.carrierFreqLabel.Text = "Carrier Frequency (Hz)"
		' 
		' powerLevelLabel
		' 
		Me.powerLevelLabel.AutoSize = True
		Me.powerLevelLabel.Location = New System.Drawing.Point(17, 88)
		Me.powerLevelLabel.Name = "powerLevelLabel"
		Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
		Me.powerLevelLabel.TabIndex = 3
		Me.powerLevelLabel.Text = "Power Level (dBm)"
		' 
		' externalAttnLabel
		' 
		Me.externalAttnLabel.AutoSize = True
		Me.externalAttnLabel.Location = New System.Drawing.Point(17, 110)
		Me.externalAttnLabel.Name = "externalAttnLabel"
		Me.externalAttnLabel.Size = New System.Drawing.Size(124, 13)
		Me.externalAttnLabel.TabIndex = 4
		Me.externalAttnLabel.Text = "External Attenuation (dB)"
		' 
		' autoHeadroomEnabLabel
		' 
		Me.autoHeadroomEnabLabel.AutoSize = True
		Me.autoHeadroomEnabLabel.Location = New System.Drawing.Point(17, 132)
		Me.autoHeadroomEnabLabel.Name = "autoHeadroomEnabLabel"
		Me.autoHeadroomEnabLabel.Size = New System.Drawing.Size(123, 13)
		Me.autoHeadroomEnabLabel.TabIndex = 5
		Me.autoHeadroomEnabLabel.Text = "Auto Headroom Enabled"
		' 
		' headroomLabel
		' 
		Me.headroomLabel.AutoSize = True
		Me.headroomLabel.Location = New System.Drawing.Point(17, 153)
		Me.headroomLabel.Name = "headroomLabel"
		Me.headroomLabel.Size = New System.Drawing.Size(78, 13)
		Me.headroomLabel.TabIndex = 6
		Me.headroomLabel.Text = "Headroom (dB)"
		' 
		' actualHeadroomLabel
		' 
		Me.actualHeadroomLabel.AutoSize = True
		Me.actualHeadroomLabel.Location = New System.Drawing.Point(17, 172)
		Me.actualHeadroomLabel.Name = "actualHeadroomLabel"
		Me.actualHeadroomLabel.Size = New System.Drawing.Size(111, 13)
		Me.actualHeadroomLabel.TabIndex = 7
		Me.actualHeadroomLabel.Text = "Actual Headroom (dB)"
		' 
		' refSourceLabel
		' 
		Me.refSourceLabel.AutoSize = True
		Me.refSourceLabel.Location = New System.Drawing.Point(16, 230)
		Me.refSourceLabel.Name = "refSourceLabel"
		Me.refSourceLabel.Size = New System.Drawing.Size(94, 13)
		Me.refSourceLabel.TabIndex = 8
		Me.refSourceLabel.Text = "Reference Source"
		' 
		' clkOutTerminalLabel
		' 
		Me.clkOutTerminalLabel.AutoSize = True
		Me.clkOutTerminalLabel.Location = New System.Drawing.Point(16, 251)
		Me.clkOutTerminalLabel.Name = "clkOutTerminalLabel"
		Me.clkOutTerminalLabel.Size = New System.Drawing.Size(67, 13)
		Me.clkOutTerminalLabel.TabIndex = 9
		Me.clkOutTerminalLabel.Text = "Export Clock"
		' 
		' allIqImpairEnLabel
		' 
		Me.allIqImpairEnLabel.AutoSize = True
		Me.allIqImpairEnLabel.Location = New System.Drawing.Point(16, 299)
		Me.allIqImpairEnLabel.Name = "allIqImpairEnLabel"
		Me.allIqImpairEnLabel.Size = New System.Drawing.Size(133, 13)
		Me.allIqImpairEnLabel.TabIndex = 10
		Me.allIqImpairEnLabel.Text = "All IQ Impairments Enabled"
		' 
		' quadratureSkewLabel
		' 
		Me.quadratureSkewLabel.AutoSize = True
		Me.quadratureSkewLabel.Location = New System.Drawing.Point(16, 321)
		Me.quadratureSkewLabel.Name = "quadratureSkewLabel"
		Me.quadratureSkewLabel.Size = New System.Drawing.Size(117, 13)
		Me.quadratureSkewLabel.TabIndex = 11
		Me.quadratureSkewLabel.Text = "Quadrature Skew (deg)"
		' 
		' iDCOffsetLabel
		' 
		Me.iDCOffsetLabel.AutoSize = True
		Me.iDCOffsetLabel.Location = New System.Drawing.Point(16, 343)
		Me.iDCOffsetLabel.Name = "iDCOffsetLabel"
		Me.iDCOffsetLabel.Size = New System.Drawing.Size(76, 13)
		Me.iDCOffsetLabel.TabIndex = 12
		Me.iDCOffsetLabel.Text = "I DC Offset (%)"
		' 
		' qDCOffsetLabel
		' 
		Me.qDCOffsetLabel.AutoSize = True
		Me.qDCOffsetLabel.Location = New System.Drawing.Point(16, 365)
		Me.qDCOffsetLabel.Name = "qDCOffsetLabel"
		Me.qDCOffsetLabel.Size = New System.Drawing.Size(81, 13)
		Me.qDCOffsetLabel.TabIndex = 13
		Me.qDCOffsetLabel.Text = "Q DC Offset (%)"
		' 
		' iqGaimbalanceLabel
		' 
		Me.iqGaimbalanceLabel.AutoSize = True
		Me.iqGaimbalanceLabel.Location = New System.Drawing.Point(16, 387)
		Me.iqGaimbalanceLabel.Name = "iqGaimbalanceLabel"
		Me.iqGaimbalanceLabel.Size = New System.Drawing.Size(117, 13)
		Me.iqGaimbalanceLabel.TabIndex = 14
		Me.iqGaimbalanceLabel.Text = "IQ Gain Imbalance (dB)"
		' 
		' carrierFreqOffLabel
		' 
		Me.carrierFreqOffLabel.AutoSize = True
		Me.carrierFreqOffLabel.Location = New System.Drawing.Point(16, 407)
		Me.carrierFreqOffLabel.Name = "carrierFreqOffLabel"
		Me.carrierFreqOffLabel.Size = New System.Drawing.Size(143, 13)
		Me.carrierFreqOffLabel.TabIndex = 15
		Me.carrierFreqOffLabel.Text = "Carrier Frequency Offset (Hz)"
		' 
		' awgnEnabledLabel
		' 
		Me.awgnEnabledLabel.AutoSize = True
		Me.awgnEnabledLabel.Location = New System.Drawing.Point(16, 428)
		Me.awgnEnabledLabel.Name = "awgnEnabledLabel"
		Me.awgnEnabledLabel.Size = New System.Drawing.Size(83, 13)
		Me.awgnEnabledLabel.TabIndex = 16
		Me.awgnEnabledLabel.Text = "AWGN Enabled"
		' 
		' cnrLabel
		' 
		Me.cnrLabel.AutoSize = True
		Me.cnrLabel.Location = New System.Drawing.Point(16, 448)
		Me.cnrLabel.Name = "cnrLabel"
		Me.cnrLabel.Size = New System.Drawing.Size(129, 13)
		Me.cnrLabel.TabIndex = 17
		Me.cnrLabel.Text = "Carrier to Noise Ratio (dB)"
		' 
		' bdaddrLapLabel
		' 
		Me.bdaddrLapLabel.AutoSize = True
		Me.bdaddrLapLabel.Location = New System.Drawing.Point(280, 24)
		Me.bdaddrLapLabel.Name = "bdaddrLapLabel"
		Me.bdaddrLapLabel.Size = New System.Drawing.Size(27, 13)
		Me.bdaddrLapLabel.TabIndex = 18
		Me.bdaddrLapLabel.Text = "LAP"
		' 
		' bdaddrUapLabel
		' 
		Me.bdaddrUapLabel.AutoSize = True
		Me.bdaddrUapLabel.Location = New System.Drawing.Point(280, 46)
		Me.bdaddrUapLabel.Name = "bdaddrUapLabel"
		Me.bdaddrUapLabel.Size = New System.Drawing.Size(29, 13)
		Me.bdaddrUapLabel.TabIndex = 19
		Me.bdaddrUapLabel.Text = "UAP"
		' 
		' bdaddrNapLabel
		' 
		Me.bdaddrNapLabel.AutoSize = True
		Me.bdaddrNapLabel.Location = New System.Drawing.Point(280, 68)
		Me.bdaddrNapLabel.Name = "bdaddrNapLabel"
		Me.bdaddrNapLabel.Size = New System.Drawing.Size(29, 13)
		Me.bdaddrNapLabel.TabIndex = 20
		Me.bdaddrNapLabel.Text = "NAP"
		' 
		' pktLtAddrLabel
		' 
		Me.pktLtAddrLabel.AutoSize = True
		Me.pktLtAddrLabel.Location = New System.Drawing.Point(280, 163)
		Me.pktLtAddrLabel.Name = "pktLtAddrLabel"
		Me.pktLtAddrLabel.Size = New System.Drawing.Size(61, 13)
		Me.pktLtAddrLabel.TabIndex = 21
		Me.pktLtAddrLabel.Text = "LT Address"
		' 
		' pktHdrFlowLabel
		' 
		Me.pktHdrFlowLabel.AutoSize = True
		Me.pktHdrFlowLabel.Location = New System.Drawing.Point(280, 188)
		Me.pktHdrFlowLabel.Name = "pktHdrFlowLabel"
		Me.pktHdrFlowLabel.Size = New System.Drawing.Size(29, 13)
		Me.pktHdrFlowLabel.TabIndex = 22
		Me.pktHdrFlowLabel.Text = "Flow"
		' 
		' pktHdrArqnLabel
		' 
		Me.pktHdrArqnLabel.AutoSize = True
		Me.pktHdrArqnLabel.Location = New System.Drawing.Point(280, 207)
		Me.pktHdrArqnLabel.Name = "pktHdrArqnLabel"
		Me.pktHdrArqnLabel.Size = New System.Drawing.Size(38, 13)
		Me.pktHdrArqnLabel.TabIndex = 23
		Me.pktHdrArqnLabel.Text = "ARQN"
		' 
		' pktHdrSeqnLabel
		' 
		Me.pktHdrSeqnLabel.AutoSize = True
		Me.pktHdrSeqnLabel.Location = New System.Drawing.Point(280, 226)
		Me.pktHdrSeqnLabel.Name = "pktHdrSeqnLabel"
		Me.pktHdrSeqnLabel.Size = New System.Drawing.Size(37, 13)
		Me.pktHdrSeqnLabel.TabIndex = 24
		Me.pktHdrSeqnLabel.Text = "SEQN"
		' 
		' payHdrLlidLabel
		' 
		Me.payHdrLlidLabel.AutoSize = True
		Me.payHdrLlidLabel.Location = New System.Drawing.Point(280, 296)
		Me.payHdrLlidLabel.Name = "payHdrLlidLabel"
		Me.payHdrLlidLabel.Size = New System.Drawing.Size(30, 13)
		Me.payHdrLlidLabel.TabIndex = 25
		Me.payHdrLlidLabel.Text = "LLID"
		' 
		' payHdrFlowLabel
		' 
		Me.payHdrFlowLabel.AutoSize = True
		Me.payHdrFlowLabel.Location = New System.Drawing.Point(280, 318)
		Me.payHdrFlowLabel.Name = "payHdrFlowLabel"
		Me.payHdrFlowLabel.Size = New System.Drawing.Size(29, 13)
		Me.payHdrFlowLabel.TabIndex = 26
		Me.payHdrFlowLabel.Text = "Flow"
		' 
		' userDefinedBitLabel
		' 
		Me.userDefinedBitLabel.AutoSize = True
		Me.userDefinedBitLabel.Location = New System.Drawing.Point(270, 497)
		Me.userDefinedBitLabel.Name = "userDefinedBitLabel"
		Me.userDefinedBitLabel.Size = New System.Drawing.Size(121, 13)
		Me.userDefinedBitLabel.TabIndex = 27
		Me.userDefinedBitLabel.Text = "User Defined Bit Pattern"
		' 
		' payHdrPaylenModeLabel
		' 
		Me.payHdrPaylenModeLabel.AutoSize = True
		Me.payHdrPaylenModeLabel.Location = New System.Drawing.Point(280, 339)
		Me.payHdrPaylenModeLabel.Name = "payHdrPaylenModeLabel"
		Me.payHdrPaylenModeLabel.Size = New System.Drawing.Size(111, 13)
		Me.payHdrPaylenModeLabel.TabIndex = 27
		Me.payHdrPaylenModeLabel.Text = "Payload Length Mode"
		' 
		' payHdrPaylenLabel
		' 
		Me.payHdrPaylenLabel.AutoSize = True
		Me.payHdrPaylenLabel.Location = New System.Drawing.Point(280, 360)
		Me.payHdrPaylenLabel.Name = "payHdrPaylenLabel"
		Me.payHdrPaylenLabel.Size = New System.Drawing.Size(81, 13)
		Me.payHdrPaylenLabel.TabIndex = 28
		Me.payHdrPaylenLabel.Text = "Payload Length"
		' 
		' payHdrActPaylenLabel
		' 
		Me.payHdrActPaylenLabel.AutoSize = True
		Me.payHdrActPaylenLabel.Location = New System.Drawing.Point(280, 382)
		Me.payHdrActPaylenLabel.Name = "payHdrActPaylenLabel"
		Me.payHdrActPaylenLabel.Size = New System.Drawing.Size(114, 13)
		Me.payHdrActPaylenLabel.TabIndex = 29
		Me.payHdrActPaylenLabel.Text = "Actual Payload Length"
		' 
		' payHdrDatatypeLabel
		' 
		Me.payHdrDatatypeLabel.AutoSize = True
		Me.payHdrDatatypeLabel.Location = New System.Drawing.Point(281, 426)
		Me.payHdrDatatypeLabel.Name = "payHdrDatatypeLabel"
		Me.payHdrDatatypeLabel.Size = New System.Drawing.Size(57, 13)
		Me.payHdrDatatypeLabel.TabIndex = 30
		Me.payHdrDatatypeLabel.Text = "Data Type"
		' 
		' paydatPnorderLabel
		' 
		Me.paydatPnorderLabel.AutoSize = True
		Me.paydatPnorderLabel.Location = New System.Drawing.Point(281, 451)
		Me.paydatPnorderLabel.Name = "paydatPnorderLabel"
		Me.paydatPnorderLabel.Size = New System.Drawing.Size(51, 13)
		Me.paydatPnorderLabel.TabIndex = 31
		Me.paydatPnorderLabel.Text = "PN Order"
		' 
		' paydatSeedLabel
		' 
		Me.paydatSeedLabel.AutoSize = True
		Me.paydatSeedLabel.Location = New System.Drawing.Point(280, 471)
		Me.paydatSeedLabel.Name = "paydatSeedLabel"
		Me.paydatSeedLabel.Size = New System.Drawing.Size(32, 13)
		Me.paydatSeedLabel.TabIndex = 32
		Me.paydatSeedLabel.Text = "Seed"
		' 
		' whiteEnLabel
		' 
		Me.whiteEnLabel.AutoSize = True
		Me.whiteEnLabel.Location = New System.Drawing.Point(535, 23)
		Me.whiteEnLabel.Name = "whiteEnLabel"
		Me.whiteEnLabel.Size = New System.Drawing.Size(46, 13)
		Me.whiteEnLabel.TabIndex = 33
		Me.whiteEnLabel.Text = "Enabled"
		' 
		' whiteClkLabel
		' 
		Me.whiteClkLabel.AutoSize = True
		Me.whiteClkLabel.Location = New System.Drawing.Point(535, 43)
		Me.whiteClkLabel.Name = "whiteClkLabel"
		Me.whiteClkLabel.Size = New System.Drawing.Size(34, 13)
		Me.whiteClkLabel.TabIndex = 34
		Me.whiteClkLabel.Text = "Clock"
		' 
		' waveNameLabel
		' 
		Me.waveNameLabel.AutoSize = True
		Me.waveNameLabel.Location = New System.Drawing.Point(524, 115)
		Me.waveNameLabel.Name = "waveNameLabel"
		Me.waveNameLabel.Size = New System.Drawing.Size(87, 13)
		Me.waveNameLabel.TabIndex = 35
		Me.waveNameLabel.Text = "Waveform Name"
		' 
		' scriptLabel
		' 
		Me.scriptLabel.AutoSize = True
		Me.scriptLabel.Location = New System.Drawing.Point(528, 148)
		Me.scriptLabel.Name = "scriptLabel"
		Me.scriptLabel.Size = New System.Drawing.Size(34, 13)
		Me.scriptLabel.TabIndex = 36
		Me.scriptLabel.Text = "Script"
		' 
		' errorLabel
		' 
		Me.errorLabel.AutoSize = True
		Me.errorLabel.Location = New System.Drawing.Point(531, 277)
		Me.errorLabel.Name = "errorLabel"
		Me.errorLabel.Size = New System.Drawing.Size(29, 13)
		Me.errorLabel.TabIndex = 36
		Me.errorLabel.Text = "Error"
		' 
		' extmsgPacket22Label
		' 
		Me.extmsgPacket22Label.AutoSize = True
		Me.extmsgPacket22Label.Location = New System.Drawing.Point(280, 148)
		Me.extmsgPacket22Label.Name = "extmsgPacket22Label"
		Me.extmsgPacket22Label.Size = New System.Drawing.Size(42, 13)
		Me.extmsgPacket22Label.TabIndex = 37
		Me.extmsgPacket22Label.Text = "Header"
		' 
		' textmsgPayloadHdrLabel
		' 
		Me.textmsgPayloadHdrLabel.AutoSize = True
		Me.textmsgPayloadHdrLabel.Location = New System.Drawing.Point(280, 276)
		Me.textmsgPayloadHdrLabel.Name = "textmsgPayloadHdrLabel"
		Me.textmsgPayloadHdrLabel.Size = New System.Drawing.Size(42, 13)
		Me.textmsgPayloadHdrLabel.TabIndex = 38
		Me.textmsgPayloadHdrLabel.Text = "Header"
		' 
		' payloadDataLabel
		' 
		Me.payloadDataLabel.AutoSize = True
		Me.payloadDataLabel.Location = New System.Drawing.Point(280, 408)
		Me.payloadDataLabel.Name = "payloadDataLabel"
		Me.payloadDataLabel.Size = New System.Drawing.Size(30, 13)
		Me.payloadDataLabel.TabIndex = 39
		Me.payloadDataLabel.Text = "Data"
		' 
		' hardwareLabel
		' 
		Me.hardwareLabel.AutoSize = True
		Me.hardwareLabel.Location = New System.Drawing.Point(30, 6)
		Me.hardwareLabel.Name = "hardwareLabel"
		Me.hardwareLabel.Size = New System.Drawing.Size(53, 13)
		Me.hardwareLabel.TabIndex = 40
		Me.hardwareLabel.Text = "Hardware"
		' 
		' frequencySettingLabel
		' 
		Me.frequencySettingLabel.AutoSize = True
		Me.frequencySettingLabel.Location = New System.Drawing.Point(29, 212)
		Me.frequencySettingLabel.Name = "frequencySettingLabel"
		Me.frequencySettingLabel.Size = New System.Drawing.Size(98, 13)
		Me.frequencySettingLabel.TabIndex = 41
		Me.frequencySettingLabel.Text = "Frequency Settings"
		' 
		' impairmentsLabel
		' 
		Me.impairmentsLabel.AutoSize = True
		Me.impairmentsLabel.Location = New System.Drawing.Point(29, 278)
		Me.impairmentsLabel.Name = "impairmentsLabel"
		Me.impairmentsLabel.Size = New System.Drawing.Size(63, 13)
		Me.impairmentsLabel.TabIndex = 42
		Me.impairmentsLabel.Text = "Impairments"
		' 
		' whiteningSettingLabel
		' 
		Me.whiteningSettingLabel.AutoSize = True
		Me.whiteningSettingLabel.Location = New System.Drawing.Point(561, 3)
		Me.whiteningSettingLabel.Name = "whiteningSettingLabel"
		Me.whiteningSettingLabel.Size = New System.Drawing.Size(96, 13)
		Me.whiteningSettingLabel.TabIndex = 43
		Me.whiteningSettingLabel.Text = "Whitening Settings"
		' 
		' bdAddressLabel
		' 
		Me.bdAddressLabel.AutoSize = True
		Me.bdAddressLabel.Location = New System.Drawing.Point(301, 6)
		Me.bdAddressLabel.Name = "bdAddressLabel"
		Me.bdAddressLabel.Size = New System.Drawing.Size(63, 13)
		Me.bdAddressLabel.TabIndex = 44
		Me.bdAddressLabel.Text = "BD Address"
		' 
		' packetLabel
		' 
		Me.packetLabel.AutoSize = True
		Me.packetLabel.Location = New System.Drawing.Point(281, 93)
		Me.packetLabel.Name = "packetLabel"
		Me.packetLabel.Size = New System.Drawing.Size(41, 13)
		Me.packetLabel.TabIndex = 45
		Me.packetLabel.Text = "Packet"
		' 
		' payloadLabel
		' 
		Me.payloadLabel.AutoSize = True
		Me.payloadLabel.Location = New System.Drawing.Point(301, 258)
		Me.payloadLabel.Name = "payloadLabel"
		Me.payloadLabel.Size = New System.Drawing.Size(45, 13)
		Me.payloadLabel.TabIndex = 46
		Me.payloadLabel.Text = "Payload"
		' 
		' chnNumberNumeric
		' 
		Me.chnNumberNumeric.Location = New System.Drawing.Point(162, 43)
		Me.chnNumberNumeric.Name = "chnNumberNumeric"
		Me.chnNumberNumeric.Size = New System.Drawing.Size(90, 20)
		Me.chnNumberNumeric.TabIndex = 1
		Me.chnNumberNumeric.Value = New Decimal(New Integer() {3, 0, 0, 0})
		' 
		' powerLevelNumeric
		' 
		Me.powerLevelNumeric.Location = New System.Drawing.Point(162, 85)
		Me.powerLevelNumeric.Name = "powerLevelNumeric"
		Me.powerLevelNumeric.Size = New System.Drawing.Size(90, 20)
		Me.powerLevelNumeric.TabIndex = 3
		' 
		' externalAttnNumeric
		' 
		Me.externalAttnNumeric.Location = New System.Drawing.Point(162, 107)
		Me.externalAttnNumeric.Name = "externalAttnNumeric"
		Me.externalAttnNumeric.Size = New System.Drawing.Size(90, 20)
		Me.externalAttnNumeric.TabIndex = 4
		' 
		' headroomNumeric
		' 
		Me.headroomNumeric.Location = New System.Drawing.Point(162, 150)
		Me.headroomNumeric.Name = "headroomNumeric"
		Me.headroomNumeric.Size = New System.Drawing.Size(90, 20)
		Me.headroomNumeric.TabIndex = 6
		' 
		' quadratureSkewNumeric
		' 
		Me.quadratureSkewNumeric.Location = New System.Drawing.Point(161, 320)
		Me.quadratureSkewNumeric.Name = "quadratureSkewNumeric"
		Me.quadratureSkewNumeric.Size = New System.Drawing.Size(90, 20)
		Me.quadratureSkewNumeric.TabIndex = 11
		' 
		' iDCOffsetNumeric
		' 
		Me.iDCOffsetNumeric.Location = New System.Drawing.Point(161, 342)
		Me.iDCOffsetNumeric.Name = "iDCOffsetNumeric"
		Me.iDCOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.iDCOffsetNumeric.TabIndex = 12
		' 
		' qDCOffsetNumeric
		' 
		Me.qDCOffsetNumeric.Location = New System.Drawing.Point(161, 364)
		Me.qDCOffsetNumeric.Name = "qDCOffsetNumeric"
		Me.qDCOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.qDCOffsetNumeric.TabIndex = 13
		' 
		' iqGaimbalanceNumeric
		' 
		Me.iqGaimbalanceNumeric.Location = New System.Drawing.Point(161, 386)
		Me.iqGaimbalanceNumeric.Name = "iqGaimbalanceNumeric"
		Me.iqGaimbalanceNumeric.Size = New System.Drawing.Size(90, 20)
		Me.iqGaimbalanceNumeric.TabIndex = 14
		' 
		' carrierFreqOffNumeric
		' 
		Me.carrierFreqOffNumeric.Location = New System.Drawing.Point(161, 405)
		Me.carrierFreqOffNumeric.Name = "carrierFreqOffNumeric"
		Me.carrierFreqOffNumeric.Size = New System.Drawing.Size(90, 20)
		Me.carrierFreqOffNumeric.TabIndex = 15
		' 
		' cnrNumeric
		' 
		Me.cnrNumeric.Location = New System.Drawing.Point(161, 447)
		Me.cnrNumeric.Name = "cnrNumeric"
		Me.cnrNumeric.Size = New System.Drawing.Size(90, 20)
		Me.cnrNumeric.TabIndex = 17
		' 
		' bdaddrLapNumeric
		' 
		Me.bdaddrLapNumeric.Location = New System.Drawing.Point(397, 21)
		Me.bdaddrLapNumeric.Name = "bdaddrLapNumeric"
		Me.bdaddrLapNumeric.Size = New System.Drawing.Size(90, 20)
		Me.bdaddrLapNumeric.TabIndex = 18
		' 
		' bdaddrUapNumeric
		' 
		Me.bdaddrUapNumeric.Location = New System.Drawing.Point(397, 44)
		Me.bdaddrUapNumeric.Name = "bdaddrUapNumeric"
		Me.bdaddrUapNumeric.Size = New System.Drawing.Size(90, 20)
		Me.bdaddrUapNumeric.TabIndex = 19
		' 
		' bdaddrNapNumeric
		' 
		Me.bdaddrNapNumeric.Location = New System.Drawing.Point(397, 63)
		Me.bdaddrNapNumeric.Name = "bdaddrNapNumeric"
		Me.bdaddrNapNumeric.Size = New System.Drawing.Size(90, 20)
		Me.bdaddrNapNumeric.TabIndex = 20
		' 
		' pktLtAddrNumeric
		' 
		Me.pktLtAddrNumeric.Location = New System.Drawing.Point(397, 162)
		Me.pktLtAddrNumeric.Name = "pktLtAddrNumeric"
		Me.pktLtAddrNumeric.Size = New System.Drawing.Size(90, 20)
		Me.pktLtAddrNumeric.TabIndex = 22
		' 
		' pktHdrFlowNumeric
		' 
		Me.pktHdrFlowNumeric.Location = New System.Drawing.Point(397, 187)
		Me.pktHdrFlowNumeric.Name = "pktHdrFlowNumeric"
		Me.pktHdrFlowNumeric.Size = New System.Drawing.Size(90, 20)
		Me.pktHdrFlowNumeric.TabIndex = 23
		' 
		' pktHdrSeqnNumeric
		' 
		Me.pktHdrSeqnNumeric.Location = New System.Drawing.Point(397, 225)
		Me.pktHdrSeqnNumeric.Name = "pktHdrSeqnNumeric"
		Me.pktHdrSeqnNumeric.Size = New System.Drawing.Size(90, 20)
		Me.pktHdrSeqnNumeric.TabIndex = 25
		' 
		' payHdrLlidNumeric
		' 
		Me.payHdrLlidNumeric.Location = New System.Drawing.Point(398, 295)
		Me.payHdrLlidNumeric.Name = "payHdrLlidNumeric"
		Me.payHdrLlidNumeric.Size = New System.Drawing.Size(90, 20)
		Me.payHdrLlidNumeric.TabIndex = 26
		' 
		' payHdrFlowNumeric
		' 
		Me.payHdrFlowNumeric.Location = New System.Drawing.Point(398, 317)
		Me.payHdrFlowNumeric.Name = "payHdrFlowNumeric"
		Me.payHdrFlowNumeric.Size = New System.Drawing.Size(90, 20)
		Me.payHdrFlowNumeric.TabIndex = 27
		' 
		' payHdrPaylenNumeric
		' 
		Me.payHdrPaylenNumeric.Location = New System.Drawing.Point(398, 359)
		Me.payHdrPaylenNumeric.Name = "payHdrPaylenNumeric"
		Me.payHdrPaylenNumeric.Size = New System.Drawing.Size(90, 20)
		Me.payHdrPaylenNumeric.TabIndex = 29
		Me.payHdrPaylenNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
		' 
		' paydatPnorderNumeric
		' 
		Me.paydatPnorderNumeric.Location = New System.Drawing.Point(397, 446)
		Me.paydatPnorderNumeric.Name = "paydatPnorderNumeric"
		Me.paydatPnorderNumeric.Size = New System.Drawing.Size(90, 20)
		Me.paydatPnorderNumeric.TabIndex = 32
		Me.paydatPnorderNumeric.Value = New Decimal(New Integer() {9, 0, 0, 0})
		' 
		' paydatSeedNumeric
		' 
		Me.paydatSeedNumeric.Location = New System.Drawing.Point(397, 468)
		Me.paydatSeedNumeric.Name = "paydatSeedNumeric"
		Me.paydatSeedNumeric.Size = New System.Drawing.Size(90, 20)
		Me.paydatSeedNumeric.TabIndex = 33
		Me.paydatSeedNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
		' 
		' whiteClkNumeric
		' 
		Me.whiteClkNumeric.Location = New System.Drawing.Point(590, 42)
		Me.whiteClkNumeric.Name = "whiteClkNumeric"
		Me.whiteClkNumeric.Size = New System.Drawing.Size(90, 20)
		Me.whiteClkNumeric.TabIndex = 35
		' 
		' rfsgResourceTextBox
		' 
		Me.rfsgResourceTextBox.Location = New System.Drawing.Point(162, 22)
		Me.rfsgResourceTextBox.Name = "rfsgResourceTextBox"
		Me.rfsgResourceTextBox.Size = New System.Drawing.Size(90, 20)
		Me.rfsgResourceTextBox.TabIndex = 0
		Me.rfsgResourceTextBox.Text = "RFSG"
		' 
		' carrierFreqTextBox
		' 
		Me.carrierFreqTextBox.Enabled = False
		Me.carrierFreqTextBox.Location = New System.Drawing.Point(162, 63)
		Me.carrierFreqTextBox.Name = "carrierFreqTextBox"
		Me.carrierFreqTextBox.Size = New System.Drawing.Size(90, 20)
		Me.carrierFreqTextBox.TabIndex = 2
		Me.carrierFreqTextBox.Text = "2.402E+9"
		' 
		' actualHeadroomTextBox
		' 
		Me.actualHeadroomTextBox.Enabled = False
		Me.actualHeadroomTextBox.Location = New System.Drawing.Point(162, 169)
		Me.actualHeadroomTextBox.Name = "actualHeadroomTextBox"
		Me.actualHeadroomTextBox.Size = New System.Drawing.Size(90, 20)
		Me.actualHeadroomTextBox.TabIndex = 7
		Me.actualHeadroomTextBox.Text = "5.00"
		' 
		' payHdrActPaylenTextBox
		' 
		Me.payHdrActPaylenTextBox.Enabled = False
		Me.payHdrActPaylenTextBox.Location = New System.Drawing.Point(399, 381)
		Me.payHdrActPaylenTextBox.Name = "payHdrActPaylenTextBox"
		Me.payHdrActPaylenTextBox.Size = New System.Drawing.Size(90, 20)
		Me.payHdrActPaylenTextBox.TabIndex = 30
		Me.payHdrActPaylenTextBox.Text = "0"
		' 
		' waveNameTextBox
		' 
		Me.waveNameTextBox.Location = New System.Drawing.Point(614, 112)
		Me.waveNameTextBox.Name = "waveNameTextBox"
		Me.waveNameTextBox.Size = New System.Drawing.Size(87, 20)
		Me.waveNameTextBox.TabIndex = 36
		Me.waveNameTextBox.Text = "Data"
		' 
		' scriptTextBox
		' 
		Me.scriptTextBox.Location = New System.Drawing.Point(531, 166)
		Me.scriptTextBox.Multiline = True
		Me.scriptTextBox.Name = "scriptTextBox"
		Me.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.scriptTextBox.Size = New System.Drawing.Size(174, 106)
		Me.scriptTextBox.TabIndex = 38
		Me.scriptTextBox.TabStop = False
		Me.scriptTextBox.Text = "script GenerateDataPkt" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    repeat forever" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       generate Data" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    end repeat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " end script"

		' 
		' errorTextBox
		' 
		Me.errorTextBox.Location = New System.Drawing.Point(531, 304)
		Me.errorTextBox.Multiline = True
		Me.errorTextBox.Name = "errorTextBox"
		Me.errorTextBox.[ReadOnly] = True
		Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.errorTextBox.Size = New System.Drawing.Size(176, 171)
		Me.errorTextBox.TabIndex = 38
		Me.errorTextBox.TabStop = False
		Me.errorTextBox.Text = "No Error"
		' 
		' generateButton
		' 
		Me.generateButton.Location = New System.Drawing.Point(186, 651)
		Me.generateButton.Name = "generateButton"
		Me.generateButton.Size = New System.Drawing.Size(75, 23)
		Me.generateButton.TabIndex = 39
		Me.generateButton.Text = "&Generate"
		Me.generateButton.UseVisualStyleBackColor = True
		AddHandler Me.generateButton.Click, New System.EventHandler(AddressOf Me.generateButton_Click)
		' 
		' stopButton
		' 
		Me.stopButton.Location = New System.Drawing.Point(469, 651)
		Me.stopButton.Name = "stopButton"
		Me.stopButton.Size = New System.Drawing.Size(75, 23)
		Me.stopButton.TabIndex = 40
		Me.stopButton.Text = "&Stop"
		Me.stopButton.UseVisualStyleBackColor = True
		AddHandler Me.stopButton.Click, New System.EventHandler(AddressOf Me.stopButton_Click)
		' 
		' autoHeadroomEnabComboBox
		' 
		Me.autoHeadroomEnabComboBox.Location = New System.Drawing.Point(162, 129)
		Me.autoHeadroomEnabComboBox.Name = "autoHeadroomEnabComboBox"
		Me.autoHeadroomEnabComboBox.Size = New System.Drawing.Size(90, 21)
		Me.autoHeadroomEnabComboBox.TabIndex = 5
		Me.autoHeadroomEnabComboBox.Text = "True"
		' 
		' refSourceComboBox
		' 
		Me.refSourceComboBox.Location = New System.Drawing.Point(161, 230)
		Me.refSourceComboBox.Name = "refSourceComboBox"
		Me.refSourceComboBox.Size = New System.Drawing.Size(90, 21)
		Me.refSourceComboBox.TabIndex = 8
		' 
		' userDefinedBitsComboBox
		' 
		Me.userDefinedBitsComboBox.Location = New System.Drawing.Point(397, 494)
		Me.userDefinedBitsComboBox.Name = "userDefinedBitsComboBox"
		Me.userDefinedBitsComboBox.Size = New System.Drawing.Size(98, 21)
		Me.userDefinedBitsComboBox.TabIndex = 8
		' 
		' clkOutTerminalComboBox
		' 
		Me.clkOutTerminalComboBox.Location = New System.Drawing.Point(161, 251)
		Me.clkOutTerminalComboBox.Name = "clkOutTerminalComboBox"
		Me.clkOutTerminalComboBox.Size = New System.Drawing.Size(90, 21)
		Me.clkOutTerminalComboBox.TabIndex = 9
		Me.clkOutTerminalComboBox.Text = "Do not export clock"
		' 
		' allIqImpairEnComboBox
		' 
		Me.allIqImpairEnComboBox.Location = New System.Drawing.Point(161, 298)
		Me.allIqImpairEnComboBox.Name = "allIqImpairEnComboBox"
		Me.allIqImpairEnComboBox.Size = New System.Drawing.Size(90, 21)
		Me.allIqImpairEnComboBox.TabIndex = 10
		Me.allIqImpairEnComboBox.Text = "False"
		' 
		' awgnEnabledComboBox
		' 
		Me.awgnEnabledComboBox.Location = New System.Drawing.Point(161, 427)
		Me.awgnEnabledComboBox.Name = "awgnEnabledComboBox"
		Me.awgnEnabledComboBox.Size = New System.Drawing.Size(90, 21)
		Me.awgnEnabledComboBox.TabIndex = 16
		Me.awgnEnabledComboBox.Text = "False"
		' 
		' pktHdrArqnComboBox
		' 
		Me.pktHdrArqnComboBox.Location = New System.Drawing.Point(397, 206)
		Me.pktHdrArqnComboBox.Name = "pktHdrArqnComboBox"
		Me.pktHdrArqnComboBox.Size = New System.Drawing.Size(90, 21)
		Me.pktHdrArqnComboBox.TabIndex = 24
		Me.pktHdrArqnComboBox.Text = "NAK"
		' 
		' packetComboBox
		' 
		Me.packetComboBox.Location = New System.Drawing.Point(397, 90)
		Me.packetComboBox.Name = "packetComboBox"
		Me.packetComboBox.Size = New System.Drawing.Size(90, 21)
		Me.packetComboBox.TabIndex = 28
		Me.packetComboBox.Text = "DH1"
		' 
		' payHdrPaylenModeComboBox
		' 
		Me.payHdrPaylenModeComboBox.Location = New System.Drawing.Point(398, 339)
		Me.payHdrPaylenModeComboBox.Name = "payHdrPaylenModeComboBox"
		Me.payHdrPaylenModeComboBox.Size = New System.Drawing.Size(90, 21)
		Me.payHdrPaylenModeComboBox.TabIndex = 28
		Me.payHdrPaylenModeComboBox.Text = "Maximum Length"
		' 
		' payHdrDatatypeComboBox
		' 
		Me.payHdrDatatypeComboBox.Location = New System.Drawing.Point(398, 426)
		Me.payHdrDatatypeComboBox.Name = "payHdrDatatypeComboBox"
		Me.payHdrDatatypeComboBox.Size = New System.Drawing.Size(90, 21)
		Me.payHdrDatatypeComboBox.TabIndex = 31
		Me.payHdrDatatypeComboBox.Text = "PN Sequence"
		' 
		' whiteEnComboBox
		' 
		Me.whiteEnComboBox.Location = New System.Drawing.Point(590, 22)
		Me.whiteEnComboBox.Name = "whiteEnComboBox"
		Me.whiteEnComboBox.Size = New System.Drawing.Size(90, 21)
		Me.whiteEnComboBox.TabIndex = 34
		Me.whiteEnComboBox.Text = "False"
		' 
		' timer
		' 
		AddHandler Me.timer.Tick, New System.EventHandler(AddressOf Me.ProcessTimerEvent)
		' 
		' numOfUniqNumeric
		' 
		Me.numOfUniqNumeric.Location = New System.Drawing.Point(397, 109)
		Me.numOfUniqNumeric.Name = "numOfUniqNumeric"
		Me.numOfUniqNumeric.Size = New System.Drawing.Size(92, 20)
		Me.numOfUniqNumeric.TabIndex = 47
		Me.numOfUniqNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
		' 
		' numOfIdleSlotsNumeric
		' 
		Me.numOfIdleSlotsNumeric.Location = New System.Drawing.Point(398, 130)
		Me.numOfIdleSlotsNumeric.Name = "numOfIdleSlotsNumeric"
		Me.numOfIdleSlotsNumeric.Size = New System.Drawing.Size(92, 20)
		Me.numOfIdleSlotsNumeric.TabIndex = 48
		Me.numOfIdleSlotsNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
		' 
		' numOfUniqLabel
		' 
		Me.numOfUniqLabel.AutoSize = True
		Me.numOfUniqLabel.Location = New System.Drawing.Point(262, 114)
		Me.numOfUniqLabel.Name = "numOfUniqLabel"
		Me.numOfUniqLabel.Size = New System.Drawing.Size(132, 13)
		Me.numOfUniqLabel.TabIndex = 49
		Me.numOfUniqLabel.Text = "Number Of Unique Packet"
		' 
		' numOfIdleLabel
		' 
		Me.numOfIdleLabel.AutoSize = True
		Me.numOfIdleLabel.Location = New System.Drawing.Point(262, 132)
		Me.numOfIdleLabel.Name = "numOfIdleLabel"
		Me.numOfIdleLabel.Size = New System.Drawing.Size(104, 13)
		Me.numOfIdleLabel.TabIndex = 50
		Me.numOfIdleLabel.Text = "Number Of Idle Slots"
		' 
		' label1
		' 
		Me.label1.AutoSize = True
		Me.label1.Location = New System.Drawing.Point(17, 471)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(61, 13)
		Me.label1.TabIndex = 52
		Me.label1.Text = "Output Port"
		' 
		' outputPortComboBox
		' 
		Me.outputPortComboBox.Location = New System.Drawing.Point(156, 471)
		Me.outputPortComboBox.Name = "outputPortComboBox"
		Me.outputPortComboBox.Size = New System.Drawing.Size(90, 21)
		Me.outputPortComboBox.TabIndex = 51
		' 
		' label2
		' 
		Me.label2.AutoSize = True
		Me.label2.Location = New System.Drawing.Point(14, 499)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(112, 13)
		Me.label2.TabIndex = 54
		Me.label2.Text = "Terminal Configuration"
		' 
		' terminalConfigurationComboBox
		' 
		Me.terminalConfigurationComboBox.Location = New System.Drawing.Point(156, 496)
		Me.terminalConfigurationComboBox.Name = "terminalConfigurationComboBox"
		Me.terminalConfigurationComboBox.Size = New System.Drawing.Size(90, 21)
		Me.terminalConfigurationComboBox.TabIndex = 53
		' 
		' iCommonModeOffsetLabel
		' 
		Me.iCommonModeOffsetLabel.AutoSize = True
		Me.iCommonModeOffsetLabel.Location = New System.Drawing.Point(16, 575)
		Me.iCommonModeOffsetLabel.Name = "iCommonModeOffsetLabel"
		Me.iCommonModeOffsetLabel.Size = New System.Drawing.Size(128, 13)
		Me.iCommonModeOffsetLabel.TabIndex = 60
		Me.iCommonModeOffsetLabel.Text = "I Common Mode Offset(V)"
		' 
		' qCommonModeOffsetLabel
		' 
		Me.qCommonModeOffsetLabel.AutoSize = True
		Me.qCommonModeOffsetLabel.Location = New System.Drawing.Point(16, 597)
		Me.qCommonModeOffsetLabel.Name = "qCommonModeOffsetLabel"
		Me.qCommonModeOffsetLabel.Size = New System.Drawing.Size(133, 13)
		Me.qCommonModeOffsetLabel.TabIndex = 62
		Me.qCommonModeOffsetLabel.Text = "Q Common Mode Offset(V)"
		' 
		' iCommonModeOffsetNumeric
		' 
		Me.iCommonModeOffsetNumeric.Location = New System.Drawing.Point(158, 573)
		Me.iCommonModeOffsetNumeric.Name = "iCommonModeOffsetNumeric"
		Me.iCommonModeOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.iCommonModeOffsetNumeric.TabIndex = 59
		' 
		' qCommonModeOffsetNumeric
		' 
		Me.qCommonModeOffsetNumeric.Location = New System.Drawing.Point(158, 595)
		Me.qCommonModeOffsetNumeric.Name = "qCommonModeOffsetNumeric"
		Me.qCommonModeOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.qCommonModeOffsetNumeric.TabIndex = 61
		' 
		' iOffsetLabel
		' 
		Me.iOffsetLabel.AutoSize = True
		Me.iOffsetLabel.Location = New System.Drawing.Point(16, 525)
		Me.iOffsetLabel.Name = "iOffsetLabel"
		Me.iOffsetLabel.Size = New System.Drawing.Size(54, 13)
		Me.iOffsetLabel.TabIndex = 64
		Me.iOffsetLabel.Text = "I Offset(V)"

		' 
		' qOffsetLabel
		' 
		Me.qOffsetLabel.AutoSize = True
		Me.qOffsetLabel.Location = New System.Drawing.Point(16, 547)
		Me.qOffsetLabel.Name = "qOffsetLabel"
		Me.qOffsetLabel.Size = New System.Drawing.Size(59, 13)
		Me.qOffsetLabel.TabIndex = 66
		Me.qOffsetLabel.Text = "Q Offset(V)"
		' 
		' iOffsetNumeric
		' 
		Me.iOffsetNumeric.Location = New System.Drawing.Point(158, 523)
		Me.iOffsetNumeric.Name = "iOffsetNumeric"
		Me.iOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.iOffsetNumeric.TabIndex = 63
		' 
		' qOffsetNumeric
		' 
		Me.qOffsetNumeric.Location = New System.Drawing.Point(156, 549)
		Me.qOffsetNumeric.Name = "qOffsetNumeric"
		Me.qOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.qOffsetNumeric.TabIndex = 65
		' 
		' MainForm
		' 
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(717, 716)
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
		Me.Controls.Add(Me.numOfIdleLabel)
		Me.Controls.Add(Me.numOfUniqLabel)
		Me.Controls.Add(Me.numOfIdleSlotsNumeric)
		Me.Controls.Add(Me.numOfUniqNumeric)
		Me.Controls.Add(Me.rfsgResourceLabel)
		Me.Controls.Add(Me.chnNumberLabel)
		Me.Controls.Add(Me.carrierFreqLabel)
		Me.Controls.Add(Me.powerLevelLabel)
		Me.Controls.Add(Me.externalAttnLabel)
		Me.Controls.Add(Me.autoHeadroomEnabLabel)
		Me.Controls.Add(Me.headroomLabel)
		Me.Controls.Add(Me.actualHeadroomLabel)
		Me.Controls.Add(Me.refSourceLabel)
		Me.Controls.Add(Me.clkOutTerminalLabel)
		Me.Controls.Add(Me.allIqImpairEnLabel)
		Me.Controls.Add(Me.quadratureSkewLabel)
		Me.Controls.Add(Me.iDCOffsetLabel)
		Me.Controls.Add(Me.qDCOffsetLabel)
		Me.Controls.Add(Me.iqGaimbalanceLabel)
		Me.Controls.Add(Me.carrierFreqOffLabel)
		Me.Controls.Add(Me.awgnEnabledLabel)
		Me.Controls.Add(Me.cnrLabel)
		Me.Controls.Add(Me.bdaddrLapLabel)
		Me.Controls.Add(Me.bdaddrUapLabel)
		Me.Controls.Add(Me.bdaddrNapLabel)
		Me.Controls.Add(Me.pktLtAddrLabel)
		Me.Controls.Add(Me.pktHdrFlowLabel)
		Me.Controls.Add(Me.pktHdrArqnLabel)
		Me.Controls.Add(Me.pktHdrSeqnLabel)
		Me.Controls.Add(Me.payHdrLlidLabel)
		Me.Controls.Add(Me.payHdrFlowLabel)
		Me.Controls.Add(Me.payHdrPaylenModeLabel)
		Me.Controls.Add(Me.userDefinedBitLabel)
		Me.Controls.Add(Me.payHdrPaylenLabel)
		Me.Controls.Add(Me.payHdrActPaylenLabel)
		Me.Controls.Add(Me.payHdrDatatypeLabel)
		Me.Controls.Add(Me.paydatPnorderLabel)
		Me.Controls.Add(Me.paydatSeedLabel)
		Me.Controls.Add(Me.whiteEnLabel)
		Me.Controls.Add(Me.whiteClkLabel)
		Me.Controls.Add(Me.waveNameLabel)
		Me.Controls.Add(Me.scriptLabel)
		Me.Controls.Add(Me.errorLabel)
		Me.Controls.Add(Me.extmsgPacket22Label)
		Me.Controls.Add(Me.textmsgPayloadHdrLabel)
		Me.Controls.Add(Me.payloadDataLabel)
		Me.Controls.Add(Me.hardwareLabel)
		Me.Controls.Add(Me.frequencySettingLabel)
		Me.Controls.Add(Me.impairmentsLabel)
		Me.Controls.Add(Me.whiteningSettingLabel)
		Me.Controls.Add(Me.bdAddressLabel)
		Me.Controls.Add(Me.packetLabel)
		Me.Controls.Add(Me.payloadLabel)
		Me.Controls.Add(Me.chnNumberNumeric)
		Me.Controls.Add(Me.powerLevelNumeric)
		Me.Controls.Add(Me.externalAttnNumeric)
		Me.Controls.Add(Me.headroomNumeric)
		Me.Controls.Add(Me.quadratureSkewNumeric)
		Me.Controls.Add(Me.iDCOffsetNumeric)
		Me.Controls.Add(Me.qDCOffsetNumeric)
		Me.Controls.Add(Me.iqGaimbalanceNumeric)
		Me.Controls.Add(Me.carrierFreqOffNumeric)
		Me.Controls.Add(Me.cnrNumeric)
		Me.Controls.Add(Me.bdaddrLapNumeric)
		Me.Controls.Add(Me.bdaddrUapNumeric)
		Me.Controls.Add(Me.bdaddrNapNumeric)
		Me.Controls.Add(Me.pktLtAddrNumeric)
		Me.Controls.Add(Me.pktHdrFlowNumeric)
		Me.Controls.Add(Me.pktHdrSeqnNumeric)
		Me.Controls.Add(Me.payHdrLlidNumeric)
		Me.Controls.Add(Me.payHdrFlowNumeric)
		Me.Controls.Add(Me.payHdrPaylenNumeric)
		Me.Controls.Add(Me.paydatPnorderNumeric)
		Me.Controls.Add(Me.paydatSeedNumeric)
		Me.Controls.Add(Me.whiteClkNumeric)
		Me.Controls.Add(Me.rfsgResourceTextBox)
		Me.Controls.Add(Me.carrierFreqTextBox)
		Me.Controls.Add(Me.actualHeadroomTextBox)
		Me.Controls.Add(Me.payHdrActPaylenTextBox)
		Me.Controls.Add(Me.waveNameTextBox)
		Me.Controls.Add(Me.scriptTextBox)
		Me.Controls.Add(Me.errorTextBox)
		Me.Controls.Add(Me.generateButton)
		Me.Controls.Add(Me.stopButton)
		Me.Controls.Add(Me.autoHeadroomEnabComboBox)
		Me.Controls.Add(Me.userDefinedBitsComboBox)
		Me.Controls.Add(Me.refSourceComboBox)
		Me.Controls.Add(Me.clkOutTerminalComboBox)
		Me.Controls.Add(Me.allIqImpairEnComboBox)
		Me.Controls.Add(Me.awgnEnabledComboBox)
		Me.Controls.Add(Me.pktHdrArqnComboBox)
		Me.Controls.Add(Me.packetComboBox)
		Me.Controls.Add(Me.payHdrPaylenModeComboBox)
		Me.Controls.Add(Me.payHdrDatatypeComboBox)
		Me.Controls.Add(Me.whiteEnComboBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.MaximizeBox = False
		Me.Name = "MainForm"
		Me.ShowIcon = False
		Me.Text = "Bluetooth Generate Data Packet Example"
		AddHandler Me.FormClosing, New System.Windows.Forms.FormClosingEventHandler(AddressOf Me.MainFormClosing)
		DirectCast(Me.chnNumberNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.externalAttnNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.headroomNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.quadratureSkewNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.iDCOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.qDCOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.iqGaimbalanceNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.carrierFreqOffNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.cnrNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.bdaddrLapNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.bdaddrUapNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.bdaddrNapNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.pktLtAddrNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.pktHdrFlowNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.pktHdrSeqnNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.payHdrLlidNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.payHdrFlowNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.payHdrPaylenNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.paydatPnorderNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.paydatSeedNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.whiteClkNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.numOfUniqNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.numOfIdleSlotsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.iCommonModeOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.qCommonModeOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.iOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.qOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
#End Region

	Private rfsgResourceLabel As System.Windows.Forms.Label
	Private chnNumberLabel As System.Windows.Forms.Label
	Private carrierFreqLabel As System.Windows.Forms.Label
	Private powerLevelLabel As System.Windows.Forms.Label
	Private externalAttnLabel As System.Windows.Forms.Label
	Private autoHeadroomEnabLabel As System.Windows.Forms.Label
	Private headroomLabel As System.Windows.Forms.Label
	Private actualHeadroomLabel As System.Windows.Forms.Label
	Private refSourceLabel As System.Windows.Forms.Label
	Private clkOutTerminalLabel As System.Windows.Forms.Label
	Private allIqImpairEnLabel As System.Windows.Forms.Label
	Private quadratureSkewLabel As System.Windows.Forms.Label
	Private iDCOffsetLabel As System.Windows.Forms.Label
	Private qDCOffsetLabel As System.Windows.Forms.Label
	Private iqGaimbalanceLabel As System.Windows.Forms.Label
	Private carrierFreqOffLabel As System.Windows.Forms.Label
	Private awgnEnabledLabel As System.Windows.Forms.Label
	Private cnrLabel As System.Windows.Forms.Label
	Private bdaddrLapLabel As System.Windows.Forms.Label
	Private bdaddrUapLabel As System.Windows.Forms.Label
	Private bdaddrNapLabel As System.Windows.Forms.Label
	Private pktLtAddrLabel As System.Windows.Forms.Label
	Private pktHdrFlowLabel As System.Windows.Forms.Label
	Private pktHdrArqnLabel As System.Windows.Forms.Label
	Private pktHdrSeqnLabel As System.Windows.Forms.Label
	Private payHdrLlidLabel As System.Windows.Forms.Label
	Private payHdrFlowLabel As System.Windows.Forms.Label
	Private userDefinedBitLabel As System.Windows.Forms.Label
	Private payHdrPaylenModeLabel As System.Windows.Forms.Label
	Private payHdrPaylenLabel As System.Windows.Forms.Label
	Private payHdrActPaylenLabel As System.Windows.Forms.Label
	Private payHdrDatatypeLabel As System.Windows.Forms.Label
	Private paydatPnorderLabel As System.Windows.Forms.Label
	Private paydatSeedLabel As System.Windows.Forms.Label
	Private whiteEnLabel As System.Windows.Forms.Label
	Private whiteClkLabel As System.Windows.Forms.Label
	Private waveNameLabel As System.Windows.Forms.Label
	Private scriptLabel As System.Windows.Forms.Label
	Private errorLabel As System.Windows.Forms.Label
	Private extmsgPacket22Label As System.Windows.Forms.Label
	Private textmsgPayloadHdrLabel As System.Windows.Forms.Label
	Private payloadDataLabel As System.Windows.Forms.Label
	Private hardwareLabel As System.Windows.Forms.Label
	Private frequencySettingLabel As System.Windows.Forms.Label
	Private impairmentsLabel As System.Windows.Forms.Label
	Private whiteningSettingLabel As System.Windows.Forms.Label
	Private bdAddressLabel As System.Windows.Forms.Label
	Private packetLabel As System.Windows.Forms.Label
	Private payloadLabel As System.Windows.Forms.Label
	Private chnNumberNumeric As System.Windows.Forms.NumericUpDown
	Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
	Private externalAttnNumeric As System.Windows.Forms.NumericUpDown
	Private headroomNumeric As System.Windows.Forms.NumericUpDown
	Private quadratureSkewNumeric As System.Windows.Forms.NumericUpDown
	Private iDCOffsetNumeric As System.Windows.Forms.NumericUpDown
	Private qDCOffsetNumeric As System.Windows.Forms.NumericUpDown
	Private iqGaimbalanceNumeric As System.Windows.Forms.NumericUpDown
	Private carrierFreqOffNumeric As System.Windows.Forms.NumericUpDown
	Private cnrNumeric As System.Windows.Forms.NumericUpDown
	Private bdaddrLapNumeric As System.Windows.Forms.NumericUpDown
	Private bdaddrUapNumeric As System.Windows.Forms.NumericUpDown
	Private bdaddrNapNumeric As System.Windows.Forms.NumericUpDown
	Private pktLtAddrNumeric As System.Windows.Forms.NumericUpDown
	Private pktHdrFlowNumeric As System.Windows.Forms.NumericUpDown
	Private pktHdrSeqnNumeric As System.Windows.Forms.NumericUpDown
	Private payHdrLlidNumeric As System.Windows.Forms.NumericUpDown
	Private payHdrFlowNumeric As System.Windows.Forms.NumericUpDown
	Private payHdrPaylenNumeric As System.Windows.Forms.NumericUpDown
	Private paydatPnorderNumeric As System.Windows.Forms.NumericUpDown
	Private paydatSeedNumeric As System.Windows.Forms.NumericUpDown
	Private whiteClkNumeric As System.Windows.Forms.NumericUpDown
	Private rfsgResourceTextBox As System.Windows.Forms.TextBox
	Private carrierFreqTextBox As System.Windows.Forms.TextBox
	Private actualHeadroomTextBox As System.Windows.Forms.TextBox
	Private payHdrActPaylenTextBox As System.Windows.Forms.TextBox
	Private waveNameTextBox As System.Windows.Forms.TextBox
	Private scriptTextBox As System.Windows.Forms.TextBox
	Private errorTextBox As System.Windows.Forms.TextBox
	Private generateButton As System.Windows.Forms.Button
	Private stopButton As System.Windows.Forms.Button
	Private autoHeadroomEnabComboBox As System.Windows.Forms.ComboBox
	Private refSourceComboBox As System.Windows.Forms.ComboBox
	Private userDefinedBitsComboBox As System.Windows.Forms.ComboBox
	Private clkOutTerminalComboBox As System.Windows.Forms.ComboBox
	Private allIqImpairEnComboBox As System.Windows.Forms.ComboBox
	Private awgnEnabledComboBox As System.Windows.Forms.ComboBox
	Private pktHdrArqnComboBox As System.Windows.Forms.ComboBox
	Private packetComboBox As System.Windows.Forms.ComboBox
	Private payHdrPaylenModeComboBox As System.Windows.Forms.ComboBox
	Private payHdrDatatypeComboBox As System.Windows.Forms.ComboBox
	Private whiteEnComboBox As System.Windows.Forms.ComboBox
	Private timer As System.Windows.Forms.Timer
	Private numOfUniqNumeric As System.Windows.Forms.NumericUpDown
	Private numOfIdleSlotsNumeric As System.Windows.Forms.NumericUpDown
	Private numOfUniqLabel As System.Windows.Forms.Label
	Private numOfIdleLabel As System.Windows.Forms.Label
	Private label1 As System.Windows.Forms.Label
	Private outputPortComboBox As System.Windows.Forms.ComboBox
	Private label2 As System.Windows.Forms.Label
	Private terminalConfigurationComboBox As System.Windows.Forms.ComboBox
	Private iCommonModeOffsetLabel As System.Windows.Forms.Label
	Private qCommonModeOffsetLabel As System.Windows.Forms.Label
	Private iCommonModeOffsetNumeric As System.Windows.Forms.NumericUpDown
	Private qCommonModeOffsetNumeric As System.Windows.Forms.NumericUpDown
	Private iOffsetLabel As System.Windows.Forms.Label
	Private qOffsetLabel As System.Windows.Forms.Label
	Private iOffsetNumeric As System.Windows.Forms.NumericUpDown
	Private qOffsetNumeric As System.Windows.Forms.NumericUpDown
End Class
