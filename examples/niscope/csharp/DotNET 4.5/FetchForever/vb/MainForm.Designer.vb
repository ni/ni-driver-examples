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
        Me.minSampleRateLabel = New System.Windows.Forms.Label()
        Me.verticalRangeLabel = New System.Windows.Forms.Label()
        Me.maxPointsFetchedLabel = New System.Windows.Forms.Label()
        Me.totalPointsFetchedLabel = New System.Windows.Forms.Label()
        Me.lastfetchpointsLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.actualSamplesFromLastFetchedLabel = New System.Windows.Forms.Label()
        Me.verticalRangeNumeric = New System.Windows.Forms.NumericUpDown()
        Me.maxPointsFetchedNumeric = New System.Windows.Forms.NumericUpDown()
        Me.totalPointsFetchedTextBox = New System.Windows.Forms.TextBox()
        Me.lastFetchedPointsTextBox = New System.Windows.Forms.TextBox()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.horizontalAndVerticalGroupBox = New System.Windows.Forms.GroupBox()
        Me.minSampleRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.generalGroupBox = New System.Windows.Forms.GroupBox()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.fetchPointsGroupBox = New System.Windows.Forms.GroupBox()
        Me.waveformDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.waveformDataGridView = New System.Windows.Forms.DataGridView()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.RichTextBox()
        Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.maxPointsFetchedNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.horizontalAndVerticalGroupBox.SuspendLayout()
        CType(Me.minSampleRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.generalGroupBox.SuspendLayout()
        Me.fetchPointsGroupBox.SuspendLayout()
        Me.waveformDataGroupBox.SuspendLayout()
        CType(Me.waveformDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.channelNameLabel.TabIndex = 0
        Me.channelNameLabel.Text = "Channel Name:"
        '
        'minSampleRateLabel
        '
        Me.minSampleRateLabel.AutoSize = True
        Me.minSampleRateLabel.Location = New System.Drawing.Point(6, 23)
        Me.minSampleRateLabel.Name = "minSampleRateLabel"
        Me.minSampleRateLabel.Size = New System.Drawing.Size(91, 13)
        Me.minSampleRateLabel.TabIndex = 1
        Me.minSampleRateLabel.Text = "Min Sample Rate:"
        '
        'verticalRangeLabel
        '
        Me.verticalRangeLabel.AutoSize = True
        Me.verticalRangeLabel.Location = New System.Drawing.Point(6, 49)
        Me.verticalRangeLabel.Name = "verticalRangeLabel"
        Me.verticalRangeLabel.Size = New System.Drawing.Size(80, 13)
        Me.verticalRangeLabel.TabIndex = 2
        Me.verticalRangeLabel.Text = "Vertical Range:"
        '
        'maxPointsFetchedLabel
        '
        Me.maxPointsFetchedLabel.AutoSize = True
        Me.maxPointsFetchedLabel.Location = New System.Drawing.Point(6, 23)
        Me.maxPointsFetchedLabel.Name = "maxPointsFetchedLabel"
        Me.maxPointsFetchedLabel.Size = New System.Drawing.Size(106, 13)
        Me.maxPointsFetchedLabel.TabIndex = 4
        Me.maxPointsFetchedLabel.Text = "Max points per fetch:"
        '
        'totalPointsFetchedLabel
        '
        Me.totalPointsFetchedLabel.AutoSize = True
        Me.totalPointsFetchedLabel.Location = New System.Drawing.Point(6, 49)
        Me.totalPointsFetchedLabel.Name = "totalPointsFetchedLabel"
        Me.totalPointsFetchedLabel.Size = New System.Drawing.Size(104, 13)
        Me.totalPointsFetchedLabel.TabIndex = 5
        Me.totalPointsFetchedLabel.Text = "Total points fetched:"
        '
        'lastfetchpointsLabel
        '
        Me.lastfetchpointsLabel.AutoSize = True
        Me.lastfetchpointsLabel.Location = New System.Drawing.Point(-34, 277)
        Me.lastfetchpointsLabel.Name = "lastfetchpointsLabel"
        Me.lastfetchpointsLabel.Size = New System.Drawing.Size(0, 13)
        Me.lastfetchpointsLabel.TabIndex = 6
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 23)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 11
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'actualSamplesFromLastFetchedLabel
        '
        Me.actualSamplesFromLastFetchedLabel.AutoSize = True
        Me.actualSamplesFromLastFetchedLabel.Location = New System.Drawing.Point(6, 68)
        Me.actualSamplesFromLastFetchedLabel.Name = "actualSamplesFromLastFetchedLabel"
        Me.actualSamplesFromLastFetchedLabel.Size = New System.Drawing.Size(101, 26)
        Me.actualSamplesFromLastFetchedLabel.TabIndex = 17
        Me.actualSamplesFromLastFetchedLabel.Text = "Actual samples from" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "last fetch:"
        '
        'verticalRangeNumeric
        '
        Me.verticalRangeNumeric.DecimalPlaces = 2
        Me.verticalRangeNumeric.Location = New System.Drawing.Point(125, 45)
        Me.verticalRangeNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.verticalRangeNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.verticalRangeNumeric.Name = "verticalRangeNumeric"
        Me.verticalRangeNumeric.Size = New System.Drawing.Size(100, 20)
        Me.verticalRangeNumeric.TabIndex = 1
        Me.verticalRangeNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'maxPointsFetchedNumeric
        '
        Me.maxPointsFetchedNumeric.Location = New System.Drawing.Point(125, 19)
        Me.maxPointsFetchedNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.maxPointsFetchedNumeric.Minimum = New Decimal(New Integer() {-2147483648, 0, 0, -2147483648})
        Me.maxPointsFetchedNumeric.Name = "maxPointsFetchedNumeric"
        Me.maxPointsFetchedNumeric.Size = New System.Drawing.Size(100, 20)
        Me.maxPointsFetchedNumeric.TabIndex = 0
        Me.maxPointsFetchedNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'totalPointsFetchedTextBox
        '
        Me.totalPointsFetchedTextBox.Location = New System.Drawing.Point(125, 45)
        Me.totalPointsFetchedTextBox.Name = "totalPointsFetchedTextBox"
        Me.totalPointsFetchedTextBox.ReadOnly = True
        Me.totalPointsFetchedTextBox.Size = New System.Drawing.Size(100, 20)
        Me.totalPointsFetchedTextBox.TabIndex = 1
        Me.totalPointsFetchedTextBox.Text = "0"
        '
        'lastFetchedPointsTextBox
        '
        Me.lastFetchedPointsTextBox.Location = New System.Drawing.Point(125, 71)
        Me.lastFetchedPointsTextBox.Name = "lastFetchedPointsTextBox"
        Me.lastFetchedPointsTextBox.ReadOnly = True
        Me.lastFetchedPointsTextBox.Size = New System.Drawing.Size(100, 20)
        Me.lastFetchedPointsTextBox.TabIndex = 2
        Me.lastFetchedPointsTextBox.Text = "0"
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(37, 19)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(75, 23)
        Me.acquireButton.TabIndex = 0
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.Location = New System.Drawing.Point(118, 19)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'horizontalAndVerticalGroupBox
        '
        Me.horizontalAndVerticalGroupBox.Controls.Add(Me.minSampleRateNumeric)
        Me.horizontalAndVerticalGroupBox.Controls.Add(Me.verticalRangeNumeric)
        Me.horizontalAndVerticalGroupBox.Controls.Add(Me.minSampleRateLabel)
        Me.horizontalAndVerticalGroupBox.Controls.Add(Me.verticalRangeLabel)
        Me.horizontalAndVerticalGroupBox.Location = New System.Drawing.Point(12, 95)
        Me.horizontalAndVerticalGroupBox.Name = "horizontalAndVerticalGroupBox"
        Me.horizontalAndVerticalGroupBox.Size = New System.Drawing.Size(231, 72)
        Me.horizontalAndVerticalGroupBox.TabIndex = 1
        Me.horizontalAndVerticalGroupBox.TabStop = False
        Me.horizontalAndVerticalGroupBox.Text = "Horizontal And Vertical"
        '
        'minSampleRateNumeric
        '
        Me.minSampleRateNumeric.DecimalPlaces = 2
        Me.minSampleRateNumeric.Location = New System.Drawing.Point(125, 19)
        Me.minSampleRateNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.minSampleRateNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.minSampleRateNumeric.Name = "minSampleRateNumeric"
        Me.minSampleRateNumeric.Size = New System.Drawing.Size(100, 20)
        Me.minSampleRateNumeric.TabIndex = 0
        Me.minSampleRateNumeric.Value = New Decimal(New Integer() {10000000, 0, 0, 0})
        '
        'generalGroupBox
        '
        Me.generalGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.generalGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.generalGroupBox.Controls.Add(Me.channelNameLabel)
        Me.generalGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.generalGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.generalGroupBox.Name = "generalGroupBox"
        Me.generalGroupBox.Size = New System.Drawing.Size(231, 72)
        Me.generalGroupBox.TabIndex = 0
        Me.generalGroupBox.TabStop = False
        Me.generalGroupBox.Text = "General"
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(125, 46)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(100, 20)
        Me.channelNameTextBox.TabIndex = 1
        Me.channelNameTextBox.Text = "0"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(125, 19)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(100, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'fetchPointsGroupBox
        '
        Me.fetchPointsGroupBox.Controls.Add(Me.totalPointsFetchedTextBox)
        Me.fetchPointsGroupBox.Controls.Add(Me.lastFetchedPointsTextBox)
        Me.fetchPointsGroupBox.Controls.Add(Me.maxPointsFetchedNumeric)
        Me.fetchPointsGroupBox.Controls.Add(Me.maxPointsFetchedLabel)
        Me.fetchPointsGroupBox.Controls.Add(Me.totalPointsFetchedLabel)
        Me.fetchPointsGroupBox.Controls.Add(Me.actualSamplesFromLastFetchedLabel)
        Me.fetchPointsGroupBox.Location = New System.Drawing.Point(12, 178)
        Me.fetchPointsGroupBox.Name = "fetchPointsGroupBox"
        Me.fetchPointsGroupBox.Size = New System.Drawing.Size(231, 106)
        Me.fetchPointsGroupBox.TabIndex = 2
        Me.fetchPointsGroupBox.TabStop = False
        Me.fetchPointsGroupBox.Text = "Fetch Points"
        '
        'waveformDataGroupBox
        '
        Me.waveformDataGroupBox.Controls.Add(Me.waveformDataGridView)
        Me.waveformDataGroupBox.Location = New System.Drawing.Point(249, 12)
        Me.waveformDataGroupBox.Name = "waveformDataGroupBox"
        Me.waveformDataGroupBox.Size = New System.Drawing.Size(202, 403)
        Me.waveformDataGroupBox.TabIndex = 5
        Me.waveformDataGroupBox.TabStop = False
        Me.waveformDataGroupBox.Text = "Waveform data"
        '
        'waveformDataGridView
        '
        Me.waveformDataGridView.AllowUserToAddRows = False
        Me.waveformDataGridView.AllowUserToDeleteRows = False
        Me.waveformDataGridView.AllowUserToResizeRows = False
        Me.waveformDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.waveformDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.waveformDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.waveformDataGridView.Name = "waveformDataGridView"
        Me.waveformDataGridView.ReadOnly = True
        Me.waveformDataGridView.RowHeadersVisible = False
        Me.waveformDataGridView.RowHeadersWidth = 15
        Me.waveformDataGridView.RowTemplate.Height = 24
        Me.waveformDataGridView.Size = New System.Drawing.Size(190, 378)
        Me.waveformDataGridView.StandardTab = True
        Me.waveformDataGridView.TabIndex = 0
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(12, 295)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(231, 60)
        Me.messageGroupBox.TabIndex = 3
        Me.messageGroupBox.TabStop = False
        Me.messageGroupBox.Text = "Message"
        '
        'messageTextBox
        '
        Me.messageTextBox.Location = New System.Drawing.Point(9, 19)
        Me.messageTextBox.Name = "messageTextBox"
        Me.messageTextBox.ReadOnly = True
        Me.messageTextBox.Size = New System.Drawing.Size(216, 35)
        Me.messageTextBox.TabIndex = 0
        Me.messageTextBox.Text = ""
        '
        'buttonsGroupBox
        '
        Me.buttonsGroupBox.Controls.Add(Me.acquireButton)
        Me.buttonsGroupBox.Controls.Add(Me.stopButton)
        Me.buttonsGroupBox.Location = New System.Drawing.Point(12, 361)
        Me.buttonsGroupBox.Name = "buttonsGroupBox"
        Me.buttonsGroupBox.Size = New System.Drawing.Size(231, 54)
        Me.buttonsGroupBox.TabIndex = 4
        Me.buttonsGroupBox.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(464, 431)
        Me.Controls.Add(Me.buttonsGroupBox)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.waveformDataGroupBox)
        Me.Controls.Add(Me.lastfetchpointsLabel)
        Me.Controls.Add(Me.horizontalAndVerticalGroupBox)
        Me.Controls.Add(Me.generalGroupBox)
        Me.Controls.Add(Me.fetchPointsGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Fetch Forever"
        CType(Me.verticalRangeNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.maxPointsFetchedNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.horizontalAndVerticalGroupBox.ResumeLayout(False)
        Me.horizontalAndVerticalGroupBox.PerformLayout()
        CType(Me.minSampleRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.generalGroupBox.ResumeLayout(False)
        Me.generalGroupBox.PerformLayout()
        Me.fetchPointsGroupBox.ResumeLayout(False)
        Me.fetchPointsGroupBox.PerformLayout()
        Me.waveformDataGroupBox.ResumeLayout(False)
        CType(Me.waveformDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.messageGroupBox.ResumeLayout(False)
        Me.buttonsGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private channelNameLabel As System.Windows.Forms.Label
    Private minSampleRateLabel As System.Windows.Forms.Label
    Private verticalRangeLabel As System.Windows.Forms.Label
    Private maxPointsFetchedLabel As System.Windows.Forms.Label
    Private totalPointsFetchedLabel As System.Windows.Forms.Label
    Private lastfetchpointsLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private actualSamplesFromLastFetchedLabel As System.Windows.Forms.Label
    Private verticalRangeNumeric As System.Windows.Forms.NumericUpDown
    Private maxPointsFetchedNumeric As System.Windows.Forms.NumericUpDown
    Private totalPointsFetchedTextBox As System.Windows.Forms.TextBox
    Private lastFetchedPointsTextBox As System.Windows.Forms.TextBox
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private horizontalAndVerticalGroupBox As System.Windows.Forms.GroupBox
    Private generalGroupBox As System.Windows.Forms.GroupBox
    Private fetchPointsGroupBox As System.Windows.Forms.GroupBox
    Private minSampleRateNumeric As System.Windows.Forms.NumericUpDown
    Private channelNameTextBox As System.Windows.Forms.TextBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private waveformDataGroupBox As System.Windows.Forms.GroupBox
    Private waveformDataGridView As System.Windows.Forms.DataGridView
    Private messageGroupBox As System.Windows.Forms.GroupBox
    Private messageTextBox As System.Windows.Forms.RichTextBox
    Private buttonsGroupBox As System.Windows.Forms.GroupBox
End Class
