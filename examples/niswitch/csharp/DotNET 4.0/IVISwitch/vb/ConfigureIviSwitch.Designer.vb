
Namespace NationalInstruments.Examples.IviSwitch
Partial Class ConfigureIviSwitch
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConfigureIviSwitch))
            Me.closeButton = New System.Windows.Forms.Button()
            Me.runPanelButton = New System.Windows.Forms.Button()
            Me.switchPathTextBox = New System.Windows.Forms.TextBox()
            Me.label6 = New System.Windows.Forms.Label()
            Me.label5 = New System.Windows.Forms.Label()
            Me.label4 = New System.Windows.Forms.Label()
            Me.label3 = New System.Windows.Forms.Label()
            Me.label2 = New System.Windows.Forms.Label()
            Me.label1 = New System.Windows.Forms.Label()
            Me.resultActionPanel = New System.Windows.Forms.Panel()
            Me.canConnectRichTextBox = New System.Windows.Forms.RichTextBox()
            Me.selectActionComboBox = New System.Windows.Forms.ComboBox()
            Me.channel2ComboBox = New System.Windows.Forms.ComboBox()
            Me.channel1ComboBox = New System.Windows.Forms.ComboBox()
            Me.SuspendLayout()
            '
            'closeButton
            '
            Me.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.closeButton.Location = New System.Drawing.Point(222, 273)
            Me.closeButton.Name = "closeButton"
            Me.closeButton.Size = New System.Drawing.Size(75, 23)
            Me.closeButton.TabIndex = 6
            Me.closeButton.Text = "Close/Quit"
            Me.closeButton.UseVisualStyleBackColor = True
            '
            'runPanelButton
            '
            Me.runPanelButton.Location = New System.Drawing.Point(16, 273)
            Me.runPanelButton.Name = "runPanelButton"
            Me.runPanelButton.Size = New System.Drawing.Size(75, 23)
            Me.runPanelButton.TabIndex = 5
            Me.runPanelButton.Text = "Run Panel"
            Me.runPanelButton.UseVisualStyleBackColor = True
            '
            'switchPathTextBox
            '
            Me.switchPathTextBox.Location = New System.Drawing.Point(16, 226)
            Me.switchPathTextBox.Name = "switchPathTextBox"
            Me.switchPathTextBox.Size = New System.Drawing.Size(283, 20)
            Me.switchPathTextBox.TabIndex = 4
            '
            'label6
            '
            Me.label6.AutoSize = True
            Me.label6.Location = New System.Drawing.Point(13, 210)
            Me.label6.Name = "label6"
            Me.label6.Size = New System.Drawing.Size(64, 13)
            Me.label6.TabIndex = 38
            Me.label6.Text = "Switch Path"
            '
            'label5
            '
            Me.label5.AutoSize = True
            Me.label5.Location = New System.Drawing.Point(13, 128)
            Me.label5.Name = "label5"
            Me.label5.Size = New System.Drawing.Size(175, 13)
            Me.label5.TabIndex = 37
            Me.label5.Text = "Can Connect/Disconnect/Get Path"
            '
            'label4
            '
            Me.label4.AutoSize = True
            Me.label4.Location = New System.Drawing.Point(162, 14)
            Me.label4.Name = "label4"
            Me.label4.Size = New System.Drawing.Size(84, 13)
            Me.label4.TabIndex = 36
            Me.label4.Text = "Result Of Action"
            '
            'label3
            '
            Me.label3.AutoSize = True
            Me.label3.Location = New System.Drawing.Point(162, 73)
            Me.label3.Name = "label3"
            Me.label3.Size = New System.Drawing.Size(140, 13)
            Me.label3.TabIndex = 35
            Me.label3.Text = "-->Connect From Channel # "
            '
            'label2
            '
            Me.label2.AutoSize = True
            Me.label2.Location = New System.Drawing.Point(13, 73)
            Me.label2.Name = "label2"
            Me.label2.Size = New System.Drawing.Size(140, 13)
            Me.label2.TabIndex = 34
            Me.label2.Text = "Connect From Channel # -->"
            '
            'label1
            '
            Me.label1.AutoSize = True
            Me.label1.Location = New System.Drawing.Point(13, 14)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(70, 13)
            Me.label1.TabIndex = 33
            Me.label1.Text = "Select Action"
            '
            'resultActionPanel
            '
            Me.resultActionPanel.BackColor = System.Drawing.Color.DarkOliveGreen
            Me.resultActionPanel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.resultActionPanel.Location = New System.Drawing.Point(176, 30)
            Me.resultActionPanel.Name = "resultActionPanel"
            Me.resultActionPanel.Size = New System.Drawing.Size(121, 21)
            Me.resultActionPanel.TabIndex = 7
            '
            'canConnectRichTextBox
            '
            Me.canConnectRichTextBox.Location = New System.Drawing.Point(16, 144)
            Me.canConnectRichTextBox.Name = "canConnectRichTextBox"
            Me.canConnectRichTextBox.Size = New System.Drawing.Size(283, 63)
            Me.canConnectRichTextBox.TabIndex = 3
            Me.canConnectRichTextBox.Text = ""
            '
            'selectActionComboBox
            '
            Me.selectActionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.selectActionComboBox.FormattingEnabled = True
            Me.selectActionComboBox.Location = New System.Drawing.Point(16, 30)
            Me.selectActionComboBox.Name = "selectActionComboBox"
            Me.selectActionComboBox.Size = New System.Drawing.Size(121, 21)
            Me.selectActionComboBox.TabIndex = 0
            '
            'channel2ComboBox
            '
            Me.channel2ComboBox.FormattingEnabled = True
            Me.channel2ComboBox.Location = New System.Drawing.Point(176, 89)
            Me.channel2ComboBox.Name = "channel2ComboBox"
            Me.channel2ComboBox.Size = New System.Drawing.Size(121, 21)
            Me.channel2ComboBox.TabIndex = 2
            '
            'channel1ComboBox
            '
            Me.channel1ComboBox.FormattingEnabled = True
            Me.channel1ComboBox.Location = New System.Drawing.Point(16, 89)
            Me.channel1ComboBox.Name = "channel1ComboBox"
            Me.channel1ComboBox.Size = New System.Drawing.Size(121, 21)
            Me.channel1ComboBox.TabIndex = 1
            '
            'ConfigureIviSwitch
            '
            Me.AcceptButton = Me.runPanelButton
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.closeButton
            Me.ClientSize = New System.Drawing.Size(318, 311)
            Me.Controls.Add(Me.closeButton)
            Me.Controls.Add(Me.runPanelButton)
            Me.Controls.Add(Me.switchPathTextBox)
            Me.Controls.Add(Me.label6)
            Me.Controls.Add(Me.label5)
            Me.Controls.Add(Me.label4)
            Me.Controls.Add(Me.label3)
            Me.Controls.Add(Me.label2)
            Me.Controls.Add(Me.label1)
            Me.Controls.Add(Me.resultActionPanel)
            Me.Controls.Add(Me.canConnectRichTextBox)
            Me.Controls.Add(Me.selectActionComboBox)
            Me.Controls.Add(Me.channel2ComboBox)
            Me.Controls.Add(Me.channel1ComboBox)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.Name = "ConfigureIviSwitch"
            Me.Text = "ConfigureIviSwitch"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
    Private WithEvents closeButton As System.Windows.Forms.Button
    Private WithEvents runPanelButton As System.Windows.Forms.Button
    Private WithEvents switchPathTextBox As System.Windows.Forms.TextBox
    Private WithEvents label6 As System.Windows.Forms.Label
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents resultActionPanel As System.Windows.Forms.Panel
    Private WithEvents canConnectRichTextBox As System.Windows.Forms.RichTextBox
    Private WithEvents selectActionComboBox As System.Windows.Forms.ComboBox
    Private WithEvents channel2ComboBox As System.Windows.Forms.ComboBox
    Private WithEvents channel1ComboBox As System.Windows.Forms.ComboBox
End Class
End Namespace