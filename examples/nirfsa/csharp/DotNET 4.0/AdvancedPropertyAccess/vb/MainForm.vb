'******************************************************************************
'*
'* Example program:
'*   Advanced Property Access
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'*   This example demonstrates how to use the Advanced Property Access Service
'*   in NI-RFSA .NET API.
'*	The example shows how to configure the following parameters
'*	of I/Q acquisition: reference clock, reference level, carrier frequency, I/Q
'*	rate, number of samples per record, and I/Q acquisition type.
'*	This example also reads and displays data on a dataGrid.
'*
'* Instructions for running:
'*   1. Configure RFSA device in the MAX for the program to run. 
'*
'*   2. Configure the Reference Clock in the UI.
'*
'*	3. Configure the Reference Level in the UI. 
'*		The reference level represents the maximum expected power of an input RF signal.
'*
'*	4. Configure the Carrier Frequency in the UI.
'*
'*	5. Configure the Samples Per Record in the UI and the IQ Rate.
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
Imports NationalInstruments.ModularInstruments
Imports NationalInstruments.ModularInstruments.NIRfsa
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Public Partial Class MainForm
	Inherits Form
	' Attribute values obtained from the C header files: ivi.h, nirfsa.h
	Friend NotInheritable Class CDriverAttributeId
		Private Sub New()
		End Sub
		' NIRFSA_ATTR_REF_CLOCK_SOURCE
		Friend Const ReferenceClockSource As Long = 1150019

		' NIRFSA_ATTR_REF_CLOCK_RATE
		Friend Const ReferenceClockRate As Long = 1150020

		' NIRFSA_ATTR_ACQUISITION_TYPE
		Friend Const AcquisitionType As Long = 1150001

		' NIRFSA_ATTR_REFERENCE_LEVEL
		Friend Const ReferenceLevel As Long = 1150004

		' NIRFSA_ATTR_IQ_CARRIER_FREQUENCY
		Friend Const IQCarrierFrequency As Long = 1150059

		' NIRFSA_ATTR_NUMBER_OF_SAMPLES
		Friend Const NumberOfSamples As Long = 1150009

		' NIRFSA_ATTR_NUMBER_OF_SAMPLES_IS_FINITE
		Friend Const NumberOfSamplesIsFinite As Long = 1150008

		' NIRFSA_ATTR_IQ_RATE
		Friend Const IQRate As Long = 1150007
	End Class

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

    Private ReadOnly Property NumberOfSamples() As Long
        Get
            Return Decimal.ToInt64(Me.samplesPerRecordNumeric.Value)
        End Get
    End Property

	Private ReadOnly Property IQRate() As Double
		Get
			Return Decimal.ToDouble(Me.iqRateNumeric.Value)
		End Get
	End Property

	Private ReadOnly Property ReferenceClock() As String
		Get
			Return Me.referenceClockComboBox.Text
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

	Private Sub acquireButton_Click(sender As Object, e As System.EventArgs)
		ChangeControlState(False)
		Try
			InitializeRfsaSession()
			ConfigureUsingAdvancedPropertyAccessService()
			ReadIQData()
			CloseSession()
		Catch ex As System.Exception
			ShowError(ex.Message)
			CloseSession()
		Finally
			ChangeControlState(True)
		End Try

	End Sub

	Private Sub ChangeControlState(isEnabled As Boolean)
		Me.acquireButton.Enabled = isEnabled
		Me.resourceNameComboBox.Enabled = isEnabled
		Me.referenceLevelNumeric.Enabled = isEnabled
		Me.carrierFrequencyNumeric.Enabled = isEnabled
		Me.iqRateNumeric.Enabled = isEnabled
		Me.referenceClockComboBox.Enabled = isEnabled
		Me.samplesPerRecordNumeric.Enabled = isEnabled
	End Sub

	Private Shared Sub ShowError(message As String)
		If String.IsNullOrEmpty(message) Then
			message = "Unexpected Error"
		End If
		MessageBox.Show(message, "Error")
		

	End Sub

	Private Sub InitializeRfsaSession()
		CloseSession()
		rfsaSession = New NIRfsa(ResourceName, True, False)
		AddHandler rfsaSession.DriverOperation.Warning, New System.EventHandler(Of RfsaWarningEventArgs)(AddressOf DriverOperationWarning)
	End Sub

	Private Sub DriverOperationWarning(sender As Object, e As RfsaWarningEventArgs)
		MessageBox.Show(e.Warning.ToString(), "Warning")
	End Sub

	Private Sub ConfigureUsingAdvancedPropertyAccessService()
		Dim rfsaAdvancedPropertyAccessService As AdvancedPropertyAccessService = DirectCast(TryCast(rfsaSession, IServiceProvider).GetService(GetType(AdvancedPropertyAccessService)), AdvancedPropertyAccessService)

		' Configure Reference Clock Using Advanced Property Access Service.
		rfsaAdvancedPropertyAccessService.SetAttributeString(CDriverAttributeId.ReferenceClockSource, ReferenceClock)
		rfsaAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.ReferenceClockRate, 10000000.0)

		' Configure IQ using Advanced Property Access Service.
        rfsaAdvancedPropertyAccessService.SetAttributeInt32(CDriverAttributeId.AcquisitionType, 100)
		' Value of NIRFSA_VAL_IQ is 100.
		rfsaAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.ReferenceLevel, ReferenceLevel)
		rfsaAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.IQCarrierFrequency, CarrierFrequency)
        rfsaAdvancedPropertyAccessService.SetAttributeInt64(CDriverAttributeId.NumberOfSamples, NumberOfSamples)
		rfsaAdvancedPropertyAccessService.SetAttributeBoolean(CDriverAttributeId.NumberOfSamplesIsFinite, True)
		rfsaAdvancedPropertyAccessService.SetAttributeDouble(CDriverAttributeId.IQRate, IQRate)
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
			Catch ex As System.Exception
				ShowError("Unable to Close Session, Reset the device." & vbLf & "Error : " & ex.Message)
				Application.[Exit]()
			End Try
		End If
	End Sub
End Class
