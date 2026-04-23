
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
        Me.resourceName1ComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceName1Label = New System.Windows.Forms.Label()
        Me.resourceName2ComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceName2Label = New System.Windows.Forms.Label()
        Me.groupBox = New System.Windows.Forms.GroupBox()
        Me.columnChannel1ComboBox = New System.Windows.Forms.ComboBox()
        Me.columnChannel1Label = New System.Windows.Forms.Label()
        Me.topologyName1ComboBox = New System.Windows.Forms.ComboBox()
        Me.topologyName1Label = New System.Windows.Forms.Label()
        Me.groupBox2 = New System.Windows.Forms.GroupBox()
        Me.columnChannel2ComboBox = New System.Windows.Forms.ComboBox()
        Me.columnChannel2Label = New System.Windows.Forms.Label()
        Me.topologyName2ComboBox = New System.Windows.Forms.ComboBox()
        Me.topologyName2Label = New System.Windows.Forms.Label()
        Me.analogBusChannelLabel = New System.Windows.Forms.Label()
        Me.connectButton = New System.Windows.Forms.Button()
        Me.disconnectButton = New System.Windows.Forms.Button()
        Me.analogBusChannelsComboBox = New System.Windows.Forms.ComboBox()
        Me.groupBox.SuspendLayout()
        Me.groupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'resourceName1ComboBox
        '
        Me.resourceName1ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceName1ComboBox.FormattingEnabled = True
        Me.resourceName1ComboBox.Location = New System.Drawing.Point(24, 33)
        Me.resourceName1ComboBox.Name = "resourceName1ComboBox"
        Me.resourceName1ComboBox.Size = New System.Drawing.Size(121, 21)
        Me.resourceName1ComboBox.TabIndex = 0
        '
        'resourceName1Label
        '
        Me.resourceName1Label.AutoSize = True
        Me.resourceName1Label.Location = New System.Drawing.Point(21, 16)
        Me.resourceName1Label.Name = "resourceName1Label"
        Me.resourceName1Label.Size = New System.Drawing.Size(93, 13)
        Me.resourceName1Label.TabIndex = 1
        Me.resourceName1Label.Text = "Resource Name 1"
        '
        'resourceName2ComboBox
        '
        Me.resourceName2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceName2ComboBox.FormattingEnabled = True
        Me.resourceName2ComboBox.Location = New System.Drawing.Point(28, 33)
        Me.resourceName2ComboBox.Name = "resourceName2ComboBox"
        Me.resourceName2ComboBox.Size = New System.Drawing.Size(121, 21)
        Me.resourceName2ComboBox.TabIndex = 0
        '
        'resourceName2Label
        '
        Me.resourceName2Label.AutoSize = True
        Me.resourceName2Label.Location = New System.Drawing.Point(25, 16)
        Me.resourceName2Label.Name = "resourceName2Label"
        Me.resourceName2Label.Size = New System.Drawing.Size(93, 13)
        Me.resourceName2Label.TabIndex = 3
        Me.resourceName2Label.Text = "Resource Name 2"
        '
        'groupBox
        '
        Me.groupBox.Controls.Add(Me.columnChannel1ComboBox)
        Me.groupBox.Controls.Add(Me.columnChannel1Label)
        Me.groupBox.Controls.Add(Me.topologyName1ComboBox)
        Me.groupBox.Controls.Add(Me.topologyName1Label)
        Me.groupBox.Controls.Add(Me.resourceName1ComboBox)
        Me.groupBox.Controls.Add(Me.resourceName1Label)
        Me.groupBox.Location = New System.Drawing.Point(15, 25)
        Me.groupBox.Name = "groupBox"
        Me.groupBox.Size = New System.Drawing.Size(175, 202)
        Me.groupBox.TabIndex = 0
        Me.groupBox.TabStop = False
        Me.groupBox.Text = "Switch Resource 1"
        '
        'columnChannel1ComboBox
        '
        Me.columnChannel1ComboBox.FormattingEnabled = True
        Me.columnChannel1ComboBox.Location = New System.Drawing.Point(24, 145)
        Me.columnChannel1ComboBox.Name = "columnChannel1ComboBox"
        Me.columnChannel1ComboBox.Size = New System.Drawing.Size(100, 21)
        Me.columnChannel1ComboBox.TabIndex = 2
        '
        'columnChannel1Label
        '
        Me.columnChannel1Label.AutoSize = True
        Me.columnChannel1Label.Location = New System.Drawing.Point(21, 129)
        Me.columnChannel1Label.Name = "columnChannel1Label"
        Me.columnChannel1Label.Size = New System.Drawing.Size(93, 13)
        Me.columnChannel1Label.TabIndex = 4
        Me.columnChannel1Label.Text = "Column Channel 1"
        '
        'topologyName1ComboBox
        '
        Me.topologyName1ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.topologyName1ComboBox.FormattingEnabled = True
        Me.topologyName1ComboBox.Location = New System.Drawing.Point(24, 91)
        Me.topologyName1ComboBox.Name = "topologyName1ComboBox"
        Me.topologyName1ComboBox.Size = New System.Drawing.Size(121, 21)
        Me.topologyName1ComboBox.TabIndex = 1
        '
        'topologyName1Label
        '
        Me.topologyName1Label.AutoSize = True
        Me.topologyName1Label.Location = New System.Drawing.Point(21, 75)
        Me.topologyName1Label.Name = "topologyName1Label"
        Me.topologyName1Label.Size = New System.Drawing.Size(82, 13)
        Me.topologyName1Label.TabIndex = 2
        Me.topologyName1Label.Text = "Topology Name"
        '
        'groupBox2
        '
        Me.groupBox2.Controls.Add(Me.columnChannel2ComboBox)
        Me.groupBox2.Controls.Add(Me.columnChannel2Label)
        Me.groupBox2.Controls.Add(Me.topologyName2ComboBox)
        Me.groupBox2.Controls.Add(Me.topologyName2Label)
        Me.groupBox2.Controls.Add(Me.resourceName2ComboBox)
        Me.groupBox2.Controls.Add(Me.resourceName2Label)
        Me.groupBox2.Location = New System.Drawing.Point(224, 25)
        Me.groupBox2.Name = "groupBox2"
        Me.groupBox2.Size = New System.Drawing.Size(202, 202)
        Me.groupBox2.TabIndex = 1
        Me.groupBox2.TabStop = False
        Me.groupBox2.Text = "Switch Resource 2"
        '
        'columnChannel2ComboBox
        '
        Me.columnChannel2ComboBox.FormattingEnabled = True
        Me.columnChannel2ComboBox.Location = New System.Drawing.Point(28, 145)
        Me.columnChannel2ComboBox.Name = "columnChannel2ComboBox"
        Me.columnChannel2ComboBox.Size = New System.Drawing.Size(100, 21)
        Me.columnChannel2ComboBox.TabIndex = 2
        '
        'columnChannel2Label
        '
        Me.columnChannel2Label.AutoSize = True
        Me.columnChannel2Label.Location = New System.Drawing.Point(25, 129)
        Me.columnChannel2Label.Name = "columnChannel2Label"
        Me.columnChannel2Label.Size = New System.Drawing.Size(93, 13)
        Me.columnChannel2Label.TabIndex = 5
        Me.columnChannel2Label.Text = "Column Channel 2"
        '
        'topologyName2ComboBox
        '
        Me.topologyName2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.topologyName2ComboBox.FormattingEnabled = True
        Me.topologyName2ComboBox.Location = New System.Drawing.Point(28, 91)
        Me.topologyName2ComboBox.Name = "topologyName2ComboBox"
        Me.topologyName2ComboBox.Size = New System.Drawing.Size(121, 21)
        Me.topologyName2ComboBox.TabIndex = 1
        '
        'topologyName2Label
        '
        Me.topologyName2Label.AutoSize = True
        Me.topologyName2Label.Location = New System.Drawing.Point(25, 75)
        Me.topologyName2Label.Name = "topologyName2Label"
        Me.topologyName2Label.Size = New System.Drawing.Size(82, 13)
        Me.topologyName2Label.TabIndex = 4
        Me.topologyName2Label.Text = "Topology Name"
        '
        'analogBusChannelLabel
        '
        Me.analogBusChannelLabel.AutoSize = True
        Me.analogBusChannelLabel.Location = New System.Drawing.Point(441, 41)
        Me.analogBusChannelLabel.Name = "analogBusChannelLabel"
        Me.analogBusChannelLabel.Size = New System.Drawing.Size(103, 13)
        Me.analogBusChannelLabel.TabIndex = 5
        Me.analogBusChannelLabel.Text = "Analog Bus Channel"
        '
        'connectButton
        '
        Me.connectButton.Location = New System.Drawing.Point(135, 257)
        Me.connectButton.Name = "connectButton"
        Me.connectButton.Size = New System.Drawing.Size(75, 23)
        Me.connectButton.TabIndex = 3
        Me.connectButton.Text = "Connect"
        Me.connectButton.UseVisualStyleBackColor = True
        '
        'disconnectButton
        '
        Me.disconnectButton.Location = New System.Drawing.Point(256, 257)
        Me.disconnectButton.Name = "disconnectButton"
        Me.disconnectButton.Size = New System.Drawing.Size(75, 23)
        Me.disconnectButton.TabIndex = 4
        Me.disconnectButton.Text = "Disconnect"
        Me.disconnectButton.UseVisualStyleBackColor = True
        '
        'analogBusChannelsComboBox
        '
        Me.analogBusChannelsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.analogBusChannelsComboBox.FormattingEnabled = True
        Me.analogBusChannelsComboBox.Location = New System.Drawing.Point(444, 58)
        Me.analogBusChannelsComboBox.Name = "analogBusChannelsComboBox"
        Me.analogBusChannelsComboBox.Size = New System.Drawing.Size(100, 21)
        Me.analogBusChannelsComboBox.TabIndex = 2
        '
        'MainForm
        '
        Me.AcceptButton = Me.connectButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(586, 292)
        Me.Controls.Add(Me.analogBusChannelsComboBox)
        Me.Controls.Add(Me.disconnectButton)
        Me.Controls.Add(Me.connectButton)
        Me.Controls.Add(Me.analogBusChannelLabel)
        Me.Controls.Add(Me.groupBox2)
        Me.Controls.Add(Me.groupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Analog Bus Sharing Enable NI2815"
        Me.groupBox.ResumeLayout(False)
        Me.groupBox.PerformLayout()
        Me.groupBox2.ResumeLayout(False)
        Me.groupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private resourceName1ComboBox As System.Windows.Forms.ComboBox
    Private resourceName1Label As System.Windows.Forms.Label
    Private resourceName2ComboBox As System.Windows.Forms.ComboBox
    Private resourceName2Label As System.Windows.Forms.Label
    Private groupBox As System.Windows.Forms.GroupBox
    Private groupBox2 As System.Windows.Forms.GroupBox
    Private topologyName1Label As System.Windows.Forms.Label
    Private topologyName1ComboBox As System.Windows.Forms.ComboBox
    Private topologyName2ComboBox As System.Windows.Forms.ComboBox
    Private topologyName2Label As System.Windows.Forms.Label
    Private analogBusChannelLabel As System.Windows.Forms.Label
    Private columnChannel1Label As System.Windows.Forms.Label
    Private columnChannel2Label As System.Windows.Forms.Label
    Private WithEvents connectButton As System.Windows.Forms.Button
    Private WithEvents disconnectButton As System.Windows.Forms.Button
    Private analogBusChannelsComboBox As System.Windows.Forms.ComboBox
    Private columnChannel1ComboBox As System.Windows.Forms.ComboBox
    Private columnChannel2ComboBox As System.Windows.Forms.ComboBox
End Class


