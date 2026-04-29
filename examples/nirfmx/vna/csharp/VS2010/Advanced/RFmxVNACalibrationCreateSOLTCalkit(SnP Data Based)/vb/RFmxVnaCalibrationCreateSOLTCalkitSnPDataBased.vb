'Steps:
'1. Open a New RFmx session.
'2. Create a New Calkit.
'3. Define Male And Female Connectors for the Calkit.
'4. Create Calibration Elements for One-port Standards.
'5. Create Calibration Elements for Two-Port Thru's.
'6. Export the Calkit to file
'7. Close RFmx Session.

Imports System
Imports System.IO
Imports System.Reflection
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaCalibrationCreateSOLTCalkitSnPDataBased
    Public Class RFmxVnaCalibrationCreateSOLTCalkitSnPDataBased
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String
        Private calkitID As String
        Private calkitSelectorString As String
        Private connectorSelectorString As String
        Private calElementSelectorString As String
        Private onePortCalibrationSParamterfiles As String()
        Private twoPortCalibrationSParamterfilesfortheThrus As String()
        Private connectorID As String()
        Private connectorID_Combined As String()
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
            calkitID = "Example SOLT CalKit (SnP Data Based)"
            calkitFilePath = "..\\..\\..\\..\\..\\Support\Example SOLT CalKit (SnP Data Based).nckt"
            connectorID = New String() {"3.5mm-Male", "3.5mm-Female"}
            connectorID_Combined = New String() {"3.5mm-Male,3.5mm-Male", "3.5mm-Female,3.5mm-Female", "3.5mm-Male,3.5mm-Female"}
            onePortCalibrationSParamterfiles = New String() {"Short(m).s1p", "Open(m).s1p", "Load(m).s1p", "OffsetShort(m).s1p", "Short(f).s1p", "Open(f).s1p", "Load(f).s1p", "OffsetShort(f).s1p"}
            twoPortCalibrationSParamterfilesfortheThrus = New String() {"Thru(m-m).s2p", "Thru(f-f).s2p", "Thru(m-f).s2p"}
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
                vna.CalkitManagerCalkitConnectorSetMaximumFrequency(connectorSelectorString, 100.0e9)
                vna.CalkitManagerCalkitConnectorSetImpedance(connectorSelectorString, 50)
            Next
            For j As Integer = 0 To connectorID.Length - 1
                Dim connector As String = connectorID(j)
                Dim startIndex As Integer = If(connector = "3.5mm-Male", 0, 4)
                Dim endIndex As Integer = If(connector = "3.5mm-Male", 4, onePortCalibrationSParamterfiles.Length)
                For i As Integer = startIndex To endIndex - 1
                    vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, Path.GetFileNameWithoutExtension(onePortCalibrationSParamterfiles(i)))
                    calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, Path.GetFileNameWithoutExtension(onePortCalibrationSParamterfiles(i)))
                    vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, New RFmxVnaMXCalkitManagerCalkitCalibrationElementType() {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Termination})
                    vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, onePortCalibrationSParamterfiles(i))
                    vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, New String() {connector})
                    vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 1.0e9)
                    vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 99.0e9)
                    vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.Sparameter)
                    vna.CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.Known)
                    vna.CalkitManagerCalkitCalibrationElementSParameterSetFromFile(calElementSelectorString, onePortCalibrationSParamterfiles(i))
                Next
            Next
            For i As Integer = 0 To connectorID_Combined.Length - 1
                vna.CalkitManagerCalkitAddCalibrationElement(calkitSelectorString, Path.GetFileNameWithoutExtension(twoPortCalibrationSParamterfilesfortheThrus(i)))
                calElementSelectorString = RFmxVnaMX.BuildCalibrationElementString(calkitSelectorString, Path.GetFileNameWithoutExtension(twoPortCalibrationSParamterfilesfortheThrus(i)))
                vna.CalkitManagerCalkitCalibrationElementSetTypes(calElementSelectorString, New RFmxVnaMXCalkitManagerCalkitCalibrationElementType() {RFmxVnaMXCalkitManagerCalkitCalibrationElementType.Thru})
                vna.CalkitManagerCalkitCalibrationElementSetDescription(calElementSelectorString, twoPortCalibrationSParamterfilesfortheThrus(i))
                vna.CalkitManagerCalkitCalibrationElementSetPortConnectors(calElementSelectorString, New String() {connectorID_Combined(i)})
                vna.CalkitManagerCalkitCalibrationElementSetMinimumFrequency(calElementSelectorString, 1.0e9)
                vna.CalkitManagerCalkitCalibrationElementSetMaximumFrequency(calElementSelectorString, 99.0e9)
                vna.CalkitManagerCalkitCalibrationElementSetSParameterDefinition(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementSParameterDefinition.Sparameter)
                vna.CalkitManagerCalkitCalibrationElementReflectModelSetSParamAvailability(calElementSelectorString, RFmxVnaMXCalkitManagerCalkitCalibrationElementReflectModelSParameterAvailability.Known)
                vna.CalkitManagerCalkitCalibrationElementSParameterSetFromFile(calElementSelectorString, twoPortCalibrationSParamterfilesfortheThrus(i))
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
