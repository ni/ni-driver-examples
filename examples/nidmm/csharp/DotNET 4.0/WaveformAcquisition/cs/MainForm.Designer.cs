namespace NationalInstruments.Examples.WaveformAcquisition
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
            this.resourceAndMeasurementGroupBox = new System.Windows.Forms.GroupBox();
            this.acquisitionModeLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.acquisitionModeComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.rateValueTextBox = new System.Windows.Forms.TextBox();
            this.rangeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.rateLabel = new System.Windows.Forms.Label();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.errorGroupBox = new System.Windows.Forms.GroupBox();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.clearButton = new System.Windows.Forms.Button();
            this.acquireButton = new System.Windows.Forms.Button();
            this.numberOfSamplesNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.actualRangeTextBox = new System.Windows.Forms.TextBox();
            this.actualRangeLabel = new System.Windows.Forms.Label();
            this.readingDataGridView = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reading = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numberOfSamplesLabel = new System.Windows.Forms.Label();
            this.resourceAndMeasurementGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangeNumericUpDown)).BeginInit();
            this.errorGroupBox.SuspendLayout();
            this.measurementGroupBox.SuspendLayout();
            this.buttonsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfSamplesNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.readingDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // resourceAndMeasurementGroupBox
            // 
            this.resourceAndMeasurementGroupBox.Controls.Add(this.acquisitionModeLabel);
            this.resourceAndMeasurementGroupBox.Controls.Add(this.resourceNameLabel);
            this.resourceAndMeasurementGroupBox.Controls.Add(this.acquisitionModeComboBox);
            this.resourceAndMeasurementGroupBox.Controls.Add(this.resourceNameComboBox);
            this.resourceAndMeasurementGroupBox.Location = new System.Drawing.Point(6, 11);
            this.resourceAndMeasurementGroupBox.Name = "resourceAndMeasurementGroupBox";
            this.resourceAndMeasurementGroupBox.Size = new System.Drawing.Size(264, 85);
            this.resourceAndMeasurementGroupBox.TabIndex = 0;
            this.resourceAndMeasurementGroupBox.TabStop = false;
            this.resourceAndMeasurementGroupBox.Text = "Resource Name and Measurement Type";
            // 
            // acquisitionModeLabel
            // 
            this.acquisitionModeLabel.AutoSize = true;
            this.acquisitionModeLabel.Location = new System.Drawing.Point(10, 55);
            this.acquisitionModeLabel.Name = "acquisitionModeLabel";
            this.acquisitionModeLabel.Size = new System.Drawing.Size(91, 13);
            this.acquisitionModeLabel.TabIndex = 3;
            this.acquisitionModeLabel.Text = "Acquisition Mode:";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(10, 26);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 2;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // acquisitionModeComboBox
            // 
            this.acquisitionModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.acquisitionModeComboBox.FormattingEnabled = true;
            this.acquisitionModeComboBox.Location = new System.Drawing.Point(140, 55);
            this.acquisitionModeComboBox.Name = "acquisitionModeComboBox";
            this.acquisitionModeComboBox.Size = new System.Drawing.Size(118, 21);
            this.acquisitionModeComboBox.TabIndex = 1;
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(140, 26);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(118, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.rateValueTextBox);
            this.configurationGroupBox.Controls.Add(this.rangeNumericUpDown);
            this.configurationGroupBox.Controls.Add(this.rateLabel);
            this.configurationGroupBox.Controls.Add(this.rangeLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(6, 102);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(265, 93);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // rateValueTextBox
            // 
            this.rateValueTextBox.Location = new System.Drawing.Point(140, 55);
            this.rateValueTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.rateValueTextBox.Name = "rateValueTextBox";
            this.rateValueTextBox.Size = new System.Drawing.Size(118, 20);
            this.rateValueTextBox.TabIndex = 1;
            this.rateValueTextBox.Text = "1.80E+6";
            // 
            // rangeNumericUpDown
            // 
            this.rangeNumericUpDown.DecimalPlaces = 3;
            this.rangeNumericUpDown.Location = new System.Drawing.Point(140, 27);
            this.rangeNumericUpDown.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.rangeNumericUpDown.Name = "rangeNumericUpDown";
            this.rangeNumericUpDown.Size = new System.Drawing.Size(117, 20);
            this.rangeNumericUpDown.TabIndex = 0;
            this.rangeNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // rateLabel
            // 
            this.rateLabel.AutoSize = true;
            this.rateLabel.Location = new System.Drawing.Point(10, 55);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(122, 13);
            this.rateLabel.TabIndex = 6;
            this.rateLabel.Text = "Rate (Samples/second):";
            // 
            // rangeLabel
            // 
            this.rangeLabel.AutoSize = true;
            this.rangeLabel.Location = new System.Drawing.Point(10, 27);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.Size = new System.Drawing.Size(42, 13);
            this.rangeLabel.TabIndex = 5;
            this.rangeLabel.Text = "Range:";
            // 
            // errorGroupBox
            // 
            this.errorGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.errorGroupBox.Controls.Add(this.messageTextBox);
            this.errorGroupBox.Location = new System.Drawing.Point(7, 202);
            this.errorGroupBox.Name = "errorGroupBox";
            this.errorGroupBox.Size = new System.Drawing.Size(264, 353);
            this.errorGroupBox.TabIndex = 3;
            this.errorGroupBox.TabStop = false;
            this.errorGroupBox.Text = "Message";
            // 
            // messageTextBox
            // 
            this.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageTextBox.Location = new System.Drawing.Point(3, 16);
            this.messageTextBox.MinimumSize = new System.Drawing.Size(0, 30);
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messageTextBox.Size = new System.Drawing.Size(258, 334);
            this.messageTextBox.TabIndex = 0;
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.measurementGroupBox.Controls.Add(this.buttonsPanel);
            this.measurementGroupBox.Controls.Add(this.numberOfSamplesNumericUpDown);
            this.measurementGroupBox.Controls.Add(this.actualRangeTextBox);
            this.measurementGroupBox.Controls.Add(this.actualRangeLabel);
            this.measurementGroupBox.Controls.Add(this.readingDataGridView);
            this.measurementGroupBox.Controls.Add(this.numberOfSamplesLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(285, 11);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(304, 544);
            this.measurementGroupBox.TabIndex = 2;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // buttonsPanel
            // 
            this.buttonsPanel.Controls.Add(this.clearButton);
            this.buttonsPanel.Controls.Add(this.acquireButton);
            this.buttonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonsPanel.Location = new System.Drawing.Point(3, 503);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(298, 38);
            this.buttonsPanel.TabIndex = 3;
            // 
            // clearButton
            // 
            this.clearButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.clearButton.Location = new System.Drawing.Point(152, 3);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 3;
            this.clearButton.Text = "&Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // acquireButton
            // 
            this.acquireButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.acquireButton.Location = new System.Drawing.Point(44, 3);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // numberOfSamplesNumericUpDown
            // 
            this.numberOfSamplesNumericUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numberOfSamplesNumericUpDown.Location = new System.Drawing.Point(180, 27);
            this.numberOfSamplesNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numberOfSamplesNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfSamplesNumericUpDown.Name = "numberOfSamplesNumericUpDown";
            this.numberOfSamplesNumericUpDown.Size = new System.Drawing.Size(100, 20);
            this.numberOfSamplesNumericUpDown.TabIndex = 0;
            this.numberOfSamplesNumericUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // actualRangeTextBox
            // 
            this.actualRangeTextBox.Location = new System.Drawing.Point(180, 55);
            this.actualRangeTextBox.Name = "actualRangeTextBox";
            this.actualRangeTextBox.ReadOnly = true;
            this.actualRangeTextBox.Size = new System.Drawing.Size(100, 20);
            this.actualRangeTextBox.TabIndex = 1;
            // 
            // actualRangeLabel
            // 
            this.actualRangeLabel.AutoSize = true;
            this.actualRangeLabel.Location = new System.Drawing.Point(18, 55);
            this.actualRangeLabel.Name = "actualRangeLabel";
            this.actualRangeLabel.Size = new System.Drawing.Size(75, 13);
            this.actualRangeLabel.TabIndex = 3;
            this.actualRangeLabel.Text = "Actual Range:";
            // 
            // readingDataGridView
            // 
            this.readingDataGridView.AllowUserToAddRows = false;
            this.readingDataGridView.AllowUserToDeleteRows = false;
            this.readingDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.readingDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.readingDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.reading});
            this.readingDataGridView.Location = new System.Drawing.Point(21, 91);
            this.readingDataGridView.Name = "readingDataGridView";
            this.readingDataGridView.ReadOnly = true;
            this.readingDataGridView.RowHeadersWidth = 15;
            this.readingDataGridView.RowTemplate.Height = 24;
            this.readingDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.readingDataGridView.Size = new System.Drawing.Size(260, 394);
            this.readingDataGridView.StandardTab = true;
            this.readingDataGridView.TabIndex = 2;
            // 
            // Index
            // 
            this.Index.HeaderText = "Point";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            // 
            // reading
            // 
            this.reading.HeaderText = "Amplitude";
            this.reading.Name = "reading";
            this.reading.ReadOnly = true;
            this.reading.Width = 250;
            // 
            // numberOfSamplesLabel
            // 
            this.numberOfSamplesLabel.AutoSize = true;
            this.numberOfSamplesLabel.Location = new System.Drawing.Point(18, 26);
            this.numberOfSamplesLabel.Name = "numberOfSamplesLabel";
            this.numberOfSamplesLabel.Size = new System.Drawing.Size(104, 13);
            this.numberOfSamplesLabel.TabIndex = 0;
            this.numberOfSamplesLabel.Text = "Number Of Samples:";
            // 
            // MainForm
            // 
            this.AcceptButton = this.acquireButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(592, 565);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.errorGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.resourceAndMeasurementGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(608, 1632);
            this.MinimumSize = new System.Drawing.Size(608, 598);
            this.Name = "MainForm";
            this.Text = "Waveform Acquisition";
            this.resourceAndMeasurementGroupBox.ResumeLayout(false);
            this.resourceAndMeasurementGroupBox.PerformLayout();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangeNumericUpDown)).EndInit();
            this.errorGroupBox.ResumeLayout(false);
            this.errorGroupBox.PerformLayout();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.buttonsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numberOfSamplesNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.readingDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox resourceAndMeasurementGroupBox;
        private System.Windows.Forms.Label acquisitionModeLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.ComboBox acquisitionModeComboBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.NumericUpDown rangeNumericUpDown;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.GroupBox errorGroupBox;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;
        private System.Windows.Forms.Panel buttonsPanel;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.NumericUpDown numberOfSamplesNumericUpDown;
        private System.Windows.Forms.TextBox actualRangeTextBox;
        private System.Windows.Forms.Label actualRangeLabel;
        private System.Windows.Forms.DataGridView readingDataGridView;
        private System.Windows.Forms.Label numberOfSamplesLabel;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.TextBox rateValueTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn reading;
    }
}

