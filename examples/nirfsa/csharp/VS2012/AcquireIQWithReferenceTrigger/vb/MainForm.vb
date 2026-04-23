'******************************************************************************
'*
'* Example program:
'*   Acquire IQ with Reference Trigger
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'*   Use this example to learn how to acquire I/Q data using the RF vector
'*   signal analyzer and a Reference trigger. This example shows how to
'*   configure NI-RFSA for triggered I/Q acquisition, how to set the carrier
'*   frequency and I/Q rate, and how to fetch I/Q data in blocks. The quadrature
'*   data is displayed on the data grid. This example demonstrates the use
'*   of three types of triggering: software, digital edge, and I/Q power edge
'*   triggering. The triggerring is always done for the Rising edge.
'*   This example also demonstrates how to configure a triggering
'*   subsystem to set the number of pretrigger points as well as relevant
'*   Reference trigger parameters such as minimum quiet time and trigger level.
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
'*   5. Configure the Trigger Type for the Acquisition. The triggering happens on the rising edge.
'*
'*	 6. Select start acquisition button to start the acquisition. If the trigger type is Software,
'*      Send Software Trigger Button will send the trigger needed for the device.	
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

Public Partial Class MainForm
	Inherits Form
	Private rfsaSession As NIRfsa

	Public Sub New()
		InitializeComponent()
		ConfigureTriggerTypeComboBox()
		ConfigureTriggerSourceComboBox()
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

	Private Sub ConfigureTriggerTypeComboBox()
        Dim triggerTypeValueList As List(Of KeyValuePair(Of String, RfsaReferenceTriggerType)) = New List(Of KeyValuePair(Of String, RfsaReferenceTriggerType))
		triggerTypeValueList.Add(New KeyValuePair(Of String, RfsaReferenceTriggerType)("None", RfsaReferenceTriggerType.None))
		triggerTypeValueList.Add(New KeyValuePair(Of String, RfsaReferenceTriggerType)("Digital Edge", RfsaReferenceTriggerType.DigitalEdge))
		triggerTypeValueList.Add(New KeyValuePair(Of String, RfsaReferenceTriggerType)("IQ Power Edge", RfsaReferenceTriggerType.IQPowerEdge))
		triggerTypeValueList.Add(New KeyValuePair(Of String, RfsaReferenceTriggerType)("Software", RfsaReferenceTriggerType.SoftwareEdge))
		triggerTypeComboBox.DisplayMember = "Key"
		triggerTypeComboBox.ValueMember = "Value"
		triggerTypeComboBox.DataSource = triggerTypeValueList
		triggerTypeComboBox.SelectedIndex = 0
	End Sub

	Private Sub ConfigureTriggerSourceComboBox()
        Dim triggerSourceValueList As List(Of KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource)) = New List(Of KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource))
        triggerSourceValueList.Add(New KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource)("PFI0", RfsaDigitalEdgeReferenceTriggerSource.Pfi0))
		triggerSourceValueList.Add(New KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource)("PFI1", RfsaDigitalEdgeReferenceTriggerSource.Pfi1))
		triggerSourceValueList.Add(New KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource)("PXI_Trig0", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine0))
		triggerSourceValueList.Add(New KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource)("PXI_Trig1", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine1))
		triggerSourceValueList.Add(New KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource)("PXI_Trig2", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine2))
		triggerSourceValueList.Add(New KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource)("PXI_Trig3", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine3))
		triggerSourceValueList.Add(New KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource)("PXI_Trig4", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine4))
		triggerSourceValueList.Add(New KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource)("PXI_Trig5", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine5))
		triggerSourceValueList.Add(New KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource)("PXI_Trig6", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine6))
		triggerSourceValueList.Add(New KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource)("PXI_Trig7", RfsaDigitalEdgeReferenceTriggerSource.PxiTriggerLine7))
		triggerSourceValueList.Add(New KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource)("PXI_STAR", RfsaDigitalEdgeReferenceTriggerSource.PxiStarLine))
		triggerSourceValueList.Add(New KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource)("TimerEvent", RfsaDigitalEdgeReferenceTriggerSource.TimerEvent))
		triggerSourceComboBox.DisplayMember = "Key"
		triggerSourceComboBox.ValueMember = "Value"
		triggerSourceComboBox.DataSource = triggerSourceValueList
		triggerSourceComboBox.SelectedIndex = 0
	End Sub

	#End Region

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

    Private ReadOnly Property PreTriggerSamples() As Long
        Get
            Return Decimal.ToInt64(Me.pretriggerSamplesNumeric.Value)
        End Get
    End Property

    Public ReadOnly Property TriggerType() As RfsaReferenceTriggerType
        Get
            Return CType(Me.triggerTypeComboBox.SelectedValue, RfsaReferenceTriggerType)
        End Get
    End Property

    Public ReadOnly Property TriggerSource() As RfsaDigitalEdgeReferenceTriggerSource
        Get
            Return If(TryCast(Me.triggerSourceComboBox.SelectedValue, RfsaDigitalEdgeReferenceTriggerSource), RfsaDigitalEdgeReferenceTriggerSource.FromString(triggerSourceComboBox.Text))
        End Get
    End Property

    Private ReadOnly Property TriggerLevel() As Double
        Get
            Return Decimal.ToDouble(Me.triggerLevelNumeric.Value)
        End Get
    End Property

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
                ShowError(("Unable to Close Session, Reset the device." & vbLf & "Error : ") + ex.Message)
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
    End Sub

    Private Sub ConfigureRefTrigger()
        Dim triggerType__1 As RfsaReferenceTriggerType = TriggerType
        Select Case triggerType__1
            Case RfsaReferenceTriggerType.None
                rfsaSession.Configuration.Triggers.ReferenceTrigger.Type = RfsaReferenceTriggerType.None
                rfsaSession.Configuration.Triggers.ReferenceTrigger.Disable()
                Exit Select
            Case RfsaReferenceTriggerType.DigitalEdge
                rfsaSession.Configuration.Triggers.ReferenceTrigger.Type = RfsaReferenceTriggerType.DigitalEdge
                rfsaSession.Configuration.Triggers.ReferenceTrigger.DigitalEdge.Edge = RfsaTriggerEdge.Rising
                rfsaSession.Configuration.Triggers.ReferenceTrigger.DigitalEdge.Source = TriggerSource
                rfsaSession.Configuration.Triggers.ReferenceTrigger.PreTriggerSamples = PreTriggerSamples
                Exit Select
            Case RfsaReferenceTriggerType.SoftwareEdge
                rfsaSession.Configuration.Triggers.ReferenceTrigger.Type = RfsaReferenceTriggerType.SoftwareEdge
                rfsaSession.Configuration.Triggers.ReferenceTrigger.PreTriggerSamples = PreTriggerSamples
                Exit Select
            Case RfsaReferenceTriggerType.IQPowerEdge
                rfsaSession.Configuration.Triggers.ReferenceTrigger.Type = RfsaReferenceTriggerType.IQPowerEdge
                rfsaSession.Configuration.Triggers.ReferenceTrigger.IQPowerEdge.Level = TriggerLevel
                rfsaSession.Configuration.Triggers.ReferenceTrigger.IQPowerEdge.Source = RfsaIQPowerEdgeReferenceTriggerSource.Zero
                rfsaSession.Configuration.Triggers.ReferenceTrigger.IQPowerEdge.Slope = RfsaIQPowerEdgeReferenceTriggerSlope.Rising
                rfsaSession.Configuration.Triggers.ReferenceTrigger.PreTriggerSamples = PreTriggerSamples
                Exit Select
            Case Else
                rfsaSession.Configuration.Triggers.ReferenceTrigger.Disable()
                Exit Select
        End Select
    End Sub

    Private Sub FetchIQdata()
        Dim wfmInfo As RfsaWaveformInfo
        Dim dataPtr As ComplexDouble()
        Dim timespan As New PrecisionTimeSpan(10.0)
        dataPtr = rfsaSession.Acquisition.IQ.FetchIQSingleRecordComplex(Of ComplexDouble)(0, NumberOfSamples, timespan, wfmInfo)
        Me.dataGridViewResults.DataSource = dataPtr
    End Sub

    Private Sub InitiateAcquisition()
        rfsaSession.Acquisition.IQ.Initiate()
    End Sub

    Private Sub SendSoftwareTrigger()
        rfsaSession.Configuration.Triggers.ReferenceTrigger.SendSoftwareEdgeTrigger()
    End Sub

    Private Sub triggerTypeComboBox_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles triggerTypeComboBox.SelectedValueChanged
        If Me.triggerTypeComboBox.SelectedValue IsNot Nothing Then
            Dim triggerType As RfsaReferenceTriggerType = CType(Me.triggerTypeComboBox.SelectedValue, RfsaReferenceTriggerType)
            If triggerType = RfsaReferenceTriggerType.None Then
                Me.triggerSourceLabel.Visible = False
                Me.triggerSourceComboBox.Visible = False
                Me.pretriggerSamplesLabel.Visible = False
                Me.pretriggerSamplesNumeric.Visible = False
                Me.sendSoftwareTriggerbutton.Visible = False
                Me.triggerLevelLabel.Visible = False
                Me.triggerLevelNumeric.Visible = False
            ElseIf triggerType = RfsaReferenceTriggerType.SoftwareEdge Then
                Me.triggerSourceLabel.Visible = False
                Me.triggerSourceComboBox.Visible = False
                Me.pretriggerSamplesLabel.Visible = True
                Me.pretriggerSamplesNumeric.Visible = True
                Me.sendSoftwareTriggerbutton.Visible = True
                Me.sendSoftwareTriggerbutton.Enabled = False
                Me.triggerLevelLabel.Visible = False
                Me.triggerLevelNumeric.Visible = False
            ElseIf triggerType = RfsaReferenceTriggerType.IQPowerEdge Then
                Me.triggerSourceLabel.Visible = False
                Me.triggerSourceComboBox.Visible = False
                Me.pretriggerSamplesLabel.Visible = True
                Me.pretriggerSamplesNumeric.Visible = True
                Me.sendSoftwareTriggerbutton.Visible = False
                Me.triggerLevelLabel.Visible = True

                Me.triggerLevelNumeric.Visible = True
            Else
                Me.triggerSourceLabel.Visible = True
                Me.triggerSourceComboBox.Visible = True
                Me.pretriggerSamplesLabel.Visible = True
                Me.pretriggerSamplesNumeric.Visible = True
                Me.sendSoftwareTriggerbutton.Visible = False
                Me.triggerLevelLabel.Visible = False
                Me.triggerLevelNumeric.Visible = False
            End If
        End If
    End Sub

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.resourceNameComboBox.Enabled = isEnabled
        Me.referenceLevelNumeric.Enabled = isEnabled
        Me.carrierFrequencyNumeric.Enabled = isEnabled
        Me.iqRateNumeric.Enabled = isEnabled
        Me.samplesPerRecordNumeric.Enabled = isEnabled
        Me.triggerTypeComboBox.Enabled = isEnabled
        Me.pretriggerSamplesNumeric.Enabled = isEnabled
        Me.triggerSourceComboBox.Enabled = isEnabled
        Me.triggerLevelNumeric.Enabled = isEnabled
        Me.startAcquisitionButton.Enabled = isEnabled
    End Sub

    Private Sub startAcquisitionButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startAcquisitionButton.Click
        ChangeControlState(False)
        Try
            InitializeRfsaSession()
            ConfigureIQ()
            ConfigureRefTrigger()
            InitiateAcquisition()

            If TriggerType <> RfsaReferenceTriggerType.SoftwareEdge Then
                FetchIQdata()
                CloseSession()
            Else
                Me.sendSoftwareTriggerbutton.Enabled = True
            End If
        Catch ex As System.Exception
            ShowError(ex.Message)
            CloseSession()
            ChangeControlState(True)
        Finally
            If TriggerType <> RfsaReferenceTriggerType.SoftwareEdge Then
                ChangeControlState(True)
            End If
        End Try

    End Sub

    Private Sub sendSoftwareTriggerButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sendSoftwareTriggerbutton.Click
        sendSoftwareTriggerbutton.Enabled = False
        SendSoftwareTrigger()
        FetchIQdata()
        CloseSession()
        ChangeControlState(True)
    End Sub
End Class
