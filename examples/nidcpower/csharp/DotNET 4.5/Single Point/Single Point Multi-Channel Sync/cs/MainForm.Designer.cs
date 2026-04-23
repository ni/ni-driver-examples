namespace NationalInstruments.Examples.SinglePointMultiChannelSync
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.masterConfigurationCurrentLimitLabel = new System.Windows.Forms.Label();
            this.masterConfigurationVoltageLevelLabel = new System.Windows.Forms.Label();
            this.masterConfigurationDelayLabel = new System.Windows.Forms.Label();
            this.masterConfigurationCurrentLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.masterConfigurationVoltageLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.masterConfigurationSourceDelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.masterConfigurationGroupBox = new System.Windows.Forms.GroupBox();
            this.masterConfigurationChannelNameTextBox = new System.Windows.Forms.TextBox();
            this.masterConfigurationResourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.masterConfigurationChannelNameLabel = new System.Windows.Forms.Label();
            this.masterConfigurationResourceNameLabel = new System.Windows.Forms.Label();
            this.masterMeasurementsGroupBox = new System.Windows.Forms.GroupBox();
            this.masterMeasurementsVoltageTextBox = new System.Windows.Forms.TextBox();
            this.masterMeasurementsCurrentTextBox = new System.Windows.Forms.TextBox();
            this.masterMeasurementsCurrentLabel = new System.Windows.Forms.Label();
            this.masterMeasurementsVoltageLabel = new System.Windows.Forms.Label();
            this.messageRichTextBox = new System.Windows.Forms.RichTextBox();
            this.slavesConfigurationGroupBox = new System.Windows.Forms.GroupBox();
            this.slavesConfigurationDataGridView = new System.Windows.Forms.DataGridView();
            this.slavesConfigurationSlaveNumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slavesConfigurationResourceNameColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.slavesConfigurationChannelNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slavesConfigurationVoltageLevelColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slavesConfigurationCurrentLimitColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slavesMeasurementsGroupBox = new System.Windows.Forms.GroupBox();
            this.slavesMeasurementsDataGridView = new System.Windows.Forms.DataGridView();
            this.slavesMesurementsSlaveNumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slavesMeasurementsVoltageColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slavesMeasurementsCurrentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.masterConfigurationCurrentLimitNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.masterConfigurationVoltageLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.masterConfigurationSourceDelayNumeric)).BeginInit();
            this.masterConfigurationGroupBox.SuspendLayout();
            this.masterMeasurementsGroupBox.SuspendLayout();
            this.slavesConfigurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slavesConfigurationDataGridView)).BeginInit();
            this.slavesMeasurementsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slavesMeasurementsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // masterConfigurationCurrentLimitLabel
            // 
            this.masterConfigurationCurrentLimitLabel.AutoSize = true;
            this.masterConfigurationCurrentLimitLabel.Location = new System.Drawing.Point(6, 111);
            this.masterConfigurationCurrentLimitLabel.Name = "masterConfigurationCurrentLimitLabel";
            this.masterConfigurationCurrentLimitLabel.Size = new System.Drawing.Size(81, 13);
            this.masterConfigurationCurrentLimitLabel.TabIndex = 6;
            this.masterConfigurationCurrentLimitLabel.Text = "Current Limit (A)";
            // 
            // masterConfigurationVoltageLevelLabel
            // 
            this.masterConfigurationVoltageLevelLabel.AutoSize = true;
            this.masterConfigurationVoltageLevelLabel.Location = new System.Drawing.Point(6, 85);
            this.masterConfigurationVoltageLevelLabel.Name = "masterConfigurationVoltageLevelLabel";
            this.masterConfigurationVoltageLevelLabel.Size = new System.Drawing.Size(88, 13);
            this.masterConfigurationVoltageLevelLabel.TabIndex = 4;
            this.masterConfigurationVoltageLevelLabel.Text = "Voltage Level (V)";
            // 
            // masterConfigurationDelayLabel
            // 
            this.masterConfigurationDelayLabel.AutoSize = true;
            this.masterConfigurationDelayLabel.Location = new System.Drawing.Point(6, 137);
            this.masterConfigurationDelayLabel.Name = "masterConfigurationDelayLabel";
            this.masterConfigurationDelayLabel.Size = new System.Drawing.Size(85, 13);
            this.masterConfigurationDelayLabel.TabIndex = 8;
            this.masterConfigurationDelayLabel.Text = "Source Delay (s)";
            // 
            // masterConfigurationCurrentLimitNumeric
            // 
            this.masterConfigurationCurrentLimitNumeric.DecimalPlaces = 6;
            this.masterConfigurationCurrentLimitNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.masterConfigurationCurrentLimitNumeric.Location = new System.Drawing.Point(131, 107);
            this.masterConfigurationCurrentLimitNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.masterConfigurationCurrentLimitNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.masterConfigurationCurrentLimitNumeric.Name = "masterConfigurationCurrentLimitNumeric";
            this.masterConfigurationCurrentLimitNumeric.Size = new System.Drawing.Size(90, 20);
            this.masterConfigurationCurrentLimitNumeric.TabIndex = 4;
            this.masterConfigurationCurrentLimitNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            131072});
            // 
            // masterConfigurationVoltageLevelNumeric
            // 
            this.masterConfigurationVoltageLevelNumeric.DecimalPlaces = 6;
            this.masterConfigurationVoltageLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.masterConfigurationVoltageLevelNumeric.Location = new System.Drawing.Point(131, 81);
            this.masterConfigurationVoltageLevelNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.masterConfigurationVoltageLevelNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.masterConfigurationVoltageLevelNumeric.Name = "masterConfigurationVoltageLevelNumeric";
            this.masterConfigurationVoltageLevelNumeric.Size = new System.Drawing.Size(90, 20);
            this.masterConfigurationVoltageLevelNumeric.TabIndex = 3;
            this.masterConfigurationVoltageLevelNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // masterConfigurationSourceDelayNumeric
            // 
            this.masterConfigurationSourceDelayNumeric.DecimalPlaces = 6;
            this.masterConfigurationSourceDelayNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.masterConfigurationSourceDelayNumeric.Location = new System.Drawing.Point(131, 133);
            this.masterConfigurationSourceDelayNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.masterConfigurationSourceDelayNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.masterConfigurationSourceDelayNumeric.Name = "masterConfigurationSourceDelayNumeric";
            this.masterConfigurationSourceDelayNumeric.Size = new System.Drawing.Size(90, 20);
            this.masterConfigurationSourceDelayNumeric.TabIndex = 5;
            this.masterConfigurationSourceDelayNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(223, 344);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 7;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // masterConfigurationGroupBox
            // 
            this.masterConfigurationGroupBox.Controls.Add(this.masterConfigurationChannelNameTextBox);
            this.masterConfigurationGroupBox.Controls.Add(this.masterConfigurationResourceNameComboBox);
            this.masterConfigurationGroupBox.Controls.Add(this.masterConfigurationChannelNameLabel);
            this.masterConfigurationGroupBox.Controls.Add(this.masterConfigurationDelayLabel);
            this.masterConfigurationGroupBox.Controls.Add(this.masterConfigurationCurrentLimitLabel);
            this.masterConfigurationGroupBox.Controls.Add(this.masterConfigurationResourceNameLabel);
            this.masterConfigurationGroupBox.Controls.Add(this.masterConfigurationVoltageLevelLabel);
            this.masterConfigurationGroupBox.Controls.Add(this.masterConfigurationVoltageLevelNumeric);
            this.masterConfigurationGroupBox.Controls.Add(this.masterConfigurationCurrentLimitNumeric);
            this.masterConfigurationGroupBox.Controls.Add(this.masterConfigurationSourceDelayNumeric);
            this.masterConfigurationGroupBox.Location = new System.Drawing.Point(12, 12);
            this.masterConfigurationGroupBox.Name = "masterConfigurationGroupBox";
            this.masterConfigurationGroupBox.Size = new System.Drawing.Size(231, 164);
            this.masterConfigurationGroupBox.TabIndex = 0;
            this.masterConfigurationGroupBox.TabStop = false;
            this.masterConfigurationGroupBox.Text = "Master Configuration";
            // 
            // masterConfigurationChannelNameTextBox
            // 
            this.masterConfigurationChannelNameTextBox.Location = new System.Drawing.Point(131, 46);
            this.masterConfigurationChannelNameTextBox.Name = "masterConfigurationChannelNameTextBox";
            this.masterConfigurationChannelNameTextBox.Size = new System.Drawing.Size(90, 20);
            this.masterConfigurationChannelNameTextBox.TabIndex = 2;
            this.masterConfigurationChannelNameTextBox.Text = "0";
            // 
            // masterConfigurationResourceNameComboBox
            // 
            this.masterConfigurationResourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.masterConfigurationResourceNameComboBox.FormattingEnabled = true;
            this.masterConfigurationResourceNameComboBox.Location = new System.Drawing.Point(96, 19);
            this.masterConfigurationResourceNameComboBox.Name = "masterConfigurationResourceNameComboBox";
            this.masterConfigurationResourceNameComboBox.Size = new System.Drawing.Size(125, 21);
            this.masterConfigurationResourceNameComboBox.TabIndex = 1;
            // 
            // masterConfigurationChannelNameLabel
            // 
            this.masterConfigurationChannelNameLabel.AutoSize = true;
            this.masterConfigurationChannelNameLabel.Location = new System.Drawing.Point(6, 50);
            this.masterConfigurationChannelNameLabel.Name = "masterConfigurationChannelNameLabel";
            this.masterConfigurationChannelNameLabel.Size = new System.Drawing.Size(77, 13);
            this.masterConfigurationChannelNameLabel.TabIndex = 2;
            this.masterConfigurationChannelNameLabel.Text = "Channel Name";
            // 
            // masterConfigurationResourceNameLabel
            // 
            this.masterConfigurationResourceNameLabel.AutoSize = true;
            this.masterConfigurationResourceNameLabel.Location = new System.Drawing.Point(6, 22);
            this.masterConfigurationResourceNameLabel.Name = "masterConfigurationResourceNameLabel";
            this.masterConfigurationResourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.masterConfigurationResourceNameLabel.TabIndex = 0;
            this.masterConfigurationResourceNameLabel.Text = "Resource Name";
            // 
            // masterMeasurementsGroupBox
            // 
            this.masterMeasurementsGroupBox.Controls.Add(this.masterMeasurementsVoltageTextBox);
            this.masterMeasurementsGroupBox.Controls.Add(this.masterMeasurementsCurrentTextBox);
            this.masterMeasurementsGroupBox.Controls.Add(this.masterMeasurementsCurrentLabel);
            this.masterMeasurementsGroupBox.Controls.Add(this.masterMeasurementsVoltageLabel);
            this.masterMeasurementsGroupBox.Location = new System.Drawing.Point(259, 12);
            this.masterMeasurementsGroupBox.Name = "masterMeasurementsGroupBox";
            this.masterMeasurementsGroupBox.Size = new System.Drawing.Size(249, 74);
            this.masterMeasurementsGroupBox.TabIndex = 1;
            this.masterMeasurementsGroupBox.TabStop = false;
            this.masterMeasurementsGroupBox.Text = "Master Measurements";
            // 
            // masterMeasurementsVoltageTextBox
            // 
            this.masterMeasurementsVoltageTextBox.Location = new System.Drawing.Point(127, 19);
            this.masterMeasurementsVoltageTextBox.Name = "masterMeasurementsVoltageTextBox";
            this.masterMeasurementsVoltageTextBox.ReadOnly = true;
            this.masterMeasurementsVoltageTextBox.Size = new System.Drawing.Size(116, 20);
            this.masterMeasurementsVoltageTextBox.TabIndex = 8;
            this.masterMeasurementsVoltageTextBox.Text = "0.000000E+000";
            // 
            // masterMeasurementsCurrentTextBox
            // 
            this.masterMeasurementsCurrentTextBox.Location = new System.Drawing.Point(127, 45);
            this.masterMeasurementsCurrentTextBox.Name = "masterMeasurementsCurrentTextBox";
            this.masterMeasurementsCurrentTextBox.ReadOnly = true;
            this.masterMeasurementsCurrentTextBox.Size = new System.Drawing.Size(116, 20);
            this.masterMeasurementsCurrentTextBox.TabIndex = 9;
            this.masterMeasurementsCurrentTextBox.Text = "0.000000E+000";
            // 
            // masterMeasurementsCurrentLabel
            // 
            this.masterMeasurementsCurrentLabel.AutoSize = true;
            this.masterMeasurementsCurrentLabel.Location = new System.Drawing.Point(6, 49);
            this.masterMeasurementsCurrentLabel.Name = "masterMeasurementsCurrentLabel";
            this.masterMeasurementsCurrentLabel.Size = new System.Drawing.Size(57, 13);
            this.masterMeasurementsCurrentLabel.TabIndex = 2;
            this.masterMeasurementsCurrentLabel.Text = "Current (A)";
            // 
            // masterMeasurementsVoltageLabel
            // 
            this.masterMeasurementsVoltageLabel.AutoSize = true;
            this.masterMeasurementsVoltageLabel.Location = new System.Drawing.Point(6, 23);
            this.masterMeasurementsVoltageLabel.Name = "masterMeasurementsVoltageLabel";
            this.masterMeasurementsVoltageLabel.Size = new System.Drawing.Size(59, 13);
            this.masterMeasurementsVoltageLabel.TabIndex = 0;
            this.masterMeasurementsVoltageLabel.Text = "Voltage (V)";
            // 
            // messageRichTextBox
            // 
            this.messageRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageRichTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageRichTextBox.Location = new System.Drawing.Point(162, 209);
            this.messageRichTextBox.Name = "messageRichTextBox";
            this.messageRichTextBox.ReadOnly = true;
            this.messageRichTextBox.Size = new System.Drawing.Size(197, 19);
            this.messageRichTextBox.TabIndex = 3;
            this.messageRichTextBox.TabStop = false;
            this.messageRichTextBox.Text = "This example requires 3 devices to work.";
            // 
            // slavesConfigurationGroupBox
            // 
            this.slavesConfigurationGroupBox.Controls.Add(this.slavesConfigurationDataGridView);
            this.slavesConfigurationGroupBox.Location = new System.Drawing.Point(12, 239);
            this.slavesConfigurationGroupBox.Name = "slavesConfigurationGroupBox";
            this.slavesConfigurationGroupBox.Size = new System.Drawing.Size(496, 96);
            this.slavesConfigurationGroupBox.TabIndex = 4;
            this.slavesConfigurationGroupBox.TabStop = false;
            this.slavesConfigurationGroupBox.Text = "Slave(s) Configuration";
            // 
            // slavesConfigurationDataGridView
            // 
            this.slavesConfigurationDataGridView.AllowUserToAddRows = false;
            this.slavesConfigurationDataGridView.AllowUserToDeleteRows = false;
            this.slavesConfigurationDataGridView.AllowUserToResizeColumns = false;
            this.slavesConfigurationDataGridView.AllowUserToResizeRows = false;
            this.slavesConfigurationDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.slavesConfigurationDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.slavesConfigurationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.slavesConfigurationDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.slavesConfigurationSlaveNumberColumn,
            this.slavesConfigurationResourceNameColumn,
            this.slavesConfigurationChannelNameColumn,
            this.slavesConfigurationVoltageLevelColumn,
            this.slavesConfigurationCurrentLimitColumn});
            this.slavesConfigurationDataGridView.Location = new System.Drawing.Point(6, 19);
            this.slavesConfigurationDataGridView.Name = "slavesConfigurationDataGridView";
            this.slavesConfigurationDataGridView.RowHeadersVisible = false;
            this.slavesConfigurationDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.slavesConfigurationDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.slavesConfigurationDataGridView.Size = new System.Drawing.Size(483, 69);
            this.slavesConfigurationDataGridView.StandardTab = true;
            this.slavesConfigurationDataGridView.TabIndex = 6;
            // 
            // slavesConfigurationSlaveNumberColumn
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.slavesConfigurationSlaveNumberColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.slavesConfigurationSlaveNumberColumn.Frozen = true;
            this.slavesConfigurationSlaveNumberColumn.HeaderText = "";
            this.slavesConfigurationSlaveNumberColumn.Name = "slavesConfigurationSlaveNumberColumn";
            this.slavesConfigurationSlaveNumberColumn.ReadOnly = true;
            this.slavesConfigurationSlaveNumberColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.slavesConfigurationSlaveNumberColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slavesConfigurationSlaveNumberColumn.Width = 50;
            // 
            // slavesConfigurationResourceNameColumn
            // 
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.slavesConfigurationResourceNameColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.slavesConfigurationResourceNameColumn.HeaderText = "Resource Name";
            this.slavesConfigurationResourceNameColumn.Name = "slavesConfigurationResourceNameColumn";
            this.slavesConfigurationResourceNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.slavesConfigurationResourceNameColumn.Width = 115;
            // 
            // slavesConfigurationChannelNameColumn
            // 
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            this.slavesConfigurationChannelNameColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.slavesConfigurationChannelNameColumn.HeaderText = "Channel Name";
            this.slavesConfigurationChannelNameColumn.Name = "slavesConfigurationChannelNameColumn";
            this.slavesConfigurationChannelNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.slavesConfigurationChannelNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slavesConfigurationChannelNameColumn.Width = 95;
            // 
            // slavesConfigurationVoltageLevelColumn
            // 
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            this.slavesConfigurationVoltageLevelColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.slavesConfigurationVoltageLevelColumn.HeaderText = "Voltage Level (V)";
            this.slavesConfigurationVoltageLevelColumn.Name = "slavesConfigurationVoltageLevelColumn";
            this.slavesConfigurationVoltageLevelColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.slavesConfigurationVoltageLevelColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slavesConfigurationVoltageLevelColumn.Width = 110;
            // 
            // slavesConfigurationCurrentLimitColumn
            // 
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            this.slavesConfigurationCurrentLimitColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.slavesConfigurationCurrentLimitColumn.HeaderText = "Current Limit (A)";
            this.slavesConfigurationCurrentLimitColumn.Name = "slavesConfigurationCurrentLimitColumn";
            this.slavesConfigurationCurrentLimitColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.slavesConfigurationCurrentLimitColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slavesConfigurationCurrentLimitColumn.Width = 110;
            // 
            // slavesMeasurementsGroupBox
            // 
            this.slavesMeasurementsGroupBox.Controls.Add(this.slavesMeasurementsDataGridView);
            this.slavesMeasurementsGroupBox.Location = new System.Drawing.Point(259, 97);
            this.slavesMeasurementsGroupBox.Name = "slavesMeasurementsGroupBox";
            this.slavesMeasurementsGroupBox.Size = new System.Drawing.Size(249, 96);
            this.slavesMeasurementsGroupBox.TabIndex = 2;
            this.slavesMeasurementsGroupBox.TabStop = false;
            this.slavesMeasurementsGroupBox.Text = "Slave(s) Measurements";
            // 
            // slavesMeasurementsDataGridView
            // 
            this.slavesMeasurementsDataGridView.AllowUserToAddRows = false;
            this.slavesMeasurementsDataGridView.AllowUserToDeleteRows = false;
            this.slavesMeasurementsDataGridView.AllowUserToResizeColumns = false;
            this.slavesMeasurementsDataGridView.AllowUserToResizeRows = false;
            this.slavesMeasurementsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.slavesMeasurementsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.slavesMeasurementsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.slavesMeasurementsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.slavesMesurementsSlaveNumberColumn,
            this.slavesMeasurementsVoltageColumn,
            this.slavesMeasurementsCurrentColumn});
            this.slavesMeasurementsDataGridView.Location = new System.Drawing.Point(9, 19);
            this.slavesMeasurementsDataGridView.Name = "slavesMeasurementsDataGridView";
            this.slavesMeasurementsDataGridView.RowHeadersVisible = false;
            this.slavesMeasurementsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.slavesMeasurementsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.slavesMeasurementsDataGridView.Size = new System.Drawing.Size(233, 69);
            this.slavesMeasurementsDataGridView.StandardTab = true;
            this.slavesMeasurementsDataGridView.TabIndex = 10;
            // 
            // slavesMesurementsSlaveNumberColumn
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black;
            this.slavesMesurementsSlaveNumberColumn.DefaultCellStyle = dataGridViewCellStyle8;
            this.slavesMesurementsSlaveNumberColumn.Frozen = true;
            this.slavesMesurementsSlaveNumberColumn.HeaderText = "";
            this.slavesMesurementsSlaveNumberColumn.Name = "slavesMesurementsSlaveNumberColumn";
            this.slavesMesurementsSlaveNumberColumn.ReadOnly = true;
            this.slavesMesurementsSlaveNumberColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.slavesMesurementsSlaveNumberColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slavesMesurementsSlaveNumberColumn.Width = 50;
            // 
            // slavesMeasurementsVoltageColumn
            // 
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.Black;
            this.slavesMeasurementsVoltageColumn.DefaultCellStyle = dataGridViewCellStyle9;
            this.slavesMeasurementsVoltageColumn.HeaderText = "Voltage (V)";
            this.slavesMeasurementsVoltageColumn.Name = "slavesMeasurementsVoltageColumn";
            this.slavesMeasurementsVoltageColumn.ReadOnly = true;
            this.slavesMeasurementsVoltageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.slavesMeasurementsVoltageColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slavesMeasurementsVoltageColumn.Width = 90;
            // 
            // slavesMeasurementsCurrentColumn
            // 
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.Black;
            this.slavesMeasurementsCurrentColumn.DefaultCellStyle = dataGridViewCellStyle10;
            this.slavesMeasurementsCurrentColumn.HeaderText = "Current (A)";
            this.slavesMeasurementsCurrentColumn.Name = "slavesMeasurementsCurrentColumn";
            this.slavesMeasurementsCurrentColumn.ReadOnly = true;
            this.slavesMeasurementsCurrentColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.slavesMeasurementsCurrentColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slavesMeasurementsCurrentColumn.Width = 90;
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 378);
            this.Controls.Add(this.slavesMeasurementsGroupBox);
            this.Controls.Add(this.slavesConfigurationGroupBox);
            this.Controls.Add(this.messageRichTextBox);
            this.Controls.Add(this.masterMeasurementsGroupBox);
            this.Controls.Add(this.masterConfigurationGroupBox);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Single Point Multi-Channel Synchronization";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.masterConfigurationCurrentLimitNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.masterConfigurationVoltageLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.masterConfigurationSourceDelayNumeric)).EndInit();
            this.masterConfigurationGroupBox.ResumeLayout(false);
            this.masterConfigurationGroupBox.PerformLayout();
            this.masterMeasurementsGroupBox.ResumeLayout(false);
            this.masterMeasurementsGroupBox.PerformLayout();
            this.slavesConfigurationGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.slavesConfigurationDataGridView)).EndInit();
            this.slavesMeasurementsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.slavesMeasurementsDataGridView)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label masterConfigurationCurrentLimitLabel;
        private System.Windows.Forms.Label masterConfigurationVoltageLevelLabel;
        private System.Windows.Forms.Label masterConfigurationDelayLabel;
        private System.Windows.Forms.NumericUpDown masterConfigurationCurrentLimitNumeric;
        private System.Windows.Forms.NumericUpDown masterConfigurationVoltageLevelNumeric;
        private System.Windows.Forms.NumericUpDown masterConfigurationSourceDelayNumeric;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox masterConfigurationGroupBox;
        private System.Windows.Forms.ComboBox masterConfigurationResourceNameComboBox;
        private System.Windows.Forms.Label masterConfigurationChannelNameLabel;
        private System.Windows.Forms.Label masterConfigurationResourceNameLabel;
        private System.Windows.Forms.GroupBox masterMeasurementsGroupBox;
        private System.Windows.Forms.TextBox masterMeasurementsVoltageTextBox;
        private System.Windows.Forms.TextBox masterMeasurementsCurrentTextBox;
        private System.Windows.Forms.Label masterMeasurementsCurrentLabel;
        private System.Windows.Forms.Label masterMeasurementsVoltageLabel;
        private System.Windows.Forms.RichTextBox messageRichTextBox;
        private System.Windows.Forms.GroupBox slavesConfigurationGroupBox;
        private System.Windows.Forms.GroupBox slavesMeasurementsGroupBox;
        private System.Windows.Forms.DataGridView slavesConfigurationDataGridView;
        private System.Windows.Forms.DataGridView slavesMeasurementsDataGridView;
        private System.Windows.Forms.TextBox masterConfigurationChannelNameTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn slavesMesurementsSlaveNumberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn slavesMeasurementsVoltageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn slavesMeasurementsCurrentColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn slavesConfigurationSlaveNumberColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn slavesConfigurationResourceNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn slavesConfigurationChannelNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn slavesConfigurationVoltageLevelColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn slavesConfigurationCurrentLimitColumn;

    }
}
