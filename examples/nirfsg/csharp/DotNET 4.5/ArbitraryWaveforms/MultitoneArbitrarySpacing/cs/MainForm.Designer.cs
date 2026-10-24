namespace NationalInstruments.Examples.MultitoneArbitrarySpacing
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
            this.toneListGroupBox = new System.Windows.Forms.GroupBox();
            this.offsetLabel = new System.Windows.Forms.Label();
            this.addButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.toneNameTextBox = new System.Windows.Forms.TextBox();
            this.initialPhaseNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.offsetNumeric = new System.Windows.Forms.NumericUpDown();
            this.toneListListBox = new System.Windows.Forms.ListBox();
            this.initialPhaseLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.toneNameLabel = new System.Windows.Forms.Label();
            this.actualPeakPowerLabel = new System.Windows.Forms.Label();
            this.actualPowerLevelLabel = new System.Windows.Forms.Label();
            this.actualPeakPowerTextBox = new System.Windows.Forms.TextBox();
            this.actualPowerLevelTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.centerFrequencyLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.numberOfSamplesLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.actualIQRateLabel = new System.Windows.Forms.Label();
            this.centerFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.numberOfSamplesNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.actualIQRateTextBox = new System.Windows.Forms.TextBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.startButton = new System.Windows.Forms.Button();
            this.mirrorImageCheckBox = new System.Windows.Forms.CheckBox();
            this.mirrorImageLabel = new System.Windows.Forms.Label();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.toneListGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.initialPhaseNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfSamplesNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            this.measurementGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // toneListGroupBox
            // 
            this.toneListGroupBox.Controls.Add(this.offsetLabel);
            this.toneListGroupBox.Controls.Add(this.addButton);
            this.toneListGroupBox.Controls.Add(this.deleteButton);
            this.toneListGroupBox.Controls.Add(this.loadButton);
            this.toneListGroupBox.Controls.Add(this.toneNameTextBox);
            this.toneListGroupBox.Controls.Add(this.initialPhaseNumeric);
            this.toneListGroupBox.Controls.Add(this.powerLevelNumeric);
            this.toneListGroupBox.Controls.Add(this.offsetNumeric);
            this.toneListGroupBox.Controls.Add(this.toneListListBox);
            this.toneListGroupBox.Controls.Add(this.initialPhaseLabel);
            this.toneListGroupBox.Controls.Add(this.powerLevelLabel);
            this.toneListGroupBox.Controls.Add(this.toneNameLabel);
            this.toneListGroupBox.Location = new System.Drawing.Point(212, 83);
            this.toneListGroupBox.Name = "toneListGroupBox";
            this.toneListGroupBox.Size = new System.Drawing.Size(345, 248);
            this.toneListGroupBox.TabIndex = 2;
            this.toneListGroupBox.TabStop = false;
            this.toneListGroupBox.Text = "Tone List";
            // 
            // offsetLabel
            // 
            this.offsetLabel.AutoSize = true;
            this.offsetLabel.Location = new System.Drawing.Point(139, 86);
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(57, 13);
            this.offsetLabel.TabIndex = 5;
            this.offsetLabel.Text = "Offset (Hz)";
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(25, 213);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 5;
            this.addButton.Text = "&Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(116, 213);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 6;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(208, 213);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 7;
            this.loadButton.Text = "&Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // toneNameTextBox
            // 
            this.toneNameTextBox.Location = new System.Drawing.Point(213, 51);
            this.toneNameTextBox.Name = "toneNameTextBox";
            this.toneNameTextBox.Size = new System.Drawing.Size(120, 20);
            this.toneNameTextBox.TabIndex = 1;
            this.toneNameTextBox.Text = "100k tone";
            // 
            // initialPhaseNumeric
            // 
            this.initialPhaseNumeric.DecimalPlaces = 2;
            this.initialPhaseNumeric.Location = new System.Drawing.Point(213, 147);
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
            this.initialPhaseNumeric.TabIndex = 4;
            // 
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.DecimalPlaces = 2;
            this.powerLevelNumeric.Location = new System.Drawing.Point(213, 115);
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
            this.powerLevelNumeric.TabIndex = 3;
            this.powerLevelNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // offsetNumeric
            // 
            this.offsetNumeric.DecimalPlaces = 2;
            this.offsetNumeric.Location = new System.Drawing.Point(213, 83);
            this.offsetNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.offsetNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.offsetNumeric.Name = "offsetNumeric";
            this.offsetNumeric.Size = new System.Drawing.Size(120, 20);
            this.offsetNumeric.TabIndex = 2;
            this.offsetNumeric.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // toneListListBox
            // 
            this.toneListListBox.FormattingEnabled = true;
            this.toneListListBox.Location = new System.Drawing.Point(22, 29);
            this.toneListListBox.Name = "toneListListBox";
            this.toneListListBox.Size = new System.Drawing.Size(100, 160);
            this.toneListListBox.TabIndex = 0;
            // 
            // initialPhaseLabel
            // 
            this.initialPhaseLabel.AutoSize = true;
            this.initialPhaseLabel.Location = new System.Drawing.Point(139, 150);
            this.initialPhaseLabel.Name = "initialPhaseLabel";
            this.initialPhaseLabel.Size = new System.Drawing.Size(67, 13);
            this.initialPhaseLabel.TabIndex = 7;
            this.initialPhaseLabel.Text = "Initial Phase ";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(139, 118);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(66, 13);
            this.powerLevelLabel.TabIndex = 6;
            this.powerLevelLabel.Text = "Power Level";
            // 
            // toneNameLabel
            // 
            this.toneNameLabel.AutoSize = true;
            this.toneNameLabel.Location = new System.Drawing.Point(139, 54);
            this.toneNameLabel.Name = "toneNameLabel";
            this.toneNameLabel.Size = new System.Drawing.Size(63, 13);
            this.toneNameLabel.TabIndex = 4;
            this.toneNameLabel.Text = "Tone Name";
            // 
            // actualPeakPowerLabel
            // 
            this.actualPeakPowerLabel.AutoSize = true;
            this.actualPeakPowerLabel.Location = new System.Drawing.Point(385, 27);
            this.actualPeakPowerLabel.Name = "actualPeakPowerLabel";
            this.actualPeakPowerLabel.Size = new System.Drawing.Size(143, 13);
            this.actualPeakPowerLabel.TabIndex = 9;
            this.actualPeakPowerLabel.Text = "Peak Envelope Power (dBm)";
            // 
            // actualPowerLevelLabel
            // 
            this.actualPowerLevelLabel.AutoSize = true;
            this.actualPowerLevelLabel.Location = new System.Drawing.Point(203, 27);
            this.actualPowerLevelLabel.Name = "actualPowerLevelLabel";
            this.actualPowerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.actualPowerLevelLabel.TabIndex = 10;
            this.actualPowerLevelLabel.Text = "Power Level (dBm)";
            // 
            // actualPeakPowerTextBox
            // 
            this.actualPeakPowerTextBox.Location = new System.Drawing.Point(382, 48);
            this.actualPeakPowerTextBox.Name = "actualPeakPowerTextBox";
            this.actualPeakPowerTextBox.ReadOnly = true;
            this.actualPeakPowerTextBox.Size = new System.Drawing.Size(120, 20);
            this.actualPeakPowerTextBox.TabIndex = 17;
            this.actualPeakPowerTextBox.TabStop = false;
            this.actualPeakPowerTextBox.Text = "0.00";
            // 
            // actualPowerLevelTextBox
            // 
            this.actualPowerLevelTextBox.Location = new System.Drawing.Point(200, 48);
            this.actualPowerLevelTextBox.Name = "actualPowerLevelTextBox";
            this.actualPowerLevelTextBox.ReadOnly = true;
            this.actualPowerLevelTextBox.Size = new System.Drawing.Size(120, 20);
            this.actualPowerLevelTextBox.TabIndex = 18;
            this.actualPowerLevelTextBox.TabStop = false;
            this.actualPowerLevelTextBox.Text = "0.00";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(21, 27);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // centerFrequencyLabel
            // 
            this.centerFrequencyLabel.AutoSize = true;
            this.centerFrequencyLabel.Location = new System.Drawing.Point(27, 29);
            this.centerFrequencyLabel.Name = "centerFrequencyLabel";
            this.centerFrequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.centerFrequencyLabel.TabIndex = 1;
            this.centerFrequencyLabel.Text = "Center Frequency (Hz)";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(27, 83);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 2;
            this.iqRateLabel.Text = "IQ Rate (S/s)";
            // 
            // numberOfSamplesLabel
            // 
            this.numberOfSamplesLabel.AutoSize = true;
            this.numberOfSamplesLabel.Location = new System.Drawing.Point(27, 138);
            this.numberOfSamplesLabel.Name = "numberOfSamplesLabel";
            this.numberOfSamplesLabel.Size = new System.Drawing.Size(99, 13);
            this.numberOfSamplesLabel.TabIndex = 3;
            this.numberOfSamplesLabel.Text = "Number of Samples";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(24, 462);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 8;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // actualIQRateLabel
            // 
            this.actualIQRateLabel.AutoSize = true;
            this.actualIQRateLabel.Location = new System.Drawing.Point(29, 27);
            this.actualIQRateLabel.Name = "actualIQRateLabel";
            this.actualIQRateLabel.Size = new System.Drawing.Size(103, 13);
            this.actualIQRateLabel.TabIndex = 11;
            this.actualIQRateLabel.Text = "Actual IQ Rate (S/s)";
            // 
            // centerFrequencyNumeric
            // 
            this.centerFrequencyNumeric.DecimalPlaces = 2;
            this.centerFrequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.centerFrequencyNumeric.Location = new System.Drawing.Point(24, 49);
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
            this.centerFrequencyNumeric.TabIndex = 0;
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
            this.iqRateNumeric.Location = new System.Drawing.Point(24, 104);
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
            this.iqRateNumeric.TabIndex = 1;
            this.iqRateNumeric.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // numberOfSamplesNumeric
            // 
            this.numberOfSamplesNumeric.DecimalPlaces = 2;
            this.numberOfSamplesNumeric.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numberOfSamplesNumeric.Location = new System.Drawing.Point(24, 159);
            this.numberOfSamplesNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numberOfSamplesNumeric.Name = "numberOfSamplesNumeric";
            this.numberOfSamplesNumeric.Size = new System.Drawing.Size(120, 20);
            this.numberOfSamplesNumeric.TabIndex = 2;
            this.numberOfSamplesNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(18, 44);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(21, 485);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(529, 47);
            this.errorTextBox.TabIndex = 13;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No Error";
            // 
            // actualIQRateTextBox
            // 
            this.actualIQRateTextBox.Location = new System.Drawing.Point(26, 48);
            this.actualIQRateTextBox.Name = "actualIQRateTextBox";
            this.actualIQRateTextBox.ReadOnly = true;
            this.actualIQRateTextBox.Size = new System.Drawing.Size(120, 20);
            this.actualIQRateTextBox.TabIndex = 19;
            this.actualIQRateTextBox.TabStop = false;
            this.actualIQRateTextBox.Text = "0.00";
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(394, 538);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(313, 538);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // mirrorImageCheckBox
            // 
            this.mirrorImageCheckBox.AutoSize = true;
            this.mirrorImageCheckBox.Location = new System.Drawing.Point(30, 219);
            this.mirrorImageCheckBox.Name = "mirrorImageCheckBox";
            this.mirrorImageCheckBox.Size = new System.Drawing.Size(40, 17);
            this.mirrorImageCheckBox.TabIndex = 3;
            this.mirrorImageCheckBox.Text = "On";
            this.mirrorImageCheckBox.UseVisualStyleBackColor = true;
            // 
            // mirrorImageLabel
            // 
            this.mirrorImageLabel.AutoSize = true;
            this.mirrorImageLabel.Location = new System.Drawing.Point(27, 198);
            this.mirrorImageLabel.Name = "mirrorImageLabel";
            this.mirrorImageLabel.Size = new System.Drawing.Size(65, 13);
            this.mirrorImageLabel.TabIndex = 26;
            this.mirrorImageLabel.Text = "Mirror Image";
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.centerFrequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.numberOfSamplesNumeric);
            this.configurationGroupBox.Controls.Add(this.iqRateNumeric);
            this.configurationGroupBox.Controls.Add(this.mirrorImageLabel);
            this.configurationGroupBox.Controls.Add(this.numberOfSamplesLabel);
            this.configurationGroupBox.Controls.Add(this.mirrorImageCheckBox);
            this.configurationGroupBox.Controls.Add(this.iqRateLabel);
            this.configurationGroupBox.Controls.Add(this.centerFrequencyLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(18, 83);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(167, 248);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualPowerLevelTextBox);
            this.measurementGroupBox.Controls.Add(this.actualIQRateTextBox);
            this.measurementGroupBox.Controls.Add(this.actualPeakPowerLabel);
            this.measurementGroupBox.Controls.Add(this.actualIQRateLabel);
            this.measurementGroupBox.Controls.Add(this.actualPeakPowerTextBox);
            this.measurementGroupBox.Controls.Add(this.actualPowerLevelLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(18, 351);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(539, 85);
            this.measurementGroupBox.TabIndex = 0;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(573, 573);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.toneListGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Multitone Arbitrary Spacing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.toneListGroupBox.ResumeLayout(false);
            this.toneListGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.initialPhaseNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfSamplesNumeric)).EndInit();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.GroupBox toneListGroupBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label centerFrequencyLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label numberOfSamplesLabel;
        private System.Windows.Forms.Label toneNameLabel;
        private System.Windows.Forms.Label offsetLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label initialPhaseLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label actualPeakPowerLabel;
        private System.Windows.Forms.Label actualPowerLevelLabel;
        private System.Windows.Forms.Label actualIQRateLabel;
        private System.Windows.Forms.NumericUpDown centerFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.NumericUpDown numberOfSamplesNumeric;
        private System.Windows.Forms.NumericUpDown offsetNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown initialPhaseNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox toneNameTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.TextBox actualPeakPowerTextBox;
        private System.Windows.Forms.TextBox actualPowerLevelTextBox;
        private System.Windows.Forms.TextBox actualIQRateTextBox;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button addButton;        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ListBox toneListListBox;
        private System.Windows.Forms.CheckBox mirrorImageCheckBox;
        private System.Windows.Forms.Label mirrorImageLabel;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;

    }
}
