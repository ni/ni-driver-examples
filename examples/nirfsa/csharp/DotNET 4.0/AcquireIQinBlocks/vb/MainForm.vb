'******************************************************************************
'*
'* Example program:
'*   Acquire IQ in Blocks
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'*   Use this example to learn how to acquire I/Q data using the RF vector
'*	 signal analyzer. The example shows how to configure NI-RFSA for finite I/Q
'*	 acquisition, how to set the carrier frequency and the I/Q rate, and how to
'*	 fetch I/Q data in blocks. The quadrature data is displayed on the I and Q datagrid.
'*
'* Instructions for running:
'*   1. Configure RFSA device in the MAX for the program to run. 
'*
'*	 2. Configure the Reference Level in the UI. 
'*		The reference level represents the maximum expected power of an input RF signal.
'*
'*	 3. Configure the Carrier Frequency in the UI.
'*
'*	 4. Configure the IQ Rate and the Samples to Read Per Block in the UI.
'*
'*   5. Configure the Maximum Samples Per Block in the UI.
'*
'*	 6. Select Acquire Button for RFSA to start acquiring the data.
'*	
'*   7. The data is displayed in the DataGrid.
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

Public Partial Class MainForm
	Inherits Form
	Private rfsaSession As NIRfsa

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

    Private ReadOnly Property SamplePerBlock() As Integer
        Get
            Return Decimal.ToInt32(Me.samplesPerBlockNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property SampleRead() As Long
        Get
            Return Long.Parse(Me.samplesReadTextBox.Text)
        End Get
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

    Private Sub InitializeRfsaSession()
        CloseSession()
        rfsaSession = New NIRfsa(ResourceName, True, False)
        AddHandler rfsaSession.DriverOperation.Warning, New EventHandler(Of RfsaWarningEventArgs)(AddressOf Me.DriverOperationWarning)
    End Sub

    Private Sub DriverOperationWarning(ByVal sender As Object, ByVal e As RfsaWarningEventArgs)
        MessageBox.Show(e.Warning.ToString(), "Warning")
    End Sub

    Private Sub ConfigureIQ()
        rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ
        rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel
        rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFrequency
        rfsaSession.Configuration.IQ.NumberOfSamples = NumberOfSamples
        rfsaSession.Configuration.IQ.IQRate = IQRate
    End Sub

    Private Sub InitiateAcquisition()
        rfsaSession.Acquisition.IQ.Initiate()
    End Sub

    Public Sub FetchIQDataInBlocks()
        Dim counter As Long = 0
        While rfsaSession IsNot Nothing AndAlso counter < NumberOfSamples
            Dim sampleFetched As Long = FetchIQData(SamplePerBlock)
            counter += sampleFetched
            samplesReadTextBox.Text = counter.ToString()
            Me.Refresh()
        End While
    End Sub

    Private Function FetchIQData(ByVal samplesPerBlock As Integer) As Long
        Dim wfmInfo As RfsaWaveformInfo
        Dim newdata As ComplexDouble()
        Dim timespan As New PrecisionTimeSpan(10.0)
        newdata = rfsaSession.Acquisition.IQ.FetchIQSingleRecordComplex(Of ComplexDouble)(0, samplesPerBlock, timespan, wfmInfo)
        Me.dataGridViewResults.DataSource = newdata
        Return wfmInfo.ActualSamples
    End Function

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles acquireButton.Click
        ChangeControlState(False)
        Try
            InitializeRfsaSession()
            ConfigureIQ()
            InitiateAcquisition()
            FetchIQDataInBlocks()
            CloseSession()
        Catch ex As Exception
            ShowError(ex.Message)
            CloseSession()
        Finally
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub ChangeControlState(ByVal state As Boolean)
        acquireButton.Enabled = state
        resourceNameComboBox.Enabled = state
        referenceLevelNumeric.Enabled = state
        carrierFrequencyNumeric.Enabled = state
        iqRateNumeric.Enabled = state
        samplesPerBlockNumeric.Enabled = state
        samplesPerRecordNumeric.Enabled = state
    End Sub

    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")
    End Sub


    Public Sub CloseSession()
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
End Class
