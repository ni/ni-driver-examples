'******************************************************************************
'*
'* Example program:
'*   RFSA Synchronization T-Clock
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'*   Use this example to learn the basics of TClk synchronization for RF vector
'	signal analyzers. This example shows how to handle multiple sessions of
'	NI-RFSA devices and pass them to NI-TClk synchronization functions. It uses
'	NI-TClk to make all the NI-RFSA devices start acquiring data at the same time.
'
'
'* Instructions for running:
'*   1. Configure both RFSA device in the MAX for the program to run.
'*
'*	 2. Configure the Reference Level, Carrier Frequency and IQ Rate in the UI.
'*
'*   3. Configure the Trigger Setting in Trigger Slope and Trigger Level.
'*
'*	 4. Configure the Clock for both Master and Slave.
'*
'*   5. Select the Acquire Button in the UI to start the acquisition.
'*
'*   6. The data is displayed in the DataGrid.
'*
'* I/O Connections Overview:
'*   Make sure your signal input terminals match the Physical Channel I/O
'*   Controls.  If you have a PXI chassis, ensure that it has been properly
'*   identified in MAX.
'*
'******************************************************************************
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsa
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices
Imports NationalInstruments.ModularInstruments.SystemServices.TimingServices


Partial Public Class MainForm
    Inherits Form
    Private sessions As NIRfsa()
    Private loFrequency As Double
    Private dataGridResults As DataGridView() = New DataGridView(1) {}

    Public Sub New()
        InitializeComponent()
        ConfigureRefClockMasterComboBox()
        ConfigureRefClockExpMasterComboBox()
        ConfigureRefClockSlaveComboBox()
        ConfigureRefClockExpSlaveComboBox()

        LoadRfsaDeviceNames()

        dataGridResults(0) = Me.dataGridViewResultsId0
        dataGridResults(1) = Me.dataGridViewResultsId1

    End Sub

#Region "UI Initial Value Config Section"

    Private Sub ConfigureRefClockMasterComboBox()
        Dim refClockMasterValueList As New List(Of KeyValuePair(Of String, RfsaReferenceClockSource))()
        refClockMasterValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("OnboardClock", RfsaReferenceClockSource.OnboardClock))
        refClockMasterValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("RefIn", RfsaReferenceClockSource.ReferenceIn))
        refClockMasterValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("PXI_Clk", RfsaReferenceClockSource.PxiClock))
        refClockMasterValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("ClkIn", RfsaReferenceClockSource.ClockIn))
        referenceClockMasterComboBox.DisplayMember = "Key"
        referenceClockMasterComboBox.ValueMember = "Value"
        referenceClockMasterComboBox.DataSource = refClockMasterValueList
        referenceClockMasterComboBox.SelectedIndex = 0
    End Sub



    Private Sub ConfigureRefClockExpMasterComboBox()
        Dim refClockExpMasterValueList As New List(Of KeyValuePair(Of String, RfsaExportReferenceClockExportedOutputTerminal))()
        refClockExpMasterValueList.Add(New KeyValuePair(Of String, RfsaExportReferenceClockExportedOutputTerminal)("RefOut", RfsaExportReferenceClockExportedOutputTerminal.ReferenceOut))
        refClockExpMasterValueList.Add(New KeyValuePair(Of String, RfsaExportReferenceClockExportedOutputTerminal)("RefOut2", RfsaExportReferenceClockExportedOutputTerminal.ReferenceOut2))
        refClockExpMasterValueList.Add(New KeyValuePair(Of String, RfsaExportReferenceClockExportedOutputTerminal)("ClkOut", RfsaExportReferenceClockExportedOutputTerminal.ClockOut))
        refClockExpMasterValueList.Add(New KeyValuePair(Of String, RfsaExportReferenceClockExportedOutputTerminal)("None", RfsaExportReferenceClockExportedOutputTerminal.None))
        referenceClockExportMasterComboBox.DisplayMember = "Key"
        referenceClockExportMasterComboBox.ValueMember = "Value"
        referenceClockExportMasterComboBox.DataSource = refClockExpMasterValueList
        referenceClockExportMasterComboBox.SelectedIndex = 2
    End Sub



    Private Sub ConfigureRefClockSlaveComboBox()
        Dim refClockSlaveValueList As New List(Of KeyValuePair(Of String, RfsaReferenceClockSource))()
        refClockSlaveValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("OnboardClock", RfsaReferenceClockSource.OnboardClock))
        refClockSlaveValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("RefIn", RfsaReferenceClockSource.ReferenceIn))
        refClockSlaveValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("PXI_Clk", RfsaReferenceClockSource.PxiClock))
        refClockSlaveValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("ClkIn", RfsaReferenceClockSource.ClockIn))
        referenceClockSlaveComboBox.DisplayMember = "Key"
        referenceClockSlaveComboBox.ValueMember = "Value"
        referenceClockSlaveComboBox.DataSource = refClockSlaveValueList
        referenceClockSlaveComboBox.SelectedIndex = 3
    End Sub



    Private Sub ConfigureRefClockExpSlaveComboBox()
        Dim refClockExpSlaveValueList As New List(Of KeyValuePair(Of String, RfsaExportReferenceClockExportedOutputTerminal))()
        refClockExpSlaveValueList.Add(New KeyValuePair(Of String, RfsaExportReferenceClockExportedOutputTerminal)("RefOut", RfsaExportReferenceClockExportedOutputTerminal.ReferenceOut))
        refClockExpSlaveValueList.Add(New KeyValuePair(Of String, RfsaExportReferenceClockExportedOutputTerminal)("RefOut2", RfsaExportReferenceClockExportedOutputTerminal.ReferenceOut2))
        refClockExpSlaveValueList.Add(New KeyValuePair(Of String, RfsaExportReferenceClockExportedOutputTerminal)("ClkOut", RfsaExportReferenceClockExportedOutputTerminal.ClockOut))
        refClockExpSlaveValueList.Add(New KeyValuePair(Of String, RfsaExportReferenceClockExportedOutputTerminal)("None", RfsaExportReferenceClockExportedOutputTerminal.None))
        referenceClockExportSlaveComboBox.DisplayMember = "Key"
        referenceClockExportSlaveComboBox.ValueMember = "Value"
        referenceClockExportSlaveComboBox.DataSource = refClockExpSlaveValueList
        referenceClockExportSlaveComboBox.SelectedIndex = 2
    End Sub

