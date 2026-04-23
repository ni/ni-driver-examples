namespace NationalInstruments.Examples.BinaryAcquisition
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
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.offsetGroupBox = new System.Windows.Forms.GroupBox();
            this.scaleOffsetTextBox = new System.Windows.Forms.TextBox();
            this.scaleGainFactorTextBox = new System.Windows.Forms.TextBox();
            this.triggerGroupBox = new System.Windows.Forms.GroupBox();
            this.triggerTypeComboBox = new System.Windows.Forms.ComboBox();
            this.triggerTypeLabel = new System.Windows.Forms.Label();
            this.verticalGroupBox = new System.Windows.Forms.GroupBox();
            this.verticalRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.verticalOffsetNumeric = new System.Windows.Forms.NumericUpDown();
            this.offsetLabel = new System.Windows.Forms.Label();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.horizontalGroupBox = new System.Windows.Forms.GroupBox();
            this.minRecordLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.minSampleRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.recordLengthMinLabel = new System.Windows.Forms.Label();
            this.sampleRateMinLabel = new System.Windows.Forms.Label();
            this.sampleDataGroupBox = new System.Windows.Forms.GroupBox();
            this.scaledDataGridView = new System.Windows.Forms.DataGridView();
            this.binaryDataGroupBox = new System.Windows.Forms.GroupBox();
            this.binaryDataGridView = new System.Windows.Forms.DataGridView();
            this.stopButton = new System.Windows.Forms.Button();
            this.acquireButton = new System.Windows.Forms.Button();
            this.messageGroupBox = new System.Windows.Forms.GroupBox();
            this.messageTextBox = new System.Windows.Forms.RichTextBox();
            this.buttonsGroupBox = new System.Windows.Forms.GroupBox();
            this.binaryDataSizeComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gainFactorGroupBox = new System.Windows.Forms.GroupBox();
            this.generalGroupBox.SuspendLayout();
            this.offsetGroupBox.SuspendLayout();
            this.triggerGroupBox.SuspendLayout();
            this.verticalGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalOffsetNumeric)).BeginInit();
            this.horizontalGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minRecordLengthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minSampleRateNumeric)).BeginInit();
            this.sampleDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaledDataGridView)).BeginInit();
            this.binaryDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.binaryDataGridView)).BeginInit();
            this.messageGroupBox.SuspendLayout();
            this.buttonsGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gainFactorGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.channelNameTextBox);
            this.generalGroupBox.Controls.Add(this.resourceNameComboBox);
            this.generalGroupBox.Controls.Add(this.channelNameLabel);
            this.generalGroupBox.Controls.Add(this.resourceNameLabel);
            this.generalGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.generalGroupBox.Location = new System.Drawing.Point(12, 12);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(248, 72);
            this.generalGroupBox.TabIndex = 0;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            // 
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(132, 46);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.channelNameTextBox.TabIndex = 1;
            this.channelNameTextBox.Text = "0";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(132, 19);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(100, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // channelNameLabel
            // 
            this.channelNameLabel.AutoSize = true;
            this.channelNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelNameLabel.Location = new System.Drawing.Point(6, 50);
            this.channelNameLabel.Name = "channelNameLabel";
            this.channelNameLabel.Size = new System.Drawing.Size(80, 13);
            this.channelNameLabel.TabIndex = 3;
            this.channelNameLabel.Text = "Channel Name:";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 23);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 1;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // offsetGroupBox
            // 
            this.offsetGroupBox.Controls.Add(this.scaleOffsetTextBox);
            this.offsetGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.offsetGroupBox.Location = new System.Drawing.Point(266, 395);
            this.offsetGroupBox.Name = "offsetGroupBox";
            this.offsetGroupBox.Size = new System.Drawing.Size(410, 60);
            this.offsetGroupBox.TabIndex = 9;
            this.offsetGroupBox.TabStop = false;
            this.offsetGroupBox.Text = "Offset";
            // 
            // scaleOffsetTextBox
            // 
            this.scaleOffsetTextBox.Location = new System.Drawing.Point(6, 20);
            this.scaleOffsetTextBox.Name = "scaleOffsetTextBox";
            this.scaleOffsetTextBox.ReadOnly = true;
            this.scaleOffsetTextBox.Size = new System.Drawing.Size(398, 20);
            this.scaleOffsetTextBox.TabIndex = 0;
            // 
            // scaleGainFactorTextBox
            // 
            this.scaleGainFactorTextBox.Location = new System.Drawing.Point(6, 20);
            this.scaleGainFactorTextBox.Name = "scaleGainFactorTextBox";
            this.scaleGainFactorTextBox.ReadOnly = true;
            this.scaleGainFactorTextBox.Size = new System.Drawing.Size(398, 20);
            this.scaleGainFactorTextBox.TabIndex = 0;
            // 
            // triggerGroupBox
            // 
            this.triggerGroupBox.Controls.Add(this.triggerTypeComboBox);
            this.triggerGroupBox.Controls.Add(this.triggerTypeLabel);
            this.triggerGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerGroupBox.Location = new System.Drawing.Point(12, 95);
            this.triggerGroupBox.Name = "triggerGroupBox";
            this.triggerGroupBox.Size = new System.Drawing.Size(248, 47);
            this.triggerGroupBox.TabIndex = 1;
            this.triggerGroupBox.TabStop = false;
            this.triggerGroupBox.Text = "Trigger";
            // 
            // triggerTypeComboBox
            // 
            this.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerTypeComboBox.FormattingEnabled = true;
            this.triggerTypeComboBox.Location = new System.Drawing.Point(142, 19);
            this.triggerTypeComboBox.Name = "triggerTypeComboBox";
            this.triggerTypeComboBox.Size = new System.Drawing.Size(100, 21);
            this.triggerTypeComboBox.TabIndex = 0;
            // 
            // triggerTypeLabel
            // 
            this.triggerTypeLabel.AutoSize = true;
            this.triggerTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.triggerTypeLabel.Location = new System.Drawing.Point(6, 23);
            this.triggerTypeLabel.Name = "triggerTypeLabel";
            this.triggerTypeLabel.Size = new System.Drawing.Size(70, 13);
            this.triggerTypeLabel.TabIndex = 0;
            this.triggerTypeLabel.Text = "Trigger Type:";
            // 
            // verticalGroupBox
            // 
            this.verticalGroupBox.Controls.Add(this.verticalRangeNumeric);
            this.verticalGroupBox.Controls.Add(this.verticalOffsetNumeric);
            this.verticalGroupBox.Controls.Add(this.offsetLabel);
            this.verticalGroupBox.Controls.Add(this.rangeLabel);
            this.verticalGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.verticalGroupBox.Location = new System.Drawing.Point(12, 153);
            this.verticalGroupBox.Name = "verticalGroupBox";
            this.verticalGroupBox.Size = new System.Drawing.Size(248, 77);
            this.verticalGroupBox.TabIndex = 2;
            this.verticalGroupBox.TabStop = false;
            this.verticalGroupBox.Text = "Vertical";
            // 
            // verticalRangeNumeric
            // 
            this.verticalRangeNumeric.DecimalPlaces = 2;
            this.verticalRangeNumeric.Location = new System.Drawing.Point(142, 20);
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
            this.verticalRangeNumeric.Size = new System.Drawing.Size(100, 20);
            this.verticalRangeNumeric.TabIndex = 0;
            this.verticalRangeNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // verticalOffsetNumeric
            // 
            this.verticalOffsetNumeric.DecimalPlaces = 4;
            this.verticalOffsetNumeric.Location = new System.Drawing.Point(142, 48);
            this.verticalOffsetNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.verticalOffsetNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.verticalOffsetNumeric.Name = "verticalOffsetNumeric";
            this.verticalOffsetNumeric.Size = new System.Drawing.Size(100, 20);
            this.verticalOffsetNumeric.TabIndex = 1;
            // 
            // offsetLabel
            // 
            this.offsetLabel.AutoSize = true;
            this.offsetLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.offsetLabel.Location = new System.Drawing.Point(6, 52);
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(38, 13);
            this.offsetLabel.TabIndex = 4;
            this.offsetLabel.Text = "Offset:";
            // 
            // rangeLabel
            // 
            this.rangeLabel.AutoSize = true;
            this.rangeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rangeLabel.Location = new System.Drawing.Point(6, 24);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.Size = new System.Drawing.Size(42, 13);
            this.rangeLabel.TabIndex = 4;
            this.rangeLabel.Text = "Range:";
            // 
            // horizontalGroupBox
            // 
            this.horizontalGroupBox.Controls.Add(this.minRecordLengthNumeric);
            this.horizontalGroupBox.Controls.Add(this.minSampleRateNumeric);
            this.horizontalGroupBox.Controls.Add(this.recordLengthMinLabel);
            this.horizontalGroupBox.Controls.Add(this.sampleRateMinLabel);
            this.horizontalGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.horizontalGroupBox.Location = new System.Drawing.Point(12, 241);
            this.horizontalGroupBox.Name = "horizontalGroupBox";
            this.horizontalGroupBox.Size = new System.Drawing.Size(248, 72);
            this.horizontalGroupBox.TabIndex = 3;
            this.horizontalGroupBox.TabStop = false;
            this.horizontalGroupBox.Text = "Horizontal";
            // 
            // minRecordLengthNumeric
            // 
            this.minRecordLengthNumeric.Location = new System.Drawing.Point(142, 45);
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
            this.minRecordLengthNumeric.Size = new System.Drawing.Size(100, 20);
            this.minRecordLengthNumeric.TabIndex = 1;
            this.minRecordLengthNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // minSampleRateNumeric
            // 
            this.minSampleRateNumeric.DecimalPlaces = 2;
            this.minSampleRateNumeric.Location = new System.Drawing.Point(142, 19);
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
            this.minSampleRateNumeric.Size = new System.Drawing.Size(100, 20);
            this.minSampleRateNumeric.TabIndex = 0;
            this.minSampleRateNumeric.Value = new decimal(new int[] {
            20000000,
            0,
            0,
            0});
            // 
            // recordLengthMinLabel
            // 
            this.recordLengthMinLabel.AutoSize = true;
            this.recordLengthMinLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.recordLengthMinLabel.Location = new System.Drawing.Point(6, 49);
            this.recordLengthMinLabel.Name = "recordLengthMinLabel";
            this.recordLengthMinLabel.Size = new System.Drawing.Size(125, 13);
            this.recordLengthMinLabel.TabIndex = 3;
            this.recordLengthMinLabel.Text = "Minimum Record Length:";
            // 
            // sampleRateMinLabel
            // 
            this.sampleRateMinLabel.AutoSize = true;
            this.sampleRateMinLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.sampleRateMinLabel.Location = new System.Drawing.Point(6, 23);
            this.sampleRateMinLabel.Name = "sampleRateMinLabel";
            this.sampleRateMinLabel.Size = new System.Drawing.Size(115, 13);
            this.sampleRateMinLabel.TabIndex = 3;
            this.sampleRateMinLabel.Text = "Minimum Sample Rate:";
            // 
            // sampleDataGroupBox
            // 
            this.sampleDataGroupBox.Controls.Add(this.scaledDataGridView);
            this.sampleDataGroupBox.Location = new System.Drawing.Point(474, 12);
            this.sampleDataGroupBox.Name = "sampleDataGroupBox";
            this.sampleDataGroupBox.Size = new System.Drawing.Size(202, 372);
            this.sampleDataGroupBox.TabIndex = 8;
            this.sampleDataGroupBox.TabStop = false;
            this.sampleDataGroupBox.Text = "Scaled Waveform";
            // 
            // scaledDataGridView
            // 
            this.scaledDataGridView.AllowUserToAddRows = false;
            this.scaledDataGridView.AllowUserToDeleteRows = false;
            this.scaledDataGridView.AllowUserToResizeRows = false;
            this.scaledDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scaledDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.scaledDataGridView.Location = new System.Drawing.Point(6, 19);
            this.scaledDataGridView.Name = "scaledDataGridView";
            this.scaledDataGridView.ReadOnly = true;
            this.scaledDataGridView.RowHeadersVisible = false;
            this.scaledDataGridView.RowHeadersWidth = 15;
            this.scaledDataGridView.RowTemplate.Height = 24;
            this.scaledDataGridView.Size = new System.Drawing.Size(190, 347);
            this.scaledDataGridView.StandardTab = true;
            this.scaledDataGridView.TabIndex = 0;
            // 
            // binaryDataGroupBox
            // 
            this.binaryDataGroupBox.Controls.Add(this.binaryDataGridView);
            this.binaryDataGroupBox.Location = new System.Drawing.Point(266, 12);
            this.binaryDataGroupBox.Name = "binaryDataGroupBox";
            this.binaryDataGroupBox.Size = new System.Drawing.Size(202, 372);
            this.binaryDataGroupBox.TabIndex = 7;
            this.binaryDataGroupBox.TabStop = false;
            this.binaryDataGroupBox.Text = "Binary Waveform";
            // 
            // binaryDataGridView
            // 
            this.binaryDataGridView.AllowUserToAddRows = false;
            this.binaryDataGridView.AllowUserToDeleteRows = false;
            this.binaryDataGridView.AllowUserToResizeRows = false;
            this.binaryDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.binaryDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.binaryDataGridView.Location = new System.Drawing.Point(6, 19);
            this.binaryDataGridView.Name = "binaryDataGridView";
            this.binaryDataGridView.ReadOnly = true;
            this.binaryDataGridView.RowHeadersVisible = false;
            this.binaryDataGridView.RowHeadersWidth = 15;
            this.binaryDataGridView.RowTemplate.Height = 24;
            this.binaryDataGridView.Size = new System.Drawing.Size(190, 347);
            this.binaryDataGridView.StandardTab = true;
            this.binaryDataGridView.TabIndex = 0;
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(132, 19);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "&Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(29, 19);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // messageGroupBox
            // 
            this.messageGroupBox.Controls.Add(this.messageTextBox);
            this.messageGroupBox.Location = new System.Drawing.Point(12, 324);
            this.messageGroupBox.Name = "messageGroupBox";
            this.messageGroupBox.Size = new System.Drawing.Size(248, 60);
            this.messageGroupBox.TabIndex = 4;
            this.messageGroupBox.TabStop = false;
            this.messageGroupBox.Text = "Message";
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(9, 19);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.Size = new System.Drawing.Size(233, 35);
            this.messageTextBox.TabIndex = 0;
            this.messageTextBox.Text = "";
            // 
            // buttonsGroupBox
            // 
            this.buttonsGroupBox.Controls.Add(this.stopButton);
            this.buttonsGroupBox.Controls.Add(this.acquireButton);
            this.buttonsGroupBox.Location = new System.Drawing.Point(12, 466);
            this.buttonsGroupBox.Name = "buttonsGroupBox";
            this.buttonsGroupBox.Size = new System.Drawing.Size(248, 50);
            this.buttonsGroupBox.TabIndex = 6;
            this.buttonsGroupBox.TabStop = false;
            // 
            // binaryDataSizeComboBox
            // 
            this.binaryDataSizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.binaryDataSizeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.binaryDataSizeComboBox.FormattingEnabled = true;
            this.binaryDataSizeComboBox.Location = new System.Drawing.Point(6, 20);
            this.binaryDataSizeComboBox.Name = "binaryDataSizeComboBox";
            this.binaryDataSizeComboBox.Size = new System.Drawing.Size(100, 21);
            this.binaryDataSizeComboBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.binaryDataSizeComboBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 395);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 60);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Binary Data Size (bits)";
            // 
            // gainFactorGroupBox
            // 
            this.gainFactorGroupBox.Controls.Add(this.scaleGainFactorTextBox);
            this.gainFactorGroupBox.Location = new System.Drawing.Point(266, 466);
            this.gainFactorGroupBox.Name = "gainFactorGroupBox";
            this.gainFactorGroupBox.Size = new System.Drawing.Size(410, 50);
            this.gainFactorGroupBox.TabIndex = 10;
            this.gainFactorGroupBox.TabStop = false;
            this.gainFactorGroupBox.Text = "Gain Factor";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 528);
            this.Controls.Add(this.gainFactorGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonsGroupBox);
            this.Controls.Add(this.messageGroupBox);
            this.Controls.Add(this.sampleDataGroupBox);
            this.Controls.Add(this.binaryDataGroupBox);
            this.Controls.Add(this.verticalGroupBox);
            this.Controls.Add(this.horizontalGroupBox);
            this.Controls.Add(this.triggerGroupBox);
            this.Controls.Add(this.offsetGroupBox);
            this.Controls.Add(this.generalGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Binary Acquisition";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_Closing);
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.offsetGroupBox.ResumeLayout(false);
            this.offsetGroupBox.PerformLayout();
            this.triggerGroupBox.ResumeLayout(false);
            this.triggerGroupBox.PerformLayout();
            this.verticalGroupBox.ResumeLayout(false);
            this.verticalGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalOffsetNumeric)).EndInit();
            this.horizontalGroupBox.ResumeLayout(false);
            this.horizontalGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minRecordLengthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minSampleRateNumeric)).EndInit();
            this.sampleDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scaledDataGridView)).EndInit();
            this.binaryDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.binaryDataGridView)).EndInit();
            this.messageGroupBox.ResumeLayout(false);
            this.buttonsGroupBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.gainFactorGroupBox.ResumeLayout(false);
            this.gainFactorGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.GroupBox offsetGroupBox;
        private System.Windows.Forms.TextBox scaleGainFactorTextBox;
        private System.Windows.Forms.TextBox scaleOffsetTextBox;
        private System.Windows.Forms.GroupBox triggerGroupBox;
        private System.Windows.Forms.Label triggerTypeLabel;
        private System.Windows.Forms.GroupBox verticalGroupBox;
        private System.Windows.Forms.Label offsetLabel;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.GroupBox horizontalGroupBox;
        private System.Windows.Forms.Label recordLengthMinLabel;
        private System.Windows.Forms.Label sampleRateMinLabel;
        private System.Windows.Forms.ComboBox triggerTypeComboBox;
        private System.Windows.Forms.NumericUpDown verticalRangeNumeric;
        private System.Windows.Forms.NumericUpDown verticalOffsetNumeric;
        private System.Windows.Forms.NumericUpDown minRecordLengthNumeric;
        private System.Windows.Forms.NumericUpDown minSampleRateNumeric;
        private System.Windows.Forms.GroupBox sampleDataGroupBox;
        private System.Windows.Forms.DataGridView scaledDataGridView;
        private System.Windows.Forms.GroupBox binaryDataGroupBox;
        private System.Windows.Forms.DataGridView binaryDataGridView;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.GroupBox messageGroupBox;
        private System.Windows.Forms.RichTextBox messageTextBox;
        private System.Windows.Forms.GroupBox buttonsGroupBox;
        private System.Windows.Forms.ComboBox binaryDataSizeComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gainFactorGroupBox;



    }
}
