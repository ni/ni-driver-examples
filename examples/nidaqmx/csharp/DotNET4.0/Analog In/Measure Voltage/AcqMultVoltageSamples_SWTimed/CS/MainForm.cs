/******************************************************************************
*
* Example program:
*   AcqMultVoltageSamples_SWTimed
*
* Category:
*   AI
*
* Description:
*   This example demonstrates how to acquire a finite amount of data using a
*   software timer.
*
* Instructions for running:
*   1.  Select the physical channel corresponding to where your signal is input
*       on the DAQ device.
*   2.  Enter the minimum and maximum voltage values.Note: For better accuracy,
*       try to match the input range to the expected voltage level of the
*       measured signal.
*   3.  Select the number of samples to acquire.
*   4.  Set the rate of the acquisition.Note: The rate should be at least twice
*       as fast as the maximum frequency component of the signal being acquired,
*       and will be limited by the maximum software clock rate of 10ms.
*
* Steps:
*   1.  Create a new task and an analog input voltage channel.
*   2.  Set the loop delay and enable the software timer.
*   3.  Read one point every time the timer event is called.
*   4.  Monitor the number of samples acquired and disable the timer once the
*       specified number of samples is reached.
*   5.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   6.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal input terminal matches the physical channel I/O
*   control. In the default case (differential channel ai0) wire the positive
*   lead for your signal to the ACH0 pin on your DAQ device and wire the
*   negative lead for your signal to the ACH8 pin on you DAQ device.  For more
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

namespace NationalInstruments.Examples.AcqMultVoltageSamples_SWTimed
{
    /// <summary>
    /// </summary>  
    public class MainForm : System.Windows.Forms.Form
    {        
        private Task myTask;                //Main Task variable which gets called in the Main Function
        private AnalogMultiChannelReader reader;
        private int totalSamples = 0;       //Global container for the number of samples to acquire
        private int acquiredSamplesCount = 0;    //Iteration variable which holds the number of samples currently acquired       
        
        private DataColumn[] dataColumn =null;
        private DataTable dataTable=null;

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label maximumLabel;
        private System.Windows.Forms.Label minimumLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.Timer loopTimer;
        private System.Windows.Forms.Label samplesLabel;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.GroupBox acquisitionResultGroupBox;
        private System.Windows.Forms.NumericUpDown numberOfSamplesNumeric;
        private System.Windows.Forms.NumericUpDown loopDelayNumeric;
        private System.Windows.Forms.DataGrid acquisitionDataGrid;
        internal System.Windows.Forms.NumericUpDown minimumValueNumeric;
        internal System.Windows.Forms.NumericUpDown maximumValueNumeric;
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
            dataTable = new DataTable();

            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
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
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.minimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.maximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.loopDelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.numberOfSamplesNumeric = new System.Windows.Forms.NumericUpDown();
            this.rateLabel = new System.Windows.Forms.Label();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.loopTimer = new System.Windows.Forms.Timer(this.components);
            this.acquisitionResultGroupBox = new System.Windows.Forms.GroupBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loopDelayNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfSamplesNumeric)).BeginInit();
            this.acquisitionResultGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.maximumLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueNumeric);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(232, 120);
            this.channelParametersGroupBox.TabIndex = 0;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(120, 24);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(96, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "Dev1/ai0";
            // 
            // minimumValueNumeric
            // 
            this.minimumValueNumeric.DecimalPlaces = 2;
            this.minimumValueNumeric.Location = new System.Drawing.Point(120, 56);
            this.minimumValueNumeric.Maximum = new System.Decimal(new int[] {
                                                                                10,
                                                                                0,
                                                                                0,
                                                                                0});
            this.minimumValueNumeric.Minimum = new System.Decimal(new int[] {
                                                                                10,
                                                                                0,
                                                                                0,
                                                                                -2147483648});
            this.minimumValueNumeric.Name = "minimumValueNumeric";
            this.minimumValueNumeric.Size = new System.Drawing.Size(96, 20);
            this.minimumValueNumeric.TabIndex = 3;
            this.minimumValueNumeric.Value = new System.Decimal(new int[] {
                                                                              100,
                                                                              0,
                                                                              0,
                                                                              -2147418112});
            // 
            // maximumLabel
            // 
            this.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumLabel.Location = new System.Drawing.Point(16, 90);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(96, 16);
            this.maximumLabel.TabIndex = 4;
            this.maximumLabel.Text = "Maximum (Volts):";
            // 
            // minimumLabel
            // 
            this.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumLabel.Location = new System.Drawing.Point(16, 58);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(96, 16);
            this.minimumLabel.TabIndex = 2;
            this.minimumLabel.Text = "Minimum (Volts):";
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 26);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(96, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
            // 
            // maximumValueNumeric
            // 
            this.maximumValueNumeric.DecimalPlaces = 2;
            this.maximumValueNumeric.Location = new System.Drawing.Point(120, 88);
            this.maximumValueNumeric.Maximum = new System.Decimal(new int[] {
                                                                                10,
                                                                                0,
                                                                                0,
                                                                                0});
            this.maximumValueNumeric.Minimum = new System.Decimal(new int[] {
                                                                                10,
                                                                                0,
                                                                                0,
                                                                                -2147483648});
            this.maximumValueNumeric.Name = "maximumValueNumeric";
            this.maximumValueNumeric.Size = new System.Drawing.Size(96, 20);
            this.maximumValueNumeric.TabIndex = 5;
            this.maximumValueNumeric.Value = new System.Decimal(new int[] {
                                                                              100,
                                                                              0,
                                                                              0,
                                                                              65536});
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.loopDelayNumeric);
            this.timingParametersGroupBox.Controls.Add(this.numberOfSamplesNumeric);
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesLabel);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 136);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(232, 100);
            this.timingParametersGroupBox.TabIndex = 3;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // loopDelayNumeric
            // 
            this.loopDelayNumeric.Location = new System.Drawing.Point(120, 24);
            this.loopDelayNumeric.Maximum = new System.Decimal(new int[] {
                                                                             10000,
                                                                             0,
                                                                             0,
                                                                             0});
            this.loopDelayNumeric.Minimum = new System.Decimal(new int[] {
                                                                             10,
                                                                             0,
                                                                             0,
                                                                             0});
            this.loopDelayNumeric.Name = "loopDelayNumeric";
            this.loopDelayNumeric.Size = new System.Drawing.Size(96, 20);
            this.loopDelayNumeric.TabIndex = 1;
            this.loopDelayNumeric.Value = new System.Decimal(new int[] {
                                                                           100,
                                                                           0,
                                                                           0,
                                                                           0});
            // 
            // numberOfSamplesNumeric
            // 
            this.numberOfSamplesNumeric.Location = new System.Drawing.Point(120, 64);
            this.numberOfSamplesNumeric.Maximum = new System.Decimal(new int[] {
                                                                                   1000000,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            this.numberOfSamplesNumeric.Minimum = new System.Decimal(new int[] {
                                                                                   1,
                                                                                   0,
                                                                                   0,
                                                                                   0});
            this.numberOfSamplesNumeric.Name = "numberOfSamplesNumeric";
            this.numberOfSamplesNumeric.Size = new System.Drawing.Size(96, 20);
            this.numberOfSamplesNumeric.TabIndex = 3;
            this.numberOfSamplesNumeric.Value = new System.Decimal(new int[] {
                                                                                 100,
                                                                                 0,
                                                                                 0,
                                                                                 0});
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(16, 24);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(96, 16);
            this.rateLabel.TabIndex = 0;
            this.rateLabel.Text = "Loop Delay (ms):";
            // 
            // samplesLabel
            // 
            this.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesLabel.Location = new System.Drawing.Point(16, 66);
            this.samplesLabel.Name = "samplesLabel";
            this.samplesLabel.Size = new System.Drawing.Size(112, 16);
            this.samplesLabel.TabIndex = 2;
            this.samplesLabel.Text = "Number of Samples:";
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(40, 392);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 24);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(136, 392);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(80, 24);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // loopTimer
            // 
            this.loopTimer.Interval = 10;
            this.loopTimer.Tick += new System.EventHandler(this.loopTimer_Tick);
            // 
            // acquisitionResultGroupBox
            // 
            this.acquisitionResultGroupBox.Controls.Add(this.resultLabel);
            this.acquisitionResultGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultGroupBox.Location = new System.Drawing.Point(8, 248);
            this.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox";
            this.acquisitionResultGroupBox.Size = new System.Drawing.Size(232, 136);
            this.acquisitionResultGroupBox.TabIndex = 4;
            this.acquisitionResultGroupBox.TabStop = false;
            this.acquisitionResultGroupBox.Text = "Acquisition Results";
            // 
            // resultLabel
            // 
            this.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resultLabel.Location = new System.Drawing.Point(16, 16);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(168, 16);
            this.resultLabel.TabIndex = 0;
            this.resultLabel.Text = "Acquisition Data (Volts):";
            // 
            // acquisitionDataGrid
            // 
            this.acquisitionDataGrid.AllowSorting = false;
            this.acquisitionDataGrid.DataMember = "";
            this.acquisitionDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.acquisitionDataGrid.Location = new System.Drawing.Point(16, 32);
            this.acquisitionDataGrid.Name = "acquisitionDataGrid";
            this.acquisitionDataGrid.ParentRowsVisible = false;
            this.acquisitionDataGrid.ReadOnly = true;
            this.acquisitionDataGrid.Size = new System.Drawing.Size(200, 96);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(250, 432);
            this.Controls.Add(this.acquisitionResultGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(256, 464);
            this.MinimumSize = new System.Drawing.Size(256, 336);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Acquire  Multiple Samples SW Timed";
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.loopDelayNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfSamplesNumeric)).EndInit();
            this.acquisitionResultGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).EndInit();
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
            startButton.Enabled = false;
            stopButton.Enabled = true;
            acquiredSamplesCount = 0;
            totalSamples = Convert.ToInt32(numberOfSamplesNumeric.Value);
            try
            {
                //Create a task that will be disposed after it has been used
                myTask = new Task();                

                //Create a virtual channel
                myTask.AIChannels.CreateVoltageChannel(physicalChannelComboBox.Text,"",
                    (AITerminalConfiguration)(-1),Convert.ToDouble(minimumValueNumeric.Value),
                    Convert.ToDouble(maximumValueNumeric.Value), AIVoltageUnits.Volts);

                //Verify the Task
                myTask.Control(TaskAction.Verify);
                
                //Initialize the table
                InitializeDataTable(myTask.AIChannels,ref dataTable); 
                acquisitionDataGrid.DataSource=dataTable;

                reader = new AnalogMultiChannelReader(myTask.Stream);
                
                //Set the loop delay and enable the timer
                loopTimer.Interval = Convert.ToInt32(loopDelayNumeric.Value);
                loopTimer.Enabled = true;
            }
            catch(DaqException exception)
            {
                //disable the timer and dispose of the task
                loopTimer.Enabled = false;
                myTask.Dispose();
                MessageBox.Show(exception.Message);
                startButton.Enabled = true;
                stopButton.Enabled = false;                 
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

        //This is where the data is being acquired
        private void loopTimer_Tick(object sender, System.EventArgs e)
        {
            if(acquiredSamplesCount < totalSamples)  
            {
                acquiredSamplesCount++;
                try
                {
                    //Read the data from the channels
                    double [] data = reader.ReadSingleSample(); 
                    
                    //Plot Multiple Channels to the table
                    dataToDataTable(data,ref dataTable);
                                        
                }
                catch(DaqException exception)
                {                   
                    //disable the timer and dispose of the task
                    loopTimer.Enabled = false;
                    myTask.Dispose();
                    startButton.Enabled = true;
                    stopButton.Enabled = false;                     
                    MessageBox.Show(exception.Message);
                }
            }
            else 
            {   //disable the timer and dispose of the task
                stopButton_Click(null, null);
            }
                
        }

        private void dataToDataTable(double[] sourceArray,ref DataTable dataTable)
        {   
            try
            {
                int channelCount = sourceArray.GetLength(0);
                int currentDataIndex = 0;
                for(int currentChannelIndex=0;currentChannelIndex<channelCount;currentChannelIndex++)
                    dataTable.Rows[currentDataIndex][currentChannelIndex] = sourceArray.GetValue(currentChannelIndex);     
                
            }
            catch(Exception e)
            {
                MessageBox.Show(e.TargetSite.ToString());
                //disable the timer and dispose of the task
                loopTimer.Enabled = false;
                myTask.Dispose();
                startButton.Enabled = true;
                stopButton.Enabled = false; 
            }
        }

        public void InitializeDataTable(AIChannelCollection channelCollection,ref DataTable data)
        {
            int numOfChannels= channelCollection.Count;
            data.Rows.Clear();
            data.Columns.Clear();
            dataColumn = new DataColumn[numOfChannels];
    
            for(int currentChannelIndex=0;currentChannelIndex<numOfChannels;currentChannelIndex++)
            {   
                dataColumn[currentChannelIndex] = new DataColumn();
                dataColumn[currentChannelIndex].DataType = typeof(double);
                dataColumn[currentChannelIndex].ColumnName=channelCollection[currentChannelIndex].PhysicalName;
            }

            data.Columns.AddRange(dataColumn); 

            object[] rowArr = new object[numOfChannels];
            data.Rows.Add(rowArr);              
        }
    }
}
