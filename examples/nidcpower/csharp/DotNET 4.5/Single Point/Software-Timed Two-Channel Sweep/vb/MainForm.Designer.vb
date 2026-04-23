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
        Me.selectedPlotNameComboBox = New System.Windows.Forms.ComboBox()
        Me.measurementsDataGridView = New System.Windows.Forms.DataGridView()
        Me.readingNoColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.voltageMeasurementColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.currentMeasurementColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.channel1SenseLabel = New System.Windows.Forms.Label()
        Me.channel1VoltageLevelStopLabel = New System.Windows.Forms.Label()
        Me.channel1VoltageLevelStartLabel = New System.Windows.Forms.Label()
        Me.channel1DelayLabel = New System.Windows.Forms.Label()
        Me.channel1PlotsLabel = New System.Windows.Forms.Label()
        Me.channel1VoltageLevelStopNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channel1CurrentLimitLabel = New System.Windows.Forms.Label()
        Me.channel1SourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channel1VoltageLevelStartNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channel1NumberOfPlotsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channel1CurrentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channel1SenseComboBox = New System.Windows.Forms.ComboBox()
        Me.channel1GroupBox = New System.Windows.Forms.GroupBox()
        Me.channel1NameTextBox = New System.Windows.Forms.TextBox()
        Me.channel1NameLabel = New System.Windows.Forms.Label()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.channel2GroupBox = New System.Windows.Forms.GroupBox()
        Me.channel2NameTextBox = New System.Windows.Forms.TextBox()
        Me.channel2VoltageLevelStopLabel = New System.Windows.Forms.Label()
        Me.channel2SenseLabel = New System.Windows.Forms.Label()
        Me.channel2VoltageLevelStopNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channel2VoltageLevelStartLabel = New System.Windows.Forms.Label()
        Me.channel2VoltageLevelStartNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channel2DelayLabel = New System.Windows.Forms.Label()
        Me.channel2PointsLabel = New System.Windows.Forms.Label()
        Me.channel2NameLabel = New System.Windows.Forms.Label()
        Me.channel2NumberOfPointsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channel2SourceDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channel2CurrentLimitLabel = New System.Windows.Forms.Label()
        Me.channel2CurrentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channel2SenseComboBox = New System.Windows.Forms.ComboBox()
        Me.message1RichTextBox = New System.Windows.Forms.RichTextBox()
        Me.message2RichTextBox = New System.Windows.Forms.RichTextBox()
        Me.message3RichTextBox = New System.Windows.Forms.RichTextBox()
        Me.message4RichTextBox = New System.Windows.Forms.RichTextBox()
        Me.resourceNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementsGroupBox.SuspendLayout()
        CType(Me.measurementsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.channel1VoltageLevelStopNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.channel1SourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.channel1VoltageLevelStartNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.channel1NumberOfPlotsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.channel1CurrentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.channel1GroupBox.SuspendLayout()
        Me.channel2GroupBox.SuspendLayout()
        CType(Me.channel2VoltageLevelStopNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.channel2VoltageLevelStartNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.channel2NumberOfPointsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.channel2SourceDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.channel2CurrentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resourceNameGroupBox.SuspendLayout()
        Me.messageGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(114, 553)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 3
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'measurementsGroupBox
        '
        Me.measurementsGroupBox.Controls.Add(Me.selectPlotLabel)
        Me.measurementsGroupBox.Controls.Add(Me.selectedPlotNameComboBox)
        Me.measurementsGroupBox.Controls.Add(Me.measurementsDataGridView)
        Me.measurementsGroupBox.Location = New System.Drawing.Point(291, 12)
        Me.measurementsGroupBox.Name = "measurementsGroupBox"
        Me.measurementsGroupBox.Size = New System.Drawing.Size(265, 362)
        Me.measurementsGroupBox.TabIndex = 4
        Me.measurementsGroupBox.TabStop = False
        Me.measurementsGroupBox.Text = "Measurements"
        '
        'selectPlotLabel
        '
        Me.selectPlotLabel.AutoSize = True
        Me.selectPlotLabel.Location = New System.Drawing.Point(6, 23)
        Me.selectPlotLabel.Name = "selectPlotLabel"
        Me.selectPlotLabel.Size = New System.Drawing.Size(58, 13)
        Me.selectPlotLabel.TabIndex = 0
        Me.selectPlotLabel.Text = "Select Plot"
        '
        'selectedPlotNameComboBox
        '
        Me.selectedPlotNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.selectedPlotNameComboBox.FormattingEnabled = True
        Me.selectedPlotNameComboBox.Location = New System.Drawing.Point(68, 19)
        Me.selectedPlotNameComboBox.Name = "selectedPlotNameComboBox"
        Me.selectedPlotNameComboBox.Size = New System.Drawing.Size(191, 21)
        Me.selectedPlotNameComboBox.TabIndex = 1
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
        Me.measurementsDataGridView.Location = New System.Drawing.Point(6, 56)
        Me.measurementsDataGridView.Name = "measurementsDataGridView"
        Me.measurementsDataGridView.ReadOnly = True
        Me.measurementsDataGridView.RowHeadersVisible = False
        Me.measurementsDataGridView.RowHeadersWidth = 15
        Me.measurementsDataGridView.RowTemplate.Height = 24
        Me.measurementsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.measurementsDataGridView.Size = New System.Drawing.Size(253, 299)
        Me.measurementsDataGridView.StandardTab = True
        Me.measurementsDataGridView.TabIndex = 2
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
        'channel1SenseLabel
        '
        Me.channel1SenseLabel.AutoSize = True
        Me.channel1SenseLabel.Location = New System.Drawing.Point(6, 122)
        Me.channel1SenseLabel.Name = "channel1SenseLabel"
        Me.channel1SenseLabel.Size = New System.Drawing.Size(37, 13)
        Me.channel1SenseLabel.TabIndex = 6
        Me.channel1SenseLabel.Text = "Sense"
        '
        'channel1VoltageLevelStopLabel
        '
        Me.channel1VoltageLevelStopLabel.AutoSize = True
        Me.channel1VoltageLevelStopLabel.Location = New System.Drawing.Point(147, 172)
        Me.channel1VoltageLevelStopLabel.Name = "channel1VoltageLevelStopLabel"
        Me.channel1VoltageLevelStopLabel.Size = New System.Drawing.Size(113, 13)
        Me.channel1VoltageLevelStopLabel.TabIndex = 12
        Me.channel1VoltageLevelStopLabel.Text = "Voltage Level Stop (V)"
        '
        'channel1VoltageLevelStartLabel
        '
        Me.channel1VoltageLevelStartLabel.AutoSize = True
        Me.channel1VoltageLevelStartLabel.Location = New System.Drawing.Point(147, 122)
        Me.channel1VoltageLevelStartLabel.Name = "channel1VoltageLevelStartLabel"
        Me.channel1VoltageLevelStartLabel.Size = New System.Drawing.Size(113, 13)
        Me.channel1VoltageLevelStartLabel.TabIndex = 8
        Me.channel1VoltageLevelStartLabel.Text = "Voltage Level Start (V)"
        '
        'channel1DelayLabel
        '
        Me.channel1DelayLabel.AutoSize = True
        Me.channel1DelayLabel.Location = New System.Drawing.Point(6, 172)
        Me.channel1DelayLabel.Name = "channel1DelayLabel"
        Me.channel1DelayLabel.Size = New System.Drawing.Size(85, 13)
        Me.channel1DelayLabel.TabIndex = 10
        Me.channel1DelayLabel.Text = "Source Delay (s)"
        '
        'channel1PlotsLabel
        '
        Me.channel1PlotsLabel.AutoSize = True
        Me.channel1PlotsLabel.Location = New System.Drawing.Point(147, 73)
        Me.channel1PlotsLabel.Name = "channel1PlotsLabel"
        Me.channel1PlotsLabel.Size = New System.Drawing.Size(30, 13)
        Me.channel1PlotsLabel.TabIndex = 4
        Me.channel1PlotsLabel.Text = "Plots"
        '
        'channel1VoltageLevelStopNumeric
        '
        Me.channel1VoltageLevelStopNumeric.DecimalPlaces = 6
        Me.channel1VoltageLevelStopNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.channel1VoltageLevelStopNumeric.Location = New System.Drawing.Point(147, 188)
        Me.channel1VoltageLevelStopNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.channel1VoltageLevelStopNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.channel1VoltageLevelStopNumeric.Name = "channel1VoltageLevelStopNumeric"
        Me.channel1VoltageLevelStopNumeric.Size = New System.Drawing.Size(100, 20)
        Me.channel1VoltageLevelStopNumeric.TabIndex = 13
        Me.channel1VoltageLevelStopNumeric.Value = New Decimal(New Integer() {39, 0, 0, 65536})
        '
        'channel1CurrentLimitLabel
        '
        Me.channel1CurrentLimitLabel.AutoSize = True
        Me.channel1CurrentLimitLabel.Location = New System.Drawing.Point(6, 73)
        Me.channel1CurrentLimitLabel.Name = "channel1CurrentLimitLabel"
        Me.channel1CurrentLimitLabel.Size = New System.Drawing.Size(81, 13)
        Me.channel1CurrentLimitLabel.TabIndex = 2
        Me.channel1CurrentLimitLabel.Text = "Current Limit (A)"
        '
        'channel1SourceDelayNumeric
        '
        Me.channel1SourceDelayNumeric.DecimalPlaces = 6
        Me.channel1SourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.channel1SourceDelayNumeric.Location = New System.Drawing.Point(6, 188)
        Me.channel1SourceDelayNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.channel1SourceDelayNumeric.Name = "channel1SourceDelayNumeric"
        Me.channel1SourceDelayNumeric.Size = New System.Drawing.Size(100, 20)
        Me.channel1SourceDelayNumeric.TabIndex = 11
        Me.channel1SourceDelayNumeric.Value = New Decimal(New Integer() {2, 0, 0, 196608})
        '
        'channel1VoltageLevelStartNumeric
        '
        Me.channel1VoltageLevelStartNumeric.DecimalPlaces = 6
        Me.channel1VoltageLevelStartNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.channel1VoltageLevelStartNumeric.Location = New System.Drawing.Point(147, 138)
        Me.channel1VoltageLevelStartNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.channel1VoltageLevelStartNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.channel1VoltageLevelStartNumeric.Name = "channel1VoltageLevelStartNumeric"
        Me.channel1VoltageLevelStartNumeric.Size = New System.Drawing.Size(100, 20)
        Me.channel1VoltageLevelStartNumeric.TabIndex = 9
        Me.channel1VoltageLevelStartNumeric.Value = New Decimal(New Integer() {35, 0, 0, 65536})
        '
        'channel1NumberOfPlotsNumeric
        '
        Me.channel1NumberOfPlotsNumeric.Location = New System.Drawing.Point(147, 89)
        Me.channel1NumberOfPlotsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.channel1NumberOfPlotsNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.channel1NumberOfPlotsNumeric.Name = "channel1NumberOfPlotsNumeric"
        Me.channel1NumberOfPlotsNumeric.Size = New System.Drawing.Size(100, 20)
        Me.channel1NumberOfPlotsNumeric.TabIndex = 5
        Me.channel1NumberOfPlotsNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'channel1CurrentLimitNumeric
        '
        Me.channel1CurrentLimitNumeric.DecimalPlaces = 6
        Me.channel1CurrentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.channel1CurrentLimitNumeric.Location = New System.Drawing.Point(6, 89)
        Me.channel1CurrentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.channel1CurrentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.channel1CurrentLimitNumeric.Name = "channel1CurrentLimitNumeric"
        Me.channel1CurrentLimitNumeric.Size = New System.Drawing.Size(100, 20)
        Me.channel1CurrentLimitNumeric.TabIndex = 3
        Me.channel1CurrentLimitNumeric.Value = New Decimal(New Integer() {2, 0, 0, 131072})
        '
        'channel1SenseComboBox
        '
        Me.channel1SenseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.channel1SenseComboBox.Location = New System.Drawing.Point(6, 138)
        Me.channel1SenseComboBox.Name = "channel1SenseComboBox"
        Me.channel1SenseComboBox.Size = New System.Drawing.Size(100, 21)
        Me.channel1SenseComboBox.TabIndex = 7
        '
        'channel1GroupBox
        '
        Me.channel1GroupBox.Controls.Add(Me.channel1NameTextBox)
        Me.channel1GroupBox.Controls.Add(Me.channel1VoltageLevelStopLabel)
        Me.channel1GroupBox.Controls.Add(Me.channel1SenseLabel)
        Me.channel1GroupBox.Controls.Add(Me.channel1VoltageLevelStopNumeric)
        Me.channel1GroupBox.Controls.Add(Me.channel1VoltageLevelStartLabel)
        Me.channel1GroupBox.Controls.Add(Me.channel1VoltageLevelStartNumeric)
        Me.channel1GroupBox.Controls.Add(Me.channel1DelayLabel)
        Me.channel1GroupBox.Controls.Add(Me.channel1PlotsLabel)
        Me.channel1GroupBox.Controls.Add(Me.channel1NameLabel)
        Me.channel1GroupBox.Controls.Add(Me.channel1NumberOfPlotsNumeric)
        Me.channel1GroupBox.Controls.Add(Me.channel1SourceDelayNumeric)
        Me.channel1GroupBox.Controls.Add(Me.channel1CurrentLimitLabel)
        Me.channel1GroupBox.Controls.Add(Me.channel1CurrentLimitNumeric)
        Me.channel1GroupBox.Controls.Add(Me.channel1SenseComboBox)
        Me.channel1GroupBox.Location = New System.Drawing.Point(12, 78)
        Me.channel1GroupBox.Name = "channel1GroupBox"
        Me.channel1GroupBox.Size = New System.Drawing.Size(263, 217)
        Me.channel1GroupBox.TabIndex = 1
        Me.channel1GroupBox.TabStop = False
        '
        'channel1NameTextBox
        '
        Me.channel1NameTextBox.Location = New System.Drawing.Point(6, 37)
        Me.channel1NameTextBox.Name = "channel1NameTextBox"
        Me.channel1NameTextBox.Size = New System.Drawing.Size(100, 20)
        Me.channel1NameTextBox.TabIndex = 1
        Me.channel1NameTextBox.Text = "0"
        '
        'channel1NameLabel
        '
        Me.channel1NameLabel.AutoSize = True
        Me.channel1NameLabel.Location = New System.Drawing.Point(6, 21)
        Me.channel1NameLabel.Name = "channel1NameLabel"
        Me.channel1NameLabel.Size = New System.Drawing.Size(77, 13)
        Me.channel1NameLabel.TabIndex = 0
        Me.channel1NameLabel.Text = "Channel Name"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(6, 19)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(126, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'channel2GroupBox
        '
        Me.channel2GroupBox.Controls.Add(Me.channel2NameTextBox)
        Me.channel2GroupBox.Controls.Add(Me.channel2VoltageLevelStopLabel)
        Me.channel2GroupBox.Controls.Add(Me.channel2SenseLabel)
        Me.channel2GroupBox.Controls.Add(Me.channel2VoltageLevelStopNumeric)
        Me.channel2GroupBox.Controls.Add(Me.channel2VoltageLevelStartLabel)
        Me.channel2GroupBox.Controls.Add(Me.channel2VoltageLevelStartNumeric)
        Me.channel2GroupBox.Controls.Add(Me.channel2DelayLabel)
        Me.channel2GroupBox.Controls.Add(Me.channel2PointsLabel)
        Me.channel2GroupBox.Controls.Add(Me.channel2NameLabel)
        Me.channel2GroupBox.Controls.Add(Me.channel2NumberOfPointsNumeric)
        Me.channel2GroupBox.Controls.Add(Me.channel2SourceDelayNumeric)
        Me.channel2GroupBox.Controls.Add(Me.channel2CurrentLimitLabel)
        Me.channel2GroupBox.Controls.Add(Me.channel2CurrentLimitNumeric)
        Me.channel2GroupBox.Controls.Add(Me.channel2SenseComboBox)
        Me.channel2GroupBox.Location = New System.Drawing.Point(12, 311)
        Me.channel2GroupBox.Name = "channel2GroupBox"
        Me.channel2GroupBox.Size = New System.Drawing.Size(263, 216)
        Me.channel2GroupBox.TabIndex = 2
        Me.channel2GroupBox.TabStop = False
        '
        'channel2NameTextBox
        '
        Me.channel2NameTextBox.Location = New System.Drawing.Point(6, 37)
        Me.channel2NameTextBox.Name = "channel2NameTextBox"
        Me.channel2NameTextBox.Size = New System.Drawing.Size(100, 20)
        Me.channel2NameTextBox.TabIndex = 1
        Me.channel2NameTextBox.Text = "1"
        '
        'channel2VoltageLevelStopLabel
        '
        Me.channel2VoltageLevelStopLabel.AutoSize = True
        Me.channel2VoltageLevelStopLabel.Location = New System.Drawing.Point(147, 172)
        Me.channel2VoltageLevelStopLabel.Name = "channel2VoltageLevelStopLabel"
        Me.channel2VoltageLevelStopLabel.Size = New System.Drawing.Size(113, 13)
        Me.channel2VoltageLevelStopLabel.TabIndex = 12
        Me.channel2VoltageLevelStopLabel.Text = "Voltage Level Stop (V)"
        '
        'channel2SenseLabel
        '
        Me.channel2SenseLabel.AutoSize = True
        Me.channel2SenseLabel.Location = New System.Drawing.Point(6, 122)
        Me.channel2SenseLabel.Name = "channel2SenseLabel"
        Me.channel2SenseLabel.Size = New System.Drawing.Size(37, 13)
        Me.channel2SenseLabel.TabIndex = 6
        Me.channel2SenseLabel.Text = "Sense"
        '
        'channel2VoltageLevelStopNumeric
        '
        Me.channel2VoltageLevelStopNumeric.DecimalPlaces = 6
        Me.channel2VoltageLevelStopNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.channel2VoltageLevelStopNumeric.Location = New System.Drawing.Point(147, 188)
        Me.channel2VoltageLevelStopNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.channel2VoltageLevelStopNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.channel2VoltageLevelStopNumeric.Name = "channel2VoltageLevelStopNumeric"
        Me.channel2VoltageLevelStopNumeric.Size = New System.Drawing.Size(100, 20)
        Me.channel2VoltageLevelStopNumeric.TabIndex = 13
        Me.channel2VoltageLevelStopNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'channel2VoltageLevelStartLabel
        '
        Me.channel2VoltageLevelStartLabel.AutoSize = True
        Me.channel2VoltageLevelStartLabel.Location = New System.Drawing.Point(147, 122)
        Me.channel2VoltageLevelStartLabel.Name = "channel2VoltageLevelStartLabel"
        Me.channel2VoltageLevelStartLabel.Size = New System.Drawing.Size(113, 13)
        Me.channel2VoltageLevelStartLabel.TabIndex = 8
        Me.channel2VoltageLevelStartLabel.Text = "Voltage Level Start (V)"
        '
        'channel2VoltageLevelStartNumeric
        '
        Me.channel2VoltageLevelStartNumeric.DecimalPlaces = 6
        Me.channel2VoltageLevelStartNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.channel2VoltageLevelStartNumeric.Location = New System.Drawing.Point(147, 138)
        Me.channel2VoltageLevelStartNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.channel2VoltageLevelStartNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.channel2VoltageLevelStartNumeric.Name = "channel2VoltageLevelStartNumeric"
        Me.channel2VoltageLevelStartNumeric.Size = New System.Drawing.Size(100, 20)
        Me.channel2VoltageLevelStartNumeric.TabIndex = 9
        '
        'channel2DelayLabel
        '
        Me.channel2DelayLabel.AutoSize = True
        Me.channel2DelayLabel.Location = New System.Drawing.Point(6, 172)
        Me.channel2DelayLabel.Name = "channel2DelayLabel"
        Me.channel2DelayLabel.Size = New System.Drawing.Size(85, 13)
        Me.channel2DelayLabel.TabIndex = 10
        Me.channel2DelayLabel.Text = "Source Delay (s)"
        '
        'channel2PointsLabel
        '
        Me.channel2PointsLabel.AutoSize = True
        Me.channel2PointsLabel.Location = New System.Drawing.Point(147, 73)
        Me.channel2PointsLabel.Name = "channel2PointsLabel"
        Me.channel2PointsLabel.Size = New System.Drawing.Size(36, 13)
        Me.channel2PointsLabel.TabIndex = 4
        Me.channel2PointsLabel.Text = "Points"
        '
        'channel2NameLabel
        '
        Me.channel2NameLabel.AutoSize = True
        Me.channel2NameLabel.Location = New System.Drawing.Point(6, 21)
        Me.channel2NameLabel.Name = "channel2NameLabel"
        Me.channel2NameLabel.Size = New System.Drawing.Size(77, 13)
        Me.channel2NameLabel.TabIndex = 0
        Me.channel2NameLabel.Text = "Channel Name"
        '
        'channel2NumberOfPointsNumeric
        '
        Me.channel2NumberOfPointsNumeric.Location = New System.Drawing.Point(147, 89)
        Me.channel2NumberOfPointsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.channel2NumberOfPointsNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.channel2NumberOfPointsNumeric.Name = "channel2NumberOfPointsNumeric"
        Me.channel2NumberOfPointsNumeric.Size = New System.Drawing.Size(100, 20)
        Me.channel2NumberOfPointsNumeric.TabIndex = 5
        Me.channel2NumberOfPointsNumeric.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'channel2SourceDelayNumeric
        '
        Me.channel2SourceDelayNumeric.DecimalPlaces = 6
        Me.channel2SourceDelayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.channel2SourceDelayNumeric.Location = New System.Drawing.Point(6, 188)
        Me.channel2SourceDelayNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.channel2SourceDelayNumeric.Name = "channel2SourceDelayNumeric"
        Me.channel2SourceDelayNumeric.Size = New System.Drawing.Size(100, 20)
        Me.channel2SourceDelayNumeric.TabIndex = 11
        Me.channel2SourceDelayNumeric.Value = New Decimal(New Integer() {5, 0, 0, 196608})
        '
        'channel2CurrentLimitLabel
        '
        Me.channel2CurrentLimitLabel.AutoSize = True
        Me.channel2CurrentLimitLabel.Location = New System.Drawing.Point(6, 73)
        Me.channel2CurrentLimitLabel.Name = "channel2CurrentLimitLabel"
        Me.channel2CurrentLimitLabel.Size = New System.Drawing.Size(81, 13)
        Me.channel2CurrentLimitLabel.TabIndex = 2
        Me.channel2CurrentLimitLabel.Text = "Current Limit (A)"
        '
        'channel2CurrentLimitNumeric
        '
        Me.channel2CurrentLimitNumeric.DecimalPlaces = 6
        Me.channel2CurrentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.channel2CurrentLimitNumeric.Location = New System.Drawing.Point(6, 89)
        Me.channel2CurrentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.channel2CurrentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.channel2CurrentLimitNumeric.Name = "channel2CurrentLimitNumeric"
        Me.channel2CurrentLimitNumeric.Size = New System.Drawing.Size(100, 20)
        Me.channel2CurrentLimitNumeric.TabIndex = 3
        Me.channel2CurrentLimitNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'channel2SenseComboBox
        '
        Me.channel2SenseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.channel2SenseComboBox.Location = New System.Drawing.Point(6, 138)
        Me.channel2SenseComboBox.Name = "channel2SenseComboBox"
        Me.channel2SenseComboBox.Size = New System.Drawing.Size(100, 21)
        Me.channel2SenseComboBox.TabIndex = 7
        '
        'message1RichTextBox
        '
        Me.message1RichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.message1RichTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.message1RichTextBox.Location = New System.Drawing.Point(6, 19)
        Me.message1RichTextBox.Name = "message1RichTextBox"
        Me.message1RichTextBox.ReadOnly = True
        Me.message1RichTextBox.Size = New System.Drawing.Size(253, 58)
        Me.message1RichTextBox.TabIndex = 0
        Me.message1RichTextBox.TabStop = False
        Me.message1RichTextBox.Text = "By default this example can be used to characterize a FET.  If you wish to charac" & _
            "terize a BJT, the example can be modified to sweep current by making the followi" & _
            "ng changes: "
        '
        'message2RichTextBox
        '
        Me.message2RichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.message2RichTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.message2RichTextBox.Location = New System.Drawing.Point(6, 83)
        Me.message2RichTextBox.Name = "message2RichTextBox"
        Me.message2RichTextBox.ReadOnly = True
        Me.message2RichTextBox.Size = New System.Drawing.Size(253, 29)
        Me.message2RichTextBox.TabIndex = 1
        Me.message2RichTextBox.TabStop = False
        Me.message2RichTextBox.Text = "1.  Change the Output Function to ""DC Current"" instead of ""DC Voltage""."
        '
        'message3RichTextBox
        '
        Me.message3RichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.message3RichTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.message3RichTextBox.Location = New System.Drawing.Point(6, 153)
        Me.message3RichTextBox.Name = "message3RichTextBox"
        Me.message3RichTextBox.ReadOnly = True
        Me.message3RichTextBox.Size = New System.Drawing.Size(253, 43)
        Me.message3RichTextBox.TabIndex = 3
        Me.message3RichTextBox.TabStop = False
        Me.message3RichTextBox.Text = "3.  Use ""Current Level Autorange"" and ""Voltage Limit Autorange"" instead of ""Curre" & _
            "nt Limit Autorange"" and ""Voltage Level Autorange""."
        '
        'message4RichTextBox
        '
        Me.message4RichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.message4RichTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.message4RichTextBox.Location = New System.Drawing.Point(6, 118)
        Me.message4RichTextBox.Name = "message4RichTextBox"
        Me.message4RichTextBox.ReadOnly = True
        Me.message4RichTextBox.Size = New System.Drawing.Size(253, 29)
        Me.message4RichTextBox.TabIndex = 2
        Me.message4RichTextBox.TabStop = False
        Me.message4RichTextBox.Text = "2.  Use ""Current Level"" and ""Voltage Limit"" instead of ""Voltage Level"" and ""Curre" & _
            "nt Limit""."
        '
        'resourceNameGroupBox
        '
        Me.resourceNameGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceNameGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.resourceNameGroupBox.Name = "resourceNameGroupBox"
        Me.resourceNameGroupBox.Size = New System.Drawing.Size(263, 50)
        Me.resourceNameGroupBox.TabIndex = 0
        Me.resourceNameGroupBox.TabStop = False
        Me.resourceNameGroupBox.Text = "Resource Name"
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.message4RichTextBox)
        Me.messageGroupBox.Controls.Add(Me.message3RichTextBox)
        Me.messageGroupBox.Controls.Add(Me.message2RichTextBox)
        Me.messageGroupBox.Controls.Add(Me.message1RichTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(291, 380)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(265, 207)
        Me.messageGroupBox.TabIndex = 5
        Me.messageGroupBox.TabStop = False
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(571, 599)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.resourceNameGroupBox)
        Me.Controls.Add(Me.channel2GroupBox)
        Me.Controls.Add(Me.measurementsGroupBox)
        Me.Controls.Add(Me.channel1GroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Softawre-Timed Two-Channel Voltage Sweep (IV Curve)"
        Me.measurementsGroupBox.ResumeLayout(False)
        Me.measurementsGroupBox.PerformLayout()
        CType(Me.measurementsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.channel1VoltageLevelStopNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.channel1SourceDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.channel1VoltageLevelStartNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.channel1NumberOfPlotsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.channel1CurrentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.channel1GroupBox.ResumeLayout(False)
        Me.channel1GroupBox.PerformLayout()
        Me.channel2GroupBox.ResumeLayout(False)
        Me.channel2GroupBox.PerformLayout()
        CType(Me.channel2VoltageLevelStopNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.channel2VoltageLevelStartNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.channel2NumberOfPointsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.channel2SourceDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.channel2CurrentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.resourceNameGroupBox.ResumeLayout(False)
        Me.messageGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private WithEvents startButton As System.Windows.Forms.Button
    Private measurementsGroupBox As System.Windows.Forms.GroupBox
    Private measurementsDataGridView As System.Windows.Forms.DataGridView
    Private channel1SenseLabel As System.Windows.Forms.Label
    Private channel1VoltageLevelStopLabel As System.Windows.Forms.Label
    Private channel1VoltageLevelStartLabel As System.Windows.Forms.Label
    Private channel1DelayLabel As System.Windows.Forms.Label
    Private channel1PlotsLabel As System.Windows.Forms.Label
    Private channel1VoltageLevelStopNumeric As System.Windows.Forms.NumericUpDown
    Private channel1CurrentLimitLabel As System.Windows.Forms.Label
    Private channel1SourceDelayNumeric As System.Windows.Forms.NumericUpDown
    Private channel1VoltageLevelStartNumeric As System.Windows.Forms.NumericUpDown
    Private channel1NumberOfPlotsNumeric As System.Windows.Forms.NumericUpDown
    Private channel1CurrentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private channel1SenseComboBox As System.Windows.Forms.ComboBox
    Private channel1GroupBox As System.Windows.Forms.GroupBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private channel1NameLabel As System.Windows.Forms.Label
    Private channel2GroupBox As System.Windows.Forms.GroupBox
    Private channel2VoltageLevelStopLabel As System.Windows.Forms.Label
    Private channel2SenseLabel As System.Windows.Forms.Label
    Private channel2VoltageLevelStopNumeric As System.Windows.Forms.NumericUpDown
    Private channel2VoltageLevelStartLabel As System.Windows.Forms.Label
    Private channel2VoltageLevelStartNumeric As System.Windows.Forms.NumericUpDown
    Private channel2DelayLabel As System.Windows.Forms.Label
    Private channel2PointsLabel As System.Windows.Forms.Label
    Private channel2NameLabel As System.Windows.Forms.Label
    Private channel2NumberOfPointsNumeric As System.Windows.Forms.NumericUpDown
    Private channel2SourceDelayNumeric As System.Windows.Forms.NumericUpDown
    Private channel2CurrentLimitLabel As System.Windows.Forms.Label
    Private channel2CurrentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private channel2SenseComboBox As System.Windows.Forms.ComboBox
    Private message1RichTextBox As System.Windows.Forms.RichTextBox
    Private message2RichTextBox As System.Windows.Forms.RichTextBox
    Private message3RichTextBox As System.Windows.Forms.RichTextBox
    Private message4RichTextBox As System.Windows.Forms.RichTextBox
    Private channel1NameTextBox As System.Windows.Forms.TextBox
    Private channel2NameTextBox As System.Windows.Forms.TextBox
    Private readingNoColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private voltageMeasurementColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private currentMeasurementColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private resourceNameGroupBox As System.Windows.Forms.GroupBox
    Private messageGroupBox As System.Windows.Forms.GroupBox
    Private selectPlotLabel As System.Windows.Forms.Label
    Private WithEvents selectedPlotNameComboBox As System.Windows.Forms.ComboBox

End Class
