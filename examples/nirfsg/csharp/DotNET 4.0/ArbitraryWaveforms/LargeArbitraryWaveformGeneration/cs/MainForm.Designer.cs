namespace NationalInstruments.Examples.LargeArbitraryWaveformGeneration
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
            this.startFrequencyLabel = new System.Windows.Forms.Label();
            this.stopFrequencyLabel = new System.Windows.Forms.Label();
            this.directDownloadLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.iqChirpDurationLabel = new System.Windows.Forms.Label();
            this.powerLevelTypeLabel = new System.Windows.Forms.Label();
            this.actualIQNumberOfSamplesLabel = new System.Windows.Forms.Label();
            this.actualIQRateLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.generatingLabel = new System.Windows.Forms.Label();
            this.generationStatusLabel = new System.Windows.Forms.Label();
            this.startFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.stopFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqChirpDurationNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.actualIQNumberOfSamplesTextBox = new System.Windows.Forms.TextBox();
            this.actualIQChirpDurationTextBox = new System.Windows.Forms.TextBox();
            this.actualIQRateTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.powerLevelTypeComboBox = new System.Windows.Forms.ComboBox();
            this.generatingLed = new System.Windows.Forms.Button();
            this.generationStatusProgressBar = new System.Windows.Forms.ProgressBar();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.directDownloadCheckBox = new System.Windows.Forms.CheckBox();
            this.actualIQChirpDurationLabel = new System.Windows.Forms.Label();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqChirpDurationNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            this.measurementGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(28, 19);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // startFrequencyLabel
            // 
            this.startFrequencyLabel.AutoSize = true;
            this.startFrequencyLabel.Location = new System.Drawing.Point(13, 22);
            this.startFrequencyLabel.Name = "startFrequencyLabel";
            this.startFrequencyLabel.Size = new System.Drawing.Size(104, 13);
            this.startFrequencyLabel.TabIndex = 1;
            this.startFrequencyLabel.Text = "Start Frequency [Hz]";
            // 
            // stopFrequencyLabel
            // 
            this.stopFrequencyLabel.AutoSize = true;
            this.stopFrequencyLabel.Location = new System.Drawing.Point(12, 89);
            this.stopFrequencyLabel.Name = "stopFrequencyLabel";
            this.stopFrequencyLabel.Size = new System.Drawing.Size(104, 13);
            this.stopFrequencyLabel.TabIndex = 2;
            this.stopFrequencyLabel.Text = "Stop Frequency [Hz]";
            // 
            // directDownloadLabel
            // 
            this.directDownloadLabel.AutoSize = true;
            this.directDownloadLabel.Location = new System.Drawing.Point(162, 90);
            this.directDownloadLabel.Name = "directDownloadLabel";
            this.directDownloadLabel.Size = new System.Drawing.Size(86, 13);
            this.directDownloadLabel.TabIndex = 3;
            this.directDownloadLabel.Text = "Direct Download";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(162, 23);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 4;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(13, 211);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 5;
            this.iqRateLabel.Text = "IQ Rate [S/s]";
            // 
            // iqChirpDurationLabel
            // 
            this.iqChirpDurationLabel.AutoSize = true;
            this.iqChirpDurationLabel.Location = new System.Drawing.Point(12, 152);
            this.iqChirpDurationLabel.Name = "iqChirpDurationLabel";
            this.iqChirpDurationLabel.Size = new System.Drawing.Size(88, 13);
            this.iqChirpDurationLabel.TabIndex = 6;
            this.iqChirpDurationLabel.Text = "Chirp Duration (s)";
            // 
            // powerLevelTypeLabel
            // 
            this.powerLevelTypeLabel.AutoSize = true;
            this.powerLevelTypeLabel.Location = new System.Drawing.Point(162, 151);
            this.powerLevelTypeLabel.Name = "powerLevelTypeLabel";
            this.powerLevelTypeLabel.Size = new System.Drawing.Size(93, 13);
            this.powerLevelTypeLabel.TabIndex = 7;
            this.powerLevelTypeLabel.Text = "Power Level Type";
            // 
            // actualIQNumberOfSamplesLabel
            // 
            this.actualIQNumberOfSamplesLabel.AutoSize = true;
            this.actualIQNumberOfSamplesLabel.Location = new System.Drawing.Point(25, 154);
            this.actualIQNumberOfSamplesLabel.Name = "actualIQNumberOfSamplesLabel";
            this.actualIQNumberOfSamplesLabel.Size = new System.Drawing.Size(126, 13);
            this.actualIQNumberOfSamplesLabel.TabIndex = 8;
            this.actualIQNumberOfSamplesLabel.Text = "Total Number of Samples";
            // 
            // actualIQRateLabel
            // 
            this.actualIQRateLabel.Location = new System.Drawing.Point(25, 84);
            this.actualIQRateLabel.Name = "actualIQRateLabel";
            this.actualIQRateLabel.Size = new System.Drawing.Size(112, 18);
            this.actualIQRateLabel.TabIndex = 9;
            this.actualIQRateLabel.Text = "Actual IQ Rate [S/s]";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(29, 354);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 10;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // generatingLabel
            // 
            this.generatingLabel.AutoSize = true;
            this.generatingLabel.Location = new System.Drawing.Point(299, 43);
            this.generatingLabel.Name = "generatingLabel";
            this.generatingLabel.Size = new System.Drawing.Size(59, 13);
            this.generatingLabel.TabIndex = 11;
            this.generatingLabel.Text = "Generating";
            // 
            // generationStatusLabel
            // 
            this.generationStatusLabel.AutoSize = true;
            this.generationStatusLabel.Location = new System.Drawing.Point(24, 213);
            this.generationStatusLabel.Name = "generationStatusLabel";
            this.generationStatusLabel.Size = new System.Drawing.Size(37, 13);
            this.generationStatusLabel.TabIndex = 12;
            this.generationStatusLabel.Text = "Status";
            // 
            // startFrequencyNumeric
            // 
            this.startFrequencyNumeric.DecimalPlaces = 6;
            this.startFrequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.startFrequencyNumeric.Location = new System.Drawing.Point(12, 43);
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
            this.stopFrequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.stopFrequencyNumeric.Location = new System.Drawing.Point(12, 110);
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
            1005000000,
            0,
            0,
            0});
            // 
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.DecimalPlaces = 2;
            this.powerLevelNumeric.Location = new System.Drawing.Point(162, 43);
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
            this.powerLevelNumeric.TabIndex = 4;
            this.powerLevelNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // iqRateNumeric
            // 
            this.iqRateNumeric.DecimalPlaces = 6;
            this.iqRateNumeric.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.iqRateNumeric.Location = new System.Drawing.Point(13, 232);
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
            this.iqRateNumeric.TabIndex = 3;
            this.iqRateNumeric.Value = new decimal(new int[] {
            8333333,
            0,
            0,
            0});
            // 
            // iqChirpDurationNumeric
            // 
            this.iqChirpDurationNumeric.DecimalPlaces = 3;
            this.iqChirpDurationNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.iqChirpDurationNumeric.Location = new System.Drawing.Point(12, 173);
            this.iqChirpDurationNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.iqChirpDurationNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.iqChirpDurationNumeric.Name = "iqChirpDurationNumeric";
            this.iqChirpDurationNumeric.Size = new System.Drawing.Size(120, 20);
            this.iqChirpDurationNumeric.TabIndex = 2;
            this.iqChirpDurationNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(28, 40);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // actualIQNumberOfSamplesTextBox
            // 
            this.actualIQNumberOfSamplesTextBox.Location = new System.Drawing.Point(25, 175);
            this.actualIQNumberOfSamplesTextBox.Name = "actualIQNumberOfSamplesTextBox";
            this.actualIQNumberOfSamplesTextBox.ReadOnly = true;
            this.actualIQNumberOfSamplesTextBox.Size = new System.Drawing.Size(120, 20);
            this.actualIQNumberOfSamplesTextBox.TabIndex = 12;
            this.actualIQNumberOfSamplesTextBox.TabStop = false;
            this.actualIQNumberOfSamplesTextBox.Text = "40000000";
            // 
            // actualIQChirpDurationTextBox
            // 
            this.actualIQChirpDurationTextBox.Location = new System.Drawing.Point(25, 46);
            this.actualIQChirpDurationTextBox.Name = "actualIQChirpDurationTextBox";
            this.actualIQChirpDurationTextBox.ReadOnly = true;
            this.actualIQChirpDurationTextBox.Size = new System.Drawing.Size(120, 20);
            this.actualIQChirpDurationTextBox.TabIndex = 13;
            this.actualIQChirpDurationTextBox.TabStop = false;
            this.actualIQChirpDurationTextBox.Text = "0.500";
            // 
            // actualIQRateTextBox
            // 
            this.actualIQRateTextBox.Location = new System.Drawing.Point(25, 105);
            this.actualIQRateTextBox.Name = "actualIQRateTextBox";
            this.actualIQRateTextBox.ReadOnly = true;
            this.actualIQRateTextBox.Size = new System.Drawing.Size(120, 20);
            this.actualIQRateTextBox.TabIndex = 14;
            this.actualIQRateTextBox.TabStop = false;
            this.actualIQRateTextBox.Text = "0.000000";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(30, 373);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(521, 43);
            this.errorTextBox.TabIndex = 16;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(388, 38);
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
            this.stopButton.Location = new System.Drawing.Point(476, 38);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // powerLevelTypeComboBox
            // 
            this.powerLevelTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.powerLevelTypeComboBox.Location = new System.Drawing.Point(162, 172);
            this.powerLevelTypeComboBox.Name = "powerLevelTypeComboBox";
            this.powerLevelTypeComboBox.Size = new System.Drawing.Size(120, 21);
            this.powerLevelTypeComboBox.TabIndex = 5;
            // 
            // generatingLed
            // 
            this.generatingLed.BackColor = System.Drawing.SystemColors.Control;
            this.generatingLed.Enabled = false;
            this.generatingLed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.generatingLed.Location = new System.Drawing.Point(274, 39);
            this.generatingLed.Name = "generatingLed";
            this.generatingLed.Size = new System.Drawing.Size(20, 20);
            this.generatingLed.TabIndex = 17;
            this.generatingLed.TabStop = false;
            this.generatingLed.UseVisualStyleBackColor = false;
            // 
            // generationStatusProgressBar
            // 
            this.generationStatusProgressBar.Location = new System.Drawing.Point(25, 234);
            this.generationStatusProgressBar.Name = "generationStatusProgressBar";
            this.generationStatusProgressBar.Size = new System.Drawing.Size(121, 21);
            this.generationStatusProgressBar.TabIndex = 18;
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
            this.directDownloadCheckBox.Location = new System.Drawing.Point(165, 113);
            this.directDownloadCheckBox.Name = "directDownloadCheckBox";
            this.directDownloadCheckBox.Size = new System.Drawing.Size(59, 17);
            this.directDownloadCheckBox.TabIndex = 5;
            this.directDownloadCheckBox.Text = "Enable";
            this.directDownloadCheckBox.UseVisualStyleBackColor = true;
            // 
            // actualIQChirpDurationLabel
            // 
            this.actualIQChirpDurationLabel.AutoSize = true;
            this.actualIQChirpDurationLabel.Location = new System.Drawing.Point(25, 25);
            this.actualIQChirpDurationLabel.Name = "actualIQChirpDurationLabel";
            this.actualIQChirpDurationLabel.Size = new System.Drawing.Size(121, 13);
            this.actualIQChirpDurationLabel.TabIndex = 20;
            this.actualIQChirpDurationLabel.Text = "Actual Chirp Duration (s)";
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.powerLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.iqChirpDurationLabel);
            this.configurationGroupBox.Controls.Add(this.directDownloadCheckBox);
            this.configurationGroupBox.Controls.Add(this.powerLevelTypeComboBox);
            this.configurationGroupBox.Controls.Add(this.iqChirpDurationNumeric);
            this.configurationGroupBox.Controls.Add(this.startFrequencyLabel);
            this.configurationGroupBox.Controls.Add(this.iqRateNumeric);
            this.configurationGroupBox.Controls.Add(this.stopFrequencyLabel);
            this.configurationGroupBox.Controls.Add(this.stopFrequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.directDownloadLabel);
            this.configurationGroupBox.Controls.Add(this.startFrequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.powerLevelLabel);
            this.configurationGroupBox.Controls.Add(this.powerLevelTypeLabel);
            this.configurationGroupBox.Controls.Add(this.iqRateLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(28, 74);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(297, 270);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualIQChirpDurationTextBox);
            this.measurementGroupBox.Controls.Add(this.generationStatusProgressBar);
            this.measurementGroupBox.Controls.Add(this.actualIQRateTextBox);
            this.measurementGroupBox.Controls.Add(this.actualIQNumberOfSamplesTextBox);
            this.measurementGroupBox.Controls.Add(this.generationStatusLabel);
            this.measurementGroupBox.Controls.Add(this.actualIQRateLabel);
            this.measurementGroupBox.Controls.Add(this.actualIQChirpDurationLabel);
            this.measurementGroupBox.Controls.Add(this.actualIQNumberOfSamplesLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(376, 74);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(175, 270);
            this.measurementGroupBox.TabIndex = 0;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(576, 460);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.generatingLabel);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.generatingLed);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Large Arbitrary Waveform Generation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqChirpDurationNumeric)).EndInit();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label startFrequencyLabel;
        private System.Windows.Forms.Label stopFrequencyLabel;
        private System.Windows.Forms.Label directDownloadLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label iqChirpDurationLabel;
        private System.Windows.Forms.Label powerLevelTypeLabel;
        private System.Windows.Forms.Label actualIQNumberOfSamplesLabel;
        private System.Windows.Forms.Label actualIQRateLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label generatingLabel;
        private System.Windows.Forms.Label generationStatusLabel;
        private System.Windows.Forms.NumericUpDown startFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown stopFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.NumericUpDown iqChirpDurationNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox actualIQNumberOfSamplesTextBox;
        private System.Windows.Forms.TextBox actualIQChirpDurationTextBox;
        private System.Windows.Forms.TextBox actualIQRateTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;        private System.Windows.Forms.ComboBox powerLevelTypeComboBox;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.Button generatingLed;
        private System.Windows.Forms.ProgressBar generationStatusProgressBar;
        private System.Windows.Forms.CheckBox directDownloadCheckBox;
        private System.Windows.Forms.Label actualIQChirpDurationLabel;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;

    }
}
