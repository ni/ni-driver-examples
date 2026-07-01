using System;
using NationalInstruments.RFToolkits.Wlan.Generation;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsg.Internal;
using System.Runtime.InteropServices;

namespace _80211abgjpnacSignalGenerationFromFile
{
   class Program
   {
      static void Main(string[] args)
      {
         SignalGenerationFromFileExample example = new SignalGenerationFromFileExample();
         example.Run();
      }
   }

   class SignalGenerationFromFileExample
   {
      double carrierFrequency = 5.18e9;
      double powerLevel = -10.0;
      double externalAttenuation = 0;
      NIRfsg rfsgSession;
      String rfsgResourceName = "RFSA";
      String rfsgClockSource = RfsgFrequencyReferenceSource.OnboardClock;
      String script = @"script GenerateWlan
                            repeat forever
		        			    generate Wlan
				            end repeat
			             end script";
      String filePath = @"";
      String waveformName = "Wlan";
      IntPtr handle = IntPtr.Zero;

      public void Run()
      {
         try
         {
            ConfigureRfsgSession();
            ReadAndDownloadWaveform();
            StopGeneration();
         }
         catch (Exception e)
         {
            Console.WriteLine("Error : " + e.ToString());
            Console.ReadKey();
         }
         finally
         {
            if (rfsgSession != null)
               rfsgSession.Close();
         }
      }

      private void ReadAndDownloadWaveform()
      {
         handle = rfsgSession.GetInstrumentHandle().DangerousGetHandle();
         niWLANG.WLANG_RFSGReadAndDownloadWaveformsFromFile(new IntPtr[] { handle }, 1, waveformName, filePath);
         niWLANG.WLANG_RFSGConfigureScript(handle, "", script, powerLevel);
         rfsgSession.RF.OutputEnabled = true;
         rfsgSession.Initiate();
         /*Check for successful generation*/
         CheckGeneration();
         Console.WriteLine(" Press any key to exit");
         Console.ReadKey();
      }

      private void ConfigureRfsgSession()
      {
         if (rfsgSession == null)
         {
            rfsgSession = new NIRfsg(rfsgResourceName, true, false);
         }

         rfsgSession.RF.Frequency = carrierFrequency;
         rfsgSession.RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
         rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
         rfsgSession.FrequencyReference.Configure(rfsgClockSource, 10.0e6);
         rfsgSession.RF.ExternalGain = -(externalAttenuation);
      }

      private bool CheckGeneration()
      {
         RfsgGenerationStatus status = rfsgSession.CheckGenerationStatus();
         return Convert.ToBoolean(status);
      }

      private void StopGeneration()
      {
         rfsgSession.Abort();
         rfsgSession.RF.OutputEnabled = false;
         rfsgSession.Utility.Commit();
         niWLANG.WLANG_RFSGClearDatabase(handle, null, waveformName);
      }
   }
}
