namespace NationalInstruments.Examples.FetchForever
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
            this.maxPointsFetchedLabel = new System.Windows.Forms.Label();
            this.totalPointsFetchedLabel = new System.Windows.Forms.Label();
            this.lastfetchpointsLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.actualSamplesFromLastFetchedLabel = new System.Windows.Forms.Label();
            this.verticalRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.maxPointsFetchedNumeric = new System.Windows.Forms.NumericUpDown();
            this.totalPointsFetchedTextBox = new System.Windows.Forms.TextBox();
            this.lastFetchedPointsTextBox = new System.Windows.Forms.TextBox();
            this.acquireButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.horizontalAndVerticalGroupBox = new System.Windows.Forms.GroupBox();
            this.minSampleRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.fetchPointsGroupBox = new System.Windows.Forms.GroupBox();
            this.waveformDataGroupBox = new System.Windows.Forms.GroupBox();
            this.waveformDataGridView = new System.Windows.Forms.DataGridView();
            this.messageGroupBox = new System.Windows.Forms.GroupBox();
            this.messageTextBox = new System.Windows.Forms.RichTextBox();
            this.buttonsGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxPointsFetchedNumeric)).BeginInit();
            this.horizontalAndVerticalGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minSampleRateNumeric)).BeginInit();
            this.generalGroupBox.SuspendLayout();
            this.fetchPointsGroupBox.SuspendLayout();
            this.waveformDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.waveformDataGridView)).BeginInit();
            this.messageGroupBox.SuspendLayout();
            this.buttonsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelNameLabel
            // 
            this.channelNameLabel.AutoSize = true;
            this.channelNameLabel.Location = new System.Drawing.Point(6, 50);
            this.channelNameLabel.Name = "channelNameLabel";
            this.channelNameLabel.Size = new System.Drawing.Size(80, 13);
            this.channelNameLabel.TabIndex = 0;
            this.channelNameLabel.Text = "Channel Name:";
            // 
            // minSampleRateLabel
            // 
            this.minSampleRateLabel.AutoSize = true;
            this.minSampleRateLabel.Location = new System.Drawing.Point(6, 23);
            this.minSampleRateLabel.Name = "minSampleRateLabel";
            this.minSampleRateLabel.Size = new System.Drawing.Size(91, 13);
            this.minSampleRateLabel.TabIndex = 1;
            this.minSampleRateLabel.Text = "Min Sample Rate:";
            // 
            // verticalRangeLabel
            // 
            this.verticalRangeLabel.AutoSize = true;
            this.verticalRangeLabel.Location = new System.Drawing.Point(6, 49);
            this.verticalRangeLabel.Name = "verticalRangeLabel";
            this.verticalRangeLabel.Size = new System.Drawing.Size(80, 13);
            this.verticalRangeLabel.TabIndex = 2;
            this.verticalRangeLabel.Text = "Vertical Range:";
            // 
            // maxPointsFetchedLabel
            // 
            this.maxPointsFetchedLabel.AutoSize = true;
            this.maxPointsFetchedLabel.Location = new System.Drawing.Point(6, 23);
            this.maxPointsFetchedLabel.Name = "maxPointsFetchedLabel";
            this.maxPointsFetchedLabel.Size = new System.Drawing.Size(106, 13);
            this.maxPointsFetchedLabel.TabIndex = 4;
            this.maxPointsFetchedLabel.Text = "Max points per fetch:";
            // 
            // totalPointsFetchedLabel
            // 
            this.totalPointsFetchedLabel.AutoSize = true;
            this.totalPointsFetchedLabel.Location = new System.Drawing.Point(6, 49);
            this.totalPointsFetchedLabel.Name = "totalPointsFetchedLabel";
            this.totalPointsFetchedLabel.Size = new System.Drawing.Size(104, 13);
            this.totalPointsFetchedLabel.TabIndex = 5;
            this.totalPointsFetchedLabel.Text = "Total points fetched:";
            // 
            // lastfetchpointsLabel
            // 
            this.lastfetchpointsLabel.AutoSize = true;
            this.lastfetchpointsLabel.Location = new System.Drawing.Point(-34, 277);
            this.lastfetchpointsLabel.Name = "lastfetchpointsLabel";
            this.lastfetchpointsLabel.Size = new System.Drawing.Size(0, 13);
            this.lastfetchpointsLabel.TabIndex = 6;
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 23);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 11;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // actualSamplesFromLastFetchedLabel
            // 
            this.actualSamplesFromLastFetchedLabel.AutoSize = true;
            this.actualSamplesFromLastFetchedLabel.Location = new System.Drawing.Point(6, 68);
            this.actualSamplesFromLastFetchedLabel.Name = "actualSamplesFromLastFetchedLabel";
            this.actualSamplesFromLastFetchedLabel.Size = new System.Drawing.Size(101, 26);
            this.actualSamplesFromLastFetchedLabel.TabIndex = 17;
            this.actualSamplesFromLastFetchedLabel.Text = "Actual samples from\r\nlast fetch:";
            // 
            // verticalRangeNumeric
            // 
            this.verticalRangeNumeric.DecimalPlaces = 2;
            this.verticalRangeNumeric.Location = new System.Drawing.Point(125, 45);
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
            2,
            0,
            0,
            0});
            // 
            // maxPointsFetchedNumeric
            // 
            this.maxPointsFetchedNumeric.Location = new System.Drawing.Point(125, 19);
            this.maxPointsFetchedNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.maxPointsFetchedNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.maxPointsFetchedNumeric.Name = "maxPointsFetchedNumeric";
            this.maxPointsFetchedNumeric.Size = new System.Drawing.Size(100, 20);
            this.maxPointsFetchedNumeric.TabIndex = 0;
            this.maxPointsFetchedNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // totalPointsFetchedTextBox
            // 
            this.totalPointsFetchedTextBox.Location = new System.Drawing.Point(125, 45);
            this.totalPointsFetchedTextBox.Name = "totalPointsFetchedTextBox";
            this.totalPointsFetchedTextBox.ReadOnly = true;
            this.totalPointsFetchedTextBox.Size = new System.Drawing.Size(100, 20);
            this.totalPointsFetchedTextBox.TabIndex = 1;
            this.totalPointsFetchedTextBox.Text = "0";
            // 
            // lastFetchedPointsTextBox
            // 
            this.lastFetchedPointsTextBox.Location = new System.Drawing.Point(125, 71);
            this.lastFetchedPointsTextBox.Name = "lastFetchedPointsTextBox";
            this.lastFetchedPointsTextBox.ReadOnly = true;
            this.lastFetchedPointsTextBox.Size = new System.Drawing.Size(100, 20);
            this.lastFetchedPointsTextBox.TabIndex = 2;
            this.lastFetchedPointsTextBox.Text = "0";
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(37, 19);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(118, 19);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "&Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // horizontalAndVerticalGroupBox
            // 
            this.horizontalAndVerticalGroupBox.Controls.Add(this.minSampleRateNumeric);
            this.horizontalAndVerticalGroupBox.Controls.Add(this.verticalRangeNumeric);
            this.horizontalAndVerticalGroupBox.Controls.Add(this.minSampleRateLabel);
            this.horizontalAndVerticalGroupBox.Controls.Add(this.verticalRangeLabel);
            this.horizontalAndVerticalGroupBox.Location = new System.Drawing.Point(12, 95);
            this.horizontalAndVerticalGroupBox.Name = "horizontalAndVerticalGroupBox";
            this.horizontalAndVerticalGroupBox.Size = new System.Drawing.Size(231, 72);
            this.horizontalAndVerticalGroupBox.TabIndex = 1;
            this.horizontalAndVerticalGroupBox.TabStop = false;
            this.horizontalAndVerticalGroupBox.Text = "Horizontal And Vertical";
            // 
            // minSampleRateNumeric
            // 
            this.minSampleRateNumeric.DecimalPlaces = 2;
            this.minSampleRateNumeric.Location = new System.Drawing.Point(125, 19);
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
            10000000,
            0,
            0,
            0});
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.channelNameTextBox);
            this.generalGroupBox.Controls.Add(this.resourceNameComboBox);
            this.generalGroupBox.Controls.Add(this.channelNameLabel);
            this.generalGroupBox.Controls.Add(this.resourceNameLabel);
            this.generalGroupBox.Location = new System.Drawing.Point(12, 12);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(231, 72);
            this.generalGroupBox.TabIndex = 0;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            // 
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(125, 46);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.channelNameTextBox.TabIndex = 1;
            this.channelNameTextBox.Text = "0";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(125, 19);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(100, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // fetchPointsGroupBox
            // 
            this.fetchPointsGroupBox.Controls.Add(this.totalPointsFetchedTextBox);
            this.fetchPointsGroupBox.Controls.Add(this.lastFetchedPointsTextBox);
            this.fetchPointsGroupBox.Controls.Add(this.maxPointsFetchedNumeric);
            this.fetchPointsGroupBox.Controls.Add(this.maxPointsFetchedLabel);
            this.fetchPointsGroupBox.Controls.Add(this.totalPointsFetchedLabel);
            this.fetchPointsGroupBox.Controls.Add(this.actualSamplesFromLastFetchedLabel);
            this.fetchPointsGroupBox.Location = new System.Drawing.Point(12, 178);
            this.fetchPointsGroupBox.Name = "fetchPointsGroupBox";
            this.fetchPointsGroupBox.Size = new System.Drawing.Size(231, 106);
            this.fetchPointsGroupBox.TabIndex = 2;
            this.fetchPointsGroupBox.TabStop = false;
            this.fetchPointsGroupBox.Text = "Fetch Points";
            // 
            // waveformDataGroupBox
            // 
            this.waveformDataGroupBox.Controls.Add(this.waveformDataGridView);
            this.waveformDataGroupBox.Location = new System.Drawing.Point(249, 12);
            this.waveformDataGroupBox.Name = "waveformDataGroupBox";
            this.waveformDataGroupBox.Size = new System.Drawing.Size(202, 403);
            this.waveformDataGroupBox.TabIndex = 5;
            this.waveformDataGroupBox.TabStop = false;
            this.waveformDataGroupBox.Text = "Waveform data";
            // 
            // waveformDataGridView
            // 
            this.waveformDataGridView.AllowUserToAddRows = false;
            this.waveformDataGridView.AllowUserToDeleteRows = false;
            this.waveformDataGridView.AllowUserToResizeRows = false;
            this.waveformDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.waveformDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.waveformDataGridView.Location = new System.Drawing.Point(6, 19);
            this.waveformDataGridView.Name = "waveformDataGridView";
            this.waveformDataGridView.ReadOnly = true;
            this.waveformDataGridView.RowHeadersVisible = false;
            this.waveformDataGridView.RowHeadersWidth = 15;
            this.waveformDataGridView.RowTemplate.Height = 24;
            this.waveformDataGridView.Size = new System.Drawing.Size(190, 378);
            this.waveformDataGridView.StandardTab = true;
            this.waveformDataGridView.TabIndex = 0;
            // 
            // messageGroupBox
            // 
            this.messageGroupBox.Controls.Add(this.messageTextBox);
            this.messageGroupBox.Location = new System.Drawing.Point(12, 295);
            this.messageGroupBox.Name = "messageGroupBox";
            this.messageGroupBox.Size = new System.Drawing.Size(231, 60);
            this.messageGroupBox.TabIndex = 3;
            this.messageGroupBox.TabStop = false;
            this.messageGroupBox.Text = "Message";
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(9, 19);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.Size = new System.Drawing.Size(216, 35);
            this.messageTextBox.TabIndex = 0;
            this.messageTextBox.Text = "";
            // 
            // buttonsGroupBox
            // 
            this.buttonsGroupBox.Controls.Add(this.acquireButton);
            this.buttonsGroupBox.Controls.Add(this.stopButton);
            this.buttonsGroupBox.Location = new System.Drawing.Point(12, 361);
            this.buttonsGroupBox.Name = "buttonsGroupBox";
            this.buttonsGroupBox.Size = new System.Drawing.Size(231, 54);
            this.buttonsGroupBox.TabIndex = 4;
            this.buttonsGroupBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 431);
            this.Controls.Add(this.buttonsGroupBox);
            this.Controls.Add(this.messageGroupBox);
            this.Controls.Add(this.waveformDataGroupBox);
            this.Controls.Add(this.lastfetchpointsLabel);
            this.Controls.Add(this.horizontalAndVerticalGroupBox);
            this.Controls.Add(this.generalGroupBox);
            this.Controls.Add(this.fetchPointsGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Fetch Forever";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxPointsFetchedNumeric)).EndInit();
            this.horizontalAndVerticalGroupBox.ResumeLayout(false);
            this.horizontalAndVerticalGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minSampleRateNumeric)).EndInit();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.fetchPointsGroupBox.ResumeLayout(false);
            this.fetchPointsGroupBox.PerformLayout();
            this.waveformDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.waveformDataGridView)).EndInit();
            this.messageGroupBox.ResumeLayout(false);
            this.buttonsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label minSampleRateLabel;
        private System.Windows.Forms.Label verticalRangeLabel;
        private System.Windows.Forms.Label maxPointsFetchedLabel;
        private System.Windows.Forms.Label totalPointsFetchedLabel;
        private System.Windows.Forms.Label lastfetchpointsLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label actualSamplesFromLastFetchedLabel;
        private System.Windows.Forms.NumericUpDown verticalRangeNumeric;
        private System.Windows.Forms.NumericUpDown maxPointsFetchedNumeric;
        private System.Windows.Forms.TextBox totalPointsFetchedTextBox;
        private System.Windows.Forms.TextBox lastFetchedPointsTextBox;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox horizontalAndVerticalGroupBox;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.GroupBox fetchPointsGroupBox;
        private System.Windows.Forms.NumericUpDown minSampleRateNumeric;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.GroupBox waveformDataGroupBox;
        private System.Windows.Forms.DataGridView waveformDataGridView;
        private System.Windows.Forms.GroupBox messageGroupBox;
        private System.Windows.Forms.RichTextBox messageTextBox;
        private System.Windows.Forms.GroupBox buttonsGroupBox;
    }
}
