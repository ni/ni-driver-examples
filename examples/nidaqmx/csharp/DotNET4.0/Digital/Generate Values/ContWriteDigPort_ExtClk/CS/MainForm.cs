/******************************************************************************
*
* Example program:
*   ContWriteDigPort_ExtClk
*
* Category:
*   DO
*
* Description:
*   This example demonstrates how to output a continuous digital pattern using
*   an external clock.
*
* Instructions for running:
*   1.  Select the Physical Channel to correspond to where your signal is output
*       on the DAQ device.
*   2.  Select the Clock Source for the generation.
*   3.  Specify the Rate of the output digital pattern.
*   4.  Enter the digital pattern data.
*
* Steps:
*   1.  Create a new task.
*   2.  Create the digital output channel.
*   3.  Configure the task to use an external sample clock.
*   4.  Set the sample mode for continuous samples.
*   5.  Create a DigitalSingleChannelWriter and associate it with the task by
*       using the task's stream.
*   6.  Call the DigitalSingleChannelWriter.WriteMultiSample method to write the
*       digital pattern to a buffer.
*   7.  Call Task.Start().
*   8.  When the user presses the stop button, stop the task.
*   9.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   10. Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal input terminals match the physical channel text box. 
*   For more information on the input and output terminals for your device, open
*   the NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
*   Considerations books in the table of contents.
*
* Microsoft Windows Vista User Account Control
*   Running certain applications on Microsoft Windows Vista requires
*   administrator privileges, 
*   because the application name contains keywords such as setup, update, or
*   install. To avoid this problem, 
*   you must add an additional manifest to the application that specifies the
*   privileges required to run 
*   the application. Some Measurement Studio NI-DAQmx examples for Visual Studio
*   include these keywords. 
*   Therefore, all examples for Visual Studio are shipped with an additional
*   manifest file that you must 
*   embed in the example executable. The manifest file is named
*   [ExampleName].exe.manifest, where [ExampleName] 
*   is the NI-provided example name. For information on how to embed the manifest
*   file, refer to http://msdn2.microsoft.com/en-us/library/bb756929.aspx.Note: 
*   The manifest file is not provided with examples for Visual Studio .NET 2003.
*
******************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using NationalInstruments.DAQmx;

namespace NationalInstruments.Examples.ContWriteDigPort_ExtClk
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label physicalChannelsLabel;
        private System.Windows.Forms.ComboBox physicalChannelsComboBox;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.Label clockSourceLabel;
        private System.Windows.Forms.ComboBox clockSourceComboBox;
        private System.Windows.Forms.Label frequencyLabel;		
        internal System.Windows.Forms.NumericUpDown frequencyNumericUpDown;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        internal System.Windows.Forms.NumericUpDown arrayData0NumericUpDown;
        internal System.Windows.Forms.NumericUpDown arrayData1NumericUpDown;
        internal System.Windows.Forms.NumericUpDown arrayData2NumericUpDown;
        internal System.Windows.Forms.NumericUpDown arrayData3NumericUpDown;
        internal System.Windows.Forms.NumericUpDown arrayData4NumericUpDown;
        internal System.Windows.Forms.NumericUpDown arrayData7NumericUpDown;
        internal System.Windows.Forms.NumericUpDown arrayData6NumericUpDown;
        internal System.Windows.Forms.NumericUpDown arrayData5NumericUpDown;
        private System.Windows.Forms.GroupBox dataToWriteGroupBox;
        private System.Windows.Forms.Label arrayData0Label;
        private System.Windows.Forms.Label arrayData1Label;
        private System.Windows.Forms.Label arrayData2Label;
        private System.Windows.Forms.Label arrayData3Label;
        private System.Windows.Forms.Label arrayData4Label;
        private System.Windows.Forms.Label arrayData5Label;
        private System.Windows.Forms.Label arrayData6Label;
        private System.Windows.Forms.Label arrayData7Label;

        private Task myTask;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            physicalChannelsComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External));
            clockSourceComboBox.Items.AddRange(DaqSystem.Local.GetTerminals(TerminalTypes.All));

            if (physicalChannelsComboBox.Items.Count > 0)
            {
                clockSourceComboBox.SelectedIndex = 1;
                physicalChannelsComboBox.SelectedIndex = 0;
                startButton.Enabled = true;
            }		
            
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
                if (myTask != null)
                {
                    myTask.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelsComboBox = new System.Windows.Forms.ComboBox();
            this.physicalChannelsLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.frequencyNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.clockSourceLabel = new System.Windows.Forms.Label();
            this.clockSourceComboBox = new System.Windows.Forms.ComboBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.arrayData0NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.arrayData1NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.arrayData2NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.arrayData3NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.arrayData4NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.arrayData7NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.arrayData6NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.arrayData5NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.dataToWriteGroupBox = new System.Windows.Forms.GroupBox();
            this.arrayData7Label = new System.Windows.Forms.Label();
            this.arrayData6Label = new System.Windows.Forms.Label();
            this.arrayData5Label = new System.Windows.Forms.Label();
            this.arrayData4Label = new System.Windows.Forms.Label();
            this.arrayData3Label = new System.Windows.Forms.Label();
            this.arrayData2Label = new System.Windows.Forms.Label();
            this.arrayData1Label = new System.Windows.Forms.Label();
            this.arrayData0Label = new System.Windows.Forms.Label();
            this.channelParametersGroupBox.SuspendLayout();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData0NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData1NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData2NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData3NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData4NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData7NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData6NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData5NumericUpDown)).BeginInit();
            this.dataToWriteGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelsComboBox);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelsLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(256, 56);
            this.channelParametersGroupBox.TabIndex = 0;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelsComboBox
            // 
            this.physicalChannelsComboBox.Location = new System.Drawing.Point(152, 24);
            this.physicalChannelsComboBox.Name = "physicalChannelsComboBox";
            this.physicalChannelsComboBox.Size = new System.Drawing.Size(96, 21);
            this.physicalChannelsComboBox.TabIndex = 1;
            this.physicalChannelsComboBox.Text = "Dev1/port0";
            // 
            // physicalChannelsLabel
            // 
            this.physicalChannelsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelsLabel.Location = new System.Drawing.Point(16, 24);
            this.physicalChannelsLabel.Name = "physicalChannelsLabel";
            this.physicalChannelsLabel.Size = new System.Drawing.Size(112, 24);
            this.physicalChannelsLabel.TabIndex = 0;
            this.physicalChannelsLabel.Text = "Physical Channels:";
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.frequencyNumericUpDown);
            this.timingParametersGroupBox.Controls.Add(this.frequencyLabel);
            this.timingParametersGroupBox.Controls.Add(this.clockSourceLabel);
            this.timingParametersGroupBox.Controls.Add(this.clockSourceComboBox);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 88);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(256, 104);
            this.timingParametersGroupBox.TabIndex = 1;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // frequencyNumericUpDown
            // 
            this.frequencyNumericUpDown.Location = new System.Drawing.Point(152, 64);
            this.frequencyNumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                             100000,
                                                                             0,
                                                                             0,
                                                                             0});
            this.frequencyNumericUpDown.Minimum = new System.Decimal(new int[] {
                                                                             1,
                                                                             0,
                                                                             0,
                                                                             0});
            this.frequencyNumericUpDown.Name = "frequencyNumericUpDown";
            this.frequencyNumericUpDown.Size = new System.Drawing.Size(96, 20);
            this.frequencyNumericUpDown.TabIndex = 3;
            this.frequencyNumericUpDown.Value = new System.Decimal(new int[] {
                                                                           100,
                                                                           0,
                                                                           0,
                                                                           0});
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.frequencyLabel.Location = new System.Drawing.Point(16, 64);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(128, 14);
            this.frequencyLabel.TabIndex = 2;
            this.frequencyLabel.Text = "Sample Clock Rate:";
            // 
            // clockSourceLabel
            // 
            this.clockSourceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clockSourceLabel.Location = new System.Drawing.Point(16, 24);
            this.clockSourceLabel.Name = "clockSourceLabel";
            this.clockSourceLabel.Size = new System.Drawing.Size(88, 14);
            this.clockSourceLabel.TabIndex = 0;
            this.clockSourceLabel.Text = "Clock Source:";
            // 
            // clockSourceComboBox
            // 
            this.clockSourceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.clockSourceComboBox.Location = new System.Drawing.Point(152, 24);
            this.clockSourceComboBox.Name = "clockSourceComboBox";
            this.clockSourceComboBox.Size = new System.Drawing.Size(96, 21);
            this.clockSourceComboBox.TabIndex = 1;
            this.clockSourceComboBox.Text = "/Dev1/PFI0";
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(40, 209);
            this.startButton.Name = "startButton";
            this.startButton.TabIndex = 3;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(152, 209);
            this.stopButton.Name = "stopButton";
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // arrayData0NumericUpDown
            // 
            this.arrayData0NumericUpDown.Location = new System.Drawing.Point(120, 24);
            this.arrayData0NumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                    255,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.arrayData0NumericUpDown.Name = "arrayData0NumericUpDown";
            this.arrayData0NumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.arrayData0NumericUpDown.TabIndex = 1;
            this.arrayData0NumericUpDown.Value = new System.Decimal(new int[] {
                                                                                  1,
                                                                                  0,
                                                                                  0,
                                                                                  0});
            // 
            // arrayData1NumericUpDown
            // 
            this.arrayData1NumericUpDown.Location = new System.Drawing.Point(120, 48);
            this.arrayData1NumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                    255,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.arrayData1NumericUpDown.Name = "arrayData1NumericUpDown";
            this.arrayData1NumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.arrayData1NumericUpDown.TabIndex = 3;
            this.arrayData1NumericUpDown.Value = new System.Decimal(new int[] {
                                                                                  2,
                                                                                  0,
                                                                                  0,
                                                                                  0});
            // 
            // arrayData2NumericUpDown
            // 
            this.arrayData2NumericUpDown.Location = new System.Drawing.Point(120, 72);
            this.arrayData2NumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                    255,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.arrayData2NumericUpDown.Name = "arrayData2NumericUpDown";
            this.arrayData2NumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.arrayData2NumericUpDown.TabIndex = 5;
            this.arrayData2NumericUpDown.Value = new System.Decimal(new int[] {
                                                                                  4,
                                                                                  0,
                                                                                  0,
                                                                                  0});
            // 
            // arrayData3NumericUpDown
            // 
            this.arrayData3NumericUpDown.Location = new System.Drawing.Point(120, 96);
            this.arrayData3NumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                    255,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.arrayData3NumericUpDown.Name = "arrayData3NumericUpDown";
            this.arrayData3NumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.arrayData3NumericUpDown.TabIndex = 7;
            this.arrayData3NumericUpDown.Value = new System.Decimal(new int[] {
                                                                                  8,
                                                                                  0,
                                                                                  0,
                                                                                  0});
            // 
            // arrayData4NumericUpDown
            // 
            this.arrayData4NumericUpDown.Location = new System.Drawing.Point(120, 120);
            this.arrayData4NumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                    255,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.arrayData4NumericUpDown.Name = "arrayData4NumericUpDown";
            this.arrayData4NumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.arrayData4NumericUpDown.TabIndex = 9;
            this.arrayData4NumericUpDown.Value = new System.Decimal(new int[] {
                                                                                  16,
                                                                                  0,
                                                                                  0,
                                                                                  0});
            // 
            // arrayData7NumericUpDown
            // 
            this.arrayData7NumericUpDown.Location = new System.Drawing.Point(120, 192);
            this.arrayData7NumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                    255,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.arrayData7NumericUpDown.Name = "arrayData7NumericUpDown";
            this.arrayData7NumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.arrayData7NumericUpDown.TabIndex = 15;
            this.arrayData7NumericUpDown.Value = new System.Decimal(new int[] {
                                                                                  128,
                                                                                  0,
                                                                                  0,
                                                                                  0});
            // 
            // arrayData6NumericUpDown
            // 
            this.arrayData6NumericUpDown.Location = new System.Drawing.Point(120, 168);
            this.arrayData6NumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                    255,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.arrayData6NumericUpDown.Name = "arrayData6NumericUpDown";
            this.arrayData6NumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.arrayData6NumericUpDown.TabIndex = 13;
            this.arrayData6NumericUpDown.Value = new System.Decimal(new int[] {
                                                                                  64,
                                                                                  0,
                                                                                  0,
                                                                                  0});
            // 
            // arrayData5NumericUpDown
            // 
            this.arrayData5NumericUpDown.Location = new System.Drawing.Point(120, 144);
            this.arrayData5NumericUpDown.Maximum = new System.Decimal(new int[] {
                                                                                    255,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.arrayData5NumericUpDown.Name = "arrayData5NumericUpDown";
            this.arrayData5NumericUpDown.Size = new System.Drawing.Size(64, 20);
            this.arrayData5NumericUpDown.TabIndex = 11;
            this.arrayData5NumericUpDown.Value = new System.Decimal(new int[] {
                                                                                  32,
                                                                                  0,
                                                                                  0,
                                                                                  0});
            // 
            // dataToWriteGroupBox
            // 
            this.dataToWriteGroupBox.Controls.Add(this.arrayData7Label);
            this.dataToWriteGroupBox.Controls.Add(this.arrayData6Label);
            this.dataToWriteGroupBox.Controls.Add(this.arrayData5Label);
            this.dataToWriteGroupBox.Controls.Add(this.arrayData4Label);
            this.dataToWriteGroupBox.Controls.Add(this.arrayData3Label);
            this.dataToWriteGroupBox.Controls.Add(this.arrayData2Label);
            this.dataToWriteGroupBox.Controls.Add(this.arrayData1Label);
            this.dataToWriteGroupBox.Controls.Add(this.arrayData0Label);
            this.dataToWriteGroupBox.Controls.Add(this.arrayData3NumericUpDown);
            this.dataToWriteGroupBox.Controls.Add(this.arrayData2NumericUpDown);
            this.dataToWriteGroupBox.Controls.Add(this.arrayData0NumericUpDown);
            this.dataToWriteGroupBox.Controls.Add(this.arrayData6NumericUpDown);
            this.dataToWriteGroupBox.Controls.Add(this.arrayData4NumericUpDown);
            this.dataToWriteGroupBox.Controls.Add(this.arrayData1NumericUpDown);
            this.dataToWriteGroupBox.Controls.Add(this.arrayData7NumericUpDown);
            this.dataToWriteGroupBox.Controls.Add(this.arrayData5NumericUpDown);
            this.dataToWriteGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataToWriteGroupBox.Location = new System.Drawing.Point(272, 8);
            this.dataToWriteGroupBox.Name = "dataToWriteGroupBox";
            this.dataToWriteGroupBox.Size = new System.Drawing.Size(200, 224);
            this.dataToWriteGroupBox.TabIndex = 2;
            this.dataToWriteGroupBox.TabStop = false;
            this.dataToWriteGroupBox.Text = "Pattern";
            // 
            // arrayData7Label
            // 
            this.arrayData7Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.arrayData7Label.Location = new System.Drawing.Point(24, 192);
            this.arrayData7Label.Name = "arrayData7Label";
            this.arrayData7Label.Size = new System.Drawing.Size(72, 24);
            this.arrayData7Label.TabIndex = 14;
            this.arrayData7Label.Text = "Array Data 7:";
            // 
            // arrayData6Label
            // 
            this.arrayData6Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.arrayData6Label.Location = new System.Drawing.Point(24, 168);
            this.arrayData6Label.Name = "arrayData6Label";
            this.arrayData6Label.Size = new System.Drawing.Size(72, 24);
            this.arrayData6Label.TabIndex = 12;
            this.arrayData6Label.Text = "Array Data 6:";
            // 
            // arrayData5Label
            // 
            this.arrayData5Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.arrayData5Label.Location = new System.Drawing.Point(24, 144);
            this.arrayData5Label.Name = "arrayData5Label";
            this.arrayData5Label.Size = new System.Drawing.Size(72, 24);
            this.arrayData5Label.TabIndex = 10;
            this.arrayData5Label.Text = "Array Data 5:";
            // 
            // arrayData4Label
            // 
            this.arrayData4Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.arrayData4Label.Location = new System.Drawing.Point(24, 120);
            this.arrayData4Label.Name = "arrayData4Label";
            this.arrayData4Label.Size = new System.Drawing.Size(72, 24);
            this.arrayData4Label.TabIndex = 8;
            this.arrayData4Label.Text = "Array Data 4:";
            // 
            // arrayData3Label
            // 
            this.arrayData3Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.arrayData3Label.Location = new System.Drawing.Point(24, 96);
            this.arrayData3Label.Name = "arrayData3Label";
            this.arrayData3Label.Size = new System.Drawing.Size(72, 24);
            this.arrayData3Label.TabIndex = 6;
            this.arrayData3Label.Text = "Array Data 3:";
            // 
            // arrayData2Label
            // 
            this.arrayData2Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.arrayData2Label.Location = new System.Drawing.Point(24, 72);
            this.arrayData2Label.Name = "arrayData2Label";
            this.arrayData2Label.Size = new System.Drawing.Size(72, 24);
            this.arrayData2Label.TabIndex = 4;
            this.arrayData2Label.Text = "Array Data 2:";
            // 
            // arrayData1Label
            // 
            this.arrayData1Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.arrayData1Label.Location = new System.Drawing.Point(24, 48);
            this.arrayData1Label.Name = "arrayData1Label";
            this.arrayData1Label.Size = new System.Drawing.Size(72, 24);
            this.arrayData1Label.TabIndex = 2;
            this.arrayData1Label.Text = "Array Data 1:";
            // 
            // arrayData0Label
            // 
            this.arrayData0Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.arrayData0Label.Location = new System.Drawing.Point(24, 24);
            this.arrayData0Label.Name = "arrayData0Label";
            this.arrayData0Label.Size = new System.Drawing.Size(72, 24);
            this.arrayData0Label.TabIndex = 0;
            this.arrayData0Label.Text = "Array Data 0:";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(498, 248);
            this.Controls.Add(this.dataToWriteGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Continuous Write Digital Port - External Clock";
            this.channelParametersGroupBox.ResumeLayout(false);
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData0NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData1NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData2NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData3NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData4NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData7NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData6NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrayData5NumericUpDown)).EndInit();
            this.dataToWriteGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            Application.EnableVisualStyles();
            Application.DoEvents();
            Application.Run(new MainForm());
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            int[] data = new int[8];

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                myTask = new Task();

                myTask.DOChannels.CreateChannel(physicalChannelsComboBox.Text, "port", 
                    ChannelLineGrouping.OneChannelForAllLines);

                myTask.Timing.ConfigureSampleClock(clockSourceComboBox.Text,
                    (double) frequencyNumericUpDown.Value,
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.ContinuousSamples);

                myTask.Control(TaskAction.Verify);

                myTask.Done += new TaskDoneEventHandler(myTask_Done);

                DigitalSingleChannelWriter writer = 
                    new DigitalSingleChannelWriter(myTask.Stream);
                
                data[0] = (int) arrayData0NumericUpDown.Value;
                data[1] = (int) arrayData1NumericUpDown.Value;
                data[2] = (int) arrayData2NumericUpDown.Value;
                data[3] = (int) arrayData3NumericUpDown.Value;
                data[4] = (int) arrayData4NumericUpDown.Value;
                data[5] = (int) arrayData5NumericUpDown.Value;
                data[6] = (int) arrayData6NumericUpDown.Value;
                data[7] = (int) arrayData7NumericUpDown.Value;

                writer.WriteMultiSamplePort(false, data);

                myTask.Start(); 

                startButton.Enabled = false;
                stopButton.Enabled = true;
                channelParametersGroupBox.Enabled = false;
                timingParametersGroupBox.Enabled = false;
                dataToWriteGroupBox.Enabled = false;
            }
            catch (DaqException exception)
            {
                myTask.Dispose();
                MessageBox.Show(exception.Message);

                ResetButtons();
            }			
            finally 
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            myTask.Dispose();			
            ResetButtons();
        }

        private void myTask_Done(object sender, TaskDoneEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }

            if (myTask != null)
            {
                myTask.Dispose();
            }

            ResetButtons();
        }

        private void ResetButtons ()
        {
            startButton.Enabled = true;
            stopButton.Enabled = false;
            channelParametersGroupBox.Enabled = true;
            timingParametersGroupBox.Enabled = true;
            dataToWriteGroupBox.Enabled = true;
        }
    }
}
