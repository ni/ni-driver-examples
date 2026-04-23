namespace NationalInstruments.Examples.IviDCPowerDotNetApplication
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
            this.initializeDCPowerGroupBox = new System.Windows.Forms.GroupBox();
            this.resetDeviceCheckBox = new System.Windows.Forms.CheckBox();
            this.idQueryCheckBox = new System.Windows.Forms.CheckBox();
            this.initializeButton = new System.Windows.Forms.Button();
            this.logicalNameTextBox = new System.Windows.Forms.TextBox();
            this.logicalNameLabel = new System.Windows.Forms.Label();
            this.messageRichTextBox = new System.Windows.Forms.RichTextBox();
            this.configureDCPowerGroupBox = new System.Windows.Forms.GroupBox();
            this.configureAndOutputButton = new System.Windows.Forms.Button();
            this.ovpEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.outputEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.channelNameComboBox = new System.Windows.Forms.ComboBox();
            this.currentLimitLabel = new System.Windows.Forms.Label();
            this.currentLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.ovpLimitLabel = new System.Windows.Forms.Label();
            this.ovpLimitNumeric = new System.Windows.Forms.NumericUpDown();
            this.voltageLevelLabel = new System.Windows.Forms.Label();
            this.voltageLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentLimitBehaviorComboBox = new System.Windows.Forms.ComboBox();
            this.currentLimitBehaviorLabel = new System.Windows.Forms.Label();
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.initializeDCPowerGroupBox.SuspendLayout();
            this.configureDCPowerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ovpLimitNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // initializeDCPowerGroupBox
            // 
            this.initializeDCPowerGroupBox.Controls.Add(this.resetDeviceCheckBox);
            this.initializeDCPowerGroupBox.Controls.Add(this.idQueryCheckBox);
            this.initializeDCPowerGroupBox.Controls.Add(this.initializeButton);
            this.initializeDCPowerGroupBox.Controls.Add(this.logicalNameTextBox);
            this.initializeDCPowerGroupBox.Controls.Add(this.logicalNameLabel);
            this.initializeDCPowerGroupBox.Controls.Add(this.messageRichTextBox);
            this.initializeDCPowerGroupBox.Location = new System.Drawing.Point(12, 12);
            this.initializeDCPowerGroupBox.Name = "initializeDCPowerGroupBox";
            this.initializeDCPowerGroupBox.Size = new System.Drawing.Size(454, 146);
            this.initializeDCPowerGroupBox.TabIndex = 0;
            this.initializeDCPowerGroupBox.TabStop = false;
            this.initializeDCPowerGroupBox.Text = "Initialize DC Power";
            // 
            // resetDeviceCheckBox
            // 
            this.resetDeviceCheckBox.AutoSize = true;
            this.resetDeviceCheckBox.Checked = true;
            this.resetDeviceCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.resetDeviceCheckBox.Location = new System.Drawing.Point(357, 24);
            this.resetDeviceCheckBox.Name = "resetDeviceCheckBox";
            this.resetDeviceCheckBox.Size = new System.Drawing.Size(91, 17);
            this.resetDeviceCheckBox.TabIndex = 3;
            this.resetDeviceCheckBox.Text = "Reset Device";
            this.resetDeviceCheckBox.UseVisualStyleBackColor = true;
            // 
            // idQueryCheckBox
            // 
            this.idQueryCheckBox.AutoSize = true;
            this.idQueryCheckBox.Checked = true;
            this.idQueryCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.idQueryCheckBox.Location = new System.Drawing.Point(248, 24);
            this.idQueryCheckBox.Name = "idQueryCheckBox";
            this.idQueryCheckBox.Size = new System.Drawing.Size(68, 17);
            this.idQueryCheckBox.TabIndex = 2;
            this.idQueryCheckBox.Text = "ID Query";
            this.idQueryCheckBox.UseVisualStyleBackColor = true;
            // 
            // initializeButton
            // 
            this.initializeButton.Location = new System.Drawing.Point(173, 115);
            this.initializeButton.Name = "initializeButton";
            this.initializeButton.Size = new System.Drawing.Size(73, 23);
            this.initializeButton.TabIndex = 5;
            this.initializeButton.Text = "&Initialize";
            this.initializeButton.UseVisualStyleBackColor = true;
            this.initializeButton.Click += new System.EventHandler(this.initializeButton_Click);
            // 
            // logicalNameTextBox
            // 
            this.logicalNameTextBox.Location = new System.Drawing.Point(84, 22);
            this.logicalNameTextBox.Name = "logicalNameTextBox";
            this.logicalNameTextBox.Size = new System.Drawing.Size(108, 20);
            this.logicalNameTextBox.TabIndex = 1;
            // 
            // logicalNameLabel
            // 
            this.logicalNameLabel.AutoSize = true;
            this.logicalNameLabel.Location = new System.Drawing.Point(6, 26);
            this.logicalNameLabel.Name = "logicalNameLabel";
            this.logicalNameLabel.Size = new System.Drawing.Size(72, 13);
            this.logicalNameLabel.TabIndex = 0;
            this.logicalNameLabel.Text = "Logical Name";
            // 
            // messageRichTextBox
            // 
            this.messageRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageRichTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageRichTextBox.Location = new System.Drawing.Point(6, 58);
            this.messageRichTextBox.Name = "messageRichTextBox";
            this.messageRichTextBox.ReadOnly = true;
            this.messageRichTextBox.Size = new System.Drawing.Size(442, 41);
            this.messageRichTextBox.TabIndex = 4;
            this.messageRichTextBox.TabStop = false;
            this.messageRichTextBox.Text = resources.GetString("messageRichTextBox.Text");
            // 
            // configureDCPowerGroupBox
            // 
            this.configureDCPowerGroupBox.Controls.Add(this.configureAndOutputButton);
            this.configureDCPowerGroupBox.Controls.Add(this.ovpEnabledCheckBox);
            this.configureDCPowerGroupBox.Controls.Add(this.outputEnabledCheckBox);
            this.configureDCPowerGroupBox.Controls.Add(this.channelNameComboBox);
            this.configureDCPowerGroupBox.Controls.Add(this.currentLimitLabel);
            this.configureDCPowerGroupBox.Controls.Add(this.currentLimitNumeric);
            this.configureDCPowerGroupBox.Controls.Add(this.ovpLimitLabel);
            this.configureDCPowerGroupBox.Controls.Add(this.ovpLimitNumeric);
            this.configureDCPowerGroupBox.Controls.Add(this.voltageLevelLabel);
            this.configureDCPowerGroupBox.Controls.Add(this.voltageLevelNumeric);
            this.configureDCPowerGroupBox.Controls.Add(this.currentLimitBehaviorComboBox);
            this.configureDCPowerGroupBox.Controls.Add(this.currentLimitBehaviorLabel);
            this.configureDCPowerGroupBox.Controls.Add(this.channelNameLabel);
            this.configureDCPowerGroupBox.Location = new System.Drawing.Point(12, 174);
            this.configureDCPowerGroupBox.Name = "configureDCPowerGroupBox";
            this.configureDCPowerGroupBox.Size = new System.Drawing.Size(454, 182);
            this.configureDCPowerGroupBox.TabIndex = 1;
            this.configureDCPowerGroupBox.TabStop = false;
            this.configureDCPowerGroupBox.Text = "Configure DC Power";
            // 
            // configureAndOutputButton
            // 
            this.configureAndOutputButton.Location = new System.Drawing.Point(149, 150);
            this.configureAndOutputButton.Name = "configureAndOutputButton";
            this.configureAndOutputButton.Size = new System.Drawing.Size(120, 23);
            this.configureAndOutputButton.TabIndex = 12;
            this.configureAndOutputButton.Text = "&Configure and Output";
            this.configureAndOutputButton.UseVisualStyleBackColor = true;
            this.configureAndOutputButton.Click += new System.EventHandler(this.configureAndOutputButton_Click);
            // 
            // ovpEnabledCheckBox
            // 
            this.ovpEnabledCheckBox.AutoSize = true;
            this.ovpEnabledCheckBox.Checked = true;
            this.ovpEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ovpEnabledCheckBox.Location = new System.Drawing.Point(52, 84);
            this.ovpEnabledCheckBox.Name = "ovpEnabledCheckBox";
            this.ovpEnabledCheckBox.Size = new System.Drawing.Size(90, 17);
            this.ovpEnabledCheckBox.TabIndex = 11;
            this.ovpEnabledCheckBox.Text = "OVP Enabled";
            this.ovpEnabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // outputEnabledCheckBox
            // 
            this.outputEnabledCheckBox.AutoSize = true;
            this.outputEnabledCheckBox.Location = new System.Drawing.Point(52, 58);
            this.outputEnabledCheckBox.Name = "outputEnabledCheckBox";
            this.outputEnabledCheckBox.Size = new System.Drawing.Size(100, 17);
            this.outputEnabledCheckBox.TabIndex = 10;
            this.outputEnabledCheckBox.Text = "Output Enabled";
            this.outputEnabledCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.outputEnabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // channelNameComboBox
            // 
            this.channelNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.channelNameComboBox.Location = new System.Drawing.Point(202, 19);
            this.channelNameComboBox.Name = "channelNameComboBox";
            this.channelNameComboBox.Size = new System.Drawing.Size(91, 21);
            this.channelNameComboBox.TabIndex = 1;
            // 
            // currentLimitLabel
            // 
            this.currentLimitLabel.AutoSize = true;
            this.currentLimitLabel.Location = new System.Drawing.Point(245, 112);
            this.currentLimitLabel.Name = "currentLimitLabel";
            this.currentLimitLabel.Size = new System.Drawing.Size(81, 13);
            this.currentLimitLabel.TabIndex = 6;
            this.currentLimitLabel.Text = "Current Limit (A)";
            // 
            // currentLimitNumeric
            // 
            this.currentLimitNumeric.DecimalPlaces = 6;
            this.currentLimitNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.currentLimitNumeric.Location = new System.Drawing.Point(357, 108);
            this.currentLimitNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.currentLimitNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.currentLimitNumeric.Name = "currentLimitNumeric";
            this.currentLimitNumeric.Size = new System.Drawing.Size(91, 20);
            this.currentLimitNumeric.TabIndex = 7;
            this.currentLimitNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            131072});
            // 
            // ovpLimitLabel
            // 
            this.ovpLimitLabel.AutoSize = true;
            this.ovpLimitLabel.Location = new System.Drawing.Point(245, 86);
            this.ovpLimitLabel.Name = "ovpLimitLabel";
            this.ovpLimitLabel.Size = new System.Drawing.Size(69, 13);
            this.ovpLimitLabel.TabIndex = 4;
            this.ovpLimitLabel.Text = "OVP Limit (V)";
            // 
            // ovpLimitNumeric
            // 
            this.ovpLimitNumeric.DecimalPlaces = 6;
            this.ovpLimitNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ovpLimitNumeric.Location = new System.Drawing.Point(357, 82);
            this.ovpLimitNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.ovpLimitNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.ovpLimitNumeric.Name = "ovpLimitNumeric";
            this.ovpLimitNumeric.Size = new System.Drawing.Size(91, 20);
            this.ovpLimitNumeric.TabIndex = 5;
            this.ovpLimitNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // voltageLevelLabel
            // 
            this.voltageLevelLabel.AutoSize = true;
            this.voltageLevelLabel.Location = new System.Drawing.Point(245, 60);
            this.voltageLevelLabel.Name = "voltageLevelLabel";
            this.voltageLevelLabel.Size = new System.Drawing.Size(106, 13);
            this.voltageLevelLabel.TabIndex = 2;
            this.voltageLevelLabel.Text = "DC Voltage Level (V)";
            // 
            // voltageLevelNumeric
            // 
            this.voltageLevelNumeric.DecimalPlaces = 6;
            this.voltageLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.voltageLevelNumeric.Location = new System.Drawing.Point(357, 56);
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
            this.voltageLevelNumeric.Size = new System.Drawing.Size(91, 20);
            this.voltageLevelNumeric.TabIndex = 3;
            this.voltageLevelNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // currentLimitBehaviorComboBox
            // 
            this.currentLimitBehaviorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.currentLimitBehaviorComboBox.Location = new System.Drawing.Point(122, 107);
            this.currentLimitBehaviorComboBox.Name = "currentLimitBehaviorComboBox";
            this.currentLimitBehaviorComboBox.Size = new System.Drawing.Size(91, 21);
            this.currentLimitBehaviorComboBox.TabIndex = 9;
            // 
            // currentLimitBehaviorLabel
            // 
            this.currentLimitBehaviorLabel.AutoSize = true;
            this.currentLimitBehaviorLabel.Location = new System.Drawing.Point(6, 112);
            this.currentLimitBehaviorLabel.Name = "currentLimitBehaviorLabel";
            this.currentLimitBehaviorLabel.Size = new System.Drawing.Size(110, 13);
            this.currentLimitBehaviorLabel.TabIndex = 8;
            this.currentLimitBehaviorLabel.Text = "Current Limit Behavior";
            // 
            // channelNameLabel
            // 
            this.channelNameLabel.AutoSize = true;
            this.channelNameLabel.Location = new System.Drawing.Point(119, 23);
            this.channelNameLabel.Name = "channelNameLabel";
            this.channelNameLabel.Size = new System.Drawing.Size(77, 13);
            this.channelNameLabel.TabIndex = 0;
            this.channelNameLabel.Text = "Channel Name";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 367);
            this.Controls.Add(this.configureDCPowerGroupBox);
            this.Controls.Add(this.initializeDCPowerGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "IVI DCPower .NET application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            this.initializeDCPowerGroupBox.ResumeLayout(false);
            this.initializeDCPowerGroupBox.PerformLayout();
            this.configureDCPowerGroupBox.ResumeLayout(false);
            this.configureDCPowerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentLimitNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ovpLimitNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltageLevelNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox initializeDCPowerGroupBox;
        private System.Windows.Forms.RichTextBox messageRichTextBox;
        private System.Windows.Forms.TextBox logicalNameTextBox;
        private System.Windows.Forms.Label logicalNameLabel;
        private System.Windows.Forms.GroupBox configureDCPowerGroupBox;
        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label voltageLevelLabel;
        private System.Windows.Forms.NumericUpDown voltageLevelNumeric;
        private System.Windows.Forms.ComboBox currentLimitBehaviorComboBox;
        private System.Windows.Forms.Label currentLimitBehaviorLabel;
        private System.Windows.Forms.Label currentLimitLabel;
        private System.Windows.Forms.NumericUpDown currentLimitNumeric;
        private System.Windows.Forms.Label ovpLimitLabel;
        private System.Windows.Forms.NumericUpDown ovpLimitNumeric;
        private System.Windows.Forms.Button configureAndOutputButton;
        private System.Windows.Forms.Button initializeButton;
        private System.Windows.Forms.ComboBox channelNameComboBox;
        private System.Windows.Forms.CheckBox resetDeviceCheckBox;
        private System.Windows.Forms.CheckBox idQueryCheckBox;
        private System.Windows.Forms.CheckBox ovpEnabledCheckBox;
        private System.Windows.Forms.CheckBox outputEnabledCheckBox;
    }
}

