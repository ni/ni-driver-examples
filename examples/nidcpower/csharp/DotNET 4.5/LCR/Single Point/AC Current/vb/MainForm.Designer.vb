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
        Me.lcrMeasurementTimeLabel = New System.Windows.Forms.Label()
        Me.cableLengthLabel = New System.Windows.Forms.Label()
        Me.lcrCurrentLabel = New System.Windows.Forms.Label()
        Me.lcrImpedanceRangeLabel = New System.Windows.Forms.Label()
        Me.vdcLabel = New System.Windows.Forms.Label()
        Me.idcLabel = New System.Windows.Forms.Label()
        Me.dcInComplianceLabel = New System.Windows.Forms.Label()
        Me.lcrCurrentUpDown = New System.Windows.Forms.NumericUpDown()
        Me.lcrImpedanceRangeUpDown = New System.Windows.Forms.NumericUpDown()
        Me.startButton = New System.Windows.Forms.Button()
        Me.measurementResultGroupBox = New System.Windows.Forms.GroupBox()
        Me.unbalancedButtonLed = New System.Windows.Forms.Button()
        Me.unbalancedLabel = New System.Windows.Forms.Label()
        Me.acInComplianceButtonLed = New System.Windows.Forms.Button()
        Me.acInComplianceLabel = New System.Windows.Forms.Label()
        Me.dTextBox = New System.Windows.Forms.TextBox()
        Me.qTextBox = New System.Windows.Forms.TextBox()
        Me.qLabel = New System.Windows.Forms.Label()
        Me.dLabel = New System.Windows.Forms.Label()
        Me.rpTextBox = New System.Windows.Forms.TextBox()
        Me.rpLabel = New System.Windows.Forms.Label()
        Me.lpTextBox = New System.Windows.Forms.TextBox()
        Me.cpTextBox = New System.Windows.Forms.TextBox()
        Me.cpLabel = New System.Windows.Forms.Label()
        Me.lpLabel = New System.Windows.Forms.Label()
        Me.rsTextBox = New System.Windows.Forms.TextBox()
        Me.rsLabel = New System.Windows.Forms.Label()
        Me.lsTextBox = New System.Windows.Forms.TextBox()
        Me.csTextBox = New System.Windows.Forms.TextBox()
        Me.csLabel = New System.Windows.Forms.Label()
        Me.lsLabel = New System.Windows.Forms.Label()
        Me.yThetaTextBox = New System.Windows.Forms.TextBox()
        Me.yThetaLabel = New System.Windows.Forms.Label()
        Me.yMagTextBox = New System.Windows.Forms.TextBox()
        Me.yMagLabel = New System.Windows.Forms.Label()
        Me.zThetaTextBox = New System.Windows.Forms.TextBox()
        Me.zThetaLabel = New System.Windows.Forms.Label()
        Me.zMagTextBox = New System.Windows.Forms.TextBox()
        Me.zMagLabel = New System.Windows.Forms.Label()
        Me.yTextBox = New System.Windows.Forms.TextBox()
        Me.yLabel = New System.Windows.Forms.Label()
        Me.zTextBox = New System.Windows.Forms.TextBox()
        Me.zLabel = New System.Windows.Forms.Label()
        Me.acCurrentTextBox = New System.Windows.Forms.TextBox()
        Me.acCurrentLabel = New System.Windows.Forms.Label()
        Me.acVoltageTextBox = New System.Windows.Forms.TextBox()
        Me.acVoltageLabel = New System.Windows.Forms.Label()
        Me.measurementModeTextBox = New System.Windows.Forms.TextBox()
        Me.stimulusFrequencyTextBox = New System.Windows.Forms.TextBox()
        Me.measurementModeLabel = New System.Windows.Forms.Label()
        Me.stimulusFrequencyLabel = New System.Windows.Forms.Label()
        Me.vdcTextBox = New System.Windows.Forms.TextBox()
        Me.idcTextBox = New System.Windows.Forms.TextBox()
        Me.dcInComplianceButtonLed = New System.Windows.Forms.Button()
        Me.cableLengthComboBox = New System.Windows.Forms.ComboBox()
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.lcrImpedanceAutorangeComboBox = New System.Windows.Forms.ComboBox()
        Me.lcrImpedanceAutorangeLabel = New System.Windows.Forms.Label()
        Me.lcrSourceDelayModeComboBox = New System.Windows.Forms.ComboBox()
        Me.sourceDelayUpDown = New System.Windows.Forms.NumericUpDown()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.sourceDelayLabel = New System.Windows.Forms.Label()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.lcrSourceDelayModeLabel = New System.Windows.Forms.Label()
        Me.lcrMeasurementTimeComboBox = New System.Windows.Forms.ComboBox()
        Me.lcrDcBiasCurrentLabel = New System.Windows.Forms.Label()
        Me.lcrDcBiasSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.lcrCustomMeasurementTimeUpDown = New System.Windows.Forms.NumericUpDown()
        Me.lcrDcBiasCurrentUpDown = New System.Windows.Forms.NumericUpDown()
        Me.lcrDcBiasLabel = New System.Windows.Forms.Label()
        Me.lcrDcBiasVoltageLabel = New System.Windows.Forms.Label()
        Me.lcrFrequencyLabel = New System.Windows.Forms.Label()
        Me.lcrFrequencyUpDown = New System.Windows.Forms.NumericUpDown()
        Me.lcrDcBiasVoltageUpDown = New System.Windows.Forms.NumericUpDown()
        Me.lcrCustomMeasurementTimeLabel = New System.Windows.Forms.Label()
        CType(Me.lcrCurrentUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lcrImpedanceRangeUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementResultGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.sourceDelayUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lcrCustomMeasurementTimeUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lcrDcBiasCurrentUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lcrFrequencyUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lcrDcBiasVoltageUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lcrMeasurementTimeLabel
        '
        Me.lcrMeasurementTimeLabel.AutoSize = True
        Me.lcrMeasurementTimeLabel.Location = New System.Drawing.Point(5, 292)
        Me.lcrMeasurementTimeLabel.Name = "lcrMeasurementTimeLabel"
        Me.lcrMeasurementTimeLabel.Size = New System.Drawing.Size(121, 13)
        Me.lcrMeasurementTimeLabel.TabIndex = 18
        Me.lcrMeasurementTimeLabel.Text = "LCR Measurement Time"
        '
        'cableLengthLabel
        '
        Me.cableLengthLabel.AutoSize = True
        Me.cableLengthLabel.Location = New System.Drawing.Point(5, 131)
        Me.cableLengthLabel.Name = "cableLengthLabel"
        Me.cableLengthLabel.Size = New System.Drawing.Size(70, 13)
        Me.cableLengthLabel.TabIndex = 6
        Me.cableLengthLabel.Text = "Cable Length"
        '
        'lcrCurrentLabel
        '
        Me.lcrCurrentLabel.AutoSize = True
        Me.lcrCurrentLabel.Location = New System.Drawing.Point(5, 185)
        Me.lcrCurrentLabel.Name = "lcrCurrentLabel"
        Me.lcrCurrentLabel.Size = New System.Drawing.Size(92, 13)
        Me.lcrCurrentLabel.TabIndex = 10
        Me.lcrCurrentLabel.Text = "LCR Current RMS"
        '
        'lcrImpedanceRangeLabel
        '
        Me.lcrImpedanceRangeLabel.AutoSize = True
        Me.lcrImpedanceRangeLabel.Location = New System.Drawing.Point(159, 130)
        Me.lcrImpedanceRangeLabel.Name = "lcrImpedanceRangeLabel"
        Me.lcrImpedanceRangeLabel.Size = New System.Drawing.Size(119, 13)
        Me.lcrImpedanceRangeLabel.TabIndex = 8
        Me.lcrImpedanceRangeLabel.Text = "LCR Impedance Range"
        '
        'vdcLabel
        '
        Me.vdcLabel.AutoSize = True
        Me.vdcLabel.Location = New System.Drawing.Point(9, 23)
        Me.vdcLabel.Name = "vdcLabel"
        Me.vdcLabel.Size = New System.Drawing.Size(32, 13)
        Me.vdcLabel.TabIndex = 0
        Me.vdcLabel.Text = "V DC"
        '
        'idcLabel
        '
        Me.idcLabel.AutoSize = True
        Me.idcLabel.Location = New System.Drawing.Point(106, 23)
        Me.idcLabel.Name = "idcLabel"
        Me.idcLabel.Size = New System.Drawing.Size(28, 13)
        Me.idcLabel.TabIndex = 2
        Me.idcLabel.Text = "I DC"
        '
        'dcInComplianceLabel
        '
        Me.dcInComplianceLabel.AutoSize = True
        Me.dcInComplianceLabel.Location = New System.Drawing.Point(9, 359)
        Me.dcInComplianceLabel.Name = "dcInComplianceLabel"
        Me.dcInComplianceLabel.Size = New System.Drawing.Size(90, 13)
        Me.dcInComplianceLabel.TabIndex = 40
        Me.dcInComplianceLabel.Text = "DC in compliance"
        '
        'lcrCurrentUpDown
        '
        Me.lcrCurrentUpDown.DecimalPlaces = 6
        Me.lcrCurrentUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 327680})
        Me.lcrCurrentUpDown.Location = New System.Drawing.Point(5, 202)
        Me.lcrCurrentUpDown.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.lcrCurrentUpDown.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.lcrCurrentUpDown.Name = "lcrCurrentUpDown"
        Me.lcrCurrentUpDown.Size = New System.Drawing.Size(125, 20)
        Me.lcrCurrentUpDown.TabIndex = 11
        Me.lcrCurrentUpDown.Value = New Decimal(New Integer() {7, 0, 0, 327680})
        '
        'lcrImpedanceRangeUpDown
        '
        Me.lcrImpedanceRangeUpDown.DecimalPlaces = 6
        Me.lcrImpedanceRangeUpDown.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.lcrImpedanceRangeUpDown.Location = New System.Drawing.Point(159, 147)
        Me.lcrImpedanceRangeUpDown.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.lcrImpedanceRangeUpDown.Name = "lcrImpedanceRangeUpDown"
        Me.lcrImpedanceRangeUpDown.Size = New System.Drawing.Size(125, 20)
        Me.lcrImpedanceRangeUpDown.TabIndex = 9
        Me.lcrImpedanceRangeUpDown.Value = New Decimal(New Integer() {100000, 0, 0, 196608})
        '
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(355, 435)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 2
        Me.startButton.Text = "&Start"
        Me.startButton.UseVisualStyleBackColor = True
        '
        'measurementResultGroupBox
        '
        Me.measurementResultGroupBox.Controls.Add(Me.unbalancedButtonLed)
        Me.measurementResultGroupBox.Controls.Add(Me.unbalancedLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.acInComplianceButtonLed)
        Me.measurementResultGroupBox.Controls.Add(Me.acInComplianceLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.dTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.qTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.qLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.dLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.rpTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.rpLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.lpTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.cpTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.cpLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.lpLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.rsTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.rsLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.lsTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.csTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.csLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.lsLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.yThetaTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.yThetaLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.yMagTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.yMagLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.zThetaTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.zThetaLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.zMagTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.zMagLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.yTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.yLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.zTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.zLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.acCurrentTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.acCurrentLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.acVoltageTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.acVoltageLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.measurementModeTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.stimulusFrequencyTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.measurementModeLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.stimulusFrequencyLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.vdcTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.idcTextBox)
        Me.measurementResultGroupBox.Controls.Add(Me.idcLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.dcInComplianceButtonLed)
        Me.measurementResultGroupBox.Controls.Add(Me.vdcLabel)
        Me.measurementResultGroupBox.Controls.Add(Me.dcInComplianceLabel)
        Me.measurementResultGroupBox.Location = New System.Drawing.Point(355, 12)
        Me.measurementResultGroupBox.Name = "measurementResultGroupBox"
        Me.measurementResultGroupBox.Size = New System.Drawing.Size(396, 417)
        Me.measurementResultGroupBox.TabIndex = 1
        Me.measurementResultGroupBox.TabStop = False
        Me.measurementResultGroupBox.Text = "Measurement Result"
        '
        'unbalancedButtonLed
        '
        Me.unbalancedButtonLed.Enabled = False
        Me.unbalancedButtonLed.Location = New System.Drawing.Point(202, 376)
        Me.unbalancedButtonLed.Name = "unbalancedButtonLed"
        Me.unbalancedButtonLed.Size = New System.Drawing.Size(21, 22)
        Me.unbalancedButtonLed.TabIndex = 45
        Me.unbalancedButtonLed.UseVisualStyleBackColor = True
        '
        'unbalancedLabel
        '
        Me.unbalancedLabel.AutoSize = True
        Me.unbalancedLabel.Location = New System.Drawing.Point(202, 359)
        Me.unbalancedLabel.Name = "unbalancedLabel"
        Me.unbalancedLabel.Size = New System.Drawing.Size(63, 13)
        Me.unbalancedLabel.TabIndex = 44
        Me.unbalancedLabel.Text = "unbalanced"
        '
        'acInComplianceButtonLed
        '
        Me.acInComplianceButtonLed.Enabled = False
        Me.acInComplianceButtonLed.Location = New System.Drawing.Point(106, 376)
        Me.acInComplianceButtonLed.Name = "acInComplianceButtonLed"
        Me.acInComplianceButtonLed.Size = New System.Drawing.Size(21, 22)
        Me.acInComplianceButtonLed.TabIndex = 43
        Me.acInComplianceButtonLed.UseVisualStyleBackColor = True
        '
        'acInComplianceLabel
        '
        Me.acInComplianceLabel.AutoSize = True
        Me.acInComplianceLabel.Location = New System.Drawing.Point(106, 359)
        Me.acInComplianceLabel.Name = "acInComplianceLabel"
        Me.acInComplianceLabel.Size = New System.Drawing.Size(89, 13)
        Me.acInComplianceLabel.TabIndex = 42
        Me.acInComplianceLabel.Text = "AC in compliance"
        '
        'dTextBox
        '
        Me.dTextBox.Location = New System.Drawing.Point(9, 292)
        Me.dTextBox.Name = "dTextBox"
        Me.dTextBox.ReadOnly = True
        Me.dTextBox.Size = New System.Drawing.Size(90, 20)
        Me.dTextBox.TabIndex = 35
        Me.dTextBox.Text = "0.0000"
        '
        'qTextBox
        '
        Me.qTextBox.Location = New System.Drawing.Point(106, 292)
        Me.qTextBox.Name = "qTextBox"
        Me.qTextBox.ReadOnly = True
        Me.qTextBox.Size = New System.Drawing.Size(90, 20)
        Me.qTextBox.TabIndex = 37
        Me.qTextBox.Text = "0.0000"
        '
        'qLabel
        '
        Me.qLabel.AutoSize = True
        Me.qLabel.Location = New System.Drawing.Point(106, 275)
        Me.qLabel.Name = "qLabel"
        Me.qLabel.Size = New System.Drawing.Size(15, 13)
        Me.qLabel.TabIndex = 36
        Me.qLabel.Text = "Q"
        '
        'dLabel
        '
        Me.dLabel.AutoSize = True
        Me.dLabel.Location = New System.Drawing.Point(9, 275)
        Me.dLabel.Name = "dLabel"
        Me.dLabel.Size = New System.Drawing.Size(15, 13)
        Me.dLabel.TabIndex = 34
        Me.dLabel.Text = "D"
        '
        'rpTextBox
        '
        Me.rpTextBox.Location = New System.Drawing.Point(203, 250)
        Me.rpTextBox.Name = "rpTextBox"
        Me.rpTextBox.ReadOnly = True
        Me.rpTextBox.Size = New System.Drawing.Size(90, 20)
        Me.rpTextBox.TabIndex = 33
        Me.rpTextBox.Text = "0.0000"
        '
        'rpLabel
        '
        Me.rpLabel.AutoSize = True
        Me.rpLabel.Location = New System.Drawing.Point(203, 233)
        Me.rpLabel.Name = "rpLabel"
        Me.rpLabel.Size = New System.Drawing.Size(21, 13)
        Me.rpLabel.TabIndex = 32
        Me.rpLabel.Text = "Rp"
        '
        'lpTextBox
        '
        Me.lpTextBox.Location = New System.Drawing.Point(9, 250)
        Me.lpTextBox.Name = "lpTextBox"
        Me.lpTextBox.ReadOnly = True
        Me.lpTextBox.Size = New System.Drawing.Size(90, 20)
        Me.lpTextBox.TabIndex = 29
        Me.lpTextBox.Text = "0.0000"
        '
        'cpTextBox
        '
        Me.cpTextBox.Location = New System.Drawing.Point(106, 250)
        Me.cpTextBox.Name = "cpTextBox"
        Me.cpTextBox.ReadOnly = True
        Me.cpTextBox.Size = New System.Drawing.Size(90, 20)
        Me.cpTextBox.TabIndex = 31
        Me.cpTextBox.Text = "0.0000"
        '
        'cpLabel
        '
        Me.cpLabel.AutoSize = True
        Me.cpLabel.Location = New System.Drawing.Point(106, 233)
        Me.cpLabel.Name = "cpLabel"
        Me.cpLabel.Size = New System.Drawing.Size(20, 13)
        Me.cpLabel.TabIndex = 30
        Me.cpLabel.Text = "Cp"
        '
        'lpLabel
        '
        Me.lpLabel.AutoSize = True
        Me.lpLabel.Location = New System.Drawing.Point(9, 233)
        Me.lpLabel.Name = "lpLabel"
        Me.lpLabel.Size = New System.Drawing.Size(19, 13)
        Me.lpLabel.TabIndex = 28
        Me.lpLabel.Text = "Lp"
        '
        'rsTextBox
        '
        Me.rsTextBox.Location = New System.Drawing.Point(203, 208)
        Me.rsTextBox.Name = "rsTextBox"
        Me.rsTextBox.ReadOnly = True
        Me.rsTextBox.Size = New System.Drawing.Size(90, 20)
        Me.rsTextBox.TabIndex = 27
        Me.rsTextBox.Text = "0.0000"
        '
        'rsLabel
        '
        Me.rsLabel.AutoSize = True
        Me.rsLabel.Location = New System.Drawing.Point(203, 191)
        Me.rsLabel.Name = "rsLabel"
        Me.rsLabel.Size = New System.Drawing.Size(20, 13)
        Me.rsLabel.TabIndex = 26
        Me.rsLabel.Text = "Rs"
        '
        'lsTextBox
        '
        Me.lsTextBox.Location = New System.Drawing.Point(9, 208)
        Me.lsTextBox.Name = "lsTextBox"
        Me.lsTextBox.ReadOnly = True
        Me.lsTextBox.Size = New System.Drawing.Size(90, 20)
        Me.lsTextBox.TabIndex = 23
        Me.lsTextBox.Text = "0.0000"
        '
        'csTextBox
        '
        Me.csTextBox.Location = New System.Drawing.Point(106, 208)
        Me.csTextBox.Name = "csTextBox"
        Me.csTextBox.ReadOnly = True
        Me.csTextBox.Size = New System.Drawing.Size(90, 20)
        Me.csTextBox.TabIndex = 25
        Me.csTextBox.Text = "0.0000"
        '
        'csLabel
        '
        Me.csLabel.AutoSize = True
        Me.csLabel.Location = New System.Drawing.Point(106, 191)
        Me.csLabel.Name = "csLabel"
        Me.csLabel.Size = New System.Drawing.Size(19, 13)
        Me.csLabel.TabIndex = 24
        Me.csLabel.Text = "Cs"
        '
        'lsLabel
        '
        Me.lsLabel.AutoSize = True
        Me.lsLabel.Location = New System.Drawing.Point(9, 191)
        Me.lsLabel.Name = "lsLabel"
        Me.lsLabel.Size = New System.Drawing.Size(18, 13)
        Me.lsLabel.TabIndex = 22
        Me.lsLabel.Text = "Ls"
        '
        'yThetaTextBox
        '
        Me.yThetaTextBox.Location = New System.Drawing.Point(301, 166)
        Me.yThetaTextBox.Name = "yThetaTextBox"
        Me.yThetaTextBox.ReadOnly = True
        Me.yThetaTextBox.Size = New System.Drawing.Size(90, 20)
        Me.yThetaTextBox.TabIndex = 21
        Me.yThetaTextBox.Text = "0.000000E+000"
        '
        'yThetaLabel
        '
        Me.yThetaLabel.AutoSize = True
        Me.yThetaLabel.Location = New System.Drawing.Point(301, 149)
        Me.yThetaLabel.Name = "yThetaLabel"
        Me.yThetaLabel.Size = New System.Drawing.Size(41, 13)
        Me.yThetaLabel.TabIndex = 20
        Me.yThetaLabel.Text = "Y theta"
        '
        'yMagTextBox
        '
        Me.yMagTextBox.Location = New System.Drawing.Point(203, 166)
        Me.yMagTextBox.Name = "yMagTextBox"
        Me.yMagTextBox.ReadOnly = True
        Me.yMagTextBox.Size = New System.Drawing.Size(90, 20)
        Me.yMagTextBox.TabIndex = 19
        Me.yMagTextBox.Text = "0.000000E+000"
        '
        'yMagLabel
        '
        Me.yMagLabel.AutoSize = True
        Me.yMagLabel.Location = New System.Drawing.Point(203, 149)
        Me.yMagLabel.Name = "yMagLabel"
        Me.yMagLabel.Size = New System.Drawing.Size(66, 13)
        Me.yMagLabel.TabIndex = 18
        Me.yMagLabel.Text = "Y magnitude"
        '
        'zThetaTextBox
        '
        Me.zThetaTextBox.Location = New System.Drawing.Point(301, 124)
        Me.zThetaTextBox.Name = "zThetaTextBox"
        Me.zThetaTextBox.ReadOnly = True
        Me.zThetaTextBox.Size = New System.Drawing.Size(90, 20)
        Me.zThetaTextBox.TabIndex = 15
        Me.zThetaTextBox.Text = "0.000000E+000"
        '
        'zThetaLabel
        '
        Me.zThetaLabel.AutoSize = True
        Me.zThetaLabel.Location = New System.Drawing.Point(301, 107)
        Me.zThetaLabel.Name = "zThetaLabel"
        Me.zThetaLabel.Size = New System.Drawing.Size(41, 13)
        Me.zThetaLabel.TabIndex = 14
        Me.zThetaLabel.Text = "Z theta"
        '
        'zMagTextBox
        '
        Me.zMagTextBox.Location = New System.Drawing.Point(203, 124)
        Me.zMagTextBox.Name = "zMagTextBox"
        Me.zMagTextBox.ReadOnly = True
        Me.zMagTextBox.Size = New System.Drawing.Size(90, 20)
        Me.zMagTextBox.TabIndex = 13
        Me.zMagTextBox.Text = "0.000000E+000"
        '
        'zMagLabel
        '
        Me.zMagLabel.AutoSize = True
        Me.zMagLabel.Location = New System.Drawing.Point(203, 107)
        Me.zMagLabel.Name = "zMagLabel"
        Me.zMagLabel.Size = New System.Drawing.Size(66, 13)
        Me.zMagLabel.TabIndex = 12
        Me.zMagLabel.Text = "Z magnitude"
        '
        'yTextBox
        '
        Me.yTextBox.Location = New System.Drawing.Point(9, 166)
        Me.yTextBox.Name = "yTextBox"
        Me.yTextBox.ReadOnly = True
        Me.yTextBox.Size = New System.Drawing.Size(187, 20)
        Me.yTextBox.TabIndex = 17
        Me.yTextBox.Text = "0.000 + 0.00i"
        '
        'yLabel
        '
        Me.yLabel.AutoSize = True
        Me.yLabel.Location = New System.Drawing.Point(9, 149)
        Me.yLabel.Name = "yLabel"
        Me.yLabel.Size = New System.Drawing.Size(14, 13)
        Me.yLabel.TabIndex = 16
        Me.yLabel.Text = "Y"
        '
        'zTextBox
        '
        Me.zTextBox.Location = New System.Drawing.Point(9, 124)
        Me.zTextBox.Name = "zTextBox"
        Me.zTextBox.ReadOnly = True
        Me.zTextBox.Size = New System.Drawing.Size(187, 20)
        Me.zTextBox.TabIndex = 11
        Me.zTextBox.Text = "0.000 + 0.00i"
        '
        'zLabel
        '
        Me.zLabel.AutoSize = True
        Me.zLabel.Location = New System.Drawing.Point(9, 107)
        Me.zLabel.Name = "zLabel"
        Me.zLabel.Size = New System.Drawing.Size(14, 13)
        Me.zLabel.TabIndex = 10
        Me.zLabel.Text = "Z"
        '
        'acCurrentTextBox
        '
        Me.acCurrentTextBox.Location = New System.Drawing.Point(203, 82)
        Me.acCurrentTextBox.Name = "acCurrentTextBox"
        Me.acCurrentTextBox.ReadOnly = True
        Me.acCurrentTextBox.Size = New System.Drawing.Size(187, 20)
        Me.acCurrentTextBox.TabIndex = 9
        Me.acCurrentTextBox.Text = "0.000 + 0.00i"
        '
        'acCurrentLabel
        '
        Me.acCurrentLabel.AutoSize = True
        Me.acCurrentLabel.Location = New System.Drawing.Point(203, 65)
        Me.acCurrentLabel.Name = "acCurrentLabel"
        Me.acCurrentLabel.Size = New System.Drawing.Size(57, 13)
        Me.acCurrentLabel.TabIndex = 8
        Me.acCurrentLabel.Text = "AC current"
        '
        'acVoltageTextBox
        '
        Me.acVoltageTextBox.Location = New System.Drawing.Point(9, 82)
        Me.acVoltageTextBox.Name = "acVoltageTextBox"
        Me.acVoltageTextBox.ReadOnly = True
        Me.acVoltageTextBox.Size = New System.Drawing.Size(187, 20)
        Me.acVoltageTextBox.TabIndex = 7
        Me.acVoltageTextBox.Text = "0.000 + 0.00i"
        '
        'acVoltageLabel
        '
        Me.acVoltageLabel.AutoSize = True
        Me.acVoltageLabel.Location = New System.Drawing.Point(9, 65)
        Me.acVoltageLabel.Name = "acVoltageLabel"
        Me.acVoltageLabel.Size = New System.Drawing.Size(59, 13)
        Me.acVoltageLabel.TabIndex = 6
        Me.acVoltageLabel.Text = "AC voltage"
        '
        'measurementModeTextBox
        '
        Me.measurementModeTextBox.Location = New System.Drawing.Point(9, 334)
        Me.measurementModeTextBox.Name = "measurementModeTextBox"
        Me.measurementModeTextBox.ReadOnly = True
        Me.measurementModeTextBox.Size = New System.Drawing.Size(90, 20)
        Me.measurementModeTextBox.TabIndex = 39
        Me.measurementModeTextBox.Text = "SMU"
        '
        'stimulusFrequencyTextBox
        '
        Me.stimulusFrequencyTextBox.Location = New System.Drawing.Point(203, 40)
        Me.stimulusFrequencyTextBox.Name = "stimulusFrequencyTextBox"
        Me.stimulusFrequencyTextBox.ReadOnly = True
        Me.stimulusFrequencyTextBox.Size = New System.Drawing.Size(90, 20)
        Me.stimulusFrequencyTextBox.TabIndex = 5
        Me.stimulusFrequencyTextBox.Text = "0.000000E+000"
        '
        'measurementModeLabel
        '
        Me.measurementModeLabel.AutoSize = True
        Me.measurementModeLabel.Location = New System.Drawing.Point(9, 317)
        Me.measurementModeLabel.Name = "measurementModeLabel"
        Me.measurementModeLabel.Size = New System.Drawing.Size(99, 13)
        Me.measurementModeLabel.TabIndex = 38
        Me.measurementModeLabel.Text = "measurement mode"
        '
        'stimulusFrequencyLabel
        '
        Me.stimulusFrequencyLabel.AutoSize = True
        Me.stimulusFrequencyLabel.Location = New System.Drawing.Point(203, 23)
        Me.stimulusFrequencyLabel.Name = "stimulusFrequencyLabel"
        Me.stimulusFrequencyLabel.Size = New System.Drawing.Size(94, 13)
        Me.stimulusFrequencyLabel.TabIndex = 4
        Me.stimulusFrequencyLabel.Text = "stimulus frequency"
        '
        'vdcTextBox
        '
        Me.vdcTextBox.Location = New System.Drawing.Point(9, 40)
        Me.vdcTextBox.Name = "vdcTextBox"
        Me.vdcTextBox.ReadOnly = True
        Me.vdcTextBox.Size = New System.Drawing.Size(90, 20)
        Me.vdcTextBox.TabIndex = 1
        Me.vdcTextBox.Text = "0.000000E+000"
        '
        'idcTextBox
        '
        Me.idcTextBox.Location = New System.Drawing.Point(106, 40)
        Me.idcTextBox.Name = "idcTextBox"
        Me.idcTextBox.ReadOnly = True
        Me.idcTextBox.Size = New System.Drawing.Size(90, 20)
        Me.idcTextBox.TabIndex = 3
        Me.idcTextBox.Text = "0.000000E+000"
        '
        'dcInComplianceButtonLed
        '
        Me.dcInComplianceButtonLed.Enabled = False
        Me.dcInComplianceButtonLed.Location = New System.Drawing.Point(9, 376)
        Me.dcInComplianceButtonLed.Name = "dcInComplianceButtonLed"
        Me.dcInComplianceButtonLed.Size = New System.Drawing.Size(21, 22)
        Me.dcInComplianceButtonLed.TabIndex = 41
        Me.dcInComplianceButtonLed.UseVisualStyleBackColor = True
        '
        'cableLengthComboBox
        '
        Me.cableLengthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cableLengthComboBox.FormattingEnabled = True
        Me.cableLengthComboBox.Location = New System.Drawing.Point(5, 148)
        Me.cableLengthComboBox.Name = "cableLengthComboBox"
        Me.cableLengthComboBox.Size = New System.Drawing.Size(125, 21)
        Me.cableLengthComboBox.TabIndex = 7
        '
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.lcrImpedanceAutorangeComboBox)
        Me.configurationGroupBox.Controls.Add(Me.lcrImpedanceAutorangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrSourceDelayModeComboBox)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayUpDown)
        Me.configurationGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.configurationGroupBox.Controls.Add(Me.sourceDelayLabel)
        Me.configurationGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.configurationGroupBox.Controls.Add(Me.lcrSourceDelayModeLabel)
        Me.configurationGroupBox.Controls.Add(Me.cableLengthComboBox)
        Me.configurationGroupBox.Controls.Add(Me.lcrMeasurementTimeComboBox)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasCurrentLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasSourceComboBox)
        Me.configurationGroupBox.Controls.Add(Me.lcrCustomMeasurementTimeUpDown)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasCurrentUpDown)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasVoltageLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrFrequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrFrequencyUpDown)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasVoltageUpDown)
        Me.configurationGroupBox.Controls.Add(Me.lcrCurrentLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrImpedanceRangeUpDown)
        Me.configurationGroupBox.Controls.Add(Me.cableLengthLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrCustomMeasurementTimeLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrCurrentUpDown)
        Me.configurationGroupBox.Controls.Add(Me.lcrImpedanceRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrMeasurementTimeLabel)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(319, 417)
        Me.configurationGroupBox.TabIndex = 0
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'lcrImpedanceAutorangeComboBox
        '
        Me.lcrImpedanceAutorangeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.lcrImpedanceAutorangeComboBox.FormattingEnabled = True
        Me.lcrImpedanceAutorangeComboBox.Location = New System.Drawing.Point(159, 94)
        Me.lcrImpedanceAutorangeComboBox.Name = "lcrImpedanceAutorangeComboBox"
        Me.lcrImpedanceAutorangeComboBox.Size = New System.Drawing.Size(125, 21)
        Me.lcrImpedanceAutorangeComboBox.TabIndex = 5
        '
        'lcrImpedanceAutorangeLabel
        '
        Me.lcrImpedanceAutorangeLabel.AutoSize = True
        Me.lcrImpedanceAutorangeLabel.Location = New System.Drawing.Point(159, 77)
        Me.lcrImpedanceAutorangeLabel.Name = "lcrImpedanceAutorangeLabel"
        Me.lcrImpedanceAutorangeLabel.Size = New System.Drawing.Size(136, 13)
        Me.lcrImpedanceAutorangeLabel.TabIndex = 4
        Me.lcrImpedanceAutorangeLabel.Text = "LCR Impedance Autorange"
        '
        'lcrSourceDelayModeComboBox
        '
        Me.lcrSourceDelayModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.lcrSourceDelayModeComboBox.FormattingEnabled = True
        Me.lcrSourceDelayModeComboBox.Location = New System.Drawing.Point(5, 363)
        Me.lcrSourceDelayModeComboBox.Name = "lcrSourceDelayModeComboBox"
        Me.lcrSourceDelayModeComboBox.Size = New System.Drawing.Size(125, 21)
        Me.lcrSourceDelayModeComboBox.TabIndex = 23
        '
        'sourceDelayUpDown
        '
        Me.sourceDelayUpDown.DecimalPlaces = 6
        Me.sourceDelayUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.sourceDelayUpDown.Location = New System.Drawing.Point(159, 363)
        Me.sourceDelayUpDown.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.sourceDelayUpDown.Name = "sourceDelayUpDown"
        Me.sourceDelayUpDown.Size = New System.Drawing.Size(125, 20)
        Me.sourceDelayUpDown.TabIndex = 25
        Me.sourceDelayUpDown.Value = New Decimal(New Integer() {1666, 0, 0, 327680})
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(5, 23)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'sourceDelayLabel
        '
        Me.sourceDelayLabel.AutoSize = True
        Me.sourceDelayLabel.Location = New System.Drawing.Point(159, 346)
        Me.sourceDelayLabel.Name = "sourceDelayLabel"
        Me.sourceDelayLabel.Size = New System.Drawing.Size(71, 13)
        Me.sourceDelayLabel.TabIndex = 24
        Me.sourceDelayLabel.Text = "Source Delay"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(5, 40)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(275, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'lcrSourceDelayModeLabel
        '
        Me.lcrSourceDelayModeLabel.AutoSize = True
        Me.lcrSourceDelayModeLabel.Location = New System.Drawing.Point(5, 346)
        Me.lcrSourceDelayModeLabel.Name = "lcrSourceDelayModeLabel"
        Me.lcrSourceDelayModeLabel.Size = New System.Drawing.Size(125, 13)
        Me.lcrSourceDelayModeLabel.TabIndex = 22
        Me.lcrSourceDelayModeLabel.Text = "LCR Source Delay Mode"
        '
        'lcrMeasurementTimeComboBox
        '
        Me.lcrMeasurementTimeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.lcrMeasurementTimeComboBox.FormattingEnabled = True
        Me.lcrMeasurementTimeComboBox.Location = New System.Drawing.Point(5, 309)
        Me.lcrMeasurementTimeComboBox.Name = "lcrMeasurementTimeComboBox"
        Me.lcrMeasurementTimeComboBox.Size = New System.Drawing.Size(125, 21)
        Me.lcrMeasurementTimeComboBox.TabIndex = 19
        '
        'lcrDcBiasCurrentLabel
        '
        Me.lcrDcBiasCurrentLabel.AutoSize = True
        Me.lcrDcBiasCurrentLabel.Location = New System.Drawing.Point(159, 239)
        Me.lcrDcBiasCurrentLabel.Name = "lcrDcBiasCurrentLabel"
        Me.lcrDcBiasCurrentLabel.Size = New System.Drawing.Size(106, 13)
        Me.lcrDcBiasCurrentLabel.TabIndex = 16
        Me.lcrDcBiasCurrentLabel.Text = "LCR DC Bias Current"
        '
        'lcrDcBiasSourceComboBox
        '
        Me.lcrDcBiasSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.lcrDcBiasSourceComboBox.FormattingEnabled = True
        Me.lcrDcBiasSourceComboBox.Location = New System.Drawing.Point(159, 201)
        Me.lcrDcBiasSourceComboBox.Name = "lcrDcBiasSourceComboBox"
        Me.lcrDcBiasSourceComboBox.Size = New System.Drawing.Size(125, 21)
        Me.lcrDcBiasSourceComboBox.TabIndex = 13
        '
        'lcrCustomMeasurementTimeUpDown
        '
        Me.lcrCustomMeasurementTimeUpDown.DecimalPlaces = 6
        Me.lcrCustomMeasurementTimeUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.lcrCustomMeasurementTimeUpDown.Location = New System.Drawing.Point(159, 309)
        Me.lcrCustomMeasurementTimeUpDown.Maximum = New Decimal(New Integer() {99999, 0, 0, 327680})
        Me.lcrCustomMeasurementTimeUpDown.Name = "lcrCustomMeasurementTimeUpDown"
        Me.lcrCustomMeasurementTimeUpDown.Size = New System.Drawing.Size(125, 20)
        Me.lcrCustomMeasurementTimeUpDown.TabIndex = 21
        Me.lcrCustomMeasurementTimeUpDown.Value = New Decimal(New Integer() {1, 0, 0, 131072})
        '
        'lcrDcBiasCurrentUpDown
        '
        Me.lcrDcBiasCurrentUpDown.DecimalPlaces = 6
        Me.lcrDcBiasCurrentUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 393216})
        Me.lcrDcBiasCurrentUpDown.Location = New System.Drawing.Point(159, 256)
        Me.lcrDcBiasCurrentUpDown.Maximum = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.lcrDcBiasCurrentUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, -2147418112})
        Me.lcrDcBiasCurrentUpDown.Name = "lcrDcBiasCurrentUpDown"
        Me.lcrDcBiasCurrentUpDown.Size = New System.Drawing.Size(125, 20)
        Me.lcrDcBiasCurrentUpDown.TabIndex = 17
        '
        'lcrDcBiasLabel
        '
        Me.lcrDcBiasLabel.AutoSize = True
        Me.lcrDcBiasLabel.Location = New System.Drawing.Point(159, 184)
        Me.lcrDcBiasLabel.Name = "lcrDcBiasLabel"
        Me.lcrDcBiasLabel.Size = New System.Drawing.Size(106, 13)
        Me.lcrDcBiasLabel.TabIndex = 12
        Me.lcrDcBiasLabel.Text = "LCR DC Bias Source"
        '
        'lcrDcBiasVoltageLabel
        '
        Me.lcrDcBiasVoltageLabel.AutoSize = True
        Me.lcrDcBiasVoltageLabel.Location = New System.Drawing.Point(6, 239)
        Me.lcrDcBiasVoltageLabel.Name = "lcrDcBiasVoltageLabel"
        Me.lcrDcBiasVoltageLabel.Size = New System.Drawing.Size(108, 13)
        Me.lcrDcBiasVoltageLabel.TabIndex = 14
        Me.lcrDcBiasVoltageLabel.Text = "LCR DC Bias Voltage"
        '
        'lcrFrequencyLabel
        '
        Me.lcrFrequencyLabel.AutoSize = True
        Me.lcrFrequencyLabel.Location = New System.Drawing.Point(5, 77)
        Me.lcrFrequencyLabel.Name = "lcrFrequencyLabel"
        Me.lcrFrequencyLabel.Size = New System.Drawing.Size(81, 13)
        Me.lcrFrequencyLabel.TabIndex = 2
        Me.lcrFrequencyLabel.Text = "LCR Frequency"
        '
        'lcrFrequencyUpDown
        '
        Me.lcrFrequencyUpDown.DecimalPlaces = 6
        Me.lcrFrequencyUpDown.Increment = New Decimal(New Integer() {100, 0, 0, 0})
        Me.lcrFrequencyUpDown.Location = New System.Drawing.Point(5, 94)
        Me.lcrFrequencyUpDown.Maximum = New Decimal(New Integer() {20000000, 0, 0, 0})
        Me.lcrFrequencyUpDown.Minimum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.lcrFrequencyUpDown.Name = "lcrFrequencyUpDown"
        Me.lcrFrequencyUpDown.Size = New System.Drawing.Size(125, 20)
        Me.lcrFrequencyUpDown.TabIndex = 3
        Me.lcrFrequencyUpDown.Value = New Decimal(New Integer() {10000, 0, 0, 0})
        '
        'lcrDcBiasVoltageUpDown
        '
        Me.lcrDcBiasVoltageUpDown.DecimalPlaces = 6
        Me.lcrDcBiasVoltageUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.lcrDcBiasVoltageUpDown.Location = New System.Drawing.Point(6, 256)
        Me.lcrDcBiasVoltageUpDown.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.lcrDcBiasVoltageUpDown.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.lcrDcBiasVoltageUpDown.Name = "lcrDcBiasVoltageUpDown"
        Me.lcrDcBiasVoltageUpDown.Size = New System.Drawing.Size(125, 20)
        Me.lcrDcBiasVoltageUpDown.TabIndex = 15
        '
        'lcrCustomMeasurementTimeLabel
        '
        Me.lcrCustomMeasurementTimeLabel.AutoSize = True
        Me.lcrCustomMeasurementTimeLabel.Location = New System.Drawing.Point(159, 292)
        Me.lcrCustomMeasurementTimeLabel.Name = "lcrCustomMeasurementTimeLabel"
        Me.lcrCustomMeasurementTimeLabel.Size = New System.Drawing.Size(159, 13)
        Me.lcrCustomMeasurementTimeLabel.TabIndex = 20
        Me.lcrCustomMeasurementTimeLabel.Text = "LCR Custom Measurement Time"
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(767, 467)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.measurementResultGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "LCR Source AC Current"
        CType(Me.lcrCurrentUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lcrImpedanceRangeUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementResultGroupBox.ResumeLayout(False)
        Me.measurementResultGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.sourceDelayUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lcrCustomMeasurementTimeUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lcrDcBiasCurrentUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lcrFrequencyUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lcrDcBiasVoltageUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private lcrCurrentUpDown As System.Windows.Forms.NumericUpDown
    Private lcrImpedanceRangeUpDown As System.Windows.Forms.NumericUpDown
    Private WithEvents startButton As System.Windows.Forms.Button
    Private measurementResultGroupBox As System.Windows.Forms.GroupBox
    Private dcInComplianceButtonLed As System.Windows.Forms.Button
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private vdcTextBox As System.Windows.Forms.TextBox
    Private idcTextBox As System.Windows.Forms.TextBox
    Private lcrMeasurementTimeLabel As System.Windows.Forms.Label
    Private cableLengthLabel As System.Windows.Forms.Label
    Private lcrCurrentLabel As System.Windows.Forms.Label
    Private lcrImpedanceRangeLabel As System.Windows.Forms.Label
    Private vdcLabel As System.Windows.Forms.Label
    Private idcLabel As System.Windows.Forms.Label
    Private dcInComplianceLabel As System.Windows.Forms.Label
    Private lcrFrequencyLabel As System.Windows.Forms.Label
    Private lcrFrequencyUpDown As System.Windows.Forms.NumericUpDown
    Private stimulusFrequencyTextBox As System.Windows.Forms.TextBox
    Private stimulusFrequencyLabel As System.Windows.Forms.Label
    Private yTextBox As System.Windows.Forms.TextBox
    Private yLabel As System.Windows.Forms.Label
    Private zTextBox As System.Windows.Forms.TextBox
    Private zLabel As System.Windows.Forms.Label
    Private acCurrentTextBox As System.Windows.Forms.TextBox
    Private acCurrentLabel As System.Windows.Forms.Label
    Private acVoltageTextBox As System.Windows.Forms.TextBox
    Private acVoltageLabel As System.Windows.Forms.Label
    Private zMagTextBox As System.Windows.Forms.TextBox
    Private zMagLabel As System.Windows.Forms.Label
    Private zThetaTextBox As System.Windows.Forms.TextBox
    Private zThetaLabel As System.Windows.Forms.Label
    Private yThetaTextBox As System.Windows.Forms.TextBox
    Private yThetaLabel As System.Windows.Forms.Label
    Private yMagTextBox As System.Windows.Forms.TextBox
    Private yMagLabel As System.Windows.Forms.Label
    Private dTextBox As System.Windows.Forms.TextBox
    Private qTextBox As System.Windows.Forms.TextBox
    Private qLabel As System.Windows.Forms.Label
    Private dLabel As System.Windows.Forms.Label
    Private rpTextBox As System.Windows.Forms.TextBox
    Private rpLabel As System.Windows.Forms.Label
    Private lpTextBox As System.Windows.Forms.TextBox
    Private cpTextBox As System.Windows.Forms.TextBox
    Private cpLabel As System.Windows.Forms.Label
    Private lpLabel As System.Windows.Forms.Label
    Private rsTextBox As System.Windows.Forms.TextBox
    Private rsLabel As System.Windows.Forms.Label
    Private lsTextBox As System.Windows.Forms.TextBox
    Private csTextBox As System.Windows.Forms.TextBox
    Private csLabel As System.Windows.Forms.Label
    Private lsLabel As System.Windows.Forms.Label
    Private acInComplianceButtonLed As System.Windows.Forms.Button
    Private acInComplianceLabel As System.Windows.Forms.Label
    Private lcrDcBiasCurrentLabel As System.Windows.Forms.Label
    Private lcrDcBiasCurrentUpDown As System.Windows.Forms.NumericUpDown
    Private lcrDcBiasSourceComboBox As System.Windows.Forms.ComboBox
    Private lcrDcBiasLabel As System.Windows.Forms.Label
    Private lcrDcBiasVoltageLabel As System.Windows.Forms.Label
    Private lcrDcBiasVoltageUpDown As System.Windows.Forms.NumericUpDown
    Private lcrMeasurementTimeComboBox As System.Windows.Forms.ComboBox
    Private measurementModeTextBox As System.Windows.Forms.TextBox
    Private measurementModeLabel As System.Windows.Forms.Label
    Private cableLengthComboBox As System.Windows.Forms.ComboBox
    Private lcrCustomMeasurementTimeUpDown As System.Windows.Forms.NumericUpDown
    Private lcrCustomMeasurementTimeLabel As System.Windows.Forms.Label
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private resourceNameLabel As Windows.Forms.Label
    Private WithEvents unbalancedButtonLed As Windows.Forms.Button
    Private WithEvents unbalancedLabel As Windows.Forms.Label
    Private WithEvents lcrSourceDelayModeComboBox As Windows.Forms.ComboBox
    Private WithEvents sourceDelayUpDown As Windows.Forms.NumericUpDown
    Private WithEvents sourceDelayLabel As Windows.Forms.Label
    Private WithEvents lcrSourceDelayModeLabel As Windows.Forms.Label
    Private WithEvents lcrImpedanceAutorangeComboBox As Windows.Forms.ComboBox
    Private WithEvents lcrImpedanceAutorangeLabel As Windows.Forms.Label
End Class
