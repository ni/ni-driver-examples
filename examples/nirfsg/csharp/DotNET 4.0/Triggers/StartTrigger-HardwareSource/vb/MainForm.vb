'==================================================================================================
' Title        : Start Trigger-Hardware Source
' Description  : This program demonstrates the use of niRFSG to generate a simple sine wave 
'			     at a specified frequency and output gain when a hardware trigger is received. 
'
'			     Note: In order to run this example, the upconverter must be configured with 
'			     an Arbitrary Waveform Generator. 
'			     To do this, open Measurement & Automation Explorer, select the upconverter 
'			     and click on properties.
'==================================================================================================

Imports System
Imports System.Windows.Forms
Imports System.Collections
Imports System.Collections.Generic
Imports NationalInstruments.ModularInstruments.NIRfsg
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private _rfsgSession As NIRfsg

    Public Sub New()
        InitializeComponent()

        LoadRfsgDeviceNames()

        ConfigureTriggerTypeComboBox()
        ConfigureTriggerSourceComboBox()
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

    Private Sub ConfigureTriggerTypeComboBox()
        triggerTypeComboBox.Items.Add(RfsgStartTriggerType.None)
        triggerTypeComboBox.Items.Add(RfsgStartTriggerType.DigitalEdge)
        triggerTypeComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureTriggerSourceComboBox()
        Dim triggerSourceValueList = New List(Of DictionaryEntry)()
        triggerSourceValueList.Add(New DictionaryEntry("PFI0", RfsgDigitalEdgeStartTriggerSource.Pfi0))
        triggerSourceValueList.Add(New DictionaryEntry("PFI1", RfsgDigitalEdgeStartTriggerSource.Pfi1))
        triggerSourceValueList.Add(New DictionaryEntry("PFI2", RfsgDigitalEdgeStartTriggerSource.Pfi2))
        triggerSourceValueList.Add(New DictionaryEntry("PFI3", RfsgDigitalEdgeStartTriggerSource.Pfi3))
        triggerSourceValueList.Add(New DictionaryEntry("PXI_Trig0", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine0))
        triggerSourceValueList.Add(New DictionaryEntry("PXI_Trig1", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine1))
        triggerSourceValueList.Add(New DictionaryEntry("PXI_Trig2", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine2))
        triggerSourceValueList.Add(New DictionaryEntry("PXI_Trig3", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine3))
        triggerSourceValueList.Add(New DictionaryEntry("PXI_Trig4", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine4))
        triggerSourceValueList.Add(New DictionaryEntry("PXI_Trig5", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine5))
        triggerSourceValueList.Add(New DictionaryEntry("PXI_Trig6", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine6))
        triggerSourceValueList.Add(New DictionaryEntry("PXI_Trig7", RfsgDigitalEdgeStartTriggerSource.PxiTriggerLine7))
        triggerSourceValueList.Add(New DictionaryEntry("PXI_STAR", RfsgDigitalEdgeStartTriggerSource.PxiStarLine))

        triggerSourceComboBox.DataSource = triggerSourceValueList
        triggerSourceComboBox.DisplayMember = "Key"
        triggerSourceComboBox.ValueMember = "Value"
        triggerSourceComboBox.SelectedValue = RfsgDigitalEdgeStartTriggerSource.Pfi0
    End Sub

#End Region

#Region "Program Functions"

    Private Sub StartGeneration()
        Dim resourceName As String
        Dim frequency As Double, power As Double
        Dim triggerType As RfsgStartTriggerType
        Dim triggerSource As RfsgDigitalEdgeStartTriggerSource
        Try
            ' Read in all of the control values 
            resourceName = resourceNameComboBox.Text
            frequency = CDbl(frequencyNumeric.Value)
            power = CDbl(powerLevelNumeric.Value)
            triggerType = DirectCast(triggerTypeComboBox.SelectedItem, RfsgStartTriggerType)
            triggerSource = If(TryCast(triggerSourceComboBox.SelectedValue, RfsgDigitalEdgeStartTriggerSource), RfsgDigitalEdgeStartTriggerSource.FromString(triggerSourceComboBox.Text))

            errorTextBox.Text = "No error."
            Application.DoEvents()

            ' Initialize the NIRfsg session
            _rfsgSession = New NIRfsg(resourceName, True, False)

            ' Subscribe to Rfsg warnings
            AddHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

            ' Configure the instrument 
            _rfsgSession.RF.Configure(frequency, power)

            If triggerType = RfsgStartTriggerType.DigitalEdge Then
                _rfsgSession.Triggers.StartTrigger.DigitalEdge.Configure(triggerSource, RfsgTriggerEdge.RisingEdge)
            Else
                _rfsgSession.Triggers.StartTrigger.Disable()
            End If

            ' Initiate Generation 
            _rfsgSession.Initiate()

            ' Start the status checking timer 
            EnableControls(False)

            ' Activate stop button 
            stopButton.Focus()
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
                ' Disable the output.  This sets the noise floor as low as possible.
                _rfsgSession.RF.OutputEnabled = False

                ' Unsubscribe from warning events
                RemoveHandler _rfsgSession.DriverOperation.Warning, AddressOf Me.DriverOperation_Warning

                ' Close the RFSG NIRfsg session
                _rfsgSession.Close()
            End If
            _rfsgSession = Nothing
        Catch ex As Exception
            errorTextBox.Text = "Error in StopGeneration(): " + ex.Message
        End Try
    End Sub

    Private Sub ShowError(ByVal functionName As String, ByVal exception As Exception)
        StopGeneration()
        errorTextBox.Text = "Error in " + functionName + ": " + exception.Message
    End Sub

#End Region

#Region "Form Events"
    Private Sub startButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startButton.Click
        StartGeneration()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles stopButton.Click
        StopGeneration()
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        StopGeneration()
    End Sub

    Private Sub rfsgStatusTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles rfsgStatusTimer.Tick
        CheckGeneration()
    End Sub
#End Region

    Private Sub EnableControls(ByVal enabled As Boolean)
        startButton.Enabled = enabled
        stopButton.Enabled = Not enabled
        resourceNameComboBox.Enabled = enabled
        frequencyNumeric.Enabled = enabled
        powerLevelNumeric.Enabled = enabled
        triggerTypeComboBox.Enabled = enabled
        triggerSourceComboBox.Enabled = enabled
        rfsgStatusTimer.Enabled = Not enabled

        Application.DoEvents()
    End Sub
End Class
