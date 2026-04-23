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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.voltageMeasurmentsLabel = New System.Windows.Forms.Label()
        Me.apertureTimesLabel = New System.Windows.Forms.Label()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.voltagemeasurments1Column = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.aperturetimes1Column = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.voltageMeasurmentsDataGridView = New System.Windows.Forms.DataGridView()
        Me.apertureTimesDataGridView = New System.Windows.Forms.DataGridView()
        Me.resourceNameAndChannelNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.voltageLevelLabel = New System.Windows.Forms.Label()
        Me.voltageLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.sourceDelayLabel = New System.Windows.Forms.Label()
        Me.sourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.voltageMeasurmentsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.apertureTimesDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resourceNameAndChannelNameGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.voltageLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 23)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.Location = New System.Drawing.Point(6, 48)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.channelNameLabel.TabIndex = 1
        Me.channelNameLabel.Text = "Channel Name"
        '
        'voltageMeasurmentsLabel
        '
        Me.voltageMeasurmentsLabel.AutoSize = True
        Me.voltageMeasurmentsLabel.Location = New System.Drawing.Point(3, 19)
        Me.voltageMeasurmentsLabel.Name = "voltageMeasurmentsLabel"
        Me.voltageMeasurmentsLabel.Size = New System.Drawing.Size(131, 13)
        Me.voltageMeasurmentsLabel.TabIndex = 4
        Me.voltageMeasurmentsLabel.Text = "Voltage Measurements (V)"
        '
        'apertureTimesLabel
        '
        Me.apertureTimesLabel.AutoSize = True
        Me.apertureTimesLabel.Location = New System.Drawing.Point(4, 100)
        Me.apertureTimesLabel.Name = "apertureTimesLabel"
        Me.apertureTimesLabel.Size = New System.Drawing.Size(114, 13)
        Me.apertureTimesLabel.TabIndex = 5
        Me.apertureTimesLabel.Text = "Aperture Time Array (s)"
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(132, 45)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(126, 20)
        Me.channelNameTextBox.TabIndex = 1
        Me.channelNameTextBox.Text = "0"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(315, 174)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 5
        Me.startButton.Text = "Start"
        Me.startButton.UseVisualStyleBackColor = True
        AddHandler Me.startButton.Click, New System.EventHandler(AddressOf Me.startButton_Click)
        '
        'voltagemeasurments1Column
        '
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black
        Me.voltagemeasurments1Column.DefaultCellStyle = DataGridViewCellStyle1
        Me.voltagemeasurments1Column.Frozen = True
        Me.voltagemeasurments1Column.HeaderText = ""
        Me.voltagemeasurments1Column.Name = "voltagemeasurments1Column"
        Me.voltagemeasurments1Column.ReadOnly = True
        Me.voltagemeasurments1Column.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.voltagemeasurments1Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.voltagemeasurments1Column.Width = 99
        '
        'aperturetimes1Column
        '
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        Me.aperturetimes1Column.DefaultCellStyle = DataGridViewCellStyle2
        Me.aperturetimes1Column.Frozen = True
        Me.aperturetimes1Column.HeaderText = ""
        Me.aperturetimes1Column.Name = "aperturetimes1Column"
        Me.aperturetimes1Column.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.aperturetimes1Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.aperturetimes1Column.Width = 99
        '
        'voltageMeasurmentsDataGridView
        '
        Me.voltageMeasurmentsDataGridView.AllowUserToAddRows = False
        Me.voltageMeasurmentsDataGridView.AllowUserToDeleteRows = False
        Me.voltageMeasurmentsDataGridView.AllowUserToResizeColumns = False
        Me.voltageMeasurmentsDataGridView.AllowUserToResizeRows = False
        Me.voltageMeasurmentsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.voltageMeasurmentsDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.voltageMeasurmentsDataGridView.ColumnHeadersVisible = False
        Me.voltageMeasurmentsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.voltagemeasurments1Column})
        Me.voltageMeasurmentsDataGridView.Location = New System.Drawing.Point(6, 35)
        Me.voltageMeasurmentsDataGridView.Name = "voltageMeasurmentsDataGridView"
        Me.voltageMeasurmentsDataGridView.ReadOnly = True
        Me.voltageMeasurmentsDataGridView.RowHeadersVisible = False
        Me.voltageMeasurmentsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.voltageMeasurmentsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.voltageMeasurmentsDataGridView.Size = New System.Drawing.Size(103, 69)
        Me.voltageMeasurmentsDataGridView.StandardTab = True
        Me.voltageMeasurmentsDataGridView.TabIndex = 6
        '
        'apertureTimesDataGridView
        '
        Me.apertureTimesDataGridView.AllowUserToAddRows = False
        Me.apertureTimesDataGridView.AllowUserToDeleteRows = False
        Me.apertureTimesDataGridView.AllowUserToResizeColumns = False
        Me.apertureTimesDataGridView.AllowUserToResizeRows = False
        Me.apertureTimesDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.apertureTimesDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.apertureTimesDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.apertureTimesDataGridView.ColumnHeadersVisible = False
        Me.apertureTimesDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.aperturetimes1Column})
        Me.apertureTimesDataGridView.Location = New System.Drawing.Point(132, 68)
        Me.apertureTimesDataGridView.Name = "apertureTimesDataGridView"
        Me.apertureTimesDataGridView.RowHeadersVisible = False
        Me.apertureTimesDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.apertureTimesDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.apertureTimesDataGridView.Size = New System.Drawing.Size(100, 66)
        Me.apertureTimesDataGridView.StandardTab = True
        Me.apertureTimesDataGridView.TabIndex = 4
        '
        'resourceNameAndChannelNameGroupBox
        '
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.channelNameLabel)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.resourceNameAndChannelNameGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.resourceNameAndChannelNameGroupBox.Name = "resourceNameAndChannelNameGroupBox"
        Me.resourceNameAndChannelNameGroupBox.Size = New System.Drawing.Size(264, 72)
        Me.resourceNameAndChannelNameGroupBox.TabIndex = 7
        Me.resourceNameAndChannelNameGroupBox.TabStop = False
        Me.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(132, 20)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(126, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayLabel)
        Me.configurationGroupBox.Controls.Add(Me.apertureTimesLabel)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayNumeric)
        Me.configurationGroupBox.Controls.Add(Me.apertureTimesDataGridView)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 102)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(264, 139)
        Me.configurationGroupBox.TabIndex = 8
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'voltageLevelLabel
        '
        Me.voltageLevelLabel.AutoSize = True
        Me.voltageLevelLabel.Location = New System.Drawing.Point(6, 18)
        Me.voltageLevelLabel.Name = "voltageLevelLabel"
        Me.voltageLevelLabel.Size = New System.Drawing.Size(88, 13)
        Me.voltageLevelLabel.TabIndex = 2
        Me.voltageLevelLabel.Text = "Voltage Level (V)"
        '
        'voltageLevelNumeric
        '
        Me.voltageLevelNumeric.DecimalPlaces = 5
        Me.voltageLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLevelNumeric.Location = New System.Drawing.Point(132, 16)
        Me.voltageLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLevelNumeric.Name = "voltageLevelNumeric"
        Me.voltageLevelNumeric.Size = New System.Drawing.Size(126, 20)
        Me.voltageLevelNumeric.TabIndex = 2
        Me.voltageLevelNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'sourceDelayLabel
        '
        Me.sourceDelayLabel.AutoSize = True
        Me.sourceDelayLabel.Location = New System.Drawing.Point(6, 44)
        Me.sourceDelayLabel.Name = "sourceDelayLabel"
        Me.sourceDelayLabel.Size = New System.Drawing.Size(85, 13)
        Me.sourceDelayLabel.TabIndex = 3
        Me.sourceDelayLabel.Text = "Source Delay (s)"
        '
        'sourceDelayNumeric
        '
        Me.sourceDelayNumeric.DecimalPlaces = 5
        Me.sourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.sourceDelayNumeric.Location = New System.Drawing.Point(132, 42)
        Me.sourceDelayNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.sourceDelayNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.sourceDelayNumeric.Name = "sourceDelayNumeric"
        Me.sourceDelayNumeric.Size = New System.Drawing.Size(126, 20)
        Me.sourceDelayNumeric.TabIndex = 3
        Me.sourceDelayNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.voltageMeasurmentsLabel)
        Me.measurementGroupBox.Controls.Add(Me.voltageMeasurmentsDataGridView)
        Me.measurementGroupBox.Location = New System.Drawing.Point(282, 11)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(142, 111)
        Me.measurementGroupBox.TabIndex = 9
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurements"
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(431, 243)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceNameAndChannelNameGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Advanced Sequencing Aperture Time"
        CType(Me.voltageMeasurmentsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.apertureTimesDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.resourceNameAndChannelNameGroupBox.ResumeLayout(False)
        Me.resourceNameAndChannelNameGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.voltageLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sourceDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
	#End Region

	Private resourceNameLabel As System.Windows.Forms.Label
	Private channelNameLabel As System.Windows.Forms.Label
	Private voltageMeasurmentsLabel As System.Windows.Forms.Label
	Private apertureTimesLabel As System.Windows.Forms.Label
	Private channelNameTextBox As System.Windows.Forms.TextBox
	Private startButton As System.Windows.Forms.Button
	Private voltagemeasurments1Column As System.Windows.Forms.DataGridViewTextBoxColumn
	Private aperturetimes1Column As System.Windows.Forms.DataGridViewTextBoxColumn
	Private voltageMeasurmentsDataGridView As System.Windows.Forms.DataGridView
	Private apertureTimesDataGridView As System.Windows.Forms.DataGridView
	Private resourceNameAndChannelNameGroupBox As System.Windows.Forms.GroupBox
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private configurationGroupBox As System.Windows.Forms.GroupBox
	Private voltageLevelLabel As System.Windows.Forms.Label
	Private voltageLevelNumeric As System.Windows.Forms.NumericUpDown
	Private sourceDelayLabel As System.Windows.Forms.Label
	Private sourceDelayNumeric As System.Windows.Forms.NumericUpDown
	Private measurementGroupBox As System.Windows.Forms.GroupBox


End Class
