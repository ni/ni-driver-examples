using System;
using NationalInstruments.ModularInstruments;
using NationalInstruments.RFToolkits.Wlan.Generation;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsg.Internal;
using NationalInstruments.ModularInstruments.SystemServices.TimingServices;


namespace _80211nacGenerateWaveformMultipleRFSG
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
      const int maxWlanChannels = 4;
      int numTx = 1;
      int standard = niWLANGConstants.Standard80211AcMimoOfdm;

      double carrierFrequency = 5.18e9;
      double channelBandwidth = 20e+6;
      double[] powerLevel = { -10.0, -10.0, -10.0, -10.0 };
      double[] externalAttenuation = { 0, 0, 0, 0 };

      NIRfsg[] rfsgSessions = new NIRfsg[maxWlanChannels];
      niWLANG wlanSession;
      String[] rfsgResourceName = { "RIO0", "RIO1", "RIO2", "RIO3" };
      String[] MIMOChannelString = {"channel0",
                           "channel1",
                           "channel2",
                           "channel3",
                           "channel4",
                           "channel5",
                           "channel6",
                           "channel7"};
      String rfsgClockSource = RfsgFrequencyReferenceSource.PxiClock;

      String waveformName = "Wlan";
      int mCSIndex = 0, DsssDataRate = niWLANGConstants.DsssDataRate1, OfdmDataRate = niWLANGConstants.OfdmDataRate6;
      int numOfSpaceTimeStreams = 1, mappingMatrixType = niWLANGConstants.MappingMatrixTypeDirect;
      int[] triggerLines = { 0, 1 };
      TClock tclock = null;
      ITClockSynchronizableDevice[] rfsgSynchronizableDevices;

      public void Run()
      {
         try
         {
            ConfigureWlanGenerationSession();
            ConfigureRfsgSession();
            CreateAndDownloadWaveform();
            bool isDone = CheckGeneration();
            if (isDone == false)
            {
               Console.WriteLine("Generating Signal Waveform");
               Console.WriteLine("Press any key to abort generation");
               Console.ReadKey();
               StopGeneration();
            }
            else
            {
               Console.WriteLine("Generation Error");
               Console.ReadKey();
            }
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

      private void ConfigureWlanGenerationSession()
      {
         int isNewSession;

         if (wlanSession == null)
            wlanSession = new niWLANG("WLANG", niWLANGConstants.CompatibilityVersion060000, out isNewSession);

         if (standard == niWLANGConstants.Standard80211bgDsss)
         {
            wlanSession.SetStandard(null, standard);
            wlanSession.SetNumberOfTransmitChannels(null, numTx);
            wlanSession.SetDsssDataRate(null, DsssDataRate);
         }
         if (standard == niWLANGConstants.Standard80211bgDsss)
         {
            wlanSession.SetStandard(null, standard);
            wlanSession.SetChannelBandwidth(null, channelBandwidth);
            wlanSession.SetNumberOfTransmitChannels(null, numTx);
            wlanSession.SetMcsIndex(null, mCSIndex);
            wlanSession.SetMappingMatrixType(null, mappingMatrixType);
         }
         if (standard == niWLANGConstants.Standard80211nMimoOfdm)
         {
            wlanSession.SetStandard(null, standard);
            wlanSession.SetChannelBandwidth(null, channelBandwidth);
            wlanSession.SetNumberOfTransmitChannels(null, numTx);
            wlanSession.SetMcsIndex(null, mCSIndex);
            wlanSession.SetMappingMatrixType(null, mappingMatrixType);
         }
         if (standard == niWLANGConstants.Standard80211AcMimoOfdm || standard == niWLANGConstants.Standard80211AhMimoOfdm || standard == niWLANGConstants.Standard80211AfMimoOfdm || standard == niWLANGConstants.Standard80211AxMimoOfdm || standard == niWLANGConstants.Standard80211beMimoOfdm || standard == niWLANGConstants.Standard80211bnMimoOfdm)
         {
            wlanSession.SetStandard(null, standard);
            wlanSession.SetChannelBandwidth(null, channelBandwidth);
            wlanSession.SetNumberOfTransmitChannels(null, numTx);
            wlanSession.SetMcsIndex(null, mCSIndex);
            wlanSession.SetNumberOfSpaceTimeStreams(null, numOfSpaceTimeStreams);
            wlanSession.SetMappingMatrixType(null, mappingMatrixType);
         }
         else
         {
            wlanSession.SetStandard(null, standard);
            wlanSession.SetChannelBandwidth(null, channelBandwidth);
            wlanSession.SetOfdmDataRate(null, OfdmDataRate);
            wlanSession.SetNumberOfTransmitChannels(null, numTx);
         }
         wlanSession.SetLOSharingEnabled(null, niWLANGConstants.False);
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
         for (int i = 0; i < numTx; i++)
            rfsgHandle[i] = rfsgSessions[i].GetInstrumentHandle().DangerousGetHandle();

         wlanSession.ConfigureMultipleDeviceSynchronization(rfsgHandle, numTx, rfsgClockSource, triggerLines, triggerLines.Length);
         wlanSession.RFSGCreateAndDownloadMIMOWaveforms(rfsgHandle, null, numTx, waveformName);

         wlanSession.GetIqRate(String.Empty, out iqRate);
         wlanSession.GetIqWaveformSize(String.Empty, out iqWaveformSize);
         waveformDuration = iqWaveformSize / iqRate;

         Console.WriteLine("Waveform Duration : {0}", waveformDuration);
         if (standard == niWLANGConstants.Standard80211nMimoOfdm || standard == niWLANGConstants.Standard80211AcMimoOfdm || standard == niWLANGConstants.Standard80211AhMimoOfdm || standard == niWLANGConstants.Standard80211AfMimoOfdm || standard == niWLANGConstants.Standard80211AxMimoOfdm || standard == niWLANGConstants.Standard80211beMimoOfdm || standard == niWLANGConstants.Standard80211bnMimoOfdm)
         {
            for (int i = 0; i < numTx; i++)
            {
               wlanSession.GetActualHeadroom(MIMOChannelString[i], out actualHeadRoom[i]);
               Console.WriteLine("Actual HeadRoom for {0} : {1}", MIMOChannelString[i], actualHeadRoom[i]);
            }
         }
         else
         {
            for (int i = 0; i < numTx; i++)
            {
               wlanSession.GetActualHeadroom(null, out actualHeadRoom[i]);
            }
         }

         /*Configure Script*/
         for (int i = 0; i < numTx; i++)
         {
            niWLANG.WLANG_RFSGConfigureScript(rfsgHandle[i], null, script, powerLevel[i]);
         }

         String instrumentModel = rfsgSessions[0].Identity.InstrumentModel;

         if (String.Equals(instrumentModel, "NI PXIe-5644R", StringComparison.OrdinalIgnoreCase) ||
             String.Equals(instrumentModel, "NI PXIe-5645R", StringComparison.OrdinalIgnoreCase) ||
             String.Equals(instrumentModel, "NI PXIe-5646R", StringComparison.OrdinalIgnoreCase))
         {
            rfsgSessions[0].Utility.Commit();
            for (int i = 1; i < numTx; i++)
            {
               rfsgSessions[i].Initiate();
            }

            rfsgSessions[0].Initiate();
         }
         else
         {
            /*Initiate Generation*/
            if (numTx == 1)
            {
               rfsgSessions[0].Initiate();
            }
            else
            {
               rfsgSynchronizableDevices = new ITClockSynchronizableDevice[rfsgSessions.Length];

               for (int i = 0; i < numTx; i++)
               {
                  rfsgSynchronizableDevices[i] = (ITClockSynchronizableDevice)rfsgSessions[i];
               }               

               tclock = new TClock(rfsgSynchronizableDevices);
               tclock.ConfigureForHomogeneousTriggers();
               tclock.Synchronize();
               tclock.Initiate();
            }
         }
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
            rfsgSessions[i].RF.Frequency = carrierFrequency;
            rfsgSessions[i].RF.ExternalGain = -(externalAttenuation[i]);
         }
      }

      private bool CheckGeneration()
      {
         RfsgGenerationStatus status = RfsgGenerationStatus.InProgress;
         String instrumentModel = rfsgSessions[0].Identity.InstrumentModel;
         if (String.Equals(instrumentModel, "NI PXIe-5644R", StringComparison.OrdinalIgnoreCase) ||
             String.Equals(instrumentModel, "NI PXIe-5645R", StringComparison.OrdinalIgnoreCase) ||
             String.Equals(instrumentModel, "NI PXIe-5646R", StringComparison.OrdinalIgnoreCase))
         {
            for (int i = 0; i < numTx; i++)
            {
               /*Check Generation Status*/
               status = rfsgSessions[i].CheckGenerationStatus();
               if (status == RfsgGenerationStatus.Complete)
                  break;
            }
            return Convert.ToBoolean(status);
         }
         else
         {
            /*Check Generation Status*/
            if (numTx == 1)
            {
               status = rfsgSessions[0].CheckGenerationStatus();
               return Convert.ToBoolean(status);
            }
            else
            {
               bool isDone = tclock.IsDone;
               return isDone;
            }   
         }      
      }

      private void StopGeneration()
      {
         for (int i = 0; i < numTx; i++)
         {
            if (rfsgSessions[i] != null)
            {
               rfsgSessions[i].Abort();
               rfsgSessions[i].RF.OutputEnabled = false;
               niWLANG.WLANG_RFSGClearDatabase(rfsgSessions[i].GetInstrumentHandle().DangerousGetHandle(), "", null);
            }
         }
      }
   }
}
