namespace NationalInstruments.Examples.AdvancedPropertyAccess
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
            this.carrierFrequencyLabel = new System.Windows.Forms.Label();
            this.samplesPerRecordLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.referenceLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.carrierFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerRecordNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.referenceClockComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.acquireButton = new System.Windows.Forms.Button();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
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
            // carrierFrequencyLabel
            // 
            this.carrierFrequencyLabel.AutoSize = true;
            this.carrierFrequencyLabel.Location = new System.Drawing.Point(9, 173);
            this.carrierFrequencyLabel.Name = "carrierFrequencyLabel";
            this.carrierFrequencyLabel.Size = new System.Drawing.Size(112, 13);
            this.carrierFrequencyLabel.TabIndex = 2;
            this.carrierFrequencyLabel.Text = "Carrier Frequency (Hz)";
            // 
            // samplesPerRecordLabel
            // 
            this.samplesPerRecordLabel.AutoSize = true;
            this.samplesPerRecordLabel.Location = new System.Drawing.Point(9, 223);
            this.samplesPerRecordLabel.Name = "samplesPerRecordLabel";
            this.samplesPerRecordLabel.Size = new System.Drawing.Size(103, 13);
            this.samplesPerRecordLabel.TabIndex = 3;
            this.samplesPerRecordLabel.Text = "Samples per Record";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(9, 273);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(44, 13);
            this.iqRateLabel.TabIndex = 4;
            this.iqRateLabel.Text = "IQ Rate";
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
            // carrierFrequencyNumeric
            // 
            this.carrierFrequencyNumeric.DecimalPlaces = 2;
            this.carrierFrequencyNumeric.Location = new System.Drawing.Point(12, 189);
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
            // samplesPerRecordNumeric
            // 
            this.samplesPerRecordNumeric.Location = new System.Drawing.Point(12, 239);
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
            // iqRateNumeric
            // 
            this.iqRateNumeric.DecimalPlaces = 2;
            this.iqRateNumeric.Location = new System.Drawing.Point(12, 289);
            this.iqRateNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.iqRateNumeric.Name = "iqRateNumeric";
            this.iqRateNumeric.Size = new System.Drawing.Size(116, 20);
            this.iqRateNumeric.TabIndex = 5;
            this.iqRateNumeric.Value = new decimal(new int[] {
            1000000,
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
            this.resourceNameLabel.Location = new System.Drawing.Point(9, 14);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 6;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 30);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(116, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(12, 327);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(116, 23);
            this.acquireButton.TabIndex = 6;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewResults.Location = new System.Drawing.Point(145, 14);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(421, 295);
            this.dataGridViewResults.TabIndex = 9;
            // 
            // MainForm
            // 
            this.AcceptButton = this.acquireButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 358);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.acquireButton);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.referenceClockLabel);
            this.Controls.Add(this.referenceLevelLabel);
            this.Controls.Add(this.carrierFrequencyLabel);
            this.Controls.Add(this.samplesPerRecordLabel);
            this.Controls.Add(this.iqRateLabel);
            this.Controls.Add(this.referenceLevelNumeric);
            this.Controls.Add(this.carrierFrequencyNumeric);
            this.Controls.Add(this.samplesPerRecordNumeric);
            this.Controls.Add(this.iqRateNumeric);
            this.Controls.Add(this.referenceClockComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "RFSA Getting Started IQ using Advanced Property Access Service";
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		
		private System.Windows.Forms.Label referenceClockLabel;
private System.Windows.Forms.Label referenceLevelLabel;
private System.Windows.Forms.Label carrierFrequencyLabel;
private System.Windows.Forms.Label samplesPerRecordLabel;
private System.Windows.Forms.Label iqRateLabel;
private System.Windows.Forms.NumericUpDown referenceLevelNumeric;
private System.Windows.Forms.NumericUpDown carrierFrequencyNumeric;
private System.Windows.Forms.NumericUpDown samplesPerRecordNumeric;
private System.Windows.Forms.NumericUpDown iqRateNumeric;
private System.Windows.Forms.ComboBox referenceClockComboBox;
private System.Windows.Forms.Label resourceNameLabel;
private System.Windows.Forms.ComboBox resourceNameComboBox;
private System.Windows.Forms.Button acquireButton;
private System.Windows.Forms.DataGridView dataGridViewResults;
		
	}
}
