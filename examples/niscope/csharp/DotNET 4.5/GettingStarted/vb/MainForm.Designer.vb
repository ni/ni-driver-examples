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
        Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.actualSampleRateTextBox = New System.Windows.Forms.TextBox()
        Me.actualSampleRateLabel = New System.Windows.Forms.Label()
        Me.actualRecordLengthTextBox = New System.Windows.Forms.TextBox()
        Me.actualRecordLengthLabel = New System.Windows.Forms.Label()
        Me.channelNameLabel = New System.Windows.Forms.Label()
        Me.sampledDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.sampledDataGridView = New System.Windows.Forms.DataGridView()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.generalGroupBox = New System.Windows.Forms.GroupBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.channelNameTextBox = New System.Windows.Forms.TextBox()
        Me.buttonsGroupBox.SuspendLayout()
        Me.sampledDataGroupBox.SuspendLayout()
        CType(Me.sampledDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.generalGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'buttonsGroupBox
        '
        Me.buttonsGroupBox.Controls.Add(Me.acquireButton)
        Me.buttonsGroupBox.Location = New System.Drawing.Point(13, 437)
        Me.buttonsGroupBox.Name = "buttonsGroupBox"
        Me.buttonsGroupBox.Size = New System.Drawing.Size(245, 44)
        Me.buttonsGroupBox.TabIndex = 2
        Me.buttonsGroupBox.TabStop = False
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(95, 14)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(75, 23)
        Me.acquireButton.TabIndex = 0
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'actualSampleRateTextBox
        '
        Me.actualSampleRateTextBox.Location = New System.Drawing.Point(124, 45)
        Me.actualSampleRateTextBox.Name = "actualSampleRateTextBox"
        Me.actualSampleRateTextBox.ReadOnly = True
        Me.actualSampleRateTextBox.Size = New System.Drawing.Size(114, 20)
        Me.actualSampleRateTextBox.TabIndex = 3
        Me.actualSampleRateTextBox.Text = "0.0"
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
        'actualRecordLengthTextBox
        '
        Me.actualRecordLengthTextBox.Location = New System.Drawing.Point(124, 19)
        Me.actualRecordLengthTextBox.Name = "actualRecordLengthTextBox"
        Me.actualRecordLengthTextBox.ReadOnly = True
        Me.actualRecordLengthTextBox.Size = New System.Drawing.Size(114, 20)
        Me.actualRecordLengthTextBox.TabIndex = 1
        Me.actualRecordLengthTextBox.Text = "0"
        '
        'actualRecordLengthLabel
        '
        Me.actualRecordLengthLabel.AutoSize = True
        Me.actualRecordLengthLabel.Location = New System.Drawing.Point(6, 23)
        Me.actualRecordLengthLabel.Name = "actualRecordLengthLabel"
        Me.actualRecordLengthLabel.Size = New System.Drawing.Size(114, 13)
        Me.actualRecordLengthLabel.TabIndex = 0
        Me.actualRecordLengthLabel.Text = "Actual Record Length:"
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
        'sampledDataGroupBox
        '
        Me.sampledDataGroupBox.Controls.Add(Me.actualSampleRateTextBox)
        Me.sampledDataGroupBox.Controls.Add(Me.actualSampleRateLabel)
        Me.sampledDataGroupBox.Controls.Add(Me.actualRecordLengthTextBox)
        Me.sampledDataGroupBox.Controls.Add(Me.actualRecordLengthLabel)
        Me.sampledDataGroupBox.Controls.Add(Me.sampledDataGridView)
        Me.sampledDataGroupBox.Location = New System.Drawing.Point(13, 100)
        Me.sampledDataGroupBox.Name = "sampledDataGroupBox"
        Me.sampledDataGroupBox.Size = New System.Drawing.Size(245, 331)
        Me.sampledDataGroupBox.TabIndex = 1
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
        Me.sampledDataGridView.Location = New System.Drawing.Point(6, 71)
        Me.sampledDataGridView.Name = "sampledDataGridView"
        Me.sampledDataGridView.ReadOnly = True
        Me.sampledDataGridView.RowHeadersVisible = False
        Me.sampledDataGridView.Size = New System.Drawing.Size(205, 254)
        Me.sampledDataGridView.StandardTab = True
        Me.sampledDataGridView.TabIndex = 4
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
        'generalGroupBox
        '
        Me.generalGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.generalGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.generalGroupBox.Controls.Add(Me.channelNameLabel)
        Me.generalGroupBox.Controls.Add(Me.channelNameTextBox)
        Me.generalGroupBox.Location = New System.Drawing.Point(13, 12)
        Me.generalGroupBox.Name = "generalGroupBox"
        Me.generalGroupBox.Size = New System.Drawing.Size(245, 72)
        Me.generalGroupBox.TabIndex = 0
        Me.generalGroupBox.TabStop = False
        Me.generalGroupBox.Text = "General"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(124, 19)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(114, 21)
        Me.resourceNameComboBox.TabIndex = 1
        '
        'channelNameTextBox
        '
        Me.channelNameTextBox.Location = New System.Drawing.Point(124, 46)
        Me.channelNameTextBox.Name = "channelNameTextBox"
        Me.channelNameTextBox.Size = New System.Drawing.Size(114, 20)
        Me.channelNameTextBox.TabIndex = 3
        Me.channelNameTextBox.Text = "0"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(270, 493)
        Me.Controls.Add(Me.buttonsGroupBox)
        Me.Controls.Add(Me.sampledDataGroupBox)
        Me.Controls.Add(Me.generalGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Getting Started"
        Me.buttonsGroupBox.ResumeLayout(False)
        Me.sampledDataGroupBox.ResumeLayout(False)
        Me.sampledDataGroupBox.PerformLayout()
        CType(Me.sampledDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.generalGroupBox.ResumeLayout(False)
        Me.generalGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private buttonsGroupBox As System.Windows.Forms.GroupBox
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private actualSampleRateTextBox As System.Windows.Forms.TextBox
    Private actualSampleRateLabel As System.Windows.Forms.Label
    Private actualRecordLengthTextBox As System.Windows.Forms.TextBox
    Private actualRecordLengthLabel As System.Windows.Forms.Label
    Private channelNameLabel As System.Windows.Forms.Label
    Private sampledDataGroupBox As System.Windows.Forms.GroupBox
    Private sampledDataGridView As System.Windows.Forms.DataGridView
    Private resourceNameLabel As System.Windows.Forms.Label
    Private generalGroupBox As System.Windows.Forms.GroupBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private channelNameTextBox As System.Windows.Forms.TextBox
End Class
