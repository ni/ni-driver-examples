namespace NationalInstruments.Examples.ExportImportConfiguration
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
         this.ResourceAndMeasurementGroupBox = new System.Windows.Forms.GroupBox();
         this.measurementTypeLabel = new System.Windows.Forms.Label();
         this.measurementModeComboBox = new System.Windows.Forms.ComboBox();
         this.resourceNameLabel = new System.Windows.Forms.Label();
         this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
         this.configurationGroupBox = new System.Windows.Forms.GroupBox();
         this.importButton = new System.Windows.Forms.Button();
         this.exportButton = new System.Windows.Forms.Button();
         this.rangeNumericUpDown = new System.Windows.Forms.NumericUpDown();
         this.powerlineFrequencyLabel = new System.Windows.Forms.Label();
         this.resolutionLabel = new System.Windows.Forms.Label();
         this.rangeLabel = new System.Windows.Forms.Label();
         this.powerlineFrequencyValueComboBox = new System.Windows.Forms.ComboBox();
         this.resolutionValueComboBox = new System.Windows.Forms.ComboBox();
         this.messageGroupBox = new System.Windows.Forms.GroupBox();
         this.messageTextBox = new System.Windows.Forms.TextBox();
         this.measurementGroupBox = new System.Windows.Forms.GroupBox();
         this.actualRangeLabel = new System.Windows.Forms.Label();
         this.actualRangTextBox = new System.Windows.Forms.TextBox();
         this.readButton = new System.Windows.Forms.Button();
         this.measurementLabel = new System.Windows.Forms.Label();
         this.measurementTextBox = new System.Windows.Forms.TextBox();
         this.ResourceAndMeasurementGroupBox.SuspendLayout();
         this.configurationGroupBox.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.rangeNumericUpDown)).BeginInit();
         this.messageGroupBox.SuspendLayout();
         this.measurementGroupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // ResourceAndMeasurementGroupBox
         // 
         this.ResourceAndMeasurementGroupBox.Controls.Add(this.measurementTypeLabel);
         this.ResourceAndMeasurementGroupBox.Controls.Add(this.measurementModeComboBox);
         this.ResourceAndMeasurementGroupBox.Controls.Add(this.resourceNameLabel);
         this.ResourceAndMeasurementGroupBox.Controls.Add(this.resourceNameComboBox);
         this.ResourceAndMeasurementGroupBox.Location = new System.Drawing.Point(7, 12);
         this.ResourceAndMeasurementGroupBox.Name = "ResourceAndMeasurementGroupBox";
         this.ResourceAndMeasurementGroupBox.Size = new System.Drawing.Size(269, 93);
         this.ResourceAndMeasurementGroupBox.TabIndex = 0;
         this.ResourceAndMeasurementGroupBox.TabStop = false;
         this.ResourceAndMeasurementGroupBox.Text = "Resource Name and Measurement Type";
         // 
         // measurementTypeLabel
         // 
         this.measurementTypeLabel.AutoSize = true;
         this.measurementTypeLabel.Location = new System.Drawing.Point(6, 64);
         this.measurementTypeLabel.Name = "measurementTypeLabel";
         this.measurementTypeLabel.Size = new System.Drawing.Size(104, 13);
         this.measurementTypeLabel.TabIndex = 3;
         this.measurementTypeLabel.Text = "Measurement Mode:";
         // 
         // measurementModeComboBox
         // 
         this.measurementModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.measurementModeComboBox.FormattingEnabled = true;
         this.measurementModeComboBox.Location = new System.Drawing.Point(142, 61);
         this.measurementModeComboBox.Name = "measurementModeComboBox";
         this.measurementModeComboBox.Size = new System.Drawing.Size(121, 21);
         this.measurementModeComboBox.TabIndex = 1;
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
         this.resourceNameComboBox.Location = new System.Drawing.Point(142, 24);
         this.resourceNameComboBox.Name = "resourceNameComboBox";
         this.resourceNameComboBox.Size = new System.Drawing.Size(121, 21);
         this.resourceNameComboBox.TabIndex = 0;
         // 
         // configurationGroupBox
         // 
         this.configurationGroupBox.Controls.Add(this.importButton);
         this.configurationGroupBox.Controls.Add(this.exportButton);
         this.configurationGroupBox.Controls.Add(this.rangeNumericUpDown);
         this.configurationGroupBox.Controls.Add(this.powerlineFrequencyLabel);
         this.configurationGroupBox.Controls.Add(this.resolutionLabel);
         this.configurationGroupBox.Controls.Add(this.rangeLabel);
         this.configurationGroupBox.Controls.Add(this.powerlineFrequencyValueComboBox);
         this.configurationGroupBox.Controls.Add(this.resolutionValueComboBox);
         this.configurationGroupBox.Location = new System.Drawing.Point(282, 12);
         this.configurationGroupBox.Name = "configurationGroupBox";
         this.configurationGroupBox.Size = new System.Drawing.Size(269, 159);
         this.configurationGroupBox.TabIndex = 1;
         this.configurationGroupBox.TabStop = false;
         this.configurationGroupBox.Text = "Configuration";
         // 
         // importButton
         // 
         this.importButton.Location = new System.Drawing.Point(163, 125);
         this.importButton.Name = "importButton";
         this.importButton.Size = new System.Drawing.Size(86, 23);
         this.importButton.TabIndex = 9;
         this.importButton.Text = "Import...";
         this.importButton.UseVisualStyleBackColor = true;
         this.importButton.Click += new System.EventHandler(this.Import_Click);
         // 
         // exportButton
         // 
         this.exportButton.Location = new System.Drawing.Point(63, 125);
         this.exportButton.Name = "exportButton";
         this.exportButton.Size = new System.Drawing.Size(86, 23);
         this.exportButton.TabIndex = 8;
         this.exportButton.Text = "Export...";
         this.exportButton.UseVisualStyleBackColor = true;
         this.exportButton.Click += new System.EventHandler(this.Export_Click);
         // 
         // rangeNumericUpDown
         // 
         this.rangeNumericUpDown.DecimalPlaces = 3;
         this.rangeNumericUpDown.Location = new System.Drawing.Point(142, 18);
         this.rangeNumericUpDown.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
         this.rangeNumericUpDown.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            -2147483648});
         this.rangeNumericUpDown.Name = "rangeNumericUpDown";
         this.rangeNumericUpDown.Size = new System.Drawing.Size(106, 20);
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
         this.powerlineFrequencyLabel.Location = new System.Drawing.Point(6, 93);
         this.powerlineFrequencyLabel.Name = "powerlineFrequencyLabel";
         this.powerlineFrequencyLabel.Size = new System.Drawing.Size(131, 13);
         this.powerlineFrequencyLabel.TabIndex = 7;
         this.powerlineFrequencyLabel.Text = "Powerline Frequency (Hz):";
         // 
         // resolutionLabel
         // 
         this.resolutionLabel.AutoSize = true;
         this.resolutionLabel.Location = new System.Drawing.Point(6, 57);
         this.resolutionLabel.Name = "resolutionLabel";
         this.resolutionLabel.Size = new System.Drawing.Size(95, 13);
         this.resolutionLabel.TabIndex = 6;
         this.resolutionLabel.Text = "Resolution (Digits):";
         // 
         // rangeLabel
         // 
         this.rangeLabel.AutoSize = true;
         this.rangeLabel.Location = new System.Drawing.Point(6, 21);
         this.rangeLabel.Name = "rangeLabel";
         this.rangeLabel.Size = new System.Drawing.Size(42, 13);
         this.rangeLabel.TabIndex = 5;
         this.rangeLabel.Text = "Range:";
         // 
         // powerlineFrequencyValueComboBox
         // 
         this.powerlineFrequencyValueComboBox.FormattingEnabled = true;
         this.powerlineFrequencyValueComboBox.Location = new System.Drawing.Point(142, 90);
         this.powerlineFrequencyValueComboBox.Name = "powerlineFrequencyValueComboBox";
         this.powerlineFrequencyValueComboBox.Size = new System.Drawing.Size(107, 21);
         this.powerlineFrequencyValueComboBox.TabIndex = 2;
         // 
         // resolutionValueComboBox
         // 
         this.resolutionValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.resolutionValueComboBox.FormattingEnabled = true;
         this.resolutionValueComboBox.Location = new System.Drawing.Point(142, 54);
         this.resolutionValueComboBox.Name = "resolutionValueComboBox";
         this.resolutionValueComboBox.Size = new System.Drawing.Size(107, 21);
         this.resolutionValueComboBox.TabIndex = 1;
         // 
         // messageGroupBox
         // 
         this.messageGroupBox.Controls.Add(this.messageTextBox);
         this.messageGroupBox.Location = new System.Drawing.Point(7, 111);
         this.messageGroupBox.Name = "messageGroupBox";
         this.messageGroupBox.Size = new System.Drawing.Size(269, 169);
         this.messageGroupBox.TabIndex = 3;
         this.messageGroupBox.TabStop = false;
         this.messageGroupBox.Text = "Message";
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
         this.messageTextBox.Size = new System.Drawing.Size(263, 150);
         this.messageTextBox.TabIndex = 0;
         // 
         // measurementGroupBox
         // 
         this.measurementGroupBox.Controls.Add(this.actualRangeLabel);
         this.measurementGroupBox.Controls.Add(this.actualRangTextBox);
         this.measurementGroupBox.Controls.Add(this.readButton);
         this.measurementGroupBox.Controls.Add(this.measurementLabel);
         this.measurementGroupBox.Controls.Add(this.measurementTextBox);
         this.measurementGroupBox.Location = new System.Drawing.Point(282, 177);
         this.measurementGroupBox.Name = "measurementGroupBox";
         this.measurementGroupBox.Size = new System.Drawing.Size(269, 103);
         this.measurementGroupBox.TabIndex = 2;
         this.measurementGroupBox.TabStop = false;
         this.measurementGroupBox.Text = "Measurement";
         // 
         // actualRangeLabel
         // 
         this.actualRangeLabel.AutoSize = true;
         this.actualRangeLabel.Location = new System.Drawing.Point(7, 54);
         this.actualRangeLabel.Name = "actualRangeLabel";
         this.actualRangeLabel.Size = new System.Drawing.Size(75, 13);
         this.actualRangeLabel.TabIndex = 6;
         this.actualRangeLabel.Text = "Actual Range:";
         // 
         // actualRangTextBox
         // 
         this.actualRangTextBox.Location = new System.Drawing.Point(143, 51);
         this.actualRangTextBox.Name = "actualRangTextBox";
         this.actualRangTextBox.ReadOnly = true;
         this.actualRangTextBox.Size = new System.Drawing.Size(107, 20);
         this.actualRangTextBox.TabIndex = 1;
         // 
         // readButton
         // 
         this.readButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.readButton.Location = new System.Drawing.Point(172, 78);
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
         this.measurementLabel.Location = new System.Drawing.Point(7, 22);
         this.measurementLabel.Name = "measurementLabel";
         this.measurementLabel.Size = new System.Drawing.Size(74, 13);
         this.measurementLabel.TabIndex = 1;
         this.measurementLabel.Text = "Measurement:";
         // 
         // measurementTextBox
         // 
         this.measurementTextBox.Location = new System.Drawing.Point(143, 20);
         this.measurementTextBox.Name = "measurementTextBox";
         this.measurementTextBox.ReadOnly = true;
         this.measurementTextBox.Size = new System.Drawing.Size(107, 20);
         this.measurementTextBox.TabIndex = 0;
         // 
         // MainForm
         // 
         this.AcceptButton = this.readButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoSize = true;
         this.ClientSize = new System.Drawing.Size(557, 287);
         this.Controls.Add(this.measurementGroupBox);
         this.Controls.Add(this.configurationGroupBox);
         this.Controls.Add(this.messageGroupBox);
         this.Controls.Add(this.ResourceAndMeasurementGroupBox);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.Name = "MainForm";
         this.Text = "Export Import Configuration";
         this.ResourceAndMeasurementGroupBox.ResumeLayout(false);
         this.ResourceAndMeasurementGroupBox.PerformLayout();
         this.configurationGroupBox.ResumeLayout(false);
         this.configurationGroupBox.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.rangeNumericUpDown)).EndInit();
         this.messageGroupBox.ResumeLayout(false);
         this.messageGroupBox.PerformLayout();
         this.measurementGroupBox.ResumeLayout(false);
         this.measurementGroupBox.PerformLayout();
         this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ResourceAndMeasurementGroupBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.ComboBox measurementModeComboBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label measurementTypeLabel;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.ComboBox powerlineFrequencyValueComboBox;
        private System.Windows.Forms.ComboBox resolutionValueComboBox;
        private System.Windows.Forms.GroupBox messageGroupBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;
        private System.Windows.Forms.Label measurementLabel;
        private System.Windows.Forms.TextBox measurementTextBox;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.Label powerlineFrequencyLabel;
        private System.Windows.Forms.Label resolutionLabel;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.NumericUpDown rangeNumericUpDown;
        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.Label actualRangeLabel;
        private System.Windows.Forms.TextBox actualRangTextBox;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Button importButton;
    }
}

