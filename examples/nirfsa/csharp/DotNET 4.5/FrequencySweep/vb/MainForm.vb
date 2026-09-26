'******************************************************************************
'*
'* Example program:
'*   Frequency Sweep
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'*  Use this example to learn how to acquire a spectrum using the RF vector
'	signal analyzer. This example shows how to configure NI-RFSA for spectrum
'	acquisition, how to set the resolution bandwidth and reference level, and
'	how to read power spectra while changing the center frequency in each
'	iteration of the sweep while keeping the span constant. The spectrum is
'	displayed on the datagrid. Note: This example illustrates how to perform
'	multiple spectrum acquisitions with different center frequency values. If
'	you need to acquire a spectrum with a span wider than the instantaneous
'	bandwidth of the device, use the Getting Started Spectrum example.
'*
'* Instructions for running:
'*  1. Configure RFSA device in the MAX for the program to run. 
'*
'*	2. Configure the Reference Level in the UI. 
'*		The reference level represents the maximum expected power of an input RF signal.
'*
'*	3. Configure the Start Center Frequency in the UI.
'*
'*	4. Configure the Stop Center Frequency in the UI.
'*
'*  5. Configure the Resolution Bandwidth in the UI.
'*
'*	6. Configure the Number of Steps in the UI.
'* 
'*  7. Select the start button to start the spectrum acquisition.
'*	
'*  8. The data is displayed in the DataGrid.
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

    Private ReadOnly Property Steps() As Integer
        Get
            Return Decimal.ToInt32(Me.numStepsNumeric.Value)
        End Get
    End Property

    Private WriteOnly Property CenterFrequency() As Double
        Set(ByVal value As Double)
            Me.currentCenterFrequencyTextBox.Text = Value.ToString()
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

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        ChangeControlState(False)
        Try
            InitializeRfsaSession()
            ConfigureForSpectrum()
            Sweep()
            CloseSession()
        Catch ex As System.Exception
            ShowError(ex.Message)
            CloseSession()
        Finally
            ChangeControlState(True)
        End Try
    End Sub

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.startButton.Enabled = isEnabled
        Me.resourceNameComboBox.Enabled = isEnabled
        Me.referenceLevelNumeric.Enabled = isEnabled
        Me.startFrequencyNumeric.Enabled = isEnabled
        Me.stopFrequencyNumeric.Enabled = isEnabled
        Me.resolutionBandwidthNumeric.Enabled = isEnabled
        Me.numStepsNumeric.Enabled = isEnabled
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

    Private Sub ConfigureForSpectrum()
        rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel
        rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.Spectrum
        rfsaSession.Configuration.Spectrum.ResolutionBandwidth = ResolutionBandwidth
    End Sub

    Private Sub Sweep()
        Dim startFreq As Double = StartFrequency
        Dim stopFreq As Double = StopFrequency
        Dim span As Double = 20000000.0

        If Steps < 2 Then
            ShowError("Invalid Input: Steps are less than 2")
            Return
        End If

        If stopFreq < startFreq Then
            ShowError("Invalid Input, Stop frequency is less than start frequency")
            Return
        End If

        Dim increment As Double = (stopFreq - startFreq) / (Steps - 1)
        While startFreq <= stopFreq
            CenterFrequency = startFreq
            rfsaSession.Configuration.Spectrum.ConfigureSpectrumFrequencyCenterSpan(startFreq, span)
            ReadPowerSpectrum()
            startFreq += increment
        End While
    End Sub

    Private Sub ReadPowerSpectrum()
        Dim data As Double()
        Dim spectrumInfo As RfsaSpectrumInfo
        Dim timespan As New PrecisionTimeSpan(10.0)
        data = rfsaSession.Acquisition.Spectrum.ReadPowerSpectrum(timespan, spectrumInfo)
        SetData(data)
    End Sub

    Private Sub SetData(ByVal data As Double())
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
