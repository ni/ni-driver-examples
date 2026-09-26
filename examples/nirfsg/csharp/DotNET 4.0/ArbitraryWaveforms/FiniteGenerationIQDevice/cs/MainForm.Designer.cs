namespace NationalInstruments.Examples.FiniteGenerationIQDevice
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.waveformDurationTextBox = new System.Windows.Forms.TextBox();
            this.waveformDurationLabel = new System.Windows.Forms.Label();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.referenceClockComboBox = new System.Windows.Forms.ComboBox();
            this.iqOutPortLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.referenceClockLabel = new System.Windows.Forms.Label();
            this.waveformRepeatCountNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqPortFrequencyLabel = new System.Windows.Forms.Label();
            this.iqOutPortLevelLabel = new System.Windows.Forms.Label();
            this.iqPortFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.measurementGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iqOutPortLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.waveformRepeatCountNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqPortFrequencyNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.waveformDurationTextBox);
            this.measurementGroupBox.Controls.Add(this.waveformDurationLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(340, 67);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(188, 95);
            this.measurementGroupBox.TabIndex = 15;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // waveformDurationTextBox
            // 
            this.waveformDurationTextBox.Location = new System.Drawing.Point(22, 49);
            this.waveformDurationTextBox.Name = "waveformDurationTextBox";
            this.waveformDurationTextBox.ReadOnly = true;
            this.waveformDurationTextBox.Size = new System.Drawing.Size(109, 20);
            this.waveformDurationTextBox.TabIndex = 9;
            this.waveformDurationTextBox.TabStop = false;
            this.waveformDurationTextBox.Text = "0.001000";
            // 
            // waveformDurationLabel
            // 
            this.waveformDurationLabel.AutoSize = true;
            this.waveformDurationLabel.Location = new System.Drawing.Point(22, 28);
            this.waveformDurationLabel.Name = "waveformDurationLabel";
            this.waveformDurationLabel.Size = new System.Drawing.Size(113, 13);
            this.waveformDurationLabel.TabIndex = 5;
            this.waveformDurationLabel.Text = "Waveform Duration [s]";
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.referenceClockComboBox);
            this.configurationGroupBox.Controls.Add(this.iqOutPortLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.referenceClockLabel);
            this.configurationGroupBox.Controls.Add(this.waveformRepeatCountNumeric);
            this.configurationGroupBox.Controls.Add(this.iqPortFrequencyLabel);
            this.configurationGroupBox.Controls.Add(this.iqOutPortLevelLabel);
            this.configurationGroupBox.Controls.Add(this.iqPortFrequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.iqRateLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 67);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(316, 153);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // referenceClockComboBox
            // 
            this.referenceClockComboBox.Location = new System.Drawing.Point(22, 49);
            this.referenceClockComboBox.Name = "referenceClockComboBox";
            this.referenceClockComboBox.Size = new System.Drawing.Size(110, 21);
            this.referenceClockComboBox.TabIndex = 1;
            // 
            // iqOutPortLevelNumeric
            // 
            this.iqOutPortLevelNumeric.DecimalPlaces = 3;
            this.iqOutPortLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.iqOutPortLevelNumeric.Location = new System.Drawing.Point(177, 103);
            this.iqOutPortLevelNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.iqOutPortLevelNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.iqOutPortLevelNumeric.Name = "iqOutPortLevelNumeric";
            this.iqOutPortLevelNumeric.Size = new System.Drawing.Size(120, 20);
            this.iqOutPortLevelNumeric.TabIndex = 4;
            this.iqOutPortLevelNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // referenceClockLabel
            // 
            this.referenceClockLabel.AutoSize = true;
            this.referenceClockLabel.Location = new System.Drawing.Point(22, 28);
            this.referenceClockLabel.Name = "referenceClockLabel";
            this.referenceClockLabel.Size = new System.Drawing.Size(71, 13);
            this.referenceClockLabel.TabIndex = 28;
            this.referenceClockLabel.Text = "Clock Source";
            // 
            // waveformRepeatCountNumeric
            // 
            this.waveformRepeatCountNumeric.Location = new System.Drawing.Point(22, 103);
            this.waveformRepeatCountNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.waveformRepeatCountNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.waveformRepeatCountNumeric.Name = "waveformRepeatCountNumeric";
            this.waveformRepeatCountNumeric.Size = new System.Drawing.Size(120, 20);
            this.waveformRepeatCountNumeric.TabIndex = 2;
            this.waveformRepeatCountNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // iqPortFrequencyLabel
            // 
            this.iqPortFrequencyLabel.AutoSize = true;
            this.iqPortFrequencyLabel.Location = new System.Drawing.Point(177, 28);
            this.iqPortFrequencyLabel.Name = "iqPortFrequencyLabel";
            this.iqPortFrequencyLabel.Size = new System.Drawing.Size(115, 13);
            this.iqPortFrequencyLabel.TabIndex = 1;
            this.iqPortFrequencyLabel.Text = "IQ Port Frequency (Hz)";
            // 
            // iqOutPortLevelLabel
            // 
            this.iqOutPortLevelLabel.AutoSize = true;
            this.iqOutPortLevelLabel.Location = new System.Drawing.Point(177, 82);
            this.iqOutPortLevelLabel.Name = "iqOutPortLevelLabel";
            this.iqOutPortLevelLabel.Size = new System.Drawing.Size(117, 13);
            this.iqOutPortLevelLabel.TabIndex = 2;
            this.iqOutPortLevelLabel.Text = "IQ Out Port Level (Vpp)";
            // 
            // iqPortFrequencyNumeric
            // 
            this.iqPortFrequencyNumeric.DecimalPlaces = 6;
            this.iqPortFrequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.iqPortFrequencyNumeric.Location = new System.Drawing.Point(177, 49);
            this.iqPortFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.iqPortFrequencyNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.iqPortFrequencyNumeric.Name = "iqPortFrequencyNumeric";
            this.iqPortFrequencyNumeric.Size = new System.Drawing.Size(120, 20);
            this.iqPortFrequencyNumeric.TabIndex = 3;
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(22, 82);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(125, 13);
            this.iqRateLabel.TabIndex = 4;
            this.iqRateLabel.Text = "Waveform Repeat Count";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(12, 9);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 13;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(12, 238);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 19;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 30);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(12, 268);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(516, 34);
            this.errorTextBox.TabIndex = 20;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(361, 28);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(453, 28);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Interval = 1;
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 335);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Finite Generation IQ Device";
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iqOutPortLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.waveformRepeatCountNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqPortFrequencyNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox measurementGroupBox;
        private System.Windows.Forms.TextBox waveformDurationTextBox;
        private System.Windows.Forms.Label waveformDurationLabel;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.NumericUpDown iqOutPortLevelNumeric;
        private System.Windows.Forms.NumericUpDown waveformRepeatCountNumeric;
        private System.Windows.Forms.Label iqPortFrequencyLabel;
        private System.Windows.Forms.Label iqOutPortLevelLabel;
        private System.Windows.Forms.NumericUpDown iqPortFrequencyNumeric;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.ComboBox referenceClockComboBox;
        private System.Windows.Forms.Label referenceClockLabel;
    }
}

