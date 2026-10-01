namespace NationalInstruments.Examples.ExternalAWG5611
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
            this.centerFrequencyLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.loGroupBox = new System.Windows.Forms.GroupBox();
            this.outputTerminalLabel = new System.Windows.Forms.Label();
            this.loSwitchLabel = new System.Windows.Forms.Label();
            this.outputTerminalComboBox = new System.Windows.Forms.ComboBox();
            this.frequencyReferenceSourceLabel = new System.Windows.Forms.Label();
            this.frequencyReferenceSourceComboBox = new System.Windows.Forms.ComboBox();
            this.loSwitchComboBox = new System.Windows.Forms.ComboBox();
            this.actualGainLabel = new System.Windows.Forms.Label();
            this.actualSkewLabel = new System.Windows.Forms.Label();
            this.actualGainImbalanceLabel = new System.Windows.Forms.Label();
            this.actualQOffsetLabel = new System.Windows.Forms.Label();
            this.actualIOffsetLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.centerFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.actualGainTextBox = new System.Windows.Forms.TextBox();
            this.actualSkewTextBox = new System.Windows.Forms.TextBox();
            this.actualGainImbalanceTextBox = new System.Windows.Forms.TextBox();
            this.actualQOffsetTextBox = new System.Windows.Forms.TextBox();
            this.actualIOffsetTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.loGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.centerFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            this.measurementGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // centerFrequencyLabel
            // 
            this.centerFrequencyLabel.AutoSize = true;
            this.centerFrequencyLabel.Location = new System.Drawing.Point(16, 67);
            this.centerFrequencyLabel.Name = "centerFrequencyLabel";
            this.centerFrequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.centerFrequencyLabel.TabIndex = 0;
            this.centerFrequencyLabel.Text = "Center Frequency (Hz)";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(16, 120);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(51, 13);
            this.powerLevelLabel.TabIndex = 1;
            this.powerLevelLabel.Text = "Gain (dB)";
            // 
            // loGroupBox
            // 
            this.loGroupBox.Controls.Add(this.outputTerminalLabel);
            this.loGroupBox.Controls.Add(this.loSwitchLabel);
            this.loGroupBox.Controls.Add(this.outputTerminalComboBox);
            this.loGroupBox.Controls.Add(this.frequencyReferenceSourceLabel);
            this.loGroupBox.Controls.Add(this.frequencyReferenceSourceComboBox);
            this.loGroupBox.Controls.Add(this.loSwitchComboBox);
            this.loGroupBox.Location = new System.Drawing.Point(17, 173);
            this.loGroupBox.Name = "loGroupBox";
            this.loGroupBox.Size = new System.Drawing.Size(215, 172);
            this.loGroupBox.TabIndex = 3;
            this.loGroupBox.TabStop = false;
            this.loGroupBox.Text = "LO Parameters";
            // 
            // outputTerminalLabel
            // 
            this.outputTerminalLabel.AutoSize = true;
            this.outputTerminalLabel.Location = new System.Drawing.Point(17, 117);
            this.outputTerminalLabel.Name = "outputTerminalLabel";
            this.outputTerminalLabel.Size = new System.Drawing.Size(188, 13);
            this.outputTerminalLabel.TabIndex = 18;
            this.outputTerminalLabel.Text = "Frequency Reference Output Terminal";
            // 
            // loSwitchLabel
            // 
            this.loSwitchLabel.AutoSize = true;
            this.loSwitchLabel.Location = new System.Drawing.Point(19, 16);
            this.loSwitchLabel.Name = "loSwitchLabel";
            this.loSwitchLabel.Size = new System.Drawing.Size(21, 13);
            this.loSwitchLabel.TabIndex = 14;
            this.loSwitchLabel.Text = "LO";
            // 
            // outputTerminalComboBox
            // 
            this.outputTerminalComboBox.FormattingEnabled = true;
            this.outputTerminalComboBox.Location = new System.Drawing.Point(17, 138);
            this.outputTerminalComboBox.Name = "outputTerminalComboBox";
            this.outputTerminalComboBox.Size = new System.Drawing.Size(120, 21);
            this.outputTerminalComboBox.TabIndex = 15;
            // 
            // frequencyReferenceSourceLabel
            // 
            this.frequencyReferenceSourceLabel.AutoSize = true;
            this.frequencyReferenceSourceLabel.Location = new System.Drawing.Point(17, 66);
            this.frequencyReferenceSourceLabel.Name = "frequencyReferenceSourceLabel";
            this.frequencyReferenceSourceLabel.Size = new System.Drawing.Size(147, 13);
            this.frequencyReferenceSourceLabel.TabIndex = 16;
            this.frequencyReferenceSourceLabel.Text = "Frequency Reference Source";
            // 
            // frequencyReferenceSourceComboBox
            // 
            this.frequencyReferenceSourceComboBox.FormattingEnabled = true;
            this.frequencyReferenceSourceComboBox.Location = new System.Drawing.Point(17, 87);
            this.frequencyReferenceSourceComboBox.Name = "frequencyReferenceSourceComboBox";
            this.frequencyReferenceSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.frequencyReferenceSourceComboBox.TabIndex = 14;
            // 
            // loSwitchComboBox
            // 
            this.loSwitchComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loSwitchComboBox.FormattingEnabled = true;
            this.loSwitchComboBox.Location = new System.Drawing.Point(17, 37);
            this.loSwitchComboBox.Name = "loSwitchComboBox";
            this.loSwitchComboBox.Size = new System.Drawing.Size(120, 21);
            this.loSwitchComboBox.TabIndex = 13;
            this.loSwitchComboBox.SelectedIndexChanged += new System.EventHandler(this.loSwitchComboBox_SelectedIndexChanged);
            // 
            // actualGainLabel
            // 
            this.actualGainLabel.AutoSize = true;
            this.actualGainLabel.Location = new System.Drawing.Point(16, 25);
            this.actualGainLabel.Name = "actualGainLabel";
            this.actualGainLabel.Size = new System.Drawing.Size(84, 13);
            this.actualGainLabel.TabIndex = 2;
            this.actualGainLabel.Text = "Actual Gain (dB)";
            // 
            // actualSkewLabel
            // 
            this.actualSkewLabel.AutoSize = true;
            this.actualSkewLabel.Location = new System.Drawing.Point(116, 144);
            this.actualSkewLabel.Name = "actualSkewLabel";
            this.actualSkewLabel.Size = new System.Drawing.Size(61, 13);
            this.actualSkewLabel.TabIndex = 3;
            this.actualSkewLabel.Text = "Skew (deg)";
            // 
            // actualGainImbalanceLabel
            // 
            this.actualGainImbalanceLabel.AutoSize = true;
            this.actualGainImbalanceLabel.Location = new System.Drawing.Point(16, 144);
            this.actualGainImbalanceLabel.Name = "actualGainImbalanceLabel";
            this.actualGainImbalanceLabel.Size = new System.Drawing.Size(71, 13);
            this.actualGainImbalanceLabel.TabIndex = 4;
            this.actualGainImbalanceLabel.Text = "Gain Imb (dB)";
            // 
            // actualQOffsetLabel
            // 
            this.actualQOffsetLabel.AutoSize = true;
            this.actualQOffsetLabel.Location = new System.Drawing.Point(116, 94);
            this.actualQOffsetLabel.Name = "actualQOffsetLabel";
            this.actualQOffsetLabel.Size = new System.Drawing.Size(62, 13);
            this.actualQOffsetLabel.TabIndex = 5;
            this.actualQOffsetLabel.Text = "Q Offset (V)";
            // 
            // actualIOffsetLabel
            // 
            this.actualIOffsetLabel.AutoSize = true;
            this.actualIOffsetLabel.Location = new System.Drawing.Point(16, 94);
            this.actualIOffsetLabel.Name = "actualIOffsetLabel";
            this.actualIOffsetLabel.Size = new System.Drawing.Size(57, 13);
            this.actualIOffsetLabel.TabIndex = 6;
            this.actualIOffsetLabel.Text = "I Offset (V)";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(16, 367);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 7;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(16, 22);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 8;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // centerFrequencyNumeric
            // 
            this.centerFrequencyNumeric.DecimalPlaces = 6;
            this.centerFrequencyNumeric.Location = new System.Drawing.Point(17, 87);
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
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.DecimalPlaces = 2;
            this.powerLevelNumeric.Location = new System.Drawing.Point(17, 137);
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
            // actualGainTextBox
            // 
            this.actualGainTextBox.Location = new System.Drawing.Point(15, 42);
            this.actualGainTextBox.Name = "actualGainTextBox";
            this.actualGainTextBox.ReadOnly = true;
            this.actualGainTextBox.Size = new System.Drawing.Size(176, 20);
            this.actualGainTextBox.TabIndex = 2;
            this.actualGainTextBox.TabStop = false;
            this.actualGainTextBox.Text = "0.00";
            // 
            // actualSkewTextBox
            // 
            this.actualSkewTextBox.Location = new System.Drawing.Point(116, 165);
            this.actualSkewTextBox.Name = "actualSkewTextBox";
            this.actualSkewTextBox.ReadOnly = true;
            this.actualSkewTextBox.Size = new System.Drawing.Size(75, 20);
            this.actualSkewTextBox.TabIndex = 3;
            this.actualSkewTextBox.TabStop = false;
            this.actualSkewTextBox.Text = "0.00";
            // 
            // actualGainImbalanceTextBox
            // 
            this.actualGainImbalanceTextBox.Location = new System.Drawing.Point(16, 165);
            this.actualGainImbalanceTextBox.Name = "actualGainImbalanceTextBox";
            this.actualGainImbalanceTextBox.ReadOnly = true;
            this.actualGainImbalanceTextBox.Size = new System.Drawing.Size(75, 20);
            this.actualGainImbalanceTextBox.TabIndex = 4;
            this.actualGainImbalanceTextBox.TabStop = false;
            this.actualGainImbalanceTextBox.Text = "0.000";
            // 
            // actualQOffsetTextBox
            // 
            this.actualQOffsetTextBox.Location = new System.Drawing.Point(116, 115);
            this.actualQOffsetTextBox.Name = "actualQOffsetTextBox";
            this.actualQOffsetTextBox.ReadOnly = true;
            this.actualQOffsetTextBox.Size = new System.Drawing.Size(75, 20);
            this.actualQOffsetTextBox.TabIndex = 5;
            this.actualQOffsetTextBox.TabStop = false;
            this.actualQOffsetTextBox.Text = "0.00";
            // 
            // actualIOffsetTextBox
            // 
            this.actualIOffsetTextBox.Location = new System.Drawing.Point(16, 115);
            this.actualIOffsetTextBox.Name = "actualIOffsetTextBox";
            this.actualIOffsetTextBox.ReadOnly = true;
            this.actualIOffsetTextBox.Size = new System.Drawing.Size(75, 20);
            this.actualIOffsetTextBox.TabIndex = 6;
            this.actualIOffsetTextBox.TabStop = false;
            this.actualIOffsetTextBox.Text = "0.00";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(17, 383);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(449, 34);
            this.errorTextBox.TabIndex = 7;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(17, 38);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(391, 36);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 5;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(310, 36);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Interval = 50;
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualGainTextBox);
            this.measurementGroupBox.Controls.Add(this.actualIOffsetTextBox);
            this.measurementGroupBox.Controls.Add(this.actualQOffsetTextBox);
            this.measurementGroupBox.Controls.Add(this.actualGainImbalanceTextBox);
            this.measurementGroupBox.Controls.Add(this.actualGainLabel);
            this.measurementGroupBox.Controls.Add(this.actualSkewTextBox);
            this.measurementGroupBox.Controls.Add(this.actualSkewLabel);
            this.measurementGroupBox.Controls.Add(this.actualIOffsetLabel);
            this.measurementGroupBox.Controls.Add(this.actualGainImbalanceLabel);
            this.measurementGroupBox.Controls.Add(this.actualQOffsetLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(252, 95);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(214, 208);
            this.measurementGroupBox.TabIndex = 18;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(488, 451);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.loGroupBox);
            this.Controls.Add(this.centerFrequencyLabel);
            this.Controls.Add(this.powerLevelLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.centerFrequencyNumeric);
            this.Controls.Add(this.powerLevelNumeric);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "External AWG (5611)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.loGroupBox.ResumeLayout(false);
            this.loGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.centerFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.GroupBox loGroupBox;
        private System.Windows.Forms.Label centerFrequencyLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label actualGainLabel;
        private System.Windows.Forms.Label actualSkewLabel;
        private System.Windows.Forms.Label actualGainImbalanceLabel;
        private System.Windows.Forms.Label actualQOffsetLabel;
        private System.Windows.Forms.Label actualIOffsetLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.NumericUpDown centerFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.TextBox actualGainTextBox;
        private System.Windows.Forms.TextBox actualSkewTextBox;
        private System.Windows.Forms.TextBox actualGainImbalanceTextBox;
        private System.Windows.Forms.TextBox actualQOffsetTextBox;
        private System.Windows.Forms.TextBox actualIOffsetTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.ComboBox loSwitchComboBox;
        private System.Windows.Forms.Label loSwitchLabel;
        private System.Windows.Forms.Label frequencyReferenceSourceLabel;
        private System.Windows.Forms.ComboBox frequencyReferenceSourceComboBox;
        private System.Windows.Forms.Label outputTerminalLabel;
        private System.Windows.Forms.ComboBox outputTerminalComboBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;

    }
}
