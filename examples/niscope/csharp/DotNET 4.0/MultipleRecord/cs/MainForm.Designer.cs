namespace NationalInstruments.Examples.MultipleRecord
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
            this.numOfRecLabel = new System.Windows.Forms.Label();
            this.verticalRangeLabel = new System.Windows.Forms.Label();
            this.minSampleRateLabel = new System.Windows.Forms.Label();
            this.minRecordLengthLabel = new System.Windows.Forms.Label();
            this.numOfRecordsNumeric = new System.Windows.Forms.NumericUpDown();
            this.verticalRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.minSampleRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.minRecordLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.acquireButton = new System.Windows.Forms.Button();
            this.verticalAndHorizontalGroupBox = new System.Windows.Forms.GroupBox();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.plotRelativeToLabel = new System.Windows.Forms.Label();
            this.plotRelativeToComboBox = new System.Windows.Forms.ComboBox();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.sampledDataGroupBox = new System.Windows.Forms.GroupBox();
            this.sampledDataGridView = new System.Windows.Forms.DataGridView();
            this.buttonsGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numOfRecordsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minSampleRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minRecordLengthNumeric)).BeginInit();
            this.verticalAndHorizontalGroupBox.SuspendLayout();
            this.generalGroupBox.SuspendLayout();
            this.sampledDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sampledDataGridView)).BeginInit();
            this.buttonsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // numOfRecLabel
            // 
            this.numOfRecLabel.AutoSize = true;
            this.numOfRecLabel.Location = new System.Drawing.Point(6, 21);
            this.numOfRecLabel.Name = "numOfRecLabel";
            this.numOfRecLabel.Size = new System.Drawing.Size(97, 13);
            this.numOfRecLabel.TabIndex = 3;
            this.numOfRecLabel.Text = "Number of records:";
            // 
            // verticalRangeLabel
            // 
            this.verticalRangeLabel.AutoSize = true;
            this.verticalRangeLabel.Location = new System.Drawing.Point(6, 49);
            this.verticalRangeLabel.Name = "verticalRangeLabel";
            this.verticalRangeLabel.Size = new System.Drawing.Size(75, 13);
            this.verticalRangeLabel.TabIndex = 4;
            this.verticalRangeLabel.Text = "Vertical range:";
            // 
            // minSampleRateLabel
            // 
            this.minSampleRateLabel.AutoSize = true;
            this.minSampleRateLabel.Location = new System.Drawing.Point(6, 75);
            this.minSampleRateLabel.Name = "minSampleRateLabel";
            this.minSampleRateLabel.Size = new System.Drawing.Size(84, 13);
            this.minSampleRateLabel.TabIndex = 5;
            this.minSampleRateLabel.Text = "Min sample rate:";
            // 
            // minRecordLengthLabel
            // 
            this.minRecordLengthLabel.AutoSize = true;
            this.minRecordLengthLabel.Location = new System.Drawing.Point(6, 101);
            this.minRecordLengthLabel.Name = "minRecordLengthLabel";
            this.minRecordLengthLabel.Size = new System.Drawing.Size(92, 13);
            this.minRecordLengthLabel.TabIndex = 6;
            this.minRecordLengthLabel.Text = "Min record length:";
            // 
            // numOfRecordsNumeric
            // 
            this.numOfRecordsNumeric.Location = new System.Drawing.Point(123, 19);
            this.numOfRecordsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numOfRecordsNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.numOfRecordsNumeric.Name = "numOfRecordsNumeric";
            this.numOfRecordsNumeric.Size = new System.Drawing.Size(100, 20);
            this.numOfRecordsNumeric.TabIndex = 0;
            this.numOfRecordsNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // verticalRangeNumeric
            // 
            this.verticalRangeNumeric.DecimalPlaces = 2;
            this.verticalRangeNumeric.Location = new System.Drawing.Point(123, 45);
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
            this.verticalRangeNumeric.TabIndex = 1;
            this.verticalRangeNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // minSampleRateNumeric
            // 
            this.minSampleRateNumeric.DecimalPlaces = 2;
            this.minSampleRateNumeric.Location = new System.Drawing.Point(123, 71);
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
            this.minSampleRateNumeric.TabIndex = 2;
            this.minSampleRateNumeric.Value = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            // 
            // minRecordLengthNumeric
            // 
            this.minRecordLengthNumeric.Location = new System.Drawing.Point(123, 97);
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
            this.minRecordLengthNumeric.TabIndex = 3;
            this.minRecordLengthNumeric.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(81, 19);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // verticalAndHorizontalGroupBox
            // 
            this.verticalAndHorizontalGroupBox.Controls.Add(this.numOfRecordsNumeric);
            this.verticalAndHorizontalGroupBox.Controls.Add(this.verticalRangeNumeric);
            this.verticalAndHorizontalGroupBox.Controls.Add(this.minSampleRateNumeric);
            this.verticalAndHorizontalGroupBox.Controls.Add(this.minRecordLengthNumeric);
            this.verticalAndHorizontalGroupBox.Controls.Add(this.numOfRecLabel);
            this.verticalAndHorizontalGroupBox.Controls.Add(this.verticalRangeLabel);
            this.verticalAndHorizontalGroupBox.Controls.Add(this.minSampleRateLabel);
            this.verticalAndHorizontalGroupBox.Controls.Add(this.minRecordLengthLabel);
            this.verticalAndHorizontalGroupBox.Location = new System.Drawing.Point(12, 120);
            this.verticalAndHorizontalGroupBox.Name = "verticalAndHorizontalGroupBox";
            this.verticalAndHorizontalGroupBox.Size = new System.Drawing.Size(229, 124);
            this.verticalAndHorizontalGroupBox.TabIndex = 1;
            this.verticalAndHorizontalGroupBox.TabStop = false;
            this.verticalAndHorizontalGroupBox.Text = "Vertical and horizontal";
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.plotRelativeToLabel);
            this.generalGroupBox.Controls.Add(this.plotRelativeToComboBox);
            this.generalGroupBox.Controls.Add(this.channelNameTextBox);
            this.generalGroupBox.Controls.Add(this.resourceNameComboBox);
            this.generalGroupBox.Controls.Add(this.channelNameLabel);
            this.generalGroupBox.Controls.Add(this.resourceNameLabel);
            this.generalGroupBox.Location = new System.Drawing.Point(12, 12);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(229, 97);
            this.generalGroupBox.TabIndex = 0;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            // 
            // plotRelativeToLabel
            // 
            this.plotRelativeToLabel.AutoSize = true;
            this.plotRelativeToLabel.Location = new System.Drawing.Point(6, 76);
            this.plotRelativeToLabel.Name = "plotRelativeToLabel";
            this.plotRelativeToLabel.Size = new System.Drawing.Size(86, 13);
            this.plotRelativeToLabel.TabIndex = 19;
            this.plotRelativeToLabel.Text = "Plot Relative To:";
            // 
            // plotRelativeToComboBox
            // 
            this.plotRelativeToComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.plotRelativeToComboBox.FormattingEnabled = true;
            this.plotRelativeToComboBox.Location = new System.Drawing.Point(123, 72);
            this.plotRelativeToComboBox.Name = "plotRelativeToComboBox";
            this.plotRelativeToComboBox.Size = new System.Drawing.Size(100, 21);
            this.plotRelativeToComboBox.TabIndex = 2;
            // 
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(123, 46);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.channelNameTextBox.TabIndex = 1;
            this.channelNameTextBox.Text = "0";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(123, 19);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(100, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // channelNameLabel
            // 
            this.channelNameLabel.AutoSize = true;
            this.channelNameLabel.Location = new System.Drawing.Point(6, 50);
            this.channelNameLabel.Name = "channelNameLabel";
            this.channelNameLabel.Size = new System.Drawing.Size(80, 13);
            this.channelNameLabel.TabIndex = 14;
            this.channelNameLabel.Text = "Channel Name:";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 23);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 15;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // sampledDataGroupBox
            // 
            this.sampledDataGroupBox.Controls.Add(this.sampledDataGridView);
            this.sampledDataGroupBox.Location = new System.Drawing.Point(247, 12);
            this.sampledDataGroupBox.Name = "sampledDataGroupBox";
            this.sampledDataGroupBox.Size = new System.Drawing.Size(327, 292);
            this.sampledDataGroupBox.TabIndex = 3;
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
            this.sampledDataGridView.Size = new System.Drawing.Size(315, 267);
            this.sampledDataGridView.StandardTab = true;
            this.sampledDataGridView.TabIndex = 0;
            // 
            // buttonsGroupBox
            // 
            this.buttonsGroupBox.Controls.Add(this.acquireButton);
            this.buttonsGroupBox.Location = new System.Drawing.Point(12, 250);
            this.buttonsGroupBox.Name = "buttonsGroupBox";
            this.buttonsGroupBox.Size = new System.Drawing.Size(229, 54);
            this.buttonsGroupBox.TabIndex = 2;
            this.buttonsGroupBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 316);
            this.Controls.Add(this.buttonsGroupBox);
            this.Controls.Add(this.sampledDataGroupBox);
            this.Controls.Add(this.verticalAndHorizontalGroupBox);
            this.Controls.Add(this.generalGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Multiple Record";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.numOfRecordsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minSampleRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minRecordLengthNumeric)).EndInit();
            this.verticalAndHorizontalGroupBox.ResumeLayout(false);
            this.verticalAndHorizontalGroupBox.PerformLayout();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.sampledDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sampledDataGridView)).EndInit();
            this.buttonsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label numOfRecLabel;
        private System.Windows.Forms.Label verticalRangeLabel;
        private System.Windows.Forms.Label minSampleRateLabel;
        private System.Windows.Forms.Label minRecordLengthLabel;
        private System.Windows.Forms.NumericUpDown numOfRecordsNumeric;
        private System.Windows.Forms.NumericUpDown verticalRangeNumeric;
        private System.Windows.Forms.NumericUpDown minSampleRateNumeric;
        private System.Windows.Forms.NumericUpDown minRecordLengthNumeric;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.GroupBox verticalAndHorizontalGroupBox;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.Label plotRelativeToLabel;
        private System.Windows.Forms.ComboBox plotRelativeToComboBox;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.GroupBox sampledDataGroupBox;
        private System.Windows.Forms.DataGridView sampledDataGridView;
        private System.Windows.Forms.GroupBox buttonsGroupBox;


    }
}
