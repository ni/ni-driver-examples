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
        Me.stopButton = New System.Windows.Forms.Button()
        Me.actualPowerLevelTextBox = New System.Windows.Forms.TextBox()
        Me.actualRangeLabel = New System.Windows.Forms.Label()
        Me.frequencyReferenceSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.errorGroupBox = New System.Windows.Forms.GroupBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.frequencyReferenceSourceLabel = New System.Windows.Forms.Label()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.frequencyLabel = New System.Windows.Forms.Label()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.generationModeLabel = New System.Windows.Forms.Label()
        Me.actualFrequencyLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameAndGenerationTypeGroupBox = New System.Windows.Forms.GroupBox()
        Me.generationModeComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.actualFrequencyTextBox = New System.Windows.Forms.TextBox()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.errorGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resourceNameAndGenerationTypeGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'stopButton
        '
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(171, 93)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 4
        Me.stopButton.Text = "Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'actualPowerLevelTextBox
        '
        Me.actualPowerLevelTextBox.Location = New System.Drawing.Point(171, 58)
        Me.actualPowerLevelTextBox.Name = "actualPowerLevelTextBox"
        Me.actualPowerLevelTextBox.ReadOnly = True
        Me.actualPowerLevelTextBox.Size = New System.Drawing.Size(120, 20)
        Me.actualPowerLevelTextBox.TabIndex = 1
        Me.actualPowerLevelTextBox.TabStop = False
        '
        'actualRangeLabel
        '
        Me.actualRangeLabel.AutoSize = True
        Me.actualRangeLabel.Location = New System.Drawing.Point(5, 64)
        Me.actualRangeLabel.Name = "actualRangeLabel"
        Me.actualRangeLabel.Size = New System.Drawing.Size(132, 13)
        Me.actualRangeLabel.TabIndex = 3
        Me.actualRangeLabel.Text = "Actual Power Level [dBm]:"
        '
        'frequencyReferenceSourceComboBox
        '
        Me.frequencyReferenceSourceComboBox.FormattingEnabled = True
        Me.frequencyReferenceSourceComboBox.Location = New System.Drawing.Point(171, 97)
        Me.frequencyReferenceSourceComboBox.Name = "frequencyReferenceSourceComboBox"
        Me.frequencyReferenceSourceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.frequencyReferenceSourceComboBox.TabIndex = 6
        '
        'frequencyNumeric
        '
        Me.frequencyNumeric.Location = New System.Drawing.Point(171, 30)
        Me.frequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.frequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.frequencyNumeric.Name = "frequencyNumeric"
        Me.frequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.frequencyNumeric.TabIndex = 0
        Me.frequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'errorGroupBox
        '
        Me.errorGroupBox.Controls.Add(Me.errorTextBox)
        Me.errorGroupBox.Location = New System.Drawing.Point(9, 144)
        Me.errorGroupBox.Name = "errorGroupBox"
        Me.errorGroupBox.Size = New System.Drawing.Size(278, 131)
        Me.errorGroupBox.TabIndex = 7
        Me.errorGroupBox.TabStop = False
        Me.errorGroupBox.Text = "Error Message"
        '
        'errorTextBox
        '
        Me.errorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.errorTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.errorTextBox.Location = New System.Drawing.Point(3, 16)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.errorTextBox.Size = New System.Drawing.Size(272, 112)
        Me.errorTextBox.TabIndex = 0
        Me.errorTextBox.TabStop = False
        '
        'frequencyReferenceSourceLabel
        '
        Me.frequencyReferenceSourceLabel.AutoSize = True
        Me.frequencyReferenceSourceLabel.Location = New System.Drawing.Point(6, 100)
        Me.frequencyReferenceSourceLabel.Name = "frequencyReferenceSourceLabel"
        Me.frequencyReferenceSourceLabel.Size = New System.Drawing.Size(150, 13)
        Me.frequencyReferenceSourceLabel.TabIndex = 5
        Me.frequencyReferenceSourceLabel.Text = "Frequency Reference Source:"
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.frequencyReferenceSourceComboBox)
        Me.configurationGroupBox.Controls.Add(Me.frequencyReferenceSourceLabel)
        Me.configurationGroupBox.Controls.Add(Me.frequencyNumeric)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelLabel)
        Me.configurationGroupBox.Controls.Add(Me.frequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.powerLevelNumeric)
        Me.configurationGroupBox.Location = New System.Drawing.Point(293, 9)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(301, 129)
        Me.configurationGroupBox.TabIndex = 5
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(5, 65)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(99, 13)
        Me.powerLevelLabel.TabIndex = 3
        Me.powerLevelLabel.Text = "Power Level [dBm]:"
        '
        'frequencyLabel
        '
        Me.frequencyLabel.AutoSize = True
        Me.frequencyLabel.Location = New System.Drawing.Point(5, 32)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(116, 13)
        Me.frequencyLabel.TabIndex = 2
        Me.frequencyLabel.Text = "Center Frequency [Hz]:"
        '
        'powerLevelNumeric
        '
        Me.powerLevelNumeric.Location = New System.Drawing.Point(171, 63)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerLevelNumeric.TabIndex = 1
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'generationModeLabel
        '
        Me.generationModeLabel.AutoSize = True
        Me.generationModeLabel.Location = New System.Drawing.Point(9, 65)
        Me.generationModeLabel.Name = "generationModeLabel"
        Me.generationModeLabel.Size = New System.Drawing.Size(92, 13)
        Me.generationModeLabel.TabIndex = 3
        Me.generationModeLabel.Text = "Generation Mode:"
        '
        'actualFrequencyLabel
        '
        Me.actualFrequencyLabel.AutoSize = True
        Me.actualFrequencyLabel.Location = New System.Drawing.Point(5, 29)
        Me.actualFrequencyLabel.Name = "actualFrequencyLabel"
        Me.actualFrequencyLabel.Size = New System.Drawing.Size(115, 13)
        Me.actualFrequencyLabel.TabIndex = 1
        Me.actualFrequencyLabel.Text = "Actual Frequency [Hz]:"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(9, 32)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 2
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'resourceNameAndGenerationTypeGroupBox
        '
        Me.resourceNameAndGenerationTypeGroupBox.Controls.Add(Me.generationModeLabel)
        Me.resourceNameAndGenerationTypeGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.resourceNameAndGenerationTypeGroupBox.Controls.Add(Me.generationModeComboBox)
        Me.resourceNameAndGenerationTypeGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceNameAndGenerationTypeGroupBox.Location = New System.Drawing.Point(9, 9)
        Me.resourceNameAndGenerationTypeGroupBox.Name = "resourceNameAndGenerationTypeGroupBox"
        Me.resourceNameAndGenerationTypeGroupBox.Size = New System.Drawing.Size(278, 129)
        Me.resourceNameAndGenerationTypeGroupBox.TabIndex = 4
        Me.resourceNameAndGenerationTypeGroupBox.TabStop = False
        Me.resourceNameAndGenerationTypeGroupBox.Text = "Resource Name and Generation Type"
        '
        'generationModeComboBox
        '
        Me.generationModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.generationModeComboBox.FormattingEnabled = True
        Me.generationModeComboBox.Location = New System.Drawing.Point(146, 62)
        Me.generationModeComboBox.Name = "generationModeComboBox"
        Me.generationModeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.generationModeComboBox.TabIndex = 1
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(146, 28)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(90, 93)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 2
        Me.startButton.Text = "Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.stopButton)
        Me.measurementGroupBox.Controls.Add(Me.actualPowerLevelTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualRangeLabel)
        Me.measurementGroupBox.Controls.Add(Me.startButton)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualFrequencyTextBox)
        Me.measurementGroupBox.Location = New System.Drawing.Point(293, 144)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(301, 131)
        Me.measurementGroupBox.TabIndex = 6
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'actualFrequencyTextBox
        '
        Me.actualFrequencyTextBox.Location = New System.Drawing.Point(171, 29)
        Me.actualFrequencyTextBox.Name = "actualFrequencyTextBox"
        Me.actualFrequencyTextBox.ReadOnly = True
        Me.actualFrequencyTextBox.Size = New System.Drawing.Size(120, 20)
        Me.actualFrequencyTextBox.TabIndex = 0
        Me.actualFrequencyTextBox.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(602, 284)
        Me.Controls.Add(Me.errorGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceNameAndGenerationTypeGroupBox)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Advanced Property Access"
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.errorGroupBox.ResumeLayout(False)
        Me.errorGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.resourceNameAndGenerationTypeGroupBox.ResumeLayout(False)
        Me.resourceNameAndGenerationTypeGroupBox.PerformLayout()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents actualPowerLevelTextBox As System.Windows.Forms.TextBox
    Private WithEvents actualRangeLabel As System.Windows.Forms.Label
    Private WithEvents frequencyReferenceSourceComboBox As System.Windows.Forms.ComboBox
    Private WithEvents frequencyNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents errorGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents errorTextBox As System.Windows.Forms.TextBox
    Private WithEvents frequencyReferenceSourceLabel As System.Windows.Forms.Label
    Private WithEvents configurationGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents powerLevelLabel As System.Windows.Forms.Label
    Private WithEvents frequencyLabel As System.Windows.Forms.Label
    Private WithEvents powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents generationModeLabel As System.Windows.Forms.Label
    Private WithEvents actualFrequencyLabel As System.Windows.Forms.Label
    Private WithEvents resourceNameLabel As System.Windows.Forms.Label
    Private WithEvents resourceNameAndGenerationTypeGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents generationModeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents resourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents measurementGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents actualFrequencyTextBox As System.Windows.Forms.TextBox

End Class
