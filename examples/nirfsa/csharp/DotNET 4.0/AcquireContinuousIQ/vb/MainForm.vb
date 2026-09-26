'******************************************************************************
'*
'* Example program:
'*   Acquire Continuous IQ 
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'*   Use this example to learn how to acquire I/Q data using the RF vector
'*   signal analyzer. The example shows how to configure NI-RFSA for infinite I/Q
'*	 acquisition, how to set the carrier frequency and the I/Q rate, and how to
'*	 fetch I/Q data continuously.
'*
'* Instructions for running:
'*   1. Configure RFSA device in the MAX for the program to run. 
'*
'*	 2. Configure the Reference Level in the UI. 
'*		The reference level represents the maximum expected power of an input RF signal.
'*
'*	 3. Configure the Carrier Frequency in the UI.
'*
'*	 4. COnfigure the IQ Rate and the Samples to Read Per Block in the UI.
'*
'*   5. Configure the IQ Rate and Samples Per Record in the UI.
'*
'*	 6. Select Start Acquisition Button for RFSA to start acquiring the data.
'*	
'*   7. This is an example of continuous acquisition and user has to use the Stop Button to stop the
'*      the acquisition.
'*		
'*  Note:This is a multithreaded example in .NET.
'*  
'* I/O Connections Overview:
'*   Make sure your signal input terminals match the Physical Channel I/O
'*   Controls.  If you have a PXI chassis, ensure that it has been properly
'*   identified in MAX.  
'*
'******************************************************************************

Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsa
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices
Imports System.Threading

Partial Public Class MainForm
    Inherits Form
    Private rfsaSession As NIRfsa
    Private workerThread As Thread
    Private totalNumberOfSamples As Long = 0
    Private threadStop As Boolean = False
    Private closeRequested As Boolean = False
    Private lockobj As New Object()

    Private Property ThreadStopMarker() As Boolean
        Get
            SyncLock lockobj
                Return threadStop
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            SyncLock lockobj
                threadStop = Value
            End SyncLock
        End Set
    End Property

    Public Sub New()
        InitializeComponent()
        LoadRfsaDeviceNames()
    End Sub

    Private Sub LoadRfsaDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-RFSA")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            resourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub

#Region "Values From the UI"

    Private ReadOnly Property ResourceName() As String
        Get
            Return Me.resourceNameComboBox.Text
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

    Private ReadOnly Property IQRate() As Double
        Get
            Return Decimal.ToDouble(Me.iqRateNumeric.Value)
        End Get
    End Property

