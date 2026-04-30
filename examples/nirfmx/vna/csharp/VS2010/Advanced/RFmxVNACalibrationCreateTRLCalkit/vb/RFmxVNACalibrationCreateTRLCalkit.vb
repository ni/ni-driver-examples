'Steps:
'1. Open a new RFmx session.
'2. Create a new Calkit.
'3. Define Male and Female Connectors for the Calkit.
'4. Create Calibration Elements for Reflect calibration standard with Male and Female Connector.
'5. Create Calibration Element for Thru calibration standard with Male-Female Connectors.
'6. Create Calibration Elements for Line calibration standards with Male-Female Connectors.
'7. Configure TRL Options.
'8. Export the Calkit to file.
'9. Close RFmx Session.

Imports System
Imports System.IO
Imports System.Reflection
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVNACalibrationCreateTRLCalkit
    Public Class RFmxVNACalibrationCreateTRLCalkit
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String
        Private calkitID As String
        Private connectorID As String()
        Private calkitSelectorString As String
        Private connectorSelectorString As String
        Private calElementSelectorString As String
        Private calibrationElementIDReflect As String()
        Private connectorIDTwoPort As String()
        Private calibrationElementIDThru As String
        Private calibrationElementIDLine As String()
        Private calibrationElementLineDelaySec As Double()
        Private calibrationElementLineMinimumFrequencyHz As Double()
        Private calibrationElementLineMaximumFrequencyHz As Double()
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
            calkitID = "Example TRL Calkit"
            calkitFilePath = "..\\..\\..\\..\\..\\Support\\Example TRL Calkit.nckt"
            connectorID = New String() {"3.5 mm Male", "3.5 mm Female"}
            calibrationElementIDReflect = New String() {"Reflect-M", "Reflect-F"}
            connectorIDTwoPort = New String() { connectorID(0), connectorID(1) }
            calibrationElementIDThru = "Insertable Thru"
            calibrationElementIDLine = New String() {"Line 1", "Line 2"}
            calibrationElementLineDelaySec = New Double() {55e-12, 15e-12}
            calibrationElementLineMinimumFrequencyHz = New Double() {2e09, 7e09}
            calibrationElementLineMaximumFrequencyHz = New Double() {7e09, 26.5e09}
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureVna()
            vna = instrSession.GetVnaSignalConfiguration()
            vna.CalkitManagerCreateCalkit("", calkitID)
            calkitSelectorString = RFmxVnaMX.BuildCalkitString("", calkitID)
            vna.CalkitManagerCalkitSetDescription(calkitSelectorString, "Example TRL Calkit")
            vna.CalkitManagerCalkitSetVersion(calkitSelectorString, "1.0.0")
            ' Create connectors
            For i As Integer = 0 To connectorID.Length - 1
                vna.CalkitManagerCalkitAddConnector(calkitSelectorString, connectorID(i))
                connectorSelectorString = RFmxVnaMX.BuildConnectorString(calkitSelectorString, connectorID(i))
                vna.CalkitManagerCalkitConnectorSetGender(connectorSelectorString, GetConnectorGender(i))
                vna.CalkitManagerCalkitConnectorSetType(connectorSelectorString, "3.5 mm")
                vna.CalkitManagerCalkitConnectorSetDescription(connectorSelectorString, "Connector Description")
                vna.CalkitManagerCalkitConnectorSetMinimumFrequency(connectorSelectorString, 0)
                vna.CalkitManagerCalkitConnectorSetMaximumFrequency(connectorSelectorString, 100000000000.0)
                vna.CalkitManagerCalkitConnectorSetImpedance(connectorSelectorString, 50)
            Next
            ' Create Reflect calibration standard for each connector
            For i As Integer = 0 To connectorID.Length - 1
                vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, calibrationElementIDReflect(i))
                calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, calibrationElementIDReflect(i))
                vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, New RFmxVnaMXCalkitManagerCalkitCalibrationElementType() {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Reflect})
                vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, New String() {connectorID(i)})
                vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, "Reflect (Short)")
                vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 0)
                vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 100.0E9)
                vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.ReflectModel)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetModelType(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelType.ReflectShort)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC0(calElementSelectorString, 0)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC1(calElementSelectorString, 0)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC2(calElementSelectorString, 0)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetC3(calElementSelectorString, 0)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetDelay(calElementSelectorString, 0)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetLoss(calElementSelectorString, 0)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetOffsetZ0(calElementSelectorString, 50)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.Estimate)
            Next
            ' Create Thru calibration standard
            vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, calibrationElementIDThru)
            calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, calibrationElementIDThru)
            vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, New RFmxVnaMXCalkitManagerCalkitCalibrationElementType() {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Thru})
            vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, connectorIDTwoPort)
            vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, "Insertable Thru")
            vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 0)
            vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 100.0e9)
            vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.DelayModel)
            vna.CalkitManagerCalkitCalibrationElementDelayModelSetDelay(calElementSelectorString, 0)
            ' Create Line standards
            For i As Integer = 0 To calibrationElementIDLine.Length - 1
                vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, calibrationElementIDLine(i))
                calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, calibrationElementIDLine(i))
                vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, New RFmxVnaMXCalkitManagerCalkitCalibrationElementType() {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Line})
                vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, connectorIDTwoPort)
                vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, calibrationElementIDLine(i))
                vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, calibrationElementLineMinimumFrequencyHz(i))
                vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, calibrationElementLineMaximumFrequencyHz(i))
                vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.DelayModel)
                vna.CalkitManagerCalkitCalibrationElementDelayModelSetDelay(calElementSelectorString, calibrationElementLineDelaySec(i))
            Next
            ' TRL Options
            vna.CalkitManagerCalkitSetTrlReferencePlane(calkitSelectorString, RFmxVnaMXCalkitManagerCalkitTrlReferencePlane.Thru)
            vna.CalkitManagerCalkitSetLrlLineAutoChar(calkitSelectorString, false)
            
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
