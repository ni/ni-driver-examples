namespace NationalInstruments.Examples.SinkDCVoltageWithOutputResistance
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
            this.sourceDelayLabel = new System.Windows.Forms.Label();
            this.voltageMeasurementLabel = new System.Windows.Forms.Label();
            this.currentMeasurementLabel = new System.Windows.Forms.Label();
            this.inComplianceLabel = new System.Windows.Forms.Label();
            this.currentLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentLimitRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageLevelRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.outputResistanceLabel = new System.Windows.Forms.Label();
            this.outputResistanceNumeric = new System.Windows.Forms.NumericUpDown();
            this.sourceDelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.resourceNameAndChannelNameGroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.measurementsGroupBox = new System.Windows.Forms.GroupBox();
            this.voltageMeasurementTextBox = new System.Windows.Forms.TextBox();
            this.currentMeasurementTextBox = new System.Windows.Forms.TextBox();
            this.inComplianceButtonLed = new System.Windows.Forms.Button();
            this.outputShortedLabel = new System.Windows.Forms.Label();
            this.outputShortedCheckBox = new System.Windows.Forms.CheckBox();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputResistanceNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDelayNumeric)).BeginInit();
            this.resourceNameAndChannelNameGroupBox.SuspendLayout();
            this.measurementsGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // currentLimitLabel
            //
            this.currentLimitLabel.AutoSize = true;
            this.currentLimitLabel.Location = new System.Drawing.Point(6, 76);
            this.currentLimitLabel.Name = "currentLimitLabel";
            this.currentLimitLabel.Size = new System.Drawing.Size(95, 15);
            this.currentLimitLabel.TabIndex = 4;
            this.currentLimitLabel.Text = "Current Limit (A)";
            //
            // currentLimitRangeLabel
            //
            this.currentLimitRangeLabel.AutoSize = true;
            this.currentLimitRangeLabel.Location = new System.Drawing.Point(6, 102);
            this.currentLimitRangeLabel.Name = "currentLimitRangeLabel";
            this.currentLimitRangeLabel.Size = new System.Drawing.Size(135, 15);
            this.currentLimitRangeLabel.TabIndex = 6;
            this.currentLimitRangeLabel.Text = "Current Limit Range (A)";
            //
            // voltageLevelLabel
            //
            this.voltageLevelLabel.AutoSize = true;
            this.voltageLevelLabel.Location = new System.Drawing.Point(6, 23);
            this.voltageLevelLabel.Name = "voltageLevelLabel";
            this.voltageLevelLabel.Size = new System.Drawing.Size(98, 15);
            this.voltageLevelLabel.TabIndex = 0;
            this.voltageLevelLabel.Text = "Voltage Level (V)";
            //
            // voltageLevelRangeLabel
            //
            this.voltageLevelRangeLabel.AutoSize = true;
            this.voltageLevelRangeLabel.Location = new System.Drawing.Point(6, 49);
            this.voltageLevelRangeLabel.Name = "voltageLevelRangeLabel";
            this.voltageLevelRangeLabel.Size = new System.Drawing.Size(138, 15);
            this.voltageLevelRangeLabel.TabIndex = 2;
            this.voltageLevelRangeLabel.Text = "Voltage Level Range (V)";
            //
            // sourceDelayLabel
            //
            this.sourceDelayLabel.AutoSize = true;
            this.sourceDelayLabel.Location = new System.Drawing.Point(6, 153);
            this.sourceDelayLabel.Name = "sourceDelayLabel";
            this.sourceDelayLabel.Size = new System.Drawing.Size(97, 15);
            this.sourceDelayLabel.TabIndex = 10;
            this.sourceDelayLabel.Text = "Source Delay (s)";
            //
            // voltageMeasurementLabel
            //
            this.voltageMeasurementLabel.AutoSize = true;
            this.voltageMeasurementLabel.Location = new System.Drawing.Point(6, 23);
            this.voltageMeasurementLabel.Name = "voltageMeasurementLabel";
            this.voltageMeasurementLabel.Size = new System.Drawing.Size(66, 15);
            this.voltageMeasurementLabel.TabIndex = 0;
            this.voltageMeasurementLabel.Text = "Voltage (V)";
            //
            // currentMeasurementLabel
            //
            this.currentMeasurementLabel.AutoSize = true;
            this.currentMeasurementLabel.Location = new System.Drawing.Point(6, 49);
            this.currentMeasurementLabel.Name = "currentMeasurementLabel";
            this.currentMeasurementLabel.Size = new System.Drawing.Size(65, 15);
            this.currentMeasurementLabel.TabIndex = 2;
            this.currentMeasurementLabel.Text = "Current (A)";
            //
            // inComplianceLabel
            //
            this.inComplianceLabel.AutoSize = true;
            this.inComplianceLabel.Location = new System.Drawing.Point(30, 85);
            this.inComplianceLabel.Name = "inComplianceLabel";
            this.inComplianceLabel.Size = new System.Drawing.Size(156, 15);
            this.inComplianceLabel.TabIndex = 4;
            this.inComplianceLabel.Text = "Compliance/Limit Reached";
            //
            // currentLimitNumeric
            //
            this.currentLimitNumeric.DecimalPlaces = 6;
            this.currentLimitNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.currentLimitNumeric.Location = new System.Drawing.Point(160, 72);
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
            4,
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
            this.currentLimitRangeNumeric.Location = new System.Drawing.Point(160, 98);
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
            4,
            0,
            0,
            0});
            //
            // voltageLevelNumeric
            //
            this.voltageLevelNumeric.DecimalPlaces = 6;
            this.voltageLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.voltageLevelNumeric.Location = new System.Drawing.Point(160, 19);
            this.voltageLevelNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.voltageLevelNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.voltageLevelNumeric.Name = "voltageLevelNumeric";
            this.voltageLevelNumeric.Size = new System.Drawing.Size(91, 20);
            this.voltageLevelNumeric.TabIndex = 1;
            this.voltageLevelNumeric.Value = new decimal(new int[] {
            1,
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
            this.voltageLevelRangeNumeric.Location = new System.Drawing.Point(160, 45);
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
            // outputResistanceLabel
            //
            this.outputResistanceLabel.AutoSize = true;
            this.outputResistanceLabel.Location = new System.Drawing.Point(6, 127);
            this.outputResistanceLabel.Name = "outputResistanceLabel";
            this.outputResistanceLabel.Size = new System.Drawing.Size(151, 15);
            this.outputResistanceLabel.TabIndex = 8;
            this.outputResistanceLabel.Text = "Output Resistance (Ohms)";
            //
            // outputResistanceNumeric
            //
            this.outputResistanceNumeric.DecimalPlaces = 6;
            this.outputResistanceNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.outputResistanceNumeric.Location = new System.Drawing.Point(160, 125);
            this.outputResistanceNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.outputResistanceNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.outputResistanceNumeric.Name = "outputResistanceNumeric";
            this.outputResistanceNumeric.Size = new System.Drawing.Size(91, 20);
            this.outputResistanceNumeric.TabIndex = 9;
            this.outputResistanceNumeric.Value = new decimal(new int[] {
            1,
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
            this.sourceDelayNumeric.Location = new System.Drawing.Point(160, 151);
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
            65536});
            //
            // startButton
            //
            this.startButton.Location = new System.Drawing.Point(345, 278);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            //
            // resourceNameAndChannelNameGroupBox
            //
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.resourceNameComboBox);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.resourceNameLabel);
            this.resourceNameAndChannelNameGroupBox.Location = new System.Drawing.Point(12, 12);
            this.resourceNameAndChannelNameGroupBox.Name = "resourceNameAndChannelNameGroupBox";
            this.resourceNameAndChannelNameGroupBox.Size = new System.Drawing.Size(257, 73);
            this.resourceNameAndChannelNameGroupBox.TabIndex = 0;
            this.resourceNameAndChannelNameGroupBox.TabStop = false;
            this.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name";
            //
            // resourceNameComboBox
            //
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(8, 39);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(243, 21);
            this.resourceNameComboBox.TabIndex = 1;
            //
            // resourceNameLabel
            //
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 22);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(97, 15);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            //
            // measurementsGroupBox
            //
            this.measurementsGroupBox.Controls.Add(this.voltageMeasurementTextBox);
            this.measurementsGroupBox.Controls.Add(this.currentMeasurementTextBox);
            this.measurementsGroupBox.Controls.Add(this.currentMeasurementLabel);
            this.measurementsGroupBox.Controls.Add(this.inComplianceButtonLed);
            this.measurementsGroupBox.Controls.Add(this.voltageMeasurementLabel);
            this.measurementsGroupBox.Controls.Add(this.inComplianceLabel);
            this.measurementsGroupBox.Location = new System.Drawing.Point(285, 12);
            this.measurementsGroupBox.Name = "measurementsGroupBox";
            this.measurementsGroupBox.Size = new System.Drawing.Size(194, 132);
            this.measurementsGroupBox.TabIndex = 2;
            this.measurementsGroupBox.TabStop = false;
            this.measurementsGroupBox.Text = "Measurements";
            //
            // voltageMeasurementTextBox
            //
            this.voltageMeasurementTextBox.Location = new System.Drawing.Point(88, 19);
            this.voltageMeasurementTextBox.Name = "voltageMeasurementTextBox";
            this.voltageMeasurementTextBox.ReadOnly = true;
            this.voltageMeasurementTextBox.Size = new System.Drawing.Size(100, 20);
            this.voltageMeasurementTextBox.TabIndex = 1;
            this.voltageMeasurementTextBox.Text = "0.000000E+000";
            //
            // currentMeasurementTextBox
            //
            this.currentMeasurementTextBox.Location = new System.Drawing.Point(88, 45);
            this.currentMeasurementTextBox.Name = "currentMeasurementTextBox";
            this.currentMeasurementTextBox.ReadOnly = true;
            this.currentMeasurementTextBox.Size = new System.Drawing.Size(100, 20);
            this.currentMeasurementTextBox.TabIndex = 3;
            this.currentMeasurementTextBox.Text = "0.000000E+000";
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
            // outputShortedLabel
            //
            this.outputShortedLabel.AutoSize = true;
            this.outputShortedLabel.Location = new System.Drawing.Point(6, 179);
            this.outputShortedLabel.Name = "outputShortedLabel";
            this.outputShortedLabel.Size = new System.Drawing.Size(89, 15);
            this.outputShortedLabel.TabIndex = 12;
            this.outputShortedLabel.Text = "Output Shorted";
            //
            // outputShortedCheckBox
            //
            this.outputShortedCheckBox.Location = new System.Drawing.Point(160, 177);
            this.outputShortedCheckBox.Name = "outputShortedCheckBox";
            this.outputShortedCheckBox.Size = new System.Drawing.Size(91, 21);
            this.outputShortedCheckBox.TabIndex = 13;
            //
            // configurationGroupBox
            //
            this.configurationGroupBox.Controls.Add(this.outputResistanceNumeric);
            this.configurationGroupBox.Controls.Add(this.outputResistanceLabel);
            this.configurationGroupBox.Controls.Add(this.voltageLevelLabel);
            this.configurationGroupBox.Controls.Add(this.sourceDelayNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLimitLabel);
            this.configurationGroupBox.Controls.Add(this.voltageLevelRangeNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLimitRangeLabel);
            this.configurationGroupBox.Controls.Add(this.voltageLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLimitRangeNumeric);
            this.configurationGroupBox.Controls.Add(this.voltageLevelRangeLabel);
            this.configurationGroupBox.Controls.Add(this.currentLimitNumeric);
            this.configurationGroupBox.Controls.Add(this.sourceDelayLabel);
            this.configurationGroupBox.Controls.Add(this.outputShortedLabel);
            this.configurationGroupBox.Controls.Add(this.outputShortedCheckBox);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 101);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(257, 214);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            //
            // MainForm
            //
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 328);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.measurementsGroupBox);
            this.Controls.Add(this.resourceNameAndChannelNameGroupBox);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Sink DC Voltage with Output Resistance";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputResistanceNumeric)).EndInit();
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

        private System.Windows.Forms.NumericUpDown currentLimitNumeric;
        private System.Windows.Forms.NumericUpDown currentLimitRangeNumeric;
        private System.Windows.Forms.NumericUpDown voltageLevelNumeric;
        private System.Windows.Forms.NumericUpDown voltageLevelRangeNumeric;
        private System.Windows.Forms.NumericUpDown sourceDelayNumeric;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox resourceNameAndChannelNameGroupBox;
        private System.Windows.Forms.GroupBox measurementsGroupBox;
        private System.Windows.Forms.Button inComplianceButtonLed;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.TextBox voltageMeasurementTextBox;
        private System.Windows.Forms.TextBox currentMeasurementTextBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label currentLimitLabel;
        private System.Windows.Forms.Label currentLimitRangeLabel;
        private System.Windows.Forms.Label voltageLevelLabel;
        private System.Windows.Forms.Label voltageLevelRangeLabel;
        private System.Windows.Forms.Label outputResistanceLabel;
        private System.Windows.Forms.Label sourceDelayLabel;
        private System.Windows.Forms.Label voltageMeasurementLabel;
        private System.Windows.Forms.Label currentMeasurementLabel;
        private System.Windows.Forms.Label inComplianceLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.NumericUpDown outputResistanceNumeric;
        private System.Windows.Forms.Label outputShortedLabel;
        private System.Windows.Forms.CheckBox outputShortedCheckBox;

    }
}
