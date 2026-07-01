using System;
using NationalInstruments.RFToolkits.Wlan.Generation;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsg.Internal;
using System.Runtime.InteropServices;


namespace _80211GenerateMIMOOfdmWaveformMultipleRFSGWithExtLO
{
   class Program
   {
      static void Main(string[] args)
      {
         GenerateWaveformMultipleRFSGExample example = new GenerateWaveformMultipleRFSGExample();
         example.Run();
      }
   }

   class GenerateWaveformMultipleRFSGExample
   {

      const int MAX_WLAN_CHANNELS = 4;
      int standard, numTx;

      double carrierFrequency;
      double channelBandwidth;
      String[] rfsgResourceName = new string[MAX_WLAN_CHANNELS] { "RIO0", "RIO1", "RIO2", "RIO3" };
      double[] powerLevel = new double[MAX_WLAN_CHANNELS] { -10.0, -10.0, -10.0, -10.0 };
      double[] externalAttenuation = new double[MAX_WLAN_CHANNELS] { 0, 0, 0, 0 };

      NIRfsg[] rfsgSessions = new NIRfsg[MAX_WLAN_CHANNELS];
      niWLANG wlanSession;
      String rfsgClockSource = RfsgFrequencyReferenceSource.PxiClock;
      int LOSource, rfsaLODaisyChainEnabled, LOExportToExternalDevicesEnabled;

      string externalLOResourceName = null;
      string externalLOReferenceClockSource = RfsgFrequencyReferenceSource.OnboardClock;
      NIRfsg externalLOSession = null;

      String waveformName = "Wlan";
      int mCSIndex, frameFormat, ahPreambleType;
      int numOfSpaceTimeStreams, mappingMatrixType;
      int[] triggerLines = { 0, 1 };

      public void Run()
      {
         try
         {
            initGlobalVaribales();
            ConfigureWlanGenerationSession();
            ConfigureRfsgSession();
            configureLOSession();
            CreateAndDownloadWaveform();
            StopGeneration();
            CloseexternalLOSessions();
         }
         catch (Exception e)
         {
            Console.WriteLine("Error : " + e.ToString());
            Console.ReadKey();
         }
         finally
         {
            for (int i = 0; i < numTx; i++)
            {
               if (rfsgSessions[i] != null)
                  rfsgSessions[i].Close();
            }

            if (wlanSession != null)
               wlanSession.Close();
         }
      }

      private void initGlobalVaribales()
      {
         standard = niWLANGConstants.Standard80211AxMimoOfdm;
         channelBandwidth = 80e+6;
         numTx = 1;
         mCSIndex = 0;
         mappingMatrixType = niWLANGConstants.MappingMatrixTypeDirect;
         numOfSpaceTimeStreams = 1;
         frameFormat = niWLANGConstants._80211nPlcpFrameFormatMixed;
         ahPreambleType = niWLANGConstants.PreambleTypeShortPreamble;
         LOSource = niWLANGConstants.LOSourceExternal;
         rfsaLODaisyChainEnabled = niWLANGConstants.False;
         LOExportToExternalDevicesEnabled = niWLANGConstants.False;
         carrierFrequency = 5.18e+9;
      }

      private void ConfigureWlanGenerationSession()
      {
         int isNewSession;
         if (wlanSession == null)
            wlanSession = new niWLANG("WLANG", niWLANGConstants.CompatibilityVersion060000, out isNewSession);
         wlanSession.SetStandard(null, standard);
         wlanSession.SetChannelBandwidth(null, channelBandwidth);
         wlanSession.SetNumberOfTransmitChannels(null, numTx);
         wlanSession.SetMcsIndex(null, mCSIndex);
         wlanSession.SetMappingMatrixType(null, mappingMatrixType);
         wlanSession.Set_80211nPlcpFrameFormat(null, frameFormat);
         wlanSession.SetNumberOfSpaceTimeStreams(null, numOfSpaceTimeStreams);
         wlanSession.Set_80211ahPreambleType(null, ahPreambleType);
         wlanSession.SetLOFrequencyOffsetMode(null, niWLANGConstants.LOFrequencyOffsetModeAuto);

         wlanSession.SetRFBlankingEnabled(null, niWLANGConstants.True);
      }

