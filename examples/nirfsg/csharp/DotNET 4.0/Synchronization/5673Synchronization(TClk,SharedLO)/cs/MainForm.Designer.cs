namespace NationalInstruments.Examples.Synchronization5673TClockSharedLO
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
            this.masterRfsgResourceNameLabel = new System.Windows.Forms.Label();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.slaveFrequencyReferenceOutputLabel = new System.Windows.Forms.Label();
            this.masterFrequencyReferenceOutputLabel = new System.Windows.Forms.Label();
            this.slaveFrequencyReferenceSourceLabel = new System.Windows.Forms.Label();
            this.masterFrequencyReferenceSourceLabel = new System.Windows.Forms.Label();
            this.textmessageLabel = new System.Windows.Forms.Label();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();            this.slaveFrequencyReferenceOutputComboBox = new System.Windows.Forms.ComboBox();
            this.masterFrequencyReferenceOutputComboBox = new System.Windows.Forms.ComboBox();
            this.slaveFrequencyReferenceSourceComboBox = new System.Windows.Forms.ComboBox();
            this.masterFrequencyReferenceSourceComboBox = new System.Windows.Forms.ComboBox();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.masterRfsgResourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.slaveRfsgResourceNamesLabel = new System.Windows.Forms.Label();
            this.frequencyReferenceGroupBox = new System.Windows.Forms.GroupBox();
            this.slaveRfsgResourceNamesTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            this.frequencyReferenceGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterRfsgResourceNameLabel
            // 
            this.masterRfsgResourceNameLabel.AutoSize = true;
            this.masterRfsgResourceNameLabel.Location = new System.Drawing.Point(27, 19);
            this.masterRfsgResourceNameLabel.Name = "masterRfsgResourceNameLabel";
            this.masterRfsgResourceNameLabel.Size = new System.Drawing.Size(144, 13);
            this.masterRfsgResourceNameLabel.TabIndex = 0;
            this.masterRfsgResourceNameLabel.Text = "Master Rfsg Resource Name";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.AutoSize = true;
            this.frequencyLabel.Location = new System.Drawing.Point(27, 145);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(79, 13);
            this.frequencyLabel.TabIndex = 1;
            this.frequencyLabel.Text = "Frequency [Hz]";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(27, 199);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 2;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(26, 416);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 3;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // slaveFrequencyReferenceOutputLabel
            // 
            this.slaveFrequencyReferenceOutputLabel.AutoSize = true;
            this.slaveFrequencyReferenceOutputLabel.Location = new System.Drawing.Point(302, 78);
            this.slaveFrequencyReferenceOutputLabel.Name = "slaveFrequencyReferenceOutputLabel";
            this.slaveFrequencyReferenceOutputLabel.Size = new System.Drawing.Size(218, 13);
            this.slaveFrequencyReferenceOutputLabel.TabIndex = 4;
            this.slaveFrequencyReferenceOutputLabel.Text = "Slave Frequency Reference Output Terminal";
            // 
            // masterFrequencyReferenceOutputLabel
            // 
            this.masterFrequencyReferenceOutputLabel.AutoSize = true;
            this.masterFrequencyReferenceOutputLabel.Location = new System.Drawing.Point(20, 78);
            this.masterFrequencyReferenceOutputLabel.Name = "masterFrequencyReferenceOutputLabel";
            this.masterFrequencyReferenceOutputLabel.Size = new System.Drawing.Size(223, 13);
            this.masterFrequencyReferenceOutputLabel.TabIndex = 5;
            this.masterFrequencyReferenceOutputLabel.Text = "Master Frequency Reference Output Terminal";
            // 
            // slaveFrequencyReferenceSourceLabel
            // 
            this.slaveFrequencyReferenceSourceLabel.AutoSize = true;
            this.slaveFrequencyReferenceSourceLabel.Location = new System.Drawing.Point(304, 28);
            this.slaveFrequencyReferenceSourceLabel.Name = "slaveFrequencyReferenceSourceLabel";
            this.slaveFrequencyReferenceSourceLabel.Size = new System.Drawing.Size(177, 13);
            this.slaveFrequencyReferenceSourceLabel.TabIndex = 6;
            this.slaveFrequencyReferenceSourceLabel.Text = "Slave Frequency Reference Source";
            // 
            // masterFrequencyReferenceSourceLabel
            // 
            this.masterFrequencyReferenceSourceLabel.AutoSize = true;
            this.masterFrequencyReferenceSourceLabel.Location = new System.Drawing.Point(21, 28);
            this.masterFrequencyReferenceSourceLabel.Name = "masterFrequencyReferenceSourceLabel";
            this.masterFrequencyReferenceSourceLabel.Size = new System.Drawing.Size(182, 13);
            this.masterFrequencyReferenceSourceLabel.TabIndex = 7;
            this.masterFrequencyReferenceSourceLabel.Text = "Master Frequency Reference Source";
            // 
            // textmessageLabel
            // 
            this.textmessageLabel.Location = new System.Drawing.Point(302, 66);
            this.textmessageLabel.Name = "textmessageLabel";
            this.textmessageLabel.Size = new System.Drawing.Size(269, 58);
            this.textmessageLabel.TabIndex = 5;
            this.textmessageLabel.Text = "The master NI 5673 should have a LO and AWG associated with it in Measurement and" +
                " Automation Explorer (MAX). Slave devices should have an AWG, but an External LO" +
                ".";
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.DecimalPlaces = 6;
            this.frequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.frequencyNumeric.Location = new System.Drawing.Point(27, 166);
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
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.DecimalPlaces = 2;
            this.powerLevelNumeric.Location = new System.Drawing.Point(27, 220);
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
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(27, 435);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(544, 56);
            this.errorTextBox.TabIndex = 8;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(385, 19);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(466, 19);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 7;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            //             // slaveFrequencyReferenceOutputComboBox
            // 
            this.slaveFrequencyReferenceOutputComboBox.Location = new System.Drawing.Point(302, 99);
            this.slaveFrequencyReferenceOutputComboBox.Name = "slaveFrequencyReferenceOutputComboBox";
            this.slaveFrequencyReferenceOutputComboBox.Size = new System.Drawing.Size(124, 21);
            this.slaveFrequencyReferenceOutputComboBox.TabIndex = 3;
            // 
            // masterFrequencyReferenceOutputComboBox
            // 
            this.masterFrequencyReferenceOutputComboBox.Location = new System.Drawing.Point(20, 99);
            this.masterFrequencyReferenceOutputComboBox.Name = "masterFrequencyReferenceOutputComboBox";
            this.masterFrequencyReferenceOutputComboBox.Size = new System.Drawing.Size(123, 21);
            this.masterFrequencyReferenceOutputComboBox.TabIndex = 1;
            // 
            // slaveFrequencyReferenceSourceComboBox
            // 
            this.slaveFrequencyReferenceSourceComboBox.Location = new System.Drawing.Point(304, 49);
            this.slaveFrequencyReferenceSourceComboBox.Name = "slaveFrequencyReferenceSourceComboBox";
            this.slaveFrequencyReferenceSourceComboBox.Size = new System.Drawing.Size(122, 21);
            this.slaveFrequencyReferenceSourceComboBox.TabIndex = 2;
            // 
            // masterFrequencyReferenceSourceComboBox
            // 
            this.masterFrequencyReferenceSourceComboBox.Location = new System.Drawing.Point(21, 49);
            this.masterFrequencyReferenceSourceComboBox.Name = "masterFrequencyReferenceSourceComboBox";
            this.masterFrequencyReferenceSourceComboBox.Size = new System.Drawing.Size(122, 21);
            this.masterFrequencyReferenceSourceComboBox.TabIndex = 0;
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // masterRfsgResourceNameComboBox
            // 
            this.masterRfsgResourceNameComboBox.FormattingEnabled = true;
            this.masterRfsgResourceNameComboBox.Location = new System.Drawing.Point(27, 43);
            this.masterRfsgResourceNameComboBox.Name = "masterRfsgResourceNameComboBox";
            this.masterRfsgResourceNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.masterRfsgResourceNameComboBox.TabIndex = 0;
            // 
            // slaveRfsgResourceNamesLabel
            // 
            this.slaveRfsgResourceNamesLabel.AutoSize = true;
            this.slaveRfsgResourceNamesLabel.Location = new System.Drawing.Point(26, 82);
            this.slaveRfsgResourceNamesLabel.Name = "slaveRfsgResourceNamesLabel";
            this.slaveRfsgResourceNamesLabel.Size = new System.Drawing.Size(237, 13);
            this.slaveRfsgResourceNamesLabel.TabIndex = 14;
            this.slaveRfsgResourceNamesLabel.Text = "Slave Rfsg Resource Names (comma separated)";
            // 
            // frequencyReferenceGroupBox
            // 
            this.frequencyReferenceGroupBox.Controls.Add(this.slaveFrequencyReferenceOutputComboBox);
            this.frequencyReferenceGroupBox.Controls.Add(this.masterFrequencyReferenceSourceComboBox);
            this.frequencyReferenceGroupBox.Controls.Add(this.slaveFrequencyReferenceSourceComboBox);
            this.frequencyReferenceGroupBox.Controls.Add(this.masterFrequencyReferenceOutputComboBox);
            this.frequencyReferenceGroupBox.Controls.Add(this.masterFrequencyReferenceSourceLabel);
            this.frequencyReferenceGroupBox.Controls.Add(this.slaveFrequencyReferenceSourceLabel);
            this.frequencyReferenceGroupBox.Controls.Add(this.masterFrequencyReferenceOutputLabel);
            this.frequencyReferenceGroupBox.Controls.Add(this.slaveFrequencyReferenceOutputLabel);
            this.frequencyReferenceGroupBox.Location = new System.Drawing.Point(27, 257);
            this.frequencyReferenceGroupBox.Name = "frequencyReferenceGroupBox";
            this.frequencyReferenceGroupBox.Size = new System.Drawing.Size(544, 141);
            this.frequencyReferenceGroupBox.TabIndex = 4;
            this.frequencyReferenceGroupBox.TabStop = false;
            this.frequencyReferenceGroupBox.Text = "Frequency Reference Parameters";
            // 
            // slaveRfsgResourceNamesTextBox
            // 
            this.slaveRfsgResourceNamesTextBox.Location = new System.Drawing.Point(27, 104);
            this.slaveRfsgResourceNamesTextBox.Name = "slaveRfsgResourceNamesTextBox";
            this.slaveRfsgResourceNamesTextBox.Size = new System.Drawing.Size(121, 20);
            this.slaveRfsgResourceNamesTextBox.TabIndex = 1;
            this.slaveRfsgResourceNamesTextBox.Text = "Slave";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;            this.ClientSize = new System.Drawing.Size(583, 532);
            this.Controls.Add(this.slaveRfsgResourceNamesTextBox);
            this.Controls.Add(this.frequencyReferenceGroupBox);
            this.Controls.Add(this.slaveRfsgResourceNamesLabel);
            this.Controls.Add(this.masterRfsgResourceNameComboBox);
            this.Controls.Add(this.masterRfsgResourceNameLabel);
            this.Controls.Add(this.frequencyLabel);
            this.Controls.Add(this.powerLevelLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.textmessageLabel);
            this.Controls.Add(this.frequencyNumeric);
            this.Controls.Add(this.powerLevelNumeric);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "5673 Synchronization (TClk, Shared LO)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            this.frequencyReferenceGroupBox.ResumeLayout(false);
            this.frequencyReferenceGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label masterRfsgResourceNameLabel;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label slaveFrequencyReferenceOutputLabel;
        private System.Windows.Forms.Label masterFrequencyReferenceOutputLabel;
        private System.Windows.Forms.Label slaveFrequencyReferenceSourceLabel;
        private System.Windows.Forms.Label masterFrequencyReferenceSourceLabel;
        private System.Windows.Forms.Label textmessageLabel;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;        private System.Windows.Forms.ComboBox slaveFrequencyReferenceOutputComboBox;
        private System.Windows.Forms.ComboBox masterFrequencyReferenceOutputComboBox;
        private System.Windows.Forms.ComboBox slaveFrequencyReferenceSourceComboBox;
        private System.Windows.Forms.ComboBox masterFrequencyReferenceSourceComboBox;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.ComboBox masterRfsgResourceNameComboBox;
        private System.Windows.Forms.Label slaveRfsgResourceNamesLabel;
        private System.Windows.Forms.GroupBox frequencyReferenceGroupBox;
        private System.Windows.Forms.TextBox slaveRfsgResourceNamesTextBox;

    }
}
