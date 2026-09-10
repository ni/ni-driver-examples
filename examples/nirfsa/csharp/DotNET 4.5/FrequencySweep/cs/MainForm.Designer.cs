namespace NationalInstruments.Examples.FrequencySweep
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
            this.startFrequencyLabel = new System.Windows.Forms.Label();
            this.stopFrequencyLabel = new System.Windows.Forms.Label();
            this.resolutionBandwidthLabel = new System.Windows.Forms.Label();
            this.numStepsLabel = new System.Windows.Forms.Label();
            this.currentCenterFrequencyLabel = new System.Windows.Forms.Label();
            this.referenceLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.startFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.stopFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.resolutionBandwidthNumeric = new System.Windows.Forms.NumericUpDown();
            this.numStepsNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentCenterFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.startButton = new System.Windows.Forms.Button();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resolutionBandwidthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStepsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.SuspendLayout();
            // 
            // referenceLevelLabel
            // 
            this.referenceLevelLabel.AutoSize = true;
            this.referenceLevelLabel.Location = new System.Drawing.Point(9, 71);
            this.referenceLevelLabel.Name = "referenceLevelLabel";
            this.referenceLevelLabel.Size = new System.Drawing.Size(116, 13);
            this.referenceLevelLabel.TabIndex = 0;
            this.referenceLevelLabel.Text = "Reference Level (dBm)";
            // 
            // startFrequencyLabel
            // 
            this.startFrequencyLabel.AutoSize = true;
            this.startFrequencyLabel.Location = new System.Drawing.Point(9, 121);
            this.startFrequencyLabel.Name = "startFrequencyLabel";
            this.startFrequencyLabel.Size = new System.Drawing.Size(138, 13);
            this.startFrequencyLabel.TabIndex = 1;
            this.startFrequencyLabel.Text = "Start Center Frequency (Hz)";
            // 
            // stopFrequencyLabel
            // 
            this.stopFrequencyLabel.AutoSize = true;
            this.stopFrequencyLabel.Location = new System.Drawing.Point(9, 171);
            this.stopFrequencyLabel.Name = "stopFrequencyLabel";
            this.stopFrequencyLabel.Size = new System.Drawing.Size(138, 13);
            this.stopFrequencyLabel.TabIndex = 2;
            this.stopFrequencyLabel.Text = "Stop Center Frequency (Hz)";
            // 
            // resolutionBandwidthLabel
            // 
            this.resolutionBandwidthLabel.AutoSize = true;
            this.resolutionBandwidthLabel.Location = new System.Drawing.Point(9, 221);
            this.resolutionBandwidthLabel.Name = "resolutionBandwidthLabel";
            this.resolutionBandwidthLabel.Size = new System.Drawing.Size(132, 13);
            this.resolutionBandwidthLabel.TabIndex = 3;
            this.resolutionBandwidthLabel.Text = "Resolution Bandwidth (Hz)";
            // 
            // numStepsLabel
            // 
            this.numStepsLabel.AutoSize = true;
            this.numStepsLabel.Location = new System.Drawing.Point(9, 271);
            this.numStepsLabel.Name = "numStepsLabel";
            this.numStepsLabel.Size = new System.Drawing.Size(34, 13);
            this.numStepsLabel.TabIndex = 4;
            this.numStepsLabel.Text = "Steps";
            // 
            // currentCenterFrequencyLabel
            // 
            this.currentCenterFrequencyLabel.AutoSize = true;
            this.currentCenterFrequencyLabel.Location = new System.Drawing.Point(249, 303);
            this.currentCenterFrequencyLabel.Name = "currentCenterFrequencyLabel";
            this.currentCenterFrequencyLabel.Size = new System.Drawing.Size(128, 13);
            this.currentCenterFrequencyLabel.TabIndex = 5;
            this.currentCenterFrequencyLabel.Text = "Current Center Frequency";
            // 
            // referenceLevelNumeric
            // 
            this.referenceLevelNumeric.DecimalPlaces = 2;
            this.referenceLevelNumeric.Location = new System.Drawing.Point(12, 87);
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
            // startFrequencyNumeric
            // 
            this.startFrequencyNumeric.DecimalPlaces = 2;
            this.startFrequencyNumeric.Location = new System.Drawing.Point(12, 137);
            this.startFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.startFrequencyNumeric.Name = "startFrequencyNumeric";
            this.startFrequencyNumeric.Size = new System.Drawing.Size(116, 20);
            this.startFrequencyNumeric.TabIndex = 2;
            this.startFrequencyNumeric.Value = new decimal(new int[] {
            990000000,
            0,
            0,
            0});
            // 
            // stopFrequencyNumeric
            // 
            this.stopFrequencyNumeric.DecimalPlaces = 2;
            this.stopFrequencyNumeric.Location = new System.Drawing.Point(12, 187);
            this.stopFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.stopFrequencyNumeric.Name = "stopFrequencyNumeric";
            this.stopFrequencyNumeric.Size = new System.Drawing.Size(116, 20);
            this.stopFrequencyNumeric.TabIndex = 3;
            this.stopFrequencyNumeric.Value = new decimal(new int[] {
            1010000000,
            0,
            0,
            0});
            // 
            // resolutionBandwidthNumeric
            // 
            this.resolutionBandwidthNumeric.DecimalPlaces = 2;
            this.resolutionBandwidthNumeric.Location = new System.Drawing.Point(12, 237);
            this.resolutionBandwidthNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.resolutionBandwidthNumeric.Name = "resolutionBandwidthNumeric";
            this.resolutionBandwidthNumeric.Size = new System.Drawing.Size(116, 20);
            this.resolutionBandwidthNumeric.TabIndex = 4;
            this.resolutionBandwidthNumeric.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // numStepsNumeric
            // 
            this.numStepsNumeric.Location = new System.Drawing.Point(12, 287);
            this.numStepsNumeric.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.numStepsNumeric.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numStepsNumeric.Name = "numStepsNumeric";
            this.numStepsNumeric.Size = new System.Drawing.Size(116, 20);
            this.numStepsNumeric.TabIndex = 5;
            this.numStepsNumeric.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // currentCenterFrequencyTextBox
            // 
            this.currentCenterFrequencyTextBox.Location = new System.Drawing.Point(249, 324);
            this.currentCenterFrequencyTextBox.Name = "currentCenterFrequencyTextBox";
            this.currentCenterFrequencyTextBox.ReadOnly = true;
            this.currentCenterFrequencyTextBox.Size = new System.Drawing.Size(128, 20);
            this.currentCenterFrequencyTextBox.TabIndex = 10;
            this.currentCenterFrequencyTextBox.Text = "0.00000000000000E+0";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(12, 16);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 11;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 32);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(116, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(12, 320);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewResults.Location = new System.Drawing.Point(153, 16);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(359, 268);
            this.dataGridViewResults.TabIndex = 9;
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 354);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.referenceLevelLabel);
            this.Controls.Add(this.startFrequencyLabel);
            this.Controls.Add(this.stopFrequencyLabel);
            this.Controls.Add(this.resolutionBandwidthLabel);
            this.Controls.Add(this.numStepsLabel);
            this.Controls.Add(this.currentCenterFrequencyLabel);
            this.Controls.Add(this.referenceLevelNumeric);
            this.Controls.Add(this.startFrequencyNumeric);
            this.Controls.Add(this.stopFrequencyNumeric);
            this.Controls.Add(this.resolutionBandwidthNumeric);
            this.Controls.Add(this.numStepsNumeric);
            this.Controls.Add(this.currentCenterFrequencyTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "RFSA Frequency Sweep";
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resolutionBandwidthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStepsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label referenceLevelLabel;
        private System.Windows.Forms.Label startFrequencyLabel;
        private System.Windows.Forms.Label stopFrequencyLabel;
        private System.Windows.Forms.Label resolutionBandwidthLabel;
        private System.Windows.Forms.Label numStepsLabel;
        private System.Windows.Forms.Label currentCenterFrequencyLabel;
        private System.Windows.Forms.NumericUpDown referenceLevelNumeric;
        private System.Windows.Forms.NumericUpDown startFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown stopFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown resolutionBandwidthNumeric;
        private System.Windows.Forms.NumericUpDown numStepsNumeric;
        private System.Windows.Forms.TextBox currentCenterFrequencyTextBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.DataGridView dataGridViewResults;

    }
}