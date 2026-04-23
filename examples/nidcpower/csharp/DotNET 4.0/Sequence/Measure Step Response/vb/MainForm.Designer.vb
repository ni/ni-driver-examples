Partial Class MainForm
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.voltageLevelRangeLabel = New System.Windows.Forms.Label()
        Me.currentLimitLabel = New System.Windows.Forms.Label()
        Me.currentLimitRangeLabel = New System.Windows.Forms.Label()
        Me.measureRecordLengthLabel = New System.Windows.Forms.Label()
        Me.transientResponseLabel = New System.Windows.Forms.Label()
        Me.voltageLevelRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.currentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.currentLimitRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.measureRecordLengthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startButton = New System.Windows.Forms.Button()
        Me.transientResponseComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameAndChannelNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.apertureTimeLabel = New System.Windows.Forms.Label()
        Me.apertureTimeNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.voltageSetPointsLabel = New System.Windows.Forms.Label()
        Me.voltageSetPointsDataGridView = New System.Windows.Forms.DataGridView()
        Me.measurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementsDataGridView = New System.Windows.Forms.DataGridView()
        Me.readingNoColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.measuredVoltageColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.measuredCurrentColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.voltageSetPointsStepColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.voltageSetPointsValueColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.voltageLevelRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.currentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.currentLimitRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.measureRecordLengthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resourceNameAndChannelNameGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.apertureTimeNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageSetPointsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementsGroupBox.SuspendLayout()
        CType(Me.measurementsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'voltageLevelRangeLabel
        '
        Me.voltageLevelRangeLabel.AutoSize = True
        Me.voltageLevelRangeLabel.Location = New System.Drawing.Point(6, 101)
        Me.voltageLevelRangeLabel.Name = "voltageLevelRangeLabel"
        Me.voltageLevelRangeLabel.Size = New System.Drawing.Size(123, 13)
        Me.voltageLevelRangeLabel.TabIndex = 8
        Me.voltageLevelRangeLabel.Text = "Voltage Level Range (V)"
        '
        'currentLimitLabel
        '
        Me.currentLimitLabel.AutoSize = True
        Me.currentLimitLabel.Location = New System.Drawing.Point(6, 127)
        Me.currentLimitLabel.Name = "currentLimitLabel"
        Me.currentLimitLabel.Size = New System.Drawing.Size(81, 13)
        Me.currentLimitLabel.TabIndex = 9
        Me.currentLimitLabel.Text = "Current Limit (A)"
        '
        'currentLimitRangeLabel
        '
        Me.currentLimitRangeLabel.AutoSize = True
        Me.currentLimitRangeLabel.Location = New System.Drawing.Point(6, 153)
        Me.currentLimitRangeLabel.Name = "currentLimitRangeLabel"
        Me.currentLimitRangeLabel.Size = New System.Drawing.Size(116, 13)
        Me.currentLimitRangeLabel.TabIndex = 10
        Me.currentLimitRangeLabel.Text = "Current Limit Range (A)"
        '
        'measureRecordLengthLabel
        '
        Me.measureRecordLengthLabel.AutoSize = True
        Me.measureRecordLengthLabel.Location = New System.Drawing.Point(6, 206)
        Me.measureRecordLengthLabel.Name = "measureRecordLengthLabel"
        Me.measureRecordLengthLabel.Size = New System.Drawing.Size(122, 13)
        Me.measureRecordLengthLabel.TabIndex = 12
        Me.measureRecordLengthLabel.Text = "Measure Record Length"
        '
        'transientResponseLabel
        '
        Me.transientResponseLabel.AutoSize = True
        Me.transientResponseLabel.Location = New System.Drawing.Point(6, 179)
        Me.transientResponseLabel.Name = "transientResponseLabel"
        Me.transientResponseLabel.Size = New System.Drawing.Size(102, 13)
        Me.transientResponseLabel.TabIndex = 11
        Me.transientResponseLabel.Text = "Transient Response"
        '
        'voltageLevelRangeNumeric
        '
        Me.voltageLevelRangeNumeric.DecimalPlaces = 6
        Me.voltageLevelRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLevelRangeNumeric.Location = New System.Drawing.Point(140, 97)
        Me.voltageLevelRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLevelRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLevelRangeNumeric.Name = "voltageLevelRangeNumeric"
        Me.voltageLevelRangeNumeric.Size = New System.Drawing.Size(91, 20)
        Me.voltageLevelRangeNumeric.TabIndex = 2
        Me.voltageLevelRangeNumeric.Value = New Decimal(New Integer() {6, 0, 0, 0})
        '
        'currentLimitNumeric
        '
        Me.currentLimitNumeric.DecimalPlaces = 6
        Me.currentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.currentLimitNumeric.Location = New System.Drawing.Point(140, 123)
        Me.currentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.currentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.currentLimitNumeric.Name = "currentLimitNumeric"
        Me.currentLimitNumeric.Size = New System.Drawing.Size(91, 20)
        Me.currentLimitNumeric.TabIndex = 3
        Me.currentLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'currentLimitRangeNumeric
        '
        Me.currentLimitRangeNumeric.DecimalPlaces = 6
        Me.currentLimitRangeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.currentLimitRangeNumeric.Location = New System.Drawing.Point(140, 149)
        Me.currentLimitRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.currentLimitRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.currentLimitRangeNumeric.Name = "currentLimitRangeNumeric"
        Me.currentLimitRangeNumeric.Size = New System.Drawing.Size(91, 20)
        Me.currentLimitRangeNumeric.TabIndex = 4
        Me.currentLimitRangeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'measureRecordLengthNumeric
        '
        Me.measureRecordLengthNumeric.Location = New System.Drawing.Point(140, 202)
        Me.measureRecordLengthNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.measureRecordLengthNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.measureRecordLengthNumeric.Name = "measureRecordLengthNumeric"
        Me.measureRecordLengthNumeric.Size = New System.Drawing.Size(91, 20)
        Me.measureRecordLengthNumeric.TabIndex = 6
        Me.measureRecordLengthNumeric.Value = New Decimal(New Integer() {200, 0, 0, 0})
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(363, 357)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 2
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'transientResponseComboBox
        '
        Me.transientResponseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.transientResponseComboBox.Location = New System.Drawing.Point(140, 175)
        Me.transientResponseComboBox.Name = "transientResponseComboBox"
        Me.transientResponseComboBox.Size = New System.Drawing.Size(91, 21)
        Me.transientResponseComboBox.TabIndex = 5
        '
        'resourceNameAndChannelNameGroupBox
        '
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.channelNameLabel)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.resourceNameAndChannelNameGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.resourceNameAndChannelNameGroupBox.Name = "resourceNameAndChannelNameGroupBox"
        Me.resourceNameAndChannelNameGroupBox.Size = New System.Drawing.Size(240, 73)
        Me.resourceNameAndChannelNameGroupBox.TabIndex = 0
        Me.resourceNameAndChannelNameGroupBox.TabStop = False
        Me.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name"
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(140, 45)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(91, 20)
        Me.channelNameTextBox.TabIndex = 1
        Me.channelNameTextBox.Text = "0"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(106, 18)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(125, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.Location = New System.Drawing.Point(6, 49)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.channelNameLabel.TabIndex = 3
        Me.channelNameLabel.Text = "Channel Name"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 22)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 2
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.apertureTimeLabel)
        Me.configurationGroupBox.Controls.Add(Me.apertureTimeNumericUpDown)
        Me.configurationGroupBox.Controls.Add(Me.transientResponseComboBox)
        Me.configurationGroupBox.Controls.Add(Me.voltageSetPointsLabel)
        Me.configurationGroupBox.Controls.Add(Me.transientResponseLabel)
        Me.configurationGroupBox.Controls.Add(Me.measureRecordLengthNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.measureRecordLengthLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltageSetPointsDataGridView)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelRangeNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLimitRangeNumeric)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 101)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(240, 258)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        '
        'apertureTimeLabel
        '
        Me.apertureTimeLabel.AutoSize = True
        Me.apertureTimeLabel.Location = New System.Drawing.Point(6, 232)
        Me.apertureTimeLabel.Name = "apertureTimeLabel"
        Me.apertureTimeLabel.Size = New System.Drawing.Size(87, 13)
        Me.apertureTimeLabel.TabIndex = 13
        Me.apertureTimeLabel.Text = "Aperture Time (s)"
        '
        'apertureTimeNumericUpDown
        '
        Me.apertureTimeNumericUpDown.DecimalPlaces = 6
        Me.apertureTimeNumericUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 393216})
        Me.apertureTimeNumericUpDown.Location = New System.Drawing.Point(140, 228)
        Me.apertureTimeNumericUpDown.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.apertureTimeNumericUpDown.Name = "apertureTimeNumericUpDown"
        Me.apertureTimeNumericUpDown.Size = New System.Drawing.Size(91, 20)
        Me.apertureTimeNumericUpDown.TabIndex = 7
        Me.apertureTimeNumericUpDown.Value = New Decimal(New Integer() {5, 0, 0, 393216})
        '
        'voltageSetPointsLabel
        '
        Me.voltageSetPointsLabel.AutoSize = True
        Me.voltageSetPointsLabel.Location = New System.Drawing.Point(6, 17)
        Me.voltageSetPointsLabel.Name = "voltageSetPointsLabel"
        Me.voltageSetPointsLabel.Size = New System.Drawing.Size(106, 13)
        Me.voltageSetPointsLabel.TabIndex = 0
        Me.voltageSetPointsLabel.Text = "Voltage Setpoints (V)"
        '
        'voltageSetPointsDataGridView
        '
        Me.voltageSetPointsDataGridView.AllowUserToAddRows = False
        Me.voltageSetPointsDataGridView.AllowUserToDeleteRows = False
        Me.voltageSetPointsDataGridView.AllowUserToResizeColumns = False
        Me.voltageSetPointsDataGridView.AllowUserToResizeRows = False
        Me.voltageSetPointsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.voltageSetPointsDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.voltageSetPointsDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.voltageSetPointsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.voltageSetPointsDataGridView.ColumnHeadersVisible = False
        Me.voltageSetPointsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.voltageSetPointsStepColumn, Me.voltageSetPointsValueColumn})
        Me.voltageSetPointsDataGridView.Location = New System.Drawing.Point(6, 33)
        Me.voltageSetPointsDataGridView.Name = "voltageSetPointsDataGridView"
        Me.voltageSetPointsDataGridView.RowHeadersVisible = False
        Me.voltageSetPointsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.voltageSetPointsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.voltageSetPointsDataGridView.Size = New System.Drawing.Size(145, 47)
        Me.voltageSetPointsDataGridView.StandardTab = True
        Me.voltageSetPointsDataGridView.TabIndex = 1
        '
        'measurementsGroupBox
        '
        Me.measurementsGroupBox.Controls.Add(Me.measurementsDataGridView)
        Me.measurementsGroupBox.Location = New System.Drawing.Point(268, 12)
        Me.measurementsGroupBox.Name = "measurementsGroupBox"
        Me.measurementsGroupBox.Size = New System.Drawing.Size(265, 339)
        Me.measurementsGroupBox.TabIndex = 3
        Me.measurementsGroupBox.TabStop = False
        Me.measurementsGroupBox.Text = "Measurements"
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
        Me.measurementsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.readingNoColumn, Me.measuredVoltageColumn, Me.measuredCurrentColumn})
        Me.measurementsDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.measurementsDataGridView.Name = "measurementsDataGridView"
        Me.measurementsDataGridView.ReadOnly = True
        Me.measurementsDataGridView.RowHeadersVisible = False
        Me.measurementsDataGridView.RowHeadersWidth = 15
        Me.measurementsDataGridView.RowTemplate.Height = 24
        Me.measurementsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.measurementsDataGridView.Size = New System.Drawing.Size(253, 311)
        Me.measurementsDataGridView.StandardTab = True
        Me.measurementsDataGridView.TabIndex = 0
        '
        'readingNoColumn
        '
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black
        Me.readingNoColumn.DefaultCellStyle = DataGridViewCellStyle4
        Me.readingNoColumn.HeaderText = "Point"
        Me.readingNoColumn.Name = "readingNoColumn"
        Me.readingNoColumn.ReadOnly = True
        Me.readingNoColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.readingNoColumn.Width = 50
        '
        'measuredVoltageColumn
        '
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black
        Me.measuredVoltageColumn.DefaultCellStyle = DataGridViewCellStyle5
        Me.measuredVoltageColumn.HeaderText = "Voltage (V)"
        Me.measuredVoltageColumn.Name = "measuredVoltageColumn"
        Me.measuredVoltageColumn.ReadOnly = True
        Me.measuredVoltageColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'measuredCurrentColumn
        '
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black
        Me.measuredCurrentColumn.DefaultCellStyle = DataGridViewCellStyle6
        Me.measuredCurrentColumn.HeaderText = "Current (A)"
        Me.measuredCurrentColumn.Name = "measuredCurrentColumn"
        Me.measuredCurrentColumn.ReadOnly = True
        Me.measuredCurrentColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'voltageSetPointsStepColumn
        '
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        Me.voltageSetPointsStepColumn.DefaultCellStyle = DataGridViewCellStyle2
        Me.voltageSetPointsStepColumn.Frozen = True
        Me.voltageSetPointsStepColumn.HeaderText = "Step"
        Me.voltageSetPointsStepColumn.Name = "voltageSetPointsStepColumn"
        Me.voltageSetPointsStepColumn.ReadOnly = True
        Me.voltageSetPointsStepColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.voltageSetPointsStepColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.voltageSetPointsStepColumn.Width = 42
        '
        'voltageSetPointsValueColumn
        '
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black
        Me.voltageSetPointsValueColumn.DefaultCellStyle = DataGridViewCellStyle3
        Me.voltageSetPointsValueColumn.HeaderText = "Value"
        Me.voltageSetPointsValueColumn.Name = "voltageSetPointsValueColumn"
        Me.voltageSetPointsValueColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.voltageSetPointsValueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(543, 393)
        Me.Controls.Add(Me.measurementsGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceNameAndChannelNameGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Measure Step Response"
        CType(Me.voltageLevelRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.currentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.currentLimitRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.measureRecordLengthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.resourceNameAndChannelNameGroupBox.ResumeLayout(False)
        Me.resourceNameAndChannelNameGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.apertureTimeNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageSetPointsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementsGroupBox.ResumeLayout(False)
        CType(Me.measurementsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private voltageLevelRangeLabel As System.Windows.Forms.Label
    Private currentLimitLabel As System.Windows.Forms.Label
    Private currentLimitRangeLabel As System.Windows.Forms.Label
    Private measureRecordLengthLabel As System.Windows.Forms.Label
    Private transientResponseLabel As System.Windows.Forms.Label
    Private voltageLevelRangeNumeric As System.Windows.Forms.NumericUpDown
    Private currentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private currentLimitRangeNumeric As System.Windows.Forms.NumericUpDown
    Private measureRecordLengthNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents startButton As System.Windows.Forms.Button
    Private transientResponseComboBox As System.Windows.Forms.ComboBox
    Private resourceNameAndChannelNameGroupBox As System.Windows.Forms.GroupBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private channelNameLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private measurementsGroupBox As System.Windows.Forms.GroupBox
    Private measurementsDataGridView As System.Windows.Forms.DataGridView
    Private channelNameTextBox As System.Windows.Forms.TextBox
    Private readingNoColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private measuredVoltageColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private measuredCurrentColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private voltageSetPointsDataGridView As System.Windows.Forms.DataGridView
    Private voltageSetPointsLabel As System.Windows.Forms.Label
    Private apertureTimeLabel As System.Windows.Forms.Label
    Private apertureTimeNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents voltageSetPointsStepColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents voltageSetPointsValueColumn As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