#End Region

    Private Sub InitializeRfsaSession()
        ' Close and Initiate a New Rfsa Session.
        CloseSession()
        rfsaSession = New NIRfsa(ResourceName, True, False)
        AddHandler rfsaSession.DriverOperation.Warning, New EventHandler(Of RfsaWarningEventArgs)(AddressOf Me.DriverOperationWarning)

    End Sub

    Private Sub CloseSession()
        'Closes the NiRfsa Session.
        If rfsaSession IsNot Nothing Then
            Try
                rfsaSession.Close()
                rfsaSession = Nothing
            Catch ex As Exception
                ShowError("Unable to Close Session, Reset the device." & vbLf & "Error : " & ex.Message)
                Application.[Exit]()
            End Try
        End If
    End Sub

    Private Shared Sub ShowError(ByVal message As String)
        MessageBox.Show(message, "Error")
    End Sub

    Private Sub ConfigureIQ()
        ' Configure various IQ Property for Acquisition.
        ' Number Of Samples is kept Infinite for Continuous Acquisition.
        rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ
        rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel
        rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFrequency
        rfsaSession.Configuration.IQ.NumberOfSamples = NumberOfSamples
        rfsaSession.Configuration.IQ.NumberOfSamplesIsFinite = False
        rfsaSession.Configuration.IQ.IQRate = IQRate
    End Sub

    Private Sub InitiateAcquisition()
        rfsaSession.Acquisition.IQ.Initiate()
    End Sub

    Private Sub ChangeControlState(ByVal state As Boolean)
        Me.resourceNameComboBox.Enabled = state
        Me.referenceLevelNumeric.Enabled = state
        Me.carrierFrequencyNumeric.Enabled = state
        Me.iqRateNumeric.Enabled = state
        Me.startAcquisitionButton.Enabled = state
        Me.samplesPerRecordNumeric.Enabled = state
        Me.stopButton.Enabled = Not state
    End Sub

    Private Sub FetchIQ(ByVal samplesPerBlock As Integer)
        ' Starts a Thread for Performing Fetch so that Main UI thread doesn't hangs.
        Dim _parameter1 As New ParameterizedThreadStart(AddressOf FetchIQData)
        workerThread = New Thread(_parameter1)
        workerThread.Start(samplesPerBlock)
    End Sub

    Private Sub ResetDefaults()
        numberOfSamplesFetchedTextBox.Clear()
        totalNumberOfSamples = 0
        ThreadStopMarker = False
    End Sub

    Private Sub startAcquisitionButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startAcquisitionButton.Click
        Try
            ' Steps:
            '1. Open a new NI-RFSA session.
            '2. Configure the acquisition type to I/Q.
            '3. Configure the reference level.
            '4. Configure the carrier frequency.
            '5. Configure the I/Q rate.
            '6. Configure the NI-RFSA device for a continuous acquisition.
            '7. Initiate the acquisition.
            '8. Fetch I/Q Data.
            '9. Get I/Q components and plot the data.
            '10. Close the NI-RFSA session.
            ResetDefaults()
            ChangeControlState(False)
            InitializeRfsaSession()
            ConfigureIQ()
            InitiateAcquisition()
            FetchIQ(NumberOfSamples)
        Catch ex As Exception
            ShowError(ex.Message)
            CloseSession()
            ChangeControlState(True)
        End Try

    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles stopButton.Click
        ' Stops the Acquisition.
        If workerThread IsNot Nothing Then
            ThreadStopMarker = True
        End If

        ChangeControlState(True)
        CloseSession()
    End Sub

    Private Sub ThreadTerminate()

        'Waits for the thread To Terminate.
        workerThread.Join()
        workerThread = Nothing
        If closeRequested = True Then
            Me.Close()
        End If
    End Sub

    Private Sub FetchIQData(ByVal numberOfSamples As Object)
        Dim timespan As New PrecisionTimeSpan(10.0)
        While ThreadStopMarker = False
            Dim wfmInfo As RfsaWaveformInfo
            Dim newdata As ComplexDouble() = Nothing
            Try
                rfsaSession.Acquisition.IQ.MemoryOptimizedFetchIQSingleRecordComplex(Of ComplexDouble)(0, CInt(numberOfSamples), timespan, newdata, wfmInfo)
                numberOfSamplesFetchedTextBox.Invoke(New Action(Of Long)(AddressOf UpdateData), wfmInfo.ActualSamples)
            Catch ex As Exception
                ShowError(ex.Message)
                Me.Invoke(New EventHandler(AddressOf stopButton_Click))
                Exit Try
            End Try
        End While
        Me.BeginInvoke(New Action(AddressOf ThreadTerminate))
    End Sub

    Private Sub UpdateData(ByVal numberOfSamples As Long)
        totalNumberOfSamples += numberOfSamples
        numberOfSamplesFetchedTextBox.Text = Convert.ToString(totalNumberOfSamples)
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        closeRequested = True
        stopButton_Click(Nothing, Nothing)
        If workerThread IsNot Nothing Then
            e.Cancel = True
        End If
    End Sub
    Private Sub DriverOperationWarning(ByVal o As Object, ByVal e As RfsaWarningEventArgs)
        MessageBox.Show(e.Warning.ToString(), "Warning")
    End Sub

End Class
