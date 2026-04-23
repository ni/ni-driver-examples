namespace NationalInstruments.Examples.MeasureStepResponse
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.voltageLevelRangeLabel = new System.Windows.Forms.Label();
            this.currentLimitLabel = new System.Windows.Forms.Label();
            this.currentLimitRangeLabel = new System.Windows.Forms.Label();
            this.measureRecordLengthLabel = new System.Windows.Forms.Label();
            this.transientResponseLabel = new System.Windows.Forms.Label();
            this.voltageLevelRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentLimitRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.measureRecordLengthNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.transientResponseComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameAndChannelNameGroupBox = new System.Windows.Forms.GroupBox();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.apertureTimeLabel = new System.Windows.Forms.Label();
            this.apertureTimeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.voltageSetPointsLabel = new System.Windows.Forms.Label();
            this.voltageSetPointsDataGridView = new System.Windows.Forms.DataGridView();
            this.measurementsGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementsDataGridView = new System.Windows.Forms.DataGridView();
            this.readingNoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.measuredVoltageColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.measuredCurrentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.voltageStepPointsStepColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.voltageStepPointsValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.measureRecordLengthNumeric)).BeginInit();
            this.resourceNameAndChannelNameGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.apertureTimeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageSetPointsDataGridView)).BeginInit();
            this.measurementsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // voltageLevelRangeLabel
            // 
            this.voltageLevelRangeLabel.AutoSize = true;
            this.voltageLevelRangeLabel.Location = new System.Drawing.Point(6, 101);
            this.voltageLevelRangeLabel.Name = "voltageLevelRangeLabel";
            this.voltageLevelRangeLabel.Size = new System.Drawing.Size(123, 13);
            this.voltageLevelRangeLabel.TabIndex = 8;
            this.voltageLevelRangeLabel.Text = "Voltage Level Range (V)";
            // 
            // currentLimitLabel
            // 
            this.currentLimitLabel.AutoSize = true;
            this.currentLimitLabel.Location = new System.Drawing.Point(6, 127);
            this.currentLimitLabel.Name = "currentLimitLabel";
            this.currentLimitLabel.Size = new System.Drawing.Size(81, 13);
            this.currentLimitLabel.TabIndex = 9;
            this.currentLimitLabel.Text = "Current Limit (A)";
            // 
            // currentLimitRangeLabel
            // 
            this.currentLimitRangeLabel.AutoSize = true;
            this.currentLimitRangeLabel.Location = new System.Drawing.Point(6, 153);
            this.currentLimitRangeLabel.Name = "currentLimitRangeLabel";
            this.currentLimitRangeLabel.Size = new System.Drawing.Size(116, 13);
            this.currentLimitRangeLabel.TabIndex = 10;
            this.currentLimitRangeLabel.Text = "Current Limit Range (A)";
            // 
            // measureRecordLengthLabel
            // 
            this.measureRecordLengthLabel.AutoSize = true;
            this.measureRecordLengthLabel.Location = new System.Drawing.Point(6, 206);
            this.measureRecordLengthLabel.Name = "measureRecordLengthLabel";
            this.measureRecordLengthLabel.Size = new System.Drawing.Size(122, 13);
            this.measureRecordLengthLabel.TabIndex = 12;
            this.measureRecordLengthLabel.Text = "Measure Record Length";
            // 
            // transientResponseLabel
            // 
            this.transientResponseLabel.AutoSize = true;
            this.transientResponseLabel.Location = new System.Drawing.Point(6, 179);
            this.transientResponseLabel.Name = "transientResponseLabel";
            this.transientResponseLabel.Size = new System.Drawing.Size(102, 13);
            this.transientResponseLabel.TabIndex = 11;
            this.transientResponseLabel.Text = "Transient Response";
            // 
            // voltageLevelRangeNumeric
            // 
            this.voltageLevelRangeNumeric.DecimalPlaces = 6;
            this.voltageLevelRangeNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.voltageLevelRangeNumeric.Location = new System.Drawing.Point(140, 97);
            this.voltageLevelRangeNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.voltageLevelRangeNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.voltageLevelRangeNumeric.Name = "voltageLevelRangeNumeric";
            this.voltageLevelRangeNumeric.Size = new System.Drawing.Size(91, 20);
            this.voltageLevelRangeNumeric.TabIndex = 2;
            this.voltageLevelRangeNumeric.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // currentLimitNumeric
            // 
            this.currentLimitNumeric.DecimalPlaces = 6;
            this.currentLimitNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.currentLimitNumeric.Location = new System.Drawing.Point(140, 123);
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
            // currentLimitRangeNumeric
            // 
            this.currentLimitRangeNumeric.DecimalPlaces = 6;
            this.currentLimitRangeNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.currentLimitRangeNumeric.Location = new System.Drawing.Point(140, 149);
            this.currentLimitRangeNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.currentLimitRangeNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.currentLimitRangeNumeric.Name = "currentLimitRangeNumeric";
            this.currentLimitRangeNumeric.Size = new System.Drawing.Size(91, 20);
            this.currentLimitRangeNumeric.TabIndex = 4;
            this.currentLimitRangeNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // measureRecordLengthNumeric
            // 
            this.measureRecordLengthNumeric.Location = new System.Drawing.Point(140, 202);
            this.measureRecordLengthNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.measureRecordLengthNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.measureRecordLengthNumeric.Name = "measureRecordLengthNumeric";
            this.measureRecordLengthNumeric.Size = new System.Drawing.Size(91, 20);
            this.measureRecordLengthNumeric.TabIndex = 6;
            this.measureRecordLengthNumeric.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(363, 357);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // transientResponseComboBox
            // 
            this.transientResponseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.transientResponseComboBox.Location = new System.Drawing.Point(140, 175);
            this.transientResponseComboBox.Name = "transientResponseComboBox";
            this.transientResponseComboBox.Size = new System.Drawing.Size(91, 21);
            this.transientResponseComboBox.TabIndex = 5;
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
            this.channelNameTextBox.TabIndex = 1;
            this.channelNameTextBox.Text = "0";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(106, 18);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(125, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // channelNameLabel
            // 
            this.channelNameLabel.AutoSize = true;
            this.channelNameLabel.Location = new System.Drawing.Point(6, 49);
            this.channelNameLabel.Name = "channelNameLabel";
            this.channelNameLabel.Size = new System.Drawing.Size(77, 13);
            this.channelNameLabel.TabIndex = 3;
            this.channelNameLabel.Text = "Channel Name";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 22);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 2;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.apertureTimeLabel);
            this.configurationGroupBox.Controls.Add(this.apertureTimeNumericUpDown);
            this.configurationGroupBox.Controls.Add(this.transientResponseComboBox);
            this.configurationGroupBox.Controls.Add(this.voltageSetPointsLabel);
            this.configurationGroupBox.Controls.Add(this.transientResponseLabel);
            this.configurationGroupBox.Controls.Add(this.measureRecordLengthNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLimitRangeLabel);
            this.configurationGroupBox.Controls.Add(this.measureRecordLengthLabel);
            this.configurationGroupBox.Controls.Add(this.voltageSetPointsDataGridView);
            this.configurationGroupBox.Controls.Add(this.currentLimitLabel);
            this.configurationGroupBox.Controls.Add(this.voltageLevelRangeLabel);
            this.configurationGroupBox.Controls.Add(this.voltageLevelRangeNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLimitNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLimitRangeNumeric);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 101);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(240, 258);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            // 
            // apertureTimeLabel
            // 
            this.apertureTimeLabel.AutoSize = true;
            this.apertureTimeLabel.Location = new System.Drawing.Point(6, 232);
            this.apertureTimeLabel.Name = "apertureTimeLabel";
            this.apertureTimeLabel.Size = new System.Drawing.Size(87, 13);
            this.apertureTimeLabel.TabIndex = 13;
            this.apertureTimeLabel.Text = "Aperture Time (s)";
            // 
            // apertureTimeNumericUpDown
            // 
            this.apertureTimeNumericUpDown.DecimalPlaces = 6;
            this.apertureTimeNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            393216});
            this.apertureTimeNumericUpDown.Location = new System.Drawing.Point(140, 228);
            this.apertureTimeNumericUpDown.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.apertureTimeNumericUpDown.Name = "apertureTimeNumericUpDown";
            this.apertureTimeNumericUpDown.Size = new System.Drawing.Size(91, 20);
            this.apertureTimeNumericUpDown.TabIndex = 7;
            this.apertureTimeNumericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            393216});
            // 
            // voltageSetPointsLabel
            // 
            this.voltageSetPointsLabel.AutoSize = true;
            this.voltageSetPointsLabel.Location = new System.Drawing.Point(6, 17);
            this.voltageSetPointsLabel.Name = "voltageSetPointsLabel";
            this.voltageSetPointsLabel.Size = new System.Drawing.Size(106, 13);
            this.voltageSetPointsLabel.TabIndex = 0;
            this.voltageSetPointsLabel.Text = "Voltage Setpoints (V)";
            // 
            // voltageSetPointsDataGridView
            // 
            this.voltageSetPointsDataGridView.AllowUserToAddRows = false;
            this.voltageSetPointsDataGridView.AllowUserToDeleteRows = false;
            this.voltageSetPointsDataGridView.AllowUserToResizeColumns = false;
            this.voltageSetPointsDataGridView.AllowUserToResizeRows = false;
            this.voltageSetPointsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.voltageSetPointsDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.voltageSetPointsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.voltageSetPointsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.voltageSetPointsDataGridView.ColumnHeadersVisible = false;
            this.voltageSetPointsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.voltageStepPointsStepColumn,
            this.voltageStepPointsValueColumn});
            this.voltageSetPointsDataGridView.Location = new System.Drawing.Point(6, 33);
            this.voltageSetPointsDataGridView.Name = "voltageSetPointsDataGridView";
            this.voltageSetPointsDataGridView.RowHeadersVisible = false;
            this.voltageSetPointsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.voltageSetPointsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.voltageSetPointsDataGridView.Size = new System.Drawing.Size(145, 47);
            this.voltageSetPointsDataGridView.StandardTab = true;
            this.voltageSetPointsDataGridView.TabIndex = 1;
            // 
            // measurementsGroupBox
            // 
            this.measurementsGroupBox.Controls.Add(this.measurementsDataGridView);
            this.measurementsGroupBox.Location = new System.Drawing.Point(268, 12);
            this.measurementsGroupBox.Name = "measurementsGroupBox";
            this.measurementsGroupBox.Size = new System.Drawing.Size(265, 339);
            this.measurementsGroupBox.TabIndex = 3;
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
            this.measuredVoltageColumn,
            this.measuredCurrentColumn});
            this.measurementsDataGridView.Location = new System.Drawing.Point(6, 19);
            this.measurementsDataGridView.Name = "measurementsDataGridView";
            this.measurementsDataGridView.ReadOnly = true;
            this.measurementsDataGridView.RowHeadersVisible = false;
            this.measurementsDataGridView.RowHeadersWidth = 15;
            this.measurementsDataGridView.RowTemplate.Height = 24;
            this.measurementsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.measurementsDataGridView.Size = new System.Drawing.Size(253, 311);
            this.measurementsDataGridView.StandardTab = true;
            this.measurementsDataGridView.TabIndex = 0;
            // 
            // readingNoColumn
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            this.readingNoColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.readingNoColumn.HeaderText = "Point";
            this.readingNoColumn.Name = "readingNoColumn";
            this.readingNoColumn.ReadOnly = true;
            this.readingNoColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.readingNoColumn.Width = 50;
            // 
            // measuredVoltageColumn
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            this.measuredVoltageColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.measuredVoltageColumn.HeaderText = "Voltage (V)";
            this.measuredVoltageColumn.Name = "measuredVoltageColumn";
            this.measuredVoltageColumn.ReadOnly = true;
            this.measuredVoltageColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // measuredCurrentColumn
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            this.measuredCurrentColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.measuredCurrentColumn.HeaderText = "Current (A)";
            this.measuredCurrentColumn.Name = "measuredCurrentColumn";
            this.measuredCurrentColumn.ReadOnly = true;
            this.measuredCurrentColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // voltageStepPointsStepColumn
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.voltageStepPointsStepColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.voltageStepPointsStepColumn.Frozen = true;
            this.voltageStepPointsStepColumn.HeaderText = "Step";
            this.voltageStepPointsStepColumn.Name = "voltageStepPointsStepColumn";
            this.voltageStepPointsStepColumn.ReadOnly = true;
            this.voltageStepPointsStepColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.voltageStepPointsStepColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.voltageStepPointsStepColumn.Width = 42;
            // 
            // voltageStepPointsValueColumn
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.voltageStepPointsValueColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.voltageStepPointsValueColumn.HeaderText = "Value";
            this.voltageStepPointsValueColumn.Name = "voltageStepPointsValueColumn";
            this.voltageStepPointsValueColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.voltageStepPointsValueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 393);
            this.Controls.Add(this.measurementsGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.resourceNameAndChannelNameGroupBox);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Measure Step Response";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.measureRecordLengthNumeric)).EndInit();
            this.resourceNameAndChannelNameGroupBox.ResumeLayout(false);
            this.resourceNameAndChannelNameGroupBox.PerformLayout();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.apertureTimeNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageSetPointsDataGridView)).EndInit();
            this.measurementsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label voltageLevelRangeLabel;
        private System.Windows.Forms.Label currentLimitLabel;
        private System.Windows.Forms.Label currentLimitRangeLabel;
        private System.Windows.Forms.Label measureRecordLengthLabel;
        private System.Windows.Forms.Label transientResponseLabel;
        private System.Windows.Forms.NumericUpDown voltageLevelRangeNumeric;
        private System.Windows.Forms.NumericUpDown currentLimitNumeric;
        private System.Windows.Forms.NumericUpDown currentLimitRangeNumeric;
        private System.Windows.Forms.NumericUpDown measureRecordLengthNumeric;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ComboBox transientResponseComboBox;
        private System.Windows.Forms.GroupBox resourceNameAndChannelNameGroupBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox measurementsGroupBox;
        private System.Windows.Forms.DataGridView measurementsDataGridView;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.DataGridView voltageSetPointsDataGridView;
        private System.Windows.Forms.Label voltageSetPointsLabel;
        private System.Windows.Forms.Label apertureTimeLabel;
        private System.Windows.Forms.NumericUpDown apertureTimeNumericUpDown;
        private System.Windows.Forms.DataGridViewTextBoxColumn readingNoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn measuredVoltageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn measuredCurrentColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn voltageStepPointsStepColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn voltageStepPointsValueColumn;

    }
}
