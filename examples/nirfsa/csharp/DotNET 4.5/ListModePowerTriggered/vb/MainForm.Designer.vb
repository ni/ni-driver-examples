Partial Class MainForm
	''' <summary>
	''' Required designer variable.
	''' </summary>
	Private components As System.ComponentModel.IContainer = Nothing

	''' <summary>
	''' Clean up any resources being used.
	''' </summary>
	''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
		Me.carrierFreqRampStrtLabel = New System.Windows.Forms.Label()
		Me.carrierFreqRampStopLabel = New System.Windows.Forms.Label()
		Me.iqRateLabel = New System.Windows.Forms.Label()
		Me.samplesPerRecordLabel = New System.Windows.Forms.Label()
		Me.numberOfStepsLabel = New System.Windows.Forms.Label()
		Me.referenceLevelRampStrtLabel = New System.Windows.Forms.Label()
		Me.referenceLevelRampStopLabel = New System.Windows.Forms.Label()
		Me.triggerLevelRampStrtLabel = New System.Windows.Forms.Label()
		Me.triggerLevelRampStopLabel = New System.Windows.Forms.Label()
		Me.carrierFreqRampStrtNumeric = New System.Windows.Forms.NumericUpDown()
		Me.carrierFreqRampStopNumeric = New System.Windows.Forms.NumericUpDown()
		Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
		Me.samplesPerRecordNumeric = New System.Windows.Forms.NumericUpDown()
		Me.numberOfStepsNumeric = New System.Windows.Forms.NumericUpDown()
		Me.referenceLevelRampStrtNumeric = New System.Windows.Forms.NumericUpDown()
		Me.referenceLevelRampStopNumeric = New System.Windows.Forms.NumericUpDown()
		Me.triggerLevelRampStrtNumeric = New System.Windows.Forms.NumericUpDown()
		Me.triggerLevelRampStopNumeric = New System.Windows.Forms.NumericUpDown()
		Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
		Me.resourceNameLabel = New System.Windows.Forms.Label()
		Me.acquireButton = New System.Windows.Forms.Button()
		Me.multiDataGridViewResults = New NationalInstruments.Examples.ListModePowerTriggered.MultiDataGridView()
        CType(Me.carrierFreqRampStrtNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.carrierFreqRampStopNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerRecordNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberOfStepsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.referenceLevelRampStrtNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.referenceLevelRampStopNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.triggerLevelRampStrtNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.triggerLevelRampStopNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        ' 
        ' carrierFreqRampStrtLabel
        ' 
        Me.carrierFreqRampStrtLabel.AutoSize = True
        Me.carrierFreqRampStrtLabel.Location = New System.Drawing.Point(12, 200)
        Me.carrierFreqRampStrtLabel.Name = "carrierFreqRampStrtLabel"
        Me.carrierFreqRampStrtLabel.Size = New System.Drawing.Size(182, 13)
        Me.carrierFreqRampStrtLabel.TabIndex = 0
        Me.carrierFreqRampStrtLabel.Text = "IQ Carrier Frequency Ramp Start (Hz)"
        ' 
        ' carrierFreqRampStopLabel
        ' 
        Me.carrierFreqRampStopLabel.AutoSize = True
        Me.carrierFreqRampStopLabel.Location = New System.Drawing.Point(12, 247)
        Me.carrierFreqRampStopLabel.Name = "carrierFreqRampStopLabel"
        Me.carrierFreqRampStopLabel.Size = New System.Drawing.Size(182, 13)
        Me.carrierFreqRampStopLabel.TabIndex = 1
        Me.carrierFreqRampStopLabel.Text = "IQ Carrier Frequency Ramp Stop (Hz)"
        ' 
        ' iqRateLabel
        ' 
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(12, 56)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
        Me.iqRateLabel.TabIndex = 2
        Me.iqRateLabel.Text = "IQ Rate (S/s)"
        ' 
        ' samplesPerRecordLabel
        ' 
        Me.samplesPerRecordLabel.AutoSize = True
        Me.samplesPerRecordLabel.Location = New System.Drawing.Point(12, 104)
        Me.samplesPerRecordLabel.Name = "samplesPerRecordLabel"
        Me.samplesPerRecordLabel.Size = New System.Drawing.Size(103, 13)
        Me.samplesPerRecordLabel.TabIndex = 3
        Me.samplesPerRecordLabel.Text = "Samples per Record"
        ' 
        ' numberOfStepsLabel
        ' 
        Me.numberOfStepsLabel.AutoSize = True
        Me.numberOfStepsLabel.Location = New System.Drawing.Point(12, 150)
        Me.numberOfStepsLabel.Name = "numberOfStepsLabel"
        Me.numberOfStepsLabel.Size = New System.Drawing.Size(86, 13)
        Me.numberOfStepsLabel.TabIndex = 4
        Me.numberOfStepsLabel.Text = "Number of Steps"
        ' 
        ' referenceLevelRampStrtLabel
        ' 
        Me.referenceLevelRampStrtLabel.AutoSize = True
        Me.referenceLevelRampStrtLabel.Location = New System.Drawing.Point(12, 301)
        Me.referenceLevelRampStrtLabel.Name = "referenceLevelRampStrtLabel"
        Me.referenceLevelRampStrtLabel.Size = New System.Drawing.Size(156, 13)
        Me.referenceLevelRampStrtLabel.TabIndex = 5
        Me.referenceLevelRampStrtLabel.Text = "Reference Level Ramp Start (s)"
        ' 
        ' referenceLevelRampStopLabel
        ' 
        Me.referenceLevelRampStopLabel.AutoSize = True
        Me.referenceLevelRampStopLabel.Location = New System.Drawing.Point(12, 347)
        Me.referenceLevelRampStopLabel.Name = "referenceLevelRampStopLabel"
        Me.referenceLevelRampStopLabel.Size = New System.Drawing.Size(156, 13)
        Me.referenceLevelRampStopLabel.TabIndex = 6
        Me.referenceLevelRampStopLabel.Text = "Reference Level Ramp Stop (s)"
        ' 
        ' triggerLevelRampStrtLabel
        ' 
        Me.triggerLevelRampStrtLabel.AutoSize = True
        Me.triggerLevelRampStrtLabel.Location = New System.Drawing.Point(12, 397)
        Me.triggerLevelRampStrtLabel.Name = "triggerLevelRampStrtLabel"
        Me.triggerLevelRampStrtLabel.Size = New System.Drawing.Size(188, 13)
        Me.triggerLevelRampStrtLabel.TabIndex = 7
        Me.triggerLevelRampStrtLabel.Text = "Power Trigger Level Ramp Start (dBm)"
        ' 
        ' triggerLevelRampStopLabel
        ' 
        Me.triggerLevelRampStopLabel.AutoSize = True
        Me.triggerLevelRampStopLabel.Location = New System.Drawing.Point(12, 449)
        Me.triggerLevelRampStopLabel.Name = "triggerLevelRampStopLabel"
        Me.triggerLevelRampStopLabel.Size = New System.Drawing.Size(188, 13)
        Me.triggerLevelRampStopLabel.TabIndex = 8
        Me.triggerLevelRampStopLabel.Text = "Power Trigger Level Ramp Stop (dBm)"
        ' 
        ' carrierFreqRampStrtNumeric
        ' 
        Me.carrierFreqRampStrtNumeric.DecimalPlaces = 4
        Me.carrierFreqRampStrtNumeric.Location = New System.Drawing.Point(12, 221)
        Me.carrierFreqRampStrtNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.carrierFreqRampStrtNumeric.Name = "carrierFreqRampStrtNumeric"
        Me.carrierFreqRampStrtNumeric.Size = New System.Drawing.Size(149, 20)
        Me.carrierFreqRampStrtNumeric.TabIndex = 4
        Me.carrierFreqRampStrtNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        ' 
        ' carrierFreqRampStopNumeric
        ' 
        Me.carrierFreqRampStopNumeric.DecimalPlaces = 4
        Me.carrierFreqRampStopNumeric.Location = New System.Drawing.Point(12, 268)
        Me.carrierFreqRampStopNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.carrierFreqRampStopNumeric.Name = "carrierFreqRampStopNumeric"
        Me.carrierFreqRampStopNumeric.Size = New System.Drawing.Size(149, 20)
        Me.carrierFreqRampStopNumeric.TabIndex = 5
        Me.carrierFreqRampStopNumeric.Value = New Decimal(New Integer() {1020000000, 0, 0, 0})
        ' 
        ' iqRateNumeric
        ' 
        Me.iqRateNumeric.DecimalPlaces = 2
        Me.iqRateNumeric.Location = New System.Drawing.Point(12, 77)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(149, 20)
        Me.iqRateNumeric.TabIndex = 1
        Me.iqRateNumeric.Value = New Decimal(New Integer() {5000000, 0, 0, 0})
        ' 
        ' samplesPerRecordNumeric
        ' 
        Me.samplesPerRecordNumeric.Location = New System.Drawing.Point(12, 125)
        Me.samplesPerRecordNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.samplesPerRecordNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerRecordNumeric.Name = "samplesPerRecordNumeric"
        Me.samplesPerRecordNumeric.Size = New System.Drawing.Size(151, 20)
        Me.samplesPerRecordNumeric.TabIndex = 2
        Me.samplesPerRecordNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        ' 
        ' numberOfStepsNumeric
        ' 
        Me.numberOfStepsNumeric.Location = New System.Drawing.Point(12, 171)
        Me.numberOfStepsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numberOfStepsNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numberOfStepsNumeric.Name = "numberOfStepsNumeric"
        Me.numberOfStepsNumeric.Size = New System.Drawing.Size(149, 20)
        Me.numberOfStepsNumeric.TabIndex = 3
        Me.numberOfStepsNumeric.Value = New Decimal(New Integer() {21, 0, 0, 0})
        ' 
        ' referenceLevelRampStrtNumeric
        ' 
        Me.referenceLevelRampStrtNumeric.DecimalPlaces = 2
        Me.referenceLevelRampStrtNumeric.Location = New System.Drawing.Point(12, 322)
        Me.referenceLevelRampStrtNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.referenceLevelRampStrtNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.referenceLevelRampStrtNumeric.Name = "referenceLevelRampStrtNumeric"
        Me.referenceLevelRampStrtNumeric.Size = New System.Drawing.Size(149, 20)
        Me.referenceLevelRampStrtNumeric.TabIndex = 6
        Me.referenceLevelRampStrtNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        ' 
        ' referenceLevelRampStopNumeric
        ' 
        Me.referenceLevelRampStopNumeric.DecimalPlaces = 2
        Me.referenceLevelRampStopNumeric.Location = New System.Drawing.Point(12, 368)
        Me.referenceLevelRampStopNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.referenceLevelRampStopNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.referenceLevelRampStopNumeric.Name = "referenceLevelRampStopNumeric"
        Me.referenceLevelRampStopNumeric.Size = New System.Drawing.Size(149, 20)
        Me.referenceLevelRampStopNumeric.TabIndex = 7
        ' 
        ' triggerLevelRampStrtNumeric
        ' 
        Me.triggerLevelRampStrtNumeric.DecimalPlaces = 2
        Me.triggerLevelRampStrtNumeric.Location = New System.Drawing.Point(12, 418)
        Me.triggerLevelRampStrtNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.triggerLevelRampStrtNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.triggerLevelRampStrtNumeric.Name = "triggerLevelRampStrtNumeric"
        Me.triggerLevelRampStrtNumeric.Size = New System.Drawing.Size(149, 20)
        Me.triggerLevelRampStrtNumeric.TabIndex = 8
        Me.triggerLevelRampStrtNumeric.Value = New Decimal(New Integer() {30, 0, 0, -2147483648})
        ' 
        ' triggerLevelRampStopNumeric
        ' 
        Me.triggerLevelRampStopNumeric.DecimalPlaces = 2
        Me.triggerLevelRampStopNumeric.Location = New System.Drawing.Point(12, 470)
        Me.triggerLevelRampStopNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.triggerLevelRampStopNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.triggerLevelRampStopNumeric.Name = "triggerLevelRampStopNumeric"
        Me.triggerLevelRampStopNumeric.Size = New System.Drawing.Size(149, 20)
        Me.triggerLevelRampStopNumeric.TabIndex = 9
        Me.triggerLevelRampStopNumeric.Value = New Decimal(New Integer() {10, 0, 0, -2147483648})
        ' 
        ' resourceNameComboBox
        ' 
        Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 31)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(151, 21)
        Me.resourceNameComboBox.TabIndex = 0
        ' 
        ' resourceNameLabel
        ' 
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(12, 14)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 10
        Me.resourceNameLabel.Text = "Resource Name"
        ' 
        ' acquireButton
        ' 
        Me.acquireButton.Location = New System.Drawing.Point(12, 507)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(149, 23)
        Me.acquireButton.TabIndex = 10
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True        
        ' 
        ' multiDataGridViewResults
        ' 
        Me.multiDataGridViewResults.Location = New System.Drawing.Point(206, 24)
        Me.multiDataGridViewResults.Name = "multiDataGridViewResults"
        Me.multiDataGridViewResults.Size = New System.Drawing.Size(442, 466)
        Me.multiDataGridViewResults.TabIndex = 41
        ' 
        ' MainForm
        ' 
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(663, 541)
        Me.Controls.Add(Me.multiDataGridViewResults)
        Me.Controls.Add(Me.acquireButton)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.carrierFreqRampStrtLabel)
        Me.Controls.Add(Me.carrierFreqRampStopLabel)
        Me.Controls.Add(Me.iqRateLabel)
        Me.Controls.Add(Me.samplesPerRecordLabel)
        Me.Controls.Add(Me.numberOfStepsLabel)
        Me.Controls.Add(Me.referenceLevelRampStrtLabel)
        Me.Controls.Add(Me.referenceLevelRampStopLabel)
        Me.Controls.Add(Me.triggerLevelRampStrtLabel)
        Me.Controls.Add(Me.triggerLevelRampStopLabel)
        Me.Controls.Add(Me.carrierFreqRampStrtNumeric)
        Me.Controls.Add(Me.carrierFreqRampStopNumeric)
        Me.Controls.Add(Me.iqRateNumeric)
        Me.Controls.Add(Me.samplesPerRecordNumeric)
        Me.Controls.Add(Me.numberOfStepsNumeric)
        Me.Controls.Add(Me.referenceLevelRampStrtNumeric)
        Me.Controls.Add(Me.referenceLevelRampStopNumeric)
        Me.Controls.Add(Me.triggerLevelRampStrtNumeric)
        Me.Controls.Add(Me.triggerLevelRampStopNumeric)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "RFSA List Mode Frequency and Power Sweep - Power Triggered"
        CType(Me.carrierFreqRampStrtNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.carrierFreqRampStopNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerRecordNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberOfStepsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.referenceLevelRampStrtNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.referenceLevelRampStopNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.triggerLevelRampStrtNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.triggerLevelRampStopNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	#End Region

	Private carrierFreqRampStrtLabel As System.Windows.Forms.Label
	Private carrierFreqRampStopLabel As System.Windows.Forms.Label
	Private iqRateLabel As System.Windows.Forms.Label
	Private samplesPerRecordLabel As System.Windows.Forms.Label
	Private numberOfStepsLabel As System.Windows.Forms.Label
	Private referenceLevelRampStrtLabel As System.Windows.Forms.Label
	Private referenceLevelRampStopLabel As System.Windows.Forms.Label
	Private triggerLevelRampStrtLabel As System.Windows.Forms.Label
	Private triggerLevelRampStopLabel As System.Windows.Forms.Label
	Private carrierFreqRampStrtNumeric As System.Windows.Forms.NumericUpDown
	Private carrierFreqRampStopNumeric As System.Windows.Forms.NumericUpDown
	Private iqRateNumeric As System.Windows.Forms.NumericUpDown
	Private samplesPerRecordNumeric As System.Windows.Forms.NumericUpDown
	Private numberOfStepsNumeric As System.Windows.Forms.NumericUpDown
	Private referenceLevelRampStrtNumeric As System.Windows.Forms.NumericUpDown
	Private referenceLevelRampStopNumeric As System.Windows.Forms.NumericUpDown
	Private triggerLevelRampStrtNumeric As System.Windows.Forms.NumericUpDown
	Private triggerLevelRampStopNumeric As System.Windows.Forms.NumericUpDown
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private resourceNameLabel As System.Windows.Forms.Label
    Private WithEvents acquireButton As System.Windows.Forms.Button
	Private multiDataGridViewResults As NationalInstruments.Examples.ListModePowerTriggered.MultiDataGridView

End Class
