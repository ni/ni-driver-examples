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
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.rfsaResourceNameLabel = New System.Windows.Forms.Label()
        Me.referenceLevelLabel = New System.Windows.Forms.Label()
        Me.carrierFrequencyLabel = New System.Windows.Forms.Label()
        Me.referenceLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.carrierFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.referenceClockLabel = New System.Windows.Forms.Label()
        Me.referenceClockComboBox = New System.Windows.Forms.ComboBox()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.samplesPerRecordLabel = New System.Windows.Forms.Label()
        Me.samplesPerRecordNumeric = New System.Windows.Forms.NumericUpDown()
        Me.triggerTypeLabel = New System.Windows.Forms.Label()
        Me.pretriggerSamplesLabel = New System.Windows.Forms.Label()
        Me.pretriggerSamplesNumeric = New System.Windows.Forms.NumericUpDown()
        Me.referenceTriggerTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerLevelLabel = New System.Windows.Forms.Label()
        Me.triggerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.sendSoftwareTriggerbutton = New System.Windows.Forms.Button()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.triggerSourceLabel = New System.Windows.Forms.Label()
        Me.triggerSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.numberOfRecordsLabel = New System.Windows.Forms.Label()
        Me.numberOfRecordsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.multiDataGridViewResults = New NationalInstruments.Examples.GettingStartedMultiRecordIQ.MultiDataGridView()
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.carrierFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerRecordNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pretriggerSamplesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberOfRecordsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(18, 35)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(116, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'rfsaResourceNameLabel
        '
        Me.rfsaResourceNameLabel.AutoSize = True
        Me.rfsaResourceNameLabel.Location = New System.Drawing.Point(18, 15)
        Me.rfsaResourceNameLabel.Name = "rfsaResourceNameLabel"
        Me.rfsaResourceNameLabel.Size = New System.Drawing.Size(115, 13)
        Me.rfsaResourceNameLabel.TabIndex = 23
        Me.rfsaResourceNameLabel.Text = "RFSA Resource Name"
        '
        'referenceLevelLabel
        '
        Me.referenceLevelLabel.AutoSize = True
        Me.referenceLevelLabel.Location = New System.Drawing.Point(18, 111)
        Me.referenceLevelLabel.Name = "referenceLevelLabel"
        Me.referenceLevelLabel.Size = New System.Drawing.Size(116, 13)
        Me.referenceLevelLabel.TabIndex = 18
        Me.referenceLevelLabel.Text = "Reference Level (dBm)"
        '
        'carrierFrequencyLabel
        '
        Me.carrierFrequencyLabel.AutoSize = True
        Me.carrierFrequencyLabel.Location = New System.Drawing.Point(18, 158)
        Me.carrierFrequencyLabel.Name = "carrierFrequencyLabel"
        Me.carrierFrequencyLabel.Size = New System.Drawing.Size(112, 13)
        Me.carrierFrequencyLabel.TabIndex = 21
        Me.carrierFrequencyLabel.Text = "Carrier Frequency (Hz)"
        '
        'referenceLevelNumeric
        '
        Me.referenceLevelNumeric.DecimalPlaces = 2
        Me.referenceLevelNumeric.Location = New System.Drawing.Point(18, 131)
        Me.referenceLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.referenceLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.referenceLevelNumeric.Name = "referenceLevelNumeric"
        Me.referenceLevelNumeric.Size = New System.Drawing.Size(116, 20)
        Me.referenceLevelNumeric.TabIndex = 2
        '
        'carrierFrequencyNumeric
        '
        Me.carrierFrequencyNumeric.DecimalPlaces = 2
        Me.carrierFrequencyNumeric.Location = New System.Drawing.Point(18, 178)
        Me.carrierFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.carrierFrequencyNumeric.Name = "carrierFrequencyNumeric"
        Me.carrierFrequencyNumeric.Size = New System.Drawing.Size(116, 20)
        Me.carrierFrequencyNumeric.TabIndex = 3
        Me.carrierFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'referenceClockLabel
        '
        Me.referenceClockLabel.AutoSize = True
        Me.referenceClockLabel.Location = New System.Drawing.Point(18, 63)
        Me.referenceClockLabel.Name = "referenceClockLabel"
        Me.referenceClockLabel.Size = New System.Drawing.Size(87, 13)
        Me.referenceClockLabel.TabIndex = 24
        Me.referenceClockLabel.Text = "Reference Clock"
        '
        'referenceClockComboBox
        '
        Me.referenceClockComboBox.Location = New System.Drawing.Point(18, 83)
        Me.referenceClockComboBox.Name = "referenceClockComboBox"
        Me.referenceClockComboBox.Size = New System.Drawing.Size(116, 21)
        Me.referenceClockComboBox.TabIndex = 1
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(18, 205)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(44, 13)
        Me.iqRateLabel.TabIndex = 26
        Me.iqRateLabel.Text = "IQ Rate"
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 2
        Me.iqRateNumeric.Location = New System.Drawing.Point(18, 225)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(116, 20)
        Me.iqRateNumeric.TabIndex = 4
        Me.iqRateNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'samplesPerRecordLabel
        '
        Me.samplesPerRecordLabel.AutoSize = True
        Me.samplesPerRecordLabel.Location = New System.Drawing.Point(18, 319)
        Me.samplesPerRecordLabel.Name = "samplesPerRecordLabel"
        Me.samplesPerRecordLabel.Size = New System.Drawing.Size(103, 13)
        Me.samplesPerRecordLabel.TabIndex = 28
        Me.samplesPerRecordLabel.Text = "Samples per Record"
        '
        'samplesPerRecordNumeric
        '
        Me.samplesPerRecordNumeric.Location = New System.Drawing.Point(18, 339)
        Me.samplesPerRecordNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.samplesPerRecordNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerRecordNumeric.Name = "samplesPerRecordNumeric"
        Me.samplesPerRecordNumeric.Size = New System.Drawing.Size(116, 20)
        Me.samplesPerRecordNumeric.TabIndex = 6
        Me.samplesPerRecordNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'triggerTypeLabel
        '
        Me.triggerTypeLabel.AutoSize = True
        Me.triggerTypeLabel.Location = New System.Drawing.Point(18, 366)
        Me.triggerTypeLabel.Name = "triggerTypeLabel"
        Me.triggerTypeLabel.Size = New System.Drawing.Size(120, 13)
        Me.triggerTypeLabel.TabIndex = 30
        Me.triggerTypeLabel.Text = "Reference Trigger Type"
        '
        'pretriggerSamplesLabel
        '
        Me.pretriggerSamplesLabel.AutoSize = True
        Me.pretriggerSamplesLabel.Location = New System.Drawing.Point(18, 414)
        Me.pretriggerSamplesLabel.Name = "pretriggerSamplesLabel"
        Me.pretriggerSamplesLabel.Size = New System.Drawing.Size(95, 13)
        Me.pretriggerSamplesLabel.TabIndex = 32
        Me.pretriggerSamplesLabel.Text = "Pretrigger Samples"
        '
        'pretriggerSamplesNumeric
        '
        Me.pretriggerSamplesNumeric.Location = New System.Drawing.Point(18, 434)
        Me.pretriggerSamplesNumeric.Maximum = New Decimal(New Integer() {-1, 0, 0, 0})
        Me.pretriggerSamplesNumeric.Name = "pretriggerSamplesNumeric"
        Me.pretriggerSamplesNumeric.Size = New System.Drawing.Size(116, 20)
        Me.pretriggerSamplesNumeric.TabIndex = 8
        '
        'referenceTriggerTypeComboBox
        '
        Me.referenceTriggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.referenceTriggerTypeComboBox.Location = New System.Drawing.Point(18, 386)
        Me.referenceTriggerTypeComboBox.Name = "referenceTriggerTypeComboBox"
        Me.referenceTriggerTypeComboBox.Size = New System.Drawing.Size(116, 21)
        Me.referenceTriggerTypeComboBox.TabIndex = 7
        '
        'triggerLevelLabel
        '
        Me.triggerLevelLabel.AutoSize = True
        Me.triggerLevelLabel.Location = New System.Drawing.Point(18, 461)
        Me.triggerLevelLabel.Name = "triggerLevelLabel"
        Me.triggerLevelLabel.Size = New System.Drawing.Size(99, 13)
        Me.triggerLevelLabel.TabIndex = 36
        Me.triggerLevelLabel.Text = "Trigger Level (dBm)"
        '
        'triggerLevelNumeric
        '
        Me.triggerLevelNumeric.DecimalPlaces = 2
        Me.triggerLevelNumeric.Location = New System.Drawing.Point(18, 481)
        Me.triggerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.triggerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.triggerLevelNumeric.Name = "triggerLevelNumeric"
        Me.triggerLevelNumeric.Size = New System.Drawing.Size(116, 20)
        Me.triggerLevelNumeric.TabIndex = 8
        '
        'sendSoftwareTriggerbutton
        '
        Me.sendSoftwareTriggerbutton.Enabled = False
        Me.sendSoftwareTriggerbutton.Location = New System.Drawing.Point(153, 545)
        Me.sendSoftwareTriggerbutton.Name = "sendSoftwareTriggerbutton"
        Me.sendSoftwareTriggerbutton.Size = New System.Drawing.Size(140, 30)
        Me.sendSoftwareTriggerbutton.TabIndex = 11
        Me.sendSoftwareTriggerbutton.Text = "&Send Software Trigger"
        Me.sendSoftwareTriggerbutton.UseVisualStyleBackColor = True
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(17, 545)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(116, 30)
        Me.acquireButton.TabIndex = 9
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.AutoSize = True
        Me.triggerSourceLabel.Location = New System.Drawing.Point(18, 461)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(77, 13)
        Me.triggerSourceLabel.TabIndex = 37
        Me.triggerSourceLabel.Text = "Trigger Source"
        '
        'triggerSourceComboBox
        '
        Me.triggerSourceComboBox.Location = New System.Drawing.Point(18, 481)
        Me.triggerSourceComboBox.Name = "triggerSourceComboBox"
        Me.triggerSourceComboBox.Size = New System.Drawing.Size(116, 21)
        Me.triggerSourceComboBox.TabIndex = 38
        '
        'numberOfRecordsLabel
        '
        Me.numberOfRecordsLabel.AutoSize = True
        Me.numberOfRecordsLabel.Location = New System.Drawing.Point(18, 263)
        Me.numberOfRecordsLabel.Name = "numberOfRecordsLabel"
        Me.numberOfRecordsLabel.Size = New System.Drawing.Size(99, 13)
        Me.numberOfRecordsLabel.TabIndex = 40
        Me.numberOfRecordsLabel.Text = "Number of Records"
        '
        'numberOfRecordsNumeric
        '
        Me.numberOfRecordsNumeric.Location = New System.Drawing.Point(18, 283)
        Me.numberOfRecordsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numberOfRecordsNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numberOfRecordsNumeric.Name = "numberOfRecordsNumeric"
        Me.numberOfRecordsNumeric.Size = New System.Drawing.Size(116, 20)
        Me.numberOfRecordsNumeric.TabIndex = 5
        Me.numberOfRecordsNumeric.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'multiDataGridViewResults
        '
        Me.multiDataGridViewResults.Location = New System.Drawing.Point(164, 15)
        Me.multiDataGridViewResults.Name = "multiDataGridViewResults"
        Me.multiDataGridViewResults.Size = New System.Drawing.Size(442, 466)
        Me.multiDataGridViewResults.TabIndex = 41
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(618, 605)
        Me.Controls.Add(Me.multiDataGridViewResults)
        Me.Controls.Add(Me.numberOfRecordsLabel)
        Me.Controls.Add(Me.numberOfRecordsNumeric)
        Me.Controls.Add(Me.triggerSourceLabel)
        Me.Controls.Add(Me.triggerSourceComboBox)
        Me.Controls.Add(Me.sendSoftwareTriggerbutton)
        Me.Controls.Add(Me.acquireButton)
        Me.Controls.Add(Me.triggerLevelLabel)
        Me.Controls.Add(Me.triggerLevelNumeric)
        Me.Controls.Add(Me.triggerTypeLabel)
        Me.Controls.Add(Me.pretriggerSamplesLabel)
        Me.Controls.Add(Me.pretriggerSamplesNumeric)
        Me.Controls.Add(Me.referenceTriggerTypeComboBox)
        Me.Controls.Add(Me.samplesPerRecordLabel)
        Me.Controls.Add(Me.samplesPerRecordNumeric)
        Me.Controls.Add(Me.iqRateLabel)
        Me.Controls.Add(Me.iqRateNumeric)
        Me.Controls.Add(Me.referenceClockLabel)
        Me.Controls.Add(Me.referenceClockComboBox)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.rfsaResourceNameLabel)
        Me.Controls.Add(Me.referenceLevelLabel)
        Me.Controls.Add(Me.carrierFrequencyLabel)
        Me.Controls.Add(Me.referenceLevelNumeric)
        Me.Controls.Add(Me.carrierFrequencyNumeric)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Getting Started MultiRecord IQ"
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.carrierFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerRecordNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pretriggerSamplesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberOfRecordsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private rfsaResourceNameLabel As System.Windows.Forms.Label
    Private referenceLevelLabel As System.Windows.Forms.Label
    Private carrierFrequencyLabel As System.Windows.Forms.Label
    Private referenceLevelNumeric As System.Windows.Forms.NumericUpDown
    Private carrierFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private referenceClockLabel As System.Windows.Forms.Label
    Private referenceClockComboBox As System.Windows.Forms.ComboBox
    Private iqRateLabel As System.Windows.Forms.Label
    Private iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private samplesPerRecordLabel As System.Windows.Forms.Label
    Private samplesPerRecordNumeric As System.Windows.Forms.NumericUpDown
    Private triggerTypeLabel As System.Windows.Forms.Label
    Private pretriggerSamplesLabel As System.Windows.Forms.Label
    Private pretriggerSamplesNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents referenceTriggerTypeComboBox As System.Windows.Forms.ComboBox
    Private triggerLevelLabel As System.Windows.Forms.Label
    Private triggerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents sendSoftwareTriggerbutton As System.Windows.Forms.Button
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private triggerSourceLabel As System.Windows.Forms.Label
    Private triggerSourceComboBox As System.Windows.Forms.ComboBox
    Private numberOfRecordsLabel As System.Windows.Forms.Label
    Private numberOfRecordsNumeric As System.Windows.Forms.NumericUpDown
    Private multiDataGridViewResults As NationalInstruments.Examples.GettingStartedMultiRecordIQ.MultiDataGridView
End Class

