'==================================================================================================
' Title        : Thermocouple Measurements
' Description  :This example helps us learn how to use Switch and Dmm to make thermocouple
'measurements.The example first configures scan operation for switch, then dmm measurement configuration
'performed. The dmm fetches data once the switch scan operation is completed, for each fetch operation
'done by the Dmm the thermocouple conversion is done and diplayed in the UI.                  
'==================================================================================================
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Threading
Imports System.Windows.Forms
Imports NationalInstruments.ModularInstruments.NIDmm
Imports NationalInstruments.ModularInstruments.NISwitch
Imports NationalInstruments.ModularInstruments.SystemServices.DeviceServices


Partial Public Class MainForm
    Inherits Form
    Private dmmSession As NIDmm
    Private switchSession As NISwitch
    Private dmmSampleCount As Integer = 1
    Private dmmSamplesArray As Double()
    Private dmmInstrumentModel As String
    Private workerThread As Thread
    Private threadStop As Boolean
    Private closeRequested As Boolean
    Private lockobj As New Object()

    Private Property ThreadStopMarker() As Boolean
        Get
            SyncLock lockobj
                Return threadStop
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            SyncLock lockobj
                threadStop = value
            End SyncLock
        End Set
    End Property

    Public Sub New()
        InitializeComponent()

        LoadSwitchDeviceNames()
        LoadTopology()
        LoadDmmDeviceNames()
        LoadMeasurementModes()
        LoadTriggerInput()
        LoadScanAdvancedOutput()
        LoadMeasCompleteDest()
        LoadTriggerSource()

        LoadThermocoupleType()
    End Sub
#Region "UI Initial Value Config Section"
    Private Sub LoadTopology()
        Dim myType As Type = GetType(SwitchDeviceTopology)
        Dim properties As PropertyInfo() = myType.GetProperties()

        For Each prop As PropertyInfo In properties
            topologyNameComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
        Next
        topologyNameComboBox.SelectedIndex = 0
    End Sub
    Private Sub LoadSwitchDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-SWITCH")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            switchResourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            switchResourceNameComboBox.SelectedIndex = 0
        End If
    End Sub
    Private Sub LoadTriggerInput()
        Dim myType As Type = GetType(SwitchScanTriggerInput)
        Dim properties As PropertyInfo() = myType.GetProperties()

        For Each prop As PropertyInfo In properties
            triggerInputComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
        Next
        triggerInputComboBox.SelectedIndex = 3
    End Sub
    Private Sub LoadScanAdvancedOutput()
        Dim myType As Type = GetType(SwitchScanAdvancedOutput)
        Dim properties As PropertyInfo() = myType.GetProperties()

        For Each prop As PropertyInfo In properties
            scanAdvancedOutputComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
        Next
        scanAdvancedOutputComboBox.SelectedIndex = 3
    End Sub
    Private Sub LoadDmmDeviceNames()
        Dim modularInstrumentsSystem As New ModularInstrumentsSystem("NI-DMM")
        For Each device As DeviceInfo In modularInstrumentsSystem.DeviceCollection
            dmmResourceNameComboBox.Items.Add(device.Name)
        Next
        If modularInstrumentsSystem.DeviceCollection.Count > 0 Then
            dmmResourceNameComboBox.SelectedIndex = 0
        End If
    End Sub
    Private Sub LoadMeasurementModes()
        measurementModeComboBox.Items.AddRange([Enum].GetNames(GetType(DmmMeasurementFunction)))
        measurementModeComboBox.Items.Remove(DmmMeasurementFunction.WaveformCurrent.ToString())
        measurementModeComboBox.Items.Remove(DmmMeasurementFunction.WaveformVoltage.ToString())
        measurementModeComboBox.SelectedIndex = 0
    End Sub
    Private Sub LoadMeasCompleteDest()
        Dim myType As Type = GetType(DmmMeasurementCompleteDestination)
        Dim properties As PropertyInfo() = myType.GetProperties()

        For Each prop As PropertyInfo In properties
            measCompleteDestComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
        Next
        measCompleteDestComboBox.SelectedIndex = 2
    End Sub
    Private Sub LoadTriggerSource()
        Dim myType As Type = GetType(DmmTriggerSource)
        Dim properties As PropertyInfo() = myType.GetProperties()

        For Each prop As PropertyInfo In properties
            triggerSourceComboBox.Items.Add(prop.GetValue(myType, Nothing).ToString())
        Next
        triggerSourceComboBox.SelectedIndex = 4
    End Sub
    Private Sub LoadThermocoupleType()
        Dim thermocoupleValue As New List(Of KeyValuePair(Of Char, UInt16))()
        thermocoupleValue.Add(New KeyValuePair(Of Char, UInt16)("B"c, 0))
        thermocoupleValue.Add(New KeyValuePair(Of Char, UInt16)("E"c, 1))
        thermocoupleValue.Add(New KeyValuePair(Of Char, UInt16)("J"c, 2))
        thermocoupleValue.Add(New KeyValuePair(Of Char, UInt16)("K"c, 3))
        thermocoupleValue.Add(New KeyValuePair(Of Char, UInt16)("R"c, 4))
        thermocoupleValue.Add(New KeyValuePair(Of Char, UInt16)("S"c, 5))
        thermocoupleValue.Add(New KeyValuePair(Of Char, UInt16)("T"c, 6))
        thermocoupleValue.Add(New KeyValuePair(Of Char, UInt16)("N"c, 7))

        thermocoupleTypeComboBox.DisplayMember = "Key"
        thermocoupleTypeComboBox.ValueMember = "Value"
        thermocoupleTypeComboBox.DataSource = thermocoupleValue
        thermocoupleTypeComboBox.SelectedIndex = 2
    End Sub

