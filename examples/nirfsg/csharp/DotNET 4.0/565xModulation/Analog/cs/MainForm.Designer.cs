namespace NationalInstruments.Examples.AnalogModulation565x
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
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.messageFrequencyLabel = new System.Windows.Forms.Label();
            this.frequencyReferenceSourceLabel = new System.Windows.Forms.Label();
            this.messageWaveformTypeLabel = new System.Windows.Forms.Label();
            this.modulationTypeLabel = new System.Windows.Forms.Label();
            this.pmDeviationLabel = new System.Windows.Forms.Label();
            this.fmDeviationLabel = new System.Windows.Forms.Label();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.messageFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.pmDeviationNumeric = new System.Windows.Forms.NumericUpDown();
            this.fmDeviationNumeric = new System.Windows.Forms.NumericUpDown();
            this.frequencyReferenceSourceComboBox = new System.Windows.Forms.ComboBox();
            this.messageWaveformTypeComboBox = new System.Windows.Forms.ComboBox();
            this.modulationTypeComboBox = new System.Windows.Forms.ComboBox();
            this.generateGroupBox = new System.Windows.Forms.GroupBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.messageFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pmDeviationNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fmDeviationNumeric)).BeginInit();
            this.generateGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.frequencyLabel);
            this.configurationGroupBox.Controls.Add(this.powerLevelLabel);
            this.configurationGroupBox.Controls.Add(this.messageFrequencyLabel);
            this.configurationGroupBox.Controls.Add(this.frequencyReferenceSourceLabel);
            this.configurationGroupBox.Controls.Add(this.messageWaveformTypeLabel);
            this.configurationGroupBox.Controls.Add(this.modulationTypeLabel);
            this.configurationGroupBox.Controls.Add(this.pmDeviationLabel);
            this.configurationGroupBox.Controls.Add(this.fmDeviationLabel);
            this.configurationGroupBox.Controls.Add(this.frequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.powerLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.messageFrequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.pmDeviationNumeric);
            this.configurationGroupBox.Controls.Add(this.fmDeviationNumeric);
            this.configurationGroupBox.Controls.Add(this.frequencyReferenceSourceComboBox);
            this.configurationGroupBox.Controls.Add(this.messageWaveformTypeComboBox);
            this.configurationGroupBox.Controls.Add(this.modulationTypeComboBox);
            this.configurationGroupBox.Location = new System.Drawing.Point(31, 74);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(488, 207);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.AutoSize = true;
            this.frequencyLabel.Location = new System.Drawing.Point(13, 24);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.frequencyLabel.TabIndex = 0;
            this.frequencyLabel.Text = "Center Frequency [Hz]";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(13, 91);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 1;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // messageFrequencyLabel
            // 
            this.messageFrequencyLabel.AutoSize = true;
            this.messageFrequencyLabel.Location = new System.Drawing.Point(325, 91);
            this.messageFrequencyLabel.Name = "messageFrequencyLabel";
            this.messageFrequencyLabel.Size = new System.Drawing.Size(155, 13);
            this.messageFrequencyLabel.TabIndex = 3;
            this.messageFrequencyLabel.Text = "Message Waveform Frequency";
            // 
            // frequencyReferenceSourceLabel
            // 
            this.frequencyReferenceSourceLabel.AutoSize = true;
            this.frequencyReferenceSourceLabel.Location = new System.Drawing.Point(13, 152);
            this.frequencyReferenceSourceLabel.Name = "frequencyReferenceSourceLabel";
            this.frequencyReferenceSourceLabel.Size = new System.Drawing.Size(147, 13);
            this.frequencyReferenceSourceLabel.TabIndex = 4;
            this.frequencyReferenceSourceLabel.Text = "Frequency Reference Source";
            // 
            // messageWaveformTypeLabel
            // 
            this.messageWaveformTypeLabel.AutoSize = true;
            this.messageWaveformTypeLabel.Location = new System.Drawing.Point(325, 24);
            this.messageWaveformTypeLabel.Name = "messageWaveformTypeLabel";
            this.messageWaveformTypeLabel.Size = new System.Drawing.Size(129, 13);
            this.messageWaveformTypeLabel.TabIndex = 5;
            this.messageWaveformTypeLabel.Text = "Message Waveform Type";
            // 
            // modulationTypeLabel
            // 
            this.modulationTypeLabel.AutoSize = true;
            this.modulationTypeLabel.Location = new System.Drawing.Point(171, 24);
            this.modulationTypeLabel.Name = "modulationTypeLabel";
            this.modulationTypeLabel.Size = new System.Drawing.Size(86, 13);
            this.modulationTypeLabel.TabIndex = 6;
            this.modulationTypeLabel.Text = "Modulation Type";
            // 
            // pmDeviationLabel
            // 
            this.pmDeviationLabel.AutoSize = true;
            this.pmDeviationLabel.Location = new System.Drawing.Point(171, 91);
            this.pmDeviationLabel.Name = "pmDeviationLabel";
            this.pmDeviationLabel.Size = new System.Drawing.Size(71, 13);
            this.pmDeviationLabel.TabIndex = 8;
            this.pmDeviationLabel.Text = "PM Deviation";
            // 
            // fmDeviationLabel
            // 
            this.fmDeviationLabel.AutoSize = true;
            this.fmDeviationLabel.Location = new System.Drawing.Point(172, 91);
            this.fmDeviationLabel.Name = "fmDeviationLabel";
            this.fmDeviationLabel.Size = new System.Drawing.Size(70, 13);
            this.fmDeviationLabel.TabIndex = 9;
            this.fmDeviationLabel.Text = "FM Deviation";
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.DecimalPlaces = 6;
            this.frequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.frequencyNumeric.Location = new System.Drawing.Point(13, 45);
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
            this.powerLevelNumeric.Location = new System.Drawing.Point(13, 112);
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
            // messageFrequencyNumeric
            // 
            this.messageFrequencyNumeric.DecimalPlaces = 2;
            this.messageFrequencyNumeric.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.messageFrequencyNumeric.Location = new System.Drawing.Point(325, 112);
            this.messageFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.messageFrequencyNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.messageFrequencyNumeric.Name = "messageFrequencyNumeric";
            this.messageFrequencyNumeric.Size = new System.Drawing.Size(120, 20);
            this.messageFrequencyNumeric.TabIndex = 6;
            this.messageFrequencyNumeric.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // pmDeviationNumeric
            // 
            this.pmDeviationNumeric.DecimalPlaces = 2;
            this.pmDeviationNumeric.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.pmDeviationNumeric.Location = new System.Drawing.Point(171, 112);
            this.pmDeviationNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.pmDeviationNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.pmDeviationNumeric.Name = "pmDeviationNumeric";
            this.pmDeviationNumeric.Size = new System.Drawing.Size(120, 20);
            this.pmDeviationNumeric.TabIndex = 4;
            this.pmDeviationNumeric.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // fmDeviationNumeric
            // 
            this.fmDeviationNumeric.DecimalPlaces = 2;
            this.fmDeviationNumeric.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.fmDeviationNumeric.Location = new System.Drawing.Point(172, 112);
            this.fmDeviationNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.fmDeviationNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.fmDeviationNumeric.Name = "fmDeviationNumeric";
            this.fmDeviationNumeric.Size = new System.Drawing.Size(120, 20);
            this.fmDeviationNumeric.TabIndex = 15;
            this.fmDeviationNumeric.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // frequencyReferenceSourceComboBox
            // 
            this.frequencyReferenceSourceComboBox.Location = new System.Drawing.Point(13, 173);
            this.frequencyReferenceSourceComboBox.Name = "frequencyReferenceSourceComboBox";
            this.frequencyReferenceSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.frequencyReferenceSourceComboBox.TabIndex = 2;
            // 
            // messageWaveformTypeComboBox
            // 
            this.messageWaveformTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.messageWaveformTypeComboBox.Location = new System.Drawing.Point(325, 45);
            this.messageWaveformTypeComboBox.Name = "messageWaveformTypeComboBox";
            this.messageWaveformTypeComboBox.Size = new System.Drawing.Size(120, 21);
            this.messageWaveformTypeComboBox.TabIndex = 5;
            // 
            // modulationTypeComboBox
            // 
            this.modulationTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modulationTypeComboBox.Location = new System.Drawing.Point(171, 45);
            this.modulationTypeComboBox.Name = "modulationTypeComboBox";
            this.modulationTypeComboBox.Size = new System.Drawing.Size(120, 21);
            this.modulationTypeComboBox.TabIndex = 3;
            this.modulationTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.modulationTypeComboBox_SelectedIndexChanged);
            // 
            // generateGroupBox
            // 
            this.generateGroupBox.Controls.Add(this.statusLabel);
            this.generateGroupBox.Controls.Add(this.stopButton);
            this.generateGroupBox.Controls.Add(this.generateButton);
            this.generateGroupBox.Controls.Add(this.statusLed);
            this.generateGroupBox.Location = new System.Drawing.Point(31, 291);
            this.generateGroupBox.Name = "generateGroupBox";
            this.generateGroupBox.Size = new System.Drawing.Size(488, 50);
            this.generateGroupBox.TabIndex = 2;
            this.generateGroupBox.TabStop = false;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(51, 21);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(92, 13);
            this.statusLabel.TabIndex = 19;
            this.statusLabel.Text = "Generation Status";
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(394, 16);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "&Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(293, 16);
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
            this.statusLed.Location = new System.Drawing.Point(13, 13);
            this.statusLed.Name = "statusLed";
            this.statusLed.Size = new System.Drawing.Size(29, 29);
            this.statusLed.TabIndex = 17;
            this.statusLed.TabStop = false;
            this.statusLed.UseVisualStyleBackColor = false;
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(31, 354);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 2;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(31, 16);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 7;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(33, 370);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(486, 60);
            this.errorTextBox.TabIndex = 7;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(31, 37);
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
            this.ClientSize = new System.Drawing.Size(549, 478);
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
            this.Text = "565x Analog Modulation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.messageFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pmDeviationNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fmDeviationNumeric)).EndInit();
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
        private System.Windows.Forms.Label messageFrequencyLabel;
        private System.Windows.Forms.Label frequencyReferenceSourceLabel;
        private System.Windows.Forms.Label messageWaveformTypeLabel;
        private System.Windows.Forms.Label modulationTypeLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label pmDeviationLabel;
        private System.Windows.Forms.Label fmDeviationLabel;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown messageFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown pmDeviationNumeric;
        private System.Windows.Forms.NumericUpDown fmDeviationNumeric;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.ComboBox frequencyReferenceSourceComboBox;
        private System.Windows.Forms.ComboBox messageWaveformTypeComboBox;
        private System.Windows.Forms.ComboBox modulationTypeComboBox;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.Button statusLed;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label statusLabel;

    }
}
