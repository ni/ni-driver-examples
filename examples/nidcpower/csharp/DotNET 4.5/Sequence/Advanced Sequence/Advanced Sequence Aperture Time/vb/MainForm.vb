'=============================================================================================================
'
' Title:
'      NI-DCPower Advanced Sequence Aperture Time
'
' Description:
'       Demonstrates how to use the Advanced Sequence functions to output a
' DC Voltage with different aperture times.  This example initializes a
' session, configures the Output Function, Source Delay, creates an advanced
' sequence with multiple aperture times, initiates generation, waits for a
' specified delay, and then measures the voltage and current output.
' This example uses Advanced Sequence mode.
'
'  Suggested Devices:
'      PXIe-4135, PXIe-4136, PXIe-4137, PXIe-4138, PXIe-4139, PXIe-4140, PXIe-4141, PXIe-4142, PXIe-4143, PXIe-4144, PXIe-4145, PXIe-4147, PXIe-4162, PXIe-4163
'
'============================================================================================================

Imports System.Windows.Forms
Imports NationalInstruments
Imports NationalInstruments.ModularInstruments.NIDCPower
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Public Partial Class MainForm
    Inherits Form
    Const ApertureSequenceSize As Integer = 3
    Dim dcPowerSession As NIDCPower

    Public Sub New()
        InitializeComponent()
        LoadDCPowerDeviceNames()
        ConfigureApertureTimesDataGridView()
    End Sub

#Region "MainForm initial configuration"

    Sub LoadDCPowerDeviceNames()
        Using dcPowerDevices As New ModularInstrumentsSystem("NI-DCPower")
            For Each device As DeviceInfo In dcPowerDevices.DeviceCollection
                resourceNameComboBox.Items.Add(device.Name)
            Next
        End Using
        If resourceNameComboBox.Items.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub

    Sub ConfigureApertureTimesDataGridView()
        apertureTimesDataGridView.Rows.Add((0.016).ToString("E"))
        apertureTimesDataGridView.Rows.Add((0.1).ToString("E"))
        apertureTimesDataGridView.Rows.Add((0.0001).ToString("E"))
    End Sub

#End Region

#Region "Mainform configuration values"

    ReadOnly Property ResourceName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    ReadOnly Property ChannelName() As String
        Get
            Return Me.channelNameTextBox.Text
        End Get
    End Property

    ReadOnly Property FullyQualifiedChannelName() As String
        Get
            Return $"{ResourceName}/{ChannelName}"
        End Get
    End Property

    ReadOnly Property VoltageLevel() As Double
        Get
            Return Convert.ToDouble(voltageLevelNumeric.Value)
        End Get
    End Property

    ReadOnly Property SourceDelay() As PrecisionTimeSpan
        Get
            Return New PrecisionTimeSpan(Convert.ToDouble(Me.sourceDelayNumeric.Value))
        End Get
    End Property

    ReadOnly Property ApertureTime() As Double()
        Get
            Dim sequence As Double() = New Double(ApertureSequenceSize - 1) {}
            For i As Integer = 0 To ApertureSequenceSize - 1
                sequence(i) = [Double].Parse(apertureTimesDataGridView.Rows(i).Cells(0).Value.ToString())
            Next
            Return sequence
        End Get
    End Property