#End Region

#Region "Program Properties"
    Private ReadOnly Property SwitchResourceName() As String
        Get
            Return Me.switchResourceNameComboBox.Text
        End Get
    End Property
    Private ReadOnly Property DmmResourceName() As String
        Get
            Return Me.dmmResourceNameComboBox.Text
        End Get
    End Property
    Private ReadOnly Property TopologyName() As String
        Get
            Return Me.topologyNameComboBox.SelectedItem.ToString()
        End Get
    End Property
    Private ReadOnly Property ScanAdvancedOutput() As String
        Get
            Return Me.scanAdvancedOutputComboBox.Text
        End Get
    End Property
    Private ReadOnly Property TriggerInput() As String
        Get
            Return Me.triggerInputComboBox.Text
        End Get
    End Property
    Private ReadOnly Property NumberOfChannels() As Integer
        Get
            Return CInt(Me.numOfChannelsNumericUpDown.Value)
        End Get
    End Property
    Private ReadOnly Property MeasurementCompleteDestination() As String
        Get
            Return Me.measCompleteDestComboBox.Text
        End Get
    End Property
    Private ReadOnly Property TriggerSource() As String
        Get
            Return Me.triggerSourceComboBox.Text
        End Get
    End Property
    Private ReadOnly Property MeasurementMode() As String
        Get
            Return Me.measurementModeComboBox.Text
        End Get
    End Property
    Private ReadOnly Property ScanList() As String
        Get
            Return Me.scanListTextBox.Text
        End Get
    End Property
    Private ReadOnly Property ThermocoupleType() As UInt16
        Get
            Return CType(Me.thermocoupleTypeComboBox.SelectedValue, UInt16)
        End Get
    End Property

