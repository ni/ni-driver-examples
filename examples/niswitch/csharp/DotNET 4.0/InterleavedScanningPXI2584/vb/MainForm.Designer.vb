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
        Me.dmmInfoGroupBox = New System.Windows.Forms.GroupBox()
        Me.dmmResourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.dmmMeasurementTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.dmmRangeNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.triggerSourceLabel = New System.Windows.Forms.Label()
        Me.measurementCompleteDestinationLabel = New System.Windows.Forms.Label()
        Me.dmmTriggerSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.dmmMeasurementCompleteDestinationComboBox = New System.Windows.Forms.ComboBox()
        Me.samplesLabel = New System.Windows.Forms.Label()
        Me.dmmSamplesNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.resolutionLabel = New System.Windows.Forms.Label()
        Me.rangeLabel = New System.Windows.Forms.Label()
        Me.measurementTypeLabel = New System.Windows.Forms.Label()
        Me.dmmResolutionNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.switchScanAdvancedOutputComboBox = New System.Windows.Forms.ComboBox()
        Me.switchResourceNameLabel = New System.Windows.Forms.Label()
        Me.startButton = New System.Windows.Forms.Button()
        Me.switchInterleavedStartChannelNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.dataGridViewResults = New System.Windows.Forms.DataGridView()
        Me.interleavedStartChannelLabel = New System.Windows.Forms.Label()
        Me.switchResourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.switchInfoGroupBox = New System.Windows.Forms.GroupBox()
        Me.switchTriggerInputComboBox = New System.Windows.Forms.ComboBox()
        Me.scanAdvancedOutputLabel = New System.Windows.Forms.Label()
        Me.switchInterleavedEndChannelNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.triggerInputLabel = New System.Windows.Forms.Label()
        Me.interleavedEndChannelLabel = New System.Windows.Forms.Label()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.dmmInfoGroupBox.SuspendLayout()
        CType(Me.dmmRangeNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dmmSamplesNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dmmResolutionNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.switchInterleavedStartChannelNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.switchInfoGroupBox.SuspendLayout()
        CType(Me.switchInterleavedEndChannelNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dmmInfoGroupBox
        '
        Me.dmmInfoGroupBox.Controls.Add(Me.dmmResourceNameComboBox)
        Me.dmmInfoGroupBox.Controls.Add(Me.dmmMeasurementTypeComboBox)
        Me.dmmInfoGroupBox.Controls.Add(Me.dmmRangeNumericUpDown)
        Me.dmmInfoGroupBox.Controls.Add(Me.triggerSourceLabel)
        Me.dmmInfoGroupBox.Controls.Add(Me.measurementCompleteDestinationLabel)
        Me.dmmInfoGroupBox.Controls.Add(Me.dmmTriggerSourceComboBox)
        Me.dmmInfoGroupBox.Controls.Add(Me.dmmMeasurementCompleteDestinationComboBox)
        Me.dmmInfoGroupBox.Controls.Add(Me.samplesLabel)
        Me.dmmInfoGroupBox.Controls.Add(Me.dmmSamplesNumericUpDown)
        Me.dmmInfoGroupBox.Controls.Add(Me.resolutionLabel)
        Me.dmmInfoGroupBox.Controls.Add(Me.rangeLabel)
        Me.dmmInfoGroupBox.Controls.Add(Me.measurementTypeLabel)
        Me.dmmInfoGroupBox.Controls.Add(Me.dmmResolutionNumericUpDown)
        Me.dmmInfoGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.dmmInfoGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.dmmInfoGroupBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.dmmInfoGroupBox.Location = New System.Drawing.Point(323, 30)
        Me.dmmInfoGroupBox.Name = "dmmInfoGroupBox"
        Me.dmmInfoGroupBox.Size = New System.Drawing.Size(316, 282)
        Me.dmmInfoGroupBox.TabIndex = 1
        Me.dmmInfoGroupBox.TabStop = False
        Me.dmmInfoGroupBox.Text = "DMM"
        '
        'dmmResourceNameComboBox
        '
        Me.dmmResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.dmmResourceNameComboBox.FormattingEnabled = True
        Me.dmmResourceNameComboBox.Location = New System.Drawing.Point(158, 21)
        Me.dmmResourceNameComboBox.Name = "dmmResourceNameComboBox"
        Me.dmmResourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.dmmResourceNameComboBox.TabIndex = 0
        '
        'dmmMeasurementTypeComboBox
        '
        Me.dmmMeasurementTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.dmmMeasurementTypeComboBox.FormattingEnabled = True
        Me.dmmMeasurementTypeComboBox.Location = New System.Drawing.Point(158, 65)
        Me.dmmMeasurementTypeComboBox.Name = "dmmMeasurementTypeComboBox"
        Me.dmmMeasurementTypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.dmmMeasurementTypeComboBox.TabIndex = 1
        '
        'dmmRangeNumericUpDown
        '
        Me.dmmRangeNumericUpDown.DecimalPlaces = 6
        Me.dmmRangeNumericUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.dmmRangeNumericUpDown.Location = New System.Drawing.Point(158, 102)
        Me.dmmRangeNumericUpDown.Minimum = New Decimal(New Integer() {300, 0, 0, -2147352576})
        Me.dmmRangeNumericUpDown.Name = "dmmRangeNumericUpDown"
        Me.dmmRangeNumericUpDown.Size = New System.Drawing.Size(120, 20)
        Me.dmmRangeNumericUpDown.TabIndex = 2
        Me.dmmRangeNumericUpDown.Value = New Decimal(New Integer() {1000, 0, 0, 131072})
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.AutoSize = True
        Me.triggerSourceLabel.Location = New System.Drawing.Point(6, 252)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(77, 13)
        Me.triggerSourceLabel.TabIndex = 13
        Me.triggerSourceLabel.Text = "Trigger Source"
        '
        'measurementCompleteDestinationLabel
        '
        Me.measurementCompleteDestinationLabel.AutoSize = True
        Me.measurementCompleteDestinationLabel.Location = New System.Drawing.Point(6, 211)
        Me.measurementCompleteDestinationLabel.Name = "measurementCompleteDestinationLabel"
        Me.measurementCompleteDestinationLabel.Size = New System.Drawing.Size(151, 13)
        Me.measurementCompleteDestinationLabel.TabIndex = 12
        Me.measurementCompleteDestinationLabel.Text = "Measure Complete Destination"
        '
        'dmmTriggerSourceComboBox
        '
        Me.dmmTriggerSourceComboBox.Location = New System.Drawing.Point(158, 244)
        Me.dmmTriggerSourceComboBox.Name = "dmmTriggerSourceComboBox"
        Me.dmmTriggerSourceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.dmmTriggerSourceComboBox.TabIndex = 6
        '
        'dmmMeasurementCompleteDestinationComboBox
        '
        Me.dmmMeasurementCompleteDestinationComboBox.FormattingEnabled = True
        Me.dmmMeasurementCompleteDestinationComboBox.Location = New System.Drawing.Point(158, 208)
        Me.dmmMeasurementCompleteDestinationComboBox.Name = "dmmMeasurementCompleteDestinationComboBox"
        Me.dmmMeasurementCompleteDestinationComboBox.Size = New System.Drawing.Size(120, 21)
        Me.dmmMeasurementCompleteDestinationComboBox.TabIndex = 5
        '
        'samplesLabel
        '
        Me.samplesLabel.AutoSize = True
        Me.samplesLabel.Location = New System.Drawing.Point(6, 176)
        Me.samplesLabel.Name = "samplesLabel"
        Me.samplesLabel.Size = New System.Drawing.Size(136, 13)
        Me.samplesLabel.TabIndex = 11
        Me.samplesLabel.Text = "Samples to Fetch at a Time"
        '
        'dmmSamplesNumericUpDown
        '
        Me.dmmSamplesNumericUpDown.Location = New System.Drawing.Point(158, 169)
        Me.dmmSamplesNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.dmmSamplesNumericUpDown.Name = "dmmSamplesNumericUpDown"
        Me.dmmSamplesNumericUpDown.Size = New System.Drawing.Size(120, 20)
        Me.dmmSamplesNumericUpDown.TabIndex = 4
        Me.dmmSamplesNumericUpDown.Value = New Decimal(New Integer() {4, 0, 0, 0})
        '
        'resolutionLabel
        '
        Me.resolutionLabel.AutoSize = True
        Me.resolutionLabel.Location = New System.Drawing.Point(6, 143)
        Me.resolutionLabel.Name = "resolutionLabel"
        Me.resolutionLabel.Size = New System.Drawing.Size(57, 13)
        Me.resolutionLabel.TabIndex = 10
        Me.resolutionLabel.Text = "Resolution"
        '
        'rangeLabel
        '
        Me.rangeLabel.AutoSize = True
        Me.rangeLabel.Location = New System.Drawing.Point(6, 104)
        Me.rangeLabel.Name = "rangeLabel"
        Me.rangeLabel.Size = New System.Drawing.Size(39, 13)
        Me.rangeLabel.TabIndex = 9
        Me.rangeLabel.Text = "Range"
        '
        'measurementTypeLabel
        '
        Me.measurementTypeLabel.AutoSize = True
        Me.measurementTypeLabel.Location = New System.Drawing.Point(6, 68)
        Me.measurementTypeLabel.Name = "measurementTypeLabel"
        Me.measurementTypeLabel.Size = New System.Drawing.Size(98, 13)
        Me.measurementTypeLabel.TabIndex = 8
        Me.measurementTypeLabel.Text = "Measurement Type"
        '
        'dmmResolutionNumericUpDown
        '
        Me.dmmResolutionNumericUpDown.DecimalPlaces = 6
        Me.dmmResolutionNumericUpDown.Location = New System.Drawing.Point(158, 136)
        Me.dmmResolutionNumericUpDown.Name = "dmmResolutionNumericUpDown"
        Me.dmmResolutionNumericUpDown.Size = New System.Drawing.Size(120, 20)
        Me.dmmResolutionNumericUpDown.TabIndex = 3
        Me.dmmResolutionNumericUpDown.Value = New Decimal(New Integer() {1, 0, 0, 196608})
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 24)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 7
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'switchScanAdvancedOutputComboBox
        '
        Me.switchScanAdvancedOutputComboBox.FormattingEnabled = True
        Me.switchScanAdvancedOutputComboBox.Location = New System.Drawing.Point(149, 173)
        Me.switchScanAdvancedOutputComboBox.Name = "switchScanAdvancedOutputComboBox"
        Me.switchScanAdvancedOutputComboBox.Size = New System.Drawing.Size(121, 21)
        Me.switchScanAdvancedOutputComboBox.TabIndex = 4
        '
        'switchResourceNameLabel
        '
        Me.switchResourceNameLabel.AutoSize = True
        Me.switchResourceNameLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.switchResourceNameLabel.Location = New System.Drawing.Point(6, 24)
        Me.switchResourceNameLabel.Name = "switchResourceNameLabel"
        Me.switchResourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.switchResourceNameLabel.TabIndex = 5
        Me.switchResourceNameLabel.Text = "Resource Name"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(185, 343)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(97, 23)
        Me.startButton.TabIndex = 2
        Me.startButton.Text = "Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'switchInterleavedStartChannelNumericUpDown
        '
        Me.switchInterleavedStartChannelNumericUpDown.Location = New System.Drawing.Point(150, 61)
        Me.switchInterleavedStartChannelNumericUpDown.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.switchInterleavedStartChannelNumericUpDown.Name = "switchInterleavedStartChannelNumericUpDown"
        Me.switchInterleavedStartChannelNumericUpDown.Size = New System.Drawing.Size(120, 20)
        Me.switchInterleavedStartChannelNumericUpDown.TabIndex = 1
        '
        'dataGridViewResults
        '
        Me.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewResults.Location = New System.Drawing.Point(663, 30)
        Me.dataGridViewResults.Name = "dataGridViewResults"
        Me.dataGridViewResults.Size = New System.Drawing.Size(293, 300)
        Me.dataGridViewResults.TabIndex = 4
        '
        'interleavedStartChannelLabel
        '
        Me.interleavedStartChannelLabel.AutoSize = True
        Me.interleavedStartChannelLabel.Location = New System.Drawing.Point(6, 68)
        Me.interleavedStartChannelLabel.Name = "interleavedStartChannelLabel"
        Me.interleavedStartChannelLabel.Size = New System.Drawing.Size(127, 13)
        Me.interleavedStartChannelLabel.TabIndex = 6
        Me.interleavedStartChannelLabel.Text = "Interleaved Start Channel"
        '
        'switchResourceNameComboBox
        '
        Me.switchResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.switchResourceNameComboBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.switchResourceNameComboBox.FormattingEnabled = True
        Me.switchResourceNameComboBox.Location = New System.Drawing.Point(149, 21)
        Me.switchResourceNameComboBox.Name = "switchResourceNameComboBox"
        Me.switchResourceNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.switchResourceNameComboBox.TabIndex = 0
        '
        'switchInfoGroupBox
        '
        Me.switchInfoGroupBox.Controls.Add(Me.switchResourceNameComboBox)
        Me.switchInfoGroupBox.Controls.Add(Me.switchScanAdvancedOutputComboBox)
        Me.switchInfoGroupBox.Controls.Add(Me.switchTriggerInputComboBox)
        Me.switchInfoGroupBox.Controls.Add(Me.scanAdvancedOutputLabel)
        Me.switchInfoGroupBox.Controls.Add(Me.switchInterleavedEndChannelNumericUpDown)
        Me.switchInfoGroupBox.Controls.Add(Me.triggerInputLabel)
        Me.switchInfoGroupBox.Controls.Add(Me.interleavedEndChannelLabel)
        Me.switchInfoGroupBox.Controls.Add(Me.interleavedStartChannelLabel)
        Me.switchInfoGroupBox.Controls.Add(Me.switchInterleavedStartChannelNumericUpDown)
        Me.switchInfoGroupBox.Controls.Add(Me.switchResourceNameLabel)
        Me.switchInfoGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.switchInfoGroupBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.switchInfoGroupBox.Location = New System.Drawing.Point(12, 30)
        Me.switchInfoGroupBox.Name = "switchInfoGroupBox"
        Me.switchInfoGroupBox.Size = New System.Drawing.Size(305, 282)
        Me.switchInfoGroupBox.TabIndex = 0
        Me.switchInfoGroupBox.TabStop = False
        Me.switchInfoGroupBox.Text = "Switch"
        '
        'switchTriggerInputComboBox
        '
        Me.switchTriggerInputComboBox.FormattingEnabled = True
        Me.switchTriggerInputComboBox.Location = New System.Drawing.Point(149, 143)
        Me.switchTriggerInputComboBox.Name = "switchTriggerInputComboBox"
        Me.switchTriggerInputComboBox.Size = New System.Drawing.Size(121, 21)
        Me.switchTriggerInputComboBox.TabIndex = 3
        '
        'scanAdvancedOutputLabel
        '
        Me.scanAdvancedOutputLabel.AutoSize = True
        Me.scanAdvancedOutputLabel.Location = New System.Drawing.Point(6, 176)
        Me.scanAdvancedOutputLabel.Name = "scanAdvancedOutputLabel"
        Me.scanAdvancedOutputLabel.Size = New System.Drawing.Size(119, 13)
        Me.scanAdvancedOutputLabel.TabIndex = 9
        Me.scanAdvancedOutputLabel.Text = "Scan Advanced Output"
        '
        'switchInterleavedEndChannelNumericUpDown
        '
        Me.switchInterleavedEndChannelNumericUpDown.Location = New System.Drawing.Point(150, 102)
        Me.switchInterleavedEndChannelNumericUpDown.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.switchInterleavedEndChannelNumericUpDown.Name = "switchInterleavedEndChannelNumericUpDown"
        Me.switchInterleavedEndChannelNumericUpDown.Size = New System.Drawing.Size(120, 20)
        Me.switchInterleavedEndChannelNumericUpDown.TabIndex = 2
        Me.switchInterleavedEndChannelNumericUpDown.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'triggerInputLabel
        '
        Me.triggerInputLabel.AutoSize = True
        Me.triggerInputLabel.Location = New System.Drawing.Point(6, 143)
        Me.triggerInputLabel.Name = "triggerInputLabel"
        Me.triggerInputLabel.Size = New System.Drawing.Size(67, 13)
        Me.triggerInputLabel.TabIndex = 8
        Me.triggerInputLabel.Text = "Trigger Input"
        '
        'interleavedEndChannelLabel
        '
        Me.interleavedEndChannelLabel.AutoSize = True
        Me.interleavedEndChannelLabel.Location = New System.Drawing.Point(6, 104)
        Me.interleavedEndChannelLabel.Name = "interleavedEndChannelLabel"
        Me.interleavedEndChannelLabel.Size = New System.Drawing.Size(124, 13)
        Me.interleavedEndChannelLabel.TabIndex = 7
        Me.interleavedEndChannelLabel.Text = "Interleaved End Channel"
        '
        'stopButton
        '
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(383, 343)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(97, 23)
        Me.stopButton.TabIndex = 3
        Me.stopButton.Text = "Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(968, 397)
        Me.Controls.Add(Me.dmmInfoGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.dataGridViewResults)
        Me.Controls.Add(Me.switchInfoGroupBox)
        Me.Controls.Add(Me.stopButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "NI-Switch Interleaved Scanning PXI2584"
        Me.dmmInfoGroupBox.ResumeLayout(False)
        Me.dmmInfoGroupBox.PerformLayout()
        CType(Me.dmmRangeNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dmmSamplesNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dmmResolutionNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.switchInterleavedStartChannelNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.switchInfoGroupBox.ResumeLayout(False)
        Me.switchInfoGroupBox.PerformLayout()
        CType(Me.switchInterleavedEndChannelNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents dmmInfoGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents dmmResourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents dmmMeasurementTypeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents dmmRangeNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents triggerSourceLabel As System.Windows.Forms.Label
    Private WithEvents measurementCompleteDestinationLabel As System.Windows.Forms.Label
    Private WithEvents dmmTriggerSourceComboBox As System.Windows.Forms.ComboBox
    Private WithEvents dmmMeasurementCompleteDestinationComboBox As System.Windows.Forms.ComboBox
    Private WithEvents samplesLabel As System.Windows.Forms.Label
    Private WithEvents dmmSamplesNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents resolutionLabel As System.Windows.Forms.Label
    Private WithEvents rangeLabel As System.Windows.Forms.Label
    Private WithEvents measurementTypeLabel As System.Windows.Forms.Label
    Private WithEvents dmmResolutionNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents resourceNameLabel As System.Windows.Forms.Label
    Private WithEvents switchScanAdvancedOutputComboBox As System.Windows.Forms.ComboBox
    Private WithEvents switchResourceNameLabel As System.Windows.Forms.Label
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents switchInterleavedStartChannelNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents dataGridViewResults As System.Windows.Forms.DataGridView
    Private WithEvents interleavedStartChannelLabel As System.Windows.Forms.Label
    Private WithEvents switchResourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents switchInfoGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents switchTriggerInputComboBox As System.Windows.Forms.ComboBox
    Private WithEvents scanAdvancedOutputLabel As System.Windows.Forms.Label
    Private WithEvents switchInterleavedEndChannelNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents triggerInputLabel As System.Windows.Forms.Label
    Private WithEvents interleavedEndChannelLabel As System.Windows.Forms.Label
    Private WithEvents stopButton As System.Windows.Forms.Button

	#End Region
End Class

