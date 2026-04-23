Imports System
Imports System.ComponentModel
Imports NationalInstruments.DAQmx

Public Class SyncTaskWriter

    Private myTask As Task
    Private writer As AnalogSingleChannelWriter
    Private syncType As SType

    Public Sub New(ByVal name As String, ByVal syncObj As ISynchronizeInvoke, ByVal sync As SType)
        myTask = New Task(name)
        writer = New AnalogSingleChannelWriter(myTask.Stream)
        syncType = CType(sync, SType)

        ' Use SynchronizeCallbacks to specify that the object 
        ' marshals callbacks across threads appropriately.
        writer.SynchronizeCallbacks = True

    End Sub 'New

    Public Sub ConfigureDecimal(ByVal physicalChannelName As String, ByVal minimumValue As Decimal, ByVal maximumValue As Decimal, ByVal samplesPerChannel As Decimal, ByVal rate As Decimal)
        ' Call the other overload to do all the work.
        Configure(physicalChannelName, Convert.ToDouble(minimumValue), Convert.ToDouble(maximumValue), Convert.ToInt32(samplesPerChannel), Convert.ToDouble(rate))
    End Sub 'ConfigureDecimal

    Public Sub Configure(ByVal physicalChannelName As String, ByVal minimumValue As Double, ByVal maximumValue As Double, ByVal samplesPerChannel As Integer, ByVal rate As Double)
        ' First, create a AI voltage channel.
        myTask.AOChannels.CreateVoltageChannel(physicalChannelName, "", minimumValue, maximumValue, AOVoltageUnits.Volts)

        ' Next, configure the sample clock.
        myTask.Timing.ConfigureSampleClock("", rate, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, samplesPerChannel) ' This selects the onboard clock
    End Sub 'Configure

    Public Sub SynchronizeSlave(ByVal master As SyncTaskReader)
        ' For an alternate way of synchronizing DSA devices, see the
        ' ContAcqSndPressureSamples_IntClk example.

        ' First, verify the master task so we can query its properties.
        master.Task.Control(TaskAction.Verify)

        ' Next, find out what device the master is using.
        ' This is so we can build terminal strings using the
        ' master device name.
        Dim firstPhysChanName As String

        If master.Task.AOChannels.Count > 0 Then
            firstPhysChanName = master.Task.AOChannels(0).PhysicalName
        Else
            firstPhysChanName = master.Task.AIChannels(0).PhysicalName
        End If
        Dim deviceName As String = firstPhysChanName.Split("/"c)(0)
        Dim terminalNameBase As String = "/" + MainForm.GetDeviceName(deviceName) + "/"

        ' Configure my (slave) timebase source and sync source
        ' to use the same settings as that of the other task
        ' passed in (master).
        myTask.Timing.SynchronizationPulseSource = terminalNameBase + "SyncPulse"

        Select Case syncType
            Case SType.DsaSampleClock
                myTask.Timing.SampleClockTimebaseSource = terminalNameBase + "SampleClockTimebase"
            Case SType.DsaReferenceClock
                myTask.Timing.ReferenceClockSource = "PXI_Clk10"
        End Select

        ' Configure a digital edge start trigger so both tasks
        ' start together.
        myTask.Triggers.StartTrigger.ConfigureDigitalEdgeTrigger(terminalNameBase + "ai/StartTrigger", DigitalEdgeStartTriggerEdge.Rising)
    End Sub 'SynchronizeSlave

    Public Sub Start()
        myTask.Start()
    End Sub 'Start

    Public Sub StopTask()
        myTask.Stop()
    End Sub 'StopTask

    Public Sub Dispose()
        myTask.Dispose()
    End Sub

    Public Sub BeginWrite(ByVal data() As Double)
        writer.WriteMultiSample(False, data)
    End Sub 'BeginWrite
End Class 'SyncTaskWriter