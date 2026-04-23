namespace NationalInstruments.Examples.BasicMeasurementLibrary
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
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.scalarMeasurementComboBox = new System.Windows.Forms.ComboBox();
            this.scalarMeasurementLabel = new System.Windows.Forms.Label();
            this.acquireButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.referenceLevelGroupBox = new System.Windows.Forms.GroupBox();
            this.highReferenceNumeric = new System.Windows.Forms.NumericUpDown();
            this.middleReferenceNumeric = new System.Windows.Forms.NumericUpDown();
            this.lowReferenceNumeric = new System.Windows.Forms.NumericUpDown();
            this.highReferenceLabel = new System.Windows.Forms.Label();
            this.middleReferenceLabel = new System.Windows.Forms.Label();
            this.lowReferenceLabel = new System.Windows.Forms.Label();
            this.edgeTriggerGroupBox = new System.Windows.Forms.GroupBox();
            this.triggerSourceComboBox = new System.Windows.Forms.ComboBox();
            this.triggerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggerCouplingLabel = new System.Windows.Forms.Label();
            this.triggerCouplingComboBox = new System.Windows.Forms.ComboBox();
            this.triggerSlopeLabel = new System.Windows.Forms.Label();
            this.triggerSlopeComboBox = new System.Windows.Forms.ComboBox();
            this.triggerLevelLabel = new System.Windows.Forms.Label();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.triggeringGroupBox = new System.Windows.Forms.GroupBox();
            this.maxTimeNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumTimeLabel = new System.Windows.Forms.Label();
            this.triggerTypeComboBox = new System.Windows.Forms.ComboBox();
            this.triggerTypeLabel = new System.Windows.Forms.Label();
            this.timingGroupBox = new System.Windows.Forms.GroupBox();
            this.sampleRateMinNumeric = new System.Windows.Forms.NumericUpDown();
            this.minRecordLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.enforceRealTimeCheckBox = new System.Windows.Forms.CheckBox();
            this.recordLengthMinLabel = new System.Windows.Forms.Label();
            this.sampleRateMinLabel = new System.Windows.Forms.Label();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.verticalRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.verticalRangeLabel = new System.Windows.Forms.Label();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.scanResultLabel = new System.Windows.Forms.Label();
            this.meanLabel = new System.Windows.Forms.Label();
            this.standardDeviationLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.numberInStatsLabel = new System.Windows.Forms.Label();
            this.clearStatisticsButton = new System.Windows.Forms.Button();
            this.scalarResultTextBox = new System.Windows.Forms.TextBox();
            this.meanTextBox = new System.Windows.Forms.TextBox();
            this.standardDeviationTextBox = new System.Windows.Forms.TextBox();
            this.maximumTextBox = new System.Windows.Forms.TextBox();
            this.minimumTextBox = new System.Windows.Forms.TextBox();
            this.numberInStatsTextBox = new System.Windows.Forms.TextBox();
            this.measurementsGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.measurementGroupBox.SuspendLayout();
            this.referenceLevelGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.highReferenceNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.middleReferenceNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lowReferenceNumeric)).BeginInit();
            this.edgeTriggerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).BeginInit();
            this.triggeringGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxTimeNumeric)).BeginInit();
            this.timingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateMinNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minRecordLengthNumeric)).BeginInit();
            this.generalGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).BeginInit();
            this.measurementsGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.scalarMeasurementComboBox);
            this.measurementGroupBox.Controls.Add(this.scalarMeasurementLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(260, 12);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(247, 66);
            this.measurementGroupBox.TabIndex = 4;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Waveform Measurement";
            // 
            // scalarMeasurementComboBox
            // 
            this.scalarMeasurementComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scalarMeasurementComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.scalarMeasurementComboBox.FormattingEnabled = true;
            this.scalarMeasurementComboBox.Location = new System.Drawing.Point(6, 38);
            this.scalarMeasurementComboBox.Name = "scalarMeasurementComboBox";
            this.scalarMeasurementComboBox.Size = new System.Drawing.Size(235, 21);
            this.scalarMeasurementComboBox.TabIndex = 1;
            // 
            // scalarMeasurementLabel
            // 
            this.scalarMeasurementLabel.AutoSize = true;
            this.scalarMeasurementLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.scalarMeasurementLabel.Location = new System.Drawing.Point(6, 22);
            this.scalarMeasurementLabel.Name = "scalarMeasurementLabel";
            this.scalarMeasurementLabel.Size = new System.Drawing.Size(107, 13);
            this.scalarMeasurementLabel.TabIndex = 0;
            this.scalarMeasurementLabel.Text = "Scalar Measurement:";
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(47, 19);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(128, 19);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "&Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // referenceLevelGroupBox
            // 
            this.referenceLevelGroupBox.Controls.Add(this.highReferenceNumeric);
            this.referenceLevelGroupBox.Controls.Add(this.middleReferenceNumeric);
            this.referenceLevelGroupBox.Controls.Add(this.lowReferenceNumeric);
            this.referenceLevelGroupBox.Controls.Add(this.highReferenceLabel);
            this.referenceLevelGroupBox.Controls.Add(this.middleReferenceLabel);
            this.referenceLevelGroupBox.Controls.Add(this.lowReferenceLabel);
            this.referenceLevelGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.referenceLevelGroupBox.Location = new System.Drawing.Point(260, 89);
            this.referenceLevelGroupBox.Name = "referenceLevelGroupBox";
            this.referenceLevelGroupBox.Size = new System.Drawing.Size(247, 97);
            this.referenceLevelGroupBox.TabIndex = 5;
            this.referenceLevelGroupBox.TabStop = false;
            this.referenceLevelGroupBox.Text = "Reference Level Percentage";
            // 
            // highReferenceNumeric
            // 
            this.highReferenceNumeric.DecimalPlaces = 2;
            this.highReferenceNumeric.Location = new System.Drawing.Point(128, 71);
            this.highReferenceNumeric.Name = "highReferenceNumeric";
            this.highReferenceNumeric.Size = new System.Drawing.Size(113, 20);
            this.highReferenceNumeric.TabIndex = 5;
            this.highReferenceNumeric.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // middleReferenceNumeric
            // 
            this.middleReferenceNumeric.DecimalPlaces = 2;
            this.middleReferenceNumeric.Location = new System.Drawing.Point(128, 45);
            this.middleReferenceNumeric.Name = "middleReferenceNumeric";
            this.middleReferenceNumeric.Size = new System.Drawing.Size(113, 20);
            this.middleReferenceNumeric.TabIndex = 3;
            this.middleReferenceNumeric.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // lowReferenceNumeric
            // 
            this.lowReferenceNumeric.DecimalPlaces = 2;
            this.lowReferenceNumeric.Location = new System.Drawing.Point(128, 19);
            this.lowReferenceNumeric.Name = "lowReferenceNumeric";
            this.lowReferenceNumeric.Size = new System.Drawing.Size(113, 20);
            this.lowReferenceNumeric.TabIndex = 1;
            this.lowReferenceNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // highReferenceLabel
            // 
            this.highReferenceLabel.AutoSize = true;
            this.highReferenceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.highReferenceLabel.Location = new System.Drawing.Point(6, 75);
            this.highReferenceLabel.Name = "highReferenceLabel";
            this.highReferenceLabel.Size = new System.Drawing.Size(85, 13);
            this.highReferenceLabel.TabIndex = 4;
            this.highReferenceLabel.Text = "High Reference:";
            // 
            // middleReferenceLabel
            // 
            this.middleReferenceLabel.AutoSize = true;
            this.middleReferenceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.middleReferenceLabel.Location = new System.Drawing.Point(6, 49);
            this.middleReferenceLabel.Name = "middleReferenceLabel";
            this.middleReferenceLabel.Size = new System.Drawing.Size(94, 13);
            this.middleReferenceLabel.TabIndex = 2;
            this.middleReferenceLabel.Text = "Middle Reference:";
            // 
            // lowReferenceLabel
            // 
            this.lowReferenceLabel.AutoSize = true;
            this.lowReferenceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lowReferenceLabel.Location = new System.Drawing.Point(6, 21);
            this.lowReferenceLabel.Name = "lowReferenceLabel";
            this.lowReferenceLabel.Size = new System.Drawing.Size(83, 13);
            this.lowReferenceLabel.TabIndex = 0;
            this.lowReferenceLabel.Text = "Low Reference:";
            // 
            // edgeTriggerGroupBox
            // 
            this.edgeTriggerGroupBox.Controls.Add(this.triggerSourceComboBox);
            this.edgeTriggerGroupBox.Controls.Add(this.triggerLevelNumeric);
            this.edgeTriggerGroupBox.Controls.Add(this.triggerCouplingLabel);
            this.edgeTriggerGroupBox.Controls.Add(this.triggerCouplingComboBox);
            this.edgeTriggerGroupBox.Controls.Add(this.triggerSlopeLabel);
            this.edgeTriggerGroupBox.Controls.Add(this.triggerSlopeComboBox);
            this.edgeTriggerGroupBox.Controls.Add(this.triggerLevelLabel);
            this.edgeTriggerGroupBox.Controls.Add(this.triggerSourceLabel);
            this.edgeTriggerGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.edgeTriggerGroupBox.Location = new System.Drawing.Point(12, 312);
            this.edgeTriggerGroupBox.Name = "edgeTriggerGroupBox";
            this.edgeTriggerGroupBox.Size = new System.Drawing.Size(242, 128);
            this.edgeTriggerGroupBox.TabIndex = 3;
            this.edgeTriggerGroupBox.TabStop = false;
            this.edgeTriggerGroupBox.Text = "Edge Trigger Parameters";
            // 
            // triggerSourceComboBox
            // 
            this.triggerSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerSourceComboBox.Location = new System.Drawing.Point(96, 19);
            this.triggerSourceComboBox.Name = "triggerSourceComboBox";
            this.triggerSourceComboBox.Size = new System.Drawing.Size(140, 21);
            this.triggerSourceComboBox.TabIndex = 1;
            // 
            // triggerLevelNumeric
            // 
            this.triggerLevelNumeric.DecimalPlaces = 2;
            this.triggerLevelNumeric.Location = new System.Drawing.Point(96, 46);
            this.triggerLevelNumeric.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.triggerLevelNumeric.Name = "triggerLevelNumeric";
            this.triggerLevelNumeric.Size = new System.Drawing.Size(140, 20);
            this.triggerLevelNumeric.TabIndex = 3;
            // 
            // triggerCouplingLabel
            // 
            this.triggerCouplingLabel.AutoSize = true;
            this.triggerCouplingLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerCouplingLabel.Location = new System.Drawing.Point(6, 103);
            this.triggerCouplingLabel.Name = "triggerCouplingLabel";
            this.triggerCouplingLabel.Size = new System.Drawing.Size(87, 13);
            this.triggerCouplingLabel.TabIndex = 6;
            this.triggerCouplingLabel.Text = "Trigger Coupling:";
            // 
            // triggerCouplingComboBox
            // 
            this.triggerCouplingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerCouplingComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerCouplingComboBox.FormattingEnabled = true;
            this.triggerCouplingComboBox.Location = new System.Drawing.Point(96, 99);
            this.triggerCouplingComboBox.Name = "triggerCouplingComboBox";
            this.triggerCouplingComboBox.Size = new System.Drawing.Size(140, 21);
            this.triggerCouplingComboBox.TabIndex = 7;
            // 
            // triggerSlopeLabel
            // 
            this.triggerSlopeLabel.AutoSize = true;
            this.triggerSlopeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerSlopeLabel.Location = new System.Drawing.Point(6, 76);
            this.triggerSlopeLabel.Name = "triggerSlopeLabel";
            this.triggerSlopeLabel.Size = new System.Drawing.Size(73, 13);
            this.triggerSlopeLabel.TabIndex = 4;
            this.triggerSlopeLabel.Text = "Trigger Slope:";
            // 
            // triggerSlopeComboBox
            // 
            this.triggerSlopeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerSlopeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerSlopeComboBox.FormattingEnabled = true;
            this.triggerSlopeComboBox.Location = new System.Drawing.Point(96, 72);
            this.triggerSlopeComboBox.Name = "triggerSlopeComboBox";
            this.triggerSlopeComboBox.Size = new System.Drawing.Size(140, 21);
            this.triggerSlopeComboBox.TabIndex = 5;
            // 
            // triggerLevelLabel
            // 
            this.triggerLevelLabel.AutoSize = true;
            this.triggerLevelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerLevelLabel.Location = new System.Drawing.Point(6, 50);
            this.triggerLevelLabel.Name = "triggerLevelLabel";
            this.triggerLevelLabel.Size = new System.Drawing.Size(72, 13);
            this.triggerLevelLabel.TabIndex = 2;
            this.triggerLevelLabel.Text = "Trigger Level:";
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.AutoSize = true;
            this.triggerSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerSourceLabel.Location = new System.Drawing.Point(6, 23);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(80, 13);
            this.triggerSourceLabel.TabIndex = 0;
            this.triggerSourceLabel.Text = "Trigger Source:";
            // 
            // triggeringGroupBox
            // 
            this.triggeringGroupBox.Controls.Add(this.maxTimeNumeric);
            this.triggeringGroupBox.Controls.Add(this.maximumTimeLabel);
            this.triggeringGroupBox.Controls.Add(this.triggerTypeComboBox);
            this.triggeringGroupBox.Controls.Add(this.triggerTypeLabel);
            this.triggeringGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggeringGroupBox.Location = new System.Drawing.Point(12, 226);
            this.triggeringGroupBox.Name = "triggeringGroupBox";
            this.triggeringGroupBox.Size = new System.Drawing.Size(242, 75);
            this.triggeringGroupBox.TabIndex = 2;
            this.triggeringGroupBox.TabStop = false;
            this.triggeringGroupBox.Text = "Triggering";
            // 
            // maxTimeNumeric
            // 
            this.maxTimeNumeric.DecimalPlaces = 2;
            this.maxTimeNumeric.Location = new System.Drawing.Point(136, 46);
            this.maxTimeNumeric.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.maxTimeNumeric.Name = "maxTimeNumeric";
            this.maxTimeNumeric.Size = new System.Drawing.Size(100, 20);
            this.maxTimeNumeric.TabIndex = 3;
            this.maxTimeNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // maximumTimeLabel
            // 
            this.maximumTimeLabel.AutoSize = true;
            this.maximumTimeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumTimeLabel.Location = new System.Drawing.Point(6, 50);
            this.maximumTimeLabel.Name = "maximumTimeLabel";
            this.maximumTimeLabel.Size = new System.Drawing.Size(62, 13);
            this.maximumTimeLabel.TabIndex = 2;
            this.maximumTimeLabel.Text = "Timeout (s):";
            // 
            // triggerTypeComboBox
            // 
            this.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerTypeComboBox.FormattingEnabled = true;
            this.triggerTypeComboBox.Location = new System.Drawing.Point(136, 19);
            this.triggerTypeComboBox.Name = "triggerTypeComboBox";
            this.triggerTypeComboBox.Size = new System.Drawing.Size(100, 21);
            this.triggerTypeComboBox.TabIndex = 1;
            // 
            // triggerTypeLabel
            // 
            this.triggerTypeLabel.AutoSize = true;
            this.triggerTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerTypeLabel.Location = new System.Drawing.Point(6, 23);
            this.triggerTypeLabel.Name = "triggerTypeLabel";
            this.triggerTypeLabel.Size = new System.Drawing.Size(70, 13);
            this.triggerTypeLabel.TabIndex = 0;
            this.triggerTypeLabel.Text = "Trigger Type:";
            // 
            // timingGroupBox
            // 
            this.timingGroupBox.Controls.Add(this.sampleRateMinNumeric);
            this.timingGroupBox.Controls.Add(this.minRecordLengthNumeric);
            this.timingGroupBox.Controls.Add(this.enforceRealTimeCheckBox);
            this.timingGroupBox.Controls.Add(this.recordLengthMinLabel);
            this.timingGroupBox.Controls.Add(this.sampleRateMinLabel);
            this.timingGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingGroupBox.Location = new System.Drawing.Point(12, 123);
            this.timingGroupBox.Name = "timingGroupBox";
            this.timingGroupBox.Size = new System.Drawing.Size(242, 97);
            this.timingGroupBox.TabIndex = 1;
            this.timingGroupBox.TabStop = false;
            this.timingGroupBox.Text = "Timing";
            // 
            // sampleRateMinNumeric
            // 
            this.sampleRateMinNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.sampleRateMinNumeric.Location = new System.Drawing.Point(136, 43);
            this.sampleRateMinNumeric.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.sampleRateMinNumeric.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.sampleRateMinNumeric.Name = "sampleRateMinNumeric";
            this.sampleRateMinNumeric.Size = new System.Drawing.Size(100, 20);
            this.sampleRateMinNumeric.TabIndex = 2;
            this.sampleRateMinNumeric.Value = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            // 
            // minRecordLengthNumeric
            // 
            this.minRecordLengthNumeric.Location = new System.Drawing.Point(136, 69);
            this.minRecordLengthNumeric.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.minRecordLengthNumeric.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.minRecordLengthNumeric.Name = "minRecordLengthNumeric";
            this.minRecordLengthNumeric.Size = new System.Drawing.Size(100, 20);
            this.minRecordLengthNumeric.TabIndex = 4;
            this.minRecordLengthNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // enforceRealTimeCheckBox
            // 
            this.enforceRealTimeCheckBox.AutoSize = true;
            this.enforceRealTimeCheckBox.Checked = true;
            this.enforceRealTimeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enforceRealTimeCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.enforceRealTimeCheckBox.Location = new System.Drawing.Point(6, 19);
            this.enforceRealTimeCheckBox.Name = "enforceRealTimeCheckBox";
            this.enforceRealTimeCheckBox.Size = new System.Drawing.Size(120, 18);
            this.enforceRealTimeCheckBox.TabIndex = 0;
            this.enforceRealTimeCheckBox.Text = "&Enforce Real Time";
            this.enforceRealTimeCheckBox.UseVisualStyleBackColor = true;
            // 
            // recordLengthMinLabel
            // 
            this.recordLengthMinLabel.AutoSize = true;
            this.recordLengthMinLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.recordLengthMinLabel.Location = new System.Drawing.Point(6, 73);
            this.recordLengthMinLabel.Name = "recordLengthMinLabel";
            this.recordLengthMinLabel.Size = new System.Drawing.Size(104, 13);
            this.recordLengthMinLabel.TabIndex = 3;
            this.recordLengthMinLabel.Text = "Min. Record Length:";
            // 
            // sampleRateMinLabel
            // 
            this.sampleRateMinLabel.AutoSize = true;
            this.sampleRateMinLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleRateMinLabel.Location = new System.Drawing.Point(6, 47);
            this.sampleRateMinLabel.Name = "sampleRateMinLabel";
            this.sampleRateMinLabel.Size = new System.Drawing.Size(116, 13);
            this.sampleRateMinLabel.TabIndex = 1;
            this.sampleRateMinLabel.Text = "Min. Sample Rate (Hz):";
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.resourceNameComboBox);
            this.generalGroupBox.Controls.Add(this.verticalRangeNumeric);
            this.generalGroupBox.Controls.Add(this.verticalRangeLabel);
            this.generalGroupBox.Controls.Add(this.channelNameTextBox);
            this.generalGroupBox.Controls.Add(this.channelNameLabel);
            this.generalGroupBox.Controls.Add(this.resourceNameLabel);
            this.generalGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.generalGroupBox.Location = new System.Drawing.Point(12, 12);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(242, 100);
            this.generalGroupBox.TabIndex = 0;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(136, 19);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(100, 21);
            this.resourceNameComboBox.TabIndex = 1;
            // 
            // verticalRangeNumeric
            // 
            this.verticalRangeNumeric.DecimalPlaces = 2;
            this.verticalRangeNumeric.Location = new System.Drawing.Point(136, 72);
            this.verticalRangeNumeric.Name = "verticalRangeNumeric";
            this.verticalRangeNumeric.Size = new System.Drawing.Size(100, 20);
            this.verticalRangeNumeric.TabIndex = 5;
            this.verticalRangeNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // verticalRangeLabel
            // 
            this.verticalRangeLabel.AutoSize = true;
            this.verticalRangeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.verticalRangeLabel.Location = new System.Drawing.Point(6, 76);
            this.verticalRangeLabel.Name = "verticalRangeLabel";
            this.verticalRangeLabel.Size = new System.Drawing.Size(80, 13);
            this.verticalRangeLabel.TabIndex = 4;
            this.verticalRangeLabel.Text = "Vertical Range:";
            // 
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(136, 46);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.channelNameTextBox.TabIndex = 3;
            this.channelNameTextBox.Text = "0";
            // 
            // channelNameLabel
            // 
            this.channelNameLabel.AutoSize = true;
            this.channelNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelNameLabel.Location = new System.Drawing.Point(6, 50);
            this.channelNameLabel.Name = "channelNameLabel";
            this.channelNameLabel.Size = new System.Drawing.Size(80, 13);
            this.channelNameLabel.TabIndex = 2;
            this.channelNameLabel.Text = "Channel Name:";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 23);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // scanResultLabel
            // 
            this.scanResultLabel.AutoSize = true;
            this.scanResultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.scanResultLabel.Location = new System.Drawing.Point(6, 23);
            this.scanResultLabel.Name = "scanResultLabel";
            this.scanResultLabel.Size = new System.Drawing.Size(73, 13);
            this.scanResultLabel.TabIndex = 0;
            this.scanResultLabel.Text = "Scalar Result:";
            // 
            // meanLabel
            // 
            this.meanLabel.AutoSize = true;
            this.meanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.meanLabel.Location = new System.Drawing.Point(6, 49);
            this.meanLabel.Name = "meanLabel";
            this.meanLabel.Size = new System.Drawing.Size(37, 13);
            this.meanLabel.TabIndex = 2;
            this.meanLabel.Text = "Mean:";
            // 
            // standardDeviationLabel
            // 
            this.standardDeviationLabel.AutoSize = true;
            this.standardDeviationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.standardDeviationLabel.Location = new System.Drawing.Point(6, 75);
            this.standardDeviationLabel.Name = "standardDeviationLabel";
            this.standardDeviationLabel.Size = new System.Drawing.Size(101, 13);
            this.standardDeviationLabel.TabIndex = 4;
            this.standardDeviationLabel.Text = "Standard Deviation:";
            // 
            // minimumLabel
            // 
            this.minimumLabel.AutoSize = true;
            this.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumLabel.Location = new System.Drawing.Point(6, 101);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(51, 13);
            this.minimumLabel.TabIndex = 6;
            this.minimumLabel.Text = "Minimum:";
            // 
            // maximumLabel
            // 
            this.maximumLabel.AutoSize = true;
            this.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumLabel.Location = new System.Drawing.Point(6, 127);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(54, 13);
            this.maximumLabel.TabIndex = 8;
            this.maximumLabel.Text = "Maximum:";
            // 
            // numberInStatsLabel
            // 
            this.numberInStatsLabel.AutoSize = true;
            this.numberInStatsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.numberInStatsLabel.Location = new System.Drawing.Point(6, 153);
            this.numberInStatsLabel.Name = "numberInStatsLabel";
            this.numberInStatsLabel.Size = new System.Drawing.Size(103, 13);
            this.numberInStatsLabel.TabIndex = 10;
            this.numberInStatsLabel.Text = "Number in Statistics:";
            // 
            // clearStatisticsButton
            // 
            this.clearStatisticsButton.Enabled = false;
            this.clearStatisticsButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clearStatisticsButton.Location = new System.Drawing.Point(80, 179);
            this.clearStatisticsButton.Name = "clearStatisticsButton";
            this.clearStatisticsButton.Size = new System.Drawing.Size(97, 23);
            this.clearStatisticsButton.TabIndex = 12;
            this.clearStatisticsButton.Text = "&Clear Statistics";
            this.clearStatisticsButton.UseVisualStyleBackColor = true;
            this.clearStatisticsButton.Click += new System.EventHandler(this.clearStatisticsButton_Click);
            // 
            // scalarResultTextBox
            // 
            this.scalarResultTextBox.Location = new System.Drawing.Point(128, 19);
            this.scalarResultTextBox.Name = "scalarResultTextBox";
            this.scalarResultTextBox.ReadOnly = true;
            this.scalarResultTextBox.Size = new System.Drawing.Size(113, 20);
            this.scalarResultTextBox.TabIndex = 1;
            // 
            // meanTextBox
            // 
            this.meanTextBox.Location = new System.Drawing.Point(129, 45);
            this.meanTextBox.Name = "meanTextBox";
            this.meanTextBox.ReadOnly = true;
            this.meanTextBox.Size = new System.Drawing.Size(113, 20);
            this.meanTextBox.TabIndex = 3;
            // 
            // standardDeviationTextBox
            // 
            this.standardDeviationTextBox.Location = new System.Drawing.Point(129, 71);
            this.standardDeviationTextBox.Name = "standardDeviationTextBox";
            this.standardDeviationTextBox.ReadOnly = true;
            this.standardDeviationTextBox.Size = new System.Drawing.Size(113, 20);
            this.standardDeviationTextBox.TabIndex = 5;
            // 
            // maximumTextBox
            // 
            this.maximumTextBox.Location = new System.Drawing.Point(129, 123);
            this.maximumTextBox.Name = "maximumTextBox";
            this.maximumTextBox.ReadOnly = true;
            this.maximumTextBox.Size = new System.Drawing.Size(113, 20);
            this.maximumTextBox.TabIndex = 9;
            // 
            // minimumTextBox
            // 
            this.minimumTextBox.Location = new System.Drawing.Point(129, 97);
            this.minimumTextBox.Name = "minimumTextBox";
            this.minimumTextBox.ReadOnly = true;
            this.minimumTextBox.Size = new System.Drawing.Size(113, 20);
            this.minimumTextBox.TabIndex = 7;
            // 
            // numberInStatsTextBox
            // 
            this.numberInStatsTextBox.Location = new System.Drawing.Point(129, 149);
            this.numberInStatsTextBox.Name = "numberInStatsTextBox";
            this.numberInStatsTextBox.ReadOnly = true;
            this.numberInStatsTextBox.Size = new System.Drawing.Size(113, 20);
            this.numberInStatsTextBox.TabIndex = 11;
            // 
            // measurementsGroupBox
            // 
            this.measurementsGroupBox.Controls.Add(this.numberInStatsTextBox);
            this.measurementsGroupBox.Controls.Add(this.minimumTextBox);
            this.measurementsGroupBox.Controls.Add(this.maximumTextBox);
            this.measurementsGroupBox.Controls.Add(this.standardDeviationTextBox);
            this.measurementsGroupBox.Controls.Add(this.meanTextBox);
            this.measurementsGroupBox.Controls.Add(this.scalarResultTextBox);
            this.measurementsGroupBox.Controls.Add(this.clearStatisticsButton);
            this.measurementsGroupBox.Controls.Add(this.numberInStatsLabel);
            this.measurementsGroupBox.Controls.Add(this.maximumLabel);
            this.measurementsGroupBox.Controls.Add(this.minimumLabel);
            this.measurementsGroupBox.Controls.Add(this.standardDeviationLabel);
            this.measurementsGroupBox.Controls.Add(this.meanLabel);
            this.measurementsGroupBox.Controls.Add(this.scanResultLabel);
            this.measurementsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.measurementsGroupBox.Location = new System.Drawing.Point(260, 197);
            this.measurementsGroupBox.Name = "measurementsGroupBox";
            this.measurementsGroupBox.Size = new System.Drawing.Size(247, 208);
            this.measurementsGroupBox.TabIndex = 6;
            this.measurementsGroupBox.TabStop = false;
            this.measurementsGroupBox.Text = "Measurements for the first channel";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.acquireButton);
            this.groupBox1.Controls.Add(this.stopButton);
            this.groupBox1.Location = new System.Drawing.Point(262, 411);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(247, 56);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 477);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.referenceLevelGroupBox);
            this.Controls.Add(this.measurementsGroupBox);
            this.Controls.Add(this.edgeTriggerGroupBox);
            this.Controls.Add(this.triggeringGroupBox);
            this.Controls.Add(this.timingGroupBox);
            this.Controls.Add(this.generalGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Basic Measurement Library";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_Closing);
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.referenceLevelGroupBox.ResumeLayout(false);
            this.referenceLevelGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.highReferenceNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.middleReferenceNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lowReferenceNumeric)).EndInit();
            this.edgeTriggerGroupBox.ResumeLayout(false);
            this.edgeTriggerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).EndInit();
            this.triggeringGroupBox.ResumeLayout(false);
            this.triggeringGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxTimeNumeric)).EndInit();
            this.timingGroupBox.ResumeLayout(false);
            this.timingGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateMinNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minRecordLengthNumeric)).EndInit();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).EndInit();
            this.measurementsGroupBox.ResumeLayout(false);
            this.measurementsGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox measurementGroupBox;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox scalarMeasurementComboBox;
        private System.Windows.Forms.Label scalarMeasurementLabel;
        private System.Windows.Forms.GroupBox referenceLevelGroupBox;
        private System.Windows.Forms.NumericUpDown highReferenceNumeric;
        private System.Windows.Forms.NumericUpDown middleReferenceNumeric;
        private System.Windows.Forms.NumericUpDown lowReferenceNumeric;
        private System.Windows.Forms.Label highReferenceLabel;
        private System.Windows.Forms.Label middleReferenceLabel;
        private System.Windows.Forms.Label lowReferenceLabel;
        private System.Windows.Forms.GroupBox edgeTriggerGroupBox;
        private System.Windows.Forms.NumericUpDown triggerLevelNumeric;
        private System.Windows.Forms.Label triggerCouplingLabel;
        private System.Windows.Forms.ComboBox triggerCouplingComboBox;
        private System.Windows.Forms.Label triggerSlopeLabel;
        private System.Windows.Forms.ComboBox triggerSlopeComboBox;
        private System.Windows.Forms.Label triggerLevelLabel;
        private System.Windows.Forms.Label triggerSourceLabel;
        private System.Windows.Forms.GroupBox triggeringGroupBox;
        private System.Windows.Forms.NumericUpDown maxTimeNumeric;
        private System.Windows.Forms.Label maximumTimeLabel;
        private System.Windows.Forms.ComboBox triggerTypeComboBox;
        private System.Windows.Forms.Label triggerTypeLabel;
        private System.Windows.Forms.GroupBox timingGroupBox;
        private System.Windows.Forms.NumericUpDown minRecordLengthNumeric;
        private System.Windows.Forms.CheckBox enforceRealTimeCheckBox;
        private System.Windows.Forms.Label recordLengthMinLabel;
        private System.Windows.Forms.Label sampleRateMinLabel;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.NumericUpDown verticalRangeNumeric;
        private System.Windows.Forms.Label verticalRangeLabel;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.ComboBox triggerSourceComboBox;
        private System.Windows.Forms.Label scanResultLabel;
        private System.Windows.Forms.Label meanLabel;
        private System.Windows.Forms.Label standardDeviationLabel;
        private System.Windows.Forms.Label minimumLabel;
        private System.Windows.Forms.Label maximumLabel;
        private System.Windows.Forms.Label numberInStatsLabel;
        private System.Windows.Forms.Button clearStatisticsButton;
        private System.Windows.Forms.TextBox scalarResultTextBox;
        private System.Windows.Forms.TextBox meanTextBox;
        private System.Windows.Forms.TextBox standardDeviationTextBox;
        private System.Windows.Forms.TextBox maximumTextBox;
        private System.Windows.Forms.TextBox minimumTextBox;
        private System.Windows.Forms.TextBox numberInStatsTextBox;
        private System.Windows.Forms.GroupBox measurementsGroupBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown sampleRateMinNumeric;
    }
}
