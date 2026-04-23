using System;
using NationalInstruments.DAQmx;

namespace NationalInstruments.Examples.AIFiniteAcquisition
{
    public enum SyncType
    {
        ESeries = 0,
        MSeries = 1,
        MSeriesPXI = 2,
        DsaSampleClock = 3,
        DsaReferenceClock = 4
    };

    /// <summary>
    /// This class is used for both the master and slave tasks
    /// in this example.
    ///
    /// Both the master and slave taks are
    /// created, configured, and read in the same manner.
    /// The only difference is that you should call
    /// SynchronizeSlave on the slave task, and not on the
    /// master task.
    /// </summary>
    public class SyncTask
    {
        private Task myTask;
        private AnalogMultiChannelReader reader;
        private SyncType syncType;
        private System.Data.DataColumn[] dataColumn;
        private System.Data.DataTable dataTable = new System.Data.DataTable();

        /// <summary>
        /// Create the Task and name it.
        /// </summary>
        public SyncTask(string name, int sync)
        {
            myTask = new Task(name);
            reader = new AnalogMultiChannelReader(myTask.Stream);
            syncType = (SyncType)sync;
        }

        /// <summary>
        /// This overload simplifies configuring a task when
        /// the inputs come from text boxes on the UI.
        /// </summary>
        public void ConfigureDecimal(string physicalChannelName, decimal minimumValue, decimal maximumValue, decimal samplesPerChannel, decimal rate)
        {
            // Call the other overload to do all the work.
            Configure(physicalChannelName,Convert.ToDouble(minimumValue), Convert.ToDouble(maximumValue), Convert.ToInt32(samplesPerChannel), Convert.ToDouble(rate));
        }

        /// <summary>
        /// Configure the task by creating a channel
        /// and setting up the timing.
        /// </summary>
        public void Configure(string physicalChannelName, double minimumValue, double maximumValue, int samplesPerChannel, double rate)
        {
            // First, create a AI voltage channel.
            myTask.AIChannels.CreateVoltageChannel(physicalChannelName, 
                "",
                (AITerminalConfiguration)(-1), // This asks the NI-DAQ driver to select
                                               // a configuration for you.
                minimumValue, 
                maximumValue, 
                AIVoltageUnits.Volts);

            // Next, configure the sample clock.
            myTask.Timing.ConfigureSampleClock(
                "", // This selects the onboard clock
                rate, 
                SampleClockActiveEdge.Rising, 
                SampleQuantityMode.FiniteSamples, 
                samplesPerChannel);

            // Setup the DataTable based on the channels in the task.
            InitializeDataTable(samplesPerChannel);
        }

        public void SynchronizeMaster()
        {
            switch(syncType)
            {
                case SyncType.ESeries:
                {
                    // E-Series Synchronization
                    break;
                }
                case SyncType.MSeries:
                {
                    // M-Series PCI Synchronization
                    myTask.Timing.ReferenceClockSource = "OnboardClock";
                    break;
                }
                case SyncType.MSeriesPXI:
                {
                    // M-Series PXI Synchronization
                    myTask.Timing.ReferenceClockSource = "PXI_Clk10";
                    myTask.Timing.ReferenceClockRate = 10000000;
                    break;
                }
                case SyncType.DsaSampleClock:
                {
                    // DSA Sample Clock Synchronization

                    // For an alternate way of synchronizing DSA devices, see the
                    // ContAcqSndPressureSamples_IntClk example.
                    break;
                }
                case SyncType.DsaReferenceClock:
                {
                    // DSA Reference Clock Synchronization

                    // For an alternate way of synchronizing DSA devices, see the
                    // ContAcqSndPressureSamples_IntClk example.
                    myTask.Timing.ReferenceClockSource = "PXI_Clk10";
                    break;   
                }
            }
        }

