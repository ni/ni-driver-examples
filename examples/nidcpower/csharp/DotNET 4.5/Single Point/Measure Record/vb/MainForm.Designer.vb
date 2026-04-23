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
        Me.voltageLevelLabel = New System.Windows.Forms.Label()
        Me.measureRecordLengthLabel = New System.Windows.Forms.Label()
        Me.autoZeroLabel = New System.Windows.Forms.Label()
        Me.voltageLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.measureRecordLengthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.autoZeroComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameAndChannelNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.isMeasureRecordFiniteCheckBox = New System.Windows.Forms.CheckBox()
        Me.effectiveMeasurementRateGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementRateTextBox = New System.Windows.Forms.TextBox()
        Me.measurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementsDataGridView = New System.Windows.Forms.DataGridView()
        Me.readingNoColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.voltageMeasurementColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.currentMeasurementColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.voltageLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.measureRecordLengthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resourceNameAndChannelNameGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        Me.effectiveMeasurementRateGroupBox.SuspendLayout()
        Me.measurementsGroupBox.SuspendLayout()
        CType(Me.measurementsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'voltageLevelLabel
        '
        Me.voltageLevelLabel.AutoSize = True
        Me.voltageLevelLabel.Location = New System.Drawing.Point(6, 26)
        Me.voltageLevelLabel.Name = "voltageLevelLabel"
        Me.voltageLevelLabel.Size = New System.Drawing.Size(88, 13)
        Me.voltageLevelLabel.TabIndex = 0
        Me.voltageLevelLabel.Text = "Voltage Level (V)"
        '
        'measureRecordLengthLabel
        '
        Me.measureRecordLengthLabel.AutoSize = True
        Me.measureRecordLengthLabel.Location = New System.Drawing.Point(6, 53)
        Me.measureRecordLengthLabel.Name = "measureRecordLengthLabel"
        Me.measureRecordLengthLabel.Size = New System.Drawing.Size(122, 13)
        Me.measureRecordLengthLabel.TabIndex = 2
        Me.measureRecordLengthLabel.Text = "Measure Record Length"
        '
        'autoZeroLabel
        '
        Me.autoZeroLabel.AutoSize = True
        Me.autoZeroLabel.Location = New System.Drawing.Point(6, 84)
        Me.autoZeroLabel.Name = "autoZeroLabel"
        Me.autoZeroLabel.Size = New System.Drawing.Size(54, 13)
        Me.autoZeroLabel.TabIndex = 8
        Me.autoZeroLabel.Text = "Auto Zero"
        '
        'voltageLevelNumeric
        '
        Me.voltageLevelNumeric.DecimalPlaces = 6
        Me.voltageLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLevelNumeric.Location = New System.Drawing.Point(140, 24)
        Me.voltageLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLevelNumeric.Name = "voltageLevelNumeric"
        Me.voltageLevelNumeric.Size = New System.Drawing.Size(91, 20)
        Me.voltageLevelNumeric.TabIndex = 1
        Me.voltageLevelNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'measureRecordLengthNumeric
        '
        Me.measureRecordLengthNumeric.Location = New System.Drawing.Point(140, 51)
        Me.measureRecordLengthNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.measureRecordLengthNumeric.Name = "measureRecordLengthNumeric"
        Me.measureRecordLengthNumeric.Size = New System.Drawing.Size(91, 20)
        Me.measureRecordLengthNumeric.TabIndex = 3
        Me.measureRecordLengthNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(57, 307)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 2
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.Location = New System.Drawing.Point(138, 307)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 3
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'autoZeroComboBox
        '
        Me.autoZeroComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.autoZeroComboBox.Location = New System.Drawing.Point(140, 81)
        Me.autoZeroComboBox.Name = "autoZeroComboBox"
        Me.autoZeroComboBox.Size = New System.Drawing.Size(91, 21)
        Me.autoZeroComboBox.TabIndex = 9
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
        Me.channelNameTextBox.TabIndex = 3
        Me.channelNameTextBox.Text = "0"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(106, 18)
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
        Me.configurationGroupBox.Controls.Add(Me.autoZeroLabel)
        Me.configurationGroupBox.Controls.Add(Me.isMeasureRecordFiniteCheckBox)
        Me.configurationGroupBox.Controls.Add(Me.autoZeroComboBox)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelNumeric)
        Me.configurationGroupBox.Controls.Add(Me.measureRecordLengthLabel)
        Me.configurationGroupBox.Controls.Add(Me.measureRecordLengthNumeric)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 101)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(240, 156)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'isMeasureRecordFiniteCheckBox
        '
        Me.isMeasureRecordFiniteCheckBox.AutoSize = True
        Me.isMeasureRecordFiniteCheckBox.Checked = True
        Me.isMeasureRecordFiniteCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.isMeasureRecordFiniteCheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.isMeasureRecordFiniteCheckBox.Location = New System.Drawing.Point(45, 117)
        Me.isMeasureRecordFiniteCheckBox.Name = "isMeasureRecordFiniteCheckBox"
        Me.isMeasureRecordFiniteCheckBox.Size = New System.Drawing.Size(150, 17)
        Me.isMeasureRecordFiniteCheckBox.TabIndex = 10
        Me.isMeasureRecordFiniteCheckBox.Text = "Is Measure Record Finite?"
        Me.isMeasureRecordFiniteCheckBox.UseVisualStyleBackColor = True
        '
        'effectiveMeasurementRateGroupBox
        '
        Me.effectiveMeasurementRateGroupBox.Controls.Add(Me.measurementRateTextBox)
        Me.effectiveMeasurementRateGroupBox.Location = New System.Drawing.Point(268, 12)
        Me.effectiveMeasurementRateGroupBox.Name = "effectiveMeasurementRateGroupBox"
        Me.effectiveMeasurementRateGroupBox.Size = New System.Drawing.Size(265, 45)
        Me.effectiveMeasurementRateGroupBox.TabIndex = 4
        Me.effectiveMeasurementRateGroupBox.TabStop = False
        Me.effectiveMeasurementRateGroupBox.Text = "Effective Measurement Rate"
        '
        'measurementRateTextBox
        '
        Me.measurementRateTextBox.Location = New System.Drawing.Point(6, 18)
        Me.measurementRateTextBox.Name = "measurementRateTextBox"
        Me.measurementRateTextBox.ReadOnly = True
        Me.measurementRateTextBox.Size = New System.Drawing.Size(131, 20)
        Me.measurementRateTextBox.TabIndex = 0
        Me.measurementRateTextBox.Text = "0.000000E+000"
        '
        'measurementsGroupBox
        '
        Me.measurementsGroupBox.Controls.Add(Me.measurementsDataGridView)
        Me.measurementsGroupBox.Location = New System.Drawing.Point(268, 73)
        Me.measurementsGroupBox.Name = "measurementsGroupBox"
        Me.measurementsGroupBox.Size = New System.Drawing.Size(265, 277)
        Me.measurementsGroupBox.TabIndex = 5
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
        Me.measurementsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.readingNoColumn, Me.voltageMeasurementColumn, Me.currentMeasurementColumn})
        Me.measurementsDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.measurementsDataGridView.Name = "measurementsDataGridView"
        Me.measurementsDataGridView.ReadOnly = True
        Me.measurementsDataGridView.RowHeadersVisible = False
        Me.measurementsDataGridView.RowHeadersWidth = 15
        Me.measurementsDataGridView.RowTemplate.Height = 24
        Me.measurementsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.measurementsDataGridView.Size = New System.Drawing.Size(253, 249)
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
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(545, 361)
        Me.Controls.Add(Me.measurementsGroupBox)
        Me.Controls.Add(Me.effectiveMeasurementRateGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceNameAndChannelNameGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Measure Record"
        CType(Me.voltageLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.measureRecordLengthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.resourceNameAndChannelNameGroupBox.ResumeLayout(False)
        Me.resourceNameAndChannelNameGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.effectiveMeasurementRateGroupBox.ResumeLayout(False)
        Me.effectiveMeasurementRateGroupBox.PerformLayout()
        Me.measurementsGroupBox.ResumeLayout(False)
        CType(Me.measurementsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private voltageLevelLabel As System.Windows.Forms.Label
    Private measureRecordLengthLabel As System.Windows.Forms.Label
    Private autoZeroLabel As System.Windows.Forms.Label
    Private voltageLevelNumeric As System.Windows.Forms.NumericUpDown
    Private measureRecordLengthNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private autoZeroComboBox As System.Windows.Forms.ComboBox
    Private resourceNameAndChannelNameGroupBox As System.Windows.Forms.GroupBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private channelNameLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private isMeasureRecordFiniteCheckBox As System.Windows.Forms.CheckBox
    Private effectiveMeasurementRateGroupBox As System.Windows.Forms.GroupBox
    Private measurementsGroupBox As System.Windows.Forms.GroupBox
    Private measurementsDataGridView As System.Windows.Forms.DataGridView
    Private measurementRateTextBox As System.Windows.Forms.TextBox
    Private channelNameTextBox As System.Windows.Forms.TextBox
    Private readingNoColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private voltageMeasurementColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Private currentMeasurementColumn As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
