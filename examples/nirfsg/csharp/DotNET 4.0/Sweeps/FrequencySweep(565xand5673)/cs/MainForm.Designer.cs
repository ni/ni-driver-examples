namespace NationalInstruments.Examples.FrequencySweep565xand5673
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
            this.frequencySweepGroupBox = new System.Windows.Forms.GroupBox();
            this.startFrequencyLabel = new System.Windows.Forms.Label();
            this.stopFrequencyLabel = new System.Windows.Forms.Label();
            this.numberStepsLabel = new System.Windows.Forms.Label();
            this.dwellTimeLabel = new System.Windows.Forms.Label();
            this.startFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.stopFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.numberStepsNumeric = new System.Windows.Forms.NumericUpDown();
            this.dwellTimeNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.actualCurrentFrequencyLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.clockSourceLabel = new System.Windows.Forms.Label();
            this.loopBandwidthLabel = new System.Windows.Forms.Label();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.actualCurrentFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();            this.clockSourceComboBox = new System.Windows.Forms.ComboBox();
            this.loopBandwidthComboBox = new System.Windows.Forms.ComboBox();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.frequencySweepGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberStepsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwellTimeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // frequencySweepGroupBox
            // 
            this.frequencySweepGroupBox.Controls.Add(this.startFrequencyLabel);
            this.frequencySweepGroupBox.Controls.Add(this.stopFrequencyLabel);
            this.frequencySweepGroupBox.Controls.Add(this.numberStepsLabel);
            this.frequencySweepGroupBox.Controls.Add(this.dwellTimeLabel);
            this.frequencySweepGroupBox.Controls.Add(this.startFrequencyNumeric);
            this.frequencySweepGroupBox.Controls.Add(this.stopFrequencyNumeric);
            this.frequencySweepGroupBox.Controls.Add(this.numberStepsNumeric);
            this.frequencySweepGroupBox.Controls.Add(this.dwellTimeNumeric);
            this.frequencySweepGroupBox.Location = new System.Drawing.Point(225, 17);
            this.frequencySweepGroupBox.Name = "frequencySweepGroupBox";
            this.frequencySweepGroupBox.Size = new System.Drawing.Size(177, 236);
            this.frequencySweepGroupBox.TabIndex = 2;
            this.frequencySweepGroupBox.TabStop = false;
            this.frequencySweepGroupBox.Text = "Frequency Sweep Parameters";
            // 
            // startFrequencyLabel
            // 
            this.startFrequencyLabel.AutoSize = true;
            this.startFrequencyLabel.Location = new System.Drawing.Point(26, 27);
            this.startFrequencyLabel.Name = "startFrequencyLabel";
            this.startFrequencyLabel.Size = new System.Drawing.Size(104, 13);
            this.startFrequencyLabel.TabIndex = 2;
            this.startFrequencyLabel.Text = "Start Frequency [Hz]";
            // 
            // stopFrequencyLabel
            // 
            this.stopFrequencyLabel.AutoSize = true;
            this.stopFrequencyLabel.Location = new System.Drawing.Point(27, 82);
            this.stopFrequencyLabel.Name = "stopFrequencyLabel";
            this.stopFrequencyLabel.Size = new System.Drawing.Size(101, 13);
            this.stopFrequencyLabel.TabIndex = 3;
            this.stopFrequencyLabel.Text = "End Frequency [Hz]";
            // 
            // numberStepsLabel
            // 
            this.numberStepsLabel.AutoSize = true;
            this.numberStepsLabel.Location = new System.Drawing.Point(27, 134);
            this.numberStepsLabel.Name = "numberStepsLabel";
            this.numberStepsLabel.Size = new System.Drawing.Size(86, 13);
            this.numberStepsLabel.TabIndex = 4;
            this.numberStepsLabel.Text = "Number of Steps";
            // 
            // dwellTimeLabel
            // 
            this.dwellTimeLabel.AutoSize = true;
            this.dwellTimeLabel.Location = new System.Drawing.Point(27, 184);
            this.dwellTimeLabel.Name = "dwellTimeLabel";
            this.dwellTimeLabel.Size = new System.Drawing.Size(137, 13);
            this.dwellTimeLabel.TabIndex = 5;
            this.dwellTimeLabel.Text = "Dwell Time in Each Step [s]";
            // 
            // startFrequencyNumeric
            // 
            this.startFrequencyNumeric.DecimalPlaces = 6;
            this.startFrequencyNumeric.Location = new System.Drawing.Point(23, 47);
            this.startFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.startFrequencyNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.startFrequencyNumeric.Name = "startFrequencyNumeric";
            this.startFrequencyNumeric.Size = new System.Drawing.Size(120, 20);
            this.startFrequencyNumeric.TabIndex = 2;
            this.startFrequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // stopFrequencyNumeric
            // 
            this.stopFrequencyNumeric.DecimalPlaces = 6;
            this.stopFrequencyNumeric.Location = new System.Drawing.Point(23, 103);
            this.stopFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.stopFrequencyNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.stopFrequencyNumeric.Name = "stopFrequencyNumeric";
            this.stopFrequencyNumeric.Size = new System.Drawing.Size(120, 20);
            this.stopFrequencyNumeric.TabIndex = 3;
            this.stopFrequencyNumeric.Value = new decimal(new int[] {
            1020000000,
            0,
            0,
            0});
            // 
            // numberStepsNumeric
            // 
            this.numberStepsNumeric.Location = new System.Drawing.Point(23, 153);
            this.numberStepsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numberStepsNumeric.Minimum = new decimal(new int[] {
            1,
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
            this.dwellTimeNumeric.DecimalPlaces = 3;
            this.dwellTimeNumeric.Location = new System.Drawing.Point(23, 203);
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
            this.resourceNameLabel.Location = new System.Drawing.Point(22, 16);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(22, 27);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 1;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // actualCurrentFrequencyLabel
            // 
            this.actualCurrentFrequencyLabel.AutoSize = true;
            this.actualCurrentFrequencyLabel.Location = new System.Drawing.Point(442, 98);
            this.actualCurrentFrequencyLabel.Name = "actualCurrentFrequencyLabel";
            this.actualCurrentFrequencyLabel.Size = new System.Drawing.Size(116, 13);
            this.actualCurrentFrequencyLabel.TabIndex = 6;
            this.actualCurrentFrequencyLabel.Text = "Current Frequency [Hz]";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(22, 265);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 7;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.AutoSize = true;
            this.clockSourceLabel.Location = new System.Drawing.Point(21, 77);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.Size = new System.Drawing.Size(147, 13);
            this.clockSourceLabel.TabIndex = 9;
            this.clockSourceLabel.Text = "Frequency Reference Source";
            // 
            // loopBandwidthLabel
            // 
            this.loopBandwidthLabel.AutoSize = true;
            this.loopBandwidthLabel.Location = new System.Drawing.Point(20, 127);
            this.loopBandwidthLabel.Name = "loopBandwidthLabel";
            this.loopBandwidthLabel.Size = new System.Drawing.Size(84, 13);
            this.loopBandwidthLabel.TabIndex = 10;
            this.loopBandwidthLabel.Text = "Loop Bandwidth";
            // 
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.DecimalPlaces = 2;
            this.powerLevelNumeric.Location = new System.Drawing.Point(22, 48);
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
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(22, 37);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // actualCurrentFrequencyTextBox
            // 
            this.actualCurrentFrequencyTextBox.Location = new System.Drawing.Point(442, 119);
            this.actualCurrentFrequencyTextBox.Name = "actualCurrentFrequencyTextBox";
            this.actualCurrentFrequencyTextBox.ReadOnly = true;
            this.actualCurrentFrequencyTextBox.Size = new System.Drawing.Size(103, 20);
            this.actualCurrentFrequencyTextBox.TabIndex = 11;
            this.actualCurrentFrequencyTextBox.TabStop = false;
            this.actualCurrentFrequencyTextBox.Text = "0.0000";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(22, 286);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(559, 34);
            this.errorTextBox.TabIndex = 12;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(425, 25);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(506, 25);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            //             // clockSourceComboBox
            // 
            this.clockSourceComboBox.Location = new System.Drawing.Point(21, 98);
            this.clockSourceComboBox.Name = "clockSourceComboBox";
            this.clockSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.clockSourceComboBox.TabIndex = 15;
            // 
            // loopBandwidthComboBox
            // 
            this.loopBandwidthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loopBandwidthComboBox.Location = new System.Drawing.Point(20, 148);
            this.loopBandwidthComboBox.Name = "loopBandwidthComboBox";
            this.loopBandwidthComboBox.Size = new System.Drawing.Size(120, 21);
            this.loopBandwidthComboBox.TabIndex = 16;
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.loopBandwidthComboBox);
            this.configurationGroupBox.Controls.Add(this.clockSourceComboBox);
            this.configurationGroupBox.Controls.Add(this.powerLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.powerLevelLabel);
            this.configurationGroupBox.Controls.Add(this.loopBandwidthLabel);
            this.configurationGroupBox.Controls.Add(this.clockSourceLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(22, 72);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(182, 181);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;            this.ClientSize = new System.Drawing.Size(600, 367);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.frequencySweepGroupBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.actualCurrentFrequencyLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.actualCurrentFrequencyTextBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Frequency Sweep (565x and 5673)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.frequencySweepGroupBox.ResumeLayout(false);
            this.frequencySweepGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberStepsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwellTimeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.GroupBox frequencySweepGroupBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label startFrequencyLabel;
        private System.Windows.Forms.Label stopFrequencyLabel;
        private System.Windows.Forms.Label numberStepsLabel;
        private System.Windows.Forms.Label dwellTimeLabel;
        private System.Windows.Forms.Label actualCurrentFrequencyLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label clockSourceLabel;
        private System.Windows.Forms.Label loopBandwidthLabel;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown startFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown stopFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown numberStepsNumeric;
        private System.Windows.Forms.NumericUpDown dwellTimeNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox actualCurrentFrequencyTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;        private System.Windows.Forms.ComboBox clockSourceComboBox;
        private System.Windows.Forms.ComboBox loopBandwidthComboBox;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.GroupBox configurationGroupBox;

    }
}
