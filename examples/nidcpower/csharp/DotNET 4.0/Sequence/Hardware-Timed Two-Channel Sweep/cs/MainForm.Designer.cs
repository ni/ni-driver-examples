namespace NationalInstruments.Examples.HardwareTimedTwoChannelSweep
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
            this.startButton = new System.Windows.Forms.Button();
            this.measurementsGroupBox = new System.Windows.Forms.GroupBox();
            this.selectPlotLabel = new System.Windows.Forms.Label();
            this.plotNameComboBox = new System.Windows.Forms.ComboBox();
            this.measurementsDataGridView = new System.Windows.Forms.DataGridView();
            this.readingNoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.measuredVoltageColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.measuredCurrentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.device0SenseLabel = new System.Windows.Forms.Label();
            this.device0VoltageLevelStopLabel = new System.Windows.Forms.Label();
            this.device0VoltageLevelStartLabel = new System.Windows.Forms.Label();
            this.device0SourceDelayLabel = new System.Windows.Forms.Label();
            this.device0NumberOfPlotsLabel = new System.Windows.Forms.Label();
            this.device0VoltageLevelStopNumeric = new System.Windows.Forms.NumericUpDown();
            this.device0CurrentLimitLabel = new System.Windows.Forms.Label();
            this.device0SourceDelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.device0VoltageLevelStartNumeric = new System.Windows.Forms.NumericUpDown();
            this.device0NumberOfPlotsNumeric = new System.Windows.Forms.NumericUpDown();
            this.device0CurrentLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.device0SenseComboBox = new System.Windows.Forms.ComboBox();
            this.device0GroupBox = new System.Windows.Forms.GroupBox();
            this.device0ChannelNameComboBox = new System.Windows.Forms.TextBox();
            this.device0ResourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.device0ChannelNameLabel = new System.Windows.Forms.Label();
            this.device0ResourceNameLabel = new System.Windows.Forms.Label();
            this.device1GroupBox = new System.Windows.Forms.GroupBox();
            this.device1ChannelNameComboBox = new System.Windows.Forms.TextBox();
            this.device1VoltageLevelStopLabel = new System.Windows.Forms.Label();
            this.device1SenseLabel = new System.Windows.Forms.Label();
            this.device1VoltageLevelStopNumeric = new System.Windows.Forms.NumericUpDown();
            this.device1ResourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.device1VoltageLevelStartLabel = new System.Windows.Forms.Label();
            this.device1VoltageLevelStartNumeric = new System.Windows.Forms.NumericUpDown();
            this.device1SourceDelayLabel = new System.Windows.Forms.Label();
            this.device1NumberOfPointsLabel = new System.Windows.Forms.Label();
            this.device1ChannelNameLabel = new System.Windows.Forms.Label();
            this.device1NumberOfPointsNumeric = new System.Windows.Forms.NumericUpDown();
            this.device1ResourceNameLabel = new System.Windows.Forms.Label();
            this.device1SourceDelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.device1CurrentLimitLabel = new System.Windows.Forms.Label();
            this.device1CurrentLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.device1SenseComboBox = new System.Windows.Forms.ComboBox();
            this.timeoutNumeric = new System.Windows.Forms.NumericUpDown();
            this.timeoutGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.device0VoltageLevelStopNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.device0SourceDelayNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.device0VoltageLevelStartNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.device0NumberOfPlotsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.device0CurrentLimitNumeric)).BeginInit();
            this.device0GroupBox.SuspendLayout();
            this.device1GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.device1VoltageLevelStopNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.device1VoltageLevelStartNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.device1NumberOfPointsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.device1SourceDelayNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.device1CurrentLimitNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumeric)).BeginInit();
            this.timeoutGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(108, 483);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // measurementsGroupBox
            // 
            this.measurementsGroupBox.Controls.Add(this.selectPlotLabel);
            this.measurementsGroupBox.Controls.Add(this.plotNameComboBox);
            this.measurementsGroupBox.Controls.Add(this.measurementsDataGridView);
            this.measurementsGroupBox.Location = new System.Drawing.Point(294, 78);
            this.measurementsGroupBox.Name = "measurementsGroupBox";
            this.measurementsGroupBox.Size = new System.Drawing.Size(265, 435);
            this.measurementsGroupBox.TabIndex = 3;
            this.measurementsGroupBox.TabStop = false;
            this.measurementsGroupBox.Text = "Measurements";
            // 
            // selectPlotLabel
            // 
            this.selectPlotLabel.AutoSize = true;
            this.selectPlotLabel.Location = new System.Drawing.Point(6, 23);
            this.selectPlotLabel.Name = "selectPlotLabel";
            this.selectPlotLabel.Size = new System.Drawing.Size(58, 13);
            this.selectPlotLabel.TabIndex = 2;
            this.selectPlotLabel.Text = "Select Plot";
            // 
            // plotNameComboBox
            // 
            this.plotNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.plotNameComboBox.FormattingEnabled = true;
            this.plotNameComboBox.Location = new System.Drawing.Point(68, 19);
            this.plotNameComboBox.Name = "plotNameComboBox";
            this.plotNameComboBox.Size = new System.Drawing.Size(191, 21);
            this.plotNameComboBox.TabIndex = 3;
            this.plotNameComboBox.SelectedIndexChanged += new System.EventHandler(this.plotNameComboBox_SelectedIndexChanged);
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
            this.measurementsDataGridView.Location = new System.Drawing.Point(6, 59);
            this.measurementsDataGridView.Name = "measurementsDataGridView";
            this.measurementsDataGridView.ReadOnly = true;
            this.measurementsDataGridView.RowHeadersVisible = false;
            this.measurementsDataGridView.RowHeadersWidth = 15;
            this.measurementsDataGridView.RowTemplate.Height = 24;
            this.measurementsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.measurementsDataGridView.Size = new System.Drawing.Size(253, 369);
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
            // measuredVoltageColumn
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.measuredVoltageColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.measuredVoltageColumn.HeaderText = "Voltage (V)";
            this.measuredVoltageColumn.Name = "measuredVoltageColumn";
            this.measuredVoltageColumn.ReadOnly = true;
            this.measuredVoltageColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // measuredCurrentColumn
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.measuredCurrentColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.measuredCurrentColumn.HeaderText = "Current (A)";
            this.measuredCurrentColumn.Name = "measuredCurrentColumn";
            this.measuredCurrentColumn.ReadOnly = true;
            this.measuredCurrentColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // device0SenseLabel
            // 
            this.device0SenseLabel.AutoSize = true;
            this.device0SenseLabel.Location = new System.Drawing.Point(6, 125);
            this.device0SenseLabel.Name = "device0SenseLabel";
            this.device0SenseLabel.Size = new System.Drawing.Size(37, 13);
            this.device0SenseLabel.TabIndex = 8;
            this.device0SenseLabel.Text = "Sense";
            // 
            // device0VoltageLevelStopLabel
            // 
            this.device0VoltageLevelStopLabel.AutoSize = true;
            this.device0VoltageLevelStopLabel.Location = new System.Drawing.Point(147, 175);
            this.device0VoltageLevelStopLabel.Name = "device0VoltageLevelStopLabel";
            this.device0VoltageLevelStopLabel.Size = new System.Drawing.Size(113, 13);
            this.device0VoltageLevelStopLabel.TabIndex = 14;
            this.device0VoltageLevelStopLabel.Text = "Voltage Level Stop (V)";
            // 
            // device0VoltageLevelStartLabel
            // 
            this.device0VoltageLevelStartLabel.AutoSize = true;
            this.device0VoltageLevelStartLabel.Location = new System.Drawing.Point(147, 125);
            this.device0VoltageLevelStartLabel.Name = "device0VoltageLevelStartLabel";
            this.device0VoltageLevelStartLabel.Size = new System.Drawing.Size(113, 13);
            this.device0VoltageLevelStartLabel.TabIndex = 10;
            this.device0VoltageLevelStartLabel.Text = "Voltage Level Start (V)";
            // 
            // device0SourceDelayLabel
            // 
            this.device0SourceDelayLabel.AutoSize = true;
            this.device0SourceDelayLabel.Location = new System.Drawing.Point(6, 175);
            this.device0SourceDelayLabel.Name = "device0SourceDelayLabel";
            this.device0SourceDelayLabel.Size = new System.Drawing.Size(85, 13);
            this.device0SourceDelayLabel.TabIndex = 12;
            this.device0SourceDelayLabel.Text = "Source Delay (s)";
            // 
            // device0NumberOfPlotsLabel
            // 
            this.device0NumberOfPlotsLabel.AutoSize = true;
            this.device0NumberOfPlotsLabel.Location = new System.Drawing.Point(147, 76);
            this.device0NumberOfPlotsLabel.Name = "device0NumberOfPlotsLabel";
            this.device0NumberOfPlotsLabel.Size = new System.Drawing.Size(30, 13);
            this.device0NumberOfPlotsLabel.TabIndex = 6;
            this.device0NumberOfPlotsLabel.Text = "Plots";
            // 
            // device0VoltageLevelStopNumeric
            // 
            this.device0VoltageLevelStopNumeric.DecimalPlaces = 6;
            this.device0VoltageLevelStopNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.device0VoltageLevelStopNumeric.Location = new System.Drawing.Point(147, 191);
            this.device0VoltageLevelStopNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.device0VoltageLevelStopNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.device0VoltageLevelStopNumeric.Name = "device0VoltageLevelStopNumeric";
            this.device0VoltageLevelStopNumeric.Size = new System.Drawing.Size(100, 20);
            this.device0VoltageLevelStopNumeric.TabIndex = 15;
            this.device0VoltageLevelStopNumeric.Value = new decimal(new int[] {
            39,
            0,
            0,
            65536});
            // 
            // device0CurrentLimitLabel
            // 
            this.device0CurrentLimitLabel.AutoSize = true;
            this.device0CurrentLimitLabel.Location = new System.Drawing.Point(6, 76);
            this.device0CurrentLimitLabel.Name = "device0CurrentLimitLabel";
            this.device0CurrentLimitLabel.Size = new System.Drawing.Size(81, 13);
            this.device0CurrentLimitLabel.TabIndex = 4;
            this.device0CurrentLimitLabel.Text = "Current Limit (A)";
            // 
            // device0SourceDelayNumeric
            // 
            this.device0SourceDelayNumeric.DecimalPlaces = 6;
            this.device0SourceDelayNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.device0SourceDelayNumeric.Location = new System.Drawing.Point(6, 191);
            this.device0SourceDelayNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.device0SourceDelayNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.device0SourceDelayNumeric.Name = "device0SourceDelayNumeric";
            this.device0SourceDelayNumeric.Size = new System.Drawing.Size(100, 20);
            this.device0SourceDelayNumeric.TabIndex = 13;
            this.device0SourceDelayNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            196608});
            // 
            // device0VoltageLevelStartNumeric
            // 
            this.device0VoltageLevelStartNumeric.DecimalPlaces = 6;
            this.device0VoltageLevelStartNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.device0VoltageLevelStartNumeric.Location = new System.Drawing.Point(147, 141);
            this.device0VoltageLevelStartNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.device0VoltageLevelStartNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.device0VoltageLevelStartNumeric.Name = "device0VoltageLevelStartNumeric";
            this.device0VoltageLevelStartNumeric.Size = new System.Drawing.Size(100, 20);
            this.device0VoltageLevelStartNumeric.TabIndex = 11;
            this.device0VoltageLevelStartNumeric.Value = new decimal(new int[] {
            35,
            0,
            0,
            65536});
            // 
            // device0NumberOfPlotsNumeric
            // 
            this.device0NumberOfPlotsNumeric.Location = new System.Drawing.Point(147, 92);
            this.device0NumberOfPlotsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.device0NumberOfPlotsNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.device0NumberOfPlotsNumeric.Name = "device0NumberOfPlotsNumeric";
            this.device0NumberOfPlotsNumeric.Size = new System.Drawing.Size(100, 20);
            this.device0NumberOfPlotsNumeric.TabIndex = 7;
            this.device0NumberOfPlotsNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // device0CurrentLimitNumeric
            // 
            this.device0CurrentLimitNumeric.DecimalPlaces = 6;
            this.device0CurrentLimitNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.device0CurrentLimitNumeric.Location = new System.Drawing.Point(6, 92);
            this.device0CurrentLimitNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.device0CurrentLimitNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.device0CurrentLimitNumeric.Name = "device0CurrentLimitNumeric";
            this.device0CurrentLimitNumeric.Size = new System.Drawing.Size(100, 20);
            this.device0CurrentLimitNumeric.TabIndex = 5;
            this.device0CurrentLimitNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // device0SenseComboBox
            // 
            this.device0SenseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device0SenseComboBox.Location = new System.Drawing.Point(6, 141);
            this.device0SenseComboBox.Name = "device0SenseComboBox";
            this.device0SenseComboBox.Size = new System.Drawing.Size(100, 21);
            this.device0SenseComboBox.TabIndex = 9;
            // 
            // device0GroupBox
            // 
            this.device0GroupBox.Controls.Add(this.device0ChannelNameComboBox);
            this.device0GroupBox.Controls.Add(this.device0VoltageLevelStopLabel);
            this.device0GroupBox.Controls.Add(this.device0SenseLabel);
            this.device0GroupBox.Controls.Add(this.device0VoltageLevelStopNumeric);
            this.device0GroupBox.Controls.Add(this.device0ResourceNameComboBox);
            this.device0GroupBox.Controls.Add(this.device0VoltageLevelStartLabel);
            this.device0GroupBox.Controls.Add(this.device0VoltageLevelStartNumeric);
            this.device0GroupBox.Controls.Add(this.device0SourceDelayLabel);
            this.device0GroupBox.Controls.Add(this.device0NumberOfPlotsLabel);
            this.device0GroupBox.Controls.Add(this.device0ChannelNameLabel);
            this.device0GroupBox.Controls.Add(this.device0NumberOfPlotsNumeric);
            this.device0GroupBox.Controls.Add(this.device0ResourceNameLabel);
            this.device0GroupBox.Controls.Add(this.device0SourceDelayNumeric);
            this.device0GroupBox.Controls.Add(this.device0CurrentLimitLabel);
            this.device0GroupBox.Controls.Add(this.device0CurrentLimitNumeric);
            this.device0GroupBox.Controls.Add(this.device0SenseComboBox);
            this.device0GroupBox.Location = new System.Drawing.Point(12, 12);
            this.device0GroupBox.Name = "device0GroupBox";
            this.device0GroupBox.Size = new System.Drawing.Size(266, 220);
            this.device0GroupBox.TabIndex = 0;
            this.device0GroupBox.TabStop = false;
            this.device0GroupBox.Text = "Device 0:  Gate Device";
            // 
            // device0ChannelNameComboBox
            // 
            this.device0ChannelNameComboBox.Location = new System.Drawing.Point(147, 39);
            this.device0ChannelNameComboBox.Name = "device0ChannelNameComboBox";
            this.device0ChannelNameComboBox.Size = new System.Drawing.Size(100, 20);
            this.device0ChannelNameComboBox.TabIndex = 3;
            this.device0ChannelNameComboBox.Text = "0";
            // 
            // device0ResourceNameComboBox
            // 
            this.device0ResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device0ResourceNameComboBox.FormattingEnabled = true;
            this.device0ResourceNameComboBox.Location = new System.Drawing.Point(9, 39);
            this.device0ResourceNameComboBox.Name = "device0ResourceNameComboBox";
            this.device0ResourceNameComboBox.Size = new System.Drawing.Size(97, 21);
            this.device0ResourceNameComboBox.TabIndex = 1;
            // 
            // device0ChannelNameLabel
            // 
            this.device0ChannelNameLabel.AutoSize = true;
            this.device0ChannelNameLabel.Location = new System.Drawing.Point(144, 23);
            this.device0ChannelNameLabel.Name = "device0ChannelNameLabel";
            this.device0ChannelNameLabel.Size = new System.Drawing.Size(77, 13);
            this.device0ChannelNameLabel.TabIndex = 2;
            this.device0ChannelNameLabel.Text = "Channel Name";
            // 
            // device0ResourceNameLabel
            // 
            this.device0ResourceNameLabel.AutoSize = true;
            this.device0ResourceNameLabel.Location = new System.Drawing.Point(6, 23);
            this.device0ResourceNameLabel.Name = "device0ResourceNameLabel";
            this.device0ResourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.device0ResourceNameLabel.TabIndex = 0;
            this.device0ResourceNameLabel.Text = "Resource Name";
            // 
            // device1GroupBox
            // 
            this.device1GroupBox.Controls.Add(this.device1ChannelNameComboBox);
            this.device1GroupBox.Controls.Add(this.device1VoltageLevelStopLabel);
            this.device1GroupBox.Controls.Add(this.device1SenseLabel);
            this.device1GroupBox.Controls.Add(this.device1VoltageLevelStopNumeric);
            this.device1GroupBox.Controls.Add(this.device1ResourceNameComboBox);
            this.device1GroupBox.Controls.Add(this.device1VoltageLevelStartLabel);
            this.device1GroupBox.Controls.Add(this.device1VoltageLevelStartNumeric);
            this.device1GroupBox.Controls.Add(this.device1SourceDelayLabel);
            this.device1GroupBox.Controls.Add(this.device1NumberOfPointsLabel);
            this.device1GroupBox.Controls.Add(this.device1ChannelNameLabel);
            this.device1GroupBox.Controls.Add(this.device1NumberOfPointsNumeric);
            this.device1GroupBox.Controls.Add(this.device1ResourceNameLabel);
            this.device1GroupBox.Controls.Add(this.device1SourceDelayNumeric);
            this.device1GroupBox.Controls.Add(this.device1CurrentLimitLabel);
            this.device1GroupBox.Controls.Add(this.device1CurrentLimitNumeric);
            this.device1GroupBox.Controls.Add(this.device1SenseComboBox);
            this.device1GroupBox.Location = new System.Drawing.Point(12, 248);
            this.device1GroupBox.Name = "device1GroupBox";
            this.device1GroupBox.Size = new System.Drawing.Size(266, 219);
            this.device1GroupBox.TabIndex = 1;
            this.device1GroupBox.TabStop = false;
            this.device1GroupBox.Text = "Device 1: Drain Device";
            // 
            // device1ChannelNameComboBox
            // 
            this.device1ChannelNameComboBox.Location = new System.Drawing.Point(147, 39);
            this.device1ChannelNameComboBox.Name = "device1ChannelNameComboBox";
            this.device1ChannelNameComboBox.Size = new System.Drawing.Size(100, 20);
            this.device1ChannelNameComboBox.TabIndex = 3;
            this.device1ChannelNameComboBox.Text = "0";
            // 
            // device1VoltageLevelStopLabel
            // 
            this.device1VoltageLevelStopLabel.AutoSize = true;
            this.device1VoltageLevelStopLabel.Location = new System.Drawing.Point(147, 174);
            this.device1VoltageLevelStopLabel.Name = "device1VoltageLevelStopLabel";
            this.device1VoltageLevelStopLabel.Size = new System.Drawing.Size(113, 13);
            this.device1VoltageLevelStopLabel.TabIndex = 14;
            this.device1VoltageLevelStopLabel.Text = "Voltage Level Stop (V)";
            // 
            // device1SenseLabel
            // 
            this.device1SenseLabel.AutoSize = true;
            this.device1SenseLabel.Location = new System.Drawing.Point(6, 124);
            this.device1SenseLabel.Name = "device1SenseLabel";
            this.device1SenseLabel.Size = new System.Drawing.Size(37, 13);
            this.device1SenseLabel.TabIndex = 8;
            this.device1SenseLabel.Text = "Sense";
            // 
            // device1VoltageLevelStopNumeric
            // 
            this.device1VoltageLevelStopNumeric.DecimalPlaces = 6;
            this.device1VoltageLevelStopNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.device1VoltageLevelStopNumeric.Location = new System.Drawing.Point(147, 190);
            this.device1VoltageLevelStopNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.device1VoltageLevelStopNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.device1VoltageLevelStopNumeric.Name = "device1VoltageLevelStopNumeric";
            this.device1VoltageLevelStopNumeric.Size = new System.Drawing.Size(100, 20);
            this.device1VoltageLevelStopNumeric.TabIndex = 15;
            this.device1VoltageLevelStopNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // device1ResourceNameComboBox
            // 
            this.device1ResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device1ResourceNameComboBox.FormattingEnabled = true;
            this.device1ResourceNameComboBox.Location = new System.Drawing.Point(6, 39);
            this.device1ResourceNameComboBox.Name = "device1ResourceNameComboBox";
            this.device1ResourceNameComboBox.Size = new System.Drawing.Size(100, 21);
            this.device1ResourceNameComboBox.TabIndex = 1;
            // 
            // device1VoltageLevelStartLabel
            // 
            this.device1VoltageLevelStartLabel.AutoSize = true;
            this.device1VoltageLevelStartLabel.Location = new System.Drawing.Point(147, 124);
            this.device1VoltageLevelStartLabel.Name = "device1VoltageLevelStartLabel";
            this.device1VoltageLevelStartLabel.Size = new System.Drawing.Size(113, 13);
            this.device1VoltageLevelStartLabel.TabIndex = 10;
            this.device1VoltageLevelStartLabel.Text = "Voltage Level Start (V)";
            // 
            // device1VoltageLevelStartNumeric
            // 
            this.device1VoltageLevelStartNumeric.DecimalPlaces = 6;
            this.device1VoltageLevelStartNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.device1VoltageLevelStartNumeric.Location = new System.Drawing.Point(147, 140);
            this.device1VoltageLevelStartNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.device1VoltageLevelStartNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.device1VoltageLevelStartNumeric.Name = "device1VoltageLevelStartNumeric";
            this.device1VoltageLevelStartNumeric.Size = new System.Drawing.Size(100, 20);
            this.device1VoltageLevelStartNumeric.TabIndex = 11;
            // 
            // device1SourceDelayLabel
            // 
            this.device1SourceDelayLabel.AutoSize = true;
            this.device1SourceDelayLabel.Location = new System.Drawing.Point(6, 174);
            this.device1SourceDelayLabel.Name = "device1SourceDelayLabel";
            this.device1SourceDelayLabel.Size = new System.Drawing.Size(85, 13);
            this.device1SourceDelayLabel.TabIndex = 12;
            this.device1SourceDelayLabel.Text = "Source Delay (s)";
            // 
            // device1NumberOfPointsLabel
            // 
            this.device1NumberOfPointsLabel.AutoSize = true;
            this.device1NumberOfPointsLabel.Location = new System.Drawing.Point(147, 75);
            this.device1NumberOfPointsLabel.Name = "device1NumberOfPointsLabel";
            this.device1NumberOfPointsLabel.Size = new System.Drawing.Size(36, 13);
            this.device1NumberOfPointsLabel.TabIndex = 6;
            this.device1NumberOfPointsLabel.Text = "Points";
            // 
            // device1ChannelNameLabel
            // 
            this.device1ChannelNameLabel.AutoSize = true;
            this.device1ChannelNameLabel.Location = new System.Drawing.Point(147, 23);
            this.device1ChannelNameLabel.Name = "device1ChannelNameLabel";
            this.device1ChannelNameLabel.Size = new System.Drawing.Size(77, 13);
            this.device1ChannelNameLabel.TabIndex = 2;
            this.device1ChannelNameLabel.Text = "Channel Name";
            // 
            // device1NumberOfPointsNumeric
            // 
            this.device1NumberOfPointsNumeric.Location = new System.Drawing.Point(147, 91);
            this.device1NumberOfPointsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.device1NumberOfPointsNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.device1NumberOfPointsNumeric.Name = "device1NumberOfPointsNumeric";
            this.device1NumberOfPointsNumeric.Size = new System.Drawing.Size(100, 20);
            this.device1NumberOfPointsNumeric.TabIndex = 7;
            this.device1NumberOfPointsNumeric.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // device1ResourceNameLabel
            // 
            this.device1ResourceNameLabel.AutoSize = true;
            this.device1ResourceNameLabel.Location = new System.Drawing.Point(6, 23);
            this.device1ResourceNameLabel.Name = "device1ResourceNameLabel";
            this.device1ResourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.device1ResourceNameLabel.TabIndex = 0;
            this.device1ResourceNameLabel.Text = "Resource Name";
            // 
            // device1SourceDelayNumeric
            // 
            this.device1SourceDelayNumeric.DecimalPlaces = 6;
            this.device1SourceDelayNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.device1SourceDelayNumeric.Location = new System.Drawing.Point(6, 190);
            this.device1SourceDelayNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.device1SourceDelayNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.device1SourceDelayNumeric.Name = "device1SourceDelayNumeric";
            this.device1SourceDelayNumeric.Size = new System.Drawing.Size(100, 20);
            this.device1SourceDelayNumeric.TabIndex = 13;
            this.device1SourceDelayNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            // 
            // device1CurrentLimitLabel
            // 
            this.device1CurrentLimitLabel.AutoSize = true;
            this.device1CurrentLimitLabel.Location = new System.Drawing.Point(6, 75);
            this.device1CurrentLimitLabel.Name = "device1CurrentLimitLabel";
            this.device1CurrentLimitLabel.Size = new System.Drawing.Size(81, 13);
            this.device1CurrentLimitLabel.TabIndex = 4;
            this.device1CurrentLimitLabel.Text = "Current Limit (A)";
            // 
            // device1CurrentLimitNumeric
            // 
            this.device1CurrentLimitNumeric.DecimalPlaces = 6;
            this.device1CurrentLimitNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.device1CurrentLimitNumeric.Location = new System.Drawing.Point(6, 91);
            this.device1CurrentLimitNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.device1CurrentLimitNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.device1CurrentLimitNumeric.Name = "device1CurrentLimitNumeric";
            this.device1CurrentLimitNumeric.Size = new System.Drawing.Size(100, 20);
            this.device1CurrentLimitNumeric.TabIndex = 5;
            this.device1CurrentLimitNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // device1SenseComboBox
            // 
            this.device1SenseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.device1SenseComboBox.Location = new System.Drawing.Point(6, 140);
            this.device1SenseComboBox.Name = "device1SenseComboBox";
            this.device1SenseComboBox.Size = new System.Drawing.Size(100, 21);
            this.device1SenseComboBox.TabIndex = 9;
            // 
            // timeoutNumeric
            // 
            this.timeoutNumeric.DecimalPlaces = 6;
            this.timeoutNumeric.Location = new System.Drawing.Point(6, 19);
            this.timeoutNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.timeoutNumeric.Name = "timeoutNumeric";
            this.timeoutNumeric.Size = new System.Drawing.Size(100, 20);
            this.timeoutNumeric.TabIndex = 0;
            this.timeoutNumeric.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // timeoutGroupBox
            // 
            this.timeoutGroupBox.Controls.Add(this.timeoutNumeric);
            this.timeoutGroupBox.Location = new System.Drawing.Point(294, 12);
            this.timeoutGroupBox.Name = "timeoutGroupBox";
            this.timeoutGroupBox.Size = new System.Drawing.Size(265, 50);
            this.timeoutGroupBox.TabIndex = 2;
            this.timeoutGroupBox.TabStop = false;
            this.timeoutGroupBox.Text = "Timeout";
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 524);
            this.Controls.Add(this.timeoutGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.device1GroupBox);
            this.Controls.Add(this.measurementsGroupBox);
            this.Controls.Add(this.device0GroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Hardware-Timed Two-Channel Voltage Sweep (IV Curve)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            this.measurementsGroupBox.ResumeLayout(false);
            this.measurementsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.device0VoltageLevelStopNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.device0SourceDelayNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.device0VoltageLevelStartNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.device0NumberOfPlotsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.device0CurrentLimitNumeric)).EndInit();
            this.device0GroupBox.ResumeLayout(false);
            this.device0GroupBox.PerformLayout();
            this.device1GroupBox.ResumeLayout(false);
            this.device1GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.device1VoltageLevelStopNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.device1VoltageLevelStartNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.device1NumberOfPointsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.device1SourceDelayNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.device1CurrentLimitNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumeric)).EndInit();
            this.timeoutGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox measurementsGroupBox;
        private System.Windows.Forms.DataGridView measurementsDataGridView;
        private System.Windows.Forms.Label device0SenseLabel;
        private System.Windows.Forms.Label device0VoltageLevelStopLabel;
        private System.Windows.Forms.Label device0VoltageLevelStartLabel;
        private System.Windows.Forms.Label device0SourceDelayLabel;
        private System.Windows.Forms.Label device0NumberOfPlotsLabel;
        private System.Windows.Forms.NumericUpDown device0VoltageLevelStopNumeric;
        private System.Windows.Forms.Label device0CurrentLimitLabel;
        private System.Windows.Forms.NumericUpDown device0SourceDelayNumeric;
        private System.Windows.Forms.NumericUpDown device0VoltageLevelStartNumeric;
        private System.Windows.Forms.NumericUpDown device0NumberOfPlotsNumeric;
        private System.Windows.Forms.NumericUpDown device0CurrentLimitNumeric;
        private System.Windows.Forms.ComboBox device0SenseComboBox;
        private System.Windows.Forms.GroupBox device0GroupBox;
        private System.Windows.Forms.ComboBox device0ResourceNameComboBox;
        private System.Windows.Forms.Label device0ChannelNameLabel;
        private System.Windows.Forms.Label device0ResourceNameLabel;
        private System.Windows.Forms.GroupBox device1GroupBox;
        private System.Windows.Forms.Label device1VoltageLevelStopLabel;
        private System.Windows.Forms.Label device1SenseLabel;
        private System.Windows.Forms.NumericUpDown device1VoltageLevelStopNumeric;
        private System.Windows.Forms.ComboBox device1ResourceNameComboBox;
        private System.Windows.Forms.Label device1VoltageLevelStartLabel;
        private System.Windows.Forms.NumericUpDown device1VoltageLevelStartNumeric;
        private System.Windows.Forms.Label device1SourceDelayLabel;
        private System.Windows.Forms.Label device1NumberOfPointsLabel;
        private System.Windows.Forms.Label device1ChannelNameLabel;
        private System.Windows.Forms.NumericUpDown device1NumberOfPointsNumeric;
        private System.Windows.Forms.Label device1ResourceNameLabel;
        private System.Windows.Forms.NumericUpDown device1SourceDelayNumeric;
        private System.Windows.Forms.Label device1CurrentLimitLabel;
        private System.Windows.Forms.NumericUpDown device1CurrentLimitNumeric;
        private System.Windows.Forms.ComboBox device1SenseComboBox;
        private System.Windows.Forms.NumericUpDown timeoutNumeric;
        private System.Windows.Forms.GroupBox timeoutGroupBox;
        private System.Windows.Forms.TextBox device0ChannelNameComboBox;
        private System.Windows.Forms.TextBox device1ChannelNameComboBox;
        private System.Windows.Forms.Label selectPlotLabel;
        private System.Windows.Forms.ComboBox plotNameComboBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn readingNoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn measuredVoltageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn measuredCurrentColumn;

    }
}
