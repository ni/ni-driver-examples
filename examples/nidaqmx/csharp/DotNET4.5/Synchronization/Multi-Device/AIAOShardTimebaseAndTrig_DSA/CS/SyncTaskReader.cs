using System;
using System.ComponentModel;
using NationalInstruments.DAQmx;

namespace NationalInstruments.Examples.MultiDeviceSync_AIAOShardTimebaseAndTrig_DSA
{
    public enum SyncType
    {
        DsaSampleClock = 0,
        DsaReferenceClock = 1
    };

    /// <summary>
    /// This class is used for both the master and slave tasks
    /// in this example.
    ///
    /// Both the master and slave tasks are
    /// created, configured, and read in the same manner.
    /// The only difference is that you should call
    /// SynchronizeSlave on the slave task, and not on the
    /// master task.
    /// </summary>
    /// 

    public class SyncTaskReader
    {
        private Task myTask;
        private AnalogSingleChannelReader reader;
        private SyncType syncType;

        /// <summary>
        /// Create the Task and name it.
        /// </summary>
        public SyncTaskReader(string name, ISynchronizeInvoke syncObj, int sync)
        {
            myTask = new Task(name);
            reader = new AnalogSingleChannelReader(myTask.Stream);
            syncType = (SyncType)sync;

            // Use SynchronizeCallbacks to specify that the object 
            // marshals callbacks across threads appropriately.
            reader.SynchronizeCallbacks = true;
        }

        /// <summary>
        /// This overload simplifies configuring a task when
        /// the inputs come from text boxes on the UI.
        /// </summary>
        public void ConfigureDecimal(string physicalChannelName, decimal minimumValue, decimal maximumValue, decimal samplesPerChannel, decimal rate)
        {
            // Call the other overload to do all the work.
            Configure(physicalChannelName, Convert.ToDouble(minimumValue), Convert.ToDouble(maximumValue), Convert.ToInt32(samplesPerChannel),Convert.ToDouble(rate));
        }

        /// <summary>
        /// Configure the task by creating a channel
        /// and setting up the timing.
        /// </summary>
        public void Configure(string physicalChannelName, double minimumValue, double maximumValue, int samplesPerChannel, double rate)
        {
            // First, create a AI voltage channel.
            myTask.AIChannels.CreateVoltageChannel(physicalChannelName, "",
                (AITerminalConfiguration)(-1), // This asks the NI-DAQ driver to select
                                               // a configuration for you.
                minimumValue, maximumValue, AIVoltageUnits.Volts);

            // Next, configure the sample clock.
            myTask.Timing.ConfigureSampleClock("", // This selects the onboard clock
                rate, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, samplesPerChannel);
        }

        /// <summary>
        /// Synchronize the slave and master tasks by
        /// sharing the master clock timebase, sync pulse
        /// and create a start trigger on the slave.
        /// </summary>
        public void SynchronizeSlave(SyncTaskReader master)
        {
            // For an alternate way of synchronizing DSA devices, see the
            // ContAcqSndPressureSamples_IntClk example.
                                
            // First, verify the master task so we can query its properties.
            master.Task.Control(TaskAction.Verify);

            // Next, find out what device the master is using.
            // This is so we can build terminal strings using the
            // master device name.
            string firstPhysChanName;

            if (master.Task.AOChannels.Count > 0)
                firstPhysChanName = master.Task.AOChannels[0].PhysicalName;
            else
                firstPhysChanName = master.Task.AIChannels[0].PhysicalName;
            
            string deviceName = firstPhysChanName.Split('/')[0];
            string terminalNameBase = "/" + MainForm.GetDeviceName(deviceName) + "/";

            // Configure my (slave) timebase source and sync source
            // to use the same settings as that of the other task
            // passed in (master).
            myTask.Timing.SynchronizationPulseSource = terminalNameBase + "SyncPulse";

            switch (syncType)
            {
                case SyncType.DsaSampleClock:
                {
                    myTask.Timing.SampleClockTimebaseSource = terminalNameBase + "SampleClockTimebase";
                    break;
                }
                case SyncType.DsaReferenceClock:
                {
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

        public void Stop()
        {
            myTask.Stop();
        }

        public void Dispose()
        {
            myTask.Dispose();
        }

        /// <summary>
        /// Read the data.
        /// </summary>
        public void BeginRead(AsyncCallback callback, SyncTaskReader state)
        {
            reader.BeginReadMultiSample((int)myTask.Timing.SamplesPerChannel, callback, state);
        }

        public double[] EndRead(IAsyncResult ar)
        {
            double[] data = reader.EndReadMultiSample(ar);
            return data;
        }

        public Task Task
        {
            get
            {
                return myTask;
            }
        }
    }
}