#End Region
#Region "ProgramFunctions"
#Region "SwitchDMMConfig"
    Private Shared Sub ShowError(ByVal message As String)
        If String.IsNullOrEmpty(message) Then
            message = "Unexpected Error"
        End If
        MessageBox.Show(message, "Error")


    End Sub
    Private Sub ChangeControlState(ByVal isEnabled As Boolean)
        Me.startButton.Enabled = isEnabled
        Me.dmmResourceNameComboBox.Enabled = isEnabled
        Me.switchResourceNameComboBox.Enabled = isEnabled
        Me.topologyNameComboBox.Enabled = isEnabled
        Me.measurementModeComboBox.Enabled = isEnabled
        Me.scanListTextBox.Enabled = isEnabled
        Me.triggerInputComboBox.Enabled = isEnabled
        Me.scanAdvancedOutputComboBox.Enabled = isEnabled
        Me.numOfChannelsNumericUpDown.Enabled = isEnabled
        Me.thermocoupleTypeComboBox.Enabled = isEnabled
        Me.triggerSourceComboBox.Enabled = isEnabled
        Me.measCompleteDestComboBox.Enabled = isEnabled
    End Sub
    Private Sub CloseSwitchSession()
        If switchSession IsNot Nothing Then
            Try
                switchSession.Close()
                switchSession = Nothing
            Catch ex As System.Exception
                ShowError("Unable to Close Session, Reset the device." & vbLf & "Error : " & ex.Message)
                Application.[Exit]()
            End Try
        End If
    End Sub
    Private Sub CloseDmmSession()
        If dmmSession IsNot Nothing Then
            Try
                dmmSession.Close()
                dmmSession = Nothing
            Catch ex As System.Exception
                ShowError("Unable to Close Session, Reset the device." & vbLf & "Error : " & ex.Message)
                Application.[Exit]()
            End Try
        End If


    End Sub
    Private Sub InitializeSwitchSession()
        'Open a session to the switch module and set the topology.
        switchSession = New NISwitch(SwitchResourceName, TopologyName, False, True)
        AddHandler switchSession.DriverOperation.Warning, New System.EventHandler(Of SwitchWarningEventArgs)(AddressOf DriverOperationWarning)
    End Sub
    Private Sub DriverOperationWarning(ByVal sender As Object, ByVal e As SwitchWarningEventArgs)
        MessageBox.Show(e.ToString(), "Warning")
    End Sub


    Private Sub InitializeDmmSession()

        ' Create a Dmm Session
        dmmSession = New NIDmm(DmmResourceName, True, True)

    End Sub
    Private Sub ConfigureDmmMeasurement()
        ConfigureMeasurement()
        ConfigureDmmTrigger()
        ConfigureDmmMultiPoint()
        ConfigureSampleTriggerSlope()
        ConfigureDmmMeasurementComplete()
    End Sub
    Private Sub InitiateDmmMeasurement()
        'Initiate the DMM measurement.
        dmmSession.Measurement.Initiate()
    End Sub

    Private Sub ConfigureDmmMeasurementComplete()
        'Configures the destination of the DMM output trigger (Measurement Complete). 
        'This should match the switch module input trigger.
        dmmSession.Trigger.MeasurementCompleteDestination = MeasurementCompleteDestination

        'Configures the slope of the DMM output trigger.
        dmmSession.Trigger.MeasurementCompleteDestinationSlope = DmmSlope.Negative
    End Sub

    Private Sub ConfigureSampleTriggerSlope()
        'Configures the slope of the secondary DMM input trigger (Sample Trigger).
        dmmSession.Trigger.MultiPoint.SampleTriggerSlope = DmmSlope.Negative
    End Sub

    Private Sub ConfigureDmmMultiPoint()
        'Configure a multipoint acquisition.

        dmmSession.Trigger.MultiPoint.Configure(1, dmmSampleCount, TriggerSource, PrecisionTimeSpan.MaxValue)
        dmmSession.Trigger.MultiPoint.SampleCount = 0
    End Sub

    Private Sub ConfigureDmmTrigger()
        ' Configure Trigger Source
        dmmSession.Trigger.Source = TriggerSource

        dmmSession.Trigger.Slope = DmmSlope.Negative
       
    End Sub

    Private Sub ConfigureMeasurement()
        'Configure the function, range, and resolution of the measurement.
        dmmSession.Configure(DirectCast([Enum].Parse(GetType(DmmMeasurementFunction), MeasurementMode), DmmMeasurementFunction), 1.0, 0.000001)

    End Sub
    Private Sub AbortSwitchScan()
        '''/Halts the scan.
        switchSession.Scan.Abort()

    End Sub

    Private Sub InitiateSwitchScan()
        'Initiates the scan usings the configured scan list and triggers.
        switchSession.Scan.Initiate()
    End Sub
    Private Function GetDmmInstrumentModel() As String
        'Queries an attribute value to determine the instrument module of the DMM.
        Return (dmmSession.DriverIdentity.InstrumentModel)
    End Function
    Private Sub ConfigureSwitchScan()
        Dim ScanListInput As String = "cjtemp->com0;" & ScanList
        'Configures the input trigger of the switch module. This should match the
        'output trigger of the DMM.
        switchSession.Scan.ConfigureTrigger(New PrecisionTimeSpan(0.0), TriggerInput, ScanAdvancedOutput)

        'Configures the switch to loop continuously through the scan list until
        'niSwitch_Abort is called.
        switchSession.Scan.Continuous = True

        'Configures the switch module for scanning. 
        switchSession.Scan.ConfigureList(ScanListInput, SwitchScanMode.BreakBeforeMake)

    End Sub
    Private Sub CommitScan()
        switchSession.Scan.Commit()
    End Sub
#End Region
    Private Sub ThreadTerminate()

        'Waits for the thread To Terminate.
        workerThread.Join()
        workerThread = Nothing
        If closeRequested = True Then
            Me.Close()
        End If
    End Sub
    Private Sub ToggleButtonState(ByVal isEnabled As Boolean)
        Me.startButton.Enabled = isEnabled
        Me.stopButton.Enabled = Not isEnabled
    End Sub
    Private Sub StartMeasurement()
        ' Starts a Thread for Performing Scan, when NextButton is clicked, so that Main UI thread doesn't hangs.
        workerThread = New Thread(AddressOf StartDmmMeasurement)
        workerThread.Start()
    End Sub

    Private Sub StartDmmMeasurement()
        'Send software trigger to switch module when Next Connection button pressed on front panel.
        Dim j As Integer = 0
        Try

            While ThreadStopMarker = False
                'Download data from the DMM.
                dmmSamplesArray = dmmSession.Measurement.FetchMultiPoint(PrecisionTimeSpan.MaxValue, NumberOfChannels + 1)
                Dim temp As Double() = New Double(NumberOfChannels) {}
                Invoke(New Action(Function() InlineAssignHelper(temp, ConvertThermocoupleReading(dmmSamplesArray))))
                Dim methodCall As Action = Sub() UpdateData(temp, j)
                dataGridViewResults.Invoke(methodCall)
                j += 1
            End While
            Me.BeginInvoke(New Action(AddressOf ThreadTerminate))

        Catch ex As Exception
            ShowError(ex.Message)
            ThreadStopMarker = True
            Me.BeginInvoke(New Action(AddressOf ChangingControlState))
            Me.BeginInvoke(New Action(AddressOf ThreadTerminate))
        End Try
    End Sub

    Private Sub UpdateData(ByVal temp As Double(), ByVal j As Integer)
        Me.dataGridViewResults.Rows.Add()
        For i As Integer = 0 To temp.Length - 1
            Me.dataGridViewResults.Rows(j).Cells(i).Value = temp(i)
        Next

    End Sub
#Region "ThermoCalculations"
    Private Function ConvertThermocoupleReading(ByVal dmmSamplesArray As Double()) As Double()
        Dim temperature As Double() = New Double(NumberOfChannels - 1) {}
        Dim thermocoupleVoltages As Double() = New Double(NumberOfChannels - 1) {}
        Array.Copy(dmmSamplesArray, 1, thermocoupleVoltages, 0, NumberOfChannels)
        Dim thermistorReading As Double = ConvertThermistorReading(dmmSamplesArray(0))
        Dim thermocoupleVoltage As Double = ConvertTempToVolts(thermistorReading)

        For i As Integer = 0 To thermocoupleVoltages.Length - 1
            temperature(i) = ConvertVoltToTemp((thermocoupleVoltages(i) + thermocoupleVoltage))
        Next

        Return temperature
    End Function

    Private Function ConvertVoltToTemp(ByVal V As Double) As Double
        Dim temp As Double = 0


        Select Case ThermocoupleType

            Case 0
                temp = CalculateBThermocoupleVoltsToTemp(V * 1000000.0)
                Exit Select
            Case 1
                temp = CalculateEThermocoupleVoltsToTemp(V * 1000000.0)
                Exit Select
            Case 2
                temp = CalculateJThermocoupleVoltsToTemp(V * 1000000.0)
                Exit Select
            Case 3
                temp = CalculateKThermocoupleTempToVolts(V * 1000000.0)
                Exit Select
            Case 4
                temp = CalculateRThermocoupleTempToVolts(V * 1000000.0)
                Exit Select
            Case 5
                temp = CalculateSThermocoupleTempToVolts(V * 1000000.0)
                Exit Select
            Case 6
                temp = CalculateTThermocoupleTempToVolts(V * 1000000.0)
                Exit Select
            Case 7
                temp = CalculateNThermocoupleTempToVolts(V * 1000000.0)
                Exit Select
        End Select

        Return (temp)
    End Function
    Private Shared Function CalculateNThermocoupleVoltsToTemp(ByVal V__1 As Double) As Double
        Dim T As Double
        Dim v__2 As Double = V__1
        Dim condition As Integer = (If((V__1 >= 20613.0), 1, 0)) + (If((V__1 >= 0.0), 1, 0))
        If condition = 0 Then
            T = v__2 * ((0.038436847) + v__2 * ((0.0000011010485) + v__2 * ((0.0000000052229312) + v__2 * ((0.0000000000072060525) + v__2 * ((0.0000000000000058488586) + v__2 * ((2.7754916E-18) + v__2 * ((7.7075166E-22) + v__2 * ((1.1582665E-25) + v__2 * (7.3138868E-30)))))))))
        ElseIf condition = 1 Then
            T = v__2 * ((0.0386896) + v__2 * ((-0.00000108267) + v__2 * ((0.0000000000470205) + v__2 * ((-2.12169E-18) + v__2 * ((-1.17272E-19) + v__2 * ((5.3928E-24) + v__2 * (-7.98156E-29)))))))
        Else
            T = (19.72485) + v__2 * ((0.03300943) + v__2 * ((-0.0000003915159) + v__2 * ((0.000000000009855391) + v__2 * ((-0.0000000000000001274371) + v__2 * (7.767022E-22)))))
        End If
        Return T
    End Function
    Private Shared Function CalculateTThermocoupleVoltsToTemp(ByVal V__1 As Double) As Double
        Dim T As Double
        Dim v__2 As Double = V__1
        If V__1 >= 0.0 Then
            T = v__2 * ((0.025928) + v__2 * ((-0.0000007602961) + v__2 * ((0.00000000004637791) + v__2 * ((-0.000000000000002165394) + v__2 * ((6.048144E-20) + v__2 * (-7.293422E-25))))))
        Else
            T = v__2 * ((0.025949192) + v__2 * ((-0.00000021316967) + v__2 * ((0.00000000079018692) + v__2 * ((0.00000000000042527777) + v__2 * ((0.00000000000000013304473) + v__2 * ((2.0241446E-20) + v__2 * (1.2668171E-24)))))))
        End If
        Return T
    End Function
    Private Shared Function CalculateSThermocoupleVoltsToTemp(ByVal V__1 As Double) As Double
        Dim condition As Integer = (If((V__1 >= 17536.0), 1, 0)) + (If((V__1 >= 10332.0), 1, 0)) + (If((V__1 >= 1874.0), 1, 0))
        Dim T As Double
        Dim v__2 As Double = V__1
        If condition = 0 Then
            T = v__2 * ((0.18494946) + v__2 * ((-0.0000800504062) + v__2 * ((0.00000010223743) + v__2 * ((-0.000000000152248592) + v__2 * ((0.000000000000188821343) + v__2 * ((-0.000000000000000159085941) + v__2 * ((8.2302788E-20) + v__2 * ((-2.34181944E-23) + v__2 * (2.7978626E-27)))))))))
        ElseIf condition = 1 Then
            T = (12.91507177) + v__2 * ((0.1466298863) + v__2 * ((-0.00001534713402) + v__2 * ((0.000000003145945973) + v__2 * ((-0.0000000000004163257839) + v__2 * ((3.187963771E-17) + v__2 * ((-1.2916375E-21) + v__2 * ((2.183475087E-26) + v__2 * ((-1.447379511E-31) + v__2 * (8.211272125E-36)))))))))
        ElseIf condition = 2 Then
            T = (-80.87801117) + v__2 * ((0.1621573104) + v__2 * ((-0.000008536869453) + v__2 * ((0.0000000004719686976) + v__2 * ((-0.00000000000001441693666) + v__2 * (2.08161889E-19)))))
        Else
            T = (53338.75126) + v__2 * ((-12.35892298) + v__2 * ((0.001092657613) + v__2 * ((-0.00000004265693686) + v__2 * (0.000000000000624720542))))
        End If
        Return T
    End Function
    Private Shared Function CalculateRThermocoupleVoltsToTemp(ByVal V__1 As Double) As Double
        Dim condition As Integer = (If((V__1 >= 19739.0), 1, 0)) + (If((V__1 >= 11361.0), 1, 0)) + (If((V__1 >= 1923.0), 1, 0))
        Dim T As Double
        Dim v__2 As Double = V__1
        If condition = 0 Then
            T = v__2 * ((0.1889138) + v__2 * ((-0.00009383529) + v__2 * ((0.00000013068619) + v__2 * ((-0.0000000002270358) + v__2 * ((0.00000000000035145659) + v__2 * ((-0.000000000000000389539) + v__2 * ((2.8239471E-19) + v__2 * ((-1.2607281E-22) + v__2 * ((3.1353611E-26) + v__2 * (-3.3187769E-30))))))))))
        ElseIf condition = 1 Then
            T = (13.34584505) + v__2 * ((0.1472644573) + v__2 * ((-0.00001844024844) + v__2 * ((0.000000004031129726) + v__2 * ((-0.000000000000624942836) + v__2 * ((6.468412046E-17) + v__2 * ((-4.458750426E-21) + v__2 * ((1.994710149E-25) + v__2 * ((-5.31340179E-30) + v__2 * (6.481976217E-35)))))))))
        ElseIf condition = 2 Then
            T = (-81.99599416) + v__2 * ((0.1553962042) + v__2 * ((-0.000008342197663) + v__2 * ((0.0000000004279433549) + v__2 * ((-0.0000000000000119157791) + v__2 * (1.492290091E-19)))))
        Else
            T = (34061.77836) + v__2 * ((-7.023729171) + v__2 * ((0.0005582903813) + v__2 * ((-0.00000001952394635) + v__2 * (0.0000000000002560740231))))
        End If
        Return T
    End Function

    Private Shared Function CalculateKThermocoupleVoltsToTemp(ByVal V__1 As Double) As Double
        Dim T As Double
        Dim v__2 As Double = V__1
        Dim condition As Integer = (If(V__1 >= 20644.0, 1, 0)) + (If(V__1 >= 0.0, 1, 0))
        If condition = 0 Then
            T = v__2 * ((0.025173462) + v__2 * ((-0.0000011662878) + v__2 * ((-0.0000000010833638) + v__2 * ((-0.0000000000008977354) + v__2 * ((-0.00000000000000037342377) + v__2 * ((-8.6632643E-20) + v__2 * ((-1.0450598E-23) + v__2 * (-5.1920577E-28))))))))
        ElseIf condition = 1 Then
            T = v__2 * ((0.02508355) + v__2 * ((0.00000007860106) + v__2 * ((-0.0000000002503131) + v__2 * ((0.0000000000000831527) + v__2 * ((-1.228034E-17) + v__2 * ((9.804036E-22) + v__2 * ((-4.41303E-26) + v__2 * ((1.057734E-30) + v__2 * (-1.052755E-35)))))))))
        Else

            T = (-131.8058) + v__2 * ((0.04830222) + v__2 * ((-0.000001646031) + v__2 * ((0.00000000005464731) + v__2 * ((-0.0000000000000009650715) + v__2 * ((8.802193E-21) + v__2 * (-3.11081E-26))))))
        End If
        Return T
    End Function
    Private Shared Function CalculateJThermocoupleVoltsToTemp(ByVal V__1 As Double) As Double
        Dim T As Double
        Dim v__2 As Double = V__1
        Dim condition As Integer = (If(V__1 >= 42919.0, 1, 0)) + (If(V__1 >= 0.0, 1, 0))
        If condition = 0 Then
            T = v__2 * ((0.019528268) + v__2 * ((-0.0000012286185) + v__2 * ((-0.0000000010752178) + v__2 * ((-0.00000000000059086933) + v__2 * ((-0.00000000000000017256713) + v__2 * ((-2.8131513E-20) + v__2 * ((-2.396337E-24) + v__2 * (-8.3823321E-29))))))))
        ElseIf condition = 1 Then
            T = v__2 * ((0.01978425) + v__2 * ((-0.0000002001204) + v__2 * ((0.00000000001036969) + v__2 * ((-0.0000000000000002549687) + v__2 * ((3.585153E-21) + v__2 * ((-5.344285E-26) + v__2 * (5.09989E-31)))))))
        Else
            T = (-3113.58187) + v__2 * ((0.300543684) + v__2 * ((-0.0000099477323) + v__2 * ((0.00000000017027663) + v__2 * ((-0.00000000000000143033468) + v__2 * (4.73886084E-21)))))
        End If
        Return T
    End Function
    Private Shared Function CalculateEThermocoupleVoltsToTemp(ByVal V__1 As Double) As Double
        Dim T As Double
        Dim v__2 As Double = V__1
        If V__1 >= 0.0 Then
            T = v__2 * ((0.017057035) + v__2 * ((-0.00000023301759) + v__2 * ((0.0000000000065435585) + v__2 * ((-7.3562749E-17) + v__2 * ((-1.7896001E-21) + v__2 * ((8.4036165E-26) + v__2 * ((-1.3735879E-30) + v__2 * ((1.0629823E-35) + v__2 * (-3.2447087E-41)))))))))
        Else
            T = v__2 * ((0.016977288) + v__2 * ((-0.0000004351497) + v__2 * ((-0.00000000015859697) + v__2 * ((-0.000000000000092502871) + v__2 * ((-2.6084314E-17) + v__2 * ((-4.1360199E-21) + v__2 * ((-3.403403E-25) + v__2 * (-1.156489E-29))))))))
        End If
        Return T
    End Function
    Private Shared Function CalculateBThermocoupleVoltsToTemp(ByVal V__1 As Double) As Double
        Dim T As Double
        Dim v__2 As Double = V__1
        If V__1 >= 2431.0 Then
            T = (213.15071) + v__2 * ((0.28510504) + v__2 * ((-0.000052742887) + v__2 * ((0.0000000099160804) + v__2 * ((-0.0000000000012965303) + v__2 * ((0.0000000000000001119587) + v__2 * ((-6.0625199E-21) + v__2 * ((1.8661696E-25) + v__2 * (-2.4878585E-30))))))))
        Else
            T = (98.42332) + v__2 * ((0.699715) + v__2 * ((-0.00084765304) + v__2 * ((0.0000010052644) + v__2 * ((-0.00000000083345952) + v__2 * ((0.00000000000045508542) + v__2 * ((-0.00000000000000015523037) + v__2 * ((2.988675E-20) + v__2 * (-2.474286E-24))))))))
        End If
        Return T
    End Function

    Private Function ConvertTempToVolts(ByVal T As Double) As Double
        Dim V As Double = 0

        Select Case ThermocoupleType

            Case 0
                V = CalculateBThermocoupleTempToVolts(T)
                Exit Select
            Case 1
                V = CalculateEThermocoupleTempToVolts(T)
                Exit Select
            Case 2
                V = CalculateJThermocoupleTempToVolts(T)
                Exit Select
            Case 3
                V = CalculateKThermocoupleTempToVolts(T)
                Exit Select
            Case 4
                V = CalculateRThermocoupleTempToVolts(T)
                Exit Select
            Case 5
                V = CalculateSThermocoupleTempToVolts(T)
                Exit Select
            Case 6
                V = CalculateTThermocoupleTempToVolts(T)
                Exit Select
            Case 7
                V = CalculateNThermocoupleTempToVolts(T)
                Exit Select
        End Select

        Return (V / 1000000.0)
    End Function
    Private Shared Function CalculateNThermocoupleTempToVolts(ByVal T As Double) As Double
        Dim V As Double
        Dim c As Double = T
        If T >= 0.0 Then
            V = (25.929394601) * c + (0.01571014188) * c * c + (0.000043825627237) * c * c * c + (-0.00000025261169794) * c * c * c * c + (0.00000000064311819339) * c * c * c * c * c + (-0.0000000000010063471519) * c * c * c * c * c * c + (0.00000000000000099745338992) * c * c * c * c * c * c * c + (-6.0863245607E-19) * c * c * c * c * c * c * c * c + (2.0849229339E-22) * c * c * c * c * c * c * c * c * c + (-3.0682196151E-26) * c * c * c * c * c * c * c * c * c * c
        Else
            V = (26.159105962) * c + (0.010957484228) * c * c + (-0.000093841111554) * c * c * c + (-0.000000046412039759) * c * c * c * c + (-0.0000000026303357716) * c * c * c * c * c + (-0.000000000022653438003) * c * c * c * c * c * c + (-0.000000000000076089300791) * c * c * c * c * c * c * c + (-9.3419667835E-17) * c * c * c * c * c * c * c * c
        End If
        Return V
    End Function
    Private Shared Function CalculateTThermocoupleTempToVolts(ByVal T As Double) As Double
        Dim V As Double
        Dim c As Double = T
        If T >= 0.0 Then
            V = (38.748106364) * c + (0.03329222788) * c * c + (0.00020618243404) * c * c * c + (-0.0000021882256846) * c * c * c * c + (0.000000010996880928) * c * c * c * c * c + (-0.000000000030815758772) * c * c * c * c * c * c + (0.00000000000004547913529) * c * c * c * c * c * c * c + (-2.7512901673E-17) * c * c * c * c * c * c * c * c
        Else
            V = (38.748106364) * c + (0.044194434347) * c * c + (0.00011844323105) * c * c * c + (0.000020032973554) * c * c * c * c + (0.00000090138019559) * c * c * c * c * c + (0.000000022651156593) * c * c * c * c * c * c + (0.00000000036071154205) * c * c * c * c * c * c * c + (0.0000000000038493939883) * c * c * c * c * c * c * c * c + (0.000000000000028213521925) * c * c * c * c * c * c * c * c * c + (0.00000000000000014251594779) * c * c * c * c * c * c * c * c * c * c + (4.8768662286E-19) * c * c * c * c * c * c * c * c * c * c * c + (1.079553927E-21) * c * c * c * c * c * c * c * c * c * c * c * c + (1.3945027062E-24) * c * c * c * c * c * c * c * c * c * c * c * c * c + (7.9795153927E-28) * c * c * c * c * c * c * c * c * c * c * c * c * c * c
        End If
        Return V
    End Function
    Private Shared Function CalculateSThermocoupleTempToVolts(ByVal T As Double) As Double
        Dim condition As Integer = (If((T >= 1664.5), 1, 0)) + (If((T >= 1064.18), 1, 0))
        Dim V As Double
        Dim c As Double = T
        If condition = 0 Then
            V = (5.40313308631) * c + (0.012593428974) * c * c + (-0.0000232477968689) * c * c * c + (0.0000000322028823036) * c * c * c * c + (-0.0000000000331465196389) * c * c * c * c * c + (0.0000000000000255744251786) * c * c * c * c * c * c + (-1.25068871393E-17) * c * c * c * c * c * c * c + (2.71443176145E-21) * c * c * c * c * c * c * c * c
        ElseIf condition = 1 Then
            V = (2951.57925316) + (-2.52061251332) * c + (0.0159564501865) * c * c + (-0.00000764085947576) * c * c * c + (0.00000000205305291024) * c * c * c * c + (-0.000000000000293359668173) * c * c * c * c * c
        Else
            V = (152232.118209) + (-268.819888545) * c + (0.171280280471) * c * c + (-0.0000345895706453) * c * c * c + (-0.00000000000934633971046) * c * c * c * c
        End If
        Return V
    End Function
    Private Shared Function CalculateRThermocoupleTempToVolts(ByVal T As Double) As Double
        Dim condition As Integer = (If((T >= 1664.5), 1, 0)) + (If((T >= 1064.18), 1, 0))
        Dim V As Double
        Dim c As Double = T
        If condition = 0 Then
            V = (5.28961729765) * c + (0.0139166589782) * c * c + (-0.0000238855693017) * c * c * c + (0.0000000356916001063) * c * c * c * c + (-0.0000000000462347666298) * c * c * c * c * c + (0.0000000000000500777441034) * c * c * c * c * c * c + (-3.73105886191E-17) * c * c * c * c * c * c * c + (1.57716482367E-20) * c * c * c * c * c * c * c * c + (-2.81038625251E-24) * c * c * c * c * c * c * c * c * c
        ElseIf condition = 1 Then
            V = (2951.57925316) + (-2.52061251332) * c + (0.0159564501865) * c * c + (-0.00000764085947576) * c * c * c + (0.00000000205305291024) * c * c * c * c + (-0.000000000000293359668173) * c * c * c * c * c
        Else
            V = (152232.118209) + (-268.819888545) * c + (0.171280280471) * c * c + (-0.0000345895706453) * c * c * c + (-0.00000000000934633971046) * c * c * c * c
        End If
        Return V
    End Function

    Private Shared Function CalculateKThermocoupleTempToVolts(ByVal T As Double) As Double
        Dim V As Double
        Dim c As Double = T
        If T >= 760.0 Then
            V = (-17.600413686) + (38.921204975) * c + (0.018558770032) * c * c + (-0.000099457592874) * c * c * c + (0.00000031840945719) * c * c * c * c + (-0.00000000056072844889) * c * c * c * c * c + (0.00000000000056075059059) * c * c * c * c * c * c + (-0.00000000000000032020720003) * c * c * c * c * c * c * c + (9.7151147152E-20) * c * c * c * c * c * c * c * c + (-1.2104721275E-23) * c * c * c * c * c * c * c * c * c + (118.5976) * Math.Exp((-0.0001183432) * (c - 126.9686) * (c - 126.9686))
        Else
            V = (39.450128025) * c + (0.023622373598) * c * c + (-0.00032858906784) * c * c * c + (-0.0000049904828777) * c * c * c * c + (-0.000000067509059173) * c * c * c * c * c + (-0.00000000057410327428) * c * c * c * c * c * c + (-0.0000000000031088872894) * c * c * c * c * c * c * c + (-0.000000000000010451609365) * c * c * c * c * c * c * c * c + (-1.9889266878E-17) * c * c * c * c * c * c * c * c * c + (-1.6322697486E-20) * c * c * c * c * c * c * c * c * c * c
        End If
        Return V
    End Function
    Private Shared Function CalculateJThermocoupleTempToVolts(ByVal T As Double) As Double
        Dim V As Double
        Dim c As Double = T
        If T >= 760.0 Then
            V = (296456.25681) + (-1497.6127786) * c + (3.1787103924) * c * c + (-0.0031847686701) * c * c * c + (0.0000015720819004) * c * c * c * c + (-0.00000000030691369056) * c * c * c * c * c
        Else
            V = InlineAssignHelper(V, (50.381187815) * c + (0.03047583693) * c * c + (-0.00008568106572) * c * c * c + (0.00000013228195295) * c * c * c * c + (-0.00000000017052958337) * c * c * c * c * c + (0.00000000000020948090697) * c * c * c * c * c * c + (-0.00000000000000012538395336) * c * c * c * c * c * c * c + (1.5631725697E-20) * c * c * c * c * c * c * c * c)
        End If
        Return V
    End Function
    Private Shared Function CalculateEThermocoupleTempToVolts(ByVal T As Double) As Double
        Dim V As Double
        Dim c As Double = T
        If T >= 0.0 Then
            V = InlineAssignHelper(V, (58.66550871) * c + (0.045032275585) * c * c + (0.000028908407212) * c * c * c + (-0.00000033056896652) * c * c * c * c + (0.0000000006502440327) * c * c * c * c * c + (-0.00000000000019197495504) * c * c * c * c * c * c + (-0.0000000000000012536600497) * c * c * c * c * c * c * c + (2.1489217569E-18) * c * c * c * c * c * c * c * c + (-1.4388041782E-21) * c * c * c * c * c * c * c * c * c + (3.5960899481E-25) * c * c * c * c * c * c * c * c * c * c)
        Else
            V = InlineAssignHelper(V, (58.665508708) * c + (0.045410977124) * c * c + (-0.00077998048686) * c * c * c + (-0.000025800160843) * c * c * c * c + (-0.00000059452583057) * c * c * c * c * c + (-0.0000000093214058667) * c * c * c * c * c * c + (-0.00000000010287605534) * c * c * c * c * c * c * c + (-0.00000000000080370123621) * c * c * c * c * c * c * c * c + (-0.0000000000000043979497391) * c * c * c * c * c * c * c * c * c + (-1.6414776355E-17) * c * c * c * c * c * c * c * c * c * c + (-3.9673619516E-20) * c * c * c * c * c * c * c * c * c * c * c + (-5.5827328721E-23) * c * c * c * c * c * c * c * c * c * c * c * c + (-3.4657842013E-26) * c * c * c * c * c * c * c * c * c * c * c * c * c)
        End If
        Return V
    End Function
    Private Shared Function CalculateBThermocoupleTempToVolts(ByVal T As Double) As Double
        Dim V As Double
        Dim c As Double = T
        If T >= 630.615 Then
            V = (-3893.8168621) + (28.57174747) * c + (-0.084885104785) * c * c + (0.00015785280164) * c * c * c + (-0.00000016835344864) * c * c * c * c + (0.00000000011109794013) * c * c * c * c * c + (-0.000000000000044515431033) * c * c * c * c * c * c + (9.8975640821E-18) * c * c * c * c * c * c * c + (-9.3791330289E-22) * c * c * c * c * c * c * c * c
        Else
            V = (-0.24650818346) * c + (0.0059040421171) * c * c + (-0.0000013257931636) * c * c * c + (0.0000000015668291901) * c * c * c * c + (-0.000000000001694452924) * c * c * c * c * c + (0.00000000000000062990347094) * c * c * c * c * c * c
        End If
        Return V
    End Function
    Private Shared Function ConvertThermistorReading(ByVal CJCVoltage As Double) As Double
        Dim voltageRef As Double = 2.5
        Dim R1 As Double = 189000.0
        Dim Irt As Double = (voltageRef - CJCVoltage) / R1
        Dim a As Double = CDbl(0.001295361)
        Dim b As Double = CDbl(0.0002343159)
        Dim c As Double = CDbl(0.0000001018703)
        Dim lnRt As Double = Math.Log(CJCVoltage / Irt)
        Dim T As Double = 1 / (a + lnRt * (b + c * (Math.Pow(lnRt, 2))))
        Return (T - 273.15)

    End Function
#End Region
#End Region
#Region "FormEvents"
    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        closeRequested = True
        If workerThread IsNot Nothing Then
            ThreadStopMarker = True
            e.Cancel = True
        End If
        If dmmSession IsNot Nothing Then
            If dmmSession.IsDisposed Then
                dmmSession.Measurement.Abort()
                dmmSession.Close()
            End If
        End If
        'Abort scanning.

        If switchSession IsNot Nothing Then
            If switchSession.IsDisposed Then
                switchSession.Scan.Abort()
                switchSession.Close()
            End If
        End If


    End Sub
    Private Sub startButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles startButton.Click
        ChangeControlState(False)
        Try
            ThreadStopMarker = False
            ToggleButtonState(False)
            CloseSwitchSession()
            InitializeSwitchSession()
            ConfigureSwitchScan()
            CommitScan()

            'Programming the DMM
            CloseDmmSession()
            InitializeDmmSession()
            ConfigureDmmMeasurement()
            dmmInstrumentModel = GetDmmInstrumentModel()
            'If the DMM model is an NI 4060, add a delay.
            If dmmInstrumentModel.Equals("PXI-4060") OrElse dmmInstrumentModel.Equals("PCI-4060") Then
                Thread.Sleep(1000)
            End If

            InitiateDmmMeasurement()
            CreateDataGridColumns()
            InitiateSwitchScan()


            StartMeasurement()
        Catch ex As Exception
            ShowError(ex.Message)
            ChangeControlState(True)
            ToggleButtonState(True)
            CloseDmmSession()
            CloseSwitchSession()
        End Try

    End Sub

    Private Sub CreateDataGridColumns()
        dataGridViewResults.Rows.Clear()
        dataGridViewResults.ColumnCount = 0
        For i As Integer = 0 To NumberOfChannels - 1
            dataGridViewResults.Columns.Add("", "Temperature Reading " & (i + 1))
        Next
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles stopButton.Click
        ' Stops the Acquisition.
        If workerThread IsNot Nothing Then
            ThreadStopMarker = True

        End If
        'Abort scanning.
        If dmmSession IsNot Nothing Then
            dmmSession.Measurement.Abort()
            CloseDmmSession()
        End If
        If switchSession IsNot Nothing Then
            switchSession.Scan.Abort()
            CloseSwitchSession()
        End If
        ChangeControlState(True)
        ToggleButtonState(True)
    End Sub
    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
        target = value
        Return value
    End Function
#End Region

    Private Sub ChangingControlState()
        ChangeControlState(True)
        ToggleButtonState(True)
    End Sub


End Class