#End Region

    Sub startButton_Click(sender As Object, e As EventArgs)
        ChangeControlState(False)
        ClearMeasurementsDataGridView()
        Start()
        ChangeControlState(True)
    End Sub

    Sub Start()

        ' Specify the number of Advanced Sequence Attribute IDs.
        Dim advancedSequenceAttributeCount As Integer = 2

        ' Specify the Advanced Sequence Attributes which can change per step.
        Dim advancedSequenceProperties As DCPowerAdvancedSequenceProperty() = {DCPowerAdvancedSequenceProperty.VoltageLevel, DCPowerAdvancedSequenceProperty.OutputFunction}
        Try
            InitializeDCPowerSession()

            ' Configure the Source mode to Sequence.
            dcPowerSession.Source.Mode = DCPowerSourceMode.Sequence

            ' Set the Output Function to DC Voltage.  If you change the Output
            ' Function to DC Current, you must use Current Level and Voltage Limit
            ' instead of Voltage Level and Current Limit.
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Output.[Function] = DCPowerSourceOutputFunction.DCVoltage

            ' Configure the source delay.  This is the amount of time the device
            ' waits after programming the output. The source operation is complete
            ' after this delay.
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.SourceDelay = SourceDelay

            Dim advancedSequenceName As String = "MySequence"
            Dim tempVoltageLevel As Double

            ' Create the Advanced Sequence.
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.AdvancedSequencing.CreateAdvancedSequence(advancedSequenceName, advancedSequenceProperties, True)

            ' Create new Advanced Sequence Step for every Advanced Sequence Attribute.
            tempVoltageLevel = VoltageLevel
            For i As Integer = 0 To ApertureSequenceSize - 1

                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.AdvancedSequencing.CreateAdvancedSequenceStep(True)

                ' Configure the Voltage Level.
                dcPowerSession.Outputs(FullyQualifiedChannelName).Source.Voltage.VoltageLevel = tempVoltageLevel
                tempVoltageLevel = tempVoltageLevel + 1

                ' Configure the Aperture Time.
                dcPowerSession.Outputs(FullyQualifiedChannelName).Measurement.ApertureTime = ApertureTime(i)
            Next

            ' Initiate the device to start generation and acquisition.
            dcPowerSession.Control.Initiate()

            ' Wait for Sequence Engine Done event.
            Dim timeout As New PrecisionTimeSpan(10.0)
            dcPowerSession.Outputs(FullyQualifiedChannelName).Events.SequenceEngineDoneEvent.WaitForEvent(timeout)

            ' Measure voltage.
            Dim result As DCPowerFetchResult = dcPowerSession.Measurement.Fetch(FullyQualifiedChannelName, timeout, ApertureSequenceSize)

            DisplayMeasurements(result)

            ' Abort the session.
            dcPowerSession.Control.Abort()

            ' Delete the Advanced Sequence.
            dcPowerSession.Outputs(FullyQualifiedChannelName).Source.AdvancedSequencing.DeleteAdvancedSequence(advancedSequenceName)

            ' Reset to disable the output.
            dcPowerSession.Utility.Reset()
        Catch ex As Exception
            ShowError(ex)
        Finally
            CloseSession()
        End Try

    End Sub

    Sub InitializeDCPowerSession()
        dcPowerSession = New NIDCPower(FullyQualifiedChannelName, False, String.Empty)
        AddHandler dcPowerSession.DriverOperation.Warning, New EventHandler(Of DCPowerWarningEventArgs)(AddressOf DCPowerDriverOperationWarning)
    End Sub

    Sub DCPowerDriverOperationWarning(sender As Object, e As DCPowerWarningEventArgs)
        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Sub DisplayMeasurements(result As DCPowerFetchResult)
        For i As Integer = 0 To result.VoltageMeasurements.Length - 1
            voltageMeasurmentsDataGridView.Rows.Add(result.VoltageMeasurements(i).ToString("E"))
        Next
    End Sub

    Sub mainForm_Closing(sender As Object, e As FormClosingEventArgs)
        CloseSession()
    End Sub

    Sub ClearMeasurementsDataGridView()
        voltageMeasurmentsDataGridView.Rows.Clear()
    End Sub

    Sub CloseSession()
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

    Shared Sub ShowError(ex As Exception)
        MessageBox.Show(ex.Message, ex.[GetType]().ToString(), MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Sub ChangeControlState(isEnabled As Boolean)
        resourceNameAndChannelNameGroupBox.Enabled = isEnabled
        configurationGroupBox.Enabled = isEnabled
        startButton.Enabled = isEnabled
        resourceNameComboBox.[Select]()
        Me.Refresh()
    End Sub
End Class