namespace NationalInstruments.Examples.DigitalFiltering
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.channelLabel = new System.Windows.Forms.Label();
            this.recordLengthMinLabel = new System.Windows.Forms.Label();
            this.sampleRateMinLabel = new System.Windows.Forms.Label();
            this.lowOrHighPassCutoffFrequencyLabel = new System.Windows.Forms.Label();
            this.bandpassOrBandstopWidthLabel = new System.Windows.Forms.Label();
            this.bandpassOrStopCenterFrequencyLabel = new System.Windows.Forms.Label();
            this.firTapsLabel = new System.Windows.Forms.Label();
            this.iirOrderLabel = new System.Windows.Forms.Label();
            this.fftFunctionLabel = new System.Windows.Forms.Label();
            this.filterTypeLabel = new System.Windows.Forms.Label();
            this.firWindowLabel = new System.Windows.Forms.Label();
            this.filterLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.recordLengthMinNumeric = new System.Windows.Forms.NumericUpDown();
            this.sampleRateMinNumeric = new System.Windows.Forms.NumericUpDown();
            this.lowOrHighPassCutoffFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.bandpassOrBandstopWidthNumeric = new System.Windows.Forms.NumericUpDown();
            this.bandpassOrStopCenterFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.firTapsNumeric = new System.Windows.Forms.NumericUpDown();
            this.iirOrderNumeric = new System.Windows.Forms.NumericUpDown();
            this.channelTextBox = new System.Windows.Forms.TextBox();
            this.acquireButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.fftFunctionComboBox = new System.Windows.Forms.ComboBox();
            this.functionsGroupBox = new System.Windows.Forms.GroupBox();
            this.filterComboBox = new System.Windows.Forms.ComboBox();
            this.filterTypeComboBox = new System.Windows.Forms.ComboBox();
            this.filterParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.firWindowComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.messageGroupBox = new System.Windows.Forms.GroupBox();
            this.messageTextBox = new System.Windows.Forms.RichTextBox();
            this.filteredWaveformDataGroupBox = new System.Windows.Forms.GroupBox();
            this.filteredWaveformDataGridView = new System.Windows.Forms.DataGridView();
            this.spectrumDataGroupBox = new System.Windows.Forms.GroupBox();
            this.spectrumDataGridView = new System.Windows.Forms.DataGridView();
            this.buttonsGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.recordLengthMinNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateMinNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lowOrHighPassCutoffFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandpassOrBandstopWidthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandpassOrStopCenterFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.firTapsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iirOrderNumeric)).BeginInit();
            this.functionsGroupBox.SuspendLayout();
            this.filterParametersGroupBox.SuspendLayout();
            this.timingParametersGroupBox.SuspendLayout();
            this.generalGroupBox.SuspendLayout();
            this.messageGroupBox.SuspendLayout();
            this.filteredWaveformDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filteredWaveformDataGridView)).BeginInit();
            this.spectrumDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumDataGridView)).BeginInit();
            this.buttonsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelLabel
            // 
            this.channelLabel.AutoSize = true;
            this.channelLabel.Location = new System.Drawing.Point(6, 50);
            this.channelLabel.Name = "channelLabel";
            this.channelLabel.Size = new System.Drawing.Size(80, 13);
            this.channelLabel.TabIndex = 8;
            this.channelLabel.Text = "Channel Name:";
            // 
            // recordLengthMinLabel
            // 
            this.recordLengthMinLabel.AutoSize = true;
            this.recordLengthMinLabel.Location = new System.Drawing.Point(6, 23);
            this.recordLengthMinLabel.Name = "recordLengthMinLabel";
            this.recordLengthMinLabel.Size = new System.Drawing.Size(125, 13);
            this.recordLengthMinLabel.TabIndex = 2;
            this.recordLengthMinLabel.Text = "Minimum Record Length:";
            // 
            // sampleRateMinLabel
            // 
            this.sampleRateMinLabel.AutoSize = true;
            this.sampleRateMinLabel.Location = new System.Drawing.Point(6, 49);
            this.sampleRateMinLabel.Name = "sampleRateMinLabel";
            this.sampleRateMinLabel.Size = new System.Drawing.Size(115, 13);
            this.sampleRateMinLabel.TabIndex = 3;
            this.sampleRateMinLabel.Text = "Minimum Sample Rate:";
            // 
            // lowOrHighPassCutoffFrequencyLabel
            // 
            this.lowOrHighPassCutoffFrequencyLabel.AutoSize = true;
            this.lowOrHighPassCutoffFrequencyLabel.Location = new System.Drawing.Point(6, 50);
            this.lowOrHighPassCutoffFrequencyLabel.Name = "lowOrHighPassCutoffFrequencyLabel";
            this.lowOrHighPassCutoffFrequencyLabel.Size = new System.Drawing.Size(185, 13);
            this.lowOrHighPassCutoffFrequencyLabel.TabIndex = 8;
            this.lowOrHighPassCutoffFrequencyLabel.Text = "Low/Highpass Cutoff Frequency (Hz):";
            // 
            // bandpassOrBandstopWidthLabel
            // 
            this.bandpassOrBandstopWidthLabel.AutoSize = true;
            this.bandpassOrBandstopWidthLabel.Location = new System.Drawing.Point(6, 102);
            this.bandpassOrBandstopWidthLabel.Name = "bandpassOrBandstopWidthLabel";
            this.bandpassOrBandstopWidthLabel.Size = new System.Drawing.Size(138, 13);
            this.bandpassOrBandstopWidthLabel.TabIndex = 10;
            this.bandpassOrBandstopWidthLabel.Text = "Bandpass/Bandstop Width:";
            // 
            // bandpassOrStopCenterFrequencyLabel
            // 
            this.bandpassOrStopCenterFrequencyLabel.AutoSize = true;
            this.bandpassOrStopCenterFrequencyLabel.Location = new System.Drawing.Point(6, 76);
            this.bandpassOrStopCenterFrequencyLabel.Name = "bandpassOrStopCenterFrequencyLabel";
            this.bandpassOrStopCenterFrequencyLabel.Size = new System.Drawing.Size(193, 13);
            this.bandpassOrStopCenterFrequencyLabel.TabIndex = 9;
            this.bandpassOrStopCenterFrequencyLabel.Text = "Bandpass/Stop Center Frequency (Hz):";
            // 
            // firTapsLabel
            // 
            this.firTapsLabel.AutoSize = true;
            this.firTapsLabel.Location = new System.Drawing.Point(6, 128);
            this.firTapsLabel.Name = "firTapsLabel";
            this.firTapsLabel.Size = new System.Drawing.Size(54, 13);
            this.firTapsLabel.TabIndex = 11;
            this.firTapsLabel.Text = "FIR Taps:";
            // 
            // iirOrderLabel
            // 
            this.iirOrderLabel.AutoSize = true;
            this.iirOrderLabel.Location = new System.Drawing.Point(6, 181);
            this.iirOrderLabel.Name = "iirOrderLabel";
            this.iirOrderLabel.Size = new System.Drawing.Size(53, 13);
            this.iirOrderLabel.TabIndex = 13;
            this.iirOrderLabel.Text = "IIR Order:";
            // 
            // fftFunctionLabel
            // 
            this.fftFunctionLabel.AutoSize = true;
            this.fftFunctionLabel.Location = new System.Drawing.Point(6, 50);
            this.fftFunctionLabel.Name = "fftFunctionLabel";
            this.fftFunctionLabel.Size = new System.Drawing.Size(73, 13);
            this.fftFunctionLabel.TabIndex = 3;
            this.fftFunctionLabel.Text = "FFT Function:";
            // 
            // filterTypeLabel
            // 
            this.filterTypeLabel.AutoSize = true;
            this.filterTypeLabel.Location = new System.Drawing.Point(6, 23);
            this.filterTypeLabel.Name = "filterTypeLabel";
            this.filterTypeLabel.Size = new System.Drawing.Size(59, 13);
            this.filterTypeLabel.TabIndex = 7;
            this.filterTypeLabel.Text = "Filter Type:";
            // 
            // firWindowLabel
            // 
            this.firWindowLabel.AutoSize = true;
            this.firWindowLabel.Location = new System.Drawing.Point(6, 154);
            this.firWindowLabel.Name = "firWindowLabel";
            this.firWindowLabel.Size = new System.Drawing.Size(69, 13);
            this.firWindowLabel.TabIndex = 12;
            this.firWindowLabel.Text = "FIR Window:";
            // 
            // filterLabel
            // 
            this.filterLabel.AutoSize = true;
            this.filterLabel.Location = new System.Drawing.Point(6, 23);
            this.filterLabel.Name = "filterLabel";
            this.filterLabel.Size = new System.Drawing.Size(32, 13);
            this.filterLabel.TabIndex = 2;
            this.filterLabel.Text = "Filter:";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 23);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 7;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // recordLengthMinNumeric
            // 
            this.recordLengthMinNumeric.Location = new System.Drawing.Point(205, 19);
            this.recordLengthMinNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.recordLengthMinNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.recordLengthMinNumeric.Name = "recordLengthMinNumeric";
            this.recordLengthMinNumeric.Size = new System.Drawing.Size(120, 20);
            this.recordLengthMinNumeric.TabIndex = 0;
            this.recordLengthMinNumeric.Value = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            // 
            // sampleRateMinNumeric
            // 
            this.sampleRateMinNumeric.DecimalPlaces = 2;
            this.sampleRateMinNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.sampleRateMinNumeric.Location = new System.Drawing.Point(205, 45);
            this.sampleRateMinNumeric.Maximum = new decimal(new int[] {
            -1247518720,
            1073741819,
            0,
            0});
            this.sampleRateMinNumeric.Minimum = new decimal(new int[] {
            -1247518720,
            1073741819,
            0,
            -2147483648});
            this.sampleRateMinNumeric.Name = "sampleRateMinNumeric";
            this.sampleRateMinNumeric.Size = new System.Drawing.Size(120, 20);
            this.sampleRateMinNumeric.TabIndex = 1;
            this.sampleRateMinNumeric.Value = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            // 
            // lowOrHighPassCutoffFrequencyNumeric
            // 
            this.lowOrHighPassCutoffFrequencyNumeric.DecimalPlaces = 2;
            this.lowOrHighPassCutoffFrequencyNumeric.Location = new System.Drawing.Point(205, 46);
            this.lowOrHighPassCutoffFrequencyNumeric.Maximum = new decimal(new int[] {
            -1247518720,
            1073741819,
            0,
            0});
            this.lowOrHighPassCutoffFrequencyNumeric.Minimum = new decimal(new int[] {
            -1247518720,
            1073741819,
            0,
            -2147483648});
            this.lowOrHighPassCutoffFrequencyNumeric.Name = "lowOrHighPassCutoffFrequencyNumeric";
            this.lowOrHighPassCutoffFrequencyNumeric.Size = new System.Drawing.Size(120, 20);
            this.lowOrHighPassCutoffFrequencyNumeric.TabIndex = 1;
            this.lowOrHighPassCutoffFrequencyNumeric.Value = new decimal(new int[] {
            2500000,
            0,
            0,
            0});
            // 
            // bandpassOrBandstopWidthNumeric
            // 
            this.bandpassOrBandstopWidthNumeric.DecimalPlaces = 2;
            this.bandpassOrBandstopWidthNumeric.Location = new System.Drawing.Point(205, 98);
            this.bandpassOrBandstopWidthNumeric.Maximum = new decimal(new int[] {
            -1247518720,
            1073741819,
            0,
            0});
            this.bandpassOrBandstopWidthNumeric.Minimum = new decimal(new int[] {
            -1247518720,
            1073741819,
            0,
            -2147483648});
            this.bandpassOrBandstopWidthNumeric.Name = "bandpassOrBandstopWidthNumeric";
            this.bandpassOrBandstopWidthNumeric.Size = new System.Drawing.Size(120, 20);
            this.bandpassOrBandstopWidthNumeric.TabIndex = 3;
            this.bandpassOrBandstopWidthNumeric.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // bandpassOrStopCenterFrequencyNumeric
            // 
            this.bandpassOrStopCenterFrequencyNumeric.DecimalPlaces = 2;
            this.bandpassOrStopCenterFrequencyNumeric.Location = new System.Drawing.Point(205, 72);
            this.bandpassOrStopCenterFrequencyNumeric.Maximum = new decimal(new int[] {
            -1247518720,
            1073741819,
            0,
            0});
            this.bandpassOrStopCenterFrequencyNumeric.Minimum = new decimal(new int[] {
            -1247518720,
            1073741819,
            0,
            -2147483648});
            this.bandpassOrStopCenterFrequencyNumeric.Name = "bandpassOrStopCenterFrequencyNumeric";
            this.bandpassOrStopCenterFrequencyNumeric.Size = new System.Drawing.Size(120, 20);
            this.bandpassOrStopCenterFrequencyNumeric.TabIndex = 2;
            this.bandpassOrStopCenterFrequencyNumeric.Value = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            // 
            // firTapsNumeric
            // 
            this.firTapsNumeric.Location = new System.Drawing.Point(205, 124);
            this.firTapsNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.firTapsNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.firTapsNumeric.Name = "firTapsNumeric";
            this.firTapsNumeric.Size = new System.Drawing.Size(120, 20);
            this.firTapsNumeric.TabIndex = 4;
            this.firTapsNumeric.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // iirOrderNumeric
            // 
            this.iirOrderNumeric.Location = new System.Drawing.Point(205, 177);
            this.iirOrderNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.iirOrderNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.iirOrderNumeric.Name = "iirOrderNumeric";
            this.iirOrderNumeric.Size = new System.Drawing.Size(120, 20);
            this.iirOrderNumeric.TabIndex = 6;
            this.iirOrderNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // channelTextBox
            // 
            this.channelTextBox.Location = new System.Drawing.Point(205, 46);
            this.channelTextBox.Name = "channelTextBox";
            this.channelTextBox.Size = new System.Drawing.Size(120, 20);
            this.channelTextBox.TabIndex = 1;
            this.channelTextBox.Text = "0";
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(127, 19);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(208, 19);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "&Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // fftFunctionComboBox
            // 
            this.fftFunctionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fftFunctionComboBox.Location = new System.Drawing.Point(146, 46);
            this.fftFunctionComboBox.Name = "fftFunctionComboBox";
            this.fftFunctionComboBox.Size = new System.Drawing.Size(179, 21);
            this.fftFunctionComboBox.TabIndex = 1;
            // 
            // functionsGroupBox
            // 
            this.functionsGroupBox.Controls.Add(this.fftFunctionComboBox);
            this.functionsGroupBox.Controls.Add(this.filterComboBox);
            this.functionsGroupBox.Controls.Add(this.filterLabel);
            this.functionsGroupBox.Controls.Add(this.fftFunctionLabel);
            this.functionsGroupBox.Location = new System.Drawing.Point(12, 182);
            this.functionsGroupBox.Name = "functionsGroupBox";
            this.functionsGroupBox.Size = new System.Drawing.Size(331, 76);
            this.functionsGroupBox.TabIndex = 2;
            this.functionsGroupBox.TabStop = false;
            this.functionsGroupBox.Text = "Functions";
            // 
            // filterComboBox
            // 
            this.filterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterComboBox.Location = new System.Drawing.Point(146, 19);
            this.filterComboBox.Name = "filterComboBox";
            this.filterComboBox.Size = new System.Drawing.Size(179, 21);
            this.filterComboBox.TabIndex = 0;
            // 
            // filterTypeComboBox
            // 
            this.filterTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterTypeComboBox.Location = new System.Drawing.Point(205, 19);
            this.filterTypeComboBox.Name = "filterTypeComboBox";
            this.filterTypeComboBox.Size = new System.Drawing.Size(120, 21);
            this.filterTypeComboBox.TabIndex = 0;
            // 
            // filterParametersGroupBox
            // 
            this.filterParametersGroupBox.Controls.Add(this.filterTypeComboBox);
            this.filterParametersGroupBox.Controls.Add(this.firWindowComboBox);
            this.filterParametersGroupBox.Controls.Add(this.filterTypeLabel);
            this.filterParametersGroupBox.Controls.Add(this.lowOrHighPassCutoffFrequencyNumeric);
            this.filterParametersGroupBox.Controls.Add(this.bandpassOrBandstopWidthNumeric);
            this.filterParametersGroupBox.Controls.Add(this.bandpassOrStopCenterFrequencyNumeric);
            this.filterParametersGroupBox.Controls.Add(this.firTapsNumeric);
            this.filterParametersGroupBox.Controls.Add(this.iirOrderNumeric);
            this.filterParametersGroupBox.Controls.Add(this.lowOrHighPassCutoffFrequencyLabel);
            this.filterParametersGroupBox.Controls.Add(this.bandpassOrBandstopWidthLabel);
            this.filterParametersGroupBox.Controls.Add(this.bandpassOrStopCenterFrequencyLabel);
            this.filterParametersGroupBox.Controls.Add(this.firTapsLabel);
            this.filterParametersGroupBox.Controls.Add(this.iirOrderLabel);
            this.filterParametersGroupBox.Controls.Add(this.firWindowLabel);
            this.filterParametersGroupBox.Location = new System.Drawing.Point(12, 269);
            this.filterParametersGroupBox.Name = "filterParametersGroupBox";
            this.filterParametersGroupBox.Size = new System.Drawing.Size(331, 207);
            this.filterParametersGroupBox.TabIndex = 3;
            this.filterParametersGroupBox.TabStop = false;
            this.filterParametersGroupBox.Text = "Filter Parameters";
            // 
            // firWindowComboBox
            // 
            this.firWindowComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.firWindowComboBox.Location = new System.Drawing.Point(205, 150);
            this.firWindowComboBox.Name = "firWindowComboBox";
            this.firWindowComboBox.Size = new System.Drawing.Size(120, 21);
            this.firWindowComboBox.TabIndex = 5;
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.Location = new System.Drawing.Point(205, 19);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.recordLengthMinNumeric);
            this.timingParametersGroupBox.Controls.Add(this.sampleRateMinNumeric);
            this.timingParametersGroupBox.Controls.Add(this.recordLengthMinLabel);
            this.timingParametersGroupBox.Controls.Add(this.sampleRateMinLabel);
            this.timingParametersGroupBox.Location = new System.Drawing.Point(12, 98);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(331, 73);
            this.timingParametersGroupBox.TabIndex = 1;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.resourceNameComboBox);
            this.generalGroupBox.Controls.Add(this.channelLabel);
            this.generalGroupBox.Controls.Add(this.resourceNameLabel);
            this.generalGroupBox.Controls.Add(this.channelTextBox);
            this.generalGroupBox.Location = new System.Drawing.Point(12, 12);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(331, 75);
            this.generalGroupBox.TabIndex = 0;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            // 
            // messageGroupBox
            // 
            this.messageGroupBox.Controls.Add(this.messageTextBox);
            this.messageGroupBox.Location = new System.Drawing.Point(12, 487);
            this.messageGroupBox.Name = "messageGroupBox";
            this.messageGroupBox.Size = new System.Drawing.Size(331, 55);
            this.messageGroupBox.TabIndex = 4;
            this.messageGroupBox.TabStop = false;
            this.messageGroupBox.Text = "Message";
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(9, 19);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.Size = new System.Drawing.Size(316, 30);
            this.messageTextBox.TabIndex = 0;
            this.messageTextBox.Text = "";
            // 
            // filteredWaveformDataGroupBox
            // 
            this.filteredWaveformDataGroupBox.Controls.Add(this.filteredWaveformDataGridView);
            this.filteredWaveformDataGroupBox.Location = new System.Drawing.Point(349, 12);
            this.filteredWaveformDataGroupBox.Name = "filteredWaveformDataGroupBox";
            this.filteredWaveformDataGroupBox.Size = new System.Drawing.Size(202, 464);
            this.filteredWaveformDataGroupBox.TabIndex = 5;
            this.filteredWaveformDataGroupBox.TabStop = false;
            this.filteredWaveformDataGroupBox.Text = "Filtered Waveform Data";
            // 
            // filteredWaveformDataGridView
            // 
            this.filteredWaveformDataGridView.AllowUserToAddRows = false;
            this.filteredWaveformDataGridView.AllowUserToDeleteRows = false;
            this.filteredWaveformDataGridView.AllowUserToResizeRows = false;
            this.filteredWaveformDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.filteredWaveformDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.filteredWaveformDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.filteredWaveformDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.filteredWaveformDataGridView.Location = new System.Drawing.Point(6, 19);
            this.filteredWaveformDataGridView.Name = "filteredWaveformDataGridView";
            this.filteredWaveformDataGridView.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.filteredWaveformDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.filteredWaveformDataGridView.RowHeadersVisible = false;
            this.filteredWaveformDataGridView.RowHeadersWidth = 15;
            this.filteredWaveformDataGridView.RowTemplate.Height = 24;
            this.filteredWaveformDataGridView.Size = new System.Drawing.Size(190, 439);
            this.filteredWaveformDataGridView.StandardTab = true;
            this.filteredWaveformDataGridView.TabIndex = 0;
            // 
            // spectrumDataGroupBox
            // 
            this.spectrumDataGroupBox.Controls.Add(this.spectrumDataGridView);
            this.spectrumDataGroupBox.Location = new System.Drawing.Point(557, 12);
            this.spectrumDataGroupBox.Name = "spectrumDataGroupBox";
            this.spectrumDataGroupBox.Size = new System.Drawing.Size(202, 464);
            this.spectrumDataGroupBox.TabIndex = 6;
            this.spectrumDataGroupBox.TabStop = false;
            this.spectrumDataGroupBox.Text = "Spectrum Data";
            // 
            // spectrumDataGridView
            // 
            this.spectrumDataGridView.AllowUserToAddRows = false;
            this.spectrumDataGridView.AllowUserToDeleteRows = false;
            this.spectrumDataGridView.AllowUserToResizeRows = false;
            this.spectrumDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.spectrumDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.spectrumDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.spectrumDataGridView.DefaultCellStyle = dataGridViewCellStyle5;
            this.spectrumDataGridView.Location = new System.Drawing.Point(6, 19);
            this.spectrumDataGridView.Name = "spectrumDataGridView";
            this.spectrumDataGridView.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.spectrumDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.spectrumDataGridView.RowHeadersVisible = false;
            this.spectrumDataGridView.RowHeadersWidth = 15;
            this.spectrumDataGridView.RowTemplate.Height = 24;
            this.spectrumDataGridView.Size = new System.Drawing.Size(190, 439);
            this.spectrumDataGridView.StandardTab = true;
            this.spectrumDataGridView.TabIndex = 0;
            // 
            // buttonsGroupBox
            // 
            this.buttonsGroupBox.Controls.Add(this.stopButton);
            this.buttonsGroupBox.Controls.Add(this.acquireButton);
            this.buttonsGroupBox.Location = new System.Drawing.Point(349, 487);
            this.buttonsGroupBox.Name = "buttonsGroupBox";
            this.buttonsGroupBox.Size = new System.Drawing.Size(410, 55);
            this.buttonsGroupBox.TabIndex = 7;
            this.buttonsGroupBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 555);
            this.Controls.Add(this.buttonsGroupBox);
            this.Controls.Add(this.spectrumDataGroupBox);
            this.Controls.Add(this.filteredWaveformDataGroupBox);
            this.Controls.Add(this.messageGroupBox);
            this.Controls.Add(this.generalGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.functionsGroupBox);
            this.Controls.Add(this.filterParametersGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Digital Filtering";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.recordLengthMinNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateMinNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lowOrHighPassCutoffFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandpassOrBandstopWidthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandpassOrStopCenterFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.firTapsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iirOrderNumeric)).EndInit();
            this.functionsGroupBox.ResumeLayout(false);
            this.functionsGroupBox.PerformLayout();
            this.filterParametersGroupBox.ResumeLayout(false);
            this.filterParametersGroupBox.PerformLayout();
            this.timingParametersGroupBox.ResumeLayout(false);
            this.timingParametersGroupBox.PerformLayout();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.messageGroupBox.ResumeLayout(false);
            this.filteredWaveformDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.filteredWaveformDataGridView)).EndInit();
            this.spectrumDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spectrumDataGridView)).EndInit();
            this.buttonsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label channelLabel;
        private System.Windows.Forms.Label recordLengthMinLabel;
        private System.Windows.Forms.Label sampleRateMinLabel;
        private System.Windows.Forms.Label lowOrHighPassCutoffFrequencyLabel;
        private System.Windows.Forms.Label bandpassOrBandstopWidthLabel;
        private System.Windows.Forms.Label bandpassOrStopCenterFrequencyLabel;
        private System.Windows.Forms.Label firTapsLabel;
        private System.Windows.Forms.Label iirOrderLabel;
        private System.Windows.Forms.Label fftFunctionLabel;
        private System.Windows.Forms.Label filterTypeLabel;
        private System.Windows.Forms.Label firWindowLabel;
        private System.Windows.Forms.Label filterLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.NumericUpDown recordLengthMinNumeric;
        private System.Windows.Forms.NumericUpDown sampleRateMinNumeric;
        private System.Windows.Forms.NumericUpDown lowOrHighPassCutoffFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown bandpassOrBandstopWidthNumeric;
        private System.Windows.Forms.NumericUpDown bandpassOrStopCenterFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown firTapsNumeric;
        private System.Windows.Forms.NumericUpDown iirOrderNumeric;
        private System.Windows.Forms.TextBox channelTextBox;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox fftFunctionComboBox;
        private System.Windows.Forms.ComboBox filterTypeComboBox;
        private System.Windows.Forms.ComboBox firWindowComboBox;
        private System.Windows.Forms.ComboBox filterComboBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.GroupBox functionsGroupBox;
        private System.Windows.Forms.GroupBox filterParametersGroupBox;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.GroupBox messageGroupBox;
        private System.Windows.Forms.RichTextBox messageTextBox;
        private System.Windows.Forms.GroupBox filteredWaveformDataGroupBox;
        private System.Windows.Forms.DataGridView filteredWaveformDataGridView;
        private System.Windows.Forms.GroupBox spectrumDataGroupBox;
        private System.Windows.Forms.DataGridView spectrumDataGridView;
        private System.Windows.Forms.GroupBox buttonsGroupBox;


    }
}
