using System;
using NationalInstruments.RFToolkits.Wlan.Generation;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsg.Internal;
using System.Runtime.InteropServices;


namespace _80211LoadConfigurationFromFileAndGenerateWaveformSingleRFSG
{
   class Program
   {
      static void Main(string[] args)
      {
         LoadConfigurationFromFileAndGenerateWaveform example = new LoadConfigurationFromFileAndGenerateWaveform();
         example.Run();
      }
   }

   class LoadConfigurationFromFileAndGenerateWaveform
   {
      double carrierFrequency = 5.18e9;
      double powerLevel = -10.0;
      double externalAttenuation = 0;
      String filePath = "";
      NIRfsg rfsgSession;
      niWLANG wlanSession;
      String rfsgResourceName = "RIO0";
      String rfsgClockSource = RfsgFrequencyReferenceSource.OnboardClock;
      NIRfsg externalLOSession = null;

      String waveformName = "Wlan";

      public void Run()
      {
         try
         {
            ConfigureRfsgSession();
            ConfigureWlanGenerationSession();
            CreateAndDownloadWaveform();
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

            if (wlanSession != null)
               wlanSession.Close();
         }
      }

      private void ConfigureWlanGenerationSession()
      {
         int isNewSession;

         if (wlanSession == null)
            wlanSession = new niWLANG("WLANG", niWLANGConstants.CompatibilityVersion060000, out isNewSession);

         wlanSession.LoadConfigurationFromFile(filePath, niWLANGConstants.True);
      }

      private void CreateAndDownloadWaveform()
      {
         double iqRate;
         double actualHeadRoom;
         String script = @"script GenerateWlan
				                repeat forever
					                generate Wlan
				                end repeat
			                end script";
         int standard;

         wlanSession.GetStandard(null, out standard);
         String ChannelString;
         IntPtr externalLOHandle = new IntPtr();

         if (externalLOSession != null)
            externalLOHandle = externalLOSession.GetInstrumentHandle().DangerousGetHandle();
         wlanSession.RFSGConfigureFrequencySingleLO(new IntPtr[] { rfsgSession.GetInstrumentHandle().DangerousGetHandle() }, niWLANGConstants.LOSourceOnboard, externalLOHandle, carrierFrequency,
                                                 niWLANGConstants.False, niWLANGConstants.False);

         if (standard == niWLANGConstants.Standard80211AcMimoOfdm || standard == niWLANGConstants.Standard80211AfMimoOfdm ||
             standard == niWLANGConstants.Standard80211nMimoOfdm || standard == niWLANGConstants.Standard80211AhMimoOfdm ||
             standard == niWLANGConstants.Standard80211AxMimoOfdm || standard == niWLANGConstants.Standard80211beMimoOfdm || standard == niWLANGConstants.Standard80211bnMimoOfdm)
         {
            wlanSession.RFSGCreateAndDownloadMIMOWaveforms(new IntPtr[] { rfsgSession.GetInstrumentHandle().DangerousGetHandle() }, null, 1, waveformName);
            ChannelString = "channel0";
         }
         else
         {
            wlanSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, waveformName);
            ChannelString = "";
         }

         wlanSession.GetIqRate(null, out iqRate);
         wlanSession.GetActualHeadroom(ChannelString, out actualHeadRoom);

         Console.WriteLine("Standard : {0}", standard);
         Console.WriteLine("IQ Rate : {0}", iqRate);
         Console.WriteLine("Actual HeadRoom : {0}", actualHeadRoom);

         wlanSession.Close();

         niWLANG.WLANG_RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, script, powerLevel);
         rfsgSession.RF.OutputEnabled = true;

         rfsgSession.Initiate();
         //Check for successful generation
         CheckGeneration();
         Console.WriteLine("Press any key to exit");
         Console.ReadKey();
      }

      private void ConfigureRfsgSession()
      {

         if (rfsgSession == null)
         {
            rfsgSession = new NIRfsg(rfsgResourceName, true, false);
         }
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
         niWLANG.WLANG_RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", null);
      }
   }
}
