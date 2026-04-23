/******************************************************************************
*
* Example program:
*   AcqPowerSamples_IntClk
*
* Category:
*   Power
*
* Description:
*   This example demonstrates how to acquire a finite amount of data using
*   the DAQ device's internal clock.
*
* Instructions for running:
*   1.  Select the physical channel(s) corresponding to your devices'
*       connections.
*   2.  Enter the voltage setpoint, current setpoint, and output enable
*       settings.
*   3.  Set the rate and number of samples of the acquisition.
*   4.  Start the acquisition.
*
* Steps:
*   1.  Create a new task.
*   2.  Create an analog input power channel.
*   3.  Set up the timing for the acquisition. In this example, we use the DAQ
*       device's internal clock to acquire samples.
*   4.  Call AnalogMultiChannelReader.ReadMultiSamplePower. Note: If the output
*       is disabled, the data read will be NaN.
*   5.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   6.  Handle any DaqExceptions, if they occur.
*
* Microsoft Windows User Account Control
*   Running certain applications on Microsoft Windows requires administrator
*   privileges, because the application name contains keywords such as setup,
*   update, or install. To avoid this problem, you must add an additional
*   manifest to the application that specifies the privileges required to run
*   the application. Some NI-DAQmx examples for Visual Studio include these
*   keywords. Therefore, all examples for Visual Studio are shipped with an
*   additional manifest file that you must embed in the example executable. The
*   manifest file is named [ExampleName].exe.manifest, where [ExampleName] is
*   the NI-provided example name. For information on how to embed the manifest
*   file, refer to http://msdn2.microsoft.com/en-us/library/bb756929.aspx.
******************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using NationalInstruments.DAQmx;

namespace NationalInstruments.Examples.AcqPowerSamples_IntClk
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {        
        private Task myTask; 
        private AnalogMultiChannelReader reader; 
        private AIPowerMeasurement[,] data;
        private DataColumn[] dataColumn = null;
        private DataTable dataTable = null;

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label maximumLabel;
        private System.Windows.Forms.Label minimumLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.Label samplesLabel;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.GroupBox acquisitionResultGroupBox;
        private System.Windows.Forms.NumericUpDown samplesPerChannelNumeric;
        private System.Windows.Forms.DataGrid acquisitionDataGrid;
        private System.Windows.Forms.NumericUpDown rateNumeric;
        internal System.Windows.Forms.NumericUpDown voltageSetpointNumeric;
        internal System.Windows.Forms.NumericUpDown currentSetpointNumeric;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.Windows.Forms.CheckBox outputEnableCheckBox;

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
            dataTable= new DataTable();

            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.Power, PhysicalChannelAccess.External));
            if (physicalChannelComboBox.Items.Count > 0)
                physicalChannelComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if(disposing)
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
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.outputEnableCheckBox = new System.Windows.Forms.CheckBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.voltageSetpointNumeric = new System.Windows.Forms.NumericUpDown();
            this.currentSetpointNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.rateLabel = new System.Windows.Forms.Label();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.samplesPerChannelNumeric = new System.Windows.Forms.NumericUpDown();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.startButton = new System.Windows.Forms.Button();
            this.acquisitionResultGroupBox = new System.Windows.Forms.GroupBox();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.voltageSetpointNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentSetpointNumeric)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            this.acquisitionResultGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.outputEnableCheckBox);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.voltageSetpointNumeric);
            this.channelParametersGroupBox.Controls.Add(this.currentSetpointNumeric);
            this.channelParametersGroupBox.Controls.Add(this.maximumLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 12);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(256, 146);
            this.channelParametersGroupBox.TabIndex = 0;
            this.channelParametersGroupBox.TabStop = false;
            this.channelParametersGroupBox.Text = "Channel Parameters";
            // 
            // outputEnableCheckBox
            // 
            this.outputEnableCheckBox.AutoSize = true;
            this.outputEnableCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.outputEnableCheckBox.Location = new System.Drawing.Point(73, 112);
            this.outputEnableCheckBox.Name = "outputEnableCheckBox";
            this.outputEnableCheckBox.Size = new System.Drawing.Size(94, 17);
            this.outputEnableCheckBox.TabIndex = 6;
            this.outputEnableCheckBox.Text = "Output Enable";
            this.outputEnableCheckBox.UseVisualStyleBackColor = true;
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(120, 24);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(130, 21);
            this.physicalChannelComboBox.TabIndex = 1;
            this.physicalChannelComboBox.Text = "TS1Mod1/power";
            // 
            // voltageSetpointNumeric
            // 
            this.voltageSetpointNumeric.DecimalPlaces = 2;
            this.voltageSetpointNumeric.Location = new System.Drawing.Point(154, 56);
            this.voltageSetpointNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.voltageSetpointNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.voltageSetpointNumeric.Name = "voltageSetpointNumeric";
            this.voltageSetpointNumeric.Size = new System.Drawing.Size(96, 20);
            this.voltageSetpointNumeric.TabIndex = 3;
            this.voltageSetpointNumeric.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // currentSetpointNumeric
            // 
            this.currentSetpointNumeric.DecimalPlaces = 2;
            this.currentSetpointNumeric.Location = new System.Drawing.Point(154, 84);
            this.currentSetpointNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.currentSetpointNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.currentSetpointNumeric.Name = "currentSetpointNumeric";
            this.currentSetpointNumeric.Size = new System.Drawing.Size(96, 20);
            this.currentSetpointNumeric.TabIndex = 5;
            this.currentSetpointNumeric.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // maximumLabel
            // 
            this.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumLabel.Location = new System.Drawing.Point(16, 85);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(132, 18);
            this.maximumLabel.TabIndex = 4;
            this.maximumLabel.Text = "Current Setpoint (Amperes):";
            this.maximumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // minimumLabel
            // 
            this.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumLabel.Location = new System.Drawing.Point(16, 56);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(132, 20);
            this.minimumLabel.TabIndex = 2;
            this.minimumLabel.Text = "Voltage Setpoint (Volts):";
            this.minimumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // physicalChannelLabel
            // 
            this.physicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.physicalChannelLabel.Location = new System.Drawing.Point(16, 24);
            this.physicalChannelLabel.Name = "physicalChannelLabel";
            this.physicalChannelLabel.Size = new System.Drawing.Size(96, 16);
            this.physicalChannelLabel.TabIndex = 0;
            this.physicalChannelLabel.Text = "Physical Channel:";
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelNumeric);
            this.timingParametersGroupBox.Controls.Add(this.rateNumeric);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 164);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(256, 88);
            this.timingParametersGroupBox.TabIndex = 1;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(16, 56);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(64, 16);
            this.rateLabel.TabIndex = 2;
            this.rateLabel.Text = "Rate (Hz):";
            // 
            // samplesLabel
            // 
            this.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesLabel.Location = new System.Drawing.Point(16, 24);
            this.samplesLabel.Name = "samplesLabel";
            this.samplesLabel.Size = new System.Drawing.Size(104, 16);
            this.samplesLabel.TabIndex = 0;
            this.samplesLabel.Text = "Samples / Channel:";
            // 
            // samplesPerChannelNumeric
            // 
            this.samplesPerChannelNumeric.Location = new System.Drawing.Point(154, 24);
            this.samplesPerChannelNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.samplesPerChannelNumeric.Name = "samplesPerChannelNumeric";
            this.samplesPerChannelNumeric.Size = new System.Drawing.Size(96, 20);
            this.samplesPerChannelNumeric.TabIndex = 1;
            this.samplesPerChannelNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // rateNumeric
            // 
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(154, 56);
            this.rateNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.rateNumeric.Name = "rateNumeric";
            this.rateNumeric.Size = new System.Drawing.Size(96, 20);
            this.rateNumeric.TabIndex = 3;
            this.rateNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(96, 258);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 24);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // acquisitionResultGroupBox
            // 
            this.acquisitionResultGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultGroupBox.Location = new System.Drawing.Point(270, 12);
            this.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox";
            this.acquisitionResultGroupBox.Size = new System.Drawing.Size(402, 276);
            this.acquisitionResultGroupBox.TabIndex = 3;
            this.acquisitionResultGroupBox.TabStop = false;
            this.acquisitionResultGroupBox.Text = "Acquisition Results";
            // 
            // acquisitionDataGrid
            // 
            this.acquisitionDataGrid.AllowSorting = false;
            this.acquisitionDataGrid.DataMember = "";
            this.acquisitionDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.acquisitionDataGrid.Location = new System.Drawing.Point(8, 24);
            this.acquisitionDataGrid.Name = "acquisitionDataGrid";
            this.acquisitionDataGrid.ParentRowsVisible = false;
            this.acquisitionDataGrid.PreferredColumnWidth = 120;
            this.acquisitionDataGrid.ReadOnly = true;
            this.acquisitionDataGrid.Size = new System.Drawing.Size(388, 248);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(684, 296);
            this.Controls.Add(this.acquisitionResultGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 336);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Acquire Power Samples - Internal Clock";
            this.channelParametersGroupBox.ResumeLayout(false);
            this.channelParametersGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.voltageSetpointNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentSetpointNumeric)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
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
           
            try
            {
                // Create a new task
                myTask = new Task();
                
                // Initialize local variables
                int samplesPerChannel = Convert.ToInt32(samplesPerChannelNumeric.Value);

                // Create a channel
                myTask.AIChannels.CreatePowerChannel(physicalChannelComboBox.Text, "",
                    Convert.ToDouble(voltageSetpointNumeric.Value),
                    Convert.ToDouble(currentSetpointNumeric.Value),
                    outputEnableCheckBox.Checked);

                // Configure timing specs    
                myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value),
                    SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples,
                    samplesPerChannel);

                // Verify the task
                myTask.Control(TaskAction.Verify);

                // Prepare the table for data
                InitializeDataTable(myTask.AIChannels, ref dataTable);   
                acquisitionDataGrid.DataSource = dataTable;

                // Read the data
                reader = new AnalogMultiChannelReader(myTask.Stream); 

                data = reader.ReadMultiSamplePower(samplesPerChannel);

                dataToDataTable(data, ref dataTable);
            }
            catch (DaqException exception)
            {
                MessageBox.Show(exception.Message);    
            }
            finally
            {
                myTask.Dispose();
                startButton.Enabled = true;
            }
        }

        private void dataToDataTable(AIPowerMeasurement[,] sourceArray, ref DataTable dataTable)
        {
            // Iterate over channels
            for (int channel = 0; channel < sourceArray.GetLength(0); ++channel)
            {
                // Iterate over samples
                for (int sample = 0; sample < sourceArray.GetLength(1); ++sample)
                {
                    if (sample == 10)
                        break;

                    AIPowerMeasurement data = sourceArray[channel, sample];
                    dataTable.Rows[sample][channel*2] = data.Voltage;
                    dataTable.Rows[sample][channel*2 + 1] = data.Current;
                }
            }
        }

        public void InitializeDataTable(AIChannelCollection channelCollection, ref DataTable data)
        {
            int numOfChannels = channelCollection.Count;
            data.Rows.Clear();
            data.Columns.Clear();
            dataColumn = new DataColumn[numOfChannels*2];
            int numOfRows = 10;

            for (int channel = 0; channel < numOfChannels; channel++)
            {
                int currentColumnIndex = channel * 2;
                dataColumn[currentColumnIndex] = new DataColumn();
                dataColumn[currentColumnIndex].DataType = typeof(double);
                dataColumn[currentColumnIndex].ColumnName = channelCollection[channel].PhysicalName + " (V)";

                currentColumnIndex = channel * 2 + 1;
                dataColumn[currentColumnIndex] = new DataColumn();
                dataColumn[currentColumnIndex].DataType = typeof(double);
                dataColumn[currentColumnIndex].ColumnName = channelCollection[channel].PhysicalName + " (A)";
            }

            data.Columns.AddRange(dataColumn); 

            for (int currentDataIndex = 0; currentDataIndex < numOfRows; currentDataIndex++)             
            {
                object[] rowArr = new object[numOfChannels];
                data.Rows.Add(rowArr);              
            }
        }

    }
}
