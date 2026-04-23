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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.triggerTypeLabel = New System.Windows.Forms.Label()
        Me.verticalRangeLabel = New System.Windows.Forms.Label()
        Me.resolutionTextMsgLabel = New System.Windows.Forms.Label()
        Me.actualSampleRateLabel = New System.Windows.Forms.Label()
        Me.sampleRateMinLabel = New System.Windows.Forms.Label()
        Me.recordLengthMinLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.channelInfoTextMsgLabel = New System.Windows.Forms.Label()
        Me.verticalRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.minSampleRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.minRecordLengthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.resolutionTextBox = New System.Windows.Forms.TextBox()
        Me.actualSampleRateTextBox = New System.Windows.Forms.TextBox()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.triggerTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.triggeringGroupBox = New System.Windows.Forms.GroupBox()
        Me.horizontalConfigurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.verticalConfigurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.sampledDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.sampledDataGridView = New System.Windows.Forms.DataGridView()
        Me.measurementDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.measurementDataGridView = New System.Windows.Forms.DataGridView()
        Me.generalGroupBox = New System.Windows.Forms.GroupBox()
        Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.RichTextBox()
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.minSampleRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.minRecordLengthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.triggeringGroupBox.SuspendLayout()
        Me.horizontalConfigurationGroupBox.SuspendLayout()
        Me.verticalConfigurationGroupBox.SuspendLayout()
        Me.sampledDataGroupBox.SuspendLayout()
        CType(Me.sampledDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.measurementDataGroupBox.SuspendLayout()
        CType(Me.measurementDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.generalGroupBox.SuspendLayout()
        Me.buttonsGroupBox.SuspendLayout()
        Me.messageGroupBox.SuspendLayout()
        Me.SuspendLayout()
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
        'verticalRangeLabel
        '
        Me.verticalRangeLabel.AutoSize = True
        Me.verticalRangeLabel.Location = New System.Drawing.Point(6, 23)
        Me.verticalRangeLabel.Name = "verticalRangeLabel"
        Me.verticalRangeLabel.Size = New System.Drawing.Size(75, 13)
        Me.verticalRangeLabel.TabIndex = 0
        Me.verticalRangeLabel.Text = "Vertical range:"
        '
        'resolutionTextMsgLabel
        '
        Me.resolutionTextMsgLabel.AutoSize = True
        Me.resolutionTextMsgLabel.Location = New System.Drawing.Point(6, 68)
        Me.resolutionTextMsgLabel.Name = "resolutionTextMsgLabel"
        Me.resolutionTextMsgLabel.Size = New System.Drawing.Size(101, 26)
        Me.resolutionTextMsgLabel.TabIndex = 4
        Me.resolutionTextMsgLabel.Text = "Advertised effective" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " number of bits:"
        '
        'actualSampleRateLabel
        '
        Me.actualSampleRateLabel.AutoSize = True
        Me.actualSampleRateLabel.Location = New System.Drawing.Point(6, 49)
        Me.actualSampleRateLabel.Name = "actualSampleRateLabel"
        Me.actualSampleRateLabel.Size = New System.Drawing.Size(97, 13)
        Me.actualSampleRateLabel.TabIndex = 2
        Me.actualSampleRateLabel.Text = "Actual sample rate:"
        '
        'sampleRateMinLabel
        '
        Me.sampleRateMinLabel.AutoSize = True
        Me.sampleRateMinLabel.Location = New System.Drawing.Point(6, 23)
        Me.sampleRateMinLabel.Name = "sampleRateMinLabel"
        Me.sampleRateMinLabel.Size = New System.Drawing.Size(84, 13)
        Me.sampleRateMinLabel.TabIndex = 0
        Me.sampleRateMinLabel.Text = "Min sample rate:"
        '
        'recordLengthMinLabel
        '
        Me.recordLengthMinLabel.AutoSize = True
        Me.recordLengthMinLabel.Location = New System.Drawing.Point(6, 101)
        Me.recordLengthMinLabel.Name = "recordLengthMinLabel"
        Me.recordLengthMinLabel.Size = New System.Drawing.Size(92, 13)
        Me.recordLengthMinLabel.TabIndex = 6
        Me.recordLengthMinLabel.Text = "Min record length:"
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
        'channelInfoTextMsgLabel
        '
        Me.channelInfoTextMsgLabel.AutoSize = True
        Me.channelInfoTextMsgLabel.Location = New System.Drawing.Point(33, 51)
        Me.channelInfoTextMsgLabel.Name = "channelInfoTextMsgLabel"
        Me.channelInfoTextMsgLabel.Size = New System.Drawing.Size(177, 13)
        Me.channelInfoTextMsgLabel.TabIndex = 2
        Me.channelInfoTextMsgLabel.Text = "Channel ""0"" is used for this example"
        '
        'verticalRangeNumeric
        '
        Me.verticalRangeNumeric.DecimalPlaces = 2
        Me.verticalRangeNumeric.Location = New System.Drawing.Point(138, 19)
        Me.verticalRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.verticalRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.verticalRangeNumeric.Name = "verticalRangeNumeric"
        Me.verticalRangeNumeric.Size = New System.Drawing.Size(97, 20)
        Me.verticalRangeNumeric.TabIndex = 0
        Me.verticalRangeNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'minSampleRateNumeric
        '
        Me.minSampleRateNumeric.DecimalPlaces = 2
        Me.minSampleRateNumeric.Location = New System.Drawing.Point(140, 19)
        Me.minSampleRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.minSampleRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.minSampleRateNumeric.Name = "minSampleRateNumeric"
        Me.minSampleRateNumeric.Size = New System.Drawing.Size(97, 20)
        Me.minSampleRateNumeric.TabIndex = 0
        Me.minSampleRateNumeric.Value = New Decimal(New Integer() {10000000, 0, 0, 0})
        '
        'minRecordLengthNumeric
        '
        Me.minRecordLengthNumeric.Location = New System.Drawing.Point(140, 97)
        Me.minRecordLengthNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.minRecordLengthNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.minRecordLengthNumeric.Name = "minRecordLengthNumeric"
        Me.minRecordLengthNumeric.Size = New System.Drawing.Size(97, 20)
        Me.minRecordLengthNumeric.TabIndex = 3
        Me.minRecordLengthNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'resolutionTextBox
        '
        Me.resolutionTextBox.Location = New System.Drawing.Point(140, 71)
        Me.resolutionTextBox.Name = "resolutionTextBox"
        Me.resolutionTextBox.ReadOnly = True
        Me.resolutionTextBox.Size = New System.Drawing.Size(97, 20)
        Me.resolutionTextBox.TabIndex = 2
        Me.resolutionTextBox.Text = "0"
        '
        'actualSampleRateTextBox
        '
        Me.actualSampleRateTextBox.Location = New System.Drawing.Point(140, 45)
        Me.actualSampleRateTextBox.Name = "actualSampleRateTextBox"
        Me.actualSampleRateTextBox.ReadOnly = True
        Me.actualSampleRateTextBox.Size = New System.Drawing.Size(97, 20)
        Me.actualSampleRateTextBox.TabIndex = 1
        Me.actualSampleRateTextBox.Text = "0"
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(121, 19)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(75, 23)
        Me.acquireButton.TabIndex = 0
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.Location = New System.Drawing.Point(202, 19)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'triggerTypeComboBox
        '
        Me.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.triggerTypeComboBox.Location = New System.Drawing.Point(140, 19)
        Me.triggerTypeComboBox.Name = "triggerTypeComboBox"
        Me.triggerTypeComboBox.Size = New System.Drawing.Size(97, 21)
        Me.triggerTypeComboBox.TabIndex = 0
        '
        'triggeringGroupBox
        '
        Me.triggeringGroupBox.Controls.Add(Me.triggerTypeComboBox)
        Me.triggeringGroupBox.Controls.Add(Me.triggerTypeLabel)
        Me.triggeringGroupBox.Location = New System.Drawing.Point(10, 95)
        Me.triggeringGroupBox.Name = "triggeringGroupBox"
        Me.triggeringGroupBox.Size = New System.Drawing.Size(243, 47)
        Me.triggeringGroupBox.TabIndex = 2
        Me.triggeringGroupBox.TabStop = False
        Me.triggeringGroupBox.Text = "Triggering"
        '
        'horizontalConfigurationGroupBox
        '
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.resolutionTextBox)
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.actualSampleRateTextBox)
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.minSampleRateNumeric)
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.minRecordLengthNumeric)
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.resolutionTextMsgLabel)
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.actualSampleRateLabel)
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.sampleRateMinLabel)
        Me.horizontalConfigurationGroupBox.Controls.Add(Me.recordLengthMinLabel)
        Me.horizontalConfigurationGroupBox.Location = New System.Drawing.Point(10, 210)
        Me.horizontalConfigurationGroupBox.Name = "horizontalConfigurationGroupBox"
        Me.horizontalConfigurationGroupBox.Size = New System.Drawing.Size(243, 125)
        Me.horizontalConfigurationGroupBox.TabIndex = 4
        Me.horizontalConfigurationGroupBox.TabStop = False
        Me.horizontalConfigurationGroupBox.Text = "Horizontal Configuration"
        '
        'verticalConfigurationGroupBox
        '
        Me.verticalConfigurationGroupBox.Controls.Add(Me.verticalRangeNumeric)
        Me.verticalConfigurationGroupBox.Controls.Add(Me.verticalRangeLabel)
        Me.verticalConfigurationGroupBox.Location = New System.Drawing.Point(12, 153)
        Me.verticalConfigurationGroupBox.Name = "verticalConfigurationGroupBox"
        Me.verticalConfigurationGroupBox.Size = New System.Drawing.Size(241, 46)
        Me.verticalConfigurationGroupBox.TabIndex = 3
        Me.verticalConfigurationGroupBox.TabStop = False
        Me.verticalConfigurationGroupBox.Text = "Vertical Configuration"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(138, 19)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(97, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'sampledDataGroupBox
        '
        Me.sampledDataGroupBox.Controls.Add(Me.sampledDataGridView)
        Me.sampledDataGroupBox.Location = New System.Drawing.Point(259, 12)
        Me.sampledDataGroupBox.Name = "sampledDataGroupBox"
        Me.sampledDataGroupBox.Size = New System.Drawing.Size(202, 323)
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
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.sampledDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.sampledDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.sampledDataGridView.DefaultCellStyle = DataGridViewCellStyle2
        Me.sampledDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.sampledDataGridView.Name = "sampledDataGridView"
        Me.sampledDataGridView.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.sampledDataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.sampledDataGridView.RowHeadersVisible = False
        Me.sampledDataGridView.RowHeadersWidth = 15
        Me.sampledDataGridView.RowTemplate.Height = 24
        Me.sampledDataGridView.Size = New System.Drawing.Size(190, 298)
        Me.sampledDataGridView.StandardTab = True
        Me.sampledDataGridView.TabIndex = 0
        '
        'measurementDataGroupBox
        '
        Me.measurementDataGroupBox.Controls.Add(Me.measurementDataGridView)
        Me.measurementDataGroupBox.Location = New System.Drawing.Point(467, 12)
        Me.measurementDataGroupBox.Name = "measurementDataGroupBox"
        Me.measurementDataGroupBox.Size = New System.Drawing.Size(191, 323)
        Me.measurementDataGroupBox.TabIndex = 7
        Me.measurementDataGroupBox.TabStop = False
        Me.measurementDataGroupBox.Text = "Measurement Data"
        '
        'measurementDataGridView
        '
        Me.measurementDataGridView.AllowUserToAddRows = False
        Me.measurementDataGridView.AllowUserToDeleteRows = False
        Me.measurementDataGridView.AllowUserToResizeRows = False
        Me.measurementDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.measurementDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.measurementDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.measurementDataGridView.DefaultCellStyle = DataGridViewCellStyle5
        Me.measurementDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.measurementDataGridView.Name = "measurementDataGridView"
        Me.measurementDataGridView.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.measurementDataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.measurementDataGridView.RowHeadersVisible = False
        Me.measurementDataGridView.RowHeadersWidth = 15
        Me.measurementDataGridView.RowTemplate.Height = 24
        Me.measurementDataGridView.Size = New System.Drawing.Size(179, 298)
        Me.measurementDataGridView.StandardTab = True
        Me.measurementDataGridView.TabIndex = 0
        '
        'generalGroupBox
        '
        Me.generalGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.generalGroupBox.Controls.Add(Me.channelInfoTextMsgLabel)
        Me.generalGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.generalGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.generalGroupBox.Name = "generalGroupBox"
        Me.generalGroupBox.Size = New System.Drawing.Size(241, 72)
        Me.generalGroupBox.TabIndex = 1
        Me.generalGroupBox.TabStop = False
        Me.generalGroupBox.Text = "General"
        '
        'buttonsGroupBox
        '
        Me.buttonsGroupBox.Controls.Add(Me.acquireButton)
        Me.buttonsGroupBox.Controls.Add(Me.stopButton)
        Me.buttonsGroupBox.Location = New System.Drawing.Point(265, 346)
        Me.buttonsGroupBox.Name = "buttonsGroupBox"
        Me.buttonsGroupBox.Size = New System.Drawing.Size(393, 57)
        Me.buttonsGroupBox.TabIndex = 7
        Me.buttonsGroupBox.TabStop = False
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(10, 346)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(243, 57)
        Me.messageGroupBox.TabIndex = 5
        Me.messageGroupBox.TabStop = False
        Me.messageGroupBox.Text = "Message"
        '
        'messageTextBox
        '
        Me.messageTextBox.Location = New System.Drawing.Point(9, 19)
        Me.messageTextBox.Name = "messageTextBox"
        Me.messageTextBox.ReadOnly = True
        Me.messageTextBox.Size = New System.Drawing.Size(228, 32)
        Me.messageTextBox.TabIndex = 0
        Me.messageTextBox.Text = ""
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(670, 416)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.buttonsGroupBox)
        Me.Controls.Add(Me.generalGroupBox)
        Me.Controls.Add(Me.measurementDataGroupBox)
        Me.Controls.Add(Me.sampledDataGroupBox)
        Me.Controls.Add(Me.triggeringGroupBox)
        Me.Controls.Add(Me.horizontalConfigurationGroupBox)
        Me.Controls.Add(Me.verticalConfigurationGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Flexible Resolution"
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.minSampleRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.minRecordLengthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.triggeringGroupBox.ResumeLayout(False)
        Me.triggeringGroupBox.PerformLayout()
        Me.horizontalConfigurationGroupBox.ResumeLayout(False)
        Me.horizontalConfigurationGroupBox.PerformLayout()
        Me.verticalConfigurationGroupBox.ResumeLayout(False)
        Me.verticalConfigurationGroupBox.PerformLayout()
        Me.sampledDataGroupBox.ResumeLayout(False)
        CType(Me.sampledDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.measurementDataGroupBox.ResumeLayout(False)
        CType(Me.measurementDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.generalGroupBox.ResumeLayout(False)
        Me.generalGroupBox.PerformLayout()
        Me.buttonsGroupBox.ResumeLayout(False)
        Me.messageGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
	#End Region

	Private triggerTypeLabel As System.Windows.Forms.Label
	Private verticalRangeLabel As System.Windows.Forms.Label
	Private resolutionTextMsgLabel As System.Windows.Forms.Label
	Private actualSampleRateLabel As System.Windows.Forms.Label
	Private sampleRateMinLabel As System.Windows.Forms.Label
	Private recordLengthMinLabel As System.Windows.Forms.Label
	Private resourceNameLabel As System.Windows.Forms.Label
	Private channelInfoTextMsgLabel As System.Windows.Forms.Label
	Private verticalRangeNumeric As System.Windows.Forms.NumericUpDown
	Private minSampleRateNumeric As System.Windows.Forms.NumericUpDown
	Private minRecordLengthNumeric As System.Windows.Forms.NumericUpDown
	Private resolutionTextBox As System.Windows.Forms.TextBox
	Private actualSampleRateTextBox As System.Windows.Forms.TextBox
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
	Private triggerTypeComboBox As System.Windows.Forms.ComboBox
	Private triggeringGroupBox As System.Windows.Forms.GroupBox
	Private horizontalConfigurationGroupBox As System.Windows.Forms.GroupBox
	Private verticalConfigurationGroupBox As System.Windows.Forms.GroupBox
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private sampledDataGroupBox As System.Windows.Forms.GroupBox
	Private sampledDataGridView As System.Windows.Forms.DataGridView
	Private measurementDataGroupBox As System.Windows.Forms.GroupBox
	Private measurementDataGridView As System.Windows.Forms.DataGridView
	Private generalGroupBox As System.Windows.Forms.GroupBox
	Private buttonsGroupBox As System.Windows.Forms.GroupBox
	Private messageGroupBox As System.Windows.Forms.GroupBox
	Private messageTextBox As System.Windows.Forms.RichTextBox


End Class
