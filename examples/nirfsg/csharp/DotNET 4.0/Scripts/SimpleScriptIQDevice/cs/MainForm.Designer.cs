namespace NationalInstruments.Examples.SimpleScriptIQDevice
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
            this.actualIQRateTextBox = new System.Windows.Forms.TextBox();
            this.actualFrequencyOffsetTextBox = new System.Windows.Forms.TextBox();
            this.actualIQRateLabel = new System.Windows.Forms.Label();
            this.actualFrequencyOffsetLabel = new System.Windows.Forms.Label();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.referenceClockComboBox = new System.Windows.Forms.ComboBox();
            this.referenceClockLabel = new System.Windows.Forms.Label();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqOutPortLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqPortFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqPortFrequencyLabel = new System.Windows.Forms.Label();
            this.iqOutPortLevelLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.scriptLabel = new System.Windows.Forms.Label();
            this.textmessageLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.scriptTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.measurementGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqOutPortLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqPortFrequencyNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualIQRateTextBox);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyOffsetTextBox);
            this.measurementGroupBox.Controls.Add(this.actualIQRateLabel);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyOffsetLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(337, 197);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(154, 131);
            this.measurementGroupBox.TabIndex = 15;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // actualIQRateTextBox
            // 
            this.actualIQRateTextBox.Location = new System.Drawing.Point(21, 101);
            this.actualIQRateTextBox.Name = "actualIQRateTextBox";
            this.actualIQRateTextBox.ReadOnly = true;
            this.actualIQRateTextBox.Size = new System.Drawing.Size(109, 20);
            this.actualIQRateTextBox.TabIndex = 11;
            this.actualIQRateTextBox.TabStop = false;
            this.actualIQRateTextBox.Text = "0.000000";
            // 
            // actualFrequencyOffsetTextBox
            // 
            this.actualFrequencyOffsetTextBox.Location = new System.Drawing.Point(21, 46);
            this.actualFrequencyOffsetTextBox.Name = "actualFrequencyOffsetTextBox";
            this.actualFrequencyOffsetTextBox.ReadOnly = true;
            this.actualFrequencyOffsetTextBox.Size = new System.Drawing.Size(109, 20);
            this.actualFrequencyOffsetTextBox.TabIndex = 10;
            this.actualFrequencyOffsetTextBox.TabStop = false;
            this.actualFrequencyOffsetTextBox.Text = "0.000000";
            // 
            // actualIQRateLabel
            // 
            this.actualIQRateLabel.AutoSize = true;
            this.actualIQRateLabel.Location = new System.Drawing.Point(21, 80);
            this.actualIQRateLabel.Name = "actualIQRateLabel";
            this.actualIQRateLabel.Size = new System.Drawing.Size(103, 13);
            this.actualIQRateLabel.TabIndex = 7;
            this.actualIQRateLabel.Text = "Actual IQ Rate [S/s]";
            // 
            // actualFrequencyOffsetLabel
            // 
            this.actualFrequencyOffsetLabel.AutoSize = true;
            this.actualFrequencyOffsetLabel.Location = new System.Drawing.Point(21, 25);
            this.actualFrequencyOffsetLabel.Name = "actualFrequencyOffsetLabel";
            this.actualFrequencyOffsetLabel.Size = new System.Drawing.Size(110, 13);
            this.actualFrequencyOffsetLabel.TabIndex = 6;
            this.actualFrequencyOffsetLabel.Text = "Frequency Offset [Hz]";
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.referenceClockComboBox);
            this.configurationGroupBox.Controls.Add(this.referenceClockLabel);
            this.configurationGroupBox.Controls.Add(this.iqRateNumeric);
            this.configurationGroupBox.Controls.Add(this.iqOutPortLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.iqPortFrequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.iqPortFrequencyLabel);
            this.configurationGroupBox.Controls.Add(this.iqOutPortLevelLabel);
            this.configurationGroupBox.Controls.Add(this.iqRateLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 197);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(309, 131);
            this.configurationGroupBox.TabIndex = 2;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // referenceClockComboBox
            // 
            this.referenceClockComboBox.Location = new System.Drawing.Point(21, 47);
            this.referenceClockComboBox.Name = "referenceClockComboBox";
            this.referenceClockComboBox.Size = new System.Drawing.Size(110, 21);
            this.referenceClockComboBox.TabIndex = 1;
            // 
            // referenceClockLabel
            // 
            this.referenceClockLabel.AutoSize = true;
            this.referenceClockLabel.Location = new System.Drawing.Point(21, 26);
            this.referenceClockLabel.Name = "referenceClockLabel";
            this.referenceClockLabel.Size = new System.Drawing.Size(71, 13);
            this.referenceClockLabel.TabIndex = 30;
            this.referenceClockLabel.Text = "Clock Source";
            // 
            // iqRateNumeric
            // 
            this.iqRateNumeric.DecimalPlaces = 6;
            this.iqRateNumeric.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.iqRateNumeric.Location = new System.Drawing.Point(168, 100);
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
            5000000,
            0,
            0,
            0});
            // 
            // iqOutPortLevelNumeric
            // 
            this.iqOutPortLevelNumeric.DecimalPlaces = 3;
            this.iqOutPortLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.iqOutPortLevelNumeric.Location = new System.Drawing.Point(168, 47);
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
            this.iqOutPortLevelNumeric.TabIndex = 3;
            this.iqOutPortLevelNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // iqPortFrequencyNumeric
            // 
            this.iqPortFrequencyNumeric.DecimalPlaces = 6;
            this.iqPortFrequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.iqPortFrequencyNumeric.Location = new System.Drawing.Point(21, 100);
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
            this.iqPortFrequencyNumeric.TabIndex = 2;
            // 
            // iqPortFrequencyLabel
            // 
            this.iqPortFrequencyLabel.AutoSize = true;
            this.iqPortFrequencyLabel.Location = new System.Drawing.Point(21, 79);
            this.iqPortFrequencyLabel.Name = "iqPortFrequencyLabel";
            this.iqPortFrequencyLabel.Size = new System.Drawing.Size(115, 13);
            this.iqPortFrequencyLabel.TabIndex = 2;
            this.iqPortFrequencyLabel.Text = "IQ Port Frequency (Hz)";
            // 
            // iqOutPortLevelLabel
            // 
            this.iqOutPortLevelLabel.AutoSize = true;
            this.iqOutPortLevelLabel.Location = new System.Drawing.Point(168, 26);
            this.iqOutPortLevelLabel.Name = "iqOutPortLevelLabel";
            this.iqOutPortLevelLabel.Size = new System.Drawing.Size(117, 13);
            this.iqOutPortLevelLabel.TabIndex = 3;
            this.iqOutPortLevelLabel.Text = "IQ Out Port Level (Vpp)";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(168, 79);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 4;
            this.iqRateLabel.Text = "IQ Rate [S/s]";
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
            // scriptLabel
            // 
            this.scriptLabel.AutoSize = true;
            this.scriptLabel.Location = new System.Drawing.Point(12, 64);
            this.scriptLabel.Name = "scriptLabel";
            this.scriptLabel.Size = new System.Drawing.Size(34, 13);
            this.scriptLabel.TabIndex = 17;
            this.scriptLabel.Text = "Script";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(12, 344);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 22;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 30);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // scriptTextBox
            // 
            this.scriptTextBox.Location = new System.Drawing.Point(12, 86);
            this.scriptTextBox.Multiline = true;
            this.scriptTextBox.Name = "scriptTextBox";
            this.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.scriptTextBox.Size = new System.Drawing.Size(309, 91);
            this.scriptTextBox.TabIndex = 1;
            this.scriptTextBox.Text = "script simple\r\n    repeat forever\r\n       generate positiveOffset\r\n       generat" +
    "e negativeOffset\r\n    end repeat\r\n end script";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(12, 365);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(478, 34);
            this.errorTextBox.TabIndex = 23;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(337, 28);
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
            this.stopButton.Location = new System.Drawing.Point(415, 28);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 449);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.scriptLabel);
            this.Controls.Add(this.textmessageLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.scriptTextBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Simple Script IQ Device";
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqOutPortLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqPortFrequencyNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox measurementGroupBox;
        private System.Windows.Forms.TextBox actualIQRateTextBox;
        private System.Windows.Forms.TextBox actualFrequencyOffsetTextBox;
        private System.Windows.Forms.Label actualIQRateLabel;
        private System.Windows.Forms.Label actualFrequencyOffsetLabel;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.NumericUpDown iqOutPortLevelNumeric;
        private System.Windows.Forms.NumericUpDown iqPortFrequencyNumeric;
        private System.Windows.Forms.Label iqPortFrequencyLabel;
        private System.Windows.Forms.Label iqOutPortLevelLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label scriptLabel;
        private System.Windows.Forms.Label textmessageLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox scriptTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.ComboBox referenceClockComboBox;
        private System.Windows.Forms.Label referenceClockLabel;

    }
}

