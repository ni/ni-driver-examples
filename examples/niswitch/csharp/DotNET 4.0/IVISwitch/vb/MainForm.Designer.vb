Namespace NationalInstruments.Examples.IviSwitch
    Partial Class MainForm
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
            Me.initializeButton = New System.Windows.Forms.Button()
            Me.resetDeviceCheckBox = New System.Windows.Forms.CheckBox()
            Me.idQueryCheckBox = New System.Windows.Forms.CheckBox()
            Me.resourceNameTextBox = New System.Windows.Forms.TextBox()
            Me.initializeGroupBox = New System.Windows.Forms.GroupBox()
            Me.resourceNameLabel = New System.Windows.Forms.Label()
            Me.initializeGroupBox.SuspendLayout()
            Me.SuspendLayout()
            '
            'initializeButton
            '
            Me.initializeButton.Location = New System.Drawing.Point(109, 83)
            Me.initializeButton.Name = "initializeButton"
            Me.initializeButton.Size = New System.Drawing.Size(75, 23)
            Me.initializeButton.TabIndex = 3
            Me.initializeButton.Text = "Initialize"
            Me.initializeButton.UseVisualStyleBackColor = True
            '
            'resetDeviceCheckBox
            '
            Me.resetDeviceCheckBox.AutoSize = True
            Me.resetDeviceCheckBox.Location = New System.Drawing.Point(140, 37)
            Me.resetDeviceCheckBox.Name = "resetDeviceCheckBox"
            Me.resetDeviceCheckBox.Size = New System.Drawing.Size(91, 17)
            Me.resetDeviceCheckBox.TabIndex = 2
            Me.resetDeviceCheckBox.Text = "Reset Device"
            Me.resetDeviceCheckBox.UseVisualStyleBackColor = True
            '
            'idQueryCheckBox
            '
            Me.idQueryCheckBox.AutoSize = True
            Me.idQueryCheckBox.Location = New System.Drawing.Point(140, 15)
            Me.idQueryCheckBox.Name = "idQueryCheckBox"
            Me.idQueryCheckBox.Size = New System.Drawing.Size(68, 17)
            Me.idQueryCheckBox.TabIndex = 1
            Me.idQueryCheckBox.Text = "ID Query"
            Me.idQueryCheckBox.UseVisualStyleBackColor = True
            '
            'resourceNameTextBox
            '
            Me.resourceNameTextBox.Location = New System.Drawing.Point(9, 35)
            Me.resourceNameTextBox.Name = "resourceNameTextBox"
            Me.resourceNameTextBox.Size = New System.Drawing.Size(100, 20)
            Me.resourceNameTextBox.TabIndex = 0
            '
            'initializeGroupBox
            '
            Me.initializeGroupBox.Controls.Add(Me.resetDeviceCheckBox)
            Me.initializeGroupBox.Controls.Add(Me.idQueryCheckBox)
            Me.initializeGroupBox.Controls.Add(Me.resourceNameTextBox)
            Me.initializeGroupBox.Controls.Add(Me.resourceNameLabel)
            Me.initializeGroupBox.Location = New System.Drawing.Point(12, 12)
            Me.initializeGroupBox.Name = "initializeGroupBox"
            Me.initializeGroupBox.Size = New System.Drawing.Size(269, 65)
            Me.initializeGroupBox.TabIndex = 20
            Me.initializeGroupBox.TabStop = False
            Me.initializeGroupBox.Text = "Initialize Switch"
            '
            'resourceNameLabel
            '
            Me.resourceNameLabel.AutoSize = True
            Me.resourceNameLabel.Location = New System.Drawing.Point(6, 16)
            Me.resourceNameLabel.Name = "resourceNameLabel"
            Me.resourceNameLabel.Size = New System.Drawing.Size(84, 13)
            Me.resourceNameLabel.TabIndex = 15
            Me.resourceNameLabel.Text = "Resource Name"
            '
            'MainForm
            '
            Me.AcceptButton = Me.initializeButton
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(293, 115)
            Me.Controls.Add(Me.initializeButton)
            Me.Controls.Add(Me.initializeGroupBox)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.Name = "MainForm"
            Me.Text = "Initialize Switch"
            Me.initializeGroupBox.ResumeLayout(False)
            Me.initializeGroupBox.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents initializeButton As System.Windows.Forms.Button
        Private WithEvents resetDeviceCheckBox As System.Windows.Forms.CheckBox
        Private WithEvents idQueryCheckBox As System.Windows.Forms.CheckBox
        Private WithEvents resourceNameTextBox As System.Windows.Forms.TextBox
        Private WithEvents initializeGroupBox As System.Windows.Forms.GroupBox
        Private WithEvents resourceNameLabel As System.Windows.Forms.Label

    End Class
End Namespace
