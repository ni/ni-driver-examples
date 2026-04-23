namespace NationalInstruments.Examples.PulseTriggerAcquisition
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
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.referenceLevelLabel = new System.Windows.Forms.Label();
            this.carrierFrequencyLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.referenceLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.carrierFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggerSlopeLabel = new System.Windows.Forms.Label();
            this.triggerSlopeComboBox = new System.Windows.Forms.ComboBox();
            this.referencePositionLabel = new System.Windows.Forms.Label();
            this.referencePositionNumeric = new System.Windows.Forms.NumericUpDown();
            this.burstLengthLabel = new System.Windows.Forms.Label();
            this.burstLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggerLevelLabel = new System.Windows.Forms.Label();
            this.triggerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.minimumQuietTimeLabel = new System.Windows.Forms.Label();
            this.minimumQuiteTimeNumeric = new System.Windows.Forms.NumericUpDown();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.startButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.referencePositionNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.burstLengthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimumQuiteTimeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.SuspendLayout();
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 38);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(116, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(12, 18);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 28;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // referenceLevelLabel
            // 
            this.referenceLevelLabel.AutoSize = true;
            this.referenceLevelLabel.Location = new System.Drawing.Point(12, 66);
            this.referenceLevelLabel.Name = "referenceLevelLabel";
            this.referenceLevelLabel.Size = new System.Drawing.Size(116, 13);
            this.referenceLevelLabel.TabIndex = 22;
            this.referenceLevelLabel.Text = "Reference Level (dBm)";
            // 
            // carrierFrequencyLabel
            // 
            this.carrierFrequencyLabel.AutoSize = true;
            this.carrierFrequencyLabel.Location = new System.Drawing.Point(12, 113);
            this.carrierFrequencyLabel.Name = "carrierFrequencyLabel";
            this.carrierFrequencyLabel.Size = new System.Drawing.Size(112, 13);
            this.carrierFrequencyLabel.TabIndex = 24;
            this.carrierFrequencyLabel.Text = "Carrier Frequency (Hz)";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(12, 160);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(44, 13);
            this.iqRateLabel.TabIndex = 26;
            this.iqRateLabel.Text = "IQ Rate";
            // 
            // referenceLevelNumeric
            // 
            this.referenceLevelNumeric.DecimalPlaces = 2;
            this.referenceLevelNumeric.Location = new System.Drawing.Point(12, 86);
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
            this.referenceLevelNumeric.TabIndex = 1;
            // 
            // carrierFrequencyNumeric
            // 
            this.carrierFrequencyNumeric.DecimalPlaces = 2;
            this.carrierFrequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.carrierFrequencyNumeric.Location = new System.Drawing.Point(12, 133);
            this.carrierFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.carrierFrequencyNumeric.Name = "carrierFrequencyNumeric";
            this.carrierFrequencyNumeric.Size = new System.Drawing.Size(116, 20);
            this.carrierFrequencyNumeric.TabIndex = 2;
            this.carrierFrequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // iqRateNumeric
            // 
            this.iqRateNumeric.DecimalPlaces = 2;
            this.iqRateNumeric.Location = new System.Drawing.Point(12, 180);
            this.iqRateNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.iqRateNumeric.Name = "iqRateNumeric";
            this.iqRateNumeric.Size = new System.Drawing.Size(116, 20);
            this.iqRateNumeric.TabIndex = 3;
            this.iqRateNumeric.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // triggerSlopeLabel
            // 
            this.triggerSlopeLabel.AutoSize = true;
            this.triggerSlopeLabel.Location = new System.Drawing.Point(12, 207);
            this.triggerSlopeLabel.Name = "triggerSlopeLabel";
            this.triggerSlopeLabel.Size = new System.Drawing.Size(70, 13);
            this.triggerSlopeLabel.TabIndex = 30;
            this.triggerSlopeLabel.Text = "Trigger Slope";
            // 
            // triggerSlopeComboBox
            // 
            this.triggerSlopeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerSlopeComboBox.Location = new System.Drawing.Point(12, 227);
            this.triggerSlopeComboBox.Name = "triggerSlopeComboBox";
            this.triggerSlopeComboBox.Size = new System.Drawing.Size(116, 21);
            this.triggerSlopeComboBox.TabIndex = 4;
            // 
            // referencePositionLabel
            // 
            this.referencePositionLabel.AutoSize = true;
            this.referencePositionLabel.Location = new System.Drawing.Point(12, 349);
            this.referencePositionLabel.Name = "referencePositionLabel";
            this.referencePositionLabel.Size = new System.Drawing.Size(114, 13);
            this.referencePositionLabel.TabIndex = 32;
            this.referencePositionLabel.Text = "Reference Position (%)";
            // 
            // referencePositionNumeric
            // 
            this.referencePositionNumeric.DecimalPlaces = 2;
            this.referencePositionNumeric.Location = new System.Drawing.Point(12, 369);
            this.referencePositionNumeric.Name = "referencePositionNumeric";
            this.referencePositionNumeric.Size = new System.Drawing.Size(116, 20);
            this.referencePositionNumeric.TabIndex = 7;
            // 
            // burstLengthLabel
            // 
            this.burstLengthLabel.AutoSize = true;
            this.burstLengthLabel.Location = new System.Drawing.Point(12, 302);
            this.burstLengthLabel.Name = "burstLengthLabel";
            this.burstLengthLabel.Size = new System.Drawing.Size(93, 13);
            this.burstLengthLabel.TabIndex = 34;
            this.burstLengthLabel.Text = "Burst Length (sec)";
            // 
            // burstLengthNumeric
            // 
            this.burstLengthNumeric.DecimalPlaces = 2;
            this.burstLengthNumeric.Location = new System.Drawing.Point(12, 322);
            this.burstLengthNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.burstLengthNumeric.Name = "burstLengthNumeric";
            this.burstLengthNumeric.Size = new System.Drawing.Size(116, 20);
            this.burstLengthNumeric.TabIndex = 6;
            this.burstLengthNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // triggerLevelLabel
            // 
            this.triggerLevelLabel.AutoSize = true;
            this.triggerLevelLabel.Location = new System.Drawing.Point(12, 255);
            this.triggerLevelLabel.Name = "triggerLevelLabel";
            this.triggerLevelLabel.Size = new System.Drawing.Size(99, 13);
            this.triggerLevelLabel.TabIndex = 36;
            this.triggerLevelLabel.Text = "Trigger Level (dBm)";
            // 
            // triggerLevelNumeric
            // 
            this.triggerLevelNumeric.DecimalPlaces = 2;
            this.triggerLevelNumeric.Location = new System.Drawing.Point(12, 275);
            this.triggerLevelNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.triggerLevelNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.triggerLevelNumeric.Name = "triggerLevelNumeric";
            this.triggerLevelNumeric.Size = new System.Drawing.Size(116, 20);
            this.triggerLevelNumeric.TabIndex = 5;
            // 
            // minimumQuietTimeLabel
            // 
            this.minimumQuietTimeLabel.AutoSize = true;
            this.minimumQuietTimeLabel.Location = new System.Drawing.Point(12, 396);
            this.minimumQuietTimeLabel.Name = "minimumQuietTimeLabel";
            this.minimumQuietTimeLabel.Size = new System.Drawing.Size(128, 13);
            this.minimumQuietTimeLabel.TabIndex = 38;
            this.minimumQuietTimeLabel.Text = "Minimum Quiet Time (sec)";
            // 
            // minimumQuiteTimeNumeric
            // 
            this.minimumQuiteTimeNumeric.DecimalPlaces = 2;
            this.minimumQuiteTimeNumeric.Location = new System.Drawing.Point(12, 416);
            this.minimumQuiteTimeNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.minimumQuiteTimeNumeric.Name = "minimumQuiteTimeNumeric";
            this.minimumQuiteTimeNumeric.Size = new System.Drawing.Size(116, 20);
            this.minimumQuiteTimeNumeric.TabIndex = 8;
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewResults.Location = new System.Drawing.Point(149, 18);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(438, 452);
            this.dataGridViewResults.TabIndex = 40;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(12, 443);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(116, 27);
            this.startButton.TabIndex = 9;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 480);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.minimumQuietTimeLabel);
            this.Controls.Add(this.minimumQuiteTimeNumeric);
            this.Controls.Add(this.triggerLevelLabel);
            this.Controls.Add(this.triggerLevelNumeric);
            this.Controls.Add(this.burstLengthLabel);
            this.Controls.Add(this.burstLengthNumeric);
            this.Controls.Add(this.referencePositionLabel);
            this.Controls.Add(this.referencePositionNumeric);
            this.Controls.Add(this.triggerSlopeLabel);
            this.Controls.Add(this.triggerSlopeComboBox);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.referenceLevelLabel);
            this.Controls.Add(this.carrierFrequencyLabel);
            this.Controls.Add(this.iqRateLabel);
            this.Controls.Add(this.referenceLevelNumeric);
            this.Controls.Add(this.carrierFrequencyNumeric);
            this.Controls.Add(this.iqRateNumeric);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "RFSA Pulse Trigger Acquisition";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.referencePositionNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.burstLengthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimumQuiteTimeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label referenceLevelLabel;
        private System.Windows.Forms.Label carrierFrequencyLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.NumericUpDown referenceLevelNumeric;
        private System.Windows.Forms.NumericUpDown carrierFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.Label triggerSlopeLabel;
        private System.Windows.Forms.ComboBox triggerSlopeComboBox;
        private System.Windows.Forms.Label referencePositionLabel;
        private System.Windows.Forms.NumericUpDown referencePositionNumeric;
        private System.Windows.Forms.Label burstLengthLabel;
        private System.Windows.Forms.NumericUpDown burstLengthNumeric;
        private System.Windows.Forms.Label triggerLevelLabel;
        private System.Windows.Forms.NumericUpDown triggerLevelNumeric;
        private System.Windows.Forms.Label minimumQuietTimeLabel;
        private System.Windows.Forms.NumericUpDown minimumQuiteTimeNumeric;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.Button startButton;
    }
}

