namespace NationalInstruments.Examples.WaveformFromBinaryFile
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
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.signalBandwidthLabel = new System.Windows.Forms.Label();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.preGainLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.directDownloadLabel = new System.Windows.Forms.Label();
            this.pathLabel = new System.Windows.Forms.Label();
            this.powerLevelTypeLabel = new System.Windows.Forms.Label();
            this.actualFrequencyOffsetLabel = new System.Windows.Forms.Label();
            this.actualIQRateLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.generatingLabel = new System.Windows.Forms.Label();
            this.actualTotalSamplesLabel = new System.Windows.Forms.Label();
            this.samplesWrittenLabel = new System.Windows.Forms.Label();
            this.signalBandwidthNumeric = new System.Windows.Forms.NumericUpDown();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.preGainNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.actualFrequencyOffsetTextBox = new System.Windows.Forms.TextBox();
            this.actualIQRateTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.actualTotalSamplesTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();            this.powerLevelTypeComboBox = new System.Windows.Forms.ComboBox();
            this.generatingLed = new System.Windows.Forms.Button();
            this.samplesWrittenProgressBar = new System.Windows.Forms.ProgressBar();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.directDownloadCheckBox = new System.Windows.Forms.CheckBox();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.signalBandwidthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.preGainNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            this.measurementGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(17, 16);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // signalBandwidthLabel
            // 
            this.signalBandwidthLabel.AutoSize = true;
            this.signalBandwidthLabel.Location = new System.Drawing.Point(14, 177);
            this.signalBandwidthLabel.Name = "signalBandwidthLabel";
            this.signalBandwidthLabel.Size = new System.Drawing.Size(111, 13);
            this.signalBandwidthLabel.TabIndex = 1;
            this.signalBandwidthLabel.Text = "Signal Bandwidth [Hz]";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.AutoSize = true;
            this.frequencyLabel.Location = new System.Drawing.Point(14, 24);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.frequencyLabel.TabIndex = 2;
            this.frequencyLabel.Text = "Center Frequency [Hz]";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(14, 78);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 3;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // preGainLabel
            // 
            this.preGainLabel.AutoSize = true;
            this.preGainLabel.Location = new System.Drawing.Point(14, 129);
            this.preGainLabel.Name = "preGainLabel";
            this.preGainLabel.Size = new System.Drawing.Size(92, 13);
            this.preGainLabel.TabIndex = 4;
            this.preGainLabel.Text = "Pre-filter Gain [dB]";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(164, 129);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 5;
            this.iqRateLabel.Text = "IQ Rate [S/s]";
            // 
            // directDownloadLabel
            // 
            this.directDownloadLabel.AutoSize = true;
            this.directDownloadLabel.Location = new System.Drawing.Point(164, 23);
            this.directDownloadLabel.Name = "directDownloadLabel";
            this.directDownloadLabel.Size = new System.Drawing.Size(86, 13);
            this.directDownloadLabel.TabIndex = 6;
            this.directDownloadLabel.Text = "Direct Download";
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(331, 97);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(112, 13);
            this.pathLabel.TabIndex = 7;
            this.pathLabel.Text = "Path to Waveform File";
            // 
            // powerLevelTypeLabel
            // 
            this.powerLevelTypeLabel.AutoSize = true;
            this.powerLevelTypeLabel.Location = new System.Drawing.Point(164, 78);
            this.powerLevelTypeLabel.Name = "powerLevelTypeLabel";
            this.powerLevelTypeLabel.Size = new System.Drawing.Size(93, 13);
            this.powerLevelTypeLabel.TabIndex = 8;
            this.powerLevelTypeLabel.Text = "Power Level Type";
            // 
            // actualFrequencyOffsetLabel
            // 
            this.actualFrequencyOffsetLabel.AutoSize = true;
            this.actualFrequencyOffsetLabel.Location = new System.Drawing.Point(141, 29);
            this.actualFrequencyOffsetLabel.Name = "actualFrequencyOffsetLabel";
            this.actualFrequencyOffsetLabel.Size = new System.Drawing.Size(110, 13);
            this.actualFrequencyOffsetLabel.TabIndex = 9;
            this.actualFrequencyOffsetLabel.Text = "Frequency Offset [Hz]";
            // 
            // actualIQRateLabel
            // 
            this.actualIQRateLabel.AutoSize = true;
            this.actualIQRateLabel.Location = new System.Drawing.Point(141, 79);
            this.actualIQRateLabel.Name = "actualIQRateLabel";
            this.actualIQRateLabel.Size = new System.Drawing.Size(103, 13);
            this.actualIQRateLabel.TabIndex = 10;
            this.actualIQRateLabel.Text = "Actual IQ Rate [S/s]";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(17, 312);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 11;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // generatingLabel
            // 
            this.generatingLabel.AutoSize = true;
            this.generatingLabel.Location = new System.Drawing.Point(375, 39);
            this.generatingLabel.Name = "generatingLabel";
            this.generatingLabel.Size = new System.Drawing.Size(59, 13);
            this.generatingLabel.TabIndex = 12;
            this.generatingLabel.Text = "Generating";
            // 
            // actualTotalSamplesLabel
            // 
            this.actualTotalSamplesLabel.AutoSize = true;
            this.actualTotalSamplesLabel.Location = new System.Drawing.Point(19, 29);
            this.actualTotalSamplesLabel.Name = "actualTotalSamplesLabel";
            this.actualTotalSamplesLabel.Size = new System.Drawing.Size(84, 13);
            this.actualTotalSamplesLabel.TabIndex = 13;
            this.actualTotalSamplesLabel.Text = "Total # Samples";
            // 
            // samplesWrittenLabel
            // 
            this.samplesWrittenLabel.AutoSize = true;
            this.samplesWrittenLabel.Location = new System.Drawing.Point(19, 77);
            this.samplesWrittenLabel.Name = "samplesWrittenLabel";
            this.samplesWrittenLabel.Size = new System.Drawing.Size(102, 13);
            this.samplesWrittenLabel.TabIndex = 14;
            this.samplesWrittenLabel.Text = "% of samples written";
            // 
            // signalBandwidthNumeric
            // 
            this.signalBandwidthNumeric.DecimalPlaces = 6;
            this.signalBandwidthNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.signalBandwidthNumeric.Location = new System.Drawing.Point(14, 196);
            this.signalBandwidthNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.signalBandwidthNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.signalBandwidthNumeric.Name = "signalBandwidthNumeric";
            this.signalBandwidthNumeric.Size = new System.Drawing.Size(120, 20);
            this.signalBandwidthNumeric.TabIndex = 5;
            this.signalBandwidthNumeric.Value = new decimal(new int[] {
            5000000,
            0,
            0,
            0});
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.DecimalPlaces = 6;
            this.frequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.frequencyNumeric.Location = new System.Drawing.Point(14, 43);
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
            this.frequencyNumeric.TabIndex = 2;
            this.frequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.DecimalPlaces = 1;
            this.powerLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.powerLevelNumeric.Location = new System.Drawing.Point(14, 97);
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
            this.powerLevelNumeric.TabIndex = 3;
            this.powerLevelNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // preGainNumeric
            // 
            this.preGainNumeric.DecimalPlaces = 1;
            this.preGainNumeric.Location = new System.Drawing.Point(14, 148);
            this.preGainNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.preGainNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.preGainNumeric.Name = "preGainNumeric";
            this.preGainNumeric.Size = new System.Drawing.Size(120, 20);
            this.preGainNumeric.TabIndex = 4;
            this.preGainNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            // 
            // iqRateNumeric
            // 
            this.iqRateNumeric.DecimalPlaces = 6;
            this.iqRateNumeric.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.iqRateNumeric.Location = new System.Drawing.Point(163, 148);
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
            833333333,
            0,
            0,
            131072});
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(17, 37);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(332, 118);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(246, 20);
            this.pathTextBox.TabIndex = 3;
            this.pathTextBox.Text = "..\\..\\ChirpWaveform.bin";
            // 
            // actualFrequencyOffsetTextBox
            // 
            this.actualFrequencyOffsetTextBox.Location = new System.Drawing.Point(144, 47);
            this.actualFrequencyOffsetTextBox.Name = "actualFrequencyOffsetTextBox";
            this.actualFrequencyOffsetTextBox.ReadOnly = true;
            this.actualFrequencyOffsetTextBox.Size = new System.Drawing.Size(111, 20);
            this.actualFrequencyOffsetTextBox.TabIndex = 14;
            this.actualFrequencyOffsetTextBox.TabStop = false;
            this.actualFrequencyOffsetTextBox.Text = "100000000.000000";
            // 
            // actualIQRateTextBox
            // 
            this.actualIQRateTextBox.Location = new System.Drawing.Point(144, 95);
            this.actualIQRateTextBox.Name = "actualIQRateTextBox";
            this.actualIQRateTextBox.ReadOnly = true;
            this.actualIQRateTextBox.Size = new System.Drawing.Size(111, 20);
            this.actualIQRateTextBox.TabIndex = 15;
            this.actualIQRateTextBox.TabStop = false;
            this.actualIQRateTextBox.Text = "8333333.330000";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(17, 328);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(600, 47);
            this.errorTextBox.TabIndex = 17;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // actualTotalSamplesTextBox
            // 
            this.actualTotalSamplesTextBox.Location = new System.Drawing.Point(19, 46);
            this.actualTotalSamplesTextBox.Name = "actualTotalSamplesTextBox";
            this.actualTotalSamplesTextBox.ReadOnly = true;
            this.actualTotalSamplesTextBox.Size = new System.Drawing.Size(99, 20);
            this.actualTotalSamplesTextBox.TabIndex = 19;
            this.actualTotalSamplesTextBox.TabStop = false;
            this.actualTotalSamplesTextBox.Text = "10000";
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(584, 115);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(32, 23);
            this.browseButton.TabIndex = 4;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(461, 35);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(542, 34);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 6;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            //             // powerLevelTypeComboBox
            // 
            this.powerLevelTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.powerLevelTypeComboBox.Location = new System.Drawing.Point(163, 97);
            this.powerLevelTypeComboBox.Name = "powerLevelTypeComboBox";
            this.powerLevelTypeComboBox.Size = new System.Drawing.Size(120, 21);
            this.powerLevelTypeComboBox.TabIndex = 7;
            // 
            // generatingLed
            // 
            this.generatingLed.BackColor = System.Drawing.SystemColors.Control;
            this.generatingLed.Enabled = false;
            this.generatingLed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.generatingLed.Location = new System.Drawing.Point(349, 36);
            this.generatingLed.Name = "generatingLed";
            this.generatingLed.Size = new System.Drawing.Size(20, 20);
            this.generatingLed.TabIndex = 20;
            this.generatingLed.TabStop = false;
            this.generatingLed.UseVisualStyleBackColor = false;
            // 
            // samplesWrittenProgressBar
            // 
            this.samplesWrittenProgressBar.Location = new System.Drawing.Point(19, 92);
            this.samplesWrittenProgressBar.Name = "samplesWrittenProgressBar";
            this.samplesWrittenProgressBar.Size = new System.Drawing.Size(99, 23);
            this.samplesWrittenProgressBar.TabIndex = 20;
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // directDownloadCheckBox
            // 
            this.directDownloadCheckBox.AutoSize = true;
            this.directDownloadCheckBox.Checked = true;
            this.directDownloadCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.directDownloadCheckBox.Location = new System.Drawing.Point(163, 44);
            this.directDownloadCheckBox.Name = "directDownloadCheckBox";
            this.directDownloadCheckBox.Size = new System.Drawing.Size(59, 17);
            this.directDownloadCheckBox.TabIndex = 6;
            this.directDownloadCheckBox.Text = "Enable";
            this.directDownloadCheckBox.UseVisualStyleBackColor = true;
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.powerLevelTypeComboBox);
            this.configurationGroupBox.Controls.Add(this.iqRateNumeric);
            this.configurationGroupBox.Controls.Add(this.directDownloadCheckBox);
            this.configurationGroupBox.Controls.Add(this.preGainNumeric);
            this.configurationGroupBox.Controls.Add(this.powerLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.signalBandwidthLabel);
            this.configurationGroupBox.Controls.Add(this.frequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.frequencyLabel);
            this.configurationGroupBox.Controls.Add(this.signalBandwidthNumeric);
            this.configurationGroupBox.Controls.Add(this.powerLevelLabel);
            this.configurationGroupBox.Controls.Add(this.powerLevelTypeLabel);
            this.configurationGroupBox.Controls.Add(this.preGainLabel);
            this.configurationGroupBox.Controls.Add(this.directDownloadLabel);
            this.configurationGroupBox.Controls.Add(this.iqRateLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(17, 73);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(298, 226);
            this.configurationGroupBox.TabIndex = 2;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualFrequencyOffsetTextBox);
            this.measurementGroupBox.Controls.Add(this.samplesWrittenProgressBar);
            this.measurementGroupBox.Controls.Add(this.actualTotalSamplesTextBox);
            this.measurementGroupBox.Controls.Add(this.actualIQRateTextBox);
            this.measurementGroupBox.Controls.Add(this.samplesWrittenLabel);
            this.measurementGroupBox.Controls.Add(this.actualTotalSamplesLabel);
            this.measurementGroupBox.Controls.Add(this.actualIQRateLabel);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyOffsetLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(334, 173);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(283, 126);
            this.measurementGroupBox.TabIndex = 3;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;            this.ClientSize = new System.Drawing.Size(631, 412);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.generatingLabel);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);            this.Controls.Add(this.generatingLed);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Waveform From File";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.signalBandwidthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.preGainNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		
		private System.Windows.Forms.Label resourceNameLabel;
