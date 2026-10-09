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
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.clockSourceLabel = New System.Windows.Forms.Label()
        Me.clockSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.selfCalibrationStepLabel = New System.Windows.Forms.Label()
        Me.selfCalibrationComboBox = New System.Windows.Forms.ComboBox()
        Me.selfCalibrationLabel = New System.Windows.Forms.Label()
        Me.startButton = New System.Windows.Forms.Button()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label4 = New System.Windows.Forms.Label()
        Me.indicatorLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.Location = New System.Drawing.Point(12, 44)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(110, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.resourceNameLabel.Location = New System.Drawing.Point(15, 21)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 22
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'clockSourceLabel
        '
        Me.clockSourceLabel.AutoSize = True
        Me.clockSourceLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.clockSourceLabel.Location = New System.Drawing.Point(12, 75)
        Me.clockSourceLabel.Name = "clockSourceLabel"
        Me.clockSourceLabel.Size = New System.Drawing.Size(71, 13)
        Me.clockSourceLabel.TabIndex = 24
        Me.clockSourceLabel.Text = "Clock Source"
        '
        'clockSourceComboBox
        '
        Me.clockSourceComboBox.Location = New System.Drawing.Point(12, 98)
        Me.clockSourceComboBox.Name = "clockSourceComboBox"
        Me.clockSourceComboBox.Size = New System.Drawing.Size(116, 21)
        Me.clockSourceComboBox.TabIndex = 1
        '
        'selfCalibrationStepLabel
        '
        Me.selfCalibrationStepLabel.AutoSize = True
        Me.selfCalibrationStepLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.selfCalibrationStepLabel.Location = New System.Drawing.Point(12, 129)
        Me.selfCalibrationStepLabel.Name = "selfCalibrationStepLabel"
        Me.selfCalibrationStepLabel.Size = New System.Drawing.Size(154, 13)
        Me.selfCalibrationStepLabel.TabIndex = 26
        Me.selfCalibrationStepLabel.Text = "Self Calibration Step Operation "
        '
        'selfCalibrationComboBox
        '
        Me.selfCalibrationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.selfCalibrationComboBox.Location = New System.Drawing.Point(12, 152)
        Me.selfCalibrationComboBox.Name = "selfCalibrationComboBox"
        Me.selfCalibrationComboBox.Size = New System.Drawing.Size(182, 21)
        Me.selfCalibrationComboBox.TabIndex = 2
        '
        'selfCalibrationLabel
        '
        Me.selfCalibrationLabel.AutoSize = True
        Me.selfCalibrationLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.selfCalibrationLabel.Location = New System.Drawing.Point(485, 31)
        Me.selfCalibrationLabel.Name = "selfCalibrationLabel"
        Me.selfCalibrationLabel.Size = New System.Drawing.Size(80, 13)
        Me.selfCalibrationLabel.TabIndex = 29
        Me.selfCalibrationLabel.Text = "Self Calibration "
        '
        'startButton
        '
        Me.startButton.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.startButton.Location = New System.Drawing.Point(12, 189)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(91, 30)
        Me.startButton.TabIndex = 3
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.label2.Location = New System.Drawing.Point(9, 233)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(666, 13)
        Me.label2.TabIndex = 31
        Me.label2.Text = "For the NI 5665, the PXI backplane and the LO must share a common Reference clock" & _
            ". You can share this clock signal in one of two ways."
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.label3.Location = New System.Drawing.Point(9, 246)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(496, 13)
        Me.label3.TabIndex = 32
        Me.label3.Text = "- Configure OnboardClock and route the 10 MHz Reference clock out of the LO into " & _
            "the PXI backplane."
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.label4.Location = New System.Drawing.Point(9, 259)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(446, 13)
        Me.label4.TabIndex = 33
        Me.label4.Text = "- Configure PXI_CLK and provide an external 10 MHz Reference clock to the PXI bac" & _
            "kplane."
        '
        'indicatorLabel
        '
        Me.indicatorLabel.BackColor = System.Drawing.Color.Gray
        Me.indicatorLabel.Location = New System.Drawing.Point(467, 58)
        Me.indicatorLabel.Name = "indicatorLabel"
        Me.indicatorLabel.Size = New System.Drawing.Size(114, 30)
        Me.indicatorLabel.TabIndex = 34
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(693, 285)
        Me.Controls.Add(Me.indicatorLabel)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.startButton)
        Me.Controls.Add(Me.selfCalibrationLabel)
        Me.Controls.Add(Me.selfCalibrationStepLabel)
        Me.Controls.Add(Me.selfCalibrationComboBox)
        Me.Controls.Add(Me.clockSourceLabel)
        Me.Controls.Add(Me.clockSourceComboBox)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "RFSA Self Calibration"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private resourceNameLabel As System.Windows.Forms.Label
    Private clockSourceLabel As System.Windows.Forms.Label
    Private clockSourceComboBox As System.Windows.Forms.ComboBox
    Private selfCalibrationStepLabel As System.Windows.Forms.Label
    Private selfCalibrationComboBox As System.Windows.Forms.ComboBox
    Private selfCalibrationLabel As System.Windows.Forms.Label
	Private WithEvents startButton As System.Windows.Forms.Button
	Private label2 As System.Windows.Forms.Label
	Private label3 As System.Windows.Forms.Label
	Private label4 As System.Windows.Forms.Label
    Private indicatorLabel As System.Windows.Forms.Label
End Class

