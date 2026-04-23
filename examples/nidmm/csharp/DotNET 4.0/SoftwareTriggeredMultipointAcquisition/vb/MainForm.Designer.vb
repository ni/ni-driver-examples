Imports System.Windows.Forms
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
        Me.measurementModeLabel = New System.Windows.Forms.Label()
        Me.readButton = New System.Windows.Forms.Button()
        Me.clearButton = New System.Windows.Forms.Button()
        Me.buttonsPanel = New System.Windows.Forms.Panel()
        Me.softwareTriggerButton = New System.Windows.Forms.Button()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.numberOfMeasurementsNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.actualRangeTextBox = New System.Windows.Forms.TextBox()
        Me.actualRangeLabel = New System.Windows.Forms.Label()
        Me.readingDataGridView = New System.Windows.Forms.DataGridView()
        Me.ReadingNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.reading = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.numberOfMeasurementsLabel = New System.Windows.Forms.Label()
        Me.triggerDelayNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.triggerGroupBox = New System.Windows.Forms.GroupBox()
        Me.triggerDelayLabel = New System.Windows.Forms.Label()
        Me.triggerSourceLabel = New System.Windows.Forms.Label()
        Me.triggerSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.messageTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.rangeNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.powerlineFrequencyLabel = New System.Windows.Forms.Label()
        Me.resolutionLabel = New System.Windows.Forms.Label()
        Me.rangeLabel = New System.Windows.Forms.Label()
        Me.powerlineFrequencyValueComboBox = New System.Windows.Forms.ComboBox()
        Me.resolutionValueComboBox = New System.Windows.Forms.ComboBox()
        Me.measurementModeComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceAndMeasurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.errorGroupBox = New System.Windows.Forms.GroupBox()
        Me.buttonsPanel.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        CType(Me.numberOfMeasurementsNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.readingDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.triggerDelayNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.triggerGroupBox.SuspendLayout()
        CType(Me.rangeNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.configurationGroupBox.SuspendLayout()
        Me.resourceAndMeasurementGroupBox.SuspendLayout()
        Me.errorGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'measurementModeLabel
        '
        Me.measurementModeLabel.AutoSize = True
        Me.measurementModeLabel.Location = New System.Drawing.Point(10, 55)
        Me.measurementModeLabel.Name = "measurementModeLabel"
        Me.measurementModeLabel.Size = New System.Drawing.Size(104, 13)
        Me.measurementModeLabel.TabIndex = 3
        Me.measurementModeLabel.Text = "Measurement Mode:"
        '
        'readButton
        '
        Me.readButton.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.readButton.Location = New System.Drawing.Point(16, 8)
        Me.readButton.Name = "readButton"
        Me.readButton.Size = New System.Drawing.Size(75, 23)
        Me.readButton.TabIndex = 0
        Me.readButton.Text = "&Read"
        Me.readButton.UseVisualStyleBackColor = True
        '
        'clearButton
        '
        Me.clearButton.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.clearButton.Location = New System.Drawing.Point(246, 8)
        Me.clearButton.Name = "clearButton"
        Me.clearButton.Size = New System.Drawing.Size(75, 23)
        Me.clearButton.TabIndex = 2
        Me.clearButton.Text = "&Clear"
        Me.clearButton.UseVisualStyleBackColor = True
        '
        'buttonsPanel
        '
        Me.buttonsPanel.Controls.Add(Me.readButton)
        Me.buttonsPanel.Controls.Add(Me.clearButton)
        Me.buttonsPanel.Controls.Add(Me.softwareTriggerButton)
        Me.buttonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.buttonsPanel.Location = New System.Drawing.Point(3, 543)
        Me.buttonsPanel.Name = "buttonsPanel"
        Me.buttonsPanel.Size = New System.Drawing.Size(324, 38)
        Me.buttonsPanel.TabIndex = 3
        '
        'softwareTriggerButton
        '
        Me.softwareTriggerButton.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.softwareTriggerButton.Enabled = False
        Me.softwareTriggerButton.Location = New System.Drawing.Point(100, 8)
        Me.softwareTriggerButton.Name = "softwareTriggerButton"
        Me.softwareTriggerButton.Size = New System.Drawing.Size(140, 23)
        Me.softwareTriggerButton.TabIndex = 1
        Me.softwareTriggerButton.Text = "&Send Software Trigger"
        Me.softwareTriggerButton.UseVisualStyleBackColor = True
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.measurementGroupBox.Controls.Add(Me.buttonsPanel)
        Me.measurementGroupBox.Controls.Add(Me.numberOfMeasurementsNumericUpDown)
        Me.measurementGroupBox.Controls.Add(Me.actualRangeTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualRangeLabel)
        Me.measurementGroupBox.Controls.Add(Me.readingDataGridView)
        Me.measurementGroupBox.Controls.Add(Me.numberOfMeasurementsLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(285, 3)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(330, 584)
        Me.measurementGroupBox.TabIndex = 3
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'numberOfMeasurementsNumericUpDown
        '
        Me.numberOfMeasurementsNumericUpDown.Location = New System.Drawing.Point(198, 22)
        Me.numberOfMeasurementsNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.numberOfMeasurementsNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numberOfMeasurementsNumericUpDown.Name = "numberOfMeasurementsNumericUpDown"
        Me.numberOfMeasurementsNumericUpDown.Size = New System.Drawing.Size(100, 20)
        Me.numberOfMeasurementsNumericUpDown.TabIndex = 0
        Me.numberOfMeasurementsNumericUpDown.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'actualRangeTextBox
        '
        Me.actualRangeTextBox.Location = New System.Drawing.Point(198, 51)
        Me.actualRangeTextBox.Name = "actualRangeTextBox"
        Me.actualRangeTextBox.ReadOnly = True
        Me.actualRangeTextBox.Size = New System.Drawing.Size(100, 20)
        Me.actualRangeTextBox.TabIndex = 1
        '
        'actualRangeLabel
        '
        Me.actualRangeLabel.AutoSize = True
        Me.actualRangeLabel.Location = New System.Drawing.Point(33, 55)
        Me.actualRangeLabel.Name = "actualRangeLabel"
        Me.actualRangeLabel.Size = New System.Drawing.Size(75, 13)
        Me.actualRangeLabel.TabIndex = 3
        Me.actualRangeLabel.Text = "Actual Range:"
        '
        'readingDataGridView
        '
        Me.readingDataGridView.AllowUserToAddRows = False
        Me.readingDataGridView.AllowUserToDeleteRows = False
        Me.readingDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.readingDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.readingDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ReadingNo, Me.reading})
        Me.readingDataGridView.Location = New System.Drawing.Point(36, 91)
        Me.readingDataGridView.Name = "readingDataGridView"
        Me.readingDataGridView.ReadOnly = True
        Me.readingDataGridView.RowHeadersWidth = 15
        Me.readingDataGridView.RowTemplate.Height = 24
        Me.readingDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.readingDataGridView.Size = New System.Drawing.Size(262, 435)
        Me.readingDataGridView.StandardTab = True
        Me.readingDataGridView.TabIndex = 2
        '
        'ReadingNo
        '
        Me.ReadingNo.HeaderText = "Point"
        Me.ReadingNo.Name = "ReadingNo"
        Me.ReadingNo.ReadOnly = True
        '
        'reading
        '
        Me.reading.HeaderText = "Amplitude"
        Me.reading.Name = "reading"
        Me.reading.ReadOnly = True
        Me.reading.Width = 287
        '
        'numberOfMeasurementsLabel
        '
        Me.numberOfMeasurementsLabel.AutoSize = True
        Me.numberOfMeasurementsLabel.Location = New System.Drawing.Point(33, 26)
        Me.numberOfMeasurementsLabel.Name = "numberOfMeasurementsLabel"
        Me.numberOfMeasurementsLabel.Size = New System.Drawing.Size(133, 13)
        Me.numberOfMeasurementsLabel.TabIndex = 0
        Me.numberOfMeasurementsLabel.Text = "Number Of Measurements:"
        '
        'triggerDelayNumericUpDown
        '
        Me.triggerDelayNumericUpDown.DecimalPlaces = 2
        Me.triggerDelayNumericUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.triggerDelayNumericUpDown.Location = New System.Drawing.Point(151, 54)
        Me.triggerDelayNumericUpDown.Maximum = New Decimal(New Integer() {149, 0, 0, 0})
        Me.triggerDelayNumericUpDown.Name = "triggerDelayNumericUpDown"
        Me.triggerDelayNumericUpDown.Size = New System.Drawing.Size(119, 20)
        Me.triggerDelayNumericUpDown.TabIndex = 1
        '
        'triggerGroupBox
        '
        Me.triggerGroupBox.Controls.Add(Me.triggerDelayNumericUpDown)
        Me.triggerGroupBox.Controls.Add(Me.triggerDelayLabel)
        Me.triggerGroupBox.Controls.Add(Me.triggerSourceLabel)
        Me.triggerGroupBox.Controls.Add(Me.triggerSourceComboBox)
        Me.triggerGroupBox.Location = New System.Drawing.Point(3, 224)
        Me.triggerGroupBox.Name = "triggerGroupBox"
        Me.triggerGroupBox.Size = New System.Drawing.Size(276, 93)
        Me.triggerGroupBox.TabIndex = 2
        Me.triggerGroupBox.TabStop = False
        Me.triggerGroupBox.Text = "Trigger"
        '
        'triggerDelayLabel
        '
        Me.triggerDelayLabel.AutoSize = True
        Me.triggerDelayLabel.Location = New System.Drawing.Point(10, 58)
        Me.triggerDelayLabel.Name = "triggerDelayLabel"
        Me.triggerDelayLabel.Size = New System.Drawing.Size(90, 13)
        Me.triggerDelayLabel.TabIndex = 4
        Me.triggerDelayLabel.Text = "Trigger Delay (s) :"
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.AutoSize = True
        Me.triggerSourceLabel.Location = New System.Drawing.Point(10, 27)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(80, 13)
        Me.triggerSourceLabel.TabIndex = 2
        Me.triggerSourceLabel.Text = "Trigger Source:"
        '
        'triggerSourceComboBox
        '
        Me.triggerSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerSourceComboBox.FormattingEnabled = True
        Me.triggerSourceComboBox.Location = New System.Drawing.Point(150, 23)
        Me.triggerSourceComboBox.Name = "triggerSourceComboBox"
        Me.triggerSourceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.triggerSourceComboBox.TabIndex = 0
        '
        'messageTextBox
        '
        Me.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.messageTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.messageTextBox.Location = New System.Drawing.Point(3, 16)
        Me.messageTextBox.MinimumSize = New System.Drawing.Size(0, 30)
        Me.messageTextBox.Multiline = True
        Me.messageTextBox.Name = "messageTextBox"
        Me.messageTextBox.ReadOnly = True
        Me.messageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.messageTextBox.Size = New System.Drawing.Size(270, 245)
        Me.messageTextBox.TabIndex = 0
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(10, 26)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 2
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'rangeNumericUpDown
        '
        Me.rangeNumericUpDown.DecimalPlaces = 3
        Me.rangeNumericUpDown.Location = New System.Drawing.Point(151, 23)
        Me.rangeNumericUpDown.Maximum = New Decimal(New Integer() {1215752192, 23, 0, 0})
        Me.rangeNumericUpDown.Name = "rangeNumericUpDown"
        Me.rangeNumericUpDown.Size = New System.Drawing.Size(119, 20)
        Me.rangeNumericUpDown.TabIndex = 0
        Me.rangeNumericUpDown.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.rangeNumericUpDown)
        Me.configurationGroupBox.Controls.Add(Me.powerlineFrequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.resolutionLabel)
        Me.configurationGroupBox.Controls.Add(Me.rangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.powerlineFrequencyValueComboBox)
        Me.configurationGroupBox.Controls.Add(Me.resolutionValueComboBox)
        Me.configurationGroupBox.Location = New System.Drawing.Point(3, 94)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(276, 124)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'powerlineFrequencyLabel
        '
        Me.powerlineFrequencyLabel.AutoSize = True
        Me.powerlineFrequencyLabel.Location = New System.Drawing.Point(10, 90)
        Me.powerlineFrequencyLabel.Name = "powerlineFrequencyLabel"
        Me.powerlineFrequencyLabel.Size = New System.Drawing.Size(131, 13)
        Me.powerlineFrequencyLabel.TabIndex = 7
        Me.powerlineFrequencyLabel.Text = "Powerline Frequency (Hz):"
        '
        'resolutionLabel
        '
        Me.resolutionLabel.AutoSize = True
        Me.resolutionLabel.Location = New System.Drawing.Point(10, 57)
        Me.resolutionLabel.Name = "resolutionLabel"
        Me.resolutionLabel.Size = New System.Drawing.Size(95, 13)
        Me.resolutionLabel.TabIndex = 6
        Me.resolutionLabel.Text = "Resolution (Digits):"
        '
        'rangeLabel
        '
        Me.rangeLabel.AutoSize = True
        Me.rangeLabel.Location = New System.Drawing.Point(10, 27)
        Me.rangeLabel.Name = "rangeLabel"
        Me.rangeLabel.Size = New System.Drawing.Size(42, 13)
        Me.rangeLabel.TabIndex = 5
        Me.rangeLabel.Text = "Range:"
        '
        'powerlineFrequencyValueComboBox
        '
        Me.powerlineFrequencyValueComboBox.FormattingEnabled = True
        Me.powerlineFrequencyValueComboBox.Location = New System.Drawing.Point(150, 86)
        Me.powerlineFrequencyValueComboBox.Name = "powerlineFrequencyValueComboBox"
        Me.powerlineFrequencyValueComboBox.Size = New System.Drawing.Size(120, 21)
        Me.powerlineFrequencyValueComboBox.TabIndex = 2
        '
        'resolutionValueComboBox
        '
        Me.resolutionValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resolutionValueComboBox.FormattingEnabled = True
        Me.resolutionValueComboBox.Location = New System.Drawing.Point(150, 53)
        Me.resolutionValueComboBox.Name = "resolutionValueComboBox"
        Me.resolutionValueComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resolutionValueComboBox.TabIndex = 1
        '
        'measurementModeComboBox
        '
        Me.measurementModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.measurementModeComboBox.FormattingEnabled = True
        Me.measurementModeComboBox.Location = New System.Drawing.Point(150, 51)
        Me.measurementModeComboBox.Name = "measurementModeComboBox"
        Me.measurementModeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.measurementModeComboBox.TabIndex = 1
        '
        'resourceAndMeasurementGroupBox
        '
        Me.resourceAndMeasurementGroupBox.Controls.Add(Me.measurementModeLabel)
        Me.resourceAndMeasurementGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.resourceAndMeasurementGroupBox.Controls.Add(Me.measurementModeComboBox)
        Me.resourceAndMeasurementGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceAndMeasurementGroupBox.Location = New System.Drawing.Point(3, 3)
        Me.resourceAndMeasurementGroupBox.Name = "resourceAndMeasurementGroupBox"
        Me.resourceAndMeasurementGroupBox.Size = New System.Drawing.Size(276, 85)
        Me.resourceAndMeasurementGroupBox.TabIndex = 0
        Me.resourceAndMeasurementGroupBox.TabStop = False
        Me.resourceAndMeasurementGroupBox.Text = "Resource Name and Measurement Type"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(150, 22)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'errorGroupBox
        '
        Me.errorGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.errorGroupBox.Controls.Add(Me.messageTextBox)
        Me.errorGroupBox.Location = New System.Drawing.Point(3, 323)
        Me.errorGroupBox.Name = "errorGroupBox"
        Me.errorGroupBox.Size = New System.Drawing.Size(276, 264)
        Me.errorGroupBox.TabIndex = 4
        Me.errorGroupBox.TabStop = False
        Me.errorGroupBox.Text = "Message"
        '
        'MainForm
        '
        Me.AcceptButton = Me.readButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(612, 599)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.triggerGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceAndMeasurementGroupBox)
        Me.Controls.Add(Me.errorGroupBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(628, 900)
        Me.MinimumSize = New System.Drawing.Size(628, 626)
        Me.Name = "MainForm"
        Me.Text = "Software Triggered Multipoint Acquisition"
        Me.buttonsPanel.ResumeLayout(False)
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        CType(Me.numberOfMeasurementsNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.readingDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.triggerDelayNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.triggerGroupBox.ResumeLayout(False)
        Me.triggerGroupBox.PerformLayout()
        CType(Me.rangeNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.resourceAndMeasurementGroupBox.ResumeLayout(False)
        Me.resourceAndMeasurementGroupBox.PerformLayout()
        Me.errorGroupBox.ResumeLayout(False)
        Me.errorGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents measurementModeLabel As System.Windows.Forms.Label
    Private WithEvents readButton As System.Windows.Forms.Button
    Private WithEvents clearButton As System.Windows.Forms.Button
    Private WithEvents buttonsPanel As System.Windows.Forms.Panel
    Private WithEvents softwareTriggerButton As System.Windows.Forms.Button
    Private WithEvents measurementGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents numberOfMeasurementsNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents actualRangeTextBox As System.Windows.Forms.TextBox
    Private WithEvents actualRangeLabel As System.Windows.Forms.Label
    Private WithEvents readingDataGridView As System.Windows.Forms.DataGridView
    Private WithEvents numberOfMeasurementsLabel As System.Windows.Forms.Label
    Private WithEvents triggerDelayNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents triggerGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents triggerDelayLabel As System.Windows.Forms.Label
    Private WithEvents triggerSourceLabel As System.Windows.Forms.Label
    Private WithEvents triggerSourceComboBox As System.Windows.Forms.ComboBox
    Private WithEvents messageTextBox As System.Windows.Forms.TextBox
    Private WithEvents resourceNameLabel As System.Windows.Forms.Label
    Private WithEvents rangeNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents configurationGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents powerlineFrequencyLabel As System.Windows.Forms.Label
    Private WithEvents resolutionLabel As System.Windows.Forms.Label
    Private WithEvents rangeLabel As System.Windows.Forms.Label
    Private WithEvents powerlineFrequencyValueComboBox As System.Windows.Forms.ComboBox
    Private WithEvents resolutionValueComboBox As System.Windows.Forms.ComboBox
    Private WithEvents measurementModeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents resourceAndMeasurementGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents resourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents errorGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ReadingNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents reading As System.Windows.Forms.DataGridViewTextBoxColumn

End Class

