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
        Me.relayNameLabel = New System.Windows.Forms.Label()
        Me.topologyNameLabel = New System.Windows.Forms.Label()
        Me.topologyNameComboBox = New System.Windows.Forms.ComboBox()
        Me.relayNameTextBox = New System.Windows.Forms.TextBox()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.closeRelayButton = New System.Windows.Forms.Button()
        Me.openRelayButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'relayNameLabel
        '
        Me.relayNameLabel.AutoSize = True
        Me.relayNameLabel.Location = New System.Drawing.Point(18, 173)
        Me.relayNameLabel.Name = "relayNameLabel"
        Me.relayNameLabel.Size = New System.Drawing.Size(65, 13)
        Me.relayNameLabel.TabIndex = 7
        Me.relayNameLabel.Text = "Relay Name"
        '
        'topologyNameLabel
        '
        Me.topologyNameLabel.AutoSize = True
        Me.topologyNameLabel.Location = New System.Drawing.Point(18, 109)
        Me.topologyNameLabel.Name = "topologyNameLabel"
        Me.topologyNameLabel.Size = New System.Drawing.Size(82, 13)
        Me.topologyNameLabel.TabIndex = 6
        Me.topologyNameLabel.Text = "Topology Name"
        '
        'topologyNameComboBox
        '
        Me.topologyNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.topologyNameComboBox.FormattingEnabled = True
        Me.topologyNameComboBox.Location = New System.Drawing.Point(18, 125)
        Me.topologyNameComboBox.Name = "topologyNameComboBox"
        Me.topologyNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.topologyNameComboBox.TabIndex = 1
        '
        'relayNameTextBox
        '
        Me.relayNameTextBox.Location = New System.Drawing.Point(21, 198)
        Me.relayNameTextBox.Name = "relayNameTextBox"
        Me.relayNameTextBox.Size = New System.Drawing.Size(121, 20)
        Me.relayNameTextBox.TabIndex = 2
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(18, 44)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
        Me.resourceNameLabel.TabIndex = 5
        Me.resourceNameLabel.Text = "Resource Name"
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.FormattingEnabled = True
        Me.resourceNameComboBox.Location = New System.Drawing.Point(18, 60)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(121, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'closeRelayButton
        '
        Me.closeRelayButton.Location = New System.Drawing.Point(192, 142)
        Me.closeRelayButton.Name = "closeRelayButton"
        Me.closeRelayButton.Size = New System.Drawing.Size(75, 23)
        Me.closeRelayButton.TabIndex = 4
        Me.closeRelayButton.Text = "Close Relay"
        Me.closeRelayButton.UseVisualStyleBackColor = True
        '
        'openRelayButton
        '
        Me.openRelayButton.Location = New System.Drawing.Point(192, 89)
        Me.openRelayButton.Name = "openRelayButton"
        Me.openRelayButton.Size = New System.Drawing.Size(75, 23)
        Me.openRelayButton.TabIndex = 3
        Me.openRelayButton.Text = "Open Relay"
        Me.openRelayButton.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AcceptButton = Me.openRelayButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Controls.Add(Me.relayNameLabel)
        Me.Controls.Add(Me.topologyNameLabel)
        Me.Controls.Add(Me.topologyNameComboBox)
        Me.Controls.Add(Me.relayNameTextBox)
        Me.Controls.Add(Me.resourceNameLabel)
        Me.Controls.Add(Me.resourceNameComboBox)
        Me.Controls.Add(Me.closeRelayButton)
        Me.Controls.Add(Me.openRelayButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Controlling An Individual Relay"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents relayNameLabel As System.Windows.Forms.Label
    Private WithEvents topologyNameLabel As System.Windows.Forms.Label
    Private WithEvents topologyNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents relayNameTextBox As System.Windows.Forms.TextBox
    Private WithEvents resourceNameLabel As System.Windows.Forms.Label
    Private WithEvents resourceNameComboBox As System.Windows.Forms.ComboBox
    Private WithEvents closeRelayButton As System.Windows.Forms.Button
    Private WithEvents openRelayButton As System.Windows.Forms.Button

	#End Region
End Class

