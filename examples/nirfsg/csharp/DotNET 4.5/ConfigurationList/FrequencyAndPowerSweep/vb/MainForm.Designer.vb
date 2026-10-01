Partial Class MainForm
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.configurationListParametersGroupBox = New System.Windows.Forms.GroupBox()
        Me.startFrequencyLabel = New System.Windows.Forms.Label()
        Me.startPowerLabel = New System.Windows.Forms.Label()
        Me.stopPowerLabel = New System.Windows.Forms.Label()
        Me.endFrequencyLabel = New System.Windows.Forms.Label()
        Me.numberStepsLabel = New System.Windows.Forms.Label()
        Me.startFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startPowerNumeric = New System.Windows.Forms.NumericUpDown()
        Me.stopPowerNumeric = New System.Windows.Forms.NumericUpDown()
        Me.endFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.numberStepsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.listTriggerSourceLabel = New System.Windows.Forms.Label()
        Me.dwellTimeLabel = New System.Windows.Forms.Label()
        Me.frequencySettlingsLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.referenceClockSourceLabel = New System.Windows.Forms.Label()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.startButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.configurationListTriggerGroupBox = New System.Windows.Forms.GroupBox()
        Me.frequencySettlingsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.dwellTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.listTriggerSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.referenceClockSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.configurationListParametersGroupBox.SuspendLayout()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.startPowerNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.stopPowerNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.endFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberStepsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.configurationListTriggerGroupBox.SuspendLayout()
        CType(Me.frequencySettlingsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dwellTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'configurationListParametersGroupBox
        '
        Me.configurationListParametersGroupBox.Controls.Add(Me.startFrequencyLabel)
        Me.configurationListParametersGroupBox.Controls.Add(Me.startPowerLabel)
        Me.configurationListParametersGroupBox.Controls.Add(Me.stopPowerLabel)
        Me.configurationListParametersGroupBox.Controls.Add(Me.endFrequencyLabel)
        Me.configurationListParametersGroupBox.Controls.Add(Me.numberStepsLabel)
        Me.configurationListParametersGroupBox.Controls.Add(Me.startFrequencyNumeric)
        Me.configurationListParametersGroupBox.Controls.Add(Me.startPowerNumeric)
        Me.configurationListParametersGroupBox.Controls.Add(Me.stopPowerNumeric)
        Me.configurationListParametersGroupBox.Controls.Add(Me.endFrequencyNumeric)
        Me.configurationListParametersGroupBox.Controls.Add(Me.numberStepsNumeric)
        Me.configurationListParametersGroupBox.Location = New System.Drawing.Point(170, 25)
        Me.configurationListParametersGroupBox.Name = "configurationListParametersGroupBox"
        Me.configurationListParametersGroupBox.Size = New System.Drawing.Size(177, 262)
        Me.configurationListParametersGroupBox.TabIndex = 3
        Me.configurationListParametersGroupBox.TabStop = False
        Me.configurationListParametersGroupBox.Text = "Configuration List Parameters"
        '
        'startFrequencyLabel
        '
        Me.startFrequencyLabel.AutoSize = True
        Me.startFrequencyLabel.Location = New System.Drawing.Point(24, 20)
        Me.startFrequencyLabel.Name = "startFrequencyLabel"
        Me.startFrequencyLabel.Size = New System.Drawing.Size(104, 13)
        Me.startFrequencyLabel.TabIndex = 1
        Me.startFrequencyLabel.Text = "Start Frequency [Hz]"
        '
        'startPowerLabel
        '
        Me.startPowerLabel.AutoSize = True
        Me.startPowerLabel.Location = New System.Drawing.Point(21, 118)
        Me.startPowerLabel.Name = "startPowerLabel"
        Me.startPowerLabel.Size = New System.Drawing.Size(92, 13)
        Me.startPowerLabel.TabIndex = 2
        Me.startPowerLabel.Text = "Start Power [dBm]"
        '
        'stopPowerLabel
        '
        Me.stopPowerLabel.AutoSize = True
        Me.stopPowerLabel.Location = New System.Drawing.Point(21, 167)
        Me.stopPowerLabel.Name = "stopPowerLabel"
        Me.stopPowerLabel.Size = New System.Drawing.Size(92, 13)
        Me.stopPowerLabel.TabIndex = 3
        Me.stopPowerLabel.Text = "Stop Power [dBm]"
        '
        'endFrequencyLabel
        '
        Me.endFrequencyLabel.AutoSize = True
        Me.endFrequencyLabel.Location = New System.Drawing.Point(25, 69)
        Me.endFrequencyLabel.Name = "endFrequencyLabel"
        Me.endFrequencyLabel.Size = New System.Drawing.Size(101, 13)
        Me.endFrequencyLabel.TabIndex = 4
        Me.endFrequencyLabel.Text = "End Frequency [Hz]"
        '
        'numberStepsLabel
        '
        Me.numberStepsLabel.AutoSize = True
        Me.numberStepsLabel.Location = New System.Drawing.Point(25, 216)
        Me.numberStepsLabel.Name = "numberStepsLabel"
        Me.numberStepsLabel.Size = New System.Drawing.Size(86, 13)
        Me.numberStepsLabel.TabIndex = 5
        Me.numberStepsLabel.Text = "Number of Steps"
        '
        'startFrequencyNumeric
        '
        Me.startFrequencyNumeric.DecimalPlaces = 6
        Me.startFrequencyNumeric.Location = New System.Drawing.Point(21, 37)
        Me.startFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.startFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.startFrequencyNumeric.Name = "startFrequencyNumeric"
        Me.startFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.startFrequencyNumeric.TabIndex = 1
        Me.startFrequencyNumeric.Value = New Decimal(New Integer() {990000000, 0, 0, 0})
        '
        'startPowerNumeric
        '
        Me.startPowerNumeric.DecimalPlaces = 2
        Me.startPowerNumeric.Location = New System.Drawing.Point(21, 135)
        Me.startPowerNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.startPowerNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.startPowerNumeric.Name = "startPowerNumeric"
        Me.startPowerNumeric.Size = New System.Drawing.Size(120, 20)
        Me.startPowerNumeric.TabIndex = 3
        Me.startPowerNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        '
        'stopPowerNumeric
        '
        Me.stopPowerNumeric.DecimalPlaces = 2
        Me.stopPowerNumeric.Location = New System.Drawing.Point(21, 184)
        Me.stopPowerNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.stopPowerNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.stopPowerNumeric.Name = "stopPowerNumeric"
        Me.stopPowerNumeric.Size = New System.Drawing.Size(120, 20)
        Me.stopPowerNumeric.TabIndex = 4
        '
        'endFrequencyNumeric
        '
        Me.endFrequencyNumeric.DecimalPlaces = 6
        Me.endFrequencyNumeric.Location = New System.Drawing.Point(21, 86)
        Me.endFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.endFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.endFrequencyNumeric.Name = "endFrequencyNumeric"
        Me.endFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.endFrequencyNumeric.TabIndex = 2
        Me.endFrequencyNumeric.Value = New Decimal(New Integer() {1010000000, 0, 0, 0})
        '
        'numberStepsNumeric
        '
        Me.numberStepsNumeric.Location = New System.Drawing.Point(21, 233)
        Me.numberStepsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numberStepsNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numberStepsNumeric.Name = "numberStepsNumeric"
        Me.numberStepsNumeric.Size = New System.Drawing.Size(120, 20)
        Me.numberStepsNumeric.TabIndex = 5
        Me.numberStepsNumeric.Value = New Decimal(New Integer() {21, 0, 0, 0})
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(12, 9)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'listTriggerSourceLabel
        '
        Me.listTriggerSourceLabel.AutoSize = True
        Me.listTriggerSourceLabel.Location = New System.Drawing.Point(10, 20)
        Me.listTriggerSourceLabel.Name = "listTriggerSourceLabel"
        Me.listTriggerSourceLabel.Size = New System.Drawing.Size(96, 13)
        Me.listTriggerSourceLabel.TabIndex = 6
        Me.listTriggerSourceLabel.Text = "List Trigger Source"
        '
        'dwellTimeLabel
        '
        Me.dwellTimeLabel.AutoSize = True
        Me.dwellTimeLabel.Location = New System.Drawing.Point(10, 68)
        Me.dwellTimeLabel.Name = "dwellTimeLabel"
        Me.dwellTimeLabel.Size = New System.Drawing.Size(121, 13)
        Me.dwellTimeLabel.TabIndex = 10
        Me.dwellTimeLabel.Text = "Dwell Time in Second(s)"
        '
        'frequencySettlingsLabel
        '
        Me.frequencySettlingsLabel.AutoSize = True
        Me.frequencySettlingsLabel.Location = New System.Drawing.Point(10, 118)
        Me.frequencySettlingsLabel.Name = "frequencySettlingsLabel"
        Me.frequencySettlingsLabel.Size = New System.Drawing.Size(106, 13)
        Me.frequencySettlingsLabel.TabIndex = 11
        Me.frequencySettlingsLabel.Text = "Frequency Settling(s)"
        '
        'errorLabel
        '
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(12, 302)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 12
        Me.errorLabel.Text = "Warning/Error Message"
        '
        'referenceClockSourceLabel
        '
        Me.referenceClockSourceLabel.AutoSize = True
        Me.referenceClockSourceLabel.Location = New System.Drawing.Point(12, 59)
        Me.referenceClockSourceLabel.Name = "referenceClockSourceLabel"
        Me.referenceClockSourceLabel.Size = New System.Drawing.Size(124, 13)
        Me.referenceClockSourceLabel.TabIndex = 13
        Me.referenceClockSourceLabel.Text = "Reference Clock Source"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 25)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'errorTextBox
        '
        Me.errorTextBox.Location = New System.Drawing.Point(15, 318)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.ReadOnly = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(332, 67)
        Me.errorTextBox.TabIndex = 16
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No Error"
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(183, 391)
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
        Me.stopButton.Location = New System.Drawing.Point(264, 391)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 5
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'rfsgStatusTimer
        '
        '
        'configurationListTriggerGroupBox
        '
        Me.configurationListTriggerGroupBox.Controls.Add(Me.frequencySettlingsNumeric)
        Me.configurationListTriggerGroupBox.Controls.Add(Me.dwellTimeNumeric)
        Me.configurationListTriggerGroupBox.Controls.Add(Me.listTriggerSourceComboBox)
        Me.configurationListTriggerGroupBox.Controls.Add(Me.frequencySettlingsLabel)
        Me.configurationListTriggerGroupBox.Controls.Add(Me.listTriggerSourceLabel)
        Me.configurationListTriggerGroupBox.Controls.Add(Me.dwellTimeLabel)
        Me.configurationListTriggerGroupBox.Location = New System.Drawing.Point(12, 123)
        Me.configurationListTriggerGroupBox.Name = "configurationListTriggerGroupBox"
        Me.configurationListTriggerGroupBox.Size = New System.Drawing.Size(152, 164)
        Me.configurationListTriggerGroupBox.TabIndex = 2
        Me.configurationListTriggerGroupBox.TabStop = False
        Me.configurationListTriggerGroupBox.Text = "Configuration List Trigger"
        '
        'frequencySettlingsNumeric
        '
        Me.frequencySettlingsNumeric.DecimalPlaces = 3
        Me.frequencySettlingsNumeric.Location = New System.Drawing.Point(10, 135)
        Me.frequencySettlingsNumeric.Name = "frequencySettlingsNumeric"
        Me.frequencySettlingsNumeric.Size = New System.Drawing.Size(120, 20)
        Me.frequencySettlingsNumeric.TabIndex = 14
        Me.frequencySettlingsNumeric.Value = New Decimal(New Integer() {7, 0, 0, 196608})
        '
        'dwellTimeNumeric
        '
        Me.dwellTimeNumeric.DecimalPlaces = 3
        Me.dwellTimeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.dwellTimeNumeric.Location = New System.Drawing.Point(10, 85)
        Me.dwellTimeNumeric.Name = "dwellTimeNumeric"
        Me.dwellTimeNumeric.Size = New System.Drawing.Size(120, 20)
        Me.dwellTimeNumeric.TabIndex = 13
        Me.dwellTimeNumeric.Value = New Decimal(New Integer() {20, 0, 0, 196608})
        '
        'listTriggerSourceComboBox
        '
        Me.listTriggerSourceComboBox.Location = New System.Drawing.Point(10, 37)
        Me.listTriggerSourceComboBox.Name = "listTriggerSourceComboBox"
        Me.listTriggerSourceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.listTriggerSourceComboBox.TabIndex = 12
        '
        'referenceClockSourceComboBox
        '
        Me.referenceClockSourceComboBox.Location = New System.Drawing.Point(12, 75)
        Me.referenceClockSourceComboBox.Name = "referenceClockSourceComboBox"
        Me.referenceClockSourceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.referenceClockSourceComboBox.TabIndex = 17
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(357, 426)
        Me.Controls.Add(Me.referenceClockSourceComboBox)
        Me.Controls.Add(Me.configurationListTriggerGroupBox)
        Me.Controls.Add(Me.configurationListParametersGroupBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.referenceClockSourceLabel)
        Me.Controls.Add(Me.stopButton)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Frequency And Power Sweep"
        Me.configurationListParametersGroupBox.ResumeLayout(False)
        Me.configurationListParametersGroupBox.PerformLayout()
        CType(Me.startFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.startPowerNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.stopPowerNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.endFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberStepsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.configurationListTriggerGroupBox.ResumeLayout(False)
        Me.configurationListTriggerGroupBox.PerformLayout()
        CType(Me.frequencySettlingsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dwellTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private configurationListParametersGroupBox As System.Windows.Forms.GroupBox
    Private resourceNameLabel As System.Windows.Forms.Label
    Private startFrequencyLabel As System.Windows.Forms.Label
    Private startPowerLabel As System.Windows.Forms.Label
    Private stopPowerLabel As System.Windows.Forms.Label
    Private endFrequencyLabel As System.Windows.Forms.Label
    Private numberStepsLabel As System.Windows.Forms.Label
    Private listTriggerSourceLabel As System.Windows.Forms.Label
    Private dwellTimeLabel As System.Windows.Forms.Label
    Private frequencySettlingsLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private referenceClockSourceLabel As System.Windows.Forms.Label
    Private startFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private startPowerNumeric As System.Windows.Forms.NumericUpDown
    Private stopPowerNumeric As System.Windows.Forms.NumericUpDown
    Private endFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private numberStepsNumeric As System.Windows.Forms.NumericUpDown
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private errorTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer
    Private configurationListTriggerGroupBox As System.Windows.Forms.GroupBox
    Private referenceClockSourceComboBox As System.Windows.Forms.ComboBox
    Private dwellTimeNumeric As System.Windows.Forms.NumericUpDown
    Private listTriggerSourceComboBox As System.Windows.Forms.ComboBox
    Private frequencySettlingsNumeric As System.Windows.Forms.NumericUpDown

End Class
