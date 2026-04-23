namespace NationalInstruments.Examples.ThermocoupleMeasurements
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
            this.dmmResourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.startButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.triggerSourceComboBox = new System.Windows.Forms.ComboBox();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.measCompleteDestComboBox = new System.Windows.Forms.ComboBox();
            this.measCompleteDestLabel = new System.Windows.Forms.Label();
            this.scanListLabel = new System.Windows.Forms.Label();
            this.topologyNameLabel = new System.Windows.Forms.Label();
            this.topologyNameComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numOfChannelsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.thermocoupleTypeComboBox = new System.Windows.Forms.ComboBox();
            this.thermocoupleTypeLabel = new System.Windows.Forms.Label();
            this.numberOfChannelsLabel = new System.Windows.Forms.Label();
            this.scanAdvancedOutputComboBox = new System.Windows.Forms.ComboBox();
            this.scanAdvancedOutputLabel = new System.Windows.Forms.Label();
            this.triggerInputComboBox = new System.Windows.Forms.ComboBox();
            this.triggerInputLabel = new System.Windows.Forms.Label();
            this.scanListTextBox = new System.Windows.Forms.TextBox();
            this.switchResourceNameLabel = new System.Windows.Forms.Label();
            this.switchResourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.stopButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOfChannelsNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // measurementTypeLabel
            // 
            this.measurementTypeLabel.AutoSize = true;
            this.measurementTypeLabel.Location = new System.Drawing.Point(6, 81);
            this.measurementTypeLabel.Name = "measurementTypeLabel";
            this.measurementTypeLabel.Size = new System.Drawing.Size(104, 13);
            this.measurementTypeLabel.TabIndex = 5;
            this.measurementTypeLabel.Text = "Measurement Mode:";
            // 
            // measurementModeComboBox
            // 
            this.measurementModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.measurementModeComboBox.FormattingEnabled = true;
            this.measurementModeComboBox.Location = new System.Drawing.Point(9, 97);
            this.measurementModeComboBox.Name = "measurementModeComboBox";
            this.measurementModeComboBox.Size = new System.Drawing.Size(121, 21);
            this.measurementModeComboBox.TabIndex = 1;
            // 
            // dmmResourceNameLabel
            // 
            this.dmmResourceNameLabel.AutoSize = true;
            this.dmmResourceNameLabel.Location = new System.Drawing.Point(6, 28);
            this.dmmResourceNameLabel.Name = "dmmResourceNameLabel";
            this.dmmResourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.dmmResourceNameLabel.TabIndex = 4;
            this.dmmResourceNameLabel.Text = "Resource Name:";
            // 
            // dmmResourceNameComboBox
            // 
            this.dmmResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dmmResourceNameComboBox.FormattingEnabled = true;
            this.dmmResourceNameComboBox.Location = new System.Drawing.Point(9, 44);
            this.dmmResourceNameComboBox.Name = "dmmResourceNameComboBox";
            this.dmmResourceNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.dmmResourceNameComboBox.TabIndex = 0;
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewResults.Location = new System.Drawing.Point(335, 35);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.Size = new System.Drawing.Size(255, 251);
            this.dataGridViewResults.TabIndex = 4;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(372, 320);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.triggerSourceComboBox);
            this.groupBox2.Controls.Add(this.triggerSourceLabel);
            this.groupBox2.Controls.Add(this.measCompleteDestComboBox);
            this.groupBox2.Controls.Add(this.measCompleteDestLabel);
            this.groupBox2.Controls.Add(this.measurementTypeLabel);
            this.groupBox2.Controls.Add(this.measurementModeComboBox);
            this.groupBox2.Controls.Add(this.dmmResourceNameLabel);
            this.groupBox2.Controls.Add(this.dmmResourceNameComboBox);
            this.groupBox2.Location = new System.Drawing.Point(167, 23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(152, 398);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dmm";
            // 
            // triggerSourceComboBox
            // 
            this.triggerSourceComboBox.FormattingEnabled = true;
            this.triggerSourceComboBox.Location = new System.Drawing.Point(9, 226);
            this.triggerSourceComboBox.Name = "triggerSourceComboBox";
            this.triggerSourceComboBox.Size = new System.Drawing.Size(121, 21);
            this.triggerSourceComboBox.TabIndex = 3;
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.AutoSize = true;
            this.triggerSourceLabel.Location = new System.Drawing.Point(6, 206);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(77, 13);
            this.triggerSourceLabel.TabIndex = 7;
            this.triggerSourceLabel.Text = "Trigger Source";
            // 
            // measCompleteDestComboBox
            // 
            this.measCompleteDestComboBox.FormattingEnabled = true;
            this.measCompleteDestComboBox.Location = new System.Drawing.Point(9, 162);
            this.measCompleteDestComboBox.Name = "measCompleteDestComboBox";
            this.measCompleteDestComboBox.Size = new System.Drawing.Size(121, 21);
            this.measCompleteDestComboBox.TabIndex = 2;
            // 
            // measCompleteDestLabel
            // 
            this.measCompleteDestLabel.AutoSize = true;
            this.measCompleteDestLabel.Location = new System.Drawing.Point(6, 133);
            this.measCompleteDestLabel.Name = "measCompleteDestLabel";
            this.measCompleteDestLabel.Size = new System.Drawing.Size(121, 26);
            this.measCompleteDestLabel.TabIndex = 6;
            this.measCompleteDestLabel.Text = "Measurement Complete \r\nDestination";
            // 
            // scanListLabel
            // 
            this.scanListLabel.AutoSize = true;
            this.scanListLabel.Location = new System.Drawing.Point(6, 133);
            this.scanListLabel.Name = "scanListLabel";
            this.scanListLabel.Size = new System.Drawing.Size(51, 13);
            this.scanListLabel.TabIndex = 9;
            this.scanListLabel.Text = "Scan List";
            // 
            // topologyNameLabel
            // 
            this.topologyNameLabel.AutoSize = true;
            this.topologyNameLabel.Location = new System.Drawing.Point(6, 81);
            this.topologyNameLabel.Name = "topologyNameLabel";
            this.topologyNameLabel.Size = new System.Drawing.Size(82, 13);
            this.topologyNameLabel.TabIndex = 8;
            this.topologyNameLabel.Text = "Topology Name";
            // 
            // topologyNameComboBox
            // 
            this.topologyNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.topologyNameComboBox.FormattingEnabled = true;
            this.topologyNameComboBox.Location = new System.Drawing.Point(6, 97);
            this.topologyNameComboBox.Name = "topologyNameComboBox";
            this.topologyNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.topologyNameComboBox.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numOfChannelsNumericUpDown);
            this.groupBox1.Controls.Add(this.thermocoupleTypeComboBox);
            this.groupBox1.Controls.Add(this.thermocoupleTypeLabel);
            this.groupBox1.Controls.Add(this.numberOfChannelsLabel);
            this.groupBox1.Controls.Add(this.scanAdvancedOutputComboBox);
            this.groupBox1.Controls.Add(this.scanAdvancedOutputLabel);
            this.groupBox1.Controls.Add(this.triggerInputComboBox);
            this.groupBox1.Controls.Add(this.triggerInputLabel);
            this.groupBox1.Controls.Add(this.scanListLabel);
            this.groupBox1.Controls.Add(this.topologyNameLabel);
            this.groupBox1.Controls.Add(this.topologyNameComboBox);
            this.groupBox1.Controls.Add(this.scanListTextBox);
            this.groupBox1.Controls.Add(this.switchResourceNameLabel);
            this.groupBox1.Controls.Add(this.switchResourceNameComboBox);
            this.groupBox1.Location = new System.Drawing.Point(9, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(152, 398);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Switch";
            // 
            // numOfChannelsNumericUpDown
            // 
            this.numOfChannelsNumericUpDown.Location = new System.Drawing.Point(6, 304);
            this.numOfChannelsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numOfChannelsNumericUpDown.Name = "numOfChannelsNumericUpDown";
            this.numOfChannelsNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.numOfChannelsNumericUpDown.TabIndex = 5;
            this.numOfChannelsNumericUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // thermocoupleTypeComboBox
            // 
            this.thermocoupleTypeComboBox.FormattingEnabled = true;
            this.thermocoupleTypeComboBox.Location = new System.Drawing.Point(6, 359);
            this.thermocoupleTypeComboBox.Name = "thermocoupleTypeComboBox";
            this.thermocoupleTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.thermocoupleTypeComboBox.TabIndex = 6;
            // 
            // thermocoupleTypeLabel
            // 
            this.thermocoupleTypeLabel.AutoSize = true;
            this.thermocoupleTypeLabel.Location = new System.Drawing.Point(6, 343);
            this.thermocoupleTypeLabel.Name = "thermocoupleTypeLabel";
            this.thermocoupleTypeLabel.Size = new System.Drawing.Size(102, 13);
            this.thermocoupleTypeLabel.TabIndex = 13;
            this.thermocoupleTypeLabel.Text = "Thermocouple Type";
            // 
            // numberOfChannelsLabel
            // 
            this.numberOfChannelsLabel.AutoSize = true;
            this.numberOfChannelsLabel.Location = new System.Drawing.Point(6, 288);
            this.numberOfChannelsLabel.Name = "numberOfChannelsLabel";
            this.numberOfChannelsLabel.Size = new System.Drawing.Size(105, 13);
            this.numberOfChannelsLabel.TabIndex = 12;
            this.numberOfChannelsLabel.Text = "Number Of Channels";
            // 
            // scanAdvancedOutputComboBox
            // 
            this.scanAdvancedOutputComboBox.FormattingEnabled = true;
            this.scanAdvancedOutputComboBox.Location = new System.Drawing.Point(6, 250);
            this.scanAdvancedOutputComboBox.Name = "scanAdvancedOutputComboBox";
            this.scanAdvancedOutputComboBox.Size = new System.Drawing.Size(121, 21);
            this.scanAdvancedOutputComboBox.TabIndex = 4;
            // 
            // scanAdvancedOutputLabel
            // 
            this.scanAdvancedOutputLabel.AutoSize = true;
            this.scanAdvancedOutputLabel.Location = new System.Drawing.Point(6, 234);
            this.scanAdvancedOutputLabel.Name = "scanAdvancedOutputLabel";
            this.scanAdvancedOutputLabel.Size = new System.Drawing.Size(119, 13);
            this.scanAdvancedOutputLabel.TabIndex = 11;
            this.scanAdvancedOutputLabel.Text = "Scan Advanced Output";
            // 
            // triggerInputComboBox
            // 
            this.triggerInputComboBox.FormattingEnabled = true;
            this.triggerInputComboBox.Location = new System.Drawing.Point(6, 198);
            this.triggerInputComboBox.Name = "triggerInputComboBox";
            this.triggerInputComboBox.Size = new System.Drawing.Size(121, 21);
            this.triggerInputComboBox.TabIndex = 3;
            // 
            // triggerInputLabel
            // 
            this.triggerInputLabel.AutoSize = true;
            this.triggerInputLabel.Location = new System.Drawing.Point(6, 182);
            this.triggerInputLabel.Name = "triggerInputLabel";
            this.triggerInputLabel.Size = new System.Drawing.Size(67, 13);
            this.triggerInputLabel.TabIndex = 10;
            this.triggerInputLabel.Text = "Trigger Input";
            // 
            // scanListTextBox
            // 
            this.scanListTextBox.Location = new System.Drawing.Point(6, 149);
            this.scanListTextBox.Name = "scanListTextBox";
            this.scanListTextBox.Size = new System.Drawing.Size(121, 20);
            this.scanListTextBox.TabIndex = 2;
            this.scanListTextBox.Text = "ch0:2->com0;";
            // 
            // switchResourceNameLabel
            // 
            this.switchResourceNameLabel.AutoSize = true;
            this.switchResourceNameLabel.Location = new System.Drawing.Point(6, 28);
            this.switchResourceNameLabel.Name = "switchResourceNameLabel";
            this.switchResourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.switchResourceNameLabel.TabIndex = 7;
            this.switchResourceNameLabel.Text = "Resource Name";
            // 
            // switchResourceNameComboBox
            // 
            this.switchResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.switchResourceNameComboBox.FormattingEnabled = true;
            this.switchResourceNameComboBox.Location = new System.Drawing.Point(6, 44);
            this.switchResourceNameComboBox.Name = "switchResourceNameComboBox";
            this.switchResourceNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.switchResourceNameComboBox.TabIndex = 0;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(492, 320);
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
            this.ClientSize = new System.Drawing.Size(625, 478);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Thermocouple Measurements";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOfChannelsNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label measurementTypeLabel;
        private System.Windows.Forms.ComboBox measurementModeComboBox;
        private System.Windows.Forms.Label dmmResourceNameLabel;
        private System.Windows.Forms.ComboBox dmmResourceNameComboBox;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label scanListLabel;
        private System.Windows.Forms.Label topologyNameLabel;
        private System.Windows.Forms.ComboBox topologyNameComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox scanListTextBox;
        private System.Windows.Forms.Label switchResourceNameLabel;
        private System.Windows.Forms.ComboBox switchResourceNameComboBox;
        private System.Windows.Forms.Label triggerInputLabel;
        private System.Windows.Forms.ComboBox triggerInputComboBox;
        private System.Windows.Forms.ComboBox scanAdvancedOutputComboBox;
        private System.Windows.Forms.Label scanAdvancedOutputLabel;
        private System.Windows.Forms.Label numberOfChannelsLabel;
        private System.Windows.Forms.ComboBox thermocoupleTypeComboBox;
        private System.Windows.Forms.Label thermocoupleTypeLabel;
        private System.Windows.Forms.ComboBox triggerSourceComboBox;
        private System.Windows.Forms.Label triggerSourceLabel;
        private System.Windows.Forms.ComboBox measCompleteDestComboBox;
        private System.Windows.Forms.Label measCompleteDestLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.NumericUpDown numOfChannelsNumericUpDown;
    }
}

