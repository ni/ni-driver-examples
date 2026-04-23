namespace NationalInstruments.Examples.SoftwareTimedTwoChannelSweep
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
            this.selectedPlotNameComboBox = new System.Windows.Forms.ComboBox();
            this.measurementsDataGridView = new System.Windows.Forms.DataGridView();
            this.readingNoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.voltageMeasurementColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentMeasurementColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.channel1SenseLabel = new System.Windows.Forms.Label();
            this.channel1VoltageLevelStopLabel = new System.Windows.Forms.Label();
            this.channel1VoltageLevelStartLabel = new System.Windows.Forms.Label();
            this.channel1DelayLabel = new System.Windows.Forms.Label();
            this.channel1PlotsLabel = new System.Windows.Forms.Label();
            this.channel1VoltageLevelStopNumeric = new System.Windows.Forms.NumericUpDown();
            this.channel1CurrentLimitLabel = new System.Windows.Forms.Label();
            this.channel1SourceDelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.channel1VoltageLevelStartNumeric = new System.Windows.Forms.NumericUpDown();
            this.channel1NumberOfPlotsNumeric = new System.Windows.Forms.NumericUpDown();
            this.channel1CurrentLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.channel1SenseComboBox = new System.Windows.Forms.ComboBox();
            this.channel1GroupBox = new System.Windows.Forms.GroupBox();
            this.channel1NameTextBox = new System.Windows.Forms.TextBox();
            this.channel1NameLabel = new System.Windows.Forms.Label();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.channel2GroupBox = new System.Windows.Forms.GroupBox();
            this.channel2NameTextBox = new System.Windows.Forms.TextBox();
            this.channel2VoltageLevelStopLabel = new System.Windows.Forms.Label();
            this.channel2SenseLabel = new System.Windows.Forms.Label();
            this.channel2VoltageLevelStopNumeric = new System.Windows.Forms.NumericUpDown();
            this.channel2VoltageLevelStartLabel = new System.Windows.Forms.Label();
            this.channel2VoltageLevelStartNumeric = new System.Windows.Forms.NumericUpDown();
            this.channel2DelayLabel = new System.Windows.Forms.Label();
            this.channel2PointsLabel = new System.Windows.Forms.Label();
            this.channel2Name = new System.Windows.Forms.Label();
            this.channel2NumberOfPointsNumeric = new System.Windows.Forms.NumericUpDown();
            this.channel2SourceDelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.channel2CurrentLimitLabel = new System.Windows.Forms.Label();
            this.channel2CurrentLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.channel2SenseComboBox = new System.Windows.Forms.ComboBox();
            this.message1RichTextBox = new System.Windows.Forms.RichTextBox();
            this.message2RichTextBox = new System.Windows.Forms.RichTextBox();
            this.message4RichTextBox = new System.Windows.Forms.RichTextBox();
            this.message3RichTextBox = new System.Windows.Forms.RichTextBox();
            this.resourceNameGroupBox = new System.Windows.Forms.GroupBox();
            this.messageGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel1VoltageLevelStopNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel1SourceDelayNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel1VoltageLevelStartNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel1NumberOfPlotsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel1CurrentLimitNumeric)).BeginInit();
            this.channel1GroupBox.SuspendLayout();
            this.channel2GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.channel2VoltageLevelStopNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel2VoltageLevelStartNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel2NumberOfPointsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel2SourceDelayNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel2CurrentLimitNumeric)).BeginInit();
            this.resourceNameGroupBox.SuspendLayout();
            this.messageGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(106, 553);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // measurementsGroupBox
            // 
            this.measurementsGroupBox.Controls.Add(this.selectPlotLabel);
            this.measurementsGroupBox.Controls.Add(this.selectedPlotNameComboBox);
            this.measurementsGroupBox.Controls.Add(this.measurementsDataGridView);
            this.measurementsGroupBox.Location = new System.Drawing.Point(291, 12);
            this.measurementsGroupBox.Name = "measurementsGroupBox";
            this.measurementsGroupBox.Size = new System.Drawing.Size(265, 362);
            this.measurementsGroupBox.TabIndex = 4;
            this.measurementsGroupBox.TabStop = false;
            this.measurementsGroupBox.Text = "Measurements";
            // 
            // selectPlotLabel
            // 
            this.selectPlotLabel.AutoSize = true;
            this.selectPlotLabel.Location = new System.Drawing.Point(6, 23);
            this.selectPlotLabel.Name = "selectPlotLabel";
            this.selectPlotLabel.Size = new System.Drawing.Size(58, 13);
            this.selectPlotLabel.TabIndex = 0;
            this.selectPlotLabel.Text = "Select Plot";
            // 
            // selectedPlotNameComboBox
            // 
            this.selectedPlotNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectedPlotNameComboBox.FormattingEnabled = true;
            this.selectedPlotNameComboBox.Location = new System.Drawing.Point(68, 19);
            this.selectedPlotNameComboBox.Name = "selectedPlotNameComboBox";
            this.selectedPlotNameComboBox.Size = new System.Drawing.Size(191, 21);
            this.selectedPlotNameComboBox.TabIndex = 1;
            this.selectedPlotNameComboBox.SelectedIndexChanged += new System.EventHandler(this.plotNameComboBox_SelectedIndexChanged);
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
            this.measurementsDataGridView.Location = new System.Drawing.Point(6, 56);
            this.measurementsDataGridView.Name = "measurementsDataGridView";
            this.measurementsDataGridView.ReadOnly = true;
            this.measurementsDataGridView.RowHeadersVisible = false;
            this.measurementsDataGridView.RowHeadersWidth = 15;
            this.measurementsDataGridView.RowTemplate.Height = 24;
            this.measurementsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.measurementsDataGridView.Size = new System.Drawing.Size(253, 299);
            this.measurementsDataGridView.StandardTab = true;
            this.measurementsDataGridView.TabIndex = 2;
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
            // channel1SenseLabel
            // 
            this.channel1SenseLabel.AutoSize = true;
            this.channel1SenseLabel.Location = new System.Drawing.Point(6, 122);
            this.channel1SenseLabel.Name = "channel1SenseLabel";
            this.channel1SenseLabel.Size = new System.Drawing.Size(37, 13);
            this.channel1SenseLabel.TabIndex = 6;
            this.channel1SenseLabel.Text = "Sense";
            // 
            // channel1VoltageLevelStopLabel
            // 
            this.channel1VoltageLevelStopLabel.AutoSize = true;
            this.channel1VoltageLevelStopLabel.Location = new System.Drawing.Point(147, 172);
            this.channel1VoltageLevelStopLabel.Name = "channel1VoltageLevelStopLabel";
            this.channel1VoltageLevelStopLabel.Size = new System.Drawing.Size(113, 13);
            this.channel1VoltageLevelStopLabel.TabIndex = 12;
            this.channel1VoltageLevelStopLabel.Text = "Voltage Level Stop (V)";
            // 
            // channel1VoltageLevelStartLabel
            // 
            this.channel1VoltageLevelStartLabel.AutoSize = true;
            this.channel1VoltageLevelStartLabel.Location = new System.Drawing.Point(147, 122);
            this.channel1VoltageLevelStartLabel.Name = "channel1VoltageLevelStartLabel";
            this.channel1VoltageLevelStartLabel.Size = new System.Drawing.Size(113, 13);
            this.channel1VoltageLevelStartLabel.TabIndex = 8;
            this.channel1VoltageLevelStartLabel.Text = "Voltage Level Start (V)";
            // 
            // channel1DelayLabel
            // 
            this.channel1DelayLabel.AutoSize = true;
            this.channel1DelayLabel.Location = new System.Drawing.Point(6, 172);
            this.channel1DelayLabel.Name = "channel1DelayLabel";
            this.channel1DelayLabel.Size = new System.Drawing.Size(85, 13);
            this.channel1DelayLabel.TabIndex = 10;
            this.channel1DelayLabel.Text = "Source Delay (s)";
            // 
            // channel1PlotsLabel
            // 
            this.channel1PlotsLabel.AutoSize = true;
            this.channel1PlotsLabel.Location = new System.Drawing.Point(147, 73);
            this.channel1PlotsLabel.Name = "channel1PlotsLabel";
            this.channel1PlotsLabel.Size = new System.Drawing.Size(30, 13);
            this.channel1PlotsLabel.TabIndex = 4;
            this.channel1PlotsLabel.Text = "Plots";
            // 
            // channel1VoltageLevelStopNumeric
            // 
            this.channel1VoltageLevelStopNumeric.DecimalPlaces = 6;
            this.channel1VoltageLevelStopNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.channel1VoltageLevelStopNumeric.Location = new System.Drawing.Point(147, 188);
            this.channel1VoltageLevelStopNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.channel1VoltageLevelStopNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.channel1VoltageLevelStopNumeric.Name = "channel1VoltageLevelStopNumeric";
            this.channel1VoltageLevelStopNumeric.Size = new System.Drawing.Size(100, 20);
            this.channel1VoltageLevelStopNumeric.TabIndex = 13;
            this.channel1VoltageLevelStopNumeric.Value = new decimal(new int[] {
            39,
            0,
            0,
            65536});
            // 
            // channel1CurrentLimitLabel
            // 
            this.channel1CurrentLimitLabel.AutoSize = true;
            this.channel1CurrentLimitLabel.Location = new System.Drawing.Point(6, 73);
            this.channel1CurrentLimitLabel.Name = "channel1CurrentLimitLabel";
            this.channel1CurrentLimitLabel.Size = new System.Drawing.Size(81, 13);
            this.channel1CurrentLimitLabel.TabIndex = 2;
            this.channel1CurrentLimitLabel.Text = "Current Limit (A)";
            // 
            // channel1SourceDelayNumeric
            // 
            this.channel1SourceDelayNumeric.DecimalPlaces = 6;
            this.channel1SourceDelayNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.channel1SourceDelayNumeric.Location = new System.Drawing.Point(6, 188);
            this.channel1SourceDelayNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.channel1SourceDelayNumeric.Name = "channel1SourceDelayNumeric";
            this.channel1SourceDelayNumeric.Size = new System.Drawing.Size(100, 20);
            this.channel1SourceDelayNumeric.TabIndex = 11;
            this.channel1SourceDelayNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            196608});
            // 
            // channel1VoltageLevelStartNumeric
            // 
            this.channel1VoltageLevelStartNumeric.DecimalPlaces = 6;
            this.channel1VoltageLevelStartNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.channel1VoltageLevelStartNumeric.Location = new System.Drawing.Point(147, 138);
            this.channel1VoltageLevelStartNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.channel1VoltageLevelStartNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.channel1VoltageLevelStartNumeric.Name = "channel1VoltageLevelStartNumeric";
            this.channel1VoltageLevelStartNumeric.Size = new System.Drawing.Size(100, 20);
            this.channel1VoltageLevelStartNumeric.TabIndex = 9;
            this.channel1VoltageLevelStartNumeric.Value = new decimal(new int[] {
            35,
            0,
            0,
            65536});
            // 
            // channel1NumberOfPlotsNumeric
            // 
            this.channel1NumberOfPlotsNumeric.Location = new System.Drawing.Point(147, 89);
            this.channel1NumberOfPlotsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.channel1NumberOfPlotsNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.channel1NumberOfPlotsNumeric.Name = "channel1NumberOfPlotsNumeric";
            this.channel1NumberOfPlotsNumeric.Size = new System.Drawing.Size(100, 20);
            this.channel1NumberOfPlotsNumeric.TabIndex = 5;
            this.channel1NumberOfPlotsNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // channel1CurrentLimitNumeric
            // 
            this.channel1CurrentLimitNumeric.DecimalPlaces = 6;
            this.channel1CurrentLimitNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.channel1CurrentLimitNumeric.Location = new System.Drawing.Point(6, 89);
            this.channel1CurrentLimitNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.channel1CurrentLimitNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.channel1CurrentLimitNumeric.Name = "channel1CurrentLimitNumeric";
            this.channel1CurrentLimitNumeric.Size = new System.Drawing.Size(100, 20);
            this.channel1CurrentLimitNumeric.TabIndex = 3;
            this.channel1CurrentLimitNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            131072});
            // 
            // channel1SenseComboBox
            // 
            this.channel1SenseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.channel1SenseComboBox.Location = new System.Drawing.Point(6, 138);
            this.channel1SenseComboBox.Name = "channel1SenseComboBox";
            this.channel1SenseComboBox.Size = new System.Drawing.Size(100, 21);
            this.channel1SenseComboBox.TabIndex = 7;
            // 
            // channel1GroupBox
            // 
            this.channel1GroupBox.Controls.Add(this.channel1NameTextBox);
            this.channel1GroupBox.Controls.Add(this.channel1VoltageLevelStopLabel);
            this.channel1GroupBox.Controls.Add(this.channel1SenseLabel);
            this.channel1GroupBox.Controls.Add(this.channel1VoltageLevelStopNumeric);
            this.channel1GroupBox.Controls.Add(this.channel1VoltageLevelStartLabel);
            this.channel1GroupBox.Controls.Add(this.channel1VoltageLevelStartNumeric);
            this.channel1GroupBox.Controls.Add(this.channel1DelayLabel);
            this.channel1GroupBox.Controls.Add(this.channel1PlotsLabel);
            this.channel1GroupBox.Controls.Add(this.channel1NameLabel);
            this.channel1GroupBox.Controls.Add(this.channel1NumberOfPlotsNumeric);
            this.channel1GroupBox.Controls.Add(this.channel1SourceDelayNumeric);
            this.channel1GroupBox.Controls.Add(this.channel1CurrentLimitLabel);
            this.channel1GroupBox.Controls.Add(this.channel1CurrentLimitNumeric);
            this.channel1GroupBox.Controls.Add(this.channel1SenseComboBox);
            this.channel1GroupBox.Location = new System.Drawing.Point(12, 78);
            this.channel1GroupBox.Name = "channel1GroupBox";
            this.channel1GroupBox.Size = new System.Drawing.Size(263, 217);
            this.channel1GroupBox.TabIndex = 1;
            this.channel1GroupBox.TabStop = false;
            // 
            // channel1NameTextBox
            // 
            this.channel1NameTextBox.Location = new System.Drawing.Point(6, 37);
            this.channel1NameTextBox.Name = "channel1NameTextBox";
            this.channel1NameTextBox.Size = new System.Drawing.Size(100, 20);
            this.channel1NameTextBox.TabIndex = 1;
            this.channel1NameTextBox.Text = "0";
            // 
            // channel1NameLabel
            // 
            this.channel1NameLabel.AutoSize = true;
            this.channel1NameLabel.Location = new System.Drawing.Point(6, 21);
            this.channel1NameLabel.Name = "channel1NameLabel";
            this.channel1NameLabel.Size = new System.Drawing.Size(77, 13);
            this.channel1NameLabel.TabIndex = 0;
            this.channel1NameLabel.Text = "Channel Name";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(6, 19);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(126, 21);
            this.resourceNameComboBox.TabIndex = 1;
            // 
            // channel2GroupBox
            // 
            this.channel2GroupBox.Controls.Add(this.channel2NameTextBox);
            this.channel2GroupBox.Controls.Add(this.channel2VoltageLevelStopLabel);
            this.channel2GroupBox.Controls.Add(this.channel2SenseLabel);
            this.channel2GroupBox.Controls.Add(this.channel2VoltageLevelStopNumeric);
            this.channel2GroupBox.Controls.Add(this.channel2VoltageLevelStartLabel);
            this.channel2GroupBox.Controls.Add(this.channel2VoltageLevelStartNumeric);
            this.channel2GroupBox.Controls.Add(this.channel2DelayLabel);
            this.channel2GroupBox.Controls.Add(this.channel2PointsLabel);
            this.channel2GroupBox.Controls.Add(this.channel2Name);
            this.channel2GroupBox.Controls.Add(this.channel2NumberOfPointsNumeric);
            this.channel2GroupBox.Controls.Add(this.channel2SourceDelayNumeric);
            this.channel2GroupBox.Controls.Add(this.channel2CurrentLimitLabel);
            this.channel2GroupBox.Controls.Add(this.channel2CurrentLimitNumeric);
            this.channel2GroupBox.Controls.Add(this.channel2SenseComboBox);
            this.channel2GroupBox.Location = new System.Drawing.Point(12, 311);
            this.channel2GroupBox.Name = "channel2GroupBox";
            this.channel2GroupBox.Size = new System.Drawing.Size(263, 216);
            this.channel2GroupBox.TabIndex = 2;
            this.channel2GroupBox.TabStop = false;
            // 
            // channel2NameTextBox
            // 
            this.channel2NameTextBox.Location = new System.Drawing.Point(6, 37);
            this.channel2NameTextBox.Name = "channel2NameTextBox";
            this.channel2NameTextBox.Size = new System.Drawing.Size(100, 20);
            this.channel2NameTextBox.TabIndex = 1;
            this.channel2NameTextBox.Text = "1";
            // 
            // channel2VoltageLevelStopLabel
            // 
            this.channel2VoltageLevelStopLabel.AutoSize = true;
            this.channel2VoltageLevelStopLabel.Location = new System.Drawing.Point(147, 172);
            this.channel2VoltageLevelStopLabel.Name = "channel2VoltageLevelStopLabel";
            this.channel2VoltageLevelStopLabel.Size = new System.Drawing.Size(113, 13);
            this.channel2VoltageLevelStopLabel.TabIndex = 12;
            this.channel2VoltageLevelStopLabel.Text = "Voltage Level Stop (V)";
            // 
            // channel2SenseLabel
            // 
            this.channel2SenseLabel.AutoSize = true;
            this.channel2SenseLabel.Location = new System.Drawing.Point(6, 122);
            this.channel2SenseLabel.Name = "channel2SenseLabel";
            this.channel2SenseLabel.Size = new System.Drawing.Size(37, 13);
            this.channel2SenseLabel.TabIndex = 6;
            this.channel2SenseLabel.Text = "Sense";
            // 
            // channel2VoltageLevelStopNumeric
            // 
            this.channel2VoltageLevelStopNumeric.DecimalPlaces = 6;
            this.channel2VoltageLevelStopNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.channel2VoltageLevelStopNumeric.Location = new System.Drawing.Point(147, 188);
            this.channel2VoltageLevelStopNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.channel2VoltageLevelStopNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.channel2VoltageLevelStopNumeric.Name = "channel2VoltageLevelStopNumeric";
            this.channel2VoltageLevelStopNumeric.Size = new System.Drawing.Size(100, 20);
            this.channel2VoltageLevelStopNumeric.TabIndex = 13;
            this.channel2VoltageLevelStopNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // channel2VoltageLevelStartLabel
            // 
            this.channel2VoltageLevelStartLabel.AutoSize = true;
            this.channel2VoltageLevelStartLabel.Location = new System.Drawing.Point(147, 122);
            this.channel2VoltageLevelStartLabel.Name = "channel2VoltageLevelStartLabel";
            this.channel2VoltageLevelStartLabel.Size = new System.Drawing.Size(113, 13);
            this.channel2VoltageLevelStartLabel.TabIndex = 8;
            this.channel2VoltageLevelStartLabel.Text = "Voltage Level Start (V)";
            // 
            // channel2VoltageLevelStartNumeric
            // 
            this.channel2VoltageLevelStartNumeric.DecimalPlaces = 6;
            this.channel2VoltageLevelStartNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.channel2VoltageLevelStartNumeric.Location = new System.Drawing.Point(147, 138);
            this.channel2VoltageLevelStartNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.channel2VoltageLevelStartNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.channel2VoltageLevelStartNumeric.Name = "channel2VoltageLevelStartNumeric";
            this.channel2VoltageLevelStartNumeric.Size = new System.Drawing.Size(100, 20);
            this.channel2VoltageLevelStartNumeric.TabIndex = 9;
            // 
            // channel2DelayLabel
            // 
            this.channel2DelayLabel.AutoSize = true;
            this.channel2DelayLabel.Location = new System.Drawing.Point(6, 172);
            this.channel2DelayLabel.Name = "channel2DelayLabel";
            this.channel2DelayLabel.Size = new System.Drawing.Size(85, 13);
            this.channel2DelayLabel.TabIndex = 10;
            this.channel2DelayLabel.Text = "Source Delay (s)";
            // 
            // channel2PointsLabel
            // 
            this.channel2PointsLabel.AutoSize = true;
            this.channel2PointsLabel.Location = new System.Drawing.Point(147, 73);
            this.channel2PointsLabel.Name = "channel2PointsLabel";
            this.channel2PointsLabel.Size = new System.Drawing.Size(36, 13);
            this.channel2PointsLabel.TabIndex = 4;
            this.channel2PointsLabel.Text = "Points";
            // 
            // channel2Name
            // 
            this.channel2Name.AutoSize = true;
            this.channel2Name.Location = new System.Drawing.Point(6, 21);
            this.channel2Name.Name = "channel2Name";
            this.channel2Name.Size = new System.Drawing.Size(77, 13);
            this.channel2Name.TabIndex = 0;
            this.channel2Name.Text = "Channel Name";
            // 
            // channel2NumberOfPointsNumeric
            // 
            this.channel2NumberOfPointsNumeric.Location = new System.Drawing.Point(147, 89);
            this.channel2NumberOfPointsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.channel2NumberOfPointsNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.channel2NumberOfPointsNumeric.Name = "channel2NumberOfPointsNumeric";
            this.channel2NumberOfPointsNumeric.Size = new System.Drawing.Size(100, 20);
            this.channel2NumberOfPointsNumeric.TabIndex = 5;
            this.channel2NumberOfPointsNumeric.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // channel2SourceDelayNumeric
            // 
            this.channel2SourceDelayNumeric.DecimalPlaces = 6;
            this.channel2SourceDelayNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.channel2SourceDelayNumeric.Location = new System.Drawing.Point(6, 188);
            this.channel2SourceDelayNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.channel2SourceDelayNumeric.Name = "channel2SourceDelayNumeric";
            this.channel2SourceDelayNumeric.Size = new System.Drawing.Size(100, 20);
            this.channel2SourceDelayNumeric.TabIndex = 11;
            this.channel2SourceDelayNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            // 
            // channel2CurrentLimitLabel
            // 
            this.channel2CurrentLimitLabel.AutoSize = true;
            this.channel2CurrentLimitLabel.Location = new System.Drawing.Point(6, 73);
            this.channel2CurrentLimitLabel.Name = "channel2CurrentLimitLabel";
            this.channel2CurrentLimitLabel.Size = new System.Drawing.Size(81, 13);
            this.channel2CurrentLimitLabel.TabIndex = 2;
            this.channel2CurrentLimitLabel.Text = "Current Limit (A)";
            // 
            // channel2CurrentLimitNumeric
            // 
            this.channel2CurrentLimitNumeric.DecimalPlaces = 6;
            this.channel2CurrentLimitNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.channel2CurrentLimitNumeric.Location = new System.Drawing.Point(6, 89);
            this.channel2CurrentLimitNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.channel2CurrentLimitNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.channel2CurrentLimitNumeric.Name = "channel2CurrentLimitNumeric";
            this.channel2CurrentLimitNumeric.Size = new System.Drawing.Size(100, 20);
            this.channel2CurrentLimitNumeric.TabIndex = 3;
            this.channel2CurrentLimitNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // channel2SenseComboBox
            // 
            this.channel2SenseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.channel2SenseComboBox.Location = new System.Drawing.Point(6, 138);
            this.channel2SenseComboBox.Name = "channel2SenseComboBox";
            this.channel2SenseComboBox.Size = new System.Drawing.Size(100, 21);
            this.channel2SenseComboBox.TabIndex = 7;
            // 
            // message1RichTextBox
            // 
            this.message1RichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.message1RichTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.message1RichTextBox.Location = new System.Drawing.Point(6, 19);
            this.message1RichTextBox.Name = "message1RichTextBox";
            this.message1RichTextBox.ReadOnly = true;
            this.message1RichTextBox.Size = new System.Drawing.Size(253, 58);
            this.message1RichTextBox.TabIndex = 0;
            this.message1RichTextBox.TabStop = false;
            this.message1RichTextBox.Text = "By default this example can be used to characterize a FET.  If you wish to charac" +
                "terize a BJT, the example can be modified to sweep current by making the followi" +
                "ng changes: ";
            // 
            // message2RichTextBox
            // 
            this.message2RichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.message2RichTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.message2RichTextBox.Location = new System.Drawing.Point(6, 83);
            this.message2RichTextBox.Name = "message2RichTextBox";
            this.message2RichTextBox.ReadOnly = true;
            this.message2RichTextBox.Size = new System.Drawing.Size(253, 29);
            this.message2RichTextBox.TabIndex = 1;
            this.message2RichTextBox.TabStop = false;
            this.message2RichTextBox.Text = "1.  Change the Output Function to \"DC Current\" instead of \"DC Voltage\".";
            // 
            // message4RichTextBox
            // 
            this.message4RichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.message4RichTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.message4RichTextBox.Location = new System.Drawing.Point(6, 153);
            this.message4RichTextBox.Name = "message4RichTextBox";
            this.message4RichTextBox.ReadOnly = true;
            this.message4RichTextBox.Size = new System.Drawing.Size(253, 43);
            this.message4RichTextBox.TabIndex = 3;
            this.message4RichTextBox.TabStop = false;
            this.message4RichTextBox.Text = "3.  Use \"Current Level Autorange\" and \"Voltage Limit Autorange\" instead of \"Curre" +
                "nt Limit Autorange\" and \"Voltage Level Autorange\".";
            // 
            // message3RichTextBox
            // 
            this.message3RichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.message3RichTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.message3RichTextBox.Location = new System.Drawing.Point(6, 118);
            this.message3RichTextBox.Name = "message3RichTextBox";
            this.message3RichTextBox.ReadOnly = true;
            this.message3RichTextBox.Size = new System.Drawing.Size(253, 29);
            this.message3RichTextBox.TabIndex = 2;
            this.message3RichTextBox.TabStop = false;
            this.message3RichTextBox.Text = "2.  Use \"Current Level\" and \"Voltage Limit\" instead of \"Voltage Level\" and \"Curre" +
                "nt Limit\".";
            // 
            // resourceNameGroupBox
            // 
            this.resourceNameGroupBox.Controls.Add(this.resourceNameComboBox);
            this.resourceNameGroupBox.Location = new System.Drawing.Point(12, 12);
            this.resourceNameGroupBox.Name = "resourceNameGroupBox";
            this.resourceNameGroupBox.Size = new System.Drawing.Size(263, 50);
            this.resourceNameGroupBox.TabIndex = 0;
            this.resourceNameGroupBox.TabStop = false;
            this.resourceNameGroupBox.Text = "Resource Name";
            // 
            // messageGroupBox
            // 
            this.messageGroupBox.Controls.Add(this.message3RichTextBox);
            this.messageGroupBox.Controls.Add(this.message4RichTextBox);
            this.messageGroupBox.Controls.Add(this.message2RichTextBox);
            this.messageGroupBox.Controls.Add(this.message1RichTextBox);
            this.messageGroupBox.Location = new System.Drawing.Point(291, 380);
            this.messageGroupBox.Name = "messageGroupBox";
            this.messageGroupBox.Size = new System.Drawing.Size(265, 207);
            this.messageGroupBox.TabIndex = 5;
            this.messageGroupBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 599);
            this.Controls.Add(this.messageGroupBox);
            this.Controls.Add(this.resourceNameGroupBox);
            this.Controls.Add(this.channel2GroupBox);
            this.Controls.Add(this.measurementsGroupBox);
            this.Controls.Add(this.channel1GroupBox);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Softawre-Timed Two-Channel Voltage Sweep (IV Curve)";
            this.measurementsGroupBox.ResumeLayout(false);
            this.measurementsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel1VoltageLevelStopNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel1SourceDelayNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel1VoltageLevelStartNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel1NumberOfPlotsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel1CurrentLimitNumeric)).EndInit();
            this.channel1GroupBox.ResumeLayout(false);
            this.channel1GroupBox.PerformLayout();
            this.channel2GroupBox.ResumeLayout(false);
            this.channel2GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.channel2VoltageLevelStopNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel2VoltageLevelStartNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel2NumberOfPointsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel2SourceDelayNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channel2CurrentLimitNumeric)).EndInit();
            this.resourceNameGroupBox.ResumeLayout(false);
            this.messageGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox measurementsGroupBox;
        private System.Windows.Forms.DataGridView measurementsDataGridView;
        private System.Windows.Forms.Label channel1SenseLabel;
        private System.Windows.Forms.Label channel1VoltageLevelStopLabel;
        private System.Windows.Forms.Label channel1VoltageLevelStartLabel;
        private System.Windows.Forms.Label channel1DelayLabel;
        private System.Windows.Forms.Label channel1PlotsLabel;
        private System.Windows.Forms.NumericUpDown channel1VoltageLevelStopNumeric;
        private System.Windows.Forms.Label channel1CurrentLimitLabel;
        private System.Windows.Forms.NumericUpDown channel1SourceDelayNumeric;
        private System.Windows.Forms.NumericUpDown channel1VoltageLevelStartNumeric;
        private System.Windows.Forms.NumericUpDown channel1NumberOfPlotsNumeric;
        private System.Windows.Forms.NumericUpDown channel1CurrentLimitNumeric;
        private System.Windows.Forms.ComboBox channel1SenseComboBox;
        private System.Windows.Forms.GroupBox channel1GroupBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label channel1NameLabel;
        private System.Windows.Forms.GroupBox channel2GroupBox;
        private System.Windows.Forms.Label channel2VoltageLevelStopLabel;
        private System.Windows.Forms.Label channel2SenseLabel;
        private System.Windows.Forms.NumericUpDown channel2VoltageLevelStopNumeric;
        private System.Windows.Forms.Label channel2VoltageLevelStartLabel;
        private System.Windows.Forms.NumericUpDown channel2VoltageLevelStartNumeric;
        private System.Windows.Forms.Label channel2DelayLabel;
        private System.Windows.Forms.Label channel2PointsLabel;
        private System.Windows.Forms.Label channel2Name;
        private System.Windows.Forms.NumericUpDown channel2NumberOfPointsNumeric;
        private System.Windows.Forms.NumericUpDown channel2SourceDelayNumeric;
        private System.Windows.Forms.Label channel2CurrentLimitLabel;
        private System.Windows.Forms.NumericUpDown channel2CurrentLimitNumeric;
        private System.Windows.Forms.ComboBox channel2SenseComboBox;
        private System.Windows.Forms.RichTextBox message1RichTextBox;
        private System.Windows.Forms.RichTextBox message2RichTextBox;
        private System.Windows.Forms.RichTextBox message4RichTextBox;
        private System.Windows.Forms.RichTextBox message3RichTextBox;
        private System.Windows.Forms.TextBox channel1NameTextBox;
        private System.Windows.Forms.TextBox channel2NameTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn readingNoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn voltageMeasurementColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentMeasurementColumn;
        private System.Windows.Forms.GroupBox resourceNameGroupBox;
        private System.Windows.Forms.GroupBox messageGroupBox;
        private System.Windows.Forms.Label selectPlotLabel;
        private System.Windows.Forms.ComboBox selectedPlotNameComboBox;

    }
}
