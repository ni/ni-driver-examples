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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.channelLabel = New System.Windows.Forms.Label()
        Me.recordLengthMinLabel = New System.Windows.Forms.Label()
        Me.sampleRateMinLabel = New System.Windows.Forms.Label()
        Me.lowOrHighPassCutoffFrequencyLabel = New System.Windows.Forms.Label()
        Me.bandpassOrBandstopWidthLabel = New System.Windows.Forms.Label()
        Me.bandpassOrStopCenterFrequencyLabel = New System.Windows.Forms.Label()
        Me.firTapsLabel = New System.Windows.Forms.Label()
        Me.iirOrderLabel = New System.Windows.Forms.Label()
        Me.fftFunctionLabel = New System.Windows.Forms.Label()
        Me.filterTypeLabel = New System.Windows.Forms.Label()
        Me.firWindowLabel = New System.Windows.Forms.Label()
        Me.filterLabel = New System.Windows.Forms.Label()
        Me.resourceNameLabel = New System.Windows.Forms.Label()
        Me.recordLengthMinNumeric = New System.Windows.Forms.NumericUpDown()
        Me.sampleRateMinNumeric = New System.Windows.Forms.NumericUpDown()
        Me.lowOrHighPassCutoffFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.bandpassOrBandstopWidthNumeric = New System.Windows.Forms.NumericUpDown()
        Me.bandpassOrStopCenterFrequencyNumeric = New System.Windows.Forms.NumericUpDown()
        Me.firTapsNumeric = New System.Windows.Forms.NumericUpDown()
        Me.iirOrderNumeric = New System.Windows.Forms.NumericUpDown()
        Me.channelTextBox = New System.Windows.Forms.TextBox()
        Me.acquireButton = New System.Windows.Forms.Button()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.fftFunctionComboBox = New System.Windows.Forms.ComboBox()
        Me.functionsGroupBox = New System.Windows.Forms.GroupBox()
        Me.filterComboBox = New System.Windows.Forms.ComboBox()
        Me.filterTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.filterParametersGroupBox = New System.Windows.Forms.GroupBox()
        Me.firWindowComboBox = New System.Windows.Forms.ComboBox()
        Me.resourceNameComboBox = New System.Windows.Forms.ComboBox()
        Me.timingParametersGroupBox = New System.Windows.Forms.GroupBox()
        Me.generalGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageGroupBox = New System.Windows.Forms.GroupBox()
        Me.messageTextBox = New System.Windows.Forms.RichTextBox()
        Me.filteredWaveformDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.filteredWaveformDataGridView = New System.Windows.Forms.DataGridView()
        Me.spectrumDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.spectrumDataGridView = New System.Windows.Forms.DataGridView()
        Me.buttonsGroupBox = New System.Windows.Forms.GroupBox()
        CType(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lowOrHighPassCutoffFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bandpassOrBandstopWidthNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bandpassOrStopCenterFrequencyNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.firTapsNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iirOrderNumeric, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.functionsGroupBox.SuspendLayout()
        Me.filterParametersGroupBox.SuspendLayout()
        Me.timingParametersGroupBox.SuspendLayout()
        Me.generalGroupBox.SuspendLayout()
        Me.messageGroupBox.SuspendLayout()
        Me.filteredWaveformDataGroupBox.SuspendLayout()
        CType(Me.filteredWaveformDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.spectrumDataGroupBox.SuspendLayout()
        CType(Me.spectrumDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.buttonsGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'channelLabel
        '
        Me.channelLabel.AutoSize = True
        Me.channelLabel.Location = New System.Drawing.Point(6, 50)
        Me.channelLabel.Name = "channelLabel"
        Me.channelLabel.Size = New System.Drawing.Size(80, 13)
        Me.channelLabel.TabIndex = 8
        Me.channelLabel.Text = "Channel Name:"
        '
        'recordLengthMinLabel
        '
        Me.recordLengthMinLabel.AutoSize = True
        Me.recordLengthMinLabel.Location = New System.Drawing.Point(6, 23)
        Me.recordLengthMinLabel.Name = "recordLengthMinLabel"
        Me.recordLengthMinLabel.Size = New System.Drawing.Size(125, 13)
        Me.recordLengthMinLabel.TabIndex = 2
        Me.recordLengthMinLabel.Text = "Minimum Record Length:"
        '
        'sampleRateMinLabel
        '
        Me.sampleRateMinLabel.AutoSize = True
        Me.sampleRateMinLabel.Location = New System.Drawing.Point(6, 49)
        Me.sampleRateMinLabel.Name = "sampleRateMinLabel"
        Me.sampleRateMinLabel.Size = New System.Drawing.Size(115, 13)
        Me.sampleRateMinLabel.TabIndex = 3
        Me.sampleRateMinLabel.Text = "Minimum Sample Rate:"
        '
        'lowOrHighPassCutoffFrequencyLabel
        '
        Me.lowOrHighPassCutoffFrequencyLabel.AutoSize = True
        Me.lowOrHighPassCutoffFrequencyLabel.Location = New System.Drawing.Point(6, 50)
        Me.lowOrHighPassCutoffFrequencyLabel.Name = "lowOrHighPassCutoffFrequencyLabel"
        Me.lowOrHighPassCutoffFrequencyLabel.Size = New System.Drawing.Size(185, 13)
        Me.lowOrHighPassCutoffFrequencyLabel.TabIndex = 8
        Me.lowOrHighPassCutoffFrequencyLabel.Text = "Low/Highpass Cutoff Frequency (Hz):"
        '
        'bandpassOrBandstopWidthLabel
        '
        Me.bandpassOrBandstopWidthLabel.AutoSize = True
        Me.bandpassOrBandstopWidthLabel.Location = New System.Drawing.Point(6, 102)
        Me.bandpassOrBandstopWidthLabel.Name = "bandpassOrBandstopWidthLabel"
        Me.bandpassOrBandstopWidthLabel.Size = New System.Drawing.Size(138, 13)
        Me.bandpassOrBandstopWidthLabel.TabIndex = 10
        Me.bandpassOrBandstopWidthLabel.Text = "Bandpass/Bandstop Width:"
        '
        'bandpassOrStopCenterFrequencyLabel
        '
        Me.bandpassOrStopCenterFrequencyLabel.AutoSize = True
        Me.bandpassOrStopCenterFrequencyLabel.Location = New System.Drawing.Point(6, 76)
        Me.bandpassOrStopCenterFrequencyLabel.Name = "bandpassOrStopCenterFrequencyLabel"
        Me.bandpassOrStopCenterFrequencyLabel.Size = New System.Drawing.Size(193, 13)
        Me.bandpassOrStopCenterFrequencyLabel.TabIndex = 9
        Me.bandpassOrStopCenterFrequencyLabel.Text = "Bandpass/Stop Center Frequency (Hz):"
        '
        'firTapsLabel
        '
        Me.firTapsLabel.AutoSize = True
        Me.firTapsLabel.Location = New System.Drawing.Point(6, 128)
        Me.firTapsLabel.Name = "firTapsLabel"
        Me.firTapsLabel.Size = New System.Drawing.Size(54, 13)
        Me.firTapsLabel.TabIndex = 11
        Me.firTapsLabel.Text = "FIR Taps:"
        '
        'iirOrderLabel
        '
        Me.iirOrderLabel.AutoSize = True
        Me.iirOrderLabel.Location = New System.Drawing.Point(6, 181)
        Me.iirOrderLabel.Name = "iirOrderLabel"
        Me.iirOrderLabel.Size = New System.Drawing.Size(53, 13)
        Me.iirOrderLabel.TabIndex = 13
        Me.iirOrderLabel.Text = "IIR Order:"
        '
        'fftFunctionLabel
        '
        Me.fftFunctionLabel.AutoSize = True
        Me.fftFunctionLabel.Location = New System.Drawing.Point(6, 50)
        Me.fftFunctionLabel.Name = "fftFunctionLabel"
        Me.fftFunctionLabel.Size = New System.Drawing.Size(73, 13)
        Me.fftFunctionLabel.TabIndex = 3
        Me.fftFunctionLabel.Text = "FFT Function:"
        '
        'filterTypeLabel
        '
        Me.filterTypeLabel.AutoSize = True
        Me.filterTypeLabel.Location = New System.Drawing.Point(6, 23)
        Me.filterTypeLabel.Name = "filterTypeLabel"
        Me.filterTypeLabel.Size = New System.Drawing.Size(59, 13)
        Me.filterTypeLabel.TabIndex = 7
        Me.filterTypeLabel.Text = "Filter Type:"
        '
        'firWindowLabel
        '
        Me.firWindowLabel.AutoSize = True
        Me.firWindowLabel.Location = New System.Drawing.Point(6, 154)
        Me.firWindowLabel.Name = "firWindowLabel"
        Me.firWindowLabel.Size = New System.Drawing.Size(69, 13)
        Me.firWindowLabel.TabIndex = 12
        Me.firWindowLabel.Text = "FIR Window:"
        '
        'filterLabel
        '
        Me.filterLabel.AutoSize = True
        Me.filterLabel.Location = New System.Drawing.Point(6, 23)
        Me.filterLabel.Name = "filterLabel"
        Me.filterLabel.Size = New System.Drawing.Size(32, 13)
        Me.filterLabel.TabIndex = 2
        Me.filterLabel.Text = "Filter:"
        '
        'resourceNameLabel
        '
        Me.resourceNameLabel.AutoSize = True
        Me.resourceNameLabel.Location = New System.Drawing.Point(6, 23)
        Me.resourceNameLabel.Name = "resourceNameLabel"
        Me.resourceNameLabel.Size = New System.Drawing.Size(87, 13)
        Me.resourceNameLabel.TabIndex = 7
        Me.resourceNameLabel.Text = "Resource Name:"
        '
        'recordLengthMinNumeric
        '
        Me.recordLengthMinNumeric.Location = New System.Drawing.Point(205, 19)
        Me.recordLengthMinNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.recordLengthMinNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.recordLengthMinNumeric.Name = "recordLengthMinNumeric"
        Me.recordLengthMinNumeric.Size = New System.Drawing.Size(120, 20)
        Me.recordLengthMinNumeric.TabIndex = 0
        Me.recordLengthMinNumeric.Value = New Decimal(New Integer() {4096, 0, 0, 0})
        '
        'sampleRateMinNumeric
        '
        Me.sampleRateMinNumeric.DecimalPlaces = 2
        Me.sampleRateMinNumeric.Increment = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.sampleRateMinNumeric.Location = New System.Drawing.Point(205, 45)
        Me.sampleRateMinNumeric.Maximum = New Decimal(New Integer() {-1247518720, 1073741819, 0, 0})
        Me.sampleRateMinNumeric.Minimum = New Decimal(New Integer() {-1247518720, 1073741819, 0, -2147483648})
        Me.sampleRateMinNumeric.Name = "sampleRateMinNumeric"
        Me.sampleRateMinNumeric.Size = New System.Drawing.Size(120, 20)
        Me.sampleRateMinNumeric.TabIndex = 1
        Me.sampleRateMinNumeric.Value = New Decimal(New Integer() {10000000, 0, 0, 0})
        '
        'lowOrHighPassCutoffFrequencyNumeric
        '
        Me.lowOrHighPassCutoffFrequencyNumeric.DecimalPlaces = 2
        Me.lowOrHighPassCutoffFrequencyNumeric.Location = New System.Drawing.Point(205, 46)
        Me.lowOrHighPassCutoffFrequencyNumeric.Maximum = New Decimal(New Integer() {-1247518720, 1073741819, 0, 0})
        Me.lowOrHighPassCutoffFrequencyNumeric.Minimum = New Decimal(New Integer() {-1247518720, 1073741819, 0, -2147483648})
        Me.lowOrHighPassCutoffFrequencyNumeric.Name = "lowOrHighPassCutoffFrequencyNumeric"
        Me.lowOrHighPassCutoffFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.lowOrHighPassCutoffFrequencyNumeric.TabIndex = 1
        Me.lowOrHighPassCutoffFrequencyNumeric.Value = New Decimal(New Integer() {2500000, 0, 0, 0})
        '
        'bandpassOrBandstopWidthNumeric
        '
        Me.bandpassOrBandstopWidthNumeric.DecimalPlaces = 2
        Me.bandpassOrBandstopWidthNumeric.Location = New System.Drawing.Point(205, 98)
        Me.bandpassOrBandstopWidthNumeric.Maximum = New Decimal(New Integer() {-1247518720, 1073741819, 0, 0})
        Me.bandpassOrBandstopWidthNumeric.Minimum = New Decimal(New Integer() {-1247518720, 1073741819, 0, -2147483648})
        Me.bandpassOrBandstopWidthNumeric.Name = "bandpassOrBandstopWidthNumeric"
        Me.bandpassOrBandstopWidthNumeric.Size = New System.Drawing.Size(120, 20)
        Me.bandpassOrBandstopWidthNumeric.TabIndex = 3
        Me.bandpassOrBandstopWidthNumeric.Value = New Decimal(New Integer() {100000, 0, 0, 0})
        '
        'bandpassOrStopCenterFrequencyNumeric
        '
        Me.bandpassOrStopCenterFrequencyNumeric.DecimalPlaces = 2
        Me.bandpassOrStopCenterFrequencyNumeric.Location = New System.Drawing.Point(205, 72)
        Me.bandpassOrStopCenterFrequencyNumeric.Maximum = New Decimal(New Integer() {-1247518720, 1073741819, 0, 0})
        Me.bandpassOrStopCenterFrequencyNumeric.Minimum = New Decimal(New Integer() {-1247518720, 1073741819, 0, -2147483648})
        Me.bandpassOrStopCenterFrequencyNumeric.Name = "bandpassOrStopCenterFrequencyNumeric"
        Me.bandpassOrStopCenterFrequencyNumeric.Size = New System.Drawing.Size(120, 20)
        Me.bandpassOrStopCenterFrequencyNumeric.TabIndex = 2
        Me.bandpassOrStopCenterFrequencyNumeric.Value = New Decimal(New Integer() {500000, 0, 0, 0})
        '
        'firTapsNumeric
        '
        Me.firTapsNumeric.Location = New System.Drawing.Point(205, 124)
        Me.firTapsNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.firTapsNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.firTapsNumeric.Name = "firTapsNumeric"
        Me.firTapsNumeric.Size = New System.Drawing.Size(120, 20)
        Me.firTapsNumeric.TabIndex = 4
        Me.firTapsNumeric.Value = New Decimal(New Integer() {25, 0, 0, 0})
        '
        'iirOrderNumeric
        '
        Me.iirOrderNumeric.Location = New System.Drawing.Point(205, 177)
        Me.iirOrderNumeric.Maximum = New Decimal(New Integer() {-1, -1, -1, 0})
        Me.iirOrderNumeric.Minimum = New Decimal(New Integer() {-1, -1, -1, -2147483648})
        Me.iirOrderNumeric.Name = "iirOrderNumeric"
        Me.iirOrderNumeric.Size = New System.Drawing.Size(120, 20)
        Me.iirOrderNumeric.TabIndex = 6
        Me.iirOrderNumeric.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'channelTextBox
        '
        Me.channelTextBox.Location = New System.Drawing.Point(205, 46)
        Me.channelTextBox.Name = "channelTextBox"
        Me.channelTextBox.Size = New System.Drawing.Size(120, 20)
        Me.channelTextBox.TabIndex = 1
        Me.channelTextBox.Text = "0"
        '
        'acquireButton
        '
        Me.acquireButton.Location = New System.Drawing.Point(127, 19)
        Me.acquireButton.Name = "acquireButton"
        Me.acquireButton.Size = New System.Drawing.Size(75, 23)
        Me.acquireButton.TabIndex = 0
        Me.acquireButton.Text = "&Acquire"
        Me.acquireButton.UseVisualStyleBackColor = True
        '
        'stopButton
        '
        Me.stopButton.Location = New System.Drawing.Point(208, 19)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(75, 23)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "&Stop"
        Me.stopButton.UseVisualStyleBackColor = True
        '
        'fftFunctionComboBox
        '
        Me.fftFunctionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.fftFunctionComboBox.Location = New System.Drawing.Point(146, 46)
        Me.fftFunctionComboBox.Name = "fftFunctionComboBox"
        Me.fftFunctionComboBox.Size = New System.Drawing.Size(179, 21)
        Me.fftFunctionComboBox.TabIndex = 1
        '
        'functionsGroupBox
        '
        Me.functionsGroupBox.Controls.Add(Me.fftFunctionComboBox)
        Me.functionsGroupBox.Controls.Add(Me.filterComboBox)
        Me.functionsGroupBox.Controls.Add(Me.filterLabel)
        Me.functionsGroupBox.Controls.Add(Me.fftFunctionLabel)
        Me.functionsGroupBox.Location = New System.Drawing.Point(12, 182)
        Me.functionsGroupBox.Name = "functionsGroupBox"
        Me.functionsGroupBox.Size = New System.Drawing.Size(331, 76)
        Me.functionsGroupBox.TabIndex = 2
        Me.functionsGroupBox.TabStop = False
        Me.functionsGroupBox.Text = "Functions"
        '
        'filterComboBox
        '
        Me.filterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.filterComboBox.Location = New System.Drawing.Point(146, 19)
        Me.filterComboBox.Name = "filterComboBox"
        Me.filterComboBox.Size = New System.Drawing.Size(179, 21)
        Me.filterComboBox.TabIndex = 0
        '
        'filterTypeComboBox
        '
        Me.filterTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.filterTypeComboBox.Location = New System.Drawing.Point(205, 19)
        Me.filterTypeComboBox.Name = "filterTypeComboBox"
        Me.filterTypeComboBox.Size = New System.Drawing.Size(120, 21)
        Me.filterTypeComboBox.TabIndex = 0
        '
        'filterParametersGroupBox
        '
        Me.filterParametersGroupBox.Controls.Add(Me.filterTypeComboBox)
        Me.filterParametersGroupBox.Controls.Add(Me.firWindowComboBox)
        Me.filterParametersGroupBox.Controls.Add(Me.filterTypeLabel)
        Me.filterParametersGroupBox.Controls.Add(Me.lowOrHighPassCutoffFrequencyNumeric)
        Me.filterParametersGroupBox.Controls.Add(Me.bandpassOrBandstopWidthNumeric)
        Me.filterParametersGroupBox.Controls.Add(Me.bandpassOrStopCenterFrequencyNumeric)
        Me.filterParametersGroupBox.Controls.Add(Me.firTapsNumeric)
        Me.filterParametersGroupBox.Controls.Add(Me.iirOrderNumeric)
        Me.filterParametersGroupBox.Controls.Add(Me.lowOrHighPassCutoffFrequencyLabel)
        Me.filterParametersGroupBox.Controls.Add(Me.bandpassOrBandstopWidthLabel)
        Me.filterParametersGroupBox.Controls.Add(Me.bandpassOrStopCenterFrequencyLabel)
        Me.filterParametersGroupBox.Controls.Add(Me.firTapsLabel)
        Me.filterParametersGroupBox.Controls.Add(Me.iirOrderLabel)
        Me.filterParametersGroupBox.Controls.Add(Me.firWindowLabel)
        Me.filterParametersGroupBox.Location = New System.Drawing.Point(12, 269)
        Me.filterParametersGroupBox.Name = "filterParametersGroupBox"
        Me.filterParametersGroupBox.Size = New System.Drawing.Size(331, 207)
        Me.filterParametersGroupBox.TabIndex = 3
        Me.filterParametersGroupBox.TabStop = False
        Me.filterParametersGroupBox.Text = "Filter Parameters"
        '
        'firWindowComboBox
        '
        Me.firWindowComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.firWindowComboBox.Location = New System.Drawing.Point(205, 150)
        Me.firWindowComboBox.Name = "firWindowComboBox"
        Me.firWindowComboBox.Size = New System.Drawing.Size(120, 21)
        Me.firWindowComboBox.TabIndex = 5
        '
        'resourceNameComboBox
        '
        Me.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.resourceNameComboBox.Location = New System.Drawing.Point(205, 19)
        Me.resourceNameComboBox.Name = "resourceNameComboBox"
        Me.resourceNameComboBox.Size = New System.Drawing.Size(120, 21)
        Me.resourceNameComboBox.TabIndex = 0
        '
        'timingParametersGroupBox
        '
        Me.timingParametersGroupBox.Controls.Add(Me.recordLengthMinNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleRateMinNumeric)
        Me.timingParametersGroupBox.Controls.Add(Me.recordLengthMinLabel)
        Me.timingParametersGroupBox.Controls.Add(Me.sampleRateMinLabel)
        Me.timingParametersGroupBox.Location = New System.Drawing.Point(12, 98)
        Me.timingParametersGroupBox.Name = "timingParametersGroupBox"
        Me.timingParametersGroupBox.Size = New System.Drawing.Size(331, 73)
        Me.timingParametersGroupBox.TabIndex = 1
        Me.timingParametersGroupBox.TabStop = False
        Me.timingParametersGroupBox.Text = "Timing Parameters"
        '
        'generalGroupBox
        '
        Me.generalGroupBox.Controls.Add(Me.resourceNameComboBox)
        Me.generalGroupBox.Controls.Add(Me.channelLabel)
        Me.generalGroupBox.Controls.Add(Me.resourceNameLabel)
        Me.generalGroupBox.Controls.Add(Me.channelTextBox)
        Me.generalGroupBox.Location = New System.Drawing.Point(12, 12)
        Me.generalGroupBox.Name = "generalGroupBox"
        Me.generalGroupBox.Size = New System.Drawing.Size(331, 75)
        Me.generalGroupBox.TabIndex = 0
        Me.generalGroupBox.TabStop = False
        Me.generalGroupBox.Text = "General"
        '
        'messageGroupBox
        '
        Me.messageGroupBox.Controls.Add(Me.messageTextBox)
        Me.messageGroupBox.Location = New System.Drawing.Point(12, 487)
        Me.messageGroupBox.Name = "messageGroupBox"
        Me.messageGroupBox.Size = New System.Drawing.Size(331, 55)
        Me.messageGroupBox.TabIndex = 4
        Me.messageGroupBox.TabStop = False
        Me.messageGroupBox.Text = "Message"
        '
        'messageTextBox
        '
        Me.messageTextBox.Location = New System.Drawing.Point(9, 19)
        Me.messageTextBox.Name = "messageTextBox"
        Me.messageTextBox.ReadOnly = True
        Me.messageTextBox.Size = New System.Drawing.Size(316, 30)
        Me.messageTextBox.TabIndex = 0
        Me.messageTextBox.Text = ""
        '
        'filteredWaveformDataGroupBox
        '
        Me.filteredWaveformDataGroupBox.Controls.Add(Me.filteredWaveformDataGridView)
        Me.filteredWaveformDataGroupBox.Location = New System.Drawing.Point(349, 12)
        Me.filteredWaveformDataGroupBox.Name = "filteredWaveformDataGroupBox"
        Me.filteredWaveformDataGroupBox.Size = New System.Drawing.Size(202, 464)
        Me.filteredWaveformDataGroupBox.TabIndex = 5
        Me.filteredWaveformDataGroupBox.TabStop = False
        Me.filteredWaveformDataGroupBox.Text = "Filtered Waveform Data"
        '
        'filteredWaveformDataGridView
        '
        Me.filteredWaveformDataGridView.AllowUserToAddRows = False
        Me.filteredWaveformDataGridView.AllowUserToDeleteRows = False
        Me.filteredWaveformDataGridView.AllowUserToResizeRows = False
        Me.filteredWaveformDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.filteredWaveformDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.filteredWaveformDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.filteredWaveformDataGridView.DefaultCellStyle = DataGridViewCellStyle2
        Me.filteredWaveformDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.filteredWaveformDataGridView.Name = "filteredWaveformDataGridView"
        Me.filteredWaveformDataGridView.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.filteredWaveformDataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.filteredWaveformDataGridView.RowHeadersVisible = False
        Me.filteredWaveformDataGridView.RowHeadersWidth = 15
        Me.filteredWaveformDataGridView.RowTemplate.Height = 24
        Me.filteredWaveformDataGridView.Size = New System.Drawing.Size(190, 439)
        Me.filteredWaveformDataGridView.StandardTab = True
        Me.filteredWaveformDataGridView.TabIndex = 0
        '
        'spectrumDataGroupBox
        '
        Me.spectrumDataGroupBox.Controls.Add(Me.spectrumDataGridView)
        Me.spectrumDataGroupBox.Location = New System.Drawing.Point(557, 12)
        Me.spectrumDataGroupBox.Name = "spectrumDataGroupBox"
        Me.spectrumDataGroupBox.Size = New System.Drawing.Size(202, 464)
        Me.spectrumDataGroupBox.TabIndex = 6
        Me.spectrumDataGroupBox.TabStop = False
        Me.spectrumDataGroupBox.Text = "Spectrum Data"
        '
        'spectrumDataGridView
        '
        Me.spectrumDataGridView.AllowUserToAddRows = False
        Me.spectrumDataGridView.AllowUserToDeleteRows = False
        Me.spectrumDataGridView.AllowUserToResizeRows = False
        Me.spectrumDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.spectrumDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.spectrumDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.spectrumDataGridView.DefaultCellStyle = DataGridViewCellStyle5
        Me.spectrumDataGridView.Location = New System.Drawing.Point(6, 19)
        Me.spectrumDataGridView.Name = "spectrumDataGridView"
        Me.spectrumDataGridView.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.spectrumDataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.spectrumDataGridView.RowHeadersVisible = False
        Me.spectrumDataGridView.RowHeadersWidth = 15
        Me.spectrumDataGridView.RowTemplate.Height = 24
        Me.spectrumDataGridView.Size = New System.Drawing.Size(190, 439)
        Me.spectrumDataGridView.StandardTab = True
        Me.spectrumDataGridView.TabIndex = 0
        '
        'buttonsGroupBox
        '
        Me.buttonsGroupBox.Controls.Add(Me.stopButton)
        Me.buttonsGroupBox.Controls.Add(Me.acquireButton)
        Me.buttonsGroupBox.Location = New System.Drawing.Point(349, 487)
        Me.buttonsGroupBox.Name = "buttonsGroupBox"
        Me.buttonsGroupBox.Size = New System.Drawing.Size(410, 55)
        Me.buttonsGroupBox.TabIndex = 7
        Me.buttonsGroupBox.TabStop = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(771, 555)
        Me.Controls.Add(Me.buttonsGroupBox)
        Me.Controls.Add(Me.spectrumDataGroupBox)
        Me.Controls.Add(Me.filteredWaveformDataGroupBox)
        Me.Controls.Add(Me.messageGroupBox)
        Me.Controls.Add(Me.generalGroupBox)
        Me.Controls.Add(Me.timingParametersGroupBox)
        Me.Controls.Add(Me.functionsGroupBox)
        Me.Controls.Add(Me.filterParametersGroupBox)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Digital Filtering"
        CType(Me.recordLengthMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sampleRateMinNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lowOrHighPassCutoffFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bandpassOrBandstopWidthNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bandpassOrStopCenterFrequencyNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.firTapsNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iirOrderNumeric, System.ComponentModel.ISupportInitialize).EndInit()
        Me.functionsGroupBox.ResumeLayout(False)
        Me.functionsGroupBox.PerformLayout()
        Me.filterParametersGroupBox.ResumeLayout(False)
        Me.filterParametersGroupBox.PerformLayout()
        Me.timingParametersGroupBox.ResumeLayout(False)
        Me.timingParametersGroupBox.PerformLayout()
        Me.generalGroupBox.ResumeLayout(False)
        Me.generalGroupBox.PerformLayout()
        Me.messageGroupBox.ResumeLayout(False)
        Me.filteredWaveformDataGroupBox.ResumeLayout(False)
        CType(Me.filteredWaveformDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.spectrumDataGroupBox.ResumeLayout(False)
        CType(Me.spectrumDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.buttonsGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private channelLabel As System.Windows.Forms.Label
    Private recordLengthMinLabel As System.Windows.Forms.Label
    Private sampleRateMinLabel As System.Windows.Forms.Label
    Private lowOrHighPassCutoffFrequencyLabel As System.Windows.Forms.Label
    Private bandpassOrBandstopWidthLabel As System.Windows.Forms.Label
    Private bandpassOrStopCenterFrequencyLabel As System.Windows.Forms.Label
    Private firTapsLabel As System.Windows.Forms.Label
    Private iirOrderLabel As System.Windows.Forms.Label
    Private fftFunctionLabel As System.Windows.Forms.Label
    Private filterTypeLabel As System.Windows.Forms.Label
    Private firWindowLabel As System.Windows.Forms.Label
    Private filterLabel As System.Windows.Forms.Label
    Private resourceNameLabel As System.Windows.Forms.Label
    Private recordLengthMinNumeric As System.Windows.Forms.NumericUpDown
    Private sampleRateMinNumeric As System.Windows.Forms.NumericUpDown
    Private lowOrHighPassCutoffFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private bandpassOrBandstopWidthNumeric As System.Windows.Forms.NumericUpDown
    Private bandpassOrStopCenterFrequencyNumeric As System.Windows.Forms.NumericUpDown
    Private firTapsNumeric As System.Windows.Forms.NumericUpDown
    Private iirOrderNumeric As System.Windows.Forms.NumericUpDown
    Private channelTextBox As System.Windows.Forms.TextBox
    Private WithEvents acquireButton As System.Windows.Forms.Button
    Private WithEvents stopButton As System.Windows.Forms.Button
    Private fftFunctionComboBox As System.Windows.Forms.ComboBox
    Private filterTypeComboBox As System.Windows.Forms.ComboBox
    Private firWindowComboBox As System.Windows.Forms.ComboBox
    Private filterComboBox As System.Windows.Forms.ComboBox
    Private resourceNameComboBox As System.Windows.Forms.ComboBox
    Private timingParametersGroupBox As System.Windows.Forms.GroupBox
    Private functionsGroupBox As System.Windows.Forms.GroupBox
    Private filterParametersGroupBox As System.Windows.Forms.GroupBox
    Private generalGroupBox As System.Windows.Forms.GroupBox
    Private messageGroupBox As System.Windows.Forms.GroupBox
    Private messageTextBox As System.Windows.Forms.RichTextBox
    Private filteredWaveformDataGroupBox As System.Windows.Forms.GroupBox
    Private filteredWaveformDataGridView As System.Windows.Forms.DataGridView
    Private spectrumDataGroupBox As System.Windows.Forms.GroupBox
    Private spectrumDataGridView As System.Windows.Forms.DataGridView
    Private buttonsGroupBox As System.Windows.Forms.GroupBox


End Class
