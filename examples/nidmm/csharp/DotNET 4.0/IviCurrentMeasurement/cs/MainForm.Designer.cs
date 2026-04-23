namespace NationalInstruments.Examples.IviCurrentMeasurement
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
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.actualRangeLabel = new System.Windows.Forms.Label();
            this.actualRangeTextBox = new System.Windows.Forms.TextBox();
            this.readButton = new System.Windows.Forms.Button();
            this.measurementLabel = new System.Windows.Forms.Label();
            this.measurementTextBox = new System.Windows.Forms.TextBox();
            this.powerlineFrequencyLabel = new System.Windows.Forms.Label();
            this.resolutionLabel = new System.Windows.Forms.Label();
            this.ResourceAndMeasurementGroupBox = new System.Windows.Forms.GroupBox();
            this.deviceNameTextBox = new System.Windows.Forms.TextBox();
            this.measurementTypeLabel = new System.Windows.Forms.Label();
            this.measurementModeComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.resolutionValueComboBox = new System.Windows.Forms.ComboBox();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.rangeTextBox = new System.Windows.Forms.TextBox();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.powerlineFrequencyValueComboBox = new System.Windows.Forms.ComboBox();
            this.acConfigurationGroupBox = new System.Windows.Forms.GroupBox();
            this.maxACFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.minACFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.maxACFrequencyLabel = new System.Windows.Forms.Label();
            this.minACFrequencyLabel = new System.Windows.Forms.Label();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.messageGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementGroupBox.SuspendLayout();
            this.ResourceAndMeasurementGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            this.acConfigurationGroupBox.SuspendLayout();
            this.messageGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualRangeLabel);
            this.measurementGroupBox.Controls.Add(this.actualRangeTextBox);
            this.measurementGroupBox.Controls.Add(this.readButton);
            this.measurementGroupBox.Controls.Add(this.measurementLabel);
            this.measurementGroupBox.Controls.Add(this.measurementTextBox);
            this.measurementGroupBox.Location = new System.Drawing.Point(287, 149);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(277, 120);
            this.measurementGroupBox.TabIndex = 3;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // actualRangeLabel
            // 
            this.actualRangeLabel.AutoSize = true;
            this.actualRangeLabel.Location = new System.Drawing.Point(15, 51);
            this.actualRangeLabel.Name = "actualRangeLabel";
            this.actualRangeLabel.Size = new System.Drawing.Size(75, 13);
            this.actualRangeLabel.TabIndex = 6;
            this.actualRangeLabel.Text = "Actual Range:";
            // 
            // actualRangeTextBox
            // 
            this.actualRangeTextBox.Location = new System.Drawing.Point(157, 49);
            this.actualRangeTextBox.Name = "actualRangeTextBox";
            this.actualRangeTextBox.ReadOnly = true;
            this.actualRangeTextBox.Size = new System.Drawing.Size(108, 20);
            this.actualRangeTextBox.TabIndex = 1;
            // 
            // readButton
            // 
            this.readButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.readButton.Location = new System.Drawing.Point(157, 86);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(78, 21);
            this.readButton.TabIndex = 2;
            this.readButton.Text = "&Read";
            this.readButton.UseVisualStyleBackColor = true;
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // measurementLabel
            // 
            this.measurementLabel.AutoSize = true;
            this.measurementLabel.Location = new System.Drawing.Point(15, 23);
            this.measurementLabel.Name = "measurementLabel";
            this.measurementLabel.Size = new System.Drawing.Size(74, 13);
            this.measurementLabel.TabIndex = 1;
            this.measurementLabel.Text = "Measurement:";
            // 
            // measurementTextBox
            // 
            this.measurementTextBox.Location = new System.Drawing.Point(157, 20);
            this.measurementTextBox.Name = "measurementTextBox";
            this.measurementTextBox.ReadOnly = true;
            this.measurementTextBox.Size = new System.Drawing.Size(108, 20);
            this.measurementTextBox.TabIndex = 0;
            // 
            // powerlineFrequencyLabel
            // 
            this.powerlineFrequencyLabel.AutoSize = true;
            this.powerlineFrequencyLabel.Location = new System.Drawing.Point(15, 92);
            this.powerlineFrequencyLabel.Name = "powerlineFrequencyLabel";
            this.powerlineFrequencyLabel.Size = new System.Drawing.Size(131, 13);
            this.powerlineFrequencyLabel.TabIndex = 7;
            this.powerlineFrequencyLabel.Text = "Powerline Frequency (Hz):";
            // 
            // resolutionLabel
            // 
            this.resolutionLabel.AutoSize = true;
            this.resolutionLabel.Location = new System.Drawing.Point(15, 59);
            this.resolutionLabel.Name = "resolutionLabel";
            this.resolutionLabel.Size = new System.Drawing.Size(95, 13);
            this.resolutionLabel.TabIndex = 6;
            this.resolutionLabel.Text = "Resolution (Digits):";
            // 
            // ResourceAndMeasurementGroupBox
            // 
            this.ResourceAndMeasurementGroupBox.Controls.Add(this.deviceNameTextBox);
            this.ResourceAndMeasurementGroupBox.Controls.Add(this.measurementTypeLabel);
            this.ResourceAndMeasurementGroupBox.Controls.Add(this.measurementModeComboBox);
            this.ResourceAndMeasurementGroupBox.Controls.Add(this.resourceNameLabel);
            this.ResourceAndMeasurementGroupBox.Location = new System.Drawing.Point(5, 12);
            this.ResourceAndMeasurementGroupBox.Name = "ResourceAndMeasurementGroupBox";
            this.ResourceAndMeasurementGroupBox.Size = new System.Drawing.Size(269, 86);
            this.ResourceAndMeasurementGroupBox.TabIndex = 0;
            this.ResourceAndMeasurementGroupBox.TabStop = false;
            this.ResourceAndMeasurementGroupBox.Text = "Resource Name and Measurement Type";
            // 
            // deviceNameTextBox
            // 
            this.deviceNameTextBox.Location = new System.Drawing.Point(158, 25);
            this.deviceNameTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.deviceNameTextBox.Name = "deviceNameTextBox";
            this.deviceNameTextBox.Size = new System.Drawing.Size(107, 20);
            this.deviceNameTextBox.TabIndex = 0;
            // 
            // measurementTypeLabel
            // 
            this.measurementTypeLabel.AutoSize = true;
            this.measurementTypeLabel.Location = new System.Drawing.Point(6, 59);
            this.measurementTypeLabel.Name = "measurementTypeLabel";
            this.measurementTypeLabel.Size = new System.Drawing.Size(104, 13);
            this.measurementTypeLabel.TabIndex = 3;
            this.measurementTypeLabel.Text = "Measurement Mode:";
            // 
            // measurementModeComboBox
            // 
            this.measurementModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.measurementModeComboBox.FormattingEnabled = true;
            this.measurementModeComboBox.Location = new System.Drawing.Point(158, 56);
            this.measurementModeComboBox.Name = "measurementModeComboBox";
            this.measurementModeComboBox.Size = new System.Drawing.Size(107, 21);
            this.measurementModeComboBox.TabIndex = 1;
            this.measurementModeComboBox.SelectedIndexChanged += new System.EventHandler(this.measurementModeComboBox_SelectedIndexChanged);
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 25);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 1;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // resolutionValueComboBox
            // 
            this.resolutionValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resolutionValueComboBox.FormattingEnabled = true;
            this.resolutionValueComboBox.Location = new System.Drawing.Point(157, 54);
            this.resolutionValueComboBox.Name = "resolutionValueComboBox";
            this.resolutionValueComboBox.Size = new System.Drawing.Size(108, 21);
            this.resolutionValueComboBox.TabIndex = 1;
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.rangeTextBox);
            this.configurationGroupBox.Controls.Add(this.powerlineFrequencyLabel);
            this.configurationGroupBox.Controls.Add(this.resolutionLabel);
            this.configurationGroupBox.Controls.Add(this.rangeLabel);
            this.configurationGroupBox.Controls.Add(this.powerlineFrequencyValueComboBox);
            this.configurationGroupBox.Controls.Add(this.resolutionValueComboBox);
            this.configurationGroupBox.Location = new System.Drawing.Point(287, 12);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(277, 128);
            this.configurationGroupBox.TabIndex = 2;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // rangeTextBox
            // 
            this.rangeTextBox.Location = new System.Drawing.Point(157, 23);
            this.rangeTextBox.Name = "rangeTextBox";
            this.rangeTextBox.Size = new System.Drawing.Size(108, 20);
            this.rangeTextBox.TabIndex = 0;
            this.rangeTextBox.Text = "2.00E-001";
            // 
            // rangeLabel
            // 
            this.rangeLabel.AutoSize = true;
            this.rangeLabel.Location = new System.Drawing.Point(15, 25);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.Size = new System.Drawing.Size(42, 13);
            this.rangeLabel.TabIndex = 5;
            this.rangeLabel.Text = "Range:";
            // 
            // powerlineFrequencyValueComboBox
            // 
            this.powerlineFrequencyValueComboBox.FormattingEnabled = true;
            this.powerlineFrequencyValueComboBox.Location = new System.Drawing.Point(157, 88);
            this.powerlineFrequencyValueComboBox.Name = "powerlineFrequencyValueComboBox";
            this.powerlineFrequencyValueComboBox.Size = new System.Drawing.Size(108, 21);
            this.powerlineFrequencyValueComboBox.TabIndex = 2;
            // 
            // acConfigurationGroupBox
            // 
            this.acConfigurationGroupBox.Controls.Add(this.maxACFrequencyTextBox);
            this.acConfigurationGroupBox.Controls.Add(this.minACFrequencyTextBox);
            this.acConfigurationGroupBox.Controls.Add(this.maxACFrequencyLabel);
            this.acConfigurationGroupBox.Controls.Add(this.minACFrequencyLabel);
            this.acConfigurationGroupBox.Enabled = false;
            this.acConfigurationGroupBox.Location = new System.Drawing.Point(5, 104);
            this.acConfigurationGroupBox.Name = "acConfigurationGroupBox";
            this.acConfigurationGroupBox.Size = new System.Drawing.Size(269, 74);
            this.acConfigurationGroupBox.TabIndex = 1;
            this.acConfigurationGroupBox.TabStop = false;
            this.acConfigurationGroupBox.Text = "AC Configuration";
            // 
            // maxACFrequencyTextBox
            // 
            this.maxACFrequencyTextBox.Location = new System.Drawing.Point(158, 42);
            this.maxACFrequencyTextBox.Name = "maxACFrequencyTextBox";
            this.maxACFrequencyTextBox.Size = new System.Drawing.Size(107, 20);
            this.maxACFrequencyTextBox.TabIndex = 1;
            this.maxACFrequencyTextBox.Text = "25.00E+03";
            // 
            // minACFrequencyTextBox
            // 
            this.minACFrequencyTextBox.Location = new System.Drawing.Point(158, 14);
            this.minACFrequencyTextBox.Name = "minACFrequencyTextBox";
            this.minACFrequencyTextBox.Size = new System.Drawing.Size(107, 20);
            this.minACFrequencyTextBox.TabIndex = 0;
            this.minACFrequencyTextBox.Text = "20.00E+0";
            // 
            // maxACFrequencyLabel
            // 
            this.maxACFrequencyLabel.AutoSize = true;
            this.maxACFrequencyLabel.Location = new System.Drawing.Point(6, 45);
            this.maxACFrequencyLabel.Name = "maxACFrequencyLabel";
            this.maxACFrequencyLabel.Size = new System.Drawing.Size(146, 13);
            this.maxACFrequencyLabel.TabIndex = 3;
            this.maxACFrequencyLabel.Text = "Maximum AC Frequency (Hz):";
            // 
            // minACFrequencyLabel
            // 
            this.minACFrequencyLabel.AutoSize = true;
            this.minACFrequencyLabel.Location = new System.Drawing.Point(6, 16);
            this.minACFrequencyLabel.Name = "minACFrequencyLabel";
            this.minACFrequencyLabel.Size = new System.Drawing.Size(143, 13);
            this.minACFrequencyLabel.TabIndex = 2;
            this.minACFrequencyLabel.Text = "Minimum AC Frequency (Hz):";
            // 
            // messageTextBox
            // 
            this.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageTextBox.Location = new System.Drawing.Point(3, 16);
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messageTextBox.Size = new System.Drawing.Size(263, 66);
            this.messageTextBox.TabIndex = 0;
            // 
            // messageGroupBox
            // 
            this.messageGroupBox.Controls.Add(this.messageTextBox);
            this.messageGroupBox.Location = new System.Drawing.Point(5, 184);
            this.messageGroupBox.Name = "messageGroupBox";
            this.messageGroupBox.Size = new System.Drawing.Size(269, 85);
            this.messageGroupBox.TabIndex = 4;
            this.messageGroupBox.TabStop = false;
            this.messageGroupBox.Text = "Message";
            // 
            // MainForm
            // 
            this.AcceptButton = this.readButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 277);
            this.Controls.Add(this.messageGroupBox);
            this.Controls.Add(this.acConfigurationGroupBox);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.ResourceAndMeasurementGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "IVI Current Measurement";
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.ResourceAndMeasurementGroupBox.ResumeLayout(false);
            this.ResourceAndMeasurementGroupBox.PerformLayout();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.acConfigurationGroupBox.ResumeLayout(false);
            this.acConfigurationGroupBox.PerformLayout();
            this.messageGroupBox.ResumeLayout(false);
            this.messageGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox measurementGroupBox;
        private System.Windows.Forms.Label actualRangeLabel;
        private System.Windows.Forms.TextBox actualRangeTextBox;
        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.Label measurementLabel;
        private System.Windows.Forms.TextBox measurementTextBox;
        private System.Windows.Forms.Label powerlineFrequencyLabel;
        private System.Windows.Forms.Label resolutionLabel;
        private System.Windows.Forms.GroupBox ResourceAndMeasurementGroupBox;
        private System.Windows.Forms.Label measurementTypeLabel;
        private System.Windows.Forms.ComboBox measurementModeComboBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.ComboBox resolutionValueComboBox;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.ComboBox powerlineFrequencyValueComboBox;
        private System.Windows.Forms.TextBox rangeTextBox;
        private System.Windows.Forms.GroupBox acConfigurationGroupBox;
        private System.Windows.Forms.Label maxACFrequencyLabel;
        private System.Windows.Forms.Label minACFrequencyLabel;
        private System.Windows.Forms.TextBox maxACFrequencyTextBox;
        private System.Windows.Forms.TextBox minACFrequencyTextBox;
        private System.Windows.Forms.TextBox deviceNameTextBox;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.GroupBox messageGroupBox;

    }
}

