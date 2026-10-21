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
        Me.startFrequencyLabel = New System.Windows.Forms.Label()
        Me.stopFrequencyLabel = New System.Windows.Forms.Label()
        Me.resolutionBandwidthLabel = New System.Windows.Forms.Label()
        Me.textmsgLabel = New System.Windows.Forms.Label()
        Me.referenceLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.stopFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resolutionBandwidthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.referenceClockComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.dataGridViewResults = New System.Windows.Forms.DataGridView()
        Me.readPowerSpectrumButton = New System.Windows.Forms.Button()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.stopFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.resolutionBandwidthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'referenceClockLabel
        '
        Me.referenceClockLabel.AutoSize = True
        Me.referenceClockLabel.Location = New System.Drawing.Point(9, 71)
        Me.referenceClockLabel.Name = "referenceClockLabel"
        Me.referenceClockLabel.Size = New System.Drawing.Size(87, 13)
        Me.referenceClockLabel.TabIndex = 0
        Me.referenceClockLabel.Text = "Reference Clock"
        '
        'referenceLevelLabel
        '
        Me.referenceLevelLabel.AutoSize = True
        Me.referenceLevelLabel.Location = New System.Drawing.Point(9, 123)
        Me.referenceLevelLabel.Name = "referenceLevelLabel"
        Me.referenceLevelLabel.Size = New System.Drawing.Size(116, 13)
        Me.referenceLevelLabel.TabIndex = 1
        Me.referenceLevelLabel.Text = "Reference Level (dBm)"
        '
        'startFrequencyLabel
        '
        Me.startFrequencyLabel.AutoSize = True
        Me.startFrequencyLabel.Location = New System.Drawing.Point(9, 173)
        Me.startFrequencyLabel.Name = "startFrequencyLabel"
        Me.startFrequencyLabel.Size = New System.Drawing.Size(104, 13)
        Me.startFrequencyLabel.TabIndex = 2
        Me.startFrequencyLabel.Text = "Start Frequency (Hz)"
        '
        'stopFrequencyLabel
        '
        Me.stopFrequencyLabel.AutoSize = True
        Me.stopFrequencyLabel.Location = New System.Drawing.Point(9, 223)
        Me.stopFrequencyLabel.Name = "stopFrequencyLabel"
        Me.stopFrequencyLabel.Size = New System.Drawing.Size(104, 13)
        Me.stopFrequencyLabel.TabIndex = 3
        Me.stopFrequencyLabel.Text = "Stop Frequency (Hz)"
        '
        'resolutionBandwidthLabel
        '
        Me.resolutionBandwidthLabel.AutoSize = True
        Me.resolutionBandwidthLabel.Location = New System.Drawing.Point(9, 273)
        Me.resolutionBandwidthLabel.Name = "resolutionBandwidthLabel"
        Me.resolutionBandwidthLabel.Size = New System.Drawing.Size(132, 13)
        Me.resolutionBandwidthLabel.TabIndex = 4
        Me.resolutionBandwidthLabel.Text = "Resolution Bandwidth (Hz)"
        '
        'textmsgLabel
        '
        Me.textmsgLabel.Location = New System.Drawing.Point(0, 0)
        Me.textmsgLabel.Name = "textmsgLabel"
        Me.textmsgLabel.Size = New System.Drawing.Size(100, 23)
        Me.textmsgLabel.TabIndex = 5
        '
        'referenceLevelNumeric
        '
        Me.referenceLevelNumeric.DecimalPlaces = 2
        Me.referenceLevelNumeric.Location = New System.Drawing.Point(12, 139)
        Me.referenceLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.referenceLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.referenceLevelNumeric.Name = "referenceLevelNumeric"
        Me.referenceLevelNumeric.Size = New System.Drawing.Size(116, 20)
        Me.referenceLevelNumeric.TabIndex = 2
        '
        'startFrequencyNumeric
        '
        Me.startFrequencyNumeric.DecimalPlaces = 2
        Me.startFrequencyNumeric.Location = New System.Drawing.Point(12, 189)
        Me.startFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.startFrequencyNumeric.Name = "startFrequencyNumeric"
        Me.startFrequencyNumeric.Size = New System.Drawing.Size(116, 20)
        Me.startFrequencyNumeric.TabIndex = 3
        Me.startFrequencyNumeric.Value = New Decimal(New Integer() {990000000, 0, 0, 0})
        '
        'stopFrequencyNumeric
        '
        Me.stopFrequencyNumeric.DecimalPlaces = 2
        Me.stopFrequencyNumeric.Location = New System.Drawing.Point(12, 239)
        Me.stopFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.stopFrequencyNumeric.Name = "stopFrequencyNumeric"
        Me.stopFrequencyNumeric.Size = New System.Drawing.Size(116, 20)
        Me.stopFrequencyNumeric.TabIndex = 4
        Me.stopFrequencyNumeric.Value = New Decimal(New Integer() {1010000000, 0, 0, 0})
        '
        'resolutionBandwidthNumeric
        '
        Me.resolutionBandwidthNumeric.DecimalPlaces = 2
        Me.resolutionBandwidthNumeric.Location = New System.Drawing.Point(12, 289)
        Me.resolutionBandwidthNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.resolutionBandwidthNumeric.Name = "resolutionBandwidthNumeric"
        Me.resolutionBandwidthNumeric.Size = New System.Drawing.Size(116, 20)
        Me.resolutionBandwidthNumeric.TabIndex = 5
        Me.resolutionBandwidthNumeric.Value = New Decimal(New Integer() {10000, 0, 0, 0})
        '
        'referenceClockComboBox
        '
        Me.referenceClockComboBox.Location = New System.Drawing.Point(12, 87)
        Me.referenceClockComboBox.Name = "referenceClockComboBox"
        Me.referenceClockComboBox.Size = New System.Drawing.Size(116, 21)
        Me.referenceClockComboBox.TabIndex = 1
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(9, 16)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 6
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'dataGridViewResults
        '
        Me.dataGridViewResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
        Me.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dataGridViewResults.Location = New System.Drawing.Point(147, 16)
        Me.dataGridViewResults.Name = "dataGridViewResults"
        Me.dataGridViewResults.Size = New System.Drawing.Size(174, 293)
        Me.dataGridViewResults.TabIndex = 9
        '
        'readPowerSpectrumButton
        '
        Me.readPowerSpectrumButton.Location = New System.Drawing.Point(12, 324)
        Me.readPowerSpectrumButton.Name = "readPowerSpectrumButton"
        Me.readPowerSpectrumButton.Size = New System.Drawing.Size(129, 23)
        Me.readPowerSpectrumButton.TabIndex = 6
        Me.readPowerSpectrumButton.Text = "&Read Power Spectrum"
        Me.readPowerSpectrumButton.UseVisualStyleBackColor = True
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 32)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'MainForm
        '
        Me.AcceptButton = Me.readPowerSpectrumButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(327, 355)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.readPowerSpectrumButton)
        Me.Controls.Add(Me.dataGridViewResults)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.referenceClockLabel)
        Me.Controls.Add(Me.referenceLevelLabel)
        Me.Controls.Add(Me.startFrequencyLabel)
        Me.Controls.Add(Me.stopFrequencyLabel)
        Me.Controls.Add(Me.resolutionBandwidthLabel)
        Me.Controls.Add(Me.textmsgLabel)
        Me.Controls.Add(Me.referenceLevelNumeric)
        Me.Controls.Add(Me.startFrequencyNumeric)
        Me.Controls.Add(Me.stopFrequencyNumeric)
        Me.Controls.Add(Me.resolutionBandwidthNumeric)
        Me.Controls.Add(Me.referenceClockComboBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "RFSA Getting Started Spectrum"
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.stopFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.resolutionBandwidthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private referenceClockLabel As System.Windows.Forms.Label
    Private referenceLevelLabel As System.Windows.Forms.Label
    Private startFrequencyLabel As System.Windows.Forms.Label
    Private stopFrequencyLabel As System.Windows.Forms.Label
    Private resolutionBandwidthLabel As System.Windows.Forms.Label
    Private textmsgLabel As System.Windows.Forms.Label
    Private referenceLevelNumeric As System.Windows.Forms.NumericUpDown
    Private startFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private stopFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private resolutionBandwidthNumeric As System.Windows.Forms.NumericUpDown
    Private referenceClockComboBox As System.Windows.Forms.ComboBox
    Private resourceNameLabel As System.Windows.Forms.Label
    Private dataGridViewResults As System.Windows.Forms.DataGridView
    Private WithEvents readPowerSpectrumButton As System.Windows.Forms.Button
    Private resourceNameComboBox As System.Windows.Forms.ComboBox

End Class
