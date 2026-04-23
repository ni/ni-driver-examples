/******************************************************************************
*
* Example program:
*   ReadDigChan
*
* Category:
*   DI
*
* Description:
*   This example demonstrates how to read values from one or more digital input
*   channels.
*
* Instructions for running:
*   1.  Select the digital lines on the DAQ device to be read.
*   2.  Click the Start button to start reading the digital channels.
*
* Steps:
*   1.  Create a Task object. Create a DIChannel object. Use one channel for all
*       lines.
*   2.  Install a timer to expire every 500 milliseconds. In the timer callback,
*       read the digital data. Continue until the user hits the stop button or
*       an error occurs. Use the DigitalSingleChannelReader object to read from
*       the channel.
*   3.  Call Task.Stop() to stop the task.
*   4.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   5.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal input terminals match the Lines textbox. In this case
*   connect your digital signals to the first eight digital lines on your DAQ
*   Device. For more information on the input and output terminals for your
*   device, open the NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals
*   and Device Considerations books in the table of contents.
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

namespace NationalInstruments.Examples.ReadDigChan
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {       
        private Task myTask;        
        private DigitalSingleChannelReader myDigitalReader;
        private System.Windows.Forms.GroupBox channelParamtersGroupBox;
        private System.Windows.Forms.GroupBox dataReadGroupBox;
        private System.Windows.Forms.Label hexLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.TextBox hexTextBox;
        private System.Windows.Forms.Timer loopTimer;
        private System.Windows.Forms.Label bit7Label;
        private System.Windows.Forms.Label bit6Label;
        private System.Windows.Forms.Label bit5Label;
        private System.Windows.Forms.Label bit4Label;
        private System.Windows.Forms.Label bit3Label;
        private System.Windows.Forms.Label bit2Label;
        private System.Windows.Forms.Label bit1Label;
        private System.Windows.Forms.Label bit0Label;
        private System.Windows.Forms.Label linesLabel;
        private System.Windows.Forms.CheckBox line7CheckBox;
        private System.Windows.Forms.CheckBox line6CheckBox;
        private System.Windows.Forms.CheckBox line5CheckBox;
        private System.Windows.Forms.CheckBox line4CheckBox;
        private System.Windows.Forms.CheckBox line3CheckBox;
        private System.Windows.Forms.CheckBox line2CheckBox;
        private System.Windows.Forms.CheckBox line1CheckBox;
        private System.Windows.Forms.CheckBox line0CheckBox;
        private System.Windows.Forms.Label displayWarningLabel;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.ComponentModel.IContainer components;

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
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.channelParamtersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.linesLabel = new System.Windows.Forms.Label();
            this.dataReadGroupBox = new System.Windows.Forms.GroupBox();
            this.displayWarningLabel = new System.Windows.Forms.Label();
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
            this.loopTimer = new System.Windows.Forms.Timer(this.components);
            this.channelParamtersGroupBox.SuspendLayout();
            this.dataReadGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelParamtersGroupBox
            // 
            this.channelParamtersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParamtersGroupBox.Controls.Add(this.linesLabel);
            this.channelParamtersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParamtersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParamtersGroupBox.Name = "channelParamtersGroupBox";
            this.channelParamtersGroupBox.Size = new System.Drawing.Size(216, 72);
            this.channelParamtersGroupBox.TabIndex = 2;
            this.channelParamtersGroupBox.TabStop = false;
            this.channelParamtersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(16, 40);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(184, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/port0/line0:7";
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
            // dataReadGroupBox
            // 
            this.dataReadGroupBox.Controls.Add(this.displayWarningLabel);
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
            this.dataReadGroupBox.Location = new System.Drawing.Point(8, 88);
            this.dataReadGroupBox.Name = "dataReadGroupBox";
            this.dataReadGroupBox.Size = new System.Drawing.Size(216, 128);
            this.dataReadGroupBox.TabIndex = 3;
            this.dataReadGroupBox.TabStop = false;
            this.dataReadGroupBox.Text = "Data Read";
            // 
            // displayWarningLabel
            // 
            this.displayWarningLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.displayWarningLabel.Location = new System.Drawing.Point(28, 104);
            this.displayWarningLabel.Name = "displayWarningLabel";
            this.displayWarningLabel.Size = new System.Drawing.Size(160, 16);
            this.displayWarningLabel.TabIndex = 18;
            this.displayWarningLabel.Text = "Lowermost 7 bits are displayed";
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
            this.hexLabel.Location = new System.Drawing.Point(8, 26);
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
            this.startButton.Location = new System.Drawing.Point(45, 232);
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
            this.stopButton.Location = new System.Drawing.Point(45, 264);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(144, 23);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // loopTimer
            // 
            this.loopTimer.Tick += new System.EventHandler(this.loopTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(234, 304);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.dataReadGroupBox);
            this.Controls.Add(this.channelParamtersGroupBox);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Read Digital Channel";
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

        private void startButton_Click(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //Create a task such that it will be disposed after
                //we are done using it.
                myTask = new Task();

                //Create channel
                myTask.DIChannels.CreateChannel(
                    physicalChannelComboBox.Text,
                    "myChannel",
                    ChannelLineGrouping.OneChannelForAllLines);

                myDigitalReader = new DigitalSingleChannelReader(myTask.Stream);

                //enable the timer
                loopTimer.Enabled = true;

                startButton.Enabled = false;
                stopButton.Enabled = true;
            }
            catch(DaqException exception)
            {
                loopTimer.Enabled = false;
                MessageBox.Show(exception.Message);

                //dispose task
                myTask.Dispose();

                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            //disable the timer and dispose of the task
            loopTimer.Enabled = false;

            myTask.Dispose();

            startButton.Enabled = true;
            stopButton.Enabled = false; 
        }

        private void loopTimer_Tick(object sender, System.EventArgs e)
        {
            try 
            {
                bool[] readData;

                //Read the digital channel
                readData = myDigitalReader.ReadSingleSampleMultiLine();

                // display only the lowemost 7 bits
                line0CheckBox.Checked = readData[0];
                line1CheckBox.Checked = readData[1];
                line2CheckBox.Checked = readData[2];
                line3CheckBox.Checked = readData[3];
                line4CheckBox.Checked = readData[4];
                line5CheckBox.Checked = readData[5];
                line6CheckBox.Checked = readData[6];
                line7CheckBox.Checked = readData[7];

                int val = 0;
                for(int i=0;i<readData.Length;i++) 
                {
                    if(readData[i])
                    {   //if bit is true
                        //add decimal value of bit
                        val += 1<<i;
                    }
                }

                //display read value in hex
                hexTextBox.Text = String.Format ("0x{0:X}",val);
            }

            catch(DaqException exception)
            {
                loopTimer.Enabled = false;
                //dispose task
                myTask.Dispose();
                MessageBox.Show(exception.Message);
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }

            catch (IndexOutOfRangeException exception)
            {
                loopTimer.Enabled = false;
                //dispose task
                myTask.Dispose();
                MessageBox.Show("Error: You must specify eight lines in the channel string (i.e., 0:7).");
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }
    }
}
