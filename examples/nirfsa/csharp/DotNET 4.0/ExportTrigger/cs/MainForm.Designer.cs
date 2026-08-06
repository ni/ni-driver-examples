namespace NationalInstruments.Examples.RfsaExportTrigger
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
            this.samplesPerRecordLabel = new System.Windows.Forms.Label();
            this.referenceLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.carrierFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerRecordNumeric = new System.Windows.Forms.NumericUpDown();
            this.outputTerminalLabel = new System.Windows.Forms.Label();
            this.outputTerminalComboBox = new System.Windows.Forms.ComboBox();
            this.startAcquisitionButton = new System.Windows.Forms.Button();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.SuspendLayout();
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 30);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(110, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(15, 10);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 20;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // referenceLevelLabel
            // 
            this.referenceLevelLabel.AutoSize = true;
            this.referenceLevelLabel.Location = new System.Drawing.Point(12, 56);
            this.referenceLevelLabel.Name = "referenceLevelLabel";
            this.referenceLevelLabel.Size = new System.Drawing.Size(116, 13);
            this.referenceLevelLabel.TabIndex = 11;
            this.referenceLevelLabel.Text = "Reference Level (dBm)";
            // 
            // carrierFrequencyLabel
            // 
            this.carrierFrequencyLabel.AutoSize = true;
            this.carrierFrequencyLabel.Location = new System.Drawing.Point(12, 101);
            this.carrierFrequencyLabel.Name = "carrierFrequencyLabel";
            this.carrierFrequencyLabel.Size = new System.Drawing.Size(112, 13);
            this.carrierFrequencyLabel.TabIndex = 13;
            this.carrierFrequencyLabel.Text = "Carrier Frequency (Hz)";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(12, 147);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(44, 13);
            this.iqRateLabel.TabIndex = 15;
            this.iqRateLabel.Text = "IQ Rate";
            // 
            // samplesPerRecordLabel
            // 
            this.samplesPerRecordLabel.AutoSize = true;
            this.samplesPerRecordLabel.Location = new System.Drawing.Point(12, 193);
            this.samplesPerRecordLabel.Name = "samplesPerRecordLabel";
            this.samplesPerRecordLabel.Size = new System.Drawing.Size(103, 13);
            this.samplesPerRecordLabel.TabIndex = 16;
            this.samplesPerRecordLabel.Text = "Samples per Record";
            // 
            // referenceLevelNumeric
            // 
            this.referenceLevelNumeric.DecimalPlaces = 2;
            this.referenceLevelNumeric.Location = new System.Drawing.Point(12, 75);
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
            this.carrierFrequencyNumeric.Location = new System.Drawing.Point(12, 121);
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
            this.iqRateNumeric.Location = new System.Drawing.Point(12, 168);
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
            // samplesPerRecordNumeric
            // 
            this.samplesPerRecordNumeric.Location = new System.Drawing.Point(12, 215);
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
            this.samplesPerRecordNumeric.TabIndex = 4;
            this.samplesPerRecordNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // outputTerminalLabel
            // 
            this.outputTerminalLabel.AutoSize = true;
            this.outputTerminalLabel.Location = new System.Drawing.Point(12, 249);
            this.outputTerminalLabel.Name = "outputTerminalLabel";
            this.outputTerminalLabel.Size = new System.Drawing.Size(82, 13);
            this.outputTerminalLabel.TabIndex = 22;
            this.outputTerminalLabel.Text = "Output Terminal";
            // 
            // outputTerminalComboBox
            // 
            this.outputTerminalComboBox.Location = new System.Drawing.Point(12, 267);
            this.outputTerminalComboBox.Name = "outputTerminalComboBox";
            this.outputTerminalComboBox.Size = new System.Drawing.Size(116, 21);
            this.outputTerminalComboBox.TabIndex = 5;
            // 
            // startAcquisitionButton
            // 
            this.startAcquisitionButton.Location = new System.Drawing.Point(12, 314);
            this.startAcquisitionButton.Name = "startAcquisitionButton";
            this.startAcquisitionButton.Size = new System.Drawing.Size(116, 30);
            this.startAcquisitionButton.TabIndex = 6;
            this.startAcquisitionButton.Text = "&Start Acquisition";
            this.startAcquisitionButton.UseVisualStyleBackColor = true;
            this.startAcquisitionButton.Click += new System.EventHandler(this.startAcquisitionButton_Click);
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewResults.Location = new System.Drawing.Point(134, 10);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(362, 334);
            this.dataGridViewResults.TabIndex = 25;
            // 
            // MainForm
            // 
            this.AcceptButton = this.startAcquisitionButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 351);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.startAcquisitionButton);
            this.Controls.Add(this.outputTerminalLabel);
            this.Controls.Add(this.outputTerminalComboBox);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.referenceLevelLabel);
            this.Controls.Add(this.carrierFrequencyLabel);
            this.Controls.Add(this.iqRateLabel);
            this.Controls.Add(this.samplesPerRecordLabel);
            this.Controls.Add(this.referenceLevelNumeric);
            this.Controls.Add(this.carrierFrequencyNumeric);
            this.Controls.Add(this.iqRateNumeric);
            this.Controls.Add(this.samplesPerRecordNumeric);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Rfsa Export Trigger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).EndInit();
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
        private System.Windows.Forms.Label samplesPerRecordLabel;
        private System.Windows.Forms.NumericUpDown referenceLevelNumeric;
        private System.Windows.Forms.NumericUpDown carrierFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.NumericUpDown samplesPerRecordNumeric;
        private System.Windows.Forms.Label outputTerminalLabel;
        private System.Windows.Forms.ComboBox outputTerminalComboBox;
        private System.Windows.Forms.Button startAcquisitionButton;
        private System.Windows.Forms.DataGridView dataGridViewResults;
    }
}

