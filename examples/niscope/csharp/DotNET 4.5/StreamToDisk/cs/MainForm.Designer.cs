namespace NationalInstruments.Examples.StreamToDisk
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
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.readFromFileRadioButton = new System.Windows.Forms.RadioButton();
            this.acquireRadioButton = new System.Windows.Forms.RadioButton();
            this.sampleRateMinLabel = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.recordLengthMinNumeric = new System.Windows.Forms.NumericUpDown();
            this.minSampleRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.verticalRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.recordLengthMinLabel = new System.Windows.Forms.Label();
            this.recordFromFileGroupBox = new System.Windows.Forms.GroupBox();
            this.recordDataGridView = new System.Windows.Forms.DataGridView();
            this.acquiredRecordGroupBox = new System.Windows.Forms.GroupBox();
            this.acquiredDataGridView = new System.Windows.Forms.DataGridView();
            this.filePathGroupBox = new System.Windows.Forms.GroupBox();
            this.buttonsGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recordLengthMinNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minSampleRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).BeginInit();
            this.recordFromFileGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recordDataGridView)).BeginInit();
            this.acquiredRecordGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquiredDataGridView)).BeginInit();
            this.filePathGroupBox.SuspendLayout();
            this.buttonsGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 22);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // readFromFileRadioButton
            // 
            this.readFromFileRadioButton.AutoSize = true;
            this.readFromFileRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.readFromFileRadioButton.Location = new System.Drawing.Point(214, 19);
            this.readFromFileRadioButton.Name = "readFromFileRadioButton";
            this.readFromFileRadioButton.Size = new System.Drawing.Size(170, 18);
            this.readFromFileRadioButton.TabIndex = 1;
            this.readFromFileRadioButton.TabStop = true;
            this.readFromFileRadioButton.Text = "Read record from file and plot";
            this.readFromFileRadioButton.UseVisualStyleBackColor = true;
            // 
            // acquireRadioButton
            // 
            this.acquireRadioButton.AutoSize = true;
            this.acquireRadioButton.Checked = true;
            this.acquireRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquireRadioButton.Location = new System.Drawing.Point(6, 19);
            this.acquireRadioButton.Name = "acquireRadioButton";
            this.acquireRadioButton.Size = new System.Drawing.Size(175, 18);
            this.acquireRadioButton.TabIndex = 0;
            this.acquireRadioButton.TabStop = true;
            this.acquireRadioButton.Text = "Acquire record and save to file";
            this.acquireRadioButton.UseVisualStyleBackColor = true;
            this.acquireRadioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // sampleRateMinLabel
            // 
            this.sampleRateMinLabel.AutoSize = true;
            this.sampleRateMinLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleRateMinLabel.Location = new System.Drawing.Point(6, 102);
            this.sampleRateMinLabel.Name = "sampleRateMinLabel";
            this.sampleRateMinLabel.Size = new System.Drawing.Size(91, 13);
            this.sampleRateMinLabel.TabIndex = 6;
            this.sampleRateMinLabel.Text = "Min Sample Rate:";
            // 
            // browseButton
            // 
            this.browseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.browseButton.Location = new System.Drawing.Point(54, 45);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 1;
            this.browseButton.Text = "&Browse...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.AcceptsReturn = true;
            this.filePathTextBox.Location = new System.Drawing.Point(6, 19);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.Size = new System.Drawing.Size(190, 20);
            this.filePathTextBox.TabIndex = 0;
            this.filePathTextBox.Text = "C:\\waveform\\waveform.txt";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(168, 19);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(112, 19);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(84, 21);
            this.resourceNameComboBox.TabIndex = 1;
            // 
            // channelNameLabel
            // 
            this.channelNameLabel.AutoSize = true;
            this.channelNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelNameLabel.Location = new System.Drawing.Point(6, 50);
            this.channelNameLabel.Name = "channelNameLabel";
            this.channelNameLabel.Size = new System.Drawing.Size(80, 13);
            this.channelNameLabel.TabIndex = 2;
            this.channelNameLabel.Text = "Channel Name:";
            // 
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(112, 46);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(84, 20);
            this.channelNameTextBox.TabIndex = 3;
            this.channelNameTextBox.Text = "0";
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.channelNameLabel);
            this.configurationGroupBox.Controls.Add(this.resourceNameLabel);
            this.configurationGroupBox.Controls.Add(this.channelNameTextBox);
            this.configurationGroupBox.Controls.Add(this.resourceNameComboBox);
            this.configurationGroupBox.Controls.Add(this.recordLengthMinNumeric);
            this.configurationGroupBox.Controls.Add(this.minSampleRateNumeric);
            this.configurationGroupBox.Controls.Add(this.verticalRangeNumeric);
            this.configurationGroupBox.Controls.Add(this.sampleRateMinLabel);
            this.configurationGroupBox.Controls.Add(this.rangeLabel);
            this.configurationGroupBox.Controls.Add(this.recordLengthMinLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 73);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(202, 152);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // recordLengthMinNumeric
            // 
            this.recordLengthMinNumeric.Location = new System.Drawing.Point(112, 124);
            this.recordLengthMinNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.recordLengthMinNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.recordLengthMinNumeric.Name = "recordLengthMinNumeric";
            this.recordLengthMinNumeric.Size = new System.Drawing.Size(84, 20);
            this.recordLengthMinNumeric.TabIndex = 9;
            this.recordLengthMinNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // minSampleRateNumeric
            // 
            this.minSampleRateNumeric.DecimalPlaces = 2;
            this.minSampleRateNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.minSampleRateNumeric.Location = new System.Drawing.Point(112, 98);
            this.minSampleRateNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.minSampleRateNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.minSampleRateNumeric.Name = "minSampleRateNumeric";
            this.minSampleRateNumeric.Size = new System.Drawing.Size(84, 20);
            this.minSampleRateNumeric.TabIndex = 7;
            this.minSampleRateNumeric.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // verticalRangeNumeric
            // 
            this.verticalRangeNumeric.Location = new System.Drawing.Point(112, 72);
            this.verticalRangeNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.verticalRangeNumeric.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.verticalRangeNumeric.Name = "verticalRangeNumeric";
            this.verticalRangeNumeric.Size = new System.Drawing.Size(84, 20);
            this.verticalRangeNumeric.TabIndex = 5;
            this.verticalRangeNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // rangeLabel
            // 
            this.rangeLabel.AutoSize = true;
            this.rangeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rangeLabel.Location = new System.Drawing.Point(6, 76);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.Size = new System.Drawing.Size(42, 13);
            this.rangeLabel.TabIndex = 4;
            this.rangeLabel.Text = "Range:";
            // 
            // recordLengthMinLabel
            // 
            this.recordLengthMinLabel.AutoSize = true;
            this.recordLengthMinLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.recordLengthMinLabel.Location = new System.Drawing.Point(6, 128);
            this.recordLengthMinLabel.Name = "recordLengthMinLabel";
            this.recordLengthMinLabel.Size = new System.Drawing.Size(101, 13);
            this.recordLengthMinLabel.TabIndex = 8;
            this.recordLengthMinLabel.Text = "Min Record Length:";
            // 
            // recordFromFileGroupBox
            // 
            this.recordFromFileGroupBox.Controls.Add(this.recordDataGridView);
            this.recordFromFileGroupBox.Location = new System.Drawing.Point(220, 231);
            this.recordFromFileGroupBox.Name = "recordFromFileGroupBox";
            this.recordFromFileGroupBox.Size = new System.Drawing.Size(202, 289);
            this.recordFromFileGroupBox.TabIndex = 4;
            this.recordFromFileGroupBox.TabStop = false;
            this.recordFromFileGroupBox.Text = "Record From File";
            // 
            // recordDataGridView
            // 
            this.recordDataGridView.AllowUserToAddRows = false;
            this.recordDataGridView.AllowUserToDeleteRows = false;
            this.recordDataGridView.AllowUserToResizeRows = false;
            this.recordDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.recordDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.recordDataGridView.Location = new System.Drawing.Point(6, 19);
            this.recordDataGridView.Name = "recordDataGridView";
            this.recordDataGridView.ReadOnly = true;
            this.recordDataGridView.RowHeadersVisible = false;
            this.recordDataGridView.RowHeadersWidth = 15;
            this.recordDataGridView.RowTemplate.Height = 24;
            this.recordDataGridView.Size = new System.Drawing.Size(190, 264);
            this.recordDataGridView.StandardTab = true;
            this.recordDataGridView.TabIndex = 0;
            // 
            // acquiredRecordGroupBox
            // 
            this.acquiredRecordGroupBox.Controls.Add(this.acquiredDataGridView);
            this.acquiredRecordGroupBox.Location = new System.Drawing.Point(12, 231);
            this.acquiredRecordGroupBox.Name = "acquiredRecordGroupBox";
            this.acquiredRecordGroupBox.Size = new System.Drawing.Size(202, 289);
            this.acquiredRecordGroupBox.TabIndex = 2;
            this.acquiredRecordGroupBox.TabStop = false;
            this.acquiredRecordGroupBox.Text = "Acquired Record";
            // 
            // acquiredDataGridView
            // 
            this.acquiredDataGridView.AllowUserToAddRows = false;
            this.acquiredDataGridView.AllowUserToDeleteRows = false;
            this.acquiredDataGridView.AllowUserToResizeRows = false;
            this.acquiredDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.acquiredDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.acquiredDataGridView.Location = new System.Drawing.Point(6, 19);
            this.acquiredDataGridView.Name = "acquiredDataGridView";
            this.acquiredDataGridView.ReadOnly = true;
            this.acquiredDataGridView.RowHeadersVisible = false;
            this.acquiredDataGridView.RowHeadersWidth = 15;
            this.acquiredDataGridView.RowTemplate.Height = 24;
            this.acquiredDataGridView.Size = new System.Drawing.Size(190, 264);
            this.acquiredDataGridView.StandardTab = true;
            this.acquiredDataGridView.TabIndex = 0;
            // 
            // filePathGroupBox
            // 
            this.filePathGroupBox.Controls.Add(this.filePathTextBox);
            this.filePathGroupBox.Controls.Add(this.browseButton);
            this.filePathGroupBox.Location = new System.Drawing.Point(220, 73);
            this.filePathGroupBox.Name = "filePathGroupBox";
            this.filePathGroupBox.Size = new System.Drawing.Size(202, 74);
            this.filePathGroupBox.TabIndex = 3;
            this.filePathGroupBox.TabStop = false;
            this.filePathGroupBox.Text = "File Path";
            // 
            // buttonsGroupBox
            // 
            this.buttonsGroupBox.Controls.Add(this.startButton);
            this.buttonsGroupBox.Location = new System.Drawing.Point(12, 526);
            this.buttonsGroupBox.Name = "buttonsGroupBox";
            this.buttonsGroupBox.Size = new System.Drawing.Size(410, 55);
            this.buttonsGroupBox.TabIndex = 5;
            this.buttonsGroupBox.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.readFromFileRadioButton);
            this.groupBox1.Controls.Add(this.acquireRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Acquisition Type";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 593);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonsGroupBox);
            this.Controls.Add(this.filePathGroupBox);
            this.Controls.Add(this.recordFromFileGroupBox);
            this.Controls.Add(this.acquiredRecordGroupBox);
            this.Controls.Add(this.configurationGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Stream To Disk";
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recordLengthMinNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minSampleRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).EndInit();
            this.recordFromFileGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.recordDataGridView)).EndInit();
            this.acquiredRecordGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.acquiredDataGridView)).EndInit();
            this.filePathGroupBox.ResumeLayout(false);
            this.filePathGroupBox.PerformLayout();
            this.buttonsGroupBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.RadioButton readFromFileRadioButton;
        private System.Windows.Forms.RadioButton acquireRadioButton;
        private System.Windows.Forms.Label sampleRateMinLabel;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox filePathTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.Label recordLengthMinLabel;
        private System.Windows.Forms.GroupBox recordFromFileGroupBox;
        private System.Windows.Forms.DataGridView recordDataGridView;
        private System.Windows.Forms.GroupBox acquiredRecordGroupBox;
        private System.Windows.Forms.DataGridView acquiredDataGridView;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.GroupBox filePathGroupBox;
        private System.Windows.Forms.GroupBox buttonsGroupBox;
        private System.Windows.Forms.NumericUpDown verticalRangeNumeric;
        private System.Windows.Forms.NumericUpDown minSampleRateNumeric;
        private System.Windows.Forms.NumericUpDown recordLengthMinNumeric;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