private System.Windows.Forms.Label signalBandwidthLabel;
private System.Windows.Forms.Label frequencyLabel;
private System.Windows.Forms.Label powerLevelLabel;
private System.Windows.Forms.Label preGainLabel;
private System.Windows.Forms.Label iqRateLabel;
private System.Windows.Forms.Label directDownloadLabel;
private System.Windows.Forms.Label pathLabel;
private System.Windows.Forms.Label powerLevelTypeLabel;
private System.Windows.Forms.Label actualFrequencyOffsetLabel;
private System.Windows.Forms.Label actualIQRateLabel;
private System.Windows.Forms.Label errorLabel;
private System.Windows.Forms.Label generatingLabel;
private System.Windows.Forms.Label actualTotalSamplesLabel;
private System.Windows.Forms.Label samplesWrittenLabel;
private System.Windows.Forms.NumericUpDown signalBandwidthNumeric;
private System.Windows.Forms.NumericUpDown frequencyNumeric;
private System.Windows.Forms.NumericUpDown powerLevelNumeric;
private System.Windows.Forms.NumericUpDown preGainNumeric;
private System.Windows.Forms.NumericUpDown iqRateNumeric;
private System.Windows.Forms.ComboBox resourceNameComboBox;
private System.Windows.Forms.TextBox pathTextBox;
private System.Windows.Forms.TextBox actualFrequencyOffsetTextBox;
private System.Windows.Forms.TextBox actualIQRateTextBox;
private System.Windows.Forms.TextBox errorTextBox;
private System.Windows.Forms.TextBox actualTotalSamplesTextBox;
private System.Windows.Forms.Button browseButton;
private System.Windows.Forms.Button startButton;
private System.Windows.Forms.Button stopButton;private System.Windows.Forms.ComboBox powerLevelTypeComboBox;
private System.Windows.Forms.Timer rfsgStatusTimer;
private System.Windows.Forms.Button generatingLed;
private System.Windows.Forms.ProgressBar samplesWrittenProgressBar;
private System.Windows.Forms.CheckBox directDownloadCheckBox;
private System.Windows.Forms.GroupBox configurationGroupBox;
private System.Windows.Forms.GroupBox measurementGroupBox;
		
	}
}
