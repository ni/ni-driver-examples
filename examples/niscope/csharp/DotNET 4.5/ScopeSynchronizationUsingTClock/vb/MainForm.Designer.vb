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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.resourceNameDevice1Label = New System.Windows.Forms.Label()
        Me.channelNameDevice1Label = New System.Windows.Forms.Label()
        Me.channelNameDevice1TextBox = New System.Windows.Forms.TextBox()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.scopeDevice1GroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameDevice1ComboBox = New System.Windows.Forms.ComboBox()
        Me.scopeDevice2GroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameDevice2ComboBox = New System.Windows.Forms.ComboBox()
        Me.channelNameDevice2Label = New System.Windows.Forms.Label()
        Me.resourceNameDevice2Label = New System.Windows.Forms.Label()
        Me.channelNameDevice2TextBox = New System.Windows.Forms.TextBox()
        Me.commonConfigurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.maximumInputFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.recordLengthMinNumeric = New System.Windows.Forms.NumericUpDown()
        Me.verticalRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.maximumInputFrequencyLabel = New System.Windows.Forms.Label()
        Me.recordLengthMinLabel = New System.Windows.Forms.Label()
        Me.sampleRateMinLabel = New System.Windows.Forms.Label()
        Me.verticalRangeLabel = New System.Windows.Forms.Label()
        Me.triggeringGroupBox = New System.Windows.Forms.GroupBox()
        Me.triggerTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.triggerTypeLabel = New System.Windows.Forms.Label()
        Me.triggerLevelLabel = New System.Windows.Forms.Label()
        Me.triggerSourceLabel = New System.Windows.Forms.Label()
        Me.triggerLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.triggerSourceComboBox = New System.Windows.Forms.ComboBox()
        Me.scope1GroupBox = New System.Windows.Forms.GroupBox()
        Me.scope1DataGridView = New System.Windows.Forms.DataGridView()
        Me.scope2GroupBox = New System.Windows.Forms.GroupBox()
        Me.scope2DataGridView = New System.Windows.Forms.DataGridView()
        Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
        Me.sampleRateMinNumeric = New System.Windows.Forms.NumericUpDown()
        Me.scopeDevice1GroupBox.SuspendLayout()
        Me.scopeDevice2GroupBox.SuspendLayout()
        Me.commonConfigurationGroupBox.SuspendLayout()
        CType(Me.maximumInputFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.triggeringGroupBox.SuspendLayout()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scope1GroupBox.SuspendLayout()
        CType(Me.scope1DataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scope2GroupBox.SuspendLayout()
        CType(Me.scope2DataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.buttonsGroupBox.SuspendLayout()
        CType(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'resourceNameDevice1Label
        '
        Me.resourceNameDevice1Label.AutoSize = True
        Me.resourceNameDevice1Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resourceNameDevice1Label.Location = New System.Drawing.Point(6, 23)
        Me.resourceNameDevice1Label.Name = "resourceNameDevice1Label"
        Me.resourceNameDevice1Label.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameDevice1Label.TabIndex = 0
        Me.resourceNameDevice1Label.Text = "Resource Name:"
        '
        'channelNameDevice1Label
        '
        Me.channelNameDevice1Label.AutoSize = True
        Me.channelNameDevice1Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelNameDevice1Label.Location = New System.Drawing.Point(6, 50)
        Me.channelNameDevice1Label.Name = "channelNameDevice1Label"
        Me.channelNameDevice1Label.Size = New System.Drawing.Size(80, 13)
        Me.channelNameDevice1Label.TabIndex = 2
        Me.channelNameDevice1Label.Text = "Channel Name:"
        '
        'channelNameDevice1TextBox
        '
        Me.channelNameDevice1TextBox.Location = New System.Drawing.Point(129, 46)
        Me.channelNameDevice1TextBox.Name = "channelNameDevice1TextBox"
        Me.channelNameDevice1TextBox.Size = New System.Drawing.Size(107, 20)
        Me.channelNameDevice1TextBox.TabIndex = 3
        Me.channelNameDevice1TextBox.Text = "0"
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(87, 19)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(75, 23)
        Me.acquireButton.TabIndex = 0
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'scopeDevice1GroupBox
        '
        Me.scopeDevice1GroupBox.Controls.Add(Me.resourceNameDevice1ComboBox)
        Me.scopeDevice1GroupBox.Controls.Add(Me.channelNameDevice1Label)
        Me.scopeDevice1GroupBox.Controls.Add(Me.resourceNameDevice1Label)
        Me.scopeDevice1GroupBox.Controls.Add(Me.channelNameDevice1TextBox)
        Me.scopeDevice1GroupBox.Location = New System.Drawing.Point(12, 12)
        Me.scopeDevice1GroupBox.Name = "scopeDevice1GroupBox"
        Me.scopeDevice1GroupBox.Size = New System.Drawing.Size(242, 75)
        Me.scopeDevice1GroupBox.TabIndex = 0
        Me.scopeDevice1GroupBox.TabStop = False
        Me.scopeDevice1GroupBox.Text = "Scope 1"
        '
        'resourceNameDevice1ComboBox
        '
        Me.resourceNameDevice1ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameDevice1ComboBox.FormattingEnabled = True
        Me.resourceNameDevice1ComboBox.Location = New System.Drawing.Point(129, 19)
        Me.resourceNameDevice1ComboBox.Name = "resourceNameDevice1ComboBox"
        Me.resourceNameDevice1ComboBox.Size = New System.Drawing.Size(107, 21)
        Me.resourceNameDevice1ComboBox.TabIndex = 1
        '
        'scopeDevice2GroupBox
        '
        Me.scopeDevice2GroupBox.Controls.Add(Me.resourceNameDevice2ComboBox)
        Me.scopeDevice2GroupBox.Controls.Add(Me.channelNameDevice2Label)
        Me.scopeDevice2GroupBox.Controls.Add(Me.resourceNameDevice2Label)
        Me.scopeDevice2GroupBox.Controls.Add(Me.channelNameDevice2TextBox)
        Me.scopeDevice2GroupBox.Location = New System.Drawing.Point(12, 98)
        Me.scopeDevice2GroupBox.Name = "scopeDevice2GroupBox"
        Me.scopeDevice2GroupBox.Size = New System.Drawing.Size(243, 73)
        Me.scopeDevice2GroupBox.TabIndex = 1
        Me.scopeDevice2GroupBox.TabStop = False
        Me.scopeDevice2GroupBox.Text = "Scope 2"
        '
        'resourceNameDevice2ComboBox
        '
        Me.resourceNameDevice2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameDevice2ComboBox.FormattingEnabled = True
        Me.resourceNameDevice2ComboBox.Location = New System.Drawing.Point(129, 19)
        Me.resourceNameDevice2ComboBox.Name = "resourceNameDevice2ComboBox"
        Me.resourceNameDevice2ComboBox.Size = New System.Drawing.Size(107, 21)
        Me.resourceNameDevice2ComboBox.TabIndex = 1
        '
        'channelNameDevice2Label
        '
        Me.channelNameDevice2Label.AutoSize = True
        Me.channelNameDevice2Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.channelNameDevice2Label.Location = New System.Drawing.Point(6, 50)
        Me.channelNameDevice2Label.Name = "channelNameDevice2Label"
        Me.channelNameDevice2Label.Size = New System.Drawing.Size(80, 13)
        Me.channelNameDevice2Label.TabIndex = 2
        Me.channelNameDevice2Label.Text = "Channel Name:"
        '
        'resourceNameDevice2Label
        '
        Me.resourceNameDevice2Label.AutoSize = True
        Me.resourceNameDevice2Label.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.resourceNameDevice2Label.Location = New System.Drawing.Point(6, 23)
        Me.resourceNameDevice2Label.Name = "resourceNameDevice2Label"
        Me.resourceNameDevice2Label.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameDevice2Label.TabIndex = 0
        Me.resourceNameDevice2Label.Text = "Resource Name:"
        '
        'channelNameDevice2TextBox
        '
        Me.channelNameDevice2TextBox.Location = New System.Drawing.Point(129, 46)
        Me.channelNameDevice2TextBox.Name = "channelNameDevice2TextBox"
        Me.channelNameDevice2TextBox.Size = New System.Drawing.Size(107, 20)
        Me.channelNameDevice2TextBox.TabIndex = 3
        Me.channelNameDevice2TextBox.Text = "0"
        '
        'commonConfigurationGroupBox
        '
        Me.commonConfigurationGroupBox.Controls.Add(Me.sampleRateMinNumeric)
        Me.commonConfigurationGroupBox.Controls.Add(Me.maximumInputFrequencyNumeric)
        Me.commonConfigurationGroupBox.Controls.Add(Me.recordLengthMinNumeric)
        Me.commonConfigurationGroupBox.Controls.Add(Me.verticalRangeNumeric)
        Me.commonConfigurationGroupBox.Controls.Add(Me.maximumInputFrequencyLabel)
        Me.commonConfigurationGroupBox.Controls.Add(Me.recordLengthMinLabel)
        Me.commonConfigurationGroupBox.Controls.Add(Me.sampleRateMinLabel)
        Me.commonConfigurationGroupBox.Controls.Add(Me.verticalRangeLabel)
        Me.commonConfigurationGroupBox.Location = New System.Drawing.Point(12, 182)
        Me.commonConfigurationGroupBox.Name = "commonConfigurationGroupBox"
        Me.commonConfigurationGroupBox.Size = New System.Drawing.Size(243, 124)
        Me.commonConfigurationGroupBox.TabIndex = 2
        Me.commonConfigurationGroupBox.TabStop = False
        Me.commonConfigurationGroupBox.Text = "Common Configuration"
        '
        'maximumInputFrequencyNumeric
        '
        Me.maximumInputFrequencyNumeric.Location = New System.Drawing.Point(129, 97)
        Me.maximumInputFrequencyNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.maximumInputFrequencyNumeric.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.maximumInputFrequencyNumeric.Name = "maximumInputFrequencyNumeric"
        Me.maximumInputFrequencyNumeric.Size = New System.Drawing.Size(107, 20)
        Me.maximumInputFrequencyNumeric.TabIndex = 7
        Me.maximumInputFrequencyNumeric.Value = New Decimal(New Integer() {1, 0, 0, -2147483648})
        '
        'recordLengthMinNumeric
        '
        Me.recordLengthMinNumeric.Location = New System.Drawing.Point(130, 71)
        Me.recordLengthMinNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.recordLengthMinNumeric.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.recordLengthMinNumeric.Name = "recordLengthMinNumeric"
        Me.recordLengthMinNumeric.Size = New System.Drawing.Size(107, 20)
        Me.recordLengthMinNumeric.TabIndex = 5
        Me.recordLengthMinNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'verticalRangeNumeric
        '
        Me.verticalRangeNumeric.Location = New System.Drawing.Point(130, 19)
        Me.verticalRangeNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.verticalRangeNumeric.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.verticalRangeNumeric.Name = "verticalRangeNumeric"
        Me.verticalRangeNumeric.Size = New System.Drawing.Size(107, 20)
        Me.verticalRangeNumeric.TabIndex = 1
        Me.verticalRangeNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'maximumInputFrequencyLabel
        '
        Me.maximumInputFrequencyLabel.AutoSize = True
        Me.maximumInputFrequencyLabel.Location = New System.Drawing.Point(6, 101)
        Me.maximumInputFrequencyLabel.Name = "maximumInputFrequencyLabel"
        Me.maximumInputFrequencyLabel.Size = New System.Drawing.Size(113, 13)
        Me.maximumInputFrequencyLabel.TabIndex = 6
        Me.maximumInputFrequencyLabel.Text = "Max. Input Frequency:"
        '
        'recordLengthMinLabel
        '
        Me.recordLengthMinLabel.AutoSize = True
        Me.recordLengthMinLabel.Location = New System.Drawing.Point(6, 75)
        Me.recordLengthMinLabel.Name = "recordLengthMinLabel"
        Me.recordLengthMinLabel.Size = New System.Drawing.Size(104, 13)
        Me.recordLengthMinLabel.TabIndex = 4
        Me.recordLengthMinLabel.Text = "Min. Record Length:"
        '
        'sampleRateMinLabel
        '
        Me.sampleRateMinLabel.AutoSize = True
        Me.sampleRateMinLabel.Location = New System.Drawing.Point(6, 49)
        Me.sampleRateMinLabel.Name = "sampleRateMinLabel"
        Me.sampleRateMinLabel.Size = New System.Drawing.Size(94, 13)
        Me.sampleRateMinLabel.TabIndex = 2
        Me.sampleRateMinLabel.Text = "Min. Sample Rate:"
        '
        'verticalRangeLabel
        '
        Me.verticalRangeLabel.AutoSize = True
        Me.verticalRangeLabel.Location = New System.Drawing.Point(6, 23)
        Me.verticalRangeLabel.Name = "verticalRangeLabel"
        Me.verticalRangeLabel.Size = New System.Drawing.Size(80, 13)
        Me.verticalRangeLabel.TabIndex = 0
        Me.verticalRangeLabel.Text = "Vertical Range:"
        '
        'triggeringGroupBox
        '
        Me.triggeringGroupBox.Controls.Add(Me.triggerTypeComboBox)
        Me.triggeringGroupBox.Controls.Add(Me.triggerTypeLabel)
        Me.triggeringGroupBox.Controls.Add(Me.triggerLevelLabel)
        Me.triggeringGroupBox.Controls.Add(Me.triggerSourceLabel)
        Me.triggeringGroupBox.Controls.Add(Me.triggerLevelNumeric)
        Me.triggeringGroupBox.Controls.Add(Me.triggerSourceComboBox)
        Me.triggeringGroupBox.Location = New System.Drawing.Point(12, 317)
        Me.triggeringGroupBox.Name = "triggeringGroupBox"
        Me.triggeringGroupBox.Size = New System.Drawing.Size(242, 100)
        Me.triggeringGroupBox.TabIndex = 3
        Me.triggeringGroupBox.TabStop = False
        Me.triggeringGroupBox.Text = "Triggering ( on Scope 1 )"
        '
        'triggerTypeComboBox
        '
        Me.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerTypeComboBox.FormattingEnabled = True
        Me.triggerTypeComboBox.Location = New System.Drawing.Point(130, 19)
        Me.triggerTypeComboBox.Name = "triggerTypeComboBox"
        Me.triggerTypeComboBox.Size = New System.Drawing.Size(107, 21)
        Me.triggerTypeComboBox.TabIndex = 1
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
        'triggerLevelLabel
        '
        Me.triggerLevelLabel.AutoSize = True
        Me.triggerLevelLabel.Location = New System.Drawing.Point(6, 77)
        Me.triggerLevelLabel.Name = "triggerLevelLabel"
        Me.triggerLevelLabel.Size = New System.Drawing.Size(72, 13)
        Me.triggerLevelLabel.TabIndex = 4
        Me.triggerLevelLabel.Text = "Trigger Level:"
        '
        'triggerSourceLabel
        '
        Me.triggerSourceLabel.AutoSize = True
        Me.triggerSourceLabel.Location = New System.Drawing.Point(6, 50)
        Me.triggerSourceLabel.Name = "triggerSourceLabel"
        Me.triggerSourceLabel.Size = New System.Drawing.Size(80, 13)
        Me.triggerSourceLabel.TabIndex = 2
        Me.triggerSourceLabel.Text = "Trigger Source:"
        '
        'triggerLevelNumeric
        '
        Me.triggerLevelNumeric.Location = New System.Drawing.Point(130, 73)
        Me.triggerLevelNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.triggerLevelNumeric.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.triggerLevelNumeric.Name = "triggerLevelNumeric"
        Me.triggerLevelNumeric.Size = New System.Drawing.Size(107, 20)
        Me.triggerLevelNumeric.TabIndex = 5
        '
        'triggerSourceComboBox
        '
        Me.triggerSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerSourceComboBox.FormattingEnabled = True
        Me.triggerSourceComboBox.Location = New System.Drawing.Point(130, 46)
        Me.triggerSourceComboBox.Name = "triggerSourceComboBox"
        Me.triggerSourceComboBox.Size = New System.Drawing.Size(107, 21)
        Me.triggerSourceComboBox.TabIndex = 3
        '
        'scope1GroupBox
        '
        Me.scope1GroupBox.Controls.Add(Me.scope1DataGridView)
        Me.scope1GroupBox.Location = New System.Drawing.Point(261, 12)
        Me.scope1GroupBox.Name = "scope1GroupBox"
        Me.scope1GroupBox.Size = New System.Drawing.Size(202, 473)
        Me.scope1GroupBox.TabIndex = 5
        Me.scope1GroupBox.TabStop = False
        Me.scope1GroupBox.Text = "Scope1"
        '
        'scope1DataGridView
        '
        Me.scope1DataGridView.AllowUserToAddRows = False
        Me.scope1DataGridView.AllowUserToDeleteRows = False
        Me.scope1DataGridView.AllowUserToResizeRows = False
        Me.scope1DataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.scope1DataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.scope1DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.scope1DataGridView.DefaultCellStyle = DataGridViewCellStyle2
        Me.scope1DataGridView.Location = New System.Drawing.Point(6, 19)
        Me.scope1DataGridView.Name = "scope1DataGridView"
        Me.scope1DataGridView.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.scope1DataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.scope1DataGridView.RowHeadersVisible = False
        Me.scope1DataGridView.RowHeadersWidth = 15
        Me.scope1DataGridView.RowTemplate.Height = 24
        Me.scope1DataGridView.Size = New System.Drawing.Size(190, 448)
        Me.scope1DataGridView.StandardTab = True
        Me.scope1DataGridView.TabIndex = 0
        '
        'scope2GroupBox
        '
        Me.scope2GroupBox.Controls.Add(Me.scope2DataGridView)
        Me.scope2GroupBox.Location = New System.Drawing.Point(469, 12)
        Me.scope2GroupBox.Name = "scope2GroupBox"
        Me.scope2GroupBox.Size = New System.Drawing.Size(202, 473)
        Me.scope2GroupBox.TabIndex = 6
        Me.scope2GroupBox.TabStop = False
        Me.scope2GroupBox.Text = "Scope2"
        '
        'scope2DataGridView
        '
        Me.scope2DataGridView.AllowUserToAddRows = False
        Me.scope2DataGridView.AllowUserToDeleteRows = False
        Me.scope2DataGridView.AllowUserToResizeRows = False
        Me.scope2DataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.scope2DataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.scope2DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.scope2DataGridView.DefaultCellStyle = DataGridViewCellStyle5
        Me.scope2DataGridView.Location = New System.Drawing.Point(6, 19)
        Me.scope2DataGridView.Name = "scope2DataGridView"
        Me.scope2DataGridView.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.scope2DataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.scope2DataGridView.RowHeadersVisible = False
        Me.scope2DataGridView.RowHeadersWidth = 15
        Me.scope2DataGridView.RowTemplate.Height = 24
        Me.scope2DataGridView.Size = New System.Drawing.Size(190, 448)
        Me.scope2DataGridView.StandardTab = True
        Me.scope2DataGridView.TabIndex = 0
        '
        'buttonsGroupBox
        '
        Me.buttonsGroupBox.Controls.Add(Me.acquireButton)
        Me.buttonsGroupBox.Location = New System.Drawing.Point(12, 428)
        Me.buttonsGroupBox.Name = "buttonsGroupBox"
        Me.buttonsGroupBox.Size = New System.Drawing.Size(242, 57)
        Me.buttonsGroupBox.TabIndex = 4
        Me.buttonsGroupBox.TabStop = False
        '
        'sampleRateMinNumeric
        '
        Me.sampleRateMinNumeric.DecimalPlaces = 2
        Me.sampleRateMinNumeric.Location = New System.Drawing.Point(130, 45)
        Me.sampleRateMinNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.sampleRateMinNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.sampleRateMinNumeric.Name = "sampleRateMinNumeric"
        Me.sampleRateMinNumeric.Size = New System.Drawing.Size(107, 20)
        Me.sampleRateMinNumeric.TabIndex = 3
        Me.sampleRateMinNumeric.Value = New Decimal(New Integer() {10000000, 0, 0, 0})
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(687, 498)
        Me.Controls.Add(Me.buttonsGroupBox)
        Me.Controls.Add(Me.scope2GroupBox)
        Me.Controls.Add(Me.scope1GroupBox)
        Me.Controls.Add(Me.triggeringGroupBox)
        Me.Controls.Add(Me.commonConfigurationGroupBox)
        Me.Controls.Add(Me.scopeDevice2GroupBox)
        Me.Controls.Add(Me.scopeDevice1GroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Scope Synchronization Using TClock"
        Me.scopeDevice1GroupBox.ResumeLayout(False)
        Me.scopeDevice1GroupBox.PerformLayout()
        Me.scopeDevice2GroupBox.ResumeLayout(False)
        Me.scopeDevice2GroupBox.PerformLayout()
        Me.commonConfigurationGroupBox.ResumeLayout(False)
        Me.commonConfigurationGroupBox.PerformLayout()
        CType(Me.maximumInputFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.triggeringGroupBox.ResumeLayout(False)
        Me.triggeringGroupBox.PerformLayout()
        CType(Me.triggerLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scope1GroupBox.ResumeLayout(False)
        CType(Me.scope1DataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scope2GroupBox.ResumeLayout(False)
        CType(Me.scope2DataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.buttonsGroupBox.ResumeLayout(False)
        CType(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private resourceNameDevice1Label As System.Windows.Forms.Label
    Private channelNameDevice1Label As System.Windows.Forms.Label
    Private channelNameDevice1TextBox As System.Windows.Forms.TextBox
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private scopeDevice1GroupBox As System.Windows.Forms.GroupBox
    Private scopeDevice2GroupBox As System.Windows.Forms.GroupBox
    Private channelNameDevice2Label As System.Windows.Forms.Label
    Private resourceNameDevice2Label As System.Windows.Forms.Label
    Private channelNameDevice2TextBox As System.Windows.Forms.TextBox
    Private commonConfigurationGroupBox As System.Windows.Forms.GroupBox
    Private recordLengthMinLabel As System.Windows.Forms.Label
    Private sampleRateMinLabel As System.Windows.Forms.Label
    Private verticalRangeLabel As System.Windows.Forms.Label
    Private triggeringGroupBox As System.Windows.Forms.GroupBox
    Private triggerLevelLabel As System.Windows.Forms.Label
    Private triggerSourceLabel As System.Windows.Forms.Label
    Private triggerLevelNumeric As System.Windows.Forms.NumericUpDown
    Private triggerSourceComboBox As System.Windows.Forms.ComboBox
    Private maximumInputFrequencyLabel As System.Windows.Forms.Label
    Private triggerTypeComboBox As System.Windows.Forms.ComboBox
    Private triggerTypeLabel As System.Windows.Forms.Label
    Private resourceNameDevice1ComboBox As System.Windows.Forms.ComboBox
    Private resourceNameDevice2ComboBox As System.Windows.Forms.ComboBox
    Private recordLengthMinNumeric As System.Windows.Forms.NumericUpDown
    Private verticalRangeNumeric As System.Windows.Forms.NumericUpDown
    Private maximumInputFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private scope1GroupBox As System.Windows.Forms.GroupBox
    Private scope1DataGridView As System.Windows.Forms.DataGridView
    Private scope2GroupBox As System.Windows.Forms.GroupBox
    Private scope2DataGridView As System.Windows.Forms.DataGridView
    Private buttonsGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents sampleRateMinNumeric As System.Windows.Forms.NumericUpDown
End Class

