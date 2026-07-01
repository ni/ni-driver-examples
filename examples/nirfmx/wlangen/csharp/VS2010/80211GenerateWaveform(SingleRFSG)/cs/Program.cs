using System;
using NationalInstruments.RFToolkits.Wlan.Generation;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsg.Internal;
using System.Runtime.InteropServices;

namespace _80211GenerateWaveformSingleRFSG
{
   class Program
   {
      static void Main(string[] args)
      {
         GenerateWaveformSingleUserExample example = new GenerateWaveformSingleUserExample();
         example.Run();
      }
   }

   class GenerateWaveformSingleUserExample
   {
      double carrierFrequency = 5.18e9;
      double channelBandwidth = 20e+6;
      int ofdmDataRate = 6;
      int dsssDataRate = 1;
      int mcsIndex = 0;
      double powerLevel = -10.0;
      double externalAttenuation = 0;
      string channelString;
      int standard = niWLANGConstants.Standard80211agOfdm;

      NIRfsg rfsgSession;
      NIRfsg externalLOSession = null;
      niWLANG wlanSession;
      String rfsgResourceName = "RFSA";
      String rfsgClockSource = RfsgFrequencyReferenceSource.OnboardClock;

      String waveformName = "Wlan";
      IntPtr handle = IntPtr.Zero;

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
            Console.WriteLine("ERROR:\n" + e.GetType() + ": " + e.Message);
         }
         finally
         {
            if (rfsgSession != null)
               rfsgSession.Close();

            if (wlanSession != null)
               wlanSession.Close();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
         }        
      }

      private void ConfigureWlanGenerationSession()
      {
         int isNewSession;

         if (wlanSession == null)
            wlanSession = new niWLANG("WLANG", niWLANGConstants.CompatibilityVersion060000, out isNewSession);

         if ((standard == niWLANGConstants.Standard80211AcMimoOfdm) || (standard == niWLANGConstants.Standard80211nMimoOfdm) ||
             (standard == niWLANGConstants.Standard80211AhMimoOfdm) || (standard == niWLANGConstants.Standard80211AfMimoOfdm) ||
             (standard == niWLANGConstants.Standard80211AxMimoOfdm) || (standard == niWLANGConstants.Standard80211beMimoOfdm) || (standard == niWLANGConstants.Standard80211bnMimoOfdm))
         {
            wlanSession.SetStandard(null, standard);
            wlanSession.SetChannelBandwidth(null, channelBandwidth);
            wlanSession.SetMcsIndex(null, mcsIndex);
            channelString = "channel0";
         }
         else if (standard == niWLANGConstants.Standard80211bgDsss)
         {
            wlanSession.SetStandard(null, standard);
            wlanSession.SetDsssDataRate(null, dsssDataRate);
            channelString = "";
         }
         else
         {
            wlanSession.SetStandard(null, standard);
            wlanSession.SetChannelBandwidth(null, channelBandwidth);
            wlanSession.SetOfdmDataRate(null, ofdmDataRate);
            channelString = "";
         }

         wlanSession.SetRFBlankingEnabled(null, niWLANGConstants.True);
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
         IntPtr externalLOHandle = new IntPtr();

         handle = rfsgSession.GetInstrumentHandle().DangerousGetHandle();

         if (externalLOSession != null)
            externalLOHandle = externalLOSession.GetInstrumentHandle().DangerousGetHandle();

         wlanSession.RFSGConfigureFrequencySingleLO(new IntPtr[] { handle }, niWLANGConstants.LOSourceOnboard, externalLOHandle, carrierFrequency, niWLANGConstants.False, niWLANGConstants.False);

         wlanSession.GetStandard(null, out standard);
         if (standard == niWLANGConstants.Standard80211AcMimoOfdm || standard == niWLANGConstants.Standard80211AfMimoOfdm ||
             standard == niWLANGConstants.Standard80211nMimoOfdm || standard == niWLANGConstants.Standard80211AhMimoOfdm ||
             standard == niWLANGConstants.Standard80211AxMimoOfdm || standard == niWLANGConstants.Standard80211beMimoOfdm || standard == niWLANGConstants.Standard80211bnMimoOfdm)
         {
            wlanSession.RFSGCreateAndDownloadMIMOWaveforms(new IntPtr[] { handle }, null, 1, waveformName);
         }
         else
         {
            wlanSession.RFSGCreateAndDownloadMIMOWaveforms(new IntPtr[] { handle }, null, 1, waveformName);
            channelString = "";
         }

         wlanSession.GetIqRate(null, out iqRate);
         wlanSession.GetActualHeadroom(channelString, out actualHeadRoom);

         Console.WriteLine("Standard : {0}", standard);
         Console.WriteLine("IQ Rate : {0}", iqRate);
         Console.WriteLine("Actual HeadRoom : {0}", actualHeadRoom);

         wlanSession.Close();

         niWLANG.WLANG_RFSGConfigureScript(handle, null, script, powerLevel);
         rfsgSession.RF.OutputEnabled = true;
         rfsgSession.Utility.Commit();
         rfsgSession.Initiate();
         //Check for successful generation
         CheckGeneration();
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
         rfsgSession.Utility.Commit();
         niWLANG.WLANG_RFSGClearDatabase(handle, null, waveformName);
      }
   }
}
