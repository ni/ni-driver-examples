using System;
namespace NationalInstruments.Examples.FrequencyAndPowerSweep
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
            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.listTriggerSourceLabel = new System.Windows.Forms.Label();
            this.configurationListTriggerGroupBox = new System.Windows.Forms.GroupBox();
            this.frequencySettlingsNumeric = new System.Windows.Forms.NumericUpDown();
            this.dwellTimeNumeric = new System.Windows.Forms.NumericUpDown();
            this.listTriggerSourceComboBox = new System.Windows.Forms.ComboBox();
            this.frequencySettlingsLabel = new System.Windows.Forms.Label();
            this.dwellTimeLabel = new System.Windows.Forms.Label();
            this.configurationListParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.startFrequencyLabel = new System.Windows.Forms.Label();
            this.startPowerLabel = new System.Windows.Forms.Label();
            this.stopPowerLabel = new System.Windows.Forms.Label();
            this.endFrequencyLabel = new System.Windows.Forms.Label();
            this.numberStepsLabel = new System.Windows.Forms.Label();
            this.startFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.startPowerNumeric = new System.Windows.Forms.NumericUpDown();
            this.stopPowerNumeric = new System.Windows.Forms.NumericUpDown();
            this.endFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.numberStepsNumeric = new System.Windows.Forms.NumericUpDown();
            this.errorLabel = new System.Windows.Forms.Label();
            this.referenceClockSourceLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.startButton = new System.Windows.Forms.Button();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.referenceClockSourceComboBox = new System.Windows.Forms.ComboBox();
            this.configurationListTriggerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencySettlingsNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwellTimeNumeric)).BeginInit();
            this.configurationListParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startPowerNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopPowerNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberStepsNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // listTriggerSourceLabel
            // 
            this.listTriggerSourceLabel.AutoSize = true;
            this.listTriggerSourceLabel.Location = new System.Drawing.Point(10, 20);
            this.listTriggerSourceLabel.Name = "listTriggerSourceLabel";
            this.listTriggerSourceLabel.Size = new System.Drawing.Size(96, 13);
            this.listTriggerSourceLabel.TabIndex = 0;
            this.listTriggerSourceLabel.Text = "List Trigger Source";
            // 
            // configurationListTriggerGroupBox
            // 
            this.configurationListTriggerGroupBox.Controls.Add(this.frequencySettlingsNumeric);
            this.configurationListTriggerGroupBox.Controls.Add(this.dwellTimeNumeric);
            this.configurationListTriggerGroupBox.Controls.Add(this.listTriggerSourceComboBox);
            this.configurationListTriggerGroupBox.Controls.Add(this.frequencySettlingsLabel);
            this.configurationListTriggerGroupBox.Controls.Add(this.listTriggerSourceLabel);
            this.configurationListTriggerGroupBox.Controls.Add(this.dwellTimeLabel);
            this.configurationListTriggerGroupBox.Location = new System.Drawing.Point(15, 123);
            this.configurationListTriggerGroupBox.Name = "configurationListTriggerGroupBox";
            this.configurationListTriggerGroupBox.Size = new System.Drawing.Size(146, 164);
            this.configurationListTriggerGroupBox.TabIndex = 4;
            this.configurationListTriggerGroupBox.TabStop = false;
            this.configurationListTriggerGroupBox.Text = "Configuration List Trigger";
            // 
            // frequencySettlingsNumeric
            // 
            this.frequencySettlingsNumeric.DecimalPlaces = 3;
            this.frequencySettlingsNumeric.Location = new System.Drawing.Point(10, 135);
            this.frequencySettlingsNumeric.Name = "frequencySettlingsNumeric";
            this.frequencySettlingsNumeric.Size = new System.Drawing.Size(120, 20);
            this.frequencySettlingsNumeric.TabIndex = 5;
            this.frequencySettlingsNumeric.Value = new decimal(new int[] {
            7,
            0,
            0,
            196608});
            // 
            // dwellTimeNumeric
            // 
            this.dwellTimeNumeric.DecimalPlaces = 3;
            this.dwellTimeNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.dwellTimeNumeric.Location = new System.Drawing.Point(10, 85);
            this.dwellTimeNumeric.Name = "dwellTimeNumeric";
            this.dwellTimeNumeric.Size = new System.Drawing.Size(120, 20);
            this.dwellTimeNumeric.TabIndex = 3;
            this.dwellTimeNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            196608});
            // 
            // listTriggerSourceComboBox
            // 
            this.listTriggerSourceComboBox.Location = new System.Drawing.Point(10, 37);
            this.listTriggerSourceComboBox.Name = "listTriggerSourceComboBox";
            this.listTriggerSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.listTriggerSourceComboBox.TabIndex = 1;
            // 
            // frequencySettlingsLabel
            // 
            this.frequencySettlingsLabel.AutoSize = true;
            this.frequencySettlingsLabel.Location = new System.Drawing.Point(10, 118);
            this.frequencySettlingsLabel.Name = "frequencySettlingsLabel";
            this.frequencySettlingsLabel.Size = new System.Drawing.Size(106, 13);
            this.frequencySettlingsLabel.TabIndex = 4;
            this.frequencySettlingsLabel.Text = "Frequency Settling(s)";
            // 
            // dwellTimeLabel
            // 
            this.dwellTimeLabel.AutoSize = true;
            this.dwellTimeLabel.Location = new System.Drawing.Point(10, 68);
            this.dwellTimeLabel.Name = "dwellTimeLabel";
            this.dwellTimeLabel.Size = new System.Drawing.Size(121, 13);
            this.dwellTimeLabel.TabIndex = 2;
            this.dwellTimeLabel.Text = "Dwell Time in Second(s)";
            // 
            // configurationListParametersGroupBox
            // 
            this.configurationListParametersGroupBox.Controls.Add(this.startFrequencyLabel);
            this.configurationListParametersGroupBox.Controls.Add(this.startPowerLabel);
            this.configurationListParametersGroupBox.Controls.Add(this.stopPowerLabel);
            this.configurationListParametersGroupBox.Controls.Add(this.endFrequencyLabel);
            this.configurationListParametersGroupBox.Controls.Add(this.numberStepsLabel);
            this.configurationListParametersGroupBox.Controls.Add(this.startFrequencyNumeric);
            this.configurationListParametersGroupBox.Controls.Add(this.startPowerNumeric);
            this.configurationListParametersGroupBox.Controls.Add(this.stopPowerNumeric);
            this.configurationListParametersGroupBox.Controls.Add(this.endFrequencyNumeric);
            this.configurationListParametersGroupBox.Controls.Add(this.numberStepsNumeric);
            this.configurationListParametersGroupBox.Location = new System.Drawing.Point(170, 25);
            this.configurationListParametersGroupBox.Name = "configurationListParametersGroupBox";
            this.configurationListParametersGroupBox.Size = new System.Drawing.Size(177, 262);
            this.configurationListParametersGroupBox.TabIndex = 5;
            this.configurationListParametersGroupBox.TabStop = false;
            this.configurationListParametersGroupBox.Text = "Configuration List Parameters";
            // 
            // startFrequencyLabel
            // 
            this.startFrequencyLabel.AutoSize = true;
            this.startFrequencyLabel.Location = new System.Drawing.Point(21, 20);
            this.startFrequencyLabel.Name = "startFrequencyLabel";
            this.startFrequencyLabel.Size = new System.Drawing.Size(104, 13);
            this.startFrequencyLabel.TabIndex = 0;
            this.startFrequencyLabel.Text = "Start Frequency [Hz]";
            // 
            // startPowerLabel
            // 
            this.startPowerLabel.AutoSize = true;
            this.startPowerLabel.Location = new System.Drawing.Point(18, 118);
            this.startPowerLabel.Name = "startPowerLabel";
            this.startPowerLabel.Size = new System.Drawing.Size(92, 13);
            this.startPowerLabel.TabIndex = 4;
            this.startPowerLabel.Text = "Start Power [dBm]";
            // 
            // stopPowerLabel
            // 
            this.stopPowerLabel.AutoSize = true;
            this.stopPowerLabel.Location = new System.Drawing.Point(18, 167);
            this.stopPowerLabel.Name = "stopPowerLabel";
            this.stopPowerLabel.Size = new System.Drawing.Size(92, 13);
            this.stopPowerLabel.TabIndex = 6;
            this.stopPowerLabel.Text = "Stop Power [dBm]";
            // 
            // endFrequencyLabel
            // 
            this.endFrequencyLabel.AutoSize = true;
            this.endFrequencyLabel.Location = new System.Drawing.Point(22, 69);
            this.endFrequencyLabel.Name = "endFrequencyLabel";
            this.endFrequencyLabel.Size = new System.Drawing.Size(101, 13);
            this.endFrequencyLabel.TabIndex = 2;
            this.endFrequencyLabel.Text = "End Frequency [Hz]";
            // 
            // numberStepsLabel
            // 
            this.numberStepsLabel.AutoSize = true;
            this.numberStepsLabel.Location = new System.Drawing.Point(22, 216);
            this.numberStepsLabel.Name = "numberStepsLabel";
            this.numberStepsLabel.Size = new System.Drawing.Size(86, 13);
            this.numberStepsLabel.TabIndex = 8;
            this.numberStepsLabel.Text = "Number of Steps";
            // 
            // startFrequencyNumeric
            // 
            this.startFrequencyNumeric.DecimalPlaces = 6;
            this.startFrequencyNumeric.Location = new System.Drawing.Point(18, 37);
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
            this.startFrequencyNumeric.TabIndex = 1;
            this.startFrequencyNumeric.Value = new decimal(new int[] {
            990000000,
            0,
            0,
            0});
            // 
            // startPowerNumeric
            // 
            this.startPowerNumeric.DecimalPlaces = 2;
            this.startPowerNumeric.Location = new System.Drawing.Point(18, 135);
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
            this.startPowerNumeric.TabIndex = 5;
            this.startPowerNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // stopPowerNumeric
            // 
            this.stopPowerNumeric.DecimalPlaces = 2;
            this.stopPowerNumeric.Location = new System.Drawing.Point(18, 184);
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
            this.stopPowerNumeric.TabIndex = 7;
            // 
            // endFrequencyNumeric
            // 
            this.endFrequencyNumeric.DecimalPlaces = 6;
            this.endFrequencyNumeric.Location = new System.Drawing.Point(18, 86);
            this.endFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.endFrequencyNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.endFrequencyNumeric.Name = "endFrequencyNumeric";
            this.endFrequencyNumeric.Size = new System.Drawing.Size(120, 20);
            this.endFrequencyNumeric.TabIndex = 3;
            this.endFrequencyNumeric.Value = new decimal(new int[] {
            1010000000,
            0,
            0,
            0});
            // 
            // numberStepsNumeric
            // 
            this.numberStepsNumeric.Location = new System.Drawing.Point(18, 233);
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
            this.numberStepsNumeric.TabIndex = 9;
            this.numberStepsNumeric.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(12, 302);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 6;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // referenceClockSourceLabel
            // 
            this.referenceClockSourceLabel.AutoSize = true;
            this.referenceClockSourceLabel.Location = new System.Drawing.Point(12, 59);
            this.referenceClockSourceLabel.Name = "referenceClockSourceLabel";
            this.referenceClockSourceLabel.Size = new System.Drawing.Size(124, 13);
            this.referenceClockSourceLabel.TabIndex = 2;
            this.referenceClockSourceLabel.Text = "Reference Clock Source";
            // 
            // stopButton
            // 
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(270, 391);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 9;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 25);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 1;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(189, 391);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 8;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(15, 318);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(332, 67);
            this.errorTextBox.TabIndex = 7;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No Error";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(12, 9);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // referenceClockSourceComboBox
            // 
            this.referenceClockSourceComboBox.Location = new System.Drawing.Point(12, 75);
            this.referenceClockSourceComboBox.Name = "referenceClockSourceComboBox";
            this.referenceClockSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.referenceClockSourceComboBox.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(357, 422);
            this.Controls.Add(this.configurationListTriggerGroupBox);
            this.Controls.Add(this.configurationListParametersGroupBox);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.referenceClockSourceLabel);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.referenceClockSourceComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Frequency And Power Sweep";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.configurationListTriggerGroupBox.ResumeLayout(false);
            this.configurationListTriggerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencySettlingsNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dwellTimeNumeric)).EndInit();
            this.configurationListParametersGroupBox.ResumeLayout(false);
            this.configurationListParametersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startPowerNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopPowerNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberStepsNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Timer rfsgStatusTimer;
        private System.Windows.Forms.Label listTriggerSourceLabel;
        private System.Windows.Forms.GroupBox configurationListTriggerGroupBox;
        private System.Windows.Forms.NumericUpDown frequencySettlingsNumeric;
        private System.Windows.Forms.NumericUpDown dwellTimeNumeric;
        private System.Windows.Forms.ComboBox listTriggerSourceComboBox;
        private System.Windows.Forms.Label frequencySettlingsLabel;
        private System.Windows.Forms.Label dwellTimeLabel;
        private System.Windows.Forms.GroupBox configurationListParametersGroupBox;
        private System.Windows.Forms.Label startFrequencyLabel;
        private System.Windows.Forms.Label startPowerLabel;
        private System.Windows.Forms.Label stopPowerLabel;
        private System.Windows.Forms.Label endFrequencyLabel;
        private System.Windows.Forms.Label numberStepsLabel;
        private System.Windows.Forms.NumericUpDown startFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown startPowerNumeric;
        private System.Windows.Forms.NumericUpDown stopPowerNumeric;
        private System.Windows.Forms.NumericUpDown endFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown numberStepsNumeric;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label referenceClockSourceLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.ComboBox referenceClockSourceComboBox;

    }
}
