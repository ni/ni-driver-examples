namespace NationalInstruments.Examples.BTGenerateDataPacket
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
			this.autoheadroomEnabLabel = new System.Windows.Forms.Label();
			this.headroomLabel = new System.Windows.Forms.Label();
			this.actualHeadroomLabel = new System.Windows.Forms.Label();
			this.refSourceLabel = new System.Windows.Forms.Label();
			this.clkOutTerminalLabel = new System.Windows.Forms.Label();
			this.allIqImpairEnLabel = new System.Windows.Forms.Label();
			this.quadratureSkewLabel = new System.Windows.Forms.Label();
			this.iDCOffsetLabel = new System.Windows.Forms.Label();
			this.qDCOffsetLabel = new System.Windows.Forms.Label();
			this.iqGaimbalanceLabel = new System.Windows.Forms.Label();
			this.carrierFreqOffLabel = new System.Windows.Forms.Label();
			this.awgnEnabledLabel = new System.Windows.Forms.Label();
			this.cnrLabel = new System.Windows.Forms.Label();
			this.bdaddrLapLabel = new System.Windows.Forms.Label();
			this.bdaddrUapLabel = new System.Windows.Forms.Label();
			this.bdaddrNapLabel = new System.Windows.Forms.Label();
			this.pktLtAddrLabel = new System.Windows.Forms.Label();
			this.pktHdrFlowLabel = new System.Windows.Forms.Label();
			this.pktHdrArqnLabel = new System.Windows.Forms.Label();
			this.pktHdrSeqnLabel = new System.Windows.Forms.Label();
			this.payHdrLlidLabel = new System.Windows.Forms.Label();
			this.payHdrFlowLabel = new System.Windows.Forms.Label();
			this.userDefinedBitLabel = new System.Windows.Forms.Label();
			this.payHdrPaylenModeLabel = new System.Windows.Forms.Label();
			this.payHdrPaylenLabel = new System.Windows.Forms.Label();
			this.payHdrActPaylenLabel = new System.Windows.Forms.Label();
			this.payHdrDatatypeLabel = new System.Windows.Forms.Label();
			this.paydatPnorderLabel = new System.Windows.Forms.Label();
			this.paydatSeedLabel = new System.Windows.Forms.Label();
			this.errorLabel = new System.Windows.Forms.Label();
			this.extmsgPacket22Label = new System.Windows.Forms.Label();
			this.textmsgPayloadHdrLabel = new System.Windows.Forms.Label();
			this.payloadDataLabel = new System.Windows.Forms.Label();
			this.hardwareLabel = new System.Windows.Forms.Label();
			this.frequencySettingLabel = new System.Windows.Forms.Label();
			this.impairmentsLabel = new System.Windows.Forms.Label();
			this.bdAddressLabel = new System.Windows.Forms.Label();
			this.packetLabel = new System.Windows.Forms.Label();
			this.payloadLabel = new System.Windows.Forms.Label();
			this.chnNumberNumeric = new System.Windows.Forms.NumericUpDown();
			this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
			this.externalAttnNumeric = new System.Windows.Forms.NumericUpDown();
			this.headroomNumeric = new System.Windows.Forms.NumericUpDown();
			this.quadratureSkewNumeric = new System.Windows.Forms.NumericUpDown();
			this.iDCOffsetNumeric = new System.Windows.Forms.NumericUpDown();
			this.qDCOffsetNumeric = new System.Windows.Forms.NumericUpDown();
			this.iqGaimbalanceNumeric = new System.Windows.Forms.NumericUpDown();
			this.carrierFreqOffNumeric = new System.Windows.Forms.NumericUpDown();
			this.cnrNumeric = new System.Windows.Forms.NumericUpDown();
			this.bdaddrLapNumeric = new System.Windows.Forms.NumericUpDown();
			this.bdaddrUapNumeric = new System.Windows.Forms.NumericUpDown();
			this.bdaddrNapNumeric = new System.Windows.Forms.NumericUpDown();
			this.pktLtAddrNumeric = new System.Windows.Forms.NumericUpDown();
			this.pktHdrFlowNumeric = new System.Windows.Forms.NumericUpDown();
			this.pktHdrSeqnNumeric = new System.Windows.Forms.NumericUpDown();
			this.payHdrLlidNumeric = new System.Windows.Forms.NumericUpDown();
			this.payHdrFlowNumeric = new System.Windows.Forms.NumericUpDown();
			this.payHdrPaylenNumeric = new System.Windows.Forms.NumericUpDown();
			this.paydatPnorderNumeric = new System.Windows.Forms.NumericUpDown();
			this.paydatSeedNumeric = new System.Windows.Forms.NumericUpDown();
			this.rfsgResourceTextBox = new System.Windows.Forms.TextBox();
			this.carrierFreqTextBox = new System.Windows.Forms.TextBox();
			this.actualHeadroomTextBox = new System.Windows.Forms.TextBox();
			this.payHdrActPaylenTextBox = new System.Windows.Forms.TextBox();
			this.errorTextBox = new System.Windows.Forms.TextBox();
			this.generateButton = new System.Windows.Forms.Button();
			this.stopButton = new System.Windows.Forms.Button();
			this.autoHeadroomEnabComboBox = new System.Windows.Forms.ComboBox();
			this.refSourceComboBox = new System.Windows.Forms.ComboBox();
			this.userDefinedBitsComboBox = new System.Windows.Forms.ComboBox();
			this.clkOutTerminalComboBox = new System.Windows.Forms.ComboBox();
			this.allIqImpairEnComboBox = new System.Windows.Forms.ComboBox();
			this.awgnEnabledComboBox = new System.Windows.Forms.ComboBox();
			this.pktHdrArqnComboBox = new System.Windows.Forms.ComboBox();
			this.payHdrPaylenModeComboBox = new System.Windows.Forms.ComboBox();
			this.payHdrDatatypeComboBox = new System.Windows.Forms.ComboBox();
			this.packetComboBox = new System.Windows.Forms.ComboBox();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.numOfUniqNumeric = new System.Windows.Forms.NumericUpDown();
			this.numOfIdleSlotsNumeric = new System.Windows.Forms.NumericUpDown();
			this.numOfUniqPacketsLabel = new System.Windows.Forms.Label();
			this.numOfIdleSlotsLabel = new System.Windows.Forms.Label();
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
			this.whiteEnLabel = new System.Windows.Forms.Label();
			this.whiteClkLabel = new System.Windows.Forms.Label();
			this.whiteningSettingLabel = new System.Windows.Forms.Label();
			this.whiteClkNumeric = new System.Windows.Forms.NumericUpDown();
			this.whiteEnComboBox = new System.Windows.Forms.ComboBox();
			this.dataRateLabel = new System.Windows.Forms.Label();
			this.dataRateNumeric = new System.Windows.Forms.NumericUpDown();
			this.LETPPayloadTypeLabel = new System.Windows.Forms.Label();
			this.LETPPayloadTypeComboBox = new System.Windows.Forms.ComboBox();
			this.LETPCorruptAlternateCRCLabel = new System.Windows.Forms.Label();
			this.LETPCorruptAlternateCRCComboBox = new System.Windows.Forms.ComboBox();
			this.highDataThroughputSettingsLabel = new System.Windows.Forms.Label();
			this.zadoffChuIndexLabel = new System.Windows.Forms.Label();
			this.zadoffChuIndexNumeric = new System.Windows.Forms.NumericUpDown();
			this.physicalChannelAddressLabel = new System.Windows.Forms.Label();
			this.physicalChannelAddressNumeric = new System.Windows.Forms.NumericUpDown();
			this.HdtPacketFormatLabel = new System.Windows.Forms.Label();
			this.HdtPacketFormatComboBox = new System.Windows.Forms.ComboBox();
			this.HdtPhyIntervalLabel = new System.Windows.Forms.Label();
			this.HdtPhyIntervalNumeric = new System.Windows.Forms.NumericUpDown();
			this.dirtyTxSettingsLabel = new System.Windows.Forms.Label();
			this.dirtyTxModeLabel = new System.Windows.Forms.Label();
			this.dirtyTxModeComboBox = new System.Windows.Forms.ComboBox();
			this.paraEnabledDataGridViewLabel = new System.Windows.Forms.Label();
			this.paraEnabledDataGridView = new System.Windows.Forms.DataGridView();
			this.parametersEnabledSetIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.parametersEnabledSet = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.paraEnabledDelete = new System.Windows.Forms.Button();
			this.paraEnabledInsert = new System.Windows.Forms.Button();
			this.carrFreqOffsetDataGridViewLabel = new System.Windows.Forms.Label();
			this.carrFreqOffsetDataGridView = new System.Windows.Forms.DataGridView();
			this.carrFreqOffDelete = new System.Windows.Forms.Button();
			this.carrFreqOffInsert = new System.Windows.Forms.Button();
			this.modulationIndexDataGridViewLabel = new System.Windows.Forms.Label();
			this.modulationIndexDataGridView = new System.Windows.Forms.DataGridView();
			this.modulationIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.modulationIndexSet = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.modulationIndexDelete = new System.Windows.Forms.Button();
			this.modulationIndexInsert = new System.Windows.Forms.Button();
			this.symbolTimingErrorDataGridViewLabel = new System.Windows.Forms.Label();
			this.symbolTimingErrorDataGridView = new System.Windows.Forms.DataGridView();
			this.symbolTimingErrorIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.symbolTimingErrorSet = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.symbolTimingErrorDelete = new System.Windows.Forms.Button();
			this.symbolTimingErrorInsert = new System.Windows.Forms.Button();
			this.dirtyTxModulationIndexTypeLabel = new System.Windows.Forms.Label();
			this.dirtyTxModulationIndexTypeComboBox = new System.Windows.Forms.ComboBox();
			this.carrFreqOffsetIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.carrFreqOffsetSet = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.chnNumberNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.externalAttnNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.headroomNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.quadratureSkewNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.iDCOffsetNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.qDCOffsetNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.iqGaimbalanceNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.carrierFreqOffNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cnrNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bdaddrLapNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bdaddrUapNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bdaddrNapNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pktLtAddrNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pktHdrFlowNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pktHdrSeqnNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.payHdrLlidNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.payHdrFlowNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.payHdrPaylenNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.paydatPnorderNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.paydatSeedNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numOfUniqNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numOfIdleSlotsNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.iOffsetNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.qOffsetNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.iCommonModeOffsetNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.qCommonModeOffsetNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.whiteClkNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataRateNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.zadoffChuIndexNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.physicalChannelAddressNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.HdtPhyIntervalNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.paraEnabledDataGridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.carrFreqOffsetDataGridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.modulationIndexDataGridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.symbolTimingErrorDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// rfsgResourceLabel
			// 
			this.rfsgResourceLabel.AutoSize = true;
			this.rfsgResourceLabel.Location = new System.Drawing.Point(45, 69);
			this.rfsgResourceLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.rfsgResourceLabel.Name = "rfsgResourceLabel";
			this.rfsgResourceLabel.Size = new System.Drawing.Size(220, 32);
			this.rfsgResourceLabel.TabIndex = 0;
			this.rfsgResourceLabel.Text = "RFSG Resource";
			// 
			// chnNumberLabel
			// 
			this.chnNumberLabel.AutoSize = true;
			this.chnNumberLabel.Location = new System.Drawing.Point(45, 112);
			this.chnNumberLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.chnNumberLabel.Name = "chnNumberLabel";
			this.chnNumberLabel.Size = new System.Drawing.Size(228, 32);
			this.chnNumberLabel.TabIndex = 1;
			this.chnNumberLabel.Text = "Channel Number";
			// 
			// carrierFreqLabel
			// 
			this.carrierFreqLabel.AutoSize = true;
			this.carrierFreqLabel.Location = new System.Drawing.Point(51, 162);
			this.carrierFreqLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.carrierFreqLabel.Name = "carrierFreqLabel";
			this.carrierFreqLabel.Size = new System.Drawing.Size(300, 32);
			this.carrierFreqLabel.TabIndex = 2;
			this.carrierFreqLabel.Text = "Carrier Frequency (Hz)";
			// 
			// powerLevelLabel
			// 
			this.powerLevelLabel.AutoSize = true;
			this.powerLevelLabel.Location = new System.Drawing.Point(45, 210);
			this.powerLevelLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.powerLevelLabel.Name = "powerLevelLabel";
			this.powerLevelLabel.Size = new System.Drawing.Size(253, 32);
			this.powerLevelLabel.TabIndex = 3;
			this.powerLevelLabel.Text = "Power Level (dBm)";
			// 
			// externalAttnLabel
			// 
			this.externalAttnLabel.AutoSize = true;
			this.externalAttnLabel.Location = new System.Drawing.Point(45, 262);
			this.externalAttnLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.externalAttnLabel.Name = "externalAttnLabel";
			this.externalAttnLabel.Size = new System.Drawing.Size(332, 32);
			this.externalAttnLabel.TabIndex = 4;
			this.externalAttnLabel.Text = "External Attenuation (dB)";
			// 
			// autoheadroomEnabLabel
			// 
			this.autoheadroomEnabLabel.AutoSize = true;
			this.autoheadroomEnabLabel.Location = new System.Drawing.Point(45, 315);
			this.autoheadroomEnabLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.autoheadroomEnabLabel.Name = "autoheadroomEnabLabel";
			this.autoheadroomEnabLabel.Size = new System.Drawing.Size(325, 32);
			this.autoheadroomEnabLabel.TabIndex = 5;
			this.autoheadroomEnabLabel.Text = "Auto Headroom Enabled";
			// 
			// headroomLabel
			// 
			this.headroomLabel.AutoSize = true;
			this.headroomLabel.Location = new System.Drawing.Point(45, 365);
			this.headroomLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.headroomLabel.Name = "headroomLabel";
			this.headroomLabel.Size = new System.Drawing.Size(206, 32);
			this.headroomLabel.TabIndex = 6;
			this.headroomLabel.Text = "Headroom (dB)";
			// 
			// actualHeadroomLabel
			// 
			this.actualHeadroomLabel.AutoSize = true;
			this.actualHeadroomLabel.Location = new System.Drawing.Point(45, 410);
			this.actualHeadroomLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.actualHeadroomLabel.Name = "actualHeadroomLabel";
			this.actualHeadroomLabel.Size = new System.Drawing.Size(293, 32);
			this.actualHeadroomLabel.TabIndex = 7;
			this.actualHeadroomLabel.Text = "Actual Headroom (dB)";
			// 
			// refSourceLabel
			// 
			this.refSourceLabel.AutoSize = true;
			this.refSourceLabel.Location = new System.Drawing.Point(43, 548);
			this.refSourceLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.refSourceLabel.Name = "refSourceLabel";
			this.refSourceLabel.Size = new System.Drawing.Size(242, 32);
			this.refSourceLabel.TabIndex = 8;
			this.refSourceLabel.Text = "Reference Source";
			// 
			// clkOutTerminalLabel
			// 
			this.clkOutTerminalLabel.AutoSize = true;
			this.clkOutTerminalLabel.Location = new System.Drawing.Point(43, 599);
			this.clkOutTerminalLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.clkOutTerminalLabel.Name = "clkOutTerminalLabel";
			this.clkOutTerminalLabel.Size = new System.Drawing.Size(174, 32);
			this.clkOutTerminalLabel.TabIndex = 9;
			this.clkOutTerminalLabel.Text = "Export Clock";
			// 
			// allIqImpairEnLabel
			// 
			this.allIqImpairEnLabel.AutoSize = true;
			this.allIqImpairEnLabel.Location = new System.Drawing.Point(43, 713);
			this.allIqImpairEnLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.allIqImpairEnLabel.Name = "allIqImpairEnLabel";
			this.allIqImpairEnLabel.Size = new System.Drawing.Size(358, 32);
			this.allIqImpairEnLabel.TabIndex = 10;
			this.allIqImpairEnLabel.Text = "All IQ Impairments Enabled";
			// 
			// quadratureSkewLabel
			// 
			this.quadratureSkewLabel.AutoSize = true;
			this.quadratureSkewLabel.Location = new System.Drawing.Point(43, 765);
			this.quadratureSkewLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.quadratureSkewLabel.Name = "quadratureSkewLabel";
			this.quadratureSkewLabel.Size = new System.Drawing.Size(307, 32);
			this.quadratureSkewLabel.TabIndex = 11;
			this.quadratureSkewLabel.Text = "Quadrature Skew (deg)";
			// 
			// iDCOffsetLabel
			// 
			this.iDCOffsetLabel.AutoSize = true;
			this.iDCOffsetLabel.Location = new System.Drawing.Point(43, 818);
			this.iDCOffsetLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.iDCOffsetLabel.Name = "iDCOffsetLabel";
			this.iDCOffsetLabel.Size = new System.Drawing.Size(201, 32);
			this.iDCOffsetLabel.TabIndex = 12;
			this.iDCOffsetLabel.Text = "I DC Offset (%)";
			// 
			// qDCOffsetLabel
			// 
			this.qDCOffsetLabel.AutoSize = true;
			this.qDCOffsetLabel.Location = new System.Drawing.Point(43, 870);
			this.qDCOffsetLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.qDCOffsetLabel.Name = "qDCOffsetLabel";
			this.qDCOffsetLabel.Size = new System.Drawing.Size(216, 32);
			this.qDCOffsetLabel.TabIndex = 13;
			this.qDCOffsetLabel.Text = "Q DC Offset (%)";
			// 
			// iqGaimbalanceLabel
			// 
			this.iqGaimbalanceLabel.AutoSize = true;
			this.iqGaimbalanceLabel.Location = new System.Drawing.Point(43, 923);
			this.iqGaimbalanceLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.iqGaimbalanceLabel.Name = "iqGaimbalanceLabel";
			this.iqGaimbalanceLabel.Size = new System.Drawing.Size(309, 32);
			this.iqGaimbalanceLabel.TabIndex = 14;
			this.iqGaimbalanceLabel.Text = "IQ Gain Imbalance (dB)";
			// 
			// carrierFreqOffLabel
			// 
			this.carrierFreqOffLabel.AutoSize = true;
			this.carrierFreqOffLabel.Location = new System.Drawing.Point(43, 971);
			this.carrierFreqOffLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.carrierFreqOffLabel.Name = "carrierFreqOffLabel";
			this.carrierFreqOffLabel.Size = new System.Drawing.Size(383, 32);
			this.carrierFreqOffLabel.TabIndex = 15;
			this.carrierFreqOffLabel.Text = "Carrier Frequency Offset (Hz)";
			// 
			// awgnEnabledLabel
			// 
			this.awgnEnabledLabel.AutoSize = true;
			this.awgnEnabledLabel.Location = new System.Drawing.Point(43, 1021);
			this.awgnEnabledLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.awgnEnabledLabel.Name = "awgnEnabledLabel";
			this.awgnEnabledLabel.Size = new System.Drawing.Size(214, 32);
			this.awgnEnabledLabel.TabIndex = 16;
			this.awgnEnabledLabel.Text = "AWGN Enabled";
			// 
			// cnrLabel
			// 
			this.cnrLabel.AutoSize = true;
			this.cnrLabel.Location = new System.Drawing.Point(43, 1068);
			this.cnrLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.cnrLabel.Name = "cnrLabel";
			this.cnrLabel.Size = new System.Drawing.Size(345, 32);
			this.cnrLabel.TabIndex = 17;
			this.cnrLabel.Text = "Carrier to Noise Ratio (dB)";
			// 
			// bdaddrLapLabel
			// 
			this.bdaddrLapLabel.AutoSize = true;
			this.bdaddrLapLabel.Location = new System.Drawing.Point(747, 57);
			this.bdaddrLapLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.bdaddrLapLabel.Name = "bdaddrLapLabel";
			this.bdaddrLapLabel.Size = new System.Drawing.Size(68, 32);
			this.bdaddrLapLabel.TabIndex = 18;
			this.bdaddrLapLabel.Text = "LAP";
			// 
			// bdaddrUapLabel
			// 
			this.bdaddrUapLabel.AutoSize = true;
			this.bdaddrUapLabel.Location = new System.Drawing.Point(747, 110);
			this.bdaddrUapLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.bdaddrUapLabel.Name = "bdaddrUapLabel";
			this.bdaddrUapLabel.Size = new System.Drawing.Size(72, 32);
			this.bdaddrUapLabel.TabIndex = 19;
			this.bdaddrUapLabel.Text = "UAP";
			// 
			// bdaddrNapLabel
			// 
			this.bdaddrNapLabel.AutoSize = true;
			this.bdaddrNapLabel.Location = new System.Drawing.Point(747, 162);
			this.bdaddrNapLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.bdaddrNapLabel.Name = "bdaddrNapLabel";
			this.bdaddrNapLabel.Size = new System.Drawing.Size(72, 32);
			this.bdaddrNapLabel.TabIndex = 20;
			this.bdaddrNapLabel.Text = "NAP";
			// 
			// pktLtAddrLabel
			// 
			this.pktLtAddrLabel.AutoSize = true;
			this.pktLtAddrLabel.Location = new System.Drawing.Point(747, 596);
			this.pktLtAddrLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.pktLtAddrLabel.Name = "pktLtAddrLabel";
			this.pktLtAddrLabel.Size = new System.Drawing.Size(158, 32);
			this.pktLtAddrLabel.TabIndex = 21;
			this.pktLtAddrLabel.Text = "LT Address";
			// 
			// pktHdrFlowLabel
			// 
			this.pktHdrFlowLabel.AutoSize = true;
			this.pktHdrFlowLabel.Location = new System.Drawing.Point(747, 641);
			this.pktHdrFlowLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.pktHdrFlowLabel.Name = "pktHdrFlowLabel";
			this.pktHdrFlowLabel.Size = new System.Drawing.Size(74, 32);
			this.pktHdrFlowLabel.TabIndex = 22;
			this.pktHdrFlowLabel.Text = "Flow";
			// 
			// pktHdrArqnLabel
			// 
			this.pktHdrArqnLabel.AutoSize = true;
			this.pktHdrArqnLabel.Location = new System.Drawing.Point(747, 687);
			this.pktHdrArqnLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.pktHdrArqnLabel.Name = "pktHdrArqnLabel";
			this.pktHdrArqnLabel.Size = new System.Drawing.Size(95, 32);
			this.pktHdrArqnLabel.TabIndex = 23;
			this.pktHdrArqnLabel.Text = "ARQN";
			// 
			// pktHdrSeqnLabel
			// 
			this.pktHdrSeqnLabel.AutoSize = true;
			this.pktHdrSeqnLabel.Location = new System.Drawing.Point(747, 732);
			this.pktHdrSeqnLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.pktHdrSeqnLabel.Name = "pktHdrSeqnLabel";
			this.pktHdrSeqnLabel.Size = new System.Drawing.Size(94, 32);
			this.pktHdrSeqnLabel.TabIndex = 24;
			this.pktHdrSeqnLabel.Text = "SEQN";
			// 
			// payHdrLlidLabel
			// 
			this.payHdrLlidLabel.AutoSize = true;
			this.payHdrLlidLabel.Location = new System.Drawing.Point(747, 890);
			this.payHdrLlidLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.payHdrLlidLabel.Name = "payHdrLlidLabel";
			this.payHdrLlidLabel.Size = new System.Drawing.Size(73, 32);
			this.payHdrLlidLabel.TabIndex = 25;
			this.payHdrLlidLabel.Text = "LLID";
			// 
			// payHdrFlowLabel
			// 
			this.payHdrFlowLabel.AutoSize = true;
			this.payHdrFlowLabel.Location = new System.Drawing.Point(747, 942);
			this.payHdrFlowLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.payHdrFlowLabel.Name = "payHdrFlowLabel";
			this.payHdrFlowLabel.Size = new System.Drawing.Size(74, 32);
			this.payHdrFlowLabel.TabIndex = 26;
			this.payHdrFlowLabel.Text = "Flow";
			// 
			// userDefinedBitLabel
			// 
			this.userDefinedBitLabel.AutoSize = true;
			this.userDefinedBitLabel.Location = new System.Drawing.Point(720, 1362);
			this.userDefinedBitLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.userDefinedBitLabel.Name = "userDefinedBitLabel";
			this.userDefinedBitLabel.Size = new System.Drawing.Size(319, 32);
			this.userDefinedBitLabel.TabIndex = 27;
			this.userDefinedBitLabel.Text = "User Defined Bit Pattern";
			// 
			// payHdrPaylenModeLabel
			// 
			this.payHdrPaylenModeLabel.AutoSize = true;
			this.payHdrPaylenModeLabel.Location = new System.Drawing.Point(747, 995);
			this.payHdrPaylenModeLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.payHdrPaylenModeLabel.Name = "payHdrPaylenModeLabel";
			this.payHdrPaylenModeLabel.Size = new System.Drawing.Size(291, 32);
			this.payHdrPaylenModeLabel.TabIndex = 27;
			this.payHdrPaylenModeLabel.Text = "Payload Length Mode";
			// 
			// payHdrPaylenLabel
			// 
			this.payHdrPaylenLabel.AutoSize = true;
			this.payHdrPaylenLabel.Location = new System.Drawing.Point(747, 1042);
			this.payHdrPaylenLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.payHdrPaylenLabel.Name = "payHdrPaylenLabel";
			this.payHdrPaylenLabel.Size = new System.Drawing.Size(213, 32);
			this.payHdrPaylenLabel.TabIndex = 28;
			this.payHdrPaylenLabel.Text = "Payload Length";
			// 
			// payHdrActPaylenLabel
			// 
			this.payHdrActPaylenLabel.AutoSize = true;
			this.payHdrActPaylenLabel.Location = new System.Drawing.Point(747, 1095);
			this.payHdrActPaylenLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.payHdrActPaylenLabel.Name = "payHdrActPaylenLabel";
			this.payHdrActPaylenLabel.Size = new System.Drawing.Size(300, 32);
			this.payHdrActPaylenLabel.TabIndex = 29;
			this.payHdrActPaylenLabel.Text = "Actual Payload Length";
			// 
			// payHdrDatatypeLabel
			// 
			this.payHdrDatatypeLabel.AutoSize = true;
			this.payHdrDatatypeLabel.Location = new System.Drawing.Point(749, 1200);
			this.payHdrDatatypeLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.payHdrDatatypeLabel.Name = "payHdrDatatypeLabel";
			this.payHdrDatatypeLabel.Size = new System.Drawing.Size(144, 32);
			this.payHdrDatatypeLabel.TabIndex = 30;
			this.payHdrDatatypeLabel.Text = "Data Type";
			// 
			// paydatPnorderLabel
			// 
			this.paydatPnorderLabel.AutoSize = true;
			this.paydatPnorderLabel.Location = new System.Drawing.Point(749, 1259);
			this.paydatPnorderLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.paydatPnorderLabel.Name = "paydatPnorderLabel";
			this.paydatPnorderLabel.Size = new System.Drawing.Size(132, 32);
			this.paydatPnorderLabel.TabIndex = 31;
			this.paydatPnorderLabel.Text = "PN Order";
			// 
			// paydatSeedLabel
			// 
			this.paydatSeedLabel.AutoSize = true;
			this.paydatSeedLabel.Location = new System.Drawing.Point(747, 1307);
			this.paydatSeedLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.paydatSeedLabel.Name = "paydatSeedLabel";
			this.paydatSeedLabel.Size = new System.Drawing.Size(81, 32);
			this.paydatSeedLabel.TabIndex = 32;
			this.paydatSeedLabel.Text = "Seed";
			// 
			// errorLabel
			// 
			this.errorLabel.AutoSize = true;
			this.errorLabel.Location = new System.Drawing.Point(1390, 1021);
			this.errorLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.errorLabel.Name = "errorLabel";
			this.errorLabel.Size = new System.Drawing.Size(76, 32);
			this.errorLabel.TabIndex = 33;
			this.errorLabel.Text = "Error";
			// 
			// extmsgPacket22Label
			// 
			this.extmsgPacket22Label.AutoSize = true;
			this.extmsgPacket22Label.Location = new System.Drawing.Point(803, 548);
			this.extmsgPacket22Label.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.extmsgPacket22Label.Name = "extmsgPacket22Label";
			this.extmsgPacket22Label.Size = new System.Drawing.Size(107, 32);
			this.extmsgPacket22Label.TabIndex = 34;
			this.extmsgPacket22Label.Text = "Header";
			// 
			// textmsgPayloadHdrLabel
			// 
			this.textmsgPayloadHdrLabel.AutoSize = true;
			this.textmsgPayloadHdrLabel.Location = new System.Drawing.Point(747, 842);
			this.textmsgPayloadHdrLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.textmsgPayloadHdrLabel.Name = "textmsgPayloadHdrLabel";
			this.textmsgPayloadHdrLabel.Size = new System.Drawing.Size(107, 32);
			this.textmsgPayloadHdrLabel.TabIndex = 35;
			this.textmsgPayloadHdrLabel.Text = "Header";
			// 
			// payloadDataLabel
			// 
			this.payloadDataLabel.AutoSize = true;
			this.payloadDataLabel.Location = new System.Drawing.Point(747, 1157);
			this.payloadDataLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.payloadDataLabel.Name = "payloadDataLabel";
			this.payloadDataLabel.Size = new System.Drawing.Size(74, 32);
			this.payloadDataLabel.TabIndex = 36;
			this.payloadDataLabel.Text = "Data";
			// 
			// hardwareLabel
			// 
			this.hardwareLabel.AutoSize = true;
			this.hardwareLabel.Location = new System.Drawing.Point(80, 14);
			this.hardwareLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.hardwareLabel.Name = "hardwareLabel";
			this.hardwareLabel.Size = new System.Drawing.Size(136, 32);
			this.hardwareLabel.TabIndex = 37;
			this.hardwareLabel.Text = "Hardware";
			// 
			// frequencySettingLabel
			// 
			this.frequencySettingLabel.AutoSize = true;
			this.frequencySettingLabel.Location = new System.Drawing.Point(77, 506);
			this.frequencySettingLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.frequencySettingLabel.Name = "frequencySettingLabel";
			this.frequencySettingLabel.Size = new System.Drawing.Size(259, 32);
			this.frequencySettingLabel.TabIndex = 38;
			this.frequencySettingLabel.Text = "Frequency Settings";
			// 
			// impairmentsLabel
			// 
			this.impairmentsLabel.AutoSize = true;
			this.impairmentsLabel.Location = new System.Drawing.Point(77, 663);
			this.impairmentsLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.impairmentsLabel.Name = "impairmentsLabel";
			this.impairmentsLabel.Size = new System.Drawing.Size(169, 32);
			this.impairmentsLabel.TabIndex = 39;
			this.impairmentsLabel.Text = "Impairments";
			// 
			// bdAddressLabel
			// 
			this.bdAddressLabel.AutoSize = true;
			this.bdAddressLabel.Location = new System.Drawing.Point(803, 14);
			this.bdAddressLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.bdAddressLabel.Name = "bdAddressLabel";
			this.bdAddressLabel.Size = new System.Drawing.Size(164, 32);
			this.bdAddressLabel.TabIndex = 40;
			this.bdAddressLabel.Text = "BD Address";
			// 
			// packetLabel
			// 
			this.packetLabel.AutoSize = true;
			this.packetLabel.Location = new System.Drawing.Point(747, 219);
			this.packetLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.packetLabel.Name = "packetLabel";
			this.packetLabel.Size = new System.Drawing.Size(101, 32);
			this.packetLabel.TabIndex = 41;
			this.packetLabel.Text = "Packet";
			// 
			// payloadLabel
			// 
			this.payloadLabel.AutoSize = true;
			this.payloadLabel.Location = new System.Drawing.Point(803, 799);
			this.payloadLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.payloadLabel.Name = "payloadLabel";
			this.payloadLabel.Size = new System.Drawing.Size(118, 32);
			this.payloadLabel.TabIndex = 42;
			this.payloadLabel.Text = "Payload";
			// 
			// chnNumberNumeric
			// 
			this.chnNumberNumeric.Location = new System.Drawing.Point(432, 103);
			this.chnNumberNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.chnNumberNumeric.Name = "chnNumberNumeric";
			this.chnNumberNumeric.Size = new System.Drawing.Size(232, 38);
			this.chnNumberNumeric.TabIndex = 1;
			// 
			// powerLevelNumeric
			// 
			this.powerLevelNumeric.Location = new System.Drawing.Point(432, 203);
			this.powerLevelNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.powerLevelNumeric.Name = "powerLevelNumeric";
			this.powerLevelNumeric.Size = new System.Drawing.Size(232, 38);
			this.powerLevelNumeric.TabIndex = 3;
			// 
			// externalAttnNumeric
			// 
			this.externalAttnNumeric.Location = new System.Drawing.Point(432, 255);
			this.externalAttnNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.externalAttnNumeric.Name = "externalAttnNumeric";
			this.externalAttnNumeric.Size = new System.Drawing.Size(232, 38);
			this.externalAttnNumeric.TabIndex = 4;
			// 
			// headroomNumeric
			// 
			this.headroomNumeric.Location = new System.Drawing.Point(432, 358);
			this.headroomNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.headroomNumeric.Name = "headroomNumeric";
			this.headroomNumeric.Size = new System.Drawing.Size(232, 38);
			this.headroomNumeric.TabIndex = 6;
			// 
			// quadratureSkewNumeric
			// 
			this.quadratureSkewNumeric.Location = new System.Drawing.Point(429, 763);
			this.quadratureSkewNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.quadratureSkewNumeric.Name = "quadratureSkewNumeric";
			this.quadratureSkewNumeric.Size = new System.Drawing.Size(240, 38);
			this.quadratureSkewNumeric.TabIndex = 11;
			// 
			// iDCOffsetNumeric
			// 
			this.iDCOffsetNumeric.Location = new System.Drawing.Point(429, 816);
			this.iDCOffsetNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.iDCOffsetNumeric.Name = "iDCOffsetNumeric";
			this.iDCOffsetNumeric.Size = new System.Drawing.Size(240, 38);
			this.iDCOffsetNumeric.TabIndex = 12;
			// 
			// qDCOffsetNumeric
			// 
			this.qDCOffsetNumeric.Location = new System.Drawing.Point(429, 868);
			this.qDCOffsetNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.qDCOffsetNumeric.Name = "qDCOffsetNumeric";
			this.qDCOffsetNumeric.Size = new System.Drawing.Size(240, 38);
			this.qDCOffsetNumeric.TabIndex = 13;
			// 
			// iqGaimbalanceNumeric
			// 
			this.iqGaimbalanceNumeric.Location = new System.Drawing.Point(429, 920);
			this.iqGaimbalanceNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.iqGaimbalanceNumeric.Name = "iqGaimbalanceNumeric";
			this.iqGaimbalanceNumeric.Size = new System.Drawing.Size(240, 38);
			this.iqGaimbalanceNumeric.TabIndex = 14;
			// 
			// carrierFreqOffNumeric
			// 
			this.carrierFreqOffNumeric.Location = new System.Drawing.Point(429, 966);
			this.carrierFreqOffNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.carrierFreqOffNumeric.Name = "carrierFreqOffNumeric";
			this.carrierFreqOffNumeric.Size = new System.Drawing.Size(240, 38);
			this.carrierFreqOffNumeric.TabIndex = 15;
			// 
			// cnrNumeric
			// 
			this.cnrNumeric.Location = new System.Drawing.Point(429, 1066);
			this.cnrNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.cnrNumeric.Name = "cnrNumeric";
			this.cnrNumeric.Size = new System.Drawing.Size(240, 38);
			this.cnrNumeric.TabIndex = 17;
			// 
			// bdaddrLapNumeric
			// 
			this.bdaddrLapNumeric.Location = new System.Drawing.Point(1059, 50);
			this.bdaddrLapNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.bdaddrLapNumeric.Name = "bdaddrLapNumeric";
			this.bdaddrLapNumeric.Size = new System.Drawing.Size(240, 38);
			this.bdaddrLapNumeric.TabIndex = 18;
			// 
			// bdaddrUapNumeric
			// 
			this.bdaddrUapNumeric.Location = new System.Drawing.Point(1059, 105);
			this.bdaddrUapNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.bdaddrUapNumeric.Name = "bdaddrUapNumeric";
			this.bdaddrUapNumeric.Size = new System.Drawing.Size(240, 38);
			this.bdaddrUapNumeric.TabIndex = 19;
			// 
			// bdaddrNapNumeric
			// 
			this.bdaddrNapNumeric.Location = new System.Drawing.Point(1059, 150);
			this.bdaddrNapNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.bdaddrNapNumeric.Name = "bdaddrNapNumeric";
			this.bdaddrNapNumeric.Size = new System.Drawing.Size(240, 38);
			this.bdaddrNapNumeric.TabIndex = 20;
			// 
			// pktLtAddrNumeric
			// 
			this.pktLtAddrNumeric.Location = new System.Drawing.Point(1059, 592);
			this.pktLtAddrNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.pktLtAddrNumeric.Name = "pktLtAddrNumeric";
			this.pktLtAddrNumeric.Size = new System.Drawing.Size(240, 38);
			this.pktLtAddrNumeric.TabIndex = 22;
			// 
			// pktHdrFlowNumeric
			// 
			this.pktHdrFlowNumeric.Location = new System.Drawing.Point(1059, 639);
			this.pktHdrFlowNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.pktHdrFlowNumeric.Name = "pktHdrFlowNumeric";
			this.pktHdrFlowNumeric.Size = new System.Drawing.Size(240, 38);
			this.pktHdrFlowNumeric.TabIndex = 23;
			// 
			// pktHdrSeqnNumeric
			// 
			this.pktHdrSeqnNumeric.Location = new System.Drawing.Point(1059, 730);
			this.pktHdrSeqnNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.pktHdrSeqnNumeric.Name = "pktHdrSeqnNumeric";
			this.pktHdrSeqnNumeric.Size = new System.Drawing.Size(240, 38);
			this.pktHdrSeqnNumeric.TabIndex = 25;
			// 
			// payHdrLlidNumeric
			// 
			this.payHdrLlidNumeric.Location = new System.Drawing.Point(1061, 887);
			this.payHdrLlidNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.payHdrLlidNumeric.Name = "payHdrLlidNumeric";
			this.payHdrLlidNumeric.Size = new System.Drawing.Size(240, 38);
			this.payHdrLlidNumeric.TabIndex = 26;
			// 
			// payHdrFlowNumeric
			// 
			this.payHdrFlowNumeric.Location = new System.Drawing.Point(1061, 940);
			this.payHdrFlowNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.payHdrFlowNumeric.Name = "payHdrFlowNumeric";
			this.payHdrFlowNumeric.Size = new System.Drawing.Size(240, 38);
			this.payHdrFlowNumeric.TabIndex = 27;
			// 
			// payHdrPaylenNumeric
			// 
			this.payHdrPaylenNumeric.Location = new System.Drawing.Point(1061, 1040);
			this.payHdrPaylenNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.payHdrPaylenNumeric.Name = "payHdrPaylenNumeric";
			this.payHdrPaylenNumeric.Size = new System.Drawing.Size(243, 38);
			this.payHdrPaylenNumeric.TabIndex = 29;
			// 
			// paydatPnorderNumeric
			// 
			this.paydatPnorderNumeric.Location = new System.Drawing.Point(1059, 1248);
			this.paydatPnorderNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.paydatPnorderNumeric.Name = "paydatPnorderNumeric";
			this.paydatPnorderNumeric.Size = new System.Drawing.Size(240, 38);
			this.paydatPnorderNumeric.TabIndex = 32;
			// 
			// paydatSeedNumeric
			// 
			this.paydatSeedNumeric.Location = new System.Drawing.Point(1059, 1300);
			this.paydatSeedNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.paydatSeedNumeric.Name = "paydatSeedNumeric";
			this.paydatSeedNumeric.Size = new System.Drawing.Size(240, 38);
			this.paydatSeedNumeric.TabIndex = 33;
			// 
			// rfsgResourceTextBox
			// 
			this.rfsgResourceTextBox.Location = new System.Drawing.Point(432, 52);
			this.rfsgResourceTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.rfsgResourceTextBox.Name = "rfsgResourceTextBox";
			this.rfsgResourceTextBox.Size = new System.Drawing.Size(233, 38);
			this.rfsgResourceTextBox.TabIndex = 0;
			this.rfsgResourceTextBox.Text = "RFSG";
			// 
			// carrierFreqTextBox
			// 
			this.carrierFreqTextBox.Enabled = false;
			this.carrierFreqTextBox.Location = new System.Drawing.Point(432, 150);
			this.carrierFreqTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.carrierFreqTextBox.Name = "carrierFreqTextBox";
			this.carrierFreqTextBox.Size = new System.Drawing.Size(233, 38);
			this.carrierFreqTextBox.TabIndex = 2;
			this.carrierFreqTextBox.Text = "2.402E+9";
			// 
			// actualHeadroomTextBox
			// 
			this.actualHeadroomTextBox.Enabled = false;
			this.actualHeadroomTextBox.Location = new System.Drawing.Point(432, 403);
			this.actualHeadroomTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.actualHeadroomTextBox.Name = "actualHeadroomTextBox";
			this.actualHeadroomTextBox.Size = new System.Drawing.Size(233, 38);
			this.actualHeadroomTextBox.TabIndex = 7;
			this.actualHeadroomTextBox.Text = "5.00";
			// 
			// payHdrActPaylenTextBox
			// 
			this.payHdrActPaylenTextBox.Enabled = false;
			this.payHdrActPaylenTextBox.Location = new System.Drawing.Point(1064, 1093);
			this.payHdrActPaylenTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.payHdrActPaylenTextBox.Name = "payHdrActPaylenTextBox";
			this.payHdrActPaylenTextBox.Size = new System.Drawing.Size(240, 38);
			this.payHdrActPaylenTextBox.TabIndex = 30;
			this.payHdrActPaylenTextBox.Text = "0";
			// 
			// errorTextBox
			// 
			this.errorTextBox.Location = new System.Drawing.Point(1396, 1072);
			this.errorTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.errorTextBox.Multiline = true;
			this.errorTextBox.Name = "errorTextBox";
			this.errorTextBox.ReadOnly = true;
			this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.errorTextBox.Size = new System.Drawing.Size(463, 417);
			this.errorTextBox.TabIndex = 34;
			this.errorTextBox.TabStop = false;
			this.errorTextBox.Text = "No Error";
			// 
			// generateButton
			// 
			this.generateButton.Location = new System.Drawing.Point(581, 1610);
			this.generateButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.generateButton.Name = "generateButton";
			this.generateButton.Size = new System.Drawing.Size(200, 55);
			this.generateButton.TabIndex = 35;
			this.generateButton.Text = "&Generate";
			this.generateButton.UseVisualStyleBackColor = true;
			this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
			// 
			// stopButton
			// 
			this.stopButton.Location = new System.Drawing.Point(1104, 1610);
			this.stopButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.stopButton.Name = "stopButton";
			this.stopButton.Size = new System.Drawing.Size(200, 55);
			this.stopButton.TabIndex = 36;
			this.stopButton.Text = "&Stop";
			this.stopButton.UseVisualStyleBackColor = true;
			this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
			// 
			// autoHeadroomEnabComboBox
			// 
			this.autoHeadroomEnabComboBox.Location = new System.Drawing.Point(432, 308);
			this.autoHeadroomEnabComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.autoHeadroomEnabComboBox.Name = "autoHeadroomEnabComboBox";
			this.autoHeadroomEnabComboBox.Size = new System.Drawing.Size(233, 39);
			this.autoHeadroomEnabComboBox.TabIndex = 5;
			// 
			// refSourceComboBox
			// 
			this.refSourceComboBox.Location = new System.Drawing.Point(429, 548);
			this.refSourceComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.refSourceComboBox.Name = "refSourceComboBox";
			this.refSourceComboBox.Size = new System.Drawing.Size(240, 39);
			this.refSourceComboBox.TabIndex = 8;
			// 
			// userDefinedBitsComboBox
			// 
			this.userDefinedBitsComboBox.Location = new System.Drawing.Point(1059, 1355);
			this.userDefinedBitsComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.userDefinedBitsComboBox.Name = "userDefinedBitsComboBox";
			this.userDefinedBitsComboBox.Size = new System.Drawing.Size(240, 39);
			this.userDefinedBitsComboBox.TabIndex = 8;
			// 
			// clkOutTerminalComboBox
			// 
			this.clkOutTerminalComboBox.Location = new System.Drawing.Point(429, 599);
			this.clkOutTerminalComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.clkOutTerminalComboBox.Name = "clkOutTerminalComboBox";
			this.clkOutTerminalComboBox.Size = new System.Drawing.Size(240, 39);
			this.clkOutTerminalComboBox.TabIndex = 9;
			// 
			// allIqImpairEnComboBox
			// 
			this.allIqImpairEnComboBox.Location = new System.Drawing.Point(429, 711);
			this.allIqImpairEnComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.allIqImpairEnComboBox.Name = "allIqImpairEnComboBox";
			this.allIqImpairEnComboBox.Size = new System.Drawing.Size(240, 39);
			this.allIqImpairEnComboBox.TabIndex = 10;
			// 
			// awgnEnabledComboBox
			// 
			this.awgnEnabledComboBox.Location = new System.Drawing.Point(429, 1018);
			this.awgnEnabledComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.awgnEnabledComboBox.Name = "awgnEnabledComboBox";
			this.awgnEnabledComboBox.Size = new System.Drawing.Size(240, 39);
			this.awgnEnabledComboBox.TabIndex = 16;
			// 
			// pktHdrArqnComboBox
			// 
			this.pktHdrArqnComboBox.Location = new System.Drawing.Point(1059, 684);
			this.pktHdrArqnComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.pktHdrArqnComboBox.Name = "pktHdrArqnComboBox";
			this.pktHdrArqnComboBox.Size = new System.Drawing.Size(240, 39);
			this.pktHdrArqnComboBox.TabIndex = 24;
			// 
			// payHdrPaylenModeComboBox
			// 
			this.payHdrPaylenModeComboBox.Location = new System.Drawing.Point(1061, 992);
			this.payHdrPaylenModeComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.payHdrPaylenModeComboBox.Name = "payHdrPaylenModeComboBox";
			this.payHdrPaylenModeComboBox.Size = new System.Drawing.Size(243, 39);
			this.payHdrPaylenModeComboBox.TabIndex = 28;
			// 
			// payHdrDatatypeComboBox
			// 
			this.payHdrDatatypeComboBox.Location = new System.Drawing.Point(1061, 1200);
			this.payHdrDatatypeComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.payHdrDatatypeComboBox.Name = "payHdrDatatypeComboBox";
			this.payHdrDatatypeComboBox.Size = new System.Drawing.Size(238, 39);
			this.payHdrDatatypeComboBox.TabIndex = 31;
			// 
			// packetComboBox
			// 
			this.packetComboBox.Location = new System.Drawing.Point(1059, 212);
			this.packetComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.packetComboBox.Name = "packetComboBox";
			this.packetComboBox.Size = new System.Drawing.Size(240, 39);
			this.packetComboBox.TabIndex = 28;
			this.packetComboBox.Text = "DH1";
			// 
			// timer
			// 
			this.timer.Tick += new System.EventHandler(this.ProcessTimerEvent);
			// 
			// numOfUniqNumeric
			// 
			this.numOfUniqNumeric.Location = new System.Drawing.Point(1059, 319);
			this.numOfUniqNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.numOfUniqNumeric.Name = "numOfUniqNumeric";
			this.numOfUniqNumeric.Size = new System.Drawing.Size(240, 38);
			this.numOfUniqNumeric.TabIndex = 43;
			this.numOfUniqNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// numOfIdleSlotsNumeric
			// 
			this.numOfIdleSlotsNumeric.Location = new System.Drawing.Point(1059, 369);
			this.numOfIdleSlotsNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.numOfIdleSlotsNumeric.Name = "numOfIdleSlotsNumeric";
			this.numOfIdleSlotsNumeric.Size = new System.Drawing.Size(240, 38);
			this.numOfIdleSlotsNumeric.TabIndex = 44;
			this.numOfIdleSlotsNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// numOfUniqPacketsLabel
			// 
			this.numOfUniqPacketsLabel.AutoSize = true;
			this.numOfUniqPacketsLabel.Location = new System.Drawing.Point(688, 323);
			this.numOfUniqPacketsLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.numOfUniqPacketsLabel.Name = "numOfUniqPacketsLabel";
			this.numOfUniqPacketsLabel.Size = new System.Drawing.Size(357, 32);
			this.numOfUniqPacketsLabel.TabIndex = 45;
			this.numOfUniqPacketsLabel.Text = "Number Of Unique Packets";
			// 
			// numOfIdleSlotsLabel
			// 
			this.numOfIdleSlotsLabel.AutoSize = true;
			this.numOfIdleSlotsLabel.Location = new System.Drawing.Point(688, 374);
			this.numOfIdleSlotsLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.numOfIdleSlotsLabel.Name = "numOfIdleSlotsLabel";
			this.numOfIdleSlotsLabel.Size = new System.Drawing.Size(275, 32);
			this.numOfIdleSlotsLabel.TabIndex = 46;
			this.numOfIdleSlotsLabel.Text = "Number Of Idle Slots";
			// 
			// iOffsetLabel
			// 
			this.iOffsetLabel.AutoSize = true;
			this.iOffsetLabel.Location = new System.Drawing.Point(51, 1285);
			this.iOffsetLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.iOffsetLabel.Name = "iOffsetLabel";
			this.iOffsetLabel.Size = new System.Drawing.Size(141, 32);
			this.iOffsetLabel.TabIndex = 76;
			this.iOffsetLabel.Text = "I Offset(V)";
			// 
			// qOffsetLabel
			// 
			this.qOffsetLabel.AutoSize = true;
			this.qOffsetLabel.Location = new System.Drawing.Point(51, 1338);
			this.qOffsetLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.qOffsetLabel.Name = "qOffsetLabel";
			this.qOffsetLabel.Size = new System.Drawing.Size(156, 32);
			this.qOffsetLabel.TabIndex = 78;
			this.qOffsetLabel.Text = "Q Offset(V)";
			// 
			// iOffsetNumeric
			// 
			this.iOffsetNumeric.Location = new System.Drawing.Point(429, 1281);
			this.iOffsetNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.iOffsetNumeric.Name = "iOffsetNumeric";
			this.iOffsetNumeric.Size = new System.Drawing.Size(240, 38);
			this.iOffsetNumeric.TabIndex = 75;
			// 
			// qOffsetNumeric
			// 
			this.qOffsetNumeric.Location = new System.Drawing.Point(424, 1343);
			this.qOffsetNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.qOffsetNumeric.Name = "qOffsetNumeric";
			this.qOffsetNumeric.Size = new System.Drawing.Size(245, 38);
			this.qOffsetNumeric.TabIndex = 77;
			// 
			// iCommonModeOffsetLabel
			// 
			this.iCommonModeOffsetLabel.AutoSize = true;
			this.iCommonModeOffsetLabel.Location = new System.Drawing.Point(51, 1405);
			this.iCommonModeOffsetLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.iCommonModeOffsetLabel.Name = "iCommonModeOffsetLabel";
			this.iCommonModeOffsetLabel.Size = new System.Drawing.Size(340, 32);
			this.iCommonModeOffsetLabel.TabIndex = 72;
			this.iCommonModeOffsetLabel.Text = "I Common Mode Offset(V)";
			// 
			// qCommonModeOffsetLabel
			// 
			this.qCommonModeOffsetLabel.AutoSize = true;
			this.qCommonModeOffsetLabel.Location = new System.Drawing.Point(51, 1457);
			this.qCommonModeOffsetLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.qCommonModeOffsetLabel.Name = "qCommonModeOffsetLabel";
			this.qCommonModeOffsetLabel.Size = new System.Drawing.Size(355, 32);
			this.qCommonModeOffsetLabel.TabIndex = 74;
			this.qCommonModeOffsetLabel.Text = "Q Common Mode Offset(V)";
			// 
			// iCommonModeOffsetNumeric
			// 
			this.iCommonModeOffsetNumeric.Location = new System.Drawing.Point(429, 1400);
			this.iCommonModeOffsetNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.iCommonModeOffsetNumeric.Name = "iCommonModeOffsetNumeric";
			this.iCommonModeOffsetNumeric.Size = new System.Drawing.Size(240, 38);
			this.iCommonModeOffsetNumeric.TabIndex = 71;
			// 
			// qCommonModeOffsetNumeric
			// 
			this.qCommonModeOffsetNumeric.Location = new System.Drawing.Point(429, 1452);
			this.qCommonModeOffsetNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.qCommonModeOffsetNumeric.Name = "qCommonModeOffsetNumeric";
			this.qCommonModeOffsetNumeric.Size = new System.Drawing.Size(240, 38);
			this.qCommonModeOffsetNumeric.TabIndex = 73;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(45, 1223);
			this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(303, 32);
			this.label2.TabIndex = 70;
			this.label2.Text = "Terminal Configuration";
			// 
			// terminalConfigurationComboBox
			// 
			this.terminalConfigurationComboBox.Location = new System.Drawing.Point(424, 1216);
			this.terminalConfigurationComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.terminalConfigurationComboBox.Name = "terminalConfigurationComboBox";
			this.terminalConfigurationComboBox.Size = new System.Drawing.Size(245, 39);
			this.terminalConfigurationComboBox.TabIndex = 69;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(53, 1157);
			this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(159, 32);
			this.label1.TabIndex = 68;
			this.label1.Text = "Output Port";
			// 
			// outputPortComboBox
			// 
			this.outputPortComboBox.Location = new System.Drawing.Point(424, 1157);
			this.outputPortComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.outputPortComboBox.Name = "outputPortComboBox";
			this.outputPortComboBox.Size = new System.Drawing.Size(245, 39);
			this.outputPortComboBox.TabIndex = 67;
			// 
			// whiteEnLabel
			// 
			this.whiteEnLabel.AutoSize = true;
			this.whiteEnLabel.Location = new System.Drawing.Point(1539, 69);
			this.whiteEnLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.whiteEnLabel.Name = "whiteEnLabel";
			this.whiteEnLabel.Size = new System.Drawing.Size(120, 32);
			this.whiteEnLabel.TabIndex = 79;
			this.whiteEnLabel.Text = "Enabled";
			// 
			// whiteClkLabel
			// 
			this.whiteClkLabel.AutoSize = true;
			this.whiteClkLabel.Location = new System.Drawing.Point(1539, 136);
			this.whiteClkLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.whiteClkLabel.Name = "whiteClkLabel";
			this.whiteClkLabel.Size = new System.Drawing.Size(85, 32);
			this.whiteClkLabel.TabIndex = 81;
			this.whiteClkLabel.Text = "Clock";
			// 
			// whiteningSettingLabel
			// 
			this.whiteningSettingLabel.AutoSize = true;
			this.whiteningSettingLabel.Location = new System.Drawing.Point(1608, 21);
			this.whiteningSettingLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.whiteningSettingLabel.Name = "whiteningSettingLabel";
			this.whiteningSettingLabel.Size = new System.Drawing.Size(253, 32);
			this.whiteningSettingLabel.TabIndex = 83;
			this.whiteningSettingLabel.Text = "Whitening Settings";
			// 
			// whiteClkNumeric
			// 
			this.whiteClkNumeric.Location = new System.Drawing.Point(1686, 131);
			this.whiteClkNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.whiteClkNumeric.Name = "whiteClkNumeric";
			this.whiteClkNumeric.Size = new System.Drawing.Size(240, 38);
			this.whiteClkNumeric.TabIndex = 82;
			// 
			// whiteEnComboBox
			// 
			this.whiteEnComboBox.Location = new System.Drawing.Point(1686, 67);
			this.whiteEnComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.whiteEnComboBox.Name = "whiteEnComboBox";
			this.whiteEnComboBox.Size = new System.Drawing.Size(233, 39);
			this.whiteEnComboBox.TabIndex = 80;
			// 
			// dataRateLabel
			// 
			this.dataRateLabel.AutoSize = true;
			this.dataRateLabel.Location = new System.Drawing.Point(720, 269);
			this.dataRateLabel.Margin = new System.Windows.Forms.Padding(21, 0, 21, 0);
			this.dataRateLabel.Name = "dataRateLabel";
			this.dataRateLabel.Size = new System.Drawing.Size(212, 32);
			this.dataRateLabel.TabIndex = 45;
			this.dataRateLabel.Text = "Data Rate (bps)";
			// 
			// dataRateNumeric
			// 
			this.dataRateNumeric.Location = new System.Drawing.Point(1059, 267);
			this.dataRateNumeric.Margin = new System.Windows.Forms.Padding(21, 17, 21, 17);
			this.dataRateNumeric.Maximum = new decimal(new int[] {
            7500000,
            0,
            0,
            0});
			this.dataRateNumeric.Name = "dataRateNumeric";
			this.dataRateNumeric.Size = new System.Drawing.Size(240, 38);
			this.dataRateNumeric.TabIndex = 43;
			this.dataRateNumeric.Value = new decimal(new int[] {
            2000000,
            0,
            0,
            0});
			// 
			// LETPPayloadTypeLabel
			// 
			this.LETPPayloadTypeLabel.AutoSize = true;
			this.LETPPayloadTypeLabel.Location = new System.Drawing.Point(688, 426);
			this.LETPPayloadTypeLabel.Margin = new System.Windows.Forms.Padding(21, 0, 21, 0);
			this.LETPPayloadTypeLabel.Name = "LETPPayloadTypeLabel";
			this.LETPPayloadTypeLabel.Size = new System.Drawing.Size(275, 32);
			this.LETPPayloadTypeLabel.TabIndex = 41;
			this.LETPPayloadTypeLabel.Text = "LE-TP Payload Type";
			// 
			// LETPPayloadTypeComboBox
			// 
			this.LETPPayloadTypeComboBox.Location = new System.Drawing.Point(1059, 423);
			this.LETPPayloadTypeComboBox.Margin = new System.Windows.Forms.Padding(21, 17, 21, 17);
			this.LETPPayloadTypeComboBox.Name = "LETPPayloadTypeComboBox";
			this.LETPPayloadTypeComboBox.Size = new System.Drawing.Size(240, 39);
			this.LETPPayloadTypeComboBox.TabIndex = 28;
			this.LETPPayloadTypeComboBox.Text = "PRBS9";
			// 
			// LETPCorruptAlternateCRCLabel
			// 
			this.LETPCorruptAlternateCRCLabel.AutoSize = true;
			this.LETPCorruptAlternateCRCLabel.Location = new System.Drawing.Point(671, 480);
			this.LETPCorruptAlternateCRCLabel.Margin = new System.Windows.Forms.Padding(21, 0, 21, 0);
			this.LETPCorruptAlternateCRCLabel.Name = "LETPCorruptAlternateCRCLabel";
			this.LETPCorruptAlternateCRCLabel.Size = new System.Drawing.Size(384, 32);
			this.LETPCorruptAlternateCRCLabel.TabIndex = 41;
			this.LETPCorruptAlternateCRCLabel.Text = "LE-TP Corrupt Alternate CRC";
			// 
			// LETPCorruptAlternateCRCComboBox
			// 
			this.LETPCorruptAlternateCRCComboBox.Location = new System.Drawing.Point(1059, 477);
			this.LETPCorruptAlternateCRCComboBox.Margin = new System.Windows.Forms.Padding(21, 17, 21, 17);
			this.LETPCorruptAlternateCRCComboBox.Name = "LETPCorruptAlternateCRCComboBox";
			this.LETPCorruptAlternateCRCComboBox.Size = new System.Drawing.Size(240, 39);
			this.LETPCorruptAlternateCRCComboBox.TabIndex = 28;
			this.LETPCorruptAlternateCRCComboBox.Text = "False";
			// 
			// highDataThroughputSettingsLabel
			// 
			this.highDataThroughputSettingsLabel.AutoSize = true;
			this.highDataThroughputSettingsLabel.Location = new System.Drawing.Point(1555, 212);
			this.highDataThroughputSettingsLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.highDataThroughputSettingsLabel.Name = "highDataThroughputSettingsLabel";
			this.highDataThroughputSettingsLabel.Size = new System.Drawing.Size(404, 32);
			this.highDataThroughputSettingsLabel.TabIndex = 83;
			this.highDataThroughputSettingsLabel.Text = "High Data Throughput Settings";
			// 
			// zadoffChuIndexLabel
			// 
			this.zadoffChuIndexLabel.AutoSize = true;
			this.zadoffChuIndexLabel.Location = new System.Drawing.Point(1441, 271);
			this.zadoffChuIndexLabel.Margin = new System.Windows.Forms.Padding(21, 0, 21, 0);
			this.zadoffChuIndexLabel.Name = "zadoffChuIndexLabel";
			this.zadoffChuIndexLabel.Size = new System.Drawing.Size(232, 32);
			this.zadoffChuIndexLabel.TabIndex = 45;
			this.zadoffChuIndexLabel.Text = "Zadoff-Chu Index";
			// 
			// zadoffChuIndexNumeric
			// 
			this.zadoffChuIndexNumeric.Location = new System.Drawing.Point(1686, 267);
			this.zadoffChuIndexNumeric.Margin = new System.Windows.Forms.Padding(21, 17, 21, 17);
			this.zadoffChuIndexNumeric.Name = "zadoffChuIndexNumeric";
			this.zadoffChuIndexNumeric.Size = new System.Drawing.Size(240, 38);
			this.zadoffChuIndexNumeric.TabIndex = 43;
			this.zadoffChuIndexNumeric.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
			// 
			// physicalChannelAddressLabel
			// 
			this.physicalChannelAddressLabel.AutoSize = true;
			this.physicalChannelAddressLabel.Location = new System.Drawing.Point(1320, 321);
			this.physicalChannelAddressLabel.Margin = new System.Windows.Forms.Padding(21, 0, 21, 0);
			this.physicalChannelAddressLabel.Name = "physicalChannelAddressLabel";
			this.physicalChannelAddressLabel.Size = new System.Drawing.Size(346, 32);
			this.physicalChannelAddressLabel.TabIndex = 45;
			this.physicalChannelAddressLabel.Text = "Physical Channel Address";
			// 
			// physicalChannelAddressNumeric
			// 
			this.physicalChannelAddressNumeric.Hexadecimal = true;
			this.physicalChannelAddressNumeric.Location = new System.Drawing.Point(1686, 318);
			this.physicalChannelAddressNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.physicalChannelAddressNumeric.Maximum = new decimal(new int[] {
            -727379968,
            232,
            0,
            0});
			this.physicalChannelAddressNumeric.Name = "physicalChannelAddressNumeric";
			this.physicalChannelAddressNumeric.Size = new System.Drawing.Size(240, 38);
			this.physicalChannelAddressNumeric.TabIndex = 43;
			this.physicalChannelAddressNumeric.Value = new decimal(new int[] {
            357913941,
            159,
            0,
            0});
			// 
			// HdtPacketFormatLabel
			// 
			this.HdtPacketFormatLabel.AutoSize = true;
			this.HdtPacketFormatLabel.Location = new System.Drawing.Point(1405, 369);
			this.HdtPacketFormatLabel.Margin = new System.Windows.Forms.Padding(21, 0, 21, 0);
			this.HdtPacketFormatLabel.Name = "HdtPacketFormatLabel";
			this.HdtPacketFormatLabel.Size = new System.Drawing.Size(261, 32);
			this.HdtPacketFormatLabel.TabIndex = 41;
			this.HdtPacketFormatLabel.Text = "HDT Packet Format";
			// 
			// HdtPacketFormatComboBox
			// 
			this.HdtPacketFormatComboBox.Location = new System.Drawing.Point(1686, 366);
			this.HdtPacketFormatComboBox.Margin = new System.Windows.Forms.Padding(21, 17, 21, 17);
			this.HdtPacketFormatComboBox.Name = "HdtPacketFormatComboBox";
			this.HdtPacketFormatComboBox.Size = new System.Drawing.Size(240, 39);
			this.HdtPacketFormatComboBox.TabIndex = 28;
			this.HdtPacketFormatComboBox.Text = "Format0";
			// 
			// HdtPhyIntervalLabel
			// 
			this.HdtPhyIntervalLabel.AutoSize = true;
			this.HdtPhyIntervalLabel.Location = new System.Drawing.Point(1398, 418);
			this.HdtPhyIntervalLabel.Margin = new System.Windows.Forms.Padding(21, 0, 21, 0);
			this.HdtPhyIntervalLabel.Name = "HdtPhyIntervalLabel";
			this.HdtPhyIntervalLabel.Size = new System.Drawing.Size(275, 32);
			this.HdtPhyIntervalLabel.TabIndex = 45;
			this.HdtPhyIntervalLabel.Text = "HDT PHY Interval (s)";
			// 
			// HdtPhyIntervalNumeric
			// 
			this.HdtPhyIntervalNumeric.DecimalPlaces = 10;
			this.HdtPhyIntervalNumeric.Location = new System.Drawing.Point(1686, 416);
			this.HdtPhyIntervalNumeric.Margin = new System.Windows.Forms.Padding(21, 17, 21, 17);
			this.HdtPhyIntervalNumeric.Name = "HdtPhyIntervalNumeric";
			this.HdtPhyIntervalNumeric.Size = new System.Drawing.Size(240, 38);
			this.HdtPhyIntervalNumeric.TabIndex = 43;
			this.HdtPhyIntervalNumeric.Value = new decimal(new int[] {
            64,
            0,
            0,
            393216});
			// 
			// dirtyTxSettingsLabel
			// 
			this.dirtyTxSettingsLabel.AutoSize = true;
			this.dirtyTxSettingsLabel.Location = new System.Drawing.Point(1608, 497);
			this.dirtyTxSettingsLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.dirtyTxSettingsLabel.Name = "dirtyTxSettingsLabel";
			this.dirtyTxSettingsLabel.Size = new System.Drawing.Size(221, 32);
			this.dirtyTxSettingsLabel.TabIndex = 83;
			this.dirtyTxSettingsLabel.Text = "Dirty Tx Settings";
			// 
			// dirtyTxModeLabel
			// 
			this.dirtyTxModeLabel.AutoSize = true;
			this.dirtyTxModeLabel.Location = new System.Drawing.Point(1471, 548);
			this.dirtyTxModeLabel.Margin = new System.Windows.Forms.Padding(21, 0, 21, 0);
			this.dirtyTxModeLabel.Name = "dirtyTxModeLabel";
			this.dirtyTxModeLabel.Size = new System.Drawing.Size(188, 32);
			this.dirtyTxModeLabel.TabIndex = 41;
			this.dirtyTxModeLabel.Text = "Dirty Tx Mode";
			// 
			// dirtyTxModeComboBox
			// 
			this.dirtyTxModeComboBox.Location = new System.Drawing.Point(1686, 547);
			this.dirtyTxModeComboBox.Margin = new System.Windows.Forms.Padding(21, 17, 21, 17);
			this.dirtyTxModeComboBox.Name = "dirtyTxModeComboBox";
			this.dirtyTxModeComboBox.Size = new System.Drawing.Size(240, 39);
			this.dirtyTxModeComboBox.TabIndex = 28;
			this.dirtyTxModeComboBox.Text = "Standard";
			// 
			// paraEnabledDataGridViewLabel
			// 
			this.paraEnabledDataGridViewLabel.AutoSize = true;
			this.paraEnabledDataGridViewLabel.Location = new System.Drawing.Point(1555, 611);
			this.paraEnabledDataGridViewLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.paraEnabledDataGridViewLabel.Name = "paraEnabledDataGridViewLabel";
			this.paraEnabledDataGridViewLabel.Size = new System.Drawing.Size(339, 32);
			this.paraEnabledDataGridViewLabel.TabIndex = 105;
			this.paraEnabledDataGridViewLabel.Text = "Parameters Enabled Set[]";
			// 
			// paraEnabledDataGridView
			// 
			this.paraEnabledDataGridView.AllowUserToAddRows = false;
			this.paraEnabledDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.paraEnabledDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.parametersEnabledSetIndex,
            this.parametersEnabledSet});
			this.paraEnabledDataGridView.Location = new System.Drawing.Point(1499, 657);
			this.paraEnabledDataGridView.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.paraEnabledDataGridView.Name = "paraEnabledDataGridView";
			this.paraEnabledDataGridView.RowHeadersWidth = 102;
			this.paraEnabledDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.paraEnabledDataGridView.Size = new System.Drawing.Size(427, 258);
			this.paraEnabledDataGridView.TabIndex = 106;
			// 
			// parametersEnabledSetIndex
			// 
			this.parametersEnabledSetIndex.Frozen = true;
			this.parametersEnabledSetIndex.HeaderText = "Index";
			this.parametersEnabledSetIndex.MinimumWidth = 12;
			this.parametersEnabledSetIndex.Name = "parametersEnabledSetIndex";
			this.parametersEnabledSetIndex.ReadOnly = true;
			this.parametersEnabledSetIndex.Width = 85;
			// 
			// parametersEnabledSet
			// 
			this.parametersEnabledSet.Frozen = true;
			this.parametersEnabledSet.HeaderText = "Parameters Enabled Set";
			this.parametersEnabledSet.MinimumWidth = 12;
			this.parametersEnabledSet.Name = "parametersEnabledSet";
			this.parametersEnabledSet.Width = 250;
			// 
			// paraEnabledDelete
			// 
			this.paraEnabledDelete.Location = new System.Drawing.Point(1730, 932);
			this.paraEnabledDelete.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.paraEnabledDelete.Name = "paraEnabledDelete";
			this.paraEnabledDelete.Size = new System.Drawing.Size(133, 52);
			this.paraEnabledDelete.TabIndex = 102;
			this.paraEnabledDelete.Text = "Delete";
			this.paraEnabledDelete.UseVisualStyleBackColor = true;
			this.paraEnabledDelete.Click += new System.EventHandler(this.paraEnabledDelete_Click);
			// 
			// paraEnabledInsert
			// 
			this.paraEnabledInsert.Location = new System.Drawing.Point(1549, 935);
			this.paraEnabledInsert.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.paraEnabledInsert.Name = "paraEnabledInsert";
			this.paraEnabledInsert.Size = new System.Drawing.Size(133, 52);
			this.paraEnabledInsert.TabIndex = 102;
			this.paraEnabledInsert.Text = "Insert";
			this.paraEnabledInsert.UseVisualStyleBackColor = true;
			this.paraEnabledInsert.Click += new System.EventHandler(this.paraEnabledInsert_Click);
			// 
			// carrFreqOffsetDataGridViewLabel
			// 
			this.carrFreqOffsetDataGridViewLabel.AutoSize = true;
			this.carrFreqOffsetDataGridViewLabel.Location = new System.Drawing.Point(2037, 70);
			this.carrFreqOffsetDataGridViewLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.carrFreqOffsetDataGridViewLabel.Name = "carrFreqOffsetDataGridViewLabel";
			this.carrFreqOffsetDataGridViewLabel.Size = new System.Drawing.Size(390, 32);
			this.carrFreqOffsetDataGridViewLabel.TabIndex = 105;
			this.carrFreqOffsetDataGridViewLabel.Text = "Carrier Frequency Offset Set[]";
			// 
			// carrFreqOffsetDataGridView
			// 
			this.carrFreqOffsetDataGridView.AllowUserToAddRows = false;
			this.carrFreqOffsetDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.carrFreqOffsetDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.carrFreqOffsetIndex,
            this.carrFreqOffsetSet});
			this.carrFreqOffsetDataGridView.Location = new System.Drawing.Point(2010, 121);
			this.carrFreqOffsetDataGridView.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.carrFreqOffsetDataGridView.Name = "carrFreqOffsetDataGridView";
			this.carrFreqOffsetDataGridView.RowHeadersWidth = 102;
			this.carrFreqOffsetDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.carrFreqOffsetDataGridView.Size = new System.Drawing.Size(427, 258);
			this.carrFreqOffsetDataGridView.TabIndex = 106;
			// 
			// carrFreqOffDelete
			// 
			this.carrFreqOffDelete.Location = new System.Drawing.Point(2249, 400);
			this.carrFreqOffDelete.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.carrFreqOffDelete.Name = "carrFreqOffDelete";
			this.carrFreqOffDelete.Size = new System.Drawing.Size(133, 52);
			this.carrFreqOffDelete.TabIndex = 102;
			this.carrFreqOffDelete.Text = "Delete";
			this.carrFreqOffDelete.UseVisualStyleBackColor = true;
			this.carrFreqOffDelete.Click += new System.EventHandler(this.carrFreqOffDelete_Click);
			// 
			// carrFreqOffInsert
			// 
			this.carrFreqOffInsert.Location = new System.Drawing.Point(2069, 400);
			this.carrFreqOffInsert.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.carrFreqOffInsert.Name = "carrFreqOffInsert";
			this.carrFreqOffInsert.Size = new System.Drawing.Size(133, 52);
			this.carrFreqOffInsert.TabIndex = 102;
			this.carrFreqOffInsert.Text = "Insert";
			this.carrFreqOffInsert.UseVisualStyleBackColor = true;
			this.carrFreqOffInsert.Click += new System.EventHandler(this.carrFreqOffInsert_Click);
			// 
			// modulationIndexDataGridViewLabel
			// 
			this.modulationIndexDataGridViewLabel.AutoSize = true;
			this.modulationIndexDataGridViewLabel.Location = new System.Drawing.Point(2076, 492);
			this.modulationIndexDataGridViewLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.modulationIndexDataGridViewLabel.Name = "modulationIndexDataGridViewLabel";
			this.modulationIndexDataGridViewLabel.Size = new System.Drawing.Size(297, 32);
			this.modulationIndexDataGridViewLabel.TabIndex = 105;
			this.modulationIndexDataGridViewLabel.Text = "Modulation Index Set[]";
			// 
			// modulationIndexDataGridView
			// 
			this.modulationIndexDataGridView.AllowUserToAddRows = false;
			this.modulationIndexDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.modulationIndexDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.modulationIndex,
            this.modulationIndexSet});
			this.modulationIndexDataGridView.Location = new System.Drawing.Point(2010, 543);
			this.modulationIndexDataGridView.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.modulationIndexDataGridView.Name = "modulationIndexDataGridView";
			this.modulationIndexDataGridView.RowHeadersWidth = 102;
			this.modulationIndexDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.modulationIndexDataGridView.Size = new System.Drawing.Size(427, 258);
			this.modulationIndexDataGridView.TabIndex = 106;
			// 
			// modulationIndex
			// 
			this.modulationIndex.Frozen = true;
			this.modulationIndex.HeaderText = "Index";
			this.modulationIndex.MinimumWidth = 12;
			this.modulationIndex.Name = "modulationIndex";
			this.modulationIndex.ReadOnly = true;
			this.modulationIndex.Width = 85;
			// 
			// modulationIndexSet
			// 
			this.modulationIndexSet.Frozen = true;
			this.modulationIndexSet.HeaderText = "Modulation Index Set";
			this.modulationIndexSet.MinimumWidth = 12;
			this.modulationIndexSet.Name = "modulationIndexSet";
			this.modulationIndexSet.Width = 250;
			// 
			// modulationIndexDelete
			// 
			this.modulationIndexDelete.Location = new System.Drawing.Point(2249, 822);
			this.modulationIndexDelete.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.modulationIndexDelete.Name = "modulationIndexDelete";
			this.modulationIndexDelete.Size = new System.Drawing.Size(133, 52);
			this.modulationIndexDelete.TabIndex = 102;
			this.modulationIndexDelete.Text = "Delete";
			this.modulationIndexDelete.UseVisualStyleBackColor = true;
			this.modulationIndexDelete.Click += new System.EventHandler(this.modulationIndexDelete_Click);
			// 
			// modulationIndexInsert
			// 
			this.modulationIndexInsert.Location = new System.Drawing.Point(2069, 822);
			this.modulationIndexInsert.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.modulationIndexInsert.Name = "modulationIndexInsert";
			this.modulationIndexInsert.Size = new System.Drawing.Size(133, 52);
			this.modulationIndexInsert.TabIndex = 102;
			this.modulationIndexInsert.Text = "Insert";
			this.modulationIndexInsert.UseVisualStyleBackColor = true;
			this.modulationIndexInsert.Click += new System.EventHandler(this.modulationIndexInsert_Click);
			// 
			// symbolTimingErrorDataGridViewLabel
			// 
			this.symbolTimingErrorDataGridViewLabel.AutoSize = true;
			this.symbolTimingErrorDataGridViewLabel.Location = new System.Drawing.Point(2054, 920);
			this.symbolTimingErrorDataGridViewLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.symbolTimingErrorDataGridViewLabel.Name = "symbolTimingErrorDataGridViewLabel";
			this.symbolTimingErrorDataGridViewLabel.Size = new System.Drawing.Size(337, 32);
			this.symbolTimingErrorDataGridViewLabel.TabIndex = 105;
			this.symbolTimingErrorDataGridViewLabel.Text = "Symbol Timing Error Set[]";
			// 
			// symbolTimingErrorDataGridView
			// 
			this.symbolTimingErrorDataGridView.AllowUserToAddRows = false;
			this.symbolTimingErrorDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.symbolTimingErrorDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.symbolTimingErrorIndex,
            this.symbolTimingErrorSet});
			this.symbolTimingErrorDataGridView.Location = new System.Drawing.Point(2010, 974);
			this.symbolTimingErrorDataGridView.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.symbolTimingErrorDataGridView.Name = "symbolTimingErrorDataGridView";
			this.symbolTimingErrorDataGridView.RowHeadersWidth = 102;
			this.symbolTimingErrorDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.symbolTimingErrorDataGridView.Size = new System.Drawing.Size(427, 258);
			this.symbolTimingErrorDataGridView.TabIndex = 106;
			// 
			// symbolTimingErrorIndex
			// 
			this.symbolTimingErrorIndex.Frozen = true;
			this.symbolTimingErrorIndex.HeaderText = "Index";
			this.symbolTimingErrorIndex.MinimumWidth = 12;
			this.symbolTimingErrorIndex.Name = "symbolTimingErrorIndex";
			this.symbolTimingErrorIndex.ReadOnly = true;
			this.symbolTimingErrorIndex.Width = 85;
			// 
			// symbolTimingErrorSet
			// 
			this.symbolTimingErrorSet.Frozen = true;
			this.symbolTimingErrorSet.HeaderText = "Symbol Timing Error Set";
			this.symbolTimingErrorSet.MinimumWidth = 12;
			this.symbolTimingErrorSet.Name = "symbolTimingErrorSet";
			this.symbolTimingErrorSet.Width = 250;
			// 
			// symbolTimingErrorDelete
			// 
			this.symbolTimingErrorDelete.Location = new System.Drawing.Point(2249, 1259);
			this.symbolTimingErrorDelete.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.symbolTimingErrorDelete.Name = "symbolTimingErrorDelete";
			this.symbolTimingErrorDelete.Size = new System.Drawing.Size(133, 52);
			this.symbolTimingErrorDelete.TabIndex = 102;
			this.symbolTimingErrorDelete.Text = "Delete";
			this.symbolTimingErrorDelete.UseVisualStyleBackColor = true;
			this.symbolTimingErrorDelete.Click += new System.EventHandler(this.symbolTimingErrorDelete_Click);
			// 
			// symbolTimingErrorInsert
			// 
			this.symbolTimingErrorInsert.Location = new System.Drawing.Point(2069, 1259);
			this.symbolTimingErrorInsert.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.symbolTimingErrorInsert.Name = "symbolTimingErrorInsert";
			this.symbolTimingErrorInsert.Size = new System.Drawing.Size(133, 52);
			this.symbolTimingErrorInsert.TabIndex = 102;
			this.symbolTimingErrorInsert.Text = "Insert";
			this.symbolTimingErrorInsert.UseVisualStyleBackColor = true;
			this.symbolTimingErrorInsert.Click += new System.EventHandler(this.symbolTimingErrorInsert_Click);
			// 
			// dirtyTxModulationIndexTypeLabel
			// 
			this.dirtyTxModulationIndexTypeLabel.AutoSize = true;
			this.dirtyTxModulationIndexTypeLabel.Location = new System.Drawing.Point(2023, 1352);
			this.dirtyTxModulationIndexTypeLabel.Margin = new System.Windows.Forms.Padding(21, 0, 21, 0);
			this.dirtyTxModulationIndexTypeLabel.Name = "dirtyTxModulationIndexTypeLabel";
			this.dirtyTxModulationIndexTypeLabel.Size = new System.Drawing.Size(404, 32);
			this.dirtyTxModulationIndexTypeLabel.TabIndex = 41;
			this.dirtyTxModulationIndexTypeLabel.Text = "Dirty Tx Modulation Index Type";
			// 
			// dirtyTxModulationIndexTypeComboBox
			// 
			this.dirtyTxModulationIndexTypeComboBox.Location = new System.Drawing.Point(2105, 1400);
			this.dirtyTxModulationIndexTypeComboBox.Margin = new System.Windows.Forms.Padding(21, 17, 21, 17);
			this.dirtyTxModulationIndexTypeComboBox.Name = "dirtyTxModulationIndexTypeComboBox";
			this.dirtyTxModulationIndexTypeComboBox.Size = new System.Drawing.Size(240, 39);
			this.dirtyTxModulationIndexTypeComboBox.TabIndex = 28;
			this.dirtyTxModulationIndexTypeComboBox.Text = "Standard";
			// 
			// carrFreqOffsetIndex
			// 
			this.carrFreqOffsetIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
			this.carrFreqOffsetIndex.Frozen = true;
			this.carrFreqOffsetIndex.HeaderText = "Index";
			this.carrFreqOffsetIndex.MinimumWidth = 12;
			this.carrFreqOffsetIndex.Name = "carrFreqOffsetIndex";
			this.carrFreqOffsetIndex.ReadOnly = true;
			this.carrFreqOffsetIndex.Width = 137;
			// 
			// carrFreqOffsetSet
			// 
			this.carrFreqOffsetSet.Frozen = true;
			this.carrFreqOffsetSet.HeaderText = "Carrier Frequency Offset Set";
			this.carrFreqOffsetSet.MinimumWidth = 12;
			this.carrFreqOffsetSet.Name = "carrFreqOffsetSet";
			this.carrFreqOffsetSet.Width = 250;
			// 
			// MainForm
			// 
			this.AcceptButton = this.generateButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(2505, 1729);
			this.Controls.Add(this.whiteEnLabel);
			this.Controls.Add(this.whiteClkLabel);
			this.Controls.Add(this.whiteningSettingLabel);
			this.Controls.Add(this.whiteClkNumeric);
			this.Controls.Add(this.whiteEnComboBox);
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
			this.Controls.Add(this.numOfIdleSlotsLabel);
			this.Controls.Add(this.numOfUniqPacketsLabel);
			this.Controls.Add(this.numOfUniqNumeric);
			this.Controls.Add(this.numOfIdleSlotsNumeric);
			this.Controls.Add(this.rfsgResourceLabel);
			this.Controls.Add(this.chnNumberLabel);
			this.Controls.Add(this.carrierFreqLabel);
			this.Controls.Add(this.powerLevelLabel);
			this.Controls.Add(this.externalAttnLabel);
			this.Controls.Add(this.autoheadroomEnabLabel);
			this.Controls.Add(this.headroomLabel);
			this.Controls.Add(this.actualHeadroomLabel);
			this.Controls.Add(this.refSourceLabel);
			this.Controls.Add(this.clkOutTerminalLabel);
			this.Controls.Add(this.allIqImpairEnLabel);
			this.Controls.Add(this.quadratureSkewLabel);
			this.Controls.Add(this.iDCOffsetLabel);
			this.Controls.Add(this.qDCOffsetLabel);
			this.Controls.Add(this.iqGaimbalanceLabel);
			this.Controls.Add(this.carrierFreqOffLabel);
			this.Controls.Add(this.awgnEnabledLabel);
			this.Controls.Add(this.cnrLabel);
			this.Controls.Add(this.bdaddrLapLabel);
			this.Controls.Add(this.bdaddrUapLabel);
			this.Controls.Add(this.bdaddrNapLabel);
			this.Controls.Add(this.pktLtAddrLabel);
			this.Controls.Add(this.pktHdrFlowLabel);
			this.Controls.Add(this.pktHdrArqnLabel);
			this.Controls.Add(this.pktHdrSeqnLabel);
			this.Controls.Add(this.payHdrLlidLabel);
			this.Controls.Add(this.payHdrFlowLabel);
			this.Controls.Add(this.payHdrPaylenModeLabel);
			this.Controls.Add(this.userDefinedBitLabel);
			this.Controls.Add(this.payHdrPaylenLabel);
			this.Controls.Add(this.payHdrActPaylenLabel);
			this.Controls.Add(this.payHdrDatatypeLabel);
			this.Controls.Add(this.paydatPnorderLabel);
			this.Controls.Add(this.paydatSeedLabel);
			this.Controls.Add(this.errorLabel);
			this.Controls.Add(this.extmsgPacket22Label);
			this.Controls.Add(this.textmsgPayloadHdrLabel);
			this.Controls.Add(this.payloadDataLabel);
			this.Controls.Add(this.hardwareLabel);
			this.Controls.Add(this.frequencySettingLabel);
			this.Controls.Add(this.impairmentsLabel);
			this.Controls.Add(this.bdAddressLabel);
			this.Controls.Add(this.packetLabel);
			this.Controls.Add(this.payloadLabel);
			this.Controls.Add(this.chnNumberNumeric);
			this.Controls.Add(this.powerLevelNumeric);
			this.Controls.Add(this.externalAttnNumeric);
			this.Controls.Add(this.headroomNumeric);
			this.Controls.Add(this.quadratureSkewNumeric);
			this.Controls.Add(this.iDCOffsetNumeric);
			this.Controls.Add(this.qDCOffsetNumeric);
			this.Controls.Add(this.iqGaimbalanceNumeric);
			this.Controls.Add(this.carrierFreqOffNumeric);
			this.Controls.Add(this.cnrNumeric);
			this.Controls.Add(this.bdaddrLapNumeric);
			this.Controls.Add(this.bdaddrUapNumeric);
			this.Controls.Add(this.bdaddrNapNumeric);
			this.Controls.Add(this.pktLtAddrNumeric);
			this.Controls.Add(this.pktHdrFlowNumeric);
			this.Controls.Add(this.pktHdrSeqnNumeric);
			this.Controls.Add(this.payHdrLlidNumeric);
			this.Controls.Add(this.payHdrFlowNumeric);
			this.Controls.Add(this.payHdrPaylenNumeric);
			this.Controls.Add(this.paydatPnorderNumeric);
			this.Controls.Add(this.paydatSeedNumeric);
			this.Controls.Add(this.rfsgResourceTextBox);
			this.Controls.Add(this.carrierFreqTextBox);
			this.Controls.Add(this.actualHeadroomTextBox);
			this.Controls.Add(this.payHdrActPaylenTextBox);
			this.Controls.Add(this.errorTextBox);
			this.Controls.Add(this.generateButton);
			this.Controls.Add(this.stopButton);
			this.Controls.Add(this.autoHeadroomEnabComboBox);
			this.Controls.Add(this.refSourceComboBox);
			this.Controls.Add(this.userDefinedBitsComboBox);
			this.Controls.Add(this.clkOutTerminalComboBox);
			this.Controls.Add(this.allIqImpairEnComboBox);
			this.Controls.Add(this.awgnEnabledComboBox);
			this.Controls.Add(this.pktHdrArqnComboBox);
			this.Controls.Add(this.payHdrPaylenModeComboBox);
			this.Controls.Add(this.payHdrDatatypeComboBox);
			this.Controls.Add(this.packetComboBox);
			this.Controls.Add(this.dataRateLabel);
			this.Controls.Add(this.dataRateNumeric);
			this.Controls.Add(this.LETPPayloadTypeLabel);
			this.Controls.Add(this.LETPPayloadTypeComboBox);
			this.Controls.Add(this.LETPCorruptAlternateCRCLabel);
			this.Controls.Add(this.LETPCorruptAlternateCRCComboBox);
			this.Controls.Add(this.highDataThroughputSettingsLabel);
			this.Controls.Add(this.zadoffChuIndexLabel);
			this.Controls.Add(this.zadoffChuIndexNumeric);
			this.Controls.Add(this.physicalChannelAddressLabel);
			this.Controls.Add(this.physicalChannelAddressNumeric);
			this.Controls.Add(this.HdtPacketFormatLabel);
			this.Controls.Add(this.HdtPacketFormatComboBox);
			this.Controls.Add(this.HdtPhyIntervalLabel);
			this.Controls.Add(this.HdtPhyIntervalNumeric);
			this.Controls.Add(this.dirtyTxSettingsLabel);
			this.Controls.Add(this.dirtyTxModeLabel);
			this.Controls.Add(this.dirtyTxModeComboBox);
			this.Controls.Add(this.paraEnabledDataGridViewLabel);
			this.Controls.Add(this.paraEnabledDataGridView);
			this.Controls.Add(this.paraEnabledInsert);
			this.Controls.Add(this.paraEnabledDelete);
			this.Controls.Add(this.carrFreqOffsetDataGridViewLabel);
			this.Controls.Add(this.carrFreqOffsetDataGridView);
			this.Controls.Add(this.carrFreqOffInsert);
			this.Controls.Add(this.carrFreqOffDelete);
			this.Controls.Add(this.modulationIndexDataGridViewLabel);
			this.Controls.Add(this.modulationIndexDataGridView);
			this.Controls.Add(this.modulationIndexInsert);
			this.Controls.Add(this.modulationIndexDelete);
			this.Controls.Add(this.symbolTimingErrorDataGridViewLabel);
			this.Controls.Add(this.symbolTimingErrorDataGridView);
			this.Controls.Add(this.symbolTimingErrorInsert);
			this.Controls.Add(this.symbolTimingErrorDelete);
			this.Controls.Add(this.dirtyTxModulationIndexTypeLabel);
			this.Controls.Add(this.dirtyTxModulationIndexTypeComboBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "Bluetooth Generate Data (Dirty Tx) Packet Example";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormClosing);
			((System.ComponentModel.ISupportInitialize)(this.chnNumberNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.externalAttnNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.headroomNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.quadratureSkewNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.iDCOffsetNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.qDCOffsetNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.iqGaimbalanceNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.carrierFreqOffNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cnrNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bdaddrLapNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bdaddrUapNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bdaddrNapNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pktLtAddrNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pktHdrFlowNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pktHdrSeqnNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.payHdrLlidNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.payHdrFlowNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.payHdrPaylenNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.paydatPnorderNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.paydatSeedNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numOfUniqNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numOfIdleSlotsNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.iOffsetNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.qOffsetNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.iCommonModeOffsetNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.qCommonModeOffsetNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.whiteClkNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataRateNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.zadoffChuIndexNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.physicalChannelAddressNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.HdtPhyIntervalNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.paraEnabledDataGridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.carrFreqOffsetDataGridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.modulationIndexDataGridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.symbolTimingErrorDataGridView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label rfsgResourceLabel;
        private System.Windows.Forms.Label chnNumberLabel;
        private System.Windows.Forms.Label carrierFreqLabel;
        private System.Windows.Forms.Label powerLevelLabel;
        private System.Windows.Forms.Label externalAttnLabel;
        private System.Windows.Forms.Label autoheadroomEnabLabel;
        private System.Windows.Forms.Label headroomLabel;
        private System.Windows.Forms.Label actualHeadroomLabel;
        private System.Windows.Forms.Label refSourceLabel;
        private System.Windows.Forms.Label clkOutTerminalLabel;
        private System.Windows.Forms.Label allIqImpairEnLabel;
        private System.Windows.Forms.Label quadratureSkewLabel;
        private System.Windows.Forms.Label iDCOffsetLabel;
        private System.Windows.Forms.Label qDCOffsetLabel;
        private System.Windows.Forms.Label iqGaimbalanceLabel;
        private System.Windows.Forms.Label carrierFreqOffLabel;
        private System.Windows.Forms.Label awgnEnabledLabel;
        private System.Windows.Forms.Label cnrLabel;
        private System.Windows.Forms.Label bdaddrLapLabel;
        private System.Windows.Forms.Label bdaddrUapLabel;
        private System.Windows.Forms.Label bdaddrNapLabel;
        private System.Windows.Forms.Label pktLtAddrLabel;
        private System.Windows.Forms.Label pktHdrFlowLabel;
        private System.Windows.Forms.Label pktHdrArqnLabel;
        private System.Windows.Forms.Label pktHdrSeqnLabel;
        private System.Windows.Forms.Label payHdrLlidLabel;
        private System.Windows.Forms.Label payHdrFlowLabel;
        private System.Windows.Forms.Label userDefinedBitLabel;
        private System.Windows.Forms.Label payHdrPaylenModeLabel;
        private System.Windows.Forms.Label payHdrPaylenLabel;
        private System.Windows.Forms.Label payHdrActPaylenLabel;
        private System.Windows.Forms.Label payHdrDatatypeLabel;
        private System.Windows.Forms.Label paydatPnorderLabel;
        private System.Windows.Forms.Label paydatSeedLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label extmsgPacket22Label;
        private System.Windows.Forms.Label textmsgPayloadHdrLabel;
        private System.Windows.Forms.Label payloadDataLabel;
        private System.Windows.Forms.Label hardwareLabel;
        private System.Windows.Forms.Label frequencySettingLabel;
        private System.Windows.Forms.Label impairmentsLabel;
        private System.Windows.Forms.Label bdAddressLabel;
        private System.Windows.Forms.Label packetLabel;
        private System.Windows.Forms.Label payloadLabel;
        private System.Windows.Forms.NumericUpDown chnNumberNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown externalAttnNumeric;
        private System.Windows.Forms.NumericUpDown headroomNumeric;
        private System.Windows.Forms.NumericUpDown quadratureSkewNumeric;
        private System.Windows.Forms.NumericUpDown iDCOffsetNumeric;
        private System.Windows.Forms.NumericUpDown qDCOffsetNumeric;
        private System.Windows.Forms.NumericUpDown iqGaimbalanceNumeric;
        private System.Windows.Forms.NumericUpDown carrierFreqOffNumeric;
        private System.Windows.Forms.NumericUpDown cnrNumeric;
        private System.Windows.Forms.NumericUpDown bdaddrLapNumeric;
        private System.Windows.Forms.NumericUpDown bdaddrUapNumeric;
        private System.Windows.Forms.NumericUpDown bdaddrNapNumeric;
        private System.Windows.Forms.NumericUpDown pktLtAddrNumeric;
        private System.Windows.Forms.NumericUpDown pktHdrFlowNumeric;
        private System.Windows.Forms.NumericUpDown pktHdrSeqnNumeric;
        private System.Windows.Forms.NumericUpDown payHdrLlidNumeric;
        private System.Windows.Forms.NumericUpDown payHdrFlowNumeric;
        private System.Windows.Forms.NumericUpDown payHdrPaylenNumeric;
        private System.Windows.Forms.NumericUpDown paydatPnorderNumeric;
        private System.Windows.Forms.NumericUpDown paydatSeedNumeric;
        private System.Windows.Forms.TextBox rfsgResourceTextBox;
        private System.Windows.Forms.TextBox carrierFreqTextBox;
        private System.Windows.Forms.TextBox actualHeadroomTextBox;
        private System.Windows.Forms.TextBox payHdrActPaylenTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox autoHeadroomEnabComboBox;
        private System.Windows.Forms.ComboBox refSourceComboBox;
        private System.Windows.Forms.ComboBox userDefinedBitsComboBox;
        private System.Windows.Forms.ComboBox clkOutTerminalComboBox;
        private System.Windows.Forms.ComboBox allIqImpairEnComboBox;
        private System.Windows.Forms.ComboBox awgnEnabledComboBox;
        private System.Windows.Forms.ComboBox pktHdrArqnComboBox;
        private System.Windows.Forms.ComboBox packetComboBox;
        private System.Windows.Forms.ComboBox payHdrPaylenModeComboBox;
        private System.Windows.Forms.ComboBox payHdrDatatypeComboBox;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.NumericUpDown numOfUniqNumeric;
        private System.Windows.Forms.NumericUpDown numOfIdleSlotsNumeric;
        private System.Windows.Forms.Label numOfUniqPacketsLabel;
        private System.Windows.Forms.Label numOfIdleSlotsLabel;
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
        private System.Windows.Forms.Label whiteEnLabel;
        private System.Windows.Forms.Label whiteClkLabel;
        private System.Windows.Forms.Label whiteningSettingLabel;
        private System.Windows.Forms.NumericUpDown whiteClkNumeric;
        private System.Windows.Forms.ComboBox whiteEnComboBox;
        private System.Windows.Forms.Label dataRateLabel;
        private System.Windows.Forms.NumericUpDown dataRateNumeric;
        private System.Windows.Forms.Label LETPPayloadTypeLabel;
        private System.Windows.Forms.ComboBox LETPPayloadTypeComboBox;
        private System.Windows.Forms.Label LETPCorruptAlternateCRCLabel;
        private System.Windows.Forms.ComboBox LETPCorruptAlternateCRCComboBox;
        private System.Windows.Forms.Label highDataThroughputSettingsLabel;
        private System.Windows.Forms.Label zadoffChuIndexLabel;
        private System.Windows.Forms.NumericUpDown zadoffChuIndexNumeric;
        private System.Windows.Forms.Label physicalChannelAddressLabel;
        private System.Windows.Forms.NumericUpDown physicalChannelAddressNumeric;
        private System.Windows.Forms.Label HdtPacketFormatLabel;
        private System.Windows.Forms.ComboBox HdtPacketFormatComboBox;
        private System.Windows.Forms.Label HdtPhyIntervalLabel;
        private System.Windows.Forms.NumericUpDown HdtPhyIntervalNumeric;
        private System.Windows.Forms.Label dirtyTxSettingsLabel;
        private System.Windows.Forms.Label dirtyTxModeLabel;
        private System.Windows.Forms.ComboBox dirtyTxModeComboBox;
        private System.Windows.Forms.Label paraEnabledDataGridViewLabel;
        private System.Windows.Forms.DataGridView paraEnabledDataGridView;
        private System.Windows.Forms.Button paraEnabledInsert;
        private System.Windows.Forms.Button paraEnabledDelete;
        private System.Windows.Forms.Label carrFreqOffsetDataGridViewLabel;
        private System.Windows.Forms.DataGridView carrFreqOffsetDataGridView;
        private System.Windows.Forms.Button carrFreqOffInsert;
        private System.Windows.Forms.Button carrFreqOffDelete;
        private System.Windows.Forms.Label modulationIndexDataGridViewLabel;
        private System.Windows.Forms.DataGridView modulationIndexDataGridView;
        private System.Windows.Forms.Button modulationIndexInsert;
        private System.Windows.Forms.Button modulationIndexDelete;
        private System.Windows.Forms.Label symbolTimingErrorDataGridViewLabel;
        private System.Windows.Forms.DataGridView symbolTimingErrorDataGridView;
        private System.Windows.Forms.Button symbolTimingErrorInsert;
        private System.Windows.Forms.Button symbolTimingErrorDelete;
        private System.Windows.Forms.Label dirtyTxModulationIndexTypeLabel;
        private System.Windows.Forms.ComboBox dirtyTxModulationIndexTypeComboBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn parametersEnabledSetIndex;
        private System.Windows.Forms.DataGridViewComboBoxColumn parametersEnabledSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn modulationIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn modulationIndexSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn symbolTimingErrorIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn symbolTimingErrorSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn carrFreqOffsetIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn carrFreqOffsetSet;
    }
}
