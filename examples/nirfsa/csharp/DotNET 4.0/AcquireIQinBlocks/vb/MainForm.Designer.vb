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
        Me.referenceLevelLabel = New System.Windows.Forms.Label()
        Me.carrierFrequencyLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.samplesPerRecordLabel = New System.Windows.Forms.Label()
        Me.samplesPerBlockLabel = New System.Windows.Forms.Label()
        Me.samplesReadLabel = New System.Windows.Forms.Label()
        Me.referenceLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.carrierFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.samplesPerRecordNumeric = New System.Windows.Forms.NumericUpDown()
        Me.samplesPerBlockNumeric = New System.Windows.Forms.NumericUpDown()
        Me.samplesReadTextBox = New System.Windows.Forms.TextBox()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.dataGridViewResults = New System.Windows.Forms.DataGridView()
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.carrierFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerRecordNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerBlockNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'referenceLevelLabel
        '
        Me.referenceLevelLabel.AutoSize = True
        Me.referenceLevelLabel.Location = New System.Drawing.Point(9, 83)
        Me.referenceLevelLabel.Name = "referenceLevelLabel"
        Me.referenceLevelLabel.Size = New System.Drawing.Size(116, 13)
        Me.referenceLevelLabel.TabIndex = 0
        Me.referenceLevelLabel.Text = "Reference Level (dBm)"
        '
        'carrierFrequencyLabel
        '
        Me.carrierFrequencyLabel.AutoSize = True
        Me.carrierFrequencyLabel.Location = New System.Drawing.Point(9, 146)
        Me.carrierFrequencyLabel.Name = "carrierFrequencyLabel"
        Me.carrierFrequencyLabel.Size = New System.Drawing.Size(112, 13)
        Me.carrierFrequencyLabel.TabIndex = 1
        Me.carrierFrequencyLabel.Text = "Carrier Frequency (Hz)"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(9, 209)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(70, 13)
        Me.iqRateLabel.TabIndex = 2
        Me.iqRateLabel.Text = "IQ Rate (S/s)"
        '
        'samplesPerRecordLabel
        '
        Me.samplesPerRecordLabel.AutoSize = True
        Me.samplesPerRecordLabel.Location = New System.Drawing.Point(9, 272)
        Me.samplesPerRecordLabel.Name = "samplesPerRecordLabel"
        Me.samplesPerRecordLabel.Size = New System.Drawing.Size(103, 13)
        Me.samplesPerRecordLabel.TabIndex = 3
        Me.samplesPerRecordLabel.Text = "Samples per Record"
        '
        'samplesPerBlockLabel
        '
        Me.samplesPerBlockLabel.AutoSize = True
        Me.samplesPerBlockLabel.Location = New System.Drawing.Point(9, 333)
        Me.samplesPerBlockLabel.Name = "samplesPerBlockLabel"
        Me.samplesPerBlockLabel.Size = New System.Drawing.Size(142, 13)
        Me.samplesPerBlockLabel.TabIndex = 4
        Me.samplesPerBlockLabel.Text = "Maximum Samples per Block"
        '
        'samplesReadLabel
        '
        Me.samplesReadLabel.AutoSize = True
        Me.samplesReadLabel.Location = New System.Drawing.Point(9, 396)
        Me.samplesReadLabel.Name = "samplesReadLabel"
        Me.samplesReadLabel.Size = New System.Drawing.Size(76, 13)
        Me.samplesReadLabel.TabIndex = 5
        Me.samplesReadLabel.Text = "Samples Read"
        '
        'referenceLevelNumeric
        '
        Me.referenceLevelNumeric.DecimalPlaces = 2
        Me.referenceLevelNumeric.Location = New System.Drawing.Point(12, 99)
        Me.referenceLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.referenceLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.referenceLevelNumeric.Name = "referenceLevelNumeric"
        Me.referenceLevelNumeric.Size = New System.Drawing.Size(116, 20)
        Me.referenceLevelNumeric.TabIndex = 1
        '
        'carrierFrequencyNumeric
        '
        Me.carrierFrequencyNumeric.DecimalPlaces = 2
        Me.carrierFrequencyNumeric.Location = New System.Drawing.Point(12, 162)
        Me.carrierFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.carrierFrequencyNumeric.Name = "carrierFrequencyNumeric"
        Me.carrierFrequencyNumeric.Size = New System.Drawing.Size(116, 20)
        Me.carrierFrequencyNumeric.TabIndex = 2
        Me.carrierFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 2
        Me.iqRateNumeric.Location = New System.Drawing.Point(12, 225)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(116, 20)
        Me.iqRateNumeric.TabIndex = 3
        Me.iqRateNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'samplesPerRecordNumeric
        '
        Me.samplesPerRecordNumeric.Location = New System.Drawing.Point(12, 288)
        Me.samplesPerRecordNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.samplesPerRecordNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerRecordNumeric.Name = "samplesPerRecordNumeric"
        Me.samplesPerRecordNumeric.Size = New System.Drawing.Size(116, 20)
        Me.samplesPerRecordNumeric.TabIndex = 4
        Me.samplesPerRecordNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'samplesPerBlockNumeric
        '
        Me.samplesPerBlockNumeric.Location = New System.Drawing.Point(12, 349)
        Me.samplesPerBlockNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.samplesPerBlockNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerBlockNumeric.Name = "samplesPerBlockNumeric"
        Me.samplesPerBlockNumeric.Size = New System.Drawing.Size(116, 20)
        Me.samplesPerBlockNumeric.TabIndex = 5
        Me.samplesPerBlockNumeric.Value = New Decimal(New Integer() {100000, 0, 0, 0})
        '
        'samplesReadTextBox
        '
        Me.samplesReadTextBox.Enabled = False
        Me.samplesReadTextBox.Location = New System.Drawing.Point(12, 412)
        Me.samplesReadTextBox.Name = "samplesReadTextBox"
        Me.samplesReadTextBox.Size = New System.Drawing.Size(116, 20)
        Me.samplesReadTextBox.TabIndex = 11
        Me.samplesReadTextBox.Text = "0"
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(12, 445)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(75, 23)
        Me.acquireButton.TabIndex = 6
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(9, 23)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 14
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 40)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(116, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'dataGridViewResults
        '
        Me.dataGridViewResults.AllowUserToAddRows = False
        Me.dataGridViewResults.AllowUserToDeleteRows = False
        Me.dataGridViewResults.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dataGridViewResults.Location = New System.Drawing.Point(175, 23)
        Me.dataGridViewResults.Name = "dataGridViewResults"
        Me.dataGridViewResults.Size = New System.Drawing.Size(448, 409)
        Me.dataGridViewResults.TabIndex = 9
        '
        'MainForm
        '
        Me.AcceptButton = Me.acquireButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(640, 480)
        Me.Controls.Add(Me.dataGridViewResults)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.acquireButton)
        Me.Controls.Add(Me.referenceLevelLabel)
        Me.Controls.Add(Me.carrierFrequencyLabel)
        Me.Controls.Add(Me.iqRateLabel)
        Me.Controls.Add(Me.samplesPerRecordLabel)
        Me.Controls.Add(Me.samplesPerBlockLabel)
        Me.Controls.Add(Me.samplesReadLabel)
        Me.Controls.Add(Me.referenceLevelNumeric)
        Me.Controls.Add(Me.carrierFrequencyNumeric)
        Me.Controls.Add(Me.iqRateNumeric)
        Me.Controls.Add(Me.samplesPerRecordNumeric)
        Me.Controls.Add(Me.samplesPerBlockNumeric)
        Me.Controls.Add(Me.samplesReadTextBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "RFSA Acquire IQ in Blocks"
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.carrierFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerRecordNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerBlockNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private referenceLevelLabel As System.Windows.Forms.Label
    Private carrierFrequencyLabel As System.Windows.Forms.Label
    Private iqRateLabel As System.Windows.Forms.Label
    Private samplesPerRecordLabel As System.Windows.Forms.Label
    Private samplesPerBlockLabel As System.Windows.Forms.Label
    Private samplesReadLabel As System.Windows.Forms.Label
    Private referenceLevelNumeric As System.Windows.Forms.NumericUpDown
    Private carrierFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private samplesPerRecordNumeric As System.Windows.Forms.NumericUpDown
    Private samplesPerBlockNumeric As System.Windows.Forms.NumericUpDown
    Private samplesReadTextBox As System.Windows.Forms.TextBox
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private resourceNameLabel As System.Windows.Forms.Label
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private dataGridViewResults As System.Windows.Forms.DataGridView

End Class
