/******************************************************************************
*
* Example program:
*   ContAcqVoltageSamples_IntClk_ToFile
*
* Category:
*   AI
*
* Description:
*   This example demonstrates how to acquire, write to file, and load from disk
*   a continuous amount of analog input data using the DAQ device's internal
*   clock.
*
* Instructions for running:
*   1.  Select the physical channels corresponding to where your signals are
*       input on the DAQ device.
*   2.  Enter the minimum and maximum voltage range.Note: For better accuracy,
*       try to match the input range to the expected voltage levels of the
*       measured signals.
*   3.  Set the rate of the acquisition and number of samples.
*   4.  Choose an output file format, either text or binary.
*   5.  Select the output filename.
*   6.  Start the acquisition.
*   7.  Select the file format of the file you want to load data from, either
*       text or binary.
*   8.  Select the input filename.
*   9.  Click the Read button to read the data from disk and display it.
*
* Steps:
*   1.  Create a new analog input task.
*   2.  Create the analog input voltage channels.
*   3.  Configure the timing for the acquisition.  In this example we use the
*       DAQ device's internal clock to take a continuous number of samples.
*   4.  Open the output file for writing.
*   5.  Create a AnalogMultiChannelReader and associate it with the task by
*       using the task's stream. Call
*       AnalogMultiChannelReader.BeginBeginReadMultiSample to install a callback
*       and begin the asynchronous read operation.
*   6.  Inside the callback, call AnalogMultiChannelReader.EndReadMultiSample to
*       retrieve the data from the read operation.  
*   7.  Call AnalogMultiChannelReader.BeginBeginReadMultiSample again inside the
*       callback to perform another read operation.
*   8.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   9.  Close the output file.
*   10. Open the input file for reading.
*   11. Read and display the data.
*   12. Handle any DaqExceptions, if they occur.
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
*   Make sure your signal input terminals match the physical I/O control.  In
*   the default case (differential channel ai0), wire the positive lead for your
*   signal to the ACH0 pin on your DAQ device and wire the negative lead for
*   your signal to the ACH8 pin.  For more information on the input and output
*   terminals for your device, open the NI-DAQmx Help and refer to the NI-DAQmx
*   Device Terminals and Device Considerations books in the table of contents.
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
using System.IO;

using NationalInstruments.DAQmx;

namespace NationalInstruments.Examples.ContAcqVoltageSamples_IntClk_ToFile
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private Task myTask;
        private AnalogMultiChannelReader analogInReader;
        private Task runningTask;
        private AsyncCallback analogCallback;
        private double[,] data;
        private DataColumn[] dataColumn = null;
        private DataTable dataTable = null;
        private ArrayList savedData;
        private StreamWriter fileStreamWriter;
        private BinaryWriter fileBinaryWriter;
        private StreamReader fileStreamReader;
        private BinaryReader fileBinaryReader;
        private string fileNameWrite;
        private string fileNameRead;
        private bool useTextFileWrite;
        private bool useTextFileRead;
        private System.Windows.Forms.GroupBox channelParametersGroupBox;
        private System.Windows.Forms.Label maximumLabel;
        private System.Windows.Forms.Label minimumLabel;
        private System.Windows.Forms.Label physicalChannelLabel;
        private System.Windows.Forms.Label rateLabel;       
        private System.Windows.Forms.Label samplesLabel;
        private System.Windows.Forms.Label resultLabel;

        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.GroupBox acquisitionResultGroupBox;
        private System.Windows.Forms.DataGrid acquisitionDataGrid;
        private System.Windows.Forms.NumericUpDown rateNumeric;
        private System.Windows.Forms.NumericUpDown samplesPerChannelNumeric;
        private System.Windows.Forms.NumericUpDown minimumValueNumeric;
        private System.Windows.Forms.NumericUpDown maximumValueNumeric;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;
        private System.Windows.Forms.SaveFileDialog writeToFileSaveFileDialog;
        private System.Windows.Forms.OpenFileDialog readFromFileOpenFileDialog;
        private System.Windows.Forms.GroupBox writeToFileGroupBox;
        private System.Windows.Forms.ToolTip fileToolTip;
        private System.Windows.Forms.TextBox filePathWriteTextBox;
        private System.Windows.Forms.Button browseWriteButton;
        private System.Windows.Forms.Label filePathWriteLabel;
        private System.Windows.Forms.RadioButton binaryFileWriteRadioButton;
        private System.Windows.Forms.RadioButton textFileWriteRadioButton;
        private System.Windows.Forms.Label fileTypeWriteLabel;
        private System.Windows.Forms.GroupBox readFromFileGroupBox;
        private System.Windows.Forms.Button browseReadButton;
        private System.Windows.Forms.Label filePathReadLabel;
        private System.Windows.Forms.RadioButton binaryFileReadRadioButton;
        private System.Windows.Forms.RadioButton textFileReadRadioButton;
        private System.Windows.Forms.TextBox filePathReadTextBox;
        private System.Windows.Forms.Label fileTypeReadLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button readButton;
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
            stopButton.Enabled = false;
            dataTable= new DataTable();

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.channelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.minimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.maximumLabel = new System.Windows.Forms.Label();
            this.minimumLabel = new System.Windows.Forms.Label();
            this.physicalChannelLabel = new System.Windows.Forms.Label();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.rateNumeric = new System.Windows.Forms.NumericUpDown();
            this.samplesLabel = new System.Windows.Forms.Label();
            this.rateLabel = new System.Windows.Forms.Label();
            this.samplesPerChannelNumeric = new System.Windows.Forms.NumericUpDown();
            this.acquisitionResultGroupBox = new System.Windows.Forms.GroupBox();
            this.resultLabel = new System.Windows.Forms.Label();
            this.acquisitionDataGrid = new System.Windows.Forms.DataGrid();
            this.filePathWriteTextBox = new System.Windows.Forms.TextBox();
            this.writeToFileSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.readFromFileOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.fileToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.writeToFileGroupBox = new System.Windows.Forms.GroupBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.browseWriteButton = new System.Windows.Forms.Button();
            this.filePathWriteLabel = new System.Windows.Forms.Label();
            this.binaryFileWriteRadioButton = new System.Windows.Forms.RadioButton();
            this.textFileWriteRadioButton = new System.Windows.Forms.RadioButton();
            this.fileTypeWriteLabel = new System.Windows.Forms.Label();
            this.readFromFileGroupBox = new System.Windows.Forms.GroupBox();
            this.browseReadButton = new System.Windows.Forms.Button();
            this.filePathReadLabel = new System.Windows.Forms.Label();
            this.binaryFileReadRadioButton = new System.Windows.Forms.RadioButton();
            this.textFileReadRadioButton = new System.Windows.Forms.RadioButton();
            this.filePathReadTextBox = new System.Windows.Forms.TextBox();
            this.fileTypeReadLabel = new System.Windows.Forms.Label();
            this.readButton = new System.Windows.Forms.Button();
            this.channelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).BeginInit();
            this.acquisitionResultGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).BeginInit();
            this.writeToFileGroupBox.SuspendLayout();
            this.readFromFileGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelParametersGroupBox
            // 
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelComboBox);
            this.channelParametersGroupBox.Controls.Add(this.minimumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.maximumValueNumeric);
            this.channelParametersGroupBox.Controls.Add(this.maximumLabel);
            this.channelParametersGroupBox.Controls.Add(this.minimumLabel);
            this.channelParametersGroupBox.Controls.Add(this.physicalChannelLabel);
            this.channelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParametersGroupBox.Location = new System.Drawing.Point(8, 8);
            this.channelParametersGroupBox.Name = "channelParametersGroupBox";
            this.channelParametersGroupBox.Size = new System.Drawing.Size(224, 120);
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
            this.minimumValueNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.minimumValueNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.minimumValueNumeric.Name = "minimumValueNumeric";
            this.minimumValueNumeric.Size = new System.Drawing.Size(96, 20);
            this.minimumValueNumeric.TabIndex = 3;
            this.minimumValueNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            -2147418112});
            // 
            // maximumValueNumeric
            // 
            this.maximumValueNumeric.DecimalPlaces = 2;
            this.maximumValueNumeric.Location = new System.Drawing.Point(120, 88);
            this.maximumValueNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.maximumValueNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.maximumValueNumeric.Name = "maximumValueNumeric";
            this.maximumValueNumeric.Size = new System.Drawing.Size(96, 20);
            this.maximumValueNumeric.TabIndex = 5;
            this.maximumValueNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            // 
            // maximumLabel
            // 
            this.maximumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.maximumLabel.Location = new System.Drawing.Point(16, 88);
            this.maximumLabel.Name = "maximumLabel";
            this.maximumLabel.Size = new System.Drawing.Size(112, 16);
            this.maximumLabel.TabIndex = 4;
            this.maximumLabel.Text = "Maximum Value (V):";
            // 
            // minimumLabel
            // 
            this.minimumLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.minimumLabel.Location = new System.Drawing.Point(16, 56);
            this.minimumLabel.Name = "minimumLabel";
            this.minimumLabel.Size = new System.Drawing.Size(104, 15);
            this.minimumLabel.TabIndex = 2;
            this.minimumLabel.Text = "Minimum Value (V):";
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
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.rateNumeric);
            this.timingParametersGroupBox.Controls.Add(this.samplesLabel);
            this.timingParametersGroupBox.Controls.Add(this.rateLabel);
            this.timingParametersGroupBox.Controls.Add(this.samplesPerChannelNumeric);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(8, 140);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(224, 92);
            this.timingParametersGroupBox.TabIndex = 1;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // rateNumeric
            // 
            this.rateNumeric.DecimalPlaces = 2;
            this.rateNumeric.Location = new System.Drawing.Point(120, 56);
            this.rateNumeric.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.rateNumeric.Name = "rateNumeric";
            this.rateNumeric.Size = new System.Drawing.Size(96, 20);
            this.rateNumeric.TabIndex = 3;
            this.rateNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // samplesLabel
            // 
            this.samplesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.samplesLabel.Location = new System.Drawing.Point(16, 26);
            this.samplesLabel.Name = "samplesLabel";
            this.samplesLabel.Size = new System.Drawing.Size(104, 16);
            this.samplesLabel.TabIndex = 0;
            this.samplesLabel.Text = "Samples/Channel:";
            // 
            // rateLabel
            // 
            this.rateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rateLabel.Location = new System.Drawing.Point(16, 58);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(56, 16);
            this.rateLabel.TabIndex = 2;
            this.rateLabel.Text = "Rate (Hz):";
            // 
            // samplesPerChannelNumeric
            // 
            this.samplesPerChannelNumeric.Location = new System.Drawing.Point(120, 24);
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
            // acquisitionResultGroupBox
            // 
            this.acquisitionResultGroupBox.Controls.Add(this.resultLabel);
            this.acquisitionResultGroupBox.Controls.Add(this.acquisitionDataGrid);
            this.acquisitionResultGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.acquisitionResultGroupBox.Location = new System.Drawing.Point(240, 8);
            this.acquisitionResultGroupBox.Name = "acquisitionResultGroupBox";
            this.acquisitionResultGroupBox.Size = new System.Drawing.Size(304, 224);
            this.acquisitionResultGroupBox.TabIndex = 3;
            this.acquisitionResultGroupBox.TabStop = false;
            this.acquisitionResultGroupBox.Text = "Acquisition Results";
            // 
            // resultLabel
            // 
            this.resultLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.resultLabel.Location = new System.Drawing.Point(8, 16);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(112, 16);
            this.resultLabel.TabIndex = 0;
            this.resultLabel.Text = "Acquisition Data (V):";
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
            this.acquisitionDataGrid.Size = new System.Drawing.Size(280, 184);
            this.acquisitionDataGrid.TabIndex = 1;
            this.acquisitionDataGrid.TabStop = false;
            // 
            // filePathWriteTextBox
            // 
            this.filePathWriteTextBox.Location = new System.Drawing.Point(120, 57);
            this.filePathWriteTextBox.Name = "filePathWriteTextBox";
            this.filePathWriteTextBox.ReadOnly = true;
            this.filePathWriteTextBox.Size = new System.Drawing.Size(384, 20);
            this.filePathWriteTextBox.TabIndex = 4;
            this.filePathWriteTextBox.Text = "Choose file location";
            // 
            // writeToFileSaveFileDialog
            // 
            this.writeToFileSaveFileDialog.CreatePrompt = true;
            this.writeToFileSaveFileDialog.DefaultExt = "txt";
            this.writeToFileSaveFileDialog.FileName = "acquisitionData.txt";
            this.writeToFileSaveFileDialog.Filter = "Text Files|*.txt| All Files|*.*";
            this.writeToFileSaveFileDialog.Title = "Save Acquisition Data To File";
            // 
            // readFromFileOpenFileDialog
            // 
            this.readFromFileOpenFileDialog.DefaultExt = "txt";
            this.readFromFileOpenFileDialog.FileName = "acquisitionData.txt";
            this.readFromFileOpenFileDialog.Filter = "Text Files|*.txt| All Files|*.*";
            this.readFromFileOpenFileDialog.Title = "Open Acquisition Data";
            // 
            // writeToFileGroupBox
            // 
            this.writeToFileGroupBox.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.writeToFileGroupBox.Controls.Add(this.stopButton);
            this.writeToFileGroupBox.Controls.Add(this.startButton);
            this.writeToFileGroupBox.Controls.Add(this.browseWriteButton);
            this.writeToFileGroupBox.Controls.Add(this.filePathWriteLabel);
            this.writeToFileGroupBox.Controls.Add(this.binaryFileWriteRadioButton);
            this.writeToFileGroupBox.Controls.Add(this.textFileWriteRadioButton);
            this.writeToFileGroupBox.Controls.Add(this.filePathWriteTextBox);
            this.writeToFileGroupBox.Controls.Add(this.fileTypeWriteLabel);
            this.writeToFileGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.writeToFileGroupBox.Location = new System.Drawing.Point(8, 240);
            this.writeToFileGroupBox.Name = "writeToFileGroupBox";
            this.writeToFileGroupBox.Size = new System.Drawing.Size(536, 120);
            this.writeToFileGroupBox.TabIndex = 2;
            this.writeToFileGroupBox.TabStop = false;
            this.writeToFileGroupBox.Text = "Write To File";
            // 
            // stopButton
            // 
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(216, 88);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(80, 24);
            this.stopButton.TabIndex = 7;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(120, 88);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 24);
            this.startButton.TabIndex = 6;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // browseWriteButton
            // 
            this.browseWriteButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.browseWriteButton.Location = new System.Drawing.Point(504, 56);
            this.browseWriteButton.Name = "browseWriteButton";
            this.browseWriteButton.Size = new System.Drawing.Size(24, 23);
            this.browseWriteButton.TabIndex = 5;
            this.browseWriteButton.Text = "...";
            this.browseWriteButton.Click += new System.EventHandler(this.browseWriteButton_Click);
            // 
            // filePathWriteLabel
            // 
            this.filePathWriteLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filePathWriteLabel.Location = new System.Drawing.Point(16, 59);
            this.filePathWriteLabel.Name = "filePathWriteLabel";
            this.filePathWriteLabel.Size = new System.Drawing.Size(72, 16);
            this.filePathWriteLabel.TabIndex = 3;
            this.filePathWriteLabel.Text = "File Path:";
            // 
            // binaryFileWriteRadioButton
            // 
            this.binaryFileWriteRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.binaryFileWriteRadioButton.Location = new System.Drawing.Point(192, 24);
            this.binaryFileWriteRadioButton.Name = "binaryFileWriteRadioButton";
            this.binaryFileWriteRadioButton.Size = new System.Drawing.Size(72, 16);
            this.binaryFileWriteRadioButton.TabIndex = 2;
            this.binaryFileWriteRadioButton.Text = "Binary File";
            this.binaryFileWriteRadioButton.CheckedChanged += new System.EventHandler(this.binaryFileWriteRadioButton_CheckedChanged);
            // 
            // textFileWriteRadioButton
            // 
            this.textFileWriteRadioButton.Checked = true;
            this.textFileWriteRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textFileWriteRadioButton.Location = new System.Drawing.Point(120, 24);
            this.textFileWriteRadioButton.Name = "textFileWriteRadioButton";
            this.textFileWriteRadioButton.Size = new System.Drawing.Size(72, 16);
            this.textFileWriteRadioButton.TabIndex = 1;
            this.textFileWriteRadioButton.TabStop = true;
            this.textFileWriteRadioButton.Text = "Text File";
            this.textFileWriteRadioButton.CheckedChanged += new System.EventHandler(this.textFileWriteRadioButton_CheckedChanged);
            // 
            // fileTypeWriteLabel
            // 
            this.fileTypeWriteLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fileTypeWriteLabel.Location = new System.Drawing.Point(16, 24);
            this.fileTypeWriteLabel.Name = "fileTypeWriteLabel";
            this.fileTypeWriteLabel.Size = new System.Drawing.Size(72, 16);
            this.fileTypeWriteLabel.TabIndex = 0;
            this.fileTypeWriteLabel.Text = "File Type:";
            // 
            // readFromFileGroupBox
            // 
            this.readFromFileGroupBox.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.readFromFileGroupBox.Controls.Add(this.browseReadButton);
            this.readFromFileGroupBox.Controls.Add(this.filePathReadLabel);
            this.readFromFileGroupBox.Controls.Add(this.binaryFileReadRadioButton);
            this.readFromFileGroupBox.Controls.Add(this.textFileReadRadioButton);
            this.readFromFileGroupBox.Controls.Add(this.filePathReadTextBox);
            this.readFromFileGroupBox.Controls.Add(this.fileTypeReadLabel);
            this.readFromFileGroupBox.Controls.Add(this.readButton);
            this.readFromFileGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.readFromFileGroupBox.Location = new System.Drawing.Point(9, 360);
            this.readFromFileGroupBox.Name = "readFromFileGroupBox";
            this.readFromFileGroupBox.Size = new System.Drawing.Size(536, 120);
            this.readFromFileGroupBox.TabIndex = 6;
            this.readFromFileGroupBox.TabStop = false;
            this.readFromFileGroupBox.Text = "Read From File";
            // 
            // browseReadButton
            // 
            this.browseReadButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.browseReadButton.Location = new System.Drawing.Point(504, 56);
            this.browseReadButton.Name = "browseReadButton";
            this.browseReadButton.Size = new System.Drawing.Size(24, 23);
            this.browseReadButton.TabIndex = 5;
            this.browseReadButton.Text = "...";
            this.browseReadButton.Click += new System.EventHandler(this.browseReadButton_Click);
            // 
            // filePathReadLabel
            // 
            this.filePathReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.filePathReadLabel.Location = new System.Drawing.Point(16, 56);
            this.filePathReadLabel.Name = "filePathReadLabel";
            this.filePathReadLabel.Size = new System.Drawing.Size(72, 16);
            this.filePathReadLabel.TabIndex = 3;
            this.filePathReadLabel.Text = "File Path:";
            // 
            // binaryFileReadRadioButton
            // 
            this.binaryFileReadRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.binaryFileReadRadioButton.Location = new System.Drawing.Point(192, 24);
            this.binaryFileReadRadioButton.Name = "binaryFileReadRadioButton";
            this.binaryFileReadRadioButton.Size = new System.Drawing.Size(72, 16);
            this.binaryFileReadRadioButton.TabIndex = 2;
            this.binaryFileReadRadioButton.Text = "Binary File";
            this.binaryFileReadRadioButton.CheckedChanged += new System.EventHandler(this.binaryFileReadRadioButton_CheckedChanged);
            // 
            // textFileReadRadioButton
            // 
            this.textFileReadRadioButton.Checked = true;
            this.textFileReadRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textFileReadRadioButton.Location = new System.Drawing.Point(120, 24);
            this.textFileReadRadioButton.Name = "textFileReadRadioButton";
            this.textFileReadRadioButton.Size = new System.Drawing.Size(72, 16);
            this.textFileReadRadioButton.TabIndex = 1;
            this.textFileReadRadioButton.TabStop = true;
            this.textFileReadRadioButton.Text = "Text File";
            this.textFileReadRadioButton.CheckedChanged += new System.EventHandler(this.textFileReadRadioButton_CheckedChanged);
            // 
            // filePathReadTextBox
            // 
            this.filePathReadTextBox.Location = new System.Drawing.Point(120, 56);
            this.filePathReadTextBox.Name = "filePathReadTextBox";
            this.filePathReadTextBox.ReadOnly = true;
            this.filePathReadTextBox.Size = new System.Drawing.Size(384, 20);
            this.filePathReadTextBox.TabIndex = 4;
            this.filePathReadTextBox.Text = "Choose file location";
            // 
            // fileTypeReadLabel
            // 
            this.fileTypeReadLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fileTypeReadLabel.Location = new System.Drawing.Point(16, 24);
            this.fileTypeReadLabel.Name = "fileTypeReadLabel";
            this.fileTypeReadLabel.Size = new System.Drawing.Size(72, 16);
            this.fileTypeReadLabel.TabIndex = 0;
            this.fileTypeReadLabel.Text = "File Type:";
            // 
            // readButton
            // 
            this.readButton.Enabled = false;
            this.readButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.readButton.Location = new System.Drawing.Point(120, 88);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(80, 24);
            this.readButton.TabIndex = 6;
            this.readButton.Text = "Read";
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(554, 504);
            this.Controls.Add(this.readFromFileGroupBox);
            this.Controls.Add(this.writeToFileGroupBox);
            this.Controls.Add(this.acquisitionResultGroupBox);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.channelParametersGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Continuous Acquisition of Voltage Samples - Int Clk - Write to File";
            this.channelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.minimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumValueNumeric)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samplesPerChannelNumeric)).EndInit();
            this.acquisitionResultGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.acquisitionDataGrid)).EndInit();
            this.writeToFileGroupBox.ResumeLayout(false);
            this.writeToFileGroupBox.PerformLayout();
            this.readFromFileGroupBox.ResumeLayout(false);
            this.readFromFileGroupBox.PerformLayout();
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

        private void browseWriteButton_Click(object sender, System.EventArgs e)
        {
            if (textFileWriteRadioButton.Checked) 
            {
                useTextFileWrite = true;
                writeToFileSaveFileDialog.DefaultExt = "*.txt";
                writeToFileSaveFileDialog.FileName = "acquisitionData.txt";
                writeToFileSaveFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
            }
            else
            {
                useTextFileWrite = false;
                writeToFileSaveFileDialog.DefaultExt = "*.bin";
                writeToFileSaveFileDialog.FileName = "acquisitionData.bin";
                writeToFileSaveFileDialog.Filter = "Binary Files|*.bin|All Files|*.*";
            }

            // Display Save File Dialog (Windows forms control)
            DialogResult result = writeToFileSaveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                fileNameWrite = writeToFileSaveFileDialog.FileName;
                filePathWriteTextBox.Text = fileNameWrite;
                fileToolTip.SetToolTip(filePathWriteTextBox, fileNameWrite);
                startButton.Enabled = true;
            }
        }

        private void browseReadButton_Click(object sender, System.EventArgs e)
        {
            if (textFileReadRadioButton.Checked) 
            {
                useTextFileRead = true;
                readFromFileOpenFileDialog.DefaultExt = "*.txt";
                readFromFileOpenFileDialog.FileName = "acquisitionData.txt";
                readFromFileOpenFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
            }
            else
            {
                useTextFileRead = false;
                readFromFileOpenFileDialog.DefaultExt = "*.bin";
                readFromFileOpenFileDialog.FileName = "acquisitionData.bin";
                readFromFileOpenFileDialog.Filter = "Binary Files|*.bin|All Files|*.*";
            }

            // Display Open File Dialog (Windows forms control)
            DialogResult result = readFromFileOpenFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                fileNameRead = readFromFileOpenFileDialog.FileName;
                filePathReadTextBox.Text = fileNameRead;
                fileToolTip.SetToolTip(filePathReadTextBox, fileNameRead);
                readButton.Enabled = true;
            }        
        }

        private void startButton_Click(object sender, System.EventArgs e)
        {
            if (runningTask == null)
            {
                try 
                {   
                    // Create a new file for data
                    bool opened = CreateDataFile();
                    if (!opened)
                    {
                        return;
                    }

                    // Modify the UI
                    stopButton.Enabled = true;
                    startButton.Enabled = false;

                    //Create a new task
                    myTask = new Task(); 
                                    
                    //Create a virtual channel
                    myTask.AIChannels.CreateVoltageChannel(physicalChannelComboBox.Text, "",
                        (AITerminalConfiguration)(-1), Convert.ToDouble(minimumValueNumeric.Value),
                        Convert.ToDouble(maximumValueNumeric.Value), AIVoltageUnits.Volts);  
                
                    //Configure the timing parameters
                    myTask.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value),
                        SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 1000);
                    
                    //Verify the Task
                    myTask.Control(TaskAction.Verify);

                    //Prepare the table and file for Data
                    String[] channelNames = new String[myTask.AIChannels.Count];
                    int i = 0;
                    foreach (AIChannel a in myTask.AIChannels)
                    {
                        channelNames[i++] = a.PhysicalName;
                    }

                    InitializeDataTable(channelNames, ref dataTable); 
                    acquisitionDataGrid.DataSource = dataTable;   

                    // Add the channel names (and any other information) to the file
                    int samples = Convert.ToInt32(samplesPerChannelNumeric.Value);
                    PrepareFileForData();
                    savedData = new ArrayList();
                    for (i = 0; i < myTask.AIChannels.Count; i++)
                    {
                        savedData.Add(new ArrayList());
                    }
                    
                    runningTask = myTask;
                    analogInReader= new AnalogMultiChannelReader(myTask.Stream);

                    // Use SynchronizeCallbacks to specify that the object 
                    // marshals callbacks across threads appropriately.
                    analogInReader.SynchronizeCallbacks = true;
                    
                    analogCallback = new AsyncCallback(AnalogInCallback);
                           
                    analogInReader.BeginReadMultiSample(samples, analogCallback, myTask);
                
                }
                catch (DaqException exception)
                {
                    //Display Errors
                    MessageBox.Show(exception.Message);
                    runningTask=null;
                    myTask.Dispose();
                    stopButton.Enabled = false;
                    startButton.Enabled = true;
                    writeToFileGroupBox.Enabled = true;
                }           
            }
        }

        private void AnalogInCallback(IAsyncResult ar)
        {
            try
            {
                if(runningTask != null && runningTask == ar.AsyncState)
                {
                    //Read the available data from the channels
                    data = analogInReader.EndReadMultiSample(ar);
                                      
                    //Plot your data here
                    //Displays data in grid and writes to file
                    DisplayData(data, ref dataTable); 

                    LogData(data);

                    analogInReader.BeginReadMultiSample(Convert.ToInt32(samplesPerChannelNumeric.Value),
                        analogCallback, myTask);
                }
            }
            catch(DaqException exception)
            {   
                //Display Errors
                MessageBox.Show(exception.Message);
                runningTask = null;
                myTask.Dispose();
                stopButton.Enabled = false;
                startButton.Enabled = true;
             }
        }

        private void stopButton_Click(object sender, System.EventArgs e)
        {
            if (runningTask != null)
            {
                //Dispose of the task
                CloseFile();

                runningTask = null;
                myTask.Dispose();
                stopButton.Enabled = false;
                startButton.Enabled = true;
                writeToFileGroupBox.Enabled = true;
            }
        }

        
        private void readButton_Click(object sender, System.EventArgs e)
        {
            // Modify UI
            readButton.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            // Open file
            bool opened = OpenDataFile();

            // Load data
            if (useTextFileRead)
            {
                ReadTextData();
                fileStreamReader.Close();
            }
            else
            {
                ReadBinaryData();
                fileBinaryReader.Close();
            }

            this.Cursor = Cursors.Default;
            readButton.Enabled = true;
        }

        private void DisplayData(double[,] sourceArray, ref DataTable dataTable)
        {   
            //Display the first 10 points of the Read/Write in the Datagrid
            try
            {
                int channelCount = sourceArray.GetLength(0);
                int dataCount;
                
                if (sourceArray.GetLength(1) < 10)
                    dataCount = sourceArray.GetLength(1);
                else
                    dataCount = 10;
                
                // Write to Data Table
                for (int i = 0; i < dataCount; i++)             
                {
                    for (int j = 0; j < channelCount; j++)
                    {
                        // Writes data to data table
                        dataTable.Rows[i][j] = sourceArray.GetValue(j, i); 
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                runningTask = null;
                myTask.Dispose();
                stopButton.Enabled = false;
                startButton.Enabled = true;
                writeToFileGroupBox.Enabled = true;
            }
        }

        private void LogData(double[,] data)
        {
            int channelCount = data.GetLength(0);
            int dataCount = data.GetLength(1);

            for (int i = 0; i < channelCount; i++)
            {
                ArrayList l = savedData[i] as ArrayList;

                for (int j = 0; j < dataCount; j++)
                {
                    l.Add(data[i, j]);
                }
            }
        }

        private void ReadTextData()
        {
            try
            {
                char[] tab = { '\t' };
                String[] split = fileStreamReader.ReadLine().Replace("\n", "").Split(tab);
                String[] channels = new String[split.GetLength(0) - 1];
                Array.Copy(split, 0, channels, 0, split.GetLength(0) - 1);
                int samples = Int32.Parse(fileStreamReader.ReadLine().Replace("\n", ""));
                int channelCount = channels.GetLength(0);

                double[,] array = new double[channelCount, samples];

                String line;
                for (int iSample = 0; iSample < samples; iSample++)
                {
                    line = fileStreamReader.ReadLine();
                    String[] values = line.Split(tab);

                    for (int iChan = 0; iChan < channelCount; iChan++)
                    {
                        array[iChan, iSample] = Convert.ToDouble(values[iChan]);
                    }
                }

                InitializeDataTable(channels, ref dataTable);
                acquisitionDataGrid.DataSource = dataTable;  
                DisplayData(array, ref dataTable);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                runningTask = null;
                readButton.Enabled = true;
                readFromFileGroupBox.Enabled = true;
            }
        }

        private void ReadBinaryData()
        {
            try
            {
                String s;
                ArrayList arrayList = new ArrayList();
                while ((s = fileBinaryReader.ReadString()) != "\r\n")
                {
                    arrayList.Add(s);
                }
                
                String[] channels = arrayList.ToArray(typeof(String)) as String[];                          
                int samples = Int32.Parse(fileBinaryReader.ReadString());
                int channelCount = channels.GetLength(0);

                double[,] array = new double[channelCount, samples];

                for (int iSample = 0; iSample < samples; iSample++)
                {
                    for (int iChan = 0; iChan < channelCount; iChan++)
                    {
                        array[iChan, iSample] = fileBinaryReader.ReadDouble();
                    }
                }

                InitializeDataTable(channels, ref dataTable);
                acquisitionDataGrid.DataSource = dataTable;  
                DisplayData(array, ref dataTable);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                runningTask = null;
                readButton.Enabled = true;
                readFromFileGroupBox.Enabled = true;
            }
        }

        private void CloseFile()
        {
            int channelCount = savedData.Count;
            int dataCount = (savedData[0] as ArrayList).Count;

            try
            {
                if (useTextFileWrite)
                {
                    fileStreamWriter.WriteLine(dataCount.ToString());

                    for (int i = 0; i < dataCount; i++)
                    {
                        for (int j = 0; j < channelCount; j++)
                        {
                            // Writes data to file
                            ArrayList l = savedData[j] as ArrayList;
                            double dataValue = (double)l[i];
                            fileStreamWriter.Write(dataValue.ToString("e6"));
                            fileStreamWriter.Write("\t"); //seperate the data for each channel
                        }
                        fileStreamWriter.WriteLine(); //new line of data (start next scan)
                    }

                    fileStreamWriter.Close();
                }
                else
                {
                    fileBinaryWriter.Write(dataCount.ToString());

                    for (int i = 0; i < dataCount; i++)
                    {
                        for (int j = 0; j < channelCount; j++)
                        {
                            // Writes data to file
                            ArrayList l = savedData[j] as ArrayList;
                            double dataValue = (double)l[i];
                            fileBinaryWriter.Write((double)dataValue);
                        }
                    }

                    fileBinaryWriter.Close();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.TargetSite.ToString());
                runningTask = null;
                myTask.Dispose();
                stopButton.Enabled = false;
                startButton.Enabled = true;
            }
        }

        public void InitializeDataTable(String[] channelNames, ref DataTable data)
        {
            int numChannels = channelNames.GetLength(0);
            data.Rows.Clear();
            data.Columns.Clear();
            dataColumn = new DataColumn[numChannels];
            int numOfRows = 10;

            for (int i = 0; i < numChannels; i++)
            {   
                dataColumn[i] = new DataColumn();
                dataColumn[i].DataType = typeof(double);
                dataColumn[i].ColumnName = channelNames[i];
            }

            data.Columns.AddRange(dataColumn); 

            for (int i = 0; i < numOfRows; i++)             
            {
                object[] rowArr = new object[numChannels];
                data.Rows.Add(rowArr);              
            }
        }

        //Creates a text/binary stream based on the user selections
        private bool CreateDataFile()
        {
            try
            {
                FileStream fs = new FileStream(fileNameWrite, FileMode.Create);
                if (useTextFileWrite)
                {
                    fileStreamWriter = new StreamWriter(fs);
                }
                else
                {
                    fileBinaryWriter = new BinaryWriter(fs);
                }
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        // Opens a text/binary stream based on the user selections
        private bool OpenDataFile()
        {
            try
            {
                FileStream fs = new FileStream(fileNameRead, FileMode.Open);
                if (useTextFileRead)
                {
                    fileStreamReader = new StreamReader(fs);
                }
                else
                {
                    fileBinaryReader = new BinaryReader(fs);
                }
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        // Only used by text files to write the channel name
        // Can expand this for binary too
        private void PrepareFileForData()
        {
            //Prepare file for data (Write out the channel names
            int numChannels = myTask.AIChannels.Count;

            if (useTextFileWrite)
            {
                for (int i = 0; i < numChannels; i++)
                {   
                    fileStreamWriter.Write(myTask.AIChannels[i].PhysicalName);
                    fileStreamWriter.Write("\t"); 
                }
                fileStreamWriter.WriteLine();
            }
            else
            {
                for (int i = 0; i < numChannels; i++)
                {   
                    fileBinaryWriter.Write(myTask.AIChannels[i].PhysicalName);
                }
                fileBinaryWriter.Write("\r\n");
            }
        }

        private void textFileWriteRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            if (textFileWriteRadioButton.Checked)
            {
                useTextFileWrite = true;
            }
            
            startButton.Enabled = false;
        }

        private void binaryFileWriteRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            if (binaryFileWriteRadioButton.Checked)
            {
                useTextFileWrite = false;
            }
            
            startButton.Enabled = false;
        }

        private void textFileReadRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            if (textFileReadRadioButton.Checked)
            {
                useTextFileRead = true;
            }

            readButton.Enabled = false;
        }

        private void binaryFileReadRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            if (binaryFileReadRadioButton.Checked)
            {
                useTextFileRead = false;
            }

            readButton.Enabled = false;
        }
    }
}
