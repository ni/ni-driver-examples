namespace NationalInstruments.Examples.StreamBinaryIQDataToDisk
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
            this.referenceLevelLabel = new System.Windows.Forms.Label();
            this.carrierFrequencyLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.samplesPerBlockLabel = new System.Windows.Forms.Label();
            this.maxSamplesLabel = new System.Windows.Forms.Label();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.dtLabel = new System.Windows.Forms.Label();
            this.gainLabel = new System.Windows.Forms.Label();
            this.offsetLabel = new System.Windows.Forms.Label();
            this.samplesSoFarLabel = new System.Windows.Forms.Label();
            this.referenceLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.carrierFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerBlockNumeric = new System.Windows.Forms.NumericUpDown();
            this.maxSamplesNumeric = new System.Windows.Forms.NumericUpDown();
            this.fileNameTextBox = new System.Windows.Forms.TextBox();
            this.dtTextBox = new System.Windows.Forms.TextBox();
            this.gainTextBox = new System.Windows.Forms.TextBox();
            this.offsetTextBox = new System.Windows.Forms.TextBox();
            this.samplesSoFarTextBox = new System.Windows.Forms.TextBox();
            this.rfsaResourceNameLabel = new System.Windows.Forms.Label();
            this.streamToDiskButton = new System.Windows.Forms.Button();
            this.readFromDiskButton = new System.Windows.Forms.Button();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBlockNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSamplesNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.SuspendLayout();
            // 
            // referenceLevelLabel
            // 
            this.referenceLevelLabel.AutoSize = true;
            this.referenceLevelLabel.Location = new System.Drawing.Point(12, 58);
            this.referenceLevelLabel.Name = "referenceLevelLabel";
            this.referenceLevelLabel.Size = new System.Drawing.Size(116, 13);
            this.referenceLevelLabel.TabIndex = 0;
            this.referenceLevelLabel.Text = "Reference Level (dBm)";
            // 
            // carrierFrequencyLabel
            // 
            this.carrierFrequencyLabel.AutoSize = true;
            this.carrierFrequencyLabel.Location = new System.Drawing.Point(12, 107);
            this.carrierFrequencyLabel.Name = "carrierFrequencyLabel";
            this.carrierFrequencyLabel.Size = new System.Drawing.Size(112, 13);
            this.carrierFrequencyLabel.TabIndex = 1;
            this.carrierFrequencyLabel.Text = "Carrier Frequency (Hz)";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(12, 156);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(44, 13);
            this.iqRateLabel.TabIndex = 2;
            this.iqRateLabel.Text = "IQ Rate";
            // 
            // samplesPerBlockLabel
            // 
            this.samplesPerBlockLabel.AutoSize = true;
            this.samplesPerBlockLabel.Location = new System.Drawing.Point(12, 206);
            this.samplesPerBlockLabel.Name = "samplesPerBlockLabel";
            this.samplesPerBlockLabel.Size = new System.Drawing.Size(142, 13);
            this.samplesPerBlockLabel.TabIndex = 3;
            this.samplesPerBlockLabel.Text = "Maximum Samples per Block";
            // 
            // maxSamplesLabel
            // 
            this.maxSamplesLabel.AutoSize = true;
            this.maxSamplesLabel.Location = new System.Drawing.Point(12, 256);
            this.maxSamplesLabel.Name = "maxSamplesLabel";
            this.maxSamplesLabel.Size = new System.Drawing.Size(114, 13);
            this.maxSamplesLabel.TabIndex = 4;
            this.maxSamplesLabel.Text = "Total Samples to Write";
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.AutoSize = true;
            this.fileNameLabel.Location = new System.Drawing.Point(12, 304);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(49, 13);
            this.fileNameLabel.TabIndex = 5;
            this.fileNameLabel.Text = "Filename";
            // 
            // dtLabel
            // 
            this.dtLabel.AutoSize = true;
            this.dtLabel.Location = new System.Drawing.Point(157, 361);
            this.dtLabel.Name = "dtLabel";
            this.dtLabel.Size = new System.Drawing.Size(16, 13);
            this.dtLabel.TabIndex = 6;
            this.dtLabel.Text = "dt";
            // 
            // gainLabel
            // 
            this.gainLabel.AutoSize = true;
            this.gainLabel.Location = new System.Drawing.Point(279, 361);
            this.gainLabel.Name = "gainLabel";
            this.gainLabel.Size = new System.Drawing.Size(29, 13);
            this.gainLabel.TabIndex = 7;
            this.gainLabel.Text = "Gain";
            // 
            // offsetLabel
            // 
            this.offsetLabel.AutoSize = true;
            this.offsetLabel.Location = new System.Drawing.Point(401, 361);
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(35, 13);
            this.offsetLabel.TabIndex = 8;
            this.offsetLabel.Text = "Offset";
            // 
            // samplesSoFarLabel
            // 
            this.samplesSoFarLabel.AutoSize = true;
            this.samplesSoFarLabel.Location = new System.Drawing.Point(157, 407);
            this.samplesSoFarLabel.Name = "samplesSoFarLabel";
            this.samplesSoFarLabel.Size = new System.Drawing.Size(149, 13);
            this.samplesSoFarLabel.TabIndex = 9;
            this.samplesSoFarLabel.Text = "Samples Read/Written So Far";
            // 
            // referenceLevelNumeric
            // 
            this.referenceLevelNumeric.DecimalPlaces = 2;
            this.referenceLevelNumeric.Location = new System.Drawing.Point(12, 77);
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
            this.carrierFrequencyNumeric.Location = new System.Drawing.Point(12, 127);
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
            this.iqRateNumeric.Location = new System.Drawing.Point(12, 175);
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
            // samplesPerBlockNumeric
            // 
            this.samplesPerBlockNumeric.Location = new System.Drawing.Point(12, 225);
            this.samplesPerBlockNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.samplesPerBlockNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.samplesPerBlockNumeric.Name = "samplesPerBlockNumeric";
            this.samplesPerBlockNumeric.Size = new System.Drawing.Size(116, 20);
            this.samplesPerBlockNumeric.TabIndex = 4;
            this.samplesPerBlockNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // maxSamplesNumeric
            // 
            this.maxSamplesNumeric.Location = new System.Drawing.Point(12, 276);
            this.maxSamplesNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.maxSamplesNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxSamplesNumeric.Name = "maxSamplesNumeric";
            this.maxSamplesNumeric.Size = new System.Drawing.Size(116, 20);
            this.maxSamplesNumeric.TabIndex = 5;
            this.maxSamplesNumeric.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Location = new System.Drawing.Point(12, 322);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Size = new System.Drawing.Size(117, 20);
            this.fileNameTextBox.TabIndex = 6;
            this.fileNameTextBox.Text = "BinaryFile.bin";
            // 
            // dtTextBox
            // 
            this.dtTextBox.Location = new System.Drawing.Point(160, 377);
            this.dtTextBox.Name = "dtTextBox";
            this.dtTextBox.ReadOnly = true;
            this.dtTextBox.Size = new System.Drawing.Size(116, 20);
            this.dtTextBox.TabIndex = 12;
            this.dtTextBox.Text = "0.00000000000000E+0";
            // 
            // gainTextBox
            // 
            this.gainTextBox.Location = new System.Drawing.Point(282, 377);
            this.gainTextBox.Name = "gainTextBox";
            this.gainTextBox.ReadOnly = true;
            this.gainTextBox.Size = new System.Drawing.Size(116, 20);
            this.gainTextBox.TabIndex = 13;
            this.gainTextBox.Text = "0.00000000000000E+0";
            // 
            // offsetTextBox
            // 
            this.offsetTextBox.Location = new System.Drawing.Point(404, 377);
            this.offsetTextBox.Name = "offsetTextBox";
            this.offsetTextBox.ReadOnly = true;
            this.offsetTextBox.Size = new System.Drawing.Size(116, 20);
            this.offsetTextBox.TabIndex = 14;
            this.offsetTextBox.Text = "0.00000000000000E+0";
            // 
            // samplesSoFarTextBox
            // 
            this.samplesSoFarTextBox.Location = new System.Drawing.Point(312, 403);
            this.samplesSoFarTextBox.Name = "samplesSoFarTextBox";
            this.samplesSoFarTextBox.ReadOnly = true;
            this.samplesSoFarTextBox.Size = new System.Drawing.Size(116, 20);
            this.samplesSoFarTextBox.TabIndex = 15;
            this.samplesSoFarTextBox.Text = "0";
            // 
            // rfsaResourceNameLabel
            // 
            this.rfsaResourceNameLabel.AutoSize = true;
            this.rfsaResourceNameLabel.Location = new System.Drawing.Point(12, 15);
            this.rfsaResourceNameLabel.Name = "rfsaResourceNameLabel";
            this.rfsaResourceNameLabel.Size = new System.Drawing.Size(115, 13);
            this.rfsaResourceNameLabel.TabIndex = 17;
            this.rfsaResourceNameLabel.Text = "RFSA Resource Name";
            // 
            // streamToDiskButton
            // 
            this.streamToDiskButton.Location = new System.Drawing.Point(12, 361);
            this.streamToDiskButton.Name = "streamToDiskButton";
            this.streamToDiskButton.Size = new System.Drawing.Size(114, 23);
            this.streamToDiskButton.TabIndex = 7;
            this.streamToDiskButton.Text = "&Stream To Disk";
            this.streamToDiskButton.UseVisualStyleBackColor = true;
            this.streamToDiskButton.Click += new System.EventHandler(this.streamToDiskButton_Click);
            // 
            // readFromDiskButton
            // 
            this.readFromDiskButton.Location = new System.Drawing.Point(12, 402);
            this.readFromDiskButton.Name = "readFromDiskButton";
            this.readFromDiskButton.Size = new System.Drawing.Size(114, 23);
            this.readFromDiskButton.TabIndex = 8;
            this.readFromDiskButton.Text = "&Read From Disk";
            this.readFromDiskButton.UseVisualStyleBackColor = true;
            this.readFromDiskButton.Click += new System.EventHandler(this.readFromDiskButton_Click);
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewResults.Location = new System.Drawing.Point(160, 15);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(360, 327);
            this.dataGridViewResults.TabIndex = 11;
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 31);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(116, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 433);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.readFromDiskButton);
            this.Controls.Add(this.streamToDiskButton);
            this.Controls.Add(this.rfsaResourceNameLabel);
            this.Controls.Add(this.referenceLevelLabel);
            this.Controls.Add(this.carrierFrequencyLabel);
            this.Controls.Add(this.iqRateLabel);
            this.Controls.Add(this.samplesPerBlockLabel);
            this.Controls.Add(this.maxSamplesLabel);
            this.Controls.Add(this.fileNameLabel);
            this.Controls.Add(this.dtLabel);
            this.Controls.Add(this.gainLabel);
            this.Controls.Add(this.offsetLabel);
            this.Controls.Add(this.samplesSoFarLabel);
            this.Controls.Add(this.referenceLevelNumeric);
            this.Controls.Add(this.carrierFrequencyNumeric);
            this.Controls.Add(this.iqRateNumeric);
            this.Controls.Add(this.samplesPerBlockNumeric);
            this.Controls.Add(this.maxSamplesNumeric);
            this.Controls.Add(this.fileNameTextBox);
            this.Controls.Add(this.dtTextBox);
            this.Controls.Add(this.gainTextBox);
            this.Controls.Add(this.offsetTextBox);
            this.Controls.Add(this.samplesSoFarTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "RFSA Stream Binary IQ Data to Disk";
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBlockNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSamplesNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		
		private System.Windows.Forms.Label referenceLevelLabel;
private System.Windows.Forms.Label carrierFrequencyLabel;
private System.Windows.Forms.Label iqRateLabel;
private System.Windows.Forms.Label samplesPerBlockLabel;
private System.Windows.Forms.Label maxSamplesLabel;
private System.Windows.Forms.Label fileNameLabel;
private System.Windows.Forms.Label dtLabel;
private System.Windows.Forms.Label gainLabel;
private System.Windows.Forms.Label offsetLabel;
private System.Windows.Forms.Label samplesSoFarLabel;
private System.Windows.Forms.NumericUpDown referenceLevelNumeric;
private System.Windows.Forms.NumericUpDown carrierFrequencyNumeric;
private System.Windows.Forms.NumericUpDown iqRateNumeric;
private System.Windows.Forms.NumericUpDown samplesPerBlockNumeric;
private System.Windows.Forms.NumericUpDown maxSamplesNumeric;
private System.Windows.Forms.TextBox fileNameTextBox;
private System.Windows.Forms.TextBox dtTextBox;
private System.Windows.Forms.TextBox gainTextBox;
private System.Windows.Forms.TextBox offsetTextBox;
private System.Windows.Forms.TextBox samplesSoFarTextBox;
private System.Windows.Forms.Label rfsaResourceNameLabel;
private System.Windows.Forms.Button streamToDiskButton;
private System.Windows.Forms.Button readFromDiskButton;
private System.Windows.Forms.DataGridView dataGridViewResults;
private System.Windows.Forms.ComboBox resourceNameComboBox;
		
	}
}
