namespace NationalInstruments.Examples.ForwardFrequencySweep5673InbandRetuning
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
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.clockSourceLabel = new System.Windows.Forms.Label();
            this.dwellTimeLabel = new System.Windows.Forms.Label();
            this.numberStepsLabel = new System.Windows.Forms.Label();
            this.deviceBandwidthToUseLabel = new System.Windows.Forms.Label();
            this.loopBandwidthLabel = new System.Windows.Forms.Label();
            this.stopFrequencyLabel = new System.Windows.Forms.Label();
            this.actualCurrentFrequencyLabel = new System.Windows.Forms.Label();
            this.startFrequencyLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.dwellTimeNumeric = new System.Windows.Forms.NumericUpDown();
            this.numberStepsNumeric = new System.Windows.Forms.NumericUpDown();
            this.deviceBandwidthToUseNumeric = new System.Windows.Forms.NumericUpDown();
            this.stopFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.startFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.actualCurrentFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();            this.clockSourceComboBox = new System.Windows.Forms.ComboBox();
            this.loopBandwidthComboBox = new System.Windows.Forms.ComboBox();
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.configurationListParametersGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwellTimeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberStepsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceBandwidthToUseNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).BeginInit();
            this.configurationGroupBox.SuspendLayout();
            this.configurationListParametersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(22, 26);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 0;
            this.powerLevelLabel.Text = "Power Level (dBm)";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.AutoSize = true;
            this.clockSourceLabel.Location = new System.Drawing.Point(22, 76);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.Size = new System.Drawing.Size(147, 13);
            this.clockSourceLabel.TabIndex = 1;
            this.clockSourceLabel.Text = "Frequency Reference Source";
            // 
            // dwellTimeLabel
            // 
            this.dwellTimeLabel.AutoSize = true;
            this.dwellTimeLabel.Location = new System.Drawing.Point(18, 177);
            this.dwellTimeLabel.Name = "dwellTimeLabel";
            this.dwellTimeLabel.Size = new System.Drawing.Size(117, 13);
            this.dwellTimeLabel.TabIndex = 2;
            this.dwellTimeLabel.Text = "Dwell Time Per Step (s)";
            // 
            // numberStepsLabel
            // 
            this.numberStepsLabel.AutoSize = true;
            this.numberStepsLabel.Location = new System.Drawing.Point(18, 127);
            this.numberStepsLabel.Name = "numberStepsLabel";
            this.numberStepsLabel.Size = new System.Drawing.Size(86, 13);
            this.numberStepsLabel.TabIndex = 3;
            this.numberStepsLabel.Text = "Number of Steps";
            // 
            // deviceBandwidthToUseLabel
            // 
            this.deviceBandwidthToUseLabel.AutoSize = true;
            this.deviceBandwidthToUseLabel.Location = new System.Drawing.Point(22, 126);
            this.deviceBandwidthToUseLabel.Name = "deviceBandwidthToUseLabel";
            this.deviceBandwidthToUseLabel.Size = new System.Drawing.Size(150, 13);
            this.deviceBandwidthToUseLabel.TabIndex = 4;
            this.deviceBandwidthToUseLabel.Text = "Device Bandwidth to Use (Hz)";
            // 
            // loopBandwidthLabel
            // 
            this.loopBandwidthLabel.AutoSize = true;
            this.loopBandwidthLabel.Location = new System.Drawing.Point(22, 176);
            this.loopBandwidthLabel.Name = "loopBandwidthLabel";
            this.loopBandwidthLabel.Size = new System.Drawing.Size(84, 13);
            this.loopBandwidthLabel.TabIndex = 5;
            this.loopBandwidthLabel.Text = "Loop Bandwidth";
            // 
            // stopFrequencyLabel
            // 
            this.stopFrequencyLabel.AutoSize = true;
            this.stopFrequencyLabel.Location = new System.Drawing.Point(18, 77);
            this.stopFrequencyLabel.Name = "stopFrequencyLabel";
            this.stopFrequencyLabel.Size = new System.Drawing.Size(104, 13);
            this.stopFrequencyLabel.TabIndex = 6;
            this.stopFrequencyLabel.Text = "Stop Frequency (Hz)";
            // 
            // actualCurrentFrequencyLabel
            // 
            this.actualCurrentFrequencyLabel.AutoSize = true;
            this.actualCurrentFrequencyLabel.Location = new System.Drawing.Point(429, 101);
            this.actualCurrentFrequencyLabel.Name = "actualCurrentFrequencyLabel";
            this.actualCurrentFrequencyLabel.Size = new System.Drawing.Size(116, 13);
            this.actualCurrentFrequencyLabel.TabIndex = 7;
            this.actualCurrentFrequencyLabel.Text = "Current Frequency (Hz)";
            // 
            // startFrequencyLabel
            // 
            this.startFrequencyLabel.AutoSize = true;
            this.startFrequencyLabel.Location = new System.Drawing.Point(18, 27);
            this.startFrequencyLabel.Name = "startFrequencyLabel";
            this.startFrequencyLabel.Size = new System.Drawing.Size(104, 13);
            this.startFrequencyLabel.TabIndex = 8;
            this.startFrequencyLabel.Text = "Start Frequency (Hz)";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(13, 319);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 9;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(19, 20);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 11;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.DecimalPlaces = 2;
            this.powerLevelNumeric.Location = new System.Drawing.Point(20, 44);
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
            this.powerLevelNumeric.TabIndex = 0;
            this.powerLevelNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // dwellTimeNumeric
            // 
            this.dwellTimeNumeric.DecimalPlaces = 6;
            this.dwellTimeNumeric.Location = new System.Drawing.Point(21, 195);
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
            this.dwellTimeNumeric.TabIndex = 3;
            this.dwellTimeNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // numberStepsNumeric
            // 
            this.numberStepsNumeric.Location = new System.Drawing.Point(21, 145);
            this.numberStepsNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numberStepsNumeric.Name = "numberStepsNumeric";
            this.numberStepsNumeric.Size = new System.Drawing.Size(120, 20);
            this.numberStepsNumeric.TabIndex = 2;
            this.numberStepsNumeric.Value = new decimal(new int[] {
            501,
            0,
            0,
            0});
            // 
            // deviceBandwidthToUseNumeric
            // 
            this.deviceBandwidthToUseNumeric.Location = new System.Drawing.Point(20, 144);
            this.deviceBandwidthToUseNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.deviceBandwidthToUseNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.deviceBandwidthToUseNumeric.Name = "deviceBandwidthToUseNumeric";
            this.deviceBandwidthToUseNumeric.Size = new System.Drawing.Size(120, 20);
            this.deviceBandwidthToUseNumeric.TabIndex = 2;
            this.deviceBandwidthToUseNumeric.Value = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            // 
            // stopFrequencyNumeric
            // 
            this.stopFrequencyNumeric.DecimalPlaces = 6;
            this.stopFrequencyNumeric.Location = new System.Drawing.Point(21, 95);
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
            this.stopFrequencyNumeric.TabIndex = 1;
            this.stopFrequencyNumeric.Value = new decimal(new int[] {
            2000000000,
            0,
            0,
            0});
            // 
            // startFrequencyNumeric
            // 
            this.startFrequencyNumeric.DecimalPlaces = 6;
            this.startFrequencyNumeric.Location = new System.Drawing.Point(21, 45);
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
            this.startFrequencyNumeric.TabIndex = 0;
            this.startFrequencyNumeric.Value = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            // 
            // actualCurrentFrequencyTextBox
            // 
            this.actualCurrentFrequencyTextBox.Location = new System.Drawing.Point(429, 119);
            this.actualCurrentFrequencyTextBox.Name = "actualCurrentFrequencyTextBox";
            this.actualCurrentFrequencyTextBox.ReadOnly = true;
            this.actualCurrentFrequencyTextBox.Size = new System.Drawing.Size(108, 20);
            this.actualCurrentFrequencyTextBox.TabIndex = 7;
            this.actualCurrentFrequencyTextBox.TabStop = false;
            this.actualCurrentFrequencyTextBox.Text = "0.0000";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(12, 337);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(525, 34);
            this.errorTextBox.TabIndex = 11;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(16, 37);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(462, 35);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(381, 35);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            //             // clockSourceComboBox
            // 
            this.clockSourceComboBox.Location = new System.Drawing.Point(20, 94);
            this.clockSourceComboBox.Name = "clockSourceComboBox";
            this.clockSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.clockSourceComboBox.TabIndex = 1;
            // 
            // loopBandwidthComboBox
            // 
            this.loopBandwidthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loopBandwidthComboBox.Location = new System.Drawing.Point(20, 194);
            this.loopBandwidthComboBox.Name = "loopBandwidthComboBox";
            this.loopBandwidthComboBox.Size = new System.Drawing.Size(120, 21);
            this.loopBandwidthComboBox.TabIndex = 3;
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Interval = 1000;
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.loopBandwidthComboBox);
            this.configurationGroupBox.Controls.Add(this.clockSourceComboBox);
            this.configurationGroupBox.Controls.Add(this.powerLevelLabel);
            this.configurationGroupBox.Controls.Add(this.deviceBandwidthToUseNumeric);
            this.configurationGroupBox.Controls.Add(this.clockSourceLabel);
            this.configurationGroupBox.Controls.Add(this.powerLevelNumeric);
            this.configurationGroupBox.Controls.Add(this.deviceBandwidthToUseLabel);
            this.configurationGroupBox.Controls.Add(this.loopBandwidthLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(16, 74);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(189, 224);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // configurationListParametersGroupBox
            // 
            this.configurationListParametersGroupBox.Controls.Add(this.dwellTimeNumeric);
            this.configurationListParametersGroupBox.Controls.Add(this.startFrequencyNumeric);
            this.configurationListParametersGroupBox.Controls.Add(this.stopFrequencyNumeric);
            this.configurationListParametersGroupBox.Controls.Add(this.numberStepsNumeric);
            this.configurationListParametersGroupBox.Controls.Add(this.dwellTimeLabel);
            this.configurationListParametersGroupBox.Controls.Add(this.startFrequencyLabel);
            this.configurationListParametersGroupBox.Controls.Add(this.numberStepsLabel);
            this.configurationListParametersGroupBox.Controls.Add(this.stopFrequencyLabel);
            this.configurationListParametersGroupBox.Location = new System.Drawing.Point(229, 74);
            this.configurationListParametersGroupBox.Name = "configurationListParametersGroupBox";
            this.configurationListParametersGroupBox.Size = new System.Drawing.Size(168, 224);
            this.configurationListParametersGroupBox.TabIndex = 2;
            this.configurationListParametersGroupBox.TabStop = false;
            this.configurationListParametersGroupBox.Text = "Configuration List Parameters";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;            this.ClientSize = new System.Drawing.Size(559, 411);
            this.Controls.Add(this.configurationListParametersGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.actualCurrentFrequencyLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.actualCurrentFrequencyTextBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Forward Frequency Sweep (5673 In-band Retuning)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwellTimeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberStepsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviceBandwidthToUseNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).EndInit();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.configurationListParametersGroupBox.ResumeLayout(false);
            this.configurationListParametersGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label clockSourceLabel;
        private System.Windows.Forms.Label dwellTimeLabel;
        private System.Windows.Forms.Label numberStepsLabel;
        private System.Windows.Forms.Label deviceBandwidthToUseLabel;
        private System.Windows.Forms.Label loopBandwidthLabel;
        private System.Windows.Forms.Label stopFrequencyLabel;
        private System.Windows.Forms.Label actualCurrentFrequencyLabel;
        private System.Windows.Forms.Label startFrequencyLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown dwellTimeNumeric;
        private System.Windows.Forms.NumericUpDown numberStepsNumeric;
        private System.Windows.Forms.NumericUpDown deviceBandwidthToUseNumeric;
        private System.Windows.Forms.NumericUpDown stopFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown startFrequencyNumeric;
        private System.Windows.Forms.TextBox actualCurrentFrequencyTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;        private System.Windows.Forms.ComboBox clockSourceComboBox;
        private System.Windows.Forms.ComboBox loopBandwidthComboBox;
        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox configurationListParametersGroupBox;

    }
}
