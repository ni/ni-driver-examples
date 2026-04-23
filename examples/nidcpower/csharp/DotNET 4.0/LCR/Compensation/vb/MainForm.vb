'=============================================================================================================
'
' Title:
'      NI-DCPower LCR Compensation Example
'
' Description:
'      Demonstrates how to perform and apply cable compensation and LCR compensation to your LCR measurements.
'      This example does the following:
'      - Initializes a session
'      - If the Generate LCR Custom Cable Compensation Data checkbox is checked:
'          - Prompts a message to the user to set up an open connection to perform open custom cable compensation.
'          - Prompts a message to the user to set up a short connection to perform short custom cable compensation.
'      - If the Generate LCR Short Compensation Data checkbox is checked:
'          - Sets the cable length configuration to apply cable compensation data when performing LCR compensation.
'          - Prompts the user to set up a short connection to perform short LCR compensation.
'      - If the Generate LCR Open Compensation Data checkbox is checked:
'          - Sets the cable length configuration to apply cable compensation data when performing LCR compensation.
'          - Prompts the user to set up an open connection to perform open LCR compensation.
'      - If the Generate LCR Load Compensation Data checkbox is checked:
'          - Sets configuration to apply cable compensation data when performing LCR compensation.
'          - Prompts the user to set up a load connection to perform load LCR compensation.
'      - Sets the LCR Open/Short/Load Compensation Data Source and applies the LCR compensation data to LCR
'         measurements.
'      - Sets the cable length configuration to apply cable compensation data to LCR sourcing.
'      - Sets the sourcing configurations, initiates generation, waits for the sourcing to complete,
'         and measures the output.
'      - Closes the session.
'
'  Suggested Device(s):
'      PXIe-4190
'
'=============================================================================================================

Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private dcPowerSession As NIDCPower

    Public Sub New()
        InitializeComponent()
        ConfigureMeasurementTimeComboBox()
        ConfigureCableLengthComboBox()
        ConfigureDcBiasSourceComboBox()
        ConfigureLcrReferenceValueTypeComboBoxColumn()
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
        cableLengthComboBox.SelectedIndex = 1
    End Sub

    Private Sub ConfigureDcBiasSourceComboBox()
        For Each item As DCPowerLCRDCBiasSource In [Enum].GetValues(GetType(DCPowerLCRDCBiasSource))
            lcrDcBiasSourceComboBox.Items.Add(item)
        Next
        lcrDcBiasSourceComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureLcrReferenceValueTypeComboBoxColumn()
        loadCompensationSpotsDataGridView.Columns("columnSpotReferenceValueType").ValueType = GetType(DCPowerLCRReferenceValueType)
        For Each item As DCPowerLCRReferenceValueType In [Enum].GetValues(GetType(DCPowerLCRReferenceValueType))
            CType(loadCompensationSpotsDataGridView.Columns("columnSpotReferenceValueType"), DataGridViewComboBoxColumn).Items.Add(item)
        Next
    End Sub
#End Region

