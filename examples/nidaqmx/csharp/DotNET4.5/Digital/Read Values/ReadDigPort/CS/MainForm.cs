/******************************************************************************
*
* Example program:
*   ReadDigPort
*
* Category:
*   DI
*
* Description:
*   This example demonstrates how to read a single value from a digital port.
*
* Instructions for running:
*   1.  Select the digital port on the DAQ device to be read.Note: The data read
*       indicator displays data in hexadecimal format.
*
* Steps:
*   1.  Create a new task and a DIChannel object by calling the CreateChannel
*       method.
*   2.  Create a DigitalSingleChannelReader object to read the data.
*   3.  Read a single sample of digital data from the port as an unsigned
*       integer.
*   4.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   5.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal input terminals match the port text box. For more
*   information on the input and output terminals for your device, open the
*   NI-DAQmx Help, and refer to the NI-DAQmx Device Terminals and Device
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

namespace NationalInstruments.Examples.ReadDigPort
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label channelParametersLabel;
        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.GroupBox dataGroupBox;
        private System.Windows.Forms.Label hexLabel;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.TextBox hexData;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        /// <summary>
        /// Required designer variable.
        /// </summary>
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
            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DIPort, PhysicalChannelAccess.External));
            if (physicalChannelComboBox.Items.Count > 0)
                physicalChannelComboBox.SelectedIndex = 0;
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.readButton = new System.Windows.Forms.Button();
            this.channelParametersLabel = new System.Windows.Forms.Label();
            this.hexData = new System.Windows.Forms.TextBox();
            this.dataGroupBox = new System.Windows.Forms.GroupBox();
            this.hexLabel = new System.Windows.Forms.Label();
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.dataGroupBox.SuspendLayout();
            this.channelParametersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // readButton
            // 
            this.readButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.readButton.Location = new System.Drawing.Point(45, 167);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(112, 24);
            this.readButton.TabIndex = 0;
            this.readButton.Text = "&Read";
            this.readButton.Click += new System.EventHandler(this.ReadButton_Click);
            // 
            // channelParametersLabel
            // 
            this.channelParametersLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersLabel.Location = new System.Drawing.Point(16, 24);
            this.channelParametersLabel.Name = "channelParametersLabel";
            this.channelParametersLabel.Size = new System.Drawing.Size(112, 16);
            this.channelParametersLabel.TabIndex = 0;
            this.channelParametersLabel.Text = "Physical Channel:";
            // 
            // hexData
            // 
            this.hexData.Location = new System.Drawing.Point(80, 24);
            this.hexData.Name = "hexData";
            this.hexData.ReadOnly = true;
            this.hexData.Size = new System.Drawing.Size(72, 20);
            this.hexData.TabIndex = 1;
            this.hexData.TabStop = false;
            this.hexData.Text = "0x0";
            // 
            // dataGroupBox
            // 
            this.dataGroupBox.Controls.Add(this.hexLabel);
            this.dataGroupBox.Controls.Add(this.hexData);
            this.dataGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataGroupBox.Location = new System.Drawing.Point(17, 95);
            this.dataGroupBox.Name = "dataGroupBox";
            this.dataGroupBox.Size = new System.Drawing.Size(168, 56);
            this.dataGroupBox.TabIndex = 2;
            this.dataGroupBox.TabStop = false;
            this.dataGroupBox.Text = "Data";
            // 
            // hexLabel
            // 
            this.hexLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.hexLabel.Location = new System.Drawing.Point(16, 24);
            this.hexLabel.Name = "hexLabel";
            this.hexLabel.Size = new System.Drawing.Size(64, 16);
            this.hexLabel.TabIndex = 0;
            this.hexLabel.Text = "Data Read:";
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.channelParametersLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(17, 15);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(168, 72);
            this.channelParametersGroupBox.TabIndex = 1;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(16, 40);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(136, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/port0";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(202, 207);
            this.Controls.Add(this.channelParametersGroupBox);
            this.Controls.Add(this.dataGroupBox);
            this.Controls.Add(this.readButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(208, 192);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Read Digital Port";
            this.dataGroupBox.ResumeLayout(false);
            this.channelParametersGroupBox.ResumeLayout(false);
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

        private void ReadButton_Click(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

			try
			{
				using (Task digitalReadTask = new Task())
				{
					digitalReadTask.DIChannels.CreateChannel(
					    physicalChannelComboBox.Text,
						"port0",
						ChannelLineGrouping.OneChannelForAllLines);

					DigitalSingleChannelReader reader = new DigitalSingleChannelReader(digitalReadTask.Stream);
					UInt32 data = reader.ReadSingleSamplePortUInt32();
                
					//Update the Data Read box
					hexData.Text = String.Format("0x{0:X}", data);
                }
			}
			catch (DaqException ex)
			{
				MessageBox.Show(ex.Message);               
			}
			finally 
			{
			    Cursor.Current = Cursors.Default;
			}
        }
    }
}
