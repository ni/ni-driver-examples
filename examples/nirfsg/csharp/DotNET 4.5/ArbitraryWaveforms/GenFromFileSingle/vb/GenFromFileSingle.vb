'Steps:
'1. Open NI-RFSG session.
'2. Configure Reference Clock Source, Frequency, Power Level, and External Gain.
'3. Read the waveform from the file and write it to RFSG memory.
'4. Set waveform generation mode to Script.
'5. Set the Script to be used for generation.
'6. Initiate signal generation.
'7A.Check the generation status.
'7B.Exit the loop if an error has occurred, the generation is done, or the generation is stopped.
'8. Abort signal generation.
'9. Disable the output.This sets the noise floor as low as possible.
'10. Call NI-RFSG Commit.
'11. Clear the waveforms and waveform properties from the device memory.
'12. Close the NI-RFSG session.

Imports NationalInstruments.ModularInstruments.NIRfsg

Namespace NationalInstruments.Examples.RfsgGenerateWaveformFromFileSingleRfsg
   Public Class RfsgGenerateWaveformFromFileSingleRfsg
      Private rfsgSession As NIRfsg
      Private filepath As String, resourceName As String, optionString As String, referenceClockSource As String, waveformName As String, script As String
      Private centerFrequency As Double, powerLevel As Double, externalAttenuation As Double

      Public Sub Run()
         Try
            InitializeVariables()
            InitializeRfsg()
            ConfigureRfsg()
            Dim generationStatus As RfsgGenerationStatus = RfsgGenerationStatus.InProgress
            Console.WriteLine("Press any key to stop generation.")
            Do
               generationStatus = rfsgSession.CheckGenerationStatus()
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
         filepath = "FileWithSingleWaveform.tdms"
         resourceName = "RFSG"
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
         rfsgSession = New NIRfsg(resourceName, True, False, optionString)
      End Sub

      Private Sub ConfigureRfsg()
         rfsgSession.RF.Configure(centerFrequency, powerLevel)
         rfsgSession.FrequencyReference.Configure(referenceClockSource, 10000000.0)
         rfsgSession.RF.ExternalGain = -externalAttenuation
         rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower
         rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, filepath, 0)
         rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script
         rfsgSession.Arb.Scripting.WriteScript(script)
         rfsgSession.Initiate()
      End Sub

      Private Sub CloseSession()
         Try
            If rfsgSession IsNot Nothing Then
               rfsgSession.Abort()
               rfsgSession.RF.OutputEnabled = False
               rfsgSession.Utility.Commit()
               rfsgSession.Arb.ClearWaveform(waveformName)
               rfsgSession.Close()
               rfsgSession = Nothing
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
