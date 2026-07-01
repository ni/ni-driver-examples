/* Comments:
 * Steps
1.
     A. Compute number of generators as the number of segments times the number of transmit channels (N_tx). 
     B. Create an array of carrier frequencies with segment 0 carrier frequency, segment 1 carrier frequency if the    Number of Segments is equal to 2. otherwise, use the segment 0 carrier frequency.
2. For each generator,
     A. Open an NI-RFSG session.
     B. Configure basic NI-RFSG properties. Set Generation Mode to Script and Power Level Type to Peak Power. 
3. Open an NI WLAN Generation session.
4. Configure basic WLAN generation properties. 
   - Set Standard to 80211BE MIMOOFDM or 80211BN MIMOOFDM.
5. Set headroom for each channel in each segment.
6. Configures advanced WLANG properties and user specific properties.
7. Configure payload and spectrum control properties.
8. Enable RF blanking to attenuate the RF OUT signal during the idle interval. 
9. Synchronize the generators. Configure the reference clock source for the master (the first generator) and share that with slaves. 
10. Configure frequency on the generators.
11. Create the waveform and download it to the NI RF vector signal generators memory.
12. For each generator, configure NI-RFSG for the waveform mentioned in the script.
13.  Initiate signal generation by calling niWLANG RFSG Multiple Device Initiate VI.
14. 
    A. Check the generation status.
    B. Exit if an error has occurred, the Stop button is pressed or generation is complete.
15. For each generator,
    A. Abort signal generation.
    B. Disable the output. This sets the noise floor as low as possible.
    C. Clear the waveform from the device memory. Clear the waveform properties from RFSG database.
    D. Close the NI-RFSG session.
16. Read signal properties (waveform size, actual headroom and packet extension duration) for display purpose.
17. Close the NI WLAN Generation session. 
*/

using System;
using NationalInstruments.RFToolkits.Wlan.Generation;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsg.Internal;
using System.Runtime.InteropServices;


