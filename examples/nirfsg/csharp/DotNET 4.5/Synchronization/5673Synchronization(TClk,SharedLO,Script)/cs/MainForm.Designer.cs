namespace NationalInstruments.Examples.Synchronization5673TClockSharedLOScript
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
            this.slaveReferenceClockOutputTerminalComboBox = new System.Windows.Forms.ComboBox();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.masterReferenceClockSourceComboBox = new System.Windows.Forms.ComboBox();
            this.masterReferenceClockOutputTerminalComboBox = new System.Windows.Forms.ComboBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.rfsgSlaveResourceNamesLabel = new System.Windows.Forms.Label();
            this.slaveRfsgResourceNamesTextBox = new System.Windows.Forms.TextBox();
            this.rfsgMasterResourceNameLabel = new System.Windows.Forms.Label();
            this.slaveReferenceClockSourceComboBox = new System.Windows.Forms.ComboBox();
            this.scriptRichTextBox = new System.Windows.Forms.RichTextBox();
            this.scriptLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.textmessageLabel = new System.Windows.Forms.Label();
            this.masterRfsgResourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.centerFrequencyLabel = new System.Windows.Forms.Label();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqrateLabel = new System.Windows.Forms.Label();
            this.softwareTriggerButton = new System.Windows.Forms.Button();
            this.frequencyReferenceGroupBox = new System.Windows.Forms.GroupBox();
            this.masterReferenceClockSourceLabel = new System.Windows.Forms.Label();
            this.slaveReferenceClockSourceLabel = new System.Windows.Forms.Label();
            this.masterReferenceClockOutputTerminalLabel = new System.Windows.Forms.Label();
            this.slaveReferenceClockOutputTerminalLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            this.frequencyReferenceGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // slaveReferenceClockOutputTerminalComboBox
            // 
            this.slaveReferenceClockOutputTerminalComboBox.Location = new System.Drawing.Point(9, 183);
            this.slaveReferenceClockOutputTerminalComboBox.Name = "slaveReferenceClockOutputTerminalComboBox";
            this.slaveReferenceClockOutputTerminalComboBox.Size = new System.Drawing.Size(119, 21);
            this.slaveReferenceClockOutputTerminalComboBox.TabIndex = 3;
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // masterReferenceClockSourceComboBox
            // 
            this.masterReferenceClockSourceComboBox.Location = new System.Drawing.Point(9, 39);
            this.masterReferenceClockSourceComboBox.Name = "masterReferenceClockSourceComboBox";
            this.masterReferenceClockSourceComboBox.Size = new System.Drawing.Size(119, 21);
            this.masterReferenceClockSourceComboBox.TabIndex = 0;
            // 
            // masterReferenceClockOutputTerminalComboBox
            // 
            this.masterReferenceClockOutputTerminalComboBox.Location = new System.Drawing.Point(9, 87);
            this.masterReferenceClockOutputTerminalComboBox.Name = "masterReferenceClockOutputTerminalComboBox";
            this.masterReferenceClockOutputTerminalComboBox.Size = new System.Drawing.Size(119, 21);
            this.masterReferenceClockOutputTerminalComboBox.TabIndex = 1;
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(439, 559);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 12;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // rfsgSlaveResourceNamesLabel
            // 
            this.rfsgSlaveResourceNamesLabel.AutoSize = true;
            this.rfsgSlaveResourceNamesLabel.Location = new System.Drawing.Point(12, 58);
            this.rfsgSlaveResourceNamesLabel.Name = "rfsgSlaveResourceNamesLabel";
            this.rfsgSlaveResourceNamesLabel.Size = new System.Drawing.Size(237, 13);
            this.rfsgSlaveResourceNamesLabel.TabIndex = 2;
            this.rfsgSlaveResourceNamesLabel.Text = "Rfsg Slave Resource Names (comma separated)";
            // 
            // slaveRfsgResourceNamesTextBox
            // 
            this.slaveRfsgResourceNamesTextBox.Location = new System.Drawing.Point(12, 76);
            this.slaveRfsgResourceNamesTextBox.Name = "slaveRfsgResourceNamesTextBox";
            this.slaveRfsgResourceNamesTextBox.Size = new System.Drawing.Size(121, 20);
            this.slaveRfsgResourceNamesTextBox.TabIndex = 3;
            this.slaveRfsgResourceNamesTextBox.Text = "Slave";
            // 
            // rfsgMasterResourceNameLabel
            // 
            this.rfsgMasterResourceNameLabel.AutoSize = true;
            this.rfsgMasterResourceNameLabel.Location = new System.Drawing.Point(12, 9);
            this.rfsgMasterResourceNameLabel.Name = "rfsgMasterResourceNameLabel";
            this.rfsgMasterResourceNameLabel.Size = new System.Drawing.Size(119, 13);
            this.rfsgMasterResourceNameLabel.TabIndex = 0;
            this.rfsgMasterResourceNameLabel.Text = "Master Resource Name";
            // 
            // slaveReferenceClockSourceComboBox
            // 
            this.slaveReferenceClockSourceComboBox.Location = new System.Drawing.Point(9, 135);
            this.slaveReferenceClockSourceComboBox.Name = "slaveReferenceClockSourceComboBox";
            this.slaveReferenceClockSourceComboBox.Size = new System.Drawing.Size(119, 21);
            this.slaveReferenceClockSourceComboBox.TabIndex = 2;
            // 
            // scriptRichTextBox
            // 
            this.scriptRichTextBox.Location = new System.Drawing.Point(12, 310);
            this.scriptRichTextBox.Name = "scriptRichTextBox";
            this.scriptRichTextBox.Size = new System.Drawing.Size(502, 153);
            this.scriptRichTextBox.TabIndex = 7;
            this.scriptRichTextBox.Text = resources.GetString("scriptRichTextBox.Text");
            // 
            // scriptLabel
            // 
            this.scriptLabel.AutoSize = true;
            this.scriptLabel.Location = new System.Drawing.Point(12, 294);
            this.scriptLabel.Name = "scriptLabel";
            this.scriptLabel.Size = new System.Drawing.Size(34, 13);
            this.scriptLabel.TabIndex = 6;
            this.scriptLabel.Text = "Script";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(12, 470);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 8;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // textmessageLabel
            // 
            this.textmessageLabel.Location = new System.Drawing.Point(255, 9);
            this.textmessageLabel.Name = "textmessageLabel";
            this.textmessageLabel.Size = new System.Drawing.Size(269, 58);
            this.textmessageLabel.TabIndex = 43;
            this.textmessageLabel.Text = "The master NI 5673 should have a LO and AWG associated with it in Measurement and" +
                " Automation Explorer (MAX). Slave devices should have an AWG, but an External LO" +
                ".";
            // 
            // masterRfsgResourceNameComboBox
            // 
            this.masterRfsgResourceNameComboBox.FormattingEnabled = true;
            this.masterRfsgResourceNameComboBox.Location = new System.Drawing.Point(12, 27);
            this.masterRfsgResourceNameComboBox.Name = "masterRfsgResourceNameComboBox";
            this.masterRfsgResourceNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.masterRfsgResourceNameComboBox.TabIndex = 1;
            // 
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.DecimalPlaces = 2;
            this.powerLevelNumeric.Location = new System.Drawing.Point(10, 87);
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
            this.powerLevelNumeric.Size = new System.Drawing.Size(121, 20);
            this.powerLevelNumeric.TabIndex = 3;
            this.powerLevelNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.DecimalPlaces = 6;
            this.frequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.frequencyNumeric.Location = new System.Drawing.Point(10, 40);
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
            this.frequencyNumeric.Size = new System.Drawing.Size(121, 20);
            this.frequencyNumeric.TabIndex = 2;
            this.frequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.powerLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.frequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.powerLevelLabel);
            this.configurationGroupBox.Controls.Add(this.centerFrequencyLabel);
            this.configurationGroupBox.Controls.Add(this.iqRateNumeric);
            this.configurationGroupBox.Controls.Add(this.iqrateLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 111);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(176, 173);
            this.configurationGroupBox.TabIndex = 4;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(10, 70);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 2;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // centerFrequencyLabel
            // 
            this.centerFrequencyLabel.AutoSize = true;
            this.centerFrequencyLabel.Location = new System.Drawing.Point(10, 23);
            this.centerFrequencyLabel.Name = "centerFrequencyLabel";
            this.centerFrequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.centerFrequencyLabel.TabIndex = 1;
            this.centerFrequencyLabel.Text = "Center Frequency [Hz]";
            // 
            // iqRateNumeric
            // 
            this.iqRateNumeric.DecimalPlaces = 2;
            this.iqRateNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.iqRateNumeric.Location = new System.Drawing.Point(10, 134);
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
            this.iqRateNumeric.Size = new System.Drawing.Size(121, 20);
            this.iqRateNumeric.TabIndex = 5;
            this.iqRateNumeric.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // iqrateLabel
            // 
            this.iqrateLabel.AutoSize = true;
            this.iqrateLabel.Location = new System.Drawing.Point(10, 117);
            this.iqrateLabel.Name = "iqrateLabel";
            this.iqrateLabel.Size = new System.Drawing.Size(44, 13);
            this.iqrateLabel.TabIndex = 4;
            this.iqrateLabel.Text = "IQ Rate";
            // 
            // softwareTriggerButton
            // 
            this.softwareTriggerButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.softwareTriggerButton.Enabled = false;
            this.softwareTriggerButton.Location = new System.Drawing.Point(302, 559);
            this.softwareTriggerButton.Name = "softwareTriggerButton";
            this.softwareTriggerButton.Size = new System.Drawing.Size(131, 23);
            this.softwareTriggerButton.TabIndex = 11;
            this.softwareTriggerButton.Text = "Send Software Trigger";
            this.softwareTriggerButton.UseVisualStyleBackColor = true;
            this.softwareTriggerButton.Click += new System.EventHandler(this.softwareTriggerButton_Click);
            // 
            // frequencyReferenceGroupBox
            // 
            this.frequencyReferenceGroupBox.Controls.Add(this.slaveReferenceClockOutputTerminalComboBox);
            this.frequencyReferenceGroupBox.Controls.Add(this.masterReferenceClockSourceComboBox);
            this.frequencyReferenceGroupBox.Controls.Add(this.slaveReferenceClockSourceComboBox);
            this.frequencyReferenceGroupBox.Controls.Add(this.masterReferenceClockOutputTerminalComboBox);
            this.frequencyReferenceGroupBox.Controls.Add(this.masterReferenceClockSourceLabel);
            this.frequencyReferenceGroupBox.Controls.Add(this.slaveReferenceClockSourceLabel);
            this.frequencyReferenceGroupBox.Controls.Add(this.masterReferenceClockOutputTerminalLabel);
            this.frequencyReferenceGroupBox.Controls.Add(this.slaveReferenceClockOutputTerminalLabel);
            this.frequencyReferenceGroupBox.Location = new System.Drawing.Point(258, 70);
            this.frequencyReferenceGroupBox.Name = "frequencyReferenceGroupBox";
            this.frequencyReferenceGroupBox.Size = new System.Drawing.Size(256, 214);
            this.frequencyReferenceGroupBox.TabIndex = 5;
            this.frequencyReferenceGroupBox.TabStop = false;
            this.frequencyReferenceGroupBox.Text = "Frequency Reference Parameters";
            // 
            // masterReferenceClockSourceLabel
            // 
            this.masterReferenceClockSourceLabel.AutoSize = true;
            this.masterReferenceClockSourceLabel.Location = new System.Drawing.Point(6, 21);
            this.masterReferenceClockSourceLabel.Name = "masterReferenceClockSourceLabel";
            this.masterReferenceClockSourceLabel.Size = new System.Drawing.Size(159, 13);
            this.masterReferenceClockSourceLabel.TabIndex = 7;
            this.masterReferenceClockSourceLabel.Text = "Master Reference Clock Source";
            // 
            // slaveReferenceClockSourceLabel
            // 
            this.slaveReferenceClockSourceLabel.AutoSize = true;
            this.slaveReferenceClockSourceLabel.Location = new System.Drawing.Point(9, 118);
            this.slaveReferenceClockSourceLabel.Name = "slaveReferenceClockSourceLabel";
            this.slaveReferenceClockSourceLabel.Size = new System.Drawing.Size(154, 13);
            this.slaveReferenceClockSourceLabel.TabIndex = 6;
            this.slaveReferenceClockSourceLabel.Text = "Slave Reference Clock Source";
            // 
            // masterReferenceClockOutputTerminalLabel
            // 
            this.masterReferenceClockOutputTerminalLabel.AutoSize = true;
            this.masterReferenceClockOutputTerminalLabel.Location = new System.Drawing.Point(9, 70);
            this.masterReferenceClockOutputTerminalLabel.Name = "masterReferenceClockOutputTerminalLabel";
            this.masterReferenceClockOutputTerminalLabel.Size = new System.Drawing.Size(200, 13);
            this.masterReferenceClockOutputTerminalLabel.TabIndex = 5;
            this.masterReferenceClockOutputTerminalLabel.Text = "Master Reference Clock Output Terminal";
            // 
            // slaveReferenceClockOutputTerminalLabel
            // 
            this.slaveReferenceClockOutputTerminalLabel.AutoSize = true;
            this.slaveReferenceClockOutputTerminalLabel.Location = new System.Drawing.Point(9, 166);
            this.slaveReferenceClockOutputTerminalLabel.Name = "slaveReferenceClockOutputTerminalLabel";
            this.slaveReferenceClockOutputTerminalLabel.Size = new System.Drawing.Size(195, 13);
            this.slaveReferenceClockOutputTerminalLabel.TabIndex = 4;
            this.slaveReferenceClockOutputTerminalLabel.Text = "Slave Reference Clock Output Terminal";
            // 
            // startButton
            // 
            this.startButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.startButton.Location = new System.Drawing.Point(221, 559);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 10;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(12, 486);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(502, 67);
            this.errorTextBox.TabIndex = 9;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(537, 588);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.rfsgSlaveResourceNamesLabel);
            this.Controls.Add(this.slaveRfsgResourceNamesTextBox);
            this.Controls.Add(this.rfsgMasterResourceNameLabel);
            this.Controls.Add(this.scriptRichTextBox);
            this.Controls.Add(this.scriptLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.textmessageLabel);
            this.Controls.Add(this.masterRfsgResourceNameComboBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.softwareTriggerButton);
            this.Controls.Add(this.frequencyReferenceGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.errorTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "5673 Synchronization (TClk, Shared LO, Script)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            this.frequencyReferenceGroupBox.ResumeLayout(false);
            this.frequencyReferenceGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.ComboBox slaveReferenceClockOutputTerminalComboBox;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.ComboBox masterReferenceClockSourceComboBox;
        private System.Windows.Forms.ComboBox masterReferenceClockOutputTerminalComboBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label rfsgSlaveResourceNamesLabel;
        private System.Windows.Forms.TextBox slaveRfsgResourceNamesTextBox;
        private System.Windows.Forms.Label rfsgMasterResourceNameLabel;
        private System.Windows.Forms.ComboBox slaveReferenceClockSourceComboBox;
        private System.Windows.Forms.RichTextBox scriptRichTextBox;
        private System.Windows.Forms.Label scriptLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label textmessageLabel;
        private System.Windows.Forms.ComboBox masterRfsgResourceNameComboBox;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label centerFrequencyLabel;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.Label iqrateLabel;
        private System.Windows.Forms.Button softwareTriggerButton;
        private System.Windows.Forms.GroupBox frequencyReferenceGroupBox;
        private System.Windows.Forms.Label masterReferenceClockSourceLabel;
        private System.Windows.Forms.Label slaveReferenceClockSourceLabel;
        private System.Windows.Forms.Label masterReferenceClockOutputTerminalLabel;
        private System.Windows.Forms.Label slaveReferenceClockOutputTerminalLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox errorTextBox;


    }
}
