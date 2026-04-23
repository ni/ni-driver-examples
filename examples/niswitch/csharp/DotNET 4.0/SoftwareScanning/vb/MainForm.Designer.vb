
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.topologyNameComboBox = New System.Windows.Forms.ComboBox()
        Me.topologyNameLabel = New System.Windows.Forms.Label()
        Me.groupBox1 = New System.Windows.Forms.GroupBox()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.scanListLabel = New System.Windows.Forms.Label()
        Me.scanListTextBox = New System.Windows.Forms.TextBox()
        Me.startScanningButton = New System.Windows.Forms.Button()
        Me.nextConnectionButton = New System.Windows.Forms.Button()
        Me.stopScanningButton = New System.Windows.Forms.Button()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(6, 30)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(143, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'topologyNameComboBox
        '
        Me.topologyNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.topologyNameComboBox.FormattingEnabled = True
        Me.topologyNameComboBox.Location = New System.Drawing.Point(6, 82)
        Me.topologyNameComboBox.Name = "topologyNameComboBox"
        Me.topologyNameComboBox.Size = New System.Drawing.Size(143, 21)
        Me.topologyNameComboBox.TabIndex = 1
        '
        'topologyNameLabel
        '
        Me.topologyNameLabel.AutoSize = True
        Me.topologyNameLabel.Location = New System.Drawing.Point(6, 66)
        Me.topologyNameLabel.Name = "topologyNameLabel"
        Me.topologyNameLabel.Size = New System.Drawing.Size(82, 13)
        Me.topologyNameLabel.TabIndex = 4
        Me.topologyNameLabel.Text = "Topology Name"
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.resourceNameLabel)
        Me.groupBox1.Controls.Add(Me.scanListLabel)
        Me.groupBox1.Controls.Add(Me.scanListTextBox)
        Me.groupBox1.Controls.Add(Me.topologyNameLabel)
        Me.groupBox1.Controls.Add(Me.topologyNameComboBox)
        Me.groupBox1.Controls.Add(Me.resourceNameComboBox)
        Me.groupBox1.Location = New System.Drawing.Point(12, 12)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(251, 164)
        Me.groupBox1.TabIndex = 0
        Me.groupBox1.TabStop = False
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 14)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 3
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'scanListLabel
        '
        Me.scanListLabel.AutoSize = True
        Me.scanListLabel.Location = New System.Drawing.Point(6, 122)
        Me.scanListLabel.Name = "scanListLabel"
        Me.scanListLabel.Size = New System.Drawing.Size(51, 13)
        Me.scanListLabel.TabIndex = 5
        Me.scanListLabel.Text = "Scan List"
        '
        'scanListTextBox
        '
        Me.scanListTextBox.Location = New System.Drawing.Point(9, 138)
        Me.scanListTextBox.Name = "scanListTextBox"
        Me.scanListTextBox.Size = New System.Drawing.Size(100, 20)
        Me.scanListTextBox.TabIndex = 2
        Me.scanListTextBox.Text = "ch0:15->com0;"
        '
        'startScanningButton
        '
        Me.startScanningButton.Location = New System.Drawing.Point(12, 202)
        Me.startScanningButton.Name = "startScanningButton"
        Me.startScanningButton.Size = New System.Drawing.Size(75, 37)
        Me.startScanningButton.TabIndex = 1
        Me.startScanningButton.Text = "Start Scanning"
        Me.startScanningButton.UseVisualStyleBackColor = True
        '
        'nextConnectionButton
        '
        Me.nextConnectionButton.Location = New System.Drawing.Point(102, 202)
        Me.nextConnectionButton.Name = "nextConnectionButton"
        Me.nextConnectionButton.Size = New System.Drawing.Size(75, 37)
        Me.nextConnectionButton.TabIndex = 2
        Me.nextConnectionButton.Text = "Next Connection"
        Me.nextConnectionButton.UseVisualStyleBackColor = True
        '
        'stopScanningButton
        '
        Me.stopScanningButton.Location = New System.Drawing.Point(192, 202)
        Me.stopScanningButton.Name = "stopScanningButton"
        Me.stopScanningButton.Size = New System.Drawing.Size(75, 37)
        Me.stopScanningButton.TabIndex = 3
        Me.stopScanningButton.Text = "Stop Scanning"
        Me.stopScanningButton.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AcceptButton = Me.startScanningButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Controls.Add(Me.stopScanningButton)
        Me.Controls.Add(Me.nextConnectionButton)
        Me.Controls.Add(Me.startScanningButton)
        Me.Controls.Add(Me.groupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.Text = "Software Scanning"
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private topologyNameComboBox As System.Windows.Forms.ComboBox
    Private topologyNameLabel As System.Windows.Forms.Label
    Private groupBox1 As System.Windows.Forms.GroupBox
    Private resourceNameLabel As System.Windows.Forms.Label
    Private scanListLabel As System.Windows.Forms.Label
    Private scanListTextBox As System.Windows.Forms.TextBox
    Private WithEvents startScanningButton As System.Windows.Forms.Button
    Private WithEvents nextConnectionButton As System.Windows.Forms.Button
    Private WithEvents stopScanningButton As System.Windows.Forms.Button

End Class