      private void configureLOSession()
      {
         if (externalLOResourceName != null && LOSource == niWLANGConstants.LOSourceExternal && carrierFrequency > 3.2e+9)
         {
            externalLOSession = new NIRfsg(externalLOResourceName, true, false);
            externalLOSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave;
            externalLOSession.FrequencyReference.Configure(externalLOReferenceClockSource, 10.0e6);
         }
      }

      private void CreateAndDownloadWaveform()
      {
         double iqRate, waveformDuration;
         int iqWaveformSize;
         string channelString;
         double[] actualHeadRoom = new double[numTx];
         String script = @"script GenerateWlan
				                repeat forever
					                generate Wlan
				                end repeat
			                end script";

         IntPtr[] rfsgHandle = new IntPtr[numTx];
         IntPtr externalLOref = new IntPtr();
         if (externalLOSession != null)
            externalLOref = externalLOSession.GetInstrumentHandle().DangerousGetHandle();

         for (int i = 0; i < numTx; i++)
            rfsgHandle[i] = rfsgSessions[i].GetInstrumentHandle().DangerousGetHandle();

         wlanSession.ConfigureMultipleDeviceSynchronization(rfsgHandle, numTx, rfsgClockSource, triggerLines, triggerLines.Length);
         wlanSession.RFSGConfigureFrequencySingleLO(rfsgHandle, LOSource, externalLOref, carrierFrequency, rfsaLODaisyChainEnabled, LOExportToExternalDevicesEnabled);

         if (externalLOSession != null)
            externalLOSession.Initiate();

         wlanSession.RFSGCreateAndDownloadMIMOWaveforms(rfsgHandle, null, numTx, waveformName);


         /*Configure Script*/
         for (int i = 0; i < numTx; i++)
         {
            niWLANG.WLANG_RFSGConfigureScript(rfsgHandle[i], null, script, powerLevel[i]);
         }
         wlanSession.RFSGMultipleDeviceInitiate(rfsgHandle);

         //Check for successful generation
         CheckGeneration();

         wlanSession.GetIqRate(String.Empty, out iqRate);
         wlanSession.GetIqWaveformSize(String.Empty, out iqWaveformSize);
         waveformDuration = iqWaveformSize / iqRate;

         Console.WriteLine("Waveform Duration {s}" + waveformDuration);
         Console.WriteLine("Actual Headroom (dB)");
         for (int i = 0; i < numTx; i++)
         {
            channelString = "Channel" + i;
            wlanSession.GetActualHeadroom(channelString, out actualHeadRoom[i]);
            Console.WriteLine("\t" + actualHeadRoom[i]);
         }
         Console.WriteLine("Press any key to exit");
         Console.ReadKey();
      }

      private void ConfigureRfsgSession()
      {
         //Open all RFSG Sessions
         for (int i = 0; i < numTx; i++)
         {
            if (rfsgSessions[i] == null)
            {
               rfsgSessions[i] = new NIRfsg(rfsgResourceName[i], true, false);
            }
            rfsgSessions[i].RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
            rfsgSessions[i].Arb.GenerationMode = RfsgWaveformGenerationMode.Script;
            rfsgSessions[i].RF.ExternalGain = -(externalAttenuation[i]);
         }
      }

      private bool CheckGeneration()
      {
         RfsgGenerationStatus status = RfsgGenerationStatus.InProgress;
         for (int i = 0; i < numTx; i++)
         {
            status = rfsgSessions[i].CheckGenerationStatus();
            if (status == RfsgGenerationStatus.Complete)
               break;
         }
         return Convert.ToBoolean(status);
      }

      private void StopGeneration()
      {
         for (int i = 0; i < numTx; i++)
         {
            if (rfsgSessions[i] != null)
            {
               rfsgSessions[i].Abort();
               rfsgSessions[i].RF.OutputEnabled = false;
               rfsgSessions[i].Utility.Commit();
               niWLANG.WLANG_RFSGClearDatabase(rfsgSessions[i].GetInstrumentHandle().DangerousGetHandle(), "", waveformName);
            }
         }
      }

      private void CloseexternalLOSessions()
      {
         if (externalLOSession != null)
         {
            externalLOSession.Abort();
            externalLOSession.RF.OutputEnabled = false;
            externalLOSession.Utility.Commit();
         }
      }

   }
}
