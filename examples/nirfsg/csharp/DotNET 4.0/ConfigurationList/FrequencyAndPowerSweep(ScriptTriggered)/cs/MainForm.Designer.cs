using System;
namespace NationalInstruments.Examples.FrequencyAndPowerSweepScriptTriggered
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
            this.configurationListGroupBox = new System.Windows.Forms.GroupBox();
            this.startFrequencyLabel = new System.Windows.Forms.Label();
            this.startPowerLabel = new System.Windows.Forms.Label();
            this.stopPowerLabel = new System.Windows.Forms.Label();
            this.stopFrequencyLabel = new System.Windows.Forms.Label();
            this.numberStepsLabel = new System.Windows.Forms.Label();
            this.startFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.startPowerNumeric = new System.Windows.Forms.NumericUpDown();
            this.stopPowerNumeric = new System.Windows.Forms.NumericUpDown();
            this.stopFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.numberStepsNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.actualContinuousWaveformToneDurationLabel = new System.Windows.Forms.Label();
            this.actualIdleWaveformDurationLabel = new System.Windows.Forms.Label();
            this.actualIQRateLabel = new System.Windows.Forms.Label();
            this.clockSourceLabel = new System.Windows.Forms.Label();
            this.loopBandwidthLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.scriptLabel = new System.Windows.Forms.Label();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.actualContinuousWaveformToneDurationTextBox = new System.Windows.Forms.TextBox();
            this.actualIdleWaveformDurationTextBox = new System.Windows.Forms.TextBox();
            this.actualIQRateTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.scriptTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.clockSourceComboBox = new System.Windows.Forms.ComboBox();
            this.loopBandwidthComboBox = new System.Windows.Forms.ComboBox();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.configurationListGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startPowerNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopPowerNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberStepsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            this.measurementGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // configurationListGroupBox
            // 
            this.configurationListGroupBox.Controls.Add(this.startFrequencyLabel);
            this.configurationListGroupBox.Controls.Add(this.startPowerLabel);
            this.configurationListGroupBox.Controls.Add(this.stopPowerLabel);
            this.configurationListGroupBox.Controls.Add(this.stopFrequencyLabel);
            this.configurationListGroupBox.Controls.Add(this.numberStepsLabel);
            this.configurationListGroupBox.Controls.Add(this.startFrequencyNumeric);
            this.configurationListGroupBox.Controls.Add(this.startPowerNumeric);
            this.configurationListGroupBox.Controls.Add(this.stopPowerNumeric);
            this.configurationListGroupBox.Controls.Add(this.stopFrequencyNumeric);
            this.configurationListGroupBox.Controls.Add(this.numberStepsNumeric);
            this.configurationListGroupBox.Location = new System.Drawing.Point(434, 93);
            this.configurationListGroupBox.Name = "configurationListGroupBox";
            this.configurationListGroupBox.Size = new System.Drawing.Size(177, 335);
            this.configurationListGroupBox.TabIndex = 3;
            this.configurationListGroupBox.TabStop = false;
            this.configurationListGroupBox.Text = "Configuration List Parameters";
            // 
            // startFrequencyLabel
            // 
            this.startFrequencyLabel.AutoSize = true;
            this.startFrequencyLabel.Location = new System.Drawing.Point(28, 28);
            this.startFrequencyLabel.Name = "startFrequencyLabel";
            this.startFrequencyLabel.Size = new System.Drawing.Size(104, 13);
            this.startFrequencyLabel.TabIndex = 1;
            this.startFrequencyLabel.Text = "Start Frequency [Hz]";
            // 
            // startPowerLabel
            // 
            this.startPowerLabel.AutoSize = true;
            this.startPowerLabel.Location = new System.Drawing.Point(25, 159);
            this.startPowerLabel.Name = "startPowerLabel";
            this.startPowerLabel.Size = new System.Drawing.Size(92, 13);
            this.startPowerLabel.TabIndex = 2;
            this.startPowerLabel.Text = "Start Power [dBm]";
            // 
            // stopPowerLabel
            // 
            this.stopPowerLabel.AutoSize = true;
            this.stopPowerLabel.Location = new System.Drawing.Point(25, 210);
            this.stopPowerLabel.Name = "stopPowerLabel";
            this.stopPowerLabel.Size = new System.Drawing.Size(92, 13);
            this.stopPowerLabel.TabIndex = 3;
            this.stopPowerLabel.Text = "Stop Power [dBm]";
            // 
            // stopFrequencyLabel
            // 
            this.stopFrequencyLabel.AutoSize = true;
            this.stopFrequencyLabel.Location = new System.Drawing.Point(29, 79);
            this.stopFrequencyLabel.Name = "stopFrequencyLabel";
            this.stopFrequencyLabel.Size = new System.Drawing.Size(101, 13);
            this.stopFrequencyLabel.TabIndex = 4;
            this.stopFrequencyLabel.Text = "End Frequency [Hz]";
            // 
            // numberStepsLabel
            // 
            this.numberStepsLabel.AutoSize = true;
            this.numberStepsLabel.Location = new System.Drawing.Point(29, 282);
            this.numberStepsLabel.Name = "numberStepsLabel";
            this.numberStepsLabel.Size = new System.Drawing.Size(86, 13);
            this.numberStepsLabel.TabIndex = 5;
            this.numberStepsLabel.Text = "Number of Steps";
            // 
            // startFrequencyNumeric
            // 
            this.startFrequencyNumeric.DecimalPlaces = 6;
            this.startFrequencyNumeric.Location = new System.Drawing.Point(25, 48);
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
            this.startFrequencyNumeric.TabIndex = 1;
            this.startFrequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // startPowerNumeric
            // 
            this.startPowerNumeric.DecimalPlaces = 2;
            this.startPowerNumeric.Location = new System.Drawing.Point(25, 180);
            this.startPowerNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.startPowerNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.startPowerNumeric.Name = "startPowerNumeric";
            this.startPowerNumeric.Size = new System.Drawing.Size(120, 20);
            this.startPowerNumeric.TabIndex = 3;
            this.startPowerNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // stopPowerNumeric
            // 
            this.stopPowerNumeric.DecimalPlaces = 2;
            this.stopPowerNumeric.Location = new System.Drawing.Point(25, 231);
            this.stopPowerNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.stopPowerNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.stopPowerNumeric.Name = "stopPowerNumeric";
            this.stopPowerNumeric.Size = new System.Drawing.Size(120, 20);
            this.stopPowerNumeric.TabIndex = 4;
            // 
            // stopFrequencyNumeric
            // 
            this.stopFrequencyNumeric.DecimalPlaces = 6;
            this.stopFrequencyNumeric.Location = new System.Drawing.Point(25, 100);
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
            this.stopFrequencyNumeric.TabIndex = 2;
            this.stopFrequencyNumeric.Value = new decimal(new int[] {
            1020000000,
            0,
            0,
            0});
            // 
            // numberStepsNumeric
            // 
            this.numberStepsNumeric.Location = new System.Drawing.Point(25, 300);
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
            this.numberStepsNumeric.TabIndex = 5;
            this.numberStepsNumeric.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(22, 21);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(17, 24);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 6;
            this.iqRateLabel.Text = "IQ Rate (S/s)";
            // 
            // actualContinuousWaveformToneDurationLabel
            // 
            this.actualContinuousWaveformToneDurationLabel.AutoSize = true;
            this.actualContinuousWaveformToneDurationLabel.Location = new System.Drawing.Point(16, 80);
            this.actualContinuousWaveformToneDurationLabel.Name = "actualContinuousWaveformToneDurationLabel";
            this.actualContinuousWaveformToneDurationLabel.Size = new System.Drawing.Size(190, 13);
            this.actualContinuousWaveformToneDurationLabel.TabIndex = 7;
            this.actualContinuousWaveformToneDurationLabel.Text = "continuousWaveformTone Duration (s)";
            // 
            // actualIdleWaveformDurationLabel
            // 
            this.actualIdleWaveformDurationLabel.AutoSize = true;
            this.actualIdleWaveformDurationLabel.Location = new System.Drawing.Point(16, 132);
            this.actualIdleWaveformDurationLabel.Name = "actualIdleWaveformDurationLabel";
            this.actualIdleWaveformDurationLabel.Size = new System.Drawing.Size(129, 13);
            this.actualIdleWaveformDurationLabel.TabIndex = 8;
            this.actualIdleWaveformDurationLabel.Text = "idleWaveform Duration (s)";
            // 
            // actualIQRateLabel
            // 
            this.actualIQRateLabel.AutoSize = true;
            this.actualIQRateLabel.Location = new System.Drawing.Point(17, 28);
            this.actualIQRateLabel.Name = "actualIQRateLabel";
            this.actualIQRateLabel.Size = new System.Drawing.Size(103, 13);
            this.actualIQRateLabel.TabIndex = 9;
            this.actualIQRateLabel.Text = "Actual IQ Rate (S/s)";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.AutoSize = true;
            this.clockSourceLabel.Location = new System.Drawing.Point(17, 79);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.Size = new System.Drawing.Size(147, 13);
            this.clockSourceLabel.TabIndex = 10;
            this.clockSourceLabel.Text = "Frequency Reference Source";
            // 
            // loopBandwidthLabel
            // 
            this.loopBandwidthLabel.AutoSize = true;
            this.loopBandwidthLabel.Location = new System.Drawing.Point(17, 131);
            this.loopBandwidthLabel.Name = "loopBandwidthLabel";
            this.loopBandwidthLabel.Size = new System.Drawing.Size(84, 13);
            this.loopBandwidthLabel.TabIndex = 11;
            this.loopBandwidthLabel.Text = "Loop Bandwidth";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(23, 450);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 12;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // scriptLabel
            // 
            this.scriptLabel.AutoSize = true;
            this.scriptLabel.Location = new System.Drawing.Point(23, 77);
            this.scriptLabel.Name = "scriptLabel";
            this.scriptLabel.Size = new System.Drawing.Size(34, 13);
            this.scriptLabel.TabIndex = 13;
            this.scriptLabel.Text = "Script";
            // 
            // iqRateNumeric
            // 
            this.iqRateNumeric.DecimalPlaces = 2;
            this.iqRateNumeric.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.iqRateNumeric.Location = new System.Drawing.Point(17, 45);
            this.iqRateNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.iqRateNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.iqRateNumeric.Name = "iqRateNumeric";
            this.iqRateNumeric.Size = new System.Drawing.Size(120, 20);
            this.iqRateNumeric.TabIndex = 8;
            this.iqRateNumeric.Value = new decimal(new int[] {
            5000000,
            0,
            0,
            0});
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(22, 42);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // actualContinuousWaveformToneDurationTextBox
            // 
            this.actualContinuousWaveformToneDurationTextBox.Location = new System.Drawing.Point(17, 98);
            this.actualContinuousWaveformToneDurationTextBox.Name = "actualContinuousWaveformToneDurationTextBox";
            this.actualContinuousWaveformToneDurationTextBox.ReadOnly = true;
            this.actualContinuousWaveformToneDurationTextBox.Size = new System.Drawing.Size(126, 20);
            this.actualContinuousWaveformToneDurationTextBox.TabIndex = 9;
            this.actualContinuousWaveformToneDurationTextBox.TabStop = false;
            this.actualContinuousWaveformToneDurationTextBox.Text = "0.00";
            // 
            // actualIdleWaveformDurationTextBox
            // 
            this.actualIdleWaveformDurationTextBox.Location = new System.Drawing.Point(17, 150);
            this.actualIdleWaveformDurationTextBox.Name = "actualIdleWaveformDurationTextBox";
            this.actualIdleWaveformDurationTextBox.ReadOnly = true;
            this.actualIdleWaveformDurationTextBox.Size = new System.Drawing.Size(126, 20);
            this.actualIdleWaveformDurationTextBox.TabIndex = 10;
            this.actualIdleWaveformDurationTextBox.TabStop = false;
            this.actualIdleWaveformDurationTextBox.Text = "0.00000000000000E+0";
            // 
            // actualIQRateTextBox
            // 
            this.actualIQRateTextBox.Location = new System.Drawing.Point(18, 46);
            this.actualIQRateTextBox.Name = "actualIQRateTextBox";
            this.actualIQRateTextBox.ReadOnly = true;
            this.actualIQRateTextBox.Size = new System.Drawing.Size(126, 20);
            this.actualIQRateTextBox.TabIndex = 11;
            this.actualIQRateTextBox.TabStop = false;
            this.actualIQRateTextBox.Text = "0.00";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(22, 471);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(589, 47);
            this.errorTextBox.TabIndex = 16;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No Error";
            // 
            // scriptTextBox
            // 
            this.scriptTextBox.AcceptsReturn = true;
            this.scriptTextBox.Location = new System.Drawing.Point(22, 93);
            this.scriptTextBox.Multiline = true;
            this.scriptTextBox.Name = "scriptTextBox";
            this.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.scriptTextBox.Size = new System.Drawing.Size(397, 138);
            this.scriptTextBox.TabIndex = 1;
            this.scriptTextBox.Text = "script configurationListControlScript\r\n    repeat forever\r\n       generate contin" +
                "uousWaveformTone\r\n       generate idleWaveform marker0(0)\r\n    end repeat\r\nend s" +
                "cript";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(454, 42);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(536, 42);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 5;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // clockSourceComboBox
            // 
            this.clockSourceComboBox.Location = new System.Drawing.Point(17, 95);
            this.clockSourceComboBox.Name = "clockSourceComboBox";
            this.clockSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.clockSourceComboBox.TabIndex = 9;
            // 
            // loopBandwidthComboBox
            // 
            this.loopBandwidthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loopBandwidthComboBox.Location = new System.Drawing.Point(17, 149);
            this.loopBandwidthComboBox.Name = "loopBandwidthComboBox";
            this.loopBandwidthComboBox.Size = new System.Drawing.Size(120, 21);
            this.loopBandwidthComboBox.TabIndex = 10;
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.iqRateNumeric);
            this.configurationGroupBox.Controls.Add(this.loopBandwidthComboBox);
            this.configurationGroupBox.Controls.Add(this.clockSourceComboBox);
            this.configurationGroupBox.Controls.Add(this.loopBandwidthLabel);
            this.configurationGroupBox.Controls.Add(this.iqRateLabel);
            this.configurationGroupBox.Controls.Add(this.clockSourceLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(22, 244);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(169, 186);
            this.configurationGroupBox.TabIndex = 2;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualIQRateLabel);
            this.measurementGroupBox.Controls.Add(this.actualIQRateTextBox);
            this.measurementGroupBox.Controls.Add(this.actualIdleWaveformDurationTextBox);
            this.measurementGroupBox.Controls.Add(this.actualContinuousWaveformToneDurationTextBox);
            this.measurementGroupBox.Controls.Add(this.actualIdleWaveformDurationLabel);
            this.measurementGroupBox.Controls.Add(this.actualContinuousWaveformToneDurationLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(207, 244);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(212, 184);
            this.measurementGroupBox.TabIndex = 0;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(661, 563);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.configurationListGroupBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.scriptLabel);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.scriptTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Frequency And Power Sweep (Script Triggered)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.configurationListGroupBox.ResumeLayout(false);
            this.configurationListGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startPowerNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopPowerNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberStepsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.GroupBox configurationListGroupBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label startFrequencyLabel;
        private System.Windows.Forms.Label startPowerLabel;
        private System.Windows.Forms.Label stopPowerLabel;
        private System.Windows.Forms.Label stopFrequencyLabel;
        private System.Windows.Forms.Label numberStepsLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label actualContinuousWaveformToneDurationLabel;
        private System.Windows.Forms.Label actualIdleWaveformDurationLabel;
        private System.Windows.Forms.Label actualIQRateLabel;
        private System.Windows.Forms.Label clockSourceLabel;
        private System.Windows.Forms.Label loopBandwidthLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label scriptLabel;
        private System.Windows.Forms.NumericUpDown startFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown startPowerNumeric;
        private System.Windows.Forms.NumericUpDown stopPowerNumeric;
        private System.Windows.Forms.NumericUpDown stopFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown numberStepsNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox actualContinuousWaveformToneDurationTextBox;
        private System.Windows.Forms.TextBox actualIdleWaveformDurationTextBox;
        private System.Windows.Forms.TextBox actualIQRateTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.TextBox scriptTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox clockSourceComboBox;
        private System.Windows.Forms.ComboBox loopBandwidthComboBox;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;

    }
}
