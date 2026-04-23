namespace NationalInstruments.Examples.SinkDCCurrentIntoElectronicLoad
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
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.currentLevelLabel = new System.Windows.Forms.Label();
            this.currentLevelRangeLabel = new System.Windows.Forms.Label();
            this.voltageLimitRangeLabel = new System.Windows.Forms.Label();
            this.sourceDelayLabel = new System.Windows.Forms.Label();
            this.outputShortedLabel = new System.Windows.Forms.Label();
            this.conductionVoltageModeLabel = new System.Windows.Forms.Label();
            this.conductionVoltageOnThresholdLabel = new System.Windows.Forms.Label();
            this.conductionVoltageOffThresholdLabel = new System.Windows.Forms.Label();
            this.currentLevelRisingSlewRateLabel = new System.Windows.Forms.Label();
            this.currentLevelFallingSlewRateLabel = new System.Windows.Forms.Label();
            this.currentLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentLevelRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageLimitRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.sourceDelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.outputShortedCheckBox = new System.Windows.Forms.CheckBox();
            this.conductionVoltageModeComboBox = new System.Windows.Forms.ComboBox();
            this.conductionVoltageOnThresholdNumeric = new System.Windows.Forms.NumericUpDown();
            this.conductionVoltageOffThresholdNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentLevelRisingSlewRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentLevelFallingSlewRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageMeasurementLabel = new System.Windows.Forms.Label();
            this.currentMeasurementLabel = new System.Windows.Forms.Label();
            this.inComplianceLabel = new System.Windows.Forms.Label();
            this.measurementResultGroupBox = new System.Windows.Forms.GroupBox();
            this.currentMeasurementTextBox = new System.Windows.Forms.TextBox();
            this.voltageMeasurementTextBox = new System.Windows.Forms.TextBox();
            this.inComplianceButtonLed = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.resourceNameAndChannelNameGroupBox = new System.Windows.Forms.GroupBox();
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLimitRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDelayNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.conductionVoltageOnThresholdNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.conductionVoltageOffThresholdNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelRisingSlewRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelFallingSlewRateNumeric)).BeginInit();
            this.measurementResultGroupBox.SuspendLayout();
            this.resourceNameAndChannelNameGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.currentLevelLabel);
            this.configurationGroupBox.Controls.Add(this.currentLevelRangeLabel);
            this.configurationGroupBox.Controls.Add(this.voltageLimitRangeLabel);
            this.configurationGroupBox.Controls.Add(this.sourceDelayLabel);
            this.configurationGroupBox.Controls.Add(this.outputShortedLabel);
            this.configurationGroupBox.Controls.Add(this.conductionVoltageModeLabel);
            this.configurationGroupBox.Controls.Add(this.conductionVoltageOnThresholdLabel);
            this.configurationGroupBox.Controls.Add(this.conductionVoltageOffThresholdLabel);
            this.configurationGroupBox.Controls.Add(this.currentLevelRisingSlewRateLabel);
            this.configurationGroupBox.Controls.Add(this.currentLevelFallingSlewRateLabel);
            this.configurationGroupBox.Controls.Add(this.currentLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLevelRangeNumeric);
            this.configurationGroupBox.Controls.Add(this.voltageLimitRangeNumeric);
            this.configurationGroupBox.Controls.Add(this.sourceDelayNumeric);
            this.configurationGroupBox.Controls.Add(this.outputShortedCheckBox);
            this.configurationGroupBox.Controls.Add(this.conductionVoltageModeComboBox);
            this.configurationGroupBox.Controls.Add(this.conductionVoltageOnThresholdNumeric);
            this.configurationGroupBox.Controls.Add(this.conductionVoltageOffThresholdNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLevelRisingSlewRateNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLevelFallingSlewRateNumeric);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 97);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(360, 282);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(8, 22);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(8, 39);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(343, 21);
            this.resourceNameComboBox.TabIndex = 1;
            // 
            // currentLevelLabel
            // 
            this.currentLevelLabel.AutoSize = true;
            this.currentLevelLabel.Location = new System.Drawing.Point(8, 23);
            this.currentLevelLabel.Name = "currentLevelLabel";
            this.currentLevelLabel.Size = new System.Drawing.Size(70, 13);
            this.currentLevelLabel.TabIndex = 0;
            this.currentLevelLabel.Text = "Current Level";
            // 
            // currentLevelRangeLabel
            // 
            this.currentLevelRangeLabel.AutoSize = true;
            this.currentLevelRangeLabel.Location = new System.Drawing.Point(8, 77);
            this.currentLevelRangeLabel.Name = "currentLevelRangeLabel";
            this.currentLevelRangeLabel.Size = new System.Drawing.Size(105, 13);
            this.currentLevelRangeLabel.TabIndex = 2;
            this.currentLevelRangeLabel.Text = "Current Level Range";
            // 
            // voltageLimitRangeLabel
            // 
            this.voltageLimitRangeLabel.AutoSize = true;
            this.voltageLimitRangeLabel.Location = new System.Drawing.Point(8, 130);
            this.voltageLimitRangeLabel.Name = "voltageLimitRangeLabel";
            this.voltageLimitRangeLabel.Size = new System.Drawing.Size(102, 13);
            this.voltageLimitRangeLabel.TabIndex = 4;
            this.voltageLimitRangeLabel.Text = "Voltage Limit Range";
            // 
            // sourceDelayLabel
            // 
            this.sourceDelayLabel.AutoSize = true;
            this.sourceDelayLabel.Location = new System.Drawing.Point(8, 183);
            this.sourceDelayLabel.Name = "sourceDelayLabel";
            this.sourceDelayLabel.Size = new System.Drawing.Size(71, 13);
            this.sourceDelayLabel.TabIndex = 6;
            this.sourceDelayLabel.Text = "Source Delay";
            // 
            // outputShortedLabel
            // 
            this.outputShortedLabel.AutoSize = true;
            this.outputShortedLabel.Location = new System.Drawing.Point(8, 237);
            this.outputShortedLabel.Name = "outputShortedLabel";
            this.outputShortedLabel.Size = new System.Drawing.Size(79, 13);
            this.outputShortedLabel.TabIndex = 8;
            this.outputShortedLabel.Text = "Output Shorted";
            // 
            // conductionVoltageModeLabel
            // 
            this.conductionVoltageModeLabel.AutoSize = true;
            this.conductionVoltageModeLabel.Location = new System.Drawing.Point(162, 22);
            this.conductionVoltageModeLabel.Name = "conductionVoltageModeLabel";
            this.conductionVoltageModeLabel.Size = new System.Drawing.Size(130, 13);
            this.conductionVoltageModeLabel.TabIndex = 10;
            this.conductionVoltageModeLabel.Text = "Conduction Voltage Mode";
            // 
            // conductionVoltageOnThresholdLabel
            // 
            this.conductionVoltageOnThresholdLabel.AutoSize = true;
            this.conductionVoltageOnThresholdLabel.Location = new System.Drawing.Point(162, 76);
            this.conductionVoltageOnThresholdLabel.Name = "conductionVoltageOnThresholdLabel";
            this.conductionVoltageOnThresholdLabel.Size = new System.Drawing.Size(167, 13);
            this.conductionVoltageOnThresholdLabel.TabIndex = 12;
            this.conductionVoltageOnThresholdLabel.Text = "Conduction Voltage On Threshold";
            // 
            // conductionVoltageOffThresholdLabel
            // 
            this.conductionVoltageOffThresholdLabel.AutoSize = true;
            this.conductionVoltageOffThresholdLabel.Location = new System.Drawing.Point(162, 130);
            this.conductionVoltageOffThresholdLabel.Name = "conductionVoltageOffThresholdLabel";
            this.conductionVoltageOffThresholdLabel.Size = new System.Drawing.Size(167, 13);
            this.conductionVoltageOffThresholdLabel.TabIndex = 14;
            this.conductionVoltageOffThresholdLabel.Text = "Conduction Voltage Off Threshold";
            // 
            // currentLevelRisingSlewRateLabel
            // 
            this.currentLevelRisingSlewRateLabel.AutoSize = true;
            this.currentLevelRisingSlewRateLabel.Location = new System.Drawing.Point(162, 183);
            this.currentLevelRisingSlewRateLabel.Name = "currentLevelRisingSlewRateLabel";
            this.currentLevelRisingSlewRateLabel.Size = new System.Drawing.Size(154, 13);
            this.currentLevelRisingSlewRateLabel.TabIndex = 16;
            this.currentLevelRisingSlewRateLabel.Text = "Current Level Rising Slew Rate";
            // 
            // currentLevelFallingSlewRateLabel
            // 
            this.currentLevelFallingSlewRateLabel.AutoSize = true;
            this.currentLevelFallingSlewRateLabel.Location = new System.Drawing.Point(162, 237);
            this.currentLevelFallingSlewRateLabel.Name = "currentLevelFallingSlewRateLabel";
            this.currentLevelFallingSlewRateLabel.Size = new System.Drawing.Size(155, 13);
            this.currentLevelFallingSlewRateLabel.TabIndex = 18;
            this.currentLevelFallingSlewRateLabel.Text = "Current Level Falling Slew Rate";
            // 
            // currentLevelNumeric
            // 
            this.currentLevelNumeric.DecimalPlaces = 6;
            this.currentLevelNumeric.Location = new System.Drawing.Point(8, 40);
            this.currentLevelNumeric.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.currentLevelNumeric.Name = "currentLevelNumeric";
            this.currentLevelNumeric.Size = new System.Drawing.Size(125, 20);
            this.currentLevelNumeric.TabIndex = 1;
            this.currentLevelNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            196608});
            // 
            // currentLevelRangeNumeric
            // 
            this.currentLevelRangeNumeric.DecimalPlaces = 6;
            this.currentLevelRangeNumeric.Location = new System.Drawing.Point(8, 94);
            this.currentLevelRangeNumeric.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.currentLevelRangeNumeric.Name = "currentLevelRangeNumeric";
            this.currentLevelRangeNumeric.Size = new System.Drawing.Size(125, 20);
            this.currentLevelRangeNumeric.TabIndex = 3;
            this.currentLevelRangeNumeric.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // voltageLimitRangeNumeric
            // 
            this.voltageLimitRangeNumeric.DecimalPlaces = 6;
            this.voltageLimitRangeNumeric.Location = new System.Drawing.Point(8, 147);
            this.voltageLimitRangeNumeric.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.voltageLimitRangeNumeric.Name = "voltageLimitRangeNumeric";
            this.voltageLimitRangeNumeric.Size = new System.Drawing.Size(125, 20);
            this.voltageLimitRangeNumeric.TabIndex = 5;
            this.voltageLimitRangeNumeric.Value = new decimal(new int[] {
            60,
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
            this.sourceDelayNumeric.Location = new System.Drawing.Point(8, 200);
            this.sourceDelayNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.sourceDelayNumeric.Name = "sourceDelayNumeric";
            this.sourceDelayNumeric.Size = new System.Drawing.Size(125, 20);
            this.sourceDelayNumeric.TabIndex = 7;
            this.sourceDelayNumeric.Value = new decimal(new int[] {
            50000,
            0,
            0,
            327680});
            // 
            // outputShortedCheckBox
            // 
            this.outputShortedCheckBox.Location = new System.Drawing.Point(8, 254);
            this.outputShortedCheckBox.Name = "outputShortedCheckBox";
            this.outputShortedCheckBox.Size = new System.Drawing.Size(125, 21);
            this.outputShortedCheckBox.TabIndex = 9;
            // 
            // conductionVoltageModeComboBox
            // 
            this.conductionVoltageModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conductionVoltageModeComboBox.FormattingEnabled = true;
            this.conductionVoltageModeComboBox.Location = new System.Drawing.Point(162, 39);
            this.conductionVoltageModeComboBox.Name = "conductionVoltageModeComboBox";
            this.conductionVoltageModeComboBox.Size = new System.Drawing.Size(189, 21);
            this.conductionVoltageModeComboBox.TabIndex = 11;
            // 
            // conductionVoltageOnThresholdNumeric
            // 
            this.conductionVoltageOnThresholdNumeric.DecimalPlaces = 6;
            this.conductionVoltageOnThresholdNumeric.Location = new System.Drawing.Point(162, 94);
            this.conductionVoltageOnThresholdNumeric.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.conductionVoltageOnThresholdNumeric.Name = "conductionVoltageOnThresholdNumeric";
            this.conductionVoltageOnThresholdNumeric.Size = new System.Drawing.Size(189, 20);
            this.conductionVoltageOnThresholdNumeric.TabIndex = 13;
            this.conductionVoltageOnThresholdNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // conductionVoltageOffThresholdNumeric
            // 
            this.conductionVoltageOffThresholdNumeric.DecimalPlaces = 6;
            this.conductionVoltageOffThresholdNumeric.Location = new System.Drawing.Point(162, 147);
            this.conductionVoltageOffThresholdNumeric.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.conductionVoltageOffThresholdNumeric.Name = "conductionVoltageOffThresholdNumeric";
            this.conductionVoltageOffThresholdNumeric.Size = new System.Drawing.Size(189, 20);
            this.conductionVoltageOffThresholdNumeric.TabIndex = 15;
            // 
            // currentLevelRisingSlewRateNumeric
            // 
            this.currentLevelRisingSlewRateNumeric.DecimalPlaces = 6;
            this.currentLevelRisingSlewRateNumeric.Location = new System.Drawing.Point(162, 200);
            this.currentLevelRisingSlewRateNumeric.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.currentLevelRisingSlewRateNumeric.Name = "currentLevelRisingSlewRateNumeric";
            this.currentLevelRisingSlewRateNumeric.Size = new System.Drawing.Size(189, 20);
            this.currentLevelRisingSlewRateNumeric.TabIndex = 17;
            this.currentLevelRisingSlewRateNumeric.Value = new decimal(new int[] {
            2400,
            0,
            0,
            131072});
            // 
            // currentLevelFallingSlewRateNumeric
            // 
            this.currentLevelFallingSlewRateNumeric.DecimalPlaces = 6;
            this.currentLevelFallingSlewRateNumeric.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.currentLevelFallingSlewRateNumeric.Location = new System.Drawing.Point(162, 254);
            this.currentLevelFallingSlewRateNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.currentLevelFallingSlewRateNumeric.Name = "currentLevelFallingSlewRateNumeric";
            this.currentLevelFallingSlewRateNumeric.Size = new System.Drawing.Size(189, 20);
            this.currentLevelFallingSlewRateNumeric.TabIndex = 19;
            this.currentLevelFallingSlewRateNumeric.Value = new decimal(new int[] {
            24000,
            0,
            0,
            196608});
            // 
            // voltageMeasurementLabel
            // 
            this.voltageMeasurementLabel.AutoSize = true;
            this.voltageMeasurementLabel.Location = new System.Drawing.Point(9, 77);
            this.voltageMeasurementLabel.Name = "voltageMeasurementLabel";
            this.voltageMeasurementLabel.Size = new System.Drawing.Size(110, 13);
            this.voltageMeasurementLabel.TabIndex = 2;
            this.voltageMeasurementLabel.Text = "Voltage";
            // 
            // currentMeasurementLabel
            // 
            this.currentMeasurementLabel.AutoSize = true;
            this.currentMeasurementLabel.Location = new System.Drawing.Point(9, 22);
            this.currentMeasurementLabel.Name = "currentMeasurementLabel";
            this.currentMeasurementLabel.Size = new System.Drawing.Size(108, 13);
            this.currentMeasurementLabel.TabIndex = 0;
            this.currentMeasurementLabel.Text = "Current";
            // 
            // inComplianceLabel
            // 
            this.inComplianceLabel.AutoSize = true;
            this.inComplianceLabel.Location = new System.Drawing.Point(9, 131);
            this.inComplianceLabel.Name = "inComplianceLabel";
            this.inComplianceLabel.Size = new System.Drawing.Size(135, 13);
            this.inComplianceLabel.TabIndex = 4;
            this.inComplianceLabel.Text = "Compliance/Limit Reached";
            // 
            // measurementResultGroupBox
            // 
            this.measurementResultGroupBox.Controls.Add(this.voltageMeasurementLabel);
            this.measurementResultGroupBox.Controls.Add(this.currentMeasurementLabel);
            this.measurementResultGroupBox.Controls.Add(this.inComplianceLabel);
            this.measurementResultGroupBox.Controls.Add(this.currentMeasurementTextBox);
            this.measurementResultGroupBox.Controls.Add(this.voltageMeasurementTextBox);
            this.measurementResultGroupBox.Controls.Add(this.inComplianceButtonLed);
            this.measurementResultGroupBox.Location = new System.Drawing.Point(388, 12);
            this.measurementResultGroupBox.Name = "measurementResultGroupBox";
            this.measurementResultGroupBox.Size = new System.Drawing.Size(210, 184);
            this.measurementResultGroupBox.TabIndex = 2;
            this.measurementResultGroupBox.TabStop = false;
            this.measurementResultGroupBox.Text = "Measurements";
            // 
            // currentMeasurementTextBox
            // 
            this.currentMeasurementTextBox.Location = new System.Drawing.Point(9, 40);
            this.currentMeasurementTextBox.Name = "currentMeasurementTextBox";
            this.currentMeasurementTextBox.ReadOnly = true;
            this.currentMeasurementTextBox.Size = new System.Drawing.Size(187, 20);
            this.currentMeasurementTextBox.TabIndex = 1;
            this.currentMeasurementTextBox.Text = "0.000000E+000";
            // 
            // voltageMeasurementTextBox
            // 
            this.voltageMeasurementTextBox.Location = new System.Drawing.Point(9, 94);
            this.voltageMeasurementTextBox.Name = "voltageMeasurementTextBox";
            this.voltageMeasurementTextBox.ReadOnly = true;
            this.voltageMeasurementTextBox.Size = new System.Drawing.Size(187, 20);
            this.voltageMeasurementTextBox.TabIndex = 3;
            this.voltageMeasurementTextBox.Text = "0.000000E+000";
            // 
            // inComplianceButtonLed
            // 
            this.inComplianceButtonLed.Enabled = false;
            this.inComplianceButtonLed.Location = new System.Drawing.Point(9, 149);
            this.inComplianceButtonLed.Name = "inComplianceButtonLed";
            this.inComplianceButtonLed.Size = new System.Drawing.Size(21, 22);
            this.inComplianceButtonLed.TabIndex = 5;
            this.inComplianceButtonLed.UseVisualStyleBackColor = true;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(457, 356);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // resourceNameAndChannelNameGroupBox
            // 
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.resourceNameLabel);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.resourceNameComboBox);
            this.resourceNameAndChannelNameGroupBox.Location = new System.Drawing.Point(12, 12);
            this.resourceNameAndChannelNameGroupBox.Name = "resourceNameAndChannelNameGroupBox";
            this.resourceNameAndChannelNameGroupBox.Size = new System.Drawing.Size(360, 71);
            this.resourceNameAndChannelNameGroupBox.TabIndex = 0;
            this.resourceNameAndChannelNameGroupBox.TabStop = false;
            this.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name";
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 387);
            this.Controls.Add(this.resourceNameAndChannelNameGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.measurementResultGroupBox);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Sink DC Current into Electronic Load";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLimitRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDelayNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.conductionVoltageOnThresholdNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.conductionVoltageOffThresholdNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelRisingSlewRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelFallingSlewRateNumeric)).EndInit();
            this.measurementResultGroupBox.ResumeLayout(false);
            this.measurementResultGroupBox.PerformLayout();
            this.resourceNameAndChannelNameGroupBox.ResumeLayout(false);
            this.resourceNameAndChannelNameGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label currentLevelLabel;
        private System.Windows.Forms.Label currentLevelRangeLabel;
        private System.Windows.Forms.Label voltageLimitRangeLabel;
        private System.Windows.Forms.Label sourceDelayLabel;
        private System.Windows.Forms.Label outputShortedLabel;
        private System.Windows.Forms.Label conductionVoltageModeLabel;
        private System.Windows.Forms.Label conductionVoltageOnThresholdLabel;
        private System.Windows.Forms.Label conductionVoltageOffThresholdLabel;
        private System.Windows.Forms.Label currentLevelRisingSlewRateLabel;
        private System.Windows.Forms.Label currentLevelFallingSlewRateLabel;
        private System.Windows.Forms.Label voltageMeasurementLabel;
        private System.Windows.Forms.Label currentMeasurementLabel;
        private System.Windows.Forms.Label inComplianceLabel;

        private System.Windows.Forms.NumericUpDown currentLevelNumeric;
        private System.Windows.Forms.NumericUpDown currentLevelRangeNumeric;
        private System.Windows.Forms.NumericUpDown voltageLimitRangeNumeric;
        private System.Windows.Forms.NumericUpDown sourceDelayNumeric;
        private System.Windows.Forms.ComboBox conductionVoltageModeComboBox;
        private System.Windows.Forms.NumericUpDown conductionVoltageOnThresholdNumeric;
        private System.Windows.Forms.NumericUpDown conductionVoltageOffThresholdNumeric;
        private System.Windows.Forms.NumericUpDown currentLevelRisingSlewRateNumeric;
        private System.Windows.Forms.NumericUpDown currentLevelFallingSlewRateNumeric;

        private System.Windows.Forms.GroupBox measurementResultGroupBox;
        private System.Windows.Forms.TextBox currentMeasurementTextBox;
        private System.Windows.Forms.TextBox voltageMeasurementTextBox;
        private System.Windows.Forms.Button inComplianceButtonLed;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.CheckBox outputShortedCheckBox;
        private System.Windows.Forms.GroupBox resourceNameAndChannelNameGroupBox;
    }
}
