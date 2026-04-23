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
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.scalarMeasurementComboBox = New System.Windows.Forms.ComboBox()
        Me.scalarMeasurementLabel = New System.Windows.Forms.Label()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.referenceLevelGroupBox = New System.Windows.Forms.GroupBox()
        Me.highReferenceNumeric = New System.Windows.Forms.NumericUpDown()
        Me.middleReferenceNumeric = New System.Windows.Forms.NumericUpDown()
        Me.lowReferenceNumeric = New System.Windows.Forms.NumericUpDown()
        Me.highReferenceLabel = New System.Windows.Forms.Label()
        Me.middleReferenceLabel = New System.Windows.Forms.Label()
        Me.lowReferenceLabel = New System.Windows.Forms.Label()
        Me.edgeTriggerGroupBox = New System.Windows.Forms.GroupBox()
        Me.triggerSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.triggerCouplingLabel = New System.Windows.Forms.Label()
        Me.triggerCouplingComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerSlopeLabel = New System.Windows.Forms.Label()
        Me.triggerSlopeComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerLevelLabel = New System.Windows.Forms.Label()
        Me.triggerSourceLabel = New System.Windows.Forms.Label()
        Me.triggeringGroupBox = New System.Windows.Forms.GroupBox()
        Me.maxTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.maximumTimeLabel = New System.Windows.Forms.Label()
        Me.triggerTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerTypeLabel = New System.Windows.Forms.Label()
        Me.timingGroupBox = New System.Windows.Forms.GroupBox()
        Me.sampleRateMinNumeric = New System.Windows.Forms.NumericUpDown()
        Me.minRecordLengthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.enforceRealTimeCheckBox = New System.Windows.Forms.CheckBox()
        Me.recordLengthMinLabel = New System.Windows.Forms.Label()
        Me.sampleRateMinLabel = New System.Windows.Forms.Label()
        Me.generalGroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.verticalRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.verticalRangeLabel = New System.Windows.Forms.Label()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.scanResultLabel = New System.Windows.Forms.Label()
        Me.meanLabel = New System.Windows.Forms.Label()
        Me.standardDeviationLabel = New System.Windows.Forms.Label()
        Me.minimumLabel = New System.Windows.Forms.Label()
        Me.maximumLabel = New System.Windows.Forms.Label()
        Me.numberInStatsLabel = New System.Windows.Forms.Label()
        Me.clearStatisticsButton = New System.Windows.Forms.Button()
        Me.scalarResultTextBox = New System.Windows.Forms.TextBox()
        Me.meanTextBox = New System.Windows.Forms.TextBox()
        Me.standardDeviationTextBox = New System.Windows.Forms.TextBox()
        Me.maximumTextBox = New System.Windows.Forms.TextBox()
        Me.minimumTextBox = New System.Windows.Forms.TextBox()
        Me.numberInStatsTextBox = New System.Windows.Forms.TextBox()
        Me.measurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.measurementGroupBox.SuspendLayout()
        Me.referenceLevelGroupBox.SuspendLayout()
        CType(Me.highReferenceNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.middleReferenceNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lowReferenceNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.edgeTriggerGroupBox.SuspendLayout()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.triggeringGroupBox.SuspendLayout()
        CType(Me.maxTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingGroupBox.SuspendLayout()
        CType(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.minRecordLengthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.generalGroupBox.SuspendLayout()
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementsGroupBox.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.scalarMeasurementComboBox)
        Me.measurementGroupBox.Controls.Add(Me.scalarMeasurementLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(260, 12)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(247, 66)
        Me.measurementGroupBox.TabIndex = 4
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Waveform Measurement"
        '
        'scalarMeasurementComboBox
        '
        Me.scalarMeasurementComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.scalarMeasurementComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.scalarMeasurementComboBox.FormattingEnabled = True
        Me.scalarMeasurementComboBox.Location = New System.Drawing.Point(6, 38)
        Me.scalarMeasurementComboBox.Name = "scalarMeasurementComboBox"
        Me.scalarMeasurementComboBox.Size = New System.Drawing.Size(235, 21)
        Me.scalarMeasurementComboBox.TabIndex = 1
        '
        'scalarMeasurementLabel
        '
        Me.scalarMeasurementLabel.AutoSize = True
        Me.scalarMeasurementLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.scalarMeasurementLabel.Location = New System.Drawing.Point(6, 22)
        Me.scalarMeasurementLabel.Name = "scalarMeasurementLabel"
        Me.scalarMeasurementLabel.Size = New System.Drawing.Size(107, 13)
        Me.scalarMeasurementLabel.TabIndex = 0
        Me.scalarMeasurementLabel.Text = "Scalar Measurement:"
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(47, 19)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(75, 23)
        Me.acquireButton.TabIndex = 0
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(128, 19)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'referenceLevelGroupBox
        '
        Me.referenceLevelGroupBox.Controls.Add(Me.highReferenceNumeric)
        Me.referenceLevelGroupBox.Controls.Add(Me.middleReferenceNumeric)
        Me.referenceLevelGroupBox.Controls.Add(Me.lowReferenceNumeric)
        Me.referenceLevelGroupBox.Controls.Add(Me.highReferenceLabel)
        Me.referenceLevelGroupBox.Controls.Add(Me.middleReferenceLabel)
        Me.referenceLevelGroupBox.Controls.Add(Me.lowReferenceLabel)
        Me.referenceLevelGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.referenceLevelGroupBox.Location = New System.Drawing.Point(260, 89)
        Me.referenceLevelGroupBox.Name = "referenceLevelGroupBox"
        Me.referenceLevelGroupBox.Size = New System.Drawing.Size(247, 97)
        Me.referenceLevelGroupBox.TabIndex = 5
        Me.referenceLevelGroupBox.TabStop = False
        Me.referenceLevelGroupBox.Text = "Reference Level Percentage"
        '
        'highReferenceNumeric
        '
        Me.highReferenceNumeric.DecimalPlaces = 2
        Me.highReferenceNumeric.Location = New System.Drawing.Point(128, 71)
        Me.highReferenceNumeric.Name = "highReferenceNumeric"
        Me.highReferenceNumeric.Size = New System.Drawing.Size(113, 20)
        Me.highReferenceNumeric.TabIndex = 5
        Me.highReferenceNumeric.Value = New Decimal(New Integer() {90, 0, 0, 0})
        '
        'middleReferenceNumeric
        '
        Me.middleReferenceNumeric.DecimalPlaces = 2
        Me.middleReferenceNumeric.Location = New System.Drawing.Point(128, 45)
        Me.middleReferenceNumeric.Name = "middleReferenceNumeric"
        Me.middleReferenceNumeric.Size = New System.Drawing.Size(113, 20)
        Me.middleReferenceNumeric.TabIndex = 3
        Me.middleReferenceNumeric.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'lowReferenceNumeric
        '
        Me.lowReferenceNumeric.DecimalPlaces = 2
        Me.lowReferenceNumeric.Location = New System.Drawing.Point(128, 19)
        Me.lowReferenceNumeric.Name = "lowReferenceNumeric"
        Me.lowReferenceNumeric.Size = New System.Drawing.Size(113, 20)
        Me.lowReferenceNumeric.TabIndex = 1
        Me.lowReferenceNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'highReferenceLabel
        '
        Me.highReferenceLabel.AutoSize = True
        Me.highReferenceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.highReferenceLabel.Location = New System.Drawing.Point(6, 75)
        Me.highReferenceLabel.Name = "highReferenceLabel"
        Me.highReferenceLabel.Size = New System.Drawing.Size(85, 13)
        Me.highReferenceLabel.TabIndex = 4
        Me.highReferenceLabel.Text = "High Reference:"
        '
        'middleReferenceLabel
        '
        Me.middleReferenceLabel.AutoSize = True
        Me.middleReferenceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.middleReferenceLabel.Location = New System.Drawing.Point(6, 49)
        Me.middleReferenceLabel.Name = "middleReferenceLabel"
        Me.middleReferenceLabel.Size = New System.Drawing.Size(94, 13)
        Me.middleReferenceLabel.TabIndex = 2
        Me.middleReferenceLabel.Text = "Middle Reference:"
        '
        'lowReferenceLabel
        '
        Me.lowReferenceLabel.AutoSize = True
        Me.lowReferenceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lowReferenceLabel.Location = New System.Drawing.Point(6, 21)
        Me.lowReferenceLabel.Name = "lowReferenceLabel"
        Me.lowReferenceLabel.Size = New System.Drawing.Size(83, 13)
        Me.lowReferenceLabel.TabIndex = 0
        Me.lowReferenceLabel.Text = "Low Reference:"
        '
        'edgeTriggerGroupBox
        '
        Me.edgeTriggerGroupBox.Controls.Add(Me.triggerSourceComboBox)
        Me.edgeTriggerGroupBox.Controls.Add(Me.triggerLevelNumeric)
        Me.edgeTriggerGroupBox.Controls.Add(Me.triggerCouplingLabel)
        Me.edgeTriggerGroupBox.Controls.Add(Me.triggerCouplingComboBox)
        Me.edgeTriggerGroupBox.Controls.Add(Me.triggerSlopeLabel)
        Me.edgeTriggerGroupBox.Controls.Add(Me.triggerSlopeComboBox)
        Me.edgeTriggerGroupBox.Controls.Add(Me.triggerLevelLabel)
        Me.edgeTriggerGroupBox.Controls.Add(Me.triggerSourceLabel)
        Me.edgeTriggerGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.edgeTriggerGroupBox.Location = New System.Drawing.Point(12, 312)
        Me.edgeTriggerGroupBox.Name = "edgeTriggerGroupBox"
        Me.edgeTriggerGroupBox.Size = New System.Drawing.Size(242, 128)
        Me.edgeTriggerGroupBox.TabIndex = 3
        Me.edgeTriggerGroupBox.TabStop = False
        Me.edgeTriggerGroupBox.Text = "Edge Trigger Parameters"
        '
        'triggerSourceComboBox
        '
        Me.triggerSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerSourceComboBox.Location = New System.Drawing.Point(96, 19)
        Me.triggerSourceComboBox.Name = "triggerSourceComboBox"
        Me.triggerSourceComboBox.Size = New System.Drawing.Size(140, 21)
        Me.triggerSourceComboBox.TabIndex = 1
        '
        'triggerLevelNumeric
        '
        Me.triggerLevelNumeric.DecimalPlaces = 2
        Me.triggerLevelNumeric.Location = New System.Drawing.Point(96, 46)
        Me.triggerLevelNumeric.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.triggerLevelNumeric.Name = "triggerLevelNumeric"
        Me.triggerLevelNumeric.Size = New System.Drawing.Size(140, 20)
        Me.triggerLevelNumeric.TabIndex = 3
        '
        'triggerCouplingLabel
        '
        Me.triggerCouplingLabel.AutoSize = True
        Me.triggerCouplingLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerCouplingLabel.Location = New System.Drawing.Point(6, 103)
        Me.triggerCouplingLabel.Name = "triggerCouplingLabel"
        Me.triggerCouplingLabel.Size = New System.Drawing.Size(87, 13)
        Me.triggerCouplingLabel.TabIndex = 6
        Me.triggerCouplingLabel.Text = "Trigger Coupling:"
        '
        'triggerCouplingComboBox
        '
        Me.triggerCouplingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerCouplingComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerCouplingComboBox.FormattingEnabled = True
        Me.triggerCouplingComboBox.Location = New System.Drawing.Point(96, 99)
        Me.triggerCouplingComboBox.Name = "triggerCouplingComboBox"
        Me.triggerCouplingComboBox.Size = New System.Drawing.Size(140, 21)
        Me.triggerCouplingComboBox.TabIndex = 7
        '
        'triggerSlopeLabel
        '
        Me.triggerSlopeLabel.AutoSize = True
        Me.triggerSlopeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSlopeLabel.Location = New System.Drawing.Point(6, 76)
        Me.triggerSlopeLabel.Name = "triggerSlopeLabel"
        Me.triggerSlopeLabel.Size = New System.Drawing.Size(73, 13)
        Me.triggerSlopeLabel.TabIndex = 4
        Me.triggerSlopeLabel.Text = "Trigger Slope:"
        '
        'triggerSlopeComboBox
        '
        Me.triggerSlopeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerSlopeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSlopeComboBox.FormattingEnabled = True
        Me.triggerSlopeComboBox.Location = New System.Drawing.Point(96, 72)
        Me.triggerSlopeComboBox.Name = "triggerSlopeComboBox"
        Me.triggerSlopeComboBox.Size = New System.Drawing.Size(140, 21)
        Me.triggerSlopeComboBox.TabIndex = 5
        '
        'triggerLevelLabel
        '
        Me.triggerLevelLabel.AutoSize = True
        Me.triggerLevelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerLevelLabel.Location = New System.Drawing.Point(6, 50)
        Me.triggerLevelLabel.Name = "triggerLevelLabel"
        Me.triggerLevelLabel.Size = New System.Drawing.Size(72, 13)
        Me.triggerLevelLabel.TabIndex = 2
        Me.triggerLevelLabel.Text = "Trigger Level:"
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.AutoSize = True
        Me.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerSourceLabel.Location = New System.Drawing.Point(6, 23)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(80, 13)
        Me.triggerSourceLabel.TabIndex = 0
        Me.triggerSourceLabel.Text = "Trigger Source:"
        '
        'triggeringGroupBox
        '
        Me.triggeringGroupBox.Controls.Add(Me.maxTimeNumeric)
        Me.triggeringGroupBox.Controls.Add(Me.maximumTimeLabel)
        Me.triggeringGroupBox.Controls.Add(Me.triggerTypeComboBox)
        Me.triggeringGroupBox.Controls.Add(Me.triggerTypeLabel)
        Me.triggeringGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggeringGroupBox.Location = New System.Drawing.Point(12, 226)
        Me.triggeringGroupBox.Name = "triggeringGroupBox"
        Me.triggeringGroupBox.Size = New System.Drawing.Size(242, 75)
        Me.triggeringGroupBox.TabIndex = 2
        Me.triggeringGroupBox.TabStop = False
        Me.triggeringGroupBox.Text = "Triggering"
        '
        'maxTimeNumeric
        '
        Me.maxTimeNumeric.DecimalPlaces = 2
        Me.maxTimeNumeric.Location = New System.Drawing.Point(136, 46)
        Me.maxTimeNumeric.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.maxTimeNumeric.Name = "maxTimeNumeric"
        Me.maxTimeNumeric.Size = New System.Drawing.Size(100, 20)
        Me.maxTimeNumeric.TabIndex = 3
        Me.maxTimeNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'maximumTimeLabel
        '
        Me.maximumTimeLabel.AutoSize = True
        Me.maximumTimeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumTimeLabel.Location = New System.Drawing.Point(6, 50)
        Me.maximumTimeLabel.Name = "maximumTimeLabel"
        Me.maximumTimeLabel.Size = New System.Drawing.Size(62, 13)
        Me.maximumTimeLabel.TabIndex = 2
        Me.maximumTimeLabel.Text = "Timeout (s):"
        '
        'triggerTypeComboBox
        '
        Me.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerTypeComboBox.FormattingEnabled = True
        Me.triggerTypeComboBox.Location = New System.Drawing.Point(136, 19)
        Me.triggerTypeComboBox.Name = "triggerTypeComboBox"
        Me.triggerTypeComboBox.Size = New System.Drawing.Size(100, 21)
        Me.triggerTypeComboBox.TabIndex = 1
        '
        'triggerTypeLabel
        '
        Me.triggerTypeLabel.AutoSize = True
        Me.triggerTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerTypeLabel.Location = New System.Drawing.Point(6, 23)
        Me.triggerTypeLabel.Name = "triggerTypeLabel"
        Me.triggerTypeLabel.Size = New System.Drawing.Size(70, 13)
        Me.triggerTypeLabel.TabIndex = 0
        Me.triggerTypeLabel.Text = "Trigger Type:"
        '
        'timingGroupBox
        '
        Me.timingGroupBox.Controls.Add(Me.sampleRateMinNumeric)
        Me.timingGroupBox.Controls.Add(Me.minRecordLengthNumeric)
        Me.timingGroupBox.Controls.Add(Me.enforceRealTimeCheckBox)
        Me.timingGroupBox.Controls.Add(Me.recordLengthMinLabel)
        Me.timingGroupBox.Controls.Add(Me.sampleRateMinLabel)
        Me.timingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timingGroupBox.Location = New System.Drawing.Point(12, 123)
        Me.timingGroupBox.Name = "timingGroupBox"
        Me.timingGroupBox.Size = New System.Drawing.Size(242, 97)
        Me.timingGroupBox.TabIndex = 1
        Me.timingGroupBox.TabStop = False
        Me.timingGroupBox.Text = "Timing"
        '
        'sampleRateMinNumeric
        '
        Me.sampleRateMinNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.sampleRateMinNumeric.Location = New System.Drawing.Point(136, 43)
        Me.sampleRateMinNumeric.Maximum = New Decimal(New Integer() {1215752192, 23, 0, 0})
        Me.sampleRateMinNumeric.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.sampleRateMinNumeric.Name = "sampleRateMinNumeric"
        Me.sampleRateMinNumeric.Size = New System.Drawing.Size(100, 20)
        Me.sampleRateMinNumeric.TabIndex = 2
        Me.sampleRateMinNumeric.Value = New Decimal(New Integer() {10000000, 0, 0, 0})
        '
        'minRecordLengthNumeric
        '
        Me.minRecordLengthNumeric.Location = New System.Drawing.Point(136, 69)
        Me.minRecordLengthNumeric.Maximum = New Decimal(New Integer() {1215752192, 23, 0, 0})
        Me.minRecordLengthNumeric.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.minRecordLengthNumeric.Name = "minRecordLengthNumeric"
        Me.minRecordLengthNumeric.Size = New System.Drawing.Size(100, 20)
        Me.minRecordLengthNumeric.TabIndex = 4
        Me.minRecordLengthNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'enforceRealTimeCheckBox
        '
        Me.enforceRealTimeCheckBox.AutoSize = True
        Me.enforceRealTimeCheckBox.Checked = True
        Me.enforceRealTimeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.enforceRealTimeCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.enforceRealTimeCheckBox.Location = New System.Drawing.Point(6, 19)
        Me.enforceRealTimeCheckBox.Name = "enforceRealTimeCheckBox"
        Me.enforceRealTimeCheckBox.Size = New System.Drawing.Size(120, 18)
        Me.enforceRealTimeCheckBox.TabIndex = 0
        Me.enforceRealTimeCheckBox.Text = "&Enforce Real Time"
        Me.enforceRealTimeCheckBox.UseVisualStyleBackColor = True
        '
        'recordLengthMinLabel
        '
        Me.recordLengthMinLabel.AutoSize = True
        Me.recordLengthMinLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.recordLengthMinLabel.Location = New System.Drawing.Point(6, 73)
        Me.recordLengthMinLabel.Name = "recordLengthMinLabel"
        Me.recordLengthMinLabel.Size = New System.Drawing.Size(104, 13)
        Me.recordLengthMinLabel.TabIndex = 3
        Me.recordLengthMinLabel.Text = "Min. Record Length:"
        '
        'sampleRateMinLabel
        '
        Me.sampleRateMinLabel.AutoSize = True
        Me.sampleRateMinLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sampleRateMinLabel.Location = New System.Drawing.Point(6, 47)
        Me.sampleRateMinLabel.Name = "sampleRateMinLabel"
        Me.sampleRateMinLabel.Size = New System.Drawing.Size(116, 13)
        Me.sampleRateMinLabel.TabIndex = 1
        Me.sampleRateMinLabel.Text = "Min. Sample Rate (Hz):"
        '
        'generalGroupBox
        '
        Me.generalGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.generalGroupBox.Controls.Add(Me.verticalRangeNumeric)
        Me.generalGroupBox.Controls.Add(Me.verticalRangeLabel)
        Me.generalGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.generalGroupBox.Controls.Add(Me.channelNameLabel)
        Me.generalGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.generalGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.generalGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.generalGroupBox.Name = "generalGroupBox"
        Me.generalGroupBox.Size = New System.Drawing.Size(242, 100)
        Me.generalGroupBox.TabIndex = 0
        Me.generalGroupBox.TabStop = False
        Me.generalGroupBox.Text = "General"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(136, 19)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(100, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'verticalRangeNumeric
        '
        Me.verticalRangeNumeric.DecimalPlaces = 2
        Me.verticalRangeNumeric.Location = New System.Drawing.Point(136, 72)
        Me.verticalRangeNumeric.Name = "verticalRangeNumeric"
        Me.verticalRangeNumeric.Size = New System.Drawing.Size(100, 20)
        Me.verticalRangeNumeric.TabIndex = 5
        Me.verticalRangeNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'verticalRangeLabel
        '
        Me.verticalRangeLabel.AutoSize = True
        Me.verticalRangeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.verticalRangeLabel.Location = New System.Drawing.Point(6, 76)
        Me.verticalRangeLabel.Name = "verticalRangeLabel"
        Me.verticalRangeLabel.Size = New System.Drawing.Size(80, 13)
        Me.verticalRangeLabel.TabIndex = 4
        Me.verticalRangeLabel.Text = "Vertical Range:"
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(136, 46)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(100, 20)
        Me.channelNameTextBox.TabIndex = 3
        Me.channelNameTextBox.Text = "0"
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelNameLabel.Location = New System.Drawing.Point(6, 50)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(80, 13)
        Me.channelNameLabel.TabIndex = 2
        Me.channelNameLabel.Text = "Channel Name:"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 23)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'scanResultLabel
        '
        Me.scanResultLabel.AutoSize = True
        Me.scanResultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.scanResultLabel.Location = New System.Drawing.Point(6, 23)
        Me.scanResultLabel.Name = "scanResultLabel"
        Me.scanResultLabel.Size = New System.Drawing.Size(73, 13)
        Me.scanResultLabel.TabIndex = 0
        Me.scanResultLabel.Text = "Scalar Result:"
        '
        'meanLabel
        '
        Me.meanLabel.AutoSize = True
        Me.meanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.meanLabel.Location = New System.Drawing.Point(6, 49)
        Me.meanLabel.Name = "meanLabel"
        Me.meanLabel.Size = New System.Drawing.Size(37, 13)
        Me.meanLabel.TabIndex = 2
        Me.meanLabel.Text = "Mean:"
        '
        'standardDeviationLabel
        '
        Me.standardDeviationLabel.AutoSize = True
        Me.standardDeviationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.standardDeviationLabel.Location = New System.Drawing.Point(6, 75)
        Me.standardDeviationLabel.Name = "standardDeviationLabel"
        Me.standardDeviationLabel.Size = New System.Drawing.Size(101, 13)
        Me.standardDeviationLabel.TabIndex = 4
        Me.standardDeviationLabel.Text = "Standard Deviation:"
        '
        'minimumLabel
        '
        Me.minimumLabel.AutoSize = True
        Me.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.minimumLabel.Location = New System.Drawing.Point(6, 101)
        Me.minimumLabel.Name = "minimumLabel"
        Me.minimumLabel.Size = New System.Drawing.Size(51, 13)
        Me.minimumLabel.TabIndex = 6
        Me.minimumLabel.Text = "Minimum:"
        '
        'maximumLabel
        '
        Me.maximumLabel.AutoSize = True
        Me.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.maximumLabel.Location = New System.Drawing.Point(6, 127)
        Me.maximumLabel.Name = "maximumLabel"
        Me.maximumLabel.Size = New System.Drawing.Size(54, 13)
        Me.maximumLabel.TabIndex = 8
        Me.maximumLabel.Text = "Maximum:"
        '
        'numberInStatsLabel
        '
        Me.numberInStatsLabel.AutoSize = True
        Me.numberInStatsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.numberInStatsLabel.Location = New System.Drawing.Point(6, 153)
        Me.numberInStatsLabel.Name = "numberInStatsLabel"
        Me.numberInStatsLabel.Size = New System.Drawing.Size(103, 13)
        Me.numberInStatsLabel.TabIndex = 10
        Me.numberInStatsLabel.Text = "Number in Statistics:"
        '
        'clearStatisticsButton
        '
        Me.clearStatisticsButton.Enabled = False
        Me.clearStatisticsButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.clearStatisticsButton.Location = New System.Drawing.Point(80, 179)
        Me.clearStatisticsButton.Name = "clearStatisticsButton"
        Me.clearStatisticsButton.Size = New System.Drawing.Size(97, 23)
        Me.clearStatisticsButton.TabIndex = 12
        Me.clearStatisticsButton.Text = "&Clear Statistics"
        Me.clearStatisticsButton.UseVisualStyleBackColor = True
        '
        'scalarResultTextBox
        '
        Me.scalarResultTextBox.Location = New System.Drawing.Point(128, 19)
        Me.scalarResultTextBox.Name = "scalarResultTextBox"
        Me.scalarResultTextBox.ReadOnly = True
        Me.scalarResultTextBox.Size = New System.Drawing.Size(113, 20)
        Me.scalarResultTextBox.TabIndex = 1
        '
        'meanTextBox
        '
        Me.meanTextBox.Location = New System.Drawing.Point(129, 45)
        Me.meanTextBox.Name = "meanTextBox"
        Me.meanTextBox.ReadOnly = True
        Me.meanTextBox.Size = New System.Drawing.Size(113, 20)
        Me.meanTextBox.TabIndex = 3
        '
        'standardDeviationTextBox
        '
        Me.standardDeviationTextBox.Location = New System.Drawing.Point(129, 71)
        Me.standardDeviationTextBox.Name = "standardDeviationTextBox"
        Me.standardDeviationTextBox.ReadOnly = True
        Me.standardDeviationTextBox.Size = New System.Drawing.Size(113, 20)
        Me.standardDeviationTextBox.TabIndex = 5
        '
        'maximumTextBox
        '
        Me.maximumTextBox.Location = New System.Drawing.Point(129, 123)
        Me.maximumTextBox.Name = "maximumTextBox"
        Me.maximumTextBox.ReadOnly = True
        Me.maximumTextBox.Size = New System.Drawing.Size(113, 20)
        Me.maximumTextBox.TabIndex = 9
        '
        'minimumTextBox
        '
        Me.minimumTextBox.Location = New System.Drawing.Point(129, 97)
        Me.minimumTextBox.Name = "minimumTextBox"
        Me.minimumTextBox.ReadOnly = True
        Me.minimumTextBox.Size = New System.Drawing.Size(113, 20)
        Me.minimumTextBox.TabIndex = 7
        '
        'numberInStatsTextBox
        '
        Me.numberInStatsTextBox.Location = New System.Drawing.Point(129, 149)
        Me.numberInStatsTextBox.Name = "numberInStatsTextBox"
        Me.numberInStatsTextBox.ReadOnly = True
        Me.numberInStatsTextBox.Size = New System.Drawing.Size(113, 20)
        Me.numberInStatsTextBox.TabIndex = 11
        '
        'measurementsGroupBox
        '
        Me.measurementsGroupBox.Controls.Add(Me.numberInStatsTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.minimumTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.maximumTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.standardDeviationTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.meanTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.scalarResultTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.clearStatisticsButton)
        Me.measurementsGroupBox.Controls.Add(Me.numberInStatsLabel)
        Me.measurementsGroupBox.Controls.Add(Me.maximumLabel)
        Me.measurementsGroupBox.Controls.Add(Me.minimumLabel)
        Me.measurementsGroupBox.Controls.Add(Me.standardDeviationLabel)
        Me.measurementsGroupBox.Controls.Add(Me.meanLabel)
        Me.measurementsGroupBox.Controls.Add(Me.scanResultLabel)
        Me.measurementsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.measurementsGroupBox.Location = New System.Drawing.Point(260, 197)
        Me.measurementsGroupBox.Name = "measurementsGroupBox"
        Me.measurementsGroupBox.Size = New System.Drawing.Size(247, 208)
        Me.measurementsGroupBox.TabIndex = 6
        Me.measurementsGroupBox.TabStop = False
        Me.measurementsGroupBox.Text = "Measurements for the first channel"
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.acquireButton)
        Me.groupBox1.Controls.Add(Me.stopButton)
        Me.groupBox1.Location = New System.Drawing.Point(262, 411)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(247, 56)
        Me.groupBox1.TabIndex = 7
        Me.groupBox1.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(521, 477)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.referenceLevelGroupBox)
        Me.Controls.Add(Me.measurementsGroupBox)
        Me.Controls.Add(Me.edgeTriggerGroupBox)
        Me.Controls.Add(Me.triggeringGroupBox)
        Me.Controls.Add(Me.timingGroupBox)
        Me.Controls.Add(Me.generalGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Basic Measurement Library"
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.referenceLevelGroupBox.ResumeLayout(False)
        Me.referenceLevelGroupBox.PerformLayout()
        CType(Me.highReferenceNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.middleReferenceNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lowReferenceNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.edgeTriggerGroupBox.ResumeLayout(False)
        Me.edgeTriggerGroupBox.PerformLayout()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.triggeringGroupBox.ResumeLayout(False)
        Me.triggeringGroupBox.PerformLayout()
        CType(Me.maxTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingGroupBox.ResumeLayout(False)
        Me.timingGroupBox.PerformLayout()
        CType(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.minRecordLengthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.generalGroupBox.ResumeLayout(False)
        Me.generalGroupBox.PerformLayout()
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementsGroupBox.ResumeLayout(False)
        Me.measurementsGroupBox.PerformLayout()
        Me.groupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private measurementGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private scalarMeasurementComboBox As System.Windows.Forms.ComboBox
    Private scalarMeasurementLabel As System.Windows.Forms.Label
    Private referenceLevelGroupBox As System.Windows.Forms.GroupBox
    Private highReferenceNumeric As System.Windows.Forms.NumericUpDown
    Private middleReferenceNumeric As System.Windows.Forms.NumericUpDown
    Private lowReferenceNumeric As System.Windows.Forms.NumericUpDown
    Private highReferenceLabel As System.Windows.Forms.Label
    Private middleReferenceLabel As System.Windows.Forms.Label
    Private lowReferenceLabel As System.Windows.Forms.Label
    Private edgeTriggerGroupBox As System.Windows.Forms.GroupBox
    Private triggerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private triggerCouplingLabel As System.Windows.Forms.Label
    Private triggerCouplingComboBox As System.Windows.Forms.ComboBox
    Private triggerSlopeLabel As System.Windows.Forms.Label
    Private triggerSlopeComboBox As System.Windows.Forms.ComboBox
    Private triggerLevelLabel As System.Windows.Forms.Label
    Private triggerSourceLabel As System.Windows.Forms.Label
    Private triggeringGroupBox As System.Windows.Forms.GroupBox
    Private maxTimeNumeric As System.Windows.Forms.NumericUpDown
    Private maximumTimeLabel As System.Windows.Forms.Label
    Private triggerTypeComboBox As System.Windows.Forms.ComboBox
    Private triggerTypeLabel As System.Windows.Forms.Label
    Private timingGroupBox As System.Windows.Forms.GroupBox
    Private minRecordLengthNumeric As System.Windows.Forms.NumericUpDown
    Private enforceRealTimeCheckBox As System.Windows.Forms.CheckBox
    Private recordLengthMinLabel As System.Windows.Forms.Label
    Private sampleRateMinLabel As System.Windows.Forms.Label
    Private generalGroupBox As System.Windows.Forms.GroupBox
    Private verticalRangeNumeric As System.Windows.Forms.NumericUpDown
    Private verticalRangeLabel As System.Windows.Forms.Label
    Private channelNameTextBox As System.Windows.Forms.TextBox
    Private channelNameLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private triggerSourceComboBox As System.Windows.Forms.ComboBox
    Private scanResultLabel As System.Windows.Forms.Label
    Private meanLabel As System.Windows.Forms.Label
    Private standardDeviationLabel As System.Windows.Forms.Label
    Private minimumLabel As System.Windows.Forms.Label
    Private maximumLabel As System.Windows.Forms.Label
    Private numberInStatsLabel As System.Windows.Forms.Label
    Private WithEvents clearStatisticsButton As System.Windows.Forms.Button
    Private scalarResultTextBox As System.Windows.Forms.TextBox
    Private meanTextBox As System.Windows.Forms.TextBox
    Private standardDeviationTextBox As System.Windows.Forms.TextBox
    Private maximumTextBox As System.Windows.Forms.TextBox
    Private minimumTextBox As System.Windows.Forms.TextBox
    Private numberInStatsTextBox As System.Windows.Forms.TextBox
    Private measurementsGroupBox As System.Windows.Forms.GroupBox
    Private groupBox1 As System.Windows.Forms.GroupBox
    Private sampleRateMinNumeric As System.Windows.Forms.NumericUpDown
End Class
