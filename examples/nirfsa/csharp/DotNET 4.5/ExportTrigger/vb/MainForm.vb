'******************************************************************************
'*
'* Example program:
'*   Rfsa Export Trigger
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'*   Use this example to learn how to acquire I/Q data using the RF vector
'*   signal analyzer. The example shows how to configure NI-RFSA for finite I/Q
'*   acquisition, how to set the carrier frequency and the I/Q rate, and how to
'*	 fetch I/Q after exporting the Output terminal for the start trigger. 
'*	 The quadrature data is displayed on the datagrid.
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
'*   5. Configure the Output Terminal in the UI.
'*
'*	 6. Select start acquisition button to start the acquisition.
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
Imports System.ComponentModel
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices
Imports NationalInstruments.ModularInstruments.NIRfsa
Imports System.Collections
Imports NationalInstruments

Public Partial Class MainForm
	Inherits Form
    Private rfsaSession As NIRfsa

	Public Sub New()
		InitializeComponent()
		ConfigureOutputTerminalComboBox()
		LoadRfsaDeviceNames()
	End Sub

	#Region "InitialCOnfiguration"

	Private Sub LoadRfsaDeviceNames()
		Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-RFSA")
		For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
			resourceNameComboBox.Items.Add(device.Name)
		Next
		If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
			resourceNameComboBox.SelectedIndex = 0
		End If
	End Sub

    Private Sub ConfigureOutputTerminalComboBox()
        Dim outputTerminalValueList As List(Of KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)) = New List(Of KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal))()
        outputTerminalValueList.Add(New KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)("RefOut", RfsaExportStartTriggerExportedOutputTerminal.ReferenceOut))
        outputTerminalValueList.Add(New KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)("RefOut2", RfsaExportStartTriggerExportedOutputTerminal.ReferenceOut2))
        outputTerminalValueList.Add(New KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)("ClkOut", RfsaExportStartTriggerExportedOutputTerminal.ClockOut))
        outputTerminalValueList.Add(New KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)("PFI0", RfsaExportStartTriggerExportedOutputTerminal.Pfi0))
        outputTerminalValueList.Add(New KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)("PFI1", RfsaExportStartTriggerExportedOutputTerminal.Pfi1))
        outputTerminalValueList.Add(New KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)("PXI_Trig0", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine0))
        outputTerminalValueList.Add(New KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)("PXI_Trig1", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine1))
        outputTerminalValueList.Add(New KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)("PXI_Trig2", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine2))
        outputTerminalValueList.Add(New KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)("PXI_Trig3", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine3))
        outputTerminalValueList.Add(New KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)("PXI_Trig4", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine4))
        outputTerminalValueList.Add(New KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)("PXI_Trig5", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine5))
        outputTerminalValueList.Add(New KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)("PXI_Trig6", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine6))
        outputTerminalValueList.Add(New KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)("PXI_Trig7", RfsaExportStartTriggerExportedOutputTerminal.PxiTriggerLine7))
        outputTerminalValueList.Add(New KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)("PXI_STAR", RfsaExportStartTriggerExportedOutputTerminal.PxiStarLine))
        outputTerminalValueList.Add(New KeyValuePair(Of String, RfsaExportStartTriggerExportedOutputTerminal)("Do_Not_Export", RfsaExportStartTriggerExportedOutputTerminal.DoNotExport))
        outputTerminalComboBox.DisplayMember = "Key"
        outputTerminalComboBox.ValueMember = "Value"
        outputTerminalComboBox.DataSource = outputTerminalValueList
        outputTerminalComboBox.SelectedIndex = 0
    End Sub
#End Region

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

    Private ReadOnly Property OutputTerminal() As RfsaExportStartTriggerExportedOutputTerminal
        Get
            Return If(TryCast(Me.outputTerminalComboBox.SelectedValue, RfsaExportStartTriggerExportedOutputTerminal), RfsaExportStartTriggerExportedOutputTerminal.FromString(outputTerminalComboBox.Text))
        End Get
    End Property

#End Region

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.resourceNameComboBox.Enabled = isEnabled
        Me.referenceLevelNumeric.Enabled = isEnabled
        Me.carrierFrequencyNumeric.Enabled = isEnabled
        Me.iqRateNumeric.Enabled = isEnabled
        Me.samplesPerRecordNumeric.Enabled = isEnabled
        Me.startAcquisitionButton.Enabled = isEnabled
        Me.outputTerminalComboBox.Enabled = isEnabled
    End Sub

	Private Sub InitializeRfsaSession()
		CloseSession()
        rfsaSession = New NIRfsa(ResourceName, True, False)
        AddHandler rfsaSession.DriverOperation.Warning, New EventHandler(Of RfsaWarningEventArgs)(AddressOf Me.DriverOperationWarning)
    End Sub

    Private Sub DriverOperationWarning(ByVal o As Object, ByVal e As RfsaWarningEventArgs)
        MessageBox.Show(e.Warning.ToString(), "Warning")
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

    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")
    End Sub

    Private Sub ConfigureIQ()
        rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ
        rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel
        rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFrequency
        rfsaSession.Configuration.IQ.NumberOfSamples = NumberOfSamples
        rfsaSession.Configuration.IQ.IQRate = IQRate
        rfsaSession.Configuration.IQ.NumberOfSamplesIsFinite = True
    End Sub

    Private Sub ConfigureExportSignal()
        rfsaSession.Configuration.Triggers.StartTrigger.Export.OutputTerminal = OutputTerminal
    End Sub

    Private Sub InitiateAcquisition()
        rfsaSession.Acquisition.IQ.Initiate()
    End Sub

    Private Sub FetchIQdata()
        Dim wfmInfo As RfsaWaveformInfo
        Dim dataPtr As ComplexDouble()
        Dim timespan As New PrecisionTimeSpan(10.0)
        dataPtr = rfsaSession.Acquisition.IQ.FetchIQSingleRecordComplex(Of ComplexDouble)(0, NumberOfSamples, timespan, wfmInfo)
        Me.dataGridView1.DataSource = dataPtr
    End Sub

    Private Sub startAcquisitionButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startAcquisitionButton.Click
        ChangeControlState(False)
        Try
            ' Steps:
            '1. Open a new NI-RFSA session.
            '2. Configure the acquisition type to I/Q.
            '3. Configure the reference level.
            '4. Configure the carrier frequency.
            '5. Configure the I/Q rate.
            '6. Configure the number of samples per record.
            '7. Export the trigger to the output terminal.
            '8. Read the I/Q data.
            '9. Get I/Q components and plot the data.
            '10. Close the NI-RFSA session.
            InitializeRfsaSession()
            ConfigureIQ()
            ConfigureExportSignal()
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
End Class
