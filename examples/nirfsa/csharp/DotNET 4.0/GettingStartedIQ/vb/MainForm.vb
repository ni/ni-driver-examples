'******************************************************************************
'* Example program:
'*   Getting Started IQ
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'*   Use this example to learn the basics of I/Q acquisition using the RF vector signal analyzer. 
'	The example shows how to configure the following parameters
'	of I/Q acquisition: reference clock, reference level, carrier frequency, I/Q
'	rate, number of samples per record, and I/Q acquisition type. This example
'	also reads and displays data on a dataGrid.
'*
'* Instructions for running:
'*   1. Configure RFSA device in the MAX for the program to run. 
'*
'*   2. Configure the Reference Clock in the UI.
'*
'*	 3. Configure the Reference Level in the UI. 
'*		The reference level represents the maximum expected power of an input RF signal.
'*
'*	 4. Configure the Carrier Frequency in the UI.
'*
'*	 5. Configure the Samples Per Record in the UI and the IQ Rate.
'*
'*   6. Select the Acquire Button in the UI to start the aqcuisition.
'*   
'*   7. The data is displayed in the DataGrid.
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

Partial Public Class MainForm
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

    Private ReadOnly Property ReferenceCLock() As RfsaReferenceClockSource
        Get
            Return If(TryCast(Me.referenceClockComboBox.SelectedValue, RfsaReferenceClockSource), RfsaReferenceClockSource.FromString(Me.referenceClockComboBox.Text))
        End Get
    End Property

    Public Sub New()
        InitializeComponent()
        ConfigureRefClockComboBox()
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


#Region "UI Initial Value Config Section"

    Private Sub ConfigureRefClockComboBox()
        Dim refClockValueList As New List(Of KeyValuePair(Of String, RfsaReferenceClockSource))()
        refClockValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("OnboardClock", RfsaReferenceClockSource.OnboardClock))
        refClockValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("RefIn", RfsaReferenceClockSource.ReferenceIn))
        refClockValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("PXI_Clk", RfsaReferenceClockSource.PxiClock))
        refClockValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("ClkIn", RfsaReferenceClockSource.ClockIn))
        referenceClockComboBox.DisplayMember = "Key"
        referenceClockComboBox.ValueMember = "Value"
        referenceClockComboBox.DataSource = refClockValueList
        referenceClockComboBox.SelectedIndex = 0
    End Sub

#End Region

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles acquireButton.Click
        ChangeControlState(False)
        Try
            InitializeRfsaSession()
            ConfigureRefClock()
            ConfigureIQ()
            ReadIQData()
            CloseSession()
        Catch ex As System.Exception
            ShowError(ex.Message)
            CloseSession()
        Finally
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.acquireButton.Enabled = isEnabled
        Me.resourceNameComboBox.Enabled = isEnabled
        Me.referenceLevelNumeric.Enabled = isEnabled
        Me.carrierFrequencyNumeric.Enabled = isEnabled
        Me.iqRateNumeric.Enabled = isEnabled
        Me.referenceClockComboBox.Enabled = isEnabled
        Me.samplesPerRecordNumeric.Enabled = isEnabled
    End Sub


    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")
    End Sub

    Private Sub InitializeRfsaSession()
        CloseSession()
        rfsaSession = New NIRfsa(ResourceName, True, False)
        AddHandler rfsaSession.DriverOperation.Warning, New EventHandler(Of RfsaWarningEventArgs)(AddressOf Me.DriverOperationWarning)
    End Sub

    Private Sub DriverOperationWarning(ByVal o As Object, ByVal e As RfsaWarningEventArgs)
        MessageBox.Show(e.Warning.ToString(), "Warning")
    End Sub

    Private Sub ConfigureRefClock()
        rfsaSession.Configuration.ReferenceClock.Source = ReferenceCLock
        rfsaSession.Configuration.ReferenceClock.Rate = 10000000.0
    End Sub

    Private Sub ConfigureIQ()
        rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ
        rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel
        rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFrequency
        rfsaSession.Configuration.IQ.NumberOfSamples = NumberOfSamples
        rfsaSession.Configuration.IQ.NumberOfSamplesIsFinite = True
        rfsaSession.Configuration.IQ.IQRate = IQRate
    End Sub

    Private Sub ReadIQData()
        Dim wfmInfo As RfsaWaveformInfo
        Dim data As ComplexDouble()
        Dim timespan As New PrecisionTimeSpan(10.0)
        data = rfsaSession.Acquisition.IQ.ReadIQSingleRecordComplex(timespan, wfmInfo)
        Me.dataGridViewResults.DataSource = data
    End Sub

    Private Sub CloseSession()
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
