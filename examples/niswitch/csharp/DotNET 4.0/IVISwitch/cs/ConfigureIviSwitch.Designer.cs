namespace NationalInstruments.Examples.IviSwitch
{
    partial class ConfigureIviSwitch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureIviSwitch));
            this.closeButton = new System.Windows.Forms.Button();
            this.runPanelButton = new System.Windows.Forms.Button();
            this.switchPathTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.resultActionPanel = new System.Windows.Forms.Panel();
            this.canConnectRichTextBox = new System.Windows.Forms.RichTextBox();
            this.selectActionComboBox = new System.Windows.Forms.ComboBox();
            this.channel2ComboBox = new System.Windows.Forms.ComboBox();
            this.channel1ComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(227, 273);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 27;
            this.closeButton.Text = "Close/Quit";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // runPanelButton
            // 
            this.runPanelButton.Location = new System.Drawing.Point(16, 273);
            this.runPanelButton.Name = "runPanelButton";
            this.runPanelButton.Size = new System.Drawing.Size(75, 23);
            this.runPanelButton.TabIndex = 26;
            this.runPanelButton.Text = "Run Panel";
            this.runPanelButton.UseVisualStyleBackColor = true;
            this.runPanelButton.Click += new System.EventHandler(this.runPanelButton_Click);
            // 
            // switchPathTextBox
            // 
            this.switchPathTextBox.Location = new System.Drawing.Point(19, 226);
            this.switchPathTextBox.Name = "switchPathTextBox";
            this.switchPathTextBox.Size = new System.Drawing.Size(283, 20);
            this.switchPathTextBox.TabIndex = 25;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 210);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Switch Path";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(175, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Can Connect/Disconnect/Get Path";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(162, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Result Of Action";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(162, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "-->Connect From Channel # ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Connect From Channel # -->";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Select Action";
            // 
            // resultActionPanel
            // 
            this.resultActionPanel.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.resultActionPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.resultActionPanel.Location = new System.Drawing.Point(176, 30);
            this.resultActionPanel.Name = "resultActionPanel";
            this.resultActionPanel.Size = new System.Drawing.Size(126, 21);
            this.resultActionPanel.TabIndex = 18;
            // 
            // canConnectRichTextBox
            // 
            this.canConnectRichTextBox.Location = new System.Drawing.Point(16, 144);
            this.canConnectRichTextBox.Name = "canConnectRichTextBox";
            this.canConnectRichTextBox.Size = new System.Drawing.Size(286, 63);
            this.canConnectRichTextBox.TabIndex = 17;
            this.canConnectRichTextBox.Text = "";
            // 
            // selectActionComboBox
            // 
            this.selectActionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectActionComboBox.FormattingEnabled = true;
            this.selectActionComboBox.Location = new System.Drawing.Point(16, 30);
            this.selectActionComboBox.Name = "selectActionComboBox";
            this.selectActionComboBox.Size = new System.Drawing.Size(121, 21);
            this.selectActionComboBox.TabIndex = 16;
            // 
            // channel2ComboBox
            // 
            this.channel2ComboBox.FormattingEnabled = true;
            this.channel2ComboBox.Location = new System.Drawing.Point(176, 89);
            this.channel2ComboBox.Name = "channel2ComboBox";
            this.channel2ComboBox.Size = new System.Drawing.Size(126, 21);
            this.channel2ComboBox.TabIndex = 15;
            // 
            // channel1ComboBox
            // 
            this.channel1ComboBox.FormattingEnabled = true;
            this.channel1ComboBox.Location = new System.Drawing.Point(16, 89);
            this.channel1ComboBox.Name = "channel1ComboBox";
            this.channel1ComboBox.Size = new System.Drawing.Size(121, 21);
            this.channel1ComboBox.TabIndex = 14;
            // 
            // ConfigureIviSwitch
            // 
            this.AcceptButton = this.runPanelButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(318, 311);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.runPanelButton);
            this.Controls.Add(this.switchPathTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resultActionPanel);
            this.Controls.Add(this.canConnectRichTextBox);
            this.Controls.Add(this.selectActionComboBox);
            this.Controls.Add(this.channel2ComboBox);
            this.Controls.Add(this.channel1ComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConfigureIviSwitch";
            this.Text = "ConfigureIviSwitch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button runPanelButton;
        private System.Windows.Forms.TextBox switchPathTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel resultActionPanel;
        private System.Windows.Forms.RichTextBox canConnectRichTextBox;
        private System.Windows.Forms.ComboBox selectActionComboBox;
        private System.Windows.Forms.ComboBox channel2ComboBox;
        private System.Windows.Forms.ComboBox channel1ComboBox;
    }
}