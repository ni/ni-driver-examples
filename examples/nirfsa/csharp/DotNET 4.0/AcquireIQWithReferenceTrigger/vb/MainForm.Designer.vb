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
        Me.samplesPerRecordLabel = New System.Windows.Forms.Label()
        Me.triggerTypeLabel = New System.Windows.Forms.Label()
        Me.pretriggerSamplesLabel = New System.Windows.Forms.Label()
        Me.triggerLevelLabel = New System.Windows.Forms.Label()
        Me.referenceLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.carrierFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.samplesPerRecordNumeric = New System.Windows.Forms.NumericUpDown()
        Me.pretriggerSamplesNumeric = New System.Windows.Forms.NumericUpDown()
        Me.triggerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.triggerTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerSourceLabel = New System.Windows.Forms.Label()
        Me.ResourceNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.startAcquisitionButton = New System.Windows.Forms.Button()
        Me.sendSoftwareTriggerbutton = New System.Windows.Forms.Button()
        Me.dataGridViewResults = New System.Windows.Forms.DataGridView()
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.carrierFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerRecordNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pretriggerSamplesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'referenceLevelLabel
        '
        Me.referenceLevelLabel.AutoSize = True
        Me.referenceLevelLabel.Location = New System.Drawing.Point(12, 57)
        Me.referenceLevelLabel.Name = "referenceLevelLabel"
        Me.referenceLevelLabel.Size = New System.Drawing.Size(116, 13)
        Me.referenceLevelLabel.TabIndex = 0
        Me.referenceLevelLabel.Text = "Reference Level (dBm)"
        '
        'carrierFrequencyLabel
        '
        Me.carrierFrequencyLabel.AutoSize = True
        Me.carrierFrequencyLabel.Location = New System.Drawing.Point(12, 102)
        Me.carrierFrequencyLabel.Name = "carrierFrequencyLabel"
        Me.carrierFrequencyLabel.Size = New System.Drawing.Size(112, 13)
        Me.carrierFrequencyLabel.TabIndex = 1
        Me.carrierFrequencyLabel.Text = "Carrier Frequency (Hz)"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(12, 148)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(44, 13)
        Me.iqRateLabel.TabIndex = 2
        Me.iqRateLabel.Text = "IQ Rate"
        '
        'samplesPerRecordLabel
        '
        Me.samplesPerRecordLabel.AutoSize = True
        Me.samplesPerRecordLabel.Location = New System.Drawing.Point(12, 194)
        Me.samplesPerRecordLabel.Name = "samplesPerRecordLabel"
        Me.samplesPerRecordLabel.Size = New System.Drawing.Size(103, 13)
        Me.samplesPerRecordLabel.TabIndex = 3
        Me.samplesPerRecordLabel.Text = "Samples per Record"
        '
        'triggerTypeLabel
        '
        Me.triggerTypeLabel.AutoSize = True
        Me.triggerTypeLabel.Location = New System.Drawing.Point(12, 241)
        Me.triggerTypeLabel.Name = "triggerTypeLabel"
        Me.triggerTypeLabel.Size = New System.Drawing.Size(67, 13)
        Me.triggerTypeLabel.TabIndex = 4
        Me.triggerTypeLabel.Text = "Trigger Type"
        '
        'pretriggerSamplesLabel
        '
        Me.pretriggerSamplesLabel.AutoSize = True
        Me.pretriggerSamplesLabel.Location = New System.Drawing.Point(12, 288)
        Me.pretriggerSamplesLabel.Name = "pretriggerSamplesLabel"
        Me.pretriggerSamplesLabel.Size = New System.Drawing.Size(95, 13)
        Me.pretriggerSamplesLabel.TabIndex = 5
        Me.pretriggerSamplesLabel.Text = "Pretrigger Samples"
        '
        'triggerLevelLabel
        '
        Me.triggerLevelLabel.AutoSize = True
        Me.triggerLevelLabel.Location = New System.Drawing.Point(12, 377)
        Me.triggerLevelLabel.Name = "triggerLevelLabel"
        Me.triggerLevelLabel.Size = New System.Drawing.Size(99, 13)
        Me.triggerLevelLabel.TabIndex = 7
        Me.triggerLevelLabel.Text = "Trigger Level (dBm)"
        '
        'referenceLevelNumeric
        '
        Me.referenceLevelNumeric.DecimalPlaces = 2
        Me.referenceLevelNumeric.Location = New System.Drawing.Point(12, 76)
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
        Me.carrierFrequencyNumeric.Location = New System.Drawing.Point(12, 122)
        Me.carrierFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.carrierFrequencyNumeric.Name = "carrierFrequencyNumeric"
        Me.carrierFrequencyNumeric.Size = New System.Drawing.Size(116, 20)
        Me.carrierFrequencyNumeric.TabIndex = 2
        Me.carrierFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 2
        Me.iqRateNumeric.Location = New System.Drawing.Point(12, 169)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(116, 20)
        Me.iqRateNumeric.TabIndex = 3
        Me.iqRateNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'samplesPerRecordNumeric
        '
        Me.samplesPerRecordNumeric.Location = New System.Drawing.Point(12, 216)
        Me.samplesPerRecordNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.samplesPerRecordNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerRecordNumeric.Name = "samplesPerRecordNumeric"
        Me.samplesPerRecordNumeric.Size = New System.Drawing.Size(116, 20)
        Me.samplesPerRecordNumeric.TabIndex = 4
        Me.samplesPerRecordNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'pretriggerSamplesNumeric
        '
        Me.pretriggerSamplesNumeric.Location = New System.Drawing.Point(12, 308)
        Me.pretriggerSamplesNumeric.Maximum = New Decimal(New Integer() {-1, 0, 0, 0})
        Me.pretriggerSamplesNumeric.Name = "pretriggerSamplesNumeric"
        Me.pretriggerSamplesNumeric.Size = New System.Drawing.Size(116, 20)
        Me.pretriggerSamplesNumeric.TabIndex = 6
        '
        'triggerLevelNumeric
        '
        Me.triggerLevelNumeric.DecimalPlaces = 2
        Me.triggerLevelNumeric.Location = New System.Drawing.Point(12, 393)
        Me.triggerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.triggerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.triggerLevelNumeric.Name = "triggerLevelNumeric"
        Me.triggerLevelNumeric.Size = New System.Drawing.Size(116, 20)
        Me.triggerLevelNumeric.TabIndex = 8
        '
        'triggerTypeComboBox
        '
        Me.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerTypeComboBox.Location = New System.Drawing.Point(12, 261)
        Me.triggerTypeComboBox.Name = "triggerTypeComboBox"
        Me.triggerTypeComboBox.Size = New System.Drawing.Size(116, 21)
        Me.triggerTypeComboBox.TabIndex = 5
        '
        'triggerSourceComboBox
        '
        Me.triggerSourceComboBox.Location = New System.Drawing.Point(12, 353)
        Me.triggerSourceComboBox.Name = "triggerSourceComboBox"
        Me.triggerSourceComboBox.Size = New System.Drawing.Size(116, 21)
        Me.triggerSourceComboBox.TabIndex = 7
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.AutoSize = True
        Me.triggerSourceLabel.Location = New System.Drawing.Point(12, 335)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(77, 13)
        Me.triggerSourceLabel.TabIndex = 6
        Me.triggerSourceLabel.Text = "Trigger Source"
        '
        'ResourceNameLabel
        '
        Me.ResourceNameLabel.AutoSize = True
        Me.ResourceNameLabel.Location = New System.Drawing.Point(15, 11)
        Me.ResourceNameLabel.Name = "ResourceNameLabel"
        Me.ResourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.ResourceNameLabel.TabIndex = 9
        Me.ResourceNameLabel.Text = "Resource Name"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 31)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(110, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'startAcquisitionButton
        '
        Me.startAcquisitionButton.Location = New System.Drawing.Point(12, 432)
        Me.startAcquisitionButton.Name = "startAcquisitionButton"
        Me.startAcquisitionButton.Size = New System.Drawing.Size(116, 30)
        Me.startAcquisitionButton.TabIndex = 9
        Me.startAcquisitionButton.Text = "Start &Acquisition"
        Me.startAcquisitionButton.UseVisualStyleBackColor = True
        '
        'sendSoftwareTriggerbutton
        '
        Me.sendSoftwareTriggerbutton.Location = New System.Drawing.Point(134, 432)
        Me.sendSoftwareTriggerbutton.Name = "sendSoftwareTriggerbutton"
        Me.sendSoftwareTriggerbutton.Size = New System.Drawing.Size(140, 30)
        Me.sendSoftwareTriggerbutton.TabIndex = 10
        Me.sendSoftwareTriggerbutton.Text = "&Send Software Trigger"
        Me.sendSoftwareTriggerbutton.UseVisualStyleBackColor = True
        '
        'dataGridViewResults
        '
        Me.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dataGridViewResults.Location = New System.Drawing.Point(134, 11)
        Me.dataGridViewResults.Name = "dataGridViewResults"
        Me.dataGridViewResults.Size = New System.Drawing.Size(472, 402)
        Me.dataGridViewResults.TabIndex = 13
        '
        'MainForm
        '
        Me.AcceptButton = Me.startAcquisitionButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(615, 467)
        Me.Controls.Add(Me.dataGridViewResults)
        Me.Controls.Add(Me.sendSoftwareTriggerbutton)
        Me.Controls.Add(Me.startAcquisitionButton)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.ResourceNameLabel)
        Me.Controls.Add(Me.referenceLevelLabel)
        Me.Controls.Add(Me.carrierFrequencyLabel)
        Me.Controls.Add(Me.iqRateLabel)
        Me.Controls.Add(Me.samplesPerRecordLabel)
        Me.Controls.Add(Me.triggerTypeLabel)
        Me.Controls.Add(Me.pretriggerSamplesLabel)
        Me.Controls.Add(Me.triggerSourceLabel)
        Me.Controls.Add(Me.triggerLevelLabel)
        Me.Controls.Add(Me.referenceLevelNumeric)
        Me.Controls.Add(Me.carrierFrequencyNumeric)
        Me.Controls.Add(Me.iqRateNumeric)
        Me.Controls.Add(Me.samplesPerRecordNumeric)
        Me.Controls.Add(Me.pretriggerSamplesNumeric)
        Me.Controls.Add(Me.triggerLevelNumeric)
        Me.Controls.Add(Me.triggerTypeComboBox)
        Me.Controls.Add(Me.triggerSourceComboBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Acquire IQ with Reference Trigger"
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.carrierFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerRecordNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pretriggerSamplesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private referenceLevelLabel As System.Windows.Forms.Label
    Private carrierFrequencyLabel As System.Windows.Forms.Label
    Private iqRateLabel As System.Windows.Forms.Label
    Private samplesPerRecordLabel As System.Windows.Forms.Label
    Private triggerTypeLabel As System.Windows.Forms.Label
    Private pretriggerSamplesLabel As System.Windows.Forms.Label
    Private triggerLevelLabel As System.Windows.Forms.Label
    Private referenceLevelNumeric As System.Windows.Forms.NumericUpDown
    Private carrierFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private samplesPerRecordNumeric As System.Windows.Forms.NumericUpDown
    Private pretriggerSamplesNumeric As System.Windows.Forms.NumericUpDown
    Private triggerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents triggerTypeComboBox As System.Windows.Forms.ComboBox
    Private triggerSourceComboBox As System.Windows.Forms.ComboBox
    Private triggerSourceLabel As System.Windows.Forms.Label
    Private ResourceNameLabel As System.Windows.Forms.Label
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents startAcquisitionButton As System.Windows.Forms.Button
    Private WithEvents sendSoftwareTriggerbutton As System.Windows.Forms.Button
    Private dataGridViewResults As System.Windows.Forms.DataGridView

End Class
