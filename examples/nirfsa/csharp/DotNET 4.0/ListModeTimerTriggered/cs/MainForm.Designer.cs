namespace NationalInstruments.Examples.ListModeTimerTriggered
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
            this.timerEventIntervalLabel = new System.Windows.Forms.Label();
            this.referenceLevelRampStrtLabel = new System.Windows.Forms.Label();
            this.referenceLevelRampStopLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.samplesPerRecordLabel = new System.Windows.Forms.Label();
            this.numberOfStepsLabel = new System.Windows.Forms.Label();
            this.carrierFrequencyRampStrtLabel = new System.Windows.Forms.Label();
            this.carrierFrequencyRampStopLabel = new System.Windows.Forms.Label();
            this.timerEventIntervalNumeric = new System.Windows.Forms.NumericUpDown();
            this.referenceLevelRampStrtNumeric = new System.Windows.Forms.NumericUpDown();
            this.referenceLevelRampStopNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerRecordNumeric = new System.Windows.Forms.NumericUpDown();
            this.numberOfStepsNumeric = new System.Windows.Forms.NumericUpDown();
            this.carrierFrequencyRampStrtNumeric = new System.Windows.Forms.NumericUpDown();
            this.carrierFrequencyRampStopNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.acquireButton = new System.Windows.Forms.Button();
            this.multiDataGridViewResults = new NationalInstruments.Examples.ListModeTimerTriggered.MultiDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.timerEventIntervalNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelRampStrtNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelRampStopNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfStepsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyRampStrtNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyRampStopNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // timerEventIntervalLabel
            // 
            this.timerEventIntervalLabel.AutoSize = true;
            this.timerEventIntervalLabel.Location = new System.Drawing.Point(12, 406);
            this.timerEventIntervalLabel.Name = "timerEventIntervalLabel";
            this.timerEventIntervalLabel.Size = new System.Drawing.Size(116, 13);
            this.timerEventIntervalLabel.TabIndex = 0;
            this.timerEventIntervalLabel.Text = "Timer Event Interval (s)";
            // 
            // referenceLevelRampStrtLabel
            // 
            this.referenceLevelRampStrtLabel.AutoSize = true;
            this.referenceLevelRampStrtLabel.Location = new System.Drawing.Point(12, 204);
            this.referenceLevelRampStrtLabel.Name = "referenceLevelRampStrtLabel";
            this.referenceLevelRampStrtLabel.Size = new System.Drawing.Size(175, 13);
            this.referenceLevelRampStrtLabel.TabIndex = 1;
            this.referenceLevelRampStrtLabel.Text = "Reference Level  Ramp Start (dBm)";
            // 
            // referenceLevelRampStopLabel
            // 
            this.referenceLevelRampStopLabel.AutoSize = true;
            this.referenceLevelRampStopLabel.Location = new System.Drawing.Point(12, 254);
            this.referenceLevelRampStopLabel.Name = "referenceLevelRampStopLabel";
            this.referenceLevelRampStopLabel.Size = new System.Drawing.Size(172, 13);
            this.referenceLevelRampStopLabel.TabIndex = 2;
            this.referenceLevelRampStopLabel.Text = "Reference Level Ramp Stop (dBm)";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(12, 56);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 3;
            this.iqRateLabel.Text = "IQ Rate (S/s)";
            // 
            // samplesPerRecordLabel
            // 
            this.samplesPerRecordLabel.AutoSize = true;
            this.samplesPerRecordLabel.Location = new System.Drawing.Point(12, 104);
            this.samplesPerRecordLabel.Name = "samplesPerRecordLabel";
            this.samplesPerRecordLabel.Size = new System.Drawing.Size(103, 13);
            this.samplesPerRecordLabel.TabIndex = 4;
            this.samplesPerRecordLabel.Text = "Samples per Record";
            // 
            // numberOfStepsLabel
            // 
            this.numberOfStepsLabel.AutoSize = true;
            this.numberOfStepsLabel.Location = new System.Drawing.Point(12, 151);
            this.numberOfStepsLabel.Name = "numberOfStepsLabel";
            this.numberOfStepsLabel.Size = new System.Drawing.Size(86, 13);
            this.numberOfStepsLabel.TabIndex = 5;
            this.numberOfStepsLabel.Text = "Number of Steps";
            // 
            // carrierFrequencyRampStrtLabel
            // 
            this.carrierFrequencyRampStrtLabel.AutoSize = true;
            this.carrierFrequencyRampStrtLabel.Location = new System.Drawing.Point(12, 304);
            this.carrierFrequencyRampStrtLabel.Name = "carrierFrequencyRampStrtLabel";
            this.carrierFrequencyRampStrtLabel.Size = new System.Drawing.Size(168, 13);
            this.carrierFrequencyRampStrtLabel.TabIndex = 6;
            this.carrierFrequencyRampStrtLabel.Text = "Carrier Frequency Ramp Start (Hz)";
            // 
            // carrierFrequencyRampStopLabel
            // 
            this.carrierFrequencyRampStopLabel.AutoSize = true;
            this.carrierFrequencyRampStopLabel.Location = new System.Drawing.Point(12, 354);
            this.carrierFrequencyRampStopLabel.Name = "carrierFrequencyRampStopLabel";
            this.carrierFrequencyRampStopLabel.Size = new System.Drawing.Size(168, 13);
            this.carrierFrequencyRampStopLabel.TabIndex = 7;
            this.carrierFrequencyRampStopLabel.Text = "Carrier Frequency Ramp Stop (Hz)";
            // 
            // timerEventIntervalNumeric
            // 
            this.timerEventIntervalNumeric.DecimalPlaces = 2;
            this.timerEventIntervalNumeric.Location = new System.Drawing.Point(12, 427);
            this.timerEventIntervalNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.timerEventIntervalNumeric.Name = "timerEventIntervalNumeric";
            this.timerEventIntervalNumeric.Size = new System.Drawing.Size(149, 20);
            this.timerEventIntervalNumeric.TabIndex = 8;
            this.timerEventIntervalNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            196608});
            // 
            // referenceLevelRampStrtNumeric
            // 
            this.referenceLevelRampStrtNumeric.DecimalPlaces = 2;
            this.referenceLevelRampStrtNumeric.Location = new System.Drawing.Point(12, 225);
            this.referenceLevelRampStrtNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.referenceLevelRampStrtNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.referenceLevelRampStrtNumeric.Name = "referenceLevelRampStrtNumeric";
            this.referenceLevelRampStrtNumeric.Size = new System.Drawing.Size(149, 20);
            this.referenceLevelRampStrtNumeric.TabIndex = 4;
            this.referenceLevelRampStrtNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // referenceLevelRampStopNumeric
            // 
            this.referenceLevelRampStopNumeric.DecimalPlaces = 2;
            this.referenceLevelRampStopNumeric.Location = new System.Drawing.Point(12, 275);
            this.referenceLevelRampStopNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.referenceLevelRampStopNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.referenceLevelRampStopNumeric.Name = "referenceLevelRampStopNumeric";
            this.referenceLevelRampStopNumeric.Size = new System.Drawing.Size(149, 20);
            this.referenceLevelRampStopNumeric.TabIndex = 5;
            // 
            // iqRateNumeric
            // 
            this.iqRateNumeric.DecimalPlaces = 2;
            this.iqRateNumeric.Location = new System.Drawing.Point(12, 77);
            this.iqRateNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.iqRateNumeric.Name = "iqRateNumeric";
            this.iqRateNumeric.Size = new System.Drawing.Size(149, 20);
            this.iqRateNumeric.TabIndex = 1;
            this.iqRateNumeric.Value = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            // 
            // samplesPerRecordNumeric
            // 
            this.samplesPerRecordNumeric.Location = new System.Drawing.Point(12, 125);
            this.samplesPerRecordNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.samplesPerRecordNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.samplesPerRecordNumeric.Name = "samplesPerRecordNumeric";
            this.samplesPerRecordNumeric.Size = new System.Drawing.Size(151, 20);
            this.samplesPerRecordNumeric.TabIndex = 2;
            this.samplesPerRecordNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // numberOfStepsNumeric
            // 
            this.numberOfStepsNumeric.Location = new System.Drawing.Point(12, 172);
            this.numberOfStepsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numberOfStepsNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfStepsNumeric.Name = "numberOfStepsNumeric";
            this.numberOfStepsNumeric.Size = new System.Drawing.Size(149, 20);
            this.numberOfStepsNumeric.TabIndex = 3;
            this.numberOfStepsNumeric.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            // 
            // carrierFrequencyRampStrtNumeric
            // 
            this.carrierFrequencyRampStrtNumeric.DecimalPlaces = 4;
            this.carrierFrequencyRampStrtNumeric.Location = new System.Drawing.Point(12, 325);
            this.carrierFrequencyRampStrtNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.carrierFrequencyRampStrtNumeric.Name = "carrierFrequencyRampStrtNumeric";
            this.carrierFrequencyRampStrtNumeric.Size = new System.Drawing.Size(149, 20);
            this.carrierFrequencyRampStrtNumeric.TabIndex = 6;
            this.carrierFrequencyRampStrtNumeric.Value = new decimal(new int[] {
            1020000000,
            0,
            0,
            0});
            // 
            // carrierFrequencyRampStopNumeric
            // 
            this.carrierFrequencyRampStopNumeric.DecimalPlaces = 4;
            this.carrierFrequencyRampStopNumeric.Location = new System.Drawing.Point(12, 375);
            this.carrierFrequencyRampStopNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.carrierFrequencyRampStopNumeric.Name = "carrierFrequencyRampStopNumeric";
            this.carrierFrequencyRampStopNumeric.Size = new System.Drawing.Size(149, 20);
            this.carrierFrequencyRampStopNumeric.TabIndex = 7;
            this.carrierFrequencyRampStopNumeric.Value = new decimal(new int[] {
            1001000000,
            0,
            0,
            0});
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 28);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(155, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(12, 11);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 12;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(12, 460);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(149, 23);
            this.acquireButton.TabIndex = 9;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // multiDataGridViewResults
            // 
            this.multiDataGridViewResults.Location = new System.Drawing.Point(200, 18);
            this.multiDataGridViewResults.Name = "multiDataGridViewResults";
            this.multiDataGridViewResults.Size = new System.Drawing.Size(442, 466);
            this.multiDataGridViewResults.TabIndex = 41;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 496);
            this.Controls.Add(this.multiDataGridViewResults);
            this.Controls.Add(this.acquireButton);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.timerEventIntervalLabel);
            this.Controls.Add(this.referenceLevelRampStrtLabel);
            this.Controls.Add(this.referenceLevelRampStopLabel);
            this.Controls.Add(this.iqRateLabel);
            this.Controls.Add(this.samplesPerRecordLabel);
            this.Controls.Add(this.numberOfStepsLabel);
            this.Controls.Add(this.carrierFrequencyRampStrtLabel);
            this.Controls.Add(this.carrierFrequencyRampStopLabel);
            this.Controls.Add(this.timerEventIntervalNumeric);
            this.Controls.Add(this.referenceLevelRampStrtNumeric);
            this.Controls.Add(this.referenceLevelRampStopNumeric);
            this.Controls.Add(this.iqRateNumeric);
            this.Controls.Add(this.samplesPerRecordNumeric);
            this.Controls.Add(this.numberOfStepsNumeric);
            this.Controls.Add(this.carrierFrequencyRampStrtNumeric);
            this.Controls.Add(this.carrierFrequencyRampStopNumeric);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "RFSA List Mode Frequency and Power Sweep - Timer Triggered";
            ((System.ComponentModel.ISupportInitialize)(this.timerEventIntervalNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelRampStrtNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelRampStopNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfStepsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyRampStrtNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyRampStopNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label timerEventIntervalLabel;
        private System.Windows.Forms.Label referenceLevelRampStrtLabel;
        private System.Windows.Forms.Label referenceLevelRampStopLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label samplesPerRecordLabel;
        private System.Windows.Forms.Label numberOfStepsLabel;
        private System.Windows.Forms.Label carrierFrequencyRampStrtLabel;
        private System.Windows.Forms.Label carrierFrequencyRampStopLabel;
        private System.Windows.Forms.NumericUpDown timerEventIntervalNumeric;
        private System.Windows.Forms.NumericUpDown referenceLevelRampStrtNumeric;
        private System.Windows.Forms.NumericUpDown referenceLevelRampStopNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.NumericUpDown samplesPerRecordNumeric;
        private System.Windows.Forms.NumericUpDown numberOfStepsNumeric;
        private System.Windows.Forms.NumericUpDown carrierFrequencyRampStrtNumeric;
        private System.Windows.Forms.NumericUpDown carrierFrequencyRampStopNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Button acquireButton;
        private NationalInstruments.Examples.ListModeTimerTriggered.MultiDataGridView multiDataGridViewResults;
    }
}
