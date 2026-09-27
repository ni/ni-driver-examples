namespace NationalInstruments.Examples.InbandRetuningWithInternalLO
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
            this.referenceClockLabel = new System.Windows.Forms.Label();
            this.referenceLevelLabel = new System.Windows.Forms.Label();
            this.centerFrequencyLabel = new System.Windows.Forms.Label();
            this.downconverterCenterFrequencyLabel = new System.Windows.Forms.Label();
            this.spanLabel = new System.Windows.Forms.Label();
            this.resolutionBandwidthLabel = new System.Windows.Forms.Label();
            this.referenceLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.centerFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.downconverterCenterFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.spanNumeric = new System.Windows.Forms.NumericUpDown();
            this.resolutionBandwidthNumeric = new System.Windows.Forms.NumericUpDown();
            this.referenceClockComboBox = new System.Windows.Forms.ComboBox();
            this.rfsaResourceNameLabel = new System.Windows.Forms.Label();
            this.readPowerSpectrumButton = new System.Windows.Forms.Button();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.rfsaResourceNameComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downconverterCenterFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spanNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resolutionBandwidthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.SuspendLayout();
            // 
            // referenceClockLabel
            // 
            this.referenceClockLabel.Location = new System.Drawing.Point(12, 47);
            this.referenceClockLabel.Name = "referenceClockLabel";
            this.referenceClockLabel.Size = new System.Drawing.Size(109, 15);
            this.referenceClockLabel.TabIndex = 0;
            this.referenceClockLabel.Text = "Reference Clock";
            // 
            // referenceLevelLabel
            // 
            this.referenceLevelLabel.Location = new System.Drawing.Point(12, 89);
            this.referenceLevelLabel.Name = "referenceLevelLabel";
            this.referenceLevelLabel.Size = new System.Drawing.Size(109, 15);
            this.referenceLevelLabel.TabIndex = 1;
            this.referenceLevelLabel.Text = "Reference Level (dBm)";
            // 
            // centerFrequencyLabel
            // 
            this.centerFrequencyLabel.AutoSize = true;
            this.centerFrequencyLabel.Location = new System.Drawing.Point(12, 135);
            this.centerFrequencyLabel.Name = "centerFrequencyLabel";
            this.centerFrequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.centerFrequencyLabel.TabIndex = 2;
            this.centerFrequencyLabel.Text = "Center Frequency (Hz)";
            // 
            // downconverterCenterFrequencyLabel
            // 
            this.downconverterCenterFrequencyLabel.Location = new System.Drawing.Point(12, 258);
            this.downconverterCenterFrequencyLabel.Name = "downconverterCenterFrequencyLabel";
            this.downconverterCenterFrequencyLabel.Size = new System.Drawing.Size(109, 15);
            this.downconverterCenterFrequencyLabel.TabIndex = 3;
            this.downconverterCenterFrequencyLabel.Text = "Downconverter Frequency (Hz)";
            // 
            // spanLabel
            // 
            this.spanLabel.Location = new System.Drawing.Point(12, 176);
            this.spanLabel.Name = "spanLabel";
            this.spanLabel.Size = new System.Drawing.Size(109, 15);
            this.spanLabel.TabIndex = 4;
            this.spanLabel.Text = "Span (Hz)";
            // 
            // resolutionBandwidthLabel
            // 
            this.resolutionBandwidthLabel.Location = new System.Drawing.Point(12, 216);
            this.resolutionBandwidthLabel.Name = "resolutionBandwidthLabel";
            this.resolutionBandwidthLabel.Size = new System.Drawing.Size(115, 15);
            this.resolutionBandwidthLabel.TabIndex = 5;
            this.resolutionBandwidthLabel.Text = "Resolution Bandwidth (Hz)";
            // 
            // referenceLevelNumeric
            // 
            this.referenceLevelNumeric.DecimalPlaces = 2;
            this.referenceLevelNumeric.Location = new System.Drawing.Point(12, 108);
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
            this.referenceLevelNumeric.Size = new System.Drawing.Size(120, 20);
            this.referenceLevelNumeric.TabIndex = 2;
            // 
            // centerFrequencyNumeric
            // 
            this.centerFrequencyNumeric.DecimalPlaces = 2;
            this.centerFrequencyNumeric.Location = new System.Drawing.Point(12, 152);
            this.centerFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.centerFrequencyNumeric.Name = "centerFrequencyNumeric";
            this.centerFrequencyNumeric.Size = new System.Drawing.Size(120, 20);
            this.centerFrequencyNumeric.TabIndex = 3;
            this.centerFrequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // downconverterCenterFrequencyNumeric
            // 
            this.downconverterCenterFrequencyNumeric.DecimalPlaces = 2;
            this.downconverterCenterFrequencyNumeric.Location = new System.Drawing.Point(12, 276);
            this.downconverterCenterFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.downconverterCenterFrequencyNumeric.Name = "downconverterCenterFrequencyNumeric";
            this.downconverterCenterFrequencyNumeric.Size = new System.Drawing.Size(120, 20);
            this.downconverterCenterFrequencyNumeric.TabIndex = 6;
            this.downconverterCenterFrequencyNumeric.Value = new decimal(new int[] {
            1010000000,
            0,
            0,
            0});
            // 
            // spanNumeric
            // 
            this.spanNumeric.DecimalPlaces = 2;
            this.spanNumeric.Location = new System.Drawing.Point(12, 193);
            this.spanNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.spanNumeric.Name = "spanNumeric";
            this.spanNumeric.Size = new System.Drawing.Size(120, 20);
            this.spanNumeric.TabIndex = 4;
            this.spanNumeric.Value = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            // 
            // resolutionBandwidthNumeric
            // 
            this.resolutionBandwidthNumeric.DecimalPlaces = 2;
            this.resolutionBandwidthNumeric.Location = new System.Drawing.Point(12, 234);
            this.resolutionBandwidthNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.resolutionBandwidthNumeric.Name = "resolutionBandwidthNumeric";
            this.resolutionBandwidthNumeric.Size = new System.Drawing.Size(120, 20);
            this.resolutionBandwidthNumeric.TabIndex = 5;
            this.resolutionBandwidthNumeric.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // referenceClockComboBox
            // 
            this.referenceClockComboBox.Location = new System.Drawing.Point(12, 65);
            this.referenceClockComboBox.Name = "referenceClockComboBox";
            this.referenceClockComboBox.Size = new System.Drawing.Size(120, 21);
            this.referenceClockComboBox.TabIndex = 1;
            // 
            // rfsaResourceNameLabel
            // 
            this.rfsaResourceNameLabel.AutoSize = true;
            this.rfsaResourceNameLabel.Location = new System.Drawing.Point(12, 9);
            this.rfsaResourceNameLabel.Name = "rfsaResourceNameLabel";
            this.rfsaResourceNameLabel.Size = new System.Drawing.Size(115, 13);
            this.rfsaResourceNameLabel.TabIndex = 10;
            this.rfsaResourceNameLabel.Text = "RFSA Resource Name";
            // 
            // readPowerSpectrumButton
            // 
            this.readPowerSpectrumButton.Location = new System.Drawing.Point(12, 312);
            this.readPowerSpectrumButton.Name = "readPowerSpectrumButton";
            this.readPowerSpectrumButton.Size = new System.Drawing.Size(134, 26);
            this.readPowerSpectrumButton.TabIndex = 7;
            this.readPowerSpectrumButton.Text = "&Read Power Spectrum";
            this.readPowerSpectrumButton.UseVisualStyleBackColor = true;
            this.readPowerSpectrumButton.Click += new System.EventHandler(this.readPowerSpectrumButton_Click);
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewResults.Location = new System.Drawing.Point(159, 12);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(174, 284);
            this.dataGridViewResults.TabIndex = 16;
            // 
            // rfsaResourceNameComboBox
            // 
            this.rfsaResourceNameComboBox.FormattingEnabled = true;
            this.rfsaResourceNameComboBox.Location = new System.Drawing.Point(12, 24);
            this.rfsaResourceNameComboBox.Name = "rfsaResourceNameComboBox";
            this.rfsaResourceNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.rfsaResourceNameComboBox.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AcceptButton = this.readPowerSpectrumButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 349);
            this.Controls.Add(this.rfsaResourceNameComboBox);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.readPowerSpectrumButton);
            this.Controls.Add(this.rfsaResourceNameLabel);
            this.Controls.Add(this.referenceClockLabel);
            this.Controls.Add(this.referenceLevelLabel);
            this.Controls.Add(this.centerFrequencyLabel);
            this.Controls.Add(this.downconverterCenterFrequencyLabel);
            this.Controls.Add(this.spanLabel);
            this.Controls.Add(this.resolutionBandwidthLabel);
            this.Controls.Add(this.referenceLevelNumeric);
            this.Controls.Add(this.centerFrequencyNumeric);
            this.Controls.Add(this.downconverterCenterFrequencyNumeric);
            this.Controls.Add(this.spanNumeric);
            this.Controls.Add(this.resolutionBandwidthNumeric);
            this.Controls.Add(this.referenceClockComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "RFSA In-band Retuning With Internal LO";
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downconverterCenterFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spanNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resolutionBandwidthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label referenceClockLabel;
        private System.Windows.Forms.Label referenceLevelLabel;
        private System.Windows.Forms.Label centerFrequencyLabel;
        private System.Windows.Forms.Label downconverterCenterFrequencyLabel;
        private System.Windows.Forms.Label spanLabel;
        private System.Windows.Forms.Label resolutionBandwidthLabel;
        private System.Windows.Forms.NumericUpDown referenceLevelNumeric;
        private System.Windows.Forms.NumericUpDown centerFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown downconverterCenterFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown spanNumeric;
        private System.Windows.Forms.NumericUpDown resolutionBandwidthNumeric;
        private System.Windows.Forms.ComboBox referenceClockComboBox;
        private System.Windows.Forms.Label rfsaResourceNameLabel;
        private System.Windows.Forms.Button readPowerSpectrumButton;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.ComboBox rfsaResourceNameComboBox;

    }
}
