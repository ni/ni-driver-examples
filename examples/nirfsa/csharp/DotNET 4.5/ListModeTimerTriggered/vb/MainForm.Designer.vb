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
		Me.timerEventIntervalLabel = New System.Windows.Forms.Label()
		Me.referenceLevelRampStrtLabel = New System.Windows.Forms.Label()
		Me.referenceLevelRampStopLabel = New System.Windows.Forms.Label()
		Me.iqRateLabel = New System.Windows.Forms.Label()
		Me.samplesPerRecordLabel = New System.Windows.Forms.Label()
		Me.numberOfStepsLabel = New System.Windows.Forms.Label()
		Me.carrierFrequencyRampStrtLabel = New System.Windows.Forms.Label()
		Me.carrierFrequencyRampStopLabel = New System.Windows.Forms.Label()
		Me.timerEventIntervalNumeric = New System.Windows.Forms.NumericUpDown()
		Me.referenceLevelRampStrtNumeric = New System.Windows.Forms.NumericUpDown()
		Me.referenceLevelRampStopNumeric = New System.Windows.Forms.NumericUpDown()
		Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
		Me.samplesPerRecordNumeric = New System.Windows.Forms.NumericUpDown()
		Me.numberOfStepsNumeric = New System.Windows.Forms.NumericUpDown()
		Me.carrierFrequencyRampStrtNumeric = New System.Windows.Forms.NumericUpDown()
		Me.carrierFrequencyRampStopNumeric = New System.Windows.Forms.NumericUpDown()
		Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
		Me.resourceNameLabel = New System.Windows.Forms.Label()
		Me.acquireButton = New System.Windows.Forms.Button()
		Me.multiDataGridViewResults = New NationalInstruments.Examples.ListModeTimerTriggered.MultiDataGridView()
        CType(Me.timerEventIntervalNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.referenceLevelRampStrtNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.referenceLevelRampStopNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerRecordNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberOfStepsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.carrierFrequencyRampStrtNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.carrierFrequencyRampStopNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		' 
		' timerEventIntervalLabel
		' 
		Me.timerEventIntervalLabel.AutoSize = True
		Me.timerEventIntervalLabel.Location = New System.Drawing.Point(12, 406)
		Me.timerEventIntervalLabel.Name = "timerEventIntervalLabel"
		Me.timerEventIntervalLabel.Size = New System.Drawing.Size(116, 13)
		Me.timerEventIntervalLabel.TabIndex = 0
		Me.timerEventIntervalLabel.Text = "Timer Event Interval (s)"
		' 
		' referenceLevelRampStrtLabel
		' 
		Me.referenceLevelRampStrtLabel.AutoSize = True
		Me.referenceLevelRampStrtLabel.Location = New System.Drawing.Point(12, 204)
		Me.referenceLevelRampStrtLabel.Name = "referenceLevelRampStrtLabel"
		Me.referenceLevelRampStrtLabel.Size = New System.Drawing.Size(175, 13)
		Me.referenceLevelRampStrtLabel.TabIndex = 1
		Me.referenceLevelRampStrtLabel.Text = "Reference Level  Ramp Start (dBm)"
		' 
		' referenceLevelRampStopLabel
		' 
		Me.referenceLevelRampStopLabel.AutoSize = True
		Me.referenceLevelRampStopLabel.Location = New System.Drawing.Point(12, 254)
		Me.referenceLevelRampStopLabel.Name = "referenceLevelRampStopLabel"
		Me.referenceLevelRampStopLabel.Size = New System.Drawing.Size(172, 13)
		Me.referenceLevelRampStopLabel.TabIndex = 2
		Me.referenceLevelRampStopLabel.Text = "Reference Level Ramp Stop (dBm)"
		' 
		' iqRateLabel
		' 
		Me.iqRateLabel.AutoSize = True
		Me.iqRateLabel.Location = New System.Drawing.Point(12, 56)
		Me.iqRateLabel.Name = "iqRateLabel"
		Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
		Me.iqRateLabel.TabIndex = 3
		Me.iqRateLabel.Text = "IQ Rate (S/s)"
		' 
		' samplesPerRecordLabel
		' 
		Me.samplesPerRecordLabel.AutoSize = True
		Me.samplesPerRecordLabel.Location = New System.Drawing.Point(12, 104)
		Me.samplesPerRecordLabel.Name = "samplesPerRecordLabel"
		Me.samplesPerRecordLabel.Size = New System.Drawing.Size(103, 13)
		Me.samplesPerRecordLabel.TabIndex = 4
		Me.samplesPerRecordLabel.Text = "Samples per Record"
		' 
		' numberOfStepsLabel
		' 
		Me.numberOfStepsLabel.AutoSize = True
		Me.numberOfStepsLabel.Location = New System.Drawing.Point(12, 151)
		Me.numberOfStepsLabel.Name = "numberOfStepsLabel"
		Me.numberOfStepsLabel.Size = New System.Drawing.Size(86, 13)
		Me.numberOfStepsLabel.TabIndex = 5
		Me.numberOfStepsLabel.Text = "Number of Steps"
		' 
		' carrierFrequencyRampStrtLabel
		' 
		Me.carrierFrequencyRampStrtLabel.AutoSize = True
		Me.carrierFrequencyRampStrtLabel.Location = New System.Drawing.Point(12, 304)
		Me.carrierFrequencyRampStrtLabel.Name = "carrierFrequencyRampStrtLabel"
		Me.carrierFrequencyRampStrtLabel.Size = New System.Drawing.Size(168, 13)
		Me.carrierFrequencyRampStrtLabel.TabIndex = 6
		Me.carrierFrequencyRampStrtLabel.Text = "Carrier Frequency Ramp Start (Hz)"
		' 
		' carrierFrequencyRampStopLabel
		' 
		Me.carrierFrequencyRampStopLabel.AutoSize = True
		Me.carrierFrequencyRampStopLabel.Location = New System.Drawing.Point(12, 354)
		Me.carrierFrequencyRampStopLabel.Name = "carrierFrequencyRampStopLabel"
		Me.carrierFrequencyRampStopLabel.Size = New System.Drawing.Size(168, 13)
		Me.carrierFrequencyRampStopLabel.TabIndex = 7
		Me.carrierFrequencyRampStopLabel.Text = "Carrier Frequency Ramp Stop (Hz)"
		' 
		' timerEventIntervalNumeric
		' 
		Me.timerEventIntervalNumeric.DecimalPlaces = 2
		Me.timerEventIntervalNumeric.Location = New System.Drawing.Point(12, 427)
		Me.timerEventIntervalNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.timerEventIntervalNumeric.Name = "timerEventIntervalNumeric"
		Me.timerEventIntervalNumeric.Size = New System.Drawing.Size(149, 20)
		Me.timerEventIntervalNumeric.TabIndex = 8
		Me.timerEventIntervalNumeric.Value = New Decimal(New Integer() {20, 0, 0, 196608})
		' 
		' referenceLevelRampStrtNumeric
		' 
		Me.referenceLevelRampStrtNumeric.DecimalPlaces = 2
		Me.referenceLevelRampStrtNumeric.Location = New System.Drawing.Point(12, 225)
		Me.referenceLevelRampStrtNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.referenceLevelRampStrtNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.referenceLevelRampStrtNumeric.Name = "referenceLevelRampStrtNumeric"
		Me.referenceLevelRampStrtNumeric.Size = New System.Drawing.Size(149, 20)
		Me.referenceLevelRampStrtNumeric.TabIndex = 4
		Me.referenceLevelRampStrtNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
		' 
		' referenceLevelRampStopNumeric
		' 
		Me.referenceLevelRampStopNumeric.DecimalPlaces = 2
		Me.referenceLevelRampStopNumeric.Location = New System.Drawing.Point(12, 275)
		Me.referenceLevelRampStopNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.referenceLevelRampStopNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.referenceLevelRampStopNumeric.Name = "referenceLevelRampStopNumeric"
		Me.referenceLevelRampStopNumeric.Size = New System.Drawing.Size(149, 20)
		Me.referenceLevelRampStopNumeric.TabIndex = 5
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
		Me.numberOfStepsNumeric.Location = New System.Drawing.Point(12, 172)
		Me.numberOfStepsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.numberOfStepsNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.numberOfStepsNumeric.Name = "numberOfStepsNumeric"
		Me.numberOfStepsNumeric.Size = New System.Drawing.Size(149, 20)
		Me.numberOfStepsNumeric.TabIndex = 3
		Me.numberOfStepsNumeric.Value = New Decimal(New Integer() {21, 0, 0, 0})
		' 
		' carrierFrequencyRampStrtNumeric
		' 
		Me.carrierFrequencyRampStrtNumeric.DecimalPlaces = 4
		Me.carrierFrequencyRampStrtNumeric.Location = New System.Drawing.Point(12, 325)
		Me.carrierFrequencyRampStrtNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.carrierFrequencyRampStrtNumeric.Name = "carrierFrequencyRampStrtNumeric"
		Me.carrierFrequencyRampStrtNumeric.Size = New System.Drawing.Size(149, 20)
		Me.carrierFrequencyRampStrtNumeric.TabIndex = 6
		Me.carrierFrequencyRampStrtNumeric.Value = New Decimal(New Integer() {1020000000, 0, 0, 0})
		' 
		' carrierFrequencyRampStopNumeric
		' 
		Me.carrierFrequencyRampStopNumeric.DecimalPlaces = 4
		Me.carrierFrequencyRampStopNumeric.Location = New System.Drawing.Point(12, 375)
		Me.carrierFrequencyRampStopNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.carrierFrequencyRampStopNumeric.Name = "carrierFrequencyRampStopNumeric"
		Me.carrierFrequencyRampStopNumeric.Size = New System.Drawing.Size(149, 20)
		Me.carrierFrequencyRampStopNumeric.TabIndex = 7
		Me.carrierFrequencyRampStopNumeric.Value = New Decimal(New Integer() {1001000000, 0, 0, 0})
		' 
		' resourceNameComboBox
		' 
		Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 28)
		Me.resourceNameComboBox.Name = "resourceNameComboBox"
		Me.resourceNameComboBox.Size = New System.Drawing.Size(155, 21)
		Me.resourceNameComboBox.TabIndex = 0
		' 
		' resourceNameLabel
		' 
		Me.resourceNameLabel.AutoSize = True
		Me.resourceNameLabel.Location = New System.Drawing.Point(12, 11)
		Me.resourceNameLabel.Name = "resourceNameLabel"
		Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
		Me.resourceNameLabel.TabIndex = 12
		Me.resourceNameLabel.Text = "Resource Name"
		' 
		' acquireButton
		' 
		Me.acquireButton.Location = New System.Drawing.Point(12, 460)
		Me.acquireButton.Name = "acquireButton"
		Me.acquireButton.Size = New System.Drawing.Size(149, 23)
		Me.acquireButton.TabIndex = 9
		Me.acquireButton.Text = "&Acquire"
		Me.acquireButton.UseVisualStyleBackColor = True		
		' 
		' multiDataGridViewResults
		' 
		Me.multiDataGridViewResults.Location = New System.Drawing.Point(200, 18)
		Me.multiDataGridViewResults.Name = "multiDataGridViewResults"
		Me.multiDataGridViewResults.Size = New System.Drawing.Size(442, 466)
		Me.multiDataGridViewResults.TabIndex = 41
		' 
		' MainForm
		' 
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(654, 496)
		Me.Controls.Add(Me.multiDataGridViewResults)
		Me.Controls.Add(Me.acquireButton)
		Me.Controls.Add(Me.resourceNameComboBox)
		Me.Controls.Add(Me.resourceNameLabel)
		Me.Controls.Add(Me.timerEventIntervalLabel)
		Me.Controls.Add(Me.referenceLevelRampStrtLabel)
		Me.Controls.Add(Me.referenceLevelRampStopLabel)
		Me.Controls.Add(Me.iqRateLabel)
		Me.Controls.Add(Me.samplesPerRecordLabel)
		Me.Controls.Add(Me.numberOfStepsLabel)
		Me.Controls.Add(Me.carrierFrequencyRampStrtLabel)
		Me.Controls.Add(Me.carrierFrequencyRampStopLabel)
		Me.Controls.Add(Me.timerEventIntervalNumeric)
		Me.Controls.Add(Me.referenceLevelRampStrtNumeric)
		Me.Controls.Add(Me.referenceLevelRampStopNumeric)
		Me.Controls.Add(Me.iqRateNumeric)
		Me.Controls.Add(Me.samplesPerRecordNumeric)
		Me.Controls.Add(Me.numberOfStepsNumeric)
		Me.Controls.Add(Me.carrierFrequencyRampStrtNumeric)
		Me.Controls.Add(Me.carrierFrequencyRampStopNumeric)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.Name = "MainForm"
		Me.Text = "RFSA List Mode Frequency and Power Sweep - Timer Triggered"
        CType(Me.timerEventIntervalNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.referenceLevelRampStrtNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.referenceLevelRampStopNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerRecordNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberOfStepsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.carrierFrequencyRampStrtNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.carrierFrequencyRampStopNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	#End Region

	Private timerEventIntervalLabel As System.Windows.Forms.Label
	Private referenceLevelRampStrtLabel As System.Windows.Forms.Label
	Private referenceLevelRampStopLabel As System.Windows.Forms.Label
	Private iqRateLabel As System.Windows.Forms.Label
	Private samplesPerRecordLabel As System.Windows.Forms.Label
	Private numberOfStepsLabel As System.Windows.Forms.Label
	Private carrierFrequencyRampStrtLabel As System.Windows.Forms.Label
	Private carrierFrequencyRampStopLabel As System.Windows.Forms.Label
	Private timerEventIntervalNumeric As System.Windows.Forms.NumericUpDown
	Private referenceLevelRampStrtNumeric As System.Windows.Forms.NumericUpDown
	Private referenceLevelRampStopNumeric As System.Windows.Forms.NumericUpDown
	Private iqRateNumeric As System.Windows.Forms.NumericUpDown
	Private samplesPerRecordNumeric As System.Windows.Forms.NumericUpDown
	Private numberOfStepsNumeric As System.Windows.Forms.NumericUpDown
	Private carrierFrequencyRampStrtNumeric As System.Windows.Forms.NumericUpDown
	Private carrierFrequencyRampStopNumeric As System.Windows.Forms.NumericUpDown
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private resourceNameLabel As System.Windows.Forms.Label
    Private WithEvents acquireButton As System.Windows.Forms.Button
	Private multiDataGridViewResults As NationalInstruments.Examples.ListModeTimerTriggered.MultiDataGridView
End Class
