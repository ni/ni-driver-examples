namespace NationalInstruments.Examples.GenerateMultiDeviceInterleavedScanlistPxi2584
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
            this.interleavedStartChannelNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.deviceScanConfigurationGroupBox = new System.Windows.Forms.GroupBox();
            this.interleavedEndChannelLabel = new System.Windows.Forms.Label();
            this.interleavedStartChannelLabel = new System.Windows.Forms.Label();
            this.interleavedEndChannelNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.interleavedScanListRichTextBox = new System.Windows.Forms.RichTextBox();
            this.interleavedScanListLabel = new System.Windows.Forms.Label();
            this.generateButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.interleavedStartChannelNumericUpDown)).BeginInit();
            this.deviceScanConfigurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.interleavedEndChannelNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // interleavedStartChannelNumericUpDown
            // 
            this.interleavedStartChannelNumericUpDown.Location = new System.Drawing.Point(50, 35);
            this.interleavedStartChannelNumericUpDown.Name = "interleavedStartChannelNumericUpDown";
            this.interleavedStartChannelNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.interleavedStartChannelNumericUpDown.TabIndex = 0;
            // 
            // deviceScanConfigurationGroupBox
            // 
            this.deviceScanConfigurationGroupBox.Controls.Add(this.interleavedEndChannelLabel);
            this.deviceScanConfigurationGroupBox.Controls.Add(this.interleavedStartChannelLabel);
            this.deviceScanConfigurationGroupBox.Controls.Add(this.interleavedEndChannelNumericUpDown);
            this.deviceScanConfigurationGroupBox.Controls.Add(this.interleavedStartChannelNumericUpDown);
            this.deviceScanConfigurationGroupBox.Location = new System.Drawing.Point(12, 23);
            this.deviceScanConfigurationGroupBox.Name = "deviceScanConfigurationGroupBox";
            this.deviceScanConfigurationGroupBox.Size = new System.Drawing.Size(395, 71);
            this.deviceScanConfigurationGroupBox.TabIndex = 0;
            this.deviceScanConfigurationGroupBox.TabStop = false;
            this.deviceScanConfigurationGroupBox.Text = "Device Scan Configuration";
            // 
            // interleavedEndChannelLabel
            // 
            this.interleavedEndChannelLabel.AutoSize = true;
            this.interleavedEndChannelLabel.Location = new System.Drawing.Point(246, 16);
            this.interleavedEndChannelLabel.Name = "interleavedEndChannelLabel";
            this.interleavedEndChannelLabel.Size = new System.Drawing.Size(124, 13);
            this.interleavedEndChannelLabel.TabIndex = 3;
            this.interleavedEndChannelLabel.Text = "Interleaved End Channel";
            // 
            // interleavedStartChannelLabel
            // 
            this.interleavedStartChannelLabel.AutoSize = true;
            this.interleavedStartChannelLabel.Location = new System.Drawing.Point(43, 19);
            this.interleavedStartChannelLabel.Name = "interleavedStartChannelLabel";
            this.interleavedStartChannelLabel.Size = new System.Drawing.Size(127, 13);
            this.interleavedStartChannelLabel.TabIndex = 2;
            this.interleavedStartChannelLabel.Text = "Interleaved Start Channel";
            // 
            // interleavedEndChannelNumericUpDown
            // 
            this.interleavedEndChannelNumericUpDown.Location = new System.Drawing.Point(250, 35);
            this.interleavedEndChannelNumericUpDown.Name = "interleavedEndChannelNumericUpDown";
            this.interleavedEndChannelNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.interleavedEndChannelNumericUpDown.TabIndex = 1;
            // 
            // interleavedScanListRichTextBox
            // 
            this.interleavedScanListRichTextBox.Location = new System.Drawing.Point(12, 126);
            this.interleavedScanListRichTextBox.Name = "interleavedScanListRichTextBox";
            this.interleavedScanListRichTextBox.ReadOnly = true;
            this.interleavedScanListRichTextBox.Size = new System.Drawing.Size(247, 52);
            this.interleavedScanListRichTextBox.TabIndex = 1;
            this.interleavedScanListRichTextBox.Text = "";
            // 
            // interleavedScanListLabel
            // 
            this.interleavedScanListLabel.AutoSize = true;
            this.interleavedScanListLabel.Location = new System.Drawing.Point(12, 110);
            this.interleavedScanListLabel.Name = "interleavedScanListLabel";
            this.interleavedScanListLabel.Size = new System.Drawing.Size(107, 13);
            this.interleavedScanListLabel.TabIndex = 2;
            this.interleavedScanListLabel.Text = "Interleaved Scan List";
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(283, 126);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(110, 52);
            this.generateButton.TabIndex = 3;
            this.generateButton.Text = "Generate Interleaved Scanlist";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.generateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 207);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.interleavedScanListLabel);
            this.Controls.Add(this.interleavedScanListRichTextBox);
            this.Controls.Add(this.deviceScanConfigurationGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Generate Multi Device Interleaved Scanlist PXI2584";
            ((System.ComponentModel.ISupportInitialize)(this.interleavedStartChannelNumericUpDown)).EndInit();
            this.deviceScanConfigurationGroupBox.ResumeLayout(false);
            this.deviceScanConfigurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.interleavedEndChannelNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown interleavedStartChannelNumericUpDown;
        private System.Windows.Forms.GroupBox deviceScanConfigurationGroupBox;
        private System.Windows.Forms.NumericUpDown interleavedEndChannelNumericUpDown;
        private System.Windows.Forms.Label interleavedEndChannelLabel;
        private System.Windows.Forms.Label interleavedStartChannelLabel;
        private System.Windows.Forms.RichTextBox interleavedScanListRichTextBox;
        private System.Windows.Forms.Label interleavedScanListLabel;
        private System.Windows.Forms.Button generateButton;

    }
}

