'******************************************************************************
'*
'* Example program:
'*   List Mode Power Triggered
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'*   Use this example to learn the basics of RF list mode using the RF vector
'	signal analyzer and how to create a configuration list and configuration list steps.
'	It uses RF list mode to vary the I/Q carrier frequency and reference level 
'	across different steps which advance on an I/Q power edge reference trigger.
'                         
'
'* Instructions for running:
'*   1. Configure RFSA device in the MAX for the program to run. 
'*
'*	2. Configure the IQ Rate, Samples Per Record and Number of Steps in the UI.
'        The number of steps created in the List are taken from Number of Steps.
'*   
'*   3. Configure the IQ Carrier Frequency Start ans Stop Values in the UI.
'*
'*	4. Configure the Reference Level Start ans Stop Values in the UI.
'*
'*	5. Configure the Power Triggered Level Start ans Stop Values in the UI.
'*   	
'*   6. Select the Acquire Button in the UI to start the acquisition.
'*   
'*   7. The data is displayed in the DataGrid.
'*
'* Note: Each Start and Stop Values are partitioned by the number of Steps for each step in Configuration List. 
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

Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices
Imports NationalInstruments.ModularInstruments.NIRfsa

Partial Public Class MainForm
    Inherits Form
    Private rfsaSession As NIRfsa

    Public Sub New()
        InitializeComponent()
        LoadRfsaDeviceNames()
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

