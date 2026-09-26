namespace NationalInstruments.Examples.GettingStartedSpectrum
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
            this.startFrequencyLabel = new System.Windows.Forms.Label();
            this.stopFrequencyLabel = new System.Windows.Forms.Label();
            this.resolutionBandwidthLabel = new System.Windows.Forms.Label();
            this.textmsgLabel = new System.Windows.Forms.Label();
            this.referenceLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.startFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.stopFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.resolutionBandwidthNumeric = new System.Windows.Forms.NumericUpDown();
            this.referenceClockComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.readPowerSpectrumButton = new System.Windows.Forms.Button();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resolutionBandwidthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.SuspendLayout();
            // 
            // referenceClockLabel
            // 
            this.referenceClockLabel.AutoSize = true;
            this.referenceClockLabel.Location = new System.Drawing.Point(9, 71);
            this.referenceClockLabel.Name = "referenceClockLabel";
            this.referenceClockLabel.Size = new System.Drawing.Size(87, 13);
            this.referenceClockLabel.TabIndex = 0;
            this.referenceClockLabel.Text = "Reference Clock";
            // 
            // referenceLevelLabel
            // 
            this.referenceLevelLabel.AutoSize = true;
            this.referenceLevelLabel.Location = new System.Drawing.Point(9, 123);
            this.referenceLevelLabel.Name = "referenceLevelLabel";
            this.referenceLevelLabel.Size = new System.Drawing.Size(116, 13);
            this.referenceLevelLabel.TabIndex = 1;
            this.referenceLevelLabel.Text = "Reference Level (dBm)";
            // 
            // startFrequencyLabel
            // 
            this.startFrequencyLabel.AutoSize = true;
            this.startFrequencyLabel.Location = new System.Drawing.Point(9, 173);
            this.startFrequencyLabel.Name = "startFrequencyLabel";
            this.startFrequencyLabel.Size = new System.Drawing.Size(104, 13);
            this.startFrequencyLabel.TabIndex = 2;
            this.startFrequencyLabel.Text = "Start Frequency (Hz)";
            // 
            // stopFrequencyLabel
            // 
            this.stopFrequencyLabel.AutoSize = true;
            this.stopFrequencyLabel.Location = new System.Drawing.Point(9, 223);
            this.stopFrequencyLabel.Name = "stopFrequencyLabel";
            this.stopFrequencyLabel.Size = new System.Drawing.Size(104, 13);
            this.stopFrequencyLabel.TabIndex = 3;
            this.stopFrequencyLabel.Text = "Stop Frequency (Hz)";
            // 
            // resolutionBandwidthLabel
            // 
            this.resolutionBandwidthLabel.AutoSize = true;
            this.resolutionBandwidthLabel.Location = new System.Drawing.Point(9, 273);
            this.resolutionBandwidthLabel.Name = "resolutionBandwidthLabel";
            this.resolutionBandwidthLabel.Size = new System.Drawing.Size(132, 13);
            this.resolutionBandwidthLabel.TabIndex = 4;
            this.resolutionBandwidthLabel.Text = "Resolution Bandwidth (Hz)";
            // 
            // textmsgLabel
            // 
            this.textmsgLabel.Location = new System.Drawing.Point(0, 0);
            this.textmsgLabel.Name = "textmsgLabel";
            this.textmsgLabel.Size = new System.Drawing.Size(100, 23);
            this.textmsgLabel.TabIndex = 5;
            // 
            // referenceLevelNumeric
            // 
            this.referenceLevelNumeric.DecimalPlaces = 2;
            this.referenceLevelNumeric.Location = new System.Drawing.Point(12, 139);
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
            // startFrequencyNumeric
            // 
            this.startFrequencyNumeric.DecimalPlaces = 2;
            this.startFrequencyNumeric.Location = new System.Drawing.Point(12, 189);
            this.startFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.startFrequencyNumeric.Name = "startFrequencyNumeric";
            this.startFrequencyNumeric.Size = new System.Drawing.Size(116, 20);
            this.startFrequencyNumeric.TabIndex = 3;
            this.startFrequencyNumeric.Value = new decimal(new int[] {
            990000000,
            0,
            0,
            0});
            // 
            // stopFrequencyNumeric
            // 
            this.stopFrequencyNumeric.DecimalPlaces = 2;
            this.stopFrequencyNumeric.Location = new System.Drawing.Point(12, 239);
            this.stopFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.stopFrequencyNumeric.Name = "stopFrequencyNumeric";
            this.stopFrequencyNumeric.Size = new System.Drawing.Size(116, 20);
            this.stopFrequencyNumeric.TabIndex = 4;
            this.stopFrequencyNumeric.Value = new decimal(new int[] {
            1010000000,
            0,
            0,
            0});
            // 
            // resolutionBandwidthNumeric
            // 
            this.resolutionBandwidthNumeric.DecimalPlaces = 2;
            this.resolutionBandwidthNumeric.Location = new System.Drawing.Point(12, 289);
            this.resolutionBandwidthNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.resolutionBandwidthNumeric.Name = "resolutionBandwidthNumeric";
            this.resolutionBandwidthNumeric.Size = new System.Drawing.Size(116, 20);
            this.resolutionBandwidthNumeric.TabIndex = 5;
            this.resolutionBandwidthNumeric.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // referenceClockComboBox
            // 
            this.referenceClockComboBox.Location = new System.Drawing.Point(12, 87);
            this.referenceClockComboBox.Name = "referenceClockComboBox";
            this.referenceClockComboBox.Size = new System.Drawing.Size(116, 21);
            this.referenceClockComboBox.TabIndex = 1;
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(9, 16);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 6;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewResults.Location = new System.Drawing.Point(147, 16);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(174, 293);
            this.dataGridViewResults.TabIndex = 9;
            // 
            // readPowerSpectrumButton
            // 
            this.readPowerSpectrumButton.Location = new System.Drawing.Point(12, 326);
            this.readPowerSpectrumButton.Name = "readPowerSpectrumButton";
            this.readPowerSpectrumButton.Size = new System.Drawing.Size(129, 23);
            this.readPowerSpectrumButton.TabIndex = 6;
            this.readPowerSpectrumButton.Text = "&Read Power Spectrum";
            this.readPowerSpectrumButton.UseVisualStyleBackColor = true;
            this.readPowerSpectrumButton.Click += new System.EventHandler(this.readPowerSpectrumButton_Click);
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 32);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AcceptButton = this.readPowerSpectrumButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 355);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.readPowerSpectrumButton);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.referenceClockLabel);
            this.Controls.Add(this.referenceLevelLabel);
            this.Controls.Add(this.startFrequencyLabel);
            this.Controls.Add(this.stopFrequencyLabel);
            this.Controls.Add(this.resolutionBandwidthLabel);
            this.Controls.Add(this.textmsgLabel);
            this.Controls.Add(this.referenceLevelNumeric);
            this.Controls.Add(this.startFrequencyNumeric);
            this.Controls.Add(this.stopFrequencyNumeric);
            this.Controls.Add(this.resolutionBandwidthNumeric);
            this.Controls.Add(this.referenceClockComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "RFSA Getting Started Spectrum";
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resolutionBandwidthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		
		private System.Windows.Forms.Label referenceClockLabel;
private System.Windows.Forms.Label referenceLevelLabel;
private System.Windows.Forms.Label startFrequencyLabel;
private System.Windows.Forms.Label stopFrequencyLabel;
private System.Windows.Forms.Label resolutionBandwidthLabel;
private System.Windows.Forms.Label textmsgLabel;
private System.Windows.Forms.NumericUpDown referenceLevelNumeric;
private System.Windows.Forms.NumericUpDown startFrequencyNumeric;
private System.Windows.Forms.NumericUpDown stopFrequencyNumeric;
private System.Windows.Forms.NumericUpDown resolutionBandwidthNumeric;
private System.Windows.Forms.ComboBox referenceClockComboBox;
private System.Windows.Forms.Label resourceNameLabel;
private System.Windows.Forms.DataGridView dataGridViewResults;
private System.Windows.Forms.Button readPowerSpectrumButton;
private System.Windows.Forms.ComboBox resourceNameComboBox;
		
	}
}
