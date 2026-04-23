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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle17 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle18 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle19 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle20 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle21 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle22 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle23 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle24 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.masterConfigurationCurrentLimitLabel = New System.Windows.Forms.Label()
        Me.masterConfigurationCurrentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.masterSourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startButton = New System.Windows.Forms.Button()
        Me.masterConfigurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.masterConfigurationSubGroupBox = New System.Windows.Forms.GroupBox()
        Me.masterConfigurationChannelNameTextBox = New System.Windows.Forms.TextBox()
        Me.masterConfigurationResourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.masterConfigurationResourceNameLabel = New System.Windows.Forms.Label()
        Me.masterConfigurationChannelNameLabel = New System.Windows.Forms.Label()
        Me.masterSequenceGroupBox = New System.Windows.Forms.GroupBox()
        Me.masterSequenceDataGridView = New System.Windows.Forms.DataGridView()
        Me.masterSequenceStepColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.masterSequenceValueColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.masterMeasureCompleteEventDelayGroupBox = New System.Windows.Forms.GroupBox()
        Me.masterMeasureCompleteEventDelayMessageRichTextBox = New System.Windows.Forms.RichTextBox()
        Me.masterMeasureCompleteEventDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.masterSourceDelayGroupBox = New System.Windows.Forms.GroupBox()
        Me.masterSourceDelayMessageRichTextBox = New System.Windows.Forms.RichTextBox()
        Me.slavesConfigurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.slavesConfigurationDataGridView = New System.Windows.Forms.DataGridView()
        Me.slavesConfigurationSlaveNumColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.resourceNameColumn = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.slavesConfigurationChannelNameColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slavesConfigurationCurrentLimitColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slave0MeasurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.slave0MeasurementsDataGridView = New System.Windows.Forms.DataGridView()
        Me.slave0MeasurementsStepColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slave0MeasurementsVoltageColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slave0MeasurementsCurrentColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.masterMeasurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.masterMeasurementDataGridView = New System.Windows.Forms.DataGridView()
        Me.masterMeasurementsStepColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.masterMeasurementsVoltageColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.masterMeasurementsCurrentColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slave1MeasurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.slave1MeasurementsDataGridView = New System.Windows.Forms.DataGridView()
        Me.slave1MeasurementsStepColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slave1MeasurementsVoltageColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slave1MeasurementsCurrentColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slavesSequenceGroupBox = New System.Windows.Forms.GroupBox()
        Me.slavesSequenceDataGridView = New System.Windows.Forms.DataGridView()
        Me.slavesSequenceStepColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slavesSequenceSlave0Column = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slavesSequenceSlave1Column = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.measurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageRichTextBox = New System.Windows.Forms.RichTextBox()
        CType(Me.masterConfigurationCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.masterSourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.masterConfigurationGroupBox.SuspendLayout()
        Me.masterConfigurationSubGroupBox.SuspendLayout()
        Me.masterSequenceGroupBox.SuspendLayout()
        CType(Me.masterSequenceDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.masterMeasureCompleteEventDelayGroupBox.SuspendLayout()
        CType(Me.masterMeasureCompleteEventDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.masterSourceDelayGroupBox.SuspendLayout()
        Me.slavesConfigurationGroupBox.SuspendLayout()
        CType(Me.slavesConfigurationDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.slave0MeasurementsGroupBox.SuspendLayout()
        CType(Me.slave0MeasurementsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.masterMeasurementsGroupBox.SuspendLayout()
        CType(Me.masterMeasurementDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.slave1MeasurementsGroupBox.SuspendLayout()
        CType(Me.slave1MeasurementsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.slavesSequenceGroupBox.SuspendLayout()
        CType(Me.slavesSequenceDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementsGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'masterConfigurationCurrentLimitLabel
        '
        Me.masterConfigurationCurrentLimitLabel.AutoSize = True
        Me.masterConfigurationCurrentLimitLabel.Location = New System.Drawing.Point(6, 115)
        Me.masterConfigurationCurrentLimitLabel.Name = "masterConfigurationCurrentLimitLabel"
        Me.masterConfigurationCurrentLimitLabel.Size = New System.Drawing.Size(81, 13)
        Me.masterConfigurationCurrentLimitLabel.TabIndex = 4
        Me.masterConfigurationCurrentLimitLabel.Text = "Current Limit (A)"
        '
        'masterConfigurationCurrentLimitNumeric
        '
        Me.masterConfigurationCurrentLimitNumeric.DecimalPlaces = 6
        Me.masterConfigurationCurrentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.masterConfigurationCurrentLimitNumeric.Location = New System.Drawing.Point(6, 131)
        Me.masterConfigurationCurrentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.masterConfigurationCurrentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.masterConfigurationCurrentLimitNumeric.Name = "masterConfigurationCurrentLimitNumeric"
        Me.masterConfigurationCurrentLimitNumeric.Size = New System.Drawing.Size(91, 20)
        Me.masterConfigurationCurrentLimitNumeric.TabIndex = 5
        Me.masterConfigurationCurrentLimitNumeric.Value = New Decimal(New Integer() {2, 0, 0, 131072})
        '
        'masterSourceDelayNumeric
        '
        Me.masterSourceDelayNumeric.DecimalPlaces = 6
        Me.masterSourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.masterSourceDelayNumeric.Location = New System.Drawing.Point(6, 68)
        Me.masterSourceDelayNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.masterSourceDelayNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.masterSourceDelayNumeric.Name = "masterSourceDelayNumeric"
        Me.masterSourceDelayNumeric.Size = New System.Drawing.Size(100, 20)
        Me.masterSourceDelayNumeric.TabIndex = 1
        Me.masterSourceDelayNumeric.Value = New Decimal(New Integer() {5, 0, 0, 131072})
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(568, 271)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 3
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'masterConfigurationGroupBox
        '
        Me.masterConfigurationGroupBox.Controls.Add(Me.masterConfigurationSubGroupBox)
        Me.masterConfigurationGroupBox.Controls.Add(Me.masterSequenceGroupBox)
        Me.masterConfigurationGroupBox.Controls.Add(Me.masterMeasureCompleteEventDelayGroupBox)
        Me.masterConfigurationGroupBox.Controls.Add(Me.masterSourceDelayGroupBox)
        Me.masterConfigurationGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.masterConfigurationGroupBox.Name = "masterConfigurationGroupBox"
        Me.masterConfigurationGroupBox.Size = New System.Drawing.Size(392, 325)
        Me.masterConfigurationGroupBox.TabIndex = 0
        Me.masterConfigurationGroupBox.TabStop = False
        Me.masterConfigurationGroupBox.Text = "Master Configuration"
        '
        'masterConfigurationSubGroupBox
        '
        Me.masterConfigurationSubGroupBox.Controls.Add(Me.masterConfigurationChannelNameTextBox)
        Me.masterConfigurationSubGroupBox.Controls.Add(Me.masterConfigurationResourceNameComboBox)
        Me.masterConfigurationSubGroupBox.Controls.Add(Me.masterConfigurationCurrentLimitNumeric)
        Me.masterConfigurationSubGroupBox.Controls.Add(Me.masterConfigurationResourceNameLabel)
        Me.masterConfigurationSubGroupBox.Controls.Add(Me.masterConfigurationCurrentLimitLabel)
        Me.masterConfigurationSubGroupBox.Controls.Add(Me.masterConfigurationChannelNameLabel)
        Me.masterConfigurationSubGroupBox.Location = New System.Drawing.Point(6, 22)
        Me.masterConfigurationSubGroupBox.Name = "masterConfigurationSubGroupBox"
        Me.masterConfigurationSubGroupBox.Size = New System.Drawing.Size(138, 162)
        Me.masterConfigurationSubGroupBox.TabIndex = 0
        Me.masterConfigurationSubGroupBox.TabStop = False
        '
        'masterConfigurationChannelNameTextBox
        '
        Me.masterConfigurationChannelNameTextBox.Location = New System.Drawing.Point(6, 82)
        Me.masterConfigurationChannelNameTextBox.Name = "masterConfigurationChannelNameTextBox"
        Me.masterConfigurationChannelNameTextBox.Size = New System.Drawing.Size(91, 20)
        Me.masterConfigurationChannelNameTextBox.TabIndex = 3
        Me.masterConfigurationChannelNameTextBox.Text = "0"
        '
        'masterConfigurationResourceNameComboBox
        '
        Me.masterConfigurationResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.masterConfigurationResourceNameComboBox.FormattingEnabled = True
        Me.masterConfigurationResourceNameComboBox.Location = New System.Drawing.Point(6, 32)
        Me.masterConfigurationResourceNameComboBox.Name = "masterConfigurationResourceNameComboBox"
        Me.masterConfigurationResourceNameComboBox.Size = New System.Drawing.Size(125, 21)
        Me.masterConfigurationResourceNameComboBox.TabIndex = 1
        '
        'masterConfigurationResourceNameLabel
        '
        Me.masterConfigurationResourceNameLabel.AutoSize = True
        Me.masterConfigurationResourceNameLabel.Location = New System.Drawing.Point(6, 16)
        Me.masterConfigurationResourceNameLabel.Name = "masterConfigurationResourceNameLabel"
        Me.masterConfigurationResourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.masterConfigurationResourceNameLabel.TabIndex = 0
        Me.masterConfigurationResourceNameLabel.Text = "Resource Name"
        '
        'masterConfigurationChannelNameLabel
        '
        Me.masterConfigurationChannelNameLabel.AutoSize = True
        Me.masterConfigurationChannelNameLabel.Location = New System.Drawing.Point(6, 66)
        Me.masterConfigurationChannelNameLabel.Name = "masterConfigurationChannelNameLabel"
        Me.masterConfigurationChannelNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.masterConfigurationChannelNameLabel.TabIndex = 2
        Me.masterConfigurationChannelNameLabel.Text = "Channel Name"
        '
        'masterSequenceGroupBox
        '
        Me.masterSequenceGroupBox.Controls.Add(Me.masterSequenceDataGridView)
        Me.masterSequenceGroupBox.Location = New System.Drawing.Point(6, 200)
        Me.masterSequenceGroupBox.Name = "masterSequenceGroupBox"
        Me.masterSequenceGroupBox.Size = New System.Drawing.Size(138, 118)
        Me.masterSequenceGroupBox.TabIndex = 1
        Me.masterSequenceGroupBox.TabStop = False
        Me.masterSequenceGroupBox.Text = "Master Sequence"
        '
        'masterSequenceDataGridView
        '
        Me.masterSequenceDataGridView.AllowUserToAddRows = False
        Me.masterSequenceDataGridView.AllowUserToDeleteRows = False
        Me.masterSequenceDataGridView.AllowUserToResizeColumns = False
        Me.masterSequenceDataGridView.AllowUserToResizeRows = False
        Me.masterSequenceDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.masterSequenceDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.masterSequenceDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.masterSequenceDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.masterSequenceDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.masterSequenceStepColumn, Me.masterSequenceValueColumn})
        Me.masterSequenceDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.masterSequenceDataGridView.Name = "masterSequenceDataGridView"
        Me.masterSequenceDataGridView.RowHeadersVisible = False
        Me.masterSequenceDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.masterSequenceDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.masterSequenceDataGridView.Size = New System.Drawing.Size(125, 91)
        Me.masterSequenceDataGridView.StandardTab = True
        Me.masterSequenceDataGridView.TabIndex = 0
        '
        'masterSequenceStepColumn
        '
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        Me.masterSequenceStepColumn.DefaultCellStyle = DataGridViewCellStyle2
        Me.masterSequenceStepColumn.Frozen = True
        Me.masterSequenceStepColumn.HeaderText = "Step"
        Me.masterSequenceStepColumn.Name = "masterSequenceStepColumn"
        Me.masterSequenceStepColumn.ReadOnly = True
        Me.masterSequenceStepColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.masterSequenceStepColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.masterSequenceStepColumn.Width = 35
        '
        'masterSequenceValueColumn
        '
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black
        Me.masterSequenceValueColumn.DefaultCellStyle = DataGridViewCellStyle3
        Me.masterSequenceValueColumn.HeaderText = "Value"
        Me.masterSequenceValueColumn.Name = "masterSequenceValueColumn"
        Me.masterSequenceValueColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.masterSequenceValueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.masterSequenceValueColumn.Width = 87
        '
        'masterMeasureCompleteEventDelayGroupBox
        '
        Me.masterMeasureCompleteEventDelayGroupBox.Controls.Add(Me.masterMeasureCompleteEventDelayMessageRichTextBox)
        Me.masterMeasureCompleteEventDelayGroupBox.Controls.Add(Me.masterMeasureCompleteEventDelayNumeric)
        Me.masterMeasureCompleteEventDelayGroupBox.Location = New System.Drawing.Point(160, 153)
        Me.masterMeasureCompleteEventDelayGroupBox.Name = "masterMeasureCompleteEventDelayGroupBox"
        Me.masterMeasureCompleteEventDelayGroupBox.Size = New System.Drawing.Size(225, 165)
        Me.masterMeasureCompleteEventDelayGroupBox.TabIndex = 3
        Me.masterMeasureCompleteEventDelayGroupBox.TabStop = False
        Me.masterMeasureCompleteEventDelayGroupBox.Text = "Master Measure Complete Event Delay (s)"
        '
        'masterMeasureCompleteEventDelayMessageRichTextBox
        '
        Me.masterMeasureCompleteEventDelayMessageRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.masterMeasureCompleteEventDelayMessageRichTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.masterMeasureCompleteEventDelayMessageRichTextBox.Location = New System.Drawing.Point(6, 23)
        Me.masterMeasureCompleteEventDelayMessageRichTextBox.Name = "masterMeasureCompleteEventDelayMessageRichTextBox"
        Me.masterMeasureCompleteEventDelayMessageRichTextBox.ReadOnly = True
        Me.masterMeasureCompleteEventDelayMessageRichTextBox.Size = New System.Drawing.Size(208, 108)
        Me.masterMeasureCompleteEventDelayMessageRichTextBox.TabIndex = 0
        Me.masterMeasureCompleteEventDelayMessageRichTextBox.TabStop = False
        Me.masterMeasureCompleteEventDelayMessageRichTextBox.Text = resources.GetString("masterMeasureCompleteEventDelayMessageRichTextBox.Text")
        '
        'masterMeasureCompleteEventDelayNumeric
        '
        Me.masterMeasureCompleteEventDelayNumeric.DecimalPlaces = 6
        Me.masterMeasureCompleteEventDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.masterMeasureCompleteEventDelayNumeric.Location = New System.Drawing.Point(6, 137)
        Me.masterMeasureCompleteEventDelayNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.masterMeasureCompleteEventDelayNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.masterMeasureCompleteEventDelayNumeric.Name = "masterMeasureCompleteEventDelayNumeric"
        Me.masterMeasureCompleteEventDelayNumeric.Size = New System.Drawing.Size(100, 20)
        Me.masterMeasureCompleteEventDelayNumeric.TabIndex = 1
        Me.masterMeasureCompleteEventDelayNumeric.Value = New Decimal(New Integer() {5, 0, 0, 131072})
        '
        'masterSourceDelayGroupBox
        '
        Me.masterSourceDelayGroupBox.Controls.Add(Me.masterSourceDelayMessageRichTextBox)
        Me.masterSourceDelayGroupBox.Controls.Add(Me.masterSourceDelayNumeric)
        Me.masterSourceDelayGroupBox.Location = New System.Drawing.Point(160, 22)
        Me.masterSourceDelayGroupBox.Name = "masterSourceDelayGroupBox"
        Me.masterSourceDelayGroupBox.Size = New System.Drawing.Size(225, 100)
        Me.masterSourceDelayGroupBox.TabIndex = 2
        Me.masterSourceDelayGroupBox.TabStop = False
        Me.masterSourceDelayGroupBox.Text = "Master Source Delay (s)"
        '
        'masterSourceDelayMessageRichTextBox
        '
        Me.masterSourceDelayMessageRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.masterSourceDelayMessageRichTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.masterSourceDelayMessageRichTextBox.Location = New System.Drawing.Point(6, 19)
        Me.masterSourceDelayMessageRichTextBox.Name = "masterSourceDelayMessageRichTextBox"
        Me.masterSourceDelayMessageRichTextBox.ReadOnly = True
        Me.masterSourceDelayMessageRichTextBox.Size = New System.Drawing.Size(208, 43)
        Me.masterSourceDelayMessageRichTextBox.TabIndex = 0
        Me.masterSourceDelayMessageRichTextBox.TabStop = False
        Me.masterSourceDelayMessageRichTextBox.Text = "This delay has to be long enough for all the devices to finish programming the ou" & _
    "tput and to settle."
        '
        'slavesConfigurationGroupBox
        '
        Me.slavesConfigurationGroupBox.Controls.Add(Me.slavesConfigurationDataGridView)
        Me.slavesConfigurationGroupBox.Location = New System.Drawing.Point(419, 12)
        Me.slavesConfigurationGroupBox.Name = "slavesConfigurationGroupBox"
        Me.slavesConfigurationGroupBox.Size = New System.Drawing.Size(373, 98)
        Me.slavesConfigurationGroupBox.TabIndex = 1
        Me.slavesConfigurationGroupBox.TabStop = False
        Me.slavesConfigurationGroupBox.Text = "Slave(s) Configuration"
        '
        'slavesConfigurationDataGridView
        '
        Me.slavesConfigurationDataGridView.AllowUserToAddRows = False
        Me.slavesConfigurationDataGridView.AllowUserToDeleteRows = False
        Me.slavesConfigurationDataGridView.AllowUserToResizeColumns = False
        Me.slavesConfigurationDataGridView.AllowUserToResizeRows = False
        Me.slavesConfigurationDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.slavesConfigurationDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.slavesConfigurationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.slavesConfigurationDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.slavesConfigurationSlaveNumColumn, Me.resourceNameColumn, Me.slavesConfigurationChannelNameColumn, Me.slavesConfigurationCurrentLimitColumn})
        Me.slavesConfigurationDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.slavesConfigurationDataGridView.Name = "slavesConfigurationDataGridView"
        Me.slavesConfigurationDataGridView.RowHeadersVisible = False
        Me.slavesConfigurationDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.slavesConfigurationDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesConfigurationDataGridView.Size = New System.Drawing.Size(358, 69)
        Me.slavesConfigurationDataGridView.StandardTab = True
        Me.slavesConfigurationDataGridView.TabIndex = 0
        '
        'slavesConfigurationSlaveNumColumn
        '
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black
        Me.slavesConfigurationSlaveNumColumn.DefaultCellStyle = DataGridViewCellStyle5
        Me.slavesConfigurationSlaveNumColumn.DividerWidth = 1
        Me.slavesConfigurationSlaveNumColumn.Frozen = True
        Me.slavesConfigurationSlaveNumColumn.HeaderText = ""
        Me.slavesConfigurationSlaveNumColumn.Name = "slavesConfigurationSlaveNumColumn"
        Me.slavesConfigurationSlaveNumColumn.ReadOnly = True
        Me.slavesConfigurationSlaveNumColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesConfigurationSlaveNumColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slavesConfigurationSlaveNumColumn.Width = 50
        '
        'resourceNameColumn
        '
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black
        Me.resourceNameColumn.DefaultCellStyle = DataGridViewCellStyle6
        Me.resourceNameColumn.HeaderText = "Resource Name"
        Me.resourceNameColumn.Name = "resourceNameColumn"
        Me.resourceNameColumn.ReadOnly = False
        Me.resourceNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.resourceNameColumn.Width = 115
        '
        'slavesConfigurationChannelNameColumn
        '
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black
        Me.slavesConfigurationChannelNameColumn.DefaultCellStyle = DataGridViewCellStyle7
        Me.slavesConfigurationChannelNameColumn.HeaderText = "Channel Name"
        Me.slavesConfigurationChannelNameColumn.Name = "slavesConfigurationChannelNameColumn"
        Me.slavesConfigurationChannelNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesConfigurationChannelNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slavesConfigurationChannelNameColumn.Width = 90
        '
        'slavesConfigurationCurrentLimitColumn
        '
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black
        Me.slavesConfigurationCurrentLimitColumn.DefaultCellStyle = DataGridViewCellStyle8
        Me.slavesConfigurationCurrentLimitColumn.HeaderText = "Current Limit (A)"
        Me.slavesConfigurationCurrentLimitColumn.Name = "slavesConfigurationCurrentLimitColumn"
        Me.slavesConfigurationCurrentLimitColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesConfigurationCurrentLimitColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'slave0MeasurementsGroupBox
        '
        Me.slave0MeasurementsGroupBox.Controls.Add(Me.slave0MeasurementsDataGridView)
        Me.slave0MeasurementsGroupBox.Location = New System.Drawing.Point(279, 19)
        Me.slave0MeasurementsGroupBox.Name = "slave0MeasurementsGroupBox"
        Me.slave0MeasurementsGroupBox.Size = New System.Drawing.Size(223, 119)
        Me.slave0MeasurementsGroupBox.TabIndex = 1
        Me.slave0MeasurementsGroupBox.TabStop = False
        Me.slave0MeasurementsGroupBox.Text = "Slave 0 Measurements"
        '
        'slave0MeasurementsDataGridView
        '
        Me.slave0MeasurementsDataGridView.AllowUserToAddRows = False
        Me.slave0MeasurementsDataGridView.AllowUserToDeleteRows = False
        Me.slave0MeasurementsDataGridView.AllowUserToResizeColumns = False
        Me.slave0MeasurementsDataGridView.AllowUserToResizeRows = False
        Me.slave0MeasurementsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.slave0MeasurementsDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle9
        Me.slave0MeasurementsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.slave0MeasurementsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.slave0MeasurementsStepColumn, Me.slave0MeasurementsVoltageColumn, Me.slave0MeasurementsCurrentColumn})
        Me.slave0MeasurementsDataGridView.Location = New System.Drawing.Point(9, 19)
        Me.slave0MeasurementsDataGridView.Name = "slave0MeasurementsDataGridView"
        Me.slave0MeasurementsDataGridView.ReadOnly = True
        Me.slave0MeasurementsDataGridView.RowHeadersVisible = False
        Me.slave0MeasurementsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.slave0MeasurementsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slave0MeasurementsDataGridView.Size = New System.Drawing.Size(207, 91)
        Me.slave0MeasurementsDataGridView.StandardTab = True
        Me.slave0MeasurementsDataGridView.TabIndex = 0
        '
        'slave0MeasurementsStepColumn
        '
        DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.Black
        Me.slave0MeasurementsStepColumn.DefaultCellStyle = DataGridViewCellStyle10
        Me.slave0MeasurementsStepColumn.Frozen = True
        Me.slave0MeasurementsStepColumn.HeaderText = "Step"
        Me.slave0MeasurementsStepColumn.Name = "slave0MeasurementsStepColumn"
        Me.slave0MeasurementsStepColumn.ReadOnly = True
        Me.slave0MeasurementsStepColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slave0MeasurementsStepColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slave0MeasurementsStepColumn.Width = 35
        '
        'slave0MeasurementsVoltageColumn
        '
        DataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.Black
        Me.slave0MeasurementsVoltageColumn.DefaultCellStyle = DataGridViewCellStyle11
        Me.slave0MeasurementsVoltageColumn.HeaderText = "Voltage (V)"
        Me.slave0MeasurementsVoltageColumn.Name = "slave0MeasurementsVoltageColumn"
        Me.slave0MeasurementsVoltageColumn.ReadOnly = True
        Me.slave0MeasurementsVoltageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slave0MeasurementsVoltageColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slave0MeasurementsVoltageColumn.Width = 84
        '
        'slave0MeasurementsCurrentColumn
        '
        DataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle12.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.Black
        Me.slave0MeasurementsCurrentColumn.DefaultCellStyle = DataGridViewCellStyle12
        Me.slave0MeasurementsCurrentColumn.HeaderText = "Current (A)"
        Me.slave0MeasurementsCurrentColumn.Name = "slave0MeasurementsCurrentColumn"
        Me.slave0MeasurementsCurrentColumn.ReadOnly = True
        Me.slave0MeasurementsCurrentColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slave0MeasurementsCurrentColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slave0MeasurementsCurrentColumn.Width = 85
        '
        'masterMeasurementsGroupBox
        '
        Me.masterMeasurementsGroupBox.Controls.Add(Me.masterMeasurementDataGridView)
        Me.masterMeasurementsGroupBox.Location = New System.Drawing.Point(6, 19)
        Me.masterMeasurementsGroupBox.Name = "masterMeasurementsGroupBox"
        Me.masterMeasurementsGroupBox.Size = New System.Drawing.Size(223, 119)
        Me.masterMeasurementsGroupBox.TabIndex = 0
        Me.masterMeasurementsGroupBox.TabStop = False
        Me.masterMeasurementsGroupBox.Text = "Master Measurements"
        '
        'masterMeasurementDataGridView
        '
        Me.masterMeasurementDataGridView.AllowUserToAddRows = False
        Me.masterMeasurementDataGridView.AllowUserToDeleteRows = False
        Me.masterMeasurementDataGridView.AllowUserToResizeColumns = False
        Me.masterMeasurementDataGridView.AllowUserToResizeRows = False
        Me.masterMeasurementDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.masterMeasurementDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle13
        Me.masterMeasurementDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.masterMeasurementDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.masterMeasurementsStepColumn, Me.masterMeasurementsVoltageColumn, Me.masterMeasurementsCurrentColumn})
        Me.masterMeasurementDataGridView.Location = New System.Drawing.Point(9, 19)
        Me.masterMeasurementDataGridView.Name = "masterMeasurementDataGridView"
        Me.masterMeasurementDataGridView.ReadOnly = True
        Me.masterMeasurementDataGridView.RowHeadersVisible = False
        Me.masterMeasurementDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.masterMeasurementDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.masterMeasurementDataGridView.Size = New System.Drawing.Size(207, 91)
        Me.masterMeasurementDataGridView.StandardTab = True
        Me.masterMeasurementDataGridView.TabIndex = 0
        '
        'masterMeasurementsStepColumn
        '
        DataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle14.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.Black
        Me.masterMeasurementsStepColumn.DefaultCellStyle = DataGridViewCellStyle14
        Me.masterMeasurementsStepColumn.Frozen = True
        Me.masterMeasurementsStepColumn.HeaderText = "Step"
        Me.masterMeasurementsStepColumn.Name = "masterMeasurementsStepColumn"
        Me.masterMeasurementsStepColumn.ReadOnly = True
        Me.masterMeasurementsStepColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.masterMeasurementsStepColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.masterMeasurementsStepColumn.Width = 35
        '
        'masterMeasurementsVoltageColumn
        '
        DataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle15.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.Black
        Me.masterMeasurementsVoltageColumn.DefaultCellStyle = DataGridViewCellStyle15
        Me.masterMeasurementsVoltageColumn.HeaderText = "Voltage (V)"
        Me.masterMeasurementsVoltageColumn.Name = "masterMeasurementsVoltageColumn"
        Me.masterMeasurementsVoltageColumn.ReadOnly = True
        Me.masterMeasurementsVoltageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.masterMeasurementsVoltageColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.masterMeasurementsVoltageColumn.Width = 84
        '
        'masterMeasurementsCurrentColumn
        '
        DataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle16.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.Black
        Me.masterMeasurementsCurrentColumn.DefaultCellStyle = DataGridViewCellStyle16
        Me.masterMeasurementsCurrentColumn.HeaderText = "Current (A)"
        Me.masterMeasurementsCurrentColumn.Name = "masterMeasurementsCurrentColumn"
        Me.masterMeasurementsCurrentColumn.ReadOnly = True
        Me.masterMeasurementsCurrentColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.masterMeasurementsCurrentColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.masterMeasurementsCurrentColumn.Width = 85
        '
        'slave1MeasurementsGroupBox
        '
        Me.slave1MeasurementsGroupBox.Controls.Add(Me.slave1MeasurementsDataGridView)
        Me.slave1MeasurementsGroupBox.Location = New System.Drawing.Point(550, 19)
        Me.slave1MeasurementsGroupBox.Name = "slave1MeasurementsGroupBox"
        Me.slave1MeasurementsGroupBox.Size = New System.Drawing.Size(223, 119)
        Me.slave1MeasurementsGroupBox.TabIndex = 2
        Me.slave1MeasurementsGroupBox.TabStop = False
        Me.slave1MeasurementsGroupBox.Text = "Slave 1 Measurements"
        '
        'slave1MeasurementsDataGridView
        '
        Me.slave1MeasurementsDataGridView.AllowUserToAddRows = False
        Me.slave1MeasurementsDataGridView.AllowUserToDeleteRows = False
        Me.slave1MeasurementsDataGridView.AllowUserToResizeColumns = False
        Me.slave1MeasurementsDataGridView.AllowUserToResizeRows = False
        Me.slave1MeasurementsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.slave1MeasurementsDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle17
        Me.slave1MeasurementsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.slave1MeasurementsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.slave1MeasurementsStepColumn, Me.slave1MeasurementsVoltageColumn, Me.slave1MeasurementsCurrentColumn})
        Me.slave1MeasurementsDataGridView.Location = New System.Drawing.Point(9, 19)
        Me.slave1MeasurementsDataGridView.Name = "slave1MeasurementsDataGridView"
        Me.slave1MeasurementsDataGridView.ReadOnly = True
        Me.slave1MeasurementsDataGridView.RowHeadersVisible = False
        Me.slave1MeasurementsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.slave1MeasurementsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slave1MeasurementsDataGridView.Size = New System.Drawing.Size(207, 91)
        Me.slave1MeasurementsDataGridView.StandardTab = True
        Me.slave1MeasurementsDataGridView.TabIndex = 0
        '
        'slave1MeasurementsStepColumn
        '
        DataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle18.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.Black
        Me.slave1MeasurementsStepColumn.DefaultCellStyle = DataGridViewCellStyle18
        Me.slave1MeasurementsStepColumn.Frozen = True
        Me.slave1MeasurementsStepColumn.HeaderText = "Step"
        Me.slave1MeasurementsStepColumn.Name = "slave1MeasurementsStepColumn"
        Me.slave1MeasurementsStepColumn.ReadOnly = True
        Me.slave1MeasurementsStepColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slave1MeasurementsStepColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slave1MeasurementsStepColumn.Width = 35
        '
        'slave1MeasurementsVoltageColumn
        '
        DataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle19.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle19.SelectionForeColor = System.Drawing.Color.Black
        Me.slave1MeasurementsVoltageColumn.DefaultCellStyle = DataGridViewCellStyle19
        Me.slave1MeasurementsVoltageColumn.HeaderText = "Voltage (V)"
        Me.slave1MeasurementsVoltageColumn.Name = "slave1MeasurementsVoltageColumn"
        Me.slave1MeasurementsVoltageColumn.ReadOnly = True
        Me.slave1MeasurementsVoltageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slave1MeasurementsVoltageColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slave1MeasurementsVoltageColumn.Width = 84
        '
        'slave1MeasurementsCurrentColumn
        '
        DataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle20.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle20.SelectionForeColor = System.Drawing.Color.Black
        Me.slave1MeasurementsCurrentColumn.DefaultCellStyle = DataGridViewCellStyle20
        Me.slave1MeasurementsCurrentColumn.HeaderText = "Current (A)"
        Me.slave1MeasurementsCurrentColumn.Name = "slave1MeasurementsCurrentColumn"
        Me.slave1MeasurementsCurrentColumn.ReadOnly = True
        Me.slave1MeasurementsCurrentColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slave1MeasurementsCurrentColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slave1MeasurementsCurrentColumn.Width = 85
        '
        'slavesSequenceGroupBox
        '
        Me.slavesSequenceGroupBox.Controls.Add(Me.slavesSequenceDataGridView)
        Me.slavesSequenceGroupBox.Location = New System.Drawing.Point(419, 126)
        Me.slavesSequenceGroupBox.Name = "slavesSequenceGroupBox"
        Me.slavesSequenceGroupBox.Size = New System.Drawing.Size(223, 118)
        Me.slavesSequenceGroupBox.TabIndex = 2
        Me.slavesSequenceGroupBox.TabStop = False
        Me.slavesSequenceGroupBox.Text = "Slave(s) Sequence"
        '
        'slavesSequenceDataGridView
        '
        Me.slavesSequenceDataGridView.AllowUserToAddRows = False
        Me.slavesSequenceDataGridView.AllowUserToDeleteRows = False
        Me.slavesSequenceDataGridView.AllowUserToResizeColumns = False
        Me.slavesSequenceDataGridView.AllowUserToResizeRows = False
        Me.slavesSequenceDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.slavesSequenceDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.slavesSequenceDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle21
        Me.slavesSequenceDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.slavesSequenceDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.slavesSequenceStepColumn, Me.slavesSequenceSlave0Column, Me.slavesSequenceSlave1Column})
        Me.slavesSequenceDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.slavesSequenceDataGridView.Name = "slavesSequenceDataGridView"
        Me.slavesSequenceDataGridView.RowHeadersVisible = False
        Me.slavesSequenceDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.slavesSequenceDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesSequenceDataGridView.Size = New System.Drawing.Size(207, 91)
        Me.slavesSequenceDataGridView.StandardTab = True
        Me.slavesSequenceDataGridView.TabIndex = 0
        '
        'slavesSequenceStepColumn
        '
        DataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle22.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle22.SelectionForeColor = System.Drawing.Color.Black
        Me.slavesSequenceStepColumn.DefaultCellStyle = DataGridViewCellStyle22
        Me.slavesSequenceStepColumn.Frozen = True
        Me.slavesSequenceStepColumn.HeaderText = "Step"
        Me.slavesSequenceStepColumn.Name = "slavesSequenceStepColumn"
        Me.slavesSequenceStepColumn.ReadOnly = True
        Me.slavesSequenceStepColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesSequenceStepColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slavesSequenceStepColumn.Width = 35
        '
        'slavesSequenceSlave0Column
        '
        DataGridViewCellStyle23.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle23.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle23.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle23.SelectionForeColor = System.Drawing.Color.Black
        Me.slavesSequenceSlave0Column.DefaultCellStyle = DataGridViewCellStyle23
        Me.slavesSequenceSlave0Column.HeaderText = "Slave 0"
        Me.slavesSequenceSlave0Column.Name = "slavesSequenceSlave0Column"
        Me.slavesSequenceSlave0Column.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesSequenceSlave0Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slavesSequenceSlave0Column.Width = 84
        '
        'slavesSequenceSlave1Column
        '
        DataGridViewCellStyle24.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle24.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle24.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle24.SelectionForeColor = System.Drawing.Color.Black
        Me.slavesSequenceSlave1Column.DefaultCellStyle = DataGridViewCellStyle24
        Me.slavesSequenceSlave1Column.HeaderText = "Slave 1"
        Me.slavesSequenceSlave1Column.Name = "slavesSequenceSlave1Column"
        Me.slavesSequenceSlave1Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slavesSequenceSlave1Column.Width = 85
        '
        'measurementsGroupBox
        '
        Me.measurementsGroupBox.Controls.Add(Me.slave1MeasurementsGroupBox)
        Me.measurementsGroupBox.Controls.Add(Me.masterMeasurementsGroupBox)
        Me.measurementsGroupBox.Controls.Add(Me.slave0MeasurementsGroupBox)
        Me.measurementsGroupBox.Location = New System.Drawing.Point(12, 353)
        Me.measurementsGroupBox.Name = "measurementsGroupBox"
        Me.measurementsGroupBox.Size = New System.Drawing.Size(780, 148)
        Me.measurementsGroupBox.TabIndex = 4
        Me.measurementsGroupBox.TabStop = False
        Me.measurementsGroupBox.Text = "Measurements"
        '
        'messageRichTextBox
        '
        Me.messageRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.messageRichTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.messageRichTextBox.Location = New System.Drawing.Point(507, 311)
        Me.messageRichTextBox.Name = "messageRichTextBox"
        Me.messageRichTextBox.ReadOnly = True
        Me.messageRichTextBox.Size = New System.Drawing.Size(197, 15)
        Me.messageRichTextBox.TabIndex = 6
        Me.messageRichTextBox.TabStop = False
        Me.messageRichTextBox.Text = "This example requires 3 devices to work."
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(804, 512)
        Me.Controls.Add(Me.messageRichTextBox)
        Me.Controls.Add(Me.measurementsGroupBox)
        Me.Controls.Add(Me.slavesSequenceGroupBox)
        Me.Controls.Add(Me.slavesConfigurationGroupBox)
        Me.Controls.Add(Me.masterConfigurationGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Sequence Multi-Channel Synchronization"
        CType(Me.masterConfigurationCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.masterSourceDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.masterConfigurationGroupBox.ResumeLayout(False)
        Me.masterConfigurationSubGroupBox.ResumeLayout(False)
        Me.masterConfigurationSubGroupBox.PerformLayout()
        Me.masterSequenceGroupBox.ResumeLayout(False)
        CType(Me.masterSequenceDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.masterMeasureCompleteEventDelayGroupBox.ResumeLayout(False)
        CType(Me.masterMeasureCompleteEventDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.masterSourceDelayGroupBox.ResumeLayout(False)
        Me.slavesConfigurationGroupBox.ResumeLayout(False)
        CType(Me.slavesConfigurationDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.slave0MeasurementsGroupBox.ResumeLayout(False)
        CType(Me.slave0MeasurementsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.masterMeasurementsGroupBox.ResumeLayout(False)
        CType(Me.masterMeasurementDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.slave1MeasurementsGroupBox.ResumeLayout(False)
        CType(Me.slave1MeasurementsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.slavesSequenceGroupBox.ResumeLayout(False)
        CType(Me.slavesSequenceDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementsGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private masterConfigurationCurrentLimitLabel As System.Windows.Forms.Label
    Private masterConfigurationCurrentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private masterSourceDelayNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents startButton As System.Windows.Forms.Button
    Private masterConfigurationGroupBox As System.Windows.Forms.GroupBox
    Private masterConfigurationResourceNameComboBox As System.Windows.Forms.ComboBox
    Private masterConfigurationChannelNameLabel As System.Windows.Forms.Label
    Private masterConfigurationResourceNameLabel As System.Windows.Forms.Label
    Private masterSourceDelayMessageRichTextBox As System.Windows.Forms.RichTextBox
    Private slavesConfigurationGroupBox As System.Windows.Forms.GroupBox
    Private slave0MeasurementsGroupBox As System.Windows.Forms.GroupBox
    Private slavesConfigurationDataGridView As System.Windows.Forms.DataGridView
    Private masterMeasureCompleteEventDelayGroupBox As System.Windows.Forms.GroupBox
    Private masterMeasureCompleteEventDelayMessageRichTextBox As System.Windows.Forms.RichTextBox
    Private masterMeasureCompleteEventDelayNumeric As System.Windows.Forms.NumericUpDown
    Private masterSourceDelayGroupBox As System.Windows.Forms.GroupBox
    Private masterConfigurationSubGroupBox As System.Windows.Forms.GroupBox
    Private masterSequenceGroupBox As System.Windows.Forms.GroupBox
    Private masterSequenceDataGridView As System.Windows.Forms.DataGridView
    Private masterMeasurementsGroupBox As System.Windows.Forms.GroupBox
    Private masterMeasurementDataGridView As System.Windows.Forms.DataGridView
    Private slave0MeasurementsDataGridView As System.Windows.Forms.DataGridView
    Private slave1MeasurementsGroupBox As System.Windows.Forms.GroupBox
    Private slave1MeasurementsDataGridView As System.Windows.Forms.DataGridView
    Private slavesSequenceGroupBox As System.Windows.Forms.GroupBox
    Private slavesSequenceDataGridView As System.Windows.Forms.DataGridView
    Private measurementsGroupBox As System.Windows.Forms.GroupBox
    Private messageRichTextBox As System.Windows.Forms.RichTextBox
    Private masterConfigurationChannelNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents masterSequenceStepColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents masterSequenceValueColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents slave0MeasurementsStepColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents slave0MeasurementsVoltageColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents slave0MeasurementsCurrentColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents masterMeasurementsStepColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents masterMeasurementsVoltageColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents masterMeasurementsCurrentColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents slave1MeasurementsStepColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents slave1MeasurementsVoltageColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents slave1MeasurementsCurrentColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents slavesConfigurationSlaveNumColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents resourceNameColumn As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents slavesConfigurationChannelNameColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents slavesConfigurationCurrentLimitColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents slavesSequenceStepColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents slavesSequenceSlave0Column As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents slavesSequenceSlave1Column As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
