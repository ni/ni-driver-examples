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
		Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
		Me.channelnameLabel = New System.Windows.Forms.Label()
		Me.verticalRangeLabel = New System.Windows.Forms.Label()
		Me.timebaseMultiplierLabel = New System.Windows.Forms.Label()
		Me.timebaseDivisorLabel = New System.Windows.Forms.Label()
		Me.timebaseRateLabel = New System.Windows.Forms.Label()
		Me.verticalOffsetLabel = New System.Windows.Forms.Label()
		Me.recordLengthMinLabel = New System.Windows.Forms.Label()
		Me.timebaseSourceLabel = New System.Windows.Forms.Label()
		Me.actualSampleRateLabel = New System.Windows.Forms.Label()
		Me.textmsgLabel = New System.Windows.Forms.Label()
		Me.textmsg3Label = New System.Windows.Forms.Label()
		Me.textmsg5Label = New System.Windows.Forms.Label()
		Me.resourceNameLabel = New System.Windows.Forms.Label()
		Me.verticalRangeNumeric = New System.Windows.Forms.NumericUpDown()
		Me.timebaseMultiplierNumeric = New System.Windows.Forms.NumericUpDown()
		Me.timebaseDivisorNumeric = New System.Windows.Forms.NumericUpDown()
		Me.timebaseRateNumeric = New System.Windows.Forms.NumericUpDown()
		Me.verticalOffsetNumeric = New System.Windows.Forms.NumericUpDown()
		Me.recordLengthMinNumeric = New System.Windows.Forms.NumericUpDown()
		Me.channelNameTextBox = New System.Windows.Forms.TextBox()
		Me.actualSampleRateTextBox = New System.Windows.Forms.TextBox()
		Me.acquireButton = New System.Windows.Forms.Button()
		Me.stopButton = New System.Windows.Forms.Button()
		Me.timebaseSourceComboBox = New System.Windows.Forms.ComboBox()
		Me.externalClockGroupBox = New System.Windows.Forms.GroupBox()
		Me.generalGroupBox = New System.Windows.Forms.GroupBox()
		Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
		Me.verticalAndHorizontalGroupBox = New System.Windows.Forms.GroupBox()
		Me.sampleDataGroupBox = New System.Windows.Forms.GroupBox()
		Me.scaledDataGridView = New System.Windows.Forms.DataGridView()
		Me.messageGroupBox = New System.Windows.Forms.GroupBox()
		Me.messageTextBox = New System.Windows.Forms.RichTextBox()
		Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
		DirectCast(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.timebaseMultiplierNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.timebaseDivisorNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.timebaseRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.verticalOffsetNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.externalClockGroupBox.SuspendLayout()
		Me.generalGroupBox.SuspendLayout()
		Me.verticalAndHorizontalGroupBox.SuspendLayout()
		Me.sampleDataGroupBox.SuspendLayout()
		DirectCast(Me.scaledDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.messageGroupBox.SuspendLayout()
		Me.buttonsGroupBox.SuspendLayout()
		Me.SuspendLayout()
		' 
		' channelnameLabel
		' 
		Me.channelnameLabel.AutoSize = True
		Me.channelnameLabel.Location = New System.Drawing.Point(6, 50)
		Me.channelnameLabel.Name = "channelnameLabel"
		Me.channelnameLabel.Size = New System.Drawing.Size(80, 13)
		Me.channelnameLabel.TabIndex = 3
		Me.channelnameLabel.Text = "Channel Name:"
		' 
		' verticalRangeLabel
		' 
		Me.verticalRangeLabel.AutoSize = True
		Me.verticalRangeLabel.Location = New System.Drawing.Point(6, 23)
		Me.verticalRangeLabel.Name = "verticalRangeLabel"
		Me.verticalRangeLabel.Size = New System.Drawing.Size(75, 13)
		Me.verticalRangeLabel.TabIndex = 2
		Me.verticalRangeLabel.Text = "Vertical range:"
		' 
		' timebaseMultiplierLabel
		' 
		Me.timebaseMultiplierLabel.AutoSize = True
		Me.timebaseMultiplierLabel.Location = New System.Drawing.Point(6, 102)
		Me.timebaseMultiplierLabel.Name = "timebaseMultiplierLabel"
		Me.timebaseMultiplierLabel.Size = New System.Drawing.Size(51, 13)
		Me.timebaseMultiplierLabel.TabIndex = 7
		Me.timebaseMultiplierLabel.Text = "Multiplier:"
		' 
		' timebaseDivisorLabel
		' 
		Me.timebaseDivisorLabel.AutoSize = True
		Me.timebaseDivisorLabel.Location = New System.Drawing.Point(6, 76)
		Me.timebaseDivisorLabel.Name = "timebaseDivisorLabel"
		Me.timebaseDivisorLabel.Size = New System.Drawing.Size(42, 13)
		Me.timebaseDivisorLabel.TabIndex = 5
		Me.timebaseDivisorLabel.Text = "Divisor:"
		' 
		' timebaseRateLabel
		' 
		Me.timebaseRateLabel.AutoSize = True
		Me.timebaseRateLabel.Location = New System.Drawing.Point(6, 50)
		Me.timebaseRateLabel.Name = "timebaseRateLabel"
		Me.timebaseRateLabel.Size = New System.Drawing.Size(33, 13)
		Me.timebaseRateLabel.TabIndex = 3
		Me.timebaseRateLabel.Text = "Rate:"
		' 
		' verticalOffsetLabel
		' 
		Me.verticalOffsetLabel.AutoSize = True
		Me.verticalOffsetLabel.Location = New System.Drawing.Point(6, 49)
		Me.verticalOffsetLabel.Name = "verticalOffsetLabel"
		Me.verticalOffsetLabel.Size = New System.Drawing.Size(74, 13)
		Me.verticalOffsetLabel.TabIndex = 3
		Me.verticalOffsetLabel.Text = "Vertical offset:"
		' 
		' recordLengthMinLabel
		' 
		Me.recordLengthMinLabel.AutoSize = True
		Me.recordLengthMinLabel.Location = New System.Drawing.Point(6, 75)
		Me.recordLengthMinLabel.Name = "recordLengthMinLabel"
		Me.recordLengthMinLabel.Size = New System.Drawing.Size(92, 13)
		Me.recordLengthMinLabel.TabIndex = 5
		Me.recordLengthMinLabel.Text = "Min record length:"
		' 
		' timebaseSourceLabel
		' 
		Me.timebaseSourceLabel.AutoSize = True
		Me.timebaseSourceLabel.Location = New System.Drawing.Point(6, 23)
		Me.timebaseSourceLabel.Name = "timebaseSourceLabel"
		Me.timebaseSourceLabel.Size = New System.Drawing.Size(44, 13)
		Me.timebaseSourceLabel.TabIndex = 1
		Me.timebaseSourceLabel.Text = "Source:"
		' 
		' actualSampleRateLabel
		' 
		Me.actualSampleRateLabel.AutoSize = True
		Me.actualSampleRateLabel.Location = New System.Drawing.Point(3, 23)
		Me.actualSampleRateLabel.Name = "actualSampleRateLabel"
		Me.actualSampleRateLabel.Size = New System.Drawing.Size(97, 13)
		Me.actualSampleRateLabel.TabIndex = 0
		Me.actualSampleRateLabel.Text = "Actual sample rate:"
		' 
		' textmsgLabel
		' 
		Me.textmsgLabel.AutoSize = True
		Me.textmsgLabel.Location = New System.Drawing.Point(11, 4)
		Me.textmsgLabel.Name = "textmsgLabel"
		Me.textmsgLabel.Size = New System.Drawing.Size(0, 13)
		Me.textmsgLabel.TabIndex = 0
		' 
		' textmsg3Label
		' 
		Me.textmsg3Label.AutoSize = True
		Me.textmsg3Label.Location = New System.Drawing.Point(14, 7)
		Me.textmsg3Label.Name = "textmsg3Label"
		Me.textmsg3Label.Size = New System.Drawing.Size(0, 13)
		Me.textmsg3Label.TabIndex = 0
		' 
		' textmsg5Label
		' 
		Me.textmsg5Label.AutoSize = True
		Me.textmsg5Label.Location = New System.Drawing.Point(12, 6)
		Me.textmsg5Label.Name = "textmsg5Label"
		Me.textmsg5Label.Size = New System.Drawing.Size(0, 13)
		Me.textmsg5Label.TabIndex = 0
		' 
		' resourceNameLabel
		' 
		Me.resourceNameLabel.AutoSize = True
		Me.resourceNameLabel.Location = New System.Drawing.Point(6, 23)
		Me.resourceNameLabel.Name = "resourceNameLabel"
		Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
		Me.resourceNameLabel.TabIndex = 1
		Me.resourceNameLabel.Text = "Resource Name:"
		' 
		' verticalRangeNumeric
		' 
		Me.verticalRangeNumeric.DecimalPlaces = 2
		Me.verticalRangeNumeric.Location = New System.Drawing.Point(137, 19)
		Me.verticalRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.verticalRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.verticalRangeNumeric.Name = "verticalRangeNumeric"
		Me.verticalRangeNumeric.Size = New System.Drawing.Size(95, 20)
		Me.verticalRangeNumeric.TabIndex = 1
		Me.verticalRangeNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
		' 
		' timebaseMultiplierNumeric
		' 
		Me.timebaseMultiplierNumeric.Location = New System.Drawing.Point(134, 98)
		Me.timebaseMultiplierNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.timebaseMultiplierNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.timebaseMultiplierNumeric.Name = "timebaseMultiplierNumeric"
		Me.timebaseMultiplierNumeric.Size = New System.Drawing.Size(95, 20)
		Me.timebaseMultiplierNumeric.TabIndex = 8
		Me.timebaseMultiplierNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
		' 
		' timebaseDivisorNumeric
		' 
		Me.timebaseDivisorNumeric.Location = New System.Drawing.Point(134, 72)
		Me.timebaseDivisorNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.timebaseDivisorNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.timebaseDivisorNumeric.Name = "timebaseDivisorNumeric"
		Me.timebaseDivisorNumeric.Size = New System.Drawing.Size(95, 20)
		Me.timebaseDivisorNumeric.TabIndex = 6
		Me.timebaseDivisorNumeric.Value = New Decimal(New Integer() {1, 0, 0, 0})
		' 
		' timebaseRateNumeric
		' 
		Me.timebaseRateNumeric.DecimalPlaces = 2
		Me.timebaseRateNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
		Me.timebaseRateNumeric.Location = New System.Drawing.Point(134, 46)
		Me.timebaseRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.timebaseRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.timebaseRateNumeric.Name = "timebaseRateNumeric"
		Me.timebaseRateNumeric.Size = New System.Drawing.Size(95, 20)
		Me.timebaseRateNumeric.TabIndex = 4
		Me.timebaseRateNumeric.Value = New Decimal(New Integer() {100000000, 0, 0, 0})
		' 
		' verticalOffsetNumeric
		' 
		Me.verticalOffsetNumeric.DecimalPlaces = 4
		Me.verticalOffsetNumeric.Location = New System.Drawing.Point(137, 45)
		Me.verticalOffsetNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.verticalOffsetNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.verticalOffsetNumeric.Name = "verticalOffsetNumeric"
		Me.verticalOffsetNumeric.Size = New System.Drawing.Size(95, 20)
		Me.verticalOffsetNumeric.TabIndex = 4
		' 
		' recordLengthMinNumeric
		' 
		Me.recordLengthMinNumeric.Location = New System.Drawing.Point(137, 71)
		Me.recordLengthMinNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
		Me.recordLengthMinNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
		Me.recordLengthMinNumeric.Name = "recordLengthMinNumeric"
		Me.recordLengthMinNumeric.Size = New System.Drawing.Size(95, 20)
		Me.recordLengthMinNumeric.TabIndex = 6
		Me.recordLengthMinNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
		' 
		' channelNameTextBox
		' 
		Me.channelNameTextBox.Location = New System.Drawing.Point(137, 46)
		Me.channelNameTextBox.Name = "channelNameTextBox"
		Me.channelNameTextBox.Size = New System.Drawing.Size(95, 20)
		Me.channelNameTextBox.TabIndex = 4
		Me.channelNameTextBox.Text = "0"
		' 
		' actualSampleRateTextBox
		' 
		Me.actualSampleRateTextBox.Location = New System.Drawing.Point(106, 19)
		Me.actualSampleRateTextBox.Name = "actualSampleRateTextBox"
		Me.actualSampleRateTextBox.[ReadOnly] = True
		Me.actualSampleRateTextBox.Size = New System.Drawing.Size(90, 20)
		Me.actualSampleRateTextBox.TabIndex = 1
		Me.actualSampleRateTextBox.Text = "0.00"
		' 
		' acquireButton
		' 
		Me.acquireButton.Location = New System.Drawing.Point(19, 19)
		Me.acquireButton.Name = "acquireButton"
		Me.acquireButton.Size = New System.Drawing.Size(75, 23)
		Me.acquireButton.TabIndex = 0
		Me.acquireButton.Text = "&Acquire"
		Me.acquireButton.UseVisualStyleBackColor = True
		AddHandler Me.acquireButton.Click, New System.EventHandler(AddressOf Me.acquireButton_Click)
		' 
		' stopButton
		' 
		Me.stopButton.Location = New System.Drawing.Point(100, 19)
		Me.stopButton.Name = "stopButton"
		Me.stopButton.Size = New System.Drawing.Size(75, 23)
		Me.stopButton.TabIndex = 1
		Me.stopButton.Text = "&Stop"
		Me.stopButton.UseVisualStyleBackColor = True
		AddHandler Me.stopButton.Click, New System.EventHandler(AddressOf Me.stopButton_Click)
		' 
		' timebaseSourceComboBox
		' 
		Me.timebaseSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.timebaseSourceComboBox.Location = New System.Drawing.Point(134, 19)
		Me.timebaseSourceComboBox.Name = "timebaseSourceComboBox"
		Me.timebaseSourceComboBox.Size = New System.Drawing.Size(95, 21)
		Me.timebaseSourceComboBox.TabIndex = 2
		' 
		' externalClockGroupBox
		' 
		Me.externalClockGroupBox.Controls.Add(Me.timebaseSourceComboBox)
		Me.externalClockGroupBox.Controls.Add(Me.timebaseMultiplierNumeric)
		Me.externalClockGroupBox.Controls.Add(Me.timebaseDivisorNumeric)
		Me.externalClockGroupBox.Controls.Add(Me.timebaseRateNumeric)
		Me.externalClockGroupBox.Controls.Add(Me.timebaseMultiplierLabel)
		Me.externalClockGroupBox.Controls.Add(Me.timebaseDivisorLabel)
		Me.externalClockGroupBox.Controls.Add(Me.timebaseRateLabel)
		Me.externalClockGroupBox.Controls.Add(Me.timebaseSourceLabel)
		Me.externalClockGroupBox.Controls.Add(Me.textmsg5Label)
		Me.externalClockGroupBox.Location = New System.Drawing.Point(15, 212)
		Me.externalClockGroupBox.Name = "externalClockGroupBox"
		Me.externalClockGroupBox.Size = New System.Drawing.Size(238, 126)
		Me.externalClockGroupBox.TabIndex = 2
		Me.externalClockGroupBox.TabStop = False
		Me.externalClockGroupBox.Text = "External Clock (Sample Clock Timebase)"
		' 
		' generalGroupBox
		' 
		Me.generalGroupBox.Controls.Add(Me.resourceNameComboBox)
		Me.generalGroupBox.Controls.Add(Me.channelNameTextBox)
		Me.generalGroupBox.Controls.Add(Me.channelnameLabel)
		Me.generalGroupBox.Controls.Add(Me.textmsgLabel)
		Me.generalGroupBox.Controls.Add(Me.resourceNameLabel)
		Me.generalGroupBox.Location = New System.Drawing.Point(12, 12)
		Me.generalGroupBox.Name = "generalGroupBox"
		Me.generalGroupBox.Size = New System.Drawing.Size(238, 73)
		Me.generalGroupBox.TabIndex = 0
		Me.generalGroupBox.TabStop = False
		Me.generalGroupBox.Text = "General"
		' 
		' resourceNameComboBox
		' 
		Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.resourceNameComboBox.FormattingEnabled = True
		Me.resourceNameComboBox.Location = New System.Drawing.Point(137, 19)
		Me.resourceNameComboBox.Name = "resourceNameComboBox"
		Me.resourceNameComboBox.Size = New System.Drawing.Size(95, 21)
		Me.resourceNameComboBox.TabIndex = 2
		' 
		' verticalAndHorizontalGroupBox
		' 
		Me.verticalAndHorizontalGroupBox.Controls.Add(Me.verticalRangeNumeric)
		Me.verticalAndHorizontalGroupBox.Controls.Add(Me.verticalOffsetNumeric)
		Me.verticalAndHorizontalGroupBox.Controls.Add(Me.recordLengthMinNumeric)
		Me.verticalAndHorizontalGroupBox.Controls.Add(Me.verticalRangeLabel)
		Me.verticalAndHorizontalGroupBox.Controls.Add(Me.verticalOffsetLabel)
		Me.verticalAndHorizontalGroupBox.Controls.Add(Me.recordLengthMinLabel)
		Me.verticalAndHorizontalGroupBox.Controls.Add(Me.textmsg3Label)
		Me.verticalAndHorizontalGroupBox.Location = New System.Drawing.Point(12, 96)
		Me.verticalAndHorizontalGroupBox.Name = "verticalAndHorizontalGroupBox"
		Me.verticalAndHorizontalGroupBox.Size = New System.Drawing.Size(238, 100)
		Me.verticalAndHorizontalGroupBox.TabIndex = 1
		Me.verticalAndHorizontalGroupBox.TabStop = False
		Me.verticalAndHorizontalGroupBox.Text = "Vertical and Horizontal"
		' 
		' sampleDataGroupBox
		' 
		Me.sampleDataGroupBox.Controls.Add(Me.scaledDataGridView)
		Me.sampleDataGroupBox.Controls.Add(Me.actualSampleRateLabel)
		Me.sampleDataGroupBox.Controls.Add(Me.actualSampleRateTextBox)
		Me.sampleDataGroupBox.Location = New System.Drawing.Point(259, 12)
		Me.sampleDataGroupBox.Name = "sampleDataGroupBox"
		Me.sampleDataGroupBox.Size = New System.Drawing.Size(202, 363)
		Me.sampleDataGroupBox.TabIndex = 4
		Me.sampleDataGroupBox.TabStop = False
		Me.sampleDataGroupBox.Text = "Scaled Waveform Data"
		' 
		' scaledDataGridView
		' 
		Me.scaledDataGridView.AllowUserToAddRows = False
		Me.scaledDataGridView.AllowUserToDeleteRows = False
		Me.scaledDataGridView.AllowUserToResizeRows = False
		Me.scaledDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.scaledDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.scaledDataGridView.Location = New System.Drawing.Point(6, 46)
		Me.scaledDataGridView.Name = "scaledDataGridView"
		Me.scaledDataGridView.[ReadOnly] = True
		Me.scaledDataGridView.RowHeadersVisible = False
		Me.scaledDataGridView.RowHeadersWidth = 15
		Me.scaledDataGridView.RowTemplate.Height = 24
		Me.scaledDataGridView.Size = New System.Drawing.Size(190, 311)
		Me.scaledDataGridView.StandardTab = True
		Me.scaledDataGridView.TabIndex = 2
		' 
		' messageGroupBox
		' 
		Me.messageGroupBox.Controls.Add(Me.messageTextBox)
		Me.messageGroupBox.Location = New System.Drawing.Point(15, 349)
		Me.messageGroupBox.Name = "messageGroupBox"
		Me.messageGroupBox.Size = New System.Drawing.Size(238, 89)
		Me.messageGroupBox.TabIndex = 3
		Me.messageGroupBox.TabStop = False
		Me.messageGroupBox.Text = "Message"
		' 
		' messageTextBox
		' 
		Me.messageTextBox.Location = New System.Drawing.Point(9, 19)
		Me.messageTextBox.Name = "messageTextBox"
		Me.messageTextBox.[ReadOnly] = True
		Me.messageTextBox.Size = New System.Drawing.Size(223, 64)
		Me.messageTextBox.TabIndex = 0
		Me.messageTextBox.Text = ""
		' 
		' buttonsGroupBox
		' 
		Me.buttonsGroupBox.Controls.Add(Me.acquireButton)
		Me.buttonsGroupBox.Controls.Add(Me.stopButton)
		Me.buttonsGroupBox.Location = New System.Drawing.Point(265, 381)
		Me.buttonsGroupBox.Name = "buttonsGroupBox"
		Me.buttonsGroupBox.Size = New System.Drawing.Size(196, 57)
		Me.buttonsGroupBox.TabIndex = 5
		Me.buttonsGroupBox.TabStop = False
		' 
		' MainForm
		' 
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(476, 451)
		Me.Controls.Add(Me.buttonsGroupBox)
		Me.Controls.Add(Me.messageGroupBox)
		Me.Controls.Add(Me.sampleDataGroupBox)
		Me.Controls.Add(Me.generalGroupBox)
		Me.Controls.Add(Me.externalClockGroupBox)
		Me.Controls.Add(Me.verticalAndHorizontalGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Icon = DirectCast(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.Name = "MainForm"
		Me.Text = "External Clocking"
		AddHandler Me.FormClosing, New System.Windows.Forms.FormClosingEventHandler(AddressOf Me.MainForm_Closing)
		DirectCast(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.timebaseMultiplierNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.timebaseDivisorNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.timebaseRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.verticalOffsetNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		Me.externalClockGroupBox.ResumeLayout(False)
		Me.externalClockGroupBox.PerformLayout()
		Me.generalGroupBox.ResumeLayout(False)
		Me.generalGroupBox.PerformLayout()
		Me.verticalAndHorizontalGroupBox.ResumeLayout(False)
		Me.verticalAndHorizontalGroupBox.PerformLayout()
		Me.sampleDataGroupBox.ResumeLayout(False)
		Me.sampleDataGroupBox.PerformLayout()
		DirectCast(Me.scaledDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
		Me.messageGroupBox.ResumeLayout(False)
		Me.buttonsGroupBox.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub
	#End Region

	Private channelnameLabel As System.Windows.Forms.Label
	Private verticalRangeLabel As System.Windows.Forms.Label
	Private timebaseMultiplierLabel As System.Windows.Forms.Label
	Private timebaseDivisorLabel As System.Windows.Forms.Label
	Private timebaseRateLabel As System.Windows.Forms.Label
	Private verticalOffsetLabel As System.Windows.Forms.Label
	Private recordLengthMinLabel As System.Windows.Forms.Label
	Private timebaseSourceLabel As System.Windows.Forms.Label
	Private actualSampleRateLabel As System.Windows.Forms.Label
	Private textmsgLabel As System.Windows.Forms.Label
	Private textmsg3Label As System.Windows.Forms.Label
	Private textmsg5Label As System.Windows.Forms.Label
	Private resourceNameLabel As System.Windows.Forms.Label
	Private verticalRangeNumeric As System.Windows.Forms.NumericUpDown
	Private timebaseMultiplierNumeric As System.Windows.Forms.NumericUpDown
	Private timebaseDivisorNumeric As System.Windows.Forms.NumericUpDown
	Private timebaseRateNumeric As System.Windows.Forms.NumericUpDown
	Private verticalOffsetNumeric As System.Windows.Forms.NumericUpDown
	Private recordLengthMinNumeric As System.Windows.Forms.NumericUpDown
	Private channelNameTextBox As System.Windows.Forms.TextBox
	Private actualSampleRateTextBox As System.Windows.Forms.TextBox
	Private acquireButton As System.Windows.Forms.Button
	Private stopButton As System.Windows.Forms.Button
	Private timebaseSourceComboBox As System.Windows.Forms.ComboBox
	Private generalGroupBox As System.Windows.Forms.GroupBox
	Private externalClockGroupBox As System.Windows.Forms.GroupBox
	Private verticalAndHorizontalGroupBox As System.Windows.Forms.GroupBox
	Private sampleDataGroupBox As System.Windows.Forms.GroupBox
	Private scaledDataGridView As System.Windows.Forms.DataGridView
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private messageGroupBox As System.Windows.Forms.GroupBox
	Private messageTextBox As System.Windows.Forms.RichTextBox
	Private buttonsGroupBox As System.Windows.Forms.GroupBox
End Class
