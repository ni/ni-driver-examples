'******************************************************************************
'*
'* Example program:
'*    Getting Started Multi Record IQ 
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'*   Use this example to learn the basics of Multi Record I/Q acquisition using
'    the RF vector signal analyzer. The example shows how to configure the following parameters
'	of I/Q acquisition: reference clock, reference level, carrier frequency, I/Q
'	rate, number of samples per record, number of records, I/Q acquisition type, and reference triggers.
'	Multi Record acquisition is non-continuous, so the example includes a triggering setup
'	to reinforce this property. This example also reads and display each record's I/Q data on a dataGrid.
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
'*   6. Configure the Reference Trigger Type in the UI.
'*
'*   7. Configure the PreTrigger Samples and the trigger Level in the UI.
'*
'*   8. Select the Acquire Button in the UI to start the aqcuisition. 
'*      If the Reference Trigger is set to Software, press the send Software Trigger Button "Number Of Records" times to perform Fetch operation
'*   
'*   9. The data is displayed in the DataGrid.
'*  
'* I/O Connections Overview:
'*   Make sure your signal input terminals match the Physical Channel I/O
'*   Controls.  If you have a PXI chassis, ensure that it has been properly
'*   identified in MAX. 

'* Known Issues
'*   This example may give Design-Time error and warnings if the Designer is opened in Visual Studio before building the project.
'*   To view the MainForm Designer after the error has occured, one should close the file, build the project and then open the Designer. 
'*
'******************************************************************************

Imports System.Collections.Generic
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices
Imports NationalInstruments.ModularInstruments.NIRfsa
Imports NationalInstruments

Partial Public Class MainForm
    Inherits Form
    Private rfsaSession As NIRfsa
    Private swTriggerCount As Integer = 0

    Public Sub New()
        InitializeComponent()
        ConfigureTriggerTypeComboBox()
        ConfigureRefClockComboBox()
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
        Dim triggerTypeValueList As List(Of KeyValuePair(Of String, RfsaReferenceTriggerType)) = New List(Of KeyValuePair(Of String, RfsaReferenceTriggerType))()
        triggerTypeValueList.Add(New KeyValuePair(Of String, RfsaReferenceTriggerType)("None", RfsaReferenceTriggerType.None))
        triggerTypeValueList.Add(New KeyValuePair(Of String, RfsaReferenceTriggerType)("Digital Edge", RfsaReferenceTriggerType.DigitalEdge))
        triggerTypeValueList.Add(New KeyValuePair(Of String, RfsaReferenceTriggerType)("IQ Power Edge", RfsaReferenceTriggerType.IQPowerEdge))
        triggerTypeValueList.Add(New KeyValuePair(Of String, RfsaReferenceTriggerType)("Software", RfsaReferenceTriggerType.SoftwareEdge))
        referenceTriggerTypeComboBox.DisplayMember = "Key"
        referenceTriggerTypeComboBox.ValueMember = "Value"
        referenceTriggerTypeComboBox.DataSource = triggerTypeValueList
        referenceTriggerTypeComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureRefClockComboBox()
        Dim refClockValueList As List(Of KeyValuePair(Of String, RfsaReferenceClockSource)) = New List(Of KeyValuePair(Of String, RfsaReferenceClockSource))()
        refClockValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("OnboardClock", RfsaReferenceClockSource.OnboardClock))
        refClockValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("RefIn", RfsaReferenceClockSource.ReferenceIn))
        refClockValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("PXI_Clk", RfsaReferenceClockSource.PxiClock))
        refClockValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("ClkIn", RfsaReferenceClockSource.ClockIn))
        referenceClockComboBox.DisplayMember = "Key"
        referenceClockComboBox.ValueMember = "Value"
        referenceClockComboBox.DataSource = refClockValueList
        referenceClockComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureTriggerSourceComboBox()
        Dim triggerSourceValueList As List(Of KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource)) = New List(Of KeyValuePair(Of String, RfsaDigitalEdgeReferenceTriggerSource))()
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

