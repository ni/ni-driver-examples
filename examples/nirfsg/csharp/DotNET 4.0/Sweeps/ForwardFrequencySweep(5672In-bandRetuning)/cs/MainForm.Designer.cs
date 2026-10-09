namespace NationalInstruments.Examples.ForwardFrequencySweep5672InbandRetuning
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
            this.frequencySweepGroupBox = new System.Windows.Forms.GroupBox();
            this.startFrequencyLabel = new System.Windows.Forms.Label();
            this.stopFrequencyLabel = new System.Windows.Forms.Label();
            this.numberStepsLabel = new System.Windows.Forms.Label();
            this.dwellTimeLabel = new System.Windows.Forms.Label();
            this.startFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.stopFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.numberStepsNumeric = new System.Windows.Forms.NumericUpDown();
            this.dwellTimeNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.deviceBandwidthToUseLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.actualCurrentFrequencyLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.phaseDetectorFrequencyLabel = new System.Windows.Forms.Label();
            this.deviceBandwidthToUseNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.actualCurrentFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.phaseDetectorFrequencyComboBox = new System.Windows.Forms.ComboBox();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.frequencySweepGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberStepsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwellTimeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceBandwidthToUseNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // frequencySweepGroupBox
            // 
            this.frequencySweepGroupBox.Controls.Add(this.startFrequencyLabel);
            this.frequencySweepGroupBox.Controls.Add(this.stopFrequencyLabel);
            this.frequencySweepGroupBox.Controls.Add(this.numberStepsLabel);
            this.frequencySweepGroupBox.Controls.Add(this.dwellTimeLabel);
            this.frequencySweepGroupBox.Controls.Add(this.startFrequencyNumeric);
            this.frequencySweepGroupBox.Controls.Add(this.stopFrequencyNumeric);
            this.frequencySweepGroupBox.Controls.Add(this.numberStepsNumeric);
            this.frequencySweepGroupBox.Controls.Add(this.dwellTimeNumeric);
            this.frequencySweepGroupBox.Location = new System.Drawing.Point(229, 26);
            this.frequencySweepGroupBox.Name = "frequencySweepGroupBox";
            this.frequencySweepGroupBox.Size = new System.Drawing.Size(176, 235);
            this.frequencySweepGroupBox.TabIndex = 2;
            this.frequencySweepGroupBox.TabStop = false;
            this.frequencySweepGroupBox.Text = "Frequency Sweep Parameters";
            // 
            // startFrequencyLabel
            // 
            this.startFrequencyLabel.AutoSize = true;
            this.startFrequencyLabel.Location = new System.Drawing.Point(28, 24);
            this.startFrequencyLabel.Name = "startFrequencyLabel";
            this.startFrequencyLabel.Size = new System.Drawing.Size(104, 13);
            this.startFrequencyLabel.TabIndex = 3;
            this.startFrequencyLabel.Text = "Start Frequency [Hz]";
            // 
            // stopFrequencyLabel
            // 
            this.stopFrequencyLabel.AutoSize = true;
            this.stopFrequencyLabel.Location = new System.Drawing.Point(29, 72);
            this.stopFrequencyLabel.Name = "stopFrequencyLabel";
            this.stopFrequencyLabel.Size = new System.Drawing.Size(101, 13);
            this.stopFrequencyLabel.TabIndex = 4;
            this.stopFrequencyLabel.Text = "End Frequency [Hz]";
            // 
            // numberStepsLabel
            // 
            this.numberStepsLabel.AutoSize = true;
            this.numberStepsLabel.Location = new System.Drawing.Point(32, 127);
            this.numberStepsLabel.Name = "numberStepsLabel";
            this.numberStepsLabel.Size = new System.Drawing.Size(86, 13);
            this.numberStepsLabel.TabIndex = 5;
            this.numberStepsLabel.Text = "Number of Steps";
            // 
            // dwellTimeLabel
            // 
            this.dwellTimeLabel.AutoSize = true;
            this.dwellTimeLabel.Location = new System.Drawing.Point(25, 184);
            this.dwellTimeLabel.Name = "dwellTimeLabel";
            this.dwellTimeLabel.Size = new System.Drawing.Size(137, 13);
            this.dwellTimeLabel.TabIndex = 6;
            this.dwellTimeLabel.Text = "Dwell Time in Each Step [s]";
            // 
            // startFrequencyNumeric
            // 
            this.startFrequencyNumeric.DecimalPlaces = 6;
            this.startFrequencyNumeric.Location = new System.Drawing.Point(25, 44);
            this.startFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.startFrequencyNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.startFrequencyNumeric.Name = "startFrequencyNumeric";
            this.startFrequencyNumeric.Size = new System.Drawing.Size(120, 20);
            this.startFrequencyNumeric.TabIndex = 0;
            this.startFrequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // stopFrequencyNumeric
            // 
            this.stopFrequencyNumeric.DecimalPlaces = 6;
            this.stopFrequencyNumeric.Location = new System.Drawing.Point(25, 93);
            this.stopFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.stopFrequencyNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.stopFrequencyNumeric.Name = "stopFrequencyNumeric";
            this.stopFrequencyNumeric.Size = new System.Drawing.Size(120, 20);
            this.stopFrequencyNumeric.TabIndex = 1;
            this.stopFrequencyNumeric.Value = new decimal(new int[] {
            1020000000,
            0,
            0,
            0});
            // 
            // numberStepsNumeric
            // 
            this.numberStepsNumeric.Location = new System.Drawing.Point(25, 146);
            this.numberStepsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numberStepsNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberStepsNumeric.Name = "numberStepsNumeric";
            this.numberStepsNumeric.Size = new System.Drawing.Size(120, 20);
            this.numberStepsNumeric.TabIndex = 3;
            this.numberStepsNumeric.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            // 
            // dwellTimeNumeric
            // 
            this.dwellTimeNumeric.DecimalPlaces = 3;
            this.dwellTimeNumeric.Location = new System.Drawing.Point(25, 202);
            this.dwellTimeNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.dwellTimeNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.dwellTimeNumeric.Name = "dwellTimeNumeric";
            this.dwellTimeNumeric.Size = new System.Drawing.Size(120, 20);
            this.dwellTimeNumeric.TabIndex = 3;
            this.dwellTimeNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(19, 16);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // deviceBandwidthToUseLabel
            // 
            this.deviceBandwidthToUseLabel.AutoSize = true;
            this.deviceBandwidthToUseLabel.Location = new System.Drawing.Point(22, 82);
            this.deviceBandwidthToUseLabel.Name = "deviceBandwidthToUseLabel";
            this.deviceBandwidthToUseLabel.Size = new System.Drawing.Size(150, 13);
            this.deviceBandwidthToUseLabel.TabIndex = 1;
            this.deviceBandwidthToUseLabel.Text = "Device Bandwidth to Use (Hz)";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(22, 27);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 2;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // actualCurrentFrequencyLabel
            // 
            this.actualCurrentFrequencyLabel.AutoSize = true;
            this.actualCurrentFrequencyLabel.Location = new System.Drawing.Point(458, 98);
            this.actualCurrentFrequencyLabel.Name = "actualCurrentFrequencyLabel";
            this.actualCurrentFrequencyLabel.Size = new System.Drawing.Size(116, 13);
            this.actualCurrentFrequencyLabel.TabIndex = 7;
            this.actualCurrentFrequencyLabel.Text = "Current Frequency [Hz]";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(19, 283);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 8;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // phaseDetectorFrequencyLabel
            // 
            this.phaseDetectorFrequencyLabel.AutoSize = true;
            this.phaseDetectorFrequencyLabel.Location = new System.Drawing.Point(21, 138);
            this.phaseDetectorFrequencyLabel.Name = "phaseDetectorFrequencyLabel";
            this.phaseDetectorFrequencyLabel.Size = new System.Drawing.Size(134, 13);
            this.phaseDetectorFrequencyLabel.TabIndex = 11;
            this.phaseDetectorFrequencyLabel.Text = "Phase Detector Frequency";
            // 
            // deviceBandwidthToUseNumeric
            // 
            this.deviceBandwidthToUseNumeric.DecimalPlaces = 3;
            this.deviceBandwidthToUseNumeric.Location = new System.Drawing.Point(25, 102);
            this.deviceBandwidthToUseNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.deviceBandwidthToUseNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.deviceBandwidthToUseNumeric.Name = "deviceBandwidthToUseNumeric";
            this.deviceBandwidthToUseNumeric.Size = new System.Drawing.Size(120, 20);
            this.deviceBandwidthToUseNumeric.TabIndex = 1;
            this.deviceBandwidthToUseNumeric.Value = new decimal(new int[] {
            20000000,
            0,
            0,
            0});
            // 
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.DecimalPlaces = 2;
            this.powerLevelNumeric.Location = new System.Drawing.Point(24, 47);
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
            this.powerLevelNumeric.TabIndex = 0;
            this.powerLevelNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(19, 37);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // actualCurrentFrequencyTextBox
            // 
            this.actualCurrentFrequencyTextBox.Location = new System.Drawing.Point(458, 119);
            this.actualCurrentFrequencyTextBox.Name = "actualCurrentFrequencyTextBox";
            this.actualCurrentFrequencyTextBox.ReadOnly = true;
            this.actualCurrentFrequencyTextBox.Size = new System.Drawing.Size(88, 20);
            this.actualCurrentFrequencyTextBox.TabIndex = 11;
            this.actualCurrentFrequencyTextBox.TabStop = false;
            this.actualCurrentFrequencyTextBox.Text = "0.00";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(19, 303);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(577, 47);
            this.errorTextBox.TabIndex = 12;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(440, 32);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(521, 32);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // phaseDetectorFrequencyComboBox
            // 
            this.phaseDetectorFrequencyComboBox.Location = new System.Drawing.Point(25, 158);
            this.phaseDetectorFrequencyComboBox.Name = "phaseDetectorFrequencyComboBox";
            this.phaseDetectorFrequencyComboBox.Size = new System.Drawing.Size(120, 21);
            this.phaseDetectorFrequencyComboBox.TabIndex = 2;
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.phaseDetectorFrequencyComboBox);
            this.configurationGroupBox.Controls.Add(this.powerLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.deviceBandwidthToUseLabel);
            this.configurationGroupBox.Controls.Add(this.deviceBandwidthToUseNumeric);
            this.configurationGroupBox.Controls.Add(this.powerLevelLabel);
            this.configurationGroupBox.Controls.Add(this.phaseDetectorFrequencyLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(19, 71);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(191, 190);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(618, 395);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.frequencySweepGroupBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.actualCurrentFrequencyLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.actualCurrentFrequencyTextBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Forward Frequency Sweep (5672 In-band Retuning)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.frequencySweepGroupBox.ResumeLayout(false);
            this.frequencySweepGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberStepsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwellTimeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceBandwidthToUseNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion

        private System.Windows.Forms.GroupBox frequencySweepGroupBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label deviceBandwidthToUseLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label startFrequencyLabel;
        private System.Windows.Forms.Label stopFrequencyLabel;
        private System.Windows.Forms.Label numberStepsLabel;
        private System.Windows.Forms.Label dwellTimeLabel;
        private System.Windows.Forms.Label actualCurrentFrequencyLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label phaseDetectorFrequencyLabel;
        private System.Windows.Forms.NumericUpDown deviceBandwidthToUseNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown startFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown stopFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown numberStepsNumeric;
        private System.Windows.Forms.NumericUpDown dwellTimeNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox actualCurrentFrequencyTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox phaseDetectorFrequencyComboBox;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.GroupBox configurationGroupBox;

    }
}
