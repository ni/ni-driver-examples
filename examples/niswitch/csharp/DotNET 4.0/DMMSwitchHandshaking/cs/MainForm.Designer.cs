namespace NationalInstruments.Examples.DmmSwitchHandshaking
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.scanAdvancedOutputComboBox = new System.Windows.Forms.ComboBox();
            this.scanAdvancedOutputLabel = new System.Windows.Forms.Label();
            this.switchTriggerInputComboBox = new System.Windows.Forms.ComboBox();
            this.triggerInputLabel = new System.Windows.Forms.Label();
            this.scanListLabel = new System.Windows.Forms.Label();
            this.topologyNameLabel = new System.Windows.Forms.Label();
            this.topologyNameComboBox = new System.Windows.Forms.ComboBox();
            this.scanListTextBox = new System.Windows.Forms.TextBox();
            this.switchResourceNameLabel = new System.Windows.Forms.Label();
            this.switchResourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.samplesToFetchNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.samplesToFetchLabel = new System.Windows.Forms.Label();
            this.rangeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.resolutionNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.resolutionLabel = new System.Windows.Forms.Label();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.measCompleteDestComboBox = new System.Windows.Forms.ComboBox();
            this.triggerSourceComboBox = new System.Windows.Forms.ComboBox();
            this.measurementCompleteDestinationLabel = new System.Windows.Forms.Label();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.measurementTypeLabel = new System.Windows.Forms.Label();
            this.measurementModeComboBox = new System.Windows.Forms.ComboBox();
            this.dmmResourceNameLabel = new System.Windows.Forms.Label();
            this.dmmResourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.startButton = new System.Windows.Forms.Button();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.stopButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesToFetchNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resolutionNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.scanAdvancedOutputComboBox);
            this.groupBox1.Controls.Add(this.scanAdvancedOutputLabel);
            this.groupBox1.Controls.Add(this.switchTriggerInputComboBox);
            this.groupBox1.Controls.Add(this.triggerInputLabel);
            this.groupBox1.Controls.Add(this.scanListLabel);
            this.groupBox1.Controls.Add(this.topologyNameLabel);
            this.groupBox1.Controls.Add(this.topologyNameComboBox);
            this.groupBox1.Controls.Add(this.scanListTextBox);
            this.groupBox1.Controls.Add(this.switchResourceNameLabel);
            this.groupBox1.Controls.Add(this.switchResourceNameComboBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(152, 359);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Switch";
            // 
            // scanAdvancedOutputComboBox
            // 
            this.scanAdvancedOutputComboBox.FormattingEnabled = true;
            this.scanAdvancedOutputComboBox.Location = new System.Drawing.Point(6, 265);
            this.scanAdvancedOutputComboBox.Name = "scanAdvancedOutputComboBox";
            this.scanAdvancedOutputComboBox.Size = new System.Drawing.Size(121, 21);
            this.scanAdvancedOutputComboBox.TabIndex = 4;
            // 
            // scanAdvancedOutputLabel
            // 
            this.scanAdvancedOutputLabel.AutoSize = true;
            this.scanAdvancedOutputLabel.Location = new System.Drawing.Point(6, 249);
            this.scanAdvancedOutputLabel.Name = "scanAdvancedOutputLabel";
            this.scanAdvancedOutputLabel.Size = new System.Drawing.Size(119, 13);
            this.scanAdvancedOutputLabel.TabIndex = 9;
            this.scanAdvancedOutputLabel.Text = "Scan Advanced Output";
            // 
            // switchTriggerInputComboBox
            // 
            this.switchTriggerInputComboBox.FormattingEnabled = true;
            this.switchTriggerInputComboBox.Location = new System.Drawing.Point(6, 215);
            this.switchTriggerInputComboBox.Name = "switchTriggerInputComboBox";
            this.switchTriggerInputComboBox.Size = new System.Drawing.Size(121, 21);
            this.switchTriggerInputComboBox.TabIndex = 3;
            // 
            // triggerInputLabel
            // 
            this.triggerInputLabel.AutoSize = true;
            this.triggerInputLabel.Location = new System.Drawing.Point(6, 199);
            this.triggerInputLabel.Name = "triggerInputLabel";
            this.triggerInputLabel.Size = new System.Drawing.Size(67, 13);
            this.triggerInputLabel.TabIndex = 8;
            this.triggerInputLabel.Text = "Trigger Input";
            // 
            // scanListLabel
            // 
            this.scanListLabel.AutoSize = true;
            this.scanListLabel.Location = new System.Drawing.Point(6, 149);
            this.scanListLabel.Name = "scanListLabel";
            this.scanListLabel.Size = new System.Drawing.Size(51, 13);
            this.scanListLabel.TabIndex = 7;
            this.scanListLabel.Text = "Scan List";
            // 
            // topologyNameLabel
            // 
            this.topologyNameLabel.AutoSize = true;
            this.topologyNameLabel.Location = new System.Drawing.Point(6, 92);
            this.topologyNameLabel.Name = "topologyNameLabel";
            this.topologyNameLabel.Size = new System.Drawing.Size(82, 13);
            this.topologyNameLabel.TabIndex = 6;
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
            // scanListTextBox
            // 
            this.scanListTextBox.Location = new System.Drawing.Point(6, 165);
            this.scanListTextBox.Name = "scanListTextBox";
            this.scanListTextBox.Size = new System.Drawing.Size(121, 20);
            this.scanListTextBox.TabIndex = 2;
            this.scanListTextBox.Text = "ab0->com0 & com0->r0 & c0:31->r0;";
            // 
            // switchResourceNameLabel
            // 
            this.switchResourceNameLabel.AutoSize = true;
            this.switchResourceNameLabel.Location = new System.Drawing.Point(6, 27);
            this.switchResourceNameLabel.Name = "switchResourceNameLabel";
            this.switchResourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.switchResourceNameLabel.TabIndex = 5;
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.samplesToFetchNumericUpDown);
            this.groupBox2.Controls.Add(this.samplesToFetchLabel);
            this.groupBox2.Controls.Add(this.rangeNumericUpDown);
            this.groupBox2.Controls.Add(this.resolutionNumericUpDown);
            this.groupBox2.Controls.Add(this.resolutionLabel);
            this.groupBox2.Controls.Add(this.rangeLabel);
            this.groupBox2.Controls.Add(this.measCompleteDestComboBox);
            this.groupBox2.Controls.Add(this.triggerSourceComboBox);
            this.groupBox2.Controls.Add(this.measurementCompleteDestinationLabel);
            this.groupBox2.Controls.Add(this.triggerSourceLabel);
            this.groupBox2.Controls.Add(this.measurementTypeLabel);
            this.groupBox2.Controls.Add(this.measurementModeComboBox);
            this.groupBox2.Controls.Add(this.dmmResourceNameLabel);
            this.groupBox2.Controls.Add(this.dmmResourceNameComboBox);
            this.groupBox2.Location = new System.Drawing.Point(170, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(206, 359);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dmm";
            // 
            // samplesToFetchNumericUpDown
            // 
            this.samplesToFetchNumericUpDown.DecimalPlaces = 2;
            this.samplesToFetchNumericUpDown.Location = new System.Drawing.Point(6, 216);
            this.samplesToFetchNumericUpDown.Name = "samplesToFetchNumericUpDown";
            this.samplesToFetchNumericUpDown.Size = new System.Drawing.Size(94, 20);
            this.samplesToFetchNumericUpDown.TabIndex = 4;
            this.samplesToFetchNumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // samplesToFetchLabel
            // 
            this.samplesToFetchLabel.AutoSize = true;
            this.samplesToFetchLabel.Location = new System.Drawing.Point(3, 200);
            this.samplesToFetchLabel.Name = "samplesToFetchLabel";
            this.samplesToFetchLabel.Size = new System.Drawing.Size(140, 13);
            this.samplesToFetchLabel.TabIndex = 10;
            this.samplesToFetchLabel.Text = "Samples To Fetch at a Time";
            // 
            // rangeNumericUpDown
            // 
            this.rangeNumericUpDown.DecimalPlaces = 2;
            this.rangeNumericUpDown.Location = new System.Drawing.Point(6, 166);
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
            this.resolutionNumericUpDown.Location = new System.Drawing.Point(105, 166);
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
            this.resolutionLabel.Location = new System.Drawing.Point(125, 149);
            this.resolutionLabel.Name = "resolutionLabel";
            this.resolutionLabel.Size = new System.Drawing.Size(57, 13);
            this.resolutionLabel.TabIndex = 13;
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
            // measCompleteDestComboBox
            // 
            this.measCompleteDestComboBox.FormattingEnabled = true;
            this.measCompleteDestComboBox.Location = new System.Drawing.Point(9, 277);
            this.measCompleteDestComboBox.Name = "measCompleteDestComboBox";
            this.measCompleteDestComboBox.Size = new System.Drawing.Size(121, 21);
            this.measCompleteDestComboBox.TabIndex = 5;
            // 
            // triggerSourceComboBox
            // 
            this.triggerSourceComboBox.FormattingEnabled = true;
            this.triggerSourceComboBox.Location = new System.Drawing.Point(9, 327);
            this.triggerSourceComboBox.Name = "triggerSourceComboBox";
            this.triggerSourceComboBox.Size = new System.Drawing.Size(121, 21);
            this.triggerSourceComboBox.TabIndex = 6;
            // 
            // measurementCompleteDestinationLabel
            // 
            this.measurementCompleteDestinationLabel.AutoSize = true;
            this.measurementCompleteDestinationLabel.Location = new System.Drawing.Point(9, 248);
            this.measurementCompleteDestinationLabel.Name = "measurementCompleteDestinationLabel";
            this.measurementCompleteDestinationLabel.Size = new System.Drawing.Size(118, 26);
            this.measurementCompleteDestinationLabel.TabIndex = 11;
            this.measurementCompleteDestinationLabel.Text = "Measurement Complete\r\nDestination";
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.AutoSize = true;
            this.triggerSourceLabel.Location = new System.Drawing.Point(9, 311);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(77, 13);
            this.triggerSourceLabel.TabIndex = 12;
            this.triggerSourceLabel.Text = "Trigger Source";
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
            // dmmResourceNameComboBox
            // 
            this.dmmResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dmmResourceNameComboBox.FormattingEnabled = true;
            this.dmmResourceNameComboBox.Location = new System.Drawing.Point(12, 44);
            this.dmmResourceNameComboBox.Name = "dmmResourceNameComboBox";
            this.dmmResourceNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.dmmResourceNameComboBox.TabIndex = 0;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(406, 343);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewResults.Location = new System.Drawing.Point(379, 24);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(245, 312);
            this.dataGridViewResults.TabIndex = 4;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(512, 343);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
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
            this.ClientSize = new System.Drawing.Size(632, 383);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Dmm-Switch Handshaking";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesToFetchNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resolutionNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label scanListLabel;
        private System.Windows.Forms.Label topologyNameLabel;
        private System.Windows.Forms.ComboBox topologyNameComboBox;
        private System.Windows.Forms.TextBox scanListTextBox;
        private System.Windows.Forms.Label switchResourceNameLabel;
        private System.Windows.Forms.ComboBox switchResourceNameComboBox;
        private System.Windows.Forms.Label measurementTypeLabel;
        private System.Windows.Forms.ComboBox measurementModeComboBox;
        private System.Windows.Forms.Label dmmResourceNameLabel;
        private System.Windows.Forms.ComboBox dmmResourceNameComboBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.Label triggerInputLabel;
        private System.Windows.Forms.ComboBox switchTriggerInputComboBox;
        private System.Windows.Forms.Label scanAdvancedOutputLabel;
        private System.Windows.Forms.ComboBox scanAdvancedOutputComboBox;
        private System.Windows.Forms.ComboBox triggerSourceComboBox;
        private System.Windows.Forms.Label triggerSourceLabel;
        private System.Windows.Forms.ComboBox measCompleteDestComboBox;
        private System.Windows.Forms.Label measurementCompleteDestinationLabel;
        private System.Windows.Forms.Label resolutionLabel;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.NumericUpDown rangeNumericUpDown;
        private System.Windows.Forms.NumericUpDown resolutionNumericUpDown;
        private System.Windows.Forms.NumericUpDown samplesToFetchNumericUpDown;
        private System.Windows.Forms.Label samplesToFetchLabel;
        private System.Windows.Forms.Button stopButton;
    }
}

