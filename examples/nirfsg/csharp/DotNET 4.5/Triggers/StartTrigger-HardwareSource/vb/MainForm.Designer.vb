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
    ''' the contents of Me method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.startTriggerParametersGroupBox = New System.Windows.Forms.GroupBox()
        Me.triggerSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerSourceLabel = New System.Windows.Forms.Label()
        Me.triggerTypeLabel = New System.Windows.Forms.Label()
        Me.startButton = New System.Windows.Forms.Button()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.frequencyLabel = New System.Windows.Forms.Label()
        Me.powerLevelLabel = New System.Windows.Forms.Label()
        Me.errorLabel = New System.Windows.Forms.Label()
        Me.frequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.powerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.errorTextBox = New System.Windows.Forms.TextBox()
        Me.stopButton = New System.Windows.Forms.Button()        Me.rfsgStatusTimer = New System.Windows.Forms.Timer(Me.components)
        Me.startTriggerParametersGroupBox.SuspendLayout()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        ' 
        ' startTriggerParametersGroupBox
        ' 
        Me.startTriggerParametersGroupBox.Controls.Add(Me.triggerSourceComboBox)
        Me.startTriggerParametersGroupBox.Controls.Add(Me.triggerTypeComboBox)
        Me.startTriggerParametersGroupBox.Controls.Add(Me.triggerSourceLabel)
        Me.startTriggerParametersGroupBox.Controls.Add(Me.triggerTypeLabel)
        Me.startTriggerParametersGroupBox.Location = New System.Drawing.Point(173, 73)
        Me.startTriggerParametersGroupBox.Name = "startTriggerParametersGroupBox"
        Me.startTriggerParametersGroupBox.Size = New System.Drawing.Size(159, 121)
        Me.startTriggerParametersGroupBox.TabIndex = 11
        Me.startTriggerParametersGroupBox.TabStop = False
        Me.startTriggerParametersGroupBox.Text = "Start Trigger Parameters"
        ' 
        ' triggerSourceComboBox
        ' 
        Me.triggerSourceComboBox.Location = New System.Drawing.Point(17, 89)
        Me.triggerSourceComboBox.Name = "triggerSourceComboBox"
        Me.triggerSourceComboBox.Size = New System.Drawing.Size(120, 21)
        Me.triggerSourceComboBox.TabIndex = 4
        ' 
        ' triggerTypeComboBox
        ' 
        Me.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerTypeComboBox.Location = New System.Drawing.Point(17, 38)
        Me.triggerTypeComboBox.Name = "triggerTypeComboBox"
        Me.triggerTypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.triggerTypeComboBox.TabIndex = 3
        ' 
        ' triggerSourceLabel
        ' 
        Me.triggerSourceLabel.AutoSize = True
        Me.triggerSourceLabel.Location = New System.Drawing.Point(14, 69)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(102, 13)
        Me.triggerSourceLabel.TabIndex = 4
        Me.triggerSourceLabel.Text = "Start Trigger Source"
        ' 
        ' triggerTypeLabel
        ' 
        Me.triggerTypeLabel.AutoSize = True
        Me.triggerTypeLabel.Location = New System.Drawing.Point(14, 17)
        Me.triggerTypeLabel.Name = "triggerTypeLabel"
        Me.triggerTypeLabel.Size = New System.Drawing.Size(92, 13)
        Me.triggerTypeLabel.TabIndex = 3
        Me.triggerTypeLabel.Text = "Start Trigger Type"
        ' 
        ' startButton
        ' 
        Me.startButton.Location = New System.Drawing.Point(354, 35)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 5
        Me.startButton.Text = "St&art"
        Me.startButton.UseVisualStyleBackColor = True

        ' 
        ' resourceNameLabel
        ' 
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(20, 16)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        ' 
        ' frequencyLabel
        ' 
        Me.frequencyLabel.AutoSize = True
        Me.frequencyLabel.Location = New System.Drawing.Point(21, 90)
        Me.frequencyLabel.Name = "frequencyLabel"
        Me.frequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.frequencyLabel.TabIndex = 1
        Me.frequencyLabel.Text = "Center Frequency [Hz]"
        ' 
        ' powerLevelLabel
        ' 
        Me.powerLevelLabel.AutoSize = True
        Me.powerLevelLabel.Location = New System.Drawing.Point(21, 142)
        Me.powerLevelLabel.Name = "powerLevelLabel"
        Me.powerLevelLabel.Size = New System.Drawing.Size(96, 13)
        Me.powerLevelLabel.TabIndex = 2
        Me.powerLevelLabel.Text = "Power Level [dBm]"
        ' 
        ' errorLabel
        ' 
        Me.errorLabel.AutoSize = True
        Me.errorLabel.Location = New System.Drawing.Point(20, 214)
        Me.errorLabel.Name = "errorLabel"
        Me.errorLabel.Size = New System.Drawing.Size(120, 13)
        Me.errorLabel.TabIndex = 5
        Me.errorLabel.Text = "Warning/Error Message"
        ' 
        ' frequencyNumeric
        ' 
        Me.frequencyNumeric.DecimalPlaces = 6
        Me.frequencyNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.frequencyNumeric.Location = New System.Drawing.Point(21, 111)
        Me.frequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.frequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.frequencyNumeric.Name = "frequencyNumeric"
        Me.frequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.frequencyNumeric.TabIndex = 1
        Me.frequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        ' 
        ' powerLevelNumeric
        ' 
        Me.powerLevelNumeric.DecimalPlaces = 2
        Me.powerLevelNumeric.Location = New System.Drawing.Point(21, 163)
        Me.powerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.powerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.powerLevelNumeric.Name = "powerLevelNumeric"
        Me.powerLevelNumeric.Size = New System.Drawing.Size(120, 20)
        Me.powerLevelNumeric.TabIndex = 2
        Me.powerLevelNumeric.Value = New Decimal(New Integer() {20, 0, 0, -2147483648})
        ' 
        ' resourceNameComboBox
        ' 
        Me.resourceNameComboBox.Location = New System.Drawing.Point(20, 37)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        ' 
        ' errorTextBox
        ' 
        Me.errorTextBox.Location = New System.Drawing.Point(22, 235)
        Me.errorTextBox.Multiline = True
        Me.errorTextBox.Name = "errorTextBox"
        Me.errorTextBox.[ReadOnly] = True
        Me.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.errorTextBox.Size = New System.Drawing.Size(488, 34)
        Me.errorTextBox.TabIndex = 10
        Me.errorTextBox.TabStop = False
        Me.errorTextBox.Text = "No error."
        ' 
        ' stopButton
        ' 
        Me.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.stopButton.Enabled = False
        Me.stopButton.Location = New System.Drawing.Point(435, 35)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 6
        Me.stopButton.Text = "St&op"
        Me.stopButton.UseVisualStyleBackColor = True

        '         ' 
        ' 
        ' rfsgStatusTimer
        ' 

        ' 
        ' MainForm
        ' 
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True        Me.ClientSize = New System.Drawing.Size(522, 317)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.startTriggerParametersGroupBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.frequencyLabel)
        Me.Controls.Add(Me.powerLevelLabel)
        Me.Controls.Add(Me.errorLabel)
        Me.Controls.Add(Me.frequencyNumeric)
        Me.Controls.Add(Me.powerLevelNumeric)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.errorTextBox)
        Me.Controls.Add(Me.stopButton)        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Start Trigger - Hardware Source"
        Me.startTriggerParametersGroupBox.ResumeLayout(False)
        Me.startTriggerParametersGroupBox.PerformLayout()
        CType(Me.frequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.powerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private startTriggerParametersGroupBox As System.Windows.Forms.GroupBox
    Private resourceNameLabel As System.Windows.Forms.Label
    Private frequencyLabel As System.Windows.Forms.Label
    Private powerLevelLabel As System.Windows.Forms.Label
    Private triggerTypeLabel As System.Windows.Forms.Label
    Private triggerSourceLabel As System.Windows.Forms.Label
    Private errorLabel As System.Windows.Forms.Label
    Private frequencyNumeric As System.Windows.Forms.NumericUpDown
    Private powerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private errorTextBox As System.Windows.Forms.TextBox
    Private WithEvents startButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button    Private triggerTypeComboBox As System.Windows.Forms.ComboBox
    Private triggerSourceComboBox As System.Windows.Forms.ComboBox
    Private WithEvents rfsgStatusTimer As System.Windows.Forms.Timer

End Class
