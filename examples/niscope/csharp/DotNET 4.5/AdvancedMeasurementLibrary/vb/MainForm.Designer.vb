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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.measurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.noteTextBox = New System.Windows.Forms.TextBox()
        Me.arrayMeasurementLabel = New System.Windows.Forms.Label()
        Me.preocessingLabel = New System.Windows.Forms.Label()
        Me.arrayMeasurementComboBox = New System.Windows.Forms.ComboBox()
        Me.processingStepComboBox = New System.Windows.Forms.ComboBox()
        Me.generalGroupBox = New System.Windows.Forms.GroupBox()
        Me.timeoutNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channelTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.timeoutLabel = New System.Windows.Forms.Label()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.filterComboBox = New System.Windows.Forms.ComboBox()
        Me.filterTypeLabel = New System.Windows.Forms.Label()
        Me.lowPassFrequencyLabel = New System.Windows.Forms.Label()
        Me.stopFrequencyLabel = New System.Windows.Forms.Label()
        Me.stopWidthLabel = New System.Windows.Forms.Label()
        Me.cutoffFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.centerFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.bandpassWidthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.filterGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.RichTextBox()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.sampledDataGridView = New System.Windows.Forms.DataGridView()
        Me.sampledDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementDataGridView = New System.Windows.Forms.DataGridView()
        Me.measurementDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementsGroupBox.SuspendLayout()
        Me.generalGroupBox.SuspendLayout()
        CType(Me.timeoutNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cutoffFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.centerFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bandpassWidthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.filterGroupBox.SuspendLayout()
        Me.messageGroupBox.SuspendLayout()
        CType(Me.sampledDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.sampledDataGroupBox.SuspendLayout()
        CType(Me.measurementDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementDataGroupBox.SuspendLayout()
        Me.buttonsGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'measurementsGroupBox
        '
        Me.measurementsGroupBox.Controls.Add(Me.noteTextBox)
        Me.measurementsGroupBox.Controls.Add(Me.arrayMeasurementLabel)
        Me.measurementsGroupBox.Controls.Add(Me.preocessingLabel)
        Me.measurementsGroupBox.Controls.Add(Me.arrayMeasurementComboBox)
        Me.measurementsGroupBox.Controls.Add(Me.processingStepComboBox)
        Me.measurementsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.measurementsGroupBox.Location = New System.Drawing.Point(12, 118)
        Me.measurementsGroupBox.Name = "measurementsGroupBox"
        Me.measurementsGroupBox.Size = New System.Drawing.Size(282, 215)
        Me.measurementsGroupBox.TabIndex = 1
        Me.measurementsGroupBox.TabStop = False
        Me.measurementsGroupBox.Text = "Measurements"
        '
        'noteTextBox
        '
        Me.noteTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.noteTextBox.Location = New System.Drawing.Point(14, 75)
        Me.noteTextBox.Multiline = True
        Me.noteTextBox.Name = "noteTextBox"
        Me.noteTextBox.ReadOnly = True
        Me.noteTextBox.Size = New System.Drawing.Size(257, 119)
        Me.noteTextBox.TabIndex = 4
        Me.noteTextBox.TabStop = False
        Me.noteTextBox.Text = resources.GetString("noteTextBox.Text")
        '
        'arrayMeasurementLabel
        '
        Me.arrayMeasurementLabel.AutoSize = True
        Me.arrayMeasurementLabel.Location = New System.Drawing.Point(6, 47)
        Me.arrayMeasurementLabel.Name = "arrayMeasurementLabel"
        Me.arrayMeasurementLabel.Size = New System.Drawing.Size(101, 13)
        Me.arrayMeasurementLabel.TabIndex = 2
        Me.arrayMeasurementLabel.Text = "Array Measurement:"
        '
        'preocessingLabel
        '
        Me.preocessingLabel.AutoSize = True
        Me.preocessingLabel.Location = New System.Drawing.Point(6, 20)
        Me.preocessingLabel.Name = "preocessingLabel"
        Me.preocessingLabel.Size = New System.Drawing.Size(87, 13)
        Me.preocessingLabel.TabIndex = 0
        Me.preocessingLabel.Text = "Processing Step:"
        '
        'arrayMeasurementComboBox
        '
        Me.arrayMeasurementComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.arrayMeasurementComboBox.FormattingEnabled = True
        Me.arrayMeasurementComboBox.Location = New System.Drawing.Point(113, 43)
        Me.arrayMeasurementComboBox.Name = "arrayMeasurementComboBox"
        Me.arrayMeasurementComboBox.Size = New System.Drawing.Size(163, 21)
        Me.arrayMeasurementComboBox.TabIndex = 3
        '
        'processingStepComboBox
        '
        Me.processingStepComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.processingStepComboBox.FormattingEnabled = True
        Me.processingStepComboBox.Location = New System.Drawing.Point(113, 16)
        Me.processingStepComboBox.Name = "processingStepComboBox"
        Me.processingStepComboBox.Size = New System.Drawing.Size(163, 21)
        Me.processingStepComboBox.TabIndex = 1
        '
        'generalGroupBox
        '
        Me.generalGroupBox.Controls.Add(Me.timeoutNumeric)
        Me.generalGroupBox.Controls.Add(Me.channelTextBox)
        Me.generalGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.generalGroupBox.Controls.Add(Me.timeoutLabel)
        Me.generalGroupBox.Controls.Add(Me.channelNameLabel)
        Me.generalGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.generalGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.generalGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.generalGroupBox.Name = "generalGroupBox"
        Me.generalGroupBox.Size = New System.Drawing.Size(282, 100)
        Me.generalGroupBox.TabIndex = 0
        Me.generalGroupBox.TabStop = False
        Me.generalGroupBox.Text = "General"
        '
        'timeoutNumeric
        '
        Me.timeoutNumeric.DecimalPlaces = 2
        Me.timeoutNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.timeoutNumeric.Location = New System.Drawing.Point(176, 69)
        Me.timeoutNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.timeoutNumeric.Name = "timeoutNumeric"
        Me.timeoutNumeric.Size = New System.Drawing.Size(100, 20)
        Me.timeoutNumeric.TabIndex = 5
        '
        'channelTextBox
        '
        Me.channelTextBox.Location = New System.Drawing.Point(176, 43)
        Me.channelTextBox.Name = "channelTextBox"
        Me.channelTextBox.Size = New System.Drawing.Size(100, 20)
        Me.channelTextBox.TabIndex = 3
        Me.channelTextBox.Text = "0"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(176, 16)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(100, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'timeoutLabel
        '
        Me.timeoutLabel.AutoSize = True
        Me.timeoutLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.timeoutLabel.Location = New System.Drawing.Point(6, 74)
        Me.timeoutLabel.Name = "timeoutLabel"
        Me.timeoutLabel.Size = New System.Drawing.Size(62, 13)
        Me.timeoutLabel.TabIndex = 4
        Me.timeoutLabel.Text = "Timeout (s):"
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
        'filterComboBox
        '
        Me.filterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.filterComboBox.FormattingEnabled = True
        Me.filterComboBox.Location = New System.Drawing.Point(176, 16)
        Me.filterComboBox.Name = "filterComboBox"
        Me.filterComboBox.Size = New System.Drawing.Size(100, 21)
        Me.filterComboBox.TabIndex = 1
        '
        'filterTypeLabel
        '
        Me.filterTypeLabel.AutoSize = True
        Me.filterTypeLabel.Location = New System.Drawing.Point(6, 20)
        Me.filterTypeLabel.Name = "filterTypeLabel"
        Me.filterTypeLabel.Size = New System.Drawing.Size(59, 13)
        Me.filterTypeLabel.TabIndex = 0
        Me.filterTypeLabel.Text = "Filter Type:"
        '
        'lowPassFrequencyLabel
        '
        Me.lowPassFrequencyLabel.AutoSize = True
        Me.lowPassFrequencyLabel.Location = New System.Drawing.Point(6, 47)
        Me.lowPassFrequencyLabel.Name = "lowPassFrequencyLabel"
        Me.lowPassFrequencyLabel.Size = New System.Drawing.Size(158, 13)
        Me.lowPassFrequencyLabel.TabIndex = 2
        Me.lowPassFrequencyLabel.Text = "Low/High Pass Cutoff Freq (hz):"
        '
        'stopFrequencyLabel
        '
        Me.stopFrequencyLabel.AutoSize = True
        Me.stopFrequencyLabel.Location = New System.Drawing.Point(6, 73)
        Me.stopFrequencyLabel.Name = "stopFrequencyLabel"
        Me.stopFrequencyLabel.Size = New System.Drawing.Size(163, 13)
        Me.stopFrequencyLabel.TabIndex = 4
        Me.stopFrequencyLabel.Text = "BandPass/Stop Center Freq (hz):"
        '
        'stopWidthLabel
        '
        Me.stopWidthLabel.AutoSize = True
        Me.stopWidthLabel.Location = New System.Drawing.Point(6, 99)
        Me.stopWidthLabel.Name = "stopWidthLabel"
        Me.stopWidthLabel.Size = New System.Drawing.Size(141, 13)
        Me.stopWidthLabel.TabIndex = 6
        Me.stopWidthLabel.Text = "BandPass/BandStop Width:"
        '
        'cutoffFrequencyNumeric
        '
        Me.cutoffFrequencyNumeric.Location = New System.Drawing.Point(176, 43)
        Me.cutoffFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.cutoffFrequencyNumeric.Name = "cutoffFrequencyNumeric"
        Me.cutoffFrequencyNumeric.Size = New System.Drawing.Size(100, 20)
        Me.cutoffFrequencyNumeric.TabIndex = 3
        Me.cutoffFrequencyNumeric.Value = New Decimal(New Integer() {500000, 0, 0, 0})
        '
        'centerFrequencyNumeric
        '
        Me.centerFrequencyNumeric.Location = New System.Drawing.Point(176, 69)
        Me.centerFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.centerFrequencyNumeric.Name = "centerFrequencyNumeric"
        Me.centerFrequencyNumeric.Size = New System.Drawing.Size(100, 20)
        Me.centerFrequencyNumeric.TabIndex = 5
        Me.centerFrequencyNumeric.Value = New Decimal(New Integer() {100000, 0, 0, 0})
        '
        'bandpassWidthNumeric
        '
        Me.bandpassWidthNumeric.Location = New System.Drawing.Point(176, 95)
        Me.bandpassWidthNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.bandpassWidthNumeric.Name = "bandpassWidthNumeric"
        Me.bandpassWidthNumeric.Size = New System.Drawing.Size(100, 20)
        Me.bandpassWidthNumeric.TabIndex = 7
        Me.bandpassWidthNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'filterGroupBox
        '
        Me.filterGroupBox.Controls.Add(Me.bandpassWidthNumeric)
        Me.filterGroupBox.Controls.Add(Me.centerFrequencyNumeric)
        Me.filterGroupBox.Controls.Add(Me.cutoffFrequencyNumeric)
        Me.filterGroupBox.Controls.Add(Me.stopWidthLabel)
        Me.filterGroupBox.Controls.Add(Me.stopFrequencyLabel)
        Me.filterGroupBox.Controls.Add(Me.lowPassFrequencyLabel)
        Me.filterGroupBox.Controls.Add(Me.filterTypeLabel)
        Me.filterGroupBox.Controls.Add(Me.filterComboBox)
        Me.filterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.filterGroupBox.Location = New System.Drawing.Point(12, 339)
        Me.filterGroupBox.Name = "filterGroupBox"
        Me.filterGroupBox.Size = New System.Drawing.Size(282, 125)
        Me.filterGroupBox.TabIndex = 2
        Me.filterGroupBox.TabStop = False
        Me.filterGroupBox.Text = "Filter Parameters"
        '
        'messageTextBox
        '
        Me.messageTextBox.Location = New System.Drawing.Point(9, 19)
        Me.messageTextBox.Name = "messageTextBox"
        Me.messageTextBox.ReadOnly = True
        Me.messageTextBox.Size = New System.Drawing.Size(267, 35)
        Me.messageTextBox.TabIndex = 0
        Me.messageTextBox.Text = ""
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(12, 470)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(282, 60)
        Me.messageGroupBox.TabIndex = 3
        Me.messageGroupBox.TabStop = False
        Me.messageGroupBox.Text = "Message"
        '
        'sampledDataGridView
        '
        Me.sampledDataGridView.AllowUserToAddRows = False
        Me.sampledDataGridView.AllowUserToDeleteRows = False
        Me.sampledDataGridView.AllowUserToResizeRows = False
        Me.sampledDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.sampledDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.sampledDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.sampledDataGridView.DefaultCellStyle = DataGridViewCellStyle2
        Me.sampledDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.sampledDataGridView.Name = "sampledDataGridView"
        Me.sampledDataGridView.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.sampledDataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.sampledDataGridView.RowHeadersVisible = False
        Me.sampledDataGridView.RowHeadersWidth = 15
        Me.sampledDataGridView.RowTemplate.Height = 24
        Me.sampledDataGridView.Size = New System.Drawing.Size(190, 427)
        Me.sampledDataGridView.StandardTab = True
        Me.sampledDataGridView.TabIndex = 0
        '
        'sampledDataGroupBox
        '
        Me.sampledDataGroupBox.Controls.Add(Me.sampledDataGridView)
        Me.sampledDataGroupBox.Location = New System.Drawing.Point(300, 12)
        Me.sampledDataGroupBox.Name = "sampledDataGroupBox"
        Me.sampledDataGroupBox.Size = New System.Drawing.Size(202, 452)
        Me.sampledDataGroupBox.TabIndex = 4
        Me.sampledDataGroupBox.TabStop = False
        Me.sampledDataGroupBox.Text = "Sampled Data"
        '
        'measurementDataGridView
        '
        Me.measurementDataGridView.AllowUserToAddRows = False
        Me.measurementDataGridView.AllowUserToDeleteRows = False
        Me.measurementDataGridView.AllowUserToResizeRows = False
        Me.measurementDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.measurementDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.measurementDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.measurementDataGridView.DefaultCellStyle = DataGridViewCellStyle5
        Me.measurementDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.measurementDataGridView.Name = "measurementDataGridView"
        Me.measurementDataGridView.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.measurementDataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.measurementDataGridView.RowHeadersVisible = False
        Me.measurementDataGridView.RowHeadersWidth = 15
        Me.measurementDataGridView.RowTemplate.Height = 24
        Me.measurementDataGridView.Size = New System.Drawing.Size(190, 427)
        Me.measurementDataGridView.StandardTab = True
        Me.measurementDataGridView.TabIndex = 0
        '
        'measurementDataGroupBox
        '
        Me.measurementDataGroupBox.Controls.Add(Me.measurementDataGridView)
        Me.measurementDataGroupBox.Location = New System.Drawing.Point(508, 12)
        Me.measurementDataGroupBox.Name = "measurementDataGroupBox"
        Me.measurementDataGroupBox.Size = New System.Drawing.Size(202, 452)
        Me.measurementDataGroupBox.TabIndex = 5
        Me.measurementDataGroupBox.TabStop = False
        Me.measurementDataGroupBox.Text = "Measurement Data"
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(125, 19)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(75, 23)
        Me.acquireButton.TabIndex = 0
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.Location = New System.Drawing.Point(206, 19)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'buttonsGroupBox
        '
        Me.buttonsGroupBox.Controls.Add(Me.stopButton)
        Me.buttonsGroupBox.Controls.Add(Me.acquireButton)
        Me.buttonsGroupBox.Location = New System.Drawing.Point(300, 471)
        Me.buttonsGroupBox.Name = "buttonsGroupBox"
        Me.buttonsGroupBox.Size = New System.Drawing.Size(410, 59)
        Me.buttonsGroupBox.TabIndex = 6
        Me.buttonsGroupBox.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(723, 542)
        Me.Controls.Add(Me.buttonsGroupBox)
        Me.Controls.Add(Me.measurementDataGroupBox)
        Me.Controls.Add(Me.sampledDataGroupBox)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.measurementsGroupBox)
        Me.Controls.Add(Me.filterGroupBox)
        Me.Controls.Add(Me.generalGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Advanced Measurement Library"
        Me.measurementsGroupBox.ResumeLayout(False)
        Me.measurementsGroupBox.PerformLayout()
        Me.generalGroupBox.ResumeLayout(False)
        Me.generalGroupBox.PerformLayout()
        CType(Me.timeoutNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cutoffFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.centerFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bandpassWidthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.filterGroupBox.ResumeLayout(False)
        Me.filterGroupBox.PerformLayout()
        Me.messageGroupBox.ResumeLayout(False)
        CType(Me.sampledDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.sampledDataGroupBox.ResumeLayout(False)
        CType(Me.measurementDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementDataGroupBox.ResumeLayout(False)
        Me.buttonsGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private measurementsGroupBox As System.Windows.Forms.GroupBox
    Private noteTextBox As System.Windows.Forms.TextBox
    Private arrayMeasurementLabel As System.Windows.Forms.Label
    Private preocessingLabel As System.Windows.Forms.Label
    Private arrayMeasurementComboBox As System.Windows.Forms.ComboBox
    Private processingStepComboBox As System.Windows.Forms.ComboBox
    Private generalGroupBox As System.Windows.Forms.GroupBox
    Private channelTextBox As System.Windows.Forms.TextBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private timeoutLabel As System.Windows.Forms.Label
    Private channelNameLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private timeoutNumeric As System.Windows.Forms.NumericUpDown
    Private filterComboBox As System.Windows.Forms.ComboBox
    Private filterTypeLabel As System.Windows.Forms.Label
    Private lowPassFrequencyLabel As System.Windows.Forms.Label
    Private stopFrequencyLabel As System.Windows.Forms.Label
    Private stopWidthLabel As System.Windows.Forms.Label
    Private cutoffFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private centerFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private bandpassWidthNumeric As System.Windows.Forms.NumericUpDown
    Private filterGroupBox As System.Windows.Forms.GroupBox
    Private messageTextBox As System.Windows.Forms.RichTextBox
    Private messageGroupBox As System.Windows.Forms.GroupBox
    Private sampledDataGridView As System.Windows.Forms.DataGridView
    Private sampledDataGroupBox As System.Windows.Forms.GroupBox
    Private measurementDataGridView As System.Windows.Forms.DataGridView
    Private measurementDataGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private buttonsGroupBox As System.Windows.Forms.GroupBox
End Class
