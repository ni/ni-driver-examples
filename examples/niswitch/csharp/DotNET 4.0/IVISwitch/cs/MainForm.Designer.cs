namespace NationalInstruments.Examples.IviSwitch
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
            this.resetDeviceCheckBox = new System.Windows.Forms.CheckBox();
            this.idQueryCheckBox = new System.Windows.Forms.CheckBox();
            this.resourceNameTextBox = new System.Windows.Forms.TextBox();
            this.initializeGroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.initializeButton = new System.Windows.Forms.Button();
            this.initializeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // resetDeviceCheckBox
            // 
            this.resetDeviceCheckBox.AutoSize = true;
            this.resetDeviceCheckBox.Location = new System.Drawing.Point(145, 37);
            this.resetDeviceCheckBox.Name = "resetDeviceCheckBox";
            this.resetDeviceCheckBox.Size = new System.Drawing.Size(91, 17);
            this.resetDeviceCheckBox.TabIndex = 19;
            this.resetDeviceCheckBox.Text = "Reset Device";
            this.resetDeviceCheckBox.UseVisualStyleBackColor = true;
            // 
            // idQueryCheckBox
            // 
            this.idQueryCheckBox.AutoSize = true;
            this.idQueryCheckBox.Location = new System.Drawing.Point(145, 15);
            this.idQueryCheckBox.Name = "idQueryCheckBox";
            this.idQueryCheckBox.Size = new System.Drawing.Size(68, 17);
            this.idQueryCheckBox.TabIndex = 18;
            this.idQueryCheckBox.Text = "ID Query";
            this.idQueryCheckBox.UseVisualStyleBackColor = true;
            // 
            // resourceNameTextBox
            // 
            this.resourceNameTextBox.Location = new System.Drawing.Point(9, 35);
            this.resourceNameTextBox.Name = "resourceNameTextBox";
            this.resourceNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.resourceNameTextBox.TabIndex = 1;
            // 
            // initializeGroupBox
            // 
            this.initializeGroupBox.Controls.Add(this.resetDeviceCheckBox);
            this.initializeGroupBox.Controls.Add(this.idQueryCheckBox);
            this.initializeGroupBox.Controls.Add(this.resourceNameTextBox);
            this.initializeGroupBox.Controls.Add(this.resourceNameLabel);
            this.initializeGroupBox.Location = new System.Drawing.Point(16, 12);
            this.initializeGroupBox.Name = "initializeGroupBox";
            this.initializeGroupBox.Size = new System.Drawing.Size(265, 68);
            this.initializeGroupBox.TabIndex = 18;
            this.initializeGroupBox.TabStop = false;
            this.initializeGroupBox.Text = "Initialize Switch";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 16);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 15;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // initializeButton
            // 
            this.initializeButton.Location = new System.Drawing.Point(108, 86);
            this.initializeButton.Name = "initializeButton";
            this.initializeButton.Size = new System.Drawing.Size(75, 23);
            this.initializeButton.TabIndex = 2;
            this.initializeButton.Text = "Initialize";
            this.initializeButton.UseVisualStyleBackColor = true;
            this.initializeButton.Click += new System.EventHandler(this.initializeButton_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.initializeButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 116);
            this.Controls.Add(this.initializeGroupBox);
            this.Controls.Add(this.initializeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Initialize Switch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.initializeGroupBox.ResumeLayout(false);
            this.initializeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox resetDeviceCheckBox;
        private System.Windows.Forms.CheckBox idQueryCheckBox;
        private System.Windows.Forms.TextBox resourceNameTextBox;
        private System.Windows.Forms.GroupBox initializeGroupBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Button initializeButton;

    }
}

