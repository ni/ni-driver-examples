namespace NationalInstruments.Examples.DigitalModulation565x
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
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.messageUserDefinedWaveformLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.messageSymbolRateLabel = new System.Windows.Forms.Label();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.messagePrbsOrderLabel = new System.Windows.Forms.Label();
            this.frequencyReferenceSourceLabel = new System.Windows.Forms.Label();
            this.messageWaveformTypeLabel = new System.Windows.Forms.Label();
            this.modulationTypeLabel = new System.Windows.Forms.Label();
            this.fskDeviationLabel = new System.Windows.Forms.Label();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.messageSymbolRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.messagePrbsOrderNumeric = new System.Windows.Forms.NumericUpDown();
            this.fskDeviationNumeric = new System.Windows.Forms.NumericUpDown();
            this.messageUserDefinedWaveformTextBox = new System.Windows.Forms.TextBox();
            this.messageWaveformTypeComboBox = new System.Windows.Forms.ComboBox();
            this.modulationTypeComboBox = new System.Windows.Forms.ComboBox();
            this.frequencyReferenceSourceComboBox = new System.Windows.Forms.ComboBox();
            this.generateGroupBox = new System.Windows.Forms.GroupBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.generateButton = new System.Windows.Forms.Button();
            this.statusLed = new System.Windows.Forms.Button();
            this.errorLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.messageSymbolRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.messagePrbsOrderNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fskDeviationNumeric)).BeginInit();
            this.generateGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.messageUserDefinedWaveformLabel);
            this.configurationGroupBox.Controls.Add(this.powerLevelLabel);
            this.configurationGroupBox.Controls.Add(this.messageSymbolRateLabel);
            this.configurationGroupBox.Controls.Add(this.frequencyLabel);
            this.configurationGroupBox.Controls.Add(this.messagePrbsOrderLabel);
            this.configurationGroupBox.Controls.Add(this.frequencyReferenceSourceLabel);
            this.configurationGroupBox.Controls.Add(this.messageWaveformTypeLabel);
            this.configurationGroupBox.Controls.Add(this.modulationTypeLabel);
            this.configurationGroupBox.Controls.Add(this.fskDeviationLabel);
            this.configurationGroupBox.Controls.Add(this.frequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.powerLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.messageSymbolRateNumeric);
            this.configurationGroupBox.Controls.Add(this.messagePrbsOrderNumeric);
            this.configurationGroupBox.Controls.Add(this.fskDeviationNumeric);
            this.configurationGroupBox.Controls.Add(this.messageUserDefinedWaveformTextBox);
            this.configurationGroupBox.Controls.Add(this.messageWaveformTypeComboBox);
            this.configurationGroupBox.Controls.Add(this.modulationTypeComboBox);
            this.configurationGroupBox.Controls.Add(this.frequencyReferenceSourceComboBox);
            this.configurationGroupBox.Location = new System.Drawing.Point(26, 62);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(484, 303);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // messageUserDefinedWaveformLabel
            // 
            this.messageUserDefinedWaveformLabel.AutoSize = true;
            this.messageUserDefinedWaveformLabel.Location = new System.Drawing.Point(22, 200);
            this.messageUserDefinedWaveformLabel.Name = "messageUserDefinedWaveformLabel";
            this.messageUserDefinedWaveformLabel.Size = new System.Drawing.Size(262, 13);
            this.messageUserDefinedWaveformLabel.TabIndex = 10;
            this.messageUserDefinedWaveformLabel.Text = "User Defined Waveform: Enter the bytes (one per line)";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(22, 81);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 1;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // messageSymbolRateLabel
            // 
            this.messageSymbolRateLabel.AutoSize = true;
            this.messageSymbolRateLabel.Location = new System.Drawing.Point(339, 81);
            this.messageSymbolRateLabel.Name = "messageSymbolRateLabel";
            this.messageSymbolRateLabel.Size = new System.Drawing.Size(113, 13);
            this.messageSymbolRateLabel.TabIndex = 3;
            this.messageSymbolRateLabel.Text = "Message Symbol Rate";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.AutoSize = true;
            this.frequencyLabel.Location = new System.Drawing.Point(24, 23);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.frequencyLabel.TabIndex = 0;
            this.frequencyLabel.Text = "Center Frequency [Hz]";
            // 
            // messagePrbsOrderLabel
            // 
            this.messagePrbsOrderLabel.AutoSize = true;
            this.messagePrbsOrderLabel.Location = new System.Drawing.Point(339, 139);
            this.messagePrbsOrderLabel.Name = "messagePrbsOrderLabel";
            this.messagePrbsOrderLabel.Size = new System.Drawing.Size(65, 13);
            this.messagePrbsOrderLabel.TabIndex = 4;
            this.messagePrbsOrderLabel.Text = "PRBS Order";
            // 
            // frequencyReferenceSourceLabel
            // 
            this.frequencyReferenceSourceLabel.AutoSize = true;
            this.frequencyReferenceSourceLabel.Location = new System.Drawing.Point(22, 139);
            this.frequencyReferenceSourceLabel.Name = "frequencyReferenceSourceLabel";
            this.frequencyReferenceSourceLabel.Size = new System.Drawing.Size(147, 13);
            this.frequencyReferenceSourceLabel.TabIndex = 7;
            this.frequencyReferenceSourceLabel.Text = "Frequency Reference Source";
            // 
            // messageWaveformTypeLabel
            // 
            this.messageWaveformTypeLabel.AutoSize = true;
            this.messageWaveformTypeLabel.Location = new System.Drawing.Point(341, 22);
            this.messageWaveformTypeLabel.Name = "messageWaveformTypeLabel";
            this.messageWaveformTypeLabel.Size = new System.Drawing.Size(129, 13);
            this.messageWaveformTypeLabel.TabIndex = 5;
            this.messageWaveformTypeLabel.Text = "Message Waveform Type";
            // 
            // modulationTypeLabel
            // 
            this.modulationTypeLabel.AutoSize = true;
            this.modulationTypeLabel.Location = new System.Drawing.Point(182, 23);
            this.modulationTypeLabel.Name = "modulationTypeLabel";
            this.modulationTypeLabel.Size = new System.Drawing.Size(86, 13);
            this.modulationTypeLabel.TabIndex = 6;
            this.modulationTypeLabel.Text = "Modulation Type";
            // 
            // fskDeviationLabel
            // 
            this.fskDeviationLabel.AutoSize = true;
            this.fskDeviationLabel.Location = new System.Drawing.Point(180, 81);
            this.fskDeviationLabel.Name = "fskDeviationLabel";
            this.fskDeviationLabel.Size = new System.Drawing.Size(75, 13);
            this.fskDeviationLabel.TabIndex = 8;
            this.fskDeviationLabel.Text = "FSK Deviation";
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.DecimalPlaces = 6;
            this.frequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.frequencyNumeric.Location = new System.Drawing.Point(23, 39);
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
            this.powerLevelNumeric.Location = new System.Drawing.Point(23, 97);
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
            // messageSymbolRateNumeric
            // 
            this.messageSymbolRateNumeric.DecimalPlaces = 2;
            this.messageSymbolRateNumeric.Location = new System.Drawing.Point(340, 97);
            this.messageSymbolRateNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.messageSymbolRateNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.messageSymbolRateNumeric.Name = "messageSymbolRateNumeric";
            this.messageSymbolRateNumeric.Size = new System.Drawing.Size(120, 20);
            this.messageSymbolRateNumeric.TabIndex = 6;
            this.messageSymbolRateNumeric.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // messagePrbsOrderNumeric
            // 
            this.messagePrbsOrderNumeric.Location = new System.Drawing.Point(340, 156);
            this.messagePrbsOrderNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.messagePrbsOrderNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.messagePrbsOrderNumeric.Name = "messagePrbsOrderNumeric";
            this.messagePrbsOrderNumeric.Size = new System.Drawing.Size(120, 20);
            this.messagePrbsOrderNumeric.TabIndex = 7;
            this.messagePrbsOrderNumeric.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            // 
            // fskDeviationNumeric
            // 
            this.fskDeviationNumeric.DecimalPlaces = 2;
            this.fskDeviationNumeric.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.fskDeviationNumeric.Location = new System.Drawing.Point(181, 97);
            this.fskDeviationNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.fskDeviationNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.fskDeviationNumeric.Name = "fskDeviationNumeric";
            this.fskDeviationNumeric.Size = new System.Drawing.Size(120, 20);
            this.fskDeviationNumeric.TabIndex = 4;
            this.fskDeviationNumeric.Value = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            // 
            // messageUserDefinedWaveformTextBox
            // 
            this.messageUserDefinedWaveformTextBox.Location = new System.Drawing.Point(23, 218);
            this.messageUserDefinedWaveformTextBox.Multiline = true;
            this.messageUserDefinedWaveformTextBox.Name = "messageUserDefinedWaveformTextBox";
            this.messageUserDefinedWaveformTextBox.Size = new System.Drawing.Size(437, 71);
            this.messageUserDefinedWaveformTextBox.TabIndex = 15;
            this.messageUserDefinedWaveformTextBox.TabStop = false;
            this.messageUserDefinedWaveformTextBox.Text = "01";
            this.messageUserDefinedWaveformTextBox.TextChanged += new System.EventHandler(this.messageUserDefinedWaveformTextBox_TextChanged);
            // 
            // messageWaveformTypeComboBox
            // 
            this.messageWaveformTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.messageWaveformTypeComboBox.Location = new System.Drawing.Point(340, 38);
            this.messageWaveformTypeComboBox.Name = "messageWaveformTypeComboBox";
            this.messageWaveformTypeComboBox.Size = new System.Drawing.Size(120, 21);
            this.messageWaveformTypeComboBox.TabIndex = 5;
            // 
            // modulationTypeComboBox
            // 
            this.modulationTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modulationTypeComboBox.Location = new System.Drawing.Point(181, 39);
            this.modulationTypeComboBox.Name = "modulationTypeComboBox";
            this.modulationTypeComboBox.Size = new System.Drawing.Size(120, 21);
            this.modulationTypeComboBox.TabIndex = 3;
            this.modulationTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.modulationTypeComboBox_SelectedIndexChanged);
            // 
            // frequencyReferenceSourceComboBox
            // 
            this.frequencyReferenceSourceComboBox.Location = new System.Drawing.Point(23, 155);
            this.frequencyReferenceSourceComboBox.Name = "frequencyReferenceSourceComboBox";
            this.frequencyReferenceSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.frequencyReferenceSourceComboBox.TabIndex = 2;
            // 
            // generateGroupBox
            // 
            this.generateGroupBox.Controls.Add(this.stopButton);
            this.generateGroupBox.Controls.Add(this.statusLabel);
            this.generateGroupBox.Controls.Add(this.generateButton);
            this.generateGroupBox.Controls.Add(this.statusLed);
            this.generateGroupBox.Location = new System.Drawing.Point(26, 369);
            this.generateGroupBox.Name = "generateGroupBox";
            this.generateGroupBox.Size = new System.Drawing.Size(484, 51);
            this.generateGroupBox.TabIndex = 2;
            this.generateGroupBox.TabStop = false;
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(381, 18);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "&Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(61, 23);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(92, 13);
            this.statusLabel.TabIndex = 11;
            this.statusLabel.Text = "Generation Status";
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(275, 18);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(75, 23);
            this.generateButton.TabIndex = 0;
            this.generateButton.Text = "&Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // statusLed
            // 
            this.statusLed.BackColor = System.Drawing.SystemColors.Control;
            this.statusLed.Enabled = false;
            this.statusLed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.statusLed.Location = new System.Drawing.Point(23, 15);
            this.statusLed.Name = "statusLed";
            this.statusLed.Size = new System.Drawing.Size(29, 29);
            this.statusLed.TabIndex = 18;
            this.statusLed.TabStop = false;
            this.statusLed.UseVisualStyleBackColor = false;
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(26, 441);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 2;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(26, 13);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 9;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(25, 461);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(485, 66);
            this.errorTextBox.TabIndex = 7;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(26, 29);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.stopButton;
            this.ClientSize = new System.Drawing.Size(533, 570);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.generateGroupBox);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.resourceNameComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "565x Digital Modulation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.messageSymbolRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.messagePrbsOrderNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fskDeviationNumeric)).EndInit();
            this.generateGroupBox.ResumeLayout(false);
            this.generateGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox generateGroupBox;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label messageSymbolRateLabel;
        private System.Windows.Forms.Label messagePrbsOrderLabel;
        private System.Windows.Forms.Label messageWaveformTypeLabel;
        private System.Windows.Forms.Label modulationTypeLabel;
        private System.Windows.Forms.Label frequencyReferenceSourceLabel;
        private System.Windows.Forms.Label fskDeviationLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label messageUserDefinedWaveformLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown messageSymbolRateNumeric;
        private System.Windows.Forms.NumericUpDown messagePrbsOrderNumeric;
        private System.Windows.Forms.NumericUpDown fskDeviationNumeric;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox messageUserDefinedWaveformTextBox;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.ComboBox messageWaveformTypeComboBox;
        private System.Windows.Forms.ComboBox modulationTypeComboBox;
        private System.Windows.Forms.ComboBox frequencyReferenceSourceComboBox;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.Button statusLed;
        private System.Windows.Forms.Button stopButton;

    }
}
