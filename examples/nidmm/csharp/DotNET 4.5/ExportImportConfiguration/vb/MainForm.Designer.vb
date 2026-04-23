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
        Me.rangeNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.actualRangeLabel = New System.Windows.Forms.Label()
        Me.actualRangTextBox = New System.Windows.Forms.TextBox()
        Me.readButton = New System.Windows.Forms.Button()
        Me.measurementLabel = New System.Windows.Forms.Label()
        Me.measurementTextBox = New System.Windows.Forms.TextBox()
        Me.powerlineFrequencyLabel = New System.Windows.Forms.Label()
        Me.messageTextBox = New System.Windows.Forms.TextBox()
        Me.resolutionLabel = New System.Windows.Forms.Label()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.ResourceAndMeasurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementTypeLabel = New System.Windows.Forms.Label()
        Me.measurementModeComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.resolutionValueComboBox = New System.Windows.Forms.ComboBox()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.exportButton = New System.Windows.Forms.Button()
        Me.importButton = New System.Windows.Forms.Button()
        Me.rangeLabel = New System.Windows.Forms.Label()
        Me.powerlineFrequencyValueComboBox = New System.Windows.Forms.ComboBox()
        CType(Me.rangeNumericUpDown,System.ComponentModel.ISupportInitialize).BeginInit
        Me.measurementGroupBox.SuspendLayout
        Me.messageGroupBox.SuspendLayout
        Me.ResourceAndMeasurementGroupBox.SuspendLayout
        Me.configurationGroupBox.SuspendLayout
        Me.SuspendLayout
        '
        'rangeNumericUpDown
        '
        Me.rangeNumericUpDown.DecimalPlaces = 3
        Me.rangeNumericUpDown.Location = New System.Drawing.Point(150, 17)
        Me.rangeNumericUpDown.Maximum = New Decimal(New Integer() {1215752192, 23, 0, 0})
        Me.rangeNumericUpDown.Minimum = New Decimal(New Integer() {3, 0, 0, -2147483648})
        Me.rangeNumericUpDown.Name = "rangeNumericUpDown"
        Me.rangeNumericUpDown.Size = New System.Drawing.Size(107, 20)
        Me.rangeNumericUpDown.TabIndex = 0
        Me.rangeNumericUpDown.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.actualRangeLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualRangTextBox)
        Me.measurementGroupBox.Controls.Add(Me.readButton)
        Me.measurementGroupBox.Controls.Add(Me.measurementLabel)
        Me.measurementGroupBox.Controls.Add(Me.measurementTextBox)
        Me.measurementGroupBox.Location = New System.Drawing.Point(284, 161)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(269, 110)
        Me.measurementGroupBox.TabIndex = 2
        Me.measurementGroupBox.TabStop = false
        Me.measurementGroupBox.Text = "Measurement"
        '
        'actualRangeLabel
        '
        Me.actualRangeLabel.AutoSize = true
        Me.actualRangeLabel.Location = New System.Drawing.Point(6, 56)
        Me.actualRangeLabel.Name = "actualRangeLabel"
        Me.actualRangeLabel.Size = New System.Drawing.Size(75, 13)
        Me.actualRangeLabel.TabIndex = 6
        Me.actualRangeLabel.Text = "Actual Range:"
        '
        'actualRangTextBox
        '
        Me.actualRangTextBox.Location = New System.Drawing.Point(150, 53)
        Me.actualRangTextBox.Name = "actualRangTextBox"
        Me.actualRangTextBox.ReadOnly = true
        Me.actualRangTextBox.Size = New System.Drawing.Size(115, 20)
        Me.actualRangTextBox.TabIndex = 1
        '
        'readButton
        '
        Me.readButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.readButton.Location = New System.Drawing.Point(185, 83)
        Me.readButton.Name = "readButton"
        Me.readButton.Size = New System.Drawing.Size(78, 21)
        Me.readButton.TabIndex = 2
        Me.readButton.Text = "&Read"
        Me.readButton.UseVisualStyleBackColor = true
        '
        'measurementLabel
        '
        Me.measurementLabel.AutoSize = true
        Me.measurementLabel.Location = New System.Drawing.Point(6, 23)
        Me.measurementLabel.Name = "measurementLabel"
        Me.measurementLabel.Size = New System.Drawing.Size(74, 13)
        Me.measurementLabel.TabIndex = 1
        Me.measurementLabel.Text = "Measurement:"
        '
        'measurementTextBox
        '
        Me.measurementTextBox.Location = New System.Drawing.Point(150, 21)
        Me.measurementTextBox.Name = "measurementTextBox"
        Me.measurementTextBox.ReadOnly = true
        Me.measurementTextBox.Size = New System.Drawing.Size(115, 20)
        Me.measurementTextBox.TabIndex = 0
        '
        'powerlineFrequencyLabel
        '
        Me.powerlineFrequencyLabel.AutoSize = true
        Me.powerlineFrequencyLabel.Location = New System.Drawing.Point(6, 92)
        Me.powerlineFrequencyLabel.Name = "powerlineFrequencyLabel"
        Me.powerlineFrequencyLabel.Size = New System.Drawing.Size(131, 13)
        Me.powerlineFrequencyLabel.TabIndex = 7
        Me.powerlineFrequencyLabel.Text = "Powerline Frequency (Hz):"
        '
        'messageTextBox
        '
        Me.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.messageTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.messageTextBox.Location = New System.Drawing.Point(3, 16)
        Me.messageTextBox.Multiline = true
        Me.messageTextBox.Name = "messageTextBox"
        Me.messageTextBox.ReadOnly = true
        Me.messageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.messageTextBox.Size = New System.Drawing.Size(270, 150)
        Me.messageTextBox.TabIndex = 0
        '
        'resolutionLabel
        '
        Me.resolutionLabel.AutoSize = true
        Me.resolutionLabel.Location = New System.Drawing.Point(6, 56)
        Me.resolutionLabel.Name = "resolutionLabel"
        Me.resolutionLabel.Size = New System.Drawing.Size(95, 13)
        Me.resolutionLabel.TabIndex = 6
        Me.resolutionLabel.Text = "Resolution (Digits):"
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(2, 102)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(276, 169)
        Me.messageGroupBox.TabIndex = 3
        Me.messageGroupBox.TabStop = false
        Me.messageGroupBox.Text = "Message"
        '
        'ResourceAndMeasurementGroupBox
        '
        Me.ResourceAndMeasurementGroupBox.Controls.Add(Me.measurementTypeLabel)
        Me.ResourceAndMeasurementGroupBox.Controls.Add(Me.measurementModeComboBox)
        Me.ResourceAndMeasurementGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.ResourceAndMeasurementGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.ResourceAndMeasurementGroupBox.Location = New System.Drawing.Point(2, 3)
        Me.ResourceAndMeasurementGroupBox.Name = "ResourceAndMeasurementGroupBox"
        Me.ResourceAndMeasurementGroupBox.Size = New System.Drawing.Size(276, 93)
        Me.ResourceAndMeasurementGroupBox.TabIndex = 0
        Me.ResourceAndMeasurementGroupBox.TabStop = false
        Me.ResourceAndMeasurementGroupBox.Text = "Resource Name and Measurement Type"
        '
        'measurementTypeLabel
        '
        Me.measurementTypeLabel.AutoSize = true
        Me.measurementTypeLabel.Location = New System.Drawing.Point(6, 64)
        Me.measurementTypeLabel.Name = "measurementTypeLabel"
        Me.measurementTypeLabel.Size = New System.Drawing.Size(104, 13)
        Me.measurementTypeLabel.TabIndex = 3
        Me.measurementTypeLabel.Text = "Measurement Mode:"
        '
        'measurementModeComboBox
        '
        Me.measurementModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.measurementModeComboBox.FormattingEnabled = true
        Me.measurementModeComboBox.Location = New System.Drawing.Point(142, 61)
        Me.measurementModeComboBox.Name = "measurementModeComboBox"
        Me.measurementModeComboBox.Size = New System.Drawing.Size(128, 21)
        Me.measurementModeComboBox.TabIndex = 1
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = true
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 28)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 1
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = true
        Me.resourceNameComboBox.Location = New System.Drawing.Point(142, 24)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(128, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'resolutionValueComboBox
        '
        Me.resolutionValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resolutionValueComboBox.FormattingEnabled = true
        Me.resolutionValueComboBox.Location = New System.Drawing.Point(150, 53)
        Me.resolutionValueComboBox.Name = "resolutionValueComboBox"
        Me.resolutionValueComboBox.Size = New System.Drawing.Size(108, 21)
        Me.resolutionValueComboBox.TabIndex = 1
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.exportButton)
        Me.configurationGroupBox.Controls.Add(Me.importButton)
        Me.configurationGroupBox.Controls.Add(Me.rangeNumericUpDown)
        Me.configurationGroupBox.Controls.Add(Me.powerlineFrequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.resolutionLabel)
        Me.configurationGroupBox.Controls.Add(Me.rangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.powerlineFrequencyValueComboBox)
        Me.configurationGroupBox.Controls.Add(Me.resolutionValueComboBox)
        Me.configurationGroupBox.Location = New System.Drawing.Point(284, 3)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(269, 152)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = false
        Me.configurationGroupBox.Text = "Configuration"
        '
        'exportButton
        '
        Me.exportButton.Location = New System.Drawing.Point(79, 122)
        Me.exportButton.Name = "exportButton"
        Me.exportButton.Size = New System.Drawing.Size(84, 23)
        Me.exportButton.TabIndex = 8
        Me.exportButton.Text = "Export..."
        Me.exportButton.UseVisualStyleBackColor = true
        '
        'importButton
        '
        Me.importButton.Location = New System.Drawing.Point(173, 122)
        Me.importButton.Name = "importButton"
        Me.importButton.Size = New System.Drawing.Size(84, 23)
        Me.importButton.TabIndex = 8
        Me.importButton.Text = "Import..."
        Me.importButton.UseVisualStyleBackColor = true
        '
        'rangeLabel
        '
        Me.rangeLabel.AutoSize = true
        Me.rangeLabel.Location = New System.Drawing.Point(6, 20)
        Me.rangeLabel.Name = "rangeLabel"
        Me.rangeLabel.Size = New System.Drawing.Size(42, 13)
        Me.rangeLabel.TabIndex = 5
        Me.rangeLabel.Text = "Range:"
        '
        'powerlineFrequencyValueComboBox
        '
        Me.powerlineFrequencyValueComboBox.FormattingEnabled = true
        Me.powerlineFrequencyValueComboBox.Location = New System.Drawing.Point(150, 89)
        Me.powerlineFrequencyValueComboBox.Name = "powerlineFrequencyValueComboBox"
        Me.powerlineFrequencyValueComboBox.Size = New System.Drawing.Size(108, 21)
        Me.powerlineFrequencyValueComboBox.TabIndex = 2
        '
        'MainForm
        '
        Me.AcceptButton = Me.readButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(565, 283)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.ResourceAndMeasurementGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.MaximizeBox = false
        Me.Name = "MainForm"
        Me.Text = "Export Import Configuration"
        CType(Me.rangeNumericUpDown,System.ComponentModel.ISupportInitialize).EndInit
        Me.measurementGroupBox.ResumeLayout(false)
        Me.measurementGroupBox.PerformLayout
        Me.messageGroupBox.ResumeLayout(false)
        Me.messageGroupBox.PerformLayout
        Me.ResourceAndMeasurementGroupBox.ResumeLayout(false)
        Me.ResourceAndMeasurementGroupBox.PerformLayout
        Me.configurationGroupBox.ResumeLayout(false)
        Me.configurationGroupBox.PerformLayout
        Me.ResumeLayout(false)

End Sub
    Private WithEvents rangeNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents measurementGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents actualRangeLabel As System.Windows.Forms.Label
    Private WithEvents actualRangTextBox As System.Windows.Forms.TextBox
    Private WithEvents readButton As System.Windows.Forms.Button
    Private WithEvents measurementLabel As System.Windows.Forms.Label
    Private WithEvents measurementTextBox As System.Windows.Forms.TextBox
    Private WithEvents powerlineFrequencyLabel As System.Windows.Forms.Label
    Private WithEvents messageTextBox As System.Windows.Forms.TextBox
    Private WithEvents resolutionLabel As System.Windows.Forms.Label
    Private WithEvents messageGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents ResourceAndMeasurementGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents measurementTypeLabel As System.Windows.Forms.Label
    Private WithEvents measurementModeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents resourceNameLabel As System.Windows.Forms.Label
    Private WithEvents resourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents resolutionValueComboBox As System.Windows.Forms.ComboBox
    Private WithEvents configurationGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents rangeLabel As System.Windows.Forms.Label
    Private WithEvents powerlineFrequencyValueComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents exportButton As System.Windows.Forms.Button
    Friend WithEvents importButton As System.Windows.Forms.Button

End Class

