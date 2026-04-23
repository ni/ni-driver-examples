
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.pulseCurrentSequenceLabel = New System.Windows.Forms.Label()
        Me.sourceDelaysLabel = New System.Windows.Forms.Label()
        Me.pulseCurrentLevelRangeLabel = New System.Windows.Forms.Label()
        Me.biasCurrentLevelLabel = New System.Windows.Forms.Label()
        Me.apertureTimeLabel = New System.Windows.Forms.Label()
        Me.pulseVoltageLimitRangeLabel = New System.Windows.Forms.Label()
        Me.pulseVoltageLimitLabel = New System.Windows.Forms.Label()
        Me.biasVoltageLimitLabel = New System.Windows.Forms.Label()
        Me.pulseOnTimeLabel = New System.Windows.Forms.Label()
        Me.pulseOffTimeLabel = New System.Windows.Forms.Label()
        Me.pulseBiasDelayLabel = New System.Windows.Forms.Label()
        Me.voltageMeasurmentsLabel = New System.Windows.Forms.Label()
        Me.currentMeasurementsLabel = New System.Windows.Forms.Label()
        Me.pulseCurrentLevelRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.biasCurrentLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.apertureTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseVoltageLimitRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseVoltageLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.biasVoltageLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseOnTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseOffTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pulseBiasDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.pulsecurrentseq1Column = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.sourcedelays1Column = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.voltagemeasurments1Column = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.currentmeasurements1Column = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pulseCurrentSequenceDataGridView = New System.Windows.Forms.DataGridView()
        Me.sourceDelaysDataGridView = New System.Windows.Forms.DataGridView()
        Me.voltageMeasurmentsDataGridView = New System.Windows.Forms.DataGridView()
        Me.currentMeasurementsDataGridView = New System.Windows.Forms.DataGridView()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceAndChannelNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.inputSequenceGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.pulseCurrentLevelRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.biasCurrentLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.apertureTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseVoltageLimitRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.biasVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseOnTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseOffTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseBiasDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pulseCurrentSequenceDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.resourceNameLabel.Location = New System.Drawing.Point(4, 26)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.Location = New System.Drawing.Point(11, 53)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.channelNameLabel.TabIndex = 1
        Me.channelNameLabel.Text = "Channel Name"
        '
        'pulseCurrentSequenceLabel
        '
        Me.pulseCurrentSequenceLabel.AutoSize = True
        Me.pulseCurrentSequenceLabel.Location = New System.Drawing.Point(4, 29)
        Me.pulseCurrentSequenceLabel.Name = "pulseCurrentSequenceLabel"
        Me.pulseCurrentSequenceLabel.Size = New System.Drawing.Size(122, 13)
        Me.pulseCurrentSequenceLabel.TabIndex = 2
        Me.pulseCurrentSequenceLabel.Text = "Pulse Current Sequence"
        '
        'sourceDelaysLabel
        '
        Me.sourceDelaysLabel.AutoSize = True
        Me.sourceDelaysLabel.Location = New System.Drawing.Point(5, 154)
        Me.sourceDelaysLabel.Name = "sourceDelaysLabel"
        Me.sourceDelaysLabel.Size = New System.Drawing.Size(76, 13)
        Me.sourceDelaysLabel.TabIndex = 3
        Me.sourceDelaysLabel.Text = "Source Delays"
        '
        'pulseCurrentLevelRangeLabel
        '
        Me.pulseCurrentLevelRangeLabel.AutoSize = True
        Me.pulseCurrentLevelRangeLabel.Location = New System.Drawing.Point(6, 23)
        Me.pulseCurrentLevelRangeLabel.Name = "pulseCurrentLevelRangeLabel"
        Me.pulseCurrentLevelRangeLabel.Size = New System.Drawing.Size(169, 13)
        Me.pulseCurrentLevelRangeLabel.TabIndex = 4
        Me.pulseCurrentLevelRangeLabel.Text = "Pulse Current Level Range (Amps)"
        '
        'biasCurrentLevelLabel
        '
        Me.biasCurrentLevelLabel.AutoSize = True
        Me.biasCurrentLevelLabel.Location = New System.Drawing.Point(6, 47)
        Me.biasCurrentLevelLabel.Name = "biasCurrentLevelLabel"
        Me.biasCurrentLevelLabel.Size = New System.Drawing.Size(157, 13)
        Me.biasCurrentLevelLabel.TabIndex = 5
        Me.biasCurrentLevelLabel.Text = "Pulse Bias Current Level (Amps)"
        '
        'apertureTimeLabel
        '
        Me.apertureTimeLabel.AutoSize = True
        Me.apertureTimeLabel.Location = New System.Drawing.Point(6, 70)
        Me.apertureTimeLabel.Name = "apertureTimeLabel"
        Me.apertureTimeLabel.Size = New System.Drawing.Size(101, 13)
        Me.apertureTimeLabel.TabIndex = 6
        Me.apertureTimeLabel.Text = "Aperture Time (Sec)"
        '
        'pulseVoltageLimitRangeLabel
        '
        Me.pulseVoltageLimitRangeLabel.AutoSize = True
        Me.pulseVoltageLimitRangeLabel.Location = New System.Drawing.Point(6, 92)
        Me.pulseVoltageLimitRangeLabel.Name = "pulseVoltageLimitRangeLabel"
        Me.pulseVoltageLimitRangeLabel.Size = New System.Drawing.Size(163, 13)
        Me.pulseVoltageLimitRangeLabel.TabIndex = 7
        Me.pulseVoltageLimitRangeLabel.Text = "Pulse Voltage Limit Range (Volts)"
        '
        'pulseVoltageLimitLabel
        '
        Me.pulseVoltageLimitLabel.AutoSize = True
        Me.pulseVoltageLimitLabel.Location = New System.Drawing.Point(6, 140)
        Me.pulseVoltageLimitLabel.Name = "pulseVoltageLimitLabel"
        Me.pulseVoltageLimitLabel.Size = New System.Drawing.Size(128, 13)
        Me.pulseVoltageLimitLabel.TabIndex = 8
        Me.pulseVoltageLimitLabel.Text = "Pulse Voltage Limit (Volts)"
        '
        'biasVoltageLimitLabel
        '
        Me.biasVoltageLimitLabel.AutoSize = True
        Me.biasVoltageLimitLabel.Location = New System.Drawing.Point(6, 186)
        Me.biasVoltageLimitLabel.Name = "biasVoltageLimitLabel"
        Me.biasVoltageLimitLabel.Size = New System.Drawing.Size(151, 13)
        Me.biasVoltageLimitLabel.TabIndex = 9
        Me.biasVoltageLimitLabel.Text = "Pulse Bias Voltage Limit (Volts)"
        '
        'pulseOnTimeLabel
        '
        Me.pulseOnTimeLabel.AutoSize = True
        Me.pulseOnTimeLabel.Location = New System.Drawing.Point(6, 116)
        Me.pulseOnTimeLabel.Name = "pulseOnTimeLabel"
        Me.pulseOnTimeLabel.Size = New System.Drawing.Size(104, 13)
        Me.pulseOnTimeLabel.TabIndex = 10
        Me.pulseOnTimeLabel.Text = "Pulse On Time (Sec)"
        '
        'pulseOffTimeLabel
        '
        Me.pulseOffTimeLabel.AutoSize = True
        Me.pulseOffTimeLabel.Location = New System.Drawing.Point(6, 164)
        Me.pulseOffTimeLabel.Name = "pulseOffTimeLabel"
        Me.pulseOffTimeLabel.Size = New System.Drawing.Size(104, 13)
        Me.pulseOffTimeLabel.TabIndex = 11
        Me.pulseOffTimeLabel.Text = "Pulse Off Time (Sec)"
        '
        'pulseBiasDelayLabel
        '
        Me.pulseBiasDelayLabel.AutoSize = True
        Me.pulseBiasDelayLabel.Location = New System.Drawing.Point(6, 212)
        Me.pulseBiasDelayLabel.Name = "pulseBiasDelayLabel"
        Me.pulseBiasDelayLabel.Size = New System.Drawing.Size(114, 13)
        Me.pulseBiasDelayLabel.TabIndex = 12
        Me.pulseBiasDelayLabel.Text = "Pulse Bias Delay (Sec)"
        '
        'voltageMeasurmentsLabel
        '
        Me.voltageMeasurmentsLabel.AutoSize = True
        Me.voltageMeasurmentsLabel.Location = New System.Drawing.Point(6, 26)
        Me.voltageMeasurmentsLabel.Name = "voltageMeasurmentsLabel"
        Me.voltageMeasurmentsLabel.Size = New System.Drawing.Size(147, 13)
        Me.voltageMeasurmentsLabel.TabIndex = 15
        Me.voltageMeasurmentsLabel.Text = "Voltage Measurements (Volts)"
        '
        'currentMeasurementsLabel
        '
        Me.currentMeasurementsLabel.AutoSize = True
        Me.currentMeasurementsLabel.Location = New System.Drawing.Point(5, 156)
        Me.currentMeasurementsLabel.Name = "currentMeasurementsLabel"
        Me.currentMeasurementsLabel.Size = New System.Drawing.Size(148, 13)
        Me.currentMeasurementsLabel.TabIndex = 16
        Me.currentMeasurementsLabel.Text = "Current Measurements (Amps)"
        '
        'pulseCurrentLevelRangeNumeric
        '
        Me.pulseCurrentLevelRangeNumeric.DecimalPlaces = 5
        Me.pulseCurrentLevelRangeNumeric.Location = New System.Drawing.Point(204, 21)
        Me.pulseCurrentLevelRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseCurrentLevelRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseCurrentLevelRangeNumeric.Name = "pulseCurrentLevelRangeNumeric"
        Me.pulseCurrentLevelRangeNumeric.Size = New System.Drawing.Size(90, 20)
        Me.pulseCurrentLevelRangeNumeric.TabIndex = 3
        Me.pulseCurrentLevelRangeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'biasCurrentLevelNumeric
        '
        Me.biasCurrentLevelNumeric.DecimalPlaces = 5
        Me.biasCurrentLevelNumeric.ForeColor = System.Drawing.SystemColors.MenuText
        Me.biasCurrentLevelNumeric.Location = New System.Drawing.Point(204, 45)
        Me.biasCurrentLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.biasCurrentLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.biasCurrentLevelNumeric.Name = "biasCurrentLevelNumeric"
        Me.biasCurrentLevelNumeric.Size = New System.Drawing.Size(90, 20)
        Me.biasCurrentLevelNumeric.TabIndex = 4
        '
        'apertureTimeNumeric
        '
        Me.apertureTimeNumeric.DecimalPlaces = 7
        Me.apertureTimeNumeric.Location = New System.Drawing.Point(204, 68)
        Me.apertureTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.apertureTimeNumeric.Name = "apertureTimeNumeric"
        Me.apertureTimeNumeric.Size = New System.Drawing.Size(90, 20)
        Me.apertureTimeNumeric.TabIndex = 5
        Me.apertureTimeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 262144})
        '
        'pulseVoltageLimitRangeNumeric
        '
        Me.pulseVoltageLimitRangeNumeric.DecimalPlaces = 5
        Me.pulseVoltageLimitRangeNumeric.Location = New System.Drawing.Point(204, 90)
        Me.pulseVoltageLimitRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseVoltageLimitRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseVoltageLimitRangeNumeric.Name = "pulseVoltageLimitRangeNumeric"
        Me.pulseVoltageLimitRangeNumeric.Size = New System.Drawing.Size(90, 20)
        Me.pulseVoltageLimitRangeNumeric.TabIndex = 6
        Me.pulseVoltageLimitRangeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'pulseVoltageLimitNumeric
        '
        Me.pulseVoltageLimitNumeric.DecimalPlaces = 5
        Me.pulseVoltageLimitNumeric.Location = New System.Drawing.Point(204, 138)
        Me.pulseVoltageLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseVoltageLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseVoltageLimitNumeric.Name = "pulseVoltageLimitNumeric"
        Me.pulseVoltageLimitNumeric.Size = New System.Drawing.Size(90, 20)
        Me.pulseVoltageLimitNumeric.TabIndex = 8
        Me.pulseVoltageLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'biasVoltageLimitNumeric
        '
        Me.biasVoltageLimitNumeric.DecimalPlaces = 5
        Me.biasVoltageLimitNumeric.Location = New System.Drawing.Point(204, 186)
        Me.biasVoltageLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.biasVoltageLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.biasVoltageLimitNumeric.Name = "biasVoltageLimitNumeric"
        Me.biasVoltageLimitNumeric.Size = New System.Drawing.Size(90, 20)
        Me.biasVoltageLimitNumeric.TabIndex = 10
        Me.biasVoltageLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'pulseOnTimeNumeric
        '
        Me.pulseOnTimeNumeric.DecimalPlaces = 7
        Me.pulseOnTimeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.pulseOnTimeNumeric.Location = New System.Drawing.Point(204, 114)
        Me.pulseOnTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseOnTimeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseOnTimeNumeric.Name = "pulseOnTimeNumeric"
        Me.pulseOnTimeNumeric.Size = New System.Drawing.Size(90, 20)
        Me.pulseOnTimeNumeric.TabIndex = 7
        Me.pulseOnTimeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        'pulseOffTimeNumeric
        '
        Me.pulseOffTimeNumeric.DecimalPlaces = 7
        Me.pulseOffTimeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.pulseOffTimeNumeric.Location = New System.Drawing.Point(204, 162)
        Me.pulseOffTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseOffTimeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseOffTimeNumeric.Name = "pulseOffTimeNumeric"
        Me.pulseOffTimeNumeric.Size = New System.Drawing.Size(90, 20)
        Me.pulseOffTimeNumeric.TabIndex = 9
        Me.pulseOffTimeNumeric.Value = New Decimal(New Integer() {5, 0, 0, 196608})
        '
        'pulseBiasDelayNumeric
        '
        Me.pulseBiasDelayNumeric.DecimalPlaces = 7
        Me.pulseBiasDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.pulseBiasDelayNumeric.Location = New System.Drawing.Point(204, 210)
        Me.pulseBiasDelayNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.pulseBiasDelayNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.pulseBiasDelayNumeric.Name = "pulseBiasDelayNumeric"
        Me.pulseBiasDelayNumeric.Size = New System.Drawing.Size(90, 20)
        Me.pulseBiasDelayNumeric.TabIndex = 11
        Me.pulseBiasDelayNumeric.Value = New Decimal(New Integer() {1, 0, 0, 393216})
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(188, 50)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(106, 20)
        Me.channelNameTextBox.TabIndex = 2
        Me.channelNameTextBox.Text = "0"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(439, 317)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 16
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'pulsecurrentseq1Column
        '
        Me.pulsecurrentseq1Column.Name = "pulsecurrentseq1Column"
        '
        'sourcedelays1Column
        '
        Me.sourcedelays1Column.Name = "sourcedelays1Column"
        '
        'voltagemeasurments1Column
        '
        Me.voltagemeasurments1Column.Name = "voltagemeasurments1Column"
        '
        'currentmeasurements1Column
        '
        Me.currentmeasurements1Column.Name = "currentmeasurements1Column"
        '
        'pulseCurrentSequenceDataGridView
        '
        Me.pulseCurrentSequenceDataGridView.AllowUserToAddRows = False
        Me.pulseCurrentSequenceDataGridView.AllowUserToDeleteRows = False
        Me.pulseCurrentSequenceDataGridView.AllowUserToResizeColumns = False
        Me.pulseCurrentSequenceDataGridView.AllowUserToResizeRows = False
        Me.pulseCurrentSequenceDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.pulseCurrentSequenceDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.pulseCurrentSequenceDataGridView.ColumnHeadersVisible = False
        Me.pulseCurrentSequenceDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.pulsecurrentseq1Column})
        Me.pulseCurrentSequenceDataGridView.Location = New System.Drawing.Point(9, 45)
        Me.pulseCurrentSequenceDataGridView.Name = "pulseCurrentSequenceDataGridView"
        Me.pulseCurrentSequenceDataGridView.RowHeadersVisible = False
        Me.pulseCurrentSequenceDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.pulseCurrentSequenceDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.pulseCurrentSequenceDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.pulseCurrentSequenceDataGridView.Size = New System.Drawing.Size(96, 88)
        Me.pulseCurrentSequenceDataGridView.StandardTab = True
        Me.pulseCurrentSequenceDataGridView.TabIndex = 12
        '
        'sourceDelaysDataGridView
        '
        Me.sourceDelaysDataGridView.AllowUserToAddRows = False
        Me.sourceDelaysDataGridView.AllowUserToDeleteRows = False
        Me.sourceDelaysDataGridView.AllowUserToResizeColumns = False
        Me.sourceDelaysDataGridView.AllowUserToResizeRows = False
        Me.sourceDelaysDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.sourceDelaysDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.sourceDelaysDataGridView.ColumnHeadersVisible = False
        Me.sourceDelaysDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.sourcedelays1Column})
        Me.sourceDelaysDataGridView.Location = New System.Drawing.Point(9, 174)
        Me.sourceDelaysDataGridView.Name = "sourceDelaysDataGridView"
        Me.sourceDelaysDataGridView.RowHeadersVisible = False
        Me.sourceDelaysDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.sourceDelaysDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.sourceDelaysDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.sourceDelaysDataGridView.Size = New System.Drawing.Size(96, 88)
        Me.sourceDelaysDataGridView.StandardTab = True
        Me.sourceDelaysDataGridView.TabIndex = 13
        '
        'voltageMeasurmentsDataGridView
        '
        Me.voltageMeasurmentsDataGridView.AllowUserToAddRows = False
        Me.voltageMeasurmentsDataGridView.AllowUserToDeleteRows = False
        Me.voltageMeasurmentsDataGridView.AllowUserToResizeColumns = False
        Me.voltageMeasurmentsDataGridView.AllowUserToResizeRows = False
        Me.voltageMeasurmentsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.voltageMeasurmentsDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.voltageMeasurmentsDataGridView.ColumnHeadersVisible = False
        Me.voltageMeasurmentsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.voltagemeasurments1Column})
        Me.voltageMeasurmentsDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.voltageMeasurmentsDataGridView.Location = New System.Drawing.Point(13, 50)
        Me.voltageMeasurmentsDataGridView.Name = "voltageMeasurmentsDataGridView"
        Me.voltageMeasurmentsDataGridView.RowHeadersVisible = False
        Me.voltageMeasurmentsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.voltageMeasurmentsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.voltageMeasurmentsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.voltageMeasurmentsDataGridView.Size = New System.Drawing.Size(94, 88)
        Me.voltageMeasurmentsDataGridView.StandardTab = True
        Me.voltageMeasurmentsDataGridView.TabIndex = 14
        '
        'currentMeasurementsDataGridView
        '
        Me.currentMeasurementsDataGridView.AllowUserToAddRows = False
        Me.currentMeasurementsDataGridView.AllowUserToDeleteRows = False
        Me.currentMeasurementsDataGridView.AllowUserToResizeColumns = False
        Me.currentMeasurementsDataGridView.AllowUserToResizeRows = False
        Me.currentMeasurementsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.currentMeasurementsDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.currentMeasurementsDataGridView.ColumnHeadersVisible = False
        Me.currentMeasurementsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.currentmeasurements1Column})
        Me.currentMeasurementsDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.currentMeasurementsDataGridView.Location = New System.Drawing.Point(13, 176)
        Me.currentMeasurementsDataGridView.Name = "currentMeasurementsDataGridView"
        Me.currentMeasurementsDataGridView.RowHeadersVisible = False
        Me.currentMeasurementsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.currentMeasurementsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.currentMeasurementsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.currentMeasurementsDataGridView.Size = New System.Drawing.Size(94, 88)
        Me.currentMeasurementsDataGridView.StandardTab = True
        Me.currentMeasurementsDataGridView.TabIndex = 15
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(188, 23)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(106, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'resourceAndChannelNameGroupBox
        '
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.channelNameLabel)
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.resourceAndChannelNameGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.resourceAndChannelNameGroupBox.Location = New System.Drawing.Point(12, 14)
        Me.resourceAndChannelNameGroupBox.Name = "resourceAndChannelNameGroupBox"
        Me.resourceAndChannelNameGroupBox.Size = New System.Drawing.Size(311, 80)
        Me.resourceAndChannelNameGroupBox.TabIndex = 18
        Me.resourceAndChannelNameGroupBox.TabStop = False
        Me.resourceAndChannelNameGroupBox.Text = "Resource Name and Channel Name"
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.pulseCurrentLevelRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseCurrentLevelRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.biasCurrentLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.biasCurrentLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseBiasDelayLabel)
        Me.configurationGroupBox.Controls.Add(Me.biasVoltageLimitLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseBiasDelayNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseVoltageLimitLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseOffTimeLabel)
        Me.configurationGroupBox.Controls.Add(Me.biasVoltageLimitNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseVoltageLimitRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.apertureTimeLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseOnTimeLabel)
        Me.configurationGroupBox.Controls.Add(Me.pulseOffTimeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.apertureTimeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseVoltageLimitNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseVoltageLimitRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.pulseOnTimeNumeric)
        Me.configurationGroupBox.Location = New System.Drawing.Point(13, 100)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(311, 249)
        Me.configurationGroupBox.TabIndex = 19
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'inputSequenceGroupBox
        '
        Me.inputSequenceGroupBox.Controls.Add(Me.pulseCurrentSequenceDataGridView)
        Me.inputSequenceGroupBox.Controls.Add(Me.pulseCurrentSequenceLabel)
        Me.inputSequenceGroupBox.Controls.Add(Me.sourceDelaysLabel)
        Me.inputSequenceGroupBox.Controls.Add(Me.sourceDelaysDataGridView)
        Me.inputSequenceGroupBox.Location = New System.Drawing.Point(330, 14)
        Me.inputSequenceGroupBox.Name = "inputSequenceGroupBox"
        Me.inputSequenceGroupBox.Size = New System.Drawing.Size(138, 285)
        Me.inputSequenceGroupBox.TabIndex = 18
        Me.inputSequenceGroupBox.TabStop = False
        Me.inputSequenceGroupBox.Text = "Input Sequence"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.currentMeasurementsDataGridView)
        Me.measurementGroupBox.Controls.Add(Me.currentMeasurementsLabel)
        Me.measurementGroupBox.Controls.Add(Me.voltageMeasurmentsDataGridView)
        Me.measurementGroupBox.Controls.Add(Me.voltageMeasurmentsLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(474, 14)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(156, 285)
        Me.measurementGroupBox.TabIndex = 20
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(640, 360)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.inputSequenceGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceAndChannelNameGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Pulse Current Sequence"
        CType(Me.pulseCurrentLevelRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.biasCurrentLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.apertureTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseVoltageLimitRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.biasVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseOnTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseOffTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseBiasDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pulseCurrentSequenceDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
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
	Private pulseCurrentSequenceLabel As System.Windows.Forms.Label
	Private sourceDelaysLabel As System.Windows.Forms.Label
	Private pulseCurrentLevelRangeLabel As System.Windows.Forms.Label
	Private biasCurrentLevelLabel As System.Windows.Forms.Label
	Private apertureTimeLabel As System.Windows.Forms.Label
	Private pulseVoltageLimitRangeLabel As System.Windows.Forms.Label
	Private pulseVoltageLimitLabel As System.Windows.Forms.Label
	Private biasVoltageLimitLabel As System.Windows.Forms.Label
	Private pulseOnTimeLabel As System.Windows.Forms.Label
	Private pulseOffTimeLabel As System.Windows.Forms.Label
	Private pulseBiasDelayLabel As System.Windows.Forms.Label
	Private voltageMeasurmentsLabel As System.Windows.Forms.Label
	Private currentMeasurementsLabel As System.Windows.Forms.Label
	Private pulseCurrentLevelRangeNumeric As System.Windows.Forms.NumericUpDown
	Private biasCurrentLevelNumeric As System.Windows.Forms.NumericUpDown
	Private apertureTimeNumeric As System.Windows.Forms.NumericUpDown
	Private pulseVoltageLimitRangeNumeric As System.Windows.Forms.NumericUpDown
	Private pulseVoltageLimitNumeric As System.Windows.Forms.NumericUpDown
	Private biasVoltageLimitNumeric As System.Windows.Forms.NumericUpDown
	Private pulseOnTimeNumeric As System.Windows.Forms.NumericUpDown
	Private pulseOffTimeNumeric As System.Windows.Forms.NumericUpDown
	Private pulseBiasDelayNumeric As System.Windows.Forms.NumericUpDown
	Private channelNameTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
	Private pulsecurrentseq1Column As System.Windows.Forms.DataGridViewTextBoxColumn
	Private sourcedelays1Column As System.Windows.Forms.DataGridViewTextBoxColumn
	Private voltagemeasurments1Column As System.Windows.Forms.DataGridViewTextBoxColumn
	Private currentmeasurements1Column As System.Windows.Forms.DataGridViewTextBoxColumn
	Private pulseCurrentSequenceDataGridView As System.Windows.Forms.DataGridView
	Private sourceDelaysDataGridView As System.Windows.Forms.DataGridView
	Private voltageMeasurmentsDataGridView As System.Windows.Forms.DataGridView
	Private currentMeasurementsDataGridView As System.Windows.Forms.DataGridView
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private resourceAndChannelNameGroupBox As System.Windows.Forms.GroupBox
	Private configurationGroupBox As System.Windows.Forms.GroupBox
	Private inputSequenceGroupBox As System.Windows.Forms.GroupBox
	Private measurementGroupBox As System.Windows.Forms.GroupBox
End Class