#Region "MainForm configuration values"

    Private ReadOnly Property FullyQualifiedChannelName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    Private ReadOnly Property Frequency() As Double
        Get
            Return Decimal.ToDouble(Me.lcrFrequencyUpDown.Value)
        End Get
    End Property

    Private ReadOnly Property ImpedanceRange() As Double
        Get
            Return Decimal.ToDouble(Me.lcrImpedanceRangeUpDown.Value)
        End Get
    End Property

    Private ReadOnly Property CableLength() As DCPowerCableLength
        Get
            Return CType(Me.cableLengthComboBox.SelectedItem, DCPowerCableLength)
        End Get
    End Property

    Private ReadOnly Property AcVoltage() As Double
        Get
            Return Decimal.ToDouble(Me.lcrVoltageUpDown.Value)
        End Get
    End Property

    Private ReadOnly Property LcrDcBiasSource() As DCPowerLCRDCBiasSource
        Get
            Return CType(Me.lcrDcBiasSourceComboBox.SelectedItem, DCPowerLCRDCBiasSource)
        End Get
    End Property

    Private ReadOnly Property DcBiasVoltage() As Double
        Get
            Return Decimal.ToDouble(Me.lcrDcBiasVoltageUpDown.Value)
        End Get
    End Property

    Private ReadOnly Property DcBiasCurrent() As Double
        Get
            Return Decimal.ToDouble(Me.lcrDcBiasCurrentUpDown.Value)
        End Get
    End Property

    Private ReadOnly Property LcrMeasurementTime() As DCPowerLCRMeasurementTime
        Get
            Return CType(Me.lcrMeasurementTimeComboBox.SelectedItem, DCPowerLCRMeasurementTime)
        End Get
    End Property

    Private ReadOnly Property LcrCustomMeasurementTime() As Double
        Get
            Return Decimal.ToDouble(Me.lcrCustomMeasurementTimeUpDown.Value)
        End Get
    End Property

    Private ReadOnly Property GenerateLcrCustomCableCompensationData() As Boolean
        Get
            Return Me.generateLcrCustomCableCompensationDataCheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property GenerateLcrOpenCompensationData() As Boolean
        Get
            Return Me.generateLcrOpenCompensationDataCheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property GenerateLcrShortCompensationData() As Boolean
        Get
            Return Me.generateLcrShortCompensationDataCheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property GenerateLcrLoadCompensationData() As Boolean
        Get
            Return Me.generateLcrLoadCompensationDataCheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property AdditionalFrequencies() As Double()
        Get
            Dim additionalFrequenciesList As List(Of Double) = New List(Of Double)()
            For Each row As DataGridViewRow In additionalCompensationFrequenciesDataGridView.Rows
                If Not String.IsNullOrEmpty(CType(row.Cells(0).Value, String)) Then
                    additionalFrequenciesList.Add(Convert.ToDouble(row.Cells(0).Value))
                End If
            Next
            Return additionalFrequenciesList.ToArray()
        End Get
    End Property

    Private ReadOnly Property LoadCompensationSpots() As DCPowerLCRLoadCompensationSpot()
        Get
            Dim loadCompensationSpotsList As List(Of DCPowerLCRLoadCompensationSpot) = New List(Of DCPowerLCRLoadCompensationSpot)()
            For Each row As DataGridViewRow In loadCompensationSpotsDataGridView.Rows
                If Not String.IsNullOrEmpty(CType(row.Cells("columnSpotFrequency").Value, String)) And row.Cells("columnSpotReferenceValueType").Value IsNot Nothing And Not String.IsNullOrEmpty(CType(row.Cells("columnSpotReferenceValueA").Value, String)) And Not String.IsNullOrEmpty(CType(row.Cells("columnSpotReferenceValueB").Value, String)) Then
                    loadCompensationSpotsList.Add(New DCPowerLCRLoadCompensationSpot(Convert.ToDouble(row.Cells("columnSpotFrequency").Value), CType(row.Cells("columnSpotReferenceValueType").Value, DCPowerLCRReferenceValueType), Convert.ToDouble(row.Cells("columnSpotReferenceValueA").Value), Convert.ToDouble(row.Cells("columnSpotReferenceValueB").Value)))
                End If
            Next
            Return loadCompensationSpotsList.ToArray()
        End Get
    End Property

    Private ReadOnly Property EnableLcrOpenCompensation() As Boolean
        Get
            Return Me.enableLcrOpenCompensationCheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property EnableLcrShortCompensation() As Boolean
        Get
            Return Me.enableLcrShortCompensationCheckBox.Checked
        End Get
    End Property

    Private ReadOnly Property EnableLcrLoadCompensation() As Boolean
        Get
            Return Me.enableLcrLoadCompensationCheckBox.Checked
        End Get
    End Property

