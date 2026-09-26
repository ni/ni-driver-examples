Partial Class MultiDataGridView
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

#Region "Component Designer generated code"

    ''' <summary> 
    ''' Required method for Designer support - do not modify 
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.dataGridView = New System.Windows.Forms.DataGridView()
        Me.recordLabel1 = New System.Windows.Forms.Label()
        Me.recordLabel2 = New System.Windows.Forms.Label()
        Me.recordNumberNumericUpDown = New System.Windows.Forms.NumericUpDown()
        CType(Me.dataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.recordNumberNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dataGridView
        '
        Me.dataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dataGridView.Location = New System.Drawing.Point(0, 3)
        Me.dataGridView.Name = "dataGridView"
        Me.dataGridView.Size = New System.Drawing.Size(438, 296)
        Me.dataGridView.TabIndex = 0
        '
        'recordLabel1
        '
        Me.recordLabel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.recordLabel1.AutoSize = True
        Me.recordLabel1.Location = New System.Drawing.Point(4, 308)
        Me.recordLabel1.Name = "recordLabel1"
        Me.recordLabel1.Size = New System.Drawing.Size(42, 13)
        Me.recordLabel1.TabIndex = 1
        Me.recordLabel1.Text = "Record"
        '
        'recordLabel2
        '
        Me.recordLabel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.recordLabel2.AutoSize = True
        Me.recordLabel2.Location = New System.Drawing.Point(117, 308)
        Me.recordLabel2.Name = "recordLabel2"
        Me.recordLabel2.Size = New System.Drawing.Size(25, 13)
        Me.recordLabel2.TabIndex = 6
        Me.recordLabel2.Text = "of 0"
        '
        'recordNumberNumericUpDown
        '
        Me.recordNumberNumericUpDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.recordNumberNumericUpDown.Enabled = False
        Me.recordNumberNumericUpDown.Location = New System.Drawing.Point(52, 305)
        Me.recordNumberNumericUpDown.Name = "recordNumberNumericUpDown"
        Me.recordNumberNumericUpDown.Size = New System.Drawing.Size(59, 20)
        Me.recordNumberNumericUpDown.TabIndex = 7
        '
        'MultiDataGridView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.recordNumberNumericUpDown)
        Me.Controls.Add(Me.recordLabel2)
        Me.Controls.Add(Me.recordLabel1)
        Me.Controls.Add(Me.dataGridView)
        Me.Name = "MultiDataGridView"
        Me.Size = New System.Drawing.Size(438, 331)
        CType(Me.dataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.recordNumberNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private dataGridView As System.Windows.Forms.DataGridView
    Private recordLabel1 As System.Windows.Forms.Label
    Private recordLabel2 As System.Windows.Forms.Label
    Private WithEvents recordNumberNumericUpDown As System.Windows.Forms.NumericUpDown


End Class
