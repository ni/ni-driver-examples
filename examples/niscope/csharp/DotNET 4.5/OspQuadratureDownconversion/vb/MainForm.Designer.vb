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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.timeOutLabel = New System.Windows.Forms.Label()
        Me.triggerTypeLabel = New System.Windows.Forms.Label()
        Me.inputImpedanceLabel = New System.Windows.Forms.Label()
        Me.verticalRangeLabel = New System.Windows.Forms.Label()
        Me.digitalGainLabel = New System.Windows.Forms.Label()
        Me.centerFrequencyLabel = New System.Windows.Forms.Label()
        Me.phaseILabel = New System.Windows.Forms.Label()
        Me.phaseQLabel = New System.Windows.Forms.Label()
        Me.triggerMinQuietTimeLabel = New System.Windows.Forms.Label()
        Me.triggerLevelLabel = New System.Windows.Forms.Label()
        Me.minSampleRateLabel = New System.Windows.Forms.Label()
        Me.actualSampleRateLabel = New System.Windows.Forms.Label()
        Me.minRecordLengthLabel = New System.Windows.Forms.Label()
        Me.actualRecordLengthLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.timeoutNumeric = New System.Windows.Forms.NumericUpDown()
        Me.verticalRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.digitalGainNumeric = New System.Windows.Forms.NumericUpDown()
        Me.centerFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.phaseINumeric = New System.Windows.Forms.NumericUpDown()
        Me.phaseQNumeric = New System.Windows.Forms.NumericUpDown()
        Me.triggerMinQuietTimeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.triggerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.sampleRateMinNumeric = New System.Windows.Forms.NumericUpDown()
        Me.recordLengthMinNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.actualSampleRateTextBox = New System.Windows.Forms.TextBox()
        Me.actualRecordLengthTextBox = New System.Windows.Forms.TextBox()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.triggerTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.basebandGroupBox = New System.Windows.Forms.GroupBox()
        Me.inputImpedanceComboBox = New System.Windows.Forms.ComboBox()
        Me.channelGroupBox = New System.Windows.Forms.GroupBox()
        Me.generalGroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.OSPGroupBox = New System.Windows.Forms.GroupBox()
        Me.horizontalConfigurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.fracResampleEnabledCheckBox = New System.Windows.Forms.CheckBox()
        Me.acquisitionDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.acquisitionDataGridView = New System.Windows.Forms.DataGridView()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.RichTextBox()
        Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.timeoutNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.digitalGainNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.centerFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.phaseINumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.phaseQNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.triggerMinQuietTimeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.basebandGroupBox.SuspendLayout()
        Me.channelGroupBox.SuspendLayout()
        Me.generalGroupBox.SuspendLayout()
        Me.OSPGroupBox.SuspendLayout()
        Me.horizontalConfigurationGroupBox.SuspendLayout()
        Me.acquisitionDataGroupBox.SuspendLayout()
        CType(Me.acquisitionDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.messageGroupBox.SuspendLayout()
        Me.buttonsGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'channelNameLabel
        '
        Me.channelNameLabel.AutoSize = True
        Me.channelNameLabel.Location = New System.Drawing.Point(6, 50)
        Me.channelNameLabel.Name = "channelNameLabel"
        Me.channelNameLabel.Size = New System.Drawing.Size(80, 13)
        Me.channelNameLabel.TabIndex = 2
        Me.channelNameLabel.Text = "Channel Name:"
        '
        'timeOutLabel
        '
        Me.timeOutLabel.AutoSize = True
        Me.timeOutLabel.Location = New System.Drawing.Point(6, 76)
        Me.timeOutLabel.Name = "timeOutLabel"
        Me.timeOutLabel.Size = New System.Drawing.Size(48, 13)
        Me.timeOutLabel.TabIndex = 4
        Me.timeOutLabel.Text = "Timeout:"
        '
        'triggerTypeLabel
        '
        Me.triggerTypeLabel.AutoSize = True
        Me.triggerTypeLabel.Location = New System.Drawing.Point(6, 23)
        Me.triggerTypeLabel.Name = "triggerTypeLabel"
        Me.triggerTypeLabel.Size = New System.Drawing.Size(70, 13)
        Me.triggerTypeLabel.TabIndex = 0
        Me.triggerTypeLabel.Text = "Trigger Type:"
        '
        'inputImpedanceLabel
        '
        Me.inputImpedanceLabel.AutoSize = True
        Me.inputImpedanceLabel.Location = New System.Drawing.Point(6, 23)
        Me.inputImpedanceLabel.Name = "inputImpedanceLabel"
        Me.inputImpedanceLabel.Size = New System.Drawing.Size(90, 13)
        Me.inputImpedanceLabel.TabIndex = 0
        Me.inputImpedanceLabel.Text = "Input Impedance:"
        '
        'verticalRangeLabel
        '
        Me.verticalRangeLabel.AutoSize = True
        Me.verticalRangeLabel.Location = New System.Drawing.Point(6, 50)
        Me.verticalRangeLabel.Name = "verticalRangeLabel"
        Me.verticalRangeLabel.Size = New System.Drawing.Size(80, 13)
        Me.verticalRangeLabel.TabIndex = 2
        Me.verticalRangeLabel.Text = "Vertical Range:"
        '
        'digitalGainLabel
        '
        Me.digitalGainLabel.AutoSize = True
        Me.digitalGainLabel.Location = New System.Drawing.Point(6, 76)
        Me.digitalGainLabel.Name = "digitalGainLabel"
        Me.digitalGainLabel.Size = New System.Drawing.Size(64, 13)
        Me.digitalGainLabel.TabIndex = 4
        Me.digitalGainLabel.Text = "Digital Gain:"
        '
        'centerFrequencyLabel
        '
        Me.centerFrequencyLabel.AutoSize = True
        Me.centerFrequencyLabel.Location = New System.Drawing.Point(6, 23)
        Me.centerFrequencyLabel.Name = "centerFrequencyLabel"
        Me.centerFrequencyLabel.Size = New System.Drawing.Size(94, 13)
        Me.centerFrequencyLabel.TabIndex = 0
        Me.centerFrequencyLabel.Text = "Center Frequency:"
        '
        'phaseILabel
        '
        Me.phaseILabel.AutoSize = True
        Me.phaseILabel.Location = New System.Drawing.Point(6, 49)
        Me.phaseILabel.Name = "phaseILabel"
        Me.phaseILabel.Size = New System.Drawing.Size(46, 13)
        Me.phaseILabel.TabIndex = 2
        Me.phaseILabel.Text = "Phase I:"
        '
        'phaseQLabel
        '
        Me.phaseQLabel.AutoSize = True
        Me.phaseQLabel.Location = New System.Drawing.Point(6, 75)
        Me.phaseQLabel.Name = "phaseQLabel"
        Me.phaseQLabel.Size = New System.Drawing.Size(51, 13)
        Me.phaseQLabel.TabIndex = 4
        Me.phaseQLabel.Text = "Phase Q:"
        '
        'triggerMinQuietTimeLabel
        '
        Me.triggerMinQuietTimeLabel.AutoSize = True
        Me.triggerMinQuietTimeLabel.Location = New System.Drawing.Point(6, 74)
        Me.triggerMinQuietTimeLabel.Name = "triggerMinQuietTimeLabel"
        Me.triggerMinQuietTimeLabel.Size = New System.Drawing.Size(117, 13)
        Me.triggerMinQuietTimeLabel.TabIndex = 4
        Me.triggerMinQuietTimeLabel.Text = "Trigger Min Quiet Time:"
        '
        'triggerLevelLabel
        '
        Me.triggerLevelLabel.AutoSize = True
        Me.triggerLevelLabel.Location = New System.Drawing.Point(6, 50)
        Me.triggerLevelLabel.Name = "triggerLevelLabel"
        Me.triggerLevelLabel.Size = New System.Drawing.Size(72, 13)
        Me.triggerLevelLabel.TabIndex = 2
        Me.triggerLevelLabel.Text = "Trigger Level:"
        '
        'minSampleRateLabel
        '
        Me.minSampleRateLabel.AutoSize = True
        Me.minSampleRateLabel.Location = New System.Drawing.Point(6, 23)
        Me.minSampleRateLabel.Name = "minSampleRateLabel"
        Me.minSampleRateLabel.Size = New System.Drawing.Size(94, 13)
        Me.minSampleRateLabel.TabIndex = 0
        Me.minSampleRateLabel.Text = "Min. Sample Rate:"
        '
        'actualSampleRateLabel
        '
        Me.actualSampleRateLabel.AutoSize = True
        Me.actualSampleRateLabel.Location = New System.Drawing.Point(6, 49)
        Me.actualSampleRateLabel.Name = "actualSampleRateLabel"
        Me.actualSampleRateLabel.Size = New System.Drawing.Size(104, 13)
        Me.actualSampleRateLabel.TabIndex = 2
        Me.actualSampleRateLabel.Text = "Actual Sample Rate:"
        '
        'minRecordLengthLabel
        '
        Me.minRecordLengthLabel.AutoSize = True
        Me.minRecordLengthLabel.Location = New System.Drawing.Point(6, 75)
        Me.minRecordLengthLabel.Name = "minRecordLengthLabel"
        Me.minRecordLengthLabel.Size = New System.Drawing.Size(104, 13)
        Me.minRecordLengthLabel.TabIndex = 4
        Me.minRecordLengthLabel.Text = "Min. Record Length:"
        '
        'actualRecordLengthLabel
        '
        Me.actualRecordLengthLabel.AutoSize = True
        Me.actualRecordLengthLabel.Location = New System.Drawing.Point(6, 101)
        Me.actualRecordLengthLabel.Name = "actualRecordLengthLabel"
        Me.actualRecordLengthLabel.Size = New System.Drawing.Size(114, 13)
        Me.actualRecordLengthLabel.TabIndex = 6
        Me.actualRecordLengthLabel.Text = "Actual Record Length:"
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
        'timeoutNumeric
        '
        Me.timeoutNumeric.DecimalPlaces = 2
        Me.timeoutNumeric.Location = New System.Drawing.Point(133, 72)
        Me.timeoutNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.timeoutNumeric.Name = "timeoutNumeric"
        Me.timeoutNumeric.Size = New System.Drawing.Size(96, 20)
        Me.timeoutNumeric.TabIndex = 5
        Me.timeoutNumeric.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'verticalRangeNumeric
        '
        Me.verticalRangeNumeric.DecimalPlaces = 2
        Me.verticalRangeNumeric.Location = New System.Drawing.Point(133, 46)
        Me.verticalRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.verticalRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.verticalRangeNumeric.Name = "verticalRangeNumeric"
        Me.verticalRangeNumeric.Size = New System.Drawing.Size(96, 20)
        Me.verticalRangeNumeric.TabIndex = 3
        Me.verticalRangeNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'digitalGainNumeric
        '
        Me.digitalGainNumeric.DecimalPlaces = 2
        Me.digitalGainNumeric.Location = New System.Drawing.Point(133, 72)
        Me.digitalGainNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.digitalGainNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.digitalGainNumeric.Name = "digitalGainNumeric"
        Me.digitalGainNumeric.Size = New System.Drawing.Size(96, 20)
        Me.digitalGainNumeric.TabIndex = 5
        Me.digitalGainNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'centerFrequencyNumeric
        '
        Me.centerFrequencyNumeric.DecimalPlaces = 2
        Me.centerFrequencyNumeric.Location = New System.Drawing.Point(133, 19)
        Me.centerFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.centerFrequencyNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.centerFrequencyNumeric.Name = "centerFrequencyNumeric"
        Me.centerFrequencyNumeric.Size = New System.Drawing.Size(96, 20)
        Me.centerFrequencyNumeric.TabIndex = 1
        Me.centerFrequencyNumeric.Value = New Decimal(New Integer() {10000000, 0, 0, 0})
        '
        'phaseINumeric
        '
        Me.phaseINumeric.DecimalPlaces = 2
        Me.phaseINumeric.Location = New System.Drawing.Point(133, 45)
        Me.phaseINumeric.Maximum = New Decimal(New Integer() {360, 0, 0, 0})
        Me.phaseINumeric.Minimum = New Decimal(New Integer() {360, 0, 0, -2147483648})
        Me.phaseINumeric.Name = "phaseINumeric"
        Me.phaseINumeric.Size = New System.Drawing.Size(96, 20)
        Me.phaseINumeric.TabIndex = 3
        '
        'phaseQNumeric
        '
        Me.phaseQNumeric.DecimalPlaces = 2
        Me.phaseQNumeric.Location = New System.Drawing.Point(133, 71)
        Me.phaseQNumeric.Maximum = New Decimal(New Integer() {360, 0, 0, 0})
        Me.phaseQNumeric.Minimum = New Decimal(New Integer() {360, 0, 0, -2147483648})
        Me.phaseQNumeric.Name = "phaseQNumeric"
        Me.phaseQNumeric.Size = New System.Drawing.Size(96, 20)
        Me.phaseQNumeric.TabIndex = 5
        Me.phaseQNumeric.Value = New Decimal(New Integer() {90, 0, 0, 0})
        '
        'triggerMinQuietTimeNumeric
        '
        Me.triggerMinQuietTimeNumeric.DecimalPlaces = 8
        Me.triggerMinQuietTimeNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 524288})
        Me.triggerMinQuietTimeNumeric.Location = New System.Drawing.Point(133, 72)
        Me.triggerMinQuietTimeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.triggerMinQuietTimeNumeric.Name = "triggerMinQuietTimeNumeric"
        Me.triggerMinQuietTimeNumeric.Size = New System.Drawing.Size(96, 20)
        Me.triggerMinQuietTimeNumeric.TabIndex = 5
        '
        'triggerLevelNumeric
        '
        Me.triggerLevelNumeric.DecimalPlaces = 2
        Me.triggerLevelNumeric.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.triggerLevelNumeric.Location = New System.Drawing.Point(133, 46)
        Me.triggerLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.triggerLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.triggerLevelNumeric.Name = "triggerLevelNumeric"
        Me.triggerLevelNumeric.Size = New System.Drawing.Size(96, 20)
        Me.triggerLevelNumeric.TabIndex = 3
        '
        'sampleRateMinNumeric
        '
        Me.sampleRateMinNumeric.DecimalPlaces = 2
        Me.sampleRateMinNumeric.Location = New System.Drawing.Point(116, 19)
        Me.sampleRateMinNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.sampleRateMinNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.sampleRateMinNumeric.Name = "sampleRateMinNumeric"
        Me.sampleRateMinNumeric.Size = New System.Drawing.Size(106, 20)
        Me.sampleRateMinNumeric.TabIndex = 1
        Me.sampleRateMinNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'recordLengthMinNumeric
        '
        Me.recordLengthMinNumeric.Location = New System.Drawing.Point(133, 71)
        Me.recordLengthMinNumeric.Maximum = New Decimal(New Integer() {-1, 0, 0, 0})
        Me.recordLengthMinNumeric.Name = "recordLengthMinNumeric"
        Me.recordLengthMinNumeric.Size = New System.Drawing.Size(97, 20)
        Me.recordLengthMinNumeric.TabIndex = 5
        Me.recordLengthMinNumeric.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(133, 46)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(96, 20)
        Me.channelNameTextBox.TabIndex = 3
        Me.channelNameTextBox.Text = "0"
        '
        'actualSampleRateTextBox
        '
        Me.actualSampleRateTextBox.Location = New System.Drawing.Point(116, 45)
        Me.actualSampleRateTextBox.Name = "actualSampleRateTextBox"
        Me.actualSampleRateTextBox.ReadOnly = True
        Me.actualSampleRateTextBox.Size = New System.Drawing.Size(114, 20)
        Me.actualSampleRateTextBox.TabIndex = 3
        '
        'actualRecordLengthTextBox
        '
        Me.actualRecordLengthTextBox.Location = New System.Drawing.Point(133, 97)
        Me.actualRecordLengthTextBox.Name = "actualRecordLengthTextBox"
        Me.actualRecordLengthTextBox.ReadOnly = True
        Me.actualRecordLengthTextBox.Size = New System.Drawing.Size(97, 20)
        Me.actualRecordLengthTextBox.TabIndex = 7
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(85, 19)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(75, 23)
        Me.acquireButton.TabIndex = 0
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.Location = New System.Drawing.Point(166, 19)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'triggerTypeComboBox
        '
        Me.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerTypeComboBox.Location = New System.Drawing.Point(133, 19)
        Me.triggerTypeComboBox.Name = "triggerTypeComboBox"
        Me.triggerTypeComboBox.Size = New System.Drawing.Size(96, 21)
        Me.triggerTypeComboBox.TabIndex = 1
        '
        'basebandGroupBox
        '
        Me.basebandGroupBox.Controls.Add(Me.triggerTypeComboBox)
        Me.basebandGroupBox.Controls.Add(Me.triggerMinQuietTimeNumeric)
        Me.basebandGroupBox.Controls.Add(Me.triggerLevelNumeric)
        Me.basebandGroupBox.Controls.Add(Me.triggerTypeLabel)
        Me.basebandGroupBox.Controls.Add(Me.triggerMinQuietTimeLabel)
        Me.basebandGroupBox.Controls.Add(Me.triggerLevelLabel)
        Me.basebandGroupBox.Location = New System.Drawing.Point(12, 513)
        Me.basebandGroupBox.Name = "basebandGroupBox"
        Me.basebandGroupBox.Size = New System.Drawing.Size(238, 101)
        Me.basebandGroupBox.TabIndex = 4
        Me.basebandGroupBox.TabStop = False
        Me.basebandGroupBox.Text = "Baseband Trigger"
        '
        'inputImpedanceComboBox
        '
        Me.inputImpedanceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.inputImpedanceComboBox.Location = New System.Drawing.Point(133, 19)
        Me.inputImpedanceComboBox.Name = "inputImpedanceComboBox"
        Me.inputImpedanceComboBox.Size = New System.Drawing.Size(96, 21)
        Me.inputImpedanceComboBox.TabIndex = 1
        '
        'channelGroupBox
        '
        Me.channelGroupBox.Controls.Add(Me.inputImpedanceComboBox)
        Me.channelGroupBox.Controls.Add(Me.verticalRangeNumeric)
        Me.channelGroupBox.Controls.Add(Me.digitalGainNumeric)
        Me.channelGroupBox.Controls.Add(Me.inputImpedanceLabel)
        Me.channelGroupBox.Controls.Add(Me.verticalRangeLabel)
        Me.channelGroupBox.Controls.Add(Me.digitalGainLabel)
        Me.channelGroupBox.Location = New System.Drawing.Point(12, 124)
        Me.channelGroupBox.Name = "channelGroupBox"
        Me.channelGroupBox.Size = New System.Drawing.Size(238, 110)
        Me.channelGroupBox.TabIndex = 1
        Me.channelGroupBox.TabStop = False
        Me.channelGroupBox.Text = "Channel Configuration"
        '
        'generalGroupBox
        '
        Me.generalGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.generalGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.generalGroupBox.Controls.Add(Me.timeoutNumeric)
        Me.generalGroupBox.Controls.Add(Me.channelNameLabel)
        Me.generalGroupBox.Controls.Add(Me.timeOutLabel)
        Me.generalGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.generalGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.generalGroupBox.Name = "generalGroupBox"
        Me.generalGroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.generalGroupBox.Size = New System.Drawing.Size(238, 101)
        Me.generalGroupBox.TabIndex = 0
        Me.generalGroupBox.TabStop = False
        Me.generalGroupBox.Text = "General Configuration"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(133, 19)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(96, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'OSPGroupBox
        '
        Me.OSPGroupBox.Controls.Add(Me.centerFrequencyNumeric)
        Me.OSPGroupBox.Controls.Add(Me.phaseINumeric)
        Me.OSPGroupBox.Controls.Add(Me.phaseQNumeric)
        Me.OSPGroupBox.Controls.Add(Me.centerFrequencyLabel)
        Me.OSPGroupBox.Controls.Add(Me.phaseILabel)
        Me.OSPGroupBox.Controls.Add(Me.phaseQLabel)
        Me.OSPGroupBox.Location = New System.Drawing.Point(12, 245)
        Me.OSPGroupBox.Name = "OSPGroupBox"
        Me.OSPGroupBox.Size = New System.Drawing.Size(238, 102)
        Me.OSPGroupBox.TabIndex = 2
        Me.OSPGroupBox.TabStop = False
        Me.OSPGroupBox.Text = "OSP Configuration"
        '
        'horizontalConfigurationGroupBox
        '
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.fracResampleEnabledCheckBox)
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.actualSampleRateTextBox)
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.actualRecordLengthTextBox)
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.sampleRateMinNumeric)
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.recordLengthMinNumeric)
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.minSampleRateLabel)
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.actualSampleRateLabel)
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.minRecordLengthLabel)
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.actualRecordLengthLabel)
        Me.horizontalConfigurationGroupBox.Location = New System.Drawing.Point(12, 358)
        Me.horizontalConfigurationGroupBox.Name = "horizontalConfigurationGroupBox"
        Me.horizontalConfigurationGroupBox.Size = New System.Drawing.Size(238, 144)
        Me.horizontalConfigurationGroupBox.TabIndex = 3
        Me.horizontalConfigurationGroupBox.TabStop = False
        Me.horizontalConfigurationGroupBox.Text = "Horizontal Configuration"
        '
        'fracResampleEnabledCheckBox
        '
        Me.fracResampleEnabledCheckBox.AutoSize = True
        Me.fracResampleEnabledCheckBox.Location = New System.Drawing.Point(6, 123)
        Me.fracResampleEnabledCheckBox.Name = "fracResampleEnabledCheckBox"
        Me.fracResampleEnabledCheckBox.Size = New System.Drawing.Size(173, 17)
        Me.fracResampleEnabledCheckBox.TabIndex = 8
        Me.fracResampleEnabledCheckBox.Text = "Fractional Resample Enabled ?"
        Me.fracResampleEnabledCheckBox.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.fracResampleEnabledCheckBox.UseVisualStyleBackColor = True
        '
        'acquisitionDataGroupBox
        '
        Me.acquisitionDataGroupBox.Controls.Add(Me.acquisitionDataGridView)
        Me.acquisitionDataGroupBox.Location = New System.Drawing.Point(256, 12)
        Me.acquisitionDataGroupBox.Name = "acquisitionDataGroupBox"
        Me.acquisitionDataGroupBox.Size = New System.Drawing.Size(327, 540)
        Me.acquisitionDataGroupBox.TabIndex = 6
        Me.acquisitionDataGroupBox.TabStop = False
        Me.acquisitionDataGroupBox.Text = "Acquisition Data"
        '
        'acquisitionDataGridView
        '
        Me.acquisitionDataGridView.AllowUserToAddRows = False
        Me.acquisitionDataGridView.AllowUserToDeleteRows = False
        Me.acquisitionDataGridView.AllowUserToResizeRows = False
        Me.acquisitionDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.acquisitionDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.acquisitionDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.acquisitionDataGridView.DefaultCellStyle = DataGridViewCellStyle2
        Me.acquisitionDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.acquisitionDataGridView.Name = "acquisitionDataGridView"
        Me.acquisitionDataGridView.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.acquisitionDataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.acquisitionDataGridView.RowHeadersVisible = False
        Me.acquisitionDataGridView.RowHeadersWidth = 15
        Me.acquisitionDataGridView.RowTemplate.Height = 24
        Me.acquisitionDataGridView.Size = New System.Drawing.Size(315, 515)
        Me.acquisitionDataGridView.StandardTab = True
        Me.acquisitionDataGridView.TabIndex = 0
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(12, 620)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(571, 48)
        Me.messageGroupBox.TabIndex = 5
        Me.messageGroupBox.TabStop = False
        Me.messageGroupBox.Text = "Message"
        '
        'messageTextBox
        '
        Me.messageTextBox.Location = New System.Drawing.Point(9, 19)
        Me.messageTextBox.Name = "messageTextBox"
        Me.messageTextBox.ReadOnly = True
        Me.messageTextBox.Size = New System.Drawing.Size(556, 22)
        Me.messageTextBox.TabIndex = 0
        Me.messageTextBox.Text = ""
        '
        'buttonsGroupBox
        '
        Me.buttonsGroupBox.Controls.Add(Me.acquireButton)
        Me.buttonsGroupBox.Controls.Add(Me.stopButton)
        Me.buttonsGroupBox.Location = New System.Drawing.Point(256, 558)
        Me.buttonsGroupBox.Name = "buttonsGroupBox"
        Me.buttonsGroupBox.Size = New System.Drawing.Size(327, 56)
        Me.buttonsGroupBox.TabIndex = 7
        Me.buttonsGroupBox.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(596, 680)
        Me.Controls.Add(Me.buttonsGroupBox)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.acquisitionDataGroupBox)
        Me.Controls.Add(Me.generalGroupBox)
        Me.Controls.Add(Me.OSPGroupBox)
        Me.Controls.Add(Me.channelGroupBox)
        Me.Controls.Add(Me.horizontalConfigurationGroupBox)
        Me.Controls.Add(Me.basebandGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "OSP Quadrature Downconversion"
        CType(Me.timeoutNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.digitalGainNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.centerFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.phaseINumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.phaseQNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.triggerMinQuietTimeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.basebandGroupBox.ResumeLayout(False)
        Me.basebandGroupBox.PerformLayout()
        Me.channelGroupBox.ResumeLayout(False)
        Me.channelGroupBox.PerformLayout()
        Me.generalGroupBox.ResumeLayout(False)
        Me.generalGroupBox.PerformLayout()
        Me.OSPGroupBox.ResumeLayout(False)
        Me.OSPGroupBox.PerformLayout()
        Me.horizontalConfigurationGroupBox.ResumeLayout(False)
        Me.horizontalConfigurationGroupBox.PerformLayout()
        Me.acquisitionDataGroupBox.ResumeLayout(False)
        CType(Me.acquisitionDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.messageGroupBox.ResumeLayout(False)
        Me.buttonsGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private channelNameLabel As System.Windows.Forms.Label
    Private timeOutLabel As System.Windows.Forms.Label
    Private triggerTypeLabel As System.Windows.Forms.Label
    Private inputImpedanceLabel As System.Windows.Forms.Label
    Private verticalRangeLabel As System.Windows.Forms.Label
    Private digitalGainLabel As System.Windows.Forms.Label
    Private centerFrequencyLabel As System.Windows.Forms.Label
    Private phaseILabel As System.Windows.Forms.Label
    Private phaseQLabel As System.Windows.Forms.Label
    Private triggerMinQuietTimeLabel As System.Windows.Forms.Label
    Private triggerLevelLabel As System.Windows.Forms.Label
    Private minSampleRateLabel As System.Windows.Forms.Label
    Private actualSampleRateLabel As System.Windows.Forms.Label
    Private minRecordLengthLabel As System.Windows.Forms.Label
    Private actualRecordLengthLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private timeoutNumeric As System.Windows.Forms.NumericUpDown
    Private verticalRangeNumeric As System.Windows.Forms.NumericUpDown
    Private digitalGainNumeric As System.Windows.Forms.NumericUpDown
    Private centerFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private phaseINumeric As System.Windows.Forms.NumericUpDown
    Private phaseQNumeric As System.Windows.Forms.NumericUpDown
    Private triggerMinQuietTimeNumeric As System.Windows.Forms.NumericUpDown
    Private triggerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private sampleRateMinNumeric As System.Windows.Forms.NumericUpDown
    Private recordLengthMinNumeric As System.Windows.Forms.NumericUpDown
    Private channelNameTextBox As System.Windows.Forms.TextBox
    Private actualSampleRateTextBox As System.Windows.Forms.TextBox
    Private actualRecordLengthTextBox As System.Windows.Forms.TextBox
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private triggerTypeComboBox As System.Windows.Forms.ComboBox
    Private inputImpedanceComboBox As System.Windows.Forms.ComboBox
    Private generalGroupBox As System.Windows.Forms.GroupBox
    Private OSPGroupBox As System.Windows.Forms.GroupBox
    Private channelGroupBox As System.Windows.Forms.GroupBox
    Private horizontalConfigurationGroupBox As System.Windows.Forms.GroupBox
    Private basebandGroupBox As System.Windows.Forms.GroupBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private acquisitionDataGroupBox As System.Windows.Forms.GroupBox
    Private acquisitionDataGridView As System.Windows.Forms.DataGridView
    Private fracResampleEnabledCheckBox As System.Windows.Forms.CheckBox
    Private messageGroupBox As System.Windows.Forms.GroupBox
    Private messageTextBox As System.Windows.Forms.RichTextBox
    Private buttonsGroupBox As System.Windows.Forms.GroupBox


End Class
