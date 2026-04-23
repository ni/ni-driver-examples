namespace NationalInstruments.Examples.AdvancedSequenceApertureTime
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.voltageMeasurmentsLabel = new System.Windows.Forms.Label();
            this.apertureTimesLabel = new System.Windows.Forms.Label();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.voltagemeasurments1Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aperturetimes1Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.voltageMeasurmentsDataGridView = new System.Windows.Forms.DataGridView();
            this.apertureTimesDataGridView = new System.Windows.Forms.DataGridView();
            this.resourceNameAndChannelNameGroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.voltageLevelLabel = new System.Windows.Forms.Label();
            this.voltageLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.sourceDelayLabel = new System.Windows.Forms.Label();
            this.sourceDelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.voltageMeasurmentsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.apertureTimesDataGridView)).BeginInit();
            this.resourceNameAndChannelNameGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDelayNumeric)).BeginInit();
            this.measurementGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 23);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // channelNameLabel
            // 
            this.channelNameLabel.AutoSize = true;
            this.channelNameLabel.Location = new System.Drawing.Point(6, 48);
            this.channelNameLabel.Name = "channelNameLabel";
            this.channelNameLabel.Size = new System.Drawing.Size(77, 13);
            this.channelNameLabel.TabIndex = 1;
            this.channelNameLabel.Text = "Channel Name";
            // 
            // voltageMeasurmentsLabel
            // 
            this.voltageMeasurmentsLabel.AutoSize = true;
            this.voltageMeasurmentsLabel.Location = new System.Drawing.Point(6, 19);
            this.voltageMeasurmentsLabel.Name = "voltageMeasurmentsLabel";
            this.voltageMeasurmentsLabel.Size = new System.Drawing.Size(131, 13);
            this.voltageMeasurmentsLabel.TabIndex = 4;
            this.voltageMeasurmentsLabel.Text = "Voltage Measurements (V)";
            // 
            // apertureTimesLabel
            // 
            this.apertureTimesLabel.AutoSize = true;
            this.apertureTimesLabel.Location = new System.Drawing.Point(4, 100);
            this.apertureTimesLabel.Name = "apertureTimesLabel";
            this.apertureTimesLabel.Size = new System.Drawing.Size(114, 13);
            this.apertureTimesLabel.TabIndex = 5;
            this.apertureTimesLabel.Text = "Aperture Time Array (s)";
            // 
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(132, 45);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(126, 20);
            this.channelNameTextBox.TabIndex = 1;
            this.channelNameTextBox.Text = "0";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(313, 181);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // voltagemeasurments1Column
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.voltagemeasurments1Column.DefaultCellStyle = dataGridViewCellStyle1;
            this.voltagemeasurments1Column.Frozen = true;
            this.voltagemeasurments1Column.HeaderText = "";
            this.voltagemeasurments1Column.Name = "voltagemeasurments1Column";
            this.voltagemeasurments1Column.ReadOnly = true;
            this.voltagemeasurments1Column.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.voltagemeasurments1Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.voltagemeasurments1Column.Width = 99;
            // 
            // aperturetimes1Column
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.aperturetimes1Column.DefaultCellStyle = dataGridViewCellStyle2;
            this.aperturetimes1Column.Frozen = true;
            this.aperturetimes1Column.HeaderText = "";
            this.aperturetimes1Column.Name = "aperturetimes1Column";
            this.aperturetimes1Column.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.aperturetimes1Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.aperturetimes1Column.Width = 99;
            // 
            // voltageMeasurmentsDataGridView
            // 
            this.voltageMeasurmentsDataGridView.AllowUserToAddRows = false;
            this.voltageMeasurmentsDataGridView.AllowUserToDeleteRows = false;
            this.voltageMeasurmentsDataGridView.AllowUserToResizeColumns = false;
            this.voltageMeasurmentsDataGridView.AllowUserToResizeRows = false;
            this.voltageMeasurmentsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.voltageMeasurmentsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.voltageMeasurmentsDataGridView.ColumnHeadersVisible = false;
            this.voltageMeasurmentsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.voltagemeasurments1Column});
            this.voltageMeasurmentsDataGridView.Location = new System.Drawing.Point(6, 35);
            this.voltageMeasurmentsDataGridView.Name = "voltageMeasurmentsDataGridView";
            this.voltageMeasurmentsDataGridView.ReadOnly = true;
            this.voltageMeasurmentsDataGridView.RowHeadersVisible = false;
            this.voltageMeasurmentsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.voltageMeasurmentsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.voltageMeasurmentsDataGridView.Size = new System.Drawing.Size(102, 69);
            this.voltageMeasurmentsDataGridView.StandardTab = true;
            this.voltageMeasurmentsDataGridView.TabIndex = 6;
            // 
            // apertureTimesDataGridView
            // 
            this.apertureTimesDataGridView.AllowUserToAddRows = false;
            this.apertureTimesDataGridView.AllowUserToDeleteRows = false;
            this.apertureTimesDataGridView.AllowUserToResizeColumns = false;
            this.apertureTimesDataGridView.AllowUserToResizeRows = false;
            this.apertureTimesDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.apertureTimesDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.apertureTimesDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.apertureTimesDataGridView.ColumnHeadersVisible = false;
            this.apertureTimesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.aperturetimes1Column});
            this.apertureTimesDataGridView.Location = new System.Drawing.Point(132, 68);
            this.apertureTimesDataGridView.Name = "apertureTimesDataGridView";
            this.apertureTimesDataGridView.RowHeadersVisible = false;
            this.apertureTimesDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.apertureTimesDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.apertureTimesDataGridView.Size = new System.Drawing.Size(99, 66);
            this.apertureTimesDataGridView.StandardTab = true;
            this.apertureTimesDataGridView.TabIndex = 4;
            // 
            // resourceNameAndChannelNameGroupBox
            // 
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.resourceNameComboBox);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.channelNameLabel);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.resourceNameLabel);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.channelNameTextBox);
            this.resourceNameAndChannelNameGroupBox.Location = new System.Drawing.Point(12, 12);
            this.resourceNameAndChannelNameGroupBox.Name = "resourceNameAndChannelNameGroupBox";
            this.resourceNameAndChannelNameGroupBox.Size = new System.Drawing.Size(264, 72);
            this.resourceNameAndChannelNameGroupBox.TabIndex = 7;
            this.resourceNameAndChannelNameGroupBox.TabStop = false;
            this.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(132, 20);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(126, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.voltageLevelLabel);
            this.configurationGroupBox.Controls.Add(this.voltageLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.sourceDelayLabel);
            this.configurationGroupBox.Controls.Add(this.apertureTimesLabel);
            this.configurationGroupBox.Controls.Add(this.sourceDelayNumeric);
            this.configurationGroupBox.Controls.Add(this.apertureTimesDataGridView);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 102);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(264, 142);
            this.configurationGroupBox.TabIndex = 8;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // voltageLevelLabel
            // 
            this.voltageLevelLabel.AutoSize = true;
            this.voltageLevelLabel.Location = new System.Drawing.Point(6, 18);
            this.voltageLevelLabel.Name = "voltageLevelLabel";
            this.voltageLevelLabel.Size = new System.Drawing.Size(88, 13);
            this.voltageLevelLabel.TabIndex = 2;
            this.voltageLevelLabel.Text = "Voltage Level (V)";
            // 
            // voltageLevelNumeric
            // 
            this.voltageLevelNumeric.DecimalPlaces = 5;
            this.voltageLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.voltageLevelNumeric.Location = new System.Drawing.Point(132, 16);
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
            this.voltageLevelNumeric.Size = new System.Drawing.Size(126, 20);
            this.voltageLevelNumeric.TabIndex = 2;
            this.voltageLevelNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // sourceDelayLabel
            // 
            this.sourceDelayLabel.AutoSize = true;
            this.sourceDelayLabel.Location = new System.Drawing.Point(6, 44);
            this.sourceDelayLabel.Name = "sourceDelayLabel";
            this.sourceDelayLabel.Size = new System.Drawing.Size(85, 13);
            this.sourceDelayLabel.TabIndex = 3;
            this.sourceDelayLabel.Text = "Source Delay (s)";
            // 
            // sourceDelayNumeric
            // 
            this.sourceDelayNumeric.DecimalPlaces = 5;
            this.sourceDelayNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.sourceDelayNumeric.Location = new System.Drawing.Point(132, 42);
            this.sourceDelayNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.sourceDelayNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.sourceDelayNumeric.Name = "sourceDelayNumeric";
            this.sourceDelayNumeric.Size = new System.Drawing.Size(126, 20);
            this.sourceDelayNumeric.TabIndex = 3;
            this.sourceDelayNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.voltageMeasurmentsLabel);
            this.measurementGroupBox.Controls.Add(this.voltageMeasurmentsDataGridView);
            this.measurementGroupBox.Location = new System.Drawing.Point(282, 12);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(138, 118);
            this.measurementGroupBox.TabIndex = 9;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurements";
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 250);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.resourceNameAndChannelNameGroupBox);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Advanced Sequencing Aperture Time";
            ((System.ComponentModel.ISupportInitialize)(this.voltageMeasurmentsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.apertureTimesDataGridView)).EndInit();
            this.resourceNameAndChannelNameGroupBox.ResumeLayout(false);
            this.resourceNameAndChannelNameGroupBox.PerformLayout();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDelayNumeric)).EndInit();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label voltageMeasurmentsLabel;
        private System.Windows.Forms.Label apertureTimesLabel;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn voltagemeasurments1Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn aperturetimes1Column;
        private System.Windows.Forms.DataGridView voltageMeasurmentsDataGridView;
        private System.Windows.Forms.DataGridView apertureTimesDataGridView;
        private System.Windows.Forms.GroupBox resourceNameAndChannelNameGroupBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.Label voltageLevelLabel;
        private System.Windows.Forms.NumericUpDown voltageLevelNumeric;
        private System.Windows.Forms.Label sourceDelayLabel;
        private System.Windows.Forms.NumericUpDown sourceDelayNumeric;
        private System.Windows.Forms.GroupBox measurementGroupBox;


    }
}
