namespace NationalInstruments.Examples.SoftwareTimedVoltageSweep
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
            this.pointsLabel = new System.Windows.Forms.Label();
            this.voltageLevelStartLabel = new System.Windows.Forms.Label();
            this.voltageLevelStopLabel = new System.Windows.Forms.Label();
            this.delayLabel = new System.Windows.Forms.Label();
            this.numberOfPointsNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageLevelStartNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageLevelStopNumeric = new System.Windows.Forms.NumericUpDown();
            this.sourceDelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.resourceNameAndChannelNameGroupBox = new System.Windows.Forms.GroupBox();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.currentLimitLabel = new System.Windows.Forms.Label();
            this.currentLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.measurementsDataGridView = new System.Windows.Forms.DataGridView();
            this.readingNoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.voltageMeasurementColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentMeasurementColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.measurementsGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfPointsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelStartNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelStopNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDelayNumeric)).BeginInit();
            this.resourceNameAndChannelNameGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).BeginInit();
            this.measurementsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pointsLabel
            // 
            this.pointsLabel.AutoSize = true;
            this.pointsLabel.Location = new System.Drawing.Point(6, 62);
            this.pointsLabel.Name = "pointsLabel";
            this.pointsLabel.Size = new System.Drawing.Size(36, 13);
            this.pointsLabel.TabIndex = 4;
            this.pointsLabel.Text = "Points";
            // 
            // voltageLevelStartLabel
            // 
            this.voltageLevelStartLabel.AutoSize = true;
            this.voltageLevelStartLabel.Location = new System.Drawing.Point(6, 97);
            this.voltageLevelStartLabel.Name = "voltageLevelStartLabel";
            this.voltageLevelStartLabel.Size = new System.Drawing.Size(113, 13);
            this.voltageLevelStartLabel.TabIndex = 6;
            this.voltageLevelStartLabel.Text = "Voltage Level Start (V)";
            // 
            // voltageLevelStopLabel
            // 
            this.voltageLevelStopLabel.AutoSize = true;
            this.voltageLevelStopLabel.Location = new System.Drawing.Point(6, 127);
            this.voltageLevelStopLabel.Name = "voltageLevelStopLabel";
            this.voltageLevelStopLabel.Size = new System.Drawing.Size(113, 13);
            this.voltageLevelStopLabel.TabIndex = 8;
            this.voltageLevelStopLabel.Text = "Voltage Level Stop (V)";
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new System.Drawing.Point(6, 153);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(85, 13);
            this.delayLabel.TabIndex = 10;
            this.delayLabel.Text = "Source Delay (s)";
            // 
            // numberOfPointsNumeric
            // 
            this.numberOfPointsNumeric.Location = new System.Drawing.Point(160, 60);
            this.numberOfPointsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numberOfPointsNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfPointsNumeric.Name = "numberOfPointsNumeric";
            this.numberOfPointsNumeric.Size = new System.Drawing.Size(90, 20);
            this.numberOfPointsNumeric.TabIndex = 5;
            this.numberOfPointsNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // voltageLevelStartNumeric
            // 
            this.voltageLevelStartNumeric.DecimalPlaces = 6;
            this.voltageLevelStartNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.voltageLevelStartNumeric.Location = new System.Drawing.Point(160, 90);
            this.voltageLevelStartNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.voltageLevelStartNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.voltageLevelStartNumeric.Name = "voltageLevelStartNumeric";
            this.voltageLevelStartNumeric.Size = new System.Drawing.Size(90, 20);
            this.voltageLevelStartNumeric.TabIndex = 7;
            this.voltageLevelStartNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // voltageLevelStopNumeric
            // 
            this.voltageLevelStopNumeric.DecimalPlaces = 6;
            this.voltageLevelStopNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.voltageLevelStopNumeric.Location = new System.Drawing.Point(161, 123);
            this.voltageLevelStopNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.voltageLevelStopNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.voltageLevelStopNumeric.Name = "voltageLevelStopNumeric";
            this.voltageLevelStopNumeric.Size = new System.Drawing.Size(90, 20);
            this.voltageLevelStopNumeric.TabIndex = 9;
            this.voltageLevelStopNumeric.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // sourceDelayNumeric
            // 
            this.sourceDelayNumeric.DecimalPlaces = 6;
            this.sourceDelayNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.sourceDelayNumeric.Location = new System.Drawing.Point(161, 149);
            this.sourceDelayNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.sourceDelayNumeric.Name = "sourceDelayNumeric";
            this.sourceDelayNumeric.Size = new System.Drawing.Size(90, 20);
            this.sourceDelayNumeric.TabIndex = 11;
            this.sourceDelayNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(103, 301);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // resourceNameAndChannelNameGroupBox
            // 
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.channelNameTextBox);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.resourceNameComboBox);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.channelNameLabel);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.resourceNameLabel);
            this.resourceNameAndChannelNameGroupBox.Location = new System.Drawing.Point(12, 12);
            this.resourceNameAndChannelNameGroupBox.Name = "resourceNameAndChannelNameGroupBox";
            this.resourceNameAndChannelNameGroupBox.Size = new System.Drawing.Size(257, 73);
            this.resourceNameAndChannelNameGroupBox.TabIndex = 0;
            this.resourceNameAndChannelNameGroupBox.TabStop = false;
            this.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name";
            // 
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(161, 45);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(90, 20);
            this.channelNameTextBox.TabIndex = 3;
            this.channelNameTextBox.Text = "0";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(126, 18);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(125, 21);
            this.resourceNameComboBox.TabIndex = 1;
            // 
            // channelNameLabel
            // 
            this.channelNameLabel.AutoSize = true;
            this.channelNameLabel.Location = new System.Drawing.Point(6, 49);
            this.channelNameLabel.Name = "channelNameLabel";
            this.channelNameLabel.Size = new System.Drawing.Size(77, 13);
            this.channelNameLabel.TabIndex = 2;
            this.channelNameLabel.Text = "Channel Name";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 22);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.voltageLevelStopLabel);
            this.configurationGroupBox.Controls.Add(this.voltageLevelStartLabel);
            this.configurationGroupBox.Controls.Add(this.delayLabel);
            this.configurationGroupBox.Controls.Add(this.pointsLabel);
            this.configurationGroupBox.Controls.Add(this.voltageLevelStopNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLimitLabel);
            this.configurationGroupBox.Controls.Add(this.sourceDelayNumeric);
            this.configurationGroupBox.Controls.Add(this.voltageLevelStartNumeric);
            this.configurationGroupBox.Controls.Add(this.numberOfPointsNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLimitNumeric);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 101);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(257, 179);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // currentLimitLabel
            // 
            this.currentLimitLabel.AutoSize = true;
            this.currentLimitLabel.Location = new System.Drawing.Point(6, 32);
            this.currentLimitLabel.Name = "currentLimitLabel";
            this.currentLimitLabel.Size = new System.Drawing.Size(81, 13);
            this.currentLimitLabel.TabIndex = 2;
            this.currentLimitLabel.Text = "Current Limit (A)";
            // 
            // currentLimitNumeric
            // 
            this.currentLimitNumeric.DecimalPlaces = 6;
            this.currentLimitNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.currentLimitNumeric.Location = new System.Drawing.Point(160, 30);
            this.currentLimitNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.currentLimitNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.currentLimitNumeric.Name = "currentLimitNumeric";
            this.currentLimitNumeric.Size = new System.Drawing.Size(91, 20);
            this.currentLimitNumeric.TabIndex = 3;
            this.currentLimitNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // measurementsDataGridView
            // 
            this.measurementsDataGridView.AllowUserToAddRows = false;
            this.measurementsDataGridView.AllowUserToDeleteRows = false;
            this.measurementsDataGridView.AllowUserToResizeRows = false;
            this.measurementsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.measurementsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.measurementsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.readingNoColumn,
            this.voltageMeasurementColumn,
            this.currentMeasurementColumn});
            this.measurementsDataGridView.Location = new System.Drawing.Point(6, 22);
            this.measurementsDataGridView.Name = "measurementsDataGridView";
            this.measurementsDataGridView.ReadOnly = true;
            this.measurementsDataGridView.RowHeadersVisible = false;
            this.measurementsDataGridView.RowHeadersWidth = 15;
            this.measurementsDataGridView.RowTemplate.Height = 24;
            this.measurementsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.measurementsDataGridView.Size = new System.Drawing.Size(253, 290);
            this.measurementsDataGridView.StandardTab = true;
            this.measurementsDataGridView.TabIndex = 0;
            // 
            // readingNoColumn
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.readingNoColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.readingNoColumn.HeaderText = "Point";
            this.readingNoColumn.Name = "readingNoColumn";
            this.readingNoColumn.ReadOnly = true;
            this.readingNoColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.readingNoColumn.Width = 40;
            // 
            // voltageMeasurementColumn
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.voltageMeasurementColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.voltageMeasurementColumn.HeaderText = "Voltage (V)";
            this.voltageMeasurementColumn.Name = "voltageMeasurementColumn";
            this.voltageMeasurementColumn.ReadOnly = true;
            this.voltageMeasurementColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.voltageMeasurementColumn.Width = 105;
            // 
            // currentMeasurementColumn
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.currentMeasurementColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.currentMeasurementColumn.HeaderText = "Current (A)";
            this.currentMeasurementColumn.Name = "currentMeasurementColumn";
            this.currentMeasurementColumn.ReadOnly = true;
            this.currentMeasurementColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.currentMeasurementColumn.Width = 105;
            // 
            // measurementsGroupBox
            // 
            this.measurementsGroupBox.Controls.Add(this.measurementsDataGridView);
            this.measurementsGroupBox.Location = new System.Drawing.Point(285, 12);
            this.measurementsGroupBox.Name = "measurementsGroupBox";
            this.measurementsGroupBox.Size = new System.Drawing.Size(265, 321);
            this.measurementsGroupBox.TabIndex = 3;
            this.measurementsGroupBox.TabStop = false;
            this.measurementsGroupBox.Text = "Measurements";
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 346);
            this.Controls.Add(this.measurementsGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.resourceNameAndChannelNameGroupBox);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Software-Timed Voltage Sweep";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.numberOfPointsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelStartNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelStopNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDelayNumeric)).EndInit();
            this.resourceNameAndChannelNameGroupBox.ResumeLayout(false);
            this.resourceNameAndChannelNameGroupBox.PerformLayout();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).EndInit();
            this.measurementsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label pointsLabel;
        private System.Windows.Forms.Label voltageLevelStartLabel;
        private System.Windows.Forms.Label voltageLevelStopLabel;
        private System.Windows.Forms.Label delayLabel;
        private System.Windows.Forms.NumericUpDown numberOfPointsNumeric;
        private System.Windows.Forms.NumericUpDown voltageLevelStartNumeric;
        private System.Windows.Forms.NumericUpDown voltageLevelStopNumeric;
        private System.Windows.Forms.NumericUpDown sourceDelayNumeric;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox resourceNameAndChannelNameGroupBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.Label currentLimitLabel;
        private System.Windows.Forms.NumericUpDown currentLimitNumeric;
        private System.Windows.Forms.DataGridView measurementsDataGridView;
        private System.Windows.Forms.GroupBox measurementsGroupBox;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn readingNoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn voltageMeasurementColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentMeasurementColumn;

    }
}
