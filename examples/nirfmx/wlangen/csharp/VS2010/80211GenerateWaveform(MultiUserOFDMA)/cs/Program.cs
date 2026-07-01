using System;
using NationalInstruments.RFToolkits.Wlan.Generation;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsg.Internal;
using System.Runtime.InteropServices;


namespace _80211GenerateWaveformMultiUserOFDMA
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
      int numTx, mappingMatrixType, PPDUType, numberOfUsers, guardIntervalType;

      double carrierFrequency;
      double channelBandwidth;
      double[] powerLevel = new double[MAX_WLAN_CHANNELS] { -10.0, -10.0, -10.0, -10.0 };
      double[] externalAttenuation = new double[MAX_WLAN_CHANNELS] { 0, 0, 0, 0 };

      int[] RuSize = {niWLANGConstants.RuSize26,niWLANGConstants.RuSize26,niWLANGConstants.RuSize52,
                        niWLANGConstants.RuSize26,niWLANGConstants.RuSize106};
      int[] RuOffsetMruIndex = { 0, 1, 2, 4, 5 };
      int[] McsIndex = { 0, 0, 0, 0, 0 };
      int[] numberOfSpaceTimeStream = { 1, 1, 1, 1, 1 };
      int[] payloadLength = { 100, 100, 100, 100, 100 };

      NIRfsg[] rfsgSessions = new NIRfsg[MAX_WLAN_CHANNELS];
      niWLANG wlanSession;
      NIRfsg externalLOSession = null;
      String[] rfsgResourceName = new string[MAX_WLAN_CHANNELS] { "RIO0", "RIO1", "RIO2", "RIO3" };
      String rfsgClockSource = RfsgFrequencyReferenceSource.PxiClock;

      String waveformName = "Wlan";
      String channelString;
      int[] triggerLines = { 0, 1 };

      public void Run()
      {
         try
         {
            initGlobalVaribales();
            ConfigureWlanGenerationSession();
            ConfigureRfsgSession();
            CreateAndDownloadWaveform();
            StopGeneration();
         }
         catch (Exception e)
         {
            Console.WriteLine("Error : " + e.ToString());
            Console.WriteLine("press any key to exit");
            Console.ReadKey();
         }
         finally
         {
            if (wlanSession != null)
               wlanSession.Close();
         }
      }

      private void initGlobalVaribales()
      {
         channelBandwidth = 20e+6;
         numTx = 1;
         mappingMatrixType = niWLANGConstants.MappingMatrixTypeDirect;
         PPDUType = niWLANGConstants.PpduTypeMuPpdu;
         numberOfUsers = 5;
         guardIntervalType = niWLANGConstants.GuardIntervalTypeOneByFour;

         carrierFrequency = 5.18e+9;
      }

      private void ConfigureWlanGenerationSession()
      {
         int isNewSession;

         if (wlanSession == null)
            wlanSession = new niWLANG("WLANG", niWLANGConstants.CompatibilityVersion060000, out isNewSession);

         wlanSession.SetStandard(null, niWLANGConstants.Standard80211AxMimoOfdm);
         wlanSession.SetChannelBandwidth(null, channelBandwidth);
         wlanSession.SetNumberOfTransmitChannels(null, numTx);
         wlanSession.SetMappingMatrixType(null, mappingMatrixType);
         wlanSession.SetPPDUType(null, PPDUType);
         wlanSession.SetNumberOfUsers(null, numberOfUsers);
         wlanSession.SetOFDMGuardIntervalType(null, guardIntervalType);

         for (int i = 0; i < numberOfUsers; i++)
         {
            channelString = "user" + i;
            wlanSession.SetRUSize(channelString, RuSize[i]);
            wlanSession.SetRUOffsetMruIndex(channelString, RuOffsetMruIndex[i]);
            wlanSession.SetMcsIndex(channelString, McsIndex[i]);
            wlanSession.SetNumberOfSpaceTimeStreams(channelString, numberOfSpaceTimeStream[i]);
            channelString = channelString + "/mpdu0";
            wlanSession.SetPayloadDataLength(channelString, payloadLength[i]);
         }

         wlanSession.SetRFBlankingEnabled(null, niWLANGConstants.True);
      }

      private void CreateAndDownloadWaveform()
      {
         double iqRate, waveformDuration;
         int iqWaveformSize;
         double[] actualHeadRoom = new double[numTx];
         String script = @"script GenerateWlan
				                repeat forever
					                generate Wlan
				                end repeat
			                end script";

         IntPtr[] rfsgHandle = new IntPtr[numTx];
         IntPtr externalLOHandle = new IntPtr();

         for (int i = 0; i < numTx; i++)
            rfsgHandle[i] = rfsgSessions[i].GetInstrumentHandle().DangerousGetHandle();
         if (externalLOSession != null)
            externalLOHandle = externalLOSession.GetInstrumentHandle().DangerousGetHandle();

         wlanSession.ConfigureMultipleDeviceSynchronization(rfsgHandle, numTx, rfsgClockSource, triggerLines, triggerLines.Length);
         wlanSession.RFSGConfigureFrequencySingleLO(rfsgHandle, niWLANGConstants.LOSourceOnboard, externalLOHandle, carrierFrequency, niWLANGConstants.False, niWLANGConstants.False);

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

         Console.WriteLine("Waveform Duration : {s}" + waveformDuration);
         Console.WriteLine("Actual HeadRoom\n");
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
               rfsgSessions[i] = new NIRfsg(rfsgResourceName[i], false, true);
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
               rfsgSessions[i].Close();
            }
         }
      }
   }
}
