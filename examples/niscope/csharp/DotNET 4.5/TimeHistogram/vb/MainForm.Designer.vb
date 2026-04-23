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
        Me.triggerReferencePositionLabel = New System.Windows.Forms.Label()
        Me.highTimeLimitLabel = New System.Windows.Forms.Label()
        Me.lowTimeLimitLabel = New System.Windows.Forms.Label()
        Me.highTimeLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.lowTimeLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.generalGroupBox = New System.Windows.Forms.GroupBox()
        Me.channelTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.histogramParametersGroupBox = New System.Windows.Forms.GroupBox()
        Me.highVoltageLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.lowVoltageLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.histogramSizeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.label4 = New System.Windows.Forms.Label()
        Me.label5 = New System.Windows.Forms.Label()
        Me.label6 = New System.Windows.Forms.Label()
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox()
        Me.triggerReferencePositionNumeric = New System.Windows.Forms.NumericUpDown()
        Me.recordLengthMinNumeric = New System.Windows.Forms.NumericUpDown()
        Me.sampleRateMinNumeric = New System.Windows.Forms.NumericUpDown()
        Me.label7 = New System.Windows.Forms.Label()
        Me.label8 = New System.Windows.Forms.Label()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.RichTextBox()
        Me.sampledDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.waveformFromScopeDataGridView = New System.Windows.Forms.DataGridView()
        Me.timeHistogramDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.timeHistogramDataGridView = New System.Windows.Forms.DataGridView()
        Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.scalarMeasurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.clearStatsCheckBox = New System.Windows.Forms.CheckBox()
        Me.measurement2ComboBox = New System.Windows.Forms.ComboBox()
        Me.measurement1ComboBox = New System.Windows.Forms.ComboBox()
        Me.meanTextBox = New System.Windows.Forms.TextBox()
        Me.measurement1Label = New System.Windows.Forms.Label()
        Me.scalarResult2TextBox = New System.Windows.Forms.TextBox()
        Me.scalarResult1TextBox = New System.Windows.Forms.TextBox()
        Me.meanLabel = New System.Windows.Forms.Label()
        Me.scalarResult1Label = New System.Windows.Forms.Label()
        Me.scalarResult2Label = New System.Windows.Forms.Label()
        Me.measurement2Label = New System.Windows.Forms.Label()
        CType(Me.highTimeLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lowTimeLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.generalGroupBox.SuspendLayout()
        Me.histogramParametersGroupBox.SuspendLayout()
        CType(Me.highVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lowVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.histogramSizeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.triggerReferencePositionNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.messageGroupBox.SuspendLayout()
        Me.sampledDataGroupBox.SuspendLayout()
        CType(Me.waveformFromScopeDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timeHistogramDataGroupBox.SuspendLayout()
        CType(Me.timeHistogramDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.buttonsGroupBox.SuspendLayout()
        Me.scalarMeasurementsGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'triggerReferencePositionLabel
        '
        Me.triggerReferencePositionLabel.AutoSize = True
        Me.triggerReferencePositionLabel.Location = New System.Drawing.Point(6, 21)
        Me.triggerReferencePositionLabel.Name = "triggerReferencePositionLabel"
        Me.triggerReferencePositionLabel.Size = New System.Drawing.Size(133, 13)
        Me.triggerReferencePositionLabel.TabIndex = 13
        Me.triggerReferencePositionLabel.Text = "Trigger Reference Position"
        '
        'highTimeLimitLabel
        '
        Me.highTimeLimitLabel.AutoSize = True
        Me.highTimeLimitLabel.Location = New System.Drawing.Point(6, 98)
        Me.highTimeLimitLabel.Name = "highTimeLimitLabel"
        Me.highTimeLimitLabel.Size = New System.Drawing.Size(93, 13)
        Me.highTimeLimitLabel.TabIndex = 15
        Me.highTimeLimitLabel.Text = "High Time Limit (s)"
        '
        'lowTimeLimitLabel
        '
        Me.lowTimeLimitLabel.AutoSize = True
        Me.lowTimeLimitLabel.Location = New System.Drawing.Point(6, 122)
        Me.lowTimeLimitLabel.Name = "lowTimeLimitLabel"
        Me.lowTimeLimitLabel.Size = New System.Drawing.Size(91, 13)
        Me.lowTimeLimitLabel.TabIndex = 17
        Me.lowTimeLimitLabel.Text = "Low Time Limit (s)"
        '
        'highTimeLimitNumeric
        '
        Me.highTimeLimitNumeric.DecimalPlaces = 7
        Me.highTimeLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 393216})
        Me.highTimeLimitNumeric.Location = New System.Drawing.Point(193, 97)
        Me.highTimeLimitNumeric.Maximum = New Decimal(New Integer() {-1247518720, 1073741819, 0, 0})
        Me.highTimeLimitNumeric.Minimum = New Decimal(New Integer() {-1247518720, 1073741819, 0, -2147483648})
        Me.highTimeLimitNumeric.Name = "highTimeLimitNumeric"
        Me.highTimeLimitNumeric.Size = New System.Drawing.Size(100, 20)
        Me.highTimeLimitNumeric.TabIndex = 3
        Me.highTimeLimitNumeric.Value = New Decimal(New Integer() {70, 0, 0, 458752})
        '
        'lowTimeLimitNumeric
        '
        Me.lowTimeLimitNumeric.DecimalPlaces = 7
        Me.lowTimeLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 393216})
        Me.lowTimeLimitNumeric.Location = New System.Drawing.Point(193, 122)
        Me.lowTimeLimitNumeric.Maximum = New Decimal(New Integer() {-1247518720, 1073741819, 0, 0})
        Me.lowTimeLimitNumeric.Minimum = New Decimal(New Integer() {-1247518720, 1073741819, 0, -2147483648})
        Me.lowTimeLimitNumeric.Name = "lowTimeLimitNumeric"
        Me.lowTimeLimitNumeric.Size = New System.Drawing.Size(100, 20)
        Me.lowTimeLimitNumeric.TabIndex = 4
        Me.lowTimeLimitNumeric.Value = New Decimal(New Integer() {15, 0, 0, 458752})
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
        Me.generalGroupBox.Size = New System.Drawing.Size(299, 74)
        Me.generalGroupBox.TabIndex = 0
        Me.generalGroupBox.TabStop = False
        Me.generalGroupBox.Text = "General"
        '
        'channelTextBox
        '
        Me.channelTextBox.Location = New System.Drawing.Point(193, 46)
        Me.channelTextBox.Name = "channelTextBox"
        Me.channelTextBox.Size = New System.Drawing.Size(100, 20)
        Me.channelTextBox.TabIndex = 3
        Me.channelTextBox.Text = "0"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(193, 19)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(100, 21)
        Me.resourceNameComboBox.TabIndex = 1
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
        'histogramParametersGroupBox
        '
        Me.histogramParametersGroupBox.Controls.Add(Me.highTimeLimitNumeric)
        Me.histogramParametersGroupBox.Controls.Add(Me.lowTimeLimitNumeric)
        Me.histogramParametersGroupBox.Controls.Add(Me.highVoltageLimitNumeric)
        Me.histogramParametersGroupBox.Controls.Add(Me.lowVoltageLimitNumeric)
        Me.histogramParametersGroupBox.Controls.Add(Me.histogramSizeNumeric)
        Me.histogramParametersGroupBox.Controls.Add(Me.label4)
        Me.histogramParametersGroupBox.Controls.Add(Me.lowTimeLimitLabel)
        Me.histogramParametersGroupBox.Controls.Add(Me.label5)
        Me.histogramParametersGroupBox.Controls.Add(Me.highTimeLimitLabel)
        Me.histogramParametersGroupBox.Controls.Add(Me.label6)
        Me.histogramParametersGroupBox.Location = New System.Drawing.Point(12, 281)
        Me.histogramParametersGroupBox.Name = "histogramParametersGroupBox"
        Me.histogramParametersGroupBox.Size = New System.Drawing.Size(299, 151)
        Me.histogramParametersGroupBox.TabIndex = 2
        Me.histogramParametersGroupBox.TabStop = False
        Me.histogramParametersGroupBox.Text = "Histogram Parameters"
        '
        'highVoltageLimitNumeric
        '
        Me.highVoltageLimitNumeric.DecimalPlaces = 2
        Me.highVoltageLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.highVoltageLimitNumeric.Location = New System.Drawing.Point(193, 45)
        Me.highVoltageLimitNumeric.Maximum = New Decimal(New Integer() {-1247518720, 1073741819, 0, 0})
        Me.highVoltageLimitNumeric.Minimum = New Decimal(New Integer() {-1247518720, 1073741819, 0, -2147483648})
        Me.highVoltageLimitNumeric.Name = "highVoltageLimitNumeric"
        Me.highVoltageLimitNumeric.Size = New System.Drawing.Size(100, 20)
        Me.highVoltageLimitNumeric.TabIndex = 1
        Me.highVoltageLimitNumeric.Value = New Decimal(New Integer() {12, 0, 0, 65536})
        '
        'lowVoltageLimitNumeric
        '
        Me.lowVoltageLimitNumeric.DecimalPlaces = 2
        Me.lowVoltageLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.lowVoltageLimitNumeric.Location = New System.Drawing.Point(193, 71)
        Me.lowVoltageLimitNumeric.Maximum = New Decimal(New Integer() {-1247518720, 1073741819, 0, 0})
        Me.lowVoltageLimitNumeric.Minimum = New Decimal(New Integer() {-1247518720, 1073741819, 0, -2147483648})
        Me.lowVoltageLimitNumeric.Name = "lowVoltageLimitNumeric"
        Me.lowVoltageLimitNumeric.Size = New System.Drawing.Size(100, 20)
        Me.lowVoltageLimitNumeric.TabIndex = 2
        Me.lowVoltageLimitNumeric.Value = New Decimal(New Integer() {5, 0, 0, -2147418112})
        '
        'histogramSizeNumeric
        '
        Me.histogramSizeNumeric.Location = New System.Drawing.Point(193, 19)
        Me.histogramSizeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.histogramSizeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.histogramSizeNumeric.Name = "histogramSizeNumeric"
        Me.histogramSizeNumeric.Size = New System.Drawing.Size(100, 20)
        Me.histogramSizeNumeric.TabIndex = 0
        Me.histogramSizeNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(6, 49)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(111, 13)
        Me.label4.TabIndex = 16
        Me.label4.Text = "High Voltage Limit (V):"
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(6, 75)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(109, 13)
        Me.label5.TabIndex = 18
        Me.label5.Text = "Low Voltage Limit (V):"
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(6, 23)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(80, 13)
        Me.label6.TabIndex = 19
        Me.label6.Text = "Histogram Size:"
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.triggerReferencePositionNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.recordLengthMinNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleRateMinNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.label7)
        Me.timingParametersGroupBox.Controls.Add(Me.label8)
        Me.timingParametersGroupBox.Controls.Add(Me.triggerReferencePositionLabel)
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(12, 443)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(299, 96)
        Me.timingParametersGroupBox.TabIndex = 3
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'triggerReferencePositionNumeric
        '
        Me.triggerReferencePositionNumeric.DecimalPlaces = 2
        Me.triggerReferencePositionNumeric.Location = New System.Drawing.Point(193, 19)
        Me.triggerReferencePositionNumeric.Maximum = New Decimal(New Integer() {-1247518720, 1073741819, 0, 0})
        Me.triggerReferencePositionNumeric.Minimum = New Decimal(New Integer() {-1247518720, 1073741819, 0, -2147483648})
        Me.triggerReferencePositionNumeric.Name = "triggerReferencePositionNumeric"
        Me.triggerReferencePositionNumeric.Size = New System.Drawing.Size(100, 20)
        Me.triggerReferencePositionNumeric.TabIndex = 0
        Me.triggerReferencePositionNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'recordLengthMinNumeric
        '
        Me.recordLengthMinNumeric.Location = New System.Drawing.Point(193, 71)
        Me.recordLengthMinNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.recordLengthMinNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.recordLengthMinNumeric.Name = "recordLengthMinNumeric"
        Me.recordLengthMinNumeric.Size = New System.Drawing.Size(100, 20)
        Me.recordLengthMinNumeric.TabIndex = 2
        Me.recordLengthMinNumeric.Value = New Decimal(New Integer() {512, 0, 0, 0})
        '
        'sampleRateMinNumeric
        '
        Me.sampleRateMinNumeric.DecimalPlaces = 2
        Me.sampleRateMinNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.sampleRateMinNumeric.Location = New System.Drawing.Point(193, 45)
        Me.sampleRateMinNumeric.Maximum = New Decimal(New Integer() {-1247518720, 1073741819, 0, 0})
        Me.sampleRateMinNumeric.Minimum = New Decimal(New Integer() {-1247518720, 1073741819, 0, -2147483648})
        Me.sampleRateMinNumeric.Name = "sampleRateMinNumeric"
        Me.sampleRateMinNumeric.Size = New System.Drawing.Size(100, 20)
        Me.sampleRateMinNumeric.TabIndex = 1
        Me.sampleRateMinNumeric.Value = New Decimal(New Integer() {20000000, 0, 0, 0})
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(6, 70)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(125, 13)
        Me.label7.TabIndex = 2
        Me.label7.Text = "Minimum Record Length:"
        '
        'label8
        '
        Me.label8.AutoSize = True
        Me.label8.Location = New System.Drawing.Point(6, 46)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(115, 13)
        Me.label8.TabIndex = 3
        Me.label8.Text = "Minimum Sample Rate:"
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(12, 545)
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
        Me.messageTextBox.Size = New System.Drawing.Size(284, 35)
        Me.messageTextBox.TabIndex = 0
        Me.messageTextBox.Text = ""
        '
        'sampledDataGroupBox
        '
        Me.sampledDataGroupBox.Controls.Add(Me.waveformFromScopeDataGridView)
        Me.sampledDataGroupBox.Location = New System.Drawing.Point(325, 12)
        Me.sampledDataGroupBox.Name = "sampledDataGroupBox"
        Me.sampledDataGroupBox.Size = New System.Drawing.Size(202, 527)
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
        Me.waveformFromScopeDataGridView.Size = New System.Drawing.Size(190, 502)
        Me.waveformFromScopeDataGridView.StandardTab = True
        Me.waveformFromScopeDataGridView.TabIndex = 0
        '
        'timeHistogramDataGroupBox
        '
        Me.timeHistogramDataGroupBox.Controls.Add(Me.timeHistogramDataGridView)
        Me.timeHistogramDataGroupBox.Location = New System.Drawing.Point(540, 15)
        Me.timeHistogramDataGroupBox.Name = "timeHistogramDataGroupBox"
        Me.timeHistogramDataGroupBox.Size = New System.Drawing.Size(202, 524)
        Me.timeHistogramDataGroupBox.TabIndex = 6
        Me.timeHistogramDataGroupBox.TabStop = False
        Me.timeHistogramDataGroupBox.Text = "Time Histogram"
        '
        'timeHistogramDataGridView
        '
        Me.timeHistogramDataGridView.AllowUserToAddRows = False
        Me.timeHistogramDataGridView.AllowUserToDeleteRows = False
        Me.timeHistogramDataGridView.AllowUserToResizeRows = False
        Me.timeHistogramDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.timeHistogramDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.timeHistogramDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.timeHistogramDataGridView.DefaultCellStyle = DataGridViewCellStyle5
        Me.timeHistogramDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.timeHistogramDataGridView.Name = "timeHistogramDataGridView"
        Me.timeHistogramDataGridView.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.timeHistogramDataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.timeHistogramDataGridView.RowHeadersVisible = False
        Me.timeHistogramDataGridView.RowHeadersWidth = 15
        Me.timeHistogramDataGridView.RowTemplate.Height = 24
        Me.timeHistogramDataGridView.Size = New System.Drawing.Size(190, 499)
        Me.timeHistogramDataGridView.StandardTab = True
        Me.timeHistogramDataGridView.TabIndex = 0
        '
        'buttonsGroupBox
        '
        Me.buttonsGroupBox.Controls.Add(Me.acquireButton)
        Me.buttonsGroupBox.Controls.Add(Me.stopButton)
        Me.buttonsGroupBox.Location = New System.Drawing.Point(325, 545)
        Me.buttonsGroupBox.Name = "buttonsGroupBox"
        Me.buttonsGroupBox.Size = New System.Drawing.Size(417, 58)
        Me.buttonsGroupBox.TabIndex = 7
        Me.buttonsGroupBox.TabStop = False
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
        Me.scalarMeasurementsGroupBox.Location = New System.Drawing.Point(12, 97)
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
        Me.clearStatsCheckBox.TabIndex = 5
        Me.clearStatsCheckBox.Text = "Clear Stats?"
        Me.clearStatsCheckBox.UseVisualStyleBackColor = True
        '
        'measurement2ComboBox
        '
        Me.measurement2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.measurement2ComboBox.Location = New System.Drawing.Point(97, 98)
        Me.measurement2ComboBox.Name = "measurement2ComboBox"
        Me.measurement2ComboBox.Size = New System.Drawing.Size(196, 21)
        Me.measurement2ComboBox.TabIndex = 3
        '
        'measurement1ComboBox
        '
        Me.measurement1ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.measurement1ComboBox.Location = New System.Drawing.Point(97, 19)
        Me.measurement1ComboBox.Name = "measurement1ComboBox"
        Me.measurement1ComboBox.Size = New System.Drawing.Size(196, 21)
        Me.measurement1ComboBox.TabIndex = 0
        '
        'meanTextBox
        '
        Me.meanTextBox.Location = New System.Drawing.Point(97, 72)
        Me.meanTextBox.Name = "meanTextBox"
        Me.meanTextBox.ReadOnly = True
        Me.meanTextBox.Size = New System.Drawing.Size(196, 20)
        Me.meanTextBox.TabIndex = 2
        '
        'measurement1Label
        '
        Me.measurement1Label.AutoSize = True
        Me.measurement1Label.Location = New System.Drawing.Point(6, 23)
        Me.measurement1Label.Name = "measurement1Label"
        Me.measurement1Label.Size = New System.Drawing.Size(83, 13)
        Me.measurement1Label.TabIndex = 26
        Me.measurement1Label.Text = "Measurement 1:"
        '
        'scalarResult2TextBox
        '
        Me.scalarResult2TextBox.Location = New System.Drawing.Point(97, 125)
        Me.scalarResult2TextBox.Name = "scalarResult2TextBox"
        Me.scalarResult2TextBox.ReadOnly = True
        Me.scalarResult2TextBox.Size = New System.Drawing.Size(196, 20)
        Me.scalarResult2TextBox.TabIndex = 4
        '
        'scalarResult1TextBox
        '
        Me.scalarResult1TextBox.BackColor = System.Drawing.SystemColors.Control
        Me.scalarResult1TextBox.Location = New System.Drawing.Point(97, 46)
        Me.scalarResult1TextBox.Name = "scalarResult1TextBox"
        Me.scalarResult1TextBox.ReadOnly = True
        Me.scalarResult1TextBox.Size = New System.Drawing.Size(196, 20)
        Me.scalarResult1TextBox.TabIndex = 1
        '
        'meanLabel
        '
        Me.meanLabel.AutoSize = True
        Me.meanLabel.Location = New System.Drawing.Point(6, 76)
        Me.meanLabel.Name = "meanLabel"
        Me.meanLabel.Size = New System.Drawing.Size(37, 13)
        Me.meanLabel.TabIndex = 20
        Me.meanLabel.Text = "Mean:"
        '
        'scalarResult1Label
        '
        Me.scalarResult1Label.AutoSize = True
        Me.scalarResult1Label.Location = New System.Drawing.Point(6, 50)
        Me.scalarResult1Label.Name = "scalarResult1Label"
        Me.scalarResult1Label.Size = New System.Drawing.Size(40, 13)
        Me.scalarResult1Label.TabIndex = 11
        Me.scalarResult1Label.Text = "Result:"
        '
        'scalarResult2Label
        '
        Me.scalarResult2Label.AutoSize = True
        Me.scalarResult2Label.BackColor = System.Drawing.SystemColors.Control
        Me.scalarResult2Label.Location = New System.Drawing.Point(6, 129)
        Me.scalarResult2Label.Name = "scalarResult2Label"
        Me.scalarResult2Label.Size = New System.Drawing.Size(40, 13)
        Me.scalarResult2Label.TabIndex = 22
        Me.scalarResult2Label.Text = "Result:"
        '
        'measurement2Label
        '
        Me.measurement2Label.AutoSize = True
        Me.measurement2Label.Location = New System.Drawing.Point(6, 102)
        Me.measurement2Label.Name = "measurement2Label"
        Me.measurement2Label.Size = New System.Drawing.Size(83, 13)
        Me.measurement2Label.TabIndex = 25
        Me.measurement2Label.Text = "Measurement 2:"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(755, 615)
        Me.Controls.Add(Me.scalarMeasurementsGroupBox)
        Me.Controls.Add(Me.buttonsGroupBox)
        Me.Controls.Add(Me.timeHistogramDataGroupBox)
        Me.Controls.Add(Me.sampledDataGroupBox)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.histogramParametersGroupBox)
        Me.Controls.Add(Me.generalGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Time Histogram"
        CType(Me.highTimeLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lowTimeLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.generalGroupBox.ResumeLayout(False)
        Me.generalGroupBox.PerformLayout()
        Me.histogramParametersGroupBox.ResumeLayout(False)
        Me.histogramParametersGroupBox.PerformLayout()
        CType(Me.highVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lowVoltageLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.histogramSizeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timingParametersGroupBox.ResumeLayout(False)
        Me.timingParametersGroupBox.PerformLayout()
        CType(Me.triggerReferencePositionNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.messageGroupBox.ResumeLayout(False)
        Me.sampledDataGroupBox.ResumeLayout(False)
        CType(Me.waveformFromScopeDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timeHistogramDataGroupBox.ResumeLayout(False)
        CType(Me.timeHistogramDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.buttonsGroupBox.ResumeLayout(False)
        Me.scalarMeasurementsGroupBox.ResumeLayout(False)
        Me.scalarMeasurementsGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private triggerReferencePositionLabel As System.Windows.Forms.Label
    Private highTimeLimitLabel As System.Windows.Forms.Label
    Private lowTimeLimitLabel As System.Windows.Forms.Label
    Private highTimeLimitNumeric As System.Windows.Forms.NumericUpDown
    Private lowTimeLimitNumeric As System.Windows.Forms.NumericUpDown
    Private generalGroupBox As System.Windows.Forms.GroupBox
    Private channelTextBox As System.Windows.Forms.TextBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private channelNameLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private histogramParametersGroupBox As System.Windows.Forms.GroupBox
    Private highVoltageLimitNumeric As System.Windows.Forms.NumericUpDown
    Private lowVoltageLimitNumeric As System.Windows.Forms.NumericUpDown
    Private histogramSizeNumeric As System.Windows.Forms.NumericUpDown
    Private label4 As System.Windows.Forms.Label
    Private label5 As System.Windows.Forms.Label
    Private label6 As System.Windows.Forms.Label
    Private timingParametersGroupBox As System.Windows.Forms.GroupBox
    Private label7 As System.Windows.Forms.Label
    Private label8 As System.Windows.Forms.Label
    Private messageGroupBox As System.Windows.Forms.GroupBox
    Private messageTextBox As System.Windows.Forms.RichTextBox
    Private sampledDataGroupBox As System.Windows.Forms.GroupBox
    Private waveformFromScopeDataGridView As System.Windows.Forms.DataGridView
    Private timeHistogramDataGroupBox As System.Windows.Forms.GroupBox
    Private timeHistogramDataGridView As System.Windows.Forms.DataGridView
    Private buttonsGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private scalarMeasurementsGroupBox As System.Windows.Forms.GroupBox
    Private clearStatsCheckBox As System.Windows.Forms.CheckBox
    Private measurement2ComboBox As System.Windows.Forms.ComboBox
    Private measurement1ComboBox As System.Windows.Forms.ComboBox
    Private meanTextBox As System.Windows.Forms.TextBox
    Private measurement1Label As System.Windows.Forms.Label
    Private scalarResult2TextBox As System.Windows.Forms.TextBox
    Private scalarResult1TextBox As System.Windows.Forms.TextBox
    Private meanLabel As System.Windows.Forms.Label
    Private scalarResult1Label As System.Windows.Forms.Label
    Private scalarResult2Label As System.Windows.Forms.Label
    Private measurement2Label As System.Windows.Forms.Label
    Private recordLengthMinNumeric As System.Windows.Forms.NumericUpDown
    Private sampleRateMinNumeric As System.Windows.Forms.NumericUpDown
    Private triggerReferencePositionNumeric As System.Windows.Forms.NumericUpDown


End Class
