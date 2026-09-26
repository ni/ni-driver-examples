namespace SelfCalibration
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
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.clockSourceLabel = new System.Windows.Forms.Label();
            this.clockSourceComboBox = new System.Windows.Forms.ComboBox();
            this.selfCalibrationStepLabel = new System.Windows.Forms.Label();
            this.selfCalibrationComboBox = new System.Windows.Forms.ComboBox();
            this.selfCalibrationLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.indicatorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 44);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(110, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.resourceNameLabel.Location = new System.Drawing.Point(15, 21);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 22;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.AutoSize = true;
            this.clockSourceLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.clockSourceLabel.Location = new System.Drawing.Point(12, 75);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.Size = new System.Drawing.Size(71, 13);
            this.clockSourceLabel.TabIndex = 24;
            this.clockSourceLabel.Text = "Clock Source";
            // 
            // clockSourceComboBox
            // 
            this.clockSourceComboBox.Location = new System.Drawing.Point(12, 98);
            this.clockSourceComboBox.Name = "clockSourceComboBox";
            this.clockSourceComboBox.Size = new System.Drawing.Size(116, 21);
            this.clockSourceComboBox.TabIndex = 1;
            // 
            // selfCalibrationStepLabel
            // 
            this.selfCalibrationStepLabel.AutoSize = true;
            this.selfCalibrationStepLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.selfCalibrationStepLabel.Location = new System.Drawing.Point(12, 129);
            this.selfCalibrationStepLabel.Name = "selfCalibrationStepLabel";
            this.selfCalibrationStepLabel.Size = new System.Drawing.Size(154, 13);
            this.selfCalibrationStepLabel.TabIndex = 26;
            this.selfCalibrationStepLabel.Text = "Self Calibration Step Operation ";
            // 
            // selfCalibrationComboBox
            // 
            this.selfCalibrationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selfCalibrationComboBox.Location = new System.Drawing.Point(12, 152);
            this.selfCalibrationComboBox.Name = "selfCalibrationComboBox";
            this.selfCalibrationComboBox.Size = new System.Drawing.Size(206, 21);
            this.selfCalibrationComboBox.TabIndex = 2;
            // 
            // selfCalibrationLabel
            // 
            this.selfCalibrationLabel.AutoSize = true;
            this.selfCalibrationLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.selfCalibrationLabel.Location = new System.Drawing.Point(485, 31);
            this.selfCalibrationLabel.Name = "selfCalibrationLabel";
            this.selfCalibrationLabel.Size = new System.Drawing.Size(80, 13);
            this.selfCalibrationLabel.TabIndex = 29;
            this.selfCalibrationLabel.Text = "Self Calibration ";
            // 
            // startButton
            // 
            this.startButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.startButton.Location = new System.Drawing.Point(12, 189);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(91, 30);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(9, 233);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(666, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "For the NI 5665, the PXI backplane and the LO must share a common Reference clock" +
                ". You can share this clock signal in one of two ways.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(9, 246);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(496, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "- Configure OnboardClock and route the 10 MHz Reference clock out of the LO into " +
                "the PXI backplane.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(9, 259);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(446, 13);
            this.label4.TabIndex = 33;
            this.label4.Text = "- Configure PXI_CLK and provide an external 10 MHz Reference clock to the PXI bac" +
                "kplane.";
            // 
            // indicatorLabel
            // 
            this.indicatorLabel.BackColor = System.Drawing.Color.Gray;
            this.indicatorLabel.Location = new System.Drawing.Point(467, 58);
            this.indicatorLabel.Name = "indicatorLabel";
            this.indicatorLabel.Size = new System.Drawing.Size(114, 30);
            this.indicatorLabel.TabIndex = 34;
            // 
            // MainForm
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 286);
            this.Controls.Add(this.indicatorLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.selfCalibrationLabel);
            this.Controls.Add(this.selfCalibrationStepLabel);
            this.Controls.Add(this.selfCalibrationComboBox);
            this.Controls.Add(this.clockSourceLabel);
            this.Controls.Add(this.clockSourceComboBox);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.resourceNameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "RFSA Self Calibration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label clockSourceLabel;
        private System.Windows.Forms.ComboBox clockSourceComboBox;
        private System.Windows.Forms.Label selfCalibrationStepLabel;
        private System.Windows.Forms.ComboBox selfCalibrationComboBox;
        private System.Windows.Forms.Label selfCalibrationLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label indicatorLabel;
    }
}

