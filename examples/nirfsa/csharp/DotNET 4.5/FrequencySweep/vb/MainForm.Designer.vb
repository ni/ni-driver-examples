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
        Me.startFrequencyLabel = New System.Windows.Forms.Label()
        Me.stopFrequencyLabel = New System.Windows.Forms.Label()
        Me.resolutionBandwidthLabel = New System.Windows.Forms.Label()
        Me.numStepsLabel = New System.Windows.Forms.Label()
        Me.currentCenterFrequencyLabel = New System.Windows.Forms.Label()
        Me.referenceLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.stopFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resolutionBandwidthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.numStepsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.currentCenterFrequencyTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.dataGridViewResults = New System.Windows.Forms.DataGridView()
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.stopFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.resolutionBandwidthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numStepsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'referenceLevelLabel
        '
        Me.referenceLevelLabel.AutoSize = True
        Me.referenceLevelLabel.Location = New System.Drawing.Point(9, 71)
        Me.referenceLevelLabel.Name = "referenceLevelLabel"
        Me.referenceLevelLabel.Size = New System.Drawing.Size(116, 13)
        Me.referenceLevelLabel.TabIndex = 0
        Me.referenceLevelLabel.Text = "Reference Level (dBm)"
        '
        'startFrequencyLabel
        '
        Me.startFrequencyLabel.AutoSize = True
        Me.startFrequencyLabel.Location = New System.Drawing.Point(9, 121)
        Me.startFrequencyLabel.Name = "startFrequencyLabel"
        Me.startFrequencyLabel.Size = New System.Drawing.Size(138, 13)
        Me.startFrequencyLabel.TabIndex = 1
        Me.startFrequencyLabel.Text = "Start Center Frequency (Hz)"
        '
        'stopFrequencyLabel
        '
        Me.stopFrequencyLabel.AutoSize = True
        Me.stopFrequencyLabel.Location = New System.Drawing.Point(9, 171)
        Me.stopFrequencyLabel.Name = "stopFrequencyLabel"
        Me.stopFrequencyLabel.Size = New System.Drawing.Size(138, 13)
        Me.stopFrequencyLabel.TabIndex = 2
        Me.stopFrequencyLabel.Text = "Stop Center Frequency (Hz)"
        '
        'resolutionBandwidthLabel
        '
        Me.resolutionBandwidthLabel.AutoSize = True
        Me.resolutionBandwidthLabel.Location = New System.Drawing.Point(9, 221)
        Me.resolutionBandwidthLabel.Name = "resolutionBandwidthLabel"
        Me.resolutionBandwidthLabel.Size = New System.Drawing.Size(132, 13)
        Me.resolutionBandwidthLabel.TabIndex = 3
        Me.resolutionBandwidthLabel.Text = "Resolution Bandwidth (Hz)"
        '
        'numStepsLabel
        '
        Me.numStepsLabel.AutoSize = True
        Me.numStepsLabel.Location = New System.Drawing.Point(9, 271)
        Me.numStepsLabel.Name = "numStepsLabel"
        Me.numStepsLabel.Size = New System.Drawing.Size(34, 13)
        Me.numStepsLabel.TabIndex = 4
        Me.numStepsLabel.Text = "Steps"
        '
        'currentCenterFrequencyLabel
        '
        Me.currentCenterFrequencyLabel.AutoSize = True
        Me.currentCenterFrequencyLabel.Location = New System.Drawing.Point(249, 303)
        Me.currentCenterFrequencyLabel.Name = "currentCenterFrequencyLabel"
        Me.currentCenterFrequencyLabel.Size = New System.Drawing.Size(128, 13)
        Me.currentCenterFrequencyLabel.TabIndex = 5
        Me.currentCenterFrequencyLabel.Text = "Current Center Frequency"
        '
        'referenceLevelNumeric
        '
        Me.referenceLevelNumeric.DecimalPlaces = 2
        Me.referenceLevelNumeric.Location = New System.Drawing.Point(12, 87)
        Me.referenceLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.referenceLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.referenceLevelNumeric.Name = "referenceLevelNumeric"
        Me.referenceLevelNumeric.Size = New System.Drawing.Size(116, 20)
        Me.referenceLevelNumeric.TabIndex = 1
        '
        'startFrequencyNumeric
        '
        Me.startFrequencyNumeric.DecimalPlaces = 2
        Me.startFrequencyNumeric.Location = New System.Drawing.Point(12, 137)
        Me.startFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.startFrequencyNumeric.Name = "startFrequencyNumeric"
        Me.startFrequencyNumeric.Size = New System.Drawing.Size(116, 20)
        Me.startFrequencyNumeric.TabIndex = 2
        Me.startFrequencyNumeric.Value = New Decimal(New Integer() {990000000, 0, 0, 0})
        '
        'stopFrequencyNumeric
        '
        Me.stopFrequencyNumeric.DecimalPlaces = 2
        Me.stopFrequencyNumeric.Location = New System.Drawing.Point(12, 187)
        Me.stopFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.stopFrequencyNumeric.Name = "stopFrequencyNumeric"
        Me.stopFrequencyNumeric.Size = New System.Drawing.Size(116, 20)
        Me.stopFrequencyNumeric.TabIndex = 3
        Me.stopFrequencyNumeric.Value = New Decimal(New Integer() {1010000000, 0, 0, 0})
        '
        'resolutionBandwidthNumeric
        '
        Me.resolutionBandwidthNumeric.DecimalPlaces = 2
        Me.resolutionBandwidthNumeric.Location = New System.Drawing.Point(12, 237)
        Me.resolutionBandwidthNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.resolutionBandwidthNumeric.Name = "resolutionBandwidthNumeric"
        Me.resolutionBandwidthNumeric.Size = New System.Drawing.Size(116, 20)
        Me.resolutionBandwidthNumeric.TabIndex = 4
        Me.resolutionBandwidthNumeric.Value = New Decimal(New Integer() {10000, 0, 0, 0})
        '
        'numStepsNumeric
        '
        Me.numStepsNumeric.Location = New System.Drawing.Point(12, 287)
        Me.numStepsNumeric.Maximum = New Decimal(New Integer() {-1, 0, 0, 0})
        Me.numStepsNumeric.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.numStepsNumeric.Name = "numStepsNumeric"
        Me.numStepsNumeric.Size = New System.Drawing.Size(116, 20)
        Me.numStepsNumeric.TabIndex = 5
        Me.numStepsNumeric.Value = New Decimal(New Integer() {128, 0, 0, 0})
        '
        'currentCenterFrequencyTextBox
        '
        Me.currentCenterFrequencyTextBox.Location = New System.Drawing.Point(249, 324)
        Me.currentCenterFrequencyTextBox.Name = "currentCenterFrequencyTextBox"
        Me.currentCenterFrequencyTextBox.ReadOnly = True
        Me.currentCenterFrequencyTextBox.Size = New System.Drawing.Size(128, 20)
        Me.currentCenterFrequencyTextBox.TabIndex = 10
        Me.currentCenterFrequencyTextBox.Text = "0.00000000000000E+0"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(12, 16)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 11
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 32)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(116, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(12, 320)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 6
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'dataGridViewResults
        '
        Me.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dataGridViewResults.Location = New System.Drawing.Point(153, 16)
        Me.dataGridViewResults.Name = "dataGridViewResults"
        Me.dataGridViewResults.Size = New System.Drawing.Size(359, 268)
        Me.dataGridViewResults.TabIndex = 9
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(524, 360)
        Me.Controls.Add(Me.dataGridViewResults)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.referenceLevelLabel)
        Me.Controls.Add(Me.startFrequencyLabel)
        Me.Controls.Add(Me.stopFrequencyLabel)
        Me.Controls.Add(Me.resolutionBandwidthLabel)
        Me.Controls.Add(Me.numStepsLabel)
        Me.Controls.Add(Me.currentCenterFrequencyLabel)
        Me.Controls.Add(Me.referenceLevelNumeric)
        Me.Controls.Add(Me.startFrequencyNumeric)
        Me.Controls.Add(Me.stopFrequencyNumeric)
        Me.Controls.Add(Me.resolutionBandwidthNumeric)
        Me.Controls.Add(Me.numStepsNumeric)
        Me.Controls.Add(Me.currentCenterFrequencyTextBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "RFSA Frequency Sweep"
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.stopFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.resolutionBandwidthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numStepsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridViewResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private referenceLevelLabel As System.Windows.Forms.Label
    Private startFrequencyLabel As System.Windows.Forms.Label
    Private stopFrequencyLabel As System.Windows.Forms.Label
    Private resolutionBandwidthLabel As System.Windows.Forms.Label
    Private numStepsLabel As System.Windows.Forms.Label
    Private currentCenterFrequencyLabel As System.Windows.Forms.Label
    Private referenceLevelNumeric As System.Windows.Forms.NumericUpDown
    Private startFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private stopFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private resolutionBandwidthNumeric As System.Windows.Forms.NumericUpDown
    Private numStepsNumeric As System.Windows.Forms.NumericUpDown
    Private currentCenterFrequencyTextBox As System.Windows.Forms.TextBox
    Private resourceNameLabel As System.Windows.Forms.Label
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private dataGridViewResults As System.Windows.Forms.DataGridView

End Class
