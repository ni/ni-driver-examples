/******************************************************************************
*
* Example program:
*   Meas2EdgeSeparation
*
* Category:
*   CI
*
* Description:
*   This example demonstrates how to measure two edge separation on a counter
*   input channel. The first edge, second edge, minimum value, and maximum value
*   are all configurable. This example measures two edge separation on the
*   counter's default input terminals (see I/O Connections Overview below for
*   more information), but could easily be expanded to measure two edge
*   separation on any PFI, RTSI, or internal signal. Refer to your device
*   documentation to see if your device supports two edge separation
*   measurements.
*
* Instructions for running:
*   1.  Select the physical channel which corresponds to the counter on the DAQ
*       device you want to perform a two edge separation measurement on.
*   2.  Enter the first edge and second edge corresponding to the two edges you
*       want the counter to 
*       measure.  Enter the maximum and minimum value to specify the range of your
*       unknown two edge separation.  
*       Additionally, you can change the first and second edge input terminals
*       using the channel property node.Note: 
*       It is important to set the maximum and minimum values of your unknown two
*       edge separation as accurately 
*       as possible so the best internal timebase can be chosen to minimize
*       measurement error.  The default values 
*       specify a range that can be measured by the counter using the 20MHz
*       timebase.
*
* Steps:
*   1.  Create a Task.
*   2.  Create a CIChannel object by using the CreateTwoEdgeSeparationChannel
*       method.
*   3.  Create a CounterReader object and use the ReadSingleSampleDouble method
*       to initiate the measurement and return the data.
*   4.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   5.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   This example will perform a measurement on the default terminal(s) of the
*   counter specified. The default counter terminal(s) depend on the type of
*   measurement being taken.  In this example the two edge separation will be
*   measured on the default input terminals on ctr0. For more information on the
*   default counter input and output terminals for your device, open the
*   NI-DAQmx Help, and refer to Counter Signal Connections found under the
*   Device Considerations book in the table of contents.
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

namespace NationalInstruments.Examples.Meas2EdgeSeparation
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private Task myTask;
		private CITwoEdgeSeparationFirstEdge firstEdge;
		private CITwoEdgeSeparationSecondEdge secondEdge;
		private CounterReader counterInReader;
		private System.Windows.Forms.GroupBox channelParametersGroupBox;
		private System.Windows.Forms.Label maximumLabel;
		private System.Windows.Forms.Label minimumLabel;
		private System.Windows.Forms.Label physicalChannelLabel;
		private System.Windows.Forms.TextBox maximumTextBox;
		private System.Windows.Forms.TextBox minimumTextBox;
		private System.Windows.Forms.GroupBox acqResultGroupBox;
		private System.Windows.Forms.Label resultLabel;
		private System.Windows.Forms.ComboBox firstEdgeComboBox;
		private System.Windows.Forms.Label firstEdgeLabel;
		private System.Windows.Forms.Label secondEdgeLabel;
		private System.Windows.Forms.ComboBox secondEdgeComboBox;
		private System.Windows.Forms.TextBox aquisitionDataTextBox;
		private System.Windows.Forms.Button measureButton;
        private System.Windows.Forms.ComboBox counterComboBox;
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
            firstEdgeComboBox.SelectedIndex = 0;
            secondEdgeComboBox.SelectedIndex = 1;

            counterComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CI, PhysicalChannelAccess.External));
            if (counterComboBox.Items.Count > 0)
                counterComboBox.SelectedIndex = 0;
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
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.counterComboBox = new System.Windows.Forms.ComboBox();
            this.secondEdgeLabel = new System.Windows.Forms.Label();
            this.secondEdgeComboBox = new System.Windows.Forms.ComboBox();
            this.firstEdgeLabel = new System.Windows.Forms.Label();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.maximumTextBox = new System.Windows.Forms.TextBox();
            this.minimumTextBox = new System.Windows.Forms.TextBox();
            this.firstEdgeComboBox = new System.Windows.Forms.ComboBox();
            this.acqResultGroupBox = new System.Windows.Forms.GroupBox();
            this.aquisitionDataTextBox = new System.Windows.Forms.TextBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.measureButton = new System.Windows.Forms.Button();
            this.channelParametersGroupBox.SuspendLayout();
            this.acqResultGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.counterComboBox);
            this.channelParametersGroupBox.Controls.Add(this.secondEdgeLabel);
            this.channelParametersGroupBox.Controls.Add(this.secondEdgeComboBox);
            this.channelParametersGroupBox.Controls.Add(this.firstEdgeLabel);
            this.channelParametersGroupBox.Controls.Add(this.maximumLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.maximumTextBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumTextBox);
            this.channelParametersGroupBox.Controls.Add(this.firstEdgeComboBox);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(248, 224);
            this.channelParametersGroupBox.TabIndex = 1;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters:";
            // 
            // counterComboBox
            // 
            this.counterComboBox.Location = new System.Drawing.Point(136, 24);
            this.counterComboBox.Name = "counterComboBox";
            this.counterComboBox.Size = new System.Drawing.Size(96, 21);
            this.counterComboBox.TabIndex = 1;
            this.counterComboBox.Text = "Dev1/ctr0";
            // 
            // secondEdgeLabel
            // 
            this.secondEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.secondEdgeLabel.Location = new System.Drawing.Point(12, 184);
            this.secondEdgeLabel.Name = "secondEdgeLabel";
            this.secondEdgeLabel.Size = new System.Drawing.Size(112, 16);
            this.secondEdgeLabel.TabIndex = 8;
            this.secondEdgeLabel.Text = "Second Edge:";
            // 
            // secondEdgeComboBox
            // 
            this.secondEdgeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.secondEdgeComboBox.Items.AddRange(new object[] {
                                                                    "Rising",
                                                                    "Falling"});
            this.secondEdgeComboBox.Location = new System.Drawing.Point(136, 184);
            this.secondEdgeComboBox.Name = "secondEdgeComboBox";
            this.secondEdgeComboBox.Size = new System.Drawing.Size(96, 21);
            this.secondEdgeComboBox.TabIndex = 9;
            // 
            // firstEdgeLabel
            // 
            this.firstEdgeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.firstEdgeLabel.Location = new System.Drawing.Point(12, 136);
            this.firstEdgeLabel.Name = "firstEdgeLabel";
            this.firstEdgeLabel.Size = new System.Drawing.Size(112, 16);
            this.firstEdgeLabel.TabIndex = 6;
            this.firstEdgeLabel.Text = "First Edge:";
            // 
            // maximumLabel
            // 
            this.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumLabel.Location = new System.Drawing.Point(12, 96);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(120, 16);
            this.maximumLabel.TabIndex = 4;
            this.maximumLabel.Text = "Maximum Value (sec):";
            // 
            // minimumLabel
            // 
            this.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumLabel.Location = new System.Drawing.Point(12, 62);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(120, 18);
            this.minimumLabel.TabIndex = 2;
            this.minimumLabel.Text = "Minimum Value (sec):";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(12, 26);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(96, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Counter:";
            // 
            // maximumTextBox
            // 
            this.maximumTextBox.Location = new System.Drawing.Point(136, 96);
            this.maximumTextBox.Name = "maximumTextBox";
            this.maximumTextBox.Size = new System.Drawing.Size(96, 20);
            this.maximumTextBox.TabIndex = 5;
            this.maximumTextBox.Text = "0.838860750";
            // 
            // minimumTextBox
            // 
            this.minimumTextBox.Location = new System.Drawing.Point(136, 60);
            this.minimumTextBox.Name = "minimumTextBox";
            this.minimumTextBox.Size = new System.Drawing.Size(96, 20);
            this.minimumTextBox.TabIndex = 3;
            this.minimumTextBox.Text = "0.000000100";
            // 
            // firstEdgeComboBox
            // 
            this.firstEdgeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.firstEdgeComboBox.Items.AddRange(new object[] {
                                                                   "Rising",
                                                                   "Falling"});
            this.firstEdgeComboBox.Location = new System.Drawing.Point(136, 136);
            this.firstEdgeComboBox.Name = "firstEdgeComboBox";
            this.firstEdgeComboBox.Size = new System.Drawing.Size(96, 21);
            this.firstEdgeComboBox.TabIndex = 7;
            // 
            // acqResultGroupBox
            // 
            this.acqResultGroupBox.Controls.Add(this.aquisitionDataTextBox);
            this.acqResultGroupBox.Controls.Add(this.resultLabel);
            this.acqResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acqResultGroupBox.Location = new System.Drawing.Point(264, 8);
            this.acqResultGroupBox.Name = "acqResultGroupBox";
            this.acqResultGroupBox.Size = new System.Drawing.Size(152, 96);
            this.acqResultGroupBox.TabIndex = 2;
            this.acqResultGroupBox.TabStop = false;
            this.acqResultGroupBox.Text = "Acquisition Results:";
            // 
            // aquisitionDataTextBox
            // 
            this.aquisitionDataTextBox.Location = new System.Drawing.Point(16, 56);
            this.aquisitionDataTextBox.Name = "aquisitionDataTextBox";
            this.aquisitionDataTextBox.ReadOnly = true;
            this.aquisitionDataTextBox.Size = new System.Drawing.Size(120, 20);
            this.aquisitionDataTextBox.TabIndex = 1;
            this.aquisitionDataTextBox.Text = "0.0";
            // 
            // resultLabel
            // 
            this.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resultLabel.Location = new System.Drawing.Point(16, 24);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(112, 32);
            this.resultLabel.TabIndex = 0;
            this.resultLabel.Text = "Measured Two Edge Separation (sec):";
            // 
            // measureButton
            // 
            this.measureButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.measureButton.Location = new System.Drawing.Point(280, 192);
            this.measureButton.Name = "measureButton";
            this.measureButton.Size = new System.Drawing.Size(128, 32);
            this.measureButton.TabIndex = 0;
            this.measureButton.Text = "Measure";
            this.measureButton.Click += new System.EventHandler(this.measureButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(426, 240);
            this.Controls.Add(this.measureButton);
            this.Controls.Add(this.acqResultGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Measure Two Edge Separation";
            this.channelParametersGroupBox.ResumeLayout(false);
            this.acqResultGroupBox.ResumeLayout(false);
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

		private void measureButton_Click(object sender, System.EventArgs e)
		{			
            // This example uses the default source (or gate) terminal for 
            // the counter of your device.  To determine what the default 
            // counter pins for your device are or to set a different source 
            // (or gate) pin, refer to the Connecting Counter Signals topic
            // in the NI-DAQmx Help (search for "Connecting Counter Signals").

			try
			{
				measureButton.Enabled = false;

				switch (firstEdgeComboBox.SelectedItem.ToString())
				{
					case "Rising":
						firstEdge = CITwoEdgeSeparationFirstEdge.Rising;
						break;
					case "Falling":
						firstEdge = CITwoEdgeSeparationFirstEdge.Falling;
						break;
				}

				switch (secondEdgeComboBox.SelectedItem.ToString())
				{
					case "Rising":
						secondEdge = CITwoEdgeSeparationSecondEdge.Rising;
						break;
					case "Falling":
						secondEdge = CITwoEdgeSeparationSecondEdge.Falling;
						break;
				}

				myTask= new Task();
				
				myTask.CIChannels.CreateTwoEdgeSeparationChannel(
                    counterComboBox.Text,"",
					Convert.ToDouble(minimumTextBox.Text),
					Convert.ToDouble(maximumTextBox.Text),
					firstEdge, secondEdge, CITwoEdgeSeparationUnits.Seconds);

				counterInReader = new CounterReader(myTask.Stream);
				double data = counterInReader.ReadSingleSampleDouble();

				aquisitionDataTextBox.Text = data.ToString();
			}
			catch(DaqException exception)
			{
				MessageBox.Show(exception.Message);
			}
			finally
			{
                myTask.Dispose();
				measureButton.Enabled = true;
			}
		}
	}
}
