namespace NationalInstruments.Examples.Timestamps
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
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.minSampleRateLabel = new System.Windows.Forms.Label();
            this.verticalRangeLabel = new System.Windows.Forms.Label();
            this.numRecordsLabel = new System.Windows.Forms.Label();
            this.triggertypeLabel = new System.Windows.Forms.Label();
            this.triggersourceLabel = new System.Windows.Forms.Label();
            this.triggerholdoffLabel = new System.Windows.Forms.Label();
            this.meanOfTriggersLabel = new System.Windows.Forms.Label();
            this.stdevOfTriggersLabel = new System.Windows.Forms.Label();
            this.freqOfTriggersLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.textmsg5Label = new System.Windows.Forms.Label();
            this.textmsg3Label = new System.Windows.Forms.Label();
            this.sampleRateMinNumeric = new System.Windows.Forms.NumericUpDown();
            this.verticalRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.numberOfRecordsNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggerHoldoffNumeric = new System.Windows.Forms.NumericUpDown();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.triggerTypeComboBox = new System.Windows.Forms.ComboBox();
            this.triggerGroupBox = new System.Windows.Forms.GroupBox();
            this.triggerSourceComboBox = new System.Windows.Forms.ComboBox();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.sampleDataGroupBox = new System.Windows.Forms.GroupBox();
            this.meanfrequencyTextBox = new System.Windows.Forms.TextBox();
            this.standardDeviationTextBox = new System.Windows.Forms.TextBox();
            this.meanTimeTextBox = new System.Windows.Forms.TextBox();
            this.histogramDataGridView = new System.Windows.Forms.DataGridView();
            this.acquireButton = new System.Windows.Forms.Button();
            this.buttonGroupBox = new System.Windows.Forms.GroupBox();
            this.histogramIndexDataGridViewColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.histogramXValueDataGridViewColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.histogramYValueDataGridViewColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateMinNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfRecordsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerHoldoffNumeric)).BeginInit();
            this.triggerGroupBox.SuspendLayout();
            this.generalGroupBox.SuspendLayout();
            this.sampleDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.histogramDataGridView)).BeginInit();
            this.buttonGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelNameLabel
            // 
            this.channelNameLabel.AutoSize = true;
            this.channelNameLabel.Location = new System.Drawing.Point(4, 50);
            this.channelNameLabel.Name = "channelNameLabel";
            this.channelNameLabel.Size = new System.Drawing.Size(80, 13);
            this.channelNameLabel.TabIndex = 0;
            this.channelNameLabel.Text = "Channel Name:";
            // 
            // minSampleRateLabel
            // 
            this.minSampleRateLabel.AutoSize = true;
            this.minSampleRateLabel.Location = new System.Drawing.Point(4, 76);
            this.minSampleRateLabel.Name = "minSampleRateLabel";
            this.minSampleRateLabel.Size = new System.Drawing.Size(91, 13);
            this.minSampleRateLabel.TabIndex = 1;
            this.minSampleRateLabel.Text = "Min Sample Rate:";
            // 
            // verticalRangeLabel
            // 
            this.verticalRangeLabel.AutoSize = true;
            this.verticalRangeLabel.Location = new System.Drawing.Point(4, 128);
            this.verticalRangeLabel.Name = "verticalRangeLabel";
            this.verticalRangeLabel.Size = new System.Drawing.Size(80, 13);
            this.verticalRangeLabel.TabIndex = 2;
            this.verticalRangeLabel.Text = "Vertical Range:";
            // 
            // numRecordsLabel
            // 
            this.numRecordsLabel.AutoSize = true;
            this.numRecordsLabel.Location = new System.Drawing.Point(4, 102);
            this.numRecordsLabel.Name = "numRecordsLabel";
            this.numRecordsLabel.Size = new System.Drawing.Size(78, 13);
            this.numRecordsLabel.TabIndex = 3;
            this.numRecordsLabel.Text = "Num. Records:";
            // 
            // triggertypeLabel
            // 
            this.triggertypeLabel.AutoSize = true;
            this.triggertypeLabel.Location = new System.Drawing.Point(4, 23);
            this.triggertypeLabel.Name = "triggertypeLabel";
            this.triggertypeLabel.Size = new System.Drawing.Size(34, 13);
            this.triggertypeLabel.TabIndex = 4;
            this.triggertypeLabel.Text = "Type:";
            // 
            // triggersourceLabel
            // 
            this.triggersourceLabel.AutoSize = true;
            this.triggersourceLabel.Location = new System.Drawing.Point(4, 50);
            this.triggersourceLabel.Name = "triggersourceLabel";
            this.triggersourceLabel.Size = new System.Drawing.Size(44, 13);
            this.triggersourceLabel.TabIndex = 5;
            this.triggersourceLabel.Text = "Source:";
            // 
            // triggerholdoffLabel
            // 
            this.triggerholdoffLabel.AutoSize = true;
            this.triggerholdoffLabel.Location = new System.Drawing.Point(4, 77);
            this.triggerholdoffLabel.Name = "triggerholdoffLabel";
            this.triggerholdoffLabel.Size = new System.Drawing.Size(44, 13);
            this.triggerholdoffLabel.TabIndex = 6;
            this.triggerholdoffLabel.Text = "Holdoff:";
            // 
            // meanOfTriggersLabel
            // 
            this.meanOfTriggersLabel.AutoSize = true;
            this.meanOfTriggersLabel.Location = new System.Drawing.Point(6, 280);
            this.meanOfTriggersLabel.Name = "meanOfTriggersLabel";
            this.meanOfTriggersLabel.Size = new System.Drawing.Size(154, 13);
            this.meanOfTriggersLabel.TabIndex = 8;
            this.meanOfTriggersLabel.Text = "Mean time between triggers (s):";
            // 
            // stdevOfTriggersLabel
            // 
            this.stdevOfTriggersLabel.AutoSize = true;
            this.stdevOfTriggersLabel.Location = new System.Drawing.Point(6, 306);
            this.stdevOfTriggersLabel.Name = "stdevOfTriggersLabel";
            this.stdevOfTriggersLabel.Size = new System.Drawing.Size(184, 13);
            this.stdevOfTriggersLabel.TabIndex = 9;
            this.stdevOfTriggersLabel.Text = "Standard deviation time of triggers (s):";
            // 
            // freqOfTriggersLabel
            // 
            this.freqOfTriggersLabel.AutoSize = true;
            this.freqOfTriggersLabel.Location = new System.Drawing.Point(6, 332);
            this.freqOfTriggersLabel.Name = "freqOfTriggersLabel";
            this.freqOfTriggersLabel.Size = new System.Drawing.Size(150, 13);
            this.freqOfTriggersLabel.TabIndex = 10;
            this.freqOfTriggersLabel.Text = "Mean frequency of triggers (s):";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(4, 23);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 14;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // textmsg5Label
            // 
            this.textmsg5Label.AutoSize = true;
            this.textmsg5Label.Location = new System.Drawing.Point(286, 6);
            this.textmsg5Label.Name = "textmsg5Label";
            this.textmsg5Label.Size = new System.Drawing.Size(0, 13);
            this.textmsg5Label.TabIndex = 5;
            // 
            // textmsg3Label
            // 
            this.textmsg3Label.AutoSize = true;
            this.textmsg3Label.Location = new System.Drawing.Point(37, 173);
            this.textmsg3Label.Name = "textmsg3Label";
            this.textmsg3Label.Size = new System.Drawing.Size(0, 13);
            this.textmsg3Label.TabIndex = 17;
            // 
            // sampleRateNumericMin
            // 
            this.sampleRateMinNumeric.DecimalPlaces = 2;
            this.sampleRateMinNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.sampleRateMinNumeric.Location = new System.Drawing.Point(104, 72);
            this.sampleRateMinNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.sampleRateMinNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.sampleRateMinNumeric.Name = "sampleRateNumericMin";
            this.sampleRateMinNumeric.Size = new System.Drawing.Size(107, 20);
            this.sampleRateMinNumeric.TabIndex = 2;
            this.sampleRateMinNumeric.Value = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            // 
            // verticalRangeNumeric
            // 
            this.verticalRangeNumeric.DecimalPlaces = 2;
            this.verticalRangeNumeric.Location = new System.Drawing.Point(104, 124);
            this.verticalRangeNumeric.Maximum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            0});
            this.verticalRangeNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.verticalRangeNumeric.Name = "verticalRangeNumeric";
            this.verticalRangeNumeric.Size = new System.Drawing.Size(107, 20);
            this.verticalRangeNumeric.TabIndex = 4;
            this.verticalRangeNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numberOfRecordsNumeric
            // 
            this.numberOfRecordsNumeric.Location = new System.Drawing.Point(104, 98);
            this.numberOfRecordsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numberOfRecordsNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.numberOfRecordsNumeric.Name = "numberOfRecordsNumeric";
            this.numberOfRecordsNumeric.Size = new System.Drawing.Size(107, 20);
            this.numberOfRecordsNumeric.TabIndex = 3;
            this.numberOfRecordsNumeric.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // triggerHoldoffNumeric
            // 
            this.triggerHoldoffNumeric.DecimalPlaces = 2;
            this.triggerHoldoffNumeric.Location = new System.Drawing.Point(104, 73);
            this.triggerHoldoffNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.triggerHoldoffNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.triggerHoldoffNumeric.Name = "triggerHoldoffNumeric";
            this.triggerHoldoffNumeric.Size = new System.Drawing.Size(107, 20);
            this.triggerHoldoffNumeric.TabIndex = 2;
            // 
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(104, 46);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(107, 20);
            this.channelNameTextBox.TabIndex = 1;
            this.channelNameTextBox.Text = "0";
            // 
            // triggerTypeComboBox
            // 
            this.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerTypeComboBox.Location = new System.Drawing.Point(104, 19);
            this.triggerTypeComboBox.Name = "triggerTypeComboBox";
            this.triggerTypeComboBox.Size = new System.Drawing.Size(107, 21);
            this.triggerTypeComboBox.TabIndex = 0;
            // 
            // triggerGroupBox
            // 
            this.triggerGroupBox.Controls.Add(this.triggerTypeComboBox);
            this.triggerGroupBox.Controls.Add(this.triggerSourceComboBox);
            this.triggerGroupBox.Controls.Add(this.triggerHoldoffNumeric);
            this.triggerGroupBox.Controls.Add(this.triggertypeLabel);
            this.triggerGroupBox.Controls.Add(this.triggersourceLabel);
            this.triggerGroupBox.Controls.Add(this.triggerholdoffLabel);
            this.triggerGroupBox.Location = new System.Drawing.Point(12, 180);
            this.triggerGroupBox.Name = "triggerGroupBox";
            this.triggerGroupBox.Size = new System.Drawing.Size(217, 102);
            this.triggerGroupBox.TabIndex = 1;
            this.triggerGroupBox.TabStop = false;
            this.triggerGroupBox.Text = "Trigger";
            // 
            // triggerSourceComboBox
            // 
            this.triggerSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerSourceComboBox.Location = new System.Drawing.Point(104, 46);
            this.triggerSourceComboBox.Name = "triggerSourceComboBox";
            this.triggerSourceComboBox.Size = new System.Drawing.Size(107, 21);
            this.triggerSourceComboBox.TabIndex = 1;
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.resourceNameComboBox);
            this.generalGroupBox.Controls.Add(this.channelNameTextBox);
            this.generalGroupBox.Controls.Add(this.sampleRateMinNumeric);
            this.generalGroupBox.Controls.Add(this.verticalRangeNumeric);
            this.generalGroupBox.Controls.Add(this.numberOfRecordsNumeric);
            this.generalGroupBox.Controls.Add(this.channelNameLabel);
            this.generalGroupBox.Controls.Add(this.minSampleRateLabel);
            this.generalGroupBox.Controls.Add(this.verticalRangeLabel);
            this.generalGroupBox.Controls.Add(this.numRecordsLabel);
            this.generalGroupBox.Controls.Add(this.resourceNameLabel);
            this.generalGroupBox.Location = new System.Drawing.Point(12, 12);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(217, 152);
            this.generalGroupBox.TabIndex = 0;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(104, 19);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(107, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // sampleDataGroupBox
            // 
            this.sampleDataGroupBox.Controls.Add(this.meanfrequencyTextBox);
            this.sampleDataGroupBox.Controls.Add(this.standardDeviationTextBox);
            this.sampleDataGroupBox.Controls.Add(this.meanTimeTextBox);
            this.sampleDataGroupBox.Controls.Add(this.histogramDataGridView);
            this.sampleDataGroupBox.Controls.Add(this.meanOfTriggersLabel);
            this.sampleDataGroupBox.Controls.Add(this.freqOfTriggersLabel);
            this.sampleDataGroupBox.Controls.Add(this.stdevOfTriggersLabel);
            this.sampleDataGroupBox.Location = new System.Drawing.Point(235, 12);
            this.sampleDataGroupBox.Name = "sampleDataGroupBox";
            this.sampleDataGroupBox.Size = new System.Drawing.Size(329, 354);
            this.sampleDataGroupBox.TabIndex = 4;
            this.sampleDataGroupBox.TabStop = false;
            this.sampleDataGroupBox.Text = "Trigger Difference Histogram";
            // 
            // meanfrequencyTextBox
            // 
            this.meanfrequencyTextBox.Location = new System.Drawing.Point(196, 329);
            this.meanfrequencyTextBox.Name = "meanfrequencyTextBox";
            this.meanfrequencyTextBox.ReadOnly = true;
            this.meanfrequencyTextBox.Size = new System.Drawing.Size(125, 20);
            this.meanfrequencyTextBox.TabIndex = 3;
            this.meanfrequencyTextBox.Text = "0";
            // 
            // standardDeviationTextBox
            // 
            this.standardDeviationTextBox.Location = new System.Drawing.Point(196, 302);
            this.standardDeviationTextBox.Name = "standardDeviationTextBox";
            this.standardDeviationTextBox.ReadOnly = true;
            this.standardDeviationTextBox.Size = new System.Drawing.Size(125, 20);
            this.standardDeviationTextBox.TabIndex = 2;
            this.standardDeviationTextBox.Text = "0";
            // 
            // meanTimeTextBox
            // 
            this.meanTimeTextBox.Location = new System.Drawing.Point(196, 276);
            this.meanTimeTextBox.Name = "meanTimeTextBox";
            this.meanTimeTextBox.ReadOnly = true;
            this.meanTimeTextBox.Size = new System.Drawing.Size(125, 20);
            this.meanTimeTextBox.TabIndex = 1;
            this.meanTimeTextBox.Text = "0";
            // 
            // histogramDataGridView
            // 
            this.histogramDataGridView.AllowUserToAddRows = false;
            this.histogramDataGridView.AllowUserToDeleteRows = false;
            this.histogramDataGridView.AllowUserToResizeRows = false;
            this.histogramDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.histogramDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.histogramDataGridView.ColumnHeadersVisible = false;
            this.histogramDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.histogramIndexDataGridViewColumn,
            this.histogramXValueDataGridViewColumn,
            this.histogramYValueDataGridViewColumn});
            this.histogramDataGridView.Location = new System.Drawing.Point(6, 19);
            this.histogramDataGridView.Name = "histogramDataGridView";
            this.histogramDataGridView.ReadOnly = true;
            this.histogramDataGridView.RowHeadersVisible = false;
            this.histogramDataGridView.RowHeadersWidth = 15;
            this.histogramDataGridView.RowTemplate.Height = 24;
            this.histogramDataGridView.Size = new System.Drawing.Size(315, 251);
            this.histogramDataGridView.StandardTab = true;
            this.histogramDataGridView.TabIndex = 0;
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(73, 19);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // buttonGroupBox
            // 
            this.buttonGroupBox.Controls.Add(this.acquireButton);
            this.buttonGroupBox.Location = new System.Drawing.Point(12, 307);
            this.buttonGroupBox.Name = "buttonGroupBox";
            this.buttonGroupBox.Size = new System.Drawing.Size(211, 59);
            this.buttonGroupBox.TabIndex = 3;
            this.buttonGroupBox.TabStop = false;
            // 
            // histogramIndexDataGridViewColumn
            // 
            this.histogramIndexDataGridViewColumn.HeaderText = "Index";
            this.histogramIndexDataGridViewColumn.Name = "histogramIndexDataGridViewColumn";
            this.histogramIndexDataGridViewColumn.ReadOnly = true;
            this.histogramIndexDataGridViewColumn.Width = 45;
            // 
            // histogramXValueDataGridViewColumn
            // 
            this.histogramXValueDataGridViewColumn.HeaderText = "Histogram X Value";
            this.histogramXValueDataGridViewColumn.Name = "histogramXValueDataGridViewColumn";
            this.histogramXValueDataGridViewColumn.ReadOnly = true;
            this.histogramXValueDataGridViewColumn.Width = 125;
            // 
            // histogramYValueDataGridViewColumn
            // 
            this.histogramYValueDataGridViewColumn.HeaderText = "Histogram Y Value";
            this.histogramYValueDataGridViewColumn.Name = "histogramYValueDataGridViewColumn";
            this.histogramYValueDataGridViewColumn.ReadOnly = true;
            this.histogramYValueDataGridViewColumn.Width = 125;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 380);
            this.Controls.Add(this.buttonGroupBox);
            this.Controls.Add(this.sampleDataGroupBox);
            this.Controls.Add(this.textmsg5Label);
            this.Controls.Add(this.textmsg3Label);
            this.Controls.Add(this.triggerGroupBox);
            this.Controls.Add(this.generalGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Timestamps";
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateMinNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfRecordsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerHoldoffNumeric)).EndInit();
            this.triggerGroupBox.ResumeLayout(false);
            this.triggerGroupBox.PerformLayout();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.sampleDataGroupBox.ResumeLayout(false);
            this.sampleDataGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.histogramDataGridView)).EndInit();
            this.buttonGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label minSampleRateLabel;
        private System.Windows.Forms.Label verticalRangeLabel;
        private System.Windows.Forms.Label numRecordsLabel;
        private System.Windows.Forms.Label triggertypeLabel;
        private System.Windows.Forms.Label triggersourceLabel;
        private System.Windows.Forms.Label triggerholdoffLabel;
        private System.Windows.Forms.Label meanOfTriggersLabel;
        private System.Windows.Forms.Label stdevOfTriggersLabel;
        private System.Windows.Forms.Label freqOfTriggersLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label textmsg5Label;
        private System.Windows.Forms.Label textmsg3Label;
        private System.Windows.Forms.NumericUpDown sampleRateMinNumeric;
        private System.Windows.Forms.NumericUpDown verticalRangeNumeric;
        private System.Windows.Forms.NumericUpDown numberOfRecordsNumeric;
        private System.Windows.Forms.NumericUpDown triggerHoldoffNumeric;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.ComboBox triggerTypeComboBox;
        private System.Windows.Forms.ComboBox triggerSourceComboBox;
        private System.Windows.Forms.GroupBox triggerGroupBox;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.GroupBox sampleDataGroupBox;
        private System.Windows.Forms.DataGridView histogramDataGridView;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.TextBox meanfrequencyTextBox;
        private System.Windows.Forms.TextBox standardDeviationTextBox;
        private System.Windows.Forms.TextBox meanTimeTextBox;
        private System.Windows.Forms.GroupBox buttonGroupBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn histogramIndexDataGridViewColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn histogramXValueDataGridViewColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn histogramYValueDataGridViewColumn;
    }
}