namespace _80211bebnGenerateWaveformMultiUser
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
      const int MAX_WLAN_CHANNELS = 8;
      int numTx, mappingMatrixType, PPDUType, numberOfUsers, guardIntervalType;
      int SwapIandQEnabled, SigCompression, LtfSize, OverSamplingFactor, AutoHeadroomEnabled;
      int NumberOfFrames, TransmissionMode, NominalPacketPadding, NumSeg;
      int PreamblePuncturingEnabled, Primary20MhzChannelIndex, PreamblePuncturingMask;
      int PulseShapingFilterEnabled, FilterType, FilterLength, OfdmWindowLength, WindowingMethod, RuAllocationMode;

      double[] carrierFrequency = { 5.18e+9 };
      double channelBandwidth;
      double FilterParameter, SampleClockRateFactor, IdleInterval;

      double[] powerLevel = new double[MAX_WLAN_CHANNELS] { -10.0, -10.0, -10.0, -10.0, -10.0, -10.0, -10.0, -10.0 };
      double[] externalAttenuation = new double[MAX_WLAN_CHANNELS] { 0, 0, 0, 0, 0, 0, 0, 0 };
      double[] Headroom = new double[MAX_WLAN_CHANNELS] { 12, 12, 12, 12, 12, 12, 12, 12 };


      int[] RuSize = {niWLANGConstants.RuSize26,niWLANGConstants.RuSize26,niWLANGConstants.RuSize52,
                        niWLANGConstants.RuSize26,niWLANGConstants.RuSize106};
      int[] RuOffset = { 0, 1, 2, 4, 5 };
      int[] McsIndex = { 0, 0, 0, 0, 0 };
      int[] numberOfSpaceTimeStream = { 1, 1, 1, 1, 1 };
      int[] payloadLength = { 100, 100, 100, 100, 100 };
      int[] RuAllocation = { 80 };
      int[] UserEnabled = { niWLANGConstants.True, niWLANGConstants.True, niWLANGConstants.True, niWLANGConstants.True, niWLANGConstants.True };
      int[] DcmEnabled = { niWLANGConstants.False, niWLANGConstants.False, niWLANGConstants.False, niWLANGConstants.False, niWLANGConstants.False };
      int[] StaId = { 0, 1, 2, 3, 4 };
      int[] PowerBoostFactor = { 1, 1, 1, 1, 1 };
      int[] FecCodingType = {niWLANGConstants.FecCodingTypeLdpc, niWLANGConstants.FecCodingTypeLdpc, niWLANGConstants.FecCodingTypeLdpc,
                               niWLANGConstants.FecCodingTypeLdpc, niWLANGConstants.FecCodingTypeLdpc};

      NIRfsg[] rfsgSessions = new NIRfsg[MAX_WLAN_CHANNELS];
      niWLANG wlanSession;
      NIRfsg[] externalLOSession = new NIRfsg[MAX_WLAN_CHANNELS] { null, null, null, null, null, null, null, null };
      String[] rfsgResourceName = new string[MAX_WLAN_CHANNELS] { "RIO0", "RIO1", "RIO2", "RIO3", "RIO4", "RIO5", "RIO6", "RIO7" };
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
         NumSeg = 1;
         SwapIandQEnabled = niWLANGConstants.False;
         SampleClockRateFactor = 1;
         SigCompression = niWLANGConstants.SigCompressionEnabled;
         LtfSize = niWLANGConstants.LtfSizeAuto;
         OverSamplingFactor = 4;
         AutoHeadroomEnabled = niWLANGConstants.True;
         NumberOfFrames = 1;
         TransmissionMode = niWLANGConstants.TransmissionModeDownlinl;
         NominalPacketPadding = niWLANGConstants.MaximumPaddingDurationAuto;
         PreamblePuncturingEnabled = niWLANGConstants.False;
         Primary20MhzChannelIndex = 0;
         PreamblePuncturingMask = 0xFFFF;
         PulseShapingFilterEnabled = niWLANGConstants.False;
         FilterType = niWLANGConstants.FilterRectangular;
         FilterLength = 8;
         OfdmWindowLength = 2;
         WindowingMethod = niWLANGConstants.WinMethodCenteredAtSymbolBoundary;
         RuAllocationMode = niWLANGConstants.RUAllocationModeIndividual;
         FilterParameter = 0.5;
         SampleClockRateFactor = 1;
         IdleInterval = 100e-6;
      }

      private void ConfigureWlanGenerationSession()
      {
         int isNewSession;

         if (wlanSession == null)
            wlanSession = new niWLANG("WLANG", niWLANGConstants.CompatibilityVersion060000, out isNewSession);

         wlanSession.SetStandard(null, niWLANGConstants.Standard80211beMimoOfdm);
         wlanSession.SetChannelBandwidth(null, channelBandwidth);
         wlanSession.SetOversamplingFactor(null, OverSamplingFactor);
         wlanSession.SetIdleInterval(null, IdleInterval);
         wlanSession.SetNumberOfFrames(null, NumberOfFrames);
         wlanSession.SetAutoHeadroomEnabled(null, AutoHeadroomEnabled);
         wlanSession.SetLtfSize(null, LtfSize);
         wlanSession.SetSigCompressionEnabled(null, SigCompression);
         wlanSession.SetOFDMGuardIntervalType(null, guardIntervalType);

         for (int i = 0; i < (numTx * NumSeg); i++)
         {
            channelString = "channel" + i;
            wlanSession.SetHeadroom(channelString, Headroom[i]);
         }

         wlanSession.SetNumberOfTransmitChannels(null, numTx);
         wlanSession.SetMappingMatrixType(null, mappingMatrixType);
         wlanSession.SetNumberOfSegments(null, NumSeg);
         wlanSession.SetNumberOfUsers(null, numberOfUsers);
         wlanSession.SetTransmissionMode(null, TransmissionMode);
         wlanSession.SetPreamblePuncturingEnabled(null, PreamblePuncturingEnabled);
         wlanSession.SetPrimary20MHzChannelIndex(null, Primary20MhzChannelIndex);
         wlanSession.SetPreamblePuncturingMask(null, PreamblePuncturingMask);
         wlanSession.SetNominalPacketPadding(null, NominalPacketPadding);

         if (PPDUType == niWLANGConstants.PpduTypeSuPpdu)
         {
            wlanSession.SetPPDUType(null, niWLANGConstants.PpduTypeSuPpdu);
            wlanSession.SetMCSIndex(null, McsIndex[0]);
            wlanSession.SetNumberOfSpaceTimeStreams(null, numberOfSpaceTimeStream[0]);
            wlanSession.SetDualCarrierModulationEnabled(null, DcmEnabled[0]);
            wlanSession.SetFecCodingType(null, FecCodingType[0]);
            wlanSession.SetPayloadDataLength("mpdu0", payloadLength[0]);
         }
         else
         {
            wlanSession.SetPPDUType(null, niWLANGConstants.PpduTypeMuPpdu);
            wlanSession.SetRUAllocationMode(null, RuAllocationMode);
            wlanSession.SetRUAllocation(null, RuAllocation, RuAllocation.Length);

            if (RuAllocationMode == niWLANGConstants.RUAllocationModeIndividual)
            {
               for (int i = 0; i < numberOfUsers; i++)
               {
                  channelString = "user" + i;
                  wlanSession.SetRUSize(channelString, RuSize[i]);
                  wlanSession.SetRUOffsetMruIndex(channelString, RuOffset[i]);
               }
            }
            else
            {
               wlanSession.GetNumberOfUsersFromRUAllocation(null, out numberOfUsers);
            }

            for (int i = 0; i < numberOfUsers; i++)
            {
               channelString = "user" + i;
               wlanSession.SetMCSIndex(channelString, McsIndex[i]);
               wlanSession.SetNumberOfSpaceTimeStreams(channelString, numberOfSpaceTimeStream[i]);
               wlanSession.SetSTAID(channelString, StaId[i]);
               wlanSession.SetDualCarrierModulationEnabled(channelString, DcmEnabled[i]);
               wlanSession.SetFecCodingType(channelString, FecCodingType[i]);
               wlanSession.SetPowerBoostFactor(channelString, PowerBoostFactor[i]);
               wlanSession.SetUserEnabled(channelString, UserEnabled[i]);
               channelString = channelString + "/mpdu0";
               wlanSession.SetPayloadDataLength(channelString, payloadLength[i]);
            }
         }

         wlanSession.SetPulseShapingFilterEnabled(null, PulseShapingFilterEnabled);
         wlanSession.SetPulseShapingFilterType(null, FilterType);
         wlanSession.SetPulseShapingFilterParameter(null, FilterParameter);
         wlanSession.SetOfdmWindowLength(null, OfdmWindowLength);
         wlanSession.SetWindowingMethod(null, WindowingMethod);
         wlanSession.SetPulseShapingFilterLength(null, FilterLength);
         wlanSession.SetSwapIAndQEnabled(null, SwapIandQEnabled);
         wlanSession.SetSampleClockRateFactor(null, SampleClockRateFactor);

         wlanSession.SetRFBlankingEnabled(null, niWLANGConstants.True);
      }

      private void CreateAndDownloadWaveform()
      {
         double iqRate, waveformDuration;
         double PacketExtensionDuration;
         int iqWaveformSize;
         double[] actualHeadRoom = new double[numTx];
         String script = @"script GenerateWlan
				                repeat forever
					                generate Wlan
				                end repeat
			                end script";

         IntPtr[] rfsgHandle = new IntPtr[numTx];
         IntPtr[] externalLOHandle = new IntPtr[MAX_WLAN_CHANNELS];

         for (int i = 0; i < numTx; i++)
         {
            rfsgHandle[i] = rfsgSessions[i].GetInstrumentHandle().DangerousGetHandle();
            //if (externalLOSession[i] != null)
            //   externalLOHandle[i] = externalLOSession[i].GetInstrumentHandle().DangerousGetHandle();
         }       

         wlanSession.ConfigureMultipleDeviceSynchronization(rfsgHandle, (numTx * NumSeg), rfsgClockSource, triggerLines, triggerLines.Length);
         wlanSession.RFSGConfigureFrequencyMultipleLO(rfsgHandle, niWLANGConstants.LOSourceOnboard, externalLOHandle, carrierFrequency, carrierFrequency.Length, niWLANGConstants.False, niWLANGConstants.False);

         wlanSession.RFSGCreateAndDownloadMIMOWaveforms(rfsgHandle, null, numTx, waveformName);
         /*Configure Script*/
         for (int i = 0; i < (numTx * NumSeg); i++)
         {
            niWLANG.WLANG_RFSGConfigureScript(rfsgHandle[i], null, script, powerLevel[i]);
         }
         wlanSession.RFSGMultipleDeviceInitiate(rfsgHandle);

         //Check for successful generation
         CheckGeneration();

         wlanSession.GetIqRate(String.Empty, out iqRate);
         wlanSession.GetIqWaveformSize(String.Empty, out iqWaveformSize);
         waveformDuration = iqWaveformSize / iqRate;
         wlanSession.GetPacketExtensionDuration(String.Empty, out PacketExtensionDuration);

         Console.WriteLine("Waveform Duration{s} : " + waveformDuration);
         Console.WriteLine("Packet Extension Duration{s} : " + PacketExtensionDuration);

         for (int i = 0; i < numTx; i++)
         {
            channelString = "Channel" + i;
            wlanSession.GetActualHeadroom(channelString, out actualHeadRoom[i]);

         }

         Console.WriteLine("Press any key to exit");
         Console.ReadKey();
      }

      private void ConfigureRfsgSession()
      {
         //Open all RFSG Sessions
         for (int i = 0; i < (numTx * NumSeg); i++)
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
