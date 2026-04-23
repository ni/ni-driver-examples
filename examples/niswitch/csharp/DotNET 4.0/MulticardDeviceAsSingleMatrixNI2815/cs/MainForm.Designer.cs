namespace NationalInstruments.Examples.MulticardDeviceAsSingleMatrixNI2815
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
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.channel2TextBox = new System.Windows.Forms.TextBox();
            this.channel1TextBox = new System.Windows.Forms.TextBox();
            this.channel2Label = new System.Windows.Forms.Label();
            this.channel1Label = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.topologyNameLabel = new System.Windows.Forms.Label();
            this.topologyNameComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.noteTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(12, 28);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 5;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // channel2TextBox
            // 
            this.channel2TextBox.Location = new System.Drawing.Point(67, 61);
            this.channel2TextBox.Name = "channel2TextBox";
            this.channel2TextBox.Size = new System.Drawing.Size(100, 20);
            this.channel2TextBox.TabIndex = 1;
            this.channel2TextBox.Text = "c0";
            // 
            // channel1TextBox
            // 
            this.channel1TextBox.Location = new System.Drawing.Point(67, 23);
            this.channel1TextBox.Name = "channel1TextBox";
            this.channel1TextBox.Size = new System.Drawing.Size(100, 20);
            this.channel1TextBox.TabIndex = 0;
            this.channel1TextBox.Text = "ab0";
            // 
            // channel2Label
            // 
            this.channel2Label.AutoSize = true;
            this.channel2Label.Location = new System.Drawing.Point(6, 64);
            this.channel2Label.Name = "channel2Label";
            this.channel2Label.Size = new System.Drawing.Size(55, 13);
            this.channel2Label.TabIndex = 3;
            this.channel2Label.Text = "Channel 2";
            // 
            // channel1Label
            // 
            this.channel1Label.AutoSize = true;
            this.channel1Label.Location = new System.Drawing.Point(6, 26);
            this.channel1Label.Name = "channel1Label";
            this.channel1Label.Size = new System.Drawing.Size(55, 13);
            this.channel1Label.TabIndex = 2;
            this.channel1Label.Text = "Channel 1";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(94, 227);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // topologyNameLabel
            // 
            this.topologyNameLabel.AutoSize = true;
            this.topologyNameLabel.Location = new System.Drawing.Point(12, 71);
            this.topologyNameLabel.Name = "topologyNameLabel";
            this.topologyNameLabel.Size = new System.Drawing.Size(82, 13);
            this.topologyNameLabel.TabIndex = 6;
            this.topologyNameLabel.Text = "Topology Name";
            // 
            // topologyNameComboBox
            // 
            this.topologyNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.topologyNameComboBox.FormattingEnabled = true;
            this.topologyNameComboBox.Location = new System.Drawing.Point(12, 87);
            this.topologyNameComboBox.Name = "topologyNameComboBox";
            this.topologyNameComboBox.Size = new System.Drawing.Size(143, 21);
            this.topologyNameComboBox.TabIndex = 1;
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 44);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(143, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.channel1Label);
            this.groupBox1.Controls.Add(this.channel1TextBox);
            this.groupBox1.Controls.Add(this.channel2TextBox);
            this.groupBox1.Controls.Add(this.channel2Label);
            this.groupBox1.Location = new System.Drawing.Point(15, 121);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(174, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // noteTextBox
            // 
            this.noteTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.noteTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noteTextBox.Location = new System.Drawing.Point(15, 282);
            this.noteTextBox.Name = "noteTextBox";
            this.noteTextBox.ReadOnly = true;
            this.noteTextBox.Size = new System.Drawing.Size(197, 15);
            this.noteTextBox.TabIndex = 4;
            this.noteTextBox.TabStop = false;
            this.noteTextBox.Text = "This example works with NI 2815.";
            // 
            // MainForm
            // 
            this.AcceptButton = this.connectButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 329);
            this.Controls.Add(this.noteTextBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.topologyNameLabel);
            this.Controls.Add(this.topologyNameComboBox);
            this.Controls.Add(this.resourceNameComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Multicard Device As Single Matrix NI2815";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.TextBox channel2TextBox;
        private System.Windows.Forms.TextBox channel1TextBox;
        private System.Windows.Forms.Label channel2Label;
        private System.Windows.Forms.Label channel1Label;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label topologyNameLabel;
        private System.Windows.Forms.ComboBox topologyNameComboBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox noteTextBox;
    }
}