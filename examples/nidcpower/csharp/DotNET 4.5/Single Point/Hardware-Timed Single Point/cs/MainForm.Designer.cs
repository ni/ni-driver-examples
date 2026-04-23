namespace NationalInstruments.Examples.HardwareTimedSinglePoint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.sourceDelayLabel = new System.Windows.Forms.Label();
            this.voltageLevelRangeLabel = new System.Windows.Forms.Label();
            this.fetchTimeoutLabel = new System.Windows.Forms.Label();
            this.currentLimitRangeLabel = new System.Windows.Forms.Label();
            this.currentLimitLabel = new System.Windows.Forms.Label();
            this.sourceDelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageLevelRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.fetchTimeoutNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageLevel1Numeric = new System.Windows.Forms.NumericUpDown();
            this.currentLimitRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.resourceNameAndChannelNameGroupBox = new System.Windows.Forms.GroupBox();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.voltageLevel1GroupBox = new System.Windows.Forms.GroupBox();
            this.voltageLevel2Numeric = new System.Windows.Forms.NumericUpDown();
            this.measurements1GroupBox = new System.Windows.Forms.GroupBox();
            this.voltage1MeasurementsTextBox = new System.Windows.Forms.TextBox();
            this.current1MeasurementsTextBox = new System.Windows.Forms.TextBox();
            this.measuredCurrent1Label = new System.Windows.Forms.Label();
            this.inCompliance1ButtonLed = new System.Windows.Forms.Button();
            this.measuredVoltage1Label = new System.Windows.Forms.Label();
            this.inCompliance1Label = new System.Windows.Forms.Label();
            this.voltageLevel2GroupBox = new System.Windows.Forms.GroupBox();
            this.measurements2GroupBox = new System.Windows.Forms.GroupBox();
            this.voltage2MeasurementsTextBox = new System.Windows.Forms.TextBox();
            this.current2MeasurementsTextBox = new System.Windows.Forms.TextBox();
            this.measuredCurrent2Label = new System.Windows.Forms.Label();
            this.inCompliance2ButtonLed = new System.Windows.Forms.Button();
            this.measuredVoltage2Label = new System.Windows.Forms.Label();
            this.inCompliance2Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDelayNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fetchTimeoutNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevel1Numeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitNumeric)).BeginInit();
            this.resourceNameAndChannelNameGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            this.voltageLevel1GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevel2Numeric)).BeginInit();
            this.measurements1GroupBox.SuspendLayout();
            this.voltageLevel2GroupBox.SuspendLayout();
            this.measurements2GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // sourceDelayLabel
            // 
            this.sourceDelayLabel.AutoSize = true;
            this.sourceDelayLabel.Location = new System.Drawing.Point(6, 120);
            this.sourceDelayLabel.Name = "sourceDelayLabel";
            this.sourceDelayLabel.Size = new System.Drawing.Size(85, 13);
            this.sourceDelayLabel.TabIndex = 8;
            this.sourceDelayLabel.Text = "Source Delay (s)";
            // 
            // voltageLevelRangeLabel
            // 
            this.voltageLevelRangeLabel.AutoSize = true;
            this.voltageLevelRangeLabel.Location = new System.Drawing.Point(6, 52);
            this.voltageLevelRangeLabel.Name = "voltageLevelRangeLabel";
            this.voltageLevelRangeLabel.Size = new System.Drawing.Size(123, 13);
            this.voltageLevelRangeLabel.TabIndex = 4;
            this.voltageLevelRangeLabel.Text = "Voltage Level Range (V)";
            // 
            // fetchTimeoutLabel
            // 
            this.fetchTimeoutLabel.AutoSize = true;
            this.fetchTimeoutLabel.Location = new System.Drawing.Point(6, 153);
            this.fetchTimeoutLabel.Name = "fetchTimeoutLabel";
            this.fetchTimeoutLabel.Size = new System.Drawing.Size(89, 13);
            this.fetchTimeoutLabel.TabIndex = 10;
            this.fetchTimeoutLabel.Text = "Fetch Timeout (s)";
            // 
            // currentLimitRangeLabel
            // 
            this.currentLimitRangeLabel.AutoSize = true;
            this.currentLimitRangeLabel.Location = new System.Drawing.Point(6, 85);
            this.currentLimitRangeLabel.Name = "currentLimitRangeLabel";
            this.currentLimitRangeLabel.Size = new System.Drawing.Size(116, 13);
            this.currentLimitRangeLabel.TabIndex = 6;
            this.currentLimitRangeLabel.Text = "Current Limit Range (A)";
            // 
            // currentLimitLabel
            // 
            this.currentLimitLabel.AutoSize = true;
            this.currentLimitLabel.Location = new System.Drawing.Point(6, 22);
            this.currentLimitLabel.Name = "currentLimitLabel";
            this.currentLimitLabel.Size = new System.Drawing.Size(81, 13);
            this.currentLimitLabel.TabIndex = 2;
            this.currentLimitLabel.Text = "Current Limit (A)";
            // 
            // sourceDelayNumeric
            // 
            this.sourceDelayNumeric.DecimalPlaces = 6;
            this.sourceDelayNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.sourceDelayNumeric.Location = new System.Drawing.Point(141, 118);
            this.sourceDelayNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.sourceDelayNumeric.Name = "sourceDelayNumeric";
            this.sourceDelayNumeric.Size = new System.Drawing.Size(90, 20);
            this.sourceDelayNumeric.TabIndex = 9;
            this.sourceDelayNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // voltageLevelRangeNumeric
            // 
            this.voltageLevelRangeNumeric.DecimalPlaces = 6;
            this.voltageLevelRangeNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.voltageLevelRangeNumeric.Location = new System.Drawing.Point(141, 50);
            this.voltageLevelRangeNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.voltageLevelRangeNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.voltageLevelRangeNumeric.Name = "voltageLevelRangeNumeric";
            this.voltageLevelRangeNumeric.Size = new System.Drawing.Size(90, 20);
            this.voltageLevelRangeNumeric.TabIndex = 5;
            this.voltageLevelRangeNumeric.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // fetchTimeoutNumeric
            // 
            this.fetchTimeoutNumeric.DecimalPlaces = 6;
            this.fetchTimeoutNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.fetchTimeoutNumeric.Location = new System.Drawing.Point(140, 149);
            this.fetchTimeoutNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.fetchTimeoutNumeric.Name = "fetchTimeoutNumeric";
            this.fetchTimeoutNumeric.Size = new System.Drawing.Size(90, 20);
            this.fetchTimeoutNumeric.TabIndex = 11;
            this.fetchTimeoutNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // voltageLevel1Numeric
            // 
            this.voltageLevel1Numeric.DecimalPlaces = 6;
            this.voltageLevel1Numeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.voltageLevel1Numeric.Location = new System.Drawing.Point(9, 18);
            this.voltageLevel1Numeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.voltageLevel1Numeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.voltageLevel1Numeric.Name = "voltageLevel1Numeric";
            this.voltageLevel1Numeric.Size = new System.Drawing.Size(90, 20);
            this.voltageLevel1Numeric.TabIndex = 0;
            this.voltageLevel1Numeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // currentLimitRangeNumeric
            // 
            this.currentLimitRangeNumeric.DecimalPlaces = 6;
            this.currentLimitRangeNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.currentLimitRangeNumeric.Location = new System.Drawing.Point(141, 83);
            this.currentLimitRangeNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.currentLimitRangeNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.currentLimitRangeNumeric.Name = "currentLimitRangeNumeric";
            this.currentLimitRangeNumeric.Size = new System.Drawing.Size(90, 20);
            this.currentLimitRangeNumeric.TabIndex = 7;
            this.currentLimitRangeNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // currentLimitNumeric
            // 
            this.currentLimitNumeric.DecimalPlaces = 6;
            this.currentLimitNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.currentLimitNumeric.Location = new System.Drawing.Point(141, 20);
            this.currentLimitNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.currentLimitNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.currentLimitNumeric.Name = "currentLimitNumeric";
            this.currentLimitNumeric.Size = new System.Drawing.Size(90, 20);
            this.currentLimitNumeric.TabIndex = 3;
            this.currentLimitNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(410, 255);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // resourceNameAndChannelNameGroupBox
            // 
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.channelNameTextBox);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.resourceNameComboBox);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.channelNameLabel);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.resourceNameLabel);
            this.resourceNameAndChannelNameGroupBox.Location = new System.Drawing.Point(12, 12);
            this.resourceNameAndChannelNameGroupBox.Name = "resourceNameAndChannelNameGroupBox";
            this.resourceNameAndChannelNameGroupBox.Size = new System.Drawing.Size(237, 73);
            this.resourceNameAndChannelNameGroupBox.TabIndex = 0;
            this.resourceNameAndChannelNameGroupBox.TabStop = false;
            this.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name";
            // 
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(140, 45);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(91, 20);
            this.channelNameTextBox.TabIndex = 15;
            this.channelNameTextBox.Text = "0";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(106, 18);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(125, 21);
            this.resourceNameComboBox.TabIndex = 1;
            // 
            // channelNameLabel
            // 
            this.channelNameLabel.AutoSize = true;
            this.channelNameLabel.Location = new System.Drawing.Point(6, 49);
            this.channelNameLabel.Name = "channelNameLabel";
            this.channelNameLabel.Size = new System.Drawing.Size(77, 13);
            this.channelNameLabel.TabIndex = 2;
            this.channelNameLabel.Text = "Channel Name";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 22);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.currentLimitLabel);
            this.configurationGroupBox.Controls.Add(this.fetchTimeoutLabel);
            this.configurationGroupBox.Controls.Add(this.sourceDelayLabel);
            this.configurationGroupBox.Controls.Add(this.currentLimitNumeric);
            this.configurationGroupBox.Controls.Add(this.voltageLevelRangeLabel);
            this.configurationGroupBox.Controls.Add(this.voltageLevelRangeNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLimitRangeLabel);
            this.configurationGroupBox.Controls.Add(this.currentLimitRangeNumeric);
            this.configurationGroupBox.Controls.Add(this.sourceDelayNumeric);
            this.configurationGroupBox.Controls.Add(this.fetchTimeoutNumeric);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 101);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(237, 177);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // voltageLevel1GroupBox
            // 
            this.voltageLevel1GroupBox.Controls.Add(this.voltageLevel1Numeric);
            this.voltageLevel1GroupBox.Location = new System.Drawing.Point(265, 12);
            this.voltageLevel1GroupBox.Name = "voltageLevel1GroupBox";
            this.voltageLevel1GroupBox.Size = new System.Drawing.Size(179, 49);
            this.voltageLevel1GroupBox.TabIndex = 2;
            this.voltageLevel1GroupBox.TabStop = false;
            this.voltageLevel1GroupBox.Text = "Voltage Level 1 (V)";
            // 
            // voltageLevel2Numeric
            // 
            this.voltageLevel2Numeric.DecimalPlaces = 6;
            this.voltageLevel2Numeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.voltageLevel2Numeric.Location = new System.Drawing.Point(6, 18);
            this.voltageLevel2Numeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.voltageLevel2Numeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.voltageLevel2Numeric.Name = "voltageLevel2Numeric";
            this.voltageLevel2Numeric.Size = new System.Drawing.Size(90, 20);
            this.voltageLevel2Numeric.TabIndex = 0;
            this.voltageLevel2Numeric.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // measurements1GroupBox
            // 
            this.measurements1GroupBox.Controls.Add(this.voltage1MeasurementsTextBox);
            this.measurements1GroupBox.Controls.Add(this.current1MeasurementsTextBox);
            this.measurements1GroupBox.Controls.Add(this.measuredCurrent1Label);
            this.measurements1GroupBox.Controls.Add(this.inCompliance1ButtonLed);
            this.measurements1GroupBox.Controls.Add(this.measuredVoltage1Label);
            this.measurements1GroupBox.Controls.Add(this.inCompliance1Label);
            this.measurements1GroupBox.Location = new System.Drawing.Point(265, 101);
            this.measurements1GroupBox.Name = "measurements1GroupBox";
            this.measurements1GroupBox.Size = new System.Drawing.Size(179, 140);
            this.measurements1GroupBox.TabIndex = 4;
            this.measurements1GroupBox.TabStop = false;
            this.measurements1GroupBox.Text = "Measurements 1";
            // 
            // voltage1MeasurementsTextBox
            // 
            this.voltage1MeasurementsTextBox.Location = new System.Drawing.Point(80, 18);
            this.voltage1MeasurementsTextBox.Name = "voltage1MeasurementsTextBox";
            this.voltage1MeasurementsTextBox.ReadOnly = true;
            this.voltage1MeasurementsTextBox.Size = new System.Drawing.Size(91, 20);
            this.voltage1MeasurementsTextBox.TabIndex = 1;
            this.voltage1MeasurementsTextBox.Text = "0.000000E+000";
            // 
            // current1MeasurementsTextBox
            // 
            this.current1MeasurementsTextBox.Location = new System.Drawing.Point(80, 45);
            this.current1MeasurementsTextBox.Name = "current1MeasurementsTextBox";
            this.current1MeasurementsTextBox.ReadOnly = true;
            this.current1MeasurementsTextBox.Size = new System.Drawing.Size(91, 20);
            this.current1MeasurementsTextBox.TabIndex = 3;
            this.current1MeasurementsTextBox.Text = "0.000000E+000";
            // 
            // measuredCurrent1Label
            // 
            this.measuredCurrent1Label.AutoSize = true;
            this.measuredCurrent1Label.Location = new System.Drawing.Point(6, 49);
            this.measuredCurrent1Label.Name = "measuredCurrent1Label";
            this.measuredCurrent1Label.Size = new System.Drawing.Size(66, 13);
            this.measuredCurrent1Label.TabIndex = 2;
            this.measuredCurrent1Label.Text = "Current 1 (A)";
            // 
            // inCompliance1ButtonLed
            // 
            this.inCompliance1ButtonLed.Enabled = false;
            this.inCompliance1ButtonLed.Location = new System.Drawing.Point(71, 101);
            this.inCompliance1ButtonLed.Name = "inCompliance1ButtonLed";
            this.inCompliance1ButtonLed.Size = new System.Drawing.Size(21, 21);
            this.inCompliance1ButtonLed.TabIndex = 5;
            this.inCompliance1ButtonLed.UseVisualStyleBackColor = true;
            // 
            // measuredVoltage1Label
            // 
            this.measuredVoltage1Label.AutoSize = true;
            this.measuredVoltage1Label.Location = new System.Drawing.Point(6, 22);
            this.measuredVoltage1Label.Name = "measuredVoltage1Label";
            this.measuredVoltage1Label.Size = new System.Drawing.Size(68, 13);
            this.measuredVoltage1Label.TabIndex = 0;
            this.measuredVoltage1Label.Text = "Voltage 1 (V)";
            // 
            // inCompliance1Label
            // 
            this.inCompliance1Label.AutoSize = true;
            this.inCompliance1Label.Location = new System.Drawing.Point(40, 85);
            this.inCompliance1Label.Name = "inCompliance1Label";
            this.inCompliance1Label.Size = new System.Drawing.Size(83, 13);
            this.inCompliance1Label.TabIndex = 4;
            this.inCompliance1Label.Text = "In Compliance 1";
            // 
            // voltageLevel2GroupBox
            // 
            this.voltageLevel2GroupBox.Controls.Add(this.voltageLevel2Numeric);
            this.voltageLevel2GroupBox.Location = new System.Drawing.Point(460, 12);
            this.voltageLevel2GroupBox.Name = "voltageLevel2GroupBox";
            this.voltageLevel2GroupBox.Size = new System.Drawing.Size(179, 49);
            this.voltageLevel2GroupBox.TabIndex = 3;
            this.voltageLevel2GroupBox.TabStop = false;
            this.voltageLevel2GroupBox.Text = "Voltage Level 2 (V)";
            // 
            // measurements2GroupBox
            // 
            this.measurements2GroupBox.Controls.Add(this.voltage2MeasurementsTextBox);
            this.measurements2GroupBox.Controls.Add(this.current2MeasurementsTextBox);
            this.measurements2GroupBox.Controls.Add(this.measuredCurrent2Label);
            this.measurements2GroupBox.Controls.Add(this.inCompliance2ButtonLed);
            this.measurements2GroupBox.Controls.Add(this.measuredVoltage2Label);
            this.measurements2GroupBox.Controls.Add(this.inCompliance2Label);
            this.measurements2GroupBox.Location = new System.Drawing.Point(460, 101);
            this.measurements2GroupBox.Name = "measurements2GroupBox";
            this.measurements2GroupBox.Size = new System.Drawing.Size(179, 140);
            this.measurements2GroupBox.TabIndex = 5;
            this.measurements2GroupBox.TabStop = false;
            this.measurements2GroupBox.Text = "Measurements 2";
            // 
            // voltage2MeasurementsTextBox
            // 
            this.voltage2MeasurementsTextBox.Location = new System.Drawing.Point(80, 18);
            this.voltage2MeasurementsTextBox.Name = "voltage2MeasurementsTextBox";
            this.voltage2MeasurementsTextBox.ReadOnly = true;
            this.voltage2MeasurementsTextBox.Size = new System.Drawing.Size(91, 20);
            this.voltage2MeasurementsTextBox.TabIndex = 1;
            this.voltage2MeasurementsTextBox.Text = "0.000000E+000";
            // 
            // current2MeasurementsTextBox
            // 
            this.current2MeasurementsTextBox.Location = new System.Drawing.Point(80, 45);
            this.current2MeasurementsTextBox.Name = "current2MeasurementsTextBox";
            this.current2MeasurementsTextBox.ReadOnly = true;
            this.current2MeasurementsTextBox.Size = new System.Drawing.Size(91, 20);
            this.current2MeasurementsTextBox.TabIndex = 3;
            this.current2MeasurementsTextBox.Text = "0.000000E+000";
            // 
            // measuredCurrent2Label
            // 
            this.measuredCurrent2Label.AutoSize = true;
            this.measuredCurrent2Label.Location = new System.Drawing.Point(6, 49);
            this.measuredCurrent2Label.Name = "measuredCurrent2Label";
            this.measuredCurrent2Label.Size = new System.Drawing.Size(66, 13);
            this.measuredCurrent2Label.TabIndex = 2;
            this.measuredCurrent2Label.Text = "Current 2 (A)";
            // 
            // inCompliance2ButtonLed
            // 
            this.inCompliance2ButtonLed.Enabled = false;
            this.inCompliance2ButtonLed.Location = new System.Drawing.Point(71, 101);
            this.inCompliance2ButtonLed.Name = "inCompliance2ButtonLed";
            this.inCompliance2ButtonLed.Size = new System.Drawing.Size(21, 21);
            this.inCompliance2ButtonLed.TabIndex = 5;
            this.inCompliance2ButtonLed.UseVisualStyleBackColor = true;
            // 
            // measuredVoltage2Label
            // 
            this.measuredVoltage2Label.AutoSize = true;
            this.measuredVoltage2Label.Location = new System.Drawing.Point(6, 22);
            this.measuredVoltage2Label.Name = "measuredVoltage2Label";
            this.measuredVoltage2Label.Size = new System.Drawing.Size(68, 13);
            this.measuredVoltage2Label.TabIndex = 0;
            this.measuredVoltage2Label.Text = "Voltage 2 (V)";
            // 
            // inCompliance2Label
            // 
            this.inCompliance2Label.AutoSize = true;
            this.inCompliance2Label.Location = new System.Drawing.Point(40, 85);
            this.inCompliance2Label.Name = "inCompliance2Label";
            this.inCompliance2Label.Size = new System.Drawing.Size(83, 13);
            this.inCompliance2Label.TabIndex = 4;
            this.inCompliance2Label.Text = "In Compliance 2";
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 288);
            this.Controls.Add(this.measurements2GroupBox);
            this.Controls.Add(this.voltageLevel2GroupBox);
            this.Controls.Add(this.measurements1GroupBox);
            this.Controls.Add(this.voltageLevel1GroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.resourceNameAndChannelNameGroupBox);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Hardware-Timed Single-Point";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.sourceDelayNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fetchTimeoutNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevel1Numeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitNumeric)).EndInit();
            this.resourceNameAndChannelNameGroupBox.ResumeLayout(false);
            this.resourceNameAndChannelNameGroupBox.PerformLayout();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.voltageLevel1GroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevel2Numeric)).EndInit();
            this.measurements1GroupBox.ResumeLayout(false);
            this.measurements1GroupBox.PerformLayout();
            this.voltageLevel2GroupBox.ResumeLayout(false);
            this.measurements2GroupBox.ResumeLayout(false);
            this.measurements2GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label sourceDelayLabel;
        private System.Windows.Forms.Label voltageLevelRangeLabel;
        private System.Windows.Forms.Label fetchTimeoutLabel;
        private System.Windows.Forms.Label currentLimitRangeLabel;
        private System.Windows.Forms.Label currentLimitLabel;
        private System.Windows.Forms.NumericUpDown sourceDelayNumeric;
        private System.Windows.Forms.NumericUpDown voltageLevelRangeNumeric;
        private System.Windows.Forms.NumericUpDown fetchTimeoutNumeric;
        private System.Windows.Forms.NumericUpDown voltageLevel1Numeric;
        private System.Windows.Forms.NumericUpDown currentLimitRangeNumeric;
        private System.Windows.Forms.NumericUpDown currentLimitNumeric;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox resourceNameAndChannelNameGroupBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox voltageLevel1GroupBox;
        private System.Windows.Forms.NumericUpDown voltageLevel2Numeric;
        private System.Windows.Forms.GroupBox measurements1GroupBox;
        private System.Windows.Forms.TextBox voltage1MeasurementsTextBox;
        private System.Windows.Forms.TextBox current1MeasurementsTextBox;
        private System.Windows.Forms.Label measuredCurrent1Label;
        private System.Windows.Forms.Button inCompliance1ButtonLed;
        private System.Windows.Forms.Label measuredVoltage1Label;
        private System.Windows.Forms.Label inCompliance1Label;
        private System.Windows.Forms.GroupBox voltageLevel2GroupBox;
        private System.Windows.Forms.GroupBox measurements2GroupBox;
        private System.Windows.Forms.TextBox voltage2MeasurementsTextBox;
        private System.Windows.Forms.TextBox current2MeasurementsTextBox;
        private System.Windows.Forms.Label measuredCurrent2Label;
        private System.Windows.Forms.Button inCompliance2ButtonLed;
        private System.Windows.Forms.Label measuredVoltage2Label;
        private System.Windows.Forms.Label inCompliance2Label;
        private System.Windows.Forms.TextBox channelNameTextBox;

    }
}
