namespace NationalInstruments.Examples.AcquireIQinBlocks
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
            this.samplesPerRecordLabel = new System.Windows.Forms.Label();
            this.samplesPerBlockLabel = new System.Windows.Forms.Label();
            this.samplesReadLabel = new System.Windows.Forms.Label();
            this.referenceLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.carrierFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerRecordNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerBlockNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesReadTextBox = new System.Windows.Forms.TextBox();
            this.acquireButton = new System.Windows.Forms.Button();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBlockNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // referenceLevelLabel
            // 
            this.referenceLevelLabel.AutoSize = true;
            this.referenceLevelLabel.Location = new System.Drawing.Point(9, 83);
            this.referenceLevelLabel.Name = "referenceLevelLabel";
            this.referenceLevelLabel.Size = new System.Drawing.Size(116, 13);
            this.referenceLevelLabel.TabIndex = 0;
            this.referenceLevelLabel.Text = "Reference Level (dBm)";
            // 
            // carrierFrequencyLabel
            // 
            this.carrierFrequencyLabel.AutoSize = true;
            this.carrierFrequencyLabel.Location = new System.Drawing.Point(9, 146);
            this.carrierFrequencyLabel.Name = "carrierFrequencyLabel";
            this.carrierFrequencyLabel.Size = new System.Drawing.Size(112, 13);
            this.carrierFrequencyLabel.TabIndex = 1;
            this.carrierFrequencyLabel.Text = "Carrier Frequency (Hz)";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(9, 209);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 2;
            this.iqRateLabel.Text = "IQ Rate (S/s)";
            // 
            // samplesPerRecordLabel
            // 
            this.samplesPerRecordLabel.AutoSize = true;
            this.samplesPerRecordLabel.Location = new System.Drawing.Point(9, 272);
            this.samplesPerRecordLabel.Name = "samplesPerRecordLabel";
            this.samplesPerRecordLabel.Size = new System.Drawing.Size(103, 13);
            this.samplesPerRecordLabel.TabIndex = 3;
            this.samplesPerRecordLabel.Text = "Samples per Record";
            // 
            // samplesPerBlockLabel
            // 
            this.samplesPerBlockLabel.AutoSize = true;
            this.samplesPerBlockLabel.Location = new System.Drawing.Point(9, 333);
            this.samplesPerBlockLabel.Name = "samplesPerBlockLabel";
            this.samplesPerBlockLabel.Size = new System.Drawing.Size(142, 13);
            this.samplesPerBlockLabel.TabIndex = 4;
            this.samplesPerBlockLabel.Text = "Maximum Samples per Block";
            // 
            // samplesReadLabel
            // 
            this.samplesReadLabel.AutoSize = true;
            this.samplesReadLabel.Location = new System.Drawing.Point(9, 396);
            this.samplesReadLabel.Name = "samplesReadLabel";
            this.samplesReadLabel.Size = new System.Drawing.Size(76, 13);
            this.samplesReadLabel.TabIndex = 5;
            this.samplesReadLabel.Text = "Samples Read";
            // 
            // referenceLevelNumeric
            // 
            this.referenceLevelNumeric.DecimalPlaces = 2;
            this.referenceLevelNumeric.Location = new System.Drawing.Point(12, 99);
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
            this.carrierFrequencyNumeric.Location = new System.Drawing.Point(12, 162);
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
            this.iqRateNumeric.Location = new System.Drawing.Point(12, 225);
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
            this.samplesPerRecordNumeric.Location = new System.Drawing.Point(12, 288);
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
            1000000,
            0,
            0,
            0});
            // 
            // samplesPerBlockNumeric
            // 
            this.samplesPerBlockNumeric.Location = new System.Drawing.Point(12, 349);
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
            this.samplesPerBlockNumeric.TabIndex = 5;
            this.samplesPerBlockNumeric.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // samplesReadTextBox
            // 
            this.samplesReadTextBox.Enabled = false;
            this.samplesReadTextBox.Location = new System.Drawing.Point(12, 412);
            this.samplesReadTextBox.Name = "samplesReadTextBox";
            this.samplesReadTextBox.Size = new System.Drawing.Size(116, 20);
            this.samplesReadTextBox.TabIndex = 11;
            this.samplesReadTextBox.Text = "0";
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(12, 445);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 6;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(9, 23);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 14;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // resourceNameComboBox
            //
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 40);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(116, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(175, 23);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(448, 409);
            this.dataGridView1.TabIndex = 9;
            // 
            // MainForm
            // 
            this.AcceptButton = this.acquireButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.acquireButton);
            this.Controls.Add(this.referenceLevelLabel);
            this.Controls.Add(this.carrierFrequencyLabel);
            this.Controls.Add(this.iqRateLabel);
            this.Controls.Add(this.samplesPerRecordLabel);
            this.Controls.Add(this.samplesPerBlockLabel);
            this.Controls.Add(this.samplesReadLabel);
            this.Controls.Add(this.referenceLevelNumeric);
            this.Controls.Add(this.carrierFrequencyNumeric);
            this.Controls.Add(this.iqRateNumeric);
            this.Controls.Add(this.samplesPerRecordNumeric);
            this.Controls.Add(this.samplesPerBlockNumeric);
            this.Controls.Add(this.samplesReadTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "RFSA Acquire IQ in Blocks";
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerBlockNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label referenceLevelLabel;
        private System.Windows.Forms.Label carrierFrequencyLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label samplesPerRecordLabel;
        private System.Windows.Forms.Label samplesPerBlockLabel;
        private System.Windows.Forms.Label samplesReadLabel;
        private System.Windows.Forms.NumericUpDown referenceLevelNumeric;
        private System.Windows.Forms.NumericUpDown carrierFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.NumericUpDown samplesPerRecordNumeric;
        private System.Windows.Forms.NumericUpDown samplesPerBlockNumeric;
        private System.Windows.Forms.TextBox samplesReadTextBox;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.DataGridView dataGridView1;

    }
}
