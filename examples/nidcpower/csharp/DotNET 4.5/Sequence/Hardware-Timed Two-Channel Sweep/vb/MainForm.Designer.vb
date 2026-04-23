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
        Me.startButton = New System.Windows.Forms.Button()
        Me.measurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.selectPlotLabel = New System.Windows.Forms.Label()
        Me.plotNameComboBox = New System.Windows.Forms.ComboBox()
        Me.measurementsDataGridView = New System.Windows.Forms.DataGridView()
        Me.readingNoColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.measuredVoltageColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.measuredCurrentColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.device0SenseLabel = New System.Windows.Forms.Label()
        Me.device0VoltageLevelStopLabel = New System.Windows.Forms.Label()
        Me.device0VoltageLevelStartLabel = New System.Windows.Forms.Label()
        Me.device0SourceDelayLabel = New System.Windows.Forms.Label()
        Me.device0NumberOfPlotsLabel = New System.Windows.Forms.Label()
        Me.device0VoltageLevelStopNumeric = New System.Windows.Forms.NumericUpDown()
        Me.device0CurrentLimitLabel = New System.Windows.Forms.Label()
        Me.device0SourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.device0VoltageLevelStartNumeric = New System.Windows.Forms.NumericUpDown()
        Me.device0NumberOfPlotsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.device0CurrentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.device0SenseComboBox = New System.Windows.Forms.ComboBox()
        Me.device0GroupBox = New System.Windows.Forms.GroupBox()
        Me.device0ChannelNameTextBox = New System.Windows.Forms.TextBox()
        Me.device0ResourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.device0ChannelNameLabel = New System.Windows.Forms.Label()
        Me.device0ResourceNameLabel = New System.Windows.Forms.Label()
        Me.device1GroupBox = New System.Windows.Forms.GroupBox()
        Me.device1ChannelNameTextBox = New System.Windows.Forms.TextBox()
        Me.device1VoltageLevelStopLabel = New System.Windows.Forms.Label()
        Me.device1SenseLabel = New System.Windows.Forms.Label()
        Me.device1VoltageLevelStopNumeric = New System.Windows.Forms.NumericUpDown()
        Me.device1ResourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.device1VoltageLevelStartLabel = New System.Windows.Forms.Label()
        Me.device1VoltageLevelStartNumeric = New System.Windows.Forms.NumericUpDown()
        Me.device1SourceDelayLabel = New System.Windows.Forms.Label()
        Me.device1NumberOfPointsLabel = New System.Windows.Forms.Label()
        Me.device1ChannelNameLabel = New System.Windows.Forms.Label()
        Me.device1NumberOfPointsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.device1ResourceNameLabel = New System.Windows.Forms.Label()
        Me.device1SourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.device1CurrentLimitLabel = New System.Windows.Forms.Label()
        Me.device1CurrentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.device1SenseComboBox = New System.Windows.Forms.ComboBox()
        Me.timeoutNumeric = New System.Windows.Forms.NumericUpDown()
        Me.timeoutGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementsGroupBox.SuspendLayout()
        CType(Me.measurementsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.device0VoltageLevelStopNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.device0SourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.device0VoltageLevelStartNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.device0NumberOfPlotsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.device0CurrentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.device0GroupBox.SuspendLayout()
        Me.device1GroupBox.SuspendLayout()
        CType(Me.device1VoltageLevelStopNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.device1VoltageLevelStartNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.device1NumberOfPointsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.device1SourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.device1CurrentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.timeoutNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.timeoutGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(108, 483)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 4
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'measurementsGroupBox
        '
        Me.measurementsGroupBox.Controls.Add(Me.selectPlotLabel)
        Me.measurementsGroupBox.Controls.Add(Me.plotNameComboBox)
        Me.measurementsGroupBox.Controls.Add(Me.measurementsDataGridView)
        Me.measurementsGroupBox.Location = New System.Drawing.Point(294, 78)
        Me.measurementsGroupBox.Name = "measurementsGroupBox"
        Me.measurementsGroupBox.Size = New System.Drawing.Size(265, 435)
        Me.measurementsGroupBox.TabIndex = 3
        Me.measurementsGroupBox.TabStop = False
        Me.measurementsGroupBox.Text = "Measurements"
        '
        'selectPlotLabel
        '
        Me.selectPlotLabel.AutoSize = True
        Me.selectPlotLabel.Location = New System.Drawing.Point(6, 23)
        Me.selectPlotLabel.Name = "selectPlotLabel"
        Me.selectPlotLabel.Size = New System.Drawing.Size(58, 13)
        Me.selectPlotLabel.TabIndex = 2
        Me.selectPlotLabel.Text = "Select Plot"
        '
        'plotNameComboBox
        '
        Me.plotNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.plotNameComboBox.FormattingEnabled = True
        Me.plotNameComboBox.Location = New System.Drawing.Point(68, 19)
        Me.plotNameComboBox.Name = "plotNameComboBox"
        Me.plotNameComboBox.Size = New System.Drawing.Size(191, 21)
        Me.plotNameComboBox.TabIndex = 3
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
        Me.measurementsDataGridView.Location = New System.Drawing.Point(6, 59)
        Me.measurementsDataGridView.Name = "measurementsDataGridView"
        Me.measurementsDataGridView.ReadOnly = True
        Me.measurementsDataGridView.RowHeadersVisible = False
        Me.measurementsDataGridView.RowHeadersWidth = 15
        Me.measurementsDataGridView.RowTemplate.Height = 24
        Me.measurementsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.measurementsDataGridView.Size = New System.Drawing.Size(253, 369)
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
        Me.readingNoColumn.Width = 50
        '
        'measuredVoltageColumn
        '
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        Me.measuredVoltageColumn.DefaultCellStyle = DataGridViewCellStyle2
        Me.measuredVoltageColumn.HeaderText = "Voltage (V)"
        Me.measuredVoltageColumn.Name = "measuredVoltageColumn"
        Me.measuredVoltageColumn.ReadOnly = True
        Me.measuredVoltageColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'measuredCurrentColumn
        '
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black
        Me.measuredCurrentColumn.DefaultCellStyle = DataGridViewCellStyle3
        Me.measuredCurrentColumn.HeaderText = "Current (A)"
        Me.measuredCurrentColumn.Name = "measuredCurrentColumn"
        Me.measuredCurrentColumn.ReadOnly = True
        Me.measuredCurrentColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'device0SenseLabel
        '
        Me.device0SenseLabel.AutoSize = True
        Me.device0SenseLabel.Location = New System.Drawing.Point(6, 125)
        Me.device0SenseLabel.Name = "device0SenseLabel"
        Me.device0SenseLabel.Size = New System.Drawing.Size(37, 13)
        Me.device0SenseLabel.TabIndex = 8
        Me.device0SenseLabel.Text = "Sense"
        '
        'device0VoltageLevelStopLabel
        '
        Me.device0VoltageLevelStopLabel.AutoSize = True
        Me.device0VoltageLevelStopLabel.Location = New System.Drawing.Point(147, 175)
        Me.device0VoltageLevelStopLabel.Name = "device0VoltageLevelStopLabel"
        Me.device0VoltageLevelStopLabel.Size = New System.Drawing.Size(113, 13)
        Me.device0VoltageLevelStopLabel.TabIndex = 14
        Me.device0VoltageLevelStopLabel.Text = "Voltage Level Stop (V)"
        '
        'device0VoltageLevelStartLabel
        '
        Me.device0VoltageLevelStartLabel.AutoSize = True
        Me.device0VoltageLevelStartLabel.Location = New System.Drawing.Point(147, 125)
        Me.device0VoltageLevelStartLabel.Name = "device0VoltageLevelStartLabel"
        Me.device0VoltageLevelStartLabel.Size = New System.Drawing.Size(113, 13)
        Me.device0VoltageLevelStartLabel.TabIndex = 10
        Me.device0VoltageLevelStartLabel.Text = "Voltage Level Start (V)"
        '
        'device0SourceDelayLabel
        '
        Me.device0SourceDelayLabel.AutoSize = True
        Me.device0SourceDelayLabel.Location = New System.Drawing.Point(6, 175)
        Me.device0SourceDelayLabel.Name = "device0SourceDelayLabel"
        Me.device0SourceDelayLabel.Size = New System.Drawing.Size(85, 13)
        Me.device0SourceDelayLabel.TabIndex = 12
        Me.device0SourceDelayLabel.Text = "Source Delay (s)"
        '
        'device0NumberOfPlotsLabel
        '
        Me.device0NumberOfPlotsLabel.AutoSize = True
        Me.device0NumberOfPlotsLabel.Location = New System.Drawing.Point(147, 76)
        Me.device0NumberOfPlotsLabel.Name = "device0NumberOfPlotsLabel"
        Me.device0NumberOfPlotsLabel.Size = New System.Drawing.Size(30, 13)
        Me.device0NumberOfPlotsLabel.TabIndex = 6
        Me.device0NumberOfPlotsLabel.Text = "Plots"
        '
        'device0VoltageLevelStopNumeric
        '
        Me.device0VoltageLevelStopNumeric.DecimalPlaces = 6
        Me.device0VoltageLevelStopNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.device0VoltageLevelStopNumeric.Location = New System.Drawing.Point(147, 191)
        Me.device0VoltageLevelStopNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.device0VoltageLevelStopNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.device0VoltageLevelStopNumeric.Name = "device0VoltageLevelStopNumeric"
        Me.device0VoltageLevelStopNumeric.Size = New System.Drawing.Size(100, 20)
        Me.device0VoltageLevelStopNumeric.TabIndex = 15
        Me.device0VoltageLevelStopNumeric.Value = New Decimal(New Integer() {39, 0, 0, 65536})
        '
        'device0CurrentLimitLabel
        '
        Me.device0CurrentLimitLabel.AutoSize = True
        Me.device0CurrentLimitLabel.Location = New System.Drawing.Point(6, 76)
        Me.device0CurrentLimitLabel.Name = "device0CurrentLimitLabel"
        Me.device0CurrentLimitLabel.Size = New System.Drawing.Size(81, 13)
        Me.device0CurrentLimitLabel.TabIndex = 4
        Me.device0CurrentLimitLabel.Text = "Current Limit (A)"
        '
        'device0SourceDelayNumeric
        '
        Me.device0SourceDelayNumeric.DecimalPlaces = 6
        Me.device0SourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.device0SourceDelayNumeric.Location = New System.Drawing.Point(6, 191)
        Me.device0SourceDelayNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.device0SourceDelayNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.device0SourceDelayNumeric.Name = "device0SourceDelayNumeric"
        Me.device0SourceDelayNumeric.Size = New System.Drawing.Size(100, 20)
        Me.device0SourceDelayNumeric.TabIndex = 13
        Me.device0SourceDelayNumeric.Value = New Decimal(New Integer() {2, 0, 0, 196608})
        '
        'device0VoltageLevelStartNumeric
        '
        Me.device0VoltageLevelStartNumeric.DecimalPlaces = 6
        Me.device0VoltageLevelStartNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.device0VoltageLevelStartNumeric.Location = New System.Drawing.Point(147, 141)
        Me.device0VoltageLevelStartNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.device0VoltageLevelStartNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.device0VoltageLevelStartNumeric.Name = "device0VoltageLevelStartNumeric"
        Me.device0VoltageLevelStartNumeric.Size = New System.Drawing.Size(100, 20)
        Me.device0VoltageLevelStartNumeric.TabIndex = 11
        Me.device0VoltageLevelStartNumeric.Value = New Decimal(New Integer() {35, 0, 0, 65536})
        '
        'device0NumberOfPlotsNumeric
        '
        Me.device0NumberOfPlotsNumeric.Location = New System.Drawing.Point(147, 92)
        Me.device0NumberOfPlotsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.device0NumberOfPlotsNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.device0NumberOfPlotsNumeric.Name = "device0NumberOfPlotsNumeric"
        Me.device0NumberOfPlotsNumeric.Size = New System.Drawing.Size(100, 20)
        Me.device0NumberOfPlotsNumeric.TabIndex = 7
        Me.device0NumberOfPlotsNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'device0CurrentLimitNumeric
        '
        Me.device0CurrentLimitNumeric.DecimalPlaces = 6
        Me.device0CurrentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.device0CurrentLimitNumeric.Location = New System.Drawing.Point(6, 92)
        Me.device0CurrentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.device0CurrentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.device0CurrentLimitNumeric.Name = "device0CurrentLimitNumeric"
        Me.device0CurrentLimitNumeric.Size = New System.Drawing.Size(100, 20)
        Me.device0CurrentLimitNumeric.TabIndex = 5
        Me.device0CurrentLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'device0SenseComboBox
        '
        Me.device0SenseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.device0SenseComboBox.Location = New System.Drawing.Point(6, 141)
        Me.device0SenseComboBox.Name = "device0SenseComboBox"
        Me.device0SenseComboBox.Size = New System.Drawing.Size(100, 21)
        Me.device0SenseComboBox.TabIndex = 9
        '
        'device0GroupBox
        '
        Me.device0GroupBox.Controls.Add(Me.device0ChannelNameTextBox)
        Me.device0GroupBox.Controls.Add(Me.device0VoltageLevelStopLabel)
        Me.device0GroupBox.Controls.Add(Me.device0SenseLabel)
        Me.device0GroupBox.Controls.Add(Me.device0VoltageLevelStopNumeric)
        Me.device0GroupBox.Controls.Add(Me.device0ResourceNameComboBox)
        Me.device0GroupBox.Controls.Add(Me.device0VoltageLevelStartLabel)
        Me.device0GroupBox.Controls.Add(Me.device0VoltageLevelStartNumeric)
        Me.device0GroupBox.Controls.Add(Me.device0SourceDelayLabel)
        Me.device0GroupBox.Controls.Add(Me.device0NumberOfPlotsLabel)
        Me.device0GroupBox.Controls.Add(Me.device0ChannelNameLabel)
        Me.device0GroupBox.Controls.Add(Me.device0NumberOfPlotsNumeric)
        Me.device0GroupBox.Controls.Add(Me.device0ResourceNameLabel)
        Me.device0GroupBox.Controls.Add(Me.device0SourceDelayNumeric)
        Me.device0GroupBox.Controls.Add(Me.device0CurrentLimitLabel)
        Me.device0GroupBox.Controls.Add(Me.device0CurrentLimitNumeric)
        Me.device0GroupBox.Controls.Add(Me.device0SenseComboBox)
        Me.device0GroupBox.Location = New System.Drawing.Point(12, 12)
        Me.device0GroupBox.Name = "device0GroupBox"
        Me.device0GroupBox.Size = New System.Drawing.Size(266, 220)
        Me.device0GroupBox.TabIndex = 0
        Me.device0GroupBox.TabStop = False
        Me.device0GroupBox.Text = "Device 0: Gate Device"
        '
        'device0ChannelNameTextBox
        '
        Me.device0ChannelNameTextBox.Location = New System.Drawing.Point(147, 39)
        Me.device0ChannelNameTextBox.Name = "device0ChannelNameTextBox"
        Me.device0ChannelNameTextBox.Size = New System.Drawing.Size(100, 20)
        Me.device0ChannelNameTextBox.TabIndex = 3
        Me.device0ChannelNameTextBox.Text = "0"
        '
        'device0ResourceNameComboBox
        '
        Me.device0ResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.device0ResourceNameComboBox.FormattingEnabled = True
        Me.device0ResourceNameComboBox.Location = New System.Drawing.Point(9, 39)
        Me.device0ResourceNameComboBox.Name = "device0ResourceNameComboBox"
        Me.device0ResourceNameComboBox.Size = New System.Drawing.Size(97, 21)
        Me.device0ResourceNameComboBox.TabIndex = 1
        '
        'device0ChannelNameLabel
        '
        Me.device0ChannelNameLabel.AutoSize = True
        Me.device0ChannelNameLabel.Location = New System.Drawing.Point(144, 23)
        Me.device0ChannelNameLabel.Name = "device0ChannelNameLabel"
        Me.device0ChannelNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.device0ChannelNameLabel.TabIndex = 2
        Me.device0ChannelNameLabel.Text = "Channel Name"
        '
        'device0ResourceNameLabel
        '
        Me.device0ResourceNameLabel.AutoSize = True
        Me.device0ResourceNameLabel.Location = New System.Drawing.Point(6, 23)
        Me.device0ResourceNameLabel.Name = "device0ResourceNameLabel"
        Me.device0ResourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.device0ResourceNameLabel.TabIndex = 0
        Me.device0ResourceNameLabel.Text = "Resource Name"
        '
        'device1GroupBox
        '
        Me.device1GroupBox.Controls.Add(Me.device1ChannelNameTextBox)
        Me.device1GroupBox.Controls.Add(Me.device1VoltageLevelStopLabel)
        Me.device1GroupBox.Controls.Add(Me.device1SenseLabel)
        Me.device1GroupBox.Controls.Add(Me.device1VoltageLevelStopNumeric)
        Me.device1GroupBox.Controls.Add(Me.device1ResourceNameComboBox)
        Me.device1GroupBox.Controls.Add(Me.device1VoltageLevelStartLabel)
        Me.device1GroupBox.Controls.Add(Me.device1VoltageLevelStartNumeric)
        Me.device1GroupBox.Controls.Add(Me.device1SourceDelayLabel)
        Me.device1GroupBox.Controls.Add(Me.device1NumberOfPointsLabel)
        Me.device1GroupBox.Controls.Add(Me.device1ChannelNameLabel)
        Me.device1GroupBox.Controls.Add(Me.device1NumberOfPointsNumeric)
        Me.device1GroupBox.Controls.Add(Me.device1ResourceNameLabel)
        Me.device1GroupBox.Controls.Add(Me.device1SourceDelayNumeric)
        Me.device1GroupBox.Controls.Add(Me.device1CurrentLimitLabel)
        Me.device1GroupBox.Controls.Add(Me.device1CurrentLimitNumeric)
        Me.device1GroupBox.Controls.Add(Me.device1SenseComboBox)
        Me.device1GroupBox.Location = New System.Drawing.Point(12, 248)
        Me.device1GroupBox.Name = "device1GroupBox"
        Me.device1GroupBox.Size = New System.Drawing.Size(266, 219)
        Me.device1GroupBox.TabIndex = 1
        Me.device1GroupBox.TabStop = False
        Me.device1GroupBox.Text = "Device 1: Drain Device"
        '
        'device1ChannelNameTextBox
        '
        Me.device1ChannelNameTextBox.Location = New System.Drawing.Point(147, 39)
        Me.device1ChannelNameTextBox.Name = "device1ChannelNameTextBox"
        Me.device1ChannelNameTextBox.Size = New System.Drawing.Size(100, 20)
        Me.device1ChannelNameTextBox.TabIndex = 3
        Me.device1ChannelNameTextBox.Text = "0"
        '
        'device1VoltageLevelStopLabel
        '
        Me.device1VoltageLevelStopLabel.AutoSize = True
        Me.device1VoltageLevelStopLabel.Location = New System.Drawing.Point(147, 174)
        Me.device1VoltageLevelStopLabel.Name = "device1VoltageLevelStopLabel"
        Me.device1VoltageLevelStopLabel.Size = New System.Drawing.Size(113, 13)
        Me.device1VoltageLevelStopLabel.TabIndex = 14
        Me.device1VoltageLevelStopLabel.Text = "Voltage Level Stop (V)"
        '
        'device1SenseLabel
        '
        Me.device1SenseLabel.AutoSize = True
        Me.device1SenseLabel.Location = New System.Drawing.Point(6, 124)
        Me.device1SenseLabel.Name = "device1SenseLabel"
        Me.device1SenseLabel.Size = New System.Drawing.Size(37, 13)
        Me.device1SenseLabel.TabIndex = 8
        Me.device1SenseLabel.Text = "Sense"
        '
        'device1VoltageLevelStopNumeric
        '
        Me.device1VoltageLevelStopNumeric.DecimalPlaces = 6
        Me.device1VoltageLevelStopNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.device1VoltageLevelStopNumeric.Location = New System.Drawing.Point(147, 190)
        Me.device1VoltageLevelStopNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.device1VoltageLevelStopNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.device1VoltageLevelStopNumeric.Name = "device1VoltageLevelStopNumeric"
        Me.device1VoltageLevelStopNumeric.Size = New System.Drawing.Size(100, 20)
        Me.device1VoltageLevelStopNumeric.TabIndex = 15
        Me.device1VoltageLevelStopNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'device1ResourceNameComboBox
        '
        Me.device1ResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.device1ResourceNameComboBox.FormattingEnabled = True
        Me.device1ResourceNameComboBox.Location = New System.Drawing.Point(6, 39)
        Me.device1ResourceNameComboBox.Name = "device1ResourceNameComboBox"
        Me.device1ResourceNameComboBox.Size = New System.Drawing.Size(100, 21)
        Me.device1ResourceNameComboBox.TabIndex = 1
        '
        'device1VoltageLevelStartLabel
        '
        Me.device1VoltageLevelStartLabel.AutoSize = True
        Me.device1VoltageLevelStartLabel.Location = New System.Drawing.Point(147, 124)
        Me.device1VoltageLevelStartLabel.Name = "device1VoltageLevelStartLabel"
        Me.device1VoltageLevelStartLabel.Size = New System.Drawing.Size(113, 13)
        Me.device1VoltageLevelStartLabel.TabIndex = 10
        Me.device1VoltageLevelStartLabel.Text = "Voltage Level Start (V)"
        '
        'device1VoltageLevelStartNumeric
        '
        Me.device1VoltageLevelStartNumeric.DecimalPlaces = 6
        Me.device1VoltageLevelStartNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.device1VoltageLevelStartNumeric.Location = New System.Drawing.Point(147, 140)
        Me.device1VoltageLevelStartNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.device1VoltageLevelStartNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.device1VoltageLevelStartNumeric.Name = "device1VoltageLevelStartNumeric"
        Me.device1VoltageLevelStartNumeric.Size = New System.Drawing.Size(100, 20)
        Me.device1VoltageLevelStartNumeric.TabIndex = 11
        '
        'device1SourceDelayLabel
        '
        Me.device1SourceDelayLabel.AutoSize = True
        Me.device1SourceDelayLabel.Location = New System.Drawing.Point(6, 174)
        Me.device1SourceDelayLabel.Name = "device1SourceDelayLabel"
        Me.device1SourceDelayLabel.Size = New System.Drawing.Size(85, 13)
        Me.device1SourceDelayLabel.TabIndex = 12
        Me.device1SourceDelayLabel.Text = "Source Delay (s)"
        '
        'device1NumberOfPointsLabel
        '
        Me.device1NumberOfPointsLabel.AutoSize = True
        Me.device1NumberOfPointsLabel.Location = New System.Drawing.Point(147, 75)
        Me.device1NumberOfPointsLabel.Name = "device1NumberOfPointsLabel"
        Me.device1NumberOfPointsLabel.Size = New System.Drawing.Size(36, 13)
        Me.device1NumberOfPointsLabel.TabIndex = 6
        Me.device1NumberOfPointsLabel.Text = "Points"
        '
        'device1ChannelNameLabel
        '
        Me.device1ChannelNameLabel.AutoSize = True
        Me.device1ChannelNameLabel.Location = New System.Drawing.Point(147, 23)
        Me.device1ChannelNameLabel.Name = "device1ChannelNameLabel"
        Me.device1ChannelNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.device1ChannelNameLabel.TabIndex = 2
        Me.device1ChannelNameLabel.Text = "Channel Name"
        '
        'device1NumberOfPointsNumeric
        '
        Me.device1NumberOfPointsNumeric.Location = New System.Drawing.Point(147, 91)
        Me.device1NumberOfPointsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.device1NumberOfPointsNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.device1NumberOfPointsNumeric.Name = "device1NumberOfPointsNumeric"
        Me.device1NumberOfPointsNumeric.Size = New System.Drawing.Size(100, 20)
        Me.device1NumberOfPointsNumeric.TabIndex = 7
        Me.device1NumberOfPointsNumeric.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'device1ResourceNameLabel
        '
        Me.device1ResourceNameLabel.AutoSize = True
        Me.device1ResourceNameLabel.Location = New System.Drawing.Point(6, 23)
        Me.device1ResourceNameLabel.Name = "device1ResourceNameLabel"
        Me.device1ResourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.device1ResourceNameLabel.TabIndex = 0
        Me.device1ResourceNameLabel.Text = "Resource Name"
        '
        'device1SourceDelayNumeric
        '
        Me.device1SourceDelayNumeric.DecimalPlaces = 6
        Me.device1SourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.device1SourceDelayNumeric.Location = New System.Drawing.Point(6, 190)
        Me.device1SourceDelayNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.device1SourceDelayNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.device1SourceDelayNumeric.Name = "device1SourceDelayNumeric"
        Me.device1SourceDelayNumeric.Size = New System.Drawing.Size(100, 20)
        Me.device1SourceDelayNumeric.TabIndex = 13
        Me.device1SourceDelayNumeric.Value = New Decimal(New Integer() {5, 0, 0, 196608})
        '
        'device1CurrentLimitLabel
        '
        Me.device1CurrentLimitLabel.AutoSize = True
        Me.device1CurrentLimitLabel.Location = New System.Drawing.Point(6, 75)
        Me.device1CurrentLimitLabel.Name = "device1CurrentLimitLabel"
        Me.device1CurrentLimitLabel.Size = New System.Drawing.Size(81, 13)
        Me.device1CurrentLimitLabel.TabIndex = 4
        Me.device1CurrentLimitLabel.Text = "Current Limit (A)"
        '
        'device1CurrentLimitNumeric
        '
        Me.device1CurrentLimitNumeric.DecimalPlaces = 6
        Me.device1CurrentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.device1CurrentLimitNumeric.Location = New System.Drawing.Point(6, 91)
        Me.device1CurrentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.device1CurrentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.device1CurrentLimitNumeric.Name = "device1CurrentLimitNumeric"
        Me.device1CurrentLimitNumeric.Size = New System.Drawing.Size(100, 20)
        Me.device1CurrentLimitNumeric.TabIndex = 5
        Me.device1CurrentLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'device1SenseComboBox
        '
        Me.device1SenseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.device1SenseComboBox.Location = New System.Drawing.Point(6, 140)
        Me.device1SenseComboBox.Name = "device1SenseComboBox"
        Me.device1SenseComboBox.Size = New System.Drawing.Size(100, 21)
        Me.device1SenseComboBox.TabIndex = 9
        '
        'timeoutNumeric
        '
        Me.timeoutNumeric.DecimalPlaces = 6
        Me.timeoutNumeric.Location = New System.Drawing.Point(6, 19)
        Me.timeoutNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.timeoutNumeric.Name = "timeoutNumeric"
        Me.timeoutNumeric.Size = New System.Drawing.Size(100, 20)
        Me.timeoutNumeric.TabIndex = 0
        Me.timeoutNumeric.Value = New Decimal(New Integer() {15, 0, 0, 0})
        '
        'timeoutGroupBox
        '
        Me.timeoutGroupBox.Controls.Add(Me.timeoutNumeric)
        Me.timeoutGroupBox.Location = New System.Drawing.Point(294, 12)
        Me.timeoutGroupBox.Name = "timeoutGroupBox"
        Me.timeoutGroupBox.Size = New System.Drawing.Size(265, 50)
        Me.timeoutGroupBox.TabIndex = 2
        Me.timeoutGroupBox.TabStop = False
        Me.timeoutGroupBox.Text = "Timeout"
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(571, 524)
        Me.Controls.Add(Me.timeoutGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.device1GroupBox)
        Me.Controls.Add(Me.measurementsGroupBox)
        Me.Controls.Add(Me.device0GroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Hardware-Timed Two-Channel Voltage Sweep (IV Curve)"
        Me.measurementsGroupBox.ResumeLayout(False)
        Me.measurementsGroupBox.PerformLayout()
        CType(Me.measurementsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.device0VoltageLevelStopNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.device0SourceDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.device0VoltageLevelStartNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.device0NumberOfPlotsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.device0CurrentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.device0GroupBox.ResumeLayout(False)
        Me.device0GroupBox.PerformLayout()
        Me.device1GroupBox.ResumeLayout(False)
        Me.device1GroupBox.PerformLayout()
        CType(Me.device1VoltageLevelStopNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.device1VoltageLevelStartNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.device1NumberOfPointsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.device1SourceDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.device1CurrentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.timeoutNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.timeoutGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private WithEvents startButton As System.Windows.Forms.Button
    Private measurementsGroupBox As System.Windows.Forms.GroupBox
    Private measurementsDataGridView As System.Windows.Forms.DataGridView
    Private device0SenseLabel As System.Windows.Forms.Label
    Private device0VoltageLevelStopLabel As System.Windows.Forms.Label
    Private device0VoltageLevelStartLabel As System.Windows.Forms.Label
    Private device0SourceDelayLabel As System.Windows.Forms.Label
    Private device0NumberOfPlotsLabel As System.Windows.Forms.Label
    Private device0VoltageLevelStopNumeric As System.Windows.Forms.NumericUpDown
    Private device0CurrentLimitLabel As System.Windows.Forms.Label
    Private device0SourceDelayNumeric As System.Windows.Forms.NumericUpDown
    Private device0VoltageLevelStartNumeric As System.Windows.Forms.NumericUpDown
    Private device0NumberOfPlotsNumeric As System.Windows.Forms.NumericUpDown
    Private device0CurrentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private device0SenseComboBox As System.Windows.Forms.ComboBox
    Private device0GroupBox As System.Windows.Forms.GroupBox
    Private device0ResourceNameComboBox As System.Windows.Forms.ComboBox
    Private device0ChannelNameLabel As System.Windows.Forms.Label
    Private device0ResourceNameLabel As System.Windows.Forms.Label
    Private device1GroupBox As System.Windows.Forms.GroupBox
    Private device1VoltageLevelStopLabel As System.Windows.Forms.Label
    Private device1SenseLabel As System.Windows.Forms.Label
    Private device1VoltageLevelStopNumeric As System.Windows.Forms.NumericUpDown
    Private device1ResourceNameComboBox As System.Windows.Forms.ComboBox
    Private device1VoltageLevelStartLabel As System.Windows.Forms.Label
    Private WithEvents device1VoltageLevelStartNumeric As System.Windows.Forms.NumericUpDown
    Private device1SourceDelayLabel As System.Windows.Forms.Label
    Private device1NumberOfPointsLabel As System.Windows.Forms.Label
    Private device1ChannelNameLabel As System.Windows.Forms.Label
    Private device1NumberOfPointsNumeric As System.Windows.Forms.NumericUpDown
    Private device1ResourceNameLabel As System.Windows.Forms.Label
    Private device1SourceDelayNumeric As System.Windows.Forms.NumericUpDown
    Private device1CurrentLimitLabel As System.Windows.Forms.Label
    Private device1CurrentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private device1SenseComboBox As System.Windows.Forms.ComboBox
    Private timeoutNumeric As System.Windows.Forms.NumericUpDown
    Private timeoutGroupBox As System.Windows.Forms.GroupBox
    Private device0ChannelNameTextBox As System.Windows.Forms.TextBox
    Private device1ChannelNameTextBox As System.Windows.Forms.TextBox
    Private selectPlotLabel As System.Windows.Forms.Label
    Private WithEvents plotNameComboBox As System.Windows.Forms.ComboBox
    Private readingNoColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private measuredVoltageColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private measuredCurrentColumn As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
