namespace NationalInstruments.Examples.VoltageHistogram
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
            this.scalarResult1Label = new System.Windows.Forms.Label();
            this.meanLabel = new System.Windows.Forms.Label();
            this.scalarResult2Label = new System.Windows.Forms.Label();
            this.highVoltageLimitLabel = new System.Windows.Forms.Label();
            this.lowVoltageLimitLabel = new System.Windows.Forms.Label();
            this.histogramSizeLabel = new System.Windows.Forms.Label();
            this.measurement2Label = new System.Windows.Forms.Label();
            this.measurement1Label = new System.Windows.Forms.Label();
            this.highVoltageLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.lowVoltageLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.histogramSizeNumeric = new System.Windows.Forms.NumericUpDown();
            this.scalarResult1TextBox = new System.Windows.Forms.TextBox();
            this.meanTextBox = new System.Windows.Forms.TextBox();
            this.scalarResult2TextBox = new System.Windows.Forms.TextBox();
            this.acquireButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.measurement2ComboBox = new System.Windows.Forms.ComboBox();
            this.scalarMeasurementsGroupBox = new System.Windows.Forms.GroupBox();
            this.clearStatsCheckBox = new System.Windows.Forms.CheckBox();
            this.measurement1ComboBox = new System.Windows.Forms.ComboBox();
            this.histogramParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.recordLengthMinNumeric = new System.Windows.Forms.NumericUpDown();
            this.sampleRateMinNumeric = new System.Windows.Forms.NumericUpDown();
            this.recordLengthMinLabel = new System.Windows.Forms.Label();
            this.sampleRateMinLabel = new System.Windows.Forms.Label();
            this.sampledDataGroupBox = new System.Windows.Forms.GroupBox();
            this.waveformFromScopeDataGridView = new System.Windows.Forms.DataGridView();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.channelTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.messageGroupBox = new System.Windows.Forms.GroupBox();
            this.messageTextBox = new System.Windows.Forms.RichTextBox();
            this.buttonsGroupBox = new System.Windows.Forms.GroupBox();
            this.voltageHistogramDataGroupBox = new System.Windows.Forms.GroupBox();
            this.voltageHistogramDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.highVoltageLimitNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lowVoltageLimitNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.histogramSizeNumeric)).BeginInit();
            this.scalarMeasurementsGroupBox.SuspendLayout();
            this.histogramParametersGroupBox.SuspendLayout();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recordLengthMinNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateMinNumeric)).BeginInit();
            this.sampledDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.waveformFromScopeDataGridView)).BeginInit();
            this.generalGroupBox.SuspendLayout();
            this.messageGroupBox.SuspendLayout();
            this.buttonsGroupBox.SuspendLayout();
            this.voltageHistogramDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.voltageHistogramDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // scalarResult1Label
            // 
            this.scalarResult1Label.AutoSize = true;
            this.scalarResult1Label.Location = new System.Drawing.Point(6, 50);
            this.scalarResult1Label.Name = "scalarResult1Label";
            this.scalarResult1Label.Size = new System.Drawing.Size(40, 13);
            this.scalarResult1Label.TabIndex = 2;
            this.scalarResult1Label.Text = "Result:";
            // 
            // meanLabel
            // 
            this.meanLabel.AutoSize = true;
            this.meanLabel.Location = new System.Drawing.Point(6, 76);
            this.meanLabel.Name = "meanLabel";
            this.meanLabel.Size = new System.Drawing.Size(37, 13);
            this.meanLabel.TabIndex = 4;
            this.meanLabel.Text = "Mean:";
            // 
            // scalarResult2Label
            // 
            this.scalarResult2Label.AutoSize = true;
            this.scalarResult2Label.BackColor = System.Drawing.SystemColors.Control;
            this.scalarResult2Label.Location = new System.Drawing.Point(6, 129);
            this.scalarResult2Label.Name = "scalarResult2Label";
            this.scalarResult2Label.Size = new System.Drawing.Size(40, 13);
            this.scalarResult2Label.TabIndex = 8;
            this.scalarResult2Label.Text = "Result:";
            // 
            // highVoltageLimitLabel
            // 
            this.highVoltageLimitLabel.AutoSize = true;
            this.highVoltageLimitLabel.Location = new System.Drawing.Point(6, 49);
            this.highVoltageLimitLabel.Name = "highVoltageLimitLabel";
            this.highVoltageLimitLabel.Size = new System.Drawing.Size(111, 13);
            this.highVoltageLimitLabel.TabIndex = 2;
            this.highVoltageLimitLabel.Text = "High Voltage Limit (V):";
            // 
            // lowVoltageLimitLabel
            // 
            this.lowVoltageLimitLabel.AutoSize = true;
            this.lowVoltageLimitLabel.Location = new System.Drawing.Point(6, 75);
            this.lowVoltageLimitLabel.Name = "lowVoltageLimitLabel";
            this.lowVoltageLimitLabel.Size = new System.Drawing.Size(109, 13);
            this.lowVoltageLimitLabel.TabIndex = 4;
            this.lowVoltageLimitLabel.Text = "Low Voltage Limit (V):";
            // 
            // histogramSizeLabel
            // 
            this.histogramSizeLabel.AutoSize = true;
            this.histogramSizeLabel.Location = new System.Drawing.Point(6, 23);
            this.histogramSizeLabel.Name = "histogramSizeLabel";
            this.histogramSizeLabel.Size = new System.Drawing.Size(80, 13);
            this.histogramSizeLabel.TabIndex = 0;
            this.histogramSizeLabel.Text = "Histogram Size:";
            // 
            // measurement2Label
            // 
            this.measurement2Label.AutoSize = true;
            this.measurement2Label.Location = new System.Drawing.Point(6, 102);
            this.measurement2Label.Name = "measurement2Label";
            this.measurement2Label.Size = new System.Drawing.Size(83, 13);
            this.measurement2Label.TabIndex = 6;
            this.measurement2Label.Text = "Measurement 2:";
            // 
            // measurement1Label
            // 
            this.measurement1Label.AutoSize = true;
            this.measurement1Label.Location = new System.Drawing.Point(6, 23);
            this.measurement1Label.Name = "measurement1Label";
            this.measurement1Label.Size = new System.Drawing.Size(83, 13);
            this.measurement1Label.TabIndex = 0;
            this.measurement1Label.Text = "Measurement 1:";
            // 
            // highVoltageLimitNumeric
            // 
            this.highVoltageLimitNumeric.DecimalPlaces = 2;
            this.highVoltageLimitNumeric.Location = new System.Drawing.Point(191, 45);
            this.highVoltageLimitNumeric.Maximum = new decimal(new int[] {
            -1247518720,
            1073741819,
            0,
            0});
            this.highVoltageLimitNumeric.Minimum = new decimal(new int[] {
            -1247518720,
            1073741819,
            0,
            -2147483648});
            this.highVoltageLimitNumeric.Name = "highVoltageLimitNumeric";
            this.highVoltageLimitNumeric.Size = new System.Drawing.Size(100, 20);
            this.highVoltageLimitNumeric.TabIndex = 3;
            this.highVoltageLimitNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // lowVoltageLimitNumeric
            // 
            this.lowVoltageLimitNumeric.DecimalPlaces = 2;
            this.lowVoltageLimitNumeric.Location = new System.Drawing.Point(191, 71);
            this.lowVoltageLimitNumeric.Maximum = new decimal(new int[] {
            -1247518720,
            1073741819,
            0,
            0});
            this.lowVoltageLimitNumeric.Minimum = new decimal(new int[] {
            -1247518720,
            1073741819,
            0,
            -2147483648});
            this.lowVoltageLimitNumeric.Name = "lowVoltageLimitNumeric";
            this.lowVoltageLimitNumeric.Size = new System.Drawing.Size(100, 20);
            this.lowVoltageLimitNumeric.TabIndex = 5;
            this.lowVoltageLimitNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            // 
            // histogramSizeNumeric
            // 
            this.histogramSizeNumeric.Location = new System.Drawing.Point(191, 19);
            this.histogramSizeNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.histogramSizeNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.histogramSizeNumeric.Name = "histogramSizeNumeric";
            this.histogramSizeNumeric.Size = new System.Drawing.Size(100, 20);
            this.histogramSizeNumeric.TabIndex = 1;
            this.histogramSizeNumeric.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            // 
            // scalarResult1TextBox
            // 
            this.scalarResult1TextBox.BackColor = System.Drawing.SystemColors.Control;
            this.scalarResult1TextBox.Location = new System.Drawing.Point(95, 46);
            this.scalarResult1TextBox.Name = "scalarResult1TextBox";
            this.scalarResult1TextBox.ReadOnly = true;
            this.scalarResult1TextBox.Size = new System.Drawing.Size(196, 20);
            this.scalarResult1TextBox.TabIndex = 3;
            // 
            // meanTextBox
            // 
            this.meanTextBox.Location = new System.Drawing.Point(95, 72);
            this.meanTextBox.Name = "meanTextBox";
            this.meanTextBox.ReadOnly = true;
            this.meanTextBox.Size = new System.Drawing.Size(196, 20);
            this.meanTextBox.TabIndex = 5;
            // 
            // scalarResult2TextBox
            // 
            this.scalarResult2TextBox.Location = new System.Drawing.Point(95, 125);
            this.scalarResult2TextBox.Name = "scalarResult2TextBox";
            this.scalarResult2TextBox.ReadOnly = true;
            this.scalarResult2TextBox.Size = new System.Drawing.Size(196, 20);
            this.scalarResult2TextBox.TabIndex = 9;
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
            // measurement2ComboBox
            // 
            this.measurement2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.measurement2ComboBox.Location = new System.Drawing.Point(95, 98);
            this.measurement2ComboBox.Name = "measurement2ComboBox";
            this.measurement2ComboBox.Size = new System.Drawing.Size(196, 21);
            this.measurement2ComboBox.TabIndex = 7;
            // 
            // scalarMeasurementsGroupBox
            // 
            this.scalarMeasurementsGroupBox.Controls.Add(this.clearStatsCheckBox);
            this.scalarMeasurementsGroupBox.Controls.Add(this.measurement2ComboBox);
            this.scalarMeasurementsGroupBox.Controls.Add(this.measurement1ComboBox);
            this.scalarMeasurementsGroupBox.Controls.Add(this.meanTextBox);
            this.scalarMeasurementsGroupBox.Controls.Add(this.measurement1Label);
            this.scalarMeasurementsGroupBox.Controls.Add(this.scalarResult2TextBox);
            this.scalarMeasurementsGroupBox.Controls.Add(this.scalarResult1TextBox);
            this.scalarMeasurementsGroupBox.Controls.Add(this.meanLabel);
            this.scalarMeasurementsGroupBox.Controls.Add(this.scalarResult1Label);
            this.scalarMeasurementsGroupBox.Controls.Add(this.scalarResult2Label);
            this.scalarMeasurementsGroupBox.Controls.Add(this.measurement2Label);
            this.scalarMeasurementsGroupBox.Location = new System.Drawing.Point(12, 92);
            this.scalarMeasurementsGroupBox.Name = "scalarMeasurementsGroupBox";
            this.scalarMeasurementsGroupBox.Size = new System.Drawing.Size(299, 173);
            this.scalarMeasurementsGroupBox.TabIndex = 1;
            this.scalarMeasurementsGroupBox.TabStop = false;
            this.scalarMeasurementsGroupBox.Text = "Scalar Measurements";
            // 
            // clearStatsCheckBox
            // 
            this.clearStatsCheckBox.AutoSize = true;
            this.clearStatsCheckBox.Location = new System.Drawing.Point(6, 151);
            this.clearStatsCheckBox.Name = "clearStatsCheckBox";
            this.clearStatsCheckBox.Size = new System.Drawing.Size(83, 17);
            this.clearStatsCheckBox.TabIndex = 10;
            this.clearStatsCheckBox.Text = "Clear Stats?";
            this.clearStatsCheckBox.UseVisualStyleBackColor = true;
            // 
            // measurement1ComboBox
            // 
            this.measurement1ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.measurement1ComboBox.Location = new System.Drawing.Point(95, 19);
            this.measurement1ComboBox.Name = "measurement1ComboBox";
            this.measurement1ComboBox.Size = new System.Drawing.Size(196, 21);
            this.measurement1ComboBox.TabIndex = 1;
            // 
            // histogramParametersGroupBox
            // 
            this.histogramParametersGroupBox.Controls.Add(this.highVoltageLimitNumeric);
            this.histogramParametersGroupBox.Controls.Add(this.lowVoltageLimitNumeric);
            this.histogramParametersGroupBox.Controls.Add(this.histogramSizeNumeric);
            this.histogramParametersGroupBox.Controls.Add(this.highVoltageLimitLabel);
            this.histogramParametersGroupBox.Controls.Add(this.lowVoltageLimitLabel);
            this.histogramParametersGroupBox.Controls.Add(this.histogramSizeLabel);
            this.histogramParametersGroupBox.Location = new System.Drawing.Point(12, 276);
            this.histogramParametersGroupBox.Name = "histogramParametersGroupBox";
            this.histogramParametersGroupBox.Size = new System.Drawing.Size(299, 98);
            this.histogramParametersGroupBox.TabIndex = 2;
            this.histogramParametersGroupBox.TabStop = false;
            this.histogramParametersGroupBox.Text = "Histogram Parameters";
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.recordLengthMinNumeric);
            this.timingParametersGroupBox.Controls.Add(this.sampleRateMinNumeric);
            this.timingParametersGroupBox.Controls.Add(this.recordLengthMinLabel);
            this.timingParametersGroupBox.Controls.Add(this.sampleRateMinLabel);
            this.timingParametersGroupBox.Location = new System.Drawing.Point(12, 385);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(299, 72);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // recordLengthMinNumeric
            // 
            this.recordLengthMinNumeric.Location = new System.Drawing.Point(191, 45);
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
            this.recordLengthMinNumeric.Size = new System.Drawing.Size(100, 20);
            this.recordLengthMinNumeric.TabIndex = 3;
            this.recordLengthMinNumeric.Value = new decimal(new int[] {
            512,
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
            this.sampleRateMinNumeric.Location = new System.Drawing.Point(191, 19);
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
            this.sampleRateMinNumeric.Size = new System.Drawing.Size(100, 20);
            this.sampleRateMinNumeric.TabIndex = 1;
            this.sampleRateMinNumeric.Value = new decimal(new int[] {
            20000000,
            0,
            0,
            0});
            // 
            // recordLengthMinLabel
            // 
            this.recordLengthMinLabel.AutoSize = true;
            this.recordLengthMinLabel.Location = new System.Drawing.Point(6, 49);
            this.recordLengthMinLabel.Name = "recordLengthMinLabel";
            this.recordLengthMinLabel.Size = new System.Drawing.Size(125, 13);
            this.recordLengthMinLabel.TabIndex = 2;
            this.recordLengthMinLabel.Text = "Minimum Record Length:";
            // 
            // sampleRateMinLabel
            // 
            this.sampleRateMinLabel.AutoSize = true;
            this.sampleRateMinLabel.Location = new System.Drawing.Point(6, 23);
            this.sampleRateMinLabel.Name = "sampleRateMinLabel";
            this.sampleRateMinLabel.Size = new System.Drawing.Size(115, 13);
            this.sampleRateMinLabel.TabIndex = 0;
            this.sampleRateMinLabel.Text = "Minimum Sample Rate:";
            // 
            // sampledDataGroupBox
            // 
            this.sampledDataGroupBox.Controls.Add(this.waveformFromScopeDataGridView);
            this.sampledDataGroupBox.Location = new System.Drawing.Point(317, 12);
            this.sampledDataGroupBox.Name = "sampledDataGroupBox";
            this.sampledDataGroupBox.Size = new System.Drawing.Size(202, 445);
            this.sampledDataGroupBox.TabIndex = 5;
            this.sampledDataGroupBox.TabStop = false;
            this.sampledDataGroupBox.Text = "Waveform From Scope";
            // 
            // waveformFromScopeDataGridView
            // 
            this.waveformFromScopeDataGridView.AllowUserToAddRows = false;
            this.waveformFromScopeDataGridView.AllowUserToDeleteRows = false;
            this.waveformFromScopeDataGridView.AllowUserToResizeRows = false;
            this.waveformFromScopeDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.waveformFromScopeDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.waveformFromScopeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.waveformFromScopeDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.waveformFromScopeDataGridView.Location = new System.Drawing.Point(6, 19);
            this.waveformFromScopeDataGridView.Name = "waveformFromScopeDataGridView";
            this.waveformFromScopeDataGridView.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.waveformFromScopeDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.waveformFromScopeDataGridView.RowHeadersVisible = false;
            this.waveformFromScopeDataGridView.RowHeadersWidth = 15;
            this.waveformFromScopeDataGridView.RowTemplate.Height = 24;
            this.waveformFromScopeDataGridView.Size = new System.Drawing.Size(190, 420);
            this.waveformFromScopeDataGridView.StandardTab = true;
            this.waveformFromScopeDataGridView.TabIndex = 0;
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.channelTextBox);
            this.generalGroupBox.Controls.Add(this.resourceNameComboBox);
            this.generalGroupBox.Controls.Add(this.channelNameLabel);
            this.generalGroupBox.Controls.Add(this.resourceNameLabel);
            this.generalGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.generalGroupBox.Location = new System.Drawing.Point(12, 12);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(299, 69);
            this.generalGroupBox.TabIndex = 0;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            // 
            // channelTextBox
            // 
            this.channelTextBox.Location = new System.Drawing.Point(191, 43);
            this.channelTextBox.Name = "channelTextBox";
            this.channelTextBox.Size = new System.Drawing.Size(100, 20);
            this.channelTextBox.TabIndex = 1;
            this.channelTextBox.Text = "0";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(191, 16);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(100, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // channelNameLabel
            // 
            this.channelNameLabel.AutoSize = true;
            this.channelNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelNameLabel.Location = new System.Drawing.Point(6, 47);
            this.channelNameLabel.Name = "channelNameLabel";
            this.channelNameLabel.Size = new System.Drawing.Size(80, 13);
            this.channelNameLabel.TabIndex = 2;
            this.channelNameLabel.Text = "Channel Name:";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 20);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // messageGroupBox
            // 
            this.messageGroupBox.Controls.Add(this.messageTextBox);
            this.messageGroupBox.Location = new System.Drawing.Point(12, 468);
            this.messageGroupBox.Name = "messageGroupBox";
            this.messageGroupBox.Size = new System.Drawing.Size(299, 58);
            this.messageGroupBox.TabIndex = 4;
            this.messageGroupBox.TabStop = false;
            this.messageGroupBox.Text = "Message";
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(9, 19);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.Size = new System.Drawing.Size(282, 35);
            this.messageTextBox.TabIndex = 0;
            this.messageTextBox.Text = "";
            // 
            // buttonsGroupBox
            // 
            this.buttonsGroupBox.Controls.Add(this.acquireButton);
            this.buttonsGroupBox.Controls.Add(this.stopButton);
            this.buttonsGroupBox.Location = new System.Drawing.Point(317, 468);
            this.buttonsGroupBox.Name = "buttonsGroupBox";
            this.buttonsGroupBox.Size = new System.Drawing.Size(410, 58);
            this.buttonsGroupBox.TabIndex = 7;
            this.buttonsGroupBox.TabStop = false;
            // 
            // voltageHistogramDataGroupBox
            // 
            this.voltageHistogramDataGroupBox.Controls.Add(this.voltageHistogramDataGridView);
            this.voltageHistogramDataGroupBox.Location = new System.Drawing.Point(525, 12);
            this.voltageHistogramDataGroupBox.Name = "voltageHistogramDataGroupBox";
            this.voltageHistogramDataGroupBox.Size = new System.Drawing.Size(202, 445);
            this.voltageHistogramDataGroupBox.TabIndex = 6;
            this.voltageHistogramDataGroupBox.TabStop = false;
            this.voltageHistogramDataGroupBox.Text = "Voltage Histogram";
            // 
            // voltageHistogramDataGridView
            // 
            this.voltageHistogramDataGridView.AllowUserToAddRows = false;
            this.voltageHistogramDataGridView.AllowUserToDeleteRows = false;
            this.voltageHistogramDataGridView.AllowUserToResizeRows = false;
            this.voltageHistogramDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.voltageHistogramDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.voltageHistogramDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.voltageHistogramDataGridView.DefaultCellStyle = dataGridViewCellStyle5;
            this.voltageHistogramDataGridView.Location = new System.Drawing.Point(6, 19);
            this.voltageHistogramDataGridView.Name = "voltageHistogramDataGridView";
            this.voltageHistogramDataGridView.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.voltageHistogramDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.voltageHistogramDataGridView.RowHeadersVisible = false;
            this.voltageHistogramDataGridView.RowHeadersWidth = 15;
            this.voltageHistogramDataGridView.RowTemplate.Height = 24;
            this.voltageHistogramDataGridView.Size = new System.Drawing.Size(190, 420);
            this.voltageHistogramDataGridView.StandardTab = true;
            this.voltageHistogramDataGridView.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 538);
            this.Controls.Add(this.voltageHistogramDataGroupBox);
            this.Controls.Add(this.buttonsGroupBox);
            this.Controls.Add(this.messageGroupBox);
            this.Controls.Add(this.generalGroupBox);
            this.Controls.Add(this.sampledDataGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.scalarMeasurementsGroupBox);
            this.Controls.Add(this.histogramParametersGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Voltage Histogram";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.highVoltageLimitNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lowVoltageLimitNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.histogramSizeNumeric)).EndInit();
            this.scalarMeasurementsGroupBox.ResumeLayout(false);
            this.scalarMeasurementsGroupBox.PerformLayout();
            this.histogramParametersGroupBox.ResumeLayout(false);
            this.histogramParametersGroupBox.PerformLayout();
            this.timingParametersGroupBox.ResumeLayout(false);
            this.timingParametersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recordLengthMinNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateMinNumeric)).EndInit();
            this.sampledDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.waveformFromScopeDataGridView)).EndInit();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.messageGroupBox.ResumeLayout(false);
            this.buttonsGroupBox.ResumeLayout(false);
            this.voltageHistogramDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.voltageHistogramDataGridView)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label scalarResult1Label;
        private System.Windows.Forms.Label meanLabel;
        private System.Windows.Forms.Label scalarResult2Label;
        private System.Windows.Forms.Label highVoltageLimitLabel;
        private System.Windows.Forms.Label lowVoltageLimitLabel;
        private System.Windows.Forms.Label histogramSizeLabel;
        private System.Windows.Forms.Label measurement2Label;
        private System.Windows.Forms.Label measurement1Label;
        private System.Windows.Forms.NumericUpDown highVoltageLimitNumeric;
        private System.Windows.Forms.NumericUpDown lowVoltageLimitNumeric;
        private System.Windows.Forms.NumericUpDown histogramSizeNumeric;
        private System.Windows.Forms.TextBox scalarResult1TextBox;
        private System.Windows.Forms.TextBox meanTextBox;
        private System.Windows.Forms.TextBox scalarResult2TextBox;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox measurement2ComboBox;
        private System.Windows.Forms.ComboBox measurement1ComboBox;
        private System.Windows.Forms.GroupBox scalarMeasurementsGroupBox;
        private System.Windows.Forms.GroupBox histogramParametersGroupBox;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.NumericUpDown recordLengthMinNumeric;
        private System.Windows.Forms.NumericUpDown sampleRateMinNumeric;
        private System.Windows.Forms.Label recordLengthMinLabel;
        private System.Windows.Forms.Label sampleRateMinLabel;
        private System.Windows.Forms.CheckBox clearStatsCheckBox;
        private System.Windows.Forms.GroupBox sampledDataGroupBox;
        private System.Windows.Forms.DataGridView waveformFromScopeDataGridView;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.TextBox channelTextBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.GroupBox messageGroupBox;
        private System.Windows.Forms.RichTextBox messageTextBox;
        private System.Windows.Forms.GroupBox buttonsGroupBox;
        private System.Windows.Forms.GroupBox voltageHistogramDataGroupBox;
        private System.Windows.Forms.DataGridView voltageHistogramDataGridView;

    }
}
