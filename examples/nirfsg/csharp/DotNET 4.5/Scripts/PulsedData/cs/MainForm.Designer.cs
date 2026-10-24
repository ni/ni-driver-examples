namespace NationalInstruments.Examples.PulsedData
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.actualTimeSlotPeriodLabel = new System.Windows.Forms.Label();
            this.actualFramePeriodLabel = new System.Windows.Forms.Label();
            this.framePeriodActualLabel = new System.Windows.Forms.Label();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.timeSlotsLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.iqRateLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.actualIQRateLabel = new System.Windows.Forms.Label();
            this.framePeriodNumeric = new System.Windows.Forms.NumericUpDown();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.timeSlotsNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.actualTimeSlotPeriodTextBox = new System.Windows.Forms.TextBox();
            this.actualFramePeriodTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.actualIQRateTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.framePeriodNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeSlotsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            this.measurementGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(18, 16);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // actualTimeSlotPeriodLabel
            // 
            this.actualTimeSlotPeriodLabel.AutoSize = true;
            this.actualTimeSlotPeriodLabel.Location = new System.Drawing.Point(24, 84);
            this.actualTimeSlotPeriodLabel.Name = "actualTimeSlotPeriodLabel";
            this.actualTimeSlotPeriodLabel.Size = new System.Drawing.Size(131, 13);
            this.actualTimeSlotPeriodLabel.TabIndex = 1;
            this.actualTimeSlotPeriodLabel.Text = "Actual Time Slot Period [s]";
            // 
            // actualFramePeriodLabel
            // 
            this.actualFramePeriodLabel.AutoSize = true;
            this.actualFramePeriodLabel.Location = new System.Drawing.Point(175, 25);
            this.actualFramePeriodLabel.Name = "actualFramePeriodLabel";
            this.actualFramePeriodLabel.Size = new System.Drawing.Size(83, 13);
            this.actualFramePeriodLabel.TabIndex = 2;
            this.actualFramePeriodLabel.Text = "Frame Period [s]";
            // 
            // framePeriodActualLabel
            // 
            this.framePeriodActualLabel.AutoSize = true;
            this.framePeriodActualLabel.Location = new System.Drawing.Point(24, 31);
            this.framePeriodActualLabel.Name = "framePeriodActualLabel";
            this.framePeriodActualLabel.Size = new System.Drawing.Size(116, 13);
            this.framePeriodActualLabel.TabIndex = 3;
            this.framePeriodActualLabel.Text = "Actual Frame Period [s]";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.AutoSize = true;
            this.frequencyLabel.Location = new System.Drawing.Point(21, 26);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.frequencyLabel.TabIndex = 4;
            this.frequencyLabel.Text = "Center Frequency [Hz]";
            // 
            // timeSlotsLabel
            // 
            this.timeSlotsLabel.AutoSize = true;
            this.timeSlotsLabel.Location = new System.Drawing.Point(175, 78);
            this.timeSlotsLabel.Name = "timeSlotsLabel";
            this.timeSlotsLabel.Size = new System.Drawing.Size(56, 13);
            this.timeSlotsLabel.TabIndex = 5;
            this.timeSlotsLabel.Text = "Time Slots";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(21, 79);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 6;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // iqRateLabel
            // 
            this.iqRateLabel.AutoSize = true;
            this.iqRateLabel.Location = new System.Drawing.Point(21, 134);
            this.iqRateLabel.Name = "iqRateLabel";
            this.iqRateLabel.Size = new System.Drawing.Size(70, 13);
            this.iqRateLabel.TabIndex = 7;
            this.iqRateLabel.Text = "IQ Rate [S/s]";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(18, 304);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 8;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // actualIQRateLabel
            // 
            this.actualIQRateLabel.AutoSize = true;
            this.actualIQRateLabel.Location = new System.Drawing.Point(24, 137);
            this.actualIQRateLabel.Name = "actualIQRateLabel";
            this.actualIQRateLabel.Size = new System.Drawing.Size(103, 13);
            this.actualIQRateLabel.TabIndex = 10;
            this.actualIQRateLabel.Text = "Actual IQ Rate [S/s]";
            // 
            // framePeriodNumeric
            // 
            this.framePeriodNumeric.DecimalPlaces = 5;
            this.framePeriodNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.framePeriodNumeric.Location = new System.Drawing.Point(175, 46);
            this.framePeriodNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.framePeriodNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.framePeriodNumeric.Name = "framePeriodNumeric";
            this.framePeriodNumeric.Size = new System.Drawing.Size(120, 20);
            this.framePeriodNumeric.TabIndex = 3;
            this.framePeriodNumeric.Value = new decimal(new int[] {
            461536,
            0,
            0,
            524288});
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.DecimalPlaces = 6;
            this.frequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.frequencyNumeric.Location = new System.Drawing.Point(21, 47);
            this.frequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.frequencyNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.frequencyNumeric.Name = "frequencyNumeric";
            this.frequencyNumeric.Size = new System.Drawing.Size(120, 20);
            this.frequencyNumeric.TabIndex = 0;
            this.frequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // timeSlotsNumeric
            // 
            this.timeSlotsNumeric.Location = new System.Drawing.Point(175, 99);
            this.timeSlotsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.timeSlotsNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.timeSlotsNumeric.Name = "timeSlotsNumeric";
            this.timeSlotsNumeric.Size = new System.Drawing.Size(120, 20);
            this.timeSlotsNumeric.TabIndex = 4;
            this.timeSlotsNumeric.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.DecimalPlaces = 2;
            this.powerLevelNumeric.Location = new System.Drawing.Point(21, 100);
            this.powerLevelNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.powerLevelNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.powerLevelNumeric.Name = "powerLevelNumeric";
            this.powerLevelNumeric.Size = new System.Drawing.Size(120, 20);
            this.powerLevelNumeric.TabIndex = 1;
            this.powerLevelNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // iqRateNumeric
            // 
            this.iqRateNumeric.DecimalPlaces = 6;
            this.iqRateNumeric.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.iqRateNumeric.Location = new System.Drawing.Point(21, 155);
            this.iqRateNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.iqRateNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.iqRateNumeric.Name = "iqRateNumeric";
            this.iqRateNumeric.Size = new System.Drawing.Size(120, 20);
            this.iqRateNumeric.TabIndex = 5;
            this.iqRateNumeric.Value = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(18, 37);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // actualTimeSlotPeriodTextBox
            // 
            this.actualTimeSlotPeriodTextBox.Location = new System.Drawing.Point(24, 105);
            this.actualTimeSlotPeriodTextBox.Name = "actualTimeSlotPeriodTextBox";
            this.actualTimeSlotPeriodTextBox.ReadOnly = true;
            this.actualTimeSlotPeriodTextBox.Size = new System.Drawing.Size(109, 20);
            this.actualTimeSlotPeriodTextBox.TabIndex = 1;
            this.actualTimeSlotPeriodTextBox.TabStop = false;
            this.actualTimeSlotPeriodTextBox.Text = "0.00000";
            // 
            // actualFramePeriodTextBox
            // 
            this.actualFramePeriodTextBox.Location = new System.Drawing.Point(24, 52);
            this.actualFramePeriodTextBox.Name = "actualFramePeriodTextBox";
            this.actualFramePeriodTextBox.ReadOnly = true;
            this.actualFramePeriodTextBox.Size = new System.Drawing.Size(109, 20);
            this.actualFramePeriodTextBox.TabIndex = 2;
            this.actualFramePeriodTextBox.TabStop = false;
            this.actualFramePeriodTextBox.Text = "0.00000";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(19, 320);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(499, 34);
            this.errorTextBox.TabIndex = 12;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // actualIQRateTextBox
            // 
            this.actualIQRateTextBox.Location = new System.Drawing.Point(24, 158);
            this.actualIQRateTextBox.Name = "actualIQRateTextBox";
            this.actualIQRateTextBox.ReadOnly = true;
            this.actualIQRateTextBox.Size = new System.Drawing.Size(109, 20);
            this.actualIQRateTextBox.TabIndex = 14;
            this.actualIQRateTextBox.TabStop = false;
            this.actualIQRateTextBox.Text = "0.000000";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(365, 37);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(443, 37);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.iqRateNumeric);
            this.configurationGroupBox.Controls.Add(this.powerLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.actualFramePeriodLabel);
            this.configurationGroupBox.Controls.Add(this.timeSlotsNumeric);
            this.configurationGroupBox.Controls.Add(this.frequencyLabel);
            this.configurationGroupBox.Controls.Add(this.frequencyNumeric);
            this.configurationGroupBox.Controls.Add(this.timeSlotsLabel);
            this.configurationGroupBox.Controls.Add(this.framePeriodNumeric);
            this.configurationGroupBox.Controls.Add(this.powerLevelLabel);
            this.configurationGroupBox.Controls.Add(this.iqRateLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(18, 79);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(317, 204);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualFramePeriodTextBox);
            this.measurementGroupBox.Controls.Add(this.actualIQRateTextBox);
            this.measurementGroupBox.Controls.Add(this.actualTimeSlotPeriodTextBox);
            this.measurementGroupBox.Controls.Add(this.actualTimeSlotPeriodLabel);
            this.measurementGroupBox.Controls.Add(this.actualIQRateLabel);
            this.measurementGroupBox.Controls.Add(this.framePeriodActualLabel);
            this.measurementGroupBox.Location = new System.Drawing.Point(352, 76);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(166, 207);
            this.measurementGroupBox.TabIndex = 0;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(540, 400);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Pulsed Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.framePeriodNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeSlotsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqRateNumeric)).EndInit();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
        #endregion

        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label actualTimeSlotPeriodLabel;
        private System.Windows.Forms.Label actualFramePeriodLabel;
        private System.Windows.Forms.Label framePeriodActualLabel;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.Label timeSlotsLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label iqRateLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label actualIQRateLabel;
        private System.Windows.Forms.NumericUpDown framePeriodNumeric;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.NumericUpDown timeSlotsNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown iqRateNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox actualTimeSlotPeriodTextBox;
        private System.Windows.Forms.TextBox actualFramePeriodTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.TextBox actualIQRateTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;

    }
}
