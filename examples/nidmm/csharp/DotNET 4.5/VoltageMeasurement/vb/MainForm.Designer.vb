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
        Me.rangeLabel = New System.Windows.Forms.Label()
        Me.powerlineFrequencyValueComboBox = New System.Windows.Forms.ComboBox()
        CType(Me.rangeNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementGroupBox.SuspendLayout()
        Me.messageGroupBox.SuspendLayout()
        Me.ResourceAndMeasurementGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'rangeNumericUpDown
        '
        Me.rangeNumericUpDown.DecimalPlaces = 3
        Me.rangeNumericUpDown.Location = New System.Drawing.Point(150, 25)
        Me.rangeNumericUpDown.Maximum = New Decimal(New Integer() {1215752192, 23, 0, 0})
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
        Me.measurementGroupBox.Location = New System.Drawing.Point(284, 138)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(269, 133)
        Me.measurementGroupBox.TabIndex = 2
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'actualRangeLabel
        '
        Me.actualRangeLabel.AutoSize = True
        Me.actualRangeLabel.Location = New System.Drawing.Point(6, 57)
        Me.actualRangeLabel.Name = "actualRangeLabel"
        Me.actualRangeLabel.Size = New System.Drawing.Size(75, 13)
        Me.actualRangeLabel.TabIndex = 6
        Me.actualRangeLabel.Text = "Actual Range:"
        '
        'actualRangTextBox
        '
        Me.actualRangTextBox.Location = New System.Drawing.Point(150, 54)
        Me.actualRangTextBox.Name = "actualRangTextBox"
        Me.actualRangTextBox.ReadOnly = True
        Me.actualRangTextBox.Size = New System.Drawing.Size(115, 20)
        Me.actualRangTextBox.TabIndex = 1
        '
        'readButton
        '
        Me.readButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.readButton.Location = New System.Drawing.Point(150, 93)
        Me.readButton.Name = "readButton"
        Me.readButton.Size = New System.Drawing.Size(78, 21)
        Me.readButton.TabIndex = 2
        Me.readButton.Text = "&Read"
        Me.readButton.UseVisualStyleBackColor = True
        '
        'measurementLabel
        '
        Me.measurementLabel.AutoSize = True
        Me.measurementLabel.Location = New System.Drawing.Point(6, 24)
        Me.measurementLabel.Name = "measurementLabel"
        Me.measurementLabel.Size = New System.Drawing.Size(74, 13)
        Me.measurementLabel.TabIndex = 1
        Me.measurementLabel.Text = "Measurement:"
        '
        'measurementTextBox
        '
        Me.measurementTextBox.Location = New System.Drawing.Point(150, 22)
        Me.measurementTextBox.Name = "measurementTextBox"
        Me.measurementTextBox.ReadOnly = True
        Me.measurementTextBox.Size = New System.Drawing.Size(115, 20)
        Me.measurementTextBox.TabIndex = 0
        '
        'powerlineFrequencyLabel
        '
        Me.powerlineFrequencyLabel.AutoSize = True
        Me.powerlineFrequencyLabel.Location = New System.Drawing.Point(6, 100)
        Me.powerlineFrequencyLabel.Name = "powerlineFrequencyLabel"
        Me.powerlineFrequencyLabel.Size = New System.Drawing.Size(131, 13)
        Me.powerlineFrequencyLabel.TabIndex = 7
        Me.powerlineFrequencyLabel.Text = "Powerline Frequency (Hz):"
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
        Me.messageTextBox.Size = New System.Drawing.Size(270, 116)
        Me.messageTextBox.TabIndex = 0
        '
        'resolutionLabel
        '
        Me.resolutionLabel.AutoSize = True
        Me.resolutionLabel.Location = New System.Drawing.Point(6, 64)
        Me.resolutionLabel.Name = "resolutionLabel"
        Me.resolutionLabel.Size = New System.Drawing.Size(95, 13)
        Me.resolutionLabel.TabIndex = 6
        Me.resolutionLabel.Text = "Resolution (Digits):"
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(2, 138)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(276, 133)
        Me.messageGroupBox.TabIndex = 3
        Me.messageGroupBox.TabStop = False
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
        Me.ResourceAndMeasurementGroupBox.Size = New System.Drawing.Size(276, 129)
        Me.ResourceAndMeasurementGroupBox.TabIndex = 0
        Me.ResourceAndMeasurementGroupBox.TabStop = False
        Me.ResourceAndMeasurementGroupBox.Text = "Resource Name and Measurement Type"
        '
        'measurementTypeLabel
        '
        Me.measurementTypeLabel.AutoSize = True
        Me.measurementTypeLabel.Location = New System.Drawing.Point(6, 64)
        Me.measurementTypeLabel.Name = "measurementTypeLabel"
        Me.measurementTypeLabel.Size = New System.Drawing.Size(104, 13)
        Me.measurementTypeLabel.TabIndex = 3
        Me.measurementTypeLabel.Text = "Measurement Mode:"
        '
        'measurementModeComboBox
        '
        Me.measurementModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.measurementModeComboBox.FormattingEnabled = True
        Me.measurementModeComboBox.Location = New System.Drawing.Point(142, 61)
        Me.measurementModeComboBox.Name = "measurementModeComboBox"
        Me.measurementModeComboBox.Size = New System.Drawing.Size(128, 21)
        Me.measurementModeComboBox.TabIndex = 1
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 28)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 1
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(142, 24)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(128, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'resolutionValueComboBox
        '
        Me.resolutionValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resolutionValueComboBox.FormattingEnabled = True
        Me.resolutionValueComboBox.Location = New System.Drawing.Point(150, 61)
        Me.resolutionValueComboBox.Name = "resolutionValueComboBox"
        Me.resolutionValueComboBox.Size = New System.Drawing.Size(108, 21)
        Me.resolutionValueComboBox.TabIndex = 1
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.rangeNumericUpDown)
        Me.configurationGroupBox.Controls.Add(Me.powerlineFrequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.resolutionLabel)
        Me.configurationGroupBox.Controls.Add(Me.rangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.powerlineFrequencyValueComboBox)
        Me.configurationGroupBox.Controls.Add(Me.resolutionValueComboBox)
        Me.configurationGroupBox.Location = New System.Drawing.Point(284, 3)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(269, 129)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'rangeLabel
        '
        Me.rangeLabel.AutoSize = True
        Me.rangeLabel.Location = New System.Drawing.Point(6, 28)
        Me.rangeLabel.Name = "rangeLabel"
        Me.rangeLabel.Size = New System.Drawing.Size(42, 13)
        Me.rangeLabel.TabIndex = 5
        Me.rangeLabel.Text = "Range:"
        '
        'powerlineFrequencyValueComboBox
        '
        Me.powerlineFrequencyValueComboBox.FormattingEnabled = True
        Me.powerlineFrequencyValueComboBox.Location = New System.Drawing.Point(150, 97)
        Me.powerlineFrequencyValueComboBox.Name = "powerlineFrequencyValueComboBox"
        Me.powerlineFrequencyValueComboBox.Size = New System.Drawing.Size(108, 21)
        Me.powerlineFrequencyValueComboBox.TabIndex = 2
        '
        'MainForm
        '
        Me.AcceptButton = Me.readButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(565, 283)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.ResourceAndMeasurementGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Voltage Measurement"
        CType(Me.rangeNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.messageGroupBox.ResumeLayout(False)
        Me.messageGroupBox.PerformLayout()
        Me.ResourceAndMeasurementGroupBox.ResumeLayout(False)
        Me.ResourceAndMeasurementGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.ResumeLayout(False)

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

End Class

