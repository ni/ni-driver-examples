namespace NationalInstruments.Examples.DmmSwitchSynchronousScanning
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
            this.measurementTypeLabel = new System.Windows.Forms.Label();
            this.measurementModeComboBox = new System.Windows.Forms.ComboBox();
            this.dmmResourceNameLabel = new System.Windows.Forms.Label();
            this.EntryNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dmmResourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.sampleIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.sampleIntervalLabel = new System.Windows.Forms.Label();
            this.measCompleteDestComboBox = new System.Windows.Forms.ComboBox();
            this.measurementCompleteDestinationLabel = new System.Windows.Forms.Label();
            this.samplesToFetchNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.rangeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.resolutionNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.resolutionLabel = new System.Windows.Forms.Label();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.scanListLabel = new System.Windows.Forms.Label();
            this.topologyNameLabel = new System.Windows.Forms.Label();
            this.topologyNameComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.scanListTextBox = new System.Windows.Forms.TextBox();
            this.triggerInputLabel = new System.Windows.Forms.Label();
            this.switchResourceNameLabel = new System.Windows.Forms.Label();
            this.switchResourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.switchTriggerInputComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sampleIntervalNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesToFetchNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resolutionNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // measurementTypeLabel
            // 
            this.measurementTypeLabel.AutoSize = true;
            this.measurementTypeLabel.Location = new System.Drawing.Point(9, 92);
            this.measurementTypeLabel.Name = "measurementTypeLabel";
            this.measurementTypeLabel.Size = new System.Drawing.Size(104, 13);
            this.measurementTypeLabel.TabIndex = 8;
            this.measurementTypeLabel.Text = "Measurement Mode:";
            // 
            // measurementModeComboBox
            // 
            this.measurementModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.measurementModeComboBox.FormattingEnabled = true;
            this.measurementModeComboBox.Location = new System.Drawing.Point(12, 108);
            this.measurementModeComboBox.Name = "measurementModeComboBox";
            this.measurementModeComboBox.Size = new System.Drawing.Size(121, 21);
            this.measurementModeComboBox.TabIndex = 1;
            // 
            // dmmResourceNameLabel
            // 
            this.dmmResourceNameLabel.AutoSize = true;
            this.dmmResourceNameLabel.Location = new System.Drawing.Point(9, 28);
            this.dmmResourceNameLabel.Name = "dmmResourceNameLabel";
            this.dmmResourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.dmmResourceNameLabel.TabIndex = 7;
            this.dmmResourceNameLabel.Text = "Resource Name:";
            // 
            // EntryNumber
            // 
            this.EntryNumber.HeaderText = "Entry Number";
            this.EntryNumber.Name = "EntryNumber";
            // 
            // dmmResourceNameComboBox
            // 
            this.dmmResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dmmResourceNameComboBox.FormattingEnabled = true;
            this.dmmResourceNameComboBox.Location = new System.Drawing.Point(12, 44);
            this.dmmResourceNameComboBox.Name = "dmmResourceNameComboBox";
            this.dmmResourceNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.dmmResourceNameComboBox.TabIndex = 0;
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EntryNumber,
            this.Value});
            this.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewResults.Location = new System.Drawing.Point(386, 21);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(244, 255);
            this.dataGridViewResults.TabIndex = 3;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(441, 282);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.sampleIntervalNumericUpDown);
            this.groupBox2.Controls.Add(this.sampleIntervalLabel);
            this.groupBox2.Controls.Add(this.measCompleteDestComboBox);
            this.groupBox2.Controls.Add(this.measurementCompleteDestinationLabel);
            this.groupBox2.Controls.Add(this.samplesToFetchNumericUpDown);
            this.groupBox2.Controls.Add(this.measurementTypeLabel);
            this.groupBox2.Controls.Add(this.samplesLabel);
            this.groupBox2.Controls.Add(this.measurementModeComboBox);
            this.groupBox2.Controls.Add(this.dmmResourceNameLabel);
            this.groupBox2.Controls.Add(this.rangeNumericUpDown);
            this.groupBox2.Controls.Add(this.resolutionNumericUpDown);
            this.groupBox2.Controls.Add(this.dmmResourceNameComboBox);
            this.groupBox2.Controls.Add(this.resolutionLabel);
            this.groupBox2.Controls.Add(this.rangeLabel);
            this.groupBox2.Location = new System.Drawing.Point(170, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(212, 305);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dmm";
            // 
            // sampleIntervalNumericUpDown
            // 
            this.sampleIntervalNumericUpDown.DecimalPlaces = 6;
            this.sampleIntervalNumericUpDown.Location = new System.Drawing.Point(112, 221);
            this.sampleIntervalNumericUpDown.Name = "sampleIntervalNumericUpDown";
            this.sampleIntervalNumericUpDown.Size = new System.Drawing.Size(94, 20);
            this.sampleIntervalNumericUpDown.TabIndex = 5;
            this.sampleIntervalNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            // 
            // sampleIntervalLabel
            // 
            this.sampleIntervalLabel.AutoSize = true;
            this.sampleIntervalLabel.Location = new System.Drawing.Point(109, 203);
            this.sampleIntervalLabel.Name = "sampleIntervalLabel";
            this.sampleIntervalLabel.Size = new System.Drawing.Size(80, 13);
            this.sampleIntervalLabel.TabIndex = 13;
            this.sampleIntervalLabel.Text = "Sample Interval";
            // 
            // measCompleteDestComboBox
            // 
            this.measCompleteDestComboBox.FormattingEnabled = true;
            this.measCompleteDestComboBox.Location = new System.Drawing.Point(12, 275);
            this.measCompleteDestComboBox.Name = "measCompleteDestComboBox";
            this.measCompleteDestComboBox.Size = new System.Drawing.Size(121, 21);
            this.measCompleteDestComboBox.TabIndex = 6;
            // 
            // measurementCompleteDestinationLabel
            // 
            this.measurementCompleteDestinationLabel.AutoSize = true;
            this.measurementCompleteDestinationLabel.Location = new System.Drawing.Point(9, 246);
            this.measurementCompleteDestinationLabel.Name = "measurementCompleteDestinationLabel";
            this.measurementCompleteDestinationLabel.Size = new System.Drawing.Size(118, 26);
            this.measurementCompleteDestinationLabel.TabIndex = 11;
            this.measurementCompleteDestinationLabel.Text = "Measurement Complete\r\nDestination";
            // 
            // samplesToFetchNumericUpDown
            // 
            this.samplesToFetchNumericUpDown.DecimalPlaces = 2;
            this.samplesToFetchNumericUpDown.Location = new System.Drawing.Point(12, 221);
            this.samplesToFetchNumericUpDown.Name = "samplesToFetchNumericUpDown";
            this.samplesToFetchNumericUpDown.Size = new System.Drawing.Size(94, 20);
            this.samplesToFetchNumericUpDown.TabIndex = 4;
            this.samplesToFetchNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // samplesLabel
            // 
            this.samplesLabel.AutoSize = true;
            this.samplesLabel.Location = new System.Drawing.Point(9, 203);
            this.samplesLabel.Name = "samplesLabel";
            this.samplesLabel.Size = new System.Drawing.Size(73, 13);
            this.samplesLabel.TabIndex = 10;
            this.samplesLabel.Text = "Sample Count";
            // 
            // rangeNumericUpDown
            // 
            this.rangeNumericUpDown.DecimalPlaces = 2;
            this.rangeNumericUpDown.Location = new System.Drawing.Point(12, 166);
            this.rangeNumericUpDown.Name = "rangeNumericUpDown";
            this.rangeNumericUpDown.Size = new System.Drawing.Size(94, 20);
            this.rangeNumericUpDown.TabIndex = 2;
            this.rangeNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // resolutionNumericUpDown
            // 
            this.resolutionNumericUpDown.DecimalPlaces = 6;
            this.resolutionNumericUpDown.Location = new System.Drawing.Point(112, 166);
            this.resolutionNumericUpDown.Name = "resolutionNumericUpDown";
            this.resolutionNumericUpDown.Size = new System.Drawing.Size(94, 20);
            this.resolutionNumericUpDown.TabIndex = 3;
            this.resolutionNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // resolutionLabel
            // 
            this.resolutionLabel.AutoSize = true;
            this.resolutionLabel.Location = new System.Drawing.Point(109, 150);
            this.resolutionLabel.Name = "resolutionLabel";
            this.resolutionLabel.Size = new System.Drawing.Size(57, 13);
            this.resolutionLabel.TabIndex = 12;
            this.resolutionLabel.Text = "Resolution";
            // 
            // rangeLabel
            // 
            this.rangeLabel.AutoSize = true;
            this.rangeLabel.Location = new System.Drawing.Point(9, 149);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.Size = new System.Drawing.Size(39, 13);
            this.rangeLabel.TabIndex = 9;
            this.rangeLabel.Text = "Range";
            // 
            // scanListLabel
            // 
            this.scanListLabel.AutoSize = true;
            this.scanListLabel.Location = new System.Drawing.Point(6, 149);
            this.scanListLabel.Name = "scanListLabel";
            this.scanListLabel.Size = new System.Drawing.Size(51, 13);
            this.scanListLabel.TabIndex = 6;
            this.scanListLabel.Text = "Scan List";
            // 
            // topologyNameLabel
            // 
            this.topologyNameLabel.AutoSize = true;
            this.topologyNameLabel.Location = new System.Drawing.Point(6, 92);
            this.topologyNameLabel.Name = "topologyNameLabel";
            this.topologyNameLabel.Size = new System.Drawing.Size(82, 13);
            this.topologyNameLabel.TabIndex = 5;
            this.topologyNameLabel.Text = "Topology Name";
            // 
            // topologyNameComboBox
            // 
            this.topologyNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.topologyNameComboBox.FormattingEnabled = true;
            this.topologyNameComboBox.Location = new System.Drawing.Point(6, 108);
            this.topologyNameComboBox.Name = "topologyNameComboBox";
            this.topologyNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.topologyNameComboBox.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.scanListLabel);
            this.groupBox1.Controls.Add(this.topologyNameLabel);
            this.groupBox1.Controls.Add(this.topologyNameComboBox);
            this.groupBox1.Controls.Add(this.scanListTextBox);
            this.groupBox1.Controls.Add(this.triggerInputLabel);
            this.groupBox1.Controls.Add(this.switchResourceNameLabel);
            this.groupBox1.Controls.Add(this.switchResourceNameComboBox);
            this.groupBox1.Controls.Add(this.switchTriggerInputComboBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(152, 305);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Switch";
            // 
            // scanListTextBox
            // 
            this.scanListTextBox.Location = new System.Drawing.Point(6, 165);
            this.scanListTextBox.Name = "scanListTextBox";
            this.scanListTextBox.Size = new System.Drawing.Size(121, 20);
            this.scanListTextBox.TabIndex = 2;
            this.scanListTextBox.Text = "ch0:5->com0;";
            // 
            // triggerInputLabel
            // 
            this.triggerInputLabel.AutoSize = true;
            this.triggerInputLabel.Location = new System.Drawing.Point(6, 203);
            this.triggerInputLabel.Name = "triggerInputLabel";
            this.triggerInputLabel.Size = new System.Drawing.Size(67, 13);
            this.triggerInputLabel.TabIndex = 7;
            this.triggerInputLabel.Text = "Trigger Input";
            // 
            // switchResourceNameLabel
            // 
            this.switchResourceNameLabel.AutoSize = true;
            this.switchResourceNameLabel.Location = new System.Drawing.Point(6, 27);
            this.switchResourceNameLabel.Name = "switchResourceNameLabel";
            this.switchResourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.switchResourceNameLabel.TabIndex = 4;
            this.switchResourceNameLabel.Text = "Resource Name";
            // 
            // switchResourceNameComboBox
            // 
            this.switchResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.switchResourceNameComboBox.FormattingEnabled = true;
            this.switchResourceNameComboBox.Location = new System.Drawing.Point(6, 43);
            this.switchResourceNameComboBox.Name = "switchResourceNameComboBox";
            this.switchResourceNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.switchResourceNameComboBox.TabIndex = 0;
            // 
            // switchTriggerInputComboBox
            // 
            this.switchTriggerInputComboBox.FormattingEnabled = true;
            this.switchTriggerInputComboBox.Location = new System.Drawing.Point(6, 220);
            this.switchTriggerInputComboBox.Name = "switchTriggerInputComboBox";
            this.switchTriggerInputComboBox.Size = new System.Drawing.Size(121, 21);
            this.switchTriggerInputComboBox.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 326);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Dmm Switch Synchronous Scanning";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sampleIntervalNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesToFetchNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resolutionNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label measurementTypeLabel;
        private System.Windows.Forms.ComboBox measurementModeComboBox;
        private System.Windows.Forms.Label dmmResourceNameLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn EntryNumber;
        private System.Windows.Forms.ComboBox dmmResourceNameComboBox;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label scanListLabel;
        private System.Windows.Forms.Label topologyNameLabel;
        private System.Windows.Forms.ComboBox topologyNameComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox scanListTextBox;
        private System.Windows.Forms.Label switchResourceNameLabel;
        private System.Windows.Forms.ComboBox switchResourceNameComboBox;
        private System.Windows.Forms.NumericUpDown samplesToFetchNumericUpDown;
        private System.Windows.Forms.Label samplesLabel;
        private System.Windows.Forms.NumericUpDown rangeNumericUpDown;
        private System.Windows.Forms.NumericUpDown resolutionNumericUpDown;
        private System.Windows.Forms.Label resolutionLabel;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.Label triggerInputLabel;
        private System.Windows.Forms.ComboBox switchTriggerInputComboBox;
        private System.Windows.Forms.ComboBox measCompleteDestComboBox;
        private System.Windows.Forms.Label measurementCompleteDestinationLabel;
        private System.Windows.Forms.NumericUpDown sampleIntervalNumericUpDown;
        private System.Windows.Forms.Label sampleIntervalLabel;
    }
}

