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
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.scanAdvancedOutputComboBox = New System.Windows.Forms.ComboBox()
        Me.scanAdvancedOutputLabel = New System.Windows.Forms.Label()
        Me.switchTriggerInputComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerInputLabel = New System.Windows.Forms.Label()
        Me.scanListLabel = New System.Windows.Forms.Label()
        Me.topologyNameLabel = New System.Windows.Forms.Label()
        Me.topologyNameComboBox = New System.Windows.Forms.ComboBox()
        Me.scanListTextBox = New System.Windows.Forms.TextBox()
        Me.switchResourceNameLabel = New System.Windows.Forms.Label()
        Me.switchResourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.samplesToFetchNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.samplesToFetchLabel = New System.Windows.Forms.Label()
        Me.rangeNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.resolutionNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.rangeLabel = New System.Windows.Forms.Label()
        Me.resolutionLabel = New System.Windows.Forms.Label()
        Me.startButton = New System.Windows.Forms.Button()
        Me.measCompleteDestComboBox = New System.Windows.Forms.ComboBox()
        Me.dataGridViewResults = New System.Windows.Forms.DataGridView()
        Me.triggerSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.measurementCompleteDestinationLabel = New System.Windows.Forms.Label()
        Me.groupBox2 = New System.Windows.Forms.GroupBox()
        Me.triggerSourceLabel = New System.Windows.Forms.Label()
        Me.measurementTypeLabel = New System.Windows.Forms.Label()
        Me.measurementModeComboBox = New System.Windows.Forms.ComboBox()
        Me.dmmResourceNameLabel = New System.Windows.Forms.Label()
        Me.dmmResourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.groupBox1.SuspendLayout()
        CType(Me.samplesToFetchNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rangeNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.resolutionNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.scanAdvancedOutputComboBox)
        Me.groupBox1.Controls.Add(Me.scanAdvancedOutputLabel)
        Me.groupBox1.Controls.Add(Me.switchTriggerInputComboBox)
        Me.groupBox1.Controls.Add(Me.triggerInputLabel)
        Me.groupBox1.Controls.Add(Me.scanListLabel)
        Me.groupBox1.Controls.Add(Me.topologyNameLabel)
        Me.groupBox1.Controls.Add(Me.topologyNameComboBox)
        Me.groupBox1.Controls.Add(Me.scanListTextBox)
        Me.groupBox1.Controls.Add(Me.switchResourceNameLabel)
        Me.groupBox1.Controls.Add(Me.switchResourceNameComboBox)
        Me.groupBox1.Location = New System.Drawing.Point(9, 12)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(152, 359)
        Me.groupBox1.TabIndex = 0
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Switch"
        '
        'scanAdvancedOutputComboBox
        '
        Me.scanAdvancedOutputComboBox.FormattingEnabled = True
        Me.scanAdvancedOutputComboBox.Location = New System.Drawing.Point(6, 265)
        Me.scanAdvancedOutputComboBox.Name = "scanAdvancedOutputComboBox"
        Me.scanAdvancedOutputComboBox.Size = New System.Drawing.Size(121, 21)
        Me.scanAdvancedOutputComboBox.TabIndex = 4
        '
        'scanAdvancedOutputLabel
        '
        Me.scanAdvancedOutputLabel.AutoSize = True
        Me.scanAdvancedOutputLabel.Location = New System.Drawing.Point(6, 249)
        Me.scanAdvancedOutputLabel.Name = "scanAdvancedOutputLabel"
        Me.scanAdvancedOutputLabel.Size = New System.Drawing.Size(119, 13)
        Me.scanAdvancedOutputLabel.TabIndex = 9
        Me.scanAdvancedOutputLabel.Text = "Scan Advanced Output"
        '
        'switchTriggerInputComboBox
        '
        Me.switchTriggerInputComboBox.FormattingEnabled = True
        Me.switchTriggerInputComboBox.Location = New System.Drawing.Point(6, 215)
        Me.switchTriggerInputComboBox.Name = "switchTriggerInputComboBox"
        Me.switchTriggerInputComboBox.Size = New System.Drawing.Size(121, 21)
        Me.switchTriggerInputComboBox.TabIndex = 3
        '
        'triggerInputLabel
        '
        Me.triggerInputLabel.AutoSize = True
        Me.triggerInputLabel.Location = New System.Drawing.Point(6, 199)
        Me.triggerInputLabel.Name = "triggerInputLabel"
        Me.triggerInputLabel.Size = New System.Drawing.Size(67, 13)
        Me.triggerInputLabel.TabIndex = 8
        Me.triggerInputLabel.Text = "Trigger Input"
        '
        'scanListLabel
        '
        Me.scanListLabel.AutoSize = True
        Me.scanListLabel.Location = New System.Drawing.Point(6, 149)
        Me.scanListLabel.Name = "scanListLabel"
        Me.scanListLabel.Size = New System.Drawing.Size(51, 13)
        Me.scanListLabel.TabIndex = 7
        Me.scanListLabel.Text = "Scan List"
        '
        'topologyNameLabel
        '
        Me.topologyNameLabel.AutoSize = True
        Me.topologyNameLabel.Location = New System.Drawing.Point(6, 92)
        Me.topologyNameLabel.Name = "topologyNameLabel"
        Me.topologyNameLabel.Size = New System.Drawing.Size(82, 13)
        Me.topologyNameLabel.TabIndex = 6
        Me.topologyNameLabel.Text = "Topology Name"
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
        'scanListTextBox
        '
        Me.scanListTextBox.Location = New System.Drawing.Point(6, 165)
        Me.scanListTextBox.Name = "scanListTextBox"
        Me.scanListTextBox.Size = New System.Drawing.Size(121, 20)
        Me.scanListTextBox.TabIndex = 2
        Me.scanListTextBox.Text = "ab0->com0 & com0->r0 & c0:31->r0;"
        '
        'switchResourceNameLabel
        '
        Me.switchResourceNameLabel.AutoSize = True
        Me.switchResourceNameLabel.Location = New System.Drawing.Point(6, 27)
        Me.switchResourceNameLabel.Name = "switchResourceNameLabel"
        Me.switchResourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.switchResourceNameLabel.TabIndex = 5
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
        'samplesToFetchNumericUpDown
        '
        Me.samplesToFetchNumericUpDown.DecimalPlaces = 2
        Me.samplesToFetchNumericUpDown.Location = New System.Drawing.Point(6, 216)
        Me.samplesToFetchNumericUpDown.Name = "samplesToFetchNumericUpDown"
        Me.samplesToFetchNumericUpDown.Size = New System.Drawing.Size(94, 20)
        Me.samplesToFetchNumericUpDown.TabIndex = 4
        Me.samplesToFetchNumericUpDown.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'samplesToFetchLabel
        '
        Me.samplesToFetchLabel.AutoSize = True
        Me.samplesToFetchLabel.Location = New System.Drawing.Point(3, 200)
        Me.samplesToFetchLabel.Name = "samplesToFetchLabel"
        Me.samplesToFetchLabel.Size = New System.Drawing.Size(140, 13)
        Me.samplesToFetchLabel.TabIndex = 10
        Me.samplesToFetchLabel.Text = "Samples To Fetch at a Time"
        '
        'rangeNumericUpDown
        '
        Me.rangeNumericUpDown.DecimalPlaces = 2
        Me.rangeNumericUpDown.Location = New System.Drawing.Point(6, 166)
        Me.rangeNumericUpDown.Name = "rangeNumericUpDown"
        Me.rangeNumericUpDown.Size = New System.Drawing.Size(94, 20)
        Me.rangeNumericUpDown.TabIndex = 2
        Me.rangeNumericUpDown.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'resolutionNumericUpDown
        '
        Me.resolutionNumericUpDown.DecimalPlaces = 6
        Me.resolutionNumericUpDown.Location = New System.Drawing.Point(105, 166)
        Me.resolutionNumericUpDown.Name = "resolutionNumericUpDown"
        Me.resolutionNumericUpDown.Size = New System.Drawing.Size(94, 20)
        Me.resolutionNumericUpDown.TabIndex = 3
        Me.resolutionNumericUpDown.Value = New Decimal(New Integer() {1, 0, 0, 196608})
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
        'resolutionLabel
        '
        Me.resolutionLabel.AutoSize = True
        Me.resolutionLabel.Location = New System.Drawing.Point(125, 149)
        Me.resolutionLabel.Name = "resolutionLabel"
        Me.resolutionLabel.Size = New System.Drawing.Size(57, 13)
        Me.resolutionLabel.TabIndex = 13
        Me.resolutionLabel.Text = "Resolution"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(406, 343)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 2
        Me.startButton.Text = "Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'measCompleteDestComboBox
        '
        Me.measCompleteDestComboBox.FormattingEnabled = True
        Me.measCompleteDestComboBox.Location = New System.Drawing.Point(9, 277)
        Me.measCompleteDestComboBox.Name = "measCompleteDestComboBox"
        Me.measCompleteDestComboBox.Size = New System.Drawing.Size(121, 21)
        Me.measCompleteDestComboBox.TabIndex = 5
        '
        'dataGridViewResults
        '
        Me.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dataGridViewResults.Location = New System.Drawing.Point(379, 24)
        Me.dataGridViewResults.Name = "dataGridViewResults"
        Me.dataGridViewResults.Size = New System.Drawing.Size(245, 312)
        Me.dataGridViewResults.TabIndex = 4
        '
        'triggerSourceComboBox
        '
        Me.triggerSourceComboBox.FormattingEnabled = True
        Me.triggerSourceComboBox.Location = New System.Drawing.Point(9, 327)
        Me.triggerSourceComboBox.Name = "triggerSourceComboBox"
        Me.triggerSourceComboBox.Size = New System.Drawing.Size(121, 21)
        Me.triggerSourceComboBox.TabIndex = 6
        '
        'measurementCompleteDestinationLabel
        '
        Me.measurementCompleteDestinationLabel.AutoSize = True
        Me.measurementCompleteDestinationLabel.Location = New System.Drawing.Point(9, 248)
        Me.measurementCompleteDestinationLabel.Name = "measurementCompleteDestinationLabel"
        Me.measurementCompleteDestinationLabel.Size = New System.Drawing.Size(118, 26)
        Me.measurementCompleteDestinationLabel.TabIndex = 11
        Me.measurementCompleteDestinationLabel.Text = "Measurement Complete" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Destination"
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.samplesToFetchNumericUpDown)
        Me.groupBox2.Controls.Add(Me.samplesToFetchLabel)
        Me.groupBox2.Controls.Add(Me.rangeNumericUpDown)
        Me.groupBox2.Controls.Add(Me.resolutionNumericUpDown)
        Me.groupBox2.Controls.Add(Me.resolutionLabel)
        Me.groupBox2.Controls.Add(Me.rangeLabel)
        Me.groupBox2.Controls.Add(Me.measCompleteDestComboBox)
        Me.groupBox2.Controls.Add(Me.triggerSourceComboBox)
        Me.groupBox2.Controls.Add(Me.measurementCompleteDestinationLabel)
        Me.groupBox2.Controls.Add(Me.triggerSourceLabel)
        Me.groupBox2.Controls.Add(Me.measurementTypeLabel)
        Me.groupBox2.Controls.Add(Me.measurementModeComboBox)
        Me.groupBox2.Controls.Add(Me.dmmResourceNameLabel)
        Me.groupBox2.Controls.Add(Me.dmmResourceNameComboBox)
        Me.groupBox2.Location = New System.Drawing.Point(167, 12)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(206, 359)
        Me.groupBox2.TabIndex = 1
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Dmm"
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.AutoSize = True
        Me.triggerSourceLabel.Location = New System.Drawing.Point(9, 311)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(77, 13)
        Me.triggerSourceLabel.TabIndex = 12
        Me.triggerSourceLabel.Text = "Trigger Source"
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
        'dmmResourceNameComboBox
        '
        Me.dmmResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.dmmResourceNameComboBox.FormattingEnabled = True
        Me.dmmResourceNameComboBox.Location = New System.Drawing.Point(12, 44)
        Me.dmmResourceNameComboBox.Name = "dmmResourceNameComboBox"
        Me.dmmResourceNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.dmmResourceNameComboBox.TabIndex = 0
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(512, 343)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 3
        Me.stopButton.Text = "Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(632, 383)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.dataGridViewResults)
        Me.Controls.Add(Me.groupBox2)
        Me.Controls.Add(Me.stopButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Dmm-Switch Handshaking"
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        CType(Me.samplesToFetchNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rangeNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.resolutionNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents scanAdvancedOutputComboBox As System.Windows.Forms.ComboBox
    Private WithEvents scanAdvancedOutputLabel As System.Windows.Forms.Label
    Private WithEvents switchTriggerInputComboBox As System.Windows.Forms.ComboBox
    Private WithEvents triggerInputLabel As System.Windows.Forms.Label
    Private WithEvents scanListLabel As System.Windows.Forms.Label
    Private WithEvents topologyNameLabel As System.Windows.Forms.Label
    Private WithEvents topologyNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents scanListTextBox As System.Windows.Forms.TextBox
    Private WithEvents switchResourceNameLabel As System.Windows.Forms.Label
    Private WithEvents switchResourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents samplesToFetchNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents samplesToFetchLabel As System.Windows.Forms.Label
    Private WithEvents rangeNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents resolutionNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents rangeLabel As System.Windows.Forms.Label
    Private WithEvents resolutionLabel As System.Windows.Forms.Label
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents measCompleteDestComboBox As System.Windows.Forms.ComboBox
    Private WithEvents dataGridViewResults As System.Windows.Forms.DataGridView
    Private WithEvents triggerSourceComboBox As System.Windows.Forms.ComboBox
    Private WithEvents measurementCompleteDestinationLabel As System.Windows.Forms.Label
    Private WithEvents groupBox2 As System.Windows.Forms.GroupBox
    Private WithEvents triggerSourceLabel As System.Windows.Forms.Label
    Private WithEvents measurementTypeLabel As System.Windows.Forms.Label
    Private WithEvents measurementModeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents dmmResourceNameLabel As System.Windows.Forms.Label
    Private WithEvents dmmResourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents stopButton As System.Windows.Forms.Button

	#End Region
End Class

