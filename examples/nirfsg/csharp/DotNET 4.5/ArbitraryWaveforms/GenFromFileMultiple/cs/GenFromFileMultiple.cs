//Steps:
//1. Open NI-RFSG sessions.
//2. Configure Reference Clock Source, Frequency, Power Level, and External Gain.
//3. Read the waveforms from the file and write it to RFSG memory.
//4. Set waveform generation mode to Script.
//5. Set the Script to be used for generation.
//6. Initiate signal generation on all configured RFSGs.
//7A.Check the generation status.
//7B.Exit the loop if an error has occurred, the generation is done, or the generation is stopped.
//8. Abort signal generation.
//9. Disable the output.This sets the noise floor as low as possible.
//10. Call NI-RFSG Commit.
//11. Clear the waveforms and waveform properties from the device memory.
//12. Close the NI-RFSG sessions.

using System;
using NationalInstruments.ModularInstruments.NIRfsg;

namespace NationalInstruments.Examples.RfsgGenerateWaveformFromFileMultipleRfsg
{
   public class RfsgGenerateWaveformFromFileMultipleRfsg
   {
      NIRfsg[] rfsgSessions;
      string filepath, optionString, referenceClockSource, waveformName, script;
      string[] resourceNames;
      double centerFrequency, powerLevel, externalAttenuation;
      const int numberOfSessions = 2;

      public void Run()
      {
         try
         {
            InitializeVariables();
            InitializeRfsg();
            ConfigureRfsg();
            RfsgGenerationStatus generationStatus = RfsgGenerationStatus.InProgress;
            Console.WriteLine("Press any key to stop generation.");
            do
            {
               for (int i = 0; i < numberOfSessions; i++)
               {
                  generationStatus = rfsgSessions[i].CheckGenerationStatus();
                  if (generationStatus == RfsgGenerationStatus.Complete)
                     break;
               }
            } while ((generationStatus != RfsgGenerationStatus.Complete) && (!Console.KeyAvailable));

         }
         catch (Exception ex)
         {
            DisplayError(ex);
         }
         finally
         {
            CloseSession();
            Console.WriteLine("Press any key to exit the application.");
            Console.ReadKey();
         }
      }

      void InitializeVariables()
      {
         filepath = "FileWithMultipleWaveforms.tdms";
         rfsgSessions = new NIRfsg[numberOfSessions];
         resourceNames = new string[numberOfSessions] { "RFSG1", "RFSG2" };
         optionString = "";
         centerFrequency = 5.18e9;                        //Hz
         powerLevel = -10.0;                              //dBm
         externalAttenuation = 0.0;                       //dB
         referenceClockSource = RfsgFrequencyReferenceSource.OnboardClock;
         waveformName = "waveform";
         script = @"script GenerateWfm
                       repeat forever
                          generate waveform
                       end repeat
                    end script";
      }

      void InitializeRfsg()
      {
         for (int i = 0; i < numberOfSessions; i++)
         {
            rfsgSessions[i] = new NIRfsg(resourceNames[i], true, false, optionString);
         }
      }

      void ConfigureRfsg()
      {
         for (int i = 0; i < numberOfSessions; i++)
         {
            rfsgSessions[i].RF.Configure(centerFrequency, powerLevel);
            rfsgSessions[i].FrequencyReference.Configure(referenceClockSource, 10e6);
            rfsgSessions[i].RF.ExternalGain = -externalAttenuation;
            rfsgSessions[i].RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
            rfsgSessions[i].Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, filepath, (uint)i);
            rfsgSessions[i].Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
            rfsgSessions[i].Arb.Scripting.WriteScript(script);
            rfsgSessions[i].Initiate();
         }
      }

      void CloseSession()
      {
         try
         {
            if (rfsgSessions != null)
            {
               for (int i = 0; i < numberOfSessions; i++)
               {
                  if (rfsgSessions[i] != null)
                  {
                     rfsgSessions[i].Abort();
                     rfsgSessions[i].RF.OutputEnabled = false;
                     rfsgSessions[i].Utility.Commit();
                     rfsgSessions[i].Arb.ClearWaveform(waveformName);
                     rfsgSessions[i].Close();
                     rfsgSessions[i] = null;
                  }
               }
            }
         }
         catch (Exception ex)
         {
            DisplayError(ex);
         }
      }

      static void DisplayError(Exception ex)
      {
         Console.WriteLine("ERROR:\n" + ex.GetType() + ": " + ex.Message);
      }
   }
}
