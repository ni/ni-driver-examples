'==================================================================================================
' Title        : FrequencyAndPowerSweep
' Description  : This example demonstrates how to create a Configuration List for sweeping frequency and/or power.                                                              
'==================================================================================================

Imports System.Collections
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg
    Const ArbPreFilterGain As Integer = -2
    Const FrequencyReferenceRate As Double = 10000000.0

    Public Sub New()
        InitializeComponent()
        LoadRfsgDeviceNames()
        ConfigureReferenceClockSourceComboBox()
        ConfigureListTriggerSourceComboBox()
    End Sub

    Private Sub LoadRfsgDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-Rfsg")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            resourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            resourceNameComboBox.SelectedIndex = 0
        End If
    End Sub

#Region "UI Initial Value Config Section"

    Private Sub ConfigureReferenceClockSourceComboBox()
        Dim referenceSourceValueList As New List(Of DictionaryEntry)()
        referenceSourceValueList.Add(New DictionaryEntry("OnboardClock", RfsgFrequencyReferenceSource.OnboardClock))
        referenceSourceValueList.Add(New DictionaryEntry("RefIn", RfsgFrequencyReferenceSource.ReferenceIn))
        referenceSourceValueList.Add(New DictionaryEntry("PXI_CLK", RfsgFrequencyReferenceSource.PxiClock))
        referenceSourceValueList.Add(New DictionaryEntry("ClkIn", RfsgFrequencyReferenceSource.ClockIn))
        referenceClockSourceComboBox.DataSource = referenceSourceValueList
        referenceClockSourceComboBox.DisplayMember = "Key"
        referenceClockSourceComboBox.ValueMember = "Value"
        referenceClockSourceComboBox.SelectedValue = RfsgFrequencyReferenceSource.OnboardClock

    End Sub

    Private Sub ConfigureListTriggerSourceComboBox()
        Dim referenceSourceValueList As New List(Of DictionaryEntry)()
        referenceSourceValueList.Add(New DictionaryEntry("TimerEvent", RfsgDigitalEdgeConfigurationListStepTriggerSource.TimerEvent))
        referenceSourceValueList.Add(New DictionaryEntry("PFI0", RfsgDigitalEdgeConfigurationListStepTriggerSource.Pfi0))
        referenceSourceValueList.Add(New DictionaryEntry("PFI1", RfsgDigitalEdgeConfigurationListStepTriggerSource.Pfi1))
        referenceSourceValueList.Add(New DictionaryEntry("PXI_Trig0", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine0))
        referenceSourceValueList.Add(New DictionaryEntry("PXI_Trig1", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine1))
        referenceSourceValueList.Add(New DictionaryEntry("PXI_Trig2", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine2))
        referenceSourceValueList.Add(New DictionaryEntry("PXI_Trig3", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine3))
        referenceSourceValueList.Add(New DictionaryEntry("PXI_Trig4", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine4))
        referenceSourceValueList.Add(New DictionaryEntry("PXI_Trig5", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine5))
        referenceSourceValueList.Add(New DictionaryEntry("PXI_Trig6", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine6))
        referenceSourceValueList.Add(New DictionaryEntry("PXI_Trig7", RfsgDigitalEdgeConfigurationListStepTriggerSource.PxiTriggerLine7))
        referenceSourceValueList.Add(New DictionaryEntry("Marker0Event", RfsgDigitalEdgeConfigurationListStepTriggerSource.Marker0Event))
        referenceSourceValueList.Add(New DictionaryEntry("Marker1Event", RfsgDigitalEdgeConfigurationListStepTriggerSource.Marker1Event))
        referenceSourceValueList.Add(New DictionaryEntry("Marker2Event", RfsgDigitalEdgeConfigurationListStepTriggerSource.Marker2Event))
        referenceSourceValueList.Add(New DictionaryEntry("Marker3Event", RfsgDigitalEdgeConfigurationListStepTriggerSource.Marker3Event))
        listTriggerSourceComboBox.DataSource = referenceSourceValueList
        listTriggerSourceComboBox.DisplayMember = "Key"
        listTriggerSourceComboBox.ValueMember = "Value"
        listTriggerSourceComboBox.SelectedValue = RfsgDigitalEdgeConfigurationListStepTriggerSource.TimerEvent
    End Sub

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim startFrequency As Double
        Dim endFrequency As Double
        Dim startPowerLevel As Double
        Dim endPowerLevel As Double
        Dim dwellTime As Double
        Dim numberOfSteps As Integer
        Dim frequencyForStep As Double
        Dim powerLevelForStep As Double
        Dim frequencyIncrement As Double = 0
        Dim powerLevelIncrement As Double = 0
        Dim frequencySettlingTime As Double
        Dim numberOfStepsIterator As Integer

        Dim configurationListAttributes As RfsgConfigurationListProperties() = {RfsgConfigurationListProperties.Frequency, RfsgConfigurationListProperties.PowerLevel}
        Dim refClockSource As RfsgFrequencyReferenceSource
        Dim listTriggerSource As RfsgDigitalEdgeConfigurationListStepTriggerSource
        Try
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            startFrequency = CDbl(startFrequencyNumeric.Value)
            endFrequency = CDbl(endFrequencyNumeric.Value)
            startPowerLevel = CDbl(startPowerNumeric.Value)
            endPowerLevel = CDbl(stopPowerNumeric.Value)
            dwellTime = CDbl(dwellTimeNumeric.Value)
            numberOfSteps = CInt(Math.Truncate(numberStepsNumeric.Value))
            frequencySettlingTime = CDbl(frequencySettlingsNumeric.Value)

            refClockSource = If(TryCast(referenceClockSourceComboBox.SelectedValue, RfsgFrequencyReferenceSource), RfsgFrequencyReferenceSource.FromString(referenceClockSourceComboBox.Text))
            listTriggerSource = If(TryCast(listTriggerSourceComboBox.SelectedValue, RfsgDigitalEdgeConfigurationListStepTriggerSource), RfsgDigitalEdgeConfigurationListStepTriggerSource.FromString(listTriggerSourceComboBox.Text))

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, New EventHandler(Of RfsgWarningEventArgs)(AddressOf DriverOperation_Warning)

            ' Configure the reference clock source 
            _rfsgSession.FrequencyReference.Configure(refClockSource, FrequencyReferenceRate)

            ' Configure the Frequency Settling Units to 'Seconds After I/O'. 
            ' RFSG currrently only supports 'Seconds After I/O' when using the Configuration List feature. 
            _rfsgSession.RF.Advanced.FrequencySettlingUnits = RfsgRFFrequencySettlingUnits.TimeAfterIO

            ' Configure the Frequency Settling to 0 seconds to indicate that we don't want to wait for the frequency to settle in each step.
            ' If the step is waiting for the frequency to settle, the device will not advance to the next configuration when a trigger is received. 
            _rfsgSession.RF.Advanced.FrequencySettlingTime = frequencySettlingTime

            ' Configure the trigger type to advance steps in the list 
            _rfsgSession.Triggers.ConfigurationListStepTrigger.TriggerType = RfsgConfigurationListStepTriggerType.DigitalEdge

            ' Configure the trigger source to advance steps in the list 
            _rfsgSession.Triggers.ConfigurationListStepTrigger.DigitalEdge.Source = listTriggerSource

            ' Configure the timer event interval 
            ' Note: Only used when listTriggerSource is TimerEvent
            If listTriggerSource.Equals(RfsgDigitalEdgeConfigurationListStepTriggerSource.TimerEvent) Then
                _rfsgSession.DeviceEvents.Timer.Interval = dwellTime
            End If

            ' Create a Configuration List. Pass Frequency and Power Level in the 
            ' Configuration List Attributes parameter to be able to configure 
            ' Frequency and Power Level in each step that we create. Pass true to the 
            ' Set As Active List parameter, this will set the Active Configuration 
            ' List attribute to the name of the created Configuration List. Once the 
            ' Active Configuration List is set, the Frequency or Power Level will be modified for this configuration list. 
            _rfsgSession.BasicConfigurationList.CreateConfigurationList("frequencyAndPowerLevelList", configurationListAttributes, True)

            frequencyForStep = startFrequency
            powerLevelForStep = startPowerLevel

            If numberOfSteps > 1 Then
                frequencyIncrement = (endFrequency - startFrequency) / (numberOfSteps - 1)
                powerLevelIncrement = (endPowerLevel - startPowerLevel) / (numberOfSteps - 1)
            End If

            ' Build the Configuration List 
            For numberOfStepsIterator = 0 To numberOfSteps - 1
                ' Create a Configuration List Step. Pass true to the Set As Active 
                ' Step parameter, this will set the Active Configuration List Step attribute to the created
                ' Configuration List Step index. Once the 
                ' Active Configuration List Step is set, the Frequency or Power Level will be modified for this
                ' Configuration List Step in the Configuration 
                ' List indicated by the Active Configuration List attribute. 
                _rfsgSession.BasicConfigurationList.CreateStep(True)

                ' Configure the Frequency for this step
                _rfsgSession.RF.Frequency = frequencyForStep

                ' Configure the Power Level for this step
                _rfsgSession.RF.PowerLevel = powerLevelForStep

                frequencyForStep += frequencyIncrement
                powerLevelForStep += powerLevelIncrement
            Next

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Start the status checking timer 
            EnableControls(False)
        Catch ex As Exception
            ShowError("StartGeneration()", ex)
        End Try
    End Sub

    Private Sub DriverOperation_Warning(ByVal sender As Object, ByVal e As RfsgWarningEventArgs)
        ' Display the rfsg warning
        errorTextBox.Text = e.Message
    End Sub

    Private Sub CheckGeneration()
        Try
            ' Check the status of the RFSG 
            _rfsgSession.CheckGenerationStatus()
        Catch ex As Exception
            ShowError("CheckGeneration()", ex)
        End Try
    End Sub

    Private Sub StopGeneration()
        ' Stop the status checking timer 
        EnableControls(True)

        Try
            If _rfsgSession IsNot Nothing Then
                ' Abort the generation to delete the Configuration List 
                _rfsgSession.Abort()

                ' Delete the Configuration List 
                _rfsgSession.BasicConfigurationList.DeleteConfigurationList("frequencyAndPowerLevelList")

                ' Set the Active Configuration List to string.Empty 
                _rfsgSession.BasicConfigurationList.ActiveList = String.Empty

                ' Disable the output 
                _rfsgSession.RF.OutputEnabled = False

                ' When a Configuration List is stopped, NI-RFSG transitions to the Configuration State.  
                '  The new value of the Output Enabled attribute is committed to apply the change to the hardware.
                _rfsgSession.Utility.Commit()

                ' Unsubscribe from warning events
                RemoveHandler _rfsgSession.DriverOperation.Warning, AddressOf DriverOperation_Warning

                ' Close the RFSG session
                _rfsgSession.Close()
            End If
            _rfsgSession = Nothing
        Catch ex As Exception
            errorTextBox.Text = "Error in StopGeneration(): " & ex.Message
        End Try
    End Sub

    Private Sub ShowError(ByVal functionName As String, ByVal exception As Exception)
        StopGeneration()
        errorTextBox.Text = "Error in " & functionName & ": " & exception.Message
    End Sub

    Private Sub EnableControls(ByVal enabled As Boolean)
        rfsgStatusTimer.Enabled = Not enabled
        startButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        configurationListTriggerGroupBox.Enabled = enabled
        resourceNameComboBox.Enabled = enabled
        configurationListParametersGroupBox.Enabled = enabled
        referenceClockSourceComboBox.Enabled = enabled

        Application.DoEvents()
    End Sub
#End Region

#Region "Form Events"
    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        StartGeneration()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        StopGeneration()
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        StopGeneration()
    End Sub

    Private Sub rfsgStatusTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles rfsgStatusTimer.Tick
        CheckGeneration()
    End Sub
#End Region

End Class
