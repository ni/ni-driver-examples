'******************************************************************************
'*
'* Example program:
'*   Inband Retuning With External LO
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'*  Use this example to learn the basics of in-band retuning with an external
'	LO using the NI PXIe-5601 RF downconverter, NI PXIe-5622 digitizer, and
'	example shows how to enable in-band retuning and configure the spectrum
'	acquisition parameters in a way that does not change the LO tuned frequency.
'
'* Instructions for running:
'*   1. Configure RFSA device in the MAX for the program to run. 
'*
'*   2. Configure the LO Source in the MAX and Select the resource in the LO Source Name.
'*
'*	 3. Configure the Reference Level in the UI. 
'*		The reference level represents the maximum expected power of an input RF signal.
'*
'*	 4. Configure the Center Frequency and Span in the UI.
'*
'*	 5. Configure the Resolution BandWidth and DownConvertor Frequency.
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
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsa
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Public Partial Class MainForm
	Inherits Form
	Private rfsaSession As NIRfsa
	Private rfsgSession As NIRfsg

	Public Sub New()
		InitializeComponent()
		LoadRfsaDeviceNames()
		LoadRfsgDeviceNames()
	End Sub

	#Region "UI Initial Value Config Section"
	Private Sub LoadRfsaDeviceNames()
		Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-RFSA")
		For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
			resourceNameComboBox.Items.Add(device.Name)
		Next
		If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
			resourceNameComboBox.SelectedIndex = 0
		End If
	End Sub

	Private Sub LoadRfsgDeviceNames()
		Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-RFSG")
		For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
			loResourceNameComboBox.Items.Add(device.Name)
		Next
		If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
			loResourceNameComboBox.SelectedIndex = 0
		End If
	End Sub
	#End Region

	Private ReadOnly Property RFSAResourceName() As String
		Get
			Return Me.resourceNameComboBox.Text
		End Get
	End Property

	Private ReadOnly Property LOResourceName() As String
		Get
			Return Me.loResourceNameComboBox.Text
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

    Private ReadOnly Property CenterFrequency() As Double
        Get
            Return Decimal.ToDouble(Me.centerFrequencyNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property Span() As Double
        Get
            Return Decimal.ToDouble(Me.spanNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property DownConverterCenterFrequency() As Double
        Get
            Return Decimal.ToDouble(Me.downconverterCenterFrequencyNumeric.Value)
        End Get
    End Property

    Private Sub InitializeRfsaSession()
        CloseRfsaSession()
        rfsaSession = New NIRfsa(RFSAResourceName, True, False, "DriverSetup = LO: <external>")
        AddHandler rfsaSession.DriverOperation.Warning, New EventHandler(Of RfsaWarningEventArgs)(AddressOf Me.DriverOperationWarning)
    End Sub

    Private Sub DriverOperationWarning(ByVal o As Object, ByVal e As RfsaWarningEventArgs)
        MessageBox.Show(e.Warning.ToString(), "Warning")
    End Sub

    Private Sub InitializeExternalLOSession()
        ' Initiate Rfsg Session  
        CloseExternalLOSession()
        rfsgSession = New NIRfsg(LOResourceName, True, False)
    End Sub

    Private Sub ConfigureRefClock()
        rfsaSession.Configuration.ReferenceClock.Source = RfsaReferenceClockSource.OnboardClock
    End Sub

    Private Function ConfigureForSpectrumAndEnableInBandRetuning() As Double
        rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel
        rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.Spectrum
        rfsaSession.Acquisition.Advanced.DownconverterCenterFrequency = DownConverterCenterFrequency
        rfsaSession.Configuration.Spectrum.ConfigureSpectrumFrequencyCenterSpan(CenterFrequency, Span)
        rfsaSession.Configuration.Spectrum.ResolutionBandwidth = ResolutionBandwidth
        Dim profInfo As System.Reflection.PropertyInfo = rfsaSession.Configuration.SignalPath.LocalOscillator.[GetType]().GetProperty("LOFrequency")
        rfsaSession.Utility.ResetAttribute(profInfo)
        Return rfsaSession.Configuration.SignalPath.LocalOscillator.LOFrequency
    End Function

    Private Function ConfigureExternalLO(ByVal loFrequency As Double) As Double
        Dim InstrumentModel As String = rfsgSession.Identity.InstrumentModel
        If String.Compare(InstrumentModel, "NI PXIe-5653", StringComparison.OrdinalIgnoreCase) = 0 Then
            rfsgSession.RF.Frequency = loFrequency
        Else
            rfsgSession.RF.Configure(loFrequency, 0.0)
        End If

        Return rfsgSession.RF.Frequency
    End Function

    Private Sub InitiateExternalLO()
        rfsgSession.Initiate()
    End Sub

    Private Sub ConfigureActualLOFrequency(ByVal actualLOFrequency As Double)
        rfsaSession.Configuration.SignalPath.LocalOscillator.LOFrequency = actualLOFrequency
    End Sub

    Private Sub ReadPowerSpectrum()
        Dim spectrumInfo As RfsaSpectrumInfo
        Dim data As Double()
        Dim timespan As New PrecisionTimeSpan(10.0)
        data = rfsaSession.Acquisition.Spectrum.ReadPowerSpectrum(timespan, spectrumInfo)
        Dim val As DoubleData() = Array.ConvertAll(Of Double, DoubleData)(data, Function(x) New DoubleData(x))
        dataGridViewResults.DataSource = val

    End Sub

    Private Sub CloseRfsaSession()
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

    Private Sub CloseExternalLOSession()
        If rfsgSession IsNot Nothing Then
            Try
                rfsgSession.Close()
                rfsgSession = Nothing
            Catch ex As Exception
                ShowError("Unable to Close the external LO Session, Reset the device." & vbLf & "Error : " & ex.Message)
                Application.[Exit]()
            End Try
        End If
    End Sub

    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")
    End Sub

    Private Sub readPowerSpectrumButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles readPowerSpectrumButton.Click
        ChangeControlState(False)
        Try
            InitializeRfsaSession()
            InitializeExternalLOSession()
            ConfigureRefClock()
            Dim loFrequency As Double = ConfigureForSpectrumAndEnableInBandRetuning()
            Dim actualLOFrequency As Double = ConfigureExternalLO(loFrequency)
            InitiateExternalLO()
            Me.loFrequencyTextBox.Text = actualLOFrequency.ToString()
            ConfigureActualLOFrequency(actualLOFrequency)
            ReadPowerSpectrum()
            CloseRfsaSession()
            CloseExternalLOSession()
        Catch ex As System.Exception
            CloseRfsaSession()
            CloseExternalLOSession()
            ShowError(ex.Message)
        Finally
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.readPowerSpectrumButton.Enabled = isEnabled
        Me.resourceNameComboBox.Enabled = isEnabled
        Me.loResourceNameComboBox.Enabled = isEnabled
        Me.referenceLevelNumeric.Enabled = isEnabled
        Me.centerFrequencyNumeric.Enabled = isEnabled
        Me.spanNumeric.Enabled = isEnabled
        Me.downconverterCenterFrequencyNumeric.Enabled = isEnabled
        Me.resolutionBandwidthNumeric.Enabled = isEnabled
    End Sub
End Class

Structure DoubleData
	Private a As Double

	Public Sub New(d As Double)
		a = d
	End Sub
	Public ReadOnly Property Value() As Double
		Get
			Return a
		End Get
	End Property
End Structure
