namespace NationalInstruments.Examples.MultipleArbitraryWaveforms
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
            this.updateGroupBox = new System.Windows.Forms.GroupBox();
            this.waveformLabel = new System.Windows.Forms.Label();
            this.updateButton = new System.Windows.Forms.Button();
            this.waveformComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.actualFrequencyOffsetLabel = new System.Windows.Forms.Label();
            this.actualIQRateLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.actualFrequencyOffsetTextBox = new System.Windows.Forms.TextBox();
            this.actualIQRateTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.updateGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            this.measurementGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // updateGroupBox
            // 
            this.updateGroupBox.Controls.Add(this.waveformLabel);
            this.updateGroupBox.Controls.Add(this.updateButton);
            this.updateGroupBox.Controls.Add(this.waveformComboBox);
            this.updateGroupBox.Location = new System.Drawing.Point(28, 264);
            this.updateGroupBox.Name = "updateGroupBox";
            this.updateGroupBox.Size = new System.Drawing.Size(349, 68);
            this.updateGroupBox.TabIndex = 2;
            this.updateGroupBox.TabStop = false;
            // 
            // waveformLabel
            // 
            this.waveformLabel.AutoSize = true;
            this.waveformLabel.Location = new System.Drawing.Point(23, 17);
            this.waveformLabel.Name = "waveformLabel";
            this.waveformLabel.Size = new System.Drawing.Size(101, 13);
            this.waveformLabel.TabIndex = 4;
            this.waveformLabel.Text = "Selected Waveform";
            // 
            // updateButton
            // 
            this.updateButton.Enabled = false;
            this.updateButton.Location = new System.Drawing.Point(253, 28);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 1;
            this.updateButton.Text = "&Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // waveformComboBox
            // 
            this.waveformComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.waveformComboBox.Location = new System.Drawing.Point(23, 38);
            this.waveformComboBox.Name = "waveformComboBox";
            this.waveformComboBox.Size = new System.Drawing.Size(120, 21);
            this.waveformComboBox.TabIndex = 0;
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
            this.frequencyLabel.Location = new System.Drawing.Point(23, 27);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.frequencyLabel.TabIndex = 1;
            this.frequencyLabel.Text = "Center Frequency [Hz]";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(23, 81);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 2;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(23, 133);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 3;
            this.iqRateLabel.Text = "IQ Rate [S/s]";
            // 
            // actualFrequencyOffsetLabel
            // 
            this.actualFrequencyOffsetLabel.AutoSize = true;
            this.actualFrequencyOffsetLabel.Location = new System.Drawing.Point(25, 30);
            this.actualFrequencyOffsetLabel.Name = "actualFrequencyOffsetLabel";
            this.actualFrequencyOffsetLabel.Size = new System.Drawing.Size(110, 13);
            this.actualFrequencyOffsetLabel.TabIndex = 5;
            this.actualFrequencyOffsetLabel.Text = "Frequency Offset [Hz]";
            // 
            // actualIQRateLabel
            // 
            this.actualIQRateLabel.AutoSize = true;
            this.actualIQRateLabel.Location = new System.Drawing.Point(25, 84);
            this.actualIQRateLabel.Name = "actualIQRateLabel";
            this.actualIQRateLabel.Size = new System.Drawing.Size(103, 13);
            this.actualIQRateLabel.TabIndex = 6;
            this.actualIQRateLabel.Text = "Actual IQ Rate [S/s]";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(28, 345);
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
            this.frequencyNumeric.Location = new System.Drawing.Point(23, 46);
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
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.DecimalPlaces = 2;
            this.powerLevelNumeric.Location = new System.Drawing.Point(23, 100);
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
            this.powerLevelNumeric.TabIndex = 2;
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
            this.iqRateNumeric.Location = new System.Drawing.Point(23, 152);
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
            1000000,
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
            // actualFrequencyOffsetTextBox
            // 
            this.actualFrequencyOffsetTextBox.Location = new System.Drawing.Point(24, 46);
            this.actualFrequencyOffsetTextBox.Name = "actualFrequencyOffsetTextBox";
            this.actualFrequencyOffsetTextBox.ReadOnly = true;
            this.actualFrequencyOffsetTextBox.Size = new System.Drawing.Size(119, 20);
            this.actualFrequencyOffsetTextBox.TabIndex = 10;
            this.actualFrequencyOffsetTextBox.TabStop = false;
            this.actualFrequencyOffsetTextBox.Text = "0.000000";
            // 
            // actualIQRateTextBox
            // 
            this.actualIQRateTextBox.Location = new System.Drawing.Point(24, 100);
            this.actualIQRateTextBox.Name = "actualIQRateTextBox";
            this.actualIQRateTextBox.ReadOnly = true;
            this.actualIQRateTextBox.Size = new System.Drawing.Size(119, 20);
            this.actualIQRateTextBox.TabIndex = 11;
            this.actualIQRateTextBox.TabStop = false;
            this.actualIQRateTextBox.Text = "0.000000";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(28, 366);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(349, 34);
            this.errorTextBox.TabIndex = 12;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(213, 33);
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
            this.stopButton.Location = new System.Drawing.Point(302, 33);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.frequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.iqRateNumeric);
            this.configurationGroupBox.Controls.Add(this.powerLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.iqRateLabel);
            this.configurationGroupBox.Controls.Add(this.frequencyLabel);
            this.configurationGroupBox.Controls.Add(this.powerLevelLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(28, 77);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(166, 182);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualIQRateLabel);
            this.measurementGroupBox.Controls.Add(this.actualIQRateTextBox);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyOffsetTextBox);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyOffsetLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(213, 77);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(164, 138);
            this.measurementGroupBox.TabIndex = 0;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(394, 441);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.updateGroupBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Multiple Arbitrary Waveforms";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.updateGroupBox.ResumeLayout(false);
            this.updateGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.GroupBox updateGroupBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label waveformLabel;
        private System.Windows.Forms.Label actualFrequencyOffsetLabel;
        private System.Windows.Forms.Label actualIQRateLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox actualFrequencyOffsetTextBox;
        private System.Windows.Forms.TextBox actualIQRateTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button updateButton;        private System.Windows.Forms.ComboBox waveformComboBox;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;

    }
}
