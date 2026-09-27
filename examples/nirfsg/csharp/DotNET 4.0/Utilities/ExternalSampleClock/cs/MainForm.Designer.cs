namespace NationalInstruments.Examples.ExternalSampleClock
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
            this.externalClockSourceLabel = new System.Windows.Forms.Label();
            this.vectorSignalGeneratorLabel = new System.Windows.Forms.Label();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.actualFrequencyOffsetLabel = new System.Windows.Forms.Label();
            this.actualArbSampleRateLabel = new System.Windows.Forms.Label();
            this.actualIQRateLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.waveformLabel = new System.Windows.Forms.Label();
            this.arbSampleClockSourceLabel = new System.Windows.Forms.Label();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.externalClockSourceComboBox = new System.Windows.Forms.ComboBox();
            this.vectorSignalGeneratorComboBox = new System.Windows.Forms.ComboBox();
            this.actualIQRateTextBox = new System.Windows.Forms.TextBox();
            this.actualFrequencyOffsetTextBox = new System.Windows.Forms.TextBox();
            this.actualArbSampleRateTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.waveformComboBox = new System.Windows.Forms.ComboBox();
            this.arbSampleClockSourceComboBox = new System.Windows.Forms.ComboBox();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.configurationParametersGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            this.measurementGroupBox.SuspendLayout();
            this.configurationParametersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // externalClockSourceLabel
            // 
            this.externalClockSourceLabel.AutoSize = true;
            this.externalClockSourceLabel.Location = new System.Drawing.Point(187, 16);
            this.externalClockSourceLabel.Name = "externalClockSourceLabel";
            this.externalClockSourceLabel.Size = new System.Drawing.Size(112, 13);
            this.externalClockSourceLabel.TabIndex = 0;
            this.externalClockSourceLabel.Text = "External Clock Source";
            // 
            // vectorSignalGeneratorLabel
            // 
            this.vectorSignalGeneratorLabel.AutoSize = true;
            this.vectorSignalGeneratorLabel.Location = new System.Drawing.Point(25, 16);
            this.vectorSignalGeneratorLabel.Name = "vectorSignalGeneratorLabel";
            this.vectorSignalGeneratorLabel.Size = new System.Drawing.Size(120, 13);
            this.vectorSignalGeneratorLabel.TabIndex = 1;
            this.vectorSignalGeneratorLabel.Text = "Vector Signal Generator";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.AutoSize = true;
            this.frequencyLabel.Location = new System.Drawing.Point(21, 23);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.frequencyLabel.TabIndex = 2;
            this.frequencyLabel.Text = "Center Frequency [Hz]";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(169, 80);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 3;
            this.iqRateLabel.Text = "IQ Rate (S/s)";
            // 
            // actualFrequencyOffsetLabel
            // 
            this.actualFrequencyOffsetLabel.AutoSize = true;
            this.actualFrequencyOffsetLabel.Location = new System.Drawing.Point(21, 80);
            this.actualFrequencyOffsetLabel.Name = "actualFrequencyOffsetLabel";
            this.actualFrequencyOffsetLabel.Size = new System.Drawing.Size(88, 13);
            this.actualFrequencyOffsetLabel.TabIndex = 4;
            this.actualFrequencyOffsetLabel.Text = "Frequency Offset";
            // 
            // actualArbSampleRateLabel
            // 
            this.actualArbSampleRateLabel.AutoSize = true;
            this.actualArbSampleRateLabel.Location = new System.Drawing.Point(21, 23);
            this.actualArbSampleRateLabel.Name = "actualArbSampleRateLabel";
            this.actualArbSampleRateLabel.Size = new System.Drawing.Size(113, 13);
            this.actualArbSampleRateLabel.TabIndex = 5;
            this.actualArbSampleRateLabel.Text = "Arb Sample Rate (S/s)";
            // 
            // actualIQRateLabel
            // 
            this.actualIQRateLabel.AutoSize = true;
            this.actualIQRateLabel.Location = new System.Drawing.Point(21, 143);
            this.actualIQRateLabel.Name = "actualIQRateLabel";
            this.actualIQRateLabel.Size = new System.Drawing.Size(103, 13);
            this.actualIQRateLabel.TabIndex = 0;
            this.actualIQRateLabel.Text = "Actual IQ Rate (S/s)";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(21, 80);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 6;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(25, 289);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 7;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // waveformLabel
            // 
            this.waveformLabel.AutoSize = true;
            this.waveformLabel.Location = new System.Drawing.Point(25, 226);
            this.waveformLabel.Name = "waveformLabel";
            this.waveformLabel.Size = new System.Drawing.Size(101, 13);
            this.waveformLabel.TabIndex = 8;
            this.waveformLabel.Text = "Selected Waveform";
            // 
            // arbSampleClockSourceLabel
            // 
            this.arbSampleClockSourceLabel.AutoSize = true;
            this.arbSampleClockSourceLabel.Location = new System.Drawing.Point(169, 23);
            this.arbSampleClockSourceLabel.Name = "arbSampleClockSourceLabel";
            this.arbSampleClockSourceLabel.Size = new System.Drawing.Size(128, 13);
            this.arbSampleClockSourceLabel.TabIndex = 9;
            this.arbSampleClockSourceLabel.Text = "Arb Sample Clock Source";
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.DecimalPlaces = 6;
            this.frequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.frequencyNumeric.Location = new System.Drawing.Point(19, 44);
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
            // iqRateNumeric
            // 
            this.iqRateNumeric.DecimalPlaces = 4;
            this.iqRateNumeric.Location = new System.Drawing.Point(172, 101);
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
            this.iqRateNumeric.TabIndex = 4;
            this.iqRateNumeric.Value = new decimal(new int[] {
            833333333,
            0,
            0,
            131072});
            // 
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.DecimalPlaces = 2;
            this.powerLevelNumeric.Location = new System.Drawing.Point(20, 101);
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
            // externalClockSourceComboBox
            // 
            this.externalClockSourceComboBox.Location = new System.Drawing.Point(187, 37);
            this.externalClockSourceComboBox.Name = "externalClockSourceComboBox";
            this.externalClockSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.externalClockSourceComboBox.TabIndex = 1;
            // 
            // vectorSignalGeneratorComboBox
            // 
            this.vectorSignalGeneratorComboBox.Location = new System.Drawing.Point(22, 37);
            this.vectorSignalGeneratorComboBox.Name = "vectorSignalGeneratorComboBox";
            this.vectorSignalGeneratorComboBox.Size = new System.Drawing.Size(120, 21);
            this.vectorSignalGeneratorComboBox.TabIndex = 0;
            // 
            // actualIQRateTextBox
            // 
            this.actualIQRateTextBox.Location = new System.Drawing.Point(21, 159);
            this.actualIQRateTextBox.Name = "actualIQRateTextBox";
            this.actualIQRateTextBox.ReadOnly = true;
            this.actualIQRateTextBox.Size = new System.Drawing.Size(109, 20);
            this.actualIQRateTextBox.TabIndex = 3;
            this.actualIQRateTextBox.TabStop = false;
            this.actualIQRateTextBox.Text = "1000000.0000";
            // 
            // actualFrequencyOffsetTextBox
            // 
            this.actualFrequencyOffsetTextBox.Location = new System.Drawing.Point(21, 100);
            this.actualFrequencyOffsetTextBox.Name = "actualFrequencyOffsetTextBox";
            this.actualFrequencyOffsetTextBox.ReadOnly = true;
            this.actualFrequencyOffsetTextBox.Size = new System.Drawing.Size(109, 20);
            this.actualFrequencyOffsetTextBox.TabIndex = 4;
            this.actualFrequencyOffsetTextBox.TabStop = false;
            this.actualFrequencyOffsetTextBox.Text = "1000000.0000";
            // 
            // actualArbSampleRateTextBox
            // 
            this.actualArbSampleRateTextBox.Location = new System.Drawing.Point(21, 43);
            this.actualArbSampleRateTextBox.Name = "actualArbSampleRateTextBox";
            this.actualArbSampleRateTextBox.ReadOnly = true;
            this.actualArbSampleRateTextBox.Size = new System.Drawing.Size(109, 20);
            this.actualArbSampleRateTextBox.TabIndex = 5;
            this.actualArbSampleRateTextBox.TabStop = false;
            this.actualArbSampleRateTextBox.Text = "1000000.0000";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(22, 305);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(499, 59);
            this.errorTextBox.TabIndex = 13;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(365, 35);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(446, 35);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 6;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // waveformComboBox
            // 
            this.waveformComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.waveformComboBox.Location = new System.Drawing.Point(22, 246);
            this.waveformComboBox.Name = "waveformComboBox";
            this.waveformComboBox.Size = new System.Drawing.Size(120, 21);
            this.waveformComboBox.TabIndex = 4;
            // 
            // arbSampleClockSourceComboBox
            // 
            this.arbSampleClockSourceComboBox.Location = new System.Drawing.Point(170, 42);
            this.arbSampleClockSourceComboBox.Name = "arbSampleClockSourceComboBox";
            this.arbSampleClockSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.arbSampleClockSourceComboBox.TabIndex = 3;
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualIQRateTextBox);
            this.measurementGroupBox.Controls.Add(this.actualArbSampleRateTextBox);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyOffsetTextBox);
            this.measurementGroupBox.Controls.Add(this.actualArbSampleRateLabel);
            this.measurementGroupBox.Controls.Add(this.actualIQRateLabel);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyOffsetLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(365, 72);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(156, 190);
            this.measurementGroupBox.TabIndex = 16;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // configurationParametersGroupBox
            // 
            this.configurationParametersGroupBox.Controls.Add(this.arbSampleClockSourceComboBox);
            this.configurationParametersGroupBox.Controls.Add(this.powerLevelNumeric);
            this.configurationParametersGroupBox.Controls.Add(this.iqRateNumeric);
            this.configurationParametersGroupBox.Controls.Add(this.frequencyNumeric);
            this.configurationParametersGroupBox.Controls.Add(this.frequencyLabel);
            this.configurationParametersGroupBox.Controls.Add(this.arbSampleClockSourceLabel);
            this.configurationParametersGroupBox.Controls.Add(this.iqRateLabel);
            this.configurationParametersGroupBox.Controls.Add(this.powerLevelLabel);
            this.configurationParametersGroupBox.Location = new System.Drawing.Point(22, 72);
            this.configurationParametersGroupBox.Name = "configurationParametersGroupBox";
            this.configurationParametersGroupBox.Size = new System.Drawing.Size(318, 140);
            this.configurationParametersGroupBox.TabIndex = 3;
            this.configurationParametersGroupBox.TabStop = false;
            this.configurationParametersGroupBox.Text = "Configuration Parameters";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(543, 404);
            this.Controls.Add(this.configurationParametersGroupBox);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.externalClockSourceLabel);
            this.Controls.Add(this.vectorSignalGeneratorLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.waveformLabel);
            this.Controls.Add(this.externalClockSourceComboBox);
            this.Controls.Add(this.vectorSignalGeneratorComboBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.waveformComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "External Sample Clock";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.configurationParametersGroupBox.ResumeLayout(false);
            this.configurationParametersGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label externalClockSourceLabel;
        private System.Windows.Forms.Label vectorSignalGeneratorLabel;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label actualFrequencyOffsetLabel;
        private System.Windows.Forms.Label actualArbSampleRateLabel;
        private System.Windows.Forms.Label actualIQRateLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label waveformLabel;
        private System.Windows.Forms.Label arbSampleClockSourceLabel;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.ComboBox externalClockSourceComboBox;
        private System.Windows.Forms.ComboBox vectorSignalGeneratorComboBox;
        private System.Windows.Forms.TextBox actualIQRateTextBox;
        private System.Windows.Forms.TextBox actualFrequencyOffsetTextBox;
        private System.Windows.Forms.TextBox actualArbSampleRateTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox waveformComboBox;
        private System.Windows.Forms.ComboBox arbSampleClockSourceComboBox;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.GroupBox measurementGroupBox;
        private System.Windows.Forms.GroupBox configurationParametersGroupBox;

    }
}
