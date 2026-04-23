namespace NationalInstruments.Examples.OspQuadratureDownconversion
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.channelNameLabel = new System.Windows.Forms.Label();
            this.timeOutLabel = new System.Windows.Forms.Label();
            this.triggerTypeLabel = new System.Windows.Forms.Label();
            this.inputImpedanceLabel = new System.Windows.Forms.Label();
            this.verticalRangeLabel = new System.Windows.Forms.Label();
            this.digitalGainLabel = new System.Windows.Forms.Label();
            this.centerFrequencyLabel = new System.Windows.Forms.Label();
            this.phaseILabel = new System.Windows.Forms.Label();
            this.phaseQLabel = new System.Windows.Forms.Label();
            this.triggerMinQuietTimeLabel = new System.Windows.Forms.Label();
            this.triggerLevelLabel = new System.Windows.Forms.Label();
            this.minSampleRateLabel = new System.Windows.Forms.Label();
            this.actualSampleRateLabel = new System.Windows.Forms.Label();
            this.minRecordLengthLabel = new System.Windows.Forms.Label();
            this.actualRecordLengthLabel = new System.Windows.Forms.Label();
            this.resourceNameLabel = new System.Windows.Forms.Label();
            this.timeoutNumeric = new System.Windows.Forms.NumericUpDown();
            this.verticalRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.digitalGainNumeric = new System.Windows.Forms.NumericUpDown();
            this.centerFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.phaseINumeric = new System.Windows.Forms.NumericUpDown();
            this.phaseQNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggerMinQuietTimeNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.sampleRateMinNumeric = new System.Windows.Forms.NumericUpDown();
            this.recordLengthMinNumeric = new System.Windows.Forms.NumericUpDown();
            this.channelNameTextBox = new System.Windows.Forms.TextBox();
            this.actualSampleRateTextBox = new System.Windows.Forms.TextBox();
            this.actualRecordLengthTextBox = new System.Windows.Forms.TextBox();
            this.acquireButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.triggerTypeComboBox = new System.Windows.Forms.ComboBox();
            this.basebandGroupBox = new System.Windows.Forms.GroupBox();
            this.inputImpedanceComboBox = new System.Windows.Forms.ComboBox();
            this.channelGroupBox = new System.Windows.Forms.GroupBox();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.OSPGroupBox = new System.Windows.Forms.GroupBox();
            this.horizontalConfigurationGroupBox = new System.Windows.Forms.GroupBox();
            this.fracResampleEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.acquisitionDataGroupBox = new System.Windows.Forms.GroupBox();
            this.acquisitionDataGridView = new System.Windows.Forms.DataGridView();
            this.messageGroupBox = new System.Windows.Forms.GroupBox();
            this.messageTextBox = new System.Windows.Forms.RichTextBox();
            this.buttonsGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.digitalGainNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.phaseINumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.phaseQNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerMinQuietTimeNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateMinNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.recordLengthMinNumeric)).BeginInit();
            this.basebandGroupBox.SuspendLayout();
            this.channelGroupBox.SuspendLayout();
            this.generalGroupBox.SuspendLayout();
            this.OSPGroupBox.SuspendLayout();
            this.horizontalConfigurationGroupBox.SuspendLayout();
            this.acquisitionDataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGridView)).BeginInit();
            this.messageGroupBox.SuspendLayout();
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
            // timeOutLabel
            // 
            this.timeOutLabel.AutoSize = true;
            this.timeOutLabel.Location = new System.Drawing.Point(6, 76);
            this.timeOutLabel.Name = "timeOutLabel";
            this.timeOutLabel.Size = new System.Drawing.Size(48, 13);
            this.timeOutLabel.TabIndex = 4;
            this.timeOutLabel.Text = "Timeout:";
            // 
            // triggerTypeLabel
            // 
            this.triggerTypeLabel.AutoSize = true;
            this.triggerTypeLabel.Location = new System.Drawing.Point(6, 23);
            this.triggerTypeLabel.Name = "triggerTypeLabel";
            this.triggerTypeLabel.Size = new System.Drawing.Size(70, 13);
            this.triggerTypeLabel.TabIndex = 0;
            this.triggerTypeLabel.Text = "Trigger Type:";
            // 
            // inputImpedanceLabel
            // 
            this.inputImpedanceLabel.AutoSize = true;
            this.inputImpedanceLabel.Location = new System.Drawing.Point(6, 23);
            this.inputImpedanceLabel.Name = "inputImpedanceLabel";
            this.inputImpedanceLabel.Size = new System.Drawing.Size(90, 13);
            this.inputImpedanceLabel.TabIndex = 0;
            this.inputImpedanceLabel.Text = "Input Impedance:";
            // 
            // verticalRangeLabel
            // 
            this.verticalRangeLabel.AutoSize = true;
            this.verticalRangeLabel.Location = new System.Drawing.Point(6, 50);
            this.verticalRangeLabel.Name = "verticalRangeLabel";
            this.verticalRangeLabel.Size = new System.Drawing.Size(80, 13);
            this.verticalRangeLabel.TabIndex = 2;
            this.verticalRangeLabel.Text = "Vertical Range:";
            // 
            // digitalGainLabel
            // 
            this.digitalGainLabel.AutoSize = true;
            this.digitalGainLabel.Location = new System.Drawing.Point(6, 76);
            this.digitalGainLabel.Name = "digitalGainLabel";
            this.digitalGainLabel.Size = new System.Drawing.Size(64, 13);
            this.digitalGainLabel.TabIndex = 4;
            this.digitalGainLabel.Text = "Digital Gain:";
            // 
            // centerFrequencyLabel
            // 
            this.centerFrequencyLabel.AutoSize = true;
            this.centerFrequencyLabel.Location = new System.Drawing.Point(6, 23);
            this.centerFrequencyLabel.Name = "centerFrequencyLabel";
            this.centerFrequencyLabel.Size = new System.Drawing.Size(94, 13);
            this.centerFrequencyLabel.TabIndex = 0;
            this.centerFrequencyLabel.Text = "Center Frequency:";
            // 
            // phaseILabel
            // 
            this.phaseILabel.AutoSize = true;
            this.phaseILabel.Location = new System.Drawing.Point(6, 49);
            this.phaseILabel.Name = "phaseILabel";
            this.phaseILabel.Size = new System.Drawing.Size(46, 13);
            this.phaseILabel.TabIndex = 2;
            this.phaseILabel.Text = "Phase I:";
            // 
            // phaseQLabel
            // 
            this.phaseQLabel.AutoSize = true;
            this.phaseQLabel.Location = new System.Drawing.Point(6, 75);
            this.phaseQLabel.Name = "phaseQLabel";
            this.phaseQLabel.Size = new System.Drawing.Size(51, 13);
            this.phaseQLabel.TabIndex = 4;
            this.phaseQLabel.Text = "Phase Q:";
            // 
            // triggerMinQuietTimeLabel
            // 
            this.triggerMinQuietTimeLabel.AutoSize = true;
            this.triggerMinQuietTimeLabel.Location = new System.Drawing.Point(6, 74);
            this.triggerMinQuietTimeLabel.Name = "triggerMinQuietTimeLabel";
            this.triggerMinQuietTimeLabel.Size = new System.Drawing.Size(117, 13);
            this.triggerMinQuietTimeLabel.TabIndex = 4;
            this.triggerMinQuietTimeLabel.Text = "Trigger Min Quiet Time:";
            // 
            // triggerLevelLabel
            // 
            this.triggerLevelLabel.AutoSize = true;
            this.triggerLevelLabel.Location = new System.Drawing.Point(6, 50);
            this.triggerLevelLabel.Name = "triggerLevelLabel";
            this.triggerLevelLabel.Size = new System.Drawing.Size(72, 13);
            this.triggerLevelLabel.TabIndex = 2;
            this.triggerLevelLabel.Text = "Trigger Level:";
            // 
            // minSampleRateLabel
            // 
            this.minSampleRateLabel.AutoSize = true;
            this.minSampleRateLabel.Location = new System.Drawing.Point(6, 23);
            this.minSampleRateLabel.Name = "minSampleRateLabel";
            this.minSampleRateLabel.Size = new System.Drawing.Size(94, 13);
            this.minSampleRateLabel.TabIndex = 0;
            this.minSampleRateLabel.Text = "Min. Sample Rate:";
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
            // minRecordLengthLabel
            // 
            this.minRecordLengthLabel.AutoSize = true;
            this.minRecordLengthLabel.Location = new System.Drawing.Point(6, 75);
            this.minRecordLengthLabel.Name = "minRecordLengthLabel";
            this.minRecordLengthLabel.Size = new System.Drawing.Size(104, 13);
            this.minRecordLengthLabel.TabIndex = 4;
            this.minRecordLengthLabel.Text = "Min. Record Length:";
            // 
            // actualRecordLengthLabel
            // 
            this.actualRecordLengthLabel.AutoSize = true;
            this.actualRecordLengthLabel.Location = new System.Drawing.Point(6, 101);
            this.actualRecordLengthLabel.Name = "actualRecordLengthLabel";
            this.actualRecordLengthLabel.Size = new System.Drawing.Size(114, 13);
            this.actualRecordLengthLabel.TabIndex = 6;
            this.actualRecordLengthLabel.Text = "Actual Record Length:";
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
            // timeoutNumeric
            // 
            this.timeoutNumeric.DecimalPlaces = 2;
            this.timeoutNumeric.Location = new System.Drawing.Point(133, 72);
            this.timeoutNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.timeoutNumeric.Name = "timeoutNumeric";
            this.timeoutNumeric.Size = new System.Drawing.Size(96, 20);
            this.timeoutNumeric.TabIndex = 5;
            this.timeoutNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // verticalRangeNumeric
            // 
            this.verticalRangeNumeric.DecimalPlaces = 2;
            this.verticalRangeNumeric.Location = new System.Drawing.Point(133, 46);
            this.verticalRangeNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.verticalRangeNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.verticalRangeNumeric.Name = "verticalRangeNumeric";
            this.verticalRangeNumeric.Size = new System.Drawing.Size(96, 20);
            this.verticalRangeNumeric.TabIndex = 3;
            this.verticalRangeNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // digitalGainNumeric
            // 
            this.digitalGainNumeric.DecimalPlaces = 2;
            this.digitalGainNumeric.Location = new System.Drawing.Point(133, 72);
            this.digitalGainNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.digitalGainNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.digitalGainNumeric.Name = "digitalGainNumeric";
            this.digitalGainNumeric.Size = new System.Drawing.Size(96, 20);
            this.digitalGainNumeric.TabIndex = 5;
            this.digitalGainNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // centerFrequencyNumeric
            // 
            this.centerFrequencyNumeric.DecimalPlaces = 2;
            this.centerFrequencyNumeric.Location = new System.Drawing.Point(133, 19);
            this.centerFrequencyNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.centerFrequencyNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.centerFrequencyNumeric.Name = "centerFrequencyNumeric";
            this.centerFrequencyNumeric.Size = new System.Drawing.Size(96, 20);
            this.centerFrequencyNumeric.TabIndex = 1;
            this.centerFrequencyNumeric.Value = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            // 
            // phaseINumeric
            // 
            this.phaseINumeric.DecimalPlaces = 2;
            this.phaseINumeric.Location = new System.Drawing.Point(133, 45);
            this.phaseINumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.phaseINumeric.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.phaseINumeric.Name = "phaseINumeric";
            this.phaseINumeric.Size = new System.Drawing.Size(96, 20);
            this.phaseINumeric.TabIndex = 3;
            // 
            // phaseQNumeric
            // 
            this.phaseQNumeric.DecimalPlaces = 2;
            this.phaseQNumeric.Location = new System.Drawing.Point(133, 71);
            this.phaseQNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.phaseQNumeric.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.phaseQNumeric.Name = "phaseQNumeric";
            this.phaseQNumeric.Size = new System.Drawing.Size(96, 20);
            this.phaseQNumeric.TabIndex = 5;
            this.phaseQNumeric.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // triggerMinQuietTimeNumeric
            // 
            this.triggerMinQuietTimeNumeric.DecimalPlaces = 8;
            this.triggerMinQuietTimeNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            524288});
            this.triggerMinQuietTimeNumeric.Location = new System.Drawing.Point(133, 72);
            this.triggerMinQuietTimeNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.triggerMinQuietTimeNumeric.Name = "triggerMinQuietTimeNumeric";
            this.triggerMinQuietTimeNumeric.Size = new System.Drawing.Size(96, 20);
            this.triggerMinQuietTimeNumeric.TabIndex = 5;
            // 
            // triggerLevelNumeric
            // 
            this.triggerLevelNumeric.DecimalPlaces = 2;
            this.triggerLevelNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.triggerLevelNumeric.Location = new System.Drawing.Point(133, 46);
            this.triggerLevelNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.triggerLevelNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.triggerLevelNumeric.Name = "triggerLevelNumeric";
            this.triggerLevelNumeric.Size = new System.Drawing.Size(96, 20);
            this.triggerLevelNumeric.TabIndex = 3;
            // 
            // sampleRateMinNumeric
            // 
            this.sampleRateMinNumeric.DecimalPlaces = 2;
            this.sampleRateMinNumeric.Location = new System.Drawing.Point(116, 19);
            this.sampleRateMinNumeric.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.sampleRateMinNumeric.Minimum = new decimal(new int[] {
            -1,
            -1,
            -1,
            -2147483648});
            this.sampleRateMinNumeric.Name = "sampleRateMinNumeric";
            this.sampleRateMinNumeric.Size = new System.Drawing.Size(106, 20);
            this.sampleRateMinNumeric.TabIndex = 1;
            this.sampleRateMinNumeric.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // recordLengthMinNumeric
            // 
            this.recordLengthMinNumeric.Location = new System.Drawing.Point(133, 71);
            this.recordLengthMinNumeric.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.recordLengthMinNumeric.Name = "recordLengthMinNumeric";
            this.recordLengthMinNumeric.Size = new System.Drawing.Size(97, 20);
            this.recordLengthMinNumeric.TabIndex = 5;
            this.recordLengthMinNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // channelNameTextBox
            // 
            this.channelNameTextBox.Location = new System.Drawing.Point(133, 46);
            this.channelNameTextBox.Name = "channelNameTextBox";
            this.channelNameTextBox.Size = new System.Drawing.Size(96, 20);
            this.channelNameTextBox.TabIndex = 3;
            this.channelNameTextBox.Text = "0";
            // 
            // actualSampleRateTextBox
            // 
            this.actualSampleRateTextBox.Location = new System.Drawing.Point(116, 45);
            this.actualSampleRateTextBox.Name = "actualSampleRateTextBox";
            this.actualSampleRateTextBox.ReadOnly = true;
            this.actualSampleRateTextBox.Size = new System.Drawing.Size(114, 20);
            this.actualSampleRateTextBox.TabIndex = 3;
            // 
            // actualRecordLengthTextBox
            // 
            this.actualRecordLengthTextBox.Location = new System.Drawing.Point(133, 97);
            this.actualRecordLengthTextBox.Name = "actualRecordLengthTextBox";
            this.actualRecordLengthTextBox.ReadOnly = true;
            this.actualRecordLengthTextBox.Size = new System.Drawing.Size(97, 20);
            this.actualRecordLengthTextBox.TabIndex = 7;
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(85, 19);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(166, 19);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "&Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // triggerTypeComboBox
            // 
            this.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerTypeComboBox.Location = new System.Drawing.Point(133, 19);
            this.triggerTypeComboBox.Name = "triggerTypeComboBox";
            this.triggerTypeComboBox.Size = new System.Drawing.Size(96, 21);
            this.triggerTypeComboBox.TabIndex = 1;
            // 
            // basebandGroupBox
            // 
            this.basebandGroupBox.Controls.Add(this.triggerTypeComboBox);
            this.basebandGroupBox.Controls.Add(this.triggerMinQuietTimeNumeric);
            this.basebandGroupBox.Controls.Add(this.triggerLevelNumeric);
            this.basebandGroupBox.Controls.Add(this.triggerTypeLabel);
            this.basebandGroupBox.Controls.Add(this.triggerMinQuietTimeLabel);
            this.basebandGroupBox.Controls.Add(this.triggerLevelLabel);
            this.basebandGroupBox.Location = new System.Drawing.Point(12, 513);
            this.basebandGroupBox.Name = "basebandGroupBox";
            this.basebandGroupBox.Size = new System.Drawing.Size(238, 101);
            this.basebandGroupBox.TabIndex = 4;
            this.basebandGroupBox.TabStop = false;
            this.basebandGroupBox.Text = "Baseband Trigger";
            // 
            // inputImpedanceComboBox
            // 
            this.inputImpedanceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputImpedanceComboBox.Location = new System.Drawing.Point(133, 19);
            this.inputImpedanceComboBox.Name = "inputImpedanceComboBox";
            this.inputImpedanceComboBox.Size = new System.Drawing.Size(96, 21);
            this.inputImpedanceComboBox.TabIndex = 1;
            // 
            // channelGroupBox
            // 
            this.channelGroupBox.Controls.Add(this.inputImpedanceComboBox);
            this.channelGroupBox.Controls.Add(this.verticalRangeNumeric);
            this.channelGroupBox.Controls.Add(this.digitalGainNumeric);
            this.channelGroupBox.Controls.Add(this.inputImpedanceLabel);
            this.channelGroupBox.Controls.Add(this.verticalRangeLabel);
            this.channelGroupBox.Controls.Add(this.digitalGainLabel);
            this.channelGroupBox.Location = new System.Drawing.Point(12, 124);
            this.channelGroupBox.Name = "channelGroupBox";
            this.channelGroupBox.Size = new System.Drawing.Size(238, 110);
            this.channelGroupBox.TabIndex = 1;
            this.channelGroupBox.TabStop = false;
            this.channelGroupBox.Text = "Channel Configuration";
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.resourceNameComboBox);
            this.generalGroupBox.Controls.Add(this.channelNameTextBox);
            this.generalGroupBox.Controls.Add(this.timeoutNumeric);
            this.generalGroupBox.Controls.Add(this.channelNameLabel);
            this.generalGroupBox.Controls.Add(this.timeOutLabel);
            this.generalGroupBox.Controls.Add(this.resourceNameLabel);
            this.generalGroupBox.Location = new System.Drawing.Point(12, 12);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.generalGroupBox.Size = new System.Drawing.Size(238, 101);
            this.generalGroupBox.TabIndex = 0;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General Configuration";
            // 
            // resourceNameComboBox
            // 
            this.resourceNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameComboBox.FormattingEnabled = true;
            this.resourceNameComboBox.Location = new System.Drawing.Point(133, 19);
            this.resourceNameComboBox.Name = "resourceNameComboBox";
            this.resourceNameComboBox.Size = new System.Drawing.Size(96, 21);
            this.resourceNameComboBox.TabIndex = 1;
            // 
            // OSPGroupBox
            // 
            this.OSPGroupBox.Controls.Add(this.centerFrequencyNumeric);
            this.OSPGroupBox.Controls.Add(this.phaseINumeric);
            this.OSPGroupBox.Controls.Add(this.phaseQNumeric);
            this.OSPGroupBox.Controls.Add(this.centerFrequencyLabel);
            this.OSPGroupBox.Controls.Add(this.phaseILabel);
            this.OSPGroupBox.Controls.Add(this.phaseQLabel);
            this.OSPGroupBox.Location = new System.Drawing.Point(12, 245);
            this.OSPGroupBox.Name = "OSPGroupBox";
            this.OSPGroupBox.Size = new System.Drawing.Size(238, 102);
            this.OSPGroupBox.TabIndex = 2;
            this.OSPGroupBox.TabStop = false;
            this.OSPGroupBox.Text = "OSP Configuration";
            // 
            // horizontalConfigurationGroupBox
            // 
            this.horizontalConfigurationGroupBox.Controls.Add(this.fracResampleEnabledCheckBox);
            this.horizontalConfigurationGroupBox.Controls.Add(this.actualSampleRateTextBox);
            this.horizontalConfigurationGroupBox.Controls.Add(this.actualRecordLengthTextBox);
            this.horizontalConfigurationGroupBox.Controls.Add(this.sampleRateMinNumeric);
            this.horizontalConfigurationGroupBox.Controls.Add(this.recordLengthMinNumeric);
            this.horizontalConfigurationGroupBox.Controls.Add(this.minSampleRateLabel);
            this.horizontalConfigurationGroupBox.Controls.Add(this.actualSampleRateLabel);
            this.horizontalConfigurationGroupBox.Controls.Add(this.minRecordLengthLabel);
            this.horizontalConfigurationGroupBox.Controls.Add(this.actualRecordLengthLabel);
            this.horizontalConfigurationGroupBox.Location = new System.Drawing.Point(12, 358);
            this.horizontalConfigurationGroupBox.Name = "horizontalConfigurationGroupBox";
            this.horizontalConfigurationGroupBox.Size = new System.Drawing.Size(238, 144);
            this.horizontalConfigurationGroupBox.TabIndex = 3;
            this.horizontalConfigurationGroupBox.TabStop = false;
            this.horizontalConfigurationGroupBox.Text = "Horizontal Configuration";
            // 
            // fracResampleEnabledCheckBox
            // 
            this.fracResampleEnabledCheckBox.AutoSize = true;
            this.fracResampleEnabledCheckBox.Location = new System.Drawing.Point(6, 123);
            this.fracResampleEnabledCheckBox.Name = "fracResampleEnabledCheckBox";
            this.fracResampleEnabledCheckBox.Size = new System.Drawing.Size(173, 17);
            this.fracResampleEnabledCheckBox.TabIndex = 8;
            this.fracResampleEnabledCheckBox.Text = "Fractional Resample Enabled ?";
            this.fracResampleEnabledCheckBox.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.fracResampleEnabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // acquisitionDataGroupBox
            // 
            this.acquisitionDataGroupBox.Controls.Add(this.acquisitionDataGridView);
            this.acquisitionDataGroupBox.Location = new System.Drawing.Point(256, 12);
            this.acquisitionDataGroupBox.Name = "acquisitionDataGroupBox";
            this.acquisitionDataGroupBox.Size = new System.Drawing.Size(327, 540);
            this.acquisitionDataGroupBox.TabIndex = 6;
            this.acquisitionDataGroupBox.TabStop = false;
            this.acquisitionDataGroupBox.Text = "Acquisition Data";
            // 
            // acquisitionDataGridView
            // 
            this.acquisitionDataGridView.AllowUserToAddRows = false;
            this.acquisitionDataGridView.AllowUserToDeleteRows = false;
            this.acquisitionDataGridView.AllowUserToResizeRows = false;
            this.acquisitionDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.acquisitionDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.acquisitionDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.acquisitionDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.acquisitionDataGridView.Location = new System.Drawing.Point(6, 19);
            this.acquisitionDataGridView.Name = "acquisitionDataGridView";
            this.acquisitionDataGridView.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.acquisitionDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.acquisitionDataGridView.RowHeadersVisible = false;
            this.acquisitionDataGridView.RowHeadersWidth = 15;
            this.acquisitionDataGridView.RowTemplate.Height = 24;
            this.acquisitionDataGridView.Size = new System.Drawing.Size(315, 515);
            this.acquisitionDataGridView.StandardTab = true;
            this.acquisitionDataGridView.TabIndex = 0;
            // 
            // messageGroupBox
            // 
            this.messageGroupBox.Controls.Add(this.messageTextBox);
            this.messageGroupBox.Location = new System.Drawing.Point(12, 620);
            this.messageGroupBox.Name = "messageGroupBox";
            this.messageGroupBox.Size = new System.Drawing.Size(571, 48);
            this.messageGroupBox.TabIndex = 5;
            this.messageGroupBox.TabStop = false;
            this.messageGroupBox.Text = "Message";
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(9, 19);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.Size = new System.Drawing.Size(556, 22);
            this.messageTextBox.TabIndex = 0;
            this.messageTextBox.Text = "";
            // 
            // buttonsGroupBox
            // 
            this.buttonsGroupBox.Controls.Add(this.acquireButton);
            this.buttonsGroupBox.Controls.Add(this.stopButton);
            this.buttonsGroupBox.Location = new System.Drawing.Point(256, 558);
            this.buttonsGroupBox.Name = "buttonsGroupBox";
            this.buttonsGroupBox.Size = new System.Drawing.Size(327, 56);
            this.buttonsGroupBox.TabIndex = 7;
            this.buttonsGroupBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 680);
            this.Controls.Add(this.buttonsGroupBox);
            this.Controls.Add(this.messageGroupBox);
            this.Controls.Add(this.acquisitionDataGroupBox);
            this.Controls.Add(this.generalGroupBox);
            this.Controls.Add(this.OSPGroupBox);
            this.Controls.Add(this.channelGroupBox);
            this.Controls.Add(this.horizontalConfigurationGroupBox);
            this.Controls.Add(this.basebandGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "OSP Quadrature Downconversion";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.digitalGainNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.phaseINumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.phaseQNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerMinQuietTimeNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateMinNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.recordLengthMinNumeric)).EndInit();
            this.basebandGroupBox.ResumeLayout(false);
            this.basebandGroupBox.PerformLayout();
            this.channelGroupBox.ResumeLayout(false);
            this.channelGroupBox.PerformLayout();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.OSPGroupBox.ResumeLayout(false);
            this.OSPGroupBox.PerformLayout();
            this.horizontalConfigurationGroupBox.ResumeLayout(false);
            this.horizontalConfigurationGroupBox.PerformLayout();
            this.acquisitionDataGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGridView)).EndInit();
            this.messageGroupBox.ResumeLayout(false);
            this.buttonsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label channelNameLabel;
        private System.Windows.Forms.Label timeOutLabel;
        private System.Windows.Forms.Label triggerTypeLabel;
        private System.Windows.Forms.Label inputImpedanceLabel;
        private System.Windows.Forms.Label verticalRangeLabel;
        private System.Windows.Forms.Label digitalGainLabel;
        private System.Windows.Forms.Label centerFrequencyLabel;
        private System.Windows.Forms.Label phaseILabel;
        private System.Windows.Forms.Label phaseQLabel;
        private System.Windows.Forms.Label triggerMinQuietTimeLabel;
        private System.Windows.Forms.Label triggerLevelLabel;
        private System.Windows.Forms.Label minSampleRateLabel;
        private System.Windows.Forms.Label actualSampleRateLabel;
        private System.Windows.Forms.Label minRecordLengthLabel;
        private System.Windows.Forms.Label actualRecordLengthLabel;
        private System.Windows.Forms.Label resourceNameLabel;
        private System.Windows.Forms.NumericUpDown timeoutNumeric;
        private System.Windows.Forms.NumericUpDown verticalRangeNumeric;
        private System.Windows.Forms.NumericUpDown digitalGainNumeric;
        private System.Windows.Forms.NumericUpDown centerFrequencyNumeric;
        private System.Windows.Forms.NumericUpDown phaseINumeric;
        private System.Windows.Forms.NumericUpDown phaseQNumeric;
        private System.Windows.Forms.NumericUpDown triggerMinQuietTimeNumeric;
        private System.Windows.Forms.NumericUpDown triggerLevelNumeric;
        private System.Windows.Forms.NumericUpDown sampleRateMinNumeric;
        private System.Windows.Forms.NumericUpDown recordLengthMinNumeric;
        private System.Windows.Forms.TextBox channelNameTextBox;
        private System.Windows.Forms.TextBox actualSampleRateTextBox;
        private System.Windows.Forms.TextBox actualRecordLengthTextBox;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ComboBox triggerTypeComboBox;
        private System.Windows.Forms.ComboBox inputImpedanceComboBox;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.GroupBox OSPGroupBox;
        private System.Windows.Forms.GroupBox channelGroupBox;
        private System.Windows.Forms.GroupBox horizontalConfigurationGroupBox;
        private System.Windows.Forms.GroupBox basebandGroupBox;
        private System.Windows.Forms.ComboBox resourceNameComboBox;
        private System.Windows.Forms.GroupBox acquisitionDataGroupBox;
        private System.Windows.Forms.DataGridView acquisitionDataGridView;
        private System.Windows.Forms.CheckBox fracResampleEnabledCheckBox;
        private System.Windows.Forms.GroupBox messageGroupBox;
        private System.Windows.Forms.RichTextBox messageTextBox;
        private System.Windows.Forms.GroupBox buttonsGroupBox;


    }
}
