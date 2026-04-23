namespace NationalInstruments.Examples.InterleavedScanningPxi2584
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
            this.switchInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.switchResourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.switchScanAdvancedOutputComboBox = new System.Windows.Forms.ComboBox();
            this.switchTriggerInputComboBox = new System.Windows.Forms.ComboBox();
            this.scanAdvancedOutputLabel = new System.Windows.Forms.Label();
            this.switchInterleavedEndChannelNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.triggerInputLabel = new System.Windows.Forms.Label();
            this.interleavedEndChannelLabel = new System.Windows.Forms.Label();
            this.interleavedStartChannelLabel = new System.Windows.Forms.Label();
            this.switchInterleavedStartChannelNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.switchResourceNameLabel = new System.Windows.Forms.Label();
            this.dmmInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.dmmResourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.dmmMeasurementTypeComboBox = new System.Windows.Forms.ComboBox();
            this.dmmRangeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.measurementCompleteDestinationLabel = new System.Windows.Forms.Label();
            this.dmmTriggerSourceComboBox = new System.Windows.Forms.ComboBox();
            this.dmmMeasurementCompleteDestinationComboBox = new System.Windows.Forms.ComboBox();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.dmmSamplesNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.resolutionLabel = new System.Windows.Forms.Label();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.measurementTypeLabel = new System.Windows.Forms.Label();
            this.dmmResolutionNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.switchInfoGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.switchInterleavedEndChannelNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchInterleavedStartChannelNumericUpDown)).BeginInit();
            this.dmmInfoGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dmmRangeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dmmSamplesNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dmmResolutionNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.SuspendLayout();
            // 
            // switchInfoGroupBox
            // 
            this.switchInfoGroupBox.Controls.Add(this.switchResourceNameComboBox);
            this.switchInfoGroupBox.Controls.Add(this.switchScanAdvancedOutputComboBox);
            this.switchInfoGroupBox.Controls.Add(this.switchTriggerInputComboBox);
            this.switchInfoGroupBox.Controls.Add(this.scanAdvancedOutputLabel);
            this.switchInfoGroupBox.Controls.Add(this.switchInterleavedEndChannelNumericUpDown);
            this.switchInfoGroupBox.Controls.Add(this.triggerInputLabel);
            this.switchInfoGroupBox.Controls.Add(this.interleavedEndChannelLabel);
            this.switchInfoGroupBox.Controls.Add(this.interleavedStartChannelLabel);
            this.switchInfoGroupBox.Controls.Add(this.switchInterleavedStartChannelNumericUpDown);
            this.switchInfoGroupBox.Controls.Add(this.switchResourceNameLabel);
            this.switchInfoGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.switchInfoGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.switchInfoGroupBox.Location = new System.Drawing.Point(12, 26);
            this.switchInfoGroupBox.Name = "switchInfoGroupBox";
            this.switchInfoGroupBox.Size = new System.Drawing.Size(305, 282);
            this.switchInfoGroupBox.TabIndex = 0;
            this.switchInfoGroupBox.TabStop = false;
            this.switchInfoGroupBox.Text = "Switch";
            // 
            // switchResourceNameComboBox
            // 
            this.switchResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.switchResourceNameComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.switchResourceNameComboBox.FormattingEnabled = true;
            this.switchResourceNameComboBox.Location = new System.Drawing.Point(149, 21);
            this.switchResourceNameComboBox.Name = "switchResourceNameComboBox";
            this.switchResourceNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.switchResourceNameComboBox.TabIndex = 0;
            // 
            // switchScanAdvancedOutputComboBox
            // 
            this.switchScanAdvancedOutputComboBox.FormattingEnabled = true;
            this.switchScanAdvancedOutputComboBox.Location = new System.Drawing.Point(149, 173);
            this.switchScanAdvancedOutputComboBox.Name = "switchScanAdvancedOutputComboBox";
            this.switchScanAdvancedOutputComboBox.Size = new System.Drawing.Size(121, 21);
            this.switchScanAdvancedOutputComboBox.TabIndex = 4;
            // 
            // switchTriggerInputComboBox
            // 
            this.switchTriggerInputComboBox.FormattingEnabled = true;
            this.switchTriggerInputComboBox.Location = new System.Drawing.Point(149, 143);
            this.switchTriggerInputComboBox.Name = "switchTriggerInputComboBox";
            this.switchTriggerInputComboBox.Size = new System.Drawing.Size(121, 21);
            this.switchTriggerInputComboBox.TabIndex = 3;
            // 
            // scanAdvancedOutputLabel
            // 
            this.scanAdvancedOutputLabel.AutoSize = true;
            this.scanAdvancedOutputLabel.Location = new System.Drawing.Point(6, 176);
            this.scanAdvancedOutputLabel.Name = "scanAdvancedOutputLabel";
            this.scanAdvancedOutputLabel.Size = new System.Drawing.Size(119, 13);
            this.scanAdvancedOutputLabel.TabIndex = 9;
            this.scanAdvancedOutputLabel.Text = "Scan Advanced Output";
            // 
            // switchInterleavedEndChannelNumericUpDown
            // 
            this.switchInterleavedEndChannelNumericUpDown.Location = new System.Drawing.Point(150, 102);
            this.switchInterleavedEndChannelNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.switchInterleavedEndChannelNumericUpDown.Name = "switchInterleavedEndChannelNumericUpDown";
            this.switchInterleavedEndChannelNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.switchInterleavedEndChannelNumericUpDown.TabIndex = 2;
            this.switchInterleavedEndChannelNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // triggerInputLabel
            // 
            this.triggerInputLabel.AutoSize = true;
            this.triggerInputLabel.Location = new System.Drawing.Point(6, 143);
            this.triggerInputLabel.Name = "triggerInputLabel";
            this.triggerInputLabel.Size = new System.Drawing.Size(67, 13);
            this.triggerInputLabel.TabIndex = 8;
            this.triggerInputLabel.Text = "Trigger Input";
            // 
            // interleavedEndChannelLabel
            // 
            this.interleavedEndChannelLabel.AutoSize = true;
            this.interleavedEndChannelLabel.Location = new System.Drawing.Point(6, 104);
            this.interleavedEndChannelLabel.Name = "interleavedEndChannelLabel";
            this.interleavedEndChannelLabel.Size = new System.Drawing.Size(124, 13);
            this.interleavedEndChannelLabel.TabIndex = 7;
            this.interleavedEndChannelLabel.Text = "Interleaved End Channel";
            // 
            // interleavedStartChannelLabel
            // 
            this.interleavedStartChannelLabel.AutoSize = true;
            this.interleavedStartChannelLabel.Location = new System.Drawing.Point(6, 68);
            this.interleavedStartChannelLabel.Name = "interleavedStartChannelLabel";
            this.interleavedStartChannelLabel.Size = new System.Drawing.Size(127, 13);
            this.interleavedStartChannelLabel.TabIndex = 6;
            this.interleavedStartChannelLabel.Text = "Interleaved Start Channel";
            // 
            // switchInterleavedStartChannelNumericUpDown
            // 
            this.switchInterleavedStartChannelNumericUpDown.Location = new System.Drawing.Point(150, 61);
            this.switchInterleavedStartChannelNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.switchInterleavedStartChannelNumericUpDown.Name = "switchInterleavedStartChannelNumericUpDown";
            this.switchInterleavedStartChannelNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.switchInterleavedStartChannelNumericUpDown.TabIndex = 1;
            // 
            // switchResourceNameLabel
            // 
            this.switchResourceNameLabel.AutoSize = true;
            this.switchResourceNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.switchResourceNameLabel.Location = new System.Drawing.Point(6, 24);
            this.switchResourceNameLabel.Name = "switchResourceNameLabel";
            this.switchResourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.switchResourceNameLabel.TabIndex = 5;
            this.switchResourceNameLabel.Text = "Resource Name";
            // 
            // dmmInfoGroupBox
            // 
            this.dmmInfoGroupBox.Controls.Add(this.dmmResourceNameComboBox);
            this.dmmInfoGroupBox.Controls.Add(this.dmmMeasurementTypeComboBox);
            this.dmmInfoGroupBox.Controls.Add(this.dmmRangeNumericUpDown);
            this.dmmInfoGroupBox.Controls.Add(this.triggerSourceLabel);
            this.dmmInfoGroupBox.Controls.Add(this.measurementCompleteDestinationLabel);
            this.dmmInfoGroupBox.Controls.Add(this.dmmTriggerSourceComboBox);
            this.dmmInfoGroupBox.Controls.Add(this.dmmMeasurementCompleteDestinationComboBox);
            this.dmmInfoGroupBox.Controls.Add(this.samplesLabel);
            this.dmmInfoGroupBox.Controls.Add(this.dmmSamplesNumericUpDown);
            this.dmmInfoGroupBox.Controls.Add(this.resolutionLabel);
            this.dmmInfoGroupBox.Controls.Add(this.rangeLabel);
            this.dmmInfoGroupBox.Controls.Add(this.measurementTypeLabel);
            this.dmmInfoGroupBox.Controls.Add(this.dmmResolutionNumericUpDown);
            this.dmmInfoGroupBox.Controls.Add(this.resourceNameLabel);
            this.dmmInfoGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.dmmInfoGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.dmmInfoGroupBox.Location = new System.Drawing.Point(323, 26);
            this.dmmInfoGroupBox.Name = "dmmInfoGroupBox";
            this.dmmInfoGroupBox.Size = new System.Drawing.Size(316, 282);
            this.dmmInfoGroupBox.TabIndex = 1;
            this.dmmInfoGroupBox.TabStop = false;
            this.dmmInfoGroupBox.Text = "DMM";
            // 
            // dmmResourceNameComboBox
            // 
            this.dmmResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dmmResourceNameComboBox.FormattingEnabled = true;
            this.dmmResourceNameComboBox.Location = new System.Drawing.Point(158, 21);
            this.dmmResourceNameComboBox.Name = "dmmResourceNameComboBox";
            this.dmmResourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.dmmResourceNameComboBox.TabIndex = 0;
            // 
            // dmmMeasurementTypeComboBox
            // 
            this.dmmMeasurementTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dmmMeasurementTypeComboBox.FormattingEnabled = true;
            this.dmmMeasurementTypeComboBox.Location = new System.Drawing.Point(158, 65);
            this.dmmMeasurementTypeComboBox.Name = "dmmMeasurementTypeComboBox";
            this.dmmMeasurementTypeComboBox.Size = new System.Drawing.Size(120, 21);
            this.dmmMeasurementTypeComboBox.TabIndex = 1;
            // 
            // dmmRangeNumericUpDown
            // 
            this.dmmRangeNumericUpDown.DecimalPlaces = 6;
            this.dmmRangeNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.dmmRangeNumericUpDown.Location = new System.Drawing.Point(158, 102);
            this.dmmRangeNumericUpDown.Minimum = new decimal(new int[] {
            300,
            0,
            0,
            -2147352576});
            this.dmmRangeNumericUpDown.Name = "dmmRangeNumericUpDown";
            this.dmmRangeNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.dmmRangeNumericUpDown.TabIndex = 2;
            this.dmmRangeNumericUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            131072});
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.AutoSize = true;
            this.triggerSourceLabel.Location = new System.Drawing.Point(6, 252);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(77, 13);
            this.triggerSourceLabel.TabIndex = 13;
            this.triggerSourceLabel.Text = "Trigger Source";
            // 
            // measurementCompleteDestinationLabel
            // 
            this.measurementCompleteDestinationLabel.AutoSize = true;
            this.measurementCompleteDestinationLabel.Location = new System.Drawing.Point(6, 211);
            this.measurementCompleteDestinationLabel.Name = "measurementCompleteDestinationLabel";
            this.measurementCompleteDestinationLabel.Size = new System.Drawing.Size(151, 13);
            this.measurementCompleteDestinationLabel.TabIndex = 12;
            this.measurementCompleteDestinationLabel.Text = "Measure Complete Destination";
            // 
            // dmmTriggerSourceComboBox
            // 
            this.dmmTriggerSourceComboBox.Location = new System.Drawing.Point(158, 244);
            this.dmmTriggerSourceComboBox.Name = "dmmTriggerSourceComboBox";
            this.dmmTriggerSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.dmmTriggerSourceComboBox.TabIndex = 6;
            // 
            // dmmMeasurementCompleteDestinationComboBox
            // 
            this.dmmMeasurementCompleteDestinationComboBox.FormattingEnabled = true;
            this.dmmMeasurementCompleteDestinationComboBox.Location = new System.Drawing.Point(158, 208);
            this.dmmMeasurementCompleteDestinationComboBox.Name = "dmmMeasurementCompleteDestinationComboBox";
            this.dmmMeasurementCompleteDestinationComboBox.Size = new System.Drawing.Size(120, 21);
            this.dmmMeasurementCompleteDestinationComboBox.TabIndex = 5;
            // 
            // samplesLabel
            // 
            this.samplesLabel.AutoSize = true;
            this.samplesLabel.Location = new System.Drawing.Point(6, 176);
            this.samplesLabel.Name = "samplesLabel";
            this.samplesLabel.Size = new System.Drawing.Size(136, 13);
            this.samplesLabel.TabIndex = 11;
            this.samplesLabel.Text = "Samples to Fetch at a Time";
            // 
            // dmmSamplesNumericUpDown
            // 
            this.dmmSamplesNumericUpDown.Location = new System.Drawing.Point(158, 169);
            this.dmmSamplesNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dmmSamplesNumericUpDown.Name = "dmmSamplesNumericUpDown";
            this.dmmSamplesNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.dmmSamplesNumericUpDown.TabIndex = 4;
            this.dmmSamplesNumericUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // resolutionLabel
            // 
            this.resolutionLabel.AutoSize = true;
            this.resolutionLabel.Location = new System.Drawing.Point(6, 143);
            this.resolutionLabel.Name = "resolutionLabel";
            this.resolutionLabel.Size = new System.Drawing.Size(57, 13);
            this.resolutionLabel.TabIndex = 10;
            this.resolutionLabel.Text = "Resolution";
            // 
            // rangeLabel
            // 
            this.rangeLabel.AutoSize = true;
            this.rangeLabel.Location = new System.Drawing.Point(6, 104);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.Size = new System.Drawing.Size(39, 13);
            this.rangeLabel.TabIndex = 9;
            this.rangeLabel.Text = "Range";
            // 
            // measurementTypeLabel
            // 
            this.measurementTypeLabel.AutoSize = true;
            this.measurementTypeLabel.Location = new System.Drawing.Point(6, 68);
            this.measurementTypeLabel.Name = "measurementTypeLabel";
            this.measurementTypeLabel.Size = new System.Drawing.Size(98, 13);
            this.measurementTypeLabel.TabIndex = 8;
            this.measurementTypeLabel.Text = "Measurement Type";
            // 
            // dmmResolutionNumericUpDown
            // 
            this.dmmResolutionNumericUpDown.DecimalPlaces = 6;
            this.dmmResolutionNumericUpDown.Location = new System.Drawing.Point(158, 136);
            this.dmmResolutionNumericUpDown.Name = "dmmResolutionNumericUpDown";
            this.dmmResolutionNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.dmmResolutionNumericUpDown.TabIndex = 3;
            this.dmmResolutionNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 24);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 7;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.Location = new System.Drawing.Point(663, 26);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(293, 300);
            this.dataGridViewResults.TabIndex = 4;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(185, 339);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(97, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(383, 339);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(97, 23);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(968, 397);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.dmmInfoGroupBox);
            this.Controls.Add(this.switchInfoGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "NI-Switch Interleaved Scanning-PXI-2584";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.switchInfoGroupBox.ResumeLayout(false);
            this.switchInfoGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.switchInterleavedEndChannelNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.switchInterleavedStartChannelNumericUpDown)).EndInit();
            this.dmmInfoGroupBox.ResumeLayout(false);
            this.dmmInfoGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dmmRangeNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dmmSamplesNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dmmResolutionNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox switchInfoGroupBox;
        private System.Windows.Forms.Label switchResourceNameLabel;
        private System.Windows.Forms.ComboBox switchScanAdvancedOutputComboBox;
        private System.Windows.Forms.ComboBox switchTriggerInputComboBox;
        private System.Windows.Forms.Label scanAdvancedOutputLabel;
        private System.Windows.Forms.NumericUpDown switchInterleavedEndChannelNumericUpDown;
        private System.Windows.Forms.Label triggerInputLabel;
        private System.Windows.Forms.Label interleavedEndChannelLabel;
        private System.Windows.Forms.Label interleavedStartChannelLabel;
        private System.Windows.Forms.NumericUpDown switchInterleavedStartChannelNumericUpDown;
        private System.Windows.Forms.GroupBox dmmInfoGroupBox;
        private System.Windows.Forms.ComboBox dmmTriggerSourceComboBox;
        private System.Windows.Forms.ComboBox dmmMeasurementCompleteDestinationComboBox;
        private System.Windows.Forms.Label samplesLabel;
        private System.Windows.Forms.NumericUpDown dmmSamplesNumericUpDown;
        private System.Windows.Forms.Label resolutionLabel;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.Label measurementTypeLabel;
        private System.Windows.Forms.NumericUpDown dmmResolutionNumericUpDown;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.ComboBox dmmMeasurementTypeComboBox;
        private System.Windows.Forms.NumericUpDown dmmRangeNumericUpDown;
        private System.Windows.Forms.Label triggerSourceLabel;
        private System.Windows.Forms.Label measurementCompleteDestinationLabel;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ComboBox switchResourceNameComboBox;
        private System.Windows.Forms.ComboBox dmmResourceNameComboBox;
        private System.Windows.Forms.Button stopButton;
    }
}

