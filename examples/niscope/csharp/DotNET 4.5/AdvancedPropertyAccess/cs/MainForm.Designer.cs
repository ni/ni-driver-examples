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
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.sampledDataGridView = new System.Windows.Forms.DataGridView();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.sampledDataGroupBox = new System.Windows.Forms.GroupBox();
            this.actualSampleRateTextBox = new System.Windows.Forms.TextBox();
            this.actualSampleRateLabel = new System.Windows.Forms.Label();
            this.actualRecordLengthTextBox = new System.Windows.Forms.TextBox();
            this.actualRecordLengthLabel = new System.Windows.Forms.Label();
            this.acquireButton = new System.Windows.Forms.Button();
            this.buttonsGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.sampledDataGridView)).BeginInit();
            this.generalGroupBox.SuspendLayout();
            this.sampledDataGroupBox.SuspendLayout();
            this.buttonsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelNameLabel
            // 
            this.channelNameLabel.AutoSize = true;
            this.channelNameLabel.Location = new System.Drawing.Point(6, 50);
            this.channelNameLabel.Name = "channelNameLabel";
            this.channelNameLabel.Size = new System.Drawing.Size(80, 13);
            this.channelNameLabel.TabIndex = 2;
            this.channelNameLabel.Text = "Channel Name:";
            // 
            // resourceNameLabel
            // 
            this.resourceNameLabel.AutoSize = true;
            this.resourceNameLabel.Location = new System.Drawing.Point(6, 23);
            this.resourceNameLabel.Name = "resourceNameLabel";
            this.resourceNameLabel.Size = new System.Drawing.Size(87, 13);
            this.resourceNameLabel.TabIndex = 0;
            this.resourceNameLabel.Text = "Resource Name:";
            // 
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(124, 46);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(114, 20);
            this.channelNameTextBox.TabIndex = 3;
            this.channelNameTextBox.Text = "0";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(124, 19);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(114, 21);
            this.resourceNameComboBox.TabIndex = 1;
            // 
            // sampledDataGridView
            // 
            this.sampledDataGridView.AllowUserToAddRows = false;
            this.sampledDataGridView.AllowUserToDeleteRows = false;
            this.sampledDataGridView.AllowUserToResizeRows = false;
            this.sampledDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sampledDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sampledDataGridView.Location = new System.Drawing.Point(6, 71);
            this.sampledDataGridView.Name = "sampledDataGridView";
            this.sampledDataGridView.ReadOnly = true;
            this.sampledDataGridView.RowHeadersVisible = false;
            this.sampledDataGridView.Size = new System.Drawing.Size(205, 254);
            this.sampledDataGridView.StandardTab = true;
            this.sampledDataGridView.TabIndex = 4;
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.resourceNameLabel);
            this.generalGroupBox.Controls.Add(this.resourceNameComboBox);
            this.generalGroupBox.Controls.Add(this.channelNameLabel);
            this.generalGroupBox.Controls.Add(this.channelNameTextBox);
            this.generalGroupBox.Location = new System.Drawing.Point(12, 12);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(245, 72);
            this.generalGroupBox.TabIndex = 0;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            // 
            // sampledDataGroupBox
            // 
            this.sampledDataGroupBox.Controls.Add(this.actualSampleRateTextBox);
            this.sampledDataGroupBox.Controls.Add(this.actualSampleRateLabel);
            this.sampledDataGroupBox.Controls.Add(this.actualRecordLengthTextBox);
            this.sampledDataGroupBox.Controls.Add(this.actualRecordLengthLabel);
            this.sampledDataGroupBox.Controls.Add(this.sampledDataGridView);
            this.sampledDataGroupBox.Location = new System.Drawing.Point(12, 100);
            this.sampledDataGroupBox.Name = "sampledDataGroupBox";
            this.sampledDataGroupBox.Size = new System.Drawing.Size(245, 331);
            this.sampledDataGroupBox.TabIndex = 1;
            this.sampledDataGroupBox.TabStop = false;
            this.sampledDataGroupBox.Text = "Sampled Data";
            // 
            // actualSampleRateTextBox
            // 
            this.actualSampleRateTextBox.Location = new System.Drawing.Point(124, 45);
            this.actualSampleRateTextBox.Name = "actualSampleRateTextBox";
            this.actualSampleRateTextBox.ReadOnly = true;
            this.actualSampleRateTextBox.Size = new System.Drawing.Size(114, 20);
            this.actualSampleRateTextBox.TabIndex = 3;
            this.actualSampleRateTextBox.Text = "0.0";
            // 
            // actualSampleRateLabel
            // 
            this.actualSampleRateLabel.AutoSize = true;
            this.actualSampleRateLabel.Location = new System.Drawing.Point(6, 49);
            this.actualSampleRateLabel.Name = "actualSampleRateLabel";
            this.actualSampleRateLabel.Size = new System.Drawing.Size(104, 13);
            this.actualSampleRateLabel.TabIndex = 2;
            this.actualSampleRateLabel.Text = "Actual Sample Rate:";
            // 
            // actualRecordLengthTextBox
            // 
            this.actualRecordLengthTextBox.Location = new System.Drawing.Point(124, 19);
            this.actualRecordLengthTextBox.Name = "actualRecordLengthTextBox";
            this.actualRecordLengthTextBox.ReadOnly = true;
            this.actualRecordLengthTextBox.Size = new System.Drawing.Size(114, 20);
            this.actualRecordLengthTextBox.TabIndex = 1;
            this.actualRecordLengthTextBox.Text = "0";
            // 
            // actualRecordLengthLabel
            // 
            this.actualRecordLengthLabel.AutoSize = true;
            this.actualRecordLengthLabel.Location = new System.Drawing.Point(6, 23);
            this.actualRecordLengthLabel.Name = "actualRecordLengthLabel";
            this.actualRecordLengthLabel.Size = new System.Drawing.Size(114, 13);
            this.actualRecordLengthLabel.TabIndex = 0;
            this.actualRecordLengthLabel.Text = "Actual Record Length:";
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(84, 19);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // buttonsGroupBox
            // 
            this.buttonsGroupBox.Controls.Add(this.acquireButton);
            this.buttonsGroupBox.Location = new System.Drawing.Point(12, 437);
            this.buttonsGroupBox.Name = "buttonsGroupBox";
            this.buttonsGroupBox.Size = new System.Drawing.Size(245, 58);
            this.buttonsGroupBox.TabIndex = 2;
            this.buttonsGroupBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 508);
            this.Controls.Add(this.buttonsGroupBox);
            this.Controls.Add(this.sampledDataGroupBox);
            this.Controls.Add(this.generalGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Advanced Property Access";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.sampledDataGridView)).EndInit();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.sampledDataGroupBox.ResumeLayout(false);
            this.sampledDataGroupBox.PerformLayout();
            this.buttonsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.DataGridView sampledDataGridView;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.GroupBox sampledDataGroupBox;
        private System.Windows.Forms.TextBox actualSampleRateTextBox;
        private System.Windows.Forms.Label actualSampleRateLabel;
        private System.Windows.Forms.TextBox actualRecordLengthTextBox;
        private System.Windows.Forms.Label actualRecordLengthLabel;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.GroupBox buttonsGroupBox;


    }
}
