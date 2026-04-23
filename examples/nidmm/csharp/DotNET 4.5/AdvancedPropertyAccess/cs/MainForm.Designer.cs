namespace NationalInstruments.Examples.AdvancedPropertyAccess
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
            this.resourceNameAndMeasurementTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.measurementModeLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.measurementModeComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.measurementGroupBox = new System.Windows.Forms.GroupBox();
            this.actualRangeTextBox = new System.Windows.Forms.TextBox();
            this.actualRangeLabel = new System.Windows.Forms.Label();
            this.readButton = new System.Windows.Forms.Button();
            this.measurementLabel = new System.Windows.Forms.Label();
            this.measurementTextBox = new System.Windows.Forms.TextBox();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.rangeTextBox = new System.Windows.Forms.TextBox();
            this.measurementsToAverageLabel = new System.Windows.Forms.Label();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.measurementsToAverageNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.messageGroupBox = new System.Windows.Forms.GroupBox();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameAndMeasurementTypeGroupBox.SuspendLayout();
            this.measurementGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measurementsToAverageNumericUpDown)).BeginInit();
            this.messageGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // resourceNameAndMeasurementTypeGroupBox
            // 
            this.resourceNameAndMeasurementTypeGroupBox.Controls.Add(this.measurementModeLabel);
            this.resourceNameAndMeasurementTypeGroupBox.Controls.Add(this.resourceNameLabel);
            this.resourceNameAndMeasurementTypeGroupBox.Controls.Add(this.measurementModeComboBox);
            this.resourceNameAndMeasurementTypeGroupBox.Controls.Add(this.resourceNameComboBox);
            this.resourceNameAndMeasurementTypeGroupBox.Location = new System.Drawing.Point(10, 12);
            this.resourceNameAndMeasurementTypeGroupBox.Name = "resourceNameAndMeasurementTypeGroupBox";
            this.resourceNameAndMeasurementTypeGroupBox.Size = new System.Drawing.Size(278, 113);
            this.resourceNameAndMeasurementTypeGroupBox.TabIndex = 0;
            this.resourceNameAndMeasurementTypeGroupBox.TabStop = false;
            this.resourceNameAndMeasurementTypeGroupBox.Text = "Resource Name and Measurement Type";
            // 
            // measurementModeLabel
            // 
            this.measurementModeLabel.AutoSize = true;
            this.measurementModeLabel.Location = new System.Drawing.Point(10, 65);
            this.measurementModeLabel.Name = "measurementModeLabel";
            this.measurementModeLabel.Size = new System.Drawing.Size(104, 13);
            this.measurementModeLabel.TabIndex = 3;
            this.measurementModeLabel.Text = "Measurement Mode:";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(10, 32);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 2;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // measurementModeComboBox
            // 
            this.measurementModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.measurementModeComboBox.FormattingEnabled = true;
            this.measurementModeComboBox.Location = new System.Drawing.Point(147, 62);
            this.measurementModeComboBox.Name = "measurementModeComboBox";
            this.measurementModeComboBox.Size = new System.Drawing.Size(121, 21);
            this.measurementModeComboBox.TabIndex = 1;
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(147, 28);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // measurementGroupBox
            // 
            this.measurementGroupBox.Controls.Add(this.actualRangeTextBox);
            this.measurementGroupBox.Controls.Add(this.actualRangeLabel);
            this.measurementGroupBox.Controls.Add(this.readButton);
            this.measurementGroupBox.Controls.Add(this.measurementLabel);
            this.measurementGroupBox.Controls.Add(this.measurementTextBox);
            this.measurementGroupBox.Location = new System.Drawing.Point(294, 131);
            this.measurementGroupBox.Name = "measurementGroupBox";
            this.measurementGroupBox.Size = new System.Drawing.Size(266, 131);
            this.measurementGroupBox.TabIndex = 3;
            this.measurementGroupBox.TabStop = false;
            this.measurementGroupBox.Text = "Measurement";
            // 
            // actualRangeTextBox
            // 
            this.actualRangeTextBox.Location = new System.Drawing.Point(147, 58);
            this.actualRangeTextBox.Name = "actualRangeTextBox";
            this.actualRangeTextBox.ReadOnly = true;
            this.actualRangeTextBox.Size = new System.Drawing.Size(100, 20);
            this.actualRangeTextBox.TabIndex = 1;
            // 
            // actualRangeLabel
            // 
            this.actualRangeLabel.AutoSize = true;
            this.actualRangeLabel.Location = new System.Drawing.Point(6, 64);
            this.actualRangeLabel.Name = "actualRangeLabel";
            this.actualRangeLabel.Size = new System.Drawing.Size(75, 13);
            this.actualRangeLabel.TabIndex = 3;
            this.actualRangeLabel.Text = "Actual Range:";
            // 
            // readButton
            // 
            this.readButton.Location = new System.Drawing.Point(147, 88);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(75, 23);
            this.readButton.TabIndex = 3;
            this.readButton.Text = "&Read";
            this.readButton.UseVisualStyleBackColor = true;
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // measurementLabel
            // 
            this.measurementLabel.AutoSize = true;
            this.measurementLabel.Location = new System.Drawing.Point(6, 29);
            this.measurementLabel.Name = "measurementLabel";
            this.measurementLabel.Size = new System.Drawing.Size(74, 13);
            this.measurementLabel.TabIndex = 1;
            this.measurementLabel.Text = "Measurement:";
            // 
            // measurementTextBox
            // 
            this.measurementTextBox.Location = new System.Drawing.Point(147, 29);
            this.measurementTextBox.Name = "measurementTextBox";
            this.measurementTextBox.ReadOnly = true;
            this.measurementTextBox.Size = new System.Drawing.Size(100, 20);
            this.measurementTextBox.TabIndex = 0;
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.rangeTextBox);
            this.configurationGroupBox.Controls.Add(this.measurementsToAverageLabel);
            this.configurationGroupBox.Controls.Add(this.rangeLabel);
            this.configurationGroupBox.Controls.Add(this.measurementsToAverageNumericUpDown);
            this.configurationGroupBox.Location = new System.Drawing.Point(294, 12);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(266, 113);
            this.configurationGroupBox.TabIndex = 1;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // rangeTextBox
            // 
            this.rangeTextBox.Location = new System.Drawing.Point(147, 29);
            this.rangeTextBox.Name = "rangeTextBox";
            this.rangeTextBox.Size = new System.Drawing.Size(100, 20);
            this.rangeTextBox.TabIndex = 0;
            this.rangeTextBox.Text = "1.00E-010";
            // 
            // measurementsToAverageLabel
            // 
            this.measurementsToAverageLabel.AutoSize = true;
            this.measurementsToAverageLabel.Location = new System.Drawing.Point(6, 65);
            this.measurementsToAverageLabel.Name = "measurementsToAverageLabel";
            this.measurementsToAverageLabel.Size = new System.Drawing.Size(134, 13);
            this.measurementsToAverageLabel.TabIndex = 3;
            this.measurementsToAverageLabel.Text = "Measurements to Average:";
            // 
            // rangeLabel
            // 
            this.rangeLabel.AutoSize = true;
            this.rangeLabel.Location = new System.Drawing.Point(6, 32);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.Size = new System.Drawing.Size(42, 13);
            this.rangeLabel.TabIndex = 2;
            this.rangeLabel.Text = "Range:";
            // 
            // measurementsToAverageNumericUpDown
            // 
            this.measurementsToAverageNumericUpDown.Location = new System.Drawing.Point(147, 63);
            this.measurementsToAverageNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.measurementsToAverageNumericUpDown.Name = "measurementsToAverageNumericUpDown";
            this.measurementsToAverageNumericUpDown.Size = new System.Drawing.Size(99, 20);
            this.measurementsToAverageNumericUpDown.TabIndex = 1;
            this.measurementsToAverageNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // messageGroupBox
            // 
            this.messageGroupBox.Controls.Add(this.messageTextBox);
            this.messageGroupBox.Location = new System.Drawing.Point(10, 131);
            this.messageGroupBox.Name = "messageGroupBox";
            this.messageGroupBox.Size = new System.Drawing.Size(278, 131);
            this.messageGroupBox.TabIndex = 2;
            this.messageGroupBox.TabStop = false;
            this.messageGroupBox.Text = "Message";
            // 
            // messageTextBox
            // 
            this.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageTextBox.Location = new System.Drawing.Point(3, 16);
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messageTextBox.Size = new System.Drawing.Size(272, 112);
            this.messageTextBox.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AcceptButton = this.readButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(569, 269);
            this.Controls.Add(this.messageGroupBox);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.measurementGroupBox);
            this.Controls.Add(this.resourceNameAndMeasurementTypeGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Advanced Property Access";
            this.resourceNameAndMeasurementTypeGroupBox.ResumeLayout(false);
            this.resourceNameAndMeasurementTypeGroupBox.PerformLayout();
            this.measurementGroupBox.ResumeLayout(false);
            this.measurementGroupBox.PerformLayout();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measurementsToAverageNumericUpDown)).EndInit();
            this.messageGroupBox.ResumeLayout(false);
            this.messageGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox resourceNameAndMeasurementTypeGroupBox;
        private System.Windows.Forms.GroupBox measurementGroupBox;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox messageGroupBox;
        private System.Windows.Forms.ComboBox measurementModeComboBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox measurementTextBox;
        private System.Windows.Forms.NumericUpDown measurementsToAverageNumericUpDown;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.Label measurementModeLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label measurementsToAverageLabel;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.Label measurementLabel;
        private System.Windows.Forms.TextBox actualRangeTextBox;
        private System.Windows.Forms.Label actualRangeLabel;
        private System.Windows.Forms.TextBox rangeTextBox;
    }
}

