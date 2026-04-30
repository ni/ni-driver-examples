'Steps:
'1. Open a New RFmx session.
'2. Create a New Calkit
'3. Define Male And Female Connectors for the Calkit
'4. Create Calibration Elements for Short calibration standard with Male And Female Connector
'5. Create Calibration Elements for Open calibration standard with Male And Female Connector
'6. Create Calibration Elements for Load calibration standard with Male And Female Connector
'7. Create Calibration Element for Thru calibration standard with Male-Male, Female-Female, And Male-Female Connectors
'8. Export the Calkit to file.
'9. Close RFmx Session.

Imports System
Imports System.IO
Imports System.Reflection
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaCalibrationCreateSOLTCalkitModelBased
    Public Class RFmxVnaCalibrationCreateSOLTCalkitModelBased
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String
        Private calkitID As String
        Private calkitSelectorString As String
        Private connectorSelectorString As String
        Private calibrationElementIDShort As String()
        Private ccalibrationElementIDOpen As String()
        Private calibrationElementIDLoad As String()
        Private calibrationElementIDThru As String()
        Private connectorID As String()
        Private connectorID_Combined As String()
        Private calElementSelectorString As String
        Private calkitFilePath As String

        Public Sub Run()
            Try
                InitializeVariables()
                InitializeInstr()
                ConfigureVna()
            Catch ex As Exception
                DisplayError(ex)
            Finally
                ' Close session
                CloseSession()
                Console.WriteLine("Press any key to exit")
                Console.ReadKey()
            End Try
        End Sub

        Private Sub InitializeVariables()
            resourceName = "VNA"
            calkitID = "Example SOLT CalKit (Model Based)"
            calkitFilePath = "..\\..\\..\\..\\..\\Support\\Example SOLT CalKit (Model Based).nckt"
            calibrationElementIDShort = New String() {"Short-M", "Short-F"}
            ccalibrationElementIDOpen = New String() {"Open-M", "Open-F"}
            calibrationElementIDLoad = New String() {"Load-M", "Load-F"}
            calibrationElementIDThru = New String() {"Thru-M-M", "Thru-F-F", "Thru-M-F"}
            connectorID = New String() {"3.5mm-Male", "3.5mm-Female"}
            connectorID_Combined = New String() {"3.5mm-Male,3.5mm-Male", "3.5mm-Female,3.5mm-Female", "3.5mm-Male,3.5mm-Female"}
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureVna()
            vna = instrSession.GetVnaSignalConfiguration()
            vna.CalkitManagerCreateCalkit("", calkitID)
            calkitSelectorString = RFmxVnaMX.BuildCalkitString("", calkitID)
            vna.CalkitManagerCalkitSetDescription(calkitSelectorString, "Example SOLT Calkit")
            vna.CalkitManagerCalkitSetVersion(calkitSelectorString, "1.0.0")
            For i As Integer = 0 To connectorID.Length - 1
                vna.CalkitManagerCalkitAddConnector(calkitSelectorString, connectorID(i))
                connectorSelectorString = RFmxVnaMX.BuildConnectorString(calkitSelectorString, connectorID(i))
                vna.CalkitManagerCalkitConnectorSetGender(connectorSelectorString, GetConnectorGender(i))
                vna.CalkitManagerCalkitConnectorSetType(connectorSelectorString, "3.5mm")
                vna.CalkitManagerCalkitConnectorSetDescription(connectorSelectorString, "Connector Description")
                vna.CalkitManagerCalkitConnectorSetMinimumFrequency(connectorSelectorString, 0)
                vna.CalkitManagerCalkitConnectorSetMaximumFrequency(connectorSelectorString, 100000000000.0)
                vna.CalkitManagerCalkitConnectorSetImpedance(connectorSelectorString, 50)
            Next
            For i As Integer = 0 To connectorID.Length - 1
                vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, calibrationElementIDShort(i))
                calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, calibrationElementIDShort(i))
                vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, New RFmxVnaMXCalkitManagerCalkitCalibrationElementType() {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Short})
                vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, New String() {connectorID(i)})
                vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, "Example Short")
                vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 1000000000.0)
                vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 99000000000.0)
                vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.ReflectModel)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetModelType(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelType.ReflectShort)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC0(calElementSelectorString, 0.000000000002)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC1(calElementSelectorString, 1.0E-22)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC2(calElementSelectorString, 2.0E-33)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC3(calElementSelectorString, 1.0E-44)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetDelay(calElementSelectorString, 0.00000000003)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetLoss(calElementSelectorString, 0.000000002)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetZ0(calElementSelectorString, 50)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.Known)
            Next
            For i As Integer = 0 To connectorID.Length - 1
                vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, ccalibrationElementIDOpen(i))
                calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, ccalibrationElementIDOpen(i))
                vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, New RFmxVnaMXCalkitManagerCalkitCalibrationElementType() {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Open})
                vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, New String() {connectorID(i)})
                vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, "Example Open")
                vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 1000000000.0)
                vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 99000000000.0)
                vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.ReflectModel)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetModelType(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelType.ReflectOpen)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC0(calElementSelectorString, 0.00000000000005)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC1(calElementSelectorString, 2.9999999999999998E-25)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC2(calElementSelectorString, 2.0E-35)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC3(calElementSelectorString, 1.0E-46)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetDelay(calElementSelectorString, 0.00000000003)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetLoss(calElementSelectorString, 0.000000002)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetZ0(calElementSelectorString, 50)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.Known)
            Next
            For i As Integer = 0 To connectorID.Length - 1
                vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, calibrationElementIDLoad(i))
                calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, calibrationElementIDLoad(i))
                vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, New RFmxVnaMXCalkitManagerCalkitCalibrationElementType() {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Load})
                vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, New String() {connectorID(i)})
                vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, "Example Load")
                vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 1000000000.0)
                vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 99000000000.0)
                vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.ReflectModel)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetModelType(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelType.Load)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.Known)
            Next
            For i As Integer = 0 To connectorID_Combined.Length - 1
                vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, calibrationElementIDThru(i))
                calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, calibrationElementIDThru(i))
                vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, New RFmxVnaMXCalkitManagerCalkitCalibrationElementType() {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Thru})
                vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, New String() {connectorID_Combined(i)})
                vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, "Example Thru")
                vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 1000000000.0)
                vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 99000000000.0)
                vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.DelayModel)
                vna.CalkitManagerCalkitCalibrationElementDelayModelSetDelay(calElementSelectorString, 0.00000000001)
            Next
            vna.CalkitManagerExportCalkit("", calkitID, calkitFilePath)
        End Sub

        Private Function GetConnectorGender(index As Integer) As RFmxVnaMXCalkitManagerCalkitConnectorGender
            Select Case index
                Case 0
                    Return RFmxVnaMXCalkitManagerCalkitConnectorGender.Male
                Case 1
                    Return RFmxVnaMXCalkitManagerCalkitConnectorGender.Female
                Case Else
                    Throw New InvalidOperationException("Invalid index")
            End Select
        End Function

        Private Sub CloseSession()
            If vna IsNot Nothing Then
                vna.Dispose()
                vna = Nothing
            End If
            If instrSession IsNot Nothing Then
                instrSession.Close()
                instrSession = Nothing
            End If
        End Sub

        Private Shared Sub DisplayError(ex As Exception)
            Console.WriteLine("ERROR:" & vbCrLf & ex.GetType().ToString() & ": " & ex.Message)
        End Sub
    End Class
End Namespace
