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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.resourcenameLabel = New System.Windows.Forms.Label()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.measurementsDataGridView = New System.Windows.Forms.DataGridView()
        Me.Point = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Frequency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Impedance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.measurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.lcrShortCompensationEnabledCheckBox = New System.Windows.Forms.CheckBox()
        Me.lcrOpenCompensationEnabledCheckBox = New System.Windows.Forms.CheckBox()
        Me.lcrDcBiasCurrentLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.lcrDcBiasCurrentLevel = New System.Windows.Forms.Label()
        Me.lcrCustomMeasurementTimeSecNumeric = New System.Windows.Forms.NumericUpDown()
        Me.lcrMeasurementTimeComboBox = New System.Windows.Forms.ComboBox()
        Me.cableLengthComboBox = New System.Windows.Forms.ComboBox()
        Me.lcrDcBiasVoltageLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.lcrDcBiasSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.lcrImpedanceRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.ImpedanceRangeLabel = New System.Windows.Forms.Label()
        Me.lcrCustomMeasurementTimeSecLabel = New System.Windows.Forms.Label()
        Me.lcrMeasurementTimeLabel = New System.Windows.Forms.Label()
        Me.cableLengthLabel = New System.Windows.Forms.Label()
        Me.lcrDcBiasVoltageLevelLabel = New System.Windows.Forms.Label()
        Me.lcrDcBiasSourceLabel = New System.Windows.Forms.Label()
        Me.VoltageAmplitudeRmsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.voltageAmplitudeRmsLabel = New System.Windows.Forms.Label()
        Me.numberOfStepsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.endFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.numberOfStepsLabel = New System.Windows.Forms.Label()
        Me.startFrequencyLabel = New System.Windows.Forms.Label()
        Me.endFrequencyLabel = New System.Windows.Forms.Label()
        Me.startButton = New System.Windows.Forms.Button()
        CType(Me.measurementsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementsGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.lcrDcBiasCurrentLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lcrCustomMeasurementTimeSecNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lcrDcBiasVoltageLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lcrImpedanceRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VoltageAmplitudeRmsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberOfStepsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.endFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'resourcenameLabel
        '
        Me.resourcenameLabel.AutoSize = True
        Me.resourcenameLabel.Location = New System.Drawing.Point(13, 24)
        Me.resourcenameLabel.Name = "resourcenameLabel"
        Me.resourcenameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourcenameLabel.TabIndex = 0
        Me.resourcenameLabel.Text = "Resource Name"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(13, 40)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(275, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'measurementsDataGridView
        '
        Me.measurementsDataGridView.AllowUserToAddRows = False
        Me.measurementsDataGridView.AllowUserToDeleteRows = False
        Me.measurementsDataGridView.AllowUserToResizeColumns = False
        Me.measurementsDataGridView.AllowUserToResizeRows = False
        Me.measurementsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.measurementsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Point, Me.Frequency, Me.Impedance})
        Me.measurementsDataGridView.Location = New System.Drawing.Point(11, 18)
        Me.measurementsDataGridView.Name = "measurementsDataGridView"
        Me.measurementsDataGridView.RowHeadersVisible = False
        Me.measurementsDataGridView.Size = New System.Drawing.Size(265, 380)
        Me.measurementsDataGridView.TabIndex = 0
        '
        'Point
        '
        Me.Point.HeaderText = "Point"
        Me.Point.Name = "Point"
        Me.Point.Width = 40
        '
        'Frequency
        '
        Me.Frequency.HeaderText = "Frequency"
        Me.Frequency.Name = "Frequency"
        Me.Frequency.Width = 110
        '
        'Impedance
        '
        Me.Impedance.HeaderText = "Impedance"
        Me.Impedance.Name = "Impedance"
        Me.Impedance.Width = 110
        '
        'measurementsGroupBox
        '
        Me.measurementsGroupBox.Controls.Add(Me.measurementsDataGridView)
        Me.measurementsGroupBox.Location = New System.Drawing.Point(521, 15)
        Me.measurementsGroupBox.Name = "measurementsGroupBox"
        Me.measurementsGroupBox.Size = New System.Drawing.Size(287, 405)
        Me.measurementsGroupBox.TabIndex = 1
        Me.measurementsGroupBox.TabStop = False
        Me.measurementsGroupBox.Text = "Measurements"
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.configurationGroupBox.Controls.Add(Me.resourcenameLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrShortCompensationEnabledCheckBox)
        Me.configurationGroupBox.Controls.Add(Me.lcrOpenCompensationEnabledCheckBox)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasCurrentLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasCurrentLevel)
        Me.configurationGroupBox.Controls.Add(Me.lcrCustomMeasurementTimeSecNumeric)
        Me.configurationGroupBox.Controls.Add(Me.lcrMeasurementTimeComboBox)
        Me.configurationGroupBox.Controls.Add(Me.cableLengthComboBox)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasVoltageLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasSourceComboBox)
        Me.configurationGroupBox.Controls.Add(Me.lcrImpedanceRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.ImpedanceRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrCustomMeasurementTimeSecLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrMeasurementTimeLabel)
        Me.configurationGroupBox.Controls.Add(Me.cableLengthLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasVoltageLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasSourceLabel)
        Me.configurationGroupBox.Controls.Add(Me.VoltageAmplitudeRmsNumeric)
        Me.configurationGroupBox.Controls.Add(Me.voltageAmplitudeRmsLabel)
        Me.configurationGroupBox.Controls.Add(Me.numberOfStepsNumeric)
        Me.configurationGroupBox.Controls.Add(Me.startFrequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.endFrequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.numberOfStepsLabel)
        Me.configurationGroupBox.Controls.Add(Me.startFrequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.endFrequencyLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(15, 15)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(500, 405)
        Me.configurationGroupBox.TabIndex = 0
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'lcrShortCompensationEnabledCheckBox
        '
        Me.lcrShortCompensationEnabledCheckBox.AutoSize = True
        Me.lcrShortCompensationEnabledCheckBox.Location = New System.Drawing.Point(301, 318)
        Me.lcrShortCompensationEnabledCheckBox.Name = "lcrShortCompensationEnabledCheckBox"
        Me.lcrShortCompensationEnabledCheckBox.Size = New System.Drawing.Size(187, 17)
        Me.lcrShortCompensationEnabledCheckBox.TabIndex = 25
        Me.lcrShortCompensationEnabledCheckBox.Text = "LCR Short Compensation Enabled"
        Me.lcrShortCompensationEnabledCheckBox.UseVisualStyleBackColor = True
        '
        'lcrOpenCompensationEnabledCheckBox
        '
        Me.lcrOpenCompensationEnabledCheckBox.AutoSize = True
        Me.lcrOpenCompensationEnabledCheckBox.Location = New System.Drawing.Point(301, 295)
        Me.lcrOpenCompensationEnabledCheckBox.Name = "lcrOpenCompensationEnabledCheckBox"
        Me.lcrOpenCompensationEnabledCheckBox.Size = New System.Drawing.Size(188, 17)
        Me.lcrOpenCompensationEnabledCheckBox.TabIndex = 24
        Me.lcrOpenCompensationEnabledCheckBox.Text = "LCR Open Compensation Enabled"
        Me.lcrOpenCompensationEnabledCheckBox.UseVisualStyleBackColor = True
        '
        'lcrDcBiasCurrentLevelNumeric
        '
        Me.lcrDcBiasCurrentLevelNumeric.DecimalPlaces = 6
        Me.lcrDcBiasCurrentLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 393216})
        Me.lcrDcBiasCurrentLevelNumeric.Location = New System.Drawing.Point(301, 211)
        Me.lcrDcBiasCurrentLevelNumeric.Maximum = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.lcrDcBiasCurrentLevelNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, -2147418112})
        Me.lcrDcBiasCurrentLevelNumeric.Name = "lcrDcBiasCurrentLevelNumeric"
        Me.lcrDcBiasCurrentLevelNumeric.Size = New System.Drawing.Size(125, 20)
        Me.lcrDcBiasCurrentLevelNumeric.TabIndex = 19
        '
        'lcrDcBiasCurrentLevel
        '
        Me.lcrDcBiasCurrentLevel.AutoSize = True
        Me.lcrDcBiasCurrentLevel.Location = New System.Drawing.Point(301, 194)
        Me.lcrDcBiasCurrentLevel.Name = "lcrDcBiasCurrentLevel"
        Me.lcrDcBiasCurrentLevel.Size = New System.Drawing.Size(135, 13)
        Me.lcrDcBiasCurrentLevel.TabIndex = 18
        Me.lcrDcBiasCurrentLevel.Text = "LCR DC Bias Current Level"
        '
        'lcrCustomMeasurementTimeSecNumeric
        '
        Me.lcrCustomMeasurementTimeSecNumeric.DecimalPlaces = 6
        Me.lcrCustomMeasurementTimeSecNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.lcrCustomMeasurementTimeSecNumeric.Location = New System.Drawing.Point(301, 260)
        Me.lcrCustomMeasurementTimeSecNumeric.Maximum = New Decimal(New Integer() {99999, 0, 0, 327680})
        Me.lcrCustomMeasurementTimeSecNumeric.Name = "lcrCustomMeasurementTimeSecNumeric"
        Me.lcrCustomMeasurementTimeSecNumeric.Size = New System.Drawing.Size(125, 20)
        Me.lcrCustomMeasurementTimeSecNumeric.TabIndex = 23
        '
        'lcrMeasurementTimeComboBox
        '
        Me.lcrMeasurementTimeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.lcrMeasurementTimeComboBox.FormattingEnabled = True
        Me.lcrMeasurementTimeComboBox.Location = New System.Drawing.Point(157, 211)
        Me.lcrMeasurementTimeComboBox.Name = "lcrMeasurementTimeComboBox"
        Me.lcrMeasurementTimeComboBox.Size = New System.Drawing.Size(125, 21)
        Me.lcrMeasurementTimeComboBox.TabIndex = 17
        '
        'cableLengthComboBox
        '
        Me.cableLengthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cableLengthComboBox.FormattingEnabled = True
        Me.cableLengthComboBox.Location = New System.Drawing.Point(157, 260)
        Me.cableLengthComboBox.Name = "cableLengthComboBox"
        Me.cableLengthComboBox.Size = New System.Drawing.Size(125, 21)
        Me.cableLengthComboBox.TabIndex = 21
        '
        'lcrDcBiasVoltageLevelNumeric
        '
        Me.lcrDcBiasVoltageLevelNumeric.DecimalPlaces = 6
        Me.lcrDcBiasVoltageLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.lcrDcBiasVoltageLevelNumeric.Location = New System.Drawing.Point(301, 161)
        Me.lcrDcBiasVoltageLevelNumeric.Maximum = New Decimal(New Integer() {40, 0, 0, 0})
        Me.lcrDcBiasVoltageLevelNumeric.Minimum = New Decimal(New Integer() {40, 0, 0, -2147483648})
        Me.lcrDcBiasVoltageLevelNumeric.Name = "lcrDcBiasVoltageLevelNumeric"
        Me.lcrDcBiasVoltageLevelNumeric.Size = New System.Drawing.Size(125, 20)
        Me.lcrDcBiasVoltageLevelNumeric.TabIndex = 13
        '
        'lcrDcBiasSourceComboBox
        '
        Me.lcrDcBiasSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.lcrDcBiasSourceComboBox.FormattingEnabled = True
        Me.lcrDcBiasSourceComboBox.Location = New System.Drawing.Point(301, 112)
        Me.lcrDcBiasSourceComboBox.Name = "lcrDcBiasSourceComboBox"
        Me.lcrDcBiasSourceComboBox.Size = New System.Drawing.Size(125, 21)
        Me.lcrDcBiasSourceComboBox.TabIndex = 7
        '
        'lcrImpedanceRangeNumeric
        '
        Me.lcrImpedanceRangeNumeric.DecimalPlaces = 6
        Me.lcrImpedanceRangeNumeric.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.lcrImpedanceRangeNumeric.Location = New System.Drawing.Point(157, 161)
        Me.lcrImpedanceRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.lcrImpedanceRangeNumeric.Name = "lcrImpedanceRangeNumeric"
        Me.lcrImpedanceRangeNumeric.Size = New System.Drawing.Size(125, 20)
        Me.lcrImpedanceRangeNumeric.TabIndex = 11
        Me.lcrImpedanceRangeNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'ImpedanceRangeLabel
        '
        Me.ImpedanceRangeLabel.AutoSize = True
        Me.ImpedanceRangeLabel.Location = New System.Drawing.Point(157, 144)
        Me.ImpedanceRangeLabel.Name = "ImpedanceRangeLabel"
        Me.ImpedanceRangeLabel.Size = New System.Drawing.Size(95, 13)
        Me.ImpedanceRangeLabel.TabIndex = 10
        Me.ImpedanceRangeLabel.Text = "Impedance Range"
        '
        'lcrCustomMeasurementTimeSecLabel
        '
        Me.lcrCustomMeasurementTimeSecLabel.AutoSize = True
        Me.lcrCustomMeasurementTimeSecLabel.Location = New System.Drawing.Point(301, 244)
        Me.lcrCustomMeasurementTimeSecLabel.Name = "lcrCustomMeasurementTimeSecLabel"
        Me.lcrCustomMeasurementTimeSecLabel.Size = New System.Drawing.Size(181, 13)
        Me.lcrCustomMeasurementTimeSecLabel.TabIndex = 22
        Me.lcrCustomMeasurementTimeSecLabel.Text = "LCR Custom Measurement Time Sec"
        '
        'lcrMeasurementTimeLabel
        '
        Me.lcrMeasurementTimeLabel.AutoSize = True
        Me.lcrMeasurementTimeLabel.Location = New System.Drawing.Point(157, 194)
        Me.lcrMeasurementTimeLabel.Name = "lcrMeasurementTimeLabel"
        Me.lcrMeasurementTimeLabel.Size = New System.Drawing.Size(121, 13)
        Me.lcrMeasurementTimeLabel.TabIndex = 16
        Me.lcrMeasurementTimeLabel.Text = "LCR Measurement Time"
        '
        'cableLengthLabel
        '
        Me.cableLengthLabel.AutoSize = True
        Me.cableLengthLabel.Location = New System.Drawing.Point(157, 244)
        Me.cableLengthLabel.Name = "cableLengthLabel"
        Me.cableLengthLabel.Size = New System.Drawing.Size(70, 13)
        Me.cableLengthLabel.TabIndex = 20
        Me.cableLengthLabel.Text = "Cable Length"
        '
        'lcrDcBiasVoltageLevelLabel
        '
        Me.lcrDcBiasVoltageLevelLabel.AutoSize = True
        Me.lcrDcBiasVoltageLevelLabel.Location = New System.Drawing.Point(301, 144)
        Me.lcrDcBiasVoltageLevelLabel.Name = "lcrDcBiasVoltageLevelLabel"
        Me.lcrDcBiasVoltageLevelLabel.Size = New System.Drawing.Size(137, 13)
        Me.lcrDcBiasVoltageLevelLabel.TabIndex = 12
        Me.lcrDcBiasVoltageLevelLabel.Text = "LCR DC Bias Voltage Level"
        '
        'lcrDcBiasSourceLabel
        '
        Me.lcrDcBiasSourceLabel.AutoSize = True
        Me.lcrDcBiasSourceLabel.Location = New System.Drawing.Point(301, 95)
        Me.lcrDcBiasSourceLabel.Name = "lcrDcBiasSourceLabel"
        Me.lcrDcBiasSourceLabel.Size = New System.Drawing.Size(106, 13)
        Me.lcrDcBiasSourceLabel.TabIndex = 6
        Me.lcrDcBiasSourceLabel.Text = "LCD DC Bias Source"
        '
        'VoltageAmplitudeRmsNumeric
        '
        Me.VoltageAmplitudeRmsNumeric.DecimalPlaces = 6
        Me.VoltageAmplitudeRmsNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.VoltageAmplitudeRmsNumeric.Location = New System.Drawing.Point(157, 112)
        Me.VoltageAmplitudeRmsNumeric.Maximum = New Decimal(New Integer() {28284272, 0, 0, 393216})
        Me.VoltageAmplitudeRmsNumeric.Minimum = New Decimal(New Integer() {28284272, 0, 0, -2147090432})
        Me.VoltageAmplitudeRmsNumeric.Name = "VoltageAmplitudeRmsNumeric"
        Me.VoltageAmplitudeRmsNumeric.Size = New System.Drawing.Size(125, 20)
        Me.VoltageAmplitudeRmsNumeric.TabIndex = 5
        Me.VoltageAmplitudeRmsNumeric.Value = New Decimal(New Integer() {7, 0, 0, 65536})
        '
        'voltageAmplitudeRmsLabel
        '
        Me.voltageAmplitudeRmsLabel.AutoSize = True
        Me.voltageAmplitudeRmsLabel.Location = New System.Drawing.Point(157, 95)
        Me.voltageAmplitudeRmsLabel.Name = "voltageAmplitudeRmsLabel"
        Me.voltageAmplitudeRmsLabel.Size = New System.Drawing.Size(119, 13)
        Me.voltageAmplitudeRmsLabel.TabIndex = 4
        Me.voltageAmplitudeRmsLabel.Text = "Voltage Amplitude RMS"
        '
        'numberOfStepsNumeric
        '
        Me.numberOfStepsNumeric.Location = New System.Drawing.Point(13, 211)
        Me.numberOfStepsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numberOfStepsNumeric.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.numberOfStepsNumeric.Name = "numberOfStepsNumeric"
        Me.numberOfStepsNumeric.Size = New System.Drawing.Size(125, 20)
        Me.numberOfStepsNumeric.TabIndex = 15
        Me.numberOfStepsNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'startFrequencyNumeric
        '
        Me.startFrequencyNumeric.DecimalPlaces = 6
        Me.startFrequencyNumeric.Increment = New Decimal(New Integer() {100, 0, 0, 0})
        Me.startFrequencyNumeric.Location = New System.Drawing.Point(13, 112)
        Me.startFrequencyNumeric.Maximum = New Decimal(New Integer() {20000000, 0, 0, 0})
        Me.startFrequencyNumeric.Minimum = New Decimal(New Integer() {40, 0, 0, 0})
        Me.startFrequencyNumeric.Name = "startFrequencyNumeric"
        Me.startFrequencyNumeric.Size = New System.Drawing.Size(125, 20)
        Me.startFrequencyNumeric.TabIndex = 3
        Me.startFrequencyNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'endFrequencyNumeric
        '
        Me.endFrequencyNumeric.DecimalPlaces = 6
        Me.endFrequencyNumeric.Increment = New Decimal(New Integer() {100, 0, 0, 0})
        Me.endFrequencyNumeric.Location = New System.Drawing.Point(13, 161)
        Me.endFrequencyNumeric.Maximum = New Decimal(New Integer() {20000000, 0, 0, 0})
        Me.endFrequencyNumeric.Minimum = New Decimal(New Integer() {40, 0, 0, 0})
        Me.endFrequencyNumeric.Name = "endFrequencyNumeric"
        Me.endFrequencyNumeric.Size = New System.Drawing.Size(125, 20)
        Me.endFrequencyNumeric.TabIndex = 9
        Me.endFrequencyNumeric.Value = New Decimal(New Integer() {500000, 0, 0, 0})
        '
        'numberOfStepsLabel
        '
        Me.numberOfStepsLabel.AutoSize = True
        Me.numberOfStepsLabel.Location = New System.Drawing.Point(13, 194)
        Me.numberOfStepsLabel.Name = "numberOfStepsLabel"
        Me.numberOfStepsLabel.Size = New System.Drawing.Size(88, 13)
        Me.numberOfStepsLabel.TabIndex = 14
        Me.numberOfStepsLabel.Text = "Number Of Steps"
        '
        'startFrequencyLabel
        '
        Me.startFrequencyLabel.AutoSize = True
        Me.startFrequencyLabel.Location = New System.Drawing.Point(13, 95)
        Me.startFrequencyLabel.Name = "startFrequencyLabel"
        Me.startFrequencyLabel.Size = New System.Drawing.Size(82, 13)
        Me.startFrequencyLabel.TabIndex = 2
        Me.startFrequencyLabel.Text = "Start Frequency"
        '
        'endFrequencyLabel
        '
        Me.endFrequencyLabel.AutoSize = True
        Me.endFrequencyLabel.Location = New System.Drawing.Point(13, 144)
        Me.endFrequencyLabel.Name = "endFrequencyLabel"
        Me.endFrequencyLabel.Size = New System.Drawing.Size(79, 13)
        Me.endFrequencyLabel.TabIndex = 8
        Me.endFrequencyLabel.Text = "End Frequency"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(392, 425)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 2
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(822, 453)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.measurementsGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "LCR Advanced Sequence Frequency Sweep"
        CType(Me.measurementsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementsGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.lcrDcBiasCurrentLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lcrCustomMeasurementTimeSecNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lcrDcBiasVoltageLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lcrImpedanceRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VoltageAmplitudeRmsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberOfStepsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.endFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private resourcenameLabel As System.Windows.Forms.Label
    Private measurementsDataGridView As System.Windows.Forms.DataGridView
    Private measurementsGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents configurationGroupBox As Windows.Forms.GroupBox
    Private WithEvents lcrShortCompensationEnabledCheckBox As Windows.Forms.CheckBox
    Private WithEvents lcrOpenCompensationEnabledCheckBox As Windows.Forms.CheckBox
    Private WithEvents lcrDcBiasCurrentLevelNumeric As Windows.Forms.NumericUpDown
    Private WithEvents lcrDcBiasCurrentLevel As Windows.Forms.Label
    Private WithEvents lcrCustomMeasurementTimeSecNumeric As Windows.Forms.NumericUpDown
    Private WithEvents lcrMeasurementTimeComboBox As Windows.Forms.ComboBox
    Private WithEvents cableLengthComboBox As Windows.Forms.ComboBox
    Private WithEvents lcrDcBiasVoltageLevelNumeric As Windows.Forms.NumericUpDown
    Private WithEvents lcrDcBiasSourceComboBox As Windows.Forms.ComboBox
    Private WithEvents lcrImpedanceRangeNumeric As Windows.Forms.NumericUpDown
    Private WithEvents ImpedanceRangeLabel As Windows.Forms.Label
    Private WithEvents lcrCustomMeasurementTimeSecLabel As Windows.Forms.Label
    Private WithEvents lcrMeasurementTimeLabel As Windows.Forms.Label
    Private WithEvents cableLengthLabel As Windows.Forms.Label
    Private WithEvents lcrDcBiasVoltageLevelLabel As Windows.Forms.Label
    Private WithEvents lcrDcBiasSourceLabel As Windows.Forms.Label
    Private WithEvents VoltageAmplitudeRmsNumeric As Windows.Forms.NumericUpDown
    Private WithEvents voltageAmplitudeRmsLabel As Windows.Forms.Label
    Private WithEvents numberOfStepsNumeric As Windows.Forms.NumericUpDown
    Private WithEvents startFrequencyNumeric As Windows.Forms.NumericUpDown
    Private WithEvents endFrequencyNumeric As Windows.Forms.NumericUpDown
    Private WithEvents numberOfStepsLabel As Windows.Forms.Label
    Private WithEvents startFrequencyLabel As Windows.Forms.Label
    Private WithEvents endFrequencyLabel As Windows.Forms.Label
    Friend WithEvents startButton As Windows.Forms.Button
    Friend WithEvents resourceNameComboBox As Windows.Forms.ComboBox
    Friend WithEvents Point As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Frequency As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Impedance As Windows.Forms.DataGridViewTextBoxColumn
End Class
