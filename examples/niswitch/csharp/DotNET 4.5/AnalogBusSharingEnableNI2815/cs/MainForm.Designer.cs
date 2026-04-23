namespace NationalInstruments.Examples.AnalogBusSharingEnableNI2815
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
            this.resourceName1ComboBox = new System.Windows.Forms.ComboBox();
            this.resourceName1Label = new System.Windows.Forms.Label();
            this.resourceName2ComboBox = new System.Windows.Forms.ComboBox();
            this.resourceName2Label = new System.Windows.Forms.Label();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.columnChannel1ComboBox = new System.Windows.Forms.ComboBox();
            this.columnChannel1Label = new System.Windows.Forms.Label();
            this.topologyName1ComboBox = new System.Windows.Forms.ComboBox();
            this.topologyName1Label = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.columnChannel2ComboBox = new System.Windows.Forms.ComboBox();
            this.columnChannel2Label = new System.Windows.Forms.Label();
            this.topologyName2ComboBox = new System.Windows.Forms.ComboBox();
            this.topologyName2Label = new System.Windows.Forms.Label();
            this.analogBusChannelLabel = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.analogBusChannelsComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // resourceName1ComboBox
            // 
            this.resourceName1ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceName1ComboBox.FormattingEnabled = true;
            this.resourceName1ComboBox.Location = new System.Drawing.Point(24, 33);
            this.resourceName1ComboBox.Name = "resourceName1ComboBox";
            this.resourceName1ComboBox.Size = new System.Drawing.Size(121, 21);
            this.resourceName1ComboBox.TabIndex = 0;
            // 
            // resourceName1Label
            // 
            this.resourceName1Label.AutoSize = true;
            this.resourceName1Label.Location = new System.Drawing.Point(21, 16);
            this.resourceName1Label.Name = "resourceName1Label";
            this.resourceName1Label.Size = new System.Drawing.Size(93, 13);
            this.resourceName1Label.TabIndex = 1;
            this.resourceName1Label.Text = "Resource Name 1";
            // 
            // resourceName2ComboBox
            // 
            this.resourceName2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceName2ComboBox.FormattingEnabled = true;
            this.resourceName2ComboBox.Location = new System.Drawing.Point(28, 33);
            this.resourceName2ComboBox.Name = "resourceName2ComboBox";
            this.resourceName2ComboBox.Size = new System.Drawing.Size(121, 21);
            this.resourceName2ComboBox.TabIndex = 0;
            // 
            // resourceName2Label
            // 
            this.resourceName2Label.AutoSize = true;
            this.resourceName2Label.Location = new System.Drawing.Point(25, 16);
            this.resourceName2Label.Name = "resourceName2Label";
            this.resourceName2Label.Size = new System.Drawing.Size(93, 13);
            this.resourceName2Label.TabIndex = 3;
            this.resourceName2Label.Text = "Resource Name 2";
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.columnChannel1ComboBox);
            this.groupBox.Controls.Add(this.columnChannel1Label);
            this.groupBox.Controls.Add(this.topologyName1ComboBox);
            this.groupBox.Controls.Add(this.topologyName1Label);
            this.groupBox.Controls.Add(this.resourceName1ComboBox);
            this.groupBox.Controls.Add(this.resourceName1Label);
            this.groupBox.Location = new System.Drawing.Point(15, 25);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(175, 202);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Switch Resource 1";
            // 
            // columnChannel1ComboBox
            // 
            this.columnChannel1ComboBox.FormattingEnabled = true;
            this.columnChannel1ComboBox.Location = new System.Drawing.Point(24, 145);
            this.columnChannel1ComboBox.Name = "columnChannel1ComboBox";
            this.columnChannel1ComboBox.Size = new System.Drawing.Size(100, 21);
            this.columnChannel1ComboBox.TabIndex = 2;
            // 
            // columnChannel1Label
            // 
            this.columnChannel1Label.AutoSize = true;
            this.columnChannel1Label.Location = new System.Drawing.Point(21, 129);
            this.columnChannel1Label.Name = "columnChannel1Label";
            this.columnChannel1Label.Size = new System.Drawing.Size(93, 13);
            this.columnChannel1Label.TabIndex = 4;
            this.columnChannel1Label.Text = "Column Channel 1";
            // 
            // topologyName1ComboBox
            // 
            this.topologyName1ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.topologyName1ComboBox.FormattingEnabled = true;
            this.topologyName1ComboBox.Location = new System.Drawing.Point(24, 91);
            this.topologyName1ComboBox.Name = "topologyName1ComboBox";
            this.topologyName1ComboBox.Size = new System.Drawing.Size(121, 21);
            this.topologyName1ComboBox.TabIndex = 1;
            // 
            // topologyName1Label
            // 
            this.topologyName1Label.AutoSize = true;
            this.topologyName1Label.Location = new System.Drawing.Point(21, 75);
            this.topologyName1Label.Name = "topologyName1Label";
            this.topologyName1Label.Size = new System.Drawing.Size(82, 13);
            this.topologyName1Label.TabIndex = 2;
            this.topologyName1Label.Text = "Topology Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.columnChannel2ComboBox);
            this.groupBox2.Controls.Add(this.columnChannel2Label);
            this.groupBox2.Controls.Add(this.topologyName2ComboBox);
            this.groupBox2.Controls.Add(this.topologyName2Label);
            this.groupBox2.Controls.Add(this.resourceName2ComboBox);
            this.groupBox2.Controls.Add(this.resourceName2Label);
            this.groupBox2.Location = new System.Drawing.Point(224, 25);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(202, 202);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Switch Resource 2";
            // 
            // columnChannel2ComboBox
            // 
            this.columnChannel2ComboBox.FormattingEnabled = true;
            this.columnChannel2ComboBox.Location = new System.Drawing.Point(28, 145);
            this.columnChannel2ComboBox.Name = "columnChannel2ComboBox";
            this.columnChannel2ComboBox.Size = new System.Drawing.Size(100, 21);
            this.columnChannel2ComboBox.TabIndex = 2;
            // 
            // columnChannel2Label
            // 
            this.columnChannel2Label.AutoSize = true;
            this.columnChannel2Label.Location = new System.Drawing.Point(25, 129);
            this.columnChannel2Label.Name = "columnChannel2Label";
            this.columnChannel2Label.Size = new System.Drawing.Size(93, 13);
            this.columnChannel2Label.TabIndex = 4;
            this.columnChannel2Label.Text = "Column Channel 2";
            // 
            // topologyName2ComboBox
            // 
            this.topologyName2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.topologyName2ComboBox.FormattingEnabled = true;
            this.topologyName2ComboBox.Location = new System.Drawing.Point(28, 91);
            this.topologyName2ComboBox.Name = "topologyName2ComboBox";
            this.topologyName2ComboBox.Size = new System.Drawing.Size(121, 21);
            this.topologyName2ComboBox.TabIndex = 1;
            // 
            // topologyName2Label
            // 
            this.topologyName2Label.AutoSize = true;
            this.topologyName2Label.Location = new System.Drawing.Point(25, 75);
            this.topologyName2Label.Name = "topologyName2Label";
            this.topologyName2Label.Size = new System.Drawing.Size(82, 13);
            this.topologyName2Label.TabIndex = 4;
            this.topologyName2Label.Text = "Topology Name";
            // 
            // analogBusChannelLabel
            // 
            this.analogBusChannelLabel.AutoSize = true;
            this.analogBusChannelLabel.Location = new System.Drawing.Point(441, 41);
            this.analogBusChannelLabel.Name = "analogBusChannelLabel";
            this.analogBusChannelLabel.Size = new System.Drawing.Size(103, 13);
            this.analogBusChannelLabel.TabIndex = 2;
            this.analogBusChannelLabel.Text = "Analog Bus Channel";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(135, 257);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(256, 257);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(75, 23);
            this.disconnectButton.TabIndex = 4;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // analogBusChannelsComboBox
            // 
            this.analogBusChannelsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.analogBusChannelsComboBox.FormattingEnabled = true;
            this.analogBusChannelsComboBox.Location = new System.Drawing.Point(444, 58);
            this.analogBusChannelsComboBox.Name = "analogBusChannelsComboBox";
            this.analogBusChannelsComboBox.Size = new System.Drawing.Size(100, 21);
            this.analogBusChannelsComboBox.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AcceptButton = this.connectButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 292);
            this.Controls.Add(this.analogBusChannelsComboBox);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.analogBusChannelLabel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Analog Bus Sharing Enable NI2815";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox resourceName1ComboBox;
        private System.Windows.Forms.Label resourceName1Label;
        private System.Windows.Forms.ComboBox resourceName2ComboBox;
        private System.Windows.Forms.Label resourceName2Label;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label topologyName1Label;
        private System.Windows.Forms.ComboBox topologyName1ComboBox;
        private System.Windows.Forms.ComboBox topologyName2ComboBox;
        private System.Windows.Forms.Label topologyName2Label;
        private System.Windows.Forms.Label analogBusChannelLabel;
        private System.Windows.Forms.Label columnChannel1Label;
        private System.Windows.Forms.Label columnChannel2Label;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.ComboBox analogBusChannelsComboBox;
        private System.Windows.Forms.ComboBox columnChannel1ComboBox;
        private System.Windows.Forms.ComboBox columnChannel2ComboBox;
    }
}

