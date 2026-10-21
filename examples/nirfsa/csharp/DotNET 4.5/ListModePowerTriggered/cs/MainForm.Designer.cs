namespace NationalInstruments.Examples.ListModePowerTriggered
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
            this.carrierFreqRampStrtLabel = new System.Windows.Forms.Label();
            this.carrierFreqRampStopLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.samplesPerRecordLabel = new System.Windows.Forms.Label();
            this.numberOfStepsLabel = new System.Windows.Forms.Label();
            this.referenceLevelRampStrtLabel = new System.Windows.Forms.Label();
            this.referenceLevelRampStopLabel = new System.Windows.Forms.Label();
            this.triggerLevelRampStrtLabel = new System.Windows.Forms.Label();
            this.triggerLevelRampStopLabel = new System.Windows.Forms.Label();
            this.carrierFreqRampStrtNumeric = new System.Windows.Forms.NumericUpDown();
            this.carrierFreqRampStopNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerRecordNumeric = new System.Windows.Forms.NumericUpDown();
            this.numberOfStepsNumeric = new System.Windows.Forms.NumericUpDown();
            this.referenceLevelRampStrtNumeric = new System.Windows.Forms.NumericUpDown();
            this.referenceLevelRampStopNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggerLevelRampStrtNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggerLevelRampStopNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.acquireButton = new System.Windows.Forms.Button();
            this.multiDataGridViewResults = new NationalInstruments.Examples.ListModePowerTriggered.MultiDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFreqRampStrtNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFreqRampStopNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfStepsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelRampStrtNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelRampStopNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelRampStrtNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelRampStopNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // carrierFreqRampStrtLabel
            // 
            this.carrierFreqRampStrtLabel.AutoSize = true;
            this.carrierFreqRampStrtLabel.Location = new System.Drawing.Point(12, 200);
            this.carrierFreqRampStrtLabel.Name = "carrierFreqRampStrtLabel";
            this.carrierFreqRampStrtLabel.Size = new System.Drawing.Size(182, 13);
            this.carrierFreqRampStrtLabel.TabIndex = 0;
            this.carrierFreqRampStrtLabel.Text = "IQ Carrier Frequency Ramp Start (Hz)";
            // 
            // carrierFreqRampStopLabel
            // 
            this.carrierFreqRampStopLabel.AutoSize = true;
            this.carrierFreqRampStopLabel.Location = new System.Drawing.Point(12, 247);
            this.carrierFreqRampStopLabel.Name = "carrierFreqRampStopLabel";
            this.carrierFreqRampStopLabel.Size = new System.Drawing.Size(182, 13);
            this.carrierFreqRampStopLabel.TabIndex = 1;
            this.carrierFreqRampStopLabel.Text = "IQ Carrier Frequency Ramp Stop (Hz)";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(12, 56);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 2;
            this.iqRateLabel.Text = "IQ Rate (S/s)";
            // 
            // samplesPerRecordLabel
            // 
            this.samplesPerRecordLabel.AutoSize = true;
            this.samplesPerRecordLabel.Location = new System.Drawing.Point(12, 104);
            this.samplesPerRecordLabel.Name = "samplesPerRecordLabel";
            this.samplesPerRecordLabel.Size = new System.Drawing.Size(103, 13);
            this.samplesPerRecordLabel.TabIndex = 3;
            this.samplesPerRecordLabel.Text = "Samples per Record";
            // 
            // numberOfStepsLabel
            // 
            this.numberOfStepsLabel.AutoSize = true;
            this.numberOfStepsLabel.Location = new System.Drawing.Point(12, 150);
            this.numberOfStepsLabel.Name = "numberOfStepsLabel";
            this.numberOfStepsLabel.Size = new System.Drawing.Size(86, 13);
            this.numberOfStepsLabel.TabIndex = 4;
            this.numberOfStepsLabel.Text = "Number of Steps";
            // 
            // referenceLevelRampStrtLabel
            // 
            this.referenceLevelRampStrtLabel.AutoSize = true;
            this.referenceLevelRampStrtLabel.Location = new System.Drawing.Point(12, 301);
            this.referenceLevelRampStrtLabel.Name = "referenceLevelRampStrtLabel";
            this.referenceLevelRampStrtLabel.Size = new System.Drawing.Size(156, 13);
            this.referenceLevelRampStrtLabel.TabIndex = 5;
            this.referenceLevelRampStrtLabel.Text = "Reference Level Ramp Start (s)";
            // 
            // referenceLevelRampStopLabel
            // 
            this.referenceLevelRampStopLabel.AutoSize = true;
            this.referenceLevelRampStopLabel.Location = new System.Drawing.Point(12, 347);
            this.referenceLevelRampStopLabel.Name = "referenceLevelRampStopLabel";
            this.referenceLevelRampStopLabel.Size = new System.Drawing.Size(156, 13);
            this.referenceLevelRampStopLabel.TabIndex = 6;
            this.referenceLevelRampStopLabel.Text = "Reference Level Ramp Stop (s)";
            // 
            // triggerLevelRampStrtLabel
            // 
            this.triggerLevelRampStrtLabel.AutoSize = true;
            this.triggerLevelRampStrtLabel.Location = new System.Drawing.Point(12, 397);
            this.triggerLevelRampStrtLabel.Name = "triggerLevelRampStrtLabel";
            this.triggerLevelRampStrtLabel.Size = new System.Drawing.Size(188, 13);
            this.triggerLevelRampStrtLabel.TabIndex = 7;
            this.triggerLevelRampStrtLabel.Text = "Power Trigger Level Ramp Start (dBm)";
            // 
            // triggerLevelRampStopLabel
            // 
            this.triggerLevelRampStopLabel.AutoSize = true;
            this.triggerLevelRampStopLabel.Location = new System.Drawing.Point(12, 449);
            this.triggerLevelRampStopLabel.Name = "triggerLevelRampStopLabel";
            this.triggerLevelRampStopLabel.Size = new System.Drawing.Size(188, 13);
            this.triggerLevelRampStopLabel.TabIndex = 8;
            this.triggerLevelRampStopLabel.Text = "Power Trigger Level Ramp Stop (dBm)";
            // 
            // carrierFreqRampStrtNumeric
            // 
            this.carrierFreqRampStrtNumeric.DecimalPlaces = 4;
            this.carrierFreqRampStrtNumeric.Location = new System.Drawing.Point(12, 221);
            this.carrierFreqRampStrtNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.carrierFreqRampStrtNumeric.Name = "carrierFreqRampStrtNumeric";
            this.carrierFreqRampStrtNumeric.Size = new System.Drawing.Size(149, 20);
            this.carrierFreqRampStrtNumeric.TabIndex = 4;
            this.carrierFreqRampStrtNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // carrierFreqRampStopNumeric
            // 
            this.carrierFreqRampStopNumeric.DecimalPlaces = 4;
            this.carrierFreqRampStopNumeric.Location = new System.Drawing.Point(12, 268);
            this.carrierFreqRampStopNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.carrierFreqRampStopNumeric.Name = "carrierFreqRampStopNumeric";
            this.carrierFreqRampStopNumeric.Size = new System.Drawing.Size(149, 20);
            this.carrierFreqRampStopNumeric.TabIndex = 5;
            this.carrierFreqRampStopNumeric.Value = new decimal(new int[] {
            1020000000,
            0,
            0,
            0});
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
            5000000,
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
            this.numberOfStepsNumeric.Location = new System.Drawing.Point(12, 171);
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
            // referenceLevelRampStrtNumeric
            // 
            this.referenceLevelRampStrtNumeric.DecimalPlaces = 2;
            this.referenceLevelRampStrtNumeric.Location = new System.Drawing.Point(12, 322);
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
            this.referenceLevelRampStrtNumeric.TabIndex = 6;
            this.referenceLevelRampStrtNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // referenceLevelRampStopNumeric
            // 
            this.referenceLevelRampStopNumeric.DecimalPlaces = 2;
            this.referenceLevelRampStopNumeric.Location = new System.Drawing.Point(12, 368);
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
            this.referenceLevelRampStopNumeric.TabIndex = 7;
            // 
            // triggerLevelRampStrtNumeric
            // 
            this.triggerLevelRampStrtNumeric.DecimalPlaces = 2;
            this.triggerLevelRampStrtNumeric.Location = new System.Drawing.Point(12, 418);
            this.triggerLevelRampStrtNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.triggerLevelRampStrtNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.triggerLevelRampStrtNumeric.Name = "triggerLevelRampStrtNumeric";
            this.triggerLevelRampStrtNumeric.Size = new System.Drawing.Size(149, 20);
            this.triggerLevelRampStrtNumeric.TabIndex = 8;
            this.triggerLevelRampStrtNumeric.Value = new decimal(new int[] {
            30,
            0,
            0,
            -2147483648});
            // 
            // triggerLevelRampStopNumeric
            // 
            this.triggerLevelRampStopNumeric.DecimalPlaces = 2;
            this.triggerLevelRampStopNumeric.Location = new System.Drawing.Point(12, 470);
            this.triggerLevelRampStopNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.triggerLevelRampStopNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.triggerLevelRampStopNumeric.Name = "triggerLevelRampStopNumeric";
            this.triggerLevelRampStopNumeric.Size = new System.Drawing.Size(149, 20);
            this.triggerLevelRampStopNumeric.TabIndex = 9;
            this.triggerLevelRampStopNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 31);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(151, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(12, 14);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 10;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(12, 507);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(149, 23);
            this.acquireButton.TabIndex = 10;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // multiDataGridViewResults
            // 
            this.multiDataGridViewResults.Location = new System.Drawing.Point(206, 24);
            this.multiDataGridViewResults.Name = "multiDataGridViewResults";
            this.multiDataGridViewResults.Size = new System.Drawing.Size(442, 466);
            this.multiDataGridViewResults.TabIndex = 41;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 541);
            this.Controls.Add(this.multiDataGridViewResults);
            this.Controls.Add(this.acquireButton);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.carrierFreqRampStrtLabel);
            this.Controls.Add(this.carrierFreqRampStopLabel);
            this.Controls.Add(this.iqRateLabel);
            this.Controls.Add(this.samplesPerRecordLabel);
            this.Controls.Add(this.numberOfStepsLabel);
            this.Controls.Add(this.referenceLevelRampStrtLabel);
            this.Controls.Add(this.referenceLevelRampStopLabel);
            this.Controls.Add(this.triggerLevelRampStrtLabel);
            this.Controls.Add(this.triggerLevelRampStopLabel);
            this.Controls.Add(this.carrierFreqRampStrtNumeric);
            this.Controls.Add(this.carrierFreqRampStopNumeric);
            this.Controls.Add(this.iqRateNumeric);
            this.Controls.Add(this.samplesPerRecordNumeric);
            this.Controls.Add(this.numberOfStepsNumeric);
            this.Controls.Add(this.referenceLevelRampStrtNumeric);
            this.Controls.Add(this.referenceLevelRampStopNumeric);
            this.Controls.Add(this.triggerLevelRampStrtNumeric);
            this.Controls.Add(this.triggerLevelRampStopNumeric);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "RFSA List Mode Frequency and Power Sweep - Power Triggered";
            ((System.ComponentModel.ISupportInitialize)(this.carrierFreqRampStrtNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFreqRampStopNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfStepsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelRampStrtNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelRampStopNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelRampStrtNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelRampStopNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label carrierFreqRampStrtLabel;
        private System.Windows.Forms.Label carrierFreqRampStopLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label samplesPerRecordLabel;
        private System.Windows.Forms.Label numberOfStepsLabel;
        private System.Windows.Forms.Label referenceLevelRampStrtLabel;
        private System.Windows.Forms.Label referenceLevelRampStopLabel;
        private System.Windows.Forms.Label triggerLevelRampStrtLabel;
        private System.Windows.Forms.Label triggerLevelRampStopLabel;
        private System.Windows.Forms.NumericUpDown carrierFreqRampStrtNumeric;
        private System.Windows.Forms.NumericUpDown carrierFreqRampStopNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.NumericUpDown samplesPerRecordNumeric;
        private System.Windows.Forms.NumericUpDown numberOfStepsNumeric;
        private System.Windows.Forms.NumericUpDown referenceLevelRampStrtNumeric;
        private System.Windows.Forms.NumericUpDown referenceLevelRampStopNumeric;
        private System.Windows.Forms.NumericUpDown triggerLevelRampStrtNumeric;
        private System.Windows.Forms.NumericUpDown triggerLevelRampStopNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Button acquireButton;
        private NationalInstruments.Examples.ListModePowerTriggered.MultiDataGridView multiDataGridViewResults;

    }
}
