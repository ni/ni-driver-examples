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
        Me.resourceAndMeasurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.acquisitionModeLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.acquisitionModeComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.rateValueTextBox = New System.Windows.Forms.TextBox()
        Me.rangeNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.rateLabel = New System.Windows.Forms.Label()
        Me.rangeLabel = New System.Windows.Forms.Label()
        Me.errorGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.TextBox()
        Me.measurementGroupBox = New System.Windows.Forms.GroupBox()
        Me.buttonsPanel = New System.Windows.Forms.Panel()
        Me.clearButton = New System.Windows.Forms.Button()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.numberOfSamplesNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.actualRangeTextBox = New System.Windows.Forms.TextBox()
        Me.actualRangeLabel = New System.Windows.Forms.Label()
        Me.readingDataGridView = New System.Windows.Forms.DataGridView()
        Me.Index = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.reading = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.numberOfSamplesLabel = New System.Windows.Forms.Label()
        Me.resourceAndMeasurementGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.rangeNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.errorGroupBox.SuspendLayout()
        Me.measurementGroupBox.SuspendLayout()
        Me.buttonsPanel.SuspendLayout()
        CType(Me.numberOfSamplesNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.readingDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'resourceAndMeasurementGroupBox
        '
        Me.resourceAndMeasurementGroupBox.Controls.Add(Me.acquisitionModeLabel)
        Me.resourceAndMeasurementGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.resourceAndMeasurementGroupBox.Controls.Add(Me.acquisitionModeComboBox)
        Me.resourceAndMeasurementGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceAndMeasurementGroupBox.Location = New System.Drawing.Point(10, 11)
        Me.resourceAndMeasurementGroupBox.Name = "resourceAndMeasurementGroupBox"
        Me.resourceAndMeasurementGroupBox.Size = New System.Drawing.Size(264, 85)
        Me.resourceAndMeasurementGroupBox.TabIndex = 0
        Me.resourceAndMeasurementGroupBox.TabStop = False
        Me.resourceAndMeasurementGroupBox.Text = "Resource Name and Measurement Type"
        '
        'acquisitionModeLabel
        '
        Me.acquisitionModeLabel.AutoSize = True
        Me.acquisitionModeLabel.Location = New System.Drawing.Point(10, 55)
        Me.acquisitionModeLabel.Name = "acquisitionModeLabel"
        Me.acquisitionModeLabel.Size = New System.Drawing.Size(91, 13)
        Me.acquisitionModeLabel.TabIndex = 3
        Me.acquisitionModeLabel.Text = "Acquisition Mode:"
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
        'acquisitionModeComboBox
        '
        Me.acquisitionModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.acquisitionModeComboBox.FormattingEnabled = True
        Me.acquisitionModeComboBox.Location = New System.Drawing.Point(140, 55)
        Me.acquisitionModeComboBox.Name = "acquisitionModeComboBox"
        Me.acquisitionModeComboBox.Size = New System.Drawing.Size(118, 21)
        Me.acquisitionModeComboBox.TabIndex = 1
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(140, 26)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(118, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.rateValueTextBox)
        Me.configurationGroupBox.Controls.Add(Me.rangeNumericUpDown)
        Me.configurationGroupBox.Controls.Add(Me.rateLabel)
        Me.configurationGroupBox.Controls.Add(Me.rangeLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(10, 110)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(265, 93)
        Me.configurationGroupBox.TabIndex = 1
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'rateValueTextBox
        '
        Me.rateValueTextBox.Location = New System.Drawing.Point(140, 55)
        Me.rateValueTextBox.Margin = New System.Windows.Forms.Padding(2)
        Me.rateValueTextBox.Name = "rateValueTextBox"
        Me.rateValueTextBox.Size = New System.Drawing.Size(118, 20)
        Me.rateValueTextBox.TabIndex = 2
        Me.rateValueTextBox.Text = "1.80E+6"
        '
        'rangeNumericUpDown
        '
        Me.rangeNumericUpDown.DecimalPlaces = 3
        Me.rangeNumericUpDown.Location = New System.Drawing.Point(140, 27)
        Me.rangeNumericUpDown.Maximum = New Decimal(New Integer() {1215752192, 23, 0, 0})
        Me.rangeNumericUpDown.Name = "rangeNumericUpDown"
        Me.rangeNumericUpDown.Size = New System.Drawing.Size(117, 20)
        Me.rangeNumericUpDown.TabIndex = 1
        Me.rangeNumericUpDown.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'rateLabel
        '
        Me.rateLabel.AutoSize = True
        Me.rateLabel.Location = New System.Drawing.Point(10, 55)
        Me.rateLabel.Name = "rateLabel"
        Me.rateLabel.Size = New System.Drawing.Size(122, 13)
        Me.rateLabel.TabIndex = 6
        Me.rateLabel.Text = "Rate (Samples/second):"
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
        'errorGroupBox
        '
        Me.errorGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.errorGroupBox.Controls.Add(Me.messageTextBox)
        Me.errorGroupBox.Location = New System.Drawing.Point(10, 210)
        Me.errorGroupBox.Name = "errorGroupBox"
        Me.errorGroupBox.Size = New System.Drawing.Size(264, 395)
        Me.errorGroupBox.TabIndex = 3
        Me.errorGroupBox.TabStop = False
        Me.errorGroupBox.Text = "Message"
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
        Me.messageTextBox.Size = New System.Drawing.Size(258, 376)
        Me.messageTextBox.TabIndex = 0
        '
        'measurementGroupBox
        '
        Me.measurementGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.measurementGroupBox.Controls.Add(Me.buttonsPanel)
        Me.measurementGroupBox.Controls.Add(Me.numberOfSamplesNumericUpDown)
        Me.measurementGroupBox.Controls.Add(Me.actualRangeTextBox)
        Me.measurementGroupBox.Controls.Add(Me.actualRangeLabel)
        Me.measurementGroupBox.Controls.Add(Me.readingDataGridView)
        Me.measurementGroupBox.Controls.Add(Me.numberOfSamplesLabel)
        Me.measurementGroupBox.Location = New System.Drawing.Point(288, 11)
        Me.measurementGroupBox.Name = "measurementGroupBox"
        Me.measurementGroupBox.Size = New System.Drawing.Size(304, 594)
        Me.measurementGroupBox.TabIndex = 2
        Me.measurementGroupBox.TabStop = False
        Me.measurementGroupBox.Text = "Measurement"
        '
        'buttonsPanel
        '
        Me.buttonsPanel.Controls.Add(Me.clearButton)
        Me.buttonsPanel.Controls.Add(Me.acquireButton)
        Me.buttonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.buttonsPanel.Location = New System.Drawing.Point(3, 553)
        Me.buttonsPanel.Name = "buttonsPanel"
        Me.buttonsPanel.Size = New System.Drawing.Size(298, 38)
        Me.buttonsPanel.TabIndex = 3
        '
        'clearButton
        '
        Me.clearButton.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.clearButton.Location = New System.Drawing.Point(152, 3)
        Me.clearButton.Name = "clearButton"
        Me.clearButton.Size = New System.Drawing.Size(75, 23)
        Me.clearButton.TabIndex = 1
        Me.clearButton.Text = "&Clear"
        Me.clearButton.UseVisualStyleBackColor = True
        '
        'acquireButton
        '
        Me.acquireButton.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.acquireButton.Location = New System.Drawing.Point(44, 3)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(75, 23)
        Me.acquireButton.TabIndex = 0
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'numberOfSamplesNumericUpDown
        '
        Me.numberOfSamplesNumericUpDown.Increment = New Decimal(New Integer() {100, 0, 0, 0})
        Me.numberOfSamplesNumericUpDown.Location = New System.Drawing.Point(180, 27)
        Me.numberOfSamplesNumericUpDown.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.numberOfSamplesNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numberOfSamplesNumericUpDown.Name = "numberOfSamplesNumericUpDown"
        Me.numberOfSamplesNumericUpDown.Size = New System.Drawing.Size(100, 20)
        Me.numberOfSamplesNumericUpDown.TabIndex = 0
        Me.numberOfSamplesNumericUpDown.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'actualRangeTextBox
        '
        Me.actualRangeTextBox.Location = New System.Drawing.Point(180, 55)
        Me.actualRangeTextBox.Name = "actualRangeTextBox"
        Me.actualRangeTextBox.ReadOnly = True
        Me.actualRangeTextBox.Size = New System.Drawing.Size(100, 20)
        Me.actualRangeTextBox.TabIndex = 1
        '
        'actualRangeLabel
        '
        Me.actualRangeLabel.AutoSize = True
        Me.actualRangeLabel.Location = New System.Drawing.Point(18, 55)
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
        Me.readingDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Index, Me.reading})
        Me.readingDataGridView.Location = New System.Drawing.Point(21, 91)
        Me.readingDataGridView.Name = "readingDataGridView"
        Me.readingDataGridView.ReadOnly = True
        Me.readingDataGridView.RowHeadersWidth = 15
        Me.readingDataGridView.RowTemplate.Height = 24
        Me.readingDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.readingDataGridView.Size = New System.Drawing.Size(260, 444)
        Me.readingDataGridView.StandardTab = True
        Me.readingDataGridView.TabIndex = 2
        '
        'Index
        '
        Me.Index.HeaderText = "Point"
        Me.Index.Name = "Index"
        Me.Index.ReadOnly = True
        '
        'reading
        '
        Me.reading.HeaderText = "Amplitude"
        Me.reading.Name = "reading"
        Me.reading.ReadOnly = True
        Me.reading.Width = 250
        '
        'numberOfSamplesLabel
        '
        Me.numberOfSamplesLabel.AutoSize = True
        Me.numberOfSamplesLabel.Location = New System.Drawing.Point(18, 26)
        Me.numberOfSamplesLabel.Name = "numberOfSamplesLabel"
        Me.numberOfSamplesLabel.Size = New System.Drawing.Size(104, 13)
        Me.numberOfSamplesLabel.TabIndex = 0
        Me.numberOfSamplesLabel.Text = "Number Of Samples:"
        '
        'MainForm
        '
        Me.AcceptButton = Me.acquireButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(598, 615)
        Me.Controls.Add(Me.measurementGroupBox)
        Me.Controls.Add(Me.errorGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.resourceAndMeasurementGroupBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximumSize = New System.Drawing.Size(614, 1632)
        Me.MinimumSize = New System.Drawing.Size(614, 648)
        Me.Name = "MainForm"
        Me.Text = "Waveform Acquisition"
        Me.resourceAndMeasurementGroupBox.ResumeLayout(False)
        Me.resourceAndMeasurementGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.rangeNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.errorGroupBox.ResumeLayout(False)
        Me.errorGroupBox.PerformLayout()
        Me.measurementGroupBox.ResumeLayout(False)
        Me.measurementGroupBox.PerformLayout()
        Me.buttonsPanel.ResumeLayout(False)
        CType(Me.numberOfSamplesNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.readingDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents resourceAndMeasurementGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents acquisitionModeLabel As System.Windows.Forms.Label
    Private WithEvents resourceNameLabel As System.Windows.Forms.Label
    Private WithEvents acquisitionModeComboBox As System.Windows.Forms.ComboBox
    Private WithEvents resourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents configurationGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents rateValueTextBox As System.Windows.Forms.TextBox
    Private WithEvents rangeNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents rateLabel As System.Windows.Forms.Label
    Private WithEvents rangeLabel As System.Windows.Forms.Label
    Private WithEvents errorGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents messageTextBox As System.Windows.Forms.TextBox
    Private WithEvents measurementGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents buttonsPanel As System.Windows.Forms.Panel
    Private WithEvents clearButton As System.Windows.Forms.Button
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private WithEvents numberOfSamplesNumericUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents actualRangeTextBox As System.Windows.Forms.TextBox
    Private WithEvents actualRangeLabel As System.Windows.Forms.Label
    Private WithEvents readingDataGridView As System.Windows.Forms.DataGridView
    Private WithEvents numberOfSamplesLabel As System.Windows.Forms.Label
    Friend WithEvents Index As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents reading As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
