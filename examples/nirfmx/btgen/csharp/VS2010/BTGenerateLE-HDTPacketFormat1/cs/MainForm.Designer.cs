using System.Windows.Forms;

namespace NationalInstruments.Examples.BTGenerateLEHDTPacketFormat1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            this.rfsgResourceLabel = new System.Windows.Forms.Label();
            this.chnNumberLabel = new System.Windows.Forms.Label();
            this.carrierFreqLabel = new System.Windows.Forms.Label();
            this.powerLevelLabel = new System.Windows.Forms.Label();
            this.externalAttnLabel = new System.Windows.Forms.Label();
            this.autoheadroomEnabLabel = new System.Windows.Forms.Label();
            this.headroomLabel = new System.Windows.Forms.Label();
            this.refSourceLabel = new System.Windows.Forms.Label();
            this.clkTerminalLabel = new System.Windows.Forms.Label();
            this.allIqImpairEnLabel = new System.Windows.Forms.Label();
            this.quadratureSkewLabel = new System.Windows.Forms.Label();
            this.iDcOffsetLabel = new System.Windows.Forms.Label();
            this.qDcOffsetLabel = new System.Windows.Forms.Label();
            this.iqGaimbalanceLabel = new System.Windows.Forms.Label();
            this.carrierFreqOffLabel = new System.Windows.Forms.Label();
            this.awgnEnabledLabel = new System.Windows.Forms.Label();
            this.cnrLabel = new System.Windows.Forms.Label();
            this.hardwareLabel = new System.Windows.Forms.Label();
            this.frequencySettingsLabel = new System.Windows.Forms.Label();
            this.impairmentsLabel = new System.Windows.Forms.Label();
            this.chnNumberNumeric = new System.Windows.Forms.NumericUpDown();
            this.powerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.externalAttnNumeric = new System.Windows.Forms.NumericUpDown();
            this.headroomNumeric = new System.Windows.Forms.NumericUpDown();
            this.quadratureSkewNumeric = new System.Windows.Forms.NumericUpDown();
            this.iDcOffsetNumeric = new System.Windows.Forms.NumericUpDown();
            this.qDcOffsetNumeric = new System.Windows.Forms.NumericUpDown();
            this.iqGaimbalanceNumeric = new System.Windows.Forms.NumericUpDown();
            this.cnrNumeric = new System.Windows.Forms.NumericUpDown();
            this.rfsgResourceTextBox = new System.Windows.Forms.TextBox();
            this.carrierFreqTextBox = new System.Windows.Forms.TextBox();
            this.autoheadroomEnabComboBox = new System.Windows.Forms.ComboBox();
            this.refSourceComboBox = new System.Windows.Forms.ComboBox();
            this.clkOutTerminalComboBox = new System.Windows.Forms.ComboBox();
            this.allIqImpairEnComboBox = new System.Windows.Forms.ComboBox();
            this.awgnEnabledComboBox = new System.Windows.Forms.ComboBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.carrierFreqOffNumeric = new System.Windows.Forms.NumericUpDown();
            this.payloadLengthModeLabel = new System.Windows.Forms.Label();
            this.waveNameLabel = new System.Windows.Forms.Label();
            this.scriptLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.waveNameTextBox = new System.Windows.Forms.TextBox();
            this.scriptTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.terminalConfigurationLabel = new System.Windows.Forms.Label();
            this.terminalConfigurationComboBox = new System.Windows.Forms.ComboBox();
            this.outputPortLabel = new System.Windows.Forms.Label();
            this.outputPortComboBox = new System.Windows.Forms.ComboBox();
            this.clkOutputTerminalLabel = new System.Windows.Forms.Label();
            this.actualHeadroomLabel = new System.Windows.Forms.Label();
            this.highDataThroughputLabel = new System.Windows.Forms.Label();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OversamplingFactorNumeric = new System.Windows.Forms.NumericUpDown();
            this.OversamplingFactorLabel = new System.Windows.Forms.Label();
            this.actualHeadroomTextBox = new System.Windows.Forms.TextBox();
            this.dataRateLabel = new System.Windows.Forms.Label();
            this.dataRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.zadoffChuIndexLabel = new System.Windows.Forms.Label();
            this.zadoffChuIndexNumeric = new System.Windows.Forms.NumericUpDown();
            this.physicalChannelAddressLabel = new System.Windows.Forms.Label();
            this.physicalChannelAddressNumeric = new System.Windows.Forms.NumericUpDown();
            this.HdtPhyIntervalLabel = new System.Windows.Forms.Label();
            this.HdtPhyIntervalNumeric = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.autoPayloadZoneProeprtiesSettingsLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.payloadLengthInsertButton = new System.Windows.Forms.Button();
            this.payloadLengthDeleteButton = new System.Windows.Forms.Button();
            this.PayloadLengthBytesGrid = new System.Windows.Forms.DataGridView();
            this.payloadLengthIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.payloadLengthBytesValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TxLenSequenceNumberInsertButton = new System.Windows.Forms.Button();
            this.TxLenSequenceNumberDeleteButton = new System.Windows.Forms.Button();
            this.TxLenSequenceNumberGrid = new System.Windows.Forms.DataGridView();
            this.TxLenSequenceNumberIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TxLenSequenceNumberValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.payloadLengthLabel = new System.Windows.Forms.Label();
            this.TxLenSequenceNumberLabel = new System.Windows.Forms.Label();
            this.NumebrOfBlocksLabel = new System.Windows.Forms.Label();
            this.NumberOfBlocksInsertButton = new System.Windows.Forms.Button();
            this.NumberOfBlocksDeleteButton = new System.Windows.Forms.Button();
            this.NumberOfBlocksGrid = new System.Windows.Forms.DataGridView();
            this.NumberOfBlocksIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NumberOfBlocksValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BlockSizeLabel = new System.Windows.Forms.Label();
            this.BlockSizeInsertButton = new System.Windows.Forms.Button();
            this.BlockSizeDeleteButton = new System.Windows.Forms.Button();
            this.BlockSizeGrid = new System.Windows.Forms.DataGridView();
            this.BlockSizeIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BlockSizeValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastBlockSizeLabel = new System.Windows.Forms.Label();
            this.LastBlockSizeInsertButton = new System.Windows.Forms.Button();
            this.LastBlockSizeDeleteButton = new System.Windows.Forms.Button();
            this.LastBlockSizeGrid = new System.Windows.Forms.DataGridView();
            this.LastBlockSizeIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastBlockSizeValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TxBlockMapLabel = new System.Windows.Forms.Label();
            this.TxBlockMapInsertButton = new System.Windows.Forms.Button();
            this.TxBlockMapDeleteButton = new System.Windows.Forms.Button();
            this.TxBlockMapGrid = new System.Windows.Forms.DataGridView();
            this.TxBlockMapIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TxBlockMapValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActualPayloadLengthLabel = new System.Windows.Forms.Label();
            this.ActualPayloadLengthInsertButton = new System.Windows.Forms.Button();
            this.ActualPayloadLengthDeleteButton = new System.Windows.Forms.Button();
            this.ActualPayloadLengthGrid = new System.Windows.Forms.DataGridView();
            this.ActualPayloadLengthIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActualPayloadLengthValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PayloadZoneLengthLabel = new System.Windows.Forms.Label();
            this.PayloadZoneLengthValue = new System.Windows.Forms.NumericUpDown();
            this.Format1PayloadZoneConfigurationModeComboBox = new System.Windows.Forms.ComboBox();
            this.Format1PayloadZoneConfigurationModeLabel = new System.Windows.Forms.Label();
            this.NumberOfPayloadsLabel = new System.Windows.Forms.Label();
            this.numberOfPayloadNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.payloadLengthModeComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.chnNumberNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAttnNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.headroomNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.quadratureSkewNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iDcOffsetNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qDcOffsetNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqGaimbalanceNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cnrNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFreqOffNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OversamplingFactorNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zadoffChuIndexNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.physicalChannelAddressNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HdtPhyIntervalNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PayloadLengthBytesGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxLenSequenceNumberGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfBlocksGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BlockSizeGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastBlockSizeGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxBlockMapGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActualPayloadLengthGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PayloadZoneLengthValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfPayloadNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // rfsgResourceLabel
            // 
            this.rfsgResourceLabel.AutoSize = true;
            this.rfsgResourceLabel.Location = new System.Drawing.Point(21, 72);
            this.rfsgResourceLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.rfsgResourceLabel.Name = "rfsgResourceLabel";
            this.rfsgResourceLabel.Size = new System.Drawing.Size(220, 32);
            this.rfsgResourceLabel.TabIndex = 43;
            this.rfsgResourceLabel.Text = "RFSG Resource";
            // 
            // chnNumberLabel
            // 
            this.chnNumberLabel.AutoSize = true;
            this.chnNumberLabel.Location = new System.Drawing.Point(21, 174);
            this.chnNumberLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.chnNumberLabel.Name = "chnNumberLabel";
            this.chnNumberLabel.Size = new System.Drawing.Size(228, 32);
            this.chnNumberLabel.TabIndex = 44;
            this.chnNumberLabel.Text = "Channel Number";
            // 
            // carrierFreqLabel
            // 
            this.carrierFreqLabel.AutoSize = true;
            this.carrierFreqLabel.Location = new System.Drawing.Point(21, 222);
            this.carrierFreqLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.carrierFreqLabel.Name = "carrierFreqLabel";
            this.carrierFreqLabel.Size = new System.Drawing.Size(300, 32);
            this.carrierFreqLabel.TabIndex = 47;
            this.carrierFreqLabel.Text = "Carrier Frequency (Hz)";
            // 
            // powerLevelLabel
            // 
            this.powerLevelLabel.AutoSize = true;
            this.powerLevelLabel.Location = new System.Drawing.Point(21, 312);
            this.powerLevelLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.powerLevelLabel.Name = "powerLevelLabel";
            this.powerLevelLabel.Size = new System.Drawing.Size(253, 32);
            this.powerLevelLabel.TabIndex = 48;
            this.powerLevelLabel.Text = "Power Level (dBm)";
            // 
            // externalAttnLabel
            // 
            this.externalAttnLabel.AutoSize = true;
            this.externalAttnLabel.Location = new System.Drawing.Point(21, 365);
            this.externalAttnLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.externalAttnLabel.Name = "externalAttnLabel";
            this.externalAttnLabel.Size = new System.Drawing.Size(332, 32);
            this.externalAttnLabel.TabIndex = 51;
            this.externalAttnLabel.Text = "External Attenuation (dB)";
            // 
            // autoheadroomEnabLabel
            // 
            this.autoheadroomEnabLabel.AutoSize = true;
            this.autoheadroomEnabLabel.Location = new System.Drawing.Point(21, 417);
            this.autoheadroomEnabLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.autoheadroomEnabLabel.Name = "autoheadroomEnabLabel";
            this.autoheadroomEnabLabel.Size = new System.Drawing.Size(325, 32);
            this.autoheadroomEnabLabel.TabIndex = 52;
            this.autoheadroomEnabLabel.Text = "Auto Headroom Enabled";
            // 
            // headroomLabel
            // 
            this.headroomLabel.AutoSize = true;
            this.headroomLabel.Location = new System.Drawing.Point(21, 467);
            this.headroomLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.headroomLabel.Name = "headroomLabel";
            this.headroomLabel.Size = new System.Drawing.Size(206, 32);
            this.headroomLabel.TabIndex = 54;
            this.headroomLabel.Text = "Headroom (dB)";
            // 
            // refSourceLabel
            // 
            this.refSourceLabel.AutoSize = true;
            this.refSourceLabel.Location = new System.Drawing.Point(24, 656);
            this.refSourceLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.refSourceLabel.Name = "refSourceLabel";
            this.refSourceLabel.Size = new System.Drawing.Size(242, 32);
            this.refSourceLabel.TabIndex = 59;
            this.refSourceLabel.Text = "Reference Source";
            // 
            // clkTerminalLabel
            // 
            this.clkTerminalLabel.AutoSize = true;
            this.clkTerminalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clkTerminalLabel.Location = new System.Drawing.Point(21, 749);
            this.clkTerminalLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.clkTerminalLabel.Name = "clkTerminalLabel";
            this.clkTerminalLabel.Size = new System.Drawing.Size(306, 32);
            this.clkTerminalLabel.TabIndex = 77;
            this.clkTerminalLabel.Text = "Export Clock Settings";
            // 
            // allIqImpairEnLabel
            // 
            this.allIqImpairEnLabel.AutoSize = true;
            this.allIqImpairEnLabel.Location = new System.Drawing.Point(27, 935);
            this.allIqImpairEnLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.allIqImpairEnLabel.Name = "allIqImpairEnLabel";
            this.allIqImpairEnLabel.Size = new System.Drawing.Size(358, 32);
            this.allIqImpairEnLabel.TabIndex = 62;
            this.allIqImpairEnLabel.Text = "All IQ Impairments Enabled";
            // 
            // quadratureSkewLabel
            // 
            this.quadratureSkewLabel.AutoSize = true;
            this.quadratureSkewLabel.Location = new System.Drawing.Point(27, 987);
            this.quadratureSkewLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.quadratureSkewLabel.Name = "quadratureSkewLabel";
            this.quadratureSkewLabel.Size = new System.Drawing.Size(307, 32);
            this.quadratureSkewLabel.TabIndex = 64;
            this.quadratureSkewLabel.Text = "Quadrature Skew (deg)";
            // 
            // iDcOffsetLabel
            // 
            this.iDcOffsetLabel.AutoSize = true;
            this.iDcOffsetLabel.Location = new System.Drawing.Point(27, 1040);
            this.iDcOffsetLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.iDcOffsetLabel.Name = "iDcOffsetLabel";
            this.iDcOffsetLabel.Size = new System.Drawing.Size(201, 32);
            this.iDcOffsetLabel.TabIndex = 67;
            this.iDcOffsetLabel.Text = "I DC Offset (%)";
            // 
            // qDcOffsetLabel
            // 
            this.qDcOffsetLabel.AutoSize = true;
            this.qDcOffsetLabel.Location = new System.Drawing.Point(27, 1092);
            this.qDcOffsetLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.qDcOffsetLabel.Name = "qDcOffsetLabel";
            this.qDcOffsetLabel.Size = new System.Drawing.Size(216, 32);
            this.qDcOffsetLabel.TabIndex = 69;
            this.qDcOffsetLabel.Text = "Q DC Offset (%)";
            // 
            // iqGaimbalanceLabel
            // 
            this.iqGaimbalanceLabel.AutoSize = true;
            this.iqGaimbalanceLabel.Location = new System.Drawing.Point(27, 1145);
            this.iqGaimbalanceLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.iqGaimbalanceLabel.Name = "iqGaimbalanceLabel";
            this.iqGaimbalanceLabel.Size = new System.Drawing.Size(309, 32);
            this.iqGaimbalanceLabel.TabIndex = 70;
            this.iqGaimbalanceLabel.Text = "IQ Gain Imbalance (dB)";
            // 
            // carrierFreqOffLabel
            // 
            this.carrierFreqOffLabel.AutoSize = true;
            this.carrierFreqOffLabel.Location = new System.Drawing.Point(27, 1192);
            this.carrierFreqOffLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.carrierFreqOffLabel.Name = "carrierFreqOffLabel";
            this.carrierFreqOffLabel.Size = new System.Drawing.Size(383, 32);
            this.carrierFreqOffLabel.TabIndex = 72;
            this.carrierFreqOffLabel.Text = "Carrier Frequency Offset (Hz)";
            // 
            // awgnEnabledLabel
            // 
            this.awgnEnabledLabel.AutoSize = true;
            this.awgnEnabledLabel.Location = new System.Drawing.Point(27, 1242);
            this.awgnEnabledLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.awgnEnabledLabel.Name = "awgnEnabledLabel";
            this.awgnEnabledLabel.Size = new System.Drawing.Size(214, 32);
            this.awgnEnabledLabel.TabIndex = 73;
            this.awgnEnabledLabel.Text = "AWGN Enabled";
            // 
            // cnrLabel
            // 
            this.cnrLabel.AutoSize = true;
            this.cnrLabel.Location = new System.Drawing.Point(27, 1290);
            this.cnrLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.cnrLabel.Name = "cnrLabel";
            this.cnrLabel.Size = new System.Drawing.Size(345, 32);
            this.cnrLabel.TabIndex = 75;
            this.cnrLabel.Text = "Carrier to Noise Ratio (dB)";
            // 
            // hardwareLabel
            // 
            this.hardwareLabel.AutoSize = true;
            this.hardwareLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hardwareLabel.Location = new System.Drawing.Point(21, 21);
            this.hardwareLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.hardwareLabel.Name = "hardwareLabel";
            this.hardwareLabel.Size = new System.Drawing.Size(144, 32);
            this.hardwareLabel.TabIndex = 77;
            this.hardwareLabel.Text = "Hardware";
            // 
            // frequencySettingsLabel
            // 
            this.frequencySettingsLabel.AutoSize = true;
            this.frequencySettingsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frequencySettingsLabel.Location = new System.Drawing.Point(24, 610);
            this.frequencySettingsLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.frequencySettingsLabel.Name = "frequencySettingsLabel";
            this.frequencySettingsLabel.Size = new System.Drawing.Size(277, 32);
            this.frequencySettingsLabel.TabIndex = 77;
            this.frequencySettingsLabel.Text = "Frequency Settings";
            // 
            // impairmentsLabel
            // 
            this.impairmentsLabel.AutoSize = true;
            this.impairmentsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.impairmentsLabel.Location = new System.Drawing.Point(29, 885);
            this.impairmentsLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.impairmentsLabel.Name = "impairmentsLabel";
            this.impairmentsLabel.Size = new System.Drawing.Size(180, 32);
            this.impairmentsLabel.TabIndex = 77;
            this.impairmentsLabel.Text = "Impairments";
            // 
            // chnNumberNumeric
            // 
            this.chnNumberNumeric.Location = new System.Drawing.Point(408, 162);
            this.chnNumberNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.chnNumberNumeric.Maximum = new decimal(new int[] {
            39,
            0,
            0,
            0});
            this.chnNumberNumeric.Name = "chnNumberNumeric";
            this.chnNumberNumeric.Size = new System.Drawing.Size(240, 38);
            this.chnNumberNumeric.TabIndex = 45;
            this.chnNumberNumeric.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // powerLevelNumeric
            // 
            this.powerLevelNumeric.Location = new System.Drawing.Point(408, 305);
            this.powerLevelNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.powerLevelNumeric.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.powerLevelNumeric.Minimum = new decimal(new int[] {
            179,
            0,
            0,
            -2147483648});
            this.powerLevelNumeric.Name = "powerLevelNumeric";
            this.powerLevelNumeric.Size = new System.Drawing.Size(240, 38);
            this.powerLevelNumeric.TabIndex = 49;
            // 
            // externalAttnNumeric
            // 
            this.externalAttnNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.externalAttnNumeric.Location = new System.Drawing.Point(408, 358);
            this.externalAttnNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.externalAttnNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.externalAttnNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.externalAttnNumeric.Name = "externalAttnNumeric";
            this.externalAttnNumeric.Size = new System.Drawing.Size(240, 38);
            this.externalAttnNumeric.TabIndex = 50;
            // 
            // headroomNumeric
            // 
            this.headroomNumeric.Location = new System.Drawing.Point(408, 460);
            this.headroomNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.headroomNumeric.Name = "headroomNumeric";
            this.headroomNumeric.Size = new System.Drawing.Size(240, 38);
            this.headroomNumeric.TabIndex = 55;
            // 
            // quadratureSkewNumeric
            // 
            this.quadratureSkewNumeric.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.quadratureSkewNumeric.Location = new System.Drawing.Point(413, 985);
            this.quadratureSkewNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.quadratureSkewNumeric.Name = "quadratureSkewNumeric";
            this.quadratureSkewNumeric.Size = new System.Drawing.Size(240, 38);
            this.quadratureSkewNumeric.TabIndex = 65;
            // 
            // iDcOffsetNumeric
            // 
            this.iDcOffsetNumeric.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.iDcOffsetNumeric.Location = new System.Drawing.Point(413, 1037);
            this.iDcOffsetNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.iDcOffsetNumeric.Name = "iDcOffsetNumeric";
            this.iDcOffsetNumeric.Size = new System.Drawing.Size(240, 38);
            this.iDcOffsetNumeric.TabIndex = 66;
            // 
            // qDcOffsetNumeric
            // 
            this.qDcOffsetNumeric.Increment = new decimal(new int[] {
            50,
            0,
            0,
            131072});
            this.qDcOffsetNumeric.Location = new System.Drawing.Point(413, 1090);
            this.qDcOffsetNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.qDcOffsetNumeric.Name = "qDcOffsetNumeric";
            this.qDcOffsetNumeric.Size = new System.Drawing.Size(240, 38);
            this.qDcOffsetNumeric.TabIndex = 68;
            // 
            // iqGaimbalanceNumeric
            // 
            this.iqGaimbalanceNumeric.Increment = new decimal(new int[] {
            50,
            0,
            0,
            131072});
            this.iqGaimbalanceNumeric.Location = new System.Drawing.Point(413, 1142);
            this.iqGaimbalanceNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.iqGaimbalanceNumeric.Name = "iqGaimbalanceNumeric";
            this.iqGaimbalanceNumeric.Size = new System.Drawing.Size(240, 38);
            this.iqGaimbalanceNumeric.TabIndex = 71;
            // 
            // cnrNumeric
            // 
            this.cnrNumeric.Increment = new decimal(new int[] {
            50,
            0,
            0,
            131072});
            this.cnrNumeric.Location = new System.Drawing.Point(413, 1285);
            this.cnrNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.cnrNumeric.Name = "cnrNumeric";
            this.cnrNumeric.Size = new System.Drawing.Size(240, 38);
            this.cnrNumeric.TabIndex = 76;
            this.cnrNumeric.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // rfsgResourceTextBox
            // 
            this.rfsgResourceTextBox.Location = new System.Drawing.Point(408, 57);
            this.rfsgResourceTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.rfsgResourceTextBox.Name = "rfsgResourceTextBox";
            this.rfsgResourceTextBox.Size = new System.Drawing.Size(233, 38);
            this.rfsgResourceTextBox.TabIndex = 42;
            this.rfsgResourceTextBox.Text = "RFSG";
            // 
            // carrierFreqTextBox
            // 
            this.carrierFreqTextBox.Enabled = false;
            this.carrierFreqTextBox.Location = new System.Drawing.Point(408, 210);
            this.carrierFreqTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.carrierFreqTextBox.Name = "carrierFreqTextBox";
            this.carrierFreqTextBox.Size = new System.Drawing.Size(233, 38);
            this.carrierFreqTextBox.TabIndex = 46;
            this.carrierFreqTextBox.Text = "2.408E+9";
            // 
            // autoheadroomEnabComboBox
            // 
            this.autoheadroomEnabComboBox.Location = new System.Drawing.Point(408, 410);
            this.autoheadroomEnabComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.autoheadroomEnabComboBox.Name = "autoheadroomEnabComboBox";
            this.autoheadroomEnabComboBox.Size = new System.Drawing.Size(233, 39);
            this.autoheadroomEnabComboBox.TabIndex = 53;
            // 
            // refSourceComboBox
            // 
            this.refSourceComboBox.Location = new System.Drawing.Point(400, 651);
            this.refSourceComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.refSourceComboBox.Name = "refSourceComboBox";
            this.refSourceComboBox.Size = new System.Drawing.Size(233, 39);
            this.refSourceComboBox.TabIndex = 58;
            // 
            // clkOutTerminalComboBox
            // 
            this.clkOutTerminalComboBox.Location = new System.Drawing.Point(400, 794);
            this.clkOutTerminalComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.clkOutTerminalComboBox.Name = "clkOutTerminalComboBox";
            this.clkOutTerminalComboBox.Size = new System.Drawing.Size(233, 39);
            this.clkOutTerminalComboBox.TabIndex = 61;
            // 
            // allIqImpairEnComboBox
            // 
            this.allIqImpairEnComboBox.Location = new System.Drawing.Point(413, 932);
            this.allIqImpairEnComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.allIqImpairEnComboBox.Name = "allIqImpairEnComboBox";
            this.allIqImpairEnComboBox.Size = new System.Drawing.Size(233, 39);
            this.allIqImpairEnComboBox.TabIndex = 63;
            // 
            // awgnEnabledComboBox
            // 
            this.awgnEnabledComboBox.Location = new System.Drawing.Point(413, 1240);
            this.awgnEnabledComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.awgnEnabledComboBox.Name = "awgnEnabledComboBox";
            this.awgnEnabledComboBox.Size = new System.Drawing.Size(233, 39);
            this.awgnEnabledComboBox.TabIndex = 74;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.ProcessTimerEvent);
            // 
            // carrierFreqOffNumeric
            // 
            this.carrierFreqOffNumeric.Location = new System.Drawing.Point(413, 1188);
            this.carrierFreqOffNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.carrierFreqOffNumeric.Name = "carrierFreqOffNumeric";
            this.carrierFreqOffNumeric.Size = new System.Drawing.Size(240, 38);
            this.carrierFreqOffNumeric.TabIndex = 80;
            // 
            // payloadLengthModeLabel
            // 
            this.payloadLengthModeLabel.AutoSize = true;
            this.payloadLengthModeLabel.Location = new System.Drawing.Point(1392, 311);
            this.payloadLengthModeLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.payloadLengthModeLabel.Name = "payloadLengthModeLabel";
            this.payloadLengthModeLabel.Size = new System.Drawing.Size(291, 32);
            this.payloadLengthModeLabel.TabIndex = 82;
            this.payloadLengthModeLabel.Text = "Payload Length Mode";
            // 
            // waveNameLabel
            // 
            this.waveNameLabel.AutoSize = true;
            this.waveNameLabel.Location = new System.Drawing.Point(747, 289);
            this.waveNameLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.waveNameLabel.Name = "waveNameLabel";
            this.waveNameLabel.Size = new System.Drawing.Size(224, 32);
            this.waveNameLabel.TabIndex = 88;
            this.waveNameLabel.Text = "Waveform Name";
            // 
            // scriptLabel
            // 
            this.scriptLabel.AutoSize = true;
            this.scriptLabel.Location = new System.Drawing.Point(752, 610);
            this.scriptLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.scriptLabel.Name = "scriptLabel";
            this.scriptLabel.Size = new System.Drawing.Size(87, 32);
            this.scriptLabel.TabIndex = 85;
            this.scriptLabel.Text = "Script";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(760, 918);
            this.errorLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(76, 32);
            this.errorLabel.TabIndex = 89;
            this.errorLabel.Text = "Error";
            // 
            // waveNameTextBox
            // 
            this.waveNameTextBox.Location = new System.Drawing.Point(752, 322);
            this.waveNameTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.waveNameTextBox.Name = "waveNameTextBox";
            this.waveNameTextBox.Size = new System.Drawing.Size(313, 38);
            this.waveNameTextBox.TabIndex = 87;
            this.waveNameTextBox.Text = "LEHDT";
            // 
            // scriptTextBox
            // 
            this.scriptTextBox.Location = new System.Drawing.Point(752, 651);
            this.scriptTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.scriptTextBox.Multiline = true;
            this.scriptTextBox.Name = "scriptTextBox";
            this.scriptTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.scriptTextBox.Size = new System.Drawing.Size(471, 221);
            this.scriptTextBox.TabIndex = 86;
            this.scriptTextBox.TabStop = false;
            this.scriptTextBox.Text = "script GenerateLEPkt\r\n  repeat forever\r\n    generate LEHDT\r\n  end repeat\r\nend scr" +
    "ipt";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Location = new System.Drawing.Point(760, 968);
            this.errorTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.errorTextBox.Multiline = true;
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorTextBox.Size = new System.Drawing.Size(463, 247);
            this.errorTextBox.TabIndex = 90;
            this.errorTextBox.TabStop = false;
            this.errorTextBox.Text = "No Error";
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(760, 1266);
            this.generateButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(200, 55);
            this.generateButton.TabIndex = 93;
            this.generateButton.Text = "&Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(1000, 1266);
            this.stopButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(200, 55);
            this.stopButton.TabIndex = 94;
            this.stopButton.Text = "&Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // terminalConfigurationLabel
            // 
            this.terminalConfigurationLabel.AutoSize = true;
            this.terminalConfigurationLabel.Location = new System.Drawing.Point(747, 160);
            this.terminalConfigurationLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.terminalConfigurationLabel.Name = "terminalConfigurationLabel";
            this.terminalConfigurationLabel.Size = new System.Drawing.Size(303, 32);
            this.terminalConfigurationLabel.TabIndex = 100;
            this.terminalConfigurationLabel.Text = "Terminal Configuration";
            // 
            // terminalConfigurationComboBox
            // 
            this.terminalConfigurationComboBox.Items.AddRange(new object[] {
            "Differential",
            "Single-Ended"});
            this.terminalConfigurationComboBox.Location = new System.Drawing.Point(752, 193);
            this.terminalConfigurationComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.terminalConfigurationComboBox.Name = "terminalConfigurationComboBox";
            this.terminalConfigurationComboBox.Size = new System.Drawing.Size(313, 39);
            this.terminalConfigurationComboBox.TabIndex = 99;
            // 
            // outputPortLabel
            // 
            this.outputPortLabel.AutoSize = true;
            this.outputPortLabel.Location = new System.Drawing.Point(747, 64);
            this.outputPortLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.outputPortLabel.Name = "outputPortLabel";
            this.outputPortLabel.Size = new System.Drawing.Size(159, 32);
            this.outputPortLabel.TabIndex = 98;
            this.outputPortLabel.Text = "Output Port";
            // 
            // outputPortComboBox
            // 
            this.outputPortComboBox.Items.AddRange(new object[] {
            "RF Out",
            "IQ Out"});
            this.outputPortComboBox.Location = new System.Drawing.Point(752, 98);
            this.outputPortComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.outputPortComboBox.Name = "outputPortComboBox";
            this.outputPortComboBox.Size = new System.Drawing.Size(313, 39);
            this.outputPortComboBox.TabIndex = 97;
            // 
            // clkOutputTerminalLabel
            // 
            this.clkOutputTerminalLabel.AutoSize = true;
            this.clkOutputTerminalLabel.Location = new System.Drawing.Point(24, 801);
            this.clkOutputTerminalLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.clkOutputTerminalLabel.Name = "clkOutputTerminalLabel";
            this.clkOutputTerminalLabel.Size = new System.Drawing.Size(266, 32);
            this.clkOutputTerminalLabel.TabIndex = 59;
            this.clkOutputTerminalLabel.Text = "Clk Output Terminal";
            // 
            // actualHeadroomLabel
            // 
            this.actualHeadroomLabel.AutoSize = true;
            this.actualHeadroomLabel.Location = new System.Drawing.Point(21, 513);
            this.actualHeadroomLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.actualHeadroomLabel.Name = "actualHeadroomLabel";
            this.actualHeadroomLabel.Size = new System.Drawing.Size(293, 32);
            this.actualHeadroomLabel.TabIndex = 56;
            this.actualHeadroomLabel.Text = "Actual Headroom (dB)";
            // 
            // highDataThroughputLabel
            // 
            this.highDataThroughputLabel.AutoSize = true;
            this.highDataThroughputLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.highDataThroughputLabel.Location = new System.Drawing.Point(1997, 21);
            this.highDataThroughputLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.highDataThroughputLabel.Name = "highDataThroughputLabel";
            this.highDataThroughputLabel.Size = new System.Drawing.Size(313, 32);
            this.highDataThroughputLabel.TabIndex = 77;
            this.highDataThroughputLabel.Text = "High Data Throughput";
            // 
            // Index
            // 
            this.Index.MinimumWidth = 12;
            this.Index.Name = "Index";
            this.Index.Width = 250;
            // 
            // OversamplingFactorNumeric
            // 
            this.OversamplingFactorNumeric.Location = new System.Drawing.Point(752, 410);
            this.OversamplingFactorNumeric.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.OversamplingFactorNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.OversamplingFactorNumeric.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.OversamplingFactorNumeric.Name = "OversamplingFactorNumeric";
            this.OversamplingFactorNumeric.Size = new System.Drawing.Size(320, 38);
            this.OversamplingFactorNumeric.TabIndex = 83;
            this.OversamplingFactorNumeric.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // OversamplingFactorLabel
            // 
            this.OversamplingFactorLabel.AutoSize = true;
            this.OversamplingFactorLabel.Location = new System.Drawing.Point(744, 372);
            this.OversamplingFactorLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.OversamplingFactorLabel.Name = "OversamplingFactorLabel";
            this.OversamplingFactorLabel.Size = new System.Drawing.Size(277, 32);
            this.OversamplingFactorLabel.TabIndex = 84;
            this.OversamplingFactorLabel.Text = "Oversampling Factor";
            // 
            // actualHeadroomTextBox
            // 
            this.actualHeadroomTextBox.Enabled = false;
            this.actualHeadroomTextBox.Location = new System.Drawing.Point(408, 506);
            this.actualHeadroomTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.actualHeadroomTextBox.Name = "actualHeadroomTextBox";
            this.actualHeadroomTextBox.Size = new System.Drawing.Size(233, 38);
            this.actualHeadroomTextBox.TabIndex = 57;
            this.actualHeadroomTextBox.Text = "0.00";
            // 
            // dataRateLabel
            // 
            this.dataRateLabel.AutoSize = true;
            this.dataRateLabel.Location = new System.Drawing.Point(747, 467);
            this.dataRateLabel.Margin = new System.Windows.Forms.Padding(21, 0, 21, 0);
            this.dataRateLabel.Name = "dataRateLabel";
            this.dataRateLabel.Size = new System.Drawing.Size(212, 32);
            this.dataRateLabel.TabIndex = 45;
            this.dataRateLabel.Text = "Data Rate (bps)";
            // 
            // dataRateNumeric
            // 
            this.dataRateNumeric.Location = new System.Drawing.Point(752, 506);
            this.dataRateNumeric.Margin = new System.Windows.Forms.Padding(21, 17, 21, 17);
            this.dataRateNumeric.Maximum = new decimal(new int[] {
            7500000,
            0,
            0,
            0});
            this.dataRateNumeric.Name = "dataRateNumeric";
            this.dataRateNumeric.Size = new System.Drawing.Size(320, 38);
            this.dataRateNumeric.TabIndex = 43;
            this.dataRateNumeric.Value = new decimal(new int[] {
            2000000,
            0,
            0,
            0});
            // 
            // zadoffChuIndexLabel
            // 
            this.zadoffChuIndexLabel.AutoSize = true;
            this.zadoffChuIndexLabel.Location = new System.Drawing.Point(1997, 72);
            this.zadoffChuIndexLabel.Margin = new System.Windows.Forms.Padding(21, 0, 21, 0);
            this.zadoffChuIndexLabel.Name = "zadoffChuIndexLabel";
            this.zadoffChuIndexLabel.Size = new System.Drawing.Size(232, 32);
            this.zadoffChuIndexLabel.TabIndex = 45;
            this.zadoffChuIndexLabel.Text = "Zadoff-Chu Index";
            // 
            // zadoffChuIndexNumeric
            // 
            this.zadoffChuIndexNumeric.Location = new System.Drawing.Point(2362, 67);
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
            this.physicalChannelAddressLabel.Location = new System.Drawing.Point(1997, 119);
            this.physicalChannelAddressLabel.Margin = new System.Windows.Forms.Padding(21, 0, 21, 0);
            this.physicalChannelAddressLabel.Name = "physicalChannelAddressLabel";
            this.physicalChannelAddressLabel.Size = new System.Drawing.Size(346, 32);
            this.physicalChannelAddressLabel.TabIndex = 45;
            this.physicalChannelAddressLabel.Text = "Physical Channel Address";
            // 
            // physicalChannelAddressNumeric
            // 
            this.physicalChannelAddressNumeric.Hexadecimal = true;
            this.physicalChannelAddressNumeric.Location = new System.Drawing.Point(2362, 117);
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
            // HdtPhyIntervalLabel
            // 
            this.HdtPhyIntervalLabel.AutoSize = true;
            this.HdtPhyIntervalLabel.Location = new System.Drawing.Point(1997, 169);
            this.HdtPhyIntervalLabel.Margin = new System.Windows.Forms.Padding(21, 0, 21, 0);
            this.HdtPhyIntervalLabel.Name = "HdtPhyIntervalLabel";
            this.HdtPhyIntervalLabel.Size = new System.Drawing.Size(275, 32);
            this.HdtPhyIntervalLabel.TabIndex = 45;
            this.HdtPhyIntervalLabel.Text = "HDT PHY Interval (s)";
            // 
            // HdtPhyIntervalNumeric
            // 
            this.HdtPhyIntervalNumeric.DecimalPlaces = 10;
            this.HdtPhyIntervalNumeric.Location = new System.Drawing.Point(2362, 169);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(747, 21);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 32);
            this.label3.TabIndex = 102;
            this.label3.Text = "Output Port";
            // 
            // autoPayloadZoneProeprtiesSettingsLabel
            // 
            this.autoPayloadZoneProeprtiesSettingsLabel.AutoSize = true;
            this.autoPayloadZoneProeprtiesSettingsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoPayloadZoneProeprtiesSettingsLabel.Location = new System.Drawing.Point(1325, 256);
            this.autoPayloadZoneProeprtiesSettingsLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.autoPayloadZoneProeprtiesSettingsLabel.Name = "autoPayloadZoneProeprtiesSettingsLabel";
            this.autoPayloadZoneProeprtiesSettingsLabel.Size = new System.Drawing.Size(541, 32);
            this.autoPayloadZoneProeprtiesSettingsLabel.TabIndex = 103;
            this.autoPayloadZoneProeprtiesSettingsLabel.Text = "Auto Payload Zone Proeprties Settings";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(1340, 836);
            this.label5.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(655, 32);
            this.label5.TabIndex = 104;
            this.label5.Text = "User Defined Payload Zone Proeprties Settings";
            // 
            // payloadLengthInsertButton
            // 
            this.payloadLengthInsertButton.Location = new System.Drawing.Point(1346, 709);
            this.payloadLengthInsertButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.payloadLengthInsertButton.Name = "payloadLengthInsertButton";
            this.payloadLengthInsertButton.Size = new System.Drawing.Size(133, 52);
            this.payloadLengthInsertButton.TabIndex = 134;
            this.payloadLengthInsertButton.Text = "Insert";
            this.payloadLengthInsertButton.UseVisualStyleBackColor = true;
            this.payloadLengthInsertButton.Click += new System.EventHandler(this.payloadLengthInsertButton_Click);
            // 
            // payloadLengthDeleteButton
            // 
            this.payloadLengthDeleteButton.Location = new System.Drawing.Point(1495, 709);
            this.payloadLengthDeleteButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.payloadLengthDeleteButton.Name = "payloadLengthDeleteButton";
            this.payloadLengthDeleteButton.Size = new System.Drawing.Size(133, 52);
            this.payloadLengthDeleteButton.TabIndex = 135;
            this.payloadLengthDeleteButton.Text = "Delete";
            this.payloadLengthDeleteButton.UseVisualStyleBackColor = true;
            this.payloadLengthDeleteButton.Click += new System.EventHandler(this.payloadLengthDeleteButton_Click);
            // 
            // PayloadLengthBytesGrid
            // 
            this.PayloadLengthBytesGrid.AllowUserToAddRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PayloadLengthBytesGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.PayloadLengthBytesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PayloadLengthBytesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.payloadLengthIndex,
            this.payloadLengthBytesValues});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.PayloadLengthBytesGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.PayloadLengthBytesGrid.Location = new System.Drawing.Point(1346, 478);
            this.PayloadLengthBytesGrid.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.PayloadLengthBytesGrid.Name = "PayloadLengthBytesGrid";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PayloadLengthBytesGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.PayloadLengthBytesGrid.RowHeadersWidth = 102;
            this.PayloadLengthBytesGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.PayloadLengthBytesGrid.Size = new System.Drawing.Size(427, 212);
            this.PayloadLengthBytesGrid.TabIndex = 137;
            // 
            // payloadLengthIndex
            // 
            this.payloadLengthIndex.Frozen = true;
            this.payloadLengthIndex.HeaderText = "Index";
            this.payloadLengthIndex.MinimumWidth = 12;
            this.payloadLengthIndex.Name = "payloadLengthIndex";
            this.payloadLengthIndex.ReadOnly = true;
            this.payloadLengthIndex.Width = 40;
            // 
            // payloadLengthBytesValues
            // 
            this.payloadLengthBytesValues.Frozen = true;
            this.payloadLengthBytesValues.HeaderText = "Payload Length (bytes)";
            this.payloadLengthBytesValues.MinimumWidth = 12;
            this.payloadLengthBytesValues.Name = "payloadLengthBytesValues";
            this.payloadLengthBytesValues.Width = 250;
            // 
            // TxLenSequenceNumberInsertButton
            // 
            this.TxLenSequenceNumberInsertButton.Location = new System.Drawing.Point(1346, 1187);
            this.TxLenSequenceNumberInsertButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.TxLenSequenceNumberInsertButton.Name = "TxLenSequenceNumberInsertButton";
            this.TxLenSequenceNumberInsertButton.Size = new System.Drawing.Size(133, 52);
            this.TxLenSequenceNumberInsertButton.TabIndex = 138;
            this.TxLenSequenceNumberInsertButton.Text = "Insert";
            this.TxLenSequenceNumberInsertButton.UseVisualStyleBackColor = true;
            this.TxLenSequenceNumberInsertButton.Click += new System.EventHandler(this.txLenSequenceNumberInsertButton_Click);
            // 
            // TxLenSequenceNumberDeleteButton
            // 
            this.TxLenSequenceNumberDeleteButton.Location = new System.Drawing.Point(1495, 1187);
            this.TxLenSequenceNumberDeleteButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.TxLenSequenceNumberDeleteButton.Name = "TxLenSequenceNumberDeleteButton";
            this.TxLenSequenceNumberDeleteButton.Size = new System.Drawing.Size(133, 52);
            this.TxLenSequenceNumberDeleteButton.TabIndex = 139;
            this.TxLenSequenceNumberDeleteButton.Text = "Delete";
            this.TxLenSequenceNumberDeleteButton.UseVisualStyleBackColor = true;
            this.TxLenSequenceNumberDeleteButton.Click += new System.EventHandler(this.txLenSequenceNumberDeleteButton_Click);
            // 
            // TxLenSequenceNumberGrid
            // 
            this.TxLenSequenceNumberGrid.AllowUserToAddRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TxLenSequenceNumberGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.TxLenSequenceNumberGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TxLenSequenceNumberGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TxLenSequenceNumberIndex,
            this.TxLenSequenceNumberValues});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.TxLenSequenceNumberGrid.DefaultCellStyle = dataGridViewCellStyle5;
            this.TxLenSequenceNumberGrid.Location = new System.Drawing.Point(1346, 951);
            this.TxLenSequenceNumberGrid.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.TxLenSequenceNumberGrid.Name = "TxLenSequenceNumberGrid";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TxLenSequenceNumberGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.TxLenSequenceNumberGrid.RowHeadersWidth = 102;
            this.TxLenSequenceNumberGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxLenSequenceNumberGrid.Size = new System.Drawing.Size(412, 226);
            this.TxLenSequenceNumberGrid.TabIndex = 140;
            // 
            // TxLenSequenceNumberIndex
            // 
            this.TxLenSequenceNumberIndex.Frozen = true;
            this.TxLenSequenceNumberIndex.HeaderText = "Index";
            this.TxLenSequenceNumberIndex.MinimumWidth = 12;
            this.TxLenSequenceNumberIndex.Name = "TxLenSequenceNumberIndex";
            this.TxLenSequenceNumberIndex.ReadOnly = true;
            this.TxLenSequenceNumberIndex.Width = 40;
            // 
            // TxLenSequenceNumberValues
            // 
            this.TxLenSequenceNumberValues.Frozen = true;
            this.TxLenSequenceNumberValues.HeaderText = "Tx Len Sequence Number";
            this.TxLenSequenceNumberValues.MinimumWidth = 12;
            this.TxLenSequenceNumberValues.Name = "TxLenSequenceNumberValues";
            this.TxLenSequenceNumberValues.Width = 250;
            // 
            // payloadLengthLabel
            // 
            this.payloadLengthLabel.AutoSize = true;
            this.payloadLengthLabel.Location = new System.Drawing.Point(1390, 428);
            this.payloadLengthLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.payloadLengthLabel.Name = "payloadLengthLabel";
            this.payloadLengthLabel.Size = new System.Drawing.Size(306, 32);
            this.payloadLengthLabel.TabIndex = 141;
            this.payloadLengthLabel.Text = "Payload Length (bytes)";
            // 
            // TxLenSequenceNumberLabel
            // 
            this.TxLenSequenceNumberLabel.AutoSize = true;
            this.TxLenSequenceNumberLabel.Location = new System.Drawing.Point(1355, 901);
            this.TxLenSequenceNumberLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.TxLenSequenceNumberLabel.Name = "TxLenSequenceNumberLabel";
            this.TxLenSequenceNumberLabel.Size = new System.Drawing.Size(336, 32);
            this.TxLenSequenceNumberLabel.TabIndex = 142;
            this.TxLenSequenceNumberLabel.Text = "TxLen Sequence Number";
            // 
            // NumebrOfBlocksLabel
            // 
            this.NumebrOfBlocksLabel.AutoSize = true;
            this.NumebrOfBlocksLabel.Location = new System.Drawing.Point(1857, 901);
            this.NumebrOfBlocksLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.NumebrOfBlocksLabel.Name = "NumebrOfBlocksLabel";
            this.NumebrOfBlocksLabel.Size = new System.Drawing.Size(236, 32);
            this.NumebrOfBlocksLabel.TabIndex = 146;
            this.NumebrOfBlocksLabel.Text = "Number of Blocks";
            // 
            // NumberOfBlocksInsertButton
            // 
            this.NumberOfBlocksInsertButton.Location = new System.Drawing.Point(1848, 1159);
            this.NumberOfBlocksInsertButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.NumberOfBlocksInsertButton.Name = "NumberOfBlocksInsertButton";
            this.NumberOfBlocksInsertButton.Size = new System.Drawing.Size(133, 52);
            this.NumberOfBlocksInsertButton.TabIndex = 143;
            this.NumberOfBlocksInsertButton.Text = "Insert";
            this.NumberOfBlocksInsertButton.UseVisualStyleBackColor = true;
            this.NumberOfBlocksInsertButton.Click += new System.EventHandler(this.numberOfBlocksInsertButton_Click);
            // 
            // NumberOfBlocksDeleteButton
            // 
            this.NumberOfBlocksDeleteButton.Location = new System.Drawing.Point(1997, 1159);
            this.NumberOfBlocksDeleteButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.NumberOfBlocksDeleteButton.Name = "NumberOfBlocksDeleteButton";
            this.NumberOfBlocksDeleteButton.Size = new System.Drawing.Size(133, 52);
            this.NumberOfBlocksDeleteButton.TabIndex = 144;
            this.NumberOfBlocksDeleteButton.Text = "Delete";
            this.NumberOfBlocksDeleteButton.UseVisualStyleBackColor = true;
            this.NumberOfBlocksDeleteButton.Click += new System.EventHandler(this.numberOfBlocksDeleteButton_Click);
            // 
            // NumberOfBlocksGrid
            // 
            this.NumberOfBlocksGrid.AllowUserToAddRows = false;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.NumberOfBlocksGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.NumberOfBlocksGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.NumberOfBlocksGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NumberOfBlocksIndex,
            this.NumberOfBlocksValues});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.NumberOfBlocksGrid.DefaultCellStyle = dataGridViewCellStyle8;
            this.NumberOfBlocksGrid.Location = new System.Drawing.Point(1848, 951);
            this.NumberOfBlocksGrid.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.NumberOfBlocksGrid.Name = "NumberOfBlocksGrid";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.NumberOfBlocksGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.NumberOfBlocksGrid.RowHeadersWidth = 102;
            this.NumberOfBlocksGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.NumberOfBlocksGrid.Size = new System.Drawing.Size(427, 194);
            this.NumberOfBlocksGrid.TabIndex = 145;
            // 
            // NumberOfBlocksIndex
            // 
            this.NumberOfBlocksIndex.Frozen = true;
            this.NumberOfBlocksIndex.HeaderText = "Index";
            this.NumberOfBlocksIndex.MinimumWidth = 12;
            this.NumberOfBlocksIndex.Name = "NumberOfBlocksIndex";
            this.NumberOfBlocksIndex.ReadOnly = true;
            this.NumberOfBlocksIndex.Width = 40;
            // 
            // NumberOfBlocksValues
            // 
            this.NumberOfBlocksValues.Frozen = true;
            this.NumberOfBlocksValues.HeaderText = "Number of Blocks";
            this.NumberOfBlocksValues.MinimumWidth = 12;
            this.NumberOfBlocksValues.Name = "NumberOfBlocksValues";
            this.NumberOfBlocksValues.Width = 250;
            // 
            // BlockSizeLabel
            // 
            this.BlockSizeLabel.AutoSize = true;
            this.BlockSizeLabel.Location = new System.Drawing.Point(1355, 1288);
            this.BlockSizeLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.BlockSizeLabel.Name = "BlockSizeLabel";
            this.BlockSizeLabel.Size = new System.Drawing.Size(240, 32);
            this.BlockSizeLabel.TabIndex = 150;
            this.BlockSizeLabel.Text = "Block Size (bytes)";
            // 
            // BlockSizeInsertButton
            // 
            this.BlockSizeInsertButton.Location = new System.Drawing.Point(1352, 1560);
            this.BlockSizeInsertButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.BlockSizeInsertButton.Name = "BlockSizeInsertButton";
            this.BlockSizeInsertButton.Size = new System.Drawing.Size(133, 52);
            this.BlockSizeInsertButton.TabIndex = 147;
            this.BlockSizeInsertButton.Text = "Insert";
            this.BlockSizeInsertButton.UseVisualStyleBackColor = true;
            this.BlockSizeInsertButton.Click += new System.EventHandler(this.blockSizeInsertButton_Click);
            // 
            // BlockSizeDeleteButton
            // 
            this.BlockSizeDeleteButton.Location = new System.Drawing.Point(1501, 1560);
            this.BlockSizeDeleteButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.BlockSizeDeleteButton.Name = "BlockSizeDeleteButton";
            this.BlockSizeDeleteButton.Size = new System.Drawing.Size(133, 52);
            this.BlockSizeDeleteButton.TabIndex = 148;
            this.BlockSizeDeleteButton.Text = "Delete";
            this.BlockSizeDeleteButton.UseVisualStyleBackColor = true;
            this.BlockSizeDeleteButton.Click += new System.EventHandler(this.blockSizeDeleteButton_Click);
            // 
            // BlockSizeGrid
            // 
            this.BlockSizeGrid.AllowUserToAddRows = false;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.BlockSizeGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.BlockSizeGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BlockSizeGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BlockSizeIndex,
            this.BlockSizeValues});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.BlockSizeGrid.DefaultCellStyle = dataGridViewCellStyle11;
            this.BlockSizeGrid.Location = new System.Drawing.Point(1346, 1338);
            this.BlockSizeGrid.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.BlockSizeGrid.Name = "BlockSizeGrid";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.BlockSizeGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.BlockSizeGrid.RowHeadersWidth = 102;
            this.BlockSizeGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.BlockSizeGrid.Size = new System.Drawing.Size(427, 208);
            this.BlockSizeGrid.TabIndex = 149;
            // 
            // BlockSizeIndex
            // 
            this.BlockSizeIndex.Frozen = true;
            this.BlockSizeIndex.HeaderText = "Index";
            this.BlockSizeIndex.MinimumWidth = 12;
            this.BlockSizeIndex.Name = "BlockSizeIndex";
            this.BlockSizeIndex.ReadOnly = true;
            this.BlockSizeIndex.Width = 40;
            // 
            // BlockSizeValues
            // 
            this.BlockSizeValues.Frozen = true;
            this.BlockSizeValues.HeaderText = "Block Size (bytes)";
            this.BlockSizeValues.MinimumWidth = 12;
            this.BlockSizeValues.Name = "BlockSizeValues";
            this.BlockSizeValues.Width = 250;
            // 
            // LastBlockSizeLabel
            // 
            this.LastBlockSizeLabel.AutoSize = true;
            this.LastBlockSizeLabel.Location = new System.Drawing.Point(1872, 1288);
            this.LastBlockSizeLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.LastBlockSizeLabel.Name = "LastBlockSizeLabel";
            this.LastBlockSizeLabel.Size = new System.Drawing.Size(301, 32);
            this.LastBlockSizeLabel.TabIndex = 154;
            this.LastBlockSizeLabel.Text = "Last Block Size (bytes)";
            // 
            // LastBlockSizeInsertButton
            // 
            this.LastBlockSizeInsertButton.Location = new System.Drawing.Point(1869, 1560);
            this.LastBlockSizeInsertButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.LastBlockSizeInsertButton.Name = "LastBlockSizeInsertButton";
            this.LastBlockSizeInsertButton.Size = new System.Drawing.Size(133, 52);
            this.LastBlockSizeInsertButton.TabIndex = 151;
            this.LastBlockSizeInsertButton.Text = "Insert";
            this.LastBlockSizeInsertButton.UseVisualStyleBackColor = true;
            this.LastBlockSizeInsertButton.Click += new System.EventHandler(this.lastBlockSizeInsertButton_Click);
            // 
            // LastBlockSizeDeleteButton
            // 
            this.LastBlockSizeDeleteButton.Location = new System.Drawing.Point(2018, 1560);
            this.LastBlockSizeDeleteButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.LastBlockSizeDeleteButton.Name = "LastBlockSizeDeleteButton";
            this.LastBlockSizeDeleteButton.Size = new System.Drawing.Size(133, 52);
            this.LastBlockSizeDeleteButton.TabIndex = 152;
            this.LastBlockSizeDeleteButton.Text = "Delete";
            this.LastBlockSizeDeleteButton.UseVisualStyleBackColor = true;
            this.LastBlockSizeDeleteButton.Click += new System.EventHandler(this.lastBlockSizeDeleteButton_Click);
            // 
            // LastBlockSizeGrid
            // 
            this.LastBlockSizeGrid.AllowUserToAddRows = false;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.LastBlockSizeGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.LastBlockSizeGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LastBlockSizeGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LastBlockSizeIndex,
            this.LastBlockSizeValues});
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.LastBlockSizeGrid.DefaultCellStyle = dataGridViewCellStyle14;
            this.LastBlockSizeGrid.Location = new System.Drawing.Point(1863, 1338);
            this.LastBlockSizeGrid.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.LastBlockSizeGrid.Name = "LastBlockSizeGrid";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.LastBlockSizeGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.LastBlockSizeGrid.RowHeadersWidth = 102;
            this.LastBlockSizeGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LastBlockSizeGrid.Size = new System.Drawing.Size(427, 208);
            this.LastBlockSizeGrid.TabIndex = 153;
            // 
            // LastBlockSizeIndex
            // 
            this.LastBlockSizeIndex.Frozen = true;
            this.LastBlockSizeIndex.HeaderText = "Index";
            this.LastBlockSizeIndex.MinimumWidth = 12;
            this.LastBlockSizeIndex.Name = "LastBlockSizeIndex";
            this.LastBlockSizeIndex.ReadOnly = true;
            this.LastBlockSizeIndex.Width = 40;
            // 
            // LastBlockSizeValues
            // 
            this.LastBlockSizeValues.Frozen = true;
            this.LastBlockSizeValues.HeaderText = "Last Block Size (bytes)";
            this.LastBlockSizeValues.MinimumWidth = 12;
            this.LastBlockSizeValues.Name = "LastBlockSizeValues";
            this.LastBlockSizeValues.Width = 250;
            // 
            // TxBlockMapLabel
            // 
            this.TxBlockMapLabel.AutoSize = true;
            this.TxBlockMapLabel.Location = new System.Drawing.Point(2359, 901);
            this.TxBlockMapLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.TxBlockMapLabel.Name = "TxBlockMapLabel";
            this.TxBlockMapLabel.Size = new System.Drawing.Size(177, 32);
            this.TxBlockMapLabel.TabIndex = 158;
            this.TxBlockMapLabel.Text = "TxBlock Map";
            // 
            // TxBlockMapInsertButton
            // 
            this.TxBlockMapInsertButton.Location = new System.Drawing.Point(2350, 1159);
            this.TxBlockMapInsertButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.TxBlockMapInsertButton.Name = "TxBlockMapInsertButton";
            this.TxBlockMapInsertButton.Size = new System.Drawing.Size(133, 52);
            this.TxBlockMapInsertButton.TabIndex = 155;
            this.TxBlockMapInsertButton.Text = "Insert";
            this.TxBlockMapInsertButton.UseVisualStyleBackColor = true;
            this.TxBlockMapInsertButton.Click += new System.EventHandler(this.txBlockMapInsertButton_Click);
            // 
            // TxBlockMapDeleteButton
            // 
            this.TxBlockMapDeleteButton.Location = new System.Drawing.Point(2499, 1159);
            this.TxBlockMapDeleteButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.TxBlockMapDeleteButton.Name = "TxBlockMapDeleteButton";
            this.TxBlockMapDeleteButton.Size = new System.Drawing.Size(133, 52);
            this.TxBlockMapDeleteButton.TabIndex = 156;
            this.TxBlockMapDeleteButton.Text = "Delete";
            this.TxBlockMapDeleteButton.UseVisualStyleBackColor = true;
            this.TxBlockMapDeleteButton.Click += new System.EventHandler(this.txBlockMapDeleteButton_Click);
            // 
            // TxBlockMapGrid
            // 
            this.TxBlockMapGrid.AllowUserToAddRows = false;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TxBlockMapGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.TxBlockMapGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TxBlockMapGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TxBlockMapIndex,
            this.TxBlockMapValues});
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.TxBlockMapGrid.DefaultCellStyle = dataGridViewCellStyle17;
            this.TxBlockMapGrid.Location = new System.Drawing.Point(2350, 951);
            this.TxBlockMapGrid.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.TxBlockMapGrid.Name = "TxBlockMapGrid";
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TxBlockMapGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.TxBlockMapGrid.RowHeadersWidth = 102;
            this.TxBlockMapGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxBlockMapGrid.Size = new System.Drawing.Size(427, 194);
            this.TxBlockMapGrid.TabIndex = 157;
            // 
            // TxBlockMapIndex
            // 
            this.TxBlockMapIndex.Frozen = true;
            this.TxBlockMapIndex.HeaderText = "Index";
            this.TxBlockMapIndex.MinimumWidth = 12;
            this.TxBlockMapIndex.Name = "TxBlockMapIndex";
            this.TxBlockMapIndex.ReadOnly = true;
            this.TxBlockMapIndex.Width = 40;
            // 
            // TxBlockMapValues
            // 
            this.TxBlockMapValues.Frozen = true;
            this.TxBlockMapValues.HeaderText = "TxBlock Map";
            this.TxBlockMapValues.MinimumWidth = 12;
            this.TxBlockMapValues.Name = "TxBlockMapValues";
            this.TxBlockMapValues.Width = 250;
            // 
            // ActualPayloadLengthLabel
            // 
            this.ActualPayloadLengthLabel.AutoSize = true;
            this.ActualPayloadLengthLabel.Location = new System.Drawing.Point(2217, 417);
            this.ActualPayloadLengthLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.ActualPayloadLengthLabel.Name = "ActualPayloadLengthLabel";
            this.ActualPayloadLengthLabel.Size = new System.Drawing.Size(393, 32);
            this.ActualPayloadLengthLabel.TabIndex = 164;
            this.ActualPayloadLengthLabel.Text = "Actual Payload Length (bytes)";
            // 
            // ActualPayloadLengthInsertButton
            // 
            this.ActualPayloadLengthInsertButton.Location = new System.Drawing.Point(2205, 691);
            this.ActualPayloadLengthInsertButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.ActualPayloadLengthInsertButton.Name = "ActualPayloadLengthInsertButton";
            this.ActualPayloadLengthInsertButton.Size = new System.Drawing.Size(133, 52);
            this.ActualPayloadLengthInsertButton.TabIndex = 161;
            this.ActualPayloadLengthInsertButton.Text = "Insert";
            this.ActualPayloadLengthInsertButton.UseVisualStyleBackColor = true;
            this.ActualPayloadLengthInsertButton.Click += new System.EventHandler(this.actualPayloadLengthInsertButton_Click);
            // 
            // ActualPayloadLengthDeleteButton
            // 
            this.ActualPayloadLengthDeleteButton.Location = new System.Drawing.Point(2354, 691);
            this.ActualPayloadLengthDeleteButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.ActualPayloadLengthDeleteButton.Name = "ActualPayloadLengthDeleteButton";
            this.ActualPayloadLengthDeleteButton.Size = new System.Drawing.Size(133, 52);
            this.ActualPayloadLengthDeleteButton.TabIndex = 162;
            this.ActualPayloadLengthDeleteButton.Text = "Delete";
            this.ActualPayloadLengthDeleteButton.UseVisualStyleBackColor = true;
            this.ActualPayloadLengthDeleteButton.Click += new System.EventHandler(this.actualPayloadLengthDeleteButton_Click);
            // 
            // ActualPayloadLengthGrid
            // 
            this.ActualPayloadLengthGrid.AllowUserToAddRows = false;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ActualPayloadLengthGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.ActualPayloadLengthGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ActualPayloadLengthGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ActualPayloadLengthIndex,
            this.ActualPayloadLengthValues});
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ActualPayloadLengthGrid.DefaultCellStyle = dataGridViewCellStyle20;
            this.ActualPayloadLengthGrid.Location = new System.Drawing.Point(2205, 460);
            this.ActualPayloadLengthGrid.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.ActualPayloadLengthGrid.Name = "ActualPayloadLengthGrid";
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ActualPayloadLengthGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle21;
            this.ActualPayloadLengthGrid.RowHeadersWidth = 102;
            this.ActualPayloadLengthGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ActualPayloadLengthGrid.Size = new System.Drawing.Size(427, 212);
            this.ActualPayloadLengthGrid.TabIndex = 163;
            // 
            // ActualPayloadLengthIndex
            // 
            this.ActualPayloadLengthIndex.Frozen = true;
            this.ActualPayloadLengthIndex.HeaderText = "Index";
            this.ActualPayloadLengthIndex.MinimumWidth = 12;
            this.ActualPayloadLengthIndex.Name = "ActualPayloadLengthIndex";
            this.ActualPayloadLengthIndex.ReadOnly = true;
            this.ActualPayloadLengthIndex.Width = 40;
            // 
            // ActualPayloadLengthValues
            // 
            this.ActualPayloadLengthValues.Frozen = true;
            this.ActualPayloadLengthValues.HeaderText = "Actual Payload Length (bytes)";
            this.ActualPayloadLengthValues.MinimumWidth = 12;
            this.ActualPayloadLengthValues.Name = "ActualPayloadLengthValues";
            this.ActualPayloadLengthValues.Width = 250;
            // 
            // PayloadZoneLengthLabel
            // 
            this.PayloadZoneLengthLabel.AutoSize = true;
            this.PayloadZoneLengthLabel.Location = new System.Drawing.Point(2217, 312);
            this.PayloadZoneLengthLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.PayloadZoneLengthLabel.Name = "PayloadZoneLengthLabel";
            this.PayloadZoneLengthLabel.Size = new System.Drawing.Size(378, 32);
            this.PayloadZoneLengthLabel.TabIndex = 160;
            this.PayloadZoneLengthLabel.Text = "Payload Zone Length (bytes)";
            // 
            // PayloadZoneLengthValue
            // 
            this.PayloadZoneLengthValue.Location = new System.Drawing.Point(2282, 358);
            this.PayloadZoneLengthValue.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.PayloadZoneLengthValue.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.PayloadZoneLengthValue.Name = "PayloadZoneLengthValue";
            this.PayloadZoneLengthValue.Size = new System.Drawing.Size(240, 38);
            this.PayloadZoneLengthValue.TabIndex = 165;
            // 
            // Format1PayloadZoneConfigurationModeComboBox
            // 
            this.Format1PayloadZoneConfigurationModeComboBox.Items.AddRange(new object[] {
            "Auto",
            "User Defined"});
            this.Format1PayloadZoneConfigurationModeComboBox.Location = new System.Drawing.Point(1302, 174);
            this.Format1PayloadZoneConfigurationModeComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Format1PayloadZoneConfigurationModeComboBox.Name = "Format1PayloadZoneConfigurationModeComboBox";
            this.Format1PayloadZoneConfigurationModeComboBox.Size = new System.Drawing.Size(233, 39);
            this.Format1PayloadZoneConfigurationModeComboBox.TabIndex = 177;
            // 
            // Format1PayloadZoneConfigurationModeLabel
            // 
            this.Format1PayloadZoneConfigurationModeLabel.AutoSize = true;
            this.Format1PayloadZoneConfigurationModeLabel.Location = new System.Drawing.Point(1293, 135);
            this.Format1PayloadZoneConfigurationModeLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.Format1PayloadZoneConfigurationModeLabel.Name = "Format1PayloadZoneConfigurationModeLabel";
            this.Format1PayloadZoneConfigurationModeLabel.Size = new System.Drawing.Size(558, 32);
            this.Format1PayloadZoneConfigurationModeLabel.TabIndex = 175;
            this.Format1PayloadZoneConfigurationModeLabel.Text = "Format1 Payload Zone Configuration Mode";
            // 
            // NumberOfPayloadsLabel
            // 
            this.NumberOfPayloadsLabel.AutoSize = true;
            this.NumberOfPayloadsLabel.Location = new System.Drawing.Point(1296, 34);
            this.NumberOfPayloadsLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.NumberOfPayloadsLabel.Name = "NumberOfPayloadsLabel";
            this.NumberOfPayloadsLabel.Size = new System.Drawing.Size(270, 32);
            this.NumberOfPayloadsLabel.TabIndex = 176;
            this.NumberOfPayloadsLabel.Text = "Number of Payloads";
            // 
            // numberOfPayloadNumericUpDown
            // 
            this.numberOfPayloadNumericUpDown.Location = new System.Drawing.Point(1299, 72);
            this.numberOfPayloadNumericUpDown.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.numberOfPayloadNumericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numberOfPayloadNumericUpDown.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.numberOfPayloadNumericUpDown.Name = "numberOfPayloadNumericUpDown";
            this.numberOfPayloadNumericUpDown.Size = new System.Drawing.Size(320, 38);
            this.numberOfPayloadNumericUpDown.TabIndex = 174;
            this.numberOfPayloadNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // payloadLengthModeComboBox
            // 
            this.payloadLengthModeComboBox.Items.AddRange(new object[] {
            "Maximum Length",
            "User Defined"});
            this.payloadLengthModeComboBox.Location = new System.Drawing.Point(1378, 357);
            this.payloadLengthModeComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.payloadLengthModeComboBox.Name = "payloadLengthModeComboBox";
            this.payloadLengthModeComboBox.Size = new System.Drawing.Size(313, 39);
            this.payloadLengthModeComboBox.TabIndex = 178;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(3242, 1925);
            this.Controls.Add(this.payloadLengthModeComboBox);
            this.Controls.Add(this.Format1PayloadZoneConfigurationModeComboBox);
            this.Controls.Add(this.Format1PayloadZoneConfigurationModeLabel);
            this.Controls.Add(this.NumberOfPayloadsLabel);
            this.Controls.Add(this.numberOfPayloadNumericUpDown);
            this.Controls.Add(this.PayloadZoneLengthValue);
            this.Controls.Add(this.ActualPayloadLengthLabel);
            this.Controls.Add(this.ActualPayloadLengthInsertButton);
            this.Controls.Add(this.ActualPayloadLengthDeleteButton);
            this.Controls.Add(this.ActualPayloadLengthGrid);
            this.Controls.Add(this.PayloadZoneLengthLabel);
            this.Controls.Add(this.TxBlockMapLabel);
            this.Controls.Add(this.TxBlockMapInsertButton);
            this.Controls.Add(this.TxBlockMapDeleteButton);
            this.Controls.Add(this.TxBlockMapGrid);
            this.Controls.Add(this.LastBlockSizeLabel);
            this.Controls.Add(this.LastBlockSizeInsertButton);
            this.Controls.Add(this.LastBlockSizeDeleteButton);
            this.Controls.Add(this.LastBlockSizeGrid);
            this.Controls.Add(this.BlockSizeLabel);
            this.Controls.Add(this.BlockSizeInsertButton);
            this.Controls.Add(this.BlockSizeDeleteButton);
            this.Controls.Add(this.BlockSizeGrid);
            this.Controls.Add(this.NumebrOfBlocksLabel);
            this.Controls.Add(this.NumberOfBlocksInsertButton);
            this.Controls.Add(this.NumberOfBlocksDeleteButton);
            this.Controls.Add(this.NumberOfBlocksGrid);
            this.Controls.Add(this.TxLenSequenceNumberLabel);
            this.Controls.Add(this.payloadLengthLabel);
            this.Controls.Add(this.TxLenSequenceNumberInsertButton);
            this.Controls.Add(this.TxLenSequenceNumberDeleteButton);
            this.Controls.Add(this.TxLenSequenceNumberGrid);
            this.Controls.Add(this.payloadLengthInsertButton);
            this.Controls.Add(this.payloadLengthDeleteButton);
            this.Controls.Add(this.PayloadLengthBytesGrid);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.autoPayloadZoneProeprtiesSettingsLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.terminalConfigurationLabel);
            this.Controls.Add(this.terminalConfigurationComboBox);
            this.Controls.Add(this.outputPortLabel);
            this.Controls.Add(this.outputPortComboBox);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.highDataThroughputLabel);
            this.Controls.Add(this.payloadLengthModeLabel);
            this.Controls.Add(this.OversamplingFactorLabel);
            this.Controls.Add(this.waveNameLabel);
            this.Controls.Add(this.scriptLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.OversamplingFactorNumeric);
            this.Controls.Add(this.waveNameTextBox);
            this.Controls.Add(this.scriptTextBox);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.carrierFreqOffNumeric);
            this.Controls.Add(this.rfsgResourceLabel);
            this.Controls.Add(this.chnNumberLabel);
            this.Controls.Add(this.carrierFreqLabel);
            this.Controls.Add(this.powerLevelLabel);
            this.Controls.Add(this.externalAttnLabel);
            this.Controls.Add(this.autoheadroomEnabLabel);
            this.Controls.Add(this.headroomLabel);
            this.Controls.Add(this.actualHeadroomLabel);
            this.Controls.Add(this.clkOutputTerminalLabel);
            this.Controls.Add(this.refSourceLabel);
            this.Controls.Add(this.clkTerminalLabel);
            this.Controls.Add(this.allIqImpairEnLabel);
            this.Controls.Add(this.quadratureSkewLabel);
            this.Controls.Add(this.iDcOffsetLabel);
            this.Controls.Add(this.qDcOffsetLabel);
            this.Controls.Add(this.iqGaimbalanceLabel);
            this.Controls.Add(this.carrierFreqOffLabel);
            this.Controls.Add(this.awgnEnabledLabel);
            this.Controls.Add(this.cnrLabel);
            this.Controls.Add(this.hardwareLabel);
            this.Controls.Add(this.frequencySettingsLabel);
            this.Controls.Add(this.impairmentsLabel);
            this.Controls.Add(this.chnNumberNumeric);
            this.Controls.Add(this.powerLevelNumeric);
            this.Controls.Add(this.externalAttnNumeric);
            this.Controls.Add(this.headroomNumeric);
            this.Controls.Add(this.quadratureSkewNumeric);
            this.Controls.Add(this.iDcOffsetNumeric);
            this.Controls.Add(this.qDcOffsetNumeric);
            this.Controls.Add(this.iqGaimbalanceNumeric);
            this.Controls.Add(this.cnrNumeric);
            this.Controls.Add(this.rfsgResourceTextBox);
            this.Controls.Add(this.carrierFreqTextBox);
            this.Controls.Add(this.actualHeadroomTextBox);
            this.Controls.Add(this.autoheadroomEnabComboBox);
            this.Controls.Add(this.refSourceComboBox);
            this.Controls.Add(this.clkOutTerminalComboBox);
            this.Controls.Add(this.allIqImpairEnComboBox);
            this.Controls.Add(this.awgnEnabledComboBox);
            this.Controls.Add(this.dataRateLabel);
            this.Controls.Add(this.dataRateNumeric);
            this.Controls.Add(this.zadoffChuIndexLabel);
            this.Controls.Add(this.zadoffChuIndexNumeric);
            this.Controls.Add(this.physicalChannelAddressLabel);
            this.Controls.Add(this.physicalChannelAddressNumeric);
            this.Controls.Add(this.HdtPhyIntervalLabel);
            this.Controls.Add(this.HdtPhyIntervalNumeric);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "MainForm";
            this.Text = "LE HDT Example";
            ((System.ComponentModel.ISupportInitialize)(this.chnNumberNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.powerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.externalAttnNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.headroomNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.quadratureSkewNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iDcOffsetNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qDcOffsetNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iqGaimbalanceNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cnrNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierFreqOffNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OversamplingFactorNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zadoffChuIndexNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.physicalChannelAddressNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HdtPhyIntervalNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PayloadLengthBytesGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxLenSequenceNumberGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfBlocksGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BlockSizeGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastBlockSizeGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxBlockMapGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActualPayloadLengthGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PayloadZoneLengthValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfPayloadNumericUpDown)).EndInit();
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
        private System.Windows.Forms.Label refSourceLabel;
        private System.Windows.Forms.Label clkTerminalLabel;
        private System.Windows.Forms.Label allIqImpairEnLabel;
        private System.Windows.Forms.Label quadratureSkewLabel;
        private System.Windows.Forms.Label iDcOffsetLabel;
        private System.Windows.Forms.Label qDcOffsetLabel;
        private System.Windows.Forms.Label iqGaimbalanceLabel;
        private System.Windows.Forms.Label carrierFreqOffLabel;
        private System.Windows.Forms.Label awgnEnabledLabel;
        private System.Windows.Forms.Label cnrLabel;
        private System.Windows.Forms.Label hardwareLabel;
        private System.Windows.Forms.Label frequencySettingsLabel;
        private System.Windows.Forms.Label impairmentsLabel;
        private System.Windows.Forms.NumericUpDown chnNumberNumeric;
        private System.Windows.Forms.NumericUpDown powerLevelNumeric;
        private System.Windows.Forms.NumericUpDown externalAttnNumeric;
        private System.Windows.Forms.NumericUpDown headroomNumeric;
        private System.Windows.Forms.NumericUpDown quadratureSkewNumeric;
        private System.Windows.Forms.NumericUpDown iDcOffsetNumeric;
        private System.Windows.Forms.NumericUpDown qDcOffsetNumeric;
        private System.Windows.Forms.NumericUpDown iqGaimbalanceNumeric;
        private System.Windows.Forms.NumericUpDown cnrNumeric;
        private System.Windows.Forms.TextBox rfsgResourceTextBox;
        private System.Windows.Forms.TextBox carrierFreqTextBox;
        private System.Windows.Forms.ComboBox autoheadroomEnabComboBox;
        private System.Windows.Forms.ComboBox refSourceComboBox;
        private System.Windows.Forms.ComboBox clkOutTerminalComboBox;
        private System.Windows.Forms.ComboBox allIqImpairEnComboBox;
        private System.Windows.Forms.ComboBox awgnEnabledComboBox;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.NumericUpDown carrierFreqOffNumeric;
        private System.Windows.Forms.Label payloadLengthModeLabel;
        private System.Windows.Forms.Label waveNameLabel;
        private System.Windows.Forms.Label scriptLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.TextBox waveNameTextBox;
        private System.Windows.Forms.TextBox scriptTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label terminalConfigurationLabel;
        private System.Windows.Forms.ComboBox terminalConfigurationComboBox;
        private System.Windows.Forms.Label outputPortLabel;
        private System.Windows.Forms.ComboBox outputPortComboBox;
        private System.Windows.Forms.Label clkOutputTerminalLabel;
        private System.Windows.Forms.Label actualHeadroomLabel;
        private System.Windows.Forms.Label highDataThroughputLabel;
        private System.Windows.Forms.ComboBox directionFindingModeComboBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.NumericUpDown OversamplingFactorNumeric;
        private System.Windows.Forms.Label OversamplingFactorLabel;
        private System.Windows.Forms.TextBox actualHeadroomTextBox;
        private System.Windows.Forms.Label dataRateLabel;
        private System.Windows.Forms.NumericUpDown dataRateNumeric;
        private System.Windows.Forms.Label zadoffChuIndexLabel;
        private System.Windows.Forms.NumericUpDown zadoffChuIndexNumeric;
        private System.Windows.Forms.Label physicalChannelAddressLabel;
        private System.Windows.Forms.NumericUpDown physicalChannelAddressNumeric;
        private System.Windows.Forms.Label HdtPhyIntervalLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label autoPayloadZoneProeprtiesSettingsLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button payloadLengthInsertButton;
        private System.Windows.Forms.Button payloadLengthDeleteButton;
        private System.Windows.Forms.DataGridView PayloadLengthBytesGrid;
        private System.Windows.Forms.Button TxLenSequenceNumberInsertButton;
        private System.Windows.Forms.Button TxLenSequenceNumberDeleteButton;
        private System.Windows.Forms.DataGridView TxLenSequenceNumberGrid;
        private System.Windows.Forms.Label payloadLengthLabel;
        private System.Windows.Forms.Label TxLenSequenceNumberLabel;
        private System.Windows.Forms.Label NumebrOfBlocksLabel;
        private System.Windows.Forms.Button NumberOfBlocksInsertButton;
        private System.Windows.Forms.Button NumberOfBlocksDeleteButton;
        private System.Windows.Forms.DataGridView NumberOfBlocksGrid;
        private System.Windows.Forms.Label BlockSizeLabel;
        private System.Windows.Forms.Button BlockSizeInsertButton;
        private System.Windows.Forms.Button BlockSizeDeleteButton;
        private System.Windows.Forms.DataGridView BlockSizeGrid;
        private System.Windows.Forms.Label LastBlockSizeLabel;
        private System.Windows.Forms.Button LastBlockSizeInsertButton;
        private System.Windows.Forms.Button LastBlockSizeDeleteButton;
        private System.Windows.Forms.DataGridView LastBlockSizeGrid;
        private System.Windows.Forms.Label TxBlockMapLabel;
        private System.Windows.Forms.Button TxBlockMapInsertButton;
        private System.Windows.Forms.Button TxBlockMapDeleteButton;
        private System.Windows.Forms.DataGridView TxBlockMapGrid;
        private System.Windows.Forms.Label ActualPayloadLengthLabel;
        private System.Windows.Forms.Button ActualPayloadLengthInsertButton;
        private System.Windows.Forms.Button ActualPayloadLengthDeleteButton;
        private System.Windows.Forms.DataGridView ActualPayloadLengthGrid;
        private System.Windows.Forms.Label PayloadZoneLengthLabel;
        private System.Windows.Forms.NumericUpDown PayloadZoneLengthValue;
        private System.Windows.Forms.NumericUpDown HdtPhyIntervalNumeric;
        private System.Windows.Forms.ComboBox Format1PayloadZoneConfigurationModeComboBox;
        private System.Windows.Forms.Label Format1PayloadZoneConfigurationModeLabel;
        private System.Windows.Forms.Label NumberOfPayloadsLabel;
        private System.Windows.Forms.NumericUpDown numberOfPayloadNumericUpDown;
        private System.Windows.Forms.ComboBox payloadLengthModeComboBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn payloadLengthIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn payloadLengthBytesValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn TxLenSequenceNumberIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn TxLenSequenceNumberValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumberOfBlocksIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumberOfBlocksValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn BlockSizeIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn BlockSizeValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastBlockSizeIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastBlockSizeValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn TxBlockMapIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn TxBlockMapValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActualPayloadLengthIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActualPayloadLengthValues;
    }
}

