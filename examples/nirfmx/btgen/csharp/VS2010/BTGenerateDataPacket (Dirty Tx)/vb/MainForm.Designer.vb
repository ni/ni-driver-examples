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
		Me.errorLabel = New System.Windows.Forms.Label()
		Me.extmsgPacket22Label = New System.Windows.Forms.Label()
		Me.textmsgPayloadHdrLabel = New System.Windows.Forms.Label()
		Me.payloadDataLabel = New System.Windows.Forms.Label()
		Me.hardwareLabel = New System.Windows.Forms.Label()
		Me.frequencySettingLabel = New System.Windows.Forms.Label()
		Me.impairmentsLabel = New System.Windows.Forms.Label()
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
		Me.rfsgResourceTextBox = New System.Windows.Forms.TextBox()
		Me.carrierFreqTextBox = New System.Windows.Forms.TextBox()
		Me.actualHeadroomTextBox = New System.Windows.Forms.TextBox()
		Me.payHdrActPaylenTextBox = New System.Windows.Forms.TextBox()
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
		Me.payHdrPaylenModeComboBox = New System.Windows.Forms.ComboBox()
		Me.payHdrDatatypeComboBox = New System.Windows.Forms.ComboBox()
		Me.packetComboBox = New System.Windows.Forms.ComboBox()
		Me.timer = New System.Windows.Forms.Timer(Me.components)
		Me.numOfUniqNumeric = New System.Windows.Forms.NumericUpDown()
		Me.numOfIdleSlotsNumeric = New System.Windows.Forms.NumericUpDown()
		Me.numOfUniqPacketsLabel = New System.Windows.Forms.Label()
		Me.numOfIdleSlotsLabel = New System.Windows.Forms.Label()
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
		Me.whiteEnLabel = New System.Windows.Forms.Label()
		Me.whiteClkLabel = New System.Windows.Forms.Label()
		Me.whiteningSettingLabel = New System.Windows.Forms.Label()
		Me.whiteClkNumeric = New System.Windows.Forms.NumericUpDown()
		Me.whiteEnComboBox = New System.Windows.Forms.ComboBox()
		Me.dataRateLabel = New System.Windows.Forms.Label()
		Me.dataRateNumeric = New System.Windows.Forms.NumericUpDown()
		Me.LETPPayloadTypeLabel = New System.Windows.Forms.Label()
		Me.LETPPayloadTypeComboBox = New System.Windows.Forms.ComboBox()
		Me.LETPCorruptAlternateCRCLabel = New System.Windows.Forms.Label()
		Me.LETPCorruptAlternateCRCComboBox = New System.Windows.Forms.ComboBox()
		Me.highDataThroughputSettingsLabel = New System.Windows.Forms.Label()
		Me.zadoffChuIndexLabel = New System.Windows.Forms.Label()
		Me.zadoffChuIndexNumeric = New System.Windows.Forms.NumericUpDown()
		Me.physicalChannelAddressLabel = New System.Windows.Forms.Label()
		Me.physicalChannelAddressNumeric = New System.Windows.Forms.NumericUpDown()
		Me.HdtPacketFormatLabel = New System.Windows.Forms.Label()
		Me.HdtPacketFormatComboBox = New System.Windows.Forms.ComboBox()
		Me.HdtPhyIntervalLabel = New System.Windows.Forms.Label()
		Me.HdtPhyIntervalNumeric = New System.Windows.Forms.NumericUpDown()
		Me.dirtyTxSettingsLabel = New System.Windows.Forms.Label()
		Me.dirtyTxModeLabel = New System.Windows.Forms.Label()
		Me.dirtyTxModeComboBox = New System.Windows.Forms.ComboBox()
		Me.paraEnabledDataGridViewLabel = New System.Windows.Forms.Label()
		Me.paraEnabledDataGridView = New System.Windows.Forms.DataGridView()
		Me.paraEnabledDelete = New System.Windows.Forms.Button()
		Me.paraEnabledInsert = New System.Windows.Forms.Button()
		Me.carrFreqOffsetDataGridViewLabel = New System.Windows.Forms.Label()
		Me.carrFreqOffsetDataGridView = New System.Windows.Forms.DataGridView()
		Me.carrFreqOffsetIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.carrFreqOffsetSet = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.carrFreqOffDelete = New System.Windows.Forms.Button()
		Me.carrFreqOffInsert = New System.Windows.Forms.Button()
		Me.modulationIndexDataGridViewLabel = New System.Windows.Forms.Label()
		Me.modulationIndexDataGridView = New System.Windows.Forms.DataGridView()
		Me.modulationIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.modulationIndexSet = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.modulationIndexDelete = New System.Windows.Forms.Button()
		Me.modulationIndexInsert = New System.Windows.Forms.Button()
		Me.symbolTimingErrorDataGridViewLabel = New System.Windows.Forms.Label()
		Me.symbolTimingErrorDataGridView = New System.Windows.Forms.DataGridView()
		Me.symbolTimingErrorIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.symbolTimingErrorSet = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.symbolTimingErrorDelete = New System.Windows.Forms.Button()
		Me.symbolTimingErrorInsert = New System.Windows.Forms.Button()
		Me.dirtyTxModulationIndexTypeLabel = New System.Windows.Forms.Label()
		Me.dirtyTxModulationIndexTypeComboBox = New System.Windows.Forms.ComboBox()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.parametersEnabledSetIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.parametersEnabledSet = New System.Windows.Forms.DataGridViewComboBoxColumn()
		CType(Me.chnNumberNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.powerLevelNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.externalAttnNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.headroomNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.quadratureSkewNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.iDCOffsetNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.qDCOffsetNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.iqGaimbalanceNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.carrierFreqOffNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.cnrNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.bdaddrLapNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.bdaddrUapNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.bdaddrNapNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.pktLtAddrNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.pktHdrFlowNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.pktHdrSeqnNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.payHdrLlidNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.payHdrFlowNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.payHdrPaylenNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.paydatPnorderNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.paydatSeedNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.numOfUniqNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.numOfIdleSlotsNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.iOffsetNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.qOffsetNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.iCommonModeOffsetNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.qCommonModeOffsetNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.whiteClkNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.dataRateNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.zadoffChuIndexNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.physicalChannelAddressNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.HdtPhyIntervalNumeric,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.paraEnabledDataGridView,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.carrFreqOffsetDataGridView,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.modulationIndexDataGridView,System.ComponentModel.ISupportInitialize).BeginInit
		CType(Me.symbolTimingErrorDataGridView,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'rfsgResourceLabel
		'
		Me.rfsgResourceLabel.AutoSize = true
		Me.rfsgResourceLabel.Location = New System.Drawing.Point(45, 69)
		Me.rfsgResourceLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.rfsgResourceLabel.Name = "rfsgResourceLabel"
		Me.rfsgResourceLabel.Size = New System.Drawing.Size(220, 32)
		Me.rfsgResourceLabel.TabIndex = 0
		Me.rfsgResourceLabel.Text = "RFSG Resource"
		'
		'chnNumberLabel
		'
		Me.chnNumberLabel.AutoSize = true
		Me.chnNumberLabel.Location = New System.Drawing.Point(45, 112)
		Me.chnNumberLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.chnNumberLabel.Name = "chnNumberLabel"
		Me.chnNumberLabel.Size = New System.Drawing.Size(228, 32)
		Me.chnNumberLabel.TabIndex = 1
		Me.chnNumberLabel.Text = "Channel Number"
		'
		'carrierFreqLabel
		'
		Me.carrierFreqLabel.AutoSize = true
		Me.carrierFreqLabel.Location = New System.Drawing.Point(51, 162)
		Me.carrierFreqLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.carrierFreqLabel.Name = "carrierFreqLabel"
		Me.carrierFreqLabel.Size = New System.Drawing.Size(300, 32)
		Me.carrierFreqLabel.TabIndex = 2
		Me.carrierFreqLabel.Text = "Carrier Frequency (Hz)"
		'
		'powerLevelLabel
		'
		Me.powerLevelLabel.AutoSize = true
		Me.powerLevelLabel.Location = New System.Drawing.Point(45, 210)
		Me.powerLevelLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.powerLevelLabel.Name = "powerLevelLabel"
		Me.powerLevelLabel.Size = New System.Drawing.Size(253, 32)
		Me.powerLevelLabel.TabIndex = 3
		Me.powerLevelLabel.Text = "Power Level (dBm)"
		'
		'externalAttnLabel
		'
		Me.externalAttnLabel.AutoSize = true
		Me.externalAttnLabel.Location = New System.Drawing.Point(45, 262)
		Me.externalAttnLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.externalAttnLabel.Name = "externalAttnLabel"
		Me.externalAttnLabel.Size = New System.Drawing.Size(332, 32)
		Me.externalAttnLabel.TabIndex = 4
		Me.externalAttnLabel.Text = "External Attenuation (dB)"
		'
		'autoheadroomEnabLabel
		'
		Me.autoheadroomEnabLabel.AutoSize = true
		Me.autoheadroomEnabLabel.Location = New System.Drawing.Point(45, 315)
		Me.autoheadroomEnabLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.autoheadroomEnabLabel.Name = "autoheadroomEnabLabel"
		Me.autoheadroomEnabLabel.Size = New System.Drawing.Size(325, 32)
		Me.autoheadroomEnabLabel.TabIndex = 5
		Me.autoheadroomEnabLabel.Text = "Auto Headroom Enabled"
		'
		'headroomLabel
		'
		Me.headroomLabel.AutoSize = true
		Me.headroomLabel.Location = New System.Drawing.Point(45, 365)
		Me.headroomLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.headroomLabel.Name = "headroomLabel"
		Me.headroomLabel.Size = New System.Drawing.Size(206, 32)
		Me.headroomLabel.TabIndex = 6
		Me.headroomLabel.Text = "Headroom (dB)"
		'
		'actualHeadroomLabel
		'
		Me.actualHeadroomLabel.AutoSize = true
		Me.actualHeadroomLabel.Location = New System.Drawing.Point(45, 410)
		Me.actualHeadroomLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.actualHeadroomLabel.Name = "actualHeadroomLabel"
		Me.actualHeadroomLabel.Size = New System.Drawing.Size(293, 32)
		Me.actualHeadroomLabel.TabIndex = 7
		Me.actualHeadroomLabel.Text = "Actual Headroom (dB)"
		'
		'refSourceLabel
		'
		Me.refSourceLabel.AutoSize = true
		Me.refSourceLabel.Location = New System.Drawing.Point(43, 548)
		Me.refSourceLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.refSourceLabel.Name = "refSourceLabel"
		Me.refSourceLabel.Size = New System.Drawing.Size(242, 32)
		Me.refSourceLabel.TabIndex = 8
		Me.refSourceLabel.Text = "Reference Source"
		'
		'clkOutTerminalLabel
		'
		Me.clkOutTerminalLabel.AutoSize = true
		Me.clkOutTerminalLabel.Location = New System.Drawing.Point(43, 599)
		Me.clkOutTerminalLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.clkOutTerminalLabel.Name = "clkOutTerminalLabel"
		Me.clkOutTerminalLabel.Size = New System.Drawing.Size(174, 32)
		Me.clkOutTerminalLabel.TabIndex = 9
		Me.clkOutTerminalLabel.Text = "Export Clock"
		'
		'allIqImpairEnLabel
		'
		Me.allIqImpairEnLabel.AutoSize = true
		Me.allIqImpairEnLabel.Location = New System.Drawing.Point(43, 713)
		Me.allIqImpairEnLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.allIqImpairEnLabel.Name = "allIqImpairEnLabel"
		Me.allIqImpairEnLabel.Size = New System.Drawing.Size(358, 32)
		Me.allIqImpairEnLabel.TabIndex = 10
		Me.allIqImpairEnLabel.Text = "All IQ Impairments Enabled"
		'
		'quadratureSkewLabel
		'
		Me.quadratureSkewLabel.AutoSize = true
		Me.quadratureSkewLabel.Location = New System.Drawing.Point(43, 765)
		Me.quadratureSkewLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.quadratureSkewLabel.Name = "quadratureSkewLabel"
		Me.quadratureSkewLabel.Size = New System.Drawing.Size(307, 32)
		Me.quadratureSkewLabel.TabIndex = 11
		Me.quadratureSkewLabel.Text = "Quadrature Skew (deg)"
		'
		'iDCOffsetLabel
		'
		Me.iDCOffsetLabel.AutoSize = true
		Me.iDCOffsetLabel.Location = New System.Drawing.Point(43, 818)
		Me.iDCOffsetLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.iDCOffsetLabel.Name = "iDCOffsetLabel"
		Me.iDCOffsetLabel.Size = New System.Drawing.Size(201, 32)
		Me.iDCOffsetLabel.TabIndex = 12
		Me.iDCOffsetLabel.Text = "I DC Offset (%)"
		'
		'qDCOffsetLabel
		'
		Me.qDCOffsetLabel.AutoSize = true
		Me.qDCOffsetLabel.Location = New System.Drawing.Point(43, 870)
		Me.qDCOffsetLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.qDCOffsetLabel.Name = "qDCOffsetLabel"
		Me.qDCOffsetLabel.Size = New System.Drawing.Size(216, 32)
		Me.qDCOffsetLabel.TabIndex = 13
		Me.qDCOffsetLabel.Text = "Q DC Offset (%)"
		'
		'iqGaimbalanceLabel
		'
		Me.iqGaimbalanceLabel.AutoSize = true
		Me.iqGaimbalanceLabel.Location = New System.Drawing.Point(43, 923)
		Me.iqGaimbalanceLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.iqGaimbalanceLabel.Name = "iqGaimbalanceLabel"
		Me.iqGaimbalanceLabel.Size = New System.Drawing.Size(309, 32)
		Me.iqGaimbalanceLabel.TabIndex = 14
		Me.iqGaimbalanceLabel.Text = "IQ Gain Imbalance (dB)"
		'
		'carrierFreqOffLabel
		'
		Me.carrierFreqOffLabel.AutoSize = true
		Me.carrierFreqOffLabel.Location = New System.Drawing.Point(43, 971)
		Me.carrierFreqOffLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.carrierFreqOffLabel.Name = "carrierFreqOffLabel"
		Me.carrierFreqOffLabel.Size = New System.Drawing.Size(383, 32)
		Me.carrierFreqOffLabel.TabIndex = 15
		Me.carrierFreqOffLabel.Text = "Carrier Frequency Offset (Hz)"
		'
		'awgnEnabledLabel
		'
		Me.awgnEnabledLabel.AutoSize = true
		Me.awgnEnabledLabel.Location = New System.Drawing.Point(43, 1021)
		Me.awgnEnabledLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.awgnEnabledLabel.Name = "awgnEnabledLabel"
		Me.awgnEnabledLabel.Size = New System.Drawing.Size(214, 32)
		Me.awgnEnabledLabel.TabIndex = 16
		Me.awgnEnabledLabel.Text = "AWGN Enabled"
		'
		'cnrLabel
		'
		Me.cnrLabel.AutoSize = true
		Me.cnrLabel.Location = New System.Drawing.Point(43, 1068)
		Me.cnrLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.cnrLabel.Name = "cnrLabel"
		Me.cnrLabel.Size = New System.Drawing.Size(345, 32)
		Me.cnrLabel.TabIndex = 17
		Me.cnrLabel.Text = "Carrier to Noise Ratio (dB)"
		'
		'bdaddrLapLabel
		'
		Me.bdaddrLapLabel.AutoSize = true
		Me.bdaddrLapLabel.Location = New System.Drawing.Point(747, 57)
		Me.bdaddrLapLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.bdaddrLapLabel.Name = "bdaddrLapLabel"
		Me.bdaddrLapLabel.Size = New System.Drawing.Size(68, 32)
		Me.bdaddrLapLabel.TabIndex = 18
		Me.bdaddrLapLabel.Text = "LAP"
		'
		'bdaddrUapLabel
		'
		Me.bdaddrUapLabel.AutoSize = true
		Me.bdaddrUapLabel.Location = New System.Drawing.Point(747, 110)
		Me.bdaddrUapLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.bdaddrUapLabel.Name = "bdaddrUapLabel"
		Me.bdaddrUapLabel.Size = New System.Drawing.Size(72, 32)
		Me.bdaddrUapLabel.TabIndex = 19
		Me.bdaddrUapLabel.Text = "UAP"
		'
		'bdaddrNapLabel
		'
		Me.bdaddrNapLabel.AutoSize = true
		Me.bdaddrNapLabel.Location = New System.Drawing.Point(747, 162)
		Me.bdaddrNapLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.bdaddrNapLabel.Name = "bdaddrNapLabel"
		Me.bdaddrNapLabel.Size = New System.Drawing.Size(72, 32)
		Me.bdaddrNapLabel.TabIndex = 20
		Me.bdaddrNapLabel.Text = "NAP"
		'
		'pktLtAddrLabel
		'
		Me.pktLtAddrLabel.AutoSize = true
		Me.pktLtAddrLabel.Location = New System.Drawing.Point(747, 589)
		Me.pktLtAddrLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.pktLtAddrLabel.Name = "pktLtAddrLabel"
		Me.pktLtAddrLabel.Size = New System.Drawing.Size(158, 32)
		Me.pktLtAddrLabel.TabIndex = 21
		Me.pktLtAddrLabel.Text = "LT Address"
		'
		'pktHdrFlowLabel
		'
		Me.pktHdrFlowLabel.AutoSize = true
		Me.pktHdrFlowLabel.Location = New System.Drawing.Point(747, 634)
		Me.pktHdrFlowLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.pktHdrFlowLabel.Name = "pktHdrFlowLabel"
		Me.pktHdrFlowLabel.Size = New System.Drawing.Size(74, 32)
		Me.pktHdrFlowLabel.TabIndex = 22
		Me.pktHdrFlowLabel.Text = "Flow"
		'
		'pktHdrArqnLabel
		'
		Me.pktHdrArqnLabel.AutoSize = true
		Me.pktHdrArqnLabel.Location = New System.Drawing.Point(747, 680)
		Me.pktHdrArqnLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.pktHdrArqnLabel.Name = "pktHdrArqnLabel"
		Me.pktHdrArqnLabel.Size = New System.Drawing.Size(95, 32)
		Me.pktHdrArqnLabel.TabIndex = 23
		Me.pktHdrArqnLabel.Text = "ARQN"
		'
		'pktHdrSeqnLabel
		'
		Me.pktHdrSeqnLabel.AutoSize = true
		Me.pktHdrSeqnLabel.Location = New System.Drawing.Point(747, 725)
		Me.pktHdrSeqnLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.pktHdrSeqnLabel.Name = "pktHdrSeqnLabel"
		Me.pktHdrSeqnLabel.Size = New System.Drawing.Size(94, 32)
		Me.pktHdrSeqnLabel.TabIndex = 24
		Me.pktHdrSeqnLabel.Text = "SEQN"
		'
		'payHdrLlidLabel
		'
		Me.payHdrLlidLabel.AutoSize = true
		Me.payHdrLlidLabel.Location = New System.Drawing.Point(747, 892)
		Me.payHdrLlidLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.payHdrLlidLabel.Name = "payHdrLlidLabel"
		Me.payHdrLlidLabel.Size = New System.Drawing.Size(73, 32)
		Me.payHdrLlidLabel.TabIndex = 25
		Me.payHdrLlidLabel.Text = "LLID"
		'
		'payHdrFlowLabel
		'
		Me.payHdrFlowLabel.AutoSize = true
		Me.payHdrFlowLabel.Location = New System.Drawing.Point(747, 944)
		Me.payHdrFlowLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.payHdrFlowLabel.Name = "payHdrFlowLabel"
		Me.payHdrFlowLabel.Size = New System.Drawing.Size(74, 32)
		Me.payHdrFlowLabel.TabIndex = 26
		Me.payHdrFlowLabel.Text = "Flow"
		'
		'userDefinedBitLabel
		'
		Me.userDefinedBitLabel.AutoSize = true
		Me.userDefinedBitLabel.Location = New System.Drawing.Point(720, 1371)
		Me.userDefinedBitLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.userDefinedBitLabel.Name = "userDefinedBitLabel"
		Me.userDefinedBitLabel.Size = New System.Drawing.Size(319, 32)
		Me.userDefinedBitLabel.TabIndex = 27
		Me.userDefinedBitLabel.Text = "User Defined Bit Pattern"
		'
		'payHdrPaylenModeLabel
		'
		Me.payHdrPaylenModeLabel.AutoSize = true
		Me.payHdrPaylenModeLabel.Location = New System.Drawing.Point(747, 997)
		Me.payHdrPaylenModeLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.payHdrPaylenModeLabel.Name = "payHdrPaylenModeLabel"
		Me.payHdrPaylenModeLabel.Size = New System.Drawing.Size(291, 32)
		Me.payHdrPaylenModeLabel.TabIndex = 27
		Me.payHdrPaylenModeLabel.Text = "Payload Length Mode"
		'
		'payHdrPaylenLabel
		'
		Me.payHdrPaylenLabel.AutoSize = true
		Me.payHdrPaylenLabel.Location = New System.Drawing.Point(747, 1044)
		Me.payHdrPaylenLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.payHdrPaylenLabel.Name = "payHdrPaylenLabel"
		Me.payHdrPaylenLabel.Size = New System.Drawing.Size(213, 32)
		Me.payHdrPaylenLabel.TabIndex = 28
		Me.payHdrPaylenLabel.Text = "Payload Length"
		'
		'payHdrActPaylenLabel
		'
		Me.payHdrActPaylenLabel.AutoSize = true
		Me.payHdrActPaylenLabel.Location = New System.Drawing.Point(747, 1097)
		Me.payHdrActPaylenLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.payHdrActPaylenLabel.Name = "payHdrActPaylenLabel"
		Me.payHdrActPaylenLabel.Size = New System.Drawing.Size(300, 32)
		Me.payHdrActPaylenLabel.TabIndex = 29
		Me.payHdrActPaylenLabel.Text = "Actual Payload Length"
		'
		'payHdrDatatypeLabel
		'
		Me.payHdrDatatypeLabel.AutoSize = true
		Me.payHdrDatatypeLabel.Location = New System.Drawing.Point(749, 1202)
		Me.payHdrDatatypeLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.payHdrDatatypeLabel.Name = "payHdrDatatypeLabel"
		Me.payHdrDatatypeLabel.Size = New System.Drawing.Size(144, 32)
		Me.payHdrDatatypeLabel.TabIndex = 30
		Me.payHdrDatatypeLabel.Text = "Data Type"
		'
		'paydatPnorderLabel
		'
		Me.paydatPnorderLabel.AutoSize = true
		Me.paydatPnorderLabel.Location = New System.Drawing.Point(749, 1261)
		Me.paydatPnorderLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.paydatPnorderLabel.Name = "paydatPnorderLabel"
		Me.paydatPnorderLabel.Size = New System.Drawing.Size(132, 32)
		Me.paydatPnorderLabel.TabIndex = 31
		Me.paydatPnorderLabel.Text = "PN Order"
		'
		'paydatSeedLabel
		'
		Me.paydatSeedLabel.AutoSize = true
		Me.paydatSeedLabel.Location = New System.Drawing.Point(747, 1309)
		Me.paydatSeedLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.paydatSeedLabel.Name = "paydatSeedLabel"
		Me.paydatSeedLabel.Size = New System.Drawing.Size(81, 32)
		Me.paydatSeedLabel.TabIndex = 32
		Me.paydatSeedLabel.Text = "Seed"
		'
		'errorLabel
		'
		Me.errorLabel.AutoSize = true
		Me.errorLabel.Location = New System.Drawing.Point(1394, 1101)
		Me.errorLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.errorLabel.Name = "errorLabel"
		Me.errorLabel.Size = New System.Drawing.Size(76, 32)
		Me.errorLabel.TabIndex = 33
		Me.errorLabel.Text = "Error"
		'
		'extmsgPacket22Label
		'
		Me.extmsgPacket22Label.AutoSize = true
		Me.extmsgPacket22Label.Location = New System.Drawing.Point(803, 543)
		Me.extmsgPacket22Label.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.extmsgPacket22Label.Name = "extmsgPacket22Label"
		Me.extmsgPacket22Label.Size = New System.Drawing.Size(107, 32)
		Me.extmsgPacket22Label.TabIndex = 34
		Me.extmsgPacket22Label.Text = "Header"
		'
		'textmsgPayloadHdrLabel
		'
		Me.textmsgPayloadHdrLabel.AutoSize = true
		Me.textmsgPayloadHdrLabel.Location = New System.Drawing.Point(747, 844)
		Me.textmsgPayloadHdrLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.textmsgPayloadHdrLabel.Name = "textmsgPayloadHdrLabel"
		Me.textmsgPayloadHdrLabel.Size = New System.Drawing.Size(107, 32)
		Me.textmsgPayloadHdrLabel.TabIndex = 35
		Me.textmsgPayloadHdrLabel.Text = "Header"
		'
		'payloadDataLabel
		'
		Me.payloadDataLabel.AutoSize = true
		Me.payloadDataLabel.Location = New System.Drawing.Point(747, 1159)
		Me.payloadDataLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.payloadDataLabel.Name = "payloadDataLabel"
		Me.payloadDataLabel.Size = New System.Drawing.Size(74, 32)
		Me.payloadDataLabel.TabIndex = 36
		Me.payloadDataLabel.Text = "Data"
		'
		'hardwareLabel
		'
		Me.hardwareLabel.AutoSize = true
		Me.hardwareLabel.Location = New System.Drawing.Point(80, 14)
		Me.hardwareLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.hardwareLabel.Name = "hardwareLabel"
		Me.hardwareLabel.Size = New System.Drawing.Size(136, 32)
		Me.hardwareLabel.TabIndex = 37
		Me.hardwareLabel.Text = "Hardware"
		'
		'frequencySettingLabel
		'
		Me.frequencySettingLabel.AutoSize = true
		Me.frequencySettingLabel.Location = New System.Drawing.Point(77, 506)
		Me.frequencySettingLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.frequencySettingLabel.Name = "frequencySettingLabel"
		Me.frequencySettingLabel.Size = New System.Drawing.Size(259, 32)
		Me.frequencySettingLabel.TabIndex = 38
		Me.frequencySettingLabel.Text = "Frequency Settings"
		'
		'impairmentsLabel
		'
		Me.impairmentsLabel.AutoSize = true
		Me.impairmentsLabel.Location = New System.Drawing.Point(77, 663)
		Me.impairmentsLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.impairmentsLabel.Name = "impairmentsLabel"
		Me.impairmentsLabel.Size = New System.Drawing.Size(169, 32)
		Me.impairmentsLabel.TabIndex = 39
		Me.impairmentsLabel.Text = "Impairments"
		'
		'bdAddressLabel
		'
		Me.bdAddressLabel.AutoSize = true
		Me.bdAddressLabel.Location = New System.Drawing.Point(803, 14)
		Me.bdAddressLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.bdAddressLabel.Name = "bdAddressLabel"
		Me.bdAddressLabel.Size = New System.Drawing.Size(164, 32)
		Me.bdAddressLabel.TabIndex = 40
		Me.bdAddressLabel.Text = "BD Address"
		'
		'packetLabel
		'
		Me.packetLabel.AutoSize = true
		Me.packetLabel.Location = New System.Drawing.Point(747, 219)
		Me.packetLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.packetLabel.Name = "packetLabel"
		Me.packetLabel.Size = New System.Drawing.Size(101, 32)
		Me.packetLabel.TabIndex = 41
		Me.packetLabel.Text = "Packet"
		'
		'payloadLabel
		'
		Me.payloadLabel.AutoSize = true
		Me.payloadLabel.Location = New System.Drawing.Point(803, 801)
		Me.payloadLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.payloadLabel.Name = "payloadLabel"
		Me.payloadLabel.Size = New System.Drawing.Size(118, 32)
		Me.payloadLabel.TabIndex = 42
		Me.payloadLabel.Text = "Payload"
		'
		'chnNumberNumeric
		'
		Me.chnNumberNumeric.Location = New System.Drawing.Point(432, 103)
		Me.chnNumberNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.chnNumberNumeric.Name = "chnNumberNumeric"
		Me.chnNumberNumeric.Size = New System.Drawing.Size(240, 38)
		Me.chnNumberNumeric.TabIndex = 1
		'
		'powerLevelNumeric
		'
		Me.powerLevelNumeric.Location = New System.Drawing.Point(432, 203)
		Me.powerLevelNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.powerLevelNumeric.Name = "powerLevelNumeric"
		Me.powerLevelNumeric.Size = New System.Drawing.Size(240, 38)
		Me.powerLevelNumeric.TabIndex = 3
		'
		'externalAttnNumeric
		'
		Me.externalAttnNumeric.Location = New System.Drawing.Point(432, 255)
		Me.externalAttnNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.externalAttnNumeric.Name = "externalAttnNumeric"
		Me.externalAttnNumeric.Size = New System.Drawing.Size(240, 38)
		Me.externalAttnNumeric.TabIndex = 4
		'
		'headroomNumeric
		'
		Me.headroomNumeric.Location = New System.Drawing.Point(432, 358)
		Me.headroomNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.headroomNumeric.Name = "headroomNumeric"
		Me.headroomNumeric.Size = New System.Drawing.Size(240, 38)
		Me.headroomNumeric.TabIndex = 6
		'
		'quadratureSkewNumeric
		'
		Me.quadratureSkewNumeric.Location = New System.Drawing.Point(429, 763)
		Me.quadratureSkewNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.quadratureSkewNumeric.Name = "quadratureSkewNumeric"
		Me.quadratureSkewNumeric.Size = New System.Drawing.Size(240, 38)
		Me.quadratureSkewNumeric.TabIndex = 11
		'
		'iDCOffsetNumeric
		'
		Me.iDCOffsetNumeric.Location = New System.Drawing.Point(429, 816)
		Me.iDCOffsetNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.iDCOffsetNumeric.Name = "iDCOffsetNumeric"
		Me.iDCOffsetNumeric.Size = New System.Drawing.Size(240, 38)
		Me.iDCOffsetNumeric.TabIndex = 12
		'
		'qDCOffsetNumeric
		'
		Me.qDCOffsetNumeric.Location = New System.Drawing.Point(429, 868)
		Me.qDCOffsetNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.qDCOffsetNumeric.Name = "qDCOffsetNumeric"
		Me.qDCOffsetNumeric.Size = New System.Drawing.Size(240, 38)
		Me.qDCOffsetNumeric.TabIndex = 13
		'
		'iqGaimbalanceNumeric
		'
		Me.iqGaimbalanceNumeric.Location = New System.Drawing.Point(429, 920)
		Me.iqGaimbalanceNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.iqGaimbalanceNumeric.Name = "iqGaimbalanceNumeric"
		Me.iqGaimbalanceNumeric.Size = New System.Drawing.Size(240, 38)
		Me.iqGaimbalanceNumeric.TabIndex = 14
		'
		'carrierFreqOffNumeric
		'
		Me.carrierFreqOffNumeric.Location = New System.Drawing.Point(429, 966)
		Me.carrierFreqOffNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.carrierFreqOffNumeric.Name = "carrierFreqOffNumeric"
		Me.carrierFreqOffNumeric.Size = New System.Drawing.Size(240, 38)
		Me.carrierFreqOffNumeric.TabIndex = 15
		'
		'cnrNumeric
		'
		Me.cnrNumeric.Location = New System.Drawing.Point(429, 1066)
		Me.cnrNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.cnrNumeric.Name = "cnrNumeric"
		Me.cnrNumeric.Size = New System.Drawing.Size(240, 38)
		Me.cnrNumeric.TabIndex = 17
		'
		'bdaddrLapNumeric
		'
		Me.bdaddrLapNumeric.Location = New System.Drawing.Point(1059, 50)
		Me.bdaddrLapNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.bdaddrLapNumeric.Name = "bdaddrLapNumeric"
		Me.bdaddrLapNumeric.Size = New System.Drawing.Size(240, 38)
		Me.bdaddrLapNumeric.TabIndex = 18
		'
		'bdaddrUapNumeric
		'
		Me.bdaddrUapNumeric.Location = New System.Drawing.Point(1059, 105)
		Me.bdaddrUapNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.bdaddrUapNumeric.Name = "bdaddrUapNumeric"
		Me.bdaddrUapNumeric.Size = New System.Drawing.Size(240, 38)
		Me.bdaddrUapNumeric.TabIndex = 19
		'
		'bdaddrNapNumeric
		'
		Me.bdaddrNapNumeric.Location = New System.Drawing.Point(1059, 150)
		Me.bdaddrNapNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.bdaddrNapNumeric.Name = "bdaddrNapNumeric"
		Me.bdaddrNapNumeric.Size = New System.Drawing.Size(240, 38)
		Me.bdaddrNapNumeric.TabIndex = 20
		'
		'pktLtAddrNumeric
		'
		Me.pktLtAddrNumeric.Location = New System.Drawing.Point(1059, 572)
		Me.pktLtAddrNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.pktLtAddrNumeric.Name = "pktLtAddrNumeric"
		Me.pktLtAddrNumeric.Size = New System.Drawing.Size(240, 38)
		Me.pktLtAddrNumeric.TabIndex = 22
		'
		'pktHdrFlowNumeric
		'
		Me.pktHdrFlowNumeric.Location = New System.Drawing.Point(1059, 632)
		Me.pktHdrFlowNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.pktHdrFlowNumeric.Name = "pktHdrFlowNumeric"
		Me.pktHdrFlowNumeric.Size = New System.Drawing.Size(240, 38)
		Me.pktHdrFlowNumeric.TabIndex = 23
		'
		'pktHdrSeqnNumeric
		'
		Me.pktHdrSeqnNumeric.Location = New System.Drawing.Point(1059, 723)
		Me.pktHdrSeqnNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.pktHdrSeqnNumeric.Name = "pktHdrSeqnNumeric"
		Me.pktHdrSeqnNumeric.Size = New System.Drawing.Size(240, 38)
		Me.pktHdrSeqnNumeric.TabIndex = 25
		'
		'payHdrLlidNumeric
		'
		Me.payHdrLlidNumeric.Location = New System.Drawing.Point(1061, 889)
		Me.payHdrLlidNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.payHdrLlidNumeric.Name = "payHdrLlidNumeric"
		Me.payHdrLlidNumeric.Size = New System.Drawing.Size(240, 38)
		Me.payHdrLlidNumeric.TabIndex = 26
		'
		'payHdrFlowNumeric
		'
		Me.payHdrFlowNumeric.Location = New System.Drawing.Point(1061, 942)
		Me.payHdrFlowNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.payHdrFlowNumeric.Name = "payHdrFlowNumeric"
		Me.payHdrFlowNumeric.Size = New System.Drawing.Size(240, 38)
		Me.payHdrFlowNumeric.TabIndex = 27
		'
		'payHdrPaylenNumeric
		'
		Me.payHdrPaylenNumeric.Location = New System.Drawing.Point(1061, 1042)
		Me.payHdrPaylenNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.payHdrPaylenNumeric.Name = "payHdrPaylenNumeric"
		Me.payHdrPaylenNumeric.Size = New System.Drawing.Size(240, 38)
		Me.payHdrPaylenNumeric.TabIndex = 29
		'
		'paydatPnorderNumeric
		'
		Me.paydatPnorderNumeric.Location = New System.Drawing.Point(1059, 1250)
		Me.paydatPnorderNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.paydatPnorderNumeric.Name = "paydatPnorderNumeric"
		Me.paydatPnorderNumeric.Size = New System.Drawing.Size(240, 38)
		Me.paydatPnorderNumeric.TabIndex = 32
		'
		'paydatSeedNumeric
		'
		Me.paydatSeedNumeric.Location = New System.Drawing.Point(1059, 1302)
		Me.paydatSeedNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.paydatSeedNumeric.Name = "paydatSeedNumeric"
		Me.paydatSeedNumeric.Size = New System.Drawing.Size(240, 38)
		Me.paydatSeedNumeric.TabIndex = 33
		'
		'rfsgResourceTextBox
		'
		Me.rfsgResourceTextBox.Location = New System.Drawing.Point(432, 52)
		Me.rfsgResourceTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.rfsgResourceTextBox.Name = "rfsgResourceTextBox"
		Me.rfsgResourceTextBox.Size = New System.Drawing.Size(233, 38)
		Me.rfsgResourceTextBox.TabIndex = 0
		Me.rfsgResourceTextBox.Text = "RFSG"
		'
		'carrierFreqTextBox
		'
		Me.carrierFreqTextBox.Enabled = false
		Me.carrierFreqTextBox.Location = New System.Drawing.Point(432, 150)
		Me.carrierFreqTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.carrierFreqTextBox.Name = "carrierFreqTextBox"
		Me.carrierFreqTextBox.Size = New System.Drawing.Size(233, 38)
		Me.carrierFreqTextBox.TabIndex = 2
		Me.carrierFreqTextBox.Text = "2.402E+9"
		'
		'actualHeadroomTextBox
		'
		Me.actualHeadroomTextBox.Enabled = false
		Me.actualHeadroomTextBox.Location = New System.Drawing.Point(432, 403)
		Me.actualHeadroomTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.actualHeadroomTextBox.Name = "actualHeadroomTextBox"
		Me.actualHeadroomTextBox.Size = New System.Drawing.Size(233, 38)
		Me.actualHeadroomTextBox.TabIndex = 7
		Me.actualHeadroomTextBox.Text = "5.00"
		'
		'payHdrActPaylenTextBox
		'
		Me.payHdrActPaylenTextBox.Enabled = false
		Me.payHdrActPaylenTextBox.Location = New System.Drawing.Point(1064, 1095)
		Me.payHdrActPaylenTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.payHdrActPaylenTextBox.Name = "payHdrActPaylenTextBox"
		Me.payHdrActPaylenTextBox.Size = New System.Drawing.Size(233, 38)
		Me.payHdrActPaylenTextBox.TabIndex = 30
		Me.payHdrActPaylenTextBox.Text = "0"
		'
		'errorTextBox
		'
		Me.errorTextBox.Location = New System.Drawing.Point(1400, 1152)
		Me.errorTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.errorTextBox.Multiline = true
		Me.errorTextBox.Name = "errorTextBox"
		Me.errorTextBox.ReadOnly = true
		Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.errorTextBox.Size = New System.Drawing.Size(463, 314)
		Me.errorTextBox.TabIndex = 34
		Me.errorTextBox.TabStop = false
		Me.errorTextBox.Text = "No Error"
		'
		'generateButton
		'
		Me.generateButton.Location = New System.Drawing.Point(581, 1610)
		Me.generateButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.generateButton.Name = "generateButton"
		Me.generateButton.Size = New System.Drawing.Size(200, 55)
		Me.generateButton.TabIndex = 35
		Me.generateButton.Text = "&Generate"
		Me.generateButton.UseVisualStyleBackColor = true
		'
		'stopButton
		'
		Me.stopButton.Location = New System.Drawing.Point(1104, 1610)
		Me.stopButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.stopButton.Name = "stopButton"
		Me.stopButton.Size = New System.Drawing.Size(200, 55)
		Me.stopButton.TabIndex = 36
		Me.stopButton.Text = "&Stop"
		Me.stopButton.UseVisualStyleBackColor = true
		'
		'autoHeadroomEnabComboBox
		'
		Me.autoHeadroomEnabComboBox.Location = New System.Drawing.Point(432, 308)
		Me.autoHeadroomEnabComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.autoHeadroomEnabComboBox.Name = "autoHeadroomEnabComboBox"
		Me.autoHeadroomEnabComboBox.Size = New System.Drawing.Size(233, 39)
		Me.autoHeadroomEnabComboBox.TabIndex = 5
		'
		'refSourceComboBox
		'
		Me.refSourceComboBox.Location = New System.Drawing.Point(429, 548)
		Me.refSourceComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.refSourceComboBox.Name = "refSourceComboBox"
		Me.refSourceComboBox.Size = New System.Drawing.Size(233, 39)
		Me.refSourceComboBox.TabIndex = 8
		'
		'userDefinedBitsComboBox
		'
		Me.userDefinedBitsComboBox.Location = New System.Drawing.Point(1059, 1364)
		Me.userDefinedBitsComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.userDefinedBitsComboBox.Name = "userDefinedBitsComboBox"
		Me.userDefinedBitsComboBox.Size = New System.Drawing.Size(255, 39)
		Me.userDefinedBitsComboBox.TabIndex = 8
		'
		'clkOutTerminalComboBox
		'
		Me.clkOutTerminalComboBox.Location = New System.Drawing.Point(429, 599)
		Me.clkOutTerminalComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.clkOutTerminalComboBox.Name = "clkOutTerminalComboBox"
		Me.clkOutTerminalComboBox.Size = New System.Drawing.Size(233, 39)
		Me.clkOutTerminalComboBox.TabIndex = 9
		'
		'allIqImpairEnComboBox
		'
		Me.allIqImpairEnComboBox.Location = New System.Drawing.Point(429, 711)
		Me.allIqImpairEnComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.allIqImpairEnComboBox.Name = "allIqImpairEnComboBox"
		Me.allIqImpairEnComboBox.Size = New System.Drawing.Size(233, 39)
		Me.allIqImpairEnComboBox.TabIndex = 10
		'
		'awgnEnabledComboBox
		'
		Me.awgnEnabledComboBox.Location = New System.Drawing.Point(429, 1018)
		Me.awgnEnabledComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.awgnEnabledComboBox.Name = "awgnEnabledComboBox"
		Me.awgnEnabledComboBox.Size = New System.Drawing.Size(233, 39)
		Me.awgnEnabledComboBox.TabIndex = 16
		'
		'pktHdrArqnComboBox
		'
		Me.pktHdrArqnComboBox.Location = New System.Drawing.Point(1059, 677)
		Me.pktHdrArqnComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.pktHdrArqnComboBox.Name = "pktHdrArqnComboBox"
		Me.pktHdrArqnComboBox.Size = New System.Drawing.Size(242, 39)
		Me.pktHdrArqnComboBox.TabIndex = 24
		'
		'payHdrPaylenModeComboBox
		'
		Me.payHdrPaylenModeComboBox.Location = New System.Drawing.Point(1061, 994)
		Me.payHdrPaylenModeComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.payHdrPaylenModeComboBox.Name = "payHdrPaylenModeComboBox"
		Me.payHdrPaylenModeComboBox.Size = New System.Drawing.Size(238, 39)
		Me.payHdrPaylenModeComboBox.TabIndex = 28
		'
		'payHdrDatatypeComboBox
		'
		Me.payHdrDatatypeComboBox.Location = New System.Drawing.Point(1061, 1202)
		Me.payHdrDatatypeComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.payHdrDatatypeComboBox.Name = "payHdrDatatypeComboBox"
		Me.payHdrDatatypeComboBox.Size = New System.Drawing.Size(233, 39)
		Me.payHdrDatatypeComboBox.TabIndex = 31
		'
		'packetComboBox
		'
		Me.packetComboBox.Location = New System.Drawing.Point(1059, 212)
		Me.packetComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.packetComboBox.Name = "packetComboBox"
		Me.packetComboBox.Size = New System.Drawing.Size(240, 39)
		Me.packetComboBox.TabIndex = 28
		Me.packetComboBox.Text = "DH1"
		'
		'numOfUniqNumeric
		'
		Me.numOfUniqNumeric.Location = New System.Drawing.Point(1059, 319)
		Me.numOfUniqNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.numOfUniqNumeric.Name = "numOfUniqNumeric"
		Me.numOfUniqNumeric.Size = New System.Drawing.Size(240, 38)
		Me.numOfUniqNumeric.TabIndex = 43
		Me.numOfUniqNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'numOfIdleSlotsNumeric
		'
		Me.numOfIdleSlotsNumeric.Location = New System.Drawing.Point(1059, 369)
		Me.numOfIdleSlotsNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.numOfIdleSlotsNumeric.Name = "numOfIdleSlotsNumeric"
		Me.numOfIdleSlotsNumeric.Size = New System.Drawing.Size(240, 38)
		Me.numOfIdleSlotsNumeric.TabIndex = 44
		Me.numOfIdleSlotsNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
		'
		'numOfUniqPacketsLabel
		'
		Me.numOfUniqPacketsLabel.AutoSize = true
		Me.numOfUniqPacketsLabel.Location = New System.Drawing.Point(688, 324)
		Me.numOfUniqPacketsLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.numOfUniqPacketsLabel.Name = "numOfUniqPacketsLabel"
		Me.numOfUniqPacketsLabel.Size = New System.Drawing.Size(357, 32)
		Me.numOfUniqPacketsLabel.TabIndex = 45
		Me.numOfUniqPacketsLabel.Text = "Number Of Unique Packets"
		'
		'numOfIdleSlotsLabel
		'
		Me.numOfIdleSlotsLabel.AutoSize = true
		Me.numOfIdleSlotsLabel.Location = New System.Drawing.Point(688, 374)
		Me.numOfIdleSlotsLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.numOfIdleSlotsLabel.Name = "numOfIdleSlotsLabel"
		Me.numOfIdleSlotsLabel.Size = New System.Drawing.Size(275, 32)
		Me.numOfIdleSlotsLabel.TabIndex = 46
		Me.numOfIdleSlotsLabel.Text = "Number Of Idle Slots"
		'
		'iOffsetLabel
		'
		Me.iOffsetLabel.AutoSize = true
		Me.iOffsetLabel.Location = New System.Drawing.Point(51, 1285)
		Me.iOffsetLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.iOffsetLabel.Name = "iOffsetLabel"
		Me.iOffsetLabel.Size = New System.Drawing.Size(141, 32)
		Me.iOffsetLabel.TabIndex = 76
		Me.iOffsetLabel.Text = "I Offset(V)"
		'
		'qOffsetLabel
		'
		Me.qOffsetLabel.AutoSize = true
		Me.qOffsetLabel.Location = New System.Drawing.Point(51, 1338)
		Me.qOffsetLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.qOffsetLabel.Name = "qOffsetLabel"
		Me.qOffsetLabel.Size = New System.Drawing.Size(156, 32)
		Me.qOffsetLabel.TabIndex = 78
		Me.qOffsetLabel.Text = "Q Offset(V)"
		'
		'iOffsetNumeric
		'
		Me.iOffsetNumeric.Location = New System.Drawing.Point(429, 1281)
		Me.iOffsetNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.iOffsetNumeric.Name = "iOffsetNumeric"
		Me.iOffsetNumeric.Size = New System.Drawing.Size(240, 38)
		Me.iOffsetNumeric.TabIndex = 75
		'
		'qOffsetNumeric
		'
		Me.qOffsetNumeric.Location = New System.Drawing.Point(424, 1343)
		Me.qOffsetNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.qOffsetNumeric.Name = "qOffsetNumeric"
		Me.qOffsetNumeric.Size = New System.Drawing.Size(240, 38)
		Me.qOffsetNumeric.TabIndex = 77
		'
		'iCommonModeOffsetLabel
		'
		Me.iCommonModeOffsetLabel.AutoSize = true
		Me.iCommonModeOffsetLabel.Location = New System.Drawing.Point(51, 1405)
		Me.iCommonModeOffsetLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.iCommonModeOffsetLabel.Name = "iCommonModeOffsetLabel"
		Me.iCommonModeOffsetLabel.Size = New System.Drawing.Size(340, 32)
		Me.iCommonModeOffsetLabel.TabIndex = 72
		Me.iCommonModeOffsetLabel.Text = "I Common Mode Offset(V)"
		'
		'qCommonModeOffsetLabel
		'
		Me.qCommonModeOffsetLabel.AutoSize = true
		Me.qCommonModeOffsetLabel.Location = New System.Drawing.Point(51, 1457)
		Me.qCommonModeOffsetLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.qCommonModeOffsetLabel.Name = "qCommonModeOffsetLabel"
		Me.qCommonModeOffsetLabel.Size = New System.Drawing.Size(355, 32)
		Me.qCommonModeOffsetLabel.TabIndex = 74
		Me.qCommonModeOffsetLabel.Text = "Q Common Mode Offset(V)"
		'
		'iCommonModeOffsetNumeric
		'
		Me.iCommonModeOffsetNumeric.Location = New System.Drawing.Point(429, 1400)
		Me.iCommonModeOffsetNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.iCommonModeOffsetNumeric.Name = "iCommonModeOffsetNumeric"
		Me.iCommonModeOffsetNumeric.Size = New System.Drawing.Size(240, 38)
		Me.iCommonModeOffsetNumeric.TabIndex = 71
		'
		'qCommonModeOffsetNumeric
		'
		Me.qCommonModeOffsetNumeric.Location = New System.Drawing.Point(429, 1452)
		Me.qCommonModeOffsetNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.qCommonModeOffsetNumeric.Name = "qCommonModeOffsetNumeric"
		Me.qCommonModeOffsetNumeric.Size = New System.Drawing.Size(240, 38)
		Me.qCommonModeOffsetNumeric.TabIndex = 73
		'
		'label2
		'
		Me.label2.AutoSize = true
		Me.label2.Location = New System.Drawing.Point(45, 1223)
		Me.label2.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(303, 32)
		Me.label2.TabIndex = 70
		Me.label2.Text = "Terminal Configuration"
		'
		'terminalConfigurationComboBox
		'
		Me.terminalConfigurationComboBox.Location = New System.Drawing.Point(424, 1216)
		Me.terminalConfigurationComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.terminalConfigurationComboBox.Name = "terminalConfigurationComboBox"
		Me.terminalConfigurationComboBox.Size = New System.Drawing.Size(233, 39)
		Me.terminalConfigurationComboBox.TabIndex = 69
		'
		'label1
		'
		Me.label1.AutoSize = true
		Me.label1.Location = New System.Drawing.Point(53, 1157)
		Me.label1.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(159, 32)
		Me.label1.TabIndex = 68
		Me.label1.Text = "Output Port"
		'
		'outputPortComboBox
		'
		Me.outputPortComboBox.Location = New System.Drawing.Point(424, 1157)
		Me.outputPortComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.outputPortComboBox.Name = "outputPortComboBox"
		Me.outputPortComboBox.Size = New System.Drawing.Size(233, 39)
		Me.outputPortComboBox.TabIndex = 67
		'
		'whiteEnLabel
		'
		Me.whiteEnLabel.AutoSize = true
		Me.whiteEnLabel.Location = New System.Drawing.Point(1454, 69)
		Me.whiteEnLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.whiteEnLabel.Name = "whiteEnLabel"
		Me.whiteEnLabel.Size = New System.Drawing.Size(120, 32)
		Me.whiteEnLabel.TabIndex = 79
		Me.whiteEnLabel.Text = "Enabled"
		'
		'whiteClkLabel
		'
		Me.whiteClkLabel.AutoSize = true
		Me.whiteClkLabel.Location = New System.Drawing.Point(1454, 136)
		Me.whiteClkLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.whiteClkLabel.Name = "whiteClkLabel"
		Me.whiteClkLabel.Size = New System.Drawing.Size(85, 32)
		Me.whiteClkLabel.TabIndex = 81
		Me.whiteClkLabel.Text = "Clock"
		'
		'whiteningSettingLabel
		'
		Me.whiteningSettingLabel.AutoSize = true
		Me.whiteningSettingLabel.Location = New System.Drawing.Point(1523, 21)
		Me.whiteningSettingLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.whiteningSettingLabel.Name = "whiteningSettingLabel"
		Me.whiteningSettingLabel.Size = New System.Drawing.Size(253, 32)
		Me.whiteningSettingLabel.TabIndex = 83
		Me.whiteningSettingLabel.Text = "Whitening Settings"
		'
		'whiteClkNumeric
		'
		Me.whiteClkNumeric.Location = New System.Drawing.Point(1601, 131)
		Me.whiteClkNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.whiteClkNumeric.Name = "whiteClkNumeric"
		Me.whiteClkNumeric.Size = New System.Drawing.Size(240, 38)
		Me.whiteClkNumeric.TabIndex = 82
		'
		'whiteEnComboBox
		'
		Me.whiteEnComboBox.Location = New System.Drawing.Point(1601, 67)
		Me.whiteEnComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.whiteEnComboBox.Name = "whiteEnComboBox"
		Me.whiteEnComboBox.Size = New System.Drawing.Size(240, 39)
		Me.whiteEnComboBox.TabIndex = 80
		'
		'dataRateLabel
		'
		Me.dataRateLabel.AutoSize = true
		Me.dataRateLabel.Location = New System.Drawing.Point(720, 270)
		Me.dataRateLabel.Margin = New System.Windows.Forms.Padding(56, 0, 56, 0)
		Me.dataRateLabel.Name = "dataRateLabel"
		Me.dataRateLabel.Size = New System.Drawing.Size(212, 32)
		Me.dataRateLabel.TabIndex = 45
		Me.dataRateLabel.Text = "Data Rate (bps)"
		'
		'dataRateNumeric
		'
		Me.dataRateNumeric.Location = New System.Drawing.Point(1059, 265)
		Me.dataRateNumeric.Margin = New System.Windows.Forms.Padding(56, 41, 56, 41)
		Me.dataRateNumeric.Maximum = New Decimal(New Integer() {7500000, 0, 0, 0})
		Me.dataRateNumeric.Name = "dataRateNumeric"
		Me.dataRateNumeric.Size = New System.Drawing.Size(240, 38)
		Me.dataRateNumeric.TabIndex = 43
		Me.dataRateNumeric.Value = New Decimal(New Integer() {2000000, 0, 0, 0})
		'
		'LETPPayloadTypeLabel
		'
		Me.LETPPayloadTypeLabel.AutoSize = true
		Me.LETPPayloadTypeLabel.Location = New System.Drawing.Point(685, 425)
		Me.LETPPayloadTypeLabel.Margin = New System.Windows.Forms.Padding(56, 0, 56, 0)
		Me.LETPPayloadTypeLabel.Name = "LETPPayloadTypeLabel"
		Me.LETPPayloadTypeLabel.Size = New System.Drawing.Size(275, 32)
		Me.LETPPayloadTypeLabel.TabIndex = 41
		Me.LETPPayloadTypeLabel.Text = "LE-TP Payload Type"
		'
		'LETPPayloadTypeComboBox
		'
		Me.LETPPayloadTypeComboBox.Location = New System.Drawing.Point(1059, 422)
		Me.LETPPayloadTypeComboBox.Margin = New System.Windows.Forms.Padding(56, 41, 56, 41)
		Me.LETPPayloadTypeComboBox.Name = "LETPPayloadTypeComboBox"
		Me.LETPPayloadTypeComboBox.Size = New System.Drawing.Size(240, 39)
		Me.LETPPayloadTypeComboBox.TabIndex = 28
		Me.LETPPayloadTypeComboBox.Text = "PRBS9"
		'
		'LETPCorruptAlternateCRCLabel
		'
		Me.LETPCorruptAlternateCRCLabel.AutoSize = true
		Me.LETPCorruptAlternateCRCLabel.Location = New System.Drawing.Point(663, 478)
		Me.LETPCorruptAlternateCRCLabel.Margin = New System.Windows.Forms.Padding(56, 0, 56, 0)
		Me.LETPCorruptAlternateCRCLabel.Name = "LETPCorruptAlternateCRCLabel"
		Me.LETPCorruptAlternateCRCLabel.Size = New System.Drawing.Size(384, 32)
		Me.LETPCorruptAlternateCRCLabel.TabIndex = 41
		Me.LETPCorruptAlternateCRCLabel.Text = "LE-TP Corrupt Alternate CRC"
		'
		'LETPCorruptAlternateCRCComboBox
		'
		Me.LETPCorruptAlternateCRCComboBox.Location = New System.Drawing.Point(1059, 474)
		Me.LETPCorruptAlternateCRCComboBox.Margin = New System.Windows.Forms.Padding(56, 41, 56, 41)
		Me.LETPCorruptAlternateCRCComboBox.Name = "LETPCorruptAlternateCRCComboBox"
		Me.LETPCorruptAlternateCRCComboBox.Size = New System.Drawing.Size(240, 39)
		Me.LETPCorruptAlternateCRCComboBox.TabIndex = 28
		Me.LETPCorruptAlternateCRCComboBox.Text = "False"
		'
		'highDataThroughputSettingsLabel
		'
		Me.highDataThroughputSettingsLabel.AutoSize = true
		Me.highDataThroughputSettingsLabel.Location = New System.Drawing.Point(4147, 506)
		Me.highDataThroughputSettingsLabel.Margin = New System.Windows.Forms.Padding(21, 0, 21, 0)
		Me.highDataThroughputSettingsLabel.Name = "highDataThroughputSettingsLabel"
		Me.highDataThroughputSettingsLabel.Size = New System.Drawing.Size(404, 32)
		Me.highDataThroughputSettingsLabel.TabIndex = 83
		Me.highDataThroughputSettingsLabel.Text = "High Data Throughput Settings"
		'
		'zadoffChuIndexLabel
		'
		Me.zadoffChuIndexLabel.AutoSize = true
		Me.zadoffChuIndexLabel.Location = New System.Drawing.Point(1394, 283)
		Me.zadoffChuIndexLabel.Margin = New System.Windows.Forms.Padding(56, 0, 56, 0)
		Me.zadoffChuIndexLabel.Name = "zadoffChuIndexLabel"
		Me.zadoffChuIndexLabel.Size = New System.Drawing.Size(232, 32)
		Me.zadoffChuIndexLabel.TabIndex = 45
		Me.zadoffChuIndexLabel.Text = "Zadoff-Chu Index"
		'
		'zadoffChuIndexNumeric
		'
		Me.zadoffChuIndexNumeric.Location = New System.Drawing.Point(1687, 277)
		Me.zadoffChuIndexNumeric.Margin = New System.Windows.Forms.Padding(56, 41, 56, 41)
		Me.zadoffChuIndexNumeric.Name = "zadoffChuIndexNumeric"
		Me.zadoffChuIndexNumeric.Size = New System.Drawing.Size(240, 38)
		Me.zadoffChuIndexNumeric.TabIndex = 43
		Me.zadoffChuIndexNumeric.Value = New Decimal(New Integer() {7, 0, 0, 0})
		'
		'physicalChannelAddressLabel
		'
		Me.physicalChannelAddressLabel.AutoSize = true
		Me.physicalChannelAddressLabel.Location = New System.Drawing.Point(1326, 345)
		Me.physicalChannelAddressLabel.Margin = New System.Windows.Forms.Padding(56, 0, 56, 0)
		Me.physicalChannelAddressLabel.Name = "physicalChannelAddressLabel"
		Me.physicalChannelAddressLabel.Size = New System.Drawing.Size(346, 32)
		Me.physicalChannelAddressLabel.TabIndex = 45
		Me.physicalChannelAddressLabel.Text = "Physical Channel Address"
		'
		'physicalChannelAddressNumeric
		'
		Me.physicalChannelAddressNumeric.Hexadecimal = true
		Me.physicalChannelAddressNumeric.Location = New System.Drawing.Point(1687, 341)
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
		Me.HdtPacketFormatLabel.Location = New System.Drawing.Point(1365, 409)
		Me.HdtPacketFormatLabel.Margin = New System.Windows.Forms.Padding(56, 0, 56, 0)
		Me.HdtPacketFormatLabel.Name = "HdtPacketFormatLabel"
		Me.HdtPacketFormatLabel.Size = New System.Drawing.Size(261, 32)
		Me.HdtPacketFormatLabel.TabIndex = 41
		Me.HdtPacketFormatLabel.Text = "HDT Packet Format"
		'
		'HdtPacketFormatComboBox
		'
		Me.HdtPacketFormatComboBox.Location = New System.Drawing.Point(1687, 403)
		Me.HdtPacketFormatComboBox.Margin = New System.Windows.Forms.Padding(56, 41, 56, 41)
		Me.HdtPacketFormatComboBox.Name = "HdtPacketFormatComboBox"
		Me.HdtPacketFormatComboBox.Size = New System.Drawing.Size(240, 39)
		Me.HdtPacketFormatComboBox.TabIndex = 28
		Me.HdtPacketFormatComboBox.Text = "Format0"
		'
		'HdtPhyIntervalLabel
		'
		Me.HdtPhyIntervalLabel.AutoSize = true
		Me.HdtPhyIntervalLabel.Location = New System.Drawing.Point(1365, 466)
		Me.HdtPhyIntervalLabel.Margin = New System.Windows.Forms.Padding(56, 0, 56, 0)
		Me.HdtPhyIntervalLabel.Name = "HdtPhyIntervalLabel"
		Me.HdtPhyIntervalLabel.Size = New System.Drawing.Size(275, 32)
		Me.HdtPhyIntervalLabel.TabIndex = 45
		Me.HdtPhyIntervalLabel.Text = "HDT PHY Interval (s)"
		'
		'HdtPhyIntervalNumeric
		'
		Me.HdtPhyIntervalNumeric.DecimalPlaces = 10
		Me.HdtPhyIntervalNumeric.Location = New System.Drawing.Point(1687, 466)
		Me.HdtPhyIntervalNumeric.Margin = New System.Windows.Forms.Padding(56, 41, 56, 41)
		Me.HdtPhyIntervalNumeric.Name = "HdtPhyIntervalNumeric"
		Me.HdtPhyIntervalNumeric.Size = New System.Drawing.Size(240, 38)
		Me.HdtPhyIntervalNumeric.TabIndex = 43
		Me.HdtPhyIntervalNumeric.Value = New Decimal(New Integer() {64, 0, 0, 393216})
		'
		'dirtyTxSettingsLabel
		'
		Me.dirtyTxSettingsLabel.AutoSize = true
		Me.dirtyTxSettingsLabel.Location = New System.Drawing.Point(1542, 551)
		Me.dirtyTxSettingsLabel.Margin = New System.Windows.Forms.Padding(21, 0, 21, 0)
		Me.dirtyTxSettingsLabel.Name = "dirtyTxSettingsLabel"
		Me.dirtyTxSettingsLabel.Size = New System.Drawing.Size(221, 32)
		Me.dirtyTxSettingsLabel.TabIndex = 83
		Me.dirtyTxSettingsLabel.Text = "Dirty Tx Settings"
		'
		'dirtyTxModeLabel
		'
		Me.dirtyTxModeLabel.AutoSize = true
		Me.dirtyTxModeLabel.Location = New System.Drawing.Point(1415, 615)
		Me.dirtyTxModeLabel.Margin = New System.Windows.Forms.Padding(56, 0, 56, 0)
		Me.dirtyTxModeLabel.Name = "dirtyTxModeLabel"
		Me.dirtyTxModeLabel.Size = New System.Drawing.Size(188, 32)
		Me.dirtyTxModeLabel.TabIndex = 41
		Me.dirtyTxModeLabel.Text = "Dirty Tx Mode"
		'
		'dirtyTxModeComboBox
		'
		Me.dirtyTxModeComboBox.Location = New System.Drawing.Point(1687, 612)
		Me.dirtyTxModeComboBox.Margin = New System.Windows.Forms.Padding(56, 41, 56, 41)
		Me.dirtyTxModeComboBox.Name = "dirtyTxModeComboBox"
		Me.dirtyTxModeComboBox.Size = New System.Drawing.Size(240, 39)
		Me.dirtyTxModeComboBox.TabIndex = 28
		Me.dirtyTxModeComboBox.Text = "Standard"
		'
		'paraEnabledDataGridViewLabel
		'
		Me.paraEnabledDataGridViewLabel.AutoSize = true
		Me.paraEnabledDataGridViewLabel.Location = New System.Drawing.Point(1502, 680)
		Me.paraEnabledDataGridViewLabel.Margin = New System.Windows.Forms.Padding(21, 0, 21, 0)
		Me.paraEnabledDataGridViewLabel.Name = "paraEnabledDataGridViewLabel"
		Me.paraEnabledDataGridViewLabel.Size = New System.Drawing.Size(339, 32)
		Me.paraEnabledDataGridViewLabel.TabIndex = 105
		Me.paraEnabledDataGridViewLabel.Text = "Parameters Enabled Set[]"
		'
		'paraEnabledDataGridView
		'
		Me.paraEnabledDataGridView.AllowUserToAddRows = false
		Me.paraEnabledDataGridView.ColumnHeadersHeight = 70
		Me.paraEnabledDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.parametersEnabledSetIndex, Me.parametersEnabledSet})
		Me.paraEnabledDataGridView.Location = New System.Drawing.Point(1460, 732)
		Me.paraEnabledDataGridView.Margin = New System.Windows.Forms.Padding(21, 17, 21, 17)
		Me.paraEnabledDataGridView.Name = "paraEnabledDataGridView"
		Me.paraEnabledDataGridView.RowHeadersWidth = 102
		Me.paraEnabledDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.paraEnabledDataGridView.Size = New System.Drawing.Size(435, 250)
		Me.paraEnabledDataGridView.TabIndex = 106
		'
		'paraEnabledDelete
		'
		Me.paraEnabledDelete.Location = New System.Drawing.Point(1701, 1003)
		Me.paraEnabledDelete.Margin = New System.Windows.Forms.Padding(21, 17, 21, 17)
		Me.paraEnabledDelete.Name = "paraEnabledDelete"
		Me.paraEnabledDelete.Size = New System.Drawing.Size(125, 50)
		Me.paraEnabledDelete.TabIndex = 102
		Me.paraEnabledDelete.Text = "Delete"
		Me.paraEnabledDelete.UseVisualStyleBackColor = true
		'
		'paraEnabledInsert
		'
		Me.paraEnabledInsert.Location = New System.Drawing.Point(1530, 1003)
		Me.paraEnabledInsert.Margin = New System.Windows.Forms.Padding(21, 17, 21, 17)
		Me.paraEnabledInsert.Name = "paraEnabledInsert"
		Me.paraEnabledInsert.Size = New System.Drawing.Size(127, 50)
		Me.paraEnabledInsert.TabIndex = 102
		Me.paraEnabledInsert.Text = "Insert"
		Me.paraEnabledInsert.UseVisualStyleBackColor = true
		'
		'carrFreqOffsetDataGridViewLabel
		'
		Me.carrFreqOffsetDataGridViewLabel.Location = New System.Drawing.Point(2123, 23)
		Me.carrFreqOffsetDataGridViewLabel.Margin = New System.Windows.Forms.Padding(21, 0, 21, 0)
		Me.carrFreqOffsetDataGridViewLabel.Name = "carrFreqOffsetDataGridViewLabel"
		Me.carrFreqOffsetDataGridViewLabel.Size = New System.Drawing.Size(339, 32)
		Me.carrFreqOffsetDataGridViewLabel.TabIndex = 107
		Me.carrFreqOffsetDataGridViewLabel.Text = "Carrier Freq Offset Set[]"
		'
		'carrFreqOffsetDataGridView
		'
		Me.carrFreqOffsetDataGridView.AllowUserToAddRows = false
		Me.carrFreqOffsetDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.carrFreqOffsetDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.carrFreqOffsetIndex, Me.carrFreqOffsetSet})
		Me.carrFreqOffsetDataGridView.Location = New System.Drawing.Point(2097, 83)
		Me.carrFreqOffsetDataGridView.Margin = New System.Windows.Forms.Padding(21, 17, 21, 17)
		Me.carrFreqOffsetDataGridView.MaximumSize = New System.Drawing.Size(435, 250)
		Me.carrFreqOffsetDataGridView.Name = "carrFreqOffsetDataGridView"
		Me.carrFreqOffsetDataGridView.RowHeadersWidth = 102
		Me.carrFreqOffsetDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
		Me.carrFreqOffsetDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.carrFreqOffsetDataGridView.Size = New System.Drawing.Size(435, 250)
		Me.carrFreqOffsetDataGridView.TabIndex = 108
		'
		'carrFreqOffsetIndex
		'
		Me.carrFreqOffsetIndex.HeaderText = "Index"
		Me.carrFreqOffsetIndex.MinimumWidth = 12
		Me.carrFreqOffsetIndex.Name = "carrFreqOffsetIndex"
		Me.carrFreqOffsetIndex.Width = 250
		'
		'carrFreqOffsetSet
		'
		Me.carrFreqOffsetSet.HeaderText = "Carrier Freq Offset Set"
		Me.carrFreqOffsetSet.MinimumWidth = 12
		Me.carrFreqOffsetSet.Name = "carrFreqOffsetSet"
		Me.carrFreqOffsetSet.Width = 250
		'
		'carrFreqOffDelete
		'
		Me.carrFreqOffDelete.Location = New System.Drawing.Point(2341, 355)
		Me.carrFreqOffDelete.Margin = New System.Windows.Forms.Padding(21, 17, 21, 17)
		Me.carrFreqOffDelete.Name = "carrFreqOffDelete"
		Me.carrFreqOffDelete.Size = New System.Drawing.Size(125, 50)
		Me.carrFreqOffDelete.TabIndex = 110
		Me.carrFreqOffDelete.Text = "Delete"
		Me.carrFreqOffDelete.UseVisualStyleBackColor = true
		'
		'carrFreqOffInsert
		'
		Me.carrFreqOffInsert.Location = New System.Drawing.Point(2178, 356)
		Me.carrFreqOffInsert.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.carrFreqOffInsert.Name = "carrFreqOffInsert"
		Me.carrFreqOffInsert.Size = New System.Drawing.Size(125, 50)
		Me.carrFreqOffInsert.TabIndex = 109
		Me.carrFreqOffInsert.Text = "Insert"
		Me.carrFreqOffInsert.UseVisualStyleBackColor = true
		'
		'modulationIndexDataGridViewLabel
		'
		Me.modulationIndexDataGridViewLabel.Location = New System.Drawing.Point(2143, 450)
		Me.modulationIndexDataGridViewLabel.Margin = New System.Windows.Forms.Padding(21, 0, 21, 0)
		Me.modulationIndexDataGridViewLabel.Name = "modulationIndexDataGridViewLabel"
		Me.modulationIndexDataGridViewLabel.Size = New System.Drawing.Size(339, 32)
		Me.modulationIndexDataGridViewLabel.TabIndex = 111
		Me.modulationIndexDataGridViewLabel.Text = "Modulation Index Set[]"
		'
		'modulationIndexDataGridView
		'
		Me.modulationIndexDataGridView.AllowUserToAddRows = false
		Me.modulationIndexDataGridView.ColumnHeadersHeight = 70
		Me.modulationIndexDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.modulationIndex, Me.modulationIndexSet})
		Me.modulationIndexDataGridView.Location = New System.Drawing.Point(2097, 500)
		Me.modulationIndexDataGridView.Margin = New System.Windows.Forms.Padding(21, 17, 21, 17)
		Me.modulationIndexDataGridView.MaximumSize = New System.Drawing.Size(435, 250)
		Me.modulationIndexDataGridView.Name = "modulationIndexDataGridView"
		Me.modulationIndexDataGridView.RowHeadersWidth = 102
		Me.modulationIndexDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
		Me.modulationIndexDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.modulationIndexDataGridView.Size = New System.Drawing.Size(435, 250)
		Me.modulationIndexDataGridView.TabIndex = 112
		'
		'modulationIndex
		'
		Me.modulationIndex.HeaderText = "Index"
		Me.modulationIndex.MinimumWidth = 12
		Me.modulationIndex.Name = "modulationIndex"
		Me.modulationIndex.Width = 250
		'
		'modulationIndexSet
		'
		Me.modulationIndexSet.HeaderText = "Modulation Index Set"
		Me.modulationIndexSet.MinimumWidth = 12
		Me.modulationIndexSet.Name = "modulationIndexSet"
		Me.modulationIndexSet.Width = 250
		'
		'modulationIndexDelete
		'
		Me.modulationIndexDelete.Location = New System.Drawing.Point(2341, 776)
		Me.modulationIndexDelete.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.modulationIndexDelete.Name = "modulationIndexDelete"
		Me.modulationIndexDelete.Size = New System.Drawing.Size(125, 50)
		Me.modulationIndexDelete.TabIndex = 114
		Me.modulationIndexDelete.Text = "Delete"
		Me.modulationIndexDelete.UseVisualStyleBackColor = true
		'
		'modulationIndexInsert
		'
		Me.modulationIndexInsert.Location = New System.Drawing.Point(2178, 776)
		Me.modulationIndexInsert.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.modulationIndexInsert.Name = "modulationIndexInsert"
		Me.modulationIndexInsert.Size = New System.Drawing.Size(125, 50)
		Me.modulationIndexInsert.TabIndex = 113
		Me.modulationIndexInsert.Text = "Insert"
		Me.modulationIndexInsert.UseVisualStyleBackColor = true
		'
		'symbolTimingErrorDataGridViewLabel
		'
		Me.symbolTimingErrorDataGridViewLabel.Location = New System.Drawing.Point(2143, 875)
		Me.symbolTimingErrorDataGridViewLabel.Margin = New System.Windows.Forms.Padding(21, 0, 21, 0)
		Me.symbolTimingErrorDataGridViewLabel.Name = "symbolTimingErrorDataGridViewLabel"
		Me.symbolTimingErrorDataGridViewLabel.Size = New System.Drawing.Size(339, 32)
		Me.symbolTimingErrorDataGridViewLabel.TabIndex = 115
		Me.symbolTimingErrorDataGridViewLabel.Text = "Symbol Timing Error Set[]"
		'
		'symbolTimingErrorDataGridView
		'
		Me.symbolTimingErrorDataGridView.AllowUserToAddRows = false
		Me.symbolTimingErrorDataGridView.ColumnHeadersHeight = 70
		Me.symbolTimingErrorDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.symbolTimingErrorIndex, Me.symbolTimingErrorSet})
		Me.symbolTimingErrorDataGridView.Location = New System.Drawing.Point(2097, 935)
		Me.symbolTimingErrorDataGridView.Margin = New System.Windows.Forms.Padding(21, 17, 21, 17)
		Me.symbolTimingErrorDataGridView.MaximumSize = New System.Drawing.Size(435, 250)
		Me.symbolTimingErrorDataGridView.Name = "symbolTimingErrorDataGridView"
		Me.symbolTimingErrorDataGridView.RowHeadersWidth = 102
		Me.symbolTimingErrorDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
		Me.symbolTimingErrorDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
		Me.symbolTimingErrorDataGridView.Size = New System.Drawing.Size(435, 250)
		Me.symbolTimingErrorDataGridView.TabIndex = 116
		'
		'symbolTimingErrorIndex
		'
		Me.symbolTimingErrorIndex.HeaderText = "Index"
		Me.symbolTimingErrorIndex.MinimumWidth = 12
		Me.symbolTimingErrorIndex.Name = "symbolTimingErrorIndex"
		Me.symbolTimingErrorIndex.Width = 250
		'
		'symbolTimingErrorSet
		'
		Me.symbolTimingErrorSet.HeaderText = "Symbol Timing Error Set"
		Me.symbolTimingErrorSet.MinimumWidth = 12
		Me.symbolTimingErrorSet.Name = "symbolTimingErrorSet"
		Me.symbolTimingErrorSet.Width = 200
		'
		'symbolTimingErrorDelete
		'
		Me.symbolTimingErrorDelete.Location = New System.Drawing.Point(2341, 1206)
		Me.symbolTimingErrorDelete.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.symbolTimingErrorDelete.Name = "symbolTimingErrorDelete"
		Me.symbolTimingErrorDelete.Size = New System.Drawing.Size(125, 50)
		Me.symbolTimingErrorDelete.TabIndex = 118
		Me.symbolTimingErrorDelete.Text = "Delete"
		Me.symbolTimingErrorDelete.UseVisualStyleBackColor = true
		'
		'symbolTimingErrorInsert
		'
		Me.symbolTimingErrorInsert.Location = New System.Drawing.Point(2178, 1205)
		Me.symbolTimingErrorInsert.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.symbolTimingErrorInsert.Name = "symbolTimingErrorInsert"
		Me.symbolTimingErrorInsert.Size = New System.Drawing.Size(125, 50)
		Me.symbolTimingErrorInsert.TabIndex = 117
		Me.symbolTimingErrorInsert.Text = "Insert"
		Me.symbolTimingErrorInsert.UseVisualStyleBackColor = true
		'
		'dirtyTxModulationIndexTypeLabel
		'
		Me.dirtyTxModulationIndexTypeLabel.AutoSize = true
		Me.dirtyTxModulationIndexTypeLabel.Location = New System.Drawing.Point(2143, 1309)
		Me.dirtyTxModulationIndexTypeLabel.Margin = New System.Windows.Forms.Padding(21, 0, 21, 0)
		Me.dirtyTxModulationIndexTypeLabel.Name = "dirtyTxModulationIndexTypeLabel"
		Me.dirtyTxModulationIndexTypeLabel.Size = New System.Drawing.Size(404, 32)
		Me.dirtyTxModulationIndexTypeLabel.TabIndex = 41
		Me.dirtyTxModulationIndexTypeLabel.Text = "Dirty Tx Modulation Index Type"
		'
		'dirtyTxModulationIndexTypeComboBox
		'
		Me.dirtyTxModulationIndexTypeComboBox.Location = New System.Drawing.Point(2222, 1364)
		Me.dirtyTxModulationIndexTypeComboBox.Margin = New System.Windows.Forms.Padding(56, 41, 56, 41)
		Me.dirtyTxModulationIndexTypeComboBox.Name = "dirtyTxModulationIndexTypeComboBox"
		Me.dirtyTxModulationIndexTypeComboBox.Size = New System.Drawing.Size(240, 39)
		Me.dirtyTxModulationIndexTypeComboBox.TabIndex = 28
		Me.dirtyTxModulationIndexTypeComboBox.Text = "Standard"
		'
		'Label3
		'
		Me.Label3.AutoSize = true
		Me.Label3.Location = New System.Drawing.Point(1474, 215)
		Me.Label3.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(404, 32)
		Me.Label3.TabIndex = 119
		Me.Label3.Text = "High Data Throughput Settings"
		'
		'parametersEnabledSetIndex
		'
		Me.parametersEnabledSetIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
		Me.parametersEnabledSetIndex.Frozen = true
		Me.parametersEnabledSetIndex.HeaderText = "Index"
		Me.parametersEnabledSetIndex.MinimumWidth = 12
		Me.parametersEnabledSetIndex.Name = "parametersEnabledSetIndex"
		Me.parametersEnabledSetIndex.ReadOnly = true
		Me.parametersEnabledSetIndex.Width = 137
		'
		'parametersEnabledSet
		'
		Me.parametersEnabledSet.Frozen = true
		Me.parametersEnabledSet.HeaderText = "Parameters Enabled Set"
		Me.parametersEnabledSet.MinimumWidth = 12
		Me.parametersEnabledSet.Name = "parametersEnabledSet"
		Me.parametersEnabledSet.Width = 250
		'
		'MainForm
		'
		Me.AcceptButton = Me.generateButton
		Me.AutoScaleDimensions = New System.Drawing.SizeF(16!, 31!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.AutoSize = true
		Me.ClientSize = New System.Drawing.Size(2663, 1729)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.whiteEnLabel)
		Me.Controls.Add(Me.whiteClkLabel)
		Me.Controls.Add(Me.whiteningSettingLabel)
		Me.Controls.Add(Me.whiteClkNumeric)
		Me.Controls.Add(Me.whiteEnComboBox)
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
		Me.Controls.Add(Me.numOfIdleSlotsLabel)
		Me.Controls.Add(Me.numOfUniqPacketsLabel)
		Me.Controls.Add(Me.numOfUniqNumeric)
		Me.Controls.Add(Me.numOfIdleSlotsNumeric)
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
		Me.Controls.Add(Me.errorLabel)
		Me.Controls.Add(Me.extmsgPacket22Label)
		Me.Controls.Add(Me.textmsgPayloadHdrLabel)
		Me.Controls.Add(Me.payloadDataLabel)
		Me.Controls.Add(Me.hardwareLabel)
		Me.Controls.Add(Me.frequencySettingLabel)
		Me.Controls.Add(Me.impairmentsLabel)
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
		Me.Controls.Add(Me.rfsgResourceTextBox)
		Me.Controls.Add(Me.carrierFreqTextBox)
		Me.Controls.Add(Me.actualHeadroomTextBox)
		Me.Controls.Add(Me.payHdrActPaylenTextBox)
		Me.Controls.Add(Me.errorTextBox)
		Me.Controls.Add(Me.generateButton)
		Me.Controls.Add(Me.stopButton)
		Me.Controls.Add(Me.autoHeadroomEnabComboBox)
		Me.Controls.Add(Me.refSourceComboBox)
		Me.Controls.Add(Me.userDefinedBitsComboBox)
		Me.Controls.Add(Me.clkOutTerminalComboBox)
		Me.Controls.Add(Me.allIqImpairEnComboBox)
		Me.Controls.Add(Me.awgnEnabledComboBox)
		Me.Controls.Add(Me.pktHdrArqnComboBox)
		Me.Controls.Add(Me.payHdrPaylenModeComboBox)
		Me.Controls.Add(Me.payHdrDatatypeComboBox)
		Me.Controls.Add(Me.packetComboBox)
		Me.Controls.Add(Me.dataRateLabel)
		Me.Controls.Add(Me.dataRateNumeric)
		Me.Controls.Add(Me.LETPPayloadTypeLabel)
		Me.Controls.Add(Me.LETPPayloadTypeComboBox)
		Me.Controls.Add(Me.LETPCorruptAlternateCRCLabel)
		Me.Controls.Add(Me.LETPCorruptAlternateCRCComboBox)
		Me.Controls.Add(Me.highDataThroughputSettingsLabel)
		Me.Controls.Add(Me.zadoffChuIndexLabel)
		Me.Controls.Add(Me.zadoffChuIndexNumeric)
		Me.Controls.Add(Me.physicalChannelAddressLabel)
		Me.Controls.Add(Me.physicalChannelAddressNumeric)
		Me.Controls.Add(Me.HdtPacketFormatLabel)
		Me.Controls.Add(Me.HdtPacketFormatComboBox)
		Me.Controls.Add(Me.HdtPhyIntervalLabel)
		Me.Controls.Add(Me.HdtPhyIntervalNumeric)
		Me.Controls.Add(Me.dirtyTxSettingsLabel)
		Me.Controls.Add(Me.dirtyTxModeLabel)
		Me.Controls.Add(Me.dirtyTxModeComboBox)
		Me.Controls.Add(Me.paraEnabledDataGridViewLabel)
		Me.Controls.Add(Me.paraEnabledDataGridView)
		Me.Controls.Add(Me.paraEnabledInsert)
		Me.Controls.Add(Me.paraEnabledDelete)
		Me.Controls.Add(Me.carrFreqOffsetDataGridViewLabel)
		Me.Controls.Add(Me.carrFreqOffsetDataGridView)
		Me.Controls.Add(Me.carrFreqOffInsert)
		Me.Controls.Add(Me.carrFreqOffDelete)
		Me.Controls.Add(Me.modulationIndexDataGridViewLabel)
		Me.Controls.Add(Me.modulationIndexDataGridView)
		Me.Controls.Add(Me.modulationIndexInsert)
		Me.Controls.Add(Me.modulationIndexDelete)
		Me.Controls.Add(Me.symbolTimingErrorDataGridViewLabel)
		Me.Controls.Add(Me.symbolTimingErrorDataGridView)
		Me.Controls.Add(Me.symbolTimingErrorInsert)
		Me.Controls.Add(Me.symbolTimingErrorDelete)
		Me.Controls.Add(Me.dirtyTxModulationIndexTypeLabel)
		Me.Controls.Add(Me.dirtyTxModulationIndexTypeComboBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
		Me.MaximizeBox = false
		Me.MaximumSize = New System.Drawing.Size(2695, 1817)
		Me.Name = "MainForm"
		Me.ShowIcon = false
		Me.Text = "Bluetooth Generate Data Packet (Dirty Tx) Example"
		CType(Me.chnNumberNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.powerLevelNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.externalAttnNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.headroomNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.quadratureSkewNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.iDCOffsetNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.qDCOffsetNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.iqGaimbalanceNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.carrierFreqOffNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.cnrNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.bdaddrLapNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.bdaddrUapNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.bdaddrNapNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.pktLtAddrNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.pktHdrFlowNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.pktHdrSeqnNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.payHdrLlidNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.payHdrFlowNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.payHdrPaylenNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.paydatPnorderNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.paydatSeedNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.numOfUniqNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.numOfIdleSlotsNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.iOffsetNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.qOffsetNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.iCommonModeOffsetNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.qCommonModeOffsetNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.whiteClkNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.dataRateNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.zadoffChuIndexNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.physicalChannelAddressNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.HdtPhyIntervalNumeric,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.paraEnabledDataGridView,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.carrFreqOffsetDataGridView,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.modulationIndexDataGridView,System.ComponentModel.ISupportInitialize).EndInit
		CType(Me.symbolTimingErrorDataGridView,System.ComponentModel.ISupportInitialize).EndInit
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
	Private errorLabel As System.Windows.Forms.Label
	Private extmsgPacket22Label As System.Windows.Forms.Label
	Private textmsgPayloadHdrLabel As System.Windows.Forms.Label
	Private payloadDataLabel As System.Windows.Forms.Label
	Private hardwareLabel As System.Windows.Forms.Label
	Private frequencySettingLabel As System.Windows.Forms.Label
	Private impairmentsLabel As System.Windows.Forms.Label
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
	Private rfsgResourceTextBox As System.Windows.Forms.TextBox
	Private carrierFreqTextBox As System.Windows.Forms.TextBox
	Private actualHeadroomTextBox As System.Windows.Forms.TextBox
	Private payHdrActPaylenTextBox As System.Windows.Forms.TextBox
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
	Private timer As System.Windows.Forms.Timer
	Private numOfUniqNumeric As System.Windows.Forms.NumericUpDown
	Private numOfIdleSlotsNumeric As System.Windows.Forms.NumericUpDown
	Private numOfUniqPacketsLabel As System.Windows.Forms.Label
	Private numOfIdleSlotsLabel As System.Windows.Forms.Label
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
	Private whiteEnLabel As System.Windows.Forms.Label
	Private whiteClkLabel As System.Windows.Forms.Label
	Private whiteningSettingLabel As System.Windows.Forms.Label
	Private whiteClkNumeric As System.Windows.Forms.NumericUpDown
	Private whiteEnComboBox As System.Windows.Forms.ComboBox
   Private dataRateLabel As System.Windows.Forms.Label 
   Private dataRateNumeric As System.Windows.Forms.NumericUpDown 
   Private LETPPayloadTypeLabel As System.Windows.Forms.Label 
   Private LETPPayloadTypeComboBox As System.Windows.Forms.ComboBox 
   Private LETPCorruptAlternateCRCLabel As System.Windows.Forms.Label 
   Private LETPCorruptAlternateCRCComboBox As System.Windows.Forms.ComboBox 
   Private highDataThroughputSettingsLabel As System.Windows.Forms.Label 
   Private zadoffChuIndexLabel As System.Windows.Forms.Label 
   Private zadoffChuIndexNumeric As System.Windows.Forms.NumericUpDown 
   Private physicalChannelAddressLabel As System.Windows.Forms.Label 
   Private physicalChannelAddressNumeric As System.Windows.Forms.NumericUpDown
   Private HdtPacketFormatLabel As System.Windows.Forms.Label 
   Private HdtPacketFormatComboBox As System.Windows.Forms.ComboBox 
   Private HdtPhyIntervalLabel As System.Windows.Forms.Label 
   Private HdtPhyIntervalNumeric As System.Windows.Forms.NumericUpDown 
   Private dirtyTxSettingsLabel As System.Windows.Forms.Label
   Private dirtyTxModeLabel As System.Windows.Forms.Label 
   Private dirtyTxModeComboBox As System.Windows.Forms.ComboBox 
   Private paraEnabledDataGridViewLabel As System.Windows.Forms.Label 
   Private paraEnabledDataGridView As System.Windows.Forms.DataGridView 
   Private WithEvents paraEnabledInsert As System.Windows.Forms.Button 
   Private WithEvents paraEnabledDelete As System.Windows.Forms.Button 
   Private carrFreqOffsetDataGridViewLabel As System.Windows.Forms.Label 
   Private carrFreqOffsetDataGridView As System.Windows.Forms.DataGridView 
   Private WithEvents carrFreqOffInsert As System.Windows.Forms.Button 
   Private WithEvents carrFreqOffDelete As System.Windows.Forms.Button 
   Private modulationIndexDataGridViewLabel As System.Windows.Forms.Label 
   Private modulationIndexDataGridView As System.Windows.Forms.DataGridView 
   Private WithEvents modulationIndexInsert As System.Windows.Forms.Button 
   Private WithEvents modulationIndexDelete As System.Windows.Forms.Button 
   Private symbolTimingErrorDataGridViewLabel As System.Windows.Forms.Label 
   Private symbolTimingErrorDataGridView As System.Windows.Forms.DataGridView 
   Private WithEvents symbolTimingErrorInsert As System.Windows.Forms.Button 
   Private WithEvents symbolTimingErrorDelete As System.Windows.Forms.Button 
   Private dirtyTxModulationIndexTypeLabel As System.Windows.Forms.Label 
   Private dirtyTxModulationIndexTypeComboBox As System.Windows.Forms.ComboBox 
   Friend WithEvents Index As System.Windows.Forms.DataGridViewTextBoxColumn
   Private WithEvents Label3 As System.Windows.Forms.Label
   Friend WithEvents carrFreqOffsetIndex As System.Windows.Forms.DataGridViewTextBoxColumn
   Friend WithEvents carrFreqOffsetSet As System.Windows.Forms.DataGridViewTextBoxColumn
   Friend WithEvents modulationIndex As System.Windows.Forms.DataGridViewTextBoxColumn
   Friend WithEvents modulationIndexSet As System.Windows.Forms.DataGridViewTextBoxColumn
   Friend WithEvents symbolTimingErrorIndex As System.Windows.Forms.DataGridViewTextBoxColumn
   Friend WithEvents symbolTimingErrorSet As System.Windows.Forms.DataGridViewTextBoxColumn
   Friend WithEvents parametersEnabledSetIndex As System.Windows.Forms.DataGridViewTextBoxColumn
   Friend WithEvents parametersEnabledSet As System.Windows.Forms.DataGridViewComboBoxColumn
End Class
