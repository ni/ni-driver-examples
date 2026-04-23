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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.exportButton = New System.Windows.Forms.Button()
        Me.inputImpedanceLabel = New System.Windows.Forms.Label()
        Me.performAutoSetupButton = New System.Windows.Forms.Button()
        Me.importButton = New System.Windows.Forms.Button()
        Me.minSampleRateTextBox = New System.Windows.Forms.TextBox()
        Me.minSampleRateLabel = New System.Windows.Forms.Label()
        Me.verticalCouplingComboBox = New System.Windows.Forms.ComboBox()
        Me.inputImpedanceComboBox = New System.Windows.Forms.ComboBox()
        Me.verticalCouplingLabel = New System.Windows.Forms.Label()
        Me.verticalOffsetTextBox = New System.Windows.Forms.TextBox()
        Me.verticalOffsetLabel = New System.Windows.Forms.Label()
        Me.verticalRangeTextBox = New System.Windows.Forms.TextBox()
        Me.verticalRangeLabel = New System.Windows.Forms.Label()
        Me.buttonsGroupBox.SuspendLayout()
        Me.sampledDataGroupBox.SuspendLayout()
        CType(Me.sampledDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.generalGroupBox.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'buttonsGroupBox
        '
        Me.buttonsGroupBox.Controls.Add(Me.acquireButton)
        Me.buttonsGroupBox.Location = New System.Drawing.Point(264, 287)
        Me.buttonsGroupBox.Name = "buttonsGroupBox"
        Me.buttonsGroupBox.Size = New System.Drawing.Size(245, 55)
        Me.buttonsGroupBox.TabIndex = 2
        Me.buttonsGroupBox.TabStop = False
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(78, 18)
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
        Me.sampledDataGroupBox.Location = New System.Drawing.Point(264, 12)
        Me.sampledDataGroupBox.Name = "sampledDataGroupBox"
        Me.sampledDataGroupBox.Size = New System.Drawing.Size(245, 276)
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
        Me.sampledDataGridView.Location = New System.Drawing.Point(19, 71)
        Me.sampledDataGridView.Name = "sampledDataGridView"
        Me.sampledDataGridView.ReadOnly = True
        Me.sampledDataGridView.RowHeadersVisible = False
        Me.sampledDataGridView.Size = New System.Drawing.Size(205, 192)
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
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.exportButton)
        Me.GroupBox1.Controls.Add(Me.inputImpedanceLabel)
        Me.GroupBox1.Controls.Add(Me.performAutoSetupButton)
        Me.GroupBox1.Controls.Add(Me.importButton)
        Me.GroupBox1.Controls.Add(Me.minSampleRateTextBox)
        Me.GroupBox1.Controls.Add(Me.minSampleRateLabel)
        Me.GroupBox1.Controls.Add(Me.verticalCouplingComboBox)
        Me.GroupBox1.Controls.Add(Me.inputImpedanceComboBox)
        Me.GroupBox1.Controls.Add(Me.verticalCouplingLabel)
        Me.GroupBox1.Controls.Add(Me.verticalOffsetTextBox)
        Me.GroupBox1.Controls.Add(Me.verticalOffsetLabel)
        Me.GroupBox1.Controls.Add(Me.verticalRangeTextBox)
        Me.GroupBox1.Controls.Add(Me.verticalRangeLabel)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 90)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(245, 247)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Configuration"
        '
        'exportButton
        '
        Me.exportButton.Location = New System.Drawing.Point(18, 208)
        Me.exportButton.Name = "exportButton"
        Me.exportButton.Size = New System.Drawing.Size(101, 23)
        Me.exportButton.TabIndex = 0
        Me.exportButton.Text = "Export..."
        Me.exportButton.UseVisualStyleBackColor = True
        '
        'inputImpedanceLabel
        '
        Me.inputImpedanceLabel.AutoSize = True
        Me.inputImpedanceLabel.Location = New System.Drawing.Point(7, 101)
        Me.inputImpedanceLabel.Name = "inputImpedanceLabel"
        Me.inputImpedanceLabel.Size = New System.Drawing.Size(116, 13)
        Me.inputImpedanceLabel.TabIndex = 0
        Me.inputImpedanceLabel.Text = "Input Impedance (ohm)"
        '
        'performAutoSetupButton
        '
        Me.performAutoSetupButton.Location = New System.Drawing.Point(18, 163)
        Me.performAutoSetupButton.Name = "performAutoSetupButton"
        Me.performAutoSetupButton.Size = New System.Drawing.Size(208, 23)
        Me.performAutoSetupButton.TabIndex = 0
        Me.performAutoSetupButton.Text = "Perform Auto-Setup"
        Me.performAutoSetupButton.UseVisualStyleBackColor = True
        '
        'importButton
        '
        Me.importButton.Location = New System.Drawing.Point(125, 208)
        Me.importButton.Name = "importButton"
        Me.importButton.Size = New System.Drawing.Size(101, 23)
        Me.importButton.TabIndex = 0
        Me.importButton.Text = "Import..."
        Me.importButton.UseVisualStyleBackColor = True
        '
        'minSampleRateTextBox
        '
        Me.minSampleRateTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.minSampleRateTextBox.Location = New System.Drawing.Point(125, 124)
        Me.minSampleRateTextBox.Name = "minSampleRateTextBox"
        Me.minSampleRateTextBox.Size = New System.Drawing.Size(114, 20)
        Me.minSampleRateTextBox.TabIndex = 6
        Me.minSampleRateTextBox.Text = "0"
        '
        'minSampleRateLabel
        '
        Me.minSampleRateLabel.AutoSize = True
        Me.minSampleRateLabel.Location = New System.Drawing.Point(7, 128)
        Me.minSampleRateLabel.Name = "minSampleRateLabel"
        Me.minSampleRateLabel.Size = New System.Drawing.Size(88, 13)
        Me.minSampleRateLabel.TabIndex = 5
        Me.minSampleRateLabel.Text = "Min Sample Rate"
        '
        'verticalCouplingComboBox
        '
        Me.verticalCouplingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.verticalCouplingComboBox.FormattingEnabled = True
        Me.verticalCouplingComboBox.Location = New System.Drawing.Point(125, 71)
        Me.verticalCouplingComboBox.Name = "verticalCouplingComboBox"
        Me.verticalCouplingComboBox.Size = New System.Drawing.Size(114, 21)
        Me.verticalCouplingComboBox.TabIndex = 1
        '
        'inputImpedanceComboBox
        '
        Me.inputImpedanceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.inputImpedanceComboBox.FormattingEnabled = True
        Me.inputImpedanceComboBox.Location = New System.Drawing.Point(125, 97)
        Me.inputImpedanceComboBox.Name = "inputImpedanceComboBox"
        Me.inputImpedanceComboBox.Size = New System.Drawing.Size(114, 21)
        Me.inputImpedanceComboBox.TabIndex = 1
        '
        'verticalCouplingLabel
        '
        Me.verticalCouplingLabel.AutoSize = True
        Me.verticalCouplingLabel.Location = New System.Drawing.Point(7, 75)
        Me.verticalCouplingLabel.Name = "verticalCouplingLabel"
        Me.verticalCouplingLabel.Size = New System.Drawing.Size(86, 13)
        Me.verticalCouplingLabel.TabIndex = 5
        Me.verticalCouplingLabel.Text = "Vertical Coupling"
        '
        'verticalOffsetTextBox
        '
        Me.verticalOffsetTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.verticalOffsetTextBox.Location = New System.Drawing.Point(125, 45)
        Me.verticalOffsetTextBox.Name = "verticalOffsetTextBox"
        Me.verticalOffsetTextBox.Size = New System.Drawing.Size(114, 20)
        Me.verticalOffsetTextBox.TabIndex = 6
        Me.verticalOffsetTextBox.Text = "0"
        '
        'verticalOffsetLabel
        '
        Me.verticalOffsetLabel.AutoSize = True
        Me.verticalOffsetLabel.Location = New System.Drawing.Point(7, 49)
        Me.verticalOffsetLabel.Name = "verticalOffsetLabel"
        Me.verticalOffsetLabel.Size = New System.Drawing.Size(89, 13)
        Me.verticalOffsetLabel.TabIndex = 5
        Me.verticalOffsetLabel.Text = "Vertical Offset (V)"
        '
        'verticalRangeTextBox
        '
        Me.verticalRangeTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.verticalRangeTextBox.Location = New System.Drawing.Point(125, 19)
        Me.verticalRangeTextBox.Name = "verticalRangeTextBox"
        Me.verticalRangeTextBox.Size = New System.Drawing.Size(114, 20)
        Me.verticalRangeTextBox.TabIndex = 6
        Me.verticalRangeTextBox.Text = "0"
        '
        'verticalRangeLabel
        '
        Me.verticalRangeLabel.AutoSize = True
        Me.verticalRangeLabel.Location = New System.Drawing.Point(7, 23)
        Me.verticalRangeLabel.Name = "verticalRangeLabel"
        Me.verticalRangeLabel.Size = New System.Drawing.Size(93, 13)
        Me.verticalRangeLabel.TabIndex = 5
        Me.verticalRangeLabel.Text = "Vertical Range (V)"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(521, 348)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.buttonsGroupBox)
        Me.Controls.Add(Me.sampledDataGroupBox)
        Me.Controls.Add(Me.generalGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Export Import Configuration"
        Me.buttonsGroupBox.ResumeLayout(False)
        Me.sampledDataGroupBox.ResumeLayout(False)
        Me.sampledDataGroupBox.PerformLayout()
        CType(Me.sampledDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.generalGroupBox.ResumeLayout(False)
        Me.generalGroupBox.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
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
    Private WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents exportButton As System.Windows.Forms.Button
    Private WithEvents inputImpedanceLabel As System.Windows.Forms.Label
    Private WithEvents performAutoSetupButton As System.Windows.Forms.Button
    Private WithEvents importButton As System.Windows.Forms.Button
    Private WithEvents minSampleRateTextBox As System.Windows.Forms.TextBox
    Private WithEvents minSampleRateLabel As System.Windows.Forms.Label
    Private WithEvents verticalCouplingComboBox As System.Windows.Forms.ComboBox
    Private WithEvents inputImpedanceComboBox As System.Windows.Forms.ComboBox
    Private WithEvents verticalCouplingLabel As System.Windows.Forms.Label
    Private WithEvents verticalOffsetTextBox As System.Windows.Forms.TextBox
    Private WithEvents verticalOffsetLabel As System.Windows.Forms.Label
    Private WithEvents verticalRangeTextBox As System.Windows.Forms.TextBox
    Private WithEvents verticalRangeLabel As System.Windows.Forms.Label
End Class
