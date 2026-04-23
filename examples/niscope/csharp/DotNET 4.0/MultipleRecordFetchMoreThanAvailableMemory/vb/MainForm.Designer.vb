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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.verticalRangeLabel = New System.Windows.Forms.Label()
        Me.minSampleRateLabel = New System.Windows.Forms.Label()
        Me.minRecordLengthLabel = New System.Windows.Forms.Label()
        Me.numOfRecordLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.verticalRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.minSampleRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.minRecordLengthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.numOfRecordNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.numRecordsFetchedTextBox = New System.Windows.Forms.TextBox()
        Me.numRecordsAcquiredTextBox = New System.Windows.Forms.TextBox()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.generalGroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.attributesGroupBox = New System.Windows.Forms.GroupBox()
        Me.dataRecordsGroupBox = New System.Windows.Forms.GroupBox()
        Me.allowMoreRecsThanAvaiMemCheckBox = New System.Windows.Forms.CheckBox()
        Me.NumOfRecordsAcquiredlabel = New System.Windows.Forms.Label()
        Me.numOfRecFetchedLabel = New System.Windows.Forms.Label()
        Me.sampledDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.sampledDataGridView = New System.Windows.Forms.DataGridView()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.RichTextBox()
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.minSampleRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.minRecordLengthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numOfRecordNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.generalGroupBox.SuspendLayout()
        Me.attributesGroupBox.SuspendLayout()
        Me.dataRecordsGroupBox.SuspendLayout()
        Me.sampledDataGroupBox.SuspendLayout()
        CType(Me.sampledDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.messageGroupBox.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.Location = New System.Drawing.Point(6, 50)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(80, 13)
        Me.channelNameLabel.TabIndex = 2
        Me.channelNameLabel.Text = "Channel Name:"
        '
        'verticalRangeLabel
        '
        Me.verticalRangeLabel.AutoSize = True
        Me.verticalRangeLabel.Location = New System.Drawing.Point(6, 23)
        Me.verticalRangeLabel.Name = "verticalRangeLabel"
        Me.verticalRangeLabel.Size = New System.Drawing.Size(80, 13)
        Me.verticalRangeLabel.TabIndex = 0
        Me.verticalRangeLabel.Text = "Vertical Range:"
        '
        'minSampleRateLabel
        '
        Me.minSampleRateLabel.AutoSize = True
        Me.minSampleRateLabel.Location = New System.Drawing.Point(6, 49)
        Me.minSampleRateLabel.Name = "minSampleRateLabel"
        Me.minSampleRateLabel.Size = New System.Drawing.Size(91, 13)
        Me.minSampleRateLabel.TabIndex = 2
        Me.minSampleRateLabel.Text = "Min Sample Rate:"
        '
        'minRecordLengthLabel
        '
        Me.minRecordLengthLabel.AutoSize = True
        Me.minRecordLengthLabel.Location = New System.Drawing.Point(6, 75)
        Me.minRecordLengthLabel.Name = "minRecordLengthLabel"
        Me.minRecordLengthLabel.Size = New System.Drawing.Size(101, 13)
        Me.minRecordLengthLabel.TabIndex = 4
        Me.minRecordLengthLabel.Text = "Min Record Length:"
        '
        'numOfRecordLabel
        '
        Me.numOfRecordLabel.AutoSize = True
        Me.numOfRecordLabel.Location = New System.Drawing.Point(6, 46)
        Me.numOfRecordLabel.Name = "numOfRecordLabel"
        Me.numOfRecordLabel.Size = New System.Drawing.Size(102, 13)
        Me.numOfRecordLabel.TabIndex = 4
        Me.numOfRecordLabel.Text = "Number of Records:"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 23)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'verticalRangeNumeric
        '
        Me.verticalRangeNumeric.DecimalPlaces = 2
        Me.verticalRangeNumeric.Location = New System.Drawing.Point(144, 19)
        Me.verticalRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.verticalRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.verticalRangeNumeric.Name = "verticalRangeNumeric"
        Me.verticalRangeNumeric.Size = New System.Drawing.Size(101, 20)
        Me.verticalRangeNumeric.TabIndex = 1
        Me.verticalRangeNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'minSampleRateNumeric
        '
        Me.minSampleRateNumeric.DecimalPlaces = 2
        Me.minSampleRateNumeric.Location = New System.Drawing.Point(144, 45)
        Me.minSampleRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.minSampleRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.minSampleRateNumeric.Name = "minSampleRateNumeric"
        Me.minSampleRateNumeric.Size = New System.Drawing.Size(101, 20)
        Me.minSampleRateNumeric.TabIndex = 3
        Me.minSampleRateNumeric.Value = New Decimal(New Integer() {10000000, 0, 0, 0})
        '
        'minRecordLengthNumeric
        '
        Me.minRecordLengthNumeric.Location = New System.Drawing.Point(144, 71)
        Me.minRecordLengthNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.minRecordLengthNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.minRecordLengthNumeric.Name = "minRecordLengthNumeric"
        Me.minRecordLengthNumeric.Size = New System.Drawing.Size(101, 20)
        Me.minRecordLengthNumeric.TabIndex = 5
        Me.minRecordLengthNumeric.Value = New Decimal(New Integer() {8192, 0, 0, 0})
        '
        'numOfRecordNumeric
        '
        Me.numOfRecordNumeric.Location = New System.Drawing.Point(157, 42)
        Me.numOfRecordNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numOfRecordNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.numOfRecordNumeric.Name = "numOfRecordNumeric"
        Me.numOfRecordNumeric.Size = New System.Drawing.Size(88, 20)
        Me.numOfRecordNumeric.TabIndex = 1
        Me.numOfRecordNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(144, 46)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(101, 20)
        Me.channelNameTextBox.TabIndex = 3
        Me.channelNameTextBox.Text = "0"
        '
        'numRecordsFetchedTextBox
        '
        Me.numRecordsFetchedTextBox.Location = New System.Drawing.Point(157, 68)
        Me.numRecordsFetchedTextBox.Name = "numRecordsFetchedTextBox"
        Me.numRecordsFetchedTextBox.ReadOnly = True
        Me.numRecordsFetchedTextBox.Size = New System.Drawing.Size(88, 20)
        Me.numRecordsFetchedTextBox.TabIndex = 2
        Me.numRecordsFetchedTextBox.Text = "0"
        '
        'numRecordsAcquiredTextBox
        '
        Me.numRecordsAcquiredTextBox.Location = New System.Drawing.Point(157, 94)
        Me.numRecordsAcquiredTextBox.Name = "numRecordsAcquiredTextBox"
        Me.numRecordsAcquiredTextBox.ReadOnly = True
        Me.numRecordsAcquiredTextBox.Size = New System.Drawing.Size(88, 20)
        Me.numRecordsAcquiredTextBox.TabIndex = 3
        Me.numRecordsAcquiredTextBox.Text = "0"
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(52, 19)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(75, 23)
        Me.acquireButton.TabIndex = 0
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.Location = New System.Drawing.Point(133, 19)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'generalGroupBox
        '
        Me.generalGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.generalGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.generalGroupBox.Controls.Add(Me.channelNameLabel)
        Me.generalGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.generalGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.generalGroupBox.Name = "generalGroupBox"
        Me.generalGroupBox.Size = New System.Drawing.Size(251, 74)
        Me.generalGroupBox.TabIndex = 0
        Me.generalGroupBox.TabStop = False
        Me.generalGroupBox.Text = "General"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(144, 19)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(101, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'attributesGroupBox
        '
        Me.attributesGroupBox.Controls.Add(Me.verticalRangeNumeric)
        Me.attributesGroupBox.Controls.Add(Me.minSampleRateNumeric)
        Me.attributesGroupBox.Controls.Add(Me.minRecordLengthNumeric)
        Me.attributesGroupBox.Controls.Add(Me.verticalRangeLabel)
        Me.attributesGroupBox.Controls.Add(Me.minSampleRateLabel)
        Me.attributesGroupBox.Controls.Add(Me.minRecordLengthLabel)
        Me.attributesGroupBox.Location = New System.Drawing.Point(12, 97)
        Me.attributesGroupBox.Name = "attributesGroupBox"
        Me.attributesGroupBox.Size = New System.Drawing.Size(251, 98)
        Me.attributesGroupBox.TabIndex = 1
        Me.attributesGroupBox.TabStop = False
        Me.attributesGroupBox.Text = "Parameters"
        '
        'dataRecordsGroupBox
        '
        Me.dataRecordsGroupBox.Controls.Add(Me.allowMoreRecsThanAvaiMemCheckBox)
        Me.dataRecordsGroupBox.Controls.Add(Me.NumOfRecordsAcquiredlabel)
        Me.dataRecordsGroupBox.Controls.Add(Me.numOfRecFetchedLabel)
        Me.dataRecordsGroupBox.Controls.Add(Me.numRecordsFetchedTextBox)
        Me.dataRecordsGroupBox.Controls.Add(Me.numRecordsAcquiredTextBox)
        Me.dataRecordsGroupBox.Controls.Add(Me.numOfRecordNumeric)
        Me.dataRecordsGroupBox.Controls.Add(Me.numOfRecordLabel)
        Me.dataRecordsGroupBox.Location = New System.Drawing.Point(12, 205)
        Me.dataRecordsGroupBox.Name = "dataRecordsGroupBox"
        Me.dataRecordsGroupBox.Size = New System.Drawing.Size(251, 122)
        Me.dataRecordsGroupBox.TabIndex = 2
        Me.dataRecordsGroupBox.TabStop = False
        Me.dataRecordsGroupBox.Text = "Data Records"
        '
        'allowMoreRecsThanAvaiMemCheckBox
        '
        Me.allowMoreRecsThanAvaiMemCheckBox.AutoSize = True
        Me.allowMoreRecsThanAvaiMemCheckBox.Location = New System.Drawing.Point(6, 19)
        Me.allowMoreRecsThanAvaiMemCheckBox.Name = "allowMoreRecsThanAvaiMemCheckBox"
        Me.allowMoreRecsThanAvaiMemCheckBox.Size = New System.Drawing.Size(229, 17)
        Me.allowMoreRecsThanAvaiMemCheckBox.TabIndex = 0
        Me.allowMoreRecsThanAvaiMemCheckBox.Text = "Allow more records than available memory?"
        Me.allowMoreRecsThanAvaiMemCheckBox.UseVisualStyleBackColor = True
        '
        'NumOfRecordsAcquiredlabel
        '
        Me.NumOfRecordsAcquiredlabel.AutoSize = True
        Me.NumOfRecordsAcquiredlabel.Location = New System.Drawing.Point(6, 98)
        Me.NumOfRecordsAcquiredlabel.Name = "NumOfRecordsAcquiredlabel"
        Me.NumOfRecordsAcquiredlabel.Size = New System.Drawing.Size(147, 13)
        Me.NumOfRecordsAcquiredlabel.TabIndex = 8
        Me.NumOfRecordsAcquiredlabel.Text = "Number of Records Acquired:"
        '
        'numOfRecFetchedLabel
        '
        Me.numOfRecFetchedLabel.AutoSize = True
        Me.numOfRecFetchedLabel.Location = New System.Drawing.Point(6, 72)
        Me.numOfRecFetchedLabel.Name = "numOfRecFetchedLabel"
        Me.numOfRecFetchedLabel.Size = New System.Drawing.Size(144, 13)
        Me.numOfRecFetchedLabel.TabIndex = 6
        Me.numOfRecFetchedLabel.Text = "Number of Records Fetched:"
        '
        'sampledDataGroupBox
        '
        Me.sampledDataGroupBox.Controls.Add(Me.sampledDataGridView)
        Me.sampledDataGroupBox.Location = New System.Drawing.Point(269, 12)
        Me.sampledDataGroupBox.Name = "sampledDataGroupBox"
        Me.sampledDataGroupBox.Size = New System.Drawing.Size(202, 443)
        Me.sampledDataGroupBox.TabIndex = 5
        Me.sampledDataGroupBox.TabStop = False
        Me.sampledDataGroupBox.Text = "Sampled Data"
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
        Me.sampledDataGridView.Size = New System.Drawing.Size(190, 418)
        Me.sampledDataGridView.StandardTab = True
        Me.sampledDataGridView.TabIndex = 0
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(12, 333)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(251, 62)
        Me.messageGroupBox.TabIndex = 3
        Me.messageGroupBox.TabStop = False
        Me.messageGroupBox.Text = "Message"
        '
        'messageTextBox
        '
        Me.messageTextBox.Location = New System.Drawing.Point(9, 19)
        Me.messageTextBox.Name = "messageTextBox"
        Me.messageTextBox.ReadOnly = True
        Me.messageTextBox.Size = New System.Drawing.Size(236, 36)
        Me.messageTextBox.TabIndex = 0
        Me.messageTextBox.Text = ""
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.acquireButton)
        Me.groupBox1.Controls.Add(Me.stopButton)
        Me.groupBox1.Location = New System.Drawing.Point(12, 401)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(251, 54)
        Me.groupBox1.TabIndex = 4
        Me.groupBox1.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(482, 468)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.sampledDataGroupBox)
        Me.Controls.Add(Me.generalGroupBox)
        Me.Controls.Add(Me.attributesGroupBox)
        Me.Controls.Add(Me.dataRecordsGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Multiple Record Fetch More Than Available Memory"
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.minSampleRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.minRecordLengthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numOfRecordNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.generalGroupBox.ResumeLayout(False)
        Me.generalGroupBox.PerformLayout()
        Me.attributesGroupBox.ResumeLayout(False)
        Me.attributesGroupBox.PerformLayout()
        Me.dataRecordsGroupBox.ResumeLayout(False)
        Me.dataRecordsGroupBox.PerformLayout()
        Me.sampledDataGroupBox.ResumeLayout(False)
        CType(Me.sampledDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.messageGroupBox.ResumeLayout(False)
        Me.groupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	#End Region

	Private channelNameLabel As System.Windows.Forms.Label
	Private verticalRangeLabel As System.Windows.Forms.Label
	Private minSampleRateLabel As System.Windows.Forms.Label
	Private minRecordLengthLabel As System.Windows.Forms.Label
	Private numOfRecordLabel As System.Windows.Forms.Label
	Private resourceNameLabel As System.Windows.Forms.Label
	Private verticalRangeNumeric As System.Windows.Forms.NumericUpDown
	Private minSampleRateNumeric As System.Windows.Forms.NumericUpDown
	Private minRecordLengthNumeric As System.Windows.Forms.NumericUpDown
	Private numOfRecordNumeric As System.Windows.Forms.NumericUpDown
	Private channelNameTextBox As System.Windows.Forms.TextBox
	Private numRecordsFetchedTextBox As System.Windows.Forms.TextBox
	Private numRecordsAcquiredTextBox As System.Windows.Forms.TextBox
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
	Private generalGroupBox As System.Windows.Forms.GroupBox
	Private attributesGroupBox As System.Windows.Forms.GroupBox
	Private dataRecordsGroupBox As System.Windows.Forms.GroupBox
	Private NumOfRecordsAcquiredlabel As System.Windows.Forms.Label
	Private numOfRecFetchedLabel As System.Windows.Forms.Label
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private allowMoreRecsThanAvaiMemCheckBox As System.Windows.Forms.CheckBox
	Private sampledDataGroupBox As System.Windows.Forms.GroupBox
	Private sampledDataGridView As System.Windows.Forms.DataGridView
	Private messageGroupBox As System.Windows.Forms.GroupBox
	Private messageTextBox As System.Windows.Forms.RichTextBox
	Private groupBox1 As System.Windows.Forms.GroupBox


End Class
