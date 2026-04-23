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
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.masterConfigurationCurrentLimitLabel = New System.Windows.Forms.Label()
        Me.masterConfigurationVoltageLevelLabel = New System.Windows.Forms.Label()
        Me.masterConfigurationDelayLabel = New System.Windows.Forms.Label()
        Me.masterConfigurationCurrentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.masterConfigurationVoltageLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.masterConfigurationSourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startButton = New System.Windows.Forms.Button()
        Me.masterConfigurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.masterConfigurationChannelNameTextBox = New System.Windows.Forms.TextBox()
        Me.masterConfigurationResourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.masterConfigurationChannelNameLabel = New System.Windows.Forms.Label()
        Me.masterConfigurationResourceNameLabel = New System.Windows.Forms.Label()
        Me.masterMeasurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.masterMeasurementsVoltageTextBox = New System.Windows.Forms.TextBox()
        Me.masterMeasurementsCurrentTextBox = New System.Windows.Forms.TextBox()
        Me.masterMeasurementsCurrentLabel = New System.Windows.Forms.Label()
        Me.masterMeasurementsVoltageLabel = New System.Windows.Forms.Label()
        Me.messageRichTextBox = New System.Windows.Forms.RichTextBox()
        Me.slavesConfigurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.slavesConfigurationDataGridView = New System.Windows.Forms.DataGridView()
        Me.slavesConfigurationSlaveNumberColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slavesConfigurationResourceNameColumn = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.slavesConfigurationChannelNameColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slavesConfigurationVoltageLevelColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slavesConfigurationCurrentLimitColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slavesMeasurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.slavesMeasurementsDataGridView = New System.Windows.Forms.DataGridView()
        Me.slavesMeasurementsSlaveNumberColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slavesMeasurementsVoltageColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.slavesMeasurementsCurrentColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.masterConfigurationCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.masterConfigurationVoltageLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.masterConfigurationSourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.masterConfigurationGroupBox.SuspendLayout()
        Me.masterMeasurementsGroupBox.SuspendLayout()
        Me.slavesConfigurationGroupBox.SuspendLayout()
        CType(Me.slavesConfigurationDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.slavesMeasurementsGroupBox.SuspendLayout()
        CType(Me.slavesMeasurementsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'masterConfigurationCurrentLimitLabel
        '
        Me.masterConfigurationCurrentLimitLabel.AutoSize = True
        Me.masterConfigurationCurrentLimitLabel.Location = New System.Drawing.Point(6, 111)
        Me.masterConfigurationCurrentLimitLabel.Name = "masterConfigurationCurrentLimitLabel"
        Me.masterConfigurationCurrentLimitLabel.Size = New System.Drawing.Size(81, 13)
        Me.masterConfigurationCurrentLimitLabel.TabIndex = 6
        Me.masterConfigurationCurrentLimitLabel.Text = "Current Limit (A)"
        '
        'masterConfigurationVoltageLevelLabel
        '
        Me.masterConfigurationVoltageLevelLabel.AutoSize = True
        Me.masterConfigurationVoltageLevelLabel.Location = New System.Drawing.Point(6, 85)
        Me.masterConfigurationVoltageLevelLabel.Name = "masterConfigurationVoltageLevelLabel"
        Me.masterConfigurationVoltageLevelLabel.Size = New System.Drawing.Size(88, 13)
        Me.masterConfigurationVoltageLevelLabel.TabIndex = 4
        Me.masterConfigurationVoltageLevelLabel.Text = "Voltage Level (V)"
        '
        'masterConfigurationDelayLabel
        '
        Me.masterConfigurationDelayLabel.AutoSize = True
        Me.masterConfigurationDelayLabel.Location = New System.Drawing.Point(6, 137)
        Me.masterConfigurationDelayLabel.Name = "masterConfigurationDelayLabel"
        Me.masterConfigurationDelayLabel.Size = New System.Drawing.Size(85, 13)
        Me.masterConfigurationDelayLabel.TabIndex = 8
        Me.masterConfigurationDelayLabel.Text = "Source Delay (s)"
        '
        'masterConfigurationCurrentLimitNumeric
        '
        Me.masterConfigurationCurrentLimitNumeric.DecimalPlaces = 6
        Me.masterConfigurationCurrentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.masterConfigurationCurrentLimitNumeric.Location = New System.Drawing.Point(131, 107)
        Me.masterConfigurationCurrentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.masterConfigurationCurrentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.masterConfigurationCurrentLimitNumeric.Name = "masterConfigurationCurrentLimitNumeric"
        Me.masterConfigurationCurrentLimitNumeric.Size = New System.Drawing.Size(90, 20)
        Me.masterConfigurationCurrentLimitNumeric.TabIndex = 4
        Me.masterConfigurationCurrentLimitNumeric.Value = New Decimal(New Integer() {2, 0, 0, 131072})
        '
        'masterConfigurationVoltageLevelNumeric
        '
        Me.masterConfigurationVoltageLevelNumeric.DecimalPlaces = 6
        Me.masterConfigurationVoltageLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.masterConfigurationVoltageLevelNumeric.Location = New System.Drawing.Point(131, 81)
        Me.masterConfigurationVoltageLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.masterConfigurationVoltageLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.masterConfigurationVoltageLevelNumeric.Name = "masterConfigurationVoltageLevelNumeric"
        Me.masterConfigurationVoltageLevelNumeric.Size = New System.Drawing.Size(90, 20)
        Me.masterConfigurationVoltageLevelNumeric.TabIndex = 3
        Me.masterConfigurationVoltageLevelNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'masterConfigurationSourceDelayNumeric
        '
        Me.masterConfigurationSourceDelayNumeric.DecimalPlaces = 6
        Me.masterConfigurationSourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.masterConfigurationSourceDelayNumeric.Location = New System.Drawing.Point(131, 133)
        Me.masterConfigurationSourceDelayNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.masterConfigurationSourceDelayNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.masterConfigurationSourceDelayNumeric.Name = "masterConfigurationSourceDelayNumeric"
        Me.masterConfigurationSourceDelayNumeric.Size = New System.Drawing.Size(90, 20)
        Me.masterConfigurationSourceDelayNumeric.TabIndex = 5
        Me.masterConfigurationSourceDelayNumeric.Value = New Decimal(New Integer() {5, 0, 0, 131072})
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(208, 349)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 7
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'masterConfigurationGroupBox
        '
        Me.masterConfigurationGroupBox.Controls.Add(Me.masterConfigurationChannelNameTextBox)
        Me.masterConfigurationGroupBox.Controls.Add(Me.masterConfigurationResourceNameComboBox)
        Me.masterConfigurationGroupBox.Controls.Add(Me.masterConfigurationChannelNameLabel)
        Me.masterConfigurationGroupBox.Controls.Add(Me.masterConfigurationDelayLabel)
        Me.masterConfigurationGroupBox.Controls.Add(Me.masterConfigurationCurrentLimitLabel)
        Me.masterConfigurationGroupBox.Controls.Add(Me.masterConfigurationResourceNameLabel)
        Me.masterConfigurationGroupBox.Controls.Add(Me.masterConfigurationVoltageLevelLabel)
        Me.masterConfigurationGroupBox.Controls.Add(Me.masterConfigurationVoltageLevelNumeric)
        Me.masterConfigurationGroupBox.Controls.Add(Me.masterConfigurationCurrentLimitNumeric)
        Me.masterConfigurationGroupBox.Controls.Add(Me.masterConfigurationSourceDelayNumeric)
        Me.masterConfigurationGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.masterConfigurationGroupBox.Name = "masterConfigurationGroupBox"
        Me.masterConfigurationGroupBox.Size = New System.Drawing.Size(231, 164)
        Me.masterConfigurationGroupBox.TabIndex = 0
        Me.masterConfigurationGroupBox.TabStop = False
        Me.masterConfigurationGroupBox.Text = "Master Configuration"
        '
        'masterConfigurationChannelNameTextBox
        '
        Me.masterConfigurationChannelNameTextBox.Location = New System.Drawing.Point(131, 46)
        Me.masterConfigurationChannelNameTextBox.Name = "masterConfigurationChannelNameTextBox"
        Me.masterConfigurationChannelNameTextBox.Size = New System.Drawing.Size(90, 20)
        Me.masterConfigurationChannelNameTextBox.TabIndex = 2
        Me.masterConfigurationChannelNameTextBox.Text = "0"
        '
        'masterConfigurationResourceNameComboBox
        '
        Me.masterConfigurationResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.masterConfigurationResourceNameComboBox.FormattingEnabled = True
        Me.masterConfigurationResourceNameComboBox.Location = New System.Drawing.Point(96, 19)
        Me.masterConfigurationResourceNameComboBox.Name = "masterConfigurationResourceNameComboBox"
        Me.masterConfigurationResourceNameComboBox.Size = New System.Drawing.Size(125, 21)
        Me.masterConfigurationResourceNameComboBox.TabIndex = 1
        '
        'masterConfigurationChannelNameLabel
        '
        Me.masterConfigurationChannelNameLabel.AutoSize = True
        Me.masterConfigurationChannelNameLabel.Location = New System.Drawing.Point(6, 50)
        Me.masterConfigurationChannelNameLabel.Name = "masterConfigurationChannelNameLabel"
        Me.masterConfigurationChannelNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.masterConfigurationChannelNameLabel.TabIndex = 2
        Me.masterConfigurationChannelNameLabel.Text = "Channel Name"
        '
        'masterConfigurationResourceNameLabel
        '
        Me.masterConfigurationResourceNameLabel.AutoSize = True
        Me.masterConfigurationResourceNameLabel.Location = New System.Drawing.Point(6, 22)
        Me.masterConfigurationResourceNameLabel.Name = "masterConfigurationResourceNameLabel"
        Me.masterConfigurationResourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.masterConfigurationResourceNameLabel.TabIndex = 0
        Me.masterConfigurationResourceNameLabel.Text = "Resource Name"
        '
        'masterMeasurementsGroupBox
        '
        Me.masterMeasurementsGroupBox.Controls.Add(Me.masterMeasurementsVoltageTextBox)
        Me.masterMeasurementsGroupBox.Controls.Add(Me.masterMeasurementsCurrentTextBox)
        Me.masterMeasurementsGroupBox.Controls.Add(Me.masterMeasurementsCurrentLabel)
        Me.masterMeasurementsGroupBox.Controls.Add(Me.masterMeasurementsVoltageLabel)
        Me.masterMeasurementsGroupBox.Location = New System.Drawing.Point(259, 12)
        Me.masterMeasurementsGroupBox.Name = "masterMeasurementsGroupBox"
        Me.masterMeasurementsGroupBox.Size = New System.Drawing.Size(229, 74)
        Me.masterMeasurementsGroupBox.TabIndex = 1
        Me.masterMeasurementsGroupBox.TabStop = False
        Me.masterMeasurementsGroupBox.Text = "Master Measurements"
        '
        'masterMeasurementsVoltageTextBox
        '
        Me.masterMeasurementsVoltageTextBox.Location = New System.Drawing.Point(107, 19)
        Me.masterMeasurementsVoltageTextBox.Name = "masterMeasurementsVoltageTextBox"
        Me.masterMeasurementsVoltageTextBox.ReadOnly = True
        Me.masterMeasurementsVoltageTextBox.Size = New System.Drawing.Size(116, 20)
        Me.masterMeasurementsVoltageTextBox.TabIndex = 8
        Me.masterMeasurementsVoltageTextBox.Text = "0.000000E+000"
        '
        'masterMeasurementsCurrentTextBox
        '
        Me.masterMeasurementsCurrentTextBox.Location = New System.Drawing.Point(107, 45)
        Me.masterMeasurementsCurrentTextBox.Name = "masterMeasurementsCurrentTextBox"
        Me.masterMeasurementsCurrentTextBox.ReadOnly = True
        Me.masterMeasurementsCurrentTextBox.Size = New System.Drawing.Size(116, 20)
        Me.masterMeasurementsCurrentTextBox.TabIndex = 9
        Me.masterMeasurementsCurrentTextBox.Text = "0.000000E+000"
        '
        'masterMeasurementsCurrentLabel
        '
        Me.masterMeasurementsCurrentLabel.AutoSize = True
        Me.masterMeasurementsCurrentLabel.Location = New System.Drawing.Point(6, 49)
        Me.masterMeasurementsCurrentLabel.Name = "masterMeasurementsCurrentLabel"
        Me.masterMeasurementsCurrentLabel.Size = New System.Drawing.Size(57, 13)
        Me.masterMeasurementsCurrentLabel.TabIndex = 2
        Me.masterMeasurementsCurrentLabel.Text = "Current (A)"
        '
        'masterMeasurementsVoltageLabel
        '
        Me.masterMeasurementsVoltageLabel.AutoSize = True
        Me.masterMeasurementsVoltageLabel.Location = New System.Drawing.Point(6, 23)
        Me.masterMeasurementsVoltageLabel.Name = "masterMeasurementsVoltageLabel"
        Me.masterMeasurementsVoltageLabel.Size = New System.Drawing.Size(59, 13)
        Me.masterMeasurementsVoltageLabel.TabIndex = 0
        Me.masterMeasurementsVoltageLabel.Text = "Voltage (V)"
        '
        'messageRichTextBox
        '
        Me.messageRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.messageRichTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.messageRichTextBox.Location = New System.Drawing.Point(152, 222)
        Me.messageRichTextBox.Name = "messageRichTextBox"
        Me.messageRichTextBox.ReadOnly = True
        Me.messageRichTextBox.Size = New System.Drawing.Size(197, 19)
        Me.messageRichTextBox.TabIndex = 3
        Me.messageRichTextBox.TabStop = False
        Me.messageRichTextBox.Text = "This example requires 3 devices to work."
        '
        'slavesConfigurationGroupBox
        '
        Me.slavesConfigurationGroupBox.Controls.Add(Me.slavesConfigurationDataGridView)
        Me.slavesConfigurationGroupBox.Location = New System.Drawing.Point(12, 247)
        Me.slavesConfigurationGroupBox.Name = "slavesConfigurationGroupBox"
        Me.slavesConfigurationGroupBox.Size = New System.Drawing.Size(476, 96)
        Me.slavesConfigurationGroupBox.TabIndex = 4
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
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.slavesConfigurationDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.slavesConfigurationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.slavesConfigurationDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.slavesConfigurationSlaveNumberColumn, Me.slavesConfigurationResourceNameColumn, Me.slavesConfigurationChannelNameColumn, Me.slavesConfigurationVoltageLevelColumn, Me.slavesConfigurationCurrentLimitColumn})
        Me.slavesConfigurationDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.slavesConfigurationDataGridView.Name = "slavesConfigurationDataGridView"
        Me.slavesConfigurationDataGridView.RowHeadersVisible = False
        Me.slavesConfigurationDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.slavesConfigurationDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesConfigurationDataGridView.Size = New System.Drawing.Size(463, 69)
        Me.slavesConfigurationDataGridView.StandardTab = True
        Me.slavesConfigurationDataGridView.TabIndex = 6
        '
        'slavesConfigurationSlaveNumberColumn
        '
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        Me.slavesConfigurationSlaveNumberColumn.DefaultCellStyle = DataGridViewCellStyle2
        Me.slavesConfigurationSlaveNumberColumn.Frozen = True
        Me.slavesConfigurationSlaveNumberColumn.HeaderText = ""
        Me.slavesConfigurationSlaveNumberColumn.Name = "slavesConfigurationSlaveNumberColumn"
        Me.slavesConfigurationSlaveNumberColumn.ReadOnly = True
        Me.slavesConfigurationSlaveNumberColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesConfigurationSlaveNumberColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slavesConfigurationSlaveNumberColumn.Width = 50
        '
        'slavesConfigurationResourceNameColumn
        '
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black
        Me.slavesConfigurationResourceNameColumn.DefaultCellStyle = DataGridViewCellStyle3
        Me.slavesConfigurationResourceNameColumn.HeaderText = "Resource Name"
        Me.slavesConfigurationResourceNameColumn.Name = "slavesConfigurationResourceNameColumn"
        Me.slavesConfigurationResourceNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesConfigurationResourceNameColumn.Width = 115
        '
        'slavesConfigurationChannelNameColumn
        '
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black
        Me.slavesConfigurationChannelNameColumn.DefaultCellStyle = DataGridViewCellStyle4
        Me.slavesConfigurationChannelNameColumn.HeaderText = "Channel Name"
        Me.slavesConfigurationChannelNameColumn.Name = "slavesConfigurationChannelNameColumn"
        Me.slavesConfigurationChannelNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesConfigurationChannelNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slavesConfigurationChannelNameColumn.Width = 95
        '
        'slavesConfigurationVoltageLevelColumn
        '
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black
        Me.slavesConfigurationVoltageLevelColumn.DefaultCellStyle = DataGridViewCellStyle5
        Me.slavesConfigurationVoltageLevelColumn.HeaderText = "Voltage Level (V)"
        Me.slavesConfigurationVoltageLevelColumn.Name = "slavesConfigurationVoltageLevelColumn"
        Me.slavesConfigurationVoltageLevelColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesConfigurationVoltageLevelColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'slavesConfigurationCurrentLimitColumn
        '
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black
        Me.slavesConfigurationCurrentLimitColumn.DefaultCellStyle = DataGridViewCellStyle6
        Me.slavesConfigurationCurrentLimitColumn.HeaderText = "Current Limit (A)"
        Me.slavesConfigurationCurrentLimitColumn.Name = "slavesConfigurationCurrentLimitColumn"
        Me.slavesConfigurationCurrentLimitColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesConfigurationCurrentLimitColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'slavesMeasurementsGroupBox
        '
        Me.slavesMeasurementsGroupBox.Controls.Add(Me.slavesMeasurementsDataGridView)
        Me.slavesMeasurementsGroupBox.Location = New System.Drawing.Point(259, 97)
        Me.slavesMeasurementsGroupBox.Name = "slavesMeasurementsGroupBox"
        Me.slavesMeasurementsGroupBox.Size = New System.Drawing.Size(229, 96)
        Me.slavesMeasurementsGroupBox.TabIndex = 2
        Me.slavesMeasurementsGroupBox.TabStop = False
        Me.slavesMeasurementsGroupBox.Text = "Slave(s) Measurement"
        '
        'slavesMeasurementsDataGridView
        '
        Me.slavesMeasurementsDataGridView.AllowUserToAddRows = False
        Me.slavesMeasurementsDataGridView.AllowUserToDeleteRows = False
        Me.slavesMeasurementsDataGridView.AllowUserToResizeColumns = False
        Me.slavesMeasurementsDataGridView.AllowUserToResizeRows = False
        Me.slavesMeasurementsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.slavesMeasurementsDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.slavesMeasurementsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.slavesMeasurementsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.slavesMeasurementsSlaveNumberColumn, Me.slavesMeasurementsVoltageColumn, Me.slavesMeasurementsCurrentColumn})
        Me.slavesMeasurementsDataGridView.Location = New System.Drawing.Point(9, 19)
        Me.slavesMeasurementsDataGridView.Name = "slavesMeasurementsDataGridView"
        Me.slavesMeasurementsDataGridView.RowHeadersVisible = False
        Me.slavesMeasurementsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.slavesMeasurementsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesMeasurementsDataGridView.Size = New System.Drawing.Size(213, 69)
        Me.slavesMeasurementsDataGridView.StandardTab = True
        Me.slavesMeasurementsDataGridView.TabIndex = 10
        '
        'slavesMeasurementsSlaveNumberColumn
        '
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black
        Me.slavesMeasurementsSlaveNumberColumn.DefaultCellStyle = DataGridViewCellStyle8
        Me.slavesMeasurementsSlaveNumberColumn.Frozen = True
        Me.slavesMeasurementsSlaveNumberColumn.HeaderText = ""
        Me.slavesMeasurementsSlaveNumberColumn.Name = "slavesMeasurementsSlaveNumberColumn"
        Me.slavesMeasurementsSlaveNumberColumn.ReadOnly = True
        Me.slavesMeasurementsSlaveNumberColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesMeasurementsSlaveNumberColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slavesMeasurementsSlaveNumberColumn.Width = 50
        '
        'slavesMeasurementsVoltageColumn
        '
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.Black
        Me.slavesMeasurementsVoltageColumn.DefaultCellStyle = DataGridViewCellStyle9
        Me.slavesMeasurementsVoltageColumn.HeaderText = "Voltage (V)"
        Me.slavesMeasurementsVoltageColumn.Name = "slavesMeasurementsVoltageColumn"
        Me.slavesMeasurementsVoltageColumn.ReadOnly = True
        Me.slavesMeasurementsVoltageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesMeasurementsVoltageColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slavesMeasurementsVoltageColumn.Width = 80
        '
        'slavesMeasurementsCurrentColumn
        '
        DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.Black
        Me.slavesMeasurementsCurrentColumn.DefaultCellStyle = DataGridViewCellStyle10
        Me.slavesMeasurementsCurrentColumn.HeaderText = "Current (A)"
        Me.slavesMeasurementsCurrentColumn.Name = "slavesMeasurementsCurrentColumn"
        Me.slavesMeasurementsCurrentColumn.ReadOnly = True
        Me.slavesMeasurementsCurrentColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.slavesMeasurementsCurrentColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.slavesMeasurementsCurrentColumn.Width = 80
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(500, 382)
        Me.Controls.Add(Me.slavesMeasurementsGroupBox)
        Me.Controls.Add(Me.slavesConfigurationGroupBox)
        Me.Controls.Add(Me.messageRichTextBox)
        Me.Controls.Add(Me.masterMeasurementsGroupBox)
        Me.Controls.Add(Me.masterConfigurationGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Single Point Multi-Channel Synchronization"
        CType(Me.masterConfigurationCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.masterConfigurationVoltageLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.masterConfigurationSourceDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.masterConfigurationGroupBox.ResumeLayout(False)
        Me.masterConfigurationGroupBox.PerformLayout()
        Me.masterMeasurementsGroupBox.ResumeLayout(False)
        Me.masterMeasurementsGroupBox.PerformLayout()
        Me.slavesConfigurationGroupBox.ResumeLayout(False)
        CType(Me.slavesConfigurationDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.slavesMeasurementsGroupBox.ResumeLayout(False)
        CType(Me.slavesMeasurementsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private masterConfigurationCurrentLimitLabel As System.Windows.Forms.Label
    Private masterConfigurationVoltageLevelLabel As System.Windows.Forms.Label
    Private masterConfigurationDelayLabel As System.Windows.Forms.Label
    Private masterConfigurationCurrentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private masterConfigurationVoltageLevelNumeric As System.Windows.Forms.NumericUpDown
    Private masterConfigurationSourceDelayNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents startButton As System.Windows.Forms.Button
    Private masterConfigurationGroupBox As System.Windows.Forms.GroupBox
    Private masterConfigurationResourceNameComboBox As System.Windows.Forms.ComboBox
    Private masterConfigurationChannelNameLabel As System.Windows.Forms.Label
    Private masterConfigurationResourceNameLabel As System.Windows.Forms.Label
    Private masterMeasurementsGroupBox As System.Windows.Forms.GroupBox
    Private masterMeasurementsVoltageTextBox As System.Windows.Forms.TextBox
    Private masterMeasurementsCurrentTextBox As System.Windows.Forms.TextBox
    Private masterMeasurementsCurrentLabel As System.Windows.Forms.Label
    Private masterMeasurementsVoltageLabel As System.Windows.Forms.Label
    Private messageRichTextBox As System.Windows.Forms.RichTextBox
    Private slavesConfigurationGroupBox As System.Windows.Forms.GroupBox
    Private slavesMeasurementsGroupBox As System.Windows.Forms.GroupBox
    Private slavesConfigurationDataGridView As System.Windows.Forms.DataGridView
    Private slavesMeasurementsDataGridView As System.Windows.Forms.DataGridView
    Private slavesMeasurementsSlaveNumberColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private slavesMeasurementsVoltageColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private slavesMeasurementsCurrentColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private masterConfigurationChannelNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents slavesConfigurationSlaveNumberColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents slavesConfigurationResourceNameColumn As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents slavesConfigurationChannelNameColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents slavesConfigurationVoltageLevelColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents slavesConfigurationCurrentLimitColumn As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
