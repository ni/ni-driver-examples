namespace NationalInstruments.Examples.MultipleRecordFetchMoreThanAvailableMemory
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.verticalRangeLabel = new System.Windows.Forms.Label();
            this.minSampleRateLabel = new System.Windows.Forms.Label();
            this.minRecordLengthLabel = new System.Windows.Forms.Label();
            this.numOfRecordLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.verticalRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.minSampleRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.minRecordLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.numOfRecordNumeric = new System.Windows.Forms.NumericUpDown();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.numRecordsFetchedTextBox = new System.Windows.Forms.TextBox();
            this.numRecordsAcquiredTextBox = new System.Windows.Forms.TextBox();
            this.acquireButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.attributesGroupBox = new System.Windows.Forms.GroupBox();
            this.dataRecordsGroupBox = new System.Windows.Forms.GroupBox();
            this.allowMoreRecsThanAvaiMemCheckBox = new System.Windows.Forms.CheckBox();
            this.NumOfRecordsAcquiredlabel = new System.Windows.Forms.Label();
            this.numOfRecFetchedLabel = new System.Windows.Forms.Label();
            this.sampledDataGroupBox = new System.Windows.Forms.GroupBox();
            this.sampledDataGridView = new System.Windows.Forms.DataGridView();
            this.messageGroupBox = new System.Windows.Forms.GroupBox();
            this.messageTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minSampleRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minRecordLengthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOfRecordNumeric)).BeginInit();
            this.generalGroupBox.SuspendLayout();
            this.attributesGroupBox.SuspendLayout();
            this.dataRecordsGroupBox.SuspendLayout();
            this.sampledDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sampledDataGridView)).BeginInit();
            this.messageGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelNameLabel
            // 
            this.channelNameLabel.AutoSize = true;
            this.channelNameLabel.Location = new System.Drawing.Point(6, 50);
            this.channelNameLabel.Name = "channelNameLabel";
            this.channelNameLabel.Size = new System.Drawing.Size(80, 13);
            this.channelNameLabel.TabIndex = 2;
            this.channelNameLabel.Text = "Channel Name:";
            // 
            // verticalRangeLabel
            // 
            this.verticalRangeLabel.AutoSize = true;
            this.verticalRangeLabel.Location = new System.Drawing.Point(6, 23);
            this.verticalRangeLabel.Name = "verticalRangeLabel";
            this.verticalRangeLabel.Size = new System.Drawing.Size(80, 13);
            this.verticalRangeLabel.TabIndex = 0;
            this.verticalRangeLabel.Text = "Vertical Range:";
            // 
            // minSampleRateLabel
            // 
            this.minSampleRateLabel.AutoSize = true;
            this.minSampleRateLabel.Location = new System.Drawing.Point(6, 49);
            this.minSampleRateLabel.Name = "minSampleRateLabel";
            this.minSampleRateLabel.Size = new System.Drawing.Size(91, 13);
            this.minSampleRateLabel.TabIndex = 2;
            this.minSampleRateLabel.Text = "Min Sample Rate:";
            // 
            // minRecordLengthLabel
            // 
            this.minRecordLengthLabel.AutoSize = true;
            this.minRecordLengthLabel.Location = new System.Drawing.Point(6, 75);
            this.minRecordLengthLabel.Name = "minRecordLengthLabel";
            this.minRecordLengthLabel.Size = new System.Drawing.Size(101, 13);
            this.minRecordLengthLabel.TabIndex = 4;
            this.minRecordLengthLabel.Text = "Min Record Length:";
            // 
            // numOfRecordLabel
            // 
            this.numOfRecordLabel.AutoSize = true;
            this.numOfRecordLabel.Location = new System.Drawing.Point(6, 46);
            this.numOfRecordLabel.Name = "numOfRecordLabel";
            this.numOfRecordLabel.Size = new System.Drawing.Size(102, 13);
            this.numOfRecordLabel.TabIndex = 4;
            this.numOfRecordLabel.Text = "Number of Records:";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 23);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // verticalRangeNumeric
            // 
            this.verticalRangeNumeric.DecimalPlaces = 2;
            this.verticalRangeNumeric.Location = new System.Drawing.Point(144, 19);
            this.verticalRangeNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.verticalRangeNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.verticalRangeNumeric.Name = "verticalRangeNumeric";
            this.verticalRangeNumeric.Size = new System.Drawing.Size(101, 20);
            this.verticalRangeNumeric.TabIndex = 1;
            this.verticalRangeNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // minSampleRateNumeric
            // 
            this.minSampleRateNumeric.DecimalPlaces = 2;
            this.minSampleRateNumeric.Location = new System.Drawing.Point(144, 45);
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
            this.minSampleRateNumeric.Size = new System.Drawing.Size(101, 20);
            this.minSampleRateNumeric.TabIndex = 3;
            this.minSampleRateNumeric.Value = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            // 
            // minRecordLengthNumeric
            // 
            this.minRecordLengthNumeric.Location = new System.Drawing.Point(144, 71);
            this.minRecordLengthNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.minRecordLengthNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.minRecordLengthNumeric.Name = "minRecordLengthNumeric";
            this.minRecordLengthNumeric.Size = new System.Drawing.Size(101, 20);
            this.minRecordLengthNumeric.TabIndex = 5;
            this.minRecordLengthNumeric.Value = new decimal(new int[] {
            8192,
            0,
            0,
            0});
            // 
            // numOfRecordNumeric
            // 
            this.numOfRecordNumeric.Location = new System.Drawing.Point(157, 42);
            this.numOfRecordNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numOfRecordNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.numOfRecordNumeric.Name = "numOfRecordNumeric";
            this.numOfRecordNumeric.Size = new System.Drawing.Size(88, 20);
            this.numOfRecordNumeric.TabIndex = 1;
            this.numOfRecordNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(144, 46);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(101, 20);
            this.channelNameTextBox.TabIndex = 3;
            this.channelNameTextBox.Text = "0";
            // 
            // numRecordsFetchedTextBox
            // 
            this.numRecordsFetchedTextBox.Location = new System.Drawing.Point(157, 68);
            this.numRecordsFetchedTextBox.Name = "numRecordsFetchedTextBox";
            this.numRecordsFetchedTextBox.ReadOnly = true;
            this.numRecordsFetchedTextBox.Size = new System.Drawing.Size(88, 20);
            this.numRecordsFetchedTextBox.TabIndex = 2;
            this.numRecordsFetchedTextBox.Text = "0";
            // 
            // numRecordsAcquiredTextBox
            // 
            this.numRecordsAcquiredTextBox.Location = new System.Drawing.Point(157, 94);
            this.numRecordsAcquiredTextBox.Name = "numRecordsAcquiredTextBox";
            this.numRecordsAcquiredTextBox.ReadOnly = true;
            this.numRecordsAcquiredTextBox.Size = new System.Drawing.Size(88, 20);
            this.numRecordsAcquiredTextBox.TabIndex = 3;
            this.numRecordsAcquiredTextBox.Text = "0";
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(52, 19);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(133, 19);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "&Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.resourceNameComboBox);
            this.generalGroupBox.Controls.Add(this.channelNameTextBox);
            this.generalGroupBox.Controls.Add(this.channelNameLabel);
            this.generalGroupBox.Controls.Add(this.resourceNameLabel);
            this.generalGroupBox.Location = new System.Drawing.Point(12, 12);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(251, 74);
            this.generalGroupBox.TabIndex = 0;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(144, 19);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(101, 21);
            this.resourceNameComboBox.TabIndex = 1;
            // 
            // attributesGroupBox
            // 
            this.attributesGroupBox.Controls.Add(this.verticalRangeNumeric);
            this.attributesGroupBox.Controls.Add(this.minSampleRateNumeric);
            this.attributesGroupBox.Controls.Add(this.minRecordLengthNumeric);
            this.attributesGroupBox.Controls.Add(this.verticalRangeLabel);
            this.attributesGroupBox.Controls.Add(this.minSampleRateLabel);
            this.attributesGroupBox.Controls.Add(this.minRecordLengthLabel);
            this.attributesGroupBox.Location = new System.Drawing.Point(12, 97);
            this.attributesGroupBox.Name = "attributesGroupBox";
            this.attributesGroupBox.Size = new System.Drawing.Size(251, 98);
            this.attributesGroupBox.TabIndex = 1;
            this.attributesGroupBox.TabStop = false;
            this.attributesGroupBox.Text = "Parameters";
            // 
            // dataRecordsGroupBox
            // 
            this.dataRecordsGroupBox.Controls.Add(this.allowMoreRecsThanAvaiMemCheckBox);
            this.dataRecordsGroupBox.Controls.Add(this.NumOfRecordsAcquiredlabel);
            this.dataRecordsGroupBox.Controls.Add(this.numOfRecFetchedLabel);
            this.dataRecordsGroupBox.Controls.Add(this.numRecordsFetchedTextBox);
            this.dataRecordsGroupBox.Controls.Add(this.numRecordsAcquiredTextBox);
            this.dataRecordsGroupBox.Controls.Add(this.numOfRecordNumeric);
            this.dataRecordsGroupBox.Controls.Add(this.numOfRecordLabel);
            this.dataRecordsGroupBox.Location = new System.Drawing.Point(12, 205);
            this.dataRecordsGroupBox.Name = "dataRecordsGroupBox";
            this.dataRecordsGroupBox.Size = new System.Drawing.Size(251, 122);
            this.dataRecordsGroupBox.TabIndex = 2;
            this.dataRecordsGroupBox.TabStop = false;
            this.dataRecordsGroupBox.Text = "Data Records";
            // 
            // allowMoreRecsThanAvaiMemCheckBox
            // 
            this.allowMoreRecsThanAvaiMemCheckBox.AutoSize = true;
            this.allowMoreRecsThanAvaiMemCheckBox.Location = new System.Drawing.Point(6, 19);
            this.allowMoreRecsThanAvaiMemCheckBox.Name = "allowMoreRecsThanAvaiMemCheckBox";
            this.allowMoreRecsThanAvaiMemCheckBox.Size = new System.Drawing.Size(229, 17);
            this.allowMoreRecsThanAvaiMemCheckBox.TabIndex = 0;
            this.allowMoreRecsThanAvaiMemCheckBox.Text = "Allow more records than available memory?";
            this.allowMoreRecsThanAvaiMemCheckBox.UseVisualStyleBackColor = true;
            // 
            // NumOfRecordsAcquiredlabel
            // 
            this.NumOfRecordsAcquiredlabel.AutoSize = true;
            this.NumOfRecordsAcquiredlabel.Location = new System.Drawing.Point(6, 98);
            this.NumOfRecordsAcquiredlabel.Name = "NumOfRecordsAcquiredlabel";
            this.NumOfRecordsAcquiredlabel.Size = new System.Drawing.Size(147, 13);
            this.NumOfRecordsAcquiredlabel.TabIndex = 8;
            this.NumOfRecordsAcquiredlabel.Text = "Number of Records Acquired:";
            // 
            // numOfRecFetchedLabel
            // 
            this.numOfRecFetchedLabel.AutoSize = true;
            this.numOfRecFetchedLabel.Location = new System.Drawing.Point(6, 72);
            this.numOfRecFetchedLabel.Name = "numOfRecFetchedLabel";
            this.numOfRecFetchedLabel.Size = new System.Drawing.Size(144, 13);
            this.numOfRecFetchedLabel.TabIndex = 6;
            this.numOfRecFetchedLabel.Text = "Number of Records Fetched:";
            // 
            // sampledDataGroupBox
            // 
            this.sampledDataGroupBox.Controls.Add(this.sampledDataGridView);
            this.sampledDataGroupBox.Location = new System.Drawing.Point(269, 12);
            this.sampledDataGroupBox.Name = "sampledDataGroupBox";
            this.sampledDataGroupBox.Size = new System.Drawing.Size(202, 443);
            this.sampledDataGroupBox.TabIndex = 5;
            this.sampledDataGroupBox.TabStop = false;
            this.sampledDataGroupBox.Text = "Sampled Data";
            // 
            // sampledDataGridView
            // 
            this.sampledDataGridView.AllowUserToAddRows = false;
            this.sampledDataGridView.AllowUserToDeleteRows = false;
            this.sampledDataGridView.AllowUserToResizeRows = false;
            this.sampledDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sampledDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.sampledDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.sampledDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.sampledDataGridView.Location = new System.Drawing.Point(6, 19);
            this.sampledDataGridView.Name = "sampledDataGridView";
            this.sampledDataGridView.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sampledDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.sampledDataGridView.RowHeadersVisible = false;
            this.sampledDataGridView.RowHeadersWidth = 15;
            this.sampledDataGridView.RowTemplate.Height = 24;
            this.sampledDataGridView.Size = new System.Drawing.Size(190, 418);
            this.sampledDataGridView.StandardTab = true;
            this.sampledDataGridView.TabIndex = 0;
            // 
            // messageGroupBox
            // 
            this.messageGroupBox.Controls.Add(this.messageTextBox);
            this.messageGroupBox.Location = new System.Drawing.Point(12, 333);
            this.messageGroupBox.Name = "messageGroupBox";
            this.messageGroupBox.Size = new System.Drawing.Size(251, 62);
            this.messageGroupBox.TabIndex = 3;
            this.messageGroupBox.TabStop = false;
            this.messageGroupBox.Text = "Message";
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(9, 19);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.Size = new System.Drawing.Size(236, 36);
            this.messageTextBox.TabIndex = 0;
            this.messageTextBox.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.acquireButton);
            this.groupBox1.Controls.Add(this.stopButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 401);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(251, 54);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 468);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.messageGroupBox);
            this.Controls.Add(this.sampledDataGroupBox);
            this.Controls.Add(this.generalGroupBox);
            this.Controls.Add(this.attributesGroupBox);
            this.Controls.Add(this.dataRecordsGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Multiple Record Fetch More Than Available Memory";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minSampleRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minRecordLengthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOfRecordNumeric)).EndInit();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.attributesGroupBox.ResumeLayout(false);
            this.attributesGroupBox.PerformLayout();
            this.dataRecordsGroupBox.ResumeLayout(false);
            this.dataRecordsGroupBox.PerformLayout();
            this.sampledDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sampledDataGridView)).EndInit();
            this.messageGroupBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label verticalRangeLabel;
        private System.Windows.Forms.Label minSampleRateLabel;
        private System.Windows.Forms.Label minRecordLengthLabel;
        private System.Windows.Forms.Label numOfRecordLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.NumericUpDown verticalRangeNumeric;
        private System.Windows.Forms.NumericUpDown minSampleRateNumeric;
        private System.Windows.Forms.NumericUpDown minRecordLengthNumeric;
        private System.Windows.Forms.NumericUpDown numOfRecordNumeric;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.TextBox numRecordsFetchedTextBox;
        private System.Windows.Forms.TextBox numRecordsAcquiredTextBox;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.GroupBox attributesGroupBox;
        private System.Windows.Forms.GroupBox dataRecordsGroupBox;
        private System.Windows.Forms.Label NumOfRecordsAcquiredlabel;
        private System.Windows.Forms.Label numOfRecFetchedLabel;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.CheckBox allowMoreRecsThanAvaiMemCheckBox;
        private System.Windows.Forms.GroupBox sampledDataGroupBox;
        private System.Windows.Forms.DataGridView sampledDataGridView;
        private System.Windows.Forms.GroupBox messageGroupBox;
        private System.Windows.Forms.RichTextBox messageTextBox;
        private System.Windows.Forms.GroupBox groupBox1;


    }
}
