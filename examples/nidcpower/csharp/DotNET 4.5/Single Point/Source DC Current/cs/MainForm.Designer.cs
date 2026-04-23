namespace NationalInstruments.Examples.SourceDCCurrent
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
            this.currentLevelLabel = new System.Windows.Forms.Label();
            this.currentLevelRangeLabel = new System.Windows.Forms.Label();
            this.voltageLimitLabel = new System.Windows.Forms.Label();
            this.voltageLimitRangeLabel = new System.Windows.Forms.Label();
            this.sourceDelayLabel = new System.Windows.Forms.Label();
            this.voltageMeasurementLabel = new System.Windows.Forms.Label();
            this.currentMeasurementLabel = new System.Windows.Forms.Label();
            this.inComplianceLabel = new System.Windows.Forms.Label();
            this.currentLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentLevelRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageLimitRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.sourceDelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLimitNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLimitRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDelayNumeric)).BeginInit();
            this.resourceNameAndChannelNameGroupBox.SuspendLayout();
            this.measurementsGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // currentLevelLabel
            // 
            this.currentLevelLabel.AutoSize = true;
            this.currentLevelLabel.Location = new System.Drawing.Point(6, 23);
            this.currentLevelLabel.Name = "currentLevelLabel";
            this.currentLevelLabel.Size = new System.Drawing.Size(86, 13);
            this.currentLevelLabel.TabIndex = 0;
            this.currentLevelLabel.Text = "Current Level (A)";
            // 
            // currentLevelRangeLabel
            // 
            this.currentLevelRangeLabel.AutoSize = true;
            this.currentLevelRangeLabel.Location = new System.Drawing.Point(6, 49);
            this.currentLevelRangeLabel.Name = "currentLevelRangeLabel";
            this.currentLevelRangeLabel.Size = new System.Drawing.Size(121, 13);
            this.currentLevelRangeLabel.TabIndex = 2;
            this.currentLevelRangeLabel.Text = "Current Level Range (A)";
            // 
            // voltageLimitLabel
            // 
            this.voltageLimitLabel.AutoSize = true;
            this.voltageLimitLabel.Location = new System.Drawing.Point(6, 75);
            this.voltageLimitLabel.Name = "voltageLimitLabel";
            this.voltageLimitLabel.Size = new System.Drawing.Size(83, 13);
            this.voltageLimitLabel.TabIndex = 4;
            this.voltageLimitLabel.Text = "Voltage Limit (V)";
            // 
            // voltageLimitRangeLabel
            // 
            this.voltageLimitRangeLabel.AutoSize = true;
            this.voltageLimitRangeLabel.Location = new System.Drawing.Point(6, 101);
            this.voltageLimitRangeLabel.Name = "voltageLimitRangeLabel";
            this.voltageLimitRangeLabel.Size = new System.Drawing.Size(118, 13);
            this.voltageLimitRangeLabel.TabIndex = 6;
            this.voltageLimitRangeLabel.Text = "Voltage Limit Range (V)";
            // 
            // sourceDelayLabel
            // 
            this.sourceDelayLabel.AutoSize = true;
            this.sourceDelayLabel.Location = new System.Drawing.Point(6, 130);
            this.sourceDelayLabel.Name = "sourceDelayLabel";
            this.sourceDelayLabel.Size = new System.Drawing.Size(85, 13);
            this.sourceDelayLabel.TabIndex = 10;
            this.sourceDelayLabel.Text = "Source Delay (s)";
            // 
            // voltageMeasurementLabel
            // 
            this.voltageMeasurementLabel.AutoSize = true;
            this.voltageMeasurementLabel.Location = new System.Drawing.Point(6, 49);
            this.voltageMeasurementLabel.Name = "voltageMeasurementLabel";
            this.voltageMeasurementLabel.Size = new System.Drawing.Size(59, 13);
            this.voltageMeasurementLabel.TabIndex = 2;
            this.voltageMeasurementLabel.Text = "Voltage (V)";
            // 
            // currentMeasurementLabel
            // 
            this.currentMeasurementLabel.AutoSize = true;
            this.currentMeasurementLabel.Location = new System.Drawing.Point(6, 23);
            this.currentMeasurementLabel.Name = "currentMeasurementLabel";
            this.currentMeasurementLabel.Size = new System.Drawing.Size(57, 13);
            this.currentMeasurementLabel.TabIndex = 0;
            this.currentMeasurementLabel.Text = "Current (A)";
            // 
            // inComplianceLabel
            // 
            this.inComplianceLabel.AutoSize = true;
            this.inComplianceLabel.Location = new System.Drawing.Point(30, 85);
            this.inComplianceLabel.Name = "inComplianceLabel";
            this.inComplianceLabel.Size = new System.Drawing.Size(135, 13);
            this.inComplianceLabel.TabIndex = 4;
            this.inComplianceLabel.Text = "Compliance/Limit Reached";
            // 
            // currentLevelNumeric
            // 
            this.currentLevelNumeric.DecimalPlaces = 6;
            this.currentLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.currentLevelNumeric.Location = new System.Drawing.Point(160, 19);
            this.currentLevelNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.currentLevelNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.currentLevelNumeric.Name = "currentLevelNumeric";
            this.currentLevelNumeric.Size = new System.Drawing.Size(91, 20);
            this.currentLevelNumeric.TabIndex = 1;
            this.currentLevelNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // currentLevelRangeNumeric
            // 
            this.currentLevelRangeNumeric.DecimalPlaces = 6;
            this.currentLevelRangeNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.currentLevelRangeNumeric.Location = new System.Drawing.Point(160, 45);
            this.currentLevelRangeNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.currentLevelRangeNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.currentLevelRangeNumeric.Name = "currentLevelRangeNumeric";
            this.currentLevelRangeNumeric.Size = new System.Drawing.Size(91, 20);
            this.currentLevelRangeNumeric.TabIndex = 3;
            this.currentLevelRangeNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // voltageLimitNumeric
            // 
            this.voltageLimitNumeric.DecimalPlaces = 6;
            this.voltageLimitNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.voltageLimitNumeric.Location = new System.Drawing.Point(160, 71);
            this.voltageLimitNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.voltageLimitNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.voltageLimitNumeric.Name = "voltageLimitNumeric";
            this.voltageLimitNumeric.Size = new System.Drawing.Size(91, 20);
            this.voltageLimitNumeric.TabIndex = 5;
            this.voltageLimitNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // voltageLimitRangeNumeric
            // 
            this.voltageLimitRangeNumeric.DecimalPlaces = 6;
            this.voltageLimitRangeNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.voltageLimitRangeNumeric.Location = new System.Drawing.Point(160, 97);
            this.voltageLimitRangeNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.voltageLimitRangeNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.voltageLimitRangeNumeric.Name = "voltageLimitRangeNumeric";
            this.voltageLimitRangeNumeric.Size = new System.Drawing.Size(91, 20);
            this.voltageLimitRangeNumeric.TabIndex = 7;
            this.voltageLimitRangeNumeric.Value = new decimal(new int[] {
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
            this.sourceDelayNumeric.Location = new System.Drawing.Point(160, 128);
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
            this.startButton.Location = new System.Drawing.Point(345, 226);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 3;
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
            this.resourceNameAndChannelNameGroupBox.Size = new System.Drawing.Size(257, 73);
            this.resourceNameAndChannelNameGroupBox.TabIndex = 0;
            this.resourceNameAndChannelNameGroupBox.TabStop = false;
            this.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name";
            // 
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(160, 45);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(91, 20);
            this.channelNameTextBox.TabIndex = 3;
            this.channelNameTextBox.Text = "0";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(126, 18);
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
            this.measurementsGroupBox.Controls.Add(this.inComplianceButtonLed);
            this.measurementsGroupBox.Controls.Add(this.currentMeasurementLabel);
            this.measurementsGroupBox.Controls.Add(this.voltageMeasurementLabel);
            this.measurementsGroupBox.Controls.Add(this.inComplianceLabel);
            this.measurementsGroupBox.Location = new System.Drawing.Point(285, 12);
            this.measurementsGroupBox.Name = "measurementsGroupBox";
            this.measurementsGroupBox.Size = new System.Drawing.Size(194, 132);
            this.measurementsGroupBox.TabIndex = 2;
            this.measurementsGroupBox.TabStop = false;
            this.measurementsGroupBox.Text = "Measurements";
            // 
            // voltageMeasurementsTextBox
            // 
            this.voltageMeasurementsTextBox.Location = new System.Drawing.Point(88, 45);
            this.voltageMeasurementsTextBox.Name = "voltageMeasurementsTextBox";
            this.voltageMeasurementsTextBox.ReadOnly = true;
            this.voltageMeasurementsTextBox.Size = new System.Drawing.Size(100, 20);
            this.voltageMeasurementsTextBox.TabIndex = 3;
            this.voltageMeasurementsTextBox.Text = "0.000000E+000";
            // 
            // currentMeasurementsTextBox
            // 
            this.currentMeasurementsTextBox.Location = new System.Drawing.Point(88, 19);
            this.currentMeasurementsTextBox.Name = "currentMeasurementsTextBox";
            this.currentMeasurementsTextBox.ReadOnly = true;
            this.currentMeasurementsTextBox.Size = new System.Drawing.Size(100, 20);
            this.currentMeasurementsTextBox.TabIndex = 1;
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
            this.configurationGroupBox.Controls.Add(this.sourceDelayNumeric);
            this.configurationGroupBox.Controls.Add(this.voltageLimitLabel);
            this.configurationGroupBox.Controls.Add(this.currentLevelLabel);
            this.configurationGroupBox.Controls.Add(this.voltageLimitRangeNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLevelRangeLabel);
            this.configurationGroupBox.Controls.Add(this.currentLevelRangeNumeric);
            this.configurationGroupBox.Controls.Add(this.voltageLimitRangeLabel);
            this.configurationGroupBox.Controls.Add(this.voltageLimitNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.sourceDelayLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 101);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(257, 159);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 277);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.measurementsGroupBox);
            this.Controls.Add(this.resourceNameAndChannelNameGroupBox);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Source DC Current";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLimitNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLimitRangeNumeric)).EndInit();
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

        private System.Windows.Forms.Label currentLevelLabel;
        private System.Windows.Forms.Label currentLevelRangeLabel;
        private System.Windows.Forms.Label voltageLimitLabel;
        private System.Windows.Forms.Label voltageLimitRangeLabel;
        private System.Windows.Forms.Label sourceDelayLabel;
        private System.Windows.Forms.Label voltageMeasurementLabel;
        private System.Windows.Forms.Label currentMeasurementLabel;
        private System.Windows.Forms.Label inComplianceLabel;
        private System.Windows.Forms.NumericUpDown currentLevelNumeric;
        private System.Windows.Forms.NumericUpDown currentLevelRangeNumeric;
        private System.Windows.Forms.NumericUpDown voltageLimitNumeric;
        private System.Windows.Forms.NumericUpDown voltageLimitRangeNumeric;
        private System.Windows.Forms.NumericUpDown sourceDelayNumeric;
        private System.Windows.Forms.Button startButton;
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
