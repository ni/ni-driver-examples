namespace NationalInstruments.Examples.ScriptTriggerSoftwareSource
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
            this.scriptLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.textmessageLabel = new System.Windows.Forms.Label();
            this.actualFrequencyOffsetLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.actualIQRateLabel = new System.Windows.Forms.Label();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.scriptIndexNumeric = new System.Windows.Forms.NumericUpDown();
            this.scriptTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.actualFrequencyOffsetTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.actualIQRateTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.trigger2Button = new System.Windows.Forms.Button();
            this.triggerButton = new System.Windows.Forms.Button();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.readyLabel = new System.Windows.Forms.Label();
            this.readyLed = new System.Windows.Forms.Button();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.triggerControlGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scriptIndexNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            this.measurementGroupBox.SuspendLayout();
            this.triggerControlGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // scriptLabel
            // 
            this.scriptLabel.AutoSize = true;
            this.scriptLabel.Location = new System.Drawing.Point(20, 65);
            this.scriptLabel.Name = "scriptLabel";
            this.scriptLabel.Size = new System.Drawing.Size(34, 13);
            this.scriptLabel.TabIndex = 0;
            this.scriptLabel.Text = "Script";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(20, 10);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 1;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.AutoSize = true;
            this.frequencyLabel.Location = new System.Drawing.Point(15, 25);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.frequencyLabel.TabIndex = 2;
            this.frequencyLabel.Text = "Center Frequency [Hz]";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(166, 24);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 3;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(15, 76);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 4;
            this.iqRateLabel.Text = "IQ Rate [S/s]";
            // 
            // actualFrequencyOffsetLabel
            // 
            this.actualFrequencyOffsetLabel.AutoSize = true;
            this.actualFrequencyOffsetLabel.Location = new System.Drawing.Point(19, 25);
            this.actualFrequencyOffsetLabel.Name = "actualFrequencyOffsetLabel";
            this.actualFrequencyOffsetLabel.Size = new System.Drawing.Size(110, 13);
            this.actualFrequencyOffsetLabel.TabIndex = 6;
            this.actualFrequencyOffsetLabel.Text = "Frequency Offset [Hz]";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(22, 400);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 7;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // actualIQRateLabel
            // 
            this.actualIQRateLabel.AutoSize = true;
            this.actualIQRateLabel.Location = new System.Drawing.Point(19, 76);
            this.actualIQRateLabel.Name = "actualIQRateLabel";
            this.actualIQRateLabel.Size = new System.Drawing.Size(103, 13);
            this.actualIQRateLabel.TabIndex = 8;
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
            this.frequencyNumeric.Location = new System.Drawing.Point(18, 45);
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
            this.powerLevelNumeric.DecimalPlaces = 2;
            this.powerLevelNumeric.Location = new System.Drawing.Point(166, 45);
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
            this.iqRateNumeric.Location = new System.Drawing.Point(17, 96);
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
            // scriptIndexNumeric
            // 
            this.scriptIndexNumeric.Location = new System.Drawing.Point(20, 82);
            this.scriptIndexNumeric.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.scriptIndexNumeric.Name = "scriptIndexNumeric";
            this.scriptIndexNumeric.Size = new System.Drawing.Size(30, 20);
            this.scriptIndexNumeric.TabIndex = 2;
            this.scriptIndexNumeric.ValueChanged += new System.EventHandler(this.scriptIndexNumeric_ValueChanged);
            // 
            // scriptTextBox
            // 
            this.scriptTextBox.Location = new System.Drawing.Point(54, 81);
            this.scriptTextBox.Multiline = true;
            this.scriptTextBox.Name = "scriptTextBox";
            this.scriptTextBox.ReadOnly = true;
            this.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.scriptTextBox.Size = new System.Drawing.Size(268, 120);
            this.scriptTextBox.TabIndex = 3;
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(20, 31);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 1;
            // 
            // actualFrequencyOffsetTextBox
            // 
            this.actualFrequencyOffsetTextBox.Location = new System.Drawing.Point(19, 46);
            this.actualFrequencyOffsetTextBox.Name = "actualFrequencyOffsetTextBox";
            this.actualFrequencyOffsetTextBox.ReadOnly = true;
            this.actualFrequencyOffsetTextBox.Size = new System.Drawing.Size(151, 20);
            this.actualFrequencyOffsetTextBox.TabIndex = 12;
            this.actualFrequencyOffsetTextBox.TabStop = false;
            this.actualFrequencyOffsetTextBox.Text = "100000000.000000";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(20, 419);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(508, 34);
            this.errorTextBox.TabIndex = 13;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // actualIQRateTextBox
            // 
            this.actualIQRateTextBox.Location = new System.Drawing.Point(19, 97);
            this.actualIQRateTextBox.Name = "actualIQRateTextBox";
            this.actualIQRateTextBox.ReadOnly = true;
            this.actualIQRateTextBox.Size = new System.Drawing.Size(151, 20);
            this.actualIQRateTextBox.TabIndex = 14;
            this.actualIQRateTextBox.TabStop = false;
            this.actualIQRateTextBox.Text = "100000.000000";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(372, 29);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 8;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(453, 29);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 9;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // trigger2Button
            // 
            this.trigger2Button.Location = new System.Drawing.Point(13, 88);
            this.trigger2Button.Name = "trigger2Button";
            this.trigger2Button.Size = new System.Drawing.Size(165, 23);
            this.trigger2Button.TabIndex = 1;
            this.trigger2Button.Text = "&Send Software scriptTrigger1";
            this.trigger2Button.UseVisualStyleBackColor = true;
            this.trigger2Button.Click += new System.EventHandler(this.trigger2Button_Click);
            // 
            // triggerButton
            // 
            this.triggerButton.Location = new System.Drawing.Point(13, 57);
            this.triggerButton.Name = "triggerButton";
            this.triggerButton.Size = new System.Drawing.Size(165, 23);
            this.triggerButton.TabIndex = 0;
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
            this.readyLabel.Location = new System.Drawing.Point(64, 24);
            this.readyLabel.Name = "readyLabel";
            this.readyLabel.Size = new System.Drawing.Size(89, 13);
            this.readyLabel.TabIndex = 19;
            this.readyLabel.Text = "Ready for Trigger";
            // 
            // readyLed
            // 
            this.readyLed.BackColor = System.Drawing.SystemColors.Control;
            this.readyLed.Enabled = false;
            this.readyLed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.readyLed.Location = new System.Drawing.Point(33, 19);
            this.readyLed.Name = "readyLed";
            this.readyLed.Size = new System.Drawing.Size(25, 23);
            this.readyLed.TabIndex = 20;
            this.readyLed.TabStop = false;
            this.readyLed.UseVisualStyleBackColor = false;
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.powerLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.iqRateNumeric);
            this.configurationGroupBox.Controls.Add(this.frequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.iqRateLabel);
            this.configurationGroupBox.Controls.Add(this.powerLevelLabel);
            this.configurationGroupBox.Controls.Add(this.frequencyLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(20, 251);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(302, 126);
            this.configurationGroupBox.TabIndex = 4;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualIQRateTextBox);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyOffsetTextBox);
            this.measurementGroupBox.Controls.Add(this.actualIQRateLabel);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyOffsetLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(337, 251);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(191, 126);
            this.measurementGroupBox.TabIndex = 7;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // triggerControlGroupBox
            // 
            this.triggerControlGroupBox.Controls.Add(this.triggerButton);
            this.triggerControlGroupBox.Controls.Add(this.trigger2Button);
            this.triggerControlGroupBox.Controls.Add(this.readyLed);
            this.triggerControlGroupBox.Controls.Add(this.readyLabel);
            this.triggerControlGroupBox.Location = new System.Drawing.Point(337, 81);
            this.triggerControlGroupBox.Name = "triggerControlGroupBox";
            this.triggerControlGroupBox.Size = new System.Drawing.Size(191, 120);
            this.triggerControlGroupBox.TabIndex = 5;
            this.triggerControlGroupBox.TabStop = false;
            this.triggerControlGroupBox.Text = "Trigger Control";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(545, 495);
            this.Controls.Add(this.triggerControlGroupBox);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.scriptLabel);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.textmessageLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.scriptIndexNumeric);
            this.Controls.Add(this.scriptTextBox);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Script Trigger - Software Source";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scriptIndexNumeric)).EndInit();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.triggerControlGroupBox.ResumeLayout(false);
            this.triggerControlGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label scriptLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label textmessageLabel;
        private System.Windows.Forms.Label actualFrequencyOffsetLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label actualIQRateLabel;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.NumericUpDown scriptIndexNumeric;
        private System.Windows.Forms.TextBox scriptTextBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox actualFrequencyOffsetTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.TextBox actualIQRateTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button trigger2Button;
        private System.Windows.Forms.Button triggerButton;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.Label readyLabel;
        private System.Windows.Forms.Button readyLed;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;
        private System.Windows.Forms.GroupBox triggerControlGroupBox;

    }
}
