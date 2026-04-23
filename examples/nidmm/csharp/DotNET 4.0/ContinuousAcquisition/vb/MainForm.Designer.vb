
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.minACFrequencyTextBox = New System.Windows.Forms.TextBox()
        Me.maxACFrequencyTextBox = New System.Windows.Forms.TextBox()
        Me.acConfigurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.maxACFrequencyLabel = New System.Windows.Forms.Label()
        Me.minACFrequencyLabel = New System.Windows.Forms.Label()
        Me.ResourceAndMeasurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementTypeLabel = New System.Windows.Forms.Label()
        Me.measurementModeComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.numberOfSamplesTextBox = New System.Windows.Forms.TextBox()
        Me.numberOfSamplesLabel = New System.Windows.Forms.Label()
        Me.maxReadingTextBox = New System.Windows.Forms.TextBox()
        Me.minReadingTextBox = New System.Windows.Forms.TextBox()
        Me.minReadingLabel = New System.Windows.Forms.Label()
        Me.maxReadingLabel = New System.Windows.Forms.Label()
        Me.buttonsPanel = New System.Windows.Forms.Panel()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.averageReadingLabel = New System.Windows.Forms.Label()
        Me.averageReadingTextBox = New System.Windows.Forms.TextBox()
        Me.actualRangeLabel = New System.Windows.Forms.Label()
        Me.actualRangeTextBox = New System.Windows.Forms.TextBox()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.TextBox()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.samplesPerReadingNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.samplesPerReadingLabel = New System.Windows.Forms.Label()
        Me.rangeTextBox = New System.Windows.Forms.TextBox()
        Me.powerlineFrequencyLabel = New System.Windows.Forms.Label()
        Me.resolutionLabel = New System.Windows.Forms.Label()
        Me.rangeLabel = New System.Windows.Forms.Label()
        Me.powerlineFrequencyValueComboBox = New System.Windows.Forms.ComboBox()
        Me.resolutionValueComboBox = New System.Windows.Forms.ComboBox()
        Me.acConfigurationGroupBox.SuspendLayout()
        Me.ResourceAndMeasurementGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.buttonsPanel.SuspendLayout()
        Me.messageGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.samplesPerReadingNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(151, 22)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(118, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'minACFrequencyTextBox
        '
        Me.minACFrequencyTextBox.Location = New System.Drawing.Point(162, 16)
        Me.minACFrequencyTextBox.Name = "minACFrequencyTextBox"
        Me.minACFrequencyTextBox.Size = New System.Drawing.Size(107, 20)
        Me.minACFrequencyTextBox.TabIndex = 0
        Me.minACFrequencyTextBox.Text = "20.00E+0"
        '
        'maxACFrequencyTextBox
        '
        Me.maxACFrequencyTextBox.Location = New System.Drawing.Point(162, 49)
        Me.maxACFrequencyTextBox.Name = "maxACFrequencyTextBox"
        Me.maxACFrequencyTextBox.Size = New System.Drawing.Size(107, 20)
        Me.maxACFrequencyTextBox.TabIndex = 1
        Me.maxACFrequencyTextBox.Text = "25.00E+03"
        '
        'acConfigurationGroupBox
        '
        Me.acConfigurationGroupBox.Controls.Add(Me.maxACFrequencyTextBox)
        Me.acConfigurationGroupBox.Controls.Add(Me.minACFrequencyTextBox)
        Me.acConfigurationGroupBox.Controls.Add(Me.maxACFrequencyLabel)
        Me.acConfigurationGroupBox.Controls.Add(Me.minACFrequencyLabel)
        Me.acConfigurationGroupBox.Enabled = False
        Me.acConfigurationGroupBox.Location = New System.Drawing.Point(2, 251)
        Me.acConfigurationGroupBox.Name = "acConfigurationGroupBox"
        Me.acConfigurationGroupBox.Size = New System.Drawing.Size(275, 80)
        Me.acConfigurationGroupBox.TabIndex = 2
        Me.acConfigurationGroupBox.TabStop = False
        Me.acConfigurationGroupBox.Text = "AC Configuration"
        '
        'maxACFrequencyLabel
        '
        Me.maxACFrequencyLabel.AutoSize = True
        Me.maxACFrequencyLabel.Location = New System.Drawing.Point(6, 51)
        Me.maxACFrequencyLabel.Name = "maxACFrequencyLabel"
        Me.maxACFrequencyLabel.Size = New System.Drawing.Size(146, 13)
        Me.maxACFrequencyLabel.TabIndex = 3
        Me.maxACFrequencyLabel.Text = "Maximum AC Frequency (Hz):"
        '
        'minACFrequencyLabel
        '
        Me.minACFrequencyLabel.AutoSize = True
        Me.minACFrequencyLabel.Location = New System.Drawing.Point(6, 19)
        Me.minACFrequencyLabel.Name = "minACFrequencyLabel"
        Me.minACFrequencyLabel.Size = New System.Drawing.Size(143, 13)
        Me.minACFrequencyLabel.TabIndex = 2
        Me.minACFrequencyLabel.Text = "Minimum AC Frequency (Hz):"
        '
        'ResourceAndMeasurementGroupBox
        '
        Me.ResourceAndMeasurementGroupBox.Controls.Add(Me.measurementTypeLabel)
        Me.ResourceAndMeasurementGroupBox.Controls.Add(Me.measurementModeComboBox)
        Me.ResourceAndMeasurementGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.ResourceAndMeasurementGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.ResourceAndMeasurementGroupBox.Location = New System.Drawing.Point(2, 3)
        Me.ResourceAndMeasurementGroupBox.Name = "ResourceAndMeasurementGroupBox"
        Me.ResourceAndMeasurementGroupBox.Size = New System.Drawing.Size(275, 93)
        Me.ResourceAndMeasurementGroupBox.TabIndex = 0
        Me.ResourceAndMeasurementGroupBox.TabStop = False
        Me.ResourceAndMeasurementGroupBox.Text = "Resource Name and Measurement Type"
        '
        'measurementTypeLabel
        '
        Me.measurementTypeLabel.AutoSize = True
        Me.measurementTypeLabel.Location = New System.Drawing.Point(6, 59)
        Me.measurementTypeLabel.Name = "measurementTypeLabel"
        Me.measurementTypeLabel.Size = New System.Drawing.Size(104, 13)
        Me.measurementTypeLabel.TabIndex = 3
        Me.measurementTypeLabel.Text = "Measurement Mode:"
        '
        'measurementModeComboBox
        '
        Me.measurementModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.measurementModeComboBox.FormattingEnabled = True
        Me.measurementModeComboBox.Location = New System.Drawing.Point(151, 56)
        Me.measurementModeComboBox.Name = "measurementModeComboBox"
        Me.measurementModeComboBox.Size = New System.Drawing.Size(118, 21)
        Me.measurementModeComboBox.TabIndex = 1
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 25)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 1
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.measurementGroupBox.Controls.Add(Me.numberOfSamplesTextBox)
        Me.measurementGroupBox.Controls.Add(Me.numberOfSamplesLabel)
        Me.measurementGroupBox.Controls.Add(Me.maxReadingTextBox)
        Me.measurementGroupBox.Controls.Add(Me.minReadingTextBox)
        Me.measurementGroupBox.Controls.Add(Me.minReadingLabel)
        Me.measurementGroupBox.Controls.Add(Me.maxReadingLabel)
        Me.measurementGroupBox.Controls.Add(Me.buttonsPanel)
        Me.measurementGroupBox.Controls.Add(Me.averageReadingLabel)
        Me.measurementGroupBox.Controls.Add(Me.averageReadingTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualRangeLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualRangeTextBox)
        Me.measurementGroupBox.Location = New System.Drawing.Point(284, 4)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(262, 240)
        Me.measurementGroupBox.TabIndex = 3
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'numberOfSamplesTextBox
        '
        Me.numberOfSamplesTextBox.Location = New System.Drawing.Point(154, 114)
        Me.numberOfSamplesTextBox.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.numberOfSamplesTextBox.Name = "numberOfSamplesTextBox"
        Me.numberOfSamplesTextBox.ReadOnly = True
        Me.numberOfSamplesTextBox.Size = New System.Drawing.Size(100, 20)
        Me.numberOfSamplesTextBox.TabIndex = 3
        '
        'numberOfSamplesLabel
        '
        Me.numberOfSamplesLabel.AutoSize = True
        Me.numberOfSamplesLabel.Location = New System.Drawing.Point(6, 116)
        Me.numberOfSamplesLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.numberOfSamplesLabel.Name = "numberOfSamplesLabel"
        Me.numberOfSamplesLabel.Size = New System.Drawing.Size(102, 13)
        Me.numberOfSamplesLabel.TabIndex = 11
        Me.numberOfSamplesLabel.Text = "Number of Samples:"
        '
        'maxReadingTextBox
        '
        Me.maxReadingTextBox.Location = New System.Drawing.Point(154, 82)
        Me.maxReadingTextBox.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.maxReadingTextBox.Name = "maxReadingTextBox"
        Me.maxReadingTextBox.ReadOnly = True
        Me.maxReadingTextBox.Size = New System.Drawing.Size(100, 20)
        Me.maxReadingTextBox.TabIndex = 2
        '
        'minReadingTextBox
        '
        Me.minReadingTextBox.Location = New System.Drawing.Point(154, 50)
        Me.minReadingTextBox.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.minReadingTextBox.Name = "minReadingTextBox"
        Me.minReadingTextBox.ReadOnly = True
        Me.minReadingTextBox.Size = New System.Drawing.Size(100, 20)
        Me.minReadingTextBox.TabIndex = 1
        '
        'minReadingLabel
        '
        Me.minReadingLabel.AutoSize = True
        Me.minReadingLabel.Location = New System.Drawing.Point(6, 53)
        Me.minReadingLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.minReadingLabel.Name = "minReadingLabel"
        Me.minReadingLabel.Size = New System.Drawing.Size(70, 13)
        Me.minReadingLabel.TabIndex = 9
        Me.minReadingLabel.Text = "Min Reading:"
        '
        'maxReadingLabel
        '
        Me.maxReadingLabel.AutoSize = True
        Me.maxReadingLabel.Location = New System.Drawing.Point(6, 84)
        Me.maxReadingLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.maxReadingLabel.Name = "maxReadingLabel"
        Me.maxReadingLabel.Size = New System.Drawing.Size(73, 13)
        Me.maxReadingLabel.TabIndex = 9
        Me.maxReadingLabel.Text = "Max Reading:"
        '
        'buttonsPanel
        '
        Me.buttonsPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.buttonsPanel.Controls.Add(Me.acquireButton)
        Me.buttonsPanel.Controls.Add(Me.stopButton)
        Me.buttonsPanel.Location = New System.Drawing.Point(8, 190)
        Me.buttonsPanel.Name = "buttonsPanel"
        Me.buttonsPanel.Size = New System.Drawing.Size(247, 35)
        Me.buttonsPanel.TabIndex = 5
        '
        'acquireButton
        '
        Me.acquireButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.acquireButton.Location = New System.Drawing.Point(24, 11)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(78, 21)
        Me.acquireButton.TabIndex = 0
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(147, 11)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(78, 21)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'averageReadingLabel
        '
        Me.averageReadingLabel.AutoSize = True
        Me.averageReadingLabel.Location = New System.Drawing.Point(6, 21)
        Me.averageReadingLabel.Name = "averageReadingLabel"
        Me.averageReadingLabel.Size = New System.Drawing.Size(93, 13)
        Me.averageReadingLabel.TabIndex = 2
        Me.averageReadingLabel.Text = "Average Reading:"
        '
        'averageReadingTextBox
        '
        Me.averageReadingTextBox.Location = New System.Drawing.Point(154, 19)
        Me.averageReadingTextBox.Name = "averageReadingTextBox"
        Me.averageReadingTextBox.ReadOnly = True
        Me.averageReadingTextBox.Size = New System.Drawing.Size(100, 20)
        Me.averageReadingTextBox.TabIndex = 0
        '
        'actualRangeLabel
        '
        Me.actualRangeLabel.AutoSize = True
        Me.actualRangeLabel.Location = New System.Drawing.Point(6, 148)
        Me.actualRangeLabel.Name = "actualRangeLabel"
        Me.actualRangeLabel.Size = New System.Drawing.Size(75, 13)
        Me.actualRangeLabel.TabIndex = 8
        Me.actualRangeLabel.Text = "Actual Range:"
        '
        'actualRangeTextBox
        '
        Me.actualRangeTextBox.Location = New System.Drawing.Point(154, 145)
        Me.actualRangeTextBox.Name = "actualRangeTextBox"
        Me.actualRangeTextBox.ReadOnly = True
        Me.actualRangeTextBox.Size = New System.Drawing.Size(100, 20)
        Me.actualRangeTextBox.TabIndex = 4
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(284, 251)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(262, 81)
        Me.messageGroupBox.TabIndex = 4
        Me.messageGroupBox.TabStop = False
        Me.messageGroupBox.Text = "Message"
        '
        'messageTextBox
        '
        Me.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.messageTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.messageTextBox.Location = New System.Drawing.Point(3, 14)
        Me.messageTextBox.Multiline = True
        Me.messageTextBox.Name = "messageTextBox"
        Me.messageTextBox.ReadOnly = True
        Me.messageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.messageTextBox.Size = New System.Drawing.Size(256, 64)
        Me.messageTextBox.TabIndex = 0
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.samplesPerReadingNumericUpDown)
        Me.configurationGroupBox.Controls.Add(Me.samplesPerReadingLabel)
        Me.configurationGroupBox.Controls.Add(Me.rangeTextBox)
        Me.configurationGroupBox.Controls.Add(Me.powerlineFrequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.resolutionLabel)
        Me.configurationGroupBox.Controls.Add(Me.rangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.powerlineFrequencyValueComboBox)
        Me.configurationGroupBox.Controls.Add(Me.resolutionValueComboBox)
        Me.configurationGroupBox.Location = New System.Drawing.Point(2, 102)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(275, 141)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'samplesPerReadingNumericUpDown
        '
        Me.samplesPerReadingNumericUpDown.Location = New System.Drawing.Point(151, 110)
        Me.samplesPerReadingNumericUpDown.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.samplesPerReadingNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.samplesPerReadingNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerReadingNumericUpDown.Name = "samplesPerReadingNumericUpDown"
        Me.samplesPerReadingNumericUpDown.Size = New System.Drawing.Size(117, 20)
        Me.samplesPerReadingNumericUpDown.TabIndex = 10
        Me.samplesPerReadingNumericUpDown.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'samplesPerReadingLabel
        '
        Me.samplesPerReadingLabel.AutoSize = True
        Me.samplesPerReadingLabel.Location = New System.Drawing.Point(6, 112)
        Me.samplesPerReadingLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.samplesPerReadingLabel.Name = "samplesPerReadingLabel"
        Me.samplesPerReadingLabel.Size = New System.Drawing.Size(111, 13)
        Me.samplesPerReadingLabel.TabIndex = 8
        Me.samplesPerReadingLabel.Text = "Samples per Reading:"
        '
        'rangeTextBox
        '
        Me.rangeTextBox.Location = New System.Drawing.Point(151, 19)
        Me.rangeTextBox.Name = "rangeTextBox"
        Me.rangeTextBox.Size = New System.Drawing.Size(118, 20)
        Me.rangeTextBox.TabIndex = 0
        Me.rangeTextBox.Text = "10"
        '
        'powerlineFrequencyLabel
        '
        Me.powerlineFrequencyLabel.AutoSize = True
        Me.powerlineFrequencyLabel.Location = New System.Drawing.Point(6, 81)
        Me.powerlineFrequencyLabel.Name = "powerlineFrequencyLabel"
        Me.powerlineFrequencyLabel.Size = New System.Drawing.Size(131, 13)
        Me.powerlineFrequencyLabel.TabIndex = 7
        Me.powerlineFrequencyLabel.Text = "Powerline Frequency (Hz):"
        '
        'resolutionLabel
        '
        Me.resolutionLabel.AutoSize = True
        Me.resolutionLabel.Location = New System.Drawing.Point(6, 50)
        Me.resolutionLabel.Name = "resolutionLabel"
        Me.resolutionLabel.Size = New System.Drawing.Size(95, 13)
        Me.resolutionLabel.TabIndex = 6
        Me.resolutionLabel.Text = "Resolution (Digits):"
        '
        'rangeLabel
        '
        Me.rangeLabel.AutoSize = True
        Me.rangeLabel.Location = New System.Drawing.Point(6, 21)
        Me.rangeLabel.Name = "rangeLabel"
        Me.rangeLabel.Size = New System.Drawing.Size(42, 13)
        Me.rangeLabel.TabIndex = 5
        Me.rangeLabel.Text = "Range:"
        '
        'powerlineFrequencyValueComboBox
        '
        Me.powerlineFrequencyValueComboBox.FormattingEnabled = True
        Me.powerlineFrequencyValueComboBox.Location = New System.Drawing.Point(151, 78)
        Me.powerlineFrequencyValueComboBox.Name = "powerlineFrequencyValueComboBox"
        Me.powerlineFrequencyValueComboBox.Size = New System.Drawing.Size(118, 21)
        Me.powerlineFrequencyValueComboBox.TabIndex = 2
        '
        'resolutionValueComboBox
        '
        Me.resolutionValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resolutionValueComboBox.FormattingEnabled = True
        Me.resolutionValueComboBox.Location = New System.Drawing.Point(151, 47)
        Me.resolutionValueComboBox.Name = "resolutionValueComboBox"
        Me.resolutionValueComboBox.Size = New System.Drawing.Size(118, 21)
        Me.resolutionValueComboBox.TabIndex = 1
        '
        'MainForm
        '
        Me.AcceptButton = Me.acquireButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.stopButton
        Me.ClientSize = New System.Drawing.Size(552, 344)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.acConfigurationGroupBox)
        Me.Controls.Add(Me.ResourceAndMeasurementGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(558, 372)
        Me.MinimumSize = New System.Drawing.Size(558, 372)
        Me.Name = "MainForm"
        Me.Text = "Continuous Acquisition"
        Me.acConfigurationGroupBox.ResumeLayout(False)
        Me.acConfigurationGroupBox.PerformLayout()
        Me.ResourceAndMeasurementGroupBox.ResumeLayout(False)
        Me.ResourceAndMeasurementGroupBox.PerformLayout()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.buttonsPanel.ResumeLayout(False)
        Me.messageGroupBox.ResumeLayout(False)
        Me.messageGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.samplesPerReadingNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents resourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents minACFrequencyTextBox As System.Windows.Forms.TextBox
    Private WithEvents maxACFrequencyTextBox As System.Windows.Forms.TextBox
    Private WithEvents acConfigurationGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents maxACFrequencyLabel As System.Windows.Forms.Label
    Private WithEvents minACFrequencyLabel As System.Windows.Forms.Label
    Private WithEvents ResourceAndMeasurementGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents measurementTypeLabel As System.Windows.Forms.Label
    Private WithEvents measurementModeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents resourceNameLabel As System.Windows.Forms.Label
    Private WithEvents measurementGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents numberOfSamplesTextBox As System.Windows.Forms.TextBox
    Private WithEvents numberOfSamplesLabel As System.Windows.Forms.Label
    Private WithEvents maxReadingTextBox As System.Windows.Forms.TextBox
    Private WithEvents minReadingTextBox As System.Windows.Forms.TextBox
    Private WithEvents minReadingLabel As System.Windows.Forms.Label
    Private WithEvents maxReadingLabel As System.Windows.Forms.Label
    Private WithEvents buttonsPanel As System.Windows.Forms.Panel
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents averageReadingLabel As System.Windows.Forms.Label
    Private WithEvents averageReadingTextBox As System.Windows.Forms.TextBox
    Private WithEvents actualRangeLabel As System.Windows.Forms.Label
    Private WithEvents actualRangeTextBox As System.Windows.Forms.TextBox
    Private WithEvents messageGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents messageTextBox As System.Windows.Forms.TextBox
    Private WithEvents configurationGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents samplesPerReadingLabel As System.Windows.Forms.Label
    Private WithEvents rangeTextBox As System.Windows.Forms.TextBox
    Private WithEvents powerlineFrequencyLabel As System.Windows.Forms.Label
    Private WithEvents resolutionLabel As System.Windows.Forms.Label
    Private WithEvents rangeLabel As System.Windows.Forms.Label
    Private WithEvents powerlineFrequencyValueComboBox As System.Windows.Forms.ComboBox
    Private WithEvents resolutionValueComboBox As System.Windows.Forms.ComboBox
    Private WithEvents samplesPerReadingNumericUpDown As System.Windows.Forms.NumericUpDown

End Class


