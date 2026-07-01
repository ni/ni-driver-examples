namespace NationalInstruments.Examples.BTLoadConfigurationAndGenerate
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
            this.waveNameLabel = new System.Windows.Forms.Label();
            this.clkOutTerminalLabel = new System.Windows.Forms.Label();
            this.refSourceLabel = new System.Windows.Forms.Label();
            this.scriptLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.hardwareLabel = new System.Windows.Forms.Label();
            this.filePathLabel = new System.Windows.Forms.Label();
            this.chnNumberNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.externalAttnNumeric = new System.Windows.Forms.NumericUpDown();
            this.rfsgResourceTextBox = new System.Windows.Forms.TextBox();
            this.carrierFreqTextBox = new System.Windows.Forms.TextBox();
            this.actualHeadroomTextBox = new System.Windows.Forms.TextBox();
            this.waveNameTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.scriptTextBox = new System.Windows.Forms.TextBox();
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.generateButton = new System.Windows.Forms.Button();
            this.browseButton = new System.Windows.Forms.Button();
            this.clkOutTerminalComboBox = new System.Windows.Forms.ComboBox();
            this.refSourceComboBox = new System.Windows.Forms.ComboBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.standardLabel = new System.Windows.Forms.Label();
            this.standardComboBox = new System.Windows.Forms.ComboBox();
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
            this.rfsgResourceLabel.Location = new System.Drawing.Point(17, 69);
            this.rfsgResourceLabel.Name = "rfsgResourceLabel";
            this.rfsgResourceLabel.Size = new System.Drawing.Size(85, 13);
            this.rfsgResourceLabel.TabIndex = 0;
            this.rfsgResourceLabel.Text = "RFSG Resource";
            // 
            // chnNumberLabel
            // 
            this.chnNumberLabel.AutoSize = true;
            this.chnNumberLabel.Location = new System.Drawing.Point(17, 87);
            this.chnNumberLabel.Name = "chnNumberLabel";
            this.chnNumberLabel.Size = new System.Drawing.Size(86, 13);
            this.chnNumberLabel.TabIndex = 1;
            this.chnNumberLabel.Text = "Channel Number";
            // 
            // carrierFreqLabel
            // 
            this.carrierFreqLabel.AutoSize = true;
            this.carrierFreqLabel.Location = new System.Drawing.Point(17, 134);
            this.carrierFreqLabel.Name = "carrierFreqLabel";
            this.carrierFreqLabel.Size = new System.Drawing.Size(112, 13);
            this.carrierFreqLabel.TabIndex = 2;
            this.carrierFreqLabel.Text = "Carrier Frequency (Hz)";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(15, 154);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(96, 13);
            this.powerLevelLabel.TabIndex = 3;
            this.powerLevelLabel.Text = "Power Level (dBm)";
            // 
            // externalAttnLabel
            // 
            this.externalAttnLabel.AutoSize = true;
            this.externalAttnLabel.Location = new System.Drawing.Point(15, 176);
            this.externalAttnLabel.Name = "externalAttnLabel";
            this.externalAttnLabel.Size = new System.Drawing.Size(124, 13);
            this.externalAttnLabel.TabIndex = 4;
            this.externalAttnLabel.Text = "External Attenuation (dB)";
            // 
            // actualHeadroomLabel
            // 
            this.actualHeadroomLabel.AutoSize = true;
            this.actualHeadroomLabel.Location = new System.Drawing.Point(15, 198);
            this.actualHeadroomLabel.Name = "actualHeadroomLabel";
            this.actualHeadroomLabel.Size = new System.Drawing.Size(111, 13);
            this.actualHeadroomLabel.TabIndex = 5;
            this.actualHeadroomLabel.Text = "Actual Headroom (dB)";
            // 
            // waveNameLabel
            // 
            this.waveNameLabel.AutoSize = true;
            this.waveNameLabel.Location = new System.Drawing.Point(271, 64);
            this.waveNameLabel.Name = "waveNameLabel";
            this.waveNameLabel.Size = new System.Drawing.Size(87, 13);
            this.waveNameLabel.TabIndex = 6;
            this.waveNameLabel.Text = "Waveform Name";
            // 
            // clkOutTerminalLabel
            // 
            this.clkOutTerminalLabel.AutoSize = true;
            this.clkOutTerminalLabel.Location = new System.Drawing.Point(17, 238);
            this.clkOutTerminalLabel.Name = "clkOutTerminalLabel";
            this.clkOutTerminalLabel.Size = new System.Drawing.Size(67, 13);
            this.clkOutTerminalLabel.TabIndex = 7;
            this.clkOutTerminalLabel.Text = "Export Clock";
            // 
            // refSourceLabel
            // 
            this.refSourceLabel.AutoSize = true;
            this.refSourceLabel.Location = new System.Drawing.Point(17, 219);
            this.refSourceLabel.Name = "refSourceLabel";
            this.refSourceLabel.Size = new System.Drawing.Size(94, 13);
            this.refSourceLabel.TabIndex = 8;
            this.refSourceLabel.Text = "Reference Source";
            // 
            // scriptLabel
            // 
            this.scriptLabel.AutoSize = true;
            this.scriptLabel.Location = new System.Drawing.Point(277, 110);
            this.scriptLabel.Name = "scriptLabel";
            this.scriptLabel.Size = new System.Drawing.Size(34, 13);
            this.scriptLabel.TabIndex = 36;
            this.scriptLabel.Text = "Script";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(21, 262);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(29, 13);
            this.errorLabel.TabIndex = 9;
            this.errorLabel.Text = "Error";
            // 
            // hardwareLabel
            // 
            this.hardwareLabel.AutoSize = true;
            this.hardwareLabel.Location = new System.Drawing.Point(30, 46);
            this.hardwareLabel.Name = "hardwareLabel";
            this.hardwareLabel.Size = new System.Drawing.Size(53, 13);
            this.hardwareLabel.TabIndex = 10;
            this.hardwareLabel.Text = "Hardware";
            // 
            // filePathLabel
            // 
            this.filePathLabel.AutoSize = true;
            this.filePathLabel.Location = new System.Drawing.Point(20, -9);
            this.filePathLabel.Name = "filePathLabel";
            this.filePathLabel.Size = new System.Drawing.Size(0, 13);
            this.filePathLabel.TabIndex = 11;
            // 
            // chnNumberNumeric
            // 
            this.chnNumberNumeric.Location = new System.Drawing.Point(162, 83);
            this.chnNumberNumeric.Name = "chnNumberNumeric";
            this.chnNumberNumeric.Size = new System.Drawing.Size(90, 20);
            this.chnNumberNumeric.TabIndex = 1;
            // 
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.Location = new System.Drawing.Point(160, 151);
            this.powerLevelNumeric.Name = "powerLevelNumeric";
            this.powerLevelNumeric.Size = new System.Drawing.Size(90, 20);
            this.powerLevelNumeric.TabIndex = 3;
            // 
            // externalAttnNumeric
            // 
            this.externalAttnNumeric.Location = new System.Drawing.Point(160, 173);
            this.externalAttnNumeric.Name = "externalAttnNumeric";
            this.externalAttnNumeric.Size = new System.Drawing.Size(90, 20);
            this.externalAttnNumeric.TabIndex = 4;
            // 
            // rfsgResourceTextBox
            // 
            this.rfsgResourceTextBox.Location = new System.Drawing.Point(162, 62);
            this.rfsgResourceTextBox.Name = "rfsgResourceTextBox";
            this.rfsgResourceTextBox.Size = new System.Drawing.Size(90, 20);
            this.rfsgResourceTextBox.TabIndex = 0;
            this.rfsgResourceTextBox.Text = "RFSG";
            // 
            // carrierFreqTextBox
            // 
            this.carrierFreqTextBox.Enabled = false;
            this.carrierFreqTextBox.Location = new System.Drawing.Point(160, 129);
            this.carrierFreqTextBox.Name = "carrierFreqTextBox";
            this.carrierFreqTextBox.Size = new System.Drawing.Size(90, 20);
            this.carrierFreqTextBox.TabIndex = 2;
            this.carrierFreqTextBox.Text = "2.402E+9";
            // 
            // actualHeadroomTextBox
            // 
            this.actualHeadroomTextBox.Enabled = false;
            this.actualHeadroomTextBox.Location = new System.Drawing.Point(160, 195);
            this.actualHeadroomTextBox.Name = "actualHeadroomTextBox";
            this.actualHeadroomTextBox.Size = new System.Drawing.Size(90, 20);
            this.actualHeadroomTextBox.TabIndex = 5;
            this.actualHeadroomTextBox.Text = "0.00";
            // 
            // waveNameTextBox
            // 
            this.waveNameTextBox.Location = new System.Drawing.Point(361, 61);
            this.waveNameTextBox.Name = "waveNameTextBox";
            this.waveNameTextBox.Size = new System.Drawing.Size(87, 20);
            this.waveNameTextBox.TabIndex = 6;
            this.waveNameTextBox.Text = "Data";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(21, 283);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(720, 73);
            this.errorTextBox.TabIndex = 10;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No Error";
            // 
            // scriptTextBox
            // 
            this.scriptTextBox.Location = new System.Drawing.Point(280, 134);
            this.scriptTextBox.Multiline = true;
            this.scriptTextBox.Name = "scriptTextBox";
            this.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.scriptTextBox.Size = new System.Drawing.Size(174, 106);
            this.scriptTextBox.TabIndex = 38;
            this.scriptTextBox.TabStop = false;
            this.scriptTextBox.Text = "script GenerateDATAPkt\r\nrepeat forever\r\ngenerate Data\r\nend repeat\r\nend script";
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Location = new System.Drawing.Point(20, 12);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.Size = new System.Drawing.Size(281, 20);
            this.filePathTextBox.TabIndex = 17;
            this.filePathTextBox.Text = "C:\\Users\\Public\\Documents\\saveConfig.tdms";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(618, 376);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 11;
            this.stopButton.Text = "&Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(24, 376);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(75, 23);
            this.generateButton.TabIndex = 15;
            this.generateButton.Text = "&Start";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(334, 12);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 0;
            this.browseButton.Text = "&Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // clkOutTerminalComboBox
            // 
            this.clkOutTerminalComboBox.Location = new System.Drawing.Point(162, 238);
            this.clkOutTerminalComboBox.Name = "clkOutTerminalComboBox";
            this.clkOutTerminalComboBox.Size = new System.Drawing.Size(90, 21);
            this.clkOutTerminalComboBox.TabIndex = 8;
            // 
            // refSourceComboBox
            // 
            this.refSourceComboBox.Location = new System.Drawing.Point(162, 219);
            this.refSourceComboBox.Name = "refSourceComboBox";
            this.refSourceComboBox.Size = new System.Drawing.Size(90, 21);
            this.refSourceComboBox.TabIndex = 9;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.ProcessTimerEvent);
            // 
            // standardLabel
            // 
            this.standardLabel.AutoSize = true;
            this.standardLabel.Location = new System.Drawing.Point(17, 110);
            this.standardLabel.Name = "standardLabel";
            this.standardLabel.Size = new System.Drawing.Size(50, 13);
            this.standardLabel.TabIndex = 39;
            this.standardLabel.Text = "Standard";
            // 
            // standardComboBox
            // 
            this.standardComboBox.FormattingEnabled = true;
            this.standardComboBox.Location = new System.Drawing.Point(162, 107);
            this.standardComboBox.Name = "standardComboBox";
            this.standardComboBox.Size = new System.Drawing.Size(90, 21);
            this.standardComboBox.TabIndex = 40;
            // 
            // iOffsetLabel
            // 
            this.iOffsetLabel.AutoSize = true;
            this.iOffsetLabel.Location = new System.Drawing.Point(478, 119);
            this.iOffsetLabel.Name = "iOffsetLabel";
            this.iOffsetLabel.Size = new System.Drawing.Size(54, 13);
            this.iOffsetLabel.TabIndex = 76;
            this.iOffsetLabel.Text = "I Offset(V)";
            // 
            // qOffsetLabel
            // 
            this.qOffsetLabel.AutoSize = true;
            this.qOffsetLabel.Location = new System.Drawing.Point(478, 141);
            this.qOffsetLabel.Name = "qOffsetLabel";
            this.qOffsetLabel.Size = new System.Drawing.Size(59, 13);
            this.qOffsetLabel.TabIndex = 78;
            this.qOffsetLabel.Text = "Q Offset(V)";
            // 
            // iOffsetNumeric
            // 
            this.iOffsetNumeric.Location = new System.Drawing.Point(620, 117);
            this.iOffsetNumeric.Name = "iOffsetNumeric";
            this.iOffsetNumeric.Size = new System.Drawing.Size(90, 20);
            this.iOffsetNumeric.TabIndex = 75;
            // 
            // qOffsetNumeric
            // 
            this.qOffsetNumeric.Location = new System.Drawing.Point(618, 143);
            this.qOffsetNumeric.Name = "qOffsetNumeric";
            this.qOffsetNumeric.Size = new System.Drawing.Size(90, 20);
            this.qOffsetNumeric.TabIndex = 77;
            // 
            // iCommonModeOffsetLabel
            // 
            this.iCommonModeOffsetLabel.AutoSize = true;
            this.iCommonModeOffsetLabel.Location = new System.Drawing.Point(478, 169);
            this.iCommonModeOffsetLabel.Name = "iCommonModeOffsetLabel";
            this.iCommonModeOffsetLabel.Size = new System.Drawing.Size(128, 13);
            this.iCommonModeOffsetLabel.TabIndex = 72;
            this.iCommonModeOffsetLabel.Text = "I Common Mode Offset(V)";
            // 
            // qCommonModeOffsetLabel
            // 
            this.qCommonModeOffsetLabel.AutoSize = true;
            this.qCommonModeOffsetLabel.Location = new System.Drawing.Point(478, 191);
            this.qCommonModeOffsetLabel.Name = "qCommonModeOffsetLabel";
            this.qCommonModeOffsetLabel.Size = new System.Drawing.Size(133, 13);
            this.qCommonModeOffsetLabel.TabIndex = 74;
            this.qCommonModeOffsetLabel.Text = "Q Common Mode Offset(V)";
            // 
            // iCommonModeOffsetNumeric
            // 
            this.iCommonModeOffsetNumeric.Location = new System.Drawing.Point(620, 167);
            this.iCommonModeOffsetNumeric.Name = "iCommonModeOffsetNumeric";
            this.iCommonModeOffsetNumeric.Size = new System.Drawing.Size(90, 20);
            this.iCommonModeOffsetNumeric.TabIndex = 71;
            // 
            // qCommonModeOffsetNumeric
            // 
            this.qCommonModeOffsetNumeric.Location = new System.Drawing.Point(620, 189);
            this.qCommonModeOffsetNumeric.Name = "qCommonModeOffsetNumeric";
            this.qCommonModeOffsetNumeric.Size = new System.Drawing.Size(90, 20);
            this.qCommonModeOffsetNumeric.TabIndex = 73;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(476, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 70;
            this.label2.Text = "Terminal Configuration";
            // 
            // terminalConfigurationComboBox
            // 
            this.terminalConfigurationComboBox.Location = new System.Drawing.Point(618, 90);
            this.terminalConfigurationComboBox.Name = "terminalConfigurationComboBox";
            this.terminalConfigurationComboBox.Size = new System.Drawing.Size(90, 21);
            this.terminalConfigurationComboBox.TabIndex = 69;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(479, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 68;
            this.label1.Text = "Output Port";
            // 
            // outputPortComboBox
            // 
            this.outputPortComboBox.Location = new System.Drawing.Point(618, 65);
            this.outputPortComboBox.Name = "outputPortComboBox";
            this.outputPortComboBox.Size = new System.Drawing.Size(90, 21);
            this.outputPortComboBox.TabIndex = 67;
            // 
            // MainForm
            // 
            this.AcceptButton = this.generateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 419);
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
            this.Controls.Add(this.standardComboBox);
            this.Controls.Add(this.standardLabel);
            this.Controls.Add(this.rfsgResourceLabel);
            this.Controls.Add(this.chnNumberLabel);
            this.Controls.Add(this.carrierFreqLabel);
            this.Controls.Add(this.powerLevelLabel);
            this.Controls.Add(this.externalAttnLabel);
            this.Controls.Add(this.actualHeadroomLabel);
            this.Controls.Add(this.waveNameLabel);
            this.Controls.Add(this.clkOutTerminalLabel);
            this.Controls.Add(this.refSourceLabel);
            this.Controls.Add(this.scriptLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.hardwareLabel);
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
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.clkOutTerminalComboBox);
            this.Controls.Add(this.refSourceComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Bluetooth Load Configuration and Generate Waveform Example";
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
        private System.Windows.Forms.Label waveNameLabel;
        private System.Windows.Forms.Label clkOutTerminalLabel;
        private System.Windows.Forms.Label refSourceLabel;
        private System.Windows.Forms.Label scriptLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label hardwareLabel;
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
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.ComboBox clkOutTerminalComboBox;
        private System.Windows.Forms.ComboBox refSourceComboBox;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label standardLabel;
        private System.Windows.Forms.ComboBox standardComboBox;
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
    }
}