        public void SynchronizeSlave(SyncTask master)
        {
            // First, verify the master task so we can query its properties.
            master.Task.Control(TaskAction.Verify);

            // Next, find out what device the master is using.
            // This is so we can build terminal strings using the
            // master device name.
            string firstPhysChanName = master.Task.AIChannels[0].PhysicalName;
            string deviceName = firstPhysChanName.Split('/')[0];
            string terminalNameBase = "/" + GetDeviceName(deviceName) + "/";

            // Depending on what kind of device, synchronize accordingly
            switch(syncType)
            {
                case SyncType.ESeries:
                {
                    // E-Series Synchronization
                    myTask.Timing.MasterTimebaseSource = master.Task.Timing.MasterTimebaseSource;
                    myTask.Timing.MasterTimebaseRate = master.Task.Timing.MasterTimebaseRate;
                    break;
                }
                case SyncType.MSeries:
                {
                    // M-Series PCI Synchronization
                    myTask.Timing.ReferenceClockSource = master.Task.Timing.ReferenceClockSource;
                    myTask.Timing.ReferenceClockRate = master.Task.Timing.ReferenceClockRate;
                    break;
                }
                case SyncType.MSeriesPXI:
                {
                    // M-Series PXI Synchronization
                    myTask.Timing.ReferenceClockSource = "PXI_Clk10";
                    myTask.Timing.ReferenceClockRate = 10000000;
                    break;
                }
                case SyncType.DsaSampleClock:
                {
                    // DSA Sample Clock Synchronization
                    
                    // For an alternate way of synchronizing DSA devices, see the
                    // ContAcqSndPressureSamples_IntClk example.
                    myTask.Timing.SampleClockTimebaseSource = terminalNameBase + "SampleClockTimebase";
                    myTask.Timing.SynchronizationPulseSource = terminalNameBase + "SyncPulse";
                    break;
                }
                case SyncType.DsaReferenceClock:
                {
                    // DSA Reference Clock Synchronization

                    // For an alternate way of synchronizing DSA devices, see the
                    // ContAcqSndPressureSamples_IntClk example.
                    myTask.Timing.SynchronizationPulseSource = terminalNameBase + "SyncPulse";
                    myTask.Timing.ReferenceClockSource = "PXI_Clk10";
                    break;
                }
            }

            // Configure a digital edge start trigger so both tasks
            // start together.
            myTask.Triggers.StartTrigger.ConfigureDigitalEdgeTrigger(
                terminalNameBase + "ai/StartTrigger",
                DigitalEdgeStartTriggerEdge.Rising);
        }

        public void Start()
        {
            myTask.Start();
        }

        public void Dispose()
        {
            myTask.Dispose();
        }

        /// <summary>
        /// Read the data.
        /// </summary>
        public double[,] Read()
        {
            double[,] data = reader.ReadMultiSample(-1);
            DataToDataTable(data);
            return data;
        }

        /// <summary>
        /// Returns the Data Table associated with this Task.
        /// </summary>
        public System.Data.DataTable DataTable
        {
            get
            {
                return dataTable;
            }
        }

        /// <summary>
        /// Private helper method that adds newly acquired data to the
        /// table.
        /// </summary>
        private void DataToDataTable(double[,] sourceArray)
        {   
            for(int currentChannelIndex=0;currentChannelIndex<sourceArray.GetLength(0);currentChannelIndex++)
            {
                for(int currentDataIndex = 0;currentDataIndex<sourceArray.GetLength(1);currentDataIndex++)
                {
                    dataTable.Rows[currentDataIndex][currentChannelIndex] = sourceArray[currentChannelIndex,currentDataIndex];
                }
            }
        }

        /// <summary>
        /// Private helper method that clears the table and adds a
        /// column for each channel in the Task.
        /// </summary>
        private void InitializeDataTable(int rows)
        {
            // First, verify the master task so we can query its properties.
            myTask.Control(TaskAction.Verify);

            int numOfChannels = myTask.AIChannels.Count;
            dataTable.Rows.Clear();
            dataTable.Columns.Clear();
            dataColumn = new System.Data.DataColumn[numOfChannels];

            for(int currentChannelIndex=0;currentChannelIndex<numOfChannels;currentChannelIndex++)
            {   
                dataColumn[currentChannelIndex] = new System.Data.DataColumn();
                dataColumn[currentChannelIndex].DataType = typeof(double);
                dataColumn[currentChannelIndex].ColumnName= myTask.AIChannels[currentChannelIndex].PhysicalName;
            }

            dataTable.Columns.AddRange(dataColumn); 

            for(int rowIndex = 0;rowIndex<rows;rowIndex++)             
            {
                object[] rowArr = new object[numOfChannels];
                dataTable.Rows.Add(rowArr);              
            }        
        }

        private string GetDeviceName(string deviceName)
        {
            Device device = DaqSystem.Local.LoadDevice(deviceName);
            if (device.BusType != DeviceBusType.CompactDaq)
                return deviceName;
            else
                return device.CompactDaqChassisDeviceName;
        }

        private Task Task
        {
            get
            {
                return myTask;
            }
        }
    }
}
