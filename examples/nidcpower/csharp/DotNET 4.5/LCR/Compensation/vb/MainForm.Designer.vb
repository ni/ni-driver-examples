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
        Me.vdcLabel = New System.Windows.Forms.Label()
        Me.idcLabel = New System.Windows.Forms.Label()
        Me.dcInComplianceLabel = New System.Windows.Forms.Label()
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
        Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.lcrDcBiasVoltageLabel = New System.Windows.Forms.Label()
        Me.lcrDcBiasVoltageUpDown = New System.Windows.Forms.NumericUpDown()
        Me.cableLengthComboBox = New System.Windows.Forms.ComboBox()
        Me.lcrMeasurementTimeComboBox = New System.Windows.Forms.ComboBox()
        Me.lcrDcBiasSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.lcrCustomMeasurementTimeUpDown = New System.Windows.Forms.NumericUpDown()
        Me.lcrDcBiasLabel = New System.Windows.Forms.Label()
        Me.lcrDcBiasCurrentLabel = New System.Windows.Forms.Label()
        Me.lcrFrequencyLabel = New System.Windows.Forms.Label()
        Me.lcrFrequencyUpDown = New System.Windows.Forms.NumericUpDown()
        Me.lcrDcBiasCurrentUpDown = New System.Windows.Forms.NumericUpDown()
        Me.lcrVoltageLabel = New System.Windows.Forms.Label()
        Me.lcrImpedanceRangeUpDown = New System.Windows.Forms.NumericUpDown()
        Me.cableLengthLabel = New System.Windows.Forms.Label()
        Me.lcrCustomMeasurementTimeLabel = New System.Windows.Forms.Label()
        Me.lcrVoltageUpDown = New System.Windows.Forms.NumericUpDown()
        Me.lcrImpedanceRangeLabel = New System.Windows.Forms.Label()
        Me.lcrMeasurementTimeLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.additionalCompensationFrequenciesDataGridView = New System.Windows.Forms.DataGridView()
        Me.columnFrequency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.applyCompensationDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.enableLcrLoadCompensationCheckBox = New System.Windows.Forms.CheckBox()
        Me.enableLcrShortCompensationCheckBox = New System.Windows.Forms.CheckBox()
        Me.enableLcrOpenCompensationCheckBox = New System.Windows.Forms.CheckBox()
        Me.generateLcrCompensationDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.loadCompensationSpotsDataGridView = New System.Windows.Forms.DataGridView()
        Me.columnSpotFrequency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.columnSpotReferenceValueType = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.columnSpotReferenceValueA = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.columnSpotReferenceValueB = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.generateLcrLoadCompensationDataCheckBox = New System.Windows.Forms.CheckBox()
        Me.generateLcrShortCompensationDataCheckBox = New System.Windows.Forms.CheckBox()
        Me.optionalAdditionalFrequenciesLabel = New System.Windows.Forms.Label()
        Me.generateLcrOpenCompensationDataCheckBox = New System.Windows.Forms.CheckBox()
        Me.generateLcrCustomCableCompensationDataCheckBox = New System.Windows.Forms.CheckBox()
        Me.measurementResultGroupBox.SuspendLayout()
        Me.configurationGroupBox.SuspendLayout()
        CType(Me.lcrDcBiasVoltageUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lcrCustomMeasurementTimeUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lcrFrequencyUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lcrDcBiasCurrentUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lcrImpedanceRangeUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lcrVoltageUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.additionalCompensationFrequenciesDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.applyCompensationDataGroupBox.SuspendLayout()
        Me.generateLcrCompensationDataGroupBox.SuspendLayout()
        CType(Me.loadCompensationSpotsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        'startButton
        '
        Me.startButton.Location = New System.Drawing.Point(912, 450)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(75, 23)
        Me.startButton.TabIndex = 4
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
        Me.measurementResultGroupBox.Location = New System.Drawing.Point(904, 12)
        Me.measurementResultGroupBox.Name = "measurementResultGroupBox"
        Me.measurementResultGroupBox.Size = New System.Drawing.Size(396, 434)
        Me.measurementResultGroupBox.TabIndex = 3
        Me.measurementResultGroupBox.TabStop = False
        Me.measurementResultGroupBox.Text = "Measurement Result"
        '
        'unbalancedButtonLed
        '
        Me.unbalancedButtonLed.Enabled = False
        Me.unbalancedButtonLed.Location = New System.Drawing.Point(206, 376)
        Me.unbalancedButtonLed.Name = "unbalancedButtonLed"
        Me.unbalancedButtonLed.Size = New System.Drawing.Size(21, 22)
        Me.unbalancedButtonLed.TabIndex = 45
        Me.unbalancedButtonLed.UseVisualStyleBackColor = True
        '
        'unbalancedLabel
        '
        Me.unbalancedLabel.AutoSize = True
        Me.unbalancedLabel.Location = New System.Drawing.Point(206, 359)
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
        'configurationGroupBox
        '
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasVoltageLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasVoltageUpDown)
        Me.configurationGroupBox.Controls.Add(Me.cableLengthComboBox)
        Me.configurationGroupBox.Controls.Add(Me.lcrMeasurementTimeComboBox)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasSourceComboBox)
        Me.configurationGroupBox.Controls.Add(Me.lcrCustomMeasurementTimeUpDown)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasCurrentLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrFrequencyLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrFrequencyUpDown)
        Me.configurationGroupBox.Controls.Add(Me.lcrDcBiasCurrentUpDown)
        Me.configurationGroupBox.Controls.Add(Me.lcrVoltageLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrImpedanceRangeUpDown)
        Me.configurationGroupBox.Controls.Add(Me.cableLengthLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrCustomMeasurementTimeLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrVoltageUpDown)
        Me.configurationGroupBox.Controls.Add(Me.lcrImpedanceRangeLabel)
        Me.configurationGroupBox.Controls.Add(Me.lcrMeasurementTimeLabel)
        Me.configurationGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.configurationGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.configurationGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.configurationGroupBox.Name = "configurationGroupBox"
        Me.configurationGroupBox.Size = New System.Drawing.Size(588, 236)
        Me.configurationGroupBox.TabIndex = 0
        Me.configurationGroupBox.TabStop = False
        Me.configurationGroupBox.Text = "Configuration"
        '
        'lcrDcBiasVoltageLabel
        '
        Me.lcrDcBiasVoltageLabel.AutoSize = True
        Me.lcrDcBiasVoltageLabel.Location = New System.Drawing.Point(178, 128)
        Me.lcrDcBiasVoltageLabel.Name = "lcrDcBiasVoltageLabel"
        Me.lcrDcBiasVoltageLabel.Size = New System.Drawing.Size(108, 13)
        Me.lcrDcBiasVoltageLabel.TabIndex = 10
        Me.lcrDcBiasVoltageLabel.Text = "LCR DC Bias Voltage"
        '
        'lcrDcBiasVoltageUpDown
        '
        Me.lcrDcBiasVoltageUpDown.DecimalPlaces = 6
        Me.lcrDcBiasVoltageUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.lcrDcBiasVoltageUpDown.Location = New System.Drawing.Point(178, 145)
        Me.lcrDcBiasVoltageUpDown.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.lcrDcBiasVoltageUpDown.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.lcrDcBiasVoltageUpDown.Name = "lcrDcBiasVoltageUpDown"
        Me.lcrDcBiasVoltageUpDown.Size = New System.Drawing.Size(125, 20)
        Me.lcrDcBiasVoltageUpDown.TabIndex = 11
        '
        'cableLengthComboBox
        '
        Me.cableLengthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cableLengthComboBox.FormattingEnabled = True
        Me.cableLengthComboBox.Location = New System.Drawing.Point(350, 201)
        Me.cableLengthComboBox.Name = "cableLengthComboBox"
        Me.cableLengthComboBox.Size = New System.Drawing.Size(125, 21)
        Me.cableLengthComboBox.TabIndex = 19
        '
        'lcrMeasurementTimeComboBox
        '
        Me.lcrMeasurementTimeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.lcrMeasurementTimeComboBox.FormattingEnabled = True
        Me.lcrMeasurementTimeComboBox.Location = New System.Drawing.Point(350, 93)
        Me.lcrMeasurementTimeComboBox.Name = "lcrMeasurementTimeComboBox"
        Me.lcrMeasurementTimeComboBox.Size = New System.Drawing.Size(125, 21)
        Me.lcrMeasurementTimeComboBox.TabIndex = 7
        '
        'lcrDcBiasSourceComboBox
        '
        Me.lcrDcBiasSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.lcrDcBiasSourceComboBox.FormattingEnabled = True
        Me.lcrDcBiasSourceComboBox.Location = New System.Drawing.Point(178, 93)
        Me.lcrDcBiasSourceComboBox.Name = "lcrDcBiasSourceComboBox"
        Me.lcrDcBiasSourceComboBox.Size = New System.Drawing.Size(125, 21)
        Me.lcrDcBiasSourceComboBox.TabIndex = 5
        '
        'lcrCustomMeasurementTimeUpDown
        '
        Me.lcrCustomMeasurementTimeUpDown.DecimalPlaces = 6
        Me.lcrCustomMeasurementTimeUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.lcrCustomMeasurementTimeUpDown.Location = New System.Drawing.Point(350, 145)
        Me.lcrCustomMeasurementTimeUpDown.Maximum = New Decimal(New Integer() {99999, 0, 0, 327680})
        Me.lcrCustomMeasurementTimeUpDown.Name = "lcrCustomMeasurementTimeUpDown"
        Me.lcrCustomMeasurementTimeUpDown.Size = New System.Drawing.Size(125, 20)
        Me.lcrCustomMeasurementTimeUpDown.TabIndex = 13
        Me.lcrCustomMeasurementTimeUpDown.Value = New Decimal(New Integer() {1, 0, 0, 131072})
        '
        'lcrDcBiasLabel
        '
        Me.lcrDcBiasLabel.AutoSize = True
        Me.lcrDcBiasLabel.Location = New System.Drawing.Point(178, 76)
        Me.lcrDcBiasLabel.Name = "lcrDcBiasLabel"
        Me.lcrDcBiasLabel.Size = New System.Drawing.Size(106, 13)
        Me.lcrDcBiasLabel.TabIndex = 4
        Me.lcrDcBiasLabel.Text = "LCR DC Bias Source"
        '
        'lcrDcBiasCurrentLabel
        '
        Me.lcrDcBiasCurrentLabel.AutoSize = True
        Me.lcrDcBiasCurrentLabel.Location = New System.Drawing.Point(178, 184)
        Me.lcrDcBiasCurrentLabel.Name = "lcrDcBiasCurrentLabel"
        Me.lcrDcBiasCurrentLabel.Size = New System.Drawing.Size(106, 13)
        Me.lcrDcBiasCurrentLabel.TabIndex = 16
        Me.lcrDcBiasCurrentLabel.Text = "LCR DC Bias Current"
        '
        'lcrFrequencyLabel
        '
        Me.lcrFrequencyLabel.AutoSize = True
        Me.lcrFrequencyLabel.Location = New System.Drawing.Point(8, 76)
        Me.lcrFrequencyLabel.Name = "lcrFrequencyLabel"
        Me.lcrFrequencyLabel.Size = New System.Drawing.Size(81, 13)
        Me.lcrFrequencyLabel.TabIndex = 2
        Me.lcrFrequencyLabel.Text = "LCR Frequency"
        '
        'lcrFrequencyUpDown
        '
        Me.lcrFrequencyUpDown.DecimalPlaces = 6
        Me.lcrFrequencyUpDown.Increment = New Decimal(New Integer() {100, 0, 0, 0})
        Me.lcrFrequencyUpDown.Location = New System.Drawing.Point(8, 93)
        Me.lcrFrequencyUpDown.Maximum = New Decimal(New Integer() {20000000, 0, 0, 0})
        Me.lcrFrequencyUpDown.Minimum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.lcrFrequencyUpDown.Name = "lcrFrequencyUpDown"
        Me.lcrFrequencyUpDown.Size = New System.Drawing.Size(125, 20)
        Me.lcrFrequencyUpDown.TabIndex = 3
        Me.lcrFrequencyUpDown.Value = New Decimal(New Integer() {10000, 0, 0, 0})
        '
        'lcrDcBiasCurrentUpDown
        '
        Me.lcrDcBiasCurrentUpDown.DecimalPlaces = 6
        Me.lcrDcBiasCurrentUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.lcrDcBiasCurrentUpDown.Location = New System.Drawing.Point(178, 201)
        Me.lcrDcBiasCurrentUpDown.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.lcrDcBiasCurrentUpDown.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.lcrDcBiasCurrentUpDown.Name = "lcrDcBiasCurrentUpDown"
        Me.lcrDcBiasCurrentUpDown.Size = New System.Drawing.Size(125, 20)
        Me.lcrDcBiasCurrentUpDown.TabIndex = 17
        '
        'lcrVoltageLabel
        '
        Me.lcrVoltageLabel.AutoSize = True
        Me.lcrVoltageLabel.Location = New System.Drawing.Point(8, 128)
        Me.lcrVoltageLabel.Name = "lcrVoltageLabel"
        Me.lcrVoltageLabel.Size = New System.Drawing.Size(94, 13)
        Me.lcrVoltageLabel.TabIndex = 8
        Me.lcrVoltageLabel.Text = "LCR Voltage RMS"
        '
        'lcrImpedanceRangeUpDown
        '
        Me.lcrImpedanceRangeUpDown.DecimalPlaces = 6
        Me.lcrImpedanceRangeUpDown.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.lcrImpedanceRangeUpDown.Location = New System.Drawing.Point(8, 201)
        Me.lcrImpedanceRangeUpDown.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.lcrImpedanceRangeUpDown.Name = "lcrImpedanceRangeUpDown"
        Me.lcrImpedanceRangeUpDown.Size = New System.Drawing.Size(125, 20)
        Me.lcrImpedanceRangeUpDown.TabIndex = 15
        Me.lcrImpedanceRangeUpDown.Value = New Decimal(New Integer() {100000, 0, 0, 196608})
        '
        'cableLengthLabel
        '
        Me.cableLengthLabel.AutoSize = True
        Me.cableLengthLabel.Location = New System.Drawing.Point(350, 184)
        Me.cableLengthLabel.Name = "cableLengthLabel"
        Me.cableLengthLabel.Size = New System.Drawing.Size(70, 13)
        Me.cableLengthLabel.TabIndex = 18
        Me.cableLengthLabel.Text = "Cable Length"
        '
        'lcrCustomMeasurementTimeLabel
        '
        Me.lcrCustomMeasurementTimeLabel.AutoSize = True
        Me.lcrCustomMeasurementTimeLabel.Location = New System.Drawing.Point(350, 128)
        Me.lcrCustomMeasurementTimeLabel.Name = "lcrCustomMeasurementTimeLabel"
        Me.lcrCustomMeasurementTimeLabel.Size = New System.Drawing.Size(159, 13)
        Me.lcrCustomMeasurementTimeLabel.TabIndex = 12
        Me.lcrCustomMeasurementTimeLabel.Text = "LCR Custom Measurement Time"
        '
        'lcrVoltageUpDown
        '
        Me.lcrVoltageUpDown.DecimalPlaces = 6
        Me.lcrVoltageUpDown.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.lcrVoltageUpDown.Location = New System.Drawing.Point(8, 145)
        Me.lcrVoltageUpDown.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.lcrVoltageUpDown.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.lcrVoltageUpDown.Name = "lcrVoltageUpDown"
        Me.lcrVoltageUpDown.Size = New System.Drawing.Size(125, 20)
        Me.lcrVoltageUpDown.TabIndex = 9
        Me.lcrVoltageUpDown.Value = New Decimal(New Integer() {707, 0, 0, 196608})
        '
        'lcrImpedanceRangeLabel
        '
        Me.lcrImpedanceRangeLabel.AutoSize = True
        Me.lcrImpedanceRangeLabel.Location = New System.Drawing.Point(8, 184)
        Me.lcrImpedanceRangeLabel.Name = "lcrImpedanceRangeLabel"
        Me.lcrImpedanceRangeLabel.Size = New System.Drawing.Size(119, 13)
        Me.lcrImpedanceRangeLabel.TabIndex = 14
        Me.lcrImpedanceRangeLabel.Text = "LCR Impedance Range"
        '
        'lcrMeasurementTimeLabel
        '
        Me.lcrMeasurementTimeLabel.AutoSize = True
        Me.lcrMeasurementTimeLabel.Location = New System.Drawing.Point(350, 76)
        Me.lcrMeasurementTimeLabel.Name = "lcrMeasurementTimeLabel"
        Me.lcrMeasurementTimeLabel.Size = New System.Drawing.Size(121, 13)
        Me.lcrMeasurementTimeLabel.TabIndex = 6
        Me.lcrMeasurementTimeLabel.Text = "LCR Measurement Time"
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
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(5, 40)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(275, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'additionalCompensationFrequenciesDataGridView
        '
        Me.additionalCompensationFrequenciesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.additionalCompensationFrequenciesDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.columnFrequency})
        Me.additionalCompensationFrequenciesDataGridView.Location = New System.Drawing.Point(178, 50)
        Me.additionalCompensationFrequenciesDataGridView.Margin = New System.Windows.Forms.Padding(2)
        Me.additionalCompensationFrequenciesDataGridView.Name = "additionalCompensationFrequenciesDataGridView"
        Me.additionalCompensationFrequenciesDataGridView.RowHeadersWidth = 51
        Me.additionalCompensationFrequenciesDataGridView.RowTemplate.Height = 24
        Me.additionalCompensationFrequenciesDataGridView.Size = New System.Drawing.Size(154, 128)
        Me.additionalCompensationFrequenciesDataGridView.TabIndex = 5
        '
        'columnFrequency
        '
        Me.columnFrequency.HeaderText = "Frequency"
        Me.columnFrequency.MinimumWidth = 6
        Me.columnFrequency.Name = "columnFrequency"
        '
        'applyCompensationDataGroupBox
        '
        Me.applyCompensationDataGroupBox.Controls.Add(Me.enableLcrLoadCompensationCheckBox)
        Me.applyCompensationDataGroupBox.Controls.Add(Me.enableLcrShortCompensationCheckBox)
        Me.applyCompensationDataGroupBox.Controls.Add(Me.enableLcrOpenCompensationCheckBox)
        Me.applyCompensationDataGroupBox.Location = New System.Drawing.Point(614, 12)
        Me.applyCompensationDataGroupBox.Name = "applyCompensationDataGroupBox"
        Me.applyCompensationDataGroupBox.Size = New System.Drawing.Size(278, 236)
        Me.applyCompensationDataGroupBox.TabIndex = 1
        Me.applyCompensationDataGroupBox.TabStop = False
        Me.applyCompensationDataGroupBox.Text = "Apply Compensation Data to Measurements"
        '
        'enableLcrLoadCompensationCheckBox
        '
        Me.enableLcrLoadCompensationCheckBox.Location = New System.Drawing.Point(10, 108)
        Me.enableLcrLoadCompensationCheckBox.Margin = New System.Windows.Forms.Padding(2)
        Me.enableLcrLoadCompensationCheckBox.Name = "enableLcrLoadCompensationCheckBox"
        Me.enableLcrLoadCompensationCheckBox.Size = New System.Drawing.Size(190, 35)
        Me.enableLcrLoadCompensationCheckBox.TabIndex = 2
        Me.enableLcrLoadCompensationCheckBox.Text = "Enable LCR Load Compensation"
        Me.enableLcrLoadCompensationCheckBox.UseVisualStyleBackColor = True
        '
        'enableLcrShortCompensationCheckBox
        '
        Me.enableLcrShortCompensationCheckBox.Location = New System.Drawing.Point(10, 67)
        Me.enableLcrShortCompensationCheckBox.Margin = New System.Windows.Forms.Padding(2)
        Me.enableLcrShortCompensationCheckBox.Name = "enableLcrShortCompensationCheckBox"
        Me.enableLcrShortCompensationCheckBox.Size = New System.Drawing.Size(190, 35)
        Me.enableLcrShortCompensationCheckBox.TabIndex = 1
        Me.enableLcrShortCompensationCheckBox.Text = "Enable LCR Short Compensation"
        Me.enableLcrShortCompensationCheckBox.UseVisualStyleBackColor = True
        '
        'enableLcrOpenCompensationCheckBox
        '
        Me.enableLcrOpenCompensationCheckBox.Location = New System.Drawing.Point(10, 24)
        Me.enableLcrOpenCompensationCheckBox.Margin = New System.Windows.Forms.Padding(2)
        Me.enableLcrOpenCompensationCheckBox.Name = "enableLcrOpenCompensationCheckBox"
        Me.enableLcrOpenCompensationCheckBox.Size = New System.Drawing.Size(190, 35)
        Me.enableLcrOpenCompensationCheckBox.TabIndex = 0
        Me.enableLcrOpenCompensationCheckBox.Text = "Enable LCR Open Compensation"
        Me.enableLcrOpenCompensationCheckBox.UseVisualStyleBackColor = True
        '
        'generateLcrCompensationDataGroupBox
        '
        Me.generateLcrCompensationDataGroupBox.Controls.Add(Me.Label1)
        Me.generateLcrCompensationDataGroupBox.Controls.Add(Me.loadCompensationSpotsDataGridView)
        Me.generateLcrCompensationDataGroupBox.Controls.Add(Me.generateLcrLoadCompensationDataCheckBox)
        Me.generateLcrCompensationDataGroupBox.Controls.Add(Me.generateLcrShortCompensationDataCheckBox)
        Me.generateLcrCompensationDataGroupBox.Controls.Add(Me.optionalAdditionalFrequenciesLabel)
        Me.generateLcrCompensationDataGroupBox.Controls.Add(Me.additionalCompensationFrequenciesDataGridView)
        Me.generateLcrCompensationDataGroupBox.Controls.Add(Me.generateLcrOpenCompensationDataCheckBox)
        Me.generateLcrCompensationDataGroupBox.Controls.Add(Me.generateLcrCustomCableCompensationDataCheckBox)
        Me.generateLcrCompensationDataGroupBox.Location = New System.Drawing.Point(12, 254)
        Me.generateLcrCompensationDataGroupBox.Name = "generateLcrCompensationDataGroupBox"
        Me.generateLcrCompensationDataGroupBox.Size = New System.Drawing.Size(880, 192)
        Me.generateLcrCompensationDataGroupBox.TabIndex = 2
        Me.generateLcrCompensationDataGroupBox.TabStop = False
        Me.generateLcrCompensationDataGroupBox.Text = "Generate LCR Compensation Data"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(344, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(131, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Load Compensation Spots"
        '
        'loadCompensationSpotsDataGridView
        '
        Me.loadCompensationSpotsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.loadCompensationSpotsDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.columnSpotFrequency, Me.columnSpotReferenceValueType, Me.columnSpotReferenceValueA, Me.columnSpotReferenceValueB})
        Me.loadCompensationSpotsDataGridView.Location = New System.Drawing.Point(344, 38)
        Me.loadCompensationSpotsDataGridView.Margin = New System.Windows.Forms.Padding(2)
        Me.loadCompensationSpotsDataGridView.Name = "loadCompensationSpotsDataGridView"
        Me.loadCompensationSpotsDataGridView.RowHeadersWidth = 51
        Me.loadCompensationSpotsDataGridView.RowTemplate.Height = 24
        Me.loadCompensationSpotsDataGridView.Size = New System.Drawing.Size(524, 140)
        Me.loadCompensationSpotsDataGridView.TabIndex = 7
        '
        'columnSpotFrequency
        '
        Me.columnSpotFrequency.HeaderText = "frequency"
        Me.columnSpotFrequency.MinimumWidth = 6
        Me.columnSpotFrequency.Name = "columnSpotFrequency"
        Me.columnSpotFrequency.Width = 80
        '
        'columnSpotReferenceValueType
        '
        Me.columnSpotReferenceValueType.HeaderText = "reference value type"
        Me.columnSpotReferenceValueType.MinimumWidth = 6
        Me.columnSpotReferenceValueType.Name = "columnSpotReferenceValueType"
        Me.columnSpotReferenceValueType.Width = 130
        '
        'columnSpotReferenceValueA
        '
        Me.columnSpotReferenceValueA.HeaderText = "reference value A"
        Me.columnSpotReferenceValueA.MinimumWidth = 6
        Me.columnSpotReferenceValueA.Name = "columnSpotReferenceValueA"
        Me.columnSpotReferenceValueA.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.columnSpotReferenceValueA.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.columnSpotReferenceValueA.Width = 130
        '
        'columnSpotReferenceValueB
        '
        Me.columnSpotReferenceValueB.HeaderText = "reference value B"
        Me.columnSpotReferenceValueB.MinimumWidth = 6
        Me.columnSpotReferenceValueB.Name = "columnSpotReferenceValueB"
        Me.columnSpotReferenceValueB.Width = 130
        '
        'generateLcrLoadCompensationDataCheckBox
        '
        Me.generateLcrLoadCompensationDataCheckBox.Location = New System.Drawing.Point(10, 144)
        Me.generateLcrLoadCompensationDataCheckBox.Margin = New System.Windows.Forms.Padding(2)
        Me.generateLcrLoadCompensationDataCheckBox.Name = "generateLcrLoadCompensationDataCheckBox"
        Me.generateLcrLoadCompensationDataCheckBox.Size = New System.Drawing.Size(162, 35)
        Me.generateLcrLoadCompensationDataCheckBox.TabIndex = 3
        Me.generateLcrLoadCompensationDataCheckBox.Text = "Generate LCR Load Compensation Data"
        Me.generateLcrLoadCompensationDataCheckBox.UseVisualStyleBackColor = True
        '
        'generateLcrShortCompensationDataCheckBox
        '
        Me.generateLcrShortCompensationDataCheckBox.Location = New System.Drawing.Point(10, 104)
        Me.generateLcrShortCompensationDataCheckBox.Margin = New System.Windows.Forms.Padding(2)
        Me.generateLcrShortCompensationDataCheckBox.Name = "generateLcrShortCompensationDataCheckBox"
        Me.generateLcrShortCompensationDataCheckBox.Size = New System.Drawing.Size(162, 35)
        Me.generateLcrShortCompensationDataCheckBox.TabIndex = 2
        Me.generateLcrShortCompensationDataCheckBox.Text = "Generate LCR Short Compensation Data"
        Me.generateLcrShortCompensationDataCheckBox.UseVisualStyleBackColor = True
        '
        'optionalAdditionalFrequenciesLabel
        '
        Me.optionalAdditionalFrequenciesLabel.AutoSize = True
        Me.optionalAdditionalFrequenciesLabel.Location = New System.Drawing.Point(178, 20)
        Me.optionalAdditionalFrequenciesLabel.MaximumSize = New System.Drawing.Size(156, 0)
        Me.optionalAdditionalFrequenciesLabel.Name = "optionalAdditionalFrequenciesLabel"
        Me.optionalAdditionalFrequenciesLabel.Size = New System.Drawing.Size(156, 26)
        Me.optionalAdditionalFrequenciesLabel.TabIndex = 4
        Me.optionalAdditionalFrequenciesLabel.Text = "Open/Short Compensation Optional Additional Frequencies"
        '
        'generateLcrOpenCompensationDataCheckBox
        '
        Me.generateLcrOpenCompensationDataCheckBox.Location = New System.Drawing.Point(10, 64)
        Me.generateLcrOpenCompensationDataCheckBox.Margin = New System.Windows.Forms.Padding(2)
        Me.generateLcrOpenCompensationDataCheckBox.Name = "generateLcrOpenCompensationDataCheckBox"
        Me.generateLcrOpenCompensationDataCheckBox.Size = New System.Drawing.Size(162, 35)
        Me.generateLcrOpenCompensationDataCheckBox.TabIndex = 1
        Me.generateLcrOpenCompensationDataCheckBox.Text = "Generate LCR Open Compensation Data"
        Me.generateLcrOpenCompensationDataCheckBox.UseVisualStyleBackColor = True
        '
        'generateLcrCustomCableCompensationDataCheckBox
        '
        Me.generateLcrCustomCableCompensationDataCheckBox.Location = New System.Drawing.Point(10, 24)
        Me.generateLcrCustomCableCompensationDataCheckBox.Margin = New System.Windows.Forms.Padding(2)
        Me.generateLcrCustomCableCompensationDataCheckBox.Name = "generateLcrCustomCableCompensationDataCheckBox"
        Me.generateLcrCustomCableCompensationDataCheckBox.Size = New System.Drawing.Size(162, 35)
        Me.generateLcrCustomCableCompensationDataCheckBox.TabIndex = 0
        Me.generateLcrCustomCableCompensationDataCheckBox.Text = "Generate LCR Custom Cable Compensation Data"
        Me.generateLcrCustomCableCompensationDataCheckBox.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AcceptButton = Me.startButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1312, 479)
        Me.Controls.Add(Me.applyCompensationDataGroupBox)
        Me.Controls.Add(Me.generateLcrCompensationDataGroupBox)
        Me.Controls.Add(Me.configurationGroupBox)
        Me.Controls.Add(Me.measurementResultGroupBox)
        Me.Controls.Add(Me.startButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "LCR Compensation"
        Me.measurementResultGroupBox.ResumeLayout(False)
        Me.measurementResultGroupBox.PerformLayout()
        Me.configurationGroupBox.ResumeLayout(False)
        Me.configurationGroupBox.PerformLayout()
        CType(Me.lcrDcBiasVoltageUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lcrCustomMeasurementTimeUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lcrFrequencyUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lcrDcBiasCurrentUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lcrImpedanceRangeUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lcrVoltageUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.additionalCompensationFrequenciesDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.applyCompensationDataGroupBox.ResumeLayout(False)
        Me.generateLcrCompensationDataGroupBox.ResumeLayout(False)
        Me.generateLcrCompensationDataGroupBox.PerformLayout()
        CType(Me.loadCompensationSpotsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region
    Private WithEvents startButton As System.Windows.Forms.Button
    Private measurementResultGroupBox As System.Windows.Forms.GroupBox
    Private dcInComplianceButtonLed As System.Windows.Forms.Button
    Private configurationGroupBox As System.Windows.Forms.GroupBox
    Private vdcTextBox As System.Windows.Forms.TextBox
    Private idcTextBox As System.Windows.Forms.TextBox
    Private vdcLabel As System.Windows.Forms.Label
    Private idcLabel As System.Windows.Forms.Label
    Private dcInComplianceLabel As System.Windows.Forms.Label
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
    Private measurementModeTextBox As System.Windows.Forms.TextBox
    Private measurementModeLabel As System.Windows.Forms.Label
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private resourceNameLabel As Windows.Forms.Label
    Private WithEvents additionalCompensationFrequenciesDataGridView As Windows.Forms.DataGridView
    Private WithEvents applyCompensationDataGroupBox As Windows.Forms.GroupBox
    Private WithEvents enableLcrShortCompensationCheckBox As Windows.Forms.CheckBox
    Private WithEvents enableLcrOpenCompensationCheckBox As Windows.Forms.CheckBox
    Private WithEvents generateLcrCompensationDataGroupBox As Windows.Forms.GroupBox
    Private WithEvents generateLcrOpenCompensationDataCheckBox As Windows.Forms.CheckBox
    Private WithEvents generateLcrCustomCableCompensationDataCheckBox As Windows.Forms.CheckBox
    Private WithEvents optionalAdditionalFrequenciesLabel As Windows.Forms.Label
    Private WithEvents lcrDcBiasVoltageLabel As Windows.Forms.Label
    Private WithEvents lcrDcBiasVoltageUpDown As Windows.Forms.NumericUpDown
    Private WithEvents cableLengthComboBox As Windows.Forms.ComboBox
    Private WithEvents lcrMeasurementTimeComboBox As Windows.Forms.ComboBox
    Private WithEvents lcrDcBiasSourceComboBox As Windows.Forms.ComboBox
    Private WithEvents lcrCustomMeasurementTimeUpDown As Windows.Forms.NumericUpDown
    Private WithEvents lcrDcBiasLabel As Windows.Forms.Label
    Private WithEvents lcrDcBiasCurrentLabel As Windows.Forms.Label
    Private WithEvents lcrFrequencyLabel As Windows.Forms.Label
    Private WithEvents lcrFrequencyUpDown As Windows.Forms.NumericUpDown
    Private WithEvents lcrDcBiasCurrentUpDown As Windows.Forms.NumericUpDown
    Private WithEvents lcrVoltageLabel As Windows.Forms.Label
    Private WithEvents lcrImpedanceRangeUpDown As Windows.Forms.NumericUpDown
    Private WithEvents cableLengthLabel As Windows.Forms.Label
    Private WithEvents lcrCustomMeasurementTimeLabel As Windows.Forms.Label
    Private WithEvents lcrVoltageUpDown As Windows.Forms.NumericUpDown
    Private WithEvents lcrImpedanceRangeLabel As Windows.Forms.Label
    Private WithEvents lcrMeasurementTimeLabel As Windows.Forms.Label
    Private WithEvents unbalancedButtonLed As Windows.Forms.Button
    Private WithEvents unbalancedLabel As Windows.Forms.Label
    Private WithEvents enableLcrLoadCompensationCheckBox As Windows.Forms.CheckBox
    Private WithEvents generateLcrLoadCompensationDataCheckBox As Windows.Forms.CheckBox
    Private WithEvents generateLcrShortCompensationDataCheckBox As Windows.Forms.CheckBox
    Private WithEvents Label1 As Windows.Forms.Label
    Private WithEvents loadCompensationSpotsDataGridView As Windows.Forms.DataGridView
    Friend WithEvents columnSpotFrequency As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents columnSpotReferenceValueType As Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents columnSpotReferenceValueA As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents columnSpotReferenceValueB As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents columnFrequency As Windows.Forms.DataGridViewTextBoxColumn
End Class
