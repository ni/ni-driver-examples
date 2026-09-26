//Steps:
//1. Open NI-RFSG session.
//2. Configure Reference Clock Source, Frequency, Power Level, and External Gain.
//3. Read the waveform from the file and write it to RFSG memory.
//4. Set waveform generation mode to Script.
//5. Set the Script to be used for generation.
//6. Initiate signal generation.
//7A.Check the generation status.
//7B.Exit the loop if an error has occurred, the generation is done, or the generation is stopped.
//8. Abort signal generation.
//9. Disable the output.This sets the noise floor as low as possible.
//10. Call NI-RFSG Commit.
//11. Clear the waveforms and waveform properties from the device memory.
//12. Close the NI-RFSG session.

using System;
using NationalInstruments.ModularInstruments.NIRfsg;

namespace NationalInstruments.Examples.RfsgGenerateWaveformFromFileSingleRfsg
{
   public class RfsgGenerateWaveformFromFileSingleRfsg
   {
      NIRfsg rfsgSession;
      string filepath, resourceName, optionString, referenceClockSource, waveformName, script;
      double centerFrequency, powerLevel, externalAttenuation;

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
               generationStatus = rfsgSession.CheckGenerationStatus();
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
         filepath = "FileWithSingleWaveform.tdms";
         resourceName = "RFSG";
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
         rfsgSession = new NIRfsg(resourceName, true, false, optionString);
      }

      void ConfigureRfsg()
      {
         rfsgSession.RF.Configure(centerFrequency, powerLevel);
         rfsgSession.FrequencyReference.Configure(referenceClockSource, 10e6);
         rfsgSession.RF.ExternalGain = -externalAttenuation;
         rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
         rfsgSession.Arb.ReadAndDownloadWaveformFromFileTdms(waveformName, filepath, 0);
         rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
         rfsgSession.Arb.Scripting.WriteScript(script);
         rfsgSession.Initiate();
      }

      void CloseSession()
      {
         try
         {
            if (rfsgSession != null)
            {
               rfsgSession.Abort();
               rfsgSession.RF.OutputEnabled = false;
               rfsgSession.Utility.Commit();
               rfsgSession.Arb.ClearWaveform(waveformName);
               rfsgSession.Close();
               rfsgSession = null;
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
