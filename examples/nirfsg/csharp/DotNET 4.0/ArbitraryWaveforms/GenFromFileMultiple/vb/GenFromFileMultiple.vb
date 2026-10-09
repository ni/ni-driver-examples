' Steps:
'1. Open NI-RFSG sessions.
'2. Configure Reference Clock Source, Frequency, Power Level, and External Gain.
'3. Read the waveforms from the file and write it to RFSG memory.
'4. Set waveform generation mode to Script.
'5. Set the Script to be used for generation.
'6. Initiate signal generation on all configured RFSGs.
'7A. Check the generation status.
'7B. Exit the loop if an error has occurred, the generation is done, or the generation is stopped.
'8. Abort signal generation.
'9. Disable the output. This sets the noise floor as low as possible.
'10. Call NI-RFSG Commit.
'11. Clear the waveforms and waveform properties from the device memory.
'12. Close the NI-RFSG sessions.
'


Imports NationalInstruments.ModularInstruments.NIRfsg

Namespace NationalInstruments.Examples.RfsgGenerateWaveformFromFileMultipleRfsg
   Public Class RfsgGenerateWaveformFromFileMultipleRfsg
      Private rfsgSessions As NIRfsg()
      Private filepath As String, optionString As String, referenceClockSource As String, waveformName As String, script As String
      Private resourceNames As String()
      Private centerFrequency As Double, powerLevel As Double, externalAttenuation As Double
      Const numberOfSessions As Integer = 2

      Public Sub Run()
         Try
            InitializeVariables()
            InitializeRfsg()
            ConfigureRfsg()
            Dim generationStatus As RfsgGenerationStatus = RfsgGenerationStatus.InProgress
            Console.WriteLine("Press any key to stop generation.")
            Do
               For i As Integer = 0 To numberOfSessions - 1
                  generationStatus = rfsgSessions(i).CheckGenerationStatus()
                  If generationStatus = RfsgGenerationStatus.Complete Then
                     Exit For
                  End If
               Next

            Loop While generationStatus <> RfsgGenerationStatus.Complete AndAlso Not Console.KeyAvailable
         Catch ex As Exception
            DisplayError(ex)
         Finally
            CloseSession()
            Console.WriteLine("Press any key to exit the application.")
            Console.ReadKey()
         End Try
      End Sub

      Private Sub InitializeVariables()
         filepath = "FileWithMultipleWaveforms.tdms"
         rfsgSessions = New NIRfsg(numberOfSessions - 1) {}
         resourceNames = New String(numberOfSessions - 1) {"RFSG1", "RFSG2"}
         optionString = ""
         centerFrequency = 5180000000.0
         'Hz
         powerLevel = -10.0
         'dBm
         externalAttenuation = 0.0
         'dB
         referenceClockSource = RfsgFrequencyReferenceSource.OnboardClock
         waveformName = "waveform"
         script = "script GenerateWfm" & vbCr & vbLf & "                       repeat forever" & vbCr & vbLf & "                          generate waveform" & vbCr & vbLf & "                       end repeat" & vbCr & vbLf & "                    end script"
      End Sub

      Private Sub InitializeRfsg()
         For i As Integer = 0 To numberOfSessions - 1
            rfsgSessions(i) = New NIRfsg(resourceNames(i), True, False, optionString)
         Next
      End Sub

      Private Sub ConfigureRfsg()
         For i As Integer = 0 To numberOfSessions - 1
            rfsgSessions(i).RF.Configure(centerFrequency, powerLevel)
            rfsgSessions(i).FrequencyReference.Configure(referenceClockSource, 10000000.0)
            rfsgSessions(i).RF.ExternalGain = -externalAttenuation
            rfsgSessions(i).RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
            rfsgSessions(i).Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, filepath, CUInt(i))
            rfsgSessions(i).Arb.GenerationMode = RfsgWaveformGenerationMode.Script
            rfsgSessions(i).Arb.Scripting.WriteScript(script)
            rfsgSessions(i).Initiate()
         Next
      End Sub

      Private Sub CloseSession()
         Try
            If rfsgSessions IsNot Nothing Then
               For i As Integer = 0 To numberOfSessions - 1
                  If rfsgSessions(i) IsNot Nothing Then
                     rfsgSessions(i).Abort()
                     rfsgSessions(i).RF.OutputEnabled = False
                     rfsgSessions(i).Utility.Commit()
                     rfsgSessions(i).Arb.ClearWaveform(waveformName)
                     rfsgSessions(i).Close()
                     rfsgSessions(i) = Nothing
                  End If
               Next
            End If
         Catch ex As Exception
            DisplayError(ex)
         End Try
      End Sub

      Private Shared Sub DisplayError(ex As Exception)
         Console.WriteLine("ERROR:" & vbLf & Convert.ToString(ex.[GetType]()) & ": " & ex.Message)
      End Sub
   End Class
End Namespace