#Region "UI Values"

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

    Private ReadOnly Property NumberOfRecords() As Integer
        Get
            Return Decimal.ToInt32(Me.numberOfRecordsNumeric.Value)
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

    Private ReadOnly Property ReferenceClock() As String
        Get
            Return If(TryCast(Me.referenceClockComboBox.SelectedValue, RfsaReferenceClockSource), RfsaReferenceClockSource.FromString(Me.referenceClockComboBox.Text))
        End Get
    End Property

    Private ReadOnly Property ReferenceTriggerType() As RfsaReferenceTriggerType
        Get
            Return CType(Me.referenceTriggerTypeComboBox.SelectedValue, RfsaReferenceTriggerType)
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

#End Region

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles acquireButton.Click
        ChangeControlState(False)
        Try
            InitializeRfsaSession()
            ConfigureRefClock()
            ConfigureIQ()
            ConfigureReferenceTrigger()
            InitiateRfsa()

            If ReferenceTriggerType <> RfsaReferenceTriggerType.SoftwareEdge Then
                FetchIQData()
                CloseSession()
            Else
                swTriggerCount = 0
                sendSoftwareTriggerbutton.Enabled = True
                sendSoftwareTriggerbutton.Focus()
            End If

        Catch ex As System.Exception
            ShowError(ex.Message)
            CloseSession()
        Finally
            ChangeControlState(True)
        End Try
    End Sub


    Private Sub FetchIQData()
        Dim wfmInfo As RfsaWaveformInfo() = New RfsaWaveformInfo(NumberOfRecords - 1) {}
        Dim data As ComplexDouble(,)
        Dim timespan As New PrecisionTimeSpan(10.0)
        data = rfsaSession.Acquisition.IQ.FetchIQMultiRecordComplex(Of ComplexDouble)(0, NumberOfRecords, NumberOfSamples, timespan, wfmInfo)
        Me.multiDataGridViewResults.SetData(data)
    End Sub

    Private Sub InitiateRfsa()
        rfsaSession.Acquisition.IQ.Initiate()
    End Sub

    Private Sub ConfigureReferenceTrigger()
        Dim triggerType As RfsaReferenceTriggerType = ReferenceTriggerType
        Select Case triggerType
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
        AddHandler rfsaSession.DriverOperation.Warning, New System.EventHandler(Of RfsaWarningEventArgs)(AddressOf DriverOperationWarning)
    End Sub

    Private Sub ConfigureRefClock()
        rfsaSession.Configuration.ReferenceClock.Source = ReferenceClock
        rfsaSession.Configuration.ReferenceClock.Rate = 10000000.0
    End Sub

    Private Sub ConfigureIQ()
        rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ
        rfsaSession.Configuration.Vertical.ReferenceLevel = ReferenceLevel
        rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFrequency
        rfsaSession.Configuration.IQ.NumberOfSamples = NumberOfSamples
        rfsaSession.Configuration.IQ.NumberOfSamplesIsFinite = True
        rfsaSession.Configuration.IQ.NumberOfRecords = NumberOfRecords
        rfsaSession.Configuration.IQ.NumberOfRecordsIsFinite = True
        rfsaSession.Configuration.IQ.IQRate = IQRate
    End Sub

    Private Sub DriverOperationWarning(ByVal sender As Object, ByVal e As RfsaWarningEventArgs)
        MessageBox.Show(e.Warning.ToString(), "Warning")
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

    Private Sub sendSoftwareTriggerbutton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles sendSoftwareTriggerbutton.Click
        rfsaSession.Configuration.Triggers.ReferenceTrigger.SendSoftwareEdgeTrigger()
        swTriggerCount += 1
        If swTriggerCount = NumberOfRecords Then
            FetchIQData()
            CloseSession()
            sendSoftwareTriggerbutton.Enabled = False
        End If

    End Sub

    Private Sub referenceTriggerTypeComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles referenceTriggerTypeComboBox.SelectedIndexChanged
        If Me.referenceTriggerTypeComboBox.SelectedValue IsNot Nothing Then
            Dim triggerType As RfsaReferenceTriggerType = CType(Me.referenceTriggerTypeComboBox.SelectedValue, RfsaReferenceTriggerType)

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


End Class

