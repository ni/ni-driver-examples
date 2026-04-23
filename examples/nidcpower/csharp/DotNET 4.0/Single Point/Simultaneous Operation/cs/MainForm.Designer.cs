namespace NationalInstruments.Examples.SimultaneousOperation
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
            this.firstChannelVoltageLevelLabel = new System.Windows.Forms.Label();
            this.firstChannelCurrentLimitLabel = new System.Windows.Forms.Label();
            this.firstChannelVoltagLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.firstChannelCurrentLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.secondChannelVoltageLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.secondChannelCurrentLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.resourceNameGroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.firstChannelNameLabel = new System.Windows.Forms.Label();
            this.firstChannelGroupBox = new System.Windows.Forms.GroupBox();
            this.firstChannelNameTextBox = new System.Windows.Forms.TextBox();
            this.firstChannelMeasurementsGroupBox = new System.Windows.Forms.GroupBox();
            this.firstChaannelVoltageMeasurementTextBox = new System.Windows.Forms.TextBox();
            this.firstChannelCurrentMeasurementTextBox = new System.Windows.Forms.TextBox();
            this.firstChannelCurrentMeasurementLabel = new System.Windows.Forms.Label();
            this.firstChannelVoltageMeasurementLabel = new System.Windows.Forms.Label();
            this.secondChannelGroupBox = new System.Windows.Forms.GroupBox();
            this.secondChannelNameTextBox = new System.Windows.Forms.TextBox();
            this.secondChannelNameLabel = new System.Windows.Forms.Label();
            this.secondChannelVoltageLevelLabel = new System.Windows.Forms.Label();
            this.secondChannelCurrentLimitLabel = new System.Windows.Forms.Label();
            this.secondChannelMeasurementsGroupBox = new System.Windows.Forms.GroupBox();
            this.secondChaannelVoltageMeasurementTextBox = new System.Windows.Forms.TextBox();
            this.secondChannelCurrentMeasurementTextBox = new System.Windows.Forms.TextBox();
            this.secondChannelCurrentMeasurementLabel = new System.Windows.Forms.Label();
            this.secondChannelVoltageMeasurementLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.firstChannelVoltagLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstChannelCurrentLimitNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondChannelVoltageLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondChannelCurrentLimitNumeric)).BeginInit();
            this.resourceNameGroupBox.SuspendLayout();
            this.firstChannelGroupBox.SuspendLayout();
            this.firstChannelMeasurementsGroupBox.SuspendLayout();
            this.secondChannelGroupBox.SuspendLayout();
            this.secondChannelMeasurementsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // firstChannelVoltageLevelLabel
            // 
            this.firstChannelVoltageLevelLabel.AutoSize = true;
            this.firstChannelVoltageLevelLabel.Location = new System.Drawing.Point(6, 49);
            this.firstChannelVoltageLevelLabel.Name = "firstChannelVoltageLevelLabel";
            this.firstChannelVoltageLevelLabel.Size = new System.Drawing.Size(88, 13);
            this.firstChannelVoltageLevelLabel.TabIndex = 2;
            this.firstChannelVoltageLevelLabel.Text = "Voltage Level (V)";
            // 
            // firstChannelCurrentLimitLabel
            // 
            this.firstChannelCurrentLimitLabel.AutoSize = true;
            this.firstChannelCurrentLimitLabel.Location = new System.Drawing.Point(6, 75);
            this.firstChannelCurrentLimitLabel.Name = "firstChannelCurrentLimitLabel";
            this.firstChannelCurrentLimitLabel.Size = new System.Drawing.Size(81, 13);
            this.firstChannelCurrentLimitLabel.TabIndex = 4;
            this.firstChannelCurrentLimitLabel.Text = "Current Limit (A)";
            // 
            // firstChannelVoltagLevelNumeric
            // 
            this.firstChannelVoltagLevelNumeric.DecimalPlaces = 6;
            this.firstChannelVoltagLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.firstChannelVoltagLevelNumeric.Location = new System.Drawing.Point(111, 45);
            this.firstChannelVoltagLevelNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.firstChannelVoltagLevelNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.firstChannelVoltagLevelNumeric.Name = "firstChannelVoltagLevelNumeric";
            this.firstChannelVoltagLevelNumeric.Size = new System.Drawing.Size(90, 20);
            this.firstChannelVoltagLevelNumeric.TabIndex = 3;
            this.firstChannelVoltagLevelNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // firstChannelCurrentLimitNumeric
            // 
            this.firstChannelCurrentLimitNumeric.DecimalPlaces = 6;
            this.firstChannelCurrentLimitNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.firstChannelCurrentLimitNumeric.Location = new System.Drawing.Point(111, 71);
            this.firstChannelCurrentLimitNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.firstChannelCurrentLimitNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.firstChannelCurrentLimitNumeric.Name = "firstChannelCurrentLimitNumeric";
            this.firstChannelCurrentLimitNumeric.Size = new System.Drawing.Size(90, 20);
            this.firstChannelCurrentLimitNumeric.TabIndex = 5;
            this.firstChannelCurrentLimitNumeric.Value = new decimal(new int[] {
            4,
            0,
            0,
            131072});
            // 
            // secondChannelVoltageLevelNumeric
            // 
            this.secondChannelVoltageLevelNumeric.DecimalPlaces = 6;
            this.secondChannelVoltageLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.secondChannelVoltageLevelNumeric.Location = new System.Drawing.Point(111, 45);
            this.secondChannelVoltageLevelNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.secondChannelVoltageLevelNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.secondChannelVoltageLevelNumeric.Name = "secondChannelVoltageLevelNumeric";
            this.secondChannelVoltageLevelNumeric.Size = new System.Drawing.Size(90, 20);
            this.secondChannelVoltageLevelNumeric.TabIndex = 3;
            this.secondChannelVoltageLevelNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // secondChannelCurrentLimitNumeric
            // 
            this.secondChannelCurrentLimitNumeric.DecimalPlaces = 6;
            this.secondChannelCurrentLimitNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.secondChannelCurrentLimitNumeric.Location = new System.Drawing.Point(111, 71);
            this.secondChannelCurrentLimitNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.secondChannelCurrentLimitNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.secondChannelCurrentLimitNumeric.Name = "secondChannelCurrentLimitNumeric";
            this.secondChannelCurrentLimitNumeric.Size = new System.Drawing.Size(90, 20);
            this.secondChannelCurrentLimitNumeric.TabIndex = 5;
            this.secondChannelCurrentLimitNumeric.Value = new decimal(new int[] {
            4,
            0,
            0,
            131072});
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(301, 323);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // resourceNameGroupBox
            // 
            this.resourceNameGroupBox.Controls.Add(this.resourceNameComboBox);
            this.resourceNameGroupBox.Location = new System.Drawing.Point(12, 12);
            this.resourceNameGroupBox.Name = "resourceNameGroupBox";
            this.resourceNameGroupBox.Size = new System.Drawing.Size(210, 49);
            this.resourceNameGroupBox.TabIndex = 0;
            this.resourceNameGroupBox.TabStop = false;
            this.resourceNameGroupBox.Text = "Resource Name";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(6, 19);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(125, 21);
            this.resourceNameComboBox.TabIndex = 1;
            // 
            // firstChannelNameLabel
            // 
            this.firstChannelNameLabel.AutoSize = true;
            this.firstChannelNameLabel.Location = new System.Drawing.Point(6, 22);
            this.firstChannelNameLabel.Name = "firstChannelNameLabel";
            this.firstChannelNameLabel.Size = new System.Drawing.Size(77, 13);
            this.firstChannelNameLabel.TabIndex = 0;
            this.firstChannelNameLabel.Text = "Channel Name";
            // 
            // firstChannelGroupBox
            // 
            this.firstChannelGroupBox.Controls.Add(this.firstChannelNameTextBox);
            this.firstChannelGroupBox.Controls.Add(this.firstChannelNameLabel);
            this.firstChannelGroupBox.Controls.Add(this.firstChannelVoltagLevelNumeric);
            this.firstChannelGroupBox.Controls.Add(this.firstChannelVoltageLevelLabel);
            this.firstChannelGroupBox.Controls.Add(this.firstChannelCurrentLimitNumeric);
            this.firstChannelGroupBox.Controls.Add(this.firstChannelCurrentLimitLabel);
            this.firstChannelGroupBox.Location = new System.Drawing.Point(12, 77);
            this.firstChannelGroupBox.Name = "firstChannelGroupBox";
            this.firstChannelGroupBox.Size = new System.Drawing.Size(210, 127);
            this.firstChannelGroupBox.TabIndex = 1;
            this.firstChannelGroupBox.TabStop = false;
            this.firstChannelGroupBox.Text = "First Channel";
            // 
            // firstChannelNameTextBox
            // 
            this.firstChannelNameTextBox.Location = new System.Drawing.Point(111, 18);
            this.firstChannelNameTextBox.Name = "firstChannelNameTextBox";
            this.firstChannelNameTextBox.Size = new System.Drawing.Size(90, 20);
            this.firstChannelNameTextBox.TabIndex = 1;
            this.firstChannelNameTextBox.Text = "0";
            // 
            // firstChannelMeasurementsGroupBox
            // 
            this.firstChannelMeasurementsGroupBox.Controls.Add(this.firstChaannelVoltageMeasurementTextBox);
            this.firstChannelMeasurementsGroupBox.Controls.Add(this.firstChannelCurrentMeasurementTextBox);
            this.firstChannelMeasurementsGroupBox.Controls.Add(this.firstChannelCurrentMeasurementLabel);
            this.firstChannelMeasurementsGroupBox.Controls.Add(this.firstChannelVoltageMeasurementLabel);
            this.firstChannelMeasurementsGroupBox.Location = new System.Drawing.Point(238, 77);
            this.firstChannelMeasurementsGroupBox.Name = "firstChannelMeasurementsGroupBox";
            this.firstChannelMeasurementsGroupBox.Size = new System.Drawing.Size(200, 74);
            this.firstChannelMeasurementsGroupBox.TabIndex = 3;
            this.firstChannelMeasurementsGroupBox.TabStop = false;
            this.firstChannelMeasurementsGroupBox.Text = "First Channel Measurements";
            // 
            // firstChaannelVoltageMeasurementTextBox
            // 
            this.firstChaannelVoltageMeasurementTextBox.Location = new System.Drawing.Point(87, 19);
            this.firstChaannelVoltageMeasurementTextBox.Name = "firstChaannelVoltageMeasurementTextBox";
            this.firstChaannelVoltageMeasurementTextBox.ReadOnly = true;
            this.firstChaannelVoltageMeasurementTextBox.Size = new System.Drawing.Size(107, 20);
            this.firstChaannelVoltageMeasurementTextBox.TabIndex = 1;
            this.firstChaannelVoltageMeasurementTextBox.Text = "0.000000";
            // 
            // firstChannelCurrentMeasurementTextBox
            // 
            this.firstChannelCurrentMeasurementTextBox.Location = new System.Drawing.Point(87, 46);
            this.firstChannelCurrentMeasurementTextBox.Name = "firstChannelCurrentMeasurementTextBox";
            this.firstChannelCurrentMeasurementTextBox.ReadOnly = true;
            this.firstChannelCurrentMeasurementTextBox.Size = new System.Drawing.Size(107, 20);
            this.firstChannelCurrentMeasurementTextBox.TabIndex = 3;
            this.firstChannelCurrentMeasurementTextBox.Text = "0.000000";
            // 
            // firstChannelCurrentMeasurementLabel
            // 
            this.firstChannelCurrentMeasurementLabel.AutoSize = true;
            this.firstChannelCurrentMeasurementLabel.Location = new System.Drawing.Point(6, 49);
            this.firstChannelCurrentMeasurementLabel.Name = "firstChannelCurrentMeasurementLabel";
            this.firstChannelCurrentMeasurementLabel.Size = new System.Drawing.Size(57, 13);
            this.firstChannelCurrentMeasurementLabel.TabIndex = 2;
            this.firstChannelCurrentMeasurementLabel.Text = "Current (A)";
            // 
            // firstChannelVoltageMeasurementLabel
            // 
            this.firstChannelVoltageMeasurementLabel.AutoSize = true;
            this.firstChannelVoltageMeasurementLabel.Location = new System.Drawing.Point(6, 23);
            this.firstChannelVoltageMeasurementLabel.Name = "firstChannelVoltageMeasurementLabel";
            this.firstChannelVoltageMeasurementLabel.Size = new System.Drawing.Size(59, 13);
            this.firstChannelVoltageMeasurementLabel.TabIndex = 0;
            this.firstChannelVoltageMeasurementLabel.Text = "Voltage (V)";
            // 
            // secondChannelGroupBox
            // 
            this.secondChannelGroupBox.Controls.Add(this.secondChannelNameTextBox);
            this.secondChannelGroupBox.Controls.Add(this.secondChannelNameLabel);
            this.secondChannelGroupBox.Controls.Add(this.secondChannelVoltageLevelLabel);
            this.secondChannelGroupBox.Controls.Add(this.secondChannelCurrentLimitLabel);
            this.secondChannelGroupBox.Controls.Add(this.secondChannelVoltageLevelNumeric);
            this.secondChannelGroupBox.Controls.Add(this.secondChannelCurrentLimitNumeric);
            this.secondChannelGroupBox.Location = new System.Drawing.Point(12, 227);
            this.secondChannelGroupBox.Name = "secondChannelGroupBox";
            this.secondChannelGroupBox.Size = new System.Drawing.Size(210, 127);
            this.secondChannelGroupBox.TabIndex = 2;
            this.secondChannelGroupBox.TabStop = false;
            this.secondChannelGroupBox.Text = "Second Channel";
            // 
            // secondChannelNameTextBox
            // 
            this.secondChannelNameTextBox.Location = new System.Drawing.Point(111, 18);
            this.secondChannelNameTextBox.Name = "secondChannelNameTextBox";
            this.secondChannelNameTextBox.Size = new System.Drawing.Size(90, 20);
            this.secondChannelNameTextBox.TabIndex = 1;
            this.secondChannelNameTextBox.Text = "1";
            // 
            // secondChannelNameLabel
            // 
            this.secondChannelNameLabel.AutoSize = true;
            this.secondChannelNameLabel.Location = new System.Drawing.Point(6, 22);
            this.secondChannelNameLabel.Name = "secondChannelNameLabel";
            this.secondChannelNameLabel.Size = new System.Drawing.Size(77, 13);
            this.secondChannelNameLabel.TabIndex = 0;
            this.secondChannelNameLabel.Text = "Channel Name";
            // 
            // secondChannelVoltageLevelLabel
            // 
            this.secondChannelVoltageLevelLabel.AutoSize = true;
            this.secondChannelVoltageLevelLabel.Location = new System.Drawing.Point(6, 49);
            this.secondChannelVoltageLevelLabel.Name = "secondChannelVoltageLevelLabel";
            this.secondChannelVoltageLevelLabel.Size = new System.Drawing.Size(88, 13);
            this.secondChannelVoltageLevelLabel.TabIndex = 2;
            this.secondChannelVoltageLevelLabel.Text = "Voltage Level (V)";
            // 
            // secondChannelCurrentLimitLabel
            // 
            this.secondChannelCurrentLimitLabel.AutoSize = true;
            this.secondChannelCurrentLimitLabel.Location = new System.Drawing.Point(6, 75);
            this.secondChannelCurrentLimitLabel.Name = "secondChannelCurrentLimitLabel";
            this.secondChannelCurrentLimitLabel.Size = new System.Drawing.Size(81, 13);
            this.secondChannelCurrentLimitLabel.TabIndex = 4;
            this.secondChannelCurrentLimitLabel.Text = "Current Limit (A)";
            // 
            // secondChannelMeasurementsGroupBox
            // 
            this.secondChannelMeasurementsGroupBox.Controls.Add(this.secondChaannelVoltageMeasurementTextBox);
            this.secondChannelMeasurementsGroupBox.Controls.Add(this.secondChannelCurrentMeasurementTextBox);
            this.secondChannelMeasurementsGroupBox.Controls.Add(this.secondChannelCurrentMeasurementLabel);
            this.secondChannelMeasurementsGroupBox.Controls.Add(this.secondChannelVoltageMeasurementLabel);
            this.secondChannelMeasurementsGroupBox.Location = new System.Drawing.Point(238, 227);
            this.secondChannelMeasurementsGroupBox.Name = "secondChannelMeasurementsGroupBox";
            this.secondChannelMeasurementsGroupBox.Size = new System.Drawing.Size(200, 73);
            this.secondChannelMeasurementsGroupBox.TabIndex = 4;
            this.secondChannelMeasurementsGroupBox.TabStop = false;
            this.secondChannelMeasurementsGroupBox.Text = "Second Channel Measurements";
            // 
            // secondChaannelVoltageMeasurementTextBox
            // 
            this.secondChaannelVoltageMeasurementTextBox.Location = new System.Drawing.Point(87, 19);
            this.secondChaannelVoltageMeasurementTextBox.Name = "secondChaannelVoltageMeasurementTextBox";
            this.secondChaannelVoltageMeasurementTextBox.ReadOnly = true;
            this.secondChaannelVoltageMeasurementTextBox.Size = new System.Drawing.Size(107, 20);
            this.secondChaannelVoltageMeasurementTextBox.TabIndex = 1;
            this.secondChaannelVoltageMeasurementTextBox.Text = "0.000000";
            // 
            // secondChannelCurrentMeasurementTextBox
            // 
            this.secondChannelCurrentMeasurementTextBox.Location = new System.Drawing.Point(87, 45);
            this.secondChannelCurrentMeasurementTextBox.Name = "secondChannelCurrentMeasurementTextBox";
            this.secondChannelCurrentMeasurementTextBox.ReadOnly = true;
            this.secondChannelCurrentMeasurementTextBox.Size = new System.Drawing.Size(107, 20);
            this.secondChannelCurrentMeasurementTextBox.TabIndex = 3;
            this.secondChannelCurrentMeasurementTextBox.Text = "0.000000";
            // 
            // secondChannelCurrentMeasurementLabel
            // 
            this.secondChannelCurrentMeasurementLabel.AutoSize = true;
            this.secondChannelCurrentMeasurementLabel.Location = new System.Drawing.Point(6, 49);
            this.secondChannelCurrentMeasurementLabel.Name = "secondChannelCurrentMeasurementLabel";
            this.secondChannelCurrentMeasurementLabel.Size = new System.Drawing.Size(57, 13);
            this.secondChannelCurrentMeasurementLabel.TabIndex = 2;
            this.secondChannelCurrentMeasurementLabel.Text = "Current (A)";
            // 
            // secondChannelVoltageMeasurementLabel
            // 
            this.secondChannelVoltageMeasurementLabel.AutoSize = true;
            this.secondChannelVoltageMeasurementLabel.Location = new System.Drawing.Point(6, 23);
            this.secondChannelVoltageMeasurementLabel.Name = "secondChannelVoltageMeasurementLabel";
            this.secondChannelVoltageMeasurementLabel.Size = new System.Drawing.Size(59, 13);
            this.secondChannelVoltageMeasurementLabel.TabIndex = 0;
            this.secondChannelVoltageMeasurementLabel.Text = "Voltage (V)";
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 365);
            this.Controls.Add(this.secondChannelMeasurementsGroupBox);
            this.Controls.Add(this.secondChannelGroupBox);
            this.Controls.Add(this.firstChannelMeasurementsGroupBox);
            this.Controls.Add(this.firstChannelGroupBox);
            this.Controls.Add(this.resourceNameGroupBox);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Simultaneous Operation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.firstChannelVoltagLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstChannelCurrentLimitNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondChannelVoltageLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondChannelCurrentLimitNumeric)).EndInit();
            this.resourceNameGroupBox.ResumeLayout(false);
            this.firstChannelGroupBox.ResumeLayout(false);
            this.firstChannelGroupBox.PerformLayout();
            this.firstChannelMeasurementsGroupBox.ResumeLayout(false);
            this.firstChannelMeasurementsGroupBox.PerformLayout();
            this.secondChannelGroupBox.ResumeLayout(false);
            this.secondChannelGroupBox.PerformLayout();
            this.secondChannelMeasurementsGroupBox.ResumeLayout(false);
            this.secondChannelMeasurementsGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label firstChannelVoltageLevelLabel;
        private System.Windows.Forms.Label firstChannelCurrentLimitLabel;
        private System.Windows.Forms.NumericUpDown firstChannelVoltagLevelNumeric;
        private System.Windows.Forms.NumericUpDown firstChannelCurrentLimitNumeric;
        private System.Windows.Forms.NumericUpDown secondChannelVoltageLevelNumeric;
        private System.Windows.Forms.NumericUpDown secondChannelCurrentLimitNumeric;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox resourceNameGroupBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label firstChannelNameLabel;
        private System.Windows.Forms.GroupBox firstChannelGroupBox;
        private System.Windows.Forms.GroupBox firstChannelMeasurementsGroupBox;
        private System.Windows.Forms.TextBox firstChaannelVoltageMeasurementTextBox;
        private System.Windows.Forms.TextBox firstChannelCurrentMeasurementTextBox;
        private System.Windows.Forms.Label firstChannelCurrentMeasurementLabel;
        private System.Windows.Forms.Label firstChannelVoltageMeasurementLabel;
        private System.Windows.Forms.GroupBox secondChannelGroupBox;
        private System.Windows.Forms.Label secondChannelNameLabel;
        private System.Windows.Forms.Label secondChannelVoltageLevelLabel;
        private System.Windows.Forms.Label secondChannelCurrentLimitLabel;
        private System.Windows.Forms.GroupBox secondChannelMeasurementsGroupBox;
        private System.Windows.Forms.TextBox secondChaannelVoltageMeasurementTextBox;
        private System.Windows.Forms.TextBox secondChannelCurrentMeasurementTextBox;
        private System.Windows.Forms.Label secondChannelCurrentMeasurementLabel;
        private System.Windows.Forms.Label secondChannelVoltageMeasurementLabel;
        private System.Windows.Forms.TextBox firstChannelNameTextBox;
        private System.Windows.Forms.TextBox secondChannelNameTextBox;

    }
}
