namespace NationalInstruments.Examples.SoftwareScanning
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
            this.topologyNameComboBox = new System.Windows.Forms.ComboBox();
            this.topologyNameLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.scanListLabel = new System.Windows.Forms.Label();
            this.scanListTextBox = new System.Windows.Forms.TextBox();
            this.startScanningButton = new System.Windows.Forms.Button();
            this.nextConnectionButton = new System.Windows.Forms.Button();
            this.stopScanningButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(6, 30);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(143, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // topologyNameComboBox
            // 
            this.topologyNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.topologyNameComboBox.FormattingEnabled = true;
            this.topologyNameComboBox.Location = new System.Drawing.Point(6, 82);
            this.topologyNameComboBox.Name = "topologyNameComboBox";
            this.topologyNameComboBox.Size = new System.Drawing.Size(143, 21);
            this.topologyNameComboBox.TabIndex = 1;
            // 
            // topologyNameLabel
            // 
            this.topologyNameLabel.AutoSize = true;
            this.topologyNameLabel.Location = new System.Drawing.Point(6, 66);
            this.topologyNameLabel.Name = "topologyNameLabel";
            this.topologyNameLabel.Size = new System.Drawing.Size(82, 13);
            this.topologyNameLabel.TabIndex = 4;
            this.topologyNameLabel.Text = "Topology Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.resourceNameLabel);
            this.groupBox1.Controls.Add(this.scanListLabel);
            this.groupBox1.Controls.Add(this.scanListTextBox);
            this.groupBox1.Controls.Add(this.topologyNameLabel);
            this.groupBox1.Controls.Add(this.topologyNameComboBox);
            this.groupBox1.Controls.Add(this.resourceNameComboBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(251, 164);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 14);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 3;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // scanListLabel
            // 
            this.scanListLabel.AutoSize = true;
            this.scanListLabel.Location = new System.Drawing.Point(6, 122);
            this.scanListLabel.Name = "scanListLabel";
            this.scanListLabel.Size = new System.Drawing.Size(51, 13);
            this.scanListLabel.TabIndex = 5;
            this.scanListLabel.Text = "Scan List";
            // 
            // scanListTextBox
            // 
            this.scanListTextBox.Location = new System.Drawing.Point(9, 138);
            this.scanListTextBox.Name = "scanListTextBox";
            this.scanListTextBox.Size = new System.Drawing.Size(100, 20);
            this.scanListTextBox.TabIndex = 2;
            this.scanListTextBox.Text = "ch0:15->com0;";
            // 
            // startScanningButton
            // 
            this.startScanningButton.Location = new System.Drawing.Point(12, 202);
            this.startScanningButton.Name = "startScanningButton";
            this.startScanningButton.Size = new System.Drawing.Size(75, 37);
            this.startScanningButton.TabIndex = 1;
            this.startScanningButton.Text = "Start Scanning";
            this.startScanningButton.UseVisualStyleBackColor = true;
            this.startScanningButton.Click += new System.EventHandler(this.startScanningButton_Click);
            // 
            // nextConnectionButton
            // 
            this.nextConnectionButton.Location = new System.Drawing.Point(102, 202);
            this.nextConnectionButton.Name = "nextConnectionButton";
            this.nextConnectionButton.Size = new System.Drawing.Size(75, 37);
            this.nextConnectionButton.TabIndex = 2;
            this.nextConnectionButton.Text = "Next Connection";
            this.nextConnectionButton.UseVisualStyleBackColor = true;
            this.nextConnectionButton.Click += new System.EventHandler(this.nextConnectionButton_Click);
            // 
            // stopScanningButton
            // 
            this.stopScanningButton.Location = new System.Drawing.Point(192, 202);
            this.stopScanningButton.Name = "stopScanningButton";
            this.stopScanningButton.Size = new System.Drawing.Size(75, 37);
            this.stopScanningButton.TabIndex = 3;
            this.stopScanningButton.Text = "Stop Scanning";
            this.stopScanningButton.UseVisualStyleBackColor = true;
            this.stopScanningButton.Click += new System.EventHandler(this.stopScanningButton_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.startScanningButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.stopScanningButton);
            this.Controls.Add(this.nextConnectionButton);
            this.Controls.Add(this.startScanningButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Software Scanning";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.ComboBox topologyNameComboBox;
        private System.Windows.Forms.Label topologyNameLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.Label scanListLabel;
        private System.Windows.Forms.TextBox scanListTextBox;
        private System.Windows.Forms.Button startScanningButton;
        private System.Windows.Forms.Button nextConnectionButton;
        private System.Windows.Forms.Button stopScanningButton;

    }
}