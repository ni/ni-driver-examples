namespace NationalInstruments.Examples.AdvancedPropertyAccess
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
            this.resourceNameAndGenerationTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.generationModeLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.generationModeComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.actualPowerLevelTextBox = new System.Windows.Forms.TextBox();
            this.actualRangeLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.actualFrequencyLabel = new System.Windows.Forms.Label();
            this.actualFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.frequencyReferenceSourceComboBox = new System.Windows.Forms.ComboBox();
            this.frequencyReferenceSourceLabel = new System.Windows.Forms.Label();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.errorGroupBox = new System.Windows.Forms.GroupBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameAndGenerationTypeGroupBox.SuspendLayout();
            this.measurementGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            this.errorGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // resourceNameAndGenerationTypeGroupBox
            // 
            this.resourceNameAndGenerationTypeGroupBox.Controls.Add(this.generationModeLabel);
            this.resourceNameAndGenerationTypeGroupBox.Controls.Add(this.resourceNameLabel);
            this.resourceNameAndGenerationTypeGroupBox.Controls.Add(this.generationModeComboBox);
            this.resourceNameAndGenerationTypeGroupBox.Controls.Add(this.resourceNameComboBox);
            this.resourceNameAndGenerationTypeGroupBox.Location = new System.Drawing.Point(9, 11);
            this.resourceNameAndGenerationTypeGroupBox.Name = "resourceNameAndGenerationTypeGroupBox";
            this.resourceNameAndGenerationTypeGroupBox.Size = new System.Drawing.Size(278, 129);
            this.resourceNameAndGenerationTypeGroupBox.TabIndex = 0;
            this.resourceNameAndGenerationTypeGroupBox.TabStop = false;
            this.resourceNameAndGenerationTypeGroupBox.Text = "Resource Name and Generation Type";
            // 
            // generationModeLabel
            // 
            this.generationModeLabel.AutoSize = true;
            this.generationModeLabel.Location = new System.Drawing.Point(9, 64);
            this.generationModeLabel.Name = "generationModeLabel";
            this.generationModeLabel.Size = new System.Drawing.Size(92, 13);
            this.generationModeLabel.TabIndex = 3;
            this.generationModeLabel.Text = "Generation Mode:";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(9, 31);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 2;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // generationModeComboBox
            // 
            this.generationModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.generationModeComboBox.FormattingEnabled = true;
            this.generationModeComboBox.Location = new System.Drawing.Point(146, 61);
            this.generationModeComboBox.Name = "generationModeComboBox";
            this.generationModeComboBox.Size = new System.Drawing.Size(120, 21);
            this.generationModeComboBox.TabIndex = 1;
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(146, 27);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.stopButton);
            this.measurementGroupBox.Controls.Add(this.actualPowerLevelTextBox);
            this.measurementGroupBox.Controls.Add(this.actualRangeLabel);
            this.measurementGroupBox.Controls.Add(this.startButton);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyLabel);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyTextBox);
            this.measurementGroupBox.Location = new System.Drawing.Point(293, 146);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(301, 131);
            this.measurementGroupBox.TabIndex = 2;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(171, 92);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // actualPowerLevelTextBox
            // 
            this.actualPowerLevelTextBox.Location = new System.Drawing.Point(171, 57);
            this.actualPowerLevelTextBox.Name = "actualPowerLevelTextBox";
            this.actualPowerLevelTextBox.ReadOnly = true;
            this.actualPowerLevelTextBox.Size = new System.Drawing.Size(120, 20);
            this.actualPowerLevelTextBox.TabIndex = 1;
            this.actualPowerLevelTextBox.TabStop = false;
            // 
            // actualRangeLabel
            // 
            this.actualRangeLabel.AutoSize = true;
            this.actualRangeLabel.Location = new System.Drawing.Point(5, 63);
            this.actualRangeLabel.Name = "actualRangeLabel";
            this.actualRangeLabel.Size = new System.Drawing.Size(132, 13);
            this.actualRangeLabel.TabIndex = 3;
            this.actualRangeLabel.Text = "Actual Power Level [dBm]:";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(90, 92);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // actualFrequencyLabel
            // 
            this.actualFrequencyLabel.AutoSize = true;
            this.actualFrequencyLabel.Location = new System.Drawing.Point(5, 28);
            this.actualFrequencyLabel.Name = "actualFrequencyLabel";
            this.actualFrequencyLabel.Size = new System.Drawing.Size(115, 13);
            this.actualFrequencyLabel.TabIndex = 1;
            this.actualFrequencyLabel.Text = "Actual Frequency [Hz]:";
            // 
            // actualFrequencyTextBox
            // 
            this.actualFrequencyTextBox.Location = new System.Drawing.Point(171, 28);
            this.actualFrequencyTextBox.Name = "actualFrequencyTextBox";
            this.actualFrequencyTextBox.ReadOnly = true;
            this.actualFrequencyTextBox.Size = new System.Drawing.Size(120, 20);
            this.actualFrequencyTextBox.TabIndex = 0;
            this.actualFrequencyTextBox.TabStop = false;
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.frequencyReferenceSourceComboBox);
            this.configurationGroupBox.Controls.Add(this.frequencyReferenceSourceLabel);
            this.configurationGroupBox.Controls.Add(this.frequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.powerLevelLabel);
            this.configurationGroupBox.Controls.Add(this.frequencyLabel);
            this.configurationGroupBox.Controls.Add(this.powerLevelNumeric);
            this.configurationGroupBox.Location = new System.Drawing.Point(293, 11);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(301, 129);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // frequencyReferenceSourceComboBox
            // 
            this.frequencyReferenceSourceComboBox.FormattingEnabled = true;
            this.frequencyReferenceSourceComboBox.Location = new System.Drawing.Point(171, 96);
            this.frequencyReferenceSourceComboBox.Name = "frequencyReferenceSourceComboBox";
            this.frequencyReferenceSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.frequencyReferenceSourceComboBox.TabIndex = 6;
            // 
            // frequencyReferenceSourceLabel
            // 
            this.frequencyReferenceSourceLabel.AutoSize = true;
            this.frequencyReferenceSourceLabel.Location = new System.Drawing.Point(6, 99);
            this.frequencyReferenceSourceLabel.Name = "frequencyReferenceSourceLabel";
            this.frequencyReferenceSourceLabel.Size = new System.Drawing.Size(150, 13);
            this.frequencyReferenceSourceLabel.TabIndex = 5;
            this.frequencyReferenceSourceLabel.Text = "Frequency Reference Source:";
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.Location = new System.Drawing.Point(171, 29);
            this.frequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.frequencyNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.frequencyNumeric.Name = "frequencyNumeric";
            this.frequencyNumeric.Size = new System.Drawing.Size(120, 20);
            this.frequencyNumeric.TabIndex = 0;
            this.frequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(5, 64);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(99, 13);
            this.powerLevelLabel.TabIndex = 3;
            this.powerLevelLabel.Text = "Power Level [dBm]:";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.AutoSize = true;
            this.frequencyLabel.Location = new System.Drawing.Point(5, 31);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(116, 13);
            this.frequencyLabel.TabIndex = 2;
            this.frequencyLabel.Text = "Center Frequency [Hz]:";
            // 
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.Location = new System.Drawing.Point(171, 62);
            this.powerLevelNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.powerLevelNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.powerLevelNumeric.Name = "powerLevelNumeric";
            this.powerLevelNumeric.Size = new System.Drawing.Size(120, 20);
            this.powerLevelNumeric.TabIndex = 1;
            this.powerLevelNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // errorGroupBox
            // 
            this.errorGroupBox.Controls.Add(this.errorTextBox);
            this.errorGroupBox.Location = new System.Drawing.Point(9, 146);
            this.errorGroupBox.Name = "errorGroupBox";
            this.errorGroupBox.Size = new System.Drawing.Size(278, 131);
            this.errorGroupBox.TabIndex = 3;
            this.errorGroupBox.TabStop = false;
            this.errorGroupBox.Text = "Error Message";
            // 
            // errorTextBox
            // 
            this.errorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.errorTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorTextBox.Location = new System.Drawing.Point(3, 16);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.errorTextBox.Size = new System.Drawing.Size(272, 112);
            this.errorTextBox.TabIndex = 0;
            this.errorTextBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(604, 284);
            this.Controls.Add(this.errorGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.resourceNameAndGenerationTypeGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Advanced Property Access";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.resourceNameAndGenerationTypeGroupBox.ResumeLayout(false);
            this.resourceNameAndGenerationTypeGroupBox.PerformLayout();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            this.errorGroupBox.ResumeLayout(false);
            this.errorGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox resourceNameAndGenerationTypeGroupBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox errorGroupBox;
        private System.Windows.Forms.ComboBox generationModeComboBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox actualFrequencyTextBox;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Label generationModeLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label actualFrequencyLabel;
        private System.Windows.Forms.TextBox actualPowerLevelTextBox;
        private System.Windows.Forms.Label actualRangeLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.Label frequencyReferenceSourceLabel;
        private System.Windows.Forms.ComboBox frequencyReferenceSourceComboBox;
    }
}
