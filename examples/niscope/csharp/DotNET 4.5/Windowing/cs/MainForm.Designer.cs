namespace NationalInstruments.Examples.Windowing
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
            this.windowLabel = new System.Windows.Forms.Label();
            this.fftFunctionLabel = new System.Windows.Forms.Label();
            this.acquireButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.windowComboBox = new System.Windows.Forms.ComboBox();
            this.scalarMeasurementsGroupBox = new System.Windows.Forms.GroupBox();
            this.clearAveragingCheckBox = new System.Windows.Forms.CheckBox();
            this.averageSpectrumCheckBox = new System.Windows.Forms.CheckBox();
            this.fftFunctionComboBox = new System.Windows.Forms.ComboBox();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.recordLengthMinNumeric = new System.Windows.Forms.NumericUpDown();
            this.sampleRateMinNumeric = new System.Windows.Forms.NumericUpDown();
            this.recordLengthMinLabel = new System.Windows.Forms.Label();
            this.sampleRateMinLabel = new System.Windows.Forms.Label();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.channelTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.messageGroupBox = new System.Windows.Forms.GroupBox();
            this.messageTextBox = new System.Windows.Forms.RichTextBox();
            this.sampledDataGroupBox = new System.Windows.Forms.GroupBox();
            this.waveformFromScopeDataGridView = new System.Windows.Forms.DataGridView();
            this.buttonsGroupBox = new System.Windows.Forms.GroupBox();
            this.spectrumDataGroupBox = new System.Windows.Forms.GroupBox();
            this.spectrumDataGridView = new System.Windows.Forms.DataGridView();
            this.scalarMeasurementsGroupBox.SuspendLayout();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recordLengthMinNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateMinNumeric)).BeginInit();
            this.generalGroupBox.SuspendLayout();
            this.messageGroupBox.SuspendLayout();
            this.sampledDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.waveformFromScopeDataGridView)).BeginInit();
            this.buttonsGroupBox.SuspendLayout();
            this.spectrumDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // windowLabel
            // 
            this.windowLabel.AutoSize = true;
            this.windowLabel.Location = new System.Drawing.Point(6, 23);
            this.windowLabel.Name = "windowLabel";
            this.windowLabel.Size = new System.Drawing.Size(46, 13);
            this.windowLabel.TabIndex = 12;
            this.windowLabel.Text = "Window";
            // 
            // fftFunctionLabel
            // 
            this.fftFunctionLabel.AutoSize = true;
            this.fftFunctionLabel.Location = new System.Drawing.Point(6, 50);
            this.fftFunctionLabel.Name = "fftFunctionLabel";
            this.fftFunctionLabel.Size = new System.Drawing.Size(70, 13);
            this.fftFunctionLabel.TabIndex = 13;
            this.fftFunctionLabel.Text = "FFT Function";
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(47, 19);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(128, 19);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "&Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // windowComboBox
            // 
            this.windowComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.windowComboBox.Location = new System.Drawing.Point(82, 19);
            this.windowComboBox.Name = "windowComboBox";
            this.windowComboBox.Size = new System.Drawing.Size(166, 21);
            this.windowComboBox.TabIndex = 0;
            // 
            // scalarMeasurementsGroupBox
            // 
            this.scalarMeasurementsGroupBox.Controls.Add(this.clearAveragingCheckBox);
            this.scalarMeasurementsGroupBox.Controls.Add(this.averageSpectrumCheckBox);
            this.scalarMeasurementsGroupBox.Controls.Add(this.windowComboBox);
            this.scalarMeasurementsGroupBox.Controls.Add(this.fftFunctionComboBox);
            this.scalarMeasurementsGroupBox.Controls.Add(this.windowLabel);
            this.scalarMeasurementsGroupBox.Controls.Add(this.fftFunctionLabel);
            this.scalarMeasurementsGroupBox.Location = new System.Drawing.Point(12, 92);
            this.scalarMeasurementsGroupBox.Name = "scalarMeasurementsGroupBox";
            this.scalarMeasurementsGroupBox.Size = new System.Drawing.Size(255, 122);
            this.scalarMeasurementsGroupBox.TabIndex = 1;
            this.scalarMeasurementsGroupBox.TabStop = false;
            this.scalarMeasurementsGroupBox.Text = "Scalar Measurements";
            // 
            // clearAveragingCheckBox
            // 
            this.clearAveragingCheckBox.AutoSize = true;
            this.clearAveragingCheckBox.Location = new System.Drawing.Point(6, 96);
            this.clearAveragingCheckBox.Name = "clearAveragingCheckBox";
            this.clearAveragingCheckBox.Size = new System.Drawing.Size(107, 17);
            this.clearAveragingCheckBox.TabIndex = 3;
            this.clearAveragingCheckBox.Text = "Clear Averaging?";
            this.clearAveragingCheckBox.UseVisualStyleBackColor = true;
            // 
            // averageSpectrumCheckBox
            // 
            this.averageSpectrumCheckBox.AutoSize = true;
            this.averageSpectrumCheckBox.Location = new System.Drawing.Point(6, 73);
            this.averageSpectrumCheckBox.Name = "averageSpectrumCheckBox";
            this.averageSpectrumCheckBox.Size = new System.Drawing.Size(120, 17);
            this.averageSpectrumCheckBox.TabIndex = 2;
            this.averageSpectrumCheckBox.Text = "Average Spectrum?";
            this.averageSpectrumCheckBox.UseVisualStyleBackColor = true;
            // 
            // fftFunctionComboBox
            // 
            this.fftFunctionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fftFunctionComboBox.Location = new System.Drawing.Point(82, 46);
            this.fftFunctionComboBox.Name = "fftFunctionComboBox";
            this.fftFunctionComboBox.Size = new System.Drawing.Size(166, 21);
            this.fftFunctionComboBox.TabIndex = 1;
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.recordLengthMinNumeric);
            this.timingParametersGroupBox.Controls.Add(this.sampleRateMinNumeric);
            this.timingParametersGroupBox.Controls.Add(this.recordLengthMinLabel);
            this.timingParametersGroupBox.Controls.Add(this.sampleRateMinLabel);
            this.timingParametersGroupBox.Location = new System.Drawing.Point(12, 225);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(255, 69);
            this.timingParametersGroupBox.TabIndex = 2;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // recordLengthMinNumeric
            // 
            this.recordLengthMinNumeric.Location = new System.Drawing.Point(148, 19);
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
            this.sampleRateMinNumeric.Location = new System.Drawing.Point(148, 45);
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
            1000000,
            0,
            0,
            0});
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
            this.sampleRateMinLabel.Location = new System.Drawing.Point(7, 49);
            this.sampleRateMinLabel.Name = "sampleRateMinLabel";
            this.sampleRateMinLabel.Size = new System.Drawing.Size(115, 13);
            this.sampleRateMinLabel.TabIndex = 3;
            this.sampleRateMinLabel.Text = "Minimum Sample Rate:";
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
            this.generalGroupBox.Size = new System.Drawing.Size(255, 69);
            this.generalGroupBox.TabIndex = 0;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            // 
            // channelTextBox
            // 
            this.channelTextBox.Location = new System.Drawing.Point(148, 43);
            this.channelTextBox.Name = "channelTextBox";
            this.channelTextBox.Size = new System.Drawing.Size(100, 20);
            this.channelTextBox.TabIndex = 3;
            this.channelTextBox.Text = "0";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(148, 16);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(100, 21);
            this.resourceNameComboBox.TabIndex = 1;
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
            this.messageGroupBox.Location = new System.Drawing.Point(12, 305);
            this.messageGroupBox.Name = "messageGroupBox";
            this.messageGroupBox.Size = new System.Drawing.Size(255, 60);
            this.messageGroupBox.TabIndex = 3;
            this.messageGroupBox.TabStop = false;
            this.messageGroupBox.Text = "Message";
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(9, 19);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.Size = new System.Drawing.Size(239, 35);
            this.messageTextBox.TabIndex = 0;
            this.messageTextBox.Text = "";
            // 
            // sampledDataGroupBox
            // 
            this.sampledDataGroupBox.Controls.Add(this.waveformFromScopeDataGridView);
            this.sampledDataGroupBox.Location = new System.Drawing.Point(273, 12);
            this.sampledDataGroupBox.Name = "sampledDataGroupBox";
            this.sampledDataGroupBox.Size = new System.Drawing.Size(202, 416);
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
            this.waveformFromScopeDataGridView.Size = new System.Drawing.Size(190, 391);
            this.waveformFromScopeDataGridView.StandardTab = true;
            this.waveformFromScopeDataGridView.TabIndex = 0;
            // 
            // buttonsGroupBox
            // 
            this.buttonsGroupBox.Controls.Add(this.acquireButton);
            this.buttonsGroupBox.Controls.Add(this.stopButton);
            this.buttonsGroupBox.Location = new System.Drawing.Point(12, 371);
            this.buttonsGroupBox.Name = "buttonsGroupBox";
            this.buttonsGroupBox.Size = new System.Drawing.Size(255, 57);
            this.buttonsGroupBox.TabIndex = 4;
            this.buttonsGroupBox.TabStop = false;
            // 
            // spectrumDataGroupBox
            // 
            this.spectrumDataGroupBox.Controls.Add(this.spectrumDataGridView);
            this.spectrumDataGroupBox.Location = new System.Drawing.Point(481, 12);
            this.spectrumDataGroupBox.Name = "spectrumDataGroupBox";
            this.spectrumDataGroupBox.Size = new System.Drawing.Size(202, 416);
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
            this.spectrumDataGridView.Size = new System.Drawing.Size(190, 391);
            this.spectrumDataGridView.StandardTab = true;
            this.spectrumDataGridView.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 441);
            this.Controls.Add(this.spectrumDataGroupBox);
            this.Controls.Add(this.buttonsGroupBox);
            this.Controls.Add(this.sampledDataGroupBox);
            this.Controls.Add(this.messageGroupBox);
            this.Controls.Add(this.generalGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.scalarMeasurementsGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Windowing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            this.scalarMeasurementsGroupBox.ResumeLayout(false);
            this.scalarMeasurementsGroupBox.PerformLayout();
            this.timingParametersGroupBox.ResumeLayout(false);
            this.timingParametersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recordLengthMinNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateMinNumeric)).EndInit();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.messageGroupBox.ResumeLayout(false);
            this.sampledDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.waveformFromScopeDataGridView)).EndInit();
            this.buttonsGroupBox.ResumeLayout(false);
            this.spectrumDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spectrumDataGridView)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label windowLabel;
        private System.Windows.Forms.Label fftFunctionLabel;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox windowComboBox;
        private System.Windows.Forms.ComboBox fftFunctionComboBox;
        private System.Windows.Forms.GroupBox scalarMeasurementsGroupBox;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.NumericUpDown recordLengthMinNumeric;
        private System.Windows.Forms.NumericUpDown sampleRateMinNumeric;
        private System.Windows.Forms.Label recordLengthMinLabel;
        private System.Windows.Forms.Label sampleRateMinLabel;
        private System.Windows.Forms.CheckBox averageSpectrumCheckBox;
        private System.Windows.Forms.CheckBox clearAveragingCheckBox;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.TextBox channelTextBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.GroupBox messageGroupBox;
        private System.Windows.Forms.RichTextBox messageTextBox;
        private System.Windows.Forms.GroupBox sampledDataGroupBox;
        private System.Windows.Forms.DataGridView waveformFromScopeDataGridView;
        private System.Windows.Forms.GroupBox buttonsGroupBox;
        private System.Windows.Forms.GroupBox spectrumDataGroupBox;
        private System.Windows.Forms.DataGridView spectrumDataGridView;


    }
}
