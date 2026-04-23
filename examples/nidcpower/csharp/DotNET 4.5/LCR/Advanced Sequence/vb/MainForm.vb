'=============================================================================================================
'
' Title:
'       NI-DCPower LCR Advanced Sequence Frequency Sweep
'
' Description
'      Demonstrates how to make sweep times faster by using LCR Advanced Sequence
'      functions to change the frequency in a sequence. During the sequence, the
'      channel steps through the predetermined set configurations (frequency in
'      this example) without any interaction with the host system. The advantage
'      of sequencing is that changes from one step in the sequence to the next
'      are deterministic. The program will perform an AC Voltage frequency sweep
'      from Start Frequency to End Frequency. The sweep frequencies are an
'      exponential sequence And the length of each of the sweeps is equal to the
'      value in the Number Of Steps control.
'
'  Suggested Device(s):
'      PXIe-4190
'
'============================================================================================================

Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Public Partial Class MainForm
    Inherits Form
    Dim dcPowerSession As NIDCPower

    Public Sub New()
        InitializeComponent()
        ConfigureMeasurementTimeComboBox()
        ConfigureCableLengthComboBox()
        ConfigureDcBiasSourceComboBox()
        LoadDCPowerDeviceNames()
    End Sub

#Region "MainForm initial configuration"

    Private Sub LoadDCPowerDeviceNames()
        Using dcPowerDevices As New ModularInstrumentsSystem("NI-DCPower")
            For Each device As DeviceInfo In dcPowerDevices.DeviceCollection
                resourceNameComboBox.Items.Add(device.Name + "/0")
            Next
        End Using
        If resourceNameComboBox.Items.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub

    Private Sub ConfigureMeasurementTimeComboBox()
        For Each item As DCPowerLCRMeasurementTime In [Enum].GetValues(GetType(DCPowerLCRMeasurementTime))
            lcrMeasurementTimeComboBox.Items.Add(item)
        Next
        lcrMeasurementTimeComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureCableLengthComboBox()
        For Each item As DCPowerCableLength In [Enum].GetValues(GetType(DCPowerCableLength))
            cableLengthComboBox.Items.Add(item)
        Next
        cableLengthComboBox.SelectedIndex = 2
    End Sub
    Private Sub ConfigureDcBiasSourceComboBox()
        For Each item As DCPowerLCRDCBiasSource In [Enum].GetValues(GetType(DCPowerLCRDCBiasSource))
            lcrDcBiasSourceComboBox.Items.Add(item)
        Next
        lcrDcBiasSourceComboBox.SelectedIndex = 0
    End Sub

#End Region

