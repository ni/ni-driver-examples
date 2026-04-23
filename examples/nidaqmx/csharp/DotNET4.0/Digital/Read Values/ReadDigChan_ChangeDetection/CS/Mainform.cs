/******************************************************************************
*
* Example program:
*   ReadDigChan_ChangeDetection
*
* Category:
*   DI
*
* Description:
*   This example demonstrates how to read values from one or more digital input
*   channels, using change detection timing.
*
* Instructions for running:
*   1.  Select the digital lines on the DAQ device to be read.
*   2.  Select which digital lines should trigger the completion of a read for
*       rising and falling edges.
*
* Steps:
*   1.  Create a new task and a digital input channel. Use one channel for all
*       lines.
*   2.  Call the Timing.ConfigureChangeDetection method to set up change
*       detection timing for the task.
*   3.  Call Task.Start() to start the task.
*   4.  Call DigitalSingleChannelReader.BeginReadSingleSampleMultiLine to
*       install a callback and begin the asynchronous read operation.
*   5.  Inside the callback, call
*       DigitalSingleChannelReader.EndReadSingleSampleMultiLine to retrieve the
*       data from the read operation.  
*   6.  Call DigitalSingleChannelReader.BeginReadSingleSampleMultiLine again
*       inside the callback to perform another read operation.
*   7.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   8.  Handle any DaqExceptions, if they occur.
*
*   Note: This example sets SynchronizeCallback to true. If SynchronizeCallback
*   is set to false, then you must give special consideration to safely dispose
*   the task and to update the UI from the callback. If SynchronizeCallback is
*   set to false, the callback executes on the worker thread and not on the main
*   UI thread. You can only update a UI component on the thread on which it was
*   created. Refer to the How to: Safely Dispose Task When Using Asynchronous
*   Callbacks topic in the NI-DAQmx .NET help for more information.
*
* I/O Connections Overview:
*   Make sure your signal input terminals match the Lines I/O Control. In this
*   case wire your digital signals to the first eight digital lines on your DAQ
*   Device.  For more information on the input and output terminals for your
*   device, open the NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals
*   and Device Considerations books in the table of contents.NOTE: For NI-6534
*   devices, either 32 bytes of data needs to be transferred first for the DMA
*   transfer to take place, or interrupts must be used instead of DMA.
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

namespace NationalInstruments.Examples.ReadDigChan_ChangeDetection
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {       
        private Task runningTask;
        private Task myTask;        
        private DigitalSingleChannelReader myDigitalReader;
        private System.Windows.Forms.GroupBox channelParamtersGroupBox;
        private System.Windows.Forms.GroupBox dataReadGroupBox;
        private System.Windows.Forms.Label hexLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.TextBox hexTextBox;
        private System.Windows.Forms.Label bit7Label;
        private System.Windows.Forms.Label bit6Label;
        private System.Windows.Forms.Label bit5Label;
        private System.Windows.Forms.Label bit4Label;
        private System.Windows.Forms.Label bit3Label;
        private System.Windows.Forms.Label bit2Label;
        private System.Windows.Forms.Label bit1Label;
        private System.Windows.Forms.Label bit0Label;
        private System.Windows.Forms.Label linesLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.CheckBox line7CheckBox;
        private System.Windows.Forms.CheckBox line6CheckBox;
        private System.Windows.Forms.CheckBox line5CheckBox;
        private System.Windows.Forms.CheckBox line4CheckBox;
        private System.Windows.Forms.CheckBox line3CheckBox;
        private System.Windows.Forms.CheckBox line2CheckBox;
        private System.Windows.Forms.CheckBox line1CheckBox;
        private System.Windows.Forms.CheckBox line0CheckBox;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.Windows.Forms.ComboBox risingEdgeLinesComboBox;
        private System.Windows.Forms.ComboBox fallingEdgeLinesComboBox;
        private System.ComponentModel.Container components = null;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine, PhysicalChannelAccess.External));
            risingEdgeLinesComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine | PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External));
            fallingEdgeLinesComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DILine | PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External));
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
                    runningTask = null;
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
            this.channelParamtersGroupBox = new System.Windows.Forms.GroupBox();
            this.fallingEdgeLinesComboBox = new System.Windows.Forms.ComboBox();
            this.risingEdgeLinesComboBox = new System.Windows.Forms.ComboBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.warningLabel = new System.Windows.Forms.Label();
            this.linesLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataReadGroupBox = new System.Windows.Forms.GroupBox();
            this.bit0Label = new System.Windows.Forms.Label();
            this.bit1Label = new System.Windows.Forms.Label();
            this.bit2Label = new System.Windows.Forms.Label();
            this.bit3Label = new System.Windows.Forms.Label();
            this.bit4Label = new System.Windows.Forms.Label();
            this.bit5Label = new System.Windows.Forms.Label();
            this.bit6Label = new System.Windows.Forms.Label();
            this.bit7Label = new System.Windows.Forms.Label();
            this.line7CheckBox = new System.Windows.Forms.CheckBox();
            this.line6CheckBox = new System.Windows.Forms.CheckBox();
            this.line5CheckBox = new System.Windows.Forms.CheckBox();
            this.line4CheckBox = new System.Windows.Forms.CheckBox();
            this.line3CheckBox = new System.Windows.Forms.CheckBox();
            this.line2CheckBox = new System.Windows.Forms.CheckBox();
            this.line1CheckBox = new System.Windows.Forms.CheckBox();
            this.hexLabel = new System.Windows.Forms.Label();
            this.hexTextBox = new System.Windows.Forms.TextBox();
            this.line0CheckBox = new System.Windows.Forms.CheckBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.channelParamtersGroupBox.SuspendLayout();
            this.dataReadGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelParamtersGroupBox
            // 
            this.channelParamtersGroupBox.Controls.Add(this.fallingEdgeLinesComboBox);
            this.channelParamtersGroupBox.Controls.Add(this.risingEdgeLinesComboBox);
            this.channelParamtersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParamtersGroupBox.Controls.Add(this.warningLabel);
            this.channelParamtersGroupBox.Controls.Add(this.linesLabel);
            this.channelParamtersGroupBox.Controls.Add(this.label1);
            this.channelParamtersGroupBox.Controls.Add(this.label2);
            this.channelParamtersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParamtersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParamtersGroupBox.Name = "channelParamtersGroupBox";
            this.channelParamtersGroupBox.Size = new System.Drawing.Size(216, 192);
            this.channelParamtersGroupBox.TabIndex = 2;
            this.channelParamtersGroupBox.TabStop = false;
            this.channelParamtersGroupBox.Text = "Channel Parameters:";
            // 
            // fallingEdgeLinesComboBox
            // 
            this.fallingEdgeLinesComboBox.Location = new System.Drawing.Point(8, 160);
            this.fallingEdgeLinesComboBox.Name = "fallingEdgeLinesComboBox";
            this.fallingEdgeLinesComboBox.Size = new System.Drawing.Size(200, 21);
            this.fallingEdgeLinesComboBox.TabIndex = 6;
            this.fallingEdgeLinesComboBox.Text = "Dev1/port0/line0:7";
            // 
            // risingEdgeLinesComboBox
            // 
            this.risingEdgeLinesComboBox.Location = new System.Drawing.Point(8, 112);
            this.risingEdgeLinesComboBox.Name = "risingEdgeLinesComboBox";
            this.risingEdgeLinesComboBox.Size = new System.Drawing.Size(200, 21);
            this.risingEdgeLinesComboBox.TabIndex = 4;
            this.risingEdgeLinesComboBox.Text = "Dev1/port0/line0:7";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(8, 32);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(200, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/port0/line0:7";
            // 
            // warningLabel
            // 
            this.warningLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.warningLabel.Location = new System.Drawing.Point(8, 56);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(184, 32);
            this.warningLabel.TabIndex = 2;
            this.warningLabel.Text = "You must specify eight lines in the channel string";
            // 
            // linesLabel
            // 
            this.linesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.linesLabel.Location = new System.Drawing.Point(8, 16);
            this.linesLabel.Name = "linesLabel";
            this.linesLabel.Size = new System.Drawing.Size(40, 16);
            this.linesLabel.TabIndex = 0;
            this.linesLabel.Text = "Lines:";
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(8, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Rising Edge Lines";
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(8, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Falling Edge Lines";
            // 
            // dataReadGroupBox
            // 
            this.dataReadGroupBox.Controls.Add(this.bit0Label);
            this.dataReadGroupBox.Controls.Add(this.bit1Label);
            this.dataReadGroupBox.Controls.Add(this.bit2Label);
            this.dataReadGroupBox.Controls.Add(this.bit3Label);
            this.dataReadGroupBox.Controls.Add(this.bit4Label);
            this.dataReadGroupBox.Controls.Add(this.bit5Label);
            this.dataReadGroupBox.Controls.Add(this.bit6Label);
            this.dataReadGroupBox.Controls.Add(this.bit7Label);
            this.dataReadGroupBox.Controls.Add(this.line7CheckBox);
            this.dataReadGroupBox.Controls.Add(this.line6CheckBox);
            this.dataReadGroupBox.Controls.Add(this.line5CheckBox);
            this.dataReadGroupBox.Controls.Add(this.line4CheckBox);
            this.dataReadGroupBox.Controls.Add(this.line3CheckBox);
            this.dataReadGroupBox.Controls.Add(this.line2CheckBox);
            this.dataReadGroupBox.Controls.Add(this.line1CheckBox);
            this.dataReadGroupBox.Controls.Add(this.hexLabel);
            this.dataReadGroupBox.Controls.Add(this.hexTextBox);
            this.dataReadGroupBox.Controls.Add(this.line0CheckBox);
            this.dataReadGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataReadGroupBox.Location = new System.Drawing.Point(8, 208);
            this.dataReadGroupBox.Name = "dataReadGroupBox";
            this.dataReadGroupBox.Size = new System.Drawing.Size(216, 112);
            this.dataReadGroupBox.TabIndex = 3;
            this.dataReadGroupBox.TabStop = false;
            this.dataReadGroupBox.Text = "Data Read:";
            // 
            // bit0Label
            // 
            this.bit0Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bit0Label.Location = new System.Drawing.Point(184, 80);
            this.bit0Label.Name = "bit0Label";
            this.bit0Label.Size = new System.Drawing.Size(16, 16);
            this.bit0Label.TabIndex = 16;
            this.bit0Label.Text = "7";
            // 
            // bit1Label
            // 
            this.bit1Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bit1Label.Location = new System.Drawing.Point(160, 80);
            this.bit1Label.Name = "bit1Label";
            this.bit1Label.Size = new System.Drawing.Size(16, 16);
            this.bit1Label.TabIndex = 14;
            this.bit1Label.Text = "6";
            // 
            // bit2Label
            // 
            this.bit2Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bit2Label.Location = new System.Drawing.Point(136, 80);
            this.bit2Label.Name = "bit2Label";
            this.bit2Label.Size = new System.Drawing.Size(16, 16);
            this.bit2Label.TabIndex = 12;
            this.bit2Label.Text = "5";
            // 
            // bit3Label
            // 
            this.bit3Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bit3Label.Location = new System.Drawing.Point(112, 80);
            this.bit3Label.Name = "bit3Label";
            this.bit3Label.Size = new System.Drawing.Size(16, 16);
            this.bit3Label.TabIndex = 10;
            this.bit3Label.Text = "4";
            // 
            // bit4Label
            // 
            this.bit4Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bit4Label.Location = new System.Drawing.Point(88, 80);
            this.bit4Label.Name = "bit4Label";
            this.bit4Label.Size = new System.Drawing.Size(16, 16);
            this.bit4Label.TabIndex = 8;
            this.bit4Label.Text = "3";
            // 
            // bit5Label
            // 
            this.bit5Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bit5Label.Location = new System.Drawing.Point(64, 80);
            this.bit5Label.Name = "bit5Label";
            this.bit5Label.Size = new System.Drawing.Size(16, 16);
            this.bit5Label.TabIndex = 6;
            this.bit5Label.Text = "2";
            // 
            // bit6Label
            // 
            this.bit6Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bit6Label.Location = new System.Drawing.Point(40, 80);
            this.bit6Label.Name = "bit6Label";
            this.bit6Label.Size = new System.Drawing.Size(16, 16);
            this.bit6Label.TabIndex = 4;
            this.bit6Label.Text = "1";
            // 
            // bit7Label
            // 
            this.bit7Label.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bit7Label.Location = new System.Drawing.Point(16, 80);
            this.bit7Label.Name = "bit7Label";
            this.bit7Label.Size = new System.Drawing.Size(8, 16);
            this.bit7Label.TabIndex = 2;
            this.bit7Label.Text = "0";
            // 
            // line7CheckBox
            // 
            this.line7CheckBox.AutoCheck = false;
            this.line7CheckBox.Enabled = false;
            this.line7CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line7CheckBox.Location = new System.Drawing.Point(184, 56);
            this.line7CheckBox.Name = "line7CheckBox";
            this.line7CheckBox.Size = new System.Drawing.Size(16, 24);
            this.line7CheckBox.TabIndex = 17;
            this.line7CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // line6CheckBox
            // 
            this.line6CheckBox.AutoCheck = false;
            this.line6CheckBox.Enabled = false;
            this.line6CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line6CheckBox.Location = new System.Drawing.Point(160, 56);
            this.line6CheckBox.Name = "line6CheckBox";
            this.line6CheckBox.Size = new System.Drawing.Size(16, 24);
            this.line6CheckBox.TabIndex = 15;
            this.line6CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // line5CheckBox
            // 
            this.line5CheckBox.AutoCheck = false;
            this.line5CheckBox.Enabled = false;
            this.line5CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line5CheckBox.Location = new System.Drawing.Point(136, 56);
            this.line5CheckBox.Name = "line5CheckBox";
            this.line5CheckBox.Size = new System.Drawing.Size(16, 24);
            this.line5CheckBox.TabIndex = 13;
            this.line5CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // line4CheckBox
            // 
            this.line4CheckBox.AutoCheck = false;
            this.line4CheckBox.Enabled = false;
            this.line4CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line4CheckBox.Location = new System.Drawing.Point(112, 56);
            this.line4CheckBox.Name = "line4CheckBox";
            this.line4CheckBox.Size = new System.Drawing.Size(16, 24);
            this.line4CheckBox.TabIndex = 11;
            this.line4CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // line3CheckBox
            // 
            this.line3CheckBox.AutoCheck = false;
            this.line3CheckBox.Enabled = false;
            this.line3CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line3CheckBox.Location = new System.Drawing.Point(88, 56);
            this.line3CheckBox.Name = "line3CheckBox";
            this.line3CheckBox.Size = new System.Drawing.Size(16, 24);
            this.line3CheckBox.TabIndex = 9;
            this.line3CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // line2CheckBox
            // 
            this.line2CheckBox.AutoCheck = false;
            this.line2CheckBox.Enabled = false;
            this.line2CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line2CheckBox.Location = new System.Drawing.Point(64, 56);
            this.line2CheckBox.Name = "line2CheckBox";
            this.line2CheckBox.Size = new System.Drawing.Size(16, 24);
            this.line2CheckBox.TabIndex = 7;
            this.line2CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // line1CheckBox
            // 
            this.line1CheckBox.AutoCheck = false;
            this.line1CheckBox.Enabled = false;
            this.line1CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line1CheckBox.Location = new System.Drawing.Point(40, 56);
            this.line1CheckBox.Name = "line1CheckBox";
            this.line1CheckBox.Size = new System.Drawing.Size(16, 24);
            this.line1CheckBox.TabIndex = 5;
            this.line1CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hexLabel
            // 
            this.hexLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.hexLabel.Location = new System.Drawing.Point(8, 24);
            this.hexLabel.Name = "hexLabel";
            this.hexLabel.Size = new System.Drawing.Size(32, 16);
            this.hexLabel.TabIndex = 0;
            this.hexLabel.Text = "Hex:";
            // 
            // hexTextBox
            // 
            this.hexTextBox.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.hexTextBox.Enabled = false;
            this.hexTextBox.Location = new System.Drawing.Point(40, 24);
            this.hexTextBox.Name = "hexTextBox";
            this.hexTextBox.ReadOnly = true;
            this.hexTextBox.Size = new System.Drawing.Size(112, 20);
            this.hexTextBox.TabIndex = 1;
            this.hexTextBox.Text = "0x0";
            // 
            // line0CheckBox
            // 
            this.line0CheckBox.AutoCheck = false;
            this.line0CheckBox.Enabled = false;
            this.line0CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.line0CheckBox.Location = new System.Drawing.Point(16, 56);
            this.line0CheckBox.Name = "line0CheckBox";
            this.line0CheckBox.Size = new System.Drawing.Size(16, 24);
            this.line0CheckBox.TabIndex = 3;
            this.line0CheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(45, 336);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(144, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "&Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(45, 368);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(144, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(234, 408);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.dataReadGroupBox);
            this.Controls.Add(this.channelParamtersGroupBox);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReadDigChan-ChangeDetect";
            this.channelParamtersGroupBox.ResumeLayout(false);
            this.dataReadGroupBox.ResumeLayout(false);
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

        private void DisplayData(bool[] readData)
        {
            long val = 0;
            int i;

            try
            {
                line0CheckBox.Checked=readData[0];
                line1CheckBox.Checked=readData[1];
                line2CheckBox.Checked=readData[2];
                line3CheckBox.Checked=readData[3];
                line4CheckBox.Checked=readData[4];
                line5CheckBox.Checked=readData[5];
                line6CheckBox.Checked=readData[6];
                line7CheckBox.Checked=readData[7];

                for  (i=0;i<readData.Length;i++) 
                {
                    if (readData[i])
                    {   //if bit is true
                        //add decimal value of bit
                        val+=1L<<i;
                    }
                }

                //display read value in hex
                hexTextBox.Text=String.Format ("0x{0:X}",val);
            }
            catch (IndexOutOfRangeException exception)
            {
                //dispose task
                myTask.Dispose();
                MessageBox.Show("Error: You must specify eight lines in the channel string (i.e., 0:7).");
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }

            
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
           
            try
            {
                //Create a task such that it will be disposed after
                //we are done using it.
                myTask = new Task();
                
                //Create channel
                myTask.DIChannels.CreateChannel(physicalChannelComboBox.Text,"myChannel",
                    ChannelLineGrouping.OneChannelForAllLines);

                myTask.Timing.ConfigureChangeDetection(
                    risingEdgeLinesComboBox.Text,
                    fallingEdgeLinesComboBox.Text,
                    SampleQuantityMode.ContinuousSamples, 1000);

                myDigitalReader= new DigitalSingleChannelReader(myTask.Stream);
                
                // Use SynchronizeCallbacks to specify that the object 
                // marshals callbacks across threads appropriately.
                myDigitalReader.SynchronizeCallbacks = true;

                runningTask = myTask;
                myDigitalReader.BeginReadSingleSampleMultiLine(
                    new AsyncCallback(OnDataReady),
                    myTask);

                startButton.Enabled = false;
                stopButton.Enabled = true;
                physicalChannelComboBox.Enabled = false;
                risingEdgeLinesComboBox.Enabled = false;
                fallingEdgeLinesComboBox.Enabled = false;
            }
            catch(DaqException exception)
            {
                MessageBox.Show(exception.Message);

                //dispose task
                if(myTask != null)
                    myTask.Dispose();
            }
            Cursor.Current = Cursors.Default;
        }

        private void OnDataReady(IAsyncResult result)
        {
            try
            {
                if (runningTask != null && runningTask == result.AsyncState)
                {
                    bool[] data = myDigitalReader.EndReadSingleSampleMultiLine(result);
                    DisplayData(data);
                
                    myDigitalReader.BeginReadSingleSampleMultiLine(
                        new AsyncCallback(OnDataReady),
                        myTask);
                }
            }
            catch(DaqException ex)
            {
                runningTask = null;
                MessageBox.Show(ex.Message);
                myDigitalReader = null;
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false;
                physicalChannelComboBox.Enabled = true;
                risingEdgeLinesComboBox.Enabled = true;
                fallingEdgeLinesComboBox.Enabled = true;
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            runningTask = null;
            
            myTask.Dispose();
                        
            startButton.Enabled = true;
            stopButton.Enabled = false; 
            physicalChannelComboBox.Enabled = true;
            risingEdgeLinesComboBox.Enabled = true;
            fallingEdgeLinesComboBox.Enabled = true;
        }
    }
}
