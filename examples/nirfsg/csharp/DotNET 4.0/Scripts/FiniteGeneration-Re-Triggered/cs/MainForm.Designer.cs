namespace NationalInstruments.Examples.FiniteGenerationReTriggered
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
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.durationLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.actualDurationLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.actualIQRateLabel = new System.Windows.Forms.Label();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.durationNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.actualDurationTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.actualIQRateTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.triggerButton = new System.Windows.Forms.Button();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.readyLabel = new System.Windows.Forms.Label();
            this.readyLed = new System.Windows.Forms.Button();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.durationNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            this.measurementGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(19, 18);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.AutoSize = true;
            this.frequencyLabel.Location = new System.Drawing.Point(20, 26);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.frequencyLabel.TabIndex = 1;
            this.frequencyLabel.Text = "Center Frequency [Hz]";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(20, 77);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 2;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // durationLabel
            // 
            this.durationLabel.AutoSize = true;
            this.durationLabel.Location = new System.Drawing.Point(20, 130);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(113, 13);
            this.durationLabel.TabIndex = 3;
            this.durationLabel.Text = "Waveform Duration [s]";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(20, 180);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 4;
            this.iqRateLabel.Text = "IQ Rate [S/s]";
            // 
            // actualDurationLabel
            // 
            this.actualDurationLabel.AutoSize = true;
            this.actualDurationLabel.Location = new System.Drawing.Point(23, 28);
            this.actualDurationLabel.Name = "actualDurationLabel";
            this.actualDurationLabel.Size = new System.Drawing.Size(146, 13);
            this.actualDurationLabel.TabIndex = 5;
            this.actualDurationLabel.Text = "Actual Waveform Duration [s]";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(19, 323);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 6;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // actualIQRateLabel
            // 
            this.actualIQRateLabel.AutoSize = true;
            this.actualIQRateLabel.Location = new System.Drawing.Point(23, 78);
            this.actualIQRateLabel.Name = "actualIQRateLabel";
            this.actualIQRateLabel.Size = new System.Drawing.Size(103, 13);
            this.actualIQRateLabel.TabIndex = 7;
            this.actualIQRateLabel.Text = "Actual IQ Rate [S/s]";
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.DecimalPlaces = 6;
            this.frequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.frequencyNumeric.Location = new System.Drawing.Point(20, 47);
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
            this.powerLevelNumeric.Location = new System.Drawing.Point(20, 98);
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
            // durationNumeric
            // 
            this.durationNumeric.DecimalPlaces = 6;
            this.durationNumeric.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.durationNumeric.Location = new System.Drawing.Point(20, 151);
            this.durationNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.durationNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.durationNumeric.Name = "durationNumeric";
            this.durationNumeric.Size = new System.Drawing.Size(120, 20);
            this.durationNumeric.TabIndex = 3;
            this.durationNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // iqRateNumeric
            // 
            this.iqRateNumeric.DecimalPlaces = 6;
            this.iqRateNumeric.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.iqRateNumeric.Location = new System.Drawing.Point(20, 201);
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
            this.resourceNameComboBox.Location = new System.Drawing.Point(19, 39);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // actualDurationTextBox
            // 
            this.actualDurationTextBox.Location = new System.Drawing.Point(23, 49);
            this.actualDurationTextBox.Name = "actualDurationTextBox";
            this.actualDurationTextBox.ReadOnly = true;
            this.actualDurationTextBox.Size = new System.Drawing.Size(151, 20);
            this.actualDurationTextBox.TabIndex = 11;
            this.actualDurationTextBox.TabStop = false;
            this.actualDurationTextBox.Text = "0.000000";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(19, 339);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(377, 48);
            this.errorTextBox.TabIndex = 12;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // actualIQRateTextBox
            // 
            this.actualIQRateTextBox.Location = new System.Drawing.Point(23, 99);
            this.actualIQRateTextBox.Name = "actualIQRateTextBox";
            this.actualIQRateTextBox.ReadOnly = true;
            this.actualIQRateTextBox.Size = new System.Drawing.Size(151, 20);
            this.actualIQRateTextBox.TabIndex = 13;
            this.actualIQRateTextBox.TabStop = false;
            this.actualIQRateTextBox.Text = "0.000000";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(241, 37);
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
            this.stopButton.Location = new System.Drawing.Point(322, 37);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 5;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // triggerButton
            // 
            this.triggerButton.Enabled = false;
            this.triggerButton.Location = new System.Drawing.Point(241, 79);
            this.triggerButton.Name = "triggerButton";
            this.triggerButton.Size = new System.Drawing.Size(156, 23);
            this.triggerButton.TabIndex = 3;
            this.triggerButton.Text = "&Send Software scriptTrigger0";
            this.triggerButton.UseVisualStyleBackColor = true;
            this.triggerButton.Click += new System.EventHandler(this.triggerButton_Click);
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Interval = 1;
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // readyLabel
            // 
            this.readyLabel.AutoSize = true;
            this.readyLabel.Location = new System.Drawing.Point(267, 128);
            this.readyLabel.Name = "readyLabel";
            this.readyLabel.Size = new System.Drawing.Size(89, 13);
            this.readyLabel.TabIndex = 16;
            this.readyLabel.Text = "Ready for Trigger";
            // 
            // readyLed
            // 
            this.readyLed.BackColor = System.Drawing.SystemColors.Control;
            this.readyLed.Enabled = false;
            this.readyLed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.readyLed.Location = new System.Drawing.Point(241, 124);
            this.readyLed.Name = "readyLed";
            this.readyLed.Size = new System.Drawing.Size(20, 20);
            this.readyLed.TabIndex = 17;
            this.readyLed.TabStop = false;
            this.readyLed.UseVisualStyleBackColor = false;
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.iqRateNumeric);
            this.configurationGroupBox.Controls.Add(this.durationNumeric);
            this.configurationGroupBox.Controls.Add(this.powerLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.frequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.iqRateLabel);
            this.configurationGroupBox.Controls.Add(this.frequencyLabel);
            this.configurationGroupBox.Controls.Add(this.durationLabel);
            this.configurationGroupBox.Controls.Add(this.powerLevelLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(19, 79);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(166, 229);
            this.configurationGroupBox.TabIndex = 2;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualIQRateTextBox);
            this.measurementGroupBox.Controls.Add(this.actualDurationTextBox);
            this.measurementGroupBox.Controls.Add(this.actualIQRateLabel);
            this.measurementGroupBox.Controls.Add(this.actualDurationLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(200, 180);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(197, 128);
            this.measurementGroupBox.TabIndex = 0;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 426);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.readyLabel);
            this.Controls.Add(this.readyLed);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.triggerButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Finite Generation - Re-Triggered";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.durationNumeric)).EndInit();
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
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label durationLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label actualDurationLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label actualIQRateLabel;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown durationNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox actualDurationTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.TextBox actualIQRateTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button triggerButton;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.Label readyLabel;
        private System.Windows.Forms.Button readyLed;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;

    }
}