#Region "Mainform configuration values"

    Private ReadOnly Property ResourceName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property FullyQualifiedChannelName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property StartFrequency As Double
        Get
            Return Decimal.ToDouble(Me.startFrequencyNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property EndFrequency As Double
        Get
            Return Decimal.ToDouble(Me.endFrequencyNumeric.Value)
        End Get
    End Property


    Private ReadOnly Property NumberOfSteps() As Integer
        Get
            Return Convert.ToInt32(Me.numberOfStepsNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property CableLength As DCPowerCableLength
        Get
            Return CType(Me.cableLengthComboBox.SelectedItem, DCPowerCableLength)
        End Get
    End Property

    Private ReadOnly Property ImpedanceRange() As Double
        Get
            Return Decimal.ToDouble(Me.lcrImpedanceRangeNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property AcVoltage() As Double
        Get
            Return Decimal.ToDouble(Me.VoltageAmplitudeRmsNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property LcrDcBiasSource() As DCPowerLCRDCBiasSource
        Get
            Return CType(Me.lcrDcBiasSourceComboBox.SelectedItem, DCPowerLCRDCBiasSource)
        End Get
    End Property

    Private ReadOnly Property DcBiasVoltage() As Double
        Get
            Return Decimal.ToDouble(Me.lcrDcBiasVoltageLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property DcBiasCurrent() As Double
        Get
            Return Decimal.ToDouble(Me.lcrDcBiasCurrentLevelNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property LcrMeasurementTime() As DCPowerLCRMeasurementTime
        Get
            Return CType(Me.lcrMeasurementTimeComboBox.SelectedItem, DCPowerLCRMeasurementTime)
        End Get
    End Property

    Private ReadOnly Property LcrCustomMeasurementTimeSec() As Double
        Get
            Return Decimal.ToDouble(Me.lcrCustomMeasurementTimeSecNumeric.Value)
        End Get
    End Property

    Private ReadOnly Property LcrOpenCompensationEnabled() As Boolean
        Get
            Return lcrOpenCompensationEnabledCheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property LcrShortCompensationEnabled() As Boolean
        Get
            Return lcrShortCompensationEnabledCheckBox.Checked
        End Get
    End Property
#End Region

    Private Sub startButton_Click(sender As Object, e As EventArgs) Handles startButton.Click
        ChangeControlState(False)
        ClearMeasurementsDataGridView()
        Start()
        ChangeControlState(True)
    End Sub

    Private Sub Start()
        If CableLength = DCPowerCableLength.CustomAsConfigured Then
            MessageBox.Show("Cable Length - (Custom) As Configured is selected. This cable length option is not supported in this example. Search ni.com for information about NI power supplies/SMUs and the NI-DCPower API.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Specify the Advanced Sequence Attributes which can change per step.
        Dim advancedSequenceProperties As DCPowerAdvancedSequenceProperty() = {DCPowerAdvancedSequenceProperty.LcrFrequency}

        Try
            InitializeDCPowerSession()

            ' Allocate enough memory for the sequence steps.
            Dim frequenciesSequence As Double() = New Double(NumberOfSteps - 1) {}

            dcPowerSession.Source.Mode = DCPowerSourceMode.Sequence

            dcPowerSession.Outputs(FullyQualifiedChannelName).InstrumentMode = DCPowerInstrumentMode.LCR
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.StimulusFunction = DCPowerLCRStimulusFunction.ACVoltage
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.VoltageAmplitude = AcVoltage
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.ImpedanceRange = ImpedanceRange
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.DCBiasSource = LcrDcBiasSource
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.DCBiasVoltageLevel = DcBiasVoltage
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.DCBiasCurrentLevel = DcBiasCurrent
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.MeasurementTime = LcrMeasurementTime
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.CustomMeasurementTime = LcrCustomMeasurementTimeSec
            dcPowerSession.Outputs(FullyQualifiedChannelName).DeviceSpecific.LCR.CableLength = CableLength
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.Compensation.OpenCompensationEnabled = LcrOpenCompensationEnabled
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.Compensation.ShortCompensationEnabled = LcrShortCompensationEnabled

            Dim advancedSequenceName As String = "MySequence"
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.AdvancedSequencing.CreateAdvancedSequence(advancedSequenceName, advancedSequenceProperties, True)

            Dim minFrequency As Double = If((StartFrequency < EndFrequency), StartFrequency, EndFrequency)
            Dim maxFrequency As Double = If((StartFrequency > EndFrequency), StartFrequency, EndFrequency)
            Dim nthRootFrequencyRatio As Double = Math.Pow(maxFrequency / minFrequency, 1.0 / (NumberOfSteps - 1))
            Dim frequenciesSequenceValue As Double

            For pointIndex As Integer = 0 To NumberOfSteps - 1
                ' Create new Advanced Sequence Step for every frequency.
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.AdvancedSequencing.CreateAdvancedSequenceStep(True)
                ' Calculate the Frequency for this step.
                frequenciesSequenceValue = Math.Pow(nthRootFrequencyRatio, pointIndex) * minFrequency

                dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.Frequency = frequenciesSequenceValue
            Next


            dcPowerSession.Control.Initiate()

            Dim timeout As New PrecisionTimeSpan(300.0)
            ' Wait for the sequence engine to be done.
            dcPowerSession.Outputs(FullyQualifiedChannelName).Events.SequenceEngineDoneEvent.WaitForEvent(timeout)

            Dim results As NILCRMeasurement() = dcPowerSession.Measurement.FetchLCR(FullyQualifiedChannelName, timeout, NumberOfSteps)
            DisplayMeasurements(results)

            dcPowerSession.Outputs(FullyQualifiedChannelName).Control.Abort()
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.AdvancedSequencing.DeleteAdvancedSequence(advancedSequenceName)

            dcPowerSession.Utility.Reset(FullyQualifiedChannelName)

        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
        End Try
    End Sub

    Private Sub DisplayMeasurements(results As NILCRMeasurement())
        For i As Integer = 0 To results.Length - 1
            measurementsDataGridView.Rows.Add((i + 1).ToString(), results(i).stimulusFrequency.ToString("E"), results(i).ZMagnitude.ToString("E"))
        Next
    End Sub

    Private Sub InitializeDCPowerSession()
        dcPowerSession = New NIDCPower(FullyQualifiedChannelName, False, String.Empty)
        AddHandler dcPowerSession.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)
    End Sub

    Private Sub DCPowerDriverOperationWarning(sender As Object, e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub mainForm_Closing(sender As Object, e As FormClosingEventArgs)
        CloseSession()
    End Sub

    Private Sub ClearMeasurementsDataGridView()
        measurementsDataGridView.Rows.Clear()
    End Sub

    Private Sub CloseSession()
        If dcPowerSession IsNot Nothing Then
            Try
                dcPowerSession.Close()
                dcPowerSession = Nothing
            Catch ex As Exception
                ShowError(ex)
                Application.[Exit]()
            End Try
        End If
    End Sub

    Private Shared Sub ShowError(ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub ChangeControlState(isEnabled As Boolean)
        Me.configurationGroupBox.Enabled = isEnabled
        Me.startButton.Enabled = isEnabled
        Me.resourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub

    Private Sub resourceNameComboBox_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

End Class