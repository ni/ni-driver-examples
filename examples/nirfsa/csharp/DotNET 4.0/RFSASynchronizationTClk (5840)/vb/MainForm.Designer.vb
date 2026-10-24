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
        Me.referenceLevelLabel = New System.Windows.Forms.Label()
        Me.carrierFrequencyLabel = New System.Windows.Forms.Label()
        Me.iqRateLabel = New System.Windows.Forms.Label()
        Me.samplesPerRecordLabel = New System.Windows.Forms.Label()
        Me.numberOfDevicesLabel = New System.Windows.Forms.Label()
        Me.referenceClockMasterLabel = New System.Windows.Forms.Label()
        Me.referenceClockExportMasterLabel = New System.Windows.Forms.Label()
        Me.referenceClockSlaveLabel = New System.Windows.Forms.Label()
        Me.textmsgLabel = New System.Windows.Forms.Label()
        Me.referenceClockExportSlaveLabel = New System.Windows.Forms.Label()
        Me.referenceLevelNumeric = New System.Windows.Forms.NumericUpDown()
        Me.carrierFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iqRateNumeric = New System.Windows.Forms.NumericUpDown()
        Me.samplesPerRecordNumeric = New System.Windows.Forms.NumericUpDown()
        Me.numberOfDevicesNumeric = New System.Windows.Forms.NumericUpDown()
        Me.referenceClockMasterComboBox = New System.Windows.Forms.ComboBox()
        Me.referenceClockExportMasterComboBox = New System.Windows.Forms.ComboBox()
        Me.referenceClockSlaveComboBox = New System.Windows.Forms.ComboBox()
        Me.referenceClockExportSlaveComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameMasterComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameMasterLabel = New System.Windows.Forms.Label()
        Me.resourceNameSlaveComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameSlaveLabel = New System.Windows.Forms.Label()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.label1 = New System.Windows.Forms.Label()
        Me.slaveDataLabel = New System.Windows.Forms.Label()
        Me.masterDataLabel = New System.Windows.Forms.Label()
        Me.dataGridViewResultsId1 = New System.Windows.Forms.DataGridView()
        Me.dataGridViewResultsId0 = New System.Windows.Forms.DataGridView()
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.carrierFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.samplesPerRecordNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numberOfDevicesNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewResultsId1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dataGridViewResultsId0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'referenceLevelLabel
        '
        Me.referenceLevelLabel.AutoSize = True
        Me.referenceLevelLabel.Location = New System.Drawing.Point(12, 165)
        Me.referenceLevelLabel.Name = "referenceLevelLabel"
        Me.referenceLevelLabel.Size = New System.Drawing.Size(116, 13)
        Me.referenceLevelLabel.TabIndex = 0
        Me.referenceLevelLabel.Text = "Reference Level (dBm)"
        '
        'carrierFrequencyLabel
        '
        Me.carrierFrequencyLabel.AutoSize = True
        Me.carrierFrequencyLabel.Location = New System.Drawing.Point(12, 214)
        Me.carrierFrequencyLabel.Name = "carrierFrequencyLabel"
        Me.carrierFrequencyLabel.Size = New System.Drawing.Size(112, 13)
        Me.carrierFrequencyLabel.TabIndex = 1
        Me.carrierFrequencyLabel.Text = "Carrier Frequency (Hz)"
        '
        'iqRateLabel
        '
        Me.iqRateLabel.AutoSize = True
        Me.iqRateLabel.Location = New System.Drawing.Point(12, 260)
        Me.iqRateLabel.Name = "iqRateLabel"
        Me.iqRateLabel.Size = New System.Drawing.Size(44, 13)
        Me.iqRateLabel.TabIndex = 2
        Me.iqRateLabel.Text = "IQ Rate"
        '
        'samplesPerRecordLabel
        '
        Me.samplesPerRecordLabel.AutoSize = True
        Me.samplesPerRecordLabel.Location = New System.Drawing.Point(12, 306)
        Me.samplesPerRecordLabel.Name = "samplesPerRecordLabel"
        Me.samplesPerRecordLabel.Size = New System.Drawing.Size(103, 13)
        Me.samplesPerRecordLabel.TabIndex = 3
        Me.samplesPerRecordLabel.Text = "Samples per Record"
        '
        'numberOfDevicesLabel
        '
        Me.numberOfDevicesLabel.AutoSize = True
        Me.numberOfDevicesLabel.Location = New System.Drawing.Point(12, 27)
        Me.numberOfDevicesLabel.Name = "numberOfDevicesLabel"
        Me.numberOfDevicesLabel.Size = New System.Drawing.Size(98, 13)
        Me.numberOfDevicesLabel.TabIndex = 4
        Me.numberOfDevicesLabel.Text = "Number of Devices"
        '
        'referenceClockMasterLabel
        '
        Me.referenceClockMasterLabel.AutoSize = True
        Me.referenceClockMasterLabel.Location = New System.Drawing.Point(12, 355)
        Me.referenceClockMasterLabel.Name = "referenceClockMasterLabel"
        Me.referenceClockMasterLabel.Size = New System.Drawing.Size(122, 13)
        Me.referenceClockMasterLabel.TabIndex = 5
        Me.referenceClockMasterLabel.Text = "Master Reference Clock"
        '
        'referenceClockExportMasterLabel
        '
        Me.referenceClockExportMasterLabel.AutoSize = True
        Me.referenceClockExportMasterLabel.Location = New System.Drawing.Point(12, 403)
        Me.referenceClockExportMasterLabel.Name = "referenceClockExportMasterLabel"
        Me.referenceClockExportMasterLabel.Size = New System.Drawing.Size(155, 13)
        Me.referenceClockExportMasterLabel.TabIndex = 6
        Me.referenceClockExportMasterLabel.Text = "Master Reference Clock Export"
        '
        'referenceClockSlaveLabel
        '
        Me.referenceClockSlaveLabel.AutoSize = True
        Me.referenceClockSlaveLabel.Location = New System.Drawing.Point(12, 453)
        Me.referenceClockSlaveLabel.Name = "referenceClockSlaveLabel"
        Me.referenceClockSlaveLabel.Size = New System.Drawing.Size(117, 13)
        Me.referenceClockSlaveLabel.TabIndex = 7
        Me.referenceClockSlaveLabel.Text = "Slave Reference Clock"
        '
        'textmsgLabel
        '
        Me.textmsgLabel.Location = New System.Drawing.Point(0, 0)
        Me.textmsgLabel.Name = "textmsgLabel"
        Me.textmsgLabel.Size = New System.Drawing.Size(100, 23)
        Me.textmsgLabel.TabIndex = 8
        '
        'referenceClockExportSlaveLabel
        '
        Me.referenceClockExportSlaveLabel.AutoSize = True
        Me.referenceClockExportSlaveLabel.Location = New System.Drawing.Point(12, 503)
        Me.referenceClockExportSlaveLabel.Name = "referenceClockExportSlaveLabel"
        Me.referenceClockExportSlaveLabel.Size = New System.Drawing.Size(150, 13)
        Me.referenceClockExportSlaveLabel.TabIndex = 9
        Me.referenceClockExportSlaveLabel.Text = "Slave Reference Clock Export"
        '
        'referenceLevelNumeric
        '
        Me.referenceLevelNumeric.DecimalPlaces = 2
        Me.referenceLevelNumeric.Location = New System.Drawing.Point(12, 186)
        Me.referenceLevelNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.referenceLevelNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.referenceLevelNumeric.Name = "referenceLevelNumeric"
        Me.referenceLevelNumeric.Size = New System.Drawing.Size(151, 20)
        Me.referenceLevelNumeric.TabIndex = 3
        '
        'carrierFrequencyNumeric
        '
        Me.carrierFrequencyNumeric.DecimalPlaces = 2
        Me.carrierFrequencyNumeric.Location = New System.Drawing.Point(12, 235)
        Me.carrierFrequencyNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.carrierFrequencyNumeric.Name = "carrierFrequencyNumeric"
        Me.carrierFrequencyNumeric.Size = New System.Drawing.Size(151, 20)
        Me.carrierFrequencyNumeric.TabIndex = 4
        Me.carrierFrequencyNumeric.Value = New Decimal(New Integer() {1000000000, 0, 0, 0})
        '
        'iqRateNumeric
        '
        Me.iqRateNumeric.DecimalPlaces = 2
        Me.iqRateNumeric.Location = New System.Drawing.Point(12, 281)
        Me.iqRateNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.iqRateNumeric.Name = "iqRateNumeric"
        Me.iqRateNumeric.Size = New System.Drawing.Size(151, 20)
        Me.iqRateNumeric.TabIndex = 5
        Me.iqRateNumeric.Value = New Decimal(New Integer() {1000000, 0, 0, 0})
        '
        'samplesPerRecordNumeric
        '
        Me.samplesPerRecordNumeric.Location = New System.Drawing.Point(12, 327)
        Me.samplesPerRecordNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.samplesPerRecordNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.samplesPerRecordNumeric.Name = "samplesPerRecordNumeric"
        Me.samplesPerRecordNumeric.Size = New System.Drawing.Size(151, 20)
        Me.samplesPerRecordNumeric.TabIndex = 6
        Me.samplesPerRecordNumeric.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'numberOfDevicesNumeric
        '
        Me.numberOfDevicesNumeric.Enabled = False
        Me.numberOfDevicesNumeric.Location = New System.Drawing.Point(12, 48)
        Me.numberOfDevicesNumeric.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.numberOfDevicesNumeric.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numberOfDevicesNumeric.Name = "numberOfDevicesNumeric"
        Me.numberOfDevicesNumeric.ReadOnly = True
        Me.numberOfDevicesNumeric.Size = New System.Drawing.Size(153, 20)
        Me.numberOfDevicesNumeric.TabIndex = 0
        Me.numberOfDevicesNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'referenceClockMasterComboBox
        '
        Me.referenceClockMasterComboBox.Location = New System.Drawing.Point(12, 376)
        Me.referenceClockMasterComboBox.Name = "referenceClockMasterComboBox"
        Me.referenceClockMasterComboBox.Size = New System.Drawing.Size(151, 21)
        Me.referenceClockMasterComboBox.TabIndex = 7
        '
        'referenceClockExportMasterComboBox
        '
        Me.referenceClockExportMasterComboBox.Location = New System.Drawing.Point(12, 424)
        Me.referenceClockExportMasterComboBox.Name = "referenceClockExportMasterComboBox"
        Me.referenceClockExportMasterComboBox.Size = New System.Drawing.Size(151, 21)
        Me.referenceClockExportMasterComboBox.TabIndex = 8
        '
        'referenceClockSlaveComboBox
        '
        Me.referenceClockSlaveComboBox.Location = New System.Drawing.Point(12, 474)
        Me.referenceClockSlaveComboBox.Name = "referenceClockSlaveComboBox"
        Me.referenceClockSlaveComboBox.Size = New System.Drawing.Size(151, 21)
        Me.referenceClockSlaveComboBox.TabIndex = 9
        '
        'referenceClockExportSlaveComboBox
        '
        Me.referenceClockExportSlaveComboBox.Location = New System.Drawing.Point(12, 524)
        Me.referenceClockExportSlaveComboBox.Name = "referenceClockExportSlaveComboBox"
        Me.referenceClockExportSlaveComboBox.Size = New System.Drawing.Size(151, 21)
        Me.referenceClockExportSlaveComboBox.TabIndex = 10
        '
        'resourceNameMasterComboBox
        '
        Me.resourceNameMasterComboBox.Location = New System.Drawing.Point(12, 96)
        Me.resourceNameMasterComboBox.Name = "resourceNameMasterComboBox"
        Me.resourceNameMasterComboBox.Size = New System.Drawing.Size(155, 21)
        Me.resourceNameMasterComboBox.TabIndex = 1
        '
        'resourceNameMasterLabel
        '
        Me.resourceNameMasterLabel.AutoSize = True
        Me.resourceNameMasterLabel.Location = New System.Drawing.Point(12, 79)
        Me.resourceNameMasterLabel.Name = "resourceNameMasterLabel"
        Me.resourceNameMasterLabel.Size = New System.Drawing.Size(125, 13)
        Me.resourceNameMasterLabel.TabIndex = 14
        Me.resourceNameMasterLabel.Text = "Resource Name (Master)"
        '
        'resourceNameSlaveComboBox
        '
        Me.resourceNameSlaveComboBox.Location = New System.Drawing.Point(12, 140)
        Me.resourceNameSlaveComboBox.Name = "resourceNameSlaveComboBox"
        Me.resourceNameSlaveComboBox.Size = New System.Drawing.Size(155, 21)
        Me.resourceNameSlaveComboBox.TabIndex = 2
        '
        'resourceNameSlaveLabel
        '
        Me.resourceNameSlaveLabel.AutoSize = True
        Me.resourceNameSlaveLabel.Location = New System.Drawing.Point(12, 123)
        Me.resourceNameSlaveLabel.Name = "resourceNameSlaveLabel"
        Me.resourceNameSlaveLabel.Size = New System.Drawing.Size(132, 13)
        Me.resourceNameSlaveLabel.TabIndex = 16
        Me.resourceNameSlaveLabel.Text = "Resource Name ( Slave 1)"
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(12, 561)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(149, 23)
        Me.acquireButton.TabIndex = 11
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.label1.Location = New System.Drawing.Point(0, 0)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(673, 13)
        Me.label1.TabIndex = 17
        Me.label1.Text = "This example assumes that the association of the LO and digitizer for each device" & _
            " in MAX is consistent with the Clock settings in this example."
        '
        'slaveDataLabel
        '
        Me.slaveDataLabel.AutoSize = True
        Me.slaveDataLabel.Location = New System.Drawing.Point(170, 305)
        Me.slaveDataLabel.Name = "slaveDataLabel"
        Me.slaveDataLabel.Size = New System.Drawing.Size(60, 13)
        Me.slaveDataLabel.TabIndex = 28
        Me.slaveDataLabel.Text = "Slave Data"
        '
        'masterDataLabel
        '
        Me.masterDataLabel.AutoSize = True
        Me.masterDataLabel.Location = New System.Drawing.Point(170, 24)
        Me.masterDataLabel.Name = "masterDataLabel"
        Me.masterDataLabel.Size = New System.Drawing.Size(65, 13)
        Me.masterDataLabel.TabIndex = 27
        Me.masterDataLabel.Text = "Master Data"
        '
        'dataGridViewResultsId1
        '
        Me.dataGridViewResultsId1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewResultsId1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dataGridViewResultsId1.Location = New System.Drawing.Point(173, 324)
        Me.dataGridViewResultsId1.Name = "dataGridViewResultsId1"
        Me.dataGridViewResultsId1.Size = New System.Drawing.Size(485, 260)
        Me.dataGridViewResultsId1.TabIndex = 26
        '
        'dataGridViewResultsId0
        '
        Me.dataGridViewResultsId0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataGridViewResultsId0.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dataGridViewResultsId0.Location = New System.Drawing.Point(173, 40)
        Me.dataGridViewResultsId0.Name = "dataGridViewResultsId0"
        Me.dataGridViewResultsId0.Size = New System.Drawing.Size(485, 260)
        Me.dataGridViewResultsId0.TabIndex = 25
        '
        'MainForm
        '
        Me.AcceptButton = Me.acquireButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(672, 597)
        Me.Controls.Add(Me.slaveDataLabel)
        Me.Controls.Add(Me.masterDataLabel)
        Me.Controls.Add(Me.dataGridViewResultsId1)
        Me.Controls.Add(Me.dataGridViewResultsId0)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.acquireButton)
        Me.Controls.Add(Me.resourceNameSlaveComboBox)
        Me.Controls.Add(Me.resourceNameSlaveLabel)
        Me.Controls.Add(Me.resourceNameMasterComboBox)
        Me.Controls.Add(Me.resourceNameMasterLabel)
        Me.Controls.Add(Me.referenceLevelLabel)
        Me.Controls.Add(Me.carrierFrequencyLabel)
        Me.Controls.Add(Me.iqRateLabel)
        Me.Controls.Add(Me.samplesPerRecordLabel)
        Me.Controls.Add(Me.numberOfDevicesLabel)
        Me.Controls.Add(Me.referenceClockMasterLabel)
        Me.Controls.Add(Me.referenceClockExportMasterLabel)
        Me.Controls.Add(Me.referenceClockSlaveLabel)
        Me.Controls.Add(Me.textmsgLabel)
        Me.Controls.Add(Me.referenceClockExportSlaveLabel)
        Me.Controls.Add(Me.referenceLevelNumeric)
        Me.Controls.Add(Me.carrierFrequencyNumeric)
        Me.Controls.Add(Me.iqRateNumeric)
        Me.Controls.Add(Me.samplesPerRecordNumeric)
        Me.Controls.Add(Me.numberOfDevicesNumeric)
        Me.Controls.Add(Me.referenceClockMasterComboBox)
        Me.Controls.Add(Me.referenceClockExportMasterComboBox)
        Me.Controls.Add(Me.referenceClockSlaveComboBox)
        Me.Controls.Add(Me.referenceClockExportSlaveComboBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "RFSA Synchronization (TClk, Shared LO and Reference Clock)"
        CType(Me.referenceLevelNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.carrierFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iqRateNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.samplesPerRecordNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numberOfDevicesNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridViewResultsId1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dataGridViewResultsId0, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Private referenceLevelLabel As System.Windows.Forms.Label
    Private carrierFrequencyLabel As System.Windows.Forms.Label
    Private iqRateLabel As System.Windows.Forms.Label
    Private samplesPerRecordLabel As System.Windows.Forms.Label
    Private numberOfDevicesLabel As System.Windows.Forms.Label
    Private referenceClockMasterLabel As System.Windows.Forms.Label
    Private referenceClockExportMasterLabel As System.Windows.Forms.Label
    Private referenceClockSlaveLabel As System.Windows.Forms.Label
    Private textmsgLabel As System.Windows.Forms.Label
    Private referenceClockExportSlaveLabel As System.Windows.Forms.Label
    Private referenceLevelNumeric As System.Windows.Forms.NumericUpDown
    Private carrierFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private iqRateNumeric As System.Windows.Forms.NumericUpDown
    Private samplesPerRecordNumeric As System.Windows.Forms.NumericUpDown
    Private numberOfDevicesNumeric As System.Windows.Forms.NumericUpDown
    Private referenceClockMasterComboBox As System.Windows.Forms.ComboBox
    Private referenceClockExportMasterComboBox As System.Windows.Forms.ComboBox
    Private referenceClockSlaveComboBox As System.Windows.Forms.ComboBox
    Private referenceClockExportSlaveComboBox As System.Windows.Forms.ComboBox
    Private resourceNameMasterComboBox As System.Windows.Forms.ComboBox
    Private resourceNameMasterLabel As System.Windows.Forms.Label
    Private resourceNameSlaveComboBox As System.Windows.Forms.ComboBox
    Private resourceNameSlaveLabel As System.Windows.Forms.Label
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private label1 As System.Windows.Forms.Label
    Private WithEvents slaveDataLabel As System.Windows.Forms.Label
    Private WithEvents masterDataLabel As System.Windows.Forms.Label
    Private WithEvents dataGridViewResultsId1 As System.Windows.Forms.DataGridView
    Private WithEvents dataGridViewResultsId0 As System.Windows.Forms.DataGridView

End Class
