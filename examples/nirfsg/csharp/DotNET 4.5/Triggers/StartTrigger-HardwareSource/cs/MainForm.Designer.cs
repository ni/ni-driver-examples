namespace NationalInstruments.Examples.StartTriggerHardwareSource
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
            this.startTriggerParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.triggerSourceComboBox = new System.Windows.Forms.ComboBox();
            this.triggerTypeComboBox = new System.Windows.Forms.ComboBox();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.triggerTypeLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.stopButton = new System.Windows.Forms.Button();            this.rfsgStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.startTriggerParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // startTriggerParametersGroupBox
            // 
            this.startTriggerParametersGroupBox.Controls.Add(this.triggerSourceComboBox);
            this.startTriggerParametersGroupBox.Controls.Add(this.triggerTypeComboBox);
            this.startTriggerParametersGroupBox.Controls.Add(this.triggerSourceLabel);
            this.startTriggerParametersGroupBox.Controls.Add(this.triggerTypeLabel);
            this.startTriggerParametersGroupBox.Location = new System.Drawing.Point(173, 73);
            this.startTriggerParametersGroupBox.Name = "startTriggerParametersGroupBox";
            this.startTriggerParametersGroupBox.Size = new System.Drawing.Size(159, 121);
            this.startTriggerParametersGroupBox.TabIndex = 11;
            this.startTriggerParametersGroupBox.TabStop = false;
            this.startTriggerParametersGroupBox.Text = "Start Trigger Parameters";
            // 
            // triggerSourceComboBox
            // 
            this.triggerSourceComboBox.Location = new System.Drawing.Point(17, 89);
            this.triggerSourceComboBox.Name = "triggerSourceComboBox";
            this.triggerSourceComboBox.Size = new System.Drawing.Size(120, 21);
            this.triggerSourceComboBox.TabIndex = 4;
            // 
            // triggerTypeComboBox
            // 
            this.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerTypeComboBox.Location = new System.Drawing.Point(17, 38);
            this.triggerTypeComboBox.Name = "triggerTypeComboBox";
            this.triggerTypeComboBox.Size = new System.Drawing.Size(120, 21);
            this.triggerTypeComboBox.TabIndex = 3;
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.AutoSize = true;
            this.triggerSourceLabel.Location = new System.Drawing.Point(14, 69);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(102, 13);
            this.triggerSourceLabel.TabIndex = 4;
            this.triggerSourceLabel.Text = "Start Trigger Source";
            // 
            // triggerTypeLabel
            // 
            this.triggerTypeLabel.AutoSize = true;
            this.triggerTypeLabel.Location = new System.Drawing.Point(14, 17);
            this.triggerTypeLabel.Name = "triggerTypeLabel";
            this.triggerTypeLabel.Size = new System.Drawing.Size(92, 13);
            this.triggerTypeLabel.TabIndex = 3;
            this.triggerTypeLabel.Text = "Start Trigger Type";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(354, 35);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "St&art";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(20, 16);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.AutoSize = true;
            this.frequencyLabel.Location = new System.Drawing.Point(21, 90);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.frequencyLabel.TabIndex = 1;
            this.frequencyLabel.Text = "Center Frequency [Hz]";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(21, 142);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 2;
            this.powerLevelLabel.Text = "Power Level [dBm]";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(20, 214);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 5;
            this.errorLabel.Text = "Warning/Error Message";
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.DecimalPlaces = 6;
            this.frequencyNumeric.Increment = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.frequencyNumeric.Location = new System.Drawing.Point(21, 111);
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
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.DecimalPlaces = 2;
            this.powerLevelNumeric.Location = new System.Drawing.Point(21, 163);
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
            this.powerLevelNumeric.TabIndex = 2;
            this.powerLevelNumeric.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(20, 37);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(120, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(22, 235);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(488, 34);
            this.errorTextBox.TabIndex = 10;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No error.";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(435, 35);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 6;
            this.stopButton.Text = "St&op";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            //             // rfsgStatusTimer
            // 
            this.rfsgStatusTimer.Tick += new System.EventHandler(this.rfsgStatusTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;            this.ClientSize = new System.Drawing.Size(522, 317);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.startTriggerParametersGroupBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.frequencyLabel);
            this.Controls.Add(this.powerLevelLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.frequencyNumeric);
            this.Controls.Add(this.powerLevelNumeric);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.stopButton);            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Start Trigger - Hardware Source";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.startTriggerParametersGroupBox.ResumeLayout(false);
            this.startTriggerParametersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.GroupBox startTriggerParametersGroupBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label triggerTypeLabel;
        private System.Windows.Forms.Label triggerSourceLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;        private System.Windows.Forms.ComboBox triggerTypeComboBox;
        private System.Windows.Forms.ComboBox triggerSourceComboBox;
        private System.Windows.Forms.Timer rfsgStatusTimer;

    }
}