#End Region
    Private Sub startButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startButton.Click
        ChangeControlState(False)
        Start()
        ChangeControlState(True)
    End Sub

    Private Sub mainForm_Closing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        CloseSession()
    End Sub

    Private Sub Start()
        If CableLength = DCPowerCableLength.CustomAsConfigured Then
            MessageBox.Show("Cable Length - (Custom) As Configured is selected. This cable length option is not supported in this example. Search ni.com for information about NI power supplies/SMUs and the NI-DCPower API.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Try
            InitializeDCPowerSession()

            Dim isOpenConnectionSetUp As Boolean = False
            Dim isShortConnectionSetUp As Boolean = False

            If GenerateLcrCustomCableCompensationData Then
                ' Call PerformOpenCustomCableCompensation if the user clicks OK. Calling this function resets most session attributes to their default values.
                If MessageBox.Show("Set up an open connection between the LCR Meter's LO CUR, LO POT and HI CUR, HI POT terminals, then click Yes to generate open custom cable compensation data. Click No to skip", "Action is required!", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
                    isOpenConnectionSetUp = True
                    isShortConnectionSetUp = False
                    dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.Compensation.PerformOpenCustomCableCompensation()
                End If
                ' Call PerformShortCustomCableCompensation if the user clicks OK. Calling this function resets most session properties to their default values.
                If MessageBox.Show("Set up a short connection between the LCR Meter's LO CUR, LO POT and HI CUR, HI POT terminals, then click Yes to generate short custom cable compensation data. Click No to skip", "Action is required!", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
                    isOpenConnectionSetUp = False
                    isShortConnectionSetUp = True
                    dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.Compensation.PerformShortCustomCableCompensation()
                End If
            End If

            If GenerateLcrShortCompensationData Then
                ' Skip the short LCR compensation message if we already have a short connection
                If Not isShortConnectionSetUp Then
                    MessageBox.Show("Set up a short connection between the LCR Meter's LO CUR, LO POT and HI CUR, HI POT terminals, then click OK to perform short LCR compensation.", "Action is required!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    isOpenConnectionSetUp = False
                    isShortConnectionSetUp = True
                End If
                ' Set Cable Length before performing short LCR compensation.
                dcPowerSession.Outputs(FullyQualifiedChannelName).DeviceSpecific.LCR.CableLength = CableLength
                ' Call PerformShortCompensation. Calling this function resets most session properties to their default values.
                dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.Compensation.PerformShortCompensation(AdditionalFrequencies)
            End If

            If GenerateLcrOpenCompensationData Then
                ' Skip the open LCR compensation message if we already have an open connection
                If Not isOpenConnectionSetUp Then
                    MessageBox.Show("Set up an open connection between the LCR Meter's LO CUR, LO POT and HI CUR, HI POT terminals, then click OK to perform open LCR compensation.", "Action is required!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    isOpenConnectionSetUp = True
                    isShortConnectionSetUp = False
                End If
                ' Set Cable Length before performing open LCR compensation.
                dcPowerSession.Outputs(FullyQualifiedChannelName).DeviceSpecific.LCR.CableLength = CableLength
                ' Call PerformOpenCompensation. Calling this function resets most session properties to their default values.
                dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.Compensation.PerformOpenCompensation(AdditionalFrequencies)
            End If

            If GenerateLcrLoadCompensationData Then
                MessageBox.Show("Connect the reference load between the LCR Meter's LO CUR, LO POT and HI CUR, HI POT terminals, then click OK to perform load LCR compensation.", "Action is required!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' Set Cable Length before performing open LCR compensation.
                dcPowerSession.Outputs(FullyQualifiedChannelName).DeviceSpecific.LCR.CableLength = CableLength
                ' Set channel properties to the runtime settings before performing load LCR compensation.
                ' This acquires the load compensation data with the same configuration as used to test the DUT.
                dcPowerSession.Outputs(FullyQualifiedChannelName).DeviceSpecific.LCR.CableLength = CableLength
                dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.VoltageAmplitude = AcVoltage
                dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.ImpedanceRange = ImpedanceRange
                dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.DCBiasSource = LcrDcBiasSource
                dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.DCBiasVoltageLevel = DcBiasVoltage
                dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.DCBiasCurrentLevel = DcBiasCurrent
                dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.MeasurementTime = LcrMeasurementTime
                dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.CustomMeasurementTime = LcrCustomMeasurementTime
                ' Call PerformLoadCompensation. Calling this function resets most session properties to their default values.
                dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.Compensation.PerformLoadCompensation(LoadCompensationSpots)
            End If

            If GenerateLcrCustomCableCompensationData Or GenerateLcrOpenCompensationData Or GenerateLcrShortCompensationData Or GenerateLcrLoadCompensationData Then
                MessageBox.Show("Set up a connection between the LCR meter and your DUT. Then click OK to proceed to take LCR measurements.", "Action is required!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            dcPowerSession.Outputs(FullyQualifiedChannelName).InstrumentMode = DCPowerInstrumentMode.LCR
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.StimulusFunction = DCPowerLCRStimulusFunction.ACVoltage
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.Frequency = Frequency
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.ImpedanceRange = ImpedanceRange
            dcPowerSession.Outputs(FullyQualifiedChannelName).DeviceSpecific.LCR.CableLength = CableLength
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.VoltageAmplitude = AcVoltage
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.DCBiasSource = LcrDcBiasSource
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.DCBiasVoltageLevel = DcBiasVoltage
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.DCBiasCurrentLevel = DcBiasCurrent
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.MeasurementTime = LcrMeasurementTime
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.CustomMeasurementTime = LcrCustomMeasurementTime

            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.Compensation.OpenShortLoadCompensationDataSource = DCPowerLCROpenShortLoadCompensationDataSource.OnboardStorage
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.Compensation.OpenCompensationEnabled = EnableLcrOpenCompensation
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.Compensation.ShortCompensationEnabled = EnableLcrShortCompensation
            dcPowerSession.Outputs(FullyQualifiedChannelName).LCR.Compensation.LoadCompensationEnabled = EnableLcrLoadCompensation

            dcPowerSession.Control.Initiate()

            ' Wait for output to settle.
            dcPowerSession.Events.SourceCompleteEvent.WaitForEvent(New PrecisionTimeSpan(21.0))

            Dim results As NILCRMeasurement() = dcPowerSession.Measurement.MeasureLCR(FullyQualifiedChannelName)

            ' Though this example can return multiple measurements from multiple channels, it is designed
            ' to display only the first measurement taken across any number of channels. You can adapt the
            ' example to display multiple measurements from multiple channels.
            DisplayMeasurements(results(0))

            dcPowerSession.Utility.Reset(FullyQualifiedChannelName)
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
        End Try
    End Sub

    Private Sub InitializeDCPowerSession()
        dcPowerSession = New NIDCPower(FullyQualifiedChannelName, False, String.Empty)
        AddHandler dcPowerSession.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)
    End Sub

    Private Sub DCPowerDriverOperationWarning(ByVal sender As Object, ByVal e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

    Private Sub DisplayMeasurements(results As NILCRMeasurement)
        Me.vdcTextBox.Text = results.Vdc.ToString("E")
        Me.idcTextBox.Text = results.Idc.ToString("E")
        Me.stimulusFrequencyTextBox.Text = results.stimulusFrequency.ToString("E")
        Me.acVoltageTextBox.Text = results.ACVoltage.ToString("E")
        Me.acCurrentTextBox.Text = results.ACCurrent.ToString("E")
        Me.zTextBox.Text = results.Z.ToString("E")
        Me.zMagTextBox.Text = results.ZMagnitude.ToString("E")
        Me.zThetaTextBox.Text = results.ZPhase.ToString("E")
        Me.yTextBox.Text = results.Y.ToString("E")
        Me.yMagTextBox.Text = results.YMagnitude.ToString("E")
        Me.yThetaTextBox.Text = results.YPhase.ToString("E")
        Me.lsTextBox.Text = results.Ls.ToString("E")
        Me.csTextBox.Text = results.Cs.ToString("E")
        Me.rsTextBox.Text = results.Rs.ToString("E")
        Me.lpTextBox.Text = results.Lp.ToString("E")
        Me.cpTextBox.Text = results.Cp.ToString("E")
        Me.rpTextBox.Text = results.Rp.ToString("E")
        Me.dTextBox.Text = results.D.ToString("E")
        Me.qTextBox.Text = results.Q.ToString("E")
        Me.measurementModeTextBox.Text = results.measurementMode.ToString()
        Me.dcInComplianceButtonLed.BackColor = If(results.dcInCompliance, Color.Red, SystemColors.Control)
        Me.acInComplianceButtonLed.BackColor = If(results.acInCompliance, Color.Red, SystemColors.Control)
        Me.unbalancedButtonLed.BackColor = If(results.unbalanced, Color.Red, SystemColors.Control)
    End Sub

    Private Shared Sub ShowError(ByVal ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub ChangeControlState(isEnabled As Boolean)
        Me.configurationGroupBox.Enabled = isEnabled
        Me.generateLcrCompensationDataGroupBox.Enabled = isEnabled
        Me.applyCompensationDataGroupBox.Enabled = isEnabled
        Me.startButton.Enabled = isEnabled
        Me.resourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub
End Class