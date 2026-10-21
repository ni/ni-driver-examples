namespace NationalInstruments.Examples.MultitoneUniformSpacing
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
            this.centerFrequencyLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.actualIQRateLabel = new System.Windows.Forms.Label();
            this.numberOfTonesLabel = new System.Windows.Forms.Label();
            this.initialPhaseLabel = new System.Windows.Forms.Label();
            this.powerPerToneLabel = new System.Windows.Forms.Label();
            this.maximumNumberOfSamplesLabel = new System.Windows.Forms.Label();
            this.frequencyBetweenTonesLabel = new System.Windows.Forms.Label();
            this.actualPeakPowerLabel = new System.Windows.Forms.Label();
            this.actualFrequencyBetweenTonesLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.centerFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.numberOfTonesNumeric = new System.Windows.Forms.NumericUpDown();
            this.initialPhaseNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerPerToneNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumNumberOfSamplesNumeric = new System.Windows.Forms.NumericUpDown();
            this.frequencyBetweenTonesNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.actualIQRateTextBox = new System.Windows.Forms.TextBox();
            this.actualPeakPowerTextBox = new System.Windows.Forms.TextBox();
            this.actualFrequencyBetweenTonesTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();            this.startButton = new System.Windows.Forms.Button();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.stopButton = new System.Windows.Forms.Button();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.messageMultitoneParamsGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.centerFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfTonesNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialPhaseNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerPerToneNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumNumberOfSamplesNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyBetweenTonesNumeric)).BeginInit();
            this.measurementGroupBox.SuspendLayout();
            this.messageMultitoneParamsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(21, 39);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // centerFrequencyLabel
            // 
            this.centerFrequencyLabel.AutoSize = true;
            this.centerFrequencyLabel.Location = new System.Drawing.Point(21, 88);
            this.centerFrequencyLabel.Name = "centerFrequencyLabel";
            this.centerFrequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.centerFrequencyLabel.TabIndex = 1;
            this.centerFrequencyLabel.Text = "Center Frequency (Hz)";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(20, 138);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 2;
            this.iqRateLabel.Text = "IQ Rate (S/s)";
            // 
            // actualIQRateLabel
            // 
            this.actualIQRateLabel.AutoSize = true;
            this.actualIQRateLabel.Location = new System.Drawing.Point(21, 24);
            this.actualIQRateLabel.Name = "actualIQRateLabel";
            this.actualIQRateLabel.Size = new System.Drawing.Size(103, 13);
            this.actualIQRateLabel.TabIndex = 3;
            this.actualIQRateLabel.Text = "Actual IQ Rate (S/s)";
            // 
            // numberOfTonesLabel
            // 
            this.numberOfTonesLabel.AutoSize = true;
            this.numberOfTonesLabel.Location = new System.Drawing.Point(14, 22);
            this.numberOfTonesLabel.Name = "numberOfTonesLabel";
            this.numberOfTonesLabel.Size = new System.Drawing.Size(89, 13);
            this.numberOfTonesLabel.TabIndex = 4;
            this.numberOfTonesLabel.Text = "Number of Tones";
            // 
            // initialPhaseLabel
            // 
            this.initialPhaseLabel.AutoSize = true;
            this.initialPhaseLabel.Location = new System.Drawing.Point(184, 23);
            this.initialPhaseLabel.Name = "initialPhaseLabel";
            this.initialPhaseLabel.Size = new System.Drawing.Size(111, 13);
            this.initialPhaseLabel.TabIndex = 5;
            this.initialPhaseLabel.Text = "Initial Phase (degrees)";
            // 
            // powerPerToneLabel
            // 
            this.powerPerToneLabel.AutoSize = true;
            this.powerPerToneLabel.Location = new System.Drawing.Point(14, 70);
            this.powerPerToneLabel.Name = "powerPerToneLabel";
            this.powerPerToneLabel.Size = new System.Drawing.Size(142, 13);
            this.powerPerToneLabel.TabIndex = 6;
            this.powerPerToneLabel.Text = "Power Level per Tone (dBm)";
            // 
            // maximumNumberOfSamplesLabel
            // 
            this.maximumNumberOfSamplesLabel.AutoSize = true;
            this.maximumNumberOfSamplesLabel.Location = new System.Drawing.Point(185, 74);
            this.maximumNumberOfSamplesLabel.Name = "maximumNumberOfSamplesLabel";
            this.maximumNumberOfSamplesLabel.Size = new System.Drawing.Size(122, 13);
            this.maximumNumberOfSamplesLabel.TabIndex = 7;
            this.maximumNumberOfSamplesLabel.Text = "Max Number of Samples";
            // 
            // frequencyBetweenTonesLabel
            // 
            this.frequencyBetweenTonesLabel.AutoSize = true;
            this.frequencyBetweenTonesLabel.Location = new System.Drawing.Point(13, 120);
            this.frequencyBetweenTonesLabel.Name = "frequencyBetweenTonesLabel";
            this.frequencyBetweenTonesLabel.Size = new System.Drawing.Size(157, 13);
            this.frequencyBetweenTonesLabel.TabIndex = 8;
            this.frequencyBetweenTonesLabel.Text = "Frequency Between Tones (Hz)";
            // 
            // actualPeakPowerLabel
            // 
            this.actualPeakPowerLabel.AutoSize = true;
            this.actualPeakPowerLabel.Location = new System.Drawing.Point(152, 26);
            this.actualPeakPowerLabel.Name = "actualPeakPowerLabel";
            this.actualPeakPowerLabel.Size = new System.Drawing.Size(143, 13);
            this.actualPeakPowerLabel.TabIndex = 10;
            this.actualPeakPowerLabel.Text = "Peak Envelope Power (dBm)";
            // 
            // actualFrequencyBetweenTonesLabel
            // 
            this.actualFrequencyBetweenTonesLabel.AutoSize = true;
            this.actualFrequencyBetweenTonesLabel.Location = new System.Drawing.Point(334, 22);
            this.actualFrequencyBetweenTonesLabel.Name = "actualFrequencyBetweenTonesLabel";
            this.actualFrequencyBetweenTonesLabel.Size = new System.Drawing.Size(190, 13);
            this.actualFrequencyBetweenTonesLabel.TabIndex = 11;
            this.actualFrequencyBetweenTonesLabel.Text = "Actual Frequency Between Tones (Hz)";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(22, 323);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 12;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // centerFrequencyNumeric
            // 
            this.centerFrequencyNumeric.DecimalPlaces = 2;
            this.centerFrequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.centerFrequencyNumeric.Location = new System.Drawing.Point(21, 109);
            this.centerFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.centerFrequencyNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.centerFrequencyNumeric.Name = "centerFrequencyNumeric";
            this.centerFrequencyNumeric.Size = new System.Drawing.Size(120, 20);
            this.centerFrequencyNumeric.TabIndex = 1;
            this.centerFrequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // iqRateNumeric
            // 
            this.iqRateNumeric.DecimalPlaces = 2;
            this.iqRateNumeric.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.iqRateNumeric.Location = new System.Drawing.Point(21, 158);
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
            1000000,
            0,
            0,
            0});
            // 
            // numberOfTonesNumeric
            // 
            this.numberOfTonesNumeric.Location = new System.Drawing.Point(14, 43);
            this.numberOfTonesNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numberOfTonesNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfTonesNumeric.Name = "numberOfTonesNumeric";
            this.numberOfTonesNumeric.Size = new System.Drawing.Size(120, 20);
            this.numberOfTonesNumeric.TabIndex = 4;
            this.numberOfTonesNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // initialPhaseNumeric
            // 
            this.initialPhaseNumeric.DecimalPlaces = 2;
            this.initialPhaseNumeric.Location = new System.Drawing.Point(184, 43);
            this.initialPhaseNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.initialPhaseNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.initialPhaseNumeric.Name = "initialPhaseNumeric";
            this.initialPhaseNumeric.Size = new System.Drawing.Size(120, 20);
            this.initialPhaseNumeric.TabIndex = 5;
            // 
            // powerPerToneNumeric
            // 
            this.powerPerToneNumeric.DecimalPlaces = 2;
            this.powerPerToneNumeric.Location = new System.Drawing.Point(14, 91);
            this.powerPerToneNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.powerPerToneNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.powerPerToneNumeric.Name = "powerPerToneNumeric";
            this.powerPerToneNumeric.Size = new System.Drawing.Size(120, 20);
            this.powerPerToneNumeric.TabIndex = 6;
            this.powerPerToneNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // maximumNumberOfSamplesNumeric
            // 
            this.maximumNumberOfSamplesNumeric.DecimalPlaces = 2;
            this.maximumNumberOfSamplesNumeric.Location = new System.Drawing.Point(184, 91);
            this.maximumNumberOfSamplesNumeric.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.maximumNumberOfSamplesNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maximumNumberOfSamplesNumeric.Name = "maximumNumberOfSamplesNumeric";
            this.maximumNumberOfSamplesNumeric.Size = new System.Drawing.Size(120, 20);
            this.maximumNumberOfSamplesNumeric.TabIndex = 7;
            this.maximumNumberOfSamplesNumeric.Value = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            // 
            // frequencyBetweenTonesNumeric
            // 
            this.frequencyBetweenTonesNumeric.DecimalPlaces = 2;
            this.frequencyBetweenTonesNumeric.Location = new System.Drawing.Point(13, 141);
            this.frequencyBetweenTonesNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.frequencyBetweenTonesNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.frequencyBetweenTonesNumeric.Name = "frequencyBetweenTonesNumeric";
            this.frequencyBetweenTonesNumeric.Size = new System.Drawing.Size(120, 20);
            this.frequencyBetweenTonesNumeric.TabIndex = 8;
            this.frequencyBetweenTonesNumeric.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(21, 60);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // actualIQRateTextBox
            // 
            this.actualIQRateTextBox.Location = new System.Drawing.Point(24, 43);
            this.actualIQRateTextBox.Name = "actualIQRateTextBox";
            this.actualIQRateTextBox.ReadOnly = true;
            this.actualIQRateTextBox.Size = new System.Drawing.Size(76, 20);
            this.actualIQRateTextBox.TabIndex = 3;
            this.actualIQRateTextBox.TabStop = false;
            this.actualIQRateTextBox.Text = "1000000.00";
            // 
            // actualPeakPowerTextBox
            // 
            this.actualPeakPowerTextBox.Location = new System.Drawing.Point(155, 43);
            this.actualPeakPowerTextBox.Name = "actualPeakPowerTextBox";
            this.actualPeakPowerTextBox.ReadOnly = true;
            this.actualPeakPowerTextBox.Size = new System.Drawing.Size(132, 20);
            this.actualPeakPowerTextBox.TabIndex = 10;
            this.actualPeakPowerTextBox.TabStop = false;
            this.actualPeakPowerTextBox.Text = "0.00";
            // 
            // actualFrequencyBetweenTonesTextBox
            // 
            this.actualFrequencyBetweenTonesTextBox.Location = new System.Drawing.Point(337, 43);
            this.actualFrequencyBetweenTonesTextBox.Name = "actualFrequencyBetweenTonesTextBox";
            this.actualFrequencyBetweenTonesTextBox.ReadOnly = true;
            this.actualFrequencyBetweenTonesTextBox.Size = new System.Drawing.Size(131, 20);
            this.actualFrequencyBetweenTonesTextBox.TabIndex = 11;
            this.actualFrequencyBetweenTonesTextBox.TabStop = false;
            this.actualFrequencyBetweenTonesTextBox.Text = "0.00000000000000E+0";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(20, 344);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(538, 47);
            this.errorTextBox.TabIndex = 14;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No Error";
            //             // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(321, 397);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 13;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(402, 397);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 15;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualIQRateTextBox);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyBetweenTonesTextBox);
            this.measurementGroupBox.Controls.Add(this.actualPeakPowerTextBox);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyBetweenTonesLabel);
            this.measurementGroupBox.Controls.Add(this.actualPeakPowerLabel);
            this.measurementGroupBox.Controls.Add(this.actualIQRateLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(21, 209);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(537, 88);
            this.measurementGroupBox.TabIndex = 16;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // messageMultitoneParamsGroupBox
            // 
            this.messageMultitoneParamsGroupBox.Controls.Add(this.frequencyBetweenTonesNumeric);
            this.messageMultitoneParamsGroupBox.Controls.Add(this.maximumNumberOfSamplesNumeric);
            this.messageMultitoneParamsGroupBox.Controls.Add(this.powerPerToneNumeric);
            this.messageMultitoneParamsGroupBox.Controls.Add(this.initialPhaseNumeric);
            this.messageMultitoneParamsGroupBox.Controls.Add(this.numberOfTonesNumeric);
            this.messageMultitoneParamsGroupBox.Controls.Add(this.frequencyBetweenTonesLabel);
            this.messageMultitoneParamsGroupBox.Controls.Add(this.numberOfTonesLabel);
            this.messageMultitoneParamsGroupBox.Controls.Add(this.maximumNumberOfSamplesLabel);
            this.messageMultitoneParamsGroupBox.Controls.Add(this.initialPhaseLabel);
            this.messageMultitoneParamsGroupBox.Controls.Add(this.powerPerToneLabel);
            this.messageMultitoneParamsGroupBox.Location = new System.Drawing.Point(176, 12);
            this.messageMultitoneParamsGroupBox.Name = "messageMultitoneParamsGroupBox";
            this.messageMultitoneParamsGroupBox.Size = new System.Drawing.Size(323, 174);
            this.messageMultitoneParamsGroupBox.TabIndex = 3;
            this.messageMultitoneParamsGroupBox.TabStop = false;
            this.messageMultitoneParamsGroupBox.Text = "Multitone Parameters";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;            this.ClientSize = new System.Drawing.Size(586, 432);
            this.Controls.Add(this.messageMultitoneParamsGroupBox);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.centerFrequencyLabel);
            this.Controls.Add(this.iqRateLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.centerFrequencyNumeric);
            this.Controls.Add(this.iqRateNumeric);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.errorTextBox);            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Multitone Uniform Spacing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.centerFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfTonesNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.initialPhaseNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerPerToneNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumNumberOfSamplesNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyBetweenTonesNumeric)).EndInit();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.messageMultitoneParamsGroupBox.ResumeLayout(false);
            this.messageMultitoneParamsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label centerFrequencyLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label actualIQRateLabel;
        private System.Windows.Forms.Label numberOfTonesLabel;
        private System.Windows.Forms.Label initialPhaseLabel;
        private System.Windows.Forms.Label powerPerToneLabel;
        private System.Windows.Forms.Label maximumNumberOfSamplesLabel;
        private System.Windows.Forms.Label frequencyBetweenTonesLabel;
        private System.Windows.Forms.Label actualPeakPowerLabel;
        private System.Windows.Forms.Label actualFrequencyBetweenTonesLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.NumericUpDown centerFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.NumericUpDown numberOfTonesNumeric;
        private System.Windows.Forms.NumericUpDown initialPhaseNumeric;
        private System.Windows.Forms.NumericUpDown powerPerToneNumeric;
        private System.Windows.Forms.NumericUpDown maximumNumberOfSamplesNumeric;
        private System.Windows.Forms.NumericUpDown frequencyBetweenTonesNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox actualIQRateTextBox;
        private System.Windows.Forms.TextBox actualPeakPowerTextBox;
        private System.Windows.Forms.TextBox actualFrequencyBetweenTonesTextBox;
        private System.Windows.Forms.TextBox errorTextBox;        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox measurementGroupBox;
        private System.Windows.Forms.GroupBox messageMultitoneParamsGroupBox;

    }
}
