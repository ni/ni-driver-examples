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
		Me.resourceNameLabel = New System.Windows.Forms.Label()
		Me.readFromFileRadioButton = New System.Windows.Forms.RadioButton()
		Me.acquireRadioButton = New System.Windows.Forms.RadioButton()
		Me.sampleRateMinLabel = New System.Windows.Forms.Label()
		Me.browseButton = New System.Windows.Forms.Button()
		Me.filePathTextBox = New System.Windows.Forms.TextBox()
		Me.startButton = New System.Windows.Forms.Button()
		Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
		Me.channelNameLabel = New System.Windows.Forms.Label()
		Me.channelNameTextBox = New System.Windows.Forms.TextBox()
		Me.configurationGroupBox = New System.Windows.Forms.GroupBox()
		Me.recordLengthMinNumeric = New System.Windows.Forms.NumericUpDown()
		Me.minSampleRateNumeric = New System.Windows.Forms.NumericUpDown()
		Me.verticalRangeNumeric = New System.Windows.Forms.NumericUpDown()
		Me.rangeLabel = New System.Windows.Forms.Label()
		Me.recordLengthMinLabel = New System.Windows.Forms.Label()
		Me.recordFromFileGroupBox = New System.Windows.Forms.GroupBox()
		Me.recordDataGridView = New System.Windows.Forms.DataGridView()
		Me.acquiredRecordGroupBox = New System.Windows.Forms.GroupBox()
		Me.acquiredDataGridView = New System.Windows.Forms.DataGridView()
		Me.filePathGroupBox = New System.Windows.Forms.GroupBox()
		Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
		Me.groupBox1 = New System.Windows.Forms.GroupBox()
		Me.configurationGroupBox.SuspendLayout()
		DirectCast(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.minSampleRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		DirectCast(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.recordFromFileGroupBox.SuspendLayout()
		DirectCast(Me.recordDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.acquiredRecordGroupBox.SuspendLayout()
		DirectCast(Me.acquiredDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.filePathGroupBox.SuspendLayout()
		Me.buttonsGroupBox.SuspendLayout()
		Me.groupBox1.SuspendLayout()
		Me.SuspendLayout()
		' 
		' resourceNameLabel
		' 
		Me.resourceNameLabel.AutoSize = True
		Me.resourceNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
		Me.resourceNameLabel.Location = New System.Drawing.Point(6, 22)
		Me.resourceNameLabel.Name = "resourceNameLabel"
		Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
		Me.resourceNameLabel.TabIndex = 0
		Me.resourceNameLabel.Text = "Resource Name:"
		' 
		' readFromFileRadioButton
		' 
		Me.readFromFileRadioButton.AutoSize = True
		Me.readFromFileRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
		Me.readFromFileRadioButton.Location = New System.Drawing.Point(214, 19)
		Me.readFromFileRadioButton.Name = "readFromFileRadioButton"
		Me.readFromFileRadioButton.Size = New System.Drawing.Size(170, 18)
		Me.readFromFileRadioButton.TabIndex = 1
		Me.readFromFileRadioButton.TabStop = True
		Me.readFromFileRadioButton.Text = "Read record from file and plot"
		Me.readFromFileRadioButton.UseVisualStyleBackColor = True
		' 
		' acquireRadioButton
		' 
		Me.acquireRadioButton.AutoSize = True
		Me.acquireRadioButton.Checked = True
		Me.acquireRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System
		Me.acquireRadioButton.Location = New System.Drawing.Point(6, 19)
		Me.acquireRadioButton.Name = "acquireRadioButton"
		Me.acquireRadioButton.Size = New System.Drawing.Size(175, 18)
		Me.acquireRadioButton.TabIndex = 0
		Me.acquireRadioButton.TabStop = True
		Me.acquireRadioButton.Text = "Acquire record and save to file"
		Me.acquireRadioButton.UseVisualStyleBackColor = True
		AddHandler Me.acquireRadioButton.CheckedChanged, New System.EventHandler(AddressOf Me.radioButton_CheckedChanged)
		' 
		' sampleRateMinLabel
		' 
		Me.sampleRateMinLabel.AutoSize = True
		Me.sampleRateMinLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
		Me.sampleRateMinLabel.Location = New System.Drawing.Point(6, 102)
		Me.sampleRateMinLabel.Name = "sampleRateMinLabel"
		Me.sampleRateMinLabel.Size = New System.Drawing.Size(91, 13)
		Me.sampleRateMinLabel.TabIndex = 6
		Me.sampleRateMinLabel.Text = "Min Sample Rate:"
		' 
		' browseButton
		' 
		Me.browseButton.FlatStyle = System.Windows.Forms.FlatStyle.System
		Me.browseButton.Location = New System.Drawing.Point(54, 45)
		Me.browseButton.Name = "browseButton"
		Me.browseButton.Size = New System.Drawing.Size(75, 23)
		Me.browseButton.TabIndex = 1
		Me.browseButton.Text = "&Browse..."
		Me.browseButton.UseVisualStyleBackColor = True
		AddHandler Me.browseButton.Click, New System.EventHandler(AddressOf Me.browseButton_Click)
		' 
		' filePathTextBox
		' 
		Me.filePathTextBox.AcceptsReturn = True
		Me.filePathTextBox.Location = New System.Drawing.Point(6, 19)
		Me.filePathTextBox.Name = "filePathTextBox"
		Me.filePathTextBox.Size = New System.Drawing.Size(190, 20)
		Me.filePathTextBox.TabIndex = 0
		Me.filePathTextBox.Text = "C:\waveform\waveform.txt"
		' 
		' startButton
		' 
		Me.startButton.Location = New System.Drawing.Point(168, 19)
		Me.startButton.Name = "startButton"
		Me.startButton.Size = New System.Drawing.Size(75, 23)
		Me.startButton.TabIndex = 0
		Me.startButton.Text = "&Start"
		Me.startButton.UseVisualStyleBackColor = True
		AddHandler Me.startButton.Click, New System.EventHandler(AddressOf Me.startButton_Click)
		' 
		' resourceNameComboBox
		' 
		Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.resourceNameComboBox.FormattingEnabled = True
		Me.resourceNameComboBox.Location = New System.Drawing.Point(112, 19)
		Me.resourceNameComboBox.Name = "resourceNameComboBox"
		Me.resourceNameComboBox.Size = New System.Drawing.Size(84, 21)
		Me.resourceNameComboBox.TabIndex = 1
		' 
		' channelNameLabel
		' 
		Me.channelNameLabel.AutoSize = True
		Me.channelNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
		Me.channelNameLabel.Location = New System.Drawing.Point(6, 50)
		Me.channelNameLabel.Name = "channelNameLabel"
		Me.channelNameLabel.Size = New System.Drawing.Size(80, 13)
		Me.channelNameLabel.TabIndex = 2
		Me.channelNameLabel.Text = "Channel Name:"
		' 
		' channelNameTextBox
		' 
		Me.channelNameTextBox.Location = New System.Drawing.Point(112, 46)
		Me.channelNameTextBox.Name = "channelNameTextBox"
		Me.channelNameTextBox.Size = New System.Drawing.Size(84, 20)
		Me.channelNameTextBox.TabIndex = 3
		Me.channelNameTextBox.Text = "0"
		' 
		' configurationGroupBox
		' 
		Me.configurationGroupBox.Controls.Add(Me.channelNameLabel)
		Me.configurationGroupBox.Controls.Add(Me.resourceNameLabel)
		Me.configurationGroupBox.Controls.Add(Me.channelNameTextBox)
		Me.configurationGroupBox.Controls.Add(Me.resourceNameComboBox)
		Me.configurationGroupBox.Controls.Add(Me.recordLengthMinNumeric)
		Me.configurationGroupBox.Controls.Add(Me.minSampleRateNumeric)
		Me.configurationGroupBox.Controls.Add(Me.verticalRangeNumeric)
		Me.configurationGroupBox.Controls.Add(Me.sampleRateMinLabel)
		Me.configurationGroupBox.Controls.Add(Me.rangeLabel)
		Me.configurationGroupBox.Controls.Add(Me.recordLengthMinLabel)
		Me.configurationGroupBox.Location = New System.Drawing.Point(12, 73)
		Me.configurationGroupBox.Name = "configurationGroupBox"
		Me.configurationGroupBox.Size = New System.Drawing.Size(202, 152)
		Me.configurationGroupBox.TabIndex = 1
		Me.configurationGroupBox.TabStop = False
		Me.configurationGroupBox.Text = "Configuration"
		' 
		' recordLengthMinNumeric
		' 
		Me.recordLengthMinNumeric.Location = New System.Drawing.Point(112, 124)
		Me.recordLengthMinNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.recordLengthMinNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.recordLengthMinNumeric.Name = "recordLengthMinNumeric"
		Me.recordLengthMinNumeric.Size = New System.Drawing.Size(84, 20)
		Me.recordLengthMinNumeric.TabIndex = 9
		Me.recordLengthMinNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
		' 
		' minSampleRateNumeric
		' 
		Me.minSampleRateNumeric.DecimalPlaces = 2
		Me.minSampleRateNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
		Me.minSampleRateNumeric.Location = New System.Drawing.Point(112, 98)
		Me.minSampleRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
		Me.minSampleRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
		Me.minSampleRateNumeric.Name = "minSampleRateNumeric"
		Me.minSampleRateNumeric.Size = New System.Drawing.Size(84, 20)
		Me.minSampleRateNumeric.TabIndex = 7
        Me.minSampleRateNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
		' 
		' verticalRangeNumeric
		' 
		Me.verticalRangeNumeric.Location = New System.Drawing.Point(112, 72)
		Me.verticalRangeNumeric.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
		Me.verticalRangeNumeric.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
		Me.verticalRangeNumeric.Name = "verticalRangeNumeric"
		Me.verticalRangeNumeric.Size = New System.Drawing.Size(84, 20)
		Me.verticalRangeNumeric.TabIndex = 5
		Me.verticalRangeNumeric.Value = New Decimal(New Integer() {10, 0, 0, 0})
		' 
		' rangeLabel
		' 
		Me.rangeLabel.AutoSize = True
		Me.rangeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
		Me.rangeLabel.Location = New System.Drawing.Point(6, 76)
		Me.rangeLabel.Name = "rangeLabel"
		Me.rangeLabel.Size = New System.Drawing.Size(42, 13)
		Me.rangeLabel.TabIndex = 4
		Me.rangeLabel.Text = "Range:"
		' 
		' recordLengthMinLabel
		' 
		Me.recordLengthMinLabel.AutoSize = True
		Me.recordLengthMinLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
		Me.recordLengthMinLabel.Location = New System.Drawing.Point(6, 128)
		Me.recordLengthMinLabel.Name = "recordLengthMinLabel"
		Me.recordLengthMinLabel.Size = New System.Drawing.Size(101, 13)
		Me.recordLengthMinLabel.TabIndex = 8
		Me.recordLengthMinLabel.Text = "Min Record Length:"
		' 
		' recordFromFileGroupBox
		' 
		Me.recordFromFileGroupBox.Controls.Add(Me.recordDataGridView)
		Me.recordFromFileGroupBox.Location = New System.Drawing.Point(220, 231)
		Me.recordFromFileGroupBox.Name = "recordFromFileGroupBox"
		Me.recordFromFileGroupBox.Size = New System.Drawing.Size(202, 289)
		Me.recordFromFileGroupBox.TabIndex = 4
		Me.recordFromFileGroupBox.TabStop = False
		Me.recordFromFileGroupBox.Text = "Record From File"
		' 
		' recordDataGridView
		' 
		Me.recordDataGridView.AllowUserToAddRows = False
		Me.recordDataGridView.AllowUserToDeleteRows = False
		Me.recordDataGridView.AllowUserToResizeRows = False
		Me.recordDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.recordDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.recordDataGridView.Location = New System.Drawing.Point(6, 19)
		Me.recordDataGridView.Name = "recordDataGridView"
		Me.recordDataGridView.[ReadOnly] = True
		Me.recordDataGridView.RowHeadersVisible = False
		Me.recordDataGridView.RowHeadersWidth = 15
		Me.recordDataGridView.RowTemplate.Height = 24
		Me.recordDataGridView.Size = New System.Drawing.Size(190, 264)
		Me.recordDataGridView.StandardTab = True
		Me.recordDataGridView.TabIndex = 0
		' 
		' acquiredRecordGroupBox
		' 
		Me.acquiredRecordGroupBox.Controls.Add(Me.acquiredDataGridView)
		Me.acquiredRecordGroupBox.Location = New System.Drawing.Point(12, 231)
		Me.acquiredRecordGroupBox.Name = "acquiredRecordGroupBox"
		Me.acquiredRecordGroupBox.Size = New System.Drawing.Size(202, 289)
		Me.acquiredRecordGroupBox.TabIndex = 2
		Me.acquiredRecordGroupBox.TabStop = False
		Me.acquiredRecordGroupBox.Text = "Acquired Record"
		' 
		' acquiredDataGridView
		' 
		Me.acquiredDataGridView.AllowUserToAddRows = False
		Me.acquiredDataGridView.AllowUserToDeleteRows = False
		Me.acquiredDataGridView.AllowUserToResizeRows = False
		Me.acquiredDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.acquiredDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.acquiredDataGridView.Location = New System.Drawing.Point(6, 19)
		Me.acquiredDataGridView.Name = "acquiredDataGridView"
		Me.acquiredDataGridView.[ReadOnly] = True
		Me.acquiredDataGridView.RowHeadersVisible = False
		Me.acquiredDataGridView.RowHeadersWidth = 15
		Me.acquiredDataGridView.RowTemplate.Height = 24
		Me.acquiredDataGridView.Size = New System.Drawing.Size(190, 264)
		Me.acquiredDataGridView.StandardTab = True
		Me.acquiredDataGridView.TabIndex = 0
		' 
		' filePathGroupBox
		' 
		Me.filePathGroupBox.Controls.Add(Me.filePathTextBox)
		Me.filePathGroupBox.Controls.Add(Me.browseButton)
		Me.filePathGroupBox.Location = New System.Drawing.Point(220, 73)
		Me.filePathGroupBox.Name = "filePathGroupBox"
		Me.filePathGroupBox.Size = New System.Drawing.Size(202, 74)
		Me.filePathGroupBox.TabIndex = 3
		Me.filePathGroupBox.TabStop = False
		Me.filePathGroupBox.Text = "File Path"
		' 
		' buttonsGroupBox
		' 
		Me.buttonsGroupBox.Controls.Add(Me.startButton)
		Me.buttonsGroupBox.Location = New System.Drawing.Point(12, 526)
		Me.buttonsGroupBox.Name = "buttonsGroupBox"
		Me.buttonsGroupBox.Size = New System.Drawing.Size(410, 55)
		Me.buttonsGroupBox.TabIndex = 5
		Me.buttonsGroupBox.TabStop = False
		' 
		' groupBox1
		' 
		Me.groupBox1.Controls.Add(Me.readFromFileRadioButton)
		Me.groupBox1.Controls.Add(Me.acquireRadioButton)
		Me.groupBox1.Location = New System.Drawing.Point(12, 12)
		Me.groupBox1.Name = "groupBox1"
		Me.groupBox1.Size = New System.Drawing.Size(410, 55)
		Me.groupBox1.TabIndex = 0
		Me.groupBox1.TabStop = False
		Me.groupBox1.Text = "Acquisition Type"
		' 
		' MainForm
		' 
				Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(434, 593)
		Me.Controls.Add(Me.groupBox1)
		Me.Controls.Add(Me.buttonsGroupBox)
		Me.Controls.Add(Me.filePathGroupBox)
		Me.Controls.Add(Me.recordFromFileGroupBox)
		Me.Controls.Add(Me.acquiredRecordGroupBox)
		Me.Controls.Add(Me.configurationGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Icon = DirectCast(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.Name = "MainForm"
		Me.Text = "Stream To Disk"
		Me.configurationGroupBox.ResumeLayout(False)
		Me.configurationGroupBox.PerformLayout()
		DirectCast(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.minSampleRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		DirectCast(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
		Me.recordFromFileGroupBox.ResumeLayout(False)
		DirectCast(Me.recordDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
		Me.acquiredRecordGroupBox.ResumeLayout(False)
		DirectCast(Me.acquiredDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
		Me.filePathGroupBox.ResumeLayout(False)
		Me.filePathGroupBox.PerformLayout()
		Me.buttonsGroupBox.ResumeLayout(False)
		Me.groupBox1.ResumeLayout(False)
		Me.groupBox1.PerformLayout()
		Me.ResumeLayout(False)

	End Sub

	#End Region

	Private resourceNameLabel As System.Windows.Forms.Label
	Private readFromFileRadioButton As System.Windows.Forms.RadioButton
	Private acquireRadioButton As System.Windows.Forms.RadioButton
	Private sampleRateMinLabel As System.Windows.Forms.Label
	Private browseButton As System.Windows.Forms.Button
	Private filePathTextBox As System.Windows.Forms.TextBox
	Private startButton As System.Windows.Forms.Button
	Private channelNameLabel As System.Windows.Forms.Label
	Private channelNameTextBox As System.Windows.Forms.TextBox
	Private configurationGroupBox As System.Windows.Forms.GroupBox
	Private rangeLabel As System.Windows.Forms.Label
	Private recordLengthMinLabel As System.Windows.Forms.Label
	Private recordFromFileGroupBox As System.Windows.Forms.GroupBox
	Private recordDataGridView As System.Windows.Forms.DataGridView
	Private acquiredRecordGroupBox As System.Windows.Forms.GroupBox
	Private acquiredDataGridView As System.Windows.Forms.DataGridView
	Private resourceNameComboBox As System.Windows.Forms.ComboBox
	Private filePathGroupBox As System.Windows.Forms.GroupBox
	Private buttonsGroupBox As System.Windows.Forms.GroupBox
	Private verticalRangeNumeric As System.Windows.Forms.NumericUpDown
	Private minSampleRateNumeric As System.Windows.Forms.NumericUpDown
	Private recordLengthMinNumeric As System.Windows.Forms.NumericUpDown
	Private groupBox1 As System.Windows.Forms.GroupBox
End Class

