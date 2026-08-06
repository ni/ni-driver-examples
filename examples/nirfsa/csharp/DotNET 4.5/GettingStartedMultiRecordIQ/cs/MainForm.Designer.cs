namespace NationalInstruments.Examples.GettingStartedMultiRecordIQ
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
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.rfsaResourceNameLabel = new System.Windows.Forms.Label();
            this.referenceLevelLabel = new System.Windows.Forms.Label();
            this.carrierFrequencyLabel = new System.Windows.Forms.Label();
            this.referenceLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.carrierFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.referenceClockLabel = new System.Windows.Forms.Label();
            this.referenceClockComboBox = new System.Windows.Forms.ComboBox();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerRecordLabel = new System.Windows.Forms.Label();
            this.samplesPerRecordNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggerTypeLabel = new System.Windows.Forms.Label();
            this.pretriggerSamplesLabel = new System.Windows.Forms.Label();
            this.pretriggerSamplesNumeric = new System.Windows.Forms.NumericUpDown();
            this.referenceTriggerTypeComboBox = new System.Windows.Forms.ComboBox();
            this.triggerLevelLabel = new System.Windows.Forms.Label();
            this.triggerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.sendSoftwareTriggerbutton = new System.Windows.Forms.Button();
            this.acquireButton = new System.Windows.Forms.Button();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.triggerSourceComboBox = new System.Windows.Forms.ComboBox();
            this.numberOfRecordsLabel = new System.Windows.Forms.Label();
            this.numberOfRecordsNumeric = new System.Windows.Forms.NumericUpDown();
            this.multiDataGridViewResults = new NationalInstruments.Examples.GettingStartedMultiRecordIQ.MultiDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pretriggerSamplesNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfRecordsNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(18, 35);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(116, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // rfsaResourceNameLabel
            // 
            this.rfsaResourceNameLabel.AutoSize = true;
            this.rfsaResourceNameLabel.Location = new System.Drawing.Point(18, 15);
            this.rfsaResourceNameLabel.Name = "rfsaResourceNameLabel";
            this.rfsaResourceNameLabel.Size = new System.Drawing.Size(115, 13);
            this.rfsaResourceNameLabel.TabIndex = 23;
            this.rfsaResourceNameLabel.Text = "RFSA Resource Name";
            // 
            // referenceLevelLabel
            // 
            this.referenceLevelLabel.AutoSize = true;
            this.referenceLevelLabel.Location = new System.Drawing.Point(18, 111);
            this.referenceLevelLabel.Name = "referenceLevelLabel";
            this.referenceLevelLabel.Size = new System.Drawing.Size(116, 13);
            this.referenceLevelLabel.TabIndex = 18;
            this.referenceLevelLabel.Text = "Reference Level (dBm)";
            // 
            // carrierFrequencyLabel
            // 
            this.carrierFrequencyLabel.AutoSize = true;
            this.carrierFrequencyLabel.Location = new System.Drawing.Point(18, 158);
            this.carrierFrequencyLabel.Name = "carrierFrequencyLabel";
            this.carrierFrequencyLabel.Size = new System.Drawing.Size(112, 13);
            this.carrierFrequencyLabel.TabIndex = 21;
            this.carrierFrequencyLabel.Text = "Carrier Frequency (Hz)";
            // 
            // referenceLevelNumeric
            // 
            this.referenceLevelNumeric.DecimalPlaces = 2;
            this.referenceLevelNumeric.Location = new System.Drawing.Point(18, 131);
            this.referenceLevelNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.referenceLevelNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.referenceLevelNumeric.Name = "referenceLevelNumeric";
            this.referenceLevelNumeric.Size = new System.Drawing.Size(116, 20);
            this.referenceLevelNumeric.TabIndex = 2;
            // 
            // carrierFrequencyNumeric
            // 
            this.carrierFrequencyNumeric.DecimalPlaces = 2;
            this.carrierFrequencyNumeric.Location = new System.Drawing.Point(18, 178);
            this.carrierFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.carrierFrequencyNumeric.Name = "carrierFrequencyNumeric";
            this.carrierFrequencyNumeric.Size = new System.Drawing.Size(116, 20);
            this.carrierFrequencyNumeric.TabIndex = 3;
            this.carrierFrequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // referenceClockLabel
            // 
            this.referenceClockLabel.AutoSize = true;
            this.referenceClockLabel.Location = new System.Drawing.Point(18, 63);
            this.referenceClockLabel.Name = "referenceClockLabel";
            this.referenceClockLabel.Size = new System.Drawing.Size(87, 13);
            this.referenceClockLabel.TabIndex = 24;
            this.referenceClockLabel.Text = "Reference Clock";
            // 
            // referenceClockComboBox
            // 
            this.referenceClockComboBox.Location = new System.Drawing.Point(18, 83);
            this.referenceClockComboBox.Name = "referenceClockComboBox";
            this.referenceClockComboBox.Size = new System.Drawing.Size(116, 21);
            this.referenceClockComboBox.TabIndex = 1;
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(18, 205);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(44, 13);
            this.iqRateLabel.TabIndex = 26;
            this.iqRateLabel.Text = "IQ Rate";
            // 
            // iqRateNumeric
            // 
            this.iqRateNumeric.DecimalPlaces = 2;
            this.iqRateNumeric.Location = new System.Drawing.Point(18, 225);
            this.iqRateNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.iqRateNumeric.Name = "iqRateNumeric";
            this.iqRateNumeric.Size = new System.Drawing.Size(116, 20);
            this.iqRateNumeric.TabIndex = 4;
            this.iqRateNumeric.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // samplesPerRecordLabel
            // 
            this.samplesPerRecordLabel.AutoSize = true;
            this.samplesPerRecordLabel.Location = new System.Drawing.Point(18, 319);
            this.samplesPerRecordLabel.Name = "samplesPerRecordLabel";
            this.samplesPerRecordLabel.Size = new System.Drawing.Size(103, 13);
            this.samplesPerRecordLabel.TabIndex = 28;
            this.samplesPerRecordLabel.Text = "Samples per Record";
            // 
            // samplesPerRecordNumeric
            // 
            this.samplesPerRecordNumeric.Location = new System.Drawing.Point(18, 339);
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
            this.samplesPerRecordNumeric.Size = new System.Drawing.Size(116, 20);
            this.samplesPerRecordNumeric.TabIndex = 6;
            this.samplesPerRecordNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // triggerTypeLabel
            // 
            this.triggerTypeLabel.AutoSize = true;
            this.triggerTypeLabel.Location = new System.Drawing.Point(18, 366);
            this.triggerTypeLabel.Name = "triggerTypeLabel";
            this.triggerTypeLabel.Size = new System.Drawing.Size(120, 13);
            this.triggerTypeLabel.TabIndex = 30;
            this.triggerTypeLabel.Text = "Reference Trigger Type";
            // 
            // pretriggerSamplesLabel
            // 
            this.pretriggerSamplesLabel.AutoSize = true;
            this.pretriggerSamplesLabel.Location = new System.Drawing.Point(18, 414);
            this.pretriggerSamplesLabel.Name = "pretriggerSamplesLabel";
            this.pretriggerSamplesLabel.Size = new System.Drawing.Size(95, 13);
            this.pretriggerSamplesLabel.TabIndex = 32;
            this.pretriggerSamplesLabel.Text = "Pretrigger Samples";
            // 
            // pretriggerSamplesNumeric
            // 
            this.pretriggerSamplesNumeric.Location = new System.Drawing.Point(18, 434);
            this.pretriggerSamplesNumeric.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.pretriggerSamplesNumeric.Name = "pretriggerSamplesNumeric";
            this.pretriggerSamplesNumeric.Size = new System.Drawing.Size(116, 20);
            this.pretriggerSamplesNumeric.TabIndex = 8;
            // 
            // referenceTriggerTypeComboBox
            // 
            this.referenceTriggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.referenceTriggerTypeComboBox.Location = new System.Drawing.Point(18, 386);
            this.referenceTriggerTypeComboBox.Name = "referenceTriggerTypeComboBox";
            this.referenceTriggerTypeComboBox.Size = new System.Drawing.Size(116, 21);
            this.referenceTriggerTypeComboBox.TabIndex = 7;
            this.referenceTriggerTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.referenceTriggerTypeComboBox_SelectedIndexChanged);
            // 
            // triggerLevelLabel
            // 
            this.triggerLevelLabel.AutoSize = true;
            this.triggerLevelLabel.Location = new System.Drawing.Point(18, 461);
            this.triggerLevelLabel.Name = "triggerLevelLabel";
            this.triggerLevelLabel.Size = new System.Drawing.Size(99, 13);
            this.triggerLevelLabel.TabIndex = 36;
            this.triggerLevelLabel.Text = "Trigger Level (dBm)";
            // 
            // triggerLevelNumeric
            // 
            this.triggerLevelNumeric.DecimalPlaces = 2;
            this.triggerLevelNumeric.Location = new System.Drawing.Point(18, 481);
            this.triggerLevelNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.triggerLevelNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.triggerLevelNumeric.Name = "triggerLevelNumeric";
            this.triggerLevelNumeric.Size = new System.Drawing.Size(116, 20);
            this.triggerLevelNumeric.TabIndex = 8;
            // 
            // sendSoftwareTriggerbutton
            // 
            this.sendSoftwareTriggerbutton.Enabled = false;
            this.sendSoftwareTriggerbutton.Location = new System.Drawing.Point(153, 545);
            this.sendSoftwareTriggerbutton.Name = "sendSoftwareTriggerbutton";
            this.sendSoftwareTriggerbutton.Size = new System.Drawing.Size(140, 30);
            this.sendSoftwareTriggerbutton.TabIndex = 11;
            this.sendSoftwareTriggerbutton.Text = "&Send Software Trigger";
            this.sendSoftwareTriggerbutton.UseVisualStyleBackColor = true;
            this.sendSoftwareTriggerbutton.Click += new System.EventHandler(this.sendSoftwareTriggerbutton_Click);
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(17, 545);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(116, 30);
            this.acquireButton.TabIndex = 9;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.AutoSize = true;
            this.triggerSourceLabel.Location = new System.Drawing.Point(18, 461);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(77, 13);
            this.triggerSourceLabel.TabIndex = 37;
            this.triggerSourceLabel.Text = "Trigger Source";
            // 
            // triggerSourceComboBox
            // 
            this.triggerSourceComboBox.Location = new System.Drawing.Point(18, 481);
            this.triggerSourceComboBox.Name = "triggerSourceComboBox";
            this.triggerSourceComboBox.Size = new System.Drawing.Size(116, 21);
            this.triggerSourceComboBox.TabIndex = 38;
            // 
            // numberOfRecordsLabel
            // 
            this.numberOfRecordsLabel.AutoSize = true;
            this.numberOfRecordsLabel.Location = new System.Drawing.Point(18, 263);
            this.numberOfRecordsLabel.Name = "numberOfRecordsLabel";
            this.numberOfRecordsLabel.Size = new System.Drawing.Size(99, 13);
            this.numberOfRecordsLabel.TabIndex = 40;
            this.numberOfRecordsLabel.Text = "Number of Records";
            // 
            // numberOfRecordsNumeric
            // 
            this.numberOfRecordsNumeric.Location = new System.Drawing.Point(18, 283);
            this.numberOfRecordsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numberOfRecordsNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfRecordsNumeric.Name = "numberOfRecordsNumeric";
            this.numberOfRecordsNumeric.Size = new System.Drawing.Size(116, 20);
            this.numberOfRecordsNumeric.TabIndex = 5;
            this.numberOfRecordsNumeric.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // multiDataGridViewResults
            // 
            this.multiDataGridViewResults.Location = new System.Drawing.Point(164, 15);
            this.multiDataGridViewResults.Name = "multiDataGridViewResults";
            this.multiDataGridViewResults.Size = new System.Drawing.Size(442, 466);
            this.multiDataGridViewResults.TabIndex = 41;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 605);
            this.Controls.Add(this.multiDataGridViewResults);
            this.Controls.Add(this.numberOfRecordsLabel);
            this.Controls.Add(this.numberOfRecordsNumeric);
            this.Controls.Add(this.triggerSourceLabel);
            this.Controls.Add(this.triggerSourceComboBox);
            this.Controls.Add(this.sendSoftwareTriggerbutton);
            this.Controls.Add(this.acquireButton);
            this.Controls.Add(this.triggerLevelLabel);
            this.Controls.Add(this.triggerLevelNumeric);
            this.Controls.Add(this.triggerTypeLabel);
            this.Controls.Add(this.pretriggerSamplesLabel);
            this.Controls.Add(this.pretriggerSamplesNumeric);
            this.Controls.Add(this.referenceTriggerTypeComboBox);
            this.Controls.Add(this.samplesPerRecordLabel);
            this.Controls.Add(this.samplesPerRecordNumeric);
            this.Controls.Add(this.iqRateLabel);
            this.Controls.Add(this.iqRateNumeric);
            this.Controls.Add(this.referenceClockLabel);
            this.Controls.Add(this.referenceClockComboBox);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.rfsaResourceNameLabel);
            this.Controls.Add(this.referenceLevelLabel);
            this.Controls.Add(this.carrierFrequencyLabel);
            this.Controls.Add(this.referenceLevelNumeric);
            this.Controls.Add(this.carrierFrequencyNumeric);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Getting Started MultiRecord IQ";
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pretriggerSamplesNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfRecordsNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label rfsaResourceNameLabel;
        private System.Windows.Forms.Label referenceLevelLabel;
        private System.Windows.Forms.Label carrierFrequencyLabel;
        private System.Windows.Forms.NumericUpDown referenceLevelNumeric;
        private System.Windows.Forms.NumericUpDown carrierFrequencyNumeric;
        private System.Windows.Forms.Label referenceClockLabel;
        private System.Windows.Forms.ComboBox referenceClockComboBox;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.Label samplesPerRecordLabel;
        private System.Windows.Forms.NumericUpDown samplesPerRecordNumeric;
        private System.Windows.Forms.Label triggerTypeLabel;
        private System.Windows.Forms.Label pretriggerSamplesLabel;
        private System.Windows.Forms.NumericUpDown pretriggerSamplesNumeric;
        private System.Windows.Forms.ComboBox referenceTriggerTypeComboBox;
        private System.Windows.Forms.Label triggerLevelLabel;
        private System.Windows.Forms.NumericUpDown triggerLevelNumeric;        
        private System.Windows.Forms.Button sendSoftwareTriggerbutton;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.Label triggerSourceLabel;
        private System.Windows.Forms.ComboBox triggerSourceComboBox;
        private System.Windows.Forms.Label numberOfRecordsLabel;
        private System.Windows.Forms.NumericUpDown numberOfRecordsNumeric;
        private NationalInstruments.Examples.GettingStartedMultiRecordIQ.MultiDataGridView multiDataGridViewResults;
    }
}

