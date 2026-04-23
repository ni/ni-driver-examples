/******************************************************************************
*
* Example program:
*   Gen0_20mACurrent
*
* Category:
*   AO
*
* Description:
*   This example demonstrates how to generate a single current value on a single
*   current output channel of a SCXI-1124 module and NI-6238/6239 M-Series
*   devices.
*
* Instructions for running:
*   1.  Select the physical channel corresponding to where your signal is to be
*       generated on the SCXI-1124 or the NI-6238/6239 M-Series devices.
*   2.  Enter the minimum and maximum current values, in amps.
*   3.  Enter a current value to generate, in amps.
*
* Steps:
*   1.  Create a new Task object.  Use the CreateCurrentChannel method to create
*       an AO channel for current output.
*   2.  Create a AnalogSingleChannelWriter object and use the WriteSingleSample
*       method to generate the current value at the output channel. The
*       autoStart parameter of the WriteSingleSample method is set to true, so
*       that the task is automatically started when the method is called.
*   3.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   4.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   The SCXI-1124 can operate on either an external or internal current source.
*   The only 
*   difference is in the signal connections. When using the internal current
*   source, connect a load between 
*   the SUPPLY and ISINK terminals. When using an external current source, connect
*   the source and load to 
*   the ISINK and GND terminals. In either case, be sure that the channel numbers
*   of the terminals used match 
*   the channel numbers specified in the Physical Channel text box.  For more
*   information on the input and 
*   output terminals for your device, open the NI-DAQmx Help, and refer to the
*   NI-DAQmx Device Terminals 
*   and Device Considerations books in the table of contents. Note: When using an
*   external current source, 
*   be careful to avoid creating an uncontrolled current loop. See the device
*   User's Manual for more information.  
*   The output current can be measured by connecting an ammeter in series with the
*   current loop. Alternatively, 
*   the current can be measured by replacing the load with a resistor of known
*   value. By measuring the voltage 
*   across the resistor and dividing by the resistance, the current through the
*   resistor can be calculated 
*   (Ohm's law).
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

namespace NationalInstruments.Examples.Gen0_20mACurrent
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox channelParaGroupBox;
        private System.Windows.Forms.Label physicalChanLabel;
        private System.Windows.Forms.Label maxValueLabel;
        private System.Windows.Forms.Label minValueLabel;
        private System.Windows.Forms.Label currentValueLabel;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.TextBox minValue;
        private System.Windows.Forms.TextBox maxValue;
        private System.Windows.Forms.TextBox currentValue;
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
            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AO, PhysicalChannelAccess.External));
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.channelParaGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.physicalChanLabel = new System.Windows.Forms.Label();
            this.maxValueLabel = new System.Windows.Forms.Label();
            this.minValueLabel = new System.Windows.Forms.Label();
            this.minValue = new System.Windows.Forms.TextBox();
            this.maxValue = new System.Windows.Forms.TextBox();
            this.currentValue = new System.Windows.Forms.TextBox();
            this.currentValueLabel = new System.Windows.Forms.Label();
            this.generateButton = new System.Windows.Forms.Button();
            this.channelParaGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelParaGroupBox
            // 
            this.channelParaGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParaGroupBox.Controls.Add(this.physicalChanLabel);
            this.channelParaGroupBox.Controls.Add(this.maxValueLabel);
            this.channelParaGroupBox.Controls.Add(this.minValueLabel);
            this.channelParaGroupBox.Controls.Add(this.minValue);
            this.channelParaGroupBox.Controls.Add(this.maxValue);
            this.channelParaGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParaGroupBox.Location = new System.Drawing.Point(16, 8);
            this.channelParaGroupBox.Name = "channelParaGroupBox";
            this.channelParaGroupBox.Size = new System.Drawing.Size(192, 176);
            this.channelParaGroupBox.TabIndex = 1;
            this.channelParaGroupBox.TabStop = false;
            this.channelParaGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(16, 40);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(160, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "SC1Mod1/ao0";
            // 
            // physicalChanLabel
            // 
            this.physicalChanLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChanLabel.Location = new System.Drawing.Point(16, 24);
            this.physicalChanLabel.Name = "physicalChanLabel";
            this.physicalChanLabel.Size = new System.Drawing.Size(100, 16);
            this.physicalChanLabel.TabIndex = 0;
            this.physicalChanLabel.Text = "Physical Channel:";
            // 
            // maxValueLabel
            // 
            this.maxValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maxValueLabel.Location = new System.Drawing.Point(16, 72);
            this.maxValueLabel.Name = "maxValueLabel";
            this.maxValueLabel.Size = new System.Drawing.Size(112, 16);
            this.maxValueLabel.TabIndex = 2;
            this.maxValueLabel.Text = "Maximum Value (A):";
            // 
            // minValueLabel
            // 
            this.minValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minValueLabel.Location = new System.Drawing.Point(16, 120);
            this.minValueLabel.Name = "minValueLabel";
            this.minValueLabel.Size = new System.Drawing.Size(112, 16);
            this.minValueLabel.TabIndex = 4;
            this.minValueLabel.Text = "Minimum Value (A):";
            // 
            // minValue
            // 
            this.minValue.Location = new System.Drawing.Point(16, 136);
            this.minValue.Name = "minValue";
            this.minValue.Size = new System.Drawing.Size(160, 20);
            this.minValue.TabIndex = 5;
            this.minValue.Text = "0.00";
            // 
            // maxValue
            // 
            this.maxValue.Location = new System.Drawing.Point(16, 88);
            this.maxValue.Name = "maxValue";
            this.maxValue.Size = new System.Drawing.Size(160, 20);
            this.maxValue.TabIndex = 3;
            this.maxValue.Text = "0.020";
            // 
            // currentValue
            // 
            this.currentValue.Location = new System.Drawing.Point(224, 48);
            this.currentValue.Name = "currentValue";
            this.currentValue.Size = new System.Drawing.Size(160, 20);
            this.currentValue.TabIndex = 3;
            this.currentValue.Text = "0.020";
            // 
            // currentValueLabel
            // 
            this.currentValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.currentValueLabel.Location = new System.Drawing.Point(224, 32);
            this.currentValueLabel.Name = "currentValueLabel";
            this.currentValueLabel.Size = new System.Drawing.Size(100, 16);
            this.currentValueLabel.TabIndex = 2;
            this.currentValueLabel.Text = "Current Value (A):";
            // 
            // generateButton
            // 
            this.generateButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.generateButton.Location = new System.Drawing.Point(248, 144);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(120, 24);
            this.generateButton.TabIndex = 0;
            this.generateButton.Text = "&Generate Current";
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(402, 199);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.currentValueLabel);
            this.Controls.Add(this.currentValue);
            this.Controls.Add(this.channelParaGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate Current";
            this.channelParaGroupBox.ResumeLayout(false);
            this.channelParaGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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

        private void generateButton_Click(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                using(Task myTask = new Task())
                {
                    myTask.AOChannels.CreateCurrentChannel(
                        physicalChannelComboBox.Text,
                        "GenCurrent",
                        Convert.ToDouble(minValue.Text),
                        Convert.ToDouble(maxValue.Text),
                        AOCurrentUnits.Amps);
                
                    AnalogSingleChannelWriter myChannelWriter = new AnalogSingleChannelWriter(myTask.Stream);
                    myChannelWriter.WriteSingleSample(true,Convert.ToDouble(currentValue.Text));
                }
            }
            catch(DaqException exception)
            {
                MessageBox.Show(exception.Message);
            }
            Cursor.Current = Cursors.Default;
        }
    }
}
