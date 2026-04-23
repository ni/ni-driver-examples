namespace NationalInstruments.Examples.Calibrate
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
            this.calibrateButton = new System.Windows.Forms.Button();
            this.optionComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.messageGroupBox = new System.Windows.Forms.GroupBox();
            this.messageTextBox = new System.Windows.Forms.RichTextBox();
            this.resourceNameGroupBox = new System.Windows.Forms.GroupBox();
            this.optoinGroupBox = new System.Windows.Forms.GroupBox();
            this.buttonsGroupBox = new System.Windows.Forms.GroupBox();
            this.messageGroupBox.SuspendLayout();
            this.resourceNameGroupBox.SuspendLayout();
            this.optoinGroupBox.SuspendLayout();
            this.buttonsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // calibrateButton
            // 
            this.calibrateButton.Location = new System.Drawing.Point(65, 19);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(75, 23);
            this.calibrateButton.TabIndex = 0;
            this.calibrateButton.Text = "&Calibrate";
            this.calibrateButton.UseVisualStyleBackColor = true;
            this.calibrateButton.Click += new System.EventHandler(this.calibrateButton_Click);
            // 
            // optionComboBox
            // 
            this.optionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.optionComboBox.Location = new System.Drawing.Point(6, 19);
            this.optionComboBox.Name = "optionComboBox";
            this.optionComboBox.Size = new System.Drawing.Size(193, 21);
            this.optionComboBox.TabIndex = 0;
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.Location = new System.Drawing.Point(9, 19);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(104, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // messageGroupBox
            // 
            this.messageGroupBox.Controls.Add(this.messageTextBox);
            this.messageGroupBox.Location = new System.Drawing.Point(12, 132);
            this.messageGroupBox.Name = "messageGroupBox";
            this.messageGroupBox.Size = new System.Drawing.Size(205, 84);
            this.messageGroupBox.TabIndex = 2;
            this.messageGroupBox.TabStop = false;
            this.messageGroupBox.Text = "Message";
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(9, 19);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.Size = new System.Drawing.Size(190, 59);
            this.messageTextBox.TabIndex = 0;
            this.messageTextBox.Text = "Note : Calibration may take several minutes to complete.";
            // 
            // resourceNameGroupBox
            // 
            this.resourceNameGroupBox.Controls.Add(this.resourceNameComboBox);
            this.resourceNameGroupBox.Location = new System.Drawing.Point(12, 12);
            this.resourceNameGroupBox.Name = "resourceNameGroupBox";
            this.resourceNameGroupBox.Size = new System.Drawing.Size(205, 49);
            this.resourceNameGroupBox.TabIndex = 0;
            this.resourceNameGroupBox.TabStop = false;
            this.resourceNameGroupBox.Text = "Resource Name";
            // 
            // optoinGroupBox
            // 
            this.optoinGroupBox.Controls.Add(this.optionComboBox);
            this.optoinGroupBox.Location = new System.Drawing.Point(12, 72);
            this.optoinGroupBox.Name = "optoinGroupBox";
            this.optoinGroupBox.Size = new System.Drawing.Size(205, 49);
            this.optoinGroupBox.TabIndex = 1;
            this.optoinGroupBox.TabStop = false;
            this.optoinGroupBox.Text = "Self Calibration Option";
            // 
            // buttonsGroupBox
            // 
            this.buttonsGroupBox.Controls.Add(this.calibrateButton);
            this.buttonsGroupBox.Location = new System.Drawing.Point(12, 222);
            this.buttonsGroupBox.Name = "buttonsGroupBox";
            this.buttonsGroupBox.Size = new System.Drawing.Size(205, 53);
            this.buttonsGroupBox.TabIndex = 3;
            this.buttonsGroupBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 288);
            this.Controls.Add(this.buttonsGroupBox);
            this.Controls.Add(this.optoinGroupBox);
            this.Controls.Add(this.resourceNameGroupBox);
            this.Controls.Add(this.messageGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Calibrate";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            this.messageGroupBox.ResumeLayout(false);
            this.resourceNameGroupBox.ResumeLayout(false);
            this.optoinGroupBox.ResumeLayout(false);
            this.buttonsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button calibrateButton;
        private System.Windows.Forms.ComboBox optionComboBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.GroupBox messageGroupBox;
        private System.Windows.Forms.RichTextBox messageTextBox;
        private System.Windows.Forms.GroupBox resourceNameGroupBox;
        private System.Windows.Forms.GroupBox optoinGroupBox;
        private System.Windows.Forms.GroupBox buttonsGroupBox;


    }
}
