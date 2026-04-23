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
        Me.generalGroupBox = New System.Windows.Forms.GroupBox()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.offsetGroupBox = New System.Windows.Forms.GroupBox()
        Me.scaleOffsetTextBox = New System.Windows.Forms.TextBox()
        Me.scaleGainFactorTextBox = New System.Windows.Forms.TextBox()
        Me.triggerGroupBox = New System.Windows.Forms.GroupBox()
        Me.triggerTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerTypeLabel = New System.Windows.Forms.Label()
        Me.verticalGroupBox = New System.Windows.Forms.GroupBox()
        Me.verticalRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.verticalOffsetNumeric = New System.Windows.Forms.NumericUpDown()
        Me.offsetLabel = New System.Windows.Forms.Label()
        Me.rangeLabel = New System.Windows.Forms.Label()
        Me.horizontalGroupBox = New System.Windows.Forms.GroupBox()
        Me.minRecordLengthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.minSampleRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.recordLengthMinLabel = New System.Windows.Forms.Label()
        Me.sampleRateMinLabel = New System.Windows.Forms.Label()
        Me.sampleDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.scaledDataGridView = New System.Windows.Forms.DataGridView()
        Me.binaryDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.binaryDataGridView = New System.Windows.Forms.DataGridView()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.RichTextBox()
        Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
        Me.binaryDataSizeComboBox = New System.Windows.Forms.ComboBox()
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.gainFactorGroupBox = New System.Windows.Forms.GroupBox()
        Me.generalGroupBox.SuspendLayout()
        Me.offsetGroupBox.SuspendLayout()
        Me.triggerGroupBox.SuspendLayout()
        Me.verticalGroupBox.SuspendLayout()
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.verticalOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.horizontalGroupBox.SuspendLayout()
        CType(Me.minRecordLengthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.minSampleRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.sampleDataGroupBox.SuspendLayout()
        CType(Me.scaledDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.binaryDataGroupBox.SuspendLayout()
        CType(Me.binaryDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.messageGroupBox.SuspendLayout()
        Me.buttonsGroupBox.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        Me.gainFactorGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'generalGroupBox
        '
        Me.generalGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.generalGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.generalGroupBox.Controls.Add(Me.channelNameLabel)
        Me.generalGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.generalGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.generalGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.generalGroupBox.Name = "generalGroupBox"
        Me.generalGroupBox.Size = New System.Drawing.Size(248, 72)
        Me.generalGroupBox.TabIndex = 0
        Me.generalGroupBox.TabStop = False
        Me.generalGroupBox.Text = "General"
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(132, 46)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(100, 20)
        Me.channelNameTextBox.TabIndex = 1
        Me.channelNameTextBox.Text = "0"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(132, 19)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(100, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelNameLabel.Location = New System.Drawing.Point(6, 50)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(80, 13)
        Me.channelNameLabel.TabIndex = 3
        Me.channelNameLabel.Text = "Channel Name:"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 23)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 1
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'offsetGroupBox
        '
        Me.offsetGroupBox.Controls.Add(Me.scaleOffsetTextBox)
        Me.offsetGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.offsetGroupBox.Location = New System.Drawing.Point(266, 395)
        Me.offsetGroupBox.Name = "offsetGroupBox"
        Me.offsetGroupBox.Size = New System.Drawing.Size(410, 60)
        Me.offsetGroupBox.TabIndex = 9
        Me.offsetGroupBox.TabStop = False
        Me.offsetGroupBox.Text = "Offset"
        '
        'scaleOffsetTextBox
        '
        Me.scaleOffsetTextBox.Location = New System.Drawing.Point(6, 20)
        Me.scaleOffsetTextBox.Name = "scaleOffsetTextBox"
        Me.scaleOffsetTextBox.ReadOnly = True
        Me.scaleOffsetTextBox.Size = New System.Drawing.Size(398, 20)
        Me.scaleOffsetTextBox.TabIndex = 0
        '
        'scaleGainFactorTextBox
        '
        Me.scaleGainFactorTextBox.Location = New System.Drawing.Point(6, 20)
        Me.scaleGainFactorTextBox.Name = "scaleGainFactorTextBox"
        Me.scaleGainFactorTextBox.ReadOnly = True
        Me.scaleGainFactorTextBox.Size = New System.Drawing.Size(398, 20)
        Me.scaleGainFactorTextBox.TabIndex = 0
        '
        'triggerGroupBox
        '
        Me.triggerGroupBox.Controls.Add(Me.triggerTypeComboBox)
        Me.triggerGroupBox.Controls.Add(Me.triggerTypeLabel)
        Me.triggerGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.triggerGroupBox.Location = New System.Drawing.Point(12, 95)
        Me.triggerGroupBox.Name = "triggerGroupBox"
        Me.triggerGroupBox.Size = New System.Drawing.Size(248, 47)
        Me.triggerGroupBox.TabIndex = 1
        Me.triggerGroupBox.TabStop = False
        Me.triggerGroupBox.Text = "Trigger"
        '
        'triggerTypeComboBox
        '
        Me.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerTypeComboBox.FormattingEnabled = True
        Me.triggerTypeComboBox.Location = New System.Drawing.Point(142, 19)
        Me.triggerTypeComboBox.Name = "triggerTypeComboBox"
        Me.triggerTypeComboBox.Size = New System.Drawing.Size(100, 21)
        Me.triggerTypeComboBox.TabIndex = 0
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
        'verticalGroupBox
        '
        Me.verticalGroupBox.Controls.Add(Me.verticalRangeNumeric)
        Me.verticalGroupBox.Controls.Add(Me.verticalOffsetNumeric)
        Me.verticalGroupBox.Controls.Add(Me.offsetLabel)
        Me.verticalGroupBox.Controls.Add(Me.rangeLabel)
        Me.verticalGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.verticalGroupBox.Location = New System.Drawing.Point(12, 153)
        Me.verticalGroupBox.Name = "verticalGroupBox"
        Me.verticalGroupBox.Size = New System.Drawing.Size(248, 77)
        Me.verticalGroupBox.TabIndex = 2
        Me.verticalGroupBox.TabStop = False
        Me.verticalGroupBox.Text = "Vertical"
        '
        'verticalRangeNumeric
        '
        Me.verticalRangeNumeric.DecimalPlaces = 2
        Me.verticalRangeNumeric.Location = New System.Drawing.Point(142, 20)
        Me.verticalRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.verticalRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.verticalRangeNumeric.Name = "verticalRangeNumeric"
        Me.verticalRangeNumeric.Size = New System.Drawing.Size(100, 20)
        Me.verticalRangeNumeric.TabIndex = 0
        Me.verticalRangeNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'verticalOffsetNumeric
        '
        Me.verticalOffsetNumeric.DecimalPlaces = 4
        Me.verticalOffsetNumeric.Location = New System.Drawing.Point(142, 48)
        Me.verticalOffsetNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.verticalOffsetNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.verticalOffsetNumeric.Name = "verticalOffsetNumeric"
        Me.verticalOffsetNumeric.Size = New System.Drawing.Size(100, 20)
        Me.verticalOffsetNumeric.TabIndex = 1
        '
        'offsetLabel
        '
        Me.offsetLabel.AutoSize = True
        Me.offsetLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.offsetLabel.Location = New System.Drawing.Point(6, 52)
        Me.offsetLabel.Name = "offsetLabel"
        Me.offsetLabel.Size = New System.Drawing.Size(38, 13)
        Me.offsetLabel.TabIndex = 4
        Me.offsetLabel.Text = "Offset:"
        '
        'rangeLabel
        '
        Me.rangeLabel.AutoSize = True
        Me.rangeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.rangeLabel.Location = New System.Drawing.Point(6, 24)
        Me.rangeLabel.Name = "rangeLabel"
        Me.rangeLabel.Size = New System.Drawing.Size(42, 13)
        Me.rangeLabel.TabIndex = 4
        Me.rangeLabel.Text = "Range:"
        '
        'horizontalGroupBox
        '
        Me.horizontalGroupBox.Controls.Add(Me.minRecordLengthNumeric)
        Me.horizontalGroupBox.Controls.Add(Me.minSampleRateNumeric)
        Me.horizontalGroupBox.Controls.Add(Me.recordLengthMinLabel)
        Me.horizontalGroupBox.Controls.Add(Me.sampleRateMinLabel)
        Me.horizontalGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.horizontalGroupBox.Location = New System.Drawing.Point(12, 241)
        Me.horizontalGroupBox.Name = "horizontalGroupBox"
        Me.horizontalGroupBox.Size = New System.Drawing.Size(248, 72)
        Me.horizontalGroupBox.TabIndex = 3
        Me.horizontalGroupBox.TabStop = False
        Me.horizontalGroupBox.Text = "Horizontal"
        '
        'minRecordLengthNumeric
        '
        Me.minRecordLengthNumeric.Location = New System.Drawing.Point(142, 45)
        Me.minRecordLengthNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.minRecordLengthNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.minRecordLengthNumeric.Name = "minRecordLengthNumeric"
        Me.minRecordLengthNumeric.Size = New System.Drawing.Size(100, 20)
        Me.minRecordLengthNumeric.TabIndex = 1
        Me.minRecordLengthNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'minSampleRateNumeric
        '
        Me.minSampleRateNumeric.DecimalPlaces = 2
        Me.minSampleRateNumeric.Location = New System.Drawing.Point(142, 19)
        Me.minSampleRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.minSampleRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.minSampleRateNumeric.Name = "minSampleRateNumeric"
        Me.minSampleRateNumeric.Size = New System.Drawing.Size(100, 20)
        Me.minSampleRateNumeric.TabIndex = 0
        Me.minSampleRateNumeric.Value = New Decimal(New Integer() {20000000, 0, 0, 0})
        '
        'recordLengthMinLabel
        '
        Me.recordLengthMinLabel.AutoSize = True
        Me.recordLengthMinLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.recordLengthMinLabel.Location = New System.Drawing.Point(6, 49)
        Me.recordLengthMinLabel.Name = "recordLengthMinLabel"
        Me.recordLengthMinLabel.Size = New System.Drawing.Size(125, 13)
        Me.recordLengthMinLabel.TabIndex = 3
        Me.recordLengthMinLabel.Text = "Minimum Record Length:"
        '
        'sampleRateMinLabel
        '
        Me.sampleRateMinLabel.AutoSize = True
        Me.sampleRateMinLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.sampleRateMinLabel.Location = New System.Drawing.Point(6, 23)
        Me.sampleRateMinLabel.Name = "sampleRateMinLabel"
        Me.sampleRateMinLabel.Size = New System.Drawing.Size(115, 13)
        Me.sampleRateMinLabel.TabIndex = 3
        Me.sampleRateMinLabel.Text = "Minimum Sample Rate:"
        '
        'sampleDataGroupBox
        '
        Me.sampleDataGroupBox.Controls.Add(Me.scaledDataGridView)
        Me.sampleDataGroupBox.Location = New System.Drawing.Point(474, 12)
        Me.sampleDataGroupBox.Name = "sampleDataGroupBox"
        Me.sampleDataGroupBox.Size = New System.Drawing.Size(202, 372)
        Me.sampleDataGroupBox.TabIndex = 8
        Me.sampleDataGroupBox.TabStop = False
        Me.sampleDataGroupBox.Text = "Scaled Waveform"
        '
        'scaledDataGridView
        '
        Me.scaledDataGridView.AllowUserToAddRows = False
        Me.scaledDataGridView.AllowUserToDeleteRows = False
        Me.scaledDataGridView.AllowUserToResizeRows = False
        Me.scaledDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scaledDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.scaledDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.scaledDataGridView.Name = "scaledDataGridView"
        Me.scaledDataGridView.ReadOnly = True
        Me.scaledDataGridView.RowHeadersVisible = False
        Me.scaledDataGridView.RowHeadersWidth = 15
        Me.scaledDataGridView.RowTemplate.Height = 24
        Me.scaledDataGridView.Size = New System.Drawing.Size(190, 347)
        Me.scaledDataGridView.StandardTab = True
        Me.scaledDataGridView.TabIndex = 0
        '
        'binaryDataGroupBox
        '
        Me.binaryDataGroupBox.Controls.Add(Me.binaryDataGridView)
        Me.binaryDataGroupBox.Location = New System.Drawing.Point(266, 12)
        Me.binaryDataGroupBox.Name = "binaryDataGroupBox"
        Me.binaryDataGroupBox.Size = New System.Drawing.Size(202, 372)
        Me.binaryDataGroupBox.TabIndex = 7
        Me.binaryDataGroupBox.TabStop = False
        Me.binaryDataGroupBox.Text = "Binary Waveform"
        '
        'binaryDataGridView
        '
        Me.binaryDataGridView.AllowUserToAddRows = False
        Me.binaryDataGridView.AllowUserToDeleteRows = False
        Me.binaryDataGridView.AllowUserToResizeRows = False
        Me.binaryDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.binaryDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.binaryDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.binaryDataGridView.Name = "binaryDataGridView"
        Me.binaryDataGridView.ReadOnly = True
        Me.binaryDataGridView.RowHeadersVisible = False
        Me.binaryDataGridView.RowHeadersWidth = 15
        Me.binaryDataGridView.RowTemplate.Height = 24
        Me.binaryDataGridView.Size = New System.Drawing.Size(190, 347)
        Me.binaryDataGridView.StandardTab = True
        Me.binaryDataGridView.TabIndex = 0
        '
        'stopButton
        '
        Me.stopButton.Location = New System.Drawing.Point(132, 19)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(29, 19)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(75, 23)
        Me.acquireButton.TabIndex = 0
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(12, 324)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(248, 60)
        Me.messageGroupBox.TabIndex = 4
        Me.messageGroupBox.TabStop = False
        Me.messageGroupBox.Text = "Message"
        '
        'messageTextBox
        '
        Me.messageTextBox.Location = New System.Drawing.Point(9, 19)
        Me.messageTextBox.Name = "messageTextBox"
        Me.messageTextBox.ReadOnly = True
        Me.messageTextBox.Size = New System.Drawing.Size(233, 35)
        Me.messageTextBox.TabIndex = 0
        Me.messageTextBox.Text = ""
        '
        'buttonsGroupBox
        '
        Me.buttonsGroupBox.Controls.Add(Me.stopButton)
        Me.buttonsGroupBox.Controls.Add(Me.acquireButton)
        Me.buttonsGroupBox.Location = New System.Drawing.Point(12, 466)
        Me.buttonsGroupBox.Name = "buttonsGroupBox"
        Me.buttonsGroupBox.Size = New System.Drawing.Size(248, 50)
        Me.buttonsGroupBox.TabIndex = 6
        Me.buttonsGroupBox.TabStop = False
        '
        'binaryDataSizeComboBox
        '
        Me.binaryDataSizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.binaryDataSizeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.binaryDataSizeComboBox.FormattingEnabled = True
        Me.binaryDataSizeComboBox.Location = New System.Drawing.Point(6, 20)
        Me.binaryDataSizeComboBox.Name = "binaryDataSizeComboBox"
        Me.binaryDataSizeComboBox.Size = New System.Drawing.Size(100, 21)
        Me.binaryDataSizeComboBox.TabIndex = 0
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.binaryDataSizeComboBox)
        Me.groupBox1.Location = New System.Drawing.Point(12, 395)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(248, 60)
        Me.groupBox1.TabIndex = 5
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Binary Data Size (bits)"
        '
        'gainFactorGroupBox
        '
        Me.gainFactorGroupBox.Controls.Add(Me.scaleGainFactorTextBox)
        Me.gainFactorGroupBox.Location = New System.Drawing.Point(266, 466)
        Me.gainFactorGroupBox.Name = "gainFactorGroupBox"
        Me.gainFactorGroupBox.Size = New System.Drawing.Size(410, 50)
        Me.gainFactorGroupBox.TabIndex = 10
        Me.gainFactorGroupBox.TabStop = False
        Me.gainFactorGroupBox.Text = "Gain Factor"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(687, 528)
        Me.Controls.Add(Me.gainFactorGroupBox)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.buttonsGroupBox)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.sampleDataGroupBox)
        Me.Controls.Add(Me.binaryDataGroupBox)
        Me.Controls.Add(Me.verticalGroupBox)
        Me.Controls.Add(Me.horizontalGroupBox)
        Me.Controls.Add(Me.triggerGroupBox)
        Me.Controls.Add(Me.offsetGroupBox)
        Me.Controls.Add(Me.generalGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Binary Acquisition"
        Me.generalGroupBox.ResumeLayout(False)
        Me.generalGroupBox.PerformLayout()
        Me.offsetGroupBox.ResumeLayout(False)
        Me.offsetGroupBox.PerformLayout()
        Me.triggerGroupBox.ResumeLayout(False)
        Me.triggerGroupBox.PerformLayout()
        Me.verticalGroupBox.ResumeLayout(False)
        Me.verticalGroupBox.PerformLayout()
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.verticalOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.horizontalGroupBox.ResumeLayout(False)
        Me.horizontalGroupBox.PerformLayout()
        CType(Me.minRecordLengthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.minSampleRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.sampleDataGroupBox.ResumeLayout(False)
        CType(Me.scaledDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.binaryDataGroupBox.ResumeLayout(False)
        CType(Me.binaryDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.messageGroupBox.ResumeLayout(False)
        Me.buttonsGroupBox.ResumeLayout(False)
        Me.groupBox1.ResumeLayout(False)
        Me.gainFactorGroupBox.ResumeLayout(False)
        Me.gainFactorGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private generalGroupBox As System.Windows.Forms.GroupBox
    Private channelNameTextBox As System.Windows.Forms.TextBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private channelNameLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private offsetGroupBox As System.Windows.Forms.GroupBox
    Private scaleGainFactorTextBox As System.Windows.Forms.TextBox
    Private scaleOffsetTextBox As System.Windows.Forms.TextBox
    Private triggerGroupBox As System.Windows.Forms.GroupBox
    Private triggerTypeLabel As System.Windows.Forms.Label
    Private verticalGroupBox As System.Windows.Forms.GroupBox
    Private offsetLabel As System.Windows.Forms.Label
    Private rangeLabel As System.Windows.Forms.Label
    Private horizontalGroupBox As System.Windows.Forms.GroupBox
    Private recordLengthMinLabel As System.Windows.Forms.Label
    Private sampleRateMinLabel As System.Windows.Forms.Label
    Private triggerTypeComboBox As System.Windows.Forms.ComboBox
    Private verticalRangeNumeric As System.Windows.Forms.NumericUpDown
    Private verticalOffsetNumeric As System.Windows.Forms.NumericUpDown
    Private minRecordLengthNumeric As System.Windows.Forms.NumericUpDown
    Private minSampleRateNumeric As System.Windows.Forms.NumericUpDown
    Private sampleDataGroupBox As System.Windows.Forms.GroupBox
    Private scaledDataGridView As System.Windows.Forms.DataGridView
    Private binaryDataGroupBox As System.Windows.Forms.GroupBox
    Private binaryDataGridView As System.Windows.Forms.DataGridView
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private messageGroupBox As System.Windows.Forms.GroupBox
    Private messageTextBox As System.Windows.Forms.RichTextBox
    Private buttonsGroupBox As System.Windows.Forms.GroupBox
    Private binaryDataSizeComboBox As System.Windows.Forms.ComboBox
    Private groupBox1 As System.Windows.Forms.GroupBox
    Private gainFactorGroupBox As System.Windows.Forms.GroupBox



End Class
