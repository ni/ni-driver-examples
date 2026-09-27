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
        Me.referenceClockLabel = New System.Windows.Forms.Label()
        Me.referenceLevelLabel = New System.Windows.Forms.Label()
        Me.centerFrequencyLabel = New System.Windows.Forms.Label()
        Me.downconverterCenterFrequencyLabel = New System.Windows.Forms.Label()
        Me.spanLabel = New System.Windows.Forms.Label()
        Me.resolutionBandwidthLabel = New System.Windows.Forms.Label()
        Me.referenceLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.centerFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.downconverterCenterFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.spanNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resolutionBandwidthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.referenceClockComboBox = New System.Windows.Forms.ComboBox()
        Me.rfsaResourceNameLabel = New System.Windows.Forms.Label()
        Me.readPowerSpectrumButton = New System.Windows.Forms.Button()
        Me.dataGridViewResults = New System.Windows.Forms.DataGridView()
        Me.rfsaResourceNameComboBox = New System.Windows.Forms.ComboBox()
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.centerFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.downconverterCenterFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.spanNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.resolutionBandwidthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'referenceClockLabel
        '
        Me.referenceClockLabel.Location = New System.Drawing.Point(12, 47)
        Me.referenceClockLabel.Name = "referenceClockLabel"
        Me.referenceClockLabel.Size = New System.Drawing.Size(109, 15)
        Me.referenceClockLabel.TabIndex = 0
        Me.referenceClockLabel.Text = "Reference Clock"
        '
        'referenceLevelLabel
        '
        Me.referenceLevelLabel.Location = New System.Drawing.Point(12, 89)
        Me.referenceLevelLabel.Name = "referenceLevelLabel"
        Me.referenceLevelLabel.Size = New System.Drawing.Size(109, 15)
        Me.referenceLevelLabel.TabIndex = 1
        Me.referenceLevelLabel.Text = "Reference Level (dBm)"
        '
        'centerFrequencyLabel
        '
        Me.centerFrequencyLabel.AutoSize = True
        Me.centerFrequencyLabel.Location = New System.Drawing.Point(12, 135)
        Me.centerFrequencyLabel.Name = "centerFrequencyLabel"
        Me.centerFrequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.centerFrequencyLabel.TabIndex = 2
        Me.centerFrequencyLabel.Text = "Center Frequency (Hz)"
        '
        'downconverterCenterFrequencyLabel
        '
        Me.downconverterCenterFrequencyLabel.Location = New System.Drawing.Point(12, 258)
        Me.downconverterCenterFrequencyLabel.Name = "downconverterCenterFrequencyLabel"
        Me.downconverterCenterFrequencyLabel.Size = New System.Drawing.Size(109, 15)
        Me.downconverterCenterFrequencyLabel.TabIndex = 3
        Me.downconverterCenterFrequencyLabel.Text = "Downconverter Frequency (Hz)"
        '
        'spanLabel
        '
        Me.spanLabel.Location = New System.Drawing.Point(12, 176)
        Me.spanLabel.Name = "spanLabel"
        Me.spanLabel.Size = New System.Drawing.Size(109, 15)
        Me.spanLabel.TabIndex = 4
        Me.spanLabel.Text = "Span (Hz)"
        '
        'resolutionBandwidthLabel
        '
        Me.resolutionBandwidthLabel.Location = New System.Drawing.Point(12, 216)
        Me.resolutionBandwidthLabel.Name = "resolutionBandwidthLabel"
        Me.resolutionBandwidthLabel.Size = New System.Drawing.Size(121, 15)
        Me.resolutionBandwidthLabel.TabIndex = 5
        Me.resolutionBandwidthLabel.Text = "Resolution Bandwidth (Hz)"
        '
        'referenceLevelNumeric
        '
        Me.referenceLevelNumeric.DecimalPlaces = 2
        Me.referenceLevelNumeric.Location = New System.Drawing.Point(12, 108)
        Me.referenceLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.referenceLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.referenceLevelNumeric.Name = "referenceLevelNumeric"
        Me.referenceLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.referenceLevelNumeric.TabIndex = 2
        '
        'centerFrequencyNumeric
        '
        Me.centerFrequencyNumeric.DecimalPlaces = 2
        Me.centerFrequencyNumeric.Location = New System.Drawing.Point(12, 152)
        Me.centerFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.centerFrequencyNumeric.Name = "centerFrequencyNumeric"
        Me.centerFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.centerFrequencyNumeric.TabIndex = 3
        Me.centerFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'downconverterCenterFrequencyNumeric
        '
        Me.downconverterCenterFrequencyNumeric.DecimalPlaces = 2
        Me.downconverterCenterFrequencyNumeric.Location = New System.Drawing.Point(12, 276)
        Me.downconverterCenterFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.downconverterCenterFrequencyNumeric.Name = "downconverterCenterFrequencyNumeric"
        Me.downconverterCenterFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.downconverterCenterFrequencyNumeric.TabIndex = 6
        Me.downconverterCenterFrequencyNumeric.Value = New Decimal(New Integer() {1010000000, 0, 0, 0})
        '
        'spanNumeric
        '
        Me.spanNumeric.DecimalPlaces = 2
        Me.spanNumeric.Location = New System.Drawing.Point(12, 193)
        Me.spanNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.spanNumeric.Name = "spanNumeric"
        Me.spanNumeric.Size = New System.Drawing.Size(120, 20)
        Me.spanNumeric.TabIndex = 4
        Me.spanNumeric.Value = New Decimal(New Integer() {10000000, 0, 0, 0})
        '
        'resolutionBandwidthNumeric
        '
        Me.resolutionBandwidthNumeric.DecimalPlaces = 2
        Me.resolutionBandwidthNumeric.Location = New System.Drawing.Point(12, 234)
        Me.resolutionBandwidthNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.resolutionBandwidthNumeric.Name = "resolutionBandwidthNumeric"
        Me.resolutionBandwidthNumeric.Size = New System.Drawing.Size(120, 20)
        Me.resolutionBandwidthNumeric.TabIndex = 5
        Me.resolutionBandwidthNumeric.Value = New Decimal(New Integer() {10000, 0, 0, 0})
        '
        'referenceClockComboBox
        '
        Me.referenceClockComboBox.Location = New System.Drawing.Point(12, 65)
        Me.referenceClockComboBox.Name = "referenceClockComboBox"
        Me.referenceClockComboBox.Size = New System.Drawing.Size(120, 21)
        Me.referenceClockComboBox.TabIndex = 1
        '
        'rfsaResourceNameLabel
        '
        Me.rfsaResourceNameLabel.AutoSize = True
        Me.rfsaResourceNameLabel.Location = New System.Drawing.Point(12, 9)
        Me.rfsaResourceNameLabel.Name = "rfsaResourceNameLabel"
        Me.rfsaResourceNameLabel.Size = New System.Drawing.Size(115, 13)
        Me.rfsaResourceNameLabel.TabIndex = 10
        Me.rfsaResourceNameLabel.Text = "RFSA Resource Name"
        '
        'readPowerSpectrumButton
        '
        Me.readPowerSpectrumButton.Location = New System.Drawing.Point(12, 312)
        Me.readPowerSpectrumButton.Name = "readPowerSpectrumButton"
        Me.readPowerSpectrumButton.Size = New System.Drawing.Size(134, 26)
        Me.readPowerSpectrumButton.TabIndex = 7
        Me.readPowerSpectrumButton.Text = "&Read Power Spectrum"
        Me.readPowerSpectrumButton.UseVisualStyleBackColor = True
        '
        'dataGridViewResults
        '
        Me.dataGridViewResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
        Me.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dataGridViewResults.Location = New System.Drawing.Point(159, 12)
        Me.dataGridViewResults.Name = "dataGridViewResults"
        Me.dataGridViewResults.Size = New System.Drawing.Size(174, 284)
        Me.dataGridViewResults.TabIndex = 16
        '
        'rfsaResourceNameComboBox
        '
        Me.rfsaResourceNameComboBox.FormattingEnabled = True
        Me.rfsaResourceNameComboBox.Location = New System.Drawing.Point(12, 24)
        Me.rfsaResourceNameComboBox.Name = "rfsaResourceNameComboBox"
        Me.rfsaResourceNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.rfsaResourceNameComboBox.TabIndex = 0
        '
        'MainForm
        '
        Me.AcceptButton = Me.readPowerSpectrumButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(341, 349)
        Me.Controls.Add(Me.rfsaResourceNameComboBox)
        Me.Controls.Add(Me.dataGridViewResults)
        Me.Controls.Add(Me.readPowerSpectrumButton)
        Me.Controls.Add(Me.rfsaResourceNameLabel)
        Me.Controls.Add(Me.referenceClockLabel)
        Me.Controls.Add(Me.referenceLevelLabel)
        Me.Controls.Add(Me.centerFrequencyLabel)
        Me.Controls.Add(Me.downconverterCenterFrequencyLabel)
        Me.Controls.Add(Me.spanLabel)
        Me.Controls.Add(Me.resolutionBandwidthLabel)
        Me.Controls.Add(Me.referenceLevelNumeric)
        Me.Controls.Add(Me.centerFrequencyNumeric)
        Me.Controls.Add(Me.downconverterCenterFrequencyNumeric)
        Me.Controls.Add(Me.spanNumeric)
        Me.Controls.Add(Me.resolutionBandwidthNumeric)
        Me.Controls.Add(Me.referenceClockComboBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "RFSA In-band Retuning With Internal LO"
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.centerFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.downconverterCenterFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.spanNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.resolutionBandwidthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private referenceClockLabel As System.Windows.Forms.Label
    Private referenceLevelLabel As System.Windows.Forms.Label
    Private centerFrequencyLabel As System.Windows.Forms.Label
    Private downconverterCenterFrequencyLabel As System.Windows.Forms.Label
    Private spanLabel As System.Windows.Forms.Label
    Private resolutionBandwidthLabel As System.Windows.Forms.Label
    Private referenceLevelNumeric As System.Windows.Forms.NumericUpDown
    Private centerFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private downconverterCenterFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private spanNumeric As System.Windows.Forms.NumericUpDown
    Private resolutionBandwidthNumeric As System.Windows.Forms.NumericUpDown
    Private referenceClockComboBox As System.Windows.Forms.ComboBox
    Private rfsaResourceNameLabel As System.Windows.Forms.Label
    Private WithEvents readPowerSpectrumButton As System.Windows.Forms.Button
    Private dataGridViewResults As System.Windows.Forms.DataGridView
    Private rfsaResourceNameComboBox As System.Windows.Forms.ComboBox

End Class
