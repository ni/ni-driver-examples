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
        Me.numOfRecLabel = New System.Windows.Forms.Label()
        Me.verticalRangeLabel = New System.Windows.Forms.Label()
        Me.minSampleRateLabel = New System.Windows.Forms.Label()
        Me.minRecordLengthLabel = New System.Windows.Forms.Label()
        Me.numOfRecordsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.verticalRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.minSampleRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.minRecordLengthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.verticalAndHorizontalGroupBox = New System.Windows.Forms.GroupBox()
        Me.generalGroupBox = New System.Windows.Forms.GroupBox()
        Me.plotRelativeToLabel = New System.Windows.Forms.Label()
        Me.plotRelativeToComboBox = New System.Windows.Forms.ComboBox()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.sampledDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.sampledDataGridView = New System.Windows.Forms.DataGridView()
        Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.numOfRecordsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.minSampleRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.minRecordLengthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.verticalAndHorizontalGroupBox.SuspendLayout()
        Me.generalGroupBox.SuspendLayout()
        Me.sampledDataGroupBox.SuspendLayout()
        CType(Me.sampledDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.buttonsGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'numOfRecLabel
        '
        Me.numOfRecLabel.AutoSize = True
        Me.numOfRecLabel.Location = New System.Drawing.Point(6, 21)
        Me.numOfRecLabel.Name = "numOfRecLabel"
        Me.numOfRecLabel.Size = New System.Drawing.Size(97, 13)
        Me.numOfRecLabel.TabIndex = 3
        Me.numOfRecLabel.Text = "Number of records:"
        '
        'verticalRangeLabel
        '
        Me.verticalRangeLabel.AutoSize = True
        Me.verticalRangeLabel.Location = New System.Drawing.Point(6, 49)
        Me.verticalRangeLabel.Name = "verticalRangeLabel"
        Me.verticalRangeLabel.Size = New System.Drawing.Size(75, 13)
        Me.verticalRangeLabel.TabIndex = 4
        Me.verticalRangeLabel.Text = "Vertical range:"
        '
        'minSampleRateLabel
        '
        Me.minSampleRateLabel.AutoSize = True
        Me.minSampleRateLabel.Location = New System.Drawing.Point(6, 75)
        Me.minSampleRateLabel.Name = "minSampleRateLabel"
        Me.minSampleRateLabel.Size = New System.Drawing.Size(84, 13)
        Me.minSampleRateLabel.TabIndex = 5
        Me.minSampleRateLabel.Text = "Min sample rate:"
        '
        'minRecordLengthLabel
        '
        Me.minRecordLengthLabel.AutoSize = True
        Me.minRecordLengthLabel.Location = New System.Drawing.Point(6, 101)
        Me.minRecordLengthLabel.Name = "minRecordLengthLabel"
        Me.minRecordLengthLabel.Size = New System.Drawing.Size(92, 13)
        Me.minRecordLengthLabel.TabIndex = 6
        Me.minRecordLengthLabel.Text = "Min record length:"
        '
        'numOfRecordsNumeric
        '
        Me.numOfRecordsNumeric.Location = New System.Drawing.Point(123, 19)
        Me.numOfRecordsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numOfRecordsNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.numOfRecordsNumeric.Name = "numOfRecordsNumeric"
        Me.numOfRecordsNumeric.Size = New System.Drawing.Size(100, 20)
        Me.numOfRecordsNumeric.TabIndex = 0
        Me.numOfRecordsNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'verticalRangeNumeric
        '
        Me.verticalRangeNumeric.DecimalPlaces = 2
        Me.verticalRangeNumeric.Location = New System.Drawing.Point(123, 45)
        Me.verticalRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.verticalRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.verticalRangeNumeric.Name = "verticalRangeNumeric"
        Me.verticalRangeNumeric.Size = New System.Drawing.Size(100, 20)
        Me.verticalRangeNumeric.TabIndex = 1
        Me.verticalRangeNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'minSampleRateNumeric
        '
        Me.minSampleRateNumeric.DecimalPlaces = 2
        Me.minSampleRateNumeric.Location = New System.Drawing.Point(123, 71)
        Me.minSampleRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.minSampleRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.minSampleRateNumeric.Name = "minSampleRateNumeric"
        Me.minSampleRateNumeric.Size = New System.Drawing.Size(100, 20)
        Me.minSampleRateNumeric.TabIndex = 2
        Me.minSampleRateNumeric.Value = New Decimal(New Integer() {10000000, 0, 0, 0})
        '
        'minRecordLengthNumeric
        '
        Me.minRecordLengthNumeric.Location = New System.Drawing.Point(123, 97)
        Me.minRecordLengthNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.minRecordLengthNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.minRecordLengthNumeric.Name = "minRecordLengthNumeric"
        Me.minRecordLengthNumeric.Size = New System.Drawing.Size(100, 20)
        Me.minRecordLengthNumeric.TabIndex = 3
        Me.minRecordLengthNumeric.Value = New Decimal(New Integer() {256, 0, 0, 0})
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(81, 19)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(75, 23)
        Me.acquireButton.TabIndex = 0
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'verticalAndHorizontalGroupBox
        '
        Me.verticalAndHorizontalGroupBox.Controls.Add(Me.numOfRecordsNumeric)
        Me.verticalAndHorizontalGroupBox.Controls.Add(Me.verticalRangeNumeric)
        Me.verticalAndHorizontalGroupBox.Controls.Add(Me.minSampleRateNumeric)
        Me.verticalAndHorizontalGroupBox.Controls.Add(Me.minRecordLengthNumeric)
        Me.verticalAndHorizontalGroupBox.Controls.Add(Me.numOfRecLabel)
        Me.verticalAndHorizontalGroupBox.Controls.Add(Me.verticalRangeLabel)
        Me.verticalAndHorizontalGroupBox.Controls.Add(Me.minSampleRateLabel)
        Me.verticalAndHorizontalGroupBox.Controls.Add(Me.minRecordLengthLabel)
        Me.verticalAndHorizontalGroupBox.Location = New System.Drawing.Point(12, 120)
        Me.verticalAndHorizontalGroupBox.Name = "verticalAndHorizontalGroupBox"
        Me.verticalAndHorizontalGroupBox.Size = New System.Drawing.Size(229, 124)
        Me.verticalAndHorizontalGroupBox.TabIndex = 1
        Me.verticalAndHorizontalGroupBox.TabStop = False
        Me.verticalAndHorizontalGroupBox.Text = "Vertical and horizontal"
        '
        'generalGroupBox
        '
        Me.generalGroupBox.Controls.Add(Me.plotRelativeToLabel)
        Me.generalGroupBox.Controls.Add(Me.plotRelativeToComboBox)
        Me.generalGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.generalGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.generalGroupBox.Controls.Add(Me.channelNameLabel)
        Me.generalGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.generalGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.generalGroupBox.Name = "generalGroupBox"
        Me.generalGroupBox.Size = New System.Drawing.Size(229, 97)
        Me.generalGroupBox.TabIndex = 0
        Me.generalGroupBox.TabStop = False
        Me.generalGroupBox.Text = "General"
        '
        'plotRelativeToLabel
        '
        Me.plotRelativeToLabel.AutoSize = True
        Me.plotRelativeToLabel.Location = New System.Drawing.Point(6, 76)
        Me.plotRelativeToLabel.Name = "plotRelativeToLabel"
        Me.plotRelativeToLabel.Size = New System.Drawing.Size(86, 13)
        Me.plotRelativeToLabel.TabIndex = 19
        Me.plotRelativeToLabel.Text = "Plot Relative To:"
        '
        'plotRelativeToComboBox
        '
        Me.plotRelativeToComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.plotRelativeToComboBox.FormattingEnabled = True
        Me.plotRelativeToComboBox.Location = New System.Drawing.Point(123, 72)
        Me.plotRelativeToComboBox.Name = "plotRelativeToComboBox"
        Me.plotRelativeToComboBox.Size = New System.Drawing.Size(100, 21)
        Me.plotRelativeToComboBox.TabIndex = 2
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(123, 46)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(100, 20)
        Me.channelNameTextBox.TabIndex = 1
        Me.channelNameTextBox.Text = "0"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(123, 19)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(100, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.Location = New System.Drawing.Point(6, 50)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(80, 13)
        Me.channelNameLabel.TabIndex = 14
        Me.channelNameLabel.Text = "Channel Name:"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 23)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 15
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'sampledDataGroupBox
        '
        Me.sampledDataGroupBox.Controls.Add(Me.sampledDataGridView)
        Me.sampledDataGroupBox.Location = New System.Drawing.Point(247, 12)
        Me.sampledDataGroupBox.Name = "sampledDataGroupBox"
        Me.sampledDataGroupBox.Size = New System.Drawing.Size(327, 292)
        Me.sampledDataGroupBox.TabIndex = 3
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
        Me.sampledDataGridView.Size = New System.Drawing.Size(315, 267)
        Me.sampledDataGridView.StandardTab = True
        Me.sampledDataGridView.TabIndex = 0
        '
        'buttonsGroupBox
        '
        Me.buttonsGroupBox.Controls.Add(Me.acquireButton)
        Me.buttonsGroupBox.Location = New System.Drawing.Point(12, 250)
        Me.buttonsGroupBox.Name = "buttonsGroupBox"
        Me.buttonsGroupBox.Size = New System.Drawing.Size(229, 54)
        Me.buttonsGroupBox.TabIndex = 2
        Me.buttonsGroupBox.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(589, 316)
        Me.Controls.Add(Me.buttonsGroupBox)
        Me.Controls.Add(Me.sampledDataGroupBox)
        Me.Controls.Add(Me.verticalAndHorizontalGroupBox)
        Me.Controls.Add(Me.generalGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Multiple Record"
        CType(Me.numOfRecordsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.minSampleRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.minRecordLengthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.verticalAndHorizontalGroupBox.ResumeLayout(False)
        Me.verticalAndHorizontalGroupBox.PerformLayout()
        Me.generalGroupBox.ResumeLayout(False)
        Me.generalGroupBox.PerformLayout()
        Me.sampledDataGroupBox.ResumeLayout(False)
        CType(Me.sampledDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.buttonsGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	#End Region

	Private numOfRecLabel As System.Windows.Forms.Label
	Private verticalRangeLabel As System.Windows.Forms.Label
	Private minSampleRateLabel As System.Windows.Forms.Label
	Private minRecordLengthLabel As System.Windows.Forms.Label
	Private numOfRecordsNumeric As System.Windows.Forms.NumericUpDown
	Private verticalRangeNumeric As System.Windows.Forms.NumericUpDown
	Private minSampleRateNumeric As System.Windows.Forms.NumericUpDown
	Private minRecordLengthNumeric As System.Windows.Forms.NumericUpDown
    'Private acquireButton As System.Windows.Forms.Button
    Private WithEvents acquireButton As System.Windows.Forms.Button
	Private verticalAndHorizontalGroupBox As System.Windows.Forms.GroupBox
	Private generalGroupBox As System.Windows.Forms.GroupBox
	Private plotRelativeToLabel As System.Windows.Forms.Label
	Private plotRelativeToComboBox As System.Windows.Forms.ComboBox
	Private channelNameTextBox As System.Windows.Forms.TextBox
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private channelNameLabel As System.Windows.Forms.Label
	Private resourceNameLabel As System.Windows.Forms.Label
	Private sampledDataGroupBox As System.Windows.Forms.GroupBox
	Private sampledDataGridView As System.Windows.Forms.DataGridView
	Private buttonsGroupBox As System.Windows.Forms.GroupBox


End Class
