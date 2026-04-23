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
        Me.firstChannelVoltageLevelLabel = New System.Windows.Forms.Label()
        Me.firstChannelCurrentLimitLabel = New System.Windows.Forms.Label()
        Me.firstChannelVoltagLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.firstChannelCurrentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.secondChannelVoltageLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.secondChannelCurrentLimitNumeric = New System.Windows.Forms.NumericUpDown()
        Me.startButton = New System.Windows.Forms.Button()
        Me.resourceNameGroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.firstChannelNameLabel = New System.Windows.Forms.Label()
        Me.firstChannelGroupBox = New System.Windows.Forms.GroupBox()
        Me.firstChannelNameTextBox = New System.Windows.Forms.TextBox()
        Me.firstChannelMeasurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.firstChaannelVoltageMeasurementTextBox = New System.Windows.Forms.TextBox()
        Me.firstChannelCurrentMeasurementTextBox = New System.Windows.Forms.TextBox()
        Me.firstChannelCurrentMeasurementLabel = New System.Windows.Forms.Label()
        Me.firstChannelVoltageMeasurementLabel = New System.Windows.Forms.Label()
        Me.secondChannelGroupBox = New System.Windows.Forms.GroupBox()
        Me.secondChannelNameTextBox = New System.Windows.Forms.TextBox()
        Me.secondChannelNameLabel = New System.Windows.Forms.Label()
        Me.secondChannelVoltageLevelLabel = New System.Windows.Forms.Label()
        Me.secondChannelCurrentLimitLabel = New System.Windows.Forms.Label()
        Me.secondChannelMeasurementsGroupBox = New System.Windows.Forms.GroupBox()
        Me.secondChaannelVoltageMeasurementTextBox = New System.Windows.Forms.TextBox()
        Me.secondChannelCurrentMeasurementTextBox = New System.Windows.Forms.TextBox()
        Me.secondChannelCurrentMeasurementLabel = New System.Windows.Forms.Label()
        Me.secondChannelVoltageMeasurementLabel = New System.Windows.Forms.Label()
        CType(Me.firstChannelVoltagLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.firstChannelCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.secondChannelVoltageLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.secondChannelCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.resourceNameGroupBox.SuspendLayout()
        Me.firstChannelGroupBox.SuspendLayout()
        Me.firstChannelMeasurementsGroupBox.SuspendLayout()
        Me.secondChannelGroupBox.SuspendLayout()
        Me.secondChannelMeasurementsGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'firstChannelVoltageLevelLabel
        '
        Me.firstChannelVoltageLevelLabel.AutoSize = True
        Me.firstChannelVoltageLevelLabel.Location = New System.Drawing.Point(6, 49)
        Me.firstChannelVoltageLevelLabel.Name = "firstChannelVoltageLevelLabel"
        Me.firstChannelVoltageLevelLabel.Size = New System.Drawing.Size(88, 13)
        Me.firstChannelVoltageLevelLabel.TabIndex = 2
        Me.firstChannelVoltageLevelLabel.Text = "Voltage Level (V)"
        '
        'firstChannelCurrentLimitLabel
        '
        Me.firstChannelCurrentLimitLabel.AutoSize = True
        Me.firstChannelCurrentLimitLabel.Location = New System.Drawing.Point(6, 75)
        Me.firstChannelCurrentLimitLabel.Name = "firstChannelCurrentLimitLabel"
        Me.firstChannelCurrentLimitLabel.Size = New System.Drawing.Size(81, 13)
        Me.firstChannelCurrentLimitLabel.TabIndex = 4
        Me.firstChannelCurrentLimitLabel.Text = "Current Limit (A)"
        '
        'firstChannelVoltagLevelNumeric
        '
        Me.firstChannelVoltagLevelNumeric.DecimalPlaces = 6
        Me.firstChannelVoltagLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.firstChannelVoltagLevelNumeric.Location = New System.Drawing.Point(111, 45)
        Me.firstChannelVoltagLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.firstChannelVoltagLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.firstChannelVoltagLevelNumeric.Name = "firstChannelVoltagLevelNumeric"
        Me.firstChannelVoltagLevelNumeric.Size = New System.Drawing.Size(90, 20)
        Me.firstChannelVoltagLevelNumeric.TabIndex = 3
        Me.firstChannelVoltagLevelNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'firstChannelCurrentLimitNumeric
        '
        Me.firstChannelCurrentLimitNumeric.DecimalPlaces = 6
        Me.firstChannelCurrentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.firstChannelCurrentLimitNumeric.Location = New System.Drawing.Point(111, 71)
        Me.firstChannelCurrentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.firstChannelCurrentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.firstChannelCurrentLimitNumeric.Name = "firstChannelCurrentLimitNumeric"
        Me.firstChannelCurrentLimitNumeric.Size = New System.Drawing.Size(90, 20)
        Me.firstChannelCurrentLimitNumeric.TabIndex = 5
        Me.firstChannelCurrentLimitNumeric.Value = New Decimal(New Integer() {4, 0, 0, 131072})
        '
        'secondChannelVoltageLevelNumeric
        '
        Me.secondChannelVoltageLevelNumeric.DecimalPlaces = 6
        Me.secondChannelVoltageLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.secondChannelVoltageLevelNumeric.Location = New System.Drawing.Point(111, 45)
        Me.secondChannelVoltageLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.secondChannelVoltageLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.secondChannelVoltageLevelNumeric.Name = "secondChannelVoltageLevelNumeric"
        Me.secondChannelVoltageLevelNumeric.Size = New System.Drawing.Size(90, 20)
        Me.secondChannelVoltageLevelNumeric.TabIndex = 3
        Me.secondChannelVoltageLevelNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'secondChannelCurrentLimitNumeric
        '
        Me.secondChannelCurrentLimitNumeric.DecimalPlaces = 6
        Me.secondChannelCurrentLimitNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.secondChannelCurrentLimitNumeric.Location = New System.Drawing.Point(111, 71)
        Me.secondChannelCurrentLimitNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.secondChannelCurrentLimitNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.secondChannelCurrentLimitNumeric.Name = "secondChannelCurrentLimitNumeric"
        Me.secondChannelCurrentLimitNumeric.Size = New System.Drawing.Size(90, 20)
        Me.secondChannelCurrentLimitNumeric.TabIndex = 5
        Me.secondChannelCurrentLimitNumeric.Value = New Decimal(New Integer() {4, 0, 0, 131072})
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(301, 323)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 5
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'resourceNameGroupBox
        '
        Me.resourceNameGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.resourceNameGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.resourceNameGroupBox.Name = "resourceNameGroupBox"
        Me.resourceNameGroupBox.Size = New System.Drawing.Size(210, 49)
        Me.resourceNameGroupBox.TabIndex = 0
        Me.resourceNameGroupBox.TabStop = False
        Me.resourceNameGroupBox.Text = "Resource Name"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(6, 19)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(125, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'firstChannelNameLabel
        '
        Me.firstChannelNameLabel.AutoSize = True
        Me.firstChannelNameLabel.Location = New System.Drawing.Point(6, 22)
        Me.firstChannelNameLabel.Name = "firstChannelNameLabel"
        Me.firstChannelNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.firstChannelNameLabel.TabIndex = 0
        Me.firstChannelNameLabel.Text = "Channel Name"
        '
        'firstChannelGroupBox
        '
        Me.firstChannelGroupBox.Controls.Add(Me.firstChannelNameTextBox)
        Me.firstChannelGroupBox.Controls.Add(Me.firstChannelNameLabel)
        Me.firstChannelGroupBox.Controls.Add(Me.firstChannelVoltagLevelNumeric)
        Me.firstChannelGroupBox.Controls.Add(Me.firstChannelVoltageLevelLabel)
        Me.firstChannelGroupBox.Controls.Add(Me.firstChannelCurrentLimitNumeric)
        Me.firstChannelGroupBox.Controls.Add(Me.firstChannelCurrentLimitLabel)
        Me.firstChannelGroupBox.Location = New System.Drawing.Point(12, 77)
        Me.firstChannelGroupBox.Name = "firstChannelGroupBox"
        Me.firstChannelGroupBox.Size = New System.Drawing.Size(210, 127)
        Me.firstChannelGroupBox.TabIndex = 1
        Me.firstChannelGroupBox.TabStop = False
        Me.firstChannelGroupBox.Text = "First Channel"
        '
        'firstChannelNameTextBox
        '
        Me.firstChannelNameTextBox.Location = New System.Drawing.Point(111, 18)
        Me.firstChannelNameTextBox.Name = "firstChannelNameTextBox"
        Me.firstChannelNameTextBox.Size = New System.Drawing.Size(90, 20)
        Me.firstChannelNameTextBox.TabIndex = 1
        Me.firstChannelNameTextBox.Text = "0"
        '
        'firstChannelMeasurementsGroupBox
        '
        Me.firstChannelMeasurementsGroupBox.Controls.Add(Me.firstChaannelVoltageMeasurementTextBox)
        Me.firstChannelMeasurementsGroupBox.Controls.Add(Me.firstChannelCurrentMeasurementTextBox)
        Me.firstChannelMeasurementsGroupBox.Controls.Add(Me.firstChannelCurrentMeasurementLabel)
        Me.firstChannelMeasurementsGroupBox.Controls.Add(Me.firstChannelVoltageMeasurementLabel)
        Me.firstChannelMeasurementsGroupBox.Location = New System.Drawing.Point(238, 77)
        Me.firstChannelMeasurementsGroupBox.Name = "firstChannelMeasurementsGroupBox"
        Me.firstChannelMeasurementsGroupBox.Size = New System.Drawing.Size(200, 74)
        Me.firstChannelMeasurementsGroupBox.TabIndex = 3
        Me.firstChannelMeasurementsGroupBox.TabStop = False
        Me.firstChannelMeasurementsGroupBox.Text = "First Channel Measurements"
        '
        'firstChaannelVoltageMeasurementTextBox
        '
        Me.firstChaannelVoltageMeasurementTextBox.Location = New System.Drawing.Point(87, 19)
        Me.firstChaannelVoltageMeasurementTextBox.Name = "firstChaannelVoltageMeasurementTextBox"
        Me.firstChaannelVoltageMeasurementTextBox.ReadOnly = True
        Me.firstChaannelVoltageMeasurementTextBox.Size = New System.Drawing.Size(107, 20)
        Me.firstChaannelVoltageMeasurementTextBox.TabIndex = 1
        Me.firstChaannelVoltageMeasurementTextBox.Text = "0.000000E+000"
        '
        'firstChannelCurrentMeasurementTextBox
        '
        Me.firstChannelCurrentMeasurementTextBox.Location = New System.Drawing.Point(87, 46)
        Me.firstChannelCurrentMeasurementTextBox.Name = "firstChannelCurrentMeasurementTextBox"
        Me.firstChannelCurrentMeasurementTextBox.ReadOnly = True
        Me.firstChannelCurrentMeasurementTextBox.Size = New System.Drawing.Size(107, 20)
        Me.firstChannelCurrentMeasurementTextBox.TabIndex = 3
        Me.firstChannelCurrentMeasurementTextBox.Text = "0.000000E+000"
        '
        'firstChannelCurrentMeasurementLabel
        '
        Me.firstChannelCurrentMeasurementLabel.AutoSize = True
        Me.firstChannelCurrentMeasurementLabel.Location = New System.Drawing.Point(6, 49)
        Me.firstChannelCurrentMeasurementLabel.Name = "firstChannelCurrentMeasurementLabel"
        Me.firstChannelCurrentMeasurementLabel.Size = New System.Drawing.Size(57, 13)
        Me.firstChannelCurrentMeasurementLabel.TabIndex = 2
        Me.firstChannelCurrentMeasurementLabel.Text = "Current (A)"
        '
        'firstChannelVoltageMeasurementLabel
        '
        Me.firstChannelVoltageMeasurementLabel.AutoSize = True
        Me.firstChannelVoltageMeasurementLabel.Location = New System.Drawing.Point(6, 23)
        Me.firstChannelVoltageMeasurementLabel.Name = "firstChannelVoltageMeasurementLabel"
        Me.firstChannelVoltageMeasurementLabel.Size = New System.Drawing.Size(59, 13)
        Me.firstChannelVoltageMeasurementLabel.TabIndex = 0
        Me.firstChannelVoltageMeasurementLabel.Text = "Voltage (V)"
        '
        'secondChannelGroupBox
        '
        Me.secondChannelGroupBox.Controls.Add(Me.secondChannelNameTextBox)
        Me.secondChannelGroupBox.Controls.Add(Me.secondChannelNameLabel)
        Me.secondChannelGroupBox.Controls.Add(Me.secondChannelVoltageLevelLabel)
        Me.secondChannelGroupBox.Controls.Add(Me.secondChannelCurrentLimitLabel)
        Me.secondChannelGroupBox.Controls.Add(Me.secondChannelVoltageLevelNumeric)
        Me.secondChannelGroupBox.Controls.Add(Me.secondChannelCurrentLimitNumeric)
        Me.secondChannelGroupBox.Location = New System.Drawing.Point(12, 227)
        Me.secondChannelGroupBox.Name = "secondChannelGroupBox"
        Me.secondChannelGroupBox.Size = New System.Drawing.Size(210, 127)
        Me.secondChannelGroupBox.TabIndex = 2
        Me.secondChannelGroupBox.TabStop = False
        Me.secondChannelGroupBox.Text = "Second Channel"
        '
        'secondChannelNameTextBox
        '
        Me.secondChannelNameTextBox.Location = New System.Drawing.Point(111, 18)
        Me.secondChannelNameTextBox.Name = "secondChannelNameTextBox"
        Me.secondChannelNameTextBox.Size = New System.Drawing.Size(90, 20)
        Me.secondChannelNameTextBox.TabIndex = 1
        Me.secondChannelNameTextBox.Text = "1"
        '
        'secondChannelNameLabel
        '
        Me.secondChannelNameLabel.AutoSize = True
        Me.secondChannelNameLabel.Location = New System.Drawing.Point(6, 22)
        Me.secondChannelNameLabel.Name = "secondChannelNameLabel"
        Me.secondChannelNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.secondChannelNameLabel.TabIndex = 0
        Me.secondChannelNameLabel.Text = "Channel Name"
        '
        'secondChannelVoltageLevelLabel
        '
        Me.secondChannelVoltageLevelLabel.AutoSize = True
        Me.secondChannelVoltageLevelLabel.Location = New System.Drawing.Point(6, 49)
        Me.secondChannelVoltageLevelLabel.Name = "secondChannelVoltageLevelLabel"
        Me.secondChannelVoltageLevelLabel.Size = New System.Drawing.Size(88, 13)
        Me.secondChannelVoltageLevelLabel.TabIndex = 2
        Me.secondChannelVoltageLevelLabel.Text = "Voltage Level (V)"
        '
        'secondChannelCurrentLimitLabel
        '
        Me.secondChannelCurrentLimitLabel.AutoSize = True
        Me.secondChannelCurrentLimitLabel.Location = New System.Drawing.Point(6, 75)
        Me.secondChannelCurrentLimitLabel.Name = "secondChannelCurrentLimitLabel"
        Me.secondChannelCurrentLimitLabel.Size = New System.Drawing.Size(81, 13)
        Me.secondChannelCurrentLimitLabel.TabIndex = 4
        Me.secondChannelCurrentLimitLabel.Text = "Current Limit (A)"
        '
        'secondChannelMeasurementsGroupBox
        '
        Me.secondChannelMeasurementsGroupBox.Controls.Add(Me.secondChaannelVoltageMeasurementTextBox)
        Me.secondChannelMeasurementsGroupBox.Controls.Add(Me.secondChannelCurrentMeasurementTextBox)
        Me.secondChannelMeasurementsGroupBox.Controls.Add(Me.secondChannelCurrentMeasurementLabel)
        Me.secondChannelMeasurementsGroupBox.Controls.Add(Me.secondChannelVoltageMeasurementLabel)
        Me.secondChannelMeasurementsGroupBox.Location = New System.Drawing.Point(238, 227)
        Me.secondChannelMeasurementsGroupBox.Name = "secondChannelMeasurementsGroupBox"
        Me.secondChannelMeasurementsGroupBox.Size = New System.Drawing.Size(200, 73)
        Me.secondChannelMeasurementsGroupBox.TabIndex = 4
        Me.secondChannelMeasurementsGroupBox.TabStop = False
        Me.secondChannelMeasurementsGroupBox.Text = "Second Channel Measurements"
        '
        'secondChaannelVoltageMeasurementTextBox
        '
        Me.secondChaannelVoltageMeasurementTextBox.Location = New System.Drawing.Point(87, 19)
        Me.secondChaannelVoltageMeasurementTextBox.Name = "secondChaannelVoltageMeasurementTextBox"
        Me.secondChaannelVoltageMeasurementTextBox.ReadOnly = True
        Me.secondChaannelVoltageMeasurementTextBox.Size = New System.Drawing.Size(107, 20)
        Me.secondChaannelVoltageMeasurementTextBox.TabIndex = 1
        Me.secondChaannelVoltageMeasurementTextBox.Text = "0.000000E+000"
        '
        'secondChannelCurrentMeasurementTextBox
        '
        Me.secondChannelCurrentMeasurementTextBox.Location = New System.Drawing.Point(87, 45)
        Me.secondChannelCurrentMeasurementTextBox.Name = "secondChannelCurrentMeasurementTextBox"
        Me.secondChannelCurrentMeasurementTextBox.ReadOnly = True
        Me.secondChannelCurrentMeasurementTextBox.Size = New System.Drawing.Size(107, 20)
        Me.secondChannelCurrentMeasurementTextBox.TabIndex = 3
        Me.secondChannelCurrentMeasurementTextBox.Text = "0.000000E+000"
        '
        'secondChannelCurrentMeasurementLabel
        '
        Me.secondChannelCurrentMeasurementLabel.AutoSize = True
        Me.secondChannelCurrentMeasurementLabel.Location = New System.Drawing.Point(6, 49)
        Me.secondChannelCurrentMeasurementLabel.Name = "secondChannelCurrentMeasurementLabel"
        Me.secondChannelCurrentMeasurementLabel.Size = New System.Drawing.Size(57, 13)
        Me.secondChannelCurrentMeasurementLabel.TabIndex = 2
        Me.secondChannelCurrentMeasurementLabel.Text = "Current (A)"
        '
        'secondChannelVoltageMeasurementLabel
        '
        Me.secondChannelVoltageMeasurementLabel.AutoSize = True
        Me.secondChannelVoltageMeasurementLabel.Location = New System.Drawing.Point(6, 23)
        Me.secondChannelVoltageMeasurementLabel.Name = "secondChannelVoltageMeasurementLabel"
        Me.secondChannelVoltageMeasurementLabel.Size = New System.Drawing.Size(59, 13)
        Me.secondChannelVoltageMeasurementLabel.TabIndex = 0
        Me.secondChannelVoltageMeasurementLabel.Text = "Voltage (V)"
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(449, 365)
        Me.Controls.Add(Me.secondChannelMeasurementsGroupBox)
        Me.Controls.Add(Me.secondChannelGroupBox)
        Me.Controls.Add(Me.firstChannelMeasurementsGroupBox)
        Me.Controls.Add(Me.firstChannelGroupBox)
        Me.Controls.Add(Me.resourceNameGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Simultaneous Operation"
        CType(Me.firstChannelVoltagLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.firstChannelCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.secondChannelVoltageLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.secondChannelCurrentLimitNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.resourceNameGroupBox.ResumeLayout(False)
        Me.firstChannelGroupBox.ResumeLayout(False)
        Me.firstChannelGroupBox.PerformLayout()
        Me.firstChannelMeasurementsGroupBox.ResumeLayout(False)
        Me.firstChannelMeasurementsGroupBox.PerformLayout()
        Me.secondChannelGroupBox.ResumeLayout(False)
        Me.secondChannelGroupBox.PerformLayout()
        Me.secondChannelMeasurementsGroupBox.ResumeLayout(False)
        Me.secondChannelMeasurementsGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private firstChannelVoltageLevelLabel As System.Windows.Forms.Label
    Private firstChannelCurrentLimitLabel As System.Windows.Forms.Label
    Private firstChannelVoltagLevelNumeric As System.Windows.Forms.NumericUpDown
    Private firstChannelCurrentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private secondChannelVoltageLevelNumeric As System.Windows.Forms.NumericUpDown
    Private secondChannelCurrentLimitNumeric As System.Windows.Forms.NumericUpDown
    Private WithEvents startButton As System.Windows.Forms.Button
    Private resourceNameGroupBox As System.Windows.Forms.GroupBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private firstChannelNameLabel As System.Windows.Forms.Label
    Private firstChannelGroupBox As System.Windows.Forms.GroupBox
    Private firstChannelMeasurementsGroupBox As System.Windows.Forms.GroupBox
    Private firstChaannelVoltageMeasurementTextBox As System.Windows.Forms.TextBox
    Private firstChannelCurrentMeasurementTextBox As System.Windows.Forms.TextBox
    Private firstChannelCurrentMeasurementLabel As System.Windows.Forms.Label
    Private firstChannelVoltageMeasurementLabel As System.Windows.Forms.Label
    Private secondChannelGroupBox As System.Windows.Forms.GroupBox
    Private secondChannelNameLabel As System.Windows.Forms.Label
    Private secondChannelVoltageLevelLabel As System.Windows.Forms.Label
    Private secondChannelCurrentLimitLabel As System.Windows.Forms.Label
    Private secondChannelMeasurementsGroupBox As System.Windows.Forms.GroupBox
    Private secondChaannelVoltageMeasurementTextBox As System.Windows.Forms.TextBox
    Private secondChannelCurrentMeasurementTextBox As System.Windows.Forms.TextBox
    Private secondChannelCurrentMeasurementLabel As System.Windows.Forms.Label
    Private secondChannelVoltageMeasurementLabel As System.Windows.Forms.Label
    Private firstChannelNameTextBox As System.Windows.Forms.TextBox
    Private secondChannelNameTextBox As System.Windows.Forms.TextBox

End Class
