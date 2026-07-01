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
		Me.clkOutTerminalLabel = New System.Windows.Forms.Label()
		Me.allIqImpairEnLabel = New System.Windows.Forms.Label()
		Me.quadratureSkewLabel = New System.Windows.Forms.Label()
		Me.iDcOffsetLabel = New System.Windows.Forms.Label()
		Me.qDcOffsetLabel = New System.Windows.Forms.Label()
		Me.iqGaimbalanceLabel = New System.Windows.Forms.Label()
		Me.carrierFreqOffLabel = New System.Windows.Forms.Label()
		Me.awgnEnabledLabel = New System.Windows.Forms.Label()
		Me.cnrLabel = New System.Windows.Forms.Label()
		Me.bdaddrLapLabel = New System.Windows.Forms.Label()
		Me.bdaddrUapLabel = New System.Windows.Forms.Label()
		Me.bdaddrNapLabel = New System.Windows.Forms.Label()
		Me.payHdrLlidLabel = New System.Windows.Forms.Label()
		Me.payHdrFlowLabel = New System.Windows.Forms.Label()
		Me.payHdrPaylenModeLabel = New System.Windows.Forms.Label()
		Me.payHdrPaylenLabel = New System.Windows.Forms.Label()
		Me.payHdrActPaylenLabel = New System.Windows.Forms.Label()
		Me.payHdrDatatypeLabel = New System.Windows.Forms.Label()
		Me.paydatPnorderLabel = New System.Windows.Forms.Label()
		Me.paydatSeedLabel = New System.Windows.Forms.Label()
		Me.dvPayhdrDatatypeLabel = New System.Windows.Forms.Label()
		Me.dvPaydatPnorderLabel = New System.Windows.Forms.Label()
		Me.dvPaydatSeedLabel = New System.Windows.Forms.Label()
		Me.pktLtAddrLabel = New System.Windows.Forms.Label()
		Me.pktHdrFlowLabel = New System.Windows.Forms.Label()
		Me.pktHdrArqnLabel = New System.Windows.Forms.Label()
		Me.pktHdrSeqnLabel = New System.Windows.Forms.Label()
		Me.whiteEnLabel = New System.Windows.Forms.Label()
		Me.whiteClkLabel = New System.Windows.Forms.Label()
		Me.waveNameLabel = New System.Windows.Forms.Label()
		Me.scriptLabel = New System.Windows.Forms.Label()
		Me.errorLabel = New System.Windows.Forms.Label()
		Me.payloadVoiceLabel = New System.Windows.Forms.Label()
		Me.textmsgPayloadHdrLabel = New System.Windows.Forms.Label()
		Me.payloadDataLabel = New System.Windows.Forms.Label()
		Me.hardwareLabel = New System.Windows.Forms.Label()
		Me.frequencySettingLabel = New System.Windows.Forms.Label()
		Me.impairmentsLabel = New System.Windows.Forms.Label()
		Me.bdAddressLabel = New System.Windows.Forms.Label()
		Me.payloadControlLabel = New System.Windows.Forms.Label()
		Me.packetHeaderLabel = New System.Windows.Forms.Label()
		Me.whiteningSettingLabel = New System.Windows.Forms.Label()
		Me.chnNumberNumeric = New System.Windows.Forms.NumericUpDown()
		Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
		Me.externalAttnNumeric = New System.Windows.Forms.NumericUpDown()
		Me.headroomNumeric = New System.Windows.Forms.NumericUpDown()
		Me.quadratureSkewNumeric = New System.Windows.Forms.NumericUpDown()
		Me.iDcOffsetNumeric = New System.Windows.Forms.NumericUpDown()
		Me.qDcOffsetNumeric = New System.Windows.Forms.NumericUpDown()
		Me.iqGaimbalanceNumeric = New System.Windows.Forms.NumericUpDown()
		Me.carrierFreqOffNumeric = New System.Windows.Forms.NumericUpDown()
		Me.cnrNumeric = New System.Windows.Forms.NumericUpDown()
		Me.bdaddrLapNumeric = New System.Windows.Forms.NumericUpDown()
		Me.bdaddrUapNumeric = New System.Windows.Forms.NumericUpDown()
		Me.bdaddrNapNumeric = New System.Windows.Forms.NumericUpDown()
		Me.payHdrLlidNumeric = New System.Windows.Forms.NumericUpDown()
		Me.payHdrFlowNumeric = New System.Windows.Forms.NumericUpDown()
		Me.payHdrPaylenNumeric = New System.Windows.Forms.NumericUpDown()
		Me.paydatPnorderNumeric = New System.Windows.Forms.NumericUpDown()
		Me.paydatSeedNumeric = New System.Windows.Forms.NumericUpDown()
		Me.dvPaydatPnorderNumeric = New System.Windows.Forms.NumericUpDown()
		Me.dvPaydatSeedNumeric = New System.Windows.Forms.NumericUpDown()
		Me.pktLtAddrNumeric = New System.Windows.Forms.NumericUpDown()
		Me.pktHdrFlowNumeric = New System.Windows.Forms.NumericUpDown()
		Me.pktHdrSeqnNumeric = New System.Windows.Forms.NumericUpDown()
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
		Me.clkOutTerminalComboBox = New System.Windows.Forms.ComboBox()
		Me.allIqImpairEnComboBox = New System.Windows.Forms.ComboBox()
		Me.awgnEnabledComboBox = New System.Windows.Forms.ComboBox()
		Me.payHdrPaylenModeComboBox = New System.Windows.Forms.ComboBox()
		Me.payHdrDatatypeComboBox = New System.Windows.Forms.ComboBox()
		Me.dvPayhdrDatatypeComboBox = New System.Windows.Forms.ComboBox()
		Me.pktHdrArqnComboBox = New System.Windows.Forms.ComboBox()
		Me.whiteEnComboBox = New System.Windows.Forms.ComboBox()
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
		DirectCast(Me.chnNumberNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.externalAttnNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.headroomNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.quadratureSkewNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.iDcOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.qDcOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.iqGaimbalanceNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.carrierFreqOffNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.cnrNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.bdaddrLapNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.bdaddrUapNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.bdaddrNapNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.payHdrLlidNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.payHdrFlowNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.payHdrPaylenNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.paydatPnorderNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.paydatSeedNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.dvPaydatPnorderNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.dvPaydatSeedNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.pktLtAddrNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.pktHdrFlowNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.pktHdrSeqnNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.whiteClkNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.iOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.qOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.iCommonModeOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.qCommonModeOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		' 
		' rfsgResourceLabel
		' 
		Me.rfsgResourceLabel.AutoSize = True
		Me.rfsgResourceLabel.Location = New System.Drawing.Point(18, 29)
		Me.rfsgResourceLabel.Name = "rfsgResourceLabel"
		Me.rfsgResourceLabel.Size = New System.Drawing.Size(85, 13)
		Me.rfsgResourceLabel.TabIndex = 0
		Me.rfsgResourceLabel.Text = "RFSG Resource"
		' 
		' chnNumberLabel
		' 
		Me.chnNumberLabel.AutoSize = True
		Me.chnNumberLabel.Location = New System.Drawing.Point(18, 46)
		Me.chnNumberLabel.Name = "chnNumberLabel"
		Me.chnNumberLabel.Size = New System.Drawing.Size(86, 13)
		Me.chnNumberLabel.TabIndex = 1
		Me.chnNumberLabel.Text = "Channel Number"
		' 
		' carrierFreqLabel
		' 
		Me.carrierFreqLabel.AutoSize = True
		Me.carrierFreqLabel.Location = New System.Drawing.Point(18, 66)
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
		' autoheadroomEnabLabel
		' 
		Me.autoheadroomEnabLabel.AutoSize = True
		Me.autoheadroomEnabLabel.Location = New System.Drawing.Point(17, 132)
		Me.autoheadroomEnabLabel.Name = "autoheadroomEnabLabel"
		Me.autoheadroomEnabLabel.Size = New System.Drawing.Size(123, 13)
		Me.autoheadroomEnabLabel.TabIndex = 5
		Me.autoheadroomEnabLabel.Text = "Auto Headroom Enabled"
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
		Me.refSourceLabel.Location = New System.Drawing.Point(17, 235)
		Me.refSourceLabel.Name = "refSourceLabel"
		Me.refSourceLabel.Size = New System.Drawing.Size(94, 13)
		Me.refSourceLabel.TabIndex = 8
		Me.refSourceLabel.Text = "Reference Source"
		' 
		' clkOutTerminalLabel
		' 
		Me.clkOutTerminalLabel.AutoSize = True
		Me.clkOutTerminalLabel.Location = New System.Drawing.Point(17, 256)
		Me.clkOutTerminalLabel.Name = "clkOutTerminalLabel"
		Me.clkOutTerminalLabel.Size = New System.Drawing.Size(67, 13)
		Me.clkOutTerminalLabel.TabIndex = 9
		Me.clkOutTerminalLabel.Text = "Export Clock"
		' 
		' allIqImpairEnLabel
		' 
		Me.allIqImpairEnLabel.AutoSize = True
		Me.allIqImpairEnLabel.Location = New System.Drawing.Point(17, 304)
		Me.allIqImpairEnLabel.Name = "allIqImpairEnLabel"
		Me.allIqImpairEnLabel.Size = New System.Drawing.Size(133, 13)
		Me.allIqImpairEnLabel.TabIndex = 10
		Me.allIqImpairEnLabel.Text = "All IQ Impairments Enabled"
		' 
		' quadratureSkewLabel
		' 
		Me.quadratureSkewLabel.AutoSize = True
		Me.quadratureSkewLabel.Location = New System.Drawing.Point(17, 326)
		Me.quadratureSkewLabel.Name = "quadratureSkewLabel"
		Me.quadratureSkewLabel.Size = New System.Drawing.Size(117, 13)
		Me.quadratureSkewLabel.TabIndex = 11
		Me.quadratureSkewLabel.Text = "Quadrature Skew (deg)"
		' 
		' iDcOffsetLabel
		' 
		Me.iDcOffsetLabel.AutoSize = True
		Me.iDcOffsetLabel.Location = New System.Drawing.Point(17, 348)
		Me.iDcOffsetLabel.Name = "iDcOffsetLabel"
		Me.iDcOffsetLabel.Size = New System.Drawing.Size(76, 13)
		Me.iDcOffsetLabel.TabIndex = 12
		Me.iDcOffsetLabel.Text = "I DC Offset (%)"
		' 
		' qDcOffsetLabel
		' 
		Me.qDcOffsetLabel.AutoSize = True
		Me.qDcOffsetLabel.Location = New System.Drawing.Point(17, 370)
		Me.qDcOffsetLabel.Name = "qDcOffsetLabel"
		Me.qDcOffsetLabel.Size = New System.Drawing.Size(81, 13)
		Me.qDcOffsetLabel.TabIndex = 13
		Me.qDcOffsetLabel.Text = "Q DC Offset (%)"
		' 
		' iqGaimbalanceLabel
		' 
		Me.iqGaimbalanceLabel.AutoSize = True
		Me.iqGaimbalanceLabel.Location = New System.Drawing.Point(17, 392)
		Me.iqGaimbalanceLabel.Name = "iqGaimbalanceLabel"
		Me.iqGaimbalanceLabel.Size = New System.Drawing.Size(117, 13)
		Me.iqGaimbalanceLabel.TabIndex = 14
		Me.iqGaimbalanceLabel.Text = "IQ Gain Imbalance (dB)"
		' 
		' carrierFreqOffLabel
		' 
		Me.carrierFreqOffLabel.AutoSize = True
		Me.carrierFreqOffLabel.Location = New System.Drawing.Point(17, 412)
		Me.carrierFreqOffLabel.Name = "carrierFreqOffLabel"
		Me.carrierFreqOffLabel.Size = New System.Drawing.Size(143, 13)
		Me.carrierFreqOffLabel.TabIndex = 15
		Me.carrierFreqOffLabel.Text = "Carrier Frequency Offset (Hz)"
		' 
		' awgnEnabledLabel
		' 
		Me.awgnEnabledLabel.AutoSize = True
		Me.awgnEnabledLabel.Location = New System.Drawing.Point(17, 433)
		Me.awgnEnabledLabel.Name = "awgnEnabledLabel"
		Me.awgnEnabledLabel.Size = New System.Drawing.Size(83, 13)
		Me.awgnEnabledLabel.TabIndex = 16
		Me.awgnEnabledLabel.Text = "AWGN Enabled"
		' 
		' cnrLabel
		' 
		Me.cnrLabel.AutoSize = True
		Me.cnrLabel.Location = New System.Drawing.Point(17, 453)
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
		' payHdrLlidLabel
		' 
		Me.payHdrLlidLabel.AutoSize = True
		Me.payHdrLlidLabel.Location = New System.Drawing.Point(280, 140)
		Me.payHdrLlidLabel.Name = "payHdrLlidLabel"
		Me.payHdrLlidLabel.Size = New System.Drawing.Size(30, 13)
		Me.payHdrLlidLabel.TabIndex = 21
		Me.payHdrLlidLabel.Text = "LLID"
		' 
		' payHdrFlowLabel
		' 
		Me.payHdrFlowLabel.AutoSize = True
		Me.payHdrFlowLabel.Location = New System.Drawing.Point(280, 162)
		Me.payHdrFlowLabel.Name = "payHdrFlowLabel"
		Me.payHdrFlowLabel.Size = New System.Drawing.Size(29, 13)
		Me.payHdrFlowLabel.TabIndex = 22
		Me.payHdrFlowLabel.Text = "Flow"
		' 
		' payHdrPaylenModeLabel
		' 
		Me.payHdrPaylenModeLabel.AutoSize = True
		Me.payHdrPaylenModeLabel.Location = New System.Drawing.Point(280, 184)
		Me.payHdrPaylenModeLabel.Name = "payHdrPaylenModeLabel"
		Me.payHdrPaylenModeLabel.Size = New System.Drawing.Size(111, 13)
		Me.payHdrPaylenModeLabel.TabIndex = 23
		Me.payHdrPaylenModeLabel.Text = "Payload Length Mode"
		' 
		' payHdrPaylenLabel
		' 
		Me.payHdrPaylenLabel.AutoSize = True
		Me.payHdrPaylenLabel.Location = New System.Drawing.Point(280, 204)
		Me.payHdrPaylenLabel.Name = "payHdrPaylenLabel"
		Me.payHdrPaylenLabel.Size = New System.Drawing.Size(81, 13)
		Me.payHdrPaylenLabel.TabIndex = 24
		Me.payHdrPaylenLabel.Text = "Payload Length"
		' 
		' payHdrActPaylenLabel
		' 
		Me.payHdrActPaylenLabel.AutoSize = True
		Me.payHdrActPaylenLabel.Location = New System.Drawing.Point(280, 226)
		Me.payHdrActPaylenLabel.Name = "payHdrActPaylenLabel"
		Me.payHdrActPaylenLabel.Size = New System.Drawing.Size(114, 13)
		Me.payHdrActPaylenLabel.TabIndex = 25
		Me.payHdrActPaylenLabel.Text = "Actual Payload Length"
		' 
		' payHdrDatatypeLabel
		' 
		Me.payHdrDatatypeLabel.AutoSize = True
		Me.payHdrDatatypeLabel.Location = New System.Drawing.Point(280, 274)
		Me.payHdrDatatypeLabel.Name = "payHdrDatatypeLabel"
		Me.payHdrDatatypeLabel.Size = New System.Drawing.Size(57, 13)
		Me.payHdrDatatypeLabel.TabIndex = 26
		Me.payHdrDatatypeLabel.Text = "Data Type"
		' 
		' paydatPnorderLabel
		' 
		Me.paydatPnorderLabel.AutoSize = True
		Me.paydatPnorderLabel.Location = New System.Drawing.Point(280, 294)
		Me.paydatPnorderLabel.Name = "paydatPnorderLabel"
		Me.paydatPnorderLabel.Size = New System.Drawing.Size(51, 13)
		Me.paydatPnorderLabel.TabIndex = 27
		Me.paydatPnorderLabel.Text = "PN Order"
		' 
		' paydatSeedLabel
		' 
		Me.paydatSeedLabel.AutoSize = True
		Me.paydatSeedLabel.Location = New System.Drawing.Point(280, 312)
		Me.paydatSeedLabel.Name = "paydatSeedLabel"
		Me.paydatSeedLabel.Size = New System.Drawing.Size(32, 13)
		Me.paydatSeedLabel.TabIndex = 28
		Me.paydatSeedLabel.Text = "Seed"
		' 
		' dvPayhdrDatatypeLabel
		' 
		Me.dvPayhdrDatatypeLabel.AutoSize = True
		Me.dvPayhdrDatatypeLabel.Location = New System.Drawing.Point(278, 362)
		Me.dvPayhdrDatatypeLabel.Name = "dvPayhdrDatatypeLabel"
		Me.dvPayhdrDatatypeLabel.Size = New System.Drawing.Size(57, 13)
		Me.dvPayhdrDatatypeLabel.TabIndex = 29
		Me.dvPayhdrDatatypeLabel.Text = "Data Type"
		' 
		' dvPaydatPnorderLabel
		' 
		Me.dvPaydatPnorderLabel.AutoSize = True
		Me.dvPaydatPnorderLabel.Location = New System.Drawing.Point(280, 383)
		Me.dvPaydatPnorderLabel.Name = "dvPaydatPnorderLabel"
		Me.dvPaydatPnorderLabel.Size = New System.Drawing.Size(51, 13)
		Me.dvPaydatPnorderLabel.TabIndex = 30
		Me.dvPaydatPnorderLabel.Text = "PN Order"
		' 
		' dvPaydatSeedLabel
		' 
		Me.dvPaydatSeedLabel.AutoSize = True
		Me.dvPaydatSeedLabel.Location = New System.Drawing.Point(280, 401)
		Me.dvPaydatSeedLabel.Name = "dvPaydatSeedLabel"
		Me.dvPaydatSeedLabel.Size = New System.Drawing.Size(32, 13)
		Me.dvPaydatSeedLabel.TabIndex = 31
		Me.dvPaydatSeedLabel.Text = "Seed"
		' 
		' pktLtAddrLabel
		' 
		Me.pktLtAddrLabel.AutoSize = True
		Me.pktLtAddrLabel.Location = New System.Drawing.Point(517, 25)
		Me.pktLtAddrLabel.Name = "pktLtAddrLabel"
		Me.pktLtAddrLabel.Size = New System.Drawing.Size(61, 13)
		Me.pktLtAddrLabel.TabIndex = 32
		Me.pktLtAddrLabel.Text = "LT Address"
		' 
		' pktHdrFlowLabel
		' 
		Me.pktHdrFlowLabel.AutoSize = True
		Me.pktHdrFlowLabel.Location = New System.Drawing.Point(517, 50)
		Me.pktHdrFlowLabel.Name = "pktHdrFlowLabel"
		Me.pktHdrFlowLabel.Size = New System.Drawing.Size(29, 13)
		Me.pktHdrFlowLabel.TabIndex = 33
		Me.pktHdrFlowLabel.Text = "Flow"
		' 
		' pktHdrArqnLabel
		' 
		Me.pktHdrArqnLabel.AutoSize = True
		Me.pktHdrArqnLabel.Location = New System.Drawing.Point(517, 69)
		Me.pktHdrArqnLabel.Name = "pktHdrArqnLabel"
		Me.pktHdrArqnLabel.Size = New System.Drawing.Size(38, 13)
		Me.pktHdrArqnLabel.TabIndex = 34
		Me.pktHdrArqnLabel.Text = "ARQN"
		' 
		' pktHdrSeqnLabel
		' 
		Me.pktHdrSeqnLabel.AutoSize = True
		Me.pktHdrSeqnLabel.Location = New System.Drawing.Point(517, 88)
		Me.pktHdrSeqnLabel.Name = "pktHdrSeqnLabel"
		Me.pktHdrSeqnLabel.Size = New System.Drawing.Size(37, 13)
		Me.pktHdrSeqnLabel.TabIndex = 35
		Me.pktHdrSeqnLabel.Text = "SEQN"
		' 
		' whiteEnLabel
		' 
		Me.whiteEnLabel.AutoSize = True
		Me.whiteEnLabel.Location = New System.Drawing.Point(526, 157)
		Me.whiteEnLabel.Name = "whiteEnLabel"
		Me.whiteEnLabel.Size = New System.Drawing.Size(46, 13)
		Me.whiteEnLabel.TabIndex = 36
		Me.whiteEnLabel.Text = "Enabled"
		' 
		' whiteClkLabel
		' 
		Me.whiteClkLabel.AutoSize = True
		Me.whiteClkLabel.Location = New System.Drawing.Point(526, 177)
		Me.whiteClkLabel.Name = "whiteClkLabel"
		Me.whiteClkLabel.Size = New System.Drawing.Size(34, 13)
		Me.whiteClkLabel.TabIndex = 37
		Me.whiteClkLabel.Text = "Clock"
		' 
		' waveNameLabel
		' 
		Me.waveNameLabel.AutoSize = True
		Me.waveNameLabel.Location = New System.Drawing.Point(524, 265)
		Me.waveNameLabel.Name = "waveNameLabel"
		Me.waveNameLabel.Size = New System.Drawing.Size(87, 13)
		Me.waveNameLabel.TabIndex = 38
		Me.waveNameLabel.Text = "Waveform Name"
		' 
		' scriptLabel
		' 
		Me.scriptLabel.AutoSize = True
		Me.scriptLabel.Location = New System.Drawing.Point(524, 295)
		Me.scriptLabel.Name = "scriptLabel"
		Me.scriptLabel.Size = New System.Drawing.Size(34, 13)
		Me.scriptLabel.TabIndex = 36
		Me.scriptLabel.Text = "Script"
		' 
		' errorLabel
		' 
		Me.errorLabel.AutoSize = True
		Me.errorLabel.Location = New System.Drawing.Point(529, 416)
		Me.errorLabel.Name = "errorLabel"
		Me.errorLabel.Size = New System.Drawing.Size(29, 13)
		Me.errorLabel.TabIndex = 39
		Me.errorLabel.Text = "Error"
		' 
		' payloadVoiceLabel
		' 
		Me.payloadVoiceLabel.AutoSize = True
		Me.payloadVoiceLabel.Location = New System.Drawing.Point(287, 337)
		Me.payloadVoiceLabel.Name = "payloadVoiceLabel"
		Me.payloadVoiceLabel.Size = New System.Drawing.Size(34, 13)
		Me.payloadVoiceLabel.TabIndex = 40
		Me.payloadVoiceLabel.Text = "Voice"
		' 
		' textmsgPayloadHdrLabel
		' 
		Me.textmsgPayloadHdrLabel.AutoSize = True
		Me.textmsgPayloadHdrLabel.Location = New System.Drawing.Point(280, 120)
		Me.textmsgPayloadHdrLabel.Name = "textmsgPayloadHdrLabel"
		Me.textmsgPayloadHdrLabel.Size = New System.Drawing.Size(42, 13)
		Me.textmsgPayloadHdrLabel.TabIndex = 41
		Me.textmsgPayloadHdrLabel.Text = "Header"
		' 
		' payloadDataLabel
		' 
		Me.payloadDataLabel.AutoSize = True
		Me.payloadDataLabel.Location = New System.Drawing.Point(280, 252)
		Me.payloadDataLabel.Name = "payloadDataLabel"
		Me.payloadDataLabel.Size = New System.Drawing.Size(30, 13)
		Me.payloadDataLabel.TabIndex = 42
		Me.payloadDataLabel.Text = "Data"
		' 
		' hardwareLabel
		' 
		Me.hardwareLabel.AutoSize = True
		Me.hardwareLabel.Location = New System.Drawing.Point(29, 8)
		Me.hardwareLabel.Name = "hardwareLabel"
		Me.hardwareLabel.Size = New System.Drawing.Size(53, 13)
		Me.hardwareLabel.TabIndex = 43
		Me.hardwareLabel.Text = "Hardware"
		' 
		' frequencySettingLabel
		' 
		Me.frequencySettingLabel.AutoSize = True
		Me.frequencySettingLabel.Location = New System.Drawing.Point(29, 217)
		Me.frequencySettingLabel.Name = "frequencySettingLabel"
		Me.frequencySettingLabel.Size = New System.Drawing.Size(98, 13)
		Me.frequencySettingLabel.TabIndex = 44
		Me.frequencySettingLabel.Text = "Frequency Settings"
		' 
		' impairmentsLabel
		' 
		Me.impairmentsLabel.AutoSize = True
		Me.impairmentsLabel.Location = New System.Drawing.Point(29, 286)
		Me.impairmentsLabel.Name = "impairmentsLabel"
		Me.impairmentsLabel.Size = New System.Drawing.Size(63, 13)
		Me.impairmentsLabel.TabIndex = 45
		Me.impairmentsLabel.Text = "Impairments"
		' 
		' bdAddressLabel
		' 
		Me.bdAddressLabel.AutoSize = True
		Me.bdAddressLabel.Location = New System.Drawing.Point(303, 8)
		Me.bdAddressLabel.Name = "bdAddressLabel"
		Me.bdAddressLabel.Size = New System.Drawing.Size(63, 13)
		Me.bdAddressLabel.TabIndex = 46
		Me.bdAddressLabel.Text = "BD Address"
		' 
		' payloadControlLabel
		' 
		Me.payloadControlLabel.AutoSize = True
		Me.payloadControlLabel.Location = New System.Drawing.Point(303, 106)
		Me.payloadControlLabel.Name = "payloadControlLabel"
		Me.payloadControlLabel.Size = New System.Drawing.Size(81, 13)
		Me.payloadControlLabel.TabIndex = 47
		Me.payloadControlLabel.Text = "Payload Control"
		' 
		' packetHeaderLabel
		' 
		Me.packetHeaderLabel.AutoSize = True
		Me.packetHeaderLabel.Location = New System.Drawing.Point(537, 8)
		Me.packetHeaderLabel.Name = "packetHeaderLabel"
		Me.packetHeaderLabel.Size = New System.Drawing.Size(79, 13)
		Me.packetHeaderLabel.TabIndex = 48
		Me.packetHeaderLabel.Text = "Packet Header"
		' 
		' whiteningSettingLabel
		' 
		Me.whiteningSettingLabel.AutoSize = True
		Me.whiteningSettingLabel.Location = New System.Drawing.Point(537, 131)
		Me.whiteningSettingLabel.Name = "whiteningSettingLabel"
		Me.whiteningSettingLabel.Size = New System.Drawing.Size(96, 13)
		Me.whiteningSettingLabel.TabIndex = 49
		Me.whiteningSettingLabel.Text = "Whitening Settings"
		' 
		' chnNumberNumeric
		' 
		Me.chnNumberNumeric.Location = New System.Drawing.Point(162, 43)
		Me.chnNumberNumeric.Maximum = New Decimal(New Integer() {79, 0, 0, 0})
		Me.chnNumberNumeric.Name = "chnNumberNumeric"
		Me.chnNumberNumeric.Size = New System.Drawing.Size(89, 20)
		Me.chnNumberNumeric.TabIndex = 1
		' 
		' powerLevelNumeric
		' 
		Me.powerLevelNumeric.DecimalPlaces = 2
		Me.powerLevelNumeric.Location = New System.Drawing.Point(162, 85)
		Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.powerLevelNumeric.Name = "powerLevelNumeric"
		Me.powerLevelNumeric.Size = New System.Drawing.Size(90, 20)
		Me.powerLevelNumeric.TabIndex = 3
		' 
		' externalAttnNumeric
		' 
		Me.externalAttnNumeric.DecimalPlaces = 2
		Me.externalAttnNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
		Me.externalAttnNumeric.Location = New System.Drawing.Point(162, 107)
		Me.externalAttnNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.externalAttnNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.externalAttnNumeric.Name = "externalAttnNumeric"
		Me.externalAttnNumeric.Size = New System.Drawing.Size(90, 20)
		Me.externalAttnNumeric.TabIndex = 4
		' 
		' headroomNumeric
		' 
		Me.headroomNumeric.DecimalPlaces = 2
		Me.headroomNumeric.Location = New System.Drawing.Point(162, 150)
		Me.headroomNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.headroomNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.headroomNumeric.Name = "headroomNumeric"
		Me.headroomNumeric.Size = New System.Drawing.Size(90, 20)
		Me.headroomNumeric.TabIndex = 6
		' 
		' quadratureSkewNumeric
		' 
		Me.quadratureSkewNumeric.DecimalPlaces = 2
		Me.quadratureSkewNumeric.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
		Me.quadratureSkewNumeric.Location = New System.Drawing.Point(162, 325)
		Me.quadratureSkewNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.quadratureSkewNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.quadratureSkewNumeric.Name = "quadratureSkewNumeric"
		Me.quadratureSkewNumeric.Size = New System.Drawing.Size(90, 20)
		Me.quadratureSkewNumeric.TabIndex = 11
		' 
		' iDcOffsetNumeric
		' 
		Me.iDcOffsetNumeric.DecimalPlaces = 2
		Me.iDcOffsetNumeric.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
		Me.iDcOffsetNumeric.Location = New System.Drawing.Point(162, 347)
		Me.iDcOffsetNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.iDcOffsetNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.iDcOffsetNumeric.Name = "iDcOffsetNumeric"
		Me.iDcOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.iDcOffsetNumeric.TabIndex = 12
		' 
		' qDcOffsetNumeric
		' 
		Me.qDcOffsetNumeric.DecimalPlaces = 2
		Me.qDcOffsetNumeric.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
		Me.qDcOffsetNumeric.Location = New System.Drawing.Point(162, 369)
		Me.qDcOffsetNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.qDcOffsetNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.qDcOffsetNumeric.Name = "qDcOffsetNumeric"
		Me.qDcOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.qDcOffsetNumeric.TabIndex = 13
		' 
		' iqGaimbalanceNumeric
		' 
		Me.iqGaimbalanceNumeric.DecimalPlaces = 2
		Me.iqGaimbalanceNumeric.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
		Me.iqGaimbalanceNumeric.Location = New System.Drawing.Point(162, 391)
		Me.iqGaimbalanceNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.iqGaimbalanceNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.iqGaimbalanceNumeric.Name = "iqGaimbalanceNumeric"
		Me.iqGaimbalanceNumeric.Size = New System.Drawing.Size(90, 20)
		Me.iqGaimbalanceNumeric.TabIndex = 14
		' 
		' carrierFreqOffNumeric
		' 
		Me.carrierFreqOffNumeric.DecimalPlaces = 3
		Me.carrierFreqOffNumeric.Location = New System.Drawing.Point(162, 410)
		Me.carrierFreqOffNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.carrierFreqOffNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.carrierFreqOffNumeric.Name = "carrierFreqOffNumeric"
		Me.carrierFreqOffNumeric.Size = New System.Drawing.Size(90, 20)
		Me.carrierFreqOffNumeric.TabIndex = 15
		' 
		' cnrNumeric
		' 
		Me.cnrNumeric.DecimalPlaces = 2
		Me.cnrNumeric.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
		Me.cnrNumeric.Location = New System.Drawing.Point(162, 452)
		Me.cnrNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.cnrNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.cnrNumeric.Name = "cnrNumeric"
		Me.cnrNumeric.Size = New System.Drawing.Size(90, 20)
		Me.cnrNumeric.TabIndex = 17
		Me.cnrNumeric.Value = New Decimal(New Integer() {50, 0, 0, 0})
		' 
		' bdaddrLapNumeric
		' 
		Me.bdaddrLapNumeric.Location = New System.Drawing.Point(397, 21)
		Me.bdaddrLapNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.bdaddrLapNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.bdaddrLapNumeric.Name = "bdaddrLapNumeric"
		Me.bdaddrLapNumeric.Size = New System.Drawing.Size(90, 20)
		Me.bdaddrLapNumeric.TabIndex = 18
		' 
		' bdaddrUapNumeric
		' 
		Me.bdaddrUapNumeric.Location = New System.Drawing.Point(397, 44)
		Me.bdaddrUapNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.bdaddrUapNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.bdaddrUapNumeric.Name = "bdaddrUapNumeric"
		Me.bdaddrUapNumeric.Size = New System.Drawing.Size(90, 20)
		Me.bdaddrUapNumeric.TabIndex = 19
		' 
		' bdaddrNapNumeric
		' 
		Me.bdaddrNapNumeric.Increment = New Decimal(New Integer() {0, 0, 0, 0})
		Me.bdaddrNapNumeric.Location = New System.Drawing.Point(397, 63)
		Me.bdaddrNapNumeric.Maximum = New Decimal(New Integer() {30, 0, 0, 0})
		Me.bdaddrNapNumeric.Name = "bdaddrNapNumeric"
		Me.bdaddrNapNumeric.Size = New System.Drawing.Size(90, 20)
		Me.bdaddrNapNumeric.TabIndex = 20
		' 
		' payHdrLlidNumeric
		' 
		Me.payHdrLlidNumeric.Location = New System.Drawing.Point(398, 139)
		Me.payHdrLlidNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.payHdrLlidNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.payHdrLlidNumeric.Name = "payHdrLlidNumeric"
		Me.payHdrLlidNumeric.Size = New System.Drawing.Size(90, 20)
		Me.payHdrLlidNumeric.TabIndex = 21
		' 
		' payHdrFlowNumeric
		' 
		Me.payHdrFlowNumeric.Location = New System.Drawing.Point(398, 161)
		Me.payHdrFlowNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.payHdrFlowNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.payHdrFlowNumeric.Name = "payHdrFlowNumeric"
		Me.payHdrFlowNumeric.Size = New System.Drawing.Size(90, 20)
		Me.payHdrFlowNumeric.TabIndex = 22
		' 
		' payHdrPaylenNumeric
		' 
		Me.payHdrPaylenNumeric.Location = New System.Drawing.Point(398, 203)
		Me.payHdrPaylenNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.payHdrPaylenNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.payHdrPaylenNumeric.Name = "payHdrPaylenNumeric"
		Me.payHdrPaylenNumeric.Size = New System.Drawing.Size(90, 20)
		Me.payHdrPaylenNumeric.TabIndex = 24
		' 
		' paydatPnorderNumeric
		' 
		Me.paydatPnorderNumeric.Location = New System.Drawing.Point(397, 288)
		Me.paydatPnorderNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.paydatPnorderNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.paydatPnorderNumeric.Name = "paydatPnorderNumeric"
		Me.paydatPnorderNumeric.Size = New System.Drawing.Size(90, 20)
		Me.paydatPnorderNumeric.TabIndex = 27
		Me.paydatPnorderNumeric.Value = New Decimal(New Integer() {9, 0, 0, 0})
		' 
		' paydatSeedNumeric
		' 
		Me.paydatSeedNumeric.Location = New System.Drawing.Point(397, 309)
		Me.paydatSeedNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.paydatSeedNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.paydatSeedNumeric.Name = "paydatSeedNumeric"
		Me.paydatSeedNumeric.Size = New System.Drawing.Size(90, 20)
		Me.paydatSeedNumeric.TabIndex = 28
		Me.paydatSeedNumeric.Value = New Decimal(New Integer() {497, 0, 0, 0})
		' 
		' dvPaydatPnorderNumeric
		' 
		Me.dvPaydatPnorderNumeric.Location = New System.Drawing.Point(397, 377)
		Me.dvPaydatPnorderNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.dvPaydatPnorderNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.dvPaydatPnorderNumeric.Name = "dvPaydatPnorderNumeric"
		Me.dvPaydatPnorderNumeric.Size = New System.Drawing.Size(90, 20)
		Me.dvPaydatPnorderNumeric.TabIndex = 30
		Me.dvPaydatPnorderNumeric.Value = New Decimal(New Integer() {9, 0, 0, 0})
		' 
		' dvPaydatSeedNumeric
		' 
		Me.dvPaydatSeedNumeric.Location = New System.Drawing.Point(397, 398)
		Me.dvPaydatSeedNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.dvPaydatSeedNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.dvPaydatSeedNumeric.Name = "dvPaydatSeedNumeric"
		Me.dvPaydatSeedNumeric.Size = New System.Drawing.Size(90, 20)
		Me.dvPaydatSeedNumeric.TabIndex = 31
		Me.dvPaydatSeedNumeric.Value = New Decimal(New Integer() {497, 0, 0, 0})
		' 
		' pktLtAddrNumeric
		' 
		Me.pktLtAddrNumeric.Location = New System.Drawing.Point(634, 24)
		Me.pktLtAddrNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.pktLtAddrNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.pktLtAddrNumeric.Name = "pktLtAddrNumeric"
		Me.pktLtAddrNumeric.Size = New System.Drawing.Size(90, 20)
		Me.pktLtAddrNumeric.TabIndex = 32
		' 
		' pktHdrFlowNumeric
		' 
		Me.pktHdrFlowNumeric.Location = New System.Drawing.Point(634, 49)
		Me.pktHdrFlowNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.pktHdrFlowNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.pktHdrFlowNumeric.Name = "pktHdrFlowNumeric"
		Me.pktHdrFlowNumeric.Size = New System.Drawing.Size(90, 20)
		Me.pktHdrFlowNumeric.TabIndex = 33
		' 
		' pktHdrSeqnNumeric
		' 
		Me.pktHdrSeqnNumeric.Location = New System.Drawing.Point(634, 87)
		Me.pktHdrSeqnNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.pktHdrSeqnNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.pktHdrSeqnNumeric.Name = "pktHdrSeqnNumeric"
		Me.pktHdrSeqnNumeric.Size = New System.Drawing.Size(90, 20)
		Me.pktHdrSeqnNumeric.TabIndex = 35
		' 
		' whiteClkNumeric
		' 
		Me.whiteClkNumeric.Location = New System.Drawing.Point(631, 176)
		Me.whiteClkNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.whiteClkNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.whiteClkNumeric.Name = "whiteClkNumeric"
		Me.whiteClkNumeric.Size = New System.Drawing.Size(90, 20)
		Me.whiteClkNumeric.TabIndex = 37
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
		Me.payHdrActPaylenTextBox.Location = New System.Drawing.Point(399, 225)
		Me.payHdrActPaylenTextBox.Name = "payHdrActPaylenTextBox"
		Me.payHdrActPaylenTextBox.Size = New System.Drawing.Size(90, 20)
		Me.payHdrActPaylenTextBox.TabIndex = 25
		Me.payHdrActPaylenTextBox.Text = "0"
		' 
		' waveNameTextBox
		' 
		Me.waveNameTextBox.Location = New System.Drawing.Point(614, 262)
		Me.waveNameTextBox.Name = "waveNameTextBox"
		Me.waveNameTextBox.Size = New System.Drawing.Size(87, 20)
		Me.waveNameTextBox.TabIndex = 38
		Me.waveNameTextBox.Text = "DV"
		' 
		' scriptTextBox
		' 
		Me.scriptTextBox.Location = New System.Drawing.Point(529, 312)
		Me.scriptTextBox.Multiline = True
		Me.scriptTextBox.Name = "scriptTextBox"
		Me.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.scriptTextBox.Size = New System.Drawing.Size(152, 85)
		Me.scriptTextBox.TabIndex = 38
		Me.scriptTextBox.TabStop = False
		Me.scriptTextBox.Text = "script GenerateDVPkt" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    repeat forever" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       generate DV" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       generate idle" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    end repeat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " end script"
		' 
		' errorTextBox
		' 
		Me.errorTextBox.Location = New System.Drawing.Point(529, 437)
		Me.errorTextBox.Multiline = True
		Me.errorTextBox.Name = "errorTextBox"
		Me.errorTextBox.[ReadOnly] = True
		Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.errorTextBox.Size = New System.Drawing.Size(174, 60)
		Me.errorTextBox.TabIndex = 40
		Me.errorTextBox.TabStop = False
		Me.errorTextBox.Text = "No Error"
		' 
		' generateButton
		' 
		Me.generateButton.Location = New System.Drawing.Point(281, 629)
		Me.generateButton.Name = "generateButton"
		Me.generateButton.Size = New System.Drawing.Size(75, 23)
		Me.generateButton.TabIndex = 41
		Me.generateButton.Text = "&Generate"
		Me.generateButton.UseVisualStyleBackColor = True
		AddHandler Me.generateButton.Click, New System.EventHandler(AddressOf Me.generateButton_Click)
		' 
		' stopButton
		' 
		Me.stopButton.Location = New System.Drawing.Point(442, 629)
		Me.stopButton.Name = "stopButton"
		Me.stopButton.Size = New System.Drawing.Size(75, 23)
		Me.stopButton.TabIndex = 42
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
		' 
		' refSourceComboBox
		' 
		Me.refSourceComboBox.Location = New System.Drawing.Point(162, 235)
		Me.refSourceComboBox.Name = "refSourceComboBox"
		Me.refSourceComboBox.Size = New System.Drawing.Size(90, 21)
		Me.refSourceComboBox.TabIndex = 8
		' 
		' clkOutTerminalComboBox
		' 
		Me.clkOutTerminalComboBox.Location = New System.Drawing.Point(162, 256)
		Me.clkOutTerminalComboBox.Name = "clkOutTerminalComboBox"
		Me.clkOutTerminalComboBox.Size = New System.Drawing.Size(90, 21)
		Me.clkOutTerminalComboBox.TabIndex = 9
		' 
		' allIqImpairEnComboBox
		' 
		Me.allIqImpairEnComboBox.Location = New System.Drawing.Point(162, 303)
		Me.allIqImpairEnComboBox.Name = "allIqImpairEnComboBox"
		Me.allIqImpairEnComboBox.Size = New System.Drawing.Size(90, 21)
		Me.allIqImpairEnComboBox.TabIndex = 10
		' 
		' awgnEnabledComboBox
		' 
		Me.awgnEnabledComboBox.Location = New System.Drawing.Point(162, 432)
		Me.awgnEnabledComboBox.Name = "awgnEnabledComboBox"
		Me.awgnEnabledComboBox.Size = New System.Drawing.Size(90, 21)
		Me.awgnEnabledComboBox.TabIndex = 16
		' 
		' payHdrPaylenModeComboBox
		' 
		Me.payHdrPaylenModeComboBox.Location = New System.Drawing.Point(398, 183)
		Me.payHdrPaylenModeComboBox.Name = "payHdrPaylenModeComboBox"
		Me.payHdrPaylenModeComboBox.Size = New System.Drawing.Size(90, 21)
		Me.payHdrPaylenModeComboBox.TabIndex = 23
		' 
		' payHdrDatatypeComboBox
		' 
		Me.payHdrDatatypeComboBox.Location = New System.Drawing.Point(397, 269)
		Me.payHdrDatatypeComboBox.Name = "payHdrDatatypeComboBox"
		Me.payHdrDatatypeComboBox.Size = New System.Drawing.Size(90, 21)
		Me.payHdrDatatypeComboBox.TabIndex = 26
		' 
		' dvPayhdrDatatypeComboBox
		' 
		Me.dvPayhdrDatatypeComboBox.Location = New System.Drawing.Point(396, 355)
		Me.dvPayhdrDatatypeComboBox.Name = "dvPayhdrDatatypeComboBox"
		Me.dvPayhdrDatatypeComboBox.Size = New System.Drawing.Size(90, 21)
		Me.dvPayhdrDatatypeComboBox.TabIndex = 29
		' 
		' pktHdrArqnComboBox
		' 
		Me.pktHdrArqnComboBox.Location = New System.Drawing.Point(634, 68)
		Me.pktHdrArqnComboBox.Name = "pktHdrArqnComboBox"
		Me.pktHdrArqnComboBox.Size = New System.Drawing.Size(90, 21)
		Me.pktHdrArqnComboBox.TabIndex = 34
		' 
		' whiteEnComboBox
		' 
		Me.whiteEnComboBox.Location = New System.Drawing.Point(631, 156)
		Me.whiteEnComboBox.Name = "whiteEnComboBox"
		Me.whiteEnComboBox.Size = New System.Drawing.Size(90, 21)
		Me.whiteEnComboBox.TabIndex = 36
		' 
		' timer
		' 
		AddHandler Me.timer.Tick, New System.EventHandler(AddressOf Me.ProcessTimerEvent)
		' 
		' iOffsetLabel
		' 
		Me.iOffsetLabel.AutoSize = True
		Me.iOffsetLabel.Location = New System.Drawing.Point(19, 539)
		Me.iOffsetLabel.Name = "iOffsetLabel"
		Me.iOffsetLabel.Size = New System.Drawing.Size(54, 13)
		Me.iOffsetLabel.TabIndex = 76
		Me.iOffsetLabel.Text = "I Offset(V)"
		' 
		' qOffsetLabel
		' 
		Me.qOffsetLabel.AutoSize = True
		Me.qOffsetLabel.Location = New System.Drawing.Point(19, 561)
		Me.qOffsetLabel.Name = "qOffsetLabel"
		Me.qOffsetLabel.Size = New System.Drawing.Size(59, 13)
		Me.qOffsetLabel.TabIndex = 78
		Me.qOffsetLabel.Text = "Q Offset(V)"
		' 
		' iOffsetNumeric
		' 
		Me.iOffsetNumeric.Location = New System.Drawing.Point(161, 537)
		Me.iOffsetNumeric.Name = "iOffsetNumeric"
		Me.iOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.iOffsetNumeric.TabIndex = 75
		' 
		' qOffsetNumeric
		' 
		Me.qOffsetNumeric.Location = New System.Drawing.Point(159, 563)
		Me.qOffsetNumeric.Name = "qOffsetNumeric"
		Me.qOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.qOffsetNumeric.TabIndex = 77
		' 
		' iCommonModeOffsetLabel
		' 
		Me.iCommonModeOffsetLabel.AutoSize = True
		Me.iCommonModeOffsetLabel.Location = New System.Drawing.Point(19, 589)
		Me.iCommonModeOffsetLabel.Name = "iCommonModeOffsetLabel"
		Me.iCommonModeOffsetLabel.Size = New System.Drawing.Size(128, 13)
		Me.iCommonModeOffsetLabel.TabIndex = 72
		Me.iCommonModeOffsetLabel.Text = "I Common Mode Offset(V)"
		' 
		' qCommonModeOffsetLabel
		' 
		Me.qCommonModeOffsetLabel.AutoSize = True
		Me.qCommonModeOffsetLabel.Location = New System.Drawing.Point(19, 611)
		Me.qCommonModeOffsetLabel.Name = "qCommonModeOffsetLabel"
		Me.qCommonModeOffsetLabel.Size = New System.Drawing.Size(133, 13)
		Me.qCommonModeOffsetLabel.TabIndex = 74
		Me.qCommonModeOffsetLabel.Text = "Q Common Mode Offset(V)"
		' 
		' iCommonModeOffsetNumeric
		' 
		Me.iCommonModeOffsetNumeric.Location = New System.Drawing.Point(161, 587)
		Me.iCommonModeOffsetNumeric.Name = "iCommonModeOffsetNumeric"
		Me.iCommonModeOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.iCommonModeOffsetNumeric.TabIndex = 71
		' 
		' qCommonModeOffsetNumeric
		' 
		Me.qCommonModeOffsetNumeric.Location = New System.Drawing.Point(161, 609)
		Me.qCommonModeOffsetNumeric.Name = "qCommonModeOffsetNumeric"
		Me.qCommonModeOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.qCommonModeOffsetNumeric.TabIndex = 73
		' 
		' label2
		' 
		Me.label2.AutoSize = True
		Me.label2.Location = New System.Drawing.Point(17, 513)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(112, 13)
		Me.label2.TabIndex = 70
		Me.label2.Text = "Terminal Configuration"
		' 
		' terminalConfigurationComboBox
		' 
		Me.terminalConfigurationComboBox.Location = New System.Drawing.Point(159, 510)
		Me.terminalConfigurationComboBox.Name = "terminalConfigurationComboBox"
		Me.terminalConfigurationComboBox.Size = New System.Drawing.Size(90, 21)
		Me.terminalConfigurationComboBox.TabIndex = 69
		' 
		' label1
		' 
		Me.label1.AutoSize = True
		Me.label1.Location = New System.Drawing.Point(20, 485)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(61, 13)
		Me.label1.TabIndex = 68
		Me.label1.Text = "Output Port"
		' 
		' outputPortComboBox
		' 
		Me.outputPortComboBox.Location = New System.Drawing.Point(159, 485)
		Me.outputPortComboBox.Name = "outputPortComboBox"
		Me.outputPortComboBox.Size = New System.Drawing.Size(90, 21)
		Me.outputPortComboBox.TabIndex = 67
		' 
		' MainForm
		' 
		Me.AcceptButton = Me.generateButton
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(742, 708)
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
		Me.Controls.Add(Me.rfsgResourceLabel)
		Me.Controls.Add(Me.chnNumberLabel)
		Me.Controls.Add(Me.carrierFreqLabel)
		Me.Controls.Add(Me.powerLevelLabel)
		Me.Controls.Add(Me.externalAttnLabel)
		Me.Controls.Add(Me.autoheadroomEnabLabel)
		Me.Controls.Add(Me.headroomLabel)
		Me.Controls.Add(Me.actualHeadroomLabel)
		Me.Controls.Add(Me.refSourceLabel)
		Me.Controls.Add(Me.clkOutTerminalLabel)
		Me.Controls.Add(Me.allIqImpairEnLabel)
		Me.Controls.Add(Me.quadratureSkewLabel)
		Me.Controls.Add(Me.iDcOffsetLabel)
		Me.Controls.Add(Me.qDcOffsetLabel)
		Me.Controls.Add(Me.iqGaimbalanceLabel)
		Me.Controls.Add(Me.carrierFreqOffLabel)
		Me.Controls.Add(Me.awgnEnabledLabel)
		Me.Controls.Add(Me.cnrLabel)
		Me.Controls.Add(Me.bdaddrLapLabel)
		Me.Controls.Add(Me.bdaddrUapLabel)
		Me.Controls.Add(Me.bdaddrNapLabel)
		Me.Controls.Add(Me.payHdrLlidLabel)
		Me.Controls.Add(Me.payHdrFlowLabel)
		Me.Controls.Add(Me.payHdrPaylenModeLabel)
		Me.Controls.Add(Me.payHdrPaylenLabel)
		Me.Controls.Add(Me.payHdrActPaylenLabel)
		Me.Controls.Add(Me.payHdrDatatypeLabel)
		Me.Controls.Add(Me.paydatPnorderLabel)
		Me.Controls.Add(Me.paydatSeedLabel)
		Me.Controls.Add(Me.dvPayhdrDatatypeLabel)
		Me.Controls.Add(Me.dvPaydatPnorderLabel)
		Me.Controls.Add(Me.dvPaydatSeedLabel)
		Me.Controls.Add(Me.pktLtAddrLabel)
		Me.Controls.Add(Me.pktHdrFlowLabel)
		Me.Controls.Add(Me.pktHdrArqnLabel)
		Me.Controls.Add(Me.pktHdrSeqnLabel)
		Me.Controls.Add(Me.whiteEnLabel)
		Me.Controls.Add(Me.whiteClkLabel)
		Me.Controls.Add(Me.waveNameLabel)
		Me.Controls.Add(Me.scriptLabel)
		Me.Controls.Add(Me.errorLabel)
		Me.Controls.Add(Me.payloadVoiceLabel)
		Me.Controls.Add(Me.textmsgPayloadHdrLabel)
		Me.Controls.Add(Me.payloadDataLabel)
		Me.Controls.Add(Me.hardwareLabel)
		Me.Controls.Add(Me.frequencySettingLabel)
		Me.Controls.Add(Me.impairmentsLabel)
		Me.Controls.Add(Me.bdAddressLabel)
		Me.Controls.Add(Me.payloadControlLabel)
		Me.Controls.Add(Me.packetHeaderLabel)
		Me.Controls.Add(Me.whiteningSettingLabel)
		Me.Controls.Add(Me.chnNumberNumeric)
		Me.Controls.Add(Me.powerLevelNumeric)
		Me.Controls.Add(Me.externalAttnNumeric)
		Me.Controls.Add(Me.headroomNumeric)
		Me.Controls.Add(Me.quadratureSkewNumeric)
		Me.Controls.Add(Me.iDcOffsetNumeric)
		Me.Controls.Add(Me.qDcOffsetNumeric)
		Me.Controls.Add(Me.iqGaimbalanceNumeric)
		Me.Controls.Add(Me.carrierFreqOffNumeric)
		Me.Controls.Add(Me.cnrNumeric)
		Me.Controls.Add(Me.bdaddrLapNumeric)
		Me.Controls.Add(Me.bdaddrUapNumeric)
		Me.Controls.Add(Me.bdaddrNapNumeric)
		Me.Controls.Add(Me.payHdrLlidNumeric)
		Me.Controls.Add(Me.payHdrFlowNumeric)
		Me.Controls.Add(Me.payHdrPaylenNumeric)
		Me.Controls.Add(Me.paydatPnorderNumeric)
		Me.Controls.Add(Me.paydatSeedNumeric)
		Me.Controls.Add(Me.dvPaydatPnorderNumeric)
		Me.Controls.Add(Me.dvPaydatSeedNumeric)
		Me.Controls.Add(Me.pktLtAddrNumeric)
		Me.Controls.Add(Me.pktHdrFlowNumeric)
		Me.Controls.Add(Me.pktHdrSeqnNumeric)
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
		Me.Controls.Add(Me.refSourceComboBox)
		Me.Controls.Add(Me.clkOutTerminalComboBox)
		Me.Controls.Add(Me.allIqImpairEnComboBox)
		Me.Controls.Add(Me.awgnEnabledComboBox)
		Me.Controls.Add(Me.payHdrPaylenModeComboBox)
		Me.Controls.Add(Me.payHdrDatatypeComboBox)
		Me.Controls.Add(Me.dvPayhdrDatatypeComboBox)
		Me.Controls.Add(Me.pktHdrArqnComboBox)
		Me.Controls.Add(Me.whiteEnComboBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.MaximizeBox = False
		Me.Name = "MainForm"
		Me.ShowIcon = False
		Me.Text = "Bluetooth Generate DV Packet Example"
		AddHandler Me.FormClosing, New System.Windows.Forms.FormClosingEventHandler(AddressOf Me.MainFormClosing)
		DirectCast(Me.chnNumberNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.externalAttnNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.headroomNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.quadratureSkewNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.iDcOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.qDcOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.iqGaimbalanceNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.carrierFreqOffNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.cnrNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.bdaddrLapNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.bdaddrUapNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.bdaddrNapNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.payHdrLlidNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.payHdrFlowNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.payHdrPaylenNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.paydatPnorderNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.paydatSeedNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.dvPaydatPnorderNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.dvPaydatSeedNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.pktLtAddrNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.pktHdrFlowNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.pktHdrSeqnNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.whiteClkNumeric, System.ComponentModel.ISupportInitialize).EndInit()
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
	Private autoheadroomEnabLabel As System.Windows.Forms.Label
	Private headroomLabel As System.Windows.Forms.Label
	Private actualHeadroomLabel As System.Windows.Forms.Label
	Private refSourceLabel As System.Windows.Forms.Label
	Private clkOutTerminalLabel As System.Windows.Forms.Label
	Private allIqImpairEnLabel As System.Windows.Forms.Label
	Private quadratureSkewLabel As System.Windows.Forms.Label
	Private iDcOffsetLabel As System.Windows.Forms.Label
	Private qDcOffsetLabel As System.Windows.Forms.Label
	Private iqGaimbalanceLabel As System.Windows.Forms.Label
	Private carrierFreqOffLabel As System.Windows.Forms.Label
	Private awgnEnabledLabel As System.Windows.Forms.Label
	Private cnrLabel As System.Windows.Forms.Label
	Private bdaddrLapLabel As System.Windows.Forms.Label
	Private bdaddrUapLabel As System.Windows.Forms.Label
	Private bdaddrNapLabel As System.Windows.Forms.Label
	Private payHdrLlidLabel As System.Windows.Forms.Label
	Private payHdrFlowLabel As System.Windows.Forms.Label
	Private payHdrPaylenModeLabel As System.Windows.Forms.Label
	Private payHdrPaylenLabel As System.Windows.Forms.Label
	Private payHdrActPaylenLabel As System.Windows.Forms.Label
	Private payHdrDatatypeLabel As System.Windows.Forms.Label
	Private paydatPnorderLabel As System.Windows.Forms.Label
	Private paydatSeedLabel As System.Windows.Forms.Label
	Private dvPayhdrDatatypeLabel As System.Windows.Forms.Label
	Private dvPaydatPnorderLabel As System.Windows.Forms.Label
	Private dvPaydatSeedLabel As System.Windows.Forms.Label
	Private pktLtAddrLabel As System.Windows.Forms.Label
	Private pktHdrFlowLabel As System.Windows.Forms.Label
	Private pktHdrArqnLabel As System.Windows.Forms.Label
	Private pktHdrSeqnLabel As System.Windows.Forms.Label
	Private whiteEnLabel As System.Windows.Forms.Label
	Private whiteClkLabel As System.Windows.Forms.Label
	Private waveNameLabel As System.Windows.Forms.Label
	Private scriptLabel As System.Windows.Forms.Label
	Private errorLabel As System.Windows.Forms.Label
	Private payloadVoiceLabel As System.Windows.Forms.Label
	Private textmsgPayloadHdrLabel As System.Windows.Forms.Label
	Private payloadDataLabel As System.Windows.Forms.Label
	Private hardwareLabel As System.Windows.Forms.Label
	Private frequencySettingLabel As System.Windows.Forms.Label
	Private impairmentsLabel As System.Windows.Forms.Label
	Private bdAddressLabel As System.Windows.Forms.Label
	Private payloadControlLabel As System.Windows.Forms.Label
	Private packetHeaderLabel As System.Windows.Forms.Label
	Private whiteningSettingLabel As System.Windows.Forms.Label
	Private chnNumberNumeric As System.Windows.Forms.NumericUpDown
	Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
	Private externalAttnNumeric As System.Windows.Forms.NumericUpDown
	Private headroomNumeric As System.Windows.Forms.NumericUpDown
	Private quadratureSkewNumeric As System.Windows.Forms.NumericUpDown
	Private iDcOffsetNumeric As System.Windows.Forms.NumericUpDown
	Private qDcOffsetNumeric As System.Windows.Forms.NumericUpDown
	Private iqGaimbalanceNumeric As System.Windows.Forms.NumericUpDown
	Private carrierFreqOffNumeric As System.Windows.Forms.NumericUpDown
	Private cnrNumeric As System.Windows.Forms.NumericUpDown
	Private bdaddrLapNumeric As System.Windows.Forms.NumericUpDown
	Private bdaddrUapNumeric As System.Windows.Forms.NumericUpDown
	Private bdaddrNapNumeric As System.Windows.Forms.NumericUpDown
	Private payHdrLlidNumeric As System.Windows.Forms.NumericUpDown
	Private payHdrFlowNumeric As System.Windows.Forms.NumericUpDown
	Private payHdrPaylenNumeric As System.Windows.Forms.NumericUpDown
	Private paydatPnorderNumeric As System.Windows.Forms.NumericUpDown
	Private paydatSeedNumeric As System.Windows.Forms.NumericUpDown
	Private dvPaydatPnorderNumeric As System.Windows.Forms.NumericUpDown
	Private dvPaydatSeedNumeric As System.Windows.Forms.NumericUpDown
	Private pktLtAddrNumeric As System.Windows.Forms.NumericUpDown
	Private pktHdrFlowNumeric As System.Windows.Forms.NumericUpDown
	Private pktHdrSeqnNumeric As System.Windows.Forms.NumericUpDown
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
	Private clkOutTerminalComboBox As System.Windows.Forms.ComboBox
	Private allIqImpairEnComboBox As System.Windows.Forms.ComboBox
	Private awgnEnabledComboBox As System.Windows.Forms.ComboBox
	Private payHdrPaylenModeComboBox As System.Windows.Forms.ComboBox
	Private payHdrDatatypeComboBox As System.Windows.Forms.ComboBox
	Private dvPayhdrDatatypeComboBox As System.Windows.Forms.ComboBox
	Private pktHdrArqnComboBox As System.Windows.Forms.ComboBox
	Private whiteEnComboBox As System.Windows.Forms.ComboBox
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

End Class
