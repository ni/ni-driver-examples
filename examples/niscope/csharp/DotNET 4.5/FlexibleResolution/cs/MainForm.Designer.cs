namespace NationalInstruments.Examples.FlexibleResolution
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.triggerTypeLabel = new System.Windows.Forms.Label();
            this.verticalRangeLabel = new System.Windows.Forms.Label();
            this.resolutionTextMsgLabel = new System.Windows.Forms.Label();
            this.actualSampleRateLabel = new System.Windows.Forms.Label();
            this.sampleRateMinLabel = new System.Windows.Forms.Label();
            this.recordLengthMinLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.channelInfoTextMsgLabel = new System.Windows.Forms.Label();
            this.verticalRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.minSampleRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.minRecordLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.resolutionTextBox = new System.Windows.Forms.TextBox();
            this.actualSampleRateTextBox = new System.Windows.Forms.TextBox();
            this.acquireButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.triggerTypeComboBox = new System.Windows.Forms.ComboBox();
            this.triggeringGroupBox = new System.Windows.Forms.GroupBox();
            this.horizontalConfigurationGroupBox = new System.Windows.Forms.GroupBox();
            this.verticalConfigurationGroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.sampledDataGroupBox = new System.Windows.Forms.GroupBox();
            this.sampledDataGridView = new System.Windows.Forms.DataGridView();
            this.measurementDataGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementDataGridView = new System.Windows.Forms.DataGridView();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.buttonsGroupBox = new System.Windows.Forms.GroupBox();
            this.messageGroupBox = new System.Windows.Forms.GroupBox();
            this.messageTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minSampleRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minRecordLengthNumeric)).BeginInit();
            this.triggeringGroupBox.SuspendLayout();
            this.horizontalConfigurationGroupBox.SuspendLayout();
            this.verticalConfigurationGroupBox.SuspendLayout();
            this.sampledDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sampledDataGridView)).BeginInit();
            this.measurementDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measurementDataGridView)).BeginInit();
            this.generalGroupBox.SuspendLayout();
            this.buttonsGroupBox.SuspendLayout();
            this.messageGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // triggerTypeLabel
            // 
            this.triggerTypeLabel.AutoSize = true;
            this.triggerTypeLabel.Location = new System.Drawing.Point(6, 23);
            this.triggerTypeLabel.Name = "triggerTypeLabel";
            this.triggerTypeLabel.Size = new System.Drawing.Size(66, 13);
            this.triggerTypeLabel.TabIndex = 0;
            this.triggerTypeLabel.Text = "Trigger type:";
            // 
            // verticalRangeLabel
            // 
            this.verticalRangeLabel.AutoSize = true;
            this.verticalRangeLabel.Location = new System.Drawing.Point(6, 23);
            this.verticalRangeLabel.Name = "verticalRangeLabel";
            this.verticalRangeLabel.Size = new System.Drawing.Size(75, 13);
            this.verticalRangeLabel.TabIndex = 0;
            this.verticalRangeLabel.Text = "Vertical range:";
            // 
            // resolutionTextMsgLabel
            // 
            this.resolutionTextMsgLabel.AutoSize = true;
            this.resolutionTextMsgLabel.Location = new System.Drawing.Point(6, 68);
            this.resolutionTextMsgLabel.Name = "resolutionTextMsgLabel";
            this.resolutionTextMsgLabel.Size = new System.Drawing.Size(101, 26);
            this.resolutionTextMsgLabel.TabIndex = 4;
            this.resolutionTextMsgLabel.Text = "Advertised effective\r\n number of bits:";
            // 
            // actualSampleRateLabel
            // 
            this.actualSampleRateLabel.AutoSize = true;
            this.actualSampleRateLabel.Location = new System.Drawing.Point(6, 49);
            this.actualSampleRateLabel.Name = "actualSampleRateLabel";
            this.actualSampleRateLabel.Size = new System.Drawing.Size(97, 13);
            this.actualSampleRateLabel.TabIndex = 2;
            this.actualSampleRateLabel.Text = "Actual sample rate:";
            // 
            // sampleRateMinLabel
            // 
            this.sampleRateMinLabel.AutoSize = true;
            this.sampleRateMinLabel.Location = new System.Drawing.Point(6, 23);
            this.sampleRateMinLabel.Name = "sampleRateMinLabel";
            this.sampleRateMinLabel.Size = new System.Drawing.Size(84, 13);
            this.sampleRateMinLabel.TabIndex = 0;
            this.sampleRateMinLabel.Text = "Min sample rate:";
            // 
            // recordLengthMinLabel
            // 
            this.recordLengthMinLabel.AutoSize = true;
            this.recordLengthMinLabel.Location = new System.Drawing.Point(6, 101);
            this.recordLengthMinLabel.Name = "recordLengthMinLabel";
            this.recordLengthMinLabel.Size = new System.Drawing.Size(92, 13);
            this.recordLengthMinLabel.TabIndex = 6;
            this.recordLengthMinLabel.Text = "Min record length:";
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
            // channelInfoTextMsgLabel
            // 
            this.channelInfoTextMsgLabel.AutoSize = true;
            this.channelInfoTextMsgLabel.Location = new System.Drawing.Point(33, 51);
            this.channelInfoTextMsgLabel.Name = "channelInfoTextMsgLabel";
            this.channelInfoTextMsgLabel.Size = new System.Drawing.Size(177, 13);
            this.channelInfoTextMsgLabel.TabIndex = 2;
            this.channelInfoTextMsgLabel.Text = "Channel \"0\" is used for this example";
            // 
            // verticalRangeNumeric
            // 
            this.verticalRangeNumeric.DecimalPlaces = 2;
            this.verticalRangeNumeric.Location = new System.Drawing.Point(138, 19);
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
            this.verticalRangeNumeric.Size = new System.Drawing.Size(97, 20);
            this.verticalRangeNumeric.TabIndex = 0;
            this.verticalRangeNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // minSampleRateNumeric
            // 
            this.minSampleRateNumeric.DecimalPlaces = 2;
            this.minSampleRateNumeric.Location = new System.Drawing.Point(140, 19);
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
            this.minSampleRateNumeric.Size = new System.Drawing.Size(97, 20);
            this.minSampleRateNumeric.TabIndex = 0;
            this.minSampleRateNumeric.Value = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            // 
            // minRecordLengthNumeric
            // 
            this.minRecordLengthNumeric.Location = new System.Drawing.Point(140, 97);
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
            this.minRecordLengthNumeric.Size = new System.Drawing.Size(97, 20);
            this.minRecordLengthNumeric.TabIndex = 3;
            this.minRecordLengthNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // resolutionTextBox
            // 
            this.resolutionTextBox.Location = new System.Drawing.Point(140, 71);
            this.resolutionTextBox.Name = "resolutionTextBox";
            this.resolutionTextBox.ReadOnly = true;
            this.resolutionTextBox.Size = new System.Drawing.Size(97, 20);
            this.resolutionTextBox.TabIndex = 2;
            this.resolutionTextBox.Text = "0";
            // 
            // actualSampleRateTextBox
            // 
            this.actualSampleRateTextBox.Location = new System.Drawing.Point(140, 45);
            this.actualSampleRateTextBox.Name = "actualSampleRateTextBox";
            this.actualSampleRateTextBox.ReadOnly = true;
            this.actualSampleRateTextBox.Size = new System.Drawing.Size(97, 20);
            this.actualSampleRateTextBox.TabIndex = 1;
            this.actualSampleRateTextBox.Text = "0";
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(121, 19);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(202, 19);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "&Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // triggerTypeComboBox
            // 
            this.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerTypeComboBox.Location = new System.Drawing.Point(140, 19);
            this.triggerTypeComboBox.Name = "triggerTypeComboBox";
            this.triggerTypeComboBox.Size = new System.Drawing.Size(97, 21);
            this.triggerTypeComboBox.TabIndex = 0;
            // 
            // triggeringGroupBox
            // 
            this.triggeringGroupBox.Controls.Add(this.triggerTypeComboBox);
            this.triggeringGroupBox.Controls.Add(this.triggerTypeLabel);
            this.triggeringGroupBox.Location = new System.Drawing.Point(10, 95);
            this.triggeringGroupBox.Name = "triggeringGroupBox";
            this.triggeringGroupBox.Size = new System.Drawing.Size(243, 47);
            this.triggeringGroupBox.TabIndex = 2;
            this.triggeringGroupBox.TabStop = false;
            this.triggeringGroupBox.Text = "Triggering";
            // 
            // horizontalConfigurationGroupBox
            // 
            this.horizontalConfigurationGroupBox.Controls.Add(this.resolutionTextBox);
            this.horizontalConfigurationGroupBox.Controls.Add(this.actualSampleRateTextBox);
            this.horizontalConfigurationGroupBox.Controls.Add(this.minSampleRateNumeric);
            this.horizontalConfigurationGroupBox.Controls.Add(this.minRecordLengthNumeric);
            this.horizontalConfigurationGroupBox.Controls.Add(this.resolutionTextMsgLabel);
            this.horizontalConfigurationGroupBox.Controls.Add(this.actualSampleRateLabel);
            this.horizontalConfigurationGroupBox.Controls.Add(this.sampleRateMinLabel);
            this.horizontalConfigurationGroupBox.Controls.Add(this.recordLengthMinLabel);
            this.horizontalConfigurationGroupBox.Location = new System.Drawing.Point(10, 210);
            this.horizontalConfigurationGroupBox.Name = "horizontalConfigurationGroupBox";
            this.horizontalConfigurationGroupBox.Size = new System.Drawing.Size(243, 125);
            this.horizontalConfigurationGroupBox.TabIndex = 4;
            this.horizontalConfigurationGroupBox.TabStop = false;
            this.horizontalConfigurationGroupBox.Text = "Horizontal Configuration";
            // 
            // verticalConfigurationGroupBox
            // 
            this.verticalConfigurationGroupBox.Controls.Add(this.verticalRangeNumeric);
            this.verticalConfigurationGroupBox.Controls.Add(this.verticalRangeLabel);
            this.verticalConfigurationGroupBox.Location = new System.Drawing.Point(12, 153);
            this.verticalConfigurationGroupBox.Name = "verticalConfigurationGroupBox";
            this.verticalConfigurationGroupBox.Size = new System.Drawing.Size(241, 46);
            this.verticalConfigurationGroupBox.TabIndex = 3;
            this.verticalConfigurationGroupBox.TabStop = false;
            this.verticalConfigurationGroupBox.Text = "Vertical Configuration";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(138, 19);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(97, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // sampledDataGroupBox
            // 
            this.sampledDataGroupBox.Controls.Add(this.sampledDataGridView);
            this.sampledDataGroupBox.Location = new System.Drawing.Point(259, 12);
            this.sampledDataGroupBox.Name = "sampledDataGroupBox";
            this.sampledDataGroupBox.Size = new System.Drawing.Size(202, 323);
            this.sampledDataGroupBox.TabIndex = 6;
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
            this.sampledDataGridView.Size = new System.Drawing.Size(190, 298);
            this.sampledDataGridView.StandardTab = true;
            this.sampledDataGridView.TabIndex = 0;
            // 
            // measurementDataGroupBox
            // 
            this.measurementDataGroupBox.Controls.Add(this.measurementDataGridView);
            this.measurementDataGroupBox.Location = new System.Drawing.Point(467, 12);
            this.measurementDataGroupBox.Name = "measurementDataGroupBox";
            this.measurementDataGroupBox.Size = new System.Drawing.Size(191, 323);
            this.measurementDataGroupBox.TabIndex = 7;
            this.measurementDataGroupBox.TabStop = false;
            this.measurementDataGroupBox.Text = "Measurement Data";
            // 
            // measurementDataGridView
            // 
            this.measurementDataGridView.AllowUserToAddRows = false;
            this.measurementDataGridView.AllowUserToDeleteRows = false;
            this.measurementDataGridView.AllowUserToResizeRows = false;
            this.measurementDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.measurementDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.measurementDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.measurementDataGridView.DefaultCellStyle = dataGridViewCellStyle5;
            this.measurementDataGridView.Location = new System.Drawing.Point(6, 19);
            this.measurementDataGridView.Name = "measurementDataGridView";
            this.measurementDataGridView.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.measurementDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.measurementDataGridView.RowHeadersVisible = false;
            this.measurementDataGridView.RowHeadersWidth = 15;
            this.measurementDataGridView.RowTemplate.Height = 24;
            this.measurementDataGridView.Size = new System.Drawing.Size(179, 298);
            this.measurementDataGridView.StandardTab = true;
            this.measurementDataGridView.TabIndex = 0;
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.resourceNameComboBox);
            this.generalGroupBox.Controls.Add(this.channelInfoTextMsgLabel);
            this.generalGroupBox.Controls.Add(this.resourceNameLabel);
            this.generalGroupBox.Location = new System.Drawing.Point(12, 12);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(241, 72);
            this.generalGroupBox.TabIndex = 1;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            // 
            // buttonsGroupBox
            // 
            this.buttonsGroupBox.Controls.Add(this.acquireButton);
            this.buttonsGroupBox.Controls.Add(this.stopButton);
            this.buttonsGroupBox.Location = new System.Drawing.Point(265, 346);
            this.buttonsGroupBox.Name = "buttonsGroupBox";
            this.buttonsGroupBox.Size = new System.Drawing.Size(393, 57);
            this.buttonsGroupBox.TabIndex = 7;
            this.buttonsGroupBox.TabStop = false;
            // 
            // messageGroupBox
            // 
            this.messageGroupBox.Controls.Add(this.messageTextBox);
            this.messageGroupBox.Location = new System.Drawing.Point(10, 346);
            this.messageGroupBox.Name = "messageGroupBox";
            this.messageGroupBox.Size = new System.Drawing.Size(243, 57);
            this.messageGroupBox.TabIndex = 5;
            this.messageGroupBox.TabStop = false;
            this.messageGroupBox.Text = "Message";
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(9, 19);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.Size = new System.Drawing.Size(228, 32);
            this.messageTextBox.TabIndex = 0;
            this.messageTextBox.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 416);
            this.Controls.Add(this.messageGroupBox);
            this.Controls.Add(this.buttonsGroupBox);
            this.Controls.Add(this.generalGroupBox);
            this.Controls.Add(this.measurementDataGroupBox);
            this.Controls.Add(this.sampledDataGroupBox);
            this.Controls.Add(this.triggeringGroupBox);
            this.Controls.Add(this.horizontalConfigurationGroupBox);
            this.Controls.Add(this.verticalConfigurationGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Flexible Resolution";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minSampleRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minRecordLengthNumeric)).EndInit();
            this.triggeringGroupBox.ResumeLayout(false);
            this.triggeringGroupBox.PerformLayout();
            this.horizontalConfigurationGroupBox.ResumeLayout(false);
            this.horizontalConfigurationGroupBox.PerformLayout();
            this.verticalConfigurationGroupBox.ResumeLayout(false);
            this.verticalConfigurationGroupBox.PerformLayout();
            this.sampledDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sampledDataGridView)).EndInit();
            this.measurementDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.measurementDataGridView)).EndInit();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.buttonsGroupBox.ResumeLayout(false);
            this.messageGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label triggerTypeLabel;
        private System.Windows.Forms.Label verticalRangeLabel;
        private System.Windows.Forms.Label resolutionTextMsgLabel;
        private System.Windows.Forms.Label actualSampleRateLabel;
        private System.Windows.Forms.Label sampleRateMinLabel;
        private System.Windows.Forms.Label recordLengthMinLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label channelInfoTextMsgLabel;
        private System.Windows.Forms.NumericUpDown verticalRangeNumeric;
        private System.Windows.Forms.NumericUpDown minSampleRateNumeric;
        private System.Windows.Forms.NumericUpDown minRecordLengthNumeric;
        private System.Windows.Forms.TextBox resolutionTextBox;
        private System.Windows.Forms.TextBox actualSampleRateTextBox;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox triggerTypeComboBox;
        private System.Windows.Forms.GroupBox triggeringGroupBox;
        private System.Windows.Forms.GroupBox horizontalConfigurationGroupBox;
        private System.Windows.Forms.GroupBox verticalConfigurationGroupBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.GroupBox sampledDataGroupBox;
        private System.Windows.Forms.DataGridView sampledDataGridView;
        private System.Windows.Forms.GroupBox measurementDataGroupBox;
        private System.Windows.Forms.DataGridView measurementDataGridView;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.GroupBox buttonsGroupBox;
        private System.Windows.Forms.GroupBox messageGroupBox;
        private System.Windows.Forms.RichTextBox messageTextBox;


    }
}
