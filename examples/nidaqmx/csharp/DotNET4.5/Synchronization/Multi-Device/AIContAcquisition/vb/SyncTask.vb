Public Enum SyncType
    ESeries = 0
    MSeries = 1
    MSeriesPXI = 2
    DsaSampleClock = 3
    DsaReferenceClock = 4
End Enum

''' <summary>
''' This class is used for both the master and slave tasks
''' in this example.
'''
''' Both the master and slave tasks are
''' created, configured, and read in the same manner.
''' The only difference is that you should call
''' SynchronizeSlave on the slave task, and not on the
''' master task.
''' </summary>
Public Class SyncTask

    Private myTask As Task
    Private reader As AnalogMultiChannelReader
    Private synchronizationType As SyncType
    Private dataColumn() As System.Data.DataColumn
    Private myDataTable As New System.Data.DataTable

    ' Create the Task and name it.
    Public Sub New(ByVal name As String, ByVal syncObj As ISynchronizeInvoke, ByVal sync As Integer)
        myTask = New Task(name)
        reader = New AnalogMultiChannelReader(myTask.Stream)
        synchronizationType = CType(sync, SyncType)

        ' Use SynchronizeCallbacks to specify that the object 
        ' marshals callbacks across threads appropriately.
        reader.SynchronizeCallbacks = True


    End Sub 'New


    ' This overload simplifies configuring a task when
    ' the inputs come from text boxes on the UI.
    Public Sub ConfigureDecimal(ByVal physicalChannelName As String, ByVal minimumValue As Decimal, ByVal maximumValue As Decimal, ByVal samplesPerChannel As Decimal, ByVal rate As Decimal)
        ' Call the other overload to do all the work.
        Configure(physicalChannelName, Convert.ToDouble(minimumValue), Convert.ToDouble(maximumValue), Convert.ToInt32(samplesPerChannel), Convert.ToDouble(rate))
    End Sub 'ConfigureDecimal


    ' Configure the task by creating a channel
    ' and setting up the timing.
    Public Sub Configure(ByVal physicalChannelName As String, ByVal minimumValue As Double, ByVal maximumValue As Double, ByVal samplesPerChannel As Integer, ByVal rate As Double)
        ' First, create a AI voltage channel.
        myTask.AIChannels.CreateVoltageChannel(physicalChannelName, "", CType(-1, AITerminalConfiguration), minimumValue, maximumValue, AIVoltageUnits.Volts)

        ' Next, configure the sample clock.
        myTask.Timing.ConfigureSampleClock("", rate, SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, samplesPerChannel)

        ' Setup the DataTable based on the channels in the task.
        InitializeDataTable(samplesPerChannel)
    End Sub 'Configure


    Public Sub SynchronizeMaster()
        Select Case synchronizationType
            Case SyncType.ESeries
                ' E-Series Synchronization

            Case SyncType.MSeries
                ' M-Series PCI Synchronization
                myTask.Timing.ReferenceClockSource = "OnboardClock"

            Case SyncType.MSeriesPXI
                ' M-Series PXI Synchronization
                myTask.Timing.ReferenceClockSource = "PXI_Clk10"
                myTask.Timing.ReferenceClockRate = 10000000

            Case SyncType.DsaSampleClock
                ' DSA Sample Clock Synchronization

                ' For an alternate way of synchronizing DSA devices, see the
                ' ContAcqSndPressureSamples_IntClk example.
            Case SyncType.DsaReferenceClock
                ' DSA Reference Clock Synchronization

                ' For an alternate way of synchronizing DSA devices, see the
                ' ContAcqSndPressureSamples_IntClk example.
                myTask.Timing.ReferenceClockSource = "PXI_Clk10"
        End Select
    End Sub

    Public Sub SynchronizeSlave(ByVal master As SyncTask)
        ' First, verify the master task so we can query its properties.
        master.Task.Control(TaskAction.Verify)

        ' Next, find out what device the master is using.
        ' This is so we can build terminal strings using the
        ' master device name.
        Dim firstPhysChanName As String = master.Task.AIChannels(0).PhysicalName
        Dim deviceName As String = firstPhysChanName.Split("/"c)(0)
        Dim terminalNameBase As String = "/" + GetDeviceName(deviceName) + "/"

        Select Case synchronizationType
            Case SyncType.ESeries
                ' E-Series Synchronization
                myTask.Timing.MasterTimebaseSource = master.Task.Timing.MasterTimebaseSource
                myTask.Timing.MasterTimebaseRate = master.Task.Timing.MasterTimebaseRate

            Case SyncType.MSeries
                ' M-Series PCI Synchronization
                myTask.Timing.ReferenceClockSource = master.Task.Timing.ReferenceClockSource
                myTask.Timing.ReferenceClockRate = master.Task.Timing.ReferenceClockRate

            Case SyncType.MSeriesPXI
                ' M-Series PXI Synchronization
                myTask.Timing.ReferenceClockSource = "PXI_Clk10"
                myTask.Timing.ReferenceClockRate = 10000000

            Case SyncType.DsaSampleClock
                ' DSA Sample Clock Synchronization

                ' For an alternate way of synchronizing DSA devices, see the
                ' ContAcqSndPressureSamples_IntClk example.
                myTask.Timing.SampleClockTimebaseSource = terminalNameBase + "SampleClockTimebase"
                myTask.Timing.SynchronizationPulseSource = terminalNameBase + "SyncPulse"

            Case SyncType.DsaReferenceClock
                ' DSA Reference Clock Synchronization

                ' For an alternate way of synchronizing DSA devices, see the
                ' ContAcqSndPressureSamples_IntClk example.
                myTask.Timing.SynchronizationPulseSource = terminalNameBase + "SyncPulse"
                myTask.Timing.ReferenceClockSource = "PXI_Clk10"
        End Select

        ' Configure a digital edge start trigger so both tasks
        ' start together.
        myTask.Triggers.StartTrigger.ConfigureDigitalEdgeTrigger(terminalNameBase + "ai/StartTrigger", _
            DigitalEdgeStartTriggerEdge.Rising)
    End Sub 'SynchronizeSlave

    Public Sub Start()
        myTask.Start()
    End Sub 'Start

    Public Sub Dispose()
        myTask.Dispose()
    End Sub

    ' Read the data.
    Public Sub BeginRead(ByVal callback As AsyncCallback, ByVal state As SyncTask)
        reader.BeginReadMultiSample(Convert.ToInt32(myTask.Timing.SamplesPerChannel), callback, state)
    End Sub 'Read
    Public Function EndRead(ByVal ar As IAsyncResult) As Double(,)
        Dim data As Double(,) = reader.EndReadMultiSample(ar)
        Return data
    End Function

    ' Returns the Data Table associated with this Task.
    Public ReadOnly Property DataTable() As System.Data.DataTable
        Get
            Return myDataTable
        End Get
    End Property

    ' Private helper method that clears the table and adds a
    ' column for each channel in the Task.
    Private Sub InitializeDataTable(ByVal numberOfSamples As Integer)
        ' First, verify the master task so we can query its properties.
        myTask.Control(TaskAction.Verify)

        Dim numOfChannels As Integer = myTask.AIChannels.Count
        Dim rows As Integer = 0
        myDataTable.Rows.Clear()
        myDataTable.Columns.Clear()
        dataColumn = New System.Data.DataColumn(numOfChannels - 1) {}

        Dim currentChannelIndex As Integer
        For currentChannelIndex = 0 To numOfChannels - 1
            dataColumn(currentChannelIndex) = New System.Data.DataColumn
            dataColumn(currentChannelIndex).DataType = GetType(Double)
            dataColumn(currentChannelIndex).ColumnName = myTask.AIChannels(currentChannelIndex).PhysicalName
        Next currentChannelIndex

        myDataTable.Columns.AddRange(dataColumn)

        If numberOfSamples < 23 Then
            rows = numberOfSamples
        Else
            rows = 23
        End If

        Dim rowIndex As Integer
        For rowIndex = 0 To rows - 1
            Dim rowArr(numOfChannels - 1) As Object
            myDataTable.Rows.Add(rowArr)
        Next rowIndex
    End Sub 'InitializeDataTable

    Private Function GetDeviceName(ByVal deviceName As String) As String
        Dim device As device = DaqSystem.Local.LoadDevice(deviceName)
        If (device.BusType <> DeviceBusType.CompactDaq) Then
            Return deviceName
        Else
            Return device.CompactDaqChassisDeviceName
        End If
    End Function 'GetDeviceName

    Private ReadOnly Property Task() As Task
        Get
            Return myTask
        End Get
    End Property

End Class 'SyncTask
