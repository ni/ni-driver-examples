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
        Dim DataGridViewCellStyle22 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle23 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle24 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle25 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle26 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle27 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle28 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle29 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle30 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle31 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle32 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle33 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle34 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle35 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle36 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle37 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle38 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle39 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle40 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle41 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle42 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.payloadLengthModeComboBox = New System.Windows.Forms.ComboBox()
        Me.Format1PayloadZoneConfigurationModeComboBox = New System.Windows.Forms.ComboBox()
        Me.Format1PayloadZoneConfigurationModeLabel = New System.Windows.Forms.Label()
        Me.NumberOfPayloadsLabel = New System.Windows.Forms.Label()
        Me.PayloadZoneLengthValue = New System.Windows.Forms.NumericUpDown()
        Me.TxLenSequenceNumberValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.numberOfPayloadNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.ActualPayloadLengthLabel = New System.Windows.Forms.Label()
        Me.ActualPayloadLengthInsertButton = New System.Windows.Forms.Button()
        Me.ActualPayloadLengthDeleteButton = New System.Windows.Forms.Button()
        Me.ActualPayloadLengthGrid = New System.Windows.Forms.DataGridView()
        Me.ActualPayloadLengthIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ActualPayloadLengthValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NumberOfBlocksIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NumberOfBlocksValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PayloadZoneLengthLabel = New System.Windows.Forms.Label()
        Me.TxBlockMapLabel = New System.Windows.Forms.Label()
        Me.TxBlockMapInsertButton = New System.Windows.Forms.Button()
        Me.TxBlockMapDeleteButton = New System.Windows.Forms.Button()
        Me.TxBlockMapGrid = New System.Windows.Forms.DataGridView()
        Me.TxBlockMapIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TxBlockMapValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TxLenSequenceNumberIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastBlockSizeLabel = New System.Windows.Forms.Label()
        Me.LastBlockSizeInsertButton = New System.Windows.Forms.Button()
        Me.LastBlockSizeDeleteButton = New System.Windows.Forms.Button()
        Me.LastBlockSizeGrid = New System.Windows.Forms.DataGridView()
        Me.LastBlockSizeIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LastBlockSizeValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NumebrOfBlocksLabel = New System.Windows.Forms.Label()
        Me.payloadLengthBytesValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.payloadLengthIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BlockSizeLabel = New System.Windows.Forms.Label()
        Me.BlockSizeInsertButton = New System.Windows.Forms.Button()
        Me.BlockSizeDeleteButton = New System.Windows.Forms.Button()
        Me.BlockSizeGrid = New System.Windows.Forms.DataGridView()
        Me.BlockSizeIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BlockSizeValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NumberOfBlocksInsertButton = New System.Windows.Forms.Button()
        Me.payloadLengthLabel = New System.Windows.Forms.Label()
        Me.terminalConfigurationLabel = New System.Windows.Forms.Label()
        Me.terminalConfigurationComboBox = New System.Windows.Forms.ComboBox()
        Me.TxLenSequenceNumberGrid = New System.Windows.Forms.DataGridView()
        Me.TxLenSequenceNumberInsertButton = New System.Windows.Forms.Button()
        Me.TxLenSequenceNumberDeleteButton = New System.Windows.Forms.Button()
        Me.NumberOfBlocksDeleteButton = New System.Windows.Forms.Button()
        Me.NumberOfBlocksGrid = New System.Windows.Forms.DataGridView()
        Me.TxLenSequenceNumberLabel = New System.Windows.Forms.Label()
        Me.payloadLengthInsertButton = New System.Windows.Forms.Button()
        Me.payloadLengthDeleteButton = New System.Windows.Forms.Button()
        Me.PayloadLengthBytesGrid = New System.Windows.Forms.DataGridView()
        Me.autoPayloadZoneProeprtiesSettingsLabel = New System.Windows.Forms.Label()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label5 = New System.Windows.Forms.Label()
        Me.outputPortLabel = New System.Windows.Forms.Label()
        Me.generateButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.highDataThroughputLabel = New System.Windows.Forms.Label()
        Me.OversamplingFactorLabel = New System.Windows.Forms.Label()
        Me.OversamplingFactorNumeric = New System.Windows.Forms.NumericUpDown()
        Me.outputPortComboBox = New System.Windows.Forms.ComboBox()
        Me.payloadLengthModeLabel = New System.Windows.Forms.Label()
        Me.waveNameLabel = New System.Windows.Forms.Label()
        Me.scriptLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.waveNameTextBox = New System.Windows.Forms.TextBox()
        Me.scriptTextBox = New System.Windows.Forms.TextBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.carrierFreqOffNumeric = New System.Windows.Forms.NumericUpDown()
        Me.rfsgResourceLabel = New System.Windows.Forms.Label()
        Me.chnNumberLabel = New System.Windows.Forms.Label()
        Me.carrierFreqLabel = New System.Windows.Forms.Label()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.externalAttnLabel = New System.Windows.Forms.Label()
        Me.autoheadroomEnabLabel = New System.Windows.Forms.Label()
        Me.headroomLabel = New System.Windows.Forms.Label()
        Me.actualHeadroomLabel = New System.Windows.Forms.Label()
        Me.clkOutputTerminalLabel = New System.Windows.Forms.Label()
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
        Me.dataRateLabel = New System.Windows.Forms.Label()
        Me.timer = New System.Windows.Forms.Timer(Me.components)
        Me.Index = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dataRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.zadoffChuIndexLabel = New System.Windows.Forms.Label()
        Me.zadoffChuIndexNumeric = New System.Windows.Forms.NumericUpDown()
        Me.physicalChannelAddressLabel = New System.Windows.Forms.Label()
        Me.physicalChannelAddressNumeric = New System.Windows.Forms.NumericUpDown()
        Me.HdtPhyIntervalLabel = New System.Windows.Forms.Label()
        Me.HdtPhyIntervalNumeric = New System.Windows.Forms.NumericUpDown()
        CType(Me.PayloadZoneLengthValue, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberOfPayloadNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ActualPayloadLengthGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxBlockMapGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LastBlockSizeGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BlockSizeGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TxLenSequenceNumberGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumberOfBlocksGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PayloadLengthBytesGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OversamplingFactorNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.carrierFreqOffNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chnNumberNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.externalAttnNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.headroomNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.quadratureSkewNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iDcOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.qDcOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqGaimbalanceNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cnrNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.zadoffChuIndexNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.physicalChannelAddressNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HdtPhyIntervalNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'payloadLengthModeComboBox
        '
        Me.payloadLengthModeComboBox.Items.AddRange(New Object() {"Maximum Length", "User Defined"})
        Me.payloadLengthModeComboBox.Location = New System.Drawing.Point(1410, 376)
        Me.payloadLengthModeComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.payloadLengthModeComboBox.Name = "payloadLengthModeComboBox"
        Me.payloadLengthModeComboBox.Size = New System.Drawing.Size(313, 39)
        Me.payloadLengthModeComboBox.TabIndex = 280
        '
        'Format1PayloadZoneConfigurationModeComboBox
        '
        Me.Format1PayloadZoneConfigurationModeComboBox.Items.AddRange(New Object() {"Auto", "User Defined"})
        Me.Format1PayloadZoneConfigurationModeComboBox.Location = New System.Drawing.Point(1334, 193)
        Me.Format1PayloadZoneConfigurationModeComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.Format1PayloadZoneConfigurationModeComboBox.Name = "Format1PayloadZoneConfigurationModeComboBox"
        Me.Format1PayloadZoneConfigurationModeComboBox.Size = New System.Drawing.Size(233, 39)
        Me.Format1PayloadZoneConfigurationModeComboBox.TabIndex = 279
        '
        'Format1PayloadZoneConfigurationModeLabel
        '
        Me.Format1PayloadZoneConfigurationModeLabel.AutoSize = True
        Me.Format1PayloadZoneConfigurationModeLabel.Location = New System.Drawing.Point(1325, 154)
        Me.Format1PayloadZoneConfigurationModeLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.Format1PayloadZoneConfigurationModeLabel.Name = "Format1PayloadZoneConfigurationModeLabel"
        Me.Format1PayloadZoneConfigurationModeLabel.Size = New System.Drawing.Size(558, 32)
        Me.Format1PayloadZoneConfigurationModeLabel.TabIndex = 277
        Me.Format1PayloadZoneConfigurationModeLabel.Text = "Format1 Payload Zone Configuration Mode"
        '
        'NumberOfPayloadsLabel
        '
        Me.NumberOfPayloadsLabel.AutoSize = True
        Me.NumberOfPayloadsLabel.Location = New System.Drawing.Point(1328, 53)
        Me.NumberOfPayloadsLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.NumberOfPayloadsLabel.Name = "NumberOfPayloadsLabel"
        Me.NumberOfPayloadsLabel.Size = New System.Drawing.Size(270, 32)
        Me.NumberOfPayloadsLabel.TabIndex = 278
        Me.NumberOfPayloadsLabel.Text = "Number of Payloads"
        '
        'PayloadZoneLengthValue
        '
        Me.PayloadZoneLengthValue.Location = New System.Drawing.Point(2314, 377)
        Me.PayloadZoneLengthValue.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.PayloadZoneLengthValue.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.PayloadZoneLengthValue.Name = "PayloadZoneLengthValue"
        Me.PayloadZoneLengthValue.Size = New System.Drawing.Size(240, 38)
        Me.PayloadZoneLengthValue.TabIndex = 275
        '
        'TxLenSequenceNumberValues
        '
        Me.TxLenSequenceNumberValues.Frozen = True
        Me.TxLenSequenceNumberValues.HeaderText = "Tx Len Sequence Number"
        Me.TxLenSequenceNumberValues.MinimumWidth = 12
        Me.TxLenSequenceNumberValues.Name = "TxLenSequenceNumberValues"
        Me.TxLenSequenceNumberValues.Width = 250
        '
        'numberOfPayloadNumericUpDown
        '
        Me.numberOfPayloadNumericUpDown.Location = New System.Drawing.Point(1331, 91)
        Me.numberOfPayloadNumericUpDown.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.numberOfPayloadNumericUpDown.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numberOfPayloadNumericUpDown.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.numberOfPayloadNumericUpDown.Name = "numberOfPayloadNumericUpDown"
        Me.numberOfPayloadNumericUpDown.Size = New System.Drawing.Size(320, 38)
        Me.numberOfPayloadNumericUpDown.TabIndex = 276
        Me.numberOfPayloadNumericUpDown.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'ActualPayloadLengthLabel
        '
        Me.ActualPayloadLengthLabel.AutoSize = True
        Me.ActualPayloadLengthLabel.Location = New System.Drawing.Point(2249, 436)
        Me.ActualPayloadLengthLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.ActualPayloadLengthLabel.Name = "ActualPayloadLengthLabel"
        Me.ActualPayloadLengthLabel.Size = New System.Drawing.Size(393, 32)
        Me.ActualPayloadLengthLabel.TabIndex = 274
        Me.ActualPayloadLengthLabel.Text = "Actual Payload Length (bytes)"
        '
        'ActualPayloadLengthInsertButton
        '
        Me.ActualPayloadLengthInsertButton.Location = New System.Drawing.Point(2237, 710)
        Me.ActualPayloadLengthInsertButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.ActualPayloadLengthInsertButton.Name = "ActualPayloadLengthInsertButton"
        Me.ActualPayloadLengthInsertButton.Size = New System.Drawing.Size(133, 52)
        Me.ActualPayloadLengthInsertButton.TabIndex = 271
        Me.ActualPayloadLengthInsertButton.Text = "Insert"
        Me.ActualPayloadLengthInsertButton.UseVisualStyleBackColor = True
        '
        'ActualPayloadLengthDeleteButton
        '
        Me.ActualPayloadLengthDeleteButton.Location = New System.Drawing.Point(2386, 710)
        Me.ActualPayloadLengthDeleteButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.ActualPayloadLengthDeleteButton.Name = "ActualPayloadLengthDeleteButton"
        Me.ActualPayloadLengthDeleteButton.Size = New System.Drawing.Size(133, 52)
        Me.ActualPayloadLengthDeleteButton.TabIndex = 272
        Me.ActualPayloadLengthDeleteButton.Text = "Delete"
        Me.ActualPayloadLengthDeleteButton.UseVisualStyleBackColor = True
        '
        'ActualPayloadLengthGrid
        '
        Me.ActualPayloadLengthGrid.AllowUserToAddRows = False
        DataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ActualPayloadLengthGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle22
        Me.ActualPayloadLengthGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ActualPayloadLengthGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ActualPayloadLengthIndex, Me.ActualPayloadLengthValues})
        DataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle23.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle23.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle23.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle23.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle23.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ActualPayloadLengthGrid.DefaultCellStyle = DataGridViewCellStyle23
        Me.ActualPayloadLengthGrid.Location = New System.Drawing.Point(2237, 479)
        Me.ActualPayloadLengthGrid.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.ActualPayloadLengthGrid.Name = "ActualPayloadLengthGrid"
        DataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle24.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle24.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle24.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle24.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ActualPayloadLengthGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle24
        Me.ActualPayloadLengthGrid.RowHeadersWidth = 102
        Me.ActualPayloadLengthGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.ActualPayloadLengthGrid.Size = New System.Drawing.Size(427, 212)
        Me.ActualPayloadLengthGrid.TabIndex = 273
        '
        'ActualPayloadLengthIndex
        '
        Me.ActualPayloadLengthIndex.Frozen = True
        Me.ActualPayloadLengthIndex.HeaderText = "Index"
        Me.ActualPayloadLengthIndex.MinimumWidth = 12
        Me.ActualPayloadLengthIndex.Name = "ActualPayloadLengthIndex"
        Me.ActualPayloadLengthIndex.ReadOnly = True
        Me.ActualPayloadLengthIndex.Width = 40
        '
        'ActualPayloadLengthValues
        '
        Me.ActualPayloadLengthValues.Frozen = True
        Me.ActualPayloadLengthValues.HeaderText = "Actual Payload Length (bytes)"
        Me.ActualPayloadLengthValues.MinimumWidth = 12
        Me.ActualPayloadLengthValues.Name = "ActualPayloadLengthValues"
        Me.ActualPayloadLengthValues.Width = 250
        '
        'NumberOfBlocksIndex
        '
        Me.NumberOfBlocksIndex.Frozen = True
        Me.NumberOfBlocksIndex.HeaderText = "Index"
        Me.NumberOfBlocksIndex.MinimumWidth = 12
        Me.NumberOfBlocksIndex.Name = "NumberOfBlocksIndex"
        Me.NumberOfBlocksIndex.ReadOnly = True
        Me.NumberOfBlocksIndex.Width = 40
        '
        'NumberOfBlocksValues
        '
        Me.NumberOfBlocksValues.Frozen = True
        Me.NumberOfBlocksValues.HeaderText = "Number of Blocks"
        Me.NumberOfBlocksValues.MinimumWidth = 12
        Me.NumberOfBlocksValues.Name = "NumberOfBlocksValues"
        Me.NumberOfBlocksValues.Width = 250
        '
        'PayloadZoneLengthLabel
        '
        Me.PayloadZoneLengthLabel.AutoSize = True
        Me.PayloadZoneLengthLabel.Location = New System.Drawing.Point(2249, 331)
        Me.PayloadZoneLengthLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.PayloadZoneLengthLabel.Name = "PayloadZoneLengthLabel"
        Me.PayloadZoneLengthLabel.Size = New System.Drawing.Size(378, 32)
        Me.PayloadZoneLengthLabel.TabIndex = 270
        Me.PayloadZoneLengthLabel.Text = "Payload Zone Length (bytes)"
        '
        'TxBlockMapLabel
        '
        Me.TxBlockMapLabel.AutoSize = True
        Me.TxBlockMapLabel.Location = New System.Drawing.Point(2391, 920)
        Me.TxBlockMapLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.TxBlockMapLabel.Name = "TxBlockMapLabel"
        Me.TxBlockMapLabel.Size = New System.Drawing.Size(177, 32)
        Me.TxBlockMapLabel.TabIndex = 269
        Me.TxBlockMapLabel.Text = "TxBlock Map"
        '
        'TxBlockMapInsertButton
        '
        Me.TxBlockMapInsertButton.Location = New System.Drawing.Point(2382, 1178)
        Me.TxBlockMapInsertButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TxBlockMapInsertButton.Name = "TxBlockMapInsertButton"
        Me.TxBlockMapInsertButton.Size = New System.Drawing.Size(133, 52)
        Me.TxBlockMapInsertButton.TabIndex = 266
        Me.TxBlockMapInsertButton.Text = "Insert"
        Me.TxBlockMapInsertButton.UseVisualStyleBackColor = True
        '
        'TxBlockMapDeleteButton
        '
        Me.TxBlockMapDeleteButton.Location = New System.Drawing.Point(2531, 1178)
        Me.TxBlockMapDeleteButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TxBlockMapDeleteButton.Name = "TxBlockMapDeleteButton"
        Me.TxBlockMapDeleteButton.Size = New System.Drawing.Size(133, 52)
        Me.TxBlockMapDeleteButton.TabIndex = 267
        Me.TxBlockMapDeleteButton.Text = "Delete"
        Me.TxBlockMapDeleteButton.UseVisualStyleBackColor = True
        '
        'TxBlockMapGrid
        '
        Me.TxBlockMapGrid.AllowUserToAddRows = False
        DataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle25.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle25.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TxBlockMapGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle25
        Me.TxBlockMapGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TxBlockMapGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.TxBlockMapIndex, Me.TxBlockMapValues})
        DataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.TxBlockMapGrid.DefaultCellStyle = DataGridViewCellStyle26
        Me.TxBlockMapGrid.Location = New System.Drawing.Point(2382, 970)
        Me.TxBlockMapGrid.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TxBlockMapGrid.Name = "TxBlockMapGrid"
        DataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle27.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle27.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle27.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TxBlockMapGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle27
        Me.TxBlockMapGrid.RowHeadersWidth = 102
        Me.TxBlockMapGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TxBlockMapGrid.Size = New System.Drawing.Size(427, 194)
        Me.TxBlockMapGrid.TabIndex = 268
        '
        'TxBlockMapIndex
        '
        Me.TxBlockMapIndex.Frozen = True
        Me.TxBlockMapIndex.HeaderText = "Index"
        Me.TxBlockMapIndex.MinimumWidth = 12
        Me.TxBlockMapIndex.Name = "TxBlockMapIndex"
        Me.TxBlockMapIndex.ReadOnly = True
        Me.TxBlockMapIndex.Width = 40
        '
        'TxBlockMapValues
        '
        Me.TxBlockMapValues.Frozen = True
        Me.TxBlockMapValues.HeaderText = "TxBlock Map"
        Me.TxBlockMapValues.MinimumWidth = 12
        Me.TxBlockMapValues.Name = "TxBlockMapValues"
        Me.TxBlockMapValues.Width = 250
        '
        'TxLenSequenceNumberIndex
        '
        Me.TxLenSequenceNumberIndex.Frozen = True
        Me.TxLenSequenceNumberIndex.HeaderText = "Index"
        Me.TxLenSequenceNumberIndex.MinimumWidth = 12
        Me.TxLenSequenceNumberIndex.Name = "TxLenSequenceNumberIndex"
        Me.TxLenSequenceNumberIndex.ReadOnly = True
        Me.TxLenSequenceNumberIndex.Width = 40
        '
        'LastBlockSizeLabel
        '
        Me.LastBlockSizeLabel.AutoSize = True
        Me.LastBlockSizeLabel.Location = New System.Drawing.Point(1904, 1307)
        Me.LastBlockSizeLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.LastBlockSizeLabel.Name = "LastBlockSizeLabel"
        Me.LastBlockSizeLabel.Size = New System.Drawing.Size(301, 32)
        Me.LastBlockSizeLabel.TabIndex = 265
        Me.LastBlockSizeLabel.Text = "Last Block Size (bytes)"
        '
        'LastBlockSizeInsertButton
        '
        Me.LastBlockSizeInsertButton.Location = New System.Drawing.Point(1901, 1579)
        Me.LastBlockSizeInsertButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.LastBlockSizeInsertButton.Name = "LastBlockSizeInsertButton"
        Me.LastBlockSizeInsertButton.Size = New System.Drawing.Size(133, 52)
        Me.LastBlockSizeInsertButton.TabIndex = 262
        Me.LastBlockSizeInsertButton.Text = "Insert"
        Me.LastBlockSizeInsertButton.UseVisualStyleBackColor = True
        '
        'LastBlockSizeDeleteButton
        '
        Me.LastBlockSizeDeleteButton.Location = New System.Drawing.Point(2050, 1579)
        Me.LastBlockSizeDeleteButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.LastBlockSizeDeleteButton.Name = "LastBlockSizeDeleteButton"
        Me.LastBlockSizeDeleteButton.Size = New System.Drawing.Size(133, 52)
        Me.LastBlockSizeDeleteButton.TabIndex = 263
        Me.LastBlockSizeDeleteButton.Text = "Delete"
        Me.LastBlockSizeDeleteButton.UseVisualStyleBackColor = True
        '
        'LastBlockSizeGrid
        '
        Me.LastBlockSizeGrid.AllowUserToAddRows = False
        DataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle28.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle28.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle28.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle28.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle28.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.LastBlockSizeGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle28
        Me.LastBlockSizeGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.LastBlockSizeGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.LastBlockSizeIndex, Me.LastBlockSizeValues})
        DataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle29.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle29.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle29.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle29.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.LastBlockSizeGrid.DefaultCellStyle = DataGridViewCellStyle29
        Me.LastBlockSizeGrid.Location = New System.Drawing.Point(1895, 1357)
        Me.LastBlockSizeGrid.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.LastBlockSizeGrid.Name = "LastBlockSizeGrid"
        DataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle30.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle30.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle30.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle30.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle30.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.LastBlockSizeGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle30
        Me.LastBlockSizeGrid.RowHeadersWidth = 102
        Me.LastBlockSizeGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.LastBlockSizeGrid.Size = New System.Drawing.Size(427, 208)
        Me.LastBlockSizeGrid.TabIndex = 264
        '
        'LastBlockSizeIndex
        '
        Me.LastBlockSizeIndex.Frozen = True
        Me.LastBlockSizeIndex.HeaderText = "Index"
        Me.LastBlockSizeIndex.MinimumWidth = 12
        Me.LastBlockSizeIndex.Name = "LastBlockSizeIndex"
        Me.LastBlockSizeIndex.ReadOnly = True
        Me.LastBlockSizeIndex.Width = 40
        '
        'LastBlockSizeValues
        '
        Me.LastBlockSizeValues.Frozen = True
        Me.LastBlockSizeValues.HeaderText = "Last Block Size (bytes)"
        Me.LastBlockSizeValues.MinimumWidth = 12
        Me.LastBlockSizeValues.Name = "LastBlockSizeValues"
        Me.LastBlockSizeValues.Width = 250
        '
        'NumebrOfBlocksLabel
        '
        Me.NumebrOfBlocksLabel.AutoSize = True
        Me.NumebrOfBlocksLabel.Location = New System.Drawing.Point(1889, 920)
        Me.NumebrOfBlocksLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.NumebrOfBlocksLabel.Name = "NumebrOfBlocksLabel"
        Me.NumebrOfBlocksLabel.Size = New System.Drawing.Size(236, 32)
        Me.NumebrOfBlocksLabel.TabIndex = 257
        Me.NumebrOfBlocksLabel.Text = "Number of Blocks"
        '
        'payloadLengthBytesValues
        '
        Me.payloadLengthBytesValues.Frozen = True
        Me.payloadLengthBytesValues.HeaderText = "Payload Length (bytes)"
        Me.payloadLengthBytesValues.MinimumWidth = 12
        Me.payloadLengthBytesValues.Name = "payloadLengthBytesValues"
        Me.payloadLengthBytesValues.Width = 250
        '
        'payloadLengthIndex
        '
        Me.payloadLengthIndex.Frozen = True
        Me.payloadLengthIndex.HeaderText = "Index"
        Me.payloadLengthIndex.MinimumWidth = 12
        Me.payloadLengthIndex.Name = "payloadLengthIndex"
        Me.payloadLengthIndex.ReadOnly = True
        Me.payloadLengthIndex.Width = 40
        '
        'BlockSizeLabel
        '
        Me.BlockSizeLabel.AutoSize = True
        Me.BlockSizeLabel.Location = New System.Drawing.Point(1387, 1307)
        Me.BlockSizeLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.BlockSizeLabel.Name = "BlockSizeLabel"
        Me.BlockSizeLabel.Size = New System.Drawing.Size(240, 32)
        Me.BlockSizeLabel.TabIndex = 261
        Me.BlockSizeLabel.Text = "Block Size (bytes)"
        '
        'BlockSizeInsertButton
        '
        Me.BlockSizeInsertButton.Location = New System.Drawing.Point(1384, 1579)
        Me.BlockSizeInsertButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.BlockSizeInsertButton.Name = "BlockSizeInsertButton"
        Me.BlockSizeInsertButton.Size = New System.Drawing.Size(133, 52)
        Me.BlockSizeInsertButton.TabIndex = 258
        Me.BlockSizeInsertButton.Text = "Insert"
        Me.BlockSizeInsertButton.UseVisualStyleBackColor = True
        '
        'BlockSizeDeleteButton
        '
        Me.BlockSizeDeleteButton.Location = New System.Drawing.Point(1533, 1579)
        Me.BlockSizeDeleteButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.BlockSizeDeleteButton.Name = "BlockSizeDeleteButton"
        Me.BlockSizeDeleteButton.Size = New System.Drawing.Size(133, 52)
        Me.BlockSizeDeleteButton.TabIndex = 259
        Me.BlockSizeDeleteButton.Text = "Delete"
        Me.BlockSizeDeleteButton.UseVisualStyleBackColor = True
        '
        'BlockSizeGrid
        '
        Me.BlockSizeGrid.AllowUserToAddRows = False
        DataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle31.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle31.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle31.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle31.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle31.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.BlockSizeGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle31
        Me.BlockSizeGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.BlockSizeGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.BlockSizeIndex, Me.BlockSizeValues})
        DataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle32.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle32.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle32.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle32.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle32.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.BlockSizeGrid.DefaultCellStyle = DataGridViewCellStyle32
        Me.BlockSizeGrid.Location = New System.Drawing.Point(1378, 1357)
        Me.BlockSizeGrid.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.BlockSizeGrid.Name = "BlockSizeGrid"
        DataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle33.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle33.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle33.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle33.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle33.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.BlockSizeGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle33
        Me.BlockSizeGrid.RowHeadersWidth = 102
        Me.BlockSizeGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.BlockSizeGrid.Size = New System.Drawing.Size(427, 208)
        Me.BlockSizeGrid.TabIndex = 260
        '
        'BlockSizeIndex
        '
        Me.BlockSizeIndex.Frozen = True
        Me.BlockSizeIndex.HeaderText = "Index"
        Me.BlockSizeIndex.MinimumWidth = 12
        Me.BlockSizeIndex.Name = "BlockSizeIndex"
        Me.BlockSizeIndex.ReadOnly = True
        Me.BlockSizeIndex.Width = 40
        '
        'BlockSizeValues
        '
        Me.BlockSizeValues.Frozen = True
        Me.BlockSizeValues.HeaderText = "Block Size (bytes)"
        Me.BlockSizeValues.MinimumWidth = 12
        Me.BlockSizeValues.Name = "BlockSizeValues"
        Me.BlockSizeValues.Width = 250
        '
        'NumberOfBlocksInsertButton
        '
        Me.NumberOfBlocksInsertButton.Location = New System.Drawing.Point(1880, 1178)
        Me.NumberOfBlocksInsertButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.NumberOfBlocksInsertButton.Name = "NumberOfBlocksInsertButton"
        Me.NumberOfBlocksInsertButton.Size = New System.Drawing.Size(133, 52)
        Me.NumberOfBlocksInsertButton.TabIndex = 254
        Me.NumberOfBlocksInsertButton.Text = "Insert"
        Me.NumberOfBlocksInsertButton.UseVisualStyleBackColor = True
        '
        'payloadLengthLabel
        '
        Me.payloadLengthLabel.AutoSize = True
        Me.payloadLengthLabel.Location = New System.Drawing.Point(1422, 447)
        Me.payloadLengthLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.payloadLengthLabel.Name = "payloadLengthLabel"
        Me.payloadLengthLabel.Size = New System.Drawing.Size(306, 32)
        Me.payloadLengthLabel.TabIndex = 252
        Me.payloadLengthLabel.Text = "Payload Length (bytes)"
        '
        'terminalConfigurationLabel
        '
        Me.terminalConfigurationLabel.AutoSize = True
        Me.terminalConfigurationLabel.Location = New System.Drawing.Point(779, 179)
        Me.terminalConfigurationLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.terminalConfigurationLabel.Name = "terminalConfigurationLabel"
        Me.terminalConfigurationLabel.Size = New System.Drawing.Size(303, 32)
        Me.terminalConfigurationLabel.TabIndex = 242
        Me.terminalConfigurationLabel.Text = "Terminal Configuration"
        '
        'terminalConfigurationComboBox
        '
        Me.terminalConfigurationComboBox.Items.AddRange(New Object() {"Differential", "Single-Ended"})
        Me.terminalConfigurationComboBox.Location = New System.Drawing.Point(784, 212)
        Me.terminalConfigurationComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.terminalConfigurationComboBox.Name = "terminalConfigurationComboBox"
        Me.terminalConfigurationComboBox.Size = New System.Drawing.Size(313, 39)
        Me.terminalConfigurationComboBox.TabIndex = 241
        '
        'TxLenSequenceNumberGrid
        '
        Me.TxLenSequenceNumberGrid.AllowUserToAddRows = False
        DataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle34.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle34.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle34.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle34.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle34.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle34.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TxLenSequenceNumberGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle34
        Me.TxLenSequenceNumberGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TxLenSequenceNumberGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.TxLenSequenceNumberIndex, Me.TxLenSequenceNumberValues})
        DataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle35.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle35.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle35.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle35.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle35.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.TxLenSequenceNumberGrid.DefaultCellStyle = DataGridViewCellStyle35
        Me.TxLenSequenceNumberGrid.Location = New System.Drawing.Point(1378, 970)
        Me.TxLenSequenceNumberGrid.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TxLenSequenceNumberGrid.Name = "TxLenSequenceNumberGrid"
        DataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle36.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle36.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle36.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle36.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle36.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle36.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TxLenSequenceNumberGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle36
        Me.TxLenSequenceNumberGrid.RowHeadersWidth = 102
        Me.TxLenSequenceNumberGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TxLenSequenceNumberGrid.Size = New System.Drawing.Size(412, 226)
        Me.TxLenSequenceNumberGrid.TabIndex = 251
        '
        'TxLenSequenceNumberInsertButton
        '
        Me.TxLenSequenceNumberInsertButton.Location = New System.Drawing.Point(1378, 1206)
        Me.TxLenSequenceNumberInsertButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TxLenSequenceNumberInsertButton.Name = "TxLenSequenceNumberInsertButton"
        Me.TxLenSequenceNumberInsertButton.Size = New System.Drawing.Size(133, 52)
        Me.TxLenSequenceNumberInsertButton.TabIndex = 249
        Me.TxLenSequenceNumberInsertButton.Text = "Insert"
        Me.TxLenSequenceNumberInsertButton.UseVisualStyleBackColor = True
        '
        'TxLenSequenceNumberDeleteButton
        '
        Me.TxLenSequenceNumberDeleteButton.Location = New System.Drawing.Point(1527, 1206)
        Me.TxLenSequenceNumberDeleteButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.TxLenSequenceNumberDeleteButton.Name = "TxLenSequenceNumberDeleteButton"
        Me.TxLenSequenceNumberDeleteButton.Size = New System.Drawing.Size(133, 52)
        Me.TxLenSequenceNumberDeleteButton.TabIndex = 250
        Me.TxLenSequenceNumberDeleteButton.Text = "Delete"
        Me.TxLenSequenceNumberDeleteButton.UseVisualStyleBackColor = True
        '
        'NumberOfBlocksDeleteButton
        '
        Me.NumberOfBlocksDeleteButton.Location = New System.Drawing.Point(2029, 1178)
        Me.NumberOfBlocksDeleteButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.NumberOfBlocksDeleteButton.Name = "NumberOfBlocksDeleteButton"
        Me.NumberOfBlocksDeleteButton.Size = New System.Drawing.Size(133, 52)
        Me.NumberOfBlocksDeleteButton.TabIndex = 255
        Me.NumberOfBlocksDeleteButton.Text = "Delete"
        Me.NumberOfBlocksDeleteButton.UseVisualStyleBackColor = True
        '
        'NumberOfBlocksGrid
        '
        Me.NumberOfBlocksGrid.AllowUserToAddRows = False
        DataGridViewCellStyle37.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle37.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle37.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle37.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle37.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle37.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.NumberOfBlocksGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle37
        Me.NumberOfBlocksGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.NumberOfBlocksGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NumberOfBlocksIndex, Me.NumberOfBlocksValues})
        DataGridViewCellStyle38.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle38.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle38.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle38.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle38.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle38.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle38.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.NumberOfBlocksGrid.DefaultCellStyle = DataGridViewCellStyle38
        Me.NumberOfBlocksGrid.Location = New System.Drawing.Point(1880, 970)
        Me.NumberOfBlocksGrid.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.NumberOfBlocksGrid.Name = "NumberOfBlocksGrid"
        DataGridViewCellStyle39.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle39.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle39.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle39.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle39.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle39.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle39.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.NumberOfBlocksGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle39
        Me.NumberOfBlocksGrid.RowHeadersWidth = 102
        Me.NumberOfBlocksGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.NumberOfBlocksGrid.Size = New System.Drawing.Size(427, 194)
        Me.NumberOfBlocksGrid.TabIndex = 256
        '
        'TxLenSequenceNumberLabel
        '
        Me.TxLenSequenceNumberLabel.AutoSize = True
        Me.TxLenSequenceNumberLabel.Location = New System.Drawing.Point(1387, 920)
        Me.TxLenSequenceNumberLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.TxLenSequenceNumberLabel.Name = "TxLenSequenceNumberLabel"
        Me.TxLenSequenceNumberLabel.Size = New System.Drawing.Size(336, 32)
        Me.TxLenSequenceNumberLabel.TabIndex = 253
        Me.TxLenSequenceNumberLabel.Text = "TxLen Sequence Number"
        '
        'payloadLengthInsertButton
        '
        Me.payloadLengthInsertButton.Location = New System.Drawing.Point(1378, 728)
        Me.payloadLengthInsertButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.payloadLengthInsertButton.Name = "payloadLengthInsertButton"
        Me.payloadLengthInsertButton.Size = New System.Drawing.Size(133, 52)
        Me.payloadLengthInsertButton.TabIndex = 246
        Me.payloadLengthInsertButton.Text = "Insert"
        Me.payloadLengthInsertButton.UseVisualStyleBackColor = True
        '
        'payloadLengthDeleteButton
        '
        Me.payloadLengthDeleteButton.Location = New System.Drawing.Point(1527, 728)
        Me.payloadLengthDeleteButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.payloadLengthDeleteButton.Name = "payloadLengthDeleteButton"
        Me.payloadLengthDeleteButton.Size = New System.Drawing.Size(133, 52)
        Me.payloadLengthDeleteButton.TabIndex = 247
        Me.payloadLengthDeleteButton.Text = "Delete"
        Me.payloadLengthDeleteButton.UseVisualStyleBackColor = True
        '
        'PayloadLengthBytesGrid
        '
        Me.PayloadLengthBytesGrid.AllowUserToAddRows = False
        DataGridViewCellStyle40.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle40.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle40.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle40.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle40.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle40.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle40.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.PayloadLengthBytesGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle40
        Me.PayloadLengthBytesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.PayloadLengthBytesGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.payloadLengthIndex, Me.payloadLengthBytesValues})
        DataGridViewCellStyle41.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle41.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle41.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle41.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle41.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle41.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle41.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.PayloadLengthBytesGrid.DefaultCellStyle = DataGridViewCellStyle41
        Me.PayloadLengthBytesGrid.Location = New System.Drawing.Point(1378, 497)
        Me.PayloadLengthBytesGrid.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.PayloadLengthBytesGrid.Name = "PayloadLengthBytesGrid"
        DataGridViewCellStyle42.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle42.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle42.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.1!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle42.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle42.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle42.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle42.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.PayloadLengthBytesGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle42
        Me.PayloadLengthBytesGrid.RowHeadersWidth = 102
        Me.PayloadLengthBytesGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.PayloadLengthBytesGrid.Size = New System.Drawing.Size(427, 212)
        Me.PayloadLengthBytesGrid.TabIndex = 248
        '
        'autoPayloadZoneProeprtiesSettingsLabel
        '
        Me.autoPayloadZoneProeprtiesSettingsLabel.AutoSize = True
        Me.autoPayloadZoneProeprtiesSettingsLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.autoPayloadZoneProeprtiesSettingsLabel.Location = New System.Drawing.Point(1357, 275)
        Me.autoPayloadZoneProeprtiesSettingsLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.autoPayloadZoneProeprtiesSettingsLabel.Name = "autoPayloadZoneProeprtiesSettingsLabel"
        Me.autoPayloadZoneProeprtiesSettingsLabel.Size = New System.Drawing.Size(541, 32)
        Me.autoPayloadZoneProeprtiesSettingsLabel.TabIndex = 244
        Me.autoPayloadZoneProeprtiesSettingsLabel.Text = "Auto Payload Zone Proeprties Settings"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label3.Location = New System.Drawing.Point(779, 40)
        Me.label3.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(170, 32)
        Me.label3.TabIndex = 243
        Me.label3.Text = "Output Port"
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label5.Location = New System.Drawing.Point(1372, 855)
        Me.label5.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(655, 32)
        Me.label5.TabIndex = 245
        Me.label5.Text = "User Defined Payload Zone Proeprties Settings"
        '
        'outputPortLabel
        '
        Me.outputPortLabel.AutoSize = True
        Me.outputPortLabel.Location = New System.Drawing.Point(779, 83)
        Me.outputPortLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.outputPortLabel.Name = "outputPortLabel"
        Me.outputPortLabel.Size = New System.Drawing.Size(159, 32)
        Me.outputPortLabel.TabIndex = 240
        Me.outputPortLabel.Text = "Output Port"
        '
        'generateButton
        '
        Me.generateButton.Location = New System.Drawing.Point(792, 1285)
        Me.generateButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.generateButton.Name = "generateButton"
        Me.generateButton.Size = New System.Drawing.Size(200, 55)
        Me.generateButton.TabIndex = 237
        Me.generateButton.Text = "&Generate"
        Me.generateButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.Location = New System.Drawing.Point(1032, 1285)
        Me.stopButton.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(200, 55)
        Me.stopButton.TabIndex = 238
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'highDataThroughputLabel
        '
        Me.highDataThroughputLabel.AutoSize = True
        Me.highDataThroughputLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.highDataThroughputLabel.Location = New System.Drawing.Point(2029, 40)
        Me.highDataThroughputLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.highDataThroughputLabel.Name = "highDataThroughputLabel"
        Me.highDataThroughputLabel.Size = New System.Drawing.Size(313, 32)
        Me.highDataThroughputLabel.TabIndex = 223
        Me.highDataThroughputLabel.Text = "High Data Throughput"
        '
        'OversamplingFactorLabel
        '
        Me.OversamplingFactorLabel.AutoSize = True
        Me.OversamplingFactorLabel.Location = New System.Drawing.Point(776, 391)
        Me.OversamplingFactorLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.OversamplingFactorLabel.Name = "OversamplingFactorLabel"
        Me.OversamplingFactorLabel.Size = New System.Drawing.Size(277, 32)
        Me.OversamplingFactorLabel.TabIndex = 230
        Me.OversamplingFactorLabel.Text = "Oversampling Factor"
        '
        'OversamplingFactorNumeric
        '
        Me.OversamplingFactorNumeric.Location = New System.Drawing.Point(784, 429)
        Me.OversamplingFactorNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.OversamplingFactorNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.OversamplingFactorNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.OversamplingFactorNumeric.Name = "OversamplingFactorNumeric"
        Me.OversamplingFactorNumeric.Size = New System.Drawing.Size(320, 38)
        Me.OversamplingFactorNumeric.TabIndex = 229
        Me.OversamplingFactorNumeric.Value = New Decimal(New Integer() {8, 0, 0, 0})
        '
        'outputPortComboBox
        '
        Me.outputPortComboBox.Items.AddRange(New Object() {"RF Out", "IQ Out"})
        Me.outputPortComboBox.Location = New System.Drawing.Point(784, 117)
        Me.outputPortComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.outputPortComboBox.Name = "outputPortComboBox"
        Me.outputPortComboBox.Size = New System.Drawing.Size(313, 39)
        Me.outputPortComboBox.TabIndex = 239
        '
        'payloadLengthModeLabel
        '
        Me.payloadLengthModeLabel.AutoSize = True
        Me.payloadLengthModeLabel.Location = New System.Drawing.Point(1424, 330)
        Me.payloadLengthModeLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.payloadLengthModeLabel.Name = "payloadLengthModeLabel"
        Me.payloadLengthModeLabel.Size = New System.Drawing.Size(291, 32)
        Me.payloadLengthModeLabel.TabIndex = 228
        Me.payloadLengthModeLabel.Text = "Payload Length Mode"
        '
        'waveNameLabel
        '
        Me.waveNameLabel.AutoSize = True
        Me.waveNameLabel.Location = New System.Drawing.Point(779, 308)
        Me.waveNameLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.waveNameLabel.Name = "waveNameLabel"
        Me.waveNameLabel.Size = New System.Drawing.Size(224, 32)
        Me.waveNameLabel.TabIndex = 234
        Me.waveNameLabel.Text = "Waveform Name"
        '
        'scriptLabel
        '
        Me.scriptLabel.AutoSize = True
        Me.scriptLabel.Location = New System.Drawing.Point(784, 629)
        Me.scriptLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.scriptLabel.Name = "scriptLabel"
        Me.scriptLabel.Size = New System.Drawing.Size(87, 32)
        Me.scriptLabel.TabIndex = 231
        Me.scriptLabel.Text = "Script"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(792, 937)
        Me.errorLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(76, 32)
        Me.errorLabel.TabIndex = 235
        Me.errorLabel.Text = "Error"
        '
        'waveNameTextBox
        '
        Me.waveNameTextBox.Location = New System.Drawing.Point(784, 341)
        Me.waveNameTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.waveNameTextBox.Name = "waveNameTextBox"
        Me.waveNameTextBox.Size = New System.Drawing.Size(313, 38)
        Me.waveNameTextBox.TabIndex = 233
        Me.waveNameTextBox.Text = "LEHDT"
        '
        'scriptTextBox
        '
        Me.scriptTextBox.Location = New System.Drawing.Point(784, 670)
        Me.scriptTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.scriptTextBox.Multiline = True
        Me.scriptTextBox.Name = "scriptTextBox"
        Me.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.scriptTextBox.Size = New System.Drawing.Size(471, 221)
        Me.scriptTextBox.TabIndex = 232
        Me.scriptTextBox.TabStop = False
        Me.scriptTextBox.Text = "script GenerateLEPkt" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  repeat forever" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "    generate LEHDT" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  end repeat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "end scr" &
    "ipt"
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(792, 987)
        Me.errorTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(463, 247)
        Me.errorTextBox.TabIndex = 236
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No Error"
        '
        'carrierFreqOffNumeric
        '
        Me.carrierFreqOffNumeric.Location = New System.Drawing.Point(445, 1207)
        Me.carrierFreqOffNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.carrierFreqOffNumeric.Name = "carrierFreqOffNumeric"
        Me.carrierFreqOffNumeric.Size = New System.Drawing.Size(240, 38)
        Me.carrierFreqOffNumeric.TabIndex = 227
        '
        'rfsgResourceLabel
        '
        Me.rfsgResourceLabel.AutoSize = True
        Me.rfsgResourceLabel.Location = New System.Drawing.Point(53, 91)
        Me.rfsgResourceLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.rfsgResourceLabel.Name = "rfsgResourceLabel"
        Me.rfsgResourceLabel.Size = New System.Drawing.Size(220, 32)
        Me.rfsgResourceLabel.TabIndex = 184
        Me.rfsgResourceLabel.Text = "RFSG Resource"
        '
        'chnNumberLabel
        '
        Me.chnNumberLabel.AutoSize = True
        Me.chnNumberLabel.Location = New System.Drawing.Point(53, 193)
        Me.chnNumberLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.chnNumberLabel.Name = "chnNumberLabel"
        Me.chnNumberLabel.Size = New System.Drawing.Size(228, 32)
        Me.chnNumberLabel.TabIndex = 185
        Me.chnNumberLabel.Text = "Channel Number"
        '
        'carrierFreqLabel
        '
        Me.carrierFreqLabel.AutoSize = True
        Me.carrierFreqLabel.Location = New System.Drawing.Point(53, 241)
        Me.carrierFreqLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.carrierFreqLabel.Name = "carrierFreqLabel"
        Me.carrierFreqLabel.Size = New System.Drawing.Size(300, 32)
        Me.carrierFreqLabel.TabIndex = 192
        Me.carrierFreqLabel.Text = "Carrier Frequency (Hz)"
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(53, 331)
        Me.powerLevelLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(253, 32)
        Me.powerLevelLabel.TabIndex = 193
        Me.powerLevelLabel.Text = "Power Level (dBm)"
        '
        'externalAttnLabel
        '
        Me.externalAttnLabel.AutoSize = True
        Me.externalAttnLabel.Location = New System.Drawing.Point(53, 384)
        Me.externalAttnLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.externalAttnLabel.Name = "externalAttnLabel"
        Me.externalAttnLabel.Size = New System.Drawing.Size(332, 32)
        Me.externalAttnLabel.TabIndex = 196
        Me.externalAttnLabel.Text = "External Attenuation (dB)"
        '
        'autoheadroomEnabLabel
        '
        Me.autoheadroomEnabLabel.AutoSize = True
        Me.autoheadroomEnabLabel.Location = New System.Drawing.Point(53, 436)
        Me.autoheadroomEnabLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.autoheadroomEnabLabel.Name = "autoheadroomEnabLabel"
        Me.autoheadroomEnabLabel.Size = New System.Drawing.Size(325, 32)
        Me.autoheadroomEnabLabel.TabIndex = 197
        Me.autoheadroomEnabLabel.Text = "Auto Headroom Enabled"
        '
        'headroomLabel
        '
        Me.headroomLabel.AutoSize = True
        Me.headroomLabel.Location = New System.Drawing.Point(53, 486)
        Me.headroomLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.headroomLabel.Name = "headroomLabel"
        Me.headroomLabel.Size = New System.Drawing.Size(206, 32)
        Me.headroomLabel.TabIndex = 199
        Me.headroomLabel.Text = "Headroom (dB)"
        '
        'actualHeadroomLabel
        '
        Me.actualHeadroomLabel.AutoSize = True
        Me.actualHeadroomLabel.Location = New System.Drawing.Point(53, 532)
        Me.actualHeadroomLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.actualHeadroomLabel.Name = "actualHeadroomLabel"
        Me.actualHeadroomLabel.Size = New System.Drawing.Size(293, 32)
        Me.actualHeadroomLabel.TabIndex = 201
        Me.actualHeadroomLabel.Text = "Actual Headroom (dB)"
        '
        'clkOutputTerminalLabel
        '
        Me.clkOutputTerminalLabel.AutoSize = True
        Me.clkOutputTerminalLabel.Location = New System.Drawing.Point(56, 820)
        Me.clkOutputTerminalLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.clkOutputTerminalLabel.Name = "clkOutputTerminalLabel"
        Me.clkOutputTerminalLabel.Size = New System.Drawing.Size(266, 32)
        Me.clkOutputTerminalLabel.TabIndex = 204
        Me.clkOutputTerminalLabel.Text = "Clk Output Terminal"
        '
        'refSourceLabel
        '
        Me.refSourceLabel.AutoSize = True
        Me.refSourceLabel.Location = New System.Drawing.Point(56, 675)
        Me.refSourceLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.refSourceLabel.Name = "refSourceLabel"
        Me.refSourceLabel.Size = New System.Drawing.Size(242, 32)
        Me.refSourceLabel.TabIndex = 205
        Me.refSourceLabel.Text = "Reference Source"
        '
        'clkTerminalLabel
        '
        Me.clkTerminalLabel.AutoSize = True
        Me.clkTerminalLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clkTerminalLabel.Location = New System.Drawing.Point(53, 768)
        Me.clkTerminalLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.clkTerminalLabel.Name = "clkTerminalLabel"
        Me.clkTerminalLabel.Size = New System.Drawing.Size(306, 32)
        Me.clkTerminalLabel.TabIndex = 224
        Me.clkTerminalLabel.Text = "Export Clock Settings"
        '
        'allIqImpairEnLabel
        '
        Me.allIqImpairEnLabel.AutoSize = True
        Me.allIqImpairEnLabel.Location = New System.Drawing.Point(59, 954)
        Me.allIqImpairEnLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.allIqImpairEnLabel.Name = "allIqImpairEnLabel"
        Me.allIqImpairEnLabel.Size = New System.Drawing.Size(358, 32)
        Me.allIqImpairEnLabel.TabIndex = 207
        Me.allIqImpairEnLabel.Text = "All IQ Impairments Enabled"
        '
        'quadratureSkewLabel
        '
        Me.quadratureSkewLabel.AutoSize = True
        Me.quadratureSkewLabel.Location = New System.Drawing.Point(59, 1006)
        Me.quadratureSkewLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.quadratureSkewLabel.Name = "quadratureSkewLabel"
        Me.quadratureSkewLabel.Size = New System.Drawing.Size(307, 32)
        Me.quadratureSkewLabel.TabIndex = 209
        Me.quadratureSkewLabel.Text = "Quadrature Skew (deg)"
        '
        'iDcOffsetLabel
        '
        Me.iDcOffsetLabel.AutoSize = True
        Me.iDcOffsetLabel.Location = New System.Drawing.Point(59, 1059)
        Me.iDcOffsetLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.iDcOffsetLabel.Name = "iDcOffsetLabel"
        Me.iDcOffsetLabel.Size = New System.Drawing.Size(201, 32)
        Me.iDcOffsetLabel.TabIndex = 212
        Me.iDcOffsetLabel.Text = "I DC Offset (%)"
        '
        'qDcOffsetLabel
        '
        Me.qDcOffsetLabel.AutoSize = True
        Me.qDcOffsetLabel.Location = New System.Drawing.Point(59, 1111)
        Me.qDcOffsetLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.qDcOffsetLabel.Name = "qDcOffsetLabel"
        Me.qDcOffsetLabel.Size = New System.Drawing.Size(216, 32)
        Me.qDcOffsetLabel.TabIndex = 214
        Me.qDcOffsetLabel.Text = "Q DC Offset (%)"
        '
        'iqGaimbalanceLabel
        '
        Me.iqGaimbalanceLabel.AutoSize = True
        Me.iqGaimbalanceLabel.Location = New System.Drawing.Point(59, 1164)
        Me.iqGaimbalanceLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.iqGaimbalanceLabel.Name = "iqGaimbalanceLabel"
        Me.iqGaimbalanceLabel.Size = New System.Drawing.Size(309, 32)
        Me.iqGaimbalanceLabel.TabIndex = 215
        Me.iqGaimbalanceLabel.Text = "IQ Gain Imbalance (dB)"
        '
        'carrierFreqOffLabel
        '
        Me.carrierFreqOffLabel.AutoSize = True
        Me.carrierFreqOffLabel.Location = New System.Drawing.Point(59, 1211)
        Me.carrierFreqOffLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.carrierFreqOffLabel.Name = "carrierFreqOffLabel"
        Me.carrierFreqOffLabel.Size = New System.Drawing.Size(383, 32)
        Me.carrierFreqOffLabel.TabIndex = 217
        Me.carrierFreqOffLabel.Text = "Carrier Frequency Offset (Hz)"
        '
        'awgnEnabledLabel
        '
        Me.awgnEnabledLabel.AutoSize = True
        Me.awgnEnabledLabel.Location = New System.Drawing.Point(59, 1261)
        Me.awgnEnabledLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.awgnEnabledLabel.Name = "awgnEnabledLabel"
        Me.awgnEnabledLabel.Size = New System.Drawing.Size(214, 32)
        Me.awgnEnabledLabel.TabIndex = 218
        Me.awgnEnabledLabel.Text = "AWGN Enabled"
        '
        'cnrLabel
        '
        Me.cnrLabel.AutoSize = True
        Me.cnrLabel.Location = New System.Drawing.Point(59, 1309)
        Me.cnrLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.cnrLabel.Name = "cnrLabel"
        Me.cnrLabel.Size = New System.Drawing.Size(345, 32)
        Me.cnrLabel.TabIndex = 220
        Me.cnrLabel.Text = "Carrier to Noise Ratio (dB)"
        '
        'hardwareLabel
        '
        Me.hardwareLabel.AutoSize = True
        Me.hardwareLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.hardwareLabel.Location = New System.Drawing.Point(53, 40)
        Me.hardwareLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.hardwareLabel.Name = "hardwareLabel"
        Me.hardwareLabel.Size = New System.Drawing.Size(144, 32)
        Me.hardwareLabel.TabIndex = 222
        Me.hardwareLabel.Text = "Hardware"
        '
        'frequencySettingsLabel
        '
        Me.frequencySettingsLabel.AutoSize = True
        Me.frequencySettingsLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frequencySettingsLabel.Location = New System.Drawing.Point(56, 629)
        Me.frequencySettingsLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.frequencySettingsLabel.Name = "frequencySettingsLabel"
        Me.frequencySettingsLabel.Size = New System.Drawing.Size(277, 32)
        Me.frequencySettingsLabel.TabIndex = 225
        Me.frequencySettingsLabel.Text = "Frequency Settings"
        '
        'impairmentsLabel
        '
        Me.impairmentsLabel.AutoSize = True
        Me.impairmentsLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.impairmentsLabel.Location = New System.Drawing.Point(61, 904)
        Me.impairmentsLabel.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.impairmentsLabel.Name = "impairmentsLabel"
        Me.impairmentsLabel.Size = New System.Drawing.Size(180, 32)
        Me.impairmentsLabel.TabIndex = 226
        Me.impairmentsLabel.Text = "Impairments"
        '
        'chnNumberNumeric
        '
        Me.chnNumberNumeric.Location = New System.Drawing.Point(440, 181)
        Me.chnNumberNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.chnNumberNumeric.Maximum = New Decimal(New Integer() {39, 0, 0, 0})
        Me.chnNumberNumeric.Name = "chnNumberNumeric"
        Me.chnNumberNumeric.Size = New System.Drawing.Size(240, 38)
        Me.chnNumberNumeric.TabIndex = 190
        Me.chnNumberNumeric.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'powerLevelNumeric
        '
        Me.powerLevelNumeric.Location = New System.Drawing.Point(440, 324)
        Me.powerLevelNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {25, 0, 0, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {179, 0, 0, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(240, 38)
        Me.powerLevelNumeric.TabIndex = 194
        '
        'externalAttnNumeric
        '
        Me.externalAttnNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.externalAttnNumeric.Location = New System.Drawing.Point(440, 377)
        Me.externalAttnNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.externalAttnNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.externalAttnNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.externalAttnNumeric.Name = "externalAttnNumeric"
        Me.externalAttnNumeric.Size = New System.Drawing.Size(240, 38)
        Me.externalAttnNumeric.TabIndex = 195
        '
        'headroomNumeric
        '
        Me.headroomNumeric.Location = New System.Drawing.Point(440, 479)
        Me.headroomNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.headroomNumeric.Name = "headroomNumeric"
        Me.headroomNumeric.Size = New System.Drawing.Size(240, 38)
        Me.headroomNumeric.TabIndex = 200
        '
        'quadratureSkewNumeric
        '
        Me.quadratureSkewNumeric.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
        Me.quadratureSkewNumeric.Location = New System.Drawing.Point(445, 1004)
        Me.quadratureSkewNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.quadratureSkewNumeric.Name = "quadratureSkewNumeric"
        Me.quadratureSkewNumeric.Size = New System.Drawing.Size(240, 38)
        Me.quadratureSkewNumeric.TabIndex = 210
        '
        'iDcOffsetNumeric
        '
        Me.iDcOffsetNumeric.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
        Me.iDcOffsetNumeric.Location = New System.Drawing.Point(445, 1056)
        Me.iDcOffsetNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.iDcOffsetNumeric.Name = "iDcOffsetNumeric"
        Me.iDcOffsetNumeric.Size = New System.Drawing.Size(240, 38)
        Me.iDcOffsetNumeric.TabIndex = 211
        '
        'qDcOffsetNumeric
        '
        Me.qDcOffsetNumeric.Increment = New Decimal(New Integer() {50, 0, 0, 131072})
        Me.qDcOffsetNumeric.Location = New System.Drawing.Point(445, 1109)
        Me.qDcOffsetNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.qDcOffsetNumeric.Name = "qDcOffsetNumeric"
        Me.qDcOffsetNumeric.Size = New System.Drawing.Size(240, 38)
        Me.qDcOffsetNumeric.TabIndex = 213
        '
        'iqGaimbalanceNumeric
        '
        Me.iqGaimbalanceNumeric.Increment = New Decimal(New Integer() {50, 0, 0, 131072})
        Me.iqGaimbalanceNumeric.Location = New System.Drawing.Point(445, 1161)
        Me.iqGaimbalanceNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.iqGaimbalanceNumeric.Name = "iqGaimbalanceNumeric"
        Me.iqGaimbalanceNumeric.Size = New System.Drawing.Size(240, 38)
        Me.iqGaimbalanceNumeric.TabIndex = 216
        '
        'cnrNumeric
        '
        Me.cnrNumeric.Increment = New Decimal(New Integer() {50, 0, 0, 131072})
        Me.cnrNumeric.Location = New System.Drawing.Point(445, 1304)
        Me.cnrNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.cnrNumeric.Name = "cnrNumeric"
        Me.cnrNumeric.Size = New System.Drawing.Size(240, 38)
        Me.cnrNumeric.TabIndex = 221
        Me.cnrNumeric.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'rfsgResourceTextBox
        '
        Me.rfsgResourceTextBox.Location = New System.Drawing.Point(440, 76)
        Me.rfsgResourceTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.rfsgResourceTextBox.Name = "rfsgResourceTextBox"
        Me.rfsgResourceTextBox.Size = New System.Drawing.Size(233, 38)
        Me.rfsgResourceTextBox.TabIndex = 179
        Me.rfsgResourceTextBox.Text = "RFSG"
        '
        'carrierFreqTextBox
        '
        Me.carrierFreqTextBox.Enabled = False
        Me.carrierFreqTextBox.Location = New System.Drawing.Point(440, 229)
        Me.carrierFreqTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.carrierFreqTextBox.Name = "carrierFreqTextBox"
        Me.carrierFreqTextBox.Size = New System.Drawing.Size(233, 38)
        Me.carrierFreqTextBox.TabIndex = 191
        Me.carrierFreqTextBox.Text = "2.408E+9"
        '
        'actualHeadroomTextBox
        '
        Me.actualHeadroomTextBox.Enabled = False
        Me.actualHeadroomTextBox.Location = New System.Drawing.Point(440, 525)
        Me.actualHeadroomTextBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.actualHeadroomTextBox.Name = "actualHeadroomTextBox"
        Me.actualHeadroomTextBox.Size = New System.Drawing.Size(233, 38)
        Me.actualHeadroomTextBox.TabIndex = 202
        Me.actualHeadroomTextBox.Text = "0.00"
        '
        'autoheadroomEnabComboBox
        '
        Me.autoheadroomEnabComboBox.Location = New System.Drawing.Point(440, 429)
        Me.autoheadroomEnabComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.autoheadroomEnabComboBox.Name = "autoheadroomEnabComboBox"
        Me.autoheadroomEnabComboBox.Size = New System.Drawing.Size(233, 39)
        Me.autoheadroomEnabComboBox.TabIndex = 198
        '
        'refSourceComboBox
        '
        Me.refSourceComboBox.Location = New System.Drawing.Point(432, 670)
        Me.refSourceComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.refSourceComboBox.Name = "refSourceComboBox"
        Me.refSourceComboBox.Size = New System.Drawing.Size(233, 39)
        Me.refSourceComboBox.TabIndex = 203
        '
        'clkOutTerminalComboBox
        '
        Me.clkOutTerminalComboBox.Location = New System.Drawing.Point(432, 813)
        Me.clkOutTerminalComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.clkOutTerminalComboBox.Name = "clkOutTerminalComboBox"
        Me.clkOutTerminalComboBox.Size = New System.Drawing.Size(233, 39)
        Me.clkOutTerminalComboBox.TabIndex = 206
        '
        'allIqImpairEnComboBox
        '
        Me.allIqImpairEnComboBox.Location = New System.Drawing.Point(445, 951)
        Me.allIqImpairEnComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.allIqImpairEnComboBox.Name = "allIqImpairEnComboBox"
        Me.allIqImpairEnComboBox.Size = New System.Drawing.Size(233, 39)
        Me.allIqImpairEnComboBox.TabIndex = 208
        '
        'awgnEnabledComboBox
        '
        Me.awgnEnabledComboBox.Location = New System.Drawing.Point(445, 1259)
        Me.awgnEnabledComboBox.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.awgnEnabledComboBox.Name = "awgnEnabledComboBox"
        Me.awgnEnabledComboBox.Size = New System.Drawing.Size(233, 39)
        Me.awgnEnabledComboBox.TabIndex = 219
        '
        'dataRateLabel
        '
        Me.dataRateLabel.AutoSize = True
        Me.dataRateLabel.Location = New System.Drawing.Point(779, 486)
        Me.dataRateLabel.Margin = New System.Windows.Forms.Padding(21, 0, 21, 0)
        Me.dataRateLabel.Name = "dataRateLabel"
        Me.dataRateLabel.Size = New System.Drawing.Size(212, 32)
        Me.dataRateLabel.TabIndex = 188
        Me.dataRateLabel.Text = "Data Rate (bps)"
        '
        'Index
        '
        Me.Index.MinimumWidth = 12
        Me.Index.Name = "Index"
        Me.Index.Width = 250
        '
        'dataRateNumeric
        '
        Me.dataRateNumeric.Location = New System.Drawing.Point(784, 525)
        Me.dataRateNumeric.Margin = New System.Windows.Forms.Padding(21, 17, 21, 17)
        Me.dataRateNumeric.Maximum = New Decimal(New Integer() {7500000, 0, 0, 0})
        Me.dataRateNumeric.Name = "dataRateNumeric"
        Me.dataRateNumeric.Size = New System.Drawing.Size(320, 38)
        Me.dataRateNumeric.TabIndex = 183
        Me.dataRateNumeric.Value = New Decimal(New Integer() {2000000, 0, 0, 0})
        '
        'zadoffChuIndexLabel
        '
        Me.zadoffChuIndexLabel.AutoSize = True
        Me.zadoffChuIndexLabel.Location = New System.Drawing.Point(2029, 91)
        Me.zadoffChuIndexLabel.Margin = New System.Windows.Forms.Padding(21, 0, 21, 0)
        Me.zadoffChuIndexLabel.Name = "zadoffChuIndexLabel"
        Me.zadoffChuIndexLabel.Size = New System.Drawing.Size(232, 32)
        Me.zadoffChuIndexLabel.TabIndex = 187
        Me.zadoffChuIndexLabel.Text = "Zadoff-Chu Index"
        '
        'zadoffChuIndexNumeric
        '
        Me.zadoffChuIndexNumeric.Location = New System.Drawing.Point(2394, 86)
        Me.zadoffChuIndexNumeric.Margin = New System.Windows.Forms.Padding(21, 17, 21, 17)
        Me.zadoffChuIndexNumeric.Name = "zadoffChuIndexNumeric"
        Me.zadoffChuIndexNumeric.Size = New System.Drawing.Size(240, 38)
        Me.zadoffChuIndexNumeric.TabIndex = 182
        Me.zadoffChuIndexNumeric.Value = New Decimal(New Integer() {7, 0, 0, 0})
        '
        'physicalChannelAddressLabel
        '
        Me.physicalChannelAddressLabel.AutoSize = True
        Me.physicalChannelAddressLabel.Location = New System.Drawing.Point(2029, 138)
        Me.physicalChannelAddressLabel.Margin = New System.Windows.Forms.Padding(21, 0, 21, 0)
        Me.physicalChannelAddressLabel.Name = "physicalChannelAddressLabel"
        Me.physicalChannelAddressLabel.Size = New System.Drawing.Size(346, 32)
        Me.physicalChannelAddressLabel.TabIndex = 186
        Me.physicalChannelAddressLabel.Text = "Physical Channel Address"
        '
        'physicalChannelAddressNumeric
        '
        Me.physicalChannelAddressNumeric.Hexadecimal = True
        Me.physicalChannelAddressNumeric.Location = New System.Drawing.Point(2394, 136)
        Me.physicalChannelAddressNumeric.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.physicalChannelAddressNumeric.Maximum = New Decimal(New Integer() {-727379968, 232, 0, 0})
        Me.physicalChannelAddressNumeric.Name = "physicalChannelAddressNumeric"
        Me.physicalChannelAddressNumeric.Size = New System.Drawing.Size(240, 38)
        Me.physicalChannelAddressNumeric.TabIndex = 181
        Me.physicalChannelAddressNumeric.Value = New Decimal(New Integer() {357913941, 159, 0, 0})
        '
        'HdtPhyIntervalLabel
        '
        Me.HdtPhyIntervalLabel.AutoSize = True
        Me.HdtPhyIntervalLabel.Location = New System.Drawing.Point(2029, 188)
        Me.HdtPhyIntervalLabel.Margin = New System.Windows.Forms.Padding(21, 0, 21, 0)
        Me.HdtPhyIntervalLabel.Name = "HdtPhyIntervalLabel"
        Me.HdtPhyIntervalLabel.Size = New System.Drawing.Size(275, 32)
        Me.HdtPhyIntervalLabel.TabIndex = 189
        Me.HdtPhyIntervalLabel.Text = "HDT PHY Interval (s)"
        '
        'HdtPhyIntervalNumeric
        '
        Me.HdtPhyIntervalNumeric.DecimalPlaces = 10
        Me.HdtPhyIntervalNumeric.Location = New System.Drawing.Point(2394, 188)
        Me.HdtPhyIntervalNumeric.Margin = New System.Windows.Forms.Padding(21, 17, 21, 17)
        Me.HdtPhyIntervalNumeric.Name = "HdtPhyIntervalNumeric"
        Me.HdtPhyIntervalNumeric.Size = New System.Drawing.Size(240, 38)
        Me.HdtPhyIntervalNumeric.TabIndex = 180
        Me.HdtPhyIntervalNumeric.Value = New Decimal(New Integer() {64, 0, 0, 393216})
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(16.0!, 31.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(2847, 1978)
        Me.Controls.Add(Me.payloadLengthModeComboBox)
        Me.Controls.Add(Me.Format1PayloadZoneConfigurationModeComboBox)
        Me.Controls.Add(Me.Format1PayloadZoneConfigurationModeLabel)
        Me.Controls.Add(Me.NumberOfPayloadsLabel)
        Me.Controls.Add(Me.PayloadZoneLengthValue)
        Me.Controls.Add(Me.numberOfPayloadNumericUpDown)
        Me.Controls.Add(Me.ActualPayloadLengthLabel)
        Me.Controls.Add(Me.ActualPayloadLengthInsertButton)
        Me.Controls.Add(Me.ActualPayloadLengthDeleteButton)
        Me.Controls.Add(Me.ActualPayloadLengthGrid)
        Me.Controls.Add(Me.PayloadZoneLengthLabel)
        Me.Controls.Add(Me.TxBlockMapLabel)
        Me.Controls.Add(Me.TxBlockMapInsertButton)
        Me.Controls.Add(Me.TxBlockMapDeleteButton)
        Me.Controls.Add(Me.TxBlockMapGrid)
        Me.Controls.Add(Me.LastBlockSizeLabel)
        Me.Controls.Add(Me.LastBlockSizeInsertButton)
        Me.Controls.Add(Me.LastBlockSizeDeleteButton)
        Me.Controls.Add(Me.LastBlockSizeGrid)
        Me.Controls.Add(Me.NumebrOfBlocksLabel)
        Me.Controls.Add(Me.BlockSizeLabel)
        Me.Controls.Add(Me.BlockSizeInsertButton)
        Me.Controls.Add(Me.BlockSizeDeleteButton)
        Me.Controls.Add(Me.BlockSizeGrid)
        Me.Controls.Add(Me.NumberOfBlocksInsertButton)
        Me.Controls.Add(Me.payloadLengthLabel)
        Me.Controls.Add(Me.terminalConfigurationLabel)
        Me.Controls.Add(Me.terminalConfigurationComboBox)
        Me.Controls.Add(Me.TxLenSequenceNumberGrid)
        Me.Controls.Add(Me.TxLenSequenceNumberInsertButton)
        Me.Controls.Add(Me.TxLenSequenceNumberDeleteButton)
        Me.Controls.Add(Me.NumberOfBlocksDeleteButton)
        Me.Controls.Add(Me.NumberOfBlocksGrid)
        Me.Controls.Add(Me.TxLenSequenceNumberLabel)
        Me.Controls.Add(Me.payloadLengthInsertButton)
        Me.Controls.Add(Me.payloadLengthDeleteButton)
        Me.Controls.Add(Me.PayloadLengthBytesGrid)
        Me.Controls.Add(Me.autoPayloadZoneProeprtiesSettingsLabel)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label5)
        Me.Controls.Add(Me.outputPortLabel)
        Me.Controls.Add(Me.generateButton)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.highDataThroughputLabel)
        Me.Controls.Add(Me.OversamplingFactorLabel)
        Me.Controls.Add(Me.OversamplingFactorNumeric)
        Me.Controls.Add(Me.outputPortComboBox)
        Me.Controls.Add(Me.payloadLengthModeLabel)
        Me.Controls.Add(Me.waveNameLabel)
        Me.Controls.Add(Me.scriptLabel)
        Me.Controls.Add(Me.errorLabel)
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
        Me.Controls.Add(Me.clkOutputTerminalLabel)
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
        Me.Controls.Add(Me.hardwareLabel)
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
        Me.Controls.Add(Me.HdtPhyIntervalLabel)
        Me.Controls.Add(Me.HdtPhyIntervalNumeric)
        Me.Margin = New System.Windows.Forms.Padding(8, 7, 8, 7)
        Me.Name = "MainForm"
        Me.Text = "LE-HDT Example"
        CType(Me.PayloadZoneLengthValue, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberOfPayloadNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ActualPayloadLengthGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxBlockMapGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LastBlockSizeGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BlockSizeGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TxLenSequenceNumberGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumberOfBlocksGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PayloadLengthBytesGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OversamplingFactorNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.carrierFreqOffNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chnNumberNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.externalAttnNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.headroomNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.quadratureSkewNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iDcOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.qDcOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqGaimbalanceNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cnrNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.zadoffChuIndexNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.physicalChannelAddressNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HdtPhyIntervalNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents payloadLengthModeComboBox As System.Windows.Forms.ComboBox
	Private WithEvents Format1PayloadZoneConfigurationModeComboBox As System.Windows.Forms.ComboBox
	Private WithEvents Format1PayloadZoneConfigurationModeLabel As System.Windows.Forms.Label
	Private WithEvents NumberOfPayloadsLabel As System.Windows.Forms.Label
	Private WithEvents PayloadZoneLengthValue As System.Windows.Forms.NumericUpDown
	Private WithEvents TxLenSequenceNumberValues As System.Windows.Forms.DataGridViewTextBoxColumn
	Private WithEvents numberOfPayloadNumericUpDown As System.Windows.Forms.NumericUpDown
	Private WithEvents ActualPayloadLengthLabel As System.Windows.Forms.Label
	Private WithEvents ActualPayloadLengthInsertButton As System.Windows.Forms.Button
	Private WithEvents ActualPayloadLengthDeleteButton As System.Windows.Forms.Button
	Private WithEvents ActualPayloadLengthGrid As System.Windows.Forms.DataGridView
	Private WithEvents ActualPayloadLengthIndex As System.Windows.Forms.DataGridViewTextBoxColumn
	Private WithEvents ActualPayloadLengthValues As System.Windows.Forms.DataGridViewTextBoxColumn
	Private WithEvents NumberOfBlocksIndex As System.Windows.Forms.DataGridViewTextBoxColumn
	Private WithEvents NumberOfBlocksValues As System.Windows.Forms.DataGridViewTextBoxColumn
	Private WithEvents PayloadZoneLengthLabel As System.Windows.Forms.Label
	Private WithEvents TxBlockMapLabel As System.Windows.Forms.Label
	Private WithEvents TxBlockMapInsertButton As System.Windows.Forms.Button
	Private WithEvents TxBlockMapDeleteButton As System.Windows.Forms.Button
	Private WithEvents TxBlockMapGrid As System.Windows.Forms.DataGridView
	Private WithEvents TxBlockMapIndex As System.Windows.Forms.DataGridViewTextBoxColumn
	Private WithEvents TxBlockMapValues As System.Windows.Forms.DataGridViewTextBoxColumn
	Private WithEvents TxLenSequenceNumberIndex As System.Windows.Forms.DataGridViewTextBoxColumn
	Private WithEvents LastBlockSizeLabel As System.Windows.Forms.Label
	Private WithEvents LastBlockSizeInsertButton As System.Windows.Forms.Button
	Private WithEvents LastBlockSizeDeleteButton As System.Windows.Forms.Button
	Private WithEvents LastBlockSizeGrid As System.Windows.Forms.DataGridView
	Private WithEvents LastBlockSizeIndex As System.Windows.Forms.DataGridViewTextBoxColumn
	Private WithEvents LastBlockSizeValues As System.Windows.Forms.DataGridViewTextBoxColumn
	Private WithEvents NumebrOfBlocksLabel As System.Windows.Forms.Label
	Private WithEvents payloadLengthBytesValues As System.Windows.Forms.DataGridViewTextBoxColumn
	Private WithEvents payloadLengthIndex As System.Windows.Forms.DataGridViewTextBoxColumn
	Private WithEvents BlockSizeLabel As System.Windows.Forms.Label
	Private WithEvents BlockSizeInsertButton As System.Windows.Forms.Button
	Private WithEvents BlockSizeDeleteButton As System.Windows.Forms.Button
	Private WithEvents BlockSizeGrid As System.Windows.Forms.DataGridView
	Private WithEvents BlockSizeIndex As System.Windows.Forms.DataGridViewTextBoxColumn
	Private WithEvents BlockSizeValues As System.Windows.Forms.DataGridViewTextBoxColumn
	Private WithEvents NumberOfBlocksInsertButton As System.Windows.Forms.Button
	Private WithEvents payloadLengthLabel As System.Windows.Forms.Label
	Private WithEvents terminalConfigurationLabel As System.Windows.Forms.Label
	Private WithEvents terminalConfigurationComboBox As System.Windows.Forms.ComboBox
	Private WithEvents TxLenSequenceNumberGrid As System.Windows.Forms.DataGridView
	Private WithEvents TxLenSequenceNumberInsertButton As System.Windows.Forms.Button
	Private WithEvents TxLenSequenceNumberDeleteButton As System.Windows.Forms.Button
	Private WithEvents NumberOfBlocksDeleteButton As System.Windows.Forms.Button
	Private WithEvents NumberOfBlocksGrid As System.Windows.Forms.DataGridView
	Private WithEvents TxLenSequenceNumberLabel As System.Windows.Forms.Label
	Private WithEvents payloadLengthInsertButton As System.Windows.Forms.Button
	Private WithEvents payloadLengthDeleteButton As System.Windows.Forms.Button
	Private WithEvents PayloadLengthBytesGrid As System.Windows.Forms.DataGridView
	Private WithEvents autoPayloadZoneProeprtiesSettingsLabel As System.Windows.Forms.Label
	Private WithEvents label3 As System.Windows.Forms.Label
	Private WithEvents label5 As System.Windows.Forms.Label
	Private WithEvents outputPortLabel As System.Windows.Forms.Label
	Private WithEvents generateButton As System.Windows.Forms.Button
	Private WithEvents stopButton As System.Windows.Forms.Button
	Private WithEvents highDataThroughputLabel As System.Windows.Forms.Label
	Private WithEvents OversamplingFactorLabel As System.Windows.Forms.Label
	Private WithEvents OversamplingFactorNumeric As System.Windows.Forms.NumericUpDown
	Private WithEvents outputPortComboBox As System.Windows.Forms.ComboBox
	Private WithEvents payloadLengthModeLabel As System.Windows.Forms.Label
	Private WithEvents waveNameLabel As System.Windows.Forms.Label
	Private WithEvents scriptLabel As System.Windows.Forms.Label
	Private WithEvents errorLabel As System.Windows.Forms.Label
	Private WithEvents waveNameTextBox As System.Windows.Forms.TextBox
	Private WithEvents scriptTextBox As System.Windows.Forms.TextBox
	Private WithEvents errorTextBox As System.Windows.Forms.TextBox
	Private WithEvents carrierFreqOffNumeric As System.Windows.Forms.NumericUpDown
	Private WithEvents rfsgResourceLabel As System.Windows.Forms.Label
	Private WithEvents chnNumberLabel As System.Windows.Forms.Label
	Private WithEvents carrierFreqLabel As System.Windows.Forms.Label
	Private WithEvents powerLevelLabel As System.Windows.Forms.Label
	Private WithEvents externalAttnLabel As System.Windows.Forms.Label
	Private WithEvents autoheadroomEnabLabel As System.Windows.Forms.Label
	Private WithEvents headroomLabel As System.Windows.Forms.Label
	Private WithEvents actualHeadroomLabel As System.Windows.Forms.Label
	Private WithEvents clkOutputTerminalLabel As System.Windows.Forms.Label
	Private WithEvents refSourceLabel As System.Windows.Forms.Label
	Private WithEvents clkTerminalLabel As System.Windows.Forms.Label
	Private WithEvents allIqImpairEnLabel As System.Windows.Forms.Label
	Private WithEvents quadratureSkewLabel As System.Windows.Forms.Label
	Private WithEvents iDcOffsetLabel As System.Windows.Forms.Label
	Private WithEvents qDcOffsetLabel As System.Windows.Forms.Label
	Private WithEvents iqGaimbalanceLabel As System.Windows.Forms.Label
	Private WithEvents carrierFreqOffLabel As System.Windows.Forms.Label
	Private WithEvents awgnEnabledLabel As System.Windows.Forms.Label
	Private WithEvents cnrLabel As System.Windows.Forms.Label
	Private WithEvents hardwareLabel As System.Windows.Forms.Label
	Private WithEvents frequencySettingsLabel As System.Windows.Forms.Label
	Private WithEvents impairmentsLabel As System.Windows.Forms.Label
	Private WithEvents chnNumberNumeric As System.Windows.Forms.NumericUpDown
	Private WithEvents powerLevelNumeric As System.Windows.Forms.NumericUpDown
	Private WithEvents externalAttnNumeric As System.Windows.Forms.NumericUpDown
	Private WithEvents headroomNumeric As System.Windows.Forms.NumericUpDown
	Private WithEvents quadratureSkewNumeric As System.Windows.Forms.NumericUpDown
	Private WithEvents iDcOffsetNumeric As System.Windows.Forms.NumericUpDown
	Private WithEvents qDcOffsetNumeric As System.Windows.Forms.NumericUpDown
	Private WithEvents iqGaimbalanceNumeric As System.Windows.Forms.NumericUpDown
	Private WithEvents cnrNumeric As System.Windows.Forms.NumericUpDown
	Private WithEvents rfsgResourceTextBox As System.Windows.Forms.TextBox
	Private WithEvents carrierFreqTextBox As System.Windows.Forms.TextBox
	Private WithEvents actualHeadroomTextBox As System.Windows.Forms.TextBox
	Private WithEvents autoheadroomEnabComboBox As System.Windows.Forms.ComboBox
	Private WithEvents refSourceComboBox As System.Windows.Forms.ComboBox
	Private WithEvents clkOutTerminalComboBox As System.Windows.Forms.ComboBox
	Private WithEvents allIqImpairEnComboBox As System.Windows.Forms.ComboBox
	Private WithEvents awgnEnabledComboBox As System.Windows.Forms.ComboBox
	Private WithEvents dataRateLabel As System.Windows.Forms.Label
	Private WithEvents timer As System.Windows.Forms.Timer
	Private WithEvents Index As System.Windows.Forms.DataGridViewTextBoxColumn
	Private WithEvents dataRateNumeric As System.Windows.Forms.NumericUpDown
	Private WithEvents zadoffChuIndexLabel As System.Windows.Forms.Label
	Private WithEvents zadoffChuIndexNumeric As System.Windows.Forms.NumericUpDown
	Private WithEvents physicalChannelAddressLabel As System.Windows.Forms.Label
	Private WithEvents physicalChannelAddressNumeric As System.Windows.Forms.NumericUpDown
	Private WithEvents HdtPhyIntervalLabel As System.Windows.Forms.Label
	Private WithEvents HdtPhyIntervalNumeric As System.Windows.Forms.NumericUpDown

#End Region
End Class

