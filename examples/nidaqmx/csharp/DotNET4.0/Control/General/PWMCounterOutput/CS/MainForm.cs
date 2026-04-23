/******************************************************************************
*
* Example program:
*   PWMCounterOutput
*
* Category:
*   Control
*
* Description:
*   This example demonstrates how to do Pulse Width Modulation using Analog
*   Input and Counter Output.
*
* Instructions for running:
*   1.  Select the physical channel to correspond to where your signal is input
*       on the DAQ device. Also, select the corresponding channel for where your
*       signal is being generated.
*   2.  Enter the minimum and maximum voltage ranges.Note:  For better accuracy
*       try to match the input range to the expected voltage level of the
*       measured signal.
*   3.  Set the sample rate of the acquisition.Note:  The rate should be at
*       least twice as fast as the maximum frequency component of the signal
*       being acquired.
*   4.  Set the initial frequency and duty cycle of the output pulse.
*
* Steps:
*   1.  Create an analog input voltage channel. Also, create acounter output
*       pulse channel.
*   2.  Set the rate for the sample clock of the analog input and thecounter
*       output. Additionally, define the sample mode to behardware timed single
*       point for both tasks.
*   3.  Call the GetDeviceName function. This willtake a task and a terminal and
*       create a properly formatteddevice + terminal name to use as the source
*       of the sampleclock for the CO task. By sharing the sample clocks the
*       taskswill be synchronized.
*   4.  Call the Start function to arm the two functions. Make surethe counter
*       output is armed before the analog input. Thiswill ensure both will start
*       at the same time.
*   5.  Call AnalogSingleChannelReader.BeginReadSingleSample to install a
*       callback and begin the asynchronous read operation.
*   6.  Inside the callback, call AnalogSingleChannelReader.EndReadSingleSample
*       to retrieve the data from the read operation.  Pass this data to
*       DutyCycleCalculation method to determine if the duty cycle has changed.
*       If the duty cycle has changed, then we need to write this new value by
*       calling CounterSingleChannelReader.WriteSingleSample.
*   7.  Call AnalogSingleChannelReader.BeginReadSingleSample again inside the
*       callback to perform another read operation.
*   8.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   9.  Handle any DaqExceptions, if they occur.
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
*   Make sure your signal input/output terminals match the Physical Channel I/O
*   controls.Note: For this example to work you must ensure you have identified
*   your PXI chassis in MAX.
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
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NationalInstruments;
using NationalInstruments.DAQmx;

namespace NationalInstruments.Examples.PWMCounterOutput
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label inputPhysicalChannelLabel;
        private System.Windows.Forms.Label inputMinimumValueLabel;
        private System.Windows.Forms.Label inputMaximumValueLabel;
        private System.Windows.Forms.Label counterOutputPhysicalChannelLabel;
        private System.Windows.Forms.ComboBox counterOutputPhysicalChannelComboBox;
        private System.Windows.Forms.Label frequencyLabel;
        private System.Windows.Forms.Label dutyCycleLabel;
        private System.Windows.Forms.GroupBox timingParametersGroupBox;
        private System.Windows.Forms.Label inputRateLabel;
        private System.Windows.Forms.Label setPointLabel;
        private System.Windows.Forms.Label proportionalGainLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.NumericUpDown frequencyNumeric;
        private System.Windows.Forms.NumericUpDown inputMinimumValueNumeric;
        private System.Windows.Forms.NumericUpDown inputMaximumValueNumeric;
        private System.Windows.Forms.NumericUpDown dutyCycleNumeric;
        
        private CounterSingleChannelWriter writer;
        private AnalogSingleChannelReader reader;
        private AsyncCallback inputCallback;
        private Task counterTask;
        private Task analogInputTask;
        private Task runningAnalogTask;
        private Task runningCounterTask;
        private COPulseIdleState idleState;
        private CODataFrequency COData;
        private double dutyCycle;
        private double lastPoint;
        private double point;
        private double tempDutyCycle;
        private double mininumInputValue = -10;
        private double maximumInputValue = 10;
        private bool dutyCycleChanged = false;
        private System.Windows.Forms.GroupBox controlParametersGroupBox;
        private System.Windows.Forms.NumericUpDown proportionalGainNumeric;
        private System.Windows.Forms.NumericUpDown setPointNumeric;
        private System.Windows.Forms.NumericUpDown inputRateNumeric;
        private System.Windows.Forms.GroupBox inputChannelParametersGroupBox;
        private System.Windows.Forms.GroupBox outputChannelParametersGroupBox;
        private System.Windows.Forms.ComboBox inputPhysicalChannelComboBox;

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
            idleState = COPulseIdleState.Low;

            inputPhysicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
            counterOutputPhysicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.CO, PhysicalChannelAccess.External));

            if (inputPhysicalChannelComboBox.Items.Count > 0)
                inputPhysicalChannelComboBox.SelectedIndex = 0;
            if (counterOutputPhysicalChannelComboBox.Items.Count > 0)
                counterOutputPhysicalChannelComboBox.SelectedIndex = 0;

            if (inputPhysicalChannelComboBox.Items.Count > 0 && counterOutputPhysicalChannelComboBox.Items.Count > 0)
                startButton.Enabled = true;
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
                if (analogInputTask != null)
                {
                    runningAnalogTask = null;
                    analogInputTask.Dispose();
                }
                if (counterTask != null)
                {
                    runningCounterTask = null;
                    counterTask.Dispose();
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
            this.inputChannelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.inputMinimumValueLabel = new System.Windows.Forms.Label();
            this.inputPhysicalChannelLabel = new System.Windows.Forms.Label();
            this.inputMaximumValueLabel = new System.Windows.Forms.Label();
            this.inputMinimumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.inputMaximumValueNumeric = new System.Windows.Forms.NumericUpDown();
            this.inputPhysicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.frequencyNumeric = new System.Windows.Forms.NumericUpDown();
            this.counterOutputPhysicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.frequencyLabel = new System.Windows.Forms.Label();
            this.counterOutputPhysicalChannelLabel = new System.Windows.Forms.Label();
            this.dutyCycleLabel = new System.Windows.Forms.Label();
            this.dutyCycleNumeric = new System.Windows.Forms.NumericUpDown();
            this.timingParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.inputRateLabel = new System.Windows.Forms.Label();
            this.inputRateNumeric = new System.Windows.Forms.NumericUpDown();
            this.setPointLabel = new System.Windows.Forms.Label();
            this.proportionalGainLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.proportionalGainNumeric = new System.Windows.Forms.NumericUpDown();
            this.setPointNumeric = new System.Windows.Forms.NumericUpDown();
            this.controlParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.outputChannelParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.inputChannelParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inputMinimumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inputMaximumValueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dutyCycleNumeric)).BeginInit();
            this.timingParametersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inputRateNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.proportionalGainNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setPointNumeric)).BeginInit();
            this.controlParametersGroupBox.SuspendLayout();
            this.outputChannelParametersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputChannelParametersGroupBox
            // 
            this.inputChannelParametersGroupBox.Controls.Add(this.inputMinimumValueLabel);
            this.inputChannelParametersGroupBox.Controls.Add(this.inputPhysicalChannelLabel);
            this.inputChannelParametersGroupBox.Controls.Add(this.inputMaximumValueLabel);
            this.inputChannelParametersGroupBox.Controls.Add(this.inputMinimumValueNumeric);
            this.inputChannelParametersGroupBox.Controls.Add(this.inputMaximumValueNumeric);
            this.inputChannelParametersGroupBox.Controls.Add(this.inputPhysicalChannelComboBox);
            this.inputChannelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.inputChannelParametersGroupBox.Location = new System.Drawing.Point(16, 16);
            this.inputChannelParametersGroupBox.Name = "inputChannelParametersGroupBox";
            this.inputChannelParametersGroupBox.Size = new System.Drawing.Size(304, 136);
            this.inputChannelParametersGroupBox.TabIndex = 0;
            this.inputChannelParametersGroupBox.TabStop = false;
            this.inputChannelParametersGroupBox.Text = "Input Channel Parameters";
            // 
            // inputMinimumValueLabel
            // 
            this.inputMinimumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.inputMinimumValueLabel.Location = new System.Drawing.Point(16, 66);
            this.inputMinimumValueLabel.Name = "inputMinimumValueLabel";
            this.inputMinimumValueLabel.Size = new System.Drawing.Size(112, 16);
            this.inputMinimumValueLabel.TabIndex = 2;
            this.inputMinimumValueLabel.Text = "Input Minimum Value:";
            // 
            // inputPhysicalChannelLabel
            // 
            this.inputPhysicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.inputPhysicalChannelLabel.Location = new System.Drawing.Point(16, 26);
            this.inputPhysicalChannelLabel.Name = "inputPhysicalChannelLabel";
            this.inputPhysicalChannelLabel.Size = new System.Drawing.Size(128, 16);
            this.inputPhysicalChannelLabel.TabIndex = 0;
            this.inputPhysicalChannelLabel.Text = "Input Physical Channel:";
            // 
            // inputMaximumValueLabel
            // 
            this.inputMaximumValueLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.inputMaximumValueLabel.Location = new System.Drawing.Point(16, 106);
            this.inputMaximumValueLabel.Name = "inputMaximumValueLabel";
            this.inputMaximumValueLabel.Size = new System.Drawing.Size(120, 16);
            this.inputMaximumValueLabel.TabIndex = 4;
            this.inputMaximumValueLabel.Text = "Input Maximum Value:";
            // 
            // inputMinimumValueNumeric
            // 
            this.inputMinimumValueNumeric.DecimalPlaces = 1;
            this.inputMinimumValueNumeric.Increment = new System.Decimal(new int[] {
                                                                                       1,
                                                                                       0,
                                                                                       0,
                                                                                       65536});
            this.inputMinimumValueNumeric.Location = new System.Drawing.Point(216, 64);
            this.inputMinimumValueNumeric.Maximum = new System.Decimal(new int[] {
                                                                                     10,
                                                                                     0,
                                                                                     0,
                                                                                     0});
            this.inputMinimumValueNumeric.Minimum = new System.Decimal(new int[] {
                                                                                     10,
                                                                                     0,
                                                                                     0,
                                                                                     -2147483648});
            this.inputMinimumValueNumeric.Name = "inputMinimumValueNumeric";
            this.inputMinimumValueNumeric.Size = new System.Drawing.Size(80, 20);
            this.inputMinimumValueNumeric.TabIndex = 3;
            this.inputMinimumValueNumeric.Value = new System.Decimal(new int[] {
                                                                                   100,
                                                                                   0,
                                                                                   0,
                                                                                   -2147418112});
            // 
            // inputMaximumValueNumeric
            // 
            this.inputMaximumValueNumeric.DecimalPlaces = 1;
            this.inputMaximumValueNumeric.Increment = new System.Decimal(new int[] {
                                                                                       1,
                                                                                       0,
                                                                                       0,
                                                                                       65536});
            this.inputMaximumValueNumeric.Location = new System.Drawing.Point(216, 104);
            this.inputMaximumValueNumeric.Maximum = new System.Decimal(new int[] {
                                                                                     10,
                                                                                     0,
                                                                                     0,
                                                                                     0});
            this.inputMaximumValueNumeric.Minimum = new System.Decimal(new int[] {
                                                                                     10,
                                                                                     0,
                                                                                     0,
                                                                                     -2147483648});
            this.inputMaximumValueNumeric.Name = "inputMaximumValueNumeric";
            this.inputMaximumValueNumeric.Size = new System.Drawing.Size(80, 20);
            this.inputMaximumValueNumeric.TabIndex = 5;
            this.inputMaximumValueNumeric.Value = new System.Decimal(new int[] {
                                                                                   100,
                                                                                   0,
                                                                                   0,
                                                                                   65536});
            // 
            // inputPhysicalChannelComboBox
            // 
            this.inputPhysicalChannelComboBox.Location = new System.Drawing.Point(216, 24);
            this.inputPhysicalChannelComboBox.Name = "inputPhysicalChannelComboBox";
            this.inputPhysicalChannelComboBox.Size = new System.Drawing.Size(80, 21);
            this.inputPhysicalChannelComboBox.TabIndex = 1;
            this.inputPhysicalChannelComboBox.Text = "Dev1/ai0";
            // 
            // frequencyNumeric
            // 
            this.frequencyNumeric.Location = new System.Drawing.Point(216, 64);
            this.frequencyNumeric.Maximum = new System.Decimal(new int[] {
                                                                             1000000,
                                                                             0,
                                                                             0,
                                                                             0});
            this.frequencyNumeric.Name = "frequencyNumeric";
            this.frequencyNumeric.Size = new System.Drawing.Size(80, 20);
            this.frequencyNumeric.TabIndex = 3;
            this.frequencyNumeric.Value = new System.Decimal(new int[] {
                                                                           100000,
                                                                           0,
                                                                           0,
                                                                           0});
            // 
            // counterOutputPhysicalChannelComboBox
            // 
            this.counterOutputPhysicalChannelComboBox.Location = new System.Drawing.Point(216, 16);
            this.counterOutputPhysicalChannelComboBox.Name = "counterOutputPhysicalChannelComboBox";
            this.counterOutputPhysicalChannelComboBox.Size = new System.Drawing.Size(80, 21);
            this.counterOutputPhysicalChannelComboBox.TabIndex = 1;
            this.counterOutputPhysicalChannelComboBox.Text = "Dev1/ctr0";
            // 
            // frequencyLabel
            // 
            this.frequencyLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.frequencyLabel.Location = new System.Drawing.Point(16, 66);
            this.frequencyLabel.Name = "frequencyLabel";
            this.frequencyLabel.Size = new System.Drawing.Size(112, 16);
            this.frequencyLabel.TabIndex = 2;
            this.frequencyLabel.Text = "Frequency:";
            // 
            // counterOutputPhysicalChannelLabel
            // 
            this.counterOutputPhysicalChannelLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.counterOutputPhysicalChannelLabel.Location = new System.Drawing.Point(16, 18);
            this.counterOutputPhysicalChannelLabel.Name = "counterOutputPhysicalChannelLabel";
            this.counterOutputPhysicalChannelLabel.Size = new System.Drawing.Size(160, 16);
            this.counterOutputPhysicalChannelLabel.TabIndex = 0;
            this.counterOutputPhysicalChannelLabel.Text = "Counter Output Physical Channel:";
            // 
            // dutyCycleLabel
            // 
            this.dutyCycleLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dutyCycleLabel.Location = new System.Drawing.Point(16, 106);
            this.dutyCycleLabel.Name = "dutyCycleLabel";
            this.dutyCycleLabel.Size = new System.Drawing.Size(104, 16);
            this.dutyCycleLabel.TabIndex = 4;
            this.dutyCycleLabel.Text = "Duty Cycle:";
            // 
            // dutyCycleNumeric
            // 
            this.dutyCycleNumeric.DecimalPlaces = 2;
            this.dutyCycleNumeric.Increment = new System.Decimal(new int[] {
                                                                               1,
                                                                               0,
                                                                               0,
                                                                               131072});
            this.dutyCycleNumeric.Location = new System.Drawing.Point(216, 104);
            this.dutyCycleNumeric.Maximum = new System.Decimal(new int[] {
                                                                             1,
                                                                             0,
                                                                             0,
                                                                             0});
            this.dutyCycleNumeric.Name = "dutyCycleNumeric";
            this.dutyCycleNumeric.Size = new System.Drawing.Size(80, 20);
            this.dutyCycleNumeric.TabIndex = 5;
            this.dutyCycleNumeric.Value = new System.Decimal(new int[] {
                                                                           5,
                                                                           0,
                                                                           0,
                                                                           65536});
            // 
            // timingParametersGroupBox
            // 
            this.timingParametersGroupBox.Controls.Add(this.inputRateLabel);
            this.timingParametersGroupBox.Controls.Add(this.inputRateNumeric);
            this.timingParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.timingParametersGroupBox.Location = new System.Drawing.Point(16, 320);
            this.timingParametersGroupBox.Name = "timingParametersGroupBox";
            this.timingParametersGroupBox.Size = new System.Drawing.Size(304, 56);
            this.timingParametersGroupBox.TabIndex = 2;
            this.timingParametersGroupBox.TabStop = false;
            this.timingParametersGroupBox.Text = "Timing Parameters";
            // 
            // inputRateLabel
            // 
            this.inputRateLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.inputRateLabel.Location = new System.Drawing.Point(16, 26);
            this.inputRateLabel.Name = "inputRateLabel";
            this.inputRateLabel.Size = new System.Drawing.Size(96, 16);
            this.inputRateLabel.TabIndex = 0;
            this.inputRateLabel.Text = "Input Rate:";
            // 
            // inputRateNumeric
            // 
            this.inputRateNumeric.Location = new System.Drawing.Point(216, 24);
            this.inputRateNumeric.Maximum = new System.Decimal(new int[] {
                                                                             100000,
                                                                             0,
                                                                             0,
                                                                             0});
            this.inputRateNumeric.Minimum = new System.Decimal(new int[] {
                                                                             100000,
                                                                             0,
                                                                             0,
                                                                             -2147483648});
            this.inputRateNumeric.Name = "inputRateNumeric";
            this.inputRateNumeric.Size = new System.Drawing.Size(80, 20);
            this.inputRateNumeric.TabIndex = 1;
            this.inputRateNumeric.Value = new System.Decimal(new int[] {
                                                                           1000,
                                                                           0,
                                                                           0,
                                                                           0});
            // 
            // setPointLabel
            // 
            this.setPointLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.setPointLabel.Location = new System.Drawing.Point(16, 26);
            this.setPointLabel.Name = "setPointLabel";
            this.setPointLabel.Size = new System.Drawing.Size(72, 16);
            this.setPointLabel.TabIndex = 0;
            this.setPointLabel.Text = "Set Point:";
            // 
            // proportionalGainLabel
            // 
            this.proportionalGainLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.proportionalGainLabel.Location = new System.Drawing.Point(16, 66);
            this.proportionalGainLabel.Name = "proportionalGainLabel";
            this.proportionalGainLabel.Size = new System.Drawing.Size(96, 16);
            this.proportionalGainLabel.TabIndex = 2;
            this.proportionalGainLabel.Text = "Proportional Gain:";
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.startButton.Location = new System.Drawing.Point(64, 512);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 24);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Start";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(192, 512);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(80, 24);
            this.stopButton.TabIndex = 5;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // proportionalGainNumeric
            // 
            this.proportionalGainNumeric.Location = new System.Drawing.Point(216, 64);
            this.proportionalGainNumeric.Maximum = new System.Decimal(new int[] {
                                                                                    1000,
                                                                                    0,
                                                                                    0,
                                                                                    0});
            this.proportionalGainNumeric.Minimum = new System.Decimal(new int[] {
                                                                                    1000,
                                                                                    0,
                                                                                    0,
                                                                                    -2147483648});
            this.proportionalGainNumeric.Name = "proportionalGainNumeric";
            this.proportionalGainNumeric.Size = new System.Drawing.Size(80, 20);
            this.proportionalGainNumeric.TabIndex = 3;
            this.proportionalGainNumeric.Value = new System.Decimal(new int[] {
                                                                                  5,
                                                                                  0,
                                                                                  0,
                                                                                  0});
            // 
            // setPointNumeric
            // 
            this.setPointNumeric.Location = new System.Drawing.Point(216, 24);
            this.setPointNumeric.Maximum = new System.Decimal(new int[] {
                                                                            1000,
                                                                            0,
                                                                            0,
                                                                            0});
            this.setPointNumeric.Minimum = new System.Decimal(new int[] {
                                                                            1000,
                                                                            0,
                                                                            0,
                                                                            -2147483648});
            this.setPointNumeric.Name = "setPointNumeric";
            this.setPointNumeric.Size = new System.Drawing.Size(80, 20);
            this.setPointNumeric.TabIndex = 1;
            this.setPointNumeric.Value = new System.Decimal(new int[] {
                                                                          8,
                                                                          0,
                                                                          0,
                                                                          0});
            // 
            // controlParametersGroupBox
            // 
            this.controlParametersGroupBox.Controls.Add(this.setPointLabel);
            this.controlParametersGroupBox.Controls.Add(this.proportionalGainNumeric);
            this.controlParametersGroupBox.Controls.Add(this.setPointNumeric);
            this.controlParametersGroupBox.Controls.Add(this.proportionalGainLabel);
            this.controlParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.controlParametersGroupBox.Location = new System.Drawing.Point(16, 392);
            this.controlParametersGroupBox.Name = "controlParametersGroupBox";
            this.controlParametersGroupBox.Size = new System.Drawing.Size(304, 104);
            this.controlParametersGroupBox.TabIndex = 3;
            this.controlParametersGroupBox.TabStop = false;
            this.controlParametersGroupBox.Text = "Control Parameters";
            // 
            // outputChannelParametersGroupBox
            // 
            this.outputChannelParametersGroupBox.Controls.Add(this.dutyCycleNumeric);
            this.outputChannelParametersGroupBox.Controls.Add(this.dutyCycleLabel);
            this.outputChannelParametersGroupBox.Controls.Add(this.counterOutputPhysicalChannelLabel);
            this.outputChannelParametersGroupBox.Controls.Add(this.frequencyLabel);
            this.outputChannelParametersGroupBox.Controls.Add(this.counterOutputPhysicalChannelComboBox);
            this.outputChannelParametersGroupBox.Controls.Add(this.frequencyNumeric);
            this.outputChannelParametersGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.outputChannelParametersGroupBox.Location = new System.Drawing.Point(16, 168);
            this.outputChannelParametersGroupBox.Name = "outputChannelParametersGroupBox";
            this.outputChannelParametersGroupBox.Size = new System.Drawing.Size(304, 136);
            this.outputChannelParametersGroupBox.TabIndex = 1;
            this.outputChannelParametersGroupBox.TabStop = false;
            this.outputChannelParametersGroupBox.Text = "Output Channel Parameters";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(336, 550);
            this.Controls.Add(this.outputChannelParametersGroupBox);
            this.Controls.Add(this.controlParametersGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.timingParametersGroupBox);
            this.Controls.Add(this.inputChannelParametersGroupBox);
            this.Controls.Add(this.stopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Pulse Width Modulation-Counter Output";
            this.inputChannelParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inputMinimumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inputMaximumValueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dutyCycleNumeric)).EndInit();
            this.timingParametersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inputRateNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.proportionalGainNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setPointNumeric)).EndInit();
            this.controlParametersGroupBox.ResumeLayout(false);
            this.outputChannelParametersGroupBox.ResumeLayout(false);
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
            if(runningAnalogTask == null)
            {
                try
                {
                    // Initialize dutyCycle
                    dutyCycle = Convert.ToDouble(dutyCycleNumeric.Value);

                    // Initialize mininumInputValue and maximumInputValue
                    mininumInputValue = (double) inputMinimumValueNumeric.Value;
                    maximumInputValue = (double) inputMaximumValueNumeric.Value;

                    // Create tasks
                    counterTask = new Task("Counter Output");
                    analogInputTask = new Task("Analog Input");

                    // Configure channels
                    analogInputTask.AIChannels.CreateVoltageChannel(inputPhysicalChannelComboBox.Text,
                        "Analog Input",
                        NationalInstruments.DAQmx.AITerminalConfiguration.Differential,
                        Convert.ToDouble(inputMinimumValueNumeric.Value),
                        Convert.ToDouble(inputMaximumValueNumeric.Value),
                        AIVoltageUnits.Volts);

                    counterTask.COChannels.CreatePulseChannelFrequency(counterOutputPhysicalChannelComboBox.Text,
                        "PWM Channel", 
                        COPulseFrequencyUnits.Hertz, 
                        idleState, 
                        0.0, 
                        Convert.ToDouble(frequencyNumeric.Value), 
                        Convert.ToDouble(dutyCycleNumeric.Value));

                    // Set up the timing for both tasks
                    analogInputTask.Timing.ConfigureSampleClock("",
                        Convert.ToDouble(inputRateNumeric.Value),
                        NationalInstruments.DAQmx.SampleClockActiveEdge.Rising,
                        SampleQuantityMode.HardwareTimedSinglePoint);
                
                    // Use the same timing source for the PWM Output
                    string deviceName = inputPhysicalChannelComboBox.Text.Split('/')[0];
                    string terminalNameBase = "/" + GetDeviceName(deviceName) + "/";
                
                    counterTask.Timing.ConfigureSampleClock(terminalNameBase + "ai/SampleClock",
                        Convert.ToDouble(Convert.ToDouble(inputRateNumeric.Value)),
                        SampleClockActiveEdge.Rising,
                        SampleQuantityMode.HardwareTimedSinglePoint);
                
                    // Start the tasks
                    StartTask();                   

                    // Start PWM as well
                    inputCallback = new AsyncCallback(InputRead);
                    reader = new AnalogSingleChannelReader(analogInputTask.Stream);
                    writer = new CounterSingleChannelWriter(counterTask.Stream);
                    COData = new CODataFrequency(Convert.ToDouble(frequencyNumeric.Value), dutyCycle);

                    // Use SynchronizeCallbacks to specify that the object 
                    // marshals callbacks across threads appropriately.
                    reader.SynchronizeCallbacks = true;
                    reader.BeginReadSingleSample(inputCallback,analogInputTask);
                }
                catch(Exception ex)
                {
                    StopTask();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void StopButton_Click(object sender, System.EventArgs e)
        {
            if(runningCounterTask != null)
            {
                StopTask();
            }
        }

        private void InputRead(IAsyncResult ar)
        {
            try
            {
                if (runningAnalogTask != null && runningAnalogTask == ar.AsyncState)
                {
                    // Calculate PWM duty cycle based on the set point, gain, and input
                    dutyCycleChanged = DutyCycleCalculation(reader.EndReadSingleSample(ar),
                        Convert.ToDouble(setPointNumeric.Value),
                        Convert.ToDouble(proportionalGainNumeric.Value));
                        
                    if(dutyCycleChanged)
                    {
                        // Change duty cycle
                        COData.DutyCycle = dutyCycle;
                        writer.WriteSingleSample(true, COData); 
                    }

                    // Wait for the next sample clock
                    analogInputTask.Timing.SinglePoint.WaitForNextSampleClock();                

                    // Set up next callback
                    reader.BeginReadSingleSample(inputCallback,analogInputTask);                
                }
            }
            catch (Exception ex)
            {
                StopTask();
                MessageBox.Show(ex.Message);
            }
        }

        // Returns true if duty cycle needs to be changed and edits duty cycle, otherwise, returns true and leaves dutyCycle unchanged
        private bool DutyCycleCalculation(double input, double setPoint, double proportionalGain)
        { 
            point = (input-setPoint)*proportionalGain;

            // If new point is not within .5 of old point, update the output
            if(point<=lastPoint-0.5 || point>=lastPoint+0.5)
            {
                lastPoint = point;
                tempDutyCycle = (input - mininumInputValue)/(maximumInputValue-mininumInputValue);

                if(tempDutyCycle>.999)
                {
                    dutyCycle = .999;
                }
                else if(tempDutyCycle < 0.001)
                {
                    dutyCycle = .001;
                }
                else 
                {
                    dutyCycle = tempDutyCycle;
                }
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetDeviceName(string deviceName)
        {
            Device device = DaqSystem.Local.LoadDevice(deviceName);
            if (device.BusType != DeviceBusType.CompactDaq)
                return deviceName;
            else
                return device.CompactDaqChassisDeviceName;
        }

        private void StartTask()
        {
            if (runningAnalogTask == null)
            {
                // Change state
                runningAnalogTask = analogInputTask;
                runningCounterTask = counterTask;

                startButton.Enabled = false;
                stopButton.Enabled = true;

                counterTask.Start();
                analogInputTask.Start();
            }
        }
        private void StopTask()
        {
            //Change State
            runningAnalogTask = null;
            runningCounterTask = null;
            
            startButton.Enabled = true;
            stopButton.Enabled = false;

            analogInputTask.Dispose();
            counterTask.Dispose();
        }
    }
}
