'******************************************************************************
'*
'* Example program:
'*   Inband Retuning With Internal LO
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'   Use this example to learn the basics of in-band retuning  using the
'	NIPXIe-5663/5663E RF vector signal analyzer.  The example shows how to
'	enable in-band retuning and configure a spectrum acquisition that does not
'	reconfigure the LO frequency of the downconverter with every change in
'	spectrum center frequency.
'
'* Instructions for running:
'*   1. Configure RFSA device in the MAX for the program to run. 
'*
'*	 2. Configure the Reference Level in the UI. 
'*		The reference level represents the maximum expected power of an input RF signal.
'*
'*	 3. Configure the Center Frequency and Span in the UI.
'*
'*	 4. Configure the Resolution BandWidth and DownConvertor Frequency.
'*	
'*   5. Select the Read Power Spectrum Button in the UI to start the spectrum acquisition.
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

Partial Public Class MainForm
    Inherits Form
    Private rfsaSession As NIRfsa

    Public Sub New()
        InitializeComponent()
        ConfigureRefClockComboBox()
        LoadRfsaDeviceNames()
    End Sub

#Region "UI Initial Value Config Section"
    Private Sub ConfigureRefClockComboBox()
        Dim refClockValueList As New List(Of KeyValuePair(Of String, RfsaReferenceClockSource))()
        refClockValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("OnboardClock", RfsaReferenceClockSource.OnboardClock))
        refClockValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("RefIn", RfsaReferenceClockSource.ReferenceIn))
        refClockValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("PXI_Clk", RfsaReferenceClockSource.PxiClock))
        refClockValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("ClkIn", RfsaReferenceClockSource.ClockIn))
        referenceClockComboBox.DataSource = refClockValueList
        referenceClockComboBox.DisplayMember = "Key"
        referenceClockComboBox.ValueMember = "Value"
        referenceClockComboBox.SelectedIndex = 0
    End Sub

    Private Sub LoadRfsaDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-RFSA")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            rfsaResourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            rfsaResourceNameComboBox.SelectedIndex = 0
        End If
    End Sub

#End Region
    Private ReadOnly Property RFSAResourceName() As String
        Get
            Return Me.rfsaResourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property ReferenceClockSource() As RfsaReferenceClockSource
        Get
            Return If(TryCast(Me.referenceClockComboBox.SelectedValue, RfsaReferenceClockSource), RfsaReferenceClockSource.FromString(Me.referenceClockComboBox.Text))
        End Get
    End Property

    Private ReadOnly Property ReferenceLevel() As Double
        Get
            Return Decimal.ToDouble(Me.referenceLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CenterFrequency() As Double
        Get
            Return Decimal.ToDouble(Me.centerFrequencyNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property ResolutionBandwidth() As Double
        Get
            Return Decimal.ToDouble(Me.resolutionBandwidthNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property DownConverterCenterFrequency() As Double
        Get
            Return Decimal.ToDouble(Me.downconverterCenterFrequencyNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Span() As Double
        Get
            Return Decimal.ToDouble(Me.spanNumeric.Value)
        End Get
    End Property

    Private Sub readPowerSpectrumButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles readPowerSpectrumButton.Click
        ChangeControlState(False)
        Try
            InitializeRfsaSession()
            ConfigureRefClock()
            configureForSpectrumAndEnableInBandRetuning()
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
        Me.rfsaResourceNameComboBox.Enabled = isEnabled
        Me.referenceClockComboBox.Enabled = isEnabled
        Me.referenceLevelNumeric.Enabled = isEnabled
        Me.centerFrequencyNumeric.Enabled = isEnabled
        Me.spanNumeric.Enabled = isEnabled
        Me.downconverterCenterFrequencyNumeric.Enabled = isEnabled
        Me.resolutionBandwidthNumeric.Enabled = isEnabled
    End Sub

    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")
    End Sub

    Private Sub InitializeRfsaSession()
        CloseSession()
        rfsaSession = New NIRfsa(RFSAResourceName, True, False)
        AddHandler rfsaSession.DriverOperation.Warning, New EventHandler(Of RfsaWarningEventArgs)(AddressOf Me.DriverOperationWarning)
    End Sub

    Private Sub DriverOperationWarning(ByVal o As Object, ByVal e As RfsaWarningEventArgs)
        MessageBox.Show(e.Warning.ToString(), "Warning")
    End Sub

    Private Sub ConfigureRefClock()
        rfsaSession.Configuration.ReferenceClock.Source = ReferenceClockSource
        rfsaSession.Configuration.ReferenceClock.Rate = 10000000.0
    End Sub

    Private Sub ConfigureForSpectrumAndEnableInBandRetuning()
        rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel
        rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.Spectrum
        rfsaSession.Acquisition.Advanced.DownconverterCenterFrequency = DownConverterCenterFrequency
        rfsaSession.Configuration.Spectrum.ConfigureSpectrumFrequencyCenterSpan(CenterFrequency, Span)
        rfsaSession.Configuration.Spectrum.ResolutionBandwidth = ResolutionBandwidth
    End Sub

    Private Sub ReadPowerSpectrum()
        Dim spectrumInfo As RfsaSpectrumInfo
        Dim data As Double()
        Dim timespan As New PrecisionTimeSpan(10.0)
        data = rfsaSession.Acquisition.Spectrum.ReadPowerSpectrum(timespan, spectrumInfo)
        DisplayData(data)
    End Sub

    Private Sub DisplayData(ByVal data As Double())
        Dim dData As DoubleData() = System.Array.ConvertAll(Of Double, DoubleData)(data, Function(x) New DoubleData(x))
        Me.dataGridViewResults.DataSource = dData
        Me.Refresh()
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
