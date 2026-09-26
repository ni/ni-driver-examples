namespace NationalInstruments.Examples.ScriptTriggerHardwareSource
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
            this.scriptTriggerParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.trigger2TypeLabel = new System.Windows.Forms.Label();
            this.trigger1TypeLabel = new System.Windows.Forms.Label();
            this.triggerSource2Label = new System.Windows.Forms.Label();
            this.triggerSource1Label = new System.Windows.Forms.Label();
            this.trigger2TypeComboBox = new System.Windows.Forms.ComboBox();
            this.trigger1TypeComboBox = new System.Windows.Forms.ComboBox();
            this.triggerSource2ComboBox = new System.Windows.Forms.ComboBox();
            this.triggerSource1ComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.textmessageLabel = new System.Windows.Forms.Label();
            this.actualFrequencyOffsetLabel = new System.Windows.Forms.Label();
            this.actualIQRateLabel = new System.Windows.Forms.Label();
            this.scriptIndexLabel = new System.Windows.Forms.Label();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.scriptIndexNumeric = new System.Windows.Forms.NumericUpDown();
            this.scriptTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.actualFrequencyOffsetTextBox = new System.Windows.Forms.TextBox();
            this.actualIQRateTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.scriptTriggerParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scriptIndexNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            this.measurementGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // scriptLabel
            // 
            this.scriptLabel.AutoSize = true;
            this.scriptLabel.Location = new System.Drawing.Point(19, 203);
            this.scriptLabel.Name = "scriptLabel";
            this.scriptLabel.Size = new System.Drawing.Size(34, 13);
            this.scriptLabel.TabIndex = 0;
            this.scriptLabel.Text = "Script";
            // 
            // scriptTriggerParametersGroupBox
            // 
            this.scriptTriggerParametersGroupBox.Controls.Add(this.trigger2TypeLabel);
            this.scriptTriggerParametersGroupBox.Controls.Add(this.trigger1TypeLabel);
            this.scriptTriggerParametersGroupBox.Controls.Add(this.triggerSource2Label);
            this.scriptTriggerParametersGroupBox.Controls.Add(this.triggerSource1Label);
            this.scriptTriggerParametersGroupBox.Controls.Add(this.trigger2TypeComboBox);
            this.scriptTriggerParametersGroupBox.Controls.Add(this.trigger1TypeComboBox);
            this.scriptTriggerParametersGroupBox.Controls.Add(this.triggerSource2ComboBox);
            this.scriptTriggerParametersGroupBox.Controls.Add(this.triggerSource1ComboBox);
            this.scriptTriggerParametersGroupBox.Location = new System.Drawing.Point(19, 69);
            this.scriptTriggerParametersGroupBox.Name = "scriptTriggerParametersGroupBox";
            this.scriptTriggerParametersGroupBox.Size = new System.Drawing.Size(290, 119);
            this.scriptTriggerParametersGroupBox.TabIndex = 2;
            this.scriptTriggerParametersGroupBox.TabStop = false;
            this.scriptTriggerParametersGroupBox.Text = "Script Trigger Parameters";
            // 
            // trigger2TypeLabel
            // 
            this.trigger2TypeLabel.AutoSize = true;
            this.trigger2TypeLabel.Location = new System.Drawing.Point(10, 71);
            this.trigger2TypeLabel.Name = "trigger2TypeLabel";
            this.trigger2TypeLabel.Size = new System.Drawing.Size(106, 13);
            this.trigger2TypeLabel.TabIndex = 2;
            this.trigger2TypeLabel.Text = "Script Trigger 2 Type";
            // 
            // trigger1TypeLabel
            // 
            this.trigger1TypeLabel.AutoSize = true;
            this.trigger1TypeLabel.Location = new System.Drawing.Point(10, 21);
            this.trigger1TypeLabel.Name = "trigger1TypeLabel";
            this.trigger1TypeLabel.Size = new System.Drawing.Size(103, 13);
            this.trigger1TypeLabel.TabIndex = 3;
            this.trigger1TypeLabel.Text = "Script Trigger 1Type";
            // 
            // triggerSource2Label
            // 
            this.triggerSource2Label.AutoSize = true;
            this.triggerSource2Label.Location = new System.Drawing.Point(154, 71);
            this.triggerSource2Label.Name = "triggerSource2Label";
            this.triggerSource2Label.Size = new System.Drawing.Size(116, 13);
            this.triggerSource2Label.TabIndex = 4;
            this.triggerSource2Label.Text = "Script Trigger Source 2";
            // 
            // triggerSource1Label
            // 
            this.triggerSource1Label.AutoSize = true;
            this.triggerSource1Label.Location = new System.Drawing.Point(154, 21);
            this.triggerSource1Label.Name = "triggerSource1Label";
            this.triggerSource1Label.Size = new System.Drawing.Size(116, 13);
            this.triggerSource1Label.TabIndex = 5;
            this.triggerSource1Label.Text = "Script Trigger Source 1";
            // 
            // trigger2TypeComboBox
            // 
            this.trigger2TypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trigger2TypeComboBox.Location = new System.Drawing.Point(10, 91);
            this.trigger2TypeComboBox.Name = "trigger2TypeComboBox";
            this.trigger2TypeComboBox.Size = new System.Drawing.Size(120, 21);
            this.trigger2TypeComboBox.TabIndex = 1;
            // 
            // trigger1TypeComboBox
            // 
            this.trigger1TypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trigger1TypeComboBox.Location = new System.Drawing.Point(10, 41);
            this.trigger1TypeComboBox.Name = "trigger1TypeComboBox";
            this.trigger1TypeComboBox.Size = new System.Drawing.Size(120, 21);
            this.trigger1TypeComboBox.TabIndex = 0;
            // 
            // triggerSource2ComboBox
            // 
            this.triggerSource2ComboBox.Location = new System.Drawing.Point(154, 91);
            this.triggerSource2ComboBox.Name = "triggerSource2ComboBox";
            this.triggerSource2ComboBox.Size = new System.Drawing.Size(120, 21);
            this.triggerSource2ComboBox.TabIndex = 3;
            // 
            // triggerSource1ComboBox
            // 
            this.triggerSource1ComboBox.Location = new System.Drawing.Point(154, 41);
            this.triggerSource1ComboBox.Name = "triggerSource1ComboBox";
            this.triggerSource1ComboBox.Size = new System.Drawing.Size(120, 21);
            this.triggerSource1ComboBox.TabIndex = 2;
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(18, 6);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 1;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.AutoSize = true;
            this.frequencyLabel.Location = new System.Drawing.Point(18, 21);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.frequencyLabel.TabIndex = 6;
            this.frequencyLabel.Text = "Center Frequency [Hz]";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(18, 71);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 7;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(162, 21);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 8;
            this.iqRateLabel.Text = "IQ Rate [S/s]";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(21, 427);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 9;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // actualFrequencyOffsetLabel
            // 
            this.actualFrequencyOffsetLabel.AutoSize = true;
            this.actualFrequencyOffsetLabel.Location = new System.Drawing.Point(19, 28);
            this.actualFrequencyOffsetLabel.Name = "actualFrequencyOffsetLabel";
            this.actualFrequencyOffsetLabel.Size = new System.Drawing.Size(110, 13);
            this.actualFrequencyOffsetLabel.TabIndex = 11;
            this.actualFrequencyOffsetLabel.Text = "Frequency Offset [Hz]";
            // 
            // actualIQRateLabel
            // 
            this.actualIQRateLabel.AutoSize = true;
            this.actualIQRateLabel.Location = new System.Drawing.Point(162, 28);
            this.actualIQRateLabel.Name = "actualIQRateLabel";
            this.actualIQRateLabel.Size = new System.Drawing.Size(103, 13);
            this.actualIQRateLabel.TabIndex = 12;
            this.actualIQRateLabel.Text = "Actual IQ Rate [S/s]";
            // 
            // scriptIndexLabel
            // 
            this.scriptIndexLabel.AutoSize = true;
            this.scriptIndexLabel.Location = new System.Drawing.Point(16, 204);
            this.scriptIndexLabel.Name = "scriptIndexLabel";
            this.scriptIndexLabel.Size = new System.Drawing.Size(0, 13);
            this.scriptIndexLabel.TabIndex = 14;
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.DecimalPlaces = 6;
            this.frequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.frequencyNumeric.Location = new System.Drawing.Point(18, 42);
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
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.DecimalPlaces = 2;
            this.powerLevelNumeric.Location = new System.Drawing.Point(18, 92);
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
            // iqRateNumeric
            // 
            this.iqRateNumeric.DecimalPlaces = 6;
            this.iqRateNumeric.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.iqRateNumeric.Location = new System.Drawing.Point(162, 42);
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
            // scriptIndexNumeric
            // 
            this.scriptIndexNumeric.Location = new System.Drawing.Point(19, 224);
            this.scriptIndexNumeric.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.scriptIndexNumeric.Name = "scriptIndexNumeric";
            this.scriptIndexNumeric.Size = new System.Drawing.Size(34, 20);
            this.scriptIndexNumeric.TabIndex = 3;
            this.scriptIndexNumeric.ValueChanged += new System.EventHandler(this.scriptIndexNumeric_ValueChanged);
            // 
            // scriptTextBox
            // 
            this.scriptTextBox.Location = new System.Drawing.Point(59, 224);
            this.scriptTextBox.Multiline = true;
            this.scriptTextBox.Name = "scriptTextBox";
            this.scriptTextBox.ReadOnly = true;
            this.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.scriptTextBox.Size = new System.Drawing.Size(250, 190);
            this.scriptTextBox.TabIndex = 4;
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(18, 27);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 1;
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(21, 448);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(606, 34);
            this.errorTextBox.TabIndex = 14;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // actualFrequencyOffsetTextBox
            // 
            this.actualFrequencyOffsetTextBox.Location = new System.Drawing.Point(19, 49);
            this.actualFrequencyOffsetTextBox.Name = "actualFrequencyOffsetTextBox";
            this.actualFrequencyOffsetTextBox.ReadOnly = true;
            this.actualFrequencyOffsetTextBox.Size = new System.Drawing.Size(109, 20);
            this.actualFrequencyOffsetTextBox.TabIndex = 16;
            this.actualFrequencyOffsetTextBox.TabStop = false;
            this.actualFrequencyOffsetTextBox.Text = "0.000000";
            // 
            // actualIQRateTextBox
            // 
            this.actualIQRateTextBox.Location = new System.Drawing.Point(162, 49);
            this.actualIQRateTextBox.Name = "actualIQRateTextBox";
            this.actualIQRateTextBox.ReadOnly = true;
            this.actualIQRateTextBox.Size = new System.Drawing.Size(109, 20);
            this.actualIQRateTextBox.TabIndex = 17;
            this.actualIQRateTextBox.TabStop = false;
            this.actualIQRateTextBox.Text = "0.000000";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(466, 24);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(552, 25);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 7;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Interval = 1;
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.iqRateNumeric);
            this.configurationGroupBox.Controls.Add(this.powerLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.frequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.iqRateLabel);
            this.configurationGroupBox.Controls.Add(this.frequencyLabel);
            this.configurationGroupBox.Controls.Add(this.powerLevelLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(330, 69);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(297, 119);
            this.configurationGroupBox.TabIndex = 5;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualIQRateTextBox);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyOffsetTextBox);
            this.measurementGroupBox.Controls.Add(this.actualIQRateLabel);
            this.measurementGroupBox.Controls.Add(this.actualFrequencyOffsetLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(330, 216);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(297, 86);
            this.measurementGroupBox.TabIndex = 22;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(646, 517);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.scriptTriggerParametersGroupBox);
            this.Controls.Add(this.scriptLabel);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.textmessageLabel);
            this.Controls.Add(this.scriptIndexLabel);
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
            this.Text = "Script Trigger - Hardware Source";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.scriptTriggerParametersGroupBox.ResumeLayout(false);
            this.scriptTriggerParametersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scriptIndexNumeric)).EndInit();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.GroupBox scriptTriggerParametersGroupBox;
        private System.Windows.Forms.Label scriptLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label trigger2TypeLabel;
        private System.Windows.Forms.Label trigger1TypeLabel;
        private System.Windows.Forms.Label triggerSource2Label;
        private System.Windows.Forms.Label triggerSource1Label;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label textmessageLabel;
        private System.Windows.Forms.Label actualFrequencyOffsetLabel;
        private System.Windows.Forms.Label actualIQRateLabel;
        private System.Windows.Forms.Label scriptIndexLabel;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.NumericUpDown scriptIndexNumeric;
        private System.Windows.Forms.TextBox scriptTextBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.TextBox actualFrequencyOffsetTextBox;
        private System.Windows.Forms.TextBox actualIQRateTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox trigger2TypeComboBox;
        private System.Windows.Forms.ComboBox trigger1TypeComboBox;
        private System.Windows.Forms.ComboBox triggerSource2ComboBox;
        private System.Windows.Forms.ComboBox triggerSource1ComboBox;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;

    }
}
