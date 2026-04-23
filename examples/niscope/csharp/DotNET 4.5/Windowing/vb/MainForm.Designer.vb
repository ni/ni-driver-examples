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
        Me.windowLabel = New System.Windows.Forms.Label()
        Me.fftFunctionLabel = New System.Windows.Forms.Label()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.windowComboBox = New System.Windows.Forms.ComboBox()
        Me.scalarMeasurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.clearAveragingCheckBox = New System.Windows.Forms.CheckBox()
        Me.averageSpectrumCheckBox = New System.Windows.Forms.CheckBox()
        Me.fftFunctionComboBox = New System.Windows.Forms.ComboBox()
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox()
        Me.recordLengthMinNumeric = New System.Windows.Forms.NumericUpDown()
        Me.sampleRateMinNumeric = New System.Windows.Forms.NumericUpDown()
        Me.recordLengthMinLabel = New System.Windows.Forms.Label()
        Me.sampleRateMinLabel = New System.Windows.Forms.Label()
        Me.generalGroupBox = New System.Windows.Forms.GroupBox()
        Me.channelTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.RichTextBox()
        Me.sampledDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.waveformFromScopeDataGridView = New System.Windows.Forms.DataGridView()
        Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
        Me.spectrumDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.spectrumDataGridView = New System.Windows.Forms.DataGridView()
        Me.scalarMeasurementsGroupBox.SuspendLayout()
        Me.timingParametersGroupBox.SuspendLayout()
        CType(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.generalGroupBox.SuspendLayout()
        Me.messageGroupBox.SuspendLayout()
        Me.sampledDataGroupBox.SuspendLayout()
        CType(Me.waveformFromScopeDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.buttonsGroupBox.SuspendLayout()
        Me.spectrumDataGroupBox.SuspendLayout()
        CType(Me.spectrumDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'windowLabel
        '
        Me.windowLabel.AutoSize = True
        Me.windowLabel.Location = New System.Drawing.Point(6, 23)
        Me.windowLabel.Name = "windowLabel"
        Me.windowLabel.Size = New System.Drawing.Size(46, 13)
        Me.windowLabel.TabIndex = 12
        Me.windowLabel.Text = "Window"
        '
        'fftFunctionLabel
        '
        Me.fftFunctionLabel.AutoSize = True
        Me.fftFunctionLabel.Location = New System.Drawing.Point(6, 50)
        Me.fftFunctionLabel.Name = "fftFunctionLabel"
        Me.fftFunctionLabel.Size = New System.Drawing.Size(70, 13)
        Me.fftFunctionLabel.TabIndex = 13
        Me.fftFunctionLabel.Text = "FFT Function"
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
        Me.stopButton.Location = New System.Drawing.Point(128, 19)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'windowComboBox
        '
        Me.windowComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.windowComboBox.Location = New System.Drawing.Point(82, 19)
        Me.windowComboBox.Name = "windowComboBox"
        Me.windowComboBox.Size = New System.Drawing.Size(166, 21)
        Me.windowComboBox.TabIndex = 0
        '
        'scalarMeasurementsGroupBox
        '
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.clearAveragingCheckBox)
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.averageSpectrumCheckBox)
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.windowComboBox)
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.fftFunctionComboBox)
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.windowLabel)
        Me.scalarMeasurementsGroupBox.Controls.Add(Me.fftFunctionLabel)
        Me.scalarMeasurementsGroupBox.Location = New System.Drawing.Point(12, 92)
        Me.scalarMeasurementsGroupBox.Name = "scalarMeasurementsGroupBox"
        Me.scalarMeasurementsGroupBox.Size = New System.Drawing.Size(255, 122)
        Me.scalarMeasurementsGroupBox.TabIndex = 1
        Me.scalarMeasurementsGroupBox.TabStop = False
        Me.scalarMeasurementsGroupBox.Text = "Scalar Measurements"
        '
        'clearAveragingCheckBox
        '
        Me.clearAveragingCheckBox.AutoSize = True
        Me.clearAveragingCheckBox.Location = New System.Drawing.Point(6, 96)
        Me.clearAveragingCheckBox.Name = "clearAveragingCheckBox"
        Me.clearAveragingCheckBox.Size = New System.Drawing.Size(107, 17)
        Me.clearAveragingCheckBox.TabIndex = 3
        Me.clearAveragingCheckBox.Text = "Clear Averaging?"
        Me.clearAveragingCheckBox.UseVisualStyleBackColor = True
        '
        'averageSpectrumCheckBox
        '
        Me.averageSpectrumCheckBox.AutoSize = True
        Me.averageSpectrumCheckBox.Location = New System.Drawing.Point(6, 73)
        Me.averageSpectrumCheckBox.Name = "averageSpectrumCheckBox"
        Me.averageSpectrumCheckBox.Size = New System.Drawing.Size(120, 17)
        Me.averageSpectrumCheckBox.TabIndex = 2
        Me.averageSpectrumCheckBox.Text = "Average Spectrum?"
        Me.averageSpectrumCheckBox.UseVisualStyleBackColor = True
        '
        'fftFunctionComboBox
        '
        Me.fftFunctionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.fftFunctionComboBox.Location = New System.Drawing.Point(82, 46)
        Me.fftFunctionComboBox.Name = "fftFunctionComboBox"
        Me.fftFunctionComboBox.Size = New System.Drawing.Size(166, 21)
        Me.fftFunctionComboBox.TabIndex = 1
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.recordLengthMinNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleRateMinNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.recordLengthMinLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleRateMinLabel)
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(12, 225)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(255, 69)
        Me.timingParametersGroupBox.TabIndex = 2
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'recordLengthMinNumeric
        '
        Me.recordLengthMinNumeric.Location = New System.Drawing.Point(148, 19)
        Me.recordLengthMinNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.recordLengthMinNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.recordLengthMinNumeric.Name = "recordLengthMinNumeric"
        Me.recordLengthMinNumeric.Size = New System.Drawing.Size(100, 20)
        Me.recordLengthMinNumeric.TabIndex = 0
        Me.recordLengthMinNumeric.Value = New Decimal(New Integer() {4096, 0, 0, 0})
        '
        'sampleRateMinNumeric
        '
        Me.sampleRateMinNumeric.DecimalPlaces = 2
        Me.sampleRateMinNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.sampleRateMinNumeric.Location = New System.Drawing.Point(148, 45)
        Me.sampleRateMinNumeric.Maximum = New Decimal(New Integer() {-1247518720, 1073741819, 0, 0})
        Me.sampleRateMinNumeric.Minimum = New Decimal(New Integer() {-1247518720, 1073741819, 0, -2147483648})
        Me.sampleRateMinNumeric.Name = "sampleRateMinNumeric"
        Me.sampleRateMinNumeric.Size = New System.Drawing.Size(100, 20)
        Me.sampleRateMinNumeric.TabIndex = 1
        Me.sampleRateMinNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'recordLengthMinLabel
        '
        Me.recordLengthMinLabel.AutoSize = True
        Me.recordLengthMinLabel.Location = New System.Drawing.Point(6, 23)
        Me.recordLengthMinLabel.Name = "recordLengthMinLabel"
        Me.recordLengthMinLabel.Size = New System.Drawing.Size(125, 13)
        Me.recordLengthMinLabel.TabIndex = 2
        Me.recordLengthMinLabel.Text = "Minimum Record Length:"
        '
        'sampleRateMinLabel
        '
        Me.sampleRateMinLabel.AutoSize = True
        Me.sampleRateMinLabel.Location = New System.Drawing.Point(7, 49)
        Me.sampleRateMinLabel.Name = "sampleRateMinLabel"
        Me.sampleRateMinLabel.Size = New System.Drawing.Size(115, 13)
        Me.sampleRateMinLabel.TabIndex = 3
        Me.sampleRateMinLabel.Text = "Minimum Sample Rate:"
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
        Me.generalGroupBox.Size = New System.Drawing.Size(255, 69)
        Me.generalGroupBox.TabIndex = 0
        Me.generalGroupBox.TabStop = False
        Me.generalGroupBox.Text = "General"
        '
        'channelTextBox
        '
        Me.channelTextBox.Location = New System.Drawing.Point(148, 43)
        Me.channelTextBox.Name = "channelTextBox"
        Me.channelTextBox.Size = New System.Drawing.Size(100, 20)
        Me.channelTextBox.TabIndex = 3
        Me.channelTextBox.Text = "0"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(148, 16)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(100, 21)
        Me.resourceNameComboBox.TabIndex = 1
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
        Me.messageGroupBox.Location = New System.Drawing.Point(12, 305)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(255, 60)
        Me.messageGroupBox.TabIndex = 3
        Me.messageGroupBox.TabStop = False
        Me.messageGroupBox.Text = "Message"
        '
        'messageTextBox
        '
        Me.messageTextBox.Location = New System.Drawing.Point(9, 19)
        Me.messageTextBox.Name = "messageTextBox"
        Me.messageTextBox.ReadOnly = True
        Me.messageTextBox.Size = New System.Drawing.Size(239, 35)
        Me.messageTextBox.TabIndex = 0
        Me.messageTextBox.Text = ""
        '
        'sampledDataGroupBox
        '
        Me.sampledDataGroupBox.Controls.Add(Me.waveformFromScopeDataGridView)
        Me.sampledDataGroupBox.Location = New System.Drawing.Point(273, 12)
        Me.sampledDataGroupBox.Name = "sampledDataGroupBox"
        Me.sampledDataGroupBox.Size = New System.Drawing.Size(202, 416)
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
        Me.waveformFromScopeDataGridView.Size = New System.Drawing.Size(190, 391)
        Me.waveformFromScopeDataGridView.StandardTab = True
        Me.waveformFromScopeDataGridView.TabIndex = 0
        '
        'buttonsGroupBox
        '
        Me.buttonsGroupBox.Controls.Add(Me.acquireButton)
        Me.buttonsGroupBox.Controls.Add(Me.stopButton)
        Me.buttonsGroupBox.Location = New System.Drawing.Point(12, 371)
        Me.buttonsGroupBox.Name = "buttonsGroupBox"
        Me.buttonsGroupBox.Size = New System.Drawing.Size(255, 57)
        Me.buttonsGroupBox.TabIndex = 4
        Me.buttonsGroupBox.TabStop = False
        '
        'spectrumDataGroupBox
        '
        Me.spectrumDataGroupBox.Controls.Add(Me.spectrumDataGridView)
        Me.spectrumDataGroupBox.Location = New System.Drawing.Point(481, 12)
        Me.spectrumDataGroupBox.Name = "spectrumDataGroupBox"
        Me.spectrumDataGroupBox.Size = New System.Drawing.Size(202, 416)
        Me.spectrumDataGroupBox.TabIndex = 6
        Me.spectrumDataGroupBox.TabStop = False
        Me.spectrumDataGroupBox.Text = "Spectrum Data"
        '
        'spectrumDataGridView
        '
        Me.spectrumDataGridView.AllowUserToAddRows = False
        Me.spectrumDataGridView.AllowUserToDeleteRows = False
        Me.spectrumDataGridView.AllowUserToResizeRows = False
        Me.spectrumDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.spectrumDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.spectrumDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.spectrumDataGridView.DefaultCellStyle = DataGridViewCellStyle5
        Me.spectrumDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.spectrumDataGridView.Name = "spectrumDataGridView"
        Me.spectrumDataGridView.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.spectrumDataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.spectrumDataGridView.RowHeadersVisible = False
        Me.spectrumDataGridView.RowHeadersWidth = 15
        Me.spectrumDataGridView.RowTemplate.Height = 24
        Me.spectrumDataGridView.Size = New System.Drawing.Size(190, 391)
        Me.spectrumDataGridView.StandardTab = True
        Me.spectrumDataGridView.TabIndex = 0
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(695, 441)
        Me.Controls.Add(Me.spectrumDataGroupBox)
        Me.Controls.Add(Me.buttonsGroupBox)
        Me.Controls.Add(Me.sampledDataGroupBox)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.generalGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.scalarMeasurementsGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Windowing"
        Me.scalarMeasurementsGroupBox.ResumeLayout(False)
        Me.scalarMeasurementsGroupBox.PerformLayout()
        Me.timingParametersGroupBox.ResumeLayout(False)
        Me.timingParametersGroupBox.PerformLayout()
        CType(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.generalGroupBox.ResumeLayout(False)
        Me.generalGroupBox.PerformLayout()
        Me.messageGroupBox.ResumeLayout(False)
        Me.sampledDataGroupBox.ResumeLayout(False)
        CType(Me.waveformFromScopeDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.buttonsGroupBox.ResumeLayout(False)
        Me.spectrumDataGroupBox.ResumeLayout(False)
        CType(Me.spectrumDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private windowLabel As System.Windows.Forms.Label
    Private fftFunctionLabel As System.Windows.Forms.Label
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private windowComboBox As System.Windows.Forms.ComboBox
    Private fftFunctionComboBox As System.Windows.Forms.ComboBox
    Private scalarMeasurementsGroupBox As System.Windows.Forms.GroupBox
    Private timingParametersGroupBox As System.Windows.Forms.GroupBox
    Private recordLengthMinNumeric As System.Windows.Forms.NumericUpDown
    Private sampleRateMinNumeric As System.Windows.Forms.NumericUpDown
    Private recordLengthMinLabel As System.Windows.Forms.Label
    Private sampleRateMinLabel As System.Windows.Forms.Label
    Private averageSpectrumCheckBox As System.Windows.Forms.CheckBox
    Private clearAveragingCheckBox As System.Windows.Forms.CheckBox
    Private generalGroupBox As System.Windows.Forms.GroupBox
    Private channelTextBox As System.Windows.Forms.TextBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private channelNameLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private messageGroupBox As System.Windows.Forms.GroupBox
    Private messageTextBox As System.Windows.Forms.RichTextBox
    Private sampledDataGroupBox As System.Windows.Forms.GroupBox
    Private waveformFromScopeDataGridView As System.Windows.Forms.DataGridView
    Private buttonsGroupBox As System.Windows.Forms.GroupBox
    Private spectrumDataGroupBox As System.Windows.Forms.GroupBox
    Private spectrumDataGridView As System.Windows.Forms.DataGridView


End Class
