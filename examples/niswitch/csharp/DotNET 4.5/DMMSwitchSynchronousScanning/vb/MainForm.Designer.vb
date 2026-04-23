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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.scanListLabel = New System.Windows.Forms.Label()
        Me.sampleIntervalLabel = New System.Windows.Forms.Label()
        Me.topologyNameLabel = New System.Windows.Forms.Label()
        Me.measCompleteDestComboBox = New System.Windows.Forms.ComboBox()
        Me.topologyNameComboBox = New System.Windows.Forms.ComboBox()
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.scanListTextBox = New System.Windows.Forms.TextBox()
        Me.triggerInputLabel = New System.Windows.Forms.Label()
        Me.switchResourceNameLabel = New System.Windows.Forms.Label()
        Me.switchResourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.switchTriggerInputComboBox = New System.Windows.Forms.ComboBox()
        Me.sampleIntervalNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.groupBox2 = New System.Windows.Forms.GroupBox()
        Me.measurementCompleteDestinationLabel = New System.Windows.Forms.Label()
        Me.samplesToFetchNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.measurementTypeLabel = New System.Windows.Forms.Label()
        Me.samplesLabel = New System.Windows.Forms.Label()
        Me.measurementModeComboBox = New System.Windows.Forms.ComboBox()
        Me.dmmResourceNameLabel = New System.Windows.Forms.Label()
        Me.rangeNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.resolutionNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.dmmResourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.resolutionLabel = New System.Windows.Forms.Label()
        Me.rangeLabel = New System.Windows.Forms.Label()
        Me.EntryNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dataGridViewResults = New System.Windows.Forms.DataGridView()
        Me.Value = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.startButton = New System.Windows.Forms.Button()
        Me.groupBox1.SuspendLayout()
        CType(Me.sampleIntervalNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox2.SuspendLayout()
        CType(Me.samplesToFetchNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rangeNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.resolutionNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'scanListLabel
        '
        Me.scanListLabel.AutoSize = True
        Me.scanListLabel.Location = New System.Drawing.Point(6, 149)
        Me.scanListLabel.Name = "scanListLabel"
        Me.scanListLabel.Size = New System.Drawing.Size(51, 13)
        Me.scanListLabel.TabIndex = 6
        Me.scanListLabel.Text = "Scan List"
        '
        'sampleIntervalLabel
        '
        Me.sampleIntervalLabel.AutoSize = True
        Me.sampleIntervalLabel.Location = New System.Drawing.Point(109, 203)
        Me.sampleIntervalLabel.Name = "sampleIntervalLabel"
        Me.sampleIntervalLabel.Size = New System.Drawing.Size(80, 13)
        Me.sampleIntervalLabel.TabIndex = 13
        Me.sampleIntervalLabel.Text = "Sample Interval"
        '
        'topologyNameLabel
        '
        Me.topologyNameLabel.AutoSize = True
        Me.topologyNameLabel.Location = New System.Drawing.Point(6, 92)
        Me.topologyNameLabel.Name = "topologyNameLabel"
        Me.topologyNameLabel.Size = New System.Drawing.Size(82, 13)
        Me.topologyNameLabel.TabIndex = 5
        Me.topologyNameLabel.Text = "Topology Name"
        '
        'measCompleteDestComboBox
        '
        Me.measCompleteDestComboBox.FormattingEnabled = True
        Me.measCompleteDestComboBox.Location = New System.Drawing.Point(12, 275)
        Me.measCompleteDestComboBox.Name = "measCompleteDestComboBox"
        Me.measCompleteDestComboBox.Size = New System.Drawing.Size(121, 21)
        Me.measCompleteDestComboBox.TabIndex = 6
        '
        'topologyNameComboBox
        '
        Me.topologyNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.topologyNameComboBox.FormattingEnabled = True
        Me.topologyNameComboBox.Location = New System.Drawing.Point(6, 108)
        Me.topologyNameComboBox.Name = "topologyNameComboBox"
        Me.topologyNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.topologyNameComboBox.TabIndex = 1
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.scanListLabel)
        Me.groupBox1.Controls.Add(Me.topologyNameLabel)
        Me.groupBox1.Controls.Add(Me.topologyNameComboBox)
        Me.groupBox1.Controls.Add(Me.scanListTextBox)
        Me.groupBox1.Controls.Add(Me.triggerInputLabel)
        Me.groupBox1.Controls.Add(Me.switchResourceNameLabel)
        Me.groupBox1.Controls.Add(Me.switchResourceNameComboBox)
        Me.groupBox1.Controls.Add(Me.switchTriggerInputComboBox)
        Me.groupBox1.Location = New System.Drawing.Point(8, 11)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(152, 305)
        Me.groupBox1.TabIndex = 0
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Switch"
        '
        'scanListTextBox
        '
        Me.scanListTextBox.Location = New System.Drawing.Point(6, 165)
        Me.scanListTextBox.Name = "scanListTextBox"
        Me.scanListTextBox.Size = New System.Drawing.Size(121, 20)
        Me.scanListTextBox.TabIndex = 2
        Me.scanListTextBox.Text = "ch0:5->com0;"
        '
        'triggerInputLabel
        '
        Me.triggerInputLabel.AutoSize = True
        Me.triggerInputLabel.Location = New System.Drawing.Point(6, 203)
        Me.triggerInputLabel.Name = "triggerInputLabel"
        Me.triggerInputLabel.Size = New System.Drawing.Size(67, 13)
        Me.triggerInputLabel.TabIndex = 7
        Me.triggerInputLabel.Text = "Trigger Input"
        '
        'switchResourceNameLabel
        '
        Me.switchResourceNameLabel.AutoSize = True
        Me.switchResourceNameLabel.Location = New System.Drawing.Point(6, 27)
        Me.switchResourceNameLabel.Name = "switchResourceNameLabel"
        Me.switchResourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.switchResourceNameLabel.TabIndex = 4
        Me.switchResourceNameLabel.Text = "Resource Name"
        '
        'switchResourceNameComboBox
        '
        Me.switchResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.switchResourceNameComboBox.FormattingEnabled = True
        Me.switchResourceNameComboBox.Location = New System.Drawing.Point(6, 43)
        Me.switchResourceNameComboBox.Name = "switchResourceNameComboBox"
        Me.switchResourceNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.switchResourceNameComboBox.TabIndex = 0
        '
        'switchTriggerInputComboBox
        '
        Me.switchTriggerInputComboBox.FormattingEnabled = True
        Me.switchTriggerInputComboBox.Location = New System.Drawing.Point(6, 220)
        Me.switchTriggerInputComboBox.Name = "switchTriggerInputComboBox"
        Me.switchTriggerInputComboBox.Size = New System.Drawing.Size(121, 21)
        Me.switchTriggerInputComboBox.TabIndex = 3
        '
        'sampleIntervalNumericUpDown
        '
        Me.sampleIntervalNumericUpDown.DecimalPlaces = 6
        Me.sampleIntervalNumericUpDown.Location = New System.Drawing.Point(112, 221)
        Me.sampleIntervalNumericUpDown.Name = "sampleIntervalNumericUpDown"
        Me.sampleIntervalNumericUpDown.Size = New System.Drawing.Size(94, 20)
        Me.sampleIntervalNumericUpDown.TabIndex = 5
        Me.sampleIntervalNumericUpDown.Value = New Decimal(New Integer() {1, 0, 0, 131072})
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.sampleIntervalNumericUpDown)
        Me.groupBox2.Controls.Add(Me.sampleIntervalLabel)
        Me.groupBox2.Controls.Add(Me.measCompleteDestComboBox)
        Me.groupBox2.Controls.Add(Me.measurementCompleteDestinationLabel)
        Me.groupBox2.Controls.Add(Me.samplesToFetchNumericUpDown)
        Me.groupBox2.Controls.Add(Me.measurementTypeLabel)
        Me.groupBox2.Controls.Add(Me.samplesLabel)
        Me.groupBox2.Controls.Add(Me.measurementModeComboBox)
        Me.groupBox2.Controls.Add(Me.dmmResourceNameLabel)
        Me.groupBox2.Controls.Add(Me.rangeNumericUpDown)
        Me.groupBox2.Controls.Add(Me.resolutionNumericUpDown)
        Me.groupBox2.Controls.Add(Me.dmmResourceNameComboBox)
        Me.groupBox2.Controls.Add(Me.resolutionLabel)
        Me.groupBox2.Controls.Add(Me.rangeLabel)
        Me.groupBox2.Location = New System.Drawing.Point(166, 11)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(212, 305)
        Me.groupBox2.TabIndex = 1
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Dmm"
        '
        'measurementCompleteDestinationLabel
        '
        Me.measurementCompleteDestinationLabel.AutoSize = True
        Me.measurementCompleteDestinationLabel.Location = New System.Drawing.Point(9, 246)
        Me.measurementCompleteDestinationLabel.Name = "measurementCompleteDestinationLabel"
        Me.measurementCompleteDestinationLabel.Size = New System.Drawing.Size(118, 26)
        Me.measurementCompleteDestinationLabel.TabIndex = 11
        Me.measurementCompleteDestinationLabel.Text = "Measurement Complete" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Destination"
        '
        'samplesToFetchNumericUpDown
        '
        Me.samplesToFetchNumericUpDown.DecimalPlaces = 2
        Me.samplesToFetchNumericUpDown.Location = New System.Drawing.Point(12, 221)
        Me.samplesToFetchNumericUpDown.Name = "samplesToFetchNumericUpDown"
        Me.samplesToFetchNumericUpDown.Size = New System.Drawing.Size(94, 20)
        Me.samplesToFetchNumericUpDown.TabIndex = 4
        Me.samplesToFetchNumericUpDown.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'measurementTypeLabel
        '
        Me.measurementTypeLabel.AutoSize = True
        Me.measurementTypeLabel.Location = New System.Drawing.Point(9, 92)
        Me.measurementTypeLabel.Name = "measurementTypeLabel"
        Me.measurementTypeLabel.Size = New System.Drawing.Size(104, 13)
        Me.measurementTypeLabel.TabIndex = 8
        Me.measurementTypeLabel.Text = "Measurement Mode:"
        '
        'samplesLabel
        '
        Me.samplesLabel.AutoSize = True
        Me.samplesLabel.Location = New System.Drawing.Point(9, 203)
        Me.samplesLabel.Name = "samplesLabel"
        Me.samplesLabel.Size = New System.Drawing.Size(73, 13)
        Me.samplesLabel.TabIndex = 10
        Me.samplesLabel.Text = "Sample Count"
        '
        'measurementModeComboBox
        '
        Me.measurementModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.measurementModeComboBox.FormattingEnabled = True
        Me.measurementModeComboBox.Location = New System.Drawing.Point(12, 108)
        Me.measurementModeComboBox.Name = "measurementModeComboBox"
        Me.measurementModeComboBox.Size = New System.Drawing.Size(121, 21)
        Me.measurementModeComboBox.TabIndex = 1
        '
        'dmmResourceNameLabel
        '
        Me.dmmResourceNameLabel.AutoSize = True
        Me.dmmResourceNameLabel.Location = New System.Drawing.Point(9, 28)
        Me.dmmResourceNameLabel.Name = "dmmResourceNameLabel"
        Me.dmmResourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.dmmResourceNameLabel.TabIndex = 7
        Me.dmmResourceNameLabel.Text = "Resource Name:"
        '
        'rangeNumericUpDown
        '
        Me.rangeNumericUpDown.DecimalPlaces = 2
        Me.rangeNumericUpDown.Location = New System.Drawing.Point(12, 166)
        Me.rangeNumericUpDown.Name = "rangeNumericUpDown"
        Me.rangeNumericUpDown.Size = New System.Drawing.Size(94, 20)
        Me.rangeNumericUpDown.TabIndex = 2
        Me.rangeNumericUpDown.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'resolutionNumericUpDown
        '
        Me.resolutionNumericUpDown.DecimalPlaces = 6
        Me.resolutionNumericUpDown.Location = New System.Drawing.Point(112, 166)
        Me.resolutionNumericUpDown.Name = "resolutionNumericUpDown"
        Me.resolutionNumericUpDown.Size = New System.Drawing.Size(94, 20)
        Me.resolutionNumericUpDown.TabIndex = 3
        Me.resolutionNumericUpDown.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        'dmmResourceNameComboBox
        '
        Me.dmmResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.dmmResourceNameComboBox.FormattingEnabled = True
        Me.dmmResourceNameComboBox.Location = New System.Drawing.Point(12, 44)
        Me.dmmResourceNameComboBox.Name = "dmmResourceNameComboBox"
        Me.dmmResourceNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.dmmResourceNameComboBox.TabIndex = 0
        '
        'resolutionLabel
        '
        Me.resolutionLabel.AutoSize = True
        Me.resolutionLabel.Location = New System.Drawing.Point(109, 150)
        Me.resolutionLabel.Name = "resolutionLabel"
        Me.resolutionLabel.Size = New System.Drawing.Size(57, 13)
        Me.resolutionLabel.TabIndex = 12
        Me.resolutionLabel.Text = "Resolution"
        '
        'rangeLabel
        '
        Me.rangeLabel.AutoSize = True
        Me.rangeLabel.Location = New System.Drawing.Point(9, 149)
        Me.rangeLabel.Name = "rangeLabel"
        Me.rangeLabel.Size = New System.Drawing.Size(39, 13)
        Me.rangeLabel.TabIndex = 9
        Me.rangeLabel.Text = "Range"
        '
        'EntryNumber
        '
        Me.EntryNumber.HeaderText = "Entry Number"
        Me.EntryNumber.Name = "EntryNumber"
        '
        'dataGridViewResults
        '
        Me.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewResults.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.EntryNumber, Me.Value})
        Me.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dataGridViewResults.Location = New System.Drawing.Point(382, 23)
        Me.dataGridViewResults.Name = "dataGridViewResults"
        Me.dataGridViewResults.Size = New System.Drawing.Size(244, 255)
        Me.dataGridViewResults.TabIndex = 3
        '
        'Value
        '
        Me.Value.HeaderText = "Value"
        Me.Value.Name = "Value"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(437, 284)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 2
        Me.startButton.Text = "Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(634, 326)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.groupBox2)
        Me.Controls.Add(Me.dataGridViewResults)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Dmm Switch Synchronous Scanning"
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        CType(Me.sampleIntervalNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox2.PerformLayout()
        CType(Me.samplesToFetchNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rangeNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.resolutionNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents scanListLabel As System.Windows.Forms.Label
    Private WithEvents sampleIntervalLabel As System.Windows.Forms.Label
    Private WithEvents topologyNameLabel As System.Windows.Forms.Label
    Private WithEvents measCompleteDestComboBox As System.Windows.Forms.ComboBox
    Private WithEvents topologyNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents scanListTextBox As System.Windows.Forms.TextBox
    Private WithEvents triggerInputLabel As System.Windows.Forms.Label
    Private WithEvents switchResourceNameLabel As System.Windows.Forms.Label
    Private WithEvents switchResourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents switchTriggerInputComboBox As System.Windows.Forms.ComboBox
    Private WithEvents sampleIntervalNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Private WithEvents measurementCompleteDestinationLabel As System.Windows.Forms.Label
    Private WithEvents samplesToFetchNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents measurementTypeLabel As System.Windows.Forms.Label
    Private WithEvents samplesLabel As System.Windows.Forms.Label
    Private WithEvents measurementModeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents dmmResourceNameLabel As System.Windows.Forms.Label
    Private WithEvents rangeNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents resolutionNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents dmmResourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents resolutionLabel As System.Windows.Forms.Label
    Private WithEvents rangeLabel As System.Windows.Forms.Label
    Private WithEvents EntryNumber As System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents dataGridViewResults As System.Windows.Forms.DataGridView
    Private WithEvents Value As System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents startButton As System.Windows.Forms.Button

#End Region

End Class


