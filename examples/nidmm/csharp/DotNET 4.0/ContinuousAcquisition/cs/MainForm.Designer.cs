namespace NationalInstruments.Examples.ContinuousAcquisition
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
            this.acConfigurationGroupBox = new System.Windows.Forms.GroupBox();
            this.maxACFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.minACFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.maxACFrequencyLabel = new System.Windows.Forms.Label();
            this.minACFrequencyLabel = new System.Windows.Forms.Label();
            this.ResourceAndMeasurementGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementTypeLabel = new System.Windows.Forms.Label();
            this.measurementModeComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.samplesPerReadingNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.samplesPerReadingLabel = new System.Windows.Forms.Label();
            this.rangeTextBox = new System.Windows.Forms.TextBox();
            this.powerlineFrequencyLabel = new System.Windows.Forms.Label();
            this.resolutionLabel = new System.Windows.Forms.Label();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.powerlineFrequencyValueComboBox = new System.Windows.Forms.ComboBox();
            this.resolutionValueComboBox = new System.Windows.Forms.ComboBox();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.numberOfSamplesTextBox = new System.Windows.Forms.TextBox();
            this.numberOfSamplesLabel = new System.Windows.Forms.Label();
            this.maxReadingTextBox = new System.Windows.Forms.TextBox();
            this.minReadingTextBox = new System.Windows.Forms.TextBox();
            this.minReadingLabel = new System.Windows.Forms.Label();
            this.maxReadingLabel = new System.Windows.Forms.Label();
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.acquireButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.averageReadingLabel = new System.Windows.Forms.Label();
            this.averageReadingTextBox = new System.Windows.Forms.TextBox();
            this.actualRangeLabel = new System.Windows.Forms.Label();
            this.actualRangeTextBox = new System.Windows.Forms.TextBox();
            this.messageGroupBox = new System.Windows.Forms.GroupBox();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.acConfigurationGroupBox.SuspendLayout();
            this.ResourceAndMeasurementGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerReadingNumericUpDown)).BeginInit();
            this.measurementGroupBox.SuspendLayout();
            this.buttonsPanel.SuspendLayout();
            this.messageGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // acConfigurationGroupBox
            // 
            this.acConfigurationGroupBox.Controls.Add(this.maxACFrequencyTextBox);
            this.acConfigurationGroupBox.Controls.Add(this.minACFrequencyTextBox);
            this.acConfigurationGroupBox.Controls.Add(this.maxACFrequencyLabel);
            this.acConfigurationGroupBox.Controls.Add(this.minACFrequencyLabel);
            this.acConfigurationGroupBox.Enabled = false;
            this.acConfigurationGroupBox.Location = new System.Drawing.Point(12, 258);
            this.acConfigurationGroupBox.Name = "acConfigurationGroupBox";
            this.acConfigurationGroupBox.Size = new System.Drawing.Size(269, 80);
            this.acConfigurationGroupBox.TabIndex = 2;
            this.acConfigurationGroupBox.TabStop = false;
            this.acConfigurationGroupBox.Text = "AC Configuration";
            // 
            // maxACFrequencyTextBox
            // 
            this.maxACFrequencyTextBox.Location = new System.Drawing.Point(154, 48);
            this.maxACFrequencyTextBox.Name = "maxACFrequencyTextBox";
            this.maxACFrequencyTextBox.Size = new System.Drawing.Size(107, 20);
            this.maxACFrequencyTextBox.TabIndex = 1;
            this.maxACFrequencyTextBox.Text = "25.00E+03";
            // 
            // minACFrequencyTextBox
            // 
            this.minACFrequencyTextBox.Location = new System.Drawing.Point(154, 16);
            this.minACFrequencyTextBox.Name = "minACFrequencyTextBox";
            this.minACFrequencyTextBox.Size = new System.Drawing.Size(107, 20);
            this.minACFrequencyTextBox.TabIndex = 0;
            this.minACFrequencyTextBox.Text = "20.00E+0";
            // 
            // maxACFrequencyLabel
            // 
            this.maxACFrequencyLabel.AutoSize = true;
            this.maxACFrequencyLabel.Location = new System.Drawing.Point(6, 50);
            this.maxACFrequencyLabel.Name = "maxACFrequencyLabel";
            this.maxACFrequencyLabel.Size = new System.Drawing.Size(146, 13);
            this.maxACFrequencyLabel.TabIndex = 3;
            this.maxACFrequencyLabel.Text = "Maximum AC Frequency (Hz):";
            // 
            // minACFrequencyLabel
            // 
            this.minACFrequencyLabel.AutoSize = true;
            this.minACFrequencyLabel.Location = new System.Drawing.Point(6, 19);
            this.minACFrequencyLabel.Name = "minACFrequencyLabel";
            this.minACFrequencyLabel.Size = new System.Drawing.Size(143, 13);
            this.minACFrequencyLabel.TabIndex = 2;
            this.minACFrequencyLabel.Text = "Minimum AC Frequency (Hz):";
            // 
            // ResourceAndMeasurementGroupBox
            // 
            this.ResourceAndMeasurementGroupBox.Controls.Add(this.measurementTypeLabel);
            this.ResourceAndMeasurementGroupBox.Controls.Add(this.measurementModeComboBox);
            this.ResourceAndMeasurementGroupBox.Controls.Add(this.resourceNameLabel);
            this.ResourceAndMeasurementGroupBox.Controls.Add(this.resourceNameComboBox);
            this.ResourceAndMeasurementGroupBox.Location = new System.Drawing.Point(12, 12);
            this.ResourceAndMeasurementGroupBox.Name = "ResourceAndMeasurementGroupBox";
            this.ResourceAndMeasurementGroupBox.Size = new System.Drawing.Size(269, 93);
            this.ResourceAndMeasurementGroupBox.TabIndex = 0;
            this.ResourceAndMeasurementGroupBox.TabStop = false;
            this.ResourceAndMeasurementGroupBox.Text = "Resource Name and Measurement Type";
            // 
            // measurementTypeLabel
            // 
            this.measurementTypeLabel.AutoSize = true;
            this.measurementTypeLabel.Location = new System.Drawing.Point(6, 63);
            this.measurementTypeLabel.Name = "measurementTypeLabel";
            this.measurementTypeLabel.Size = new System.Drawing.Size(104, 13);
            this.measurementTypeLabel.TabIndex = 3;
            this.measurementTypeLabel.Text = "Measurement Mode:";
            // 
            // measurementModeComboBox
            // 
            this.measurementModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.measurementModeComboBox.FormattingEnabled = true;
            this.measurementModeComboBox.Location = new System.Drawing.Point(154, 59);
            this.measurementModeComboBox.Name = "measurementModeComboBox";
            this.measurementModeComboBox.Size = new System.Drawing.Size(107, 21);
            this.measurementModeComboBox.TabIndex = 1;
            this.measurementModeComboBox.SelectedIndexChanged += new System.EventHandler(this.measurementModeComboBox_SelectedIndexChanged);
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 28);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 1;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(154, 25);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(107, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.samplesPerReadingNumericUpDown);
            this.configurationGroupBox.Controls.Add(this.samplesPerReadingLabel);
            this.configurationGroupBox.Controls.Add(this.rangeTextBox);
            this.configurationGroupBox.Controls.Add(this.powerlineFrequencyLabel);
            this.configurationGroupBox.Controls.Add(this.resolutionLabel);
            this.configurationGroupBox.Controls.Add(this.rangeLabel);
            this.configurationGroupBox.Controls.Add(this.powerlineFrequencyValueComboBox);
            this.configurationGroupBox.Controls.Add(this.resolutionValueComboBox);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 111);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(269, 141);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // samplesPerReadingNumericUpDown
            // 
            this.samplesPerReadingNumericUpDown.Location = new System.Drawing.Point(154, 112);
            this.samplesPerReadingNumericUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.samplesPerReadingNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.samplesPerReadingNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.samplesPerReadingNumericUpDown.Name = "samplesPerReadingNumericUpDown";
            this.samplesPerReadingNumericUpDown.Size = new System.Drawing.Size(106, 20);
            this.samplesPerReadingNumericUpDown.TabIndex = 9;
            this.samplesPerReadingNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // samplesPerReadingLabel
            // 
            this.samplesPerReadingLabel.AutoSize = true;
            this.samplesPerReadingLabel.Location = new System.Drawing.Point(6, 115);
            this.samplesPerReadingLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.samplesPerReadingLabel.Name = "samplesPerReadingLabel";
            this.samplesPerReadingLabel.Size = new System.Drawing.Size(111, 13);
            this.samplesPerReadingLabel.TabIndex = 8;
            this.samplesPerReadingLabel.Text = "Samples per Reading:";
            // 
            // rangeTextBox
            // 
            this.rangeTextBox.Location = new System.Drawing.Point(154, 21);
            this.rangeTextBox.Name = "rangeTextBox";
            this.rangeTextBox.Size = new System.Drawing.Size(107, 20);
            this.rangeTextBox.TabIndex = 0;
            this.rangeTextBox.Text = "10";
            // 
            // powerlineFrequencyLabel
            // 
            this.powerlineFrequencyLabel.AutoSize = true;
            this.powerlineFrequencyLabel.Location = new System.Drawing.Point(6, 84);
            this.powerlineFrequencyLabel.Name = "powerlineFrequencyLabel";
            this.powerlineFrequencyLabel.Size = new System.Drawing.Size(131, 13);
            this.powerlineFrequencyLabel.TabIndex = 7;
            this.powerlineFrequencyLabel.Text = "Powerline Frequency (Hz):";
            // 
            // resolutionLabel
            // 
            this.resolutionLabel.AutoSize = true;
            this.resolutionLabel.Location = new System.Drawing.Point(6, 54);
            this.resolutionLabel.Name = "resolutionLabel";
            this.resolutionLabel.Size = new System.Drawing.Size(95, 13);
            this.resolutionLabel.TabIndex = 6;
            this.resolutionLabel.Text = "Resolution (Digits):";
            // 
            // rangeLabel
            // 
            this.rangeLabel.AutoSize = true;
            this.rangeLabel.Location = new System.Drawing.Point(6, 24);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.Size = new System.Drawing.Size(42, 13);
            this.rangeLabel.TabIndex = 5;
            this.rangeLabel.Text = "Range:";
            // 
            // powerlineFrequencyValueComboBox
            // 
            this.powerlineFrequencyValueComboBox.FormattingEnabled = true;
            this.powerlineFrequencyValueComboBox.Location = new System.Drawing.Point(154, 81);
            this.powerlineFrequencyValueComboBox.Name = "powerlineFrequencyValueComboBox";
            this.powerlineFrequencyValueComboBox.Size = new System.Drawing.Size(107, 21);
            this.powerlineFrequencyValueComboBox.TabIndex = 2;
            // 
            // resolutionValueComboBox
            // 
            this.resolutionValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resolutionValueComboBox.FormattingEnabled = true;
            this.resolutionValueComboBox.Location = new System.Drawing.Point(154, 50);
            this.resolutionValueComboBox.Name = "resolutionValueComboBox";
            this.resolutionValueComboBox.Size = new System.Drawing.Size(107, 21);
            this.resolutionValueComboBox.TabIndex = 1;
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.measurementGroupBox.Controls.Add(this.numberOfSamplesTextBox);
            this.measurementGroupBox.Controls.Add(this.numberOfSamplesLabel);
            this.measurementGroupBox.Controls.Add(this.maxReadingTextBox);
            this.measurementGroupBox.Controls.Add(this.minReadingTextBox);
            this.measurementGroupBox.Controls.Add(this.minReadingLabel);
            this.measurementGroupBox.Controls.Add(this.maxReadingLabel);
            this.measurementGroupBox.Controls.Add(this.buttonsPanel);
            this.measurementGroupBox.Controls.Add(this.averageReadingLabel);
            this.measurementGroupBox.Controls.Add(this.averageReadingTextBox);
            this.measurementGroupBox.Controls.Add(this.actualRangeLabel);
            this.measurementGroupBox.Controls.Add(this.actualRangeTextBox);
            this.measurementGroupBox.Location = new System.Drawing.Point(302, 13);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(262, 240);
            this.measurementGroupBox.TabIndex = 3;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // numberOfSamplesTextBox
            // 
            this.numberOfSamplesTextBox.Location = new System.Drawing.Point(154, 116);
            this.numberOfSamplesTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numberOfSamplesTextBox.Name = "numberOfSamplesTextBox";
            this.numberOfSamplesTextBox.ReadOnly = true;
            this.numberOfSamplesTextBox.Size = new System.Drawing.Size(99, 20);
            this.numberOfSamplesTextBox.TabIndex = 3;
            // 
            // numberOfSamplesLabel
            // 
            this.numberOfSamplesLabel.AutoSize = true;
            this.numberOfSamplesLabel.Location = new System.Drawing.Point(6, 119);
            this.numberOfSamplesLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.numberOfSamplesLabel.Name = "numberOfSamplesLabel";
            this.numberOfSamplesLabel.Size = new System.Drawing.Size(102, 13);
            this.numberOfSamplesLabel.TabIndex = 11;
            this.numberOfSamplesLabel.Text = "Number of Samples:";
            // 
            // maxReadingTextBox
            // 
            this.maxReadingTextBox.Location = new System.Drawing.Point(154, 83);
            this.maxReadingTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.maxReadingTextBox.Name = "maxReadingTextBox";
            this.maxReadingTextBox.ReadOnly = true;
            this.maxReadingTextBox.Size = new System.Drawing.Size(100, 20);
            this.maxReadingTextBox.TabIndex = 2;
            // 
            // minReadingTextBox
            // 
            this.minReadingTextBox.Location = new System.Drawing.Point(154, 49);
            this.minReadingTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.minReadingTextBox.Name = "minReadingTextBox";
            this.minReadingTextBox.ReadOnly = true;
            this.minReadingTextBox.Size = new System.Drawing.Size(100, 20);
            this.minReadingTextBox.TabIndex = 1;
            // 
            // minReadingLabel
            // 
            this.minReadingLabel.AutoSize = true;
            this.minReadingLabel.Location = new System.Drawing.Point(6, 51);
            this.minReadingLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.minReadingLabel.Name = "minReadingLabel";
            this.minReadingLabel.Size = new System.Drawing.Size(70, 13);
            this.minReadingLabel.TabIndex = 9;
            this.minReadingLabel.Text = "Min Reading:";
            // 
            // maxReadingLabel
            // 
            this.maxReadingLabel.AutoSize = true;
            this.maxReadingLabel.Location = new System.Drawing.Point(6, 85);
            this.maxReadingLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.maxReadingLabel.Name = "maxReadingLabel";
            this.maxReadingLabel.Size = new System.Drawing.Size(73, 13);
            this.maxReadingLabel.TabIndex = 9;
            this.maxReadingLabel.Text = "Max Reading:";
            // 
            // buttonsPanel
            // 
            this.buttonsPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonsPanel.Controls.Add(this.acquireButton);
            this.buttonsPanel.Controls.Add(this.stopButton);
            this.buttonsPanel.Location = new System.Drawing.Point(8, 190);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(247, 35);
            this.buttonsPanel.TabIndex = 5;
            // 
            // acquireButton
            // 
            this.acquireButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.acquireButton.Location = new System.Drawing.Point(24, 11);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(78, 21);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(147, 11);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(78, 21);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "&Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // averageReadingLabel
            // 
            this.averageReadingLabel.AutoSize = true;
            this.averageReadingLabel.Location = new System.Drawing.Point(6, 21);
            this.averageReadingLabel.Name = "averageReadingLabel";
            this.averageReadingLabel.Size = new System.Drawing.Size(93, 13);
            this.averageReadingLabel.TabIndex = 2;
            this.averageReadingLabel.Text = "Average Reading:";
            // 
            // averageReadingTextBox
            // 
            this.averageReadingTextBox.Location = new System.Drawing.Point(154, 19);
            this.averageReadingTextBox.Name = "averageReadingTextBox";
            this.averageReadingTextBox.ReadOnly = true;
            this.averageReadingTextBox.Size = new System.Drawing.Size(100, 20);
            this.averageReadingTextBox.TabIndex = 0;
            // 
            // actualRangeLabel
            // 
            this.actualRangeLabel.AutoSize = true;
            this.actualRangeLabel.Location = new System.Drawing.Point(6, 150);
            this.actualRangeLabel.Name = "actualRangeLabel";
            this.actualRangeLabel.Size = new System.Drawing.Size(75, 13);
            this.actualRangeLabel.TabIndex = 8;
            this.actualRangeLabel.Text = "Actual Range:";
            // 
            // actualRangeTextBox
            // 
            this.actualRangeTextBox.Location = new System.Drawing.Point(154, 147);
            this.actualRangeTextBox.Name = "actualRangeTextBox";
            this.actualRangeTextBox.ReadOnly = true;
            this.actualRangeTextBox.Size = new System.Drawing.Size(100, 20);
            this.actualRangeTextBox.TabIndex = 4;
            // 
            // messageGroupBox
            // 
            this.messageGroupBox.Controls.Add(this.messageTextBox);
            this.messageGroupBox.Location = new System.Drawing.Point(302, 258);
            this.messageGroupBox.Name = "messageGroupBox";
            this.messageGroupBox.Size = new System.Drawing.Size(269, 81);
            this.messageGroupBox.TabIndex = 4;
            this.messageGroupBox.TabStop = false;
            this.messageGroupBox.Text = "Message";
            // 
            // messageTextBox
            // 
            this.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageTextBox.Location = new System.Drawing.Point(3, 14);
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messageTextBox.Size = new System.Drawing.Size(263, 64);
            this.messageTextBox.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AcceptButton = this.acquireButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.stopButton;
            this.ClientSize = new System.Drawing.Size(575, 351);
            this.Controls.Add(this.messageGroupBox);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.acConfigurationGroupBox);
            this.Controls.Add(this.ResourceAndMeasurementGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(581, 379);
            this.MinimumSize = new System.Drawing.Size(581, 379);
            this.Name = "MainForm";
            this.Text = "Continuous Acquisition";
            this.acConfigurationGroupBox.ResumeLayout(false);
            this.acConfigurationGroupBox.PerformLayout();
            this.ResourceAndMeasurementGroupBox.ResumeLayout(false);
            this.ResourceAndMeasurementGroupBox.PerformLayout();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerReadingNumericUpDown)).EndInit();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.buttonsPanel.ResumeLayout(false);
            this.messageGroupBox.ResumeLayout(false);
            this.messageGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox acConfigurationGroupBox;
        private System.Windows.Forms.TextBox maxACFrequencyTextBox;
        private System.Windows.Forms.TextBox minACFrequencyTextBox;
        private System.Windows.Forms.Label maxACFrequencyLabel;
        private System.Windows.Forms.Label minACFrequencyLabel;
        private System.Windows.Forms.GroupBox ResourceAndMeasurementGroupBox;
        private System.Windows.Forms.Label measurementTypeLabel;
        private System.Windows.Forms.ComboBox measurementModeComboBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.TextBox rangeTextBox;
        private System.Windows.Forms.Label powerlineFrequencyLabel;
        private System.Windows.Forms.Label resolutionLabel;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.ComboBox powerlineFrequencyValueComboBox;
        private System.Windows.Forms.ComboBox resolutionValueComboBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.Label actualRangeLabel;
        private System.Windows.Forms.TextBox actualRangeTextBox;
        private System.Windows.Forms.GroupBox messageGroupBox;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.Label averageReadingLabel;
        private System.Windows.Forms.TextBox averageReadingTextBox;
        private System.Windows.Forms.Panel buttonsPanel;
        private System.Windows.Forms.TextBox maxReadingTextBox;
        private System.Windows.Forms.TextBox minReadingTextBox;
        private System.Windows.Forms.Label minReadingLabel;
        private System.Windows.Forms.Label maxReadingLabel;
        private System.Windows.Forms.Label numberOfSamplesLabel;
        private System.Windows.Forms.TextBox numberOfSamplesTextBox;
        private System.Windows.Forms.Label samplesPerReadingLabel;
        private System.Windows.Forms.NumericUpDown samplesPerReadingNumericUpDown;
    }
}