#End Region

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

    Private ReadOnly Property ResourceName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property IQRate() As Double
        Get
            Return Decimal.ToDouble(Me.iqRateNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CarrierFreqRmpStart() As Double
        Get
            Return Decimal.ToDouble(Me.carrierFreqRampStrtNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CarrierFreqRmpStop() As Double
        Get
            Return Decimal.ToDouble(Me.carrierFreqRampStopNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property RefLevelRmpStart() As Double
        Get
            Return Decimal.ToDouble(Me.referenceLevelRampStrtNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property RefLevelRmpStop() As Double
        Get
            Return Decimal.ToDouble(Me.referenceLevelRampStopNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PowerTriggerLevelRmpStart() As Double
        Get
            Return Decimal.ToDouble(Me.triggerLevelRampStrtNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property PowerTriggerLevelRmpStop() As Double
        Get
            Return Decimal.ToDouble(Me.triggerLevelRampStopNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property NumberOfSamples() As Integer
        Get
            Return Decimal.ToInt32(Me.samplesPerRecordNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property NumberOfSteps() As Integer
        Get
            Return Decimal.ToInt32(Me.numberOfStepsNumeric.Value)
        End Get
    End Property

    Private Sub acquireButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles acquireButton.Click
        ChangeControlState(False)
        Try
            InitializeRfsaSession()
            ConfigureForIQ()
            ConfigureForMultiRecord()
            ConfigureListMode()
            InitiateAcquisition()
            FetchIQData()
            CloseRfsaSession()
        Catch ex As System.Exception
            ShowError(ex.Message)
            CloseRfsaSession()
        Finally
            ChangeControlState(True)
        End Try

    End Sub

    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.acquireButton.Enabled = isEnabled
        Me.resourceNameComboBox.Enabled = isEnabled
        Me.iqRateNumeric.Enabled = isEnabled
        Me.samplesPerRecordNumeric.Enabled = isEnabled
        Me.numberOfStepsNumeric.Enabled = isEnabled
        Me.carrierFreqRampStrtNumeric.Enabled = isEnabled
        Me.carrierFreqRampStopNumeric.Enabled = isEnabled
        Me.referenceLevelRampStrtNumeric.Enabled = isEnabled
        Me.referenceLevelRampStopNumeric.Enabled = isEnabled
        Me.triggerLevelRampStrtNumeric.Enabled = isEnabled
        Me.triggerLevelRampStopNumeric.Enabled = isEnabled
    End Sub

    Private Sub InitializeRfsaSession()
        CloseRfsaSession()
        rfsaSession = New NIRfsa(ResourceName, True, False)
        AddHandler rfsaSession.DriverOperation.Warning, New System.EventHandler(Of RfsaWarningEventArgs)(AddressOf DriverOperationWarning)
    End Sub

    Private Sub DriverOperationWarning(ByVal o As Object, ByVal e As RfsaWarningEventArgs)
        MessageBox.Show(e.Warning.ToString(), "Warning")
    End Sub

    Private Sub ConfigureForIQ()
        rfsaSession.Configuration.AcquisitionType = RfsaAcquisitionType.IQ
        rfsaSession.Configuration.IQ.IQRate = IQRate
    End Sub

    Private Sub ConfigureForMultiRecord()
        rfsaSession.Configuration.IQ.NumberOfSamples = NumberOfSamples
        rfsaSession.Configuration.IQ.NumberOfSamplesIsFinite = True
        rfsaSession.Configuration.IQ.NumberOfRecords = NumberOfSteps
        rfsaSession.Configuration.IQ.NumberOfRecordsIsFinite = True
    End Sub

    Private Sub FetchIQData()
        Dim _timespan As New PrecisionTimeSpan(5.0)
        Dim _wfmInfo As RfsaWaveformInfo() = New RfsaWaveformInfo(NumberOfSteps - 1) {}
        Dim data As ComplexDouble(,)
        data = rfsaSession.Acquisition.IQ.FetchIQMultiRecordComplex(Of ComplexDouble)(0, NumberOfSteps, NumberOfSamples, _timespan, _wfmInfo)
        multiDataGridViewResults.SetData(data)
    End Sub

    Private Sub InitiateAcquisition()
        rfsaSession.Acquisition.IQ.Initiate()
    End Sub

    Private Sub ConfigureListMode()
        Dim propertyList As RfsaConfigurationListProperties() = New RfsaConfigurationListProperties() {RfsaConfigurationListProperties.IQCarrierFrequency, RfsaConfigurationListProperties.ReferenceLevel, RfsaConfigurationListProperties.IQPowerEdgeRefTriggerLevel}

        Dim carrierFreqRampIncrement As Double = (CarrierFreqRmpStop - CarrierFreqRmpStart) / NumberOfSteps
        Dim refLevelRampIncrement As Double = (RefLevelRmpStop - RefLevelRmpStart) / NumberOfSteps
        Dim powerTriggerLevelRampIncrement As Double = (PowerTriggerLevelRmpStop - PowerTriggerLevelRmpStart) / NumberOfSteps

        rfsaSession.Configuration.SignalPath.LocalOscillator.FrequencySettlingUnits = RfsaFrequencySettlingUnits.SecondsAfterIO
        rfsaSession.Configuration.SignalPath.LocalOscillator.FrequencySettlingTime = 0.005

        Dim instrumentModel As String = rfsaSession.Identity.InstrumentModel

        If [String].Equals(instrumentModel, "NI PXIe-5665 (3.6GHz)", StringComparison.Ordinal) OrElse [String].Equals(instrumentModel, "NI PXIe-5665 (14GHz)", StringComparison.Ordinal) OrElse [String].Equals(instrumentModel, "NI PXIe-5667 (3.6GHz)", StringComparison.Ordinal) OrElse [String].Equals(instrumentModel, "NI PXIe-5667 (7GHz)", StringComparison.Ordinal) Then
            rfsaSession.Configuration.SignalPath.LocalOscillator.LOYigMainCoilDrive = RfsaLOYigMainCoilDrive.Fast
        Else
            ' Otherwise, set the Downconverter Loop Bandwidth to Wide 
            rfsaSession.Configuration.SignalPath.LocalOscillator.DownconverterLoopBandwidth = RfsaDownconverterLoopBandwidth.Wide
        End If

        rfsaSession.Configuration.Triggers.ReferenceTrigger.Type = RfsaReferenceTriggerType.IQPowerEdge
        rfsaSession.Configuration.BasicConfigurationList.CreateConfigurationList("exampleList", propertyList, True)

        For ii As Integer = 0 To NumberOfSteps - 1
            rfsaSession.Configuration.BasicConfigurationList.CreateStep(True)
            rfsaSession.Configuration.IQ.CarrierFrequency = CarrierFreqRmpStart + (ii * carrierFreqRampIncrement)
            rfsaSession.Configuration.Vertical.ReferenceLevel = RefLevelRmpStart + (ii * refLevelRampIncrement)
            rfsaSession.Configuration.Triggers.ReferenceTrigger.IQPowerEdge.Level = PowerTriggerLevelRmpStart + (ii * powerTriggerLevelRampIncrement)
            rfsaSession.Configuration.Triggers.ReferenceTrigger.IQPowerEdge.Source = RfsaIQPowerEdgeReferenceTriggerSource.Zero
            rfsaSession.Configuration.Triggers.ReferenceTrigger.IQPowerEdge.Slope = RfsaIQPowerEdgeReferenceTriggerSlope.Rising
            rfsaSession.Configuration.Triggers.ReferenceTrigger.PreTriggerSamples = 0
        Next

    End Sub

    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")


    End Sub
End Class
