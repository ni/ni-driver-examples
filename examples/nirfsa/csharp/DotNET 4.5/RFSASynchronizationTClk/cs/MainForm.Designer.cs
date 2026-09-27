namespace NationalInstruments.Examples.RfsaSynchronizationTClock
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
            this.numberOfDevicesLabel = new System.Windows.Forms.Label();
            this.referenceClockMasterLabel = new System.Windows.Forms.Label();
            this.referenceClockExportMasterLabel = new System.Windows.Forms.Label();
            this.referenceClockSlaveLabel = new System.Windows.Forms.Label();
            this.textmsgLabel = new System.Windows.Forms.Label();
            this.referenceClockExportSlaveLabel = new System.Windows.Forms.Label();
            this.referenceLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.carrierFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesPerRecordNumeric = new System.Windows.Forms.NumericUpDown();
            this.numberOfDevicesNumeric = new System.Windows.Forms.NumericUpDown();
            this.referenceClockMasterComboBox = new System.Windows.Forms.ComboBox();
            this.referenceClockExportMasterComboBox = new System.Windows.Forms.ComboBox();
            this.referenceClockSlaveComboBox = new System.Windows.Forms.ComboBox();
            this.referenceClockExportSlaveComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameMasterComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameMasterLabel = new System.Windows.Forms.Label();
            this.resourceNameSlaveComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameSlaveLabel = new System.Windows.Forms.Label();
            this.acquireButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.slaveDataLabel = new System.Windows.Forms.Label();
            this.masterDataLabel = new System.Windows.Forms.Label();
            this.dataGridViewResultsId1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewResultsId0 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfDevicesNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResultsId1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResultsId0)).BeginInit();
            this.SuspendLayout();
            // 
            // referenceLevelLabel
            // 
            this.referenceLevelLabel.AutoSize = true;
            this.referenceLevelLabel.Location = new System.Drawing.Point(12, 165);
            this.referenceLevelLabel.Name = "referenceLevelLabel";
            this.referenceLevelLabel.Size = new System.Drawing.Size(116, 13);
            this.referenceLevelLabel.TabIndex = 0;
            this.referenceLevelLabel.Text = "Reference Level (dBm)";
            // 
            // carrierFrequencyLabel
            // 
            this.carrierFrequencyLabel.AutoSize = true;
            this.carrierFrequencyLabel.Location = new System.Drawing.Point(12, 214);
            this.carrierFrequencyLabel.Name = "carrierFrequencyLabel";
            this.carrierFrequencyLabel.Size = new System.Drawing.Size(112, 13);
            this.carrierFrequencyLabel.TabIndex = 1;
            this.carrierFrequencyLabel.Text = "Carrier Frequency (Hz)";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(12, 260);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(44, 13);
            this.iqRateLabel.TabIndex = 2;
            this.iqRateLabel.Text = "IQ Rate";
            // 
            // samplesPerRecordLabel
            // 
            this.samplesPerRecordLabel.AutoSize = true;
            this.samplesPerRecordLabel.Location = new System.Drawing.Point(12, 306);
            this.samplesPerRecordLabel.Name = "samplesPerRecordLabel";
            this.samplesPerRecordLabel.Size = new System.Drawing.Size(103, 13);
            this.samplesPerRecordLabel.TabIndex = 3;
            this.samplesPerRecordLabel.Text = "Samples per Record";
            // 
            // numberOfDevicesLabel
            // 
            this.numberOfDevicesLabel.AutoSize = true;
            this.numberOfDevicesLabel.Location = new System.Drawing.Point(12, 27);
            this.numberOfDevicesLabel.Name = "numberOfDevicesLabel";
            this.numberOfDevicesLabel.Size = new System.Drawing.Size(98, 13);
            this.numberOfDevicesLabel.TabIndex = 4;
            this.numberOfDevicesLabel.Text = "Number of Devices";
            // 
            // referenceClockMasterLabel
            // 
            this.referenceClockMasterLabel.AutoSize = true;
            this.referenceClockMasterLabel.Location = new System.Drawing.Point(12, 355);
            this.referenceClockMasterLabel.Name = "referenceClockMasterLabel";
            this.referenceClockMasterLabel.Size = new System.Drawing.Size(122, 13);
            this.referenceClockMasterLabel.TabIndex = 5;
            this.referenceClockMasterLabel.Text = "Master Reference Clock";
            // 
            // referenceClockExportMasterLabel
            // 
            this.referenceClockExportMasterLabel.AutoSize = true;
            this.referenceClockExportMasterLabel.Location = new System.Drawing.Point(12, 403);
            this.referenceClockExportMasterLabel.Name = "referenceClockExportMasterLabel";
            this.referenceClockExportMasterLabel.Size = new System.Drawing.Size(155, 13);
            this.referenceClockExportMasterLabel.TabIndex = 6;
            this.referenceClockExportMasterLabel.Text = "Master Reference Clock Export";
            // 
            // referenceClockSlaveLabel
            // 
            this.referenceClockSlaveLabel.AutoSize = true;
            this.referenceClockSlaveLabel.Location = new System.Drawing.Point(12, 453);
            this.referenceClockSlaveLabel.Name = "referenceClockSlaveLabel";
            this.referenceClockSlaveLabel.Size = new System.Drawing.Size(117, 13);
            this.referenceClockSlaveLabel.TabIndex = 7;
            this.referenceClockSlaveLabel.Text = "Slave Reference Clock";
            // 
            // textmsgLabel
            // 
            this.textmsgLabel.Location = new System.Drawing.Point(0, 0);
            this.textmsgLabel.Name = "textmsgLabel";
            this.textmsgLabel.Size = new System.Drawing.Size(100, 23);
            this.textmsgLabel.TabIndex = 8;
            // 
            // referenceClockExportSlaveLabel
            // 
            this.referenceClockExportSlaveLabel.AutoSize = true;
            this.referenceClockExportSlaveLabel.Location = new System.Drawing.Point(12, 503);
            this.referenceClockExportSlaveLabel.Name = "referenceClockExportSlaveLabel";
            this.referenceClockExportSlaveLabel.Size = new System.Drawing.Size(150, 13);
            this.referenceClockExportSlaveLabel.TabIndex = 9;
            this.referenceClockExportSlaveLabel.Text = "Slave Reference Clock Export";
            // 
            // referenceLevelNumeric
            // 
            this.referenceLevelNumeric.DecimalPlaces = 2;
            this.referenceLevelNumeric.Location = new System.Drawing.Point(12, 186);
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
            this.referenceLevelNumeric.Size = new System.Drawing.Size(151, 20);
            this.referenceLevelNumeric.TabIndex = 3;
            // 
            // carrierFrequencyNumeric
            // 
            this.carrierFrequencyNumeric.DecimalPlaces = 2;
            this.carrierFrequencyNumeric.Location = new System.Drawing.Point(12, 235);
            this.carrierFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.carrierFrequencyNumeric.Name = "carrierFrequencyNumeric";
            this.carrierFrequencyNumeric.Size = new System.Drawing.Size(151, 20);
            this.carrierFrequencyNumeric.TabIndex = 4;
            this.carrierFrequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // iqRateNumeric
            // 
            this.iqRateNumeric.DecimalPlaces = 2;
            this.iqRateNumeric.Location = new System.Drawing.Point(12, 281);
            this.iqRateNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.iqRateNumeric.Name = "iqRateNumeric";
            this.iqRateNumeric.Size = new System.Drawing.Size(151, 20);
            this.iqRateNumeric.TabIndex = 5;
            this.iqRateNumeric.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // samplesPerRecordNumeric
            // 
            this.samplesPerRecordNumeric.Location = new System.Drawing.Point(12, 327);
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
            this.samplesPerRecordNumeric.Size = new System.Drawing.Size(151, 20);
            this.samplesPerRecordNumeric.TabIndex = 6;
            this.samplesPerRecordNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // numberOfDevicesNumeric
            // 
            this.numberOfDevicesNumeric.Enabled = false;
            this.numberOfDevicesNumeric.Location = new System.Drawing.Point(12, 48);
            this.numberOfDevicesNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numberOfDevicesNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfDevicesNumeric.Name = "numberOfDevicesNumeric";
            this.numberOfDevicesNumeric.ReadOnly = true;
            this.numberOfDevicesNumeric.Size = new System.Drawing.Size(153, 20);
            this.numberOfDevicesNumeric.TabIndex = 0;
            this.numberOfDevicesNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // referenceClockMasterComboBox
            // 
            this.referenceClockMasterComboBox.Location = new System.Drawing.Point(12, 376);
            this.referenceClockMasterComboBox.Name = "referenceClockMasterComboBox";
            this.referenceClockMasterComboBox.Size = new System.Drawing.Size(151, 21);
            this.referenceClockMasterComboBox.TabIndex = 7;
            // 
            // referenceClockExportMasterComboBox
            // 
            this.referenceClockExportMasterComboBox.Location = new System.Drawing.Point(12, 424);
            this.referenceClockExportMasterComboBox.Name = "referenceClockExportMasterComboBox";
            this.referenceClockExportMasterComboBox.Size = new System.Drawing.Size(151, 21);
            this.referenceClockExportMasterComboBox.TabIndex = 8;
            // 
            // referenceClockSlaveComboBox
            // 
            this.referenceClockSlaveComboBox.Location = new System.Drawing.Point(12, 474);
            this.referenceClockSlaveComboBox.Name = "referenceClockSlaveComboBox";
            this.referenceClockSlaveComboBox.Size = new System.Drawing.Size(151, 21);
            this.referenceClockSlaveComboBox.TabIndex = 9;
            // 
            // referenceClockExportSlaveComboBox
            // 
            this.referenceClockExportSlaveComboBox.Location = new System.Drawing.Point(12, 524);
            this.referenceClockExportSlaveComboBox.Name = "referenceClockExportSlaveComboBox";
            this.referenceClockExportSlaveComboBox.Size = new System.Drawing.Size(151, 21);
            this.referenceClockExportSlaveComboBox.TabIndex = 10;
            // 
            // resourceNameMasterComboBox
            // 
            this.resourceNameMasterComboBox.Location = new System.Drawing.Point(12, 96);
            this.resourceNameMasterComboBox.Name = "resourceNameMasterComboBox";
            this.resourceNameMasterComboBox.Size = new System.Drawing.Size(155, 21);
            this.resourceNameMasterComboBox.TabIndex = 1;
            // 
            // resourceNameMasterLabel
            // 
            this.resourceNameMasterLabel.AutoSize = true;
            this.resourceNameMasterLabel.Location = new System.Drawing.Point(12, 79);
            this.resourceNameMasterLabel.Name = "resourceNameMasterLabel";
            this.resourceNameMasterLabel.Size = new System.Drawing.Size(125, 13);
            this.resourceNameMasterLabel.TabIndex = 14;
            this.resourceNameMasterLabel.Text = "Resource Name (Master)";
            // 
            // resourceNameSlaveComboBox
            // 
            this.resourceNameSlaveComboBox.Location = new System.Drawing.Point(12, 140);
            this.resourceNameSlaveComboBox.Name = "resourceNameSlaveComboBox";
            this.resourceNameSlaveComboBox.Size = new System.Drawing.Size(155, 21);
            this.resourceNameSlaveComboBox.TabIndex = 2;
            // 
            // resourceNameSlaveLabel
            // 
            this.resourceNameSlaveLabel.AutoSize = true;
            this.resourceNameSlaveLabel.Location = new System.Drawing.Point(12, 123);
            this.resourceNameSlaveLabel.Name = "resourceNameSlaveLabel";
            this.resourceNameSlaveLabel.Size = new System.Drawing.Size(132, 13);
            this.resourceNameSlaveLabel.TabIndex = 16;
            this.resourceNameSlaveLabel.Text = "Resource Name ( Slave 1)";
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(12, 561);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(149, 23);
            this.acquireButton.TabIndex = 11;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(673, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "This example assumes that the association of the LO and digitizer for each device" +
    " in MAX is consistent with the Clock settings in this example.";
            // 
            // slaveDataLabel
            // 
            this.slaveDataLabel.AutoSize = true;
            this.slaveDataLabel.Location = new System.Drawing.Point(170, 305);
            this.slaveDataLabel.Name = "slaveDataLabel";
            this.slaveDataLabel.Size = new System.Drawing.Size(60, 13);
            this.slaveDataLabel.TabIndex = 28;
            this.slaveDataLabel.Text = "Slave Data";
            // 
            // masterDataLabel
            // 
            this.masterDataLabel.AutoSize = true;
            this.masterDataLabel.Location = new System.Drawing.Point(170, 24);
            this.masterDataLabel.Name = "masterDataLabel";
            this.masterDataLabel.Size = new System.Drawing.Size(65, 13);
            this.masterDataLabel.TabIndex = 27;
            this.masterDataLabel.Text = "Master Data";
            // 
            // dataGridViewResultsId1
            // 
            this.dataGridViewResultsId1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResultsId1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewResultsId1.Location = new System.Drawing.Point(173, 324);
            this.dataGridViewResultsId1.Name = "dataGridViewResultsId1";
            this.dataGridViewResultsId1.Size = new System.Drawing.Size(485, 260);
            this.dataGridViewResultsId1.TabIndex = 26;
            // 
            // dataGridViewResultsId0
            // 
            this.dataGridViewResultsId0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResultsId0.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewResultsId0.Location = new System.Drawing.Point(173, 40);
            this.dataGridViewResultsId0.Name = "dataGridViewResultsId0";
            this.dataGridViewResultsId0.Size = new System.Drawing.Size(485, 260);
            this.dataGridViewResultsId0.TabIndex = 25;
            // 
            // MainForm
            // 
            this.AcceptButton = this.acquireButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 599);
            this.Controls.Add(this.slaveDataLabel);
            this.Controls.Add(this.masterDataLabel);
            this.Controls.Add(this.dataGridViewResultsId1);
            this.Controls.Add(this.dataGridViewResultsId0);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.acquireButton);
            this.Controls.Add(this.resourceNameSlaveComboBox);
            this.Controls.Add(this.resourceNameSlaveLabel);
            this.Controls.Add(this.resourceNameMasterComboBox);
            this.Controls.Add(this.resourceNameMasterLabel);
            this.Controls.Add(this.referenceLevelLabel);
            this.Controls.Add(this.carrierFrequencyLabel);
            this.Controls.Add(this.iqRateLabel);
            this.Controls.Add(this.samplesPerRecordLabel);
            this.Controls.Add(this.numberOfDevicesLabel);
            this.Controls.Add(this.referenceClockMasterLabel);
            this.Controls.Add(this.referenceClockExportMasterLabel);
            this.Controls.Add(this.referenceClockSlaveLabel);
            this.Controls.Add(this.textmsgLabel);
            this.Controls.Add(this.referenceClockExportSlaveLabel);
            this.Controls.Add(this.referenceLevelNumeric);
            this.Controls.Add(this.carrierFrequencyNumeric);
            this.Controls.Add(this.iqRateNumeric);
            this.Controls.Add(this.samplesPerRecordNumeric);
            this.Controls.Add(this.numberOfDevicesNumeric);
            this.Controls.Add(this.referenceClockMasterComboBox);
            this.Controls.Add(this.referenceClockExportMasterComboBox);
            this.Controls.Add(this.referenceClockSlaveComboBox);
            this.Controls.Add(this.referenceClockExportSlaveComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "RFSA Synchronization (TClk, Shared LO and Reference Clock)";
            ((System.ComponentModel.ISupportInitialize)(this.referenceLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerRecordNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfDevicesNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResultsId1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResultsId0)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label referenceLevelLabel;
        private System.Windows.Forms.Label carrierFrequencyLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label samplesPerRecordLabel;
        private System.Windows.Forms.Label numberOfDevicesLabel;
        private System.Windows.Forms.Label referenceClockMasterLabel;
        private System.Windows.Forms.Label referenceClockExportMasterLabel;
        private System.Windows.Forms.Label referenceClockSlaveLabel;
        private System.Windows.Forms.Label textmsgLabel;
        private System.Windows.Forms.Label referenceClockExportSlaveLabel;
        private System.Windows.Forms.NumericUpDown referenceLevelNumeric;
        private System.Windows.Forms.NumericUpDown carrierFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.NumericUpDown samplesPerRecordNumeric;
        private System.Windows.Forms.NumericUpDown numberOfDevicesNumeric;
        private System.Windows.Forms.ComboBox referenceClockMasterComboBox;
        private System.Windows.Forms.ComboBox referenceClockExportMasterComboBox;
        private System.Windows.Forms.ComboBox referenceClockSlaveComboBox;
        private System.Windows.Forms.ComboBox referenceClockExportSlaveComboBox;
        private System.Windows.Forms.ComboBox resourceNameMasterComboBox;
        private System.Windows.Forms.Label resourceNameMasterLabel;
        private System.Windows.Forms.ComboBox resourceNameSlaveComboBox;
        private System.Windows.Forms.Label resourceNameSlaveLabel;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label slaveDataLabel;
        private System.Windows.Forms.Label masterDataLabel;
        private System.Windows.Forms.DataGridView dataGridViewResultsId1;
        private System.Windows.Forms.DataGridView dataGridViewResultsId0;

    }
}
