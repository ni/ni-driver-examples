'******************************************************************************
'*
'* Example program:
'*   Rfsa Self Calibration
'*
'* Category:
'*   NI-RFSA
'*
'* Description:
'*   Use this example to learn how to do self calibration for the NI-RFSA.
'*
'* Instructions for running:
'*   1. Configure both RFSA device in the MAX for the program to run. 
'*
'*	 2. Configure the Clock Source in the UI.
'*   
'*   3. Configure the Self Calibration Step Operation.
'*
'*   4. Select the Start Button in the UI to start the self-calibration.
'*   
'*   5. The data is displayed in the DataGrid.
'*
'* I/O Connections Overview:
'*   Make sure your signal input terminals match the Physical Channel I/O
'*   Controls.  If you have a PXI chassis, ensure that it has been properly
'*   identified in MAX.  
'*
'******************************************************************************
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIRfsa
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices

Partial Public Class MainForm
    Inherits Form
    Private rfsaSession As NIRfsa
    Public Sub New()
        InitializeComponent()
        LoadRfsaDeviceNames()
        ConfigureOutputTerminalComboBox()
        ConfigureSelfCalibrationStepOperation()
        indicatorLabel.BackColor = Color.Gray
    End Sub

#Region "Initial Configuration"

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
        Dim clockSourceValueList As List(Of KeyValuePair(Of String, RfsaReferenceClockSource)) = New List(Of KeyValuePair(Of String, RfsaReferenceClockSource))()
        clockSourceValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("OnboardClock", RfsaReferenceClockSource.OnboardClock))
        clockSourceValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("RefIn", RfsaReferenceClockSource.ReferenceIn))
        clockSourceValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("PXI_Clk", RfsaReferenceClockSource.PxiClock))
        clockSourceValueList.Add(New KeyValuePair(Of String, RfsaReferenceClockSource)("ClkIn", RfsaReferenceClockSource.ClockIn))
        clockSourceComboBox.DisplayMember = "Key"
        clockSourceComboBox.ValueMember = "Value"
        clockSourceComboBox.DataSource = clockSourceValueList
        clockSourceComboBox.SelectedIndex = 0
    End Sub

    Private Sub ConfigureSelfCalibrationStepOperation()
        Dim selfCalibrationOperationValueList As List(Of String) = New List(Of String)()
        selfCalibrationOperationValueList.Add("Perform All Self Calibration Step")
        selfCalibrationOperationValueList.Add("Perform Neccessary Self Calibration")
        selfCalibrationOperationValueList.Add("Omit IF Flatness Self Calibration")
        selfCalibrationComboBox.DataSource = selfCalibrationOperationValueList
    End Sub

#End Region

#Region "UI Gets"

    Private ReadOnly Property ResourceName() As String
        Get
            Return Me.resourceNameComboBox.Text
        End Get
    End Property

    Public ReadOnly Property ReferenceClockSource() As RfsaReferenceClockSource
        Get
            Return If(TryCast(Me.clockSourceComboBox.SelectedValue, RfsaReferenceClockSource), RfsaReferenceClockSource.FromString(Me.clockSourceComboBox.Text))
        End Get
    End Property

    Private ReadOnly Property SelfCalibrationOperation() As Integer
        Get
            Return Me.selfCalibrationComboBox.SelectedIndex
        End Get
    End Property

#End Region
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

    Private Sub ConfigureReferenceClock()
        rfsaSession.Configuration.ReferenceClock.Source = ReferenceClockSource
    End Sub

    Private Sub ConfigureSelfCalibration()
        Dim validSteps As RfsaSelfCalibrationSteps
        Select Case SelfCalibrationOperation
            Case 0
                'Passing an empty array will ensure NI-RFSA perform all Self Calibration steps.
                rfsaSession.Calibration.Self.SelfCalibrate(RfsaSelfCalibrationSteps.None)
                Exit Select
            Case 1
                ' Queries NI-RFSA for the valid calibration steps.
                ' Passing this value to the niRFSA Self Cal VI ensures valid calibration steps are not repeated,
                ' thus only calibrating necessary steps
                rfsaSession.Calibration.Self.IsSelfCalibrationValid(validSteps)
                rfsaSession.Calibration.Self.SelfCalibrate(validSteps)
                Exit Select
            Case 2
                ' IF Flatness Self Calibration can take up to 15 minutes.
                rfsaSession.Calibration.Self.SelfCalibrate(RfsaSelfCalibrationSteps.IFFlatness)
                Exit Select
        End Select
    End Sub

    Private Sub ChangeControlState(ByVal state As Boolean)
        Me.resourceNameComboBox.Enabled = state
        Me.clockSourceComboBox.Enabled = state
        Me.selfCalibrationComboBox.Enabled = state
        Me.startButton.Enabled = state
    End Sub

    Private Sub startButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startButton.Click
        indicatorLabel.BackColor = Color.Gray
        ChangeControlState(False)
        Try
            ' Steps:
            '1. Open a new NI-RFSA session.
            '2. Configure the Reference clock.
            '3. Determine which self-calibration steps to omit.
            '4. Initiate self-calibration.
            '5. Close the NI-RFSA session.
            InitializeRfsaSession()
            ConfigureReferenceClock()
            ConfigureSelfCalibration()
            CloseSession()
            indicatorLabel.BackColor = Color.Green
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



    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")
    End Sub
End Class
