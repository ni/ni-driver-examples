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
        Me.referenceLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.carrierFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.triggerSlopeLabel = New System.Windows.Forms.Label()
        Me.triggerSlopeComboBox = New System.Windows.Forms.ComboBox()
        Me.referencePositionLabel = New System.Windows.Forms.Label()
        Me.referencePositionNumeric = New System.Windows.Forms.NumericUpDown()
        Me.burstLengthLabel = New System.Windows.Forms.Label()
        Me.burstLengthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.triggerLevelLabel = New System.Windows.Forms.Label()
        Me.triggerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.minimumQuietTimeLabel = New System.Windows.Forms.Label()
        Me.minimumQuiteTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.dataGridViewResults = New System.Windows.Forms.DataGridView()
        Me.startButton = New System.Windows.Forms.Button()
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.carrierFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.referencePositionNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.burstLengthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.minimumQuiteTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 38)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(116, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(12, 18)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 28
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'referenceLevelLabel
        '
        Me.referenceLevelLabel.AutoSize = True
        Me.referenceLevelLabel.Location = New System.Drawing.Point(12, 66)
        Me.referenceLevelLabel.Name = "referenceLevelLabel"
        Me.referenceLevelLabel.Size = New System.Drawing.Size(116, 13)
        Me.referenceLevelLabel.TabIndex = 22
        Me.referenceLevelLabel.Text = "Reference Level (dBm)"
        '
        'carrierFrequencyLabel
        '
        Me.carrierFrequencyLabel.AutoSize = True
        Me.carrierFrequencyLabel.Location = New System.Drawing.Point(12, 113)
        Me.carrierFrequencyLabel.Name = "carrierFrequencyLabel"
        Me.carrierFrequencyLabel.Size = New System.Drawing.Size(112, 13)
        Me.carrierFrequencyLabel.TabIndex = 24
        Me.carrierFrequencyLabel.Text = "Carrier Frequency (Hz)"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(12, 160)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(44, 13)
        Me.iqRateLabel.TabIndex = 26
        Me.iqRateLabel.Text = "IQ Rate"
        '
        'referenceLevelNumeric
        '
        Me.referenceLevelNumeric.DecimalPlaces = 2
        Me.referenceLevelNumeric.Location = New System.Drawing.Point(12, 86)
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
        Me.carrierFrequencyNumeric.Location = New System.Drawing.Point(12, 133)
        Me.carrierFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.carrierFrequencyNumeric.Name = "carrierFrequencyNumeric"
        Me.carrierFrequencyNumeric.Size = New System.Drawing.Size(116, 20)
        Me.carrierFrequencyNumeric.TabIndex = 2
        Me.carrierFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 2
        Me.iqRateNumeric.Location = New System.Drawing.Point(12, 180)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(116, 20)
        Me.iqRateNumeric.TabIndex = 3
        Me.iqRateNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'triggerSlopeLabel
        '
        Me.triggerSlopeLabel.AutoSize = True
        Me.triggerSlopeLabel.Location = New System.Drawing.Point(12, 207)
        Me.triggerSlopeLabel.Name = "triggerSlopeLabel"
        Me.triggerSlopeLabel.Size = New System.Drawing.Size(70, 13)
        Me.triggerSlopeLabel.TabIndex = 30
        Me.triggerSlopeLabel.Text = "Trigger Slope"
        '
        'triggerSlopeComboBox
        '
        Me.triggerSlopeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerSlopeComboBox.Location = New System.Drawing.Point(12, 227)
        Me.triggerSlopeComboBox.Name = "triggerSlopeComboBox"
        Me.triggerSlopeComboBox.Size = New System.Drawing.Size(116, 21)
        Me.triggerSlopeComboBox.TabIndex = 4
        '
        'referencePositionLabel
        '
        Me.referencePositionLabel.AutoSize = True
        Me.referencePositionLabel.Location = New System.Drawing.Point(12, 349)
        Me.referencePositionLabel.Name = "referencePositionLabel"
        Me.referencePositionLabel.Size = New System.Drawing.Size(114, 13)
        Me.referencePositionLabel.TabIndex = 32
        Me.referencePositionLabel.Text = "Reference Position (%)"
        '
        'referencePositionNumeric
        '
        Me.referencePositionNumeric.DecimalPlaces = 2
        Me.referencePositionNumeric.Location = New System.Drawing.Point(12, 369)
        Me.referencePositionNumeric.Name = "referencePositionNumeric"
        Me.referencePositionNumeric.Size = New System.Drawing.Size(116, 20)
        Me.referencePositionNumeric.TabIndex = 7
        '
        'burstLengthLabel
        '
        Me.burstLengthLabel.AutoSize = True
        Me.burstLengthLabel.Location = New System.Drawing.Point(12, 302)
        Me.burstLengthLabel.Name = "burstLengthLabel"
        Me.burstLengthLabel.Size = New System.Drawing.Size(93, 13)
        Me.burstLengthLabel.TabIndex = 34
        Me.burstLengthLabel.Text = "Burst Length (sec)"
        '
        'burstLengthNumeric
        '
        Me.burstLengthNumeric.DecimalPlaces = 2
        Me.burstLengthNumeric.Location = New System.Drawing.Point(12, 322)
        Me.burstLengthNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.burstLengthNumeric.Name = "burstLengthNumeric"
        Me.burstLengthNumeric.Size = New System.Drawing.Size(116, 20)
        Me.burstLengthNumeric.TabIndex = 6
        Me.burstLengthNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'triggerLevelLabel
        '
        Me.triggerLevelLabel.AutoSize = True
        Me.triggerLevelLabel.Location = New System.Drawing.Point(12, 255)
        Me.triggerLevelLabel.Name = "triggerLevelLabel"
        Me.triggerLevelLabel.Size = New System.Drawing.Size(99, 13)
        Me.triggerLevelLabel.TabIndex = 36
        Me.triggerLevelLabel.Text = "Trigger Level (dBm)"
        '
        'triggerLevelNumeric
        '
        Me.triggerLevelNumeric.DecimalPlaces = 2
        Me.triggerLevelNumeric.Location = New System.Drawing.Point(12, 275)
        Me.triggerLevelNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.triggerLevelNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.triggerLevelNumeric.Name = "triggerLevelNumeric"
        Me.triggerLevelNumeric.Size = New System.Drawing.Size(116, 20)
        Me.triggerLevelNumeric.TabIndex = 5
        '
        'minimumQuietTimeLabel
        '
        Me.minimumQuietTimeLabel.AutoSize = True
        Me.minimumQuietTimeLabel.Location = New System.Drawing.Point(12, 396)
        Me.minimumQuietTimeLabel.Name = "minimumQuietTimeLabel"
        Me.minimumQuietTimeLabel.Size = New System.Drawing.Size(128, 13)
        Me.minimumQuietTimeLabel.TabIndex = 38
        Me.minimumQuietTimeLabel.Text = "Minimum Quiet Time (sec)"
        '
        'minimumQuiteTimeNumeric
        '
        Me.minimumQuiteTimeNumeric.DecimalPlaces = 2
        Me.minimumQuiteTimeNumeric.Location = New System.Drawing.Point(12, 416)
        Me.minimumQuiteTimeNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.minimumQuiteTimeNumeric.Name = "minimumQuiteTimeNumeric"
        Me.minimumQuiteTimeNumeric.Size = New System.Drawing.Size(116, 20)
        Me.minimumQuiteTimeNumeric.TabIndex = 8
        '
        'dataGridViewResults
        '
        Me.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dataGridViewResults.Location = New System.Drawing.Point(149, 18)
        Me.dataGridViewResults.Name = "dataGridViewResults"
        Me.dataGridViewResults.Size = New System.Drawing.Size(438, 454)
        Me.dataGridViewResults.TabIndex = 40
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(12, 443)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(116, 29)
        Me.startButton.TabIndex = 9
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(597, 482)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.dataGridViewResults)
        Me.Controls.Add(Me.minimumQuietTimeLabel)
        Me.Controls.Add(Me.minimumQuiteTimeNumeric)
        Me.Controls.Add(Me.triggerLevelLabel)
        Me.Controls.Add(Me.triggerLevelNumeric)
        Me.Controls.Add(Me.burstLengthLabel)
        Me.Controls.Add(Me.burstLengthNumeric)
        Me.Controls.Add(Me.referencePositionLabel)
        Me.Controls.Add(Me.referencePositionNumeric)
        Me.Controls.Add(Me.triggerSlopeLabel)
        Me.Controls.Add(Me.triggerSlopeComboBox)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.referenceLevelLabel)
        Me.Controls.Add(Me.carrierFrequencyLabel)
        Me.Controls.Add(Me.iqRateLabel)
        Me.Controls.Add(Me.referenceLevelNumeric)
        Me.Controls.Add(Me.carrierFrequencyNumeric)
        Me.Controls.Add(Me.iqRateNumeric)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "RFSA Pulse Trigger Acquisition"
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.carrierFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.referencePositionNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.burstLengthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.minimumQuiteTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private resourceNameLabel As System.Windows.Forms.Label
    Private referenceLevelLabel As System.Windows.Forms.Label
    Private carrierFrequencyLabel As System.Windows.Forms.Label
    Private iqRateLabel As System.Windows.Forms.Label
    Private referenceLevelNumeric As System.Windows.Forms.NumericUpDown
    Private carrierFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private triggerSlopeLabel As System.Windows.Forms.Label
    Private triggerSlopeComboBox As System.Windows.Forms.ComboBox
    Private referencePositionLabel As System.Windows.Forms.Label
    Private referencePositionNumeric As System.Windows.Forms.NumericUpDown
    Private burstLengthLabel As System.Windows.Forms.Label
    Private burstLengthNumeric As System.Windows.Forms.NumericUpDown
    Private triggerLevelLabel As System.Windows.Forms.Label
    Private triggerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private minimumQuietTimeLabel As System.Windows.Forms.Label
    Private minimumQuiteTimeNumeric As System.Windows.Forms.NumericUpDown
    Private dataGridViewResults As System.Windows.Forms.DataGridView
	Private WithEvents startButton As System.Windows.Forms.Button
End Class

