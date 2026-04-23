namespace NationalInstruments.Examples.ScopeSynchronizationUsingTClock
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.resourceNameDevice1Label = new System.Windows.Forms.Label();
            this.channelNameDevice1Label = new System.Windows.Forms.Label();
            this.channelNameDevice1TextBox = new System.Windows.Forms.TextBox();
            this.acquireButton = new System.Windows.Forms.Button();
            this.scopeDevice1GroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameDevice1ComboBox = new System.Windows.Forms.ComboBox();
            this.scopeDevice2GroupBox = new System.Windows.Forms.GroupBox();
            this.resourceNameDevice2ComboBox = new System.Windows.Forms.ComboBox();
            this.channelNameDevice2Label = new System.Windows.Forms.Label();
            this.resourceNameDevice2Label = new System.Windows.Forms.Label();
            this.channelNameDevice2TextBox = new System.Windows.Forms.TextBox();
            this.commonConfigurationGroupBox = new System.Windows.Forms.GroupBox();
            this.maximumInputFrequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.recordLengthMinNumeric = new System.Windows.Forms.NumericUpDown();
            this.verticalRangeNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumInputFrequencyLabel = new System.Windows.Forms.Label();
            this.recordLengthMinLabel = new System.Windows.Forms.Label();
            this.sampleRateMinLabel = new System.Windows.Forms.Label();
            this.verticalRangeLabel = new System.Windows.Forms.Label();
            this.triggeringGroupBox = new System.Windows.Forms.GroupBox();
            this.triggerTypeComboBox = new System.Windows.Forms.ComboBox();
            this.triggerTypeLabel = new System.Windows.Forms.Label();
            this.triggerLevelLabel = new System.Windows.Forms.Label();
            this.triggerSourceLabel = new System.Windows.Forms.Label();
            this.triggerLevelNumeric = new System.Windows.Forms.NumericUpDown();
            this.triggerSourceComboBox = new System.Windows.Forms.ComboBox();
            this.scope1GroupBox = new System.Windows.Forms.GroupBox();
            this.scope1DataGridView = new System.Windows.Forms.DataGridView();
            this.scope2GroupBox = new System.Windows.Forms.GroupBox();
            this.scope2DataGridView = new System.Windows.Forms.DataGridView();
            this.buttonsGroupBox = new System.Windows.Forms.GroupBox();
            this.sampleRateMinNumeric = new System.Windows.Forms.NumericUpDown();
            this.scopeDevice1GroupBox.SuspendLayout();
            this.scopeDevice2GroupBox.SuspendLayout();
            this.commonConfigurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maximumInputFrequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.recordLengthMinNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).BeginInit();
            this.triggeringGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).BeginInit();
            this.scope1GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scope1DataGridView)).BeginInit();
            this.scope2GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scope2DataGridView)).BeginInit();
            this.buttonsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateMinNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // resourceNameDevice1Label
            // 
            this.resourceNameDevice1Label.AutoSize = true;
            this.resourceNameDevice1Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resourceNameDevice1Label.Location = new System.Drawing.Point(6, 23);
            this.resourceNameDevice1Label.Name = "resourceNameDevice1Label";
            this.resourceNameDevice1Label.Size = new System.Drawing.Size(87, 13);
            this.resourceNameDevice1Label.TabIndex = 0;
            this.resourceNameDevice1Label.Text = "Resource Name:";
            // 
            // channelNameDevice1Label
            // 
            this.channelNameDevice1Label.AutoSize = true;
            this.channelNameDevice1Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelNameDevice1Label.Location = new System.Drawing.Point(6, 50);
            this.channelNameDevice1Label.Name = "channelNameDevice1Label";
            this.channelNameDevice1Label.Size = new System.Drawing.Size(80, 13);
            this.channelNameDevice1Label.TabIndex = 2;
            this.channelNameDevice1Label.Text = "Channel Name:";
            // 
            // channelNameDevice1TextBox
            // 
            this.channelNameDevice1TextBox.Location = new System.Drawing.Point(129, 46);
            this.channelNameDevice1TextBox.Name = "channelNameDevice1TextBox";
            this.channelNameDevice1TextBox.Size = new System.Drawing.Size(107, 20);
            this.channelNameDevice1TextBox.TabIndex = 3;
            this.channelNameDevice1TextBox.Text = "0";
            // 
            // acquireButton
            // 
            this.acquireButton.Location = new System.Drawing.Point(87, 19);
            this.acquireButton.Name = "acquireButton";
            this.acquireButton.Size = new System.Drawing.Size(75, 23);
            this.acquireButton.TabIndex = 0;
            this.acquireButton.Text = "&Acquire";
            this.acquireButton.UseVisualStyleBackColor = true;
            this.acquireButton.Click += new System.EventHandler(this.acquireButton_Click);
            // 
            // scopeDevice1GroupBox
            // 
            this.scopeDevice1GroupBox.Controls.Add(this.resourceNameDevice1ComboBox);
            this.scopeDevice1GroupBox.Controls.Add(this.channelNameDevice1Label);
            this.scopeDevice1GroupBox.Controls.Add(this.resourceNameDevice1Label);
            this.scopeDevice1GroupBox.Controls.Add(this.channelNameDevice1TextBox);
            this.scopeDevice1GroupBox.Location = new System.Drawing.Point(12, 12);
            this.scopeDevice1GroupBox.Name = "scopeDevice1GroupBox";
            this.scopeDevice1GroupBox.Size = new System.Drawing.Size(242, 75);
            this.scopeDevice1GroupBox.TabIndex = 0;
            this.scopeDevice1GroupBox.TabStop = false;
            this.scopeDevice1GroupBox.Text = "Scope 1";
            // 
            // resourceNameDevice1ComboBox
            // 
            this.resourceNameDevice1ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameDevice1ComboBox.FormattingEnabled = true;
            this.resourceNameDevice1ComboBox.Location = new System.Drawing.Point(129, 19);
            this.resourceNameDevice1ComboBox.Name = "resourceNameDevice1ComboBox";
            this.resourceNameDevice1ComboBox.Size = new System.Drawing.Size(107, 21);
            this.resourceNameDevice1ComboBox.TabIndex = 1;
            // 
            // scopeDevice2GroupBox
            // 
            this.scopeDevice2GroupBox.Controls.Add(this.resourceNameDevice2ComboBox);
            this.scopeDevice2GroupBox.Controls.Add(this.channelNameDevice2Label);
            this.scopeDevice2GroupBox.Controls.Add(this.resourceNameDevice2Label);
            this.scopeDevice2GroupBox.Controls.Add(this.channelNameDevice2TextBox);
            this.scopeDevice2GroupBox.Location = new System.Drawing.Point(12, 98);
            this.scopeDevice2GroupBox.Name = "scopeDevice2GroupBox";
            this.scopeDevice2GroupBox.Size = new System.Drawing.Size(243, 73);
            this.scopeDevice2GroupBox.TabIndex = 1;
            this.scopeDevice2GroupBox.TabStop = false;
            this.scopeDevice2GroupBox.Text = "Scope 2";
            // 
            // resourceNameDevice2ComboBox
            // 
            this.resourceNameDevice2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resourceNameDevice2ComboBox.FormattingEnabled = true;
            this.resourceNameDevice2ComboBox.Location = new System.Drawing.Point(129, 19);
            this.resourceNameDevice2ComboBox.Name = "resourceNameDevice2ComboBox";
            this.resourceNameDevice2ComboBox.Size = new System.Drawing.Size(107, 21);
            this.resourceNameDevice2ComboBox.TabIndex = 1;
            // 
            // channelNameDevice2Label
            // 
            this.channelNameDevice2Label.AutoSize = true;
            this.channelNameDevice2Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelNameDevice2Label.Location = new System.Drawing.Point(6, 50);
            this.channelNameDevice2Label.Name = "channelNameDevice2Label";
            this.channelNameDevice2Label.Size = new System.Drawing.Size(80, 13);
            this.channelNameDevice2Label.TabIndex = 2;
            this.channelNameDevice2Label.Text = "Channel Name:";
            // 
            // resourceNameDevice2Label
            // 
            this.resourceNameDevice2Label.AutoSize = true;
            this.resourceNameDevice2Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resourceNameDevice2Label.Location = new System.Drawing.Point(6, 23);
            this.resourceNameDevice2Label.Name = "resourceNameDevice2Label";
            this.resourceNameDevice2Label.Size = new System.Drawing.Size(87, 13);
            this.resourceNameDevice2Label.TabIndex = 0;
            this.resourceNameDevice2Label.Text = "Resource Name:";
            // 
            // channelNameDevice2TextBox
            // 
            this.channelNameDevice2TextBox.Location = new System.Drawing.Point(129, 46);
            this.channelNameDevice2TextBox.Name = "channelNameDevice2TextBox";
            this.channelNameDevice2TextBox.Size = new System.Drawing.Size(107, 20);
            this.channelNameDevice2TextBox.TabIndex = 3;
            this.channelNameDevice2TextBox.Text = "0";
            // 
            // commonConfigurationGroupBox
            // 
            this.commonConfigurationGroupBox.Controls.Add(this.sampleRateMinNumeric);
            this.commonConfigurationGroupBox.Controls.Add(this.maximumInputFrequencyNumeric);
            this.commonConfigurationGroupBox.Controls.Add(this.recordLengthMinNumeric);
            this.commonConfigurationGroupBox.Controls.Add(this.verticalRangeNumeric);
            this.commonConfigurationGroupBox.Controls.Add(this.maximumInputFrequencyLabel);
            this.commonConfigurationGroupBox.Controls.Add(this.recordLengthMinLabel);
            this.commonConfigurationGroupBox.Controls.Add(this.sampleRateMinLabel);
            this.commonConfigurationGroupBox.Controls.Add(this.verticalRangeLabel);
            this.commonConfigurationGroupBox.Location = new System.Drawing.Point(12, 182);
            this.commonConfigurationGroupBox.Name = "commonConfigurationGroupBox";
            this.commonConfigurationGroupBox.Size = new System.Drawing.Size(243, 124);
            this.commonConfigurationGroupBox.TabIndex = 2;
            this.commonConfigurationGroupBox.TabStop = false;
            this.commonConfigurationGroupBox.Text = "Common Configuration";
            // 
            // maximumInputFrequencyNumeric
            // 
            this.maximumInputFrequencyNumeric.Location = new System.Drawing.Point(129, 97);
            this.maximumInputFrequencyNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.maximumInputFrequencyNumeric.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.maximumInputFrequencyNumeric.Name = "maximumInputFrequencyNumeric";
            this.maximumInputFrequencyNumeric.Size = new System.Drawing.Size(107, 20);
            this.maximumInputFrequencyNumeric.TabIndex = 7;
            this.maximumInputFrequencyNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // recordLengthMinNumeric
            // 
            this.recordLengthMinNumeric.Location = new System.Drawing.Point(130, 71);
            this.recordLengthMinNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.recordLengthMinNumeric.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.recordLengthMinNumeric.Name = "recordLengthMinNumeric";
            this.recordLengthMinNumeric.Size = new System.Drawing.Size(107, 20);
            this.recordLengthMinNumeric.TabIndex = 5;
            this.recordLengthMinNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // verticalRangeNumeric
            // 
            this.verticalRangeNumeric.Location = new System.Drawing.Point(130, 19);
            this.verticalRangeNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.verticalRangeNumeric.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.verticalRangeNumeric.Name = "verticalRangeNumeric";
            this.verticalRangeNumeric.Size = new System.Drawing.Size(107, 20);
            this.verticalRangeNumeric.TabIndex = 1;
            this.verticalRangeNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // maximumInputFrequencyLabel
            // 
            this.maximumInputFrequencyLabel.AutoSize = true;
            this.maximumInputFrequencyLabel.Location = new System.Drawing.Point(6, 101);
            this.maximumInputFrequencyLabel.Name = "maximumInputFrequencyLabel";
            this.maximumInputFrequencyLabel.Size = new System.Drawing.Size(113, 13);
            this.maximumInputFrequencyLabel.TabIndex = 6;
            this.maximumInputFrequencyLabel.Text = "Max. Input Frequency:";
            // 
            // recordLengthMinLabel
            // 
            this.recordLengthMinLabel.AutoSize = true;
            this.recordLengthMinLabel.Location = new System.Drawing.Point(6, 75);
            this.recordLengthMinLabel.Name = "recordLengthMinLabel";
            this.recordLengthMinLabel.Size = new System.Drawing.Size(104, 13);
            this.recordLengthMinLabel.TabIndex = 4;
            this.recordLengthMinLabel.Text = "Min. Record Length:";
            // 
            // sampleRateMinLabel
            // 
            this.sampleRateMinLabel.AutoSize = true;
            this.sampleRateMinLabel.Location = new System.Drawing.Point(6, 49);
            this.sampleRateMinLabel.Name = "sampleRateMinLabel";
            this.sampleRateMinLabel.Size = new System.Drawing.Size(94, 13);
            this.sampleRateMinLabel.TabIndex = 2;
            this.sampleRateMinLabel.Text = "Min. Sample Rate:";
            // 
            // verticalRangeLabel
            // 
            this.verticalRangeLabel.AutoSize = true;
            this.verticalRangeLabel.Location = new System.Drawing.Point(6, 23);
            this.verticalRangeLabel.Name = "verticalRangeLabel";
            this.verticalRangeLabel.Size = new System.Drawing.Size(80, 13);
            this.verticalRangeLabel.TabIndex = 0;
            this.verticalRangeLabel.Text = "Vertical Range:";
            // 
            // triggeringGroupBox
            // 
            this.triggeringGroupBox.Controls.Add(this.triggerTypeComboBox);
            this.triggeringGroupBox.Controls.Add(this.triggerTypeLabel);
            this.triggeringGroupBox.Controls.Add(this.triggerLevelLabel);
            this.triggeringGroupBox.Controls.Add(this.triggerSourceLabel);
            this.triggeringGroupBox.Controls.Add(this.triggerLevelNumeric);
            this.triggeringGroupBox.Controls.Add(this.triggerSourceComboBox);
            this.triggeringGroupBox.Location = new System.Drawing.Point(12, 317);
            this.triggeringGroupBox.Name = "triggeringGroupBox";
            this.triggeringGroupBox.Size = new System.Drawing.Size(242, 100);
            this.triggeringGroupBox.TabIndex = 3;
            this.triggeringGroupBox.TabStop = false;
            this.triggeringGroupBox.Text = "Triggering ( on Scope 1 )";
            // 
            // triggerTypeComboBox
            // 
            this.triggerTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerTypeComboBox.FormattingEnabled = true;
            this.triggerTypeComboBox.Location = new System.Drawing.Point(130, 19);
            this.triggerTypeComboBox.Name = "triggerTypeComboBox";
            this.triggerTypeComboBox.Size = new System.Drawing.Size(107, 21);
            this.triggerTypeComboBox.TabIndex = 1;
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
            // triggerLevelLabel
            // 
            this.triggerLevelLabel.AutoSize = true;
            this.triggerLevelLabel.Location = new System.Drawing.Point(6, 77);
            this.triggerLevelLabel.Name = "triggerLevelLabel";
            this.triggerLevelLabel.Size = new System.Drawing.Size(72, 13);
            this.triggerLevelLabel.TabIndex = 4;
            this.triggerLevelLabel.Text = "Trigger Level:";
            // 
            // triggerSourceLabel
            // 
            this.triggerSourceLabel.AutoSize = true;
            this.triggerSourceLabel.Location = new System.Drawing.Point(6, 50);
            this.triggerSourceLabel.Name = "triggerSourceLabel";
            this.triggerSourceLabel.Size = new System.Drawing.Size(80, 13);
            this.triggerSourceLabel.TabIndex = 2;
            this.triggerSourceLabel.Text = "Trigger Source:";
            // 
            // triggerLevelNumeric
            // 
            this.triggerLevelNumeric.Location = new System.Drawing.Point(130, 73);
            this.triggerLevelNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.triggerLevelNumeric.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.triggerLevelNumeric.Name = "triggerLevelNumeric";
            this.triggerLevelNumeric.Size = new System.Drawing.Size(107, 20);
            this.triggerLevelNumeric.TabIndex = 5;
            // 
            // triggerSourceComboBox
            // 
            this.triggerSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerSourceComboBox.FormattingEnabled = true;
            this.triggerSourceComboBox.Location = new System.Drawing.Point(130, 46);
            this.triggerSourceComboBox.Name = "triggerSourceComboBox";
            this.triggerSourceComboBox.Size = new System.Drawing.Size(107, 21);
            this.triggerSourceComboBox.TabIndex = 3;
            // 
            // scope1GroupBox
            // 
            this.scope1GroupBox.Controls.Add(this.scope1DataGridView);
            this.scope1GroupBox.Location = new System.Drawing.Point(261, 12);
            this.scope1GroupBox.Name = "scope1GroupBox";
            this.scope1GroupBox.Size = new System.Drawing.Size(202, 473);
            this.scope1GroupBox.TabIndex = 5;
            this.scope1GroupBox.TabStop = false;
            this.scope1GroupBox.Text = "Scope1";
            // 
            // scope1DataGridView
            // 
            this.scope1DataGridView.AllowUserToAddRows = false;
            this.scope1DataGridView.AllowUserToDeleteRows = false;
            this.scope1DataGridView.AllowUserToResizeRows = false;
            this.scope1DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.scope1DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.scope1DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.scope1DataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.scope1DataGridView.Location = new System.Drawing.Point(6, 19);
            this.scope1DataGridView.Name = "scope1DataGridView";
            this.scope1DataGridView.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.scope1DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.scope1DataGridView.RowHeadersVisible = false;
            this.scope1DataGridView.RowHeadersWidth = 15;
            this.scope1DataGridView.RowTemplate.Height = 24;
            this.scope1DataGridView.Size = new System.Drawing.Size(190, 448);
            this.scope1DataGridView.StandardTab = true;
            this.scope1DataGridView.TabIndex = 0;
            // 
            // scope2GroupBox
            // 
            this.scope2GroupBox.Controls.Add(this.scope2DataGridView);
            this.scope2GroupBox.Location = new System.Drawing.Point(469, 12);
            this.scope2GroupBox.Name = "scope2GroupBox";
            this.scope2GroupBox.Size = new System.Drawing.Size(202, 473);
            this.scope2GroupBox.TabIndex = 6;
            this.scope2GroupBox.TabStop = false;
            this.scope2GroupBox.Text = "Scope2";
            // 
            // scope2DataGridView
            // 
            this.scope2DataGridView.AllowUserToAddRows = false;
            this.scope2DataGridView.AllowUserToDeleteRows = false;
            this.scope2DataGridView.AllowUserToResizeRows = false;
            this.scope2DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.scope2DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.scope2DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.scope2DataGridView.DefaultCellStyle = dataGridViewCellStyle5;
            this.scope2DataGridView.Location = new System.Drawing.Point(6, 19);
            this.scope2DataGridView.Name = "scope2DataGridView";
            this.scope2DataGridView.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.scope2DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.scope2DataGridView.RowHeadersVisible = false;
            this.scope2DataGridView.RowHeadersWidth = 15;
            this.scope2DataGridView.RowTemplate.Height = 24;
            this.scope2DataGridView.Size = new System.Drawing.Size(190, 448);
            this.scope2DataGridView.StandardTab = true;
            this.scope2DataGridView.TabIndex = 0;
            // 
            // buttonsGroupBox
            // 
            this.buttonsGroupBox.Controls.Add(this.acquireButton);
            this.buttonsGroupBox.Location = new System.Drawing.Point(12, 428);
            this.buttonsGroupBox.Name = "buttonsGroupBox";
            this.buttonsGroupBox.Size = new System.Drawing.Size(242, 57);
            this.buttonsGroupBox.TabIndex = 4;
            this.buttonsGroupBox.TabStop = false;
            // 
            // sampleRateMinNumeric
            // 
            this.sampleRateMinNumeric.DecimalPlaces = 2;
            this.sampleRateMinNumeric.Location = new System.Drawing.Point(129, 45);
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
            this.sampleRateMinNumeric.Size = new System.Drawing.Size(108, 20);
            this.sampleRateMinNumeric.TabIndex = 3;
            this.sampleRateMinNumeric.Value = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 498);
            this.Controls.Add(this.buttonsGroupBox);
            this.Controls.Add(this.scope2GroupBox);
            this.Controls.Add(this.scope1GroupBox);
            this.Controls.Add(this.triggeringGroupBox);
            this.Controls.Add(this.commonConfigurationGroupBox);
            this.Controls.Add(this.scopeDevice2GroupBox);
            this.Controls.Add(this.scopeDevice1GroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Scope Synchronization Using TClock";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_Closing);
            this.scopeDevice1GroupBox.ResumeLayout(false);
            this.scopeDevice1GroupBox.PerformLayout();
            this.scopeDevice2GroupBox.ResumeLayout(false);
            this.scopeDevice2GroupBox.PerformLayout();
            this.commonConfigurationGroupBox.ResumeLayout(false);
            this.commonConfigurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maximumInputFrequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.recordLengthMinNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalRangeNumeric)).EndInit();
            this.triggeringGroupBox.ResumeLayout(false);
            this.triggeringGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.triggerLevelNumeric)).EndInit();
            this.scope1GroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scope1DataGridView)).EndInit();
            this.scope2GroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scope2DataGridView)).EndInit();
            this.buttonsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sampleRateMinNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label resourceNameDevice1Label;
        private System.Windows.Forms.Label channelNameDevice1Label;
        private System.Windows.Forms.TextBox channelNameDevice1TextBox;
        private System.Windows.Forms.Button acquireButton;
        private System.Windows.Forms.GroupBox scopeDevice1GroupBox;
        private System.Windows.Forms.GroupBox scopeDevice2GroupBox;
        private System.Windows.Forms.Label channelNameDevice2Label;
        private System.Windows.Forms.Label resourceNameDevice2Label;
        private System.Windows.Forms.TextBox channelNameDevice2TextBox;
        private System.Windows.Forms.GroupBox commonConfigurationGroupBox;
        private System.Windows.Forms.Label recordLengthMinLabel;
        private System.Windows.Forms.Label sampleRateMinLabel;
        private System.Windows.Forms.Label verticalRangeLabel;
        private System.Windows.Forms.GroupBox triggeringGroupBox;
        private System.Windows.Forms.Label triggerLevelLabel;
        private System.Windows.Forms.Label triggerSourceLabel;
        private System.Windows.Forms.NumericUpDown triggerLevelNumeric;
        private System.Windows.Forms.ComboBox triggerSourceComboBox;
        private System.Windows.Forms.Label maximumInputFrequencyLabel;
        private System.Windows.Forms.ComboBox triggerTypeComboBox;
        private System.Windows.Forms.Label triggerTypeLabel;
        private System.Windows.Forms.ComboBox resourceNameDevice1ComboBox;
        private System.Windows.Forms.ComboBox resourceNameDevice2ComboBox;
        private System.Windows.Forms.NumericUpDown recordLengthMinNumeric;
        private System.Windows.Forms.NumericUpDown verticalRangeNumeric;
        private System.Windows.Forms.NumericUpDown maximumInputFrequencyNumeric;
        private System.Windows.Forms.GroupBox scope1GroupBox;
        private System.Windows.Forms.DataGridView scope1DataGridView;
        private System.Windows.Forms.GroupBox scope2GroupBox;
        private System.Windows.Forms.DataGridView scope2DataGridView;
        private System.Windows.Forms.GroupBox buttonsGroupBox;
        private System.Windows.Forms.NumericUpDown sampleRateMinNumeric;
    }
}

