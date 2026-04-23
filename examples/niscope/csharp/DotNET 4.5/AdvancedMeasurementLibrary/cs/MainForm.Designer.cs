namespace NationalInstruments.Examples.AdvancedMeasurementLibrary
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.measurementsGroupBox = new System.Windows.Forms.GroupBox();
            this.noteTextBox = new System.Windows.Forms.TextBox();
            this.arrayMeasurementLabel = new System.Windows.Forms.Label();
            this.preocessingLabel = new System.Windows.Forms.Label();
            this.arrayMeasurementComboBox = new System.Windows.Forms.ComboBox();
            this.processingStepComboBox = new System.Windows.Forms.ComboBox();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.timeoutNumeric = new System.Windows.Forms.NumericUpDown();
            this.channelTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.timeoutLabel = new System.Windows.Forms.Label();
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.filterComboBox = new System.Windows.Forms.ComboBox();
            this.filterTypeLabel = new System.Windows.Forms.Label();
            this.lowPassFrequencyLabel = new System.Windows.Forms.Label();
            this.stopFrequencyLabel = new System.Windows.Forms.Label();
            this.stopWidthLabel = new System.Windows.Forms.Label();
            this.cutoffFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.centerFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.bandpassWidthNumeric = new System.Windows.Forms.NumericUpDown();
            this.filterGroupBox = new System.Windows.Forms.GroupBox();
            this.messageTextBox = new System.Windows.Forms.RichTextBox();
            this.messageGroupBox = new System.Windows.Forms.GroupBox();
            this.sampledDataGridView = new System.Windows.Forms.DataGridView();
            this.sampledDataGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementDataGridView = new System.Windows.Forms.DataGridView();
            this.measurementDataGroupBox = new System.Windows.Forms.GroupBox();
            this.acquireButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.buttonsGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementsGroupBox.SuspendLayout();
            this.generalGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutoffFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandpassWidthNumeric)).BeginInit();
            this.filterGroupBox.SuspendLayout();
            this.messageGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sampledDataGridView)).BeginInit();
            this.sampledDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measurementDataGridView)).BeginInit();
            this.measurementDataGroupBox.SuspendLayout();
            this.buttonsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // measurementsGroupBox
            // 
            this.measurementsGroupBox.Controls.Add(this.noteTextBox);
            this.measurementsGroupBox.Controls.Add(this.arrayMeasurementLabel);
            this.measurementsGroupBox.Controls.Add(this.preocessingLabel);
            this.measurementsGroupBox.Controls.Add(this.arrayMeasurementComboBox);
            this.measurementsGroupBox.Controls.Add(this.processingStepComboBox);
            this.measurementsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.measurementsGroupBox.Location = new System.Drawing.Point(12, 118);
            this.measurementsGroupBox.Name = "measurementsGroupBox";
            this.measurementsGroupBox.Size = new System.Drawing.Size(282, 215);
            this.measurementsGroupBox.TabIndex = 1;
            this.measurementsGroupBox.TabStop = false;
            this.measurementsGroupBox.Text = "Measurements";
            // 
            // noteTextBox
            // 
            this.noteTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.noteTextBox.Location = new System.Drawing.Point(14, 75);
            this.noteTextBox.Multiline = true;
            this.noteTextBox.Name = "noteTextBox";
            this.noteTextBox.ReadOnly = true;
            this.noteTextBox.Size = new System.Drawing.Size(257, 119);
            this.noteTextBox.TabIndex = 4;
            this.noteTextBox.TabStop = false;
            this.noteTextBox.Text = resources.GetString("noteTextBox.Text");
            // 
            // arrayMeasurementLabel
            // 
            this.arrayMeasurementLabel.AutoSize = true;
            this.arrayMeasurementLabel.Location = new System.Drawing.Point(6, 47);
            this.arrayMeasurementLabel.Name = "arrayMeasurementLabel";
            this.arrayMeasurementLabel.Size = new System.Drawing.Size(101, 13);
            this.arrayMeasurementLabel.TabIndex = 2;
            this.arrayMeasurementLabel.Text = "Array Measurement:";
            // 
            // preocessingLabel
            // 
            this.preocessingLabel.AutoSize = true;
            this.preocessingLabel.Location = new System.Drawing.Point(6, 20);
            this.preocessingLabel.Name = "preocessingLabel";
            this.preocessingLabel.Size = new System.Drawing.Size(87, 13);
            this.preocessingLabel.TabIndex = 0;
            this.preocessingLabel.Text = "Processing Step:";
            // 
            // arrayMeasurementComboBox
            // 
            this.arrayMeasurementComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.arrayMeasurementComboBox.FormattingEnabled = true;
            this.arrayMeasurementComboBox.Location = new System.Drawing.Point(113, 43);
            this.arrayMeasurementComboBox.Name = "arrayMeasurementComboBox";
            this.arrayMeasurementComboBox.Size = new System.Drawing.Size(163, 21);
            this.arrayMeasurementComboBox.TabIndex = 3;
            // 
            // processingStepComboBox
            // 
            this.processingStepComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.processingStepComboBox.FormattingEnabled = true;
            this.processingStepComboBox.Location = new System.Drawing.Point(113, 16);
            this.processingStepComboBox.Name = "processingStepComboBox";
            this.processingStepComboBox.Size = new System.Drawing.Size(163, 21);
            this.processingStepComboBox.TabIndex = 1;
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.timeoutNumeric);
            this.generalGroupBox.Controls.Add(this.channelTextBox);
            this.generalGroupBox.Controls.Add(this.resourceNameComboBox);
            this.generalGroupBox.Controls.Add(this.timeoutLabel);
            this.generalGroupBox.Controls.Add(this.channelNameLabel);
            this.generalGroupBox.Controls.Add(this.resourceNameLabel);
            this.generalGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.generalGroupBox.Location = new System.Drawing.Point(12, 12);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(282, 100);
            this.generalGroupBox.TabIndex = 0;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            // 
            // timeoutNumeric
            // 
            this.timeoutNumeric.DecimalPlaces = 2;
            this.timeoutNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.timeoutNumeric.Location = new System.Drawing.Point(176, 69);
            this.timeoutNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.timeoutNumeric.Name = "timeoutNumeric";
            this.timeoutNumeric.Size = new System.Drawing.Size(100, 20);
            this.timeoutNumeric.TabIndex = 5;
            // 
            // channelTextBox
            // 
            this.channelTextBox.Location = new System.Drawing.Point(176, 43);
            this.channelTextBox.Name = "channelTextBox";
            this.channelTextBox.Size = new System.Drawing.Size(100, 20);
            this.channelTextBox.TabIndex = 3;
            this.channelTextBox.Text = "0";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(176, 16);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(100, 21);
            this.resourceNameComboBox.TabIndex = 1;
            // 
            // timeoutLabel
            // 
            this.timeoutLabel.AutoSize = true;
            this.timeoutLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timeoutLabel.Location = new System.Drawing.Point(6, 74);
            this.timeoutLabel.Name = "timeoutLabel";
            this.timeoutLabel.Size = new System.Drawing.Size(62, 13);
            this.timeoutLabel.TabIndex = 4;
            this.timeoutLabel.Text = "Timeout (s):";
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
            // filterComboBox
            // 
            this.filterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterComboBox.FormattingEnabled = true;
            this.filterComboBox.Location = new System.Drawing.Point(176, 16);
            this.filterComboBox.Name = "filterComboBox";
            this.filterComboBox.Size = new System.Drawing.Size(100, 21);
            this.filterComboBox.TabIndex = 1;
            // 
            // filterTypeLabel
            // 
            this.filterTypeLabel.AutoSize = true;
            this.filterTypeLabel.Location = new System.Drawing.Point(6, 20);
            this.filterTypeLabel.Name = "filterTypeLabel";
            this.filterTypeLabel.Size = new System.Drawing.Size(59, 13);
            this.filterTypeLabel.TabIndex = 0;
            this.filterTypeLabel.Text = "Filter Type:";
            // 
            // lowPassFrequencyLabel
            // 
            this.lowPassFrequencyLabel.AutoSize = true;
            this.lowPassFrequencyLabel.Location = new System.Drawing.Point(6, 47);
            this.lowPassFrequencyLabel.Name = "lowPassFrequencyLabel";
            this.lowPassFrequencyLabel.Size = new System.Drawing.Size(158, 13);
            this.lowPassFrequencyLabel.TabIndex = 2;
            this.lowPassFrequencyLabel.Text = "Low/High Pass Cutoff Freq (hz):";
            // 
            // stopFrequencyLabel
            // 
            this.stopFrequencyLabel.AutoSize = true;
            this.stopFrequencyLabel.Location = new System.Drawing.Point(6, 73);
            this.stopFrequencyLabel.Name = "stopFrequencyLabel";
            this.stopFrequencyLabel.Size = new System.Drawing.Size(163, 13);
            this.stopFrequencyLabel.TabIndex = 4;
            this.stopFrequencyLabel.Text = "BandPass/Stop Center Freq (hz):";
            // 
            // stopWidthLabel
            // 
            this.stopWidthLabel.AutoSize = true;
            this.stopWidthLabel.Location = new System.Drawing.Point(6, 99);
            this.stopWidthLabel.Name = "stopWidthLabel";
            this.stopWidthLabel.Size = new System.Drawing.Size(141, 13);
            this.stopWidthLabel.TabIndex = 6;
            this.stopWidthLabel.Text = "BandPass/BandStop Width:";
            // 
            // cutoffFrequencyNumeric
            // 
            this.cutoffFrequencyNumeric.Location = new System.Drawing.Point(176, 43);
            this.cutoffFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.cutoffFrequencyNumeric.Name = "cutoffFrequencyNumeric";
            this.cutoffFrequencyNumeric.Size = new System.Drawing.Size(100, 20);
            this.cutoffFrequencyNumeric.TabIndex = 3;
            this.cutoffFrequencyNumeric.Value = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            // 
            // centerFrequencyNumeric
            // 
            this.centerFrequencyNumeric.Location = new System.Drawing.Point(176, 69);
            this.centerFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.centerFrequencyNumeric.Name = "centerFrequencyNumeric";
            this.centerFrequencyNumeric.Size = new System.Drawing.Size(100, 20);
            this.centerFrequencyNumeric.TabIndex = 5;
            this.centerFrequencyNumeric.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // bandpassWidthNumeric
            // 
            this.bandpassWidthNumeric.Location = new System.Drawing.Point(176, 95);
            this.bandpassWidthNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.bandpassWidthNumeric.Name = "bandpassWidthNumeric";
            this.bandpassWidthNumeric.Size = new System.Drawing.Size(100, 20);
            this.bandpassWidthNumeric.TabIndex = 7;
            this.bandpassWidthNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // filterGroupBox
            // 
            this.filterGroupBox.Controls.Add(this.bandpassWidthNumeric);
            this.filterGroupBox.Controls.Add(this.centerFrequencyNumeric);
            this.filterGroupBox.Controls.Add(this.cutoffFrequencyNumeric);
            this.filterGroupBox.Controls.Add(this.stopWidthLabel);
            this.filterGroupBox.Controls.Add(this.stopFrequencyLabel);
            this.filterGroupBox.Controls.Add(this.lowPassFrequencyLabel);
            this.filterGroupBox.Controls.Add(this.filterTypeLabel);
            this.filterGroupBox.Controls.Add(this.filterComboBox);
            this.filterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filterGroupBox.Location = new System.Drawing.Point(12, 339);
            this.filterGroupBox.Name = "filterGroupBox";
            this.filterGroupBox.Size = new System.Drawing.Size(282, 125);
            this.filterGroupBox.TabIndex = 2;
            this.filterGroupBox.TabStop = false;
            this.filterGroupBox.Text = "Filter Parameters";
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(9, 19);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.Size = new System.Drawing.Size(267, 35);
            this.messageTextBox.TabIndex = 0;
            this.messageTextBox.Text = "";
            // 
            // messageGroupBox
            // 
            this.messageGroupBox.Controls.Add(this.messageTextBox);
            this.messageGroupBox.Location = new System.Drawing.Point(12, 470);
            this.messageGroupBox.Name = "messageGroupBox";
            this.messageGroupBox.Size = new System.Drawing.Size(282, 60);
            this.messageGroupBox.TabIndex = 3;
            this.messageGroupBox.TabStop = false;
            this.messageGroupBox.Text = "Message";
            // 
            // sampledDataGridView
            // 
            this.sampledDataGridView.AllowUserToAddRows = false;
            this.sampledDataGridView.AllowUserToDeleteRows = false;
            this.sampledDataGridView.AllowUserToResizeRows = false;
            this.sampledDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sampledDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.sampledDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.sampledDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.sampledDataGridView.Location = new System.Drawing.Point(6, 19);
            this.sampledDataGridView.Name = "sampledDataGridView";
            this.sampledDataGridView.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sampledDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.sampledDataGridView.RowHeadersVisible = false;
            this.sampledDataGridView.RowHeadersWidth = 15;
            this.sampledDataGridView.RowTemplate.Height = 24;
            this.sampledDataGridView.Size = new System.Drawing.Size(190, 427);
            this.sampledDataGridView.StandardTab = true;
            this.sampledDataGridView.TabIndex = 0;
            // 
            // sampledDataGroupBox
            // 
            this.sampledDataGroupBox.Controls.Add(this.sampledDataGridView);
            this.sampledDataGroupBox.Location = new System.Drawing.Point(300, 12);
            this.sampledDataGroupBox.Name = "sampledDataGroupBox";
            this.sampledDataGroupBox.Size = new System.Drawing.Size(202, 452);
            this.sampledDataGroupBox.TabIndex = 4;
            this.sampledDataGroupBox.TabStop = false;
            this.sampledDataGroupBox.Text = "Sampled Data";
            // 
            // measurementDataGridView
            // 
            this.measurementDataGridView.AllowUserToAddRows = false;
            this.measurementDataGridView.AllowUserToDeleteRows = false;
            this.measurementDataGridView.AllowUserToResizeRows = false;
            this.measurementDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.measurementDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.measurementDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.measurementDataGridView.DefaultCellStyle = dataGridViewCellStyle5;
            this.measurementDataGridView.Location = new System.Drawing.Point(6, 19);
            this.measurementDataGridView.Name = "measurementDataGridView";
            this.measurementDataGridView.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.measurementDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.measurementDataGridView.RowHeadersVisible = false;
            this.measurementDataGridView.RowHeadersWidth = 15;
            this.measurementDataGridView.RowTemplate.Height = 24;
            this.measurementDataGridView.Size = new System.Drawing.Size(190, 427);
            this.measurementDataGridView.StandardTab = true;
            this.measurementDataGridView.TabIndex = 0;
            // 
            // measurementDataGroupBox
            // 
            this.measurementDataGroupBox.Controls.Add(this.measurementDataGridView);
            this.measurementDataGroupBox.Location = new System.Drawing.Point(508, 12);
            this.measurementDataGroupBox.Name = "measurementDataGroupBox";
            this.measurementDataGroupBox.Size = new System.Drawing.Size(202, 452);
            this.measurementDataGroupBox.TabIndex = 5;
            this.measurementDataGroupBox.TabStop = false;
            this.measurementDataGroupBox.Text = "Measurement Data";
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(125, 19);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(206, 19);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "&Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // buttonsGroupBox
            // 
            this.buttonsGroupBox.Controls.Add(this.stopButton);
            this.buttonsGroupBox.Controls.Add(this.acquireButton);
            this.buttonsGroupBox.Location = new System.Drawing.Point(300, 471);
            this.buttonsGroupBox.Name = "buttonsGroupBox";
            this.buttonsGroupBox.Size = new System.Drawing.Size(410, 59);
            this.buttonsGroupBox.TabIndex = 6;
            this.buttonsGroupBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 542);
            this.Controls.Add(this.buttonsGroupBox);
            this.Controls.Add(this.measurementDataGroupBox);
            this.Controls.Add(this.sampledDataGroupBox);
            this.Controls.Add(this.messageGroupBox);
            this.Controls.Add(this.measurementsGroupBox);
            this.Controls.Add(this.filterGroupBox);
            this.Controls.Add(this.generalGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Advanced Measurement Library";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            this.measurementsGroupBox.ResumeLayout(false);
            this.measurementsGroupBox.PerformLayout();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cutoffFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandpassWidthNumeric)).EndInit();
            this.filterGroupBox.ResumeLayout(false);
            this.filterGroupBox.PerformLayout();
            this.messageGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sampledDataGridView)).EndInit();
            this.sampledDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.measurementDataGridView)).EndInit();
            this.measurementDataGroupBox.ResumeLayout(false);
            this.buttonsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.GroupBox measurementsGroupBox;
        private System.Windows.Forms.TextBox noteTextBox;
        private System.Windows.Forms.Label arrayMeasurementLabel;
        private System.Windows.Forms.Label preocessingLabel;
        private System.Windows.Forms.ComboBox arrayMeasurementComboBox;
        private System.Windows.Forms.ComboBox processingStepComboBox;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.TextBox channelTextBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label timeoutLabel;
        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.NumericUpDown timeoutNumeric;
        private System.Windows.Forms.ComboBox filterComboBox;
        private System.Windows.Forms.Label filterTypeLabel;
        private System.Windows.Forms.Label lowPassFrequencyLabel;
        private System.Windows.Forms.Label stopFrequencyLabel;
        private System.Windows.Forms.Label stopWidthLabel;
        private System.Windows.Forms.NumericUpDown cutoffFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown centerFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown bandpassWidthNumeric;
        private System.Windows.Forms.GroupBox filterGroupBox;
        private System.Windows.Forms.RichTextBox messageTextBox;
        private System.Windows.Forms.GroupBox messageGroupBox;
        private System.Windows.Forms.DataGridView sampledDataGridView;
        private System.Windows.Forms.GroupBox sampledDataGroupBox;
        private System.Windows.Forms.DataGridView measurementDataGridView;
        private System.Windows.Forms.GroupBox measurementDataGroupBox;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox buttonsGroupBox;
    }
}
