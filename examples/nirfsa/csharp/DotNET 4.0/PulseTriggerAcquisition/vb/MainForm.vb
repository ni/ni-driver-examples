'******************************************************************************
'*
'* Example program:
'*   Pulse Trigger Acquisition
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'*   Use this example to learn how to acquire I/Q data using the RF vector signal analyzer.
'    The example shows how to configure NI-RFSA for finite I/Q acquisition, how to set the carrier frequency and the I/Q rate,
'    and how to fetch I/Q data on a IQ Power Edge Reference Trigger. The quadrature data is displayed on the datagrid.
'                         
'
'* Instructions for running:
'*   1. Configure RFSA device in the MAX for the program to run. 
'*
'*	 2. Configure the Reference Level, Carrier Frequency and IQ Rate in the UI.
'*   
'*   3. Configure the Trigger Setting in Trigger Slope and Trigger Level.
'*
'*	 4. Configure the Burst Length, Reference Position and Minimum Quiet Time.
'*
'*   5. Select the Start Button in the UI to start the acquisition.
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
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices
Imports NationalInstruments.ModularInstruments.NIRfsa

Public Partial Class MainForm
	Inherits Form
    Private rfsaSession As NIRfsa
    Private _actualCoercedIQRate As Double
    Private _numberOfSamples As Integer

    Public Sub New()
        InitializeComponent()
        LoadRfsaDeviceNames()
        ConfigureTriggerSlopeComboBox()
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

    Private Sub ConfigureTriggerSlopeComboBox()
        Dim triggerSlopeValueList As List(Of KeyValuePair(Of String, RfsaIQPowerEdgeReferenceTriggerSlope)) = New List(Of KeyValuePair(Of String, RfsaIQPowerEdgeReferenceTriggerSlope))()
        triggerSlopeValueList.Add(New KeyValuePair(Of String, RfsaIQPowerEdgeReferenceTriggerSlope)("Falling", RfsaIQPowerEdgeReferenceTriggerSlope.Falling))
        triggerSlopeValueList.Add(New KeyValuePair(Of String, RfsaIQPowerEdgeReferenceTriggerSlope)("Rising", RfsaIQPowerEdgeReferenceTriggerSlope.Rising))
        triggerSlopeComboBox.DisplayMember = "Key"
        triggerSlopeComboBox.ValueMember = "Value"
        triggerSlopeComboBox.DataSource = triggerSlopeValueList
        triggerSlopeComboBox.SelectedIndex = 0
    End Sub

#Region "UI Gets"

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

    Private ReadOnly Property IQRate() As Double
        Get
            Return Decimal.ToDouble(Me.iqRateNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property TriggerLevel() As Double
        Get
            Return Decimal.ToDouble(Me.triggerLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property BurstLength() As Double
        Get
            Return Decimal.ToDouble(Me.burstLengthNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property ReferencePosition() As Double
        Get
            Return Decimal.ToDouble(Me.referencePositionNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property MinimumQuiteTime() As Double
        Get
            Return Decimal.ToDouble(Me.minimumQuiteTimeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property TriggerSlope() As RfsaIQPowerEdgeReferenceTriggerSlope
        Get
            Return CType(Me.triggerSlopeComboBox.SelectedValue, RfsaIQPowerEdgeReferenceTriggerSlope)
        End Get
    End Property

#End Region

    Private Sub InitializeRfsaSession()
        'Open a new NI-RFSA session.
        CloseSession()
        rfsaSession = New NIRfsa(ResourceName, True, False)
        AddHandler rfsaSession.DriverOperation.Warning, New EventHandler(Of RfsaWarningEventArgs)(AddressOf Me.DriverOperationWarning)
    End Sub

    Private Sub DriverOperationWarning(ByVal o As Object, ByVal e As RfsaWarningEventArgs)
        MessageBox.Show(e.Warning.ToString(), "Warning")
    End Sub

    Private Sub CloseSession()
        ' Close the NI-RFSA session.
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

    Private Sub ConfigureIQ()
        ' Set the acquisition type to I/Q.
        ' Configure the reference level.
        ' Configure the carrier frequency.
        ' Configure the I/Q rate.
        rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ
        rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel
        rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFrequency
        rfsaSession.Configuration.IQ.IQRate = IQRate
    End Sub

    Private Sub ConfigureNumberOfSamples()
        '  Read the actual coerced I/Q rate for the calculations.
        '  Configure the samples per record (set to the burst length * IQ rate).
        _actualCoercedIQRate = rfsaSession.Configuration.IQ.IQRate
        _numberOfSamples = CInt(Math.Truncate(_actualCoercedIQRate * BurstLength))
        rfsaSession.Configuration.IQ.NumberOfSamples = _numberOfSamples
        rfsaSession.Configuration.IQ.NumberOfSamplesIsFinite = True
    End Sub

    Private Sub ConfigureIQPowerEdgeTrigger()
        ' Configure the I/Q power edge Reference trigger.
        ' Configure the minimum quiet time.
        Dim pretriggerSamples As Integer = CInt(Math.Truncate(_numberOfSamples * (ReferencePosition / 100)))

        rfsaSession.Configuration.Triggers.ReferenceTrigger.IQPowerEdge.Level = TriggerLevel
        rfsaSession.Configuration.Triggers.ReferenceTrigger.IQPowerEdge.Slope = TriggerSlope
        rfsaSession.Configuration.Triggers.ReferenceTrigger.IQPowerEdge.Source = RfsaIQPowerEdgeReferenceTriggerSource.Zero
        rfsaSession.Configuration.Triggers.ReferenceTrigger.Type = RfsaReferenceTriggerType.IQPowerEdge
        rfsaSession.Configuration.Triggers.ReferenceTrigger.PreTriggerSamples = pretriggerSamples
        rfsaSession.Configuration.Triggers.ReferenceTrigger.MinimumQuietTime = MinimumQuiteTime

    End Sub

    Private Sub InitiateAcquisition()
        ' Initiate the acquisition.
        rfsaSession.Acquisition.IQ.Initiate()
    End Sub

    Private Sub FetchIQdata()
        ' Fetch the I/Q Data.
        ' Get the I/Q components and plot the data.
        Dim dataPtr As ComplexDouble()
        Dim timespan As New PrecisionTimeSpan(10.0)
        dataPtr = rfsaSession.Acquisition.IQ.FetchIQSingleRecordComplex(Of ComplexDouble)(0, _numberOfSamples, timespan)
        Me.dataGridViewResults.DataSource = dataPtr
        Me.Refresh()
    End Sub

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.resourceNameComboBox.Enabled = isEnabled
        Me.referenceLevelNumeric.Enabled = isEnabled
        Me.carrierFrequencyNumeric.Enabled = isEnabled
        Me.iqRateNumeric.Enabled = isEnabled
        Me.triggerLevelNumeric.Enabled = isEnabled
        Me.burstLengthNumeric.Enabled = isEnabled
        Me.referencePositionNumeric.Enabled = isEnabled
        Me.minimumQuiteTimeNumeric.Enabled = isEnabled
        Me.triggerSlopeComboBox.Enabled = isEnabled
        Me.startButton.Enabled = isEnabled
    End Sub

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startButton.Click
        Try
            ChangeControlState(False)
            InitializeRfsaSession()
            ConfigureIQ()
            ConfigureNumberOfSamples()
            ConfigureIQPowerEdgeTrigger()
            InitiateAcquisition()
            FetchIQdata()
            CloseSession()
        Catch ex As Exception
            ShowError(ex.Message)
            CloseSession()
        Finally
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

	

    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")
    End Sub
End Class
