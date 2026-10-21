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
        Me.centerFrequencyLabel = New System.Windows.Forms.Label()
        Me.downconverterCenterFrequencyLabel = New System.Windows.Forms.Label()
        Me.spanLabel = New System.Windows.Forms.Label()
        Me.loFrequencyLabel = New System.Windows.Forms.Label()
        Me.resolutionBandwidthLabel = New System.Windows.Forms.Label()
        Me.referenceLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.centerFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.downconverterCenterFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.spanNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resolutionBandwidthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.loFrequencyTextBox = New System.Windows.Forms.TextBox()
        Me.rfsaResourceNameLabel = New System.Windows.Forms.Label()
        Me.loResourceNamelabel = New System.Windows.Forms.Label()
        Me.readPowerSpectrumButton = New System.Windows.Forms.Button()
        Me.dataGridViewResults = New System.Windows.Forms.DataGridView()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.loResourceNameComboBox = New System.Windows.Forms.ComboBox()
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.centerFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.downconverterCenterFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.spanNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.resolutionBandwidthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'referenceLevelLabel
        '
        Me.referenceLevelLabel.AutoSize = True
        Me.referenceLevelLabel.Location = New System.Drawing.Point(10, 102)
        Me.referenceLevelLabel.Name = "referenceLevelLabel"
        Me.referenceLevelLabel.Size = New System.Drawing.Size(116, 13)
        Me.referenceLevelLabel.TabIndex = 0
        Me.referenceLevelLabel.Text = "Reference Level (dBm)"
        '
        'centerFrequencyLabel
        '
        Me.centerFrequencyLabel.AutoSize = True
        Me.centerFrequencyLabel.Location = New System.Drawing.Point(10, 152)
        Me.centerFrequencyLabel.Name = "centerFrequencyLabel"
        Me.centerFrequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.centerFrequencyLabel.TabIndex = 1
        Me.centerFrequencyLabel.Text = "Center Frequency (Hz)"
        '
        'downconverterCenterFrequencyLabel
        '
        Me.downconverterCenterFrequencyLabel.AutoSize = True
        Me.downconverterCenterFrequencyLabel.Location = New System.Drawing.Point(10, 289)
        Me.downconverterCenterFrequencyLabel.Name = "downconverterCenterFrequencyLabel"
        Me.downconverterCenterFrequencyLabel.Size = New System.Drawing.Size(155, 13)
        Me.downconverterCenterFrequencyLabel.TabIndex = 2
        Me.downconverterCenterFrequencyLabel.Text = "Downconverter Frequency (Hz)"
        '
        'spanLabel
        '
        Me.spanLabel.AutoSize = True
        Me.spanLabel.Location = New System.Drawing.Point(10, 199)
        Me.spanLabel.Name = "spanLabel"
        Me.spanLabel.Size = New System.Drawing.Size(54, 13)
        Me.spanLabel.TabIndex = 3
        Me.spanLabel.Text = "Span (Hz)"
        '
        'loFrequencyLabel
        '
        Me.loFrequencyLabel.AutoSize = True
        Me.loFrequencyLabel.Location = New System.Drawing.Point(264, 354)
        Me.loFrequencyLabel.Name = "loFrequencyLabel"
        Me.loFrequencyLabel.Size = New System.Drawing.Size(93, 13)
        Me.loFrequencyLabel.TabIndex = 4
        Me.loFrequencyLabel.Text = "LO Frequency(Hz)"
        '
        'resolutionBandwidthLabel
        '
        Me.resolutionBandwidthLabel.AutoSize = True
        Me.resolutionBandwidthLabel.Location = New System.Drawing.Point(10, 246)
        Me.resolutionBandwidthLabel.Name = "resolutionBandwidthLabel"
        Me.resolutionBandwidthLabel.Size = New System.Drawing.Size(132, 13)
        Me.resolutionBandwidthLabel.TabIndex = 5
        Me.resolutionBandwidthLabel.Text = "Resolution Bandwidth (Hz)"
        '
        'referenceLevelNumeric
        '
        Me.referenceLevelNumeric.DecimalPlaces = 2
        Me.referenceLevelNumeric.Location = New System.Drawing.Point(10, 124)
        Me.referenceLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.referenceLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.referenceLevelNumeric.Name = "referenceLevelNumeric"
        Me.referenceLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.referenceLevelNumeric.TabIndex = 2
        '
        'centerFrequencyNumeric
        '
        Me.centerFrequencyNumeric.DecimalPlaces = 2
        Me.centerFrequencyNumeric.Location = New System.Drawing.Point(10, 173)
        Me.centerFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.centerFrequencyNumeric.Name = "centerFrequencyNumeric"
        Me.centerFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.centerFrequencyNumeric.TabIndex = 3
        Me.centerFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'downconverterCenterFrequencyNumeric
        '
        Me.downconverterCenterFrequencyNumeric.DecimalPlaces = 2
        Me.downconverterCenterFrequencyNumeric.Location = New System.Drawing.Point(10, 310)
        Me.downconverterCenterFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.downconverterCenterFrequencyNumeric.Name = "downconverterCenterFrequencyNumeric"
        Me.downconverterCenterFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.downconverterCenterFrequencyNumeric.TabIndex = 6
        Me.downconverterCenterFrequencyNumeric.Value = New Decimal(New Integer() {1010000000, 0, 0, 0})
        '
        'spanNumeric
        '
        Me.spanNumeric.DecimalPlaces = 2
        Me.spanNumeric.Location = New System.Drawing.Point(10, 219)
        Me.spanNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.spanNumeric.Name = "spanNumeric"
        Me.spanNumeric.Size = New System.Drawing.Size(120, 20)
        Me.spanNumeric.TabIndex = 4
        Me.spanNumeric.Value = New Decimal(New Integer() {10000000, 0, 0, 0})
        '
        'resolutionBandwidthNumeric
        '
        Me.resolutionBandwidthNumeric.DecimalPlaces = 2
        Me.resolutionBandwidthNumeric.Location = New System.Drawing.Point(10, 264)
        Me.resolutionBandwidthNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.resolutionBandwidthNumeric.Name = "resolutionBandwidthNumeric"
        Me.resolutionBandwidthNumeric.Size = New System.Drawing.Size(120, 20)
        Me.resolutionBandwidthNumeric.TabIndex = 5
        Me.resolutionBandwidthNumeric.Value = New Decimal(New Integer() {10000, 0, 0, 0})
        '
        'loFrequencyTextBox
        '
        Me.loFrequencyTextBox.Location = New System.Drawing.Point(363, 351)
        Me.loFrequencyTextBox.Name = "loFrequencyTextBox"
        Me.loFrequencyTextBox.ReadOnly = True
        Me.loFrequencyTextBox.Size = New System.Drawing.Size(116, 20)
        Me.loFrequencyTextBox.TabIndex = 11
        Me.loFrequencyTextBox.Text = "0.00000000000000E+0"
        '
        'rfsaResourceNameLabel
        '
        Me.rfsaResourceNameLabel.AutoSize = True
        Me.rfsaResourceNameLabel.Location = New System.Drawing.Point(10, 16)
        Me.rfsaResourceNameLabel.Name = "rfsaResourceNameLabel"
        Me.rfsaResourceNameLabel.Size = New System.Drawing.Size(115, 13)
        Me.rfsaResourceNameLabel.TabIndex = 8
        Me.rfsaResourceNameLabel.Text = "RFSA Resource Name"
        '
        'loResourceNamelabel
        '
        Me.loResourceNamelabel.AutoSize = True
        Me.loResourceNamelabel.Location = New System.Drawing.Point(10, 62)
        Me.loResourceNamelabel.Name = "loResourceNamelabel"
        Me.loResourceNamelabel.Size = New System.Drawing.Size(101, 13)
        Me.loResourceNamelabel.TabIndex = 10
        Me.loResourceNamelabel.Text = "LO Resource Name"
        '
        'readPowerSpectrumButton
        '
        Me.readPowerSpectrumButton.Location = New System.Drawing.Point(10, 347)
        Me.readPowerSpectrumButton.Name = "readPowerSpectrumButton"
        Me.readPowerSpectrumButton.Size = New System.Drawing.Size(134, 26)
        Me.readPowerSpectrumButton.TabIndex = 8
        Me.readPowerSpectrumButton.Text = "&ReadPowerSpectrum"
        Me.readPowerSpectrumButton.UseVisualStyleBackColor = True
        '
        'dataGridViewResults
        '
        Me.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dataGridViewResults.Location = New System.Drawing.Point(171, 16)
        Me.dataGridViewResults.Name = "dataGridViewResults"
        Me.dataGridViewResults.Size = New System.Drawing.Size(447, 314)
        Me.dataGridViewResults.TabIndex = 10
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(10, 32)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'loResourceNameComboBox
        '
        Me.loResourceNameComboBox.FormattingEnabled = True
        Me.loResourceNameComboBox.Location = New System.Drawing.Point(10, 78)
        Me.loResourceNameComboBox.Name = "loResourceNameComboBox"
        Me.loResourceNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.loResourceNameComboBox.TabIndex = 1
        '
        'MainForm
        '
        Me.AcceptButton = Me.readPowerSpectrumButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(630, 385)
        Me.Controls.Add(Me.loResourceNameComboBox)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.dataGridViewResults)
        Me.Controls.Add(Me.readPowerSpectrumButton)
        Me.Controls.Add(Me.loResourceNamelabel)
        Me.Controls.Add(Me.rfsaResourceNameLabel)
        Me.Controls.Add(Me.referenceLevelLabel)
        Me.Controls.Add(Me.centerFrequencyLabel)
        Me.Controls.Add(Me.downconverterCenterFrequencyLabel)
        Me.Controls.Add(Me.spanLabel)
        Me.Controls.Add(Me.loFrequencyLabel)
        Me.Controls.Add(Me.resolutionBandwidthLabel)
        Me.Controls.Add(Me.referenceLevelNumeric)
        Me.Controls.Add(Me.centerFrequencyNumeric)
        Me.Controls.Add(Me.downconverterCenterFrequencyNumeric)
        Me.Controls.Add(Me.spanNumeric)
        Me.Controls.Add(Me.resolutionBandwidthNumeric)
        Me.Controls.Add(Me.loFrequencyTextBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "RFSA In-band Retuning With External LO"
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

    Private referenceLevelLabel As System.Windows.Forms.Label
    Private centerFrequencyLabel As System.Windows.Forms.Label
    Private downconverterCenterFrequencyLabel As System.Windows.Forms.Label
    Private spanLabel As System.Windows.Forms.Label
    Private loFrequencyLabel As System.Windows.Forms.Label
    Private resolutionBandwidthLabel As System.Windows.Forms.Label
    Private referenceLevelNumeric As System.Windows.Forms.NumericUpDown
    Private centerFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private downconverterCenterFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private spanNumeric As System.Windows.Forms.NumericUpDown
    Private resolutionBandwidthNumeric As System.Windows.Forms.NumericUpDown
    Private loFrequencyTextBox As System.Windows.Forms.TextBox
    Private rfsaResourceNameLabel As System.Windows.Forms.Label
    Private loResourceNamelabel As System.Windows.Forms.Label
    Private WithEvents readPowerSpectrumButton As System.Windows.Forms.Button
    Private dataGridViewResults As System.Windows.Forms.DataGridView
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private loResourceNameComboBox As System.Windows.Forms.ComboBox

End Class
