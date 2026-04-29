'Steps:
'1. Open a New RFmx session.
'2. Configure Frequency Reference.
'3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number Of Frequency Points, If Bandwidth, And Power Level And Test Rx Attenuation With different port names.
'4. Select S-Parameter measurement. 
'5. Configure Calibration Ports And Calibration Method.
'6. Configure Connector type & vCal Resource Name for each VNA port.
'7. Initiate Calibration.
'8. Acquire Calibration data after user confirmation.
'9. Save Calibration data.
'10. Save Calset data to a file.
'11. Get Calset Error Terms.
'11a. Calculate Magnitude of Error Terms
'12. Close RFmx Session.

Imports System.Linq
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaCalibrationWithCalsetSaveTwoPort
    Public Class RFmxVnaCalibrationWithCalsetSaveTwoPort
        Private instrSession As RFmxInstrMX
        Private vna As RFmxVnaMX
        Private resourceName As String

        Private frequencyStart As Double
        Private frequencyEnd As Double
        Private numberOfFrequencyPoints As Integer
        Private port1PowerLevel As Double
        Private port2PowerLevel As Double
        Private port1TestReceiverAttenuation As Double
        Private port2TestReceiverAttenuation As Double
        Private IFBandwidth As Double

        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double

        Private vCalResourceName As String
        Private connectorType As String
        Private calibrationTimeout As Double
        Private vCalOrientation As String
        Private autoDetectVCalOrientation As Boolean

        Private calsetFilePath As String
        Private frequencyGrid As Double()
        Private portSelectorString As String

        Private timeout As Double

        Public Sub Run()
            Try
                InitializeVariables()
                InitializeInstr()
                ConfigureVna()
                RetrieveResults()
            Catch ex As Exception
                DisplayError(ex)
            Finally
                'Close session
                CloseSession()
                Console.WriteLine("Press any key to exit")
                Console.ReadKey()
            End Try
        End Sub

        Private Sub InitializeVariables()
            resourceName = "VNA"

            frequencyStart = 1000000000.0                                                                       '(Hz)
            frequencyEnd = 26000000000.0                                                                        '(Hz)
            numberOfFrequencyPoints = 251
            port1PowerLevel = -10.0                                                                             '(dBm)
            port2PowerLevel = -10.0                                                                             '(dBm)
            port1TestReceiverAttenuation = 0.0                                                                  '(dB)
            port2TestReceiverAttenuation = 0.0                                                                  '(dB)
            IFBandwidth = 100000.0                                                                              '(Hz)

            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock
            frequencyReferenceFrequency = 100000000.0                                                           '(Hz)

            vCalResourceName = "vCal"
            connectorType = "3.5 mm female"
            calibrationTimeout = 100.0                                                                          'seconds
            vCalOrientation = "PortA:Port1,PortB:Port2"
            autoDetectVCalOrientation = True

            calsetFilePath = ""

            timeout = 10.0                                                                                      'seconds
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureVna()
            vna = instrSession.GetVnaSignalConfiguration()                                                      'Create a new RFmx Session
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            vna.SetSweepType("", RFmxVnaMXSweepType.Linear)
            vna.SetStartFrequency("", frequencyStart)
            vna.SetStopFrequency("", frequencyEnd)
            vna.SetNumberOfPoints("", numberOfFrequencyPoints)
            vna.SetIFBandwidth("", IFBandwidth)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port1")
            vna.SetPowerLevel(portSelectorString, port1PowerLevel)
            vna.SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation)
            portSelectorString = RFmxVnaMX.BuildPortString("", "port2")
            vna.SetPowerLevel(portSelectorString, port2PowerLevel)
            vna.SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation)

            vna.SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, False)

            vna.SetCorrectionCalibrationPorts("", New String() {"port1", "port2"})
            vna.SetCorrectionCalibrationMethod("", RFmxVnaMXCorrectionCalibrationMethod.Solt)

            vna.SetCorrectionCalibrationConnectorType("port::all", connectorType)
            vna.SetCorrectionCalibrationCalkitElectronicResourceName("port::all", vCalResourceName)

            If autoDetectVCalOrientation Then
                vna.AutoDetectvCalOrientation("")
                vna.GetCorrectionCalibrationCalkitElectronicOrientation("", vCalOrientation)
                Console.WriteLine(String.Format("vCal Orientation      : {0}", vCalOrientation))
            Else
                vna.SetCorrectionCalibrationCalkitElectronicOrientation("", vCalOrientation)
            End If

            vna.CalibrationInitiate("")

            vna.CalibrationAcquire("", calibrationTimeout)
            vna.CalibrationSave("", "")

            vna.CalsetSaveToFile("", "", calsetFilePath)

        End Sub
        Private Sub RetrieveResults()
            Dim errorTermIdentifier() As RFmxVnaMXCalErrorTerm = New RFmxVnaMXCalErrorTerm() {
                RFmxVnaMXCalErrorTerm.Directivity,
                RFmxVnaMXCalErrorTerm.SourceMatch,
                RFmxVnaMXCalErrorTerm.ReflectionTracking,
                RFmxVnaMXCalErrorTerm.TransmissionTracking,
                RFmxVnaMXCalErrorTerm.LoadMatch}
            vna.CalsetGetFrequencyGrid("", "", RFmxVnaMXCalFrequencyGrid.Directivity, frequencyGrid)
            Dim errorTermsPort1(frequencyGrid.Length) As ComplexSingle
            Dim errorTermsPort2(frequencyGrid.Length) As ComplexSingle
            For Each errorTermType As RFmxVnaMXCalErrorTerm In CType(errorTermIdentifier, Collections.Generic.IEnumerable(Of RFmxVnaMXCalErrorTerm))
                vna.CalsetGetErrorTerm("", "", errorTermType, "port1", "port2", errorTermsPort1)
                vna.CalsetGetErrorTerm("", "", errorTermType, "port2", "port1", errorTermsPort2)
                Dim errorTerms(errorTermsPort1.Length + errorTermsPort2.Length - 2) As ComplexSingle
                errorTerms = errorTermsPort1.Concat(errorTermsPort2).ToArray()
                For i As Integer = 0 To errorTerms.Length - 1
                    Dim errorTermMagnitude() As Single = CalculateMagnitude(errorTerms)
                Next
            Next
        End Sub

        Private Function CalculateMagnitude(ByRef complexArray() As ComplexSingle) As Single()
            Dim magnitude(complexArray.Length - 1) As Single
            For i As Integer = 0 To complexArray.Length - 1
                Dim absValue As Double = Math.Sqrt(complexArray(i).Real * complexArray(i).Real + complexArray(i).Imaginary * complexArray(i).Imaginary)
                magnitude(i) = CSng(20 * Math.Log10(absValue))
            Next
            Return magnitude
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
            Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " + ex.Message)
        End Sub
    End Class
End Namespace
