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
		Me.clkTerminalLabel = New System.Windows.Forms.Label()
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
		Me.pktLtAddrLabel = New System.Windows.Forms.Label()
		Me.pktHdrFlowLabel = New System.Windows.Forms.Label()
		Me.pktHdrArqnLabel = New System.Windows.Forms.Label()
		Me.pktHdrSeqnLabel = New System.Windows.Forms.Label()
		Me.fhsBdaddrLapLabel = New System.Windows.Forms.Label()
		Me.fhsBdaddrUapLabel = New System.Windows.Forms.Label()
		Me.fhsBdaddrNapLabel = New System.Windows.Forms.Label()
		Me.fhsLtAddrLabel = New System.Windows.Forms.Label()
		Me.fhsDevClassLabel = New System.Windows.Forms.Label()
		Me.fhsScanRepLabel = New System.Windows.Forms.Label()
		Me.fhsPgScanLabel = New System.Windows.Forms.Label()
		Me.fhsDevClockLabel = New System.Windows.Forms.Label()
		Me.whiteEnLabel = New System.Windows.Forms.Label()
		Me.whiteClkLabel = New System.Windows.Forms.Label()
		Me.waveNameLabel = New System.Windows.Forms.Label()
		Me.scriptLabel = New System.Windows.Forms.Label()
		Me.errorLabel = New System.Windows.Forms.Label()
		Me.hardwareLabel = New System.Windows.Forms.Label()
		Me.frequencySettingsLabel = New System.Windows.Forms.Label()
		Me.impairmentsLabel = New System.Windows.Forms.Label()
		Me.whiteningSettingsLabel = New System.Windows.Forms.Label()
		Me.slaveBDAddressLabel = New System.Windows.Forms.Label()
		Me.bdAddressLabel = New System.Windows.Forms.Label()
		Me.packetHeaderLabel = New System.Windows.Forms.Label()
		Me.fhsPayloadLabel = New System.Windows.Forms.Label()
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
		Me.pktLtAddrNumeric = New System.Windows.Forms.NumericUpDown()
		Me.pktHdrFlowNumeric = New System.Windows.Forms.NumericUpDown()
		Me.pktHdrSeqnNumeric = New System.Windows.Forms.NumericUpDown()
		Me.fhsBdaddrLapNumeric = New System.Windows.Forms.NumericUpDown()
		Me.fhsBdaddrUapNumeric = New System.Windows.Forms.NumericUpDown()
		Me.fhsBdaddrNapNumeric = New System.Windows.Forms.NumericUpDown()
		Me.fhsLtAddrNumeric = New System.Windows.Forms.NumericUpDown()
		Me.fhsDevClassNumeric = New System.Windows.Forms.NumericUpDown()
		Me.fhsScanRepNumeric = New System.Windows.Forms.NumericUpDown()
		Me.fhsPgScanNumeric = New System.Windows.Forms.NumericUpDown()
		Me.fhsDevClockNumeric = New System.Windows.Forms.NumericUpDown()
		Me.whiteClkNumeric = New System.Windows.Forms.NumericUpDown()
		Me.rfsgResourceTextBox = New System.Windows.Forms.TextBox()
		Me.carrierFreqTextBox = New System.Windows.Forms.TextBox()
		Me.actualHeadroomTextBox = New System.Windows.Forms.TextBox()
		Me.waveNameTextBox = New System.Windows.Forms.TextBox()
		Me.scriptTextBox = New System.Windows.Forms.TextBox()
		Me.errorTextBox = New System.Windows.Forms.TextBox()
		Me.generateButton = New System.Windows.Forms.Button()
		Me.stopButton = New System.Windows.Forms.Button()
		Me.autoheadroomEnabComboBox = New System.Windows.Forms.ComboBox()
		Me.refSourceComboBox = New System.Windows.Forms.ComboBox()
		Me.clkOutTerminalComboBox = New System.Windows.Forms.ComboBox()
		Me.allIqImpairEnComboBox = New System.Windows.Forms.ComboBox()
		Me.awgnEnabledComboBox = New System.Windows.Forms.ComboBox()
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
		DirectCast(Me.pktLtAddrNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.pktHdrFlowNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.pktHdrSeqnNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.fhsBdaddrLapNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.fhsBdaddrUapNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.fhsBdaddrNapNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.fhsLtAddrNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.fhsDevClassNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.fhsScanRepNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.fhsPgScanNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.fhsDevClockNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
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
		Me.rfsgResourceLabel.Location = New System.Drawing.Point(17, 29)
		Me.rfsgResourceLabel.Name = "rfsgResourceLabel"
		Me.rfsgResourceLabel.Size = New System.Drawing.Size(85, 13)
		Me.rfsgResourceLabel.TabIndex = 0
		Me.rfsgResourceLabel.Text = "RFSG Resource"
		' 
		' chnNumberLabel
		' 
		Me.chnNumberLabel.AutoSize = True
		Me.chnNumberLabel.Location = New System.Drawing.Point(17, 48)
		Me.chnNumberLabel.Name = "chnNumberLabel"
		Me.chnNumberLabel.Size = New System.Drawing.Size(86, 13)
		Me.chnNumberLabel.TabIndex = 1
		Me.chnNumberLabel.Text = "Channel Number"
		' 
		' carrierFreqLabel
		' 
		Me.carrierFreqLabel.AutoSize = True
		Me.carrierFreqLabel.Location = New System.Drawing.Point(17, 68)
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
		Me.refSourceLabel.Location = New System.Drawing.Point(17, 225)
		Me.refSourceLabel.Name = "refSourceLabel"
		Me.refSourceLabel.Size = New System.Drawing.Size(94, 13)
		Me.refSourceLabel.TabIndex = 8
		Me.refSourceLabel.Text = "Reference Source"
		' 
		' clkTerminalLabel
		' 
		Me.clkTerminalLabel.AutoSize = True
		Me.clkTerminalLabel.Location = New System.Drawing.Point(17, 246)
		Me.clkTerminalLabel.Name = "clkTerminalLabel"
		Me.clkTerminalLabel.Size = New System.Drawing.Size(67, 13)
		Me.clkTerminalLabel.TabIndex = 9
		Me.clkTerminalLabel.Text = "Export Clock"
		' 
		' allIqImpairEnLabel
		' 
		Me.allIqImpairEnLabel.AutoSize = True
		Me.allIqImpairEnLabel.Location = New System.Drawing.Point(17, 294)
		Me.allIqImpairEnLabel.Name = "allIqImpairEnLabel"
		Me.allIqImpairEnLabel.Size = New System.Drawing.Size(133, 13)
		Me.allIqImpairEnLabel.TabIndex = 10
		Me.allIqImpairEnLabel.Text = "All IQ Impairments Enabled"
		' 
		' quadratureSkewLabel
		' 
		Me.quadratureSkewLabel.AutoSize = True
		Me.quadratureSkewLabel.Location = New System.Drawing.Point(17, 316)
		Me.quadratureSkewLabel.Name = "quadratureSkewLabel"
		Me.quadratureSkewLabel.Size = New System.Drawing.Size(117, 13)
		Me.quadratureSkewLabel.TabIndex = 11
		Me.quadratureSkewLabel.Text = "Quadrature Skew (deg)"
		' 
		' iDcOffsetLabel
		' 
		Me.iDcOffsetLabel.AutoSize = True
		Me.iDcOffsetLabel.Location = New System.Drawing.Point(17, 338)
		Me.iDcOffsetLabel.Name = "iDcOffsetLabel"
		Me.iDcOffsetLabel.Size = New System.Drawing.Size(76, 13)
		Me.iDcOffsetLabel.TabIndex = 12
		Me.iDcOffsetLabel.Text = "I DC Offset (%)"
		' 
		' qDcOffsetLabel
		' 
		Me.qDcOffsetLabel.AutoSize = True
		Me.qDcOffsetLabel.Location = New System.Drawing.Point(17, 360)
		Me.qDcOffsetLabel.Name = "qDcOffsetLabel"
		Me.qDcOffsetLabel.Size = New System.Drawing.Size(81, 13)
		Me.qDcOffsetLabel.TabIndex = 13
		Me.qDcOffsetLabel.Text = "Q DC Offset (%)"
		' 
		' iqGaimbalanceLabel
		' 
		Me.iqGaimbalanceLabel.AutoSize = True
		Me.iqGaimbalanceLabel.Location = New System.Drawing.Point(17, 382)
		Me.iqGaimbalanceLabel.Name = "iqGaimbalanceLabel"
		Me.iqGaimbalanceLabel.Size = New System.Drawing.Size(117, 13)
		Me.iqGaimbalanceLabel.TabIndex = 14
		Me.iqGaimbalanceLabel.Text = "IQ Gain Imbalance (dB)"
		' 
		' carrierFreqOffLabel
		' 
		Me.carrierFreqOffLabel.AutoSize = True
		Me.carrierFreqOffLabel.Location = New System.Drawing.Point(17, 402)
		Me.carrierFreqOffLabel.Name = "carrierFreqOffLabel"
		Me.carrierFreqOffLabel.Size = New System.Drawing.Size(143, 13)
		Me.carrierFreqOffLabel.TabIndex = 15
		Me.carrierFreqOffLabel.Text = "Carrier Frequency Offset (Hz)"
		' 
		' awgnEnabledLabel
		' 
		Me.awgnEnabledLabel.AutoSize = True
		Me.awgnEnabledLabel.Location = New System.Drawing.Point(17, 423)
		Me.awgnEnabledLabel.Name = "awgnEnabledLabel"
		Me.awgnEnabledLabel.Size = New System.Drawing.Size(83, 13)
		Me.awgnEnabledLabel.TabIndex = 16
		Me.awgnEnabledLabel.Text = "AWGN Enabled"
		' 
		' cnrLabel
		' 
		Me.cnrLabel.AutoSize = True
		Me.cnrLabel.Location = New System.Drawing.Point(17, 443)
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
		Me.pktLtAddrLabel.Location = New System.Drawing.Point(281, 113)
		Me.pktLtAddrLabel.Name = "pktLtAddrLabel"
		Me.pktLtAddrLabel.Size = New System.Drawing.Size(61, 13)
		Me.pktLtAddrLabel.TabIndex = 21
		Me.pktLtAddrLabel.Text = "LT Address"
		' 
		' pktHdrFlowLabel
		' 
		Me.pktHdrFlowLabel.AutoSize = True
		Me.pktHdrFlowLabel.Location = New System.Drawing.Point(281, 138)
		Me.pktHdrFlowLabel.Name = "pktHdrFlowLabel"
		Me.pktHdrFlowLabel.Size = New System.Drawing.Size(29, 13)
		Me.pktHdrFlowLabel.TabIndex = 22
		Me.pktHdrFlowLabel.Text = "Flow"
		' 
		' pktHdrArqnLabel
		' 
		Me.pktHdrArqnLabel.AutoSize = True
		Me.pktHdrArqnLabel.Location = New System.Drawing.Point(281, 157)
		Me.pktHdrArqnLabel.Name = "pktHdrArqnLabel"
		Me.pktHdrArqnLabel.Size = New System.Drawing.Size(38, 13)
		Me.pktHdrArqnLabel.TabIndex = 23
		Me.pktHdrArqnLabel.Text = "ARQN"
		' 
		' pktHdrSeqnLabel
		' 
		Me.pktHdrSeqnLabel.AutoSize = True
		Me.pktHdrSeqnLabel.Location = New System.Drawing.Point(281, 176)
		Me.pktHdrSeqnLabel.Name = "pktHdrSeqnLabel"
		Me.pktHdrSeqnLabel.Size = New System.Drawing.Size(37, 13)
		Me.pktHdrSeqnLabel.TabIndex = 24
		Me.pktHdrSeqnLabel.Text = "SEQN"
		' 
		' fhsBdaddrLapLabel
		' 
		Me.fhsBdaddrLapLabel.AutoSize = True
		Me.fhsBdaddrLapLabel.Location = New System.Drawing.Point(280, 250)
		Me.fhsBdaddrLapLabel.Name = "fhsBdaddrLapLabel"
		Me.fhsBdaddrLapLabel.Size = New System.Drawing.Size(27, 13)
		Me.fhsBdaddrLapLabel.TabIndex = 25
		Me.fhsBdaddrLapLabel.Text = "LAP"
		' 
		' fhsBdaddrUapLabel
		' 
		Me.fhsBdaddrUapLabel.AutoSize = True
		Me.fhsBdaddrUapLabel.Location = New System.Drawing.Point(280, 272)
		Me.fhsBdaddrUapLabel.Name = "fhsBdaddrUapLabel"
		Me.fhsBdaddrUapLabel.Size = New System.Drawing.Size(29, 13)
		Me.fhsBdaddrUapLabel.TabIndex = 26
		Me.fhsBdaddrUapLabel.Text = "UAP"
		' 
		' fhsBdaddrNapLabel
		' 
		Me.fhsBdaddrNapLabel.AutoSize = True
		Me.fhsBdaddrNapLabel.Location = New System.Drawing.Point(280, 294)
		Me.fhsBdaddrNapLabel.Name = "fhsBdaddrNapLabel"
		Me.fhsBdaddrNapLabel.Size = New System.Drawing.Size(29, 13)
		Me.fhsBdaddrNapLabel.TabIndex = 27
		Me.fhsBdaddrNapLabel.Text = "NAP"
		' 
		' fhsLtAddrLabel
		' 
		Me.fhsLtAddrLabel.AutoSize = True
		Me.fhsLtAddrLabel.Location = New System.Drawing.Point(280, 334)
		Me.fhsLtAddrLabel.Name = "fhsLtAddrLabel"
		Me.fhsLtAddrLabel.Size = New System.Drawing.Size(61, 13)
		Me.fhsLtAddrLabel.TabIndex = 28
		Me.fhsLtAddrLabel.Text = "LT Address"
		' 
		' fhsDevClassLabel
		' 
		Me.fhsDevClassLabel.AutoSize = True
		Me.fhsDevClassLabel.Location = New System.Drawing.Point(280, 356)
		Me.fhsDevClassLabel.Name = "fhsDevClassLabel"
		Me.fhsDevClassLabel.Size = New System.Drawing.Size(69, 13)
		Me.fhsDevClassLabel.TabIndex = 29
		Me.fhsDevClassLabel.Text = "Device Class"
		' 
		' fhsScanRepLabel
		' 
		Me.fhsScanRepLabel.AutoSize = True
		Me.fhsScanRepLabel.Location = New System.Drawing.Point(280, 379)
		Me.fhsScanRepLabel.Name = "fhsScanRepLabel"
		Me.fhsScanRepLabel.Size = New System.Drawing.Size(79, 13)
		Me.fhsScanRepLabel.TabIndex = 30
		Me.fhsScanRepLabel.Text = "Scan Repitition"
		' 
		' fhsPgScanLabel
		' 
		Me.fhsPgScanLabel.AutoSize = True
		Me.fhsPgScanLabel.Location = New System.Drawing.Point(280, 403)
		Me.fhsPgScanLabel.Name = "fhsPgScanLabel"
		Me.fhsPgScanLabel.Size = New System.Drawing.Size(60, 13)
		Me.fhsPgScanLabel.TabIndex = 31
		Me.fhsPgScanLabel.Text = "Page Scan"
		' 
		' fhsDevClockLabel
		' 
		Me.fhsDevClockLabel.AutoSize = True
		Me.fhsDevClockLabel.Location = New System.Drawing.Point(280, 424)
		Me.fhsDevClockLabel.Name = "fhsDevClockLabel"
		Me.fhsDevClockLabel.Size = New System.Drawing.Size(71, 13)
		Me.fhsDevClockLabel.TabIndex = 32
		Me.fhsDevClockLabel.Text = "Device Clock"
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
		Me.waveNameLabel.TabIndex = 37
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
		Me.errorLabel.Location = New System.Drawing.Point(531, 281)
		Me.errorLabel.Name = "errorLabel"
		Me.errorLabel.Size = New System.Drawing.Size(29, 13)
		Me.errorLabel.TabIndex = 38
		Me.errorLabel.Text = "Error"
		' 
		' hardwareLabel
		' 
		Me.hardwareLabel.AutoSize = True
		Me.hardwareLabel.Location = New System.Drawing.Point(30, 6)
		Me.hardwareLabel.Name = "hardwareLabel"
		Me.hardwareLabel.Size = New System.Drawing.Size(53, 13)
		Me.hardwareLabel.TabIndex = 39
		Me.hardwareLabel.Text = "Hardware"
		' 
		' frequencySettingsLabel
		' 
		Me.frequencySettingsLabel.AutoSize = True
		Me.frequencySettingsLabel.Location = New System.Drawing.Point(30, 207)
		Me.frequencySettingsLabel.Name = "frequencySettingsLabel"
		Me.frequencySettingsLabel.Size = New System.Drawing.Size(98, 13)
		Me.frequencySettingsLabel.TabIndex = 40
		Me.frequencySettingsLabel.Text = "Frequency Settings"
		' 
		' impairmentsLabel
		' 
		Me.impairmentsLabel.AutoSize = True
		Me.impairmentsLabel.Location = New System.Drawing.Point(30, 273)
		Me.impairmentsLabel.Name = "impairmentsLabel"
		Me.impairmentsLabel.Size = New System.Drawing.Size(63, 13)
		Me.impairmentsLabel.TabIndex = 41
		Me.impairmentsLabel.Text = "Impairments"
		' 
		' whiteningSettingsLabel
		' 
		Me.whiteningSettingsLabel.AutoSize = True
		Me.whiteningSettingsLabel.Location = New System.Drawing.Point(561, 3)
		Me.whiteningSettingsLabel.Name = "whiteningSettingsLabel"
		Me.whiteningSettingsLabel.Size = New System.Drawing.Size(96, 13)
		Me.whiteningSettingsLabel.TabIndex = 42
		Me.whiteningSettingsLabel.Text = "Whitening Settings"
		' 
		' slaveBDAddressLabel
		' 
		Me.slaveBDAddressLabel.AutoSize = True
		Me.slaveBDAddressLabel.Location = New System.Drawing.Point(287, 227)
		Me.slaveBDAddressLabel.Name = "slaveBDAddressLabel"
		Me.slaveBDAddressLabel.Size = New System.Drawing.Size(93, 13)
		Me.slaveBDAddressLabel.TabIndex = 43
		Me.slaveBDAddressLabel.Text = "Slave BD Address"
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
		' packetHeaderLabel
		' 
		Me.packetHeaderLabel.AutoSize = True
		Me.packetHeaderLabel.Location = New System.Drawing.Point(301, 96)
		Me.packetHeaderLabel.Name = "packetHeaderLabel"
		Me.packetHeaderLabel.Size = New System.Drawing.Size(79, 13)
		Me.packetHeaderLabel.TabIndex = 45
		Me.packetHeaderLabel.Text = "Packet Header"
		' 
		' fhsPayloadLabel
		' 
		Me.fhsPayloadLabel.AutoSize = True
		Me.fhsPayloadLabel.Location = New System.Drawing.Point(301, 209)
		Me.fhsPayloadLabel.Name = "fhsPayloadLabel"
		Me.fhsPayloadLabel.Size = New System.Drawing.Size(69, 13)
		Me.fhsPayloadLabel.TabIndex = 46
		Me.fhsPayloadLabel.Text = "FHS Payload"
		' 
		' chnNumberNumeric
		' 
		Me.chnNumberNumeric.Location = New System.Drawing.Point(162, 43)
		Me.chnNumberNumeric.Name = "chnNumberNumeric"
		Me.chnNumberNumeric.Size = New System.Drawing.Size(90, 20)
		Me.chnNumberNumeric.TabIndex = 1
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
		Me.quadratureSkewNumeric.Location = New System.Drawing.Point(162, 315)
		Me.quadratureSkewNumeric.Name = "quadratureSkewNumeric"
		Me.quadratureSkewNumeric.Size = New System.Drawing.Size(90, 20)
		Me.quadratureSkewNumeric.TabIndex = 11
		' 
		' iDcOffsetNumeric
		' 
		Me.iDcOffsetNumeric.Location = New System.Drawing.Point(162, 337)
		Me.iDcOffsetNumeric.Name = "iDcOffsetNumeric"
		Me.iDcOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.iDcOffsetNumeric.TabIndex = 12
		' 
		' qDcOffsetNumeric
		' 
		Me.qDcOffsetNumeric.Location = New System.Drawing.Point(162, 359)
		Me.qDcOffsetNumeric.Name = "qDcOffsetNumeric"
		Me.qDcOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.qDcOffsetNumeric.TabIndex = 13
		' 
		' iqGaimbalanceNumeric
		' 
		Me.iqGaimbalanceNumeric.Location = New System.Drawing.Point(162, 381)
		Me.iqGaimbalanceNumeric.Name = "iqGaimbalanceNumeric"
		Me.iqGaimbalanceNumeric.Size = New System.Drawing.Size(90, 20)
		Me.iqGaimbalanceNumeric.TabIndex = 14
		' 
		' carrierFreqOffNumeric
		' 
		Me.carrierFreqOffNumeric.Location = New System.Drawing.Point(162, 400)
		Me.carrierFreqOffNumeric.Name = "carrierFreqOffNumeric"
		Me.carrierFreqOffNumeric.Size = New System.Drawing.Size(90, 20)
		Me.carrierFreqOffNumeric.TabIndex = 15
		' 
		' cnrNumeric
		' 
		Me.cnrNumeric.Location = New System.Drawing.Point(162, 442)
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
		Me.pktLtAddrNumeric.Location = New System.Drawing.Point(398, 112)
		Me.pktLtAddrNumeric.Name = "pktLtAddrNumeric"
		Me.pktLtAddrNumeric.Size = New System.Drawing.Size(90, 20)
		Me.pktLtAddrNumeric.TabIndex = 21
		' 
		' pktHdrFlowNumeric
		' 
		Me.pktHdrFlowNumeric.Location = New System.Drawing.Point(398, 137)
		Me.pktHdrFlowNumeric.Name = "pktHdrFlowNumeric"
		Me.pktHdrFlowNumeric.Size = New System.Drawing.Size(90, 20)
		Me.pktHdrFlowNumeric.TabIndex = 22
		' 
		' pktHdrSeqnNumeric
		' 
		Me.pktHdrSeqnNumeric.Location = New System.Drawing.Point(398, 175)
		Me.pktHdrSeqnNumeric.Name = "pktHdrSeqnNumeric"
		Me.pktHdrSeqnNumeric.Size = New System.Drawing.Size(90, 20)
		Me.pktHdrSeqnNumeric.TabIndex = 24
		' 
		' fhsBdaddrLapNumeric
		' 
		Me.fhsBdaddrLapNumeric.Location = New System.Drawing.Point(397, 247)
		Me.fhsBdaddrLapNumeric.Name = "fhsBdaddrLapNumeric"
		Me.fhsBdaddrLapNumeric.Size = New System.Drawing.Size(90, 20)
		Me.fhsBdaddrLapNumeric.TabIndex = 25
		' 
		' fhsBdaddrUapNumeric
		' 
		Me.fhsBdaddrUapNumeric.Location = New System.Drawing.Point(397, 270)
		Me.fhsBdaddrUapNumeric.Name = "fhsBdaddrUapNumeric"
		Me.fhsBdaddrUapNumeric.Size = New System.Drawing.Size(90, 20)
		Me.fhsBdaddrUapNumeric.TabIndex = 26
		' 
		' fhsBdaddrNapNumeric
		' 
		Me.fhsBdaddrNapNumeric.Location = New System.Drawing.Point(397, 289)
		Me.fhsBdaddrNapNumeric.Name = "fhsBdaddrNapNumeric"
		Me.fhsBdaddrNapNumeric.Size = New System.Drawing.Size(90, 20)
		Me.fhsBdaddrNapNumeric.TabIndex = 27
		' 
		' fhsLtAddrNumeric
		' 
		Me.fhsLtAddrNumeric.Location = New System.Drawing.Point(397, 331)
		Me.fhsLtAddrNumeric.Name = "fhsLtAddrNumeric"
		Me.fhsLtAddrNumeric.Size = New System.Drawing.Size(90, 20)
		Me.fhsLtAddrNumeric.TabIndex = 28
		' 
		' fhsDevClassNumeric
		' 
		Me.fhsDevClassNumeric.Location = New System.Drawing.Point(397, 354)
		Me.fhsDevClassNumeric.Name = "fhsDevClassNumeric"
		Me.fhsDevClassNumeric.Size = New System.Drawing.Size(90, 20)
		Me.fhsDevClassNumeric.TabIndex = 29
		' 
		' fhsScanRepNumeric
		' 
		Me.fhsScanRepNumeric.Location = New System.Drawing.Point(397, 376)
		Me.fhsScanRepNumeric.Name = "fhsScanRepNumeric"
		Me.fhsScanRepNumeric.Size = New System.Drawing.Size(90, 20)
		Me.fhsScanRepNumeric.TabIndex = 30
		' 
		' fhsPgScanNumeric
		' 
		Me.fhsPgScanNumeric.Location = New System.Drawing.Point(397, 399)
		Me.fhsPgScanNumeric.Name = "fhsPgScanNumeric"
		Me.fhsPgScanNumeric.Size = New System.Drawing.Size(90, 20)
		Me.fhsPgScanNumeric.TabIndex = 31
		' 
		' fhsDevClockNumeric
		' 
		Me.fhsDevClockNumeric.Location = New System.Drawing.Point(397, 421)
		Me.fhsDevClockNumeric.Name = "fhsDevClockNumeric"
		Me.fhsDevClockNumeric.Size = New System.Drawing.Size(90, 20)
		Me.fhsDevClockNumeric.TabIndex = 32
		' 
		' whiteClkNumeric
		' 
		Me.whiteClkNumeric.Location = New System.Drawing.Point(590, 42)
		Me.whiteClkNumeric.Name = "whiteClkNumeric"
		Me.whiteClkNumeric.Size = New System.Drawing.Size(90, 20)
		Me.whiteClkNumeric.TabIndex = 34
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
		' waveNameTextBox
		' 
		Me.waveNameTextBox.Location = New System.Drawing.Point(614, 112)
		Me.waveNameTextBox.Name = "waveNameTextBox"
		Me.waveNameTextBox.Size = New System.Drawing.Size(87, 20)
		Me.waveNameTextBox.TabIndex = 35
		Me.waveNameTextBox.Text = "FHS"
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
		Me.scriptTextBox.Text = "script GenerateFHSPkt" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    repeat forever" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       generate FHS" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       generate idle" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    end repeat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " end script"
		' 
		' errorTextBox
		' 
		Me.errorTextBox.Location = New System.Drawing.Point(531, 302)
		Me.errorTextBox.Multiline = True
		Me.errorTextBox.Name = "errorTextBox"
		Me.errorTextBox.[ReadOnly] = True
		Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.errorTextBox.Size = New System.Drawing.Size(176, 177)
		Me.errorTextBox.TabIndex = 37
		Me.errorTextBox.TabStop = False
		Me.errorTextBox.Text = "No Error"
		' 
		' generateButton
		' 
		Me.generateButton.Location = New System.Drawing.Point(233, 668)
		Me.generateButton.Name = "generateButton"
		Me.generateButton.Size = New System.Drawing.Size(75, 23)
		Me.generateButton.TabIndex = 38
		Me.generateButton.Text = "&Generate"
		Me.generateButton.UseVisualStyleBackColor = True
		AddHandler Me.generateButton.Click, New System.EventHandler(AddressOf Me.generateButton_Click)
		' 
		' stopButton
		' 
		Me.stopButton.Location = New System.Drawing.Point(452, 668)
		Me.stopButton.Name = "stopButton"
		Me.stopButton.Size = New System.Drawing.Size(75, 23)
		Me.stopButton.TabIndex = 39
		Me.stopButton.Text = "&Stop"
		Me.stopButton.UseVisualStyleBackColor = True
		AddHandler Me.stopButton.Click, New System.EventHandler(AddressOf Me.stopButton_Click)
		' 
		' autoheadroomEnabComboBox
		' 
		Me.autoheadroomEnabComboBox.Location = New System.Drawing.Point(162, 129)
		Me.autoheadroomEnabComboBox.Name = "autoheadroomEnabComboBox"
		Me.autoheadroomEnabComboBox.Size = New System.Drawing.Size(90, 21)
		Me.autoheadroomEnabComboBox.TabIndex = 5
		' 
		' refSourceComboBox
		' 
		Me.refSourceComboBox.Location = New System.Drawing.Point(162, 225)
		Me.refSourceComboBox.Name = "refSourceComboBox"
		Me.refSourceComboBox.Size = New System.Drawing.Size(90, 21)
		Me.refSourceComboBox.TabIndex = 8
		' 
		' clkOutTerminalComboBox
		' 
		Me.clkOutTerminalComboBox.Location = New System.Drawing.Point(162, 246)
		Me.clkOutTerminalComboBox.Name = "clkOutTerminalComboBox"
		Me.clkOutTerminalComboBox.Size = New System.Drawing.Size(90, 21)
		Me.clkOutTerminalComboBox.TabIndex = 9
		' 
		' allIqImpairEnComboBox
		' 
		Me.allIqImpairEnComboBox.Location = New System.Drawing.Point(162, 293)
		Me.allIqImpairEnComboBox.Name = "allIqImpairEnComboBox"
		Me.allIqImpairEnComboBox.Size = New System.Drawing.Size(90, 21)
		Me.allIqImpairEnComboBox.TabIndex = 10
		' 
		' awgnEnabledComboBox
		' 
		Me.awgnEnabledComboBox.Location = New System.Drawing.Point(162, 422)
		Me.awgnEnabledComboBox.Name = "awgnEnabledComboBox"
		Me.awgnEnabledComboBox.Size = New System.Drawing.Size(90, 21)
		Me.awgnEnabledComboBox.TabIndex = 16
		' 
		' pktHdrArqnComboBox
		' 
		Me.pktHdrArqnComboBox.Location = New System.Drawing.Point(398, 156)
		Me.pktHdrArqnComboBox.Name = "pktHdrArqnComboBox"
		Me.pktHdrArqnComboBox.Size = New System.Drawing.Size(90, 21)
		Me.pktHdrArqnComboBox.TabIndex = 23
		' 
		' whiteEnComboBox
		' 
		Me.whiteEnComboBox.Location = New System.Drawing.Point(590, 22)
		Me.whiteEnComboBox.Name = "whiteEnComboBox"
		Me.whiteEnComboBox.Size = New System.Drawing.Size(90, 21)
		Me.whiteEnComboBox.TabIndex = 33
		' 
		' timer
		' 
		AddHandler Me.timer.Tick, New System.EventHandler(AddressOf Me.ProcessTimerEvent)
		' 
		' iOffsetLabel
		' 
		Me.iOffsetLabel.AutoSize = True
		Me.iOffsetLabel.Location = New System.Drawing.Point(17, 542)
		Me.iOffsetLabel.Name = "iOffsetLabel"
		Me.iOffsetLabel.Size = New System.Drawing.Size(54, 13)
		Me.iOffsetLabel.TabIndex = 76
		Me.iOffsetLabel.Text = "I Offset(V)"
		' 
		' qOffsetLabel
		' 
		Me.qOffsetLabel.AutoSize = True
		Me.qOffsetLabel.Location = New System.Drawing.Point(17, 564)
		Me.qOffsetLabel.Name = "qOffsetLabel"
		Me.qOffsetLabel.Size = New System.Drawing.Size(59, 13)
		Me.qOffsetLabel.TabIndex = 78
		Me.qOffsetLabel.Text = "Q Offset(V)"
		' 
		' iOffsetNumeric
		' 
		Me.iOffsetNumeric.Location = New System.Drawing.Point(159, 540)
		Me.iOffsetNumeric.Name = "iOffsetNumeric"
		Me.iOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.iOffsetNumeric.TabIndex = 75
		' 
		' qOffsetNumeric
		' 
		Me.qOffsetNumeric.Location = New System.Drawing.Point(157, 566)
		Me.qOffsetNumeric.Name = "qOffsetNumeric"
		Me.qOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.qOffsetNumeric.TabIndex = 77
		' 
		' iCommonModeOffsetLabel
		' 
		Me.iCommonModeOffsetLabel.AutoSize = True
		Me.iCommonModeOffsetLabel.Location = New System.Drawing.Point(17, 592)
		Me.iCommonModeOffsetLabel.Name = "iCommonModeOffsetLabel"
		Me.iCommonModeOffsetLabel.Size = New System.Drawing.Size(128, 13)
		Me.iCommonModeOffsetLabel.TabIndex = 72
		Me.iCommonModeOffsetLabel.Text = "I Common Mode Offset(V)"
		' 
		' qCommonModeOffsetLabel
		' 
		Me.qCommonModeOffsetLabel.AutoSize = True
		Me.qCommonModeOffsetLabel.Location = New System.Drawing.Point(17, 614)
		Me.qCommonModeOffsetLabel.Name = "qCommonModeOffsetLabel"
		Me.qCommonModeOffsetLabel.Size = New System.Drawing.Size(133, 13)
		Me.qCommonModeOffsetLabel.TabIndex = 74
		Me.qCommonModeOffsetLabel.Text = "Q Common Mode Offset(V)"
		' 
		' iCommonModeOffsetNumeric
		' 
		Me.iCommonModeOffsetNumeric.Location = New System.Drawing.Point(159, 590)
		Me.iCommonModeOffsetNumeric.Name = "iCommonModeOffsetNumeric"
		Me.iCommonModeOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.iCommonModeOffsetNumeric.TabIndex = 71
		' 
		' qCommonModeOffsetNumeric
		' 
		Me.qCommonModeOffsetNumeric.Location = New System.Drawing.Point(159, 612)
		Me.qCommonModeOffsetNumeric.Name = "qCommonModeOffsetNumeric"
		Me.qCommonModeOffsetNumeric.Size = New System.Drawing.Size(90, 20)
		Me.qCommonModeOffsetNumeric.TabIndex = 73
		' 
		' label2
		' 
		Me.label2.AutoSize = True
		Me.label2.Location = New System.Drawing.Point(15, 516)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(112, 13)
		Me.label2.TabIndex = 70
		Me.label2.Text = "Terminal Configuration"
		' 
		' terminalConfigurationComboBox
		' 
		Me.terminalConfigurationComboBox.Location = New System.Drawing.Point(157, 513)
		Me.terminalConfigurationComboBox.Name = "terminalConfigurationComboBox"
		Me.terminalConfigurationComboBox.Size = New System.Drawing.Size(90, 21)
		Me.terminalConfigurationComboBox.TabIndex = 69
		' 
		' label1
		' 
		Me.label1.AutoSize = True
		Me.label1.Location = New System.Drawing.Point(18, 488)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(61, 13)
		Me.label1.TabIndex = 68
		Me.label1.Text = "Output Port"
		' 
		' outputPortComboBox
		' 
		Me.outputPortComboBox.Location = New System.Drawing.Point(157, 488)
		Me.outputPortComboBox.Name = "outputPortComboBox"
		Me.outputPortComboBox.Size = New System.Drawing.Size(90, 21)
		Me.outputPortComboBox.TabIndex = 67
		' 
		' MainForm
		' 
		Me.AcceptButton = Me.generateButton
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(717, 770)
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
		Me.Controls.Add(Me.clkTerminalLabel)
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
		Me.Controls.Add(Me.pktLtAddrLabel)
		Me.Controls.Add(Me.pktHdrFlowLabel)
		Me.Controls.Add(Me.pktHdrArqnLabel)
		Me.Controls.Add(Me.pktHdrSeqnLabel)
		Me.Controls.Add(Me.fhsBdaddrLapLabel)
		Me.Controls.Add(Me.fhsBdaddrUapLabel)
		Me.Controls.Add(Me.fhsBdaddrNapLabel)
		Me.Controls.Add(Me.fhsLtAddrLabel)
		Me.Controls.Add(Me.fhsDevClassLabel)
		Me.Controls.Add(Me.fhsScanRepLabel)
		Me.Controls.Add(Me.fhsPgScanLabel)
		Me.Controls.Add(Me.fhsDevClockLabel)
		Me.Controls.Add(Me.whiteEnLabel)
		Me.Controls.Add(Me.whiteClkLabel)
		Me.Controls.Add(Me.scriptLabel)
		Me.Controls.Add(Me.waveNameLabel)
		Me.Controls.Add(Me.errorLabel)
		Me.Controls.Add(Me.hardwareLabel)
		Me.Controls.Add(Me.frequencySettingsLabel)
		Me.Controls.Add(Me.impairmentsLabel)
		Me.Controls.Add(Me.whiteningSettingsLabel)
		Me.Controls.Add(Me.slaveBDAddressLabel)
		Me.Controls.Add(Me.bdAddressLabel)
		Me.Controls.Add(Me.packetHeaderLabel)
		Me.Controls.Add(Me.fhsPayloadLabel)
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
		Me.Controls.Add(Me.pktLtAddrNumeric)
		Me.Controls.Add(Me.pktHdrFlowNumeric)
		Me.Controls.Add(Me.pktHdrSeqnNumeric)
		Me.Controls.Add(Me.fhsBdaddrLapNumeric)
		Me.Controls.Add(Me.fhsBdaddrUapNumeric)
		Me.Controls.Add(Me.fhsBdaddrNapNumeric)
		Me.Controls.Add(Me.fhsLtAddrNumeric)
		Me.Controls.Add(Me.fhsDevClassNumeric)
		Me.Controls.Add(Me.fhsScanRepNumeric)
		Me.Controls.Add(Me.fhsPgScanNumeric)
		Me.Controls.Add(Me.fhsDevClockNumeric)
		Me.Controls.Add(Me.whiteClkNumeric)
		Me.Controls.Add(Me.rfsgResourceTextBox)
		Me.Controls.Add(Me.carrierFreqTextBox)
		Me.Controls.Add(Me.actualHeadroomTextBox)
		Me.Controls.Add(Me.waveNameTextBox)
		Me.Controls.Add(Me.scriptTextBox)
		Me.Controls.Add(Me.errorTextBox)
		Me.Controls.Add(Me.generateButton)
		Me.Controls.Add(Me.stopButton)
		Me.Controls.Add(Me.autoheadroomEnabComboBox)
		Me.Controls.Add(Me.refSourceComboBox)
		Me.Controls.Add(Me.clkOutTerminalComboBox)
		Me.Controls.Add(Me.allIqImpairEnComboBox)
		Me.Controls.Add(Me.awgnEnabledComboBox)
		Me.Controls.Add(Me.pktHdrArqnComboBox)
		Me.Controls.Add(Me.whiteEnComboBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.MaximizeBox = False
		Me.Name = "MainForm"
		Me.ShowIcon = False
		Me.Text = "Bluetooth Generate FHS Packet Example"
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
		DirectCast(Me.pktLtAddrNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.pktHdrFlowNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.pktHdrSeqnNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.fhsBdaddrLapNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.fhsBdaddrUapNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.fhsBdaddrNapNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.fhsLtAddrNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.fhsDevClassNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.fhsScanRepNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.fhsPgScanNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.fhsDevClockNumeric, System.ComponentModel.ISupportInitialize).EndInit()
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
	Private clkTerminalLabel As System.Windows.Forms.Label
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
	Private pktLtAddrLabel As System.Windows.Forms.Label
	Private pktHdrFlowLabel As System.Windows.Forms.Label
	Private pktHdrArqnLabel As System.Windows.Forms.Label
	Private pktHdrSeqnLabel As System.Windows.Forms.Label
	Private fhsBdaddrLapLabel As System.Windows.Forms.Label
	Private fhsBdaddrUapLabel As System.Windows.Forms.Label
	Private fhsBdaddrNapLabel As System.Windows.Forms.Label
	Private fhsLtAddrLabel As System.Windows.Forms.Label
	Private fhsDevClassLabel As System.Windows.Forms.Label
	Private fhsScanRepLabel As System.Windows.Forms.Label
	Private fhsPgScanLabel As System.Windows.Forms.Label
	Private fhsDevClockLabel As System.Windows.Forms.Label
	Private whiteEnLabel As System.Windows.Forms.Label
	Private whiteClkLabel As System.Windows.Forms.Label
	Private waveNameLabel As System.Windows.Forms.Label
	Private scriptLabel As System.Windows.Forms.Label
	Private errorLabel As System.Windows.Forms.Label
	Private hardwareLabel As System.Windows.Forms.Label
	Private frequencySettingsLabel As System.Windows.Forms.Label
	Private impairmentsLabel As System.Windows.Forms.Label
	Private whiteningSettingsLabel As System.Windows.Forms.Label
	Private slaveBDAddressLabel As System.Windows.Forms.Label
	Private bdAddressLabel As System.Windows.Forms.Label
	Private packetHeaderLabel As System.Windows.Forms.Label
	Private fhsPayloadLabel As System.Windows.Forms.Label
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
	Private pktLtAddrNumeric As System.Windows.Forms.NumericUpDown
	Private pktHdrFlowNumeric As System.Windows.Forms.NumericUpDown
	Private pktHdrSeqnNumeric As System.Windows.Forms.NumericUpDown
	Private fhsBdaddrLapNumeric As System.Windows.Forms.NumericUpDown
	Private fhsBdaddrUapNumeric As System.Windows.Forms.NumericUpDown
	Private fhsBdaddrNapNumeric As System.Windows.Forms.NumericUpDown
	Private fhsLtAddrNumeric As System.Windows.Forms.NumericUpDown
	Private fhsDevClassNumeric As System.Windows.Forms.NumericUpDown
	Private fhsScanRepNumeric As System.Windows.Forms.NumericUpDown
	Private fhsPgScanNumeric As System.Windows.Forms.NumericUpDown
	Private fhsDevClockNumeric As System.Windows.Forms.NumericUpDown
	Private whiteClkNumeric As System.Windows.Forms.NumericUpDown
	Private rfsgResourceTextBox As System.Windows.Forms.TextBox
	Private carrierFreqTextBox As System.Windows.Forms.TextBox
	Private actualHeadroomTextBox As System.Windows.Forms.TextBox
	Private waveNameTextBox As System.Windows.Forms.TextBox
	Private scriptTextBox As System.Windows.Forms.TextBox
	Private errorTextBox As System.Windows.Forms.TextBox
	Private generateButton As System.Windows.Forms.Button
	Private stopButton As System.Windows.Forms.Button
	Private autoheadroomEnabComboBox As System.Windows.Forms.ComboBox
	Private refSourceComboBox As System.Windows.Forms.ComboBox
	Private clkOutTerminalComboBox As System.Windows.Forms.ComboBox
	Private allIqImpairEnComboBox As System.Windows.Forms.ComboBox
	Private awgnEnabledComboBox As System.Windows.Forms.ComboBox
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
