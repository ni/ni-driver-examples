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
      const int NUMBER_OF_EXTERNAL_LO = 1;
      const int NUMBER_OF_SEGMENTS = 1;

      int standard, numTx;

      double segment0CarrierFrequency, segment1CarrierFrequency, channelBandwidth;
      String[] rfsgResourceName = new string[MAX_WLAN_CHANNELS] { "RIO0", "RIO1", "RIO2", "RIO3" };
      double[] powerLevel = new double[MAX_WLAN_CHANNELS] { -10.0, -10.0, -10.0, -10.0 };
      double[] externalAttenuation = new double[MAX_WLAN_CHANNELS] { 0, 0, 0, 0 };
      double[] carrierFrequencies = new double[NUMBER_OF_SEGMENTS];

      NIRfsg[] rfsgSessions = new NIRfsg[MAX_WLAN_CHANNELS];
      niWLANG wlanSession;
      String rfsgClockSource = RfsgFrequencyReferenceSource.PxiClock;
      int LOSource, rfsaLODaisyChainEnabled, LOExportToExternalDevicesEnabled;

      string[] externalLOResourceName = new string[NUMBER_OF_EXTERNAL_LO] { null };
      string[] externalLOReferenceClockSource = new string[NUMBER_OF_EXTERNAL_LO] { RfsgFrequencyReferenceSource.OnboardClock };
      NIRfsg[] externalLOSession;

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

         segment0CarrierFrequency = 5.21e+9;
         segment1CarrierFrequency = 5.53e+9;
         if (NUMBER_OF_SEGMENTS == 1)
         {
            carrierFrequencies = new double[1];
            carrierFrequencies[0] = segment0CarrierFrequency;
         }
         else
         {
            carrierFrequencies = new double[2];
            carrierFrequencies[0] = segment0CarrierFrequency;
            carrierFrequencies[1] = segment1CarrierFrequency;
         }
      }

      private void ConfigureWlanGenerationSession()
      {
         int isNewSession;
         if (wlanSession == null)
            wlanSession = new niWLANG("WLANG", niWLANGConstants.CompatibilityVersion060000, out isNewSession);
         wlanSession.SetStandard(null, standard);
         wlanSession.SetChannelBandwidth(null, channelBandwidth);
         wlanSession.SetNumberOfTransmitChannels(null, numTx);
         wlanSession.SetNumberOfSegments(null, NUMBER_OF_SEGMENTS);
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
         externalLOSession = new NIRfsg[NUMBER_OF_EXTERNAL_LO];
         if (externalLOResourceName[0] != null && LOSource == niWLANGConstants.LOSourceExternal && segment0CarrierFrequency > 3.2e+9)
         {
            for (int i = 0; i < NUMBER_OF_EXTERNAL_LO; i++)
            {
               externalLOSession[i] = new NIRfsg(externalLOResourceName[i], true, false);
               externalLOSession[i].Arb.GenerationMode = RfsgWaveformGenerationMode.ContinuousWave;
               externalLOSession[i].FrequencyReference.Configure(externalLOReferenceClockSource[i], 10.0e6);
            }
         }
      }

      private void CreateAndDownloadWaveform()
      {
         double iqRate, waveformDuration;
         int iqWaveformSize, k = 0;
         int rfsgArraySize = numTx * NUMBER_OF_SEGMENTS;
         string[] channelString = new string[30];
         double[] actualHeadRoom = new double[rfsgArraySize];
         String script = @"script GenerateWlan
				                repeat forever
					                generate Wlan
				                end repeat
			                end script";

         IntPtr[] rfsgHandle = new IntPtr[rfsgArraySize];
         IntPtr[] externalLOref = new IntPtr[NUMBER_OF_EXTERNAL_LO];

         for (int i = 0; i < NUMBER_OF_EXTERNAL_LO; i++)
         {
            if (externalLOSession[i] != null)
               externalLOref[i] = externalLOSession[i].GetInstrumentHandle().DangerousGetHandle();
         }
         for (int i = 0; i < rfsgArraySize; i++)
            rfsgHandle[i] = rfsgSessions[i].GetInstrumentHandle().DangerousGetHandle();

         wlanSession.ConfigureMultipleDeviceSynchronization(rfsgHandle, rfsgArraySize, rfsgClockSource, triggerLines, triggerLines.Length);
         wlanSession.RFSGConfigureFrequencyMultipleLO(rfsgHandle, LOSource,
             externalLOref, carrierFrequencies, carrierFrequencies.Length, rfsaLODaisyChainEnabled, LOExportToExternalDevicesEnabled);

         for (int i = 0; i < NUMBER_OF_EXTERNAL_LO; i++)
         {
            if (externalLOSession[i] != null)
               externalLOSession[i].Initiate();
         }
         wlanSession.RFSGCreateAndDownloadMIMOWaveforms(rfsgHandle, null, rfsgArraySize, waveformName);
         /*Configure Script*/
         for (int i = 0; i < numTx * NUMBER_OF_SEGMENTS; i++)
         {
            niWLANG.WLANG_RFSGConfigureScript(rfsgHandle[i], null, script, powerLevel[i]);
         }

         wlanSession.RFSGMultipleDeviceInitiate(rfsgHandle);

         //Check for successful generation
         CheckGeneration();
         if (standard == niWLANGConstants.Standard80211nMimoOfdm || standard == niWLANGConstants.Standard80211AhMimoOfdm ||
             standard == niWLANGConstants.Standard80211beMimoOfdm || standard == niWLANGConstants.Standard80211bnMimoOfdm)
         {
            for (int i = 0; i < numTx; i++)
            {
               channelString[i] = "Channel" + i;
            }
         }
         else
         {
            for (int i = 0; i < NUMBER_OF_SEGMENTS; i++)
            {
               for (int j = 0; j < numTx; j++)
               {
                  channelString[k++] = "Segment" + i + "/Channel" + j;
               }
            }
         }
         wlanSession.GetIqRate(String.Empty, out iqRate);
         wlanSession.GetIqWaveformSize(String.Empty, out iqWaveformSize);
         waveformDuration = iqWaveformSize / iqRate;

         Console.WriteLine("Waveform Duration {0}" + waveformDuration);
         Console.WriteLine("Actual Headroom (dB)");
         for (int i = 0; i < numTx * NUMBER_OF_SEGMENTS; i++)
         {
            wlanSession.GetActualHeadroom(channelString[i], out actualHeadRoom[i]);
            Console.WriteLine("\t" + actualHeadRoom[i]);
         }

         Console.WriteLine("Press any key to exit");
         Console.ReadKey();
      }

      private void ConfigureRfsgSession()
      {
         //Open all RFSG Sessions
         for (int i = 0; i < numTx * NUMBER_OF_SEGMENTS; i++)
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
         for (int i = 0; i < NUMBER_OF_EXTERNAL_LO; i++)
         {
            if (externalLOSession[i] != null)
            {
               externalLOSession[i].Abort();
               externalLOSession[i].RF.OutputEnabled = false;
               externalLOSession[i].Utility.Commit();
            }
         }
      }

   }
}
