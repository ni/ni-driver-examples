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
		Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
		Me.channelNameLabel = New System.Windows.Forms.Label()
		Me.minSampleRateLabel = New System.Windows.Forms.Label()
		Me.verticalRangeLabel = New System.Windows.Forms.Label()
		Me.numRecordsLabel = New System.Windows.Forms.Label()
		Me.triggertypeLabel = New System.Windows.Forms.Label()
		Me.triggersourceLabel = New System.Windows.Forms.Label()
		Me.triggerholdoffLabel = New System.Windows.Forms.Label()
		Me.meanOfTriggersLabel = New System.Windows.Forms.Label()
		Me.stdevOfTriggersLabel = New System.Windows.Forms.Label()
		Me.freqOfTriggersLabel = New System.Windows.Forms.Label()
		Me.resourceNameLabel = New System.Windows.Forms.Label()
		Me.textmsg5Label = New System.Windows.Forms.Label()
		Me.textmsg3Label = New System.Windows.Forms.Label()
		Me.sampleRateMinNumeric = New System.Windows.Forms.NumericUpDown()
		Me.verticalRangeNumeric = New System.Windows.Forms.NumericUpDown()
		Me.numberOfRecordsNumeric = New System.Windows.Forms.NumericUpDown()
		Me.triggerHoldoffNumeric = New System.Windows.Forms.NumericUpDown()
		Me.channelNameTextBox = New System.Windows.Forms.TextBox()
		Me.triggerTypeComboBox = New System.Windows.Forms.ComboBox()
		Me.triggerGroupBox = New System.Windows.Forms.GroupBox()
		Me.triggerSourceComboBox = New System.Windows.Forms.ComboBox()
		Me.generalGroupBox = New System.Windows.Forms.GroupBox()
		Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
		Me.sampleDataGroupBox = New System.Windows.Forms.GroupBox()
		Me.meanfrequencyTextBox = New System.Windows.Forms.TextBox()
		Me.standardDeviationTextBox = New System.Windows.Forms.TextBox()
		Me.meanTimeTextBox = New System.Windows.Forms.TextBox()
		Me.histogramDataGridView = New System.Windows.Forms.DataGridView()
		Me.acquireButton = New System.Windows.Forms.Button()
		Me.buttonGroupBox = New System.Windows.Forms.GroupBox()
		Me.histogramIndexDataGridViewColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.histogramXValueDataGridViewColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
		Me.histogramYValueDataGridViewColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
		DirectCast(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.numberOfRecordsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.triggerHoldoffNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.triggerGroupBox.SuspendLayout()
		Me.generalGroupBox.SuspendLayout()
		Me.sampleDataGroupBox.SuspendLayout()
		DirectCast(Me.histogramDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.buttonGroupBox.SuspendLayout()
		Me.SuspendLayout()
		' 
		' channelNameLabel
		' 
		Me.channelNameLabel.AutoSize = True
		Me.channelNameLabel.Location = New System.Drawing.Point(4, 50)
		Me.channelNameLabel.Name = "channelNameLabel"
		Me.channelNameLabel.Size = New System.Drawing.Size(80, 13)
		Me.channelNameLabel.TabIndex = 0
		Me.channelNameLabel.Text = "Channel Name:"
		' 
		' minSampleRateLabel
		' 
		Me.minSampleRateLabel.AutoSize = True
		Me.minSampleRateLabel.Location = New System.Drawing.Point(4, 76)
		Me.minSampleRateLabel.Name = "minSampleRateLabel"
		Me.minSampleRateLabel.Size = New System.Drawing.Size(91, 13)
		Me.minSampleRateLabel.TabIndex = 1
		Me.minSampleRateLabel.Text = "Min Sample Rate:"
		' 
		' verticalRangeLabel
		' 
		Me.verticalRangeLabel.AutoSize = True
		Me.verticalRangeLabel.Location = New System.Drawing.Point(4, 128)
		Me.verticalRangeLabel.Name = "verticalRangeLabel"
		Me.verticalRangeLabel.Size = New System.Drawing.Size(80, 13)
		Me.verticalRangeLabel.TabIndex = 2
		Me.verticalRangeLabel.Text = "Vertical Range:"
		' 
		' numRecordsLabel
		' 
		Me.numRecordsLabel.AutoSize = True
		Me.numRecordsLabel.Location = New System.Drawing.Point(4, 102)
		Me.numRecordsLabel.Name = "numRecordsLabel"
		Me.numRecordsLabel.Size = New System.Drawing.Size(78, 13)
		Me.numRecordsLabel.TabIndex = 3
		Me.numRecordsLabel.Text = "Num. Records:"
		' 
		' triggertypeLabel
		' 
		Me.triggertypeLabel.AutoSize = True
		Me.triggertypeLabel.Location = New System.Drawing.Point(4, 23)
		Me.triggertypeLabel.Name = "triggertypeLabel"
		Me.triggertypeLabel.Size = New System.Drawing.Size(34, 13)
		Me.triggertypeLabel.TabIndex = 4
		Me.triggertypeLabel.Text = "Type:"
		' 
		' triggersourceLabel
		' 
		Me.triggersourceLabel.AutoSize = True
		Me.triggersourceLabel.Location = New System.Drawing.Point(4, 50)
		Me.triggersourceLabel.Name = "triggersourceLabel"
		Me.triggersourceLabel.Size = New System.Drawing.Size(44, 13)
		Me.triggersourceLabel.TabIndex = 5
		Me.triggersourceLabel.Text = "Source:"
		' 
		' triggerholdoffLabel
		' 
		Me.triggerholdoffLabel.AutoSize = True
		Me.triggerholdoffLabel.Location = New System.Drawing.Point(4, 77)
		Me.triggerholdoffLabel.Name = "triggerholdoffLabel"
		Me.triggerholdoffLabel.Size = New System.Drawing.Size(44, 13)
		Me.triggerholdoffLabel.TabIndex = 6
		Me.triggerholdoffLabel.Text = "Holdoff:"
		' 
		' meanOfTriggersLabel
		' 
		Me.meanOfTriggersLabel.AutoSize = True
		Me.meanOfTriggersLabel.Location = New System.Drawing.Point(6, 280)
		Me.meanOfTriggersLabel.Name = "meanOfTriggersLabel"
		Me.meanOfTriggersLabel.Size = New System.Drawing.Size(154, 13)
		Me.meanOfTriggersLabel.TabIndex = 8
		Me.meanOfTriggersLabel.Text = "Mean time between triggers (s):"
		' 
		' stdevOfTriggersLabel
		' 
		Me.stdevOfTriggersLabel.AutoSize = True
		Me.stdevOfTriggersLabel.Location = New System.Drawing.Point(6, 306)
		Me.stdevOfTriggersLabel.Name = "stdevOfTriggersLabel"
		Me.stdevOfTriggersLabel.Size = New System.Drawing.Size(184, 13)
		Me.stdevOfTriggersLabel.TabIndex = 9
		Me.stdevOfTriggersLabel.Text = "Standard deviation time of triggers (s):"
		' 
		' freqOfTriggersLabel
		' 
		Me.freqOfTriggersLabel.AutoSize = True
		Me.freqOfTriggersLabel.Location = New System.Drawing.Point(6, 332)
		Me.freqOfTriggersLabel.Name = "freqOfTriggersLabel"
		Me.freqOfTriggersLabel.Size = New System.Drawing.Size(150, 13)
		Me.freqOfTriggersLabel.TabIndex = 10
		Me.freqOfTriggersLabel.Text = "Mean frequency of triggers (s):"
		' 
		' resourceNameLabel
		' 
		Me.resourceNameLabel.AutoSize = True
		Me.resourceNameLabel.Location = New System.Drawing.Point(4, 23)
		Me.resourceNameLabel.Name = "resourceNameLabel"
		Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
		Me.resourceNameLabel.TabIndex = 14
		Me.resourceNameLabel.Text = "Resource Name:"
		' 
		' textmsg5Label
		' 
		Me.textmsg5Label.AutoSize = True
		Me.textmsg5Label.Location = New System.Drawing.Point(286, 6)
		Me.textmsg5Label.Name = "textmsg5Label"
		Me.textmsg5Label.Size = New System.Drawing.Size(0, 13)
		Me.textmsg5Label.TabIndex = 5
		' 
		' textmsg3Label
		' 
		Me.textmsg3Label.AutoSize = True
		Me.textmsg3Label.Location = New System.Drawing.Point(37, 173)
		Me.textmsg3Label.Name = "textmsg3Label"
		Me.textmsg3Label.Size = New System.Drawing.Size(0, 13)
		Me.textmsg3Label.TabIndex = 17
		' 
		' sampleRateNumericMin
		' 
		Me.sampleRateMinNumeric.DecimalPlaces = 2
		Me.sampleRateMinNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
		Me.sampleRateMinNumeric.Location = New System.Drawing.Point(104, 72)
		Me.sampleRateMinNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.sampleRateMinNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.sampleRateMinNumeric.Name = "sampleRateNumericMin"
		Me.sampleRateMinNumeric.Size = New System.Drawing.Size(107, 20)
		Me.sampleRateMinNumeric.TabIndex = 2
		Me.sampleRateMinNumeric.Value = New Decimal(New Integer() {100000000, 0, 0, 0})
		' 
		' verticalRangeNumeric
		' 
		Me.verticalRangeNumeric.DecimalPlaces = 2
		Me.verticalRangeNumeric.Location = New System.Drawing.Point(104, 124)
		Me.verticalRangeNumeric.Maximum = New Decimal(New Integer() {-2147483648, 0, 0, 0})
		Me.verticalRangeNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.verticalRangeNumeric.Name = "verticalRangeNumeric"
		Me.verticalRangeNumeric.Size = New System.Drawing.Size(107, 20)
		Me.verticalRangeNumeric.TabIndex = 4
		Me.verticalRangeNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
		' 
		' numberOfRecordsNumeric
		' 
		Me.numberOfRecordsNumeric.Location = New System.Drawing.Point(104, 98)
		Me.numberOfRecordsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.numberOfRecordsNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.numberOfRecordsNumeric.Name = "numberOfRecordsNumeric"
		Me.numberOfRecordsNumeric.Size = New System.Drawing.Size(107, 20)
		Me.numberOfRecordsNumeric.TabIndex = 3
		Me.numberOfRecordsNumeric.Value = New Decimal(New Integer() {500, 0, 0, 0})
		' 
		' triggerHoldoffNumeric
		' 
		Me.triggerHoldoffNumeric.DecimalPlaces = 2
		Me.triggerHoldoffNumeric.Location = New System.Drawing.Point(104, 73)
		Me.triggerHoldoffNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.triggerHoldoffNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.triggerHoldoffNumeric.Name = "triggerHoldoffNumeric"
		Me.triggerHoldoffNumeric.Size = New System.Drawing.Size(107, 20)
		Me.triggerHoldoffNumeric.TabIndex = 2
		' 
		' channelNameTextBox
		' 
		Me.channelNameTextBox.Location = New System.Drawing.Point(104, 46)
		Me.channelNameTextBox.Name = "channelNameTextBox"
		Me.channelNameTextBox.Size = New System.Drawing.Size(107, 20)
		Me.channelNameTextBox.TabIndex = 1
		Me.channelNameTextBox.Text = "0"
		' 
		' triggerTypeComboBox
		' 
		Me.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.triggerTypeComboBox.Location = New System.Drawing.Point(104, 19)
		Me.triggerTypeComboBox.Name = "triggerTypeComboBox"
		Me.triggerTypeComboBox.Size = New System.Drawing.Size(107, 21)
		Me.triggerTypeComboBox.TabIndex = 0
		' 
		' triggerGroupBox
		' 
		Me.triggerGroupBox.Controls.Add(Me.triggerTypeComboBox)
		Me.triggerGroupBox.Controls.Add(Me.triggerSourceComboBox)
		Me.triggerGroupBox.Controls.Add(Me.triggerHoldoffNumeric)
		Me.triggerGroupBox.Controls.Add(Me.triggertypeLabel)
		Me.triggerGroupBox.Controls.Add(Me.triggersourceLabel)
		Me.triggerGroupBox.Controls.Add(Me.triggerholdoffLabel)
		Me.triggerGroupBox.Location = New System.Drawing.Point(12, 180)
		Me.triggerGroupBox.Name = "triggerGroupBox"
		Me.triggerGroupBox.Size = New System.Drawing.Size(217, 102)
		Me.triggerGroupBox.TabIndex = 1
		Me.triggerGroupBox.TabStop = False
		Me.triggerGroupBox.Text = "Trigger"
		' 
		' triggerSourceComboBox
		' 
		Me.triggerSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.triggerSourceComboBox.Location = New System.Drawing.Point(104, 46)
		Me.triggerSourceComboBox.Name = "triggerSourceComboBox"
		Me.triggerSourceComboBox.Size = New System.Drawing.Size(107, 21)
		Me.triggerSourceComboBox.TabIndex = 1
		' 
		' generalGroupBox
		' 
		Me.generalGroupBox.Controls.Add(Me.resourceNameComboBox)
		Me.generalGroupBox.Controls.Add(Me.channelNameTextBox)
		Me.generalGroupBox.Controls.Add(Me.sampleRateMinNumeric)
		Me.generalGroupBox.Controls.Add(Me.verticalRangeNumeric)
		Me.generalGroupBox.Controls.Add(Me.numberOfRecordsNumeric)
		Me.generalGroupBox.Controls.Add(Me.channelNameLabel)
		Me.generalGroupBox.Controls.Add(Me.minSampleRateLabel)
		Me.generalGroupBox.Controls.Add(Me.verticalRangeLabel)
		Me.generalGroupBox.Controls.Add(Me.numRecordsLabel)
		Me.generalGroupBox.Controls.Add(Me.resourceNameLabel)
		Me.generalGroupBox.Location = New System.Drawing.Point(12, 12)
		Me.generalGroupBox.Name = "generalGroupBox"
		Me.generalGroupBox.Size = New System.Drawing.Size(217, 152)
		Me.generalGroupBox.TabIndex = 0
		Me.generalGroupBox.TabStop = False
		Me.generalGroupBox.Text = "General"
		' 
		' resourceNameComboBox
		' 
		Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.resourceNameComboBox.FormattingEnabled = True
		Me.resourceNameComboBox.Location = New System.Drawing.Point(104, 19)
		Me.resourceNameComboBox.Name = "resourceNameComboBox"
		Me.resourceNameComboBox.Size = New System.Drawing.Size(107, 21)
		Me.resourceNameComboBox.TabIndex = 0
		' 
		' sampleDataGroupBox
		' 
		Me.sampleDataGroupBox.Controls.Add(Me.meanfrequencyTextBox)
		Me.sampleDataGroupBox.Controls.Add(Me.standardDeviationTextBox)
		Me.sampleDataGroupBox.Controls.Add(Me.meanTimeTextBox)
		Me.sampleDataGroupBox.Controls.Add(Me.histogramDataGridView)
		Me.sampleDataGroupBox.Controls.Add(Me.meanOfTriggersLabel)
		Me.sampleDataGroupBox.Controls.Add(Me.freqOfTriggersLabel)
		Me.sampleDataGroupBox.Controls.Add(Me.stdevOfTriggersLabel)
		Me.sampleDataGroupBox.Location = New System.Drawing.Point(235, 12)
		Me.sampleDataGroupBox.Name = "sampleDataGroupBox"
		Me.sampleDataGroupBox.Size = New System.Drawing.Size(329, 354)
		Me.sampleDataGroupBox.TabIndex = 4
		Me.sampleDataGroupBox.TabStop = False
		Me.sampleDataGroupBox.Text = "Trigger Difference Histogram"
		' 
		' meanfrequencyTextBox
		' 
		Me.meanfrequencyTextBox.Location = New System.Drawing.Point(196, 329)
		Me.meanfrequencyTextBox.Name = "meanfrequencyTextBox"
		Me.meanfrequencyTextBox.[ReadOnly] = True
		Me.meanfrequencyTextBox.Size = New System.Drawing.Size(125, 20)
		Me.meanfrequencyTextBox.TabIndex = 3
		Me.meanfrequencyTextBox.Text = "0"
		' 
		' standardDeviationTextBox
		' 
		Me.standardDeviationTextBox.Location = New System.Drawing.Point(196, 302)
		Me.standardDeviationTextBox.Name = "standardDeviationTextBox"
		Me.standardDeviationTextBox.[ReadOnly] = True
		Me.standardDeviationTextBox.Size = New System.Drawing.Size(125, 20)
		Me.standardDeviationTextBox.TabIndex = 2
		Me.standardDeviationTextBox.Text = "0"
		' 
		' meanTimeTextBox
		' 
		Me.meanTimeTextBox.Location = New System.Drawing.Point(196, 276)
		Me.meanTimeTextBox.Name = "meanTimeTextBox"
		Me.meanTimeTextBox.[ReadOnly] = True
		Me.meanTimeTextBox.Size = New System.Drawing.Size(125, 20)
		Me.meanTimeTextBox.TabIndex = 1
		Me.meanTimeTextBox.Text = "0"
		' 
		' histogramDataGridView
		' 
		Me.histogramDataGridView.AllowUserToAddRows = False
		Me.histogramDataGridView.AllowUserToDeleteRows = False
		Me.histogramDataGridView.AllowUserToResizeRows = False
		Me.histogramDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.histogramDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.histogramDataGridView.ColumnHeadersVisible = False
		Me.histogramDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.histogramIndexDataGridViewColumn, Me.histogramXValueDataGridViewColumn, Me.histogramYValueDataGridViewColumn})
		Me.histogramDataGridView.Location = New System.Drawing.Point(6, 19)
		Me.histogramDataGridView.Name = "histogramDataGridView"
		Me.histogramDataGridView.[ReadOnly] = True
		Me.histogramDataGridView.RowHeadersVisible = False
		Me.histogramDataGridView.RowHeadersWidth = 15
		Me.histogramDataGridView.RowTemplate.Height = 24
		Me.histogramDataGridView.Size = New System.Drawing.Size(315, 251)
		Me.histogramDataGridView.StandardTab = True
		Me.histogramDataGridView.TabIndex = 0
		' 
		' acquireButton
		' 
		Me.acquireButton.Location = New System.Drawing.Point(73, 19)
		Me.acquireButton.Name = "acquireButton"
		Me.acquireButton.Size = New System.Drawing.Size(75, 23)
		Me.acquireButton.TabIndex = 0
		Me.acquireButton.Text = "&Acquire"
		Me.acquireButton.UseVisualStyleBackColor = True
		AddHandler Me.acquireButton.Click, New System.EventHandler(AddressOf Me.acquireButton_Click)
		' 
		' buttonGroupBox
		' 
		Me.buttonGroupBox.Controls.Add(Me.acquireButton)
		Me.buttonGroupBox.Location = New System.Drawing.Point(12, 307)
		Me.buttonGroupBox.Name = "buttonGroupBox"
		Me.buttonGroupBox.Size = New System.Drawing.Size(211, 59)
		Me.buttonGroupBox.TabIndex = 3
		Me.buttonGroupBox.TabStop = False
		' 
		' histogramIndexDataGridViewColumn
		' 
		Me.histogramIndexDataGridViewColumn.HeaderText = "Index"
		Me.histogramIndexDataGridViewColumn.Name = "histogramIndexDataGridViewColumn"
		Me.histogramIndexDataGridViewColumn.[ReadOnly] = True
		Me.histogramIndexDataGridViewColumn.Width = 45
		' 
		' histogramXValueDataGridViewColumn
		' 
		Me.histogramXValueDataGridViewColumn.HeaderText = "Histogram X Value"
		Me.histogramXValueDataGridViewColumn.Name = "histogramXValueDataGridViewColumn"
		Me.histogramXValueDataGridViewColumn.[ReadOnly] = True
		Me.histogramXValueDataGridViewColumn.Width = 125
		' 
		' histogramYValueDataGridViewColumn
		' 
		Me.histogramYValueDataGridViewColumn.HeaderText = "Histogram Y Value"
		Me.histogramYValueDataGridViewColumn.Name = "histogramYValueDataGridViewColumn"
		Me.histogramYValueDataGridViewColumn.[ReadOnly] = True
		Me.histogramYValueDataGridViewColumn.Width = 125
		' 
		' MainForm
		' 
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(581, 380)
		Me.Controls.Add(Me.buttonGroupBox)
		Me.Controls.Add(Me.sampleDataGroupBox)
		Me.Controls.Add(Me.textmsg5Label)
		Me.Controls.Add(Me.textmsg3Label)
		Me.Controls.Add(Me.triggerGroupBox)
		Me.Controls.Add(Me.generalGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Icon = DirectCast(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.Name = "MainForm"
		Me.Text = "Timestamps"
		DirectCast(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.numberOfRecordsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.triggerHoldoffNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		Me.triggerGroupBox.ResumeLayout(False)
		Me.triggerGroupBox.PerformLayout()
		Me.generalGroupBox.ResumeLayout(False)
		Me.generalGroupBox.PerformLayout()
		Me.sampleDataGroupBox.ResumeLayout(False)
		Me.sampleDataGroupBox.PerformLayout()
		DirectCast(Me.histogramDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
		Me.buttonGroupBox.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	#End Region

	Private channelNameLabel As System.Windows.Forms.Label
	Private minSampleRateLabel As System.Windows.Forms.Label
	Private verticalRangeLabel As System.Windows.Forms.Label
	Private numRecordsLabel As System.Windows.Forms.Label
	Private triggertypeLabel As System.Windows.Forms.Label
	Private triggersourceLabel As System.Windows.Forms.Label
	Private triggerholdoffLabel As System.Windows.Forms.Label
	Private meanOfTriggersLabel As System.Windows.Forms.Label
	Private stdevOfTriggersLabel As System.Windows.Forms.Label
	Private freqOfTriggersLabel As System.Windows.Forms.Label
	Private resourceNameLabel As System.Windows.Forms.Label
	Private textmsg5Label As System.Windows.Forms.Label
	Private textmsg3Label As System.Windows.Forms.Label
	Private sampleRateMinNumeric As System.Windows.Forms.NumericUpDown
	Private verticalRangeNumeric As System.Windows.Forms.NumericUpDown
	Private numberOfRecordsNumeric As System.Windows.Forms.NumericUpDown
	Private triggerHoldoffNumeric As System.Windows.Forms.NumericUpDown
	Private channelNameTextBox As System.Windows.Forms.TextBox
	Private triggerTypeComboBox As System.Windows.Forms.ComboBox
	Private triggerSourceComboBox As System.Windows.Forms.ComboBox
	Private triggerGroupBox As System.Windows.Forms.GroupBox
	Private generalGroupBox As System.Windows.Forms.GroupBox
	Private sampleDataGroupBox As System.Windows.Forms.GroupBox
	Private histogramDataGridView As System.Windows.Forms.DataGridView
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private acquireButton As System.Windows.Forms.Button
	Private meanfrequencyTextBox As System.Windows.Forms.TextBox
	Private standardDeviationTextBox As System.Windows.Forms.TextBox
	Private meanTimeTextBox As System.Windows.Forms.TextBox
	Private buttonGroupBox As System.Windows.Forms.GroupBox
	Private histogramIndexDataGridViewColumn As System.Windows.Forms.DataGridViewTextBoxColumn
	Private histogramXValueDataGridViewColumn As System.Windows.Forms.DataGridViewTextBoxColumn
	Private histogramYValueDataGridViewColumn As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
