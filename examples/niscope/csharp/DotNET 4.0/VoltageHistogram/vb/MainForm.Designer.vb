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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.scalarResult1Label = New System.Windows.Forms.Label()
        Me.meanLabel = New System.Windows.Forms.Label()
        Me.scalarResult2Label = New System.Windows.Forms.Label()
        Me.highVoltageLimitLabel = New System.Windows.Forms.Label()
        Me.lowVoltageLimitLabel = New System.Windows.Forms.Label()
        Me.histogramSizeLabel = New System.Windows.Forms.Label()
        Me.measurement2Label = New System.Windows.Forms.Label()
        Me.measurement1Label = New System.Windows.Forms.Label()
        Me.highVoltageLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.lowVoltageLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.histogramSizeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.scalarResult1TextBox = New System.Windows.Forms.TextBox()
        Me.meanTextBox = New System.Windows.Forms.TextBox()
        Me.scalarResult2TextBox = New System.Windows.Forms.TextBox()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.measurement2ComboBox = New System.Windows.Forms.ComboBox()
        Me.scalarMeasurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.clearStatsCheckBox = New System.Windows.Forms.CheckBox()
        Me.measurement1ComboBox = New System.Windows.Forms.ComboBox()
        Me.histogramParametersGroupBox = New System.Windows.Forms.GroupBox()
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox()
        Me.recordLengthMinNumeric = New System.Windows.Forms.NumericUpDown()
        Me.sampleRateMinNumeric = New System.Windows.Forms.NumericUpDown()
        Me.recordLengthMinLabel = New System.Windows.Forms.Label()
        Me.sampleRateMinLabel = New System.Windows.Forms.Label()
        Me.sampledDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.waveformFromScopeDataGridView = New System.Windows.Forms.DataGridView()
        Me.generalGroupBox = New System.Windows.Forms.GroupBox()
        Me.channelTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.RichTextBox()
        Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
        Me.voltageHistogramDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.voltageHistogramDataGridView = New System.Windows.Forms.DataGridView()
        CType(Me.highVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lowVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.histogramSizeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scalarMeasurementsGroupBox.SuspendLayout()
        Me.histogramParametersGroupBox.SuspendLayout()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.sampledDataGroupBox.SuspendLayout()
        CType(Me.waveformFromScopeDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.generalGroupBox.SuspendLayout()
        Me.messageGroupBox.SuspendLayout()
        Me.buttonsGroupBox.SuspendLayout()
        Me.voltageHistogramDataGroupBox.SuspendLayout()
        CType(Me.voltageHistogramDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'scalarResult1Label
        '
        Me.scalarResult1Label.AutoSize = True
        Me.scalarResult1Label.Location = New System.Drawing.Point(6, 50)
        Me.scalarResult1Label.Name = "scalarResult1Label"
        Me.scalarResult1Label.Size = New System.Drawing.Size(40, 13)
        Me.scalarResult1Label.TabIndex = 2
        Me.scalarResult1Label.Text = "Result:"
        '
        'meanLabel
        '
        Me.meanLabel.AutoSize = True
        Me.meanLabel.Location = New System.Drawing.Point(6, 76)
        Me.meanLabel.Name = "meanLabel"
        Me.meanLabel.Size = New System.Drawing.Size(37, 13)
        Me.meanLabel.TabIndex = 4
        Me.meanLabel.Text = "Mean:"
        '
        'scalarResult2Label
        '
        Me.scalarResult2Label.AutoSize = True
        Me.scalarResult2Label.BackColor = System.Drawing.SystemColors.Control
        Me.scalarResult2Label.Location = New System.Drawing.Point(6, 129)
        Me.scalarResult2Label.Name = "scalarResult2Label"
        Me.scalarResult2Label.Size = New System.Drawing.Size(40, 13)
        Me.scalarResult2Label.TabIndex = 8
        Me.scalarResult2Label.Text = "Result:"
        '
        'highVoltageLimitLabel
        '
        Me.highVoltageLimitLabel.AutoSize = True
        Me.highVoltageLimitLabel.Location = New System.Drawing.Point(6, 49)
        Me.highVoltageLimitLabel.Name = "highVoltageLimitLabel"
        Me.highVoltageLimitLabel.Size = New System.Drawing.Size(111, 13)
        Me.highVoltageLimitLabel.TabIndex = 2
        Me.highVoltageLimitLabel.Text = "High Voltage Limit (V):"
        '
        'lowVoltageLimitLabel
        '
        Me.lowVoltageLimitLabel.AutoSize = True
        Me.lowVoltageLimitLabel.Location = New System.Drawing.Point(6, 75)
        Me.lowVoltageLimitLabel.Name = "lowVoltageLimitLabel"
        Me.lowVoltageLimitLabel.Size = New System.Drawing.Size(109, 13)
        Me.lowVoltageLimitLabel.TabIndex = 4
        Me.lowVoltageLimitLabel.Text = "Low Voltage Limit (V):"
        '
        'histogramSizeLabel
        '
        Me.histogramSizeLabel.AutoSize = True
        Me.histogramSizeLabel.Location = New System.Drawing.Point(6, 23)
        Me.histogramSizeLabel.Name = "histogramSizeLabel"
        Me.histogramSizeLabel.Size = New System.Drawing.Size(80, 13)
        Me.histogramSizeLabel.TabIndex = 0
        Me.histogramSizeLabel.Text = "Histogram Size:"
        '
        'measurement2Label
        '
        Me.measurement2Label.AutoSize = True
        Me.measurement2Label.Location = New System.Drawing.Point(6, 102)
        Me.measurement2Label.Name = "measurement2Label"
        Me.measurement2Label.Size = New System.Drawing.Size(83, 13)
        Me.measurement2Label.TabIndex = 6
        Me.measurement2Label.Text = "Measurement 2:"
        '
        'measurement1Label
        '
        Me.measurement1Label.AutoSize = True
        Me.measurement1Label.Location = New System.Drawing.Point(6, 23)
        Me.measurement1Label.Name = "measurement1Label"
        Me.measurement1Label.Size = New System.Drawing.Size(83, 13)
        Me.measurement1Label.TabIndex = 0
        Me.measurement1Label.Text = "Measurement 1:"
        '
        'highVoltageLimitNumeric
        '
        Me.highVoltageLimitNumeric.DecimalPlaces = 2
        Me.highVoltageLimitNumeric.Location = New System.Drawing.Point(191, 45)
        Me.highVoltageLimitNumeric.Maximum = New Decimal(New Integer() {-1247518720, 1073741819, 0, 0})
        Me.highVoltageLimitNumeric.Minimum = New Decimal(New Integer() {-1247518720, 1073741819, 0, -2147483648})
        Me.highVoltageLimitNumeric.Name = "highVoltageLimitNumeric"
        Me.highVoltageLimitNumeric.Size = New System.Drawing.Size(100, 20)
        Me.highVoltageLimitNumeric.TabIndex = 3
        Me.highVoltageLimitNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'lowVoltageLimitNumeric
        '
        Me.lowVoltageLimitNumeric.DecimalPlaces = 2
        Me.lowVoltageLimitNumeric.Location = New System.Drawing.Point(191, 71)
        Me.lowVoltageLimitNumeric.Maximum = New Decimal(New Integer() {-1247518720, 1073741819, 0, 0})
        Me.lowVoltageLimitNumeric.Minimum = New Decimal(New Integer() {-1247518720, 1073741819, 0, -2147483648})
        Me.lowVoltageLimitNumeric.Name = "lowVoltageLimitNumeric"
        Me.lowVoltageLimitNumeric.Size = New System.Drawing.Size(100, 20)
        Me.lowVoltageLimitNumeric.TabIndex = 5
        Me.lowVoltageLimitNumeric.Value = New Decimal(New Integer() {2, 0, 0, -2147483648})
        '
        'histogramSizeNumeric
        '
        Me.histogramSizeNumeric.Location = New System.Drawing.Point(191, 19)
        Me.histogramSizeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.histogramSizeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.histogramSizeNumeric.Name = "histogramSizeNumeric"
        Me.histogramSizeNumeric.Size = New System.Drawing.Size(100, 20)
        Me.histogramSizeNumeric.TabIndex = 1
        Me.histogramSizeNumeric.Value = New Decimal(New Integer() {75, 0, 0, 0})
        '
        'scalarResult1TextBox
        '
        Me.scalarResult1TextBox.BackColor = System.Drawing.SystemColors.Control
        Me.scalarResult1TextBox.Location = New System.Drawing.Point(95, 46)
        Me.scalarResult1TextBox.Name = "scalarResult1TextBox"
        Me.scalarResult1TextBox.ReadOnly = True
        Me.scalarResult1TextBox.Size = New System.Drawing.Size(196, 20)
        Me.scalarResult1TextBox.TabIndex = 3
        '
        'meanTextBox
        '
        Me.meanTextBox.Location = New System.Drawing.Point(95, 72)
        Me.meanTextBox.Name = "meanTextBox"
        Me.meanTextBox.ReadOnly = True
        Me.meanTextBox.Size = New System.Drawing.Size(196, 20)
        Me.meanTextBox.TabIndex = 5
        '
        'scalarResult2TextBox
        '
        Me.scalarResult2TextBox.Location = New System.Drawing.Point(95, 125)
        Me.scalarResult2TextBox.Name = "scalarResult2TextBox"
        Me.scalarResult2TextBox.ReadOnly = True
        Me.scalarResult2TextBox.Size = New System.Drawing.Size(196, 20)
        Me.scalarResult2TextBox.TabIndex = 9
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(127, 19)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(75, 23)
        Me.acquireButton.TabIndex = 0
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.Location = New System.Drawing.Point(208, 19)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'measurement2ComboBox
        '
        Me.measurement2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.measurement2ComboBox.Location = New System.Drawing.Point(95, 98)
        Me.measurement2ComboBox.Name = "measurement2ComboBox"
        Me.measurement2ComboBox.Size = New System.Drawing.Size(196, 21)
        Me.measurement2ComboBox.TabIndex = 7
        '
        'scalarMeasurementsGroupBox
        '
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.clearStatsCheckBox)
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.measurement2ComboBox)
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.measurement1ComboBox)
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.meanTextBox)
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.measurement1Label)
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.scalarResult2TextBox)
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.scalarResult1TextBox)
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.meanLabel)
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.scalarResult1Label)
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.scalarResult2Label)
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.measurement2Label)
        Me.scalarMeasurementsGroupBox.Location = New System.Drawing.Point(12, 92)
        Me.scalarMeasurementsGroupBox.Name = "scalarMeasurementsGroupBox"
        Me.scalarMeasurementsGroupBox.Size = New System.Drawing.Size(299, 173)
        Me.scalarMeasurementsGroupBox.TabIndex = 1
        Me.scalarMeasurementsGroupBox.TabStop = False
        Me.scalarMeasurementsGroupBox.Text = "Scalar Measurements"
        '
        'clearStatsCheckBox
        '
        Me.clearStatsCheckBox.AutoSize = True
        Me.clearStatsCheckBox.Location = New System.Drawing.Point(6, 151)
        Me.clearStatsCheckBox.Name = "clearStatsCheckBox"
        Me.clearStatsCheckBox.Size = New System.Drawing.Size(83, 17)
        Me.clearStatsCheckBox.TabIndex = 10
        Me.clearStatsCheckBox.Text = "Clear Stats?"
        Me.clearStatsCheckBox.UseVisualStyleBackColor = True
        '
        'measurement1ComboBox
        '
        Me.measurement1ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.measurement1ComboBox.Location = New System.Drawing.Point(95, 19)
        Me.measurement1ComboBox.Name = "measurement1ComboBox"
        Me.measurement1ComboBox.Size = New System.Drawing.Size(196, 21)
        Me.measurement1ComboBox.TabIndex = 1
        '
        'histogramParametersGroupBox
        '
        Me.histogramParametersGroupBox.Controls.Add(Me.highVoltageLimitNumeric)
        Me.histogramParametersGroupBox.Controls.Add(Me.lowVoltageLimitNumeric)
        Me.histogramParametersGroupBox.Controls.Add(Me.histogramSizeNumeric)
        Me.histogramParametersGroupBox.Controls.Add(Me.highVoltageLimitLabel)
        Me.histogramParametersGroupBox.Controls.Add(Me.lowVoltageLimitLabel)
        Me.histogramParametersGroupBox.Controls.Add(Me.histogramSizeLabel)
        Me.histogramParametersGroupBox.Location = New System.Drawing.Point(12, 276)
        Me.histogramParametersGroupBox.Name = "histogramParametersGroupBox"
        Me.histogramParametersGroupBox.Size = New System.Drawing.Size(299, 98)
        Me.histogramParametersGroupBox.TabIndex = 2
        Me.histogramParametersGroupBox.TabStop = False
        Me.histogramParametersGroupBox.Text = "Histogram Parameters"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.recordLengthMinNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleRateMinNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.recordLengthMinLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleRateMinLabel)
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(12, 385)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(299, 72)
        Me.timingParametersGroupBox.TabIndex = 3
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'recordLengthMinNumeric
        '
        Me.recordLengthMinNumeric.Location = New System.Drawing.Point(191, 45)
        Me.recordLengthMinNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.recordLengthMinNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.recordLengthMinNumeric.Name = "recordLengthMinNumeric"
        Me.recordLengthMinNumeric.Size = New System.Drawing.Size(100, 20)
        Me.recordLengthMinNumeric.TabIndex = 3
        Me.recordLengthMinNumeric.Value = New Decimal(New Integer() {512, 0, 0, 0})
        '
        'sampleRateMinNumeric
        '
        Me.sampleRateMinNumeric.DecimalPlaces = 2
        Me.sampleRateMinNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.sampleRateMinNumeric.Location = New System.Drawing.Point(191, 19)
        Me.sampleRateMinNumeric.Maximum = New Decimal(New Integer() {-1247518720, 1073741819, 0, 0})
        Me.sampleRateMinNumeric.Minimum = New Decimal(New Integer() {-1247518720, 1073741819, 0, -2147483648})
        Me.sampleRateMinNumeric.Name = "sampleRateMinNumeric"
        Me.sampleRateMinNumeric.Size = New System.Drawing.Size(100, 20)
        Me.sampleRateMinNumeric.TabIndex = 1
        Me.sampleRateMinNumeric.Value = New Decimal(New Integer() {20000000, 0, 0, 0})
        '
        'recordLengthMinLabel
        '
        Me.recordLengthMinLabel.AutoSize = True
        Me.recordLengthMinLabel.Location = New System.Drawing.Point(6, 49)
        Me.recordLengthMinLabel.Name = "recordLengthMinLabel"
        Me.recordLengthMinLabel.Size = New System.Drawing.Size(125, 13)
        Me.recordLengthMinLabel.TabIndex = 2
        Me.recordLengthMinLabel.Text = "Minimum Record Length:"
        '
        'sampleRateMinLabel
        '
        Me.sampleRateMinLabel.AutoSize = True
        Me.sampleRateMinLabel.Location = New System.Drawing.Point(6, 23)
        Me.sampleRateMinLabel.Name = "sampleRateMinLabel"
        Me.sampleRateMinLabel.Size = New System.Drawing.Size(115, 13)
        Me.sampleRateMinLabel.TabIndex = 0
        Me.sampleRateMinLabel.Text = "Minimum Sample Rate:"
        '
        'sampledDataGroupBox
        '
        Me.sampledDataGroupBox.Controls.Add(Me.waveformFromScopeDataGridView)
        Me.sampledDataGroupBox.Location = New System.Drawing.Point(317, 12)
        Me.sampledDataGroupBox.Name = "sampledDataGroupBox"
        Me.sampledDataGroupBox.Size = New System.Drawing.Size(202, 445)
        Me.sampledDataGroupBox.TabIndex = 5
        Me.sampledDataGroupBox.TabStop = False
        Me.sampledDataGroupBox.Text = "Waveform From Scope"
        '
        'waveformFromScopeDataGridView
        '
        Me.waveformFromScopeDataGridView.AllowUserToAddRows = False
        Me.waveformFromScopeDataGridView.AllowUserToDeleteRows = False
        Me.waveformFromScopeDataGridView.AllowUserToResizeRows = False
        Me.waveformFromScopeDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.waveformFromScopeDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.waveformFromScopeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.waveformFromScopeDataGridView.DefaultCellStyle = DataGridViewCellStyle2
        Me.waveformFromScopeDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.waveformFromScopeDataGridView.Name = "waveformFromScopeDataGridView"
        Me.waveformFromScopeDataGridView.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.waveformFromScopeDataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.waveformFromScopeDataGridView.RowHeadersVisible = False
        Me.waveformFromScopeDataGridView.RowHeadersWidth = 15
        Me.waveformFromScopeDataGridView.RowTemplate.Height = 24
        Me.waveformFromScopeDataGridView.Size = New System.Drawing.Size(190, 420)
        Me.waveformFromScopeDataGridView.StandardTab = True
        Me.waveformFromScopeDataGridView.TabIndex = 0
        '
        'generalGroupBox
        '
        Me.generalGroupBox.Controls.Add(Me.channelTextBox)
        Me.generalGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.generalGroupBox.Controls.Add(Me.channelNameLabel)
        Me.generalGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.generalGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.generalGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.generalGroupBox.Name = "generalGroupBox"
        Me.generalGroupBox.Size = New System.Drawing.Size(299, 69)
        Me.generalGroupBox.TabIndex = 0
        Me.generalGroupBox.TabStop = False
        Me.generalGroupBox.Text = "General"
        '
        'channelTextBox
        '
        Me.channelTextBox.Location = New System.Drawing.Point(191, 43)
        Me.channelTextBox.Name = "channelTextBox"
        Me.channelTextBox.Size = New System.Drawing.Size(100, 20)
        Me.channelTextBox.TabIndex = 1
        Me.channelTextBox.Text = "0"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(191, 16)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(100, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelNameLabel.Location = New System.Drawing.Point(6, 47)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(80, 13)
        Me.channelNameLabel.TabIndex = 2
        Me.channelNameLabel.Text = "Channel Name:"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 20)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(12, 468)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(299, 58)
        Me.messageGroupBox.TabIndex = 4
        Me.messageGroupBox.TabStop = False
        Me.messageGroupBox.Text = "Message"
        '
        'messageTextBox
        '
        Me.messageTextBox.Location = New System.Drawing.Point(9, 19)
        Me.messageTextBox.Name = "messageTextBox"
        Me.messageTextBox.ReadOnly = True
        Me.messageTextBox.Size = New System.Drawing.Size(282, 35)
        Me.messageTextBox.TabIndex = 0
        Me.messageTextBox.Text = ""
        '
        'buttonsGroupBox
        '
        Me.buttonsGroupBox.Controls.Add(Me.acquireButton)
        Me.buttonsGroupBox.Controls.Add(Me.stopButton)
        Me.buttonsGroupBox.Location = New System.Drawing.Point(317, 468)
        Me.buttonsGroupBox.Name = "buttonsGroupBox"
        Me.buttonsGroupBox.Size = New System.Drawing.Size(410, 58)
        Me.buttonsGroupBox.TabIndex = 7
        Me.buttonsGroupBox.TabStop = False
        '
        'voltageHistogramDataGroupBox
        '
        Me.voltageHistogramDataGroupBox.Controls.Add(Me.voltageHistogramDataGridView)
        Me.voltageHistogramDataGroupBox.Location = New System.Drawing.Point(525, 12)
        Me.voltageHistogramDataGroupBox.Name = "voltageHistogramDataGroupBox"
        Me.voltageHistogramDataGroupBox.Size = New System.Drawing.Size(202, 445)
        Me.voltageHistogramDataGroupBox.TabIndex = 6
        Me.voltageHistogramDataGroupBox.TabStop = False
        Me.voltageHistogramDataGroupBox.Text = "Voltage Histogram"
        '
        'voltageHistogramDataGridView
        '
        Me.voltageHistogramDataGridView.AllowUserToAddRows = False
        Me.voltageHistogramDataGridView.AllowUserToDeleteRows = False
        Me.voltageHistogramDataGridView.AllowUserToResizeRows = False
        Me.voltageHistogramDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.voltageHistogramDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.voltageHistogramDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.voltageHistogramDataGridView.DefaultCellStyle = DataGridViewCellStyle5
        Me.voltageHistogramDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.voltageHistogramDataGridView.Name = "voltageHistogramDataGridView"
        Me.voltageHistogramDataGridView.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.voltageHistogramDataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.voltageHistogramDataGridView.RowHeadersVisible = False
        Me.voltageHistogramDataGridView.RowHeadersWidth = 15
        Me.voltageHistogramDataGridView.RowTemplate.Height = 24
        Me.voltageHistogramDataGridView.Size = New System.Drawing.Size(190, 420)
        Me.voltageHistogramDataGridView.StandardTab = True
        Me.voltageHistogramDataGridView.TabIndex = 0
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(739, 538)
        Me.Controls.Add(Me.voltageHistogramDataGroupBox)
        Me.Controls.Add(Me.buttonsGroupBox)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.generalGroupBox)
        Me.Controls.Add(Me.sampledDataGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.scalarMeasurementsGroupBox)
        Me.Controls.Add(Me.histogramParametersGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Voltage Histogram"
        CType(Me.highVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lowVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.histogramSizeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scalarMeasurementsGroupBox.ResumeLayout(False)
        Me.scalarMeasurementsGroupBox.PerformLayout()
        Me.histogramParametersGroupBox.ResumeLayout(False)
        Me.histogramParametersGroupBox.PerformLayout()
        Me.timingParametersGroupBox.ResumeLayout(False)
        Me.timingParametersGroupBox.PerformLayout()
        CType(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.sampledDataGroupBox.ResumeLayout(False)
        CType(Me.waveformFromScopeDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.generalGroupBox.ResumeLayout(False)
        Me.generalGroupBox.PerformLayout()
        Me.messageGroupBox.ResumeLayout(False)
        Me.buttonsGroupBox.ResumeLayout(False)
        Me.voltageHistogramDataGroupBox.ResumeLayout(False)
        CType(Me.voltageHistogramDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private scalarResult1Label As System.Windows.Forms.Label
    Private meanLabel As System.Windows.Forms.Label
    Private scalarResult2Label As System.Windows.Forms.Label
    Private highVoltageLimitLabel As System.Windows.Forms.Label
    Private lowVoltageLimitLabel As System.Windows.Forms.Label
    Private histogramSizeLabel As System.Windows.Forms.Label
    Private measurement2Label As System.Windows.Forms.Label
    Private measurement1Label As System.Windows.Forms.Label
    Private highVoltageLimitNumeric As System.Windows.Forms.NumericUpDown
    Private lowVoltageLimitNumeric As System.Windows.Forms.NumericUpDown
    Private histogramSizeNumeric As System.Windows.Forms.NumericUpDown
    Private scalarResult1TextBox As System.Windows.Forms.TextBox
    Private meanTextBox As System.Windows.Forms.TextBox
    Private scalarResult2TextBox As System.Windows.Forms.TextBox
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private measurement2ComboBox As System.Windows.Forms.ComboBox
    Private measurement1ComboBox As System.Windows.Forms.ComboBox
    Private scalarMeasurementsGroupBox As System.Windows.Forms.GroupBox
    Private histogramParametersGroupBox As System.Windows.Forms.GroupBox
    Private timingParametersGroupBox As System.Windows.Forms.GroupBox
    Private recordLengthMinNumeric As System.Windows.Forms.NumericUpDown
    Private sampleRateMinNumeric As System.Windows.Forms.NumericUpDown
    Private recordLengthMinLabel As System.Windows.Forms.Label
    Private sampleRateMinLabel As System.Windows.Forms.Label
    Private clearStatsCheckBox As System.Windows.Forms.CheckBox
    Private sampledDataGroupBox As System.Windows.Forms.GroupBox
    Private waveformFromScopeDataGridView As System.Windows.Forms.DataGridView
    Private generalGroupBox As System.Windows.Forms.GroupBox
    Private channelTextBox As System.Windows.Forms.TextBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private channelNameLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private messageGroupBox As System.Windows.Forms.GroupBox
    Private messageTextBox As System.Windows.Forms.RichTextBox
    Private buttonsGroupBox As System.Windows.Forms.GroupBox
    Private voltageHistogramDataGroupBox As System.Windows.Forms.GroupBox
    Private voltageHistogramDataGridView As System.Windows.Forms.DataGridView

End Class
