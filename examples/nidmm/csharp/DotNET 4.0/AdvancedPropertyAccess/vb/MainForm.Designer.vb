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
        Me.actualRangeTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameAndMeasurementTypeGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementModeLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.measurementModeComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.rangeTextBox = New System.Windows.Forms.TextBox()
        Me.actualRangeLabel = New System.Windows.Forms.Label()
        Me.measurementsToAverageLabel = New System.Windows.Forms.Label()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.TextBox()
        Me.rangeLabel = New System.Windows.Forms.Label()
        Me.measurementsToAverageNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementLabel = New System.Windows.Forms.Label()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.readButton = New System.Windows.Forms.Button()
        Me.measurementTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameAndMeasurementTypeGroupBox.SuspendLayout()
        Me.messageGroupBox.SuspendLayout()
        CType(Me.measurementsToAverageNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.configurationGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'actualRangeTextBox
        '
        Me.actualRangeTextBox.Location = New System.Drawing.Point(147, 62)
        Me.actualRangeTextBox.Name = "actualRangeTextBox"
        Me.actualRangeTextBox.ReadOnly = True
        Me.actualRangeTextBox.Size = New System.Drawing.Size(100, 20)
        Me.actualRangeTextBox.TabIndex = 1
        '
        'resourceNameAndMeasurementTypeGroupBox
        '
        Me.resourceNameAndMeasurementTypeGroupBox.Controls.Add(Me.measurementModeLabel)
        Me.resourceNameAndMeasurementTypeGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.resourceNameAndMeasurementTypeGroupBox.Controls.Add(Me.measurementModeComboBox)
        Me.resourceNameAndMeasurementTypeGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceNameAndMeasurementTypeGroupBox.Location = New System.Drawing.Point(10, 12)
        Me.resourceNameAndMeasurementTypeGroupBox.Name = "resourceNameAndMeasurementTypeGroupBox"
        Me.resourceNameAndMeasurementTypeGroupBox.Size = New System.Drawing.Size(279, 113)
        Me.resourceNameAndMeasurementTypeGroupBox.TabIndex = 0
        Me.resourceNameAndMeasurementTypeGroupBox.TabStop = False
        Me.resourceNameAndMeasurementTypeGroupBox.Text = "Resource Name and Measurement Type"
        '
        'measurementModeLabel
        '
        Me.measurementModeLabel.AutoSize = True
        Me.measurementModeLabel.Location = New System.Drawing.Point(10, 65)
        Me.measurementModeLabel.Name = "measurementModeLabel"
        Me.measurementModeLabel.Size = New System.Drawing.Size(104, 13)
        Me.measurementModeLabel.TabIndex = 3
        Me.measurementModeLabel.Text = "Measurement Mode:"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(10, 32)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 2
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'measurementModeComboBox
        '
        Me.measurementModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.measurementModeComboBox.FormattingEnabled = True
        Me.measurementModeComboBox.Location = New System.Drawing.Point(147, 62)
        Me.measurementModeComboBox.Name = "measurementModeComboBox"
        Me.measurementModeComboBox.Size = New System.Drawing.Size(121, 21)
        Me.measurementModeComboBox.TabIndex = 1
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(147, 28)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'rangeTextBox
        '
        Me.rangeTextBox.Location = New System.Drawing.Point(147, 29)
        Me.rangeTextBox.Name = "rangeTextBox"
        Me.rangeTextBox.Size = New System.Drawing.Size(100, 20)
        Me.rangeTextBox.TabIndex = 0
        Me.rangeTextBox.Text = "1.00E-010"
        '
        'actualRangeLabel
        '
        Me.actualRangeLabel.AutoSize = True
        Me.actualRangeLabel.Location = New System.Drawing.Point(6, 64)
        Me.actualRangeLabel.Name = "actualRangeLabel"
        Me.actualRangeLabel.Size = New System.Drawing.Size(75, 13)
        Me.actualRangeLabel.TabIndex = 3
        Me.actualRangeLabel.Text = "Actual Range:"
        '
        'measurementsToAverageLabel
        '
        Me.measurementsToAverageLabel.AutoSize = True
        Me.measurementsToAverageLabel.Location = New System.Drawing.Point(6, 65)
        Me.measurementsToAverageLabel.Name = "measurementsToAverageLabel"
        Me.measurementsToAverageLabel.Size = New System.Drawing.Size(134, 13)
        Me.measurementsToAverageLabel.TabIndex = 3
        Me.measurementsToAverageLabel.Text = "Measurements to Average:"
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(10, 131)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(279, 131)
        Me.messageGroupBox.TabIndex = 2
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
        Me.messageTextBox.Size = New System.Drawing.Size(273, 112)
        Me.messageTextBox.TabIndex = 0
        '
        'rangeLabel
        '
        Me.rangeLabel.AutoSize = True
        Me.rangeLabel.Location = New System.Drawing.Point(6, 32)
        Me.rangeLabel.Name = "rangeLabel"
        Me.rangeLabel.Size = New System.Drawing.Size(42, 13)
        Me.rangeLabel.TabIndex = 2
        Me.rangeLabel.Text = "Range:"
        '
        'measurementsToAverageNumericUpDown
        '
        Me.measurementsToAverageNumericUpDown.Location = New System.Drawing.Point(147, 63)
        Me.measurementsToAverageNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.measurementsToAverageNumericUpDown.Name = "measurementsToAverageNumericUpDown"
        Me.measurementsToAverageNumericUpDown.Size = New System.Drawing.Size(99, 20)
        Me.measurementsToAverageNumericUpDown.TabIndex = 1
        Me.measurementsToAverageNumericUpDown.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.rangeTextBox)
        Me.configurationGroupBox.Controls.Add(Me.measurementsToAverageLabel)
        Me.configurationGroupBox.Controls.Add(Me.rangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.measurementsToAverageNumericUpDown)
        Me.configurationGroupBox.Location = New System.Drawing.Point(295, 12)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(271, 113)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'measurementLabel
        '
        Me.measurementLabel.AutoSize = True
        Me.measurementLabel.Location = New System.Drawing.Point(6, 29)
        Me.measurementLabel.Name = "measurementLabel"
        Me.measurementLabel.Size = New System.Drawing.Size(74, 13)
        Me.measurementLabel.TabIndex = 1
        Me.measurementLabel.Text = "Measurement:"
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Controls.Add(Me.actualRangeTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualRangeLabel)
        Me.measurementGroupBox.Controls.Add(Me.readButton)
        Me.measurementGroupBox.Controls.Add(Me.measurementLabel)
        Me.measurementGroupBox.Controls.Add(Me.measurementTextBox)
        Me.measurementGroupBox.Location = New System.Drawing.Point(295, 131)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(271, 131)
        Me.measurementGroupBox.TabIndex = 3
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'readButton
        '
        Me.readButton.Location = New System.Drawing.Point(147, 102)
        Me.readButton.Name = "readButton"
        Me.readButton.Size = New System.Drawing.Size(75, 23)
        Me.readButton.TabIndex = 3
        Me.readButton.Text = "&Read"
        Me.readButton.UseVisualStyleBackColor = True
        '
        'measurementTextBox
        '
        Me.measurementTextBox.Location = New System.Drawing.Point(147, 27)
        Me.measurementTextBox.Name = "measurementTextBox"
        Me.measurementTextBox.ReadOnly = True
        Me.measurementTextBox.Size = New System.Drawing.Size(100, 20)
        Me.measurementTextBox.TabIndex = 0
        '
        'MainForm
        '
        Me.AcceptButton = Me.readButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(575, 270)
        Me.Controls.Add(Me.resourceNameAndMeasurementTypeGroupBox)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Advanced Property Access"
        Me.resourceNameAndMeasurementTypeGroupBox.ResumeLayout(False)
        Me.resourceNameAndMeasurementTypeGroupBox.PerformLayout()
        Me.messageGroupBox.ResumeLayout(False)
        Me.messageGroupBox.PerformLayout()
        CType(Me.measurementsToAverageNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents actualRangeTextBox As System.Windows.Forms.TextBox
    Private WithEvents resourceNameAndMeasurementTypeGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents measurementModeLabel As System.Windows.Forms.Label
    Private WithEvents resourceNameLabel As System.Windows.Forms.Label
    Private WithEvents measurementModeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents resourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents rangeTextBox As System.Windows.Forms.TextBox
    Private WithEvents actualRangeLabel As System.Windows.Forms.Label
    Private WithEvents measurementsToAverageLabel As System.Windows.Forms.Label
    Private WithEvents messageGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents messageTextBox As System.Windows.Forms.TextBox
    Private WithEvents rangeLabel As System.Windows.Forms.Label
    Private WithEvents measurementsToAverageNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents configurationGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents measurementLabel As System.Windows.Forms.Label
    Private WithEvents measurementGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents readButton As System.Windows.Forms.Button
    Private WithEvents measurementTextBox As System.Windows.Forms.TextBox

End Class
