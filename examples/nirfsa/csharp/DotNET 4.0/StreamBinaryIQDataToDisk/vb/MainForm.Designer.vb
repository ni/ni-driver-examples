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
        Me.referenceLevelLabel = New System.Windows.Forms.Label()
        Me.carrierFrequencyLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.samplesPerBlockLabel = New System.Windows.Forms.Label()
        Me.maxSamplesLabel = New System.Windows.Forms.Label()
        Me.fileNameLabel = New System.Windows.Forms.Label()
        Me.dtLabel = New System.Windows.Forms.Label()
        Me.gainLabel = New System.Windows.Forms.Label()
        Me.offsetLabel = New System.Windows.Forms.Label()
        Me.samplesSoFarLabel = New System.Windows.Forms.Label()
        Me.referenceLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.carrierFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.samplesPerBlockNumeric = New System.Windows.Forms.NumericUpDown()
        Me.maxSamplesNumeric = New System.Windows.Forms.NumericUpDown()
        Me.fileNameTextBox = New System.Windows.Forms.TextBox()
        Me.dtTextBox = New System.Windows.Forms.TextBox()
        Me.gainTextBox = New System.Windows.Forms.TextBox()
        Me.offsetTextBox = New System.Windows.Forms.TextBox()
        Me.samplesSoFarTextBox = New System.Windows.Forms.TextBox()
        Me.rfsaResourceNameLabel = New System.Windows.Forms.Label()
        Me.streamToDiskButton = New System.Windows.Forms.Button()
        Me.readFromDiskButton = New System.Windows.Forms.Button()
        Me.dataGridViewResults = New System.Windows.Forms.DataGridView()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.carrierFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerBlockNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maxSamplesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'referenceLevelLabel
        '
        Me.referenceLevelLabel.AutoSize = True
        Me.referenceLevelLabel.Location = New System.Drawing.Point(12, 58)
        Me.referenceLevelLabel.Name = "referenceLevelLabel"
        Me.referenceLevelLabel.Size = New System.Drawing.Size(116, 13)
        Me.referenceLevelLabel.TabIndex = 0
        Me.referenceLevelLabel.Text = "Reference Level (dBm)"
        '
        'carrierFrequencyLabel
        '
        Me.carrierFrequencyLabel.AutoSize = True
        Me.carrierFrequencyLabel.Location = New System.Drawing.Point(12, 107)
        Me.carrierFrequencyLabel.Name = "carrierFrequencyLabel"
        Me.carrierFrequencyLabel.Size = New System.Drawing.Size(112, 13)
        Me.carrierFrequencyLabel.TabIndex = 1
        Me.carrierFrequencyLabel.Text = "Carrier Frequency (Hz)"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(12, 156)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(44, 13)
        Me.iqRateLabel.TabIndex = 2
        Me.iqRateLabel.Text = "IQ Rate"
        '
        'samplesPerBlockLabel
        '
        Me.samplesPerBlockLabel.AutoSize = True
        Me.samplesPerBlockLabel.Location = New System.Drawing.Point(12, 206)
        Me.samplesPerBlockLabel.Name = "samplesPerBlockLabel"
        Me.samplesPerBlockLabel.Size = New System.Drawing.Size(142, 13)
        Me.samplesPerBlockLabel.TabIndex = 3
        Me.samplesPerBlockLabel.Text = "Maximum Samples per Block"
        '
        'maxSamplesLabel
        '
        Me.maxSamplesLabel.AutoSize = True
        Me.maxSamplesLabel.Location = New System.Drawing.Point(12, 256)
        Me.maxSamplesLabel.Name = "maxSamplesLabel"
        Me.maxSamplesLabel.Size = New System.Drawing.Size(114, 13)
        Me.maxSamplesLabel.TabIndex = 4
        Me.maxSamplesLabel.Text = "Total Samples to Write"
        '
        'fileNameLabel
        '
        Me.fileNameLabel.AutoSize = True
        Me.fileNameLabel.Location = New System.Drawing.Point(12, 304)
        Me.fileNameLabel.Name = "fileNameLabel"
        Me.fileNameLabel.Size = New System.Drawing.Size(49, 13)
        Me.fileNameLabel.TabIndex = 5
        Me.fileNameLabel.Text = "Filename"
        '
        'dtLabel
        '
        Me.dtLabel.AutoSize = True
        Me.dtLabel.Location = New System.Drawing.Point(157, 361)
        Me.dtLabel.Name = "dtLabel"
        Me.dtLabel.Size = New System.Drawing.Size(16, 13)
        Me.dtLabel.TabIndex = 6
        Me.dtLabel.Text = "dt"
        '
        'gainLabel
        '
        Me.gainLabel.AutoSize = True
        Me.gainLabel.Location = New System.Drawing.Point(279, 361)
        Me.gainLabel.Name = "gainLabel"
        Me.gainLabel.Size = New System.Drawing.Size(29, 13)
        Me.gainLabel.TabIndex = 7
        Me.gainLabel.Text = "Gain"
        '
        'offsetLabel
        '
        Me.offsetLabel.AutoSize = True
        Me.offsetLabel.Location = New System.Drawing.Point(401, 361)
        Me.offsetLabel.Name = "offsetLabel"
        Me.offsetLabel.Size = New System.Drawing.Size(35, 13)
        Me.offsetLabel.TabIndex = 8
        Me.offsetLabel.Text = "Offset"
        '
        'samplesSoFarLabel
        '
        Me.samplesSoFarLabel.AutoSize = True
        Me.samplesSoFarLabel.Location = New System.Drawing.Point(157, 407)
        Me.samplesSoFarLabel.Name = "samplesSoFarLabel"
        Me.samplesSoFarLabel.Size = New System.Drawing.Size(149, 13)
        Me.samplesSoFarLabel.TabIndex = 9
        Me.samplesSoFarLabel.Text = "Samples Read/Written So Far"
        '
        'referenceLevelNumeric
        '
        Me.referenceLevelNumeric.DecimalPlaces = 2
        Me.referenceLevelNumeric.Location = New System.Drawing.Point(12, 77)
        Me.referenceLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.referenceLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.referenceLevelNumeric.Name = "referenceLevelNumeric"
        Me.referenceLevelNumeric.Size = New System.Drawing.Size(116, 20)
        Me.referenceLevelNumeric.TabIndex = 1
        '
        'carrierFrequencyNumeric
        '
        Me.carrierFrequencyNumeric.DecimalPlaces = 2
        Me.carrierFrequencyNumeric.Location = New System.Drawing.Point(12, 127)
        Me.carrierFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.carrierFrequencyNumeric.Name = "carrierFrequencyNumeric"
        Me.carrierFrequencyNumeric.Size = New System.Drawing.Size(116, 20)
        Me.carrierFrequencyNumeric.TabIndex = 2
        Me.carrierFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 2
        Me.iqRateNumeric.Location = New System.Drawing.Point(12, 175)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(116, 20)
        Me.iqRateNumeric.TabIndex = 3
        Me.iqRateNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'samplesPerBlockNumeric
        '
        Me.samplesPerBlockNumeric.Location = New System.Drawing.Point(12, 225)
        Me.samplesPerBlockNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.samplesPerBlockNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerBlockNumeric.Name = "samplesPerBlockNumeric"
        Me.samplesPerBlockNumeric.Size = New System.Drawing.Size(116, 20)
        Me.samplesPerBlockNumeric.TabIndex = 4
        Me.samplesPerBlockNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'maxSamplesNumeric
        '
        Me.maxSamplesNumeric.Location = New System.Drawing.Point(12, 276)
        Me.maxSamplesNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.maxSamplesNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.maxSamplesNumeric.Name = "maxSamplesNumeric"
        Me.maxSamplesNumeric.Size = New System.Drawing.Size(116, 20)
        Me.maxSamplesNumeric.TabIndex = 5
        Me.maxSamplesNumeric.Value = New Decimal(New Integer() {100000, 0, 0, 0})
        '
        'fileNameTextBox
        '
        Me.fileNameTextBox.Location = New System.Drawing.Point(12, 322)
        Me.fileNameTextBox.Name = "fileNameTextBox"
        Me.fileNameTextBox.Size = New System.Drawing.Size(117, 20)
        Me.fileNameTextBox.TabIndex = 6
        Me.fileNameTextBox.Text = "BinaryFile.bin"
        '
        'dtTextBox
        '
        Me.dtTextBox.Location = New System.Drawing.Point(160, 377)
        Me.dtTextBox.Name = "dtTextBox"
        Me.dtTextBox.ReadOnly = True
        Me.dtTextBox.Size = New System.Drawing.Size(116, 20)
        Me.dtTextBox.TabIndex = 12
        Me.dtTextBox.Text = "0.00000000000000E+0"
        '
        'gainTextBox
        '
        Me.gainTextBox.Location = New System.Drawing.Point(282, 377)
        Me.gainTextBox.Name = "gainTextBox"
        Me.gainTextBox.ReadOnly = True
        Me.gainTextBox.Size = New System.Drawing.Size(116, 20)
        Me.gainTextBox.TabIndex = 13
        Me.gainTextBox.Text = "0.00000000000000E+0"
        '
        'offsetTextBox
        '
        Me.offsetTextBox.Location = New System.Drawing.Point(404, 377)
        Me.offsetTextBox.Name = "offsetTextBox"
        Me.offsetTextBox.ReadOnly = True
        Me.offsetTextBox.Size = New System.Drawing.Size(116, 20)
        Me.offsetTextBox.TabIndex = 14
        Me.offsetTextBox.Text = "0.00000000000000E+0"
        '
        'samplesSoFarTextBox
        '
        Me.samplesSoFarTextBox.Location = New System.Drawing.Point(312, 403)
        Me.samplesSoFarTextBox.Name = "samplesSoFarTextBox"
        Me.samplesSoFarTextBox.ReadOnly = True
        Me.samplesSoFarTextBox.Size = New System.Drawing.Size(116, 20)
        Me.samplesSoFarTextBox.TabIndex = 15
        Me.samplesSoFarTextBox.Text = "0"
        '
        'rfsaResourceNameLabel
        '
        Me.rfsaResourceNameLabel.AutoSize = True
        Me.rfsaResourceNameLabel.Location = New System.Drawing.Point(12, 15)
        Me.rfsaResourceNameLabel.Name = "rfsaResourceNameLabel"
        Me.rfsaResourceNameLabel.Size = New System.Drawing.Size(115, 13)
        Me.rfsaResourceNameLabel.TabIndex = 17
        Me.rfsaResourceNameLabel.Text = "RFSA Resource Name"
        '
        'streamToDiskButton
        '
        Me.streamToDiskButton.Location = New System.Drawing.Point(12, 361)
        Me.streamToDiskButton.Name = "streamToDiskButton"
        Me.streamToDiskButton.Size = New System.Drawing.Size(114, 23)
        Me.streamToDiskButton.TabIndex = 7
        Me.streamToDiskButton.Text = "&Stream To Disk"
        Me.streamToDiskButton.UseVisualStyleBackColor = True
        '
        'readFromDiskButton
        '
        Me.readFromDiskButton.Location = New System.Drawing.Point(12, 402)
        Me.readFromDiskButton.Name = "readFromDiskButton"
        Me.readFromDiskButton.Size = New System.Drawing.Size(114, 23)
        Me.readFromDiskButton.TabIndex = 8
        Me.readFromDiskButton.Text = "&Read From Disk"
        Me.readFromDiskButton.UseVisualStyleBackColor = True
        '
        'dataGridViewResults
        '
        Me.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dataGridViewResults.Location = New System.Drawing.Point(160, 15)
        Me.dataGridViewResults.Name = "dataGridViewResults"
        Me.dataGridViewResults.Size = New System.Drawing.Size(360, 327)
        Me.dataGridViewResults.TabIndex = 11
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 31)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(116, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(529, 433)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.dataGridViewResults)
        Me.Controls.Add(Me.readFromDiskButton)
        Me.Controls.Add(Me.streamToDiskButton)
        Me.Controls.Add(Me.rfsaResourceNameLabel)
        Me.Controls.Add(Me.referenceLevelLabel)
        Me.Controls.Add(Me.carrierFrequencyLabel)
        Me.Controls.Add(Me.iqRateLabel)
        Me.Controls.Add(Me.samplesPerBlockLabel)
        Me.Controls.Add(Me.maxSamplesLabel)
        Me.Controls.Add(Me.fileNameLabel)
        Me.Controls.Add(Me.dtLabel)
        Me.Controls.Add(Me.gainLabel)
        Me.Controls.Add(Me.offsetLabel)
        Me.Controls.Add(Me.samplesSoFarLabel)
        Me.Controls.Add(Me.referenceLevelNumeric)
        Me.Controls.Add(Me.carrierFrequencyNumeric)
        Me.Controls.Add(Me.iqRateNumeric)
        Me.Controls.Add(Me.samplesPerBlockNumeric)
        Me.Controls.Add(Me.maxSamplesNumeric)
        Me.Controls.Add(Me.fileNameTextBox)
        Me.Controls.Add(Me.dtTextBox)
        Me.Controls.Add(Me.gainTextBox)
        Me.Controls.Add(Me.offsetTextBox)
        Me.Controls.Add(Me.samplesSoFarTextBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "RFSA Stream Binary IQ Data to Disk"
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.carrierFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerBlockNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maxSamplesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private referenceLevelLabel As System.Windows.Forms.Label
    Private carrierFrequencyLabel As System.Windows.Forms.Label
    Private iqRateLabel As System.Windows.Forms.Label
    Private samplesPerBlockLabel As System.Windows.Forms.Label
    Private maxSamplesLabel As System.Windows.Forms.Label
    Private fileNameLabel As System.Windows.Forms.Label
    Private dtLabel As System.Windows.Forms.Label
    Private gainLabel As System.Windows.Forms.Label
    Private offsetLabel As System.Windows.Forms.Label
    Private samplesSoFarLabel As System.Windows.Forms.Label
    Private referenceLevelNumeric As System.Windows.Forms.NumericUpDown
    Private carrierFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private samplesPerBlockNumeric As System.Windows.Forms.NumericUpDown
    Private maxSamplesNumeric As System.Windows.Forms.NumericUpDown
    Private fileNameTextBox As System.Windows.Forms.TextBox
    Private dtTextBox As System.Windows.Forms.TextBox
    Private gainTextBox As System.Windows.Forms.TextBox
    Private offsetTextBox As System.Windows.Forms.TextBox
    Private samplesSoFarTextBox As System.Windows.Forms.TextBox
    Private rfsaResourceNameLabel As System.Windows.Forms.Label
    Private WithEvents streamToDiskButton As System.Windows.Forms.Button
    Private WithEvents readFromDiskButton As System.Windows.Forms.Button
    Private dataGridViewResults As System.Windows.Forms.DataGridView
    Private resourceNameComboBox As System.Windows.Forms.ComboBox

End Class
