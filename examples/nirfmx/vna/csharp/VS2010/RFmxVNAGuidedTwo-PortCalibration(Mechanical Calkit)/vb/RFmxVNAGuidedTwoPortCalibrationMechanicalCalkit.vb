'Steps:
'1. Open a New RFmx session.
'2. Configure Frequency Reference.
'3. Configure sweep settings: Sweep Type, Start Frequency, Stop Frequency, Number Of Frequency Points, If Bandwidth, And Power Level And Test Rx Attenuation With different port names.
'4. Select S-Parameter measurement. 
'5. Configure Calibration Ports, Calibration Method And Thru.
'6. Import Calkit File
'7. Configure Connector type & Mechanical Calkit Name for each VNA port.
'8. Initiate Calibration.
'9. Acquire Calibration data after user confirmation.
'10. Save Calibration data.
'11. Save Calset data to a file.
'12. Get Calset Frequency Grid.
'13. Get Calset Error Terms.
'13a. Calculate Magnitude of Error Terms
'14. Close RFmx Session.

Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaGuidedTwoPortCalibrationMechanicalCalkit
    Public Class RFmxVnaGuidedTwoPortCalibrationMechanicalCalkit
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
        Private CorrectionCalibrationMethod As RFmxVnaMXCorrectionCalibrationMethod
        Private CorrectionCalibrationThruMethod As RFmxVnaMXCorrectionCalibrationThruMethod
        Private calibrationPorts As String()
        Private MechanicalCalkitName As String
        Private connectorType As String
        Private calibrationTimeout As Double
        Private ThruCoaxDelay As Double
        Private CalibrationEstimatedThruDelay As Double
        Private CalStepCount As Integer
        Private CalStepDescription As String
        Dim calkitFilePath() As String = {""}
        Private calsetFilePath As String
        Private portSelectorString As String
        Private calstepSelectorString As String
        Private frequencyGrid As Double()
        Private errorTerm(9)() As ComplexSingle

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
            CorrectionCalibrationMethod = RFmxVnaMXCorrectionCalibrationMethod.Solt
            CorrectionCalibrationThruMethod = RFmxVnaMXCorrectionCalibrationThruMethod.Auto
            calibrationTimeout = 100.0                                                                          'seconds
            ThruCoaxDelay = Double.NaN                                                                          'seconds
            calibrationPorts = New String() {"port1", "port2"}
            MechanicalCalkitName = ""
            connectorType = "3.5mm female"
            calsetFilePath = ""
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
            vna.SetCorrectionCalibrationPorts("", calibrationPorts)
            vna.SetCorrectionCalibrationMethod("", CorrectionCalibrationMethod)
            vna.SetCorrectionCalibrationThruMethod("", CorrectionCalibrationThruMethod)
            If Not Double.IsNaN(ThruCoaxDelay) Then
                vna.SetCorrectionCalibrationThruCoaxDelay("", ThruCoaxDelay)
            End If
            For Each filePath As String In calkitFilePath
              vna.CalkitManagerImportCalkit("", filePath)
            Next
            For Each portSelector As String In calibrationPorts
              Dim portSelectorString As String = RFmxVnaMX.BuildPortString("", portSelector)
              vna.SetCorrectionCalibrationCalkitType(portSelectorString, RFmxVnaMXCorrectionCalibrationCalkitType.Mechanical)
              vna.SetCorrectionCalibrationConnectorType(portSelectorString, connectorType)
              vna.SetCorrectionCalibrationCalkitMechanicalName(portSelectorString, MechanicalCalkitName)
            Next
            vna.CalibrationInitiate("")
            vna.GetCorrectionCalibrationStepCount("", CalStepCount)
            For i As Integer = 0 To CalStepCount - 1
                calstepSelectorString = RFmxVnaMX.BuildCalstepString("", i)
                vna.GetCorrectionCalibrationStepDescription(calstepSelectorString, CalStepDescription)
                Console.WriteLine($"CalStep {i + 1} Description: {CalStepDescription}")
                Console.WriteLine("Press any key for Next Cal Step...")
                Console.ReadKey(True)
                Console.Clear()
                vna.CalibrationAcquire(calstepSelectorString, calibrationTimeout)
            Next
            vna.CalibrationSave("", "")
            vna.CalsetSaveToFile("", "", calsetFilePath)
            vna.GetCorrectionCalibrationEstimatedThruDelay("", CalibrationEstimatedThruDelay)
            Console.WriteLine($"Calibration Estimated Thru Delay: {CalibrationEstimatedThruDelay}")
        End Sub

        Private Sub RetrieveResults()
            vna.CalsetGetFrequencyGrid("", "", RFmxVnaMXCalFrequencyGrid.Directivity, frequencyGrid)
            For i As Integer = 0 To 9
                vna.CalsetGetErrorTerm("", "", GetCalErrorTermType(i), GetCalPort1(i), GetCalPort2(i), errorTerm(i))
                CalculateMagnitude(errorTerm(i))
            Next
        End Sub

        Private Function GetCalErrorTermType(index As Integer) As RFmxVnaMXCalErrorTerm
            Select Case index
                Case 0, 1
                    Return RFmxVnaMXCalErrorTerm.Directivity
                Case 2, 3
                    Return RFmxVnaMXCalErrorTerm.SourceMatch
                Case 4, 5
                    Return RFmxVnaMXCalErrorTerm.ReflectionTracking
                Case 6, 7
                    Return RFmxVnaMXCalErrorTerm.TransmissionTracking
                Case 8, 9
                    Return RFmxVnaMXCalErrorTerm.LoadMatch
                Case Else
                    Throw New InvalidOperationException("Invalid index")
            End Select
        End Function

        Private Function GetCalPort1(index As Integer) As String
            Return If(index Mod 2 = 0, "port1", "port2")
        End Function

        Private Function GetCalPort2(index As Integer) As String
            return If(index Mod 2 = 0, "port2", "port1")
        End Function

        Private Sub CalculateMagnitude(ByRef complexArray() As ComplexSingle)
            For i As Integer = 0 To complexArray.Length - 1
                Dim absValue As Double = Math.Sqrt(complexArray(i).Real * complexArray(i).Real + complexArray(i).Imaginary * complexArray(i).Imaginary)
                Dim magnitude As Single = CSng(20 * Math.Log10(absValue))
                complexArray(i) = New ComplexSingle(magnitude, 0)
            Next
        End Sub

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
