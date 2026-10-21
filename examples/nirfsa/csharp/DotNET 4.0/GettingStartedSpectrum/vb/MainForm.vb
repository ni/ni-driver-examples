'******************************************************************************
'*
'* Example program:
'*   Getting Started Spectrum
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'*   Use this example to learn the basics of spectrum acquisition using the RF
'	vector signal analyzer. The example shows how to configure the following
'	parameters of the spectrum acquisition: reference clock source, reference
'	level, start and stop frequencies, resolution bandwidth, and the spectrum
'	acquisition type. If you configure the spectrum span (Stop Frequency - Start
'	Frequency) to a value larger than the instantaneous bandwidth of the device,
'	NI-RFSA performs multiple acquisitions and combines them into one spectrum
'	of the size you requested. 
'
'* Instructions for running:
'*   1. Configure RFSA device in the MAX for the program to run. 
'*
'*   2. Configure the Reference Clock in the UI.
'*
'*	 3. Configure the Reference Level in the UI. 
'*		The reference level represents the maximum expected power of an input RF signal.
'*
'*	 4. Configure the Start and Stop Center Frequency.
'*
'*	 5. Configure the Resolution BandWidth.
'*	
'*   6. Select the Read Power Spectrum Button in the UI to start the spectrum acquisition.
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

#Region "UI Value Config Section"

    Private ReadOnly Property ReferenceClockSource() As RfsaReferenceClockSource
        Get
            Return If(TryCast(Me.referenceClockComboBox.SelectedValue, RfsaReferenceClockSource), RfsaReferenceClockSource.FromString(Me.referenceClockComboBox.Text))
        End Get
    End Property

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

    Private ReadOnly Property ResolutionBandwidth() As Double
        Get
            Return Decimal.ToDouble(Me.resolutionBandwidthNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property StartFrequency() As Double
        Get
            Return Decimal.ToDouble(Me.startFrequencyNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property StopFrequency() As Double
        Get
            Return Decimal.ToDouble(Me.stopFrequencyNumeric.Value)
        End Get
    End Property

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

    Private Sub readPowerSpectrumButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles readPowerSpectrumButton.Click
        ChangeControlState(False)
        Try
            InitializeRfsaSession(ResourceName)
            ConfigureRefClock(ReferenceClockSource)
            ConfigureForSpectrum(ReferenceLevel, StartFrequency, StopFrequency, ResolutionBandwidth)
            ReadPowerSpectrum()
            CloseSession()
        Catch ex As System.Exception
            ShowError(ex.Message)
            CloseSession()
        Finally
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.readPowerSpectrumButton.Enabled = isEnabled
        Me.resourceNameComboBox.Enabled = isEnabled
        Me.referenceLevelNumeric.Enabled = isEnabled
        Me.startFrequencyNumeric.Enabled = isEnabled
        Me.stopFrequencyNumeric.Enabled = isEnabled
        Me.referenceClockComboBox.Enabled = isEnabled
        Me.resolutionBandwidthNumeric.Enabled = isEnabled
    End Sub

    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")
    End Sub

    Private Sub InitializeRfsaSession(ByVal ResourceName As String)
        CloseSession()
        rfsaSession = New NIRfsa(ResourceName, True, False)
        AddHandler rfsaSession.DriverOperation.Warning, New EventHandler(Of RfsaWarningEventArgs)(AddressOf Me.DriverOperationWarning)
    End Sub

    Private Sub DriverOperationWarning(ByVal o As Object, ByVal e As RfsaWarningEventArgs)
        MessageBox.Show(e.Warning.ToString(), "Warning")
    End Sub

    Private Sub ConfigureRefClock(ByVal refClockSource As RfsaReferenceClockSource)
        rfsaSession.Configuration.ReferenceClock.Source = refClockSource
    End Sub

    Private Sub ConfigureForSpectrum(ByVal ReferenceLevel As Double, ByVal StartFrequency As Double, ByVal StopFrequency As Double, ByVal ResolutionBandwidth As Double)
        rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel
        rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.Spectrum
        rfsaSession.Configuration.Spectrum.ResolutionBandwidth = ResolutionBandwidth
        rfsaSession.Configuration.Spectrum.ConfigureSpectrumFrequencyStartStop(StartFrequency, StopFrequency)
    End Sub

    Private Sub ReadPowerSpectrum()
        Dim spectrumInfo As RfsaSpectrumInfo
        Dim data As Double()
        Dim timespan As New PrecisionTimeSpan(10.0)
        data = rfsaSession.Acquisition.Spectrum.ReadPowerSpectrum(timespan, spectrumInfo)
        DisplayData(data)
    End Sub

    Private Sub DisplayData(ByVal data As Double())
        Dim val As DoubleData() = Array.ConvertAll(Of Double, DoubleData)(data, Function(x) New DoubleData(x))
        Me.dataGridViewResults.DataSource = val
        Me.Refresh()
    End Sub

    Private Sub CloseSession()
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
End Class

Structure DoubleData
    Private a As Double

    Public Sub New(ByVal d As Double)
        a = d
    End Sub
    Public ReadOnly Property Value() As Double
        Get
            Return a
        End Get
    End Property
End Structure
