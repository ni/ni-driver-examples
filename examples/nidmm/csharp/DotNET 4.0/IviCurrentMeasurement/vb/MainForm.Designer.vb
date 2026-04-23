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
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.actualRangeLabel = New System.Windows.Forms.Label()
        Me.actualRangeTextBox = New System.Windows.Forms.TextBox()
        Me.readButton = New System.Windows.Forms.Button()
        Me.measurementLabel = New System.Windows.Forms.Label()
        Me.measurementTextBox = New System.Windows.Forms.TextBox()
        Me.rangeTextBox = New System.Windows.Forms.TextBox()
        Me.measurementModeComboBox = New System.Windows.Forms.ComboBox()
        Me.rangeLabel = New System.Windows.Forms.Label()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.powerlineFrequencyLabel = New System.Windows.Forms.Label()
        Me.resolutionLabel = New System.Windows.Forms.Label()
        Me.powerlineFrequencyValueComboBox = New System.Windows.Forms.ComboBox()
        Me.resolutionValueComboBox = New System.Windows.Forms.ComboBox()
        Me.acConfigurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.maxACFrequencyTextBox = New System.Windows.Forms.TextBox()
        Me.minACFrequencyTextBox = New System.Windows.Forms.TextBox()
        Me.maxACFrequencyLabel = New System.Windows.Forms.Label()
        Me.minACFrequencyLabel = New System.Windows.Forms.Label()
        Me.measurementTypeLabel = New System.Windows.Forms.Label()
        Me.ResourceAndMeasurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.deviceNameTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.TextBox()
        Me.measurementGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        Me.acConfigurationGroupBox.SuspendLayout()
        Me.ResourceAndMeasurementGroupBox.SuspendLayout()
        Me.messageGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.actualRangeLabel)
        Me.measurementGroupBox.Controls.Add(Me.actualRangeTextBox)
        Me.measurementGroupBox.Controls.Add(Me.readButton)
        Me.measurementGroupBox.Controls.Add(Me.measurementLabel)
        Me.measurementGroupBox.Controls.Add(Me.measurementTextBox)
        Me.measurementGroupBox.Location = New System.Drawing.Point(306, 140)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(277, 120)
        Me.measurementGroupBox.TabIndex = 3
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'actualRangeLabel
        '
        Me.actualRangeLabel.AutoSize = True
        Me.actualRangeLabel.Location = New System.Drawing.Point(15, 51)
        Me.actualRangeLabel.Name = "actualRangeLabel"
        Me.actualRangeLabel.Size = New System.Drawing.Size(75, 13)
        Me.actualRangeLabel.TabIndex = 6
        Me.actualRangeLabel.Text = "Actual Range:"
        '
        'actualRangeTextBox
        '
        Me.actualRangeTextBox.Location = New System.Drawing.Point(157, 49)
        Me.actualRangeTextBox.Name = "actualRangeTextBox"
        Me.actualRangeTextBox.ReadOnly = True
        Me.actualRangeTextBox.Size = New System.Drawing.Size(108, 20)
        Me.actualRangeTextBox.TabIndex = 1
        '
        'readButton
        '
        Me.readButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.readButton.Location = New System.Drawing.Point(157, 77)
        Me.readButton.Name = "readButton"
        Me.readButton.Size = New System.Drawing.Size(78, 21)
        Me.readButton.TabIndex = 2
        Me.readButton.Text = "&Read"
        Me.readButton.UseVisualStyleBackColor = True
        '
        'measurementLabel
        '
        Me.measurementLabel.AutoSize = True
        Me.measurementLabel.Location = New System.Drawing.Point(15, 16)
        Me.measurementLabel.Name = "measurementLabel"
        Me.measurementLabel.Size = New System.Drawing.Size(74, 13)
        Me.measurementLabel.TabIndex = 1
        Me.measurementLabel.Text = "Measurement:"
        '
        'measurementTextBox
        '
        Me.measurementTextBox.Location = New System.Drawing.Point(157, 14)
        Me.measurementTextBox.Name = "measurementTextBox"
        Me.measurementTextBox.ReadOnly = True
        Me.measurementTextBox.Size = New System.Drawing.Size(108, 20)
        Me.measurementTextBox.TabIndex = 0
        '
        'rangeTextBox
        '
        Me.rangeTextBox.Location = New System.Drawing.Point(157, 23)
        Me.rangeTextBox.Name = "rangeTextBox"
        Me.rangeTextBox.Size = New System.Drawing.Size(108, 20)
        Me.rangeTextBox.TabIndex = 0
        Me.rangeTextBox.Text = "2.00E-001"
        '
        'measurementModeComboBox
        '
        Me.measurementModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.measurementModeComboBox.FormattingEnabled = True
        Me.measurementModeComboBox.Location = New System.Drawing.Point(165, 54)
        Me.measurementModeComboBox.Name = "measurementModeComboBox"
        Me.measurementModeComboBox.Size = New System.Drawing.Size(126, 21)
        Me.measurementModeComboBox.TabIndex = 1
        '
        'rangeLabel
        '
        Me.rangeLabel.AutoSize = True
        Me.rangeLabel.Location = New System.Drawing.Point(15, 25)
        Me.rangeLabel.Name = "rangeLabel"
        Me.rangeLabel.Size = New System.Drawing.Size(42, 13)
        Me.rangeLabel.TabIndex = 5
        Me.rangeLabel.Text = "Range:"
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.rangeTextBox)
        Me.configurationGroupBox.Controls.Add(Me.powerlineFrequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.resolutionLabel)
        Me.configurationGroupBox.Controls.Add(Me.rangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.powerlineFrequencyValueComboBox)
        Me.configurationGroupBox.Controls.Add(Me.resolutionValueComboBox)
        Me.configurationGroupBox.Location = New System.Drawing.Point(306, 6)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(277, 128)
        Me.configurationGroupBox.TabIndex = 2
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'powerlineFrequencyLabel
        '
        Me.powerlineFrequencyLabel.AutoSize = True
        Me.powerlineFrequencyLabel.Location = New System.Drawing.Point(15, 92)
        Me.powerlineFrequencyLabel.Name = "powerlineFrequencyLabel"
        Me.powerlineFrequencyLabel.Size = New System.Drawing.Size(131, 13)
        Me.powerlineFrequencyLabel.TabIndex = 7
        Me.powerlineFrequencyLabel.Text = "Powerline Frequency (Hz):"
        '
        'resolutionLabel
        '
        Me.resolutionLabel.AutoSize = True
        Me.resolutionLabel.Location = New System.Drawing.Point(15, 59)
        Me.resolutionLabel.Name = "resolutionLabel"
        Me.resolutionLabel.Size = New System.Drawing.Size(95, 13)
        Me.resolutionLabel.TabIndex = 6
        Me.resolutionLabel.Text = "Resolution (Digits):"
        '
        'powerlineFrequencyValueComboBox
        '
        Me.powerlineFrequencyValueComboBox.FormattingEnabled = True
        Me.powerlineFrequencyValueComboBox.Location = New System.Drawing.Point(157, 89)
        Me.powerlineFrequencyValueComboBox.Name = "powerlineFrequencyValueComboBox"
        Me.powerlineFrequencyValueComboBox.Size = New System.Drawing.Size(108, 21)
        Me.powerlineFrequencyValueComboBox.TabIndex = 2
        '
        'resolutionValueComboBox
        '
        Me.resolutionValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resolutionValueComboBox.FormattingEnabled = True
        Me.resolutionValueComboBox.Location = New System.Drawing.Point(157, 56)
        Me.resolutionValueComboBox.Name = "resolutionValueComboBox"
        Me.resolutionValueComboBox.Size = New System.Drawing.Size(108, 21)
        Me.resolutionValueComboBox.TabIndex = 1
        '
        'acConfigurationGroupBox
        '
        Me.acConfigurationGroupBox.Controls.Add(Me.maxACFrequencyTextBox)
        Me.acConfigurationGroupBox.Controls.Add(Me.minACFrequencyTextBox)
        Me.acConfigurationGroupBox.Controls.Add(Me.maxACFrequencyLabel)
        Me.acConfigurationGroupBox.Controls.Add(Me.minACFrequencyLabel)
        Me.acConfigurationGroupBox.Enabled = False
        Me.acConfigurationGroupBox.Location = New System.Drawing.Point(3, 95)
        Me.acConfigurationGroupBox.Name = "acConfigurationGroupBox"
        Me.acConfigurationGroupBox.Size = New System.Drawing.Size(297, 74)
        Me.acConfigurationGroupBox.TabIndex = 1
        Me.acConfigurationGroupBox.TabStop = False
        Me.acConfigurationGroupBox.Text = "AC Configuration"
        '
        'maxACFrequencyTextBox
        '
        Me.maxACFrequencyTextBox.Location = New System.Drawing.Point(165, 41)
        Me.maxACFrequencyTextBox.Name = "maxACFrequencyTextBox"
        Me.maxACFrequencyTextBox.Size = New System.Drawing.Size(126, 20)
        Me.maxACFrequencyTextBox.TabIndex = 1
        Me.maxACFrequencyTextBox.Text = "25.00E+03"
        '
        'minACFrequencyTextBox
        '
        Me.minACFrequencyTextBox.Location = New System.Drawing.Point(165, 14)
        Me.minACFrequencyTextBox.Name = "minACFrequencyTextBox"
        Me.minACFrequencyTextBox.Size = New System.Drawing.Size(126, 20)
        Me.minACFrequencyTextBox.TabIndex = 0
        Me.minACFrequencyTextBox.Text = "20.00E+0"
        '
        'maxACFrequencyLabel
        '
        Me.maxACFrequencyLabel.AutoSize = True
        Me.maxACFrequencyLabel.Location = New System.Drawing.Point(6, 44)
        Me.maxACFrequencyLabel.Name = "maxACFrequencyLabel"
        Me.maxACFrequencyLabel.Size = New System.Drawing.Size(146, 13)
        Me.maxACFrequencyLabel.TabIndex = 3
        Me.maxACFrequencyLabel.Text = "Maximum AC Frequency (Hz):"
        '
        'minACFrequencyLabel
        '
        Me.minACFrequencyLabel.AutoSize = True
        Me.minACFrequencyLabel.Location = New System.Drawing.Point(6, 16)
        Me.minACFrequencyLabel.Name = "minACFrequencyLabel"
        Me.minACFrequencyLabel.Size = New System.Drawing.Size(143, 13)
        Me.minACFrequencyLabel.TabIndex = 2
        Me.minACFrequencyLabel.Text = "Minimum AC Frequency (Hz):"
        '
        'measurementTypeLabel
        '
        Me.measurementTypeLabel.AutoSize = True
        Me.measurementTypeLabel.Location = New System.Drawing.Point(6, 58)
        Me.measurementTypeLabel.Name = "measurementTypeLabel"
        Me.measurementTypeLabel.Size = New System.Drawing.Size(104, 13)
        Me.measurementTypeLabel.TabIndex = 3
        Me.measurementTypeLabel.Text = "Measurement Mode:"
        '
        'ResourceAndMeasurementGroupBox
        '
        Me.ResourceAndMeasurementGroupBox.Controls.Add(Me.deviceNameTextBox)
        Me.ResourceAndMeasurementGroupBox.Controls.Add(Me.measurementTypeLabel)
        Me.ResourceAndMeasurementGroupBox.Controls.Add(Me.measurementModeComboBox)
        Me.ResourceAndMeasurementGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.ResourceAndMeasurementGroupBox.Location = New System.Drawing.Point(3, 6)
        Me.ResourceAndMeasurementGroupBox.Name = "ResourceAndMeasurementGroupBox"
        Me.ResourceAndMeasurementGroupBox.Size = New System.Drawing.Size(297, 86)
        Me.ResourceAndMeasurementGroupBox.TabIndex = 0
        Me.ResourceAndMeasurementGroupBox.TabStop = False
        Me.ResourceAndMeasurementGroupBox.Text = "Resource Name and Measurement Type"
        '
        'deviceNameTextBox
        '
        Me.deviceNameTextBox.Location = New System.Drawing.Point(165, 30)
        Me.deviceNameTextBox.Margin = New System.Windows.Forms.Padding(2)
        Me.deviceNameTextBox.Name = "deviceNameTextBox"
        Me.deviceNameTextBox.Size = New System.Drawing.Size(126, 20)
        Me.deviceNameTextBox.TabIndex = 0
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 30)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 1
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(3, 175)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(297, 85)
        Me.messageGroupBox.TabIndex = 4
        Me.messageGroupBox.TabStop = False
        Me.messageGroupBox.Text = "Message"
        '
        'messageTextBox
        '
        Me.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.messageTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.messageTextBox.Location = New System.Drawing.Point(3, 16)
        Me.messageTextBox.Multiline = True
        Me.messageTextBox.Name = "messageTextBox"
        Me.messageTextBox.ReadOnly = True
        Me.messageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.messageTextBox.Size = New System.Drawing.Size(291, 66)
        Me.messageTextBox.TabIndex = 0
        '
        'MainForm
        '
        Me.AcceptButton = Me.readButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(595, 270)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.acConfigurationGroupBox)
        Me.Controls.Add(Me.ResourceAndMeasurementGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.Text = "IVI Current Measurement"
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.acConfigurationGroupBox.ResumeLayout(False)
        Me.acConfigurationGroupBox.PerformLayout()
        Me.ResourceAndMeasurementGroupBox.ResumeLayout(False)
        Me.ResourceAndMeasurementGroupBox.PerformLayout()
        Me.messageGroupBox.ResumeLayout(False)
        Me.messageGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents measurementGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents actualRangeLabel As System.Windows.Forms.Label
    Private WithEvents actualRangeTextBox As System.Windows.Forms.TextBox
    Private WithEvents readButton As System.Windows.Forms.Button
    Private WithEvents measurementLabel As System.Windows.Forms.Label
    Private WithEvents measurementTextBox As System.Windows.Forms.TextBox
    Private WithEvents rangeTextBox As System.Windows.Forms.TextBox
    Private WithEvents measurementModeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents rangeLabel As System.Windows.Forms.Label
    Private WithEvents configurationGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents powerlineFrequencyLabel As System.Windows.Forms.Label
    Private WithEvents resolutionLabel As System.Windows.Forms.Label
    Private WithEvents powerlineFrequencyValueComboBox As System.Windows.Forms.ComboBox
    Private WithEvents resolutionValueComboBox As System.Windows.Forms.ComboBox
    Private WithEvents acConfigurationGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents maxACFrequencyTextBox As System.Windows.Forms.TextBox
    Private WithEvents minACFrequencyTextBox As System.Windows.Forms.TextBox
    Private WithEvents maxACFrequencyLabel As System.Windows.Forms.Label
    Private WithEvents minACFrequencyLabel As System.Windows.Forms.Label
    Private WithEvents measurementTypeLabel As System.Windows.Forms.Label
    Private WithEvents ResourceAndMeasurementGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents resourceNameLabel As System.Windows.Forms.Label
    Friend WithEvents deviceNameTextBox As System.Windows.Forms.TextBox
    Private WithEvents messageGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents messageTextBox As System.Windows.Forms.TextBox

End Class


