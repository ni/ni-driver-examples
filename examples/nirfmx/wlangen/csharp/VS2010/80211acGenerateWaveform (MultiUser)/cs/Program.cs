using System;
using NationalInstruments.RFToolkits.Wlan.Generation;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsg.Internal;
using System.Runtime.InteropServices;


namespace _80211acGenerateWaveformMultiUser
{
   class Program
   {
      static void Main(string[] args)
      {
         GenerateeWaveformMultiUserExample example = new GenerateeWaveformMultiUserExample();
         example.Run();
      }
   }

   class GenerateeWaveformMultiUserExample
   {
      const int maxWlanChannels = 4;
      int numTx = 2;
      int numberOfUsers = 2;

      double carrierFrequency = 5.18e9;
      double channelBandwidth = 20e+6;
      double[] powerLevel = { -10.0, -10.0, -10.0, -10.0 };
      double[] externalAttenuation = { 0, 0, 0, 0 };

      NIRfsg[] rfsgSessions = new NIRfsg[maxWlanChannels];
      niWLANG wlanSession;
      NIRfsg externalLOSession = null;
      String[] rfsgResourceName = { "RIO0", "RIO1", "RIO2", "RIO3" };
      String rfsgClockSource = RfsgFrequencyReferenceSource.PxiClock;

      String waveformName = "Wlan";
      private int[] mCSIndex = new int[] { 0, 8 };
      private int[] numOfSpaceTimeStreams = { 1, 1 };
      int[] triggerLines = { 0, 1 };

      public void Run()
      {
         try
         {
            ConfigureWlanGenerationSession();
            ConfigureRfsgSession();
            CreateAndDownloadWaveform();
            StopGeneration();
         }
         catch (Exception e)
         {
            Console.WriteLine("Error : " + e.ToString());
            Console.WriteLine(" Press any key to exit");
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
         int mappingMatrixType = niWLANGConstants.MappingMatrixTypeDirect;

         if (wlanSession == null)
            wlanSession = new niWLANG("WLANG", niWLANGConstants.CompatibilityVersion060000, out isNewSession);

         wlanSession.SetStandard(null, niWLANGConstants.Standard80211AcMimoOfdm);
         wlanSession.SetNumberOfTransmitChannels(null, numTx);
         wlanSession.SetChannelBandwidth(null, channelBandwidth);
         wlanSession.SetMappingMatrixType(null, mappingMatrixType);
         wlanSession.SetPPDUType(null, niWLANGConstants.PpduTypeMuPpdu);
         wlanSession.SetNumberOfUsers(null, numberOfUsers);

         String activeChannel = "user";

         for (int i = 0; i < numTx; i++)
         {
            wlanSession.SetMCSIndex(activeChannel + i, mCSIndex[i]);
            wlanSession.SetNumberOfSpaceTimeStreams(activeChannel + i, numOfSpaceTimeStreams[i]);
         }

         wlanSession.SetRFBlankingEnabled(null, niWLANGConstants.True);
      }


      private void CreateAndDownloadWaveform()
      {
         double iqRate;
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


         for (int i = 0; i < numTx; i++)
         {
            niWLANG.WLANG_RFSGConfigureScript(rfsgSessions[i].GetInstrumentHandle().DangerousGetHandle(), null, script, powerLevel[i]);
         }
         wlanSession.RFSGMultipleDeviceInitiate(rfsgHandle);

         //Check for successful generation
         CheckGeneration();

         wlanSession.GetIqRate(String.Empty, out iqRate);
         Console.WriteLine("IQ Rate : {0}", iqRate);

         string channelString = "segment0/channel";
         for (int i = 0; i < numTx; i++)
         {
            wlanSession.GetActualHeadroom(channelString + i, out actualHeadRoom[i]);
            Console.WriteLine("Actual HeadRoom User: {0}", actualHeadRoom[i]);
         }
         Console.WriteLine("Press any key to exit");
         Console.ReadKey();
      }

      private void ConfigureRfsgSession()
      {
         for (int i = 0; i < numTx; i++)
         {
            if (rfsgSessions[i] == null)
            {
               rfsgSessions[i] = new NIRfsg(rfsgResourceName[i], true, false);
            }

            rfsgSessions[i].RF.PowerLevelType = RfsgRFPowerLevelType.PeakPower;
            rfsgSessions[i].Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform;
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
               niWLANG.WLANG_RFSGClearDatabase(rfsgSessions[i].GetInstrumentHandle().DangerousGetHandle(), "", null);
            }
         }
      }
   }
}
