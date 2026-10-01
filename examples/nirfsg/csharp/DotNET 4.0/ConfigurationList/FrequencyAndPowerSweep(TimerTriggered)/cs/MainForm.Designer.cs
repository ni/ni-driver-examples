using System;
namespace NationalInstruments.Examples.FrequencyAndPowerSweepTimerTriggered
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
            this.dwellTimeLabel = new System.Windows.Forms.Label();
            this.startFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.startPowerNumeric = new System.Windows.Forms.NumericUpDown();
            this.stopPowerNumeric = new System.Windows.Forms.NumericUpDown();
            this.stopFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.numberStepsNumeric = new System.Windows.Forms.NumericUpDown();
            this.dwellTimeNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.actualIQRateLabel = new System.Windows.Forms.Label();
            this.clockSourceLabel = new System.Windows.Forms.Label();
            this.loopBandwidthLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.scriptLabel = new System.Windows.Forms.Label();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.actualIQRateTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.scriptTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.clockSourceComboBox = new System.Windows.Forms.ComboBox();
            this.loopBandwidthComboBox = new System.Windows.Forms.ComboBox();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.configurationListGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startPowerNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopPowerNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberStepsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwellTimeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // configurationListGroupBox
            // 
            this.configurationListGroupBox.Controls.Add(this.startFrequencyLabel);
            this.configurationListGroupBox.Controls.Add(this.startPowerLabel);
            this.configurationListGroupBox.Controls.Add(this.stopPowerLabel);
            this.configurationListGroupBox.Controls.Add(this.stopFrequencyLabel);
            this.configurationListGroupBox.Controls.Add(this.numberStepsLabel);
            this.configurationListGroupBox.Controls.Add(this.dwellTimeLabel);
            this.configurationListGroupBox.Controls.Add(this.startFrequencyNumeric);
            this.configurationListGroupBox.Controls.Add(this.startPowerNumeric);
            this.configurationListGroupBox.Controls.Add(this.stopPowerNumeric);
            this.configurationListGroupBox.Controls.Add(this.stopFrequencyNumeric);
            this.configurationListGroupBox.Controls.Add(this.numberStepsNumeric);
            this.configurationListGroupBox.Controls.Add(this.dwellTimeNumeric);
            this.configurationListGroupBox.Location = new System.Drawing.Point(348, 86);
            this.configurationListGroupBox.Name = "configurationListGroupBox";
            this.configurationListGroupBox.Size = new System.Drawing.Size(175, 335);
            this.configurationListGroupBox.TabIndex = 11;
            this.configurationListGroupBox.TabStop = false;
            this.configurationListGroupBox.Text = "Configuration List Parameters";
            // 
            // startFrequencyLabel
            // 
            this.startFrequencyLabel.AutoSize = true;
            this.startFrequencyLabel.Location = new System.Drawing.Point(23, 26);
            this.startFrequencyLabel.Name = "startFrequencyLabel";
            this.startFrequencyLabel.Size = new System.Drawing.Size(104, 13);
            this.startFrequencyLabel.TabIndex = 1;
            this.startFrequencyLabel.Text = "Start Frequency [Hz]";
            // 
            // startPowerLabel
            // 
            this.startPowerLabel.AutoSize = true;
            this.startPowerLabel.Location = new System.Drawing.Point(19, 136);
            this.startPowerLabel.Name = "startPowerLabel";
            this.startPowerLabel.Size = new System.Drawing.Size(92, 13);
            this.startPowerLabel.TabIndex = 2;
            this.startPowerLabel.Text = "Start Power [dBm]";
            // 
            // stopPowerLabel
            // 
            this.stopPowerLabel.AutoSize = true;
            this.stopPowerLabel.Location = new System.Drawing.Point(19, 183);
            this.stopPowerLabel.Name = "stopPowerLabel";
            this.stopPowerLabel.Size = new System.Drawing.Size(92, 13);
            this.stopPowerLabel.TabIndex = 3;
            this.stopPowerLabel.Text = "Stop Power [dBm]";
            // 
            // stopFrequencyLabel
            // 
            this.stopFrequencyLabel.AutoSize = true;
            this.stopFrequencyLabel.Location = new System.Drawing.Point(19, 78);
            this.stopFrequencyLabel.Name = "stopFrequencyLabel";
            this.stopFrequencyLabel.Size = new System.Drawing.Size(101, 13);
            this.stopFrequencyLabel.TabIndex = 4;
            this.stopFrequencyLabel.Text = "End Frequency [Hz]";
            // 
            // numberStepsLabel
            // 
            this.numberStepsLabel.AutoSize = true;
            this.numberStepsLabel.Location = new System.Drawing.Point(19, 239);
            this.numberStepsLabel.Name = "numberStepsLabel";
            this.numberStepsLabel.Size = new System.Drawing.Size(86, 13);
            this.numberStepsLabel.TabIndex = 5;
            this.numberStepsLabel.Text = "Number of Steps";
            // 
            // dwellTimeLabel
            // 
            this.dwellTimeLabel.AutoSize = true;
            this.dwellTimeLabel.Location = new System.Drawing.Point(19, 289);
            this.dwellTimeLabel.Name = "dwellTimeLabel";
            this.dwellTimeLabel.Size = new System.Drawing.Size(137, 13);
            this.dwellTimeLabel.TabIndex = 6;
            this.dwellTimeLabel.Text = "Dwell Time in Each Step [s]";
            // 
            // startFrequencyNumeric
            // 
            this.startFrequencyNumeric.DecimalPlaces = 6;
            this.startFrequencyNumeric.Location = new System.Drawing.Point(19, 42);
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
            this.startFrequencyNumeric.TabIndex = 5;
            this.startFrequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // startPowerNumeric
            // 
            this.startPowerNumeric.DecimalPlaces = 2;
            this.startPowerNumeric.Location = new System.Drawing.Point(19, 152);
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
            this.startPowerNumeric.TabIndex = 7;
            this.startPowerNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // stopPowerNumeric
            // 
            this.stopPowerNumeric.DecimalPlaces = 2;
            this.stopPowerNumeric.Location = new System.Drawing.Point(19, 199);
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
            this.stopPowerNumeric.TabIndex = 8;
            // 
            // stopFrequencyNumeric
            // 
            this.stopFrequencyNumeric.DecimalPlaces = 6;
            this.stopFrequencyNumeric.Location = new System.Drawing.Point(19, 94);
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
            this.stopFrequencyNumeric.TabIndex = 6;
            this.stopFrequencyNumeric.Value = new decimal(new int[] {
            1020000000,
            0,
            0,
            0});
            // 
            // numberStepsNumeric
            // 
            this.numberStepsNumeric.Location = new System.Drawing.Point(19, 255);
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
            this.numberStepsNumeric.TabIndex = 9;
            this.numberStepsNumeric.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            // 
            // dwellTimeNumeric
            // 
            this.dwellTimeNumeric.DecimalPlaces = 3;
            this.dwellTimeNumeric.Location = new System.Drawing.Point(19, 305);
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
            this.dwellTimeNumeric.TabIndex = 10;
            this.dwellTimeNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            131072});
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
            this.iqRateLabel.Location = new System.Drawing.Point(23, 254);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 7;
            this.iqRateLabel.Text = "IQ Rate (S/s)";
            // 
            // actualIQRateLabel
            // 
            this.actualIQRateLabel.AutoSize = true;
            this.actualIQRateLabel.Location = new System.Drawing.Point(196, 254);
            this.actualIQRateLabel.Name = "actualIQRateLabel";
            this.actualIQRateLabel.Size = new System.Drawing.Size(103, 13);
            this.actualIQRateLabel.TabIndex = 8;
            this.actualIQRateLabel.Text = "Actual IQ Rate (S/s)";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.AutoSize = true;
            this.clockSourceLabel.Location = new System.Drawing.Point(22, 308);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.Size = new System.Drawing.Size(147, 13);
            this.clockSourceLabel.TabIndex = 9;
            this.clockSourceLabel.Text = "Frequency Reference Source";
            // 
            // loopBandwidthLabel
            // 
            this.loopBandwidthLabel.AutoSize = true;
            this.loopBandwidthLabel.Location = new System.Drawing.Point(22, 364);
            this.loopBandwidthLabel.Name = "loopBandwidthLabel";
            this.loopBandwidthLabel.Size = new System.Drawing.Size(84, 13);
            this.loopBandwidthLabel.TabIndex = 10;
            this.loopBandwidthLabel.Text = "Loop Bandwidth";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(23, 442);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 11;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // scriptLabel
            // 
            this.scriptLabel.AutoSize = true;
            this.scriptLabel.Location = new System.Drawing.Point(22, 86);
            this.scriptLabel.Name = "scriptLabel";
            this.scriptLabel.Size = new System.Drawing.Size(34, 13);
            this.scriptLabel.TabIndex = 12;
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
            this.iqRateNumeric.Location = new System.Drawing.Point(22, 272);
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
            this.iqRateNumeric.TabIndex = 2;
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
            // actualIQRateTextBox
            // 
            this.actualIQRateTextBox.Location = new System.Drawing.Point(197, 272);
            this.actualIQRateTextBox.Name = "actualIQRateTextBox";
            this.actualIQRateTextBox.ReadOnly = true;
            this.actualIQRateTextBox.Size = new System.Drawing.Size(126, 20);
            this.actualIQRateTextBox.TabIndex = 10;
            this.actualIQRateTextBox.TabStop = false;
            this.actualIQRateTextBox.Text = "0.00";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(22, 463);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(501, 47);
            this.errorTextBox.TabIndex = 15;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No Error";
            // 
            // scriptTextBox
            // 
            this.scriptTextBox.Location = new System.Drawing.Point(23, 107);
            this.scriptTextBox.Multiline = true;
            this.scriptTextBox.Name = "scriptTextBox";
            this.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.scriptTextBox.Size = new System.Drawing.Size(300, 118);
            this.scriptTextBox.TabIndex = 1;
            this.scriptTextBox.Text = "script continuousWaveformToneScript\r\n   repeat forever\r\n       generate continuou" +
                "sWaveformTone\r\n   end repeat\r\nend script";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(367, 42);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 11;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(448, 42);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 12;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // clockSourceComboBox
            // 
            this.clockSourceComboBox.Location = new System.Drawing.Point(22, 329);
            this.clockSourceComboBox.Name = "clockSourceComboBox";
            this.clockSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.clockSourceComboBox.TabIndex = 3;
            // 
            // loopBandwidthComboBox
            // 
            this.loopBandwidthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loopBandwidthComboBox.Location = new System.Drawing.Point(22, 385);
            this.loopBandwidthComboBox.Name = "loopBandwidthComboBox";
            this.loopBandwidthComboBox.Size = new System.Drawing.Size(120, 21);
            this.loopBandwidthComboBox.TabIndex = 4;
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(549, 556);
            this.Controls.Add(this.configurationListGroupBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.iqRateLabel);
            this.Controls.Add(this.actualIQRateLabel);
            this.Controls.Add(this.clockSourceLabel);
            this.Controls.Add(this.loopBandwidthLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.scriptLabel);
            this.Controls.Add(this.iqRateNumeric);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.actualIQRateTextBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.scriptTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.clockSourceComboBox);
            this.Controls.Add(this.loopBandwidthComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Frequency and Power Sweep (Timer Triggered)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.configurationListGroupBox.ResumeLayout(false);
            this.configurationListGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startPowerNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopPowerNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberStepsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwellTimeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
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
        private System.Windows.Forms.Label dwellTimeLabel;
        private System.Windows.Forms.Label iqRateLabel;
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
        private System.Windows.Forms.NumericUpDown dwellTimeNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox actualIQRateTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.TextBox scriptTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox clockSourceComboBox;
        private System.Windows.Forms.ComboBox loopBandwidthComboBox;
        private System.Windows.Forms.Timer rfsgStatusTimer;

    }
}
