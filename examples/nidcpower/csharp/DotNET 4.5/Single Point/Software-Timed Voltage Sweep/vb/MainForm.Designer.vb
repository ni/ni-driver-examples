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
        Me.pointsLabel = New System.Windows.Forms.Label()
        Me.voltageLevelStartLabel = New System.Windows.Forms.Label()
        Me.voltageLevelStopLabel = New System.Windows.Forms.Label()
        Me.delayLabel = New System.Windows.Forms.Label()
        Me.numberOfPointsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.voltageLevelStartNumeric = New System.Windows.Forms.NumericUpDown()
        Me.voltageLevelStopNumeric = New System.Windows.Forms.NumericUpDown()
        Me.sourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startButton = New System.Windows.Forms.Button()
        Me.resourceNameAndChannelNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.currentLimitLabel = New System.Windows.Forms.Label()
        Me.currentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.measurementsDataGridView = New System.Windows.Forms.DataGridView()
        Me.readingNoColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.voltageMeasurementColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.currentMeasurementColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.measurementsGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.numberOfPointsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageLevelStartNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageLevelStopNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resourceNameAndChannelNameGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.currentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.measurementsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementsGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'pointsLabel
        '
        Me.pointsLabel.AutoSize = True
        Me.pointsLabel.Location = New System.Drawing.Point(6, 61)
        Me.pointsLabel.Name = "pointsLabel"
        Me.pointsLabel.Size = New System.Drawing.Size(36, 13)
        Me.pointsLabel.TabIndex = 4
        Me.pointsLabel.Text = "Points"
        '
        'voltageLevelStartLabel
        '
        Me.voltageLevelStartLabel.AutoSize = True
        Me.voltageLevelStartLabel.Location = New System.Drawing.Point(6, 94)
        Me.voltageLevelStartLabel.Name = "voltageLevelStartLabel"
        Me.voltageLevelStartLabel.Size = New System.Drawing.Size(113, 13)
        Me.voltageLevelStartLabel.TabIndex = 6
        Me.voltageLevelStartLabel.Text = "Voltage Level Start (V)"
        '
        'voltageLevelStopLabel
        '
        Me.voltageLevelStopLabel.AutoSize = True
        Me.voltageLevelStopLabel.Location = New System.Drawing.Point(6, 127)
        Me.voltageLevelStopLabel.Name = "voltageLevelStopLabel"
        Me.voltageLevelStopLabel.Size = New System.Drawing.Size(113, 13)
        Me.voltageLevelStopLabel.TabIndex = 8
        Me.voltageLevelStopLabel.Text = "Voltage Level Stop (V)"
        '
        'delayLabel
        '
        Me.delayLabel.AutoSize = True
        Me.delayLabel.Location = New System.Drawing.Point(6, 153)
        Me.delayLabel.Name = "delayLabel"
        Me.delayLabel.Size = New System.Drawing.Size(85, 13)
        Me.delayLabel.TabIndex = 10
        Me.delayLabel.Text = "Source Delay (s)"
        '
        'numberOfPointsNumeric
        '
        Me.numberOfPointsNumeric.Location = New System.Drawing.Point(161, 59)
        Me.numberOfPointsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numberOfPointsNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numberOfPointsNumeric.Name = "numberOfPointsNumeric"
        Me.numberOfPointsNumeric.Size = New System.Drawing.Size(90, 20)
        Me.numberOfPointsNumeric.TabIndex = 5
        Me.numberOfPointsNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'voltageLevelStartNumeric
        '
        Me.voltageLevelStartNumeric.DecimalPlaces = 6
        Me.voltageLevelStartNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLevelStartNumeric.Location = New System.Drawing.Point(161, 92)
        Me.voltageLevelStartNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLevelStartNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLevelStartNumeric.Name = "voltageLevelStartNumeric"
        Me.voltageLevelStartNumeric.Size = New System.Drawing.Size(90, 20)
        Me.voltageLevelStartNumeric.TabIndex = 7
        Me.voltageLevelStartNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'voltageLevelStopNumeric
        '
        Me.voltageLevelStopNumeric.DecimalPlaces = 6
        Me.voltageLevelStopNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLevelStopNumeric.Location = New System.Drawing.Point(161, 123)
        Me.voltageLevelStopNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLevelStopNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLevelStopNumeric.Name = "voltageLevelStopNumeric"
        Me.voltageLevelStopNumeric.Size = New System.Drawing.Size(90, 20)
        Me.voltageLevelStopNumeric.TabIndex = 9
        Me.voltageLevelStopNumeric.Value = New Decimal(New Integer() {6, 0, 0, 0})
        '
        'sourceDelayNumeric
        '
        Me.sourceDelayNumeric.DecimalPlaces = 6
        Me.sourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.sourceDelayNumeric.Location = New System.Drawing.Point(161, 149)
        Me.sourceDelayNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.sourceDelayNumeric.Name = "sourceDelayNumeric"
        Me.sourceDelayNumeric.Size = New System.Drawing.Size(90, 20)
        Me.sourceDelayNumeric.TabIndex = 11
        Me.sourceDelayNumeric.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(103, 301)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 2
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'resourceNameAndChannelNameGroupBox
        '
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.channelNameLabel)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.resourceNameAndChannelNameGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.resourceNameAndChannelNameGroupBox.Name = "resourceNameAndChannelNameGroupBox"
        Me.resourceNameAndChannelNameGroupBox.Size = New System.Drawing.Size(257, 73)
        Me.resourceNameAndChannelNameGroupBox.TabIndex = 0
        Me.resourceNameAndChannelNameGroupBox.TabStop = False
        Me.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name"
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(161, 45)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(90, 20)
        Me.channelNameTextBox.TabIndex = 3
        Me.channelNameTextBox.Text = "0"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(126, 18)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(125, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.Location = New System.Drawing.Point(6, 49)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.channelNameLabel.TabIndex = 2
        Me.channelNameLabel.Text = "Channel Name"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 22)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelStopLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelStartLabel)
        Me.configurationGroupBox.Controls.Add(Me.delayLabel)
        Me.configurationGroupBox.Controls.Add(Me.pointsLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelStopNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitLabel)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayNumeric)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelStartNumeric)
        Me.configurationGroupBox.Controls.Add(Me.numberOfPointsNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitNumeric)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 101)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(257, 179)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'currentLimitLabel
        '
        Me.currentLimitLabel.AutoSize = True
        Me.currentLimitLabel.Location = New System.Drawing.Point(6, 31)
        Me.currentLimitLabel.Name = "currentLimitLabel"
        Me.currentLimitLabel.Size = New System.Drawing.Size(81, 13)
        Me.currentLimitLabel.TabIndex = 2
        Me.currentLimitLabel.Text = "Current Limit (A)"
        '
        'currentLimitNumeric
        '
        Me.currentLimitNumeric.DecimalPlaces = 6
        Me.currentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.currentLimitNumeric.Location = New System.Drawing.Point(160, 29)
        Me.currentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.currentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.currentLimitNumeric.Name = "currentLimitNumeric"
        Me.currentLimitNumeric.Size = New System.Drawing.Size(91, 20)
        Me.currentLimitNumeric.TabIndex = 3
        Me.currentLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'measurementsDataGridView
        '
        Me.measurementsDataGridView.AllowUserToAddRows = False
        Me.measurementsDataGridView.AllowUserToDeleteRows = False
        Me.measurementsDataGridView.AllowUserToResizeRows = False
        Me.measurementsDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.measurementsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.measurementsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.readingNoColumn, Me.voltageMeasurementColumn, Me.currentMeasurementColumn})
        Me.measurementsDataGridView.Location = New System.Drawing.Point(6, 22)
        Me.measurementsDataGridView.Name = "measurementsDataGridView"
        Me.measurementsDataGridView.ReadOnly = True
        Me.measurementsDataGridView.RowHeadersVisible = False
        Me.measurementsDataGridView.RowHeadersWidth = 15
        Me.measurementsDataGridView.RowTemplate.Height = 24
        Me.measurementsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.measurementsDataGridView.Size = New System.Drawing.Size(253, 290)
        Me.measurementsDataGridView.StandardTab = True
        Me.measurementsDataGridView.TabIndex = 0
        '
        'readingNoColumn
        '
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black
        Me.readingNoColumn.DefaultCellStyle = DataGridViewCellStyle1
        Me.readingNoColumn.HeaderText = "Point"
        Me.readingNoColumn.Name = "readingNoColumn"
        Me.readingNoColumn.ReadOnly = True
        Me.readingNoColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.readingNoColumn.Width = 40
        '
        'voltageMeasurementColumn
        '
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        Me.voltageMeasurementColumn.DefaultCellStyle = DataGridViewCellStyle2
        Me.voltageMeasurementColumn.HeaderText = "Voltage (V)"
        Me.voltageMeasurementColumn.Name = "voltageMeasurementColumn"
        Me.voltageMeasurementColumn.ReadOnly = True
        Me.voltageMeasurementColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.voltageMeasurementColumn.Width = 105
        '
        'currentMeasurementColumn
        '
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black
        Me.currentMeasurementColumn.DefaultCellStyle = DataGridViewCellStyle3
        Me.currentMeasurementColumn.HeaderText = "Current (A)"
        Me.currentMeasurementColumn.Name = "currentMeasurementColumn"
        Me.currentMeasurementColumn.ReadOnly = True
        Me.currentMeasurementColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.currentMeasurementColumn.Width = 105
        '
        'measurementsGroupBox
        '
        Me.measurementsGroupBox.Controls.Add(Me.measurementsDataGridView)
        Me.measurementsGroupBox.Location = New System.Drawing.Point(285, 12)
        Me.measurementsGroupBox.Name = "measurementsGroupBox"
        Me.measurementsGroupBox.Size = New System.Drawing.Size(265, 321)
        Me.measurementsGroupBox.TabIndex = 3
        Me.measurementsGroupBox.TabStop = False
        Me.measurementsGroupBox.Text = "Measurements"
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(564, 346)
        Me.Controls.Add(Me.measurementsGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceNameAndChannelNameGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Software-Timed Voltage Sweep"
        CType(Me.numberOfPointsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageLevelStartNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageLevelStopNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.resourceNameAndChannelNameGroupBox.ResumeLayout(False)
        Me.resourceNameAndChannelNameGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.currentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.measurementsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementsGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private pointsLabel As System.Windows.Forms.Label
    Private voltageLevelStartLabel As System.Windows.Forms.Label
    Private voltageLevelStopLabel As System.Windows.Forms.Label
    Private delayLabel As System.Windows.Forms.Label
    Private numberOfPointsNumeric As System.Windows.Forms.NumericUpDown
    Private voltageLevelStartNumeric As System.Windows.Forms.NumericUpDown
    Private voltageLevelStopNumeric As System.Windows.Forms.NumericUpDown
    Private sourceDelayNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents startButton As System.Windows.Forms.Button
    Private resourceNameAndChannelNameGroupBox As System.Windows.Forms.GroupBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private channelNameLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private currentLimitLabel As System.Windows.Forms.Label
    Private currentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private measurementsDataGridView As System.Windows.Forms.DataGridView
    Private measurementsGroupBox As System.Windows.Forms.GroupBox
    Private channelNameTextBox As System.Windows.Forms.TextBox
    Private readingNoColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private voltageMeasurementColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private currentMeasurementColumn As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
