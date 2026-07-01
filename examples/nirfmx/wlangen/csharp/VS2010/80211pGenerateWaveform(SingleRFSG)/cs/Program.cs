using System;
using NationalInstruments.RFToolkits.Wlan.Generation;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsg.Internal;
using System.Runtime.InteropServices;


namespace _80211pGenerateWaveformSingleRFSG
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
      double powerLevel = -10.0;
      double externalAttenuation = 0;

      int standard = niWLANGConstants.Standard80211pOfdm;

      NIRfsg rfsgSession;
      NIRfsg externalLOSession = null;
      niWLANG wlanSession;
      String rfsgResourceName = "RIO0";
      String rfsgClockSource = RfsgFrequencyReferenceSource.OnboardClock;

      public void Run()
      {
         try
         {
            ConfigureRfsgSession();
            ConfigureWlanGenerationSession();
            CreateAndDownloadWaveform();
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

         wlanSession.SetStandard(null, standard);
         wlanSession.SetChannelBandwidth(null, channelBandwidth);
         wlanSession.SetOfdmDataRate(null, ofdmDataRate);
         wlanSession.SetPulseShapingFilterEnabled(null, niWLANGConstants.True);
         wlanSession.SetPulseShapingFilterParameter(null, 0.1);
         wlanSession.SetPulseShapingFilterType(null, niWLANGConstants.FilterRaisedCosine);
         wlanSession.SetPulseShapingFilterLength(null, 100);
         wlanSession.SetRFBlankingEnabled(null, niWLANGConstants.True);
      }

      private void CreateAndDownloadWaveform()
      {
         double iqRate;
         double actualHeadRoom;
         int standard;
         IntPtr externalLOHandle = new IntPtr();

         String script = @"script GenerateWlan
				                repeat forever
					                generate Wlan
				                end repeat
			                end script";

         wlanSession.GetStandard(null, out standard);

         if (externalLOSession != null)
            externalLOHandle = externalLOSession.GetInstrumentHandle().DangerousGetHandle();
         wlanSession.RFSGConfigureFrequencySingleLO(new IntPtr[] { rfsgSession.GetInstrumentHandle().DangerousGetHandle() }, niWLANGConstants.LOSourceOnboard, externalLOHandle, carrierFrequency,
                                                 niWLANGConstants.False, niWLANGConstants.False);


         wlanSession.RFSGCreateAndDownloadWaveform(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, "Wlan");
         wlanSession.GetIqRate(null, out iqRate);
         wlanSession.GetActualHeadroom("", out actualHeadRoom);

         Console.WriteLine("Standard : {0}", standard);
         Console.WriteLine("IQ Rate : {0}", iqRate);
         Console.WriteLine("Actual HeadRoom : {0}", actualHeadRoom);

         /*Close WLAN session*/
         wlanSession.Close();

         /*Configure Script*/
         niWLANG.WLANG_RFSGConfigureScript(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), null, script, powerLevel);
         rfsgSession.RF.OutputEnabled = true;

         /*Initiate Generation*/
         rfsgSession.Initiate();

         /*Check for successful generation*/
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
         rfsgSession.Utility.Commit();
         niWLANG.WLANG_RFSGClearDatabase(rfsgSession.GetInstrumentHandle().DangerousGetHandle(), "", null);
      }
   }
}
