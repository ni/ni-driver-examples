namespace NationalInstruments.Examples.BTReadWaveformFromFile
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
            this.components = new System.ComponentModel.Container();
            this.rfsgResourceLabel = new System.Windows.Forms.Label();
            this.chnNumberLabel = new System.Windows.Forms.Label();
            this.carrierFreqLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.externalAttnLabel = new System.Windows.Forms.Label();
            this.actualHeadroomLabel = new System.Windows.Forms.Label();
            this.waveformNameLabel = new System.Windows.Forms.Label();
            this.clkOutTerminalLabel = new System.Windows.Forms.Label();
            this.refSourceLabel = new System.Windows.Forms.Label();
            this.scriptLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.hardwareLabel = new System.Windows.Forms.Label();
            this.idleFilePathLabel = new System.Windows.Forms.Label();
            this.filePathLabel = new System.Windows.Forms.Label();
            this.chnNumberNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.externalAttnNumeric = new System.Windows.Forms.NumericUpDown();
            this.rfsgResourceTextBox = new System.Windows.Forms.TextBox();
            this.carrierFreqTextBox = new System.Windows.Forms.TextBox();
            this.actualHeadroomTextBox = new System.Windows.Forms.TextBox();
            this.waveNameTextBox = new System.Windows.Forms.TextBox();
            this.scriptTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.generateButton = new System.Windows.Forms.Button();
            this.clkOutTerminalComboBox = new System.Windows.Forms.ComboBox();
            this.refSourceComboBox = new System.Windows.Forms.ComboBox();
            this.dataWaveformFileBrowseButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.iOffsetLabel = new System.Windows.Forms.Label();
            this.qOffsetLabel = new System.Windows.Forms.Label();
            this.iOffsetNumeric = new System.Windows.Forms.NumericUpDown();
            this.qOffsetNumeric = new System.Windows.Forms.NumericUpDown();
            this.iCommonModeOffsetLabel = new System.Windows.Forms.Label();
            this.qCommonModeOffsetLabel = new System.Windows.Forms.Label();
            this.iCommonModeOffsetNumeric = new System.Windows.Forms.NumericUpDown();
            this.qCommonModeOffsetNumeric = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.terminalConfigurationComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.outputPortComboBox = new System.Windows.Forms.ComboBox();
            this.standardLabel = new System.Windows.Forms.Label();
            this.standardComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.chnNumberNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAttnNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iOffsetNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qOffsetNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iCommonModeOffsetNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qCommonModeOffsetNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // rfsgResourceLabel
            // 
            this.rfsgResourceLabel.AutoSize = true;
            this.rfsgResourceLabel.Location = new System.Drawing.Point(5, 80);
            this.rfsgResourceLabel.Name = "rfsgResourceLabel";
            this.rfsgResourceLabel.Size = new System.Drawing.Size(84, 13);
            this.rfsgResourceLabel.TabIndex = 0;
            this.rfsgResourceLabel.Text = "Resource Name";
            // 
            // chnNumberLabel
            // 
            this.chnNumberLabel.AutoSize = true;
            this.chnNumberLabel.Location = new System.Drawing.Point(4, 101);
            this.chnNumberLabel.Name = "chnNumberLabel";
            this.chnNumberLabel.Size = new System.Drawing.Size(86, 13);
            this.chnNumberLabel.TabIndex = 1;
            this.chnNumberLabel.Text = "Channel Number";
            // 
            // carrierFreqLabel
            // 
            this.carrierFreqLabel.AutoSize = true;
            this.carrierFreqLabel.Location = new System.Drawing.Point(6, 122);
            this.carrierFreqLabel.Name = "carrierFreqLabel";
            this.carrierFreqLabel.Size = new System.Drawing.Size(112, 13);
            this.carrierFreqLabel.TabIndex = 2;
            this.carrierFreqLabel.Text = "Carrier Frequency (Hz)";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(4, 142);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 3;
            this.powerLevelLabel.Text = "Power Level (dBm)";
            // 
            // externalAttnLabel
            // 
            this.externalAttnLabel.AutoSize = true;
            this.externalAttnLabel.Location = new System.Drawing.Point(4, 164);
            this.externalAttnLabel.Name = "externalAttnLabel";
            this.externalAttnLabel.Size = new System.Drawing.Size(124, 13);
            this.externalAttnLabel.TabIndex = 4;
            this.externalAttnLabel.Text = "External Attenuation (dB)";
            // 
            // actualHeadroomLabel
            // 
            this.actualHeadroomLabel.AutoSize = true;
            this.actualHeadroomLabel.Location = new System.Drawing.Point(4, 186);
            this.actualHeadroomLabel.Name = "actualHeadroomLabel";
            this.actualHeadroomLabel.Size = new System.Drawing.Size(111, 13);
            this.actualHeadroomLabel.TabIndex = 5;
            this.actualHeadroomLabel.Text = "Actual Headroom (dB)";
            // 
            // waveformNameLabel
            // 
            this.waveformNameLabel.AutoSize = true;
            this.waveformNameLabel.Location = new System.Drawing.Point(262, 95);
            this.waveformNameLabel.Name = "waveformNameLabel";
            this.waveformNameLabel.Size = new System.Drawing.Size(87, 13);
            this.waveformNameLabel.TabIndex = 7;
            this.waveformNameLabel.Text = "Waveform Name";
            // 
            // clkOutTerminalLabel
            // 
            this.clkOutTerminalLabel.AutoSize = true;
            this.clkOutTerminalLabel.Location = new System.Drawing.Point(7, 268);
            this.clkOutTerminalLabel.Name = "clkOutTerminalLabel";
            this.clkOutTerminalLabel.Size = new System.Drawing.Size(67, 13);
            this.clkOutTerminalLabel.TabIndex = 8;
            this.clkOutTerminalLabel.Text = "Export Clock";
            // 
            // refSourceLabel
            // 
            this.refSourceLabel.AutoSize = true;
            this.refSourceLabel.Location = new System.Drawing.Point(7, 249);
            this.refSourceLabel.Name = "refSourceLabel";
            this.refSourceLabel.Size = new System.Drawing.Size(94, 13);
            this.refSourceLabel.TabIndex = 9;
            this.refSourceLabel.Text = "Reference Source";
            // 
            // scriptLabel
            // 
            this.scriptLabel.AutoSize = true;
            this.scriptLabel.Location = new System.Drawing.Point(269, 158);
            this.scriptLabel.Name = "scriptLabel";
            this.scriptLabel.Size = new System.Drawing.Size(34, 13);
            this.scriptLabel.TabIndex = 10;
            this.scriptLabel.Text = "Script";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(11, 292);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(120, 13);
            this.errorLabel.TabIndex = 11;
            this.errorLabel.Text = "Error/Warning Message";
            // 
            // hardwareLabel
            // 
            this.hardwareLabel.AutoSize = true;
            this.hardwareLabel.Location = new System.Drawing.Point(21, 54);
            this.hardwareLabel.Name = "hardwareLabel";
            this.hardwareLabel.Size = new System.Drawing.Size(53, 13);
            this.hardwareLabel.TabIndex = 12;
            this.hardwareLabel.Text = "Hardware";
            // 
            // idleFilePathLabel
            // 
            this.idleFilePathLabel.AutoSize = true;
            this.idleFilePathLabel.Location = new System.Drawing.Point(10, 22);
            this.idleFilePathLabel.Name = "idleFilePathLabel";
            this.idleFilePathLabel.Size = new System.Drawing.Size(0, 13);
            this.idleFilePathLabel.TabIndex = 13;
            // 
            // filePathLabel
            // 
            this.filePathLabel.AutoSize = true;
            this.filePathLabel.Location = new System.Drawing.Point(10, -9);
            this.filePathLabel.Name = "filePathLabel";
            this.filePathLabel.Size = new System.Drawing.Size(0, 13);
            this.filePathLabel.TabIndex = 14;
            // 
            // chnNumberNumeric
            // 
            this.chnNumberNumeric.Location = new System.Drawing.Point(136, 97);
            this.chnNumberNumeric.Name = "chnNumberNumeric";
            this.chnNumberNumeric.Size = new System.Drawing.Size(103, 20);
            this.chnNumberNumeric.TabIndex = 1;
            // 
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.Location = new System.Drawing.Point(136, 139);
            this.powerLevelNumeric.Name = "powerLevelNumeric";
            this.powerLevelNumeric.Size = new System.Drawing.Size(103, 20);
            this.powerLevelNumeric.TabIndex = 3;
            // 
            // externalAttnNumeric
            // 
            this.externalAttnNumeric.Location = new System.Drawing.Point(136, 161);
            this.externalAttnNumeric.Name = "externalAttnNumeric";
            this.externalAttnNumeric.Size = new System.Drawing.Size(103, 20);
            this.externalAttnNumeric.TabIndex = 4;
            // 
            // rfsgResourceTextBox
            // 
            this.rfsgResourceTextBox.Location = new System.Drawing.Point(136, 76);
            this.rfsgResourceTextBox.Name = "rfsgResourceTextBox";
            this.rfsgResourceTextBox.Size = new System.Drawing.Size(103, 20);
            this.rfsgResourceTextBox.TabIndex = 0;
            this.rfsgResourceTextBox.Text = "RFSG";
            // 
            // carrierFreqTextBox
            // 
            this.carrierFreqTextBox.Enabled = false;
            this.carrierFreqTextBox.Location = new System.Drawing.Point(136, 117);
            this.carrierFreqTextBox.Name = "carrierFreqTextBox";
            this.carrierFreqTextBox.Size = new System.Drawing.Size(103, 20);
            this.carrierFreqTextBox.TabIndex = 2;
            this.carrierFreqTextBox.Text = "2.405E+9";
            // 
            // actualHeadroomTextBox
            // 
            this.actualHeadroomTextBox.Enabled = false;
            this.actualHeadroomTextBox.Location = new System.Drawing.Point(136, 183);
            this.actualHeadroomTextBox.Name = "actualHeadroomTextBox";
            this.actualHeadroomTextBox.Size = new System.Drawing.Size(103, 20);
            this.actualHeadroomTextBox.TabIndex = 5;
            this.actualHeadroomTextBox.Text = "0.00";
            // 
            // waveNameTextBox
            // 
            this.waveNameTextBox.Location = new System.Drawing.Point(352, 92);
            this.waveNameTextBox.Name = "waveNameTextBox";
            this.waveNameTextBox.Size = new System.Drawing.Size(87, 20);
            this.waveNameTextBox.TabIndex = 7;
            this.waveNameTextBox.Text = "Data";
            // 
            // scriptTextBox
            // 
            this.scriptTextBox.Location = new System.Drawing.Point(272, 188);
            this.scriptTextBox.Multiline = true;
            this.scriptTextBox.Name = "scriptTextBox";
            this.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.scriptTextBox.Size = new System.Drawing.Size(162, 82);
            this.scriptTextBox.TabIndex = 15;
            this.scriptTextBox.TabStop = false;
            this.scriptTextBox.Text = "script GenerateDataPkt\r\n  repeat forever\r\n    generate Data\r\n     end repeat\r\nend" +
                " script";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(11, 313);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(603, 73);
            this.errorTextBox.TabIndex = 11;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No Error";
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Location = new System.Drawing.Point(10, 12);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.Size = new System.Drawing.Size(303, 20);
            this.filePathTextBox.TabIndex = 20;
            this.filePathTextBox.Text = "C:\\Users\\Public\\Documents\\saveConfig.tdms";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(493, 405);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 12;
            this.stopButton.Text = "S&top";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(16, 405);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(75, 23);
            this.generateButton.TabIndex = 16;
            this.generateButton.Text = "&Start";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // clkOutTerminalComboBox
            // 
            this.clkOutTerminalComboBox.Location = new System.Drawing.Point(152, 268);
            this.clkOutTerminalComboBox.Name = "clkOutTerminalComboBox";
            this.clkOutTerminalComboBox.Size = new System.Drawing.Size(90, 21);
            this.clkOutTerminalComboBox.TabIndex = 9;
            // 
            // refSourceComboBox
            // 
            this.refSourceComboBox.Location = new System.Drawing.Point(152, 249);
            this.refSourceComboBox.Name = "refSourceComboBox";
            this.refSourceComboBox.Size = new System.Drawing.Size(90, 21);
            this.refSourceComboBox.TabIndex = 10;
            // 
            // dataWaveformFileBrowseButton
            // 
            this.dataWaveformFileBrowseButton.Location = new System.Drawing.Point(322, 12);
            this.dataWaveformFileBrowseButton.Name = "dataWaveformFileBrowseButton";
            this.dataWaveformFileBrowseButton.Size = new System.Drawing.Size(112, 23);
            this.dataWaveformFileBrowseButton.TabIndex = 21;
            this.dataWaveformFileBrowseButton.Text = "&Data Waveform File";
            this.dataWaveformFileBrowseButton.UseVisualStyleBackColor = true;
            this.dataWaveformFileBrowseButton.Click += new System.EventHandler(this.dataWaveformFileBrowseButton_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.ProcessTimerEvent);
            // 
            // iOffsetLabel
            // 
            this.iOffsetLabel.AutoSize = true;
            this.iOffsetLabel.Location = new System.Drawing.Point(458, 108);
            this.iOffsetLabel.Name = "iOffsetLabel";
            this.iOffsetLabel.Size = new System.Drawing.Size(54, 13);
            this.iOffsetLabel.TabIndex = 76;
            this.iOffsetLabel.Text = "I Offset(V)";
            // 
            // qOffsetLabel
            // 
            this.qOffsetLabel.AutoSize = true;
            this.qOffsetLabel.Location = new System.Drawing.Point(458, 130);
            this.qOffsetLabel.Name = "qOffsetLabel";
            this.qOffsetLabel.Size = new System.Drawing.Size(59, 13);
            this.qOffsetLabel.TabIndex = 78;
            this.qOffsetLabel.Text = "Q Offset(V)";
            // 
            // iOffsetNumeric
            // 
            this.iOffsetNumeric.Location = new System.Drawing.Point(600, 106);
            this.iOffsetNumeric.Name = "iOffsetNumeric";
            this.iOffsetNumeric.Size = new System.Drawing.Size(90, 20);
            this.iOffsetNumeric.TabIndex = 75;
            // 
            // qOffsetNumeric
            // 
            this.qOffsetNumeric.Location = new System.Drawing.Point(598, 132);
            this.qOffsetNumeric.Name = "qOffsetNumeric";
            this.qOffsetNumeric.Size = new System.Drawing.Size(90, 20);
            this.qOffsetNumeric.TabIndex = 77;
            // 
            // iCommonModeOffsetLabel
            // 
            this.iCommonModeOffsetLabel.AutoSize = true;
            this.iCommonModeOffsetLabel.Location = new System.Drawing.Point(458, 158);
            this.iCommonModeOffsetLabel.Name = "iCommonModeOffsetLabel";
            this.iCommonModeOffsetLabel.Size = new System.Drawing.Size(128, 13);
            this.iCommonModeOffsetLabel.TabIndex = 72;
            this.iCommonModeOffsetLabel.Text = "I Common Mode Offset(V)";
            // 
            // qCommonModeOffsetLabel
            // 
            this.qCommonModeOffsetLabel.AutoSize = true;
            this.qCommonModeOffsetLabel.Location = new System.Drawing.Point(458, 180);
            this.qCommonModeOffsetLabel.Name = "qCommonModeOffsetLabel";
            this.qCommonModeOffsetLabel.Size = new System.Drawing.Size(133, 13);
            this.qCommonModeOffsetLabel.TabIndex = 74;
            this.qCommonModeOffsetLabel.Text = "Q Common Mode Offset(V)";
            // 
            // iCommonModeOffsetNumeric
            // 
            this.iCommonModeOffsetNumeric.Location = new System.Drawing.Point(600, 156);
            this.iCommonModeOffsetNumeric.Name = "iCommonModeOffsetNumeric";
            this.iCommonModeOffsetNumeric.Size = new System.Drawing.Size(90, 20);
            this.iCommonModeOffsetNumeric.TabIndex = 71;
            // 
            // qCommonModeOffsetNumeric
            // 
            this.qCommonModeOffsetNumeric.Location = new System.Drawing.Point(600, 178);
            this.qCommonModeOffsetNumeric.Name = "qCommonModeOffsetNumeric";
            this.qCommonModeOffsetNumeric.Size = new System.Drawing.Size(90, 20);
            this.qCommonModeOffsetNumeric.TabIndex = 73;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(456, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 70;
            this.label2.Text = "Terminal Configuration";
            // 
            // terminalConfigurationComboBox
            // 
            this.terminalConfigurationComboBox.Location = new System.Drawing.Point(598, 79);
            this.terminalConfigurationComboBox.Name = "terminalConfigurationComboBox";
            this.terminalConfigurationComboBox.Size = new System.Drawing.Size(90, 21);
            this.terminalConfigurationComboBox.TabIndex = 69;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(459, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 68;
            this.label1.Text = "Output Port";
            // 
            // outputPortComboBox
            // 
            this.outputPortComboBox.Location = new System.Drawing.Point(598, 54);
            this.outputPortComboBox.Name = "outputPortComboBox";
            this.outputPortComboBox.Size = new System.Drawing.Size(90, 21);
            this.outputPortComboBox.TabIndex = 67;
            // 
            // standardLabel
            // 
            this.standardLabel.AutoSize = true;
            this.standardLabel.Location = new System.Drawing.Point(6, 210);
            this.standardLabel.Name = "standardLabel";
            this.standardLabel.Size = new System.Drawing.Size(50, 13);
            this.standardLabel.TabIndex = 79;
            this.standardLabel.Text = "Standard";
            // 
            // standardComboBox
            // 
            this.standardComboBox.Location = new System.Drawing.Point(136, 210);
            this.standardComboBox.Name = "standardComboBox";
            this.standardComboBox.Size = new System.Drawing.Size(103, 21);
            this.standardComboBox.TabIndex = 80;
            // 
            // MainForm
            // 
            this.AcceptButton = this.generateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 444);
            this.Controls.Add(this.standardComboBox);
            this.Controls.Add(this.standardLabel);
            this.Controls.Add(this.iOffsetLabel);
            this.Controls.Add(this.qOffsetLabel);
            this.Controls.Add(this.iOffsetNumeric);
            this.Controls.Add(this.qOffsetNumeric);
            this.Controls.Add(this.iCommonModeOffsetLabel);
            this.Controls.Add(this.qCommonModeOffsetLabel);
            this.Controls.Add(this.iCommonModeOffsetNumeric);
            this.Controls.Add(this.qCommonModeOffsetNumeric);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.terminalConfigurationComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outputPortComboBox);
            this.Controls.Add(this.dataWaveformFileBrowseButton);
            this.Controls.Add(this.rfsgResourceLabel);
            this.Controls.Add(this.chnNumberLabel);
            this.Controls.Add(this.carrierFreqLabel);
            this.Controls.Add(this.powerLevelLabel);
            this.Controls.Add(this.externalAttnLabel);
            this.Controls.Add(this.actualHeadroomLabel);
            this.Controls.Add(this.waveformNameLabel);
            this.Controls.Add(this.clkOutTerminalLabel);
            this.Controls.Add(this.refSourceLabel);
            this.Controls.Add(this.scriptLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.hardwareLabel);
            this.Controls.Add(this.idleFilePathLabel);
            this.Controls.Add(this.filePathLabel);
            this.Controls.Add(this.chnNumberNumeric);
            this.Controls.Add(this.powerLevelNumeric);
            this.Controls.Add(this.externalAttnNumeric);
            this.Controls.Add(this.rfsgResourceTextBox);
            this.Controls.Add(this.carrierFreqTextBox);
            this.Controls.Add(this.actualHeadroomTextBox);
            this.Controls.Add(this.waveNameTextBox);
            this.Controls.Add(this.scriptTextBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.filePathTextBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.clkOutTerminalComboBox);
            this.Controls.Add(this.refSourceComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Bluetooth Generate Waveform from File Example";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.chnNumberNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAttnNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iOffsetNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qOffsetNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iCommonModeOffsetNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qCommonModeOffsetNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		
		private System.Windows.Forms.Label rfsgResourceLabel;
        private System.Windows.Forms.Label chnNumberLabel;
        private System.Windows.Forms.Label carrierFreqLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label externalAttnLabel;
        private System.Windows.Forms.Label actualHeadroomLabel;
        private System.Windows.Forms.Label waveformNameLabel;
        private System.Windows.Forms.Label clkOutTerminalLabel;
        private System.Windows.Forms.Label refSourceLabel;
        private System.Windows.Forms.Label scriptLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label hardwareLabel;
        private System.Windows.Forms.Label idleFilePathLabel;
        private System.Windows.Forms.Label filePathLabel;
        private System.Windows.Forms.NumericUpDown chnNumberNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown externalAttnNumeric;
        private System.Windows.Forms.TextBox rfsgResourceTextBox;
        private System.Windows.Forms.TextBox carrierFreqTextBox;
        private System.Windows.Forms.TextBox actualHeadroomTextBox;
        private System.Windows.Forms.TextBox waveNameTextBox;
        private System.Windows.Forms.TextBox scriptTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.TextBox filePathTextBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.ComboBox clkOutTerminalComboBox;
        private System.Windows.Forms.ComboBox refSourceComboBox;
        private System.Windows.Forms.Button dataWaveformFileBrowseButton;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label iOffsetLabel;
        private System.Windows.Forms.Label qOffsetLabel;
        private System.Windows.Forms.NumericUpDown iOffsetNumeric;
        private System.Windows.Forms.NumericUpDown qOffsetNumeric;
        private System.Windows.Forms.Label iCommonModeOffsetLabel;
        private System.Windows.Forms.Label qCommonModeOffsetLabel;
        private System.Windows.Forms.NumericUpDown iCommonModeOffsetNumeric;
        private System.Windows.Forms.NumericUpDown qCommonModeOffsetNumeric;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox terminalConfigurationComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox outputPortComboBox;
        private System.Windows.Forms.Label standardLabel;
        private System.Windows.Forms.ComboBox standardComboBox;
		
	}
}
