namespace NationalInstruments.Examples.ExportImportConfiguration
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
            this.buttonsGroupBox = new System.Windows.Forms.GroupBox();
            this.acquireButton = new System.Windows.Forms.Button();
            this.actualSampleRateTextBox = new System.Windows.Forms.TextBox();
            this.actualSampleRateLabel = new System.Windows.Forms.Label();
            this.actualRecordLengthTextBox = new System.Windows.Forms.TextBox();
            this.actualRecordLengthLabel = new System.Windows.Forms.Label();
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.sampledDataGroupBox = new System.Windows.Forms.GroupBox();
            this.sampledDataGridView = new System.Windows.Forms.DataGridView();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.exportButton = new System.Windows.Forms.Button();
            this.inputImpedanceLabel = new System.Windows.Forms.Label();
            this.performAutoSetupButton = new System.Windows.Forms.Button();
            this.importButton = new System.Windows.Forms.Button();
            this.minSampleRateTextBox = new System.Windows.Forms.TextBox();
            this.minSampleRateLabel = new System.Windows.Forms.Label();
            this.verticalCouplingComboBox = new System.Windows.Forms.ComboBox();
            this.inputImpedanceComboBox = new System.Windows.Forms.ComboBox();
            this.verticalCouplingLabel = new System.Windows.Forms.Label();
            this.verticalOffsetToolbox = new System.Windows.Forms.TextBox();
            this.verticalOffsetLabel = new System.Windows.Forms.Label();
            this.verticalRangeToolbox = new System.Windows.Forms.TextBox();
            this.verticalRangeLabel = new System.Windows.Forms.Label();
            this.buttonsGroupBox.SuspendLayout();
            this.sampledDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sampledDataGridView)).BeginInit();
            this.generalGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonsGroupBox
            // 
            this.buttonsGroupBox.Controls.Add(this.acquireButton);
            this.buttonsGroupBox.Location = new System.Drawing.Point(264, 293);
            this.buttonsGroupBox.Name = "buttonsGroupBox";
            this.buttonsGroupBox.Size = new System.Drawing.Size(245, 44);
            this.buttonsGroupBox.TabIndex = 2;
            this.buttonsGroupBox.TabStop = false;
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(83, 14);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
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
            // channelNameLabel
            // 
            this.channelNameLabel.AutoSize = true;
            this.channelNameLabel.Location = new System.Drawing.Point(6, 50);
            this.channelNameLabel.Name = "channelNameLabel";
            this.channelNameLabel.Size = new System.Drawing.Size(80, 13);
            this.channelNameLabel.TabIndex = 2;
            this.channelNameLabel.Text = "Channel Name:";
            // 
            // sampledDataGroupBox
            // 
            this.sampledDataGroupBox.Controls.Add(this.actualSampleRateTextBox);
            this.sampledDataGroupBox.Controls.Add(this.actualSampleRateLabel);
            this.sampledDataGroupBox.Controls.Add(this.actualRecordLengthTextBox);
            this.sampledDataGroupBox.Controls.Add(this.actualRecordLengthLabel);
            this.sampledDataGroupBox.Controls.Add(this.sampledDataGridView);
            this.sampledDataGroupBox.Location = new System.Drawing.Point(264, 12);
            this.sampledDataGroupBox.Name = "sampledDataGroupBox";
            this.sampledDataGroupBox.Size = new System.Drawing.Size(245, 275);
            this.sampledDataGroupBox.TabIndex = 1;
            this.sampledDataGroupBox.TabStop = false;
            this.sampledDataGroupBox.Text = "Sampled Data";
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
            this.sampledDataGridView.Location = new System.Drawing.Point(19, 71);
            this.sampledDataGridView.Name = "sampledDataGridView";
            this.sampledDataGridView.ReadOnly = true;
            this.sampledDataGridView.RowHeadersVisible = false;
            this.sampledDataGridView.Size = new System.Drawing.Size(205, 193);
            this.sampledDataGridView.StandardTab = true;
            this.sampledDataGridView.TabIndex = 4;
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
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.resourceNameLabel);
            this.generalGroupBox.Controls.Add(this.resourceNameComboBox);
            this.generalGroupBox.Controls.Add(this.channelNameLabel);
            this.generalGroupBox.Controls.Add(this.channelNameTextBox);
            this.generalGroupBox.Location = new System.Drawing.Point(13, 12);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(245, 72);
            this.generalGroupBox.TabIndex = 0;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
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
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(124, 46);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(114, 20);
            this.channelNameTextBox.TabIndex = 3;
            this.channelNameTextBox.Text = "0";
            // 
            // configurationGroupBox
            // 
            this.configurationGroupBox.Controls.Add(this.exportButton);
            this.configurationGroupBox.Controls.Add(this.inputImpedanceLabel);
            this.configurationGroupBox.Controls.Add(this.performAutoSetupButton);
            this.configurationGroupBox.Controls.Add(this.importButton);
            this.configurationGroupBox.Controls.Add(this.minSampleRateTextBox);
            this.configurationGroupBox.Controls.Add(this.minSampleRateLabel);
            this.configurationGroupBox.Controls.Add(this.verticalCouplingComboBox);
            this.configurationGroupBox.Controls.Add(this.inputImpedanceComboBox);
            this.configurationGroupBox.Controls.Add(this.verticalCouplingLabel);
            this.configurationGroupBox.Controls.Add(this.verticalOffsetToolbox);
            this.configurationGroupBox.Controls.Add(this.verticalOffsetLabel);
            this.configurationGroupBox.Controls.Add(this.verticalRangeToolbox);
            this.configurationGroupBox.Controls.Add(this.verticalRangeLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(12, 90);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(245, 247);
            this.configurationGroupBox.TabIndex = 4;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(18, 208);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(101, 23);
            this.exportButton.TabIndex = 1;
            this.exportButton.Text = "Export...";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.Export_Click);
            // 
            // inputImpedanceLabel
            // 
            this.inputImpedanceLabel.AutoSize = true;
            this.inputImpedanceLabel.Location = new System.Drawing.Point(7, 101);
            this.inputImpedanceLabel.Name = "inputImpedanceLabel";
            this.inputImpedanceLabel.Size = new System.Drawing.Size(116, 13);
            this.inputImpedanceLabel.TabIndex = 0;
            this.inputImpedanceLabel.Text = "Input Impedance (ohm)";
            // 
            // performAutoSetupButton
            // 
            this.performAutoSetupButton.Location = new System.Drawing.Point(18, 163);
            this.performAutoSetupButton.Name = "performAutoSetupButton";
            this.performAutoSetupButton.Size = new System.Drawing.Size(208, 23);
            this.performAutoSetupButton.TabIndex = 1;
            this.performAutoSetupButton.Text = "Perform Auto-Setup";
            this.performAutoSetupButton.UseVisualStyleBackColor = true;
            this.performAutoSetupButton.Click += new System.EventHandler(this.AutoSetup_Click);
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(125, 208);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(101, 23);
            this.importButton.TabIndex = 1;
            this.importButton.Text = "Import...";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.Import_Click);
            // 
            // minSampleRateTextBox
            // 
            this.minSampleRateTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.minSampleRateTextBox.Location = new System.Drawing.Point(125, 124);
            this.minSampleRateTextBox.Name = "minSampleRateTextBox";
            this.minSampleRateTextBox.Size = new System.Drawing.Size(114, 20);
            this.minSampleRateTextBox.TabIndex = 6;
            this.minSampleRateTextBox.Text = "0";
            // 
            // minSampleRateLabel
            // 
            this.minSampleRateLabel.AutoSize = true;
            this.minSampleRateLabel.Location = new System.Drawing.Point(7, 128);
            this.minSampleRateLabel.Name = "minSampleRateLabel";
            this.minSampleRateLabel.Size = new System.Drawing.Size(88, 13);
            this.minSampleRateLabel.TabIndex = 5;
            this.minSampleRateLabel.Text = "Min Sample Rate";
            // 
            // verticalCouplingComboBox
            // 
            this.verticalCouplingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.verticalCouplingComboBox.FormattingEnabled = true;
            this.verticalCouplingComboBox.Location = new System.Drawing.Point(125, 71);
            this.verticalCouplingComboBox.Name = "verticalCouplingComboBox";
            this.verticalCouplingComboBox.Size = new System.Drawing.Size(114, 21);
            this.verticalCouplingComboBox.TabIndex = 1;
            // 
            // inputImpedanceComboBox
            // 
            this.inputImpedanceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputImpedanceComboBox.FormattingEnabled = true;
            this.inputImpedanceComboBox.Location = new System.Drawing.Point(125, 97);
            this.inputImpedanceComboBox.Name = "inputImpedanceComboBox";
            this.inputImpedanceComboBox.Size = new System.Drawing.Size(114, 21);
            this.inputImpedanceComboBox.TabIndex = 1;
            // 
            // verticalCouplingLabel
            // 
            this.verticalCouplingLabel.AutoSize = true;
            this.verticalCouplingLabel.Location = new System.Drawing.Point(7, 75);
            this.verticalCouplingLabel.Name = "verticalCouplingLabel";
            this.verticalCouplingLabel.Size = new System.Drawing.Size(86, 13);
            this.verticalCouplingLabel.TabIndex = 5;
            this.verticalCouplingLabel.Text = "Vertical Coupling";
            // 
            // verticalOffsetToolbox
            // 
            this.verticalOffsetToolbox.BackColor = System.Drawing.SystemColors.Window;
            this.verticalOffsetToolbox.Location = new System.Drawing.Point(125, 45);
            this.verticalOffsetToolbox.Name = "verticalOffsetToolbox";
            this.verticalOffsetToolbox.Size = new System.Drawing.Size(114, 20);
            this.verticalOffsetToolbox.TabIndex = 6;
            this.verticalOffsetToolbox.Text = "0";
            // 
            // verticalOffsetLabel
            // 
            this.verticalOffsetLabel.AutoSize = true;
            this.verticalOffsetLabel.Location = new System.Drawing.Point(7, 49);
            this.verticalOffsetLabel.Name = "verticalOffsetLabel";
            this.verticalOffsetLabel.Size = new System.Drawing.Size(89, 13);
            this.verticalOffsetLabel.TabIndex = 5;
            this.verticalOffsetLabel.Text = "Vertical Offset (V)";
            // 
            // verticalRangeToolbox
            // 
            this.verticalRangeToolbox.BackColor = System.Drawing.SystemColors.Window;
            this.verticalRangeToolbox.Location = new System.Drawing.Point(125, 19);
            this.verticalRangeToolbox.Name = "verticalRangeToolbox";
            this.verticalRangeToolbox.Size = new System.Drawing.Size(114, 20);
            this.verticalRangeToolbox.TabIndex = 6;
            this.verticalRangeToolbox.Text = "0";
            // 
            // verticalRangeLabel
            // 
            this.verticalRangeLabel.AutoSize = true;
            this.verticalRangeLabel.Location = new System.Drawing.Point(7, 23);
            this.verticalRangeLabel.Name = "verticalRangeLabel";
            this.verticalRangeLabel.Size = new System.Drawing.Size(93, 13);
            this.verticalRangeLabel.TabIndex = 5;
            this.verticalRangeLabel.Text = "Vertical Range (V)";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 348);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.buttonsGroupBox);
            this.Controls.Add(this.sampledDataGroupBox);
            this.Controls.Add(this.generalGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Export Import Configuration";
            this.buttonsGroupBox.ResumeLayout(false);
            this.sampledDataGroupBox.ResumeLayout(false);
            this.sampledDataGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sampledDataGridView)).EndInit();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.GroupBox buttonsGroupBox;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.TextBox actualSampleRateTextBox;
        private System.Windows.Forms.Label actualSampleRateLabel;
        private System.Windows.Forms.TextBox actualRecordLengthTextBox;
        private System.Windows.Forms.Label actualRecordLengthLabel;
        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.GroupBox sampledDataGroupBox;
        private System.Windows.Forms.DataGridView sampledDataGridView;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.TextBox verticalOffsetToolbox;
        private System.Windows.Forms.Label verticalOffsetLabel;
        private System.Windows.Forms.TextBox verticalRangeToolbox;
        private System.Windows.Forms.Label verticalRangeLabel;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Label verticalCouplingLabel;
        private System.Windows.Forms.Label inputImpedanceLabel;
        private System.Windows.Forms.TextBox minSampleRateTextBox;
        private System.Windows.Forms.Label minSampleRateLabel;
        private System.Windows.Forms.ComboBox inputImpedanceComboBox;
        private System.Windows.Forms.Button performAutoSetupButton;
        private System.Windows.Forms.ComboBox verticalCouplingComboBox;
    }
}
