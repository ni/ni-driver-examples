
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
        Me.interleavedStartChannelNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.deviceScanConfigurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.interleavedEndChannelLabel = New System.Windows.Forms.Label()
        Me.interleavedStartChannelLabel = New System.Windows.Forms.Label()
        Me.interleavedEndChannelNumericUpDown = New System.Windows.Forms.NumericUpDown()
        Me.interleavedScanListRichTextBox = New System.Windows.Forms.RichTextBox()
        Me.interleavedScanListLabel = New System.Windows.Forms.Label()
        Me.generateButton = New System.Windows.Forms.Button()
        CType(Me.interleavedStartChannelNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.deviceScanConfigurationGroupBox.SuspendLayout()
        CType(Me.interleavedEndChannelNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'interleavedStartChannelNumericUpDown
        '
        Me.interleavedStartChannelNumericUpDown.Location = New System.Drawing.Point(50, 35)
        Me.interleavedStartChannelNumericUpDown.Name = "interleavedStartChannelNumericUpDown"
        Me.interleavedStartChannelNumericUpDown.Size = New System.Drawing.Size(120, 20)
        Me.interleavedStartChannelNumericUpDown.TabIndex = 0
        '
        'deviceScanConfigurationGroupBox
        '
        Me.deviceScanConfigurationGroupBox.Controls.Add(Me.interleavedEndChannelLabel)
        Me.deviceScanConfigurationGroupBox.Controls.Add(Me.interleavedStartChannelLabel)
        Me.deviceScanConfigurationGroupBox.Controls.Add(Me.interleavedEndChannelNumericUpDown)
        Me.deviceScanConfigurationGroupBox.Controls.Add(Me.interleavedStartChannelNumericUpDown)
        Me.deviceScanConfigurationGroupBox.Location = New System.Drawing.Point(12, 23)
        Me.deviceScanConfigurationGroupBox.Name = "deviceScanConfigurationGroupBox"
        Me.deviceScanConfigurationGroupBox.Size = New System.Drawing.Size(395, 71)
        Me.deviceScanConfigurationGroupBox.TabIndex = 0
        Me.deviceScanConfigurationGroupBox.TabStop = False
        Me.deviceScanConfigurationGroupBox.Text = "Device Scan Configuration"
        '
        'interleavedEndChannelLabel
        '
        Me.interleavedEndChannelLabel.AutoSize = True
        Me.interleavedEndChannelLabel.Location = New System.Drawing.Point(246, 16)
        Me.interleavedEndChannelLabel.Name = "interleavedEndChannelLabel"
        Me.interleavedEndChannelLabel.Size = New System.Drawing.Size(124, 13)
        Me.interleavedEndChannelLabel.TabIndex = 3
        Me.interleavedEndChannelLabel.Text = "Interleaved End Channel"
        '
        'interleavedStartChannelLabel
        '
        Me.interleavedStartChannelLabel.AutoSize = True
        Me.interleavedStartChannelLabel.Location = New System.Drawing.Point(43, 19)
        Me.interleavedStartChannelLabel.Name = "interleavedStartChannelLabel"
        Me.interleavedStartChannelLabel.Size = New System.Drawing.Size(127, 13)
        Me.interleavedStartChannelLabel.TabIndex = 2
        Me.interleavedStartChannelLabel.Text = "Interleaved Start Channel"
        '
        'interleavedEndChannelNumericUpDown
        '
        Me.interleavedEndChannelNumericUpDown.Location = New System.Drawing.Point(250, 35)
        Me.interleavedEndChannelNumericUpDown.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.interleavedEndChannelNumericUpDown.Name = "interleavedEndChannelNumericUpDown"
        Me.interleavedEndChannelNumericUpDown.Size = New System.Drawing.Size(120, 20)
        Me.interleavedEndChannelNumericUpDown.TabIndex = 1
        '
        'interleavedScanListRichTextBox
        '
        Me.interleavedScanListRichTextBox.Location = New System.Drawing.Point(12, 126)
        Me.interleavedScanListRichTextBox.Name = "interleavedScanListRichTextBox"
        Me.interleavedScanListRichTextBox.ReadOnly = True
        Me.interleavedScanListRichTextBox.Size = New System.Drawing.Size(247, 52)
        Me.interleavedScanListRichTextBox.TabIndex = 1
        Me.interleavedScanListRichTextBox.Text = ""
        '
        'interleavedScanListLabel
        '
        Me.interleavedScanListLabel.AutoSize = True
        Me.interleavedScanListLabel.Location = New System.Drawing.Point(12, 110)
        Me.interleavedScanListLabel.Name = "interleavedScanListLabel"
        Me.interleavedScanListLabel.Size = New System.Drawing.Size(107, 13)
        Me.interleavedScanListLabel.TabIndex = 2
        Me.interleavedScanListLabel.Text = "Interleaved Scan List"
        '
        'generateButton
        '
        Me.generateButton.Location = New System.Drawing.Point(283, 126)
        Me.generateButton.Name = "generateButton"
        Me.generateButton.Size = New System.Drawing.Size(110, 52)
        Me.generateButton.TabIndex = 3
        Me.generateButton.Text = "Generate Interleaved Scanlist"
        Me.generateButton.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AcceptButton = Me.generateButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(427, 207)
        Me.Controls.Add(Me.generateButton)
        Me.Controls.Add(Me.interleavedScanListLabel)
        Me.Controls.Add(Me.interleavedScanListRichTextBox)
        Me.Controls.Add(Me.deviceScanConfigurationGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Generate Multi Device Interleaved Scanlist PXI2584"
        CType(Me.interleavedStartChannelNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.deviceScanConfigurationGroupBox.ResumeLayout(False)
        Me.deviceScanConfigurationGroupBox.PerformLayout()
        CType(Me.interleavedEndChannelNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private interleavedStartChannelNumericUpDown As System.Windows.Forms.NumericUpDown
    Private deviceScanConfigurationGroupBox As System.Windows.Forms.GroupBox
    Private interleavedEndChannelNumericUpDown As System.Windows.Forms.NumericUpDown
    Private interleavedEndChannelLabel As System.Windows.Forms.Label
    Private interleavedStartChannelLabel As System.Windows.Forms.Label
    Private interleavedScanListRichTextBox As System.Windows.Forms.RichTextBox
    Private interleavedScanListLabel As System.Windows.Forms.Label
    Private WithEvents generateButton As System.Windows.Forms.Button

End Class


