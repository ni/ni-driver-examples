namespace NationalInstruments.Examples.SoftwareTriggeredMultiPointAquisition
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
            this.measurementModeLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.measurementModeComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.resourceAndMeasurementGroupBox = new System.Windows.Forms.GroupBox();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.rangeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.powerlineFrequencyLabel = new System.Windows.Forms.Label();
            this.resolutionLabel = new System.Windows.Forms.Label();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.powerlineFrequencyValueComboBox = new System.Windows.Forms.ComboBox();
            this.resolutionValueComboBox = new System.Windows.Forms.ComboBox();
            this.errorGroupBox = new System.Windows.Forms.GroupBox();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.triggerGroupBox = new System.Windows.Forms.GroupBox();
            this.triggerDelayNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.triggerDelayLabel = new System.Windows.Forms.Label();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.triggerSourceComboBox = new System.Windows.Forms.ComboBox();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.readButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.softwareTriggerButton = new System.Windows.Forms.Button();
            this.numberOfMeasurementsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.actualRangeTextBox = new System.Windows.Forms.TextBox();
            this.actualRangeLabel = new System.Windows.Forms.Label();
            this.readingDataGridView = new System.Windows.Forms.DataGridView();
            this.ReadingNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reading = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numberOfMeasurementsLabel = new System.Windows.Forms.Label();
            this.resourceAndMeasurementGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangeNumericUpDown)).BeginInit();
            this.errorGroupBox.SuspendLayout();
            this.triggerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.triggerDelayNumericUpDown)).BeginInit();
            this.measurementGroupBox.SuspendLayout();
            this.buttonsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfMeasurementsNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.readingDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // measurementModeLabel
            // 
            this.measurementModeLabel.AutoSize = true;
            this.measurementModeLabel.Location = new System.Drawing.Point(3, 59);
            this.measurementModeLabel.Name = "measurementModeLabel";
            this.measurementModeLabel.Size = new System.Drawing.Size(104, 13);
            this.measurementModeLabel.TabIndex = 3;
            this.measurementModeLabel.Text = "Measurement Mode:";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(3, 26);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 2;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // measurementModeComboBox
            // 
            this.measurementModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.measurementModeComboBox.FormattingEnabled = true;
            this.measurementModeComboBox.Location = new System.Drawing.Point(139, 55);
            this.measurementModeComboBox.Name = "measurementModeComboBox";
            this.measurementModeComboBox.Size = new System.Drawing.Size(118, 21);
            this.measurementModeComboBox.TabIndex = 1;
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(139, 22);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(118, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // resourceAndMeasurementGroupBox
            // 
            this.resourceAndMeasurementGroupBox.Controls.Add(this.measurementModeLabel);
            this.resourceAndMeasurementGroupBox.Controls.Add(this.resourceNameLabel);
            this.resourceAndMeasurementGroupBox.Controls.Add(this.measurementModeComboBox);
            this.resourceAndMeasurementGroupBox.Controls.Add(this.resourceNameComboBox);
            this.resourceAndMeasurementGroupBox.Location = new System.Drawing.Point(12, 12);
            this.resourceAndMeasurementGroupBox.Name = "resourceAndMeasurementGroupBox";
            this.resourceAndMeasurementGroupBox.Size = new System.Drawing.Size(264, 85);
            this.resourceAndMeasurementGroupBox.TabIndex = 0;
            this.resourceAndMeasurementGroupBox.TabStop = false;
            this.resourceAndMeasurementGroupBox.Text = "Resource Name and Measurement Type";
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.rangeNumericUpDown);
            this.configurationGroupBox.Controls.Add(this.powerlineFrequencyLabel);
            this.configurationGroupBox.Controls.Add(this.resolutionLabel);
            this.configurationGroupBox.Controls.Add(this.rangeLabel);
            this.configurationGroupBox.Controls.Add(this.powerlineFrequencyValueComboBox);
            this.configurationGroupBox.Controls.Add(this.resolutionValueComboBox);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 103);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(265, 124);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // rangeNumericUpDown
            // 
            this.rangeNumericUpDown.DecimalPlaces = 3;
            this.rangeNumericUpDown.Location = new System.Drawing.Point(140, 23);
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
            // powerlineFrequencyLabel
            // 
            this.powerlineFrequencyLabel.AutoSize = true;
            this.powerlineFrequencyLabel.Location = new System.Drawing.Point(3, 87);
            this.powerlineFrequencyLabel.Name = "powerlineFrequencyLabel";
            this.powerlineFrequencyLabel.Size = new System.Drawing.Size(131, 13);
            this.powerlineFrequencyLabel.TabIndex = 7;
            this.powerlineFrequencyLabel.Text = "Powerline Frequency (Hz):";
            // 
            // resolutionLabel
            // 
            this.resolutionLabel.AutoSize = true;
            this.resolutionLabel.Location = new System.Drawing.Point(3, 57);
            this.resolutionLabel.Name = "resolutionLabel";
            this.resolutionLabel.Size = new System.Drawing.Size(95, 13);
            this.resolutionLabel.TabIndex = 6;
            this.resolutionLabel.Text = "Resolution (Digits):";
            // 
            // rangeLabel
            // 
            this.rangeLabel.AutoSize = true;
            this.rangeLabel.Location = new System.Drawing.Point(3, 27);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.Size = new System.Drawing.Size(42, 13);
            this.rangeLabel.TabIndex = 5;
            this.rangeLabel.Text = "Range:";
            // 
            // powerlineFrequencyValueComboBox
            // 
            this.powerlineFrequencyValueComboBox.FormattingEnabled = true;
            this.powerlineFrequencyValueComboBox.Location = new System.Drawing.Point(139, 83);
            this.powerlineFrequencyValueComboBox.Name = "powerlineFrequencyValueComboBox";
            this.powerlineFrequencyValueComboBox.Size = new System.Drawing.Size(118, 21);
            this.powerlineFrequencyValueComboBox.TabIndex = 2;
            // 
            // resolutionValueComboBox
            // 
            this.resolutionValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resolutionValueComboBox.FormattingEnabled = true;
            this.resolutionValueComboBox.Location = new System.Drawing.Point(139, 53);
            this.resolutionValueComboBox.Name = "resolutionValueComboBox";
            this.resolutionValueComboBox.Size = new System.Drawing.Size(118, 21);
            this.resolutionValueComboBox.TabIndex = 1;
            // 
            // errorGroupBox
            // 
            this.errorGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.errorGroupBox.Controls.Add(this.messageTextBox);
            this.errorGroupBox.Location = new System.Drawing.Point(12, 332);
            this.errorGroupBox.Name = "errorGroupBox";
            this.errorGroupBox.Size = new System.Drawing.Size(264, 229);
            this.errorGroupBox.TabIndex = 4;
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
            this.messageTextBox.Size = new System.Drawing.Size(258, 210);
            this.messageTextBox.TabIndex = 0;
            // 
            // triggerGroupBox
            // 
            this.triggerGroupBox.Controls.Add(this.triggerDelayNumericUpDown);
            this.triggerGroupBox.Controls.Add(this.triggerDelayLabel);
            this.triggerGroupBox.Controls.Add(this.triggerSourceLabel);
            this.triggerGroupBox.Controls.Add(this.triggerSourceComboBox);
            this.triggerGroupBox.Location = new System.Drawing.Point(12, 233);
            this.triggerGroupBox.Name = "triggerGroupBox";
            this.triggerGroupBox.Size = new System.Drawing.Size(265, 93);
            this.triggerGroupBox.TabIndex = 2;
            this.triggerGroupBox.TabStop = false;
            this.triggerGroupBox.Text = "Trigger";
            // 
            // triggerDelayNumericUpDown
            // 
            this.triggerDelayNumericUpDown.DecimalPlaces = 2;
            this.triggerDelayNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.triggerDelayNumericUpDown.Location = new System.Drawing.Point(139, 54);
            this.triggerDelayNumericUpDown.Maximum = new decimal(new int[] {
            149,
            0,
            0,
            0});
            this.triggerDelayNumericUpDown.Name = "triggerDelayNumericUpDown";
            this.triggerDelayNumericUpDown.Size = new System.Drawing.Size(118, 20);
            this.triggerDelayNumericUpDown.TabIndex = 1;
            // 
            // triggerDelayLabel
            // 
            this.triggerDelayLabel.AutoSize = true;
            this.triggerDelayLabel.Location = new System.Drawing.Point(3, 58);
            this.triggerDelayLabel.Name = "triggerDelayLabel";
            this.triggerDelayLabel.Size = new System.Drawing.Size(87, 13);
            this.triggerDelayLabel.TabIndex = 4;
            this.triggerDelayLabel.Text = "Trigger Delay (s):";
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.AutoSize = true;
            this.triggerSourceLabel.Location = new System.Drawing.Point(3, 27);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(80, 13);
            this.triggerSourceLabel.TabIndex = 2;
            this.triggerSourceLabel.Text = "Trigger Source:";
            // 
            // triggerSourceComboBox
            // 
            this.triggerSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerSourceComboBox.FormattingEnabled = true;
            this.triggerSourceComboBox.Location = new System.Drawing.Point(138, 23);
            this.triggerSourceComboBox.Name = "triggerSourceComboBox";
            this.triggerSourceComboBox.Size = new System.Drawing.Size(119, 21);
            this.triggerSourceComboBox.TabIndex = 0;
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.measurementGroupBox.Controls.Add(this.buttonsPanel);
            this.measurementGroupBox.Controls.Add(this.numberOfMeasurementsNumericUpDown);
            this.measurementGroupBox.Controls.Add(this.actualRangeTextBox);
            this.measurementGroupBox.Controls.Add(this.actualRangeLabel);
            this.measurementGroupBox.Controls.Add(this.readingDataGridView);
            this.measurementGroupBox.Controls.Add(this.numberOfMeasurementsLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(283, 12);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(330, 549);
            this.measurementGroupBox.TabIndex = 3;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // buttonsPanel
            // 
            this.buttonsPanel.Controls.Add(this.readButton);
            this.buttonsPanel.Controls.Add(this.clearButton);
            this.buttonsPanel.Controls.Add(this.softwareTriggerButton);
            this.buttonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonsPanel.Location = new System.Drawing.Point(3, 508);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(324, 38);
            this.buttonsPanel.TabIndex = 3;
            // 
            // readButton
            // 
            this.readButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.readButton.Location = new System.Drawing.Point(18, 8);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(75, 23);
            this.readButton.TabIndex = 0;
            this.readButton.Text = "&Read";
            this.readButton.UseVisualStyleBackColor = true;
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.clearButton.Location = new System.Drawing.Point(227, 8);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 2;
            this.clearButton.Text = "&Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // softwareTriggerButton
            // 
            this.softwareTriggerButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.softwareTriggerButton.Enabled = false;
            this.softwareTriggerButton.Location = new System.Drawing.Point(99, 8);
            this.softwareTriggerButton.Name = "softwareTriggerButton";
            this.softwareTriggerButton.Size = new System.Drawing.Size(122, 23);
            this.softwareTriggerButton.TabIndex = 1;
            this.softwareTriggerButton.Text = "&Send Software Trigger";
            this.softwareTriggerButton.UseVisualStyleBackColor = true;
            this.softwareTriggerButton.Click += new System.EventHandler(this.softwareTriggerButton_Click);
            // 
            // numberOfMeasurementsNumericUpDown
            // 
            this.numberOfMeasurementsNumericUpDown.Location = new System.Drawing.Point(194, 22);
            this.numberOfMeasurementsNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numberOfMeasurementsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfMeasurementsNumericUpDown.Name = "numberOfMeasurementsNumericUpDown";
            this.numberOfMeasurementsNumericUpDown.Size = new System.Drawing.Size(100, 20);
            this.numberOfMeasurementsNumericUpDown.TabIndex = 0;
            this.numberOfMeasurementsNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // actualRangeTextBox
            // 
            this.actualRangeTextBox.Location = new System.Drawing.Point(194, 51);
            this.actualRangeTextBox.Name = "actualRangeTextBox";
            this.actualRangeTextBox.ReadOnly = true;
            this.actualRangeTextBox.Size = new System.Drawing.Size(100, 20);
            this.actualRangeTextBox.TabIndex = 1;
            // 
            // actualRangeLabel
            // 
            this.actualRangeLabel.AutoSize = true;
            this.actualRangeLabel.Location = new System.Drawing.Point(29, 55);
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
            this.ReadingNo,
            this.reading});
            this.readingDataGridView.Location = new System.Drawing.Point(32, 91);
            this.readingDataGridView.Name = "readingDataGridView";
            this.readingDataGridView.ReadOnly = true;
            this.readingDataGridView.RowHeadersWidth = 15;
            this.readingDataGridView.RowTemplate.Height = 24;
            this.readingDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.readingDataGridView.Size = new System.Drawing.Size(262, 400);
            this.readingDataGridView.StandardTab = true;
            this.readingDataGridView.TabIndex = 2;
            // 
            // ReadingNo
            // 
            this.ReadingNo.HeaderText = "Point";
            this.ReadingNo.Name = "ReadingNo";
            this.ReadingNo.ReadOnly = true;
            // 
            // reading
            // 
            this.reading.HeaderText = "Amplitude";
            this.reading.Name = "reading";
            this.reading.ReadOnly = true;
            this.reading.Width = 249;
            // 
            // numberOfMeasurementsLabel
            // 
            this.numberOfMeasurementsLabel.AutoSize = true;
            this.numberOfMeasurementsLabel.Location = new System.Drawing.Point(29, 26);
            this.numberOfMeasurementsLabel.Name = "numberOfMeasurementsLabel";
            this.numberOfMeasurementsLabel.Size = new System.Drawing.Size(133, 13);
            this.numberOfMeasurementsLabel.TabIndex = 0;
            this.numberOfMeasurementsLabel.Text = "Number Of Measurements:";
            // 
            // MainForm
            // 
            this.AcceptButton = this.readButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(607, 573);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.triggerGroupBox);
            this.Controls.Add(this.errorGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.resourceAndMeasurementGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(623, 1000);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(623, 600);
            this.Name = "MainForm";
            this.Text = "Software Triggered MultiPoint Aquisition";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainFormForm_Closing);
            this.resourceAndMeasurementGroupBox.ResumeLayout(false);
            this.resourceAndMeasurementGroupBox.PerformLayout();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangeNumericUpDown)).EndInit();
            this.errorGroupBox.ResumeLayout(false);
            this.errorGroupBox.PerformLayout();
            this.triggerGroupBox.ResumeLayout(false);
            this.triggerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.triggerDelayNumericUpDown)).EndInit();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.buttonsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numberOfMeasurementsNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.readingDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label measurementModeLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.ComboBox measurementModeComboBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.GroupBox resourceAndMeasurementGroupBox;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.NumericUpDown rangeNumericUpDown;
        private System.Windows.Forms.Label powerlineFrequencyLabel;
        private System.Windows.Forms.Label resolutionLabel;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.ComboBox powerlineFrequencyValueComboBox;
        private System.Windows.Forms.ComboBox resolutionValueComboBox;
        private System.Windows.Forms.GroupBox errorGroupBox;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.GroupBox triggerGroupBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;
        private System.Windows.Forms.Label numberOfMeasurementsLabel;
        private System.Windows.Forms.TextBox actualRangeTextBox;
        private System.Windows.Forms.Label actualRangeLabel;
        private System.Windows.Forms.DataGridView readingDataGridView;
        private System.Windows.Forms.Label triggerDelayLabel;
        private System.Windows.Forms.Label triggerSourceLabel;
        private System.Windows.Forms.ComboBox triggerSourceComboBox;
        private System.Windows.Forms.Button softwareTriggerButton;
        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.NumericUpDown triggerDelayNumericUpDown;
        private System.Windows.Forms.NumericUpDown numberOfMeasurementsNumericUpDown;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Panel buttonsPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReadingNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn reading;
    }
}

