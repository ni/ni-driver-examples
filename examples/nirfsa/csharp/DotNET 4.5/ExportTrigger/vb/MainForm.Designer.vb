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
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.referenceLevelLabel = New System.Windows.Forms.Label()
        Me.carrierFrequencyLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.samplesPerRecordLabel = New System.Windows.Forms.Label()
        Me.referenceLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.carrierFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.samplesPerRecordNumeric = New System.Windows.Forms.NumericUpDown()
        Me.outputTerminalLabel = New System.Windows.Forms.Label()
        Me.outputTerminalComboBox = New System.Windows.Forms.ComboBox()
        Me.startAcquisitionButton = New System.Windows.Forms.Button()
        Me.dataGridView1 = New System.Windows.Forms.DataGridView()
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.carrierFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerRecordNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 30)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(110, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(15, 10)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 20
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'referenceLevelLabel
        '
        Me.referenceLevelLabel.AutoSize = True
        Me.referenceLevelLabel.Location = New System.Drawing.Point(12, 56)
        Me.referenceLevelLabel.Name = "referenceLevelLabel"
        Me.referenceLevelLabel.Size = New System.Drawing.Size(116, 13)
        Me.referenceLevelLabel.TabIndex = 11
        Me.referenceLevelLabel.Text = "Reference Level (dBm)"
        '
        'carrierFrequencyLabel
        '
        Me.carrierFrequencyLabel.AutoSize = True
        Me.carrierFrequencyLabel.Location = New System.Drawing.Point(12, 101)
        Me.carrierFrequencyLabel.Name = "carrierFrequencyLabel"
        Me.carrierFrequencyLabel.Size = New System.Drawing.Size(112, 13)
        Me.carrierFrequencyLabel.TabIndex = 13
        Me.carrierFrequencyLabel.Text = "Carrier Frequency (Hz)"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(12, 147)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(44, 13)
        Me.iqRateLabel.TabIndex = 15
        Me.iqRateLabel.Text = "IQ Rate"
        '
        'samplesPerRecordLabel
        '
        Me.samplesPerRecordLabel.AutoSize = True
        Me.samplesPerRecordLabel.Location = New System.Drawing.Point(12, 193)
        Me.samplesPerRecordLabel.Name = "samplesPerRecordLabel"
        Me.samplesPerRecordLabel.Size = New System.Drawing.Size(103, 13)
        Me.samplesPerRecordLabel.TabIndex = 16
        Me.samplesPerRecordLabel.Text = "Samples per Record"
        '
        'referenceLevelNumeric
        '
        Me.referenceLevelNumeric.DecimalPlaces = 2
        Me.referenceLevelNumeric.Location = New System.Drawing.Point(12, 75)
        Me.referenceLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.referenceLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.referenceLevelNumeric.Name = "referenceLevelNumeric"
        Me.referenceLevelNumeric.Size = New System.Drawing.Size(116, 20)
        Me.referenceLevelNumeric.TabIndex = 1
        '
        'carrierFrequencyNumeric
        '
        Me.carrierFrequencyNumeric.DecimalPlaces = 2
        Me.carrierFrequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.carrierFrequencyNumeric.Location = New System.Drawing.Point(12, 121)
        Me.carrierFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.carrierFrequencyNumeric.Name = "carrierFrequencyNumeric"
        Me.carrierFrequencyNumeric.Size = New System.Drawing.Size(116, 20)
        Me.carrierFrequencyNumeric.TabIndex = 2
        Me.carrierFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 2
        Me.iqRateNumeric.Location = New System.Drawing.Point(12, 168)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(116, 20)
        Me.iqRateNumeric.TabIndex = 3
        Me.iqRateNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'samplesPerRecordNumeric
        '
        Me.samplesPerRecordNumeric.Location = New System.Drawing.Point(12, 215)
        Me.samplesPerRecordNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.samplesPerRecordNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerRecordNumeric.Name = "samplesPerRecordNumeric"
        Me.samplesPerRecordNumeric.Size = New System.Drawing.Size(116, 20)
        Me.samplesPerRecordNumeric.TabIndex = 4
        Me.samplesPerRecordNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'outputTerminalLabel
        '
        Me.outputTerminalLabel.AutoSize = True
        Me.outputTerminalLabel.Location = New System.Drawing.Point(12, 249)
        Me.outputTerminalLabel.Name = "outputTerminalLabel"
        Me.outputTerminalLabel.Size = New System.Drawing.Size(82, 13)
        Me.outputTerminalLabel.TabIndex = 22
        Me.outputTerminalLabel.Text = "Output Terminal"
        '
        'outputTerminalComboBox
        '
        Me.outputTerminalComboBox.Location = New System.Drawing.Point(12, 267)
        Me.outputTerminalComboBox.Name = "outputTerminalComboBox"
        Me.outputTerminalComboBox.Size = New System.Drawing.Size(116, 21)
        Me.outputTerminalComboBox.TabIndex = 5
        '
        'startAcquisitionButton
        '
        Me.startAcquisitionButton.Location = New System.Drawing.Point(12, 316)
        Me.startAcquisitionButton.Name = "startAcquisitionButton"
        Me.startAcquisitionButton.Size = New System.Drawing.Size(116, 30)
        Me.startAcquisitionButton.TabIndex = 6
        Me.startAcquisitionButton.Text = "&Start Acquisition"
        Me.startAcquisitionButton.UseVisualStyleBackColor = True
        '
        'dataGridView1
        '
        Me.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dataGridView1.Location = New System.Drawing.Point(134, 12)
        Me.dataGridView1.Name = "dataGridView1"
        Me.dataGridView1.Size = New System.Drawing.Size(362, 334)
        Me.dataGridView1.TabIndex = 25
        '
        'MainForm
        '
        Me.AcceptButton = Me.startAcquisitionButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(505, 355)
        Me.Controls.Add(Me.dataGridView1)
        Me.Controls.Add(Me.startAcquisitionButton)
        Me.Controls.Add(Me.outputTerminalLabel)
        Me.Controls.Add(Me.outputTerminalComboBox)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.referenceLevelLabel)
        Me.Controls.Add(Me.carrierFrequencyLabel)
        Me.Controls.Add(Me.iqRateLabel)
        Me.Controls.Add(Me.samplesPerRecordLabel)
        Me.Controls.Add(Me.referenceLevelNumeric)
        Me.Controls.Add(Me.carrierFrequencyNumeric)
        Me.Controls.Add(Me.iqRateNumeric)
        Me.Controls.Add(Me.samplesPerRecordNumeric)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Rfsa Export Trigger"
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.carrierFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerRecordNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private resourceNameLabel As System.Windows.Forms.Label
    Private referenceLevelLabel As System.Windows.Forms.Label
    Private carrierFrequencyLabel As System.Windows.Forms.Label
    Private iqRateLabel As System.Windows.Forms.Label
    Private samplesPerRecordLabel As System.Windows.Forms.Label
    Private referenceLevelNumeric As System.Windows.Forms.NumericUpDown
	Private carrierFrequencyNumeric As System.Windows.Forms.NumericUpDown
	Private iqRateNumeric As System.Windows.Forms.NumericUpDown
	Private samplesPerRecordNumeric As System.Windows.Forms.NumericUpDown
	Private outputTerminalLabel As System.Windows.Forms.Label
	Private outputTerminalComboBox As System.Windows.Forms.ComboBox
    Private WithEvents startAcquisitionButton As System.Windows.Forms.Button
    Private dataGridView1 As System.Windows.Forms.DataGridView
End Class

