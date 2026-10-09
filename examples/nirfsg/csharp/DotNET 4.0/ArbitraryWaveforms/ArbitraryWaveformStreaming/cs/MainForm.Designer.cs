namespace NationalInstruments.Examples.ArbitraryWaveformStreaming
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
            this.streamingSizeLabel = new System.Windows.Forms.Label();
            this.blockSizeLabel = new System.Windows.Forms.Label();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.preGainLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.actualIQRateLabel = new System.Windows.Forms.Label();
            this.generatingLabel = new System.Windows.Forms.Label();
            this.streamingSizeNumeric = new System.Windows.Forms.NumericUpDown();
            this.blockSizeNumeric = new System.Windows.Forms.NumericUpDown();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.preGainNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.actualIQRateTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.generatingLed = new System.Windows.Forms.Button();
            this.pathLabel = new System.Windows.Forms.Label();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.streamingSizeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blockSizeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.preGainNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(12, 16);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // streamingSizeLabel
            // 
            this.streamingSizeLabel.AutoSize = true;
            this.streamingSizeLabel.Location = new System.Drawing.Point(161, 185);
            this.streamingSizeLabel.Name = "streamingSizeLabel";
            this.streamingSizeLabel.Size = new System.Drawing.Size(145, 13);
            this.streamingSizeLabel.TabIndex = 1;
            this.streamingSizeLabel.Text = "Streaming Waveform Size (S)";
            // 
            // blockSizeLabel
            // 
            this.blockSizeLabel.AutoSize = true;
            this.blockSizeLabel.Location = new System.Drawing.Point(161, 134);
            this.blockSizeLabel.Name = "blockSizeLabel";
            this.blockSizeLabel.Size = new System.Drawing.Size(101, 13);
            this.blockSizeLabel.TabIndex = 2;
            this.blockSizeLabel.Text = "Write Block Size (S)";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.AutoSize = true;
            this.frequencyLabel.Location = new System.Drawing.Point(12, 79);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.frequencyLabel.TabIndex = 2;
            this.frequencyLabel.Text = "Center Frequency [Hz]";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(12, 134);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 4;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // preGainLabel
            // 
            this.preGainLabel.AutoSize = true;
            this.preGainLabel.Location = new System.Drawing.Point(12, 185);
            this.preGainLabel.Name = "preGainLabel";
            this.preGainLabel.Size = new System.Drawing.Size(92, 13);
            this.preGainLabel.TabIndex = 5;
            this.preGainLabel.Text = "Pre-filter Gain [dB]";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(12, 236);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 6;
            this.iqRateLabel.Text = "IQ Rate [S/s]";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(12, 301);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 7;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // actualIQRateLabel
            // 
            this.actualIQRateLabel.AutoSize = true;
            this.actualIQRateLabel.Location = new System.Drawing.Point(332, 133);
            this.actualIQRateLabel.Name = "actualIQRateLabel";
            this.actualIQRateLabel.Size = new System.Drawing.Size(103, 13);
            this.actualIQRateLabel.TabIndex = 8;
            this.actualIQRateLabel.Text = "Actual IQ Rate [S/s]";
            // 
            // generatingLabel
            // 
            this.generatingLabel.AutoSize = true;
            this.generatingLabel.Location = new System.Drawing.Point(195, 40);
            this.generatingLabel.Name = "generatingLabel";
            this.generatingLabel.Size = new System.Drawing.Size(59, 13);
            this.generatingLabel.TabIndex = 9;
            this.generatingLabel.Text = "Generating";
            // 
            // streamingSizeNumeric
            // 
            this.streamingSizeNumeric.Location = new System.Drawing.Point(161, 206);
            this.streamingSizeNumeric.Maximum = new decimal(new int[] {
            2147483646,
            0,
            0,
            0});
            this.streamingSizeNumeric.Name = "streamingSizeNumeric";
            this.streamingSizeNumeric.Size = new System.Drawing.Size(120, 20);
            this.streamingSizeNumeric.TabIndex = 8;
            this.streamingSizeNumeric.Value = new decimal(new int[] {
            4000000,
            0,
            0,
            0});
            // 
            // blockSizeNumeric
            // 
            this.blockSizeNumeric.Location = new System.Drawing.Point(161, 155);
            this.blockSizeNumeric.Maximum = new decimal(new int[] {
            2147483646,
            0,
            0,
            0});
            this.blockSizeNumeric.Name = "blockSizeNumeric";
            this.blockSizeNumeric.Size = new System.Drawing.Size(120, 20);
            this.blockSizeNumeric.TabIndex = 7;
            this.blockSizeNumeric.Value = new decimal(new int[] {
            250000,
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
            this.frequencyNumeric.Location = new System.Drawing.Point(12, 100);
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
            this.powerLevelNumeric.DecimalPlaces = 1;
            this.powerLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.powerLevelNumeric.Location = new System.Drawing.Point(12, 155);
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
            // preGainNumeric
            // 
            this.preGainNumeric.DecimalPlaces = 1;
            this.preGainNumeric.Location = new System.Drawing.Point(12, 206);
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
            this.preGainNumeric.TabIndex = 3;
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
            this.iqRateNumeric.Location = new System.Drawing.Point(12, 257);
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
            1000000,
            0,
            0,
            0});
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 37);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(12, 317);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(466, 77);
            this.errorTextBox.TabIndex = 10;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // actualIQRateTextBox
            // 
            this.actualIQRateTextBox.Location = new System.Drawing.Point(332, 154);
            this.actualIQRateTextBox.Name = "actualIQRateTextBox";
            this.actualIQRateTextBox.ReadOnly = true;
            this.actualIQRateTextBox.Size = new System.Drawing.Size(113, 20);
            this.actualIQRateTextBox.TabIndex = 14;
            this.actualIQRateTextBox.TabStop = false;
            this.actualIQRateTextBox.Text = "8333333.000000";
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(453, 97);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(25, 23);
            this.browseButton.TabIndex = 6;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(321, 37);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 9;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(403, 37);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 10;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // generatingLed
            // 
            this.generatingLed.BackColor = System.Drawing.SystemColors.Control;
            this.generatingLed.Enabled = false;
            this.generatingLed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.generatingLed.Location = new System.Drawing.Point(171, 37);
            this.generatingLed.Name = "generatingLed";
            this.generatingLed.Size = new System.Drawing.Size(20, 20);
            this.generatingLed.TabIndex = 15;
            this.generatingLed.TabStop = false;
            this.generatingLed.UseVisualStyleBackColor = false;
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(160, 78);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(112, 13);
            this.pathLabel.TabIndex = 17;
            this.pathLabel.Text = "Path to Waveform File";
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(161, 99);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(284, 20);
            this.pathTextBox.TabIndex = 5;
            this.pathTextBox.Text = "..\\..\\ChirpWaveform.bin";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(488, 444);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.streamingSizeLabel);
            this.Controls.Add(this.blockSizeLabel);
            this.Controls.Add(this.frequencyLabel);
            this.Controls.Add(this.powerLevelLabel);
            this.Controls.Add(this.preGainLabel);
            this.Controls.Add(this.iqRateLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.actualIQRateLabel);
            this.Controls.Add(this.generatingLabel);
            this.Controls.Add(this.streamingSizeNumeric);
            this.Controls.Add(this.blockSizeNumeric);
            this.Controls.Add(this.frequencyNumeric);
            this.Controls.Add(this.powerLevelNumeric);
            this.Controls.Add(this.preGainNumeric);
            this.Controls.Add(this.iqRateNumeric);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.actualIQRateTextBox);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.generatingLed);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Arbitrary Waveform Streaming";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.streamingSizeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blockSizeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.preGainNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label streamingSizeLabel;
        private System.Windows.Forms.Label blockSizeLabel;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label preGainLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label actualIQRateLabel;
        private System.Windows.Forms.Label generatingLabel;
        private System.Windows.Forms.NumericUpDown streamingSizeNumeric;
        private System.Windows.Forms.NumericUpDown blockSizeNumeric;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown preGainNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.TextBox actualIQRateTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;        private System.Windows.Forms.Button generatingLed;
        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.TextBox pathTextBox;

    }
}
