namespace NationalInstruments.Examples.AdvancedSequenceOutputFunction
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
            this.resourcenameLabel = new System.Windows.Forms.Label();
            this.channelnameLabel = new System.Windows.Forms.Label();
            this.stepsLabel = new System.Windows.Forms.Label();
            this.currentlevelstartLabel = new System.Windows.Forms.Label();
            this.voltagelevelstartLabel = new System.Windows.Forms.Label();
            this.currentlevelstopLabel = new System.Windows.Forms.Label();
            this.voltagelevelstopLabel = new System.Windows.Forms.Label();
            this.delayLabel = new System.Windows.Forms.Label();
            this.stepsNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentLevelStartNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageLevelStartNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentLevelStopNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageLevelStopNumeric = new System.Windows.Forms.NumericUpDown();
            this.delayNumeric = new System.Windows.Forms.NumericUpDown();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameAndChannelNameGroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.measurementsDataGridView = new System.Windows.Forms.DataGridView();
            this.Point = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Voltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Current = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.measurementsGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.stepsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelStartNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelStartNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelStopNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelStopNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delayNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            this.resourceNameAndChannelNameGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).BeginInit();
            this.measurementsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // resourcenameLabel
            // 
            this.resourcenameLabel.AutoSize = true;
            this.resourcenameLabel.Location = new System.Drawing.Point(10, 22);
            this.resourcenameLabel.Name = "resourcenameLabel";
            this.resourcenameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourcenameLabel.TabIndex = 0;
            this.resourcenameLabel.Text = "Resource Name";
            // 
            // channelnameLabel
            // 
            this.channelnameLabel.AutoSize = true;
            this.channelnameLabel.Location = new System.Drawing.Point(10, 47);
            this.channelnameLabel.Name = "channelnameLabel";
            this.channelnameLabel.Size = new System.Drawing.Size(77, 13);
            this.channelnameLabel.TabIndex = 1;
            this.channelnameLabel.Text = "Channel Name";
            // 
            // stepsLabel
            // 
            this.stepsLabel.AutoSize = true;
            this.stepsLabel.Location = new System.Drawing.Point(10, 21);
            this.stepsLabel.Name = "stepsLabel";
            this.stepsLabel.Size = new System.Drawing.Size(133, 13);
            this.stepsLabel.TabIndex = 2;
            this.stepsLabel.Text = "Points per Output Function";
            // 
            // currentlevelstartLabel
            // 
            this.currentlevelstartLabel.AutoSize = true;
            this.currentlevelstartLabel.Location = new System.Drawing.Point(10, 47);
            this.currentlevelstartLabel.Name = "currentlevelstartLabel";
            this.currentlevelstartLabel.Size = new System.Drawing.Size(108, 13);
            this.currentlevelstartLabel.TabIndex = 3;
            this.currentlevelstartLabel.Text = "Current Level Start(A)";
            // 
            // voltagelevelstartLabel
            // 
            this.voltagelevelstartLabel.AutoSize = true;
            this.voltagelevelstartLabel.Location = new System.Drawing.Point(10, 74);
            this.voltagelevelstartLabel.Name = "voltagelevelstartLabel";
            this.voltagelevelstartLabel.Size = new System.Drawing.Size(110, 13);
            this.voltagelevelstartLabel.TabIndex = 4;
            this.voltagelevelstartLabel.Text = "Voltage Level Start(V)";
            // 
            // currentlevelstopLabel
            // 
            this.currentlevelstopLabel.AutoSize = true;
            this.currentlevelstopLabel.Location = new System.Drawing.Point(10, 102);
            this.currentlevelstopLabel.Name = "currentlevelstopLabel";
            this.currentlevelstopLabel.Size = new System.Drawing.Size(111, 13);
            this.currentlevelstopLabel.TabIndex = 5;
            this.currentlevelstopLabel.Text = "Current Level Stop (A)";
            // 
            // voltagelevelstopLabel
            // 
            this.voltagelevelstopLabel.AutoSize = true;
            this.voltagelevelstopLabel.Location = new System.Drawing.Point(10, 126);
            this.voltagelevelstopLabel.Name = "voltagelevelstopLabel";
            this.voltagelevelstopLabel.Size = new System.Drawing.Size(113, 13);
            this.voltagelevelstopLabel.TabIndex = 6;
            this.voltagelevelstopLabel.Text = "Voltage Level Stop (V)";
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new System.Drawing.Point(10, 152);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(85, 13);
            this.delayLabel.TabIndex = 7;
            this.delayLabel.Text = "Source Delay (s)";
            // 
            // stepsNumeric
            // 
            this.stepsNumeric.Location = new System.Drawing.Point(172, 19);
            this.stepsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.stepsNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.stepsNumeric.Name = "stepsNumeric";
            this.stepsNumeric.Size = new System.Drawing.Size(90, 20);
            this.stepsNumeric.TabIndex = 2;
            this.stepsNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // currentLevelStartNumeric
            // 
            this.currentLevelStartNumeric.DecimalPlaces = 5;
            this.currentLevelStartNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.currentLevelStartNumeric.Location = new System.Drawing.Point(172, 45);
            this.currentLevelStartNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.currentLevelStartNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.currentLevelStartNumeric.Name = "currentLevelStartNumeric";
            this.currentLevelStartNumeric.Size = new System.Drawing.Size(90, 20);
            this.currentLevelStartNumeric.TabIndex = 3;
            this.currentLevelStartNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            // 
            // voltageLevelStartNumeric
            // 
            this.voltageLevelStartNumeric.DecimalPlaces = 5;
            this.voltageLevelStartNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.voltageLevelStartNumeric.Location = new System.Drawing.Point(172, 72);
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
            this.voltageLevelStartNumeric.TabIndex = 4;
            this.voltageLevelStartNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // currentLevelStopNumeric
            // 
            this.currentLevelStopNumeric.DecimalPlaces = 5;
            this.currentLevelStopNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.currentLevelStopNumeric.Location = new System.Drawing.Point(172, 100);
            this.currentLevelStopNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.currentLevelStopNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.currentLevelStopNumeric.Name = "currentLevelStopNumeric";
            this.currentLevelStopNumeric.Size = new System.Drawing.Size(90, 20);
            this.currentLevelStopNumeric.TabIndex = 5;
            this.currentLevelStopNumeric.Value = new decimal(new int[] {
            3,
            0,
            0,
            262144});
            // 
            // voltageLevelStopNumeric
            // 
            this.voltageLevelStopNumeric.DecimalPlaces = 5;
            this.voltageLevelStopNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.voltageLevelStopNumeric.Location = new System.Drawing.Point(172, 124);
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
            this.voltageLevelStopNumeric.TabIndex = 6;
            this.voltageLevelStopNumeric.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // delayNumeric
            // 
            this.delayNumeric.DecimalPlaces = 3;
            this.delayNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.delayNumeric.Location = new System.Drawing.Point(172, 150);
            this.delayNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.delayNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.delayNumeric.Name = "delayNumeric";
            this.delayNumeric.Size = new System.Drawing.Size(90, 20);
            this.delayNumeric.TabIndex = 7;
            this.delayNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(131, 44);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(131, 20);
            this.channelNameTextBox.TabIndex = 1;
            this.channelNameTextBox.Text = "0";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(253, 292);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 8;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.stepsNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLevelStartNumeric);
            this.configurationGroupBox.Controls.Add(this.voltageLevelStartNumeric);
            this.configurationGroupBox.Controls.Add(this.currentLevelStopNumeric);
            this.configurationGroupBox.Controls.Add(this.voltageLevelStopNumeric);
            this.configurationGroupBox.Controls.Add(this.delayNumeric);
            this.configurationGroupBox.Controls.Add(this.stepsLabel);
            this.configurationGroupBox.Controls.Add(this.currentlevelstartLabel);
            this.configurationGroupBox.Controls.Add(this.voltagelevelstartLabel);
            this.configurationGroupBox.Controls.Add(this.currentlevelstopLabel);
            this.configurationGroupBox.Controls.Add(this.voltagelevelstopLabel);
            this.configurationGroupBox.Controls.Add(this.delayLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(15, 96);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(276, 180);
            this.configurationGroupBox.TabIndex = 11;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // resourceNameAndChannelNameGroupBox
            // 
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.resourceNameComboBox);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.channelNameTextBox);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.resourcenameLabel);
            this.resourceNameAndChannelNameGroupBox.Controls.Add(this.channelnameLabel);
            this.resourceNameAndChannelNameGroupBox.Location = new System.Drawing.Point(15, 15);
            this.resourceNameAndChannelNameGroupBox.Name = "resourceNameAndChannelNameGroupBox";
            this.resourceNameAndChannelNameGroupBox.Size = new System.Drawing.Size(276, 75);
            this.resourceNameAndChannelNameGroupBox.TabIndex = 12;
            this.resourceNameAndChannelNameGroupBox.TabStop = false;
            this.resourceNameAndChannelNameGroupBox.Text = "Resource Name and Channel Name";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.resourceNameComboBox.Location = new System.Drawing.Point(131, 19);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(131, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // measurementsDataGridView
            // 
            this.measurementsDataGridView.AllowUserToAddRows = false;
            this.measurementsDataGridView.AllowUserToDeleteRows = false;
            this.measurementsDataGridView.AllowUserToResizeColumns = false;
            this.measurementsDataGridView.AllowUserToResizeRows = false;
            this.measurementsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.measurementsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Point,
            this.Voltage,
            this.Current});
            this.measurementsDataGridView.Location = new System.Drawing.Point(6, 19);
            this.measurementsDataGridView.Name = "measurementsDataGridView";
            this.measurementsDataGridView.RowHeadersVisible = false;
            this.measurementsDataGridView.Size = new System.Drawing.Size(224, 232);
            this.measurementsDataGridView.TabIndex = 9;
            // 
            // Point
            // 
            this.Point.HeaderText = "Point";
            this.Point.MinimumWidth = 3;
            this.Point.Name = "Point";
            this.Point.Width = 40;
            // 
            // Voltage
            // 
            this.Voltage.FillWeight = 90F;
            this.Voltage.HeaderText = "Voltage (V)";
            this.Voltage.Name = "Voltage";
            this.Voltage.Width = 90;
            // 
            // Current
            // 
            this.Current.FillWeight = 90F;
            this.Current.HeaderText = "Current (A)";
            this.Current.Name = "Current";
            this.Current.Width = 90;
            // 
            // measurementsGroupBox
            // 
            this.measurementsGroupBox.Controls.Add(this.measurementsDataGridView);
            this.measurementsGroupBox.Location = new System.Drawing.Point(297, 15);
            this.measurementsGroupBox.Name = "measurementsGroupBox";
            this.measurementsGroupBox.Size = new System.Drawing.Size(236, 261);
            this.measurementsGroupBox.TabIndex = 14;
            this.measurementsGroupBox.TabStop = false;
            this.measurementsGroupBox.Text = "Measurements";
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 324);
            this.Controls.Add(this.measurementsGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.resourceNameAndChannelNameGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Advanced Sequence Output Function";
            ((System.ComponentModel.ISupportInitialize)(this.stepsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelStartNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelStartNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentLevelStopNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelStopNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delayNumeric)).EndInit();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.resourceNameAndChannelNameGroupBox.ResumeLayout(false);
            this.resourceNameAndChannelNameGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measurementsDataGridView)).EndInit();
            this.measurementsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label resourcenameLabel;
        private System.Windows.Forms.Label channelnameLabel;
        private System.Windows.Forms.Label stepsLabel;
        private System.Windows.Forms.Label currentlevelstartLabel;
        private System.Windows.Forms.Label voltagelevelstartLabel;
        private System.Windows.Forms.Label currentlevelstopLabel;
        private System.Windows.Forms.Label voltagelevelstopLabel;
        private System.Windows.Forms.Label delayLabel;
        private System.Windows.Forms.NumericUpDown stepsNumeric;
        private System.Windows.Forms.NumericUpDown currentLevelStartNumeric;
        private System.Windows.Forms.NumericUpDown voltageLevelStartNumeric;
        private System.Windows.Forms.NumericUpDown currentLevelStopNumeric;
        private System.Windows.Forms.NumericUpDown voltageLevelStopNumeric;
        private System.Windows.Forms.NumericUpDown delayNumeric;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox resourceNameAndChannelNameGroupBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.DataGridView measurementsDataGridView;
        private System.Windows.Forms.GroupBox measurementsGroupBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn Point;
        private System.Windows.Forms.DataGridViewTextBoxColumn Voltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn Current;


    }
}
