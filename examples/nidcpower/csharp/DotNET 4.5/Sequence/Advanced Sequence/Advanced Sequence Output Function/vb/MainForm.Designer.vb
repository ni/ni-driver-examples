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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.resourcenameLabel = New System.Windows.Forms.Label()
        Me.channelnameLabel = New System.Windows.Forms.Label()
        Me.stepsLabel = New System.Windows.Forms.Label()
        Me.currentlevelstartLabel = New System.Windows.Forms.Label()
        Me.voltagelevelstartLabel = New System.Windows.Forms.Label()
        Me.currentlevelstopLabel = New System.Windows.Forms.Label()
        Me.voltagelevelstopLabel = New System.Windows.Forms.Label()
        Me.delayLabel = New System.Windows.Forms.Label()
        Me.stepsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.currentLevelStartNumeric = New System.Windows.Forms.NumericUpDown()
        Me.voltageLevelStartNumeric = New System.Windows.Forms.NumericUpDown()
        Me.currentLevelStopNumeric = New System.Windows.Forms.NumericUpDown()
        Me.voltageLevelStopNumeric = New System.Windows.Forms.NumericUpDown()
        Me.delayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameAndChannelNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.measurementsDataGridView = New System.Windows.Forms.DataGridView()
        Me.Point = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Voltage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Current = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.measurementsGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.stepsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.currentLevelStartNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageLevelStartNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.currentLevelStopNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.voltageLevelStopNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.delayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.configurationGroupBox.SuspendLayout()
        Me.resourceNameAndChannelNameGroupBox.SuspendLayout()
        CType(Me.measurementsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementsGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'resourcenameLabel
        '
        Me.resourcenameLabel.AutoSize = True
        Me.resourcenameLabel.Location = New System.Drawing.Point(10, 22)
        Me.resourcenameLabel.Name = "resourcenameLabel"
        Me.resourcenameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourcenameLabel.TabIndex = 0
        Me.resourcenameLabel.Text = "Resource Name"
        '
        'channelnameLabel
        '
        Me.channelnameLabel.AutoSize = True
        Me.channelnameLabel.Location = New System.Drawing.Point(10, 47)
        Me.channelnameLabel.Name = "channelnameLabel"
        Me.channelnameLabel.Size = New System.Drawing.Size(77, 13)
        Me.channelnameLabel.TabIndex = 1
        Me.channelnameLabel.Text = "Channel Name"
        '
        'stepsLabel
        '
        Me.stepsLabel.AutoSize = True
        Me.stepsLabel.Location = New System.Drawing.Point(10, 21)
        Me.stepsLabel.Name = "stepsLabel"
        Me.stepsLabel.Size = New System.Drawing.Size(133, 13)
        Me.stepsLabel.TabIndex = 2
        Me.stepsLabel.Text = "Points per Output Function"
        '
        'currentlevelstartLabel
        '
        Me.currentlevelstartLabel.AutoSize = True
        Me.currentlevelstartLabel.Location = New System.Drawing.Point(10, 47)
        Me.currentlevelstartLabel.Name = "currentlevelstartLabel"
        Me.currentlevelstartLabel.Size = New System.Drawing.Size(108, 13)
        Me.currentlevelstartLabel.TabIndex = 3
        Me.currentlevelstartLabel.Text = "Current Level Start(A)"
        '
        'voltagelevelstartLabel
        '
        Me.voltagelevelstartLabel.AutoSize = True
        Me.voltagelevelstartLabel.Location = New System.Drawing.Point(10, 74)
        Me.voltagelevelstartLabel.Name = "voltagelevelstartLabel"
        Me.voltagelevelstartLabel.Size = New System.Drawing.Size(110, 13)
        Me.voltagelevelstartLabel.TabIndex = 4
        Me.voltagelevelstartLabel.Text = "Voltage Level Start(V)"
        '
        'currentlevelstopLabel
        '
        Me.currentlevelstopLabel.AutoSize = True
        Me.currentlevelstopLabel.Location = New System.Drawing.Point(10, 102)
        Me.currentlevelstopLabel.Name = "currentlevelstopLabel"
        Me.currentlevelstopLabel.Size = New System.Drawing.Size(111, 13)
        Me.currentlevelstopLabel.TabIndex = 5
        Me.currentlevelstopLabel.Text = "Current Level Stop (A)"
        '
        'voltagelevelstopLabel
        '
        Me.voltagelevelstopLabel.AutoSize = True
        Me.voltagelevelstopLabel.Location = New System.Drawing.Point(10, 126)
        Me.voltagelevelstopLabel.Name = "voltagelevelstopLabel"
        Me.voltagelevelstopLabel.Size = New System.Drawing.Size(113, 13)
        Me.voltagelevelstopLabel.TabIndex = 6
        Me.voltagelevelstopLabel.Text = "Voltage Level Stop (V)"
        '
        'delayLabel
        '
        Me.delayLabel.AutoSize = True
        Me.delayLabel.Location = New System.Drawing.Point(10, 152)
        Me.delayLabel.Name = "delayLabel"
        Me.delayLabel.Size = New System.Drawing.Size(85, 13)
        Me.delayLabel.TabIndex = 7
        Me.delayLabel.Text = "Source Delay (s)"
        '
        'stepsNumeric
        '
        Me.stepsNumeric.Location = New System.Drawing.Point(172, 19)
        Me.stepsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.stepsNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.stepsNumeric.Name = "stepsNumeric"
        Me.stepsNumeric.Size = New System.Drawing.Size(90, 20)
        Me.stepsNumeric.TabIndex = 2
        Me.stepsNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'currentLevelStartNumeric
        '
        Me.currentLevelStartNumeric.DecimalPlaces = 5
        Me.currentLevelStartNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.currentLevelStartNumeric.Location = New System.Drawing.Point(172, 45)
        Me.currentLevelStartNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.currentLevelStartNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.currentLevelStartNumeric.Name = "currentLevelStartNumeric"
        Me.currentLevelStartNumeric.Size = New System.Drawing.Size(90, 20)
        Me.currentLevelStartNumeric.TabIndex = 3
        Me.currentLevelStartNumeric.Value = New Decimal(New Integer() {1, 0, 0, 262144})
        '
        'voltageLevelStartNumeric
        '
        Me.voltageLevelStartNumeric.DecimalPlaces = 5
        Me.voltageLevelStartNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.voltageLevelStartNumeric.Location = New System.Drawing.Point(172, 72)
        Me.voltageLevelStartNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLevelStartNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLevelStartNumeric.Name = "voltageLevelStartNumeric"
        Me.voltageLevelStartNumeric.Size = New System.Drawing.Size(90, 20)
        Me.voltageLevelStartNumeric.TabIndex = 4
        Me.voltageLevelStartNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'currentLevelStopNumeric
        '
        Me.currentLevelStopNumeric.DecimalPlaces = 5
        Me.currentLevelStopNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.currentLevelStopNumeric.Location = New System.Drawing.Point(172, 100)
        Me.currentLevelStopNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.currentLevelStopNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.currentLevelStopNumeric.Name = "currentLevelStopNumeric"
        Me.currentLevelStopNumeric.Size = New System.Drawing.Size(90, 20)
        Me.currentLevelStopNumeric.TabIndex = 5
        Me.currentLevelStopNumeric.Value = New Decimal(New Integer() {3, 0, 0, 262144})
        '
        'voltageLevelStopNumeric
        '
        Me.voltageLevelStopNumeric.DecimalPlaces = 5
        Me.voltageLevelStopNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.voltageLevelStopNumeric.Location = New System.Drawing.Point(172, 124)
        Me.voltageLevelStopNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.voltageLevelStopNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.voltageLevelStopNumeric.Name = "voltageLevelStopNumeric"
        Me.voltageLevelStopNumeric.Size = New System.Drawing.Size(90, 20)
        Me.voltageLevelStopNumeric.TabIndex = 6
        Me.voltageLevelStopNumeric.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'delayNumeric
        '
        Me.delayNumeric.DecimalPlaces = 3
        Me.delayNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.delayNumeric.Location = New System.Drawing.Point(172, 150)
        Me.delayNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.delayNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.delayNumeric.Name = "delayNumeric"
        Me.delayNumeric.Size = New System.Drawing.Size(90, 20)
        Me.delayNumeric.TabIndex = 7
        Me.delayNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(131, 44)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(131, 20)
        Me.channelNameTextBox.TabIndex = 1
        Me.channelNameTextBox.Text = "0"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(253, 292)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 8
        Me.startButton.Text = "Start"
        Me.startButton.UseVisualStyleBackColor = True
        AddHandler Me.startButton.Click, New System.EventHandler(AddressOf Me.startButton_Click)
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.stepsNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLevelStartNumeric)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelStartNumeric)
        Me.configurationGroupBox.Controls.Add(Me.currentLevelStopNumeric)
        Me.configurationGroupBox.Controls.Add(Me.voltageLevelStopNumeric)
        Me.configurationGroupBox.Controls.Add(Me.delayNumeric)
        Me.configurationGroupBox.Controls.Add(Me.stepsLabel)
        Me.configurationGroupBox.Controls.Add(Me.currentlevelstartLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltagelevelstartLabel)
        Me.configurationGroupBox.Controls.Add(Me.currentlevelstopLabel)
        Me.configurationGroupBox.Controls.Add(Me.voltagelevelstopLabel)
        Me.configurationGroupBox.Controls.Add(Me.delayLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(15, 96)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(276, 180)
        Me.configurationGroupBox.TabIndex = 11
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'resourceNameAndChannelNameGroupBox
        '
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.resourcenameLabel)
        Me.resourceNameAndChannelNameGroupBox.Controls.Add(Me.channelnameLabel)
        Me.resourceNameAndChannelNameGroupBox.Location = New System.Drawing.Point(15, 15)
        Me.resourceNameAndChannelNameGroupBox.Name = "resourceNameAndChannelNameGroupBox"
        Me.resourceNameAndChannelNameGroupBox.Size = New System.Drawing.Size(276, 75)
        Me.resourceNameAndChannelNameGroupBox.TabIndex = 12
        Me.resourceNameAndChannelNameGroupBox.TabStop = False
        Me.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.resourceNameComboBox.Location = New System.Drawing.Point(131, 19)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(131, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'measurementsDataGridView
        '
        Me.measurementsDataGridView.AllowUserToAddRows = False
        Me.measurementsDataGridView.AllowUserToDeleteRows = False
        Me.measurementsDataGridView.AllowUserToResizeColumns = False
        Me.measurementsDataGridView.AllowUserToResizeRows = False
        Me.measurementsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.measurementsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Point, Me.Voltage, Me.Current})
        Me.measurementsDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.measurementsDataGridView.Name = "measurementsDataGridView"
        Me.measurementsDataGridView.RowHeadersVisible = False
        Me.measurementsDataGridView.Size = New System.Drawing.Size(244, 232)
        Me.measurementsDataGridView.TabIndex = 9
        '
        'Point
        '
        Me.Point.HeaderText = "Point"
        Me.Point.Name = "Point"
        Me.Point.Width = 40
        '
        'Voltage
        '
        Me.Voltage.HeaderText = "Voltage (V)"
        Me.Voltage.Name = "Voltage"
        '
        'Current
        '
        Me.Current.HeaderText = "Current (A)"
        Me.Current.Name = "Current"
        '
        'measurementsGroupBox
        '
        Me.measurementsGroupBox.Controls.Add(Me.measurementsDataGridView)
        Me.measurementsGroupBox.Location = New System.Drawing.Point(297, 15)
        Me.measurementsGroupBox.Name = "measurementsGroupBox"
        Me.measurementsGroupBox.Size = New System.Drawing.Size(257, 261)
        Me.measurementsGroupBox.TabIndex = 14
        Me.measurementsGroupBox.TabStop = False
        Me.measurementsGroupBox.Text = "Measurements"
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(583, 324)
        Me.Controls.Add(Me.measurementsGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceNameAndChannelNameGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Advanced Sequence Output Function"
        CType(Me.stepsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.currentLevelStartNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageLevelStartNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.currentLevelStopNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.voltageLevelStopNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.delayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.resourceNameAndChannelNameGroupBox.ResumeLayout(False)
        Me.resourceNameAndChannelNameGroupBox.PerformLayout()
        CType(Me.measurementsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementsGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	#End Region

	Private resourcenameLabel As System.Windows.Forms.Label
	Private channelnameLabel As System.Windows.Forms.Label
	Private stepsLabel As System.Windows.Forms.Label
	Private currentlevelstartLabel As System.Windows.Forms.Label
	Private voltagelevelstartLabel As System.Windows.Forms.Label
	Private currentlevelstopLabel As System.Windows.Forms.Label
	Private voltagelevelstopLabel As System.Windows.Forms.Label
	Private delayLabel As System.Windows.Forms.Label
	Private stepsNumeric As System.Windows.Forms.NumericUpDown
	Private currentLevelStartNumeric As System.Windows.Forms.NumericUpDown
	Private voltageLevelStartNumeric As System.Windows.Forms.NumericUpDown
	Private currentLevelStopNumeric As System.Windows.Forms.NumericUpDown
	Private voltageLevelStopNumeric As System.Windows.Forms.NumericUpDown
	Private delayNumeric As System.Windows.Forms.NumericUpDown
	Private channelNameTextBox As System.Windows.Forms.TextBox
	Private startButton As System.Windows.Forms.Button
	Private configurationGroupBox As System.Windows.Forms.GroupBox
	Private resourceNameAndChannelNameGroupBox As System.Windows.Forms.GroupBox
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private measurementsDataGridView As System.Windows.Forms.DataGridView
    Private measurementsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents Point As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Voltage As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Current As System.Windows.Forms.DataGridViewTextBoxColumn


End Class
