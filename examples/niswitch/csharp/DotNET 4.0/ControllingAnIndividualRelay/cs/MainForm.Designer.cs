namespace NationalInstruments.Examples.ControllingAnIndividualRelay
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
            this.openRelayButton = new System.Windows.Forms.Button();
            this.closeRelayButton = new System.Windows.Forms.Button();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.relayNameTextBox = new System.Windows.Forms.TextBox();
            this.topologyNameComboBox = new System.Windows.Forms.ComboBox();
            this.topologyNameLabel = new System.Windows.Forms.Label();
            this.relayNameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // openRelayButton
            // 
            this.openRelayButton.Location = new System.Drawing.Point(186, 57);
            this.openRelayButton.Name = "openRelayButton";
            this.openRelayButton.Size = new System.Drawing.Size(75, 23);
            this.openRelayButton.TabIndex = 3;
            this.openRelayButton.Text = "Open Relay";
            this.openRelayButton.UseVisualStyleBackColor = true;
            this.openRelayButton.Click += new System.EventHandler(this.openRelayButton_Click);
            // 
            // closeRelayButton
            // 
            this.closeRelayButton.Location = new System.Drawing.Point(186, 110);
            this.closeRelayButton.Name = "closeRelayButton";
            this.closeRelayButton.Size = new System.Drawing.Size(75, 23);
            this.closeRelayButton.TabIndex = 4;
            this.closeRelayButton.Text = "Close Relay";
            this.closeRelayButton.UseVisualStyleBackColor = true;
            this.closeRelayButton.Click += new System.EventHandler(this.closeRelayButton_Click);
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(12, 28);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.resourceNameComboBox.TabIndex = 0;
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(12, 12);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(84, 13);
            this.resourceNameLabel.TabIndex = 5;
            this.resourceNameLabel.Text = "Resource Name";
            // 
            // relayNameTextBox
            // 
            this.relayNameTextBox.Location = new System.Drawing.Point(15, 166);
            this.relayNameTextBox.Name = "relayNameTextBox";
            this.relayNameTextBox.Size = new System.Drawing.Size(121, 20);
            this.relayNameTextBox.TabIndex = 2;
            // 
            // topologyNameComboBox
            // 
            this.topologyNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.topologyNameComboBox.FormattingEnabled = true;
            this.topologyNameComboBox.Location = new System.Drawing.Point(12, 93);
            this.topologyNameComboBox.Name = "topologyNameComboBox";
            this.topologyNameComboBox.Size = new System.Drawing.Size(121, 21);
            this.topologyNameComboBox.TabIndex = 1;
            // 
            // topologyNameLabel
            // 
            this.topologyNameLabel.AutoSize = true;
            this.topologyNameLabel.Location = new System.Drawing.Point(12, 77);
            this.topologyNameLabel.Name = "topologyNameLabel";
            this.topologyNameLabel.Size = new System.Drawing.Size(82, 13);
            this.topologyNameLabel.TabIndex = 6;
            this.topologyNameLabel.Text = "Topology Name";
            // 
            // relayNameLabel
            // 
            this.relayNameLabel.AutoSize = true;
            this.relayNameLabel.Location = new System.Drawing.Point(12, 141);
            this.relayNameLabel.Name = "relayNameLabel";
            this.relayNameLabel.Size = new System.Drawing.Size(65, 13);
            this.relayNameLabel.TabIndex = 7;
            this.relayNameLabel.Text = "Relay Name";
            // 
            // MainForm
            // 
            this.AcceptButton = this.openRelayButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 230);
            this.Controls.Add(this.relayNameLabel);
            this.Controls.Add(this.topologyNameLabel);
            this.Controls.Add(this.topologyNameComboBox);
            this.Controls.Add(this.relayNameTextBox);
            this.Controls.Add(this.resourceNameLabel);
            this.Controls.Add(this.resourceNameComboBox);
            this.Controls.Add(this.closeRelayButton);
            this.Controls.Add(this.openRelayButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Controlling An Individual Relay";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openRelayButton;
        private System.Windows.Forms.Button closeRelayButton;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.TextBox relayNameTextBox;
        private System.Windows.Forms.ComboBox topologyNameComboBox;
        private System.Windows.Forms.Label topologyNameLabel;
        private System.Windows.Forms.Label relayNameLabel;
    }
}

