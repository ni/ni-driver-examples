namespace NationalInstruments.Examples.ExternalAWG5610
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
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.gainLabel = new System.Windows.Forms.Label();
            this.bandwidthLabel = new System.Windows.Forms.Label();
            this.arbCarrierLabel = new System.Windows.Forms.Label();
            this.actualFrequencyLabel = new System.Windows.Forms.Label();
            this.actualGainLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.gainNumeric = new System.Windows.Forms.NumericUpDown();
            this.bandwidthNumeric = new System.Windows.Forms.NumericUpDown();
            this.arbCarrierNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.actualFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.actualGainTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gainNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandwidthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arbCarrierNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            this.measurementGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(28, 16);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.AutoSize = true;
            this.frequencyLabel.Location = new System.Drawing.Point(21, 22);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.frequencyLabel.TabIndex = 1;
            this.frequencyLabel.Text = "Center Frequency [Hz]";
            // 
            // gainLabel
            // 
            this.gainLabel.AutoSize = true;
            this.gainLabel.Location = new System.Drawing.Point(21, 70);
            this.gainLabel.Name = "gainLabel";
            this.gainLabel.Size = new System.Drawing.Size(51, 13);
            this.gainLabel.TabIndex = 2;
            this.gainLabel.Text = "Gain [dB]";
            // 
            // bandwidthLabel
            // 
            this.bandwidthLabel.AutoSize = true;
            this.bandwidthLabel.Location = new System.Drawing.Point(21, 119);
            this.bandwidthLabel.Name = "bandwidthLabel";
            this.bandwidthLabel.Size = new System.Drawing.Size(79, 13);
            this.bandwidthLabel.TabIndex = 3;
            this.bandwidthLabel.Text = "Bandwidth [Hz]";
            // 
            // arbCarrierLabel
            // 
            this.arbCarrierLabel.AutoSize = true;
            this.arbCarrierLabel.Location = new System.Drawing.Point(21, 172);
            this.arbCarrierLabel.Name = "arbCarrierLabel";
            this.arbCarrierLabel.Size = new System.Drawing.Size(131, 13);
            this.arbCarrierLabel.TabIndex = 4;
            this.arbCarrierLabel.Text = "Arb Carrier Frequency [Hz]";
            // 
            // actualFrequencyLabel
            // 
            this.actualFrequencyLabel.AutoSize = true;
            this.actualFrequencyLabel.Location = new System.Drawing.Point(19, 25);
            this.actualFrequencyLabel.Name = "actualFrequencyLabel";
            this.actualFrequencyLabel.Size = new System.Drawing.Size(146, 13);
            this.actualFrequencyLabel.TabIndex = 5;
            this.actualFrequencyLabel.Text = "Actual Center Frequency [Hz]";
            // 
            // actualGainLabel
            // 
            this.actualGainLabel.AutoSize = true;
            this.actualGainLabel.Location = new System.Drawing.Point(19, 73);
            this.actualGainLabel.Name = "actualGainLabel";
            this.actualGainLabel.Size = new System.Drawing.Size(84, 13);
            this.actualGainLabel.TabIndex = 6;
            this.actualGainLabel.Text = "Actual Gain [dB]";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(28, 334);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 7;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.DecimalPlaces = 6;
            this.frequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.frequencyNumeric.Location = new System.Drawing.Point(21, 43);
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
            this.frequencyNumeric.TabIndex = 1;
            this.frequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // gainNumeric
            // 
            this.gainNumeric.DecimalPlaces = 2;
            this.gainNumeric.Location = new System.Drawing.Point(21, 91);
            this.gainNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.gainNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.gainNumeric.Name = "gainNumeric";
            this.gainNumeric.Size = new System.Drawing.Size(120, 20);
            this.gainNumeric.TabIndex = 2;
            this.gainNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // bandwidthNumeric
            // 
            this.bandwidthNumeric.DecimalPlaces = 6;
            this.bandwidthNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.bandwidthNumeric.Location = new System.Drawing.Point(21, 140);
            this.bandwidthNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.bandwidthNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.bandwidthNumeric.Name = "bandwidthNumeric";
            this.bandwidthNumeric.Size = new System.Drawing.Size(120, 20);
            this.bandwidthNumeric.TabIndex = 3;
            this.bandwidthNumeric.Value = new decimal(new int[] {
            5000000,
            0,
            0,
            0});
            // 
            // arbCarrierNumeric
            // 
            this.arbCarrierNumeric.DecimalPlaces = 6;
            this.arbCarrierNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.arbCarrierNumeric.Location = new System.Drawing.Point(21, 193);
            this.arbCarrierNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.arbCarrierNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.arbCarrierNumeric.Name = "arbCarrierNumeric";
            this.arbCarrierNumeric.Size = new System.Drawing.Size(120, 20);
            this.arbCarrierNumeric.TabIndex = 4;
            this.arbCarrierNumeric.Value = new decimal(new int[] {
            25000000,
            0,
            0,
            0});
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(28, 37);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // actualFrequencyTextBox
            // 
            this.actualFrequencyTextBox.Location = new System.Drawing.Point(19, 43);
            this.actualFrequencyTextBox.Name = "actualFrequencyTextBox";
            this.actualFrequencyTextBox.ReadOnly = true;
            this.actualFrequencyTextBox.Size = new System.Drawing.Size(109, 20);
            this.actualFrequencyTextBox.TabIndex = 8;
            this.actualFrequencyTextBox.TabStop = false;
            this.actualFrequencyTextBox.Text = "0.000000";
            // 
            // actualGainTextBox
            // 
            this.actualGainTextBox.Location = new System.Drawing.Point(19, 91);
            this.actualGainTextBox.Name = "actualGainTextBox";
            this.actualGainTextBox.ReadOnly = true;
            this.actualGainTextBox.Size = new System.Drawing.Size(109, 20);
            this.actualGainTextBox.TabIndex = 9;
            this.actualGainTextBox.TabStop = false;
            this.actualGainTextBox.Text = "0.00";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(31, 352);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(365, 34);
            this.errorTextBox.TabIndex = 10;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(321, 37);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.frequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.arbCarrierNumeric);
            this.configurationGroupBox.Controls.Add(this.bandwidthNumeric);
            this.configurationGroupBox.Controls.Add(this.frequencyLabel);
            this.configurationGroupBox.Controls.Add(this.gainNumeric);
            this.configurationGroupBox.Controls.Add(this.gainLabel);
            this.configurationGroupBox.Controls.Add(this.arbCarrierLabel);
            this.configurationGroupBox.Controls.Add(this.bandwidthLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(28, 81);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(165, 229);
            this.configurationGroupBox.TabIndex = 2;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualGainTextBox);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyTextBox);
            this.measurementGroupBox.Controls.Add(this.actualGainLabel);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(214, 81);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(182, 121);
            this.measurementGroupBox.TabIndex = 0;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(425, 424);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "External AWG (5610)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gainNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandwidthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arbCarrierNumeric)).EndInit();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.Label gainLabel;
        private System.Windows.Forms.Label bandwidthLabel;
        private System.Windows.Forms.Label arbCarrierLabel;
        private System.Windows.Forms.Label actualFrequencyLabel;
        private System.Windows.Forms.Label actualGainLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.NumericUpDown gainNumeric;
        private System.Windows.Forms.NumericUpDown bandwidthNumeric;
        private System.Windows.Forms.NumericUpDown arbCarrierNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox actualFrequencyTextBox;
        private System.Windows.Forms.TextBox actualGainTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Button startButton;        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;

    }
}
