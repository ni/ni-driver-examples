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
        Me.numOfChannelsNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.thermocoupleTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.measurementTypeLabel = New System.Windows.Forms.Label()
        Me.thermocoupleTypeLabel = New System.Windows.Forms.Label()
        Me.numberOfChannelsLabel = New System.Windows.Forms.Label()
        Me.scanAdvancedOutputComboBox = New System.Windows.Forms.ComboBox()
        Me.scanAdvancedOutputLabel = New System.Windows.Forms.Label()
        Me.triggerInputComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerInputLabel = New System.Windows.Forms.Label()
        Me.scanListLabel = New System.Windows.Forms.Label()
        Me.topologyNameComboBox = New System.Windows.Forms.ComboBox()
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.topologyNameLabel = New System.Windows.Forms.Label()
        Me.scanListTextBox = New System.Windows.Forms.TextBox()
        Me.switchResourceNameLabel = New System.Windows.Forms.Label()
        Me.switchResourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.measurementModeComboBox = New System.Windows.Forms.ComboBox()
        Me.measCompleteDestLabel = New System.Windows.Forms.Label()
        Me.dmmResourceNameLabel = New System.Windows.Forms.Label()
        Me.dmmResourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerSourceLabel = New System.Windows.Forms.Label()
        Me.dataGridViewResults = New System.Windows.Forms.DataGridView()
        Me.startButton = New System.Windows.Forms.Button()
        Me.measCompleteDestComboBox = New System.Windows.Forms.ComboBox()
        Me.groupBox2 = New System.Windows.Forms.GroupBox()
        CType(Me.numOfChannelsNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox1.SuspendLayout()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'numOfChannelsNumericUpDown
        '
        Me.numOfChannelsNumericUpDown.Location = New System.Drawing.Point(6, 304)
        Me.numOfChannelsNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numOfChannelsNumericUpDown.Name = "numOfChannelsNumericUpDown"
        Me.numOfChannelsNumericUpDown.Size = New System.Drawing.Size(120, 20)
        Me.numOfChannelsNumericUpDown.TabIndex = 5
        Me.numOfChannelsNumericUpDown.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'thermocoupleTypeComboBox
        '
        Me.thermocoupleTypeComboBox.FormattingEnabled = True
        Me.thermocoupleTypeComboBox.Location = New System.Drawing.Point(6, 359)
        Me.thermocoupleTypeComboBox.Name = "thermocoupleTypeComboBox"
        Me.thermocoupleTypeComboBox.Size = New System.Drawing.Size(121, 21)
        Me.thermocoupleTypeComboBox.TabIndex = 6
        '
        'measurementTypeLabel
        '
        Me.measurementTypeLabel.AutoSize = True
        Me.measurementTypeLabel.Location = New System.Drawing.Point(6, 81)
        Me.measurementTypeLabel.Name = "measurementTypeLabel"
        Me.measurementTypeLabel.Size = New System.Drawing.Size(104, 13)
        Me.measurementTypeLabel.TabIndex = 5
        Me.measurementTypeLabel.Text = "Measurement Mode:"
        '
        'thermocoupleTypeLabel
        '
        Me.thermocoupleTypeLabel.AutoSize = True
        Me.thermocoupleTypeLabel.Location = New System.Drawing.Point(6, 343)
        Me.thermocoupleTypeLabel.Name = "thermocoupleTypeLabel"
        Me.thermocoupleTypeLabel.Size = New System.Drawing.Size(102, 13)
        Me.thermocoupleTypeLabel.TabIndex = 13
        Me.thermocoupleTypeLabel.Text = "Thermocouple Type"
        '
        'numberOfChannelsLabel
        '
        Me.numberOfChannelsLabel.AutoSize = True
        Me.numberOfChannelsLabel.Location = New System.Drawing.Point(6, 288)
        Me.numberOfChannelsLabel.Name = "numberOfChannelsLabel"
        Me.numberOfChannelsLabel.Size = New System.Drawing.Size(105, 13)
        Me.numberOfChannelsLabel.TabIndex = 12
        Me.numberOfChannelsLabel.Text = "Number Of Channels"
        '
        'scanAdvancedOutputComboBox
        '
        Me.scanAdvancedOutputComboBox.FormattingEnabled = True
        Me.scanAdvancedOutputComboBox.Location = New System.Drawing.Point(6, 250)
        Me.scanAdvancedOutputComboBox.Name = "scanAdvancedOutputComboBox"
        Me.scanAdvancedOutputComboBox.Size = New System.Drawing.Size(121, 21)
        Me.scanAdvancedOutputComboBox.TabIndex = 4
        '
        'scanAdvancedOutputLabel
        '
        Me.scanAdvancedOutputLabel.AutoSize = True
        Me.scanAdvancedOutputLabel.Location = New System.Drawing.Point(6, 234)
        Me.scanAdvancedOutputLabel.Name = "scanAdvancedOutputLabel"
        Me.scanAdvancedOutputLabel.Size = New System.Drawing.Size(119, 13)
        Me.scanAdvancedOutputLabel.TabIndex = 11
        Me.scanAdvancedOutputLabel.Text = "Scan Advanced Output"
        '
        'triggerInputComboBox
        '
        Me.triggerInputComboBox.FormattingEnabled = True
        Me.triggerInputComboBox.Location = New System.Drawing.Point(6, 198)
        Me.triggerInputComboBox.Name = "triggerInputComboBox"
        Me.triggerInputComboBox.Size = New System.Drawing.Size(121, 21)
        Me.triggerInputComboBox.TabIndex = 3
        '
        'triggerInputLabel
        '
        Me.triggerInputLabel.AutoSize = True
        Me.triggerInputLabel.Location = New System.Drawing.Point(6, 182)
        Me.triggerInputLabel.Name = "triggerInputLabel"
        Me.triggerInputLabel.Size = New System.Drawing.Size(67, 13)
        Me.triggerInputLabel.TabIndex = 10
        Me.triggerInputLabel.Text = "Trigger Input"
        '
        'scanListLabel
        '
        Me.scanListLabel.AutoSize = True
        Me.scanListLabel.Location = New System.Drawing.Point(6, 133)
        Me.scanListLabel.Name = "scanListLabel"
        Me.scanListLabel.Size = New System.Drawing.Size(51, 13)
        Me.scanListLabel.TabIndex = 9
        Me.scanListLabel.Text = "Scan List"
        '
        'topologyNameComboBox
        '
        Me.topologyNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.topologyNameComboBox.FormattingEnabled = True
        Me.topologyNameComboBox.Location = New System.Drawing.Point(6, 97)
        Me.topologyNameComboBox.Name = "topologyNameComboBox"
        Me.topologyNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.topologyNameComboBox.TabIndex = 1
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.numOfChannelsNumericUpDown)
        Me.groupBox1.Controls.Add(Me.thermocoupleTypeComboBox)
        Me.groupBox1.Controls.Add(Me.thermocoupleTypeLabel)
        Me.groupBox1.Controls.Add(Me.numberOfChannelsLabel)
        Me.groupBox1.Controls.Add(Me.scanAdvancedOutputComboBox)
        Me.groupBox1.Controls.Add(Me.scanAdvancedOutputLabel)
        Me.groupBox1.Controls.Add(Me.triggerInputComboBox)
        Me.groupBox1.Controls.Add(Me.triggerInputLabel)
        Me.groupBox1.Controls.Add(Me.scanListLabel)
        Me.groupBox1.Controls.Add(Me.topologyNameLabel)
        Me.groupBox1.Controls.Add(Me.topologyNameComboBox)
        Me.groupBox1.Controls.Add(Me.scanListTextBox)
        Me.groupBox1.Controls.Add(Me.switchResourceNameLabel)
        Me.groupBox1.Controls.Add(Me.switchResourceNameComboBox)
        Me.groupBox1.Location = New System.Drawing.Point(22, 40)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(152, 398)
        Me.groupBox1.TabIndex = 0
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Switch"
        '
        'topologyNameLabel
        '
        Me.topologyNameLabel.AutoSize = True
        Me.topologyNameLabel.Location = New System.Drawing.Point(6, 81)
        Me.topologyNameLabel.Name = "topologyNameLabel"
        Me.topologyNameLabel.Size = New System.Drawing.Size(82, 13)
        Me.topologyNameLabel.TabIndex = 8
        Me.topologyNameLabel.Text = "Topology Name"
        '
        'scanListTextBox
        '
        Me.scanListTextBox.Location = New System.Drawing.Point(6, 149)
        Me.scanListTextBox.Name = "scanListTextBox"
        Me.scanListTextBox.Size = New System.Drawing.Size(121, 20)
        Me.scanListTextBox.TabIndex = 2
        Me.scanListTextBox.Text = "ch0:2->com0;"
        '
        'switchResourceNameLabel
        '
        Me.switchResourceNameLabel.AutoSize = True
        Me.switchResourceNameLabel.Location = New System.Drawing.Point(6, 28)
        Me.switchResourceNameLabel.Name = "switchResourceNameLabel"
        Me.switchResourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.switchResourceNameLabel.TabIndex = 7
        Me.switchResourceNameLabel.Text = "Resource Name"
        '
        'switchResourceNameComboBox
        '
        Me.switchResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.switchResourceNameComboBox.FormattingEnabled = True
        Me.switchResourceNameComboBox.Location = New System.Drawing.Point(6, 44)
        Me.switchResourceNameComboBox.Name = "switchResourceNameComboBox"
        Me.switchResourceNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.switchResourceNameComboBox.TabIndex = 0
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(505, 337)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 3
        Me.stopButton.Text = "Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'measurementModeComboBox
        '
        Me.measurementModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.measurementModeComboBox.FormattingEnabled = True
        Me.measurementModeComboBox.Location = New System.Drawing.Point(9, 97)
        Me.measurementModeComboBox.Name = "measurementModeComboBox"
        Me.measurementModeComboBox.Size = New System.Drawing.Size(121, 21)
        Me.measurementModeComboBox.TabIndex = 1
        '
        'measCompleteDestLabel
        '
        Me.measCompleteDestLabel.AutoSize = True
        Me.measCompleteDestLabel.Location = New System.Drawing.Point(6, 133)
        Me.measCompleteDestLabel.Name = "measCompleteDestLabel"
        Me.measCompleteDestLabel.Size = New System.Drawing.Size(121, 26)
        Me.measCompleteDestLabel.TabIndex = 6
        Me.measCompleteDestLabel.Text = "Measurement Complete " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Destination"
        '
        'dmmResourceNameLabel
        '
        Me.dmmResourceNameLabel.AutoSize = True
        Me.dmmResourceNameLabel.Location = New System.Drawing.Point(6, 28)
        Me.dmmResourceNameLabel.Name = "dmmResourceNameLabel"
        Me.dmmResourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.dmmResourceNameLabel.TabIndex = 4
        Me.dmmResourceNameLabel.Text = "Resource Name:"
        '
        'dmmResourceNameComboBox
        '
        Me.dmmResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.dmmResourceNameComboBox.FormattingEnabled = True
        Me.dmmResourceNameComboBox.Location = New System.Drawing.Point(9, 44)
        Me.dmmResourceNameComboBox.Name = "dmmResourceNameComboBox"
        Me.dmmResourceNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.dmmResourceNameComboBox.TabIndex = 0
        '
        'triggerSourceComboBox
        '
        Me.triggerSourceComboBox.FormattingEnabled = True
        Me.triggerSourceComboBox.Location = New System.Drawing.Point(9, 226)
        Me.triggerSourceComboBox.Name = "triggerSourceComboBox"
        Me.triggerSourceComboBox.Size = New System.Drawing.Size(121, 21)
        Me.triggerSourceComboBox.TabIndex = 3
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.AutoSize = True
        Me.triggerSourceLabel.Location = New System.Drawing.Point(6, 206)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(77, 13)
        Me.triggerSourceLabel.TabIndex = 7
        Me.triggerSourceLabel.Text = "Trigger Source"
        '
        'dataGridViewResults
        '
        Me.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dataGridViewResults.Location = New System.Drawing.Point(348, 52)
        Me.dataGridViewResults.Name = "dataGridViewResults"
        Me.dataGridViewResults.Size = New System.Drawing.Size(255, 251)
        Me.dataGridViewResults.TabIndex = 4
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(385, 337)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 2
        Me.startButton.Text = "Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'measCompleteDestComboBox
        '
        Me.measCompleteDestComboBox.FormattingEnabled = True
        Me.measCompleteDestComboBox.Location = New System.Drawing.Point(9, 162)
        Me.measCompleteDestComboBox.Name = "measCompleteDestComboBox"
        Me.measCompleteDestComboBox.Size = New System.Drawing.Size(121, 21)
        Me.measCompleteDestComboBox.TabIndex = 2
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.triggerSourceComboBox)
        Me.groupBox2.Controls.Add(Me.triggerSourceLabel)
        Me.groupBox2.Controls.Add(Me.measCompleteDestComboBox)
        Me.groupBox2.Controls.Add(Me.measCompleteDestLabel)
        Me.groupBox2.Controls.Add(Me.measurementTypeLabel)
        Me.groupBox2.Controls.Add(Me.measurementModeComboBox)
        Me.groupBox2.Controls.Add(Me.dmmResourceNameLabel)
        Me.groupBox2.Controls.Add(Me.dmmResourceNameComboBox)
        Me.groupBox2.Location = New System.Drawing.Point(180, 40)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(152, 398)
        Me.groupBox2.TabIndex = 1
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Dmm"
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(625, 478)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.dataGridViewResults)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.groupBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Thermocouple Measurements"
        CType(Me.numOfChannelsNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents numOfChannelsNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents thermocoupleTypeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents measurementTypeLabel As System.Windows.Forms.Label
    Private WithEvents thermocoupleTypeLabel As System.Windows.Forms.Label
    Private WithEvents numberOfChannelsLabel As System.Windows.Forms.Label
    Private WithEvents scanAdvancedOutputComboBox As System.Windows.Forms.ComboBox
    Private WithEvents scanAdvancedOutputLabel As System.Windows.Forms.Label
    Private WithEvents triggerInputComboBox As System.Windows.Forms.ComboBox
    Private WithEvents triggerInputLabel As System.Windows.Forms.Label
    Private WithEvents scanListLabel As System.Windows.Forms.Label
    Private WithEvents topologyNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents topologyNameLabel As System.Windows.Forms.Label
    Private WithEvents scanListTextBox As System.Windows.Forms.TextBox
    Private WithEvents switchResourceNameLabel As System.Windows.Forms.Label
    Private WithEvents switchResourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents measurementModeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents measCompleteDestLabel As System.Windows.Forms.Label
    Private WithEvents dmmResourceNameLabel As System.Windows.Forms.Label
    Private WithEvents dmmResourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents triggerSourceComboBox As System.Windows.Forms.ComboBox
    Private WithEvents triggerSourceLabel As System.Windows.Forms.Label
    Private WithEvents dataGridViewResults As System.Windows.Forms.DataGridView
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents measCompleteDestComboBox As System.Windows.Forms.ComboBox
    Private WithEvents groupBox2 As System.Windows.Forms.GroupBox

	#End Region
End Class

