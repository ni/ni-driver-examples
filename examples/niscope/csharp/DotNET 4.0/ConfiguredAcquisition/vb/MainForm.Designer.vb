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
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.timeoutLabel = New System.Windows.Forms.Label()
        Me.acquisitionTypeLabel = New System.Windows.Forms.Label()
        Me.verticalRangeLabel = New System.Windows.Forms.Label()
        Me.verticalOffsetLabel = New System.Windows.Forms.Label()
        Me.probeAttenuationLabel = New System.Windows.Forms.Label()
        Me.verticalCouplingLabel = New System.Windows.Forms.Label()
        Me.minSampleRateLabel = New System.Windows.Forms.Label()
        Me.actualSampleRateLabel = New System.Windows.Forms.Label()
        Me.minRecordLengthLabel = New System.Windows.Forms.Label()
        Me.actualRecordLengthLabel = New System.Windows.Forms.Label()
        Me.numRecordsLabel = New System.Windows.Forms.Label()
        Me.maxInputFrequencyLabel = New System.Windows.Forms.Label()
        Me.inputImpedanceLabel = New System.Windows.Forms.Label()
        Me.triggerTypeLabel = New System.Windows.Forms.Label()
        Me.triggerSourceLabel = New System.Windows.Forms.Label()
        Me.referencePositionLabel = New System.Windows.Forms.Label()
        Me.triggerHoldoffLabel = New System.Windows.Forms.Label()
        Me.triggerDelayLabel = New System.Windows.Forms.Label()
        Me.triggerLevelLabel = New System.Windows.Forms.Label()
        Me.triggerCouplingLabel = New System.Windows.Forms.Label()
        Me.triggerSlopeLabel = New System.Windows.Forms.Label()
        Me.hysteresisLabel = New System.Windows.Forms.Label()
        Me.windowModeLabel = New System.Windows.Forms.Label()
        Me.lowLevelWindowLabel = New System.Windows.Forms.Label()
        Me.highLevelWindowLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.triggerParametersTextMsgLabel = New System.Windows.Forms.Label()
        Me.timeoutNumeric = New System.Windows.Forms.NumericUpDown()
        Me.verticalRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.verticalOffsetNumeric = New System.Windows.Forms.NumericUpDown()
        Me.probeAttenuationNumeric = New System.Windows.Forms.NumericUpDown()
        Me.minSampleRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.minRecordLengthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.numRecordsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.maxInputFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.referencePositionNumeric = New System.Windows.Forms.NumericUpDown()
        Me.triggerHoldoffNumeric = New System.Windows.Forms.NumericUpDown()
        Me.triggerDelayNumeric = New System.Windows.Forms.NumericUpDown()
        Me.triggerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.hysteresisNumeric = New System.Windows.Forms.NumericUpDown()
        Me.lowLevelWindowNumeric = New System.Windows.Forms.NumericUpDown()
        Me.highLevelWindowNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.actualSampleRateTextBox = New System.Windows.Forms.TextBox()
        Me.actualRecordLengthTextBox = New System.Windows.Forms.TextBox()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.acquisitionTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.generalGroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.verticalCouplingComboBox = New System.Windows.Forms.ComboBox()
        Me.decorationVerticalGroupBox = New System.Windows.Forms.GroupBox()
        Me.inputImpedanceComboBox = New System.Windows.Forms.ComboBox()
        Me.decorationInputGroupBox = New System.Windows.Forms.GroupBox()
        Me.triggerTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.decorationTriggeringGroupBox = New System.Windows.Forms.GroupBox()
        Me.commonParametersGroupBox = New System.Windows.Forms.GroupBox()
        Me.triggerSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerCouplingComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerSlopeComboBox = New System.Windows.Forms.ComboBox()
        Me.windowModeComboBox = New System.Windows.Forms.ComboBox()
        Me.decorationHorizontalGroupBox = New System.Windows.Forms.GroupBox()
        Me.enableTimeInterleavedSamplingCheckBox = New System.Windows.Forms.CheckBox()
        Me.enforceRealtimecheckBox = New System.Windows.Forms.CheckBox()
        Me.sampledDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.sampledDataGridView = New System.Windows.Forms.DataGridView()
        Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.RichTextBox()
        CType(Me.timeoutNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.verticalOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.probeAttenuationNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.minSampleRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.minRecordLengthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numRecordsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maxInputFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.referencePositionNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.triggerHoldoffNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.triggerDelayNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.hysteresisNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lowLevelWindowNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.highLevelWindowNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.generalGroupBox.SuspendLayout()
        Me.decorationVerticalGroupBox.SuspendLayout()
        Me.decorationInputGroupBox.SuspendLayout()
        Me.decorationTriggeringGroupBox.SuspendLayout()
        Me.commonParametersGroupBox.SuspendLayout()
        Me.decorationHorizontalGroupBox.SuspendLayout()
        Me.sampledDataGroupBox.SuspendLayout()
        CType(Me.sampledDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.buttonsGroupBox.SuspendLayout()
        Me.messageGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.Location = New System.Drawing.Point(6, 50)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(78, 13)
        Me.channelNameLabel.TabIndex = 2
        Me.channelNameLabel.Text = "Channel name:"
        '
        'timeoutLabel
        '
        Me.timeoutLabel.AutoSize = True
        Me.timeoutLabel.Location = New System.Drawing.Point(6, 76)
        Me.timeoutLabel.Name = "timeoutLabel"
        Me.timeoutLabel.Size = New System.Drawing.Size(62, 13)
        Me.timeoutLabel.TabIndex = 4
        Me.timeoutLabel.Text = "Timeout [s]:"
        '
        'acquisitionTypeLabel
        '
        Me.acquisitionTypeLabel.AutoSize = True
        Me.acquisitionTypeLabel.Location = New System.Drawing.Point(6, 102)
        Me.acquisitionTypeLabel.Name = "acquisitionTypeLabel"
        Me.acquisitionTypeLabel.Size = New System.Drawing.Size(84, 13)
        Me.acquisitionTypeLabel.TabIndex = 6
        Me.acquisitionTypeLabel.Text = "Acquisition type:"
        '
        'verticalRangeLabel
        '
        Me.verticalRangeLabel.AutoSize = True
        Me.verticalRangeLabel.Location = New System.Drawing.Point(6, 23)
        Me.verticalRangeLabel.Name = "verticalRangeLabel"
        Me.verticalRangeLabel.Size = New System.Drawing.Size(75, 13)
        Me.verticalRangeLabel.TabIndex = 0
        Me.verticalRangeLabel.Text = "Vertical range:"
        '
        'verticalOffsetLabel
        '
        Me.verticalOffsetLabel.AutoSize = True
        Me.verticalOffsetLabel.Location = New System.Drawing.Point(6, 49)
        Me.verticalOffsetLabel.Name = "verticalOffsetLabel"
        Me.verticalOffsetLabel.Size = New System.Drawing.Size(74, 13)
        Me.verticalOffsetLabel.TabIndex = 2
        Me.verticalOffsetLabel.Text = "Vertical offset:"
        '
        'probeAttenuationLabel
        '
        Me.probeAttenuationLabel.AutoSize = True
        Me.probeAttenuationLabel.Location = New System.Drawing.Point(6, 75)
        Me.probeAttenuationLabel.Name = "probeAttenuationLabel"
        Me.probeAttenuationLabel.Size = New System.Drawing.Size(94, 13)
        Me.probeAttenuationLabel.TabIndex = 4
        Me.probeAttenuationLabel.Text = "Probe attenuation:"
        '
        'verticalCouplingLabel
        '
        Me.verticalCouplingLabel.AutoSize = True
        Me.verticalCouplingLabel.Location = New System.Drawing.Point(6, 100)
        Me.verticalCouplingLabel.Name = "verticalCouplingLabel"
        Me.verticalCouplingLabel.Size = New System.Drawing.Size(88, 13)
        Me.verticalCouplingLabel.TabIndex = 6
        Me.verticalCouplingLabel.Text = "Vertical coupling:"
        '
        'minSampleRateLabel
        '
        Me.minSampleRateLabel.AutoSize = True
        Me.minSampleRateLabel.Location = New System.Drawing.Point(6, 69)
        Me.minSampleRateLabel.Name = "minSampleRateLabel"
        Me.minSampleRateLabel.Size = New System.Drawing.Size(84, 13)
        Me.minSampleRateLabel.TabIndex = 2
        Me.minSampleRateLabel.Text = "Min sample rate:"
        '
        'actualSampleRateLabel
        '
        Me.actualSampleRateLabel.AutoSize = True
        Me.actualSampleRateLabel.Location = New System.Drawing.Point(6, 97)
        Me.actualSampleRateLabel.Name = "actualSampleRateLabel"
        Me.actualSampleRateLabel.Size = New System.Drawing.Size(97, 13)
        Me.actualSampleRateLabel.TabIndex = 4
        Me.actualSampleRateLabel.Text = "Actual sample rate:"
        '
        'minRecordLengthLabel
        '
        Me.minRecordLengthLabel.AutoSize = True
        Me.minRecordLengthLabel.Location = New System.Drawing.Point(6, 123)
        Me.minRecordLengthLabel.Name = "minRecordLengthLabel"
        Me.minRecordLengthLabel.Size = New System.Drawing.Size(92, 13)
        Me.minRecordLengthLabel.TabIndex = 6
        Me.minRecordLengthLabel.Text = "Min record length:"
        '
        'actualRecordLengthLabel
        '
        Me.actualRecordLengthLabel.AutoSize = True
        Me.actualRecordLengthLabel.Location = New System.Drawing.Point(6, 149)
        Me.actualRecordLengthLabel.Name = "actualRecordLengthLabel"
        Me.actualRecordLengthLabel.Size = New System.Drawing.Size(105, 13)
        Me.actualRecordLengthLabel.TabIndex = 8
        Me.actualRecordLengthLabel.Text = "Actual record length:"
        '
        'numRecordsLabel
        '
        Me.numRecordsLabel.AutoSize = True
        Me.numRecordsLabel.Location = New System.Drawing.Point(6, 175)
        Me.numRecordsLabel.Name = "numRecordsLabel"
        Me.numRecordsLabel.Size = New System.Drawing.Size(70, 13)
        Me.numRecordsLabel.TabIndex = 10
        Me.numRecordsLabel.Text = "Num records:"
        '
        'maxInputFrequencyLabel
        '
        Me.maxInputFrequencyLabel.AutoSize = True
        Me.maxInputFrequencyLabel.Location = New System.Drawing.Point(6, 23)
        Me.maxInputFrequencyLabel.Name = "maxInputFrequencyLabel"
        Me.maxInputFrequencyLabel.Size = New System.Drawing.Size(106, 13)
        Me.maxInputFrequencyLabel.TabIndex = 0
        Me.maxInputFrequencyLabel.Text = "Max input frequency:"
        '
        'inputImpedanceLabel
        '
        Me.inputImpedanceLabel.AutoSize = True
        Me.inputImpedanceLabel.Location = New System.Drawing.Point(6, 49)
        Me.inputImpedanceLabel.Name = "inputImpedanceLabel"
        Me.inputImpedanceLabel.Size = New System.Drawing.Size(118, 13)
        Me.inputImpedanceLabel.TabIndex = 2
        Me.inputImpedanceLabel.Text = "Input impedance (ohm):"
        '
        'triggerTypeLabel
        '
        Me.triggerTypeLabel.AutoSize = True
        Me.triggerTypeLabel.Location = New System.Drawing.Point(6, 23)
        Me.triggerTypeLabel.Name = "triggerTypeLabel"
        Me.triggerTypeLabel.Size = New System.Drawing.Size(66, 13)
        Me.triggerTypeLabel.TabIndex = 0
        Me.triggerTypeLabel.Text = "Trigger type:"
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.AutoSize = True
        Me.triggerSourceLabel.Location = New System.Drawing.Point(6, 49)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(78, 13)
        Me.triggerSourceLabel.TabIndex = 2
        Me.triggerSourceLabel.Text = "Trigger source:"
        '
        'referencePositionLabel
        '
        Me.referencePositionLabel.AutoSize = True
        Me.referencePositionLabel.Location = New System.Drawing.Point(6, 23)
        Me.referencePositionLabel.Name = "referencePositionLabel"
        Me.referencePositionLabel.Size = New System.Drawing.Size(69, 13)
        Me.referencePositionLabel.TabIndex = 0
        Me.referencePositionLabel.Text = "Ref. position:"
        '
        'triggerHoldoffLabel
        '
        Me.triggerHoldoffLabel.AutoSize = True
        Me.triggerHoldoffLabel.Location = New System.Drawing.Point(6, 49)
        Me.triggerHoldoffLabel.Name = "triggerHoldoffLabel"
        Me.triggerHoldoffLabel.Size = New System.Drawing.Size(78, 13)
        Me.triggerHoldoffLabel.TabIndex = 2
        Me.triggerHoldoffLabel.Text = "Trigger holdoff:"
        '
        'triggerDelayLabel
        '
        Me.triggerDelayLabel.AutoSize = True
        Me.triggerDelayLabel.Location = New System.Drawing.Point(6, 75)
        Me.triggerDelayLabel.Name = "triggerDelayLabel"
        Me.triggerDelayLabel.Size = New System.Drawing.Size(71, 13)
        Me.triggerDelayLabel.TabIndex = 4
        Me.triggerDelayLabel.Text = "Trigger delay:"
        '
        'triggerLevelLabel
        '
        Me.triggerLevelLabel.AutoSize = True
        Me.triggerLevelLabel.Location = New System.Drawing.Point(6, 293)
        Me.triggerLevelLabel.Name = "triggerLevelLabel"
        Me.triggerLevelLabel.Size = New System.Drawing.Size(68, 13)
        Me.triggerLevelLabel.TabIndex = 6
        Me.triggerLevelLabel.Text = "Trigger level:"
        '
        'triggerCouplingLabel
        '
        Me.triggerCouplingLabel.AutoSize = True
        Me.triggerCouplingLabel.Location = New System.Drawing.Point(6, 320)
        Me.triggerCouplingLabel.Name = "triggerCouplingLabel"
        Me.triggerCouplingLabel.Size = New System.Drawing.Size(86, 13)
        Me.triggerCouplingLabel.TabIndex = 8
        Me.triggerCouplingLabel.Text = "Trigger coupling:"
        '
        'triggerSlopeLabel
        '
        Me.triggerSlopeLabel.AutoSize = True
        Me.triggerSlopeLabel.Location = New System.Drawing.Point(6, 347)
        Me.triggerSlopeLabel.Name = "triggerSlopeLabel"
        Me.triggerSlopeLabel.Size = New System.Drawing.Size(71, 13)
        Me.triggerSlopeLabel.TabIndex = 10
        Me.triggerSlopeLabel.Text = "Trigger slope:"
        '
        'hysteresisLabel
        '
        Me.hysteresisLabel.AutoSize = True
        Me.hysteresisLabel.Location = New System.Drawing.Point(6, 373)
        Me.hysteresisLabel.Name = "hysteresisLabel"
        Me.hysteresisLabel.Size = New System.Drawing.Size(58, 13)
        Me.hysteresisLabel.TabIndex = 12
        Me.hysteresisLabel.Text = "Hysteresis:"
        '
        'windowModeLabel
        '
        Me.windowModeLabel.AutoSize = True
        Me.windowModeLabel.Location = New System.Drawing.Point(6, 400)
        Me.windowModeLabel.Name = "windowModeLabel"
        Me.windowModeLabel.Size = New System.Drawing.Size(78, 13)
        Me.windowModeLabel.TabIndex = 14
        Me.windowModeLabel.Text = "Window mode:"
        '
        'lowLevelWindowLabel
        '
        Me.lowLevelWindowLabel.AutoSize = True
        Me.lowLevelWindowLabel.Location = New System.Drawing.Point(6, 426)
        Me.lowLevelWindowLabel.Name = "lowLevelWindowLabel"
        Me.lowLevelWindowLabel.Size = New System.Drawing.Size(94, 13)
        Me.lowLevelWindowLabel.TabIndex = 16
        Me.lowLevelWindowLabel.Text = "Low level window:"
        '
        'highLevelWindowLabel
        '
        Me.highLevelWindowLabel.AutoSize = True
        Me.highLevelWindowLabel.Location = New System.Drawing.Point(6, 452)
        Me.highLevelWindowLabel.Name = "highLevelWindowLabel"
        Me.highLevelWindowLabel.Size = New System.Drawing.Size(96, 13)
        Me.highLevelWindowLabel.TabIndex = 18
        Me.highLevelWindowLabel.Text = "High level window:"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 23)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 0
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'triggerParametersTextMsgLabel
        '
        Me.triggerParametersTextMsgLabel.AutoSize = True
        Me.triggerParametersTextMsgLabel.Location = New System.Drawing.Point(17, 251)
        Me.triggerParametersTextMsgLabel.Name = "triggerParametersTextMsgLabel"
        Me.triggerParametersTextMsgLabel.Size = New System.Drawing.Size(208, 26)
        Me.triggerParametersTextMsgLabel.TabIndex = 5
        Me.triggerParametersTextMsgLabel.Text = "Trigger Parameters - Not all parameters are" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "              used by all trigger ty" & _
            "pes"
        '
        'timeoutNumeric
        '
        Me.timeoutNumeric.DecimalPlaces = 2
        Me.timeoutNumeric.Increment = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.timeoutNumeric.Location = New System.Drawing.Point(130, 72)
        Me.timeoutNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.timeoutNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.timeoutNumeric.Name = "timeoutNumeric"
        Me.timeoutNumeric.Size = New System.Drawing.Size(100, 20)
        Me.timeoutNumeric.TabIndex = 5
        Me.timeoutNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'verticalRangeNumeric
        '
        Me.verticalRangeNumeric.DecimalPlaces = 2
        Me.verticalRangeNumeric.Location = New System.Drawing.Point(132, 19)
        Me.verticalRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.verticalRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.verticalRangeNumeric.Name = "verticalRangeNumeric"
        Me.verticalRangeNumeric.Size = New System.Drawing.Size(100, 20)
        Me.verticalRangeNumeric.TabIndex = 1
        Me.verticalRangeNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'verticalOffsetNumeric
        '
        Me.verticalOffsetNumeric.DecimalPlaces = 2
        Me.verticalOffsetNumeric.Location = New System.Drawing.Point(132, 45)
        Me.verticalOffsetNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.verticalOffsetNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.verticalOffsetNumeric.Name = "verticalOffsetNumeric"
        Me.verticalOffsetNumeric.Size = New System.Drawing.Size(100, 20)
        Me.verticalOffsetNumeric.TabIndex = 3
        '
        'probeAttenuationNumeric
        '
        Me.probeAttenuationNumeric.DecimalPlaces = 1
        Me.probeAttenuationNumeric.Location = New System.Drawing.Point(132, 71)
        Me.probeAttenuationNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.probeAttenuationNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.probeAttenuationNumeric.Name = "probeAttenuationNumeric"
        Me.probeAttenuationNumeric.Size = New System.Drawing.Size(100, 20)
        Me.probeAttenuationNumeric.TabIndex = 5
        Me.probeAttenuationNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'minSampleRateNumeric
        '
        Me.minSampleRateNumeric.DecimalPlaces = 2
        Me.minSampleRateNumeric.Location = New System.Drawing.Point(132, 65)
        Me.minSampleRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.minSampleRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.minSampleRateNumeric.Name = "minSampleRateNumeric"
        Me.minSampleRateNumeric.Size = New System.Drawing.Size(100, 20)
        Me.minSampleRateNumeric.TabIndex = 3
        Me.minSampleRateNumeric.Value = New Decimal(New Integer() {10000000, 0, 0, 0})
        '
        'minRecordLengthNumeric
        '
        Me.minRecordLengthNumeric.Location = New System.Drawing.Point(132, 119)
        Me.minRecordLengthNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.minRecordLengthNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.minRecordLengthNumeric.Name = "minRecordLengthNumeric"
        Me.minRecordLengthNumeric.Size = New System.Drawing.Size(100, 20)
        Me.minRecordLengthNumeric.TabIndex = 7
        Me.minRecordLengthNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'numRecordsNumeric
        '
        Me.numRecordsNumeric.Location = New System.Drawing.Point(132, 171)
        Me.numRecordsNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numRecordsNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.numRecordsNumeric.Name = "numRecordsNumeric"
        Me.numRecordsNumeric.Size = New System.Drawing.Size(100, 20)
        Me.numRecordsNumeric.TabIndex = 11
        Me.numRecordsNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'maxInputFrequencyNumeric
        '
        Me.maxInputFrequencyNumeric.DecimalPlaces = 2
        Me.maxInputFrequencyNumeric.Location = New System.Drawing.Point(130, 19)
        Me.maxInputFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.maxInputFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.maxInputFrequencyNumeric.Name = "maxInputFrequencyNumeric"
        Me.maxInputFrequencyNumeric.Size = New System.Drawing.Size(100, 20)
        Me.maxInputFrequencyNumeric.TabIndex = 1
        '
        'referencePositionNumeric
        '
        Me.referencePositionNumeric.DecimalPlaces = 2
        Me.referencePositionNumeric.Location = New System.Drawing.Point(122, 19)
        Me.referencePositionNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.referencePositionNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.referencePositionNumeric.Name = "referencePositionNumeric"
        Me.referencePositionNumeric.Size = New System.Drawing.Size(103, 20)
        Me.referencePositionNumeric.TabIndex = 1
        Me.referencePositionNumeric.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'triggerHoldoffNumeric
        '
        Me.triggerHoldoffNumeric.DecimalPlaces = 2
        Me.triggerHoldoffNumeric.Location = New System.Drawing.Point(122, 45)
        Me.triggerHoldoffNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.triggerHoldoffNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.triggerHoldoffNumeric.Name = "triggerHoldoffNumeric"
        Me.triggerHoldoffNumeric.Size = New System.Drawing.Size(103, 20)
        Me.triggerHoldoffNumeric.TabIndex = 3
        '
        'triggerDelayNumeric
        '
        Me.triggerDelayNumeric.DecimalPlaces = 2
        Me.triggerDelayNumeric.Location = New System.Drawing.Point(122, 71)
        Me.triggerDelayNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.triggerDelayNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.triggerDelayNumeric.Name = "triggerDelayNumeric"
        Me.triggerDelayNumeric.Size = New System.Drawing.Size(103, 20)
        Me.triggerDelayNumeric.TabIndex = 5
        '
        'triggerLevelNumeric
        '
        Me.triggerLevelNumeric.DecimalPlaces = 2
        Me.triggerLevelNumeric.Location = New System.Drawing.Point(134, 289)
        Me.triggerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.triggerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.triggerLevelNumeric.Name = "triggerLevelNumeric"
        Me.triggerLevelNumeric.Size = New System.Drawing.Size(103, 20)
        Me.triggerLevelNumeric.TabIndex = 7
        '
        'hysteresisNumeric
        '
        Me.hysteresisNumeric.DecimalPlaces = 2
        Me.hysteresisNumeric.Location = New System.Drawing.Point(134, 369)
        Me.hysteresisNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.hysteresisNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.hysteresisNumeric.Name = "hysteresisNumeric"
        Me.hysteresisNumeric.Size = New System.Drawing.Size(103, 20)
        Me.hysteresisNumeric.TabIndex = 13
        Me.hysteresisNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'lowLevelWindowNumeric
        '
        Me.lowLevelWindowNumeric.DecimalPlaces = 2
        Me.lowLevelWindowNumeric.Location = New System.Drawing.Point(134, 422)
        Me.lowLevelWindowNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.lowLevelWindowNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.lowLevelWindowNumeric.Name = "lowLevelWindowNumeric"
        Me.lowLevelWindowNumeric.Size = New System.Drawing.Size(103, 20)
        Me.lowLevelWindowNumeric.TabIndex = 17
        Me.lowLevelWindowNumeric.Value = New Decimal(New Integer() {1, 0, 0, -2147418112})
        '
        'highLevelWindowNumeric
        '
        Me.highLevelWindowNumeric.DecimalPlaces = 2
        Me.highLevelWindowNumeric.Location = New System.Drawing.Point(134, 448)
        Me.highLevelWindowNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.highLevelWindowNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.highLevelWindowNumeric.Name = "highLevelWindowNumeric"
        Me.highLevelWindowNumeric.Size = New System.Drawing.Size(103, 20)
        Me.highLevelWindowNumeric.TabIndex = 19
        Me.highLevelWindowNumeric.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(130, 46)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(100, 20)
        Me.channelNameTextBox.TabIndex = 3
        Me.channelNameTextBox.Text = "0"
        '
        'actualSampleRateTextBox
        '
        Me.actualSampleRateTextBox.Location = New System.Drawing.Point(132, 93)
        Me.actualSampleRateTextBox.Name = "actualSampleRateTextBox"
        Me.actualSampleRateTextBox.ReadOnly = True
        Me.actualSampleRateTextBox.Size = New System.Drawing.Size(100, 20)
        Me.actualSampleRateTextBox.TabIndex = 5
        Me.actualSampleRateTextBox.Text = "0"
        '
        'actualRecordLengthTextBox
        '
        Me.actualRecordLengthTextBox.Location = New System.Drawing.Point(132, 145)
        Me.actualRecordLengthTextBox.Name = "actualRecordLengthTextBox"
        Me.actualRecordLengthTextBox.ReadOnly = True
        Me.actualRecordLengthTextBox.Size = New System.Drawing.Size(100, 20)
        Me.actualRecordLengthTextBox.TabIndex = 9
        Me.actualRecordLengthTextBox.Text = "0"
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(27, 23)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(75, 23)
        Me.acquireButton.TabIndex = 0
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.Location = New System.Drawing.Point(108, 23)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'acquisitionTypeComboBox
        '
        Me.acquisitionTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.acquisitionTypeComboBox.Location = New System.Drawing.Point(130, 98)
        Me.acquisitionTypeComboBox.Name = "acquisitionTypeComboBox"
        Me.acquisitionTypeComboBox.Size = New System.Drawing.Size(100, 21)
        Me.acquisitionTypeComboBox.TabIndex = 7
        '
        'generalGroupBox
        '
        Me.generalGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.generalGroupBox.Controls.Add(Me.acquisitionTypeComboBox)
        Me.generalGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.generalGroupBox.Controls.Add(Me.timeoutNumeric)
        Me.generalGroupBox.Controls.Add(Me.channelNameLabel)
        Me.generalGroupBox.Controls.Add(Me.timeoutLabel)
        Me.generalGroupBox.Controls.Add(Me.acquisitionTypeLabel)
        Me.generalGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.generalGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.generalGroupBox.Name = "generalGroupBox"
        Me.generalGroupBox.Size = New System.Drawing.Size(239, 130)
        Me.generalGroupBox.TabIndex = 0
        Me.generalGroupBox.TabStop = False
        Me.generalGroupBox.Text = "General"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.Location = New System.Drawing.Point(130, 19)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(100, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'verticalCouplingComboBox
        '
        Me.verticalCouplingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.verticalCouplingComboBox.Location = New System.Drawing.Point(132, 96)
        Me.verticalCouplingComboBox.Name = "verticalCouplingComboBox"
        Me.verticalCouplingComboBox.Size = New System.Drawing.Size(100, 21)
        Me.verticalCouplingComboBox.TabIndex = 7
        '
        'decorationVerticalGroupBox
        '
        Me.decorationVerticalGroupBox.Controls.Add(Me.verticalCouplingComboBox)
        Me.decorationVerticalGroupBox.Controls.Add(Me.verticalRangeNumeric)
        Me.decorationVerticalGroupBox.Controls.Add(Me.verticalOffsetNumeric)
        Me.decorationVerticalGroupBox.Controls.Add(Me.probeAttenuationNumeric)
        Me.decorationVerticalGroupBox.Controls.Add(Me.verticalRangeLabel)
        Me.decorationVerticalGroupBox.Controls.Add(Me.verticalOffsetLabel)
        Me.decorationVerticalGroupBox.Controls.Add(Me.probeAttenuationLabel)
        Me.decorationVerticalGroupBox.Controls.Add(Me.verticalCouplingLabel)
        Me.decorationVerticalGroupBox.Location = New System.Drawing.Point(10, 153)
        Me.decorationVerticalGroupBox.Name = "decorationVerticalGroupBox"
        Me.decorationVerticalGroupBox.Size = New System.Drawing.Size(241, 125)
        Me.decorationVerticalGroupBox.TabIndex = 1
        Me.decorationVerticalGroupBox.TabStop = False
        Me.decorationVerticalGroupBox.Text = "Vertical"
        '
        'inputImpedanceComboBox
        '
        Me.inputImpedanceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.inputImpedanceComboBox.Location = New System.Drawing.Point(130, 45)
        Me.inputImpedanceComboBox.Name = "inputImpedanceComboBox"
        Me.inputImpedanceComboBox.Size = New System.Drawing.Size(100, 21)
        Me.inputImpedanceComboBox.TabIndex = 3
        '
        'decorationInputGroupBox
        '
        Me.decorationInputGroupBox.Controls.Add(Me.inputImpedanceComboBox)
        Me.decorationInputGroupBox.Controls.Add(Me.maxInputFrequencyNumeric)
        Me.decorationInputGroupBox.Controls.Add(Me.maxInputFrequencyLabel)
        Me.decorationInputGroupBox.Controls.Add(Me.inputImpedanceLabel)
        Me.decorationInputGroupBox.Location = New System.Drawing.Point(12, 501)
        Me.decorationInputGroupBox.Name = "decorationInputGroupBox"
        Me.decorationInputGroupBox.Size = New System.Drawing.Size(239, 75)
        Me.decorationInputGroupBox.TabIndex = 3
        Me.decorationInputGroupBox.TabStop = False
        Me.decorationInputGroupBox.Text = "Input"
        '
        'triggerTypeComboBox
        '
        Me.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerTypeComboBox.Location = New System.Drawing.Point(134, 19)
        Me.triggerTypeComboBox.Name = "triggerTypeComboBox"
        Me.triggerTypeComboBox.Size = New System.Drawing.Size(103, 21)
        Me.triggerTypeComboBox.TabIndex = 1
        '
        'decorationTriggeringGroupBox
        '
        Me.decorationTriggeringGroupBox.Controls.Add(Me.commonParametersGroupBox)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.triggerTypeComboBox)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.triggerSourceComboBox)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.triggerCouplingComboBox)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.triggerSlopeComboBox)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.windowModeComboBox)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.triggerLevelNumeric)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.hysteresisNumeric)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.lowLevelWindowNumeric)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.highLevelWindowNumeric)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.triggerTypeLabel)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.triggerSourceLabel)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.triggerLevelLabel)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.triggerCouplingLabel)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.triggerSlopeLabel)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.hysteresisLabel)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.windowModeLabel)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.lowLevelWindowLabel)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.highLevelWindowLabel)
        Me.decorationTriggeringGroupBox.Controls.Add(Me.triggerParametersTextMsgLabel)
        Me.decorationTriggeringGroupBox.Location = New System.Drawing.Point(257, 12)
        Me.decorationTriggeringGroupBox.Name = "decorationTriggeringGroupBox"
        Me.decorationTriggeringGroupBox.Size = New System.Drawing.Size(243, 478)
        Me.decorationTriggeringGroupBox.TabIndex = 4
        Me.decorationTriggeringGroupBox.TabStop = False
        Me.decorationTriggeringGroupBox.Text = "Triggering"
        '
        'commonParametersGroupBox
        '
        Me.commonParametersGroupBox.Controls.Add(Me.referencePositionLabel)
        Me.commonParametersGroupBox.Controls.Add(Me.triggerDelayLabel)
        Me.commonParametersGroupBox.Controls.Add(Me.triggerHoldoffLabel)
        Me.commonParametersGroupBox.Controls.Add(Me.triggerDelayNumeric)
        Me.commonParametersGroupBox.Controls.Add(Me.triggerHoldoffNumeric)
        Me.commonParametersGroupBox.Controls.Add(Me.referencePositionNumeric)
        Me.commonParametersGroupBox.Location = New System.Drawing.Point(6, 98)
        Me.commonParametersGroupBox.Name = "commonParametersGroupBox"
        Me.commonParametersGroupBox.Size = New System.Drawing.Size(231, 101)
        Me.commonParametersGroupBox.TabIndex = 4
        Me.commonParametersGroupBox.TabStop = False
        Me.commonParametersGroupBox.Text = "Common Parameters"
        '
        'triggerSourceComboBox
        '
        Me.triggerSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerSourceComboBox.Location = New System.Drawing.Point(134, 45)
        Me.triggerSourceComboBox.Name = "triggerSourceComboBox"
        Me.triggerSourceComboBox.Size = New System.Drawing.Size(103, 21)
        Me.triggerSourceComboBox.TabIndex = 3
        '
        'triggerCouplingComboBox
        '
        Me.triggerCouplingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerCouplingComboBox.Location = New System.Drawing.Point(134, 315)
        Me.triggerCouplingComboBox.Name = "triggerCouplingComboBox"
        Me.triggerCouplingComboBox.Size = New System.Drawing.Size(103, 21)
        Me.triggerCouplingComboBox.TabIndex = 9
        '
        'triggerSlopeComboBox
        '
        Me.triggerSlopeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerSlopeComboBox.Location = New System.Drawing.Point(134, 342)
        Me.triggerSlopeComboBox.Name = "triggerSlopeComboBox"
        Me.triggerSlopeComboBox.Size = New System.Drawing.Size(103, 21)
        Me.triggerSlopeComboBox.TabIndex = 11
        '
        'windowModeComboBox
        '
        Me.windowModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.windowModeComboBox.Location = New System.Drawing.Point(134, 395)
        Me.windowModeComboBox.Name = "windowModeComboBox"
        Me.windowModeComboBox.Size = New System.Drawing.Size(103, 21)
        Me.windowModeComboBox.TabIndex = 15
        '
        'decorationHorizontalGroupBox
        '
        Me.decorationHorizontalGroupBox.Controls.Add(Me.enableTimeInterleavedSamplingCheckBox)
        Me.decorationHorizontalGroupBox.Controls.Add(Me.enforceRealtimecheckBox)
        Me.decorationHorizontalGroupBox.Controls.Add(Me.actualSampleRateTextBox)
        Me.decorationHorizontalGroupBox.Controls.Add(Me.actualRecordLengthTextBox)
        Me.decorationHorizontalGroupBox.Controls.Add(Me.minSampleRateNumeric)
        Me.decorationHorizontalGroupBox.Controls.Add(Me.minRecordLengthNumeric)
        Me.decorationHorizontalGroupBox.Controls.Add(Me.numRecordsNumeric)
        Me.decorationHorizontalGroupBox.Controls.Add(Me.minSampleRateLabel)
        Me.decorationHorizontalGroupBox.Controls.Add(Me.actualSampleRateLabel)
        Me.decorationHorizontalGroupBox.Controls.Add(Me.minRecordLengthLabel)
        Me.decorationHorizontalGroupBox.Controls.Add(Me.actualRecordLengthLabel)
        Me.decorationHorizontalGroupBox.Controls.Add(Me.numRecordsLabel)
        Me.decorationHorizontalGroupBox.Location = New System.Drawing.Point(10, 289)
        Me.decorationHorizontalGroupBox.Name = "decorationHorizontalGroupBox"
        Me.decorationHorizontalGroupBox.Size = New System.Drawing.Size(241, 201)
        Me.decorationHorizontalGroupBox.TabIndex = 2
        Me.decorationHorizontalGroupBox.TabStop = False
        Me.decorationHorizontalGroupBox.Text = "Horizontal"
        '
        'enableTimeInterleavedSamplingCheckBox
        '
        Me.enableTimeInterleavedSamplingCheckBox.AutoSize = True
        Me.enableTimeInterleavedSamplingCheckBox.Location = New System.Drawing.Point(6, 42)
        Me.enableTimeInterleavedSamplingCheckBox.Name = "enableTimeInterleavedSamplingCheckBox"
        Me.enableTimeInterleavedSamplingCheckBox.Size = New System.Drawing.Size(187, 17)
        Me.enableTimeInterleavedSamplingCheckBox.TabIndex = 1
        Me.enableTimeInterleavedSamplingCheckBox.Text = "Enable Time Interleaved Sampling"
        Me.enableTimeInterleavedSamplingCheckBox.UseVisualStyleBackColor = True
        '
        'enforceRealtimecheckBox
        '
        Me.enforceRealtimecheckBox.AutoSize = True
        Me.enforceRealtimecheckBox.Checked = True
        Me.enforceRealtimecheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.enforceRealtimecheckBox.Location = New System.Drawing.Point(6, 19)
        Me.enforceRealtimecheckBox.Name = "enforceRealtimecheckBox"
        Me.enforceRealtimecheckBox.Size = New System.Drawing.Size(107, 17)
        Me.enforceRealtimecheckBox.TabIndex = 0
        Me.enforceRealtimecheckBox.Text = "Enforce Realtime"
        Me.enforceRealtimecheckBox.UseVisualStyleBackColor = True
        '
        'sampledDataGroupBox
        '
        Me.sampledDataGroupBox.Controls.Add(Me.sampledDataGridView)
        Me.sampledDataGroupBox.Location = New System.Drawing.Point(506, 12)
        Me.sampledDataGroupBox.Name = "sampledDataGroupBox"
        Me.sampledDataGroupBox.Size = New System.Drawing.Size(203, 478)
        Me.sampledDataGroupBox.TabIndex = 6
        Me.sampledDataGroupBox.TabStop = False
        Me.sampledDataGroupBox.Text = "Sampled Data"
        '
        'sampledDataGridView
        '
        Me.sampledDataGridView.AllowUserToAddRows = False
        Me.sampledDataGridView.AllowUserToDeleteRows = False
        Me.sampledDataGridView.AllowUserToResizeRows = False
        Me.sampledDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.sampledDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.sampledDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.sampledDataGridView.Name = "sampledDataGridView"
        Me.sampledDataGridView.ReadOnly = True
        Me.sampledDataGridView.RowHeadersVisible = False
        Me.sampledDataGridView.Size = New System.Drawing.Size(190, 452)
        Me.sampledDataGridView.StandardTab = True
        Me.sampledDataGridView.TabIndex = 0
        '
        'buttonsGroupBox
        '
        Me.buttonsGroupBox.Controls.Add(Me.stopButton)
        Me.buttonsGroupBox.Controls.Add(Me.acquireButton)
        Me.buttonsGroupBox.Location = New System.Drawing.Point(506, 501)
        Me.buttonsGroupBox.Name = "buttonsGroupBox"
        Me.buttonsGroupBox.Size = New System.Drawing.Size(203, 75)
        Me.buttonsGroupBox.TabIndex = 7
        Me.buttonsGroupBox.TabStop = False
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(257, 501)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(243, 75)
        Me.messageGroupBox.TabIndex = 5
        Me.messageGroupBox.TabStop = False
        Me.messageGroupBox.Text = "Message"
        '
        'messageTextBox
        '
        Me.messageTextBox.Location = New System.Drawing.Point(9, 19)
        Me.messageTextBox.Name = "messageTextBox"
        Me.messageTextBox.ReadOnly = True
        Me.messageTextBox.Size = New System.Drawing.Size(228, 47)
        Me.messageTextBox.TabIndex = 0
        Me.messageTextBox.Text = ""
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(724, 587)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.buttonsGroupBox)
        Me.Controls.Add(Me.sampledDataGroupBox)
        Me.Controls.Add(Me.decorationVerticalGroupBox)
        Me.Controls.Add(Me.decorationInputGroupBox)
        Me.Controls.Add(Me.decorationTriggeringGroupBox)
        Me.Controls.Add(Me.decorationHorizontalGroupBox)
        Me.Controls.Add(Me.generalGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Configured Acquisition"
        CType(Me.timeoutNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.verticalOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.probeAttenuationNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.minSampleRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.minRecordLengthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numRecordsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maxInputFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.referencePositionNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.triggerHoldoffNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.triggerDelayNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.hysteresisNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lowLevelWindowNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.highLevelWindowNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.generalGroupBox.ResumeLayout(False)
        Me.generalGroupBox.PerformLayout()
        Me.decorationVerticalGroupBox.ResumeLayout(False)
        Me.decorationVerticalGroupBox.PerformLayout()
        Me.decorationInputGroupBox.ResumeLayout(False)
        Me.decorationInputGroupBox.PerformLayout()
        Me.decorationTriggeringGroupBox.ResumeLayout(False)
        Me.decorationTriggeringGroupBox.PerformLayout()
        Me.commonParametersGroupBox.ResumeLayout(False)
        Me.commonParametersGroupBox.PerformLayout()
        Me.decorationHorizontalGroupBox.ResumeLayout(False)
        Me.decorationHorizontalGroupBox.PerformLayout()
        Me.sampledDataGroupBox.ResumeLayout(False)
        CType(Me.sampledDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.buttonsGroupBox.ResumeLayout(False)
        Me.messageGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private channelNameLabel As System.Windows.Forms.Label
    Private timeoutLabel As System.Windows.Forms.Label
    Private acquisitionTypeLabel As System.Windows.Forms.Label
    Private verticalRangeLabel As System.Windows.Forms.Label
    Private verticalOffsetLabel As System.Windows.Forms.Label
    Private probeAttenuationLabel As System.Windows.Forms.Label
    Private verticalCouplingLabel As System.Windows.Forms.Label
    Private minSampleRateLabel As System.Windows.Forms.Label
    Private actualSampleRateLabel As System.Windows.Forms.Label
    Private minRecordLengthLabel As System.Windows.Forms.Label
    Private actualRecordLengthLabel As System.Windows.Forms.Label
    Private numRecordsLabel As System.Windows.Forms.Label
    Private maxInputFrequencyLabel As System.Windows.Forms.Label
    Private inputImpedanceLabel As System.Windows.Forms.Label
    Private triggerTypeLabel As System.Windows.Forms.Label
    Private triggerSourceLabel As System.Windows.Forms.Label
    Private referencePositionLabel As System.Windows.Forms.Label
    Private triggerHoldoffLabel As System.Windows.Forms.Label
    Private triggerDelayLabel As System.Windows.Forms.Label
    Private triggerLevelLabel As System.Windows.Forms.Label
    Private triggerCouplingLabel As System.Windows.Forms.Label
    Private triggerSlopeLabel As System.Windows.Forms.Label
    Private hysteresisLabel As System.Windows.Forms.Label
    Private windowModeLabel As System.Windows.Forms.Label
    Private lowLevelWindowLabel As System.Windows.Forms.Label
    Private highLevelWindowLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private triggerParametersTextMsgLabel As System.Windows.Forms.Label
    Private timeoutNumeric As System.Windows.Forms.NumericUpDown
    Private verticalRangeNumeric As System.Windows.Forms.NumericUpDown
    Private verticalOffsetNumeric As System.Windows.Forms.NumericUpDown
    Private probeAttenuationNumeric As System.Windows.Forms.NumericUpDown
    Private minSampleRateNumeric As System.Windows.Forms.NumericUpDown
    Private minRecordLengthNumeric As System.Windows.Forms.NumericUpDown
    Private numRecordsNumeric As System.Windows.Forms.NumericUpDown
    Private maxInputFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private referencePositionNumeric As System.Windows.Forms.NumericUpDown
    Private triggerHoldoffNumeric As System.Windows.Forms.NumericUpDown
    Private triggerDelayNumeric As System.Windows.Forms.NumericUpDown
    Private triggerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private hysteresisNumeric As System.Windows.Forms.NumericUpDown
    Private lowLevelWindowNumeric As System.Windows.Forms.NumericUpDown
    Private highLevelWindowNumeric As System.Windows.Forms.NumericUpDown
    Private channelNameTextBox As System.Windows.Forms.TextBox
    Private actualSampleRateTextBox As System.Windows.Forms.TextBox
    Private actualRecordLengthTextBox As System.Windows.Forms.TextBox
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private acquisitionTypeComboBox As System.Windows.Forms.ComboBox
    Private verticalCouplingComboBox As System.Windows.Forms.ComboBox
    Private inputImpedanceComboBox As System.Windows.Forms.ComboBox
    Private triggerTypeComboBox As System.Windows.Forms.ComboBox
    Private triggerSourceComboBox As System.Windows.Forms.ComboBox
    Private triggerCouplingComboBox As System.Windows.Forms.ComboBox
    Private triggerSlopeComboBox As System.Windows.Forms.ComboBox
    Private windowModeComboBox As System.Windows.Forms.ComboBox
    Private decorationVerticalGroupBox As System.Windows.Forms.GroupBox
    Private decorationInputGroupBox As System.Windows.Forms.GroupBox
    Private decorationTriggeringGroupBox As System.Windows.Forms.GroupBox
    Private decorationHorizontalGroupBox As System.Windows.Forms.GroupBox
    Private generalGroupBox As System.Windows.Forms.GroupBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private enableTimeInterleavedSamplingCheckBox As System.Windows.Forms.CheckBox
    Private enforceRealtimecheckBox As System.Windows.Forms.CheckBox
    Private commonParametersGroupBox As System.Windows.Forms.GroupBox
    Private sampledDataGroupBox As System.Windows.Forms.GroupBox
    Private sampledDataGridView As System.Windows.Forms.DataGridView
    Private buttonsGroupBox As System.Windows.Forms.GroupBox
    Private messageGroupBox As System.Windows.Forms.GroupBox
    Private messageTextBox As System.Windows.Forms.RichTextBox


End Class
