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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.pulseVoltageSequenceLabel = New System.Windows.Forms.Label()
        Me.sourceDelaysLabel = New System.Windows.Forms.Label()
        Me.pulseVoltageLevelRangeLabel = New System.Windows.Forms.Label()
        Me.biasVoltageLevelLabel = New System.Windows.Forms.Label()
        Me.apertureTimeLabel = New System.Windows.Forms.Label()
        Me.pulseCurrentLimitRangeLabel = New System.Windows.Forms.Label()
        Me.pulseCurrentLimitLabel = New System.Windows.Forms.Label()
        Me.biasCurrentLimitLabel = New System.Windows.Forms.Label()
        Me.pulseOnTimeLabel = New System.Windows.Forms.Label()
        Me.pulseOffTimeLabel = New System.Windows.Forms.Label()
        Me.pulseBiasDelayLabel = New System.Windows.Forms.Label()
        Me.voltageMeasurmentsLabel = New System.Windows.Forms.Label()
        Me.currentMeasurementsLabel = New System.Windows.Forms.Label()
        Me.pulseVoltLevelRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.biasVoltageLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.apertureTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseCurLimitRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseCurrentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.biasCurrentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseOnTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseOffTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseBiasDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.pulsevoltageseq1Column = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.sourcedelays1Column = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.voltagemeasurments1Column = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.currentmeasurements1Column = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pulseVoltageSequenceDataGridView = New System.Windows.Forms.DataGridView()
        Me.sourceDelaysDataGridView = New System.Windows.Forms.DataGridView()
        Me.voltageMeasurmentsDataGridView = New System.Windows.Forms.DataGridView()
        Me.currentMeasurementsDataGridView = New System.Windows.Forms.DataGridView()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceAndChannelNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.inputSequenceGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.pulseVoltLevelRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.biasVoltageLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.apertureTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseCurLimitRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.biasCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseOnTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseOffTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseBiasDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseVoltageSequenceDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sourceDelaysDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageMeasurmentsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.currentMeasurementsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resourceAndChannelNameGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        Me.inputSequenceGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 24)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.Location = New System.Drawing.Point(6, 51)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.channelNameLabel.TabIndex = 1
        Me.channelNameLabel.Text = "Channel Name"
        '
        'pulseVoltageSequenceLabel
        '
        Me.pulseVoltageSequenceLabel.AutoSize = True
        Me.pulseVoltageSequenceLabel.Location = New System.Drawing.Point(6, 22)
        Me.pulseVoltageSequenceLabel.Name = "pulseVoltageSequenceLabel"
        Me.pulseVoltageSequenceLabel.Size = New System.Drawing.Size(124, 13)
        Me.pulseVoltageSequenceLabel.TabIndex = 2
        Me.pulseVoltageSequenceLabel.Text = "Pulse Voltage Sequence"
        '
        'sourceDelaysLabel
        '
        Me.sourceDelaysLabel.AutoSize = True
        Me.sourceDelaysLabel.Location = New System.Drawing.Point(7, 127)
        Me.sourceDelaysLabel.Name = "sourceDelaysLabel"
        Me.sourceDelaysLabel.Size = New System.Drawing.Size(76, 13)
        Me.sourceDelaysLabel.TabIndex = 3
        Me.sourceDelaysLabel.Text = "Source Delays"
        '
        'pulseVoltageLevelRangeLabel
        '
        Me.pulseVoltageLevelRangeLabel.AutoSize = True
        Me.pulseVoltageLevelRangeLabel.Location = New System.Drawing.Point(6, 28)
        Me.pulseVoltageLevelRangeLabel.Name = "pulseVoltageLevelRangeLabel"
        Me.pulseVoltageLevelRangeLabel.Size = New System.Drawing.Size(168, 13)
        Me.pulseVoltageLevelRangeLabel.TabIndex = 4
        Me.pulseVoltageLevelRangeLabel.Text = "Pulse Voltage Level Range (Volts)"
        '
        'biasVoltageLevelLabel
        '
        Me.biasVoltageLevelLabel.AutoSize = True
        Me.biasVoltageLevelLabel.Location = New System.Drawing.Point(6, 51)
        Me.biasVoltageLevelLabel.Name = "biasVoltageLevelLabel"
        Me.biasVoltageLevelLabel.Size = New System.Drawing.Size(156, 13)
        Me.biasVoltageLevelLabel.TabIndex = 5
        Me.biasVoltageLevelLabel.Text = "Pulse Bias Voltage Level (Volts)"
        '
        'apertureTimeLabel
        '
        Me.apertureTimeLabel.AutoSize = True
        Me.apertureTimeLabel.Location = New System.Drawing.Point(6, 75)
        Me.apertureTimeLabel.Name = "apertureTimeLabel"
        Me.apertureTimeLabel.Size = New System.Drawing.Size(101, 13)
        Me.apertureTimeLabel.TabIndex = 6
        Me.apertureTimeLabel.Text = "Aperture Time (Sec)"
        '
        'pulseCurrentLimitRangeLabel
        '
        Me.pulseCurrentLimitRangeLabel.AutoSize = True
        Me.pulseCurrentLimitRangeLabel.Location = New System.Drawing.Point(6, 100)
        Me.pulseCurrentLimitRangeLabel.Name = "pulseCurrentLimitRangeLabel"
        Me.pulseCurrentLimitRangeLabel.Size = New System.Drawing.Size(164, 13)
        Me.pulseCurrentLimitRangeLabel.TabIndex = 7
        Me.pulseCurrentLimitRangeLabel.Text = "Pulse Current Limit Range (Amps)"
        '
        'pulseCurrentLimitLabel
        '
        Me.pulseCurrentLimitLabel.AutoSize = True
        Me.pulseCurrentLimitLabel.Location = New System.Drawing.Point(6, 122)
        Me.pulseCurrentLimitLabel.Name = "pulseCurrentLimitLabel"
        Me.pulseCurrentLimitLabel.Size = New System.Drawing.Size(129, 13)
        Me.pulseCurrentLimitLabel.TabIndex = 8
        Me.pulseCurrentLimitLabel.Text = "Pulse Current Limit (Amps)"
        '
        'biasCurrentLimitLabel
        '
        Me.biasCurrentLimitLabel.AutoSize = True
        Me.biasCurrentLimitLabel.Location = New System.Drawing.Point(6, 144)
        Me.biasCurrentLimitLabel.Name = "biasCurrentLimitLabel"
        Me.biasCurrentLimitLabel.Size = New System.Drawing.Size(152, 13)
        Me.biasCurrentLimitLabel.TabIndex = 9
        Me.biasCurrentLimitLabel.Text = "Pulse Bias Current Limit (Amps)"
        '
        'pulseOnTimeLabel
        '
        Me.pulseOnTimeLabel.AutoSize = True
        Me.pulseOnTimeLabel.Location = New System.Drawing.Point(6, 167)
        Me.pulseOnTimeLabel.Name = "pulseOnTimeLabel"
        Me.pulseOnTimeLabel.Size = New System.Drawing.Size(104, 13)
        Me.pulseOnTimeLabel.TabIndex = 10
        Me.pulseOnTimeLabel.Text = "Pulse On Time (Sec)"
        '
        'pulseOffTimeLabel
        '
        Me.pulseOffTimeLabel.AutoSize = True
        Me.pulseOffTimeLabel.Location = New System.Drawing.Point(6, 190)
        Me.pulseOffTimeLabel.Name = "pulseOffTimeLabel"
        Me.pulseOffTimeLabel.Size = New System.Drawing.Size(104, 13)
        Me.pulseOffTimeLabel.TabIndex = 11
        Me.pulseOffTimeLabel.Text = "Pulse Off Time (Sec)"
        '
        'pulseBiasDelayLabel
        '
        Me.pulseBiasDelayLabel.AutoSize = True
        Me.pulseBiasDelayLabel.Location = New System.Drawing.Point(6, 213)
        Me.pulseBiasDelayLabel.Name = "pulseBiasDelayLabel"
        Me.pulseBiasDelayLabel.Size = New System.Drawing.Size(114, 13)
        Me.pulseBiasDelayLabel.TabIndex = 12
        Me.pulseBiasDelayLabel.Text = "Pulse Bias Delay (Sec)"
        '
        'voltageMeasurmentsLabel
        '
        Me.voltageMeasurmentsLabel.AutoSize = True
        Me.voltageMeasurmentsLabel.Location = New System.Drawing.Point(6, 24)
        Me.voltageMeasurmentsLabel.Name = "voltageMeasurmentsLabel"
        Me.voltageMeasurmentsLabel.Size = New System.Drawing.Size(150, 13)
        Me.voltageMeasurmentsLabel.TabIndex = 15
        Me.voltageMeasurmentsLabel.Text = "Voltage Measurements (Amps)"
        '
        'currentMeasurementsLabel
        '
        Me.currentMeasurementsLabel.AutoSize = True
        Me.currentMeasurementsLabel.Location = New System.Drawing.Point(0, 127)
        Me.currentMeasurementsLabel.Name = "currentMeasurementsLabel"
        Me.currentMeasurementsLabel.Size = New System.Drawing.Size(148, 13)
        Me.currentMeasurementsLabel.TabIndex = 16
        Me.currentMeasurementsLabel.Text = "Current Measurements (Amps)"
        '
        'pulseVoltLevelRangeNumeric
        '
        Me.pulseVoltLevelRangeNumeric.DecimalPlaces = 5
        Me.pulseVoltLevelRangeNumeric.Location = New System.Drawing.Point(182, 26)
        Me.pulseVoltLevelRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseVoltLevelRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseVoltLevelRangeNumeric.Name = "pulseVoltLevelRangeNumeric"
        Me.pulseVoltLevelRangeNumeric.Size = New System.Drawing.Size(90, 20)
        Me.pulseVoltLevelRangeNumeric.TabIndex = 3
        Me.pulseVoltLevelRangeNumeric.Value = New Decimal(New Integer() {6, 0, 0, 0})
        '
        'biasVoltageLevelNumeric
        '
        Me.biasVoltageLevelNumeric.DecimalPlaces = 5
        Me.biasVoltageLevelNumeric.Location = New System.Drawing.Point(182, 50)
        Me.biasVoltageLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.biasVoltageLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.biasVoltageLevelNumeric.Name = "biasVoltageLevelNumeric"
        Me.biasVoltageLevelNumeric.Size = New System.Drawing.Size(90, 20)
        Me.biasVoltageLevelNumeric.TabIndex = 4
        '
        'apertureTimeNumeric
        '
        Me.apertureTimeNumeric.DecimalPlaces = 5
        Me.apertureTimeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.apertureTimeNumeric.Location = New System.Drawing.Point(182, 74)
        Me.apertureTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.apertureTimeNumeric.Name = "apertureTimeNumeric"
        Me.apertureTimeNumeric.Size = New System.Drawing.Size(90, 20)
        Me.apertureTimeNumeric.TabIndex = 5
        Me.apertureTimeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 262144})
        '
        'pulseCurLimitRangeNumeric
        '
        Me.pulseCurLimitRangeNumeric.DecimalPlaces = 5
        Me.pulseCurLimitRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.pulseCurLimitRangeNumeric.Location = New System.Drawing.Point(182, 98)
        Me.pulseCurLimitRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseCurLimitRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseCurLimitRangeNumeric.Name = "pulseCurLimitRangeNumeric"
        Me.pulseCurLimitRangeNumeric.Size = New System.Drawing.Size(90, 20)
        Me.pulseCurLimitRangeNumeric.TabIndex = 6
        Me.pulseCurLimitRangeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'pulseCurrentLimitNumeric
        '
        Me.pulseCurrentLimitNumeric.DecimalPlaces = 5
        Me.pulseCurrentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.pulseCurrentLimitNumeric.Location = New System.Drawing.Point(182, 120)
        Me.pulseCurrentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseCurrentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseCurrentLimitNumeric.Name = "pulseCurrentLimitNumeric"
        Me.pulseCurrentLimitNumeric.Size = New System.Drawing.Size(90, 20)
        Me.pulseCurrentLimitNumeric.TabIndex = 7
        Me.pulseCurrentLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'biasCurrentLimitNumeric
        '
        Me.biasCurrentLimitNumeric.DecimalPlaces = 5
        Me.biasCurrentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.biasCurrentLimitNumeric.Location = New System.Drawing.Point(182, 142)
        Me.biasCurrentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.biasCurrentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.biasCurrentLimitNumeric.Name = "biasCurrentLimitNumeric"
        Me.biasCurrentLimitNumeric.Size = New System.Drawing.Size(90, 20)
        Me.biasCurrentLimitNumeric.TabIndex = 8
        Me.biasCurrentLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 131072})
        '
        'pulseOnTimeNumeric
        '
        Me.pulseOnTimeNumeric.DecimalPlaces = 7
        Me.pulseOnTimeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.pulseOnTimeNumeric.Location = New System.Drawing.Point(182, 165)
        Me.pulseOnTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseOnTimeNumeric.Name = "pulseOnTimeNumeric"
        Me.pulseOnTimeNumeric.Size = New System.Drawing.Size(90, 20)
        Me.pulseOnTimeNumeric.TabIndex = 9
        Me.pulseOnTimeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        'pulseOffTimeNumeric
        '
        Me.pulseOffTimeNumeric.DecimalPlaces = 7
        Me.pulseOffTimeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.pulseOffTimeNumeric.Location = New System.Drawing.Point(182, 188)
        Me.pulseOffTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseOffTimeNumeric.Name = "pulseOffTimeNumeric"
        Me.pulseOffTimeNumeric.Size = New System.Drawing.Size(90, 20)
        Me.pulseOffTimeNumeric.TabIndex = 10
        Me.pulseOffTimeNumeric.Value = New Decimal(New Integer() {5, 0, 0, 196608})
        '
        'pulseBiasDelayNumeric
        '
        Me.pulseBiasDelayNumeric.DecimalPlaces = 7
        Me.pulseBiasDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.pulseBiasDelayNumeric.Location = New System.Drawing.Point(182, 211)
        Me.pulseBiasDelayNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseBiasDelayNumeric.Name = "pulseBiasDelayNumeric"
        Me.pulseBiasDelayNumeric.Size = New System.Drawing.Size(90, 20)
        Me.pulseBiasDelayNumeric.TabIndex = 11
        Me.pulseBiasDelayNumeric.Value = New Decimal(New Integer() {1, 0, 0, 393216})
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(151, 48)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(121, 20)
        Me.channelNameTextBox.TabIndex = 2
        Me.channelNameTextBox.Text = "0"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(395, 290)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 13
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'pulsevoltageseq1Column
        '
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black
        Me.pulsevoltageseq1Column.DefaultCellStyle = DataGridViewCellStyle1
        Me.pulsevoltageseq1Column.Frozen = True
        Me.pulsevoltageseq1Column.HeaderText = ""
        Me.pulsevoltageseq1Column.Name = "pulsevoltageseq1Column"
        Me.pulsevoltageseq1Column.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.pulsevoltageseq1Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.pulsevoltageseq1Column.Width = 99
        '
        'sourcedelays1Column
        '
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        Me.sourcedelays1Column.DefaultCellStyle = DataGridViewCellStyle2
        Me.sourcedelays1Column.Frozen = True
        Me.sourcedelays1Column.HeaderText = ""
        Me.sourcedelays1Column.Name = "sourcedelays1Column"
        Me.sourcedelays1Column.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.sourcedelays1Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.sourcedelays1Column.Width = 99
        '
        'voltagemeasurments1Column
        '
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black
        Me.voltagemeasurments1Column.DefaultCellStyle = DataGridViewCellStyle3
        Me.voltagemeasurments1Column.Frozen = True
        Me.voltagemeasurments1Column.HeaderText = ""
        Me.voltagemeasurments1Column.Name = "voltagemeasurments1Column"
        Me.voltagemeasurments1Column.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.voltagemeasurments1Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.voltagemeasurments1Column.Width = 99
        '
        'currentmeasurements1Column
        '
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black
        Me.currentmeasurements1Column.DefaultCellStyle = DataGridViewCellStyle4
        Me.currentmeasurements1Column.Frozen = True
        Me.currentmeasurements1Column.HeaderText = ""
        Me.currentmeasurements1Column.Name = "currentmeasurements1Column"
        Me.currentmeasurements1Column.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.currentmeasurements1Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.currentmeasurements1Column.Width = 99
        '
        'pulseVoltageSequenceDataGridView
        '
        Me.pulseVoltageSequenceDataGridView.AllowUserToAddRows = False
        Me.pulseVoltageSequenceDataGridView.AllowUserToDeleteRows = False
        Me.pulseVoltageSequenceDataGridView.AllowUserToResizeColumns = False
        Me.pulseVoltageSequenceDataGridView.AllowUserToResizeRows = False
        Me.pulseVoltageSequenceDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.pulseVoltageSequenceDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.pulseVoltageSequenceDataGridView.ColumnHeadersVisible = False
        Me.pulseVoltageSequenceDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.pulsevoltageseq1Column})
        Me.pulseVoltageSequenceDataGridView.Location = New System.Drawing.Point(10, 41)
        Me.pulseVoltageSequenceDataGridView.Name = "pulseVoltageSequenceDataGridView"
        Me.pulseVoltageSequenceDataGridView.RowHeadersVisible = False
        Me.pulseVoltageSequenceDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.pulseVoltageSequenceDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.pulseVoltageSequenceDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.pulseVoltageSequenceDataGridView.Size = New System.Drawing.Size(98, 66)
        Me.pulseVoltageSequenceDataGridView.StandardTab = True
        Me.pulseVoltageSequenceDataGridView.TabIndex = 12
        '
        'sourceDelaysDataGridView
        '
        Me.sourceDelaysDataGridView.AllowUserToAddRows = False
        Me.sourceDelaysDataGridView.AllowUserToDeleteRows = False
        Me.sourceDelaysDataGridView.AllowUserToResizeColumns = False
        Me.sourceDelaysDataGridView.AllowUserToResizeRows = False
        Me.sourceDelaysDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.sourceDelaysDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.sourceDelaysDataGridView.ColumnHeadersVisible = False
        Me.sourceDelaysDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.sourcedelays1Column})
        Me.sourceDelaysDataGridView.Location = New System.Drawing.Point(10, 145)
        Me.sourceDelaysDataGridView.Name = "sourceDelaysDataGridView"
        Me.sourceDelaysDataGridView.RowHeadersVisible = False
        Me.sourceDelaysDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.sourceDelaysDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.sourceDelaysDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.sourceDelaysDataGridView.Size = New System.Drawing.Size(98, 67)
        Me.sourceDelaysDataGridView.StandardTab = True
        Me.sourceDelaysDataGridView.TabIndex = 13
        '
        'voltageMeasurmentsDataGridView
        '
        Me.voltageMeasurmentsDataGridView.AllowUserToAddRows = False
        Me.voltageMeasurmentsDataGridView.AllowUserToDeleteRows = False
        Me.voltageMeasurmentsDataGridView.AllowUserToResizeColumns = False
        Me.voltageMeasurmentsDataGridView.AllowUserToResizeRows = False
        Me.voltageMeasurmentsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.voltageMeasurmentsDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.voltageMeasurmentsDataGridView.ColumnHeadersVisible = False
        Me.voltageMeasurmentsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.voltagemeasurments1Column})
        Me.voltageMeasurmentsDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.voltageMeasurmentsDataGridView.Location = New System.Drawing.Point(9, 40)
        Me.voltageMeasurmentsDataGridView.Name = "voltageMeasurmentsDataGridView"
        Me.voltageMeasurmentsDataGridView.RowHeadersVisible = False
        Me.voltageMeasurmentsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.voltageMeasurmentsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.voltageMeasurmentsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.voltageMeasurmentsDataGridView.Size = New System.Drawing.Size(99, 67)
        Me.voltageMeasurmentsDataGridView.StandardTab = True
        Me.voltageMeasurmentsDataGridView.TabIndex = 14
        '
        'currentMeasurementsDataGridView
        '
        Me.currentMeasurementsDataGridView.AllowUserToAddRows = False
        Me.currentMeasurementsDataGridView.AllowUserToDeleteRows = False
        Me.currentMeasurementsDataGridView.AllowUserToResizeColumns = False
        Me.currentMeasurementsDataGridView.AllowUserToResizeRows = False
        Me.currentMeasurementsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.currentMeasurementsDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle8
        Me.currentMeasurementsDataGridView.ColumnHeadersVisible = False
        Me.currentMeasurementsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.currentmeasurements1Column})
        Me.currentMeasurementsDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.currentMeasurementsDataGridView.Location = New System.Drawing.Point(9, 145)
        Me.currentMeasurementsDataGridView.Name = "currentMeasurementsDataGridView"
        Me.currentMeasurementsDataGridView.RowHeadersVisible = False
        Me.currentMeasurementsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.currentMeasurementsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.currentMeasurementsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.currentMeasurementsDataGridView.Size = New System.Drawing.Size(99, 67)
        Me.currentMeasurementsDataGridView.StandardTab = True
        Me.currentMeasurementsDataGridView.TabIndex = 15
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(151, 21)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'resourceAndChannelNameGroupBox
        '
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.channelNameLabel)
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.resourceAndChannelNameGroupBox.Location = New System.Drawing.Point(11, 15)
        Me.resourceAndChannelNameGroupBox.Name = "resourceAndChannelNameGroupBox"
        Me.resourceAndChannelNameGroupBox.Size = New System.Drawing.Size(279, 76)
        Me.resourceAndChannelNameGroupBox.TabIndex = 18
        Me.resourceAndChannelNameGroupBox.TabStop = False
        Me.resourceAndChannelNameGroupBox.Text = "Resource Name and Channel Name"
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.pulseVoltageLevelRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseVoltLevelRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.biasVoltageLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.biasVoltageLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseBiasDelayLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseOffTimeLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseBiasDelayNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseOnTimeLabel)
        Me.configurationGroupBox.Controls.Add(Me.biasCurrentLimitLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseOffTimeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseCurrentLimitLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseCurrentLimitRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseOnTimeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.apertureTimeLabel)
        Me.configurationGroupBox.Controls.Add(Me.apertureTimeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseCurLimitRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseCurrentLimitNumeric)
        Me.configurationGroupBox.Controls.Add(Me.biasCurrentLimitNumeric)
        Me.configurationGroupBox.Location = New System.Drawing.Point(11, 109)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(279, 240)
        Me.configurationGroupBox.TabIndex = 19
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'inputSequenceGroupBox
        '
        Me.inputSequenceGroupBox.Controls.Add(Me.pulseVoltageSequenceDataGridView)
        Me.inputSequenceGroupBox.Controls.Add(Me.pulseVoltageSequenceLabel)
        Me.inputSequenceGroupBox.Controls.Add(Me.sourceDelaysLabel)
        Me.inputSequenceGroupBox.Controls.Add(Me.sourceDelaysDataGridView)
        Me.inputSequenceGroupBox.Location = New System.Drawing.Point(296, 15)
        Me.inputSequenceGroupBox.Name = "inputSequenceGroupBox"
        Me.inputSequenceGroupBox.Size = New System.Drawing.Size(137, 229)
        Me.inputSequenceGroupBox.TabIndex = 20
        Me.inputSequenceGroupBox.TabStop = False
        Me.inputSequenceGroupBox.Text = "Input Sequences"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.voltageMeasurmentsLabel)
        Me.measurementGroupBox.Controls.Add(Me.voltageMeasurmentsDataGridView)
        Me.measurementGroupBox.Controls.Add(Me.currentMeasurementsLabel)
        Me.measurementGroupBox.Controls.Add(Me.currentMeasurementsDataGridView)
        Me.measurementGroupBox.Location = New System.Drawing.Point(439, 15)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(158, 229)
        Me.measurementGroupBox.TabIndex = 21
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurements"
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(611, 362)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.inputSequenceGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceAndChannelNameGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Pulse Voltage Sequence"
        CType(Me.pulseVoltLevelRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.biasVoltageLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.apertureTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseCurLimitRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.biasCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseOnTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseOffTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseBiasDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseVoltageSequenceDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sourceDelaysDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageMeasurmentsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.currentMeasurementsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.resourceAndChannelNameGroupBox.ResumeLayout(False)
        Me.resourceAndChannelNameGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.inputSequenceGroupBox.ResumeLayout(False)
        Me.inputSequenceGroupBox.PerformLayout()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

	#End Region

	Private resourceNameLabel As System.Windows.Forms.Label
	Private channelNameLabel As System.Windows.Forms.Label
	Private pulseVoltageSequenceLabel As System.Windows.Forms.Label
	Private sourceDelaysLabel As System.Windows.Forms.Label
	Private pulseVoltageLevelRangeLabel As System.Windows.Forms.Label
	Private biasVoltageLevelLabel As System.Windows.Forms.Label
	Private apertureTimeLabel As System.Windows.Forms.Label
	Private pulseCurrentLimitRangeLabel As System.Windows.Forms.Label
	Private pulseCurrentLimitLabel As System.Windows.Forms.Label
	Private biasCurrentLimitLabel As System.Windows.Forms.Label
	Private pulseOnTimeLabel As System.Windows.Forms.Label
	Private pulseOffTimeLabel As System.Windows.Forms.Label
	Private pulseBiasDelayLabel As System.Windows.Forms.Label
	Private voltageMeasurmentsLabel As System.Windows.Forms.Label
	Private currentMeasurementsLabel As System.Windows.Forms.Label
	Private pulseVoltLevelRangeNumeric As System.Windows.Forms.NumericUpDown
	Private biasVoltageLevelNumeric As System.Windows.Forms.NumericUpDown
	Private apertureTimeNumeric As System.Windows.Forms.NumericUpDown
	Private pulseCurLimitRangeNumeric As System.Windows.Forms.NumericUpDown
	Private pulseCurrentLimitNumeric As System.Windows.Forms.NumericUpDown
	Private biasCurrentLimitNumeric As System.Windows.Forms.NumericUpDown
	Private pulseOnTimeNumeric As System.Windows.Forms.NumericUpDown
	Private pulseOffTimeNumeric As System.Windows.Forms.NumericUpDown
	Private pulseBiasDelayNumeric As System.Windows.Forms.NumericUpDown
	Private channelNameTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
	Private pulsevoltageseq1Column As System.Windows.Forms.DataGridViewTextBoxColumn
	Private sourcedelays1Column As System.Windows.Forms.DataGridViewTextBoxColumn
	Private voltagemeasurments1Column As System.Windows.Forms.DataGridViewTextBoxColumn
	Private currentmeasurements1Column As System.Windows.Forms.DataGridViewTextBoxColumn
	Private pulseVoltageSequenceDataGridView As System.Windows.Forms.DataGridView
	Private sourceDelaysDataGridView As System.Windows.Forms.DataGridView
	Private voltageMeasurmentsDataGridView As System.Windows.Forms.DataGridView
	Private currentMeasurementsDataGridView As System.Windows.Forms.DataGridView
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private resourceAndChannelNameGroupBox As System.Windows.Forms.GroupBox
	Private configurationGroupBox As System.Windows.Forms.GroupBox
	Private inputSequenceGroupBox As System.Windows.Forms.GroupBox
	Private measurementGroupBox As System.Windows.Forms.GroupBox
End Class

