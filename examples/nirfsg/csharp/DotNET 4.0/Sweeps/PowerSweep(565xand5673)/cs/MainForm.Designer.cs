namespace NationalInstruments.Examples.PowerSweep565xand5673
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
            this.powerSweepGroupBox = new System.Windows.Forms.GroupBox();
            this.startPowerLabel = new System.Windows.Forms.Label();
            this.stopPowerLabel = new System.Windows.Forms.Label();
            this.numberStepsLabel = new System.Windows.Forms.Label();
            this.dwellTimeLabel = new System.Windows.Forms.Label();
            this.startPowerNumeric = new System.Windows.Forms.NumericUpDown();
            this.stopPowerNumeric = new System.Windows.Forms.NumericUpDown();
            this.numberStepsNumeric = new System.Windows.Forms.NumericUpDown();
            this.dwellTimeNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.actualCurrentPowerLabel = new System.Windows.Forms.Label();
            this.clockSourceLabel = new System.Windows.Forms.Label();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.actualCurrentPowerTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();            this.clockSourceComboBox = new System.Windows.Forms.ComboBox();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.powerSweepGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startPowerNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopPowerNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberStepsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwellTimeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // powerSweepGroupBox
            // 
            this.powerSweepGroupBox.Controls.Add(this.startPowerLabel);
            this.powerSweepGroupBox.Controls.Add(this.stopPowerLabel);
            this.powerSweepGroupBox.Controls.Add(this.numberStepsLabel);
            this.powerSweepGroupBox.Controls.Add(this.dwellTimeLabel);
            this.powerSweepGroupBox.Controls.Add(this.startPowerNumeric);
            this.powerSweepGroupBox.Controls.Add(this.stopPowerNumeric);
            this.powerSweepGroupBox.Controls.Add(this.numberStepsNumeric);
            this.powerSweepGroupBox.Controls.Add(this.dwellTimeNumeric);
            this.powerSweepGroupBox.Location = new System.Drawing.Point(171, 12);
            this.powerSweepGroupBox.Name = "powerSweepGroupBox";
            this.powerSweepGroupBox.Size = new System.Drawing.Size(176, 228);
            this.powerSweepGroupBox.TabIndex = 3;
            this.powerSweepGroupBox.TabStop = false;
            this.powerSweepGroupBox.Text = "Power Sweep Parameters";
            // 
            // startPowerLabel
            // 
            this.startPowerLabel.AutoSize = true;
            this.startPowerLabel.Location = new System.Drawing.Point(28, 31);
            this.startPowerLabel.Name = "startPowerLabel";
            this.startPowerLabel.Size = new System.Drawing.Size(92, 13);
            this.startPowerLabel.TabIndex = 2;
            this.startPowerLabel.Text = "Start Power [dBm]";
            // 
            // stopPowerLabel
            // 
            this.stopPowerLabel.AutoSize = true;
            this.stopPowerLabel.Location = new System.Drawing.Point(28, 78);
            this.stopPowerLabel.Name = "stopPowerLabel";
            this.stopPowerLabel.Size = new System.Drawing.Size(92, 13);
            this.stopPowerLabel.TabIndex = 3;
            this.stopPowerLabel.Text = "Stop Power [dBm]";
            // 
            // numberStepsLabel
            // 
            this.numberStepsLabel.AutoSize = true;
            this.numberStepsLabel.Location = new System.Drawing.Point(28, 125);
            this.numberStepsLabel.Name = "numberStepsLabel";
            this.numberStepsLabel.Size = new System.Drawing.Size(86, 13);
            this.numberStepsLabel.TabIndex = 4;
            this.numberStepsLabel.Text = "Number of Steps";
            // 
            // dwellTimeLabel
            // 
            this.dwellTimeLabel.AutoSize = true;
            this.dwellTimeLabel.Location = new System.Drawing.Point(28, 172);
            this.dwellTimeLabel.Name = "dwellTimeLabel";
            this.dwellTimeLabel.Size = new System.Drawing.Size(111, 13);
            this.dwellTimeLabel.TabIndex = 5;
            this.dwellTimeLabel.Text = "Dwell in Each Step [s]";
            // 
            // startPowerNumeric
            // 
            this.startPowerNumeric.DecimalPlaces = 2;
            this.startPowerNumeric.Location = new System.Drawing.Point(28, 52);
            this.startPowerNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.startPowerNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.startPowerNumeric.Name = "startPowerNumeric";
            this.startPowerNumeric.Size = new System.Drawing.Size(120, 20);
            this.startPowerNumeric.TabIndex = 2;
            this.startPowerNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // stopPowerNumeric
            // 
            this.stopPowerNumeric.DecimalPlaces = 2;
            this.stopPowerNumeric.Location = new System.Drawing.Point(28, 99);
            this.stopPowerNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.stopPowerNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.stopPowerNumeric.Name = "stopPowerNumeric";
            this.stopPowerNumeric.Size = new System.Drawing.Size(120, 20);
            this.stopPowerNumeric.TabIndex = 3;
            // 
            // numberStepsNumeric
            // 
            this.numberStepsNumeric.Location = new System.Drawing.Point(28, 146);
            this.numberStepsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numberStepsNumeric.Name = "numberStepsNumeric";
            this.numberStepsNumeric.Size = new System.Drawing.Size(120, 20);
            this.numberStepsNumeric.TabIndex = 4;
            this.numberStepsNumeric.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            // 
            // dwellTimeNumeric
            // 
            this.dwellTimeNumeric.DecimalPlaces = 2;
            this.dwellTimeNumeric.Location = new System.Drawing.Point(28, 193);
            this.dwellTimeNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.dwellTimeNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.dwellTimeNumeric.Name = "dwellTimeNumeric";
            this.dwellTimeNumeric.Size = new System.Drawing.Size(120, 20);
            this.dwellTimeNumeric.TabIndex = 5;
            this.dwellTimeNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(19, 42);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.AutoSize = true;
            this.frequencyLabel.Location = new System.Drawing.Point(19, 90);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.frequencyLabel.TabIndex = 1;
            this.frequencyLabel.Text = "Center Frequency [Hz]";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(19, 252);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 6;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // actualCurrentPowerLabel
            // 
            this.actualCurrentPowerLabel.AutoSize = true;
            this.actualCurrentPowerLabel.Location = new System.Drawing.Point(382, 90);
            this.actualCurrentPowerLabel.Name = "actualCurrentPowerLabel";
            this.actualCurrentPowerLabel.Size = new System.Drawing.Size(104, 13);
            this.actualCurrentPowerLabel.TabIndex = 7;
            this.actualCurrentPowerLabel.Text = "Current Power [dBm]";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.AutoSize = true;
            this.clockSourceLabel.Location = new System.Drawing.Point(19, 137);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.Size = new System.Drawing.Size(147, 13);
            this.clockSourceLabel.TabIndex = 9;
            this.clockSourceLabel.Text = "Frequency Reference Source";
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.DecimalPlaces = 6;
            this.frequencyNumeric.Location = new System.Drawing.Point(19, 111);
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
            this.frequencyNumeric.TabIndex = 1;
            this.frequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(19, 63);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(19, 273);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(485, 34);
            this.errorTextBox.TabIndex = 11;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // actualCurrentPowerTextBox
            // 
            this.actualCurrentPowerTextBox.Location = new System.Drawing.Point(382, 111);
            this.actualCurrentPowerTextBox.Name = "actualCurrentPowerTextBox";
            this.actualCurrentPowerTextBox.ReadOnly = true;
            this.actualCurrentPowerTextBox.Size = new System.Drawing.Size(101, 20);
            this.actualCurrentPowerTextBox.TabIndex = 12;
            this.actualCurrentPowerTextBox.TabStop = false;
            this.actualCurrentPowerTextBox.Text = "0.00";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(358, 20);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(435, 20);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 5;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            //             // clockSourceComboBox
            // 
            this.clockSourceComboBox.Location = new System.Drawing.Point(19, 158);
            this.clockSourceComboBox.Name = "clockSourceComboBox";
            this.clockSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.clockSourceComboBox.TabIndex = 2;
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Interval = 250;
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;            this.ClientSize = new System.Drawing.Size(522, 360);
            this.Controls.Add(this.powerSweepGroupBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.frequencyLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.actualCurrentPowerLabel);
            this.Controls.Add(this.clockSourceLabel);
            this.Controls.Add(this.frequencyNumeric);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.actualCurrentPowerTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);            this.Controls.Add(this.clockSourceComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Power Sweep (565x and 5673)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.powerSweepGroupBox.ResumeLayout(false);
            this.powerSweepGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startPowerNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopPowerNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberStepsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwellTimeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.GroupBox powerSweepGroupBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.Label startPowerLabel;
        private System.Windows.Forms.Label stopPowerLabel;
        private System.Windows.Forms.Label numberStepsLabel;
        private System.Windows.Forms.Label dwellTimeLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label actualCurrentPowerLabel;
        private System.Windows.Forms.Label clockSourceLabel;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.NumericUpDown startPowerNumeric;
        private System.Windows.Forms.NumericUpDown stopPowerNumeric;
        private System.Windows.Forms.NumericUpDown numberStepsNumeric;
        private System.Windows.Forms.NumericUpDown dwellTimeNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.TextBox actualCurrentPowerTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;        private System.Windows.Forms.ComboBox clockSourceComboBox;
        private System.Windows.Forms.Timer rfsgStatusTimer;

    }
}
