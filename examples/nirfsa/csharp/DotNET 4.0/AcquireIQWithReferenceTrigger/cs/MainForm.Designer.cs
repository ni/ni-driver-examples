namespace NationalInstruments.Examples.AcquireIQWithReferenceTrigger
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
            this.triggerTypeLabel = new System.Windows.Forms.Label();
            this.pretriggerSamplesLabel = new System.Windows.Forms.Label();
            this.triggerLevelLabel = new System.Windows.Forms.Label();
            this.referenceLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.carrierFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerRecordNumeric = new System.Windows.Forms.NumericUpDown();
            this.pretriggerSamplesNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggerTypeComboBox = new System.Windows.Forms.ComboBox();
            this.triggerSourceComboBox = new System.Windows.Forms.ComboBox();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.startAcquisitionButton = new System.Windows.Forms.Button();
            this.sendSoftwareTriggerbutton = new System.Windows.Forms.Button();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pretriggerSamplesNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.SuspendLayout();
            // 
            // referenceLevelLabel
            // 
            this.referenceLevelLabel.AutoSize = true;
            this.referenceLevelLabel.Location = new System.Drawing.Point(12, 57);
            this.referenceLevelLabel.Name = "referenceLevelLabel";
            this.referenceLevelLabel.Size = new System.Drawing.Size(116, 13);
            this.referenceLevelLabel.TabIndex = 0;
            this.referenceLevelLabel.Text = "Reference Level (dBm)";
            // 
            // carrierFrequencyLabel
            // 
            this.carrierFrequencyLabel.AutoSize = true;
            this.carrierFrequencyLabel.Location = new System.Drawing.Point(12, 102);
            this.carrierFrequencyLabel.Name = "carrierFrequencyLabel";
            this.carrierFrequencyLabel.Size = new System.Drawing.Size(112, 13);
            this.carrierFrequencyLabel.TabIndex = 1;
            this.carrierFrequencyLabel.Text = "Carrier Frequency (Hz)";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(12, 148);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(44, 13);
            this.iqRateLabel.TabIndex = 2;
            this.iqRateLabel.Text = "IQ Rate";
            // 
            // samplesPerRecordLabel
            // 
            this.samplesPerRecordLabel.AutoSize = true;
            this.samplesPerRecordLabel.Location = new System.Drawing.Point(12, 194);
            this.samplesPerRecordLabel.Name = "samplesPerRecordLabel";
            this.samplesPerRecordLabel.Size = new System.Drawing.Size(103, 13);
            this.samplesPerRecordLabel.TabIndex = 3;
            this.samplesPerRecordLabel.Text = "Samples per Record";
            // 
            // triggerTypeLabel
            // 
            this.triggerTypeLabel.AutoSize = true;
            this.triggerTypeLabel.Location = new System.Drawing.Point(12, 241);
            this.triggerTypeLabel.Name = "triggerTypeLabel";
            this.triggerTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.triggerTypeLabel.TabIndex = 4;
            this.triggerTypeLabel.Text = "Trigger Type";
            // 
            // pretriggerSamplesLabel
            // 
            this.pretriggerSamplesLabel.AutoSize = true;
            this.pretriggerSamplesLabel.Location = new System.Drawing.Point(12, 288);
            this.pretriggerSamplesLabel.Name = "pretriggerSamplesLabel";
            this.pretriggerSamplesLabel.Size = new System.Drawing.Size(95, 13);
            this.pretriggerSamplesLabel.TabIndex = 5;
            this.pretriggerSamplesLabel.Text = "Pretrigger Samples";
            // 
            // triggerLevelLabel
            // 
            this.triggerLevelLabel.AutoSize = true;
            this.triggerLevelLabel.Location = new System.Drawing.Point(12, 377);
            this.triggerLevelLabel.Name = "triggerLevelLabel";
            this.triggerLevelLabel.Size = new System.Drawing.Size(99, 13);
            this.triggerLevelLabel.TabIndex = 7;
            this.triggerLevelLabel.Text = "Trigger Level (dBm)";
            // 
            // referenceLevelNumeric
            // 
            this.referenceLevelNumeric.DecimalPlaces = 2;
            this.referenceLevelNumeric.Location = new System.Drawing.Point(12, 76);
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
            this.carrierFrequencyNumeric.Location = new System.Drawing.Point(12, 122);
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
            this.iqRateNumeric.Location = new System.Drawing.Point(12, 169);
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
            this.samplesPerRecordNumeric.Location = new System.Drawing.Point(12, 216);
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
            // pretriggerSamplesNumeric
            // 
            this.pretriggerSamplesNumeric.Location = new System.Drawing.Point(12, 308);
            this.pretriggerSamplesNumeric.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.pretriggerSamplesNumeric.Name = "pretriggerSamplesNumeric";
            this.pretriggerSamplesNumeric.Size = new System.Drawing.Size(116, 20);
            this.pretriggerSamplesNumeric.TabIndex = 6;
            // 
            // triggerLevelNumeric
            // 
            this.triggerLevelNumeric.DecimalPlaces = 2;
            this.triggerLevelNumeric.Location = new System.Drawing.Point(12, 393);
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
            // triggerTypeComboBox
            // 
            this.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerTypeComboBox.Location = new System.Drawing.Point(12, 261);
            this.triggerTypeComboBox.Name = "triggerTypeComboBox";
            this.triggerTypeComboBox.Size = new System.Drawing.Size(116, 21);
            this.triggerTypeComboBox.TabIndex = 5;
            this.triggerTypeComboBox.SelectedValueChanged += new System.EventHandler(this.triggerTypeComboBox_SelectedValueChanged);
            // 
            // triggerSourceComboBox
            // 
            this.triggerSourceComboBox.Location = new System.Drawing.Point(12, 353);
            this.triggerSourceComboBox.Name = "triggerSourceComboBox";
            this.triggerSourceComboBox.Size = new System.Drawing.Size(116, 21);
            this.triggerSourceComboBox.TabIndex = 7;
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.AutoSize = true;
            this.triggerSourceLabel.Location = new System.Drawing.Point(12, 335);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(77, 13);
            this.triggerSourceLabel.TabIndex = 6;
            this.triggerSourceLabel.Text = "Trigger Source";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(15, 11);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 9;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 31);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(110, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // startAcquisitionButton
            // 
            this.startAcquisitionButton.Location = new System.Drawing.Point(12, 431);
            this.startAcquisitionButton.Name = "startAcquisitionButton";
            this.startAcquisitionButton.Size = new System.Drawing.Size(116, 30);
            this.startAcquisitionButton.TabIndex = 9;
            this.startAcquisitionButton.Text = "Start &Acquisition";
            this.startAcquisitionButton.UseVisualStyleBackColor = true;
            this.startAcquisitionButton.Click += new System.EventHandler(this.startAcquisitionButton_Click);
            // 
            // sendSoftwareTriggerbutton
            // 
            this.sendSoftwareTriggerbutton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.sendSoftwareTriggerbutton.Location = new System.Drawing.Point(145, 431);
            this.sendSoftwareTriggerbutton.Name = "sendSoftwareTriggerbutton";
            this.sendSoftwareTriggerbutton.Size = new System.Drawing.Size(140, 30);
            this.sendSoftwareTriggerbutton.TabIndex = 10;
            this.sendSoftwareTriggerbutton.Text = "&Send Software Trigger";
            this.sendSoftwareTriggerbutton.UseVisualStyleBackColor = true;
            this.sendSoftwareTriggerbutton.Click += new System.EventHandler(this.sendSoftwareTriggerbutton_Click);
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewResults.Location = new System.Drawing.Point(145, 11);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(472, 402);
            this.dataGridViewResults.TabIndex = 13;
            // 
            // MainForm
            // 
            this.AcceptButton = this.startAcquisitionButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 470);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.sendSoftwareTriggerbutton);
            this.Controls.Add(this.startAcquisitionButton);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.referenceLevelLabel);
            this.Controls.Add(this.carrierFrequencyLabel);
            this.Controls.Add(this.iqRateLabel);
            this.Controls.Add(this.samplesPerRecordLabel);
            this.Controls.Add(this.triggerTypeLabel);
            this.Controls.Add(this.pretriggerSamplesLabel);
            this.Controls.Add(this.triggerSourceLabel);
            this.Controls.Add(this.triggerLevelLabel);
            this.Controls.Add(this.referenceLevelNumeric);
            this.Controls.Add(this.carrierFrequencyNumeric);
            this.Controls.Add(this.iqRateNumeric);
            this.Controls.Add(this.samplesPerRecordNumeric);
            this.Controls.Add(this.pretriggerSamplesNumeric);
            this.Controls.Add(this.triggerLevelNumeric);
            this.Controls.Add(this.triggerTypeComboBox);
            this.Controls.Add(this.triggerSourceComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Acquire IQ with Reference Trigger";
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pretriggerSamplesNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label referenceLevelLabel;
        private System.Windows.Forms.Label carrierFrequencyLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label samplesPerRecordLabel;
        private System.Windows.Forms.Label triggerTypeLabel;
        private System.Windows.Forms.Label pretriggerSamplesLabel;
        private System.Windows.Forms.Label triggerLevelLabel;
        private System.Windows.Forms.NumericUpDown referenceLevelNumeric;
        private System.Windows.Forms.NumericUpDown carrierFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.NumericUpDown samplesPerRecordNumeric;
        private System.Windows.Forms.NumericUpDown pretriggerSamplesNumeric;
        private System.Windows.Forms.NumericUpDown triggerLevelNumeric;
        private System.Windows.Forms.ComboBox triggerTypeComboBox;
        private System.Windows.Forms.ComboBox triggerSourceComboBox;
        private System.Windows.Forms.Label triggerSourceLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Button startAcquisitionButton;
        private System.Windows.Forms.Button sendSoftwareTriggerbutton;
        private System.Windows.Forms.DataGridView dataGridViewResults;

    }
}
