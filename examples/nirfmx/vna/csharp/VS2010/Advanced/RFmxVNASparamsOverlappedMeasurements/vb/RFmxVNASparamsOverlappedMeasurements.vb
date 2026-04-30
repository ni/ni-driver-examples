'Steps:
'1. Open a New RFmx session
'2. Create two named Signals called 'Signal1' and 'Signal2'. Signals here can be considered equivalent to the concept of Channels in third party software.
'For Each Signal, configure sweep And measurement settings. Note that, Power Level varies for each Signal in this example.
'    2A. Create Signal configuration
'    2B. Configure the sweep properties - Start Frequency, Stop Frequency, Number of Frequency Points, IF Bandwidth, Test Receiver Attn, Power Level
'    2C. Select S-Parameter Measurement
'    2D. Configure the number of S-Parameters
'    2E. Configure the S-Parameter And Format
'3. For each Signal, initiate the measurement And wait for the acquisition to complete.
'Here note that, between the two signals, only acquisition Is sequential And measurements are overlapped.
'4. Fetch the measurement results of each Signal
'5. Close the RFmx Session

Imports System
Imports System.IO
Imports System.Reflection
Imports NationalInstruments.RFmx.InstrMX
Imports NationalInstruments.RFmx.VnaMX

Namespace NationalInstruments.Examples.RFmxVnaSparamsOverlappedMeasurements
    Public Class RFmxVnaSparamsOverlappedMeasurements
        Private instrSession As RFmxInstrMX
        Private vnaSignal1 As RFmxVnaMX
        private vnaSignal2 As RFmxVnaMX
        Private resourceName As String
        Private namedSignals As String()
        Private frequencyStart As Double
        Private frequencyEnd As Double
        Private numberOfFrequencyPoints As Integer
        Private powerLevel As Double
        Private signalPowerLevel As Double
        Private port1TestReceiverAttenuation As Double
        Private port2TestReceiverAttenuation As Double
        Private IFBandwidth As Double
        Private frequencyReferenceSource As String
        Private frequencyReferenceFrequency As Double
        Private numberOfSParams As Integer
        Private sParamsSParameters As String()
        Private sParamsFormats As RFmxVnaMXSParamsFormat()
        Private sParamSelectorString As String
        Private portSelectorString As String
        Private acquisitionTimeout As Double
        Private fetchTimeout As Double
        Private numberOfSParamsResult As Integer
        Private correctionStateResult As RFmxVnaMXSParamsCorrectionState
        Private sParamsXDataResult As Double()
        Private sParamsY1DataResult As Single()()
        Private sParamsY2DataResult As Single()()

      Public Sub Run()
            Try
                InitializeVariables()
                InitializeInstr()
                ConfigureVna()
                RetrieveResults()
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
            frequencyStart = 1000000000.0                                                   ' (Hz)
            frequencyEnd = 26000000000.0                                                    ' (Hz)
            numberOfFrequencyPoints = 251
            powerLevel = -10.0                                                              ' (dBm)
            port1TestReceiverAttenuation = 0.0                                              ' (dB)
            port2TestReceiverAttenuation = 0.0                                              ' (dB)
            IFBandwidth = 100.0e3                                                           ' (Hz)
            frequencyReferenceSource = RFmxInstrMXConstants.PxiClock
            frequencyReferenceFrequency = 100000000.0                                       ' (Hz)
            numberOfSParams = 4
            sParamsSParameters = New String() {"S11", "S12", "S21", "S22"}
            namedSignals = {"Signal1", "Signal2"}
            sParamsFormats = {RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude, RFmxVnaMXSParamsFormat.Magnitude}
            acquisitionTimeout = 10.0                                                       ' seconds
            fetchTimeout = 10.0                                                             ' seconds
        End Sub

        Private Sub InitializeInstr()
            instrSession = New RFmxInstrMX(resourceName, "")
        End Sub

        Private Sub ConfigureVna()
            instrSession.ConfigureFrequencyReference("", frequencyReferenceSource, frequencyReferenceFrequency)
            vnaSignal1 = instrSession.GetVnaSignalConfiguration("Signal1")                  ' Create a new RFmx Session for Signal1
            vnaSignal2 = instrSession.GetVnaSignalConfiguration("Signal2")                  ' Create a new RFmx Session for Signal2
            For i As Integer = 0 To namedSignals.Length - 1
                GetSignalName(i).SetSweepType("", RFmxVnaMXSweepType.Linear)
                GetSignalName(i).SetStartFrequency("", frequencyStart)
                GetSignalName(i).SetStopFrequency("", frequencyEnd)
                GetSignalName(i).SetNumberOfPoints("", numberOfFrequencyPoints)
                GetSignalName(i).SetIFBandwidth("", IFBandwidth)
                portSelectorString = RFmxVnaMX.BuildPortString("", "port1")
                GetSignalName(i).SetTestReceiverAttenuation(portSelectorString, port1TestReceiverAttenuation)
                signalPowerLevel = powerLevel - (i * 10)
                GetSignalName(i).SetPowerLevel(portSelectorString, signalPowerLevel)
                portSelectorString = RFmxVnaMX.BuildPortString("", "port2")
                GetSignalName(i).SetPowerLevel(portSelectorString, signalPowerLevel)
                GetSignalName(i).SetTestReceiverAttenuation(portSelectorString, port2TestReceiverAttenuation)
                GetSignalName(i).SelectMeasurements("", RFmxVnaMXMeasurementTypes.SParams, False)
                GetSignalName(i).SParams.Configuration.SetNumberOfSParameters("", numberOfSParams)
                For k As Integer = 0 To numberOfSParams - 1
                    sParamSelectorString = RFmxVnaMX.BuildSParameterString("", k)
                    GetSignalName(i).SParams.Configuration.ConfigureSParameter(sParamSelectorString, sParamsSParameters(k))
                    GetSignalName(i).SParams.Configuration.SetFormat(sParamSelectorString, sParamsFormats(k))
                Next
            Next
            For i As Integer = 0 To namedSignals.Length - 1
                GetSignalName(i).Initiate("", "")
                instrSession.WaitForAcquisitionComplete(acquisitionTimeout)
            Next
        End Sub

        Private Sub RetrieveResults()
            For j As Integer = 0 To namedSignals.Length - 1
            GetSignalName(j).SParams.Configuration.GetNumberOfSParameters("", numberOfSParamsResult)
                GetSignalName(j).SParams.Results.FetchXData("", fetchTimeout, sParamsXDataResult)
                ReDim sParamsY1DataResult(numberOfSParamsResult - 1)
                ReDim sParamsY2DataResult(numberOfSParamsResult - 1)
                For i As Integer = 0 To numberOfSParamsResult - 1
                    sParamSelectorString = RFmxVnaMX.BuildSParameterString("", i)
                    GetSignalName(j).SParams.Results.FetchYData(sParamSelectorString, fetchTimeout, sParamsY1DataResult(i), sParamsY2DataResult(i))
                Next
                GetSignalName(j).SParams.Results.GetCorrectionState("", correctionStateResult)
            Next
        End Sub

        Private Function GetSignalName(index As Integer) As RFmxVnaMX
            Select Case index
                Case 0
                    Return vnaSignal1
                Case 1
                    Return vnaSignal2
                Case Else
                    Throw New InvalidOperationException("Invalid index")
            End Select
        End Function

        Private Sub CloseSession()
            If vnaSignal1 IsNot Nothing Then
                vnaSignal1.Dispose()
                vnaSignal1 = Nothing
            End If
            If vnaSignal2 IsNot Nothing Then
                vnaSignal2.Dispose()
                vnaSignal2 = Nothing
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
