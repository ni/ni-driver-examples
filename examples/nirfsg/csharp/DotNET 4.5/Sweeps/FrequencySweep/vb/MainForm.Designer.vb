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
		''' the contents of Me method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.frequencySweepGroupBox = New System.Windows.Forms.GroupBox()
        Me.startFrequencyLabel = New System.Windows.Forms.Label()
        Me.stopFrequencyLabel = New System.Windows.Forms.Label()
        Me.numberStepsLabel = New System.Windows.Forms.Label()
        Me.dwellTimeLabel = New System.Windows.Forms.Label()
        Me.startFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.stopFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.numberStepsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.dwellTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.actualCurrentFrequencyLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.actualCurrentFrequencyTextBox = New System.Windows.Forms.TextBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.frequencySweepGroupBox.SuspendLayout()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.stopFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberStepsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dwellTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'frequencySweepGroupBox
        '
        Me.frequencySweepGroupBox.Controls.Add(Me.startFrequencyLabel)
        Me.frequencySweepGroupBox.Controls.Add(Me.stopFrequencyLabel)
        Me.frequencySweepGroupBox.Controls.Add(Me.numberStepsLabel)
        Me.frequencySweepGroupBox.Controls.Add(Me.dwellTimeLabel)
        Me.frequencySweepGroupBox.Controls.Add(Me.startFrequencyNumeric)
        Me.frequencySweepGroupBox.Controls.Add(Me.stopFrequencyNumeric)
        Me.frequencySweepGroupBox.Controls.Add(Me.numberStepsNumeric)
        Me.frequencySweepGroupBox.Controls.Add(Me.dwellTimeNumeric)
        Me.frequencySweepGroupBox.Location = New System.Drawing.Point(166, 21)
        Me.frequencySweepGroupBox.Name = "frequencySweepGroupBox"
        Me.frequencySweepGroupBox.Size = New System.Drawing.Size(177, 223)
        Me.frequencySweepGroupBox.TabIndex = 3
        Me.frequencySweepGroupBox.TabStop = False
        Me.frequencySweepGroupBox.Text = "Frequency Sweep Parameters"
        '
        'startFrequencyLabel
        '
        Me.startFrequencyLabel.AutoSize = True
        Me.startFrequencyLabel.Location = New System.Drawing.Point(22, 22)
        Me.startFrequencyLabel.Name = "startFrequencyLabel"
        Me.startFrequencyLabel.Size = New System.Drawing.Size(104, 13)
        Me.startFrequencyLabel.TabIndex = 2
        Me.startFrequencyLabel.Text = "Start Frequency [Hz]"
        '
        'stopFrequencyLabel
        '
        Me.stopFrequencyLabel.AutoSize = True
        Me.stopFrequencyLabel.Location = New System.Drawing.Point(22, 71)
        Me.stopFrequencyLabel.Name = "stopFrequencyLabel"
        Me.stopFrequencyLabel.Size = New System.Drawing.Size(101, 13)
        Me.stopFrequencyLabel.TabIndex = 3
        Me.stopFrequencyLabel.Text = "End Frequency [Hz]"
        '
        'numberStepsLabel
        '
        Me.numberStepsLabel.AutoSize = True
        Me.numberStepsLabel.Location = New System.Drawing.Point(22, 123)
        Me.numberStepsLabel.Name = "numberStepsLabel"
        Me.numberStepsLabel.Size = New System.Drawing.Size(86, 13)
        Me.numberStepsLabel.TabIndex = 4
        Me.numberStepsLabel.Text = "Number of Steps"
        '
        'dwellTimeLabel
        '
        Me.dwellTimeLabel.AutoSize = True
        Me.dwellTimeLabel.Location = New System.Drawing.Point(22, 173)
        Me.dwellTimeLabel.Name = "dwellTimeLabel"
        Me.dwellTimeLabel.Size = New System.Drawing.Size(137, 13)
        Me.dwellTimeLabel.TabIndex = 5
        Me.dwellTimeLabel.Text = "Dwell Time in Each Step [s]"
        '
        'startFrequencyNumeric
        '
        Me.startFrequencyNumeric.DecimalPlaces = 6
        Me.startFrequencyNumeric.Location = New System.Drawing.Point(25, 40)
        Me.startFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.startFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.startFrequencyNumeric.Name = "startFrequencyNumeric"
        Me.startFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.startFrequencyNumeric.TabIndex = 2
        Me.startFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'stopFrequencyNumeric
        '
        Me.stopFrequencyNumeric.DecimalPlaces = 6
        Me.stopFrequencyNumeric.Location = New System.Drawing.Point(25, 90)
        Me.stopFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.stopFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.stopFrequencyNumeric.Name = "stopFrequencyNumeric"
        Me.stopFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.stopFrequencyNumeric.TabIndex = 3
        Me.stopFrequencyNumeric.Value = New Decimal(New Integer() {1020000000, 0, 0, 0})
        '
        'numberStepsNumeric
        '
        Me.numberStepsNumeric.Location = New System.Drawing.Point(25, 142)
        Me.numberStepsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numberStepsNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numberStepsNumeric.Name = "numberStepsNumeric"
        Me.numberStepsNumeric.Size = New System.Drawing.Size(120, 20)
        Me.numberStepsNumeric.TabIndex = 4
        Me.numberStepsNumeric.Value = New Decimal(New Integer() {21, 0, 0, 0})
        '
        'dwellTimeNumeric
        '
        Me.dwellTimeNumeric.DecimalPlaces = 3
        Me.dwellTimeNumeric.Location = New System.Drawing.Point(25, 192)
        Me.dwellTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.dwellTimeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.dwellTimeNumeric.Name = "dwellTimeNumeric"
        Me.dwellTimeNumeric.Size = New System.Drawing.Size(120, 20)
        Me.dwellTimeNumeric.TabIndex = 5
        Me.dwellTimeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(19, 40)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'powerLevelLabel
        '
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(19, 90)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.powerLevelLabel.TabIndex = 1
        Me.powerLevelLabel.Text = "Power Level [dBm]"
        '
        'actualCurrentFrequencyLabel
        '
        Me.actualCurrentFrequencyLabel.AutoSize = True
        Me.actualCurrentFrequencyLabel.Location = New System.Drawing.Point(401, 89)
        Me.actualCurrentFrequencyLabel.Name = "actualCurrentFrequencyLabel"
        Me.actualCurrentFrequencyLabel.Size = New System.Drawing.Size(116, 13)
        Me.actualCurrentFrequencyLabel.TabIndex = 6
        Me.actualCurrentFrequencyLabel.Text = "Current Frequency [Hz]"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(19, 258)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 7
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'powerLevelNumeric
        '
        Me.powerLevelNumeric.DecimalPlaces = 2
        Me.powerLevelNumeric.Location = New System.Drawing.Point(19, 111)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerLevelNumeric.TabIndex = 1
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(19, 61)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'actualCurrentFrequencyTextBox
        '
        Me.actualCurrentFrequencyTextBox.Location = New System.Drawing.Point(401, 110)
        Me.actualCurrentFrequencyTextBox.Name = "actualCurrentFrequencyTextBox"
        Me.actualCurrentFrequencyTextBox.ReadOnly = True
        Me.actualCurrentFrequencyTextBox.Size = New System.Drawing.Size(75, 20)
        Me.actualCurrentFrequencyTextBox.TabIndex = 11
        Me.actualCurrentFrequencyTextBox.TabStop = False
        Me.actualCurrentFrequencyTextBox.Text = "0.00000"
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(19, 279)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(507, 34)
        Me.errorTextBox.TabIndex = 12
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(370, 35)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 4
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
                Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(451, 35)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 5
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '        '        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True        Me.ClientSize = New System.Drawing.Size(547, 360)
        Me.Controls.Add(Me.frequencySweepGroupBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.powerLevelLabel)
        Me.Controls.Add(Me.actualCurrentFrequencyLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.powerLevelNumeric)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.actualCurrentFrequencyTextBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.stopButton)        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Frequency Sweep"
        Me.frequencySweepGroupBox.ResumeLayout(False)
        Me.frequencySweepGroupBox.PerformLayout()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.stopFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberStepsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dwellTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private WithEvents frequencySweepGroupBox As System.Windows.Forms.GroupBox
    Private resourceNameLabel As System.Windows.Forms.Label
    Private powerLevelLabel As System.Windows.Forms.Label
    Private startFrequencyLabel As System.Windows.Forms.Label
    Private stopFrequencyLabel As System.Windows.Forms.Label
    Private numberStepsLabel As System.Windows.Forms.Label
    Private dwellTimeLabel As System.Windows.Forms.Label
    Private actualCurrentFrequencyLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private startFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private stopFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private numberStepsNumeric As System.Windows.Forms.NumericUpDown
		Private dwellTimeNumeric As System.Windows.Forms.NumericUpDown
		Private resourceNameComboBox As System.Windows.Forms.ComboBox
		Private actualCurrentFrequencyTextBox As System.Windows.Forms.TextBox
		Private errorTextBox As System.Windows.Forms.TextBox
		Private WithEvents startButton As System.Windows.Forms.Button
		Private WithEvents stopButton As System.Windows.Forms.Button		Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer

	End Class