#End Region

    Private Sub LoadRfsaDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-RFSA")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            resourceNameMasterComboBox.Items.Add(device.Name)
            resourceNameSlaveComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            resourceNameMasterComboBox.SelectedIndex = 0
            resourceNameSlaveComboBox.SelectedIndex = 0
        End If
    End Sub


    Private ReadOnly Property NumberOfSession() As Integer
        Get
            Return Decimal.ToInt32(numberOfDevicesNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property MasterResourceName() As String
        Get
            Return resourceNameMasterComboBox.Text
        End Get
    End Property

    Private ReadOnly Property SlaveResourceName() As String
        Get
            Return resourceNameSlaveComboBox.Text
        End Get
    End Property

    Private ReadOnly Property IQRate() As Double
        Get
            Return Decimal.ToDouble(Me.iqRateNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property ReferenceLevel() As Double
        Get
            Return Decimal.ToDouble(Me.referenceLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CarrierFrequency() As Double
        Get
            Return Decimal.ToDouble(Me.carrierFrequencyNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property NumberOfSamples() As Integer
        Get
            Return Decimal.ToInt32(Me.samplesPerRecordNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property MasterReferenceClockSource() As RfsaReferenceClockSource
        Get
            Return If(TryCast(Me.referenceClockMasterComboBox.SelectedValue, RfsaReferenceClockSource), RfsaReferenceClockSource.FromString(Me.referenceClockMasterComboBox.Text))
        End Get
    End Property

    Private ReadOnly Property SlaveReferenceClockSource() As RfsaReferenceClockSource
        Get
            Return If(TryCast(Me.referenceClockSlaveComboBox.SelectedValue, RfsaReferenceClockSource), RfsaReferenceClockSource.FromString(Me.referenceClockSlaveComboBox.Text))
        End Get
    End Property

    Private ReadOnly Property MasterRefClockExport() As RfsaExportReferenceClockExportedOutputTerminal
        Get
            Return If(TryCast(Me.referenceClockExportMasterComboBox.SelectedValue, RfsaExportReferenceClockExportedOutputTerminal), RfsaExportReferenceClockExportedOutputTerminal.FromString(Me.referenceClockExportMasterComboBox.Text))


        End Get
    End Property

    Private ReadOnly Property SlaveRefClockExport() As RfsaExportReferenceClockExportedOutputTerminal
        Get
            Return If(TryCast(Me.referenceClockExportSlaveComboBox.SelectedValue, RfsaExportReferenceClockExportedOutputTerminal), RfsaExportReferenceClockExportedOutputTerminal.FromString(Me.referenceClockExportSlaveComboBox.Text))


        End Get
    End Property

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles acquireButton.Click
        ChangeControlState(False)
        Try
            ConfigureSynchronizeAcquire()
        Catch ex As System.Exception
            ShowError(ex.Message)
            For i As Integer = 0 To NumberOfSession - 1
                CloseRfsaSession(sessions(i))
            Next
        Finally
            ChangeControlState(True)
        End Try
    End Sub


    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.acquireButton.Enabled = isEnabled
        Me.resourceNameMasterComboBox.Enabled = isEnabled
        Me.resourceNameSlaveComboBox.Enabled = isEnabled
        Me.referenceLevelNumeric.Enabled = isEnabled
        Me.carrierFrequencyNumeric.Enabled = isEnabled
        Me.iqRateNumeric.Enabled = isEnabled
        Me.samplesPerRecordNumeric.Enabled = isEnabled
        Me.referenceClockMasterComboBox.Enabled = isEnabled
        Me.referenceClockExportMasterComboBox.Enabled = isEnabled
        Me.referenceClockSlaveComboBox.Enabled = isEnabled
        Me.referenceClockExportSlaveComboBox.Enabled = isEnabled
    End Sub

    Private Sub ConfigureSynchronizeAcquire()

        PrepareDeviceSessions()
        For i As Integer = 0 To NumberOfSession - 1
            IntializeRfsaSession(i)
            ConfigureForIQ(i)
            ConfigureRefClock(i)
        Next

        Dim tClockSession As New TClock(sessions(0), sessions(1))
        Dim minTime As New PrecisionTimeSpan(0)
        tClockSession.ConfigureForHomogeneousTriggers()
        tClockSession.Synchronize(minTime)
        tClockSession.Initiate()

        For i As Integer = 0 To NumberOfSession - 1
            FetchIQData(i)
        Next
        For i As Integer = 0 To NumberOfSession - 1
            CloseRfsaSession(sessions(i))
        Next
    End Sub

    Private Sub FetchIQData(ByVal i As Integer)
        Dim wfmInfo As RfsaWaveformInfo
        Dim data As ComplexDouble()
        Dim timespan As New PrecisionTimeSpan(10.0)
        data = sessions(i).Acquisition.IQ.FetchIQSingleRecordComplex(Of ComplexDouble)(0, NumberOfSamples, timespan, wfmInfo)
        Me.dataGridResults(i).DataSource = data
        Me.Refresh()
    End Sub

    Private Sub ConfigureRefClock(ByVal sessionIndex As Integer)
        Dim clockSource As RfsaReferenceClockSource
        Dim clockExport As RfsaExportReferenceClockExportedOutputTerminal
        Dim _model As String
        Dim _referenceClockRate As Double
        If sessionIndex = 0 Then
            clockSource = MasterReferenceClockSource
            clockExport = MasterRefClockExport
        Else
            clockSource = SlaveReferenceClockSource
            clockExport = SlaveRefClockExport
        End If

        sessions(sessionIndex).Configuration.ReferenceClock.Source = clockSource
        sessions(sessionIndex).Configuration.ReferenceClock.Export.OutputTerminal = clockExport

    End Sub

    Private Sub ConfigureForIQ(ByVal sessionIndex As Integer)
        sessions(sessionIndex).Configuration.Vertical.ReferenceLevel = ReferenceLevel
        sessions(sessionIndex).Configuration.AcquisitionType = RfsaAcquisitionType.IQ
        sessions(sessionIndex).Configuration.IQ.CarrierFrequency = CarrierFrequency
        sessions(sessionIndex).Configuration.IQ.NumberOfSamples = NumberOfSamples
        sessions(sessionIndex).Configuration.IQ.NumberOfSamplesIsFinite = True
        sessions(sessionIndex).Configuration.IQ.IQRate = IQRate
    End Sub

    Private Sub IntializeRfsaSession(ByVal sessionIndex As Integer)
        CloseRfsaSession(sessions(sessionIndex))

        If sessionIndex = 0 Then
            sessions(sessionIndex) = New NIRfsa(MasterResourceName, True, False)
            AddHandler sessions(sessionIndex).DriverOperation.Warning, New EventHandler(Of RfsaWarningEventArgs)(AddressOf Me.DriverOperationWarning)
        Else
            sessions(sessionIndex) = New NIRfsa(SlaveResourceName, True, False)
            AddHandler sessions(sessionIndex).DriverOperation.Warning, New EventHandler(Of RfsaWarningEventArgs)(AddressOf Me.DriverOperationWarning)
        End If

    End Sub

    Private Sub DriverOperationWarning(ByVal o As Object, ByVal e As RfsaWarningEventArgs)
        MessageBox.Show(e.Warning.ToString(), "Warning")
    End Sub

    Private Shared Sub CloseRfsaSession(ByVal rfsaSession As NIRfsa)
        If rfsaSession IsNot Nothing Then
            Try
                rfsaSession.Close()
                rfsaSession = Nothing
            Catch ex As Exception
                ShowError(("Unable to Close Session, Reset the device." & vbLf & "Error : ") + ex.Message)
                Application.[Exit]()
            End Try
        End If
    End Sub

    Private Sub PrepareDeviceSessions()
        sessions = New NIRfsa(NumberOfSession - 1) {}

        For i As Integer = 0 To NumberOfSession - 1
            sessions(i) = Nothing
        Next
    End Sub

    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")
    End Sub
End Class
