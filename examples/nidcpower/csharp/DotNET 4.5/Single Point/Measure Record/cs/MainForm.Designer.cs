namespace NationalInstruments.Examples.MeasureRecord
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
            this.voltageLevelLabel = new System.Windows.Forms.Label();
            this.measureRecordLengthLabel = new System.Windows.Forms.Label();
            this.autoZeroLabel = new System.Windows.Forms.Label();
            this.voltageLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.measureRecordLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.autoZeroComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameAndChannelNameGroupBox = new System.Windows.Forms.GroupBox();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.isMeasureRecordFiniteCheckBox = new System.Windows.Forms.CheckBox();
            this.effectiveMeasurementRateGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementRateTextBox = new System.Windows.Forms.TextBox();
            this.measurementsGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementsDataGridView = new System.Windows.Forms.DataGridView();
            this.readingNoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.voltageMeasurementColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentMeasurementColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.measureRecordLengthNumeric)).BeginInit();
            this.resourceNameAndChannelNameGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            this.effectiveMeasurementRateGroupBox.SuspendLayout();
            this.measurementsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // voltageLevelLabel
            // 
            this.voltageLevelLabel.AutoSize = true;
            this.voltageLevelLabel.Location = new System.Drawing.Point(6, 26);
            this.voltageLevelLabel.Name = "voltageLevelLabel";
            this.voltageLevelLabel.Size = new System.Drawing.Size(88, 13);
            this.voltageLevelLabel.TabIndex = 0;
            this.voltageLevelLabel.Text = "Voltage Level (V)";
            // 
            // measureRecordLengthLabel
            // 
            this.measureRecordLengthLabel.AutoSize = true;
            this.measureRecordLengthLabel.Location = new System.Drawing.Point(6, 54);
            this.measureRecordLengthLabel.Name = "measureRecordLengthLabel";
            this.measureRecordLengthLabel.Size = new System.Drawing.Size(122, 13);
            this.measureRecordLengthLabel.TabIndex = 2;
            this.measureRecordLengthLabel.Text = "Measure Record Length";
            // 
            // autoZeroLabel
            // 
            this.autoZeroLabel.AutoSize = true;
            this.autoZeroLabel.Location = new System.Drawing.Point(6, 85);
            this.autoZeroLabel.Name = "autoZeroLabel";
            this.autoZeroLabel.Size = new System.Drawing.Size(54, 13);
            this.autoZeroLabel.TabIndex = 8;
            this.autoZeroLabel.Text = "Auto Zero";
            // 
            // voltageLevelNumeric
            // 
            this.voltageLevelNumeric.DecimalPlaces = 6;
            this.voltageLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.voltageLevelNumeric.Location = new System.Drawing.Point(140, 24);
            this.voltageLevelNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.voltageLevelNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.voltageLevelNumeric.Name = "voltageLevelNumeric";
            this.voltageLevelNumeric.Size = new System.Drawing.Size(91, 20);
            this.voltageLevelNumeric.TabIndex = 1;
            this.voltageLevelNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // measureRecordLengthNumeric
            // 
            this.measureRecordLengthNumeric.Location = new System.Drawing.Point(140, 52);
            this.measureRecordLengthNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.measureRecordLengthNumeric.Name = "measureRecordLengthNumeric";
            this.measureRecordLengthNumeric.Size = new System.Drawing.Size(91, 20);
            this.measureRecordLengthNumeric.TabIndex = 3;
            this.measureRecordLengthNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(57, 307);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(138, 307);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "&Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // autoZeroComboBox
            // 
            this.autoZeroComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autoZeroComboBox.Location = new System.Drawing.Point(140, 82);
            this.autoZeroComboBox.Name = "autoZeroComboBox";
            this.autoZeroComboBox.Size = new System.Drawing.Size(91, 21);
            this.autoZeroComboBox.TabIndex = 9;
            // 
            // resourceNameAndChannelNameGroupBox
            // 
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.channelNameTextBox);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.resourceNameComboBox);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.channelNameLabel);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.resourceNameLabel);
            this.resourceNameAndChannelNameGroupBox.Location = new System.Drawing.Point(12, 12);
            this.resourceNameAndChannelNameGroupBox.Name = "resourceNameAndChannelNameGroupBox";
            this.resourceNameAndChannelNameGroupBox.Size = new System.Drawing.Size(240, 73);
            this.resourceNameAndChannelNameGroupBox.TabIndex = 0;
            this.resourceNameAndChannelNameGroupBox.TabStop = false;
            this.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name";
            // 
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(140, 45);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(91, 20);
            this.channelNameTextBox.TabIndex = 3;
            this.channelNameTextBox.Text = "0";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(106, 18);
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
            this.configurationGroupBox.Controls.Add(this.autoZeroLabel);
            this.configurationGroupBox.Controls.Add(this.isMeasureRecordFiniteCheckBox);
            this.configurationGroupBox.Controls.Add(this.autoZeroComboBox);
            this.configurationGroupBox.Controls.Add(this.voltageLevelLabel);
            this.configurationGroupBox.Controls.Add(this.voltageLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.measureRecordLengthLabel);
            this.configurationGroupBox.Controls.Add(this.measureRecordLengthNumeric);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 101);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(240, 157);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // isMeasureRecordFiniteCheckBox
            // 
            this.isMeasureRecordFiniteCheckBox.AutoSize = true;
            this.isMeasureRecordFiniteCheckBox.Checked = true;
            this.isMeasureRecordFiniteCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isMeasureRecordFiniteCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.isMeasureRecordFiniteCheckBox.Location = new System.Drawing.Point(36, 120);
            this.isMeasureRecordFiniteCheckBox.Name = "isMeasureRecordFiniteCheckBox";
            this.isMeasureRecordFiniteCheckBox.Size = new System.Drawing.Size(150, 17);
            this.isMeasureRecordFiniteCheckBox.TabIndex = 10;
            this.isMeasureRecordFiniteCheckBox.Text = "Is Measure Record Finite?";
            this.isMeasureRecordFiniteCheckBox.UseVisualStyleBackColor = true;
            // 
            // effectiveMeasurementRateGroupBox
            // 
            this.effectiveMeasurementRateGroupBox.Controls.Add(this.measurementRateTextBox);
            this.effectiveMeasurementRateGroupBox.Location = new System.Drawing.Point(268, 12);
            this.effectiveMeasurementRateGroupBox.Name = "effectiveMeasurementRateGroupBox";
            this.effectiveMeasurementRateGroupBox.Size = new System.Drawing.Size(265, 45);
            this.effectiveMeasurementRateGroupBox.TabIndex = 4;
            this.effectiveMeasurementRateGroupBox.TabStop = false;
            this.effectiveMeasurementRateGroupBox.Text = "Effective Measurement Rate";
            // 
            // measurementRateTextBox
            // 
            this.measurementRateTextBox.Location = new System.Drawing.Point(6, 18);
            this.measurementRateTextBox.Name = "measurementRateTextBox";
            this.measurementRateTextBox.ReadOnly = true;
            this.measurementRateTextBox.Size = new System.Drawing.Size(131, 20);
            this.measurementRateTextBox.TabIndex = 0;
            this.measurementRateTextBox.Text = "0.000000E+000";
            // 
            // measurementsGroupBox
            // 
            this.measurementsGroupBox.Controls.Add(this.measurementsDataGridView);
            this.measurementsGroupBox.Location = new System.Drawing.Point(268, 73);
            this.measurementsGroupBox.Name = "measurementsGroupBox";
            this.measurementsGroupBox.Size = new System.Drawing.Size(265, 286);
            this.measurementsGroupBox.TabIndex = 5;
            this.measurementsGroupBox.TabStop = false;
            this.measurementsGroupBox.Text = "Measurements";
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
            this.measurementsDataGridView.Location = new System.Drawing.Point(6, 19);
            this.measurementsDataGridView.Name = "measurementsDataGridView";
            this.measurementsDataGridView.ReadOnly = true;
            this.measurementsDataGridView.RowHeadersVisible = false;
            this.measurementsDataGridView.RowHeadersWidth = 15;
            this.measurementsDataGridView.RowTemplate.Height = 24;
            this.measurementsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.measurementsDataGridView.Size = new System.Drawing.Size(253, 258);
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
            this.readingNoColumn.Width = 50;
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
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 367);
            this.Controls.Add(this.measurementsGroupBox);
            this.Controls.Add(this.effectiveMeasurementRateGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.resourceNameAndChannelNameGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Measure Record";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.measureRecordLengthNumeric)).EndInit();
            this.resourceNameAndChannelNameGroupBox.ResumeLayout(false);
            this.resourceNameAndChannelNameGroupBox.PerformLayout();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.effectiveMeasurementRateGroupBox.ResumeLayout(false);
            this.effectiveMeasurementRateGroupBox.PerformLayout();
            this.measurementsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label voltageLevelLabel;
        private System.Windows.Forms.Label measureRecordLengthLabel;
        private System.Windows.Forms.Label autoZeroLabel;
        private System.Windows.Forms.NumericUpDown voltageLevelNumeric;
        private System.Windows.Forms.NumericUpDown measureRecordLengthNumeric;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox autoZeroComboBox;
        private System.Windows.Forms.GroupBox resourceNameAndChannelNameGroupBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.CheckBox isMeasureRecordFiniteCheckBox;
        private System.Windows.Forms.GroupBox effectiveMeasurementRateGroupBox;
        private System.Windows.Forms.GroupBox measurementsGroupBox;
        private System.Windows.Forms.DataGridView measurementsDataGridView;
        private System.Windows.Forms.TextBox measurementRateTextBox;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn readingNoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn voltageMeasurementColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentMeasurementColumn;

    }
}
