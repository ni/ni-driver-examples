namespace NationalInstruments.Examples.AdvancedPropertyAccess
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
            this.currentLimitLabel = new System.Windows.Forms.Label();
            this.currentLimitRangeLabel = new System.Windows.Forms.Label();
            this.voltageLevelLabel = new System.Windows.Forms.Label();
            this.voltageLevelRangeLabel = new System.Windows.Forms.Label();
            this.senseLabel = new System.Windows.Forms.Label();
            this.sourceDelayLabel = new System.Windows.Forms.Label();
            this.measuredVoltageLabel = new System.Windows.Forms.Label();
            this.measuredCurrentLabel = new System.Windows.Forms.Label();
            this.limitReachedLabel = new System.Windows.Forms.Label();
            this.currentLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentLimitRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageLevelRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.sourceDelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.senseComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameAndChannelNameGroupBox = new System.Windows.Forms.GroupBox();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.measurementsGroupBox = new System.Windows.Forms.GroupBox();
            this.voltageMeasurementsTextBox = new System.Windows.Forms.TextBox();
            this.currentMeasurementsTextBox = new System.Windows.Forms.TextBox();
            this.inComplianceButtonLed = new System.Windows.Forms.Button();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDelayNumeric)).BeginInit();
            this.resourceNameAndChannelNameGroupBox.SuspendLayout();
            this.measurementsGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // currentLimitLabel
            // 
            this.currentLimitLabel.AutoSize = true;
            this.currentLimitLabel.Location = new System.Drawing.Point(6, 75);
            this.currentLimitLabel.Name = "currentLimitLabel";
            this.currentLimitLabel.Size = new System.Drawing.Size(81, 13);
            this.currentLimitLabel.TabIndex = 4;
            this.currentLimitLabel.Text = "Current Limit (A)";
            // 
            // currentLimitRangeLabel
            // 
            this.currentLimitRangeLabel.AutoSize = true;
            this.currentLimitRangeLabel.Location = new System.Drawing.Point(6, 101);
            this.currentLimitRangeLabel.Name = "currentLimitRangeLabel";
            this.currentLimitRangeLabel.Size = new System.Drawing.Size(118, 13);
            this.currentLimitRangeLabel.TabIndex = 6;
            this.currentLimitRangeLabel.Text = "Current Limitl Range (A)";
            // 
            // voltageLevelLabel
            // 
            this.voltageLevelLabel.AutoSize = true;
            this.voltageLevelLabel.Location = new System.Drawing.Point(6, 23);
            this.voltageLevelLabel.Name = "voltageLevelLabel";
            this.voltageLevelLabel.Size = new System.Drawing.Size(88, 13);
            this.voltageLevelLabel.TabIndex = 0;
            this.voltageLevelLabel.Text = "Voltage Level (V)";
            // 
            // voltageLevelRangeLabel
            // 
            this.voltageLevelRangeLabel.AutoSize = true;
            this.voltageLevelRangeLabel.Location = new System.Drawing.Point(6, 49);
            this.voltageLevelRangeLabel.Name = "voltageLevelRangeLabel";
            this.voltageLevelRangeLabel.Size = new System.Drawing.Size(123, 13);
            this.voltageLevelRangeLabel.TabIndex = 2;
            this.voltageLevelRangeLabel.Text = "Voltage Level Range (V)";
            // 
            // senseLabel
            // 
            this.senseLabel.AutoSize = true;
            this.senseLabel.Location = new System.Drawing.Point(6, 127);
            this.senseLabel.Name = "senseLabel";
            this.senseLabel.Size = new System.Drawing.Size(37, 13);
            this.senseLabel.TabIndex = 8;
            this.senseLabel.Text = "Sense";
            // 
            // sourceDelayLabel
            // 
            this.sourceDelayLabel.AutoSize = true;
            this.sourceDelayLabel.Location = new System.Drawing.Point(6, 154);
            this.sourceDelayLabel.Name = "sourceDelayLabel";
            this.sourceDelayLabel.Size = new System.Drawing.Size(85, 13);
            this.sourceDelayLabel.TabIndex = 10;
            this.sourceDelayLabel.Text = "Source Delay (s)";
            // 
            // measuredVoltageLabel
            // 
            this.measuredVoltageLabel.AutoSize = true;
            this.measuredVoltageLabel.Location = new System.Drawing.Point(6, 23);
            this.measuredVoltageLabel.Name = "measuredVoltageLabel";
            this.measuredVoltageLabel.Size = new System.Drawing.Size(59, 13);
            this.measuredVoltageLabel.TabIndex = 0;
            this.measuredVoltageLabel.Text = "Voltage (V)";
            // 
            // measuredCurrentLabel
            // 
            this.measuredCurrentLabel.AutoSize = true;
            this.measuredCurrentLabel.Location = new System.Drawing.Point(6, 49);
            this.measuredCurrentLabel.Name = "measuredCurrentLabel";
            this.measuredCurrentLabel.Size = new System.Drawing.Size(57, 13);
            this.measuredCurrentLabel.TabIndex = 2;
            this.measuredCurrentLabel.Text = "Current (A)";
            // 
            // limitReachedLabel
            // 
            this.limitReachedLabel.AutoSize = true;
            this.limitReachedLabel.Location = new System.Drawing.Point(30, 85);
            this.limitReachedLabel.Name = "limitReachedLabel";
            this.limitReachedLabel.Size = new System.Drawing.Size(135, 13);
            this.limitReachedLabel.TabIndex = 4;
            this.limitReachedLabel.Text = "Compliance/Limit Reached";
            // 
            // currentLimitNumeric
            // 
            this.currentLimitNumeric.DecimalPlaces = 6;
            this.currentLimitNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.currentLimitNumeric.Location = new System.Drawing.Point(140, 71);
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
            this.currentLimitNumeric.Size = new System.Drawing.Size(91, 20);
            this.currentLimitNumeric.TabIndex = 5;
            this.currentLimitNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            131072});
            // 
            // currentLimitRangeNumeric
            // 
            this.currentLimitRangeNumeric.DecimalPlaces = 6;
            this.currentLimitRangeNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.currentLimitRangeNumeric.Location = new System.Drawing.Point(140, 97);
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
            this.currentLimitRangeNumeric.Size = new System.Drawing.Size(91, 20);
            this.currentLimitRangeNumeric.TabIndex = 7;
            this.currentLimitRangeNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            131072});
            // 
            // voltageLevelNumeric
            // 
            this.voltageLevelNumeric.DecimalPlaces = 6;
            this.voltageLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.voltageLevelNumeric.Location = new System.Drawing.Point(140, 19);
            this.voltageLevelNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.voltageLevelNumeric.Name = "voltageLevelNumeric";
            this.voltageLevelNumeric.Size = new System.Drawing.Size(91, 20);
            this.voltageLevelNumeric.TabIndex = 1;
            this.voltageLevelNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // voltageLevelRangeNumeric
            // 
            this.voltageLevelRangeNumeric.DecimalPlaces = 6;
            this.voltageLevelRangeNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.voltageLevelRangeNumeric.Location = new System.Drawing.Point(140, 45);
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
            this.voltageLevelRangeNumeric.Size = new System.Drawing.Size(91, 20);
            this.voltageLevelRangeNumeric.TabIndex = 3;
            this.voltageLevelRangeNumeric.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // sourceDelayNumeric
            // 
            this.sourceDelayNumeric.DecimalPlaces = 6;
            this.sourceDelayNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.sourceDelayNumeric.Location = new System.Drawing.Point(140, 150);
            this.sourceDelayNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.sourceDelayNumeric.Name = "sourceDelayNumeric";
            this.sourceDelayNumeric.Size = new System.Drawing.Size(91, 20);
            this.sourceDelayNumeric.TabIndex = 11;
            this.sourceDelayNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(342, 228);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // senseComboBox
            // 
            this.senseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.senseComboBox.Location = new System.Drawing.Point(140, 123);
            this.senseComboBox.Name = "senseComboBox";
            this.senseComboBox.Size = new System.Drawing.Size(91, 21);
            this.senseComboBox.TabIndex = 9;
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
            this.channelNameTextBox.TabIndex = 3;
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
            // measurementsGroupBox
            // 
            this.measurementsGroupBox.Controls.Add(this.voltageMeasurementsTextBox);
            this.measurementsGroupBox.Controls.Add(this.currentMeasurementsTextBox);
            this.measurementsGroupBox.Controls.Add(this.measuredCurrentLabel);
            this.measurementsGroupBox.Controls.Add(this.inComplianceButtonLed);
            this.measurementsGroupBox.Controls.Add(this.measuredVoltageLabel);
            this.measurementsGroupBox.Controls.Add(this.limitReachedLabel);
            this.measurementsGroupBox.Location = new System.Drawing.Point(265, 12);
            this.measurementsGroupBox.Name = "measurementsGroupBox";
            this.measurementsGroupBox.Size = new System.Drawing.Size(221, 132);
            this.measurementsGroupBox.TabIndex = 2;
            this.measurementsGroupBox.TabStop = false;
            this.measurementsGroupBox.Text = "Measurements";
            // 
            // voltageMeasurementsTextBox
            // 
            this.voltageMeasurementsTextBox.Location = new System.Drawing.Point(115, 19);
            this.voltageMeasurementsTextBox.Name = "voltageMeasurementsTextBox";
            this.voltageMeasurementsTextBox.ReadOnly = true;
            this.voltageMeasurementsTextBox.Size = new System.Drawing.Size(100, 20);
            this.voltageMeasurementsTextBox.TabIndex = 1;
            this.voltageMeasurementsTextBox.Text = "0.000000E+000";
            // 
            // currentMeasurementsTextBox
            // 
            this.currentMeasurementsTextBox.Location = new System.Drawing.Point(115, 45);
            this.currentMeasurementsTextBox.Name = "currentMeasurementsTextBox";
            this.currentMeasurementsTextBox.ReadOnly = true;
            this.currentMeasurementsTextBox.Size = new System.Drawing.Size(100, 20);
            this.currentMeasurementsTextBox.TabIndex = 3;
            this.currentMeasurementsTextBox.Text = "0.000000E+000";
            // 
            // inComplianceButtonLed
            // 
            this.inComplianceButtonLed.Enabled = false;
            this.inComplianceButtonLed.Location = new System.Drawing.Point(87, 101);
            this.inComplianceButtonLed.Name = "inComplianceButtonLed";
            this.inComplianceButtonLed.Size = new System.Drawing.Size(21, 21);
            this.inComplianceButtonLed.TabIndex = 5;
            this.inComplianceButtonLed.UseVisualStyleBackColor = true;
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.voltageLevelLabel);
            this.configurationGroupBox.Controls.Add(this.senseComboBox);
            this.configurationGroupBox.Controls.Add(this.sourceDelayNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLimitLabel);
            this.configurationGroupBox.Controls.Add(this.senseLabel);
            this.configurationGroupBox.Controls.Add(this.currentLimitNumeric);
            this.configurationGroupBox.Controls.Add(this.voltageLevelRangeNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLimitRangeLabel);
            this.configurationGroupBox.Controls.Add(this.voltageLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLimitRangeNumeric);
            this.configurationGroupBox.Controls.Add(this.voltageLevelRangeLabel);
            this.configurationGroupBox.Controls.Add(this.sourceDelayLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 101);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(237, 180);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 292);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.measurementsGroupBox);
            this.Controls.Add(this.resourceNameAndChannelNameGroupBox);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Source DC Voltage using Advanced Property Access";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDelayNumeric)).EndInit();
            this.resourceNameAndChannelNameGroupBox.ResumeLayout(false);
            this.resourceNameAndChannelNameGroupBox.PerformLayout();
            this.measurementsGroupBox.ResumeLayout(false);
            this.measurementsGroupBox.PerformLayout();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label currentLimitLabel;
        private System.Windows.Forms.Label currentLimitRangeLabel;
        private System.Windows.Forms.Label voltageLevelLabel;
        private System.Windows.Forms.Label voltageLevelRangeLabel;
        private System.Windows.Forms.Label senseLabel;
        private System.Windows.Forms.Label sourceDelayLabel;
        private System.Windows.Forms.Label measuredVoltageLabel;
        private System.Windows.Forms.Label measuredCurrentLabel;
        private System.Windows.Forms.Label limitReachedLabel;
        private System.Windows.Forms.NumericUpDown currentLimitNumeric;
        private System.Windows.Forms.NumericUpDown currentLimitRangeNumeric;
        private System.Windows.Forms.NumericUpDown voltageLevelNumeric;
        private System.Windows.Forms.NumericUpDown voltageLevelRangeNumeric;
        private System.Windows.Forms.NumericUpDown sourceDelayNumeric;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ComboBox senseComboBox;
        private System.Windows.Forms.GroupBox resourceNameAndChannelNameGroupBox;
        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.GroupBox measurementsGroupBox;
        private System.Windows.Forms.Button inComplianceButtonLed;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.TextBox voltageMeasurementsTextBox;
        private System.Windows.Forms.TextBox currentMeasurementsTextBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox channelNameTextBox;

    }
}
