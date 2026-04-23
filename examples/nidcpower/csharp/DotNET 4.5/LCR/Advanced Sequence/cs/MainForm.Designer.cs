namespace NationalInstruments.Examples.LCRAdvancedSequenceFrequencySweep
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
            this.numberOfStepsLabel = new System.Windows.Forms.Label();
            this.startFrequencyLabel = new System.Windows.Forms.Label();
            this.endFrequencyLabel = new System.Windows.Forms.Label();
            this.numberOfStepsNumeric = new System.Windows.Forms.NumericUpDown();
            this.startFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.endFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.lcrShortCompensationEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.lcrOpenCompensationEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.lcrDcBiasCurrentLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.lcrDcBiasCurrentLevel = new System.Windows.Forms.Label();
            this.lcrCustomMeasurementTimeSecNumeric = new System.Windows.Forms.NumericUpDown();
            this.lcrMeasurementTimeComboBox = new System.Windows.Forms.ComboBox();
            this.cableLengthComboBox = new System.Windows.Forms.ComboBox();
            this.lcrDcBiasVoltageLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.lcrDcBiasSourceComboBox = new System.Windows.Forms.ComboBox();
            this.lcrImpedanceRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.ImpedanceRangeLabel = new System.Windows.Forms.Label();
            this.lcrCustomMeasurementTimeSecLabel = new System.Windows.Forms.Label();
            this.lcrMeasurementTimeLabel = new System.Windows.Forms.Label();
            this.cableLengthLabel = new System.Windows.Forms.Label();
            this.lcrDcBiasVoltageLevelLabel = new System.Windows.Forms.Label();
            this.lcrDcBiasSourceLabel = new System.Windows.Forms.Label();
            this.VoltageAmplitudeRmsNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageAmplitudeRmsLabel = new System.Windows.Forms.Label();
            this.measurementsDataGridView = new System.Windows.Forms.DataGridView();
            this.Point = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Frequency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Impedance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.measurementsGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfStepsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endFrequencyNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lcrDcBiasCurrentLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcrCustomMeasurementTimeSecNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcrDcBiasVoltageLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcrImpedanceRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VoltageAmplitudeRmsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).BeginInit();
            this.measurementsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // numberOfStepsLabel
            // 
            this.numberOfStepsLabel.AutoSize = true;
            this.numberOfStepsLabel.Location = new System.Drawing.Point(12, 201);
            this.numberOfStepsLabel.Name = "numberOfStepsLabel";
            this.numberOfStepsLabel.Size = new System.Drawing.Size(88, 13);
            this.numberOfStepsLabel.TabIndex = 14;
            this.numberOfStepsLabel.Text = "Number Of Steps";
            // 
            // startFrequencyLabel
            // 
            this.startFrequencyLabel.AutoSize = true;
            this.startFrequencyLabel.Location = new System.Drawing.Point(12, 100);
            this.startFrequencyLabel.Name = "startFrequencyLabel";
            this.startFrequencyLabel.Size = new System.Drawing.Size(82, 13);
            this.startFrequencyLabel.TabIndex = 2;
            this.startFrequencyLabel.Text = "Start Frequency";
            // 
            // endFrequencyLabel
            // 
            this.endFrequencyLabel.AutoSize = true;
            this.endFrequencyLabel.Location = new System.Drawing.Point(12, 153);
            this.endFrequencyLabel.Name = "endFrequencyLabel";
            this.endFrequencyLabel.Size = new System.Drawing.Size(79, 13);
            this.endFrequencyLabel.TabIndex = 8;
            this.endFrequencyLabel.Text = "End Frequency";
            // 
            // numberOfStepsNumeric
            // 
            this.numberOfStepsNumeric.Location = new System.Drawing.Point(12, 217);
            this.numberOfStepsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numberOfStepsNumeric.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numberOfStepsNumeric.Name = "numberOfStepsNumeric";
            this.numberOfStepsNumeric.Size = new System.Drawing.Size(125, 20);
            this.numberOfStepsNumeric.TabIndex = 15;
            this.numberOfStepsNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // startFrequencyNumeric
            // 
            this.startFrequencyNumeric.DecimalPlaces = 6;
            this.startFrequencyNumeric.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.startFrequencyNumeric.Location = new System.Drawing.Point(12, 116);
            this.startFrequencyNumeric.Maximum = new decimal(new int[] {
            20000000,
            0,
            0,
            0});
            this.startFrequencyNumeric.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.startFrequencyNumeric.Name = "startFrequencyNumeric";
            this.startFrequencyNumeric.Size = new System.Drawing.Size(125, 20);
            this.startFrequencyNumeric.TabIndex = 3;
            this.startFrequencyNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // endFrequencyNumeric
            // 
            this.endFrequencyNumeric.DecimalPlaces = 6;
            this.endFrequencyNumeric.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.endFrequencyNumeric.Location = new System.Drawing.Point(12, 169);
            this.endFrequencyNumeric.Maximum = new decimal(new int[] {
            20000000,
            0,
            0,
            0});
            this.endFrequencyNumeric.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.endFrequencyNumeric.Name = "endFrequencyNumeric";
            this.endFrequencyNumeric.Size = new System.Drawing.Size(125, 20);
            this.endFrequencyNumeric.TabIndex = 9;
            this.endFrequencyNumeric.Value = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(392, 425);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.resourceNameComboBox);
            this.configurationGroupBox.Controls.Add(this.resourceNameLabel);
            this.configurationGroupBox.Controls.Add(this.lcrShortCompensationEnabledCheckBox);
            this.configurationGroupBox.Controls.Add(this.lcrOpenCompensationEnabledCheckBox);
            this.configurationGroupBox.Controls.Add(this.lcrDcBiasCurrentLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.lcrDcBiasCurrentLevel);
            this.configurationGroupBox.Controls.Add(this.lcrCustomMeasurementTimeSecNumeric);
            this.configurationGroupBox.Controls.Add(this.lcrMeasurementTimeComboBox);
            this.configurationGroupBox.Controls.Add(this.cableLengthComboBox);
            this.configurationGroupBox.Controls.Add(this.lcrDcBiasVoltageLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.lcrDcBiasSourceComboBox);
            this.configurationGroupBox.Controls.Add(this.lcrImpedanceRangeNumeric);
            this.configurationGroupBox.Controls.Add(this.ImpedanceRangeLabel);
            this.configurationGroupBox.Controls.Add(this.lcrCustomMeasurementTimeSecLabel);
            this.configurationGroupBox.Controls.Add(this.lcrMeasurementTimeLabel);
            this.configurationGroupBox.Controls.Add(this.cableLengthLabel);
            this.configurationGroupBox.Controls.Add(this.lcrDcBiasVoltageLevelLabel);
            this.configurationGroupBox.Controls.Add(this.lcrDcBiasSourceLabel);
            this.configurationGroupBox.Controls.Add(this.VoltageAmplitudeRmsNumeric);
            this.configurationGroupBox.Controls.Add(this.voltageAmplitudeRmsLabel);
            this.configurationGroupBox.Controls.Add(this.numberOfStepsNumeric);
            this.configurationGroupBox.Controls.Add(this.startFrequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.endFrequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.numberOfStepsLabel);
            this.configurationGroupBox.Controls.Add(this.startFrequencyLabel);
            this.configurationGroupBox.Controls.Add(this.endFrequencyLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(15, 15);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(500, 405);
            this.configurationGroupBox.TabIndex = 0;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 47);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(275, 21);
            this.resourceNameComboBox.TabIndex = 1;
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(12, 28);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // lcrShortCompensationEnabledCheckBox
            // 
            this.lcrShortCompensationEnabledCheckBox.AutoSize = true;
            this.lcrShortCompensationEnabledCheckBox.Location = new System.Drawing.Point(305, 326);
            this.lcrShortCompensationEnabledCheckBox.Name = "lcrShortCompensationEnabledCheckBox";
            this.lcrShortCompensationEnabledCheckBox.Size = new System.Drawing.Size(187, 17);
            this.lcrShortCompensationEnabledCheckBox.TabIndex = 25;
            this.lcrShortCompensationEnabledCheckBox.Text = "LCR Short Compensation Enabled";
            this.lcrShortCompensationEnabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // lcrOpenCompensationEnabledCheckBox
            // 
            this.lcrOpenCompensationEnabledCheckBox.AutoSize = true;
            this.lcrOpenCompensationEnabledCheckBox.Location = new System.Drawing.Point(305, 302);
            this.lcrOpenCompensationEnabledCheckBox.Name = "lcrOpenCompensationEnabledCheckBox";
            this.lcrOpenCompensationEnabledCheckBox.Size = new System.Drawing.Size(188, 17);
            this.lcrOpenCompensationEnabledCheckBox.TabIndex = 24;
            this.lcrOpenCompensationEnabledCheckBox.Text = "LCR Open Compensation Enabled";
            this.lcrOpenCompensationEnabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // lcrDcBiasCurrentLevelNumeric
            // 
            this.lcrDcBiasCurrentLevelNumeric.DecimalPlaces = 6;
            this.lcrDcBiasCurrentLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            393216});
            this.lcrDcBiasCurrentLevelNumeric.Location = new System.Drawing.Point(305, 217);
            this.lcrDcBiasCurrentLevelNumeric.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.lcrDcBiasCurrentLevelNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147418112});
            this.lcrDcBiasCurrentLevelNumeric.Name = "lcrDcBiasCurrentLevelNumeric";
            this.lcrDcBiasCurrentLevelNumeric.Size = new System.Drawing.Size(125, 20);
            this.lcrDcBiasCurrentLevelNumeric.TabIndex = 19;
            // 
            // lcrDcBiasCurrentLevel
            // 
            this.lcrDcBiasCurrentLevel.AutoSize = true;
            this.lcrDcBiasCurrentLevel.Location = new System.Drawing.Point(305, 201);
            this.lcrDcBiasCurrentLevel.Name = "lcrDcBiasCurrentLevel";
            this.lcrDcBiasCurrentLevel.Size = new System.Drawing.Size(135, 13);
            this.lcrDcBiasCurrentLevel.TabIndex = 18;
            this.lcrDcBiasCurrentLevel.Text = "LCR DC Bias Current Level";
            // 
            // lcrCustomMeasurementTimeSecNumeric
            // 
            this.lcrCustomMeasurementTimeSecNumeric.DecimalPlaces = 6;
            this.lcrCustomMeasurementTimeSecNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.lcrCustomMeasurementTimeSecNumeric.Location = new System.Drawing.Point(305, 264);
            this.lcrCustomMeasurementTimeSecNumeric.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            327680});
            this.lcrCustomMeasurementTimeSecNumeric.Name = "lcrCustomMeasurementTimeSecNumeric";
            this.lcrCustomMeasurementTimeSecNumeric.Size = new System.Drawing.Size(125, 20);
            this.lcrCustomMeasurementTimeSecNumeric.TabIndex = 23;
            // 
            // lcrMeasurementTimeComboBox
            // 
            this.lcrMeasurementTimeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lcrMeasurementTimeComboBox.FormattingEnabled = true;
            this.lcrMeasurementTimeComboBox.ItemHeight = 13;
            this.lcrMeasurementTimeComboBox.Location = new System.Drawing.Point(159, 217);
            this.lcrMeasurementTimeComboBox.Name = "lcrMeasurementTimeComboBox";
            this.lcrMeasurementTimeComboBox.Size = new System.Drawing.Size(125, 21);
            this.lcrMeasurementTimeComboBox.TabIndex = 17;
            // 
            // cableLengthComboBox
            // 
            this.cableLengthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cableLengthComboBox.FormattingEnabled = true;
            this.cableLengthComboBox.ItemHeight = 13;
            this.cableLengthComboBox.Location = new System.Drawing.Point(159, 264);
            this.cableLengthComboBox.Name = "cableLengthComboBox";
            this.cableLengthComboBox.Size = new System.Drawing.Size(125, 21);
            this.cableLengthComboBox.TabIndex = 21;
            // 
            // lcrDcBiasVoltageLevelNumeric
            // 
            this.lcrDcBiasVoltageLevelNumeric.DecimalPlaces = 6;
            this.lcrDcBiasVoltageLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.lcrDcBiasVoltageLevelNumeric.Location = new System.Drawing.Point(305, 169);
            this.lcrDcBiasVoltageLevelNumeric.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.lcrDcBiasVoltageLevelNumeric.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            -2147483648});
            this.lcrDcBiasVoltageLevelNumeric.Name = "lcrDcBiasVoltageLevelNumeric";
            this.lcrDcBiasVoltageLevelNumeric.Size = new System.Drawing.Size(125, 20);
            this.lcrDcBiasVoltageLevelNumeric.TabIndex = 13;
            // 
            // lcrDcBiasSourceComboBox
            // 
            this.lcrDcBiasSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lcrDcBiasSourceComboBox.FormattingEnabled = true;
            this.lcrDcBiasSourceComboBox.ItemHeight = 13;
            this.lcrDcBiasSourceComboBox.Location = new System.Drawing.Point(305, 116);
            this.lcrDcBiasSourceComboBox.Name = "lcrDcBiasSourceComboBox";
            this.lcrDcBiasSourceComboBox.Size = new System.Drawing.Size(125, 21);
            this.lcrDcBiasSourceComboBox.TabIndex = 7;
            // 
            // lcrImpedanceRangeNumeric
            // 
            this.lcrImpedanceRangeNumeric.DecimalPlaces = 6;
            this.lcrImpedanceRangeNumeric.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.lcrImpedanceRangeNumeric.Location = new System.Drawing.Point(159, 169);
            this.lcrImpedanceRangeNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.lcrImpedanceRangeNumeric.Name = "lcrImpedanceRangeNumeric";
            this.lcrImpedanceRangeNumeric.Size = new System.Drawing.Size(125, 20);
            this.lcrImpedanceRangeNumeric.TabIndex = 11;
            this.lcrImpedanceRangeNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // ImpedanceRangeLabel
            // 
            this.ImpedanceRangeLabel.AutoSize = true;
            this.ImpedanceRangeLabel.Location = new System.Drawing.Point(159, 153);
            this.ImpedanceRangeLabel.Name = "ImpedanceRangeLabel";
            this.ImpedanceRangeLabel.Size = new System.Drawing.Size(95, 13);
            this.ImpedanceRangeLabel.TabIndex = 10;
            this.ImpedanceRangeLabel.Text = "Impedance Range";
            // 
            // lcrCustomMeasurementTimeSecLabel
            // 
            this.lcrCustomMeasurementTimeSecLabel.AutoSize = true;
            this.lcrCustomMeasurementTimeSecLabel.Location = new System.Drawing.Point(305, 248);
            this.lcrCustomMeasurementTimeSecLabel.Name = "lcrCustomMeasurementTimeSecLabel";
            this.lcrCustomMeasurementTimeSecLabel.Size = new System.Drawing.Size(181, 13);
            this.lcrCustomMeasurementTimeSecLabel.TabIndex = 22;
            this.lcrCustomMeasurementTimeSecLabel.Text = "LCR Custom Measurement Time Sec";
            // 
            // lcrMeasurementTimeLabel
            // 
            this.lcrMeasurementTimeLabel.AutoSize = true;
            this.lcrMeasurementTimeLabel.Location = new System.Drawing.Point(159, 201);
            this.lcrMeasurementTimeLabel.Name = "lcrMeasurementTimeLabel";
            this.lcrMeasurementTimeLabel.Size = new System.Drawing.Size(121, 13);
            this.lcrMeasurementTimeLabel.TabIndex = 16;
            this.lcrMeasurementTimeLabel.Text = "LCR Measurement Time";
            // 
            // cableLengthLabel
            // 
            this.cableLengthLabel.AutoSize = true;
            this.cableLengthLabel.Location = new System.Drawing.Point(159, 248);
            this.cableLengthLabel.Name = "cableLengthLabel";
            this.cableLengthLabel.Size = new System.Drawing.Size(70, 13);
            this.cableLengthLabel.TabIndex = 20;
            this.cableLengthLabel.Text = "Cable Length";
            // 
            // lcrDcBiasVoltageLevelLabel
            // 
            this.lcrDcBiasVoltageLevelLabel.AutoSize = true;
            this.lcrDcBiasVoltageLevelLabel.Location = new System.Drawing.Point(305, 153);
            this.lcrDcBiasVoltageLevelLabel.Name = "lcrDcBiasVoltageLevelLabel";
            this.lcrDcBiasVoltageLevelLabel.Size = new System.Drawing.Size(137, 13);
            this.lcrDcBiasVoltageLevelLabel.TabIndex = 12;
            this.lcrDcBiasVoltageLevelLabel.Text = "LCR DC Bias Voltage Level";
            // 
            // lcrDcBiasSourceLabel
            // 
            this.lcrDcBiasSourceLabel.AutoSize = true;
            this.lcrDcBiasSourceLabel.Location = new System.Drawing.Point(305, 100);
            this.lcrDcBiasSourceLabel.Name = "lcrDcBiasSourceLabel";
            this.lcrDcBiasSourceLabel.Size = new System.Drawing.Size(106, 13);
            this.lcrDcBiasSourceLabel.TabIndex = 6;
            this.lcrDcBiasSourceLabel.Text = "LCD DC Bias Source";
            // 
            // VoltageAmplitudeRmsNumeric
            // 
            this.VoltageAmplitudeRmsNumeric.DecimalPlaces = 6;
            this.VoltageAmplitudeRmsNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.VoltageAmplitudeRmsNumeric.Location = new System.Drawing.Point(159, 116);
            this.VoltageAmplitudeRmsNumeric.Maximum = new decimal(new int[] {
            28284272,
            0,
            0,
            393216});
            this.VoltageAmplitudeRmsNumeric.Minimum = new decimal(new int[] {
            28284272,
            0,
            0,
            -2147090432});
            this.VoltageAmplitudeRmsNumeric.Name = "VoltageAmplitudeRmsNumeric";
            this.VoltageAmplitudeRmsNumeric.Size = new System.Drawing.Size(125, 20);
            this.VoltageAmplitudeRmsNumeric.TabIndex = 5;
            this.VoltageAmplitudeRmsNumeric.Value = new decimal(new int[] {
            7,
            0,
            0,
            65536});
            // 
            // voltageAmplitudeRmsLabel
            // 
            this.voltageAmplitudeRmsLabel.AutoSize = true;
            this.voltageAmplitudeRmsLabel.Location = new System.Drawing.Point(159, 100);
            this.voltageAmplitudeRmsLabel.Name = "voltageAmplitudeRmsLabel";
            this.voltageAmplitudeRmsLabel.Size = new System.Drawing.Size(119, 13);
            this.voltageAmplitudeRmsLabel.TabIndex = 4;
            this.voltageAmplitudeRmsLabel.Text = "Voltage Amplitude RMS";
            // 
            // measurementsDataGridView
            // 
            this.measurementsDataGridView.AllowUserToAddRows = false;
            this.measurementsDataGridView.AllowUserToDeleteRows = false;
            this.measurementsDataGridView.AllowUserToResizeColumns = false;
            this.measurementsDataGridView.AllowUserToResizeRows = false;
            this.measurementsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.measurementsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Point,
            this.Frequency,
            this.Impedance});
            this.measurementsDataGridView.Location = new System.Drawing.Point(11, 17);
            this.measurementsDataGridView.Name = "measurementsDataGridView";
            this.measurementsDataGridView.RowHeadersVisible = false;
            this.measurementsDataGridView.Size = new System.Drawing.Size(265, 380);
            this.measurementsDataGridView.TabIndex = 0;
            // 
            // Point
            // 
            this.Point.HeaderText = "Point";
            this.Point.MinimumWidth = 3;
            this.Point.Name = "Point";
            this.Point.Width = 40;
            // 
            // Frequency
            // 
            this.Frequency.FillWeight = 90F;
            this.Frequency.HeaderText = "Frequency";
            this.Frequency.Name = "Frequency";
            this.Frequency.Width = 110;
            // 
            // Impedance
            // 
            this.Impedance.FillWeight = 90F;
            this.Impedance.HeaderText = "Impedance";
            this.Impedance.Name = "Impedance";
            this.Impedance.Width = 110;
            // 
            // measurementsGroupBox
            // 
            this.measurementsGroupBox.Controls.Add(this.measurementsDataGridView);
            this.measurementsGroupBox.Location = new System.Drawing.Point(521, 15);
            this.measurementsGroupBox.Name = "measurementsGroupBox";
            this.measurementsGroupBox.Size = new System.Drawing.Size(287, 405);
            this.measurementsGroupBox.TabIndex = 1;
            this.measurementsGroupBox.TabStop = false;
            this.measurementsGroupBox.Text = "Measurements";
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 453);
            this.Controls.Add(this.measurementsGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.configurationGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "LCR Advanced Sequence Frequency Sweep";
            ((System.ComponentModel.ISupportInitialize)(this.numberOfStepsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endFrequencyNumeric)).EndInit();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lcrDcBiasCurrentLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcrCustomMeasurementTimeSecNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcrDcBiasVoltageLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcrImpedanceRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VoltageAmplitudeRmsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).EndInit();
            this.measurementsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.Label numberOfStepsLabel;
        private System.Windows.Forms.Label startFrequencyLabel;
        private System.Windows.Forms.Label endFrequencyLabel;
        private System.Windows.Forms.NumericUpDown numberOfStepsNumeric;
        private System.Windows.Forms.NumericUpDown startFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown endFrequencyNumeric;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.DataGridView measurementsDataGridView;
        private System.Windows.Forms.GroupBox measurementsGroupBox;
        private System.Windows.Forms.Label cableLengthLabel;
        private System.Windows.Forms.Label lcrDcBiasVoltageLevelLabel;
        private System.Windows.Forms.Label lcrDcBiasSourceLabel;
        private System.Windows.Forms.NumericUpDown VoltageAmplitudeRmsNumeric;
        private System.Windows.Forms.Label voltageAmplitudeRmsLabel;
        private System.Windows.Forms.NumericUpDown lcrImpedanceRangeNumeric;
        private System.Windows.Forms.Label ImpedanceRangeLabel;
        private System.Windows.Forms.Label lcrCustomMeasurementTimeSecLabel;
        private System.Windows.Forms.Label lcrMeasurementTimeLabel;
        private System.Windows.Forms.NumericUpDown lcrCustomMeasurementTimeSecNumeric;
        private System.Windows.Forms.ComboBox lcrMeasurementTimeComboBox;
        private System.Windows.Forms.ComboBox cableLengthComboBox;
        private System.Windows.Forms.NumericUpDown lcrDcBiasVoltageLevelNumeric;
        private System.Windows.Forms.ComboBox lcrDcBiasSourceComboBox;
        private System.Windows.Forms.CheckBox lcrShortCompensationEnabledCheckBox;
        private System.Windows.Forms.CheckBox lcrOpenCompensationEnabledCheckBox;
        private System.Windows.Forms.NumericUpDown lcrDcBiasCurrentLevelNumeric;
        private System.Windows.Forms.Label lcrDcBiasCurrentLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Point;
        private System.Windows.Forms.DataGridViewTextBoxColumn Frequency;
        private System.Windows.Forms.DataGridViewTextBoxColumn Impedance;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
    }
}
